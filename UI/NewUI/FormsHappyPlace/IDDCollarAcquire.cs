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
using System.Collections.Generic;
using AnalysisDefs;
namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;
    public partial class IDDCollarAcquire : Form
    {
        NormParameters norm = new NormParameters();
        AcquireParameters ap = new AcquireParameters();
        Measurement passive = null;
        Measurement active = null;
        AcquireHandlers ah = null;
        AnalysisMethods am = null;
        INCCAnalysisParams.collar_combined_rec col;
        public IDDCollarAcquire(NormParameters npp)
        {
            InitializeComponent();
            ap = Integ.GetCurrentAcquireParams();
            col = new INCCAnalysisParams.collar_combined_rec();
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.verification;
            Text += " for detector " + ah.det.Id.DetectorName;
            //ableTermControls();
            Detector d= new Detector();
            Integ.GetCurrentAcquireDetectorPair(ref ap, ref d);
            npp.CopyTo(norm);
            Integ.BuildMeasurement(ap, d, AssaySelector.MeasurementOption.verification);
            am = Integ.GetMethodSelections(ah.ap);
            // Main window checks if Collar is defined for material type. No check needed here.
            FillForm();
        }

        private void FillForm()
        {
            AnalysisMethods am = Integ.GetMethodSelections(ap);
            col = new INCCAnalysisParams.collar_combined_rec();
            if (am != null)
            {
                // Grab the settings.
                if (am.HasMethod(AnalysisMethod.Collar))
                    col = (INCCAnalysisParams.collar_combined_rec)am.GetMethodParameters(AnalysisMethod.Collar);
            }

            // Default is to request passive measurement info. Once that is entered or pulled from a measurement, we 
            // can enable the active, then the calculation. HN 5/3/2017
            ModeComboBox.SelectedIndex = (int)col.collar.collar_mode;
            PassiveMeasurementRadioButton.Checked = true;
            PassiveMeasurementRadioButton.Enabled = false;
            ActiveMeasurementRadioButton.Checked = false;
            ActiveMeasurementRadioButton.Enabled = false;
            CalculateResultsRadioButton.Checked = false;
            CalculateResultsRadioButton.Enabled = false;

            //These are all filled based on 1) a live measurement or 2) a stored measurement
            ActiveSinglesTextBox.ReadOnly = true;
            ActiveSinglesTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            ActiveSinglesErrorTextBox.ReadOnly = true;
            ActiveSinglesErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            ActiveDoublesTextBox.ReadOnly = true;
            ActiveDoublesTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            ActiveDoublesErrorTextBox.ReadOnly = true;
            ActiveDoublesErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;

            PassiveSinglesTextBox.ReadOnly = true;
            PassiveSinglesTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            PassiveSinglesErrorTextBox.ReadOnly = true;
            PassiveSinglesErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            PassiveDoublesTextBox.ReadOnly = true;
            PassiveDoublesTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            PassiveDoublesErrorTextBox.ReadOnly = true;
            PassiveDoublesErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            //If they didn't load a passive measurement from the DB, so measure it.

            if (passive == null)
            {
                ItemId Cur = NC.App.DB.ItemIds.Get(ap.item_id);
                Cur.IsoApply(NC.App.DB.Isotopics.Get(ap.isotopics_id));           // apply the iso dates to the item

                NC.App.DB.ItemIds.Set();  // writes any new or modified item ids to the DB
                NC.App.DB.ItemIds.Refresh();    // save and update the in-memory item list 

                if (DialogResult == DialogResult.OK)
                {
                    Visible = false;
                    // Add strata update to measurement object.    HN 9.23.2015              
                    UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                    UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                }
                DialogResult = DialogResult.None;
                passive = NCC.CentralizedState.App.Opstate.Measurement;
            }
            else // We've got the passive data, go ahead and get the active cycles
            {
                Visible = false;
                INCCAnalysisParams.collar_combined_rec ka = (INCCAnalysisParams.collar_combined_rec)am.GetMethodParameters(AnalysisMethod.Collar);
                Integ.BuildMeasurement(ap, Integ.GetCurrentAcquireDetector(), AssaySelector.MeasurementOption.verification);
                Integ.PersistDetectorAndAssociations(Integ.GetCurrentAcquireDetector());
                //active = new Measurement(AssaySelector.MeasurementOption.verification, NC.App.Loggers.DataLogger);
                //active.InitializeResultsSummarizers();
                //active.InitializeContext(true);
                //active.Persist();
                // Add strata update to measurement object.    HN 9.23.2015              
                UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void FromDB_Click(object sender, EventArgs e)
        {
            Detector det = Integ.GetCurrentAcquireDetector();  // todo: test this mod
            List<INCCDB.IndexedResults> mlist = NC.App.DB.IndexedResultsForDetWithItem(det.Id.DetectorId, string.Empty, ap.ItemId.item); // filter on item id and detector first
            IDDMeasurementList measlist = new IDDMeasurementList();
            measlist.Init(mlist,
                        AssaySelector.MeasurementOption.unspecified,
                        goal: IDDMeasurementList.EndGoal.Summary, lmonly: false, inspnum: "All", detector: det);
            if (measlist.bGood)
            {
                DialogResult = measlist.ShowDialog();
                if (DialogResult == DialogResult.OK)
                {
                    passive = measlist.GetSingleSelectedMeas();
            }
            if (passive != null)
            {
                TransferCounts(passive);
                passive.Persist();
            }
            }
            else
                DialogResult = DialogResult.None;
        }

        
		private void TransferCounts (Measurement m)
        {
            m.CalcAvgAndSums();
            MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[Integ.GetCurrentAcquireDetector().MultiplicityParams];

            PassiveSinglesTextBox.Value = mcr.DeadtimeCorrectedSinglesRate.v;
            PassiveSinglesErrorTextBox.Value = mcr.DeadtimeCorrectedSinglesRate.err;
            PassiveDoublesTextBox.Value = mcr.DeadtimeCorrectedDoublesRate.v;
            PassiveDoublesErrorTextBox.Value = mcr.DeadtimeCorrectedDoublesRate.err;

        }
    }
}
