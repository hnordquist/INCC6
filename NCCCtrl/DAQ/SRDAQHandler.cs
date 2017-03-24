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
using System.Threading.Tasks;
using AnalysisDefs;
using DetectorDefs;
using LMSR;
using NCCReporter;
using NCCTransfer;
using Instr;
namespace DAQ
{
    
    using NC = NCC.CentralizedState;
    using SR = INCCSR;

    public sealed class SRTakeDataHandler
    {
        #region Fields

        private Measurement meas;
        public Measurement Meas
        {
            get { return meas; }
            set { meas = value; }
        }

        private LMLoggers.LognLM logger;
        public LMLoggers.LognLM Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        // SemaphoreSlim pendingCompletion;
        #endregion

        #region Constructors

        public SRTakeDataHandler(LMLoggers.LognLM logger)
        {
            this.logger = logger;
            // this.pendingCompletion = new SemaphoreSlim(0, 1);
            threads = new Dictionary<DataSourceIdentifier, SRControlThread>();
        }
        #endregion

        #region Public Methods

        /// <summary>
        ///  cancel then wait for the bitter end
        /// </summary>
        /// <param name="inst"></param>
        public void StopThread(SRInstrument inst, bool wait)
        {
            SRControlThread srct = null;
            foreach (KeyValuePair<DataSourceIdentifier, SRControlThread> kvp in threads)
            {
                if (kvp.Key.Equals(inst.id))
                {
                    kvp.Value.CancelMe();  // doing the cancel in two ways!
                    srct = kvp.Value;
                    break;
                }
            }
            int counter = 0;
            while (srct != null & srct.IsBusy & wait & counter < 10)
            {
                counter++;
                NC.App.CollectLogger.TraceEvent(LogLevels.Verbose, 87233, "Waiting for thread {0} to terminate", srct.SRCtrl.identifyingNumber);
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Is any SR thread active?
        /// </summary>
        public int IsSRThreadRunning
        {
            get
            {
                int b = 0;
                foreach (KeyValuePair<DataSourceIdentifier, SRControlThread> kvp in threads)
                {
                    if (kvp.Value.IsBusy)
                    {
                        b = kvp.Value.SRCtrl.identifyingNumber;
                        break;
                    }
                }
                return b;
            }
        }

        public SRControl StartSRControl(SRInstrument sr, ProgressChangedEventHandler pceh, SROpCompletedEventHandler opeh)
        {
            SRControlThread srct;
            if (threads.ContainsKey(sr.id))
            {
                srct = threads[sr.id];
                return srct.SRCtrl; // what?
            }
            // lookup the associated detector for the SRInstrument (match on DSID)

            Detector det = meas.Detectors.GetIt(sr.id);  // in practice this is always the same as meas.Detector
            // must be there!

            srct = new SRControlThread(threads.Count + 1, logger, meas, det);
            srct.sri = sr;
            threads[sr.id] = srct;
            srct.WorkerReportsProgress = true;
            srct.WorkerSupportsCancellation = true;
            srct.DoWork += new DoWorkEventHandler(ThreadOp);
            srct.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Done);
            srct.ProgressChanged += new ProgressChangedEventHandler(pceh);
            srct.SROpCompleted += new SROpCompletedEventHandler(opeh);
            srct.RunWorkerAsync(sr);

            return srct.SRCtrl;
        }


        #endregion

        public enum SROp { Nothing, InitializeContext, InitializeSR, StartSRDAQ, WaitForResults, CloseSR, Shutdown, ReInitializeSR } ;

        // a general idea of what to do next in an SR processing sequence
        public static SROp NextOp(SROp thisOp, int curStatus)
        {

            SROp next = SROp.Nothing;
            switch (thisOp)
            {
                case SROp.Shutdown:
                    next = SROp.Nothing;
                    break;
                case SROp.Nothing:
                    next = SROp.InitializeContext;
                    break;
                case SROp.InitializeContext:
                    next = SROp.InitializeSR;
                    break;
                case SROp.InitializeSR:
                    if (curStatus != sr_h.SR_SUCCESS)  // status here is an sr.h machine code only
                        next = SROp.CloseSR;
                    else
                        next = SROp.StartSRDAQ;
                    break;
                case SROp.StartSRDAQ:
                    if (curStatus != SR.MEAS_CONTINUE)  // MEAS_ABORT or MEAS_TERMINATE
                        next = SROp.StartSRDAQ;
                    else
                        next = SROp.CloseSR;
                    break;
                case SROp.WaitForResults:
                    if ((curStatus == SR.ZERO_COUNT_TIME) || (curStatus == SR.SR_TRY_AGAIN))  // dev note: empty cycles are eliminated by this step
                    {
                        next = SROp.StartSRDAQ;
                    }
                    else if (curStatus != SR.SUCCESS)  // MEAS_ABORT or MEAS_TERMINATE
                    {
                        next = SROp.CloseSR;
                    }
                    else
                    {
                        next = SROp.CloseSR;
                    }
                    break;
                case SROp.CloseSR:
                    next = SROp.Shutdown;
                    break;
            }
            return next;

        }

        public delegate void SROpCompletedEventHandler(object sender, SROpCompletedEventArgs e);

        // Summary:
        //     Provides data for the SROpCompleted event.
        public class SROpCompletedEventArgs : EventArgs
        {
            public SROpCompletedEventArgs(SROp op, int statusNow, object userState)
            {
                Op = op;
                OpStatus = statusNow;
                UserState = userState;
            }
            public SROp Op { get; set; }
            public int OpStatus { get; set; }
            public object UserState { get; set; }

        }

        /// <summary>
        ///  Dev note, unsolved problem: fail on init or start should exit as if cancelled but then it will notify and then try to start the assay cycle anyway and then it hangs. At least, it should not hang, and then cancel doesn't work, likely the thread is dead and we need to check for a dead thread
        /// </summary>
        public class SRControlThread : BackgroundWorker
        {

            public SRControlThread(int count, LMLoggers.LognLM log, Measurement meas, Detector det)
            {
                op = SROp.Nothing;
                SRCtrl = new SRControl(count, log, meas, det);
                opgate = new ManualResetEventSlim(false);
                callwait = new ManualResetEventSlim(false);
                cts = new CancellationTokenSource();
            }

            // every piece of data needed to do an autonomous SR control process
            public ManualResetEventSlim opgate, callwait;
            public SROp op;
            public SRControl SRCtrl;
            public CancellationTokenSource cts;
            public event SROpCompletedEventHandler SROpCompleted;
            public int status;
            public Measurement meas;
            public SRInstrument sri;

            public void CancelMe()
            {
                CancelAsync();  // doing the cancel in two ways!
                cts.Cancel();
            }
            public void Shutdown()
            {
                op = SROp.Shutdown;
                CancelMe();
            }
            public void SROpCompletedEvent(int statusNow)
            {
                if (SROpCompleted != null)
                {
                    SROpCompleted(this, new SROpCompletedEventArgs(op, statusNow, SRCtrl));
                }
            }
        }

        public void SetAction(DataSourceIdentifier dsid, SROp op)
        {
            SRControlThread srct;
            if (threads.ContainsKey(dsid))
            {
                srct = threads[dsid];
                if (!srct.IsBusy)
                    return;

                srct.op = op;

                // tell caller to wait, then kick the waiting thread into action
                srct.callwait.Reset();

                srct.opgate.Set();
            }
        }

        public int StartSRActionAndWait(DataSourceIdentifier dsid, SROp op, int setpt = -1, int runtime = -1)
        {
            SRControlThread srct;
            int res = SR.SUCCESS;
            if (threads.ContainsKey(dsid))
            {
                srct = threads[dsid];
                if (srct == null || !srct.IsBusy)
                    return res;

                try
                {
                    if (setpt >= 0)
                        srct.SRCtrl.HVSet(setpt, runtime);

                    srct.op = op;
                    // tell caller to wait, then kick the thread into action
                    srct.callwait.Reset();

                    // tell the thread to go ahead
                    srct.opgate.Set();

                    // caller will now wait for completion
                    srct.callwait.Wait(srct.cts.Token);

                    // fire the operation completed event here, consciously placed out of the executing SR service thread context
                    srct.SROpCompletedEvent(srct.status);
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    res = srct.SRCtrl.LastMeasStatus;
                }
            }
            return res;
        }

        // dev note: prolly need to lock access to data on these thread classes
        public void GetLastStatus(DataSourceIdentifier dsid, ref int lastSr, ref int lastMeas)
        {
            SRControlThread srct;
            if (threads.ContainsKey(dsid))
            {
                srct = threads[dsid];
                lastSr = srct.SRCtrl.LastSRStatus;
                lastMeas = srct.SRCtrl.LastMeasStatus;
            }
        }

        //public void GetResults(DataSourceIdentifier dsid)
        //{
        //    SRControlThread srct;
        //    if (threads.ContainsKey(dsid))
        //    {
        //        srct = threads[dsid];
        //        srct.SRCtrl.TransformResults(dsid, srct.sri.RDT.Cycle);
        //    }
        //}

        //public MultiplicityCountingRes GetTransformedResults(DataSourceIdentifier dsid)
        //{
        //    SRControlThread srct;
        //    if (threads.ContainsKey(dsid))
        //    {
        //        srct = threads[dsid];
        //        return srct.SRCtrl.TransformedResults;
        //    }
        //    else
        //        return null;
        //}

        //// create mcr on each thread control state
        //public void GetResults()
        //{

        //    foreach (KeyValuePair<DataSourceIdentifier, SRControlThread> kvp in threads)
        //    {
        //        SRControlThread srct = (SRControlThread)kvp.Value;
        //        srct.SRCtrl.TransformResults(kvp.Key, srct.sri.RDT.Cycle);
        //    }
        //}

        void Done(object sender, RunWorkerCompletedEventArgs e)
        {
            // use Result or UserState to capture status and sr-status
            SRControlThread srct = sender as SRControlThread;
            string name = "<unset>";
            int resulto = -3;
            string res = "";
            if (e.Result != null)
                resulto = (int)e.Result;
            try 
            {
                res = SR.SRAPIReturnStatusCode(resulto); 
                name = srct.SRCtrl != null ? srct.SRCtrl.Identifier : "a thread";
            }
            catch (Exception caught)
            {
                logger.TraceEvent(LogLevels.Error, 87258, LMLoggers.LognLM.FlattenChars("SR Thread reference '" + caught.Message + "' "));
                res += (" -> " + caught.ToString());
            }

            if (e.Cancelled)
            {
                logger.TraceEvent(LogLevels.Warning, 87256, name + " cancelled with " + res);
            }
            else if (e.Error != null)
            {
                logger.TraceEvent(LogLevels.Warning, 87257, name + " ended with exception, " + res);
                logger.TraceException(e.Error, false);
            }
            else
            {
                logger.TraceEvent(LogLevels.Info, 87210, name + " completed with " + res);
            }
        }

        bool CancellationRequested(SRControlThread srct)
        {
            return NC.App.Opstate.IsQuitRequested || // globally?
                srct.CancellationPending || // via the thread AsyncCancel() op
                srct.cts.IsCancellationRequested;  // via the threads personal token
        }

        void ThreadOp(object sender, DoWorkEventArgs ea)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            SRControlThread srct = (SRControlThread)sender;
            SRControl sr = srct.SRCtrl;
            string identifier = "<unset>";
            sr.Log.TraceEvent(LogLevels.Verbose, 87200, "Starting new SR thread");
            try
            {
                identifier = sr.Identifier;
                sr.Log.TraceEvent(LogLevels.Verbose, 87201, "SR {0} thread running ", identifier); 
                do
                {
                    sr.Log.TraceEvent(LogLevels.Verbose, 87202, "SR {0} thread wait for command", identifier);
                    try
                    {
                        srct.opgate.Wait(srct.cts.Token); // wait for caller to set an operation in motion
                    }
                    catch (OperationCanceledException)
                    {
                        //NEXT: Should this do something.  It is being thrown, but no action hn 9/23/2014
                    }
                    if (CancellationRequested(srct))
                    {
                        sr.Log.TraceEvent(LogLevels.Verbose, 87255, "SR {0} thread cancelled", identifier);
                        break;
                    }
                    srct.callwait.Reset(); // make caller wait for results

                    switch (srct.op)
                    {
                        case SROp.InitializeContext:
                            sr.Log.TraceEvent(LogLevels.Verbose, 87203, "SR {0} InitializeContext", identifier);
                            ushort numnutsd = sr.InitializeContext(sr.Meas, sr.Det);  // sets sr.acquire_num_runs
                            srct.ReportProgress(0, sr);
                            break;
                        case SROp.InitializeSR:
                            try
                            {
                                sr.Log.TraceEvent(LogLevels.Verbose, 87204, "SR {0} InitializeSR", identifier);
                                srct.status = sr.InitializeSR(srct.cts);
                                if (srct.status != sr_h.SR_SUCCESS)  // status here is an sr.h machine code only
                                {
                                    // stop = true; caller will stop, thread itself cannot
                                }
                                ea.Result = srct.status;
                                sr.InitTDState(); // uses  sr.acquire_num_runs
                            }
                            catch (Exception e)
                            {
                                // do not set LastSRStatus because there may have never been one set, use the Meas status
                                ea.Result = srct.status = srct.SRCtrl.LastMeasStatus = SR.SR_FAIL;
                                sr.Log.TraceException(e, false);
                                sr.Log.TraceEvent(LogLevels.Warning, 87259, "SR {0} InitializeSR failure ", identifier);
                            }
                            srct.ReportProgress(0, sr);
                            break;
                        case SROp.StartSRDAQ:
                            sr.Log.TraceEvent(LogLevels.Verbose, 87205, "SR {0} StartSRDAQ", identifier);
                            srct.status = sr.StartSRDAQ();
                            if (srct.status != SR.MEAS_CONTINUE)  // MEAS_ABORT or MEAS_TERMINATE
                            {
                                // stop = true; caller will stop, thread itself cannot
                            }
                            ea.Result = srct.status;
                            srct.ReportProgress((int)(100 * ((double)sr.run_number / (double)sr.acquire_num_runs)), sr);
                            break;
                        case SROp.WaitForResults:
                            sr.Log.TraceEvent(LogLevels.Verbose, 87206, "SR {0} PollAndGetResults", identifier);
                            srct.status = sr.PollAndGetResults();
                            ea.Result = srct.status;
                            if ((srct.status == SR.ZERO_COUNT_TIME) || (srct.status == SR.SR_TRY_AGAIN))  // dev note: empty cycles are eliminated by this step
                            {
                                sr.run_number--;
                            }
                            else if (srct.status != SR.SUCCESS)  // MEAS_ABORT or MEAS_TERMINATE
                            {
                                // stop = true; caller will stop, thread itself cannot
                            }
                            else
                            {
                                // sr.tds has the results now, caller must retrieve them manually using TransformResults
                                sr.fraction += sr.Meas.AcquireState.run_count_time;
                                // run this out-of-thread so that caller wait is reset prior to the restart in StartLMCAssay!
                                Action<object> action = (object obj) =>
                                {
                                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                                    SRControlThread talsrct = (SRControlThread)obj;
                                    talsrct.SROpCompletedEvent(talsrct.status);
                                };
                                Task signifyingnothing = Task.Factory.StartNew(action, srct);
                            }
                            srct.ReportProgress((int)(100 * ((double)sr.run_number / (double)sr.acquire_num_runs)), sr);
                            break;
                        case SROp.CloseSR:
                            sr.Log.TraceEvent(LogLevels.Verbose, 87207, "SR {0} CloseDownSR", identifier);
                            sr.CloseDownSR();
                            srct.ReportProgress(100, sr);
                            break;
                        case SROp.Shutdown:
                            sr.Log.TraceEvent(LogLevels.Verbose, 87207, "SR {0} shutdown thread", identifier);
                            break;

                        case SROp.ReInitializeSR:
                            try
                            {
                                sr.Log.TraceEvent(LogLevels.Verbose, 87208, "SR {0} ReInitializeSR", identifier);
                                srct.status = sr.ReInitializeSR();
                                if (srct.status != sr_h.SR_SUCCESS)  // status here is an sr.h machine code only
                                {
                                    // stop = true; caller will stop, thread itself cannot
                                }
                                ea.Result = srct.status;
                                sr.InitTDState(); // uses  sr.acquire_num_runs
                            }
                            catch (Exception e)
                            {
                                // do not set LastSRStatus because there may have never been one set, use the Meas status
                                ea.Result = srct.status = srct.SRCtrl.LastMeasStatus = SR.SR_FAIL;
                                sr.Log.TraceException(e, false);
                                sr.Log.TraceEvent(LogLevels.Verbose, 87260, "SR {0} ReInitializeSR failure ", identifier);
                            }
                            srct.ReportProgress(0, sr);
                            break;
                    }
                    srct.opgate.Reset(); // wait again at the top of loop, but first check for cancel or stop in the while condition here
                    srct.callwait.Set(); // release caller to get state and do work
                } while (!(srct.op == SROp.Shutdown) || CancellationRequested(srct));  // run until a cancel occurs, or caller specifies the Shutdown operation

            }
            catch (Exception e)
            {
                sr.Log.TraceException(e, true);
                sr.Log.TraceEvent(LogLevels.Verbose, 87257, "SR ThreadOp bonk {0}", identifier);
            }

            sr.Log.TraceEvent(LogLevels.Verbose, 87297, "SR {0} thread exiting", identifier);
            srct.callwait.Set(); // release caller to get state and do work
            //srct.SROpCompletedEvent(srct.status);  // signal failure via the final event if ungraceful exit in play

        }

        Dictionary<DataSourceIdentifier, SRControlThread> threads;
    }




    /*
     *  Sample driver code, Mar. 31, 2011, successfully controlled a JSR-12 with the ported DLLs and C# wrapper code using this set of statements 
     * LMSR.take_data td = new LMSR.take_data(NC.App.CollectLogger);
     * m.AcquireState.num_runs = 3;
     * m.AcquireState.run_count_time = 10.0;
     * m.MeasurementId.SetSRType("JSR12");
     * m.MeasurementId.ID.ConnInfo = "0";
     * m.Detector.SRParams.highVoltage = 1599;
     * m.Detector.SRParams.predelay = 30; 
     * m.Detector.SRParams.gateLength = 640;
     * td.take_sr_data(m);
     */

    // NEXT: see the comments marked SR_PORT for those tasks required to get a functioning SR take_data processing cycle going

    // Map these steps into the INCC steps and the DAQ/FileProc steps
    // 1   InitializeContext 
    // 2   InitializeSR 
    // 3   Loop a,b until done:
    // 3.a    StartSRDAQ --> if abort, delete up to now, if terminate do outlier+sums and then stop and save as if Rates Only <-- need this new logic fragment implemented at high level 
    // 3.b    PollAndGetResults --> if abort, delete up to now, if terminate do outlier+sums and then stop and save as if Rates Only, if funky SR or cycle, skip it and continue
    // 4   Close SR
    // *: At each step, user or code can cancel, update status for using the ReportProgress event from the thread
    // *: Conjure up event and timer callback interface to support interactive user intervention with continue/abort (supra) (also see gl_exit_ok and gl_take_data_status as the exported flag entry points)
    // * implement the run to cycle transfer
    // SR_PORT: implement the use of cycle conditioning steps
    // *: implement the timeout structure use
    // *: a single detector should be the controlling data structure for a threaded call of this function. That is, we can connect n SRs and run them at the same time out of n comports. But need the threading control at a higher level to manage that. Much like MIC or the anonymous socket handling thread approach in LM
    // *: abort and terminate state are set and read via the cancellation token watch and the stop event handler
    public class SRControl
    {

        public bool SaveOnTerminate;
        private LMLoggers.LognLM log;
        private Measurement meas;
        private Detector detector;
        private INCCSR sr;
        private TDState tds;

        private double hvsetpt, runtime = -1.0;

        public int identifyingNumber;
        public UInt16 acquire_num_runs;
        public int LastSRStatus;
        public int LastMeasStatus;

        public LMLoggers.LognLM Log
        {
            get { return log; }
        }

        public Measurement Meas
        {
            get { return meas; }
        }

        public Detector Det
        {
            get { return detector; }
        }

        public string Identifier
        {
            get { return (IsInitialized ? SR.Id.Identifier() : "New") + " #" + identifyingNumber.ToString(); }
        }
        public bool IsInitialized
        {
            get { return sr != null && tds != null; }
        }
        public INCCSR SR
        {
            get { return sr; }
        }

        public UInt16 run_number
        {
            get { return tds.run_number; }
            set { tds.run_number = value; }
        }

        public double fraction
        {
            get { return tds.fraction; }
            set { tds.fraction = value; }
        }

        public MultiplicityCountingRes TransformedResults
        {
            get { return tds.mcr; }
        }

        public void HVSet(int setpt, int runtime)
        {
            hvsetpt = setpt;
            this.runtime = runtime;
        }

        /// <summary>
        /// transform run_rec_ext results into a MultiplicityCountingRes, also see INCKnew.RunToCycle
        /// later step will place results on current cycle in appropriate results map using the Det.MultiplicityParams key
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cycle"></param>
        public void TransformResults(DataSourceIdentifier Id, Cycle cycle)
        {
            FAType fa = Id.SRType.DefaultFAFor();
            tds.mcr = new MultiplicityCountingRes(fa, 0);
            RunValuesToResults(tds.run, cycle, Det.MultiplicityParams, tds.mcr);
        }

        /*============================================================================
        *
        * function name: SRControl() et al. from takedata.cpp
        *
        * purpose: acquire a set of shift register data (measurement).
        *
        * return value: SUCCESS/FAIL/MEAS_ABORTED/SR_FAIL/CHECKSUM_FAIL/ACC_SNGL_FAIL
        *
        * special notes: 
        *
        * revision history:
        *
        *  date	author		revision
        *  ----	------		--------
        *  12/09/93	Bill Harker	created
        *  03/29/11 Frank Black reimplemented in C#
        *============================================================================*/
        public SRControl(int identifyingNumber, LMLoggers.LognLM log, Measurement meas, Detector det)
        {
            this.log = log;
            this.identifyingNumber = identifyingNumber;
            LastSRStatus = sr_h.SR_SUCCESS;
            LastMeasStatus = SR.SUCCESS;
            SaveOnTerminate = true;
            this.meas = meas;
            this.detector = det;
        }

        /// <summary>
        /// Set up state to take a measurement
        /// </summary>
        /// <param name="meas">Measurement context instance</param>
        /// <returns>number of cycles (runs), or repetitions, to attempt</returns>
        public UInt16 InitializeContext(Measurement meas, Detector det)
        {
            this.meas = meas;
            this.detector = det;
            if ((meas.MeasOption == AssaySelector.MeasurementOption.precision) && (detector.Id.SRType == InstrType.DGSR))
            {
                //sr_parms.gate_length2 = sr_parms.gate_length;  // todo: DGSR gate_length2, but no-one will ever need it
            }

            acquire_num_runs = meas.AcquireState.num_runs;
            if (meas.MeasOption == AssaySelector.MeasurementOption.verification)
            {
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.ActivePassive) ||
                    meas.INCCAnalysisState.Methods.Has(AnalysisMethod.Active))
                {
                    acquire_num_runs = meas.AcquireState.active_num_runs;
                }
            }
 
            LastMeasStatus = SR.SUCCESS;
            return acquire_num_runs;
        }

        public void InitTDState()
        {
            tds = new TDState(acquire_num_runs);
        }

        /// <summary>
        /// Create SR state controller and initialize the SR
        /// </summary>
        /// <param name="meas"></param>
        /// <returns>SR codes, e.g. SR_SUCCESS/SR_INVPORT/SR_IOERR/SR_TIMEOUT/ZERO_COUNT_TIME</returns>
        public int InitializeSR(CancellationTokenSource controllingThreadCTS)
        {
            // dev note: add-a-source <snip>
            if (sr == null)
            {
                sr = new SR(detector.SRParams, detector.Id, meas.AcquireState, meas.Tests, log);
                sr.LinkedCancelSource = NC.App.Opstate.CancelStopAbort.NewLinkedCancelStopAbortAndClientTokenSources(controllingThreadCTS.Token);
                if (runtime == -1.0) // not set for HVP so use the expected values
                {
                    runtime = meas.AcquireState.run_count_time;
                    hvsetpt = detector.SRParams.highVoltage;
                }
            }

            /* open, set parameters and initialize shift register */
            int init_status = sr.sr_init(runtime, hvsetpt);
            if (init_status != sr_h.SR_SUCCESS)
            {
                //LastSRStatus= sr.sr_stop();  // init retries to stop twice
                LastMeasStatus = SR.SR_FAIL;
            }
            else
                LastMeasStatus = SR.SUCCESS;
            LastSRStatus = init_status;
            return init_status;
        }

        /// <summary>
        /// reinitialize various SR settings (used only by HVP op)
        /// </summary>
        /// <param name="meas"></param>
        /// <returns>SR codes, e.g. SR_SUCCESS/SR_INVPORT/SR_IOERR/SR_TIMEOUT/ZERO_COUNT_TIME</returns>
        public int ReInitializeSR()
        {
            
            /* open, set parameters and initialize shift register */
            int init_status = sr.sr_init(runtime, hvsetpt);
            if (init_status != sr_h.SR_SUCCESS)
            {
                LastMeasStatus = SR.SR_FAIL;
            }
            else
                LastMeasStatus = SR.SUCCESS;
            LastSRStatus = init_status;
            return init_status;
        }

        /// <summary>
        /// Start the SR
        /// </summary>
        /// <returns>SR.MEAS_TERMINATED: stop and do outliers and sums on existing collected results</returns>
        /// <returns>SR.MEAS_ABORTED: abort and delete existing collected results</returns>
        /// <returns>SR.MEAS_CONTINUE: move forward with the collect step</returns>
        public int StartSRDAQ()
        {
            int start_status;
            int mstatus = SR.MEAS_CONTINUE;

            tds.run_number++;
            tds.run.run_number = tds.run_number;

            tds.total_time = meas.AcquireState.run_count_time *
                (double)tds.acquire_num_runs;

            /* setup COM timeout equal to count time (if JSR-11 use 1 second) */
            if (detector.Id.SRType != InstrType.JSR11)
                SRLib.Set_timeout(ref tds.timeout, meas.AcquireState.run_count_time);  
            else
                SRLib.Set_timeout(ref tds.timeout, 1.0);

            /* start shift register counting */
            UInt16 retries = 0;
            do
            {
                start_status = sr.sr_start_counting();
                if (start_status == SR.SR_TRY_AGAIN)
                {
                    meas.AddWarningMessage("Shift register start failure, cycle " + tds.run_number.ToString(), 0x44F00, Det.MultiplicityParams);
                }
                retries++;
            } while ((start_status == SR.SR_TRY_AGAIN) && (retries < SR.SR_NUM_TRYS));
            if (start_status != sr_h.SR_SUCCESS)
            {
                sr.sr_stop();
                /// dev note: add-a-source <snip>
                if (tds.run_number > 1)
                {
                    tds.gl_take_data_status = SaveOnTerminate ? SR.MEAS_TERMINATED : SR.MEAS_ABORTED;
                }
                else
                {
                    // set error state here
                    tds.gl_take_data_status = SR.MEAS_ABORTED;
                    meas.AddWarningMessage("Unable to start shift register", 0x44F07, Det.MultiplicityParams);
                }
                if (tds.gl_take_data_status == SR.MEAS_ABORTED)
                {
                    // delete_measurement ();
                    mstatus = SR.MEAS_ABORTED;
                }
                else
                {
                    mstatus = (SR.MEAS_TERMINATED);
                }
            }
            LastSRStatus = start_status;
            LastMeasStatus = mstatus;
            return mstatus;
        }


        /// <summary>
        /// Pending results from the SR, wait and read loop
        /// </summary>
        /// <param name="meas"></param>
        /// <returns>SR.MEAS_TERMINATED: stop and do outliers and sums on existing collected results</returns>
        /// <returns>SR.MEAS_ABORTED: abort and delete existing collected results</returns>
        /// <returns>SR.SUCCESS: continue forward with the next collect step</returns>
        /// <returns>SR.ZERO_COUNT_TIME, SR.SR_TRY_AGAIN: continue, but eliminate this attempt from the accumulated state</returns>
        public int PollAndGetResults()
        {
            int status = SR.SUCCESS;
            do
            {
                Thread.Sleep(1000);		/* allow windows system to execute */

                int com_status = SRLib.Is_timeout(ref tds.timeout);
                if (com_status != 0)
                {
                    /* if shift register stopped, get data into run record */
                    status = sr.sr_get_data(ref tds.run);

                    if (status == SR.SR_TRY_AGAIN)
                    {
                        meas.AddWarningMessage("Shift register read failure, cycle " + tds.run_number.ToString(), 0x44F01, Det.MultiplicityParams);
                        break;
                    }
                    if ((status != SR.SR_NOT_FINISHED) && (status != sr_h.SR_SUCCESS))
                    {
                        if (tds.run_number > 1)
                            tds.gl_take_data_status = SaveOnTerminate ? SR.MEAS_TERMINATED : SR.MEAS_ABORTED;
                        else
                            tds.gl_take_data_status = SR.MEAS_ABORTED;
                    }
                }
                else
                {
                    status = SR.SR_NOT_FINISHED;
                }
                LastSRStatus = com_status;
                if ((tds.gl_take_data_status == SR.MEAS_ABORTED) ||
                    (tds.gl_take_data_status == SR.MEAS_TERMINATED))
                {

                    sr.sr_stop();
                    /// dev note: add-a-source <snip>
                }
                if ((tds.gl_take_data_status == SR.MEAS_TERMINATED) && (tds.run_number <= 1))
                    tds.gl_take_data_status = SR.MEAS_ABORTED;
                if (tds.gl_take_data_status == SR.MEAS_ABORTED)
                {
                    // delete_measurement

                    status = SR.MEAS_ABORTED;
                }
                else if (tds.gl_take_data_status == SR.MEAS_TERMINATED)
                {
                    // SR_PORT FACTOR: this is the end of the incomplete measurement, where OutlierProcessing and CalcAvgsAndSums are called (only)
                    status = SR.MEAS_TERMINATED;
                }
            } while (status == SR.SR_NOT_FINISHED);

            LastMeasStatus = status;
            return status;
        }


        public int CloseDownSR()
        {
            // step 4
            int status = sr.sr_stop();
            Thread.Sleep(1000);		/* allow windows system to execute */
            LastSRStatus = status;
            LastMeasStatus = SR.SUCCESS;
            return status;
        }

        unsafe static void RunValuesToResults(run_rec_ext run, Cycle cycle, Multiplicity key, MultiplicityCountingRes mcr)
        {
            cycle.seq = run.run_number;
            cycle.TS = TimeSpan.FromSeconds(run.run_count_time);  // is not always whole seconds hn 10-1.

            cycle.Totals = (ulong)run.run_singles;
            cycle.SinglesRate = run.run_singles / run.run_count_time;

            // table lookup on the strings, so test status is correct
            string s = TransferUtils.str(run.run_tests, INCC.MAX_RUN_TESTS_LENGTH);
            QCTestStatus qcts = QCTestStatusExtensions.FromString(s);

            cycle.SetQCStatus(key, qcts); // creates entry if not found


            mcr.Totals = cycle.Totals;
            mcr.TS = cycle.TS;

            mcr.DeadtimeCorrectedSinglesRate.v = run.run_singles_rate;
            mcr.DeadtimeCorrectedDoublesRate.v = run.run_doubles_rate;
            mcr.DeadtimeCorrectedTriplesRate.v = run.run_triples_rate;

            mcr.RASum = (ulong)run.run_reals_plus_acc;
            mcr.ASum = (ulong)run.run_acc;

            mcr.efficiency = run.run_multiplicity_efficiency;
            mcr.mass = run.run_mass;
            mcr.multiAlpha = run.run_multiplicity_alpha;
            mcr.multiplication = run.run_multiplicity_mult;
            cycle.HighVoltage = run.run_high_voltage;

            // assign the hits to a single channel (0)
            cycle.HitsPerChannel[0] = run.run_singles;
            mcr.RawSinglesRate.v = run.run_singles_rate;
            mcr.RawDoublesRate.v = run.run_doubles_rate;
            mcr.RawTriplesRate.v = run.run_triples_rate;

            mcr.Scaler1.v = run.run_scaler1;
            mcr.Scaler2.v = run.run_scaler2;
            mcr.Scaler1Rate.v = run.run_scaler1_rate;
            mcr.Scaler2Rate.v = run.run_scaler2_rate;

            long index = 0;
            for (ulong i = 0; i < INCC.SR_EX_MAX_MULT; i++)
            {
                if (run.run_mult_acc[i] > 0 || run.run_mult_reals_plus_acc[i] > 0)
                {
                    index = (long)i;
                }
            }

            mcr.MaxBins = (ulong)index + 1;
            mcr.MinBins = (ulong)index + 1;

            mcr.NormedAMult = new ulong[mcr.MaxBins];
            mcr.RAMult = new ulong[mcr.MaxBins];
            mcr.UnAMult = new ulong[mcr.MaxBins];

            // was not setting these to the right values hn 10-2
            for (ulong i = 0; i < (ulong)mcr.MaxBins; i++)
            {
                mcr.RAMult[i] = (ulong)run.run_mult_reals_plus_acc[i]; 
                mcr.NormedAMult[i] = (ulong)run.run_mult_acc[i];
            }
            mcr.RASum = run.run_reals_plus_acc;
            mcr.ASum = run.run_acc;
            mcr.AB.Resize((int)mcr.MaxBins);
        }



        /// <summary>
        ///  This is a test driver used to develop the SR classes and methods in SRControl, SRTakeDataHandler and the SREventHandler and for driving an asynchronous SR measurement together with LM.
        ///  Take data according to parameters on measurement, modeled after code in take_data.cpp
        /// </summary>
        /// <param name="meas">the measurement, has acq params</param>
        /// <returns>SR.MEAS_TERMINATED: stop and do outliers and sums on existing collected results</returns>
        /// <returns>SR.MEAS_ABORTED: abort and delete existing collected results</returns>
        /// <returns>SR.MEAS_CONTINUE: move forward with the next collect step</returns>
        public int take_sr_data(Measurement meas, Detector det)
        {
            // step 1
            InitializeContext(meas, det);

            // step 2
            int status = InitializeSR(new CancellationTokenSource());
            if (status != sr_h.SR_SUCCESS)
            {
                return status;
            }

            /* new Measurement should be set up prior to here, or right here, with empty cycle list 

            /* set up window for displaying run data as it is acquired */
            // dev note: replaced with timer callback and progress update events
            tds = new TDState(acquire_num_runs);

            /* See StartLMCAssay for similar steps (LM file, cycle instance) "create a results record for this measurement" */

            // step 3 should occur at a higher level  (e.g. see StartLMCAssay)

            // step 3, loop until  1) something bad happens, 2) external interrupt, or 3) to completion
            do
            {
                // step 3.a
                status = StartSRDAQ();
                if (status != SR.MEAS_CONTINUE)  // MEAS_ABORT or MEAS_TERMINATE
                {
                    return status;
                }

                // step 3.b
                status = PollAndGetResults();
                // Added A/S fail to cycle re-do decrement 3.16.2015 HN
                if ((status == SR.ZERO_COUNT_TIME) || (status == SR.SR_TRY_AGAIN) ||(status == SR.ACC_SNGL_FAIL))  // dev note: empty cycles are eliminated by this step
                {
                    tds.run_number--;
                    continue;
                }
                else if (status != SR.SUCCESS)  // MEAS_ABORT or MEAS_TERMINATE
                {
                    return status;
                }
                
                tds.fraction += meas.AcquireState.run_count_time;
            } while (tds.run_number < acquire_num_runs);

            // step 4 
            CloseDownSR();

            //this is the end of the complete measurement, next the 2nd and 3rd phases of calculation kick in (Measurement.CalculateMeasurementResults())
            return (SR.SUCCESS);

        }
    }


	/// <summary>
	/// the take_data state, all in one place
	/// </summary>
	public class TDState
    {
        public TDState(UInt16 acquire_num_runs)
        {
            gl_take_data_status = SR.SUCCESS;
            this.acquire_num_runs = acquire_num_runs;
            initial_num_runs = acquire_num_runs;
            timeout = new Com_h.com_timeout(); timeout.start = timeout.total = 0;
            run = new run_rec_ext();
        }
        public int gl_take_data_status;
        public UInt16 run_number;
        public UInt16 initial_num_runs, acquire_num_runs;
        // for status
        public double fraction;
        public double total_time;
        public Com_h.com_timeout timeout;
        public run_rec_ext run;
        public MultiplicityCountingRes mcr;
    }

}
