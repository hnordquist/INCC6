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
using LMDAQ;
using NCCReporter;
namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;



    // NEXT: connect this to SR and LM HV params respectively
    public partial class IDDHighVoltagePlateau : Form
    {
        AnalysisDefs.HVCalibrationParameters hvp;
        AnalysisDefs.Detector det;
        AnalysisDefs.AcquireParameters acq;

        public IDDHighVoltagePlateau()
        {
            InitializeComponent();

            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            hvp = Integ.GetCurrentHVCalibrationParams(det);

            this.Text += " for detector " + det.Id.DetectorName;
            MinimumVoltageTextBox.Text = hvp.MinHV.ToString();
            MaximumVoltageTextBox.Text = hvp.MaxHV.ToString();
            VoltageStepSizeTextBox.Text = hvp.Step.ToString();
            CountTimeTextBox.Text = hvp.HVDuration.ToString();
            HVStepDelay.Text = hvp.DelayMS.ToString();

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
                DialogResult = System.Windows.Forms.DialogResult.OK;
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
                DialogResult = System.Windows.Forms.DialogResult.Ignore;

            // The acquire is set to occur, build up the measurement state 
            Integ.BuildMeasurement(acq, det, AssaySelector.MeasurementOption.unspecified);
            
            if (acq.modified)
            {
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, DateTime.Now);
                acq.MeasDateTime = sel.TimeStamp; acq.lm.TimeStamp = sel.TimeStamp;
                NC.App.DB.AcquireParametersMap().Add(sel, acq);  // it's a new one, not the existing one modified
                NC.App.DB.UpdateAcquireParams(acq, det.ListMode);
            }

            UIIntegration.Controller.file = false;  // make sure to use the DAQ controller, not the file controller
            NC.App.AppContext.FileInput = null;  // reset the cmd line file input flag
            if (NC.App.Opstate.Measurement.Detectors[0].ListMode)
            {
                if (NC.App.Opstate.Measurement.Detectors[0].Id.SRType == InstrType.PTR32) {
                    Ptr32Instrument instrument = new Ptr32Instrument(NC.App.Opstate.Measurement.Detectors[0]);
                    instrument.DAQState = DAQInstrState.Offline;
                    instrument.selected = true;
                    instrument.Init(NC.App.Logger(LMLoggers.AppSection.Data), NC.App.Logger(LMLoggers.AppSection.Analysis));

                    if (!Instruments.Active.Contains(instrument)) {
                        Instruments.Active.Add(instrument);
                    }
                }
                else {
                    // if ok, the analyzers are set up, so can kick it off now.
                    LMInstrument lm = new LMInstrument(NC.App.Opstate.Measurement.Detectors[0]);
                    lm.DAQState = DAQInstrState.Offline; // these are manually initiated as opposed to auto-pickup
                    lm.selected = false;  //must broadcast first to get it selected
                    if (!Instruments.All.Contains(lm))
                        Instruments.All.Add(lm); // add to global runtime list
                }
            }
            else
            {
                SRInstrument sri = new SRInstrument(NC.App.Opstate.Measurement.Detectors[0]);
                sri.selected = true;
                sri.Init(null, null);
                if (!Instruments.All.Contains(sri))
                    Instruments.All.Add(sri); // add to global runtime list 
            }

            UIIntegration.Controller.SetHVCalib();  // tell the controller to do an HV operation using the current measurement state
            UIIntegration.Controller.Perform();  // start the HV DAQ thread
            
            
            this.Close();        
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void OpenInExcel_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
