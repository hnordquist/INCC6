/*
Copyright 2016, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using NDesk.Options;

namespace NCCConfig
{

	public partial class Config
    {

        public AppContextConfig App
        {
            get { return app; }
        }
        public DBConfig DB
        {
            get { return db; }
            set { db = value; }
        }

        // Currently exposes the cmd line action override var only
        // rename this
        // carries LM runtime spec LMAcquireParams, as well as a few unrelated global flags
        public LMAcquireConfig Cur
        {
            get { return acq; }
            set { acq = value; }
        }


        // exposes console string ops independent of rest of program
        public CmdConfig Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }

        // cmd line overrides of default LM params, used to replace DB defaults for new LM definitions generated from cmd line only
        // the code has LM HW and Conn defaults used to create new LM detector defs. These values replace the code defaults.
        protected LMMMNetComm NetComm
        {
            get { return netcomm; }
            set { netcomm = value; }
        }

        protected LMMMConfig LMMM
        {
            get { return lmmm; }
            set { lmmm = value; }
        }



        private CmdConfig cmd;
        LMMMNetComm netcomm;
        LMAcquireConfig acq;
        AppContextConfig app;
        LMMMConfig lmmm;
        DBConfig db;

        public string AppName
        {
            get { return AppContextConfig.AppName; }
        }

        public string VersionString
        {
            get { return AppContextConfig.GetVersionString(); }
        }

        public string VersionBranchString
        {
            get { return AppContextConfig.GetVersionBranchString(); }
        }

        public string RootLoc
        {
            get { return App.RootLoc; }
        }

        private const string _defaultpath = @"./";
        public static string DefaultPath
        {
            get { return _defaultpath; }
        }

        public static bool isDefaultPath(string underconsideration) { return _defaultpath.Equals(underconsideration); }

        private const string _defaultreportsectional = @"d c i t";
        public static string DefaultReportSectional 
        {
            get { return _defaultreportsectional; }
        }

        public struct CmdParams { public object val; public bool set; public System.Type type; public bool sticky;}

        private Hashtable _parms;
        private string[] _args;

        public static Hashtable ParameterBasis()
        {
            Hashtable x = new Hashtable(System.Enum.GetValues(typeof(NCCFlags)).Length);

            foreach (NCCFlags e in System.Enum.GetValues(typeof(NCCFlags)))
            {
                CmdParams f = new CmdParams();
                x.Add(e, f);
            }
            return x;
        }

        public Config(String settingsFile)
        {
            _parms = ParameterBasis();  // alloc table
            db = new DBConfig(_parms);  // set up DB config
            ReadAppSettings(settingsFile); // get the DB settings
        }
        public Config()
        {
            _parms = ParameterBasis();  // alloc table
            db = new DBConfig(_parms);  // set up DB config
            ReadAppSettings(); // get the DB settings
        }

        public void AfterDBSetup(AppContextConfig appctx, string[] args)
        {
            _args = args;
            // sets hardcoded defaults for all values
            app = appctx;
            acq = new LMAcquireConfig(_parms);
            netcomm = new LMMMNetComm(_parms);
            lmmm = new LMMMConfig(_parms);
            cmd = new CmdConfig(_parms);            
        }


        // override default file input setting with active command action (if any) from cmd line
        //dev note:  UsingFileInput flag is retained in DB, and must be logically or directly overridden by cmd line presence of -prompt, -hv, -discover, -assay, 
        public void CmdLineActionOverride()
        {
            // Prompt = 1, Discover = 2, Assay = 3, HVCalibration = 4,
            switch (Cur.Action)
            {
                case 3:
                    if (!app.AssayFromFiles)
                        app.ResetFileInput();
                    break;
                case 1:
                case 2:
                case 4:
                    app.ResetFileInput();
                    break;
                default:
                    break;
            }
        }

        private OptionSet _p = null;

        public string[] ShowAppCfgLines()
        {

            string[] x = new string[500];
            int ix = 0;
            System.Configuration.Configuration config =
              ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            x[ix++] = "";

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(config.FilePath))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        x[ix++] = String.Copy(line);
                    }
                }
            }
            catch (Exception)
            {
                // Let the user know what went wrong.
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
            }
            Array.Resize(ref x, ix);
            return x;
        }
		public static string[] ShowCfgLines(Config cfg, bool fullAssembly, bool flush)
		{
			string[] x = new string[4096];
			int ix = 0;
			x[ix++] = "";

			string[] v = cfg.ShowVersionLines(fullAssembly);
			for (int i = 0; i < v.Length; i++)
				x[ix++] = v[i];
			if (flush)
			{
				cfg.RetainChanges();
			}

			string[] a = new string[0];
			string[] b = new string[0];
			bool richcontent = (cfg.acq.IncludeConfig || cfg.cmd.Showcfg);
			Configuration config =
			  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			if (config.HasFile)
			{
				x[ix++] = ("Config source: " + config.FilePath);
				if (richcontent)
				{
					a = cfg.ShowAppCfgLines();
					for (int i = 0; i < a.Length; i++)
						x[ix++] = a[i];
				}
			}
			//else
			{
				if (richcontent)
				{
					x[ix++] = "";
					x[ix++] = ("Config values from database, command line overrides and system defaults");
					b = ShowDefCfgLines(cfg);
					for (int i = 0; i < b.Length; i++)
						x[ix++] = b[i];
				}
			}

			x[ix++] = "";
			Array.Resize(ref x, ix);
			return x;
		}

		public static string[] ShowDefCfgLines(Config cfg)
        {
            string[] x = new string[100];
            int ix = 0;
            x[ix++] = "";

            string[] a;

            a = LMAcquireConfig.ToLines(cfg, cfg.acq);
            for (int i = 0; i < a.Length; i++)
                x[ix++] = a[i];
            x[ix++] = "";

            a = cfg.app.ToLines();
            for (int i = 0; i < a.Length; i++)
                x[ix++] = a[i];
            x[ix++] = "";

            a = cfg.netcomm.ToLines();
            for (int i = 0; i < a.Length; i++)
                x[ix++] = a[i];
            x[ix++] = "";

            a = cfg.db.ToLines();
            for (int i = 0; i < a.Length; i++)
                x[ix++] = a[i];
            x[ix++] = "";

            Array.Resize(ref x, ix);
            return x;
        }

        public static void ShowCfg(Config cfg, bool full)
        {
            string[] x = ShowCfgLines(cfg, full, false);
            for (int i = 0; i < x.Length; i++)
                Console.WriteLine(x[i]);
        }

        public void ShowHelp()
        {
            Cmd.ShowHelp(_p);
        }


        public string[] ShowVersionLines(bool full)
        {
            string[] x = new string[100];
            int ix = 0;

            if (full)
            {
                Assembly eas = Assembly.GetEntryAssembly();
                Assembly eax = Assembly.GetExecutingAssembly();
                x[ix++] = eas.FullName;
                x[ix++] = "using";
                string[] a = GetAssemblyVersionStrings(eas, eax);
                for (int i = 0; i < a.Length; i++)
                    x[ix++] = ("   " + a[i]);
            }
            else
            {
                x[ix++] = AppContextConfig.AppName + " " + AppContextConfig.GetVersionString();
            }
            x[ix++] = "";
            Array.Resize(ref x, ix);
            return x;
        }

        static string[] GetAssemblyVersionStrings(Assembly ea, Assembly xa)
        {
            HashSet<string> workingset = new HashSet<string>();
            HashSet<string> fullset = new HashSet<string>();
            fullset.Add(xa.FullName); // add the executing assembly, the root is shown by the caller
            AssemblyName[] ean = ea.GetReferencedAssemblies();
            AssemblyName[] xan = xa.GetReferencedAssemblies();
            for (int i = 0; i < ean.Length; i++)
            {
                workingset.Add(ean[i].FullName);
            }
            for (int i = 0; i < xan.Length; i++)
            {
                workingset.Add(xan[i].FullName);
            }

            foreach (string s in workingset)
            {
                Assembly a = null;
                AssemblyName an;
                try
                {
                    a = Assembly.Load(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                fullset.Add(s);
                if (a == null)
                    continue;
                an = a.GetName();
                if (an.Name.Equals("RepDB") || an.Name.Equals("NCCCore") || an.Name.Equals("NCCCtrl") || an.Name.Equals("Defs") ||
                    an.Name.Equals("INCCCmd") || an.Name.Equals("INCC6"))  // todo: this hack needs redeeming: the list can and will change
                {
                    System.Reflection.AssemblyName[] ann = a.GetReferencedAssemblies();
                    foreach (AssemblyName ns in ann)
                    {
                        fullset.Add(ns.FullName);
                    }
                }
            }
            string[] res = new string[fullset.Count];
            int j = 0;
            foreach (string s in fullset)
            {
                res[j++] = s;
            }
            return res;
        }



        public void RetainChanges()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            bool gottawrite = false;
            try
            {
                // save changed entries
                foreach (NCCFlags e in System.Enum.GetValues(typeof(NCCFlags)))
                {
                    CmdParams f = (CmdParams)_parms[e];
                    if (f.set && f.sticky)
                    {
                        // save it here, or flag to save all of them at once
                        gottawrite = true;
                        // dev note: better to use an extension method instead of this olde-fashioned loope
                        break;
                    }
                }
                if (gottawrite)
                {
                    // Get the application configuration file.
                    System.Configuration.Configuration config =
                      ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    // do a remove and add only on those that actually change, not the entire damn thing
                    foreach (NCCFlags e in System.Enum.GetValues(typeof(NCCFlags)))
                    {
                        CmdParams f = (CmdParams)_parms[e];
                        if (f.set && f.sticky)
                        {
                            config.AppSettings.Settings.Remove(e.ToString());
                            config.AppSettings.Settings.Add(e.ToString(), f.val.ToString());
                        }
                    }

                    // Save the configuration file.
                    config.Save(ConfigurationSaveMode.Modified);
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("[CreateAppSettings: {0}]", e.ToString());
            }
        }

        public void ReadAppSettings(String FullConfigFilePathAndName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(FullConfigFilePathAndName);

            try
            {
                // Get the AppSettings section and store DB info.
                System.Configuration.AppSettingsSection appSettings =
                    (System.Configuration.AppSettingsSection)config.AppSettings;
                foreach (var key in appSettings.Settings.AllKeys)
                {
                    Console.WriteLine("Key: {0} Value: {1}", key, appSettings.Settings[key]);
                    switch (key)
                    {
                        case "MyProviderName":
                            this.DB.MyProviderName = config.AppSettings.Settings[key].Value;
                            break;
                        case "MyDBConnectionString":
                            this.DB.MyDBConnectionString = config.AppSettings.Settings[key].Value;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("[ReadAppSettings: {0}]",
                    e.ToString());
            }
        }

        // Get the AppSettings section.        
        // This function uses the AppSettings property to read the appSettings configuration section.
        // The values are loaded into an internal params array with type markers.
        // All subsequent configuration set/get is through this in-memory parameter array.
        public void ReadAppSettings()
        {
            try
            {
                // Get the AppSettings section.
                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count <= 0)
                {
                    Assembly exa = System.Reflection.Assembly.GetExecutingAssembly();
                    Assembly ena = System.Reflection.Assembly.GetEntryAssembly();
                    ResourceManager rm = new ResourceManager("DB.Properties.Resources", exa);  // LMConfig
                    // now write the file using the exe name
                    string f2 = rm.GetString("AppConfig");
                    string f3 = ena.ManifestModule.FullyQualifiedName + ".Config";

                    try
                    {
                        Console.WriteLine("No application configuration file found, creating the file {0}, retry your operation", f3);

                        // Create the file
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(f3))
                        {
                            sw.Write(f2);  // write the XML
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    appSettings = ConfigurationManager.AppSettings;
                }

                // for (int i = 0; i < Math.Min(appSettings.Count, _parms.Count); i++)
                for (int i = 0; i < _parms.Count; i++)
                {

                    NCCFlags t = (NCCFlags)i;
                    Config.CmdParams x = ((Config.CmdParams)_parms[t]);

                    // dev note: funky type switch thang, must be a better way with reflection or something eh?
                    string v = (string)appSettings[t.ToString()];
                    if (v != null)
                    {
                        if (x.type == typeof(int))
                        {
                            Int32 res = 0;
                            Int32.TryParse(v, out res);
                            x.val = res;
                        }
                        if (x.type == typeof(int))
                        {
                            Int32 res = 0;
                            Int32.TryParse(v, out res);
                            x.val = res;
                        }
                        else if (x.type == typeof(bool))
                        {
                            bool res = false;
                            bool.TryParse(v, out res);
                            x.val = res;
                        }
                        else if (x.type == typeof(string))
                        {
                            x.val = v;
                        }
                        else if (x.type == typeof(ushort))
                        {
                            UInt16 res = 0;
                            UInt16.TryParse(v, out res);
                            x.val = res;
                        }
                        else if (x.type == typeof(uint))
                        {
                            UInt32 res = 0;
                            UInt32.TryParse(v, out res);
                            x.val = res;
                        }
                        else // dev note: oughta noughta evah get here
                            x.val = v;
                    }
                    x.set = false; _parms[t] = x;
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("[ReadAppSettings: {0}]",
                    e.ToString());
            }
        }
    }

    public class ConfigHelper
    {
        static private string eol = "\r\n";

        static public string Eol
        {
            get { return eol; }
        }


        protected Hashtable _parms;
        public Hashtable FineParamTable
        {
            get
            {
                return _parms;
            }
        }
        protected object getVal(NCCFlags t)
        {
            return ((Config.CmdParams)_parms[t]).val;
        }
        public bool isSet(NCCFlags t)
        {
            return ((Config.CmdParams)_parms[t]).set;
        }
        private Config.CmdParams getParam(NCCFlags t)
        {
            return (Config.CmdParams)_parms[t];
        }
        protected void resetVal(NCCFlags f, object c, System.Type t, bool retain = true)
        {
            Config.CmdParams x = ((Config.CmdParams)_parms[f]);
            x.val = c; x.set = false; x.type = t; x.sticky = retain; _parms[f] = x;
        }
        protected void setVal(NCCFlags f, object c)
        {
            Config.CmdParams x = ((Config.CmdParams)_parms[f]);
            x.val = c; x.set = true; _parms[f] = x;
        }

        // trim and clean up path first
        // e.g. the path could be "E:\LM\N-2 Grande\N-2 LM sample NCD and DAT files\2006_06_14 Ball Mill"
        // or "c:\\temp"
        protected string TrimCmdLineFlagpath(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return "";
			char[] charsToTrim = { '\"', '\'' };
            //string thawed = raw.Trim();
            string warmed = raw.Trim(charsToTrim);
            return warmed;
        }
    }

    public class AppContextConfig : ConfigHelper
    {
        public const string AppName = AssemblyConstants.ShortAssemblyProduct;
            
        public static string GetVersionString()
        {
			Assembly a = Assembly.GetEntryAssembly();
            Version MyVersion = a.GetName().Version;
			string result = MyVersion.ToString();
            string branch = GetVersionBranchString(a);
            if (!string.IsNullOrEmpty(branch))
                result = result + " " + branch;
            return result;
            // MyVersion.Build = days after 2000-01-01
            // MyVersion.Revision*2 = seconds after 0-hour  (NEVER daylight saving time)
        }

        public static string GetVersionBranchString(Assembly entry = null)
        {
            string result = string.Empty;
            if (entry == null)
                entry = Assembly.GetEntryAssembly();
            object[] o = entry.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
            if (o != null && o.Length > 0)
            {
                AssemblyConfigurationAttribute aca = (AssemblyConfigurationAttribute)o[0];
                if (!string.IsNullOrEmpty(aca.Configuration))
                    result = aca.Configuration;
            }
            return result;

        }

        static public string EightCharConvert(DateTimeOffset dto)
		{
			char[] table = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
			string y = dto.ToString("yy");
			char Y = y[y.Length-1];
			string M = string.Format("{0:X1}", dto.Month);
			char D = table[dto.Day];
			char H = table[dto.Hour + 10];
			string s = Y + M + D + H + dto.Minute.ToString("00") + dto.Second.ToString("00");

			return s;
		}


        public AppContextConfig()
        {
            setupmaps();
        }
        public AppContextConfig(Hashtable _parms)
        {
            setupmaps();
            this._parms = _parms;

            resetVal(NCCFlags.root, Config.DefaultPath, typeof(string));
            resetVal(NCCFlags.dailyRootPath, false, typeof(bool));
 
            resetVal(NCCFlags.logging, false, typeof(bool));
            //resetVal(LMFlags.logAutoPath, false, typeof(bool));
            resetVal(NCCFlags.logDetails, (int)TraceOptions.None, typeof(int));
            resetVal(NCCFlags.level, (ushort)4, typeof(ushort));
            resetVal(NCCFlags.rolloverIntervalMin, 30, typeof(int));
            resetVal(NCCFlags.rolloverSizeMB, 50, typeof(int)); /* (1024 * 1024), */
            resetVal(NCCFlags.logResults, (ushort)3, typeof(ushort)); // 0 none, 1 log file only, 2 console/UI only, 3 everywhere
            resetVal(NCCFlags.fpPrec, (ushort)3, typeof(ushort));
            resetVal(NCCFlags.openResults, false, typeof(bool));
            resetVal(NCCFlags.results8Char, true, typeof(bool));
            resetVal(NCCFlags.reportSect, Config.DefaultReportSectional, typeof(string));
            resetVal(NCCFlags.assayTypeSuffix, true, typeof(bool));
            resetVal(NCCFlags.logFileLoc, Config.DefaultPath, typeof(string));
            resetVal(NCCFlags.resultsFileLoc, Config.DefaultPath, typeof(string));
          
            resetVal(NCCFlags.verbose, (ushort)4, typeof(ushort));

            resetVal(NCCFlags.emulatorapp, Config.DefaultPath, typeof(string));
            resetVal(NCCFlags.serveremulation, false, typeof(bool));

            ResetFileInput();
            resetVal(NCCFlags.recurse, false, typeof(bool), retain: false);
            resetVal(NCCFlags.parseGen2, false, typeof(bool), retain: false);
            resetVal(NCCFlags.replay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.INCCParity, true, typeof(bool), retain: true);
            resetVal(NCCFlags.INCCXfer, false, typeof(bool), retain: false);
            resetVal(NCCFlags.sortPulseFile, false, typeof(bool), retain: false);
            resetVal(NCCFlags.pulseFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.ptrFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.mcaFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.dbDataAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.testDataFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.ncdFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.reviewFileAssay, false, typeof(bool), retain: false);
            resetVal(NCCFlags.opStatusPktInterval, (uint)128, typeof(uint)); /* every 1Mb, or 128 8192Kb, socket packet receipts, get the status from the analyzes, */
            resetVal(NCCFlags.opStatusTimeInterval, (uint)1000, typeof(uint)); /* status poll timer fires every 1000 milliseconds */
            resetVal(NCCFlags.autoCreateMissing, false, typeof(bool), retain: false);
            resetVal(NCCFlags.auxRatioReport, false, typeof(bool), retain: false);
            resetVal(NCCFlags.overwriteImportedDefs, false, typeof(bool));
            resetVal(NCCFlags.liveFileWrite, true, typeof(bool));
            resetVal(NCCFlags.gen5TestDataFile, false, typeof(bool));
        }

        private Hashtable srclevelmap = null; // for the .NET log listener filter only
        private Hashtable tracelevelmap = null; // for the mapping between .NET trace event type and LM Modules verbosity/level

        void setupmaps()
        {
            srclevelmap = new Hashtable();
            srclevelmap.Add((ushort)0, SourceLevels.Off);
            srclevelmap.Add((ushort)1, SourceLevels.Critical);
            srclevelmap.Add((ushort)2, SourceLevels.Error);
            srclevelmap.Add((ushort)3, SourceLevels.Warning);
            srclevelmap.Add((ushort)4, SourceLevels.Information);
            srclevelmap.Add((ushort)5, SourceLevels.Verbose);
            tracelevelmap = new Hashtable();
            tracelevelmap.Add((ushort)0, 0); // not defined /*LogLevels*/TraceEventType.None);
            tracelevelmap.Add((ushort)1, /*LogLevels*/TraceEventType.Critical);
            tracelevelmap.Add((ushort)2, /*LogLevels*/TraceEventType.Error);
            tracelevelmap.Add((ushort)3, /*LogLevels*/TraceEventType.Warning);
            tracelevelmap.Add((ushort)4, /*LogLevels*/TraceEventType.Information);
            tracelevelmap.Add((ushort)5, /*LogLevels*/TraceEventType.Verbose);
        }

        public List<string> FileInputList {get; set; }

        public void ResetFileInput() { resetVal(NCCFlags.fileinput, Config.DefaultPath, typeof(string)); }

        public bool UsingFileInput
        {
            get { return isSet(NCCFlags.fileinput); }
        }
        public bool AssayFromFiles
        {
            get { return UsingFileInput && (TestDataFileAssay || ReviewFileAssay || NCDFileAssay || MCA527FileAssay || PTRFileAssay || PulseFileAssay || DBDataAssay); }
        }

		private string overridepath(NCCFlags flag)
		{
			if (isSet(flag))
				return (string)getVal(flag);
			else
				return RootLoc;
		}

		public string FileInput
        {
            get
            {
				return overridepath(NCCFlags.fileinput);
            }
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                setVal(NCCFlags.fileinput, warmed);
            }
        }
        public string FileInputDBSetter
        {
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                if (!string.IsNullOrEmpty(warmed))
                    setVal(NCCFlags.fileinput, warmed);
            }
        }
        public bool Recurse
        {
            get { return (bool)getVal(NCCFlags.recurse); }
            set { setVal(NCCFlags.recurse, value); }
        }
        public bool ParseGen2
        {
            get { return (bool)getVal(NCCFlags.parseGen2); }
            set { setVal(NCCFlags.parseGen2, value); }
        }
        public bool Replay
        {
            get { return (bool)getVal(NCCFlags.replay); }
            set { setVal(NCCFlags.replay, value); }
        }
        public bool OverwriteImportedDefs
        {
            get { return (bool)getVal(NCCFlags.overwriteImportedDefs); }
            set { setVal(NCCFlags.overwriteImportedDefs, value); }
        }
        public bool LiveFileWrite
        {
            get { return (bool)getVal(NCCFlags.liveFileWrite); }
            set { setVal(NCCFlags.liveFileWrite, value); }
        }
        
        public bool CreateINCC5TestDataFile
        {
            get { return (bool)getVal(NCCFlags.gen5TestDataFile); }
            set { setVal(NCCFlags.gen5TestDataFile, value); }
        }
        public bool INCCParity
        {
            get { return (bool)getVal(NCCFlags.INCCParity); }
            set { setVal(NCCFlags.INCCParity, value); }
        }
        public bool INCCXfer
        {
            get { return (bool)getVal(NCCFlags.INCCXfer); }
            set
            {
                MutuallyExclusiveFileActions(NCCFlags.INCCXfer, value);
            }
        }

        public bool AutoCreateMissing
        {
            get { return (bool)getVal(NCCFlags.autoCreateMissing); }
            set { setVal(NCCFlags.autoCreateMissing, value); }
        }
        public bool AuxRatioReport
        {
            get { return (bool)getVal(NCCFlags.auxRatioReport); }
            set { setVal(NCCFlags.auxRatioReport, value); }
        }
        public bool TestDataFileAssay
        {
            get { return (bool)getVal(NCCFlags.testDataFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.testDataFileAssay, value); }
        }
        public bool DBDataAssay
        {
            get { return (bool)getVal(NCCFlags.dbDataAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.dbDataAssay, value); }
        }
        public bool ReviewFileAssay
        {
            get { return (bool)getVal(NCCFlags.reviewFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.reviewFileAssay, value); }
        }
        public bool PulseFileAssay
        {
            get { return (bool)getVal(NCCFlags.pulseFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.pulseFileAssay, value); }
        }
        public bool NCDFileAssay
        {
            get { return (bool)getVal(NCCFlags.ncdFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.ncdFileAssay, value); }
        }
        public bool PTRFileAssay
        {
            get { return (bool)getVal(NCCFlags.ptrFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.ptrFileAssay, value); }
        }
        public bool MCA527FileAssay
        {
            get { return (bool)getVal(NCCFlags.mcaFileAssay); }
            set { MutuallyExclusiveFileActions(NCCFlags.mcaFileAssay, value); }
        }
        public bool SortPulseFile
        {
            get { return (bool)getVal(NCCFlags.sortPulseFile); }
            set { MutuallyExclusiveFileActions(NCCFlags.sortPulseFile, value); }
        }

		public bool HasFileAction
		{
            get { return IsFileActionSet(); }
		}

		public string LogFilePath
        {
            get { return overridepath(NCCFlags.logFileLoc); }
            set { setIfNotOverride(NCCFlags.logFileLoc, value); }
        }

		public string ResultsFilePath
        {
            get { return  overridepath(NCCFlags.resultsFileLoc); }
            set { setIfNotOverride(NCCFlags.resultsFileLoc, value); }
        }

		private void setIfNotOverride(NCCFlags flag, string path)
		{
			// do not set if it is the override value
			Match m = PathMatch(path);
			if (m.Success) // if it is a daily path match, do not save it
				return;
			// if not a daily path match and not the current root path, go ahead and save it
			if (!path.Equals(RootPathOverride(), StringComparison.OrdinalIgnoreCase))
					setVal(flag, path);	
		}

        public string RootPath
        {
            get { return (string)getVal(NCCFlags.root); }
            set { setVal(NCCFlags.root, value); }
        }

		public string RootPathOverride()
		{
			if (DailyRootPath)
			{
				string part = DateTimeOffset.Now.ToString("yyyy-MMdd");
				if (RootPath.EndsWith(part))
					return RootPath;
				else
				{
					try
					{

						Match m = PathMatch(RootPath);
						if (m.Success)
						{
							// strip and replace
							string s = RootPath.Remove(m.Index);
							return Path.Combine(s, part);
						}
						return Path.Combine(RootPath, part);
					} catch (ArgumentException)  // illegal characters
					{
						return RootPath;
					}
				}

			} else
				return RootPath;
		}

		Match PathMatch(string path)
		{ 
			Match m = Regex.Match(path, "\\d{4}-\\d{4}$");
			return m;			
		}

        /// <summary>
        /// resolved with daily path generation every time this is called
        /// </summary>
        public string RootLoc
        {
            get
            {
                return RootPathOverride();
            }
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                RootPath = value;
            }
        }

        public bool DailyRootPath
        {
            get { return (bool)getVal(NCCFlags.dailyRootPath); }
            set { setVal(NCCFlags.dailyRootPath, value); }
        }

        public bool Logging
        {
            get { return (bool)getVal(NCCFlags.logging); }
            set { setVal(NCCFlags.logging, value); }
        }

        /// <summary>
        /// integer representation of enum TraceOptions bitmap flag  
        /// </summary>
        public Int32 LoggingDetails
        {
            get { return (Int32)getVal(NCCFlags.logDetails); }
            set { setVal(NCCFlags.logDetails, value); }
        }

        /// <summary>
        /// 0 none, 1 file only, 2 console/UI only, 3 everywhere
        /// </summary>
        public ushort LogResults
        {
            get { return (ushort)getVal(NCCFlags.logResults); }
            set { setVal(NCCFlags.logResults, value); }
        }
        /// <summary>
        /// 3 default, 
        /// </summary>
        public ushort FPPrecision
        {
            get { return (ushort)getVal(NCCFlags.fpPrec); }
            set { setVal(NCCFlags.fpPrec, value); }
        }

		/// <summary>
		/// Eight ASCII char file name scheme encodes date and time to 1s precision 
		///   YMDHMMSS
		/// Y = last digit of the year
		/// M = month (0-9, A-C)
		/// D = day (0-9, A-V)
		/// H = hour (A-X)
		/// MM = minutes (00-59)
		/// SS = seconds (00-59)
		/// </summary>
		public bool Results8Char
        {
            get { return (bool)getVal(NCCFlags.results8Char); }
            set { setVal(NCCFlags.results8Char, value); }
		}


		/// <summary>
		/// reportSect=
		///            d|de|detector        (default true)
		///            c|ca|calib           (default true)
		///            i|is|isotopics       (default true)
		///            r|rd|run_raw_data     (default false)
		///		    t|rr|run_rate        (default true)
		///		    m|rm|run_mult_dist    (default false)
		///            s|sr|rawsum |summed_raw_data     (default false)
		///		    e|sd|distsum|summed_mult_dist   (default false)
		/// </summary>
		public string ReportSectional
        {
            get { return (string)getVal(NCCFlags.reportSect); }
            set { setVal(NCCFlags.reportSect, value); }  // parsed by caller        
		}

		/// <summary>
		/// Rates only files have a suffix of .RTS
		/// Background files have a suffix of .BKG
		/// Initial source files have a suffix of .INS
		/// Normalization files have a suffix of .NOR
		/// Precision files have a suffix of .PRE
		/// Verification files have a suffix of .VER
		/// Calibration files have a suffix of .CAL
		/// Holdup files have a suffix of .HUP
		/// </summary>
		public bool AssayTypeSuffix
        {
            get { return (bool)getVal(NCCFlags.assayTypeSuffix); }
            set { setVal(NCCFlags.assayTypeSuffix, value); }
        }

        public bool OpenResults
        {
            get { return (bool)getVal(NCCFlags.openResults); }
            set { setVal(NCCFlags.openResults, value); }
        }

        public TraceOptions LoggingDetailOptions
        {
            get { return (TraceOptions)LoggingDetails; }
        }
        public ushort LoggingDestination
        {
            get { return (ushort)getVal(NCCFlags.logResults); }
            set { setVal(NCCFlags.logResults, value); }
        }
        public TraceEventType Verbose()  // LMLoggers.LogLevels
        {
			ushort l = Convert.ToUInt16(getVal(NCCFlags.verbose));
            if (l < tracelevelmap.Count)
                return (/*LogLevels*/TraceEventType)tracelevelmap[l];
            else
                return /*LogLevels*/TraceEventType.Error;
        }
        public void SetVerbose(ushort l)
        {
            setVal(NCCFlags.verbose, l);
        }
        public bool IsVerboseSet()
        {
            return isSet(NCCFlags.verbose);
        }

        public SourceLevels Level()
        {
            ushort l = System.Convert.ToUInt16(getVal(NCCFlags.level));
            if (l < srclevelmap.Count)
                return (SourceLevels)srclevelmap[l];
            else
                return SourceLevels.Error;
        }
        public ushort LevelAsUInt16
        {
            get {return Convert.ToUInt16(getVal(NCCFlags.level)); }

        }
        public void SetLevel(ushort l)
        {
            setVal(NCCFlags.level, l);
        }
        public bool IsLevelSet()
        {
            return isSet(NCCFlags.level);
        }

        public int RolloverSizeMB
        {
            get { return (int)getVal(NCCFlags.rolloverSizeMB); }
            set { setVal(NCCFlags.rolloverSizeMB, value); }
        }
        public int RolloverIntervalMin
        {
            get { return (int)getVal(NCCFlags.rolloverIntervalMin); }
            set { setVal(NCCFlags.rolloverIntervalMin, value); }
        }

        public uint StatusTimerMilliseconds
        {
            get { return (uint)getVal(NCCFlags.opStatusTimeInterval); }
            set { setVal(NCCFlags.opStatusTimeInterval, value); }
        }

        public uint StatusPacketCount
        {
            get { return (uint)getVal(NCCFlags.opStatusPktInterval); }
            set { setVal(NCCFlags.opStatusPktInterval, value); }
        }

        public bool UseINCC5Ini
        {
            get { return (bool)getVal(NCCFlags.serveremulation); }
            set { setVal(NCCFlags.serveremulation, value); }
        }
        public string INCC5IniLoc
        {
            get { return (string)getVal(NCCFlags.emulatorapp); }
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                setVal(NCCFlags.emulatorapp, warmed);
            }
        }

        public void MutuallyExclusiveFileActions(NCCFlags flag, bool val)
        {
            if (val)
            {
                setVal(NCCFlags.ncdFileAssay, false);
                setVal(NCCFlags.sortPulseFile, false);
                setVal(NCCFlags.INCCXfer, false);
                setVal(NCCFlags.testDataFileAssay, false);
                setVal(NCCFlags.reviewFileAssay, false);
                setVal(NCCFlags.pulseFileAssay, false);
                setVal(NCCFlags.ptrFileAssay, false);
                setVal(NCCFlags.mcaFileAssay, false);
                setVal(NCCFlags.dbDataAssay, false);
            }
            setVal(flag, val);
        }

		bool IsFileActionSet()
        {
			return 
				(bool)getVal(NCCFlags.testDataFileAssay) ||
                (bool)getVal(NCCFlags.reviewFileAssay) ||
                (bool)getVal(NCCFlags.ptrFileAssay) ||
                (bool)getVal(NCCFlags.mcaFileAssay) ||
                (bool)getVal(NCCFlags.dbDataAssay) ||
                (bool)getVal(NCCFlags.INCCXfer) ||
                (bool)getVal(NCCFlags.pulseFileAssay) ||
                (bool)getVal(NCCFlags.ncdFileAssay) ||
                (bool)getVal(NCCFlags.sortPulseFile);
        }

        public string[] ToLines()
        {
            string[] x = new string[100];
            int ix = 0;
            x[ix++] = "  verbose: " + Verbose();
            if (isSet(NCCFlags.root))
                x[ix++] = "  root: " + RootLoc;
            if (isSet(NCCFlags.dailyRootPath))
                x[ix++] = "  daily root path in use: " + DailyRootPath.ToString();
			if (isSet(NCCFlags.logFileLoc))
				x[ix++] = "  log file path: " + LogFilePath;
			if (isSet(NCCFlags.resultsFileLoc))
				x[ix++] = "  results path: " + ResultsFilePath;

            x[ix++] = "  logging: " + Logging;
            x[ix++] = "    log level: " + Level();
          //  x[ix++] = "    log folder: " + LogFileLoc;
            x[ix++] = "    log rollovers: " + RolloverSizeMB + " MB & " + RolloverIntervalMin + " minutes";
           // x[ix++] = "    daily log path in use: " + DailyLogPath.ToString();
            if (isSet(NCCFlags.logDetails))
                x[ix++] = "  log entry details: " + LoggingDetailOptions.ToString();
            if (isSet(NCCFlags.fpPrec))
                x[ix++] = "  FP precision: " + FPPrecision.ToString();
            if (isSet(NCCFlags.logResults))
                x[ix++] = "  log results: " + LogResults.ToString();

            if (isSet(NCCFlags.openResults))
                x[ix++] = "  open results: " + OpenResults.ToString();
            if (isSet(NCCFlags.assayTypeSuffix))
                x[ix++] = "  INCC5 YMDHMMSS results: " + Results8Char.ToString();
            if (isSet(NCCFlags.results8Char))
                x[ix++] = "  INCC5 suffix scheme: " + AssayTypeSuffix.ToString();

            x[ix++] = "  status update packet count: every " + StatusPacketCount + " receipts";
            x[ix++] = (UseINCC5Ini ? "  use INCC5 ini file at: " + INCC5IniLoc : "  no INCC5 ini file use");

			if (isSet(NCCFlags.fileinput))
			{
				string fis = " for input: " + FileInput;
				if (INCCXfer)
					x[ix++] = ("  use INCC Transfer files" + fis);
				else if (PulseFileAssay)
					x[ix++] = ("  use sorted pulse files" + fis);
				else if (PTRFileAssay)
					x[ix++] = ("  use PTR-32 file pairs" + fis);
				else if (MCA527FileAssay)
					x[ix++] = ("  use MCA-527 mca files" + fis);
				else if (TestDataFileAssay)
					x[ix++] = ("  use sorted pulse files" + fis);
				else if (ReviewFileAssay)
					x[ix++] = ("  use sorted pulse files" + fis);
				else if (NCDFileAssay)
					x[ix++] = ("  use LMMM NCD files" + fis);
				else
					x[ix++] = ("  (default) use LMMM NCD files" + fis);
			}
			if (!INCCXfer)
            {
                if (Recurse) x[ix++] = "  search subfolders for files";
                if (ParseGen2) x[ix++] = "  accept Gen. 2 NCD file format";
            }
            x[ix++] = (INCCParity ? "  INCC Parity, use INCC isotopics whole-day only decay constraint" : "  use isotopics fractional day decay calculations (more precise than INCC)");
            x[ix++] = "  OverwriteImportedDefs: " + OverwriteImportedDefs;
            x[ix++] = "  LiveFileWrite: " + LiveFileWrite;
            Array.Resize(ref x, ix);
            return x;
        }

        override public string ToString()
        {
            string[] x = ToLines();
            string s = "";
            for (int i = 0; i < x.Length; i++)
            {
                s += (x[i] + Eol);
            }
            return s;
        }
    }

    public class CmdConfig : ConfigHelper
    {
        private bool showcfgLiteral = false;
        public CmdConfig(Hashtable _parms)
        {
            this._parms = _parms;
        }

        public bool Showcfg
        {
            get;
            set;
		}
        public bool ShowLiteral
        {
            get;
        }
        public string ShowcfgLiteral
        {
            set { bool.TryParse(value, out showcfgLiteral); }
        }
        public bool Showhelp
        {
            get;
            set;
		}

        public bool ShowVersion
        {
            get;
            set;
		}

		public string Query
        {
            get;
            set;
        }

        public void ShowHelp(OptionSet _p)
        {
            Console.WriteLine("Usage: Cmd [OPTIONS]+");
            Console.WriteLine();
            Console.WriteLine("Options:");
            _p.WriteOptionDescriptions(Console.Out);
        }


        public void ShowVersionOnConsole(Config me, bool full)
        {
            string[] x = me.ShowVersionLines(full);
            for (int i = 0; i < x.Length; i++)
                Console.WriteLine(x[i]);
        }
    }

    public class LMMMNetComm : ConfigHelper
    {

        private const string defsubnet = "169.254.255.255";
        public IPAddress subnetip;
        public bool validip = false;

        public LMMMNetComm()
        {
        }
        public LMMMNetComm(LMMMNetComm src)
        {
            _parms = (Hashtable)src._parms.Clone();
            LMListeningPort = src.LMListeningPort; Broadcast = src.Broadcast; Port = src.Port; Wait = src.Wait; Subnet = String.Copy(src.Subnet);
            UseAsynchAnalysis = src.UseAsynchAnalysis; UseAsynchFileIO = src.UseAsynchFileIO; NumConnections = src.NumConnections; ReceiveBufferSize = src.ReceiveBufferSize;
            UsingStreamRawAnalysis = src.UsingStreamRawAnalysis; ParseBufferSize = src.ParseBufferSize;
        }
        public LMMMNetComm(Hashtable _parms)
        {
            this._parms = _parms;
            resetVal(NCCFlags.numConnections, 8, typeof(int));
            resetVal(NCCFlags.receiveBufferSize, 8192, typeof(int));
            resetVal(NCCFlags.parseBufferSize, (uint)50, typeof(uint)); // 50 MB
            resetVal(NCCFlags.useAsyncFileIO, false, typeof(bool));
            resetVal(NCCFlags.useAsyncAnalysis, false, typeof(bool));
            resetVal(NCCFlags.streamRawAnalysis, true, typeof(bool));
            resetVal(NCCFlags.broadcast, true, typeof(bool));
            resetVal(NCCFlags.port, 5011, typeof(int));
            resetVal(NCCFlags.broadcastport, 5000, typeof(int));
            resetVal(NCCFlags.subnet, defsubnet, typeof(string));
            resetVal(NCCFlags.wait, 500, typeof(int));
            validip = IPAddress.TryParse(defsubnet, out subnetip);
        }
        public bool UseAsynchAnalysis
        {
            get { return (bool)getVal(NCCFlags.useAsyncAnalysis); }
            set { setVal(NCCFlags.useAsyncAnalysis, value); }
        }
        public bool UseAsynchFileIO
        {
            get { return (bool)getVal(NCCFlags.useAsyncFileIO); }
            set { setVal(NCCFlags.useAsyncFileIO, value); }
        }
        public int NumConnections
        {
            get { return (int)getVal(NCCFlags.numConnections); }
            set { setVal(NCCFlags.numConnections, value); }
        }
        public int ReceiveBufferSize
        {
            get { return (int)getVal(NCCFlags.receiveBufferSize); }
            set { setVal(NCCFlags.receiveBufferSize, value); }
        }
        public uint ParseBufferSize
        {
            get { return (uint)getVal(NCCFlags.parseBufferSize); }
            set
            {
                if (value > 1024) value = 512;  // let's not go overboard LOL
                if (value < 1) value = 1;  // let's not go underboard LOL
                setVal(NCCFlags.parseBufferSize, value);
            }
        }

        public bool UsingStreamRawAnalysis
        {
            get { return (bool)getVal(NCCFlags.streamRawAnalysis); }
            set { setVal(NCCFlags.streamRawAnalysis, value); }
        }
        public uint StreamEventCount
        {
            get
            {
                uint v = (uint)getVal(NCCFlags.parseBufferSize);
                return (v * 1024 * 1024) / 8;
            }
        }
        public int LMListeningPort
        {
            get { return (int)(getVal(NCCFlags.broadcastport)); }
            set
            {
                setVal(NCCFlags.broadcastport, value);
            }
        }

        public bool Broadcast
        {
            get { return (bool)(getVal(NCCFlags.broadcast)); }
            set { setVal(NCCFlags.broadcast, value); }
        }

        public int Port
        {
            get { return (int)(getVal(NCCFlags.port)); }
            set { setVal(NCCFlags.port, value); }
        }
        public int Wait
        {
            get { return (int)(getVal(NCCFlags.wait)); }
            set { setVal(NCCFlags.wait, value); }
        }
        public string Subnet
        {
            get { return (string)(getVal(NCCFlags.subnet)); }
            set
            {
                setVal(NCCFlags.subnet, value);
                validip = IPAddress.TryParse(value, out subnetip);
                if (!validip)
                {
                    validip = IPAddress.TryParse(defsubnet, out subnetip);
                }
            }
        }

        public string[] ToLines()
        {
            string[] x = new string[100];
            int ix = 0;
            x[ix++] = "  LMMM subnet: " + Subnet;
            x[ix++] = "  LMMM sending port: " + Port;
            x[ix++] = "  LMMM broadcast response wait delay: " + Wait;
            x[ix++] = "  LMMM broadcast endpoint port: " + LMListeningPort;
            x[ix++] = (Broadcast ? "  LMMM broadcast go" : "  LMMM synchronous start");
            x[ix++] = "  LMMM connections: " + NumConnections;
            x[ix++] = "  LMMM TCP/IP receive buffer: " + ReceiveBufferSize + " bytes";
            x[ix++] = "  event processing buffer: " + ParseBufferSize + " MB (1024*1024)";
            if (UsingStreamRawAnalysis)
                x[ix++] = "  neutron counting event block count: " + StreamEventCount + " (" + StreamEventCount / (1024 * 1024) + " MB events)";
            else
                x[ix++] = "  using single event neutron counting feed";
            if (UseAsynchFileIO)
                x[ix++] = "  asynch file writer task enabled";
            if (UseAsynchAnalysis)
                x[ix++] = "  asynch analysis task enabled";
            Array.Resize(ref x, ix);
            return x;
        }

        override public string ToString()
        {
            string[] x = ToLines();
            string s = "";
            for (int i = 0; i < x.Length; i++)
            {
                s += (x[i] + Eol);
            }
            return s;
        }
    }

    public class LMMMConfig : ConfigHelper
    {
        public LMMMConfig()
        {
        }

        public LMMMConfig(LMMMConfig src)
        {
            _parms = (Hashtable)src._parms.Clone();
            LEDs = src.LEDs; Input = src.Input; Debug = src.Debug; HV = src.HV; LLD = src.LLD; HVTimeout = src.HVTimeout;
        }

        public LMMMConfig(Hashtable _parms)
        {
            this._parms = _parms;
            resetVal(NCCFlags.leds, 1, typeof(int));
            resetVal(NCCFlags.debug, 0, typeof(int));
            resetVal(NCCFlags.hv, 1650, typeof(int));
            resetVal(NCCFlags.LLD, 500, typeof(int));
            resetVal(NCCFlags.hvtimeout, 120, typeof(int));
            resetVal(NCCFlags.input, 1, typeof(int));
        }
        public int LEDs
        {
            get { return (int)getVal(NCCFlags.leds); }
            set { setVal(NCCFlags.leds, value); }
        }
        public int Input
        {
            get { return (int)getVal(NCCFlags.input); }
            set { setVal(NCCFlags.input, value); }
        }
        public int Debug
        {
            get { return (int)getVal(NCCFlags.debug); }
            set { setVal(NCCFlags.debug, value); }
        }
        public int HV
        {
            get { return (int)getVal(NCCFlags.hv); }
            set { setVal(NCCFlags.hv, value); }
        }
        public int LLD
        {
            get { return (int)getVal(NCCFlags.LLD); }
            set { setVal(NCCFlags.LLD, value); }
        }
		public int VoltageTolerance
        {
            get { return (int)getVal(NCCFlags.LLD); }
            set { setVal(NCCFlags.LLD, value); }
        }
        public int HVTimeout
        {
            get { int v = (int)getVal(NCCFlags.hvtimeout); if (v < 5) v = 30; return v; }
            set { setVal(NCCFlags.hvtimeout, value); }
        }
    }

    public class LMAcquireConfig : ConfigHelper
    {
        public LMAcquireConfig()
        {
        }

        public LMAcquireConfig(LMAcquireConfig src)
        {
            _parms = (Hashtable)src._parms.Clone();
            SaveOnTerminate = src.SaveOnTerminate;// DailyResultsPath = src.DailyResultsPath; 
            IncludeConfig = src.IncludeConfig; LM = src.LM; Message = String.Copy(src.Message);
            Feedback = src.Feedback; Separation = src.Separation;
            Cycles = src.Cycles; Interval = src.Interval; MinHV = src.MinHV; MaxHV = src.MaxHV;
            Step = src.Step; HVDuration = src.HVDuration; Delay = src.Delay; HVX = src.HVX;
            AssayType = src.AssayType; Detector = String.Copy(src.Detector);
            ItemId = src.ItemId; Material = String.Copy(src.Material);
        }
        public LMAcquireConfig(Hashtable _parms)
        {
            this._parms = _parms;
            resetVal(NCCFlags.feedback, false, typeof(bool));

            resetVal(NCCFlags.separation, (Int32)0, typeof(int));
            resetVal(NCCFlags.interval, (Double)5, typeof(Double)); // seconds
            resetVal(NCCFlags.cycles, (Int32)1, typeof(int));

            resetVal(NCCFlags.minHV, (Int32)0, typeof(int)); // volts
            resetVal(NCCFlags.maxHV, (Int32)2000, typeof(int)); // volts
            resetVal(NCCFlags.step, (Int32)10, typeof(int));// volts
            resetVal(NCCFlags.hvduration, (Int32)5, typeof(int)); // seconds
            resetVal(NCCFlags.delay, (Int32)2, typeof(int)); // seconds
            resetVal(NCCFlags.hvx, (bool)false, typeof(bool)); // uses external view for progess and results (e.g. Excel)

            resetVal(NCCFlags.saveOnTerminate, (bool)false, typeof(bool));
            //resetVal(LMFlags.resultsAutoPath, (bool)false, typeof(bool));
            //resetVal(LMFlags.results, Config.DefaultPath, typeof(string));
            resetVal(NCCFlags.includeConfig, (bool)false, typeof(bool), retain:false);
            resetVal(NCCFlags.message, "", typeof(string), retain: false);
            resetVal(NCCFlags.assaytype, 0, typeof(int));
            resetVal(NCCFlags.raw, Config.DefaultPath, typeof(string));
            resetVal(NCCFlags.lm, (Int32)(-1), typeof(int));

            resetVal(NCCFlags.detector, "Default", typeof(string));
            resetVal(NCCFlags.item, "", typeof(string));
            resetVal(NCCFlags.material, "Pu", typeof(string));

        }

        // dev note: the action itself is not preserved in the config state, but rather inferred from the presence of a required flag (-prompt, -discover, -assay, -hvcalib)
        private Int32 action = 0;

        public bool IncludeConfig
        {
            get { return (bool)getVal(NCCFlags.includeConfig); }
            set { setVal(NCCFlags.includeConfig, value); }
        }

        public string Detector
        {
            get { return (string)getVal(NCCFlags.detector); }
            set { setVal(NCCFlags.detector, value); }
        }

        public string ItemId
        {
            get { return (string)getVal(NCCFlags.item); }
            set { setVal(NCCFlags.item, value); }
        }

        public string Material
        {
            get { return (string)getVal(NCCFlags.material); }
            set { setVal(NCCFlags.material, value); }
        }

        public string Message
        {
            get { return (string)getVal(NCCFlags.message); }
            set { setVal(NCCFlags.message, value); }
        }

        public Int32 Action
        {
            get { return action; }
            set { action = value; }
        }
        public string Raw
        {
            get { return (string)getVal(NCCFlags.raw); }
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                setVal(NCCFlags.raw, warmed);
            }
        }
        public Int32 LM
        {
            get { return (Int32)getVal(NCCFlags.lm); }
            set { setVal(NCCFlags.lm, value); }
        }

        public bool Feedback
        {
            get { return (bool)getVal(NCCFlags.feedback); }
            set { setVal(NCCFlags.feedback, value); }
        }

        public Int32 Separation
        {
            get { return (Int32)getVal(NCCFlags.separation); }
            set { setVal(NCCFlags.separation, value); }
        }

        public Int32 Cycles
        {
            get { return (Int32)getVal(NCCFlags.cycles); }
            set { setVal(NCCFlags.cycles, value); }
        }
        public Double Interval
        {
            get { return (Double)getVal(NCCFlags.interval); }
            set { setVal(NCCFlags.interval, value); }
        }

        public Int32 MinHV
        {
            get { return (Int32)getVal(NCCFlags.minHV); }
            set { setVal(NCCFlags.minHV, value); }
        }

        public Int32 MaxHV
        {
            get { return (Int32)getVal(NCCFlags.maxHV); }
            set { setVal(NCCFlags.maxHV, value); }
        }

        public Int32 Step
        {
            get { return (Int32)getVal(NCCFlags.step); }
            set { setVal(NCCFlags.step, value); }
        }
        public Int32 HVDuration
        {
            get { return (Int32)getVal(NCCFlags.hvduration); }
            set { setVal(NCCFlags.hvduration, value); }
        }

        public Int32 Delay
        {
            get { return (Int32)getVal(NCCFlags.delay); }
            set { setVal(NCCFlags.delay, value); }
        }

        // cmd line dual setters
        public void HVRange(string l, string h)
        {
            try
            {
                Int32 il, ih;
                int.TryParse(l, out il);
                Int32.TryParse(h, out ih);
                MinHV = il;
                MaxHV = ih;
            }
            catch (Exception)
            {
            }
        }
        public void HVTime(string b, string s)
        {
            try
            {
                Int32 idu, idl;
                int.TryParse(b, out idu);
                Int32.TryParse(s, out idl);
                Delay = idl;
                HVDuration = idu;
            }
            catch (Exception)
            {
            }
        }
        public bool HVX
        {
            get { return (bool)getVal(NCCFlags.hvx); }
            set { setVal(NCCFlags.hvx, value); }
        }

        /// <summary>
        ///   save results upon early termination (true means stop + save, false means abort and do not preserve)
        /// </summary>
        public bool SaveOnTerminate
        {
            get { return (bool)getVal(NCCFlags.saveOnTerminate); }
            set { setVal(NCCFlags.saveOnTerminate, value); }
        }

        public Int32 AssayType
        {
            get { return (Int32)getVal(NCCFlags.assaytype); }
            set { setVal(NCCFlags.assaytype, value); }
        }

        // takes an int or the string rep of the enum 
        public void asAssayType(string v)
        {
            int res = 0;
            bool b = AssayTypeConv(v, out res);
            if (b)
                AssayType = res;
        }
        internal bool AssayTypeConv(string v, out int res)
        {
            res = 0;
            bool ok = Int32.TryParse(v, out res);
            if (!ok)
            {
                string lv = v.ToLower();
                res = Conv(v[0]);
            }
            return ok;
        }
        internal int Conv(Char MeasurementOption)
        {
            int res = 0;
            switch (MeasurementOption)
            {
                case 'r': //MeasurementOption.rates:
                    res = 0;
                    break;
                case 'b': //MeasurementOption.background:
                    res = 1;
                    break;
                case 'i': //MeasurementOption.initial:
                    res = 2;
                    break;
                case 'n': //MeasurementOption.normalization:
                    res = 3;
                    break;
                case 'p': //MeasurementOption.precision:
                    res = 4;
                    break;
                case 'v': //MeasurementOption.verification:
                    res = 5;
                    break;
                case 'c': //MeasurementOption.calibration:
                    res = 6;
                    break;
                case 'h': //MeasurementOption.holdup:
                    res = 7;
                    break;
                case 'u': // MeasurementOption.unspecified:
                    res = 99;
                    break;
                default:
                    res = 99;
                    break;
            }
            return res;
        }
        internal string PoliteName(int MeasurementOption)
        {
            string res = "";
            switch (MeasurementOption)
            {
                case 0: //MeasurementOption.rates:
                    res = "Rates";
                    break;
                case 1: //MeasurementOption.background:
                    res = "Background";
                    break;
                case 2: //MeasurementOption.initial:
                    res = "InitialSource";
                    break;
                case 3: //MeasurementOption.normalization:
                    res = "Normalization";
                    break;
                case 4: //MeasurementOption.precision:
                    res = "Precision";
                    break;
                case 5: //MeasurementOption.verification:
                    res = "Verification";
                    break;
                case 6: //MeasurementOption.calibration:
                    res = "Calibration";
                    break;
                case 7: //MeasurementOption.holdup:
                    res = "Holdup";
                    break;
                case 8: // MeasurementOption.unspecified: // usually a list mode only operation
                default:
                    res = "Unspecified";
                    break;
            }
            return res;
        }

        public static string[] ToLines(Config cfg, LMAcquireConfig c)
        {
            string[] x = new string[100];
            int ix = 0;
			if (c.LM >= 0)
				x[ix++] = "  LMMM LM #: " + c.LM;
            x[ix++] = "  annotation message: " + "'" + c.Message + "'";
            //x[ix++] = "  raw NCD file location: " + (cfg == null ? c.Raw : cfg.Raw); fix lameness
            //x[ix++] = "  results file location: " + (cfg == null ? c.Results : cfg.Results);
            if (c.IncludeConfig)
                x[ix++] = "  append app config content to results file";
            x[ix++] = "";
            x[ix++] = "  assay cycles: " + c.Cycles;
            x[ix++] = "  assay cycle interval: " + c.Interval + " seconds";
            x[ix++] = "  assay cycle separation: " + c.Separation + " ms";
            x[ix++] = "  assay feedback flag: " + c.Feedback;
            x[ix++] = "  assay type: " + c.PoliteName(c.AssayType);
            if (c.SaveOnTerminate)
                x[ix++] = "  SaveOnTerminate: on cancellation or DAQ error, preserve results";
            else
                x[ix++] = "  SaveOnTerminate: on cancellation or DAQ error, abandon results";
            x[ix++] = "";
            x[ix++] = "  HV calib minHV, maxHV, step: " + c.MinHV + ", " + c.MaxHV + ", " + c.Step + " volts";
            x[ix++] = "  HV calib step and delay duration: " + c.HVDuration + ", " + c.Delay + " seconds";
            if (c.HVX)
                x[ix++] = "  HV calib push to Excel enabled";
            Array.Resize(ref x, ix);
            return x;
        }

        public string[] ToLines()
        {
            LMAcquireConfig c = this;
            return LMAcquireConfig.ToLines(null, c);
        }


        override public string ToString()
        {
            string[] x = ToLines();
            string s = "";
            for (int i = 0; i < x.Length; i++)
            {
                s += (x[i] + Eol);
            }
            return s;
        }
    }
  
    public class DBConfig : ConfigHelper
    {
        public DBConfig(Hashtable _parms)
        {
            this._parms = _parms;
            resetVal(NCCFlags.MyProviderName, "System.Data.SQLite", typeof(string));
            resetVal(NCCFlags.MyDBConnectionString, "Data Source=.\\INCC6.SQLite;Version=3;New=False;Compress=True;foreign_keys=on;", typeof(string));
        }

        public string MyProviderName
        {
            get { return (string)getVal(NCCFlags.MyProviderName); }
            set { setVal(NCCFlags.MyProviderName, value); }
        }
        public string MyDBConnectionString
        {
            get { return (string)getVal(NCCFlags.MyDBConnectionString); }
            set { setVal(NCCFlags.MyDBConnectionString, value); }
        }

        public string[] ToLines()
        {
            string[] x = new string[10];
            int ix = 0;
            x[ix++] = "  MyProviderName: " + MyProviderName;
            x[ix++] = "  MyDBConnectionString: " + MyDBConnectionString;
            Array.Resize(ref x, ix);
            return x;
        }
        override public string ToString()
        {
            string[] x = ToLines();
            string s = "";
            for (int i = 0; i < x.Length; i++)
            {
                s += (x[i] + Eol);
            }
            return s;
        }


    }

	public class WPFEventArgs: EventArgs
	{
		public bool value;

		//
		// Summary:
		//     Initializes a new instance of the System.EventArgs class.
		public WPFEventArgs()
		{
			value = false;
		}
	}

    // loaded and configured in the exe.config file
    public class WPFListener : TraceListener
    {
        public delegate void OnTextHandler(string msg, bool newLine);
        public event OnTextHandler OnText;

        public WPFListener()
        {
            current = String.Empty;
            header = true;
            count = 0;
        }
        ~WPFListener()
        {
			Dispose(false);
        }

        string current = string.Empty;
        int previous;
        ulong count = 0;
        bool header = true;

        protected bool CombineHeaderAndBody(string message)
        {
            bool writeNow = false;
            if (header)
            {
                current = message;
            }
            else
            {
                current = current + message;
                writeNow = true;
            }
            header = !header; //toggle
            return writeNow; // if true then write on next step
        }

        //
        // The header is logged first then a second call has the message body
        // Helpfully both appear in one single entry, so we cache the header and append the body and then write it.
        //
        public override void Write(string message)
        {
            bool writeNow = CombineHeaderAndBody(message);
            if (!writeNow)
                return;
            InternalWrite(message);
        }

        private void InternalWrite(string message)
        {
            if (current.GetHashCode() != previous)
            {
                previous = current.GetHashCode();
                count = 0;
            }
            else
                count++;
            if (count > 10)
            {
                if (count == 10)
                    current = "Message repeated . . .";
                else if ((count % 1000) == 0)
                    current = "Message repeated 1000 times . . .";
                else
                    return;
            }
            if (OnText != null)
                OnText.Invoke(current, true);

        }
        public override void WriteLine(string message)
        {
            if (ExternalPrefix(message) && header) // expecting a header, but not getting one, so make one
            {
                Write("*Ext ");
            }
            Write(message);
        }
        static string[] prefixes = { "===", "DB", "Control", "Data", "Analysis", "App"};
        bool ExternalPrefix(string message)
        {
            bool res = true;
            foreach (string s in prefixes)
            {
                if (message.StartsWith(s))
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

    }


}
