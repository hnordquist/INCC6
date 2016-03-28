/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
    public partial class IDDMeasurementList : Form
    {
 
        public IDDMeasurementList(AssaySelector.MeasurementOption filter, bool alltypes, bool report, Detector detector = null)
        {
            InitializeComponent();
            SetTitlesAndChoices(filter, alltypes, report,
                detector == null ? string.Empty : detector.Id.DetectorId, string.Empty);
            mlist = N.App.DB.MeasurementsFor(detector == null? string.Empty : detector.Id.DetectorId, filter);
            bGood = PrepList(filter, detector);
            SummarySelections = null;
        }

        public void Init(List<INCCDB.IndexedResults> ilist, 
                    AssaySelector.MeasurementOption filter, 
                    bool report, bool lmonly, string inspnum = "", Detector detector = null)
        {
            LMOnly = lmonly;
            bool alltypes = (AssaySelector.MeasurementOption.unspecified == filter) && !lmonly;
            SetTitlesAndChoices(filter, alltypes, report,
                detector == null ? string.Empty : detector.Id.DetectorId, string.Empty);
            mlist = N.App.DB.MeasurementsFor(ilist, LMOnly);
            bGood = PrepList(filter, detector);
        }

        public IDDMeasurementList()
        {
            InitializeComponent();
            SummarySelections = null;
            cols = new SortOrder[listView1.Columns.Count];
            for (int i = 0; i < cols.Length; i++) cols[i] = SortOrder.Descending;
            cols[4] = SortOrder.Ascending;  // datetime column
        }

        private List<Measurement> mlist;
        protected LMLoggers.LognLM ctrllog;
        SortOrder[] cols;

        bool AllMeas = false; // true means all measurements, false means the specified option type
        bool Reports = false; // false means summary, summary means all detectors        
        bool LMOnly = false;
        public bool bGood = false;

        void SetTitlesAndChoices(AssaySelector.MeasurementOption filter, bool alltypes, bool report, string detector = "", string inspnum = "")
        {
            string upthehill = "Measurement Selection for Detector";
            string backwards = "Measurement Selection for All Detectors";
            AllMeas = alltypes;
            Reports = report;
            string title = "";
            if (Reports)
                title = upthehill;
            else
                title = backwards;
            if (!AllMeas)
                title = (filter.PrintName() + " " + title);
            if (!string.IsNullOrEmpty(detector))
                title += (" " + detector);
            if (!string.IsNullOrEmpty(inspnum))
                title += (", Inspection #" + inspnum);
            this.Text = title;
        }

        bool PrepList(AssaySelector.MeasurementOption filter, Detector det)
        {
            if (Reports)
            { 
                if (LMOnly)  // LMOnly
                    mlist.RemoveAll(EmptyCSVFile);    // cull those without LM CSV results
                else
                    mlist.RemoveAll(EmptyINCC5File);  // cull those with traditional INCC5 results
            }
			ctrllog = N.App.Loggers.Logger(LMLoggers.AppSection.Control);
            if (mlist.Count == 0)
            {
                string msg = string.Format("No {0}measurements for {1} found.", TypeTextFragment(filter), det == null ? "any" : det.Id.DetectorId);
                MessageBox.Show(msg, "WARNING");
                return false;
            }
            LoadList(filter);
            if (Reports)   // it is all detectors 
                listView1.Columns[1].Width = 0;
            if (!AllMeas)
				listView1.Columns[0].Width = 0;
            if (filter == AssaySelector.MeasurementOption.rates)    // show item id
            {
                listView1.Columns[2].Width = 0;
            }
            if (filter == AssaySelector.MeasurementOption.normalization)
            {
                listView1.Columns[2].Width = 0;
                listView1.Columns[3].Width = 0;   
            }
            if (filter == AssaySelector.MeasurementOption.background)     // next: add configuration active or passive column
            {
                listView1.Columns[2].Width = 0;
                listView1.Columns[3].Width = 0;
            }
            return true;
		}

		void LoadList(AssaySelector.MeasurementOption filter)
		{
 			listView1.ShowItemToolTips = true;
			foreach (Measurement m in mlist)
            {
                string ItemWithNumber = string.IsNullOrEmpty(m.MeasurementId.Item.item) ? "-" : m.AcquireState.ItemId.item;
                if (Path.GetFileName(m.MeasurementId.FileName).Contains("_") && (AssaySelector.MeasurementOption.verification == filter) && (filter == m.MeasOption))
                    //scan file name to display subsequent reanalysis number...... hn 9.21.2015
                    ItemWithNumber += "(" + Path.GetFileName(m.MeasurementId.FileName).Substring(Path.GetFileName(m.MeasurementId.FileName).IndexOf('_') + 1, 2) + ")";

                ListViewItem lvi = new ListViewItem(new string[] {
                    m.MeasOption.PrintName(), m.Detector.Id.DetectorId, ItemWithNumber,
					string.IsNullOrEmpty(m.AcquireState.stratum_id.Name) ? "-" : m.AcquireState.stratum_id.Name,
                    m.MeasDate.DateTime.ToString("MM.dd.yy  HH:mm:ss"), GetMainFilePath(m.ResultsFiles, m.MeasOption, true)
                        });
                listView1.Items.Add(lvi);
                lvi.Tag = m.MeasDate;  // for proper column sorting
                lvi.ToolTipText = GetMainFilePath(m.ResultsFiles, m.MeasOption, false);
				if (string.IsNullOrEmpty(lvi.ToolTipText))
					lvi.ToolTipText = "No results file available";
            }
            MCount.Text = listView1.Items.Count.ToString() + " measurements";
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString();
            else
                MCountSel.Text = string.Empty;
        }

        string TypeTextFragment(AssaySelector.MeasurementOption filter)
		{
            if (filter == AssaySelector.MeasurementOption.unspecified)
				return "List Mode ";
			else
				return (AssaySelector.MeasurementOption.rates == filter ? "Rates Only" : filter.PrintName()) + " ";
		}

        public static bool EmptyINCC5File(Measurement m)
        {
            return m.ResultsFiles.Count <= 0 || string.IsNullOrEmpty(m.ResultsFiles.PrimaryINCC5Filename.Path);
        }
		public static bool EmptyCSVFile(Measurement m)
        {
            return string.IsNullOrEmpty(m.ResultsFiles.CSVResultsFileName.Path);
        }
        private void PrintBtn_Click(object sender, EventArgs e)   // next: this must be customized for each list view subtype
        {
            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            s.Write (String.Format ("{0,20}\t\t{1,20}\t\t{2,20}\t\t{3,20}\t\t{4,20}\r\n","Item id","Stratum id","Measurement Type","Measurement Date and Time",""));
            foreach (ListViewItem lvi in listView1.Items)
            {
                s.Write(String.Format("{0,20}\t\t{1,20}\t\t{2,20}\t\t{3,20}\t\t{4,20}\r\n", lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[4].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text));
            }
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text, "Print List");
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (Reports)
                ShowResults();
            else
                WriteSummary();          
            Close();
        }

        void WriteSummary()
        {
            SummarySelections.ResetSummaryRows();
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (!lvi.Selected)
                    continue;
                SummarySelections.Apply(mlist[lvi.Index]);
            }
            if (SummarySelections.HasAny)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "CSV files (*.csv) | All files (*.*)";
                dlg.DefaultExt = ".csv";
                dlg.FileName = "summary.csv";
                dlg.InitialDirectory = N.App.AppContext.ResultsFilePath;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string lw = SummarySelections.HeaderRow;
                    try
                    {
                        StreamWriter tx = File.CreateText(dlg.FileName);
                           tx.WriteLine(lw);
                        System.Collections.IEnumerator iter = SummarySelections.GetEntryEnumerator();
                        while (iter.MoveNext())
                        {
                           string entry = (string)iter.Current;
                           tx.WriteLine(entry);
                        }
                        tx.Close();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message, "Error on " + dlg.FileName);
                    }
                }
            }
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
              Help.ShowHelp (null,".\\inccuser.chm");
        }

        private void ShowResults()
        {
            string notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    if (File.Exists(notepadPath))
                    {
						string path = GetMainFilePath(mlist[lvi.Index].ResultsFiles, mlist[lvi.Index].MeasOption, false);
                        if (File.Exists(path))
                            System.Diagnostics.Process.Start(notepadPath, path);
                        else if (!string.IsNullOrEmpty(path))
                            ctrllog.TraceEvent(LogLevels.Error, 22222, "The file path '" + path + "' cannot be accessed.");
                        else 
                            ctrllog.TraceEvent(LogLevels.Error, 22222, "No file path");
                    }
                    lvi.Selected = false;
                }
            }
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
                res= Path.GetFileName(res);
            return res;
		}

		private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Reports)
                ShowResults();
        }
        public void SetSummarySelections(ResultsSummary sel)
        {
            int i = sel.SortStratum ? 3 : 1;  // column 3 is strata, 1 is detector, 0 is option, 2 is item id, 4 is date time
            cols[i] = SortOrder.Ascending;
            SummarySelections = sel;
            ListItemSorter(listView1, new ColumnClickEventArgs(i));
        }
        ResultsSummary SummarySelections;


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListItemSorter(sender, e);
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
            if (column != 4)
                res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
            else if (column == 4)  // 4 is the datetime column, such fragile coding ... fix by adding a type value to the column header Tag 
                res = (((DateTimeOffset)a.Tag).CompareTo((DateTimeOffset)b.Tag)) < 0;
            return res;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString() + " selected";
            else
                MCountSel.Text = string.Empty;
        }
    }
}
