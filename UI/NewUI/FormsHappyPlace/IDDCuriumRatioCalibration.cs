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
    using NC = NCC.CentralizedState;

    public partial class IDDCuriumRatioCalibration : Form
    {

        INCCAnalysisParams.curium_ratio_rec cm_ratio;
        MethodParamFormFields mp;

        public void FieldFiller()
        {
            switch (cm_ratio.curium_ratio_type)
            {
                case INCCAnalysisParams.CuriumRatioVariant.UseSingles:
                    UseSinglesRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.CuriumRatioVariant.UseDoubles:
                    UseDoublesRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles:
                    UseAddASourceRadioButton.Checked = true;
                    break;
            }  
        }

        public void FieldFiller(INCCAnalysisParams.CurveEquationVals cev)
        {
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

            LowerPuMassLimitTextBox.Text = cev.lower_mass_limit.ToString("N3");
            UpperPuMassLimitTextBox.Text = cev.lower_mass_limit.ToString("N3");
            SigmaXTextBox.Text = cev.sigma_x.ToString("E6");
        }

        public IDDCuriumRatioCalibration()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.CuriumRatio);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;
            this.MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.SelectedItem = mp.acq.item_type;
            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            mp.RefreshCurveEqComboBox(CurveTypeComboBox);
            FieldFiller(cm_ratio.cev);
            FieldFiller();
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            mp.ams = Integ.GetMethodSelections(mp.acq.detector_id, mp.acq.item_type); // unfinished, test and firm up
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.curium_ratio_rec((INCCAnalysisParams.curium_ratio_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.curium_ratio_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            cm_ratio = (INCCAnalysisParams.curium_ratio_rec)mp.imd;
            mp.cev = cm_ratio.cev;
            FieldFiller(cm_ratio.cev);
			FieldFiller();
        }

        private void UseSinglesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            INCCAnalysisParams.CuriumRatioVariant type = cm_ratio.curium_ratio_type;
            if (CheckType((RadioButton)sender, ref type))
            {
                cm_ratio.curium_ratio_type = type;
                cm_ratio.modified = true;
            }
        }

        private void UseDoublesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            INCCAnalysisParams.CuriumRatioVariant type = cm_ratio.curium_ratio_type;
            if (CheckType((RadioButton)sender, ref type))
            {
                cm_ratio.curium_ratio_type = type;
                cm_ratio.modified = true;
            }
        }

        private void UseAddASourceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            INCCAnalysisParams.CuriumRatioVariant type = cm_ratio.curium_ratio_type;
            if (CheckType((RadioButton)sender, ref type))
            {
                cm_ratio.curium_ratio_type = type;
                cm_ratio.modified = true;
            }
        }
        private bool CheckType (object sender, ref INCCAnalysisParams.CuriumRatioVariant type)
        {
            bool modified = false;
            if (((RadioButton)sender).Checked)
            {
                switch (((RadioButton)sender).Text)
                {
                    case "Use singles to calculate mass":
                        modified = (INCCAnalysisParams.CuriumRatioVariant.UseSingles != type);
                        if (modified) type = INCCAnalysisParams.CuriumRatioVariant.UseSingles;
                        break;
                    case "Use doubles to calculate mass":
                        modified = (INCCAnalysisParams.CuriumRatioVariant.UseDoubles != type);
                        if (modified) type = INCCAnalysisParams.CuriumRatioVariant.UseDoubles;
                        break;
                    case "UseAddASourceRadioButton":
                        modified = (INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles != type);
                        if (modified) type = INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles;
                        break;
                }
            }
            return modified;
        }

        private void LowerPuMassLimitTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref mp.cev.lower_mass_limit);
        }

        private void UpperPuMassLimitTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref mp.cev.upper_mass_limit);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = cm_ratio.ToLines(null);
            sec.AddRange(rows);

            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (NCCReporter.Row r in rows)
                s.WriteLine(r.ToLine(' '));
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text);
            pf.ShowDialog();
            File.Delete(path);
        }

        private void CurveTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((ComboBox)sender).SelectedText) &&
                ((ComboBox)sender).SelectedText != cm_ratio.cev.cal_curve_equation.ToDisplayString())
            {
                cm_ratio.cev.cal_curve_equation= (INCCAnalysisParams.CurveEquation)((ComboBox)sender).SelectedIndex;
                cm_ratio.modified = true;
            }
        }

        private void ATextBox_TextChanged(object sender, EventArgs e)
        {
            mp.ATextBox_Leave((TextBox)sender);
        }

        private void BTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.BTextBox_Leave((TextBox)sender);
        }

        private void CTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CTextBox_Leave((TextBox)sender);
        }

        private void DTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DTextBox_Leave((TextBox)sender);
        }

        private void VarianceATextBox_TextChanged(object sender, EventArgs e)
        {
            mp.VarianceATextBox_Leave((TextBox)sender);
        }

        private void VarianceBTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.VarianceBTextBox_Leave((TextBox)sender);
        }

        private void VarianceCTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.VarianceCTextBox_Leave((TextBox)sender);
        }

        private void VarianceDTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.VarianceDTextBox_Leave((TextBox)sender);
        }

        private void CovarianceABTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceABTextBox_Leave((TextBox)sender);
        }

        private void CovarianceACTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceACTextBox_Leave((TextBox)sender);
        }

        private void CovarianceADTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceADTextBox_Leave((TextBox)sender);
        }

        private void CovarianceBCTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceBCTextBox_Leave((TextBox)sender);
        }

        private void CovarianceBDTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceBDTextBox_Leave((TextBox)sender);
        }

        private void CovarianceCDTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.CovarianceCDTextBox_Leave((TextBox)sender);
        }

        private void SigmaXTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.SigmaXTextBox_Leave((TextBox)sender);
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
