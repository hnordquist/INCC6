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

	public partial class DAQControl : ActionEvents//, IActionStatus
    {

        // currently supports both assay and HV calib actions, but not file-based computing
        public class AssayState : OperationalState
        {
            public AssayState(AssaySelector.MeasurementOption at = AssaySelector.MeasurementOption.unspecified)
            {
            }

            public AssayState(OperationalState os) : base(os)
            {
            }


            private DAQInstrState _state;

            public bool broadcastGo;

            public string currentDataFilenamePrefix;

            public DAQInstrState State
            {
                get { return _state; }
                set { _state = value; }
            }

            public void StampAssayTime() // affix start time for use in file naming and more
            {
                StampOperationStartTime();
            }
            public void GenDateTimePrefix(string loc) // file naming
            {
                if (!loc.EndsWith("\\"))  // might be some URI interface to abstract this better, this is platform dependent
                {
                    loc += "\\";
                }
                currentDataFilenamePrefix = loc + start.ToString("yyyy_MM_dd_HHmmss");
            }

        }

        /// <summary>
        /// Global progress state for live assay and hvcalib operations.  If ops are to be threaded this is to be locked 
        /// </summary>
        static public AssayState CurState
        {
            get { return (AssayState)NC.App.Opstate; }
            set { NC.App.Opstate = value; }
        }

        void ResetForMeasurement()
        {
            CurState.Measurement.CurrentRepetition = 0;
            CurState.ResetTokens();
            CurState.broadcastGo = true; //  ((LMInstrument)Instruments.Active.FirstActive().id.FullConnInfo).NetComm.B; //devnote: geez this idea sure didn;t work out
            //CurState.ResetTimer(1, tupleprocessing, Instruments.Active.FirstActive(), 250, (int)NC.App.AppContext.StatusTimerMilliseconds);
            CurState.Measurement.PrepareINCCResults();
        }

        // Instruments.All Maintains a list of instruments connected.
        // Instruments.Active points to the ones selected in the listBoxInstr listBox.

        public Instrument FirstActiveInstrument(Type t)
        {
            return Instruments.Active.FirstActive(t);
        }
        public int FirstActiveIndexOf(Instrument ind)
        {
            return Instruments.Active.IndexOfActiveInst(ind);
        }

		/// <summary>
        /// Prepare a detector for live assay/HV by adding it to the active list.
        /// </summary>
        /// <param name="det"></param>
        public static void ActivateDetector(Detector det)
        {
            if (det.Id.SRType.IsSocketBasedLM())
            {
				if (det.Id.SRType == InstrType.LMMM)
				{
					LMInstrument lm = new LMInstrument(det);
					lm.DAQState = DAQInstrState.Offline; 
					lm.selected = false;  //must broadcast first to get it selected
					if (!Instruments.All.Contains(lm))
						Instruments.All.Add(lm); // add to global runtime list
				} 
				else if (det.Id.SRType == InstrType.MCA527)
				{
                    MCA527Instrument mca = new MCA527Instrument(NC.App.Opstate.Measurement.Detector);
                    mca.DAQState = DAQInstrState.Offline; 
                    mca.selected = true;
                    if (!Instruments.Active.Contains(mca))
                        Instruments.Active.Add(mca);   
				}
				else
				{
                    // LMMM and MCA-527 are the only socket-based devices supported
				}
            }
            else if (det.Id.SRType.IsUSBBasedLM())
            {
                Ptr32Instrument ptr = new Ptr32Instrument(det);
                ptr.DAQState = DAQInstrState.Offline; // these are manually initiated as opposed to auto-pickup
                if (!Instruments.All.Contains(ptr))
                    Instruments.All.Add(ptr); // add to global runtime list
            }
        }


        /// <summary>
		/// Threaded handler for Shift Register DAQ support
		/// </summary>
        private SRTakeDataHandler _sr;
        public SRTakeDataHandler SRWrangler
        {
            get
            {
                if (_sr == null)
                {
                    // you will need to assign your current measurement object to the handler before performing an assay 
                    _sr = new SRTakeDataHandler(NC.App.Logger(LMLoggers.AppSection.Collect));
                    _sr.Meas = CurState.Measurement;
                }
                return _sr;
            }
        }

		/// <summary>
		/// Socket server instance for LMMM DAQ support
		/// </summary>
        private Server _SL;

        //  why are these static? because of the coding style with the socket callbacks, static could be removed at any time with a bit of reorg
        private static Int32 connections = 0;
        private static long packets = 0;

        private static ulong numTotalBytes;

        public bool collectingFileData = false;

        public HVControl ctrlHVCalib = null;

        /// <summary>
        ///
        /// </summary>
        internal void StartLMDAQServer(LMConnectionInfo lmc = null)
        {
            if (_SL == null)
            {
                LMMMNetComm lmn;
                if (lmc == null)
                {
                    // get the one associated with selected current detector, and if not available, then get the last one used 
					Detector det = IntegrationHelpers.GetCurrentAcquireDetector();
					if (det.Id.SRType.IsSocketBasedLM())
						lmc = (DetectorDefs.LMConnectionInfo)det.Id.FullConnInfo;
					else 
						lmc = NC.App.LMBD.LastBestConnInfo();
                }
                lmn = lmc.NetComm;

                _SL = new Server(lmn.Port, lmn.NumConnections, lmn.ReceiveBufferSize, lmn.subnetip);
                _SL.Logger = collog;
                LMMMComm.LMServer = _SL;  // server root visible to COMM class 
                _SL.clientConnected += SL_ClientConnected;
                _SL.DataReceived += SL_DataReceived;
            }

            if (_SL.IsRunning)
            {
                ctrllog.TraceEvent(LogLevels.Verbose, 123, "The LM DAQ Server is already running");
                return;
            }
            else
            {
                _SL.Start();
            }

            CurState.State = DAQInstrState.Offline;
        }
        internal void PrepSRDAQHandler()
        {
            SRTakeDataHandler sr = SRWrangler;  // creates if not there
            sr.Meas = CurState.Measurement;
        }

		public void Cleanup()
        {
            NC.App.Opstate.ClearTimers();
            GC.Collect();
        }

		
        internal bool CheckCOMPortExistence(int comport)
        {
			string check = "COM" + comport.ToString();
            string[] ports = System.IO.Ports.SerialPort.GetPortNames(); // "COMnnn"

            foreach (string p in ports)
            {
                if (p.CompareTo(check) == 0)
                {
                    return true;
                }
            }
            return false;
        }

		// need to call this AFTER the instruments are identified from the sys config
		internal void ConnectSRInstruments()
		{

			// start the control thread for each SR and and make sure it is waiting for a command
			IEnumerator iter = Instruments.Active.GetSREnumerator();
			while (iter.MoveNext())
			{
				SRInstrument sri = (SRInstrument)iter.Current;
				bool found = false;
				try // Fix case where crashes because COM port doesn't exist any more..... hn 5.15.2015
				{
					if (sri.id.SRType.IsCOMPortBasedSR()) // always true if code gets you here 
					{
						if (!CheckCOMPortExistence(sri.id.SerialPort))  // if not found then emit error
						{
							collog.TraceInformation("The COM port {0} is no longer available. Instrument {1} cannot be connected. Please set a valid COM port.", sri.id.SerialPort.ToString(), sri.id.DetectorId);
							string[] ports = System.IO.Ports.SerialPort.GetPortNames(); // "COMnnn"
							string ps = string.Empty;
							if (ports.Length == 0)
								ps = "No COM ports found";
							else
							{
								ps = "These COM ports are available:";
								foreach (string p in ports)
								{
									ps += (" " + p + ",");
								}
							}
							collog.TraceInformation(ps);
							sri.selected = false;
						} else
							found = true;
					}
				} catch (Exception ex)
				{
					collog.TraceException(ex);
				}
				if (!found)
				{
					Instruments.Active.RemoveAll(i => i.selected == false);
					return;
				}

				sri.SRCtrl = SRWrangler.StartSRControl(sri, pceh: (sender, args) =>  // the report progress eh
					{
						SRControl srctrl = args.UserState as SRControl;
						if (srctrl.IsInitialized)
							SRWrangler.Logger.TraceEvent(LogLevels.Verbose, 383, "{0}, SRControl {1}: SR status '{2}', SR Control Status '{3}' ({4})",
								args.ProgressPercentage, srctrl.Identifier,
								INCCSR.SRAPIReturnStatusCode(srctrl.LastSRStatus), INCCSR.SRAPIReturnStatusCode(srctrl.LastMeasStatus), srctrl.fraction);
						else
						{
							SRWrangler.Logger.TraceEvent(LogLevels.Verbose, 384, "{0}, SRControl {1}", args.ProgressPercentage, srctrl.Identifier);
						}
					},
					// the operation complete eh
					opeh: SREventHandler
					);
			}

			iter = Instruments.Active.GetSREnumerator();
			// associate each new SR thread with the current measurement 
			while (iter.MoveNext())
			{
				SRInstrument sri = (SRInstrument)iter.Current;
				SRWrangler.StartSRActionAndWait(sri.id, SRTakeDataHandler.SROp.InitializeContext);
			}

			// devnote: the attempt to connect occurs in StartLMCAssay
		}

		void DisconnectFromSRInstruments()
        {
            IEnumerator iter = Instruments.Active.GetSREnumerator();
            // stop all connected instruments and close the connections
            while (iter.MoveNext())
            {
                SRInstrument sri = (SRInstrument)iter.Current;
                SRWrangler.StopThread(sri, true);
            }
        }

        void DisconnectFromLMMMInstruments()
        {
            if (Instruments.Active.HasLMMM())
            {
                if (_SL == null)
                    return;
                // Stop incoming accepts
                _SL.Stop();
                IEnumerator iter = Instruments.Active.GetLMEnumerator();
                // Close all connected instruments
                while (iter.MoveNext())
                {
                    LMInstrument lmi = (LMInstrument)iter.Current;
                    if (lmi.id.SRType.IsSocketBasedLM())   /// URGENT: implement a new interface implemented for LMMM and MCA-527 separately
                        _SL.StopClient(lmi.instrSocketEvent);
                }
            }
        }

        // still need to test if this works  
        //         typing 'stop' at prompt stops the current cycle, but not the overall processing state, this method should now stop the entire process
        // devnote: Could just use a timer-based check of the token, same as for progress polling into the analyzer
        /// <summary>
        /// send the cancel command to each LMMM, set the instrument state Online to prevent data processing of any additional packets 
        /// </summary>
        /// <param name="removeCurNCDFile">delete the current NCD files created for the current interval</param>
        private void StopLMCAssay(bool removeCurNCDFile)
        {
            collog.TraceEvent(LogLevels.Info, 0, "Stopping assay...");
            CurState.State = DAQInstrState.Online;

            // stop each instrument in the active list
            foreach (Instrument active in Instruments.Active)
            {
                //This new from USB version incc6, commenting out has no effect on doubles/triples errors hn 9/22/2014
                active.StopAssay(); // for PTR-32 and MCA527

                // send cancel command to the LMMM instruments
                if (active is LMInstrument && (active.id.SRType == InstrType.LMMM))
                {
                    LMInstrument lmi = active as LMInstrument;
                    LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.cancel, 0, Instruments.Active.RankPositionInList(lmi)); // send to all active, 1 by 1
                    if (collectingFileData && lmi.file != null)
                    {
                        lmi.file.CloseWriter();
                        if (removeCurNCDFile)
                            lmi.file.Delete();
                    }
                    active.DAQState = DAQInstrState.Online;
                }
                else if (active is SRInstrument)
                {
                    // tell the SR thread handler to cancel it
                    ctrllog.TraceInformation("Stop SR {0}", active.id.Identifier());
                    SRWrangler.StopThread(active as SRInstrument, true);
                    active.DAQState = DAQInstrState.Offline;
                }
            }

            ctrllog.TraceInformation("Assay cancelled");
            NC.App.Loggers.Flush();
        }


        /// <summary>
        /// Glom the wait handles from each active instrument's RDT
        /// </summary>
        /// <returns>ManualResetEvent array of event handles, one per instrument </returns>
        private ManualResetEventSlim[] GetTheAssayWaitTokens()
        {
            ManualResetEventSlim[] m = new ManualResetEventSlim[Instruments.Active.Count];
            int i = 0;
            foreach (Instrument active in Instruments.Active)
            {
                m[i] = active.RDT.GetManualResetEvent();
                i++;
            }
            return m;
        }

        // 
        /// <summary>
        /// Glob the wait handles from each active instrument involved in the HV Calib dance
        /// </summary>
        /// <returns>array of event handles, one per instrument</returns>
        private ManualResetEventSlim[] GetTheHVOpWaitTokens()
        {
            return GetTheAssayWaitTokens();
        }

        /// <summary>
        /// Start an assay on every instrument connected, this is for a single interval only
        /// The LMMM does just one assay from it's POV, and needs to be retold by DAQ here to do the next interval
        /// </summary>
        private int StartLM_SRAssay()
        {
            collog.TraceEvent(LogLevels.Info, 0, "Starting assay...");

            CurState.Measurement.CurrentRepetition++;
            CurState.State = DAQInstrState.ReceivingData;

            numTotalBytes = 0;

            collectingFileData = NC.App.AppContext.LiveFileWrite;

            CurState.StampAssayTime();
            CurState.GenDateTimePrefix(NC.App.AppContext.RootLoc);

            foreach (Instrument active in Instruments.Active)
            {
                if (active is SRInstrument)
                {
                    ctrllog.TraceEvent(LogLevels.Verbose, 999333, "Got SR {0} here", (active as SRInstrument).id.Identifier());
                }
                active.DAQState = DAQInstrState.ReceivingData;

                Cycle cycle = new Cycle(ctrllog);
                cycle.SetUpdatedDataSourceId(active.id); // where the cycle came from, but with updated timestamp
                CurState.Measurement.Add(cycle);  // todo: this mixes the cycles from the different instruments onto one list, gotta change this now that we are at more than one instrument, well you can simply write iterators that select on specific instrument Ids, over the entire list, or use LINQ select * where dsid == whatever syntax on the list

                // devnote: file writing is selectable via the UI, and raw analysis should be independently
                // start the file capture
                if (active is LMInstrument)
                {
                    NCCFile.INeutronDataFile f = (active as LMInstrument).PrepFile(CurState.currentDataFilenamePrefix, Instruments.Active.IndexOf(active), collog);
                    active.RDT.StartCycle(cycle, f); // internal handler needs access to the file handle for PTR-32, but not for LMMM
                }
                else
                    active.RDT.StartCycle(cycle);

                if (CurState.Measurement.CurrentRepetition == 1)
                {
                    //  devnote: with more than one active, these need to be revisited
                    if (active is LMInstrument)
                    {
                        (active as LMInstrument).RDT.SetupCountingAnalyzerHandler(NC.App.Config, active.id.source.TimeBase(active.id.SRType));
                        (active as LMInstrument).RDT.PrepareAndStartCountingAnalyzers(CurState.Measurement.AnalysisParams);
                    }

                    // stamp instrument time on measurement id
                    // todo: Measurement Detector List exists but is not integrated with associated Instrument objects here
                    // CurState.Measurement.MeasDate = active.id.dt;

                    if (active is SRInstrument)
                    {
                        // kick off the thread to try and init the SR
                        SRWrangler.StartSRActionAndWait((active as SRInstrument).id, SRTakeDataHandler.SROp.InitializeSR);
                    }
                    else if (active is LMInstrument && active.id.SRType == InstrType.LMMM)
                    {
						DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.prep, 0, Instruments.All.IndexOf(active)); // send this config message to this LMMM
                    }

                    // devnote: index might be wrong if some of multiple LMs are disabled via UI. This will require a revisit at integration time 
                }
            }

            NC.App.Loggers.Flush();
            FireEvent(EventType.ActionInProgress, this);
            Thread.Sleep(250); // wait for last send to finish, todo could we use EventHandler<SocketAsyncEventArgs> Completed here?

            // PTR-32
            // This loop works for PTR-32 (and soon MCA-527) instruments, based on an improved instrument and control design

            // devnote: rewrite SR and LMMM sections below to use the StartAssay virtual method abstraction for measurement control
            foreach (Instrument instrument in Instruments.Active) {
                try {
                    instrument.StartAssay(CurState.Measurement);
                }
                catch (Exception ex) {
                    collog.TraceException(ex);
                }
            }

            // The following sections are for SR and LMMM 
            // LMMM
            if (Instruments.Active.HasLMMM())  // send to a plurality of thresholding units, err, I mean, LMMM Instruments
            {
                if (CurState.broadcastGo) // send go
                {
                    // this has to be sent separately, because linux control is looking for the Arm alone.
                    DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.arm); // send to all active

                    Thread.Sleep(250);  // allow linux code to setup waiting socket.

                    // broadcast go message to all NCC.App.Config.Net.Subnet addresses.    This is the instrument group.
                    DAQControl.LMMMComm.PostLMMMCommand(LMMMLingo.Tokens.go, true);
                }
                else
                {
                    DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.go); // send to all active
                }
            }

            // SR
            bool srgood = false, hasSR = false;
            // send the start DAQ to every active SR, probably better to do this in the event handler not here
            IEnumerator iter = Instruments.Active.GetSREnumerator();
            while (iter.MoveNext())
            {
                hasSR = true;
                SRInstrument sri = (SRInstrument)iter.Current;
                int srStatus = 0, measStatus = 0;
                SRWrangler.GetLastStatus(sri.id, ref srStatus, ref measStatus);

                if (measStatus != INCCSR.SUCCESS) // failure means we cannot see or use the SR, so go to the next one, whereas success means it initialized correctly in the InitializeSR step
                {
                    sri.DAQState = DAQInstrState.Offline;
                    continue;
                }

                int status = SRWrangler.StartSRActionAndWait(sri.id, SRTakeDataHandler.SROp.StartSRDAQ);  // NEXT: check if pending here is going to be an issue
                if (status == INCCSR.MEAS_CONTINUE)  // the SR started
                {
                    srgood = true;
                    SRWrangler.SetAction(sri.id, SRTakeDataHandler.SROp.WaitForResults); // event handler will pick up results when the internal timer polling in the thread detects results and fires the event
                }
            }
            // need a better test in here to skip all this startup stuff when the SR/LM/cycle init failed
            if (Instruments.Active.Count < 1)
            {
                CurState.Measurement.CurrentRepetition--;
                ctrllog.TraceEvent(LogLevels.Warning, 46, "No active instruments available now");
            }
            else if (hasSR && !srgood)
            {
                CurState.Measurement.CurrentRepetition--;
                ctrllog.TraceEvent(LogLevels.Warning, 46, "No Shift Register is available now");
            }
            else
            {
                if (CurState.Measurement.CurrentRepetition == 1)
                {
                    if (Instruments.Active.Count > 1)
                    {
                        if (Instruments.Active.Count != Instruments.All.Count)
                            ctrllog.TraceInformation("Using " + Instruments.Active.Count + " of " + Instruments.All.Count + " instruments");
                        else
                            ctrllog.TraceInformation("Using " + Instruments.Active.Count + " instruments");
                    }
                    else
                    {
                        ctrllog.TraceInformation("Using one instrument");
                    }
                }

                string str = "Assay cycle " + CurState.Measurement.CurrentRepetition + " of ";
                if (CurState.Measurement.RequestedRepetitions == 0)
                    str += "a continuous assay starting";
                else
                    str += (CurState.Measurement.RequestedRepetitions + " cycles starting");
                ctrllog.TraceInformation(str);
            }
            if (Instruments.Active.Count > 0)
            { 
                FireEvent(EventType.ActionInProgress, this);
                collog.TraceEvent(LogLevels.Verbose, 0, "Started assay with {0} instrument{1}", Instruments.Active.Count, (Instruments.Active.Count > 1 ? "s" : ""));
            }
            return Instruments.Active.Count;
        }

        static public void ReadInstrStatus(LMInstrument lmi)
        {
            DAQControl.LMMMComm.FormatAndSendLMMMCommand(LMMMLingo.Tokens.cstatus, 0, Instruments.Active.RankPositionInList(lmi));
        }


        #region Status event and timer callbacks

        static public string MeasStatusString(Measurement m)
        {
            MeasurementStatus ms = new MeasurementStatus();
            return MeasStatusString(ms);
        }
        static public string MeasStatusString(MeasurementStatus ms)
        {
            return ms.ToString();
        }

		public static string LogAndSkimDAQProcessingStatus(EventType EH, LMLoggers.LognLM log, LogLevels lvl, object o)
		{
			LoggableDAQProcessingStatus(EH,log,lvl,o);
			return SuccinctDAQProcessingStatus(o);
		}
        public static string SuccinctDAQProcessingStatus(object o)
        {
            string s = String.Empty;
            if (o != null)
            {
                string ss = string.Empty;
                if (o is DAQControl)
                {
                    DAQControl d = (DAQControl)o;
					ss = d.SuccinctInstrStatusString(Instruments.Active.FirstActive());
                }
                else if (o is Measurement)
                {
                    Measurement m = (Measurement)o;
                    ss = DAQControl.MeasStatusString(m);
                }
                s += ss;
            }
            return s;
		}
        public static string LoggableDAQProcessingStatus(EventType EH, LMLoggers.LognLM log, LogLevels lvl, object o)
        {
            string s = String.Empty;
            if (o != null)
            {
                string ss = string.Empty;
                if (o is DAQControl)
                {
                    DAQControl d = (DAQControl)o;
                    ss = d.InstrStatusString(Instruments.Active.FirstActive(), true);
                }
                else if (o is Measurement)
                {
                    Measurement m = (Measurement)o;
                    ss = DAQControl.MeasStatusString(m);
                }
                s += ss;
            }
            log.TraceEvent(lvl, DAQControl.logid[EH], s);
            return s;  // just in case it could be of further use
        }
		
        #endregion Status event and timer callbacks
    }
}