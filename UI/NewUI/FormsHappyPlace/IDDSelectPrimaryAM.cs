/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
    public partial class IDDSelectPrimaryAM : Form
    {
        AnalysisMethods am;

        public IDDSelectPrimaryAM(AnalysisDefs.AnalysisMethods aam)
        {
            am = aam;
            InitializeComponent();
            EnableThoseWhomAreChosen();
            if (am.Normal == AnalysisMethod.None)
                am.Normal = GetFirst();

            switch (am.Normal)
            {
                case AnalysisMethod.None:
                case AnalysisMethod.INCCNone:
                    break;
                case AnalysisMethod.CalibrationCurve:
                    CalibrationCurveRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownA:
                    KnownAlphaRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownM:
                    KnownMRadioButton.Checked = true;
                    break;
                case AnalysisMethod.Multiplicity:
                    MultiplicityRadioButton.Checked = true;
                    break;
                case AnalysisMethod.AddASource:
                    AddASourceRadioButton.Checked = true;
                    break;
                case AnalysisMethod.CuriumRatio:
                    CuriumRatioRadioButton.Checked = true;
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    TruncatedMultiplicityRadioButton.Checked = true;
                    break;
            }

            if (am.Backup.IsNone() || am.Normal == am.Backup)
                am.Backup = GetNext(am.Normal);
            switch (am.Backup)
            {
                case AnalysisMethod.None:
                case AnalysisMethod.INCCNone:
                    break;
                case AnalysisMethod.CalibrationCurve:
                    BCalibrationCurveCheckBox.Checked = true;
                    break;
                case AnalysisMethod.KnownA:
                    BKnownAlphaCheckBox.Checked = true;
                    break;
                case AnalysisMethod.KnownM:
                    BKnownMCheckBox.Checked = true;
                    break;
                case AnalysisMethod.Multiplicity:
                    BMultiplicityCheckBox.Checked = true;
                    break;
                case AnalysisMethod.AddASource:
                    BAddASourceCheckBox.Checked = true;
                    break;
                case AnalysisMethod.CuriumRatio:
                    BCuriumRatioCheckBox.Checked = true;
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    BTruncatedMultiplicity.Checked = true;
                    break;
            }
        }

        AnalysisMethod GetFirst()
        {
            int res = 0;
            for (int i = (int)AnalysisMethod.CalibrationCurve; i <= (int)AnalysisMethod.TruncatedMultiplicity; i++)
            {
                if (i >= (int)AnalysisMethod.Active && i <= (int)AnalysisMethod.ActivePassive)
                    continue;
                if (i == (int)AnalysisMethod.INCCNone)  // better to make an enumerator that skips this shit internally 
                    continue;
                if (am.choices[i])
                {
                    res = i;
                    break;
                }
            }
            return (AnalysisMethod)res;
        }

        void EnableThoseWhomAreChosen()
        {
            CalibrationCurveRadioButton.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];
            BCalibrationCurveCheckBox.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];
            ACalibrationCurveCheckBox.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];

            KnownAlphaRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownA];
            BKnownAlphaCheckBox.Enabled = am.choices[(int)AnalysisMethod.KnownA];
            AKnownAlphaCheckBox.Enabled = am.choices[(int)AnalysisMethod.KnownA];

            KnownMRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownM];
            BKnownMCheckBox.Enabled = am.choices[(int)AnalysisMethod.KnownM];
            AKnownMCheckBox.Enabled = am.choices[(int)AnalysisMethod.KnownM];

            MultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];
            BMultiplicityCheckBox.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];
            AMultiplicityCheckBox.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];

            AddASourceRadioButton.Enabled = am.choices[(int)AnalysisMethod.AddASource];
            BAddASourceCheckBox.Enabled = am.choices[(int)AnalysisMethod.AddASource];
            AAddASourceCheckBox.Enabled = am.choices[(int)AnalysisMethod.AddASource];

            TruncatedMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];
            BTruncatedMultiplicity.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];
            ATruncatedMultiplicityCheckBox.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];

            CuriumRatioRadioButton.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
            BCuriumRatioCheckBox.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
            ACuriumRatioCheckBox.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
        }
        AnalysisMethod GetNext(AnalysisMethod firstm)
        {
            int res = 0;
            int first = (int)firstm;
            for (int i = (int)AnalysisMethod.CalibrationCurve; i <= (int)AnalysisMethod.TruncatedMultiplicity; i++)
            {
                if (i >= (int)AnalysisMethod.Active && i <= (int)AnalysisMethod.ActivePassive)
                    continue;
                if (i == (int)AnalysisMethod.INCCNone)
                    continue;
                if (am.choices[i] && i != first)
                {
                    res = i;
                    break;
                }
            }
            return (AnalysisMethod)res;
        }
        private void CalibrationCurveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.CalibrationCurve, ((RadioButton)sender), BCalibrationCurveCheckBox, ACalibrationCurveCheckBox);
        }

        private void KnownAlphaRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.KnownA, ((RadioButton)sender), BKnownAlphaCheckBox, AKnownAlphaCheckBox);
        }

        private void KnownMRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.KnownM, ((RadioButton)sender), BKnownMCheckBox, AKnownMCheckBox);
        }

        private void MultiplicityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.Multiplicity, ((RadioButton)sender), BMultiplicityCheckBox, AMultiplicityCheckBox);
        }

        private void AddASourceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.AddASource, ((RadioButton)sender), BAddASourceCheckBox, AAddASourceCheckBox);
        }

        private void CuriumRatioRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.CuriumRatio, ((RadioButton)sender), BCuriumRatioCheckBox, ACuriumRatioCheckBox);
        }

        private void TruncatedMultiplicityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.TruncatedMultiplicity, ((RadioButton)sender), BTruncatedMultiplicity, ATruncatedMultiplicityCheckBox);
        }

        private void GroupHug(AnalysisMethod anm, RadioButton rb, RadioButton B, RadioButton A)
        {
            bool ckd = rb.Checked;
            if (am.choices[(int)anm])
            {
                B.Enabled = !ckd;
                A.Enabled = !ckd;
            }
            if (ckd)
            {
                B.Checked = !ckd;
                A.Checked = !ckd;
                am.Normal = anm;
            }
        }

        private void BCalibrationCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BKnownAlphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BKnownMCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BMultiplicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BAddASourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BCuriumRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BTruncatedMultiplicity_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACalibrationCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AKnownAlphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AKnownMCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AMultiplicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void AAddASourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ACuriumRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ATruncatedMultiplicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            //am.choices[(int)AnalysisMethod.CalibrationCurve] = CalibrationCurveRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.KnownA] = KnownAlphaRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.KnownM] = KnownMRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.Multiplicity] = MultiplicityRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.AddASource] = AddASourceRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.TruncatedMultiplicity] = TruncatedMultiplicityRadioButton.Checked;
            //am.choices[(int)AnalysisMethod.CuriumRatio] = CuriumRatioRadioButton.Checked;

                if (CalibrationCurveRadioButton.Checked)
                     am.Normal = AnalysisMethod.CalibrationCurve;
                else if (KnownAlphaRadioButton.Checked)
                    am.Normal = AnalysisMethod.KnownA;
                else if (KnownMRadioButton.Checked)
                    am.Normal = AnalysisMethod.KnownM;
                 else if (CalibrationCurveRadioButton.Checked)
                    am.Normal = AnalysisMethod.Multiplicity;
                else if (AddASourceRadioButton.Checked)
                    am.Normal = AnalysisMethod.AddASource;
                 else if (CuriumRatioRadioButton.Checked)
                    am.Normal = AnalysisMethod.CuriumRatio;
                else if (TruncatedMultiplicityRadioButton.Checked)
                    am.Normal = AnalysisMethod.TruncatedMultiplicity;

                if (BCalibrationCurveCheckBox.Checked)
                     am.Backup = AnalysisMethod.CalibrationCurve;
                else if (BKnownAlphaCheckBox.Checked)
                    am.Backup = AnalysisMethod.KnownA;
                else if (BKnownMCheckBox.Checked)
                    am.Backup = AnalysisMethod.KnownM;
                 else if (BCalibrationCurveCheckBox.Checked)
                    am.Backup = AnalysisMethod.Multiplicity;
                else if (BAddASourceCheckBox.Checked)
                    am.Backup = AnalysisMethod.AddASource;
                 else if (BCuriumRatioCheckBox.Checked)
                    am.Backup = AnalysisMethod.CuriumRatio;
                else if (BTruncatedMultiplicity.Checked)
                    am.Backup = AnalysisMethod.TruncatedMultiplicity;
                DialogResult = System.Windows.Forms.DialogResult.OK;
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
