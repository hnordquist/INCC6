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
using NCCReporter;
namespace NewUI
{
    using N = NCC.CentralizedState;

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
        public CalibrationCurveList CalcDataList;
        public AssaySelector.MeasurementOption mopt;

        private string DetectorId;

        public void Init(string id, AssaySelector.MeasurementOption mo = AssaySelector.MeasurementOption.verification)
        {
            CalcDataList = new CalibrationCurveList();
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
                N.App.Loggers.Logger(LMLoggers.AppSection.Control).TraceEvent(LogLevels.Warning, 3363, msg);
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
                DoublesDclMass dl = GetTheDataPoints(m);
                string ItemWithNumber = string.IsNullOrEmpty(m.MeasurementId.Item.item) ? "-" : m.AcquireState.ItemId.item;
                ListViewItem lvi = new ListViewItem(new string[] {
                    ItemWithNumber,
                    m.MeasDate.DateTime.ToString("yy.MM.dd  HH:mm:ss"),
                    dl.Mass.v.ToString("F2"),
                    dl.Doubles.v.ToString("F2"),
                    mlistIndex.ToString(),  // subitem at index 4 has the original mlist index of this element
					});
                listView1.Items.Add(lvi);
                lvi.Tag = dl;  // for proper column sorting
                lvi.ToolTipText = "Using " + m.INCCAnalysisState.Methods.selector.ToString();
                mlistIndex++;
            }
            MCount.Text = listView1.Items.Count.ToString() + " measurements";
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString();
            else
                MCountSel.Text = string.Empty;
        }

        public DoublesDclMass GetTheDataPoints(Measurement m)
        {
            DoublesDclMass dl = new DoublesDclMass(m.MeasDate);
            MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[m.Detector.MultiplicityParams];
            INCCMethodResult imr = m.INCCAnalysisResults.LookupMethodResults(m.Detector.MultiplicityParams, m.INCCAnalysisState.Methods.selector, AnalysisMethod, create: false);
            switch (AnalysisMethod)
            {
                case AnalysisMethod.CalibrationCurve:
                    INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)imr;
                    dl.Doubles.v = mcr.DeadtimeCorrectedDoublesRate.v;
                    dl.Mass.v = ccres.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.KnownA:
                    INCCMethodResults.results_known_alpha_rec kares = (INCCMethodResults.results_known_alpha_rec)imr;
                    dl.Doubles.v = (kares.corr_doubles.v == 0 ? mcr.DeadtimeCorrectedDoublesRate.v : kares.corr_doubles.v);
                    dl.Mass.v = kares.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.AddASource:
                    INCCMethodResults.results_add_a_source_rec aares = (INCCMethodResults.results_add_a_source_rec)imr;
                    dl.Doubles.v = mcr.DeadtimeCorrectedDoublesRate.v;
                    dl.Mass.v = aares.dcl_pu240e_mass;
                    break;
                case AnalysisMethod.Active:
                    INCCMethodResults.results_active_rec acres = (INCCMethodResults.results_active_rec)imr;
                    dl.Doubles.v = mcr.DeadtimeCorrectedDoublesRate.v;
                    dl.Mass.v = acres.dcl_u235_mass;
                    break;
            }
            return dl;
        }

        private void OKBtn_Click(object sender, EventArgs e)
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
            SaveCurveXYValues(list);
            Close();
        }

        void SaveCurveXYValues(CalibrationCurveList list)
        {
            CalcDataList = new CalibrationCurveList();
            if (list.Count < 1)
                return;
            // save the data-points for use in the Get file op that replaces the deming
            System.Collections.IEnumerator _iter = list.GetEnumerator();
            while (_iter.MoveNext())
            {
                DoublesDclMass dl = (DoublesDclMass)_iter.Current;
                CalcDataList.Add(dl);
            }
            CalcDataList.CalcLowerUpper();          // compute upper and lower mass limits from the data points,
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
                res = (((DoublesDclMass)a.Tag).Mass.v.CompareTo(((DoublesDclMass)b.Tag).Mass.v)) < 0;
            else if (column == 3)  // doubles
                res = (((DoublesDclMass)a.Tag).Doubles.v.CompareTo(((DoublesDclMass)b.Tag).Doubles.v)) < 0;
            else if (column == 4) // 4 is the datetime column, such fragile coding
                res = (((DoublesDclMass)a.Tag).dt.CompareTo(((DoublesDclMass)b.Tag).dt)) < 0;
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
	}
}
