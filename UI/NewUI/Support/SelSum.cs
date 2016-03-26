using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewUI
{
	class SelSum
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
			bool Enabled {get; set; }
			Selections Parent {get; set; }
			Selections Me { get; set; }
		}

		public	Dictionary<Selections, State> Root = new Dictionary<Selections, State>();

		public SelSum()
		{
			Root.Add(Selections.Measurement, new State(Selections.Measurement, true, Selections.Measurement));
			Root.Add(Selections.Identifier, new State(Selections.Identifier, true, Selections.Measurement));
			Root.Add(Selections.Detector, new State(Selections.DetId, true, Selections.Measurement));
				Root.Add(Selections.DetParams, new State(Selections.DetId, true, Selections.Detector));
				Root.Add(Selections.DetId, new State(Selections.MassAnalysisMethods, true, Selections.Detector));
			Root.Add(Selections.MassAnalysisMethods, new State(Selections.MassAnalysisMethods, true, Selections.Measurement));
			Root.Add(Selections.MassAndError, new State(Selections.MassAndError, true, Selections.Measurement));
			Root.Add(Selections.AcquireParams, new State(Selections.AcquireParams, true, Selections.Measurement));
			Root.Add(Selections.BackgroundParams, new State(Selections.BackgroundParams, true, Selections.Measurement));
			Root.Add(Selections.Isotopics, new State(Selections.Isotopics, true, Selections.Measurement));
			Root.Add(Selections.NormalizationParams, new State(Selections.NormalizationParams, true, Selections.Measurement));
			Root.Add(Selections.TestParams, new State(Selections.TestParams, true, Selections.Measurement));
			Root.Add(Selections.Stratum, new State(Selections.Stratum, true, Selections.Measurement));
			Root.Add(Selections.Summaries, new State(Selections.Summaries, true, Selections.Measurement));
			Root.Add(Selections.ResultsFiles, new State(Selections.ResultsFiles, true, Selections.Measurement));
			Root.Add(Selections.MassAnalysisMethods, new State(Selections.MassAnalysisMethods, true, Selections.Measurement));
				Root.Add(Selections.CalibrationCurve, new State(Selections.CalibrationCurve, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.KnownAlpha, new State(Selections.KnownAlpha, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.KnownM, new State(Selections.KnownM, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.Multiplicity, new State(Selections.Multiplicity, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.AddASource, new State(Selections.AddASource, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.CuriumRatio, new State(Selections.CuriumRatio, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.TruncatedMultiplicity, new State(Selections.TruncatedMultiplicity, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.ActiveCalibCurve, new State(Selections.ActiveCalibCurve, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.Collar, new State(Selections.Collar, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.ActiveMultiplicity, new State(Selections.ActiveMultiplicity, true, Selections.MassAnalysisMethods));
				Root.Add(Selections.ActivePassive, new State(Selections.ActivePassive, true, Selections.MassAnalysisMethods));
			Root.Add(Selections.LMResults, new State(Selections.LMResults, true, Selections.Measurement));
		}
	}

}
