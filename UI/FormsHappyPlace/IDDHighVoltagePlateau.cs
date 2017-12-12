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
using Instr;
using NCCReporter;
namespace UI
{
	using Integ = NCC.IntegrationHelpers;
	using NC = NCC.CentralizedState;

	public partial class IDDHighVoltagePlateau : Form
    {
        HVCalibrationParameters hvp;
        Detector det;
        AcquireParameters acq;

        public IDDHighVoltagePlateau()
        {
            InitializeComponent();

            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            hvp = Integ.GetCurrentHVCalibrationParams(det);

            Text += " for detector " + det.Id.DetectorName;
            MinimumVoltageTextBox.Text = hvp.MinHV.ToString();
            MaximumVoltageTextBox.Text = hvp.MaxHV.ToString();
            VoltageStepSizeTextBox.Text = hvp.Step.ToString();
            CountTimeTextBox.Text = hvp.HVDuration.ToString();
            HVStepDelay.Text = hvp.DelayMS.ToString();
			OpenInExcel.Checked = acq.lm.HVX;
			OpenInExcel.Enabled = DAQ.HVExcel.ExcelPresent();
        }

        private void MinimumVoltageTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int hv = hvp.MinHV;
            if (Format.ToInt32(((TextBox)sender).Text, ref hv, 0, 2000)) { hvp.modified = true; hvp.MinHV = hv; }

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = hvp.MinHV.ToString();
        }

        private void MaximumVoltageTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int hv = hvp.MaxHV;
            if (Format.ToInt32(((TextBox)sender).Text, ref hv, 0, 2100)) { hvp.modified = true; hvp.MaxHV = hv; }

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = hvp.MaxHV.ToString();
        }

        private void VoltageStepSizeTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int st = hvp.Step;
            if (Format.ToInt32(((TextBox)sender).Text, ref st, 1, 1000)) { hvp.modified = true; hvp.Step = st; }

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = hvp.Step.ToString();
        }

        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int d = hvp.HVDuration;
            if (Format.ToInt32(((TextBox)sender).Text, ref d, 0, 3600)) { hvp.modified = true; hvp.HVDuration = d; }

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = hvp.HVDuration.ToString();
        }

        private void HVStepDelay_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int d = hvp.DelayMS;
            if (Format.ToInt32(((TextBox)sender).Text, ref d, 0, 50000)) { hvp.modified = true; hvp.DelayMS = d; }

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = hvp.DelayMS.ToString();
        }       

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (hvp.modified)
            {
                DialogResult = DialogResult.OK;
                HVCalibrationParameters c = NC.App.DB.HVParameters.Get(det.Id.DetectorName);
                if (c != null)
                    c.Copy(hvp);  // copy changes back to original on user affirmation
                else
                {
                    c = hvp;
                    NC.App.DB.HVParameters.GetMap().Add(det, c);
                }
                NC.App.DB.HVParameters.Set(det,c);
                // dev note: LM HV vals on LM Acquire record, but SR uses separate DB table so move HV vals so unify the scheme                 

            }
            else
                DialogResult = DialogResult.Ignore;

			acq.data_src = ConstructedSource.Live;
			if (OpenInExcel.Checked != acq.lm.HVX)
			{
				acq.lm.HVX = OpenInExcel.Checked;
                NC.App.DB.UpdateAcquireParams(acq, det.ListMode);
			}

            // The acquire is set to occur, build up the measurement state 
            Integ.BuildMeasurement(acq, det, AssaySelector.MeasurementOption.unspecified);
			NC.App.Opstate.Measurement.Detector.Id.source =   ConstructedSource.Live;           
            if (acq.modified)
            {
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, DateTime.Now);
                acq.MeasDateTime = sel.TimeStamp; acq.lm.TimeStamp = sel.TimeStamp;
                NC.App.DB.AddAcquireParams(sel, acq);  // it's a new one, not the existing one modified
            }

            UIIntegration.Controller.file = false;  // make sure to use the DAQ controller, not the file controller
            NC.App.AppContext.FileInput = null;  // reset the cmd line file input flag
			Instrument instrument = null;
			if (NC.App.Opstate.Measurement.Detector.ListMode)
			{
				if (NC.App.Opstate.Measurement.Detector.Id.SRType == InstrType.PTR32)
				{
					instrument = new Ptr32Instrument(NC.App.Opstate.Measurement.Detector);
				} 
				else if (NC.App.Opstate.Measurement.Detector.Id.SRType == InstrType.MCA527)
				{
					instrument = new MCA527Instrument(NC.App.Opstate.Measurement.Detector);
					((Analysis.MCA527ProcessingState)instrument.RDT.State).writingFile = false;  // force this
				} 
				else if (NC.App.Opstate.Measurement.Detector.Id.SRType == InstrType.LMMM)
				{
					instrument = new LMInstrument(NC.App.Opstate.Measurement.Detector);
				}
				instrument.DAQState = DAQInstrState.Offline;
				instrument.selected = true;
				instrument.Init(NC.App.DataLogger, NC.App.AnalysisLogger);
				if (!Instruments.Active.Exists(i => instrument.id.Equals(i.id)))
					Instruments.Active.Add(instrument);
			} 
			else
			{
				instrument = new SRInstrument(NC.App.Opstate.Measurement.Detector);
				instrument.selected = true;
				instrument.Init(null, null);
				if (!Instruments.Active.Exists(i => instrument.id.Equals(i)))
					Instruments.All.Add(instrument); // add to global runtime list 
			}

			UIIntegration.Controller.SetHVCalib();  // tell the controller to do an HV operation using the current measurement state
            UIIntegration.Controller.Perform();  // start the HV DAQ thread
            
            
            Close();        
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void OpenInExcel_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
