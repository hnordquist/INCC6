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
using System.Threading;
using AnalysisDefs;
using NCCReporter;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Analysis
{

    using NC = NCC.CentralizedState;


    public class SRProcessingState : ProcessingState
    {

        public DAQ.SRControl SRCtrl;


        internal SRProcessingState()
        {
            StartCycle(null);
            assayPending = new ManualResetEventSlim(false);

                

        }

        public override void StartCycle(Cycle cycle, object param = null)
        {
            base.StartCycle(cycle, param);
            ResetProcessingCounters();
        }
        
        //    LMDAQ.DAQControl.gControl.SRWrangler.GetResults();
        //}
        
        public override bool TransferResults(CountingAnalysisParameters ap)
        {
            // accumulate totals on the cycle object from the time/hits processor
            AccumulateCycleSummary();  // has an effect only if called after StartNewBuffer and additional data has arrived during input processing
            return true;
        }
        

        // copy accumulated data to cycle object and reset for next buffers-worth
        public void StartNewBuffer()
        {
            // accumulate totals and current raw rate on the current cycle
            AccumulateCycleSummary();

            // reset the buffer counters
            ResetProcessingCounters();
        }

        // reset for next buffers-worth
        public override void ResetProcessingCounters()
        {
            base.ResetProcessingCounters();
        }

        public override void AccumulateCycleSummary() // accumulate any remaining data on the cycle object from the time/hits processor
        {
            base.AccumulateCycleSummary();
        }

        public override StreamStatusBlock ConvertDataBuffer(int bytecount)
        {
            return null;
        }
    }

    public class SRCycleDataTransform : RDTBase
    {
        private UdpClient sender;
        public new ProcessingState State
        {
            get { return state as ProcessingState; }
        }


        /// <summary>
        /// The current Cycle instance undergoing processing
        /// </summary>
        public override Cycle Cycle
        {
            get { return state.cycle; }
            set { state.cycle = value; }
        }

        public DAQ.SRControl SRCtrl
        {
            get { return (state as SRProcessingState).SRCtrl; }
            set { (state as SRProcessingState).SRCtrl = value; }
        }

        public override void EndAnalysisImmediately()
        {
        }

        public SRCycleDataTransform()
        {
            state = new SRProcessingState();
        }

        public override void Init(LMLoggers.LognLM datalogger, LMLoggers.LognLM alogger)
        {
            base.Init(datalogger, alogger);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="meas"></param>
        /// <returns></returns>
        public override bool EndOfCycleProcessing(Measurement meas, bool last = false)
        {
            bool happyfun = true;

            if (NC.App.Opstate.SOH == NCC.OperatingState.Living) // all other states mean do not wait here
            {
                // drain analyzer queue
                EndCountingWhenFinishedWithPresentEventQueue();
                // wait for results to be available
                AssayPendingWait();
            }
            else
            {
                logger.TraceEvent(LogLevels.Warning, 952, "Processing is '{0}', not waiting for analyzer completion", NC.App.Opstate.SOH);
                EndAnalysisImmediately(); // again ok?
            }

            // the transformed results are already resident on each SR thread controller object 

            try
            {

                //HV Plateau does not have a cycle to do this with. HN 3/15/2017
                if (NC.App.Opstate.Action.Equals(NCC.NCCAction.HVCalibration))
                {
                    state.cycle = new Cycle(NC.App.Loggers.AppLogger);
                }
                // not much for this subclass
                happyfun = state.TransferResults(meas.AnalysisParams);
                Cycle.CountingAnalysisResults.Add(SRCtrl.Det.MultiplicityParams, SRCtrl.TransformedResults);

            }
            catch (System.Exception ex)
            {
                logger.TraceEvent(LogLevels.Error, 142, "EndOfCycleProcessing result transfer internal error: " + ex.Message);
                happyfun = false;
            }
            //try
            //{
            //    // reset the counters 
            //    State.Sup.ResetCompletely();
            //}
            //catch (Exception ex)
            //{
            //    logger.TraceEvent(LogLevels.Error, 143, "EndOfCycleProcessing reset internal error: " + ex.Message);
            //    happyfun = false;
            //}
            try
            {
                state.cycle.SetDatastreamEndStatus();
                if (!NC.App.Opstate.Action.Equals(NCC.NCCAction.HVCalibration))
                    CycleProcessing.ApplyTheCycleConditioningSteps(state.cycle, meas);
                meas.CycleStatusTerminationCheck(state.cycle);

                using (sender = new UdpClient(19999))
                {
                    string s = "Singles: " + state.cycle.SinglesRate + "Doubles: " + state.cycle.SinglesRate + "\r\n";
                    sender.Send(Encoding.ASCII.GetBytes(s), s.Length, "localhost", 20000);
                }
                sender.Close();
                FlushCycleSummaryResults();
            }
            catch (System.Exception ex)
            {
                logger.TraceEvent(LogLevels.Error, 144, "EndOfCycleProcessing summary internal error: " + ex.Message);
            }

            return happyfun;
        }

        public void EndCountingWhenFinishedWithPresentEventQueue()
        {
        }

        public override void AssayPendingComplete()
        {
            state.assayPending.Set();
        }
        public override bool AssayPendingWait()
        {
            state.assayPending.Wait();
            return true;
        }
        public override void AssayPendingReset()
        {
            state.assayPending.Reset();
        }
        public override bool AssayPendingIsSet()
        {
            return state.assayPending.IsSet;
        }
        public override ManualResetEventSlim GetManualResetEvent()
        {
            return state.assayPending;
        }

        public override void StartCycle(Cycle cycle, object param = null)
        {
            state.StartCycle(cycle);
        }
    } // SRCycleDataTransform



}