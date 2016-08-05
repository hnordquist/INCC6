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

    public partial class IDDAcquirePrecision : Form
    {
        AcquireHandlers ah;

        public IDDAcquirePrecision()
        {
            InitializeComponent();

            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.precision;          
            this.Text += " for detector " + ah.det.Id.DetectorName;

            // Populate the UI fields with values from the local AcquireParameters object
            this.QCTestsCheckBox.Checked = ah.ap.qc_tests;
            this.PrintResultsCheckBox.Checked = ah.ap.print;
            this.CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            this.NumCyclesTextBox.Text = Format.Rend(ah.ap.num_runs);
            this.CommentTextBox.Text = ah.ap.comment;
            this.CountTimeTextBox.Text = Format.Rend(ah.ap.run_count_time);
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
            SetHelp();
        }
        private void SetHelp()
        {
            ToolTip tip = new ToolTip();
            String helpString = "Check to enable quality assurance tests. The QC tests\r\n" +
                "used are accidentals/singles test,\r\nchecksum tests on raw shift register data " +
                "and an outlier test using the calculated mass for each cycle.";
            tip.SetToolTip(this.QCTestsCheckBox, helpString);
            provider.SetShowHelp(this.QCTestsCheckBox, true);
            provider.SetHelpString(this.QCTestsCheckBox, helpString);

            helpString = "You can use this field to describe this measurement.\r\n" +
               "This comment will remain with the measurement results.";
            tip.SetToolTip(this.CommentTextBox, helpString);
            provider.SetShowHelp(this.CommentTextBox, true);
            provider.SetHelpString(this.CommentTextBox, helpString);

            helpString = "Check this box if you want to be able to describe the results\r\n" +
                "of this measurement. This comment will remain with the measurement results.";
            tip.SetToolTip(this.CommentAtEndCheckBox, helpString);
            provider.SetShowHelp(this.CommentAtEndCheckBox, true);
            provider.SetHelpString(this.CommentAtEndCheckBox, helpString);

            helpString = "The number of cycles require for this measurement that pass all QC tests.";
            tip.SetToolTip(this.NumCyclesTextBox, helpString);
            provider.SetShowHelp(this.NumCyclesTextBox, true);
            provider.SetHelpString(this.NumCyclesTextBox, helpString);

            helpString = "The length of time in seconds for each cycle in this measurement.";
            tip.SetToolTip(this.CountTimeTextBox, helpString);
            provider.SetShowHelp(this.CountTimeTextBox, true);
            provider.SetHelpString(this.CountTimeTextBox, helpString);

            helpString = "This measurement will continue until this measurement precision is\r\n" +
                "reached. The effective Pu240 mass is used to determine precision if\r\n" +
                "multiplicity analysis is being used, otherwise the doubles rate is used.";
            tip.SetToolTip(this.MeasPrecisionTextBox, helpString);
            provider.SetShowHelp(this.MeasPrecisionTextBox, true);
            provider.SetHelpString(this.MeasPrecisionTextBox, helpString);

            helpString = "The measurement will not terminate until at least this many cycles,\r\n" +
                "regardless of the measurement precision";
            tip.SetToolTip(this.MinNumCyclesTextBox, helpString);
            provider.SetShowHelp(this.MinNumCyclesTextBox, true);
            provider.SetHelpString(this.MinNumCyclesTextBox, helpString);

            helpString = "The measurement will terminate after this many cycles or when the\r\n" +
                "requested measurement precision is reached, whichever comes comes first.";
            tip.SetToolTip(this.MaxNumCyclesTextBox, helpString);
            provider.SetShowHelp(this.MaxNumCyclesTextBox, true);
            provider.SetHelpString(this.MaxNumCyclesTextBox, helpString);

            helpString = "Check this box to print and display results, uncheck to only display results.";
            tip.SetToolTip(this.PrintResultsCheckBox, helpString);
            provider.SetShowHelp(this.PrintResultsCheckBox, true);
            provider.SetHelpString(this.PrintResultsCheckBox, helpString);

            helpString = "Check this box to print the comment entered above on the measurement results.";
            tip.SetToolTip(this.CommentAtEndCheckBox, helpString);
            provider.SetShowHelp(this.CommentAtEndCheckBox, true);
            provider.SetHelpString(this.CommentAtEndCheckBox, helpString);

            helpString = "Possible data sources are:\r\n" +
                "- Acquire new data from shift register/pulse train recorder\r\n" +
                "- Reanalyze data previously acquired\r\n" +
                "- Read in raw data from a disk file (data must be in a specific format)\r\n" +
                "For more information on this format see the help file topic:\r\n" +
                "\"Radiation Review Measurement Data File Format\"";
            tip.SetToolTip(this.DataSourceComboBox, helpString);
            provider.SetShowHelp(this.DataSourceComboBox, true);
            provider.SetHelpString(this.DataSourceComboBox, helpString);
        }
        private void UseNumCyclesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ah.NumberOfCyclesRadioButton_CheckedChanged(sender, e);
 			EnableTermControls();
       }

        private void UseDblsRadioButton_CheckedChanged(object sender, EventArgs e)
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
        private void DataSrcComboBox_SelectedIndexChanged(object sender, EventArgs e)
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
        private void OKBtn_Click(object sender, EventArgs e)
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

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/precisionacquire.htm");
        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }

        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            ah.CountTimeTextBox_Leave(sender, e);
        }

        private void NumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.NumCyclesTextBox_Leave(sender, e);
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
    }
}
