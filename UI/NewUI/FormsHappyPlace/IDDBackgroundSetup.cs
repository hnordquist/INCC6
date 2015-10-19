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

namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;
    
    public partial class IDDBackgroundSetup : Form
    {

        BackgroundParameters bp;
        Detector det;
        public IDDBackgroundSetup()
        {
            InitializeComponent();
            det = Integ.GetCurrentAcquireDetector();
            bp = Integ.GetCurrentBackgroundParams(det);
            bp.modified = false;
            this.Text += " for detector " + det.Id.DetectorName;
            foreach (Control nt in this.Controls)
            {
                if (nt.GetType() == typeof(NumericTextBox))
                {
                    ((NumericTextBox)nt).NumberFormat = NumericTextBox.Formatter.F6;
                    ((NumericTextBox)nt).ToValidate = NumericTextBox.ValidateType.Float;
                    ((NumericTextBox)nt).Min = -100.0;
                    ((NumericTextBox)nt).Max = 100000.0;
                }
            }
            FieldFiller();
        }

        private void FieldFiller ()
        {
            PassiveSinglesTextBox.Value = bp.DeadtimeCorrectedRates.Singles.v;
            PassiveSinglesErrorTextBox.Value = bp.DeadtimeCorrectedRates.Singles.err;
            PassiveDoublesTextBox.Value = bp.DeadtimeCorrectedRates.Doubles.v;
            PassiveDoublesErrorTextBox.Value = bp.DeadtimeCorrectedRates.Doubles.err;
            PassiveTriplesTextBox.Value = bp.DeadtimeCorrectedRates.Triples.v;
            PassiveTriplesErrorTextBox.Value = bp.DeadtimeCorrectedRates.Triples.err;
            PassiveScaler1TextBox.Value = bp.Scaler1.v;
            PassiveScaler2TextBox.Value = bp.Scaler2.v;
            ActiveSinglesTextBox.Value = bp.INCCActive.Singles.v;
            ActiveSinglesErrorTextBox.Value = bp.INCCActive.Singles.err;
            ActiveDoublesTextBox.Value = bp.INCCActive.Doubles.v;
            ActiveDoublesErrorTextBox.Value = bp.INCCActive.Doubles.err;
            ActiveTriplesTextBox.Value = bp.INCCActive.Triples.v;
            ActiveTriplesErrorTextBox.Value = bp.INCCActive.Triples.err;
            ActiveScaler1TextBox.Value = bp.INCCActive.Scaler1Rate;
            ActiveScaler2TextBox.Value = bp.INCCActive.Scaler2Rate;
            DisplayMultiplicityCheckBox.Checked = bp.TMMode;
            TMSinglesTextBox.Value = bp.TMBkgParams.Singles.v;
            TMSinglesErrorTextBox.Value = bp.TMBkgParams.Singles.err;
            TMZerosTextBox.Value = bp.TMBkgParams.Zeros.v;
            TMZerosErrorTextBox.Value = bp.TMBkgParams.Zeros.err;
            TMOnesTextBox.Value = bp.TMBkgParams.Ones.v;
            TMOnesErrorTextBox.Value = bp.TMBkgParams.Ones.err;
            TMTwosTextBox.Value = bp.TMBkgParams.Twos.v;
            TMTwosErrorTextBox.Value = bp.TMBkgParams.Twos.err;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            if (PassiveSinglesTextBox.Value != bp.rates.DeadtimeCorrectedRates.SinglesRate)
            {
                //Are there really three copies of this?
                bp.DeadtimeCorrectedSinglesRate.v = PassiveSinglesTextBox.Value;
                bp.rates.DeadtimeCorrectedRates.SinglesRate= PassiveSinglesTextBox.Value;
                bp.rates.DTCRates.Singles.v = PassiveSinglesTextBox.Value;
                bp.modified = true;
            }
            if (PassiveDoublesTextBox.Value != bp.rates.DeadtimeCorrectedRates.DoublesRate)
            {
                bp.DeadtimeCorrectedRates.Doubles.v = PassiveDoublesTextBox.Value;
                bp.rates.DeadtimeCorrectedRates.DoublesRate = PassiveDoublesTextBox.Value;
                bp.rates.DTCRates.Doubles.v = PassiveDoublesTextBox.Value;
                bp.modified = true;
            }
            if (PassiveTriplesTextBox.Value != bp.rates.DeadtimeCorrectedRates.TriplesRate)
            {
                bp.DeadtimeCorrectedRates.Triples.v = PassiveTriplesTextBox.Value;
                bp.rates.DeadtimeCorrectedRates.TriplesRate = PassiveTriplesTextBox.Value;
                bp.rates.DTCRates.Triples.v = PassiveTriplesTextBox.Value;
                bp.modified = true;
            }
            if (PassiveSinglesErrorTextBox.Value != bp.rates.DTCRates.Singles.err)
            {
                bp.DeadtimeCorrectedRates.Singles.err = PassiveSinglesErrorTextBox.Value;
                bp.rates.DTCRates.Singles.err = PassiveSinglesErrorTextBox.Value;
                bp.modified = true;
            }
            if (PassiveDoublesErrorTextBox.Value != bp.rates.DTCRates.Doubles.err)
            {
                bp.DeadtimeCorrectedRates.Doubles.err = PassiveDoublesErrorTextBox.Value; 
                bp.rates.DTCRates.Doubles.err = PassiveDoublesErrorTextBox.Value;
                bp.modified = true;
            }
            if (PassiveTriplesErrorTextBox.Value != bp.rates.DTCRates.Triples.err)
            {
                bp.DeadtimeCorrectedRates.Triples.err = PassiveTriplesErrorTextBox.Value; 
                bp.rates.DTCRates.Triples.err = PassiveTriplesErrorTextBox.Value;
                bp.modified = true;
            }
            if (PassiveScaler1TextBox.Value != bp.Scaler1.v)
            {
                bp.Scaler1.v = PassiveScaler1TextBox.Value;
                bp.modified = true;
            }
            if (PassiveScaler2TextBox.Value != bp.Scaler2.v)
            {
                bp.Scaler2.v = PassiveScaler2TextBox.Value;
                bp.modified = true;
            }

            if (ActiveSinglesTextBox.Value != bp.INCCActive.SinglesRate)
            {
                bp.INCCActive.SinglesRate = ActiveSinglesTextBox.Value;
                bp.INCCActive.Singles.v = ActiveSinglesTextBox.Value;
                bp.modified = true;
            }
            if (ActiveDoublesTextBox.Value != bp.INCCActive.DoublesRate)
            {
                bp.INCCActive.DoublesRate = ActiveDoublesTextBox.Value;
                bp.INCCActive.Doubles.v = ActiveDoublesTextBox.Value;
                bp.modified = true;
            }
            if (ActiveTriplesTextBox.Value != bp.INCCActive.TriplesRate)
            {
                bp.INCCActive.TriplesRate = ActiveTriplesTextBox.Value;
                bp.INCCActive.Triples.v = ActiveTriplesTextBox.Value;
                bp.modified = true;
            }
            if (ActiveSinglesErrorTextBox.Value != bp.INCCActive.Singles.err)
            {
                bp.INCCActive.Singles.err = ActiveSinglesErrorTextBox.Value;
                bp.modified = true;
            }
            if (ActiveDoublesErrorTextBox.Value != bp.INCCActive.Doubles.err)
            {
                bp.INCCActive.Doubles.err = ActiveDoublesErrorTextBox.Value;
                bp.modified = true;
            }
            if (ActiveTriplesErrorTextBox.Value != bp.INCCActive.Triples.err)
            {
                bp.INCCActive.Singles.err = ActiveTriplesErrorTextBox.Value;
                bp.modified = true;
            }
            if (ActiveScaler1TextBox.Value != bp.INCCActive.Scaler1Rate)
            {
                bp.INCCActive.Scaler1Rate = ActiveScaler1TextBox.Value;
                bp.modified = true;
            }
            if (ActiveScaler2TextBox.Value != bp.INCCActive.Scaler2Rate)
            {
                bp.INCCActive.Scaler2Rate = ActiveScaler2TextBox.Value;
                bp.modified = true;
            }
            if (bp.modified)
            {
                NC.App.DB.BackgroundParameters.GetMap().Remove(det);
                NC.App.DB.BackgroundParameters.GetMap().Add(det, bp);
                NC.App.DB.BackgroundParameters.Set(det, bp);
            }
            //Later, deal with TM params......
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

 

    }
}
