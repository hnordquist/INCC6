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
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnalysisDefs;
namespace NewUI
{
	public partial class PlotSDTChart : Form
	{
		public PlotSDTChart(Measurement m)
		{
			InitializeComponent();
			string id =  m.MeasOption.PrintName() + " Measurement " + m.MeasDate.ToString("yy.MM.dd HH:mm:ss K");
            Text += " for " + id;
			if (!m.ResultsFiles.PrimaryINCC5Filename.IsNullOrEmpty)
				id = id + "  File: " + System.IO.Path.GetFileName(m.ResultsFiles.PrimaryINCC5Filename.Path);
			Triples.Titles[1].Text = id;

			// get the cycles for the selected measurement from the database, add them to the measurement, then recalculate the cycle SDTs
            m.ReportRecalc();
            PlotThePlots(m.Cycles, m.Detector.MultiplicityParams);
		}

		void PlotThePlots(CycleList cl, Multiplicity mkey)
		{			
			Series s1 = Singles.Series["Vals"];
			s1.MarkerStyle = MarkerStyle.Circle;
			s1.MarkerColor = System.Drawing.Color.SkyBlue;
			s1.MarkerSize = 5;
			s1.MarkerBorderColor = System.Drawing.Color.DarkCyan;
			s1.Color = System.Drawing.Color.MediumPurple;

			Series s2 = Doubles.Series["Vals"];
			s2.MarkerStyle = MarkerStyle.Circle;
			s2.MarkerColor = System.Drawing.Color.SkyBlue;
			s2.MarkerSize = 5;
			s2.MarkerBorderColor = System.Drawing.Color.DarkCyan;
			s2.Color = System.Drawing.Color.MediumPurple;

			Series s3 = Triples.Series["Vals"];
			s3.MarkerStyle = MarkerStyle.Circle;
			s3.MarkerColor = System.Drawing.Color.SkyBlue;
			s3.MarkerSize = 5;
			s3.MarkerBorderColor = System.Drawing.Color.DarkCyan;
			s3.Color = System.Drawing.Color.MediumPurple;

			int i = 0;
			foreach (Cycle c in cl)
			{
				i++;
				MultiplicityCountingRes mcr = c.MultiplicityResults(mkey);
                bool somtingfunnyhere = (c.QCStatus(mkey).status != QCTestStatus.Pass);

                int idx = s1.Points.AddXY(i, mcr.DeadtimeCorrectedSinglesRate.v);  // next: vary color and shape based on cycle status/outlier status
				s2.Points.AddXY(i, mcr.DeadtimeCorrectedDoublesRate.v);
				s3.Points.AddXY(i, mcr.DeadtimeCorrectedTriplesRate.v);
                if (somtingfunnyhere)
                {
                    s1.Points[idx].MarkerColor = System.Drawing.Color.Orchid;
                    s2.Points[idx].MarkerColor = System.Drawing.Color.Orchid;
                    s3.Points[idx].MarkerColor = System.Drawing.Color.Orchid;
                    s1.Points[idx].ToolTip = string.Format("#{0} {1}", c.seq, c.QCStatus(mkey).INCCString());
                    s3.Points[idx].ToolTip = s2.Points[idx].ToolTip = s1.Points[idx].ToolTip;
                }

            }
		}


		private void OKBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
