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
            this.Text += " for detector " + ah.det.Id.DetectorName;

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
                    DataSourceComboBox.Items.Add(cs.HappyFunName());
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
            DataSourceComboBox.SelectedItem = ah.ap.data_src.HappyFunName();
            MaterialTypeComboBox.SelectedItem = ah.ap.item_type;
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
            if (NC.App.DB.ItemIds.Has (selItem))
            {
                //They selected an item that does exist in DB.  Fill in the dialog accordingly. HN 6.15.2015
                ItemId id = NC.App.DB.ItemIds.Get (selItem);
                ah.ap.ApplyItemId(id);
                FieldFiller();
            }
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MaterialTypeComboBox_SelectedIndexChanged(sender, e);
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
            IDDIsotopics f = new IDDIsotopics(ah.ap.isotopics_id);
            f.ShowDialog();
        }

        private void CompositeIsotopicsBtn_Click(object sender, EventArgs e)
        {
            // TODO:  Add button click handler
        }

        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);
            CountTimeTextBox.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;
            UseDoublesRadioButton.Enabled = true;
            UseTriplesRadioButton.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;

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

        private void Pu240eCoeffBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pu240e coefficients, sorry, bye bye, see ya later, adios");
            // next: implement this choice
        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }


    }
}
