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
using System.Threading;
using AnalysisDefs;
using DetectorDefs;
using LMComm;
using NCC;
using NCCReporter;
using Instr;
namespace DAQ
{

    using NC = NCC.CentralizedState;

    public partial class DAQControl : NCC.ActionEvents, NCC.IActionControl
    {
        // todo: eliminate this static global referenced by static callback methods during socket event handling; 
        public static DAQControl gControl;

        // dev note: is this singleton thread-safe when we move to threads?
        static internal LMComm.TalkToLMMMM LMMMComm;

        protected LMLoggers.LognLM collog;
        protected LMLoggers.LognLM ctrllog;
        protected LMLoggers.LognLM datalog;
        protected LMLoggers.LognLM applog;
        public bool gui;

        public DAQControl(bool usingGui)
        {
            gui = usingGui;

            gControl = this;

            applog = NC.App.Loggers.Logger(LMLoggers.AppSection.App);
            collog = NC.App.Loggers.Logger(LMLoggers.AppSection.Collect);
            ctrllog = NC.App.Loggers.Logger(LMLoggers.AppSection.Control);
            datalog = NC.App.Loggers.Logger(LMLoggers.AppSection.Data);

            // The state was set on cmd line prior to exec here, and it still is, so must upcast to an Assay instance
            NC.App.Opstate = new AssayState(NC.App.Opstate);

            LMMMComm = LMMMComm ?? new TalkToLMMMM(collog);  // a singleton, modern syntax just for fun

            CurState.SOH = NCC.OperatingState.Starting;

        }


        // controller has active control of processing through these three entry points
        public void CancelCurrentAction()
        {
            ctrllog.TraceInformation("Cancelling the {0} action ({1} instrument{2})", NC.App.Opstate.Action, Instruments.Active.Count, (Instruments.Active.Count == 1 ? String.Empty : "s"));
            NC.App.Opstate.SOH = NCC.OperatingState.Cancelling;
            CurState.Cancel();
        }

        public void StopCurrentAction()
        {
            ctrllog.TraceInformation("Stopping the {0} action ({1} instruments{2})", NC.App.Opstate.Action, Instruments.Active.Count, (Instruments.Active.Count == 1 ? String.Empty : "s"));
            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            CurState.Cancel();
        }

        public void StartAction()
        {
            NC.App.Opstate.ResetTokens();
            ctrllog.TraceInformation("Starting the {0} action ({1} instrument{2})", NC.App.Opstate.Action, Instruments.Active.Count, (Instruments.Active.Count == 1 ? String.Empty : "s"));
            NC.App.Opstate.SOH = NCC.OperatingState.Starting;
        }


        // Do what the command line said: 
        //    start assay,  (via LM) 
        //    start HV Calib, (LM only)
        //    broadcast go and/or 
        //    wait at command prompt for individual queries against the LMMMMMM
        public void Run()
        {
            ShiftRegisterRuntimeInit();
            switch (NC.App.Opstate.Action)  // these are the actions available from the command line only
            {
                case NCCAction.Prompt:
                    ApplyInstrumentSettings();
                    CommandPrompt();   // launch command line interpreter loop 
                    break;
                case NCCAction.Discover: // for LMMM only, not needed for SR AFAIKT
                    ConnectInstruments();
                    ApplyInstrumentSettings();
                    CommandPrompt();   // launch command line interpreter loop 
                    break;
                case NCCAction.HVCalibration:       // if SR, must have SR RT inst preset
                    FireEvent(EventType.PreAction, this);
                    DoHVCalib();
					DisconnectInstruments();
                    FireEvent(EventType.ActionFinished, this);
                    break;
                case NCCAction.Assay:       // if SR, must have SR RT inst preset
                    FireEvent(EventType.PreAction, this);
                    DoAnAssay();
                    FireEvent(EventType.ActionFinished, this);
                    break;
            }

            LMMMComm.Settle();
            NC.App.Loggers.Flush();
            NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
        }

        protected void ShiftRegisterRuntimeInit()
        {
            Detector det = NC.App.Opstate.Measurement.Detector;
            if (det.ListMode)
                return;
            SRInstrument sri = new SRInstrument(det);
            sri.selected = true;
            sri.Init(datalog, NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
            if (!Instruments.All.Contains(sri))
                Instruments.All.Add(sri); // add to global runtime list 
        }


        protected void LogConfig()
        {
            string[] x = NCCConfig.Config.ShowCfgLines(NC.App.Config, NC.App.AppContext.Verbose() == System.Diagnostics.TraceEventType.Verbose, false);
            for (int i = 0; i < x.Length; i++)
            {
                applog.TraceInformation(x[i]);
            }
        }

        protected void CommandPrompt(Thread t = null)
        {
            // dev note: this Ctrl-C handler does not work when vshost is active in the dev env
            Console.CancelKeyPress += new ConsoleCancelEventHandler(LiveDAQCtrlCHandler);
            Console.TreatControlCAsInput = false;
            string line = "";
            bool keepGoing = true;
            bool explicitPrompt = (t == null);
            Console.WriteLine(NC.App.AbbrName + "> command prompt " + NC.App.Config.VersionString + " (press CTRL+Z to exit):");
            Console.WriteLine();
            Console.Write(NC.App.AbbrName + "> ");
            NC.App.Opstate.SOH = NCC.OperatingState.Living;
            do
            {
                if ((t != null) && !Console.KeyAvailable && !explicitPrompt)
                {
                    keepGoing = !t.Join(100); // when the batch op thread ends, we're out of the prompt loop, unless we asked for the prompt. "prompt with wait sync point approach"
                }
                else
                {
                    line = Console.ReadLine();
                    if (line != null && (line.Length > 0))
                    {
                        string tline = line.Trim();
                        LMMMLingo.OpDesc cmdt = new LMMMLingo.OpDesc(true);
                        LMMMComm.CommandPromptMatchPrefix(tline, ref cmdt);

                        if (LMMMLingo.Tokens.unknown == cmdt.tok)
                            Console.WriteLine(NC.App.AbbrName + "> skipping '" + tline + "', \r\n  enter 'help' for valid commands");
                        else
                        {
                            Thread pnt = LMMMComm.ProcessUserCommand(cmdt, tline, this, ref keepGoing);
                            if (t == null)
                                t = pnt;
                            Console.Write(NC.App.AbbrName + ">");
                        }
                    }
                    else
                        Console.Write(NC.App.AbbrName + "> ");
                }
            } while (line != null && keepGoing);

            // Issue: user quits operations via cmd prompt but system is still going waiting for HV or file or DAQ analysis to complete.
            // todo: Solution 1 (todo): prevent exit if waiting HVCalib or Assay operations to complete, pend woud occur in ProcessUserCommand above or around it via a polling loop
            // Solution 2 (below): to use the cancellation token here, provided that t is non-null & user typed quit to prompt, so need to stop LMMM and stop the pending thread represented by t, if possible.

            if (NC.App.Opstate.SOH == NCC.OperatingState.Living && t != null && line.ToLower().StartsWith("quit"))
            {
                CancelCurrentAction();
                // todo: test this, might need to wait on it or kill it or something
                if (t.IsAlive)
                    t.Abort(); // messy and likely to fail if sub-threads are pending
            }

        }

        // this starts the first iteration of the assay loop, 
        // subsequent iterations are driven by the SR thread termination, PTR-32 thread termination, or LMMM socket callback 
        // at the end of each cycle
        protected bool StartAssay()
        {
            ResetForMeasurement(); // timer started in here
            bool some = (0 < StartLM_SRAssay());
            if (!some) // each active Active Instrument has its own wait handle, and it was set earlier, waiting point is in the caller
            {
                collog.TraceInformation("No active instruments found, finished");
            }
            return some;
        }

        public void ConnectWithRetries(bool batch, ushort retry)
        {

            if (batch)// the method calling this method is a single command 'batch' that exits when complete, so emit config at start of operations
                LogConfig();
            _completed[1].Reset(1);
            ushort attempts = 0;
            while (attempts < retry && Instruments.Active.ConnectedCount() <= 0 && Instruments.Active.Count > 0)
            {
                ConnectInstruments();  // try to connect to any and all instruments (LMMM, PTR-32, MCA-527 and SR) 
				Thread.Sleep(90);      // wait a moment
                attempts++;
            }
			_completed[1].Signal();
            NC.App.Opstate.StopTimer();
        }

        // set only when all assay or HVCalib cycles are completed, or cancelled
        CountdownEvent [] _completed = {  new CountdownEvent(0),// meas 
										  new CountdownEvent(0) // conn
										};

        public void MajorOperationCompleted()
        {
			if (_completed[0].CurrentCount > 0)
				_completed[0].Signal();
            Flush();
            FireEvent(EventType.ActionStop, this);
        }


        // The HV op control
        public void HVCoreOp()
        {
            if (Instruments.Active.ConnectedLMCount() <= 0 && !Instruments.Active.HasSR()) // LM only and non connected earlier
			{
				NC.App.Opstate.ResetTokens();
                return;  //nothing to do
			}
            NCCAction x = NC.App.Opstate.Action;
            NC.App.Opstate.Action = NCCAction.HVCalibration;
            bool ok = HVCalibInception(); // the current thread pends in this method until the active instrument completes HV processing
			if (ok)
				OutputResults(NCCAction.HVCalibration);
            CurState.StopTimer(); // started in StartHVCalib
            FireEvent(EventType.ActionStop, this);
            NC.App.Opstate.Action = x;
			NC.App.Opstate.ResetTokens();
        }

        public Thread HVCalibOperation()
        {
            Thread hvt = new Thread(HVCoreOp);
            hvt.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            hvt.Start();
            return hvt;
        }

        private bool HVCalibInception()
        {
            _completed[0].Reset(Instruments.Active.Count);
            ApplyInstrumentSettings();
			NC.App.Opstate.SOH = OperatingState.Living;
            bool going = StartHVCalib();
            if (going)
            {
                ManualResetEventSlim[] me = GetTheHVOpWaitTokens(); // each active Instr has it's own analysis handler wait handle
                foreach (ManualResetEventSlim mres in me)
                    mres.Wait(); // wait for signal from DAQ on each active instrument.
            }
			 _completed[0].Wait();
            Thread.Sleep(500); 
            return going;
        }

        // the third requirement from Doug (DONE!)
        // dev note: does this need a progress timer? yes but only while waiting for a step/response, the response is a progress update in and of itself
        private void DoHVCalib()
        {
            applog.TraceInformation("Starting High Voltage Calibration Plateau");
            FireEvent(EventType.ActionPrep, this);
            ConnectWithRetries(true, 3);
            if (Instruments.Active.Count > 0)
            {
                FireEvent(EventType.ActionStart, this);
                FireEvent(EventType.ActionInProgress, this);
                try
                {
                    Thread hvt = HVCalibOperation();  // Start the underlying HV thread, but use the console prompt to pend
                    CommandPrompt(hvt); // shared wait on the thread with user input at the prompt "prompt with wait sync point approach"
                }
                catch (Exception ex)
                {
                    applog.TraceException(ex, true);
                }
                DisconnectInstruments();
                FireEvent(EventType.ActionStop, this);
                applog.TraceInformation("Completed High Voltage Calibration Plateau");
            }
            else
            {
                FireEvent(EventType.ActionStop, this);
                if (_SL != null)
					_SL.Stop();  // shutdown server process
            }
        }

        public bool StartHVCalib()
        {
            ResetForMeasurement();
            bool res = false;
            if (ctrlHVCalib == null)
                ctrlHVCalib = new HVControl(this);

            if (CurState.State == DAQInstrState.HVCalib)  // one already in progress
            {
                CurState.Cancel();
            }
            else
            {
                res = ctrlHVCalib.HVStartCalibration();
            }
            return res;
        }

        public void StepHVCalibration()
        {
            ctrlHVCalib.HVCalibRun();
        }

        public void AppendHVCalibration(HVControl.HVStatus hvst)
        {
            ctrlHVCalib.AddStepData(hvst);
        }





        // The Assay op control
		public void AssayCoreOp()
        {
            if (Instruments.Active.ConnectedLMCount() <= 0 && !Instruments.Active.HasSR()) // LM only and non connected earlier
			{
				NC.App.Opstate.SOH = OperatingState.Stopped;
                return;  //nothing to do
			}
            NCCAction x = NC.App.Opstate.Action;
            NC.App.Opstate.Action = NCCAction.Assay;
            NC.App.Opstate.SOH = OperatingState.Living;
            bool ok = AssayInception();
            if (ok)
            {
                FireEvent(EventType.ActionStart, this);
                FireEvent(EventType.ActionInProgress, this);
                if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
                {
                    //Nothing was saving or displaying..... Don't think HasReportableData is finished HN 9.4.2015
                    if (CurState.Measurement.HasReportableData)
                    {
                        CalculateMeasurementResults();
                        SaveMeasurementBasics();
                        SaveMeasurementResults();
                        FireEvent(EventType.ActionInProgress, this);
                        OutputResults(NCCAction.Assay);
                    }
					else
						collog.TraceEvent(LogLevels.Warning, 0x1A3E, "No reportable results for this measurement");
                }
                NC.App.Opstate.ResetTokens();
            }
            NC.App.Opstate.Action = x;
        }

		public bool AssayInception()
        {
            _completed[0].Reset(Instruments.Active.Count);
            ApplyInstrumentSettings();
            bool task = StartAssay(); // note: data collection occurs in async socket event callbacks through the LM DAQ server, don't need another thread 
            if (task)
            {
                ManualResetEventSlim[] me = GetTheAssayWaitTokens(); // each active Instr has it's own analysis handler wait handle
                foreach (ManualResetEventSlim mres in me)
                   if (mres != null) mres.Wait(); // wait for signal from DAQ + Analyzer on each active instrument.
            }
            _completed[0].Wait(); // wait for all the instr assays to complete (might be more than 1 in NPOD model)
            CurState.StopTimer(); // started in StartAssay > ResetForMeasurement
            Thread.Sleep(500);  // todo: separately config the various waits, grep for Thread.Sleep to find them
            return task;
        }




		public void ThreadAssayCoreOp()
        {
            if (Instruments.Active.ConnectedLMCount() <= 0 && !Instruments.Active.HasSR()) // LM only and non connected earlier
			{
				NC.App.Opstate.SOH = OperatingState.Stopped;
                return;  //nothing to do
			}
            NCCAction x = NC.App.Opstate.Action;
            NC.App.Opstate.Action = NCCAction.Assay;
            NC.App.Opstate.SOH = OperatingState.Living;
            bool ok = false;// AssayInception();
            if (ok)
            {
                FireEvent(EventType.ActionStart, this);
                FireEvent(EventType.ActionInProgress, this);
                if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
                {
                    //Nothing was saving or displaying..... Don't think HasReportableData is finished HN 9.4.2015
                    if (CurState.Measurement.HasReportableData)
                    {
                        CalculateMeasurementResults();
                        SaveMeasurementBasics();
                        SaveMeasurementResults();
                        FireEvent(EventType.ActionInProgress, this);
                        OutputResults(NCCAction.Assay);
                    }
					else
						collog.TraceEvent(LogLevels.Warning, 0x1A3E, "No reportable results for this measurement");
                }
                NC.App.Opstate.ResetTokens();
            }
            NC.App.Opstate.Action = x;
        }

        public Thread AssayOperation()
        {
            Thread at = new Thread(ThreadAssayCoreOp);
            at.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;    
            at.Start();
            return at;
        }


        // self-threaded for console
        private void DoAnAssay()
        {
            applog.TraceInformation("Starting " + CurState.Measurement.MeasOption.PrintName() + " Assay");
            ConnectWithRetries(true, 3);
            if (Instruments.Active.Count > 0)
            {
                FireEvent(EventType.ActionStart, this);
                NC.App.Opstate.SOH = NCC.OperatingState.Living;
                try
                {
                    Thread at = AssayOperation();
                    CommandPrompt(at);  // shared wait on the thread with user input at the prompt "prompt with wait sync point approach"
                }
                catch (Exception ex)
                {
                    applog.TraceException(ex, true);
                }
                DisconnectInstruments();
                FireEvent(EventType.ActionStop, this);
                applog.TraceInformation("Completed Assay");
            }
            else
            {
                FireEvent(EventType.ActionStop, this); 
                if (_SL != null) 
                    _SL.Stop();  // shutdown server process
            }
        }

        public void StopAssay()
        {
            StopLMCAssay(false);
        }
        //public void CancelAssay()
        //{
        //    StopLMCAssay(true);
        //}

        internal void SaveMeasurementBasics()
        {
            CurState.Measurement.Persist();
        }

        /// <summary>
        /// Analysis processing after all cycles are collected
        /// </summary>
        internal void CalculateMeasurementResults(bool ABCorrected = false)
        {
            CurState.Measurement.CalculateMeasurementResults();
        }

        internal void SaveMeasurementResults()
        {
            CurState.Measurement.SaveMeasurementResults();
        }

        public void OutputResults(NCC.NCCAction action = NCC.NCCAction.Nothing)
        {
            if (action == NCCAction.Nothing)
                action = NC.App.Opstate.Action;
            switch (action)  // these are the actions available from the command line only
            {
                case NCCAction.HVCalibration:
                    if (ctrlHVCalib != null)
					{
	                    collog.TraceInformation("Producing HV Calib results output file");
						ctrlHVCalib.GenerateReport();
					}
                    break;

                case NCCAction.Assay:
                    collog.TraceInformation("Producing Assay results output file");
                    ReportMangler rm = new ReportMangler(ctrllog);
                    rm.GenerateReports(CurState.Measurement);
                    NC.App.DB.AddResultsFileNames(CurState.Measurement);
                    break;
            }
        }
        public void ConnectInstruments()
        {
            collog.TraceEvent(LogLevels.Info, 0, "Connecting instruments...");
            ConnectMCAInstruments();

            // for the LMMMs
            ConnectLMMMInstruments();

            // for the PTR-32s
            ConnectPTR32Instruments();

            // now for the SRs, loop over the instrument list and try to connect to each SR via InitializeContext;InitializeSR
            if (Instruments.Active.HasSR())
            {
                PrepSRDAQHandler();
                ConnectSRInstruments();
            }
        }

		public Device.MCADeviceInfo[] ConnectMCAInstruments()
		{
			Device.MCADeviceInfo[] deviceInfos = null;
			if (!Instruments.Active.HasMCA())
				return deviceInfos;
			LMInstrument lmi = (LMInstrument)Instruments.Active.FirstLM();
			if (lmi.id.SRType != InstrType.MCA527)
				return deviceInfos;
			collog.TraceInformation("Broadcasting to MCA instruments. . .");
			try
			{
				deviceInfos = Device.MCADevice.QueryDevices();
				if (deviceInfos.Length > 0)
				{
					Device.MCADeviceInfo thisone = null;
					// Match based on Electronics Id field
					foreach (Device.MCADeviceInfo d in deviceInfos)
					{
						string id = d.Serial.ToString("D5");
						string s = string.Format("MCA-527#{0}  FW# {1}  HW# {2} on {3}", id, d.FirmwareVersion, d.HardwareVersion, d.Address);
						collog.TraceInformation("Checking " + s);
						if (string.Equals(id, lmi.id.ElectronicsId, StringComparison.OrdinalIgnoreCase))
						{
							collog.TraceInformation("Connecting to " + s);
							thisone = d;
							break;
						}
					}
					if (thisone == null)
						return deviceInfos;
					((MCA527Instrument)lmi).DeviceInfo = thisone;
					lmi.Connect();
				}
			} 
			catch (AggregateException ex)
			{
				collog.TraceException(ex.InnerException);
			}
			catch (Exception e)
			{
				collog.TraceException(e);
			}
			return deviceInfos;
		}

		public void ConnectLMMMInstruments()
        {
            if (!Instruments.Active.HasLMMM())
                return;
            LMInstrument lmi = (LMInstrument)Instruments.Active.FirstLM();
			if (lmi.id.SRType != InstrType.LMMM)
				return;
            // for the LMs
            // Start listening for instruments.
            StartLMDAQServer((LMConnectionInfo)lmi.id.FullConnInfo);   // NEXT: socket reset should occur here for robust restart and recovery            
            collog.TraceInformation("Broadcasting to LMMM instruments. . .");

            // broadcast message to all subnet (configurable, defaulting to 169.254.x.x) addresses. This is the instrument group.
            // look for the number of requested instruments
            DAQControl.LMMMComm.PostLMMMCommand(LMMMLingo.Tokens.broadcast);
            collog.TraceInformation("Sent broadcast. Waiting for LMMM instruments to connect");

            // wait until enough time has elapsed to be sure live instruments can report back
            Thread.Sleep(lmi.id.FullConnInfo.Wait);  // todo: configure this with a unique wait parameter value
            if (!LMMMComm.LMServer.IsRunning)
                collog.TraceEvent(LogLevels.Error, 0x2A29, "No socket server for LMMM support running");
        }

        public void ConnectPTR32Instruments()
        {
			bool exists = Instruments.Active.Exists( lm => { return lm.id.SRType == InstrType.PTR32; });
			if (!exists)
				return;
            if (!Instruments.Active.HasUSBBasedLM()) // USB support only
                return;

            foreach (Instrument instrument in Instruments.Active)
            {
                try
                {
                    if (instrument.id.SRType.IsUSBBasedLM())
                    {
                        collog.TraceInformation("Connecting to " + instrument.id.DetectorId);
                        instrument.Connect();
                    }
                }
                catch (Exception ex)
                {
                    collog.TraceException(ex);
                }
            }
        }
        protected void DisconnectInstruments()
        {
            foreach (Instrument active in Instruments.Active)  // disconnect PTR-32 and MCA-527
			{
				active.Disconnect();
            }

            DisconnectFromLMMMInstruments();
            DisconnectFromSRInstruments();
            Instruments.All.Clear();
            collog.TraceInformation("Offline");
            collog.TraceEvent(LogLevels.Info, 0, "Disconnected instruments");
        }

		public void ApplyInstrumentSettings()
		{
			foreach (Instrument instrument in Instruments.Active)
			{
				try
				{
					instrument.ApplySettings();
				} 
				catch (Exception ex)
				{
					collog.TraceException(ex);
				}
			}

			// for now, this is for the LMs only
			if (!Instruments.Active.HasConnectedLM())
				return;

			LMInstrument lm = (LMInstrument)Instruments.Active.AConnectedLM();
			LMConnectionInfo lmc = (LMConnectionInfo)lm.id.FullConnInfo;

			if (lm.id.SRType == InstrType.LMMM) // it's an LMMM
			{
				// look for any flags requiring conditioning of the instrument prior to assay or HV
				// e.g. input=0, the arg to each is already parsed in the command line processing state
				//if (NC.App.Config.LMMM.isSet(LMFlags.input))
				{
					DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.input, lmc.DeviceConfig.Input);
				}
				//if (NC.App.Config.LMMM.isSet(LMFlags.debug))
				{
					DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.debug, lmc.DeviceConfig.Debug);
				}
				//if (NC.App.Config.LMMM.isSet(LMFlags.leds))
				{
					DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.leds, lmc.DeviceConfig.LEDs);
				}
				//if (NC.App.Config.LMMM.isSet(LMFlags.hv))
				{
					DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.hvset, lmc.DeviceConfig.HV);
				}
			} else if (lm.id.SRType == InstrType.PTR32) // its a PTR-32
			{
			} else if (lm.id.SRType == InstrType.MCA527)
			{
			}
		}

		public void Flush()
        {
            NC.App.Loggers.Flush();
        }

        protected void LiveDAQCtrlCHandler(object sender, ConsoleCancelEventArgs args)
        {
            //Announce that the event handler has been invoked.
            ctrllog.TraceInformation("Interrupting the {0} action", NC.App.Opstate.Action);
            args.Cancel = true;  // Set the Cancel property to true to prevent the process from terminating
            NC.App.Opstate.SOH = NCC.OperatingState.Cancelling;
            CurState.Cancel();
        }


#region status display

		public string InstrStatusString(CombinedInstrumentProcessingStateSnapshot cipss, bool channels = false)
		{
			string s = String.Empty;
			if (cipss == null)
				return ("No snapshot");	// NEXT: revisit how this occurs, the status update implementation is broken
			try {
				string id = cipss.iss.action.ToString() + " " + cipss.iss.ins.ToString();  // some identifying info
				if (cipss.iss.ins.IsSuspect)
					s = cipss.iss.ins.Reason + ", " + id;
				else {
					String ss = cipss.cs.ToString() + ";";
					ss += cipss.iss.ToString();
					s = ss + " " + id;
					if (/*channels && cipss.iss.asy != null*/cipss.iss.ins.dsid.SRType == InstrType.LMMM || cipss.iss.ins.dsid.SRType == InstrType.PTR32)
						s += ";  " + cipss.iss.asy.cpss.HitsPerChnImage();
				}
			} catch (System.ObjectDisposedException) {
			}
			return s;
		}

		public string InstrStatusString(object o, bool channels = false)
        {
            Instr.Instrument inst = (Instr.Instrument)o;
            CombinedInstrumentProcessingStateSnapshot cps = new CombinedInstrumentProcessingStateSnapshot(inst);
            return InstrStatusString(cps, channels);
        }

		public string SuccinctInstrStatusString(object o)
        {
            if (o == null)
				return String.Empty;

			Instr.Instrument inst = (Instr.Instrument)o;
			return inst.id.Identifier();
        }
#endregion status display
    }
}
