/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using AnalysisDefs;
using NCCReporter;
namespace Analysis
{
    using RawAnalyzerHandler = LMRawAnalysis.AnalyzerHandler;
    using RawResults = LMRawAnalysis.AnalyzerHandlerResult;
    using RawStatus = LMRawAnalysis.AnalyzerHandlerStatus;

    public enum CounterType { BaseRate, Coincidence, TimeInterval, Multiplicity, MultiplicityFastAccidentals, Rossi, Feynman };

    // todo: deal with hard-coded limits of 998 or 1000 code in the LJD analyzer code, need an exposed settable property
    public class Supporter
    {

        // wrapper over LJD analyzer code for now
        public Supporter()
        {
            status = new RawStatus();
        }

        ~Supporter()
        {
            logger = null;
            handler = null;
            results = null;
        }

        LMLoggers.LognLM logger;
        RawAnalyzerHandler handler;
        RawResults results;

        public RawResults Results
        {
            get { return results; }
        }
        RawStatus status;

        public void Construct(RawAnalyzerHandler.AnalysesCompleted f,
                        RawAnalyzerHandler.NeutronOutOfSequenceErrorEvent f2,
                        RawAnalyzerHandler.BlockCountMismatchErrorEvent f3,
                        double theTicSizeInSeconds)
        {
            handler = new RawAnalyzerHandler(theTicSizeInSeconds:theTicSizeInSeconds,logger:logger);
            handler.OnAnalysesCompleted += f;
            handler.OnNeutronOutOfSequenceErrorEvent += f2;
            handler.OnBlockCountMismatchErrorEvent += f3;
        }

        public void ResetTickSizeInSeconds(double theTicSizeInSeconds)
        {
			handler.TickSizeInSeconds = theTicSizeInSeconds;
        }

        public bool AddRates(ulong gateWidthTics)
        {
            bool good;
            good = handler.InstantiateRateAnalyzer(gateWidthTics);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1502, "Rate analyzer (at {0} ticks) creation failed, dunno why", gateWidthTics);
            return good;
        }

        public bool AddMultiplicity(Multiplicity m, FAType fa)
        {
            bool good;
            if (FAType.FAOn == fa)
                good = handler.InstantiateMultiplicityAnalyzerFastBackground(m.SR.gateLength, m.SR.predelay, m.BackgroundGateTimeStepInTics, m.SR.deadTimeCoefficientTinNanoSecs, m.SR.deadTimeCoefficientAinMicroSecs, m.SR.deadTimeCoefficientBinPicoSecs, m.SR.deadTimeCoefficientCinNanoSecs);
            else
                good = handler.InstantiateMultiplicityAnalyzerSlowBackground(m.SR.gateLength, m.SR.predelay, m.AccidentalsGateDelayInTics, m.SR.deadTimeCoefficientTinNanoSecs, m.SR.deadTimeCoefficientAinMicroSecs, m.SR.deadTimeCoefficientBinPicoSecs, m.SR.deadTimeCoefficientCinNanoSecs);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1501, "Multiplicity analyzer creation failed, que no? {0}", m.SR.ToString());  // todo: implement a nice ToString for ShiftRegisterParameters
            return good;
        }

        public bool AddFeynman(ulong gateWidthTics)
        {
            bool good;
            good = handler.InstantiateFeynmanAnalyzer(gateWidthTics);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1503, "Feynman analyzer (at {0} ticks) creation failed, dunno why", gateWidthTics);
            return good;
        }

        public bool AddRossi(ulong gateWidthTics)
        {
            bool good;
            good = handler.InstantiateRossiAlphaAnalyzer(gateWidthTics);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1504, "Rossi analyzer (at {0} ticks) creation failed, dunno why", gateWidthTics);

            return good;
        }
        public bool AddTimeInterval(ulong gateWidthTics)
        {
            bool good;
            good = handler.InstantiateEventSpacingAnalyzer(gateWidthTics);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1505, "TimeInterval analyzer (at {0} ticks) creation failed, dunno why", gateWidthTics);
            return good;
        }

         public bool AddCoincidenceMatrix(Coincidence co)
        {
            bool good;
            good = handler.InstantiateCoincidenceAnalyzerSlowBackground(co.SR.gateLength, co.SR.predelay, co.AccidentalsGateDelayInTics);
            if (!good)
                logger.TraceEvent(LogLevels.Warning, 1506, "CoincidenceMatrix analyzer trouble, que no? ", co.SR.ToString());
            return good;
        }

        //[inline]
        public void HandleANeutronEvent(UInt64 timeOfNewEvent, UInt32 neutronsOfNewEvent)
        {
            handler.HandleANeutronEvent(timeOfNewEvent, neutronsOfNewEvent);
        }

        public void HandleAnArrayOfNeutronEvents(List<ulong> timeOfNewEvents, List<uint> neutronsOfNewEvents, int actualEventCount)
        {
			handler.HandleAnArrayOfNeutronEvents(timeOfNewEvents, neutronsOfNewEvents, actualEventCount);
        }

        // dev note: this can only be called ONCE in response to AnalyzerHandler.NeutronOutOfSequenceErrorEvent or AnalyzerHandler.BlockCountMismatchErrorEvent
        public void EndCountingImmediately()
        {
            try
            {
                handler.EndAnalysisImmediately();
            }
            catch (Exception x)
            {
                logger.TraceEvent(LogLevels.Error, 1013, "Exception stopping analysis " + x.Message);
                logger.TraceException(x, true);
            }
        }

        public void EndCountingWhenFinishedWithPresentEventQueue()
        {
            handler.EndAnalysisWhenFinishedWithPresentEventQueue();
        }

        public void ResetCompletely(bool closeCounters)
        {
            handler.ResetCompletely(closeCounters);
            if (!closeCounters)
                logger.TraceEvent(LogLevels.Verbose, 1011, "Reset and prepared analysers for the next cycle");
            else
                logger.TraceEvent(LogLevels.Verbose, 1012, "Reset analyzers after last cycle");
        }

        public void GetCountingResults()
        {
            try
            {
                results = handler.GetResults();
            }
            catch (Exception x)
            {
                logger.TraceEvent(LogLevels.Error, 1010, "Exception getting results " + x.Message);
                logger.TraceException(x, true);
            }
        }

        // dev note: rewrite raw data counters "the analyzer handlers" to use the existing .NET thread progress and cancellation callbacks
        // e.g. ProgressChangedEventHandler, ReportProgress, OnProgressChanged, CancelAsync, WorkerSupportsCancellation
        public void GetCountingProcessorStatus()
        {
            if (handler != null)
                status = handler.GetStatus();
        }

        public RawAnalyzerHandler Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        public RawStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public void SetLogger(LMLoggers.LognLM analogger)
        {
            logger = analogger;
        }


        // dev note: use iterators over the arrays to hide the array-ness


        public FeynmanResult GetIthFeynmanResult(int i)
        {
            if (results.numFeynmanAnalyzers > 0)
                return results.feynmanResults[i];
            else
                return null;
        }


        public MultiplicityResult GetIthMultiplicityResult(FAType fa, int i)
        {
            if (fa == FAType.FAOn)
            {
                if (results.numFastBackgroundMultiplicityAnalyzers > 0)
                    return results.multiplicityFastBackgroundResults[i];
                else
                    return null;
            }
            else if (fa == FAType.FAOff)
            {
                if (results.numSlowBackgroundMultiplicityAnalyzers > 0)
                    return results.multiplicitySlowBackgroundResults[i];
                else
                    return null;
            }
            return null;
        }

        public RateResult GetIthRateResult(int i)
        {
            if (results.numRateAnalyzers > 0)
                return results.rateResults[i];
            else
                return null;
        }
        
        public CoincidenceResult GetIthCoincidenceMatrixResult(int i)
        {
            if (results.numCoincidenceSlowBackgroundAnalyzers > 0)
                return results.coincidenceSlowBackgroundResults[i];
            else
                return null;
        }

        public RossiAlphaResult GetIthRossiAlphaResult(int i)
        {
            if (results.numRossiAlphaAnalyzers > 0)
                return results.rossiAlphaResults[i];
            else
                return null;
        }
        public EventSpacingResult GetIthTimeIntervalResult(int i)
        {
            if (results.numEventSpacingAnalyzers > 0)
                return results.eventSpacingResults[i];
            else
                return null;
        }
    }

}




