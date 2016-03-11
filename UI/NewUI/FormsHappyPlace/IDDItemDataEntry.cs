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
        List<String> mbas, isotopics, mats, strats, iocodes, invcodes;
        List<ItemId> newIDs;

        // Flag set if user enters new data 
        Boolean NewContent = false;

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
            newIDs = new List<ItemId>();
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

            DataGridViewRowCollection rows = this.ItemIdDataGrid.Rows;
            foreach (ItemId ito in NC.App.DB.ItemIds.GetList())
            {
                string[] a = ito.ToSimpleValueArray();
                rows.Add(a);
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

        private void OKBtn_Click(object sender, EventArgs e)
        {
            bool newrow;

            if (NewContent)
            {
                // User has added to or changed content in the table; process table row-by-row back into database
                foreach (DataGridViewRow row in this.ItemIdDataGrid.Rows)
                {
                    newrow = false;
                    //bool changedName = false;

                    // Grab the Item ID cell value 
                    string i = (string)row.Cells[0].Value;

                    if (i != null)
                    {
                        // See if this row's ItemId string was changed
                        //if (row.Tag != null && i.Equals(row.Tag.ToString(), StringComparison.InvariantCultureIgnoreCase)) 
                        //{
                        //    changedName = true;
                        //}

                        // Look for an entry in the DB for that Item ID
                        ItemId iid = NC.App.DB.ItemIds.Get(d => String.Compare(d.item, i, false) == 0);
                        if (iid == null)
                        {
                            iid = new ItemId(i);  // Create a new object if there is no match.
                            newrow = true;
                        }

                        // Update the ItemId object with the data from the row
                        iid.mba = (string)((DataGridViewComboBoxCell)row.Cells["MBA"]).Value;
                        iid.material = (string)((DataGridViewComboBoxCell)row.Cells["MatType"]).Value;
                        iid.isotopics = (string)((DataGridViewComboBoxCell)row.Cells["IsoId"]).Value;
                        iid.stratum = (string)((DataGridViewComboBoxCell)row.Cells["StratId"]).Value;
                        iid.inventoryChangeCode = (string)((DataGridViewComboBoxCell)row.Cells["InvChangeCode"]).Value;
                        iid.IOCode = (string)((DataGridViewComboBoxCell)row.Cells["IOCode"]).Value;

                        if (!Double.TryParse((string)row.Cells["Mass"].Value, out iid.declaredMass))
                        {
                            // Value in mass column did not parse as a number; set to default?
                        }

                        if (!Double.TryParse((string)row.Cells["HvyMetalUMass"].Value, out iid.declaredUMass))
                        {
                            // Value in U mass column did not parse as a number; set to default?
                        }

                        if (!Double.TryParse((string)row.Cells["HeavyMetalLen"].Value, out iid.length))
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
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {

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
