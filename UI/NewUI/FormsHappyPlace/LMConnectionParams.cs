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
using System.Net;

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
            SelectorPanel.Visible = false;
            AddDetectorTypePanel.Visible = false;
            if (det.Id.SRType == InstrType.ALMM)
            {
                ALMMPanel.Visible = true;
                ALMMBackBtn.Enabled = false;
            }
            else if (det.Id.SRType == InstrType.PTR32 || det.Id.SRType == InstrType.MCA527)
            {
                PTR32Panel.Visible = true;
                PTR32Back.Enabled = false;
            }

            Text = oTitle + (" for " + det.Id.DetectorName);
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
			oTitle = Text;

			// Reposition the various panels on top of each other
			SelectorPanel.Top = 4;
			SelectorPanel.Left = 6;
			ALMMPanel.Top = 4;
			ALMMPanel.Left = 6;
			PTR32Panel.Top = 4;
			PTR32Panel.Left = 6;
			AddDetectorTypePanel.Top = 4;
			AddDetectorTypePanel.Left = 6;
			AddDetectorTypePanel.Top = 4;
			AddDetectorTypePanel.Left = 6;

			RefreshDetectorCombo();
			DetectorComboBox.SelectedItem = candidate;

			RefreshDetectorTypeCombo();
			AddDetectorTypeComboBox.SelectedItem = candidate.Id.SRType;

			det = candidate;
			this.acq = acq;
			MCAComboBox.Visible = false;
			if (det != null)
			{
				if (det.Id.SRType == InstrType.MCA527)
				{
					PopulateMCA527ParamFields();
				} else
				{
					PopulateALMM_PTR32ParamFields();
				}
			}
		}

		// Depending on the shift register type for d, fill in the fields in the appropriate panel and make it visible.
		private void PopulateALMM_PTR32ParamFields() 
        {
            if (det != null)
            {
                if (det.Id.SRType == InstrType.ALMM) 
                {    // Fill edit panel fields
                    // LMNetComm
                    LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
                    ALMMIPAddressTextBox.Text = l.NetComm.ipaddress.ToString();
                    ALMMPortTextBox.Text = l.NetComm.Port.ToString();

                    ALMMHVTextBox.Text = l.DeviceConfig.HV.ToString();
                    // Make edit panel visible
                    ALMMPanel.Visible = true;
                }
                else if (det.Id.SRType == InstrType.PTR32)
                {
                    PTR32Panel.Visible = true;
					MCANameLabel.Visible = MCAName.Visible = false;
					ConnIdField.Text = string.Copy(det.Id.DetectorId);
					connIdLabel.Text = "PTR-32 instrument identifier: ";
					connLabel.Text = "PTR-32 Connection Parameters";

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

		private void PopulateMCA527ParamFields()
		{
			if (det != null && det.Id.SRType == InstrType.MCA527)
			{
				PTR32Panel.Visible = true;
				MCANameLabel.Visible = MCAName.Visible = true;
				MCAName.Text = string.Copy(det.Id.DetectorId);
				ConnIdField.Text = string.Copy(det.Id.ElectronicsId);
				connIdLabel.Text = "MCA-527 serial number: ";
				connLabel.Text = "MCA-527 Connection Parameters";
				LoadtheMCACombobox();


				LMConnectionInfo lmi = (LMConnectionInfo)(det.Id.FullConnInfo);
				check_HV_set.Checked = lmi.DeviceConfig.LEDs == 2 ? true : false;
				VoltageTimeout.Visible = true;
				VoltageTimeout.Text = lmi.DeviceConfig.HVTimeout.ToString();
				VoltageTolerance.Text = lmi.DeviceConfig.VoltageTolerance.ToString();
				VoltageTolerance.Visible = true;
			}
		}


		//// SELECTOR PANEL ////////////////////////////////////////////////////////////////////////////////////

		private void CancelButt_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpButt_Click(object sender, EventArgs e)
        {

        }

        private void DetectorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditBtn.Enabled = true;
            DeleteBtn.Enabled = true;
        }

		private void EditBtn_Click(object sender, EventArgs e)
		{
			SelectorPanel.Visible = false;
			if (DetectorComboBox.SelectedItem != null)
			{
				det = (Detector)DetectorComboBox.SelectedItem;
				if (det != null)
				{
					if (det.Id.SRType == InstrType.MCA527)
					{
						PopulateMCA527ParamFields();
					} else
					{
						PopulateALMM_PTR32ParamFields();
					}
				}
				Text = oTitle + (" for " + det.Id.DetectorName);
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
            SelectorPanel.Visible = false;
            AddDetectorTypePanel.Visible = true;
        }


   //// ADD A DETECTOR /////////////////////////////////////////////////////////////////////////////////////

        private void AddDetectorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enabled the next button if they've chosen a type and added a name
            if ((AddDetectorNameTextBox.Text != "") && (AddDetectorTypeComboBox.Text != ""))
            {
                AddNextBtn.Enabled = true;
            }
        }

        private void AddDetectorNameTextBox_TextChanged(object sender, EventArgs e)
        {
            // Enabled the next button if they've chosen a type and added a name
            if ((!String.IsNullOrWhiteSpace(AddDetectorNameTextBox.Text)) && (!String.IsNullOrWhiteSpace(AddDetectorTypeComboBox.Text)))
            {
                AddNextBtn.Enabled = true;
            }
        }

        bool AddingNew;
		private void AddNextBtn_Click(object sender, EventArgs e)
		{
			SelectorPanel.Visible = false;
			AddDetectorTypePanel.Visible = false;

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
			if (det != null)
			{
				if (det.Id.SRType == InstrType.MCA527)
				{
					PopulateMCA527ParamFields();
				} else
				{
					PopulateALMM_PTR32ParamFields();
				}
			}
			AddingNew = true;
		}

		private void AddBackBtn_Click(object sender, EventArgs e)
        {
            AddingNew = false;
            AddDetectorTypePanel.Visible = false;
            SelectorPanel.Visible = true;
        }

   //// ALMM PARAMETERS ////////////////////////////////////////////////////////////////////////////////////

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            ALMMPanel.Visible = false;
            SelectorPanel.Visible = true;
        }

        private void AddDetectorNameTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void ALMMOKBtn_Click(object sender, EventArgs e)
        {
            // set the ALMM params on the current ALMM acquire and det def pair
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            l.Port = l.NetComm.Port.ToString();
            
            if (AddingNew)
            {
                NC.App.DB.Detectors.AddOnlyIfNotThere(det);
                RefreshDetectorCombo();
                AddingNew = false;
            }

            NC.App.DB.UpdateDetector(det);
            if (acq.modified)
            {
                NC.App.DB.ReplaceAcquireParams(new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime), acq);
            }
            ALMMPanel.Visible = false;  // like the back button
            SelectorPanel.Visible = true;

            //if (INCCEntry)
                Close();

        }
        bool modified;

        private void ALMMIPAddressTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            IPAddress old = l.NetComm.ipaddress;
            IPAddress IP;
            bool success = IPAddress.TryParse(ALMMIPAddressTextBox.Text, out IP);
            if (success)
                modified |= old.Equals(IP);
            else
            {
                MessageBox.Show(ALMMIPAddressTextBox.Text + " is not a valid IP address. Please enter a valid IP address.", "IP error", MessageBoxButtons.OK);
                ALMMIPAddressTextBox.SelectAll();
            }
        }

        private void ALMMPortTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.Port;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.Port = i;
        }

        
        private void ALMMBufferTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.ReceiveBufferSize;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.ReceiveBufferSize = i;
        }

        private void ALMMLongWaitTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.LongWaitTime;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.LongWaitTime = i;

        }

        private void ALMMCommandWaitTimeTextBox_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.NetComm.CmdWaitTime;
            modified |= Format.ToInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.NetComm.CmdWaitTime = i;

        }

        private void ALMMHV_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.HV;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.HV = i;
        }
        //End ALMM Stuff

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

			if (det.Id.modified && det.Id.SRType == InstrType.MCA527)
			{
				det.Id.modified = !NC.App.DB.UpdateDetectorFields(det); // the only thing that changes here is the detector electronics id, a proxy for the MCA-527 unique id
			}

            if (acq.modified)
            {
                NC.App.DB.ReplaceAcquireParams(new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime), acq);
            }
            PTR32Panel.Visible = false;  // like the back button
            SelectorPanel.Visible = true;

            //if (INCCEntry)
            Close();
        }

        private void PTR32Back_Click(object sender, EventArgs e)
        {
            PTR32Panel.Visible = false;
            SelectorPanel.Visible = true;
        }

        private void PTR32Cancel_Click(object sender, EventArgs e)
        {
            Close();
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
            int disabled = l.DeviceConfig.LEDs;
            if (((CheckBox)sender).Checked == true)
            {
                l.DeviceConfig.LEDs = 2;
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
            }
            modified = disabled != l.DeviceConfig.LEDs;
        }

        private void VoltageTolerance_Leave(object sender, EventArgs e)
        {
            LMConnectionInfo l = (LMConnectionInfo)(det.Id.FullConnInfo);
            int i = l.DeviceConfig.VoltageTolerance;
            modified |= Format.ToNZInt(((TextBox)sender).Text, ref i);
            if (modified)
                l.DeviceConfig.VoltageTolerance = i;
        }

		private void MCAComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (0 == MCAComboBox.Text.IndexOf(MCAPrefix))
			{
				string id = MCAComboBox.Text.Substring(MCAPrefix.Length, 5);
				Console.WriteLine(id);
				if (!string.Equals(id, det.Id.ElectronicsId, StringComparison.OrdinalIgnoreCase))
				{
					det.Id.ElectronicsId = id;
					ConnIdField.Text = string.Copy(det.Id.ElectronicsId);
					det.Id.modified = true;
				}
			}
		}

		void LoadtheMCACombobox()
		{
			Device.MCADeviceInfo[] deviceInfos = null;
			int indexOfMyDesire = -1;
			try
			{
				deviceInfos = Device.MCADevice.QueryDevices();
				MCAComboBox.Items.Clear();
				if (deviceInfos.Length > 0)
				{   
					MCAComboBox.Visible = true;
					// Populate the combobox in the selector panel
					foreach (Device.MCADeviceInfo d in deviceInfos)
					{
						string id =  d.Serial.ToString("D5");
						string s = string.Format("{0}{1}  FW# {2}  HW# {3} on {4}", MCAPrefix, id, d.FirmwareVersion, d.HardwareVersion, d.Address);
						int i = MCAComboBox.Items.Add(s);
						object o = MCAComboBox.Items[i];
						if (string.Equals(id, det.Id.ElectronicsId, StringComparison.OrdinalIgnoreCase))
						{
							indexOfMyDesire = i;
						}
					}
					if (indexOfMyDesire >= 0)
						MCAComboBox.SelectedIndex = indexOfMyDesire;
				} else
					MCAComboBox.Visible = false;
			} catch (Exception e)
			{
				NCCReporter.LMLoggers.LognLM log = NC.App.Loggers.Logger(NCCReporter.LMLoggers.AppSection.Control);
				log.TraceException(e);
			}
		}

		private void ConnIdField_Leave(object sender, EventArgs e)
        {
			if (det.Id.SRType != InstrType.MCA527)
				return;
			// MCA527 detector Electronics Id file can be changed here
			if (!string.Equals(ConnIdField.Text, det.Id.ElectronicsId, StringComparison.OrdinalIgnoreCase))
				det.Id.modified = true;

        }
		const string MCAPrefix ="MCA-527#";


    }
}
