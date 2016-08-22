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
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using AnalysisDefs;
using DetectorDefs;
using Instr;
using NCCReporter;

namespace NewUI
{
	using Integ = NCC.IntegrationHelpers;
	using NC = NCC.CentralizedState;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {

        private WinPos main;
        public MainWindow()
        {
            InitializeComponent();
			main = new WinPos();
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /////////////////
        //  FILE MENU  //
        ///////////////// 

        //////////////////
        //  SETUP MENU  //
        //////////////////

        private void SetupFacilityClick(object sender, RoutedEventArgs e)
        {
            IDDFacility f = new IDDFacility();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
               // nothing to do here
            }
        }

        private void SetupMeasParamsClick(object sender, RoutedEventArgs e)
        {
            Detector det = Integ.GetCurrentAcquireDetector();
         
            IDDShiftRegisterSetup f = new IDDShiftRegisterSetup(det);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)  // copy SR and SR type/baud params to the current detector obj, then do the DB push
            {
                //det.SRParams.CopyValues(f.sr);
                // only SR values copied here, changes to baud and det type (not on SRParams) already copied in dlg OK event 
                // Conceptually possible, but practically too much work: if type changed from SR to LMMM, call up the LMMM dialogs now
                //NC.App.DB.UpdateDetector(det);
            }
            
        }

        private void SetupLMInstConnParamsClick(object sender, RoutedEventArgs e)
        {
            Detector det =null;
            AcquireParameters acq = null;
            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            if (det.ListMode)
            {
                NewUI.LMConnectionParams f = new NewUI.LMConnectionParams(det, acq, false);
                f.StartWithLMDetail();
                f.ShowDialog();
            }
            else
            {
                SetupMeasParamsClick(sender, e);
            }
        }

        private void SetupIsotopicsClick(object sender, RoutedEventArgs e)
        {
            IDDIsotopics f = new IDDIsotopics();
            f.ShowDialog();
        }

        private void SetupCompositeIsotopicsClick(object sender, RoutedEventArgs e)
        {
            IDDCompositeIsotopics f = new IDDCompositeIsotopics(string.Empty);
            f.ShowDialog();
        }

        private void SetupItemDataEntryClick(object sender, RoutedEventArgs e)
        {
            IDDItemDataEntry f = new IDDItemDataEntry();
            f.ShowDialog();
        }

        private void SetupCollarItemDataEntryClick(object sender, RoutedEventArgs e)
        {
            IDDCollarData f = new IDDCollarData();
            f.ShowDialog();
			if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.EditItem)
			{
				MessageBox.Show("The Item Id Entry dialogbox will now be displayed for you to enter\r\n" + 
								"the item data needed for the collar data you just entered", NCC.IntegrationHelpers.GetAppTitle());
				IDDItemDataEntry g = new IDDItemDataEntry();
				g.ShowDialog();
			}
        }        

        /////////////////////
        //  MAINTAIN MENU  //
        /////////////////////

        private void MaintainDemingClick(object sender, RoutedEventArgs e)
        {
            IDDDemingFit f = new IDDDemingFit();
            f.ShowDialog();
			if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
				f.ict.ShowDialog();
        }

        private void MaintainAnalysisMethodsClick(object sender, RoutedEventArgs e)
        {
            IDDAnalysisMethodsConfig f = new IDDAnalysisMethodsConfig();
            f.ShowDialog();
        }

        private void MaintainPassiveCalCurveClick(object sender, RoutedEventArgs e)
        {
            IDDCalibrationCurveCal f = new IDDCalibrationCurveCal();
            f.ShowDialog();
        }

        private void MaintainKnownAlphaClick(object sender, RoutedEventArgs e)
        {
            IDDKnownAlphaCal f = new IDDKnownAlphaCal();
            f.ShowDialog();
        }

        private void MaintainKnownMClick(object sender, RoutedEventArgs e)
        {
            IDDKnownMCal f = new IDDKnownMCal();
            f.ShowDialog();
        }

        private void MaintainPassiveMultClick(object sender, RoutedEventArgs e)
        {
            IDDMultiplicityCal f = new IDDMultiplicityCal();
            f.ShowDialog();
        }

        private void MaintainAddASourceClick(object sender, RoutedEventArgs e)
        {
            IDDAddASourceCal f = new IDDAddASourceCal();
            f.ShowDialog();
        }

        private void MaintainCuriumRatioClick(object sender, RoutedEventArgs e)
        {
            IDDCuriumRatioCalibration f = new IDDCuriumRatioCalibration();
            f.ShowDialog();
        }

        private void MaintainTruncatedMultClick(object sender, RoutedEventArgs e)
        {
            IDDTruncatedMultCalibration f = new IDDTruncatedMultCalibration();
            f.ShowDialog();
        }

        private void MaintainActiveCalCurveClick(object sender, RoutedEventArgs e)
        {
            IDDActiveCal f = new IDDActiveCal();
            f.ShowDialog();
        }

        private void MaintainCollarClick(object sender, RoutedEventArgs e)
        {
            //HN -- Cross ref shown first, next button takes you to IDDCollarCal
            IDDCollarCrossRef f = new IDDCollarCrossRef(); 
            WinPos childPos = main.GetChildPos(f.Height, f.Width);
            f.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            if (childPos.height < f.Height || childPos.width < f.Width) // Resize if it will go off screen.
                f.Size = new System.Drawing.Size((int)childPos.width, (int)childPos.height);
            f.Location = new System.Drawing.Point((int)childPos.left, (int)childPos.top);

            f.ShowDialog();
        }

        private void MaintainActiveMultClick(object sender, RoutedEventArgs e)
        {
            IDDActiveMultCal f = new IDDActiveMultCal();
            f.ShowDialog();
        }

        private void MaintainActivePassiveClick(object sender, RoutedEventArgs e)
        {
            IDDActivePassiveCal f = new IDDActivePassiveCal();
            f.Show();
        }

        private void MaintainBackgroundSetupClick(object sender, RoutedEventArgs e)
        {
            IDDBackgroundSetup f = new IDDBackgroundSetup();
            f.ShowDialog();
        }

        private void MaintainNormSetupClick(object sender, RoutedEventArgs e)
        {
            IDDNormalizationSetup f = new IDDNormalizationSetup();
            f.ShowDialog();
        }

        private void MaintainUnattendedMeasSetupClick(object sender, RoutedEventArgs e)
        {
            IDDSetupUnattendedMeas
            f = new IDDSetupUnattendedMeas(Integ.GetCurrentAcquireDetector());
            f.ShowDialog();
        }

        private void MaintainQCTestClick(object sender, RoutedEventArgs e)
        {
            IDDTestParametersSetup f = new IDDTestParametersSetup();
            f.ShowDialog();
        }

        private void MaintainStratumRejectionLimitsClick(object sender, RoutedEventArgs e)
        {
            IDDStratumLimits f = new IDDStratumLimits();
            f.ShowDialog();
        }

        private void MaintainErrorCalcMethodClick(object sender, RoutedEventArgs e)
        {
            IDDErrorCalcMethod f = new IDDErrorCalcMethod();
            f.ShowDialog();
        }

        private void MaintainSweepClick(object sender, RoutedEventArgs e)
        {
            IDDSweepFeature f = new IDDSweepFeature();
            f.ShowDialog();
        }

        private void MaintainArchiveClick(object sender, RoutedEventArgs e)
        {
            IDDArchiveConfig f = new IDDArchiveConfig();
            f.ShowDialog();
        }

        private void MaintainDetectorAddEditClick(object sender, RoutedEventArgs e)
        {
            IDDDetectorEdit f = new IDDDetectorEdit();
            f.ShowDialog();
        }

        private void MaintainFacilityAddDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDFacilityEdit f = new IDDFacilityEdit();
            f.ShowDialog();
        }

        private void MaintainMBAAddDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDMBAEdit f = new IDDMBAEdit();
            f.ShowDialog();
        }

        private void MaintainStratumIdAddDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDStratumId f = new IDDStratumId();
            f.ShowDialog();
        }

        private void MaintainMaterialTypeAddDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDItemType f = new IDDItemType();
            f.ShowDialog();
        }

        private void MaintainPoisonRodTypeAddDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDPoisonRodType f = new IDDPoisonRodType();
            f.ShowDialog();
        }

        private void MaintainGloveboxAddEditDeleteClick(object sender, RoutedEventArgs e)
        {
            IDDGlovebox f = new IDDGlovebox();
            f.ShowDialog();
        }

        private void MaintainAddASourceSetupClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MaintainDeleteMeasurementClick(object sender, RoutedEventArgs e)
        {
            IDDDeleteMeas f = new IDDDeleteMeas();
            f.ShowDialog();
        }

        private void MaintainSomeSettingsClick(object sender, RoutedEventArgs e)
        {
            Preferences f = new Preferences();
            f.ShowDialog();
        }

        ////////////////////
        //  ACQUIRE MENU  //
        ////////////////////

        private void AcquireRatesClick(object sender, RoutedEventArgs e)
        {
            IDDAcquireRatesOnly f = new IDDAcquireRatesOnly();
            f.ShowDialog();
        }

        private void AcquireBackgroundClick(object sender, RoutedEventArgs e)
        {
            IDDAcquireBackground f = new IDDAcquireBackground();
            f.Show();  // Note: keep like this, try a measurement acquire while rest of software is available
        }

        private void AcquireInitSrcClick(object sender, RoutedEventArgs e)
        {            
            IDDAcquireInitSrc f = new IDDAcquireInitSrc();
            switch (f.np.biasMode)
            {
                default:
                    f.ShowDialog();
                    break;
                case NormTest.Collar:
                    MessageBox.Show("Initial source measurements cannot be done when the normalization type is collar.");
                    break;
            }

        }

        private void AcquireNormClick(object sender, RoutedEventArgs e)
        {
            IDDAcquireBias f = new IDDAcquireBias();          
	        /* if an Initial source measurement has not been done for this detector, do not allow a bias test to be performed. */
	        if ((f.np.biasMode== NormTest.AmLiSingles) &&
		        (f.np.amliRefSinglesRate <= 0.0))
                MessageBox.Show(String.Format("Normalization measurements for detector {0} cannot be done until an Initial Source measurement has been made, or the reference singles rate and date are entered manually.", f.Acq.detector_id));
	        else if ((f.np.biasMode== NormTest.Cf252Doubles) &&
		        (f.np.cf252RefDoublesRate.v <= 0.0))
                MessageBox.Show(String.Format("Normalization measurements for detector {0} cannot be done until an Initial Source measurement has been made, or the reference doubles rate and date are entered manually.", f.Acq.detector_id));
	        else if ((f.np.biasMode== NormTest.Cf252Singles) &&
		        (f.np.cf252RefDoublesRate.v <= 0.0))
                MessageBox.Show(String.Format("Normalization measurements for detector {0} cannot be done until an Initial Source measurement has been made, or the reference singles rate and date are entered manually.", f.Acq.detector_id));
            else
                f.ShowDialog();
        }

        private void AcquirePrecisionClick(object sender, RoutedEventArgs e)
        {
            IDDAcquirePrecision f = new IDDAcquirePrecision();
            f.ShowDialog();
        }

        private void AcquireVerificationClick(object sender, RoutedEventArgs e)
        {
            IDDAcquireAssay f = new IDDAcquireAssay();
            f.ShowDialog();
        }

        private void AcquireCalMeasClick(object sender, RoutedEventArgs e)
        {
            IDDAcquireCalibration f = new IDDAcquireCalibration();
            f.ShowDialog();
        }

        private void AcquireHoldupClick(object sender, RoutedEventArgs e)
        {
            // TODO:  Needs glovebox definition test prior to this call:
            IDDAcquireHoldup f = new IDDAcquireHoldup();
            f.ShowDialog();
        }

        //////////////////////
        //  REANALYZE MENU  //
        //////////////////////

        private void ReanalyzeVerificationClick(object sender, RoutedEventArgs e)
        {
            IDDReanalyzeDBMeas f = new IDDReanalyzeDBMeas();
            f.ShowOther();
        }

        ///////////////////
        //  REPORT MENU  //
        ///////////////////

        private void ReportAllMeasClick(object sender, RoutedEventArgs e)
        {
            IDDReviewAll f = new IDDReviewAll(LMOnly:false);
            if (f.bGood)
                f.Show();
        }

        private void ReportRatesClick(object sender, RoutedEventArgs e)
        {
            IDDReviewRatesOnly f = new IDDReviewRatesOnly();
            f.Show();
        }

        private void ReportBackgroundClick(object sender, RoutedEventArgs e)
        {
            IDDReviewBackground f = new IDDReviewBackground();
            f.Show();
        }

        private void ReportInitialSourceClick(object sender, RoutedEventArgs e)
        {
            IDDReviewInitSrc f = new IDDReviewInitSrc();
            f.Show();
        }

        private void ReportNormClick(object sender, RoutedEventArgs e)
        {
            IDDReviewNormalization f = new IDDReviewNormalization();
            f.Show();
        }

        private void ReportPrecisionClick(object sender, RoutedEventArgs e)
        {
            IDDReviewPrecision f = new IDDReviewPrecision();
            f.Show();
        }

        private void ReportVerificationClick(object sender, RoutedEventArgs e)
        {
            IDDReviewAssay f = new IDDReviewAssay();
            f.Show();
        }

        private void ReportCalibrationMeasClick(object sender, RoutedEventArgs e)
        {
            IDDReviewCalibration f = new IDDReviewCalibration();
            f.Show();
        }

        private void ReportHoldupClick(object sender, RoutedEventArgs e)
        {
            IDDReviewHoldup f = new IDDReviewHoldup();
            f.Show();
        }

		private void ReportLMClick(object sender, RoutedEventArgs e)
        {
            IDDReviewAll f = new IDDReviewAll(LMOnly:true);
            if (f.bGood)
                f.Show();
        }

        private void ReportMeasSummaryClick(object sender, RoutedEventArgs e)
        {
            IDDAssaySummary f = new IDDAssaySummary(string.Empty);   // all types
            f.Show();
        }

        private void ReportVerificationSummaryClick(object sender, RoutedEventArgs e)
        {
            IDDAssaySummary f = new IDDAssaySummary("verification");
            f.Show();
        }

        private void ReportHoldupSummaryClick(object sender, RoutedEventArgs e)
        {
            IDDAssaySummary f = new IDDAssaySummary("holdup");   // NYI
            f.Show();
        }

        private void ReportPlotNormHistoryClick(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void ReportPlotCalVerificationClick(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        //////////////////
        //  TOOLS MENU  //
        //////////////////

        private void ToolsReviewToolClick(object sender, RoutedEventArgs e)
        {
            IDDTools f = new IDDTools();
            f.Show();
        }

        /////////////////
        //  HELP MENU  //
        /////////////////

        private void HelpTopicsClick(object sender, RoutedEventArgs e)
        {
            // Opens some Windows help window
            System.Windows.Forms.Help.ShowHelp (null,".\\inccuser.chm");
        }

        private void HelpProceduresClick(object sender, RoutedEventArgs e)
        {
            // Opens a text file
        }

        private void HelpAppInfoClick(object sender, RoutedEventArgs e)
        {
            IDDHelpInfoDialog f = new IDDHelpInfoDialog();
            f.Show();
        }

        private void HelpAboutINCCClick(object sender, RoutedEventArgs e)
        {
            Splash f = new Splash();
            f.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        ///////////////////
        //  MOUSER MENU  //
        ///////////////////

        private void AnalysisWizardClick(object sender, RoutedEventArgs e)
        {
            AcquireParameters acq = null;
            Detector det = null;
			NC.App.DB.ResetAcquireParametersMap();  // force read from DB to get as-is default acquire state
			// next: do a refresh of the specific acquire instance, not the entire list
            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            if (!det.ListMode)
            {
                MessageBox.Show("'" + det.ToString() + "' is not a List Mode detector,\r\ncreate or select a List Mode detector\r\n with Setup > Facility/Inspection...", "List Mode Acquire");
                return;
            }
            AnalysisWizard f = new AnalysisWizard(AnalysisWizard.AWSteps.Step3, acq, det);
            System.Windows.Forms.DialogResult dr = f.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                if (acq.modified || acq.lm.modified)
                {
                    INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, DateTime.Now);
                    acq.MeasDateTime = sel.TimeStamp; acq.lm.TimeStamp = sel.TimeStamp;
                    NC.App.DB.AddAcquireParams(sel, acq);  // it's a new one, not the existing one modified
                }

                switch (NC.App.Opstate.Measurement.AcquireState.data_src)  // global access to latest acq here, same as acq set in wizard
                {
                    case ConstructedSource.Live:
                        UIIntegration.Controller.file = false;  // make sure to use the DAQ controller, not the file controller
                        NC.App.AppContext.FileInput = null;  // reset the cmd line file input flag
                        if (NC.App.Opstate.Measurement.Detector.ListMode)
                        {
                            // if ok, the analyzers are set up, so can kick it off now.
                            if (NC.App.Opstate.Measurement.Detector.Id.SRType == InstrType.PTR32)
                            {
                                Ptr32Instrument instrument = new Ptr32Instrument(NC.App.Opstate.Measurement.Detector);
                                instrument.DAQState = DAQInstrState.Offline;
                                instrument.selected = true;
                                instrument.Init(NC.App.Logger(LMLoggers.AppSection.Data), NC.App.Logger(LMLoggers.AppSection.Analysis));  // todo: is this reduntant?
                                if (!Instruments.Active.Contains(instrument))
                                    Instruments.Active.Add(instrument);
                            }
                            else if (NC.App.Opstate.Measurement.Detector.Id.SRType == InstrType.MCA527)
                            {
                                MCA527Instrument mca = new MCA527Instrument(NC.App.Opstate.Measurement.Detector);
                                mca.DAQState = DAQInstrState.Offline; // these are manually initiated as opposed to auto-pickup
                                mca.selected = true;
								mca.Init(NC.App.Logger(LMLoggers.AppSection.Data), NC.App.Logger(LMLoggers.AppSection.Analysis));
                                if (!Instruments.Active.Contains(mca))
                                    Instruments.Active.Add(mca);                                
                            } 
							else // LMMM
							{
                                LMInstrument lm = new LMInstrument(NC.App.Opstate.Measurement.Detector);
                                lm.DAQState = DAQInstrState.Offline; // these are manually initiated as opposed to auto-pickup
                                lm.selected = false;  //must broadcast first to get it selected
                                if (!Instruments.All.Contains(lm))
                                    Instruments.All.Add(lm); // add to global runtime list		
							}
                        }
                        else
                        {
                            SRInstrument sri = new SRInstrument(NC.App.Opstate.Measurement.Detector);
                            sri.selected = true;
                            sri.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
                            if (!Instruments.All.Contains(sri))
                                Instruments.All.Add(sri); // add to global runtime list 
                        }
                        break;
                    case ConstructedSource.DB:
                        UIIntegration.Controller.file = true;
                        return;
                        //break;
                    case ConstructedSource.Manual:
                        UIIntegration.Controller.file = true;
                        return;
                        //break;
                    case ConstructedSource.CycleFile:
                    case ConstructedSource.ReviewFile:
                        UIIntegration.Controller.file = true;
                        string xs = UIIntegration.GetUsersFolder("Select Input Folder", NC.App.AppContext.FileInput);
                        if (!String.IsNullOrEmpty(xs))
                        {
                            NC.App.AppContext.FileInput = xs;
                            NC.App.AppContext.FileInputList = null;  // no explicit file list
                        }
                        SRInstrument sri2 = new SRInstrument(NC.App.Opstate.Measurement.Detector);
                        sri2.selected = true;
                        sri2.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
                        if (!Instruments.All.Contains(sri2))
                            Instruments.All.Add(sri2); // add to global runtime list 
                        break;

                    case ConstructedSource.NCDFile:
                        NC.App.AppContext.NCDFileAssay = true; // suntoucher, this is right here how we're flowing now
                        UIIntegration.Controller.file = true;
                        break;
                    case ConstructedSource.SortedPulseTextFile:
                        NC.App.AppContext.PulseFileAssay = true;
                        UIIntegration.Controller.file = true;
                        break;
                    case ConstructedSource.PTRFile:
                        NC.App.AppContext.PTRFileAssay = true;
                        UIIntegration.Controller.file = true;
                        break;
                    case ConstructedSource.MCA527File:
                        NC.App.AppContext.MCA527FileAssay = true;
                        UIIntegration.Controller.file = true;
                        break;
                    default:
                        break;
                }
                NC.App.Opstate.Measurement.Detector.Id.source = NC.App.Opstate.Measurement.AcquireState.data_src;  // set the detector overall data source value here
                UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
            }
        }


		private void TransferFileInClick(object sender, RoutedEventArgs e)
		{

			string s = UIIntegration.GetUsersFolder("Select Input Transfer File Folder", NC.App.AppContext.FileInput);
			if (!string.IsNullOrEmpty(s))
			{
				NC.App.AppContext.FileInput = s;
				NC.App.AppContext.FileInputList = null;  // no explicit file list
				UIIntegration.Controller.file = true;
				UIIntegration.Controller.SetFileTransform();  // it is a file action
				NC.App.AppContext.MutuallyExclusiveFileActions(NCCConfig.NCCFlags.INCCXfer, true);  // xfer file processing only
				bool ok = UIIntegration.Controller.procFctrl.SpecialPrepAction();
				if (!ok)
					return;
				UIIntegration.Controller.Perform();  // run the current specified operation
			}
		}

		private void TransferFileOutClick(object sender, RoutedEventArgs e)
		{
			IDDSaveTransfer q = new IDDSaveTransfer();
			q.ShowDialog();
		}

		private void InitialDataSelectorClick(object sender, RoutedEventArgs e)
        {
            IDDRestoreInitialData d = new IDDRestoreInitialData();  // all processing occurs in the OK handler 
            d.ShowDialog();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void EnableLog_Unchecked(object sender, RoutedEventArgs e)
        {
            logLevels.IsEnabled = false; logResults.IsEnabled = false;
            if (NC.App.AppContext.Logging)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.Logging = false;
            }
        }

        private void EnableLog_Checked(object sender, RoutedEventArgs e)
        {
            logLevels.IsEnabled = true; logResults.IsEnabled = true;
            if (!NC.App.AppContext.Logging)
            {
                NC.App.AppContext.modified = true; NC.App.AppContext.Logging = true;
            }
        }

        private void logLevels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ushort idx = (ushort)((ComboBox)sender).SelectedIndex;
            if (NC.App.AppContext.LevelAsUInt16 != idx)
            {
                if (idx >= 0)
                {
                    NC.App.AppContext.SetLevel(idx); // 0 is none
                    NC.App.AppContext.modified = true;
                    NC.App.Loggers.UpdateFilterLevel(idx);
                }
            }
        }

        private void logResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NC.App.AppContext.LogResults != (ushort)((ComboBox)sender).SelectedIndex)
            {
                NC.App.AppContext.LogResults = (ushort)((ComboBox)sender).SelectedIndex;
                NC.App.AppContext.modified = true;
            }
        }

        private void HVPClick(object sender, RoutedEventArgs e)
        {
            IDDHighVoltagePlateau d = new IDDHighVoltagePlateau(); // love sweet sound
            d.ShowDialog();
        }

        private void ChangeDatabaseClick(object sender, RoutedEventArgs e)
        {
            IDDDBPicker dbpick = new IDDDBPicker();
            dbpick.ShowDialog();
            //todo: decide if we need to allow on-the-fly DB changes or they just change the App.Config file with new DB name.
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            //todo: Are there other cleanup tasks to do on closing?
            Close();
        }


        private void PSALogsheetOutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is not implemented yet.", "DOING NOTHING NOW");
        }

        private void PerformanceMonitoringFileOutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is not implemented yet.", "DOING NOTHING NOW");
        }

        private void InitialDataOutClick(object sender, RoutedEventArgs e)
        {
			IDDSaveInitialData f = new IDDSaveInitialData();
			f.ShowDialog();
        }

        private void StratumAuthorityFileClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog aDlg = new System.Windows.Forms.OpenFileDialog();
            aDlg.CheckFileExists = true;
            aDlg.FileName = "NCC_Strat.dat";
            aDlg.Filter = "Dat files (*.dat)|*.dat|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            aDlg.DefaultExt = ".dat";
            aDlg.InitialDirectory = NC.App.AppContext.FileInput;
            aDlg.Title = "Select a Stratum Authority file";
            aDlg.Multiselect = false;
            aDlg.RestoreDirectory = true;
            System.Windows.Forms.DialogResult qw = aDlg.ShowDialog();
            if (qw == System.Windows.Forms.DialogResult.OK)
            {
                NCCFile.SAFile onefile = new NCCFile.SAFile();
                string path = System.IO.Path.GetFullPath(aDlg.FileName);
                onefile.Process(path);
            }
        }

        private void ItemRelevantDataClick(object sender, RoutedEventArgs e)
        {
			System.Windows.Forms.OpenFileDialog aDlg =  new System.Windows.Forms.OpenFileDialog();
			aDlg.CheckFileExists = true;
			aDlg.FileName = "NCC_Item.dat";
            aDlg.Filter = "Dat files (*.dat)|*.dat|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            aDlg.DefaultExt = ".dat";
            aDlg.InitialDirectory = NC.App.AppContext.FileInput;
            aDlg.Title = "Select an Item Relevant Data file";
            aDlg.Multiselect = false;
            aDlg.RestoreDirectory = true;
			System.Windows.Forms.DialogResult qw = aDlg.ShowDialog();
            if (qw == System.Windows.Forms.DialogResult.OK)
			{
				NCCFile.ItemFile onefile = new NCCFile.ItemFile();
				string path = System.IO.Path.GetFullPath(aDlg.FileName);
				onefile.Process(path);
				// todo: now do something with the results
			}
			
        }

        private void BackupAllDataClick(object sender, RoutedEventArgs e)
        {
            string dest = UIIntegration.GetUsersFolder("Select Destination", string.Empty);
            if (!string.IsNullOrEmpty(dest))
            {
                //Path to sql or sqlite files
                string destFileName;
                destFileName = string.Format("INCC-{0}.zip", DateTime.Now.ToString ("yyyy-MM-dd-HH-mm-ss"));
                using (FileStream zipToOpen = new FileStream(Path.Combine (dest,destFileName), FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        // Cannot archive connected DB or cfg, copy, archive, then delete.
                        // TODO: is this all that we want to save? HN 7/27/2016
                        string DBFolder = Path.GetDirectoryName(NC.App.Pest.GetDBFileFromConxString());
                        string DBFile = Path.GetFullPath(NC.App.Pest.GetDBFileFromConxString());
                        string DBFileName = Path.GetFileName(NC.App.Pest.GetDBFileFromConxString());
                        string CfgPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                        File.Copy(Path.Combine(DBFolder,DBFile), Path.Combine(dest,DBFileName),true);
                        File.Copy(CfgPath, Path.Combine (dest, Path.GetFileName(CfgPath)), true);

                        ZipArchiveEntry dbFile = archive.CreateEntryFromFile(Path.Combine(dest,DBFileName),DBFileName);
                        ZipArchiveEntry cfgFile = archive.CreateEntryFromFile(Path.Combine(dest, Path.GetFileName(CfgPath)), Path.GetFileName(CfgPath));
                        NC.App.Loggers.AppLogger.TraceEvent(LogLevels.Info, 1111, String.Format("Zip file containing database and config created: {0}", Path.Combine(dest, destFileName)));
                        File.Delete(Path.Combine(dest, DBFileName));
                        File.Delete(Path.Combine(dest, Path.GetFileName(CfgPath)));

                    }
                }

            }
            else
                MessageBox.Show("The destination folder could not be created", "ERROR");
        }

        private void RestoreAllDataClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            openFileDialog1.InitialDirectory = Path.GetTempPath();
            openFileDialog1.Filter = "archive files (*.zip)|*.7z|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (ZipArchive archive = new ZipArchive(openFileDialog1.OpenFile()))
                {
                    // TODO: What does restore mean? Right now, just extracts to temp. 
                    // would have to do some somersaults to switch DB and change app context to stored config.
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(Path.GetTempPath(), entry.FullName),true);
                    }
                }
            }
        }

        private void BatchAnalysisClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is not implemented yet.", "DOING NOTHING NOW");
        }

        private void SetupPrinterClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is not implemented yet.", "DOING NOTHING NOW");
        }
    }
}
