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
    sealed public class CoincidenceAnalysisSlowBackground
    {
        public UInt64 coincidenceGateWidth;
        public UInt64 coincidenceDeadDelay;
        public UInt64 coincidenceAccidentalsDelay;  //time in tics between NeutronEvent trigger and the time at which the accidentals gate OPENS.
        public UInt64 totalWindow;  //the sum of gate width and accidentals delay
        private UInt64 realsWindow; //the sum of gate width and dead delay
        private double ticSizeInSeconds;

        //use a C# array to keep track of coicidence count.
        //For array, row number is the channel "a" of the initiating neutron,
        //column number os the channel "b" of the coincident neutron,
        //and value is the number of occurances of that "a,b" concidence.
        public UInt64[][] RAcoincidence;
        public UInt64[][] Acoincidence;
        private double totalMeasurementTime;

        public CoincidenceNeutronEvent theEventCircularLinkedList;
        public CoincidenceNeutronEvent startOfList;
        public CoincidenceNeutronEvent endOfList;
        public int numObjectsInCircularLinkedList;
        public int numCircuits;

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNeutrons;  //another thread places neutron-event neutron data here for processing NOTE THESE ARE NEUTRONS BITS
        public int numEventsThisBlock;

        public UInt32[] chnmask;

        public BackgroundWorker CASBWorker = null;
        public bool keepRunning;
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        public bool isReadyToAnalyze;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        /// <summary>
        /// CoincidenceAnalysisSlowBackground.
        /// Note that accidentalsGateDelayInTics is the time between the trigger and the OPENING of the accidentals gate.
        /// Similarly, preDelayInTics is the time between the trigger and the OPENING of the R+A gate (the "dead time")
        /// </summary>
        public CoincidenceAnalysisSlowBackground(UInt64 gateWidthInTics, UInt64 preDelayInTics,
                                                 UInt64 accidentalsGateDelayInTics,
                                                 double theTicSizeInSeconds)
        {

            //store the initialization parameters
            ticSizeInSeconds = theTicSizeInSeconds;
            coincidenceGateWidth = gateWidthInTics;
            coincidenceDeadDelay = preDelayInTics;
            coincidenceAccidentalsDelay = accidentalsGateDelayInTics;
            totalWindow = gateWidthInTics + accidentalsGateDelayInTics;
            realsWindow = gateWidthInTics + preDelayInTics;

            chnmask = new UInt32[RawAnalysisProperties.ChannelCount];
            for (int i = 0; i < RawAnalysisProperties.ChannelCount; i++)
            {
                chnmask[i] = (uint)1 << i;
            }


#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
            spinTimeReady = false;
#endif

            //create the coincidence arrays
            RAcoincidence = new UInt64[RawAnalysisProperties.ChannelCount][];
            Acoincidence = new UInt64[RawAnalysisProperties.ChannelCount][];
            for (int i = 0; i < RawAnalysisProperties.ChannelCount; i++)
            {
                RAcoincidence[i] = new UInt64[RawAnalysisProperties.ChannelCount];
                Acoincidence[i] = new UInt64[RawAnalysisProperties.ChannelCount];
            }

            totalMeasurementTime = 0.0;

            //initialize the event holders
            inputEventNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

            //create the circular linked list
            theEventCircularLinkedList = new CoincidenceNeutronEvent(0);
            startOfList = theEventCircularLinkedList;
            endOfList = theEventCircularLinkedList;
            for (int i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
            {
                endOfList.next = new CoincidenceNeutronEvent(i);
                endOfList = endOfList.next;
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;
            endOfList = startOfList;

            numCircuits = 0;

            //set up the MASB BackgroundWorker
            keepRunning = true;
            isReadyToAnalyze = false;
            CASBWorker = new BackgroundWorker();
            CASBWorker.WorkerSupportsCancellation = false;
            CASBWorker.DoWork += new DoWorkEventHandler(MASBWorkerDoWork);
            CASBWorker.RunWorkerAsync();

            //pause until the CASBWorker is working
            while (CASBWorker.IsBusy == false)
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

        void MASBWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String threadName = "CASBAnalyzer_" + coincidenceGateWidth + "_" + coincidenceDeadDelay + "_" + coincidenceAccidentalsDelay;
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

                waitingForMessage.Wait();  //wait for some other thread to send data or send quit signal

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
                    int i, j;
                    UInt64 eventTime;
                    UInt32 eventNeutrons;
                    CoincidenceNeutronEvent anEvent;
                    CoincidenceNeutronEvent nextEvent;
                    CoincidenceNeutronEvent lastEvent;

                    for (j = 0; j < numEventsThisBlock; j++)
                    {
                        eventTime = inputEventTime[j];
                        eventNeutrons = inputEventNeutrons[j];

                        //fill in these new data at the tail of the circular linked list,
                        //remembering that endOfList points to the next EMPTY struct in the list
                        //INCLUDING at the beginning when the head and tail point to the same struct.
                        endOfList.eventTime = eventTime;
                        endOfList.eventNeutrons.Clear();  //clear the list to remove any left-over neutrons from previous use of this object
                        for (i = 0; i < RawAnalysisProperties.ChannelCount; i++)
                        {
                            if ((eventNeutrons & chnmask[i]) != 0)
                            {
                                endOfList.eventNeutrons.Add(i);
                            }
                        }

                        //check to see if the circular list will overflow
                        if (endOfList.next.serialNumber == startOfList.serialNumber)
                        {
                            //if stack would overflow, add RawAnalysisProperties.circularListBlockIncrement neutron events at this spot in the stack...
                            anEvent = endOfList;
                            nextEvent = endOfList.next;
                            for (i = 0; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                            {
                                anEvent.next = new CoincidenceNeutronEvent(i + numObjectsInCircularLinkedList);
                                anEvent = anEvent.next;
                            }
                            anEvent.next = nextEvent;  //patch the circular linked list back together
                            numObjectsInCircularLinkedList += RawAnalysisProperties.circularListBlockIncrement;  //increase the record of the number of structs in the circular list
                        }

                        //remember this new last event
                        lastEvent = endOfList;

                        //move endOfList to the next empty struct
                        endOfList = endOfList.next;

                        //for fun, count how many times we have lapped the circular list in this experiment
                        if (endOfList.serialNumber == 0)
                        {
                            numCircuits++;
                        }

                        //produce counts for all the expiring events at beginning of stack, and remove these events
                        // NEXT:  should this be >= instead of > ??????  might be related to A/S issue #51
                        while ((lastEvent.eventTime - startOfList.eventTime) > totalWindow)
                        {
                            UInt64 startTime;
                            int aindex, bindex;
                            int numa, numb;
                            int neutronA, neutronB;

                            startTime = startOfList.eventTime;
                            numa = startOfList.eventNeutrons.Count;

                            anEvent = startOfList.next;  //skip the neutrons of the triggering event

                            //count the number of neutrons in the stack within the allowed deltaTimes
                            //NOTE: logic for these gates are: start <= included < end
                            //That is, an event equal to gate-open time is inside the gate
                            //     but an event equal to gate-end  time is outside the gate
                            while ((anEvent != null) && (anEvent.serialNumber != endOfList.serialNumber)
                                   && (anEvent.eventTime < (startTime + totalWindow)))
                            {
                                //LOGIC NOTE: can't check for eventTime against gate opening in the while() test,
                                //else a discriminated event would terminate before later events were counted...
                                if ((anEvent.eventTime >= (startTime + coincidenceDeadDelay))
                                    &&
                                    (anEvent.eventTime < (startTime + realsWindow)))
                                {
                                    //for each pair of neutrons "a" in the expiring event and "b" in the event in the window,
                                    //increment the coincidence element "a,b"...
                                    numb = anEvent.eventNeutrons.Count;
                                    for (aindex = 0; aindex < numa; aindex++)
                                    {
                                        neutronA = startOfList.eventNeutrons[aindex];
                                        for (bindex = 0; bindex < numb; bindex++)
                                        {
                                            neutronB = anEvent.eventNeutrons[bindex];
                                            RAcoincidence[neutronA][neutronB]++;
                                        }
                                    }
                                }

                                //now check for neutrons in the accidentals gate
                                if (anEvent.eventTime >= (startTime + coincidenceAccidentalsDelay))
                                {
                                    numb = anEvent.eventNeutrons.Count;
                                    for (aindex = 0; aindex < numa; aindex++)
                                    {
                                        neutronA = startOfList.eventNeutrons[aindex];
                                        for (bindex = 0; bindex < numb; bindex++)
                                        {
                                            neutronB = anEvent.eventNeutrons[bindex];
                                            Acoincidence[neutronA][neutronB]++;
                                        }
                                    }
                                }

                                //...but always continue to the next event...
                                anEvent = anEvent.next;
                            }

                            //record the time of this expiring event; this is the total measurement time
                            totalMeasurementTime = ((double)(startOfList.eventTime + totalWindow)) * ticSizeInSeconds;
                            //expire this now-too-old event, and continue until all expiring events are handled...
                            startOfList = startOfList.next;
                        }
                    } //END of handling this NeutronEvent in this block

                    //prepare this thread's wait condition so will wait for a message 
                    //before telling master thread this thread's analysis is complete
                    waitingForMessage.Reset();

                } //END handling this block of events
            } //END of while (keepRunning)  
        }

        public CoincidenceResult GetResult()
        {
            int i, j;
            CoincidenceResult result;

            result = new CoincidenceResult(RawAnalysisProperties.ChannelCount);

            result.coincidenceGateWidth = coincidenceGateWidth;
            result.coincidenceDeadDelay = coincidenceDeadDelay;
            result.isSlowBackground = true;
            result.accidentalsDelay = coincidenceAccidentalsDelay;

            for (i = 0; i < RawAnalysisProperties.ChannelCount; i++)
            {
                for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
                {
                    if (totalMeasurementTime > 0.0)
                    {
                        result.RACoincidenceRate[i][j] = ((double)RAcoincidence[i][j]) / totalMeasurementTime;
                        result.ACoincidenceRate[i][j] = ((double)Acoincidence[i][j]) / totalMeasurementTime;
                    }
                    else
                    {
                        result.RACoincidenceRate[i][j] = 0.0;
                        result.ACoincidenceRate[i][j] = 0.0;
                    }
                }
            }

            return (result);
        }

        /// <summary>
        /// ResetCompletely() clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
            int i, j;
            CoincidenceNeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            //empty the coincidence array
            for (i = 0; i < RawAnalysisProperties.ChannelCount; i++)
            {
                for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
                {
                    RAcoincidence[i][j] = 0;
                    Acoincidence[i][j] = 0;
                }
            }

            totalMeasurementTime = 0.0;

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
                theEventCircularLinkedList = new CoincidenceNeutronEvent(0);
                startOfList = theEventCircularLinkedList;
                endOfList = theEventCircularLinkedList;
                for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                {
                    endOfList.next = new CoincidenceNeutronEvent(i);
                    endOfList = endOfList.next;
                }
                numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
                endOfList.next = startOfList;
                endOfList = startOfList;

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
            CoincidenceNeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            totalMeasurementTime = 0.0;

            //Don't empty the coincidence dictionary

            //break the circular references, so garbage collector will do its thing
            nextEvent = theEventCircularLinkedList;
            while (nextEvent != null)
            {
                thisEvent = nextEvent;
                nextEvent = thisEvent.next;
                thisEvent.next = null;
            }

            //create the circular linked list
            theEventCircularLinkedList = new CoincidenceNeutronEvent(0);
            startOfList = theEventCircularLinkedList;
            endOfList = theEventCircularLinkedList;
            for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
            {
                endOfList.next = new CoincidenceNeutronEvent(i);
                endOfList = endOfList.next;
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;
            endOfList = startOfList;

            numCircuits = 0;

            isReadyToAnalyze = true;
        }

    }
}
