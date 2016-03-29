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
                Collar,
                ActiveMultiplicity,
                ActivePassive,
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
            Root.Add(Selections.ResultsFiles.ToString(), new State(Selections.ResultsFiles, true));
            Root.Add(Selections.MassAnalysisMethods.ToString(), new State(Selections.MassAnalysisMethods, false));
            Root.Add(Selections.CalibrationCurve.ToString(), new State(Selections.CalibrationCurve, false));
            Root.Add(Selections.KnownAlpha.ToString(), new State(Selections.KnownAlpha, false));
            Root.Add(Selections.KnownM.ToString(), new State(Selections.KnownM, false));
            Root.Add(Selections.Multiplicity.ToString(), new State(Selections.Multiplicity, false));
            Root.Add(Selections.AddASource.ToString(), new State(Selections.AddASource, false));
            Root.Add(Selections.CuriumRatio.ToString(), new State(Selections.CuriumRatio, false));
            Root.Add(Selections.TruncatedMultiplicity.ToString(), new State(Selections.TruncatedMultiplicity, false));
            Root.Add(Selections.ActiveCalibCurve.ToString(), new State(Selections.ActiveCalibCurve, false));
            Root.Add(Selections.Collar.ToString(), new State(Selections.Collar, false));
            Root.Add(Selections.ActiveMultiplicity.ToString(), new State(Selections.ActiveMultiplicity, false));
            Root.Add(Selections.ActivePassive.ToString(), new State(Selections.ActivePassive, false));
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
            foreach (Selections s in System.Enum.GetValues(typeof(Selections)))
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
				entries["IC"] = Q(m.AcquireState.inventory_change_code);  // URGENT: not loading
				entries["IO"] = Q(m.AcquireState.io_code);                              // ditto
				entries["Measurement Type"] = Q(m.MeasOption.PrintName());
				entries["Meas Date"] = m.MeasurementId.MeasDateTime.ToString("MM.dd.yy");
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
				if (m.INCCAnalysisState != null && m.INCCAnalysisResults != null)  // URGENT: this is not constructed upon a measurement read from the DB yet
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
							entries["Scaler 2"] = ir.Scaler1.v.ToString("F1");
							entries["Scaler 2 Error"] = ir.Scaler2.err.ToString("F3");
						}
						entries["Count Time"] = (m.Cycles.Count > 0 ? m.Cycles[0].TS.TotalSeconds.ToString("F0") : "0"); // "Total count time for a measurement"					
						entries["Singles Sum"] = m.SinglesSum.ToString("F0"); // Sums - singles  Singles sum for a measurement"
						entries["R+A Sum"] = ir.RASum.ToString("F0");    // Reals + accidentals sum for a measurement"
						entries["A Sum"] = ir.ASum.ToString("F0");  // "Accidentals sum for a measurement"
					}
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
