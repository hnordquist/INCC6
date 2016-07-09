/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

    using NC = NCC.CentralizedState;

    public partial class IDDItemDataEntry : Form
    {

            // the stringified lists for the item combos
        List<string> mbas, isotopics, mats, strats, iocodes, invcodes;

        // Flag set if user enters new data 
        bool NewContent = false;

        void buildlists()
        {
            invcodes = new List<string>();
            foreach (INCCDB.Descriptor desc in NC.App.DB.InvChangeCodes.GetList())
                invcodes.Add(desc.Name);
            iocodes = new List<string>();
            foreach (INCCDB.Descriptor desc in NC.App.DB.IOCodes.GetList())
                iocodes.Add(desc.Name);
            strats = new List<string>();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Stratums.GetList())
                strats.Add(desc.Name);
            mats = new List<string>();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
                mats.Add(desc.Name);
            isotopics = new List<string>();
            foreach (Isotopics iso in NC.App.DB.Isotopics.GetList())
                isotopics.Add(iso.id);
            mbas = new List<string>();
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
                mbas.Add(desc.Name);
        }

        public IDDItemDataEntry()
        {
            InitializeComponent();
            buildlists();
            DataGridViewColumnCollection dgvcc =  ItemIdDataGrid.Columns;

            DataGridViewComboBoxColumn c = (DataGridViewComboBoxColumn)dgvcc["MBA"];
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
                c.Items.Add(desc.Name);

            c = (DataGridViewComboBoxColumn)dgvcc["IsoId"];
            foreach (string s in isotopics)
                c.Items.Add(s);

            c = (DataGridViewComboBoxColumn)dgvcc["MatType"];
            foreach (string s in mats)
                c.Items.Add(s);

            c = (DataGridViewComboBoxColumn)dgvcc["StratId"];
            foreach (string s in strats)
                c.Items.Add(s);

            c = (DataGridViewComboBoxColumn)dgvcc["IOCode"];
            foreach (string s in iocodes)
                c.Items.Add(s);

            c = (DataGridViewComboBoxColumn)dgvcc["InvChangeCode"];
            foreach (string s in invcodes)
                c.Items.Add(s);   
            BuildRows();  
        }

        void BuildRows()
        {
            DataGridViewRowCollection rows = ItemIdDataGrid.Rows;
            foreach (ItemId ito in NC.App.DB.ItemIds.GetList())
            {
                string[] a = ToSimpleValueArray(ito);
                int i = rows.Add(a);
                rows[i].Cells[7].Tag = ito.declaredMass;
                rows[i].Cells[8].Tag = ito.declaredUMass;
                rows[i].Cells[9].Tag = ito.length;
            }

            // Generate a copy of the ItemId string in case the user changes it so we can tell what is a new row and what is a name change
            for (int i = 0; i < rows.Count; i++)
            {
                DataGridViewRow row = rows[i];
                if (string.IsNullOrEmpty((string)((DataGridViewTextBoxCell)row.Cells["ItemId"]).Value))
                    continue;
                else
                    // Set the row's Tag member to a copy of the ItemId string
                    row.Tag = string.Copy((string)((DataGridViewTextBoxCell)row.Cells["ItemId"]).Value);
            }

        }

        string[] ToSimpleValueArray(ItemId ito)
		{
			string[] vals = new string[10];
			vals[0] = ito.item;
			vals[1] = ito.mba;
			vals[2] = ito.material;
			vals[3] = ito.isotopics;
			vals[4] = ito.stratum;
			vals[5] = ito.inventoryChangeCode;
			vals[6] = ito.IOCode;
			vals[7] = ito.declaredMass.ToString("F3");
			vals[8] = ito.declaredUMass.ToString("F3");
			vals[9] = ito.length.ToString("F3");
			return vals;
		}

        private void OKBtn_Click(object sender, EventArgs e)
        {
            bool newrow;

            if (NewContent)
            {
                // User has added to or changed content in the table; process table row-by-row back into database
                foreach (DataGridViewRow row in ItemIdDataGrid.Rows)
                {
                    newrow = false;
                    // Grab the Item ID cell value 
                    string iname = (string)row.Cells[0].Value;
                    if (iname != null)
                    {

                        // Look for an entry in the DB for that Item ID
                        ItemId iid = NC.App.DB.ItemIds.Get(d => string.Compare(d.item, iname, false) == 0);
                        if (iid == null)
                        {
                            iid = new ItemId(iname);  // Create a new object if there is no match.
                            newrow = true;
                        }

                        // Update the ItemId object with the data from the row
                        iid.mba = (string)((DataGridViewComboBoxCell)row.Cells["MBA"]).Value;
                        iid.material = (string)((DataGridViewComboBoxCell)row.Cells["MatType"]).Value;
                        iid.isotopics = (string)((DataGridViewComboBoxCell)row.Cells["IsoId"]).Value;
                        iid.stratum = (string)((DataGridViewComboBoxCell)row.Cells["StratId"]).Value;
                        iid.inventoryChangeCode = (string)((DataGridViewComboBoxCell)row.Cells["InvChangeCode"]).Value;
                        iid.IOCode = (string)((DataGridViewComboBoxCell)row.Cells["IOCode"]).Value;

                        if (!double.TryParse((string)row.Cells["Mass"].Value, out iid.declaredMass))
                        {
                            // Value in mass column did not parse as a number; set to default?
                        }

                        if (!double.TryParse((string)row.Cells["HvyMetalUMass"].Value, out iid.declaredUMass))
                        {
                            // Value in U mass column did not parse as a number; set to default?
                        }

                        if (!double.TryParse((string)row.Cells["HeavyMetalLen"].Value, out iid.length))
                        {
                            // Value in length column did not parse as a number; set to default?
                        }
                        iid.modified = true;
                        if (newrow)
                        {
                            // This was new data, so it won't get captured unless we add it                       
                            NC.App.DB.ItemIds.GetList().Add(iid);
                        }
                    }
                }
                // Commit the in-memory data to the database
                NC.App.DB.ItemIds.SetList();
            }
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }

		private void ItemIdDataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column.Name == "Mass" || (e.Column.Name== "HvyMetalUMass") ||  (e.Column.Name== "HeavyMetalLen"))
			{
				double d1 = (double)ItemIdDataGrid.Rows[e.RowIndex1].Cells[e.Column.Name].Tag;
				double d2 = (double)ItemIdDataGrid.Rows[e.RowIndex2].Cells[e.Column.Name].Tag;
				e.SortResult = d1.CompareTo(d2);
				e.Handled = true;
			}
		}

		private void DeleteBtn_Click(object sender, EventArgs e)
        {
            List<ItemId> list = new List<ItemId>();
            foreach (DataGridViewRow row in ItemIdDataGrid.SelectedRows)
            {
                ItemId iid = NC.App.DB.ItemIds.Get((string)row.Cells["Item"].Value);
                if (iid != null)
                    list.Add(iid);
            }
            NC.App.DB.ItemIds.Delete(list);
            //I//temIdDataGrid.Rows.RemoveAt
        }

        private void ItemIdDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ItemIdDataGrid_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            // new row edited; set flag
            NewContent = true;
        }

        private void ItemIdDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // wheeee!
        }

        private void ItemIdDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // values changed; set flag
            NewContent = true;
        }
    }
}
