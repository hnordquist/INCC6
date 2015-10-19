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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{

    using Integ = NCC.IntegrationHelpers;

    public partial class IDDActiveCal : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.active_rec active;

        public void CalibrationFieldFiller(INCCAnalysisParams.CurveEquationVals cev)
        {
            LowerMassLimitTextBox.Text = cev.lower_mass_limit.ToString("N3");
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

        public IDDActiveCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.Active);

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            mp.RefreshCurveEqComboBox(CurveTypeComboBox);

            CalibrationFieldFiller(active.cev);
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.active_rec((INCCAnalysisParams.active_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.active_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            active = (INCCAnalysisParams.active_rec)mp.imd;
            mp.cev = active.cev;
            CalibrationFieldFiller(active.cev);
        }

        private void CurveTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!String.IsNullOrEmpty(((ComboBox)sender).SelectedText) && 
                ((ComboBox)sender).SelectedText != mp.cev.cal_curve_equation.ToDisplayString()))
            {
                mp.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)((ComboBox)sender).SelectedIndex;
                mp.imd.modified = true;
            }
        }

        private void ATextBox_Leave(object sender, EventArgs e)
        {
            mp.ATextBox_Leave(((TextBox)sender));
        }

        private void BTextBox_Leave(object sender, EventArgs e)
        {
            mp.BTextBox_Leave(((TextBox)sender));
        }

        private void CTextBox_Leave(object sender, EventArgs e)
        {
            mp.CTextBox_Leave(((TextBox)sender));
        }

        private void DTextBox_Leave(object sender, EventArgs e)
        {
            mp.DTextBox_Leave(((TextBox)sender));
        }

        private void VarianceATextBox_Leave(object sender, EventArgs e)
        {
            mp.VarianceATextBox_Leave(((TextBox)sender));
        }

        private void VarianceBTextBox_Leave(object sender, EventArgs e)
        {
            mp.VarianceBTextBox_Leave(((TextBox)sender));
        }

        private void VarianceCTextBox_Leave(object sender, EventArgs e)
        {
            mp.VarianceCTextBox_Leave(((TextBox)sender));
        }

        private void VarianceDTextBox_Leave(object sender, EventArgs e)
        {
            mp.VarianceDTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceABTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceABTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceACTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceACTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceADTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceADTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceBCTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceBCTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceBDTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceBDTextBox_Leave(((TextBox)sender));
        }

        private void CovarianceCDTextBox_Leave(object sender, EventArgs e)
        {
            mp.CovarianceCDTextBox_Leave(((TextBox)sender));
        }

        private void SigmaXTextBox_Leave(object sender, EventArgs e)
        {
            mp.SigmaXTextBox_Leave(((TextBox)sender));
        }

        private void LowerMassLimitTextBox_Leave(object sender, EventArgs e)
        {
            mp.LowerMassLimitTextBox_Leave(((TextBox)sender));

        }
        private void UpperMassLimitTextBox_Leave(object sender, EventArgs e)
        {
            mp.UpperMassLimitTextBox_Leave(((TextBox)sender));
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null,0,0,0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = active.ToLines(null);
            sec.AddRange(rows);

            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (NCCReporter.Row r in rows)
                s.WriteLine(r.ToLine (' '));
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text);
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            mp.Persist();
            this.Close();
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
