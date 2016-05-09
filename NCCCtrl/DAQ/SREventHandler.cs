/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Collections;
using NCCReporter;
using Instr;
namespace DAQ
{


    public partial class DAQControl
    {

        // register this using StartSRControl
        public static void SREventHandler(object sender, SRTakeDataHandler.SROpCompletedEventArgs e)
        {
            SRControl sr = (SRControl)e.UserState;
            // SRTakeDataHandler.SROp next = SRTakeDataHandler.NextOp(e.Op, e.OpStatus);
            sr.Log.TraceEvent(LogLevels.Verbose, 87266, "SR {0} operation {1} completed '{2}'",
                sr.Identifier, e.Op, INCCSR.SRAPIReturnStatusCode(e.OpStatus));
            try
            {
                switch (e.Op)
                {
                    case SRTakeDataHandler.SROp.Shutdown:

                        break;
                    case SRTakeDataHandler.SROp.Nothing:

                        break;
                    case SRTakeDataHandler.SROp.InitializeContext:

                        break;
                    case SRTakeDataHandler.SROp.InitializeSR:
                    case SRTakeDataHandler.SROp.ReInitializeSR:
                        if (!(e.OpStatus == INCCSR.SUCCESS || e.OpStatus == INCCSR.MEAS_CONTINUE))
                        {
                            StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)sender);
                            //Action<object> action = (object obj) =>
                            //{
                            //    StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)obj);
                            //};
                            //Task signifyingnothing = Task.Factory.StartNew(action, sender);
                        }
                        break;
                    case SRTakeDataHandler.SROp.StartSRDAQ:
                        if (!(e.OpStatus == INCCSR.SUCCESS || e.OpStatus == INCCSR.MEAS_CONTINUE))
                        {
                            StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)sender);
                            //Action<object> action = (object obj) =>
                            //{
                            //    StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)obj);
                            //};
                            //Task signifyingnothing = Task.Factory.StartNew(action, sender);

                        }
                        break;
                    case SRTakeDataHandler.SROp.WaitForResults:
                        if (!(e.OpStatus == INCCSR.SUCCESS || e.OpStatus == INCCSR.MEAS_CONTINUE))
                        {
                            StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)sender);
                            //Action<object> action = (object obj) =>
                            //{
                            //    StopThisSRAssayImmediately((SRTakeDataHandler.SRControlThread)obj);
                            //};
                            //Task signifyingnothing = Task.Factory.StartNew(action, sender);
                         }
                        else
                        {
                            SRTakeDataHandler.SRControlThread srct = (SRTakeDataHandler.SRControlThread)sender;
                            sr.Log.TraceEvent(LogLevels.Verbose, 87225, "Transform the results, or get the transformed results");
                            srct.sri.PendingComplete();                        // when we get here, the raw results are already available on the SR thread structure
                            if (srct.sri.DAQState == DAQInstrState.HVCalib)
                            {

                            }
                            else
                                srct.SRCtrl.TransformResults(srct.SRCtrl.Det.Id, srct.sri.RDT.Cycle); // convert to enhanced results
                            HandleEndOfCycleProcessing(srct.sri);
                        }
                        break;
                    case SRTakeDataHandler.SROp.CloseSR:
                        break;
                }
            }
            catch (AnalysisDefs.CancellationRequestedException)  // thrown from this method
            {
                StopActiveAssayImmediately();
            }
            catch (Exception oddex)
            {
                sr.Log.TraceEvent(LogLevels.Error, 643, "Internal error, stopping active processing, {0}", oddex.Message);
                if (sender != null)
                {
                    SRTakeDataHandler.SRControlThread srct = (SRTakeDataHandler.SRControlThread)sender;
                    HandleFatalGeneralError(srct.sri, oddex);
                }
                else
                {
                    // will blow if not really an Assay subclass, e.g. not running in the context of the full DAQCOntrol class 
                    CurState.State = DAQInstrState.Offline; // remaining buffers should now bypass DAQ section
                    gControl.StopLMCAssay(removeCurLMDataFile: false); // stop the instruments
                    gControl.collog.TraceException(oddex, false);
                    gControl.collog.TraceEvent(LogLevels.Info, 429, "DAQ processing incomplete: {0}, processing stopped", oddex.Message);
                    //activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);
                    gControl.MajorOperationCompleted();  // signal the controlling loop we are done

                }

            }
        }

        static void StopThisSRAssayImmediately(SRTakeDataHandler.SRControlThread srct)
        {
            srct.sri.PendingComplete();
            // stop the single SR thread 
            srct.CancelMe();
            srct.sri.DAQState = DAQInstrState.Offline;
            srct.SRCtrl.Log.TraceEvent(LogLevels.Info, 428, "SR {0} DAQ cancelled", srct.sri.id.Identifier());

            // dev note: this still isn't working very well, need to completely fix the early termination logic, it should exit cleanly without extra cycle and results processing
            //srct.sri.RDT.EndOfCycleProcessing(NC.App.Opstate.Measurement);
            //srct.sri.selected = false;
            DAQControl.CurState.State = DAQInstrState.Online;
            gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
        }

        /// <summary>
        /// Checks control state for DAQ continuation, then
        /// If at the end of a cycle
        /// 1) finishes cycle processing (get results, summarize counts, cycle conditioning steps)
        ///   a) starts the next assay cycle, or
        ///   b) notifies controller of DAQ measurement completion 
        /// </summary>
        /// <param name="activeInstr">instrument object associated with the current DAQ state</param>
        /// <param name="sb">status block object indicating end of a cycle</param>
        static void HandleEndOfCycleProcessing(SRInstrument activeInstr)
        {

            //activeInstr.RDT.PlaceStatusTextOnCurrentCycle(sb);
            //activeInstr.RDT.Cycle.ApplyTheCycleConditioningSteps(CurState.Measurement);
            bool done = true; // assume done for all instruments

            activeInstr.DAQState = DAQInstrState.Offline;

            
            IEnumerator iter = Instruments.Active.GetSREnumerator();
            while (iter.MoveNext())
            {
                SRInstrument sri = (SRInstrument)iter.Current;
                if (sri.DAQState == DAQInstrState.ReceivingData || sri.DAQState == DAQInstrState.Online)
                    done = false;
                break;
            }

            if (done) // all are done
            {
                CurState.State = DAQInstrState.Online;
                activeInstr.RDT.logger.TraceInformation("Assay cycle " + CurState.Measurement.CurrentRepetition + " complete");
                if ((CurState.Measurement.CurrentRepetition < CurState.Measurement.RequestedRepetitions) ||
                    (CurState.Measurement.RequestedRepetitions == 0) && !CurState.IsQuitRequested)
                {
                    bool ok = activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);
                    if (gControl != null)
                    {
                        if (ok)
                            gControl.StartLM_SRAssay();
                        else
                            gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                    }
                }
                else
                {
                    activeInstr.RDT.logger.TraceInformation("All assay cycles completed");
                    activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);
                    if (gControl != null)
                    {
                        gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                    }
                }
            }
        }
    }
}