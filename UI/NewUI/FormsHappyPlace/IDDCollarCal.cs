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
    public partial class IDDCollarCal : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.collar_combined_rec col;
        bool modified;

        public IDDCollarCal(INCCAnalysisParams.collar_combined_rec c, bool mod)
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.Collar);
            modified = mod;
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            col = c;
            mp.cev = col.collar.cev;
            mp.RefreshCurveEqComboBox(CurveTypeComboBox);
            CalibrationFieldFiller(col.collar.cev);
            SetHelp();
            this.TopMost = true;
        }

        public void CalibrationFieldFiller(INCCAnalysisParams.CurveEquationVals cev)
        {
            LowerMassLimitTextBox.Text = cev.lower_mass_limit.ToString("N3");
            LowerMassLimitTextBox.Min = -10000000.0;
            UpperMassLimitTextBox.Text = cev.upper_mass_limit.ToString("N3");
            ATextBox.Text = cev.a.ToString("E6");
            BTextBox.Text = cev.b.ToString("E6");
            CTextBox.Text = cev.c.ToString("E6");
            DTextBox.Text = cev.d.ToString("E6");
            VarianceATextBox.Text = cev.var_a.ToString("E6");
            VarianceBTextBox.Text = cev.var_b.ToString("E6");
            VarianceCTextBox.Text = cev.var_c.ToString("E6");
            VarianceDTextBox.Text = cev.var_d.ToString("E6");
            CovarianceABTextBox.Text = cev.covar(Coeff.a, Coeff.b).ToString("E6");
            CovarianceACTextBox.Text = cev.covar(Coeff.a, Coeff.c).ToString("E6");
            CovarianceADTextBox.Text = cev.covar(Coeff.a, Coeff.d).ToString("E6");
            CovarianceBCTextBox.Text = cev.covar(Coeff.b, Coeff.c).ToString("E6");
            CovarianceBDTextBox.Text = cev.covar(Coeff.b, Coeff.d).ToString("E6");
            CovarianceCDTextBox.Text = cev.covar(Coeff.c, Coeff.d).ToString("E6");
            SigmaXTextBox.Text = cev.sigma_x.ToString("E6");
        }

        private void CurveTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(((ComboBox)sender).SelectedText,mp.cev.cal_curve_equation.ToDisplayString()) != 0)
            {
                mp.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)((ComboBox)sender).SelectedIndex;
                modified = true;
            }
        }

        public void SetHelp ()
        {
            ToolTip t1 = new ToolTip();
            t1.IsBalloon = true;
            String display = "Calibration parameters must be entered manually. The calibration\r\n" +
                            "curve is usually D = a * m / (1 + b * m), where D is the doubles\r\n" +
                            "rate and m is the U235 mass.";
            provider.SetShowHelp(this.ATextBox, true);
            provider.SetHelpString(this.ATextBox, display);
            t1.SetToolTip(this.ATextBox, display);

            provider.SetShowHelp(this.BTextBox, true);
            provider.SetHelpString(this.BTextBox, display);
            t1.SetToolTip(this.BTextBox, display);

            provider.SetShowHelp(this.CTextBox, true);
            provider.SetHelpString(this.CTextBox, display);
            t1.SetToolTip(this.CTextBox, display);

            provider.SetShowHelp(this.DTextBox, true);
            provider.SetHelpString(this.DTextBox, display);
            t1.SetToolTip(this.DTextBox, display);

            provider.SetShowHelp(this.VarianceATextBox, true);
            provider.SetHelpString(this.VarianceATextBox, display);
            t1.SetToolTip(this.VarianceATextBox, display);

            provider.SetShowHelp(this.VarianceBTextBox, true);
            provider.SetHelpString(this.VarianceBTextBox, display);
            t1.SetToolTip(this.VarianceBTextBox, display);

            provider.SetShowHelp(this.VarianceCTextBox, true);
            provider.SetHelpString(this.VarianceCTextBox, display);
            t1.SetToolTip(this.VarianceCTextBox, display);

            provider.SetShowHelp(this.VarianceDTextBox, true);
            provider.SetHelpString(this.VarianceDTextBox, display);
            t1.SetToolTip(this.VarianceDTextBox, display);

            provider.SetShowHelp(this.CovarianceABTextBox, true);
            provider.SetHelpString(this.CovarianceABTextBox, display);
            t1.SetToolTip(this.CovarianceABTextBox, display);

            provider.SetShowHelp(this.CovarianceACTextBox, true);
            provider.SetHelpString(this.CovarianceACTextBox, display);
            t1.SetToolTip(this.CovarianceACTextBox, display);

            provider.SetShowHelp(this.CovarianceADTextBox, true);
            provider.SetHelpString(this.CovarianceADTextBox, display);
            t1.SetToolTip(this.CovarianceADTextBox, display);

            provider.SetShowHelp(this.CovarianceBCTextBox, true);
            provider.SetHelpString(this.CovarianceBCTextBox, display);
            t1.SetToolTip(this.CovarianceBCTextBox, display);

            provider.SetShowHelp(this.CovarianceBDTextBox, true);
            provider.SetHelpString(this.CovarianceBDTextBox, display);
            t1.SetToolTip(this.CovarianceBDTextBox, display);

            provider.SetShowHelp(this.CovarianceCDTextBox, true);
            provider.SetHelpString(this.CovarianceCDTextBox, display);
            t1.SetToolTip(this.CovarianceCDTextBox, display);

            display = "Sigma x is an extra error term for errors resulting from a mismatch\r\n" +
                        "between calibration standards and items measured. This error is\r\n" +
                        "combined quadraticlaly with the other errors. Value of sigma x is\r\n" +
                        "determined from experience; if unknown, enter 0.";
            provider.SetShowHelp(this.SigmaXTextBox, true);
            provider.SetHelpString(this.SigmaXTextBox, display);
            t1.SetToolTip(this.SigmaXTextBox, display);

            display = "Enter the mass for Pu240 effective or U235 below which the calibration\r\n" +
                    "parameters are invalid. A calculated mass below this value will have an\r\n" +
                    "out of calibration range warning message.";
            provider.SetShowHelp(this.LowerMassLimitTextBox, true);
            provider.SetHelpString(this.LowerMassLimitTextBox, display);
            t1.SetToolTip(this.LowerMassLimitTextBox, display);

            display = "Enter the mass for Pu240 effective or U235 above which the calibration\r\n" +
                    "parameters are invalid. A calculated mass above this value will have an\r\n" +
                    "out of calibration range warning message.";
            provider.SetShowHelp(this.UpperMassLimitTextBox, true);
            provider.SetHelpString(this.UpperMassLimitTextBox, display);
            t1.SetToolTip(this.UpperMassLimitTextBox, display);
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            IDDCorrectionFactors CorrFact = new IDDCorrectionFactors(col,modified);
            CorrFact.StartPosition = FormStartPosition.CenterScreen;
            CorrFact.Show();
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            if (modified)
            {
                if (MessageBox.Show("Are you sure you want to abandon your changes and exit?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.Close();
            }
            else
                this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            //TODO: I don't see collar help??
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm"); 
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            IDDCollarCrossRef CrossRef = new IDDCollarCrossRef(-1/*do nothing to mode here*/,modified,col);
            CrossRef.StartPosition = FormStartPosition.CenterScreen;
            CrossRef.Show();
            this.Close();
        }

        private void StoreChanges()
        {
            if (col.collar.cev.a != ATextBox.Value)
            {
                col.collar.cev.a = ATextBox.Value;
                modified = true;
            }

            if (col.collar.cev.b != BTextBox.Value)
            {
                col.collar.cev.b = BTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.c != CTextBox.Value)
            {
                col.collar.cev.c = CTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.d != DTextBox.Value)
            {
                col.collar.cev.d = DTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.var_a != VarianceATextBox.Value)
            {
                col.collar.cev.var_a = VarianceATextBox.Value;
                modified = true;
            }

            if (col.collar.cev.var_b != VarianceBTextBox.Value)
            {
                col.collar.cev.var_b = VarianceBTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.var_c != VarianceCTextBox.Value)
            {
                col.collar.cev.var_c = VarianceCTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.var_d != VarianceDTextBox.Value)
            {
                col.collar.cev.var_d = VarianceDTextBox.Value;
                modified = true;
            }
           
            if (col.collar.cev.lower_mass_limit != LowerMassLimitTextBox.Value)
            {
                col.collar.cev.lower_mass_limit = LowerMassLimitTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.upper_mass_limit != UpperMassLimitTextBox.Value)
            {
                col.collar.cev.upper_mass_limit = UpperMassLimitTextBox.Value;
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.a,Coeff.b) != CovarianceABTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.a, Coeff.b, CovarianceABTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.a, Coeff.c) != CovarianceACTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.a, Coeff.c, CovarianceACTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.a, Coeff.d) != CovarianceADTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.a, Coeff.d, CovarianceADTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.b, Coeff.c) != CovarianceBCTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.b, Coeff.c, CovarianceBCTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.b, Coeff.d) != CovarianceBDTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.b, Coeff.d, CovarianceBDTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.covar(Coeff.c, Coeff.d) != CovarianceCDTextBox.Value)
            {
                col.collar.cev.setcovar(Coeff.c, Coeff.d, CovarianceCDTextBox.Value);
                modified = true;
            }

            if (col.collar.cev.sigma_x != SigmaXTextBox.Value)
            {
                col.collar.cev.sigma_x = SigmaXTextBox.Value;
            }
        }

    }
}
;