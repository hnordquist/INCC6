/*
The Dataz INCC integration source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

The Dataz INCC integration source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2017, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using AnalysisDefs;
using DetectorDefs;
using NCC;
using NCCReporter;
using System;
using System.Collections.Generic;
using System.IO;

namespace NCCFile
{
    using NC = CentralizedState;
    public partial class FileCtrl : ActionEvents, IActionControl
    {

        void DatazFileAssay()
        {
            List<string> exts = new List<string>() { ".dataz" };
            FileList<DatazFile> hdlr = new FileList<DatazFile>();
            hdlr.Init(exts, ctrllog);
            FileList<DatazFile> files = null;

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<DatazFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, true);
            else
                files = (FileList<DatazFile>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
            if (files == null || files.Count < 1)
            {
                NC.App.Opstate.StopTimer();
                NC.App.Opstate.StampOperationStopTime();
                FireEvent(EventType.ActionStop, this);
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "No usable Dataz files found");
                return;
            }

            AssaySelector.MeasurementOption mo = NC.App.Opstate.Measurement.MeasOption;

            foreach (DatazFile mc in files)
            {
                Measurement meas = null;

                try
                {
                    if (!mc.OpenForReading())
                        continue;
                    if (NC.App.Opstate.IsQuitRequested)
                        break;

                    mc.ScanSections();
                    mc.ProcessSections();
                    if (mc.Cycles.Count == 0)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 404, "This Dataz file has no good cycles.");
                    }
                    if (mc.Plateaux.Count == 0)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 404, $"This Dataz file has no defined sequences, over {mc.Cycles.Count} cycles.");
                    }
                    else
                    {
                        AcquireParameters orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
                        Detector curdet = NC.App.Opstate.Measurement.Detector;
                        if (mc.AcquistionStateChanged)
                        {
                            orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
                            curdet = mc.DataZDetector;
                            if (curdet.AB.Unset)
                            {
                                ABKey abkey = new ABKey(curdet.MultiplicityParams, mc.MaxBins);
                                LMRawAnalysis.SDTMultiplicityCalculator.SetAlphaBeta(abkey, curdet.AB);
                            }
                        }

                        ctrllog.TraceInformation($"{mc.Cycles.Count} cycles and {mc.Plateaux.Count} sequences encountered in Dataz file {mc.Filename}");
                        System.Collections.IEnumerator iter = mc.GetSequences();
                        while (iter.MoveNext())
                        {
                            DatazFile.Plateau pla = (DatazFile.Plateau)iter.Current;
                            ResetMeasurement();
                            // update acq and then meas here
                            AcquireParameters newacq = ConfigureAcquireState(curdet, orig_acq, pla.Cycles[0].DTO, (ushort)pla.Num, mc.Filename);
                            newacq.data_src = ConstructedSource.DatazFile;
                            IntegrationHelpers.BuildMeasurement(newacq, curdet, mo);
                            meas = NC.App.Opstate.Measurement;
                            meas.MeasDate = newacq.MeasDateTime;
                            meas.Persist();  // preserve the basic results record
                            meas.RequestedRepetitions = (ushort)pla.Num;
                            for (int i = 0; i < meas.RequestedRepetitions; i++)
                            {
                                /* run date and time (IAEA format) */
                                AddMCSRDataCycle(i, pla.Cycles[i], meas, mc.Filename);
                                if (i % 8 == 0)
                                    FireEvent(EventType.ActionInProgress, this);
                            }
                            FireEvent(EventType.ActionInProgress, this);
                            ComputeFromINCC5SRData(meas);
                            FireEvent(EventType.ActionInProgress, this);
                        }
                    }
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
                    ctrllog.TraceEvent(LogLevels.Error, 437, "Dataz data file processing stopped with error: '" + e.Message + "'");
                }
                finally
                {
                    mc.CloseReader();
                    NC.App.Loggers.Flush();
                }
            }

            NC.App.Opstate.ResetTokens();
            NC.App.Opstate.SOH = OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
            FireEvent(EventType.ActionFinished, this);
        }

        void AddMCSRDataCycle(int run, DatazFile.Cycle c, Measurement meas, string fname)
        {
            Cycle cycle = new Cycle(datalog);
            try
            {
                cycle.UpdateDataSourceId(ConstructedSource.DatazFile, meas.Detector.Id.SRType, c.DTO, fname);
                cycle.seq = run;
                cycle.TS = c.Duration;
                /* init run tests */
                cycle.SetQCStatus(meas.Detector.MultiplicityParams, QCTestStatus.None); // APluralityOfMultiplicityAnalyzers: creates entry if not found, expand from the single mult key from detector here
                meas.Add(cycle);
                /* singles, reals + accidentals, accidentals */
                cycle.Totals = c.Singles;
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(meas.Detector.MultiplicityParams.FA, cycle.seq); // APluralityOfMultiplicityAnalyzers: expand when detector has multiple analyzers
                cycle.CountingAnalysisResults.Add(meas.Detector.MultiplicityParams, mcr);  // APluralityOfMultiplicityAnalyzers: expand when detector has multiple analyzers
                mcr.AB.TransferIntermediates(meas.Detector.AB);  // copy alpha beta onto the cycle's results 
                mcr.Totals = cycle.Totals;
                mcr.TS = cycle.TS;
                mcr.ASum = c.Accidentals;
                mcr.RASum = c.RealsPlusAccidentals;
                mcr.Scaler1.v = 0;
                mcr.Scaler2.v = 0;
                cycle.SinglesRate = cycle.Totals / c.Duration.TotalSeconds;

                // assign the hits to a single channel (0)
                cycle.HitsPerChannel[0] = cycle.Totals;

                mcr.RawSinglesRate.v = cycle.SinglesRate;

                /* number of multiplicity values */
                mcr.MinBins = mcr.MaxBins = (ulong)c.BinLen;
                mcr.RAMult = new ulong[c.BinLen];
                mcr.NormedAMult = new ulong[c.BinLen];
                mcr.UnAMult = new ulong[c.BinLen]; // not used, not LM
                /* multiplicity values */
                for (ushort j = 0; j < c.BinLen; j++)
                {
                    mcr.RAMult[j] = c.MultRABins[j];
                    mcr.NormedAMult[j] = c.MultNormedAccBins[j];
                }
                ctrllog.TraceEvent(LogLevels.Verbose, 5439, "Cycle " + cycle.seq.ToString() + ((mcr.RAMult[0] + mcr.NormedAMult[0]) > 0 ? " max:" + mcr.MaxBins.ToString() : " *"));

            }
            catch (Exception e)
            {
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "cycle processing error {0} {1}", run, e.Message);
            }
        }

        enum DatazConversionTarget : ushort { TestData, NCC, XFer, InitialDataPair }
        protected void DatazFileConvert() // URGENT //  0 INCC5 test data file, 1 NCC Review file, 2 INCC5 xfer file, 3 INCC5 ini data detector and calibration files
        {
            List<string> exts = new List<string>() { ".dataz" };
            FileList<DatazFile> hdlr = new FileList<DatazFile>();
            hdlr.Init(exts, ctrllog);
            FileList<DatazFile> files = null;

            // initialize operation timer here
            NC.App.Opstate.ResetTimer(filegather, files, 170, (int)NC.App.AppContext.StatusTimerMilliseconds);
            FireEvent(EventType.ActionPrep, this);
            NC.App.Opstate.StampOperationStartTime();

            if (NC.App.AppContext.FileInputList == null)
                files = (FileList<DatazFile>)hdlr.BuildFileList(NC.App.AppContext.FileInput, NC.App.AppContext.Recurse, true);
            else
                files = (FileList<DatazFile>)hdlr.BuildFileList(NC.App.AppContext.FileInputList);
            if (files == null || files.Count < 1)
            {
                NC.App.Opstate.StopTimer();
                NC.App.Opstate.StampOperationStopTime();
                FireEvent(EventType.ActionStop, this);
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "No usable Dataz files found");
                return;
            }

            AssaySelector.MeasurementOption mo = NC.App.Opstate.Measurement.MeasOption;

            foreach (DatazFile mc in files)
            {

                try
                {
                    if (!mc.OpenForReading())
                        continue;
                    if (NC.App.Opstate.IsQuitRequested)
                        break;

                    mc.ScanSections();
                    mc.ProcessSections(analyze:true);
                    if (mc.Cycles.Count == 0)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 404, "This Dataz file has no good cycles.");
                    }
                    if (mc.Plateaux.Count == 0)
                    {
                        ctrllog.TraceEvent(LogLevels.Error, 404, $"This Dataz file has no defined sequences, over {mc.Cycles.Count} cycles.");
                    }
                    else
                    {
                        Measurement meas = null;
                        AcquireParameters orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
                        Detector curdet = NC.App.Opstate.Measurement.Detector;
                        if (mc.AcquistionStateChanged)
                        {
                            orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
                            curdet = mc.DataZDetector;
                            if (curdet.AB.Unset)
                            {
                                ABKey abkey = new ABKey(curdet.MultiplicityParams, mc.MaxBins);
                                LMRawAnalysis.SDTMultiplicityCalculator.SetAlphaBeta(abkey, curdet.AB);
                            }
                        }
                        ctrllog.TraceInformation($"{mc.Cycles.Count} cycles and {mc.Plateaux.Count} sequences encountered in Dataz file {mc.Filename}");
                        System.Collections.IEnumerator iter = mc.GetSequences();
                        while (iter.MoveNext())
                        {
                            DatazFile.Plateau pla = (DatazFile.Plateau)iter.Current;
                            ResetMeasurement();
                            // update acq and then meas here
                            AcquireParameters newacq = ConfigureAcquireState(curdet, orig_acq, pla.Cycles[0].DTO, (ushort)pla.Num, mc.Filename);
                            newacq.data_src = ConstructedSource.DatazFile;
                            IntegrationHelpers.BuildMeasurement(newacq, curdet, mo);
                            meas = NC.App.Opstate.Measurement;
                            meas.MeasDate = newacq.MeasDateTime;
                            meas.RequestedRepetitions = (ushort)pla.Num;
                            FireEvent(EventType.ActionInProgress, this);
                            switch ((DatazConversionTarget)NC.App.Config.Cur.DatazConvertType)
                            {
                                case DatazConversionTarget.TestData:
                                    for (int i = 0; i < meas.RequestedRepetitions; i++)
                                        AddMCSRDataCycle(i, pla.Cycles[i], meas, mc.Filename);
                                    AnalysisDefs.TestDataFile mdat = new AnalysisDefs.TestDataFile(ctrllog);
                                    mdat.GenerateReport(meas);
                                    break;
                                case DatazConversionTarget.NCC:
                                    {
                                        string name;
                                        if (NC.App.AppContext.Results8Char)
                                            name = MethodResultsReport.EightCharConvert(meas.MeasDate) + ".ncc";
                                        else
                                        {
                                            name = meas.MeasOption.PrintName() + meas.MeasDate.ToString("yyyyMMddHHmmss");
                                            if (!string.IsNullOrEmpty(meas.Detector.Id.DetectorId))
                                                name = meas.Detector.Id.DetectorId + " " + name;
                                            name += ".ncc";
                                        }
                                        NCCFileWriter _NCC = new NCCFileWriter
                                        {
                                            MeasStartDate = meas.MeasDate,
                                            DetectorId = meas.Detector.Id.DetectorId,
                                            Name = name,
                                            Path = meas.AcquireState.lm.Results,
                                            ItemId = meas.AcquireState.ItemId.item,
                                            MeasOption = "V", // todo: expand cmd line flags o include B,V,N, or add meas option to Dataz configuration section  meas.MeasOption.PrintName(),
                                            Log = ctrllog,
                                            RunTime = meas.AcquireState.run_count_time
                                        };

                                        ctrllog.TraceEvent(LogLevels.Info, 111, "Creating new output file: " + System.IO.Path.Combine(_NCC.Path, _NCC.Name));
                                        bool yes = _NCC.StartNCCFile((ushort)pla.Num); // yes it truncates
                                        if (!yes)
                                        {
                                            ctrllog.TraceEvent(LogLevels.Warning, 33085, "NCC file initialization failed."); 
                                            continue;
                                        }
                                        List<NCCFileWriter.NCCData> xfer = new List<NCCFileWriter.NCCData>();
                                        for (int i = 0; i < meas.RequestedRepetitions; i++)
                                        {
                                            DatazFile.Cycle c = pla.Cycles[i];
                                            NCCFileWriter.NCCData ncd = new NCCFileWriter.NCCData()
                                            {
                                                dt = c.DTO.DateTime,
                                                sr = new NCCFileWriter.SRData()
                                                {
                                                    totals = c.Singles,
                                                    r_plus_a = c.RealsPlusAccidentals,
                                                    a = c.Accidentals,
                                                    scaler1 = 0,
                                                    scaler2 = 0
                                                },
                                                mult = new NCCFileWriter.SRMult((ushort)c.BinLen)
                                            };
                                            if (ncd.mult.n_mult > 0)
                                            {
                                                for (int j = 0; j < c.BinLen; j++)
                                                {
                                                    ncd.mult.r_plus_a[j] = (uint)c.MultRABins[j]; // oops
                                                    ncd.mult.a[j] = (uint)c.MultNormedAccBins[j]; // oops
                                                }
                                            }
                                            xfer.Add(ncd);                                         
                                        }
                                        _NCC.TransferIRAPRunDataToNCCList(xfer);
                                        _NCC.WriteBody();
                                        _NCC.EndNCCFile();
                                        //_NCC.FullPath
                                    }

                                    break;
                                case DatazConversionTarget.XFer:
                                    ctrllog.TraceEvent(LogLevels.Warning, 33085, "Dataz to Transfer file conversion not yet implemented, and may never be");
                                    break;
                                case DatazConversionTarget.InitialDataPair:
                                    ctrllog.TraceEvent(LogLevels.Warning, 33085, "Dataz det/det.cal pair file conversion not yet implemented, and may never be");
                                    break;
                            }
                            FireEvent(EventType.ActionInProgress, this);
                        }
                    }
                }
                catch (Exception e)
                {
                    NC.App.Opstate.SOH = OperatingState.Trouble;
                    ctrllog.TraceException(e, true);
                    ctrllog.TraceEvent(LogLevels.Error, 437, "Dataz data file processing stopped with error: '" + e.Message + "'");
                }
                finally
                {
                    mc.CloseReader();
                    NC.App.Loggers.Flush();
                }
            }

            NC.App.Opstate.ResetTokens();
            NC.App.Opstate.SOH = OperatingState.Stopping;
            NC.App.Opstate.StampOperationStopTime();
            FireEvent(EventType.ActionFinished, this);


        }


        public class DatazFile : NeutronDataFile   // NEXT: this is only the MCSR specific variant, expand class definitions later
        {

            public DatazFile()
            {

                Header = new Dictionary<string, string>();
                History = new List<Tuple<string, string>>();

                // MAIN, SG, MULTIPLICITY, INCC, SEQUENCES, SEQUENCES:MULTIPLICITY 
                Configuration = new Dictionary<SecondaryTag, Dictionary<string, string>>();
                Configuration[SecondaryTag.MAIN] = new Dictionary<string, string>();  // config:main
                Configuration[SecondaryTag.SG] = new Dictionary<string, string>();
                Configuration[SecondaryTag.INCC] = new Dictionary<string, string>();
                Configuration[SecondaryTag.MULTIPLICITY] = new Dictionary<string, string>();
                Configuration[SecondaryTag.SEQUENCES] = new Dictionary<string, string>();  // todo: not used, config:seq:multiplicity

                // DATA:MULTIPLICITY, SETTINGS:MULTIPLICITY
                Settings = new Dictionary<SecondaryTag, Dictionary<string, string>>();
                Settings[SecondaryTag.MULTIPLICITY] = new Dictionary<string, string>();
                Data = new Dictionary<SecondaryTag, List<string[]>>();
                Data[SecondaryTag.MULTIPLICITY] = new List<string[]>();

                // MULTIPLICITY:Mult0 (or Totals_SR)
                Sequences = new Dictionary<SecondaryTag, List<string[]>>();
                Sequences[SecondaryTag.MULTIPLICITY] = new List<string[]>(); // Mult0, Totals_SR

                Cycles = new List<Cycle>();
                Plateaux = new List<Plateau>();
                dataIndices = new Indexes();
            }

            StreamReader reader;

            public override bool OpenForReading(string filename = null)
            {
                bool ok = base.OpenForReading(filename);
                if (ok)
                    reader = new StreamReader(stream);
                return ok;
            }

            public void CloseReader()
            {
                if (reader == null)
                    return;
                reader.Close();
                base.CloseStream();
            }

            public override void ConstructFullPathName(string opt = "")
            {
                base.ConstructFullPathName(opt);
                Filename += ".dataz";
            }


            public System.Collections.IEnumerator GetSequences()
            {
                foreach (Plateau p in Plateaux)
                {
                    yield return p;
                }
            }

            public class Plateau
            {
                public int StartIdx, EndIdx;
                public int Num;
                public double Avg;
                public double CycleInterval;
                public Cycle[] Cycles;

                public Plateau()
                {
                }

                static public Plateau Parse(string[] s, List<Cycle> clist)
                {
                    Plateau p = new Plateau();
                    int.TryParse(s[1], out p.StartIdx);
                    int.TryParse(s[2], out p.EndIdx);
                    int.TryParse(s[3], out p.Num);
                    double.TryParse(s[4], out p.Avg);
                    p.Cycles = new Cycle[p.Num];
                    for (int i = p.StartIdx, j = 0; i <= p.EndIdx; i++, j++)  // use GetRange and ToArray you silly person
                        p.Cycles[j] = clist[i];
                    if (clist != null && clist.Count > 0)
                        p.CycleInterval = clist[0].Duration.TotalSeconds;
                    return p;
                }

                public DateTimeOffset FirstCycleTime
                {
                    get { return (Cycles != null && Cycles.Length > 0) ? Cycles[0].DTO : new DateTimeOffset(NCCTransfer.INCC.ZeroIAEATime);  }
                }
            }

            public void ProcessSections(bool analyze = true)
            {
                AcquistionStateChanged = false;
                ExtractTimeZone();
                foreach (string[] s in Data[SecondaryTag.MULTIPLICITY])
                {
                    if (dataIndices.totalChannels == 0)
                        DoDataHeader(s);
                    else
                    {
                        Cycle c = Cycle.Parse(s, dataIndices, _TZ);
                        MaxBins = Math.Max(MaxBins, c.BinLen);
                        Cycles.Add(c);
                    }
                }
                Data[SecondaryTag.MULTIPLICITY] = new List<string[]>();  // clear the majority of the file content from memory
                int seqnum = -1;
                foreach (string[] s in Sequences[SecondaryTag.MULTIPLICITY])
                {
                    if (seqnum < 0)
                        seqnum = 0;
                    else
                        Plateaux.Add(Plateau.Parse(s, Cycles));
                }
                if (Plateaux.Count > 0 && analyze)  // set the current runtime state only if it will be used
                    SetCurrentAcquireStateFromZFile();
            }

            TimeSpan _TZ;
            void ExtractTimeZone()
            {
                float tz = 0;
                try
                {
                    string tzs;
                    if (Header.TryGetValue("TimeZone", out tzs))
                        float.TryParse(tzs, out tz);
                }
                catch (Exception)
                {
                }
                _TZ = new TimeSpan(0, (int)(tz * 60), 0);
            }


            void SetCurrentAcquireStateFromZFile()
            {
                string a = string.Empty;

                Configuration[SecondaryTag.MAIN].TryGetValue("InstrumentName", out a);
                if (string.IsNullOrEmpty(a))
                    Configuration[SecondaryTag.SG].TryGetValue("Instrument", out a);  // unlikely now
                if (string.IsNullOrEmpty(a))
                {
                    NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, "No detector name specified, using current detector for analysis");
                    return;
                }

                Detector _candidate = new Detector(); // build from these config section details
                _candidate.Id.DetectorName = a;
                _candidate.Id.source = ConstructedSource.DatazFile;

                if (Configuration[SecondaryTag.MAIN].TryGetValue("DetectorType", out a))
                    _candidate.Id.Type = a;
                if (Configuration[SecondaryTag.MAIN].TryGetValue("InstrumentType", out a))
                    _candidate.Id.SetSRType(a);

                if (Configuration[SecondaryTag.MULTIPLICITY].TryGetValue("InstrumentConfigurationID", out a))
                    _candidate.Id.ElectronicsId = a;
                if (Configuration[SecondaryTag.MULTIPLICITY].TryGetValue("Port", out a))
                    _candidate.Id.FullConnInfo.Port = a;
                if (Configuration[SecondaryTag.MULTIPLICITY].TryGetValue("HV", out a))
                    double.TryParse(a, out _candidate.SRParams.highVoltage);

                // defaults
                if (Configuration[SecondaryTag.MULTIPLICITY].TryGetValue("PD_us", out a))
                {
                    double d;
                    double.TryParse(a, out d);
                    _candidate.SRParams.predelayMS = d;
                }
                if (Configuration[SecondaryTag.MULTIPLICITY].TryGetValue("GW_us", out a))
                {
                    double d;
                    double.TryParse(a, out d);
                    _candidate.SRParams.gateLengthMS = d;
                }
                if (Configuration[SecondaryTag.INCC].TryGetValue("AccidentalsMode", out a))
                {
                    _candidate.MultiplicityParams.FA = a.Equals("Standard", StringComparison.InvariantCultureIgnoreCase) ? FAType.FAOff : FAType.FAOn;
                    if (_candidate.MultiplicityParams.FA == FAType.FAOff)
                        _candidate.Id.SRType = InstrType.JSR15; //  test this experiment
                    else
                        _candidate.Id.SRType = InstrType.AMSR; // should this always be the default?
                }

                // specific settings that override defaults, might be more these, so code this up more generally lol
                if (Settings[SecondaryTag.MULTIPLICITY].TryGetValue("PD_us", out a))
                {
                    double d;
                    double.TryParse(a, out d);
                    _candidate.SRParams.predelayMS = d;
                }
                if (Settings[SecondaryTag.MULTIPLICITY].TryGetValue("GW_us", out a))
                {
                    double d;
                    double.TryParse(a, out d);
                    _candidate.SRParams.gateLengthMS = d;
                }

                string item = string.Empty;
                Configuration[SecondaryTag.INCC].TryGetValue("ItemId", out item);
                a = string.Empty;
                Configuration[SecondaryTag.INCC].TryGetValue("MaterialType", out a);
                string fac = string.Empty;
                Configuration[SecondaryTag.MAIN].TryGetValue("FacilityCode", out fac);
                string mba = string.Empty;
                Configuration[SecondaryTag.MAIN].TryGetValue("System", out mba);
                string msg = string.Empty;
                Header.TryGetValue("Remarks", out msg);

                // if det exists in DB then switch to that one, else add it to DB and switch to it, same for facility, system/mba, item type
                SetNewAcquireState(_candidate, a, fac, mba, msg, Plateaux[0].CycleInterval);
            }


            bool SetNewAcquireState(Detector det0, string mtl, string fac, string mba, string msg, double interval)
            {
                AcquireParameters acq = NC.App.DB.LastAcquire();
                Detector det = NC.App.DB.Detectors.GetItByDetectorId(det0.Id.DetectorName);
                if (det == null)
                {
                    det = det0;
                    NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, "Creating new detector " + det.Id.DetectorName + " now");
                    // add fresh param instances to in-memory maps
                    NC.App.DB.NormParameters.Map.Add(det, new NormParameters());
                    NC.App.DB.UnattendedParameters.Map.Add(det, new UnattendedParameters());
                    NC.App.DB.BackgroundParameters.Map.Add(det, new BackgroundParameters());
                    NC.App.DB.AASSParameters.Map.Add(det, new AddASourceSetup());
                    NC.App.DB.HVParameters.Map.Add(det, new HVCalibrationParameters());
                    NC.App.DB.Detectors.Add(det); // add detector to in-memory list

                    IntegrationHelpers.PersistDetectorAndAssociations(det);
                }
                else
                {
                    // Apply any changes to GL, PD, HV (but not FA) to existing detector
                    if (SRParamUpdateNeeded(det.MultiplicityParams, det0.MultiplicityParams))
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, $"Modifying detector {det.Id.DetectorName} SR params");
                        det.SRParams.highVoltage = det0.SRParams.highVoltage;
                        det.SRParams.predelayMS = det0.SRParams.predelayMS;
                        det.SRParams.gateLengthMS = det0.SRParams.gateLengthMS;
                        NC.App.DB.UpdateDetectorParams(det);
                    }
                }
                INCCDB.Descriptor facdesc = null, mbadesc = null, mtldesc = null;
                if (!string.IsNullOrEmpty(fac))
                {
                    facdesc = NC.App.DB.Facilities.Get(fac);
                    if (facdesc == null)
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, "Creating " + fac + " facility");
                        facdesc = new INCCDB.Descriptor(fac, "Dataz creation source");
                        NC.App.DB.Facilities.Update(facdesc);
                    }
                }
                if (!string.IsNullOrEmpty(mba))
                {
                    mbadesc = NC.App.DB.MBAs.Get(mba);
                    if (mbadesc == null)
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, "Creating " + mba + " MBA");
                        mbadesc = new INCCDB.Descriptor(mba, "Dataz creation source");
                        NC.App.DB.MBAs.Update(mbadesc);
                    }
                }
                if (!string.IsNullOrEmpty(mtl))
                {
                    mtldesc = NC.App.DB.Materials.Get(mtl);
                    if (mtldesc == null)
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 32441, "Creating " + mtl + " material type");
                        mtldesc = new INCCDB.Descriptor(mtl, "Dataz creation source");
                        NC.App.DB.Materials.Update(mtldesc);
                    }
                }
                acq.MeasDateTime = DateTimeOffset.Now;
                DataZDetector = NC.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, det0.Id.DetectorName, true) == 0);
                DataZDetector.Id.source = ConstructedSource.DatazFile;
                acq.data_src = ConstructedSource.DatazFile;
                if (!string.IsNullOrEmpty(msg))
                {
                    acq.ending_comment = true;
                    acq.ending_comment_str = msg;
                }
                if (AcquistionChangeNeeded(acq, det.Id.DetectorName, mtl, fac, mba, interval))
                {
                    acq.run_count_time = interval;
                    acq.detector_id = string.Copy(det.Id.DetectorName);
                    acq.meas_detector_id = string.Copy(det.Id.DetectorName);
                    if (!string.IsNullOrEmpty(mtl))
                        acq.item_type = string.Copy(mtl);
                    if (facdesc != null)
                        acq.facility = facdesc;
                    if (mbadesc != null)
                        acq.mba = mbadesc;
                    INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime);
                    NC.App.DB.AddAcquireParams(sel, acq);
                }
                else    // update existing entry
                    NC.App.DB.UpdateAcquireParams(acq, det.ListMode);

                AcquistionStateChanged = true;

                NC.App.ControlLogger.TraceEvent(LogLevels.Info, 32444, $"The current detector/material now {acq.detector_id},{acq.item_type}");
                return true;
            }

            static bool SRParamUpdateNeeded(Multiplicity curmul, Multiplicity newmul)
            {
                return (curmul.SR.predelayMS != newmul.SR.predelayMS) || (curmul.SR.gateLengthMS != newmul.SR.gateLengthMS) || (curmul.SR.highVoltage != newmul.SR.highVoltage);  // dev note: FAType change not possible with current fixed SRType to FA type mapping for SRs
            }

            static bool AcquistionChangeNeeded(AcquireParameters acq, string det, string mtl, string fac, string mba, double interval)
            {
                return !acq.detector_id.Equals(det, StringComparison.OrdinalIgnoreCase) ||
                       !acq.meas_detector_id.Equals(det, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(fac) && !acq.facility.Name.Equals(fac, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(mba) && !acq.mba.Name.Equals(mba, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(mtl) && !acq.item_type.Equals(mtl, StringComparison.OrdinalIgnoreCase)) ||
                    interval != acq.run_count_time;
            }

            public bool AcquistionStateChanged { get; private set; }
            public uint MaxBins { get; private set; }

            public Detector DataZDetector { get; private set; }

            public struct Indexes
            {
                public int DTIdx, // start time of cycle
                    intervalIdx,  // cycle time interval
                    totalChannels, // computed number of channels for totals
                    totalsSRIdx,  // use instead of totalsIdx entry, if found 
                    totalsIdx, // raw totals summed
                    realsIdx, // Reals sum (also can be calculated)
                    totalIdx, // first index of totals channel values
                    realIdx, // Reals + Accidentals sum
                    accIdx, // Accidentals sum
                    multChannelsIdx,  // max mult bin count index
                    multIdx, // first index of multiplicity bins, assumes Mult0..n, MultAcc0 ..n appears AFTER multChannelsIdx
                    multNormAccIdx; // first index of mult+acc multiplicity bins, assumes Mult0..n, MultAcc0 ..n appears AFTER multChannelsIdx
            }
            Indexes dataIndices;

            // Cycle data columns as encountered in the wild:
            //Nr,DateTime,MeasurementInterval, Totals,Totals_SR, Reals&Acc,Acc, Reals, Total1,...,Total32,                MultiplicityChannels, Mult0,...,Mult19,           MultAcc0,...,MultAcc19
            //   DateTime,MeasurementInterval,                                         Total1,...,Total32, Reals&Acc,Acc, MultDataPoints,       Mult0,...,Mult19,...,Mult99,MultAcc0,...,MultAcc19,...,MultAcc99

            const string DT = "DateTime";
            const string MeasurementInterval = "MeasurementInterval";
            const string Total = "Total";
            const string Totals = Total + "s";
            const string Reals = "Reals";
            const string RealsPA = Reals + "&";
            const string Acc = "Acc";
            const string BinCounts0 = "MultDataPoints";
            const string BinCounts1 = "MultiplicityChannels";

            void DoDataHeader(string[] s)
            {
                int i = 0;
                while (i < s.Length && !s[i].StartsWith(DT)) i++;
                dataIndices.DTIdx = i;

                // get bin counts and totals counts and column ids
                i = 0;
                while (i < s.Length && !s[i].StartsWith(RealsPA)) i++;
                dataIndices.realIdx = i < s.Length ? i : -1;

                i = 0;
                while (i < s.Length && !s[i].StartsWith(Acc)) i++;
                dataIndices.accIdx = i < s.Length ? i : -1;


                i = 0;
                while (i < s.Length && !s[i].Equals(Reals)) i++;
                dataIndices.realsIdx = i < s.Length ? i : -1;

                i = 0;
                while (i < s.Length && !s[i].StartsWith(Totals + "_SR")) i++;  // !
                dataIndices.totalsSRIdx = i < s.Length ? i : -1;

                i = 0;
                while (i < s.Length && !s[i].StartsWith(Totals)) i++;  // !
                dataIndices.totalsIdx = i < s.Length ? i : -1;

                i = 0;
                while (i < s.Length && !s[i].StartsWith(Total + "1")) i++; // !
                dataIndices.totalIdx = i;
                while (i < s.Length && s[i].StartsWith(Total)) i++;
                string num = s[i - 1].Substring(Total.Length);
                int.TryParse(num, out dataIndices.totalChannels);

                i = 0;
                while (i < s.Length && !s[i].StartsWith(MeasurementInterval)) i++;
                dataIndices.intervalIdx = i < s.Length ? i : -1;

                i = 0;
                while (i < s.Length && !s[i].StartsWith(BinCounts1)) i++;
                if (i == s.Length)
                {
                    i = 0;
                    while (i < s.Length && !s[i].StartsWith(BinCounts0)) i++;
                }
                dataIndices.multChannelsIdx = i < s.Length ? i : -1;
                dataIndices.multIdx = i < s.Length ? i : -1;
            }

            public class Cycle
            {
                public DateTimeOffset DTO;
                public TimeSpan Duration;
                public ulong Singles, RealsPlusAccidentals, Accidentals, Reals;
                public ulong[] Totals;  // 8 used of 32 possible
                public uint BinLen;
                public ulong[] MultNormedAccBins;
                public ulong[] MultRABins;

                static public Cycle Parse(string[] s, Indexes ind, TimeSpan tz)
                {
                    Cycle c = new Cycle();
                    try
                    {
                        DateTime dt = DateTime.ParseExact(s[ind.DTIdx], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);  // DateTime col 2017-07-19 07:59:48
                        c.DTO = new DateTimeOffset(dt.Ticks, tz);
                    }
                    catch (FormatException)
                    {
                        c.DTO = DateTimeOffset.Now;
                    }

                    float dursecs = 0;
                    if (float.TryParse(s[ind.intervalIdx], out dursecs))
                        c.Duration = TimeSpan.FromSeconds(dursecs);// MeasurementInterval 
                    c.Totals = new ulong[ind.totalChannels];
                    for (int i = ind.totalIdx; i < (ind.totalIdx + ind.totalChannels); i++)
                        ulong.TryParse(s[i], out c.Totals[i - ind.totalIdx]);

                    if (ind.totalsIdx == -1 && ind.totalsSRIdx == -1)
                        foreach (ulong u in c.Totals)
                            c.Singles += u;
                    else if (ind.totalsSRIdx == -1)
                        ulong.TryParse(s[ind.totalsIdx], out c.Singles);  // a fallback if not SR entry
                    else
                        ulong.TryParse(s[ind.totalsSRIdx], out c.Singles);  // best entry to use here

                    ulong.TryParse(s[ind.realIdx], out c.RealsPlusAccidentals);
                    ulong.TryParse(s[ind.accIdx], out c.Accidentals);
                    if (ind.realsIdx >= 0) ulong.TryParse(s[ind.realsIdx], out c.Reals);
                    //c.Reals = c.RealsPlusAccidentals - c.Accidentals;

                    uint.TryParse(s[ind.multChannelsIdx], out c.BinLen);
                    c.MultRABins = new ulong[c.BinLen];
                    c.MultNormedAccBins = new ulong[c.BinLen];
                    for (int i = ind.multIdx + 1; i < (ind.multIdx + c.BinLen); i++)
                        ulong.TryParse(s[i], out c.MultRABins[i - (ind.multIdx + 1)]);
                    ind.multNormAccIdx = ind.multIdx + 1 + (int)c.BinLen;
                    for (int i = ind.multNormAccIdx; i < (ind.multNormAccIdx + c.BinLen); i++)
                        ulong.TryParse(s[i], out c.MultNormedAccBins[i - (ind.multNormAccIdx)]);

                    int idx = 0;
                    for (idx = (int)(c.BinLen - 1); idx > 0; idx--)
                    {
                        if (c.MultRABins[idx] != 0 || c.MultNormedAccBins[idx] != 0)
                            break;
                    }
                    if (idx < (c.BinLen-1))
                        c.BinLen = (uint)idx + 1;
                    return c;
                }
            }

            public List<Cycle> Cycles;
            public List<Plateau> Plateaux;

            #region scan

            enum PrimaryTag { HEADER, CONFIGURATION, SETTINGS, DATA, SEQUENCES, HISTORY, END };
            enum SecondaryTag { NONE, MAIN, SG, MULTIPLICITY, SEQUENCES, INCC };

            const char TagSignifier = '$';
            // HEADER, HISTORY
            Dictionary<string, string> Header;
            List<Tuple<string, string>> History;
            // SG, INCC, MULTIPLICITY, SEQUENCES, SEQUENCES:MULTIPLICITY,SETTINGS:MULTIPLICITY
            Dictionary<SecondaryTag, Dictionary<string, string>> Configuration, Settings;
            // MULTIPLICITY
            // MULTIPLICITY:Mult0 or MULTIPLICITY:Totals_SR
            // SEQUENCES:MULTIPLICITY:<Totals_SR>, 
            Dictionary<SecondaryTag, List<string[]>> Data, Sequences;

            public void ScanSections()
            {
                PrimaryTag tag = PrimaryTag.END; SecondaryTag stag = SecondaryTag.NONE; string ttag;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length < 1)
                        continue;
                    if (line[0] == TagSignifier)
                        PTag(line, out tag, out stag, out ttag);  // TODO: ttag (used for index labels) not used yet, but must be for full implementation
                    else
                    {
                        switch (tag)
                        {
                            case PrimaryTag.HEADER:
                                {
                                    Tuple<string, string> tp = BreakAssignmentLine(line);
                                    if (tp.Item1.Length > 0)  // skip blanks
                                        Header.Add(tp.Item1, tp.Item2);
                                }
                                break;
                            case PrimaryTag.CONFIGURATION:
                                switch (stag)
                                {
                                    case SecondaryTag.MULTIPLICITY:
                                    case SecondaryTag.SG:
                                    case SecondaryTag.SEQUENCES:
                                    case SecondaryTag.INCC:
                                    case SecondaryTag.MAIN:
                                        {
                                            Tuple<string, string> tp = BreakAssignmentLine(line);
                                            if (tp.Item1.Length > 0)  // skip blanks
                                                Configuration[stag].Add(tp.Item1, tp.Item2);
                                        }
                                        break;
                                }
                                break;
                            case PrimaryTag.SETTINGS:
                                switch (stag)
                                {
                                    case SecondaryTag.MULTIPLICITY:
                                        {
                                            Tuple<string, string> tp = BreakAssignmentLine(line);
                                            if (tp.Item1.Length > 0)  // skip blanks
                                                Settings[stag].Add(tp.Item1, tp.Item2);
                                        }
                                        break;
                                }
                                break;
                            case PrimaryTag.DATA:
                                switch (stag)
                                {
                                    case SecondaryTag.MULTIPLICITY:
                                        Data[stag].Add(CaptureCSVLine(line));
                                        break;
                                }
                                break;
                            case PrimaryTag.SEQUENCES:
                                switch (stag)
                                {
                                    case SecondaryTag.MULTIPLICITY:
                                        Sequences[stag].Add(CaptureCSVLine(line));
                                        break;
                                }
                                break;
                            case PrimaryTag.HISTORY:
                                History.Add(BreakAssignmentLine(line));
                                break;
                            case PrimaryTag.END:
                                break;
                        }
                    }
                }

            }

            void PTag(string s, out PrimaryTag tag, out SecondaryTag stag, out string tertiary)
            {
                tag = PrimaryTag.END;
                stag = SecondaryTag.NONE;
                tertiary = string.Empty;
                string[] r = s.Substring(1).Split(':');
                bool good = false;
                if (r.Length >= 1)
                    good = Enum.TryParse(r[0], out tag);
                if (r.Length >= 2)
                    good = Enum.TryParse(r[1], out stag);
                if (r.Length >= 3)
                    tertiary = r[2];
            }

            Tuple<string, string> BreakAssignmentLine(string line)
            {
                string[] r = line.Split('=');
                if (r.Length == 2)
                    return Tuple.Create(r[0], r[1]);
                else if (r.Length == 1)
                    return Tuple.Create(r[0], "");
                else if (r.Length > 2)
                {
                    string s = r[1];
                    for (int i = 2; i < r.Length; i++)
                        s += r[i];
                    return Tuple.Create(r[0],s);
                }
                else
                    return Tuple.Create("", "");
            }

            string[] CaptureCSVLine(string line)
            {
                string[] r = line.Split(',');
                return r;
            }

            #endregion scan

        }


    }
}
