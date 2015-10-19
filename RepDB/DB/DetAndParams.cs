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
    // database tables with references back to a specific detector
    public class ParamsRelatedBackToDetector
    {
        public string table;

        public ParamsRelatedBackToDetector(string table, IDB db = null)
        {
            this.table = table;
            if (db != null)
                this.db = db;
            else
                this.db = new DB(false);
        }
        IDB db;

        public bool Has(string measDetId)
        {
            DataTable dt = Get(measDetId);
            return dt.Rows.Count > 0;
        }

        public DataTable Get(string measDetId)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL = "Select * FROM " + table + " where detector_id = " + l.ToString();
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public bool Create(string measDetId, ElementList sParams) 
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            sParams.Add(new Element("detector_id", l));
            string sSQL1 = "Insert into " + table + " ";
            string sSQL = sSQL1 + sParams.ColumnsValues;            
            return db.Execute(sSQL);
        }

        public bool Update(string measDetId, ElementList sParams)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL1 = "UPDATE " + table + " SET ";
            string sSQL1i = String.Empty;
            string wh = " where " + "detector_id = " + l.ToString();
            string sSQL = sSQL1 + sParams.ColumnEqValueList + wh;
            return db.Execute(sSQL);
        }

        public bool Delete(string measDetId)
        {
            db.SetConnection();
            Detectors dets = new Detectors(db);
            long l = dets.PrimaryKey(measDetId);
            string sSQL1 = "DELETE from " + table;
            string sSQL1i = String.Empty;
            string wh = " where " + "detector_id = " + l.ToString();
            string sSQL = sSQL1 + wh;
            return db.Execute(sSQL);
        }

    }
    public class TruncatedBackgroundParams : ParamsRelatedBackToDetector
    {
        public TruncatedBackgroundParams(IDB db = null)
            : base("tm_bkg_parms_rec", db)
        {
        }
    }
    public class BackgroundParams : ParamsRelatedBackToDetector
    {
        public BackgroundParams(IDB db = null)
            : base("bkg_parms_rec", db)
        {
        }
    }
    public class UnattendParams : ParamsRelatedBackToDetector
    {
        public UnattendParams(IDB db = null)
            : base("unattended_parms_rec", db)
        {
        }
    }
    public class NormParams : ParamsRelatedBackToDetector
    {
        public NormParams(IDB db = null)
            : base("norm_parms_rec", db)
        {
        }
    }
    public class AASSetupParams : ParamsRelatedBackToDetector
    {
        public AASSetupParams(IDB db = null)
            : base("add_a_source_setup_rec", db)
        {
        }
    }
    public class HVParams : ParamsRelatedBackToDetector
    {
        public HVParams(IDB db = null)
            : base("HVCalibrationParams", db)
        {
        }
    }

}
