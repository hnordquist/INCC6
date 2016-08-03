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
    public partial class IDDKnownAlphaCal : Form
    {
        INCCAnalysisParams.known_alpha_rec known_alpha;
        MethodParamFormFields mp;

        public void FieldFiller(INCCAnalysisParams.CurveEquationVals cev)
        {
            LowerMassLimitTextBox.Value = cev.lower_mass_limit;
            UpperMassLimitTextBox.Value = cev.upper_mass_limit;
            LowerAlphaLimitTextBox.Value = known_alpha.lower_corr_factor_limit;
            UpperAlphaLimitTextBox.Value = known_alpha.upper_corr_factor_limit;
            ConventionalATextBox.Value = cev.a;
            ConventionalBTextBox.Value = cev.b;
            VarianceATextBox.Value = cev.var_a;
            VarianceBTextBox.Value = cev.var_b;

            CovarianceABTextBox.Value = cev.covar(Coeff.a, Coeff.b);
            
            SigmaXTextBox.Value = cev.sigma_x;
        }

        public void FieldFiller()
        {
            HvyMetalRefTextBox.Value = known_alpha.heavy_metal_reference;
            HvyMetalWeightingTextBox.Value = known_alpha.heavy_metal_corr_factor;
            AlphaWeightTextBox.Value = known_alpha.alpha_wt;
            RhoZeroTextBox.Value = known_alpha.rho_zero;
            KTextBox.Value = known_alpha.k;

            if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.Conventional)
                ConventionalRadioButton.Checked = true;
            else if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection)
                HeavyMetalRadioButton.Checked = true;
            else if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha)
                MoistureToDryAlphaRadioButton.Checked = true;
            else if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles)
                MoistureToDoublesRadioButton.Checked = true;

            MoistureATextBox.Value = known_alpha.ring_ratio.a;
            MoistureBTextBox.Value = known_alpha.ring_ratio.b;
            MoistureCTextBox.Value = known_alpha.ring_ratio.c;
            MoistureDTextBox.Value = known_alpha.ring_ratio.d;
        }

        public IDDKnownAlphaCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.KnownA);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            mp.RefreshCurveEqComboBox(CurveTypeComboBox, known_alpha.ring_ratio);

            SetNumberFormats(); 
            FieldFiller(known_alpha.cev);
            FieldFiller(); 
        }
        private void SetNumberFormats()
        {
            LowerMassLimitTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            LowerMassLimitTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            LowerMassLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            LowerMassLimitTextBox.Min = -100000000;
            UpperMassLimitTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            UpperMassLimitTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            UpperMassLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            LowerAlphaLimitTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            LowerAlphaLimitTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            LowerAlphaLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            UpperAlphaLimitTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            UpperAlphaLimitTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            UpperAlphaLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
        }
        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.known_alpha_rec((INCCAnalysisParams.known_alpha_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.known_alpha_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            known_alpha = (INCCAnalysisParams.known_alpha_rec)mp.imd;
            mp.cev = known_alpha.cev;
            FieldFiller(known_alpha.cev);
        }
        private void TextBoxEnablementStep()
        {
            if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.Conventional)
            {
                HvyMetalRefTextBox.Enabled = false;
                HvyMetalWeightingTextBox.Enabled = false;
                LowerAlphaLimitTextBox.Enabled = false;
                UpperAlphaLimitTextBox.Enabled = false;
                MoistureATextBox.Enabled = false;
                MoistureBTextBox.Enabled = false;
                MoistureCTextBox.Enabled = false;
                MoistureDTextBox.Enabled = false;
                CurveTypeComboBox.Enabled = false;
            }
            else if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection)
            {
                HvyMetalRefTextBox.Enabled = true;
                HvyMetalWeightingTextBox.Enabled = true;
                LowerAlphaLimitTextBox.Enabled = false;
                UpperAlphaLimitTextBox.Enabled = false;
                MoistureATextBox.Enabled = false;
                MoistureBTextBox.Enabled = false;
                MoistureCTextBox.Enabled = false;
                MoistureDTextBox.Enabled = false;
                CurveTypeComboBox.Enabled = false;
            }
            else if (known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha || 
                     known_alpha.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles)
            {
                HvyMetalRefTextBox.Enabled = false;
                HvyMetalWeightingTextBox.Enabled = false;
                LowerAlphaLimitTextBox.Enabled = true;
                UpperAlphaLimitTextBox.Enabled = true;
                MoistureATextBox.Enabled = true;
                MoistureBTextBox.Enabled = true;
                MoistureCTextBox.Enabled = true;
                MoistureDTextBox.Enabled = true;
                CurveTypeComboBox.Enabled = true;
            }
        }

        private void CheckBox_fandango(Boolean ischecked, INCCAnalysisParams.KnownAlphaVariant known_alpha_type)
        {
            known_alpha.modified |= (known_alpha.known_alpha_type != known_alpha_type);
            known_alpha.known_alpha_type = known_alpha_type;
        }

        private void ConventionalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_fandango(((RadioButton)sender).Checked, INCCAnalysisParams.KnownAlphaVariant.Conventional);
            TextBoxEnablementStep();
        }

        private void HeavyMetalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_fandango(((RadioButton)sender).Checked, INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection);
            TextBoxEnablementStep();
        }

        private void MoistureToDryAlphaRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_fandango(((RadioButton)sender).Checked, INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha); 
            TextBoxEnablementStep();
        }

        private void MoistureToDoublesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_fandango(((RadioButton)sender).Checked, INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles); 
            TextBoxEnablementStep();
        }

        private void CurveTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((ComboBox)sender).SelectedText) && 
                ((ComboBox)sender).SelectedText != known_alpha.ring_ratio.cal_curve_equation.ToDisplayString())
            {
                known_alpha.ring_ratio.cal_curve_equation = (INCCAnalysisParams.CurveEquation)((ComboBox)sender).SelectedIndex;
                known_alpha.modified = true;
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = known_alpha.ToLines(null);
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

        private void OKBtn_Click(object sender, EventArgs e)
        {
            //if (cct != cal_curve.CalCurveType)
            //todo: probably should check for changes, but storing each time not super expensive hn 5.6.2015
            mp.imd.modified = true;
            known_alpha.cev.lower_mass_limit = LowerMassLimitTextBox.Value;
            known_alpha.cev.upper_mass_limit = UpperMassLimitTextBox.Value;
            known_alpha.cev.a = ConventionalATextBox.Value;
            known_alpha.cev.b = ConventionalBTextBox.Value;
            known_alpha.cev.var_a = VarianceATextBox.Value;
            known_alpha.cev.var_b =VarianceBTextBox.Value;

            known_alpha.cev.setcovar(Coeff.a,Coeff.b, CovarianceABTextBox.Value);

            known_alpha.cev.sigma_x = SigmaXTextBox.Value;

            known_alpha.heavy_metal_reference = HvyMetalRefTextBox.Value;
            known_alpha.heavy_metal_corr_factor = HvyMetalWeightingTextBox.Value;
            known_alpha.alpha_wt = AlphaWeightTextBox.Value;
            known_alpha.rho_zero = RhoZeroTextBox.Value;
            known_alpha.k = KTextBox.Value;

            if (ConventionalRadioButton.Checked == true)
                known_alpha.known_alpha_type = INCCAnalysisParams.KnownAlphaVariant.Conventional;
            else if (HeavyMetalRadioButton.Checked == true)
                known_alpha.known_alpha_type = INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection;
            else if (MoistureToDryAlphaRadioButton.Checked == true)
                known_alpha.known_alpha_type = INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha;
            else if (MoistureToDoublesRadioButton.Checked == true)
                known_alpha.known_alpha_type = INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles;


            known_alpha.lower_corr_factor_limit = LowerAlphaLimitTextBox.Value;
            known_alpha.upper_corr_factor_limit = UpperAlphaLimitTextBox.Value;

            known_alpha.ring_ratio.a = MoistureATextBox.Value;
            known_alpha.ring_ratio.b = MoistureBTextBox.Value;
            known_alpha.ring_ratio.c = MoistureCTextBox.Value;
            known_alpha.ring_ratio.d = MoistureDTextBox.Value;

            known_alpha.modified = true;
            mp.ams.modified = true;
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
