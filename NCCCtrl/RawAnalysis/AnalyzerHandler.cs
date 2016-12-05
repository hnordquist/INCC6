/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
using NCCReporter;

namespace LMRawAnalysis
{

    sealed public class AnalyzerHandler
    {
        public double ticSizeInSeconds;
        private ulong timeBaseConversion;

        public NeutronEvent theEventCircularLinkedList;
        public NeutronEvent startOfNeutronEventList;
        public NeutronEvent endOfNeutronEventList;  //pointer to the empty structure ready to be filled at the end of the list
        public int numEventsInCircularLinkedList;  //for creating new serial numbers for new events if needed

        public BackgroundWorker AHWorker;
        public bool AHWorkerStopNow;
        public bool AHWorkerStopAtEndOfEvents;

        public ManualResetEventSlim waitingForEvent = new ManualResetEventSlim(false);  //set initial permission to false, so thread will wait
        private SemaphoreSlim permissionToUseEndOfEvents; // LMKV-52: cannot use Mutex due to cross-thread behavior, type changed to single count semaphore, semantically identical to mutex but without the thread context issues, moved creation into the constructor, we know when the constructor is called and from what thread, that meant something when it was Mutex

        public List<FeynmanGateAnalysis> feynmanAnalyzers = new List<FeynmanGateAnalysis>();
        public List<FastBackgroundAnalysis> fastBackgroundAnalyzers = new List<FastBackgroundAnalysis>();
        public List<MultiplicityAnalysisFastBackground> multiplicityFastBackgroundAnalyzers = new List<MultiplicityAnalysisFastBackground>();
        public List<MultiplicityAnalysisSlowBackground> multiplicitySlowBackgroundAnalyzers = new List<MultiplicityAnalysisSlowBackground>();
        public List<RateGateAnalysis> rateAnalyzers = new List<RateGateAnalysis>();
        public List<RossiAlphaCircularStackAnalysis> rossiAlphaAnalyzers = new List<RossiAlphaCircularStackAnalysis>();
        public List<EventSpacingAnalysis> eventSpacingAnalyzers = new List<EventSpacingAnalysis>();

        public List<CoincidenceAnalysisSlowBackground> coincidenceSlowBackgroundAnalyzers = new List<CoincidenceAnalysisSlowBackground>();

        private bool AHWorkerHasCompleted;
        private UInt64 numNeutronEventsReceived;
        private UInt64 numNeutronEventsReceivedWhetherProcessedOrNot;
        private UInt32 numCircuits;
        private UInt64 numNeutronEventsCompleted, accumuNumNeutronEventsCompleted;

        private UInt64 timeOfLastNeutronEvent;

        private LMLoggers.LognLM log;
        private bool verboseTrace;

        public long spinningWaitForInputTimeTotal;
        public long totalRossiAlphaWait;
        public long totalFeynmanWait;
        public long totalFastMultiplicityWait;
        public long totalSlowMultiplicityWait;
        public long totalFastBackWait;
        public long totalEventSpacingWait;
#if USE_SPINTIME
        public long spinTimeStart;
#endif

		public double TickSizeInSeconds
		{
			set
			{
				ticSizeInSeconds = value;
				if (ticSizeInSeconds == 1e-7)
				{
					timeBaseConversion = 1ul;  // no external to internal conversion
				} else if (ticSizeInSeconds == 1e-8)
				{
					timeBaseConversion = 10ul; // shift gate units from tics (1e-7) to shakes (1e-8)
				}
			}
		}

        #region Events

        public delegate void AnalysesCompleted(string statusMessage);
        public event AnalysesCompleted OnAnalysesCompleted;

        public delegate void NeutronOutOfSequenceErrorEvent(string statusMessage);
        public event NeutronOutOfSequenceErrorEvent OnNeutronOutOfSequenceErrorEvent;

        public delegate void BlockCountMismatchErrorEvent(string statusMessage);
        public event BlockCountMismatchErrorEvent OnBlockCountMismatchErrorEvent;

        #endregion


		void InitCounters()
		{
			numNeutronEventsReceived = 0;
            numNeutronEventsReceivedWhetherProcessedOrNot = 0;
            numNeutronEventsCompleted = 0; accumuNumNeutronEventsCompleted = 0;
			numCircuits = 0;
            timeOfLastNeutronEvent = 0;
		}
        public AnalyzerHandler(double theTicSizeInSeconds, LMLoggers.LognLM logger)
        {

            TickSizeInSeconds = theTicSizeInSeconds;
            log = logger;
            verboseTrace = log.ShouldTrace(LogLevels.Verbose);
			InitCounters();

#if USE_SPINTIME
            ResetSpinTime();
#endif
            //create stack of RawAnalysisProperties.circularListBlockIncrement neutron events, as a starting point
            //InitializeEventList(RawAnalysisProperties.circularListBlockIncrement);

            permissionToUseEndOfEvents = new SemaphoreSlim(1, 1); // LMKV-52 fix: create here with counter set at 1 and only 1 thread

            //set up the AH BackgroundWorker
            AHWorkerStopNow = false;
            AHWorkerHasCompleted = false;
            AHWorkerStopAtEndOfEvents = false;
            AHWorker = new BackgroundWorker();
            AHWorker.WorkerSupportsCancellation = false;  //parent will stop the worker with the boolean flags
            AHWorker.DoWork += new DoWorkEventHandler(AHWorkerDoWork);
            AHWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AHWorkerRunWorkerCompleted);
            AHWorker.RunWorkerAsync();

            // permissionToUseEndOfEventsRelease();  // LMKV-52 fix: allow 1 thread at a time now that bkg worker started
            //pause until the AHWorker is working...
            while (AHWorker.IsBusy == false)
            {
                //spin
            }

            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler constructor started its thread.");
        }

        private void FireAnalysesCompletedEvent(string statusMessage)
        {
            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalysesCompletedEvent: " + statusMessage);

            if (OnAnalysesCompleted != null)
            {
                OnAnalysesCompleted(statusMessage);
            }
        }

        private void FireNeutronOutOfSequenceErrorEvent(string statusMessage)
        {
            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "NeutronOutOfSequenceErrorEvent: " + statusMessage);

            if (OnNeutronOutOfSequenceErrorEvent != null)
            {
                OnNeutronOutOfSequenceErrorEvent(statusMessage);
            }
        }

        private void FireBlockCountMismatchErrorEvent(string statusMessage)
        {
            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "BlockCountMismatchErrorEvent: " + statusMessage);

            if (OnBlockCountMismatchErrorEvent != null)
            {
                OnBlockCountMismatchErrorEvent(statusMessage);
            }
        }

        void PermissionToUseEndOfEventsRelease(string where)
        {
            int count = permissionToUseEndOfEvents.CurrentCount;

            if (count == 0)
            {
                permissionToUseEndOfEvents.Release();
                if (verboseTrace) log.TraceEvent(LogLevels.Verbose, 4239, "Released permissionToUseEndOfEvents ({0} -> {1}) {2}", count, permissionToUseEndOfEvents.CurrentCount, where);
            }
            else
            {
                if (verboseTrace) log.TraceEvent(LogLevels.Verbose, 4238, "Caller tried to grant permissionToUseEndOfEvents again ({0} > 0) {1}", count, where);
            }
        }

        void PermissionToUseEndOfEventsWait()
        {
            if (verboseTrace) log.TraceEvent(LogLevels.Verbose, 4237, "Petitioning for the shared end-of-stack ({0})", permissionToUseEndOfEvents.CurrentCount);
            permissionToUseEndOfEvents.Wait();
            if (verboseTrace) log.TraceEvent(LogLevels.Verbose, 4273, "Petition granted for end-of-stack ({0})", permissionToUseEndOfEvents.CurrentCount);
        }

        #region SpinTimeMethods

#if USE_SPINTIME
        private void StartSpinTime()
        {
            spinTimeStart = DateTime.Now.Ticks;
        }

        private void EndSpinTime()
        {
            spinningWaitForInputTimeTotal += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndRossiAlphaSpinTime()
        {
            totalRossiAlphaWait += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndFeynmanSpinTime()
        {
            totalFeynmanWait += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndFastBackSpinTime()
        {
            totalFastBackWait += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndFastMultiplicitySpinTime()
        {
            totalFastMultiplicityWait += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndSlowMultiplicitySpinTime()
        {
            totalSlowMultiplicityWait += DateTime.Now.Ticks - spinTimeStart;
        }

        private void EndEventSpacingSpinTime()
        {
            totalEventSpacingWait += DateTime.Now.Ticks - spinTimeStart;
        }

        public void ResetSpinTime()
        {
            spinningWaitForInputTimeTotal = 0;
            spinTimeStart = 0;
            totalFastBackWait = 0;
            totalFeynmanWait = 0;
            totalFastMultiplicityWait = 0;
            totalSlowMultiplicityWait = 0;
            totalRossiAlphaWait = 0;
        }
#endif

        #endregion

        #region BackgroundWorkerMethods
        void AHWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            int i, j;
            int numFeynmanAnalyzers;
            int numMultiplicityFastBackgroundAnalyzers;
            int numMultiplicitySlowBackgroundAnalyzers;
            int numFastBackgroundAnalyzers;
            int numRateAnalyzers;
            int numRossiAlphaAnalyzers;
            int numEventSpacingAnalyzers;
            int numCoincidenceSlowBackgroundAnalyzers;
            bool quittingAtEndOfList = false;
            UInt32[] eventNeutrons;
            UInt32[] eventNumNeutrons;
            UInt64[] eventTimes;
            int numEventsThisBlock;
            int maxEventsPerBlock = RawAnalysisProperties.maxEventsPerBlock;

#if USE_SPINTIME
            bool spinTimeReady;
#endif

            String threadName = "AnalyzerHandler";
            Thread.CurrentThread.Name = threadName;

            eventNeutrons = new UInt32[maxEventsPerBlock];
            eventNumNeutrons = new UInt32[maxEventsPerBlock];
            eventTimes = new UInt64[maxEventsPerBlock];

#if USE_SPINTIME
            spinTimeReady = false;
#endif

            while (AHWorkerStopNow == false)
            {

#if USE_SPINTIME
                if (spinTimeReady)
                {
                    StartSpinTime();
                }
#endif

                waitingForEvent.Wait();  //wait for a neutron or other signal

                log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Beginning processing a neutron or other signal.");

#if USE_SPINTIME
                if (spinTimeReady)
                {
                    EndSpinTime();
                }
                else
                {
                    spinTimeReady = true;
                }
#endif

                if ((AHWorkerStopNow == false)) //then process the next neutron
                {
                    //first, see if we are at the end of the list already
                    //Note that AHWorkerStopAtEndOfEvents is true only if the controlling thread
                    //has called EndAnalysisWhenFinishedWithPresentEventQueue() to indicate
                    //there will be no more neutron events, 
                    //giving permission for this worker to stop at the end of the queue.
                    if (AHWorkerStopAtEndOfEvents == true)
                    {
                        PermissionToUseEndOfEventsWait();
                        //see if we have processed the last neutron...
                        if (startOfNeutronEventList == null || (startOfNeutronEventList.serialNumber == endOfNeutronEventList.serialNumber))
                        {
                            quittingAtEndOfList = true;
                            AHWorkerStopNow = true;

                            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler reached end of events and is setting flag to stop its background worker at location #1.");
                        }
                        PermissionToUseEndOfEventsRelease("AH AHWorkerStopAtEndOfEvents");
                    }

                    if (quittingAtEndOfList == false)  //...then haven't reached end of list...keep processing...
                    {
                        //process up to RawAnalysisProperties.maxEventsPerBlock neutron events
                        numEventsThisBlock = 0;
                        PermissionToUseEndOfEventsWait();
                        while ((numEventsThisBlock < maxEventsPerBlock)
                               && (startOfNeutronEventList.serialNumber != endOfNeutronEventList.serialNumber))
                        {
                            eventNeutrons[numEventsThisBlock] = startOfNeutronEventList.eventNeutrons;
                            eventNumNeutrons[numEventsThisBlock] = startOfNeutronEventList.numNeutrons;
                            eventTimes[numEventsThisBlock] = startOfNeutronEventList.eventTime;
                            numEventsThisBlock++;
                            startOfNeutronEventList = startOfNeutronEventList.next;

                        }
                        PermissionToUseEndOfEventsRelease("AH process maximum events");


                        //pass the events to all the RateAnalyzers.
                        //XXX Since now handling blocks, is it time to multithread the RateAnalyzers??
                        for (j = 0; j < numEventsThisBlock; j++)
                        {
                            numRateAnalyzers = rateAnalyzers.Count;
                            for (i = 0; i < numRateAnalyzers; i++)
                            {
                                rateAnalyzers[i].HandleANeutronEvent(eventTimes[j], eventNeutrons[j], eventNumNeutrons[j]);
                            }
                        }


                        //pass the event to all the MultiplicityAnalyzers
                        numMultiplicityFastBackgroundAnalyzers = multiplicityFastBackgroundAnalyzers.Count;
                        for (i = 0; i < numMultiplicityFastBackgroundAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (multiplicityFastBackgroundAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this MultiplicityFastBackgroundAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndFastMultiplicitySpinTime();
#endif

                            //set the input for the ith MultiplicityFastBackgroundAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                multiplicityFastBackgroundAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                multiplicityFastBackgroundAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            multiplicityFastBackgroundAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith MultiplicityFastBackgroundAnalyzer will analyze
                            multiplicityFastBackgroundAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            multiplicityFastBackgroundAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the SlowMultiplicityAnalyzers
                        numMultiplicitySlowBackgroundAnalyzers = multiplicitySlowBackgroundAnalyzers.Count;
                        for (i = 0; i < numMultiplicitySlowBackgroundAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (multiplicitySlowBackgroundAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this MultiplicityFastBackgroundAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndSlowMultiplicitySpinTime();
#endif

                            //set the input for the ith MultiplicitySlowBackgroundAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                multiplicitySlowBackgroundAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                multiplicitySlowBackgroundAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            multiplicitySlowBackgroundAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith MultiplicitySlowBackgroundAnalyzer will analyzer
                            multiplicitySlowBackgroundAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            multiplicitySlowBackgroundAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the FastBackgroundAnalyzers
                        numFastBackgroundAnalyzers = fastBackgroundAnalyzers.Count;
                        for (i = 0; i < numFastBackgroundAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (fastBackgroundAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this FastBackgroundAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndFastBackSpinTime();
#endif
                            //set the input for the ith FastBackgroundAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                fastBackgroundAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                fastBackgroundAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            fastBackgroundAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith FastBackgroundAnalyzer will analyze
                            fastBackgroundAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            fastBackgroundAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the CoincidenceSlowBackgroundAnalyzers
                        numCoincidenceSlowBackgroundAnalyzers = coincidenceSlowBackgroundAnalyzers.Count;
                        for (i = 0; i < numCoincidenceSlowBackgroundAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (coincidenceSlowBackgroundAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this CoincidenceSlowBackgroundAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndSlowMultiplicitySpinTime();
#endif
                            //set the input for the ith CoincidenceSlowBackgroundAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                coincidenceSlowBackgroundAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                coincidenceSlowBackgroundAnalyzers[i].inputEventNeutrons[j] = eventNeutrons[j];
                            }
                            coincidenceSlowBackgroundAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith CoincidenceSlowBackgroundAnalyzer will analyze
                            coincidenceSlowBackgroundAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            coincidenceSlowBackgroundAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the FeynmanAnalyzers
                        numFeynmanAnalyzers = feynmanAnalyzers.Count;
                        for (i = 0; i < numFeynmanAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (feynmanAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this FeynmanAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndFeynmanSpinTime();
#endif
                            //set the input for the ith FeynmanAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                feynmanAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                feynmanAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            feynmanAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith FeynmanAnalyzer will analyze
                            feynmanAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            feynmanAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the RossiAlphaAnalyzers
                        numRossiAlphaAnalyzers = rossiAlphaAnalyzers.Count;
                        for (i = 0; i < numRossiAlphaAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (rossiAlphaAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this RossiAlphaAnalzyer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndRossiAlphaSpinTime();
#endif
                            //set the input for the ith RossiAlphaAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                rossiAlphaAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                rossiAlphaAnalyzers[i].inputEventNeutrons[j] = eventNeutrons[j];
                                rossiAlphaAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            rossiAlphaAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith RossiAlphaAnalyzer will analyze
                            rossiAlphaAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            rossiAlphaAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //pass the event to all the EventSpacingAnalyzers
                        numEventSpacingAnalyzers = eventSpacingAnalyzers.Count;
                        for (i = 0; i < numEventSpacingAnalyzers; i++)
                        {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                            while (eventSpacingAnalyzers[i].isReadyToAnalyze == false)
                            {
                                //spin until this EventSpacingAnalyzer has completed the last neutron it was given
                            }
#if USE_SPINTIME
                            EndEventSpacingSpinTime();
#endif
                            //set the input for the ith EventSpacingAnalyzer
                            for (j = 0; j < numEventsThisBlock; j++)
                            {
                                eventSpacingAnalyzers[i].inputEventTime[j] = eventTimes[j];
                                eventSpacingAnalyzers[i].inputEventNumNeutrons[j] = eventNumNeutrons[j];
                            }
                            eventSpacingAnalyzers[i].numEventsThisBlock = numEventsThisBlock;
                            //clear the thread wait condition, so ith EventSpacingAnalyzer will analyze
                            eventSpacingAnalyzers[i].isReadyToAnalyze = false; //clear the flag because we're about to make this analyzer busy and no longer ready
                            eventSpacingAnalyzers[i].waitingForMessage.Set();  //"put the ticket into the gate," so thread will do its analysis
                        }

                        //FINISHED passing this neutron event to all the analyzers.
                        numNeutronEventsCompleted += (UInt64)numEventsThisBlock;
                        accumuNumNeutronEventsCompleted += (UInt64)numEventsThisBlock;
                        //See if there are any more neutron events
                        PermissionToUseEndOfEventsWait();
                        //see if we have processed the last neutron...
                        if (startOfNeutronEventList.serialNumber == endOfNeutronEventList.serialNumber)
                        {
                            //reset the ManualResetEventSlim, so the thread will wait for an event
                            waitingForEvent.Reset();

                            //if already have been told to stop at the end, then have reached the end so stop now
                            if (AHWorkerStopAtEndOfEvents == true)
                            {
                                AHWorkerStopNow = true;

                                log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler reached end of events and is setting flag to stop its background worker at location #2.");
                            }
                        }

                        PermissionToUseEndOfEventsRelease("AH remaining events processing");
                    }
                }  //END of processing the next neutron
                else if (AHWorkerStopAtEndOfEvents == true)
                {
                    AHWorkerStopNow = true;

                    log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler reached end of events and is setting flag to stop its background worker at location #3.");
                }
            } //END of while (AHWorkerStopNow == false)

            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler is exiting end of events and is waiting for Analyses to complete.");

            //MAKE SURE all analyzers have finished with the last buffer of neutrons before ending this AHWorkerDoWork()
            //Don't have to check RateGateAnalyzers - these are single threaded and have been completed by this thread
            //Check the MultiplicityFastBackgroundAnalyzers
            numMultiplicityFastBackgroundAnalyzers = multiplicityFastBackgroundAnalyzers.Count;
            for (i = 0; i < numMultiplicityFastBackgroundAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (multiplicityFastBackgroundAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this MultiplicityFastBackgroundAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndFastMultiplicitySpinTime();
#endif
            }

            //Check the MultiplicitySlowBackgroundAnalyzers
            numMultiplicitySlowBackgroundAnalyzers = multiplicitySlowBackgroundAnalyzers.Count;
            for (i = 0; i < numMultiplicitySlowBackgroundAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (multiplicitySlowBackgroundAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this MultiplicityFastBackgroundAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndSlowMultiplicitySpinTime();
#endif
            }

            //Check the FastBackgroundAnalyzers
            numFastBackgroundAnalyzers = fastBackgroundAnalyzers.Count;
            for (i = 0; i < numFastBackgroundAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (fastBackgroundAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this FastBackgroundAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndFastBackSpinTime();
#endif
            }

            //Check the CoincidenceSlowBackgroundAnalyzers
            numCoincidenceSlowBackgroundAnalyzers = coincidenceSlowBackgroundAnalyzers.Count;
            for (i = 0; i < numCoincidenceSlowBackgroundAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (coincidenceSlowBackgroundAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this MultiplicityFastBackgroundAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndSlowMultiplicitySpinTime();
#endif
            }

            //Check the FeynmanAnalyzers
            numFeynmanAnalyzers = feynmanAnalyzers.Count;
            for (i = 0; i < numFeynmanAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (feynmanAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this FeynmanAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndFeynmanSpinTime();
#endif
            }

            //Check the RossiAlphaAnalyzers
            numRossiAlphaAnalyzers = rossiAlphaAnalyzers.Count;
            for (i = 0; i < numRossiAlphaAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (rossiAlphaAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this RossiAlphaAnalzyer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndRossiAlphaSpinTime();
#endif
            }

            //Check the EventSpacingAnalyzers
            numEventSpacingAnalyzers = eventSpacingAnalyzers.Count;
            for (i = 0; i < numEventSpacingAnalyzers; i++)
            {
#if USE_SPINTIME
                            StartSpinTime();
#endif
                while (eventSpacingAnalyzers[i].isReadyToAnalyze == false)
                {
                    //spin until this FeynmanAnalyzer has completed the last neutron it was given
                }
#if USE_SPINTIME
                            EndEventSpacingSpinTime();
#endif
            }
            //FINISHED confirming all the analyzers have finished

            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler has completed handling neutrons and is exiting.");
        }  //END of AHWorkerDoWork()

        void AHWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AHWorkerHasCompleted = true;
            FireAnalysesCompletedEvent("list mode analysis completed.");
        }
        #endregion

        #region MakeVariousAnalyzers
        public bool InstantiateFeynmanAnalyzer(UInt64 gateWidthInTics)
        {
            FeynmanGateAnalysis fa = new FeynmanGateAnalysis(gateWidthInTics);
            feynmanAnalyzers.Add(fa);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created FeynmanAnalyzer with gate of " + gateWidthInTics + " tics.");

            return (true);
        }

        public bool InstantiateEventSpacingAnalyzer(UInt64 gateWidthInTics)
        {
            EventSpacingAnalysis esa = new EventSpacingAnalysis(gateWidthInTics);
            eventSpacingAnalyzers.Add(esa);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created EventSpacingAnalyzer with gate of " + gateWidthInTics + " tics.");

            return (true);
        }

        /// <summary>
        /// InstantiateMultiplicityAnalyzerFastBackground() creates a multiplicity analyzer.
        /// gateWidthInTics is the length of the gate for neutron counting, in 100-nSec tics.
        /// preDelayInTics is the time after the trigger until opening the gate for neutron counting, in 100-nSec tics.
        /// Note this MAFB has no longDelayInTics parameter.  Instead, it has a sibling FastBackgroundAnalysis
        /// that does the fast-background calculations for us.  The MAFB object gets a pointer to this sibling FBA.
        /// </summary>
        /// <param name="gateWidthInTics"></param>
        /// <param name="preDelayInTics"></param>
        public bool InstantiateMultiplicityAnalyzerFastBackground(UInt64 gateWidthInTics, UInt64 preDelayInTics,
                                                                  UInt64 backgroundGateTimeStepInTics,
                                                                  double deadTimeCoefficientTinNanoSecs,
                                                                  double deadTimeCoefficientAinMicroSecs,
                                                                  double deadTimeCoefficientBinPicoSecs,
                                                                  double deadTimeCoefficientCinNanoSecs)
        {
            FastBackgroundAnalysis fba;
            MultiplicityAnalysisFastBackground ma;
            int i;

            //set up the Fast-Background Analyzer.
            //see if necessary analyzer is already in the list
            fba = null;
            for (i = 0; i < fastBackgroundAnalyzers.Count; i++)
            {
                if (((fastBackgroundAnalyzers[i].gateWidthInTics / timeBaseConversion) == gateWidthInTics)
                    && ((fastBackgroundAnalyzers[i].gateStepInTics / timeBaseConversion) == backgroundGateTimeStepInTics))
                {
                    fba = fastBackgroundAnalyzers[i];
                    i = fastBackgroundAnalyzers.Count;

                    log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Already have a FastBackgroundAnalyzer with gate of " + gateWidthInTics + " tics and step of " + backgroundGateTimeStepInTics + " tics.");
                }
            }
            if (fba == null)  //...then no previous analyzer, so make one...
            {
                fba = new FastBackgroundAnalysis(gateWidthInTics * timeBaseConversion, backgroundGateTimeStepInTics * timeBaseConversion);
                fastBackgroundAnalyzers.Add(fba);

                log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a FastBackgroundAnalyzer with gate of " + gateWidthInTics + " tics and step of " + backgroundGateTimeStepInTics + " tics.");
            }

            //now make the "foreground" analyzer
            ma = new MultiplicityAnalysisFastBackground(ticSizeInSeconds,
                                                        gateWidthInTics * timeBaseConversion, preDelayInTics * timeBaseConversion,
                                                        fba,
                                                        deadTimeCoefficientTinNanoSecs,
                                                        deadTimeCoefficientAinMicroSecs,
                                                        deadTimeCoefficientBinPicoSecs,
                                                        deadTimeCoefficientCinNanoSecs);
			ma.Log = log;
            multiplicityFastBackgroundAnalyzers.Add(ma);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a MultiplicityFastBGAnalyzer with gate= " + gateWidthInTics
                                                                                             + " tics, predelay= " + preDelayInTics
                                                                                             + " tics, coeffT= " + deadTimeCoefficientTinNanoSecs
                                                                                             + " ns, coeffA= " + deadTimeCoefficientAinMicroSecs
                                                                                             + " us, coeffB= " + deadTimeCoefficientBinPicoSecs
                                                                                             + " ps, coeffC= " + deadTimeCoefficientCinNanoSecs
                                                                                             + " ns");

            return (true);
        }

        /// <summary>
        /// InstantiateMultiplicityAnalyzerSlowBackground() creates a multiplicity analyzer.
        /// gateWidthInTics is the length of the gate for neutron counting, in 100-nSec tics.
        /// preDelayInTics is the time after the trigger until opening the gate for neutron counting, in 100-nSec tics.
        /// </summary>
        /// <param name="gateWidthInTics"></param>
        /// <param name="preDelayInTics"></param>
        /// <param name="accidentalsGateDelayInTics">The so-called "long delay," between the trigger and OPENING of the delayed accidentals gate</param>
        public bool InstantiateMultiplicityAnalyzerSlowBackground(UInt64 gateWidthInTics, UInt64 preDelayInTics, UInt64 accidentalsGateDelayInTics,
                                                                  double deadTimeCoefficientTinNanoSecs,
                                                                  double deadTimeCoefficientAinMicroSecs,
                                                                  double deadTimeCoefficientBinPicoSecs,
                                                                  double deadTimeCoefficientCinNanoSecs)
        {
            MultiplicityAnalysisSlowBackground ma;

            //now make the "foreground" analyzer
            ma = new MultiplicityAnalysisSlowBackground(ticSizeInSeconds,
                                                        gateWidthInTics * timeBaseConversion, preDelayInTics * timeBaseConversion,
                                                        accidentalsGateDelayInTics,
                                                        deadTimeCoefficientTinNanoSecs,
                                                        deadTimeCoefficientAinMicroSecs,
                                                        deadTimeCoefficientBinPicoSecs,
                                                        deadTimeCoefficientCinNanoSecs);
			ma.Log = log;
            multiplicitySlowBackgroundAnalyzers.Add(ma);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a MultiplicitySlowBGAnalyzer with gate= " + gateWidthInTics
                                                                                              + " tics, predelay= " + preDelayInTics
                                                                                              + " tics, coeffT= " + deadTimeCoefficientTinNanoSecs
                                                                                              + " ns, coeffA= " + deadTimeCoefficientAinMicroSecs
                                                                                              + " us, coeffB= " + deadTimeCoefficientBinPicoSecs
                                                                                              + " ps, coeffC= " + deadTimeCoefficientCinNanoSecs
                                                                                              + " ns");

            return (true);
        }

        /// <summary>
        /// InstantiateCoincidenceAnalyzerSlowBackground() creates a coincidence analyzer.
        /// gateWidthInTics is the length of the gate for neutron counting, in 100-nSec tics.
        /// preDelayInTics is the time after the trigger until opening the gate for neutron counting, in 100-nSec tics.
        /// </summary>
        /// <param name="gateWidthInTics"></param>
        /// <param name="preDelayInTics"></param>
        /// <param name="accidentalsGateDelayInTics">The so-called "long delay," between the trigger and OPENING of the delayed accidentals gate</param>
        public bool InstantiateCoincidenceAnalyzerSlowBackground(UInt64 gateWidthInTics, UInt64 preDelayInTics, UInt64 accidentalsGateDelayInTics)
        {
            CoincidenceAnalysisSlowBackground ca;

            ca = new CoincidenceAnalysisSlowBackground(
                 gateWidthInTics * timeBaseConversion, preDelayInTics * timeBaseConversion,
                accidentalsGateDelayInTics, ticSizeInSeconds);
            coincidenceSlowBackgroundAnalyzers.Add(ca);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a CoincidenceSlowBGAnalyzer with gate= " + gateWidthInTics
                                                                                              + " tics, predelay= " + preDelayInTics
                                                                                              + " tics, accidentalsDelay= " + accidentalsGateDelayInTics
                                                                                              + " tics");

            return (true);
        }

        /// <summary>
        /// InstantiateRateAnalyzer() creates a "neutron speedometer" counting the number of neutrons in every gate,
        /// saving data for every such gate.  Num gates can be very long if gateWidthInTics is too short.
        /// Minimum gateWidthInTics is 1000000, or 1 million, representing 0.1 sec.  This value is still awfully short
        /// for experiments that will run for many hours.
        /// </summary>
        /// <param name="gateWidthInTics"></param>
        /// <returns></returns>
        public bool InstantiateRateAnalyzer(UInt64 gateWidthInTics)
        {
            bool result;

            result = true;
            if (gateWidthInTics >= 1000000)  //1e6 tics == 0.1 sec, a default min gate width - still a little too short for cycles lasting days
            {
                RateGateAnalysis ra = new RateGateAnalysis(gateWidthInTics);
                rateAnalyzers.Add(ra);

                log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a RateAnalyzer with gate= " + gateWidthInTics + " tics.");
            }
            else
            {

                log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Could not create a RateAnalyzer with gate= " + gateWidthInTics + " tics.");

                result = false;
            }
            return (result);
        }

        public bool InstantiateRossiAlphaAnalyzer(UInt64 gateWidthInTics)
        {
            RossiAlphaCircularStackAnalysis ra = new RossiAlphaCircularStackAnalysis(gateWidthInTics);
            rossiAlphaAnalyzers.Add(ra);

            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "Created a RossiAlphaAnalyzer with gate= " + gateWidthInTics + " tics.");
            log.TraceEvent(LogLevels.Info, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "The number of RossiAlpha gates is= " + RawAnalysisProperties.numRAGatesPerWindow);

            return (true);
        }

        public int GetNumberOfFeynmanAnalyzers
        {
            get { return feynmanAnalyzers.Count; }
        }

        public int GetNumberOfMultiplicityFastBackgroundAnalyzers()
        {
            int result;
            result = multiplicityFastBackgroundAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfMultiplicitySlowBackgroundAnalyzers()
        {
            int result;
            result = multiplicitySlowBackgroundAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfFastBackgroundAnalyzers()
        {
            int result;
            result = fastBackgroundAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfCoincidenceSlowBackgroundAnalyzers()
        {
            int result;
            result = coincidenceSlowBackgroundAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfRateAnalyzers()
        {
            int result;
            result = rateAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfRossiAlphaAnalyzers()
        {
            int result;
            result = rossiAlphaAnalyzers.Count;
            return (result);
        }

        public int GetNumberOfEventSpacingAnalyzers()
        {
            int result;
            result = eventSpacingAnalyzers.Count;
            return (result);
        }

        #endregion

        #region InputNeutronDataForAnalysis
        public void HandleANeutronEvent(ulong timeOfNewEvent, uint neutronsOfNewEvent)
        {
            uint numNeutrons;

            //count another event received whether processed or not, in case this event has a time that is out of sequence
            numNeutronEventsReceivedWhetherProcessedOrNot++;

            if (verboseTrace) log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler received Neutron event #"
                                                                                              + numNeutronEventsReceivedWhetherProcessedOrNot
                                                                                              + " at Time= " + timeOfNewEvent
                                                                                              + " Neutrons=0x" + String.Format("{0:X8}", neutronsOfNewEvent));

            if (timeOfNewEvent <= timeOfLastNeutronEvent)
            {
                if (timeOfLastNeutronEvent == 0)  //then everything is OK
                {
                }
                else  //then we've received a neutron out of sequence - abort handling this neutron
                {
                    String str;
                    str = "Neutron event #" + numNeutronEventsReceivedWhetherProcessedOrNot + " out of sequence with time " + timeOfNewEvent + " tics";

                    FireNeutronOutOfSequenceErrorEvent(str);

                    log.TraceEvent(LogLevels.Warning, (int)AnalyzerEventCode.AnalyzerHandlerEvent, str);

                    return;  //abort handling this neutron
                }
            }

            //count another event received
            numNeutronEventsReceived++;

            //record time of this event
            timeOfLastNeutronEvent = timeOfNewEvent;


            //WAIT for permission to use the shared end-of-stack object
            PermissionToUseEndOfEventsWait();

            //place the new neutron data in the data-holder at the end of the list
            endOfNeutronEventList.eventTime = timeOfNewEvent;
            endOfNeutronEventList.eventNeutrons = neutronsOfNewEvent;
            numNeutrons = 0;
            while (neutronsOfNewEvent != 0)
            {
                numNeutrons++;
                neutronsOfNewEvent &= (neutronsOfNewEvent - 1);

                //Look at how this works:
                //suppose neutronsOfNewEvent is 01001000, so result should be two.
                //since event is non zero, count 1 bit and set event to
                //   01001000 (neutronsOfNewEvent)
                //  &01000111 (neutronsOfNewEvent-1)
                //  =01000000
                //then still isn't zero, so count 1 more bit (now 2) and set event to
                //   01000000
                //  &00111111
                //  =00000000
                // Done! and result is 2 as desired
            }
            endOfNeutronEventList.numNeutrons = numNeutrons;

            //check to see if the circular linked list would overflow
            ExtendListIfNeeded();

            //move endOfNeutronEventList to the next empty struct
            endOfNeutronEventList = endOfNeutronEventList.next;

            //count the number of times around the circular list
            if (endOfNeutronEventList.serialNumber == 0)
            {
                numCircuits++;
            }

            //unblock the BackgroundWorker which will dispense events to the analyzers
            waitingForEvent.Set();

            PermissionToUseEndOfEventsRelease("HandleANeutronEvent");
            //END OF WAIT to use the shared end-of-stack object
        }

        public void HandleAnArrayOfNeutronEvents(List<ulong> timeOfNewEvents, List<uint> neutronsOfNewEvents, int actualEventCount)
        {
            uint numNeutrons, aNeutronEvent;
            int which, numEvents;

            if (timeOfNewEvents.Count != neutronsOfNewEvents.Count)  // note: new list semantics, this conditional may be unneeded now that the actualEventCount param is in use
            {
                String theProblem = "Array lengths unequal: time[" + timeOfNewEvents.Count + "] neutrons[" + neutronsOfNewEvents.Count + "]";
                FireBlockCountMismatchErrorEvent(theProblem);
                log.TraceEvent(LogLevels.Warning, (int)AnalyzerEventCode.AnalyzerHandlerEvent, theProblem);
                return;
            }
            else if (timeOfNewEvents.Count < 1) // skip these
            {
                log.TraceEvent(LogLevels.Warning, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "skipping empty event buffer");
                return;
            }

            numEvents = actualEventCount; // timeOfNewEvents.Length;

            //count another event received whether processed or not, in case this event has a time that is out of sequence
            numNeutronEventsReceivedWhetherProcessedOrNot += (UInt64)numEvents;

            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler received Neutron array with "
                                                                                                + numEvents
                                                                                                + " events. Time of first event="
                                                                                                + timeOfNewEvents[0]);

            //WAIT for permission to use the shared end-of-stack object
            PermissionToUseEndOfEventsWait();

			InitializeEventList(actualEventCount);

            for (which = 0; which < numEvents; which++)
            {
                if (timeOfNewEvents[which] <= timeOfLastNeutronEvent)
                {
                    if (timeOfLastNeutronEvent == 0)  //then everything is OK
                    {
                    }
                    else  //then we've received a neutron out of sequence - abort handling this neutron
                    {
                        String str;
                        str = "Neutron event #" + numNeutronEventsReceivedWhetherProcessedOrNot + " out of sequence with time " + timeOfNewEvents[which] + " tics & " + timeOfLastNeutronEvent;

                        FireNeutronOutOfSequenceErrorEvent(str);

                        log.TraceEvent(LogLevels.Warning, (int)AnalyzerEventCode.AnalyzerHandlerEvent, str);
                    }
                }
                else //...add this neutron to the list...
                {
                    //count another event received
                    numNeutronEventsReceived++;

                    //record time of this event
                    timeOfLastNeutronEvent = timeOfNewEvents[which];

                    //place the new neutron data in the data-holder at the end of the list
                    aNeutronEvent = neutronsOfNewEvents[which];
                    endOfNeutronEventList.eventTime = timeOfLastNeutronEvent;
                    endOfNeutronEventList.eventNeutrons = aNeutronEvent;
                    numNeutrons = 0;
                    while (aNeutronEvent != 0)
                    {
                        numNeutrons++;
                        aNeutronEvent &= (aNeutronEvent - 1);

                        //Look at how this works:
                        //suppose neutronsOfNewEvent is 01001000, so result should be two.
                        //since event is non zero, count 1 bit and set event to
                        //   01001000 (neutronsOfNewEvent)
                        //  &01000111 (neutronsOfNewEvent-1)
                        //  =01000000
                        //then still isn't zero, so count 1 more bit (now 2) and set event to
                        //   01000000
                        //  &00111111
                        //  =00000000
                        // Done! and result is 2 as desired
                    }
                    endOfNeutronEventList.numNeutrons = numNeutrons;

                    //check to see if the circular linked list would overflow
                    if ((numEvents - which) > numEventsInCircularLinkedList)
						ExtendListIfNeeded();

                    //move endOfNeutronEventList to the next empty struct
                    endOfNeutronEventList = endOfNeutronEventList.next;

                    //count the number of times around the circular list
                    if (endOfNeutronEventList.serialNumber == 0)
                    {
                        numCircuits++;
                    }
                }
            }
			//AHGCCollect();  // needed here at all now after buffer sizing change?

            //unblock the BackgroundWorker which will dispense events to the analyzers
            waitingForEvent.Set();

            PermissionToUseEndOfEventsRelease("HandleAnArrayOfNeutronEvents");
            //END OF WAIT to use the shared end-of-stack object
        }

        void ExtendListIfNeeded()
        {
            if (endOfNeutronEventList.next.serialNumber == startOfNeutronEventList.serialNumber)
            {
                int i;
                NeutronEvent anEvent;
                NeutronEvent nextEvent;

                //if stack would overflow, add RawAnalysisProperties.circularListBlockIncrement new neutron events to this point in the circular linked list
                anEvent = endOfNeutronEventList;
                nextEvent = endOfNeutronEventList.next;
                for (i = 0; i < RawAnalysisProperties.circularListBlockIncrement; i++)
                {
                    anEvent.next = new NeutronEvent(i + numEventsInCircularLinkedList);
                    anEvent = anEvent.next;
                }
                anEvent.next = nextEvent;  //re-close the linked list, pointing the last new event back to the next event in list
                numEventsInCircularLinkedList += RawAnalysisProperties.circularListBlockIncrement;


                log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler increasing CircularArraySize to "
                                                                                                    + numEventsInCircularLinkedList);
            }
        }
		void ExtendListBy(int num)
        {
                int i;
                NeutronEvent anEvent;
                NeutronEvent nextEvent;

                anEvent = endOfNeutronEventList;
                nextEvent = endOfNeutronEventList.next;
                for (i = 0; i < num; i++)
                {
                    anEvent.next = new NeutronEvent(i + numEventsInCircularLinkedList);
                    anEvent = anEvent.next;
                }
                anEvent.next = nextEvent;  //re-close the linked list, pointing the last new event back to the next event in list
                numEventsInCircularLinkedList += num;
				endOfNeutronEventList = anEvent;
				startOfNeutronEventList = theEventCircularLinkedList;
                log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler extending CircularArraySize by " + num + " to "
                                                                                                    + numEventsInCircularLinkedList);
        }
        #endregion

        public AnalyzerHandlerStatus GetStatus()
        {
            AnalyzerHandlerStatus status = new AnalyzerHandlerStatus();

            // dev note: let's find more states here, this is too coarse for informational RT reporting 
            // e.g. the state per analyzer instance might be useful to see which one is running behind (using a a verbose display update selection)
            if (AHWorkerHasCompleted == true)
            {
                status.presentStatus = StatusCode.FinishedProcessingNeutrons;
            }
            else if (waitingForEvent.IsSet == true)
            {
                status.presentStatus = StatusCode.NowProcessingNeutrons;
            }
            else
            {
                status.presentStatus = StatusCode.WaitingForNeutrons;
            }

            //get numbers of the various analyzers
            status.numFeynmanAnalyzers = feynmanAnalyzers.Count;
            status.numMultiplicityFastBackgroundAnalyzers = multiplicityFastBackgroundAnalyzers.Count;
            status.numMultiplicitySlowBackgroundAnalyzers = multiplicitySlowBackgroundAnalyzers.Count;
            status.numRossiAlphaAnalyzers = rossiAlphaAnalyzers.Count;
            status.numRateAnalyzers = rateAnalyzers.Count;
            status.numEventSpacingAnalyzers = eventSpacingAnalyzers.Count;
            status.numCoincidenceSlowBackgroundAnalyzers = coincidenceSlowBackgroundAnalyzers.Count;

            //get technical-performance info
            status.numNeutronEventsReceived = numNeutronEventsReceived;
            status.numNeutronEventsProcessed = numNeutronEventsCompleted;
            status.accumuNumNeutronEventsCompleted = accumuNumNeutronEventsCompleted;
            status.capacityOfQueue = (UInt64)numEventsInCircularLinkedList;
            status.numCircuits = numCircuits;
            status.numNeutronEventsReceivedWhetherProcessedOrNot = numNeutronEventsReceivedWhetherProcessedOrNot;

            return (status);
        }

        #region GetVariousResults

        //warning: must return something, and probably one get-result per analyzer type...
        public AnalyzerHandlerResult GetResults()
        {
            int i;
            AnalyzerHandlerResult results = new AnalyzerHandlerResult();

            results.numFeynmanAnalyzers = feynmanAnalyzers.Count;
            results.numFastBackgroundMultiplicityAnalyzers = multiplicityFastBackgroundAnalyzers.Count;
            results.numSlowBackgroundMultiplicityAnalyzers = multiplicitySlowBackgroundAnalyzers.Count;
            results.numRateAnalyzers = rateAnalyzers.Count;
            results.numRossiAlphaAnalyzers = rossiAlphaAnalyzers.Count;
            results.numEventSpacingAnalyzers = eventSpacingAnalyzers.Count;
            results.numCoincidenceSlowBackgroundAnalyzers = coincidenceSlowBackgroundAnalyzers.Count;

            //pause until the current AHWorker is done ...
            while (AHWorker.IsBusy == true)
            {
            }

            if (results.numFeynmanAnalyzers > 0)
            {
                results.feynmanResults = new AnalysisDefs.FeynmanResult[results.numFeynmanAnalyzers];
                for (i = 0; i < results.numFeynmanAnalyzers; i++)
                {
                    results.feynmanResults[i] = feynmanAnalyzers[i].GetResult();
                }
            }

            if (results.numFastBackgroundMultiplicityAnalyzers > 0)
            {
                results.multiplicityFastBackgroundResults = new AnalysisDefs.MultiplicityResult[results.numFastBackgroundMultiplicityAnalyzers];
                for (i = 0; i < results.numFastBackgroundMultiplicityAnalyzers; i++)
                {
                    results.multiplicityFastBackgroundResults[i] = multiplicityFastBackgroundAnalyzers[i].GetResult();
                    foreach (string s in results.multiplicityFastBackgroundResults[i].warnings)
                        log.TraceEvent(LogLevels.Warning, 8642, "(MFA " + i + ") " + s);
                }
            }

            if (results.numSlowBackgroundMultiplicityAnalyzers > 0)
            {
                results.multiplicitySlowBackgroundResults = new AnalysisDefs.MultiplicityResult[results.numSlowBackgroundMultiplicityAnalyzers];
                for (i = 0; i < results.numSlowBackgroundMultiplicityAnalyzers; i++)
                {
                    results.multiplicitySlowBackgroundResults[i] = multiplicitySlowBackgroundAnalyzers[i].GetResult();
                    foreach (string s in results.multiplicitySlowBackgroundResults[i].warnings)
                        log.TraceEvent(LogLevels.Warning, 8643, "(M " + i + ") " + s);
                }
            }

            if (results.numCoincidenceSlowBackgroundAnalyzers > 0)
            {
                results.coincidenceSlowBackgroundResults = new AnalysisDefs.CoincidenceResult[results.numCoincidenceSlowBackgroundAnalyzers];
                for (i = 0; i < results.numCoincidenceSlowBackgroundAnalyzers; i++)
                {
                    results.coincidenceSlowBackgroundResults[i] = coincidenceSlowBackgroundAnalyzers[i].GetResult();

                }
            }

            if (results.numRateAnalyzers > 0)
            {
                results.rateResults = new AnalysisDefs.RateResult[results.numRateAnalyzers];
                for (i = 0; i < results.numRateAnalyzers; i++)
                {
                    results.rateResults[i] = rateAnalyzers[i].GetResult(log);
                }
            }

            if (results.numRossiAlphaAnalyzers > 0)
            {
                results.rossiAlphaResults = new AnalysisDefs.RossiAlphaResult[results.numRossiAlphaAnalyzers];
                for (i = 0; i < results.numRossiAlphaAnalyzers; i++)
                {
                    results.rossiAlphaResults[i] = rossiAlphaAnalyzers[i].GetResult();
                }
            }

            if (results.numEventSpacingAnalyzers > 0)
            {
                results.eventSpacingResults = new AnalysisDefs.EventSpacingResult[results.numEventSpacingAnalyzers];
                for (i = 0; i < results.numEventSpacingAnalyzers; i++)
                {
                    results.eventSpacingResults[i] = eventSpacingAnalyzers[i].GetResult();
                }
            }

            return (results);
        }

        public AnalysisDefs.FeynmanResult GetFeynmanResult(int whichFeynmanAnalyzer)
        {
            AnalysisDefs.FeynmanResult result;
            if (whichFeynmanAnalyzer < feynmanAnalyzers.Count)
            {
                result = feynmanAnalyzers[whichFeynmanAnalyzer].GetResult();
            }
            else
            {
                result = null;
            }

            return (result);
        }

        /// <summary>
        /// GetFeynmanResultFromDictionary() produces a normal FeynmanResult object, but uses
        /// a user-supplied NumNeutrons+NumGates histogram,
        /// feynmanGateDictionary<UInt32 numNeutrons, UInt32 numGatesWithThatManyNeutrons>,
        /// as the basis for this calculation.
        /// </summary>
        /// <param name="whichFeynmanAnalyzer">The integer index of one of this AnalyzerHandler's list of Feynman analyzers.
        /// Note that this parameter matters in setting the FeynmanResult.gateWidth member, otherwise the return value
        /// is independent of which analyzer.</param>
        /// <param name="feynmanGateDictionary">The user-supplied histogram feynmanGateDictionary<UInt32 numNeutrons, UInt32 numGatesWithThatManyNeutrons>.</param>
        /// <returns>The FeynmanResult object based upon the user-supplied histogram dictionary.</returns>
        public AnalysisDefs.FeynmanResult GetFeynmanResultFromDictionary(int whichFeynmanAnalyzer, Dictionary<UInt32, UInt32> feynmanGateDictionary)
        {
            AnalysisDefs.FeynmanResult result;

            result = null;
            if (whichFeynmanAnalyzer < feynmanAnalyzers.Count)
            {
                result = FeynmanGateAnalysis.GetResultFromFeynmanGateDictionary(feynmanGateDictionary, true);
                result.gateWidth = feynmanAnalyzers[whichFeynmanAnalyzer].gateWidthInTics;
            }
            return (result);
        }

        public AnalysisDefs.EventSpacingResult GetEventSpacingResult(int whichEventSpacingAnalyzer)
        {
            AnalysisDefs.EventSpacingResult result;
            if (whichEventSpacingAnalyzer < eventSpacingAnalyzers.Count)
            {
                result = eventSpacingAnalyzers[whichEventSpacingAnalyzer].GetResult();
            }
            else
            {
                result = null;
            }

            return (result);
        }

        public AnalysisDefs.MultiplicityResult GetMultiplicityFastBackgroundResult(int whichMultiplicityFastBackgroundAnalyzer)
        {
            AnalysisDefs.MultiplicityResult result;

            if (whichMultiplicityFastBackgroundAnalyzer < multiplicityFastBackgroundAnalyzers.Count)
            {
                result = multiplicityFastBackgroundAnalyzers[whichMultiplicityFastBackgroundAnalyzer].GetResult();
            }
            else
            {
                result = null;
            }

            return (result);
        }

        public AnalysisDefs.MultiplicityResult GetMultiplicitySlowBackgroundResult(int whichMultiplicitySlowBackgroundAnalyzer)
        {
            AnalysisDefs.MultiplicityResult result;

            if (whichMultiplicitySlowBackgroundAnalyzer < multiplicitySlowBackgroundAnalyzers.Count)
            {
                result = multiplicitySlowBackgroundAnalyzers[whichMultiplicitySlowBackgroundAnalyzer].GetResult();
            }
            else
            {
                result = null;
            }

            return (result);
        }

        public AnalysisDefs.CoincidenceResult GetCoincidenceSlowBackgroundResult(int whichCoincidenceSlowBackgroundAnalyzer)
        {
            AnalysisDefs.CoincidenceResult result;

            result = null;

            if (whichCoincidenceSlowBackgroundAnalyzer < coincidenceSlowBackgroundAnalyzers.Count)
            {
                result = coincidenceSlowBackgroundAnalyzers[whichCoincidenceSlowBackgroundAnalyzer].GetResult();
            }

            return (result);
        }

        public AnalysisDefs.RateResult GetRateResult(int whichRateAnalyzer)
        {
            AnalysisDefs.RateResult result;
            if (whichRateAnalyzer < rateAnalyzers.Count)
            {
                result = rateAnalyzers[whichRateAnalyzer].GetResult(log);
            }
            else
            {
                result = null;
            }

            return (result);
        }

        public AnalysisDefs.RossiAlphaResult GetRossiAlphaResult(int whichRossiAlphaAnalyzer)
        {
            AnalysisDefs.RossiAlphaResult result;
            if (whichRossiAlphaAnalyzer < rossiAlphaAnalyzers.Count)
            {
                result = rossiAlphaAnalyzers[whichRossiAlphaAnalyzer].GetResult();
            }
            else
            {
                result = null;
            }

            return (result);
        }
        #endregion

        #region CommandAnalyzersToStop
        public void EndAnalysisImmediately()
        {
            AHWorkerStopNow = true;
            waitingForEvent.Set();
            PermissionToUseEndOfEventsRelease("EndAnalysisImmediately");  // LMKV-52: must release this too, but only once
            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler received instruction to stop immediately.");
        }

        public void EndAnalysisWhenFinishedWithPresentEventQueue()
        {
            AHWorkerStopAtEndOfEvents = true;
            waitingForEvent.Set();
            log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler received instruction to stop at end of events.");
        }
        #endregion

        #region ResetAnalyzers
        /// <summary>
        /// ResetCompletely() clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
            int i;
            NeutronEvent thisEvent, nextEvent;

            //pause until the current AHWorker is done ...
            while (AHWorker.IsBusy == true)
            {
            }

            //reset all the analyzers
            for (i = 0; i < feynmanAnalyzers.Count; i++)
            {
                feynmanAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < multiplicityFastBackgroundAnalyzers.Count; i++)
            {
                multiplicityFastBackgroundAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < multiplicitySlowBackgroundAnalyzers.Count; i++)
            {
                multiplicitySlowBackgroundAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < coincidenceSlowBackgroundAnalyzers.Count; i++)
            {
                coincidenceSlowBackgroundAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < rateAnalyzers.Count; i++)
            {
                rateAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < rossiAlphaAnalyzers.Count; i++)
            {
                rossiAlphaAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < fastBackgroundAnalyzers.Count; i++)
            {
                fastBackgroundAnalyzers[i].ResetCompletely(closeCounters);
            }
            for (i = 0; i < eventSpacingAnalyzers.Count; i++)
            {
                eventSpacingAnalyzers[i].ResetCompletely(closeCounters);
            }

			InitCounters();

#if USE_SPINTIME
            ResetSpinTime();
#endif

            //break the circular references, so garbage collector will do its thing
			if (closeCounters)
			{
				nextEvent = theEventCircularLinkedList;
				while (nextEvent != null)
				{
					thisEvent = nextEvent;
					nextEvent = thisEvent.next;
					thisEvent.next = null;
				}
				thisEvent = null;
				theEventCircularLinkedList = null;
				startOfNeutronEventList = null;
				endOfNeutronEventList = null;
			}

            // selectively invoke the garbage collector now
            AHGCCollect();

			if (closeCounters)
				return;

            //create stack of RawAnalysisProperties.circularListBlockIncrement neutron events, as a starting point
            //InitializeEventList(RawAnalysisProperties.circularListBlockIncrement);

            //set up the AH BackgroundWorker
            waitingForEvent.Reset();
            AHWorkerStopNow = false;
            AHWorkerHasCompleted = false;
            AHWorkerStopAtEndOfEvents = false;
            //pause until the previous AHWorker is done ...
            while (AHWorker.IsBusy == true)
            {
            }
            AHWorker = new BackgroundWorker();
            AHWorker.WorkerSupportsCancellation = false;  //parent will stop the worker with the boolean flags
            AHWorker.DoWork += new DoWorkEventHandler(AHWorkerDoWork);
            AHWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AHWorkerRunWorkerCompleted);
            AHWorker.RunWorkerAsync();

            //pause until the AHWorker is working...
            while (AHWorker.IsBusy == false)
            {
            }
        }


        ///
        void InitializeEventList(int num)
        {
            AHGCCollect();
            if (numEventsInCircularLinkedList == 0 || theEventCircularLinkedList == null)
			{
				//create stack of RawAnalysisProperties.circularListBlockIncrement neutron events, as a starting point
				theEventCircularLinkedList = new NeutronEvent(0);  //make the first event in the list
				startOfNeutronEventList = theEventCircularLinkedList;     //set pointer to start of list to this first event
				endOfNeutronEventList = theEventCircularLinkedList;       //set pointer to end of list to this first event
				for (int i = 1; i < num; i++)
				{
					endOfNeutronEventList.next = new NeutronEvent(i);  //after the present end of list, make a new event
					endOfNeutronEventList = endOfNeutronEventList.next;       //move the pointer to the new end of list
				}
				endOfNeutronEventList.next = startOfNeutronEventList;  //close the circular linked list, joining the end to the beginning
				//endOfNeutronEventList = startOfNeutronEventList;  //now both the start and end point to the first empty structure
				numEventsInCircularLinkedList = num;

				log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler new "
                                                                                                    + numEventsInCircularLinkedList);
			}
			else if (numEventsInCircularLinkedList < num) // extend
			{
				ExtendListBy(num - numEventsInCircularLinkedList);
			}
			else if (numEventsInCircularLinkedList > num) // clear it, shrink it
			{
				startOfNeutronEventList = theEventCircularLinkedList;     //set pointer to start of list to this first event
				endOfNeutronEventList = theEventCircularLinkedList;       //set pointer to end of list to this first event
				theEventCircularLinkedList.Set(0);
				NeutronEvent ende = startOfNeutronEventList, two = startOfNeutronEventList;
				for (int i = 0; i < num; i++)
				{
					ende.Set(i);
					two = ende;
					ende = ende.next;
				}
				ende = two; // move back 1
				endOfNeutronEventList = ende;
				endOfNeutronEventList.next = startOfNeutronEventList;  //close the circular linked list, joining the end to the beginning
				//endOfNeutronEventList = startOfNeutronEventList;  //now both the start and end point to the first empty structure

				log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler clear "
                                                             + numEventsInCircularLinkedList + " (remove " + (numEventsInCircularLinkedList - num) + ")");

				numEventsInCircularLinkedList = num;
			} 
			else // same size just clear it
			{
				theEventCircularLinkedList.Set(0);
				NeutronEvent ende = startOfNeutronEventList.next;
				for (int i = 0; i < num; i++)
				{
					ende.Set(i);
					ende = ende.next;
				}
				log.TraceEvent(LogLevels.Verbose, (int)AnalyzerEventCode.AnalyzerHandlerEvent, "AnalyzerHandler clear same size " + numEventsInCircularLinkedList);
			}
            AHGCCollect();
        }

        /// <summary>
        /// Do a full GC.Collect, but only if the current allocated memory size exceeds a certain ceiling, 512Mb default)
        /// </summary>
        /// <param name="MbCeiling"></param>
        void AHGCCollect(long MbCeiling = 257)
        {
            long mem = GC.GetTotalMemory(false);
            log.TraceEvent(LogLevels.Verbose, 4255, "Total GC Memory now {0:N0}Kb", mem / 1024L);
            if (mem > MbCeiling * (1024 * 1024)) // default is like a rabbit, pulled out of my hat and should be a config value
            {
                log.TraceEvent(LogLevels.Verbose, 4248, "GC now");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                log.TraceEvent(LogLevels.Verbose, 4284, "GC complete {0:N0}Kb", GC.GetTotalMemory(true) / 1024L);
            }
        }

        /// <summary>
        /// ResetForConcatenation() clears metadata for a new experiment,
        /// but preserves present histogram array so subsequent data can be added.
        /// </summary>
        public void ResetForConcatenation()
        {
            int i;
            //pause until the current AHWorker is done ...
            // dev note: instead of busy waiting like this, consider using a synchronization point, e.g. a semaphore
            while (AHWorker.IsBusy == true)
            {
            }
            //reset all the analyzers
            for (i = 0; i < feynmanAnalyzers.Count; i++)
            {
                feynmanAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < multiplicityFastBackgroundAnalyzers.Count; i++)
            {
                multiplicityFastBackgroundAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < multiplicitySlowBackgroundAnalyzers.Count; i++)
            {
                multiplicitySlowBackgroundAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < coincidenceSlowBackgroundAnalyzers.Count; i++)
            {
                coincidenceSlowBackgroundAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < rateAnalyzers.Count; i++)
            {
                rateAnalyzers[i].ResetCompletely(false);  //rate analyzers can't be reset for concatenation - just start over
            }
            for (i = 0; i < rossiAlphaAnalyzers.Count; i++)
            {
                rossiAlphaAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < fastBackgroundAnalyzers.Count; i++)
            {
                fastBackgroundAnalyzers[i].ResetForConcatenation();
            }
            for (i = 0; i < eventSpacingAnalyzers.Count; i++)
            {
                eventSpacingAnalyzers[i].ResetForConcatenation();
            }

            numNeutronEventsReceived = 0;
            numNeutronEventsReceivedWhetherProcessedOrNot = 0;
            numNeutronEventsCompleted = 0;
            numCircuits = 0;

            timeOfLastNeutronEvent = 0;

#if USE_SPINTIME
            ResetSpinTime();
#endif

            //create stack of RawAnalysisProperties.circularListBlockIncrement neutron events, as a starting point
            //InitializeEventList(RawAnalysisProperties.circularListBlockIncrement);

            //set up the AH BackgroundWorker
            waitingForEvent.Reset();
            AHWorkerStopNow = false;
            AHWorkerHasCompleted = false;
            AHWorkerStopAtEndOfEvents = false;
            AHWorker = new BackgroundWorker();
            AHWorker.WorkerSupportsCancellation = false;  //parent will stop the worker with the boolean flags
            AHWorker.DoWork += new DoWorkEventHandler(AHWorkerDoWork);
            AHWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AHWorkerRunWorkerCompleted);
            AHWorker.RunWorkerAsync();

            //pause until the AHWorker is working...
            while (AHWorker.IsBusy == false)
            {
            }
        }
        #endregion
    }

}
