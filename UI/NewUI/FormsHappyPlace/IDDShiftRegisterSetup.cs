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

    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;    

    public partial class IDDShiftRegisterSetup : Form
    {
        Detector det;
        int[] bauds;
		


		public IDDShiftRegisterSetup(Detector d)
		{
			InitializeComponent();
			bauds = new int[] { 75, 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };

			for (int i = 0; i < bauds.Length; i++) {
				BaudCombo.Items.Add(bauds[i].ToString());
			}
			det = d;

			ShiftRegisterTypeComboBox.Items.Clear();

			/* LM entries */
			if (det.ListMode) 
			{
				foreach (INCCDB.Descriptor dt in NC.App.DB.DetectorTypes.GetLMList()) {
					InstrType dty;
					Enum.TryParse(dt.Name, out dty);
					if (dty.IsListMode())
						ShiftRegisterTypeComboBox.Items.Add(dty.ToString());
				}
				ShiftRegisterTypeComboBox.SelectedItem = det.Id.SRType.ToString();
			} 
			else /* SR entries */
			{
				foreach (INCCDB.Descriptor dt in NC.App.DB.DetectorTypes.GetINCC5SRList()) {
					ShiftRegisterTypeComboBox.Items.Add(dt.Name);
				}
				ShiftRegisterTypeComboBox.SelectedItem = InstrTypeExtensions.INCC5ComboBoxString(det.Id.SRType);
			}

			EditBtn.Visible = false;
			if (det.Id.SRType < InstrType.NPOD) {

				Refre_Click(null, null);
				SetBaudRateSelectorVisibility(det.Id.SRType.IsSRWithVariableBaudRate());
			} 
			else if (det.ListMode) 
			{
				ShiftRegisterSerialPortComboBox.Visible = false;
				ShiftRegisterSerialPortLabel.Visible = false;
				ShiftRegisterSerialPortComboBox.Visible = false;
				ShiftRegisterSerialPortLabel.Visible = false;
				ShiftRegisterSerialPortComboBoxRefresh.Visible = false;
				SetBaudRateSelectorVisibility(false);
				EditBtn.Visible = true;
				EditBtn.Enabled = true;
			}

			SetLMVSRFATypeAndVis();

			InitializeNumericBoxes();

			ShiftRegisterTypeComboBox.SelectedItem = det.Id.SRType.ToString();
			BaudCombo.SelectedIndex = BaudCombo.FindStringExact(d.Id.BaudRate.ToString());
			BaudCombo.Refresh();

			this.Text += (" for " + det.Id.DetectorName);
		}

        void SetBaudRateSelectorVisibility(bool visible)
        {
            BawdyRateLabel.Visible = visible;
            BawdyRateLabel.Enabled = visible;
            BaudCombo.Visible = visible;
            BaudCombo.Enabled = visible;
        }

        private void InitializeNumericBoxes()
        {
            //devnote: Don't know if ranges for all are same.  These taken out of ISR firmware documentation. hn 5.15.2015
            //dev note: For each dialog port, consider using the ranges used in the corresponding INCC5 dialog definition.
            //          The INCC5 dialog ranges were created based on the accumulated knowledge and experience of the INCC5 creators and users.
            //          So, for this dialog, consider reviewing the ranges used in set_sr.cpp for IDD_SHIFT_REGISTER_SETUP, then make changes for the JSR-15.
            PredelayTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            PredelayTextBox.Min = 0;
            PredelayTextBox.Max = 1237.75;
            PredelayTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            PredelayTextBox.NumberFormat = NumericTextBox.Formatter.F2;
            PredelayTextBox.Value = det.SRParams.predelayMS;
            SetPredelayStepConstraint(det.Id.SRType);

            DieAwayTimeTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            DieAwayTimeTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            DieAwayTimeTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            DieAwayTimeTextBox.Value = det.SRParams.dieAwayTimeMS;

            DeadtimeCoefficientATextBox.ToValidate = NumericTextBox.ValidateType.Float;
            DeadtimeCoefficientATextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            DeadtimeCoefficientATextBox.NumberFormat = NumericTextBox.Formatter.F4;
            DeadtimeCoefficientATextBox.Value = det.SRParams.deadTimeCoefficientAinMicroSecs;

            DeadtimeCoefficientBTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            DeadtimeCoefficientBTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            DeadtimeCoefficientBTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            DeadtimeCoefficientBTextBox.Value = det.SRParams.deadTimeCoefficientBinPicoSecs;

            DeadtimeCoefficientCTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            DeadtimeCoefficientCTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            DeadtimeCoefficientCTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            DeadtimeCoefficientCTextBox.Value = det.SRParams.deadTimeCoefficientCinNanoSecs;

            MultiplicityDeadtimeTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            MultiplicityDeadtimeTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            MultiplicityDeadtimeTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            MultiplicityDeadtimeTextBox.Value = det.SRParams.deadTimeCoefficientMultiplicityinNanoSecs;

            DoublesGateFractionTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            DoublesGateFractionTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            DoublesGateFractionTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            DoublesGateFractionTextBox.Value = det.SRParams.doublesGateFraction;

            TriplesGateFractionTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            TriplesGateFractionTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            TriplesGateFractionTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            TriplesGateFractionTextBox.Value = det.SRParams.triplesGateFraction;

            EfficiencyTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            EfficiencyTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            EfficiencyTextBox.NumberFormat = NumericTextBox.Formatter.F4;
            EfficiencyTextBox.Value = det.SRParams.efficiency;
            EfficiencyTextBox.Min = 0;

            HighVoltageTextBox.ToValidate = NumericTextBox.ValidateType.Integer;
            HighVoltageTextBox.NumStyles = System.Globalization.NumberStyles.Integer;
            HighVoltageTextBox.NumberFormat = NumericTextBox.Formatter.NONE;
            HighVoltageTextBox.Steps = 1;
            HighVoltageTextBox.Value = det.SRParams.highVoltage;

            GateLengthTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            GateLengthTextBox.NumStyles = System.Globalization.NumberStyles.AllowDecimalPoint;
            GateLengthTextBox.NumberFormat = NumericTextBox.Formatter.F2;
            GateLengthTextBox.Min = 0.25;
            GateLengthTextBox.Max = 1024.0;
            GateLengthTextBox.Steps = 0.25;
            GateLengthTextBox.Value = det.SRParams.gateLengthMS;
        }

        private void ShiftRegisterTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InstrType t = InstrType.AMSR;
            /* added back, type changes within the SR/LM families are supported */
            if (det.Id.SRType.IsListMode())
                Enum.TryParse((string)ShiftRegisterTypeComboBox.SelectedItem, true, out t);
            else
                t = InstrTypeExtensions.INCC5ComboBoxStringToType((string)ShiftRegisterTypeComboBox.SelectedItem);
            if (det.Id.SRType != t)
                det.Id.modified = true;
            det.Id.SRType = t;

            SecondGateLengthLabel.Visible = (t == InstrType.DGSR);
            SecondGateLengthTextBox.Visible = (t == InstrType.DGSR);
            SetBaudRateSelectorVisibility(t.IsSRWithVariableBaudRate());
			SetPredelayStepConstraint(t);

        }

		private void SetPredelayStepConstraint(InstrType t)
		{
			if (t.IsListMode() || t == InstrType.JSR15)
				PredelayTextBox.Steps = -1; // no check
			else
			{
				PredelayTextBox.Steps = 0.25; // traditional step increment
				if ((PredelayTextBox.Value % 0.25) != 0)
				{
					PredelayTextBox.Value = Bounce25Step(PredelayTextBox.Value);
				}
			}
		}

		/// <summary>
		/// returns value bounced down to the closest 0.25 increment
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		private double Bounce25Step(double x) 
		{
			double tx = Math.Truncate(x);
			double r = x - tx;
			double m = r % 0.25;
			if (m != 0.0)
				return tx + (r - m);
			else
				return x;
		} 

		private void ShiftRegisterSerialPortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            // check for modifications to detector and srparams, do the copy back to the real det.
            bool mod = false;
            

            if (det.Id.SRType < InstrType.NPOD)
            {
                string c = (String)(ShiftRegisterSerialPortComboBox.SelectedItem);
				//"COM PORT nnn (some text)"
                // HA, I crashed this.  I disconnected the JSR-15 with INCC6 running, so COM3 went away and this was fubar
                // need to check for existence. Do we need to do something to check at startup also? hn 5.7.2015
                if (!String.IsNullOrEmpty(c))
                {
                    string[] s = c.Split();
                    if (s.Length > 2)
                    {
                        Int32 p = 0;
                        Int32.TryParse(s[2], out p);
                        if (p != det.Id.SerialPort)
                        {
                            det.Id.SerialPort = p;
                            mod = true;
                        }
                    }
                }
            }


            if (det.Id.SRType.isVirtualAMSR())
            {
                string c = (String)(BaudCombo.SelectedItem);
                Int32 p = 0;
                Int32.TryParse(c, out p);
                if (p != det.Id.BaudRate)
                {
                    det.Id.BaudRate = p;
                    mod = true;
                }
            }


            //Was not storing all predelays and gate lengths because of order op to get a ulong hn 5.7.2015
            double pd = PredelayTextBox.Value * 10;
            double gl = GateLengthTextBox.Value * 10;
            ShiftRegisterParameters sr1 = new ShiftRegisterParameters((ulong)pd,(ulong)gl,HighVoltageTextBox.Value,DieAwayTimeTextBox.Value*10,
                EfficiencyTextBox.Value,DoublesGateFractionTextBox.Value,TriplesGateFractionTextBox.Value, 
                DeadtimeCoefficientATextBox.Value,DeadtimeCoefficientBTextBox.Value, DeadtimeCoefficientCTextBox.Value,MultiplicityDeadtimeTextBox.Value);
            bool modified = sr1.CompareTo(det.SRParams) != 0;

            if (m_bgateTriggerTypeChange)
            {
                AcquireParameters acq = Integ.GetCurrentAcquireParamsFor(det);
                acq.lm.FADefault = m_curGateTriggerType;
                NC.App.DB.UpdateAcquireParams(acq, isLM: true); // update it
            }
            if (det.Id.modified) // type changed only, so gotta save the change in the Detectors table, as well as the sr_parms table
            {
                if (modified || mod)  // also updates the params in one step  /* Update SR Params */                   
                    det.SRParams.CopyValues(sr1);
                NC.App.DB.UpdateDetector(det); // also updates the params in one step
                det.Id.modified = false;
                DialogResult = DialogResult.OK;
            }
            else if (modified || mod)  // only SR params changed
            {
                DialogResult = DialogResult.OK;
                /* Update SR Params */
                det.SRParams.CopyValues(sr1);
                if (NC.App.DB.Detectors.GetItByDetectorId(det.Id.DetectorId) != null)
                    NC.App.DB.UpdateDetectorParams(det); // detector must exist in DB prior to this call, if not presetn, the in-memory representation is saved by the caller later.
            }
            else
                DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {

        }


        private void EditBtn_Click(object sender, EventArgs e)
        {
            AcquireParameters acq = null;
            acq = Integ.GetCurrentAcquireParamsFor(det);

            LMConnectionParams f = new LMConnectionParams(det, acq, false);
            f.StartWithLMDetail();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)  // update performed in OK EH
            {

            }
        }

        private void Refre_Click(object sender, EventArgs e)
        {
            string sel = "COM" + det.Id.SerialPort.ToString();
            int idx = -1;
            int i = 0;
            string[] ports = System.IO.Ports.SerialPort.GetPortNames(); // "COMnnn"
            ShiftRegisterSerialPortComboBox.Items.Clear();
            
            foreach (string p in ports)
            {
                ShiftRegisterSerialPortComboBox.Items.Add(comportwindisplaystring(p));
                if (p.CompareTo(sel) == 0)
                {
                    idx = i;
                }
                i++;
            }
            string cur = comportdisplaystring(det.Id.SerialPort.ToString());
            if (idx >= 0)
                ShiftRegisterSerialPortComboBox.SelectedIndex = idx;
            else // the specified COM port is not available in the current machine state
            {
                i = 0;
                idx = -1;
                foreach (object o in this.ShiftRegisterSerialPortComboBox.Items)
                {
                    if (o.ToString().CompareTo(cur) > 0)
                    {
                        idx = i;
                        ShiftRegisterSerialPortComboBox.Items.Insert(idx, cur + " (unavailable)");
                        ShiftRegisterSerialPortComboBox.SelectedIndex = idx;
                        break;
                    }
                    i++;
                }
            }
        }

        string comportwindisplaystring(string windows)
        {
            return "COM PORT " + windows.Substring(3); // "COMnnn"
        }

        string comportdisplaystring(string number)
        {
            return "COM PORT " + number; // "nnn"
        }

		private void LMFA_CheckedChanged(object sender, EventArgs e)
		{
			m_curGateTriggerType = ((CheckBox)sender).Checked ? FAType.FAOn : FAType.FAOff;
            m_bgateTriggerTypeChange = (m_gateTriggerType != m_curGateTriggerType);
		}

		private void SetLMVSRFATypeAndVis()
		{
			LMFA.Visible = det.ListMode;

			if (!det.ListMode)
				return;

			AcquireParameters acq = Integ.GetCurrentAcquireParamsFor(det);
			m_curGateTriggerType = m_gateTriggerType = acq.lm.FADefault;
			LMFA.Checked = (m_gateTriggerType == FAType.FAOn);
		}

		FAType m_gateTriggerType, m_curGateTriggerType = FAType.FAOff;// This was missing from the database used by LANL. The DB has been updated.
		bool m_bgateTriggerTypeChange;
    }
}
