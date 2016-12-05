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
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace DB
{
    /// <summary>
    /// Summary description for Measurements.
    /// CREATE TABLE measurements(
    //    [ID] INTEGER Primary Key,
    // 	  [detector_id] nvarchar(256) NOT NULL,
    //    [DateTime] nvarchar(32) NOT NULL,
    //    [Notes] nvarchar(1024) NULL,
    //    [Type] nvarchar(50) NOT NULL,
    //    [FileName]  nvarchar(1024) NOT NULL
    //
    /// </summary>
    public class Measurements
    {
        public Measurements()
        {
            db = new DB(false);
        }
        public Measurements(DB d)
        {
            db = d;
        }
        public DB db;
        public DataTable AllMeasurements(string name = null)
        {
            if (!string.IsNullOrEmpty(name))
                return MeasurementsForDet(name);
            else
            {
                db.SetConnection();
                string sSQL = "SELECT * FROM measurements";
                return db.DT(sSQL);
            }
        }
        public DataTable MeasurementsForDet(string name)
        {
            db.SetConnection();
            //Changed SQL to display measurements w/newest first hn 9.10.2015
            string sSQL = "SELECT * FROM measurements where detector_id=" + SQLSpecific.QVal(name) + " ORDER BY DateTime DESC";
            return db.DT(sSQL);
        }
        public DataTable MeasurementsForInspection(string number)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM measurements where detector_id=" + SQLSpecific.QVal(number) + " ORDER BY DateTime DESC";
            return db.DT(sSQL);
        }
        public DataTable Measurement(long M_id)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM measurements Where id=" + M_id;
            return db.DT(sSQL);
        }

        //public long PrimaryKey(string name, DateTimeOffset dt, string type = null)
        //{
        //    db.SetConnection();
        //    long id = Lookup(name, dt, type);
        //    return id;
        //}

        public long Create(ElementList sParams) 
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "Insert into measurements " + sParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("measurements"));
            return db.ExecuteTransactionID(sqlList);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="sParams"></param>
        /// <returns></returns>
        public bool Update(string name, DateTimeOffset dt, ElementList sParams) 
        {
            db.SetConnection();
            string sSQL1 = "UPDATE measurements SET ";
            string wh = " where detector_id=" + SQLSpecific.QVal(name) + " AND DateTime=" + SQLSpecific.getDate(dt);
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }


        public bool Delete(long ID)
        {
            db.SetConnection();
            string s = "DELETE FROM measurements where id=" + ID.ToString();
            return db.Execute(s);
        }

        /// <summary>
        /// Look up the measurement identified by name, measurement type and timestamp
        /// </summary>
        /// <param name="name">detector</param>
        /// <param name="dt">timestamp</param>
        /// <param name="type">measurement type</param>
        /// <param name="counter">nth instance of this measurement, unique id it when other 3 values are identical</param>
        /// <returns>if found, the newest matching measurement primary key, o.w. -1</returns>
        public long Lookup(string name, DateTimeOffset dt, string type, int counter)
        {
            if (string.IsNullOrEmpty(name))
                return -1;
            db.SetConnection();
            string s = "SELECT * FROM measurements WHERE detector_id=" + SQLSpecific.QVal(name) + " AND DateTime=" + SQLSpecific.getDate(dt);
            if (!string.IsNullOrEmpty(type))
                s += " AND Type=" + SQLSpecific.QVal(type);

            string r = db.Scalar(s);
            long lr = -1;
            if (!long.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

		public long CountOf(string name, DateTimeOffset dt, string type)
        {
            if (string.IsNullOrEmpty(name))
                return 0;
            db.SetConnection();
            string s = "SELECT COUNT(*) FROM measurements WHERE detector_id=" + SQLSpecific.QVal(name) + " AND DateTime=" + SQLSpecific.getDate(dt);
            if (!string.IsNullOrEmpty(type))
                s += " AND Type=" + SQLSpecific.QVal(type);

            string r = db.Scalar(s);
            long lr = -1;
            if (!long.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

		public long CountOf(string name, string type = "")
        {
            if (string.IsNullOrEmpty(name))
                return 0;
            db.SetConnection();
            string s = "SELECT COUNT(*) FROM measurements WHERE detector_id=" + SQLSpecific.QVal(name);
            if (!string.IsNullOrEmpty(type))
                s += " AND Type=" + SQLSpecific.QVal(type);
            string r = db.Scalar(s);
            long lr = -1;
            if (!long.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

        public long Add(string name, DateTimeOffset date, string mtype, string filename, string notes) 
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "Insert into measurements (detector_id, DateTime, Notes, Type, FileName) VALUES (" + SQLSpecific.QVal(name)
            + "," + SQLSpecific.getDate(date) + "," + SQLSpecific.QVal(notes) + "," + SQLSpecific.QVal(mtype)  +
            "," + SQLSpecific.QVal(filename) + ")";
            sqlList.Add(sSQL1);
            sqlList.Add(SQLSpecific.getLastID("measurements"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool UpdateFileName(string fileName, long Meas_ID)
        {
            db.SetConnection();
            string wh = " where id = " + Meas_ID.ToString();
            string sSQL1 = "UPDATE measurements SET FileName = " +  SQLSpecific.QVal(fileName ) + wh;
            return db.Execute(sSQL1);
        }
		public bool UpdateNote(string notes, long Meas_ID)
        {
            db.SetConnection();
            string wh = " where id = " + Meas_ID.ToString();
            string sSQL1 = "UPDATE measurements SET Notes = " +  SQLSpecific.QVal(notes) + wh;
            return db.Execute(sSQL1);
        }

        /// Messages /////////////////////////////////////////
        public bool AddMessages(long Measurement_ID, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQLa = "Insert into analysis_messages ";
            foreach (ElementList sParam in sParams)
            {
                sParam.Add(new Element("mid",Measurement_ID));
                sqlList.Add(sSQLa + sParam.ColumnsValues);
            }
            return db.ExecuteTransaction(sqlList);
        }

		/// Additional results file names /////////////////////////////////////////
        public bool AddResultsFiles(long Measurement_ID, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQLa = "Insert into results_filenames ";
            foreach (ElementList sParam in sParams)
            {
                sParam.Add(new Element("mid",Measurement_ID));
                sqlList.Add(sSQLa + sParam.ColumnsValues);
            }
            return db.ExecuteTransaction(sqlList);
        }
        public DataTable GetResultFiles(long mid)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM results_filenames Where mid=" + mid;
            return db.DT(sSQL);
        }
        /// Cycles /////////////////////////////////////////
        ///
        /// yes
       public long AddCycleRetId(long Measurement_ID, ElementList sParams)
        {
            //Returns cycle ID
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = CycleInsertStatement(Measurement_ID, sParams) ;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("cycles"));
            return db.Execute(sqlList);
        }

        public bool AddCycle(long Measurement_ID, ElementList sParams)
        {
            db.SetConnection();
            string sSQL = CycleInsertStatement(Measurement_ID, sParams);
            return db.Execute(sSQL);
        }

        protected string CycleInsertStatement(long Measurement_ID, ElementList sParams)
        {
            sParams.Add(new Element("mid", Measurement_ID));
            return "Insert into cycles " + sParams.ColumnsValues;
        }

        public bool AddCycles(long Measurement_ID, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "";
            foreach (ElementList els in sParams)
            {
                sSQL = CycleInsertStatement(Measurement_ID, els);
                sqlList.Add(sSQL);
            }
            return db.ExecuteTransaction(sqlList);
        }

        public bool AddOneCycle(string table, ElementList sParams)
        {
            db.SetConnection();
            string sSQL ="Insert into " + table + " " + sParams.ColumnsValues;
            return db.Execute(sSQL);
        }

        public DataTable GetCycles(long mid)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM cycles Where mid=" + mid;
            return db.DT(sSQL);
        }

		public DataTable GetAllLMCycles(string table, long mid)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM table Where mid=" + mid;
            return db.DT(sSQL);
        }
		public DataTable GetLMCycles(string table, long cid, long mid)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM table Where cid=" + cid + " AND mid=" + mid;
            return db.DT(sSQL);
        }
        public int GetCycleCount(long mid)
        {
            string sSQL = "SELECT COUNT(id) FROM cycles Where mid=" + mid;
            db.CreateCommand(sSQL);
            db.SetConnection();
            return db.ScalarIntx();
        }


		public bool DeleteCycles(long mid)
        {
            db.SetConnection();
            string sSQL = "DELETE FROM cycles Where mid=" + mid;
            return db.Execute(sSQL);
        }
    }


    public class HVPlateauResults
    {
        public HVPlateauResults()
        {
            db = new DB(false);
        }
        public HVPlateauResults(DB d)
        {
            db = d;
        }
        DB db;
        public DataTable AllHVPlateauResults(string name = null)
        {
            if (!String.IsNullOrEmpty(name))
                return AllHVPlateauResults(name);
            else
            {
                db.SetConnection();
                string sSQL = "SELECT * FROM HVResult";
                return db.DT(sSQL);
            }
        }
        public DataTable HVPlateauResultsForDet(string name, bool includeRuns)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM HVResult where detector_id=" + SQLSpecific.QVal(name);
            return db.DT(sSQL);
        }
        public DataTable HVPlateauResult(string name, DateTime dt)
        {
            db.SetConnection();
            long id = Lookup(name, dt);
            return HVPlateauResult(id);
        }
        public DataTable HVPlateauResult(long M_id)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM HVResult Where id=" + M_id;
            return db.DT(sSQL);
        }

        public long PrimaryKey(string name, DateTime dt)
        {
            db.SetConnection();
            long id = Lookup(name, dt);
            return id;
        }

        public long Create(ElementList sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "Insert into HVResult " + sParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("HVResult"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Update(string name, DateTime dt, ElementList sParams)
        {
            db.SetConnection();
            string sSQL1 = "UPDATE HVResult SET ";
            string wh = " where detector_id=" + SQLSpecific.QVal(name) + " AND HVPDateTime=" + SQLSpecific.getDate(dt);
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }

        public long Delete(long id)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string s1 = "DELETE FROM HVStatus where hvp_id=" + id.ToString();
            string s2 = "DELETE FROM HVResult where id=" + id.ToString();
            sqlList.Add(s1);
            sqlList.Add(s2);
            return db.ExecuteTransactionID(sqlList);
        }

        public long Delete(string name, DateTime dt)
        {
            db.SetConnection();
            long id = Lookup(name, dt);
            return Delete(id);
        }

        public long Lookup(string name, DateTime dt)
        {
            if (String.IsNullOrEmpty(name))
                return -1;
            db.SetConnection();
            string s = "SELECT * FROM HVResult WHERE detector_id=" + SQLSpecific.QVal(name) + " AND HVPDateTime=" + SQLSpecific.getDate(dt);
            string r = db.Scalar(s);
            long lr = -1;
            if (!Int64.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

 
        /// HVP RUns /////////////////////////////////////////
        public bool AddRuns(long HVPId, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQLa = "Insert into HVStatus ";
            foreach (ElementList sParam in sParams)
            {
                sParam.Add(new Element("hvp_id", HVPId));
                sqlList.Add(sSQLa + sParam.ColumnsValues);
            }
            return db.ExecuteTransaction(sqlList);
        }

        public bool AddRun(long HVPId, ElementList sParams)
        {
            db.SetConnection();
            string sSQL = HVPRunInsertStatement(HVPId, sParams);
            return db.Execute(sSQL);
        }

        protected string HVPRunInsertStatement(long HVPId, ElementList sParams)
        {
            sParams.Add(new Element("hvp_id", HVPId));
            return "Insert into HVStatus " + sParams.ColumnsValues;
        }

        public bool AddCycles(long HVPId, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "";
            foreach (ElementList els in sParams)
            {
                sSQL = HVPRunInsertStatement(HVPId, els);
                sqlList.Add(sSQL);
            }
            return db.ExecuteTransaction(sqlList);
        }
    }

}

