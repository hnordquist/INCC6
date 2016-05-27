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
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

	public partial class IDDDeleteMeas : Form
    {
        private List<INCCDB.IndexedResults> ilist;
        private List<Measurement> mlist;
        SortOrder[] cols;
        private AcquireParameters acq;
		private Detector det;

        public IDDDeleteMeas()
        {
            InitializeComponent();
			//IDDMeasurementList.PrepNotepad();
            cols = new SortOrder[MeasurementView.Columns.Count];
            for (int i = 0; i < cols.Length; i++) cols[i] = SortOrder.Descending;
            cols[3] = SortOrder.Ascending;  // datetime column
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
			bool notify = true;
			NCCReporter.LMLoggers.LognLM ctrllog = N.App.Loggers.Logger(NCCReporter.LMLoggers.AppSection.Control);
            foreach (ListViewItem lvi in MeasurementView.Items)
            {
                if (lvi.Selected)
                {
					int lvIndex = 0;
					if (!int.TryParse(lvi.SubItems[5].Text, out lvIndex)) // 5 has the original mlist index of this sorted row element
						continue;
					MeasId mid = mlist[lvIndex].MeasurementId;
					ctrllog.TraceEvent(NCCReporter.LogLevels.Info, 22222, 
						"Deleting " + mid.MeasOption.PrintName() + " " + mid.MeasDateTime.ToString("yy.MM.dd HH:mm:ss") + ", #" + mid.UniqueId + " for " + det.Id.DetectorId);
					N.App.DB.DeleteMeasurement(mlist[lvIndex].MeasurementId);
					notify = false;
                }
            }
			LoadList(notify);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }
        private void LoadMeasurementList(bool notify = true)
        {
			MeasurementView.Items.Clear();
			Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
			ilist = N.App.DB.IndexedResultsFor(det.Id.DetectorId, string.Empty, "All");
			mlist = N.App.DB.MeasurementsFor(ilist, LMOnly: false);
            if (notify && mlist.Count == 0)
            {
                string msg = string.Format("No measurements for {0} found.", det == null ? "any" : det.Id.DetectorId);
                MessageBox.Show(msg, "WARNING");
                return;
            }
            MeasurementView.ShowItemToolTips = true;
			int mlistIndex = 0;

            foreach (Measurement m in mlist)
            {
                ListViewItem lvi = new ListViewItem(new string[] {
                    m.MeasOption.PrintName(),
                    m.AcquireState.item_id,
					string.IsNullOrEmpty(m.AcquireState.stratum_id.Name) ? "Default" : m.AcquireState.stratum_id.Name,
                    m.MeasDate.DateTime.ToString("yy.MM.dd  HH:mm:ss"), m.AcquireState.comment,
					mlistIndex.ToString()  // subitem at index 7 has the original mlist index of this element
					});
                MeasurementView.Items.Add(lvi);

				lvi.Tag = m.MeasDate;  // for proper column sorting
                lvi.ToolTipText = GetMainFilePath(m.ResultsFiles, m.MeasOption, false);
				if (string.IsNullOrEmpty(lvi.ToolTipText))
					lvi.ToolTipText = "No results file available";
				mlistIndex++;
            }
            MCount.Text = MeasurementView.Items.Count.ToString() + " measurements";
			if (MeasurementView.SelectedItems.Count > 0)
				MCountSel.Text = MeasurementView.SelectedItems.Count.ToString();
			else
				MCountSel.Text = string.Empty;
			//Refresh();
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
                res= System.IO.Path.GetFileName(res);
            return res;

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

		private void MeasurementView_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (MeasurementView.SelectedItems.Count > 0)
                MCountSel.Text = MeasurementView.SelectedItems.Count.ToString() + " selected";
            else
                MCountSel.Text = string.Empty;   
		}

		bool MeasListColumnCompare(ListViewItem a, ListViewItem b, int column)
        {
            bool res = false;
            if (column != 3)
                res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
            else if (column == 3)  // 3 is the datetime column, such fragile coding ... fix by adding a type value to the column header Tag 
                res = (((DateTimeOffset)a.Tag).CompareTo((DateTimeOffset)b.Tag)) < 0;
            return res;
        }

		private void MeasurementView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			Cursor sav = MeasurementView.Cursor;
			MeasurementView.Cursor = Cursors.WaitCursor;
            ListItemSorter(sender, e);
			MeasurementView.Cursor = sav;
		}

		private void LoadList(bool notify = true)
		{
			System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			try
			{
				Refresh();
				LoadMeasurementList(notify);
			}
			finally
			{
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
		}

		private void IDDDeleteMeas_Shown(object sender, EventArgs e)
		{
			LoadList();
		}
	}
}
