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
using System.Data;
using System.Reflection;
using AnalysisDefs;
using DetectorDefs;
namespace ListModeDB
{

    using NC = NCC.CentralizedState;

    public class LMDB
    {
        public LMDB()
        {
            // set up the internal DB transfer state
        }        

        public static LMINCCAppContext AppContext
        {
            get
            {
                LMINCCAppContext app = new LMINCCAppContext();
                DB.AppContext db = new DB.AppContext();
                DataTable dt = db.GetAll();
                if (dt.Rows.Count < 1)
                {
                    db.Update(app.ToDBElementList());  // creates if not found
                    dt = db.GetAll();  // try again
                    if (dt.Rows.Count < 1)
                        return app;
                }

                DataRow dr = dt.Rows[0];
                app.RootPath = (string)(dr["root"]);
                app.DailyRootPath = DB.Utils.DBBool(dr["dailyRootPath"]);
                app.Logging = DB.Utils.DBBool(dr["logging"]);
               // app.DailyLogPath = DB.Utils.DBBool(dr["logAutoPath"]);
                app.LoggingDetails = DB.Utils.DBInt32(dr["logDetails"]);
                app.SetLevel(DB.Utils.DBUInt16(dr["level"]));
                app.RolloverIntervalMin = DB.Utils.DBInt32(dr["rolloverIntervalMin"]);
                app.RolloverSizeMB = DB.Utils.DBInt32(dr["rolloverSizeMB"]);
                app.LogResults = DB.Utils.DBUInt16(dr["logResults"]);  // 0 none, 1 log file only, 2 console/UI only, 3 everywhere
                app.FPPrecision = DB.Utils.DBUInt16(dr["fpPrec"]);
                app.OpenResults = DB.Utils.DBBool(dr["openResults"]);
                app.SetVerbose(DB.Utils.DBUInt16(dr["verbose"]));
                app.INCC5IniLoc = (string)(dr["emulatorapp"]);
                app.UseINCC5Ini = DB.Utils.DBBool(dr["serveremulation"]);
                app.FileInputDBSetter = (string)(dr["fileinput"]);
                app.Recurse = DB.Utils.DBBool(dr["recurse"]);
                app.ParseGen2 = DB.Utils.DBBool(dr["parseGen2"]);
                app.Replay = DB.Utils.DBBool(dr["replay"]);
                app.INCCParity = DB.Utils.DBBool(dr["INCCParity"]);

                app.SortPulseFile = DB.Utils.DBBool(dr["sortPulseFile"]);

                app.PulseFileAssay = DB.Utils.DBBool(dr["pulseFileAssay"]);
                app.PTRFileAssay = DB.Utils.DBBool(dr["ptrFileAssay"]);
                app.MCA527FileAssay = DB.Utils.DBBool(dr["nilaFileAssay"]);
                app.TestDataFileAssay = DB.Utils.DBBool(dr["testDataFileAssay"]);
                app.ReviewFileAssay = DB.Utils.DBBool(dr["reviewFileAssay"]);

                app.StatusPacketCount = DB.Utils.DBUInt32(dr["opStatusPktInterval"]);
                app.StatusTimerMilliseconds = DB.Utils.DBUInt32(dr["opStatusTimeInterval"]);
                app.AutoCreateMissing = DB.Utils.DBBool(dr["autoCreateMissing"]);
                app.AuxRatioReport = DB.Utils.DBBool(dr["auxRatioReport"]);
                app.OverwriteImportedDefs = DB.Utils.DBBool(dr["overwriteDefs"]);
                app.CreateINCC5TestDataFile = DB.Utils.DBBool(dr["gen5RevDataFile"]);
                app.LiveFileWrite = DB.Utils.DBBool(dr["liveFileWrite"]);

                if (existtest(dr, "dbDataAssay"))
                    app.DBDataAssay = DB.Utils.DBBool(dr["dbDataAssay"]);
                if (existtest(dr, "datazFileAssay"))
                    app.DatazFileAssay = DB.Utils.DBBool(dr["datazFileAssay"]);
                if (strvaluetest(dr, "resultsFilePath"))
                    app.ResultsFilePath = (string)(dr["resultsFilePath"]);
                if (strvaluetest(dr, "logFilePath"))
                    app.LogFilePath = (string)(dr["logFilePath"]);
                if (existtest(dr, "results8Char"))
					app.Results8Char = DB.Utils.DBBool(dr["results8Char"]);
                if (existtest(dr, "assayTypeSuffix"))
					app.AssayTypeSuffix = DB.Utils.DBBool(dr["assayTypeSuffix"]);
               return app;
            }
		}
        
		static private bool strvaluetest(DataRow dr, string key)
		{
			return dr.Table.Columns.Contains(key) && (!dr[key].Equals(DBNull.Value)) && (!string.IsNullOrEmpty((string)dr[key]));
		}

		static private bool existtest(DataRow dr, string key)
		{
			return dr.Table.Columns.Contains(key);
		}

        private CountingAnalysisParameters CountingParameters(string detname)
        {
            DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.CountingAnalyzers, detname);
            CountingAnalysisParameters cp = new CountingAnalysisParameters();

            // 0 is mat name, 1 is det name, 2 is mat id, 3 is det id, 4 is first choice boolean
            foreach (DataRow dr in dt.Rows)
            {
				SpecificCountingAnalyzerParams sca = ConstructCountingAnalyzerParams(dr);
                cp.Add(sca);
            }
            return cp;
        }

		public SpecificCountingAnalyzerParams ConstructCountingAnalyzerParams(DataRow dr)
		{
                string type = "AnalysisDefs.";   // dev note: careful here, this is subject to bit rot
                if (dr["counter_type"].Equals(DBNull.Value))
                    type += "BaseRate";
                else
                    type += (string)dr["counter_type"];
                Type t = Type.GetType(type);
                ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                SpecificCountingAnalyzerParams sca = (SpecificCountingAnalyzerParams)ci.Invoke(null);
                sca.gateWidthTics = DB.Utils.DBUInt64(dr["gatewidth"]);
                if (t == typeof(Multiplicity))
                {
                    ((Multiplicity)sca).AccidentalsGateDelayInTics = DB.Utils.DBUInt64(dr["accidentalsgatewidth"]);
                    ((Multiplicity)sca).BackgroundGateTimeStepInTics = DB.Utils.DBUInt64(dr["backgroundgatewidth"]);
                    ((Multiplicity)sca).FA = (DB.Utils.DBBool(dr["FA"]) ? FAType.FAOn : FAType.FAOff);
                    ((Multiplicity)sca).SR.gateLength = sca.gateWidthTics;
                    // predelay is on the SR member
                }
                else if (t == typeof(Coincidence))
                {
                    ((Coincidence)sca).AccidentalsGateDelayInTics = DB.Utils.DBUInt64(dr["accidentalsgatewidth"]);
                    ((Coincidence)sca).BackgroundGateTimeStepInTics = DB.Utils.DBUInt64(dr["backgroundgatewidth"]);
                    // ((Coincidence)sca).FA = FAType.FAOff;  // always off by definition
                    ((Coincidence)sca).SR.gateLength = sca.gateWidthTics;
                    // predelay is on the SR member
            }
            sca.Active = DB.Utils.DBBool(dr["active"]);
                if (dr.Table.Columns.Contains("rank"))
                    sca.Rank = DB.Utils.DBUInt16(dr["rank"]);

				return sca;
		}
        public bool UpdateLMINCCAppContext()
        {
            DB.AppContext db = new DB.AppContext();
            bool res = db.Update(NC.App.AppContext.ToDBElementList());
            if (res) NC.App.AppContext.modified = false;
            return res;
        }

        public LMConnectionInfo LastBestConnInfo()
        {
            DB.LMNetCommParams gray = new DB.LMNetCommParams();
            DataTable dt = gray.GetComms();
            LMConnectionInfo lm = new LMConnectionInfo();
            if (dt.Rows.Count < 1)
                return lm;
            DataRow drl = dt.Rows[dt.Rows.Count - 1];

            lm.NetComm.Broadcast = DB.Utils.DBBool(drl["broadcast"]);
            lm.NetComm.LMListeningPort = DB.Utils.DBInt32(drl["broadcastport"]);
            lm.NetComm.Port = DB.Utils.DBInt32(drl["port"]);
            lm.NetComm.Subnet = (string)(drl["subnet"]);
            lm.NetComm.Wait = DB.Utils.DBInt32(drl["wait"]);

            lm.NetComm.NumConnections = DB.Utils.DBInt32(drl["numConnections"]);
            lm.NetComm.ReceiveBufferSize = DB.Utils.DBInt32(drl["receiveBufferSize"]);
            lm.NetComm.ParseBufferSize = DB.Utils.DBUInt32(drl["parseBufferSize"]);
            lm.NetComm.UseAsynchAnalysis = DB.Utils.DBBool(drl["useAsyncAnalysis"]);
            lm.NetComm.UseAsynchFileIO = DB.Utils.DBBool(drl["useAsyncFileIO"]);
            lm.NetComm.UsingStreamRawAnalysis = DB.Utils.DBBool(drl["streamRawAnalysis"]);

            dt = gray.GetHW(); 
            if (dt.Rows.Count < 1)
                return lm;
            drl = dt.Rows[dt.Rows.Count - 1];
            lm.DeviceConfig.LEDs = DB.Utils.DBInt32(drl["leds"]);
            lm.DeviceConfig.HV = DB.Utils.DBInt32(drl["hv"]);
            lm.DeviceConfig.LLD = DB.Utils.DBInt32(drl["LLD"]); // alias for VoltageTolerance on PTR32 and MCA527
            lm.DeviceConfig.Debug = DB.Utils.DBInt32(drl["debug"]);
            lm.DeviceConfig.Input = DB.Utils.DBInt32(drl["input"]);
            try {
                lm.DeviceConfig.HVTimeout = DB.Utils.DBInt32(drl["hvtimeout"]);
            } catch (Exception) { }
           return lm;
        }


        public CountingAnalysisParameters CountingParameters(Detector det, bool applySRFromDetector)
        {
            CountingAnalysisParameters cap = CountingParameters(det.Id.DetectorName);
            if (applySRFromDetector)
            {
                Multiplicity m = det.MultiplicityParams;
                foreach (SpecificCountingAnalyzerParams s in cap)
                {
                    Type t = s.GetType();
                    if (t.Equals(typeof(Multiplicity)))
                    {
                        Multiplicity thisone = ((Multiplicity)s);
                        ulong gw = thisone.gateWidthTics;
                        //ulong predelay = thisone.SR.predelay;
                        thisone.SR = new ShiftRegisterParameters(m.SR);   // use the current detector's SR params, then 
                        thisone.SetGateWidthTics(gw); // override with the user's choice from the DB
						//thisone.SR.predelay = predelay;
                    }
                    else if (t.Equals(typeof(Coincidence)))
                    {
                        Coincidence thisone = ((Coincidence)s);
                        ulong gw = thisone.gateWidthTics;
                        //ulong predelay = thisone.SR.predelay;
                        thisone.SR = new ShiftRegisterParameters(m.SR);   // use the current detector's SR params, then 
                        thisone.SetGateWidthTics(gw); // override with the user's choice from the DB
						//thisone.SR.predelay = predelay;
                    }
                }
            }
            return cap;
        }



        /// the details
        public void UpdateCounters(string detname, CountingAnalysisParameters cap)
        {
            DB.CountingAnalysisParameters db = new DB.CountingAnalysisParameters();

            foreach(SpecificCountingAnalyzerParams s in cap)
            {
                DB.ElementList parms = s.ToDBElementList();
                if (parms != null)
                {
                    db.Update(detname, s.GetType().Name, parms);
                }
            }
        }

        public void UpdateCounters(Detector det,  CountingAnalysisParameters cap)
        {
            UpdateCounters(det.Id.DetectorName, cap);
        }

        public void ReplaceCounters(Detector det, CountingAnalysisParameters cap)
        {
            DB.CountingAnalysisParameters db = new DB.CountingAnalysisParameters();
            db.DeleteAll(det.Id.DetectorName);
            InsertCounters(det.Id.DetectorName, cap); 
        }

        public void InsertCounters(string detname, CountingAnalysisParameters cap)
        {
            DB.CountingAnalysisParameters db = new DB.CountingAnalysisParameters(); 
            foreach (SpecificCountingAnalyzerParams s in cap)
            {
                DB.ElementList parms = s.ToDBElementList();
                if (parms != null)
                {
                    db.Insert(detname, s.GetType().Name, parms);
                }
            }
        }
    }
}
