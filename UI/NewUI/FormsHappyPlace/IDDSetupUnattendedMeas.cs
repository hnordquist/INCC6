/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

    using NC = NCC.CentralizedState;


    public partial class IDDSetupUnattendedMeas : Form
    {
        Detector det;
        UnattendedParameters up;
        public IDDSetupUnattendedMeas(Detector d)
        {
            InitializeComponent();
            det = d;
            up = new UnattendedParameters();
			if (det == null)
				return;
            if (NC.App.DB.UnattendedParameters.Map.ContainsKey(det))
                up.Copy(NC.App.DB.UnattendedParameters.Map[det]);
            MaxTimeTextBox.Text = up.ErrorSeconds.ToString();
            AutoImportCheckBox.Checked = up.AutoImport;
            DoublesThresholdTextBox.Text = up.AASThreshold.ToString("F1");
            Text += " for detector " + det.Id.DetectorName;
        }

        private void MaxTimeTextBox_Leave(object sender, EventArgs e)
        {
            uint u = up.ErrorSeconds;
            bool b = Format.ToUInt(((TextBox)sender).Text, ref u);
            ((TextBox)sender).Text = u.ToString();
            up.modified |= b;
            if (b) up.ErrorSeconds = u;
        }

        private void AutoImportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            up.AutoImport = AutoImportCheckBox.Checked;
        }

        private void DoublesThresholdTextBox_Leave(object sender, EventArgs e)
        {
            double d = up.AASThreshold;
            bool b = Format.ToNN(((TextBox)sender).Text, ref d);
            ((TextBox)sender).Text = d.ToString("F1");
            up.modified |= b;
            if (b) up.AASThreshold = d;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            UnattendedParameters cur = NC.App.DB.UnattendedParameters.Map[det];
            if (up.CompareTo(cur) != 0)
			{
				// copy changes back to original
				NC.App.DB.UnattendedParameters.Map[det] = up;  // in the in-memory map
                NC.App.DB.UnattendedParameters.Set(det, up);  // in the database
			}
			DialogResult = DialogResult.OK;
			Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }



    }
}
