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
    public partial class IDDCf252SinglesNorm : Form
    {
        public NormParameters np;

        public IDDCf252SinglesNorm(NormParameters npp)
        {
            np = npp;
            InitializeComponent();
            RefSinglesDateTimePicker.Value = np.refDate;

            CfSourceIdTextBox.Text = np.sourceId;

            NormConstTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            NormConstTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            NormConstTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            NormConstTextBox.Value = np.currNormalizationConstant.v;

            NormConstErrTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            NormConstErrTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            NormConstErrTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            NormConstErrTextBox.Value = np.currNormalizationConstant.err;

            AccLimitPercentTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            AccLimitPercentTextBox.NumberFormat = NumericTextBox.Formatter.F2;
            AccLimitPercentTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            AccLimitPercentTextBox.Value = np.acceptanceLimitPercent;

            RefSinglesRateTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            RefSinglesRateTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            RefSinglesRateTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            RefSinglesRateTextBox.Value = np.cf252RefDoublesRate.v;

            RefSinglesRateErrorTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            RefSinglesRateErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            RefSinglesRateErrorTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            RefSinglesRateErrorTextBox.Value = np.cf252RefDoublesRate.err;

            PrecisionLimitTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            PrecisionLimitTextBox.NumberFormat = NumericTextBox.Formatter.F1;
            PrecisionLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            PrecisionLimitTextBox.Value = np.initSrcPrecisionLimit;

            AccLimitStdDevTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            AccLimitStdDevTextBox.NumberFormat = NumericTextBox.Formatter.F1;
            AccLimitStdDevTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            AccLimitStdDevTextBox.Value = np.acceptanceLimitStdDev;
        }

        private void CfSourceIdTextBox_Leave(object sender, EventArgs e)
        {
            if (np.sourceId.CompareTo(((TextBox)sender).Text) == 0)
                return;
            np.sourceId = ((TextBox)sender).Text;
            np.modified = true;
        }

        private void RefSinglesDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (np.refDate != dt)
            {
                np.refDate = dt;
                np.modified = true;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            bool mod = NormConstTextBox.Value != np.currNormalizationConstant.v || NormConstErrTextBox.Value != np.currNormalizationConstant.err || RefSinglesRateTextBox.Value != np.cf252RefDoublesRate.v
                || RefSinglesRateErrorTextBox.Value != np.cf252RefDoublesRate.err || AccLimitPercentTextBox.Value != np.acceptanceLimitPercent || PrecisionLimitTextBox.Value != np.initSrcPrecisionLimit
                || AccLimitStdDevTextBox.Value != np.acceptanceLimitStdDev;
            
            if (np.modified || mod)
            {

                np.currNormalizationConstant.v = NormConstTextBox.Value;
                np.currNormalizationConstant.err = NormConstErrTextBox.Value;
                np.cf252RefDoublesRate.v = RefSinglesRateTextBox.Value;
                np.cf252RefDoublesRate.err = RefSinglesRateErrorTextBox.Value;
                np.acceptanceLimitPercent = AccLimitPercentTextBox.Value;
                np.initSrcPrecisionLimit = PrecisionLimitTextBox.Value;
                np.acceptanceLimitStdDev = AccLimitStdDevTextBox.Value;
                DialogResult = DialogResult.OK;
                np.biasPrecisionLimit = np.initSrcPrecisionLimit;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/normalizationsetup.htm");
        }


    }
}
