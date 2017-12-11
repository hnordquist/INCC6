﻿/*
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
using System.Collections;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualBasic.Logging;

namespace NCCReporter
{


    public enum LogLevels
    {
        None = 0, Critical = TraceEventType.Critical, Error = TraceEventType.Error,
        Warning = TraceEventType.Warning, Info = TraceEventType.Information, Verbose = TraceEventType.Verbose
    };

    /// Keep LMLoggers for LANL namespace visibility, change elsewhere to the clearer 'Logging' name
    //// Some non-core classes and methods were moved into the core project after the first division in 2014
    public class Logging : LMLoggers
    {
        public Logging(NCCConfig.Config cfg) : base(cfg)
        {

        }
    }

    public class Log : LMLoggers.LognLM
    {
        public Log(string section, NCCConfig.Config cfg, int pid) : base(section, cfg, pid)
        {

        }
    }

    // dev note: the multiple loggers created here appear to each create a deferred procedure thread in the process, 
    // dev note: consider reducing the number of loggers after release testing for the multiple thread performance impact
    public class LMLoggers
    {

        public enum AppSection { App, Data, Analysis, Control, Collect, DB };

        Hashtable reps = null;
        NCCConfig.Config cfg = null;

        public LognLM Logger(AppSection wp)
        {
            return (LognLM)reps[wp];
        }

        public LognLM AppLogger
        {
            get { return (LognLM)reps[AppSection.App]; }
        }
        public LognLM DataLogger
        {
            get { return (LognLM)reps[AppSection.Data]; }
        }
        public LognLM ControlLogger
        {
            get { return (LognLM)reps[AppSection.Control]; }
        }
        public LMLoggers(NCCConfig.Config cfg)
        {
            this.cfg = cfg;
            Array a = Enum.GetValues(typeof(AppSection));
            reps = new Hashtable(a.Length);
            int pid = Process.GetCurrentProcess().Id;

            foreach (AppSection wp in a)
            {
                Log l = new Log(wp.ToString(), cfg, pid);
                reps.Add(wp, l);
            }
            DB.DBMain.ConsoleQuiet = cfg.App.Quiet;
        }

        public void Flush()
        {
            foreach (AppSection a in Enum.GetValues(typeof(AppSection)))
            {
                Log l = (Log)reps[a];
                if (l != null)
                {
                    l.TS.Flush();
                }
            }
        }

        public void UpdateFilterLevel(ushort v)
        {
            foreach (Log l in reps.Values)
            {
                l.TS.Switch.Level = cfg.App.Level();
            }
        }

        // Keep LognLM for LANL namespace visibility, change elsewhere to the clearer 'Log' name
        // Some non-core classes and methods were moved into the core project after the first division in 2014


        public class LognLM
        {

            TraceSource ts = null;

            public TraceSource TS
            {
                get { return ts; }
                set { ts = value; }
            }

            public static string CurrentLogFilePath { get; set; }

            static string _uniquename;


            public LognLM(string section, NCCConfig.Config cfg, int pid)
            {
                ts = new TraceSource(section);

                if (cfg.App.Quiet)
                {
                    ts.Listeners.Remove("console");
                }

                try
                {
                    if (string.IsNullOrEmpty(_uniquename))
                        _uniquename = string.Format("INCC6" + GetVersionBranchString() + DateTime.Now.TimeOfDay.Ticks.ToString());
                    foreach (TraceListener item in ts.Listeners)
                    {
                        // every file logger points to the same file and has the same attributes, they are really a merged logger set (for now, until we need something else)
                        if (item is FileLogTraceListener)
                        {
                            FileLogTraceListener listener = (FileLogTraceListener)item;
                            if (!NCCConfig.Config.isDefaultPath(cfg.App.RootLoc))
                            {
                                listener.Location = LogFileLocation.Custom;
                                listener.CustomLocation = cfg.App.LogFilePath;
                            }
                            if (cfg.App.isSet(NCCConfig.NCCFlags.logLocation))
                            {
                                listener.BaseFileName = System.IO.Path.GetFileName(cfg.App.LogFilePathAndName);
                                listener.Location = LogFileLocation.Custom;
                                string x = System.IO.Path.GetDirectoryName(cfg.App.LogFilePathAndName);
                                if (string.IsNullOrEmpty(x) || NCCConfig.Config.isDefaultPath(x))
                                    listener.CustomLocation = cfg.App.LogFilePath;
                                else
                                    listener.CustomLocation = x;
                            }
                            else
                                listener.BaseFileName = _uniquename;
                            listener.Append = true;
                            listener.AutoFlush = false; // dev note: expose a setter for this property, to set true for critical passages
                            listener.MaxFileSize = cfg.App.RolloverSizeMB * 1024 * 1024;
                            // logdetails cmd line flag crudely enables this option set, only because the App.Config sharedListeners and switch source sections do not permit setting this attribute.
                            if (cfg.App.isSet(NCCConfig.NCCFlags.logDetails))
                                item.TraceOutputOptions |= cfg.App.LoggingDetailOptions;

                            if (string.IsNullOrEmpty(CurrentLogFilePath))
                                CurrentLogFilePath = listener.FullLogFileName;
                        }
                        else
                        {
                            // item.TraceOutputOptions |= (TraceOptions.DateTime | TraceOptions.ThreadId);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (cfg.App.IsLevelSet())  // use the cmd line or UI override, if one is present, 
                {
                    // test: but if the internal switch value was higher as set in app.config, this assignment is either overridden later or has no effect on the pre-constructed switches
                    if (ts != null)
                    {
                        ts.Switch.Level = cfg.App.Level();
                    }
                }

            }

            static string GetVersionBranchString()
            {
                string result = string.Empty;
                System.Reflection.Assembly entry = System.Reflection.Assembly.GetEntryAssembly();
                object[] o = entry.GetCustomAttributes(typeof(System.Reflection.AssemblyConfigurationAttribute), true);
                if (o != null && o.Length > 0)
                {
                    System.Reflection.AssemblyConfigurationAttribute aca = (System.Reflection.AssemblyConfigurationAttribute)o[0];
                    if (!string.IsNullOrEmpty(aca.Configuration))
                        result = aca.Configuration;
                }
                return result;

            }

            readonly string colossalErrorMsg = "Sometng whrong ihere";
            public void EmitFatalErrorMsg()
            {
                TraceEvent(LogLevels.Error, 0x2A2A, colossalErrorMsg);
            }
            public void Flush()
            {
                ts.Flush();
            }

            // use these pass-throughs for now


            //
            // Summary:
            //     Determines if trace listeners should be called, based on the trace event
            //     type.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values.
            //
            // Returns:
            //     True if the trace listeners should be called; otherwise, false.
            public bool ShouldTrace(LogLevels eventType)
            {
                return ts.Switch.ShouldTrace((TraceEventType)eventType);
            }

            //
            // Summary:
            //     Writes trace data to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified event type, event identifier, and trace data.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values that specifies the event
            //     type of the trace data.
            //
            //   id:
            //     A numeric identifier for the event.
            //
            //   data:
            //     The trace data.
            //
            // Exceptions:
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceData(LogLevels eventType, int id, object data)
            {
                ts.TraceData((TraceEventType)eventType, id, data);
            }
            //
            // Summary:
            //     Writes trace data to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified event type, event identifier, and trace data
            //     array.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values that specifies the event
            //     type of the trace data.
            //
            //   id:
            //     A numeric identifier for the event.
            //
            //   data:
            //     An object array containing the trace data.
            //
            // Exceptions:
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceData(LogLevels eventType, int id, params object[] data)
            {
                if (!ShouldTrace(eventType))
                    return;
                ts.TraceData((TraceEventType)eventType, id, data);
            }
            //
            // Summary:
            //     Writes a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified event type and event identifier.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values that specifies the event
            //     type of the trace data.
            //
            //   id:
            //     A numeric identifier for the event.
            //
            // Exceptions:
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceEvent(LogLevels eventType, int id)
            {
                ts.TraceEvent((TraceEventType)eventType, id);
            }
            //
            // Summary:
            //     Writes a trace event message to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified event type, event identifier, and message.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values that specifies the event
            //     type of the trace data.
            //
            //   id:
            //     A numeric identifier for the event.
            //
            //   message:
            //     The trace message to write.
            //
            // Exceptions:
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceEvent(LogLevels eventType, int id, string message)
            {
                ts.TraceEvent((TraceEventType)eventType, id, message);
            }

            public void TraceEventConsole(LogLevels eventType, int id, string message, TraceEventCache tec)
            {
                if (!ShouldTrace(eventType))
                    return;
                System.Diagnostics.TraceListener tl = ts.Listeners["console"];
                if (tl != null)
                    tl.TraceEvent(tec, ts.Name, (TraceEventType)eventType, id, message);
            }
            public void TraceEventConsoleUI(LogLevels eventType, int id, string message, TraceEventCache tec)
            {
                if (!ShouldTrace(eventType))
                    return;
                System.Diagnostics.TraceListener tl = ts.Listeners["WPF"];
                if (tl != null)
                    tl.TraceEvent(tec, ts.Name, (TraceEventType)eventType, id, message);
            }
            public void TraceEventFileOnly(LogLevels eventType, int id, string message, TraceEventCache tec)
            {
                if (!ShouldTrace(eventType))
                    return;
                System.Diagnostics.TraceListener tl = ts.Listeners["FileLog"];
                if (tl != null)
                    tl.TraceEvent(tec, ts.Name, (TraceEventType)eventType, id, message);
            }
            public void TraceEventButNotToUI(LogLevels eventType, int id, string message, TraceEventCache tec = null)
            {
                if (!ShouldTrace(eventType))
                    return;
                if (tec == null)
                    tec = new TraceEventCache();
                System.Diagnostics.TraceListener tl = ts.Listeners["FileLog"];
                if (tl != null)
                    tl.TraceEvent(tec, ts.Name, (TraceEventType)eventType, id, message);
                tl = ts.Listeners["console"];
                if (tl != null)
                    tl.TraceEvent(tec, ts.Name, (TraceEventType)eventType, id, message);
            }
            //
            // Summary:
            //     Writes a trace event to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified event type, event identifier, and argument
            //     array and format.
            //
            // Parameters:
            //   eventType:
            //     One of the System.Diagnostics.TraceEventType values that specifies the event
            //     type of the trace data.
            //
            //   id:
            //     A numeric identifier for the event.
            //
            //   format:
            //     A composite format string that contains text intermixed with zero or more
            //     format items, which correspond to objects in the args array.
            //
            //   args:
            //     An object array containing zero or more objects to format.
            //
            // Exceptions:
            //   System.ArgumentNullException:
            //     format is null.
            //
            //   System.FormatException:
            //     format is invalid.-or- The number that indicates an argument to format is
            //     less than zero, or greater than or equal to the number of specified objects
            //     to format.
            //
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceEvent(LogLevels eventType, int id, string format, params object[] args)
            {
                ts.TraceEvent((TraceEventType)eventType, id, format, args);
            }
            //
            // Summary:
            //     Writes an informational message to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified message.
            //
            // Parameters:
            //   message:
            //     The informative message to write.
            //
            // Exceptions:
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceInformation(string message)
            {
                ts.TraceInformation(message);
            }
            //
            // Summary:
            //     Writes an informational message to the trace listeners in the System.Diagnostics.TraceSource.Listeners
            //     collection using the specified object array and formatting information.
            //
            // Parameters:
            //   format:
            //     A composite format string that contains text intermixed with zero or more
            //     format items, which correspond to objects in the args array.
            //
            //   args:
            //     An array containing zero or more objects to format.
            //
            // Exceptions:
            //   System.ArgumentNullException:
            //     format is null.
            //
            //   System.FormatException:
            //     format is invalid.-or- The number that indicates an argument to format is
            //     less than zero, or greater than or equal to the number of specified objects
            //     to format.
            //
            //   System.ObjectDisposedException:
            //     An attempt was made to trace an event during finalization.
            [Conditional("TRACE")]
            public void TraceInformation(string format, params object[] args)
            {
                ts.TraceInformation(format, args);
            }

            public void TraceException(Exception ex, bool stack = false)	// Writes exception information to the application's log listeners.
            {
                ts.TraceEvent(TraceEventType.Error, 999, ex.Source + ":" + ex.Message);
                if (stack)
                    ts.TraceEvent(TraceEventType.Error, 998, FlattenChars(ex.StackTrace, '*', 0x269b)); // unicode atom symbol
            }

            // dev note: s = System.Text.RegularExpressions.Regex.Replace(s, @"[\r\n\\'""]", @"\$0");
            static public string FlattenChars(string s, char eolchar = '\\', int npchar = 46) // 46 is '.' //  unicode 0x26F7 is SKIER if your font has it LOL
            {
                if (string.IsNullOrEmpty(s))  // null to empty string, callers may now relax
                    return "";
                char[] ca = s.ToCharArray();
                int i;
                char dchar;
                //string npfiller = Char.ConvertFromUtf32(npchar);
                StringBuilder dumptext = new StringBuilder();
                for (i = 0; i < ca.Length; i++)
                {
                    dchar = ca[i];
                    // replace 'non-printable' chars with a '.'.
                    if ((dchar == '\r') || (dchar == '\n') || (dchar == '\v'))  // dev note: add in full flattener for other such chars
                    {
                        dchar = eolchar;
                    }
                    else if (Char.IsControl(dchar))
                    {
                        dchar = '.';
                    }
                    dumptext.Append(dchar);
                }
                return dumptext.ToString();
            }

        }
    }
}