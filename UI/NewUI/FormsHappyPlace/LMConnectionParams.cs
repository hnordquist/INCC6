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
using DetectorDefs;
namespace NewUI
{

    using NC = NCC.CentralizedState;


    public partial class LMConnectionParams : Form
    {

        string oTitle;
        AcquireParameters acq;
        Detector det;
        public void StartWithLMDetail()
        {
            this.SelectorPanel.Visible = false;
            this.AddDetectorTypePanel.Visible = false;
            if (det.Id.SRType == InstrType.LMMM)
            {
                this.LMMMPanel.Visible = true;
                this.LMMMBackBtn.Enabled = false;
            }
            else if (det.Id.SRType == InstrType.PTR32)
            {
                this.PTR32Panel.Visible = true;
                this.PTR32Back.Enabled = false;
            }

            this.Text = oTitle + (" for " + det.Id.DetectorName);
        }
        void RefreshDetectorCombo()
        {
            // Populate the combobox in the selector panel
            DetectorComboBox.Items.Clear();
            foreach (Detector d in NC.App.DB.Detectors)
            {
                if (d.ListMode)
                    DetectorComboBox.Items.Add(d);
            }
        }

        void RefreshDetectorTypeCombo()
        {
            AddDetectorTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor dt in NC.App.DB.DetectorTypes.GetLMList())
            {
                InstrType dty;
                System.Enum.TryParse<InstrType>(dt.Name, out dty);
                if (dty.IsListMode())
                    AddDetectorTypeComboBox.Items.Add(dt);
            }
        }


        // Constructor including initialization of comboboxen in the various panels
        public LMConnectionParams(Detector candidate, AcquireParameters acq, bool isnew)
        {
            AddingNew = isnew;
            InitializeComponent();
            oTitle = this.Text;

            // Reposition the various panels on top of each other
            this.SelectorPanel.Top = 4;
            this.SelectorPanel.Left = 6;
            this.LMMMPanel.Top = 4;
            this.LMMMPanel.Left = 6;
            this.PTR32Panel.Top = 4;
            this.PTR32Panel.Left = 6;
            this.AddDetectorTypePanel.Top = 4;
            this.AddDetectorTypePanel.Left = 6;
            this.AddDetectorTypePanel.Top = 4;
            this.AddDetectorTypePanel.Left = 6;

            RefreshDetectorCombo();
            DetectorComboBox.SelectedItem = candidate;

            RefreshDetectorTypeCombo();
            AddDetectorTypeComboBox.SelectedItem = candidate.Id.SRType;

            det = candidate;
            this.acq = acq;
            PopulateParamFields();

        }

        // Depending on the shift register type for d, fill in the fields in the appropriate panel and make it visible.
        private void PopulateParamFields() 
        {
            if (det != null)
            {
                if (det.Id.SRType == InstrType.LMMM) 
                {    // Fill edit panel fields
                    // LMNetComm
                    LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
                    LMMMSubnetTextBox.Text = l.NetComm.Subnet;
                    LMMMLocalPortTextBox.Text = l.NetComm.Port.ToString();
                    LMMMRemotePortTextBox.Text = l.NetComm.LMListeningPort.ToString();
                    LMMMDiscWaitTimeTextBox.Text = l.NetComm.Wait.ToString();
                    LMMMBroadcastCheckBox.Checked = l.NetComm.Broadcast;

                    LMMMConnectionsTextBox.Text = l.NetComm.NumConnections.ToString();
                    LMMMBufferTextBox.Text = l.NetComm.ReceiveBufferSize.ToString();
                    LMMMEventBufferTextBox.Text = l.NetComm.ParseBufferSize.ToString(); 
                    LMMMStreamCheckBox.Checked = l.NetComm.UsingStreamRawAnalysis;
                    LMMMAsyncFileCheckBox.Checked = l.NetComm.UseAsynchFileIO;
                    LMMMSyncAnalysisCheckBox.Checked = !l.NetComm.UseAsynchAnalysis;

                    // LMHWConfig
                    LMMMInModeComboBox.SelectedIndex = l.DeviceConfig.Input;  // 0 is ribbon, 1 is TTL
                    LMMMDebugFlag.Checked = (l.DeviceConfig.Debug == 1 ? true : false);
                    LMMMHV.Text = l.DeviceConfig.HV.ToString();
                    LMMMLEDs.Checked = (l.DeviceConfig.LEDs == 2 ? true : false);
                    LMMMLLDmV.Text = l.DeviceConfig.LLD.ToString();

                    LMMMIntervalTextBox.Text = acq.lm.Separation.ToString();
                    LMMMFeedbackFlagCheckBox.Checked = acq.lm.Feedback;

                    // Make edit panel visible
                    this.LMMMPanel.Visible = true;
                }
                else if (det.Id.SRType == InstrType.PTR32 || det.Id.SRType == InstrType.MCA527)
                {
                    this.PTR32Panel.Visible = true;
					if (det.Id.SRType == InstrType.MCA527)
					{
						MCAName.Visible = true;
						MCAName.Text = String.Copy(det.Id.DetectorId);
						ConnIdField.Text = String.Copy(det.Id.ElectronicsId);
						connIdLabel.Text = "MCA-527 serial number: ";
						connLabel.Text = "MCA-527 Connection Parameters";
						//Device.MCADeviceInfo[] deviceInfos = Device.MCADevice.QueryDevices();
						//MCAComboBox.Visible = (deviceInfos.Length > 0);
					}
					else
					{ 
						MCAName.Visible = false;
						ConnIdField.Text = String.Copy(det.Id.DetectorId);
						connIdLabel.Text = "PTR-32 instrument identifier: ";
						connLabel.Text = "PTR-32 Connection Parameters";
					}

                    //Warning: hack attack.  using extra fields here to store hv enabling and tolerance hn 2.3.2015
                    // OK, was logic error.  Checked box means HV disabled..... fixed.  hn 5.19.2015
                    LMConnectionInfo lmi = (LMConnectionInfo)(det.Id.FullConnInfo);
                    check_HV_set.Checked = lmi.DeviceConfig.LEDs == 2 ? true : false;
                    VoltageTimeout.Visible = true;
                    VoltageTimeout.Text = lmi.DeviceConfig.HVTimeout.ToString();
                    VoltageTolerance.Text = lmi.DeviceConfig.VoltageTolerance.ToString();
                    VoltageTolerance.Visible = true;
                           
                }

            }
        }


  //// SELECTOR PANEL ////////////////////////////////////////////////////////////////////////////////////
  
        private void CancelButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButt_Click(object sender, EventArgs e)
        {

        }

        private void DetectorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EditBtn.Enabled = true;
            this.DeleteBtn.Enabled = true;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            this.SelectorPanel.Visible = false;
            if (DetectorComboBox.SelectedItem != null)
            {
                det = (Detector)DetectorComboBox.SelectedItem;
                PopulateParamFields();
                this.Text = oTitle + (" for " + det.Id.DetectorName);
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (NCC.IntegrationHelpers.DeleteDetectorAndAssociations(det)) // removes from DB then from Detectors list, also punts teh related parameters
                det = null;
            RefreshDetectorCombo();

        }
            
        private void AddDetectorBtn_Click(object sender, EventArgs e)
        {
            // Activate the AddDetectorType selector list
            this.SelectorPanel.Visible = false;
            this.AddDetectorTypePanel.Visible = true;
        }


   //// ADD A DETECTOR /////////////////////////////////////////////////////////////////////////////////////

        private void AddDetectorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enabled the next button if they've chosen a type and added a name
            if ((this.AddDetectorNameTextBox.Text != "") && (this.AddDetectorTypeComboBox.Text != ""))
            {
                this.AddNextBtn.Enabled = true;
            }
        }

        private void AddDetectorNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // Enabled the next button if they've chosen a type and added a name
            if ((!String.IsNullOrWhiteSpace(AddDetectorNameTextBox.Text)) && (!String.IsNullOrWhiteSpace(AddDetectorTypeComboBox.Text)))
            {
                this.AddNextBtn.Enabled = true;
            }
        }

        bool AddingNew;
        private void AddNextBtn_Click(object sender, EventArgs e)
        {
            this.SelectorPanel.Visible = false;
            this.AddDetectorTypePanel.Visible = false;

            // Create new detector with default parameters for the selected type
            DataSourceIdentifier did = new DataSourceIdentifier();
            did.DetectorName = AddDetectorNameTextBox.Text;
            InstrType dt;
            INCCDB.Descriptor desc = (INCCDB.Descriptor)AddDetectorTypeComboBox.SelectedItem;
            System.Enum.TryParse<InstrType>(desc.Name, out dt);
            did.SRType = dt;
            did.FullConnInfo = new LMConnectionInfo();
            Multiplicity mkey = new Multiplicity(InstrTypeExtensions.DefaultFAFor(did.SRType));
            det = new Detector(did, mkey, null);

            // Jump to an edit panel for the parameters of the appropriate type
           PopulateParamFields();
           AddingNew = true;

        }

        private void AddBackBtn_Click(object sender, EventArgs e)
        {
            AddingNew = false;
            this.AddDetectorTypePanel.Visible = false;
            this.SelectorPanel.Visible = true;
        }

   //// LMMM PARAMETERS ////////////////////////////////////////////////////////////////////////////////////

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.LMMMPanel.Visible = false;
            this.SelectorPanel.Visible = true;
        }

        private void AddDetectorNameTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void LMMMOKBtn_Click(object sender, EventArgs e)
        {
            // set the LMMM params on the current LMMM acquire and det def pair
            //LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            if (AddingNew)
            {
                NC.App.DB.Detectors.AddOnlyIfNotThere(det);
                RefreshDetectorCombo();
                AddingNew = false;
            }
            NC.App.DB.UpdateDetector(det);
            if (acq.modified)
            {
                NC.App.DB.UpdateAcquireParams(acq, det.ListMode);
                NC.App.DB.AcquireParametersMap()[new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime)] = acq;
            }
            this.LMMMPanel.Visible = false;  // like the back button
            this.SelectorPanel.Visible = true;

            //if (INCCEntry)
                this.Close();

        }
        bool modified;
        private void LMMMSubnetTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            String s = l.NetComm.Subnet;
            modified |= Format.Changed(((TextBox)sender).Text, ref s);
            if (modified)
                l.NetComm.Subnet = s;
        }

        private void LMMMLocalPortTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.Port;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.Port = i;
        }

        private void LMMMRemotePortTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.LMListeningPort;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.LMListeningPort = i;
        }

        private void LMMMInModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.Input;
            if (((ComboBox)sender).SelectedIndex != i)
            {
                l.DeviceConfig.Input = ((ComboBox)sender).SelectedIndex;
                modified |= true;
            }
        }

        private void LMMMDiscWaitTimeTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.Wait;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.Wait = i;
        }

        private void LMMMBroadcastCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            modified |= (l.NetComm.Broadcast != ((CheckBox)sender).Checked);
             l.NetComm.Broadcast = !((CheckBox)sender).Checked;
        }

        private void LMMMIntervalTextBox_Leave(object sender, EventArgs e)
        {
            int i = acq.lm.Separation;
            acq.modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (acq.modified)
                acq.lm.Separation = i;
        }

        private void LMMMFeedbackFlagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool bi = acq.lm.Feedback;
            acq.modified |= (acq.lm.Feedback != ((CheckBox)sender).Checked);
            if (acq.modified)
                acq.lm.Feedback = ((CheckBox)sender).Checked;
        }

        private void LMMMConnectionsTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.NumConnections;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.NumConnections = i;
        }

        private void LMMMBufferTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.ReceiveBufferSize;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.ReceiveBufferSize = i;
        }

        private void LMMMEventBufferTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            uint i = l.NetComm.ParseBufferSize;
            modified |= Format.ToNN(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.ParseBufferSize = i;
        }

        private void LMMMSyncAnalysisCheckBox_CheckedChanged(object sender, EventArgs e)
        {
             LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
             modified |= (l.NetComm.UseAsynchAnalysis != ((CheckBox)sender).Checked);
             l.NetComm.UseAsynchAnalysis = !((CheckBox)sender).Checked;
        }

        private void LMMMAsyncFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            modified |= (l.NetComm.UseAsynchFileIO != ((CheckBox)sender).Checked);
            l.NetComm.UseAsynchFileIO = ((CheckBox)sender).Checked;
        }

        private void LMMMStreamCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            modified |= (l.NetComm.UsingStreamRawAnalysis != ((CheckBox)sender).Checked);
            l.NetComm.UsingStreamRawAnalysis = ((CheckBox)sender).Checked;
        }

        private void LMMMHV_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.HV;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.HV = i;
        }

        private void LMMMDebugFlag_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int val =  ((CheckBox)sender).Checked ? 1 : 0;
            modified |= (l.DeviceConfig.Debug != val);
            l.DeviceConfig.Debug = val;
        }

        private void LMMMLLDmV_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.LLD;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.LLD = i;
        }

        private void LMMMLEDs_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int val = ((CheckBox)sender).Checked ? 2 : 1;  // the 1 and 2 must be an LMMM-specific detail
            modified |= (l.DeviceConfig.LEDs != val);
            l.DeviceConfig.LEDs = val;
        }

        private void PTR32Ok_Click(object sender, EventArgs e)
        {
            // set the params on the current PTR32 acquire and det def pair
            if (AddingNew)
            {
                NC.App.DB.Detectors.AddOnlyIfNotThere(det);
                RefreshDetectorCombo();
                AddingNew = false;
            }
            if (modified)
                NC.App.DB.UpdateDetectorParams(det); // the only thing that changes here are the LM Connection HW params

            if (acq.modified)
            {
                NC.App.DB.UpdateAcquireParams(acq, det.ListMode);
                NC.App.DB.AcquireParametersMap()[new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime)] = acq;
            }
            this.PTR32Panel.Visible = false;  // like the back button
            this.SelectorPanel.Visible = true;

            //if (INCCEntry)
            this.Close();
        }

        private void PTR32Back_Click(object sender, EventArgs e)
        {
            this.PTR32Panel.Visible = false;
            this.SelectorPanel.Visible = true;
        }

        private void PTR32Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PTR32Help_Click(object sender, EventArgs e)
        {

        }

        private void PTR32Id_Leave(object sender, EventArgs e)
        {
            // Set the current PTR32 id somewhere, e.g. current detector det.Id.FullConnInfo
            
            //modified |= 
        }

        private void VoltageTimeout_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.HVTimeout;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.HVTimeout = i;
        }

        private void check_HV_set_CheckedChanged(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
           // int tolerance = l.DeviceConfig.LLD;
           // int to = l.DeviceConfig.HVTimeout;
            int disabled = l.DeviceConfig.LEDs;
            if (((CheckBox)sender).Checked == true)
            {
                //l.DeviceConfig.HVTimeout = 0;
                l.DeviceConfig.LEDs = 2;
                //l.DeviceConfig.LLD = 0;
                VoltageTolerance.Enabled = false;
                VoltageTolerance.ReadOnly = true;
                VoltageTimeout.Enabled = false;
                VoltageTimeout.ReadOnly = true;
            }
            else
            {
                VoltageTolerance.Enabled = true;
                VoltageTolerance.ReadOnly = false;
                VoltageTimeout.Enabled = true;
                VoltageTimeout.ReadOnly = false;
                l.DeviceConfig.LEDs = 1;
                //l.DeviceConfig.HVTimeout = 0;
                //l.DeviceConfig.LLD = 1;
                // Int32.TryParse(VoltageTolerance.Text, out tolerance);
                // This is a hack for the moment, just a place to store our definable tolerance hn 2.3.3015
               // l.DeviceConfig.LLD = tolerance;
            }
            modified = /* to != l.DeviceConfig.HVTimeout || tolerance != l.DeviceConfig.LLD || */ 
                disabled != l.DeviceConfig.LEDs;
        }

        private void VoltageTolerance_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.VoltageTolerance;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.VoltageTolerance = i;
        }

    }
}
