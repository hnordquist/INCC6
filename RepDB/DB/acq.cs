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
namespace DB
{

    // tables that ref a detector and datetime
    /// <summary>
    /// Summary description for AcquireParams.
    /// </summary>
    public class AcquireParams
    {
        public AcquireParams()
        {
            db = new DB2(true);
        }
        public AcquireParams(IDB db)
        {
            this.db = db;
        }
        public IDB db;

        // NEXT: figure out difference between use of meas_detector_id and detector_id
        public bool Has(DateTimeOffset measDatetime, string measDetId, string item_type)
        {
            db.SetConnection();
            string sSQL = "select * from acquire_parms_rec where meas_detector_id=" + SQLSpecific.Value(measDetId, true) +
                            " and item_type=" + SQLSpecific.Value(item_type, true);
            DataTable dt = db.DT(sSQL);
            return dt.Rows.Count > 0;
        }

        public DataTable Get(string measDetId) // joins the matching LM if found
        {
            db.SetConnection();
			Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "select * from acquire_parms_rec left join LMAcquireParams on (LMAcquireParams.detector_id=" + l.ToString() + ") " +
                "where (acquire_parms_rec.meas_detector_id=" + SQLSpecific.Value(measDetId, true) + ")" +
                 " order by acquire_parms_rec.MeasDate DESC";
            DataTable dt = db.DT(sSQL);                
            return dt;

        }
        public bool Create(ElementList sParams)
        {
            db.SetConnection();
            string sSQL1 = "Insert into acquire_parms_rec ";
            string sSQL = sSQL1 + sParams.ColumnsValues;
            return db.Execute(sSQL);
        }

        public bool Update(DateTimeOffset measDatetime, string measDetId, string itemId, ElementList sParams)
        {
            db.SetConnection();
            string wh = " where item_type = " + SQLSpecific.Value(itemId, true) + " AND meas_detector_id = " + SQLSpecific.Value(measDetId, true);
            string sSQL1 = "UPDATE acquire_parms_rec SET ";
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }
  


    }


    public class LMAcquireParams
    {
        public LMAcquireParams()
        {
            db = new DB2(true);
        }
        public LMAcquireParams(IDB x)
        {
            db = x;
        }
        public IDB db;

        public bool Has(DateTimeOffset measDatetime, string measDetId, string item_type) 
        {
            DataTable dt = Get(measDatetime, measDetId, item_type);
            return dt.Rows.Count > 0;
        }

        public DataTable Get(string measDetId, string item_type) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "Select * FROM LMAcquireParams";
            sSQL += " where detector_id = " + l.ToString() + " AND item_type=" + SQLSpecific.Value(item_type, true);
            sSQL += " ORDER BY MeasDate DESC";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        public DataTable Get(DateTimeOffset measDatetime, string measDetId, string item_type)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db); 
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "Select * FROM LMAcquireParams where detector_id = " + l.ToString() + " AND item_type=" + SQLSpecific.Value(item_type, true);
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        public bool Create(DateTimeOffset measDatetime, string measDetId, ElementList sParams)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db); 
            long l = dets.PrimaryKey(measDetId);
            sParams.Add(new Element("detector_id", l));
            string sSQL1 = "Insert into LMAcquireParams ";
            string sSQL = sSQL1 + sParams.ColumnsValues;
            return db.Execute(sSQL);
        }

        public bool Update(string measDetId, string item_type, ElementList sParams)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL1 = "UPDATE LMAcquireParams SET ";
            string sSQL1i = String.Empty;
            string wh = " where " + "item_type = " + SQLSpecific.Value(item_type, true) + " AND detector_id = " + l.ToString();
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }

    }


}

