﻿/*
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AnalysisDefs;
using NCCReporter;
namespace UI
{
	using N = NCC.CentralizedState;
	public partial class IDDMeasurementList : Form
	{

		public enum EndGoal { Report, Summary, Reanalysis, Transfer }

		public IDDMeasurementList(AssaySelector.MeasurementOption filter, bool alltypes, EndGoal goal, Detector detector = null)
		{
			InitializeComponent();
			System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			try
			{
				InitSort();
				PrepNotepad();
				SetTitlesAndChoices(filter, alltypes, goal,
					detector == null ? string.Empty : detector.Id.DetectorId, string.Empty);
				mlist = N.App.DB.MeasurementsFor(detector == null ? string.Empty : detector.Id.DetectorId, filter);
				bGood = PrepList(filter, detector);
			} finally
			{
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
			SummarySelections = null;
		}

		public void Init(List<INCCDB.IndexedResults> ilist,
					AssaySelector.MeasurementOption filter,
					EndGoal goal, bool lmonly, string inspnum = "", Detector detector = null)
		{
			LMOnly = lmonly;
			bool alltypes = filter.IsWildCard() && !lmonly;
			System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			try
			{
				PrepNotepad();
				SetTitlesAndChoices(filter, alltypes, goal,
					detector == null ? string.Empty : detector.Id.DetectorId, string.Empty);
				mlist = N.App.DB.MeasurementsFor(ilist, LMOnly, skipMethods: false);
				bGood = PrepList(filter, detector);
			} finally
			{
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
		}

		public IDDMeasurementList()
		{
			InitializeComponent();
			SummarySelections = null;
			InitSort();
        }

		private List<Measurement> mlist;
		SortOrder[] cols;

		bool AllMeas = false; // true means all measurements, false means the specified option type
		EndGoal Goal = EndGoal.Summary; // summary implies all detectors        
		bool LMOnly = false;
		public bool TextReport = true;
		public bool bGood = false;
        public ReportSectional Sections;

        void InitSort()
		{
			cols = new SortOrder[listView1.Columns.Count];
			for (int i = 0; i < cols.Length; i++)
				cols[i] = SortOrder.Descending;
			cols[4] = SortOrder.Ascending;  // datetime column
		}

		void SetTitlesAndChoices(AssaySelector.MeasurementOption filter, bool alltypes, EndGoal goal, string detector = "", string inspnum = "")
		{
			string upthehill = "Measurement Selection for Detector";
			string backwards = "Measurement Selection for All Detectors";
			string itwillbe = "Select Measurement for Detector";
			string allright = "Select Measurements to Save for Detector";
			AllMeas = alltypes;
			Goal = goal;
			string title = "";
			if (Goal == EndGoal.Report)
				title = upthehill;
			else if (Goal == EndGoal.Summary)
				title = backwards;
			else if (Goal == EndGoal.Reanalysis)
			{
				title = itwillbe;
				listView1.MultiSelect = false;
			} else // if (Goal == EndGoal.Transfer)
				title = allright;
			if (!AllMeas && Goal != EndGoal.Reanalysis)
				title = (filter.PrintName() + " " + title);
			if (!string.IsNullOrEmpty(detector))
				title += (" " + detector);
			if (!string.IsNullOrEmpty(inspnum))
				title += (", Inspection #" + inspnum);
			Text = title;
		}

		bool PrepList(AssaySelector.MeasurementOption filter, Detector det)
		{
			if (Goal == EndGoal.Report)
			{
				if (LMOnly)  // LMOnly
					mlist.RemoveAll(EmptyCSVFile);    // cull those without LM CSV results
				else
					mlist.RemoveAll(EmptyINCC5File);  // cull those with traditional INCC5 results
			}
			if (mlist.Count == 0)
			{
				string msg = string.Format("No {0}measurements for {1} found.", TypeTextFragment(filter), det == null ? "any" : det.Id.DetectorId);
				MessageBox.Show(msg, "WARNING");
				return false;
			}
			LoadList(filter);
			if (Goal == EndGoal.Report || Goal == EndGoal.Reanalysis || Goal == EndGoal.Transfer)   // it is for a named detector so elide the detector column
				listView1.Columns[1].Width = 0;
			if (!AllMeas)
				listView1.Columns[0].Width = 0;
			if (filter == AssaySelector.MeasurementOption.rates)    // show item id
				listView1.Columns[2].Width = 0;
			if (filter == AssaySelector.MeasurementOption.normalization)
			{
				listView1.Columns[2].Width = 0;
				listView1.Columns[3].Width = 0;
			}
			if (filter == AssaySelector.MeasurementOption.background)     // NEXT: add configuration active or passive column
			{
				listView1.Columns[2].Width = 0;
				listView1.Columns[3].Width = 0;
			}
			if (!AssaySelector.ForMass(filter) && !filter.IsWildCard())
				listView1.Columns[7].Width = 0;		  // material column
			
			if (Goal == EndGoal.Reanalysis)
			{
				listView1.Columns[0].Text = "Id";
				listView1.Columns[0].Width = 43;
			}

			return true;
		}

		void LoadList(AssaySelector.MeasurementOption filter)
		{
			listView1.ShowItemToolTips = true;
			int mlistIndex = 0;
			foreach (Measurement m in mlist)
			{
                int CycleCount = N.App.DB.GetCycleCount(m.MeasurementId);

                string ItemWithNumber = string.IsNullOrEmpty(m.MeasurementId.Item.item) ? "-" : m.AcquireState.ItemId.item;
				if (Path.GetFileName(m.MeasurementId.FileName).Contains("_") && (AssaySelector.MeasurementOption.verification == filter) && (filter == m.MeasOption))
					//scan file name to display subsequent reanalysis number...... hn 9.21.2015
					ItemWithNumber += "(" + Path.GetFileName(m.MeasurementId.FileName).Substring(Path.GetFileName(m.MeasurementId.FileName).IndexOf('_') + 1, 2) + ")";
				string col0;
				if (Goal == EndGoal.Reanalysis)
					col0 = m.MeasurementId.UniqueId.ToString();
				else
					col0 = m.MeasOption.PrintName();
				ListViewItem lvi = new ListViewItem(new string[] {
					col0, m.Detector.Id.DetectorId, ItemWithNumber,
					string.IsNullOrEmpty(m.AcquireState.stratum_id.Name) ? "-" : m.AcquireState.stratum_id.Name,
					m.MeasDate.DateTime.ToString("yy.MM.dd  HH:mm:ss"), GetMainFilePath(m.ResultsFiles, m.MeasOption, true),
                    CycleCount.ToString(),
                    AssaySelector.ForMass(m.MeasOption) ? m.AcquireState.item_type : string.Empty,
                    m.AcquireState.comment,
                    mlistIndex.ToString()  // subitem at index 9 has the original mlist index of this element
                        });
				listView1.Items.Add(lvi);
				lvi.Tag = m.MeasDate;  // for proper column sorting
				string p = GetMainFilePath(m.ResultsFiles, m.MeasOption, false);
				if (string.IsNullOrEmpty(p))
					lvi.ToolTipText = "(" + m.MeasurementId.UniqueId.ToString() + ") No results file available";
				else
					lvi.ToolTipText = "(" + m.MeasurementId.UniqueId.ToString() + ") " + p;
				mlistIndex++;
			}
			MCount.Text = listView1.Items.Count.ToString() + " measurements";
			if (listView1.SelectedItems.Count > 0)
				MCountSel.Text = listView1.SelectedItems.Count.ToString();
			else
				MCountSel.Text = string.Empty;
		}

		string TypeTextFragment(AssaySelector.MeasurementOption filter)
		{
			if (filter.IsListMode() && LMOnly)
				return "List Mode ";
			else
			{
				if (AllMeas)
					return string.Empty;
				else
					return (AssaySelector.MeasurementOption.rates == filter ? "Rates Only" : filter.PrintName()) + " ";
			}
		}

		public static bool EmptyINCC5File(Measurement m)
		{
			return m.ResultsFiles.Count <= 0 || string.IsNullOrEmpty(m.ResultsFiles.PrimaryINCC5Filename.Path);
		}
		public static bool EmptyCSVFile(Measurement m)
		{
			return string.IsNullOrEmpty(m.ResultsFiles.CSVResultsFileName.Path);
		}
		private void PrintBtn_Click(object sender, EventArgs e)   // NEXT: this must be customized for each list view subtype
		{
			string path = Path.GetTempFileName();
			FileStream f = new FileStream(path, FileMode.OpenOrCreate);
			StreamWriter s = new StreamWriter(f);
			s.AutoFlush = true;
			s.Write(string.Format("{0,20}\t\t{1,20}\t\t{2,20}\t\t{3,20}\t\t{4,20}\r\n", "Item id", "Stratum id", "Measurement Type", "Measurement Date and Time", ""));
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
			if (Goal == EndGoal.Report && TextReport)
				ShowReconstitutedResults();
			else if (Goal == EndGoal.Summary)
				WriteSummary();
			else if (Goal == EndGoal.Reanalysis)
				DialogResult = DialogResult.OK;
			else if (Goal == EndGoal.Transfer)
				DialogResult = DialogResult.OK;
			Close();
		}

		void WriteSummary()
		{
			SummarySelections.ResetSummaryRows();
			foreach (ListViewItem lvi in listView1.Items)
			{
				if (!lvi.Selected)
					continue;
				int lvIndex = 0;
				int.TryParse(lvi.SubItems[9].Text, out lvIndex); // 9 has the original mlist index of this sorted row element
				SummarySelections.Apply(mlist[lvIndex]);
			}
			if (SummarySelections.HasAny)
			{
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
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
                        N.App.ControlLogger.TraceInformation("Summary written to " + dlg.FileName);
					} catch (IOException ex)
					{
						MessageBox.Show(ex.Message, "Error on " + dlg.FileName);
					}
				}
			}
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(null, ".\\inccuser.chm");
		}

		private void ShowResults()
		{
			foreach (ListViewItem lvi in listView1.Items)
			{
				if (lvi.Selected)
				{
					int lvIndex = 0;
					int.TryParse(lvi.SubItems[9].Text, out lvIndex); // 9 has the original mlist index of this sorted row element
					if (bNotepadHappensToBeThere)
					{
						string path = GetMainFilePath(mlist[lvIndex].ResultsFiles, mlist[lvIndex].MeasOption, false);
						if (File.Exists(path))
							System.Diagnostics.Process.Start(notepadPath, path);
						else if (!string.IsNullOrEmpty(path))
                            N.App.ControlLogger.TraceEvent(LogLevels.Error, 22222, "The file path '" + path + "' cannot be accessed.");
						else
                            N.App.ControlLogger.TraceEvent(LogLevels.Error, 22222, "No file path");
					}
					lvi.Selected = false;
				}
			}
		}

        
        private void ShowReconstitutedResults()
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    int lvIndex = 0;
                    int.TryParse(lvi.SubItems[9].Text, out lvIndex); // 9 has the original mlist index of this sorted row element
                    Measurement m = mlist[lvIndex];
                    m.AcquireState.review = Sections;
                    if (Sections.CycleDataSelected)
                    {
                        m.ReportRecalc();
                    }
                    m.ResultsFiles.Reset();
                    new ReportMangler(N.App.ControlLogger).GenerateReports(m);
                    if (!N.App.AppContext.OpenResults && bNotepadHappensToBeThere)  // opened in GenerateReports if true flag App.AppContext.OpenResults
                    {
                        string path = GetMainFilePath(m.ResultsFiles, m.MeasOption, false);
                        if (File.Exists(path))
                            System.Diagnostics.Process.Start(notepadPath, path);
                        else if (!string.IsNullOrEmpty(path))
                            N.App.ControlLogger.TraceEvent(LogLevels.Error, 22222, "The file path '" + path + "' cannot be accessed.");
                        else
                            N.App.ControlLogger.TraceEvent(LogLevels.Error, 22222, "No file path");
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
				res = Path.GetFileName(res);
			return res;
		}

		public Measurement GetSingleSelectedMeas()
		{
			foreach (ListViewItem lvi in listView1.Items)
			{
				if (!lvi.Selected)
					continue;
				int lvIndex = 0;
				int.TryParse(lvi.SubItems[9].Text, out lvIndex); // 9 has the original mlist index of this sorted row element
				return (mlist[lvIndex]);
			}
			return null;
		}

		public List<Measurement> GetSelectedMeas()
		{
			List<Measurement> newlist = new List<Measurement>();
			foreach (ListViewItem lvi in listView1.Items)
			{
				if (!lvi.Selected)
					continue;
				int lvIndex = 0;
				int.TryParse(lvi.SubItems[9].Text, out lvIndex); // 9 has the original mlist index of this sorted row element
				newlist.Add(mlist[lvIndex]);
			}
			return newlist;
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (Goal == EndGoal.Report)
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
			Cursor sav = listView1.Cursor;
			listView1.Cursor = Cursors.WaitCursor;
			ListItemSorter(sender, e);
			listView1.Cursor = sav;
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
			if (column != 4 && column != 6)
				res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
			else if (column == 4)  // 4 is the datetime column, such fragile coding ... fix by adding a type value to the column header Tag 
				res = (((DateTimeOffset)a.Tag).CompareTo((DateTimeOffset)b.Tag)) < 0;
            else if (column == 6)    // 6 is the integer cycle count
            {
				int la = 0, rb = 0;
                int.TryParse(a.SubItems[column].Text, out la);
                int.TryParse(b.SubItems[column].Text, out rb);
                res = (la < rb);
            }
            return res;
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count > 0)
				MCountSel.Text = listView1.SelectedItems.Count.ToString() + " selected";
			else
				MCountSel.Text = string.Empty;
		}

		static string notepadPath = string.Empty;
		static bool bNotepadHappensToBeThere = false;
		static void PrepNotepad()
		{
			if (string.IsNullOrEmpty(notepadPath))
			{
				notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");
				bNotepadHappensToBeThere = File.Exists(notepadPath);
			}
		}

    }
}
