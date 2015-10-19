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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace LMRawAnalysis
{
    /// <summary>
    /// FeynmanGateAnalysis is an object for handling statistics of neutron events
    /// within "delta time" units of time.  Each one of these units of time is a gate,
    /// and the gate width is this delta time.
    /// 
    /// Statistics that are accummulated are how many gates there have been,
    /// how many gates have had N neutrons (aggregated across all selected channels),
    /// and usual stuff like mean, mode, variance, whathaveyou.
    /// </summary>
    public class FeynmanGateAnalysis
    {
        public UInt64 gateWidthInTics;
        public UInt64 timeNextGateWillCloseInTics;
        private UInt32 numNeutronsInPresentGate;

        private bool statsAreUpToDate;
        public double cbar;
        public double c2bar;
        public double c3bar;
        public double C;  //the denominator used during calculations of cbar, c2bar, and c3bar;

        //use a C# Dictionary to keep track of num gates having num neutrons.
        //For Dictionary numGatesHavingNumNeutrons, numNeutrons is the Key and numGates is the Value.
        public Dictionary<UInt32, UInt32> numGatesHavingNumNeutrons = new Dictionary<UInt32,UInt32>();

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNumNeutrons;  //another thread places neutron-event neutron data here for processing
        public int numEventsThisBlock;

        public BackgroundWorker FAWorker = null;
        public bool keepRunning;
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        public bool isReadyToAnalyze;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        public FeynmanGateAnalysis(UInt64 gateDeltaTimeInTics)
        {
            gateWidthInTics = gateDeltaTimeInTics;

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
            spinTimeReady = false;
#endif

            //starting from time = 0, then first gate will close in "delta time"
            timeNextGateWillCloseInTics = gateDeltaTimeInTics;

            numNeutronsInPresentGate = 0;

            statsAreUpToDate = false;

            //initialize the events inputs
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            inputEventNumNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

            //set up the FA BackgroundWorker
            keepRunning = true;
            isReadyToAnalyze = false;
            FAWorker = new BackgroundWorker();
            FAWorker.WorkerSupportsCancellation = false;
            FAWorker.DoWork += new DoWorkEventHandler(FAWorkerDoWork);
            FAWorker.RunWorkerAsync();

            //pause until the FBWorker is working
            while (FAWorker.IsBusy == false)
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

        void FAWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String threadName = "FeynmanAnalyzer_" + gateWidthInTics;
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
                    UInt64 eventTime;
                    UInt32 eventNumNeutrons;
                    int j;

                    for (j = 0; j < numEventsThisBlock; j++)
                    {
                        eventTime = inputEventTime[j];
                        eventNumNeutrons = inputEventNumNeutrons[j];


                        //is time still within the present gate?
                        if (eventTime <= timeNextGateWillCloseInTics)  //then yes, still in present gate, so aggregate neutrons
                        {
                            //add one neutron to numNeutronsInPresentGate for every non-zero bit in the neutronEventByChannels
                            numNeutronsInPresentGate += eventNumNeutrons;
                        }
                        else //then time is beyond the gate, so close this gate, calculate statistics, and restart for the next delta-time gate
                        {
                            //First, close the gate, incrementing the number of gates having this many counts...
                            UInt32 prevNumGatesWithThisManyCounts;
                            UInt32 prevNumGatesWithZeroCounts;

                            //see if gates with this many neutrons is already in the dictionary
                            if (numGatesHavingNumNeutrons.TryGetValue(numNeutronsInPresentGate, out prevNumGatesWithThisManyCounts))
                            {
                                //if so, then change the value for this number of neutrons by +1
                                numGatesHavingNumNeutrons[numNeutronsInPresentGate] = prevNumGatesWithThisManyCounts + 1;
                            }
                            else
                            {
                                //if not, add new dictionary entry for this num neutrons with value equal to 1 (because this gate has that many neutrons)
                                numGatesHavingNumNeutrons.Add(numNeutronsInPresentGate, 1);
                            }

                            //Second, see if any gates were skipped...
                            UInt32 numGatesSkipped;

                            numGatesSkipped = (uint)((eventTime - timeNextGateWillCloseInTics) / gateWidthInTics);
                            if (numGatesHavingNumNeutrons.TryGetValue(0, out prevNumGatesWithZeroCounts))
                            {
                                //if so, then change the value for this number of neutrons by +1
                                numGatesHavingNumNeutrons[0] = prevNumGatesWithZeroCounts + numGatesSkipped;
                            }
                            else
                            {
                                //if not, add new dictionary entry for zero neutrons with value equal to numGatesSkipped
                                numGatesHavingNumNeutrons.Add(0, numGatesSkipped);
                            }

                            //Finally, start a new gate containing this newest time
                            //set the closing time for the new gate...
                            timeNextGateWillCloseInTics += (numGatesSkipped + 1) * gateWidthInTics;
                            //set the initial number of neutrons for the new gate...
                            numNeutronsInPresentGate = eventNumNeutrons;
                        }
                    }  //END of handling individual NeutronEvent
                }  //END of handling this block of NeutronEvent

                //prepare this thread's wait condition so will wait for a message 
                //before telling master thread this thread's analysis is complete
                waitingForMessage.Reset();
            }  //END of while (keepRunning)            
        }


        /// <summary>
        /// calculateFeynmanStatistics()
        /// For the present population of number of gates having howevermany neutrons,
        /// calculate the Feynman statistics.  
        /// Note this can either be final (e.g., at end of an NCD file)
        /// or incremental (e.g., partway through an NCD file or in middle of receiving some real-time stream)
        /// </summary>
        public void calculateFeynmanStatistics()
        {
            if (statsAreUpToDate == false)
            {
                AnalysisDefs.FeynmanResult result;

                result = GetResultFromFeynmanGateDictionary(numGatesHavingNumNeutrons,true);
                result.gateWidth = gateWidthInTics;
                //save the Feynman statistics for public access
                cbar = result.cbar;
                c2bar = result.c2bar;
                c3bar = result.c3bar;
                C = result.C;

                statsAreUpToDate = true;
            }
        }

        static public AnalysisDefs.FeynmanResult GetResultFromFeynmanGateDictionary(Dictionary<UInt32, UInt32> feynmanGateDictionary, bool copy)
        {
            AnalysisDefs.FeynmanResult result = new AnalysisDefs.FeynmanResult();

            double C, nC, n2C, n3C;
            double temp;
            double numNeutrons;

            C = 0.0;
            nC = 0.0;
            n2C = 0.0;
            n3C = 0.0;

           // result.gateWidth = gateWidthInTics;

            //for each entry in numGatesHavingNumNeutrons
            result.maxDictionaryKey = 0;
            foreach (KeyValuePair<UInt32, UInt32> pair in feynmanGateDictionary)
            {
                numNeutrons = pair.Key;
                temp = (double)pair.Value;  //the number of gates having that numNeutrons

                C += temp;  //so C will be the total number of complete gates with any numNeutrons
                temp *= numNeutrons;
                nC += temp;  //nC is the weighted sum of numGates * numNeutrons
                temp *= numNeutrons;
                n2C += temp; //n2C is the weight sum of numGates * numNeutrons^2
                temp *= numNeutrons;
                n3C += temp; //n3C is the weight sum of numGates * numNeutrons^3

                if (copy)
                {
                    result.numGatesHavingNumNeutrons.Add(pair.Key, pair.Value);
                    if (pair.Key > result.maxDictionaryKey)
                    {
                        result.maxDictionaryKey = pair.Key;
                    }
                }
            }


            if (C > 0.0)
            {
                result.C = C;
                result.cbar = nC / C;
                result.c2bar = n2C / C;
                result.c3bar = n3C / C;
            }
            return (result);
        }

        public AnalysisDefs.FeynmanResult GetResult()
        {
            AnalysisDefs.FeynmanResult result = GetResultFromFeynmanGateDictionary(numGatesHavingNumNeutrons, true);
            result.gateWidth = gateWidthInTics;
            return (result);
        }

        /// <summary>
        /// ResetCompletely() clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            if (closeCounters)
            {
                keepRunning = false;
                isReadyToAnalyze = false;
                waitingForMessage.Set();
            }
            else
            {
                //reset the closing time for the next gate to be the first gate starting from 0
                timeNextGateWillCloseInTics = gateWidthInTics;
                //clear any neutrons from the present gate
                numNeutronsInPresentGate = 0;

                //set flags and clear statistics
                statsAreUpToDate = false;
                cbar = 0.0;
                c2bar = 0.0;
                c3bar = 0.0;

                //empty the dictionary of gate counts
                numGatesHavingNumNeutrons.Clear();
            }
        }
        
        /// <summary>
        /// ResetForConcatenation() clears metadata for a new experiment,
        /// but preserves present histogram dictionary so subsequent data can be added.
        /// </summary>
        public void ResetForConcatenation()
        {
#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif

            //reset the closing time for the next gate to be the first gate starting from 0
            timeNextGateWillCloseInTics = gateWidthInTics;
            //clear any neutrons from the present gate
            numNeutronsInPresentGate = 0;

            //set flags and clear statistics
            statsAreUpToDate = false;
            cbar = 0.0;
            c2bar = 0.0;
            c3bar = 0.0;
        }
    }
}
