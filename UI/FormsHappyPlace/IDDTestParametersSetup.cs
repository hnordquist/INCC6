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
namespace UI
{
    using NC = NCC.CentralizedState;


    public partial class IDDTestParametersSetup : Form
    {

        TestParameters tp;

        public IDDTestParametersSetup()
        {
            InitializeComponent();
            tp = new TestParameters(NC.App.DB.TestParameters.Get()); // last one on the list is the newest one, we make local copy, and we update on the way out
            if (tp == null)
            {
                tp = new TestParameters();
            }
            // Fill in the GUI elements with the current values stored in the local data structure
            this.AccidentalsSinglesTestRateLimitTextBox.Text      = tp.accSnglTestRateLimit.ToString("F2");
            this.AccidentalsSinglesTestPrecisionLimitTextBox.Text = tp.accSnglTestPrecisionLimit.ToString("F2");
            this.AccidentalsSinglesTestOutlierLimitTextBox.Text   = tp.accSnglTestOutlierLimit.ToString("F2");
            this.OutlierTestLimitTextBox.Text                     = tp.outlierTestLimit.ToString("F2");
            this.MaxCyclesTextBox.Text                            = tp.maxCyclesForOutlierTest.ToString();
            this.MaxChecksumTextBox.Text                          = tp.maxNumFailures.ToString();
            this.ChecksumCheckbox.Checked                         = tp.checksum;
            this.VerificationLimitTextBox.Text                    = tp.normalBackupAssayTestLimit.ToString("F2");
            this.BackgroundDoublesRateLimitTextBox.Text           = tp.bkgDoublesRateLimit.ToString("F2");
            this.BackgroundTriplesRateLimitTextBox.Text           = tp.bkgTriplesRateLimit.ToString("F2");;
            this.ChiSquaredLimitTextBox.Text                      = tp.chiSquaredLimit.ToString("F2");
            this.HighVoltageTestLimitTextBox.Text                 = tp.highVoltageTestLimit.ToString("F2");
            this.MeasureAccidentalsRadioButton.Checked = (tp.accidentalsMethod == AccidentalsMethod.Measure ? true : false);
            this.CalculateAccidentalsRadioButton.Checked = (tp.accidentalsMethod == AccidentalsMethod.Calculate ? true : false);
        }

        private void MeasureAccidentalsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            tp.modified = true;
            if (this.MeasureAccidentalsRadioButton.Checked) tp.accidentalsMethod = AccidentalsMethod.Measure;
        }

        private void CalculateAccidentalsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            tp.modified = true;
            if (this.CalculateAccidentalsRadioButton.Checked) tp.accidentalsMethod = AccidentalsMethod.Calculate;
        }

        private void ChecksumCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            tp.modified = true;
            tp.checksum = this.ChecksumCheckbox.Checked;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            // If changes have been made to parameters, write the results to the DB.
            if (tp.modified)
            {
                NC.App.DB.TestParameters.Set(tp);  // if tp is not on list, this operation adds tp to list and fires a DB persist event
            }
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            //softwara obscura needs no help
            //the software works in mysterious ways
        }


        //// TEXT BOX LEAVE EVENT HANDLERS /////////////////////////////////////////////

        // If they've changed the value in this text box to a legal value that is different than what we had before,
        // copy the new value into the local data structure and set the Modified flag for eventual incorporation 
        // into the database if they click OK.
        private void AccidentalsSinglesTestRateLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a posive, non-zero number            
            if (Format.ToPNZ(((TextBox)sender).Text, ref tp.accSnglTestRateLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.accSnglTestRateLimit.ToString("F2");
        }

        private void AccidentalsSinglesTestPrecisionLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a value 0.0 <= x <= 100.0        
            if (Format.ToPct(((TextBox)sender).Text, ref tp.accSnglTestPrecisionLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.accSnglTestPrecisionLimit.ToString("F2");  
        }

        private void AccidentalsSinglesTestOutlierLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.accSnglTestOutlierLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.accSnglTestOutlierLimit.ToString("F2");      
        }

        private void OutlierTestLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.outlierTestLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.outlierTestLimit.ToString("F2");
        }

        private void MaxCyclesTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.maxCyclesForOutlierTest)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.maxCyclesForOutlierTest.ToString();
        }

        private void MaxChecksumTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.maxNumFailures)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.maxNumFailures.ToString();
        }

        private void VerificationLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.normalBackupAssayTestLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.normalBackupAssayTestLimit.ToString("F2");
        }

        private void BackgroundDoublesRateLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.bkgDoublesRateLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.bkgDoublesRateLimit.ToString("F2");   
        }

        private void BackgroundTriplesRateLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-negative number            
            if (Format.ToNN(((TextBox)sender).Text, ref tp.bkgTriplesRateLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.bkgTriplesRateLimit.ToString("F2");   
        }

        private void ChiSquaredLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a value 0.0 <= x <= 100.0        
            if (Format.ToPct(((TextBox)sender).Text, ref tp.chiSquaredLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.chiSquaredLimit.ToString("F2");     
        }

        private void HighVoltageTestLimitTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a value 0.0 <= x <= 100.0        
            if (Format.ToPct(((TextBox)sender).Text, ref tp.highVoltageTestLimit)) tp.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = tp.highVoltageTestLimit.ToString("F2");     
        }

    }
}
