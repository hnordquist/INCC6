/*
Copyright 2016, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Collections;
using System.Collections.Generic;
using DetectorDefs;
using LMRawAnalysis;
using NCCReporter;
namespace AnalysisDefs
{

    using NC = NCC.CentralizedState;

    /// <summary>
    /// A Measurement is created using one or more Instruments
    /// Each Instrument has one or more detectors 
    /// A Detector has its id, SR params and alpha-beta
    /// 
    /// With LM, an instrument is discovered and created for live DAQ, an associated detector is then created for each virtual SR specified by the user for that instrument.
    /// For SR, a detector is first created by user, with specific params and a type and such, at run-time, when a SR is found on the serial port, an instrument is created and placed on the global Instrument DAQ list.
    /// In both cases there is an instrument to SR spec relationship that always exists. Each instrument can have one SR detector, and one or more virtual detectors in the context of a measurement. Muddy clear!
    /// </summary>
    public class Detector : Tuple<DataSourceIdentifier, Multiplicity, AlphaBeta>, IComparable<Detector>
    {

        public Detector()
            : base(new DataSourceIdentifier(), new Multiplicity(), new AlphaBeta())
        {
        }

        //simple id only
        public override string ToString()
        {
            return Item1.DetectorId;
        }

        public Detector(Detector src)
            : base(new DataSourceIdentifier(src.Item1), new Multiplicity(src.Item2), new AlphaBeta(src.Item3))
        {
        }

        public Detector(DataSourceIdentifier dsid, Multiplicity mul, AlphaBeta ab)
            : base(new DataSourceIdentifier(dsid), new Multiplicity(mul), new AlphaBeta(ab))
        {
        }
       
        public ShiftRegisterParameters SRParams
        {
            get { return Item2.SR; }
        }

        public Multiplicity MultiplicityParams
        {
            get { return Item2; }
        }

        public DataSourceIdentifier Id
        {
            get { return Item1; }
        }

        public AlphaBeta AB
        {
            get { return Item3; }
        }

        public static int Compare(Detector x, Detector y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = 0;
            if (x.Item1 != null)
                res = x.Item1.CompareTo(y.Item1);
            if (res == 0 && (x.Item2 != null))
                res = x.Item2.CompareTo(y.Item2);
            if (res == 0 && (x.Item3 != null))
                res = x.Item3.CompareTo(y.Item3);

            return res;
        }

        public int CompareTo(Detector other)
        {
            return Compare(this, other);
        }

        /// <summary>
        /// This is a List Mode detector (LMMM, PTR-32, etc.)
        /// </summary>
        public bool ListMode
        {
            get { return Id.SRType.IsListMode(); }
        }

    }

    public class DetectorList : List<Detector>
    {
        public DetectorList()
        {
        }

        public DetectorList(Detector first)
        {
            Add(first);
        }

        public Detector GetIt(DataSourceIdentifier dsid)
        {
            Detector d = null;
            d = Find(det => det.Item1.Equals(dsid));
            return d;
        }

        public Detector GetItByDetectorId(string id)
        {
            Detector d = null;
            d = Find(det => det.Item1.DetectorId.Equals(id));
            return d;
        }
        public void Replace(Detector det)
        {
            Detector d = GetItByDetectorId(det.Id.DetectorId);
            if (d == null)
                Add(det);
            else
            {
                Remove(d);
                Add(det);
            }

        }
        public void AddOnlyIfNotThere(Detector det)
        {
            Detector d = GetItByDetectorId(det.Id.DetectorId);
            if (d == null)
                Add(det);
        }

    }

    public static class AssaySelector
    {
        public enum MeasurementOption // the term is from INCC
        {
            rates = 0, // the default operation
            background = 1, initial = 2, normalization = 3, precision = 4, verification = 5, calibration = 6, holdup = 7, /* high voltage plateau = 10, */ unspecified = 99
        }

        public static bool ForMass(MeasurementOption mo)
        {
            return mo == MeasurementOption.verification || mo == MeasurementOption.calibration || mo == MeasurementOption.holdup;
        }
        public static bool HasStratum(MeasurementOption mo)
        {
            return mo == MeasurementOption.verification || mo == MeasurementOption.holdup;
        }
        public static bool DougsBasics(MeasurementOption mo)
        {
            return mo == MeasurementOption.rates || mo == MeasurementOption.background;
        }

        /// <summary>
        ///  anything but background and precision
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        public static bool UsesBackground(MeasurementOption mo)
        {
            return ForMass(mo) || mo == MeasurementOption.rates || mo == MeasurementOption.initial || mo == MeasurementOption.normalization;
        }

       /// <summary>
       /// Convert from integer or string signifier to the internal enumerated type.
       /// Command line utility.
       /// </summary>
       /// <param name="v">string rep of enum, or integer</param>
       /// <param name="res">true iff successful conversion</param>
       /// <returns></returns>
        public static bool AssayTypeConv(string v, out int res)
        {
            MeasurementOption a = MeasurementOption.rates;
            res = 0;
            bool ok = int.TryParse(v, out res); // is it a integer? If so, convert to the enum
            if (!ok)  // OMFG not an integer, look for string name and convert appropriately
            {
                string lv = v.ToLower();
                ok = Enum.TryParse(lv, out a);
                res = (int)a;
            }
            return ok;
        }
    }

    public static class AssaySelectorExtensions
    {

        static Dictionary<AssaySelector.MeasurementOption, string> PrintableName;
        static Dictionary<AssaySelector.MeasurementOption, string> Suffix;

        static AssaySelectorExtensions()
        {
            if (PrintableName == null)
            {
                PrintableName = new Dictionary<AssaySelector.MeasurementOption, string>();
                PrintableName.Add(AssaySelector.MeasurementOption.rates, "Rates");
                PrintableName.Add(AssaySelector.MeasurementOption.background, "Background");
                PrintableName.Add(AssaySelector.MeasurementOption.initial, "InitialSource");
                PrintableName.Add(AssaySelector.MeasurementOption.normalization, "Normalization");
                PrintableName.Add(AssaySelector.MeasurementOption.precision, "Precision");
                PrintableName.Add(AssaySelector.MeasurementOption.verification, "Verification");
                PrintableName.Add(AssaySelector.MeasurementOption.calibration, "Calibration");
                PrintableName.Add(AssaySelector.MeasurementOption.holdup, "Holdup");
                PrintableName.Add(AssaySelector.MeasurementOption.unspecified, "Unspecified");
            }

			if (Suffix == null)
            {
                Suffix = new Dictionary<AssaySelector.MeasurementOption, string>();
                Suffix.Add(AssaySelector.MeasurementOption.rates, "RTS");
                Suffix.Add(AssaySelector.MeasurementOption.background, "BKG");
                Suffix.Add(AssaySelector.MeasurementOption.initial, "INS");
                Suffix.Add(AssaySelector.MeasurementOption.normalization, "NOR");
                Suffix.Add(AssaySelector.MeasurementOption.precision, "PRE");
                Suffix.Add(AssaySelector.MeasurementOption.verification, "VER");
                Suffix.Add(AssaySelector.MeasurementOption.calibration, "CAL");
                Suffix.Add(AssaySelector.MeasurementOption.holdup, "HUP");
                Suffix.Add(AssaySelector.MeasurementOption.unspecified, "txt");
            }
        }

        public static string PrintName(this AssaySelector.MeasurementOption src)
        {
            return PrintableName[src];
        }

		public static string INCC5Suffix(this AssaySelector.MeasurementOption src)
        {
            return Suffix[src];
        }

		public static bool IsWildCard(this AssaySelector.MeasurementOption src)
		{
			return src == AssaySelector.MeasurementOption.unspecified;
		}

		public static bool IsListMode(this AssaySelector.MeasurementOption src)
		{
			return src == AssaySelector.MeasurementOption.unspecified;
		}

        public static AssaySelector.MeasurementOption SrcToEnum(this string src)
        {
            AssaySelector.MeasurementOption res = AssaySelector.MeasurementOption.unspecified;

            if (string.IsNullOrEmpty(src))
                return res;

            bool match = false;
            foreach (KeyValuePair<AssaySelector.MeasurementOption, string> pair in PrintableName)
            {
                if (pair.Value.Equals(src))
                {
                    res = pair.Key;
                    match = true;
                    break;
                }
            }

            if (!match)  // non-null string that does not match, try a direct conversion
				Enum.TryParse(src, out res);
            return res;
        }
    }

    //  map of INCCSelector(detectors, materials) ->  AnalysisMethods map
    public class INCCAnalysisMethodMap : Dictionary<INCCSelector, AnalysisMethods>
    {

        public INCCAnalysisMethodMap()
            : base(new INCCSelector.SelectorEquality())
        {
        }
    }

    public class CycleList : List<Cycle>
    {
        public CycleList()
        {
            hitsPerChn = new Double[NC.ChannelCount];
        }

        // hits per channel, can be huge numbers for a long cycle ;`)
        public double[] HitsPerChannel
        {
            get { return hitsPerChn; }
        }
        double[] hitsPerChn;

        public IEnumerator<Cycle> GetValidCycleList()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusStronglyValid)
                    yield return this[i];
            }
        }
        public uint GetValidCycleCount()
        {
            uint j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusStronglyValid)
                    j++;
            }
            return j;
        }

		/// <summary>
        /// Count of cycles useful for INCC5 analyses.
        /// </summary>
        /// <returns>Useable cycle count</returns>
        public uint GetUseableCycleCount()
        {
            uint j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusWeaklyValid)
                    j++;
            }
            return j;
        }

        public IEnumerator<Cycle> GetValidCycleListForThisKey(Multiplicity mkey)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusValid(mkey))
                    yield return this[i];
            }
        }
        public IEnumerator<Cycle> GetFilteredCycleListForThisKey(QCTestStatus filter, Multiplicity mkey)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusHasFlag(filter, mkey))
                    yield return this[i];
            }
        }
        public uint GetStatusCycleCountForThisKey(QCTestStatus filter, Multiplicity mkey)
        {
            uint j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusHasFlag(filter, mkey))
                    j++;
            }
            return j;
        }
        public uint GetValidCycleCountForThisKey(Multiplicity mkey)
        {
            uint j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].QCStatusValid(mkey))
                    j++;
            }
            return j;
        }

        public uint GetTotalCycleCountForThisKey(Multiplicity mkey)
        {
            uint j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].HasEntry(mkey))
                    j++;
            }
            return j;
        }

		public bool HasReportableCyclesForThisKey(Multiplicity mkey)
		{
			return Count > 0 && GetValidCycleCountForThisKey(mkey) > 0;			
		}

        /// <summary>
        /// One or more cycles, with any status whatsoever, AND
        /// if measurement is using an SR or a virtual SR, at least one cycle must have a calculated status
        /// </summary>
		public bool HasReportableCycles
		{
			get
			{
                return Count > 0 &&   
                    GetUseableCycleCount() > 0; // 1 or more useful for the INCC5 analyses;
			}
		}

        public void ResetStatus(Multiplicity mkey, QCTestStatus st = QCTestStatus.None)
        {
            for (int i = 0; i < this.Count; i++)
            {
               this[i].SetQCStatus(mkey, st);
            }
        }

        /// <summary>
        /// Remove all cycles starting after the first unset cycle on the list.
        /// useful for catching cancelled measurements in-place
        /// </summary>
        /// <param name="mkey"></param>
        public void Trim(Multiplicity mkey)
        {
            int start = FindIndex(e => { return e.QCStatus(mkey).Unset(); });
            if (start < 0)
                return;
            RemoveRange(start, this.Count - start);
            TrimExcess();
        }
    }

    public class MeasurementCycleStatusCounts
    {
        public uint num_checksum_failures;
        public uint num_acc_sngl_failures;
        public uint num_outlier_failures;
        public uint num_high_voltage_failures;
        public uint initial_num_runs;
        public uint acquire_num_runs;

        public bool acc_sngl_warning_sent;
        public bool checksum_warning_sent;
        public bool high_voltage_warning_sent;
    }

    /// <summary>
    /// Input - Acquire Context & Method Selections & Detector Params 
    /// Generate Measurement 
    /// Output - Generic Results and Specific Results
    /// </summary>
    public partial class Measurement
    {

        public DateTimeOffset MeasDate
        {
            get { return mid.MeasDateTime; }
            set { mid.MeasDateTime = value; }
        }
        public AssaySelector.MeasurementOption MeasOption
        {
            get { return mid.MeasOption; }
            // set { mid.MeasOption = value; }
        }
        public ResultFiles ResultsFiles; // the CSV file with general SR and LM results (non-mass) and cycle summaries per analysis,  INCC5-style text files; can have multiple output files if more than one SR params

        public double CountTimeInSeconds;
        public ushort CurrentRepetition;
        public ushort RequestedRepetitions
        {
            get { return (Detector.ListMode ? (ushort) AcquireState.lm.Cycles : AcquireState.num_runs); }
            set { if (Detector.ListMode) AcquireState.lm.Cycles = value; else AcquireState.num_runs = value; }  // questionable use to stop acquire cycles looping
        }

        /// <summary>
        /// The sum of all the counts across all the cycles
        /// </summary>
        public double SinglesSum
        {
            get { return singles; }
        }

        /// <summary>
        /// Each detector has an SR Params. Each SR Params is mapped to pre-analyzers and so on
        /// A Detector has its id, SR params and alpha-beta
        /// 
        /// With LM, an instrument is discovered and created for live DAQ, an associated detector is then created for each virtual SR specified by the user for that instrument.
        /// For SR, a detector is first created by user, with specific Parms and a type and such, at run-time, when a SR is found on the serial port, an instrument is created and placed on the global Instrument DAQ list.
        /// In both cases there is an instrument to SR spec relationship that always exists. Each instrument can have one SR detector, and one or more virtual detectors in the context of a measurment. Muddy clear!
        ///
        /// </summary>
        public DetectorList Detectors
        {
            get { return mt.Detectors; }
        }

		/// <summary>
		/// The single detector used for a measurement. Multiple active detectors are not used 
		/// </summary>
		public Detector Detector
        {
            get { return mt.Detectors[0]; }
        }

        /// <summary>
        ///  The test params are global to all detectors involved in the measurement 
        /// </summary>
        public TestParameters Tests
        {
            get { return mt.Item2; }
        }
        /// <summary>
        ///  the normalization params are associated with the detector list 
        /// </summary>
        public NormParameters Norm
        {
            get { return mt.Item3; }
        }

        /// <summary>
        ///  the background params are associated with the detector list 
        /// </summary>
        public BackgroundParameters Background
        {
            get { return mt.Item4; }
        }
        /// <summary>
        ///  a single global value for the measurement context 
        /// </summary>
        public Isotopics Isotopics
        {
            get { return mt.Item5; }
        }
        /// <summary>
        ///  a single global value for the measurement context
        /// </summary>
        public AcquireParameters AcquireState
        {
            get { return mt.Item6; }
        }
        /// <summary>
        ///  a single global value for the measurement context, checked against global strata map before caller uses in mass calculations
        /// </summary>
        public Stratum Stratum
        {
            get;
            set;
        }
        private MeasurementTuple mt;

        private MeasId mid;
        public MeasId MeasurementId
        {
            get { return mid; }
            set { mid = value; }
        }

        // Sums
        private double singles; // also counted by rate analyzer and the byte transform code

        /// <summary>
        /// Results from the first analyzer pass over raw data, typically includes at least one multiplicity counting result
        /// </summary>
        private CountingResults countresults;

        /// <summary>
        /// List of all cycles for this measurement
        /// </summary>
        private CycleList cycles;

        /// <summary>
        /// AAS positional cycles lists
        /// </summary>
        private List<CycleList> cfcycles;
        /// <summary>
        /// Logger handle
        /// </summary>
        internal LMLoggers.LognLM logger;
        public LMLoggers.LognLM Logger { get { return logger; } }

        /// <summary>
        /// INCC analyses add messages to the measurement results, on a per-SR/LM entry point basis
        /// </summary>
        public AnalysisMessages Messages;


        internal List<MeasurementMsg> LookupMessageList(Multiplicity mul)
        {
            List<MeasurementMsg> sl = null;
            bool ok = Messages.TryGetValue(mul, out sl);
            if (!ok)
            {
                sl = new List<MeasurementMsg>();
                Messages.Add(mul, sl);
            }
            return sl;
        }

        // These are live adders, they use for now for the dt ts
        // NEXT: DB restore messages whilst retaining timestamps
        public List<MeasurementMsg> GetMessageList(Multiplicity mul)
        {
            return LookupMessageList(mul);
        }

        public void AddMessage(List<MeasurementMsg> msgs, LogLevels level, int id, string s, DateTimeOffset dt)
        {
            msgs.Add(new MeasurementMsg(level, id, s, dt));
        }

        public void AddErrorMessage(string s, int id, Multiplicity mul)
        {
            LookupMessageList(mul).Add(new MeasurementMsg(LogLevels.Error, id, s));
            logger.TraceEvent(LogLevels.Error, id, s);
        }
        public void AddWarningMessage(string s, int id, Multiplicity mul)
        {
            LookupMessageList(mul).Add(new MeasurementMsg(LogLevels.Warning, id, s));
            logger.TraceEvent(LogLevels.Warning, id, s);
        }
        public void AddDireMessage(string s, int id, Multiplicity mul)
        {
            LookupMessageList(mul).Add(new MeasurementMsg(LogLevels.Critical, id, s));
            logger.TraceEvent(LogLevels.Critical, id, s);
        }


        /// <summary>
        /// Parameters prepared for each of the raw data counting analyzers
        /// </summary>
        private CountingAnalysisParameters ap;
        /// <summary>
        /// parameters prepared for each of the raw data counting analyzers
        /// </summary>
        public CountingAnalysisParameters AnalysisParams
        {
            get { return ap; }
            set { ap = value; }
        }

        /// <summary>
        /// The combined measurement state that defines the processing of cycles and the final analyses. A copy of this is included in the final measurement results
        /// </summary>
        private INCCAnalysisState ias;
        public INCCAnalysisState INCCAnalysisState
        {
            get { return ias; }
            set { ias = value; }
        }

        /// <summary>
        /// Results from the INCC processing steps, only contains multiplicity counting results processed for stat and method
        /// </summary>
        public INCCResults INCCAnalysisResults
        {
            get { return INCCAnalysisState.Results; }
        }

        /// <summary>
        /// Results from the first analyzer pass over raw data, may not include multiplicity counting result
        /// </summary>
        public CountingResults CountingAnalysisResults
        {
            get { return countresults; }
        }

        /// <summary>
        /// List of all cycles for this measurement
        /// </summary>
        public CycleList Cycles
        {
            get { return cycles; }
        }
        /// <summary>
        /// List of all AAS pos cycles for this measurement
        /// </summary>
        public List<CycleList> CFCycles
        {
            get { return cfcycles; }
        }
        private MeasurementCycleStatusCounts cyclestatus;

        public MeasurementCycleStatusCounts MeasCycleStatus
        {
            get { return cyclestatus; }
            set { cyclestatus = value; }
        }
        public bool FirstCycle; // reset at first cycle

		/// <summary>
		/// True iff there is at least one complete cycle with non-zero total elapsed time.
		/// And if list mode 
		///    and a multiplicity analyzer is defined
		///      at least one must have a cycle with a calculated status
		///    else
		///      one or more cycles exist
		/// else (using SR data where there is always a multiplicity analyser defined)
		///    at least one must have a cycle with a calculated status
		/// </summary>
		public bool HasReportableData
		{
			get
			{
				if (Detector.ListMode)
				{
					if (AnalysisParams.HasMultiplicity())
						return Cycles.HasReportableCycles;
					else
						return Cycles.Count > 0;
				} else
					return Cycles.HasReportableCycles;
			}
		}

		/// <summary>
		/// Constructs a new Measurement object with default values
		/// </summary>
		/// <param name="at">The measurement method or goal</param>
		/// <param name="logger">the logger handle</param>
		public Measurement(AssaySelector.MeasurementOption at, LMLoggers.LognLM logger)
        {
            mt = new MeasurementTuple();
            this.logger = logger;
            mid = new MeasId(at);
            InitMisc();
        }

        public Measurement(MeasurementTuple newMT, AssaySelector.MeasurementOption at, LMLoggers.LognLM logger)
        {
            mt = newMT;
            this.logger = logger;
            mid = new MeasId(at);
            InitMisc();
        }

        public Measurement(MeasurementTuple newMT, LMLoggers.LognLM logger)
        {
            mt = newMT;
            this.logger = logger;
            InitMisc();
        }


        /// <summary>
        /// Imprint a new measurement with as much information as possible from a results_rec.
        /// </summary>
        /// <param name="rec">The results_rec with the measurement details</param>
        /// <param name="meaId">Unique id for the measurement, from the results_rec fields</param>
        /// <param name="logger">logger handle</param>
        /// <returns>A new measurement</returns>
        public Measurement (INCCResults.results_rec rec, MeasId meaId, LMLoggers.LognLM logger)
        {
            HVCalibrationParameters hv = NCC.IntegrationHelpers.GetCurrentHVCalibrationParams(rec.det);
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(rec.det), rec.tests, rec.norm, rec.bkg, rec.iso, rec.acq, hv);
            this.mt = mt;
            this.logger = logger;
            mid = meaId;
            InitMisc();
			
            if (rec.det.ListMode)
            {
                AnalysisParams =  NC.App.LMBD.CountingParameters(rec.det, applySRFromDetector:true);
                if (meaId.MeasOption.IsListMode()) // pure List Mode, not INCC5
                {
                    // for a list-mode-only measurement with a multiplicity analyzer the detector SR params must match at least one of the multiplicity analyzer SR params
                    NCC.IntegrationHelpers.ApplyVSRChangesToDefaultDetector(this);
                }
                else // it is an INCC5 analysis driven with LM data
                {
                    // prepare or identify an active CA entry with matching CA gatewidth and FA, with remaining SR params as the detector
                    AnalysisParams.PrepareMatchingVSR(rec.det.MultiplicityParams);
                }
            }
            else
            {
                // prepare analyzer params from detector SR params
                AnalysisParams = NC.App.LMBD.CountingParameters(rec.det, applySRFromDetector:false);
                if (!AnalysisParams.Exists(w => { return (w is Multiplicity) && (w as Multiplicity).Equals(rec.det.MultiplicityParams); }))
                    AnalysisParams.Add(rec.det.MultiplicityParams);
            }

            // get the INCC5 analysis methods
            INCCAnalysisState = new INCCAnalysisState();
            INCCSelector sel = new INCCSelector(rec.acq.detector_id, rec.acq.item_type);
            AnalysisMethods am;
            bool found = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out am);
            if (found)
            {
                am.selector = sel; // gotta do this so that the equality operator is correct
                INCCAnalysisState.Methods = am;
            }
            else
                INCCAnalysisState.Methods = new AnalysisMethods(sel);
            InitializeContext(clearCounterResults: true);
            PrepareINCCResults();
            // a list mode measurement may not have a multiplicity analyzer at all, create on results, copying the current values
            if (CountingAnalysisResults.ContainsKey(rec.det.MultiplicityParams))
			{ 
				MultiplicityCountingRes mcr = (MultiplicityCountingRes)CountingAnalysisResults[rec.det.MultiplicityParams];
				if (rec.mcr.AB.Unset)
						SDTMultiplicityCalculator.SetAlphaBeta(rec.det.MultiplicityParams, rec.mcr); // works only if MaxBins is set
				mcr.CopyFrom(rec.mcr); // copy the mcr results onto the first moskey entry
				// the same results are copied to the full results structure			
				MeasOptionSelector mos = new MeasOptionSelector(MeasOption, rec.det.MultiplicityParams);
				INCCResult result = INCCAnalysisState.Lookup(mos);
				result.CopyFrom(rec.mcr);
			}

			Stratum = new Stratum(rec.st); // the stratum from the results rec
        }

        private void InitMisc()
        {
            countresults = new CountingResults();
            cycles = new CycleList();
            Messages = new AnalysisMessages();
            cyclestatus = new MeasurementCycleStatusCounts();
			ResultsFiles = new ResultFiles();
        }

        /// <summary>
        /// Add a new cycle to the measurement list
        /// </summary>
        /// <param name="c">the Cycle instance</param>
        public void Add(Cycle c, int cfindex = -1)  // cf index only for AAS positional cycles
        {
            if (cfindex == -1)
            {
                cycles.Add(c);
                c.seq = cycles.Count;
            }
            else  // use the AAS lists
            {
                if (cfcycles == null)
                {
                    cfcycles = new List<CycleList>(INCCAnalysisParams.MAX_ADDASRC_POSITIONS + 1);
                    for (int j = 0; j <= INCCAnalysisParams.MAX_ADDASRC_POSITIONS; j++)
                        cfcycles.Add(new CycleList());
                }
                cfcycles[cfindex].Add(c);
                c.seq = cfcycles[cfindex].Count;
            }
            FirstCycle = true;  // used by outlier test
        }

        /// <summary>
        /// Add the list of prepared cycles to this measurement's cycle list.
        /// Expected use is during database-driven measurements reconstruction
        /// </summary>
        /// <param name="cl">list of cycles</param>
        /// <param name="init">if true, reset related measurement attributes, e.g. requested repetitions</param>
        public void Add(CycleList cl, bool init = true) 
        {
            cycles.AddRange(cl);
            if (init)
            {
                CurrentRepetition = 0;
                RequestedRepetitions = (ushort)cycles.Count;
                FirstCycle = true;  // used by outlier test
                if (cycles.Count > 0 &&  AcquireState.run_count_time == 0)
                    AcquireState.run_count_time = cycles[0].TS.TotalSeconds;
            }
            else if (cycles.Count > 0) // adjust sequence numbers
            {
                int lastseq = cycles[cycles.Count - 1].seq;
                foreach (Cycle c in cl)
                    c.seq += lastseq; 
            }

        }

        public void InitializeResultsSummarizers()
        {
            logger.TraceEvent(LogLevels.Verbose, 4042, "Initialize results summarizers (countresults)");
            countresults.Clear();
            try
            {
                int idx = 0;
                foreach (BaseRate b in (ap.GetBases()))
                {
                    countresults.Add(b, new RatesResultEnhanced(1, b.gateWidthTics));
                }
                idx = 0;
                foreach (Multiplicity muon in (ap.GetMults(FAType.FAOn)))
                {
                    countresults.Add(muon, new MultiplicityCountingRes(FAType.FAOn, idx)); idx++;
                }
                idx = 0;
                foreach (Multiplicity muoff in (ap.GetMults(FAType.FAOff)))
                {
                    countresults.Add(muoff, new MultiplicityCountingRes(FAType.FAOff, idx)); idx++;
                }

                foreach (Rossi ro in (ap.GetRossis()))
                {
                    countresults.Add(ro, new RossiAlphaResultExt(10, new uint[RawAnalysisProperties.numRAGatesPerWindow]));
                }

                foreach (Feynman fy in (ap.GetFeynmans()))
                {
                    countresults.Add(fy, new FeynmanResultExt());
                }

                foreach (TimeInterval ti in (ap.GetTimeIntervals()))
                {
                    TimeIntervalResult tir = new TimeIntervalResult();
                    tir.timeIntervalHistogram = new uint[RawAnalysisProperties.maxEventSpacing + 1];  // alloc the entire possible range, but use maxIndexOfNonzeroHistogramEntry upon output
                    countresults.Add(ti, tir);
                }

                foreach (Coincidence co in ap.GetCoincidences())
                {
                    CoincidenceMatrixResult cor = new CoincidenceMatrixResult(RawAnalysisProperties.ChannelCount);
                    cor.coincidenceDeadDelay = co.SR.predelay;
                    cor.accidentalsDelay = co.AccidentalsGateDelayInTics;
                    cor.coincidenceGateWidth = co.SR.gateLength;
                    cor.isSlowBackground = true; // see constructor in LMRawAnalysis.CoincidenceAnalysisSlowBackGround.GetResult()
                    countresults.Add(co, cor);
                }

            }
            catch (Exception ex)
            {
                logger.TraceEvent(LogLevels.Error, 4041, "Unable to create counting analyzers: " + ex.Message);
            }
        }
        public void InitializeContext(bool clearCounterResults = true)
        {
            try
            {
                if (clearCounterResults)
					InitializeResultsSummarizers();
                MeasCycleStatus.acquire_num_runs = AcquireState.num_runs; // may need improvement when considering active/passive combined measurements (active_num_runs)
                MeasCycleStatus.initial_num_runs = AcquireState.num_runs;
            }
            catch (Exception ex)
            {
                logger.TraceEvent(LogLevels.Error, 4041, "Measurement context: " + ex.Message);
            }
        }

        // all non-zero cycles are counted here
        public uint NumberOfRawCyclesWithCounts()
        {
            uint validCyclesCount = 0;
            foreach (Cycle cc in cycles) 
            {
                    if (cc.TS.Ticks > 0)
                        validCyclesCount++;
            }
            return validCyclesCount;
        }

        // summarizes using all cycles that have passed evaluation (e.g. marked as Pass)
        public uint CycleSummary(bool ignoreSuspectResults = true)
        {
            long t = 0;                 // the cycle average time
            ulong totals = 0;            // singles raw sums

            for (int i = 0; i < NC.ChannelCount; i++)
                cycles.HitsPerChannel[i] = 0;

            foreach (Cycle cc in cycles)  // counts merge, totals and time come from the multiplicity analyzers only. this needs to change
            {
                if (!ignoreSuspectResults || cc.QCStatusStronglyValid)
                {
                    t += cc.TS.Ticks;
                    totals += cc.Totals;
                    for (int i = 0; i < NC.ChannelCount; i++)
                        cycles.HitsPerChannel[i] += cc.HitsPerChannel[i];
                }
            }
            uint validCyclesCount = cycles.GetValidCycleCount();
            CountTimeInSeconds = new TimeSpan(t).TotalSeconds;
            singles = totals;
            return validCyclesCount;
        }

        /// <summary>
        /// create the results structures, including the methods map from (detector+multcounter) -> (detector+material) -> (results, method results)
        /// </summary>
        public void PrepareINCCResults()
        {
            IEnumerator iter = CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.Multiplicity));
            while (iter.MoveNext())
            {
				bool existed = false;
                Multiplicity mcr = (Multiplicity)iter.Current;
                try
                {
                    existed = INCCAnalysisState.PrepareINCCResults(MeasOption, mcr, (MultiplicityCountingRes)CountingAnalysisResults[mcr]);
					if (!existed) // it was created just now in PrepareINCCResults
	                    logger.TraceEvent(LogLevels.Verbose, 4028, "Preparing INCC {0} results for {1}", MeasOption.PrintName(), mcr.ToString());
                    existed = INCCAnalysisState.PrepareINCCMethodResults(mcr, new INCCSelector(INCCAnalysisState.Methods.selector),this);
 					if (!existed) // it was created just now in PrepareINCCMethodResults
						logger.TraceEvent(LogLevels.Verbose, 4029, "Preparing INCC method {0} results for {1}", INCCAnalysisState.Methods.selector.ToString(), mcr.ToString());
                }
                catch (Exception ex)
                {
                    logger.TraceEvent(LogLevels.Error, 4027, "PrepareINCCResults error: " + ex.Message);
                }
            }
        }


        /// <summary>
        /// Store the metadata identifier and the generalized results of a measurement into the database.
        /// Creates a unique entry for a new measurement.
        /// Creates an entry for the traditional INCC5 results table for the summary results.
        /// Detailed measurement results (e.g. cycles, param methods and results), are saved by other methods.
        /// </summary>
        /// <returns>Unique database id of the measurement</returns>
        public long Persist()
        {
            logger.TraceEvent(LogLevels.Verbose, 34001, "Preserving measurement ...");
            DB.Measurements dbm = new DB.Measurements();
            long mid = dbm.Add(name: Detector.Id.DetectorName,
                                date: MeasDate,  // NEXT: file-based ops use the file date, but we want to replace with current time stamp 
                                mtype: MeasOption.PrintName(),
                                filename: MeasurementId.FileName,  // the file names are generated at the end of the process, in GenerateReports, subsequently the database entry is updated with the new file names
                                notes: "2016");

            logger.TraceEvent(LogLevels.Verbose, 34001, "Preserved measurement id {0}", mid);

            DB.Results dbres = new DB.Results();
            // save results with mid as foreign key
            long rid = dbres.Create(mid, INCCAnalysisResults.TradResultsRec.ToDBElementList());
            logger.TraceEvent(LogLevels.Verbose, 34045, "Preserved summary results with id {0}", rid);

			long c = dbm.CountOf(name: Detector.Id.DetectorName,
                                dt: MeasDate, 
                                type: MeasOption.PrintName());

			dbm.UpdateNote(c.ToString(),mid);
			MeasurementId.UniqueId = mid;

            return mid;
        }

        /// <summary>
        /// Preserve first generated INCC or LM file name on the measurement id, then the remaining filenames 
        /// </summary>
        public void PersistFileNames()
        {
			NC.App.DB.AddResultsFileNames(this);
        }

        public void AdjustCycleCountsBaseOnStatus(bool curCycleIncomplete)
        {
             if (curCycleIncomplete)
                 CurrentRepetition--;


            // also consider deleting incomplete cycles, or make sure the status marking causes it to be skipped in any validity counts
        }
        // devnote: detached attributes from INCC5 ride on the results_rec entry in the Results map

        public class SourceYieldFactoredRates
        {

            public double source_yield_factor;
            public VTuple corrected_doubles;
            public VTuple corrected_triples;
            public VTuple total_corr_fact;

            public SourceYieldFactoredRates(MultiplicityCountingRes results, Measurement meas)
            {
                corrected_doubles = new VTuple();
                corrected_triples = new VTuple();
                total_corr_fact = new VTuple();
                ApplySourceYieldFactor(results, meas);
            }

            protected void ApplySourceYieldFactor(MultiplicityCountingRes results, Measurement meas)
            {
                double delta_days;
                // For Cf, this source yield factor will change -- So Martyn says. Hn 7.23.2015 -- Martyn likes happyfun
                delta_days = (meas.MeasDate - meas.Norm.refDate).TotalDays;
                //Now, K0 is modified based on whether AmLi or Cf normalization is chosen.... happyfun
                if (meas.Norm.biasMode == NormTest.Cf252Doubles || meas.Norm.biasMode == NormTest.Cf252Singles)
                    source_yield_factor = Math.Pow(2.0, (delta_days / Isotopics.CF252HL));
                else
                    source_yield_factor = Math.Pow(2.0, (delta_days / Isotopics.AM241HL));
                corrected_doubles.v = source_yield_factor * results.DeadtimeCorrectedDoublesRate.v;
                corrected_doubles.err = source_yield_factor * results.DeadtimeCorrectedDoublesRate.err;
                corrected_triples.v = source_yield_factor * results.DeadtimeCorrectedTriplesRate.v;
                corrected_triples.err = source_yield_factor * results.DeadtimeCorrectedTriplesRate.err;
                total_corr_fact.v = source_yield_factor * meas.Norm.currNormalizationConstant.v;
                total_corr_fact.err = source_yield_factor * meas.Norm.currNormalizationConstant.err;
            }
        }
    }
}


