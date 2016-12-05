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
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnalysisDefs;
namespace NewUI
{
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;
	public partial class IDDPlotBiasMeasHistory : Form
    {
        private Detector det;
        private AcquireParameters ap;
        NormParameters norm;
        NormList normlist;
        public List<INCCDB.IndexedResults> irlist;

        public IDDPlotBiasMeasHistory()
        {
            InitializeComponent();
            Integ.GetCurrentAcquireDetectorPair(ref ap, ref det);
            Text += " for detector " + det.Id.DetectorName;
            norm = N.App.DB.NormParameters.Get(det);
            normlist = new NormList();
            if ((norm.biasMode == NormTest.AmLiSingles) || (norm.biasMode == NormTest.Collar))
            {
                normlist.RefDoubles[0] = normlist.RefDoubles[1] = norm.amliRefSinglesRate;
                normlist.RefDoublesPlusErr[0] = normlist.RefDoublesPlusErr[1] = norm.amliRefSinglesRate;
                normlist.RefDoublesMinusErr[0] = normlist.RefDoublesMinusErr[1] = norm.amliRefSinglesRate;
            }
            else
            {
                normlist.RefDoubles[0] = normlist.RefDoubles[1] = norm.cf252RefDoublesRate.v;
                normlist.RefDoublesPlusErr[0] = normlist.RefDoublesPlusErr[1] = norm.cf252RefDoublesRate.v + norm.cf252RefDoublesRate.err;
                normlist.RefDoublesMinusErr[0] = normlist.RefDoublesMinusErr[1] = norm.cf252RefDoublesRate.v - norm.cf252RefDoublesRate.err;
            }
            irlist = N.App.DB.IndexedResultsFor(det.Id.DetectorId, "normalization", "All");
			if (irlist.Count < 1)
				return;
            irlist.Sort((r1, r2) => // sort chronologically
            {
                return DateTimeOffset.Compare(r1.DateTime, r2.DateTime);
            });

            if (norm.biasMode != NormTest.Cf252Doubles)
				chart1.Titles[1].Text = "Singles Rate";

			// get the mult results with the singles/doubles
            normlist.RefNumber = irlist.Count;
			List<INCCResults.results_rec> mlist = new List<INCCResults.results_rec>();
			foreach(INCCDB.IndexedResults ir in irlist)
				mlist.Add(N.App.DB.ResultsRecFor(ir.Mid));

            // now create the list of norm points
			int num = 0;
			foreach(INCCResults.results_rec res in mlist)
			{
				NormValues nv = new NormValues(res.acq.MeasDateTime);
				nv.test = norm.biasMode;
				if (norm.biasMode == NormTest.Cf252Doubles)
				{
					nv.Doubles = res.mcr.DeadtimeCorrectedDoublesRate.v;
					nv.DoublesPlusErr = res.mcr.DeadtimeCorrectedDoublesRate.v + res.mcr.DeadtimeCorrectedDoublesRate.err;
					nv.DoublesMinusErr = res.mcr.DeadtimeCorrectedDoublesRate.v - res.mcr.DeadtimeCorrectedDoublesRate.err;
					normlist.Add(nv);
				}
				else
				{
					nv.Doubles = res.mcr.DeadtimeCorrectedSinglesRate.v;
					nv.DoublesPlusErr = res.mcr.DeadtimeCorrectedSinglesRate.v + res.mcr.DeadtimeCorrectedSinglesRate.err;
					nv.DoublesMinusErr = res.mcr.DeadtimeCorrectedSinglesRate.v - res.mcr.DeadtimeCorrectedSinglesRate.err;
					normlist.Add(nv);
				}
				if (nv.DoublesPlusErr > normlist.MaxDoubles)
				{
					normlist.MaxDoubles = nv.DoublesPlusErr;
				}
				if (nv.DoublesMinusErr < normlist.MinDoubles)
				{
					normlist.MinDoubles = nv.DoublesMinusErr;
				}
				nv.number = ++num;
			}
			normlist.CalcLowerUpper();

			Series s = chart1.Series["Vals"];

			// Set error bar upper & lower error style
			s["ErrorBarStyle"] = "Both";
			s["ErrorBarCenterMarkerStyle"] = "Circle";

			// Set error bar center marker style
			s.MarkerStyle = MarkerStyle.None;
			s.MarkerColor = System.Drawing.Color.DarkViolet;

            int imax = 0;
            ArrowAnnotation maxpt = new ArrowAnnotation();
            maxpt.Name = "max";
            maxpt.Height = -5;
            maxpt.Width = 0;
            maxpt.AnchorOffsetY = -2.5;
			foreach(NormValues n in normlist)
			{
				int i = s.Points.AddXY(n.number, n.Doubles, n.DoublesMinusErr, n.DoublesPlusErr);
                if (n.DoublesPlusErr == normlist.MaxDoubles)   
                {
                    imax = i;
                    maxpt.ToolTip = "Max " + n.ToString();
                }
                s.Points[i].ToolTip = n.ToString();
			}      
            chart1.Annotations.Add(maxpt);
			if (s.Points.Count > 0)
 				maxpt.AnchorDataPoint = s.Points[imax];
       }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    class NormValues
    {
        public double Doubles;
        public double DoublesPlusErr;
        public double DoublesMinusErr;
        public int number;
        public DateTimeOffset dto;
		public NormTest test;

        internal NormValues(DateTimeOffset _dto)
        {
            dto = _dto;
			test = NormTest.Cf252Doubles;
        }

        public override string ToString()
        {
			string s = "Doubles";
			if (test != NormTest.Cf252Doubles)
				s = "Singles";
			return string.Format(s + ": {0:F5} -{1:F5} +{2:F5},   {3}", Doubles, Doubles - DoublesMinusErr, DoublesPlusErr - Doubles, dto.ToString("yyyy-MM-dd HH:mm:ss")); 
        }
    }

    class NormList : List<NormValues>
    {
        public NormList()
        {
            RefDoubles = new double[2];
            RefDoublesPlusErr = new double[2];
            RefDoublesMinusErr = new double[2];
            RefNumber = 0;
			MinDoubles = 0;
			MaxDoubles = 1000000;
        }
        public double MinDoubles, MaxDoubles;
        public double[] RefDoubles;
        public double[] RefDoublesPlusErr;
        public double[] RefDoublesMinusErr;
        public int RefNumber;

        public double[] ValuesAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((NormValues)iter.Current).Doubles;
                    i++;
                }
                return a;
            }
        }


        public void CalcLowerUpper()
        {
            double lower = 0, upper = 0;
            System.Collections.IEnumerator iter = GetEnumerator();
			bool first = true;
            while (iter.MoveNext())
            {
                NormValues dl = (NormValues)iter.Current;
				if (first)
				{
					lower = dl.DoublesMinusErr;
					upper = dl.DoublesPlusErr;
					first = false;
				}
                if (dl.DoublesPlusErr > upper)
                    upper = dl.DoublesPlusErr;
                if (dl.DoublesMinusErr < lower)
                    lower = dl.DoublesMinusErr;
            }
            MinDoubles = lower;
            MaxDoubles = upper;
        }
    }

}
