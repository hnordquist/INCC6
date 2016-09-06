/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
namespace NewUI
{

	using Integ = NCC.IntegrationHelpers;
	using NC = NCC.CentralizedState;

	public class AcquireHandlers
    {
        public Detector det;
        public AcquireParameters ap;
        public AssaySelector.MeasurementOption mo;

        public class AcqEventArgs : EventArgs
        {
        }

        public AcquireHandlers()
        {
            Integ.GetCurrentAcquireDetectorPair(ref ap, ref det);
        }

		public AcquireHandlers(AcquireParameters a, Detector dt)
        {
            ap = a; det = dt;
        }

        public void RefreshParams()
        {
            ap = Integ.GetCurrentAcquireParamsFor(det);   
        }

        //// TEXTBOXLEAVE HANDLERS ///////////////////////////////////////////

        public void NumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number 
            ap.modified |= (Format.ToPNZ(((TextBox)sender).Text, ref ap.num_runs));
            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.num_runs);
        }

        public void NumActiveCyclesTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            if (Format.ToPNZ(((TextBox)sender).Text, ref ap.active_num_runs)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.active_num_runs);
        }

        public void CommentTextBox_Leave(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Text) != ap.comment)
            {
                ap.modified = true;
                ap.comment = ((TextBox)sender).Text;
            }
        }

        public void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            double ap_run_count_time = ap.run_count_time;
            if (Format.ToPNZ(((TextBox)sender).Text, ref ap_run_count_time)) ap.modified = true;
            ap.run_count_time = ap_run_count_time;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.run_count_time);
        }

        public void ItemIdTextBox_Leave(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Text) != ap.item_id)
            {
                ap.modified = true;
                ap.item_id = ((TextBox)sender).Text;
            }
        }
        public void ItemIdComboBox_Leave(object sender, EventArgs e)
        {
            if ((((ComboBox)sender).Text) != ap.item_id)
            {
                ap.modified = true;
                ap.item_id = ((ComboBox)sender).Text;
            }
        }
        public void MeasurementPrecisionTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            if (Format.ToPct(((TextBox)sender).Text, ref ap.meas_precision)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = ap.meas_precision.ToString("F2");
        }

        public void MinNumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            if (Format.ToNN(((TextBox)sender).Text, ref ap.min_num_runs)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.min_num_runs);
        }
        
        public void MaxNumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            if (Format.ToPNZ(((TextBox)sender).Text, ref ap.max_num_runs)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.max_num_runs);
        }

        public void DeclaredMassTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a non-zero number            
            if (Format.ToNN(((TextBox)sender).Text, ref ap.mass)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = ap.mass.ToString("F3");
        }
        
        public void DrumEmptyWeightTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            if (Format.ToPNZ(((TextBox)sender).Text, ref ap.drum_empty_weight)) ap.modified = true;

            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = ap.drum_empty_weight.ToString("F3"); ;
        }

        //// CHECKEDCHANGED HANDLERS ///////////////////////////////////////////

		public bool CycleCount { get { return ap.acquire_type == AcquireConvergence.CycleCount; } }
		public bool DoublesPrecision { get { return ap.acquire_type == AcquireConvergence.DoublesPrecision; } }
		public bool TriplesPrecision { get { return ap.acquire_type == AcquireConvergence.TriplesPrecision; } }
		public bool Pu240EffPrecision { get { return ap.acquire_type == AcquireConvergence.Pu240EffPrecision; } }

        public void NumberOfCyclesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
			bool Checked = ((RadioButton)sender).Checked;
			if (CycleCount != Checked)
            {
                ap.modified = true; if (Checked) ap.acquire_type = AcquireConvergence.CycleCount;
            }
        }

        public void DoublesMeasurementPrecisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
			bool Checked = ((RadioButton)sender).Checked;
			if (DoublesPrecision != Checked)
            {
                ap.modified = true; if (Checked) ap.acquire_type = AcquireConvergence.DoublesPrecision;
            }
        }

        public void TriplesMeasurementPrecisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
			bool Checked = ((RadioButton)sender).Checked;
			if (TriplesPrecision != Checked)
            {
                ap.modified = true; if (Checked) ap.acquire_type = AcquireConvergence.TriplesPrecision;
            }
        }

        public void Pu240ePrecisionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
			bool Checked = ((RadioButton)sender).Checked;
			if (Pu240EffPrecision != Checked)
            {
                ap.modified = true; if (Checked) ap.acquire_type = AcquireConvergence.Pu240EffPrecision;
            }
        }

        public void QCTestsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.qc_tests = ((CheckBox)sender).Checked;
        }

        public void PrintResultsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.print = ((CheckBox)sender).Checked;
        }

        public void CommentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.ending_comment = ((CheckBox)sender).Checked;
        }

        //// COMBOBOXSELECTEDINDEXCHANGED HANDLERS //////////////////////////////

        public void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            // Done but keeping the Mouser comment:  SOMETHING with data_src (which is bool!?) Urgency: unknown.  It's like one of Rumsfeld's loved "unknown unknowns."   
            ap.data_src = ConstructedSourceExtensions.SrcToEnum((string)(((ComboBox)sender).SelectedItem));
			if (ap.data_src == ConstructedSource.Unknown) // it was blank or the LM string
				ap.data_src = ConstructedSource.Live;
        }

        public void IOCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.io_code = ((ComboBox)sender).Text;
        }

        public void InvChangeCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.inventory_change_code = ((ComboBox)sender).Text;
        }

        public void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.item_type = ((ComboBox)sender).Text;
        }

        public void StratumIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            string name = (string)((ComboBox)sender).SelectedItem;
            INCCDB.Descriptor d = NC.App.DB.Stratums.Get(name);
            if (d != null)
                ap.stratum_id = d;
        }

        public void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.item_id = ((ComboBox)sender).Text;
        }
        
        public void MBAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.mba = (INCCDB.Descriptor)((ComboBox)sender).SelectedItem;
        }

        public void GloveboxIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ap.modified = true;
            ap.glovebox_id = ((ComboBox)sender).Text;
        }

        //// BUTTONCLICK HANDLERS ///////////////////////////////////////////////

        public DialogResult OKButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Cancel;
            if (ap.modified)
            {
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);
                ap.MeasDateTime = sel.TimeStamp; ap.lm.TimeStamp = sel.TimeStamp;
                NC.App.DB.AddAcquireParams(sel, ap);  // it's a new one, not the existing one modified
            }

            // The acquire is set to occur
            LMAcquire.ResetMeasurement();

            switch (ap.data_src)
            {
                case ConstructedSource.Live:             // set up the instrument list for the action controller
					Integ.BuildMeasurement(ap, det, mo);
                    UIIntegration.Controller.file = false;  // make sure to use the DAQ controller, not the file controller
                    NC.App.AppContext.FileInput = null;  // reset the cmd line file input flag
                    if (det.ListMode)
                    {
						// patch override lm.Interval with run_count_time from dialog
						// the acquire dialogs field values, as seen and modified by the user, override the LM-only acquire settings for virtual SR measurements
                        NC.App.Opstate.Measurement.AcquireState.lm.Interval = NC.App.Opstate.Measurement.AcquireState.run_count_time;
                        NC.App.Opstate.Measurement.AcquireState.lm.Cycles = NC.App.Opstate.Measurement.AcquireState.num_runs;

                        // Check NC.App.Opstate.Measurement.AnalysisParams for at least one VSR 
                        // If not present, inform and pop up the wizard
                        // If present, inform with new dialog, do not pop up the wizard
                        if (NC.App.Opstate.Measurement.AnalysisParams.HasMatchingVSR(det.MultiplicityParams))
                        {
                            dr = DialogResult.OK;
                        }
                        else
                        {
	                        dr = (new LMAcquire(ap, det, fromINCC5Acq: true)).ShowDialog();// analyzers are created in here, placed on global measurement
                            if (dr == DialogResult.OK)
                            {
                                NC.App.DB.UpdateAcquireParams(ap); //update it again
                                NC.App.DB.UpdateDetector(det);
                            }
                        }

                        if (dr == DialogResult.OK)
                        {
                            // if ok, the analyzers are set up, so can kick it off now.
                            UIIntegration.Controller.ActivateDetector(det); 
                        }
                    }
                    else
                    {
                        SRInstrument sri = new SRInstrument(det);
                        sri.selected = true;
                        sri.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
                        if (!Instruments.All.Contains(sri))
                            Instruments.All.Add(sri); // add to global runtime list 
                        dr = DialogResult.OK;
                    }
                    break;
                case ConstructedSource.DB:
					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.DBDataAssay = true;
                    UIIntegration.Controller.file = true;
                    IDDAcquireDBMeas dbdlg = new IDDAcquireDBMeas(this);
                    if (dbdlg.HasItems())
                    {
                        dr = dbdlg.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            DateTimeOffset dto = dbdlg.measurementId.MeasDateTime;
                            DateTimeOffset cur = new DateTimeOffset(dto.Ticks, dto.Offset);
                            NC.App.Logger(NCCReporter.LMLoggers.AppSection.App).TraceEvent(NCCReporter.LogLevels.Info, 87654,
                                    "Using " + dto.ToString("MMM dd yyy HH:mm:ss.ff K"));
							
							NC.App.Opstate.Measurement.MeasDate = dto;
                            // get the cycles for the selected measurement from the database, and add them to the current measurement
                            CycleList cl = NC.App.DB.GetCycles(det, dbdlg.measurementId);
                            foreach(Cycle cycle in cl)  // add the necessary meta-data to the cycle identifier instance
                            {
                                cycle.UpdateDataSourceId(ap.data_src, det.Id.SRType, 
                                                    cur.AddTicks(cycle.TS.Ticks), det.Id.FileName);
                                cur = cycle.DataSourceId.dt;
                            }

                            NC.App.Opstate.Measurement.Add(cl);

							// use the cycle time interval as found in the data, taking the first entry because equal intervals are assumed 
							if (cl.Count > 0)
								NC.App.Opstate.Measurement.AcquireState.lm.Interval = NC.App.Opstate.Measurement.AcquireState.run_count_time
									= cl[0].TS.TotalSeconds;
                            NC.App.DB.UpdateAcquireParams(ap); //update it again
                        }
                    }
                    else
                        MessageBox.Show("No items found in database matching these parameters", "WARNING");
                    break;
                case ConstructedSource.Manual:
                    UIIntegration.Controller.file = true;
                    NC.App.AppContext.DBDataAssay = true;
                    IDDManualDataEntry mdlg = new IDDManualDataEntry();
                    mdlg.AH = this;
                    dr = mdlg.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        // the work is done in the dialog class
                    }
                    break;
                case ConstructedSource.Æther:
                    UIIntegration.Controller.file = true;
                    NC.App.AppContext.DBDataAssay = true;
                    dr = DialogResult.OK;
                    // the measurement re-creation work is done in the IDDReanalysisAssay dialog class                    
                    break;
                case ConstructedSource.CycleFile:
 					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.TestDataFileAssay = true;
                    UIIntegration.Controller.file = true;
                    dr = UIIntegration.GetUsersFile("Select a test data (disk) file", NC.App.AppContext.FileInput, "INCC5 Test data (disk)", "dat", "cnn");
                    break;
                case ConstructedSource.ReviewFile:
					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.ReviewFileAssay = true;
                    UIIntegration.Controller.file = true;
                    dr = UIIntegration.GetUsersFile("Select an NCC file", NC.App.AppContext.FileInput, "INCC5 Review", "NCC");
                    break;
                case ConstructedSource.NCDFile:
					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.NCDFileAssay = true;
                    UIIntegration.Controller.file = true;
                    if (det.ListMode || NC.App.Opstate.Measurement.MeasOption.IsListMode())
                    {
                        dr = (new LMAcquire(ap, det, fromINCC5Acq: true)).ShowDialog();  // show LM-relevant acquire-style settings for modification or confirmation;
                    }
                    else
                    {
                        dr = UIIntegration.GetUsersFilesFolder("Select NCD files or folder", NC.App.AppContext.FileInput, "LMMM NCD", "ncd");
                    }
                    break;
                case ConstructedSource.SortedPulseTextFile:
					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.PulseFileAssay = true;
                    UIIntegration.Controller.file = true;
                    if (det.ListMode || NC.App.Opstate.Measurement.MeasOption.IsListMode())
                    {
                        dr = (new LMAcquire(ap, det, fromINCC5Acq: true)).ShowDialog();  // show LM-relevant acquire-style settings for modification or confirmation;
                    }
                    else
                    {
                        dr = UIIntegration.GetUsersFilesFolder("Select pulse files or folder", NC.App.AppContext.FileInput, "pulse", "txt");
                    }
                    break;
                case ConstructedSource.PTRFile: 
					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.PTRFileAssay = true;
                    UIIntegration.Controller.file = true;
                    if (det.ListMode || NC.App.Opstate.Measurement.MeasOption.IsListMode())
                    {
                        dr = (new LMAcquire(ap, det, fromINCC5Acq: true)).ShowDialog();  // show LM-relevant acquire-style settings for modification or confirmation;
                    }
                    else
                    {
                        dr = UIIntegration.GetUsersFilesFolder("Select PTR-32 files or folder", NC.App.AppContext.FileInput, "PTR-32", "bin", "chn");
                    }
                    break;
                case ConstructedSource.MCA527File:
 					Integ.BuildMeasurement(ap, det, mo);
                    NC.App.AppContext.MCA527FileAssay = true;
                    UIIntegration.Controller.file = true;
				    if (det.ListMode || NC.App.Opstate.Measurement.MeasOption.IsListMode())
                    {
                        dr = (new LMAcquire(ap, det, fromINCC5Acq: true)).ShowDialog();  // show LM-relevant acquire-style settings for modification or confirmation;
                    }
                    else
                    {
	                    dr = UIIntegration.GetUsersFilesFolder("Select MCA files or folder", NC.App.AppContext.FileInput, "MCA527", "mca");
                    }
                    break;
                default:
                    break;
            }
            return dr;
        }
    }
}
