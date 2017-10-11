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
using System;
using System.Collections.Generic;
using AnalysisDefs;
using DetectorDefs;
using Instr;
using NCC;
using NCCReporter;
using NCCTransfer;
using System.IO;
using System.Text;

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

            AcquireParameters orig_acq = new AcquireParameters(NC.App.Opstate.Measurement.AcquireState);
            Detector curdet = NC.App.Opstate.Measurement.Detector;
            AssaySelector.MeasurementOption mo = NC.App.Opstate.Measurement.MeasOption;

            if (curdet.AB.Unset)
            {
                ABKey abkey = new ABKey(curdet.MultiplicityParams, 512);  // NEXT: maxbins is arbitrary, just the first of many for VSR
                LMRawAnalysis.SDTMultiplicityCalculator.SetAlphaBeta(abkey, curdet.AB);
            }

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
                        ctrllog.TraceEvent(LogLevels.Error, 404, "This Dataz file has no good sequences.");
                    }
                    else
                    {
                        ctrllog.TraceInformation($"{mc.Cycles.Count} cycles and {mc.Plateaux.Count} sequences encountered in Dataz file {mc.Filename}");
                        System.Collections.IEnumerator iter = mc.GetSequences();
                        while (iter.MoveNext())
                        {
                            DatazFile.Plateau pla = (DatazFile.Plateau)iter.Current;
                            ResetMeasurement();
                            // update acq and then meas here
                            AcquireParameters newacq = ConfigureAcquireState(curdet, orig_acq, pla.Cycles[0].DTO, (ushort)pla.Num, mc.Filename);
                            IntegrationHelpers.BuildMeasurement(newacq, curdet, mo);
                            meas = NC.App.Opstate.Measurement;
                            meas.MeasDate = newacq.MeasDateTime;
                            meas.Persist();  // preserve the basic results record
                            meas.RequestedRepetitions = (ushort)pla.Num;
                            uint run_seconds = 0;
                            for (int i = 0; i < meas.RequestedRepetitions; i++)
                            {
                                /* run date and time (IAEA format) */
                                run_seconds = (uint)(i * (ushort)pla.Cycles[i].Duration.TotalSeconds); // from start time
                                AddMCSRDataCycle(i, run_seconds, pla.Cycles[i], meas, mc.Filename);
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

        void AddMCSRDataCycle(int run, uint run_seconds, DatazFile.Cycle c, Measurement meas, string fname)
        {
            Cycle cycle = new Cycle(datalog);
            try
            {
                cycle.UpdateDataSourceId(ConstructedSource.DatazFile, meas.Detector.Id.SRType,c.DTO,fname);
                cycle.seq = run;
                cycle.TS = c.Duration; 
                /* init run tests */
                cycle.SetQCStatus(meas.Detector.MultiplicityParams, QCTestStatus.None); // APluralityOfMultiplicityAnalyzers: creates entry if not found, expand from the single mult key from detector here
                meas.Add(cycle);
                /* singles, reals + accidentals, accidentals */
                cycle.Totals = c.Singles;// URGENT
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(meas.Detector.MultiplicityParams.FA, cycle.seq); // APluralityOfMultiplicityAnalyzers: expand when detector has multiple analyzers
                cycle.CountingAnalysisResults.Add(meas.Detector.MultiplicityParams, mcr);  // APluralityOfMultiplicityAnalyzers: expand when detector has multiple analyzers
                mcr.AB.TransferIntermediates(meas.Detector.AB);  // copy alpha beta onto the cycle's results 
                mcr.Totals = cycle.Totals;
                mcr.TS = cycle.TS;
                mcr.ASum = c.Accidentals;
                mcr.RASum = c.RealsPlusAccidentals;
                mcr.Scaler1.v = 0;
                mcr.Scaler2.v = 0;
                cycle.SinglesRate = c.Totals[0] / c.Duration.TotalSeconds;

                // assign the hits to a single channel (0)
                cycle.HitsPerChannel[0] = cycle.Totals;

                mcr.RawSinglesRate.v = cycle.SinglesRate;

                /* number of multiplicity values */
                mcr.MinBins = mcr.MaxBins = (ulong)c.BinLen;
                mcr.RAMult = new ulong[c.BinLen];
                mcr.NormedAMult = new ulong[c.BinLen];
                mcr.UnAMult = new ulong[c.BinLen]; // todo: compute this
                /* multiplicity values */
                for (ushort j = 0; j < c.BinLen; j++)
                {
                    mcr.RAMult[j] = c.MultBins[j];
                    mcr.NormedAMult[j] = c.MultAccBins[j];
                }
                ctrllog.TraceEvent(LogLevels.Verbose, 5439, "Cycle " + cycle.seq.ToString() + ((mcr.RAMult[0] + mcr.NormedAMult[0]) > 0 ? " max:" + mcr.MaxBins.ToString() : " *"));

            }
            catch (Exception e)
            {
                ctrllog.TraceEvent(LogLevels.Warning, 33085, "cycle processing error {0} {1}", run, e.Message);
            }
        }



        public class DatazFile : NeutronDataFile   // NEXT: this is only the MCSR specific variant, expand class definitions later
        {

            public StreamReader reader;


            public DatazFile()
            {

                Header = new List<Tuple<string, string>>();
                History = new List<Tuple<string, string>>();

                // SG, MULTIPLICITY, SEQUENCES, SEQUENCES:MULTIPLICITY 
                Configuration = new Dictionary<SecondaryTag, List<Tuple<string, string>>>();
                Configuration[SecondaryTag.SG] = new List<Tuple<string, string>>();
                Configuration[SecondaryTag.MULTIPLICITY] = new List<Tuple<string, string>>();
                Configuration[SecondaryTag.SEQUENCES] = new List<Tuple<string, string>>();  // config:seq:multiplicity

                // MULTIPLICITY
                Data = new Dictionary<SecondaryTag, List<string[]>>();
                Data[SecondaryTag.MULTIPLICITY] = new List<string[]>();
                // MULTIPLICITY:Mult0
                Sequences = new Dictionary<SecondaryTag, List<string[]>>();
                Sequences[SecondaryTag.MULTIPLICITY] = new List<string[]>(); // Mult0

                Cycles = new List<Cycle>();
                Plateaux = new List<Plateau>();
                indices = new Indexes();
            }

            public DateTimeOffset StartTime
            {
                get
                {
                    return DateTimeOffset.Now; // todo
                }
            }

            public TimeSpan RealTime
            {
                get
                {
                    return new TimeSpan(0, 0, 1000); // todo
                }
            }


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
                //public DateTime Start, End;
                public int StartIdx, EndIdx;
                public int Num;
                public double Avg;
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
                    return p;
                }
            }

            public void ProcessSections()
            {
                ExtractTimeZone();
                foreach (string[] s in Data[SecondaryTag.MULTIPLICITY])
                {
                    if (indices.totalChannels == 0)
                        DoHeader(s);
                    else
                        Cycles.Add(Cycle.Parse(s, indices, _TZ));
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
                Sequences[SecondaryTag.MULTIPLICITY] = new List<string[]>();
            }

            TimeSpan _TZ;
            void ExtractTimeZone()
            {
                float tz = 0;
                try
                {
                    string tzs = Header.Find(t => t.Item1.Equals("TimeZone")).Item2;
                    bool b = float.TryParse(tzs, out tz);
                }
                catch (Exception)
                {

                }
                _TZ = new TimeSpan(0,(int)(tz*60),0);
            }

            public struct Indexes
            {
                public int totalChannels, totalIdx, multIdx, multChannels, realIdx, accIdx, multAccIdx;
            }

            Indexes indices;

            const string Totals = "Total";
            const string RealsPA = "Reals&";
            const string Acc = "Acc";
            const string BinCounts = "MultDataPoints";
            void DoHeader(string[] s)
            {
                int i = 0;
                // get bin counts and totals counts and column ids
                while(!s[i].StartsWith(Totals)) i++;
                indices.totalIdx = i;
                while (s[i].StartsWith(Totals)) i++;
                string num = s[i - 1].Substring(Totals.Length);
                int.TryParse(num, out indices.totalChannels);
                while (!s[i].StartsWith(RealsPA)) i++;
                indices.realIdx = i;
                while (!s[i].StartsWith(Acc)) i++;
                indices.accIdx = i;
                while (!s[i].StartsWith(BinCounts)) i++;
                indices.multIdx = i;
            }

            public class Cycle
            {
                public DateTimeOffset DTO;
                public TimeSpan Duration;
                public ulong Singles, RealsPlusAccidentals, Accidentals;
                public ulong[] Totals;  // 8 used of 32 possible
                public int BinLen;
                public ulong[] MultAccBins;
                public ulong[] MultBins;

                static public Cycle Parse(string[] s, Indexes ind, TimeSpan tz)
                {
                    Cycle c = new Cycle();
                    DateTime dt = DateTime.ParseExact(s[0], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);  // DateTime col 2017-07-19 07:59:48
                    c.DTO = new DateTimeOffset(dt.Ticks, tz);
                    c.Duration = TimeSpan.ParseExact(s[1], "ss", System.Globalization.CultureInfo.InvariantCulture);// MeasurementInterval 
                    c.Totals = new ulong[ind.totalChannels];
                    for (int i = ind.totalIdx; i < (ind.totalIdx + ind.totalChannels); i++)
                        ulong.TryParse(s[i], out c.Totals[i - ind.totalIdx]);

                    foreach (ulong u in c.Totals)
                        c.Singles += u;
                    ulong.TryParse(s[ind.realIdx], out c.RealsPlusAccidentals);
                    ulong.TryParse(s[ind.accIdx], out c.Accidentals);
                    //c.Reals = c.RealsPlusAccidentals - c.Accidentals;

                    int.TryParse(s[ind.multIdx], out c.BinLen);
                    c.MultBins = new ulong[c.BinLen];
                    c.MultAccBins = new ulong[c.BinLen];
                    for (int i = ind.multIdx + 1; i < (ind.multIdx + c.BinLen); i++)
                        ulong.TryParse(s[i], out c.MultBins[i - (ind.multIdx + 1)]);
                    ind.multAccIdx = ind.multIdx + 1 + c.BinLen;
                    for (int i = ind.multAccIdx; i < (ind.multAccIdx + c.BinLen); i++)
                        ulong.TryParse(s[i], out c.MultAccBins[i - (ind.multAccIdx)]);

                    return c;
                }
            }

            public List<Cycle> Cycles;
            public List<Plateau> Plateaux;

            #region scan

            enum PrimaryTag { HEADER, CONFIGURATION, DATA, SEQUENCES, HISTORY, END };
            enum SecondaryTag { NONE, SG, MULTIPLICITY, SEQUENCES };

            const char TagSignifier = '$';
            // HEADER, HISTORY
            List<Tuple<string, string>> Header, History;
            // SG, MULTIPLICITY, SEQUENCES, SEQUENCES:MULTIPLICITY 
            Dictionary<SecondaryTag, List<Tuple<string, string>>> Configuration;
            // MULTIPLICITY
            // MULTIPLICITY:Mult0
            Dictionary<SecondaryTag, List<string[]>> Data, Sequences;

            public void ScanSections()
            {
                PrimaryTag tag = PrimaryTag.END; SecondaryTag stag = SecondaryTag.NONE;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line[0] == TagSignifier)
                    {
                        PTag(line, out tag, out stag);
                    }
                    else
                    {
                        switch (tag)
                        {
                            case PrimaryTag.HEADER:
                                Header.Add(BreakAssignmentLine(line));
                                break;
                            case PrimaryTag.CONFIGURATION:
                                switch (stag)
                                {
                                    case SecondaryTag.MULTIPLICITY:
                                    case SecondaryTag.SG:
                                    case SecondaryTag.SEQUENCES:
                                        Configuration[stag].Add(BreakAssignmentLine(line));
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

            void PTag(string s, out PrimaryTag tag, out SecondaryTag stag)
            {
                tag = PrimaryTag.END;
                stag = SecondaryTag.NONE;
                string[] r = s.Substring(1).Split(':');
                bool good = false;
                if (r.Length >= 1)
                    good = Enum.TryParse(r[0], out tag);
                if (r.Length >= 2)
                    good = Enum.TryParse(r[1], out stag);
            }

            Tuple<string, string> BreakAssignmentLine(string line)
            {
                string[] r = line.Split('=');
                if (r.Length > 1)
                    return Tuple.Create(r[0], r[1]);
                else if (r.Length > 0)
                    return Tuple.Create(r[0],"");
                else
                    return Tuple.Create("","");
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