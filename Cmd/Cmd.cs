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
using AnalysisDefs;
using NCCReporter;
using System;
using System.Diagnostics;

namespace NCCCmd
{
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

	class AppEntry
	{

		static void Main(string[] args)
		{
			new N("INCC6 Console");
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			NCCConfig.Config c = new NCCConfig.Config(); // gets initial DB params from config file
            c.BeforeDBSetup(args);
			if (!N.App.LoadPersistenceConfig(c.DB)) // loads up DB, sets global AppContext
				return;
			c.AfterDBSetup(N.App.AppContext, args);  // apply the cmd line, after database init
            string[] possiblepaths = NCCFile.FileCtrl.ProcessINCC5IniFile(N.App.ControlLogger); // iRap: optional use of INCC5 ini file to find results and output paths
            if (possiblepaths.Length > 2)  // use the iRAP defined input, results and log file paths
            {
                N.App.AppContext.FileInput = possiblepaths[0];
                if (System.IO.Directory.Exists(possiblepaths[1]))
                    N.App.AppContext.ResultsFilePath = possiblepaths[1];
                if (System.IO.Directory.Exists(possiblepaths[2]))
                    N.App.AppContext.LogFilePath = possiblepaths[2];
            }
            // check return bool and exit here on error
            bool initialized = N.App.Initialize(c);
			if (!initialized)
				return;

			if (N.App.Config.Cmd.ShowVersion && !N.App.Config.Cmd.Showcfg)
			{
				N.App.Config.Cmd.ShowVersionOnConsole(N.App.Config, N.App.Config.App.Verbose() == TraceEventType.Verbose);
				return;
			} 
			else if (N.App.Config.Cmd.Showhelp)
			{
				N.App.Config.ShowHelp();
				return;
			}
			else if (N.App.Config.Cmd.Showcfg)
			{
				NCCConfig.Config.ShowCfg(N.App.Config, N.App.Config.App.Verbose() == TraceEventType.Verbose);
				return;
			}

            LMLoggers.LognLM applog = N.App.ControlLogger;
			bool OpenResults = N.App.AppContext.OpenResults;
			try
			{
				applog.TraceInformation("==== Starting " + DateTime.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " [Cmd] " + N.App.Name + " " + N.App.Config.VersionString);
				applog.TraceInformation("==== DB " + N.App.Pest.DBDescStr);
				// These affect the current acquire state, so they occur here and not earlier in the initial processing sequence
				if (!string.IsNullOrEmpty(c.Cur.Detector) && !c.Cur.Detector.Equals("Default")) // command line set the value
					initialized = Integ.SetNewCurrentDetector(c.Cur.Detector, true);
				if (!initialized)
					goto end;

                if (!string.IsNullOrEmpty(c.Cur.Material) && !c.Cur.Material.Equals("Pu")) // command line set the value
                    initialized = Integ.SetNewCurrentMaterial(c.Cur.Material, true);
                if (!initialized)
                    goto end;

                if (!string.IsNullOrEmpty(c.Cur.ItemId)) // command line set the item value, use it to override the material and other acquire params
                    initialized = Integ.SetNewCurrentMaterial(c.Cur.Material, true);
                if (!initialized)
                    goto end;

				if (!N.App.AppContext.ReportSectional.Equals(NCCConfig.Config.DefaultReportSectional)) // command line set the value
				{
					AcquireParameters acq = null;
					Detector det = null;
					Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
					acq.review.Scan(N.App.AppContext.ReportSectional);
					acq.MeasDateTime = DateTime.Now;
	                N.App.DB.UpdateAcquireParams(acq, det.ListMode);
					N.App.ControlLogger.TraceEvent(LogLevels.Info, 32444, "The current report sections are now " + N.App.AppContext.ReportSectional);
				}
				if (N.App.Config.App.UsingFileInput || N.App.Opstate.Action == NCC.NCCAction.File)
				{
					if (N.App.AppContext.AssayFromFiles)
						N.App.Opstate.Action = NCC.NCCAction.Assay;
					else if (N.App.AppContext.HasFileAction)
						N.App.Opstate.Action = NCC.NCCAction.File;
				}

				if (N.App.Config.Cmd.Query != null)
				{
					AcquireParameters acq = Integ.GetCurrentAcquireParams();
					System.Collections.Generic.List<string> ls = acq.ToDBElementList(generate:true).AlignedNameValueList;
					foreach(string s in ls)
						Console.WriteLine(s);
					OpenResults = false;
					if (N.App.Opstate.Action == NCC.NCCAction.Nothing)
						return;
				}


				if (N.App.Config.App.UsingFileInput || N.App.Opstate.Action == NCC.NCCAction.File)
				{
	                BuildMeasurement();
					if (N.App.AppContext.AssayFromFiles)
						N.App.Opstate.Action = NCC.NCCAction.Assay;
					else if (N.App.AppContext.HasFileAction)
						N.App.Opstate.Action = NCC.NCCAction.File;

					// file processing for analysis and more
					FileControlBind filecontrol = new FileControlBind();
					filecontrol.SetupEventHandlers();
					filecontrol.StartAction();  // step into the code and run
					N.App.Opstate.SOH = NCC.OperatingState.Stopped;
				}
				else
				{
					BuildMeasurement();
					DAQControlBind control = new DAQControlBind();
					DAQ.DAQControl.ActivateDetector(N.App.Opstate.Measurement.Detector);
					control.SetupEventHandlers();
					control.StartAction();
					N.App.Opstate.SOH = NCC.OperatingState.Stopped;
                }
                end:
				;
			} catch (Exception e)
			{
				N.App.Opstate.SOH = NCC.OperatingState.Trouble;
				applog.TraceException(e, true);
				applog.EmitFatalErrorMsg();
			} finally
			{
				N.App.Opstate.SOH = NCC.OperatingState.Stopped;
				N.App.Config.RetainChanges();
				applog.TraceInformation("==== Exiting " + DateTime.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " [Cmd] " + N.App.Name + " . . .");
				N.App.Loggers.Flush();  
                if (OpenResults) Process.Start(System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe"), LMLoggers.LognLM.CurrentLogFilePath);
            }
		}

		static void BuildMeasurement()
		{
			Detector det = null;
			AcquireParameters ap = null;
			AssaySelector.MeasurementOption mo = (AssaySelector.MeasurementOption)N.App.Config.Cur.AssayType;
			Integ.GetCurrentAcquireDetectorPair(ref ap, ref det);
			INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);

			N.App.DB.ReplaceAcquireParams(sel, ap); // add new or replace existing with new

			// The acquire is set to occur, build up the measurement state 
			Integ.BuildMeasurement(ap, det, mo);

		}
	}


}