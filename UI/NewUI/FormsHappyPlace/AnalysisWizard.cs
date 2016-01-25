/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
using System.Drawing;
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;
namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;


    public partial class AnalysisWizard : Form
    {

        // State diagram for wizard is as follows:
        //
        //  Step 1:     user selects either to post-process existing data (goto Step 2A) or to process real time data from an attached detector (goto Step 2B)
        //  Step 2A:    user selects a dataset to post-process. If dataset is invalid, throw error dialog and return to Step 2A.  Otherwise, goto Step 3.
        //  Step 2B:    user selects a detector configuration from a list of pre-defined detectors or can launch the detector configuration.  Goto Step 3.
        //  Step 3:     user selects an analysis method from a list, goto Step 4.
        //  Step 4:     user selects analysis parameters appropriate for the previously selected method.  Goto Step 5
        //  Step 5:     user chooses to proceed with analysis as defined (leave wizard and execute) or to add another parallel analysis task (goto Step 6). 
        //  Steps 6+:   duplicates of Steps 3-5, but for additional parallel analysis tasks.

        public enum AWSteps { NullState, Step1, Step2A, Step2B, Step3, Step4, Step5 };

        private AWSteps state = AWSteps.Step1;  // Tracks UI state within wizard steps as follows:
                                                // state == 1:  Step 1
                                                // state == 2:  Step 2A
                                                // state == 3:  Step 2B
                                                // state == 4:  Step 3
                                                // state == 5:  Step 4
                                                // state == 6:  Step 5
                                                // state >  6:  Steps 3-5 repeated but with additional analyses added


        // checkbox row type positional lookup table
        AWAnalysisType[] iixamap = new AWAnalysisType[] {
            AWAnalysisType.MultFast, AWAnalysisType.Mult, AWAnalysisType.CoinMat, AWAnalysisType.Feynman, AWAnalysisType.Event, AWAnalysisType.Rossi, AWAnalysisType.Rates };

        // CheckBox row to actual type as int
        int[] ixamap = new int[] { 0, 2, 4, 3, 1, 0, 5, 6 };

        private enum AWAnalysisType { NullType, CoinMat, Event, Feynman, Mult, MultFast, Rossi, Rates };
        private AWAnalysisType AnalysisType = AWAnalysisType.NullType;   // Type of analysis to perform.
                                                                         // == 0: No type defined yet
                                                                         // == 1: Coincidence matrix channel correlation counting
                                                                         // == 2: Event spacing
                                                                         // == 3: Feynman
                                                                         // == 4: Multiplicity with conventional accidentals
                                                                         // == 5: Multiplicity with fast accidentals
                                                                         // == 6: Rossi-Alpha
                                                                         // == 7: Rate analysis
                                                                         //  > 7: undefined

        private Boolean fromINCCAcquire = false;  // if called directly from INCC acquire, force user to select a multiplicity analyzer

        private int active = 0;         // Used to track which parameter is selected for use in mouserover™ highlighting in the diagrams.

        private string CMCCCDescription = "This is an unfinished description of the Coincidence Matrix analysis technique.";
        private string EventSpacingDescription = "This is an unfinished description of the Event Spacing analysis technique.";
        private string FeynmanDescription = "This is an unfinished description of the Feynman analysis technique.";
        private string MultConvAccDescription = "This is an unfinished description of the Multiplicity with Conventional Accidentals analysis technique.";
        private string MultFastAccDescription = "This is an unfinished description of the Multiplicity with Fast Accidentals analysis technique.";
        private string RossiAlphaDescription = "This is an unfinished description of the Rossi-Alpha analysis technique.";
        private string RatesDescription = "This is an unfinished description of the rates analysis technique.";

        private string CurrentDescription;      // Placeholder for the description string for whatever analysis technique is currently selected.
        private Image CurrentDiagram;           // Placeholder for the analysis method diagram with no highlighting
        private Image CurrentDiagram1;          // Placeholders for the analysis method diagrams with various parameters highlighted
        private Image CurrentDiagram2;
        private Image CurrentDiagram3;
        Detector det;
        AcquireParameters ap;
        CountingAnalysisParameters alt;

        /// <summary>
        /// Clears an existing Measurement if one exists. Forces a GC because a completed Measurement can be quite large
        /// </summary> 
        public static void ResetMeasurement()
        {

            if (NC.App.Opstate.Measurement != null)
            {
                NC.App.Opstate.Measurement = null;
                NCCReporter.LMLoggers.LognLM log = NC.App.Loggers.Logger(NCCReporter.LMLoggers.AppSection.Control);
                long mem = GC.GetTotalMemory(false);
                log.TraceEvent(NCCReporter.LogLevels.Verbose, 4255, "Total GC Memory is {0:N0}Kb", mem / 1024L);
                log.TraceEvent(NCCReporter.LogLevels.Verbose, 4248, "GC now");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                log.TraceEvent(NCCReporter.LogLevels.Verbose, 4284, "GC complete");
                mem = GC.GetTotalMemory(true);
                log.TraceEvent(NCCReporter.LogLevels.Verbose, 4255, "Total GC Memory now {0:N0}Kb", mem / 1024L);
            }
        }

        public AnalysisWizard(AWSteps startHere, AcquireParameters lmap, Detector lmdet)
        {

            det = lmdet;
            ap = lmap;

            if (startHere == AWSteps.Step3)  // fresh List Mode only
            {
                ResetMeasurement();
                Integ.BuildMeasurement(ap, det, AssaySelector.MeasurementOption.unspecified);
            }

            alt = CountingAnalysisParameters.Copy(NC.App.Opstate.Measurement.AnalysisParams);

            InitializeComponent();

            // Stack all the panels up on top of each other; window is set to autosize to whatever is visible.
            this.Step2APanel.Left = 4;
            this.Step2APanel.Top = 6;
            this.Step2BPanel.Left = 4;
            this.Step2BPanel.Top = 6;
            this.Step3Panel.Left = 4;
            this.Step3Panel.Top = 6;
            this.Step4Panel.Left = 4;
            this.Step4Panel.Top = 6;

            state = startHere;
            if (state == AWSteps.Step2A || state == AWSteps.Step2B)
            {
                fromINCCAcquire = true;
                ap.data_src = (state == AWSteps.Step2B ? ConstructedSource.Live : ConstructedSource.PTRFile);  // default file type is PTR-32, updated in UI from acquire param details
            }
            CycleIntervalPatch(); // guard for bad LM cycle and interval
            // Set the visibilities correctly for the initial state 
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (state == AWSteps.Step1)
            {
                this.Step1ControlsPanel.Visible = true;
            }
            else
            {
                this.Step1ControlsPanel.Visible = false;
            }

            if (state == AWSteps.Step2A)
            {
                LoadParams(state);
                this.Step2APanel.Left = 4;
                this.Step2APanel.Top = 6;
                this.Step2APanel.Visible = true;
                if (det != null)
                {
                    Step2ADetCB.SelectedItem = det;
                }
            }
            else
            {
                this.Step2APanel.Visible = false;
            }

            if (state == AWSteps.Step2B)
            {
                LoadParams(state);
                this.Step2BPanel.Visible = true;
                if (det != null)
                {
                    Step2BDetectorComboBox.SelectedItem = det;
                    Step2BNextBtn.Enabled = true;
                }
                else Step2BNextBtn.Enabled = false;
            }
            else
            {
                this.Step2BPanel.Visible = false;
            }

            if (state == AWSteps.Step3)
            {
                if (AnalysisType == AWAnalysisType.NullType)
                    AnalysisType = AWAnalysisType.Feynman;  // default when nothing else known
                this.DescriptionLabel.Text = "Select an analysis method.";
                this.DiagramPanel.BackgroundImage = null;
                this.CMCCCTimingParamsGroupBox.Visible = false;
                this.FeynmanTimingParamsGroupBox.Visible = false;
                this.MultFastAccTimingParamsGroupBox.Visible = false;
                this.DescriptionLabel.Top = 150;
                this.Step3Panel.Visible = true;
                this.LMAnalyzers.Visible = true;
                LoadParams(state);

                this.Step3NextBtn.Enabled = true;   // enable this because there are always valid useful values now

            }
            else
            {
                this.Step3Panel.Visible = false;
            }

            if (state == AWSteps.Step4)
            {
                this.Step4Panel.Visible = true;
                this.Step4SaveAndExit.Enabled = true;
				Comment.Text = ap.comment;
                // Fill in data source and output
                if (ap.data_src.Live())
                {
                    this.Step4DataSourceTextBox.Text = this.Step2BDetectorComboBox.Text;
                    this.Step4OutputDirTextBox.Text = this.Step2BOutputDirectoryTextBox.Text;
                    // Do something like this if you want the detector configuration to come after analysis setup rather than as part of detector definition during Step 2B.
                    // this.Step4NextBtn.Text = "Configure Detector";   
                }
                else
                {
                    // Fill in the input location text box, starting with the file type
                    if (this.Step2NCDRadioBtn.Checked)
                    {
                        this.Step4DataSourceTextBox.Text = "LMMM NCD files in ";
                    }
                    else if (this.Step2SortedPulseRadioBtn.Checked)
                    {
                        this.Step4DataSourceTextBox.Text = "Sorted pulse files in ";
                    }
                    else if (this.Step2PTR32RadioBtn.Checked)
                    {
                        this.Step4DataSourceTextBox.Text = "PTR-32 dual files in ";
                    }
                    else if (this.Step2MCA5272RadioBtn.Checked)
                    {
                        this.Step4DataSourceTextBox.Text = "MCA-527 files in ";
                    }
                    else
                    {
                        this.Step4DataSourceTextBox.Text = "";
                    }

                    // Add the input file directory string
                    this.Step4DataSourceTextBox.Text += this.Step2InputDirectoryTextBox.Text;

                    // Make note of any input file special handling checkboxes
                    if (this.Step2RecurseCheckBox.Checked)
                    {
                        this.Step4DataSourceTextBox.Text += " (recursive)";
                    }

                    // Fill in the output location text box
                    this.Step4OutputDirTextBox.Text = this.Step2OutputDirectoryTextBox.Text;
                }

                // Fill in analysis method and parameters 
                switch (AnalysisType)
                {
                    case AWAnalysisType.CoinMat:
                        this.Step4AnalysisMethodTextBox.Text = "Coincidence Matrix Channel Correlation Counting";
                        this.Step4Param1TextBox.Text = this.CMCCCInitDelayTextBox.Text;
                        this.Step4Param2TextBox.Text = this.CMCCCGateWidthTextBox.Text;
                        this.Step4Param3TextBox.Text = this.CMCCCLongDelayTextBox.Text;
                        this.Step4Param2TextBox.Visible = true;
                        this.Step4Param2Label.Visible = true;
                        this.Step4Param3TextBox.Visible = true;
                        this.Step4Param3Label.Visible = true;
                        this.Step4Param2UnitsLabel.Visible = true;
                        this.Step4Param3UnitsLabel.Visible = true;
                        break;
                    case AWAnalysisType.Event:
                        this.Step4AnalysisMethodTextBox.Text = "Event Spacing Analysis";
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = false;
                        this.Step4Param2Label.Visible = false;
                        this.Step4Param3TextBox.Visible = false;
                        this.Step4Param3Label.Visible = false;
                        this.Step4Param2UnitsLabel.Visible = false;
                        this.Step4Param3UnitsLabel.Visible = false;
                        break;
                    case AWAnalysisType.Feynman:
                        this.Step4AnalysisMethodTextBox.Text = "Feynman Analysis";
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = false;
                        this.Step4Param2Label.Visible = false;
                        this.Step4Param3TextBox.Visible = false;
                        this.Step4Param3Label.Visible = false;
                        this.Step4Param2UnitsLabel.Visible = false;
                        this.Step4Param3UnitsLabel.Visible = false;
                        break;
                    case AWAnalysisType.Mult:
                        this.Step4AnalysisMethodTextBox.Text = "Multiplicity Analysis with Conventional Accidentals";
                        this.Step4Param1TextBox.Text = this.MultConvAccInitDelayTextBox.Text;
                        this.Step4Param2TextBox.Text = this.MultConvAccGateWidthTextBox.Text;
                        this.Step4Param3TextBox.Text = this.MultConvAccClockWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = true;
                        this.Step4Param2Label.Visible = true;
                        this.Step4Param3TextBox.Visible = true;
                        this.Step4Param3Label.Visible = true;
                        this.Step4Param2UnitsLabel.Visible = true;
                        this.Step4Param3UnitsLabel.Visible = true;
                        break;
                    case AWAnalysisType.MultFast:
                        this.Step4AnalysisMethodTextBox.Text = "Multiplicity Analysis with Fast Accidentals";
                        this.Step4Param1TextBox.Text = this.MultFastAccInitDelayTextBox.Text;
                        this.Step4Param2TextBox.Text = this.MultFastAccGateWidthTextBox.Text;
                        this.Step4Param3TextBox.Text = this.MultFastAccClockWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = true;
                        this.Step4Param2Label.Visible = true;
                        this.Step4Param3TextBox.Visible = true;
                        this.Step4Param3Label.Visible = true;
                        this.Step4Param2UnitsLabel.Visible = true;
                        this.Step4Param3UnitsLabel.Visible = true;
                        break;
                    case AWAnalysisType.Rossi:
                        this.Step4AnalysisMethodTextBox.Text = "Rossi-Alpha Analysis";
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = false;
                        this.Step4Param2Label.Visible = false;
                        this.Step4Param3TextBox.Visible = false;
                        this.Step4Param3Label.Visible = false;
                        this.Step4Param2UnitsLabel.Visible = false;
                        this.Step4Param3UnitsLabel.Visible = false;
                        break;
                    case AWAnalysisType.Rates:
                        this.Step4AnalysisMethodTextBox.Text = "Rates Analysis";
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        this.Step4Param2TextBox.Visible = false;
                        this.Step4Param2Label.Visible = false;
                        this.Step4Param3TextBox.Visible = false;
                        this.Step4Param3Label.Visible = false;
                        this.Step4Param2UnitsLabel.Visible = false;
                        this.Step4Param3UnitsLabel.Visible = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.Step4Panel.Visible = false;
            }
        }

        // STATE 1 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Step1PostProcessButton_Click(object sender, EventArgs e)
        {
            state = AWSteps.Step2A;
            ap.data_src = ConstructedSource.NCDFile;  // todo: get from AppContext State
            UpdateUI();
        }

        private void Step1RealTimeButton_Click(object sender, EventArgs e)
        {
            state = AWSteps.Step2B;
            ap.data_src = ConstructedSource.Live;
            UpdateUI();
        }

        private void Step1CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // STATE 2 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void Step2BackBtn_Click(object sender, EventArgs e)
        {
            if (fromINCCAcquire)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else
            {
                state = AWSteps.Step1;
                UpdateUI();
            }
        }

        private void Step2CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Step2ANextBtn_Click(object sender, EventArgs e)
        {
            state = AWSteps.Step3;
            if (fromINCCAcquire)
            {
                Step3DescriptionLabel.Text = "Select a multiplicity analyzer for INCC";
            }
            else
            {
                Step3DescriptionLabel.Text = "Choose a List Mode counting analysis method";
            }
            UpdateUI();
        }

        private void Step2FilenameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Step2ANextBtn.Enabled = true;
        }

        private void Step2FilenameTextBox_Leave(object sender, EventArgs e)
        {
            NC.App.AppContext.FileInput = ((TextBox)(sender)).Text;
        }

        private void Step2BrowseBtn_Click(object sender, EventArgs e)
        {
            string s = UIIntegration.GetUsersFolder("Select Input Folder", NC.App.AppContext.FileInput);
            if (!String.IsNullOrEmpty(s))
            {
                NC.App.AppContext.FileInput = s;
                NC.App.AppContext.modified = true;
                Step2InputDirectoryTextBox.Text = s;
            }
            else
                Step2InputDirectoryTextBox.Text = NC.App.AppContext.FileInput;
        }

        private void FilePicker2_Click(object sender, EventArgs e)
        {
            string s = UIIntegration.GetUsersFolder("Select Output Folder", ap.lm.Results);
            if (!String.IsNullOrEmpty(s) && !String.Equals(ap.lm.Results, s))
            {
                ap.lm.Results = s;
                Step2OutputDirectoryTextBox.Text = s;
                ap.modified = true;
            }
            else
                Step2OutputDirectoryTextBox.Text = ap.lm.Results;

        }

        // STATE 3 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Step2BDetectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Step2BNextBtn.Enabled = true;
            det = (Detector)((ComboBox)sender).SelectedItem;
        }
        private void Step2ADetectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            det = (Detector)((ComboBox)sender).SelectedItem;
        }

        private void Step2BBackBtn_Click(object sender, EventArgs e)
        {
            if (fromINCCAcquire)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else
            {
                state = AWSteps.Step1;
                this.Step2BNextBtn.Enabled = false;
                UpdateUI();
            }
        }

        private void Step2BCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Step2BNextBtn_Click(object sender, EventArgs e)
        {
            state = AWSteps.Step3;
            if (fromINCCAcquire)
            {
                Step3DescriptionLabel.Text = "Select a multiplicity analyzer for INCC";
                // todo: disable the non-SR rows
            }
            else
            {
                Step3DescriptionLabel.Text = "Choose a List Mode counting analysis method";
            }
            UpdateUI();
        }


        private void FilePicker5_Click(object sender, EventArgs e)
        {
            string s = UIIntegration.GetUsersFolder("Select Output Folder", ap.lm.Results);
            if (!String.IsNullOrEmpty(s) && !String.Equals(ap.lm.Results, s))
            {
                ap.lm.Results = s;
                Step2BOutputDirectoryTextBox.Text = s;
                ap.modified = true;
            }
            else
                Step2BOutputDirectoryTextBox.Text = ap.lm.Results;
        }

        // STATE 4 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void CoinMatSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = CMCCCDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.coincidence_matrix;
            CurrentDiagram1 = NewUI.Properties.Resources.coincidence_matrix_initdelay;
            CurrentDiagram2 = NewUI.Properties.Resources.coincidence_matrix_gatewidth;
            CurrentDiagram3 = NewUI.Properties.Resources.coincidence_matrix_longdelay;
            this.DiagramPanel.BackgroundImage = CurrentDiagram;
            this.CMCCCTimingParamsGroupBox.Visible = true;
            this.FeynmanTimingParamsGroupBox.Visible = false;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.DescriptionLabel.Top = 286;

            //fill in current SR Params from the local copy
            Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
            CMCCCInitDelayTextBox.Text = co.SR.predelay.ToString();
            CMCCCGateWidthTextBox.Text = co.gateWidthTics.ToString();
            CMCCCLongDelayTextBox.Text = co.AccidentalsGateDelayInTics.ToString();

        }

        void EventSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = EventSpacingDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.feynman;
            CurrentDiagram1 = NewUI.Properties.Resources.feynman_gatewidth;
            this.DiagramPanel.BackgroundImage = CurrentDiagram;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = true;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.DescriptionLabel.Top = 286;
            this.FeynmanTimingParamsGroupBox.Top = 184;

            //fill in current from the local copy
            TimeInterval f = alt.GetFirstTimeInterval(activeOnly: false, addIfNotPresent: true);
            SharedGateWidthTextBox.Text = f.gateWidthTics.ToString();
        }

        void FeynSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = FeynmanDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.feynman;
            CurrentDiagram1 = NewUI.Properties.Resources.feynman_gatewidth;
            this.DiagramPanel.BackgroundImage = NewUI.Properties.Resources.feynman;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = true;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Top = 184;
            this.DescriptionLabel.Top = 286;
            Feynman f = alt.GetFirstFeynman(activeOnly: false, addIfNotPresent: true);
            SharedGateWidthTextBox.Text = f.gateWidthTics.ToString();
        }

        void MultiplicityConvSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = MultConvAccDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.coincidence_matrix;
            CurrentDiagram1 = NewUI.Properties.Resources.coincidence_matrix_initdelay;
            CurrentDiagram2 = NewUI.Properties.Resources.coincidence_matrix_gatewidth;
            CurrentDiagram3 = NewUI.Properties.Resources.coincidence_matrix_longdelay;
            this.DiagramPanel.BackgroundImage = NewUI.Properties.Resources.coincidence_matrix;
            this.MultConvAccTimingParamsGroupBox.Visible = true;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = false;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultFastAccTimingParamsGroupBox.Top = 350;
            this.DescriptionLabel.Top = 450;
            this.DiagramPanel.Height = 334;

            Multiplicity m0 = GetAMergedMult(FAType.FAOff);
            MultConvAccInitDelayTextBox.Text = m0.SR.predelay.ToString();
            MultConvAccGateWidthTextBox.Text = m0.gateWidthTics.ToString();
            MultConvAccClockWidthTextBox.Text = m0.AccidentalsGateDelayInTics.ToString();
        }

        void MultiplicityFastSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = MultFastAccDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.multiplicity_FA;
            CurrentDiagram1 = NewUI.Properties.Resources.multiplicity_FA_initdelay;
            CurrentDiagram2 = NewUI.Properties.Resources.multiplicity_FA_gatewidth;
            CurrentDiagram3 = NewUI.Properties.Resources.multiplicity_FA_clockwidth;
            this.DiagramPanel.BackgroundImage = NewUI.Properties.Resources.multiplicity_FA;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = false;
            this.MultFastAccTimingParamsGroupBox.Visible = true;
            this.MultFastAccTimingParamsGroupBox.Top = 350;
            this.DescriptionLabel.Top = 450;
            this.DiagramPanel.Height = 334;

            Multiplicity m = GetAMergedMult(FAType.FAOn);
            MultFastAccInitDelayTextBox.Text = m.SR.predelay.ToString();
            MultFastAccGateWidthTextBox.Text = m.gateWidthTics.ToString();
            MultFastAccClockWidthTextBox.Text = m.BackgroundGateTimeStepInTics.ToString();

        }


        void RossiSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = RossiAlphaDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            CurrentDiagram = NewUI.Properties.Resources.feynman;
            CurrentDiagram1 = NewUI.Properties.Resources.feynman_gatewidth;
            this.DiagramPanel.BackgroundImage = NewUI.Properties.Resources.feynman;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = true;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Top = 184;
            this.DescriptionLabel.Top = 286;
            Rossi f = alt.GetFirstRossi(activeOnly: false, addIfNotPresent: true);
            SharedGateWidthTextBox.Text = f.gateWidthTics.ToString();
        }

        void RatesSelect()
        {
            this.Step3NextBtn.Enabled = true;
            active = 0;
            CurrentDescription = RatesDescription;
            this.DescriptionLabel.Text = CurrentDescription;
            this.DiagramPanel.BackgroundImage = null;
            CurrentDiagram = null;
            CurrentDiagram1 = null;
            this.CMCCCTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Visible = true;
            this.MultFastAccTimingParamsGroupBox.Visible = false;
            this.MultConvAccTimingParamsGroupBox.Visible = false;
            this.FeynmanTimingParamsGroupBox.Top = 5;
            this.DescriptionLabel.Top = 100;
            BaseRate b = alt.GetFirstRate(activeOnly: false, addIfNotPresent: true);
            SharedGateWidthTextBox.Text = b.gateWidthTics.ToString();
        }

        private void Step3BackBtn_Click(object sender, EventArgs e)
        {
            active = 0;
            if (ap.data_src.Live())
            {
                state = AWSteps.Step2B;
            }
            else
            {
                state = AWSteps.Step2A;
            }
            UpdateUI();
        }

        private void Step3NextBtn_Click(object sender, EventArgs e)
        {
            state = AWSteps.Step4;
            active = 0;
            UpdateUI();
        }

        private void Step3CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ///////////////////////////////////////////


        private void CMCCCLongDelayTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram3;
                this.DescriptionLabel.Text = "The long delay is the time, measured in ticks (10^-7 seconds), between the end of the R+A Gate and the beginning of the A Gate.";
            }
        }

        private void CMCCCLongDelayTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        private void CMCCCLongDelayTextBox_Enter(object sender, EventArgs e)
        {
            active = 3;
            this.DiagramPanel.BackgroundImage = CurrentDiagram3;
            this.DescriptionLabel.Text = "The long delay is the time, measured in ticks (10^-7 seconds), between the end of the R+A Gate and the beginning of the A Gate.";
        }

        private void CMCCCLongDelayTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 3)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.AccidentalsGateDelayInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            co.AccidentalsGateDelayInTics = x; co.modified = true;
                        }
                        ((TextBox)sender).Text = co.AccidentalsGateDelayInTics.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.AccidentalsGateDelayInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.AccidentalsGateDelayInTics = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.AccidentalsGateDelayInTics.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.AccidentalsGateDelayInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.AccidentalsGateDelayInTics = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.AccidentalsGateDelayInTics.ToString();
                    }
                    break;
            }
        }

        ///////////////////////////////////////////


        private void CMCCCGateWidthTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram2;
                this.DescriptionLabel.Text = "The gate width is the amount of time, measured in ticks (10^-7 seconds), the gates are open and counting.";
            }
        }

        private void CMCCCGateWidthTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        private void CMCCCGateWidthTextBox_Enter(object sender, EventArgs e)
        {
            active = 2;
            this.DiagramPanel.BackgroundImage = CurrentDiagram2;
            this.DescriptionLabel.Text = "The gate width is the amount of time, measured in ticks (10^-7 seconds), the gates are open and counting.";
        }

        private void CMCCCGateWidthTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 2)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            co.gateWidthTics = x; co.SR.gateLength = x; co.modified = true;
                        }
                        ((TextBox)sender).Text = co.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.gateWidthTics = x; m.SR.gateLength = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.gateWidthTics = x; m.SR.gateLength = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.gateWidthTics.ToString();
                    }
                    break;
            }

        }

        ///////////////////////////////////////////

        private void CMCCCInitDelayTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 1)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								co.SR.predelay = x; co.modified = true;
                        }
                        ((TextBox)sender).Text = co.SR.predelay.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								m.SR.predelay = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.SR.predelay.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								m.SR.predelay = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.SR.predelay.ToString();
                    }
                    break;
            }
        }

        private void CMCCCInitDelayTextBox_Enter(object sender, EventArgs e)
        {
            active = 1;
            this.DiagramPanel.BackgroundImage = CurrentDiagram1;
            this.DescriptionLabel.Text = "The predelay is the length of time, measured in ticks (10^-7 seconds), between the detection of the first neutron at t0 and the beginning of the R+A Gate.";
        }

        private void CMCCCInitDelayTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram1;
                this.DescriptionLabel.Text = "The predelay is the length of time, measured in ticks (10^-7 seconds), between the detection of the first neutron at t0 and the beginning of the R+A Gate.";
            }
        }

        private void CMCCCInitDelayTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        ///////////////////////////////////////////


        private void SharedGateWidthTextBox_Enter(object sender, EventArgs e)
        {
            active = 1;
            this.DiagramPanel.BackgroundImage = CurrentDiagram1;
            this.DescriptionLabel.Text = "The gate width is the length of time, measured in ticks (10^-7 seconds), taken up by each discrete time bin.";
        }

        private void SharedGateWidthTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 1)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 640;

            switch (AnalysisType)
            {
                case AWAnalysisType.Event:
                    {
                        TimeInterval ti = alt.GetFirstTimeInterval(activeOnly: false, addIfNotPresent: true);
                        x = ti.gateWidthTics;
                        bool mod = Format.ToNN(((TextBox)sender).Text, ref x);
                        if (mod)
                        {
                            ti.gateWidthTics = x; ti.modified = true;
                        }
                        ((TextBox)sender).Text = ti.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.Feynman:
                    {
                        Feynman f = alt.GetFirstFeynman(activeOnly: false, addIfNotPresent: true);
                        x = f.gateWidthTics;
                        bool mod = Format.ToNN(((TextBox)sender).Text, ref x);
                        if (mod)
                        {
                            f.gateWidthTics = x; f.modified = true;
                        }
                        ((TextBox)sender).Text = f.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.Rossi:
                    {
                        Rossi f = alt.GetFirstRossi(activeOnly: false, addIfNotPresent: true);
                        x = f.gateWidthTics;
                        bool mod = Format.ToNN(((TextBox)sender).Text, ref x);
                        if (mod)
                        {
                            f.gateWidthTics = x; f.modified = true;
                        }
                        ((TextBox)sender).Text = f.gateWidthTics.ToString();
                    }
                    break;
            }
        }

        private void SharedGateWidthTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram1;
                this.DescriptionLabel.Text = "The gate width is the length of time, measured in ticks (10^-7 seconds), taken up by a discrete time bin.";
            }
        }

        private void SharedGateWidthTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        ///////////////////////////////////////////

        private void MultFastAccInitDelayTextBox_Enter(object sender, EventArgs e)
        {
            active = 1;
            this.DiagramPanel.BackgroundImage = CurrentDiagram1;
            this.DescriptionLabel.Text = "The predelay is the length of time, measured in ticks (10^-7 seconds), between the detection of the first neutron at t0 and the beginning of the R+A Gate.";
        }

        private void MultFastAccInitDelayTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 1)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								co.SR.predelay = x; co.modified = true;
						}
                        ((TextBox)sender).Text = co.SR.predelay.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								m.SR.predelay = x; m.modified = true;
						}
                        ((TextBox)sender).Text = m.SR.predelay.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.SR.predelay;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
								m.SR.predelay = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.SR.predelay.ToString();
                    }
                    break;
            }
        }

        private void MultFastAccInitDelayTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram1;
                this.DescriptionLabel.Text = "The predelay is the length of time, measured in ticks (10^-7 seconds), between the detection of the first neutron at t0 and the beginning of the R+A Gate.";
            }
        }

        private void MultFastAccInitDelayTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        ///////////////////////////////////////////


        private void MultFastAccGateWidthTextBox_Enter(object sender, EventArgs e)
        {
            active = 2;
            this.DiagramPanel.BackgroundImage = CurrentDiagram2;
            this.DescriptionLabel.Text = "The gate width is the amount of time, measured in ticks (10^-7 seconds), during which either the gates are open and counting.";
        }

        private void MultFastAccGateWidthTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 1)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            co.gateWidthTics = x; co.SR.gateLength = x; co.modified = true;
                        }
                        ((TextBox)sender).Text = co.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.gateWidthTics = x; m.SR.gateLength = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.gateWidthTics.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.gateWidthTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.gateWidthTics = x; m.SR.gateLength = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.gateWidthTics.ToString();
                    }
                    break;
            }
        }

        private void MultFastAccGateWidthTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram2;
                this.DescriptionLabel.Text = "The gate width is the amount of time, measured in ticks (10^-7 seconds), during which either the gates are open and counting.";
            }
        }

        private void MultFastAccGateWidthTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        ///////////////////////////////////////////


        private void MultFastAccClockWidthTextBox_Enter(object sender, EventArgs e)
        {
            active = 3;
            this.DiagramPanel.BackgroundImage = CurrentDiagram3;
            this.DescriptionLabel.Text = "The clock width is the amount of time, measured in ticks (10^-7 seconds), between subsequent fast accidental gates.";
        }

        private void MultFastAccClockWidthTextBox_Leave(object sender, EventArgs e)
        {
            if (active == 1)
            {
                active = 0;
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
            ulong x = 0;
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    {
                        Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        x = co.BackgroundGateTimeStepInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            co.BackgroundGateTimeStepInTics = x; co.modified = true;
                        }
                        ((TextBox)sender).Text = co.BackgroundGateTimeStepInTics.ToString();
                    }
                    break;
                case AWAnalysisType.Mult:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOff);
                        x = m.BackgroundGateTimeStepInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.BackgroundGateTimeStepInTics = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.BackgroundGateTimeStepInTics.ToString();
                    }
                    break;
                case AWAnalysisType.MultFast:
                    {
                        Multiplicity m = GetAMergedMult(FAType.FAOn);
                        x = m.BackgroundGateTimeStepInTics;
                        bool mod = (Format.ToNN(((TextBox)sender).Text, ref x));
                        if (mod)
                        {
                            m.BackgroundGateTimeStepInTics = x; m.modified = true;
                        }
                        ((TextBox)sender).Text = m.BackgroundGateTimeStepInTics.ToString();
                    }
                    break;
            }
        }

        private void MultFastAccClockWidthTextBox_MouseEnter(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram3;
                this.DescriptionLabel.Text = "The clock width is the amount of time, measured in ticks (10^-7 seconds), between subsequent fast accidental gates.";
            }
        }

        private void MultFastAccClockWidthTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (active == 0)
            {
                this.DiagramPanel.BackgroundImage = CurrentDiagram;
                this.DescriptionLabel.Text = CurrentDescription;
            }
        }

        // STATE 5 /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PreserveNewState()
        {
            // 1: create the analysis param objects
            // 2: associate the analysis params with the current detector/instrument definition (part of the contextual measurement state)

            // devnote: permitting only one per run FTTB, so clear the list!
            // NEXT: Need to support multiple analyzers of each type from the UI
            CountingAnalysisParameters cntap = new CountingAnalysisParameters();
            foreach (string s in LMAnalyzers.Items)
            {
                AWAnalysisType at = CheckBoxTextMap(s);
                bool chosenForUse = LMAnalyzers.GetItemChecked(ixamap[(int)at]);
                switch (at)
                {
                    case AWAnalysisType.Rossi:
                        AnalysisDefs.Rossi r = alt.GetFirstRossi(activeOnly: false, addIfNotPresent: true);
                        r.Active = chosenForUse;
                        cntap.Add(r);
                        break;
                    case AWAnalysisType.Rates:
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        AnalysisDefs.BaseRate b = alt.GetFirstRate(activeOnly: false, addIfNotPresent: true);
                        b.Active = chosenForUse;
                        cntap.Add(b);
                        break;
                    case AWAnalysisType.Event:
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        AnalysisDefs.TimeInterval ti = alt.GetFirstTimeInterval(activeOnly: false, addIfNotPresent: true);
                        ti.Active = chosenForUse;
                        cntap.Add(ti);
                        break;
                    case AWAnalysisType.Feynman:
                        this.Step4Param1TextBox.Text = this.SharedGateWidthTextBox.Text;
                        AnalysisDefs.Feynman f = alt.GetFirstFeynman(activeOnly: false, addIfNotPresent: true);
                        f.Active = chosenForUse;
                        cntap.Add(f);
                        break;
                    case AWAnalysisType.CoinMat:  // like mult infra
                        AnalysisDefs.Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        co.Active = chosenForUse;
                        // NEXT: CoinMat is incomplete and untested
                        // todo: these steps might occur during analysis processing too, check
                        //MultiplicityCountingRes mcr1 = new MultiplicityCountingRes(co.FA, 0);
                        //INCCCycleConditioning.calc_alpha_beta(co, mcr1);
                        //det.AB.TransferIntermediates(mcr1.AB);
                        // todo: the detector  must get mFA  predelay and gatewidth assigned back to it after the construction steps here
                        cntap.Add(co);

                        break;
                    case AWAnalysisType.Mult:
                        this.Step4Param1TextBox.Text = this.MultConvAccInitDelayTextBox.Text;
                        this.Step4Param2TextBox.Text = this.MultConvAccGateWidthTextBox.Text;
                        this.Step4Param3TextBox.Text = this.MultConvAccClockWidthTextBox.Text;
                        AnalysisDefs.Multiplicity mFAOff = new AnalysisDefs.Multiplicity(AnalysisDefs.FAType.FAOff);
                        mFAOff.SR = new ShiftRegisterParameters(det.SRParams);
                        ulong u;
                        if (UInt64.TryParse(this.MultConvAccClockWidthTextBox.Text, out u))  // ok
                            mFAOff.AccidentalsGateDelayInTics = u;
                        if (UInt64.TryParse(this.MultConvAccInitDelayTextBox.Text, out u)) // on SR
                            mFAOff.SR.predelay = u;
                        if (UInt64.TryParse(this.MultConvAccGateWidthTextBox.Text, out u)) // from SR def 
                            mFAOff.SetGateWidthTics(u);
                        mFAOff.Active = chosenForUse;
                        cntap.Add(mFAOff);
                        break;
                    case AWAnalysisType.MultFast:
                        this.Step4Param1TextBox.Text = this.MultFastAccInitDelayTextBox.Text;
                        this.Step4Param2TextBox.Text = this.MultFastAccGateWidthTextBox.Text;
                        this.Step4Param3TextBox.Text = this.MultFastAccClockWidthTextBox.Text;
                        AnalysisDefs.Multiplicity mFA = new AnalysisDefs.Multiplicity(AnalysisDefs.FAType.FAOn);
                        mFA.SR = new ShiftRegisterParameters(det.SRParams);
                        ulong v;
                        if (UInt64.TryParse(this.MultFastAccInitDelayTextBox.Text, out v))  // ok
                            mFA.SR.predelay = v;
                        if (UInt64.TryParse(this.MultFastAccClockWidthTextBox.Text, out v))  // ok
                            mFA.BackgroundGateTimeStepInTics = v;
                        if (UInt64.TryParse(this.MultFastAccGateWidthTextBox.Text, out v))  // ok 
                            mFA.SetGateWidthTics(v);
                        mFA.Active = chosenForUse;
                        cntap.Add(mFA);
                        break;

                    default:
                        break;
                }
            }
            // at last, assign the analyzer(s) to the global measurement structure
            NC.App.Opstate.Measurement.AnalysisParams = cntap;
            NC.App.LMBD.UpdateCounters(det, cntap);
            NC.App.DB.UpdateDetectorParams(det);  //  gw and predelay values!
            if (NC.App.AppContext.modified)
                NC.App.LMBD.UpdateLMINCCAppContext();
            Integ.ApplyVSRChangesToDefaultDetector(NC.App.Opstate.Measurement);
        }

        private void Step4BackBtn_Click(object sender, EventArgs e)
        {
            active = 0;
            state = AWSteps.Step3;
            UpdateUI();
        }

        /// <summary>
        /// Build analyzers, update db with state, ready to run
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Step4NextBtn_Click(object sender, EventArgs e)
        {
            // 1: create the analysis param objects
            // 2: associate the analysis params with the current detector/instrument definition (part of the contextual measurement state)
            // 3: assuming the measurement state is ready, start the live performance or the file-based IO

            PreserveNewState();
            ///
            /// Because the FAOn, FAOff or coin settings may have changed, the CountingAnalysisResults and INCCMethodResults maps must be reconstructed,
            ///
            NC.App.Opstate.Measurement.InitializeResultsSummarizers();
            NC.App.Opstate.Measurement.INCCAnalysisState.ClearINCCAnalysisResults();
            NC.App.Opstate.Measurement.PrepareINCCResults();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Step4SaveAndExit_Click(object sender, EventArgs e)
        {
            PreserveNewState();
            if (ap.modified || ap.lm.modified)
            {
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);
                ap.MeasDateTime = sel.TimeStamp; ap.lm.TimeStamp = sel.TimeStamp;
                NC.App.DB.AcquireParametersMap().Add(sel, ap);  // it's a new one, not the existing one modified
                NC.App.DB.UpdateAcquireParams(ap, det.ListMode);
            }
            this.Close();
        }
        private void Step4CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void SetCheckListBoxAnalyzers()
        {
            AWAnalysisType aw = AWAnalysisType.Rates;
            foreach (SpecificCountingAnalyzerParams s in alt)  // alt has the original settings with recent local modifications until the final OK replaces the database and in-memory copies on the measurement 
            {
                Type t = s.GetType();
                if (t.Equals(typeof(AnalysisDefs.Rossi)))
                    aw = AWAnalysisType.Rossi;
                else if (t.Equals(typeof(AnalysisDefs.TimeInterval)))
                    aw = AWAnalysisType.Event;
                else if (t.Equals(typeof(AnalysisDefs.Feynman)))
                    aw = AWAnalysisType.Feynman;
                else if (t.Equals(typeof(AnalysisDefs.Coincidence)))
                    aw = AWAnalysisType.CoinMat;
                else if (t.Equals(typeof(AnalysisDefs.Multiplicity)))
                {
                    if (((AnalysisDefs.Multiplicity)s).FA == FAType.FAOn)
                        aw = AWAnalysisType.MultFast;
                    else
                        aw = AWAnalysisType.Mult;
                }
                else if (t.Equals(typeof(AnalysisDefs.BaseRate)))
                    aw = AWAnalysisType.Rates;

                if (s.Active) LMAnalyzers.SetItemChecked(ixamap[(int)aw], true);  // set the typed row item as checked
            }
        }

        void ShowCurrentAnalzyerSelection()
        {
            switch (AnalysisType)
            {
                case AWAnalysisType.CoinMat:
                    CoinMatSelect();
                    break;
                case AWAnalysisType.Event:
                    EventSelect();
                    break;
                case AWAnalysisType.Feynman:
                    FeynSelect();
                    break;
                case AWAnalysisType.Mult:
                    MultiplicityConvSelect();
                    break;
                case AWAnalysisType.MultFast:
                    MultiplicityFastSelect();
                    break;
                case AWAnalysisType.Rates:
                    RatesSelect();
                    break;
                case AWAnalysisType.Rossi:
                    RossiSelect();
                    break;
            }
        }

        /// <summary>
        /// populates dialog fields from current state, can be moved out of here later
        /// </summary>
        void LoadParams(AnalysisWizard.AWSteps step)
        {
            switch (step)
            {
                case AnalysisWizard.AWSteps.Step1:
                    break;
                case AnalysisWizard.AWSteps.Step2A:
                    Step2InputDirectoryTextBox.Text = NC.App.AppContext.FileInput;
                    Step2RecurseCheckBox.CheckState = (NC.App.AppContext.Recurse ? CheckState.Checked : CheckState.Unchecked);
                    Step2AutoOpenCheckBox.CheckState = (NC.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2IncludeConfigCheckBox.CheckState = (ap.lm.IncludeConfig ? CheckState.Checked : CheckState.Unchecked);
                    Step2SaveEarlyTermCheckBox.CheckState = (ap.lm.SaveOnTerminate ? CheckState.Checked : CheckState.Unchecked);

                    // el radio buton setter
                    if (NC.App.AppContext.PTRFileAssay)
                    {
                        Step2PTR32RadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.PTRFile;
                    }
                    else if (NC.App.AppContext.PulseFileAssay)
                    {
                        Step2SortedPulseRadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.SortedPulseTextFile;
                    }
                    else if (NC.App.AppContext.MCA527FileAssay)
                    {
                        Step2MCA5272RadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.MCA527File;
                    }
                    else // always the default
                    {
                        Step2NCDRadioBtn.Checked = true;
                    }

                    Step2OutputDirectoryTextBox.Text = ap.lm.Results;
                    RefreshDetectorCombo(Step2ADetCB);
                    break;
                case AnalysisWizard.AWSteps.Step2B:
                    Step2BAutoOpenCheckBox.CheckState = (NC.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2BIncludeConfigCheckBox.CheckState = (ap.lm.IncludeConfig ? CheckState.Checked : CheckState.Unchecked);
                    Step2BSaveEarlyTermCheckBox.CheckState = (ap.lm.SaveOnTerminate ? CheckState.Checked : CheckState.Unchecked);
                    if (fromINCCAcquire)
                        IntervalTextBox.Text = ap.run_count_time.ToString();
                    else
                        IntervalTextBox.Text = ap.lm.Interval.ToString();
                    Step2BOutputDirectoryTextBox.Text = ap.lm.Results;
                    if (fromINCCAcquire)
                        CycleNumTextBox.Text = ap.num_runs.ToString();
                    else
                        CycleNumTextBox.Text = ap.lm.Cycles.ToString();
                    Step2BAutoOpenCheckBox.CheckState = (NC.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2BWriteDataFiles.CheckState = (NC.App.AppContext.LiveFileWrite ? CheckState.Checked : CheckState.Unchecked);

                    RefreshDetectorCombo(Step2BDetectorComboBox);
                    break;
                case AnalysisWizard.AWSteps.Step3:
					if (string.IsNullOrEmpty(Step2InputDirectoryTextBox.Text))
						Step2InputDirectoryTextBox.Text = NC.App.AppContext.FileInput;
					if (string.IsNullOrEmpty(Step2BOutputDirectoryTextBox.Text))
						Step2BOutputDirectoryTextBox.Text = ap.lm.Results;
                    SetCheckListBoxAnalyzers();
                    ShowCurrentAnalzyerSelection();
                    break;
                case AnalysisWizard.AWSteps.Step4:
                    // step 4 next creates the selected analyzer and adds it to the ap var
                    break;
                case AnalysisWizard.AWSteps.Step5:

                    break;
                default:
                    break;
            }
        }

        void FileSpecStateUpdate()
        {
            CycleNumTextBox.Enabled = Step2BWriteDataFiles.Checked;
            IntervalTextBox.Enabled = Step2BWriteDataFiles.Checked;
            CycleNumTextBox.Refresh(); IntervalTextBox.Refresh();
        }
        void RefreshDetectorCombo(ComboBox cb)
        {
            // Populate the combobox in the selector panel
            cb.Items.Clear();
            foreach (Detector d in NC.App.DB.Detectors)
            {
                if (d.ListMode)
                    cb.Items.Add(d);
            }
        }

        private void AnalysisWizard_Load(object sender, EventArgs e)
        {

        }

        private void ReviewDetector_Click(object sender, EventArgs e)
        {
            Detector d = (Detector)Step2BDetectorComboBox.SelectedItem;
            if (d == null)
                return;
            int idx = -1, i = 0;
            if (d != null)
                foreach (object o in this.Step2BDetectorComboBox.Items)
                {
                    if (o.ToString().CompareTo(d.ToString()) == 0)
                    {
                        idx = i;
                        break;
                    }
                    i++;
                }

            if (d.ListMode)
            {
                LMConnectionParams f = new LMConnectionParams(d, ap, false);
                f.StartWithLMDetail();
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                }
            }
            else
            {
                MeasSetup f = new MeasSetup();
                f.InitialSelection = idx;
                f.ShowDialog();
                if (f.DialogResult == true)
                {

                }
            }
        }

        private void DefineNewLM_Click(object sender, EventArgs e)
        {

        }

        private void CycleNumTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number            
            ap.modified |= (Format.ToPNZ(((TextBox)sender).Text, ref ap.num_runs));
            ((TextBox)sender).Text = Format.Rend(ap.num_runs);
            // if (fromINCCAcquire)
            ap.lm.Cycles = ap.num_runs;
        }
        private void IntervalTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number
            double ap_run_count_time = ap.run_count_time;
            ap.modified |= (Format.ToPNZ(((TextBox)sender).Text, ref ap_run_count_time));
            ap.run_count_time = ap_run_count_time;
            // Auto-format or reset the textbox value, depending on whether the entered value was different/valid
            ((TextBox)sender).Text = Format.Rend(ap.run_count_time);
            //   if (fromINCCAcquire)
            ap.lm.Interval = ap.run_count_time;
        }

        private void CycleIntervalPatch() // patches are nearly always a bad thing eh wot?
        {
            if (ap.lm.Interval <= 0)
            {
                ap.lm.Interval = ap.run_count_time;
                ap.modified = ap.lm.modified = true;
            }

            if (ap.lm.Cycles < 1)
            {
                ap.lm.Cycles = ap.num_runs;
                ap.modified = ap.lm.modified = true;
            }
        }

        private void Step2IncludeConfigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.IncludeConfig != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.IncludeConfig = ((CheckBox)sender).Checked;
            }
        }

        private void Step2AutoOpenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.OpenResults != ((CheckBox)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.OpenResults = ((CheckBox)sender).Checked;
            }
        }

        private void Step2SaveEarlyTermCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.SaveOnTerminate != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.SaveOnTerminate = ((CheckBox)sender).Checked;
            }
        }

        private void Step2BIncludeConfigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.IncludeConfig != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.IncludeConfig = ((CheckBox)sender).Checked;
            }
        }

        private void Step2BAutoOpenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.OpenResults != ((CheckBox)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.OpenResults = ((CheckBox)sender).Checked;
            }
        }

        private void Step2BWriteDataFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.LiveFileWrite != ((CheckBox)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.LiveFileWrite = ((CheckBox)sender).Checked;
            }
        }

        private void Step2BSaveEarlyTermCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.SaveOnTerminate != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.SaveOnTerminate = ((CheckBox)sender).Checked;
            }
        }

        private void Step2NCDRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                ap.data_src = ConstructedSource.NCDFile;  // NEXT: inconsistent logic with other 3 radios
        }

        private void Step2SortedPulseRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.PulseFileAssay != ((RadioButton)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.PulseFileAssay = ((RadioButton)sender).Checked;
            }
            if (NC.App.AppContext.PulseFileAssay)
                ap.data_src = ConstructedSource.SortedPulseTextFile;
        }

        private void Step2PTR32RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.PTRFileAssay != ((RadioButton)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.PTRFileAssay = ((RadioButton)sender).Checked;
            }
            if (NC.App.AppContext.PTRFileAssay)
                ap.data_src = ConstructedSource.PTRFile;
        }
        private void Step2MCA527RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.MCA527FileAssay != ((RadioButton)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.MCA527FileAssay = ((RadioButton)sender).Checked;
            }
            if (NC.App.AppContext.MCA527FileAssay)
                ap.data_src = ConstructedSource.MCA527File;
        }

        private void Step2RecurseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NC.App.AppContext.Recurse != ((CheckBox)sender).Checked)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.Recurse = ((CheckBox)sender).Checked;
            }
        }

        private void Step2OutputDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            String s = ap.lm.Results;
            ap.lm.modified = Format.Changed(((TextBox)sender).Text, ref s);
            if (ap.lm.modified)
                ap.lm.Results = s;
        }

        private void Step2BOutputDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            String s = ap.lm.Results;
            ap.lm.modified = Format.Changed(((TextBox)sender).Text, ref s);
            if (ap.lm.modified)
                ap.lm.Results = s;
        }

        AWAnalysisType CheckBoxTextMap(string s)
        {
            AWAnalysisType at = AWAnalysisType.NullType;
            if (s.Equals("Multiplicity with Fast Accidentals"))
                at = AWAnalysisType.MultFast;
            else if (s.Equals("Multiplicity (conventional)"))
                at = AWAnalysisType.Mult;
            else if (s.Equals("Coincidence matrix"))
                at = AWAnalysisType.CoinMat;
            else if (s.Equals("Feynman"))
                at = AWAnalysisType.Feynman;
            else if (s.Equals("Event spacing"))
                at = AWAnalysisType.Event;
            else if (s.Equals("Rossi-α (alpha)"))
                at = AWAnalysisType.Rossi;
            else if (s.Equals("Rates"))
                at = AWAnalysisType.Rates;

            return at;
        }

        private void LMAnalyzers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = (string)((CheckedListBox)sender).SelectedItem;
            AWAnalysisType at = CheckBoxTextMap(s);
            if (at != AnalysisType)
            {
                AnalysisType = at;
                ShowCurrentAnalzyerSelection(); // do refresh here
            }
        }

        private void DefSR_Click(object sender, EventArgs e)
        {
            foreach (string s in LMAnalyzers.Items)
            {
                AWAnalysisType at = CheckBoxTextMap(s);
                switch (at)
                {
                    case AWAnalysisType.Rossi:
                        break;
                    case AWAnalysisType.Rates:
                        break;
                    case AWAnalysisType.Event:
                        break;
                    case AWAnalysisType.Feynman:
                        break;
                    case AWAnalysisType.CoinMat:  // like mult infra
                        AnalysisDefs.Coincidence co = alt.GetFirstCoincidence(activeOnly: false, addIfNotPresent: true);
                        // NEXT: incomplete


                        break;
                    case AWAnalysisType.Mult:
                        Multiplicity mFAOff = GetAMergedMult(FAType.FAOff);
                        MultConvAccInitDelayTextBox.Text = mFAOff.SR.predelay.ToString();
                        MultConvAccGateWidthTextBox.Text = mFAOff.gateWidthTics.ToString();
                        MultConvAccClockWidthTextBox.Text = mFAOff.AccidentalsGateDelayInTics.ToString();
                        break;
                    case AWAnalysisType.MultFast:
                        Multiplicity mFA = GetAMergedMult(FAType.FAOn);
                        MultFastAccInitDelayTextBox.Text = mFA.SR.predelay.ToString();
                        MultFastAccGateWidthTextBox.Text = mFA.gateWidthTics.ToString();
                        MultFastAccClockWidthTextBox.Text = mFA.BackgroundGateTimeStepInTics.ToString();
                        break;

                    default:
                        break;
                }
            }

        }


        Multiplicity GetAMergedMult(FAType FA)
        {
            bool addedIfNotPresent = false;
            Multiplicity m0 = alt.GetFirstMult(FA, out addedIfNotPresent);
            if (addedIfNotPresent)
            {
                m0.SR.CopyValues(det.SRParams);  // merge default values with this analyzer
                m0.SetGateWidthTics(det.SRParams.gateLength);
            }
            return m0;
        }

		private void Comment_Leave(object sender, EventArgs e)
		{
            bool thisonechanged = (string.Compare(ap.comment,((TextBox)sender).Text) != 0);
			if (thisonechanged)
			{
				ap.modified = true;
				ap.comment = string.Copy(((TextBox)sender).Text);
			}
		}
	}

}
