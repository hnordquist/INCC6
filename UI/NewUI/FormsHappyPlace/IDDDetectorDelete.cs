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

    using Integ = NCC.IntegrationHelpers;
    using N = NCC.CentralizedState;
    public partial class IDDDetectorDelete : Form
    {
        Detector target;

        void RefreshDetectorCombo()
        {
            // Populate the combobox in the selector panel
            DetectorIdComboBox.Items.Clear();
            foreach (Detector d in N.App.DB.Detectors)
            {
                //if (!d.ListMode)
                    DetectorIdComboBox.Items.Add(d);
            }
            DetectorIdComboBox.SelectedIndex = 0;
        }

        public IDDDetectorDelete()
        {
            InitializeComponent();
            RefreshDetectorCombo();
            target = (Detector)DetectorIdComboBox.SelectedItem;
        }

        private void DetectorIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            target = (Detector)DetectorIdComboBox.SelectedItem;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (target == null)
                return;
            if (null != N.App.DB.Detectors.Find(d => String.Compare(d.Id.DetectorName, target.Id.DetectorName, true) == 0))
            {
                DialogResult r = MessageBox.Show(
                    string.Format("Do you want to delete detector {0}, including all its measurements and all its database parameters?", target.Id.DetectorName),
                     "Confirm Delete", MessageBoxButtons.YesNo);
                if (r == System.Windows.Forms.DialogResult.Yes)
                {
                    if (Integ.DeleteDetectorAndAssociations(target)) // removes from DB then from Detectors list, just like isotopics
                    {
                        target = null;
                        RefreshDetectorCombo();
                    }
                }
            }

        }

        /*
		URGENT doing this step by step
			
analysis_method_rec             INCCAnalysisMethodMap DetectorMaterialAnalysisMethods

// optional if defined, not in-memory, cascade delete causes deletion in DB, 
// these do not have in-memory maps or lists other than DetectorMaterialAnalysisMethods 
active_rec
active_mult_rec
active_passive_rec
add_a_source_rec
cal_curve_rec
collar_rec
collar_detector_rec
collar_k5_rec
curium_ratio_rec
known_alpha_rec
known_m_rec
multiplicity_rec





sr_parms_rec
sr_parms_ext
stratum_id_detector
stratum_id_instances
tm_bkg_parms_rec		

LMNetComm
LMHWParams
LMAcquireParams
CountingParams
LMMultiplicity

        */

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
                string msg = "Select the detector id you want to delete. WARNNG: all database parameters and measurements associated with this detector id will be deleted. ";
                MessageBox.Show(msg, "WARNING");
        }
    }
}
