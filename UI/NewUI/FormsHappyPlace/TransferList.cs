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
using System.IO;
using System.Windows.Forms;
using AnalysisDefs;
using NCCReporter;
using NCCTransfer;
namespace NewUI
{
	using N = NCC.CentralizedState;
	public partial class TransferList : Form
    {

		public List<INCCKnew.TransferSummary> list;

        public TransferList(List<INCCKnew.TransferSummary> l)
        {
            InitializeComponent();
            cols = new SortOrder[listView1.Columns.Count];
            for (int i = 0; i < cols.Length; i++) cols[i] = SortOrder.Descending;
            cols[3] = SortOrder.Ascending;  // datetime column
			list = l;
			LoadList();
        }

        protected LMLoggers.LognLM ctrllog;
        SortOrder[] cols;
        public bool bGood = false;

		void LoadList()
		{
 			listView1.ShowItemToolTips = true;
			int mlistIndex = 0;
			foreach (INCCKnew.TransferSummary ts in list)
            {
                 ListViewItem lvi = new ListViewItem(new string[] {
                    ts.det, ts.item,
					string.IsNullOrEmpty(ts.stratum) ? "-" : ts.stratum,
                    ts.dto.ToString("yy.MM.dd  HH:mm:ss"), System.IO.Path.GetFileName(ts.path),
                    ts.comment,
					mlistIndex.ToString()  // subitem at index 6 has the original mlist index of this element
                        });
                listView1.Items.Add(lvi);
                lvi.Tag = ts.dto;  // for proper column sorting
				mlistIndex++;
            }
            MCount.Text = listView1.Items.Count.ToString() + " files";
            if (listView1.SelectedItems.Count > 0)
                MCountSel.Text = listView1.SelectedItems.Count.ToString();
            else
                MCountSel.Text = string.Empty;
        }


       private void OKBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
			foreach (ListViewItem lv in listView1.SelectedItems)
			{
				if (lv.Selected)
				{
					string s = (string)lv.SubItems[6].Text;
					int i = 0;
					int.TryParse(s, out i);
					list[i].select = true;
				}
			}
			Close();
        }

   
        private void HelpBtn_Click(object sender, EventArgs e)
        {
              Help.ShowHelp (null,".\\inccuser.chm");
        }


        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            foreach (ListViewItem lv in listView1.Items)
            {
                    string s = (string)lv.SubItems[6].Text;
                    int i = 0;
                    int.TryParse(s, out i);
                    list[i].select = false;
            }
            Close();
        }


        public void SetSummarySelections(ResultsSummary sel)
        {
            int i = sel.SortStratum ? 2 : 0;  // column 2 is strata, 0 is detector, 1 is item id, 3 is date time, 4 is path, 5 is comment
            cols[i] = SortOrder.Ascending;
            ListItemSorter(listView1, new ColumnClickEventArgs(i));
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
                    bool test = XFerListColumnCompare(list.Items[j], list.Items[minIdx], e.Column);
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

        bool XFerListColumnCompare(ListViewItem a, ListViewItem b, int column)
        {
            bool res = false;
            if (column != 3)
                res = a.SubItems[column].Text.CompareTo(b.SubItems[column].Text) < 0;
            else if (column == 3)  // 3 is the datetime column, such fragile coding ... fix by adding a type value to the column header Tag 
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
