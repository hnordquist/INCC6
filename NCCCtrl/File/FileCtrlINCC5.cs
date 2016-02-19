/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Collections.Generic;
using AnalysisDefs;
using DetectorDefs;
using NCC;
using NCCReporter;
using NCCTransfer;
using Instr;
namespace NCCFile
{
    using NC = NCC.CentralizedState;
    public partial class FileCtrl : ActionEvents, IActionControl
    {
        // Acquire from Database and Manual data work the same from this point 
        void DBDataAssay()
        {
            NC.App.Opstate.ResetTimer(filegather, null, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            Measurement meas = NC.App.Opstate.Measurement;
            if (meas.AcquireState.run_count_time <= 0.0)
            {
                ctrllog.TraceEvent(LogLevels.Error, 441, "Count time is <= 0.");
            }
            else
            if (meas.Cycles.GetUseableCycleCount() < 1)
            {
                ctrllog.TraceEvent(LogLevels.Error, 440, "This measurement has no good cycles.");
            }
            else
            {
                //Let's use old measurement date...... We will deal with dups later.
                meas.MeasDate = meas.Cycles[0].DataSourceId.dt;
                meas.Persist();  // preserve the basic results record
                FireEvent(EventType.ActionInProgress, this);
                meas.Cycles.ResetStatus(meas.Detector.MultiplicityParams);  // set to None, CycleConditioning sets each cycle anew
                ComputeFromINCC5SRData(meas);
                FireEvent(EventType.ActionInProgress, this);
            }

            NC.App.Opstate.ResetTokens();
            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
            FireEvent(EventType.ActionFinished, this);
        }



        void TestDataAssay()
        {
            List<string> exts = new List<string>() { ".dat", ".cnn" };
            FileList<TestDataFile> hdlr = new FileList<TestDataFile>();
            hdlr.Init(exts, ctrllog);
            FileList<TestDataFile> files = null;

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<TestDataFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, true);
            else
                files = (FileList<TestDataFile>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
            if (files == null || files.Count < 1)
            {
                NC.App.Opstate.StopTimer();
                NC.App.Opstate.StampOperationStopTime();
                FireEvent(EventType.ActionStop, this);
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "No usable Test data/Disk .dat or .cnn files found");
                return;
            }

            AcquireParameters orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
            Detector curdet = NC.App.Opstate.Measurement.Detector;
            AssaySelector.MeasurementOption mo = NC.App.Opstate.Measurement.MeasOption;

            // Each .dat file is a separate measurement 
            foreach (TestDataFile td in files)
            {
                Measurement meas = null;
                ResetMeasurement();

                try
                {
                    if (!td.OpenForReading())
                        continue;
                    if (NC.App.Opstate.IsQuitRequested)
                        break;
                    UInt32 run_seconds;

                    UInt16 number_good_runs = 0;
                    UInt16 total_number_runs = 0;
                    double run_count_time = 0;
                    double total_good_count_time = 0;

                    if (mo != AssaySelector.MeasurementOption.holdup)
                    {
                        /* number of good runs */
                        string l = td.reader.ReadLine();
                        UInt16.TryParse(l, out number_good_runs);
                        if (number_good_runs == 0)
                        {
                            ctrllog.TraceEvent(LogLevels.Error, 440, "This measurement has no good cycles.");
                            continue;
                        }
                        /* run count time */
                        l = td.reader.ReadLine();
                        Double.TryParse(l, out run_count_time);
                        if (run_count_time <= 0.0)
                        {
                            ctrllog.TraceEvent(LogLevels.Error, 441, "Count time is <= 0.");
                            continue;
                        }
                    }

                    // update acq and then meas here
                    AcquireParameters newacq = ConfigureAcquireState(curdet, orig_acq, DateTimeOffset.Now, number_good_runs, td.Filename);
                    IntegrationHelpers.BuildMeasurement(newacq, curdet, mo);
                    meas = NC.App.Opstate.Measurement;
                    meas.MeasDate = newacq.MeasDateTime;
                    meas.Persist();  // preserve the basic results record

                    /* convert to total count time */
                    total_number_runs = number_good_runs;
                    total_good_count_time = (double)number_good_runs *
                        run_count_time;
                    meas.RequestedRepetitions = total_number_runs;

                    // update the acq status and do the persist

                    /* read in run data */
                    for (int i = 0; i < number_good_runs; i++)
                    {
                        /* run date and time (IAEA format) */
                        run_seconds = (UInt32)(i * (UInt16)run_count_time); // from start time
                        AddTestDataCycle(i, run_seconds, run_count_time, meas, td);
                        if (i % 8 == 0)
                            FireEvent(EventType.ActionInProgress, this);
                    }
                    FireEvent(EventType.ActionInProgress, this);

                    if (meas.MeasOption == AssaySelector.MeasurementOption.verification &&
                        meas.INCCAnalysisState.Methods.Has(AnalysisMethod.AddASource) &&
                        meas.AcquireState.well_config == WellConfiguration.Passive)
                    {
                        AddASourceSetup aass = IntegrationHelpers.GetCurrentAASSParams(meas.Detector);
                        for (int n = 1; n <= aass.number_positions; n++)
                        {
                            /* number of good runs */
                            string l = td.reader.ReadLine();
                            if (td.reader.EndOfStream)
                            {
                                ctrllog.TraceEvent(LogLevels.Error, 440, "No add-a-source data found in disk file. " + "AAS p" + n.ToString());
                            }
                            UInt16.TryParse(l, out number_good_runs);
                            if (number_good_runs == 0)
                            {
                                ctrllog.TraceEvent(LogLevels.Error, 440, "This measurement has no good cycles. " + "AAS p" + n.ToString());
                                continue;
                            }
                            /* run count time */
                            l = td.reader.ReadLine();
                            Double.TryParse(l, out run_count_time);
                            if (run_count_time <= 0.0)
                            {
                                ctrllog.TraceEvent(LogLevels.Error, 441, "Count time is <= 0. " + "AAS p" + n.ToString());
                                continue;
                            }
                            /* read in run data */
                            for (int i = 0; i < number_good_runs; i++)
                            {
                                /* run date and time (IAEA format) */
                                run_seconds = (UInt32)((n + 1) * (i + 1) * (UInt16)run_count_time); // from start time
                                AddTestDataCycle(i, run_seconds, run_count_time, meas, td, " AAS p" + n.ToString(), n);
                                if (i % 8 == 0)
                                    FireEvent(EventType.ActionInProgress, this);
                            }
                            FireEvent(EventType.ActionInProgress, this);
                        }
                    }
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
                    ctrllog.TraceEvent(LogLevels.Error, 437, "Test data file processing stopped with error: '" + e.Message + "'");
                }
                finally
                {
                    td.CloseReader();
                    NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
                ComputeFromINCC5SRData(meas);
                FireEvent(EventType.ActionInProgress, this);
            }

            NC.App.Opstate.ResetTokens();
            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);
        }

        AcquireParameters ConfigureAcquireState(Detector det, AcquireParameters def, DateTimeOffset dto, ushort runs, string path)
        {
            AcquireParameters acq = NC.App.DB.LastAcquireFor(det, def.item_type);
            if (acq == null)
                acq = new AcquireParameters(def);
            acq.MeasDateTime = dto; acq.lm.TimeStamp = dto;
            acq.detector_id = string.Copy(det.Id.DetectorId);
            acq.meas_detector_id = string.Copy(acq.detector_id);
            acq.num_runs = runs;
            int tx = def.comment.IndexOf(" (Original file name");
            if (tx >= 0)
                acq.comment = def.comment.Substring(0, tx);
            else
                acq.comment = def.comment;
            acq.comment += " (Original file name " + System.IO.Path.GetFileName(path) + ")";
            INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, dto);
            NC.App.DB.ReplaceAcquireParams(sel, acq);             // only one allowed, same for actual measurement
            return acq;
        }

        void AddTestDataCycle(int run, uint run_seconds, double run_count_time, Measurement meas, TestDataFile td, string pivot = "", int cfindex = -1)
        {
            Cycle cycle = new Cycle(datalog);
            try
            {
                cycle.UpdateDataSourceId(ConstructedSource.CycleFile, meas.Detector.Id.SRType,
                        td.DTO.AddSeconds(run_seconds), td.Filename);
                cycle.seq = run;
                cycle.TS = TimeSpan.FromSeconds(run_count_time);  // dev note: check if this is always only in seconds, or fractions of a second
                                                                  // hn -- 9/4/14 -- not integer for count time.  Convert from double seconds here.
                                                                  // Joe still has force to int.  bleck!

                /* init run tests */
                cycle.SetQCStatus(meas.Detector.MultiplicityParams, QCTestStatus.None); // multmult creates entry if not found
                meas.Add(cycle, cfindex);
                /* singles, reals + accidentals, accidentals */
                string l = td.reader.ReadLine();
                string[] zorks = l.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                double[] v = new double[5];
                for (int z = 0; z < 5; z++)
                {
                    double d;
                    bool b = Double.TryParse(zorks[z], out d);
                    if (b)
                        v[z] = d;
                }
                cycle.Totals = (ulong)v[0];
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(meas.Detector.MultiplicityParams.FA, cycle.seq); // multmult
                cycle.CountingAnalysisResults.Add(meas.Detector.MultiplicityParams, mcr);  // multmult
                mcr.AB.TransferIntermediates(meas.Detector.AB);  // copy alpha beta onto the cycle's results 
                mcr.Totals = cycle.Totals;
                mcr.TS = cycle.TS;
                mcr.ASum = v[4];
                mcr.RASum = v[3];
                mcr.Scaler1.v = v[1];
                mcr.Scaler2.v = v[2];
                cycle.SinglesRate = v[0] / run_count_time;

                // assign the hits to a single channel (0)
                cycle.HitsPerChannel[0] = cycle.Totals;

                mcr.RawSinglesRate.v = cycle.SinglesRate;

                /* number of multiplicity values */
                string mv = td.reader.ReadLine();
                UInt16 k = 0;
                UInt16.TryParse(mv, out k);
                if (k == 0)  // test data files require an entry with 1 bin set 0s for the absence of multiplicity, go figure
                {
                    ctrllog.TraceEvent(LogLevels.Error, 440, "This" + pivot + " cycle " + run.ToString() + " has no good multiplicity data.");
                    return;
                }
                mcr.MinBins = mcr.MaxBins = k;
                mcr.RAMult = new ulong[k];
                mcr.NormedAMult = new ulong[k];
                mcr.UnAMult = new ulong[k]; // todo: compute this
                /* multiplicity values */
                for (UInt16 j = 0; j < k; j++)
                {
                    string ra = td.reader.ReadLine();
                    string[] blorks = ra.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    double[] ve = new double[2];
                    for (int z = 0; z < 2; z++)
                    {
                        double d;
                        bool b = Double.TryParse(blorks[z], out d);
                        if (b)
                            ve[z] = d;
                    }
                    mcr.RAMult[j] = (ulong)ve[0];
                    mcr.NormedAMult[j] = (ulong)ve[1];
                }
                ctrllog.TraceEvent(LogLevels.Verbose, 5439, "Cycle " + cycle.seq.ToString() + pivot + ((mcr.RAMult[0] + mcr.NormedAMult[0]) > 0 ? " max:" + mcr.MaxBins.ToString() : " *"));

            }
            catch (Exception e)
            {
                ctrllog.TraceEvent(LogLevels.Warning, 33085, pivot + "cycle processing error {0} {1} {2}", run, e.Message, pivot);
            }
        }

        static public string[] ProcessINCC5IniFile(LMLoggers.LognLM ctrllog)
		{
			string [] paths = new string[] { string.Empty };
			if (!NC.App.AppContext.UseINCC5Ini)
				return paths;
			string filename = System.IO.Path.Combine(NC.App.AppContext.INCC5IniLoc, "incc.ini");
			if (!System.IO.File.Exists(filename))
			{
				if (ctrllog != null)
					ctrllog.TraceEvent(LogLevels.Warning, 112, "Ooof! File does not exist or cannot be opened: " + filename);
				return paths;
			}
			string inputpath = string.Empty;
			using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
			{
				string line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine()) != null)
				{
					string tline = line.TrimStart();
					if (tline.StartsWith(@"//") || tline.StartsWith(@"#"))
						continue;
					if (tline.TrimStart().StartsWith("RT_COMMON_DATABASE_PATH")) // lolz
					{
						string[] otkens = line.Split();
						inputpath = otkens.Length > 1 ? otkens[1] : string.Empty;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(inputpath))
			{
                System.IO.DirectoryInfo parentpath = System.IO.Directory.GetParent(inputpath.TrimEnd(new char[] {'\\', '/'}));
				paths = new string[] { inputpath,   // as specified in the ini file entry
                                        System.IO.Path.Combine(System.IO.Path.GetFullPath("./"), @"Data"),  // todo: hard-coded path from examples in iRAP, but could be wrong
                                        System.IO.Path.Combine(parentpath.FullName, @"output") }; // log file goes into the final sweep output path
			}
			return paths;
		}

		/// <summary>
		/// Process one or more .NCC Review measurement data files, with related .NOP/.COP files.
		/// Presence of NOP/COP files is optional
		/// See Import Operator Declarations from Operator Review and Measurements from Radiation Review p. 24,
		/// See Operator Declaration File Format p. 87, 
		/// See Operator Declaration File Format for Curium Ratio Measurements p. 88, 
		/// See Radiation Review Measurement Data File Format p. 93, INCC Software Users Manual, March 29, 2009
		/// </summary>
		void INCCReviewFileProcessing()
        {

            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();
            // This code would be the same for an interactive version of this operation
            //  > Start here
            // first find and process and .NOP files 
            List<string> exts = new List<string>() { ".nop", ".cop" };
            FileList<CSVFile> hdlr = new FileList<CSVFile>();
            hdlr.Init(exts, ctrllog);
            FileList<CSVFile> files = null;
			OPFiles opfiles = new OPFiles();

            // The INCC5 semantics
            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<CSVFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, true);
            else
                files = (FileList<CSVFile>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
			if (files != null && files.Count > 0)
            {
				// construct lists of isotopics and items from the NOP and COP files
				opfiles.Process(files);
	            ctrllog.TraceEvent(LogLevels.Verbose, 33085, "NOP items " + opfiles.Results.ItemIds.Count);
            }
			else
	            ctrllog.TraceEvent(LogLevels.Warning, 33085, "No operator declarations available, continuing with default values");

            // process the NCC files only
            INCCFileOrFolderInfo foo = new INCCFileOrFolderInfo(ctrllog, "*.NCC");
            if (NC.App.AppContext.FileInputList == null)
                foo.SetPath(NC.App.AppContext.FileInput);
            else
                foo.SetFileList(NC.App.AppContext.FileInputList);
            List<INCCTransferBase> res = foo.Restore();
            // use RemoveAll to cull those NCC files that reference a non-existent detector
            DetectorList dl = NC.App.DB.Detectors;
            foreach (INCCReviewFile rf in res)
            {
                List<Detector> tdk = dl.FindAll(d => 0 == string.Compare(d.Id.DetectorName, rf.detector, true));
                if (tdk.Count < 1)
                {
                    rf.skip = true;
                    ctrllog.TraceEvent(LogLevels.Warning, 33085, "No detector " + rf.detector + " defined, cannot complete processing NCC review file " + System.IO.Path.GetFileName(rf.Path));
                }
            }
            res.RemoveAll(rf => (rf as INCCReviewFile).skip);
            res.Sort((rf1, rf2) => // sort chronologically
            {
                return DateTime.Compare((rf1 as INCCReviewFile).dt, (rf2 as INCCReviewFile).dt);
            });
            /// end here > The sorted, filtered and processed list here would be returned to the UI for display and interactive selection
            if (res == null || res.Count < 1)
            {
                NC.App.Opstate.StopTimer();
                NC.App.Opstate.StampOperationStopTime();
                FireEvent(EventType.ActionStop, this);
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "No usable NCC review files found in " + System.IO.Path.GetFullPath(foo.GetPath()));
                return;
            }

            AcquireParameters orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);

            // Each NCC file is a separate measurement 
            foreach (INCCReviewFile irf in res)
            {
                ResetMeasurement();
                if (NC.App.Opstate.IsQuitRequested)
                    break;
                // Find the detector named in the NCC file (existence shown in earlier processing above)
                Detector curdet = NC.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, irf.detector, true) == 0);

                // set up acquire state based on the NCC file content
                AcquireParameters newacq = ConfigureAcquireState(curdet, orig_acq, irf);

                AssaySelector.MeasurementOption mo = NCCMeasOption(irf);
                // make a temp MeasId
                MeasId thisone = new MeasId(mo, newacq.MeasDateTime);
                // get the list of measurement Ids for this detector
                List<MeasId> list = NC.App.DB.MeasurementIds(curdet.Id.DetectorName, mo.PrintName());
                MeasId already = list.Find(mid => mid.Equals(thisone));
                if (already != null)
                {
                    // URGENT: do the replacement as it says 
                    ctrllog.TraceEvent(LogLevels.Warning, 33085, "Replacing the matching {0} measurement, timestamp {1} (in a future release)", already.MeasOption.PrintName(), already.MeasDateTime.ToString());
                }

                IntegrationHelpers.BuildMeasurement(newacq, curdet, mo);
                Measurement meas = NC.App.Opstate.Measurement;
                meas.MeasDate = newacq.MeasDateTime; // use the date and time from the NCC file content      

                meas.Persist();  // preserve the basic results record
                if (AssaySelector.ForMass(meas.MeasOption) && !meas.INCCAnalysisState.Methods.AnySelected())
                    ctrllog.TraceEvent(LogLevels.Warning, 437, "No mass methods for " + meas.INCCAnalysisState.Methods.selector.ToString());

                try
                {

                    UInt16 total_number_runs = 0;
                    double run_count_time = 0;
                    double total_good_count_time = 0;

                    if (irf.num_runs == 0)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 440, "This measurement has no good cycles.");
                        continue;
                    }
                    if (meas.MeasOption == AssaySelector.MeasurementOption.holdup)
                        continue;

                    /* convert to total count time */
                    total_number_runs = irf.num_runs;
                    total_good_count_time = (double)irf.num_runs *
                        run_count_time;
                    meas.RequestedRepetitions = total_number_runs;

                    // convert runs to cycles
                    for (int i = 0; i < irf.num_runs; i++)
                    {
                        /* run date and time (IAEA format) */
                        AddReviewFileCycle(i, irf.runs[i], irf.times[i], meas, irf.Path);
                        if (i % 8 == 0)
                            FireEvent(EventType.ActionInProgress, this);
                    }
                    FireEvent(EventType.ActionInProgress, this);

                    // NEXT: handle true AAS cycles as in INCC5
                    if (meas.MeasOption == AssaySelector.MeasurementOption.verification &&
                        meas.INCCAnalysisState.Methods.Has(AnalysisMethod.AddASource) &&
                        meas.AcquireState.well_config == WellConfiguration.Passive)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 440, "No add-a-source data processed because the implementation is incomplete. " + "AAS ");
                        //AddASourceSetup aass = IntegrationHelpers.GetCurrentAASSParams(meas.Detector);
                        //for (int n = 1; n <= aass.number_positions; n++)
                        //{
                        //    /* number of good runs */
                        //    string l = td.reader.ReadLine();
                        //    if (td.reader.EndOfStream)
                        //    {
                        //        ctrllog.TraceEvent(LogLevels.Error, 440, "No add-a-source data found in disk file. " + "AAS p" + n.ToString());

                        //    }
                        //}
                    }
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
                    ctrllog.TraceEvent(LogLevels.Error, 437, "NCC file processing stopped with error: '" + e.Message + "'");
                }
                finally
                {
                    NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
                ComputeFromINCC5SRData(meas);
                FireEvent(EventType.ActionInProgress, this);
            }


            NC.App.Opstate.ResetTokens();
            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);
        }

        AcquireParameters ConfigureAcquireState(Detector det, AcquireParameters def, INCCReviewFile irf)
        {
            AcquireParameters acq = NC.App.DB.LastAcquireFor(det, def.item_type);
            if (acq == null)
                acq = new AcquireParameters(def);
            acq.MeasDateTime = irf.dt; acq.lm.TimeStamp = irf.dt;
            acq.num_runs = irf.num_runs; // RequestedRepetitions
            int tx = def.comment.IndexOf(" (Original file name");
            if (tx >= 0)
                acq.comment = def.comment.Substring(0, tx);
            else
                acq.comment = def.comment;
            acq.comment += " (Original file name " + System.IO.Path.GetFileName(irf.Path) + ")";
            acq.detector_id = string.Copy(det.Id.DetectorId);
            acq.meas_detector_id = string.Copy(acq.detector_id);

            // iid should have been added from the NOP processing above
            ItemId iid = NC.App.DB.ItemIds.Get(irf.item);
            if (iid == null)
            {
                ctrllog.TraceEvent(LogLevels.Warning, 5439, "Item id '" + irf.item + "' is referenced by the Review file measurement data, but is not found in op. rev. files or the database. Item type will default to " + acq.item_type);
            }
            else
            {
                acq.ApplyItemId(iid);
                INCCDB.Descriptor s = NC.App.DB.Stratums.Get(iid.stratum);
                INCCDB.Descriptor m = NC.App.DB.MBAs.Get(iid.mba);
                if (s != null)
                    acq.stratum_id = s;
                if (m != null)
                    acq.mba = m;
            }
            // todo: also need to account for iid UMass to mass condition

            INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, irf.dt);
            NC.App.DB.ReplaceAcquireParams(sel, acq);             // only one allowed, same for actual measurement
            return acq;
        }

        AssaySelector.MeasurementOption NCCMeasOption(INCCReviewFile irf)
        {
            // INCC5 semantics: ASCII vV 86 118, bB 66 98, nN 78 110
            AssaySelector.MeasurementOption mo = AssaySelector.MeasurementOption.unspecified;
            if (irf.meas_option == 118 || irf.meas_option == 86) mo = AssaySelector.MeasurementOption.verification;
            else if (irf.meas_option == 66 || irf.meas_option == 98) mo = AssaySelector.MeasurementOption.background;
            else if (irf.meas_option == 78 || irf.meas_option == 110) mo = AssaySelector.MeasurementOption.normalization;
            return mo;
        }

        unsafe void AddReviewFileCycle(int i, run_rec_ext run, INCCReviewFile.run_rec_ext_plus rrep, Measurement meas, string fn)
        {
            Cycle cycle = new Cycle(datalog);
            try
            {
                cycle.UpdateDataSourceId(ConstructedSource.ReviewFile, meas.Detector.Id.SRType,
                        rrep.dt, fn);
                cycle.seq = (run.run_number > 0 ? run.run_number : i); // INCC run record sequence numbers start at 1
                cycle.TS = TimeSpan.FromSeconds(run.run_count_time);

                /* init run tests */
                cycle.SetQCStatus(meas.Detector.MultiplicityParams, QCTestStatus.Pass, run.run_high_voltage); // multmult creates entry if not found
                meas.Add(cycle);
                /* singles, reals + accidentals, accidentals */
                cycle.Totals = (ulong)run.run_singles;
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(meas.Detector.MultiplicityParams.FA, cycle.seq); // multmult
                cycle.CountingAnalysisResults.Add(meas.Detector.MultiplicityParams, mcr); // multmult
                mcr.AB.TransferIntermediates(meas.Detector.AB);  // copy alpha beta onto the cycle's results 
                mcr.Totals = cycle.Totals;
                mcr.TS = cycle.TS;
                mcr.ASum = run.run_acc;
                mcr.RASum = run.run_reals_plus_acc;
                mcr.Scaler1.v = run.run_scaler1;
                mcr.Scaler2.v = run.run_scaler2;
                cycle.SinglesRate = run.run_singles / run.run_count_time;

                // assign the hits to a single channel (0)
                cycle.HitsPerChannel[0] = cycle.Totals;

                mcr.RawSinglesRate.v = cycle.SinglesRate;

                // now back-compute the actual limits of the bins
                for (int n = rrep.n_mult - 1; n >= 0; n--)
                {
                    if ((run.run_mult_reals_plus_acc[n] > 0.0) || (run.run_mult_acc[n] > 0.0))
                    {
                        mcr.MinBins = mcr.MaxBins = (ulong)(n + 1);
                        break;
                    }
                }
                mcr.RAMult = new ulong[mcr.MaxBins];
                mcr.NormedAMult = new ulong[mcr.MaxBins];
                mcr.UnAMult = new ulong[mcr.MaxBins]; // todo: compute this

                // copy the bin values, if any
                for (UInt16 j = 0; j < mcr.MaxBins; j++)
                {
                    mcr.RAMult[j] = (ulong)run.run_mult_reals_plus_acc[j];
                    mcr.NormedAMult[j] = (ulong)run.run_mult_acc[j];
                }
                ctrllog.TraceEvent(LogLevels.Verbose, 5439, "Cycle " + cycle.seq.ToString() + (rrep.n_mult > 0 ? " n_:" + rrep.n_mult.ToString() + " max:" + mcr.MaxBins.ToString() : " *"));
            }
            catch (Exception e)
            {
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "Cycle processing error {0} {1}", run, e.Message);
            }
        }


        static void ResetMeasurement()
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

        // assumes initalized measurement with at least one cycle, and at least one defined counting analysis result indexed by the detector's mult params
        // NEXT: implement for LM results 
        void ComputeFromINCC5SRData(Measurement m)
        {
            ctrllog.TraceEvent(LogLevels.Info, 34071, "Recomputing: '" + m.MeasurementId.MeasDateTime.ToString() + ", " + m.MeasOption.PrintName() + "'");
            if (m.Cycles.Count < 1)
            {
                ctrllog.TraceEvent(LogLevels.Error, 34830, "Skipping, no cycles on '" + m.MeasurementId.MeasDateTime.ToString() + ", " + m.MeasOption.PrintName() + "'");
                return;
            }

            if (m.CountingAnalysisResults.Count < 1 || !m.CountingAnalysisResults.HasMultiplicity)
            {
                ctrllog.TraceEvent(LogLevels.Error, 34831, "Bad match between detector and this file type." + (!m.CountingAnalysisResults.HasMultiplicity ? " No Multiplicity counter defined." : ""));
                return; //bad match between detector and this file type.
            }

            SRInstrument PseudoInstrument = new SRInstrument(m.Detector);
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list

            m.CurrentRepetition = 0;
            NC.App.Opstate.SOH = NCC.OperatingState.Living;

            try
            {
                // urgent: there is more work to do for the bins and AB here, AB can be copied from the detector values
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[m.Detector.MultiplicityParams]; // multmult
                // start counting using the per-cycle accumulation of summary results
                Array.Clear(mcr.RAMult, 0, mcr.RAMult.Length);
                Array.Clear(mcr.NormedAMult, 0, mcr.NormedAMult.Length);
                mcr.AB.TransferIntermediates(src: m.Detector.AB);

                foreach (AnalysisDefs.Cycle cycle in m.Cycles)
                {
                    if (NC.App.Opstate.IsQuitRequested)  // exit via abort or quit/save, follow-on code decides to continue with processing
                    {
                        ctrllog.TraceEvent(LogLevels.Warning, 430, "Cycle first-pass processing " + NC.App.Opstate.CancelStopAbortStateRep + " at sequence #" + cycle.seq + ", " + m.CurrentRepetition);
                        break;
                    }
                    m.CurrentRepetition++;
                    cycle.SetQCStatus(m.Detector.MultiplicityParams, QCTestStatus.Pass, cycle.HighVoltage);  // multmult prep for analyis one by one
                    CycleProcessing.ApplyTheCycleConditioningSteps(cycle, m);
                    m.CycleStatusTerminationCheck(cycle);
                    ctrllog.TraceEvent(LogLevels.Verbose, 5439, "Cycle " + cycle.seq.ToString());
                    if (m.CurrentRepetition % 8 == 0)
                        FireEvent(EventType.ActionInProgress, this);
                }
                FireEvent(EventType.ActionInProgress, this);
                // trim any None's that were not processed (occurs during a cancel/stop intervention)
                m.Cycles.Trim(m.Detector.MultiplicityParams); // multmult
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                ctrllog.TraceException(e, true);
                ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing stopped at cycle " + m.CurrentRepetition);
            }
            finally
            {
                NC.App.Loggers.Flush();
            }

            if (!NC.App.Opstate.IsAbortRequested)  // stop/quit means continue with what is available
            { 
                if (m.HasReportableData)
				{
					m.CalculateMeasurementResults();
					new ReportMangler(ctrllog).GenerateReports(m);
					m.SaveMeasurementResults();
				}
            }
            NC.App.Opstate.ResetTokens();
            Instruments.All.Remove(PseudoInstrument);
        }


    }
}