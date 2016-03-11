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



    public partial class IDDFacility : Form
    {

        AcquireParameters acq;
        Detector det;
        string initdetname;
        public IDDFacility()
        {
            InitializeComponent();
            FacilityComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Facilities.GetList())
            {
                FacilityComboBox.Items.Add(desc);
            }
            MBAComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
            {
                MBAComboBox.Items.Add(desc);
            }

            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            initdetname = String.Copy(det.Id.DetectorId);
            try
            {
                FacilityComboBox.SelectedItem = NC.App.DB.Facilities.Get(acq.facility.Name);
            }
            catch (InvalidOperationException)
            { }
            try
            {
                MBAComboBox.SelectedItem = NC.App.DB.MBAs.Get(acq.mba.Name);
            }
            catch (InvalidOperationException)
            { }

            DetectorIdComboBox.Items.Clear();
            foreach (Detector d in NC.App.DB.Detectors)
            {
                //if (!d.ListMode)
                    DetectorIdComboBox.Items.Add(d);
            }

            //Bolth.Checked = (det != null && det.ListMode);  // EH reloads combo box with LMs if true 
            if (det != null)
                DetectorIdComboBox.SelectedItem = det;
            DetectorParametersCheckBox.Checked = acq.review_detector_parms;
            CalibrationParametersCheckBox.Checked = acq.review_calib_parms;
            IsotopicsCheckBox.Checked = acq.review_isotopics;
            IndividualCycleRawDataCheckBox.Checked = acq.review_run_raw_data;
            IndividualCycleRateData.Checked = acq.review_run_rate_data;
            SummedRawCoincidenceDataCheckBox.Checked = acq.review_summed_raw_data;
            SummedMultiplicityDistributionsCheckBox.Checked = acq.review_summed_mult_dist;
            IndividualMultiplicityDistributionsCheckBox.Checked = acq.review_run_mult_dist;
            InspectorNameTextBox.Text = acq.user_id;
            InspectionNumberTextBox.Text = acq.campaign_id;
        }

        private void FacilityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            acq.facility = new INCCDB.Descriptor((INCCDB.Descriptor)FacilityComboBox.SelectedItem);
        }

        private void MBAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            acq.mba = new INCCDB.Descriptor((INCCDB.Descriptor)MBAComboBox.SelectedItem);
        }

        private void DetectorIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            det = (Detector)((ComboBox)sender).SelectedItem;
            acq.detector_id =  String.Copy(det.Id.DetectorId);
            acq.meas_detector_id = String.Copy(det.Id.DetectorId);
            ElectronicsIdTextBox.Text = det.Id.ElectronicsId;
            DetectorTypeTextBox.Text = det.Id.Type;
            acq.modified = true;
        }


        private void ChangeSysTimeBtn_Click(object sender, EventArgs e)
        {

        }

        private void ChangeDirBtn_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// On user Ok, preserve any changes to the current Acquire state as well as the current detector 
        /// </summary>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (acq.modified)
            {
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, DateTime.Now);
                acq.review_detector_parms = DetectorParametersCheckBox.Checked;
                acq.review_calib_parms = CalibrationParametersCheckBox.Checked;
                acq.review_isotopics = IsotopicsCheckBox.Checked;
                acq.review_run_raw_data = IndividualCycleRawDataCheckBox.Checked;
                acq.review_run_rate_data = IndividualCycleRateData.Checked;
                acq.review_summed_raw_data = SummedRawCoincidenceDataCheckBox.Checked;
                acq.review_summed_mult_dist = SummedMultiplicityDistributionsCheckBox.Checked;
                acq.review_run_mult_dist = IndividualMultiplicityDistributionsCheckBox.Checked;
                acq.user_id = InspectorNameTextBox.Text;
                acq.campaign_id = InspectionNumberTextBox.Text;
                acq.MeasDateTime = sel.TimeStamp;
                acq.lm.TimeStamp = sel.TimeStamp;
                // facility and mba already set by selector handlers
                NC.App.DB.AddAcquireParams(sel, acq);  // it's a new one, not the existing one modified
            }

            if (det.Id.modified)
                NC.App.DB.UpdateDetector(det);
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void ElectronicsIdTextBox_Leave(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.CompareTo(det.Id.ElectronicsId) != 0) 
            {
                det.Id.ElectronicsId = text; det.Id.modified = true;
            }
        }

        private void DetectorTypeTextBox_Leave(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.CompareTo(det.Id.Type) != 0)
            {
                det.Id.Type = text; det.Id.modified = true;
            }
        }

        private void InspectorNameTextBox_Leave(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.CompareTo(acq.user_id) != 0)
            {
                acq.user_id = text; acq.modified = true;
            }
        }

        private void InspectionNumberTextBox_Leave(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.CompareTo(acq.campaign_id) != 0)
            {
                acq.campaign_id = text; acq.modified = true;
            }
        }

        private void Bolth_CheckedChanged(object sender, EventArgs e)
        {
            if (Bolth.Checked)
            {
                DetectorIdComboBox.Items.Clear();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    DetectorIdComboBox.Items.Add(d);
                }
            }
            else
            {
                DetectorIdComboBox.Items.Clear();
                foreach (Detector d in NC.App.DB.Detectors)
                {
                    if (!d.ListMode)
                        DetectorIdComboBox.Items.Add(d);
                }
            }
        }

    }
}
