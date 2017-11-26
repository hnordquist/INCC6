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
using System.Data;
using NCCReporter;
namespace DB
{

   public class AnalysisMethodSpecifiers: IDisposable
    {
       public AnalysisMethodSpecifiers()
        {
            db = new DB2(true);
        }
        DB2 db;

        public void Dispose()
        {
			Dispose(true); 
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                try
                {
                    if (db != null)
                    {
                        db.Dispose();
                        db = null;
                    }
                }
                catch (Exception caught)
                {
                    if (!DBMain.ConsoleQuiet)
                        Console.WriteLine(caught.Message);
                }
        }
        public DataTable Methods()
        {
            return AllWithKeyNames();
        }
        public DataTable MethodsForDetector(string DetName)
        {
            if (String.IsNullOrEmpty(DetName))
                return AllWithKeyNames();
            return BasicSelect(DetName, "");
        }

        public bool Update(string DetectorName, string ItemType, ElementList sParams)
        {
                bool res = false;
                db.SetConnection();
                //NEXT: this duo-lookup part takes too long, so get the values once in a wrapper call, then cache them, then reuse them
                DataRow dr = HasRow(DetectorName, ItemType); // sets the connection
                Detectors dets = new Detectors(db);
                long l = dets.PrimaryKey(DetectorName);
                Descriptors mats = new Descriptors("material_types", db);
                long m = mats.PrimaryKey(ItemType);

                if (l == -1 || m == -1)
                {
                    DBMain.AltLog(LogLevels.Warning, 70130, "Missing Det/Mat keys ({0},{1}) selecting AnalysisMethods", l, m);
                    return false;
                }


                if (dr == null) // a new entry!
                {
                    string sSQL = "insert into analysis_method_rec ";
                    sParams.Add(new Element("item_type_id", m));
                    sParams.Add(new Element("analysis_method_detector_id", l));
                    sSQL = sSQL + sParams.ColumnsValues;
                    res = db.Execute(sSQL);
                }
                else
                {
                    string wh = " where item_type_id= " + m.ToString() + " AND analysis_method_detector_id=" + l.ToString();
                    string sSQL1 = "UPDATE analysis_method_rec SET ";
                    string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
                    res = db.Execute(sSQL);
                }
            
            return res;
        }


        public DataRow HasRow(string DetectorName, string ItemType)
        {
            DataTable dt = BasicSelect(DetectorName, ItemType);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }
        public DataRow HasRow(long l, long m)
        {
            DataTable dt = BasicSelect(l, m);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }
 
        public bool Has(string DetectorName, string ItemType)
        {
            return (HasRow(DetectorName, ItemType) != null);
        }

        private DataTable AllWithKeyNames()
        {
            db.SetConnection();
            string sSQL = "SELECT material_types.name, detectors.detector_name, analysis_method_rec.* " +
               "FROM analysis_method_rec " +
               "INNER JOIN detectors ON (detectors.detector_id=analysis_method_rec.analysis_method_detector_id)" +
               "INNER JOIN material_types ON (material_types.id=analysis_method_rec.item_type_id)"; 
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        private DataTable BasicSelect(long l, long m)
        {
            db.SetConnection();
            string did = "", mid = "";
            if (l != -1)
                did = "analysis_method_detector_id= " + l.ToString();
            if (m != -1)
                mid = "item_type_id= " + m.ToString();
            string bin = ((l >= 0 && m >= 0) ? " AND " : "");
            string sSQL = "SELECT material_types.name, detectors.detector_name, analysis_method_rec.* " +
                "FROM analysis_method_rec " +
                "INNER JOIN detectors ON (detectors.detector_id=analysis_method_rec.analysis_method_detector_id)" +
                "INNER JOIN material_types ON (material_types.id=analysis_method_rec.item_type_id)";
            if (l >= 0 || m >= 0)
                sSQL += " where " + did + bin + mid;
            DataTable dt = db.DT(sSQL);
            return dt;
        }

       private void GetKeys(string DetectorName, string ItemType, out long l, out long m)
       {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            l = dets.PrimaryKey(DetectorName);
            Descriptors mats = new Descriptors("material_types", db);
            m = mats.PrimaryKey(ItemType);
       }

       private DataTable BasicSelect(string DetectorName, string ItemType)
        {
            db.SetConnection();
            long l,m;
            GetKeys(DetectorName, ItemType, out l, out m);

            string did = "", mid = "";
            if (l != -1)
                did = "analysis_method_detector_id= " +l.ToString();
            if (m != -1)
                mid = "item_type_id= " + m.ToString();
            string bin = ((l >= 0 && m >= 0) ?  " AND " : "");
            string sSQL = "SELECT material_types.name, detectors.detector_name, analysis_method_rec.* " +
                "FROM analysis_method_rec " + 
                "INNER JOIN detectors ON (detectors.detector_id=analysis_method_rec.analysis_method_detector_id)" +
                "INNER JOIN material_types ON (material_types.id=analysis_method_rec.item_type_id)";
             if (l >= 0 || m >= 0)  
                sSQL += " where " + did + bin + mid;
            DataTable dt = db.DT(sSQL);
            return dt;
        }


       ///////////////////////

        public bool UpdateCalib(string DetectorName, string ItemType, string calib_table, ElementList sParams)
        {
            //NEXT: this duo-lookup part takes too long, so get the values once in a wrapper call, then cache them, then reuse them
            long l, m; 
            GetKeys(DetectorName, ItemType, out l, out m);
            DataRow dr = HasRow(l, m); // sets the connection

            if (l == -1 || m == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det/Mat keys ({0},{1}) selecting method calibration params", l, m);
                return false;
            }
            string wh = " where item_type_id= " + m.ToString() + " AND detector_id=" + l.ToString();
            string exsql = "select item_type_id from " + calib_table + wh; // 1 column is sufficient to show existence

            DataTable dt = db.DT(exsql);
            if (dt.Rows.Count < 1)
            {
                string sSQL = "insert into " + calib_table + " ";
                sParams.Add(new Element("item_type_id", m));
                sParams.Add(new Element("detector_id", l));
                sSQL += sParams.ColumnsValues;
                return db.Execute(sSQL);
            }
            else
            {
                string sSQL1 = "UPDATE " + calib_table + " SET ";
                string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
                return db.Execute(sSQL);
            }
        }


        public DataRow Get(string DetectorName, string ItemType, string calib_table)
        {
            long l, m;
            GetKeys(DetectorName, ItemType, out l, out m);
            DataRow dr = HasRow(l, m); // sets the connection

            if (l == -1 || m == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det/Mat keys ({0},{1}) selecting method calibration params", l, m);
                return null;
            }
            string wh = " where item_type_id= " + m.ToString() + " AND detector_id=" + l.ToString(); string sql = "select * from " + calib_table + wh;
            DataTable dt = db.DT(sql);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            else
                return null;
        }

    }
}
