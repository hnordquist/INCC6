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
            Measurement,
            Identifier,
            AcquireParams,
            Detector,
                DetId,
                DetParams,
            MassAndError,
            Summaries,
            BackgroundParams,
            Isotopics,
            NormalizationParams,
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
            public State(Selections m, bool b, Selections s)
            {
                Enabled = b; Parent = s; Entries = new List<Dictionary<string, string>>() { };
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
            Root.Add(Selections.Measurement.ToString(), new State(Selections.Measurement, true, Selections.Measurement));
            Root.Add(Selections.Identifier.ToString(), new State(Selections.Identifier, true, Selections.Measurement));
            Root.Add(Selections.Detector.ToString(), new State(Selections.DetId, true, Selections.Measurement));
            Root.Add(Selections.DetParams.ToString(), new State(Selections.DetId, false, Selections.Detector));
            Root.Add(Selections.DetId.ToString(), new State(Selections.MassAnalysisMethods, true, Selections.Detector));
            Root.Add(Selections.MassAndError.ToString(), new State(Selections.MassAndError, true, Selections.Measurement));
            Root.Add(Selections.AcquireParams.ToString(), new State(Selections.AcquireParams, false, Selections.Measurement));
            Root.Add(Selections.BackgroundParams.ToString(), new State(Selections.BackgroundParams, false, Selections.Measurement));
            Root.Add(Selections.Isotopics.ToString(), new State(Selections.Isotopics, false, Selections.Measurement));
            Root.Add(Selections.NormalizationParams.ToString(), new State(Selections.NormalizationParams, false, Selections.Measurement));
            Root.Add(Selections.TestParams.ToString(), new State(Selections.TestParams, false, Selections.Measurement));
            Root.Add(Selections.Stratum.ToString(), new State(Selections.Stratum, false, Selections.Measurement));
            Root.Add(Selections.Summaries.ToString(), new State(Selections.Summaries, true, Selections.Measurement));
            Root.Add(Selections.ResultsFiles.ToString(), new State(Selections.ResultsFiles, true, Selections.Measurement));
            Root.Add(Selections.MassAnalysisMethods.ToString(), new State(Selections.MassAnalysisMethods, false, Selections.Measurement));
            Root.Add(Selections.CalibrationCurve.ToString(), new State(Selections.CalibrationCurve, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.KnownAlpha.ToString(), new State(Selections.KnownAlpha, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.KnownM.ToString(), new State(Selections.KnownM, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.Multiplicity.ToString(), new State(Selections.Multiplicity, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.AddASource.ToString(), new State(Selections.AddASource, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.CuriumRatio.ToString(), new State(Selections.CuriumRatio, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.TruncatedMultiplicity.ToString(), new State(Selections.TruncatedMultiplicity, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.ActiveCalibCurve.ToString(), new State(Selections.ActiveCalibCurve, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.Collar.ToString(), new State(Selections.Collar, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.ActiveMultiplicity.ToString(), new State(Selections.ActiveMultiplicity, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.ActivePassive.ToString(), new State(Selections.ActivePassive, false, Selections.MassAnalysisMethods));
            Root.Add(Selections.LMResults.ToString(), new State(Selections.LMResults, false, Selections.Measurement));
         }

        public void Apply(Measurement m)
        {
            foreach (Selections s in System.Enum.GetValues(typeof(Selections)))
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

        // example summary section options, here I force all to be enabled, and use only a single header line.
        // INCC5 enables subselecting each field and creates a two line header instead of one.
        Dictionary<string, string> GenerateEntries(Selections s, Measurement m)
        {
            Dictionary<string, string> entries = new Dictionary<string, string>();
            switch (s)
            {
                case Selections.Identifier:
                    entries["Facility"] = m.AcquireState.facility.Name;
                    entries["MBA"] = m.AcquireState.mba.Name;
                    entries["Detector"] = m.AcquireState.detector_id;
                    entries["IC"] = m.AcquireState.inventory_change_code;
                    entries["IO"] = m.AcquireState.io_code;
                    entries["Measurement Type"] = m.MeasOption.PrintName();
                    entries["Meas Date"] = m.MeasurementId.MeasDateTime.ToString("MM.dd.yy");
                    entries["Meas Time"] = m.MeasurementId.MeasDateTime.ToString("HH:mm:ss");
                    entries["File Name"] = GetMainFilePath(m.ResultsFiles, m.MeasOption, true);
                    entries["Inspector"] = m.AcquireState.user_id;
                    entries["Inspection Number"] = m.AcquireState.campaign_id;
                    entries["Item ID"] = m.AcquireState.item_id;
                    entries["Isotopics ID"] = m.AcquireState.isotopics_id;
                    entries["Stratum ID"] = m.AcquireState.stratum_id.Name;
                    entries["Material Type"] = m.AcquireState.item_type;
                    break;
                default:
                    break;
            }
            return entries;
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
                foreach (Selections s in System.Enum.GetValues(typeof(Selections)))
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
            string key = Selections.Identifier.ToString();
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
            foreach (Selections sel in System.Enum.GetValues(typeof(Selections)))
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
                foreach (Selections sel in System.Enum.GetValues(typeof(Selections)))
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
