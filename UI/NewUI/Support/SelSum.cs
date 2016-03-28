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
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
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
using System;
using System.Collections.Generic;

namespace NewUI
{
    public class ResultsSummary
	{
		public enum Selections
		{ Measurement,
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

		public class State {
			public State(Selections m, bool b, Selections s) {Enabled = b; Parent = s;}
            public bool Enabled {get; set; }
            public Selections Parent {get; set; }
            public Selections Me { get; set; }   
		}

		public	Dictionary<string, State> Root = new Dictionary<string, State>();
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
	}

}
