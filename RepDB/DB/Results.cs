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
using System.Collections;
using System.Data;
namespace DB
{

    public class Results
    {
        public Results()
        {
            db = new DB(false);
        }

        DB db;
        public Results(DB existing)
        {
            db = existing;
        }

        public DataTable AllResults(string name = null)
        {
            if (!string.IsNullOrEmpty(name))
                return ResultsForDet(name);
            else
            {
                db.SetConnection();
                string sSQL = "SELECT * FROM results_rec";
                return db.DT(sSQL);
            }
        }
        public DataTable ResultsForDet(string name)
        {
            db.SetConnection();
            string sSQL = "SELECT results_rec.* FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(name) + ")";
            return db.DT(sSQL);
        }

        public DataTable ResultsForDetector(string name, DateTime dt)
        {
            db.SetConnection();
            long id = Lookup(name, dt);
            return Result(id);
        }
        public DataTable Result(long id)
        {
            db.SetConnection();
            string sSQL = "SELECT * from results_rec where id=" + id;
            return db.DT(sSQL);
        }

		public DataTable ResultForMeasId(long mid)
        {
            db.SetConnection();
            string sSQL = "SELECT * from results_rec where mid=" + mid;
            return db.DT(sSQL);
        }
        public long Create(long measid, ElementList sParams) 
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "Insert into results_rec ";
            sParams.Add(new Element("mid", measid));
            string sSQL = sSQL1 + sParams.ColumnsValues; sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("results_rec"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Delete(long rid)
        {
            db.SetConnection();
            string s = "DELETE FROM results_rec where id=" + rid.ToString();
            return db.Execute(s);
        }

        public bool Delete(string detname, DateTime dt)
        {
            db.SetConnection();
            long id = Lookup(detname, dt);
            string s = "DELETE FROM results_rec where id=" + id.ToString();
            return db.Execute(s);
        }
        public long Lookup(string detname, DateTime dt)
        {
            if (string.IsNullOrEmpty(detname))
                return -1;
            db.SetConnection();
            Measurements m = new Measurements(db);
            string s = "select results_rec.id from results_rec INNER JOIN measurements ON "+
                "(measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(detname) +
                "AND measurements.DateTime=results_rec.original_meas_date) where results_rec.original_meas_date=" + SQLSpecific.getDate(dt);
            string r = db.Scalar(s);
            long lr = -1;
            if (!long.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

        public bool Update(long rid, ElementList sParams) 
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "UPDATE results_rec SET ";
            string wh = " where id=" + rid.ToString();
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }

		public bool UpdateEndingComment(long mid, string ec)
		{
            db.SetConnection();
            string wh = " where mid=" + mid.ToString();
            string sSQL1 = "UPDATE results_rec SET ending_comment=" +  SQLSpecific.QVal(ec) + wh;
            return db.Execute(sSQL1);
        }

		public DataTable ResultsForDetWithC(string name)
        {
            db.SetConnection();
<<<<<<< HEAD
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type,results_rec.item_id FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(name) + ")";
            return db.DT(sSQL);
        }
        public DataTable ResultsForDetWithItem(string name, string item)
        {
            db.SetConnection();
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type,results_rec.item_id FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(name) + " AND results_rec.item_id=" + SQLSpecific.QVal(item) + ")";
=======
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(name) + ")";
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
            return db.DT(sSQL);
        }
        public DataTable ResultsForDetWithItem(string name, string item)
        {
            db.SetConnection();
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type,results_rec.item_id FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid AND measurements.detector_id=" + SQLSpecific.QVal(name) + " AND results_rec.item_id=" + SQLSpecific.QVal(item) + ")";
            return db.DT(sSQL);
        }
		public DataTable ResultsSubset()
        {
            db.SetConnection();
<<<<<<< HEAD
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type,results_rec.detector_name FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid)";

=======
<<<<<<< .mine
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,results_rec.item_type,results_rec.detector_name FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid)";
=======
            string sSQL = "SELECT measurements.DateTime, results_rec.id,results_rec.mid,results_rec.campaign_id,results_rec.meas_option,,results_rec.item_type,results_rec.detector_name FROM results_rec INNER JOIN measurements ON (measurements.id=results_rec.mid)";
>>>>>>> .theirs
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
            return db.DT(sSQL);
        }
    }

    // for combined results and copy of method params (_m)
    public class ParamsRelatedBackToMeasurement
    {
        public string table;

        public ParamsRelatedBackToMeasurement(string table = "", DB db = null)
        {
            this.table = table;
            if (db != null)
                this.db = db;
            else
                this.db = new DB(false);
        }

		public ParamsRelatedBackToMeasurement(DB db)
        {
            if (db != null)
                this.db = db;
            else
                this.db = new DB(false);
        }
        DB db;

        string MethodTableName(string opttable = null)
		{ 
            if (string.IsNullOrEmpty(table)) 
                return "__";  // guaranteed to throw exceptions
            else
			{
				if (string.IsNullOrEmpty(opttable))
				{
					return table.Remove(0, 8) + "_m";  // 8 is length of 'results_'
				}
				else
				{
					return opttable + "_m"; // crude hack to accomodate old 3 method tables for collar scheme that I kept for some lame reason
				}
			}
       } 

        public long CreateMethod(long resid, long mid, ElementList resParams)
        {
            db.SetConnection();
            resParams.Add(new Element("rid", resid));
            resParams.Add(new Element("mid", mid));
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "Insert into " + MethodTableName(resParams.OptTable) + " ";
            string sSQL = sSQL1 + resParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID(MethodTableName(resParams.OptTable)));
            return db.ExecuteTransactionID(sqlList);
        }

        public long Create(long mid, ElementList resParams)
        {
            db.SetConnection();
            resParams.Add(new Element("mid", mid));
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "Insert into " + this.table + " ";
            string sSQL = sSQL1 + resParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID(table));
            return db.ExecuteTransactionID(sqlList);
        }


		public DataTable GetCombinedResults(long mid)
        {
			db.SetConnection();
			string sSQL = "SELECT * FROM " + table + "," + MethodTableName() + " where "+ table + ".mid=" + mid.ToString() + " AND " + MethodTableName() + ".rid=" + table + ".id";
            return db.DT(sSQL);
        }
		
        public DataTable GetMethodResults(long mid)
        {
			db.SetConnection();
            string sSQL = "SELECT * from " + table + " where mid=" + mid.ToString();
            return db.DT(sSQL);
        }

        public DataTable GetMethodResultsMethod(long mid, long rid)
        {
			db.SetConnection();
            string sSQL = "SELECT * from " + table + "_m where mid=" + mid.ToString() + " AND rid=" + rid.ToString();
            return db.DT(sSQL);
        }

		public DataTable GetMethodResultsMethod(long mid, long rid, string opttable)
        {
			db.SetConnection();
			string newtable = opttable + "_m";
			string sSQL = "SELECT * FROM "+ newtable + " where mid=" + mid.ToString() + " AND " + newtable + ".rid=" + rid.ToString();
            return db.DT(sSQL);
        }
    }

	public class LMParamsRelatedBackToMeasurement
    {
        public string table;

        public LMParamsRelatedBackToMeasurement(string table = "", DB db = null)
        {
            this.table = table;
            if (db != null)
                this.db = db;
            else
                this.db = new DB(false);
        }

		public LMParamsRelatedBackToMeasurement(DB db)
        {
            if (db != null)
                this.db = db;
            else
                this.db = new DB(false);
        }
        DB db;

        public long Create(long mid, ElementList resParams)
        {
            db.SetConnection();
            resParams.Add(new Element("mid", mid));
            ArrayList sqlList = new ArrayList();
            string sSQL1 = "Insert into " + table + "_m" + " ";
            string sSQL = sSQL1 + resParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID(table + "_m" ));
            return db.ExecuteTransactionID(sqlList);
        }

        public DataTable GetCounterParams(long mid)
        {
			db.SetConnection();
            string sSQL = "SELECT * from " + table + "_m" + " where mid=" + mid.ToString();
            return db.DT(sSQL);
        }

    }


}
