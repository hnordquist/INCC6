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
    using NC = NCC.CentralizedState;   
    public partial class IDDNormalizationSetup : Form
    {
        NormParameters np;
        Detector det;
        public IDDNormalizationSetup()
        {
            InitializeComponent();

            det = Integ.GetCurrentAcquireDetector();
            np = Integ.GetCurrentNormParams(det);
            this.Text += " for detector " + det.Id.DetectorName;
            switch (np.biasMode)
            {
                case NormTest.AmLiSingles:
                    UseAmLiRadioButton.Checked = true;
                    break;
                case NormTest.Cf252Doubles:
                    UseCf252DoublesRadioButton.Checked = true;
                    break;
                case NormTest.Cf252Singles:
                    UseCf252SinglesRadioButton.Checked = true;
                    break;
                case NormTest.Collar:
                    CollarRadioButton.Checked = true;
                    break;
            }
        }

        private void UseCf252DoublesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            np.biasMode = NormTest.Cf252Doubles;
        }

        private void UseCf252SinglesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            np.biasMode = NormTest.Cf252Singles;
        }

        private void UseAmLiRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            np.biasMode = NormTest.AmLiSingles;
        }

        private void CollarRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            np.biasMode = NormTest.Collar;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.Ignore; 
            switch (np.biasMode)
            {
                case NormTest.AmLiSingles:
                    IDDAmLiNormalization dlg = new IDDAmLiNormalization(np);
                    r = dlg.ShowDialog(this);
                    break;
                case NormTest.Cf252Doubles:
                    IDDCf252Normalization dlg2 = new IDDCf252Normalization(np);
                    r = dlg2.ShowDialog(this);
                    break;
                case NormTest.Cf252Singles:
                    IDDCf252SinglesNorm dlg3 = new IDDCf252SinglesNorm(np);
                    r = dlg3.ShowDialog(this);
                    break;
                case NormTest.Collar:
                    IDDCollarNormalization dlg4 = new IDDCollarNormalization(np);
                    r = dlg4.ShowDialog(this);
                    break;
            }
            if (r == DialogResult.OK)
            {
                /* if normalization constant or error has changed then set
                    the measured rate and error to zero. */
                NormParameters onp = NC.App.DB.NormParameters.GetMap()[det];
                if ((onp.currNormalizationConstant.v != np.currNormalizationConstant.v) ||
                        onp.currNormalizationConstant.err != np.currNormalizationConstant.err)
                {
                    np.measRate.Zero();
                }
                // copy changes back to original on user affirmation
                NC.App.DB.NormParameters.GetMap()[det] = np;  // in the in-memory map
                NC.App.DB.NormParameters.Set(det,np);  // in the database

                np.modified = false;
                this.Close();
            }
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
