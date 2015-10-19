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
    public partial class IDDCollarCrossRef : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.collar_rec col;
        public void CrossReferenceFieldFiller(INCCAnalysisParams.collar_combined_rec col)
        {
        }
        public IDDCollarCrossRef()
        {
            InitializeComponent();
            MessageBox.Show("This functionality is not complete yet.", "RESULTS UNKNOWN AT THIS TIME");
            mp = new MethodParamFormFields(AnalysisMethod.COLLAR_DETECTOR_SAVE_RESTORE);

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.collar_rec((INCCAnalysisParams.collar_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.collar_rec(); // not mapped, so make a new one
                mp.imd.modified = true;

            }
            col = (INCCAnalysisParams.collar_rec)mp.imd;
            col.GenParamList();
            mp.RefreshMatTypeComboBox (MaterialTypeComboBox);
            //CrossReferenceFieldFiller(col);
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ReferenceDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void UnknownComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PoisonAbsorptionFactorTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RelativeDoublesRateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }

        private void NextBtn_Click(object sender, EventArgs e)
        {

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
