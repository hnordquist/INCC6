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
using System.Collections.Generic;
using System.Data;

namespace DB
{
    /// <summary>
    /// Summary description for Strata.
    /// 	[stratum_id] INTEGER Primary Key,
    //[name] [nvarchar(256) ] NULL,
    //[description] [nvarchar(4000) ] NULL,
    //[historical_bias] [float] NULL,
    //[historical_rand_uncert] [float] NULL,
    //[historical_systematic_uncert] [float] NULL,
    //	[detector_id] INTEGER NULL 
    /// </summary>
    public class Strata
    {
        DB db;
        public Strata()
        {
            db = new DB();
        }
       


        //////

        public bool HasAssociation(string measDetId, string stratumId) 
        {
            DataTable dt = Get(measDetId, stratumId);
            return dt.Rows.Count > 0;
        }
        public bool Has(string stratumId)
        {
            return PrimaryKey(stratumId) >= 0;
        }
        // return the stratums only
        public DataTable Get()
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            string sSQL = "Select * FROM stratum_ids";
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        // return the stratum
        public DataTable Get(string stratumId)
        {
            db.SetConnection();
            string sSQL = "Select * FROM stratum_ids where name=" + SQLSpecific.QVal(stratumId);
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        // return the stratums associated with a detector
        public DataTable GetAssociations(string measDetId)
        {
            if (String.IsNullOrEmpty(measDetId))
                return Get();
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "Select * FROM stratum_ids INNER JOIN stratum_id_detector ON (stratum_id_detector.stratum_id=stratum_ids.stratum_id AND stratum_id_detector.detector_id=" + l.ToString() + ")";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        // return the detectors associated with a stratum
        public List<string> GetAssociationsByStratum(string stratum_id)
        {
            List<string> dets = new List<string>();

            db.SetConnection();
            string sSQL = "Select * FROM stratum_id_detector WHERE stratum_id=" + PrimaryKey(stratum_id).ToString(); ;
            DataTable dt = db.DT(sSQL);
            foreach (DataRow dr in dt.Rows)
            {
                string det_id = dr["detector_id"].ToString();
                dets.Add(det_id);
            }
            return dets;
        }
        public long PrimaryKey(string Id)
        {
            if (String.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT stratum_id FROM stratum_ids where name=" + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!Int64.TryParse(r, out l))
                l = -1;
            return l;
        }
        // return the named stratum associated with a detector
        public DataTable Get(string measDetId, string stratumId) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            long m = PrimaryKey(stratumId);
            string sSQL = "Select * FROM stratum_ids INNER JOIN stratum_id_detector ON " + 
                "(stratum_id_detector.stratum_id=stratum_ids.stratum_id AND stratum_id_detector.detector_id=" + l.ToString() 
                + " AND stratum_id_detector.stratum_id=" + m.ToString() + ")";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        // delete the named stratum association with a detector
        public bool Delete(string measDetId, string stratumId)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            long m = PrimaryKey(stratumId);
            string sSQL = "DELETE FROM stratum_id_detector where " + "(stratum_id_detector.stratum_id=" + m.ToString() + " AND stratum_id_detector.detector_id=" + l.ToString() + ")";
            return db.Execute(sSQL);
        }

        // delete all the stratum associations with a detector
        public bool Unassociate(string measDetId)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "DELETE FROM stratum_id_detector where " + "(stratum_id_detector.detector_id=" + l.ToString() + ")";
            return db.Execute(sSQL);
        }

        // delete the stratum and all detector associations
        public bool Delete(string stratumId)
        {
            db.SetConnection();
            long m = PrimaryKey(stratumId);
            ArrayList sqlList = new ArrayList();
            string sSQL = "DELETE FROM stratum_ids where " + "(stratum_id=" + m.ToString() + ")";
            string sSQL2 = "DELETE FROM stratum_id_detector where " + "(stratum_id_detector.stratum_id=" + m.ToString() + ")";
            sqlList.Add(sSQL);
            sqlList.Add(sSQL2);
            return db.Execute(sSQL);
        }

        private string InsertStratum(ElementList sParams)
        {
            string sSQL1 = "Insert into stratum_ids " + sParams.ColumnsValues;
            return sSQL1;
        }
        // create a stratum 
        public long Create(ElementList sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = InsertStratum(sParams);
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("stratum_ids"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Update(string name, ElementList sParams)
        {
            db.SetConnection();
            long m = PrimaryKey(name);
            string sSQL1 = "UPDATE stratum_ids SET ";
            string wh = " where stratum_id = " + m.ToString();
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }

        // create a stratum and detector stratum association
        public bool Create(string measDetId, ElementList sParams)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = InsertStratum(sParams);
            ArrayList sqlList = new ArrayList();
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("stratum_ids"));
            long newID = db.ExecuteTransactionID(sqlList);
            string saSQL = "insert into stratum_id_detector VALUES(" + l.ToString() + "," + newID.ToString() + ")";
            return db.Execute(saSQL);
        }
        public bool Associate(string measDetId, string stratumID)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            long m = PrimaryKey(stratumID);
            string saSQL = "insert into stratum_id_detector VALUES(" + l.ToString() + "," + m.ToString() + ")";
            return db.Execute(saSQL);
        }

    }
}
