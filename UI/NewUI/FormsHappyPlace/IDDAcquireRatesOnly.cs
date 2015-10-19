using System;
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;
namespace NewUI
{
 
    public partial class IDDAcquireRatesOnly : Form
    {
        AcquireHandlers ah;

        public IDDAcquireRatesOnly()
        {
            InitializeComponent();

            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.rates;
            this.Text += " for detector " + ah.det.Id.DetectorName;

            // Populate the UI fields with values from the local AcquireParameters object
            this.QCTestsCheckbox.Checked = ah.ap.qc_tests;
            this.PrintResultsCheckBox.Checked = ah.ap.print;
            this.CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            this.NumCyclesTextBox.Text = Format.Rend(ah.ap.num_runs);
            this.CommentTextBox.Text = ah.ap.comment;
            this.CountTimeTextBox.Text = Format.Rend(ah.ap.run_count_time);
            this.ItemIdTextBox.Text = ah.ap.item_id;
            this.MeasPrecisionTextBox.Text = ah.ap.meas_precision.ToString("F2");
            this.MinNumCyclesTextBox.Text = Format.Rend(ah.ap.min_num_runs);
            this.MaxNumCyclesTextBox.Text = Format.Rend(ah.ap.max_num_runs);

            this.DataSourceComboBox.Items.Clear();
            foreach (ConstructedSource cs in System.Enum.GetValues(typeof(ConstructedSource)))
            {
                if (cs.AcquireChoices() || cs.LMFiles(ah.det.Id.SRType))
                    DataSourceComboBox.Items.Add(cs.HappyFunName());
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
            DataSourceComboBox.SelectedItem = ah.ap.data_src.HappyFunName();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (ah.OKButton_Click(sender, e) == System.Windows.Forms.DialogResult.OK)
            {
                //user can cancel in here during LM set-up, account for it.
                this.Visible = false;
                UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            // TODO:  WTF.
        }

        private void NumberOfCyclesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.NumberOfCyclesRadioButton_CheckedChanged(sender, e);
        }

        private void DoublesMeasurementPrecisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.DoublesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);            
        }

        private void TriplesMeasurementPrecisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.TriplesMeasurementPrecisionRadioButton_CheckedChanged(sender, e);
        }

        private void QCTestsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ah.QCTestsCheckbox_CheckedChanged(sender, e);
        }

        private void PrintResultsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ah.PrintResultsCheckbox_CheckedChanged(sender, e);
        }

        private void CommentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ah.CommentCheckbox_CheckedChanged(sender, e);
        }

        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);

            CountTimeTextBox.Enabled = true;
            UseNumCyclesRadioButton.Enabled = true;
            UseDoublesRadioButton.Enabled = true;
            UseTriplesRadioButton.Enabled = true;

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

        private void NumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.NumCyclesTextBox_Leave(sender, e);
        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }

        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            ah.CountTimeTextBox_Leave(sender, e);
        }

        private void ItemIdTextBox_Leave(object sender, EventArgs e)
        {
            ah.ItemIdTextBox_Leave(sender, e);
        }

        private void MeasurementPrecisionTextBox_Leave(object sender, EventArgs e)
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
    }
}
