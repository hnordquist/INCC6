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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NCCReporter
{


    // only for gen'ing CSV and text files for now

    // to create a tabular output report (set up columns)
    //
    // create file object
    // 
    // generate row content
    // save the results out to the file
    public class TabularReport: IDisposable
    {
        public char separator;
        public string sepAsString;
        ResultsOutputFile f;  // for the physical file management
        public HeadFoot hf;  // for the columns and so on
        public Row[] rows; // each row?
        protected LMLoggers loggers;

        public string FullFilePath
        {
            get { return f.filename; }
        }

        /// <summary>
        /// The final formatted report content, line by line
        /// </summary>
        public List<string> lines;

        /// <summary>
        /// specify the field separator for an output line by character
        /// The file suffix is inferred from the value
        ///  ',',  suffix = "csv"
        ///     '\t'  suffix = "dat"
        ///   all others I make a text file -> suffix = "txt"
        /// </summary>
        public char Separator
        {
            set
            {
                separator = value;
                sepAsString = new string(separator, 1);
            }
        }

        /// <summary>
        /// Create a container for generating a line-based report, defaults to CSV fields and file suffix
        /// </summary>
        /// <param name="et">the column header type</param>
        /// <param name="loggers">the logger handle, lines are output to the logger as well as the output file</param>
        public TabularReport(System.Type et, LMLoggers loggers)
        {
            this.loggers = loggers;
            Separator = '\t';
            GenColumns(et);
            rows = new Row[0]; // non-null to start
            f = new ResultsOutputFile(loggers.ControlLogger);
        }

        /// <summary>
        /// Create a container for generating a line-based report, defaults to CSV fields and file suffix
        /// </summary>
        /// <param name="loggers">the logger handle, lines are output to the logger as well as the output file</param>
        public TabularReport(LMLoggers loggers)
        {
            this.loggers = loggers;
            Separator = '\t';
            rows = new Row[0]; // non-null to start
            f = new ResultsOutputFile(loggers.ControlLogger);
        }

        public void GenColumns(System.Type et)
        {
            hf = new HeadFoot(et);
        }

        public bool CreateOutputFile(string loc, string name, string suffixoverride)
        {
            string suffix;
            switch (separator)
            {
                case ',': suffix = "csv"; break;
                case '\t': suffix = "csv"; break;
                case '|': suffix = "txt"; break;
                default: suffix = "txt"; break;
            }
            f.name = name;
            if (!String.IsNullOrEmpty(suffixoverride))
                suffix = suffixoverride;
            f.GenName(loc, suffix);
            return f.CreateForWriting();
        }


        // assumes file is created and open, header and footer text is set and rows constructed.
        public virtual void CreateReport(UInt16 logResults)
        {
			bool sendToLogFile = false, logToConsole = false;
            switch (logResults)
            {
                case 1:
                    sendToLogFile = true;
                    break;
                case 2:
                    logToConsole = true;
                    break;
                case 3:
                    sendToLogFile = logToConsole = true;
                    break;
            }
			if (sendToLogFile || logToConsole)
            {
                LMLoggers.LognLM log = loggers.AppLogger;
				if (log != null) log.TraceEvent(LogLevels.Info, 111, "Using output file: " + f.filename);
			}

            lines = new List<string>(2 + rows.Length + 1);

            if (hf != null)
            {
                lines.Add(hf.header);
                lines.Add(hf.GetColumnHeader());
            }

            string s;
            for (int i = 0; i < rows.Length; i++)
            {
                Row r = rows[i];
                if (r != null)
                    s = rows[i].ToLine(separator);
                else
                    s = sepAsString;
                lines.Add(s);
            }

            if (hf != null)
                lines.Add(hf.footer);

            if (f != null)
                foreach (string ls in lines)
                {
                    f.WriteLine(ls);
                }

            if (sendToLogFile || logToConsole)
            {
                LMLoggers.LognLM log = loggers.AppLogger;
                TraceEventCache tec = new TraceEventCache();
                foreach (string ls in lines)
                {
                    if (sendToLogFile && logToConsole)
                        log.TraceEvent(LogLevels.Verbose, 717, ls);
                    else if (logToConsole)
                        log.TraceEventConsole(LogLevels.Verbose, 717, ls, tec);
                    else
                        log.TraceEventFileOnly(LogLevels.Verbose, 717, ls, tec);
                }
            }
        }

        public void CloseOutputFile()
        {
            f.Dispose();
			f = null;
        }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					CloseOutputFile();

					rows = null;
				}

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TabularReport() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

    }


    // a row is an enum array's worth of strings, just like Columns
    public class Row : SortedDictionary<int, string>
    {
        public Row()
        {
            TS = Default;
        }

        public string Default(ValueType v)
        {
            return v.ToString();
        }
        public delegate string headertransform(ValueType v);
        public headertransform TS;

        public void GenFromEnum(System.Type et, string rowindex = null, int repeat = 1)
        {
            int count = System.Enum.GetValues(et).Length;
            int i = 0;
            if (rowindex != null)
            {
                Add(i, rowindex); i++;
            }
            int r = 0;
            do
            {
                foreach (ValueType v in System.Enum.GetValues(et))
                {
                    Add(i, TS(v));
                    i++;
                }
                r++;
            } while (r < repeat);
        }

        public string ToLine(char separator = '\t' )//tab delimited instead
        {

            string l = "";
            foreach (KeyValuePair<int, string> pair in this)
            {
                // assume no gaps in list, otherwise need a filler extender here for missing row entries
                l += (pair.Value + separator);
            }
            return l;
        }

    }

    public class Section : List<Row>
    {

        System.Type ch;
        short prelines;
        //short postlines;
        int datarows;
        // int cur;
        public int dataidx;
        int fulllen;

        public Section(System.Type column, short prelines)
        {
            //ASSERT(labelrows != null);
            this.prelines = prelines;

            this.ch = column; // null means no header row

            // the padders
            for (long i = 0; i < prelines; i++)
                Add(new Row());
        }

        public void AddLabelAndColumn(Row label, string rowindexlabel = null)
        {
            Add(label);
            // build the column header row (optionally indexed)
            if (ch != null)
            {
                Row hcrow = new Row();
                hcrow.GenFromEnum(ch, rowindexlabel);
                Add(hcrow);
            }
        }

        //add repeater for multiple analyzer column sets across the report
        public Section(System.Type column, short labellines, short prelines, int datarows, string rowindexlabel = null, int repeat = 0)
        {
            this.prelines = prelines;
            this.datarows = datarows;
            this.ch = column; // null means no header row
            int hcount = 0;
            if (ch != null)
                hcount = 1;
            dataidx = (hcount + labellines) + prelines;
            fulllen = dataidx;
            fulllen += datarows;
            this.Capacity = fulllen;

            // the padders
            for (long i = 0; i < prelines; i++)
                Add(new Row());

            // skip past the label lines
            for (long i = 0; i < labellines; i++)
                Add(new Row());

            // build the column header row (optionally indexed)
            if (ch != null)
            {
                Row hcrow = new Row();
                hcrow.GenFromEnum(ch, rowindexlabel, repeat);
                Add(hcrow);
            }
        }

        //public void Add(Row r)
        //{
        //    this[cur++] = r;
        //}

    }


    public class ResultsOutputFile: IDisposable
    {
        private LMLoggers.LognLM log;

        public string prefixPath;
        public string filename;
        public StreamWriter writer;
        public FileStream stream;
        public string name;
        string suffix;

        public ResultsOutputFile(LMLoggers.LognLM logger)
        {
            log = logger;
        }

        //  when encountering the same file name, create a file name with a index count addendum
        public bool CreateForWriting()
        {
            try
            {
                int i = 1;
                while (File.Exists(filename))
                {
                    ConstructFullPathName("_" + i.ToString("X2")); //a simple counter
                    i++;
                }

                if (log != null) log.TraceEvent(LogLevels.Info, 111, "Creating new output file: " + filename);

                string path = Path.GetDirectoryName(filename);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                stream = File.Create(filename);
                writer = new StreamWriter(stream, System.Text.Encoding.Unicode);
                return true;
            }
            catch (Exception e)
            {
                if (log != null) log.TraceException(e);
                return false;
            }
        }

        private void ConstructFullPathName(string opt = "")
        {
            // replace offending chars in the name/filename before use
            filename = prefixPath + CleansePotentialFilename(name + opt + "." + suffix);
        }

		private static string CleansePotentialFilename(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            char[] ca = s.ToCharArray();
            int i;
            char dchar;

            System.Text.StringBuilder text = new System.Text.StringBuilder();
            char[] inv = Path.GetInvalidFileNameChars();
            for (i = 0; i < ca.Length; i++)
            {
                dchar = ca[i];
                foreach (char c in inv)
                    if (dchar == c)
                    {
                        dchar = '_';
                        break;
                    }
                text.Append(dchar);
            }
            return text.ToString();
        }

        public void GenName(string locPrefix, string suffix)
        {
            this.suffix = suffix;
            prefixPath = locPrefix;
            if (!prefixPath.EndsWith("\\"))
            {
                prefixPath += "\\";
            }
            ConstructFullPathName();
        }
        public void WriteLine(string line)
        {
            if (writer != null)
                writer.WriteLine(line);
        }
        public void CloseWriter()
        {
            log.TraceEvent(LogLevels.Verbose, 115, "Closing " + filename);
            try
            {
                if (writer != null)
                {
                    writer.Close();
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                if (log != null) log.TraceException(e);
            }
        }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					CloseWriter();
					writer = null;
					stream = null;
				}

				disposedValue = true;
			}
		}

		// override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ResultsOutputFile() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
    }
}
