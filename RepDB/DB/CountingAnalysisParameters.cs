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
using NCCReporter;
using System.Collections;

namespace DB
{
    public class CountingAnalysisParameters
    {

        public CountingAnalysisParameters()
        {
            db = new DB(false);
        }
        public IDB db;
        private const int faidx = 6;
        private const int gwidx = 0;

        public DataTable AnalyzerParamsForDetector(string DetName)
        {
            return AllWithKeyNames(DetName);
        }

        public bool Update(string DetectorName, string CounterType, ElementList sParams)
        {
            DataRow dr = null;

            string table = "CountingParams";
            if (CounterType.Equals("Multiplicity") || CounterType.Equals("Coincidence"))
                table = "LMMultiplicity";

            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            if (l == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det key ({0}) selecting CountingParams", l);
                return false;
            }

            if (table.Equals("LMMultiplicity"))
                dr = HasRow(l, CounterType, table, sParams, sParams[faidx].Value); // the FA parameter
            else
                dr = HasRow(l, CounterType, table, sParams);
            if (dr == null)
            {
                sParams.Add(new Element("detector_id", l));
                string sSQL = "Insert into " + table;
                sSQL += sParams.ColumnsValues;
                return db.Execute(sSQL);
            }
            else
            {
                string sSQL = "UPDATE " + table + " SET ";
                sSQL += (sParams.ColumnEqValueList + " where counter_type=" + SQLSpecific.QVal(CounterType) + " AND detector_id=" + l.ToString() + " AND gatewidth=" + sParams[gwidx].Value);
                if (table.Equals("LMMultiplicity"))
                    sSQL += " AND " + sParams[faidx].Name + "=" + sParams[faidx].Value;
                return db.Execute(sSQL);
            }
        }

        public bool Insert(string DetectorName, string CounterType, ElementList sParams)
        {
            DataRow dr = null;

            string table = "CountingParams";
            if (CounterType.Equals("Multiplicity") || CounterType.Equals("Coincidence"))
                table = "LMMultiplicity";

            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            if (l == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det key ({0}) selecting CountingParams", l);
                return false;
            }

            if (table.Equals("LMMultiplicity"))
                dr = HasRow(l, CounterType, table, sParams, sParams[faidx].Value); // the FA parameter
            else
                dr = HasRow(l, CounterType, table, sParams);
            if (dr == null)
            {
                sParams.Add(new Element("detector_id", l));
                string sSQL = "Insert into " + table;
                sSQL += sParams.ColumnsValues;
                return db.Execute(sSQL);
            }
            else
            {
                // identical row found, skip it
                return false;
            }
        }


        public bool Delete(string DetectorName, string CounterType, ElementList sParams)
        {
            DataRow dr = null;

            string table = "CountingParams";
            if (CounterType.Equals("Multiplicity") || CounterType.Equals("Coincidence"))
                table = "LMMultiplicity";

            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            if (l == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det key ({0}) selecting CountingParams", l);
                return false;
            }

            if (table.Equals("LMMultiplicity"))
                dr = HasRow(l, CounterType, table, sParams, sParams[faidx].Value); // the FA parameter
            else
                dr = HasRow(l, CounterType, table, sParams);
            if (dr != null)
            {
                string sSQL = "DELETE FROM " + table + " where counter_type=" + SQLSpecific.QVal(CounterType) + " AND detector_id=" + l.ToString();
                if (table.Equals("LMMultiplicity"))
                    sSQL += " AND " + sParams[faidx].Name + "=" + sParams[faidx].Value;
                return db.Execute(sSQL);
            }
            else
                return true;
        }

        public bool Delete(long DetectorId, string CounterType, ElementList sParams)
        {
            DataRow dr = null;

            string table = "CountingParams";
            if (CounterType.Equals("Multiplicity") || CounterType.Equals("Coincidence"))
                table = "LMMultiplicity";

            if (table.Equals("LMMultiplicity"))
                dr = HasRow(DetectorId, CounterType, table, sParams, sParams[faidx].Value); // the FA parameter
            else
                dr = HasRow(DetectorId, CounterType, table, sParams);
            if (dr != null)
            {
                string sSQL = "DELETE FROM " + table + " where counter_type=" + SQLSpecific.QVal(CounterType) + " AND detector_id=" + DetectorId.ToString();
                if (table.Equals("LMMultiplicity"))
                    sSQL += " AND " + sParams[faidx].Name + "=" + sParams[faidx].Value;
                return db.Execute(sSQL);
            }
            else
                return true;
        }

        public bool DeleteAll(string DetectorName, string CounterType, ElementList sParams)
        {
            DataRow dr = null;

            string table = "CountingParams";
            if (CounterType.Equals("Multiplicity") || CounterType.Equals("Coincidence"))
                table = "LMMultiplicity";

            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            if (l == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det key ({0}) selecting CountingParams", l);
                return false;
            }

            if (table.Equals("LMMultiplicity"))
                dr = HasRow(l, CounterType, table, sParams, sParams[faidx].Value); // the FA parameter
            else
                dr = HasRow(l, CounterType, table, sParams);
            if (dr != null)
            {
                string sSQL = "DELETE FROM " + table + " where counter_type=" + SQLSpecific.QVal(CounterType) + " AND detector_id=" + l.ToString();
                if (table.Equals("LMMultiplicity"))
                    sSQL += " AND " + sParams[faidx].Name + "=" + sParams[faidx].Value;
                return db.Execute(sSQL);
            }
            else
                return true;
        }

        public long DeleteAll(string DetectorName)
        {
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            if (l == -1)
            {
                DBMain.AltLog(LogLevels.Warning, 70137, "Missing Det key ({0}) selecting CountingParams", l);
                return -1;
            }
            string table1 = "CountingParams";
            string table2 = "LMMultiplicity";
            ArrayList sqlList = new ArrayList();  
            sqlList.Add("DELETE FROM " + table1 + " where detector_id=" + l.ToString());
            sqlList.Add("DELETE FROM " + table2 + " where detector_id=" + l.ToString());
            return db.Execute(sqlList);

        }

        public DataRow HasRow(long DetectorId, string CounterType, string table, ElementList el, string FA = null)
        {
            DataTable dt = GetRows(DetectorId, CounterType, table, el, FA);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public bool Has(string DetectorName, string CounterType)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            string sSQL = "Select * from INNER JOIN LMMultiplicity, CountingParams ON (LMMultiplicity.detector_id=CountingParams.detector_id) " +
                "where detector_id=" + l.ToString() + " AND " + "counter_type=" + SQLSpecific.QVal(CounterType);
            DataTable dt = db.DT(sSQL);
            return (dt.Rows.Count > 0);
        }

        private DataTable AllWithKeyNames(string DetectorName)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(DetectorName);
            string sSQLa = "Select * from CountingParams where CountingParams.detector_id=" + l.ToString();
            string sSQLb = "Select * from LMMultiplicity where LMMultiplicity.detector_id=" + l.ToString();
            DataTable dta = db.DT(sSQLa);
            DataTable dtb = db.DT(sSQLb);
            dta.Merge(dtb);
            return dta;
        }

        private DataTable GetRows(long DetectorId, string CounterType, string table, ElementList el, string FA)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM " + table + " where detector_id=" + DetectorId.ToString() +
                " AND counter_type=" + SQLSpecific.QVal(CounterType) + 
                " AND gatewidth=" + SQLSpecific.QVal(el[gwidx].Value);
            if (!string.IsNullOrEmpty(FA))
            {
                sSQL += " AND " + el[faidx-2].Name + "=" + el[faidx-2].Value;
                sSQL += " AND " + el[faidx-1].Name + "=" + el[faidx-1].Value;
                sSQL += " AND " + el[faidx].Name + "=" + FA;
            }
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        ///////////////////////

    }


}
