/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Data;

namespace DB
{
    /// <summary>
    /// Summary description for Detectors.
    /// </summary>
    public class Detectors
    {
        public IDB db; 
        public Detectors()
        {
            db = new DB(false);
        }
        public Detectors(IDB existing)
        {
            db = existing;
        }

        public bool StrictDefinition { get; set; }

        public bool DefinitionExists(ElementList els)
        {
            db.SetConnection();
            string sSQL = "Select * FROM detectors where ";
            sSQL = sSQL + els.ColumnEqValueAndList;
            DataTable dt = db.DT(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool DefinitionExists(string DetectorName)
        {
            db.SetConnection();
            string sSQL = "Select * FROM detector Where detector_name = " + SQLSpecific.QVal(DetectorName);
            DataTable dt = db.DT(sSQL);
            return dt.Rows.Count > 0;
        }
        public bool DefinitionExists(int DetectorId)
        {
            db.SetConnection();
            string sSQL = "Select * FROM detector Where detector_id = " + DetectorId;
            DataTable dt = db.DT(sSQL);
            return dt.Rows.Count > 0;
        }
        public DataTable getDetectors(bool withParams)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM detectors";
            if (withParams)
            {
                sSQL += " join sr_parms_rec on(detectors.detector_id=sr_parms_rec.detector_id)";
                sSQL += " join alpha_beta_rec on(detectors.detector_id=alpha_beta_rec.detector_id)";
            }
            return db.DT(sSQL);
        }

        public DataRow UniqueRow(string DetectorName, Int32 DetectorType, string DetectorElectronics)
        {
            DataTable dt = BasicSelect(DetectorName, DetectorType, DetectorElectronics);
            if (dt.Rows.Count == 1) return dt.Rows[0];
            else return null;
        }

        public DataRow HasRow(string DetectorName, Int32 DetectorType, string DetectorElectronics)
        {
            DataTable dt = BasicSelect(DetectorName, DetectorType, DetectorElectronics);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public Int32 GetType(string DetectorName)
        {
            db.SetConnection();
            string sSQL = "SELECT detector_type_id "
                  + " FROM detectors"
                  + " Where detector_name = " + SQLSpecific.QVal(DetectorName);
            DataTable dt = db.DT(sSQL);
            if (dt.Rows.Count > 0) return
                (Int32)dt.Rows[0]["detector_type_id"];
            else
                return 10;
        }

        public bool Unique(string DetectorName, Int32 DetectorType, string DetectorElectronics)
        {
            return (UniqueRow(DetectorName, DetectorType, DetectorElectronics) != null);
        }

        public bool Has(string DetectorName, Int32 DetectorType, string DetectorElectronics)
        {
            return (HasRow(DetectorName, DetectorType, DetectorElectronics) != null);
        }


        private DataTable BasicSelect(string DetectorName, Int32 DetectorType, string DetectorElectronics)
        {
            db.SetConnection();
            string sSQL = "SELECT detector_name "
                  + " FROM detectors"
                  + " Where detector_name = " + SQLSpecific.QVal(DetectorName);
            if (StrictDefinition)
                sSQL += " AND detector_type_id = " + DetectorType.ToString() + " AND electronics_id = " + SQLSpecific.QVal(DetectorElectronics);
            DataTable dt = db.DT(sSQL);
            return dt;
        }


        public bool Delete(string DetectorName, string DetectorElectronics, Int32 DetectorType)
        {
            ArrayList sqlList = new ArrayList();
            db.SetConnection();
            string s = SQLSpecific.QVal(DetectorName) + " AND detector_type_id = " + DetectorType.ToString();
            string sSQL = "DELETE FROM detectors where detector_name = " + SQLSpecific.QVal(DetectorName) + " AND detector_type_id = " + DetectorType.ToString();
            sqlList.Add(sSQL);
            long l = PrimaryKey(DetectorName); 
            string srSQL = "DELETE FROM sr_parms_rec where detector_id = " + l.ToString()/* + " AND sr_type = " + DetectorType.ToString()*/;//This modified to also just match id.... hn 5.5.5015
            string abSQL = "DELETE FROM alpha_beta_rec where detector_id = " + l.ToString();
            sqlList.Add(srSQL);
            sqlList.Add(abSQL);
            return db.ExecuteTransaction(sqlList);
        }

        // i think this was meant to insert new types as well as new sets of SR params
        public long Insert(string DetectorName, string DetectorAlias, Int32 DetectorType, string DetectorElectronics, string DetectorDesc)
        {
            long DetectorID = -1;

            DataRow dr = HasRow(DetectorName, DetectorType, DetectorElectronics); // sets the connection
            if (dr == null) // a new entry!
            {
                ArrayList sqlList = new ArrayList();
                string sSQL = "insert into detectors (detector_name,detector_alias,detector_type_id,electronics_id,detector_type_freeform)";
                string sv = " values (" + SQLSpecific.QVal(DetectorName) + "," + SQLSpecific.QVal(DetectorAlias) + "," + DetectorType.ToString() +
                    "," + SQLSpecific.QVal(DetectorElectronics) + "," + SQLSpecific.QVal(DetectorDesc)+")";
                sqlList.Add(sSQL + sv);
                sqlList.Add(SQLSpecific.getLastID("detectors"));
                DetectorID = db.ExecuteTransactionID(sqlList);
            }
            else // already there, just return the id
            {
                DetectorID = PrimaryKey(DetectorName);
            }
            return DetectorID;
        }


        public long PrimaryKey(string Id)
        {
            if (String.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT detector_id FROM detectors where detector_name = " + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!Int64.TryParse(r, out l))
                l = -1;
            return l;
        }

        public bool Update(string DetectorName, ElementList els)
        {
            db.SetConnection();
            string wh = " where detector_name = " + SQLSpecific.QVal(DetectorName);
            string sSQL1 = "UPDATE detectors SET ";
            string sSQL1i = String.Empty;
            string sSQL = sSQL1 + els.ColumnEqValueList + wh; // NEXT: use exclusion for params in this case it's detector_name
            bool b = db.Execute(sSQL);
            return b;
        }


        //////////

        // TBD
        public DataTable getDetectorHV(string DetectorID)
        {
            db.SetConnection();
            string sSQL = "SELECT [detector_id],[min_voltage],[max_voltage],[voltage_step_size],[voltage_count_time],Delay"
                + " FROM detector_hvSettings  Where Detector_ID = " + DetectorID
                + " Union"
                + " SELECT -1 as detector_id,[min_voltage],[max_voltage],[voltage_step_size],[voltage_count_time],Delay"
                + " FROM hvSettings Order By Detector_ID DESC";
            return db.DT(sSQL);

        }
        // TBD
        public bool updateDetectorHV(string detector_id, string min_voltage, string max_voltage, string voltage_step_size,
            string voltage_count_time, string delay, string sTransType)
        {
            db.SetConnection();
            string sSQL = "";
            ArrayList sqlList = new ArrayList();
            sSQL = "Delete from detector_hvSettings where detector_ID = " + detector_id;
            sqlList.Add(sSQL);

            sSQL = "INSERT INTO detector_hvSettings"
               + " ([detector_id]"
               + " ,[min_voltage]"
               + " ,[max_voltage]"
               + " ,[voltage_step_size]"
               + " ,[voltage_count_time],delay)"
               + " VALUES"
               + " (" + detector_id
               + " ," + min_voltage
               + " ," + max_voltage
               + " ," + voltage_step_size
               + " ," + voltage_count_time + "," + delay + ")";

            sqlList.Add(sSQL);
            return db.ExecuteTransaction(sqlList);
        }

        // TBD
        public DataTable getElectronicIDs()
        {
            db.SetConnection();
            string sSQL = "SELECT electronics_id, electronics.electronics_desc"
            + " FROM "
            + " electronics ";

            return db.DT(sSQL);
        }
        // TBD
        public DataTable getDetectorChannels(string sDetectorID)
        {
            db.SetConnection();
            string sSQL = "SELECT '' as changed, detector_id,'' as MultiDetectorID, channel_id, channel_num, active"
                + " FROM detector_channels"
                + " WHERE detector_id = " + sDetectorID;
            return db.DT(sSQL);
        }

    }


    public class ShiftRegisterParams
    {
        IDB db;

        public bool Id;

        private string ResolvedKey(string val, IDB _db)
        {
            Detectors dets = new Detectors(_db);
            if (Id)
            {
                long l = dets.PrimaryKey(val);
                return Field + "=" + l.ToString();
            }
            else
            {
                return Field + "=" + SQLSpecific.Value(val, true);   
            }
        }

        private string Field
        {
            get { return (Id ? "detector_id" : "sr_detector_id"); }
        }


        public ShiftRegisterParams(bool newid = false)
        {
            db = new DB(false);
            Id = newid;
        }

        public ShiftRegisterParams(IDB _db, bool newid = false)
        {
            db = _db;
            Id = newid;
        }

        public bool Has(string measDetId) 
        {
            db.SetConnection();
            DataTable dt = Get(measDetId);
            return dt.Rows.Count > 0;
        }

        public DataTable Get(string measDetId) 
        {
            db.SetConnection();
            string sSQL = "Select * FROM sr_parms_rec where " + ResolvedKey(measDetId, db);// +" AND sr_type = " + type.ToString();
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public bool Create(string measDetId, Int32 type, ElementList sParams)
        {
            db.SetConnection();
            string sSQL1 = "Insert into sr_parms_rec ";
            string sSQL = sSQL1 + sParams.ColumnsValues;
            return db.Execute(sSQL);
        }

        public bool Update(string measDetId, Int32 type, ElementList sParams) 
        {
            db.SetConnection();
            //OK, this could get in strange state when sr_type was changed.  So, Martyn and I decided does not matter.  No dup detector id with different type.  Is there?
            //Just match db record by id. hn 5/4/2015.
            string wh = " where " + ResolvedKey(measDetId, db);// +
                                                           // " AND sr_type = " + type.ToString();
            string sSQL1 = "UPDATE sr_parms_rec SET ";
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }
    }


    public class ShiftRegisterParams2 : ParamsRelatedBackToDetector
    {
        public ShiftRegisterParams2(IDB db = null)
            : base("sr_parms_rec", db)
        {
        }
    }

    public class AlphaBeta : ParamsRelatedBackToDetector
    {
        public AlphaBeta(IDB db = null)
            : base("alpha_beta_rec", db)
        {
        }
    }

    /// used for LM default FA choice
    public class SRParamsExtension : ParamsRelatedBackToDetector
    {
        public SRParamsExtension(IDB db = null)
            : base("sr_parms_ext", db)
        {
        }
    }



//CREATE TABLE LMNetComm(
//    [detector_id] INTEGER NULL,
//    [broadcast] [int] NULL,
//    [port] [int] NULL,
//    [broadcastport] [int] NULL,
//    [subnet] [nvarchar(256)] NULL,
//    [wait] [int] NULL,
	
//    [numConnections] [int] NULL,
//    [receiveBufferSize] [int] NULL,
//    [parseBufferSize] [int] NULL,
//    [useAsyncAnalysis] [int] NULL,
//    [streamRawAnalysis] [int] NULL,
//    [useAsyncFileIO] [int] NULL
//);

    public class LMNetCommParams
    {
        IDB db;

        public LMNetCommParams()
        {
            db = new DB(false);
        }

        public LMNetCommParams(IDB _db)
        {
            db = _db;
        }

        public bool Has(string measDetId) 
        {
            db.SetConnection();
            DataTable dt = Get(measDetId);
            return dt.Rows.Count > 0;
        }

        public DataTable GetComms()
        {
            db.SetConnection(); 
            string sSQL = "Select * from LMNetComm";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        public DataTable GetHW()
        {
            db.SetConnection();
            string sSQL = "Select * from LMHWParams";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        public DataTable Get(string measDetId) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "Select * from LMNetComm, LMHWParams where (LMNetComm.detector_id=" + l.ToString() + " AND LMHWParams.detector_id=" + l.ToString() +")";
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public long Create(string measDetId, string table, ElementList sParams) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            if (table == "net")
               return CreateNetComm(l, sParams, db);
            else
              return  CreateCfg(l, sParams, db);
        }
        public long CreateCfg(long id, ElementList sParams, IDB db) 
        {
            return CreateTbl(id, "LMHWParams", sParams, db);
        }
        public long CreateNetComm(long id, ElementList sParams, IDB db) 
        {
            return CreateTbl(id, "LMNetComm", sParams, db);
        }
        public long CreateTbl(long id, string table, ElementList sParams, IDB db) 
        {
            sParams.Add(new Element("detector_id", id));
            string sSQL1 = "Insert into " + table + " ";
            string sSQL = sSQL1 + sParams.ColumnsValues;
            
            ArrayList sqlList = new ArrayList(); 
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID(table));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Update(string measDetId, string table, ElementList sParams) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            if (table == "net")
                return UpdateNetComm(l, sParams, db);
            else
                return UpdateCfg(l, sParams, db);
        }
        public bool UpdateCfg(long id, ElementList sParams, IDB db) 
        {
            return UpdateTbl(id, "LMHWParams", sParams, db);
        }
        public bool UpdateNetComm(long id, ElementList sParams, IDB db) 
        {
            return UpdateTbl(id, "LMNetComm", sParams, db);
        }
        public bool UpdateTbl(long id, string table, ElementList sParams, IDB db) 
        {
            db.SetConnection();
            string sSQL1 = "UPDATE " + table + " SET ";
            string sSQL1i = String.Empty;
            string wh = " where detector_id = " + id.ToString();
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }
        public int checkUnique(string sValue)
        {
            return -0;
        }
    }

    public class LMMultiplicityParams : ParamsRelatedBackToDetector
    {
        public LMMultiplicityParams(IDB db = null)
            : base("LMMultiplicity", db)
        {
        }
    }

}
