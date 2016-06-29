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
using System.Reflection;
namespace AnalysisDefs
{
    using Tuple = VTuple;
    
    /* state-based INCC Results must include:
     * 1 bkg params, norm params, test params, acq params, isotopics, meas date time stamp, (meas and meas.cms)
     * computed results include:
     * 2 computed avgs and sum from calc_averages_and_sums
     * 3 final rates DTC and raw, computed by specific method (see the mcr results in the [mkey[selector]] results map)
     * 4 stratum uncertainty, updated isotopics (don't have these yet)
     * 5 results for each method, and sr/mtl type key
     * 6 Assay-type results (Bias/Normalization, Precision, Initial Source, &c;)
    */

    /// <summary>
    /// Represents SR params keyed assay selection, use these to map to the results 
    /// </summary>
    public class MeasOptionSelector : Tuple<AssaySelector.MeasurementOption, Multiplicity>
    {
        public MeasOptionSelector(AssaySelector.MeasurementOption opt, Multiplicity srkey)
            : base(opt, srkey) // might need a deep copy later
        {
        }

        public MeasOptionSelector()
            : base(AssaySelector.MeasurementOption.rates, new Multiplicity(FAType.FAOn))
        {
        }

        public MeasOptionSelector(MeasOptionSelector src)
            : base(src.Item1, new Multiplicity(src.Item2))
        {
        }


        int Compare(MeasOptionSelector x, MeasOptionSelector y) // depends upon the Multiplicity equality implementation 
        {
            return ((new System.Collections.CaseInsensitiveComparer()).Compare(x.Item1, y.Item1));
        }

        public int CompareTo(object other)
        {
            return Compare(this, (MeasOptionSelector)other);
        }
        public override bool Equals(object obj)
        {
            return (CompareTo(obj) == 0);
        }
       public override int GetHashCode()
        {
            int hCode = Item1.GetHashCode() ^ Item2.GetHashCode();
            return hCode.GetHashCode();
        }
        public override string ToString()
        {
            return Item1 + " => " + Item2;
        }

        public ShiftRegisterParameters SRParams 
        {
            get { return Item2.SR; }
        }

        public Multiplicity MultiplicityParams
        {
            get { return this.Item2; }
        }
    }

    /// <summary>
    /// Maintains a pair of INCC results for each multi analyzer.
    /// There is a summary result as cloned from INCC5, and 
    ///  the specific method results used to compute mass (KA, Mult, &c.)
    /// Consists of a single INCC5 result plus a map of mkey analyzer identifiers to final INCC results for that analyzer mkey
    /// </summary>
    public class INCCResults : Dictionary<MeasOptionSelector, INCCResult>  // the overall results for the mkey mcr counters
    {

        Dictionary<SpecificCountingAnalyzerParams, INCCMethodResults> methodsresults;  // the specialized INCC method analysis results
        results_rec incc5tradresults;
        public INCCResults()
        {
            methodsresults = new Dictionary<SpecificCountingAnalyzerParams, INCCMethodResults>(new SpecificCountingAnalyzerParamsEqualityComparer());
        }
        
        public Dictionary<SpecificCountingAnalyzerParams, INCCMethodResults> MethodsResults
        {
            get { return methodsresults; }
            set { methodsresults = value; }
        }
        public results_rec TradResultsRec
        {
            get { return incc5tradresults; }
            set { incc5tradresults = value; }
        }

        public bool TryGetMethodResults(SpecificCountingAnalyzerParams mkey, INCCSelector selector, AnalysisMethod method, out INCCMethodResult value)
        {
            INCCMethodResults res;
            // get the results map for this mult analyzer
            bool good = methodsresults.TryGetValue(mkey, out res);
            value = null;
            if (good && res != null)
            {
                // the result instance for this method
                good = res.GetMethodResults(selector, method, out value);
            }
            return good;
        }

        public bool TryGetINCCResults(SpecificCountingAnalyzerParams mkey, out INCCMethodResults value)
        {
            // get the results map for this mult analyzer
            value = null;
            bool good = methodsresults.TryGetValue(mkey, out value);
            return good;
        }

        public void AddMethodResults(SpecificCountingAnalyzerParams mkey, INCCSelector selector, AnalysisMethod method, INCCMethodResult value)  // could derive method type fror last arg, lazy boy
        {
            INCCMethodResults methods;
            //Joe thinks not needed 10-1  INCCMethodResult temp = new INCCMethodResult();
            bool good = methodsresults.TryGetValue(mkey, out methods);
            // first look for the incc results map
            if (!good || methods == null) // add new method map to results for this key
            {
                methods = new INCCMethodResults();
                methodsresults.Add(mkey, methods);
            }
                      
            methods.AddMethodResults(selector, method, value); // add the method results on the list for the selector, create new list if not there
        }

        /// <summary>
        /// Get the specific results instance for the given det, mtl and SR instrument.
        /// Optionally create a placeholder instance if not found
        /// </summary>
        /// <param name="mkey">Mult and SR Params</param>
        /// <param name="sel">det,mtl pair</param>
        /// <param name="am">analysis method sought</param>
        /// <param name="create">create if not found</param>
        /// <returns>The existing (or newly created) result instance</returns>
        public INCCMethodResult LookupMethodResults(Multiplicity mkey, INCCSelector sel, AnalysisMethod am, bool create)
        {
            INCCMethodResult m = null;
            bool got = TryGetMethodResults(mkey, sel, am, out m);
            if (!got && create)
            {
                ConstructorInfo ci = GetMethodType(am).GetConstructor(Type.EmptyTypes);
                m = (INCCMethodResult)ci.Invoke(null);
                AddMethodResults(mkey, new INCCSelector(sel), am, m);
            }
            return m;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="am"></param>
        /// <returns></returns>
        private System.Type GetMethodType(AnalysisMethod am)
        {
            System.Type res = null;
            switch (am)
            {
                case AnalysisMethod.CalibrationCurve:
                    res = typeof(INCCMethodResults.results_cal_curve_rec);
                    break;
                case AnalysisMethod.KnownA:
                    res = typeof(INCCMethodResults.results_known_alpha_rec);
                    break;
                case AnalysisMethod.Multiplicity:
                    res = typeof(INCCMethodResults.results_multiplicity_rec);
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    res = typeof(INCCMethodResults.results_truncated_mult_rec);
                    break;
                case AnalysisMethod.KnownM:
                    res = typeof(INCCMethodResults.results_known_m_rec);
                    break;
                case AnalysisMethod.AddASource:
                    res = typeof(INCCMethodResults.results_add_a_source_rec);
                    break;
                case AnalysisMethod.CuriumRatio:
                    res = typeof(INCCMethodResults.results_curium_ratio_rec);
                    break;
                case AnalysisMethod.Collar:
                    res = typeof(INCCMethodResults.results_collar_rec);
                    break;
                case AnalysisMethod.Active:
                    res = typeof(INCCMethodResults.results_active_rec);
                    break;
                case AnalysisMethod.ActiveMultiplicity:
                    res = typeof(INCCMethodResults.results_active_mult_rec);
                    break;
                case AnalysisMethod.ActivePassive:
                    res = typeof(INCCMethodResults.results_active_passive_rec);
                    break;
                default:
                    res = typeof(INCCMethodResult);
                    break;
            }
            return res;
        }

        public System.Collections.IEnumerator GetResultsEnumerator()
        {
            foreach (KeyValuePair<MeasOptionSelector, INCCResult> pair in this)
            {
                yield return pair.Value;
            }
        }

        public System.Collections.IEnumerator GetMeasSelectorResultsEnumerator()
        {
            foreach (KeyValuePair<MeasOptionSelector, INCCResult> pair in this)
            {
                yield return pair.Key;
            }
        }

        public class results_init_src_rec : INCCResult
        {
            public string init_src_id;
            public bool pass;
            public NormTest mode; 

            public results_init_src_rec()
            {
                init_src_id = string.Empty;
            }

            // Uses 3 external datums: the meas date, and the corrected rates and corrected rates selector appear on the parent class Mult Counting results
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Initial source results:");  // section header
                sec.AddTwo("Source id:", init_src_id);
                if (mode == NormTest.AmLiSingles || mode == NormTest.Collar)
                {
                 sec.AddNumericRow("Reference singles rate:", DeadtimeCorrectedSinglesRate.v);
                 sec.AddDateTimeRow("Reference singles date:", m.MeasDate);
                }
                else if (mode == NormTest.Cf252Singles)
                {
                    sec.AddNumericRow("Reference singles rate:", DeadtimeCorrectedSinglesRate);
                    sec.AddDateTimeRow("Reference singles date:", m.MeasDate);

                }
                else if (mode == NormTest.Cf252Doubles)
                {
                    //doubles are doubles and singles are singles hn 9.24.2015 -- nonhappyfun
                    sec.AddNumericRow("Reference doubles rate:", DeadtimeCorrectedDoublesRate);
                    sec.AddDateTimeRow("Reference doubles date:", m.MeasDate);
                }
               sec.AddTwo("Initial source measurement:", (pass ? "passed" : "failed") + ".");
               return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_init_src_rec";

                ps.Add(new DBParamEntry("init_src_id", init_src_id));
                ps.Add(new DBParamEntry("mode", (Int32)mode));
                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        public class results_bias_rec : INCCResult
        {
            public string sourceId;
            public bool pass;
            public NormTest mode;
            public Tuple biasSnglsRateExpect;
            public Tuple biasSnglsRateExpectMeas;
            public Tuple biasDblsRateExpect;
            public Tuple biasDblsRateExpectMeas;
            public Tuple newNormConstant;
            public double measPrecision;
            public double requiredPrecision;
            public double requiredMeasSeconds;

            public results_bias_rec()
            {
                sourceId = string.Empty;
                biasSnglsRateExpect = new Tuple();
                biasSnglsRateExpectMeas = new Tuple();
                biasDblsRateExpect = new Tuple();
                biasDblsRateExpectMeas = new Tuple();
                newNormConstant = new Tuple();
            }

            // Uses 3 external datums: Meas.Norm.currNormalizationConstant, and the corrected rates and corrected rates selector appear on the parent class Mult Counting results
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                string h = string.Format("Normalization results for reference source: {0}", sourceId);
                sec.AddHeader(h);  // section header
                sec.AddNumericRow("Current normalization constant", m.Norm.currNormalizationConstant);

                if (mode == NormTest.AmLiSingles || mode == NormTest.Collar)
                {
                    sec.AddNumericRow("AmLi expected singles rate:", biasSnglsRateExpect);
                    sec.AddNumericRow("AmLi measured singles rate:", DeadtimeCorrectedSinglesRate);
                    sec.AddNumericRow("Singles rate expected/measured:", biasSnglsRateExpectMeas);
                }
                else if (mode == NormTest.Cf252Singles)
                {
                    sec.AddNumericRow("Cf252 expected singles rate:", biasDblsRateExpect);  // dev note: apparently carried in the doubles field due to INCC hacking, but check
                    sec.AddNumericRow("Cf252 measured singles rate:", DeadtimeCorrectedSinglesRate); 
                    sec.AddNumericRow("Singles rate expected/measured:", biasDblsRateExpectMeas); 

                }
                else if (mode == NormTest.Cf252Doubles)
                {
                    sec.AddNumericRow("Cf252 expected doubles rate:", biasDblsRateExpect);
                    sec.AddNumericRow("Cf252 measured doubles rate:", DeadtimeCorrectedDoublesRate);
                    sec.AddNumericRow("Doubles rate expected/measured:", biasDblsRateExpectMeas);
                }
                sec.AddNumericRow("New normalization constant:", newNormConstant);
                sec.AddTwo("Normalization test  -- ", (pass ? "data quality is good" : "data quality is inadequate") + ".");

                if (mode == NormTest.Cf252Doubles && !pass)
                {
                     sec.AddNumericRow("Measured percent precision:", measPrecision);
                     sec.AddNumericRow("Required percent precision:", requiredPrecision);
                     sec.AddNumericRow("Repeat measurement for at least:", requiredMeasSeconds);
                }
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_bias_rec";

                ps.Add(new DBParamEntry("source_id", this.sourceId));
                ps.Add(new DBParamEntry("mode", (Int32)mode));
                ps.Add(new DBParamEntry("pass", pass));
                ps.AddRange(DBParamList.TuplePair("sngls_rate_expect", biasSnglsRateExpect));
                ps.AddRange(DBParamList.TuplePair("sngls_rate_expect_meas", biasSnglsRateExpectMeas));
                ps.AddRange(DBParamList.TuplePair("dbls_rate_expect", biasDblsRateExpect));
                ps.AddRange(DBParamList.TuplePair("dbls_rate_expect_meas", biasDblsRateExpectMeas));
                ps.AddRange(DBParamList.TuplePair("new_norm_constant", newNormConstant));
                ps.Add(new DBParamEntry("meas_precision", measPrecision));
                ps.Add(new DBParamEntry("required_precision", requiredPrecision));
                ps.Add(new DBParamEntry("required_meas_seconds", requiredMeasSeconds));
            }
        }

        public class results_precision_rec : INCCResult
        {
            public bool pass;
            public double precSampleVar;
            public double precTheoreticalVar;
            public double precChiSq;
            public double chiSqLowerLimit;
            public double chiSqUpperLimit;

            public results_precision_rec()
            {
            }
            
            // needs 1 datum from the current measurement: number_good_runs
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Precision results");  // section header
                sec.AddIntegerRow("Number of cycles:", (int)m.Cycles.GetValidCycleCount()); // dev note: might have incorrect number  . . . number_good_runs
                sec.AddNumericRow("Chi-square lower limit:", chiSqLowerLimit);
                sec.AddNumericRow("Chi-square upper limit:", chiSqUpperLimit);
                sec.AddNumericRow("Sample variance:", precSampleVar);
                sec.AddNumericRow("Theoretical variance:", precTheoreticalVar);
                sec.AddNumericRow("Chi-square:", precChiSq);
                sec.AddTwo("Precision test ",(pass ? "passed" : "failed") + ".");

                return sec;
            }
            
            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_precision_rec";
                ps.Add(new DBParamEntry("sample_var", precSampleVar));
                ps.Add(new DBParamEntry("theoretical_var", precTheoreticalVar));
                ps.Add(new DBParamEntry("chi_sq", precChiSq));
                ps.Add(new DBParamEntry("chi_sq_lower_limit", chiSqLowerLimit));
                ps.Add(new DBParamEntry("chi_sq_upper_limit", chiSqUpperLimit));
                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        /// <summary>
        /// INCC5 style aggregate of generalized results
        /// </summary>
        public class results_rec : INCCResult
        {
            // acq params, det, isotopics, sr amd mcr vals, bkg, norm, test params, decl mass, prim a, drum, outlier max, act/pas ids, a few more
            public AcquireParameters acq;
            public Detector det;
            public Isotopics iso;
            public TestParameters tests;
            public NormParameters norm;
            public BackgroundParameters bkg;
            public CycleList cycles;
            public Stratum st;
            public MultiplicityCountingRes mcr;
            public holdup_config_rec hc;
            public ItemId item;

            public AnalysisMessages messages;

            // separate from data structures and tables, exclusive to this class
            public AnalysisMethod primary;
            public int total_number_runs;
            public double total_good_count_time, net_drum_weight, db_version;
            public AssaySelector.MeasurementOption meas_option;
            public bool completed;
			public long MeasId {get; set; }

            public results_rec()
            {
            }

            public results_rec(results_rec src)
            {
                acq = new AcquireParameters(src.acq);
                st = new Stratum(src.st);
                messages = new AnalysisMessages(src.messages);
                det = new Detector(src.det);
                iso = new Isotopics();
                if (src.iso != null) src.iso.CopyTo(iso);
                tests = new TestParameters(src.tests);
                norm = new NormParameters(src.norm); 
                bkg = new BackgroundParameters(src.bkg); 
                cycles = src.cycles;  // next: not a copy, so do not change list until this is saved!
   
                hc = new holdup_config_rec(hc);  // left unfinished in real code, only available on transfer op
                item = new ItemId(src.item);

                meas_option = src.meas_option;

                mcr = new MultiplicityCountingRes(src.mcr);

                //TODO INCCMethodResults imr;
                //if (m.INCCAnalysisResults.TryGetINCCResults(det.MultiplicityParams, out imr))
                //    primary = imr.primaryMethod;
            }

            // this should be created at some correct moment, after analyzer and method map construction, AND THEN inserted into the meas.Result map
            public results_rec(Measurement m)
            {
                acq = new AcquireParameters(m.AcquireState);
                st = new Stratum(m.Stratum);
                messages = new AnalysisMessages(m.Messages);
                det = new Detector(m.Detector);
                iso = new Isotopics();
                if (m.Isotopics != null) m.Isotopics.CopyTo(iso);
                tests = new TestParameters(m.Tests);
                norm = new NormParameters(); m.Norm.CopyTo(norm);
                bkg = new BackgroundParameters(); m.Background.CopyTo(bkg);
                cycles = m.Cycles;  // dev note: not a copy, so do not change list until this is saved!

                // get the first results from the results map 
                if (m.CountingAnalysisResults.Count > 0 && m.CountingAnalysisResults.HasMultiplicity)
                    try
                    {
                        mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[det.MultiplicityParams];
                    }
                    catch (Exception)
                    {
                        // NEXT: code 'gets here' if the detector settings do not match any of the explicitly LM-defined Multiplicity analyzer(s).
                        // so for now we'll override det.MultiplicityParams with the first available LM-defined Multiplicity upstream, so the code does not 'get here'
                        if (mcr == null)
                            mcr = m.CountingAnalysisResults.GetFirstMultiplicity;
                    }
                if (mcr == null)
                    mcr = new MultiplicityCountingRes();  // inadequate attempt tries to account for LM-only condition, where no mcr, or no matching mcr, exists

                hc = new holdup_config_rec();  // left unfinished in real code, only available on transfer op
                item = new ItemId(m.MeasurementId.Item);
                // pu_date wiped out in item when was in measurement.
                item.pu_date = m.Isotopics.pu_date;
                item.am_date = m.Isotopics.am_date;
                meas_option = m.MeasOption;

                INCCMethodResults imr;
                if (m.INCCAnalysisResults.TryGetINCCResults(det.MultiplicityParams, out imr)) // devnote: see notes above about issues with multiple MultiplicityParams that do not match the detector's default values 
                    primary = imr.primaryMethod;
            }


            /// <summary>
            /// Match the results rec with the measurement id's type and timestamp.
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            public bool MeasurementIdMatch(MeasId m)
            {
                bool b = false;
                b = (m.MeasOption == meas_option) && m.MeasDateTime.Equals(acq.MeasDateTime);
                return b;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("????? results");  // section header
                sec.AddIntegerRow("Number of cycles:", (int)m.Cycles.GetValidCycleCount()); // dev note: might have incorrect number  . . . number_good_runs;

                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_rec";
                det.Id.GenParamList(); ps.AddRange(det.Id.ps);
                ps.AddRange(acq.SubsetForResults());
                ps.AddRange(st.SubsetForResults());
                iso.GenParamList(); ps.AddRange(iso.ps);
                tests.GenParamList(); ps.AddRange(tests.ps);
                norm.GenParamList(); ps.Add(norm.Find("normalization_constant")); ps.Add(norm.Find("normalization_constant_err"));
                bkg.GenParamList(); ps.AddRange(bkg.ps);
                det.SRParams.GenParamList(); ps.AddRange(det.SRParams.ps);
                item.GenParamList();

                // impedance mismatch with item table and results table
                DBParamEntry x = item.Find("length_entry");
                DBParamEntry cbpe = new DBParamEntry("length", x);
                x = item.Find("declared_u_mass_entry");
                DBParamEntry dbpe = new DBParamEntry("declared_u_mass",x);
                ps.Add(cbpe); ps.Add(dbpe);

                mcr.GenParamList(); ps.AddRange(mcr.ps);
                ps.AddRange(mcr.MoreForResults());
                hc.GenParamList(); ps.AddRange(hc.ps);

                // dev note: messages are now preserved in the analysis_messages table

                ps.Add(new DBParamEntry("completed", completed));
                ps.Add(new DBParamEntry("db_version", db_version));
                ps.Add(new DBParamEntry("net_drum_weight", net_drum_weight));

                ps.Add(new DBParamEntry("total_number_runs", total_number_runs));
                ps.Add(new DBParamEntry("total_good_count_time", total_good_count_time));

                ps.Add(new DBParamEntry("meas_option", meas_option.ToString()));
                ps.Add(new DBParamEntry("primary_analysis_method", primary.ToString()));
            }
        }

    }

    // interface for two types of report line listings (INCC and CSV)
    // implement for report generation for each parameter class definition 
    public interface IINCCStringRep
    {
        // the old INCC report output style label -> value(s), convert to lines by ToString() on each row entry
        List<NCCReporter.Row> ToLines(Measurement m);

        // suitable for spreadsheet use, convert to lines by ToString() on each row entry
        List<NCCReporter.Row> ToColumns(Measurement m);
    }

    public class INCCResult : MultiplicityCountingRes  // todo: revisit this as a parent class, do we need it or something slimmer and more specific 
        , IINCCStringRep
    {

        // the old INCC report output style label -> value(s), convert to lines by ToString() on each row entry
        public virtual List<NCCReporter.Row> ToLines(Measurement m) { return new List<NCCReporter.Row>(); }

        // suitable for spreadsheet use, convert to lines by ToString() on each row entry
        public virtual List<NCCReporter.Row> ToColumns(Measurement m) { return new List<NCCReporter.Row>(); }

        public override void GenParamList()  // force implementation in subclasses
        {
			ps = new List<DBParamEntry>();
        }
   
    }

    public class INCCMethodResult : ParameterBase, IINCCStringRep 
    {
        // the old INCC report output style label -> value(s), convert to lines by ToString() on each row entry
        public virtual List<NCCReporter.Row> ToLines(Measurement m) { return new List<NCCReporter.Row>(); }

        // suitable for spreadsheet use, convert to lines by ToString() on each row entry
        public virtual List<NCCReporter.Row> ToColumns(Measurement m) { return new List<NCCReporter.Row>(); }

        // implement in subclass
        public virtual void CopyTo(INCCMethodResult imd) { }

        // the method params used to calculate these results: retain results and input values used to calculate 
        public INCCAnalysisParams.INCCMethodDescriptor methodParams, methodParams2;
      
    }

    public class INCCMethodResults : Dictionary<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>
    {
        public AnalysisMethod primaryMethod; // dev note: this might need to be indexed under the selector too

        public INCCMethodResults()
            : base(new INCCSelector.SelectorEquality())
        {
            primaryMethod = AnalysisMethod.INCCNone;
        }

        public void AddMethodResults(INCCSelector selector, AnalysisMethod method, INCCMethodResult value)
        {
            Dictionary<AnalysisMethod, INCCMethodResult> d;
            bool good = TryGetValue(selector, out d);
            if (!good || d == null) // add new list to results for this selector
            {
                d = new Dictionary<AnalysisMethod, INCCMethodResult>();
                this.Add(selector, d);
            }
            d.Add(method, value);
        }

        public bool GetMethodResults(INCCSelector selector, AnalysisMethod method, out INCCMethodResult res)
        {
            Dictionary<AnalysisMethod, INCCMethodResult> d;
            bool good = TryGetValue(selector, out d);
            res = null;
            if (good && d != null)
            {
                good = d.TryGetValue(method, out res);
            }
            return good;
        }


        public static double calc_percent_pu240e(double pu240e, double total_pu)
        {

            double percent_pu240e;

            if (total_pu != 0)
                percent_pu240e = Math.Abs(pu240e / total_pu) * 100;
            else
                percent_pu240e = 0;

            return (percent_pu240e);

        }

        public class results_known_alpha_rec : INCCMethodResult
        {
            public double mult;
            public double alphaK;
            public Tuple mult_corr_doubles;
            public Tuple pu240e_mass;
            public Tuple pu_mass;
            public double dcl_pu240e_mass;
            public double dcl_pu_mass;
            public Tuple dcl_minus_asy_pu_mass;
            public double dcl_minus_asy_pu_mass_pct;
            public bool pass;
            public double dcl_u_mass;
            public double length;
            public double heavy_metal_content;
            public double heavy_metal_correction;
            public Tuple corr_singles;
            public Tuple corr_doubles;
            public double corr_factor;
            public double dry_alpha_or_mult_dbls;
            public double lower_corr_factor_limit;
            public double upper_corr_factor_limit;
            public new INCCAnalysisParams.known_alpha_rec methodParams // the associated params, but could leave these back in the DB for lookup when needed
            {
                get { return (INCCAnalysisParams.known_alpha_rec)base.methodParams; }
                set { base.methodParams = value; }
            }

            public results_known_alpha_rec()
            {
                mult = 1.0;
                mult_corr_doubles = Tuple.Create();
                pu240e_mass = Tuple.Create();
                pu_mass = Tuple.Create();
                dcl_minus_asy_pu_mass = Tuple.Create();
                corr_singles = Tuple.Create();
                corr_doubles = Tuple.Create();
                methodParams = new INCCAnalysisParams.known_alpha_rec();
            }

            public results_known_alpha_rec(results_known_alpha_rec src)
            {
                src.methodParams.CopyTo(methodParams);
                mult = src.mult;
                alphaK = src.alphaK;
                mult_corr_doubles = new Tuple(src.mult_corr_doubles);
                pu240e_mass = new Tuple(src.pu240e_mass);
                pu_mass = new Tuple(src.pu_mass);
                dcl_pu240e_mass = src.dcl_pu240e_mass;
                dcl_pu_mass = src.dcl_pu_mass;
                dcl_minus_asy_pu_mass = new Tuple(src.dcl_minus_asy_pu_mass);
                dcl_minus_asy_pu_mass_pct = src.dcl_minus_asy_pu_mass_pct;
                pass = src.pass;
                dcl_u_mass = src.dcl_u_mass;
                length = src.length;
                heavy_metal_content = src.heavy_metal_content;
                heavy_metal_correction = src.heavy_metal_correction;
                corr_singles = new Tuple(src.corr_singles);
                corr_doubles = new Tuple(src.corr_doubles);
                corr_factor = src.corr_factor;
                dry_alpha_or_mult_dbls = src.dry_alpha_or_mult_dbls;
                lower_corr_factor_limit = src.lower_corr_factor_limit;
                upper_corr_factor_limit = src.upper_corr_factor_limit;
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_known_alpha_rec tgt = (results_known_alpha_rec)imr;
                methodParams.CopyTo(tgt.methodParams);
                tgt.mult = mult;
                tgt.alphaK = alphaK;
                tgt.mult_corr_doubles = new Tuple(mult_corr_doubles);
                tgt.pu240e_mass= new Tuple(pu240e_mass);
                tgt.pu_mass= new Tuple(pu_mass);
                tgt.dcl_pu240e_mass = dcl_pu240e_mass;
                tgt.dcl_pu_mass = dcl_pu_mass;
                tgt.dcl_minus_asy_pu_mass= new Tuple(dcl_minus_asy_pu_mass);
                tgt.dcl_minus_asy_pu_mass_pct = dcl_minus_asy_pu_mass_pct;
                tgt.pass = pass;
                tgt.dcl_u_mass = dcl_u_mass;
                tgt.length = length;
                tgt.heavy_metal_content = heavy_metal_content;
                tgt.heavy_metal_correction = heavy_metal_correction;
                tgt.corr_singles = new Tuple(corr_singles);
                tgt.corr_doubles = new Tuple(corr_doubles); 
                tgt.corr_factor = corr_factor;
                tgt.dry_alpha_or_mult_dbls = dry_alpha_or_mult_dbls;
                tgt.lower_corr_factor_limit = lower_corr_factor_limit;
                tgt.upper_corr_factor_limit = upper_corr_factor_limit;

                imr.modified = true;
            }


            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader(" Known alpha results");  // section header
                if (methodParams.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection)
                {
                    /* declared U mass */
                    sec.AddNumericRow("Declared U mass (g):", dcl_u_mass);
                    /* length */
                    sec.AddNumericRow("Length (cm):", length);
                    sec.AddNumericRow("Heavy metal content (g/cm):", heavy_metal_content);
                    sec.AddNumericRow("Heavy metal correction:", heavy_metal_correction);
                    sec.AddNumericRow("Heavy metal corrected singles:", corr_singles);
                    sec.AddNumericRow("Heavy metal corrected doubles:", corr_doubles);
                }
                if (methodParams.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha)
                {
                    sec.AddNumericRow("Ring ratio:", corr_singles);
                    sec.AddNumericRow("Dry alpha:", dry_alpha_or_mult_dbls);
                    sec.AddNumericRow("Moisture correction factor:", corr_factor);
                }
                if (methodParams.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles)
                {
                    sec.AddNumericRow("Ring ratio:", corr_singles);
                    sec.AddNumericRow("Multiplication corrected doubles:", mult_corr_doubles);
                    sec.AddNumericRow("Moisture correction factor:", corr_factor);
                }
                /* alpha */
                if (methodParams.known_alpha_type != INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha)
                {
                    sec.AddNumericRow("Alpha:", alphaK);
                }
                else
                {
                    sec.AddNumericRow("Wet alpha", alphaK);
                }
                /* multiplication */
                sec.AddNumericRow("Multiplication:", mult);

                /* multiplication corrected doubles and error */
                if (methodParams.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles)
                {
                    Tuple temp = new Tuple(dry_alpha_or_mult_dbls, mult_corr_doubles.err);
                    sec.AddNumericRow("Moisture and mult. corr. doubles:", temp);
                }
                else
                {
                    sec.AddNumericRow("Multiplication corrected doubles:", mult_corr_doubles);
                }

                /* pu240e mass and error */
                sec.AddNumericRow("Pu240e mass (g):", pu240e_mass);
                sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(pu240e_mass.v, pu_mass.v));
                sec.AddNumericRow("Pu mass (g):", pu_mass);
                if (dcl_pu_mass > 0.0)
                {
                    /* declared pu240e */
                    sec.AddNumericRow("Declared Pu240e mass (g):", dcl_pu240e_mass);
                    sec.AddNumericRow("Declared Pu mass (g):", dcl_pu_mass);
                    sec.AddNumericRow("Declared - assay Pu mass (g):", dcl_minus_asy_pu_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_pu_mass_pct, pu_mass.err / dcl_pu_mass * 100.0);
                    sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                }
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_known_alpha_rec";
                ps.Add(new DBParamEntry("mult", mult));
                ps.Add(new DBParamEntry("alpha", alphaK));
                ps.AddRange(DBParamList.TuplePair(mult_corr_doubles, "mult_corr_doubles"));

                ps.AddRange(DBParamList.TuplePair(pu240e_mass, "pu240e_mass"));
                ps.AddRange(DBParamList.TuplePair(pu_mass, "pu_mass"));
                ps.Add(new DBParamEntry("dcl_pu240e_mass", dcl_pu240e_mass));
                ps.Add(new DBParamEntry("dcl_pu_mass", dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_pu_mass, "dcl_minus_asy_pu_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", dcl_minus_asy_pu_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));

                ps.Add(new DBParamEntry("dcl_u_mass", dcl_u_mass));
                ps.Add(new DBParamEntry("length", length));
                ps.Add(new DBParamEntry("heavy_metal_content", heavy_metal_content));
                ps.Add(new DBParamEntry("heavy_metal_correction", heavy_metal_correction));
                ps.AddRange(DBParamList.TuplePair(corr_singles, "corr_singles"));
                ps.AddRange(DBParamList.TuplePair(corr_doubles, "corr_doubles"));
                ps.Add(new DBParamEntry("corr_factor", corr_factor));
                ps.Add(new DBParamEntry("dry_alpha_or_mult_dbls", dry_alpha_or_mult_dbls));
            }
               
        }

        public class results_multiplicity_rec : INCCMethodResult
        {
            public Tuple mult;
            public Tuple alphaK;
            public Tuple corr_factor;
            public Tuple efficiencyComputed;
            public Tuple pu240e_mass;
            public Tuple pu_mass;
            public double dcl_pu240e_mass;
            public double dcl_pu_mass;
            public Tuple dcl_minus_asy_pu_mass;
            public double dcl_minus_asy_pu_mass_pct;

            public bool pass;
            public INCCAnalysisParams.MultChoice solve_efficiency_choice;

            public new INCCAnalysisParams.multiplicity_rec methodParams
            {
                get { return (INCCAnalysisParams.multiplicity_rec)base.methodParams; }
                set { base.methodParams = value; }
            }

            public results_multiplicity_rec()
            {
                mult = Tuple.Create();
                alphaK = Tuple.Create();
                corr_factor = Tuple.Create();
                efficiencyComputed = Tuple.Create();
                pu240e_mass = Tuple.Create();
                pu_mass = Tuple.Create();
                dcl_minus_asy_pu_mass = new Tuple();
                methodParams = new INCCAnalysisParams.multiplicity_rec();
            }

            public results_multiplicity_rec(results_multiplicity_rec src)
            {
                if (methodParams == null)
                    methodParams = new INCCAnalysisParams.multiplicity_rec();
                src.methodParams.CopyTo(methodParams);
                mult = new Tuple(src.mult);
                alphaK = new Tuple(src.alphaK);
                corr_factor = new Tuple(src.corr_factor);
                efficiencyComputed = new Tuple(src.efficiencyComputed);
                pu240e_mass = new Tuple(src.pu240e_mass);
                pu_mass = new Tuple(src.pu_mass);
                dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                dcl_pu240e_mass = src.dcl_pu240e_mass;
                dcl_pu_mass = src.dcl_pu_mass;
                dcl_minus_asy_pu_mass_pct = src.dcl_minus_asy_pu_mass_pct;
                pass=src.pass;
                solve_efficiency_choice = src.solve_efficiency_choice;
            }


            public override void CopyTo(INCCMethodResult imr)
            {
                results_multiplicity_rec tgt = (results_multiplicity_rec)imr;
                methodParams.CopyTo(tgt.methodParams);
                tgt.mult = new Tuple(mult); 
                tgt.alphaK   = new Tuple(alphaK);
                tgt.corr_factor = new Tuple(corr_factor);
                tgt.efficiencyComputed = new Tuple(efficiencyComputed);
                tgt.pu240e_mass = new Tuple(pu240e_mass);
                tgt.pu_mass = new Tuple(pu_mass); 
                
                tgt.dcl_pu240e_mass = dcl_pu240e_mass;
                tgt.dcl_pu_mass = dcl_pu_mass;
                tgt.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                tgt.dcl_minus_asy_pu_mass_pct = dcl_minus_asy_pu_mass_pct;
                tgt.pass = pass;

                tgt.solve_efficiency_choice = solve_efficiency_choice;

                imr.modified = true;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                if (solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA)
                    sec.AddHeader(" Passive multiplicity results using known alpha");
                else if (solve_efficiency_choice == INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED)
                    sec.AddHeader(" Passive multiplicity results using weighted coefficients");
                else
                    sec.AddHeader(" Passive multiplicity results");  // section header

                sec.AddNumericRow("Multiplication:", mult);
                if (solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA)
                    sec.AddNumericRow("Alpha:", alphaK.v);
                else
                    sec.AddNumericRow("Alpha:", alphaK);

                if (solve_efficiency_choice == INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT ||
                    solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL ||
                    solve_efficiency_choice == INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED)
                    sec.AddNumericRow("Multiplication correction factor:", corr_factor.v);
                else if ((solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_SOLVE_EFFICIENCY) ||
                    (solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA))
                    sec.AddNumericRow("Efficiency:", efficiencyComputed);
                if (solve_efficiency_choice == INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL)
                {
                    /* dual energy correction factor */
                    //sec.AddNumericRow("Energy correction factor:", de_energy_corr_factor);

                    /* (alpha, n) ring ratio */
                    //sec.AddNumericRow("(alpha, n) ring ratio:", de_meas_ring_ratio);

                    /* (alpha, n) energy */
                    //sec.AddNumericRow("(alpha, n) energy:",de_interpolated_neutron_energy);        
                }
                /* pu240e mass and error */
				 sec.AddNumericRow("Pu240e mass (g):", pu240e_mass);
				/* % pu240e */
				 sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e (pu240e_mass.v, pu_mass.v));

				/* pu mass and error */
				 sec.AddNumericRow("Pu mass (g):", pu_mass);

				if (dcl_pu_mass > 0.0)
				{
					/* declared pu240e */
                    sec.AddNumericRow("Declared Pu240e mass (g):", dcl_pu240e_mass);
					/* declared Pu mass */
                    sec.AddNumericRow("Declared Pu mass (g):",  dcl_pu_mass);
					/* declared minus assay Pu mass and error */
                    sec.AddNumericRow("Declared - assay Pu mass (g):",  dcl_minus_asy_pu_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_pu_mass_pct,(pu_mass.err/dcl_pu_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay Pu mass (%):",temp);
				}
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_multiplicity_rec";

                ps.AddRange(DBParamList.TuplePair(mult, "mult"));
                ps.AddRange(DBParamList.TuplePair(alphaK, "alpha"));
                ps.AddRange(DBParamList.TuplePair(corr_factor, "corr_factor"));
                ps.AddRange(DBParamList.TuplePair(efficiencyComputed, "efficiency"));                 
                
                ps.AddRange(DBParamList.TuplePair(pu240e_mass, "pu240e_mass"));
                ps.AddRange(DBParamList.TuplePair(pu_mass, "pu_mass"));
                ps.Add(new DBParamEntry("dcl_pu240e_mass", dcl_pu240e_mass));
                ps.Add(new DBParamEntry("dcl_pu_mass", dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_pu_mass, "dcl_minus_asy_pu_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", dcl_minus_asy_pu_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        public class results_cal_curve_rec : INCCMethodResult
        {
            public Tuple pu240e_mass;
            public Tuple pu_mass;
            public double dcl_pu240e_mass;
            public double dcl_pu_mass;
            public Tuple dcl_minus_asy_pu_mass;
            public double dcl_minus_asy_pu_mass_pct;
            public bool pass;
            public double dcl_u_mass;
            public double length;
            public double heavy_metal_content;
            public double heavy_metal_correction;            
            public Tuple heavy_metal_corr_singles;
            public Tuple heavy_metal_corr_doubles;

            public new INCCAnalysisParams.cal_curve_rec methodParams
            {
                 get { return (INCCAnalysisParams.cal_curve_rec)base.methodParams; }
                 set { base.methodParams = value; }
            }

            public results_cal_curve_rec()
            {
                pu240e_mass = Tuple.Create();
                pu_mass = Tuple.Create();
                dcl_minus_asy_pu_mass = Tuple.Create();
                heavy_metal_corr_doubles = Tuple.Create();
                heavy_metal_corr_singles = Tuple.Create();
                methodParams = new INCCAnalysisParams.cal_curve_rec();
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_cal_curve_rec tgt = (results_cal_curve_rec)imr;
                tgt.pu240e_mass = new Tuple(pu240e_mass);
                tgt.pu_mass = new Tuple(pu_mass);
                tgt.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                tgt.heavy_metal_corr_doubles = new Tuple(heavy_metal_corr_doubles);
                tgt.heavy_metal_corr_singles = new Tuple(heavy_metal_corr_singles);
                tgt.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader(" Passive calibration curve results");  // section header

                if (methodParams.CalCurveType == INCCAnalysisParams.CalCurveType.HM)
                {
                    /* declared U mass */
                    sec.AddNumericRow("Declared U mass (g):", dcl_u_mass);
                    /* length */
                    sec.AddNumericRow("Length (cm):",length);
                    /* heavy metal content */
                    sec.AddNumericRow("Heavy metal content (g/cm):",heavy_metal_content);
                    /* heavy metal correction */
                    sec.AddNumericRow("Heavy metal correction:",heavy_metal_correction);
                    /* corrected doubles rate and error */
                    sec.AddNumericRow("Heavy metal corrected doubles:",heavy_metal_corr_doubles);
                }
                if (methodParams.CalCurveType == INCCAnalysisParams.CalCurveType.U)
                {
                    /* u238 mass and error */
                    sec.AddNumericRow("U238 mass (g):", pu240e_mass);
                    sec.AddNumericRow("U235 (%):", methodParams.percent_u235);
                    sec.AddNumericRow("U235 mass (g):", pu_mass);

                    /* Total U mass and error */
                    Tuple temp = new Tuple(pu240e_mass.v + pu_mass.v, Math.Sqrt(Math.Pow(pu240e_mass.err, 2.0) + Math.Pow(pu_mass.err, 2.0)));
                    sec.AddNumericRow("Total U mass (g):", temp);
                    if (dcl_pu_mass > 0.0)
                    {
                        /* declared u235 mass */
                        sec.AddNumericRow("Declared U235 mass (g):", dcl_pu_mass);
                        /* declared minus assay U235 mass and error */
                        sec.AddNumericRow("Declared - assay U235 mass (g):", dcl_minus_asy_pu_mass);
                        temp = new Tuple(dcl_minus_asy_pu_mass_pct, pu_mass.err / dcl_pu_mass * 100.0);
                        sec.AddNumericRow("Declared - assay U235 mass (%):", temp);
                    }
                }
                else
                {
                    sec.AddNumericRow("Pu240e mass (g):", pu240e_mass);
                    /* % pu240e */
                    sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(pu240e_mass.v, pu_mass.v));
                    /* pu mass and error */
                    sec.AddNumericRow("Pu mass (g):", pu_mass);
                    if (dcl_pu_mass > 0.0)
                    {
                        /* declared pu240e */
                        sec.AddNumericRow("Declared Pu240e mass (g):", dcl_pu240e_mass);
                        /* declared Pu mass */
                        sec.AddNumericRow("Declared Pu mass (g):", dcl_pu_mass);
                        /* declared minus assay Pu mass and error */
                        sec.AddNumericRow("Declared - assay Pu mass (g):", dcl_minus_asy_pu_mass);
                        Tuple temp = new Tuple(dcl_minus_asy_pu_mass_pct, pu_mass.err / dcl_pu_mass * 100.0);
                        sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                    }
                }
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_cal_curve_rec";

                ps.AddRange(DBParamList.TuplePair(pu240e_mass, "pu240e_mass"));
                ps.AddRange(DBParamList.TuplePair(pu_mass, "pu_mass"));

                ps.Add(new DBParamEntry("dcl_pu240e_mass", dcl_pu240e_mass));
                ps.Add(new DBParamEntry("dcl_pu_mass", dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_pu_mass, "dcl_minus_asy_pu_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", dcl_minus_asy_pu_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));

                ps.Add(new DBParamEntry("dcl_u_mass", dcl_u_mass));
                ps.Add(new DBParamEntry("length", length));
                ps.Add(new DBParamEntry("heavy_metal_content", heavy_metal_content));
                ps.Add(new DBParamEntry("heavy_metal_correction", heavy_metal_correction));
                ps.AddRange(DBParamList.TuplePair(heavy_metal_corr_singles, "heavy_metal_corr_singles"));
                ps.AddRange(DBParamList.TuplePair(heavy_metal_corr_doubles, "heavy_metal_corr_doubles"));
            }
        }

        public class results_known_m_rec : INCCMethodResult
        {
            public Tuple pu240e_mass;
            public Tuple pu_mass;
            public double mult;
            public double alpha;
            public double pu239e_mass;
            public double dcl_pu240e_mass;
            public double dcl_pu_mass;
            public Tuple dcl_minus_asy_pu_mass;
            public double dcl_minus_asy_pu_mass_pct;
            public bool pass;
            public new INCCAnalysisParams.known_m_rec methodParams
            {
                get { return (INCCAnalysisParams.known_m_rec)base.methodParams; }
                set { base.methodParams = value; }
            }

            public results_known_m_rec()
            {
                mult = 1.0;
                pu240e_mass = Tuple.Create();
                pu_mass = Tuple.Create();
                dcl_minus_asy_pu_mass = Tuple.Create();
                methodParams = new INCCAnalysisParams.known_m_rec();
            }

            // todo: copy constructor here

            public override void CopyTo(INCCMethodResult imr)
            {
                results_known_m_rec tgt = (results_known_m_rec)imr;
                methodParams.CopyTo(tgt.methodParams);
                tgt.pu240e_mass = new Tuple(pu240e_mass);
                tgt.pu_mass = new Tuple(pu_mass); 
                
                tgt.mult = mult;
                tgt.alpha = alpha;
                tgt.pu239e_mass = pu239e_mass; 
                
                tgt.dcl_pu240e_mass = dcl_pu240e_mass;
                tgt.dcl_pu_mass = dcl_pu_mass;
                tgt.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                tgt.dcl_minus_asy_pu_mass_pct = dcl_minus_asy_pu_mass_pct;
                tgt.pass = pass;

                imr.modified = true;
            }


            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader(" Known M results");  // section header

                sec.AddNumericRow("Multiplication:", mult);
                sec.AddNumericRow("Alpha:", alpha);
                sec.AddNumericRow("Pu239 mass (g):", pu239e_mass);
                sec.AddNumericRow("Pu240e mass (g):", pu240e_mass);
                /* % pu240e */
                sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(pu240e_mass.v, pu_mass.v));
                /* pu mass and error */
                sec.AddNumericRow("Pu mass (g):", pu_mass);
                if (dcl_pu_mass > 0.0)
                {
                    /* declared pu240e */
                    sec.AddNumericRow("Declared Pu240e mass (g):", dcl_pu240e_mass);
                    /* declared Pu mass */
                    sec.AddNumericRow("Declared Pu mass (g):", dcl_pu_mass);
                    /* declared minus assay Pu mass and error */
                    sec.AddNumericRow("Declared - assay Pu mass (g):", dcl_minus_asy_pu_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_pu_mass_pct, (pu_mass.err / dcl_pu_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                }
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));

                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_known_m_rec";

                ps.Add(new DBParamEntry("mult", mult));
                ps.Add(new DBParamEntry("alpha", alpha));
                ps.Add(new DBParamEntry("pu239e_mass", pu239e_mass));

                ps.AddRange(DBParamList.TuplePair(pu240e_mass, "pu240e_mass"));
                ps.AddRange(DBParamList.TuplePair(pu_mass, "pu_mass"));
                ps.Add(new DBParamEntry("dcl_pu240e_mass", dcl_pu240e_mass));
                ps.Add(new DBParamEntry("dcl_pu_mass", dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_pu_mass, "dcl_minus_asy_pu_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", dcl_minus_asy_pu_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));   
            }
        }

        public class results_active_rec : INCCMethodResult
        {
            public Tuple k, k0, k1;
            public Tuple u235_mass;
            public double dcl_u235_mass;
            public Tuple dcl_minus_asy_u235_mass;
            public double dcl_minus_asy_u235_mass_pct;
            public bool pass;

            public new INCCAnalysisParams.active_rec methodParams
            {
                get { return (INCCAnalysisParams.active_rec)base.methodParams; }
                set { base.methodParams = value; }
            }
            public results_active_rec()
            {
                u235_mass = Tuple.Create();
                dcl_minus_asy_u235_mass = Tuple.Create();
                k = Tuple.Create();
                k0 = Tuple.Create();
                k1 = Tuple.Create();
                methodParams = new INCCAnalysisParams.active_rec();
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_active_rec tgt = (results_active_rec)imr;
                tgt.k = new Tuple(k);
                tgt.k0 = new Tuple(k0);
                tgt.k1 = new Tuple(k1);
                tgt.dcl_u235_mass = dcl_u235_mass;
                tgt.dcl_minus_asy_u235_mass = new Tuple(dcl_minus_asy_u235_mass);
                tgt.u235_mass = new Tuple(u235_mass);
                tgt.dcl_minus_asy_u235_mass_pct = dcl_minus_asy_u235_mass_pct;
                tgt.pass = pass;
                if (tgt.methodParams == null)
                    tgt.methodParams = new INCCAnalysisParams.active_rec(methodParams);
                else
                    methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }


            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
 
                sec.AddHeader(" Active calibration curve results");
                sec.AddNumericRow("k0 (source yield factor):", k0.v);
                sec.AddNumericRow("k1 (stability factor):", k1);
                sec.AddNumericRow("K (total correction factor):", k);
                sec.AddNumericRow("U235 mass (g):", u235_mass);

                if (dcl_u235_mass > 0.0)
                {
                    /* declared pu240e */
                    sec.AddNumericRow("Declared U235 mass (g):", dcl_u235_mass);
                    /* declared minus assay Pu mass and error */
                    sec.AddNumericRow("Declared - assay U235 mass (g):", dcl_minus_asy_u235_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_u235_mass_pct, (u235_mass.err / dcl_u235_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay U235 mass (%):", temp);
                }
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_active_rec";

                ps.Add(new DBParamEntry("k0", k0.v));
                ps.AddRange(DBParamList.TuplePair("k", k));
                ps.AddRange(DBParamList.TuplePair("k1", k1));

                ps.Add(new DBParamEntry("dcl_u235_mass", dcl_u235_mass));
                ps.AddRange(DBParamList.TuplePair("u235_mass", u235_mass));
                ps.AddRange(DBParamList.TuplePair("dcl_minus_asy_u235_mass", dcl_minus_asy_u235_mass));
                ps.Add(new DBParamEntry("dcl_minus_asy_u235_mass_pct", dcl_minus_asy_u235_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        public class results_active_mult_rec : INCCMethodResult
        {
            public Tuple mult;

            public new INCCAnalysisParams.active_mult_rec methodParams 
                    {   get { return (INCCAnalysisParams.active_mult_rec)base.methodParams; } 
                        set { base.methodParams = value; }
                    }

            public results_active_mult_rec()
            {
                mult = Tuple.Create();
                methodParams = new INCCAnalysisParams.active_mult_rec();
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_active_mult_rec tgt = (results_active_mult_rec)imr;
                tgt.mult = new Tuple(mult);
                methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);

                sec.AddHeader(" Active Multiplicity results"); // todo: complete this, DB table etc.; might already be done
                sec.AddNumericRow("mult:", mult);
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }
            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_active_mult_rec";
                ps.AddRange(DBParamList.TuplePair(mult, "mult"));
            }
        }

        public class results_active_passive_rec : INCCMethodResult
        {
            public Tuple delta_doubles;
            public Tuple u235_mass;
            public Tuple k0, k1, k;
            public double dcl_u235_mass;
            public Tuple dcl_minus_asy_u235_mass;
            public double dcl_minus_asy_u235_mass_pct;
            public bool pass;

            public new INCCAnalysisParams.active_passive_rec methodParams;

            public results_active_passive_rec() : base()
            {
                delta_doubles = Tuple.Create();
                u235_mass = Tuple.Create();
                dcl_minus_asy_u235_mass = Tuple.Create();
                k = Tuple.Create();
                k0 = Tuple.Create();
                k1 = Tuple.Create();
                delta_doubles = Tuple.Create();
                methodParams = new INCCAnalysisParams.active_passive_rec();
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_active_passive_rec tgt = (results_active_passive_rec)imr;
                tgt.delta_doubles = new Tuple(delta_doubles);
                tgt.u235_mass = u235_mass;
                tgt.k = new Tuple(k);
                tgt.k0 = new Tuple(k0);
                tgt.k1 = new Tuple(k1);
                tgt.dcl_u235_mass = dcl_u235_mass;
                tgt.dcl_minus_asy_u235_mass = new Tuple(dcl_minus_asy_u235_mass);
                tgt.u235_mass = new Tuple(u235_mass);
                tgt.dcl_minus_asy_u235_mass_pct = dcl_minus_asy_u235_mass_pct;
                tgt.pass = pass;
                methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }

            // todo: check for INCC5 output completeness, and integrate with parent output
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);

                sec.AddHeader(" Active Passive calibration curve results");
                sec.AddNumericRow("k0 (source yield factor):", k0.v);
                sec.AddNumericRow("k1 (stability factor):", k1);
                sec.AddNumericRow("K (total correction factor):", k);
                sec.AddNumericRow("U235 mass (g):", u235_mass);

                if (dcl_u235_mass > 0.0)
                {
                    /* declared pu240e */
                    sec.AddNumericRow("Declared U235 mass (g):", dcl_u235_mass);
                    /* declared minus assay Pu mass and error */
                    sec.AddNumericRow("Declared - assay U235 mass (g):", dcl_minus_asy_u235_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_u235_mass_pct, (u235_mass.err / dcl_u235_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay U235 mass (%):", temp);
                }
                // optional parameter lines
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_active_passive_rec";

                ps.AddRange(DBParamList.TuplePair(delta_doubles, "delta_doubles"));
                ps.AddRange(DBParamList.TuplePair(delta_doubles, "u235_mass"));

                ps.Add(new DBParamEntry("k0", k0.v));
                ps.AddRange(DBParamList.TuplePair(k, "k"));
                ps.AddRange(DBParamList.TuplePair(k, "k1"));

                ps.Add(new DBParamEntry("dcl_u235_mass", dcl_u235_mass));
                ps.AddRange(DBParamList.TuplePair(u235_mass, "u235_mass"));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_u235_mass, "u235_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_u235_mass_pct", dcl_minus_asy_u235_mass_pct));

                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        public class TMAttributes
        {
            public Tuple alpha, pu240e_mass, pu_mass, dcl_minus_asy_pu_mass, eff;
            public double dcl_pu240e_mass, dcl_pu_mass, dcl_minus_asy_pu_mass_pct;
            public bool pass;
            public TMAttributes()
            {
                alpha = new Tuple();
                pu240e_mass = new Tuple();
                pu_mass = new Tuple();
                dcl_minus_asy_pu_mass = new Tuple();
                eff = new Tuple();
            }
            public TMAttributes(TMAttributes src)
            {
                dcl_pu240e_mass = src.dcl_pu240e_mass;
                dcl_pu_mass = src.dcl_pu_mass;
                dcl_minus_asy_pu_mass_pct = src.dcl_minus_asy_pu_mass_pct;
                alpha = new Tuple(src.alpha);
                pu240e_mass = new Tuple(src.pu240e_mass);
                pu_mass = new Tuple(src.pu_mass);
                dcl_minus_asy_pu_mass = new Tuple(src.dcl_minus_asy_pu_mass);
                eff = new Tuple(src.eff);
                pass = src.pass;
            }
            public void CopyTo(TMAttributes dest)
            {
                dest.dcl_pu240e_mass = dcl_pu240e_mass;
                dest.dcl_pu_mass = dcl_pu_mass;
                dest.dcl_minus_asy_pu_mass_pct = dcl_minus_asy_pu_mass_pct;
                dest.alpha = new Tuple(alpha);
                dest.pu240e_mass = new Tuple(pu240e_mass);
                dest.pu_mass = new Tuple(pu_mass);
                dest.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                dest.eff = new Tuple(eff);
                dest.pass = pass;
            }

            // aliases in case the class instance really is referring to U or U235
            public Tuple mass
            {
                get { return pu_mass; } 
                set { pu_mass = value; }
            }
            public double dcl_mass
            {
                get { return dcl_pu_mass; }
                set { dcl_pu_mass = value; }
            }

            public Tuple dcl_minus_asy_mass
            {
                get { return dcl_minus_asy_pu_mass; } 
                set { dcl_minus_asy_pu_mass = value; }
            }
            public double dcl_minus_asy_mass_pct
            {
                get { return dcl_minus_asy_pu_mass_pct; }
                set { dcl_minus_asy_pu_mass_pct = value; }
            }
            
        }

        public class results_truncated_mult_rec : INCCMethodResult
        {
            public TruncatedMultiplicityBins bkg, net;
            public TMAttributes k, s;
            public new INCCAnalysisParams.truncated_mult_rec methodParams
                                {   get { return (INCCAnalysisParams.truncated_mult_rec)base.methodParams; } 
                                    set { base.methodParams = value; }
                                }
            // todo: pass ??
            public results_truncated_mult_rec()
            {
                methodParams = new INCCAnalysisParams.truncated_mult_rec();
                k = new TMAttributes();
                s = new TMAttributes();
                bkg = new TruncatedMultiplicityBins();
                net = new TruncatedMultiplicityBins();
            }

            public results_truncated_mult_rec(results_truncated_mult_rec src)
            {
                methodParams = new INCCAnalysisParams.truncated_mult_rec(src.methodParams);
                k = new TMAttributes(src.k);
                s = new TMAttributes(src.s);
                bkg = new TruncatedMultiplicityBins(src.bkg);
                net = new TruncatedMultiplicityBins(src.net);
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_truncated_mult_rec tgt = (results_truncated_mult_rec)imr;
                methodParams.CopyTo(tgt.methodParams);
                k.CopyTo(tgt.k);
                s.CopyTo(tgt.s);
                bkg.CopyTo(tgt.bkg);
                net.CopyTo(tgt.net);
                // todo: pass ??
                imr.modified = true;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Truncated multiplicity results");  // section header
                sec.AddNumericRow("Background singles:", bkg.Singles);
                sec.AddNumericRow("Background zeros:", bkg.Zeros);
                sec.AddNumericRow("Background ones:", bkg.Ones);
                sec.AddNumericRow("Background twos:", bkg.Twos);
                sec.AddNumericRow("Net singles:", net.Singles);
                sec.AddNumericRow("Net zeros:", net.Zeros);
                sec.AddNumericRow("Net ones:", net.Ones);
                sec.AddNumericRow("Net twos:", net.Twos);
                if (methodParams.known_eff)
                {
                    sec.AddHeader("Known efficiency method");
                    sec.AddNumericRow("Alpha:", k.alpha);
                    sec.AddNumericRow("Pu240e mass (g):", k.pu240e_mass);
                    sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(k.pu240e_mass.v, k.pu_mass.v));
                    sec.AddNumericRow("Pu mass (g):", k.pu_mass);
                    if (k.dcl_pu_mass > 0.0)
                    {
                        sec.AddNumericRow("Declared Pu240e mass (g):", k.dcl_pu240e_mass);
                        sec.AddNumericRow("Declared Pu mass (g):", k.dcl_pu_mass);
                        sec.AddNumericRow("Declared - assay Pu mass (g):", k.dcl_minus_asy_pu_mass);
                        Tuple temp = new Tuple(k.dcl_minus_asy_pu_mass_pct, (k.pu_mass.err / k.dcl_pu_mass) * 100.0);
                        sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                    }
                }
                if (methodParams.solve_eff)
                {
                    sec.AddHeader("Solve for efficiency method");
                    sec.AddNumericRow("Alpha:", k.alpha);
                    sec.AddNumericRow("Pu240e mass (g):", k.pu240e_mass);
                    sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(k.pu240e_mass.v, k.pu_mass.v));
                    sec.AddNumericRow("Pu mass (g):", k.pu_mass);
                    if (k.dcl_pu_mass > 0.0)
                    {
                        sec.AddNumericRow("Declared Pu240e mass (g):", k.dcl_pu240e_mass);
                        sec.AddNumericRow("Declared Pu mass (g):", k.dcl_pu_mass);
                        sec.AddNumericRow("Declared - assay Pu mass (g):", k.dcl_minus_asy_pu_mass);
                        Tuple temp = new Tuple(k.dcl_minus_asy_pu_mass_pct, (k.pu_mass.err / k.dcl_pu_mass) * 100.0);
                        sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                    }
                    if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                }
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_truncated_mult_rec";
                bkg.prefix = "bkg"; bkg.GenParamList(); ps.AddRange(bkg.ps);
                net.prefix = "net"; net.GenParamList(); ps.AddRange(net.ps);
                ps.AddRange(DBParamList.TuplePair("k_alpha", k.alpha));
                ps.AddRange(DBParamList.TuplePair("k_pu240e_mass", k.pu240e_mass));
                ps.AddRange(DBParamList.TuplePair("k_pu_mass", k.pu_mass));
                ps.AddRange(DBParamList.TuplePair("k_pu240e_mass", k.pu240e_mass));
                ps.AddRange(DBParamList.TuplePair("pu_mass", k.pu_mass));
                ps.Add(new DBParamEntry("k_dcl_pu240e_mass", k.dcl_pu240e_mass));
                ps.Add(new DBParamEntry("k_dcl_pu_mass", k.dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair("k_dcl_minus_asy_pu_mass", k.dcl_minus_asy_pu_mass));
                ps.Add(new DBParamEntry("k_dcl_minus_asy_pu_mass_pct", k.dcl_minus_asy_pu_mass_pct));
                ps.Add(new DBParamEntry("k_pass", k.pass));
                ps.AddRange(DBParamList.TuplePair( "s_eff", s.eff));
                ps.AddRange(DBParamList.TuplePair("s_alpha", s.alpha));
                ps.AddRange(DBParamList.TuplePair( "s_pu240e_mass", s.pu240e_mass));
                ps.AddRange(DBParamList.TuplePair("s_pu_mass", s.pu_mass));
                ps.AddRange(DBParamList.TuplePair("s_pu240e_mass", s.pu240e_mass));
                ps.AddRange(DBParamList.TuplePair("pu_mass", s.pu_mass));
                ps.Add(new DBParamEntry("s_dcl_pu240e_mass", s.dcl_pu240e_mass));
                ps.Add(new DBParamEntry("s_dcl_pu_mass", s.dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair( "s_dcl_minus_asy_pu_mass", s.dcl_minus_asy_pu_mass));
                ps.Add(new DBParamEntry("s_dcl_minus_asy_pu_mass_pct", s.dcl_minus_asy_pu_mass_pct));
                ps.Add(new DBParamEntry("s_pass", s.pass));   
            }
               
        }

        public class results_add_a_source_rec : INCCMethodResult
        {
            public Tuple pu240e_mass;
            public Tuple pu_mass;
            public double dzero_cf252_doubles;
            public Tuple sample_avg_cf252_doubles;
            public Tuple corr_doubles;
            public Tuple delta;
            public Tuple corr_factor;
            public double[] sample_cf252_ratio;
            public Tuple[] sample_cf252_doubles;
            public Tuple tm_doubles_bkg, tm_uncorr_doubles, tm_corr_doubles;

            public double dcl_pu240e_mass;
            public double dcl_pu_mass;
            public Tuple dcl_minus_asy_pu_mass;
            public double dcl_minus_asy_pu_mass_pct;
            public bool pass;

            public new INCCAnalysisParams.add_a_source_rec methodParams
            {
                get { return (INCCAnalysisParams.add_a_source_rec)base.methodParams; }
                set { base.methodParams = value; }
            }

            public results_add_a_source_rec()
            {
                pu240e_mass = Tuple.Create();
                pu_mass = Tuple.Create();
                sample_avg_cf252_doubles = Tuple.Create();
                corr_doubles = Tuple.Create();
                delta = Tuple.Create();
                corr_factor = Tuple.Create();
                dcl_minus_asy_pu_mass = Tuple.Create();
                sample_cf252_ratio = new double[INCCAnalysisParams.MAX_ADDASRC_POSITIONS];
                sample_cf252_doubles = Tuple.MakeArray(INCCAnalysisParams.MAX_ADDASRC_POSITIONS);
                tm_doubles_bkg= Tuple.Create(); tm_uncorr_doubles= Tuple.Create(); tm_corr_doubles= Tuple.Create();
                methodParams = new INCCAnalysisParams.add_a_source_rec();
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_add_a_source_rec tgt = (results_add_a_source_rec)imr;
                tgt.pu240e_mass = new Tuple(pu240e_mass);
                tgt.pu_mass = new Tuple(pu_mass);
                tgt.sample_avg_cf252_doubles = new Tuple(sample_avg_cf252_doubles);
                tgt.corr_doubles = new Tuple(corr_doubles);
                tgt.delta = new Tuple(delta);
                tgt.dcl_minus_asy_pu_mass = new Tuple(dcl_minus_asy_pu_mass);
                tgt.tm_doubles_bkg = new Tuple(tm_doubles_bkg);
                tgt.tm_uncorr_doubles = new Tuple(tm_uncorr_doubles);
                tgt.tm_corr_doubles = new Tuple(tm_corr_doubles);
                Array.Copy(sample_cf252_ratio, tgt.sample_cf252_ratio, sample_cf252_ratio.Length);
                Array.Copy(sample_cf252_doubles, tgt.sample_cf252_doubles, sample_cf252_doubles.Length); // todo: not a deep copy but does it matter?
                methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }


            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[m.Detector.MultiplicityParams];  // devnote: multmult assuming only one detector here, doh

                sec.AddHeader(" Add-a-source results");  // section header
                if (methodParams.use_truncated_mult) // && (m.->doubles <= methodParams.tm_dbls_rate_upper_limit)) // next: Where does double value come from? AAS results or summary results or where?
                    sec.AddHeader("using truncated multiplicity rates");
                
                // look up on global AASS map to find settings used for the current detector, it's independent of the measurement but ... 
                // NEXT: needs a copy for results in db related back to this measurement (e.g. add_a_source_setup_rec_m)
                AddASourceSetup aass = NCC.CentralizedState.App.DB.AASSParameters.Get(m.AcquireState.detector_id);

                if ((aass.type == AddASourceFlavors.Canberra_Counter) || (aass.type == AddASourceFlavors.PSC_WDAS))
                    sec.AddTwo("Net drum weight (kg):", m.INCCAnalysisResults.TradResultsRec.net_drum_weight); // drum weight computed when taking AAS measurement, stored on INCC5 results aggregate record
                sec.AddTwoWithTag("Reference Cf252 doubles:", dzero_cf252_doubles,"               ref/meas");

                for (int i = 0; i < aass.number_positions; i++)
                {
                    string s = string.Format("Position {0} Cf252 - item doubles:", i+1);
                    sec.AddNumericRowWithExtra(s, sample_cf252_doubles[i],sample_cf252_ratio[i]);
                }

                if (aass.number_positions > 1)
                    sec.AddNumericRow("Average Cf252 - item doubles:", sample_avg_cf252_doubles);

                if (methodParams.use_truncated_mult && (mcr.DeadtimeCorrectedDoublesRate.v <= methodParams.tm_dbls_rate_upper_limit))
                {
                    sec.AddNumericRow("Truncated mult ones bkgrnd", tm_doubles_bkg);
                    sec.AddNumericRow("Truncated mult item ones", tm_uncorr_doubles);
                    sec.AddNumericRow("Truncated mult weighting factor", methodParams.tm_weighting_factor);
                }
                else
                    sec.AddNumericRow("Item only doubles", mcr.DeadtimeCorrectedDoublesRate.v);

                sec.AddNumericRow("Item only doubles", mcr.DeadtimeCorrectedDoublesRate.v);
                sec.AddNumericRow("Delta:", delta);
                sec.AddNumericRow("Correction factor:", corr_factor);
                sec.AddNumericRow("Corrected item only doubles:", corr_doubles);
                sec.AddNumericRow("Pu240e mass (g):", pu240e_mass);
                sec.AddNumericRow("Pu240e (%):", calc_percent_pu240e(pu240e_mass.v, pu_mass.v));
                sec.AddNumericRow("Pu mass (g):", pu_mass);
                if (dcl_pu_mass > 0.0)
                {
                    sec.AddNumericRow("Declared Pu240e mass (g):", dcl_pu240e_mass);
                    sec.AddNumericRow("Declared Pu mass (g):", dcl_pu_mass);
                    sec.AddNumericRow("Declared - assay Pu mass (g):", dcl_minus_asy_pu_mass);
                    Tuple temp = new Tuple(dcl_minus_asy_pu_mass_pct, (pu_mass.err / dcl_pu_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                }

                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));

                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_add_a_source_rec";
                ps.Add(new DBParamEntry("dzero_cf252_doubles", dzero_cf252_doubles));
                ps.AddRange(DBParamList.TupleArrayPair("sample_cf252_doubles", sample_cf252_doubles));
                ps.Add(new DBParamEntry("sample_cf252_ratio", sample_cf252_ratio));
                ps.AddRange(DBParamList.TuplePair( "sample_avg_cf252_doubles", sample_avg_cf252_doubles));
                ps.AddRange(DBParamList.TuplePair("corr_doubles", corr_doubles));
                ps.AddRange(DBParamList.TuplePair("delta", delta));
                ps.AddRange(DBParamList.TuplePair("corr_factor", corr_factor));

                ps.AddRange(DBParamList.TuplePair("tm_doubles_bkg", tm_doubles_bkg));
                ps.AddRange(DBParamList.TuplePair("tm_uncorr_doubles", tm_uncorr_doubles));
                ps.AddRange(DBParamList.TuplePair("tm_corr_doubles", tm_corr_doubles));

                ps.AddRange(DBParamList.TuplePair("pu240e_mass", pu240e_mass));
                ps.AddRange(DBParamList.TuplePair("pu_mass", pu_mass));

                ps.Add(new DBParamEntry("dcl_pu240e_mass", dcl_pu240e_mass));
                ps.Add(new DBParamEntry("dcl_pu_mass", dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair("dcl_minus_asy_pu_mass", dcl_minus_asy_pu_mass));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", dcl_minus_asy_pu_mass_pct));
                ps.Add(new DBParamEntry("pass", pass));
            }

        }

        public class results_curium_ratio_rec : INCCMethodResult
        {
            public Tuple cm_mass, cm_pu_ratio_decay_corr, cm_u_ratio_decay_corr;

            public TMAttributes u235, u, pu;

            public new INCCAnalysisParams.curium_ratio_rec methodParams
            {
                get { return (INCCAnalysisParams.curium_ratio_rec)base.methodParams; }
                set { base.methodParams = value; }
            }
            public new INCCAnalysisParams.cm_pu_ratio_rec methodParams2
            {
                get { return (INCCAnalysisParams.cm_pu_ratio_rec)base.methodParams2; }
                set { base.methodParams2 = value; }
            }
            public results_curium_ratio_rec()
            {
                methodParams = new INCCAnalysisParams.curium_ratio_rec();
                methodParams2 = new INCCAnalysisParams.cm_pu_ratio_rec();
                u235 = new TMAttributes();
                u = new TMAttributes();
                pu = new TMAttributes();
                cm_mass = new Tuple();
                cm_pu_ratio_decay_corr = new Tuple(); cm_u_ratio_decay_corr = new Tuple();
            }

            public results_curium_ratio_rec(results_curium_ratio_rec src)
            {
                methodParams = new INCCAnalysisParams.curium_ratio_rec(src.methodParams);
                methodParams2 = new INCCAnalysisParams.cm_pu_ratio_rec(src.methodParams2);
                u235 = new TMAttributes(src.u235);
                u = new TMAttributes(src.u);
                pu = new TMAttributes(src.pu);
                cm_mass = new Tuple(src.cm_mass); 
                cm_pu_ratio_decay_corr = new Tuple(src.cm_pu_ratio_decay_corr);
                cm_u_ratio_decay_corr = new Tuple(src.cm_u_ratio_decay_corr);
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_curium_ratio_rec tgt = (results_curium_ratio_rec)imr;
                methodParams.CopyTo(tgt.methodParams);
                methodParams2.CopyTo(tgt.methodParams2);
                u235.CopyTo(tgt.u235);
                u.CopyTo(tgt.u);
                pu.CopyTo(tgt.pu);
                tgt.cm_mass = new Tuple(cm_mass);
                tgt.cm_pu_ratio_decay_corr = new Tuple(cm_pu_ratio_decay_corr);
                tgt.cm_u_ratio_decay_corr = new Tuple(cm_u_ratio_decay_corr);

                imr.modified = true;
            }


            // NEXT: incomplete, still needs to get results_add_a_source.ad_corr_doubles
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Curium ratio results");  // section header
                if (methodParams2 != null) sec.AddRange(methodParams2.ToLines(m));
                sec.AddNumericRow("Decay corrected Cm/Pu ratio:", cm_pu_ratio_decay_corr);
                sec.AddNumericRow("Decay corrected Cm/U ratio:", cm_u_ratio_decay_corr);
                if (this.methodParams.curium_ratio_type == INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles)
                    sec.AddTwo("Add-a-source corrected doubles:", "todo: get results_add_a_source.ad_corr_doubles");
                sec.AddNumericRow("Cm mass (g)", cm_mass);
                sec.AddNumericRow("Pu mass (g)", pu.mass);
                sec.AddNumericRow("U mass (g)", u.mass);
                sec.AddNumericRow("U235 mass (g)", u235.mass);
                if (pu.dcl_pu_mass > 0.0)
                {
                    sec.AddNumericRow("Declared Pu mass (g):", pu.dcl_mass);
                    sec.AddNumericRow("Declared - assay Pu mass (g):", pu.dcl_minus_asy_mass);
                    Tuple temp = new Tuple(pu.dcl_minus_asy_mass_pct, (pu.mass.err / pu.dcl_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay Pu mass (%):", temp);
                }             
                if (u.dcl_mass > 0.0)
                {
                    sec.AddNumericRow("Declared U mass (g):", u.dcl_mass);
                    sec.AddNumericRow("Declared - assay U mass (g):", u.dcl_minus_asy_mass);
                    Tuple temp = new Tuple(u.dcl_minus_asy_mass_pct, (u.mass.err / u.dcl_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay U mass (%):", temp);
                }
                if (u235.dcl_mass > 0.0)
                {
                    sec.AddNumericRow("Declared U235 mass (g):", u235.dcl_mass);
                    sec.AddNumericRow("Declared - assay U235 mass (g):", u235.dcl_minus_asy_mass);
                    Tuple temp = new Tuple(u.dcl_minus_asy_mass_pct, (u235.mass.err / u235.dcl_mass) * 100.0);
                    sec.AddNumericRow("Declared - assay U235 mass (%):", temp);
                }
                if (methodParams != null) sec.AddRange(methodParams.ToLines(m));
                if (methodParams.curium_ratio_type == INCCAnalysisParams.CuriumRatioVariant.UseSingles)
                {
                    sec.AddHeader("Analysis based on singles rate.");
                }
                else if (methodParams.curium_ratio_type == INCCAnalysisParams.CuriumRatioVariant.UseDoubles)
                {
                    sec.AddHeader("Analysis based on doubles rate.");
                }
                else if (methodParams.curium_ratio_type == INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles)
                {
                    sec.AddHeader("Analysis based on add-a-source corrected doubles rate.");
                }
                return sec;
            }
            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_curium_ratio_rec";
                ps.AddRange(DBParamList.TuplePair(pu.pu240e_mass, "pu240e_mass"));
                ps.AddRange(DBParamList.TuplePair(pu.pu_mass, "pu_mass"));
                ps.Add(new DBParamEntry("dcl_pu_mass",pu.dcl_pu_mass));
                ps.AddRange(DBParamList.TuplePair(pu.dcl_minus_asy_pu_mass, "dcl_minus_asy_pu_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_pu_mass_pct", pu.dcl_minus_asy_pu_mass_pct));

                ps.AddRange(DBParamList.TuplePair(cm_mass, "cm_mass"));

                ps.AddRange(DBParamList.TuplePair(u.mass, "u_mass"));               
                ps.AddRange(DBParamList.TuplePair(u.dcl_minus_asy_mass, "dcl_minus_asy_u_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_u_mass_pct", u.dcl_minus_asy_mass_pct));
                ps.Add(new DBParamEntry("dcl_u_mass", u.dcl_mass));

                ps.AddRange(DBParamList.TuplePair(u235.mass, "u235_mass"));               
                ps.AddRange(DBParamList.TuplePair(u235.dcl_minus_asy_mass, "dcl_minus_asy_u235_mass"));
                ps.Add(new DBParamEntry("dcl_minus_asy_u235_mass_pct", u235.dcl_minus_asy_mass_pct));
                ps.Add(new DBParamEntry("dcl_u235_mass", u235.dcl_mass));

                ps.Add(new DBParamEntry("pu_pass", pu.pass));
                ps.Add(new DBParamEntry("u_pass", u.pass));

                ps.AddRange(DBParamList.TuplePair(cm_pu_ratio_decay_corr, "cm_pu_ratio_decay_corr"));
                ps.AddRange(DBParamList.TuplePair(cm_u_ratio_decay_corr, "cm_u_ratio_decay_corr"));

            }

        }

        public class results_collar_rec : INCCMethodResult
        {
            public Tuple k0,k1,k2,k3,k4,k5;
            public Tuple u235_mass;
            public double percent_u235;
            public double total_u_mass;
            public Tuple total_corr_fact;
            public string source_id;
            public Tuple corr_doubles, dcl_length, dcl_total_u235, dcl_total_u238;
            public double dcl_total_rods, dcl_total_poison_rods;
            public Tuple dcl_poison_percent;

            //public double dcl_u235_mass;
            public Tuple dcl_minus_asy_u235_mass;
            public double dcl_minus_asy_u235_mass_pct;
            public bool pass;

			public void InitMethodParams()
			{
				methodParams = new INCCAnalysisParams.collar_combined_rec();
			}
			
			public void InitMethodParams(results_collar_rec src)
			{
				methodParams = new INCCAnalysisParams.collar_combined_rec((INCCAnalysisParams.collar_combined_rec)src.methodParams);
			}


            public INCCAnalysisParams.collar_rec methodParamsc { get { return ((INCCAnalysisParams.collar_combined_rec)methodParams).collar; }
																 set { ((INCCAnalysisParams.collar_combined_rec)methodParams).collar = value; } }
			public INCCAnalysisParams.collar_detector_rec methodParamscd  { get { return ((INCCAnalysisParams.collar_combined_rec)methodParams).collar_det; }
																 set { ((INCCAnalysisParams.collar_combined_rec)methodParams).collar_det = value; } }
			public INCCAnalysisParams.collar_k5_rec methodParamsck5 { get { return ((INCCAnalysisParams.collar_combined_rec)methodParams).k5; } 
																 set { ((INCCAnalysisParams.collar_combined_rec)methodParams).k5 = value; } }
            public string poison_rod_type
            {
                get { return methodParamsc.poison_rod_type[0]; }
            }
            public double poison_absorption_fact
            {
                get { return methodParamsc.poison_absorption_fact[0]; }
            }
            public Tuple poison_rod_a
            {
                get { return methodParamsc.poison_rod_a[0]; }
            }
            public Tuple poison_rod_b
            {
                get { return methodParamsc.poison_rod_b[0]; }
            }
            public Tuple poison_rod_c
            {
                get { return methodParamsc.poison_rod_c[0]; }
            }
            public results_collar_rec()
            {
				InitMethodParams();
                source_id = string.Empty;
                k0= new Tuple();k1= new Tuple();k2= new Tuple();k3= new Tuple();k4= new Tuple();k5 = new Tuple();
                u235_mass = new Tuple();
                total_corr_fact = new Tuple();
                corr_doubles= new Tuple(); dcl_length= new Tuple();dcl_total_u235= new Tuple(); dcl_total_u238= new Tuple();
                dcl_poison_percent = new Tuple();
                dcl_minus_asy_u235_mass = new Tuple();
            }

            public results_collar_rec(results_collar_rec src)
            {
                InitMethodParams(src);
                k0 = new Tuple(src.k0); k1 = new Tuple(src.k1); k2 = new Tuple(src.k2); k3 = new Tuple(src.k3); k4 = new Tuple(src.k4); k5 = new Tuple(src.k5);
                u235_mass = new Tuple(src.u235_mass);
                percent_u235 = src.percent_u235;
                total_u_mass = src.total_u_mass;
                total_corr_fact = new Tuple(src.total_corr_fact);
                source_id = string.Copy(src.source_id);
                corr_doubles = new Tuple(src.corr_doubles); dcl_length = new Tuple(src.dcl_length); dcl_total_u235 = new Tuple(src.dcl_total_u235); dcl_total_u238 = new Tuple(src.dcl_total_u238);
                dcl_poison_percent = new Tuple(src.dcl_poison_percent);
                dcl_total_rods = src.dcl_total_rods;
                dcl_total_poison_rods = src.dcl_total_poison_rods;
                dcl_minus_asy_u235_mass = new Tuple(src.dcl_minus_asy_u235_mass);
                dcl_minus_asy_u235_mass_pct = src.dcl_minus_asy_u235_mass_pct;
                pass = src.pass;
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_collar_rec tgt = (results_collar_rec)imr;
                tgt.k0 = new Tuple(k0); tgt.k1 = new Tuple(k1); tgt.k2 = new Tuple(k2); tgt.k3 = new Tuple(k3); tgt.k4 = new Tuple(k4); tgt.k5 = new Tuple(k5);
                tgt.u235_mass = u235_mass;
                tgt.percent_u235 = percent_u235;
                tgt.total_corr_fact = new Tuple(total_corr_fact);
                tgt.source_id = string.Copy(source_id);
                tgt.corr_doubles = new Tuple(corr_doubles); tgt.dcl_length = new Tuple(dcl_length); tgt.dcl_total_u235 = new Tuple(dcl_total_u235); tgt.dcl_total_u238 = new Tuple(dcl_total_u238);
                tgt.dcl_poison_percent = new Tuple(dcl_poison_percent);
                tgt.dcl_total_rods = dcl_total_rods;
                tgt.dcl_total_poison_rods = dcl_total_poison_rods;
                tgt.dcl_minus_asy_u235_mass = new Tuple(dcl_minus_asy_u235_mass);
                tgt.dcl_minus_asy_u235_mass_pct = dcl_minus_asy_u235_mass_pct;
                tgt.pass = pass;
                methodParams.CopyTo(tgt.methodParams);
                imr.modified = true;
            }

            // NEXT: report line generation needed; start with INCC5 style, then expand 
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Do this next results (collar)");  // section header


                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_collar_rec";
                ps.AddRange(DBParamList.TuplePair(u235_mass, "u235_mass"));
                ps.Add(new DBParamEntry("percent_u235", percent_u235));
                ps.Add(new DBParamEntry("total_u_mass", total_u_mass));
                ps.AddRange(DBParamList.TuplePair(k0, "k0"));
                ps.AddRange(DBParamList.TuplePair(k1, "k1"));
                ps.AddRange(DBParamList.TuplePair(k2, "k2"));
                ps.AddRange(DBParamList.TuplePair(k3, "k3"));
                ps.AddRange(DBParamList.TuplePair(k4, "k4"));
                ps.AddRange(DBParamList.TuplePair(k5, "k5"));
                ps.AddRange(DBParamList.TuplePair(total_corr_fact, "total_corr_fact"));
                ps.Add (new DBParamEntry("source_id",source_id));
                ps.AddRange(DBParamList.TuplePair(corr_doubles, "corr_doubles"));
                ps.AddRange(DBParamList.TuplePair(dcl_length, "dcl_length"));
                ps.AddRange(DBParamList.TuplePair(dcl_total_u235, "dcl_total_u235"));
                ps.AddRange(DBParamList.TuplePair(dcl_total_u238, "dcl_total_u238"));
                ps.Add(new DBParamEntry("dcl_total_rods", dcl_total_rods));
                ps.Add(new DBParamEntry("dcl_total_poison_rods", dcl_total_poison_rods));
                ps.AddRange(DBParamList.TuplePair(dcl_poison_percent, "dcl_poison_percent"));
                ps.AddRange(DBParamList.TuplePair(dcl_minus_asy_u235_mass, "dcl_minus_asy_u235_mass"));
                ps.Add(new DBParamEntry("pass", pass));
            }
        }

        public class results_de_mult_rec : INCCMethodResult
        {
            public double meas_ring_ratio;
            public double interpolated_neutron_energy;
            public double energy_corr_factor;

            public new INCCAnalysisParams.de_mult_rec methodParams
            {
                get
                {
                    INCCAnalysisParams.de_mult_rec r = (INCCAnalysisParams.de_mult_rec)base.methodParams;
                    return r;
                }
                set { base.methodParams = value; }
            }

            public results_de_mult_rec()
            {
                methodParams = new INCCAnalysisParams.de_mult_rec();
            }

            public results_de_mult_rec(results_de_mult_rec src)
            {
                methodParams = new INCCAnalysisParams.de_mult_rec(src.methodParams);
                meas_ring_ratio = src.meas_ring_ratio;
                interpolated_neutron_energy = src.interpolated_neutron_energy;
                energy_corr_factor = src.energy_corr_factor;
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                results_de_mult_rec tgt = (results_de_mult_rec)imr;
                tgt.meas_ring_ratio = meas_ring_ratio;
                tgt.interpolated_neutron_energy = interpolated_neutron_energy;
                tgt.energy_corr_factor = energy_corr_factor;
                methodParams.CopyTo(tgt.methodParams);  
                imr.modified = true;
            }

            // NEXT: report line generation needed; start with INCC5 style, then expand 
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Do this next results (dual-energy multiplicity)");  // section header
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                this.Table = "results_de_mult_rec";
                ps.Add(new DBParamEntry("meas_ring_ratio", meas_ring_ratio));
                ps.Add(new DBParamEntry("interpolated_neutron_energy", interpolated_neutron_energy));
                ps.Add(new DBParamEntry("energy_corr_factor", energy_corr_factor));
            }
        }

        // NEXT: finish truncated multiplicity background use, these values are attached to bkg measurements when the truncated flag is enabled
        public class results_tm_bkg_rec : INCCMethodResult
        {

            private TruncatedMultiplicityBackgroundParameters tm;

            public new TruncatedMultiplicityBackgroundParameters methodParams
            {
                get { return tm; }
                set { tm = value; }
            }

            public results_tm_bkg_rec()
            {
                tm = new TruncatedMultiplicityBackgroundParameters();
            }

            public results_tm_bkg_rec(results_tm_bkg_rec src)
            {
                methodParams = new TruncatedMultiplicityBackgroundParameters(src.methodParams);
            }

            public override void CopyTo(INCCMethodResult imr)
            {
                //TruncatedMultiplicityBackgroundParameters tgt = (TruncatedMultiplicityBackgroundParameters)imr; // todo:this is a problem
               // methodParams.CopyTo(tgt);
                imr.modified = true;
            }

            // NEXT: report line generation needed; start with INCC5 style, then expand 
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.AddHeader("Do this next results (tm bkg)");  // section header
                return sec;
            }

            public override void GenParamList()
            {
                tm.prefix = "tm";
                tm.GenParamList();
                base.GenParamList();
                this.Table = "results_tm_bkg_rec";
                ps.AddRange(tm.ps);
            }
        }

    }  // end INCCMethodResults

}






