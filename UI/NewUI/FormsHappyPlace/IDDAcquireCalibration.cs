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
    public partial class IDDAcquireCalibration : Form
    {
        AcquireHandlers ah;

        public IDDAcquireCalibration()
        {
            InitializeComponent();
            
            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.calibration;
            Text += " for detector " + ah.det.Id.DetectorName;

            NumCyclesTextBox.ToValidate = NumericTextBox.ValidateType.Integer;
            NumCyclesTextBox.NumberFormat = NumericTextBox.Formatter.NONE;

            CountTimeTextBox.ToValidate = NumericTextBox.ValidateType.Integer;

            MeasPrecisionTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            MeasPrecisionTextBox.NumberFormat = NumericTextBox.Formatter.F2;

            MinNumCyclesTextBox.ToValidate = NumericTextBox.ValidateType.Integer;
            MinNumCyclesTextBox.NumberFormat = NumericTextBox.Formatter.NONE;

            MaxNumCyclesTextBox.ToValidate = NumericTextBox.ValidateType.Integer;
            MinNumCyclesTextBox.NumberFormat = NumericTextBox.Formatter.NONE;
            LoadLists();
            FieldFiller();
            Pu240eCoeffBtn.Enabled = !string.IsNullOrEmpty(NC.App.Config.VersionBranchString);
        }

        private void LoadLists ()
        {
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            this.ItemIdComboBox.Items.Clear();
            foreach (ItemId id in NC.App.DB.ItemIds.GetList())
            {
                ItemIdComboBox.Items.Add(id.item);
            }
            this.DataSourceComboBox.Items.Clear();
            foreach (ConstructedSource cs in System.Enum.GetValues(typeof(ConstructedSource)))
            {
                if (cs.AcquireChoices() || cs.LMFiles(ah.det.Id.SRType))
                    DataSourceComboBox.Items.Add(cs.NameForViewing(ah.det.Id.SRType));
            }
        }
        private void FieldFiller ()
        {
            // Populate the UI fields with values from the local AcquireParameters object
            this.QCTestsCheckBox.Checked = ah.ap.qc_tests;
            this.PrintResultsCheckBox.Checked = ah.ap.print;
            this.CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            this.NumCyclesTextBox.Value = ah.ap.num_runs;
            this.CommentTextBox.Text = ah.ap.comment;
            this.CountTimeTextBox.Value = ah.ap.run_count_time;
            this.MeasPrecisionTextBox.Value = ah.ap.meas_precision;
            this.MinNumCyclesTextBox.Value = ah.ap.min_num_runs;
            this.MaxNumCyclesTextBox.Value = ah.ap.max_num_runs;

            this.DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
            
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
            DataSourceComboBox.SelectedItem = ah.ap.data_src.NameForViewing(ah.det.Id.SRType);
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            ItemIdComboBox.SelectedItem = ah.ap.item_id;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.Cancel;
            /* build message to warn user about selected analysis methods           and requirements for calibration of known alpha. */
            AnalysisMethods am = Integ.GetMethodSelections(ah.ap);
            if (am == null || !am.CalibrationAnalysisSelected())
                MessageBox.Show("Warning - no analysis methods selected for calibration.", "Check calibration methods", MessageBoxButtons.OKCancel);
            else
            {
                string a = String.Format("These analysis methods are selected for calibration of material type {0} for detector {1}",
                        ah.ap.item_type,
                        ah.ap.detector_id);
                if (am.Has(AnalysisMethod.CalibrationCurve))
                    a += "\r\n" + " Calibration Curve";
                if (am.Has(AnalysisMethod.Active))
                    a += "\r\n" + " Active";
                if (am.Has(AnalysisMethod.AddASource))
                    a += "\r\n" + " Add-A-Source";
                if (am.Has(AnalysisMethod.KnownA))
                {
                    a += "\r\n" + " Known Alpha";
                    a += "\r\n Known alpha calibration requires correct values for alpha weight and rho zero.";
                }
                a += "\r\n" + String.Format("Declared mass for item {0} = {1}\r\n", ah.ap.item_id, ah.ap.mass.ToString("F4"));
                res = MessageBox.Show(a, "Check calibration methods", MessageBoxButtons.OKCancel);
            }
            if (res != System.Windows.Forms.DialogResult.OK)
                return;

            if (DeclaredMassTextBox.Value != ah.ap.mass)
            {
                ah.ap.mass = DeclaredMassTextBox.Value; ah.ap.modified = true;
            }
            if (CountTimeTextBox.Value != ah.ap.run_count_time)
            {
                ah.ap.run_count_time = CountTimeTextBox.Value; ah.ap.modified = true;
            }
            if ((ushort)NumCyclesTextBox.Value != ah.ap.num_runs)
            {
                ah.ap.num_runs = (ushort)NumCyclesTextBox.Value; ah.ap.modified = true;
            }
            if (MeasPrecisionTextBox.Value != ah.ap.meas_precision)
            {
                ah.ap.meas_precision = MeasPrecisionTextBox.Value; ah.ap.modified = true;
            }
            if ((ushort)MinNumCyclesTextBox.Value != ah.ap.min_num_runs)
            {
                ah.ap.min_num_runs = (ushort)MinNumCyclesTextBox.Value; ah.ap.modified = true;
            }
            if ((ushort)MaxNumCyclesTextBox.Value != ah.ap.max_num_runs)
            {
                ah.ap.max_num_runs = (ushort)MaxNumCyclesTextBox.Value; ah.ap.modified = true;
            }

            // save/update item id changes only when user selects OK
            NC.App.DB.ItemIds.Set();  // writes any new or modified item ids to the DB
            NC.App.DB.ItemIds.Refresh();    // save and update the in-memory item list
            if (ah.OKButton_Click(sender, e) == System.Windows.Forms.DialogResult.OK)
            {
                //user can cancel in here during LM set-up, account for it.
                UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/calibrationmeasurementsacquire.htm"); 
        }

        private void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selItem = ((ComboBox)sender).Text;
            ItemId Cur = NC.App.DB.ItemIds.Get(selItem);
            if (Cur == null)
            {
                return; 
            }
            ah.ap.ApplyItemId(Cur);
            FieldFillerOnItemId();
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

        private void IsotopicsBtn_Click(object sender, EventArgs e)
        {
            UpdateIsotopics(straight: true);
        }

        private void CompositeIsotopicsBtn_Click(object sender, EventArgs e)
        {
            IDDCompositeIsotopics f = new IDDCompositeIsotopics(ah.ap.comp_isotopics_id);
            DialogResult dlg = f.ShowDialog();
            if (dlg != DialogResult.OK)
                return;
            ah.RefreshParams();
            CompositeIsotopics selected = f.GetSelectedIsotopics;
            if (ah.ap.isotopics_id != selected.id) /* They changed the isotopics id.  Isotopics already saved to DB in IDDIsotopics*/
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
		void EnableTermControls()
		{
			switch (ah.ap.data_src)
			{
				case ConstructedSource.Live:
					CountTimeTextBox.Enabled = true;
					UseNumCyclesRadioButton.Enabled = true;
					UseDoublesRadioButton.Enabled = true;
					UseTriplesRadioButton.Enabled = true;
					NumCyclesTextBox.Enabled = ah.CycleCount;
					MeasPrecisionTextBox.Enabled = !ah.CycleCount;
					MinNumCyclesTextBox.Enabled = !ah.CycleCount;
					MaxNumCyclesTextBox.Enabled = !ah.CycleCount;
					break;
				default:
					CountTimeTextBox.Enabled = false;
					UseNumCyclesRadioButton.Enabled = false;
					UseDoublesRadioButton.Enabled = false;
					UseTriplesRadioButton.Enabled = false;
					MeasPrecisionTextBox.Enabled = false;
					MinNumCyclesTextBox.Enabled = false;
					MaxNumCyclesTextBox.Enabled = false;
					NumCyclesTextBox.Enabled = false;
				break;
			}
		}
        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);
			EnableTermControls();
            CommentAtEndCheckBox.Enabled = true;
            PrintResultsCheckBox.Enabled = true;
            switch (ah.ap.data_src)
            {
                case ConstructedSource.Live:
                    // every set as above
                    break;
                case ConstructedSource.DB:
                    break;
                case ConstructedSource.CycleFile:
                    CommentAtEndCheckBox.Enabled = false;
                    break;
                case ConstructedSource.Manual:
                    CommentAtEndCheckBox.Enabled = false;
                    break;
                case ConstructedSource.ReviewFile:
                default:
                    CommentAtEndCheckBox.Enabled = false;
                    //PrintResultsCheckbox.Enabled = false;
                    break;
            }
        }

        private void Pu240eCoeffBtn_Click(object sender, EventArgs e)
        {
            new IDDKValSelector().ShowDialog();
        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }

        private void FieldFillerOnItemId()
        {
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
            DeclaredMassTextBox.Text = ah.ap.mass.ToString("F3");
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

        void UpdateIsotopics(bool straight)      // straight means from iso button
        {
            IDDIsotopics f = new IDDIsotopics(ah.ap.isotopics_id);
            DialogResult dlg = f.ShowDialog();
            if (dlg != DialogResult.OK)
                return;
            ah.RefreshParams();
            Isotopics selected = f.GetSelectedIsotopics;
            if (ah.ap.isotopics_id != selected.id) /* They changed the isotopics id.  Isotopics already saved to DB in IDDIsotopics*/
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
                NC.App.DB.ItemIds.Set(Cur);
                NC.App.DB.ItemIds.Refresh();    // save and update the list of items
            }
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
    }
}
