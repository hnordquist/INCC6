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
using DetectorDefs;
namespace NewUI
{
	using Integ = NCC.IntegrationHelpers;
	using NC = NCC.CentralizedState;

	public partial class IDDAcquireAssay : Form
    {
        //NEXT: use NumericTextBox to validate input on this dialog.  hn 5.14.2015
        AcquireHandlers ah;
        AnalysisMethods am;
        public IDDAcquireAssay()
        {
            InitializeComponent();
            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.verification;
            Text += " for detector " + ah.det.Id.DetectorName;
            FieldFiller();
			EnableTermControls();
            ToolTip mustSelect = new ToolTip();
            mustSelect.SetToolTip(StratumIdComboBox, "You must select an existing stratum.");
            mustSelect.SetToolTip(MaterialTypeComboBox, "You must select an existing material type.");
        }

        private void FieldFillerOnItemId()
        {
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            StratumIdComboBox.SelectedItem = ah.ap.stratum_id.Name;
            MBAComboBox.SelectedItem = ah.ap.mba;
            InventoryChangeCodeComboBox.SelectedItem = ah.ap.inventory_change_code;
            IOCodeComboBox.SelectedItem = ah.ap.io_code;
            DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");  
        }

        private void FieldFiller()
        {
            // Populate the UI fields with values from the local AcquireParameters object

            QCTestsCheckBox.Checked = ah.ap.qc_tests;
            PrintResultsCheckBox.Checked = ah.ap.print;
            CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            NumPassiveCyclesTextBox.Text = Format.Rend(ah.ap.num_runs);
            NumActiveCyclesTextBox.Text = Format.Rend(ah.ap.active_num_runs);
            CommentTextBox.Text = ah.ap.comment;
            CountTimeTextBox.Text = Format.Rend(ah.ap.run_count_time);
            MeasPrecisionTextBox.Text = ah.ap.meas_precision.ToString("F2");
            MinNumCyclesTextBox.Text = Format.Rend(ah.ap.min_num_runs);
            MaxNumCyclesTextBox.Text = Format.Rend(ah.ap.max_num_runs);
            DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
            DrumWeightTextBox.Text = ah.ap.drum_empty_weight.ToString("F3");
            MBAComboBox.Items.Clear();

            InventoryChangeCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.InvChangeCodes.GetList())
            {
                InventoryChangeCodeComboBox.Items.Add(desc.Name);
            }
            IOCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.IOCodes.GetList())
            {
                IOCodeComboBox.Items.Add(desc.Name);
            }
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
            {
                MBAComboBox.Items.Add(desc);
            }
            ItemIdComboBox.Items.Clear();
            foreach (ItemId id in NC.App.DB.ItemIds.GetList())
            {
                ItemIdComboBox.Items.Add(id.item);
            }
            StratumIdComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Stratums.GetList())
            {
                StratumIdComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }

            DataSourceComboBox.Items.Clear();
            foreach (ConstructedSource cs in System.Enum.GetValues(typeof(ConstructedSource)))
            {
                if (cs.AcquireChoices() || cs.LMFiles(ah.det.Id.SRType))
                {
                    DataSourceComboBox.Items.Add(cs.HappyFunName());
                }
            }

			am = Integ.GetMethodSelections(ah.ap);
            if (ah.ap.acquire_type == AcquireConvergence.CycleCount)
            {
                UseNumCyclesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.DoublesPrecision)
            {
                UseDoublesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.TriplesPrecision)
            {
                UseTriplesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.Pu240EffPrecision)
            {
                UsePu240eRadioButton.Checked = true;
            }
            DataSourceComboBox.SelectedItem = ah.ap.data_src.HappyFunName();
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            StratumIdComboBox.SelectedItem = ah.ap.stratum_id.Name;
            MBAComboBox.SelectedItem = ah.ap.mba;
            InventoryChangeCodeComboBox.SelectedItem = ah.ap.inventory_change_code;
            IOCodeComboBox.SelectedItem = ah.ap.io_code;
            ItemIdComboBox.SelectedItem = ah.ap.item_id;

            if (am != null)
            {
                if (am.Has(AnalysisMethod.CuriumRatio))
                {
                    IsotopicsBtn.Enabled = false; CompositeIsotopicsBtn.Enabled = false;
                }
                if (am.Has(AnalysisMethod.ActivePassive) || am.Has(AnalysisMethod.Collar))
                    NumActiveCyclesLabel.Visible = NumActiveCyclesTextBox.Visible = true;
                else
                    NumActiveCyclesLabel.Visible = NumActiveCyclesTextBox.Visible = false;
                if (!am.Has(AnalysisMethod.AddASource))
                {
                    DrumWeightLabel.Visible = DrumWeightTextBox.Visible = false;
                    DrumWeightTextBox.Text = ah.ap.drum_empty_weight.ToString("F3");
                }
            }

        }
        private void Pu240eCoeffBtn_Click(object sender, EventArgs e)
        {
			Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm");
        }

        private void MaterialTypeHelpBtn_Click(object sender, EventArgs e)
        {
			Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/materialtypeadddelete.htm");
        }

        private void IsotopicsBtn_Click(object sender, EventArgs e)
        {
            UpdateIsotopics(straight: true);
        }

        void UpdateIsotopics(bool straight)      // straight means from iso button
        {
            IDDIsotopics f = new IDDIsotopics(ah.ap.isotopics_id);
            DialogResult dlg = f.ShowDialog();
            if (dlg != DialogResult.OK)
                return;
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
                ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
                if (Cur == null)                         // blank or unspecified somehow
                    return;
                Cur.IsoApply(NC.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
                Cur.modified = true;
            }
        }

        private void CompositeIsotopicsBtn_Click(object sender, EventArgs e)
        {
            IDDCompositeIsotopics f = new IDDCompositeIsotopics(ah.ap.comp_isotopics_id);
            DialogResult dlg = f.ShowDialog();
            if (dlg != DialogResult.OK)
                return;
            ah.RefreshParams();
            CompositeIsotopics selected = f.GetSelectedIsotopics;
            if (ah.ap.isotopics_id != selected.id) /* They changed the isotopics id. Isotopics already saved to DB in IDDIsotopics*/
            {
                ah.ap.isotopics_id = selected.id;
                ah.ap.comp_isotopics_id = selected.id;
                // NEXT: do new item id stuff right after this
            }
            else
            {
                // isotopic settings will be loaded from DB prior to running measurement

                // change the isotopics setting on the current item id state
                ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
                if (Cur == null)                         // blank or unspecified somehow
                    return;
                Cur.IsoApply(NC.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
                NC.App.DB.ItemIds.Set(Cur);
                NC.App.DB.ItemIds.Refresh();    // save and update the list of items
            }

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
					dlgres = (new IDDCollarData().ShowDialog());
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
                // save/update item id changes only when user selects OK
                ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
                Cur.IsoApply(NC.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item

                NC.App.DB.ItemIds.Set();  // writes any new or modified item ids to the DB
				NC.App.DB.ItemIds.Refresh();    // save and update the in-memory item list 
				bool ocntinue = GetAdditionalParameters();
				if (ocntinue && (ah.OKButton_Click(sender, e) == DialogResult.OK))
				{
					Visible = false;
					// Add strata update to measurement object.    HN 9.23.2015              
					//user can cancel in here during LM set-up, account for it.
					UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
					UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
					Close();
				}
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e)
        {
            //Store any changes before exiting
            //HN 08-04-2016
            ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
            Cur.IsoApply(NC.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
            NC.App.DB.ItemIds.Set();  // writes any new or modified item ids to the DB
            NC.App.DB.ItemIds.Refresh();    // save and update the in-memory item list 

            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/verificationacquire.htm");
        }

        private void MBAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MBAComboBox_SelectedIndexChanged(sender, e);
        }

        private void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemId Cur = NC.App.DB.ItemIds.Get(d => string.Compare(d.item, ItemIdComboBox.Text, false) == 0);
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

        private void UseNumCyclesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.NumberOfCyclesRadioButton_CheckedChanged(sender, e);
			EnableTermControls();
        }

        private void UseDoublesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.DoublesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);
			EnableTermControls();
        }

        private void UseTriplesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.TriplesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);
			EnableTermControls();
        }

        private void UsePu240eRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.Pu240ePrecisionRadioButton_CheckedChanged(sender, e);
			EnableTermControls();
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

		void EnableTermControls()
		{
			switch (ah.ap.data_src)
			{
				case ConstructedSource.Live:
					UseNumCyclesRadioButton.Enabled = true;
					UseDoublesRadioButton.Enabled = true;
					UseTriplesRadioButton.Enabled = false;
					UsePu240eRadioButton.Enabled = false;
					NumPassiveCyclesTextBox.Enabled = ah.CycleCount;
					MeasPrecisionTextBox.Enabled = !ah.CycleCount;
					MinNumCyclesTextBox.Enabled = !ah.CycleCount;
					MaxNumCyclesTextBox.Enabled = !ah.CycleCount;
					CountTimeTextBox.Enabled = (ah.det.Id.SRType != InstrType.JSR11);
					if (ah.det.Id.SRType == InstrType.PSR || ah.det.Id.SRType == InstrType.AMSR || ah.det.Id.SRType == InstrType.MSR4A || ah.det.ListMode)
					{
						UseTriplesRadioButton.Enabled = true;
						if ( (am != null) && am.HasMethod(AnalysisMethod.Multiplicity))
						{
							UsePu240eRadioButton.Enabled = true;
						}
					}

					break;
				default:
					CountTimeTextBox.Enabled = false;
					UseNumCyclesRadioButton.Enabled = false;
					UseDoublesRadioButton.Enabled = false;
					UseTriplesRadioButton.Enabled = false;
					UsePu240eRadioButton.Enabled = false;

					MeasPrecisionTextBox.Enabled = false;
					MinNumCyclesTextBox.Enabled = false;
					MaxNumCyclesTextBox.Enabled = false;
					NumPassiveCyclesTextBox.Enabled = false;
				break;
			}
		}


        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);
            CountTimeTextBox.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;
            UseDoublesRadioButton.Enabled = true;
            UseTriplesRadioButton.Enabled = true;
            UsePu240eRadioButton.Enabled = true;
            CommentTextBox.Enabled = true;
            NumPassiveCyclesTextBox.Enabled = true;
            MeasPrecisionTextBox.Enabled = false;
            MinNumCyclesTextBox.Enabled = false;
            MaxNumCyclesTextBox.Enabled = false;

            CommentAtEndCheckBox.Enabled = true;
            PrintResultsCheckBox.Enabled = true;

            //enable/disable selected controls here
            switch (ah.ap.data_src)
            {
                case ConstructedSource.Live:
                    // every set as above
                    break;
                case ConstructedSource.DB:
                case ConstructedSource.CycleFile:
                case ConstructedSource.Manual:
                case ConstructedSource.ReviewFile:
                default:
                    NumPassiveCyclesTextBox.Enabled = false;
                    CountTimeTextBox.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    UseDoublesRadioButton.Enabled = false;
                    UseTriplesRadioButton.Enabled = false;
                    UsePu240eRadioButton.Enabled = false;
                    CommentAtEndCheckBox.Enabled = false;
                    //PrintResultsCheckbox.Enabled = false;
                    break;
            }

        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }

        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            ah.CountTimeTextBox_Leave(sender, e);
        }

        private void NumPassiveCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.NumCyclesTextBox_Leave(sender, e);
        }

        private void NumActiveCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.NumActiveCyclesTextBox_Leave(sender, e);
        }

        private void MeasPrecisionTextBox_Leave(object sender, EventArgs e)
        {
            ah.MeasurementPrecisionTextBox_Leave(sender, e);
        }

        private void MinNumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.MinNumCyclesTextBox_Leave(sender, e);
        }

        private void MaxNumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.MaxNumCyclesTextBox_Leave(sender, e);
        }

        private void DrumWeightTextBox_Leave(object sender, EventArgs e)
        {
            ah.DrumEmptyWeightTextBox_Leave(sender, e);
        }

        private void DeclaredMassTextBox_Leave(object sender, EventArgs e)
        {
            ah.DeclaredMassTextBox_Leave(sender, e);
            // mass changed, update the current item id associated field, but save it off only when user presses OK
            ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
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
            ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
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
            ItemId Cur = NC.App.DB.ItemIds.Get(d => String.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (Cur == null)
            {
                // new items need the isotopics specified
                UpdateIsotopics(straight: false);
                //Add the item id if not found in DB. Use the current values on the dialog for the item id content
                ah.ItemIdComboBox_Leave(sender, e);  // put the new id on the acq helper
                Cur = ah.ap.ItemId;  // the dlg and acq parms are the same, use the ItemId helper to construct. Create a new object if there is no match.
                ah.ap.mass = 0;
                Cur.declaredMass = 0;
                Cur.modified = true;
                Cur.IsoApply(NC.App.DB.Isotopics.Get(ah.ap.isotopics_id));           // apply the iso dates to the item
                NC.App.DB.ItemIds.GetList().Add(Cur);   // add only to in-memory, save to DB on user OK/exit

                FieldFillerOnItemId();  // refresh the dialog

                ItemIdComboBox.Items.Add(ItemIdComboBox.Text);  // rebuild combo box and select the new time
                ItemIdComboBox.Items.Clear();
                foreach (ItemId id in NC.App.DB.ItemIds.GetList())
                {
                    ItemIdComboBox.Items.Add(id.item);
                }
            }
        }
    }
}
