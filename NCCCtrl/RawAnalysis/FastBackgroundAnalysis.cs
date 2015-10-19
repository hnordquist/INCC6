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

namespace LMRawAnalysis
{

    /// <summary>
    /// FastBackgroundAnalysis is an object for handling statistics of neutron events
    /// within "delta time" units of time.  Each one of these units of time is a gate,
    /// and the gate width is this delta time.
    /// 
    /// Statistics that are accummulated are how many gates there have been,
    /// how many gates have had N neutrons (aggregated across all selected channels),
    /// and usual stuff like mean, mode, variance, whathaveyou.
    /// 
    /// Almost identical to a FeynmanGateAnalysis, FastBackgroundAnalysis differs
    /// by allowing the gates to overlap, typically incrementing the gate-opening-time
    /// by one microsecond rather than by the gate width.
    /// </summary>
    public class FastBackgroundAnalysis
    {
        public UInt64 gateWidthInTics;
        public UInt64 gateStepInTics;
        public UInt64 timeNextGateWillOpenInTics;
        public UInt64 timeNextGateWillCloseInTics;

        public UInt64[] numGatesHavingNumNeutrons = new UInt64[RawAnalysisProperties.maxNeutronsPerMultiplicityGate];

        public NeutronEvent theEventCircularLinkedList;
        public NeutronEvent startOfList;
        public NeutronEvent endOfList;
        public int numObjectsInCircularLinkedList;
        public int numCircuits;

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNumNeutrons;  //another thread places neutron-event neutron data here for processing
        public int numEventsThisBlock;

        public BackgroundWorker FBWorker = null;
        public bool keepRunning;
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        public bool isReadyToAnalyze;

        public UInt64 numFastBackgroundGates;
        public UInt64 totalFastBackgroundObservationTime;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        public FastBackgroundAnalysis(UInt64 gateDeltaTimeInTics, UInt64 gateStartDeltaInTics)
        {
            int i;

            gateWidthInTics = gateDeltaTimeInTics;
            gateStepInTics = gateStartDeltaInTics;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
#endif

            //initialize the counts
            for (i = 0; i < RawAnalysisProperties.maxNeutronsPerMultiplicityGate; i++)
            {
                numGatesHavingNumNeutrons[i] = 0;
            }

            //initialize the event holders
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            inputEventNumNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

            //starting from time = 0, then first gate will close in "delta time"
            timeNextGateWillOpenInTics = 0;
            timeNextGateWillCloseInTics = gateDeltaTimeInTics;

            //create the circular linked list
            theEventCircularLinkedList = new NeutronEvent(0);
            startOfList = theEventCircularLinkedList;
            endOfList = theEventCircularLinkedList;
            for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
            {
                endOfList.next = new NeutronEvent(i);
                endOfList = endOfList.next;
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;
            endOfList = startOfList;

            numCircuits = 0;

            //initialize fast-background counters
            numFastBackgroundGates = 0;
            totalFastBackgroundObservationTime = 0;

            //set up the FA BackgroundWorker
            keepRunning = true;
            isReadyToAnalyze = false;
            FBWorker = new BackgroundWorker();
            FBWorker.WorkerSupportsCancellation = false;
            FBWorker.DoWork += new DoWorkEventHandler(FBWorkerDoWork);
            FBWorker.RunWorkerAsync();

            //pause until the FBWorker is working
            while (FBWorker.IsBusy == false)
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

        void FBWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String threadName = "FastBackgroundAnalyzer_" + gateWidthInTics;
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
                    UInt32 eventNumNeutrons;
                    NeutronEvent anEvent;
                    NeutronEvent nextEvent;
                    NeutronEvent lastEvent;
                    UInt32 totalNeutronsInGate;

                    for (j = 0; j < numEventsThisBlock; j++)
                    {
                        eventTime = inputEventTime[j];
                        eventNumNeutrons = inputEventNumNeutrons[j];

                        //fill in these new data at the tail of the circular linked list,
                        //remembering that endOfList points to the next EMPTY struct in the list
                        //INCLUDING at the beginning when the head and tail point to the same struct.
                        //NOTE: MuliplicityAnalysisFastBackground needs ONLY the numNeutrons, not the channels...
                        endOfList.eventTime = eventTime;
                        endOfList.numNeutrons = eventNumNeutrons;

                        //check to see if the circular list will overflow
                        if (endOfList.next.serialNumber == startOfList.serialNumber)
                        {
                            //if stack would overflow, add RawAnalysisProperties.circularListBlockIncrement neutron events at this spot in the stack...
                            anEvent = endOfList;
                            nextEvent = endOfList.next;
                            for (i = 0; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                            {
                                anEvent.next = new NeutronEvent(i + numObjectsInCircularLinkedList);
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

                        while (lastEvent.eventTime > timeNextGateWillCloseInTics)
                        {
                            totalNeutronsInGate = 0;
                            //throw out any expired events
                            while ((startOfList.serialNumber != endOfList.serialNumber)
                                   && (startOfList.eventTime < timeNextGateWillOpenInTics))
                            {
                                startOfList = startOfList.next;
                            }

                            //skip ahead past any empty gates
                            if (startOfList.eventTime >= timeNextGateWillCloseInTics)
                            {
                                //This looks complicated, so bear with me...

                                //Earliest a non-empty gate could open is next eventtime minus gateWidth,
                                //but +1 because events are excluded if they occur at the time the gate would close.
                                UInt64 timeNonEmptyGateCouldOpen = (startOfList.eventTime + 1) - gateWidthInTics;

                                //emptyTimeSpan is the delta time between the time the present gate opened and this earliest time
                                UInt64 emptyTimeSpan = timeNonEmptyGateCouldOpen - timeNextGateWillOpenInTics;

                                //Then the number of empty time steps is this deltaTime divided by the gateStep
                                UInt64 numEmptyTimesteps = emptyTimeSpan / gateStepInTics;

                                //But if the remainder of this deltaTime/gateStep isn't 0, then "round up" to next gate, or +1 to numEmptysteps
                                if ((emptyTimeSpan % gateStepInTics) > 0)
                                {
                                    numEmptyTimesteps++;
                                }

                                //use this numEmpty gates to jump ahead to first non-empty gate
                                timeNextGateWillOpenInTics += numEmptyTimesteps * gateStepInTics;
                                timeNextGateWillCloseInTics = timeNextGateWillOpenInTics + gateWidthInTics;
                                numGatesHavingNumNeutrons[0] += numEmptyTimesteps;
                                numFastBackgroundGates += numEmptyTimesteps;
                            }

                            //check again, so non-empty gates will be counted only once,
                            //to make sure the first non-empty gate is closed before we count in it...
                            if (lastEvent.eventTime > timeNextGateWillCloseInTics)
                            {

                                //count neutrons in this gate
                                anEvent = startOfList;
                                while ((anEvent.serialNumber != endOfList.serialNumber)
                                       && (anEvent.eventTime < timeNextGateWillCloseInTics))
                                {
                                    //LOGIC NOTE: can't check for eventTime against gate opening in the while() test,
                                    //else a discriminated event would terminate before later events were counted...
                                    if (anEvent.eventTime >= timeNextGateWillOpenInTics)
                                    {
                                        totalNeutronsInGate += anEvent.numNeutrons;
                                    }
                                    //...but always continue to the next event...
                                    anEvent = anEvent.next;
                                }

                                //store results
                                if (totalNeutronsInGate < 999)
                                {
                                    numGatesHavingNumNeutrons[totalNeutronsInGate]++;
                                }
                                else
                                {
                                    //XXX need to fire an event here to announce a gate having population above limit supported (998)
                                    numGatesHavingNumNeutrons[999]++;
                                }

                                //move gate forward in time, until last event isn't after the gate-close time
                                timeNextGateWillOpenInTics += gateStepInTics;
                                timeNextGateWillCloseInTics += gateStepInTics;

                                //count the completion of another gate
                                numFastBackgroundGates++;
                            }
                        }

                        //store the final observation time
                        totalFastBackgroundObservationTime = timeNextGateWillOpenInTics;

                    }  //END of handling this NeutronEvent
                }  //END of handling all the NeutronEvents in this block

                //prepare this thread's wait condition so will wait for a message 
                //before telling master thread this thread's analysis is complete
                waitingForMessage.Reset();
            }  //END of while (keepRunning)            
        }

        /// <summary>
        /// ResetCompletely() clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
            int i;
            NeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            //reset the closing time for the next gate to be the first gate starting from 0
            timeNextGateWillOpenInTics = 0;
            timeNextGateWillCloseInTics = gateWidthInTics;
            
            //empty the dictionary of gate counts
            for (i = 0; i < RawAnalysisProperties.maxNeutronsPerMultiplicityGate; i++)
            {
                numGatesHavingNumNeutrons[i] = 0;
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
                theEventCircularLinkedList = new NeutronEvent(0);
                startOfList = theEventCircularLinkedList;
                endOfList = theEventCircularLinkedList;
                for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                {
                    endOfList.next = new NeutronEvent(i);
                    endOfList = endOfList.next;
                }
                numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
                endOfList.next = startOfList;
                endOfList = startOfList;

                numCircuits = 0;

                //initialize fast-background counters
                numFastBackgroundGates = 0;
                totalFastBackgroundObservationTime = 0;
            }

        }
        
        /// <summary>
        /// ResetForConcatenation() clears metadata for a new experiment,
        /// but preserves present histogram dictionary so subsequent data can be added.
        /// </summary>
        public void ResetForConcatenation()
        {
            int i;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            //reset the closing time for the next gate to be the first gate starting from 0
            timeNextGateWillOpenInTics = 0;
            timeNextGateWillCloseInTics = gateWidthInTics;

            //create the circular linked list
            theEventCircularLinkedList = new NeutronEvent(0);
            startOfList = theEventCircularLinkedList;
            endOfList = theEventCircularLinkedList;
            for (i = 1; i < RawAnalysisProperties.circularListBlockIncrement; i++)
            {
                endOfList.next = new NeutronEvent(i);
                endOfList = endOfList.next;
            }
            numObjectsInCircularLinkedList = RawAnalysisProperties.circularListBlockIncrement;
            endOfList.next = startOfList;
            endOfList = startOfList;

            numCircuits = 0;

            //initialize fast-background counters
            numFastBackgroundGates = 0;
            totalFastBackgroundObservationTime = 0;
        }
    }
}
