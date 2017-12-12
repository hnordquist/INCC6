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
namespace UI
{
	using Integ = NCC.IntegrationHelpers;

    public partial class IDDKValSelector : Form
    {
        public IDDKValSelector()
        {
            InitializeComponent();
            MessageBox.Show("This functionality is not implemented yet.", "DOING NOTHING NOW");
			acq = Integ.GetCurrentAcquireParams();
			if (acq.KValSource == KValsSource.AGTNMC)
				PANDARadioButton.Checked = true;
			else if (acq.KValSource == KValsSource.N_HENCC)
				N9588RadioButton.Checked = true;
			else if (acq.KValSource == KValsSource.ESARDA_128)
				ESARDA128RadioButton.Checked = true;
			else if (acq.KValSource == KValsSource.ESARDA_64)
				ESARDA64RadioButton.Checked = true;
        }
        AcquireParameters acq;

        // the constants and the index are defined in AnalysisDefs.KValsForAlpha

        private void OKBtn_Click(object sender, EventArgs e)
        {
                    // KValsSource   // used for Calibraton and Verification methods
			if (PANDARadioButton.Checked)
				acq.KValSource = KValsSource.AGTNMC;
			if (N9588RadioButton.Checked)
				acq.KValSource = KValsSource.N_HENCC;
			if (ESARDA128RadioButton.Checked)
				acq.KValSource = KValsSource.ESARDA_128;
			if (ESARDA64RadioButton.Checked)
				acq.KValSource = KValsSource.ESARDA_64;
              Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
