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
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

	public partial class IDDReanalysisAssay : Form
    {
        AcquireHandlers ah;
        AnalysisMethods am;
		Measurement meas;
		// local for this specifc dlg
		VTuple norm;
		bool normmodified;
        public IDDReanalysisAssay(Measurement m, Detector det)
        {
            InitializeComponent();
            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers(m.AcquireState, det);
            ah.mo = AssaySelector.MeasurementOption.verification;
            Text = "Select measurement for detector " + ah.det.Id.DetectorName;
            toolTip1.SetToolTip(StratumIdComboBox, "You must select an existing stratum.");
            toolTip1.SetToolTip(MaterialTypeComboBox, "You must select an existing material type.");
            toolTip1.SetToolTip(UseCurrentCalibCheckBox, "x = use current calibration parameters for the selected material type.\r\nblank = use calibration parameters from the original measurement.");
			normmodified = false;
			meas = m;
            norm = new VTuple(meas.Norm.currNormalizationConstant);  // fill from m
            FieldFiller();
        }

        private void FieldFillerOnItemId()
        {
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            StratumIdComboBox.SelectedItem = ah.ap.stratum_id;
            InventoryChangeCodeComboBox.SelectedItem = ah.ap.inventory_change_code;
            IOCodeComboBox.SelectedItem = ah.ap.io_code;
            DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");  
        }

        private void FieldFiller()
        {
            // Populate the UI fields with values from the local AcquireParameters object
			UseCurrentCalibCheckBox.Checked = true;
			ReplaceOriginalCheckBox.Checked = false;
			QCTestsCheckBox.Checked = ah.ap.qc_tests;
			PrintResultsCheckBox.Checked = ah.ap.print;
			CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
			CommentTextBox.Text = ah.ap.comment;
			DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
			NormConst.Text = norm.v.ToString("F4");
			NormConstErr.Text = norm.err.ToString("F4");
            dateTimePicker1.Value = new DateTime(meas.MeasDate.Ticks);

            InventoryChangeCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in N.App.DB.InvChangeCodes.GetList())
            {
                InventoryChangeCodeComboBox.Items.Add(desc.Name);
            }
            IOCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in N.App.DB.IOCodes.GetList())
            {
                IOCodeComboBox.Items.Add(desc.Name);
            }

            ItemIdComboBox.Items.Clear();
            foreach (ItemId id in N.App.DB.ItemIds.GetList())
            {
                ItemIdComboBox.Items.Add(id.item);
            }
            StratumIdComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in N.App.DB.Stratums.GetList())
            {
                StratumIdComboBox.Items.Add(desc);
            }
            MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in N.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }

            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            StratumIdComboBox.SelectedItem = ah.ap.stratum_id;
            InventoryChangeCodeComboBox.SelectedItem = ah.ap.inventory_change_code;
            IOCodeComboBox.SelectedItem = ah.ap.io_code;
            ItemIdComboBox.SelectedItem = ah.ap.item_id;

            am = Integ.GetMethodSelections(ah.ap);
            if (am != null)
            {
                if (am.Has(AnalysisMethod.CuriumRatio))
                {
                    IsotopicsBtn.Enabled = false; BackgroundBtn.Enabled = false;
                }
            }
        }

        private void IsotopicsBtn_Click(object sender, EventArgs e)
        {
            Isotopics selected = UpdateIsotopics(straight: true);
			if (selected != null && selected.CompareTo(meas.Isotopics) != 0) // copy any changes to the measurement
			{
				meas.Isotopics.Copy(selected);
				meas.MeasurementId.Item.IsoApply(selected);           // apply the pssibly new iso dates to the item
			}
        }

        Isotopics UpdateIsotopics(bool straight)      // straight means from iso button
        {
            IDDIsotopics f = new IDDIsotopics(ah.ap.isotopics_id);
            DialogResult dlg = f.ShowDialog();
            if (dlg != DialogResult.OK)
                return null;
            ah.RefreshParams();
            Isotopics selected = f.GetSelectedIsotopics;
            if (ah.ap.isotopics_id != selected.id) /* They changed the isotopics id. Isotopics already saved to DB in IDDIsotopics*/
            {
                ah.ap.isotopics_id = selected.id;
                // NEXT: do new item id stuff right after this
            }
            else if (straight)     // same iso name from iso selector but params might have changed, new item id application update occurs elsewhere
            {
                // isotopic settings will be loaded from DB prior to running measurement

                // change the isotopics setting on the current item id state
                ItemId Cur = N.App.DB.ItemIds.Get(ah.ap.item_id);
                if (Cur == null)                         // blank or unspecified somehow
                    return null;
                Cur.IsoApply(N.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
                Cur.modified = true;
            }
			return selected;
        }

		private bool GetAdditionalParameters()
		{
			bool res = false;

			AnalysisMethods am = Integ.GetMethodSelections(ah.ap);
			if (am != null)
			{
				DialogResult dlgres = DialogResult.OK;
				// if Verif + cal curve get U235 percent/get heavy metal data
				if (am.Has(AnalysisMethod.CalibrationCurve))
				{
					INCCAnalysisParams.cal_curve_rec cal_curve = (INCCAnalysisParams.cal_curve_rec)am.GetMethodParameters(AnalysisMethod.CalibrationCurve);
					if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.U)
						dlgres = (new IDDPercentU235(am, cal_curve).ShowDialog());
					else if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.HM)
						dlgres = (new IDDHeavyMetalItemData(am, ah.ap.ItemId).ShowDialog());
				}
				// if Verif + (cal curve or KA) get heavy metal data
				if (am.Has(AnalysisMethod.KnownA))
				{
					INCCAnalysisParams.known_alpha_rec ka = (INCCAnalysisParams.known_alpha_rec)am.GetMethodParameters(AnalysisMethod.KnownA);
					if (ka.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection)
						dlgres = (new IDDHeavyMetalItemData(am, ah.ap.ItemId).ShowDialog());
				}
				// if Verif + collar  get collar data
				if (am.Has(AnalysisMethod.Collar))
				{
					dlgres = (new IDDCollarData().ShowDialog()); // todo: implement collar data dlg
				}
				// if Verif + curium ratio, get cm_pu_ratio w dlg; 
				if (am.Has(AnalysisMethod.CuriumRatio))
				{
					dlgres = (new IDDCmPuRatio(ah.ap).ShowDialog());
				}
				res = (dlgres == DialogResult.OK); 
			} else
			{
				MessageBox.Show(string.Format("No analysis methods specified for detector {0} and material {1}", ah.ap.detector_id, ah.ap.item_type),
					"Verification", MessageBoxButtons.OK);
			}
			return res;
		}

		private void OKBtn_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(ItemIdComboBox.Text))
				MessageBox.Show("You must enter an item id for this assay.", "ERROR");
			else
			{
                Integ.FillOutMeasurement(meas);  
                if (normmodified)
					meas.Norm.currNormalizationConstant = norm;
				// save/update item id changes only when user selects OK
				N.App.DB.ItemIds.Set();  // writes any new or modified item ids to the DB
				N.App.DB.ItemIds.Refresh();    // save and update the in-memory item list 
				bool keepgoing = GetAdditionalParameters();
                if (keepgoing)
                {
                    ah.ap.data_src = DetectorDefs.ConstructedSource.Æther;
                    if (ah.OKButton_Click(sender, e) == DialogResult.OK)
				    {
                        N.App.Opstate.Measurement = meas;
                        Visible = false;
                        UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                        UIIntegration.Controller.Perform();  // start the thread
                        Close();
                    }
                }
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
			Help.ShowHelp(null, ".\\inccuser.chm");
        }

        private void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemId Cur = N.App.DB.ItemIds.Get(d => string.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (Cur == null)
            {
                return;
            }
        // Copy the value back after an existing item id is selected from the combo box
        //    else /*if (iid.item != ah.ap.item_id)*/ // This did not catch situation in which isotopics had changed for item. hn 5.12.2015
        //    {
                //existing item id.....just update hn 5.14.2015
                ah.ap.ApplyItemId(Cur);
                FieldFillerOnItemId();
        //    }

        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MaterialTypeComboBox_SelectedIndexChanged(sender, e);
        }
  
        private void QCTestsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.QCTestsCheckbox_CheckedChanged(sender, e);
        }

        private void PrintResultsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.PrintResultsCheckbox_CheckedChanged(sender, e);
        }

        private void CommentAtEndCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.CommentCheckbox_CheckedChanged(sender, e);
        }

        private void InventoryChangeCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.InvChangeCodeComboBox_SelectedIndexChanged(sender, e);
        }

        private void IOCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.IOCodeComboBox_SelectedIndexChanged(sender, e);
        }
   
        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }


        private void DeclaredMassTextBox_Leave(object sender, EventArgs e)
        {
            ah.DeclaredMassTextBox_Leave(sender, e);
            // mass changed, update the current item id associated field, but save it off only when user presses OK
            ItemId Cur = N.App.DB.ItemIds.Get(ah.ap.item_id);
            if (Cur == null)                         // blank or unspecified somehow
                return;
            if (Cur.declaredMass != ah.ap.mass)
            {
                Cur.declaredMass = ah.ap.mass;
                Cur.modified = true;
            }
        }

        private void StratumIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.StratumIdComboBox_SelectedIndexChanged(sender, e);
            // stratum id changes, update the current item id associated field, but save it off only when user presses OK
            ItemId Cur = N.App.DB.ItemIds.Get(ah.ap.item_id);
            if (Cur == null)                         // blank or unspecified somehow
                return;
            if (Cur.stratum.CompareTo(ah.ap.stratum_id.Name) != 0)  // id changed, save it on the item 
            {
                Cur.stratum = string.Copy(ah.ap.stratum_id.Name);
                Cur.modified = true;
            }
        }

        private void ItemIdComboBox_Leave(object sender, EventArgs e)
        {  
            // save new item id to DB only when user selects OK, save to in-memory list for transient use
            ItemId Cur = N.App.DB.ItemIds.Get(d => String.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (Cur == null)
            {
                // new items need the isotopics specified
                UpdateIsotopics(straight: false);
                //Add the item id if not found in DB. Use the current values on the dialog for the item id content
                ah.ItemIdComboBox_Leave(sender, e);  // put the new id on the acq helper
                Cur = ah.ap.ItemId;  // the dlg and acq parms are the same, use the ItemId helper to construct. Create a new object if there is no match.
                Cur.IsoApply(N.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
                N.App.DB.ItemIds.GetList().Add(Cur);   // add only to in-memory, save to DB on user OK/exit

                FieldFillerOnItemId();  // refresh the dialog

                ItemIdComboBox.Items.Add(ItemIdComboBox.Text);  // rebuild combo box and select the new time
                ItemIdComboBox.Items.Clear();
                foreach (ItemId id in N.App.DB.ItemIds.GetList())
                {
                    ItemIdComboBox.Items.Add(id.item);
                }
            }
        }

        private void MaterialTypeComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void StratumIdComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void ItemIdComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

		private void UseCurrentCalibCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void ReplaceOriginalCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

        private void calendarEditingControl1_ValueChanged(object sender, EventArgs e)
        {

        }

		private void NormConstErr_Leave(object sender, EventArgs e)
		{
            double d = norm.err;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 100));
            if (modified) { norm.err = d; normmodified = true; }
            ((TextBox)sender).Text = norm.err.ToString("F4");
		}

		private void NormConst_Leave(object sender, EventArgs e)
		{
            double d = norm.v;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 100));
            if (modified) { norm.v = d; normmodified = true; }
            ((TextBox)sender).Text = norm.v.ToString("F4");
		}

		private void BackgroundBtn_Click(object sender, EventArgs e)
		{
            IDDBackgroundSetup f = new IDDBackgroundSetup(ah.det, meas.Background);
            if (f.ShowDialog() == DialogResult.OK)
			{
				f.GetBP.CopyTo(meas.Background);
			}
		}
	}

 
}
