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

	using NC = NCC.CentralizedState;
	public partial class IDDCollarData : Form
	{
		// Flag set if user enters new data 
		bool NewContent = false;
		public bool EditItem;  // flag for invoking item data entry for new collar item
		public IDDCollarData()
		{
			InitializeComponent();
			DataGridViewColumnCollection dgvcc = ItemIdDataGrid.Columns;
			DataGridViewComboBoxColumn c = (DataGridViewComboBoxColumn)dgvcc["RodType"];
			foreach (poison_rod_type_rec pr in NC.App.DB.PoisonRods.GetList())
				c.Items.Add(pr.rod_type);
			BuildRows();
			EditItem = false;
		}

		void BuildRows()
		{
			DataGridViewRowCollection rows = ItemIdDataGrid.Rows;
			foreach (CollarItemId cid in NC.App.DB.CollarItemIds.GetList())
			{
				string[] a = ToSimpleValueArray(cid);
				int i = rows.Add(a);
				rows[i].Cells[1].Tag = cid.length.v;
				rows[i].Cells[2].Tag = cid.length.err;
				rows[i].Cells[3].Tag = cid.total_u235.v;
				rows[i].Cells[4].Tag = cid.total_u235.err;
				rows[i].Cells[5].Tag = cid.total_u238.v;
				rows[i].Cells[6].Tag = cid.total_u238.err;
				rows[i].Cells[7].Tag = cid.total_rods;
				rows[i].Cells[8].Tag = cid.total_poison_rods;
				rows[i].Cells[9].Tag = cid.poison_percent.v;
				rows[i].Cells[10].Tag = cid.poison_percent.err;
			}

			// Generate a copy of the ItemId string in case the user changes it so we can tell what is a new row and what is a name change
			for (int i = 0; i < rows.Count; i++)
			{
				DataGridViewRow row = rows[i];
				if (string.IsNullOrEmpty((string)((DataGridViewTextBoxCell)row.Cells["Item"]).Value))
					continue;
				else
					// Set the row's Tag member to a copy of the ItemId string
					row.Tag = string.Copy((string)((DataGridViewTextBoxCell)row.Cells["Item"]).Value);
			}

		}

		string[] ToSimpleValueArray(CollarItemId cid)
		{
			string[] vals = new string[12];
			vals[0] = cid.item_id;
			vals[1] = cid.length.v.ToString("F3");
			vals[2] = cid.length.err.ToString("F3");
			vals[3] = cid.total_u235.v.ToString("F3");
			vals[4] = cid.total_u235.err.ToString("F3");
			vals[5] = cid.total_u238.v.ToString("F3");
			vals[6] = cid.total_u238.err.ToString("F3");
			vals[7] = ((int)cid.total_rods).ToString();
			vals[8] = ((int)cid.total_poison_rods).ToString();
			vals[9] = cid.poison_percent.v.ToString("F3");
			vals[10] = cid.poison_percent.err.ToString("F3");
			vals[11] = cid.rod_type;
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
					string ciname = (string)row.Cells[0].Value;  // or ["Item"]
					if (ciname != null)
					{
						if (!NC.App.DB.ItemIds.Has(ciname))
						{
							EditItem = true;
							ItemId nid = new ItemId(ciname);
							nid.modified = true;
							NC.App.DB.ItemIds.GetList().Add(nid);
						}

						// Look for an entry in the DB for that Collar Item ID
						CollarItemId cid = NC.App.DB.CollarItemIds.Get(d => string.Compare(d.item_id, ciname, false) == 0);
						if (cid == null)
						{
							cid = new CollarItemId(ciname);  // Create a new object if there is no match.
							newrow = true;
						}

						// Update the CollarItemId object with the data from the row
						cid.rod_type = (string)((DataGridViewComboBoxCell)row.Cells["RodType"]).Value;

						if (!double.TryParse((string)row.Cells["Length"].Value, out cid.length.v))
						{
						}
						if (!double.TryParse((string)row.Cells["LengthErr"].Value, out cid.length.sigma))
						{
						}

						if (!double.TryParse((string)row.Cells["TotU235"].Value, out cid.total_u235.v))
						{
						}
						if (!double.TryParse((string)row.Cells["TotU235Err"].Value, out cid.total_u235.sigma))
						{
						}

						if (!double.TryParse((string)row.Cells["TotU238"].Value, out cid.total_u238.v))
						{
						}
						if (!double.TryParse((string)row.Cells["TotU238Err"].Value, out cid.total_u238.sigma))
						{
						}

						if (!double.TryParse((string)row.Cells["TotRods"].Value, out cid.total_rods))
						{
						}
						if (!double.TryParse((string)row.Cells["TotPoisonRods"].Value, out cid.total_poison_rods))
						{
						}

						if (!double.TryParse((string)row.Cells["PoiPct"].Value, out cid.poison_percent.v))
						{
						}
						if (!double.TryParse((string)row.Cells["PoiPctErr"].Value, out cid.poison_percent.sigma))
						{
						}
						cid.modified = true;
						if (newrow)
						{
							// This was new data, so it won't get captured unless we add it                       
							NC.App.DB.CollarItemIds.GetList().Add(cid);
						}
					}
				}
				// Commit the in-memory data to the database
				NC.App.DB.CollarItemIds.SetList();
				NC.App.DB.CollarItemIds.Refresh(); // regen list from new db content
			}
			if (EditItem)
			{
				NC.App.DB.ItemIds.SetList();
				NC.App.DB.ItemIds.Refresh(); // regen ItemId list from new db content
			}
			DialogResult = DialogResult.OK;
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

		private void DeleteBtn_Click(object sender, EventArgs e)
		{
			List<CollarItemId> list = new List<CollarItemId>();
			foreach (DataGridViewRow row in ItemIdDataGrid.SelectedRows)
			{
				CollarItemId cid = NC.App.DB.CollarItemIds.Get((string)row.Cells["Item"].Value); // or index 0
				if (cid != null)
					list.Add(cid);
			}
			if (list.Count < 1)
				return;
			NC.App.DB.CollarItemIds.Delete(list);
			NC.App.DB.CollarItemIds.Refresh();
			foreach (DataGridViewRow row in ItemIdDataGrid.SelectedRows)
			{
				int idx = ItemIdDataGrid.Rows.IndexOf(row);
				ItemIdDataGrid.Rows.RemoveAt(idx);
			}
		}

		private void ItemIdDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			// wheeee!
		}

		private void ItemIdDataGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			// new row edited; set flag
			NewContent = true;
		}

		private void ItemIdDataGrid_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			// new row edited; set flag
			NewContent = true;
		}

		private void ItemIdDataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (!(e.Column.Name.Equals("RodType") || e.Column.Name.Equals("Item")))
			{
				if (ItemIdDataGrid.Rows[e.RowIndex1].Cells[e.Column.Name].Tag == null ||
					ItemIdDataGrid.Rows[e.RowIndex2].Cells[e.Column.Name].Tag == null)
				{
					e.SortResult = 0;
				} else
				{
					double d1 = (double)ItemIdDataGrid.Rows[e.RowIndex1].Cells[e.Column.Name].Tag;
					double d2 = (double)ItemIdDataGrid.Rows[e.RowIndex2].Cells[e.Column.Name].Tag;
					e.SortResult = d1.CompareTo(d2);
				}
				e.Handled = true;
			}
		}
	}
}
