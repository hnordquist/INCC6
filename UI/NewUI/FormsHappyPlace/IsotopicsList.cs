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
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AnalysisDefs;
using NCCReporter;
namespace NewUI
{
	using N = NCC.CentralizedState;
	public partial class IsotopicsList : Form
    {

		bool IsIso;
        public IsotopicsList(bool iso)
        {
			IsIso = iso;
            InitializeComponent();
			cols = new SortOrder[listView1.Columns.Count];
            for (int i = 0; i < cols.Length; i++) cols[i] = SortOrder.Descending;
            cols[1] = SortOrder.Ascending;  // datetime column
			System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
			try
			{
				if (IsIso)
					ilist = N.App.DB.Isotopics.GetList();
				else
					cilist = N.App.DB.CompositeIsotopics.GetList();

				bGood = PrepList();
			}
			finally
			{
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
        }

        private List<Isotopics> ilist;
        private List<CompositeIsotopics> cilist;
        protected LMLoggers.LognLM ctrllog;
        SortOrder[] cols;
        public bool bGood = false;

        bool PrepList()
        {
			ctrllog = N.App.ControlLogger;
            if ((IsIso && ilist.Count == 0) || (!IsIso && cilist.Count == 0))
            {
                string msg = string.Format("No isotopics found.");
                MessageBox.Show(msg, "WARNING");
                return false;
            }
            LoadList();
			return true;
		}

		void LoadList()
		{
 			listView1.ShowItemToolTips = true;
			int ilistIndex = 0;
			if (IsIso)
			{
				foreach (Isotopics i in ilist)
				{
					ListViewItem lvi = new ListViewItem(new string[] {
						i.id, i.pu_date.ToString("yy.MM.dd"), i.am_date.ToString("yy.MM.dd"), i.source_code.ToString(),
						ilistIndex.ToString()  // subitem at index 5 has the original ilist index of this element
							});
					listView1.Items.Add(lvi);
					lvi.Tag = i.pu_date;  // for proper column sorting
					ilistIndex++;
				}
				MCount.Text = listView1.Items.Count.ToString() + " isotopics";
			}
			else
			{
				foreach (CompositeIsotopics i in cilist)
				{
					ListViewItem lvi = new ListViewItem(new string[] {
						i.id, i.pu_date.ToString("yy.MM.dd"), i.am_date.ToString("yy.MM.dd"), i.source_code.ToString(),
						ilistIndex.ToString()  // subitem at index 5 has the original ilist index of this element
							});
					listView1.Items.Add(lvi);
					lvi.Tag = i.pu_date;  // for proper column sorting
					ilistIndex++;
				}
				MCount.Text = listView1.Items.Count.ToString() + " composite isotopics";
            }
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString();
            else
                MCountSel.Text = string.Empty;
        }


  
        private void OKBtn_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.OK;
			// do something here        
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
              Help.ShowHelp (null,".\\inccuser.chm");
        }




        public Isotopics GetSingleSelectedIsotopics()
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (!lvi.Selected)
                    continue;
                int lvIndex = 0;
                int.TryParse(lvi.SubItems[4].Text, out lvIndex); // 5 has the original ilist index of this sorted row element
                return (ilist[lvIndex]);
            }
            return null;
        }

		public CompositeIsotopics GetSingleSelectedCompIsotopics()
		{
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (!lvi.Selected)
                    continue;
                int lvIndex = 0;
                int.TryParse(lvi.SubItems[4].Text, out lvIndex); // 5 has the original ilist index of this sorted row element
                return (cilist[lvIndex]);
            }
            return null;
		}

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }


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
                    bool test = IsoListColumnCompare(list.Items[j], list.Items[minIdx], e.Column);
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

        bool IsoListColumnCompare(ListViewItem a, ListViewItem b, int column)
        {
            bool res = false;
            if (column == 0 || column == 3) // id and code
                res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
            else if (column == 1)  // 1 is the Pu Date 
                res = (((DateTime)a.Tag).CompareTo((DateTime)b.Tag)) < 0;
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
