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
    using NC = NCC.CentralizedState;
    public partial class IDDFacilityAdd : Form
    {

        void RefreshCombo()
        {
            // Populate the combobox in the selector panel
            CurrentFacilitiesComboBox.Items.Clear();
            foreach (INCCDB.Descriptor d in NC.App.DB.Facilities.GetList())
            {
                    CurrentFacilitiesComboBox.Items.Add(d.Name);
            }
            if (CurrentFacilitiesComboBox.Items.Count >=1)
                CurrentFacilitiesComboBox.SelectedIndex = 0;
        }

        public IDDFacilityAdd()
        {
            InitializeComponent();
            RefreshCombo();
        }

        private void CurrentFacilitiesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FacilityTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void FacilityDescriptionTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(FacilityTextBox.Text))
            {

                INCCDB.Descriptor d = new INCCDB.Descriptor(FacilityTextBox.Text, FacilityDescriptionTextBox.Text);
                if (NC.App.DB.Facilities.Has(d))
                    return;
                INCCDB.Descriptor d2 = null;
                try{
                    NC.App.DB.Facilities.Get(d);
                }
                catch (InvalidOperationException)  // id not there, it;s new
                { }
                if (d2 != null) // if names match update description
                    NC.App.DB.Facilities.Delete(d2);
                d.modified = true;
                NC.App.DB.Facilities.GetList().Add(d);
                NC.App.DB.Facilities.Set(d);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                RefreshCombo();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {

        }
    }
}
