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
                am.Normal = am.GetFirstSelected();
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
                am.Backup = am.GetSecondSelected();
            switch (am.Backup)
            {
                case AnalysisMethod.None:
                case AnalysisMethod.INCCNone:
                    break;
                case AnalysisMethod.CalibrationCurve:
                    BCalibrationCurveRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownA:
                    BKnownAlphaRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownM:
                    BKnownMRadioButton.Checked = true;
                    break;
                case AnalysisMethod.Multiplicity:
                    BMultiplicityRadioButton.Checked = true;
                    break;
                case AnalysisMethod.AddASource:
                    BAddASourceRadioButton.Checked = true;
                    break;
                case AnalysisMethod.CuriumRatio:
                    BCuriumRatioRadioButton.Checked = true;
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    BTruncatedMultiplicityRadioButton.Checked = true;
                    break;
            }

            if (am.Auxiliary.IsNone() || am.Auxiliary == am.Backup)
                am.Auxiliary = am.GetThirdSelected();
            switch (am.Auxiliary)
            {
                case AnalysisMethod.None:
                case AnalysisMethod.INCCNone:
                    break;
                case AnalysisMethod.CalibrationCurve:
                    ACalibrationCurveRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownA:
                    AKnownAlphaRadioButton.Checked = true;
                    break;
                case AnalysisMethod.KnownM:
                    AKnownMRadioButton.Checked = true;
                    break;
                case AnalysisMethod.Multiplicity:
                    AMultiplicityRadioButton.Checked = true;
                    break;
                case AnalysisMethod.AddASource:
                    AAddASourceRadioButton.Checked = true;
                    break;
                case AnalysisMethod.CuriumRatio:
                    ACuriumRatioRadioButton.Checked = true;
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    ATruncatedMultiplicityRadioButton.Checked = true;
                    break;
            }
        }

        void EnableThoseWhomAreChosen()
        {
            CalibrationCurveRadioButton.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];
            BCalibrationCurveRadioButton.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];
            ACalibrationCurveRadioButton.Enabled = am.choices[(int)AnalysisMethod.CalibrationCurve];

            KnownAlphaRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownA];
            BKnownAlphaRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownA];
            AKnownAlphaRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownA];

            KnownMRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownM];
            BKnownMRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownM];
            AKnownMRadioButton.Enabled = am.choices[(int)AnalysisMethod.KnownM];

            MultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];
            BMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];
            AMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.Multiplicity];

            AddASourceRadioButton.Enabled = am.choices[(int)AnalysisMethod.AddASource];
            BAddASourceRadioButton.Enabled = am.choices[(int)AnalysisMethod.AddASource];
            AAddASourceRadioButton.Enabled = am.choices[(int)AnalysisMethod.AddASource];

            TruncatedMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];
            BTruncatedMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];
            ATruncatedMultiplicityRadioButton.Enabled = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];

            CuriumRatioRadioButton.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
            BCuriumRatioRadioButton.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
            ACuriumRatioRadioButton.Enabled = am.choices[(int)AnalysisMethod.CuriumRatio];
        }
        private void CalibrationCurveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.CalibrationCurve, ((RadioButton)sender), BCalibrationCurveRadioButton, ACalibrationCurveRadioButton);
        }

        private void KnownAlphaRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.KnownA, ((RadioButton)sender), BKnownAlphaRadioButton, AKnownAlphaRadioButton);
        }

        private void KnownMRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.KnownM, ((RadioButton)sender), BKnownMRadioButton, AKnownMRadioButton);
        }

        private void MultiplicityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.Multiplicity, ((RadioButton)sender), BMultiplicityRadioButton, AMultiplicityRadioButton);
        }

        private void AddASourceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.AddASource, ((RadioButton)sender), BAddASourceRadioButton, AAddASourceRadioButton);
        }

        private void CuriumRatioRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.CuriumRatio, ((RadioButton)sender), BCuriumRatioRadioButton, ACuriumRatioRadioButton);
        }

        private void TruncatedMultiplicityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            GroupHug(AnalysisMethod.TruncatedMultiplicity, ((RadioButton)sender), BTruncatedMultiplicityRadioButton, ATruncatedMultiplicityRadioButton);
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

        private void OKBtn_Click(object sender, EventArgs e)
        {
                if (CalibrationCurveRadioButton.Checked)
                     am.Normal = AnalysisMethod.CalibrationCurve;
                else if (KnownAlphaRadioButton.Checked)
                    am.Normal = AnalysisMethod.KnownA;
                else if (KnownMRadioButton.Checked)
                    am.Normal = AnalysisMethod.KnownM;
                 else if (MultiplicityRadioButton.Checked)
                    am.Normal = AnalysisMethod.Multiplicity;
                else if (AddASourceRadioButton.Checked)
                    am.Normal = AnalysisMethod.AddASource;
                 else if (CuriumRatioRadioButton.Checked)
                    am.Normal = AnalysisMethod.CuriumRatio;
                else if (TruncatedMultiplicityRadioButton.Checked)
                    am.Normal = AnalysisMethod.TruncatedMultiplicity;

                if (BCalibrationCurveRadioButton.Checked)
                     am.Backup = AnalysisMethod.CalibrationCurve;
                else if (BKnownAlphaRadioButton.Checked)
                    am.Backup = AnalysisMethod.KnownA;
                else if (BKnownMRadioButton.Checked)
                    am.Backup = AnalysisMethod.KnownM;
                 else if (BMultiplicityRadioButton.Checked)
                    am.Backup = AnalysisMethod.Multiplicity;
                else if (BAddASourceRadioButton.Checked)
                    am.Backup = AnalysisMethod.AddASource;
                 else if (BCuriumRatioRadioButton.Checked)
                    am.Backup = AnalysisMethod.CuriumRatio;
                else if (BTruncatedMultiplicityRadioButton.Checked)
                    am.Backup = AnalysisMethod.TruncatedMultiplicity;

                if (ACalibrationCurveRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.CalibrationCurve;
                else if (AKnownAlphaRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.KnownA;
                else if (AKnownMRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.KnownM;
                else if (AMultiplicityRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.Multiplicity;
                else if (AAddASourceRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.AddASource;
                else if (ACuriumRatioRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.CuriumRatio;
                else if (ATruncatedMultiplicityRadioButton.Checked)
                    am.Auxiliary = AnalysisMethod.TruncatedMultiplicity;

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
