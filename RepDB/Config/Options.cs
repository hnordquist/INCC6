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
using NDesk.Options;

namespace NCCConfig
{

    // a flat enum for now, this is a hack for the command line only, must be replaced with a full DB 
    public enum NCCFlags
    {
        root, dailyRootPath,
        serveremulation, emulatorapp,
        logging, level, quiet, logDetails, logFileLoc, loglocation, logResults, resultsFileLoc, openResults, results8Char, assayTypeSuffix, 
		rolloverSizeMB, rolloverIntervalMin, fpPrec,
        verbose, opStatusPktInterval, opStatusTimeInterval,
        assaytype,

        broadcast, port, broadcastport, subnet, wait,
        numConnections, receiveBufferSize, useAsyncFileIO, useAsyncAnalysis, parseBufferSize, streamRawAnalysis,

        leds, input, debug, hv, LLD, hvtimeout,

        lm, message, raw, includeConfig, 

        cycles, interval, separation, feedback,
        minHV, maxHV, step, hvduration, delay, hvx,

        detector, item, material, saveOnTerminate,

        fileinput, recurse, parseGen2, INCCXfer, replay, INCCParity,
        sortPulseFile, filterLMOutliers, datazConvert, pulseFileAssay, ptrFileAssay, mcaFileAssay, datazFileAssay, testDataFileAssay, reviewFileAssay, dbDataAssay, ncdFileAssay,
        autoCreateMissing, auxRatioReport,

        overwriteImportedDefs, liveFileWrite, gen5TestDataFile, MyProviderName, MyDBConnectionString,

		reportSect, query, tau, Tee, datazConvertParams
    }

	public partial class Config
    {
        public bool ParseCommandShellArgs()
        {
            _p = new OptionSet() {
            // program details
            { "V|version", "output version info and exit, use with -v=5 for full assembly list", b => cmd.ShowVersion = b != null},
			{ "config|show",  "show the current configuration and exit, shows XML config file content",  l =>  cmd.Showcfg = true  },
  			{ "h|?|help",  "show this message and exit", v => cmd.Showhelp = v != null },
  			{ "q|query",  "show current acquire state", v => cmd.Query = v ?? v },
            
             { "v:", "filter console output with {level}: default is 4, CRITICAL=1, ERROR=2, WARNING=3, INFO=4, DEBUG=5",  
                                            v => { if (v != null) { ushort uiv = 4; if (ushort.TryParse(v, out uiv)) app.SetVerbose(uiv); } } },
            { "log:", "enable logging with filter {level} (see -v)",  
                                            l => {app.Logging = true; if (l != null) app.SetLevel(ushort.Parse(l)); }},

            { "quiet", "disable console logging", q => {app.Quiet = (q != null); }},
            { "root=", "specify base {file location} for all input and output files", c => app.RootLoc = c},
  			{ "rootAutoPath",  "append a daily root file folder name to the current root file folder specification", 
                b => { if (b != null) app.DailyRootPath = true;} },

            { "logLoc=", "specify base {file location} for log files, overrides root", l => app.LogFilePath = l},
            { "logLocation=", "specify base {file location} for log files, including file name, overrides root", l => app.LogFilePathAndName = l},
            { "resultsLoc=", "specify base {file location} for results files, overrides root", r => app.ResultsFilePath = r},
            { "logDetails=", "integer flag specifying additional logging content details: for thread id use 16, (see System.Diagnostics.TraceOptions)",  
                                           (int n) => app.LoggingDetails = n},
            { "logResults=", "integer flag specifying results logging details: 0 none, 1 file only, 2 console/UI only, 3 all log listeners", (ushort n) => app.LogResults = n},
            { "openResults", "set true to open results files in notepad and Excel", b => app.OpenResults = b != null},
            { "results8Char", "set true to use the INCC5 YMDHMMSS results file naming scheme, false uses list mode results scheme", b => app.Results8Char = b != null},
            { "assayTypeSuffix", "set false to use .txt, true for the INCC5 file suffix style, e.g .VER for verification results...", b => app.AssayTypeSuffix = b != null},
            { "reportSect=", "char flags specifying report content sections to include or exclude, default \"d c i t\": ",  
                                           s => app.ReportSectional = s},
            { "prompt", "start in interactive prompt mode",  b => {if (b != null) acq.Action = 1;} },            
            { "discover", "send UDP discovery message on the LM subnet, then enter interactive prompt mode\r\n\r\nLMMM DAQ control ********************", 
                                            b => {if (b != null) acq.Action = 2;} },

             // this is for LMMM DAQ control, originally assumed as the primary input instrument
            { "subnet=", "subnet {mask} for LM network, defaults to 169.254.255.255",  s => netcomm.Subnet = s },
            { "port=", "port {number} for this app's LM TCP/IP listener, defaults to 5011",  (int n) => netcomm.Port = n },
            { "wait=", "milliseconds to wait for response to broadcast command, default 500",  (int n) => netcomm.Wait = n },
            { "broadcast", "use one or more uncoupled multiple LMs, default on", b => netcomm.Broadcast = b != null},
            { "synch", "use synchronous start director+pupil scheme for multiple LMs, default off", b => netcomm.Broadcast = b == null},
            { "lmport=", "port {number} used by LMs to listen for broadcast messages, def 5000",  (int n) => netcomm.LMListeningPort = n },
            { "input=", "LMMM input mode 0/TTL or 1/ribbon, defaults to 1\r\n\r\nLMMM DAQ tuning ********************", (int n) => lmmm.Input = n}, // dev note: current HW is defaulted at 1, so app must set to 0 manually upon start-up for NPS input to work
 
            // LMMM DAQ tuning
            { "connections=", "maximum {number} connections permitted for LM TCP/IP listener", (int n) => netcomm.NumConnections = n}, 
            { "buffer=", "{byte size} of input buffer for each LM TCP/IP listener connection, default is 8192 to match hardware value, change only with supervision", (int n) => netcomm.ReceiveBufferSize = n}, 
            { "eventbuff=", "{MB size} of input buffer for DAQ and NCD file input, default is 50MB (50), max is 1024", (uint n) => netcomm.ParseBufferSize = n}, 
            { "synchanalysis=", "use synchronous analysis processing, default true", bs => {if (bs != null) { bool b = false; bool.TryParse(bs, out b); netcomm.UseAsynchAnalysis = !b;}}},
            { "asynchio=", "use asynchronous NCD file processing, default false", bs => {if (bs != null) { bool b = false; bool.TryParse(bs, out b); netcomm.UseAsynchFileIO = b;}}},

            // neutron counting analysis tuning
            { "stream", "number of time:channel pairs to present to the neutron counting processor in a single operation, derived from eventbuff; on = eventbuff/8, off = 1\r\n\r\nAssay control ********************",
                                            (b) => { netcomm.UsingStreamRawAnalysis = (b != null);} }, // todo: should go on it's own config class, expand to include other analysis tuning parameters.

  
            // action commands
  
            // assay action and control parameters
            { "a|assay:", "start an assay using the current assay parameters, default type is Rates (0)\r\n           0 Rates, 1 Background, 2 InitialSource, 3 Normalization, 4 Precision, 5 Verification, 6 Calib", 
                                            v => { acq.Action = 3; if (v != null) acq.asAssayType(v); } },
            { "d|det=", "detector for the subsequent operations",  v => {if (v != null) acq.Detector = v; } },
            { "item=", "Item identifer to be used in assay",  v => {if (v != null) acq.ItemId = v; } },
            { "mat|material=", "Material or item type to be used in calibration or verification assay",
                    v => {if (v != null) acq.Material = v; } },
            { "includeConfig", "append copy of app config file to CSV results file, [false] ", b => acq.IncludeConfig = b != null}, 
            { "ncd|raw=", "location of this assay's MCA-527|PTR-32|pulse|LMMM NCD raw data file output, overrides root", 
                                            v => {if (v != null) acq.Raw = v; } },
            { "c|rep|cycles=", "{number} of cycles to run for this operation, defaults to 1, 0 means continuous",  (int n) => acq.Cycles = n },
            { "i|interval=", "interval or cycle time in {seconds}, defaults to 5", (double d) => acq.Interval = d}, 
            { "sep=", "LMMM network buffer send separation in {milliseconds}, defaults to 0, \"set higher if network freezes\"", (int n) => acq.Separation = n}, 
            { "feedback", "LMMM feedback flag, [false]", b => acq.Feedback = b != null},
            { "save|saveOnTerminate", "save results upon early termination, [true] ", b => acq.SaveOnTerminate = b != null}, 
            { "precision=", "precison of floating point results mass reporting output, default to INCC5 standard",
                                            (ushort n) => app.FPPrecision = n},
            { "liveFileWrite", "Create data files for each interval during live List Mode data collection",  v => app.LiveFileWrite = v != null },                                                           
            { "gen5TestDataFile", "Create an INCC5 Test data file at the end of a measurement",  v => app.CreateINCC5TestDataFile = v != null },                                                           

             // this section is for various files for input
            { "f|file|fileinput|datasource=", "start a file-based operation using the {location} for file input, all matching files in the folder will be used", 
                                            v => { if (v != null) app.FileInput = v; else app.FileInput = RootLoc; if (acq.Action != 3) acq.Action = 6;} },
            { "r|recurse", "look for files in subdirectories", b => app.Recurse = b != null },                                                           
            { "gen2", "identify and process ye olde generation 2 NCD files, adds extra processing time", v => app.ParseGen2 = v != null },
            { "datazConvert", "convert dataz data file intervals to INCC5 useable data files", v => app.DatazConvert = v != null },
            { "filterLMOutliers", "trim out certain outlier events in LM pulse trains", v => app.FilterLMOutliers = v != null },
            { "sortPulseFile", "sort and save pulse files, (line-delimited fixed-point strings, unsorted)", v => app.SortPulseFile = v != null },                                                           
            { "INCCXfer", "identify and process ye olde INCC Transfer files, (incomplete but worthy)",  v => app.INCCXfer = v != null },                                                           
            { "overwriteXfer", "replace existing definitions during each INCC Transfer operation",  v => app.OverwriteImportedDefs = v != null },                                                           
            { "INCCParity", "process mass calculations using INCC constraints",  v => app.INCCParity = v != null },                                                           
            { "replay", "run the last transfer measurement, valid only when the INCCXfer flag is specified",  v => app.Replay = v != null },                                                           
            { "pulseFileAssay", "use sorted pulse files for input", v => app.PulseFileAssay = v != null }, 
            { "ncdFileAssay", "use LMMM NCD files for input", v => app.NCDFileAssay = v != null }, 
            { "ptrFileAssay", "use PTR-32 dual file streams for input", v => app.PTRFileAssay = v != null }, 
            { "mcaFileAssay", "use MCA-527 file streams for input", v => app.MCA527FileAssay = v != null },
            { "datazFileAssay", "use Dataz file streams for input", v => app.DatazFileAssay = v != null },
            { "testDataFileAssay", "use INCC5 test data files (.DAT, .CNN) for input", v => app.TestDataFileAssay = v != null },
            { "dbDataAssay", "use existing measurement data (database) for input (next: no way to specify MeasId from cmd line though)", v => app.DBDataAssay = v != null },
            { "reviewFileAssay|import", "use Rad Review (.NCC) data files for input", v => app.ReviewFileAssay = v != null },
            { "datazConvertParams=", " 0 INCC5 test data file, 1 NCC Review file, 2 INCC5 xfer file, 3 INCC5 ini data detector and calibration files",
                                            (ushort n) => acq.DatazConvertType = n },
            { "LMFilterParams=", "interval in µ-seconds (1-64256) and cutoff count level {µ-seconds:neutrons}, defaults to 140 µ-seconds and 4 neutrons\r\n\r\nLMMM HV control ********************", 
                                            (b, s) => acq.LMFilterParams(b, s) },  

         
            // HV Calibration action and parameters
            { "hv|hvcalib:", "start an HV calibration using the current HV parameters", 
                                            v => {acq.Action = 4; if (v != null) acq.LM = int.Parse(v); } },
            { "hvx", "send HV calibration steps to Excel, might confuse user though", v => acq.HVX = v != null },                                                           
            { "hvrange=", "HV calibration minHV, maxHV voltages, defaults to 0, 2000 respectively",  
                                            (b, s) => acq.HVRange(b, s) },
            { "hvstep=", "HV calibration step voltages defaults to 10",  
                                            (int n) => acq.Step = n },
            { "hvi|hvintervals|hvtimes=", "step duration and delay after step is complete in {seconds:seconds}, defaults to 1 and 2 seconds\r\n\r\nLMMM emulation for testing ********************", 
                                            (b, s) => acq.HVTime(b, s) },   

            { "m|msg|message=", "annotation for this action", v => {if (v != null) acq.Message = v; } },       // todo: message retention and use

			{ "INCC5Ini|emu|emo|emulator:", "Use INCC5 ini file for /import path source at {file location}", l => { app.UseINCC5Ini = true; if (l != null) app.INCC5IniLoc = l;}},
            };

            try
            {
                //extra = 
                _p.Parse(_args);
            }
            catch (OptionException e)
            {
                Console.Write(AppContextConfig.AppName + " bonk: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `incccmd --help' for more information");
                return false;
            }
            return true;
        }

    }
}