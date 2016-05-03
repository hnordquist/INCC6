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
using System.ComponentModel;
using System.Threading.Tasks;
using AnalysisDefs;
using DAQ;
using NCC;
using NCCFile;
using NCCReporter;
namespace NewUI
{

    using NC = NCC.CentralizedState;

    public delegate void UILoggingFunc(String s, bool newline = false);


    public partial class Controller
    {
        NCC.MeasurementStatus measStatus;
        public bool updateGUIWithNewData = true;
        public bool updateGUIWithChannelRatesData = false;

        // DAQ operations
        public DAQBind daqbind;

        // FileIO operations
        public FCtrlBind fctrlbind, measFctrl, procFctrl;

        // state
        /// <summary>
        /// use the file or DAQ controller
        /// </summary>
        public bool file;

        public Controller()
        {
        }

        // access global state, but layered apart from form space
        public NCCAction CurAction
        {
            get
            {
                if (file)
                    return fctrlbind.CurAction;
                else
                    return daqbind.CurAction;
            }
        }
        public OperatingState SOH
        {
            get
            {
                if (file)
                    return fctrlbind.SOH;
                else
                    return daqbind.SOH;
            }
        }
        public string FileInput
        {
            get { return NC.App.AppContext.FileInput; }
            set { NC.App.AppContext.FileInput = value; }
        }
        public void Perform()
        {
            if (file)
            {
                if (NC.App.AppContext.INCCXfer)
                    fctrlbind = procFctrl;
                else
                    fctrlbind = measFctrl;
                fctrlbind.StartAction();
            }
            else
                daqbind.StartAction();
        }

        public void ActivateDetector(Detector det = null)
        {
            if (!file)
                DAQControl.ActivateDetector(det);
        }

        /// <summary>
        /// set the next action to be a file transform using all pre-configured settings
        /// </summary>
        public void SetFileTransform()
        {
            NC.App.Opstate.Action = NCCAction.File;
        }
        public void SetDiscover()
        {
            NC.App.Opstate.Action = NCCAction.Discover;
        }

        /// <summary>
        /// set the next action to be an assay using all pre-configured settings
        /// </summary>
        public void SetAssay()
        {
            NC.App.Opstate.Action = NCCAction.Assay;
        }

        /// <summary>
        /// set the next action to be a HV Calib action using all pre-configured settings
        /// </summary>
        public void SetHVCalib()
        {
            NC.App.Opstate.Action = NCCAction.HVCalibration;
        }

        /// <summary>
        /// Load the config file, apply any cmd line params to the config file param state, initialize the logging subsystem, load the database
        /// </summary>
        /// <returns></returns>
        public bool LoadConfiguration()
        {
            new NC("INCC6 GUI");

            // check return bool and exit here on error
            string[] fullargs = Environment.GetCommandLineArgs();
            string[] args = new string[fullargs.Length - 1];
            Array.Copy(fullargs, 1, args, 0, args.Length);
            
            NCCConfig.Config c = new NCCConfig.Config(); // gets DB params
            NC.App.LoadPersistenceConfig(c.DB); // loads up DB, sets global AppContext
            c.AfterDBSetup(NC.App.AppContext, args);  // apply the cmd line 
            bool initialized = NC.App.Initialize(c);
            if (!initialized)
                return false;

            NC.App.Logger(LMLoggers.AppSection.App).
                TraceInformation("==== Starting " + DateTime.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " " + NC.App.Name + " " + NC.App.Config.VersionString);
            // TODO: log DB details at startup too 
            return true;
        }

        /// <summary>
        /// thread-safe progress update method called from down in the DAQ and FileCtrl processing threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Progress(object sender, ProgressChangedEventArgs e)
        {
            bool doingSomething = (e.ProgressPercentage > 0 && e.ProgressPercentage < 100);

            if (!doingSomething && // enable an action if no action is in progress and the overall state is inactive
                (SOH == OperatingState.Void || SOH == OperatingState.Stopped || SOH == OperatingState.Trouble))
            {
            }
        }

        /// <summary>
        /// Load LM config state and create DAQ and FileCtrl controller instances
        /// </summary>
        public void Initialize(Object mainWindowForm)
        {
            if (LoadConfiguration())
            {
                InitializeFileControllers();
                InitializeDAQController();
                measStatus = new MeasurementStatus();
            }
        }

        delegate void ActionCB();

        /// <summary>
        /// Set up action event handlers for a FileCtrl instance, uses FCtrlBind subclass
        /// </summary>
        /// <returns></returns>
        public bool InitializeFileControllers()
        {

            procFctrl = new FCtrlBind();
            measFctrl = new FCtrlBind();
            LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);

            /* 
             * The action event handlers.
             * 
             * There are 7 event types, see ActionEvents.EventType.
             * 
             * The code fires a registered event handler at appropriate processing stages based on the action event type.
             * Unregistered handlers are simply ignored.
             * E.g. The ActionStart event is fired when an action starts. An action is a specific user task such as 'assay'
             * over NCD files or an assay from live LMMM DAQ.
             * 
             * In the examples shown here for NCD file processing, very little reporting actually occurs. 
             * The parameter for each event hnadler is an object, and as yet undefined.
             * I would use delegates to force the appropriate parameter types here, unlike the timer callbacks above.
             * But I've not yet completed this design and implementation 
             * 
             * Implementing these to report complex status to the logger and the UI control is the next step.
             *
             * I do not know which approach will serve us better for a large system, but we do have both timer pull and system push approaches here, so I'll keep them for now.
             *
             */

            /// set up the 7 magic event handlers
            /// here I have a base handler that does the same thing for every event (writes a string to the log),
            /// and I reuse that string by posting it to the progress handler
            measFctrl.SetEventHandler(ActionEvents.EventType.PreAction, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.PreAction, applog, LogLevels.Verbose, o);
                measFctrl.mProgressTracker.ReportProgress(0, s);//  "...");
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionPrep, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionPrep, applog, LogLevels.Verbose, o);
                measFctrl.mProgressTracker.ReportProgress(0, s);//"Prep");
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionStart, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionStart, applog, LogLevels.Verbose, o);
                measFctrl.mProgressTracker.ReportProgress(1, s);//"Starting...");
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionInProgress, (object o) =>
            {
                measStatus = new MeasurementStatus();
                measStatus.UpdateWithMeasurement();
                measStatus.UpdateWithInstruments();
                updateGUIWithChannelRatesData = true;
                string s2 = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionInProgress, applog, LogLevels.Verbose, o);
				if (!string.IsNullOrEmpty(s2))
					s2 = "(" + s2 + ")";
				int per = Math.Abs(Math.Min(100, (int)Math.Round(100.0 * ((double)(measStatus.CurrentRepetition - 1) / (double)measStatus.RequestedRepetitions))));
				try
				{
                    measFctrl.mProgressTracker.ReportProgress(per, // a % est of files
						string.Format("{0} of {1} {2}", measStatus.CurrentRepetition, measStatus.RequestedRepetitions, s2)); // n of m, and file name
				}
				catch (ArgumentOutOfRangeException)
				{
					applog.TraceEvent(LogLevels.Verbose, 58,  "{0} inconsistent", per);
				}				
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionStop, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionStop, applog, LogLevels.Warning, o);
                measFctrl.mProgressTracker.ReportProgress(100, s);//"Stopping...");
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionCancel, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionCancel, applog, LogLevels.Warning, o);
                measFctrl.mProgressTracker.ReportProgress(100, s);//"Cancelling...");
            });

            measFctrl.SetEventHandler(ActionEvents.EventType.ActionFinished, (object o) =>
            {
                string s = "";
                if (o != null && o is FileCtrl)
                {
                    FileCtrl f = (FileCtrl)o;
                        measStatus = new MeasurementStatus();  // preps local state for refresh on channel and computed rates for now, follow up with all other state
                        measStatus.UpdateWithMeasurement();
                        s = FileCtrl.MeasStatusString(measStatus);
                        updateGUIWithChannelRatesData = true;
                        UpdateGUIWithNewdata();
                    }

                NC.App.Opstate.SOH = NCC.OperatingState.Stopped;  // in case we got here after a Cancel
                // general logger: to the console, and/or listbox and/or log file or DB
                applog.TraceEvent(LogLevels.Verbose, FileCtrl.logid[ActionEvents.EventType.ActionFinished], s);

                // specialized updater for UI or file
                measFctrl.mProgressTracker.ReportProgress(100, "Completed");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.PreAction, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.PreAction, applog, LogLevels.Verbose, o);
                procFctrl.mProgressTracker.ReportProgress(0, s);//  "...");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionPrep, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionPrep, applog, LogLevels.Verbose, o);
                procFctrl.mProgressTracker.ReportProgress(0, s);//"Prep");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionStart, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionStart, applog, LogLevels.Verbose, o);
                procFctrl.mProgressTracker.ReportProgress(1, s);//"Starting...");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionInProgress, (object o) =>
            {
                NCCTransfer.TransferEventArgs e = (NCCTransfer.TransferEventArgs)o;
                procFctrl.mProgressTracker.ReportProgress(e.percent, e.msg);
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionStop, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionStop, applog, LogLevels.Warning, o);
                procFctrl.mProgressTracker.ReportProgress(100, s);//"Stopping...");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionCancel, (object o) =>
            {
                string s = FileCtrl.LogAndSkimFileProcessingStatus(ActionEvents.EventType.ActionCancel, applog, LogLevels.Warning, o);
                procFctrl.mProgressTracker.ReportProgress(100, s);//"Cancelling...");
            });

            procFctrl.SetEventHandler(ActionEvents.EventType.ActionFinished, (object o) =>
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Stopped;  // in case we got here after a Cancel
                // general logger: to the console, and/or listbox and/or log file or DB
                applog.TraceEvent(LogLevels.Verbose, FileCtrl.logid[ActionEvents.EventType.ActionFinished]);

                // specialized updater for UI or file
                procFctrl.mProgressTracker.ReportProgress(100, "Completed");
            });

            NC.App.Opstate.SOH = NCC.OperatingState.Void;
            return true;
        }

        /// <summary>
        /// Set up timer callback and action event handlers for a DAQController instance, uses DAQBind subclass
        /// </summary>
        /// <returns></returns>
        public bool InitializeDAQController()
        {
            // DAQ
            daqbind = new DAQBind();
            LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);


            daqbind.SetEventHandler(ActionEvents.EventType.PreAction, (object o) =>
            {
                string s = DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.PreAction, applog, LogLevels.Verbose, o);
				daqbind.mProgressTracker.ReportProgress(0, s);//"...");
            });

            daqbind.SetEventHandler(ActionEvents.EventType.ActionPrep, (object o) =>
            {
                string s = "Action Prep: ";
                //can also look at active instrument list here LogLevels.Verbose,
                s = s + Instr.Instruments.Active.Count + " devices found, [" + NC.App.Opstate.SOH + "] " + DateTimeOffset.Now.ToString("MMM dd yyy HH:mm:ss.ff K");
                applog.TraceEvent(LogLevels.Verbose, 0xABCE, "Action Prep: SOH " + o.ToString() + "'");
				daqbind.mProgressTracker.ReportProgress(0, s);//"Prep");
            });
            daqbind.SetEventHandler(ActionEvents.EventType.ActionStart, (object o) =>
            {
                string s = DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.ActionStart, applog, LogLevels.Verbose, o);
                daqbind.mProgressTracker.ReportProgress(1, s);//"Starting...");
            });

            daqbind.SetEventHandler(ActionEvents.EventType.ActionInProgress, (object o) =>
            {
                measStatus = new MeasurementStatus();
                measStatus.UpdateWithMeasurement();
                measStatus.UpdateWithInstruments();
                updateGUIWithChannelRatesData = true;
                string s2 = DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.ActionInProgress, applog, LogLevels.Verbose, o);
				if (!string.IsNullOrEmpty(s2))
					s2 = "(" + s2 + ")";
				int per = 0;
				try
				{
					switch (daqbind.CurAction)
					{
					case NCCAction.HVCalibration:
						if (measStatus.snaps.iss != null && measStatus.snaps.iss.hv != null)
						{
							per = Math.Abs(Math.Min(100, (int)Math.Round(100.0 * ((double)(measStatus.snaps.iss.hv.HVread - 1) / (double)measStatus.snaps.iss.hv.HVsetpt))));
							daqbind.mProgressTracker.ReportProgress(per, // a % est of steps
								string.Format("{0} volts, with max voltage {1} {2}", measStatus.snaps.iss.hv.HVread, measStatus.snaps.iss.hv.HVsetpt, s2));
						}
						break;
					default:
						per = Math.Abs(Math.Min(100, (int)Math.Round(100.0 * ((double)(measStatus.CurrentRepetition - 1) / (double)measStatus.RequestedRepetitions))));
						daqbind.mProgressTracker.ReportProgress(per, // a % est of files
							string.Format("{0} of {1} {2}", measStatus.CurrentRepetition, measStatus.RequestedRepetitions, s2)); // dev note: need a better focused description of the state
						break;


					}
				}
				catch (ArgumentOutOfRangeException)
				{
					applog.TraceEvent(LogLevels.Verbose, 58,  "{0} inconsistent", per);
				}
            });

            daqbind.SetEventHandler(ActionEvents.EventType.ActionStop, (object o) =>
            {
                string s = DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.ActionStop, applog, LogLevels.Info, o);
                daqbind.mProgressTracker.ReportProgress(100, s);
            });

            daqbind.SetEventHandler(ActionEvents.EventType.ActionCancel, (object o) =>
            {
                string s = DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.ActionCancel, applog, LogLevels.Warning, o);
                daqbind.mProgressTracker.ReportProgress(100, s);
            });

            daqbind.SetEventHandler(ActionEvents.EventType.ActionFinished, (object o) =>
            {
                string s = "";
                if (o != null && o is DAQControl)
                {
                    DAQControl f = (DAQControl)o;
                    measStatus = new MeasurementStatus();  // preps local state for refresh on channel and computed rates for now, follow up with all other state
                    measStatus.UpdateWithMeasurement();
                    s = DAQControl.MeasStatusString(measStatus);
                    updateGUIWithChannelRatesData = true;
                    UpdateGUIWithNewdata();
                }
                else
                {
                    s = "Finished: SOH " + NC.App.Opstate.SOH + " but no processing occurred  " + DAQControl.LogAndSkimDAQProcessingStatus(ActionEvents.EventType.ActionFinished, applog, LogLevels.Verbose, o);
                }
                NC.App.Opstate.SOH = NCC.OperatingState.Stopped;  // in case we got here after a Cancel
                // general logger: to the console, and/or listbox and/or log file or DB
                applog.TraceEvent(LogLevels.Verbose, FileCtrl.logid[ActionEvents.EventType.ActionFinished], s);
                // specialized updater for UI or file
                daqbind.mProgressTracker.ReportProgress(100, s);

            });

            return true;
        }


        public void UpdateGUIWithNewData(MainWindow df)
        {
            updateGUIWithNewData = true;
            NC.App.Logger(LMLoggers.AppSection.App).TraceInformation("!!!!");
        }

        static LMLoggers.LognLM lawg;
        void UpdateGUIWithNewdata()
        {
            if (lawg == null)
                lawg = NC.App.Logger(LMLoggers.AppSection.App);
            if (lawg.ShouldTrace(LogLevels.Verbose))
                lawg.TS.TraceEvent((System.Diagnostics.TraceEventType)LogLevels.Verbose, 0, "$$$");

        }

        public void Close()
        {
            NC.App.Opstate.SOH = OperatingState.Stopped;
            NC.App.Config.RetainChanges();
            LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);
            applog.TraceInformation("==== Exiting " + DateTimeOffset.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " " + NC.App.Name + " . . .");
            NC.App.Loggers.Flush();
            //DisconnectUILogger = true;
        }

    }

    // dev note: notice the similarities between the two bind classes here, and those in the LM project; unify them into a single interface that must be implemented by all clients

    /// <summary>
    /// Class for controlling file processing from a UI or other controlling client code
    /// </summary>
    public class FCtrlBind : FileCtrl
    {

        public Task task;
        public ProgressTracker mProgressTracker;
        public FCtrlBind()
            : base(true)
        {                         
            mProgressTracker = new ProgressTracker();
        }


        // controller has active control of processing through these three entry points
        public new void StartAction()
        {
            mProgressTracker = new ProgressTracker();
            task = Task.Factory.StartNew(() => ThreadOp(null, null), NC.App.Opstate.CancelStopAbort.LinkedCancelStopAbortToken); 
            string titletext = (NC.App.AppContext.DBDataAssay ? "Database " : "File ") + (NC.App.Opstate.Action == NCCAction.Assay ? "Analysis" : "Processing");
            Progress.ProgressDialog.Show(null,  titletext, NC.App.Name, task, NC.App.Opstate.CancelStopAbort, mProgressTracker, NC.App.Opstate.Action == NCCAction.Assay);
        }

        public new void CancelCurrentAction()
        {
            base.CancelCurrentAction();
            Cleanup();
        }

        public new void StopCurrentAction()
        {
            base.StopCurrentAction();
            Cleanup();
        }

        void ThreadOp(object sender, DoWorkEventArgs ea)
        {
            try
            {
                base.StartAction();
                Run();
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);
                applog.TraceException(e, true);
                applog.EmitFatalErrorMsg();
            }
            Cleanup();
        }

        void Done(object sender, RunWorkerCompletedEventArgs e)
        {
            Cleanup();
        }

        public OperatingState SOH
        {
            get { return NC.App.Opstate.SOH; }
        }
        public NCCAction CurAction
        {
            get { return NC.App.Opstate.Action; }
        }
    }


    /// <summary>
    /// Class for controlling live DAQ from a UI or other controlling client code
    /// </summary>
    public class DAQBind : DAQControl
    {
        public Task task;
        public ProgressTracker mProgressTracker;

        public DAQBind()
            : base(true)
        {            
            mProgressTracker = new ProgressTracker();
        }

		public new void Run()
        {
            try
            {
                switch (NC.App.Opstate.Action)  // these are the actions available from the GUI only
                {
                    case NCCAction.Discover:
                        FireEvent(EventType.PreAction, this);  
						ConnectWithRetries(false, 3);
                        ApplyInstrumentSettings();
                        FireEvent(EventType.ActionFinished, this);
                        break;
                    case NCCAction.HVCalibration:
                        FireEvent(EventType.PreAction, this);
                        ConnectWithRetries(false, 3);
                        ApplyInstrumentSettings();
                        HVCoreOp();
                        FireEvent(EventType.ActionFinished, this);
                        break;
                    case NCCAction.Assay:
                        FireEvent(EventType.PreAction, this);
						ConnectWithRetries(false, 3);
                        ApplyInstrumentSettings();
                        AssayCoreOp();
                        DisconnectInstruments();
                        FireEvent(EventType.ActionFinished, this);
                        break;
                }

                NC.App.Loggers.Flush();
                NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);
                applog.TraceException(e, true);
                applog.EmitFatalErrorMsg();
                FireEvent(EventType.ActionFinished, this);
            }
        }

        void ThreadOp(object sender, DoWorkEventArgs ea)
        {
            try
            {
                base.StartAction();
                Run();
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);
                applog.TraceException(e, true);
                applog.EmitFatalErrorMsg();
            }
			base.Cleanup();
        }

        void Done(object sender, RunWorkerCompletedEventArgs e)
        {
			Cleanup();
        }

        public new void StartAction()
        {
            mProgressTracker = new ProgressTracker();
            task = Task.Factory.StartNew(() => ThreadOp(null, null), NC.App.Opstate.CancelStopAbort.LinkedCancelStopAbortToken);
            Progress.ProgressDialog.Show(null, "DAQ " + NC.App.Opstate.Action.ToString(), NC.App.Name, task, NC.App.Opstate.CancelStopAbort, mProgressTracker, NC.App.Opstate.Action == NCCAction.Assay);
        }

        public new void CancelCurrentAction()
        {
            base.CancelCurrentAction();
			Cleanup();
        }

        public new void StopCurrentAction()
        {
            base.StopCurrentAction();
			Cleanup();
        }

        public NCCAction CurAction
        {
            get { return NC.App.Opstate.Action; }
        }
        public OperatingState SOH
        {
            get { return NC.App.Opstate.SOH; }
        }
    }
	 
}





