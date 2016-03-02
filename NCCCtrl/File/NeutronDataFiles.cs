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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading;
using NCCReporter;
namespace NCCFile
{

    using NC = NCC.CentralizedState;

    // Classes to handle externalities and raw I/O of NCD and other related files, such as string-based sorted and unsorted pulse files.
    // For semantics of NCD file content see Analysis.RawDataTransform class

    // reading, caller must do buffering work
    // writing, including generating automatic file names

    // dev note: I hope to create a tuple stream source on top of these readers and writers.
    // This approach provides a stream API that operates on channel vectors, times or tuples of them, so 
    // we get three stream classes that can be used over files or the socket readers

    #region specific file container classes
    public interface INeutronDataFile
    {
        List<string> FileExtensions
        {
            get;
        }

        string ThisSuffix
        {
            get;
            set;
        }
        string PrefixPath
        {
            get;
            set;
        }
        string Filename
        {
            get;
            set;
        }
        LMLoggers.LognLM Log
        {
            get;
            set;
        }
        int Num
        {
            get;
            set;
        }
        string SourceName
        {
            get;
            set;
        }
        DateTimeOffset DTO
        {
            get;
            set;
        }
        // set DT from the file name
        void ExtractDateFromFilename();

        /// <summary>
        /// Generate a file name pertinent for the output file
        /// </summary>
        /// <param name="locPrefix">File-specific prefix to prepend the generated file name with, can be string.Empty</param>
        /// <param name="lm">Instrument number</param>
        void GenName(string locPrefix, int lm);

        bool CreateForWriting();

        void CloseWriter();

        void Delete();


    }


    public class NeutronDataFile : INeutronDataFile
    {
        public LMLoggers.LognLM Log
        {
            get;
            set;
        }
        public List<string> FileExtensions
        {
            get;
            set;
        }
        public int Num
        {
            get;
            set;
        }
        public string PrefixPath
        {
            get;
            set;
        }
        public string ThisSuffix
        {
            get;
            set;
        }
        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                SourceName = value.Substring(value.LastIndexOf("\\") + 1);// Remove path information from string.
            }
        }
        public DateTimeOffset DTO
        {
            get;
            set;
        }

        public FileStream stream;

        public int id = 0;

        public string SourceName
        {
            get;
            set;
        }

        public bool Active { get { return stream != null; } }

        public NeutronDataFile()
        {
            stream = null;
        }

        public virtual bool OpenForReading(string filename = null)
        {
            try
            {
                if (filename != null)
                {
                    this.Filename = filename;
                    if (!File.Exists(filename))
                        return false;
                    if (string.IsNullOrEmpty(SourceName))
                        SourceName = filename.Substring(filename.LastIndexOf("\\") + 1);// Remove path information from string.
                }
                if (!File.Exists(this.Filename))
                {
                    if (Log != null) Log.TraceEvent(LogLevels.Warning, 112, "Ooof! File does not exist or cannot be opened: " + this.Filename);
                    return false;
                }
                stream = File.OpenRead(this.Filename);
                if (Log != null) Log.TraceEvent(LogLevels.Info, 112, "Opening file " + this.Filename + ", " + stream.Length + " bytes");

                ExtractDateFromFilename();

                return true;
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
                return false;
            }
        }
        public virtual void CloseWriter()
        {
        }
        public virtual void CloseStream()
        {
            if (Log != null) Log.TraceEvent(LogLevels.Verbose, 115, "Closing stream for " + Filename);
            try
            {
                if (stream != null)
                    stream.Close();
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }


        //  when encountering the same file name, create a file name with a millisecond addendum
        public virtual bool CreateForWriting()
        {
            try
            {
                if (File.Exists(Filename))
                    ConstructFullPathName("[" + DateTime.Now.Millisecond.ToString() + "]"); // use sub-second marker 
                if (Log != null) Log.TraceEvent(LogLevels.Info, 111, "Creating new file: " + Filename);
                stream = File.Create(Filename);
                return true;
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
                return false;
            }
        }

        public virtual void ConstructFullPathName(string opt = "")
        {
            Filename = PrefixPath + "_" + Num + opt; //dev note: good to provide external config approach to specify output file pattern
            // opt is really id
        }
        public void GenName(string locPrefix, int lm)
        {
            PrefixPath = locPrefix;
            Num = lm;
            ConstructFullPathName();
        }

        public void Delete()
        {
            try
            {
                File.Delete(Filename);
            }
            catch (Exception ex)
            {
                if (Log != null) Log.TraceException(ex);
            }
            Filename = null;
        }

        public virtual void ExtractDateFromFilename()
        {
            DTO = FromFilename(this.Filename);
        }

        public string GeneratePathWithDTPrefix()
        {
            string dtime = DTO.ToString("yyyy_MM_dd_HHmmss"); // dev note: good to provide external config approach to specify output file pattern 
            int idx = Filename.LastIndexOf("\\");
            string loc = "";
            if (idx > -1)
                loc = Filename.Remove(idx); // Remove file name information from string.

            if (!loc.EndsWith("\\"))  // might be some URI interface to abstract this better, this is platform dependent
            {
                loc += "\\";
            }

            return loc + dtime;

        }

        public string GenerateDerivedName()
        {

            string currentDataFilenamePrefix = GeneratePathWithDTPrefix() + " " + SourceName;

            return currentDataFilenamePrefix;
        }

        // todo: pull id and milliseconds from file name too
        public static DateTime FromFilename(string fname)
        {

            DateTime dt;
            string name = fname.Substring(fname.LastIndexOf("\\") + 1);
            //yyyy_MM_dd_HHmmss_#.ncd
            //yyyy_MM_dd_HHmmss[mmm]_#.ncd
            //yyyy_MM_dd HHmmss.ncd      gen2
            string[] split = name.Split(new Char[] { ' ', '_', '[', ']', '.' },
                                        StringSplitOptions.RemoveEmptyEntries);  // [mmm]_ returns a "", remove it

            bool bad = (split.Length < 5 || split.Length > 7);   // check counts first
            if (bad) // use file last write time
            {
                dt = File.GetLastWriteTime(fname);
            }
            else
            {
                string[] sub = new string[4];
                Array.Copy(split, sub, 4);
                string newname = string.Join(" ", sub);

                try
                {
                    dt = DateTime.ParseExact(newname, "yyyy MM dd HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    dt = File.GetLastWriteTime(fname);
                }
            }
            return dt;

        }
    }

    public class TestDataFile : NeutronDataFile
    {

        public TextWriter writer;
        public StreamReader reader;

        public TestDataFile()
        {
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
            if (reader != null)
            {
                reader.Close();
                base.CloseStream();
            }
        }

        //  when encountering the same file name, create a file name with a millisecond addendum
        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
                writer = new StreamWriter(stream);
            return ok;
        }
        public void Write(double d)
        {
            if (writer != null)
                writer.WriteLine(d);
        }
        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".dat";
        }

        public void Write(ulong[] buffer)
        {
            if (writer != null)
                writer.WriteLine(buffer.ToString());// todo: ??
        }

        public override void CloseWriter()
        {
            if (Log != null) Log.TraceEvent(LogLevels.Verbose, 117, "Closing writer for " + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }

        public override void ExtractDateFromFilename()
        {

            string name = Filename.Substring(Filename.LastIndexOf("\\") + 1);
            //detid assaytype app yyyyMMdd_HHmmss_#.dat
            string[] split = name.Split(new Char[] { ' ', '_', '[', ']', '.' },
                                        StringSplitOptions.RemoveEmptyEntries);  // [mmm]_ returns a "", remove it

            bool bad = (split.Length < 5 || split.Length > 7);   // check counts first
            if (bad) // use file last write time
            {
                DTO = File.GetLastWriteTime(Filename);
            }
            else
            {
                try
                {
                    DTO = DateTime.ParseExact(split[3], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    DTO = File.GetLastWriteTime(Filename);
                }
            }
        }


    }

    ///  dev note: we are assuming the list is sorted during enumeration based on file naming scheme 
    ///  dev note: regex string sPattern = "^\\d{4}_\\d{2}_\\d{2}_\\d{6}{\[\\\d{3}]}\.ncd$";
    public class NCDFile : NeutronDataFile
    {

        public BinaryWriter writer;
        public BinaryReader reader;


        public NCDFile()
        {
            FirstEventTimeInTics = 0; TotalDups = 0; TotalEvents = 0; LastTimeInTics = 0;
        }


        public override bool OpenForReading(string filename = null)
        {
            bool ok = base.OpenForReading(filename);
            if (ok)
                reader = new BinaryReader(stream);
            return ok;
        }

        public void CloseReader()
        {
            if (reader == null)
                return;
            reader.Close();
            base.CloseStream();
        }


        //  when encountering the same file name, create a file name with a millisecond addendum
        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
                writer = new BinaryWriter(stream);
            return ok;
        }

        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".ncd";
        }

        public void Write(byte[] buffer, int index, int count)
        {
            if (writer != null)
                writer.Write(buffer, index, count);
        }

        public override void CloseWriter()
        {
            base.CloseWriter();
            if (Log != null) Log.TraceEvent(LogLevels.Verbose, 117, "Closing writer for " + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }


        public long FirstEventTimeInTics;
        public ulong TotalDups;
        public ulong TotalEvents;
        public long LastTimeInTics;

        /// <summary>
        /// single channel pulse stream only
        /// </summary>
        /// <param name="list"></param>
        /// <param name="chnbytes"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public string TransferToTraditionalNCDFormat(IEnumerable<double> list, byte[] chnbytes, ulong num)
        {
            Int64 ROllOverTics = 0x100000000;

            string issue = string.Empty;
            ulong dups = 0, events = 0, timei = 0;

            Double lasttime = 0, timeR8tics, timeR8ticsRnd;
            UInt32 timeU4, timeU4B4 = 0;
            Int64 timeI8tics = 0, divrem = 0, circuits = 0;
            double time = 0;
            double firstread = 0;

            try
            {

                byte[] timebytes;
                byte[] timeswapped = new byte[4];
                foreach (double timesre in list)
                {
                    time = timesre;
                    events++;

                    if (events > num)
                    {
                        events--;
                        break;
                    }

                    timeR8tics = time / 10.0; // from 10e-8 shakes to 10e-7 tics
                    timeR8ticsRnd = Math.Round(timeR8tics, 2, MidpointRounding.AwayFromZero);
                    timeI8tics = Convert.ToInt64(timeR8ticsRnd);

                    if (events == 1)
                    {
                        if (FirstEventTimeInTics == 0)
                            FirstEventTimeInTics = timeI8tics;
                        firstread = time;
                    }

                    if (timeI8tics >= ROllOverTics)  // first 429.4967295... secs are handled quickly, but here we must do a moddiv
                    {
                        circuits = Math.DivRem(timeI8tics, ROllOverTics, out divrem);
                        timeU4 = Convert.ToUInt32(divrem);
                    }
                    else
                    {
                        timeU4 = Convert.ToUInt32(timeI8tics);
                    }

                    if (lasttime > time) // ooops! 
                    {
                        throw new Exception(string.Format("{0}, {1} ({2}, {3}) out-of-order, you forgot to sort", lasttime, time, timeU4B4, timeU4));
                    }
                    else if (timeU4B4 == timeU4) // rounding created a duplicate ! 
                    {
                        dups++;
                        Log.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate event {0} at time {1} (due to rounding)", events, timeU4B4);
                        continue;
                    }

                    timebytes = BitConverter.GetBytes(timeU4);
                    timeswapped[0] = timebytes[3]; // byte 4 goes to byte 1
                    timeswapped[1] = timebytes[2]; // byte 3 goes to byte 2
                    timeswapped[2] = timebytes[1]; // byte 2 goes to byte 3
                    timeswapped[3] = timebytes[0]; // byte 1 goes to byte 4

                    Write(chnbytes, 0, 4);
                    Write(timeswapped, 0, 4);
                    timei++;
                    timeU4B4 = timeU4;
                    lasttime = time;
                }
                Log.TraceEvent(LogLevels.Verbose, 3338, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) into {5} ({6} duplicates skipped)", events, timei, firstread, lasttime, circuits, Filename, dups);

            }
            catch (Exception e)
            {
                Log.TraceEvent(LogLevels.Verbose, 3339, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) into {5} ({6} duplicates skipped)", events, timei, firstread, lasttime, circuits, Filename, dups);
                Log.TraceEvent(LogLevels.Warning, 3363, "Error parsing pulses encountered '{0}'", e.Message);
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                issue = e.Message;
            }
            TotalEvents += events;
            TotalDups += dups;
            LastTimeInTics = timeI8tics;
            return issue;
        }

        public string TransferToTraditionalNCDFormat(IEnumerable<double> times, UInt32[] chns, ulong num, bool combineDuplicateHits = false)
        {

            Int64 ROllOverTics = 0x100000000;
            string issue = string.Empty;
            ulong dups = 0, events = 0, timei = 0;

            Double lasttime = 0, timeR8tics, timeR8ticsRnd;
            UInt32 timeU4, timeU4B4 = 0;
            Int64 timeI8tics = 0, divrem = 0, circuits = 0;
            double firstread = 0;

            try
            {
                byte[] timebytes;
                byte[] timeswapped = new byte[4];
                byte[] channelbytes;
                byte[] channelswapped = new byte[4];
                uint chann_els = 0;
                foreach (uint time in times)
                {
                    events++;

                    if (events > num)
                    {
                        events--;
                        break;
                    }

                    timeR8tics = time / 10.0; // from 10e-8 shakes to 10e-7 tics
                    timeR8ticsRnd = Math.Round(timeR8tics, 2, MidpointRounding.AwayFromZero);
                    timeI8tics = Convert.ToInt64(timeR8ticsRnd);

                    if (events == 1)
                    {
                        if (FirstEventTimeInTics == 0)
                            FirstEventTimeInTics = timeI8tics;
                        firstread = time;
                    }

                    if (timeI8tics >= ROllOverTics)  // first 429.4967295... secs are handled quickly, but here we must do a moddiv
                    {
                        //if (events > 4294100)
                        //{
                        //    string output = events.ToString() + " ";
                        //    for (int ctr = chnbytes.Length - 1; ctr >= 0; ctr--)
                        //        output += string.Format("{0:X2} ", chnbytes[ctr]);
                        //    //output += ",";
                        //    for (int ctr = timeswapped.Length - 1; ctr >= 0; ctr--)
                        //        output += string.Format("{0:X2} ", timeswapped[ctr]);
                        //    output += " |";
                        //    for (int ctr = timebytes.Length - 1; ctr >= 0; ctr--)
                        //        output += string.Format("{0:X2} ", timebytes[ctr]);
                        //    pf.Log.TraceEvent(LogLevels.Info, 773773773, output + "| " + timeU4.ToString());
                        //}
                        //pf.Log.TraceEvent(LogLevels.Info, 773773773, "stop here!"); events--;
                        //break;
                        circuits = Math.DivRem(timeI8tics, ROllOverTics, out divrem);
                        timeU4 = Convert.ToUInt32(divrem);

                    }
                    else
                    {
                        timeU4 = Convert.ToUInt32(timeI8tics);
                    }

                    if (lasttime > time) // ooops! 
                    {
                        throw new Exception(string.Format("{0}, {1} ({2}, {3}) out-of-order, you forgot to sort", lasttime, time, timeU4B4, timeU4));
                    }
                    else if (timeU4B4 == timeU4) // rounding created a duplicate ! 
                    {
                        dups++;
                        if (combineDuplicateHits)
                        {
                            chann_els |= chns[events];
                            Log.TraceEvent(LogLevels.Verbose, 3331, "Combining hits [{0:x8}] from duplicate event {1} ({2})[{3:x8}] at time {4}", chann_els, timei, events, chns[events], timeU4B4);
                        }
                        else
                            Log.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate event {0} [{1:x8}] at time {2} (due to rounding)", events, chns[events], timeU4B4);
                        continue;
                    }

                    timebytes = BitConverter.GetBytes(timeU4);
                    timeswapped[0] = timebytes[3]; // byte 4 goes to byte 1
                    timeswapped[1] = timebytes[2]; // byte 3 goes to byte 2
                    timeswapped[2] = timebytes[1]; // byte 2 goes to byte 3
                    timeswapped[3] = timebytes[0]; // byte 1 goes to byte 4

                    chann_els |= chns[events];
                    channelbytes = BitConverter.GetBytes(chann_els);
                    channelswapped[0] = channelbytes[3]; // byte 4 goes to byte 1
                    channelswapped[1] = channelbytes[2]; // byte 3 goes to byte 2
                    channelswapped[2] = channelbytes[1]; // byte 2 goes to byte 3
                    channelswapped[3] = channelbytes[0]; // byte 1 goes to byte 4

                    Write(channelswapped, 0, 4);
                    Write(timeswapped, 0, 4);
                    timei++;
                    timeU4B4 = timeU4;
                    lasttime = time;
                    chann_els = 0;
                    //Log.TraceEvent(LogLevels.Verbose, 777, "{0} {1} [{2:x8}]", events, timeU4, chann_els);

                }

                Log.TraceEvent(LogLevels.Verbose, 3338, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) into {5} ({6} duplicates skipped)", events, timei, firstread, lasttime, circuits, Filename, dups);
            }
            catch (Exception e)
            {
                Log.TraceEvent(LogLevels.Verbose, 3339, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) into {5} ({6} duplicates skipped)", events, timei, firstread, lasttime, circuits, Filename, dups);
                Log.TraceEvent(LogLevels.Warning, 3363, "Error parsing pulses encountered '{0}'", e.Message);
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                issue = e.Message;
            }
            TotalEvents += events;
            TotalDups += dups;
            LastTimeInTics = timeI8tics;
            return issue;
        }

        Analysis.StreamStatusBlock sb;
        public void CustomStatusBlock(string HW, string devicename, string source, string message)
        {
            double durationTics = (double)(LastTimeInTics - FirstEventTimeInTics);
            if (durationTics <= 0)
                durationTics = 1;

            string sourcename = source.Substring(source.LastIndexOf("\\") + 1);

            string s2 = string.Format("Status\r\nSingle\r\nDuration = {0:F6} s\r\ntriggers = {1}\r\ntotals   = {2}\r\noverflow = 0xfeedacat, FALSE\r\npackets  = NA\r\nrate     = {3:F6} ~samples/sec\r\n",
                durationTics / 10e7, TotalEvents, TotalEvents, ((double)TotalEvents / durationTics) / 10e-7);

            string s3 = string.Format("rate     = NA Mbits/sec\r\nbin size = 100 ns\r\nbuf sep  = 0 ms\r\nname     = {0} : {1}\r\nLMC      = f00dbeef\r\nsource   = {2}\r\n", HW, devicename, sourcename);
            string s = s2 + s3;
            if (!string.IsNullOrEmpty(message))
            {
                s += string.Format("Extra    = {0}\r\n", message);
            }
            sb = new Analysis.StreamStatusBlock();
            sb.Encode(s);

            Log.TraceEvent(LogLevels.Verbose, 3333, "Converted {0} events between {1} and {2} tics ({3} duplicates skipped)", TotalEvents, FirstEventTimeInTics, LastTimeInTics, TotalDups);

        }

        public byte[] SwapBytes(Int32 token)
        {
            byte[] bytes;
            byte[] swapped = new byte[4];
            bytes = BitConverter.GetBytes(token);
            swapped[0] = bytes[3]; // byte 4 goes to byte 1
            swapped[1] = bytes[2]; // byte 3 goes to byte 2
            swapped[2] = bytes[1]; // byte 2 goes to byte 3
            swapped[3] = bytes[0]; // byte 1 goes to byte 4

            return swapped;
        }

        public void WriteTagAndStatusBlock()
        {
            UInt32 zero = 0;
            Int32 len = sb.msglen;
            writer.Write(zero); writer.Write(zero);
            byte[] bytes = SwapBytes(len);  // swap bytes!
            writer.Write(bytes);
            writer.Write(sb.byterep);
        }

    }

    public class PTRChannelFile : NeutronDataFile
    {

        public BinaryWriter writer;
        public BinaryReader reader;
        public int thisread;
        public long read, fulllen;

        public ushort CycleNumber;

		public PTRChannelFile()
        {
   
        }

        // PTR-32 chn files use 16-bit short integers between 0-31 to represent individual channel hits
        public void Write(uint[] buffer, int index, int count)
        {
            if (writer == null)
                return;
            for (int i = 0; i < count; i++)
            {
                uint v = buffer[index + i];
                // experimental speed up  approach, check for multiple, then write the individual bytes in a 0-31 loop
                //int c = BitsSetTable256[v & 0xff] +
                //        BitsSetTable256[(v >> 8) & 0xff] +
                //        BitsSetTable256[(v >> 16) & 0xff] +
                //        BitsSetTable256[v >> 24];
                //if (c > 1)
                //{
                //    byte b = 0;
                //    for (b = 0; b < 32; b++)
                //    {
                //        if ((v & (1 << b)) != 0)
                //             writer.Write(b);
                //    }
                //   
                //}
                //else
                //{
                // dev note: there must be a faster way to walk the channel byte, find it and combine with a larger block encoding and write buffer scheme
                byte b = 0;
                for (b = 0; b < 32; b++)  // 0 - 31 loop, likely too slow
                {
                    if ((v & (1 << b)) != 0)
                        writer.Write(b);
                }

                //}

            }
        }

        public void Write(List<uint> buffer, int index, int count)
        {
            if (writer == null)
                return;
            for (int i = 0; i < count; i++)
            {
                uint v = buffer[index + i];
                // experimental speed up  approach, check for multiple, then write the individual bytes in a 0-31 loop
                //int c = BitsSetTable256[v & 0xff] +
                //        BitsSetTable256[(v >> 8) & 0xff] +
                //        BitsSetTable256[(v >> 16) & 0xff] +
                //        BitsSetTable256[v >> 24];
                //if (c > 1)
                //{
                //    byte b = 0;
                //    for (b = 0; b < 32; b++)
                //    {
                //        if ((v & (1 << b)) != 0)
                //             writer.Write(b);
                //    }
                //   
                //}
                //else
                //{
                // dev note: there must be a faster way to walk the channel byte, find it and combine with a larger block encoding and write buffer scheme
                byte b = 0;
                for (b = 0; b < 32; b++)  // 0 - 31 loop, likely too slow
                {
                    if ((v & (1 << b)) != 0)
                        writer.Write(b);
                }

                //}

            }

        }

        public override bool OpenForReading(string filename = null)
        {
            bool ok = base.OpenForReading(filename);
            if (ok)
            {
                reader = new BinaryReader(stream);
                fulllen = stream.Length;
            }
            return ok;
        }

        public void CloseReader()
        {
            if (reader != null)
            {
                reader.Close();
                base.CloseStream();
            }
        }


        //  when encountering the same file name, create a file name with a millisecond addendum
        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
            {
                writer = new BinaryWriter(stream);
                // 2015 Jan. 3 see https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetTable
                //BitsSetTable256 = new byte[256];
                //BitsSetTable256[0] = 0;
                //for (ushort i = 0; i <= 255; i++)
                //{
                //    byte b = (byte)(i & 0x1);
                //    BitsSetTable256[i] = (byte)(b + BitsSetTable256[i / 2]);
                //}
            }
            return ok;
        }

        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".chn";
        }

        public void Write(byte[] buffer, int index, int count)
        {
            if (writer != null)
                writer.Write(buffer, index, count);
        }

        public override void CloseWriter()
        {
            if (writer != null && Log != null) Log.TraceEvent(LogLevels.Verbose, 117, "Closing writer for " + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
				writer = null;
				stream = null;
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }

        public override void ExtractDateFromFilename()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            PTRFilePair.GetDateFromName(Filename, ref dt, ref CycleNumber);
            DTO = dt;
        }
    }
    public class PTREventFile : NeutronDataFile
    {

        public BinaryWriter writer;
        public BinaryReader reader;
        byte[] header;
        public string headerstr;
        public int thisread;
        public long read, eventsectionlen;
        ulong mark;

        public UInt16 CycleNumber;
        public const int HeaderLength = 0x1000;
        public int ReportedCountTimeSecs = 0;

        public PTREventFile()
        {
            mark = 0;
            header = new byte[HeaderLength];
        }

        public int ReadHeader()
        {
            eventsectionlen = stream.Length;
            thisread = stream.Read(header, 0, 0x1000);
            read = thisread;
            eventsectionlen -= thisread;
            //count time is in here at 
            headerstr = bytestoASCIIchars(header, 0x1000, thisread);
            // This parses out the count time from the file header per Huzsti's definition. w/TryParse, should default to zero
            // Currently, no check at end is implemented. HN 10.16.2015
            Regex reg = new Regex("^@(\\d+).+@(\\d+)");
            if (reg.IsMatch (headerstr))
            {
                Match m = reg.Match(headerstr);
                ReportedCountTimeSecs = 0;
                
                Int32.TryParse(m.Groups[2].Value, out ReportedCountTimeSecs);
            }
            return thisread;
        }

        public int EventsYetToRead()
        {
            return (int)(stream.Length - read) / sizeof(UInt32);
        }
        public override bool OpenForReading(string filename = null)
        {
            bool ok = base.OpenForReading(filename);
            if (ok)
                reader = new BinaryReader(stream);
            return ok;
        }

        public void CloseReader()
        {
            if (reader != null)
            {
                reader.Close();
                base.CloseStream();
            }
        }


        //  when encountering the same file name, create a file name with a millisecond addendum
        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
                writer = new BinaryWriter(stream);
            mark = 0;
            return ok;
        }

        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".bin";
        }

        public void WriteHeader(string start)
        {
            header.Initialize();  // nulls, 
            string s = ("@0PulseTrainRecorder X7.?"); // todo: version, serial #, author, comment, etc. figure out what the text should say by examining files from various devices
            Byte[] sa = Encoding.ASCII.GetBytes(s);
            Byte[] sb = Encoding.ASCII.GetBytes(start);
            Array.Copy(sa, header, sa.Length);
            Array.Copy(sb, 0, header, 0x20, sb.Length); // start at 32 byte boundary for firmware string
            Write(header, 0, header.Length);
        }

        public void Write(byte[] buffer, int index, int count)
        {
            if (writer != null)
                writer.Write(buffer, index, count);
        }
        public void Write(ulong[] buffer, int index, int count)
        {
            if (writer == null)
                return;
            uint ubuff = 0;
            int i = 0;
            for (i = 0; i < count; i++) // convert back down to uint delta times
            {
                ubuff = (uint)((buffer[index + i] - mark) & 0x0000fffful);
                writer.Write(ubuff);
                mark = buffer[index + i];
            }           
        }
        public void Write(List<ulong> buffer, int index, int count)
        {
            if (writer == null)
                return;
            uint ubuff = 0;
            int i = 0;
            for (i = 0; i < count; i++) // convert back down to uint delta times
            {
                ubuff = (uint)((buffer[index + i] - mark) & 0x0000fffful);
                writer.Write(ubuff);
                mark = buffer[index + i];
            }
        }

        public override void CloseWriter()
        {
            if (writer != null && Log != null) Log.TraceEvent(LogLevels.Verbose, 117, "Closing writer for " + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
				writer = null;
				stream = null;
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }

        public string bytestoASCIIchars(byte[] bdata, int len, int max)
        {
            int i;
            char dchar;
            if (len > max) len = max;
            StringBuilder text = new StringBuilder();
            for (i = 0; i < len; i++)
            {
                dchar = (char)bdata[i];
                // replace 'non-printable' chars with a '.'.
                if ((dchar == '\r') || (dchar == '\n') || (dchar == '\v'))
                {
                    dchar = '\\';
                }
                else
                    if (Char.IsControl(dchar))
                    {
                        dchar = '.';
                    }
                text.Append(dchar);
            }
            return text.ToString();
        }

        public int ReadUInt32Array(UInt32[] target, int startidx, int maxuints)
        {
            int i = 0;
            for (i = 0; i < maxuints; i++)
            {
                target[startidx + i] = reader.ReadUInt32();
            }
            return i * sizeof(UInt32);
        }

        public override void ExtractDateFromFilename()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            PTRFilePair.GetDateFromName(Filename, ref dt, ref CycleNumber);
            DTO = dt;
        }

    }
    public class PTRFilePair : INeutronDataFile
    {

        public PTRChannelFile Channels;
        public PTREventFile Events;
        public bool SoloBinFile;
        public ushort CycleNumber;
        public string FirmwareIdent // this is from instrument.m_device.FirmwareVersion 
        {
            get;
            set;
        }
        public DateTimeOffset DTO
        {
            get;
            set;
        }
        public List<string> FileExtensions
        {
            get;
            set;
        }

        public string PrefixPath
        {
            get;
            set;
        }
        public string ThisSuffix
        {
            get;
            set;
        }
        public string Filename
        {
            get;
            set;
        }
        public LMLoggers.LognLM Log
        {
            get;
            set;
        }
        public int Num
        {
            get;
            set;
        }
        public string SourceName
        {
            get;
            set;
        }


        public PTRFilePair()
        {
            Events = new PTREventFile();
            Channels = new PTRChannelFile();
        }


        public bool PairEntryFileExtension(string ext)
        {
            return Filename.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool OpenForReading(string filename = null)
        {
            if (filename == null)
                filename = Filename;
            int dot = filename.LastIndexOf('.');
            string chnfile, binfile;
            if (dot < 0)
            {
                chnfile = filename + ".chn";
                binfile = filename + ".bin";
            }
            else
            {
                chnfile = filename.Remove(dot) + ".chn";
                binfile = filename.Remove(dot) + ".bin";
            }
            SoloBinFile = !Channels.OpenForReading(chnfile);
            return Events.OpenForReading(binfile);
        }

        public void CloseReader()
        {
            Events.CloseReader();
            Channels.CloseReader();
        }


        public bool CreateForWriting()
        {
            bool res = Events.CreateForWriting();
            if (res)
            {
                Channels.CreateForWriting();
                Events.WriteHeader(FirmwareIdent);
            }
            return res;
        }

        public void ConstructFullPathName(string opt = "")
        {
        }

        public void Write(byte[] buffer, int index, int count)
        {
        }

        public void CloseWriter()
        {
            Events.CloseWriter();
            Channels.CloseWriter();
        }

        public void GenName(string locPrefix, int lm)
        {
            Events.GenName(locPrefix, lm);
            Channels.Filename = Events.Filename.Replace(".bin", ".chn");
			Filename = Events.Filename;
        }

        public void Delete()
        {
            Events.Delete();
            Channels.Delete();
        }
        public void ExtractDateFromFilename()
        {
            DateTimeOffset dt = DateTimeOffset.Now;
            PTRFilePair.GetDateFromName(Filename, ref dt, ref CycleNumber);
            DTO = dt;
        }

        public static void GetDateFromName(string fname, ref DateTimeOffset DTO, ref UInt16 CycleNumber)
        {

            DateTimeOffset dt;
            string name = fname.Substring(fname.LastIndexOf("\\") + 1);
            string[] split = name.Split(new Char[] { '-', '_', '.' }, StringSplitOptions.RemoveEmptyEntries);

            //yyMMdd_HHmm-cycle #, where yyMMdd_HHmm is the measurement start time, and the trailing number is the cycle number

            bool bad = (split.Length != 4);   // check counts first
            if (bad) // use file last write time
            {
                dt = File.GetLastWriteTime(fname);
            }
            else
            {
                try
                {
                    DateTime dtyyMMdd = DateTime.ParseExact(split[0], "yyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    TimeSpan tsHHmmsss = TimeSpan.ParseExact(split[1], "hhmm", System.Globalization.CultureInfo.InvariantCulture);
                    CycleNumber = Convert.ToUInt16(split[2]);
                    dt = new DateTime(dtyyMMdd.Ticks + tsHHmmsss.Ticks);
                }
                catch (FormatException)
                {
                    dt = File.GetLastWriteTime(fname);
                }
            }
            DTO = dt;
        }


    }

	public class TimestampOverflowException : Exception { }
	public class SometingWrongHierException : Exception { public SometingWrongHierException(string message) : base(message) {  } }

	public class MCAFile : NeutronDataFile
	{

		internal class MCATimestamp
		{
			uint fileBlockSize;  // from subject file's UsedMemorySize field

			internal MCATimestamp(uint blockSize)
			{
				fileBlockSize = blockSize;
			}
			protected IEnumerable<ulong> EnumerateTimestampsUsingMethod0(BinaryReader br)
			{
				byte[] bytes = new byte[8];
				long startPosition = br.BaseStream.Position;
				bool usePreviousTimestamp = false;
				ulong previousTimestamp = 0;
				while (br.BaseStream.Position < startPosition + fileBlockSize)
				{
					ulong timestamp = (ulong)ReadVariableLengthUInt(br, bytes);
					if (timestamp + 1 == 0x40C30C0)
					{ // maximum value, means no event occurred...
						previousTimestamp += timestamp;
						usePreviousTimestamp = true;
					} 
					else
					{
						if (usePreviousTimestamp)
						{
							if (timestamp > ulong.MaxValue - previousTimestamp)
							{
								throw new TimestampOverflowException();
							}
							timestamp += previousTimestamp;
							previousTimestamp = 0;
							usePreviousTimestamp = false;
						}
						yield return timestamp;
					}
				}
			}
			IEnumerable<ulong> EnumerateTimestampsUsingMethod1(BinaryReader br)
			{
				long startPosition = br.BaseStream.Position;
				bool usePreviousTimestamp = false;
				ulong previousTimestamp = 0;
				while (br.BaseStream.Position < startPosition + fileBlockSize) {
					ulong timestamp = (ulong)br.ReadByte();
					if (timestamp == 0xff) {
						previousTimestamp += timestamp;
						usePreviousTimestamp = true;
					} else {
						if (usePreviousTimestamp) {
							if (timestamp > ulong.MaxValue - previousTimestamp) {
								throw new TimestampOverflowException();
							}
							timestamp += previousTimestamp;
							previousTimestamp = 0;
							usePreviousTimestamp = false;
						}
						yield return timestamp;
					}
				}
			}
			IEnumerable<ulong> EnumerateTimestampsUsingMethod2(BinaryReader br)
			{
				long startPosition = br.BaseStream.Position;
				bool usePreviousTimestamp = false;
				ulong previousTimestamp = 0;
				while (br.BaseStream.Position < startPosition + fileBlockSize) {
					ulong timestamp = (ulong)br.ReadUInt16();
					if (timestamp == 0xffff) {
						previousTimestamp += timestamp;
						usePreviousTimestamp = true;
					} else {
						if (usePreviousTimestamp) {
							if (timestamp > ulong.MaxValue - previousTimestamp) {
								throw new TimestampOverflowException();
							}
							timestamp += previousTimestamp;
							previousTimestamp = 0;
							usePreviousTimestamp = false;
						}
						yield return timestamp;
					}
				}
			}
			internal IEnumerable<ulong> EnumerateTimestamps(BinaryReader br, ushort dataCodingMethod)
			{
				switch (dataCodingMethod) {
				case 0:
					return EnumerateTimestampsUsingMethod0(br);
				case 1:
					return EnumerateTimestampsUsingMethod1(br);
				default:
					return EnumerateTimestampsUsingMethod2(br);
				}
			}

			//public static List<ulong> Parse(FileStream fs, uint fileBlockSize, ushort dataCodingMethod)
			//{
			//	IEnumerable<ulong> timestampsEnumerator = EnumerateTimestamps(fs, fileBlockSize, dataCodingMethod);
			//	return timestampsEnumerator.ToList();
			//}


			uint ReadVariableLengthUInt(BinaryReader br, byte[] bytes)
			{
				int bytesRead = br.Read(bytes, 0, 1);
				if (bytesRead != 1) { throw new SometingWrongHierException("R1"); }
				// if the value is less than 192, we are done
				if (bytes[0] < 0xC0) {
					return (uint)bytes[0];
				}
				if (bytes[0] < 0xF0) {
					// two bytes
					bytesRead = br.Read(bytes, 1, 1);
					if (bytesRead != 1) { throw new SometingWrongHierException("R2"); }
					// 0b00111111 => 0x3f
					return ((((((uint)bytes[0]) & 0x3f) << 8) | ((uint)bytes[1]))) + 0xC0;
				} else if (bytes[0] < 0xFC) {
					// three bytes
					bytesRead = br.Read(bytes, 1, 2);
					if (bytesRead != 2) { throw new SometingWrongHierException("R3");}
					// 0b00001111 => 0x0f
					return (((((uint)bytes[0]) & 0x0f) << 16) | (((uint)bytes[1]) << 8) | ((uint)bytes[2])) + 0x30C0;
				} else {
					// four bytes
					bytesRead = br.Read(bytes, 1, 3);
					if (bytesRead != 3) { throw new SometingWrongHierException("R4"); }
					// 0b00000011 => 0x03
					return (((((uint)bytes[0]) & 0x03) << 24) | (((uint)bytes[1]) << 16) | (((uint)bytes[2]) << 8) | ((uint)bytes[3])) + 0x0C30C0;
				}
			}
		}

		public class MCAHeader
		{
			public string FileIdentification;
			public ushort ValidByteCount;
			public ushort FirmwareVersion;
			public ushort HardwareVersion;
			public ushort FirmwareModification;
			public ushort HardwareModification;
			public ushort SerialNumber;
			public ushort GeneralMode;

			public override string ToString()
			{
				return "MCAHeader:\n" +
					" - file id: " + FileIdentification + "\n" +
					" - valid byte count: " + ValidByteCount + "\n" +
					" - firmware version: " + FirmwareVersion + "\n" +
					" - hardware version: " + HardwareVersion + "\n" +
					" - firmware modification: " + FirmwareModification + "\n" +
					" - hardware modification: " + HardwareModification + "\n" +
					" - serial number: " + SerialNumber + "\n" +
					" - general mode: " + GeneralMode;
			}


			public static MCAHeader Scan(BinaryReader br)
			{
				MCAHeader header = new MCAHeader {
					FileIdentification = ReadUTF8ByteString(br, 14),
					ValidByteCount = br.ReadUInt16(),
					FirmwareVersion = br.ReadUInt16(),
					HardwareVersion = br.ReadUInt16(),
					FirmwareModification = br.ReadUInt16(),
					HardwareModification = br.ReadUInt16(),
					SerialNumber = br.ReadUInt16(),
					GeneralMode = br.ReadUInt16()
				};
				return header;
			}

			public void Write(MCAFile mca)
			{
				// jump to the beginning of the stream...
				mca.stream.Seek(0, SeekOrigin.Begin);
				ValidByteCount = MCAFile.BasisBlockSize;
				mca.Write(FileIdentification, 14);
				mca.Write(ValidByteCount);
				mca.Write(FirmwareVersion);
				mca.Write(HardwareVersion);
				mca.Write(FirmwareModification);
				mca.Write(HardwareModification);
				mca.Write(SerialNumber);
				mca.Write(GeneralMode);
			}
		}

		public class MCATimestampsRecorderModeHeader
		{
			public string ApplicationIdentification;
			public ushort TimeUnitLengthNanoSec;
			public ushort Preset;
			public uint PresetValue;
			public uint PresetMemorySize;
			public uint UsedMemorySize;
			public ushort HighVoltage;
			public ushort HighVoltagePolarity;
			public short HVInhibitMode;
			public ushort PreamplifierPowerSwitches;
			public byte TTLLowLevel;
			public byte TTLHighLevel;
			public ushort AmplifierCoarseGain;
			public ushort ADCInputPolarity;
			public ushort ShapingTimeChoice;
			public byte TriggerFilterForLowShapingTime;
			public byte TriggerFilterForHighShapingTime;
			public ushort OffsetDAC;
			public ushort TriggerLevelForAutomaticThresholdCalculation;
			public int SetTriggerThreshold;
			public byte ExtensionPortPartAConfiguration;
			public byte ExtensionPortPartBConfiguration;
			public byte ExtensionPortPartCConfiguration;
			public byte ExtensionPortPartFConfiguration;
			public ushort ExtensionPortRS232BaudRate;
			public ushort ExtensionPortRS232Flags;
			public ushort StartFlag;
			public uint StartTime;
			public uint RealTime;
			public uint BatteryCurrentAtStop;
			public uint ChargerCurrentAtStop;
			public uint HVPrimaryCurrentAtStop;
			public uint Plus12VPrimaryCurrentAtStop;
			public uint Minus12VPrimaryCurrentAtStop;
			public uint Plus24VPrimaryCurrentAtStop;
			public uint Minus24VPrimaryCurrentAtStop;
			public uint BatteryVoltageAtStop;
			public uint HighVoltageAtStop;
			public byte Plus12VActualValueAtStop;
			public byte Minus12VActualValueAtStop;
			public byte Plus24VActualValueAtStop;
			public byte Minus24VActualValueAtStop;
			public ushort VoltageOnSubD9Pin3AtStop;
			public ushort VoltageOnSubD9Pin5AtStop;
			public ushort CurrentSourceStateOnSubD9Pin5;
			public ushort CurrentSourceValueOnSubD9Pin5;
			public ushort InputResistanceOnSubD9Pin5;
			public sbyte ADCCorrectionOffsetOnSubD9Pin5;
			public sbyte GainCorrectionFactorOnSubD9Pin5;
			public sbyte ADCCorrectionOffsetOnSubD9Pin3;
			public sbyte GainCorrectionFactorOnSubD9Pin3;
			public short MCATemperatureAtStop;
			public short DetectorTemperatureAtStop;
			public short PowerModuleTemperatureAtStop;
			// since version 1.00.0007
			public sbyte RepeatMode;
			public sbyte RepeatModeOptions;
			public ushort RepeatValue; // binary data format docs say this is short, firmware commands say it is ushort
			// since version 1.00.0000
			public uint AHRCGroup0Width;
			public uint AHRCGroup1Width;
			public uint AHRCGroup2Width;
			public uint AHRCGroup3Width;
			public uint AHRCGroup4Width;
			public uint AHRCGroup5Width;
			public uint AHRCGroup6Width;
			public uint AHRCGroup7Width;
			public uint AHRCGroup8Width;
			public uint AHRCGroup9Width;
			public ushort AHRCTriggerThreshold;
			public ushort DataCodingMethod;


			public override string ToString()
			{
				return "TimestampsRecorderModeHeader:\n" +
					" - application id: " + ApplicationIdentification + "\n" +
					" - time unit length (nanoseconds): " + TimeUnitLengthNanoSec + "\n" +
					" - preset: " + Preset + "\n" +
					" - preset value: " + PresetValue + "\n" +
					" - preset memory size: " + PresetMemorySize + "\n" +
					" - used memory size: " + UsedMemorySize + "\n" +
					" - start time: " + StartTime + "\n" +
					" - real time: " + RealTime + "\n" +
					" - high voltage: " + HighVoltage + "\n" +
					" - high voltage polarity: " + HighVoltagePolarity + "\n" +
					" - hv inhibit mode: " + HVInhibitMode + "\n" +
					" - preamp power switches: " + PreamplifierPowerSwitches + "\n" +
					" - ttl low level: " + TTLLowLevel + "\n" +
					" - ttl high level: " + TTLHighLevel + "\n" +
					" - amp coarse gain: " + AmplifierCoarseGain + "\n" +
					" - adc input polarity: " + ADCInputPolarity + "\n" +
					" - shaping time choice: " + ShapingTimeChoice + "\n" +
					" - trigger filter for low shaping time: " + TriggerFilterForLowShapingTime + "\n" +
					" - trigger filter for high shaping time: " + TriggerFilterForHighShapingTime + "\n" +
					" - offset dac: " + OffsetDAC + "\n" +
					" - trigger level for automatic threshold calculation: " + TriggerLevelForAutomaticThresholdCalculation + "\n" +
					" - set trigger threshold: " + SetTriggerThreshold + "\n" +
					" - ext port part A config: " + ExtensionPortPartAConfiguration + "\n" +
					" - ext port part B config: " + ExtensionPortPartBConfiguration + "\n" +
					" - ext port part C config: " + ExtensionPortPartCConfiguration + "\n" +
					" - ext port part F config: " + ExtensionPortPartFConfiguration + "\n" +
					" - ext port rs232 baud rate: " + ExtensionPortRS232BaudRate + "\n" +
					" - ext port rs232 flags: " + ExtensionPortRS232Flags + "\n" +
					" - start flag: " + StartFlag + "\n" +
					" - bat cur at stop: " + BatteryCurrentAtStop + "\n" +
					" - chrg cur at stop: " + ChargerCurrentAtStop + "\n" +
					" - hv prim cur at stop: " + HVPrimaryCurrentAtStop + "\n" +
					" - +12v prim cur at stop: " + Plus12VPrimaryCurrentAtStop + "\n" +
					" - -12v prim cur at stop: " + Minus12VPrimaryCurrentAtStop + "\n" +
					" - +24v prim cur at stop: " + Plus24VPrimaryCurrentAtStop + "\n" +
					" - -24v prim cur at stop: " + Minus24VPrimaryCurrentAtStop + "\n" +
					" - bat volt at stop: " + BatteryVoltageAtStop + "\n" +
					" - high volt at stop: " + HighVoltageAtStop + "\n" +
					" - +12v actual val at stop: " + Plus12VActualValueAtStop + "\n" +
					" - -12v actual val at stop: " + Minus12VActualValueAtStop + "\n" +
					" - +24v actual val at stop: " + Plus24VActualValueAtStop + "\n" +
					" - -24v actual val at stop: " + Minus24VActualValueAtStop + "\n" +
					" - volt on SUB-D9 pin 3 at stop: " + VoltageOnSubD9Pin3AtStop + "\n" +
					" - volt on SUB-D9 pin 5 at stop: " + VoltageOnSubD9Pin5AtStop + "\n" +
					" - cur source state on SUB-D9 pin 5: " + CurrentSourceStateOnSubD9Pin5 + "\n" +
					" - cur source val on SUB-D9 pin 5: " + CurrentSourceValueOnSubD9Pin5 + "\n" +
					" - input resist on SUB-D9 pin 5: " + InputResistanceOnSubD9Pin5 + "\n" +
					" - adc correct offset on SUB-D9 pin 5: " + ADCCorrectionOffsetOnSubD9Pin5 + "\n" +
					" - gain correct fact on SUB-D9 pin 5: " + GainCorrectionFactorOnSubD9Pin5 + "\n" +
					" - adc correct offset on SUB-D9 pin 3: " + ADCCorrectionOffsetOnSubD9Pin3 + "\n" +
					" - gain correct fact on SUB-D9 pin 3: " + GainCorrectionFactorOnSubD9Pin3 + "\n" +
					" - mca temp at stop: " + MCATemperatureAtStop + "\n" +
					" - detect temp at stop: " + DetectorTemperatureAtStop + "\n" +
					" - power module temp at stop: " + PowerModuleTemperatureAtStop + "\n" +
					" - repeat mode: " + RepeatMode + "\n" +
					" - repeat mode options: " + RepeatModeOptions + "\n" +
					" - repeat value: " + RepeatValue + "\n" +
					" - ahrc group 0 width: " + AHRCGroup0Width + "\n" +
					" - ahrc group 1 width: " + AHRCGroup1Width + "\n" +
					" - ahrc group 2 width: " + AHRCGroup2Width + "\n" +
					" - ahrc group 3 width: " + AHRCGroup3Width + "\n" +
					" - ahrc group 4 width: " + AHRCGroup4Width + "\n" +
					" - ahrc group 5 width: " + AHRCGroup5Width + "\n" +
					" - ahrc group 6 width: " + AHRCGroup6Width + "\n" +
					" - ahrc group 7 width: " + AHRCGroup7Width + "\n" +
					" - ahrc group 8 width: " + AHRCGroup8Width + "\n" +
					" - ahrc group 9 width: " + AHRCGroup9Width + "\n" +
					" - ahrc trigger threshold: " + AHRCTriggerThreshold + "\n" +
					" - data coding method: " + DataCodingMethod;
			}

			public static MCATimestampsRecorderModeHeader Scan(BinaryReader br)
			{
				MCATimestampsRecorderModeHeader header = new MCATimestampsRecorderModeHeader {
					ApplicationIdentification = ReadUTF8ByteString(br, 32),
					TimeUnitLengthNanoSec = br.ReadUInt16(),
					Preset = br.ReadUInt16(),
					PresetValue = br.ReadUInt32(),
					PresetMemorySize = br.ReadUInt32(),
					UsedMemorySize = br.ReadUInt32(),
					HighVoltage = br.ReadUInt16(),
					HighVoltagePolarity = br.ReadUInt16(),
					HVInhibitMode = br.ReadInt16(),
					PreamplifierPowerSwitches = br.ReadUInt16(),
					TTLLowLevel = br.ReadByte(),
					TTLHighLevel = br.ReadByte(),
					AmplifierCoarseGain = br.ReadUInt16(),
					ADCInputPolarity = br.ReadUInt16(),
					ShapingTimeChoice = br.ReadUInt16(),
					TriggerFilterForLowShapingTime = br.ReadByte(),
					TriggerFilterForHighShapingTime = br.ReadByte(),
					OffsetDAC = br.ReadUInt16(),
					TriggerLevelForAutomaticThresholdCalculation = br.ReadUInt16(),
					SetTriggerThreshold = br.ReadInt32(),
					ExtensionPortPartAConfiguration = br.ReadByte(),
					ExtensionPortPartBConfiguration = br.ReadByte(),
					ExtensionPortPartCConfiguration = br.ReadByte(),
					ExtensionPortPartFConfiguration = br.ReadByte(),
					ExtensionPortRS232BaudRate = br.ReadUInt16(),
					ExtensionPortRS232Flags = br.ReadUInt16(),
					StartFlag = br.ReadUInt16(),
					StartTime = br.ReadUInt32(),
					RealTime = br.ReadUInt32(),
					BatteryCurrentAtStop = br.ReadUInt32(),
					ChargerCurrentAtStop = br.ReadUInt32(),
					HVPrimaryCurrentAtStop = br.ReadUInt32(),
					Plus12VPrimaryCurrentAtStop = br.ReadUInt32(),
					Minus12VPrimaryCurrentAtStop = br.ReadUInt32(),
					Plus24VPrimaryCurrentAtStop = br.ReadUInt32(),
					Minus24VPrimaryCurrentAtStop = br.ReadUInt32(),
					BatteryVoltageAtStop = br.ReadUInt32(),
					HighVoltageAtStop = br.ReadUInt32(),
					Plus12VActualValueAtStop = br.ReadByte(),
					Minus12VActualValueAtStop = br.ReadByte(),
					Plus24VActualValueAtStop = br.ReadByte(),
					Minus24VActualValueAtStop = br.ReadByte(),
					VoltageOnSubD9Pin3AtStop = br.ReadUInt16(),
					VoltageOnSubD9Pin5AtStop = br.ReadUInt16(),
					CurrentSourceStateOnSubD9Pin5 = br.ReadUInt16(),
					CurrentSourceValueOnSubD9Pin5 = br.ReadUInt16(),
					InputResistanceOnSubD9Pin5 = br.ReadUInt16(),
					ADCCorrectionOffsetOnSubD9Pin5 = br.ReadSByte(),
					GainCorrectionFactorOnSubD9Pin5 = br.ReadSByte(),
					ADCCorrectionOffsetOnSubD9Pin3 = br.ReadSByte(),
					GainCorrectionFactorOnSubD9Pin3 = br.ReadSByte(),
					MCATemperatureAtStop = br.ReadInt16(),
					DetectorTemperatureAtStop = br.ReadInt16(),
					PowerModuleTemperatureAtStop = br.ReadInt16(),
					// since version 1.00.0007
					RepeatMode = br.ReadSByte(),
					RepeatModeOptions = br.ReadSByte(),
					RepeatValue = br.ReadUInt16(),
					// since version 1.00.0000
					AHRCGroup0Width = br.ReadUInt32(),
					AHRCGroup1Width = br.ReadUInt32(),
					AHRCGroup2Width = br.ReadUInt32(),
					AHRCGroup3Width = br.ReadUInt32(),
					AHRCGroup4Width = br.ReadUInt32(),
					AHRCGroup5Width = br.ReadUInt32(),
					AHRCGroup6Width = br.ReadUInt32(),
					AHRCGroup7Width = br.ReadUInt32(),
					AHRCGroup8Width = br.ReadUInt32(),
					AHRCGroup9Width = br.ReadUInt32(),
					AHRCTriggerThreshold = br.ReadUInt16(),
					DataCodingMethod = br.ReadUInt16()
				};
				return header;
			}
			public void Write(MCAFile mca)
			{
				DataCodingMethod = 0;
				
				mca.Write(ApplicationIdentification, 32);
				mca.Write(TimeUnitLengthNanoSec);
				mca.Write(Preset);
				mca.Write(PresetValue);
				mca.Write(PresetMemorySize);
				mca.Write(UsedMemorySize);
				mca.Write(HighVoltage);
				mca.Write(HighVoltagePolarity);
				mca.Write(HVInhibitMode);
				mca.Write(PreamplifierPowerSwitches);
				mca.Write(TTLLowLevel);
				mca.Write(TTLHighLevel);
				mca.Write(AmplifierCoarseGain);
				mca.Write(ADCInputPolarity);
				mca.Write(ShapingTimeChoice);
				mca.Write(TriggerFilterForLowShapingTime);
				mca.Write(TriggerFilterForHighShapingTime);
				mca.Write(OffsetDAC);
				mca.Write(TriggerLevelForAutomaticThresholdCalculation);
				mca.Write(SetTriggerThreshold);
				mca.Write(ExtensionPortPartAConfiguration);
				mca.Write(ExtensionPortPartBConfiguration);
				mca.Write(ExtensionPortPartCConfiguration);
				mca.Write(ExtensionPortPartFConfiguration);
				mca.Write(ExtensionPortRS232BaudRate);
				mca.Write(ExtensionPortRS232Flags);
				mca.Write(StartFlag);
				mca.Write(StartTime);
				mca.Write(RealTime);
				mca.Write(BatteryCurrentAtStop);
				mca.Write(ChargerCurrentAtStop);
				mca.Write(HVPrimaryCurrentAtStop);
				mca.Write(Plus12VPrimaryCurrentAtStop);
				mca.Write(Minus12VPrimaryCurrentAtStop);
				mca.Write(Plus24VPrimaryCurrentAtStop);
				mca.Write(Minus24VPrimaryCurrentAtStop);
				mca.Write(BatteryVoltageAtStop);
				mca.Write(HighVoltageAtStop);
				mca.Write(Plus12VActualValueAtStop);
				mca.Write(Minus12VActualValueAtStop);
				mca.Write(Plus24VActualValueAtStop);
				mca.Write(Minus24VActualValueAtStop);
				mca.Write(VoltageOnSubD9Pin3AtStop);
				mca.Write(VoltageOnSubD9Pin5AtStop);
				mca.Write(CurrentSourceStateOnSubD9Pin5);
				mca.Write(CurrentSourceValueOnSubD9Pin5);
				mca.Write(InputResistanceOnSubD9Pin5);
				mca.Write(ADCCorrectionOffsetOnSubD9Pin5);
				mca.Write(GainCorrectionFactorOnSubD9Pin5);
				mca.Write(MCATemperatureAtStop);
				mca.Write(DetectorTemperatureAtStop);
				mca.Write(PowerModuleTemperatureAtStop);
				mca.Write(RepeatMode);
				mca.Write(RepeatModeOptions);
				mca.Write(RepeatValue);
				mca.Write(AHRCGroup0Width);
				mca.Write(AHRCGroup1Width);
				mca.Write(AHRCGroup2Width);
				mca.Write(AHRCGroup3Width);
				mca.Write(AHRCGroup4Width);
				mca.Write(AHRCGroup5Width);
				mca.Write(AHRCGroup6Width);
				mca.Write(AHRCGroup7Width);
				mca.Write(AHRCGroup8Width);
				mca.Write(AHRCGroup9Width);
				mca.Write(AHRCTriggerThreshold);
				mca.Write(DataCodingMethod);
			}
		}

		internal MCAHeader header;
		internal MCATimestampsRecorderModeHeader rheader;
		internal MCATimestamp mca;

		public BinaryWriter writer;
		public BinaryReader reader;


		public MCAFile()
		{
			FirstEventTimeInTics = 0;
			TotalDups = 0;
			TotalEvents = 0;
			LastTimeInTics = 0;
		}

		public void ReadHeader()
		{
			header = MCAHeader.Scan(reader);
			switch (header.GeneralMode) {
			case 0:
				// General Mode = 'MCA'
				throw new NotImplementedException("General Mode = 'MCA' not implemented");
			case 3:
			case 4:
			case 5:
				// General Mode = 'Timestamps recorder'
				rheader = MCATimestampsRecorderModeHeader.Scan(reader);
				long seekgap = header.ValidByteCount - 228;
				if (seekgap > 0)	// Seek ahead header.ValidByteCount - 228 (a hard-coded value, the number of bytes in the rheader)
									// This can be non-zero on files taken directly from the MCA-527 SD card storage
				{
					reader.BaseStream.Seek(header.ValidByteCount - 228, SeekOrigin.Current);
					Log.TraceEvent(LogLevels.Verbose, 117, "Seek " + seekgap.ToString() + "bytes");
				}
				if (rheader.UsedMemorySize > 0 &&
					(rheader.ExtensionPortPartAConfiguration == 0x5 ||
					 rheader.ExtensionPortPartCConfiguration == 0x5)) {
					// RS-232 data
					// Do we need to parse it? Not for now
				}
				mca = new MCATimestamp(rheader.UsedMemorySize);
				break;
			default:
				throw new FormatException("Unexpected MCA file format");
			}
		}

		public uint TotalBytes { get { return header.ValidByteCount + rheader.UsedMemorySize; } } 

		public long ReaderPosition { get { return reader.BaseStream.Position; } } 

		public uint MeasTime { get { return rheader.RealTime; } } 

		public ulong TimeUnitNanoSec { get { return rheader.TimeUnitLengthNanoSec; } } 

		public ulong HighVoltage { get { return rheader.HighVoltage; } } 


		public DateTimeOffset StartTime { 
			get 
			{
				uint startTime = rheader.StartTime;
				// MCA start time => number of seconds since Dec 31, 1969, 16:00:00 -8:00 PST (midnight 1970 GMT)
				int hours = 0;
				while (startTime > int.MaxValue) {
					hours += 1;
					startTime -= 60 * 60;
				}
				DateTimeOffset mcaEpoch = new DateTimeOffset(1969, 12, 31, 16, 0, 0, new TimeSpan(-8,0,0)); // start at GMT unix time 0 i.e. GMT time from US west coast time is an 8 hour offset
				mcaEpoch = mcaEpoch.AddSeconds(startTime);
				return mcaEpoch;		
			} }

		public TimeSpan RealTime { 
			get 
			{
				//int hrs = (int)(rheader.RealTime / 3600);
				//int min = (int)((rheader.RealTime - (hrs * 3600)) / 60);
				//int sec = (int)((rheader.RealTime - ((hrs * 3600) + (min * 60))));
				return new TimeSpan(0, 0, (int)rheader.RealTime);  // dev note: cannot expect a measurement time of greater than int.MaxValue seconds (24855 days), so this truncation is acceptable

			} }


		public IEnumerable<ulong> EnumerateTimestamps()
		{
			return mca.EnumerateTimestamps(reader, rheader.DataCodingMethod);
		}
		public override bool OpenForReading(string filename = null)
		{
			bool ok = base.OpenForReading(filename);
			if (ok)
				reader = new BinaryReader(stream);
			return ok;
		}

		public void CloseReader()
		{
			if (reader == null)
				return;
			reader.Close();
			base.CloseStream();
		}


		//  when encountering the same file name, create a file name with a millisecond addendum
		public override bool CreateForWriting()
		{
			bool ok = base.CreateForWriting();
			if (ok)
			{
				writer = new BinaryWriter(stream);
				AdvanceToTimestampsBlock();  // move file pointer to first timestamp location
			}
			return ok;
		}

		public bool OpenForWriting()
		{
            if (Log != null) Log.TraceEvent(LogLevels.Info, 111, "opening existing file for writing: " + Filename);
			try
			{ 
				stream = File.OpenWrite(Filename);
				writer = new BinaryWriter(stream);					
			}
			catch (Exception e) 
			{
				if (Log != null)
					Log.TraceException(e);
				return false;
			}
			return true;
		}

		public override void ConstructFullPathName(string opt = "")
		{
			base.ConstructFullPathName(opt);
			Filename += ".mca";
		}

		public void Write(byte[] buffer, int index, int count)
		{
			if (writer != null)
				writer.Write(buffer, index, count);
		}

		public override void CloseWriter()
		{
			base.CloseWriter();
			if (Log != null)
				Log.TraceEvent(LogLevels.Verbose, 117, "Closing writer for " + Filename);
			try {
				if (writer != null) {
					writer.Flush();
					writer.Close();
					writer = null;
				}
				CloseStream();
				stream = null;
			} catch (Exception e) {
				if (Log != null)
					Log.TraceException(e);
			}
		}

		const ushort BasisBlockSize = 228;

		// create, then call this, write timestamps, at end seek back and write final header
		public void AdvanceToTimestampsBlock()
        {
            writer.Seek(BasisBlockSize, SeekOrigin.Begin);
        }

		public void WriteTimestampsRawDataChunk(byte[] rawDataChunk, int offset, int count)
        {
            Write(rawDataChunk, offset, count);
        }

		public void Write(string str, int length)
        {
			int len = 0;
			if (str != null) {
				byte[] bytes = Encoding.UTF8.GetBytes(str);
				len = Math.Min(length, bytes.Length);
				writer.Write(bytes, 0, len);
			}
			while (len++ < length) { writer.Write((byte)0); }
		}

        public void Write(sbyte value)
        {
            Write((byte)value);
        }

        public void Write(byte value)
        {
            writer.Write(value);
        }

		public void Write(ushort value)
		{           
			writer.Write((byte)(value & 0xff));
			writer.Write((byte)((value >> 8) & 0xff));
		}

        public void Write(short value)
        {
            Write((ushort)value);
        }

		public void Write(uint value)
		{
			writer.Write((byte)(value & 0xff));
			writer.Write((byte)((value >> 8) & 0xff));
			writer.Write((byte)((value >> 16) & 0xff));
			writer.Write((byte)((value >> 24) & 0xff));
		}

        public void Write(int value)
        {
            Write((uint)value);
        }

        public void Write(long value)
        {
            Write((ulong)value);
        }

		private void Write(ulong value)
		{
			writer.Write((byte)(value & 0xff));
			writer.Write((byte)((value >> 8) & 0xff));
			writer.Write((byte)((value >> 16) & 0xff));
			writer.Write((byte)((value >> 24) & 0xff));
			writer.Write((byte)((value >> 32) & 0xff));
			writer.Write((byte)((value >> 40) & 0xff));
			writer.Write((byte)((value >> 48) & 0xff));
			writer.Write((byte)((value >> 56) & 0xff));
		}

		private void Write(double value)
		{
			Write(BitConverter.DoubleToInt64Bits(value));
		}

		public void WriteHeader()
		{
			header.Write(this);
			rheader.Write(this);
		}


		public long FirstEventTimeInTics;
		public ulong TotalDups;
		public ulong TotalEvents;
		public long LastTimeInTics;

		Analysis.StreamStatusBlock sb;
		public void CustomStatusBlock(string HW, string devicename, string source, string message)
		{
			double durationTics = (double)(LastTimeInTics - FirstEventTimeInTics);
			if (durationTics <= 0)
				durationTics = 1;

			string sourcename = source.Substring(source.LastIndexOf("\\") + 1);

			string s2 = string.Format("Status\r\nSingle\r\nDuration = {0:F6} s\r\ntriggers = {1}\r\ntotals   = {2}\r\noverflow = 0xfeedacat, FALSE\r\npackets  = NA\r\nrate     = {3:F6} ~samples/sec\r\n",
				durationTics / 10e7, TotalEvents, TotalEvents, ((double)TotalEvents / durationTics) / 10e-7);

			string s3 = string.Format("rate     = NA Mbits/sec\r\nbin size = 100 ns\r\nbuf sep  = 0 ms\r\nname     = {0} : {1}\r\nLMC      = f00dbeef\r\nsource   = {2}\r\n", HW, devicename, sourcename);
			string s = s2 + s3;
			if (!string.IsNullOrEmpty(message)) {
				s += string.Format("Extra    = {0}\r\n", message);
			}
			sb = new Analysis.StreamStatusBlock();
			sb.Encode(s);

			Log.TraceEvent(LogLevels.Verbose, 3333, "Converted {0} events between {1} and {2} tics ({3} duplicates skipped)", TotalEvents, FirstEventTimeInTics, LastTimeInTics, TotalDups);

		}

		static string ReadUTF8ByteString(BinaryReader r, int count)
		{
			byte[] bytes = null;
			try {
				bytes = r.ReadBytes(count);
				return System.Text.Encoding.UTF8.GetString(bytes);
			} catch (Exception) {
			}
			return string.Empty;
		}


	}


	public class UnsortedPulseFile : NeutronDataFile
    {

        static public List<string> ExtensionList = new List<string>() { ".pulse", ".txt" };

        public TextWriter writer;
        public StreamReader reader;

        public UnsortedPulseFile()
        {
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
            if (reader != null)
            {
                reader.Close();
                base.CloseStream();
            }
        }

        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
                writer = new StreamWriter(stream);
            return ok;
        }

        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".pulse"; // should be ThisSuffix; but only used when creating and we never create these 
        }


        public void Write(double d)
        {
            if (writer != null)
                writer.WriteLine(d);
        }
        public override void CloseWriter()
        {
            if (Log != null) Log.TraceEvent(LogLevels.Verbose, 116, "Closing writer for" + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }


    }

    // dev note: 12 byte tag to specify different file formats for size and speed. The tag is a 12 byte token to tell parser how to parse the data.
    // dev note: 8 bytes specify the time and channel scheme, 4 bytes to specify endianess
    //  64 bit times only
    //  64 bit times with tag byte indicating channel bin count bytes following the time, permits variation within the file and is a generalized file format
    //  0000 F000 + 1 byte for channel count (4->E), implies number of bytes after a time to convert as channels
    //  0000 FF00 + 2 bytes for huge channel count (not useful today)
    //
    // The 4 bytes for byte endianess specifier (copied from TIFF)
    // "Every TIFF begins with a 2-byte indicator of byte order: "II" for little-endian and "MM" for big-endian byte ordering. The next 2 bytes represent the number 42, selected because this is the binary pattern 101010"
    // 49 49 2A 00
    // 4D 4D 00 2A
    //
    // The next 8 bytes, (after endianess adjustment)
    //  32 + 32   = 00 00 00 00  F4 00 00 00
    //  32 + 0    = 00 00 00 00  F0 00 00 00   for 1 channel, no channel byte!  *** see mergesort file output note
    //  32 + 128  = 00 00 00 00  F8 00 00 00   (2^8 channels requires 128 bits i.e. 8 bytes)
    //  32 + 8192 = 00 00 00 00  FD 00 00 00   (2^13 channels)
    //  32 + 65536= 00 00 00 00  FF 10 00 00   (2^16 channels)
    //  64 + 32   = 00 00 00 00  E4 00 00 00
    //  64 + 0    = 00 00 00 00  E0 00 00 00   for 1 channel, no channel byte! 
    //  64 + 128  = 00 00 00 00  E8 00 00 00   (2^8 channels requires 128 bits i.e. 8 bytes)
    //  64 + 8192 = 00 00 00 00  ED 00 00 00   (2^13 channels)
    //  64 + 65536= 00 00 00 00  EF 10 00 00   (2^16 channels)
    //
    // dev note: add option to write the sorted file out as a series of integers instead of writing out as doubles, write it out as ints, 
    // dev note: creation option to write directly to NCD binary file using the merge sort
    // dev note: maybe keep the time in shakes with a notation in the end block
    // dev note, then need a flag on the NCDFIle class 
    // dev note: then need to use the flag in the parser
    public class SortedPulseFile : UnsortedPulseFile
    {

        public SortedPulseFile()
        {
        }


        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".pulse.sorted";
        }
    }

    public class LMPair
    {
        public LMPair(UInt32 chn, Double time)
        {
            ChannelMask = chn;
            Time = time;
        }

        public UInt32 ChannelMask
        {
            get;
            set;
        }
        public Double Time
        {
            get;
            set;
        }
    }


    public class SortedLMDFile : UnsortedLMDFile
    {

        public SortedLMDFile()
        {
        }


        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".lmd.sorted";
        }
    }

    public class UnsortedLMDFile : NeutronDataFile
    {

        public TextWriter writer;
        public StreamReader reader;

        public UnsortedLMDFile()
        {
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
            if (reader != null)
            {
                reader.Close();
                base.CloseStream();
            }
        }

        public override bool CreateForWriting()
        {
            bool ok = base.CreateForWriting();
            if (ok)
                writer = new StreamWriter(stream);
            return ok;
        }

        public override void ConstructFullPathName(string opt = "")
        {
            base.ConstructFullPathName(opt);
            Filename += ".lmd";
        }


        public void Write(double d)
        {
            if (writer != null)
                writer.WriteLine(d);
        }
        public override void CloseWriter()
        {
            if (Log != null) Log.TraceEvent(LogLevels.Verbose, 116, "Closing writer for" + Filename);
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                CloseStream();
            }
            catch (Exception e)
            {
                if (Log != null) Log.TraceException(e);
            }
        }



        /// <summary>
        /// spaces, then  0 - 31 for channel hit then spaces then a large int for time (is it in shakes?)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// 
        public void ParseLine(string line, ref Int32 chn, ref double time)
        {
            string[] two = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // any and all whitespace separates

            if (two != null)
            {
                if (two.Length >= 2)
                {
                    bool ok = Int32.TryParse(two[0], out chn);
                    if (ok && chn >= 0 && chn < 32)
                        chn = (1 << chn);
                    ok = Double.TryParse(two[1], out time);
                }
                else if (two.Length == 1)
                {
                    bool ok = Double.TryParse(two[0], out time);
                }
            }
        }

        public LMPair ParseLinesToNCDPair(List<string> lines)
        {
            Int32 chn = 0;
            Double time = 0;

            foreach (string line in lines)
            {
                ParseLine(line, ref chn, ref time);
            }
            return new LMPair((uint)chn, time);
        }
    }

    #endregion specific file container classes

    #region big file sort technique

    /// <summary>
    /// sort text pulse streams files into sorted text pulse stream files
    /// after sorting, the files are suirable for input into the pulseFileToNCD operation
    /// </summary>
    public static class ExternalMergeSort
    {

        public static LMLoggers.LognLM Logger;

        public static int sizeFromMB(int mb)
        {
            long lmb = (long)mb;
            long bytes = lmb * 1024 * 1024;
            return (int)(bytes / sizeof(double));  // TEMPLATE: data type
        }

        public class Options
        {
            public readonly int MaxSortSize;
            public readonly int MergeFileSize;
            public readonly int OutputBufferSize;
            public bool SkipInitialSort = false;
            public bool RemoveIntermediateFiles = false;
            public Options(int maxSortSize, int mergeFileSize, int outputBufferSize)
            {
                this.MaxSortSize = maxSortSize;
                this.MergeFileSize = mergeFileSize;
                this.OutputBufferSize = outputBufferSize;
            }

        }

        private class OrderedSeqFromFile
        {
            FileStream fs;

            public class Param
            {
                public FileStream fs;
                public long fileLen;
                public long remaining;
                public int bufLen;
                public int ioSize;
                public byte[] byteBuffer;
                public double[] dblBuffer; // TEMPLATE: data type
                public ManualResetEvent mre;
                public int id;
                public int sz;
                public Param(FileStream fs, long fileLen, long remaining, int ioSize, ManualResetEvent mre, int id)
                {
                    this.fs = fs;
                    this.fileLen = fileLen;
                    this.remaining = remaining;
                    this.ioSize = ioSize;
                    bufLen = sizeof(double) * ioSize; // TEMPLATE: data type
                    byteBuffer = new byte[bufLen];
                    dblBuffer = new double[ioSize]; // TEMPLATE: data type
                    this.mre = mre;
                    this.id = id;
                    sz = 0;
                }
            }

            ManualResetEvent mre;
            Param[] parameters;
            int currentParam = 0;

            public OrderedSeqFromFile(string fileName, int ioSize)
            {
                this.fs = File.Open(fileName, FileMode.Open);
                long fileLen = fs.Length;
                long remaining = fileLen / sizeof(double); // TEMPLATE: data type
                mre = new ManualResetEvent(true);
                parameters = new Param[2];
                parameters[0] = new Param(fs, fileLen, remaining, ioSize, mre, 0);
                parameters[1] = new Param(fs, fileLen, remaining, ioSize, mre, 1);
                currentParam = 0;
                ReadAhead(parameters[currentParam]);
            }

            static void ReadOnParam(Param param)
            {
                if (param.remaining > 0)
                {
                    int sz = (int)Math.Min(param.remaining, (long)param.ioSize);
                    param.remaining -= sz;
                    param.fs.Read(param.byteBuffer, 0, sizeof(double) * sz); // TEMPLATE: data type
                    ByteArray.ToDouble(param.byteBuffer, param.dblBuffer, sz); // TEMPLATE: data type
                    param.sz = sz;
                }
                param.mre.Set();
            }

            static void ThreadProc(Object state)
            {
                Param param = (Param)state;
                ReadOnParam(param);
            }



            public void ReadAhead(Param param)
            {
                mre.Reset();
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), param);
            }

            public bool GetRest(OrderedSeq rest)
            {
                mre.WaitOne();//wait for previous read to complete
                Param param = parameters[currentParam];//one should always be available
                if (param.remaining + param.sz > 0)
                {
                    rest.Init(param.dblBuffer, 0, param.sz, this, null, null);
                    //swap param
                    currentParam = (currentParam + 1) % 2;
                    Param newParam = parameters[currentParam];
                    newParam.remaining = param.remaining;
                    newParam.sz = 0;
                    ReadAhead(newParam);
                    return true;
                }
                mre.Dispose();
                fs.Close();
                fs.Dispose();
                return false;
            }

        }

        private class OrderedSeq
        {
            public double[] data; // TEMPLATE: data type
            public int fromPos;
            public int toPos;
            public OrderedSeq set1;
            public OrderedSeq set2;
            public OrderedSeqFromFile fromFile; //if <> null it's a file stream

            public void Init(double[] data, int fromPos, int toPos, OrderedSeqFromFile fromFile, OrderedSeq set1, OrderedSeq set2) // TEMPLATE: data type
            {
                this.data = data;
                this.fromPos = fromPos;
                this.toPos = toPos;
                this.set1 = set1;
                this.set2 = set2;
                this.fromFile = fromFile;
            }

            public void CopyFrom(OrderedSeq other)
            {
                data = other.data;
                fromPos = other.fromPos;
                toPos = other.toPos;
                set1 = other.set1;
                set2 = other.set2;
                fromFile = other.fromFile;
            }

            public bool ReplaceWithSubseq(int newFrom, int newTo)
            {
                this.fromPos = newFrom;
                this.toPos = newTo;
                if (this.fromPos < this.toPos)
                {
                    return false;
                }
                return ReplaceWithRest();
            }

            public bool ReplaceWithRest()
            {
                if (fromFile != null)
                {
                    return fromFile.GetRest(this);
                }
                else if (set1 == null)
                {
                    if (set2 == null)
                        return false;
                    //oldSet2 = set2
                    CopyFrom(set2);
                    //oldSet2.Destroy();
                    return true;
                }
                else if (set2 == null)
                {
                    //oldSet1 = set1
                    CopyFrom(set1);
                    //oldSet1.Destroy();
                    return true;
                }
                return Merge2Seq(set1, set2, this);
            }
        }

        private static bool FromFile(string fileName, int ioSize, OrderedSeq rest)
        {
            OrderedSeqFromFile fromFile = new OrderedSeqFromFile(fileName, ioSize);
            return fromFile.GetRest(rest);
        }

        private static bool Merge2Seq(OrderedSeq set1, OrderedSeq set2, OrderedSeq rest)
        {
            int i1 = set1.fromPos;
            int i2 = set2.fromPos;
            int e1 = set1.toPos;
            int e2 = set2.toPos;
            double[] arr1 = set1.data; // TEMPLATE: data type
            double[] arr2 = set2.data; // TEMPLATE: data type
            double[] temp = rest.data; // TEMPLATE: data type
            Debug.Assert(set1 == rest.set1);
            Debug.Assert(set2 == rest.set2);
            Debug.Assert(temp != null);
            Debug.Assert(set1 != set2);
            Debug.Assert(set1 != rest);
            Debug.Assert(set2 != rest);
            int j = 0;
        loop:
            if (i1 >= e1) { goto copy_2; }
            if (i2 >= e2) { goto copy_1; }
            double v1 = arr1[i1]; double v2 = arr2[i2]; // TEMPLATE: data type
            if (v1 < v2) { temp[j++] = v1; i1++; }
            else { temp[j++] = v2; i2++; }
            goto loop;
        copy_1:
            rest.fromPos = 0;
            rest.toPos = j;
            set1.ReplaceWithSubseq(i1, e1);
            if (!set2.ReplaceWithRest())
                rest.set2 = null;
            return true;
        copy_2:
            rest.fromPos = 0;
            rest.toPos = j;
            if (!set1.ReplaceWithRest())
                rest.set1 = null;
            set2.ReplaceWithSubseq(i2, e2);
            return true;
        }

        /// <summary>
        /// ***  Create sorted binary file too (not a list of strings as the source is, but a binary concat of UInt32 (see notes *** supra/infra)
        /// </summary>
        class WriteToFileParam
        {
            public double[] buffer; // TEMPLATE: data type
            public char[] charBuffer;
            public int id;
            public ManualResetEvent mre;
            public int len;
            public StreamWriter w;
            public BinaryWriter bw;
            public WriteToFileParam(int len, StreamWriter w, ManualResetEvent mre, int id)
            {
                buffer = new double[len]; // TEMPLATE: data type
                charBuffer = new char[1024];
                this.len = len;
                this.w = w;
                this.mre = mre;
                this.id = id;
            }
            public WriteToFileParam(int len, BinaryWriter bw, ManualResetEvent mre, int id)
            {
                buffer = new double[len]; // TEMPLATE: data type
                charBuffer = new char[1024];
                this.len = len;
                this.bw = bw;
                this.mre = mre;
                this.id = id;
            }
        }

        public static bool StringToDouble(string s, out double res) // TEMPLATE: data type
        {


            res = 0;
            return Double.TryParse(s, out res);            // dev note: sloooow
            //int len = s.Length;
            //if (len >= 1)
            //{
            //    int zero = (int)'0';
            //    int i = 0;
            //    if (s[0] == '-')
            //        i++;
            //    int num = 0;
            //    while (i < len)
            //    {
            //        char digit = s[i++];
            //        if (digit < '0' || digit > '9')
            //            return false;
            //        num = 10 * num + ((int)(digit) - zero);
            //    }
            //    if (s[0] == '-') num = -num;
            //    res = num;
            //    return true;
            //}
            //return false;
        }

        public static int DoubleToCharBuffer(double num, char[] buffer)  // TEMPLATE: data type
        {
            //int i = 0;
            char[] foo = num.ToString().ToCharArray();
            Array.Copy(foo, 0, buffer, 0, foo.Length);
            //int off = 1;
            //if (num < 0)
            //{
            //    buffer[i++] = '-';
            //    num = -num;
            //    off = 0;
            //}
            //int start = i;

            //int zero = (int)'0';
            //do
            //{
            //    int nextNum = (int)num / 10;
            //    char d = (char)(zero + num - 10 * nextNum);
            //    num = nextNum;
            //    buffer[i++] = d;
            //} while (num > 0);
            //int mid = start + (i - start) / 2;
            //while (start < mid)
            //{
            //    char tmp = buffer[start];
            //    int end_ = i - start - off;
            //    buffer[start] = buffer[end_];
            //    buffer[end_] = tmp;
            //    start++;
            //}
            return foo.Length;
        }

        private static void DoublesToFile(WriteToFileParam param) // TEMPLATE: data type
        {
            if (param.w != null)
            {
                StreamWriter w = param.w;
                double[] data = param.buffer;
                char[] charBuffer = param.charBuffer;
                int len = param.len;
                for (int i = 0; i < len; i++)
                {
                    int j = DoubleToCharBuffer(data[i], charBuffer);
                    charBuffer[j++] = '\r';
                    charBuffer[j++] = '\n';
                    for (int k = 0; k < j; k++)
                        w.Write(charBuffer[k]);
                }
            }
            else // *** trying to capture regular ncd output via Gezorn into  here
            {
                BinaryWriter bw = param.bw;
                double[] data = param.buffer;
                char[] charBuffer = param.charBuffer;
                int len = param.len;
            }
        }

        static void ThreadProcDblsToFile(object state)
        {
            WriteToFileParam param = (WriteToFileParam)state;
            DoublesToFile(param);
            param.mre.Set();//waiting thread can proceed
        }

        private static void WriteOnThread(WriteToFileParam param)
        {
            param.mre.WaitOne();//wait for previous thread to complete
            param.mre.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProcDblsToFile), param);
        }

        private static void ToTextFile(OrderedSeq s, string fileName, int outputLen)
        {
            using (StreamWriter w = new StreamWriter(fileName))
            {
                using (ManualResetEvent mre = new ManualResetEvent(true))
                {
                    bool flag = true;

                    WriteToFileParam[] parameters = new WriteToFileParam[2];
                    parameters[0] = new WriteToFileParam(outputLen, w, mre, 0);
                    parameters[1] = new WriteToFileParam(outputLen, w, mre, 1);
                    int currentParam = 0;
                    double[] buffer = parameters[currentParam].buffer; // TEMPLATE: data type
                    int j = 0;
                    while (flag)
                    {
                        int fromPos = s.fromPos;
                        int finalToPos = s.toPos;
                        while (fromPos < finalToPos)
                        {
                            int sz = Math.Min(outputLen - j, finalToPos - fromPos);
                            int intermToPos = fromPos + sz;
                            for (int i = fromPos; i < intermToPos; i++)
                            {
                                buffer[j++] = s.data[i];
                            }
                            fromPos += sz;
                            if (j >= outputLen)
                            {
                                //write to file
                                WriteOnThread(parameters[currentParam]);
                                currentParam = (currentParam + 1) % 2;
                                buffer = parameters[currentParam].buffer;
                                j = 0;
                            }
                        }
                        flag = s.ReplaceWithRest();
                    }
                    if (j > 0)
                    {
                        //write to file
                        parameters[currentParam].len = j;
                        WriteOnThread(parameters[currentParam]);
                    }
                    mre.WaitOne();
                    parameters[0].w = null; parameters[0].mre = null;
                    parameters[1].w = null; parameters[1].mre = null;

                }
                w.Close();
            }
        }

        private static void ToNCDFile(OrderedSeq s, string fileName, int outputLen)
        {
            using (FileStream stream = File.Create(fileName))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    using (ManualResetEvent mre = new ManualResetEvent(true))
                    {
                        bool flag = true;

                        WriteToFileParam[] parameters = new WriteToFileParam[2];
                        parameters[0] = new WriteToFileParam(outputLen, bw, mre, 0);
                        parameters[1] = new WriteToFileParam(outputLen, bw, mre, 1);
                        int currentParam = 0;
                        double[] buffer = parameters[currentParam].buffer; // TEMPLATE: data type
                        int j = 0;
                        while (flag)
                        {
                            int fromPos = s.fromPos;
                            int finalToPos = s.toPos;
                            while (fromPos < finalToPos)
                            {
                                int sz = Math.Min(outputLen - j, finalToPos - fromPos);
                                int intermToPos = fromPos + sz;
                                for (int i = fromPos; i < intermToPos; i++)
                                {
                                    buffer[j++] = s.data[i];
                                }
                                fromPos += sz;
                                if (j >= outputLen)
                                {
                                    //write to file
                                    WriteOnThread(parameters[currentParam]);
                                    currentParam = (currentParam + 1) % 2;
                                    buffer = parameters[currentParam].buffer;
                                    j = 0;
                                }
                            }
                            flag = s.ReplaceWithRest();
                        }
                        if (j > 0)
                        {
                            //write to file
                            parameters[currentParam].len = j;
                            WriteOnThread(parameters[currentParam]);
                        }
                        mre.WaitOne();
                        parameters[0].w = null; parameters[0].mre = null;
                        parameters[1].w = null; parameters[1].mre = null;

                    }
                    bw.Close();
                }
            }
        }
        private static void ArrayToFile(double[] data, byte[] byteOutput, int len, string fileName)
        {
            using (FileStream fs = File.Open(fileName, FileMode.Create)) //todo: change to .CreateNew
            {
                ByteArray.FromDouble(data, len, byteOutput);  // TEMPLATE: data type
                fs.Write(byteOutput, 0, sizeof(double) * len);
                fs.Close();
            }
        }

        private static void Error(string desc)
        {
            throw new Exception(desc);
        }

        private static bool MergeBy2(string[] inputFiles, int fromIdx, int toIdx, int ioSize, OrderedSeq rest)
        {
            int len = toIdx - fromIdx;
            if (len == 0)
            {
                Error("internal error");
                return false;
            }
            else if (len == 1)
            {
                return FromFile(inputFiles[fromIdx], ioSize, rest);
            }
            else if (len == 2)
            {
                OrderedSeq s1 = new OrderedSeq();
                OrderedSeq s2 = new OrderedSeq();
                FromFile(inputFiles[fromIdx], ioSize, s1);
                FromFile(inputFiles[fromIdx + 1], ioSize, s2);
                rest.data = new double[s1.data.Length + s2.data.Length];  // TEMPLATE: data type
                rest.set1 = s1; rest.set2 = s2;
                return Merge2Seq(s1, s2, rest);
            }
            else
            {
                int mid = fromIdx + len / 2;
                OrderedSeq s1 = new OrderedSeq();
                MergeBy2(inputFiles, fromIdx, mid, ioSize, s1);
                OrderedSeq s2 = new OrderedSeq();
                MergeBy2(inputFiles, mid, toIdx, ioSize, s2);
                rest.data = new double[s1.data.Length + s2.data.Length];  // TEMPLATE: data type
                rest.set1 = s1; rest.set2 = s2;
                return Merge2Seq(s1, s2, rest);
            }
        }

        private static void MergeMany(string[] inputFiles, string outputFile, int ioSize, int outputLen)
        {
            int len = inputFiles.Length;

            if (len == 0)
            {
                Error("Number of input files must larger than 0");
            }
            else if (len == 1)
            {
                OrderedSeq rest = new OrderedSeq();
                FromFile(inputFiles[0], ioSize, rest);
                ToTextFile(rest, outputFile, outputLen);
            }
            else
            {
                OrderedSeq rest = new OrderedSeq();
                MergeBy2(inputFiles, 0, len, ioSize, rest);
                ToTextFile(rest, outputFile, outputLen);
            }
        }
        private static void MergeManyBinary(string[] inputFiles, string outputFile, int ioSize, int outputLen)
        {
            int len = inputFiles.Length;

            if (len == 0)
            {
                Error("Number of input files must larger than 0");
            }
            else if (len == 1)
            {
                OrderedSeq rest = new OrderedSeq();
                FromFile(inputFiles[0], ioSize, rest);
                ToNCDFile(rest, outputFile, outputLen);
            }
            else
            {
                OrderedSeq rest = new OrderedSeq();
                MergeBy2(inputFiles, 0, len, ioSize, rest);
                ToNCDFile(rest, outputFile, outputLen);
            }
        }
        private static void Warn(string line)
        {
            Logger.TraceEvent(LogLevels.Warning, 3370, line);
        }

        private class NameGen
        {
            string dir;
            int cntr = 0;
            public NameGen(string dir)
            {
                this.dir = dir;
            }

            public string NewName()
            {
                cntr++;
                return System.IO.Path.Combine(dir, "tmpsortfile" + cntr.ToString());
            }
        }

        class SortParam
        {
            public double[] buffer; // TEMPLATE: data type
            public byte[] byteBuffer;
            public int len;
            //public int[] aux;
            public string name;
            public ManualResetEvent mre;
            public int id;

            public SortParam(int sz, ManualResetEvent mre, int id)
            {
                buffer = new double[sz]; // TEMPLATE: data type
                //aux = new int[sz]; //used for merge sort in old version
                byteBuffer = new byte[sizeof(double) * sz]; // TEMPLATE: data type
                this.mre = mre;
                this.id = id;
            }

            public void Reset(int len, string name)
            {
                this.len = len;
                this.name = name;
            }
        }

        static void SortFromParam(SortParam param)
        {
            //merge sort vs. dot net sort
            Array.Sort(param.buffer, 0, param.len);
            ArrayToFile(param.buffer, param.byteBuffer, param.len, param.name);
        }

        static void ThreadProc(Object state)
        {
            SortParam param = (SortParam)state;
            SortFromParam(param);
            param.mre.Set();//waiting thread can proceed
        }

        private static void SortOnThread(SortParam param)
        {
            param.mre.WaitOne();//wait for previous thread to complete
            param.mre.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), param);
        }

        private static string[] InitialSort(string inputFile, string tempDir, Options opt)
        {
            List<string> tempFiles = new List<string>();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //read a chunk, sort it, then write it to file
            using (StreamReader reader = new StreamReader(inputFile))
            {
                using (ManualResetEvent mre = new ManualResetEvent(true))
                {
                    NameGen nameGen = new NameGen(tempDir);
                    string line;
                    int lineNo = 0;
                    int maxSortSize = opt.MaxSortSize;
                    SortParam[] parameters = new SortParam[2];
                    parameters[0] = new SortParam(maxSortSize, mre, 0);
                    parameters[1] = new SortParam(maxSortSize, mre, 1);
                    int currentParam = 0;

                    double[] buffer = parameters[currentParam].buffer;  // TEMPLATE: data type
                    int j = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNo++;
                        double data;
                        //if (Int32.TryParse(line, out data)) //slow
                        if (StringToDouble(line, out data))  // Louise Evan's and Vlad Henzl's pulse files  // TEMPLATE: data type
                        {
                            buffer[j++] = data;
                            if (j >= maxSortSize)
                            {
                                string name = nameGen.NewName();
                                tempFiles.Add(name);
                                parameters[currentParam].Reset(j, name);
                                SortOnThread(parameters[currentParam]);
                                //swap
                                currentParam = (currentParam + 1) % 2;
                                buffer = parameters[currentParam].buffer;
                                j = 0;
                            }
                        }
                        else
                            Warn("Skipping line " + lineNo.ToString());
                    }
                    if (j > 0)
                    {
                        string name = nameGen.NewName();
                        tempFiles.Add(name);
                        parameters[currentParam].Reset(j, name);
                        SortOnThread(parameters[currentParam]);
                    }
                    mre.WaitOne();
                    parameters[0].mre = null; parameters[1].mre = null;
                }
                reader.Close();
            }
            sw.Stop();
            ExternalMergeSort.Logger.TraceEvent(LogLevels.Verbose, 3360, "Time to sort intermediate files: {0}", sw.Elapsed.ToString());

            return tempFiles.ToArray();
        }

        public static void Sort(string inputFile, string outputFile, string tempDir, Options opt)
        {
            string[] tempFiles = null;
            if (opt.SkipInitialSort)
                tempFiles = Directory.EnumerateFiles(tempDir).ToArray();
            else
                tempFiles = InitialSort(inputFile, tempDir, opt);
            //merge the resulting small files
            ExternalMergeSort.Logger.TraceEvent(LogLevels.Info, 3350, "Number of files to merge: {0}", tempFiles.Length);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            MergeMany(tempFiles, outputFile, opt.MergeFileSize, opt.OutputBufferSize);
            sw.Stop();
            ExternalMergeSort.Logger.TraceEvent(LogLevels.Verbose, 3370, "Time to merge intermediate files: {0}", sw.Elapsed.ToString());
            if (opt.RemoveIntermediateFiles)
            {
                foreach (string f in tempFiles)
                {
                    ExternalMergeSort.Logger.TraceEvent(LogLevels.Verbose, 3381, "Removing intermediate file {0}", f);
                    File.Delete(f);
                }
            }
        }
    }

    static class ByteArray
    {

        public static unsafe void FromDouble(double[] input, int inputLen, byte[] output)
        {
            if (output.Length < 8 * inputLen || inputLen > input.Length)
                throw new Exception("ByteArray.FromDouble index error");

            fixed (byte* fixedOutput = output)
            {
                fixed (double* fixedInput = input)
                {
                    double* outputIntPtr = (double*)fixedOutput;
                    double* inputPtr = fixedInput;
                    for (int i = 0; i < inputLen; i++)
                    {
                        *outputIntPtr++ = *inputPtr++;
                    }
                }
            }
        }
        public static unsafe void ToDouble(byte[] input, double[] output, int outputLen)
        {
            if (input.Length < 8 * outputLen || outputLen > output.Length)
                throw new Exception("ByteArray.ToInt Index error");

            fixed (double* fixedOutput = output)
            {
                fixed (byte* fixedInput = input)
                {
                    double* outputPtr = fixedOutput;
                    double* inputIntPtr = (double*)fixedInput;
                    for (int i = 0; i < outputLen; i++)
                    {
                        *outputPtr++ = *inputIntPtr++;
                    }
                }
            }
        }

        public static unsafe void FromInt(int[] input, int inputLen, byte[] output)
        {
            if (output.Length < 4 * inputLen || inputLen > input.Length)
                throw new Exception("ByteArray.FromInt index error");

            fixed (byte* fixedOutput = output)
            {
                fixed (int* fixedInput = input)
                {
                    int* outputIntPtr = (int*)fixedOutput;
                    int* inputPtr = fixedInput;
                    for (int i = 0; i < inputLen; i++)
                    {
                        *outputIntPtr++ = *inputPtr++;
                    }
                }
            }
        }

        public static unsafe void ToInt(byte[] input, int[] output, int outputLen)
        {
            if (input.Length < 4 * outputLen || outputLen > output.Length)
                throw new Exception("ByteArray.ToInt Index error");

            fixed (int* fixedOutput = output)
            {
                fixed (byte* fixedInput = input)
                {
                    int* outputPtr = fixedOutput;
                    int* inputIntPtr = (int*)fixedInput;
                    for (int i = 0; i < outputLen; i++)
                    {
                        *outputPtr++ = *inputIntPtr++;
                    }
                }
            }
        }

        public static unsafe UInt32 ToUInt32(byte[] input)
        {
            if (input.Length < 4)
                throw new Exception("ByteArray.ToUInt32 Index error");

            UInt32[] output = new UInt32[1];
            fixed (UInt32* fixedOutput = output)
            {
                fixed (byte* fixedInput = input)
                {
                    UInt32* outputPtr = fixedOutput;
                    UInt32* inputIntPtr = (UInt32*)fixedInput;
                    *outputPtr++ = *inputIntPtr++;
                }
            }
            return output[0];
        }
    }
    #endregion big file sort technique

    #region file list generic, recurses and iterates
    public class FileListState
    {
        public int cur;
        public int count;
    }

    public class FileList<T> : List<T> where T : INeutronDataFile, new()
    {
        public FileListState state;

        List<string> Extensions;

        LMLoggers.LognLM log;

        public FileList(List<string> ext, LMLoggers.LognLM log)
        {
            this.log = log;
            Extensions = new List<string>(ext);
            state = new FileListState();
        }

        public FileList()
        {
            Extensions = new List<string>();
            state = new FileListState();
        }
        public void Init(List<string> ext, LMLoggers.LognLM log)
        {
            this.log = log;
            Extensions = new List<string>(ext);
            state = new FileListState();
        }


		/// <summary>
		///
		/// get the list of files from the named folder, check for other conditions if not a folder
		/// if this a single file then
		///  if the file is an T file then
		///    run with an T FileList of one file
		///  NYI: if the file is a compressed archive then
		///     unpack the archive all at once into a temp folder and then construct the list OR unpack 1 at a time as the list is processed?
		///
		/// </summary>
		/// <param name="dir">The root folder to examine for files, always NC.App.AppContext.FileInput</param>
		/// <param name="recurse">use subfolders or not</param>
		/// <returns>A List of T type files</returns>
		public List<T> BuildFileList(string dir, bool recurse, bool sort)
		{

			bool folder = false, singlefile = false, oneOfTheChosen = false, none = false; // compressedPerhaps = false, 
			folder = Directory.Exists(dir);
			System.IO.FileInfo fi = null;
			if (singlefile = File.Exists(dir))
			{
				fi = new System.IO.FileInfo(dir);
				if (Extensions.Exists(ext => ext.ToLower().EndsWith(fi.Extension.ToLower())))
				{
					oneOfTheChosen = true;
				}
				//if ((fi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed)
				//{
				//	compressedPerhaps = true;
				//}
			}

			SearchOption so = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			IEnumerable<string> effs = null;
			if (folder)
			{
				foreach (string extension in Extensions)
				{
					var leffs = from f in
									Directory.EnumerateFileSystemEntries(dir, "*" + extension, so)
								select f;
					if (effs == null)
						effs = leffs;
					else
						effs = effs.Concat(leffs);
				}
			} else if (singlefile)
			{
				if (oneOfTheChosen)
					effs = from f in
							   Directory.EnumerateFileSystemEntries(fi.DirectoryName, fi.Name)
						   select f;
				//if (compressedPerhaps)
				//{
				//	log.TraceInformation(dir + " might be compressed, or not. The API is ambiguous");
				//}
			}

			FileList<T> files = null;
			if (!folder && !singlefile)
			{
				log.TraceInformation(dir + " cannot be processed, folder or file not found");
				none = true;
			} else if (folder)
			{
				if (effs == null || (effs.Count() <= 0))
				{
					string s = string.Empty;
					Extensions.ForEach(i => s += i + ", ");
					s = s.TrimEnd(new char[] { ' ', ',' });
					log.TraceInformation("No {0} files found in {1}, . . .", s, dir);
					none = true;
				}
				if (!none)
				{
					if (recurse)
						log.TraceInformation("Processing {0} files from {1} and its subfolders", effs.Count(), dir);
					else
						log.TraceInformation("Processing {0} files in {1}", effs.Count(), dir);
				}
			} else if (singlefile)
			{
				if (effs == null || (effs.Count() <= 0))
				{
					log.TraceInformation("{0} cannot be processed, . . .", dir);
					none = true;
				} else
					log.TraceInformation("Processing {0}", dir);
			}

			if (NC.App.Opstate.IsQuitRequested)  // cancellation allowed only in between files
				none = true;

			if (none)
				return null;

			files = new FileList<T>(Extensions, log);

			files.state.cur = 0;
			// Show files and build list
			foreach (var f in effs)
			{
				string name = f.Substring(f.LastIndexOf("\\") + 1); // Remove path information from string
				log.TraceEvent(LogLevels.Verbose, 406, "  {0}", name);
				T n = new T();
				n.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Data);
				n.Num = files.state.cur++;
				n.Filename = f;
				n.ExtractDateFromFilename();
				n.ThisSuffix = name.Substring(name.IndexOf('.'));
				files.Add(n);
			}
			files.state.count = files.Count;

			if (sort)
				files.Sort((f1, f2) => {
					return f1.DTO.CompareTo(f2.DTO);
				});

			return files;
		}

		public List<T> BuildFileList(List<string> ufiles)
        {

            FileList<T> files = new FileList<T>(Extensions, log);

            files.state.cur = 0;
            // Show files and build list
            foreach (string f in ufiles)
            {
                string name = f.Substring(f.LastIndexOf("\\") + 1); // Remove path information from string
                log.TraceEvent(LogLevels.Verbose, 406, "  {0}", name);
                T n = new T();
                n.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Data);
                n.Num = files.state.cur++;
                n.Filename = f;
                n.ExtractDateFromFilename();
                n.ThisSuffix = name.Substring(name.IndexOf('.'));
                files.Add(n);
            }
            files.state.count = files.Count;

            return files;
        }

    }

    #endregion file list generic, recurses and iterates


}
