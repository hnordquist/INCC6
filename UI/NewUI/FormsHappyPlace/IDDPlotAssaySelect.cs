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
using AnalysisDefs;
using NCCReporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace NewUI
{
    using N = NCC.CentralizedState;
    using Integ = NCC.IntegrationHelpers;

    public partial class IDDPlotAssaySelect : Form
    {
        public IDDPlotAssaySelect()
        {
            InitializeComponent();
            InitSort();
        }

        private List<Measurement> mlist;
        SortOrder[] cols;
        public bool bGood;
        public string Material;
        public AnalysisMethod AnalysisMethod;
        public INCCAnalysisParams.CurveEquation CurveEquation;
        public MeasPointList MeasDataList;
        public CalibList CalibDataList;
        public AssaySelector.MeasurementOption mopt;

        private string DetectorId;

        public void Init(string id, AssaySelector.MeasurementOption mo = AssaySelector.MeasurementOption.verification)
        {
            MeasDataList = new MeasPointList();
            CalibDataList = new CalibList();
            DetectorId = id;
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                InitSort();
                mopt = mo;
                mlist = N.App.DB.MeasurementsFor(DetectorId, mopt);
                bGood = PrepList(DetectorId);
            }
            finally
            {
                System.Windows.Input.Mouse.OverrideCursor = null;
            }
            if (AnalysisMethod == AnalysisMethod.Active)
            {
                listView1.Columns[2].Text = "Decl U235 Mass";
            }
            if (AnalysisMethod == AnalysisMethod.KnownA)
            {
                listView1.Columns[3].Text = "Mult Corr Dbls Rt";
            }
        }

        void InitSort()
        {
            cols = new SortOrder[listView1.Columns.Count];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = SortOrder.Descending;
            cols[1] = SortOrder.Ascending;  // datetime column
        }

        bool MatchingSwipeLeft(Measurement m)
        {
            return !(m.INCCAnalysisState.Methods.HasMethod(AnalysisMethod) &&
                     (string.Compare(m.INCCAnalysisState.Methods.selector.material, Material, true) == 0));
        }

        bool PrepList(string DetectorId)
        {

            mlist.RemoveAll(MatchingSwipeLeft);    // cull those without matching attributes
            if (mlist.Count == 0)
            {
                string msg = string.Format("No '{0}' {1} {2} measurements found", Material, AnalysisMethod.FullName(), mopt.PrintName());
                MessageBox.Show(msg, "Verification measurements", MessageBoxButtons.OK);
                N.App.ControlLogger.TraceEvent(LogLevels.Warning, 3363, msg);
                return false;
            }
            LoadList();
            return true;
        }

        void LoadList()
        {
            listView1.ShowItemToolTips = true;
            int mlistIndex = 0;
            foreach (Measurement m in mlist)
            {
                MeasPointData p = GetTheDataPoints(m); p.number = mlistIndex+1;
                string ItemWithNumber = string.IsNullOrEmpty(m.MeasurementId.Item.item) ? "-" : m.AcquireState.ItemId.item;
                ListViewItem lvi = new ListViewItem(new string[] {
                    ItemWithNumber,
                    m.MeasDate.DateTime.ToString("yy.MM.dd  HH:mm:ss"),
                    p.Mass.ToString("F2"),
                    p.Doubles.ToString("F2"),
                    mlistIndex.ToString(),  // subitem at index 4 has the original mlist index of this element
					});
                listView1.Items.Add(lvi);
                lvi.Tag = p;  // for proper column sorting
                lvi.ToolTipText = "Using " + m.INCCAnalysisState.Methods.selector.ToString();
                mlistIndex++;
            }
            MCount.Text = listView1.Items.Count.ToString() + " measurements";
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString();
            else
                MCountSel.Text = string.Empty;
        }

        public MeasPointData GetTheDataPoints(Measurement m)
        {
            MeasPointData p = new MeasPointData(m.MeasDate);
            MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[m.Detector.MultiplicityParams];
            INCCMethodResult imr = m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod, create: false);
            switch (AnalysisMethod)
            {
                case AnalysisMethod.CalibrationCurve:
                    INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)imr;
                    p.Doubles= mcr.DeadtimeCorrectedDoublesRate.v;
                    p.Mass = ccres.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.KnownA:
                    INCCMethodResults.results_known_alpha_rec kares = (INCCMethodResults.results_known_alpha_rec)imr;
                    p.Doubles = (kares.mult_corr_doubles.v == 0 ? mcr.DeadtimeCorrectedDoublesRate.v : kares.mult_corr_doubles.v);
                    p.Mass = kares.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.AddASource:
                    INCCMethodResults.results_add_a_source_rec aares = (INCCMethodResults.results_add_a_source_rec)imr;
                    p.Doubles = mcr.DeadtimeCorrectedDoublesRate.v;
                    p.Mass = aares.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.Active:
                    INCCMethodResults.results_active_rec acres = (INCCMethodResults.results_active_rec)imr;
                    p.Doubles = mcr.DeadtimeCorrectedDoublesRate.v;
                    p.Mass = acres.dcl_u235_mass;
                    break;
            }
            return p;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            MeasPointList list = new MeasPointList();
			int num = 0;
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    list.Add((MeasPointData)lvi.Tag);
                    lvi.Selected = false;
					((MeasPointData)lvi.Tag).number = ++num;
                }
            }
            PrepareMeasDataList(list);
			PrepareCalibList();
            Close();
        }

        void PrepareMeasDataList(MeasPointList list)
        {
            if (list.Count < 1)
                return;
			MeasDataList.am = AnalysisMethod;
			MeasDataList = list;
            MeasDataList.CalcLowerUpper();          // compute upper and lower mass from the data points
        }

		void PrepareCalibList()
		{
			CalibDataList.Clear();
			CalibDataList.am = AnalysisMethod;
			AnalysisMethods ams = Integ.GetMethodSelections(DetectorId, Material); 
			INCCAnalysisParams.INCCMethodDescriptor imd = ams.GetMethodParameters(AnalysisMethod);
			INCCAnalysisParams.CurveEquationVals cev = null;
			double[] doubles = null; double[] decl_mass = null; 
			GetCalCurve(imd, ref cev, ref doubles, ref decl_mass);

			for (int i = 0; i < doubles.Length; i++)
			{
				CalibData p = new CalibData();
				p.CalPtsMass = decl_mass[i];
				p.CalPtsDoubles = doubles[i];
				p.number = i+1;
				CalibDataList.Add(p);
			}
			CalibDataList.CalcLowerUpper();
			CalibDataList.CalculateMassBasis(cev);
			ApplyMethodCurve(CalibDataList, cev);
		}

		void GetCalCurve(INCCAnalysisParams.INCCMethodDescriptor imd,
						ref INCCAnalysisParams.CurveEquationVals cev, ref double[] doubles, ref double[] decl_mass)
		{
		    switch (AnalysisMethod)
            {
                case AnalysisMethod.CalibrationCurve:
                    INCCAnalysisParams.cal_curve_rec cc = (INCCAnalysisParams.cal_curve_rec)imd;
					doubles = cc.doubles;
					decl_mass = cc.dcl_mass;
					cev = cc.cev;
                    break;
                case AnalysisMethod.KnownA:
                    INCCAnalysisParams.known_alpha_rec ka = (INCCAnalysisParams.known_alpha_rec)imd;
					doubles = ka.doubles;
					decl_mass = ka.dcl_mass;
					cev = ka.cev;
                    break;
                case AnalysisMethod.AddASource:
                    INCCAnalysisParams.add_a_source_rec aa = (INCCAnalysisParams.add_a_source_rec)imd;
					doubles = aa.doubles;
					decl_mass = aa.dcl_mass;
 					cev = aa.cev;
                    break;
                case AnalysisMethod.Active:
                    INCCAnalysisParams.active_rec ac = (INCCAnalysisParams.active_rec)imd;
					doubles = ac.doubles;
					decl_mass = ac.dcl_mass;
 					cev = ac.cev;
                    break;
			}
			if (doubles == null)
				doubles = new double[0];
			if (decl_mass == null)
				decl_mass = new double[0];
			if (cev == null)
				cev = new INCCAnalysisParams.CurveEquationVals();
		}

		void ApplyMethodCurve(CalibList list, INCCAnalysisParams.CurveEquationVals cev)
		{

			for (int i = 0; i < list.NumCalibPoints; i++)
			{
				if (AnalysisMethod == AnalysisMethod.KnownA)
					list[i].CalCurvDoubles = cev.a +
						cev.b * list[i].CalCurvMass;
				else
					switch (cev.cal_curve_equation)
					{
					case INCCAnalysisParams.CurveEquation.CUBIC:
						list[i].CalCurvDoubles = cev.a +
							cev.b * list[i].CalCurvMass +
							cev.c * list[i].CalCurvMass * list[i].CalCurvMass +
							cev.d * list[i].CalCurvMass * list[i].CalCurvMass * list[i].CalCurvMass;
						break;
					case INCCAnalysisParams.CurveEquation.POWER:
						list[i].CalCurvDoubles = cev.a * Math.Pow(list[i].CalCurvMass, cev.b);
						break;
					case INCCAnalysisParams.CurveEquation.HOWARDS:
						list[i].CalCurvDoubles = (cev.a * list[i].CalCurvMass) / (1.0 + cev.b * list[i].CalCurvMass);
						break;
					case INCCAnalysisParams.CurveEquation.EXPONENTIAL:
						list[i].CalCurvDoubles = cev.a * (1.0 - Math.Exp(-cev.b * list[i].CalCurvMass));
						break;
					}
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        public void ListItemSorter(object sender, ColumnClickEventArgs e)
        {
            ListView list = (ListView)sender;
            int total = list.Items.Count;
            list.BeginUpdate();
            ListViewItem[] items = new ListViewItem[total];
            for (int i = 0; i < total; i++)
            {
                int count = list.Items.Count;
                int minIdx = 0;
                for (int j = 1; j < count; j++)
                {
                    bool test = MeasListColumnCompare(list.Items[j], list.Items[minIdx], e.Column);
                    if ((cols[e.Column] == SortOrder.Ascending && test) ||
                        (cols[e.Column] == SortOrder.Descending && !test))
                        minIdx = j;
                }

                items[i] = list.Items[minIdx];
                list.Items.RemoveAt(minIdx);
            }
            list.Items.AddRange(items);
            list.EndUpdate();
            if (cols[e.Column] == SortOrder.Descending)
                cols[e.Column] = SortOrder.Ascending;
            else if (cols[e.Column] == SortOrder.Ascending)
                cols[e.Column] = SortOrder.Descending;
        }

        bool MeasListColumnCompare(ListViewItem a, ListViewItem b, int column)
        {
            bool res = false;
            if (column == 1) // item id
                res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
            else if (column == 2)  // 240 pu mass
                res = (((MeasPointData)a.Tag).Mass.CompareTo(((MeasPointData)b.Tag).Mass)) < 0;
            else if (column == 3)  // doubles
                res = (((MeasPointData)a.Tag).Doubles.CompareTo(((MeasPointData)b.Tag).Doubles)) < 0;
            else if (column == 4) // 4 is the datetime column, such fragile coding
                res = (((MeasPointData)a.Tag).dto.CompareTo(((MeasPointData)b.Tag).dto)) < 0;
            return res;
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString() + " selected";
            else
                MCountSel.Text = string.Empty;
        }

        private void listView1_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            Cursor sav = listView1.Cursor;
            listView1.Cursor = Cursors.WaitCursor;
            ListItemSorter(sender, e);
            listView1.Cursor = sav;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CalibrationCurveList list = new CalibrationCurveList();
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    list.Add((DoublesDclMass)lvi.Tag);
                    lvi.Selected = false;
                }
            }
            if (list.Count < 1)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV files (.csv)|*.csv|                 (.txt)| *.txt";
            dlg.DefaultExt = ".csv";
            dlg.FileName = Material + AnalysisMethod.FullName() + ".csv";
            dlg.InitialDirectory = N.App.AppContext.ResultsFilePath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter w = new StreamWriter(dlg.FileName))
                {
                    foreach (DoublesDclMass x in list)
                    {
                        w.WriteLine(x.ToString());
                    }
                    w.Close();
                }

            }

        }

    }

   public class CalibData
    {
        public double CalCurvMass;
        public double CalCurvDoubles;
        public double CalPtsMass;
        public double CalPtsDoubles;
        public int number;

        internal CalibData()
        {
        }

		public string CurveRep
		{
			get
			{
				return string.Format("{0} Mass: {1:F5} Doubles: {2:F5}", number, CalCurvMass, CalCurvDoubles);
			}
		}

		public string PointRep
		{
			get
			{
				return string.Format("{0} Mass: {1:F5} Doubles: {2:F5}", number, CalPtsMass, CalPtsDoubles);
			}
		}
    }

	public class MeasPointData
    {
        public double Mass;
        public double Doubles;
        public int number;
		public DateTimeOffset dto;

        internal MeasPointData(DateTimeOffset _dto)
        {
			dto = _dto;
        }

		public override string ToString()
		{
			return string.Format("Mass: {0:F5} Doubles: {1:F5}, {2}", Mass, Doubles, dto.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}


	public class MeasPointList : List<MeasPointData>  // measured results
	{
        public AnalysisMethod am;
		public double LowerMass, UpperMass;
		public MeasPointList()
		{
		}


        public double[] DoublesAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((MeasPointData)iter.Current).Doubles;
                    i++;
                }
                return a;
            }
        }

        public double[] MassAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((MeasPointData)iter.Current).Mass;
                    i++;
                }
                return a;
            }
        }

		public void CalcLowerUpper()
		{
			double lower = 0, upper = 0;
			System.Collections.IEnumerator iter = GetEnumerator();
			while (iter.MoveNext())
			{
				MeasPointData dl = (MeasPointData)iter.Current;
				if (dl.Mass > upper)
					upper = dl.Mass;
				if (dl.Mass < lower)
					lower = dl.Mass;
			}
			LowerMass = lower;
			UpperMass = upper;
		}
	}

    public class CalibList : List<CalibData>  // analysis method calib points, dcl mass[] dbls[], and cal curve adjusted mass[] and dbls[]
    {
        public CalibList()
        {
            MinCalCurvMass = 0;
            MaxCalCurvMass = 1000000;
            MinCalCurvDoubles = 0;
            MaxCalCurvDoubles = 1000000;
            MinCalPtsDoubles = 0;
            MaxCalPtsDoubles = 1000000;
            MinCalPtsMass = 0;
            MaxCalPtsMass = 1000000;
        }

        public AnalysisMethod am;
        public double MinCalCurvDoubles, MaxCalCurvDoubles, MinCalCurvMass, MaxCalCurvMass;
        public double MinCalPtsDoubles, MaxCalPtsDoubles, MinCalPtsMass, MaxCalPtsMass;

		const double LargeMassConst = 999.9;
		public double MaxDeclMass;
		public int NumCalibPoints = 0;

        public double[] CalCurveDoublesAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((CalibData)iter.Current).CalCurvDoubles;
                    i++;
                }
                return a;
            }
        }
        public double[] CalCurveMassAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((CalibData)iter.Current).CalCurvMass;
                    i++;
                }
                return a;
            }
        }

		public double[] CalPtsMass
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((CalibData)iter.Current).CalPtsMass;
                    i++;
                }
                return a;
            }

        }

		public double[] CalPtsDoublesAsArray
        {
            get
            {
                double[] a = new double[Count];
                System.Collections.IEnumerator iter = GetEnumerator();
                int i = 0;
                while (iter.MoveNext())
                {
                    a[i] = ((CalibData)iter.Current).CalPtsDoubles;
                    i++;
                }
                return a;
            }
        }

        public void CalcLowerUpper()  // this is rather lame code 
        {
            double[] lower = new double[4];
			double[] upper = new double[4];
            System.Collections.IEnumerator iter = GetEnumerator();
            bool first = true;
            while (iter.MoveNext())
            {
                CalibData dl = (CalibData)iter.Current;
                if (first)
                {
                    lower[0] = dl.CalCurvDoubles;
                    upper[0] = dl.CalCurvDoubles;
                    lower[1] = dl.CalCurvMass;
                    upper[1] = dl.CalCurvMass;
                    lower[2] = dl.CalPtsDoubles;
                    upper[2] = dl.CalPtsDoubles;
                    lower[3] = dl.CalPtsMass;
                    upper[3] = dl.CalPtsMass;
                    first = false;
                }
                if (dl.CalCurvDoubles > upper[0])
                    upper[0] = dl.CalCurvDoubles;
                if (dl.CalCurvDoubles < lower[0])
                    lower[0] = dl.CalCurvDoubles;
                if (dl.CalCurvMass > upper[1])
                    upper[1] = dl.CalCurvMass;
                if (dl.CalCurvMass < lower[1])
                    lower[1] = dl.CalCurvMass;
                if (dl.CalPtsDoubles > upper[2])
                    upper[2] = dl.CalPtsDoubles;
                if (dl.CalPtsDoubles < lower[2])
                    lower[2] = dl.CalPtsDoubles;
				if (dl.CalPtsMass > upper[3])
                    upper[3] = dl.CalPtsMass;
                if (dl.CalPtsMass < lower[3])
                    lower[3] = dl.CalPtsMass;
			}
            MinCalCurvDoubles = lower[0];
            MaxCalCurvDoubles = upper[0];
            MinCalCurvMass = lower[1];
            MaxCalCurvMass = upper[1];
            MinCalPtsDoubles = lower[2];
            MaxCalPtsDoubles = upper[2];
            MinCalPtsMass = lower[3];
            MaxCalPtsMass = upper[3];
		}

		public void CalculateMassBasis(INCCAnalysisParams.CurveEquationVals cev)
		{
			MaxDeclMass = 0;
			int num = 0;
			foreach(CalibData p in this)
			{
				if (p.CalPtsMass == 0)
					continue;
				num++;
				if (p.CalPtsMass > MaxDeclMass)
					MaxDeclMass = p.CalPtsMass;
			}
			if (MaxDeclMass <= 0)
				MaxDeclMass = LargeMassConst;  // use this NCC5 max constant
			for (int i = 0; i < Count; i++)  // normalize
				this[i].CalCurvMass = ((double)i / (Count - 1)) * MaxDeclMass;
			NumCalibPoints = num;			
		}

    }
}
