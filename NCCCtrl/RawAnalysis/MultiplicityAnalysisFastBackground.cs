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
    sealed public class MultiplicityAnalysisFastBackground : SDTMultiplicityCalculator
    {
        public FastBackgroundAnalysis backgroundAnalyzer;
        public UInt64 multiplicityGateWidth;
        public UInt64 multiplicityDeadDelay;
        public UInt64 totalWindow;  //the sum of gate width and deadtime
      
        //use a C# array to keep track of num gates having num neutrons.
        //For array, numNeutrons is the Index and numOccurances is the Value.
        public UInt64[] multiplicity = new UInt64[RawAnalysisProperties.maxNeutronsPerMultiplicityGate];
        public double totalMeasurementTime;

        public NeutronEvent theEventCircularLinkedList;
        public NeutronEvent startOfList;
        public NeutronEvent endOfList;
        public int numObjectsInCircularLinkedList;
        public int numCircuits;

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNumNeutrons;  //another thread places neutron-event neutron data here for processing
        public int numEventsThisBlock;

        public BackgroundWorker MAFBWorker = null;
        public bool keepRunning;
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        public bool isReadyToAnalyze;

        //INCC deadTime coefficients are in strange INCC-format time units, as indicated
        public double deadTimeCoeffTinNanoSecs;
        public double deadTimeCoeffAinMicroSecs;
        public double deadTimeCoeffBinPicoSecs;
        public double deadTimeCoeffCinNanoSecs;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        public MultiplicityAnalysisFastBackground(double theTicSizeInSeconds, UInt64 gateWidthInTics, UInt64 preDelayInTics, FastBackgroundAnalysis fba,
                                                  double deadTimeCoefficientTinNanoSecs,
                                                  double deadTimeCoefficientAinMicroSecs,
                                                  double deadTimeCoefficientBinPicoSecs,
                                                  double deadTimeCoefficientCinNanoSecs)
            : base(theTicSizeInSeconds) //tell the inherited SDTMultiplicityCounter the ticSize
        {
            int i;

            //store the initialization parameters
            backgroundAnalyzer = fba;
            multiplicityGateWidth = gateWidthInTics;
            multiplicityDeadDelay = preDelayInTics;
            totalWindow = gateWidthInTics + preDelayInTics;

            //store the SingleDoubleTriple-calculation parameters
            deadTimeCoeffTinNanoSecs = deadTimeCoefficientTinNanoSecs;
            deadTimeCoeffAinMicroSecs = deadTimeCoefficientAinMicroSecs;
            deadTimeCoeffBinPicoSecs = deadTimeCoefficientBinPicoSecs;
            deadTimeCoeffCinNanoSecs = deadTimeCoefficientCinNanoSecs;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
            spinTimeReady = false;
#endif

            //empty the multiplicity array
            for (i = 0; i < RawAnalysisProperties.maxNeutronsPerMultiplicityGate; i++)
            {
                multiplicity[i] = 0;
            }

            totalMeasurementTime = 0.0;

            //initialize the event holders
            inputEventNumNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

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

            //set up the MAFB BackgroundWorker
            keepRunning = true;
            isReadyToAnalyze = false;
            MAFBWorker = new BackgroundWorker();
            MAFBWorker.WorkerSupportsCancellation = false;
            MAFBWorker.DoWork += new DoWorkEventHandler(MAFBWorkerDoWork);
            MAFBWorker.RunWorkerAsync();

            //pause until the MAFBWorker is working
            while (MAFBWorker.IsBusy == false)
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

        void MAFBWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String threadName = "MAFBAnalyzer_" + multiplicityGateWidth + "_" + multiplicityDeadDelay;
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

                        //produce counts for all the expiring events at beginning of stack, and remove these events
                        // NEXT:  should this be >= instead of > ??????  might be related to A/S issue #51
                        while ((lastEvent.eventTime - startOfList.eventTime) > totalWindow)
                        {
                            UInt64 startTime;

                            startTime = startOfList.eventTime;
                            totalNeutronsInGate = 0;

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
                                if (anEvent.eventTime >= (startTime + multiplicityDeadDelay))
                                {
                                    totalNeutronsInGate += anEvent.numNeutrons;
                                }
                                //...but always continue to the next event...
                                anEvent = anEvent.next;
                            }

                            UInt32 numIdenticalGatesForThisNeutronEvent;

                            //store this gate having this many neutrons, once for each neutron in the triggering event
                            //NOTE: we DO store the gates having zero neutrons
                            numIdenticalGatesForThisNeutronEvent = startOfList.numNeutrons;
                            if (totalNeutronsInGate < 999)
                            {
                                multiplicity[totalNeutronsInGate] += numIdenticalGatesForThisNeutronEvent;
                            }
                            else
                            {
                                //XXX need to fire an event here to announce a gate with too many neutrons
                                multiplicity[999] += numIdenticalGatesForThisNeutronEvent;
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

        public AnalysisDefs.MultiplicityResult GetResult()
        {
            UInt64[] multiplicityHistogram;
            UInt64[] accidentalsHistogram;
            int i;

            //xxx Warning: this Mutex should go in a higher-order object, so all the threads will share it
            Mutex permissionToUseData = new Mutex();

            multiplicityHistogram = new UInt64[RawAnalysisProperties.maxNeutronsPerMultiplicityGate];
            accidentalsHistogram = new UInt64[RawAnalysisProperties.maxNeutronsPerMultiplicityGate];

            //*********** BEGIN use of thread-sensitive data
            permissionToUseData.WaitOne();

            //copy the real-plus-accidental multiplicity histogram
            for (i = 0; i < RawAnalysisProperties.maxNeutronsPerMultiplicityGate; i++)
            {
                multiplicityHistogram[i] = multiplicity[i];
                accidentalsHistogram[i] = backgroundAnalyzer.numGatesHavingNumNeutrons[i];
            }

            permissionToUseData.ReleaseMutex();
            //*********** END use of thread-sensitive data

            return (GetSDTMultiplicityResult(multiplicityHistogram,
                                                  accidentalsHistogram,
                                                  true,
                                                  multiplicityGateWidth,
                                                  multiplicityDeadDelay,
                                                  0,
                                                  deadTimeCoeffTinNanoSecs,
                                                  deadTimeCoeffAinMicroSecs,
                                                  deadTimeCoeffBinPicoSecs,
                                                  deadTimeCoeffCinNanoSecs,
                                                  totalMeasurementTime));
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

            //empty the multiplicity array
            for (i = 0; i < RawAnalysisProperties.maxNeutronsPerMultiplicityGate; i++)
            {
                multiplicity[i] = 0;
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
                isReadyToAnalyze = true;
            }
            backgroundAnalyzer.ResetCompletely(closeCounters);

        }

        /// <summary>
        /// ResetForConcatenation() clears metadata for a new experiment,
        /// but preserves present histogram array so subsequent data can be added.
        /// </summary>
        public void ResetForConcatenation()
        {
            int i;
            NeutronEvent thisEvent, nextEvent;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            totalMeasurementTime = 0.0;

            //Don't empty the multiplicity dictionary

            //break the circular references, so garbage collector will do its thing
            nextEvent = theEventCircularLinkedList;
            while (nextEvent != null)
            {
                thisEvent = nextEvent;
                nextEvent = thisEvent.next;
                thisEvent.next = null;
            }

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

            isReadyToAnalyze = true;

            backgroundAnalyzer.ResetForConcatenation();
        }

    }
}
