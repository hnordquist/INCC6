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
    sealed public class EventSpacingAnalysis
    {
        public UInt64 gateWidth;  //allowing coarseness in observation, defines a unit for measuring spacing
        public bool observedFirstNeutron;  //false until first neutron comes in
        public UInt64 timeOfLastNeutron;
        public UInt32[] eventSpacingHistogram;  //records number of neutrons having spacing equal to the index

        public UInt64[] inputEventTime;  //another thread places neutron-event time data here for processing
        public UInt32[] inputEventNumNeutrons;  //another thread places neutron-event neutron data here for processing
        public int numEventsThisBlock;

        public BackgroundWorker ESWorker = null;
        public bool keepRunning;
        public ManualResetEventSlim waitingForMessage = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        public bool isReadyToAnalyze;

#if USE_SPINTIME
        public long spinTimeTotal;
        public long spinTimeStart;
        private bool spinTimeReady;
#endif

        public EventSpacingAnalysis(UInt64 gateWidthInTics)
        {
            int i;

            //store the initialization parameters
            gateWidth = gateWidthInTics;

            //create and initialize the histogram
            eventSpacingHistogram = new UInt32[RawAnalysisProperties.maxEventSpacing + 1];
            for (i = 0; i <= RawAnalysisProperties.maxEventSpacing; i++)
            {
                eventSpacingHistogram[i] = 0;
            }

#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeStart = 0;
            spinTimeReady = false;
#endif

            //initialize the event holders
            inputEventNumNeutrons = new UInt32[RawAnalysisProperties.maxEventsPerBlock];
            inputEventTime = new UInt64[RawAnalysisProperties.maxEventsPerBlock];
            numEventsThisBlock = 0;

            //prepare to receive neutrons
            observedFirstNeutron = false;
            timeOfLastNeutron = 0;
            keepRunning = true;
            isReadyToAnalyze = false;
            ESWorker = new BackgroundWorker();
            ESWorker.WorkerSupportsCancellation = false;
            ESWorker.DoWork += new DoWorkEventHandler(ESWorkerDoWork);
            ESWorker.RunWorkerAsync();

            //pause until the worker is working
            while (ESWorker.IsBusy == false)
            {
                //spin;
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

        void ESWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            String threadName = "ESAnalyzer_" + gateWidth;
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
                    int j;
                    UInt64 eventTime, eventSpacing;
                    UInt32 eventNumNeutrons;

                    for (j = 0; j < numEventsThisBlock; j++)
                    {
                        eventTime = inputEventTime[j];
                        eventNumNeutrons = inputEventNumNeutrons[j];

                        if (observedFirstNeutron == false)
                        {
                            timeOfLastNeutron = eventTime;
                            observedFirstNeutron = true;
                        }
                        else
                        {
                            eventSpacing = (eventTime - timeOfLastNeutron) / gateWidth;
                            timeOfLastNeutron = eventTime;

                            if (eventSpacing < ((UInt64)(RawAnalysisProperties.maxEventSpacing)))
                            {
                                eventSpacingHistogram[eventSpacing] += eventNumNeutrons;
                            }
                            else
                            {
                                //XXX need to fire an eent here to announce a spacing bigger than histogram can store
                                eventSpacingHistogram[RawAnalysisProperties.maxEventSpacing] += eventNumNeutrons;
                            }
                        }
                    }  //END of handling one of the NeutronEvents in this block

                    //prepare this thread's wait condition so will wait for a message 
                    //before telling master thread this thread's analysis is complete
                    waitingForMessage.Reset();

                } //END handling this block of events
            } //END of while (keepRunning)
        } //END of ESWorkerDoWork

        public AnalysisDefs.EventSpacingResult GetResult()
        {
            int i;
            AnalysisDefs.EventSpacingResult result = new AnalysisDefs.EventSpacingResult();

            result.gateWidthInTics = gateWidth;
            for (i = 0; i <= RawAnalysisProperties.maxEventSpacing; i++)
            {
                result.eventSpacingHistogram[i] = eventSpacingHistogram[i];
                if (eventSpacingHistogram[i] > 0)
                {
                    result.maxIndexOfNonzeroHistogramEntry = i;
                }
            }

            return (result);
        }

        public void ResetCompletely(bool closeCounters)
        {
            int i;

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
				//reset the histogram
				for (i = 0; i <= RawAnalysisProperties.maxEventSpacing; i++)
				{
					eventSpacingHistogram[i] = 0;
				}

				//reset the time-of-event variables
				timeOfLastNeutron = 0;
				observedFirstNeutron = false;
			}
        }

        public void ResetForConcatenation()
        {
#if USE_SPINTIME
            spinTimeTotal = 0;
            spinTimeReady = false;
#endif
            //don't reset the histogram

            //reset the time-of-event variables
            timeOfLastNeutron = 0;
            observedFirstNeutron = false;
        }
    }
}
