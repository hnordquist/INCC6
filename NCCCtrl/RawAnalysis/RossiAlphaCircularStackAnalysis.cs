/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, 
such modified software should be clearly marked, so as not to confuse it with the version available from LANL.

Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:
•	Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
disclaimer. 
•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
disclaimer in the documentation and/or other materials provided with the distribution. 
•	Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, 
nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
prior written permission. 
THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.ComponentModel;
using System.Threading;
using AnalysisDefs;

namespace LMRawAnalysis
{
    sealed public class RossiAlphaCircularNeutronEvent
    {
        public UInt64 eventTime;
        public UInt32 eventNeutrons;  //by bits
        public UInt32 numNeutrons;
        public RossiAlphaCircularNeutronEvent next;
        public int serialNumber;

        public RossiAlphaCircularNeutronEvent(int identifyingNumber)
        {
            //initialize the linked-list information
            next = null;

            //store the identifier for this structure
            serialNumber = identifyingNumber;
        }

        public void FillEventWithData(UInt64 time, UInt32 neutrons, UInt32 theNumNeutrons)
        {
            //store the time and the 32 neutron bits
            eventTime = time;
            eventNeutrons = neutrons;
            numNeutrons = theNumNeutrons;
        }
    }

    /// <summary>
    /// RossiAlphaAnalysis is a type of temporal histogram across a big-delta-time window
    /// containing 1000 small-delta-time subwindows or bins.
    /// To do RossiAlpha analysis in real time when large numbers of neutron events 
    /// are possible (e.g, always) then what is needed is a running queue across the 
    /// window with new events pushed onto the top of this stack with expiring events 
    /// popped off the bottom of this stack.
    /// That is a formidiable memory-management problem with substantial risks for
    /// speed performance.
    /// Revision 2.0, 8 Dec 2010, uses a circlar stack of C# objects,
    /// adding new events at arbitrary points in the circle when new events fill the
    /// stack, and using moveable beginning and ending points for analysis.
    /// </summary>
    sealed public class RossiAlphaCircularStackAnalysis
    {
        public UInt64 rossiAlphaGateWidth;
        public UInt64 rossiAlphaWindowWidth;  //equal to RawAnalysisProperties.numRAGatesPerWindow * gate width
        public UInt32[] neutronsPerRossiAlphaGate = new UInt32[RawAnalysisProperties.numRAGatesPerWindow];
        public UInt64 totalRossiAlphaAnalysisTime;  //the time of the closing of the last completed RA gate, in other words, the duration of this experiment

        public RossiAlphaCircularNeutronEvent theEventCircularLinkedList;
        public RossiAlphaCircularNeutronEvent startOfList;
        public RossiAlphaCircularNeutronEvent endOfList;
        public int numObjectsInCircularLinkedList;
        public int numCircuits;  //number of times the circular linked list was "lapped"

        public BackgroundWorker RACWorker = null;

        //use a ManualResetEvent here instead of ManualResetEventSlim, 
        //because it might be milliseconds between neutron events and we don't want to spin that long
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNeutrons;  //another thread places neutron-event neutron data here for processing
        public UInt32[] inputEventNumNeutrons;
        public int numEventsThisBlock;

        public bool keepRunning;

        public bool isReadyToAnalyze = false;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        public RossiAlphaCircularStackAnalysis(UInt64 gateWidth)
        {
            int i;

            isReadyToAnalyze = false;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
            spinTimeReady = false;
#endif

            rossiAlphaGateWidth = gateWidth;
            rossiAlphaWindowWidth = ((UInt64)(RawAnalysisProperties.numRAGatesPerWindow)) * gateWidth;
            totalRossiAlphaAnalysisTime = 0;

            for (i = 0; i < RawAnalysisProperties.numRAGatesPerWindow; i++)
            {
                neutronsPerRossiAlphaGate[i] = 0;
            }

            //initialize the event inputs
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            inputEventNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            inputEventNumNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

            //create the circular linked list
            theEventCircularLinkedList = new RossiAlphaCircularNeutronEvent(0);  //create the first element in the list
            startOfList = theEventCircularLinkedList;  //set the head of the list to this first element
            endOfList = theEventCircularLinkedList;  //set the tail of thelist to this first element
            for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)  //as a starter, put RawAnalysisProperties.circularListBlockIncrement structures in the list
            {
                endOfList.next = new RossiAlphaCircularNeutronEvent(i);  //create the next new struct at the end of the tail
                endOfList = endOfList.next;  //move the tail to this new stuct
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;  //close the loop, connecting the tail back to the head
            endOfList = startOfList;  //set the head and tail to the first empty structure

            numCircuits = 0;

            //Set Up the RAC BackgroundWorker
            keepRunning = true;
            isReadyToAnalyze = false;
            RACWorker = new BackgroundWorker();
            RACWorker.WorkerSupportsCancellation = false;  //it either processes a neutron event, or it doesn't.
            RACWorker.DoWork += new DoWorkEventHandler(RACWorker_DoWork);
            RACWorker.RunWorkerAsync();

            //pause until the RAcWorker is working
            while (RACWorker.IsBusy == false)
            {
                Thread.Sleep(1);
            }
        }

        #region SpinTimeMethods
#if USE_SPINTIME
        private void StartSpinCount()
        {
            spinTimeStart = DateTime.Now.Ticks;
        }
        private void EndSpinCount()
        {
            spinTimeTotal += DateTime.Now.Ticks - spinTimeStart;
        }
#endif
        #endregion

        void RACWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UInt64 deltaTimeInQueue;

            String threadName = "RossiAlphaAnalyzer_" + rossiAlphaGateWidth;
            Thread.CurrentThread.Name = threadName;

#if USE_SPINTIME
            spinTimeReady = false;
#endif

            while (keepRunning == true)
            {
                isReadyToAnalyze = true;

#if USE_SPINTIME
                if (spinTimeReady)
                {
                    StartSpinCount();
                }
#endif

                waitingForMessage.Wait();  //wait for some other thread to place neutron data in this object and signal this worker to analyze

#if USE_SPINTIME
                if (spinTimeReady)
                {
                    EndSpinCount();
                }
                else
                {
                   spinTimeReady = true;
                }
#endif

                if (keepRunning == true)
                {
                    UInt64 eventTime;
                    UInt32 eventNeutrons;
                    UInt32 eventNumNeutrons;
                    int j;

                    for (j = 0; j < numEventsThisBlock; j++)
                    {
                        eventTime = inputEventTime[j];
                        eventNeutrons = inputEventNeutrons[j];
                        eventNumNeutrons = inputEventNumNeutrons[j];

                        //fill in these new data at the tail of the circular linked list,
                        //remembering that endOfNeutronEventList points to the next EMPTY struct in the list
                        //INCLUDING at the beginning when the head and tail point to the same struct
                        endOfList.FillEventWithData(eventTime, eventNeutrons, eventNumNeutrons);

                        //check to see if the circular list will overflow
                        if (endOfList.next.serialNumber == startOfList.serialNumber)
                        {
                            int i;
                            RossiAlphaCircularNeutronEvent anEvent;
                            RossiAlphaCircularNeutronEvent nextEvent;

                            //if stack would overflow, add RawAnalysisProperties.circularListBlockIncrement neutron events at this spot in the stack...
                            anEvent = endOfList;
                            nextEvent = endOfList.next;
                            for (i = 0; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                            {
                                anEvent.next = new RossiAlphaCircularNeutronEvent(i + numObjectsInCircularLinkedList);
                                anEvent = anEvent.next;
                            }
                            anEvent.next = nextEvent;  //patch the circular linked list back together
                            numObjectsInCircularLinkedList += RawAnalysisProperties.circularListBlockIncrement;  //increase the record of the number of structs in the circular list
                        }

                        //move endOfNeutronEventList to the next empty struct
                        endOfList = endOfList.next;

                        //for fun, count how many times we have lapped the circular list in this experiment
                        if (endOfList.serialNumber == 0)
                        {
                            numCircuits++;
                        }

                        //see if oldest event in the queue is expiring,
                        //and while there are expiring events calculate the RossiAlpha statistics for those events
                        deltaTimeInQueue = eventTime - startOfList.eventTime;
                        while (deltaTimeInQueue > rossiAlphaWindowWidth)
                        {
                            //record the time of the expiring gate from the startOfList event, the total measured time for this experiment
                            totalRossiAlphaAnalysisTime = startOfList.eventTime + rossiAlphaWindowWidth;

                            //do RossiAlpha analysis here, accumulating statistics
                            //add up how many neutrons there were in each RossiAlpha gate from the time of the expiring event
                            int whichBin;
                            UInt64 deltaTime;
                            RossiAlphaCircularNeutronEvent anEvent;

                            //skip the event at the head of the list.  
                            //To match data from the legacy code, start with the neutron event following this expiring head event.
                            anEvent = startOfList.next;
                            while (anEvent.serialNumber != endOfList.serialNumber)  //...that is, until we reach the end of the list...
                            {
                                //Find the lowest bin with end time greater than the time of anEvent.
                                deltaTime = anEvent.eventTime - startOfList.eventTime;
                                whichBin = (int)(Math.Ceiling((double)deltaTime / (double)rossiAlphaGateWidth) + 0.1);

                                if (whichBin < RawAnalysisProperties.numRAGatesPerWindow)  //then this event falls into a bin; add this event's neutrons to that bin
                                {
                                    neutronsPerRossiAlphaGate[whichBin] += anEvent.numNeutrons;
                                    //go to the next event
                                    anEvent = anEvent.next;
                                }
                                else
                                {
                                    anEvent = endOfList;  //abort the loop by going to the end of the list
                                }
                            }

                            //remove expiring oldest event from the stack
                            startOfList = startOfList.next;

                            //re-calculate the delta time in the stack
                            deltaTimeInQueue = eventTime - startOfList.eventTime;
                        }  //END of handling an individual NeutronEvent

                    }  //END of handling this block of events

                    //prepare this thread's wait condition so will wait for a message 
                    //before telling master thread this thread's analysis is complete
                    waitingForMessage.Reset();

                }  //END of if(keepRunning)
                else
                {
                    //The master thread has cleared the keepRunning flag, so this worker is exiting
                    //Do any cleanup etc. here.
                }
            }  //END of while(keepRunning)
        } 


        /// <summary>
        /// GetRossiAlphaAnalysisResult() retrieves a copy of the array of RawAnalysisProperties.numRAGatesPerWindow counts-per-gate 
        /// characteristic of the RossiAlpha analysis.  The copy is made for the moment
        /// at which the GetRossiAlphaAnalysisResult is called, and can either be at the end 
        /// of the experiment or at an arbitrary time partway through an experiment.
        /// </summary>
        /// <returns>UInt32[] array of RawAnalysisProperties.numRAGatesPerWindow counts-per-gate values, where the index is the gate number.</returns>
        public UInt32[] GetRossiAlphaAnalysisResult()
        {
            int i;
            UInt32[] result = new UInt32[RawAnalysisProperties.numRAGatesPerWindow];

            for (i = 0; i < RawAnalysisProperties.numRAGatesPerWindow; i++)
            {
                result[i] = neutronsPerRossiAlphaGate[i];
            }

            return (result);
        }

        public RossiAlphaResult GetResult()
        {
            RossiAlphaResult result = new RossiAlphaResult(rossiAlphaGateWidth,
                                                           GetRossiAlphaAnalysisResult());
            return (result);
        }

        /// <summary>
        /// ResetCompletely() clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
            int i;
            RossiAlphaCircularNeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            for (i = 0; i < RawAnalysisProperties.numRAGatesPerWindow; i++)
            {
                neutronsPerRossiAlphaGate[i] = 0;
            }

            //break the circular references, so garbage collector will do its thing
            nextEvent = theEventCircularLinkedList;
            while (nextEvent != null)
            {
                thisEvent = nextEvent;
                nextEvent = thisEvent.next;
                thisEvent.next = null;
            }

            theEventCircularLinkedList = null;
            startOfList = null;
            endOfList = null;
            numObjectsInCircularLinkedList = 0;

            if (closeCounters)
            {
                keepRunning = false;
                isReadyToAnalyze = false;
                waitingForMessage.Set();
            }
            else
            {
                //create the circular linked list
                theEventCircularLinkedList = new RossiAlphaCircularNeutronEvent(0);  //create the first element in the list
                startOfList = theEventCircularLinkedList;  //set the head of the list to this first element
                endOfList = theEventCircularLinkedList;  //set the tail of thelist to this first element
                for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)  //as a starter, put RawAnalysisProperties.circularListBlockIncrement structures in the list
                {
                    endOfList.next = new RossiAlphaCircularNeutronEvent(i);  //create the next new struct at the end of the tail
                    endOfList = endOfList.next;  //move the tail to this new stuct
                }
                numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
                endOfList.next = startOfList;  //close the loop, connecting the tail back to the head
                endOfList = startOfList;  //set the head and tail to the first empty structure

                numCircuits = 0;
                isReadyToAnalyze = true;
            }
        }

        /// <summary>
        /// ResetForConcatenation() clears metadata for a new experiment,
        /// but preserves present histogram array so subsequent data can be added.
        /// </summary>
        public void ResetForConcatenation()
        {
            int i;
            RossiAlphaCircularNeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            //break the circular references, so garbage collector will do its thing
            nextEvent = theEventCircularLinkedList;
            while (nextEvent != null)
            {
                thisEvent = nextEvent;
                nextEvent = thisEvent.next;
                thisEvent.next = null;
            }

            //create the circular linked list
            theEventCircularLinkedList = new RossiAlphaCircularNeutronEvent(0);  //create the first element in the list
            startOfList = theEventCircularLinkedList;  //set the head of the list to this first element
            endOfList = theEventCircularLinkedList;  //set the tail of thelist to this first element
            for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)  //as a starter, put RawAnalysisProperties.circularListBlockIncrement structures in the list
            {
                endOfList.next = new RossiAlphaCircularNeutronEvent(i);  //create the next new struct at the end of the tail
                endOfList = endOfList.next;  //move the tail to this new stuct
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;  //close the loop, connecting the tail back to the head
            endOfList = startOfList;  //set the head and tail to the first empty structure

            numCircuits = 0;
            isReadyToAnalyze = true;
        }
    }
}
