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
namespace NewUI
{
	public partial class PlotAssayChart : Form
	{

		public PlotAssayChart(MeasPointList MeasPoints, CalibList CalibPoints)
		{
			InitializeComponent();
			chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			PlotVerificationPoints(MeasPoints);
			PlotCalibCurveLine(CalibPoints);
		}

		void PlotCalibCurveLine(CalibList CalibPoints)
		{
			Series s = chart1.Series["Curve"];
			//s.MarkerStyle = MarkerStyle.Square;
			s.MarkerColor = System.Drawing.Color.NavajoWhite;
			s.MarkerBorderColor = System.Drawing.Color.DarkCyan;
			s.Color = System.Drawing.Color.Cyan;
			int imax = 0;
			foreach (CalibData p in CalibPoints)
			{
				if (p.CalCurvDoubles == 0)
					continue;
				int i = s.Points.AddXY(p.CalCurvMass, p.CalCurvDoubles);
				if (p.CalCurvMass == CalibPoints.MaxCalCurvMass)
				{
					imax = i;
					//maxpt.ToolTip = "Max " + p.ToString();
				}
				s.Points[i].ToolTip = p.CurveRep;
			}
			//        chart1.Annotations.Add(maxpt);
			//maxpt.AnchorDataPoint = s.Points[imax];

			s = chart1.Series["Calib"];
			s.MarkerStyle = MarkerStyle.Triangle;
			s.MarkerColor = System.Drawing.Color.DarkCyan;
			s.MarkerSize = 10;
			foreach (CalibData p in CalibPoints)
			{
				int i = s.Points.AddXY(p.CalPtsMass, p.CalPtsDoubles);
				if (p.CalPtsMass == CalibPoints.MaxCalPtsDoubles)
				{
					imax = i;
					//maxpt.ToolTip = "Max " + p.ToString();
				}
				s.Points[i].ToolTip = p.PointRep;
			}
			//        chart1.Annotations.Add(maxpt);
			//maxpt.AnchorDataPoint = s.Points[imax]
		}


		void PlotVerificationPoints(MeasPointList MeasPoints)
		{
			Series s = chart1.Series["Verif"];
			s.MarkerStyle = MarkerStyle.Square;
			// Set error bar center marker style
			s.MarkerStyle = MarkerStyle.None;
			s.MarkerColor = System.Drawing.Color.Fuchsia;
			chart1.ChartAreas[0].AxisX.Minimum = MeasPoints.LowerMass * 0.9;
			chart1.ChartAreas[0].AxisX.Maximum = MeasPoints.UpperMass * 1.1;
			int imax = 0;
			ArrowAnnotation maxpt = new ArrowAnnotation();
			maxpt.Name = "max";
			maxpt.Height = -4;
			maxpt.Width = 0;
			maxpt.AnchorOffsetY = -2.5;
			maxpt.ResizeToContent();
			foreach (MeasPointData p in MeasPoints)
			{
				int i = s.Points.AddXY(p.Mass, p.Doubles);
				if (p.Mass == MeasPoints.UpperMass)
				{
					imax = i;
					maxpt.ToolTip = "Max " + p.ToString();
				}
				s.Points[i].ToolTip = p.ToString();
			}
			chart1.Annotations.Add(maxpt);
			maxpt.AnchorDataPoint = s.Points[imax];
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


	}
}
