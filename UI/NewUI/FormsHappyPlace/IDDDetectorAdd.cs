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
using DetectorDefs;
namespace NewUI
{
    
    using Integ = NCC.IntegrationHelpers;
    /* creating a detector also involves
            * creating an analysis selector for each (det,) material type in the DB
            * creating a stratum association for each detector in the DB
            * creating a det -> association with bkg, tm_bkg, norm, SR, AAS setup, Unattended setup, alpha-beta and hvparam
        */
    using NC = NCC.CentralizedState;

    /// <summary>
    /// Implement (see above)
    /// </summary>
    public partial class IDDDetectorAdd : Form
    {

        Detector model;
        bool srtype; 

        void RefreshDetectorCombo()
        {
            // Populate the combobox in the selector panel
            CurrentDetectorsComboBox.Items.Clear();
            foreach (Detector d in NC.App.DB.Detectors)
            {
                    CurrentDetectorsComboBox.Items.Add(d);
            }
        }

        public IDDDetectorAdd()
        {
            InitializeComponent();
            srtype = true;
            SR.Checked = true;
            LM.Checked = false;
            RefreshDetectorCombo();
            model = (Detector)CurrentDetectorsComboBox.SelectedItem;

            // load the LM type combo selector
            LMTypes.Items.Clear();
            foreach (INCCDB.Descriptor dt in NC.App.DB.DetectorTypes.GetList())
            {
                InstrType dty;
                System.Enum.TryParse<InstrType>(dt.Name, out dty);
                if (dty.IsListMode())
                    LMTypes.Items.Add(dty);
            }
            LMTypes.SelectedItem = InstrType.LMMM;
        }

        private void CurrentDetectorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model = (Detector)CurrentDetectorsComboBox.SelectedItem;
        }


        private void OKButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(DetectorIdTextBox.Text))
            {
                Detector newdet = null;
                if (srtype)
                {
                    newdet = Integ.CreateDetectorWithAssociations(model, DetectorIdTextBox.Text, ElectronicsIdTextBox.Text, DetectorTypeTextBox.Text);
                    IDDShiftRegisterSetup f = new IDDShiftRegisterSetup(newdet); // copies updated SR PAram value to the newdet instance, database persist follows below
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                       // new behavior: newdet.SRParams has the changes already;
                    }
                }
                else
                {
                    newdet = Integ.CreateDetectorWithAssociations(model, DetectorIdTextBox.Text, ElectronicsIdTextBox.Text, DetectorTypeTextBox.Text, (InstrType)LMTypes.SelectedItem);

                    AcquireParameters acq = Integ.GetCurrentAcquireParams(); 
                    LMConnectionParams f = new LMConnectionParams(newdet, acq, false);
                    f.StartWithLMDetail();
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    { 
                    }
                }
                Integ.PersistDetectorAndAssociations(newdet);
                RefreshDetectorCombo(); // Assuming the detector was added to the internal detector list by CreateDetectorWithAssociations, this will add a new detector to the detector list

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
        }

        private void ElectronicsIdTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void DetectorTypeTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void DetectorIdTextBox_Leave(object sender, EventArgs e)
        {
            if (null != NC.App.DB.Detectors.Find(d => String.Compare(d.Id.DetectorName, ((TextBox)sender).Text, true) == 0) )
            {
                 DialogResult r = MessageBox.Show(String.Format("Detector {0} already exists.",((TextBox)sender).Text));
                 ((TextBox)sender).Text = "";
            }

        }

        private void LMSR_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Name == "SR" && ((RadioButton)sender).Checked)
            {
                srtype = true;
                LMTypes.Visible = false;
            }
            else{
                LMTypes.Visible = true;
                srtype = false;
            }

        }
    }
}
