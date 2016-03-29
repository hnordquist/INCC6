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
	using N = NCC.CentralizedState;
    public partial class IDDAssaySummary : Form
    {
        static ResultsSummary sel;
        AssaySelector.MeasurementOption option; 
        public IDDAssaySummary(string whuchuwan)                          
        {
            InitializeComponent();
			mlist = N.App.DB.IndexedResultsFor(whuchuwan);
            if (sel == null)
                sel = new ResultsSummary();
            sel.Option = whuchuwan;
            LoadInspNumCombo();
            SortByStratRadioBtn.Checked = sel.SortStratum;
            DefaultCurrentRadioBtn.Checked = sel.WithCurrentEndDate;
            StartDateTimePicker.Value = sel.Start.DateTime;
            LoadSelections(treeView1.Nodes);
            string l = option.PrintName();
            if (!Enum.TryParse(whuchuwan, out option))
            {
                option = AssaySelector.MeasurementOption.unspecified;
                l = "Measurement";
            }
            this.Text = l + " " + this.Text; 
        }           

        private List<INCCDB.IndexedResults> mlist;

        void LoadSelections(TreeNodeCollection col)
        {
            if (col == null) return;
            foreach (TreeNode cn in col)
            {
                cn.Checked = sel.Root[cn.Name].Enabled;
                LoadSelections(cn.Nodes);
            }
        }

        void LoadInspNumCombo()
		{
			SortedSet<string> set = new SortedSet<string>();
			InspectionNumComboBox.Items.Add("All");
			foreach(INCCDB.IndexedResults ir in mlist)
			{
				if (!string.IsNullOrEmpty(ir.Campaign))
					set.Add(ir.Campaign);
			}
			foreach(string si in set)
			{
				InspectionNumComboBox.Items.Add(si);
			}
            InspectionNumComboBox.SelectedItem = sel.InspectionNumber;
        }

        private void SortByStratRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = ((RadioButton)sender);
            sel.SortStratum = r.Checked;
        }

        private void DefaultCurrentRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = ((RadioButton)sender);
            sel.WithCurrentEndDate = r.Checked;
            if (sel.WithCurrentEndDate)
                EndDateTimePicker.Value = sel.End.DateTime;
            else
                EndDateTimePicker.Value = DateTime.Now; // value will get swept back to the "sel" state upon Ok      
        }

        private void PrintCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        static bool CheckForMeasIDMatch(INCCDB.IndexedResults ir)
            {
            if (sel.Start <= ir.DateTime && sel.End >= ir.DateTime)
            {
                return (string.Compare(sel.InspectionNumber, "All", true) == 0) ||
                       (string.Compare(sel.InspectionNumber, ir.Campaign, true) == 0);
            }
            return false;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            //AssaySelector.MeasurementOption option =
            //    AssaySelector.MeasurementOption
            sel.InspectionNumber = InspectionNumComboBox.SelectedItem.ToString();
            sel.Start = StartDateTimePicker.Value;
            sel.End = EndDateTimePicker.Value;
            List<INCCDB.IndexedResults> list = null;
            list = mlist.FindAll(ir => CheckForMeasIDMatch(ir));
            IDDMeasurementList measlist = new IDDMeasurementList();
            measlist.Init(list, option, false, false, sel.InspectionNumber);
            measlist.SetSummarySelections(sel); // does a pre-sort before display
            if (measlist.bGood)
                measlist.ShowDialog();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.ByMouse) // user selected
			{
				ResultsSummary.State sta = sel.Root[e.Node.Name];
				sta.Enabled = e.Node.Checked;
			}
		}
    }
}
