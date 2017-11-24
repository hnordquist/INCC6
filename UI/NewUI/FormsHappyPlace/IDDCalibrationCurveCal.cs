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
    public partial class IDDCalibrationCurveCal : Form
    {

        protected  void FieldFiller(INCCAnalysisParams.CurveEquationVals cev)
        {
            //Now using NumericTextBox
            LowerMassLimitTextBox.Value = cev.lower_mass_limit;
            UpperMassLimitTextBox.Value = cev.upper_mass_limit;
            ATextBox.Value = cev.a;
            BTextBox.Value = cev.b;
            CTextBox.Value = cev.c;
            DTextBox.Value = cev.d;
            VarianceATextBox.Value = cev.var_a;
            VarianceBTextBox.Value = cev.var_b;
            VarianceCTextBox.Value = cev.var_c;
            VarianceDTextBox.Value = cev.var_d;
            CovarianceABTextBox.Value = cev.covar(Coeff.a, Coeff.b);
            CovarianceACTextBox.Value = cev.covar(Coeff.a, Coeff.c);
            CovarianceADTextBox.Value = cev.covar(Coeff.a, Coeff.d);
            CovarianceBCTextBox.Value = cev.covar(Coeff.b, Coeff.c);
            CovarianceBDTextBox.Value = cev.covar(Coeff.b, Coeff.d);
            CovarianceCDTextBox.Value = cev.covar(Coeff.c, Coeff.d);
            SigmaXTextBox.Text = cev.sigma_x.ToString("E6");
            FieldFiller();
        }

        protected void FieldFiller()
        {
            HvyMetalRefTextBox.Value = cal_curve.heavy_metal_reference;
            HvyMetalWeightingTextBox.Value = cal_curve.heavy_metal_corr_factor;
            U235PercentTextBox.Value = cal_curve.percent_u235;

            if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.STD)
                ConventionalRadioButton.Checked = true;
            else if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.HM)
                HeavyMetalRadioButton.Checked = true;
            else if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.U)
                PassiveUraniumRadioButton.Checked = true;
            if (cal_curve.cev.useSingles)
                SinglesForMassRadioButton.Checked = true;
            else
                DoublesForMassRadioButton.Checked = true;

        }

        INCCAnalysisParams.CalCurveType cct;
        INCCAnalysisParams.cal_curve_rec cal_curve;
        MethodParamFormFields mp;

        public IDDCalibrationCurveCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.CalibrationCurve);

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            mp.RefreshCurveEqComboBox(CurveTypeComboBox);

            FieldFiller(cal_curve.cev);
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.cal_curve_rec((INCCAnalysisParams.cal_curve_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.cal_curve_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            cal_curve = (INCCAnalysisParams.cal_curve_rec)mp.imd;
            mp.cev = cal_curve.cev;
            cct = cal_curve.CalCurveType;
            CurveTypeComboBox.SelectedItem = cal_curve.cev.cal_curve_equation;
            FieldFiller(cal_curve.cev);
        }

        private void TextBoxEnablementProcessingMethodOfFineness()
        {
            if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.STD)
            {
                HvyMetalRefTextBox.Enabled = false;
                HvyMetalWeightingTextBox.Enabled = false;
                U235PercentTextBox.Enabled = false;
            }
            else if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.HM)
            {
                HvyMetalRefTextBox.Enabled = true;
                HvyMetalWeightingTextBox.Enabled = true;
                U235PercentTextBox.Enabled = false;
            }
            else if (cal_curve.CalCurveType == INCCAnalysisParams.CalCurveType.U)
            {
                HvyMetalRefTextBox.Enabled = false;
                HvyMetalWeightingTextBox.Enabled = false;
                U235PercentTextBox.Enabled = true;
            }
        }

        private void SinglesForMassRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cal_curve.cev.useSingles = ((RadioButton)sender).Checked;
        }

        private void DoublesForMassRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cal_curve.cev.useSingles = !((RadioButton)sender).Checked;
        }

        private void ConventionalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ConventionalRadioButton.Checked) cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.STD;
            TextBoxEnablementProcessingMethodOfFineness();
        }

        private void HeavyMetalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (HeavyMetalRadioButton.Checked) cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.HM;
            TextBoxEnablementProcessingMethodOfFineness();
        }

        private void PassiveUraniumRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PassiveUraniumRadioButton.Checked) cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.U;
            TextBoxEnablementProcessingMethodOfFineness();
        }

        private void CurveTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Now take string, find enum value
            INCCAnalysisParams.CurveEquation curve= (INCCAnalysisParams.CurveEquation)((ComboBox)sender).SelectedIndex;
            if (curve != mp.cev.cal_curve_equation)
            {
                mp.cev.cal_curve_equation = curve;
                mp.imd.modified = true;
            }
        }
        private void SaveChanges()
        {
            //Now using NumericTextBox
            //Save every time.
            mp.imd.modified = true;
            mp.cev.modified = true;
            mp.cev.lower_mass_limit = LowerMassLimitTextBox.Value;
            mp.cev.upper_mass_limit = UpperMassLimitTextBox.Value;
            mp.cev.a = ATextBox.Value;
            mp.cev.b = BTextBox.Value;
            mp.cev.c = CTextBox.Value;
            mp.cev.d = DTextBox.Value;
            mp.cev.var_a = VarianceATextBox.Value;
            mp.cev.var_b = VarianceBTextBox.Value;
            mp.cev.var_c = VarianceCTextBox.Value;
            mp.cev.var_d = VarianceDTextBox.Value;
            mp.cev.setcovar(Coeff.a,Coeff.b,CovarianceABTextBox.Value);
            mp.cev.setcovar(Coeff.a, Coeff.c, CovarianceACTextBox.Value);
            mp.cev.setcovar(Coeff.a, Coeff.d, CovarianceADTextBox.Value);
            mp.cev.setcovar(Coeff.b, Coeff.c, CovarianceBCTextBox.Value);
            mp.cev.setcovar(Coeff.b, Coeff.d, CovarianceBDTextBox.Value);
            mp.cev.setcovar(Coeff.c, Coeff.d, CovarianceCDTextBox.Value);
            mp.cev.sigma_x = SigmaXTextBox.Value;
            mp.cev.lower_mass_limit = LowerMassLimitTextBox.Value;
            mp.cev.upper_mass_limit = UpperMassLimitTextBox.Value;
            mp.cev.useSingles = SinglesForMassRadioButton.Checked;
            mp.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation) CurveTypeComboBox.SelectedIndex;

            if (HeavyMetalRadioButton.Checked)
        {
                cal_curve.heavy_metal_corr_factor = HvyMetalWeightingTextBox.Value;
                cal_curve.heavy_metal_reference = HvyMetalRefTextBox.Value;
                cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.HM;
        }
            else if (PassiveUraniumRadioButton.Checked)
        {
                cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.U;
                cal_curve.percent_u235 = U235PercentTextBox.Value;
        }
            else
        {
                cal_curve.CalCurveType = INCCAnalysisParams.CalCurveType.STD;
        }
            mp.Persist();
        }
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            SaveChanges();
            NCCReporter.Section sec = new NCCReporter.Section(null,0,0,0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = cal_curve.ToLines(null);
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

            if (cct != cal_curve.CalCurveType)
                mp.imd.modified = true;
            //Was not storing the changed values.HN
            //todo: use numerictextbox instead. Can check ranges. HN 9/22/2017
            SaveChanges();
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
