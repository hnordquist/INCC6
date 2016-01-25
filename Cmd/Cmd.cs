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
	using NC = NCC.CentralizedState;

	class AppEntry
	{

		static void Main(string[] args)
		{
			new NC("INCC6 Console");

			NCCConfig.Config c = new NCCConfig.Config(); // gets DB params
			NC.App.LoadPersistenceConfig(c.DB); // loads up DB, sets global AppContext
			c.AfterDBSetup(NC.App.AppContext, args);  // apply the cmd line 
													  // check return bool and exit here on error
			bool initialized = NC.App.Initialize(c);
			if (!initialized)
				return;

			if (NC.App.Config.Cmd.ShowVersion && !NC.App.Config.Cmd.Showcfg)
			{
				NC.App.Config.Cmd.ShowVersionOnConsole(NC.App.Config, NC.App.Config.App.Verbose() == TraceEventType.Verbose);
				return;
			} else if (NC.App.Config.Cmd.Showhelp)
			{
				NC.App.Config.ShowHelp();
				return;
			} else if (NC.App.Config.Cmd.Showcfg)
			{
				NCCConfig.Config.ShowCfg(NC.App.Config, NC.App.Config.App.Verbose() == TraceEventType.Verbose);
				return;
			}

			LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);

			try
			{
				applog.TraceInformation("==== Starting " + DateTime.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " [Cmd] " + NC.App.Name + " " + NC.App.Config.VersionString);
				if (!string.IsNullOrEmpty(c.Cur.Detector) && !c.Cur.Detector.Equals("Default")) // command line set the value
					initialized = NCC.IntegrationHelpers.SetNewCurrentDetector(c.Cur.Detector, true);
				if (!initialized)
					goto frob;

                if (!string.IsNullOrEmpty(c.Cur.Material) && !c.Cur.Material.Equals("Pu")) // command line set the value
                    initialized = NCC.IntegrationHelpers.SetNewCurrentMaterial(c.Cur.Material, true);
                if (!initialized)
                    goto frob;

                if (!string.IsNullOrEmpty(c.Cur.ItemId)) // command line set the item value, use it to override the material and other acquire params
                    initialized = NCC.IntegrationHelpers.SetNewCurrentMaterial(c.Cur.Material, true);
                if (!initialized)
                    goto frob;

                BuildMeasurement();
				if (NC.App.Config.App.UsingFileInput)
				{
					if (NC.App.AppContext.AssayFromFiles)
						NC.App.Opstate.Action = NCC.NCCAction.Assay;
					else
						NC.App.Opstate.Action = NCC.NCCAction.File;

					// file processing for analysis and more
					FileControlBind filecontrol = new FileControlBind();

					filecontrol.SetupEventHandlers();

					filecontrol.StartAction();  // step into the code and Run run run!

					NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
				} else
				{
					// NEXT: one of Assay, HV or Discover NC.App.Opstate.Action = NCC.NCCAction.;
					// DAQ + prompt
					DAQControlBind daqcontrol = new DAQControlBind((MLMEmulation.IEmulatorDiversion)(new LMProcessor.NullEmulation())); //<- here

					daqcontrol.SetupTimerCallBacks();
					daqcontrol.SetupEventHandlers();

					DAQControlBind.ActivateDetector(NC.App.Opstate.Measurement.Detector);

					daqcontrol.StartAction(); // step into the code and Run run run!

					NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
				}
frob:
				;
			} catch (Exception e)
			{
				NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
				applog.TraceException(e, true);
				applog.EmitFatalErrorMsg();
			} finally
			{
				NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
				NC.App.Config.RetainChanges();
				applog.TraceInformation("==== Exiting " + DateTime.Now.ToString("MMM dd yyy HH:mm:ss.ff K") + " [Cmd] " + NC.App.Name + " . . .");
				NC.App.Loggers.Flush();  
                if (NC.App.AppContext.OpenResults) Process.Start(System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe"), LMLoggers.LognLM.CurrentLogFilePath);
            }
		}

		static void BuildMeasurement()
		{
			Detector det = null;
			AcquireParameters ap = null;
			AssaySelector.MeasurementOption mo = (AssaySelector.MeasurementOption)NC.App.Config.Cur.AssayType;
			Integ.GetCurrentAcquireDetectorPair(ref ap, ref det);
			INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);

			NC.App.DB.AcquireParametersMap().Add(sel, ap);  // it's a new one, not the existing one modified
			NC.App.DB.UpdateAcquireParams(ap, det.ListMode);

			// The acquire is set to occur, build up the measurement state 
			Integ.BuildMeasurement(ap, det, mo);

		}
	}


}