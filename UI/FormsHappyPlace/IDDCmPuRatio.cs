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

namespace UI
{
    using NC = NCC.CentralizedState;

    public partial class IDDCmPuRatio : Form
    {
        AnalysisDefs.INCCAnalysisParams.cm_pu_ratio_rec cmpuratio;
        public AnalysisDefs.AcquireParameters acq;

        public IDDCmPuRatio(AnalysisDefs.AcquireParameters acqp)
        {
            InitializeComponent();
            acq = acqp;
            AnalysisDefs.INCCAnalysisParams.cm_pu_ratio_rec lcmpuratio = NC.App.DB.Cm_Pu_RatioParameters.Get();
            if (lcmpuratio == null)
                cmpuratio = new AnalysisDefs.INCCAnalysisParams.cm_pu_ratio_rec();
            else
                cmpuratio = new AnalysisDefs.INCCAnalysisParams.cm_pu_ratio_rec(lcmpuratio);

            LabelIdTextBox.Text = cmpuratio.cm_id_label;
            InputBatchIdTextBox.Text = cmpuratio.cm_input_batch_id;
            IdTextBox.Text = cmpuratio.cm_id;
            CmPuRatioTextBox.Text = cmpuratio.cm_pu_ratio.v.ToString("F7"); 
            CmPuRatioErrorTextBox.Text = cmpuratio.cm_pu_ratio.err.ToString("F7");
            CmURatioTextBox.Text = cmpuratio.cm_u_ratio.v.ToString("F7");
            CmURatioErrorTextBox.Text = cmpuratio.cm_u_ratio.err.ToString("F7");
            HalfLifeTextBox.Text = cmpuratio.pu_half_life.ToString("F4");
            DeclaredU235TextBox.Text = cmpuratio.cm_dcl_u235_mass.ToString("F6");
            DeclaredUTextBox.Text = cmpuratio.cm_dcl_u_mass.ToString("F6");
            DeclaredPuTextBox.Text = acq.mass.ToString("F6");
            CmPuRatioDateTimePicker.Value = cmpuratio.cm_pu_ratio_date;
            CmURatioDateTimePicker.Value = cmpuratio.cm_u_ratio_date;
        }

        private void CmPuRatioDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!cmpuratio.cm_pu_ratio_date.Equals(dt))
            {
                cmpuratio.cm_pu_ratio_date = dt;
                cmpuratio.modified = true;
            }
         }

        private void CmURatioDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!cmpuratio.cm_u_ratio_date.Equals(dt))
            {
                cmpuratio.cm_u_ratio_date = dt;
                cmpuratio.modified = true;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // If changes have been made to parameters, write the results to the DB.
            if (cmpuratio.modified)
            {
                NC.App.DB.Cm_Pu_RatioParameters.Set(cmpuratio);  // event fires at scope of Cm_Pu_RatioParameters.GetList, writes changes to the DB
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            // exit withtout doing anything, any changes to cmpuratio are local and discarded here
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void CmPuRatioTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_pu_ratio.v;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d,1e-20,1e20));
            if (modified) { cmpuratio.cm_pu_ratio.v = d; cmpuratio.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_pu_ratio.v.ToString("F7");
        }

        private void CmPuRatioErrorTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_pu_ratio.err;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 1e20));
            if (modified) { cmpuratio.cm_pu_ratio.err = d; cmpuratio.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_pu_ratio.err.ToString("F7");
        }

        private void HalfLifeTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.pu_half_life;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 1, 1e20));
            if (modified) { cmpuratio.pu_half_life = d; cmpuratio.modified = true; }
            ((TextBox)sender).Text = cmpuratio.pu_half_life.ToString("F4");
        }

        private void DeclaredPuTextBox_Leave(object sender, EventArgs e)
        {
            Double d = acq.mass;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 1e6));
            if (modified) { acq.mass = d; acq.modified = true; }
            ((TextBox)sender).Text = acq.mass.ToString("F6");
        }

        private void DeclaredUTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_dcl_u_mass;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 1e6));
            if (modified) { cmpuratio.cm_dcl_u_mass = d; acq.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_dcl_u_mass.ToString("F6");
        }

        private void DeclaredU235TextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_dcl_u235_mass;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 1e6));
            if (modified) { cmpuratio.cm_dcl_u235_mass = d; acq.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_dcl_u235_mass.ToString("F6");
        }

        private void CmURatioTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_u_ratio.v;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 1e-20, 1e20));
            if (modified) { cmpuratio.cm_u_ratio.v = d; cmpuratio.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_u_ratio.v.ToString("F7");
        }

        private void CmURatioErrorTextBox_Leave(object sender, EventArgs e)
        {
            Double d = cmpuratio.cm_u_ratio.err;
            bool modified = (Format.ToDbl(((TextBox)sender).Text, ref d, 0, 1e20));
            if (modified) { cmpuratio.cm_u_ratio.err = d; cmpuratio.modified = true; }
            ((TextBox)sender).Text = cmpuratio.cm_u_ratio.err.ToString("F7");
        }

        private void LabelIdTextBox_Leave(object sender, EventArgs e)
        {
            if (!cmpuratio.cm_id_label.Equals(((TextBox)sender).Text))
            {
                cmpuratio.cm_id_label = ((TextBox)sender).Text;
                cmpuratio.modified = true;
            }
        }

        private void IdTextBox_Leave(object sender, EventArgs e)
        {
            if (!cmpuratio.cm_id.Equals(((TextBox)sender).Text))
            {
                cmpuratio.cm_id = ((TextBox)sender).Text;
                cmpuratio.modified = true;
            }
        }

        private void InputBatchIdTextBox_Leave(object sender, EventArgs e)
        {
            if (!cmpuratio.cm_input_batch_id.Equals(((TextBox)sender).Text))
            {
                cmpuratio.cm_input_batch_id = ((TextBox)sender).Text;
                cmpuratio.modified = true;
            }
        }
    }
}
