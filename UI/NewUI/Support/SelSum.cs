/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using AnalysisDefs;
using System;
using System.Collections.Generic;

namespace NewUI
{
    public class ResultsSummary
    {
        public enum Selections
        {
            AcquireParams,
            DetectorParams,
            NormalizationParams,
            BackgroundParams,
            Summaries,
            Isotopics,
            TestParams,
            Stratum,
            ResultsFiles,
            MassAnalysisMethods,
                CalibrationCurve,
                KnownAlpha,
                KnownM,
                Multiplicity,
                AddASource,
                CuriumRatio,
                TruncatedMultiplicity,
                ActiveCalibCurve,
                CollarAmLi,
                ActiveMultiplicity,
                ActivePassive,
                CollarCf,
			Comments,
            LMResults
        }

        public class State
        {
            public State(Selections m, bool b)
            {
                Enabled = b; Entries = new List<Dictionary<string, string>>() { };
            }
            public bool Enabled { get; set; }
            public Selections Parent { get; set; }
            public Selections Me { get; set; }

            public List<Dictionary<string, string>> Entries;
        }

        public Dictionary<string, State> Root = new Dictionary<string, State>();
        public bool SortStratum, WithCurrentEndDate;
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public string InspectionNumber { get; set; }
        public string Option { get; set; }

        public ResultsSummary()
        {
            SortStratum = true; WithCurrentEndDate = true;
            Start = new DateTimeOffset(new DateTime(1997, 1, 1));
            End = DateTimeOffset.Now;
            InspectionNumber = "All";
            Root.Add(Selections.AcquireParams.ToString(), new State(Selections.AcquireParams, true));
            Root.Add(Selections.DetectorParams.ToString(), new State(Selections.DetectorParams, false));
            Root.Add(Selections.Summaries.ToString(), new State(Selections.Summaries, true));
            Root.Add(Selections.NormalizationParams.ToString(), new State(Selections.NormalizationParams, false));
            Root.Add(Selections.BackgroundParams.ToString(), new State(Selections.BackgroundParams, false));

            Root.Add(Selections.TestParams.ToString(), new State(Selections.TestParams, false));
            Root.Add(Selections.Isotopics.ToString(), new State(Selections.Isotopics, false));
            Root.Add(Selections.Stratum.ToString(), new State(Selections.Stratum, false));
            Root.Add(Selections.ResultsFiles.ToString(), new State(Selections.ResultsFiles, false));

			Root.Add(Selections.MassAnalysisMethods.ToString(), new State(Selections.MassAnalysisMethods, false));

			Root.Add(Selections.CalibrationCurve.ToString(), new State(Selections.CalibrationCurve, false));
            Root.Add(Selections.KnownAlpha.ToString(), new State(Selections.KnownAlpha, false));
            Root.Add(Selections.KnownM.ToString(), new State(Selections.KnownM, false));
            Root.Add(Selections.Multiplicity.ToString(), new State(Selections.Multiplicity, false));
            Root.Add(Selections.AddASource.ToString(), new State(Selections.AddASource, false));
            Root.Add(Selections.CuriumRatio.ToString(), new State(Selections.CuriumRatio, false));
            Root.Add(Selections.TruncatedMultiplicity.ToString(), new State(Selections.TruncatedMultiplicity, false));
            Root.Add(Selections.ActiveCalibCurve.ToString(), new State(Selections.ActiveCalibCurve, false));
            Root.Add(Selections.CollarAmLi.ToString(), new State(Selections.CollarAmLi, false));
            Root.Add(Selections.CollarCf.ToString(), new State(Selections.CollarCf, false));
            Root.Add(Selections.ActiveMultiplicity.ToString(), new State(Selections.ActiveMultiplicity, false));
            Root.Add(Selections.ActivePassive.ToString(), new State(Selections.ActivePassive, false));

			Root.Add(Selections.Comments.ToString(), new State(Selections.Comments, false));

			Root.Add(Selections.LMResults.ToString(), new State(Selections.LMResults, false));
         }

        public void Apply(Measurement m)
        {
            foreach (Selections s in Enum.GetValues(typeof(Selections)))
            {
                string key = s.ToString();
                if (!Root[key].Enabled)
                    continue;
                Dictionary<string, string> d = GenerateEntries(s, m);
                if (Root[key].Entries != null)
                    Root[key].Entries.Add(d);
            }
        }
        public void ResetSummaryRows()
        {
            foreach (Selections s in Enum.GetValues(typeof(Selections)))
            {
                string key = s.ToString();
                Root[key].Entries = new List<Dictionary<string, string>>();
            }
        }

		// INCC5 enables subselecting each field and creates a two line header instead of one.
		Dictionary<string, string> GenerateEntries(Selections s, Measurement m)
		{
			Dictionary<string, string> entries = new Dictionary<string, string>();
			switch (s)
			{
			case Selections.AcquireParams:
				entries["Facility"] = Q(m.AcquireState.facility.Name);
				entries["MBA"] = Q(m.AcquireState.mba.Name);
				entries["Detector"] = Q(m.AcquireState.detector_id);
				entries["IC"] = Q(m.AcquireState.inventory_change_code);
				entries["IO"] = Q(m.AcquireState.io_code);
				entries["Measurement Type"] = Q(m.MeasOption.PrintName());
				entries["Meas Date"] = m.MeasurementId.MeasDateTime.ToString("yy.MM.dd");
				entries["Meas Time"] = m.MeasurementId.MeasDateTime.ToString("HH:mm:ss");
				entries["File Name"] = Q(GetMainFilePath(m.ResultsFiles, m.MeasOption, true));
				entries["Inspector"] = Q(m.AcquireState.user_id);
				entries["Inspection Number"] = Q(m.AcquireState.campaign_id);
				entries["Item ID"] = Q(m.AcquireState.item_id);
				entries["Isotopics ID"] = Q(m.AcquireState.isotopics_id);
				entries["Stratum ID"] = Q(m.AcquireState.stratum_id.Name);
				entries["Material Type"] = Q(m.AcquireState.item_type);
				break;
			case Selections.NormalizationParams:
				entries["Norm Cnst"] = m.Norm.currNormalizationConstant.v.ToString("F4"); //	"Normalization constant"
				entries["Norm Cnst Error"] = m.Norm.currNormalizationConstant.err.ToString("F4"); //	"Normalization constant error"
				break;
			case Selections.BackgroundParams:
				entries["Singles Bkg"] = m.Background.DeadtimeCorrectedSinglesRate.v.ToString("F1"); //	"Passive singles background"
				entries["Singles Bkg Error"] = m.Background.DeadtimeCorrectedSinglesRate.err.ToString("F3"); //	"Passive singles background error"
				entries["Doubles Bkg"] = m.Background.DeadtimeCorrectedDoublesRate.v.ToString("F2"); //	"Passive doubles background"
				entries["Doubles Bkg Error"] = m.Background.DeadtimeCorrectedDoublesRate.err.ToString("F3"); //	"Passive doubles background error"
				entries["Triples Bkg"] = m.Background.DeadtimeCorrectedTriplesRate.v.ToString("F2"); //	"Passive triples background"
				entries["Triples Bkg Error"] = m.Background.DeadtimeCorrectedTriplesRate.err.ToString("F3"); //	"Passive triples background error"
				entries["Act Sngls Bkg"] = m.Background.INCCActive.SinglesRate.ToString("F1"); //	"Active singles background"
				entries["Act Sngls Bkg Error"] = m.Background.INCCActive.Singles.err.ToString("F3"); //	"Active singles background error"
				break;
			case Selections.Summaries:
				if (m.INCCAnalysisState != null && m.INCCAnalysisResults != null)
				{
					System.Collections.IEnumerator iter = m.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();
					while (iter.MoveNext())
					{
						MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
						INCCResult ir = m.INCCAnalysisResults[moskey];
						entries["Singles"] = ir.DeadtimeCorrectedSinglesRate.v.ToString("F1"); //	"Singles"
						entries["Singles Error"] = ir.DeadtimeCorrectedSinglesRate.err.ToString("F3"); //	"Singles error"
						entries["Doubles"] = ir.DeadtimeCorrectedDoublesRate.v.ToString("F2"); //	"Doubles"
						entries["Doubles Error"] = ir.DeadtimeCorrectedDoublesRate.err.ToString("F3"); //	"Doubles error"
						entries["Triples"] = ir.DeadtimeCorrectedTriplesRate.v.ToString("F2"); //	"Triples"
						entries["Triples Error"] = ir.DeadtimeCorrectedTriplesRate.err.ToString("F3"); //	"Triples error"
						if (ir.Scaler1.v > 0 || ir.Scaler2.v > 0)
						{
							entries["Scaler 1"] = ir.Scaler1.v.ToString("F1");
							entries["Scaler 1 Error"] = ir.Scaler1.err.ToString("F3");
							entries["Scaler 2"] = ir.Scaler2.v.ToString("F1");
							entries["Scaler 2 Error"] = ir.Scaler2.err.ToString("F3");
						}
						entries["Count Time"] = (m.Cycles.Count > 0 ? m.Cycles[0].TS.TotalSeconds.ToString("F0") : "0"); // "Total count time for a measurement"					
						entries["Singles Sum"] = m.SinglesSum.ToString("F0"); // Sums - singles  Singles sum for a measurement"
						entries["R+A Sum"] = ir.RASum.ToString("F0");    // Reals + accidentals sum for a measurement"
						entries["A Sum"] = ir.ASum.ToString("F0");  // "Accidentals sum for a measurement"
					}
				}
				break;
			case Selections.Comments:
				entries["Comment"] = m.AcquireState.comment;
				if (m.INCCAnalysisResults.TradResultsRec.acq.ending_comment && 
					!string.IsNullOrEmpty(m.INCCAnalysisResults.TradResultsRec.acq.ending_comment_str))
					entries["End Comment"] = m.AcquireState.ending_comment_str;
				break;
			case Selections.MassAnalysisMethods:
				break;
			case Selections.CalibrationCurve:
               INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.CalibrationCurve, false);
				if (ccres != null)
				{
					entries["Cal Cur Dcl Mass"] = ccres.dcl_pu_mass.ToString("F2"); //	Calibration curve - declared mass"
					entries["Cal Cur Mass"] = ccres.pu_mass.v.ToString("F2");	//	"Calibration curve - mass"
					entries["Cal Cur Mass Err"] = ccres.pu_mass.err.ToString("F3"); //	"Calibration curve - mass error"
					entries["Cal Cur Dcl-Asy"] = ccres.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Calibration curve - declared minus assay"
					entries["Cal Cur Dcl-Asy %"] = ccres.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Calibration curve - declared minus assay %"
					entries["Cal Cur Status"] = ccres.pass ? "Pass": ""; //	"Calibration curve - measurement status"
				}
				break;
			case Selections.KnownAlpha:
               INCCMethodResults.results_known_alpha_rec kares = (INCCMethodResults.results_known_alpha_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownA, false);
				if (kares != null)
				{
					entries["Known A Dcl Mass"] = kares.dcl_pu_mass.ToString("F2"); //	Known alpha - declared mass"
					entries["Known A Mass"] = kares.pu_mass.v.ToString("F2");	//	"Known alpha - mass"
					entries["Known A Mass Err"] = kares.pu_mass.err.ToString("F3"); //	"Known alpha - mass error"
					entries["Known A Dcl-Asy"] = kares.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Known alpha - declared minus assay"
					entries["Known A Dcl-Asy %"] = kares.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Known alpha - declared minus assay %"
					entries["Known A Mult"] = kares.mult.ToString("F3"); //	"Known alpha - multiplication"
					entries["Known A Alpha"] = kares.alphaK.ToString("F3"); //	"Known alpha - alpha"
					entries["Known A Status"] = kares.pass ? "Pass": ""; //	"Known alpha - measurement status"
				}
				break;
			case Selections.KnownM:
               INCCMethodResults.results_known_m_rec kmres = (INCCMethodResults.results_known_m_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownM, false);
				if (kmres != null)
				{
					entries["Known M Dcl Mass"] = kmres.dcl_pu_mass.ToString("F2"); //	Known M - declared mass"
					entries["Known M Mass"] = kmres.pu_mass.v.ToString("F2");	//	"Known M - mass"
					entries["Known M Mass Err"] = kmres.pu_mass.err.ToString("F3"); //	"Known M - mass error"
					entries["Known M Dcl-Asy"] = kmres.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Known M - declared minus assay"
					entries["Known M Dcl-Asy %"] = kmres.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Known M - declared minus assay %"
					entries["Known M Mult"] = kmres.mult.ToString("F3"); //	"Known M - multiplication"
					entries["Known M Alpha"] = kmres.alpha.ToString("F3"); //	"Known M - alpha"
					entries["Known M Status"] = kmres.pass ? "Pass": ""; //	"Known M - measurement status"
				}
				break;
			case Selections.Multiplicity:
               INCCMethodResults.results_multiplicity_rec mres = (INCCMethodResults.results_multiplicity_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.Multiplicity, false);
				if (mres != null)
				{
					entries["Mult Dcl Mass"] = mres.dcl_pu_mass.ToString("F2"); //	Multiplicity - declared mass"
					entries["Mult Mass"] = mres.pu_mass.v.ToString("F2");	//	"Multiplicity- mass"
					entries["Mult Mass Err"] = mres.pu_mass.err.ToString("F3"); //	"Multiplicity - mass error"
					entries["Mult Dcl-Asy"] = mres.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Multiplicity - declared minus assay"
					entries["Mult Dcl-Asy %"] = mres.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Multiplicity - declared minus assay %"
					entries["Mult Mult"] = mres.mult.v.ToString("F3"); //	"Multiplicity - multiplication"
					entries["Mult Mult Err"] = mres.mult.err.ToString("F3"); //	"Multiplicity - multiplication error"
					entries["Mult Alpha"] = mres.alphaK.v.ToString("F3"); //	"Multiplicity - alpha"
					entries["Mult Alpha Err"] = mres.alphaK.err.ToString("F3"); //	"Multiplicity - alpha error"
					entries["Mult Efficiency"] = mres.efficiencyComputed.v.ToString("F3"); //	"Multiplicity - efficiency"
					entries["Mult Eff Err"] = mres.efficiencyComputed.err.ToString("F3"); //	"Multiplicity - efficiency error"
					entries["Mult Status"] = mres.pass ? "Pass": ""; //	"Multiplicity - measurement status"
					entries["Mult Predelay ms"] = m.Detector.SRParams.predelayMS.ToString(); // 	"Multiplicity - predelay "		
					entries["Mult Gate ms"] = m.Detector.SRParams.gateLengthMS.ToString(); // 	"Multiplicity - gate width"
				}
				break;

			case Selections.AddASource: 
               INCCMethodResults.results_add_a_source_rec aares = (INCCMethodResults.results_add_a_source_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.AddASource, false);
				if (aares != null)
				{
					entries["Add-a-src Dcl Mass"] = aares.dcl_pu_mass.ToString("F2"); //	Add-a-source - declared mass"
					entries["Add-a-src Mass"] = aares.pu_mass.v.ToString("F2");	//	"Add-a-source - mass"
					entries["Add-a-src Mass Err"] = aares.pu_mass.err.ToString("F3"); //	"Add-a-source - mass error"
					entries["Add-a-src Dcl-Asy"] = aares.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Add-a-source - declared minus assay"
					entries["Add-a-src Dcl-Asy %"] = aares.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Add-a-source - declared minus assay %"
					entries["Add-a-src Status"] = aares.pass ? "Pass": ""; //	"Add-a-source - measurement status"
					entries["Add-a-src Corr"] = aares.corr_factor.v.ToString("F3"); //	"Add-a-source - measurement status"
				}
				break;
			case Selections.CuriumRatio: 
               INCCMethodResults.results_curium_ratio_rec cures = (INCCMethodResults.results_curium_ratio_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.CuriumRatio, false);
				if (cures != null)
				{
					entries["Cm Ratio ID"] = cures.methodParams2.cm_id; // Curium ratio - ID"
					entries["Cm Ratio Input ID"] = cures.methodParams2.cm_input_batch_id; // Curium ratio - input batch ID"
					entries["Cm Ratio Cm/Pu"] = cures.methodParams2.cm_pu_ratio.v.ToString("F4"); // Curium ratio - Cm/Pu ratio"
					entries["Cm Ratio Cm/U"] = cures.methodParams2.cm_u_ratio.v.ToString("F4"); // Curium ratio - Cm/U ratio"
					entries["Cm Mass"] = cures.cm_mass.v.ToString("F3"); // Curium ratio - Cm mass"
					entries["Cm Err"] = cures.cm_mass.err.ToString("F3"); // Curium ratio - Cm mass error"

					entries["Cm Ratio Pu Dcl Mass"] = cures.pu.dcl_pu_mass.ToString("F2"); // Curium ratio - declared mass"
					entries["Cm Ratio Pu Mass"] = cures.pu.pu_mass.v.ToString("F2");	//	"Curium ratio - mass"
					entries["Cm Ratio Pu Mass Err"] = cures.pu.pu_mass.err.ToString("F3"); //	"Curium ratio - mass error"
					entries["Cm Ratio Pu Dcl-Asy"] = cures.pu.dcl_minus_asy_pu_mass.v.ToString("F2"); //	"Curium ratio - declared minus assay"
					entries["Cm Ratio Pu Dcl-Asy %"] = cures.pu.dcl_minus_asy_pu_mass_pct.ToString("F2"); //	"Curium ratio - declared minus assay %"
					entries["Cm Ratio U Dcl Mass"] = cures.u.dcl_mass.ToString("F2"); // "Curium ratio - U declared mass"
					entries["Cm Ratio U Mass"] = cures.u.mass.v.ToString("F2");	//	"Curium ratio - U mass"
					entries["Cm Ratio U Mass Err"] = cures.u.mass.err.ToString("F3"); //	"Curium ratio - U mass error"
					entries["Cm Ratio U Dcl-Asy"] = cures.u.dcl_minus_asy_mass.v.ToString("F2"); //	"Curium ratio - U declared minus assay"
					entries["Cm Ratio U Dcl-Asy %"] = cures.u.dcl_minus_asy_mass_pct.ToString("F2"); //	"Curium ratio - U declared minus assay %"
					entries["Cm Ratio U235 Dcl Mass"] = cures.u235.dcl_mass.ToString("F2"); // Curium ratio - U235 declared mass"
                    entries["Cm Ratio U235 Mass"] = cures.u235.mass.v.ToString("F2");   //	"Curium ratio - U235 mass"
                    entries["Cm Ratio U235 Mass Err"] = cures.u235.mass.err.ToString("F3"); //	"Curium ratio - U235 mass error"
                    entries["Cm Ratio U235 Dcl-Asy"] = cures.u235.dcl_minus_asy_mass.v.ToString("F2"); //	"Curium ratio - U235 declared minus assay"
					entries["Cm Ratio U235 Dcl-Asy %"] = cures.u235.dcl_minus_asy_mass_pct.ToString("F2"); //	"Curium ratio - U235 declared minus assay %"
                    entries["Cm Ratio Pu Status"] = cures.pu.pass ? "Pass": ""; //	"Curium ratio - Pu measurement status"
					entries["Cm Ratio U Status"] = cures.u.pass ? "Pass": ""; //	"Curium ratio - U measurement status"
				}
				break;
			case Selections.TruncatedMultiplicity: 
               INCCMethodResults.results_truncated_mult_rec tmres = (INCCMethodResults.results_truncated_mult_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.TruncatedMultiplicity, false);
				if (tmres != null)
				{
					entries["Trunc Mult Dcl Mass"] = tmres.k.dcl_mass.ToString("F2"); // "Truncated multiplicity - declared mass"
					entries["Trunc Mult Mass"] = tmres.k.pu_mass.v.ToString("F2"); // "Truncated multiplicity - mass"
					entries["Trunc Mult Mass Err"] = tmres.k.pu_mass.err.ToString("F3"); // "Truncated multiplicity - mass error"
					entries["Trunc Mult Dcl-Asy"] = tmres.k.dcl_minus_asy_mass.v.ToString("F2"); // "Truncated multiplicity - declared minus assay"
					entries["Trunc Dcl-Asy %"] = tmres.k.dcl_minus_asy_mass_pct.ToString("F2"); //"Truncated multiplicity - declared minus assay %"
					entries["Trunc Mult Status"] = tmres.k.pass ? "Pass": ""; // "Truncated multiplicity - measurement status"  // todo: what about 's'?
				}
				break;
			case Selections.ActiveCalibCurve:
               INCCMethodResults.results_active_rec acres = (INCCMethodResults.results_active_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.Active, false);
				if (acres != null)
				{
					entries["Active Dcl Mass"] = acres.dcl_u235_mass.ToString("F2"); //	Active calibration curve - declared mass"
					entries["Active Mass"] = acres.u235_mass.v.ToString("F2");	//	"Active calibration curve - mass"
					entries["Active Mass Err"] = acres.u235_mass.err.ToString("F3"); //	"Active calibration curve - mass error"
					entries["Active Dcl-Asy"] = acres.dcl_minus_asy_u235_mass.v.ToString("F2"); //	"Active calibration curve - declared minus assay"
					entries["Active Dcl-Asy %"] = acres.dcl_minus_asy_u235_mass_pct.ToString("F2"); //	"Active calibration curve - declared minus assay %"
					entries["Active Status"] = acres.pass ? "Pass": ""; //	"Active calibration curve - measurement status"
				}
				break;
			case Selections.CollarAmLi:
            case Selections.CollarCf:
                INCCMethodResults.results_collar_rec rcres = (INCCMethodResults.results_collar_rec)
                        m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod.Collar, false);
				if (rcres != null)
				{
					entries["Collar Dbls Rate"] = rcres.total_corr_fact.v.ToString("F2"); //	Collar - corrected doubles rate"
					entries["Collar Dbls Rate Err"] = rcres.total_corr_fact.v.ToString("F2"); //	Collar -- corrected doubles rate error"
					entries["Collar Dcl Mass"] = rcres.dcl_total_u235.v.ToString("F2"); //	Collar - declared mass"
					entries["Collar Mass"] = rcres.u235_mass.v.ToString("F2");	//	"Collar - mass"
					entries["Collar Mass Err"] = rcres.u235_mass.err.ToString("F3"); //	"Collar - mass error"
					entries["Collar Dcl-Asy"] = rcres.dcl_minus_asy_u235_mass.v.ToString("F2"); //	"Collar - declared minus assay"
					entries["Collar Dcl-Asy %"] = rcres.dcl_minus_asy_u235_mass_pct.ToString("F2"); //	"Collar - declared minus assay %"
					entries["Collar Status"] = rcres.pass ? "Pass": ""; //	"Collar - measurement status"
				}
				break;
			default:
				break;
			}
			return entries;
		}

		private bool QuoteMe(object o)
		{
			return o.GetType().Equals(typeof(string));
		}

		private object Q(object o)
		{  
			if (QuoteMe(o))
				return '"' + (o as string) + '"';
			else
				return o;
		}

		private string Q(string o)
		{  
				return "\"" + (o as string) + "\"";
		}
        private string GetMainFilePath(ResultFiles files, AssaySelector.MeasurementOption mo, bool elide)
        {
            string res = string.Empty;
            switch (mo)
            {
                case AssaySelector.MeasurementOption.unspecified:
                    res = files.CSVResultsFileName.Path;
                    break;
                default:
                    res = files.PrimaryINCC5Filename.Path;
                    break;
            }
            if (elide)
                res = System.IO.Path.GetFileName(res);
            return res;
        }

        public bool HasAny
        {
            get
            {
                bool res = false;
                foreach (Selections s in Enum.GetValues(typeof(Selections)))
                {
                    string key = s.ToString();
                    if (!Root[key].Enabled)
                        continue;
                    if (Root[key].Entries != null)
                    {
                        res = Root[key].Entries.Count > 0;
                        break;
                    }
                }
                return res;
            }
        }

        public System.Collections.IEnumerator GetEntryEnumerator()
        {
            string key = Selections.AcquireParams.ToString();  // the first enum entry
            if (!Root[key].Enabled)
                yield break;
            if (Root[key].Entries == null || Root[key].Entries.Count < 1)
                yield break;
            for (int i = 0; i < Root[key].Entries.Count; i++)
            {
                yield return FullRow(i);
            }
        }

        string FullRow(int i)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder(143);
            foreach (Selections sel in Enum.GetValues(typeof(Selections)))
            {
                string key = sel.ToString();
                if (!Root[key].Enabled)
                    continue;
                if (Root[key].Entries == null || Root[key].Entries.Count < 1)
                    continue;
                Dictionary<string, string> d = Root[key].Entries[i];
                foreach (string v in d.Values)
                {
                   s.Append(v); s.Append(',');
                }
            }
            if (s.Length > 0) s.Remove(s.Length - 1, 1);
            return s.ToString();

        }
        public string HeaderRow {
            get {
                System.Text.StringBuilder s = new System.Text.StringBuilder(143);
                foreach (Selections sel in Enum.GetValues(typeof(Selections)))
                {
                    string key = sel.ToString();
                    if (!Root[key].Enabled)
                        continue;
                    if (Root[key].Entries == null || Root[key].Entries.Count < 1)
                        continue;
                    Dictionary<string, string> d = Root[key].Entries[0];
                    foreach (string v in d.Keys)
                    {
                        s.Append('"'); s.Append(v); s.Append('"'); s.Append(',');
                    }
                }
                if (s.Length > 0) s.Remove(s.Length - 1, 1);
                return s.ToString();
            }
        }
    }

}
