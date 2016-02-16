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
using System.Linq;
using System.Threading;
using Analysis;
using AnalysisDefs;
using DetectorDefs;
using NCC;
using NCCReporter;
using NCCTransfer;
using Instr;
namespace NCCFile
{

    using NC = NCC.CentralizedState;

    public partial class FileCtrl : ActionEvents, IActionControl//, IActionStatus
    {

        protected LMLoggers.LognLM ctrllog;
        protected LMLoggers.LognLM datalog;
        public Instrument PseudoInstrument;
        public bool gui;

        CountdownEvent _completed;
        public CountdownEvent Completed
		{
			get { return _completed; }
		}

        public FileCtrl(bool usingGui)
        {
            ctrllog = NC.App.Loggers.Logger(LMLoggers.AppSection.Control);
            datalog = NC.App.Loggers.Logger(LMLoggers.AppSection.Data);
            NC.App.Opstate.SOH = NCC.OperatingState.Starting;
            gui = usingGui;
        }

        public void Run()
        {
            try
            {
				_completed = new CountdownEvent(1);
                switch (NC.App.Opstate.Action)
                {
                    case NCCAction.Assay:
                        if (!gui)
                        {
                            Console.CancelKeyPress += new ConsoleCancelEventHandler(FileProcessingCtrlCHandler);
                            Console.TreatControlCAsInput = false;
                        }
                        //FireEvent(EventType.PreAction, this);
                        if (NC.App.AppContext.PulseFileAssay)
                            SortedPulseFileAssay();
                        else if (NC.App.AppContext.PTRFileAssay)
                            PTRFilePairAssay();
                        else if (NC.App.AppContext.MCA527FileAssay)
                            MCA527FileAssay();
                        else if (NC.App.AppContext.TestDataFileAssay)
                            TestDataAssay();
                        else if (NC.App.AppContext.ReviewFileAssay)
                            INCCReviewFileProcessing();
                        else if (NC.App.AppContext.DBDataAssay)
                            DBDataAssay();
                        //else if (NC.App.AppContext.ManualDataAssay) // todo:
                        //    DBDataAssay();
                        else
                            NCDFileAssay();
                        break;
                    case NCCAction.File:
                        if (NC.App.AppContext.INCCXfer)
                            INCCTransferFileProcessing();
                        else if (NC.App.AppContext.SortPulseFile)
                            PulseFileSort();
                        else
							FireEvent(EventType.ActionFinished, this);
                        break;
                    case NCCAction.Nothing:
                        ctrllog.TraceInformation("Specify an action (e.g. '-assay:0') for file-based input processing");
                        break;
                    default:
                        ctrllog.TraceInformation("(" + NC.App.Opstate.Action.ToString() + ") Actions other than 'assay' and 'file' are unavailable for file-based input");
                        break;
                }
                NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
                NC.App.Loggers.Flush();
				_completed.Signal();
            }
            catch (Exception e)
            {
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                LMLoggers.LognLM applog = NC.App.Logger(LMLoggers.AppSection.App);
				_completed.Signal();
                applog.TraceException(e, true);
                applog.EmitFatalErrorMsg();
                FireEvent(EventType.ActionFinished, this);
            }
        }


        void Replay(Measurement m, ConstructedSource src)
        {
            ctrllog.TraceEvent(LogLevels.Info, 34071, "Replay this: '" + m.MeasurementId.MeasDateTime.ToString() + ", " + m.MeasOption.PrintName() + "'");
            m.AcquireState.comment += " replay";
            // todo: make sure assay type on measurement is not overridden by cmd line artifacts 
            NC.App.Opstate.Measurement = m;
            SRInstrument PseudoInstrument = new SRInstrument(m.Detector);  // psuedo LM until we can map from user or deduce from file content at run-time
            PseudoInstrument.id.source = ConstructedSource.INCCTransfer;
            // remove PseudoInstrument.id.SetSRType(PseudoInstrument.id.Type); // hack, the SR type should be pre-defined by an earlier import of a INCCInitialDataDetectorFile
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list

            m.CurrentRepetition = 0;
            NC.App.Opstate.SOH = NCC.OperatingState.Living;
            try
            {
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults.First().Value;
                for (int i = 0; i < mcr.RAMult.Length; i++)  // count again using the per-cycle accumulation of summary results
                    mcr.RAMult[i] = 0;
                for (int i = 0; i < mcr.NormedAMult.Length; i++)
                    mcr.NormedAMult[i] = 0;
                m.Detector.Id.source = src;
                // need to get alpha beta onto the summary too.
                mcr.AB.TransferIntermediates(m.Detector.AB);

                foreach (AnalysisDefs.Cycle cycle in m.Cycles)
                {
                    if (NC.App.Opstate.IsCancellationRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
                        break;
                    m.CurrentRepetition++;
                    CycleProcessing.ApplyTheCycleConditioningSteps(cycle, m);
                    m.CycleStatusTerminationCheck(cycle);
                }
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
            if (m.HasReportableData)  // todo: test replay 
			{
				m.CalculateMeasurementResults();

				ReportMangler rm = new ReportMangler(ctrllog);
				rm.GenerateReports(m);

				m.SaveMeasurementResults();
			}

            Instruments.All.Remove(PseudoInstrument);
        }

        void PassThru(Measurement m) // preserve existing measurement without re-computing it
        {
            ctrllog.TraceEvent(LogLevels.Info, 34072, "Preserve this: '" + m.MeasurementId.MeasDateTime.ToString() + ", " + m.MeasOption.PrintName() + "'");
            NC.App.Opstate.Measurement = m;
            m.AcquireState.comment += " pass-through";
            MultiplicityCountingRes mcr = (MultiplicityCountingRes)m.CountingAnalysisResults.First().Value;
            // need to get alpha beta onto the summary too.
            mcr.AB.TransferIntermediates(m.Detector.AB);

            // sum per-cycle channel hits
            m.CycleSummary(false);

            ReportMangler rm = new ReportMangler(ctrllog);
            rm.GenerateReports(m);

            m.PersistFileNames();
        }

        void INCCTransferFileProcessing()
        {
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            INCCFileOrFolderInfo foo = new INCCFileOrFolderInfo(ctrllog);
            if (NC.App.AppContext.FileInputList == null)
                foo.SetPath(NC.App.AppContext.FileInput);
            else
                foo.SetFileList(NC.App.AppContext.FileInputList);
            List<INCCTransferBase> res = foo.Restore();
            if (res != null)
            {
                // do the detectors and the methods first, then do the transfer files with the data and results
                res.Sort((itb1, itb2) =>
                {
                    if (itb1 is INCCInitialDataDetectorFile)
                        if (!(itb2 is INCCInitialDataDetectorFile))
                            return -1;
                        else
                            return 0;
                    if (itb1 is INCCInitialDataCalibrationFile)
                        if (itb2 is INCCInitialDataDetectorFile)
                            return 1;
                        else if (itb2 is INCCTransferFile)
                            return -1;
                        else
                            return 0;
                    if (itb1 is INCCTransferFile)
                        if (!(itb2 is INCCTransferFile))
                            return 1;
                        else
                            return 0;
                    return 0;
                });

                // the in-memory load now occurs 
                if (res.Count > 0)
                {
                    ctrllog.TraceInformation("=== Processing the intermediate transfer content");
                    int j = 1;
                    // when processing a detector, detector calib, and 1 or more transfer files in batch, the order is important.
                    bool modI = false, modC = false;
                    foreach (INCCTransferBase bar in res)
                    {
                        if (bar is INCCInitialDataDetectorFile)  // first in the list, define the detectors
                        {
                            INCCKnew k = new INCCKnew(ctrllog);
                            k.BuildDetector((INCCInitialDataDetectorFile)bar, j); j++;
                            modI = true;
                        }
                    }
                    if (modI) // detector files have these various related parameters, update the in-memory collection state and the database to reflect this
                    {
                        // DB create/update on all records
                        NC.App.DB.UpdateDetectors();
                        NC.App.DB.BackgroundParameters.SetMap();
                        NC.App.DB.UnattendedParameters.SetMap();  // created from scratch
                        NC.App.DB.NormParameters.SetMap();
                        NC.App.DB.AASSParameters.SetMap();
                        NC.App.DB.HVParameters.SetMap();
                    }
                    foreach (INCCTransferBase bar in res)
                    {
                        if (bar is INCCInitialDataCalibrationFile)   // second in the list, get the calib info attached to detectors
                        {
                            INCCKnew k = new INCCKnew(ctrllog);
                            k.BuildCalibration((INCCInitialDataCalibrationFile)bar, j); j++;
                            modC = true;
                        }
                    }
                    if (modC) // update the datbase with new material defs and method analysis parameters 
                    {
                        NC.App.DB.Materials.SetList(); NC.App.DB.Materials.Reset();   
                        NC.App.DB.UpdateAnalysisMethods();
                    }
                    foreach (INCCTransferBase bar in res)
                    {
                        if (bar is INCCTransferFile)   // third, after prep with detector data and calib, this is measurement data and results derived using detectors and thier calibration parameters.
                        {
                            INCCKnew k = new INCCKnew(ctrllog);
                            bool supercool = k.BuildMeasurement((INCCTransferFile)bar, j); j++;
                            if (!supercool)
                                continue;
                            if (NC.App.AppContext.Replay)
                                Replay(k.Meas, ConstructedSource.INCCTransfer);
                            else
                                PassThru(k.Meas);
                        }
                    }
                }

                NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
                NC.App.Opstate.StampOperationStopTime();
                FireEvent(EventType.ActionFinished, this);

            }
        }

        /// <summary>
        /// Process a list of NCD files
        /// </summary>
        void NCDFileAssay()
        {
            List<string> ext = new List<string>() { ".ncd" };
            FileList<NCDFile> hdlr = new FileList<NCDFile>();
            hdlr.Init(ext, ctrllog);
            FileList<NCDFile> files = null;

			Measurement meas = NC.App.Opstate.Measurement;
			if (!meas.Detector.ListMode)
			{
				ctrllog.TraceEvent(LogLevels.Warning, 430, "LMMM NCD data file processing requires a List Mode detector; '" + meas.Detector.ToString() + "' is not");
				return;
			}
            PseudoInstrument = new LMInstrument(meas.Detector);  // psuedo LM until we can map from user or deduce from file content at run-time
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list
            DataSourceIdentifier did = meas.Detector.Id;
            LMRawDataTransform rdt = (LMRawDataTransform)PseudoInstrument.RDT;
            rdt.SetLMState(((LMConnectionInfo)(PseudoInstrument.id.FullConnInfo)).NetComm, useRawBuff:true);

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            NC.App.Opstate.ResetTimer(neutronCountingPrep, 0, 170, (int)NC.App.AppContext.StatusTimerMilliseconds / 4);

            rdt.SetupCountingAnalyzerHandler(NC.App.Config, did.source.TimeBase(did.SRType),  // 1e-7 for LMMM/NCD
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Verbose, 439, "Neutron counting processing complete: '" + s + "'");
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 438, "Neutron counting processing stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 437, "Neutron counting processing [Block] stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        }
                    );


            meas.AcquireState.num_runs = (ushort)files.Count(); // RequestedRepetitions

            rdt.PrepareAndStartCountingAnalyzers(meas.AnalysisParams);
            NC.App.Opstate.StopTimer();

            FireEvent(EventType.ActionStart, this);
			ulong totalBuffersProcessed = 0;
			meas.CurrentRepetition = 0;
            NC.App.Opstate.ResetTimer(filerawprocessing, PseudoInstrument, 250, (int)NC.App.AppContext.StatusTimerMilliseconds);
            foreach (var ncd in files)
            {
                if (NC.App.Opstate.IsQuitRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
                    break;
                if (!ncd.OpenForReading())
                    continue;
                PseudoInstrument.PendingReset();
                if (meas.CurrentRepetition == 0)
                {
                    meas.MeasDate = ncd.DTO;
                    meas.Detector.Id.source = ConstructedSource.NCDFile;
                    PseudoInstrument.id.source = ConstructedSource.NCDFile;
                }

                Cycle cycle = new Cycle(ctrllog);
                cycle.UpdateDataSourceId(ConstructedSource.NCDFile, InstrType.LMMM /* revisit this, it could be from any source */,
                                         ncd.DTO, ncd.Filename);
                meas.Add(cycle);
                rdt.StartCycle(cycle);
                meas.CurrentRepetition++;
                rdt.NumProcessedRawDataBuffers = 0;
                int thisread = 0;
                long read = 0, fulllen = ncd.stream.Length;
                NC.App.Opstate.SOH = NCC.OperatingState.Living;
                PseudoInstrument.id.FileName = ncd.Filename;

                ctrllog.TraceEvent(LogLevels.Info, 3335, "Assaying with {0}", ncd.Filename);

                try
                {
                    while (read < fulllen)
                    {
                        rdt.NumProcessedRawDataBuffers++;
                        // divide file size into discrete lengths of a reasonable size, say 50Mb or a 128 Mb default
                        int len = (fulllen > (int)rdt.CurEventBuffLen ? (int)rdt.CurEventBuffLen : (int)fulllen);

                        thisread = ncd.reader.Read(rdt.RawDataBuff, 0, len);
                        read += thisread;

                        ctrllog.TraceEvent(LogLevels.Verbose, 410, "Processing buffer {0} of {1} bytes", rdt.NumProcessedRawDataBuffers, len);

                        // push the bytes through the pipeline
                        StreamStatusBlock res = rdt.PassBufferToTheCounters(thisread);
                        if (res != null)
                        {
                            rdt.ParseStatusBlock(res, cycle);
                            // assert read >= fullen here, because we found a valid status block at the end of the file
                            ctrllog.TraceEvent(LogLevels.Verbose, 412, "End of stream, status message at byte {0}, len {1}", res.index, res.msglen);
                        }
						totalBuffersProcessed++;
                        rdt.StartNewBuffer();

                        ctrllog.TraceEvent(LogLevels.Verbose, 411, "[{0}] Counted {1} triggers, {2} hits, over {3} secs", rdt.NumProcessedRawDataBuffers, cycle.TotalEvents, cycle.Totals, cycle.TS.TotalSeconds);
                        NC.App.Loggers.Flush();

                        if (NC.App.Opstate.IsQuitRequested)  // cancellation in between buffers
                        {
                            ctrllog.TraceEvent(LogLevels.Warning, 428, "Processing cancelled, stopped at " + BufferStateSnapshot(PseudoInstrument));
                            break;
                        }

                    }
                }
                catch (FatalNeutronCountingException e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e);
                    ctrllog.TraceEvent(LogLevels.Warning, 429, "Neutron counting incomplete: {0}, processing stopped at {1}", e.Message, BufferStateSnapshot(PseudoInstrument));
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
					ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing stopped at cycle " + BufferStateSnapshot(PseudoInstrument));
                }
                finally
                {
                    ncd.CloseReader();
                    if (meas.CurrentRepetition == 1)// this is the first file, create the results before they get used 
                    {
                        meas.PrepareINCCResults();
                        meas.Persist();
                    }
                    rdt.EndOfCycleProcessing(meas);
                    NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
            } // loop over each NCD file

            PseudoInstrument.selected = false;
            rdt.EndOfCycleProcessing(meas, last: true);

            FireEvent(EventType.ActionInProgress, this);

            NC.App.Opstate.StopTimer();
            if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
            {
                if (meas.HasReportableData && totalBuffersProcessed > 0) // todo: test 
                {
					// if we have more than one cycle (one per file), and the cycles are combined into a 'measurement', then do the meta-processing across the results cycle list here
					NC.App.Opstate.ResetTimer(postprocessing, meas, 50, (int)NC.App.AppContext.StatusTimerMilliseconds);

					meas.CalculateMeasurementResults();

					NC.App.Opstate.StopTimer();
					FireEvent(EventType.ActionInProgress, this);

					ReportMangler rm = new ReportMangler(ctrllog);
					rm.GenerateReports(meas);

					meas.SaveMeasurementResults();
                }
            }

            NC.App.Opstate.ResetTokens();
            Instruments.All.Remove(PseudoInstrument);

            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);

        }


		protected void PulseFileSort()
		{

			//uint eventBufferLength = 50 * 1024 * 1024;

			FileList<UnsortedPulseFile> hdlr = new FileList<UnsortedPulseFile>();
			hdlr.Init(UnsortedPulseFile.ExtensionList, ctrllog);

			// get the list of files from the named folder
			FileList<UnsortedPulseFile> files = (FileList<UnsortedPulseFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, false);
			if (files == null || files.Count() < 1)
			{
				return;
			}

			foreach (var pf in files)
			{
				if (NC.App.Opstate.IsQuitRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
					break;

				string derivedDataFilenamePrefix = pf.GenerateDerivedName();
				ctrllog.TraceEvent(LogLevels.Verbose, 3330, "Sorting {0} to {1}", pf.Filename, derivedDataFilenamePrefix + ".sorted");

				var opt = new ExternalMergeSort.Options(ExternalMergeSort.sizeFromMB(50), ExternalMergeSort.sizeFromMB(10 /*0*/ /*10*/) / 10, 1024 * 10);
				opt.SkipInitialSort = false; // a single file only, so no external merge at this time, save that feature for later
				opt.RemoveIntermediateFiles = true; // no merge here, we are breaking up a single pulse file into n temp files, so we need to remove the n temp files
				ExternalMergeSort.Logger = pf.Log;
				ExternalMergeSort.Sort(pf.Filename, derivedDataFilenamePrefix + ".sorted", NC.App.Config.RootLoc, opt);

			} // loop over each file
			FireEvent(EventType.ActionFinished, this);

		}

		internal static NCDFile PrepNCDFile(string opath, NeutronDataFile spf, int idx)
        {
            NCDFile file = new NCDFile();
            file.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Data);

            file.Filename = opath + "\\stub";  // will change to ncd in the next steps
            file.DTO = new DateTimeOffset(spf.DTO.Ticks, spf.DTO.Offset);
            string currentDataFilenamePrefix = file.GeneratePathWithDTPrefix();
            file.GenName(currentDataFilenamePrefix, idx); // dev note: good to provide external config approach to specify output file pattern 
            bool ok = file.CreateForWriting();

            return ok ? file : null;
        }


        protected void PTRFilePairAssay()
        {

            List<string> ext = new List<string>() { ".chn", ".bin" };
            FileList<PTRFilePair> hdlr = new FileList<PTRFilePair>();
            FileList<PTRFilePair> files = null;
            hdlr.Init(ext, datalog);

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            // get the list of files from the named folder, or use the supplied list
            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<PTRFilePair>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, false);
            else
                files = (FileList<PTRFilePair>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
            if (files == null || files.Count() < 1)
            {
                NC.App.Opstate.StopTimer();
                return;
            }
            int removed = files.RemoveAll(f => f.PairEntryFileExtension(".chn"));
            if (files.Count() < 1)
            {
                NC.App.Opstate.StopTimer();
                return;
            }

			Measurement meas = NC.App.Opstate.Measurement;
			if (!meas.Detector.ListMode)
			{
				ctrllog.TraceEvent(LogLevels.Warning, 430, "PTR-32 data file processing a List Mode detector; '" + meas.Detector.ToString() + "' is not");
				return;
			}
            PseudoInstrument = new Instr.LMInstrument(meas.Detector);  // psuedo LM until we can map from user or deduce from file content at run-time
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list

            // Force RDT.State to be a LM ptrFile file RDT, this shows a design failure, so need to rework the entire scheme, (like there is still time . . .)
            LMRawDataTransform rdt = (PseudoInstrument as Instr.LMInstrument).RDT;
            rdt.SetLMState(((LMConnectionInfo)(PseudoInstrument.id.FullConnInfo)).NetComm, 4); // 4 bytes per event, contrast with 8 bytes contiguous for LMMM
            PTRFileProcessingState c = new PTRFileProcessingState(rdt.State.maxValuesInBuffer, (LMProcessingState)PseudoInstrument.RDT.State);
            PseudoInstrument.RDT.State = c;
            rdt.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));

            NC.App.Opstate.ResetTimer(neutronCountingPrep, 0, 170, (int)NC.App.AppContext.StatusTimerMilliseconds / 4);
            DataSourceIdentifier did = meas.Detector.Id;
            rdt.SetupCountingAnalyzerHandler(NC.App.Config, did.source.TimeBase(did.SRType), // 1e-8 expected here
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Verbose, 439, "Neutron counting processing complete: '" + s + "'");
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 438, "Neutron counting processing stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 437, "Neutron counting processing [Block] stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        }
                    );


            meas.AcquireState.num_runs = (ushort)files.Count(); // RequestedRepetitions

            rdt.PrepareAndStartCountingAnalyzers(meas.AnalysisParams);
            NC.App.Opstate.StopTimer();

            FireEvent(EventType.ActionStart, this);

            meas.CurrentRepetition = 0;
            NC.App.Opstate.ResetTimer(filerawprocessing, PseudoInstrument, 250, (int)NC.App.AppContext.StatusTimerMilliseconds);
			ulong totalBuffersProcessed = 0;
            PTRFileProcessingState pps = PseudoInstrument.RDT.State as PTRFileProcessingState;
            foreach (var ptrFile in files)
            {
                if (NC.App.Opstate.IsQuitRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
                    break;
                if (!ptrFile.OpenForReading())
                    continue;
                PseudoInstrument.PendingReset();
                if (meas.CurrentRepetition == 0)
                {
                    meas.MeasDate = new DateTimeOffset(ptrFile.DTO.Ticks, ptrFile.DTO.Offset);
                    meas.Detector.Id.source = ConstructedSource.PTRFile;
                    PseudoInstrument.id.source = ConstructedSource.PTRFile;
                }

                Cycle cycle = new Cycle(ctrllog);
                cycle.UpdateDataSourceId(ConstructedSource.PTRFile, InstrType.PTR32 /* revisit this, it could be from any source */,
                                         ptrFile.DTO, ptrFile.Filename);
                meas.Add(cycle);
                rdt.StartCycle(cycle);
                meas.CurrentRepetition++;
                pps.Reset();
                NC.App.Opstate.SOH = NCC.OperatingState.Living;
                PseudoInstrument.id.FileName = ptrFile.Filename;

                ctrllog.TraceEvent(LogLevels.Info, 3335, "Assaying with {0}", ptrFile.Filename);
                /// unique here
                UInt32 deltaTime = 0;
                Double ShakeTime; // like pulse files 10^e-8

                int maxValuesInBuffer = (int)pps.maxValuesInBuffer;
                try
                {

                    UInt32[] chnmask = new UInt32[32];
                    for (int i = 0; i < 32; i++)
                    {
                        chnmask[i] = (uint)1 << i;
                    }

                    rdt.NumProcessedRawDataBuffers = 0;
                    string issue = string.Empty;

                    //read the header from the BIN file
                    ptrFile.Events.ReadHeader();
                    ShakeTime = 0;
					cycle.TS = TimeSpan.FromSeconds(ptrFile.Events.ReportedCountTimeSecs);  // requested or specified time in seconds
					ctrllog.TraceEvent(LogLevels.Info, 3335, "The reported assay interval is {0} seconds", cycle.TS.TotalSeconds);
                    //Add this as check. We should have this count - big T as final count rate
                    pps.PTRReportedCountTime += ptrFile.Events.ReportedCountTimeSecs;
                    if (!ptrFile.Channels.Active)
                    {
                        ptrFile.Channels.fulllen = ptrFile.Events.stream.Length;
                    }
                    while (ptrFile.Channels.read < ptrFile.Channels.fulllen && string.IsNullOrEmpty(issue))
                    {
                        rdt.NumProcessedRawDataBuffers++;

                        // divide file size into discrete lengths of a reasonable size, say 50Mb or a 128 Mb default
                        int elen = (ptrFile.Events.eventsectionlen > pps.eventBufferLength ? (int)pps.eventBufferLength : (int)ptrFile.Events.eventsectionlen);
                        int clen = (ptrFile.Channels.fulllen > maxValuesInBuffer ? maxValuesInBuffer : (int)ptrFile.Channels.fulllen);

                        // read the times up to the buffer limit
                        int events = Math.Min(maxValuesInBuffer, elen / sizeof(UInt32));
                        events = Math.Min(events, ptrFile.Events.EventsYetToRead());  // might be at the last buffers-worth, check and constrain
                        ptrFile.Events.thisread = ptrFile.Events.ReadUInt32Array(pps.timeInBuffer, 0, events);
                        ptrFile.Events.read += ptrFile.Events.thisread;

                        // read or simulate the channel bytes
                        if (ptrFile.Channels.Active)
                        {
                            ptrFile.Channels.thisread = ptrFile.Channels.reader.Read(pps.chnInBuffer, 0, clen);
                            ptrFile.Channels.read += ptrFile.Channels.thisread;
                        }
                        else  // fake it with a single channel, an empty channel mask means channel 1
                        {
                            ptrFile.Channels.read += clen;
                        }

                        // Todo: fix this because could be or'ing over the buffer end, so this will blow
                        int edi = -1;  // actual events
                        for (int ec = 0; ec < events; ec++) // raw event counts
                        {
                            deltaTime = pps.timeInBuffer[ec];
                            if (deltaTime == 0) // or the channel mask                            
                            {
                                pps.channels[edi] |= chnmask[pps.chnInBuffer[ec]];
                                ptrFile.Log.TraceEvent(LogLevels.Verbose, 3246, "multiple hits {0:x8} at {1} ({2} {3})", pps.channels[edi], ShakeTime, edi, ec);
                                continue;
                            }

                            edi++;  // the next event
                            ShakeTime += deltaTime; // accumulate total time

                            //set the neutron event with the new neutron
                            pps.channels[edi] = chnmask[pps.chnInBuffer[ec]];
                            //set the event time with the new neutron time
                            pps.times[edi] = ShakeTime;

                            if (pps.channels[edi] == 0)
                                ptrFile.Log.TraceEvent(LogLevels.Warning, 3334, "0 event at {0}", ShakeTime);

                            // ptrFile.Log.TraceEvent(LogLevels.Verbose, 777, "{0} {1} [{2:x8}]", ae, pps.times[ae], pps.channels[ae]);
                        }
                        int counted = edi + 1;
                        ptrFile.Log.TraceEvent(LogLevels.Verbose, 3336, "constructed buffer {0} of {1} hits for {2} neutrons", rdt.NumProcessedRawDataBuffers, counted, events);

                        // push the time doubles through the pipeline
                        StreamStatusBlock ssb = rdt.PassBufferToTheCounters(counted);
                        if (ssb != null)
                        {
                            rdt.ParseStatusBlock(ssb, cycle);
                            ctrllog.TraceEvent(LogLevels.Verbose, 412, "End of stream, status message at byte {0}, len {1}", ssb.index, ssb.msglen);
                        }
						totalBuffersProcessed++;
                        rdt.StartNewBuffer();

                        ctrllog.TraceEvent(LogLevels.Verbose, 411, "[{0}] Counted {1} triggers, {2} hits, over {3} secs", rdt.NumProcessedRawDataBuffers, cycle.TotalEvents, cycle.Totals, cycle.TS.TotalSeconds);
                        NC.App.Loggers.Flush();

                        if (NC.App.Opstate.IsQuitRequested)  // cancellation in between buffers
                        {
                            ctrllog.TraceEvent(LogLevels.Warning, 428, "Processing cancelled, stopped at " + BufferStateSnapshot(PseudoInstrument));
                            break;
                        }
                    }
                }
                catch (FatalNeutronCountingException e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e);
                    ctrllog.TraceEvent(LogLevels.Warning, 429, "Neutron counting incomplete: {0}, processing stopped at {1}", e.Message, BufferStateSnapshot(PseudoInstrument));
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
					ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing stopped at cycle " + BufferStateSnapshot(PseudoInstrument));
                }
                finally
                {
                    ptrFile.CloseReader();
                    if (meas.CurrentRepetition == 1)// this is the first file, create the results before they get used 
                    {
                        meas.PrepareINCCResults();
                        meas.Persist();
                    }
                    rdt.EndOfCycleProcessing(meas);
                    rdt.FlushCycleSummaryResults();
                    NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
            } // loop over each PTR-32 file (pair)
            rdt.EndOfCycleProcessing(meas, last:true);

            PseudoInstrument.selected = false;

            FireEvent(EventType.ActionInProgress, this);

            NC.App.Opstate.StopTimer();

            if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
            { 
                if (meas.HasReportableData && totalBuffersProcessed > 0) // todo: test 
                {
					// if we have more than one cycle (one per file), and the cycles are combined into a 'measurement', then do the meta-processing across the results cycle list here
					NC.App.Opstate.ResetTimer(postprocessing, meas, 50, (int)NC.App.AppContext.StatusTimerMilliseconds);

					meas.CalculateMeasurementResults();

					NC.App.Opstate.StopTimer();
					FireEvent(EventType.ActionInProgress, this);

					ReportMangler rm = new ReportMangler(ctrllog);
					rm.GenerateReports(meas);

					meas.SaveMeasurementResults();
				}
            }

            NC.App.Opstate.ResetTokens();
            Instruments.All.Remove(PseudoInstrument);

            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);

        }

        protected void MCA527FileAssay()
        {

            FileList<MCAFile> hdlr = new FileList<MCAFile>();
            FileList<MCAFile> files = null;
            hdlr.Init( new List<string>() { ".mca" }, datalog);

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            // get the list of files from the named folder, or use the supplied list
            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<MCAFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, false);
            else
                files = (FileList<MCAFile>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
            if (files == null || files.Count() < 1)
            {
                NC.App.Opstate.StopTimer();
                return;
            }

			Measurement meas = NC.App.Opstate.Measurement;
			if (!meas.Detector.ListMode)
			{
				ctrllog.TraceEvent(LogLevels.Warning, 430, "MCA-527 data file processing requires a List Mode detector; '" + meas.Detector.ToString() + "' is not");
				return;
			}
			PseudoInstrument = new Instr.LMInstrument(meas.Detector);  // psuedo LM until we can map from user or deduce from file content at run-time
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list

            // Force RDT.State to be a LM mcaFile file RDT, this shows a design failure, so need to rework the entire scheme
            LMRawDataTransform rdt = (PseudoInstrument as Instr.LMInstrument).RDT;
            rdt.SetLMState(((LMConnectionInfo)(PseudoInstrument.id.FullConnInfo)).NetComm, 4);
            MCA527FileProcessingState fps = new MCA527FileProcessingState(rdt.State.maxValuesInBuffer, (LMProcessingState)PseudoInstrument.RDT.State);
            PseudoInstrument.RDT.State = fps;
            rdt.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));

            NC.App.Opstate.ResetTimer(neutronCountingPrep, 0, 170, (int)NC.App.AppContext.StatusTimerMilliseconds / 4);
            DataSourceIdentifier did = meas.Detector.Id;
            rdt.SetupCountingAnalyzerHandler(NC.App.Config, did.source.TimeBase(did.SRType), // 1e-7 expected here, normally, but the actual value is found in each file, see mcaFile.TimeUnitNanoSec
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Verbose, 439, "Neutron counting processing complete: '" + s + "'");
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 438, "Neutron counting processing stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 437, "Neutron counting processing [Block] stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        }
                    );


            meas.AcquireState.num_runs = (ushort)files.Count(); // RequestedRepetitions

            rdt.PrepareAndStartCountingAnalyzers(meas.AnalysisParams);
            NC.App.Opstate.StopTimer();

            FireEvent(EventType.ActionStart, this);

            meas.CurrentRepetition = 0;
            NC.App.Opstate.ResetTimer(filerawprocessing, PseudoInstrument, 250, (int)NC.App.AppContext.StatusTimerMilliseconds);

			long newMeasId = 0;
			MCAFile[] afiles = files.ToArray();
			ulong totalBuffersProcessed = 0;
            for (int i = 0; i < afiles.Length; i++)
            {
                if (NC.App.Opstate.IsQuitRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
                    break;
				MCAFile mcaFile = afiles[i];
                if (!mcaFile.OpenForReading())
                    continue;
                PseudoInstrument.PendingReset();
                if (meas.CurrentRepetition == 0)
                {
                    meas.MeasDate = new DateTimeOffset(mcaFile.DTO.Ticks, mcaFile.DTO.Offset);
                    meas.Detector.Id.source = ConstructedSource.MCA527File;
                    PseudoInstrument.id.source = ConstructedSource.MCA527File;
                }

                Cycle cycle = new Cycle(ctrllog);
                cycle.UpdateDataSourceId(ConstructedSource.MCA527File, InstrType.MCA527 /* revisit this, it could be from any source */,
                                         mcaFile.DTO, mcaFile.Filename);
                meas.Add(cycle);
                rdt.StartCycle(cycle);
                meas.CurrentRepetition++;
                fps.Reset();
                NC.App.Opstate.SOH = NCC.OperatingState.Living;
                PseudoInstrument.id.FileName = mcaFile.Filename;

                ctrllog.TraceEvent(LogLevels.Info, 3335, "Assaying with {0}", mcaFile.Filename);
                /// unique here
                ulong ShakeTime = 0, prevBuffLastShakeTime = 0; // like ptr32 files 10^e-8

                int maxValuesInBuffer = (int)fps.maxValuesInBuffer;
                try
                {

                    rdt.NumProcessedRawDataBuffers = 0;
                    string issue = string.Empty;
                    int tbindex = 0; // event count

                    // read the two headers from the MCA file, can throw exceptions that cause skipping
                    mcaFile.ReadHeader();
                    rdt.ResetTickSizeInSeconds(mcaFile.TimeUnitNanoSec / 1e9);
                    cycle.TS = TimeSpan.FromSeconds(mcaFile.MeasTime);  // requested or specified time in seconds
                    ctrllog.TraceEvent(LogLevels.Info, 3335, "The reported assay interval is {0} seconds", cycle.TS.TotalSeconds);
                    uint FBbytes = mcaFile.TotalBytes;
                    // read timestamps...
                    foreach (ulong deltaTime in mcaFile.EnumerateTimestamps())
                    {
                        ShakeTime += deltaTime; // accumulate total time
                                                // fill up the processing buffer with events, this is like the bulk read for the other file formats
                                                // read the times up to the buffer limit
                        if (tbindex < maxValuesInBuffer)
                        {
                            fps.timeInBuffer[tbindex++] = ShakeTime;
                            if (mcaFile.ReaderPosition > 0 && mcaFile.ReaderPosition < FBbytes)         // more bytes to read				
                                continue;
                        }
                        Console.WriteLine("total {0}; last delta {1}; event count {2}; file length time {3}; start {4}; seconds {5};", ShakeTime, deltaTime, tbindex, mcaFile.RealTime, mcaFile.StartTime, mcaFile.MeasTime);
                        if (NC.App.Opstate.IsQuitRequested)
						{
							NC.App.Opstate.SOH = NC.App.Opstate.Requested;
                            break;
						}
						rdt.NumProcessedRawDataBuffers++; 
                        // push the time deltas through the convertor code and then the counting analyzer threads
                        StreamStatusBlock ssb = rdt.PassBufferToTheCounters(tbindex);
                        totalBuffersProcessed++;
                        if (ssb != null)
                        {
                            rdt.ParseStatusBlock(ssb, cycle);
                            ctrllog.TraceEvent(LogLevels.Verbose, 412, "End of stream, status message at byte {0}, len {1}", ssb.index, ssb.msglen);
                        }

                        tbindex = 0;
                        rdt.StartNewBuffer();
                        prevBuffLastShakeTime = ShakeTime;
                        ctrllog.TraceEvent(LogLevels.Verbose, 411, "[{0}] Counted {1} triggers, {2} hits, over {3} secs", rdt.NumProcessedRawDataBuffers, cycle.TotalEvents, cycle.Totals, cycle.TS.TotalSeconds);
                        NC.App.Loggers.Flush();
                    }
                }
                catch (NotImplementedException e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Living;
					ctrllog.TraceException(e);
                    ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing skips this file: " + e.Message);
                }
                catch (FatalNeutronCountingException e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e);
                    ctrllog.TraceEvent(LogLevels.Warning, 429, "Neutron counting incomplete: {0}, processing stopped at {1}", e.Message, BufferStateSnapshot(PseudoInstrument));
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
                    ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing stopped at cycle " + BufferStateSnapshot(PseudoInstrument));
                }
                finally
                {
                    mcaFile.CloseReader();
                    rdt.EndOfCycleProcessing(meas);
                    rdt.FlushCycleSummaryResults();
					meas.StartSavingMeasurement(rdt.NumProcessedRawDataBuffers, ref newMeasId);
					NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
            } // loop over each MCA527file
            rdt.EndOfCycleProcessing(meas, last: true);

            PseudoInstrument.selected = false;

            FireEvent(EventType.ActionInProgress, this);

            NC.App.Opstate.StopTimer();

            if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
            {
                if (meas.HasReportableData && totalBuffersProcessed > 0) // todo: test 
                {
					// if we have more than one cycle (one per file), and the cycles are combined into a 'measurement', then do the meta-processing across the results cycle list here
					NC.App.Opstate.ResetTimer(postprocessing, meas, 50, (int)NC.App.AppContext.StatusTimerMilliseconds);

					meas.CalculateMeasurementResults();

					NC.App.Opstate.StopTimer();
					FireEvent(EventType.ActionInProgress, this);

					ReportMangler rm = new ReportMangler(ctrllog);
					rm.GenerateReports(meas);

					meas.SaveMeasurementResults();
				}
            }

            NC.App.Opstate.ResetTokens();
            Instruments.All.Remove(PseudoInstrument);

            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);

        }



		protected void SortedPulseFileAssay()
        {

            List<string> ext = new List<string>() { ".pulse.sorted", ".txt.sorted" };
            FileList<SortedPulseFile> hdlr = new FileList<SortedPulseFile>();
            hdlr.Init(ext, ctrllog);
            FileList<SortedPulseFile> files = null;

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            // get the list of files from the named folder
            files = (FileList<SortedPulseFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, true);
            if (files == null || files.Count() < 1)
            {
                NC.App.Opstate.StopTimer();
                return;
            }

			Measurement meas = NC.App.Opstate.Measurement;
			if (!meas.Detector.ListMode)
			{
				ctrllog.TraceEvent(LogLevels.Warning, 430, "Pulse data file processing requires a List Mode detector; '" + meas.Detector.ToString() + "' is not");
				return;
			}
            PseudoInstrument = new Instr.LMInstrument(meas.Detector);  // psuedo LM until we can map from user or deduce from file content at run-time
            PseudoInstrument.selected = true;
            if (!Instruments.Active.Contains(PseudoInstrument))
                Instruments.Active.Add(PseudoInstrument); // add to global runtime list

            // Force RDT.State To be a pulse file RDT, this shows a design failure, so need to rework the entire scheme
            LMRawDataTransform rdt = (PseudoInstrument as Instr.LMInstrument).RDT;
            PulseProcessingState c = new PulseProcessingState(rdt.State.maxValuesInBuffer);
            PseudoInstrument.RDT.State = null;
            PseudoInstrument.RDT.State = c;
            rdt.Init(NC.App.Loggers.Logger(LMLoggers.AppSection.Data), NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
            rdt.SetLMState(((LMConnectionInfo)(PseudoInstrument.id.FullConnInfo)).NetComm);

            NC.App.Opstate.ResetTimer(neutronCountingPrep, 0, 170, (int)NC.App.AppContext.StatusTimerMilliseconds / 4);
            DataSourceIdentifier did = meas.Detector.Id;
            rdt.SetupCountingAnalyzerHandler(NC.App.Config, did.source.TimeBase(did.SRType), // 1e-8 expected here
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Verbose, 439, "Neutron counting processing complete: '" + s + "'");
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 438, "Neutron counting processing stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        },
                        (string s) =>
                        {
                            PseudoInstrument.PendingComplete();
                            ctrllog.TraceEvent(LogLevels.Error, 437, "Neutron counting processing [Block] stopped with error: '" + s + "'");
                            rdt.EndAnalysisImmediately();
                            throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in file processing loop below
                        }
                    );


            meas.AcquireState.num_runs = (ushort)files.Count(); // RequestedRepetitions

            rdt.PrepareAndStartCountingAnalyzers(meas.AnalysisParams);
            NC.App.Opstate.StopTimer();

            FireEvent(EventType.ActionStart, this);

            meas.CurrentRepetition = 0;
            NC.App.Opstate.ResetTimer(filerawprocessing, PseudoInstrument, 250, (int)NC.App.AppContext.StatusTimerMilliseconds);

            Random rand = new Random();
            int a = rand.Next(0, 3);
            byte byt = (byte)(1 << (byte)rand.Next(1, 7));
 			ulong totalBuffersProcessed = 0;
            PulseProcessingState pps = PseudoInstrument.RDT.State as PulseProcessingState;
            foreach (var sortedpulse in files)
            {
                if (NC.App.Opstate.IsQuitRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
                    break;
                if (!sortedpulse.OpenForReading())
                    continue;
                PseudoInstrument.PendingReset();
                if (meas.CurrentRepetition == 0)
                {
                    meas.MeasDate = new DateTimeOffset(sortedpulse.DTO.Ticks, sortedpulse.DTO.Offset);
                    meas.Detector.Id.source = ConstructedSource.SortedPulseTextFile;
                    PseudoInstrument.id.source = ConstructedSource.SortedPulseTextFile;
                }

                Cycle cycle = new Cycle(ctrllog);
                cycle.UpdateDataSourceId(ConstructedSource.SortedPulseTextFile, InstrType.LMMM, // MCNPX is common source of pulse files, but we need an explicit LM type here /* revisit this, it could be from any source */,
                          sortedpulse.DTO, sortedpulse.Filename);
                meas.Add(cycle);
                rdt.StartCycle(cycle);
                meas.CurrentRepetition++;
                pps.Reset();
                NC.App.Opstate.SOH = NCC.OperatingState.Living;
                PseudoInstrument.id.FileName = sortedpulse.Filename;
                ctrllog.TraceEvent(LogLevels.Info, 3335, "Assaying with {0}", sortedpulse.Filename);
                try
                {
                    pps.chnbytes[a] = byt;  // use the same rand value for all these files, makes for consistent per-channel rates counting
                    rdt.NumProcessedRawDataBuffers = 0;
                    string issue = string.Empty;
                    while (!sortedpulse.reader.EndOfStream && string.IsNullOrEmpty(issue))
                    {
                        string s;
                        int rb = 0;
                        rdt.NumProcessedRawDataBuffers++;
                        do
                        {
                            s = sortedpulse.reader.ReadLine();
                            double res = 0;
                            if (Double.TryParse(s, out res))
                            {
                                pps.timeInBuffer[rb++] = res;
                            }
                        } while (!sortedpulse.reader.EndOfStream && rb < pps.maxValuesInBuffer);
                        // now transform the doubles list and assay at the same time

                        ctrllog.TraceEvent(LogLevels.Verbose, 410, "Processing buffer {0} of {1} times", rdt.NumProcessedRawDataBuffers, rb);

                        // push the time doubles through the pipeline
                        StreamStatusBlock ssb = rdt.PassBufferToTheCounters(rb);
                        if (ssb != null)
                        {
                            rdt.ParseStatusBlock(ssb, cycle);
                            // assert read >= fullen here, because we found a valid status block at the end of the file
                            ctrllog.TraceEvent(LogLevels.Verbose, 412, "End of stream, status message at byte {0}, len {1}", ssb.index, ssb.msglen);
                        }
						totalBuffersProcessed++;
                        rdt.StartNewBuffer();

                        ctrllog.TraceEvent(LogLevels.Verbose, 411, "[{0}] Counted {1} triggers, {2} hits, over {3} secs", rdt.NumProcessedRawDataBuffers, cycle.TotalEvents, cycle.Totals, cycle.TS.TotalSeconds);
                        NC.App.Loggers.Flush();

                        if (NC.App.Opstate.IsQuitRequested)  // cancellation in between buffers
                        {
                            ctrllog.TraceEvent(LogLevels.Warning, 428, "Processing cancelled, stopped at " + BufferStateSnapshot(PseudoInstrument));
                            break;
                        }

                    }
                    // Analysis.StreamStatusBlock sb = NCDFile.CustomStatusBlock("Pulse", NC.App.name + " " + NC.App.Config.VersionString, pf.Filename, issue);

                }
                catch (FatalNeutronCountingException e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e);
                    ctrllog.TraceEvent(LogLevels.Warning, 429, "Neutron counting incomplete: {0}, processing stopped at {1}", e.Message, BufferStateSnapshot(PseudoInstrument));
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
					ctrllog.TraceEvent(LogLevels.Warning, 430, "Processing stopped at cycle " + BufferStateSnapshot(PseudoInstrument));
                }
                finally
                {
                    sortedpulse.CloseReader();
                    if (meas.CurrentRepetition == 1)// this is the first file, create the results before they get used 
                    {
                        meas.PrepareINCCResults();
                        meas.Persist();
                    }
                    rdt.EndOfCycleProcessing(meas);
                    NC.App.Loggers.Flush();
                }
                FireEvent(EventType.ActionInProgress, this);
            } // loop over each NCD file
            rdt.EndOfCycleProcessing(meas, last: true);
            PseudoInstrument.selected = false;

            FireEvent(EventType.ActionInProgress, this);

            NC.App.Opstate.StopTimer();
            if (!NC.App.Opstate.IsAbortRequested) // stop/quit means continue with what is available
            {
                if (meas.HasReportableData && totalBuffersProcessed > 0) // todo: test 
                { 
                // if we have more than one cycle (one per file), and the cycles are combined into a 'measurement', then do the meta-processing across the results cycle list here
                NC.App.Opstate.ResetTimer(postprocessing, meas, 50, (int)NC.App.AppContext.StatusTimerMilliseconds);

                meas.CalculateMeasurementResults();

                NC.App.Opstate.StopTimer();
                FireEvent(EventType.ActionInProgress, this);

                ReportMangler rm = new ReportMangler(ctrllog);
                rm.GenerateReports(meas);
                meas.SaveMeasurementResults();
                }
            }

            NC.App.Opstate.ResetTokens();
            Instruments.All.Remove(PseudoInstrument);

            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
			FireEvent(EventType.ActionFinished, this);

        }

        public void Cleanup()
        {
            PseudoInstrument = null;
            NC.App.Opstate.ClearTimers();
            GC.Collect();
        }

        string BufferStateSnapshot(Instrument inst)
        {
            InstrumentStateSnapshot iss = new InstrumentStateSnapshot(inst);

            string s = string.Format("buffer {0}, {1}", iss.asy.numProcessedRawDataBuffers, iss.ToString());

            return s;
        }


        // controller has active control of processing through these three entry points
        public void CancelCurrentAction()
        {
            string name = (NC.App.AppContext.DBDataAssay ? "database" : "file");
            if (PseudoInstrument != null)
                name = PseudoInstrument.id.Identifier();
            ctrllog.TraceInformation("Cancelling the {0} {1} action", name, NC.App.Opstate.Action);
            if (PseudoInstrument != null)
                PseudoInstrument.RDT.EndAnalysisImmediately();
            NC.App.Opstate.SOH = NCC.OperatingState.Cancelling;
            NC.App.Opstate.Cancel();
            if (PseudoInstrument != null)
                PseudoInstrument.PendingComplete();
        }

        public void StopCurrentAction()
        {
            string name = (NC.App.AppContext.DBDataAssay ? "database" : "file");
            if (PseudoInstrument != null)
                name = PseudoInstrument.id.Identifier();
            ctrllog.TraceInformation("Stopping the {0} {1} action", name, NC.App.Opstate.Action);
            if (PseudoInstrument != null)
                PseudoInstrument.RDT.EndAnalysisImmediately();
            NC.App.Opstate.SOH = NCC.OperatingState.Stopping;
            NC.App.Opstate.Cancel();
            if (PseudoInstrument != null)
                PseudoInstrument.PendingComplete();
        }

        public void StartAction()
        {
            string name = (NC.App.AppContext.DBDataAssay ? "database" : "file");
            if (PseudoInstrument != null)
                name = PseudoInstrument.id.Identifier();
            NC.App.Opstate.ResetTokens();
            ctrllog.TraceInformation("Starting the {0} {1} action", name, NC.App.Opstate.Action);
            NC.App.Opstate.SOH = NCC.OperatingState.Starting;
        }



        #region FileControl Callbacks and Event Handlers

		public string InstrStatusString(CombinedInstrumentProcessingStateSnapshot cipss, bool channels = false)
		{
			string s = string.Empty;
			try
			{
				string id = cipss.iss.action.ToString() + " " + cipss.iss.ins.ToString();  // some identifying info
				if (cipss.iss.ins.IsSuspect)
					s = cipss.iss.ins.Reason + ", " + id;
				else
				{
					string ss = cipss.cs.ToString() + ";";
					ss += cipss.iss.ToString();
					s = ss + " " + id;
					if (channels && cipss.iss.asy != null)
						s += ";  " + cipss.iss.asy.cpss.HitsPerChnImage();
				}
			}
			catch (System.ObjectDisposedException)
			{
			}
			return s;
		}

		public string LoggableInstrStatusString(object o, bool channels = false)
        {
            Instrument inst = (Instrument)o;
            CombinedInstrumentProcessingStateSnapshot cps = new CombinedInstrumentProcessingStateSnapshot(inst);
            return FileProcStatusString(cps, channels);
        }

		string Elide(string s, int limit)
		{
			if (s.Length > limit + 3)
				return s.Substring(s.Length-limit)  + "...";
			else
				return s;
		}
        
        public string SuccinctInstrStatusString(object o)
        {
            if (o == null)
				return string.Empty;

			Instrument inst = (Instrument)o;
			return Elide(inst.id.FileName, 64);
        }
        public string FileProcStatusString(CombinedInstrumentProcessingStateSnapshot cipss, bool channels = false)
        {
            string s = string.Empty;
            string id = string.Empty;
            string ss = string.Empty;
            try
            {
                if (cipss.HasIdent)
                    id = cipss.iss.ins.dsid.Identifier();
                if (cipss.HasCounterStatus)
                    ss = cipss.cs.ToString() + ";";
                //ss += cipss.iss.ToString();
                s = ss + " " + id;
                if (channels && cipss.HasProcessingStatus)
                    s += ";  " + cipss.iss.asy.cpss.HitsPerChnImage();
            }
            catch (System.ObjectDisposedException)
            {
            }
            return s;
        }
    

        static public string MeasStatusString(Measurement m)
        {
            MeasurementStatus ms = new MeasurementStatus();
            return MeasStatusString(ms);
        }
        static public string MeasStatusString(MeasurementStatus ms)
        {
            return ms.ToString();
        }
        // timer stubs, implement these in your subclass
        public void PreActionStatusTimerCB(object o)
        {
        }
        public void ActionPreparationTimerCB(object o)
        {
        }
        public void ActionInProgressStatusTimerCB(object o)
        {
        }
        public void FurtherActionInProgressStatusTimerCB(object o)
        {
        }

		public static string LogAndSkimFileProcessingStatus(EventType EH, LMLoggers.LognLM log, LogLevels lvl, object o)
		{
			LoggableFileProcessingStatus(EH,log,lvl,o);
			return SuccinctFileProcessingStatus(EH,o);
		}
        public static string LoggableFileProcessingStatus(EventType EH, LMLoggers.LognLM log, LogLevels lvl, object o)
        {
            string s = EH.ToString() + ": ";
            if (o != null)
            {
                string ss = string.Empty;
                if (o is FileCtrl)
                {
                    FileCtrl f = (FileCtrl)o;
                    ss = f.LoggableInstrStatusString(f.PseudoInstrument, true);
                }
                else if (o is Measurement)
                {
                    Measurement m = (Measurement)o;
                    ss = FileCtrl.MeasStatusString(m);
                }
                s += ss;
            }
            log.TraceEvent(lvl, FileCtrl.logid[EH], s);
            return s;  // just in case it could be of further use
        }

        public static string SuccinctFileProcessingStatus(EventType EH, object o)
        {
            string s = string.Empty;
            if (o != null)
            {
                string ss = string.Empty;
                if (o is FileCtrl)
                {
                    FileCtrl f = (FileCtrl)o;
                    ss = f.SuccinctInstrStatusString(f.PseudoInstrument);
                }
                else if (o is Measurement)
                {
                    Measurement m = (Measurement)o;
                    ss = FileCtrl.MeasStatusString(m);
                }
                s += ss;
            }            
            return s;
        }

        // four progress callbacks are required. 
        // caller sets these
        // null means no progress update via timer.
        private TimerCallback neutronCountingPrep;
        private TimerCallback filegather;
        private TimerCallback filerawprocessing;
        private TimerCallback postprocessing;

        public TimerCallback NeutronCountingPrep
        {
            get { return neutronCountingPrep; }
            set { if (value == null) neutronCountingPrep = null; else neutronCountingPrep = new TimerCallback(value); }
        }
        public TimerCallback Filegather
        {
            get { return filegather; }
            set { if (value == null) filegather = null; else filegather = new TimerCallback(value); }
        }

        // these are two independent timers so they can post simultaneously
        public TimerCallback Filerawprocessing
        {
            get { return filerawprocessing; }
            set { if (value == null) filerawprocessing = null; else filerawprocessing = new TimerCallback(value); }
        }
        public TimerCallback Postprocessing
        {
            get { return postprocessing; }
            set { if (value == null) postprocessing = null; else postprocessing = new TimerCallback(value); }
        }

        protected void FileProcessingCtrlCHandler(object sender, ConsoleCancelEventArgs args)
        {
            // Announce that the event handler has been invoked.
            ctrllog.TraceInformation("Interrupting the {0} action", NC.App.Opstate.Action);
            args.Cancel = true;
            PseudoInstrument.RDT.EndAnalysisImmediately();
            NC.App.Opstate.SOH = NCC.OperatingState.Cancelling;
            NC.App.Opstate.Cancel();
            PseudoInstrument.PendingComplete();
        }

        #endregion FileControl Callbacks and Event Handlers

    }


    /*
    NEXT: implement the interactive version of NCC Review file import 
5. Each measurement that passes step 4 is displayed in chronological order, one line per measurement, in a selection screen with the following information:
    Detector id Item id Meas. Type Date Time Filename

6. The user selects as many measurements as desired, and they are processed in chronological order. The default is to select all measurements. The last processed background and normalization measurements will be saved in the database and used with subsequent verification measurements.

7. At this point there are two options. If the detector dependent “auto import” box was not checked, as each measurement is processed, the appropriate acquire dialog box is displayed, enabling the user to specify the desired details for that measurement. Steps 7, 8 and 9 only apply if “Auto import” was not checked.

Background measurements
Enter a comment if desired
From a checkbox select to use or not use QC tests
From a checkbox select to print or not print results
From a checkbox select to enter or not enter an ending comment
From a separate dialog box choose whether this is a passive or active measurement

Normalization measurements
Enter a comment if desired
From a checkbox select to use or not use QC tests
From a checkbox select to print or not print results
From a checkbox select to enter or not enter an ending comment
Source id (display only)

Verification measurements
Select Material Balance Area (defaults to current MBA)
Select or enter Item Id (defaults to the one in the file header)
Select Stratum Id (defaults to one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Select Material Type (defaults to the one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Enter declared mass (defaults to the one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Select Inventory Change Code (defaults to the one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Select I/O Code (defaults to the one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Select isotopics or composite isotopics by clicking on the button (defaults to the one corresponding to the item id in the file header if previously set through the Item Data Entry dialog box)
Enter a comment if desired
From a checkbox select to use or not use QC tests
From a checkbox select to print or not print results
From a checkbox select to enter or not enter an ending comment
If the “Auto import” checkbox has been checked, then as each measurement is processed only the dialog box for selecting active or passive is displayed for background measurements, no dialog boxes are displayed for normalization measurements, and if the item data for the item id of the current measurement have been previously entered then no dialog boxes are displayed for verification measurements. The “Auto import” checkbox is accessible in maintenance mode by selecting Maintain | Unattended Measurements Setup.

8. In manual mode a results text window is optionally printed for each selected measurement in the same format as for other measurements. The data source will be “Review disk file”.

9. In manual mode, if ‘Ending comment’ was selected then a dialog

10. If this is a verification measurement then the following data for this measurement will be appended to the file for Integrated Review to reconcile and display:
Pass/Fail status (pass specified as green, fail specified as yellow)
Detector id
Item id
Measurement start time
Measurement start date
Calculated mass (g) from the primary analysis method
Calculated mass error
Declared - assay mass (g)
Declared - assay mass (%)
Isotopics source code

If the analysis method is curium ratio, the following data for this measurement will be appended to the file for Integrated Review to reconcile and display:
Pu Pass/Fail status (pass specified as green, fail specified as yellow)
Total U Pass/Fail status (pass specified as green, fail specified as yellow)
235U Pass/Fail status (pass specified as green, fail specified as yellow)
Detector id
Item id
Measurement start time
Measurement start date
Calculated Pu mass (g)
Calculated Pu mass error
Declared - assay Pu mass (g)
Declared - assay Pu mass (%)
Calculated total U mass (g)
Calculated total U mass error
Declared - assay total U mass (g)
Declared - assay total U mass (%)
Calculated 235U mass (g)
Calculated 235U mass error
Declared - assay 235U mass (g)
Declared - assay 235U mass (%)

11. The import files created by Operator Review and Radiation Review for INCC will be deleted so that these operator declarations and measurements will not get imported again the next time there is an import.

12. When all measurement files have been processed, INCC automatically will exit.

*/



    // This file processing specification includes other actions that operate on file sets to merge/filter/transform/cleanse/normalize/obfuscate them, or whatever we need to do with them

    // Operations occur on folders with files, selected lists of files or packages of files (zip/gz/gzip)
    // Input files are specified using lists of file names, regex or folders
    // A "file" is a file of the target type or a package of one or more ncd files
    // A file name includes single files or packages
    // 
    // All the operations on files can take a System.IO.FileInfo or DirectoryInfo as input, unroll the contents, apply the operation to the unrolled contents
    // 
    // See System.IO.Compression and DirectoryInfo.GetFileSystemInfos for classes to use for input file and folder processing 
    //
    // There are four kinds of file operations
    //    File analysis (output: reports and/or modified files)
    //        simple summary            in: files out: report content in a data structure, report written to file/DB
    //        smoothing/filtering       in: files, quality and conditioning metrics, out: files, report content in data structure, report in file/DB
    //        time and channel shifting  in: files, specs for modifying times and channels, out: ditto
    //        gap and consistency analysis  in: files, specs for conditions under review, out: ditto
    //
    //    File transforms (output: files and packages)
    //        converting to text or other formats, in: files, xform spec, out: files
    //
    //    File handling (output: files and packages)
    //        merging multiple files into single files or packages  in:
    //        organizing => packaging, unpackaging, compress files/pkgs, uncompress files/pkgs, encrypt files/pkgs, decrypt files/pkgs)
    //
    //    Analysis processing (outputs: reports, results and intermediate results, in files or database)
    //      



    //protected void SortedLMDFileToNCD()
    //{

    //    List<string> ext = new List<string>() { "sorted.lmd" };
    //    FileList<SortedLMDFile> hdlr = new FileList<SortedLMDFile>();
    //    hdlr.Init(ext, ctrllog);

    //    // get the list of files from the named folder
    //    FileList<SortedLMDFile> files = (FileList<SortedLMDFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, false);
    //    if (files == null || files.Count() < 1)
    //    {
    //        return;
    //    }
    //    int eventBufferLength = (int)NC.App.Config.Buff.ParseBufferSize * 1024 * 1024;
    //    int maxValuesInBuffer = eventBufferLength / sizeof(double);
    //    LMPair[] buffer = new LMPair[maxValuesInBuffer];
    //    int NumProcessedRawDataBuffers = 0;

    //    foreach (var pf in files)
    //    {
    //        if (NC.App.Opstate.IsCancellationRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
    //            break;
    //        if (!pf.OpenForReading())
    //            continue;
    //        NCDFile ncdfile = PrepNCDFile(pf, 0);
    //        if (ncdfile == null)
    //            continue;
    //        ctrllog.TraceEvent(LogLevels.Info, 3335, "Converting {0} to {1}", pf.Filename, ncdfile.Filename);
    //        try
    //        {
    //            NumProcessedRawDataBuffers = 0;
    //            Double time = 0, lasttime = 0;
    //            string issue = string.Empty;
    //            while (!pf.reader.EndOfStream && string.IsNullOrEmpty(issue))
    //            {
    //                Int32 tchn = 0, chn = 0;
    //                string s;
    //                int rb = 0;
    //                NumProcessedRawDataBuffers++;
    //                do
    //                {
    //                    tchn = 0;
    //                    s = pf.reader.ReadLine();
    //                    pf.ParseLine(s, ref tchn, ref time);  // keep or'ing the channels into the same chn int, time is the same
    //                    chn |= tchn;  // or the bits
    //                    if (time == lasttime && rb > 0)
    //                        buffer[rb - 1].ChannelMask |= (uint)tchn;  // or the bits
    //                    else
    //                    {
    //                        lasttime = time; // new time, capture the pair and move to next line
    //                        buffer[rb++] = new LMPair((uint)chn, time);
    //                        chn = 0;  // reset final
    //                    }
    //                } while (!pf.reader.EndOfStream && rb < maxValuesInBuffer);
    //                // now transform the pairs to ncd 

    //                issue = ncdfile.TransferToTraditionalNCDFormat(buffer, (ulong)rb);

    //                pf.Log.TraceEvent(LogLevels.Verbose, 3336, "Processing buffer {0} of {1} doubles", NumProcessedRawDataBuffers, rb);
    //            }
    //            ncdfile.CustomStatusBlock("Pulse", NC.App.name + " " + NC.App.Config.VersionString, pf.Filename, issue);
    //            ncdfile.WriteTagAndStatusBlock();

    //        }
    //        catch (Exception e)
    //        {
    //            NC.App.Opstate.SOH = NCC.LMOpState.Trouble;
    //            ctrllog.TraceException(e, true);
    //            ctrllog.TraceEvent(LogLevels.Warning, 3361, "Processing stopped at cycle " + NumProcessedRawDataBuffers);
    //        }
    //        finally
    //        {
    //            pf.CloseReader();
    //            ncdfile.CloseWriter();
    //            NC.App.Loggers.Flush();
    //        }
    //    } // loop over each file
    //}
    /// <summary>
    /// NEXT: Incomplete, need an object byte serializer for the dual entry (Channle + time) in the merge sort template
    /// </summary>
    //protected void LMDFileSort()
    //{

    //    uint eventBufferLength = NC.App.Config.Buff.ParseBufferSize * 1024 * 1024;

    //    List<string> ext = new List<string>() { ".lmd" };
    //    FileList<UnsortedLMDFile> hdlr = new FileList<UnsortedLMDFile>();
    //    hdlr.Init(ext, ctrllog);

    //    // get the list of files from the named folder
    //    FileList<UnsortedLMDFile> files = (FileList<UnsortedLMDFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, false);
    //    if (files == null || files.Count() < 1)
    //    {
    //        return;
    //    }

    //    foreach (var pf in files)
    //    {
    //        if (NC.App.Opstate.IsCancellationRequested)  // cancellation occurs here and at selected steps in the internal file and analyzer processing 
    //            break;

    //        string derivedDataFilenamePrefix = pf.GenerateDerivedName();
    //        ctrllog.TraceEvent(LogLevels.Info, 3330, "Sorting {0} to {1}", pf.Filename, derivedDataFilenamePrefix + ".sorted");

    //        var opt = new ExternalMergeSort.Options(ExternalMergeSort.sizeFromMB((int)NC.App.Config.Buff.ParseBufferSize), ExternalMergeSort.sizeFromMB(10 /*0*/ /*10*/) / 10, 1024 * 10);
    //        opt.SkipInitialSort = false; // a single file only, so no external merge at this time, save that feature for later
    //        opt.RemoveIntermediateFiles = true; // no merge here, we are breaking up a single pulse file into n temp files, so we need to remove the n temp files
    //        ExternalMergeSort.Logger = pf.Log;
    //        ExternalMergeSort.Sort(pf.Filename, derivedDataFilenamePrefix + ".sorted", NC.App.Config.RootLoc, opt);

    //    } // loop over each file
    //}
}
