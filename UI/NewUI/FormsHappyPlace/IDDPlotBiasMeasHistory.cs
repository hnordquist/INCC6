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
using AnalysisDefs;

namespace NewUI
{
	using System.Windows.Forms.DataVisualization.Charting;
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
				NormDoubles nd = new NormDoubles(res.acq.MeasDateTime);
				if (norm.biasMode == NormTest.Cf252Doubles)
				{
					nd.Doubles = res.mcr.DeadtimeCorrectedDoublesRate.v;
					nd.DoublesPlusErr = res.mcr.DeadtimeCorrectedDoublesRate.v + res.mcr.DeadtimeCorrectedDoublesRate.err;
					nd.DoublesMinusErr = res.mcr.DeadtimeCorrectedDoublesRate.v - res.mcr.DeadtimeCorrectedDoublesRate.err;
					normlist.Add(nd);
				}
				else
				{
					nd.Doubles = res.mcr.DeadtimeCorrectedSinglesRate.v;
					nd.DoublesPlusErr = res.mcr.DeadtimeCorrectedSinglesRate.v + res.mcr.DeadtimeCorrectedSinglesRate.err;
					nd.DoublesMinusErr = res.mcr.DeadtimeCorrectedSinglesRate.v - res.mcr.DeadtimeCorrectedSinglesRate.err;
				}
				if (nd.DoublesPlusErr > normlist.MaxDoubles)
				{
					normlist.MaxDoubles = nd.DoublesPlusErr;
				}
				if (nd.DoublesMinusErr < normlist.MinDoubles)
				{
					normlist.MinDoubles = nd.DoublesMinusErr;
				}
				nd.number = ++num;
			}
			normlist.CalcLowerUpper();

			// Set error bar upper & lower error style
			chart1.Series["Vals"]["ErrorBarStyle"] = "Both";

			// Set error bar center marker style
				chart1.Series["Vals"]["PointWidth"] = "7.5";
			//	chart1.Series["Vals"].MarkerStyle = MarkerStyle.Cross;
			//// Set error bar marker style
			//chart1.Series["Errors"].MarkerStyle = MarkerStyle.Circle;
			//chart1.Series["Vals"].MarkerStyle = MarkerStyle.Circle;
			Series s = chart1.Series["Vals"];
			foreach(NormDoubles n in normlist)
			{
				int i = s.Points.AddXY(n.number, n.Doubles, n.DoublesMinusErr, n.DoublesPlusErr);
				s.Points[i].ToolTip = n.ToString();
			}
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    class NormDoubles
    {
        public double Doubles;
        public double DoublesPlusErr;
        public double DoublesMinusErr;
        public int number;
        public DateTimeOffset dto;

        internal NormDoubles(DateTimeOffset _dto)
        {
            dto = _dto;
        }

        public override string ToString()
        {
            return string.Format("Doubles: {0:F5} -{1:F5} +{2:F5} {3}", Doubles, Doubles - DoublesMinusErr, DoublesPlusErr - Doubles, dto.ToString("yyyy-MM-dd HH:mm:ss")); 
        }
    }

    class NormList : List<NormDoubles>
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

        public double[] DoublesAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((NormDoubles)iter.Current).Doubles;
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
                NormDoubles dl = (NormDoubles)iter.Current;
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
