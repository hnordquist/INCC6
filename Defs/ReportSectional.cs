/*
The MCA-527 INCC integration source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

The MCA-527 INCC integration source code is distributed under the New BSD license:

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
namespace AnalysisDefs
{
	using RS = MethodResultsReport.INCCReportSection;

	public class ReportSectional : ParameterBase
	{

		public bool [] Selections;
		public ReportSectional()
		{
			Array sv = Enum.GetValues(typeof(RS));
			Selections = new bool[sv.Length];
			Selections[(int)RS.Header] = true;
			Selections[(int)RS.Context] = true;
			Selections[(int)RS.Adjustments] = true;
			Selections[(int)RS.Messages] = true;
			Selections[(int)RS.MassResults] = true;
            Selections[(int)RS.CycleSummary] = true;
            Selections[(int)RS.Reference] = true;
            DetectorParameters = true;
			CalibrationParameters = true;
			Isotopics = true;
			RateCycleData = true;
		}

		public ReportSectional(ReportSectional src)
		{
			Selections = new bool[src.Selections.Length];
			Array.Copy(src.Selections, Selections, Selections.Length);
		}

		public bool DetectorParameters
		{
			get { return Selections[(int)RS.ShiftRegister]; }
			set { Selections[(int)RS.ShiftRegister] = value; }
		}
		public bool CalibrationParameters
		{
			get { return Selections[(int)RS.MethodResultsAndParams]; }
			set { Selections[(int)RS.MethodResultsAndParams] = value; }
		}
		public bool Isotopics 
		{
			get { return Selections[(int)RS.Isotopics]; }
			set { Selections[(int)RS.Isotopics] = value; }
		}
		public bool RawCycleData
		{
			get { return Selections[(int)RS.RawCycles]; }
			set { Selections[(int)RS.RawCycles] = value; }
		}
		public bool RateCycleData
		{
			get { return Selections[(int)RS.DTCRateCycles]; }
			set { Selections[(int)RS.DTCRateCycles] = value; }
		}
		public bool SummedRawCoincData
		{
			get { return Selections[(int)RS.SummedRawData]; }
			set { Selections[(int)RS.SummedRawData] = value; }
		}
		public bool SummedMultiplicityDistributions
		{
			get { return Selections[(int)RS.SummedRA]; }
			set { Selections[(int)RS.SummedRA] = value; }
		}
		public bool MultiplicityDistributions
		{
			get { return Selections[(int)RS.MultiplicityDistributions]; }
			set { Selections[(int)RS.MultiplicityDistributions] = value; }
		}
        public bool CycleDataSelected
        {
            get { return RateCycleData || RawCycleData || MultiplicityDistributions; }
        }

        public override void GenParamList()
        {
            base.GenParamList();
            Table = "ReportSections";
            ps.Add(new DBParamEntry("review_detector_parms", DetectorParameters));
            ps.Add(new DBParamEntry("review_calib_parms", CalibrationParameters));
            ps.Add(new DBParamEntry("review_isotopics", Isotopics));
            ps.Add(new DBParamEntry("review_run_raw_data", RawCycleData));
            ps.Add(new DBParamEntry("review_run_rate_data", RateCycleData));
            ps.Add(new DBParamEntry("review_summed_raw_data", SummedRawCoincData));
		    ps.Add(new DBParamEntry("review_summed_mult_dist", SummedMultiplicityDistributions));
		    ps.Add(new DBParamEntry("review_run_mult_dist", MultiplicityDistributions));
		}

	}
}
