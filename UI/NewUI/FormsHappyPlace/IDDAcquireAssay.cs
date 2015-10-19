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
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;
using System.Collections.Generic;
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
            this.Text += " for detector " + ah.det.Id.DetectorName;
            FieldFiller();
            //this.SetDesktopLocation(this.Parent.DisplayRectangle.X + this.Parent.DisplayRectangle.Width + 10, this.Parent.DisplayRectangle.Y);
            ToolTip mustSelect = new ToolTip();
            mustSelect.SetToolTip(StratumIdComboBox, "You must select an existing stratum.");
            mustSelect.SetToolTip(MaterialTypeComboBox, "You must select an existing material type.");
        }

        private void FieldFiller()
        {
            // Populate the UI fields with values from the local AcquireParameters object

            this.QCTestsCheckBox.Checked = ah.ap.qc_tests;
            this.PrintResultsCheckBox.Checked = ah.ap.print;
            this.CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            this.NumPassiveCyclesTextBox.Text = Format.Rend(ah.ap.num_runs);
            this.NumActiveCyclesTextBox.Text = Format.Rend(ah.ap.active_num_runs);
            this.CommentTextBox.Text = ah.ap.comment;
            this.CountTimeTextBox.Text = Format.Rend(ah.ap.run_count_time);
            this.MeasPrecisionTextBox.Text = ah.ap.meas_precision.ToString("F2");
            this.MinNumCyclesTextBox.Text = Format.Rend(ah.ap.min_num_runs);
            this.MaxNumCyclesTextBox.Text = Format.Rend(ah.ap.max_num_runs);
            this.DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
            this.DrumWeightTextBox.Text = ah.ap.drum_empty_weight.ToString("F3");
            this.MBAComboBox.Items.Clear();

            InventoryChangeCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.InvChangeCodes.GetList())
            {
                this.InventoryChangeCodeComboBox.Items.Add(desc.Name);
            }
            IOCodeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.IOCodes.GetList())
            {
                this.IOCodeComboBox.Items.Add(desc.Name);
            }
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
            {
                MBAComboBox.Items.Add(desc);
            }
            this.ItemIdComboBox.Items.Clear();
            foreach (ItemId id in NC.App.DB.ItemIds.GetList())
            {
                ItemIdComboBox.Items.Add(id.item);
            }
            this.StratumIdComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Stratums.GetList())
            {
                StratumIdComboBox.Items.Add(desc);
            }
            this.MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }

            this.DataSourceComboBox.Items.Clear();
            foreach (ConstructedSource cs in System.Enum.GetValues(typeof(ConstructedSource)))
            {
                if (cs.AcquireChoices() || cs.LMFiles(ah.det.Id.SRType))
                {
                    DataSourceComboBox.Items.Add(cs.HappyFunName());
                }
            }

            if (ah.ap.acquire_type == AcquireConvergence.CycleCount)
            {
                this.UseNumCyclesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.DoublesPrecision)
            {
                this.UseDoublesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.TriplesPrecision)
            {
                this.UseTriplesRadioButton.Checked = true;
            }
            else if (ah.ap.acquire_type == AcquireConvergence.Pu240EffPrecision)
            {
                this.UsePu240eRadioButton.Checked = true;
            }
            DataSourceComboBox.SelectedItem = ah.ap.data_src.HappyFunName();
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            StratumIdComboBox.SelectedItem = ah.ap.stratum_id;
            MBAComboBox.SelectedItem = ah.ap.mba;
            InventoryChangeCodeComboBox.SelectedItem = ah.ap.inventory_change_code;
            IOCodeComboBox.SelectedItem = ah.ap.io_code;
            ItemIdComboBox.SelectedItem = ah.ap.item_id;

            am = Integ.GetMethodSelections(ah.ap);
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
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm");
        }

        private void MaterialTypeHelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/materialtypeadddelete.htm");
        }

        private void IsotopicsBtn_Click(object sender, EventArgs e)
        {
            IDDIsotopics f = new IDDIsotopics(ah.ap.isotopics_id);
            DialogResult dlg = f.ShowDialog();
            //check to see if they changed the isotopic id hn 5.12.2015
            Isotopics selected = f.GetSelectedIsotopics;
            if (ah.ap.isotopics_id != selected.id) /* They changed the isotopics id.  Isotopics already saved to DB in IDDIsotopics*/
            {
                selected.modified = true;
                NC.App.DB.Isotopics.Set(selected);
                ItemId Cur = NC.App.DB.ItemIds.Get(ah.ap.item_id);
                Cur.isotopics = selected.id;
                Cur.modified = true;
                //force item for new id.... hn 5.13.2015
                NC.App.DB.ItemIds.Set(Cur);
                ah.ap.ApplyItemId(Cur);
                ah.ap.isotopics_id = selected.id;
            }
            else
            {
                //refresh isotopic settings?? Will these now load from DB prior to running measurement?  Let's see hn 5.13.2015
            }
            
        }

        private void CompositeIsotopicsBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implement, but we are working on it.");
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (ItemIdComboBox.Text == String.Empty)
                MessageBox.Show("You must enter an item id for this assay.", "ERROR");
            else
            {
                if (ah.OKButton_Click(sender, e) == System.Windows.Forms.DialogResult.OK)
                {
                    //This is fubar. Must save changed parameters before running analysis. HN 9.10.2015
                    AnalysisMethods am = Integ.GetMethodSelections(ah.ap);
                    if (am != null)
                    {

                        // NEXT: implement these choices, needs dialogs
                        // if Verif + cal curve get U235 percent
                        // if Verif + (cal curve or KA) get heavy metal data
                        // if Verif + collar  get collar data
                        // if Verif + curium ratio, get cm_pu_ratio w dlg; 
                        if (am.Has(AnalysisMethod.CuriumRatio))
                        {
                            new IDDCmPuRatio(ah.ap).ShowDialog();
                        }
                        this.Visible = false;
                        // Add strata update to measurement object.    HN 9.23.2015              

                        //user can cancel in here during LM set-up, account for it.
                        UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                        UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(String.Format("No analysis methods specified for detector {0} and material {1}", ah.ap.detector_id, ah.ap.item_type),
                            "Verification", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm");
        }

        private void MBAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MBAComboBox_SelectedIndexChanged(sender, e);
        }

        private void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemId iid = NC.App.DB.ItemIds.Get(d => String.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (iid == null)
            {
                //Add the item id if not found in DB.
                iid = new ItemId(ItemIdComboBox.Text);  // Create a new object if there is no match.
                iid.stratum = StratumIdComboBox.Text;
                iid.modified = true;

                NC.App.DB.ItemIds.Set(iid); 

                //Again, check "item_id" issues. What if they switch item_id for previously measured item hn 9.10.2015
                ItemIdComboBox.Items.Add(ItemIdComboBox.Text);
                ah.ap.isotopics_id = iid.isotopics;
                ah.ap.ApplyItemId(iid); // set the item Id and let them pick the isotopics.... hn 5.14.2015
                IsotopicsBtn_Click(sender, e);

                FieldFiller();
                // Missing step of adding declared mass from item..... hn 5.13.2015
                DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
            }
            else /*if (iid.item != ah.ap.item_id)*/ // This did not catch situation in which isotopics had changed for item. hn 5.12.2015
            {
                //existing item id.....just update hn 5.14.2015
                ah.ap.isotopics_id = iid.isotopics;
                iid.stratum = StratumIdComboBox.Text;
                ah.ap.ApplyItemId(iid);
                DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
                NC.App.DB.ItemIds.Set(iid);
            }

        }

        private void StratumIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Was not storying stratum id. hn 9.21.2015
            ah.StratumIdComboBox_SelectedIndexChanged(sender, e);
            int itemIndex = ((ComboBox)sender).SelectedIndex;
            ah.ap.ItemId.stratum = ((ComboBox)sender).Items[itemIndex].ToString();
            ItemId iid = NC.App.DB.ItemIds.Get(d => String.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (iid != null)
            {
                iid.stratum = ah.ap.ItemId.stratum;
                iid.modified = true;
                NC.App.DB.ItemIds.Set(iid);
                ah.ap.ApplyItemId(iid);
            }
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Was not storying material type. hn 9.21.2015
            ah.MaterialTypeComboBox_SelectedIndexChanged(sender, e);
            int itemIndex = ((ComboBox)sender).SelectedIndex;
            ah.ap.item_type = ((ComboBox)sender).Items[itemIndex].ToString();
        }

        private void UseNumCyclesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.NumberOfCyclesRadioButton_CheckedChanged(sender, e);
        }

        private void UseDoublesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.DoublesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);
        }

        private void UseTriplesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.TriplesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);
        }

        private void UsePu240eRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.Pu240ePrecisionRadioButton_CheckedChanged(sender, e);
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

        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);
            CountTimeTextBox.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;
            UseDoublesRadioButton.Enabled = true;
            UseTriplesRadioButton.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;
            CommentTextBox.Enabled = true;

            //only verification uses these
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
                    CountTimeTextBox.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    UseDoublesRadioButton.Enabled = false;
                    UseTriplesRadioButton.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    break;
                case ConstructedSource.CycleFile:
                    CountTimeTextBox.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    UseDoublesRadioButton.Enabled = false;
                    UseTriplesRadioButton.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    CommentAtEndCheckBox.Enabled = false;
                    break;
                case ConstructedSource.Manual:
                    CountTimeTextBox.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    UseDoublesRadioButton.Enabled = false;
                    UseTriplesRadioButton.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    CommentAtEndCheckBox.Enabled = false;
                    break;
                case ConstructedSource.ReviewFile:
                default:
                    CountTimeTextBox.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    UseDoublesRadioButton.Enabled = false;
                    UseTriplesRadioButton.Enabled = false;
                    UseNumCyclesRadioButton.Enabled = false;
                    CommentAtEndCheckBox.Enabled = false;
                    //PrintResultsCheckbox.Enabled = false;
                    break;
            }

        }

        private void DeclaredMassTextBox_Leave(object sender, EventArgs e)
        {
            ah.DeclaredMassTextBox_Leave(sender, e);
            ItemId iid = NC.App.DB.ItemIds.Get(d => String.Compare(d.item, ItemIdComboBox.Text, false) == 0);
            if (iid != null)
            {
                iid.declaredMass = Double.Parse(DeclaredMassTextBox.Text);
                iid.modified = true;
                NC.App.DB.ItemIds.Set(iid);
                ah.ap.ApplyItemId(iid);
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

        private void ItemIdComboBox_Leave(object sender, EventArgs e)
        {
            ItemIdComboBox_SelectedIndexChanged(sender, e);
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
    }
}
