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
using System.Collections.Generic;
using System.Threading;
using AnalysisDefs;
using DB;
using DetectorDefs;
using ListModeDB;
using NCCConfig;
using NCCReporter;

// dev notes: 
//
// selectable sources for conditioning activity include directives and params from DB or File overridden by cmd line flags (NCCConfig and related TBD)
// selectable sources for data include files/DB/sockets/some generalized data source    (NCDFile/Data Handlers and related TBD)
//
// UI is a separate project
//
// To support UI independence each module must have a similar control API and cmd line flag set.
// The control API to include config/prep/process/pause/cancel/progress status, error conditions and similar events thrown on selected state changes

namespace NCC
{
    public enum OperatingState { Void, Starting, Living, Stopping, Cancelling, Stopped, Trouble };

    // tasks to perform with the NCC6 code
    public enum NCCAction { Nothing = 0, Prompt = 1, Discover = 2, Assay = 3, HVCalibration = 4, Analysis = 5, File = 6, Bonk = 99 }


    // carries current status and support state like 
    // timestamps, cancellation token for ops like 
    // HV calibration, DAQ assay, and file-based analysis
    public class OperationalState
    {

        public DateTime start, stop;
        public NCCAction Action;

        public OperatingState SOH
        {
            get;
            set;
        }
        public Measurement Measurement
        {
            get { return meas; }
            set { meas = value; }
        }
		
        public void ResetTimer(TimerCallback callback, object state = null, int dueTime = Timeout.Infinite, int period = Timeout.Infinite)
        {
            StopTimer();
            if (callback != null)
                statusTimer = new Timer(callback, state, dueTime, period);
        }

        public void StopTimer()
        {
            if (statusTimer != null)
                statusTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void ClearTimers()
        {
            if (statusTimer != null)
            {
                statusTimer.Change(Timeout.Infinite, Timeout.Infinite);
                statusTimer.Dispose();
                statusTimer = null;
            }
        }

        /// <summary>
        /// User requests interruption and optional completion of processing
        /// </summary>
        public bool IsQuitRequested
        {
            get { return csa.IsQuitRequested; }
        }

		public bool IsCancelAbortRequested
        {
            get { return csa.IsCancelAbortRequested; }
        }

		public bool Continue
        {
            get { return csa.Continue; }
        }

        /// <summary>
        /// User requests the current operation be cancelled without persisting or completing 
        /// </summary>
        public bool IsCancellationRequested
        {
            get { return csa.IsCancellationRequested; }
        }
        /// <summary>
        /// User requests aborting current measurement without completing existing data calculations or preserving results
        /// </summary>
        public bool IsAbortRequested
        {
            get { return csa.IsAbortRequested; }
        }
        /// <summary>
        /// User requests stopping current measurement processing, saving and completing the measurement with any existing data 
        /// </summary>
        public bool IsStopRequested
        {
            get { return csa.IsStopRequested; }
        }

		public OperatingState Requested
		{
			get
			{
				if (csa.IsCancellationRequested)
					return OperatingState.Cancelling;
				else if (csa.IsAbortRequested)
					return OperatingState.Cancelling;
				else if (csa.IsStopRequested)
					return OperatingState.Stopping;
				else
					return SOH;
			}
		}

        public string CancelStopAbortStateRep
        {
            get { return csa.StateString; }
        }
        public void Cancel()
        {
            csa.CancellationTokenSource.Cancel();
        }
        public void Stop()
        {
            csa.StopTokenSource.Cancel();
        }
        public void Abort()
        {
            csa.AbortTokenSource.Cancel();
        }
        //public void ResetCancellationToken()
        //{
        //    csa.ResetCancellationToken();
        //}
        public CancelStopAbort CancelStopAbort
        {
            get { return csa; }
        }
        public void ResetTokens()
        { 
            csa.ResetTokens();
        }

        public void StampOperationStartTime()
        {
            start = DateTime.Now;
        }
        public void StampOperationStopTime()
        {
            stop = DateTime.Now;
        }

        public OperationalState()
        {
            csa = new CancelStopAbort();
            SOH = OperatingState.Void;
        }

        public OperationalState(OperationalState src)
        {
            csa = src.csa;
            SOH = src.SOH;
            statusTimer = src.statusTimer;
            meas = src.meas;
            start = src.start;
            stop = src.stop;
        }

        protected Measurement meas;
        protected CancelStopAbort csa;
        protected Timer statusTimer; // ok, I let you have two at the same time

    }
    public class CancelStopAbort
    {
        public CancelStopAbort()
        {
            CancellationTokenSource = new CancellationTokenSource();
            StopTokenSource = new CancellationTokenSource();
            AbortTokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// User requests interruption and optional completion of processing
        /// </summary>
        public bool IsQuitRequested { get { return IsAbortRequested || IsCancellationRequested || IsStopRequested; } }

		public bool IsCancelAbortRequested { get { return IsAbortRequested || IsCancellationRequested ; } }

		public bool Continue { get { return !IsCancelAbortRequested; } }


        public CancellationToken CancellationToken { get { return CancellationTokenSource.Token; } }
        public CancellationTokenSource CancellationTokenSource { get; set; }
		/// <summary>
        /// User requests the current operation be cancelled without persisting or completing 
        /// </summary>
        public bool IsCancellationRequested { get { return CancellationTokenSource.IsCancellationRequested; } }

        public CancellationToken StopToken { get { return StopTokenSource.Token; } }
        public CancellationTokenSource StopTokenSource { get; set; }
		/// <summary>
        /// User requests stopping current measurement processing, saving and completing the measurement with any existing data 
        /// </summary>
        public bool IsStopRequested { get { return StopTokenSource.IsCancellationRequested; } }

		public CancellationToken AbortToken { get { return AbortTokenSource.Token; } }
        public CancellationTokenSource AbortTokenSource { get; set; }
		/// <summary>
        /// User requests aborting current measurement without completing existing data calculations or preserving results
        /// </summary>
        public bool IsAbortRequested { get { return AbortTokenSource.IsCancellationRequested; } }

        public string StateString
        {
            get
            {
                string s = string.Empty;
                if (IsAbortRequested)
                    s = "'abort'";
                if (IsStopRequested)
                {
                    s += (s != string.Empty ? "," : string.Empty);
                    s += "'quit and finish (stop)'";
                }
                if (IsCancellationRequested)
                {
                    s += (s != string.Empty ? "," : string.Empty);
                    s += "'cancel'";
                }
                return (s != string.Empty ? s: "free");
            }
        }

        CancellationTokenSource lcta;
        public CancellationToken NewLinkedCancelStopAbortToken
        {
            get
            {
                CancellationToken[] cta = new System.Threading.CancellationToken[3];
                cta[0] = CancellationToken;
                cta[1] = StopToken;
                cta[2] = AbortToken;
                lcta = CancellationTokenSource.CreateLinkedTokenSource(cta);
                return lcta.Token;
            }
        }
        public CancellationToken LinkedCancelStopAbortToken
        {
            get
            {
                if (lcta == null)
                { 
                    CancellationToken[] cta = new System.Threading.CancellationToken[3];
                    cta[0] = CancellationToken;
                    cta[1] = StopToken;
                    cta[2] = AbortToken;
                    lcta = CancellationTokenSource.CreateLinkedTokenSource(cta);
                }
                return lcta.Token;
            }
        }
        public void ResetLinkedCancelStopAbortToken()
        {
            lcta = null;
        }

        CancellationTokenSource lctac;
        public CancellationTokenSource NewLinkedCancelStopAbortAndClientTokenSources(CancellationToken ct)
        {
                CancellationToken[] cta = new System.Threading.CancellationToken[4];
                cta[0] = CancellationToken;
                cta[1] = StopToken;
                cta[2] = AbortToken;
                cta[3] = ct;
                lctac = CancellationTokenSource.CreateLinkedTokenSource(cta);
                return lctac;
        }
        public CancellationTokenSource LinkedCancelStopAbortAndClientTokenSources(CancellationToken ct)
        {
            if (lctac == null)
            {
                NewLinkedCancelStopAbortAndClientTokenSources(ct);
            }
            return lctac;
        }
        public CancellationToken LinkedCancelStopAbortAndClientToken(CancellationToken ct)
        {
            return LinkedCancelStopAbortAndClientTokenSources(ct).Token;
        }
        public CancellationToken NewLinkedCancelStopAbortAndClientToken(CancellationToken ct)
        {
            return NewLinkedCancelStopAbortAndClientTokenSources(ct).Token;
        }
        public void ResetLinkedCancelStopAbortAndClientToken()
        {
            lctac = null;
        }


        public void ResetTokens()
        { 
            bool tripped = ResetCancellationToken();
            tripped |= ResetStopToken();
            tripped |= ResetAbortToken();

            if (tripped)
            {
                ResetLinkedCancelStopAbortToken();
                ResetLinkedCancelStopAbortAndClientToken();
            }
        }

        bool ResetCancellationToken()
        {
            bool res = false;
            lock (CancellationTokenSource)
            {
                if (CancellationTokenSource.IsCancellationRequested)
                {
                    CancellationTokenSource = new CancellationTokenSource();
                    res = true;
                }
            }
            return res;
        }
        bool ResetStopToken()
        {
            bool res = false;
            lock (StopTokenSource)
            {
                if (StopTokenSource.IsCancellationRequested)
                {
                    StopTokenSource = new CancellationTokenSource();
                    res = true;
                }
            }
            return res;
        }
        bool ResetAbortToken()
        {
            bool res = false;
            lock (AbortTokenSource)
            {
                if (AbortTokenSource.IsCancellationRequested)
                {
                    AbortTokenSource = new CancellationTokenSource();
                    res = true;
                }    
            }
            return res;
        }
    }

    // global state for synchronized operations and access to singleton class instances for logging, configuration, DB access API+parameters
    public class CentralizedState
    {
        public const Int32 ChannelCount = 32; // forever, but what about MCA-527 single channel now eh?

        static public CentralizedState App
        {
            get { return unitary; }
            //set { unitary = value; }
        }

        public Config Config
        {
            get { return cfg; }
            set { cfg = value; }
        }

        public LMLoggers Loggers
        {
            get { return loggers; }
            set { loggers = value; }
        }

        public Persistence Pest
        {
            get { return pest; }
            set { pest = value; }
        }

        public INCCDB DB
        {
            get { return db; }
            set { db = value; }
        }
        public LMDB LMBD
        {
            get { return lmdb; }
            set { lmdb = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string AbbrName
        {
            get { return name.Substring(0, 7); }
        }
        LMINCCAppContext appctx;
        public LMINCCAppContext AppContext
        {
            get { return appctx; }
            set { appctx = value; }

        }
        public bool Initialize(Config cfg)
        {
            this.cfg = cfg;
            bool good = cfg.ParseCommandShellArgs();// override defaults and stored config values with flags from the command line     
            if (good)
            {
                cfg.CmdLineActionOverride(); // override default file input setting with active command action (if any) from cmd line
                name = Config.AppName;
                Opstate.Action = (NCCAction)cfg.Cur.Action;  // command line flag can set this, the override above makes sure the cmd line is the state

                loggers = new LMLoggers(cfg);
                pest.logger = Logger(LMLoggers.AppSection.DB);
            }
            return good;
        }

        public void LoadPersistenceConfig(DBConfig mynewdb)
        {
            pest = new Persistence(Logger(LMLoggers.AppSection.DB), mynewdb);
            DB = new INCCDB();
            DB.Populate(pest);
            lmdb = new ListModeDB.LMDB();
            lmdb.Populate(pest);
            appctx = ListModeDB.LMDB.AppContext;
        }

        public LMLoggers.LognLM Logger(LMLoggers.AppSection wp)
        {
            if (loggers != null)
                return loggers.Logger(wp);
            else
                return null;
        }

        public OperationalState Opstate
        {
            get { return opstate; }
            set { opstate = value; }
        }
        // need a map of these for multi-process UI behaviors.
        // can use many with one logger and config object
        //dev note: add the UI state event on this class too
        OperationalState opstate;




        static CentralizedState unitary;

        // you get just one each of these wonderful baubles for the entire app 
        LMLoggers loggers;
        Config cfg;
        Persistence pest;
        INCCDB db;
        ListModeDB.LMDB lmdb;
        string name;

        public CentralizedState(string AppName)
        {
            opstate = new OperationalState(); // reworked later for assay ops
            name = AppName;
            unitary = this;
        }

    }

    public static class IntegrationHelpers
    {

        /// <summary>
        /// Create a new detector in-memory, with the related classes.
        /// Emulates the INCC relationship constructor, caller must insert new det into global list, then update to DB later for persistence
        /// </summary>
        /// <param name="model">detector to copy</param>
        /// <param name="newId">New detector name</param>
        /// <param name="elecId">Electronics id (just a string)</param>
        /// <param name="typeDesc">Type description (just a string)</param>
        /// <param name="srType">Actual instrument type</param>
        /// <returns>The newly created in-memory Detector class instance</returns>
        static public Detector CreateDetectorWithAssociations(Detector model, string newId, string elecId, string typeDesc, InstrType srType = InstrType.AMSR)
        {
            if (model != null)
            {
                Detector det = new Detector(model); // copies the SR too
                det.Id.SetIdDetails(newId, elecId, typeDesc, model.Id.SRType);

                // copy the model detector's related parameters (skips stratum)
                NormParameters n = CentralizedState.App.DB.NormParameters.Get(model);
                NormParameters Norm = new NormParameters(n);
                BackgroundParameters b = CentralizedState.App.DB.BackgroundParameters.Get(model);
                BackgroundParameters Background = new BackgroundParameters(b);
                UnattendedParameters u = CentralizedState.App.DB.UnattendedParameters.Get(model);
                UnattendedParameters Unatt = new UnattendedParameters(u);
                AddASourceSetup a = CentralizedState.App.DB.AASSParameters.Get(model);
                AddASourceSetup Aass = new AddASourceSetup(a);
                HVCalibrationParameters h = CentralizedState.App.DB.HVParameters.Get(model);
                HVCalibrationParameters Hv = new HVCalibrationParameters(h);

                // add copied param instances to in-memory maps
                CentralizedState.App.DB.NormParameters.Map.Add(det, Norm);
                CentralizedState.App.DB.UnattendedParameters.Map.Add(det, Unatt);
                CentralizedState.App.DB.BackgroundParameters.Map.Add(det, Background);
                CentralizedState.App.DB.AASSParameters.Map.Add(det, Aass);
                CentralizedState.App.DB.HVParameters.Map.Add(det, Hv);

                CentralizedState.App.DB.Detectors.Add(det); // add detector to in-memory list

                return det;
            }
            else
            {
                Detector det = new Detector();
                det.Id.SetIdDetails(newId, elecId, typeDesc,srType);

                if (srType.IsListMode())
                    det.Id.FullConnInfo = new LMConnectionInfo();

                // add fresh param instances to in-memory maps
                CentralizedState.App.DB.NormParameters.Map.Add(det, new NormParameters());
                CentralizedState.App.DB.UnattendedParameters.Map.Add(det, new UnattendedParameters());
                CentralizedState.App.DB.BackgroundParameters.Map.Add(det, new BackgroundParameters());
                CentralizedState.App.DB.AASSParameters.Map.Add(det, new AddASourceSetup());
                CentralizedState.App.DB.HVParameters.Map.Add(det, new HVCalibrationParameters());

                CentralizedState.App.DB.Detectors.Add(det); // add detector to in-memory list

                return det;
            }

            /*  * todo: create analysis selector (or it happens automatically when first referenced?)
                * creating a stratum association
             * */
        }

        /// <summary>
        /// Delete a detector in-memory, and from the DB. Also deletes the params from maps and db.
        /// </summary>
        /// <returns>true if detector is deleted from in-memory map and database</returns>
        static public bool DeleteDetectorAndAssociations(Detector det)
        {
            bool gone = CentralizedState.App.DB.DeleteDetector(det); // removes from DB then from Detectors list, just like isotopics. Delet cascades thorug the datbase, so only in-memory collecitons need to be refreshed
            if (gone)
            {
                // remove from in-memory and database 
                CentralizedState.App.DB.NormParameters.Reset();
                CentralizedState.App.DB.UnattendedParameters.Reset();
                CentralizedState.App.DB.BackgroundParameters.Reset();
                CentralizedState.App.DB.AASSParameters.Reset();
                CentralizedState.App.DB.HVParameters.Reset();
				CentralizedState.App.DB.ResetAcquireParametersMap();
            }
            return gone;
        }


        /// <summary>
        /// Save a detector and its related class instances on the in-memory collections to the database
        /// </summary>
        /// <param name="newdet">The detector to persist</param>
        public static void PersistDetectorAndAssociations(Detector newdet)
        {
            // write detector essentials to DB, detector must be in DB before related insertions 
            try
            {
                CentralizedState.App.DB.UpdateDetector(newdet); // write detector and related sr_parms to database

                BackgroundParameters bp = CentralizedState.App.DB.BackgroundParameters.Get(newdet);
                CentralizedState.App.DB.BackgroundParameters.Set(newdet, bp);

                NormParameters np = CentralizedState.App.DB.NormParameters.Get(newdet);
                CentralizedState.App.DB.NormParameters.Set(newdet, np);

                UnattendedParameters unp = CentralizedState.App.DB.UnattendedParameters.Get(newdet);
                CentralizedState.App.DB.UnattendedParameters.Set(newdet, unp);

                AddASourceSetup aass = CentralizedState.App.DB.AASSParameters.Get(newdet);
                CentralizedState.App.DB.AASSParameters.Set(newdet, aass);

                HVCalibrationParameters hv = CentralizedState.App.DB.HVParameters.Get(newdet);
                CentralizedState.App.DB.HVParameters.Set(newdet, hv);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Construct a full measurement state and set it on the internal lameness
        /// </summary>
        /// <param name="acq">Acquire Parameters</param>
        /// <param name="det">Detector</param>
        /// <param name="mo">MeasurementOption</param>
        public static void BuildMeasurement(AcquireParameters acq, Detector det, AssaySelector.MeasurementOption mo)
        {
            // gather it all together
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(det),
                                    CentralizedState.App.DB.TestParameters.Get(),
                                    GetCurrentNormParams(det),
                                    GetCurrentBackgroundParams(det),
                                    GetAcquireIsotopics(acq),
                                    acq,
                                    GetCurrentHVCalibrationParams(det));
            det.Id.source = acq.data_src;  // set the detector overall data source value here

            // create the context holder for the measurement. Everything is rooted here ...
            Measurement meas = new Measurement(mt, mo, CentralizedState.App.Logger(LMLoggers.AppSection.Data));

            FillOutMeasurement(meas);
            // ready for insertion of methods and processing start

        }

        public static void FillOutMeasurement(Measurement meas)
        {
            if (meas.Detector.ListMode)
            {
                // dev note: design flaw exposed by this mess here, when LM has > 1 SVR, there is no way to associate > 1 SVRs with a single detector/SR/Mult/Conn pairing
                // It should be the other way around, the Mult params for each LMMultiplicity analyzer override the single entry on the sr_parms_rec when each analyis is performed. 

                meas.AnalysisParams = CentralizedState.App.LMBD.CountingParameters(meas.Detector, applySRFromDetector: true);
                if (meas.MeasOption == AssaySelector.MeasurementOption.unspecified) // pure List Mode, not INCC5
                {
                    // for a list-mode-only measurement with a multiplicity analyzer the detector SR params must match at least one of the multiplicity analyzer SR params
                    ApplyVSRChangesToDefaultDetector(meas);
                }
                else // it is an INCC5 analysis driven with LM data
                {
                    // check to see if detector settings are copied into an active CA entry
                    if (!meas.AnalysisParams.Exists(w => { return (w is Multiplicity) && (w as Multiplicity).Equals(meas.Detector.MultiplicityParams) && w.Active; }))
                        meas.AnalysisParams.Add(meas.Detector.MultiplicityParams);
                }
            }
            else
            {
                // prepare analyzer params from detector SR params
                meas.AnalysisParams = CentralizedState.App.LMBD.CountingParameters(meas.Detector, applySRFromDetector: false);
                if (!meas.AnalysisParams.Exists(w => { return (w is Multiplicity) && (w as Multiplicity).Equals(meas.Detector.MultiplicityParams); }))
                    meas.AnalysisParams.Add(meas.Detector.MultiplicityParams);
            }

            // get the INCC5 analysis methods
            meas.INCCAnalysisState = new INCCAnalysisState();
            INCCSelector sel = new INCCSelector(meas.AcquireState.detector_id, meas.AcquireState.item_type);
            AnalysisMethods am;
            bool found = CentralizedState.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out am);
            if (found)
            {
                am.selector = sel; // gotta do this so that the equality operator is not incorrect
                meas.INCCAnalysisState.Methods = am;
            }
            else
                meas.INCCAnalysisState.Methods = new AnalysisMethods(sel);

            meas.InitializeContext();

            meas.PrepareINCCResults();

            // next: stratum not set
            List<INCCDB.StratumDescriptor> sl = CentralizedState.App.DB.StrataList();
            INCCDB.StratumDescriptor s = sl.Find(w => w.Desc.CompareTo(meas.AcquireState.stratum_id) == 0);
            if (s == null)
                meas.Stratum = new Stratum();
            else
                meas.Stratum = new Stratum(s.Stratum);

            INCCResults.results_rec xres = new INCCResults.results_rec(meas);
            meas.INCCAnalysisResults.TradResultsRec = xres;
            CentralizedState.App.Opstate.Measurement = meas;   // put the measurement definition on the global state

        }

        /// <summary>
        /// For a list-mode-only measurement with a multiplicity analyzer the detector SR params must match at least one of the multiplicity analyzer SR params
        /// </summary>
        public static void ApplyVSRChangesToDefaultDetector(Measurement meas)
        {
            if (meas.AnalysisParams.HasMultiplicity()) // devnote: override default detector settings 
            {
                Multiplicity mkey = meas.AnalysisParams.GetFirstMultiplicityAnalyzer();  // hack: multmult just using the first one found, lame, shoud be using closest match
                meas.Detector.MultiplicityParams.CopyValues(mkey);
            }
        }

        /// <summary>
        ///  retrieves the most recent acquire record and associated detector
        /// </summary>
        /// <param name="acq"></param>
        /// <param name="det"></param>
        public static void GetCurrentAcquireDetectorPair(ref AcquireParameters acq, ref Detector det)
        {
            acq = CentralizedState.App.DB.LastAcquire();
            String curdet = acq.detector_id;
            det = CentralizedState.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, curdet, true) == 0);
            if (det == null || string.IsNullOrWhiteSpace(det.Id.DetectorName))
            {
                det = new AnalysisDefs.Detector();
                CentralizedState.App.Logger(LMLoggers.AppSection.App).TraceEvent(LogLevels.Warning, 32443, "Detector " + curdet + " is not defined in the database");
            }
			if (det.ListMode)
				det.MultiplicityParams.FA = acq.lm.FADefault;
        }

        public static AcquireParameters GetCurrentAcquireParams()
        {
            AcquireParameters acq = CentralizedState.App.DB.LastAcquire();
            return acq;
        }

        public static AcquireParameters GetCurrentAcquireParamsFor(Detector det)
        {
            AcquireParameters acq = CentralizedState.App.DB.LastAcquireFor(det);
            if (acq == null)
                acq = new AcquireParameters();
            return acq;
        }

        public static Detector GetCurrentAcquireDetector()
        {
            Detector det = null;
            AcquireParameters acq = CentralizedState.App.DB.LastAcquire();
            string curdet = acq.detector_id;
            det = CentralizedState.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, curdet, true) == 0);
            if (det == null)
                det = new AnalysisDefs.Detector();
			if (det.ListMode)
				det.MultiplicityParams.FA = acq.lm.FADefault;
            return det;
        }

		public static string GetAppTitle()
		{
            AcquireParameters acq = CentralizedState.App.DB.LastAcquire();
			return acq.facility.Name + " - " + CentralizedState.App.Name + " " + CentralizedState.App.Config.VersionString;
		}

        public static AnalysisDefs.Isotopics GetAcquireIsotopics(AcquireParameters acq)
        {
            AnalysisDefs.Isotopics iso = CentralizedState.App.DB.Isotopics.GetList().
                                            Find(i => string.Compare(i.id, acq.isotopics_id, true) == 0);
            if (iso == null)
                iso = CentralizedState.App.DB.Isotopics.GetDefault();
            return iso;
        }

        public static AddASourceSetup GetCurrentAASSParams(Detector det)
        {
            AddASourceSetup aass = new AnalysisDefs.AddASourceSetup();
            if ((det != null) && CentralizedState.App.DB.AASSParameters.Map.ContainsKey(det))
                aass.Copy(CentralizedState.App.DB.AASSParameters.Map[det]);
            return aass;
        }


        public static NormParameters GetCurrentNormParams(Detector det)
        {
            NormParameters np = new AnalysisDefs.NormParameters();
            if ((det != null) && CentralizedState.App.DB.NormParameters.Map.ContainsKey(det))
                np.Copy(CentralizedState.App.DB.NormParameters.Map[det]);
            return np;
        }

        public static BackgroundParameters GetCurrentBackgroundParams(Detector det)
        {
            BackgroundParameters bp = new AnalysisDefs.BackgroundParameters();
            if ((det != null) && (CentralizedState.App.DB.BackgroundParameters.Get(det.Id.DetectorName) != null))
                bp.Copy(CentralizedState.App.DB.BackgroundParameters.Map[det]);
            return bp;
        }

        public static HVCalibrationParameters GetCurrentHVCalibrationParams(Detector det)
        {
            HVCalibrationParameters bp = new AnalysisDefs.HVCalibrationParameters();
            if (det != null)
            {
                if (CentralizedState.App.DB.HVParameters.Map.ContainsKey(det))
                    bp.Copy(CentralizedState.App.DB.HVParameters.Map[det]);
                else if (det.ListMode)
                {
                    AcquireParameters acq = GetCurrentAcquireParamsFor(det);
                    bp = new AnalysisDefs.HVCalibrationParameters(acq.lm);
                }
            }
            return bp;
        }

        public static AnalysisMethods GetMethodSelections(string det, string mat)
        {
            INCCSelector sel = new INCCSelector(det, mat);
            AnalysisMethods lam;
            bool found = CentralizedState.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out lam);
            return lam;
        }
        public static AnalysisMethods GetMethodSelections(AcquireParameters acq)
        {
            return GetMethodSelections(acq.detector_id, acq.item_type);
        }

        public static bool SetNewCurrentDetector(string name, bool checkForExistence)
        {
            AcquireParameters acq = GetCurrentAcquireParams();
			bool exists = true;
            Detector det = CentralizedState.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, name, true) == 0);
            if (det == null)
			{
				exists = false;
				CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Warning, 32441, "Detector " + name + " undefined");
 				if (checkForExistence)
					return false;			
				else
					det = new AnalysisDefs.Detector();
			}
			acq.MeasDateTime = DateTime.Now;
            if (!acq.detector_id.Equals(name, StringComparison.OrdinalIgnoreCase) || !acq.meas_detector_id.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				// change detector on current acquire parms state
				if (!exists)
					CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Warning, 32442, "Temporary detector definition for missing detector " + name + " created");				
				acq.detector_id = string.Copy(name);
				acq.meas_detector_id = string.Copy(name);
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime);
                CentralizedState.App.DB.AddAcquireParams(sel, acq);
				if (!exists)
					CentralizedState.App.DB.Detectors.Add(det);
			}
            else    // update existing entry
                CentralizedState.App.DB.UpdateAcquireParams(acq, det.ListMode);
            CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Info, 32444, "The current detector is now " + name);

			return true;
		}

        public static bool SetNewCurrentMaterial(string name, bool checkForExistence)
        {
            AcquireParameters acq = GetCurrentAcquireParams();
            Detector det = GetCurrentAcquireDetector();
            bool exists = true;
            INCCDB.Descriptor desc = CentralizedState.App.DB.Materials.Get(name);
            if (desc == null)
            {
                exists = false;
                CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Warning, 32441, "Material " + name + " undefined");
                if (checkForExistence)
                    return false;
                else
                    desc = new INCCDB.Descriptor(name,name);
            }
            acq.MeasDateTime = DateTime.Now;
            if (!acq.item_type.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                // change material on current acquire parms state
                if (!exists)
                    CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Warning, 32442, "Temporary material definition " + name + " created");
                acq.item_type = string.Copy(name);
                CentralizedState.App.DB.AddAcquireParams(new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime), acq);
                if (!exists)
                    CentralizedState.App.DB.Materials.Update(desc);
            }
            else
                CentralizedState.App.DB.UpdateAcquireParams(acq, det.ListMode);
            CentralizedState.App.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Info, 32444, "The current material is now " + name);

            return true;
        }
    }


    namespace Utils
    {



        public class ByteByByteFormatter : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                    return this;
                else
                    return null;
            }

            public string Format(string format, object arg,
                                   IFormatProvider formatProvider)
            {
                if (!formatProvider.Equals(this)) return null;

                // Handle only hexadecimal format string.
                if (!format.StartsWith("X")) return null;

                byte[] bytes;
                string output = null;

                // Handle only integral types.
                if (arg is Byte)
                    bytes = BitConverter.GetBytes((Byte)arg);
                else if (arg is Int16)
                    bytes = BitConverter.GetBytes((Int16)arg);
                else if (arg is Int32)
                    bytes = BitConverter.GetBytes((Int32)arg);
                else if (arg is Int64)
                    bytes = BitConverter.GetBytes((Int64)arg);
                else if (arg is SByte)
                    bytes = BitConverter.GetBytes((SByte)arg);
                else if (arg is UInt16)
                    bytes = BitConverter.GetBytes((UInt16)arg);
                else if (arg is UInt32)
                    bytes = BitConverter.GetBytes((UInt32)arg);
                else if (arg is UInt64)
                    bytes = BitConverter.GetBytes((UInt64)arg);
                else if (arg is Byte[])
                    bytes = (Byte[])arg;
                else
                    return null;

                for (int ctr = bytes.Length - 1; ctr >= 0; ctr--)
                    output += String.Format("{0:X2} ", bytes[ctr]);

                return output.Trim();
            }
        }
    }
}


