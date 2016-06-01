/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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

	public partial class IDDHeavyMetalItemData : Form
    {
        double umass, length;
		bool modified = false;

		// all of this so the item id and acquire params can be updated upon OK
        public Detector det;
        public AcquireParameters acq = null;

		public IDDHeavyMetalItemData(AnalysisMethods ams_, ItemId item)
        {
            InitializeComponent();
			umass = item.declaredUMass;
			length = item.length;
            DeclaredUMassTextBox.Text = umass.ToString("F3"); 
            LengthTextBox.Text = length.ToString("F3"); 
			Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
        }

        private void DeclaredUMassTextBox_Leave(object sender, EventArgs e)
        {
			//0.001, 1e6
            double d = umass;
            modified = (Format.ToDblBracket(((TextBox)sender).Text, ref d, 0.001, 1e6));
            if (modified) { umass = d; }
            ((TextBox)sender).Text = umass.ToString("F3");
        }

        private void LengthTextBox_Leave(object sender, EventArgs e)
        {
			// 0.001, 10000.0
            double d = length;
            modified = (Format.ToDblBracket(((TextBox)sender).Text, ref d, 0.001, 1e4));
            if (modified) { length = d; }
            ((TextBox)sender).Text = length.ToString("F3");
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
			if (modified)
			{
				ItemId id = NC.App.DB.ItemIds.Get(acq.ItemId.item);
				if (id != null)
				{
					id.length = length;
					id.declaredUMass = umass;
					id.modified = true;
					NC.App.DB.ItemIds.SetList();
					acq.ApplyItemId(id);
					NC.App.DB.UpdateAcquireParams(acq, det.ListMode); // update the acquire params too
				}
			}
			DialogResult = System.Windows.Forms.DialogResult.OK;
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
