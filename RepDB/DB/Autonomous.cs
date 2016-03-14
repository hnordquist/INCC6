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
using System.Data.Common;

namespace DB
{

    public class AppContext
    {

        public AppContext()
        {
            db = new DB(false);
        }
        DB db;


        public DataTable GetAll()
        {
            db.SetConnection();
            string sSQL = "Select * from LMINCCAppContext";
            DataTable dt = db.DT(sSQL);
            return dt;
        }
        public DataRow Get()
        {
            db.SetConnection();
            DataTable dt = GetAll();
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }
        public bool Update(ElementList sParams)
        {
            DataRow dr = null;
            string table = "LMINCCAppContext";
            dr = Get();
            if (dr == null)
            {
                string sSQL = "Insert into " + table;
                sSQL += sParams.ColumnsValues;
                return db.Execute(sSQL);
            }
            else
            {
                string sSQL1 = "UPDATE " + table + " SET ";
                string sSQL = sSQL1 + sParams.ColumnEqValueList;
                return db.Execute(sSQL);
            }
        }

        public bool Has()
        {
            db.SetConnection();
            string sSQL = "Select * from LMINCCAppContext";
            DataTable dt = db.DT(sSQL);
            return (dt.Rows.Count > 0);
        }
    }
    public class TestParams
    {
        public TestParams()
        {
            db = new DB(false);
        }

        DB db;

        public bool Has() // all of them and all them's's columns
        {
            DataTable dt = Get();
            return dt.Rows.Count > 0;
        }

        public long Create(ElementList sParams) // all of them and all them's's columns
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "Insert into test_parms_rec ";
            sSQL += sParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("test_parms_rec"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Update(ElementList sParams) // all of them and all them's's columns
        {
            db.SetConnection();
            string sSQL = "UPDATE test_parms_rec SET ";
            sSQL += sParams.ColumnEqValueList;
            return db.Execute(sSQL);
        }

        public DataTable Get() // all of them
        {
            db.SetConnection();
            string sSQL = "Select * FROM test_parms_rec";
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public bool Delete(string Id)
        {
            db.SetConnection();
            string s = "DELETE FROM test_parms_rec";
            return db.Execute(s);
        }
    }


    /// <summary>
    /// Summary description for Items.
    /// </summary>
    public class Items
    {

        public Items()
        {
            db = new DB(false);
        }
        DB db;

        public DataTable getItems()
        {
            db.SetConnection();
            string sSQL = "Select * FROM items";

			//DbCommand dbcmd = DBMain.DBCmd;
			//dbcmd.Connection = db.sql_con;
			//DbParameter p = dbcmd.CreateParameter();
			//p.ParameterName = "items";
			//p.Value = "items";
			//dbcmd.CommandType = CommandType.Text;
			//dbcmd.Parameters.Add(p);
			//dbcmd.CommandText = "Select * FROM";
			//DbDataReader dbr = dbcmd.ExecuteReader();

            return (db.DT(sSQL));
        }

        public bool Delete(String Id)
        {
            db.SetConnection();
            string s = "DELETE FROM items where item_name = " + SQLSpecific.QVal(Id); ;
            return db.Execute(s);
        }
        public bool Delete(Int32 Id)
        {
            db.SetConnection();
            string s = "DELETE FROM items where item_id = " + Id.ToString();
            return db.Execute(s);
        }
        public long PrimaryKey(string Id)
        {
            if (String.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT item_id FROM items where item_name = " + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!Int64.TryParse(r, out l))
                l = -1;
            return l;
        }

        public bool Update(long Id, string NewId)
        {
            db.SetConnection();
            string wh = " where " + "item_id = " + Id.ToString();
            string sSQL1 = "UPDATE items SET item_name=" + SQLSpecific.QVal(NewId);
            string sSQL = sSQL1 + wh;
            return db.Execute(sSQL);
        }

        public long Update(string Name, ElementList sParams)
        {
            db.SetConnection();
            DataTable dt = GetRows(Name); // must be unknown or at least one with same name because unique did not fire
            if (dt != null)
            {
                string wh = " where item_name = " + SQLSpecific.QVal(Name);
                string sSQL = "UPDATE items SET ";
                sSQL += sParams.ColumnEqValueList + wh;
                return db.Execute(sSQL) ? 0 : -1;
            }
            else  // totally new 
            {
                ArrayList sqlList = new ArrayList();
                string sSQL = "Insert into items ";
                sSQL += sParams.ColumnsValues;
                sqlList.Add(sSQL);
                sqlList.Add(SQLSpecific.getLastID("items"));
                return db.ExecuteTransactionID(sqlList);
            }
        }

        public DataTable GetRows(string Name)
        {
            string sSQL = "SELECT * "
                  + " FROM items"
                  + " Where item_name = " + SQLSpecific.QVal(Name);
            DataTable dt = db.DT(sSQL);
            if (dt.Rows.Count > 0) return dt;
            else return null;
        }


        public DataRow UniqueRow(string ItemName)
        {
            DataTable dt = BasicSelect(ItemName);
            if (dt.Rows.Count == 1) return dt.Rows[0];
            else return null;
        }

        public DataRow HasRow(string ItemName)
        {
            DataTable dt = BasicSelect(ItemName);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public bool Unique(string ItemName)
        {
            return (UniqueRow(ItemName) != null);
        }

        public bool Has(string ItemName)
        {
            return (HasRow(ItemName) != null);
        }

        private DataTable BasicSelect(string ItemName)
        {
            db.SetConnection();
            string sSQL = "SELECT item_name "
                  + " FROM items"
                  + " Where item_name = " + SQLSpecific.QVal(ItemName);
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public long getItemID(string item_name)
        {
            //Check item name against current entries
            db.CreateCommand("Select item_id from items where item_name=" + SQLSpecific.QVal(item_name));
            db.SetConnection();
            return db.ScalarIntx();
        }

    }

    /// <summary>
    /// Summary description for CollarItems.
    /// </summary>
    public class CollarItems
    {

        public CollarItems()
        {
            db = new DB(false);
        }
        DB db;

        public DataTable getItems()
        {
            db.SetConnection();
            string sSQL = "Select * FROM collar_data_entry";
            return (db.DT(sSQL));
        }

        public bool Delete(String Id)
        {
            db.SetConnection();
            string s = "DELETE FROM collar_data_entry where item_name = " + SQLSpecific.QVal(Id); ;
            return db.Execute(s);
        }
        public bool Delete(Int32 Id)
        {
            db.SetConnection();
            string s = "DELETE FROM collar_data_entry where item_id = " + Id.ToString();
            return db.Execute(s);
        }
        public long PrimaryKey(string Id)
        {
            if (String.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT item_id FROM collar_data_entry where item_name = " + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!Int64.TryParse(r, out l))
                l = -1;
            return l;
        }

        public bool Update(long Id, string NewId)
        {
            db.SetConnection();
            string wh = " where " + "item_id = " + Id.ToString();
            string sSQL1 = "UPDATE collar_data_entry SET item_name=" + SQLSpecific.QVal(NewId);
            string sSQL = sSQL1 + wh;
            return db.Execute(sSQL);
        }

        public long Update(string Name, ElementList sParams)
        {
            db.SetConnection();
            DataTable dt = GetRows(Name); // must be unknown or at least one with same name because unique did not fire
            if (dt != null)
            {
                string wh = " where item_name = " + SQLSpecific.QVal(Name);
                string sSQL = "UPDATE collar_data_entry SET ";
                sSQL += sParams.ColumnEqValueList + wh;
                return db.Execute(sSQL) ? 0 : -1;
            }
            else  // totally new 
            {
                ArrayList sqlList = new ArrayList();
                string sSQL = "Insert into collar_data_entry ";
                sSQL += sParams.ColumnsValues;
                sqlList.Add(sSQL);
                sqlList.Add(SQLSpecific.getLastID("collar_data_entry"));
                return db.ExecuteTransactionID(sqlList);
            }
        }

        public DataTable GetRows(string Name)
        {
            string sSQL = "SELECT * "
                  + " FROM collar_data_entry"
                  + " Where item_name = " + SQLSpecific.QVal(Name);
            DataTable dt = db.DT(sSQL);
            if (dt.Rows.Count > 0) return dt;
            else return null;
        }


        public DataRow UniqueRow(string ItemName)
        {
            DataTable dt = BasicSelect(ItemName);
            if (dt.Rows.Count == 1) return dt.Rows[0];
            else return null;
        }

        public DataRow HasRow(string ItemName)
        {
            DataTable dt = BasicSelect(ItemName);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public bool Unique(string ItemName)
        {
            return (UniqueRow(ItemName) != null);
        }

        public bool Has(string ItemName)
        {
            return (HasRow(ItemName) != null);
        }

        private DataTable BasicSelect(string ItemName)
        {
            db.SetConnection();
            string sSQL = "SELECT item_name "
                  + " FROM collar_data_entry"
                  + " Where item_name = " + SQLSpecific.QVal(ItemName);
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public long getItemID(string item_name)
        {
            //Check item name against current entries
			db.CreateCommand("Select item_id from collar_data_entry where item_name=" + SQLSpecific.QVal(item_name));
            db.SetConnection();
            return db.ScalarIntx();
        }

    }
    /// <summary>
    /// Summary description for Isotopics.
    /// </summary>
    public class Isotopics
    {

        DB db;
        public Isotopics()
        {
            db = new DB(false);
        }

        public DataTable getIsotopics() // all of them and all them's's columns
        {
            db.SetConnection();
            string sSQL = "Select * FROM isotopics";
            return db.DT(sSQL);
        }

        public bool Delete(string Id)
        {
            db.SetConnection();
            string s = "DELETE FROM isotopics where isotopics_id = " + SQLSpecific.QVal(Id);
            return db.Execute(s);
        }

        public long PrimaryKey(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT id FROM isotopics where isotopics_id = " + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!long.TryParse(r, out l))
                l = -1;
            return l;
        }

        public bool Update(long Id, string NewId)
        {
            db.SetConnection();
            string wh = " where id = " + Id.ToString();
            string sSQL1 = "UPDATE isotopics SET isotopics_id=" + SQLSpecific.QVal(NewId);
            string sSQL = sSQL1 + wh;
            return db.Execute(sSQL);
        }

        public long Update(string Id, ElementList sParams)
        {
            db.SetConnection();

            DataTable dt = GetRows(Id); // must be unknown or at least one with same name because unique did not fire
            if (dt != null)
            {
                string wh = " where " + "isotopics_id = " + SQLSpecific.QVal(Id);
                string sSQL = "UPDATE isotopics SET ";
                sSQL += sParams.ColumnEqValueList + wh;
                if (db.Execute(sSQL))
                    return PrimaryKey(Id);
                else
                    return -1;
            }
            else  // totally new 
            {
                ArrayList sqlList = new ArrayList();
                string sSQL = "Insert into isotopics ";
                sSQL += sParams.ColumnsValues;
                sqlList.Add(sSQL);
                sqlList.Add(SQLSpecific.getLastID("isotopics"));
                return db.ExecuteTransactionID(sqlList);
            }
        }

        public DataTable GetRows(string Name)
        {
            string sSQL = "SELECT * "
                  + " FROM isotopics where isotopics_id = " + SQLSpecific.QVal(Name);
            db.SetConnection();
            DataTable dt = db.DT(sSQL);
            if (dt.Rows.Count > 0) return dt;
            else return null;
        }


        public DataRow UniqueRow(string IsoName)
        {
            DataTable dt = BasicSelect(IsoName);
            if (dt.Rows.Count == 1) return dt.Rows[0];
            else return null;
        }

        public DataRow HasRow(string IsoName)
        {
            DataTable dt = BasicSelect(IsoName);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public bool Unique(string IsoName)
        {
            return (UniqueRow(IsoName) != null);
        }

        public bool Has(string IsoName, Int32 DetectorType, string DetectorElectronics)
        {
            return (HasRow(IsoName) != null);
        }

        private DataTable BasicSelect(string IsoName)
        {
            db.SetConnection();
            string sSQL = "SELECT isotopics_id "
                  + " FROM isotopics"
                  + " Where isotopics_id = " + SQLSpecific.QVal(IsoName);
            DataTable dt = db.DT(sSQL);
            return dt;
        }

    }

    /// <summary>
    /// Summary description for CompositeIsotopics.
    /// </summary>
    public class CompositeIsotopics
    {

        DB db;
        public CompositeIsotopics()
        {
            db = new DB(false);
        }

        public DataTable getCompositeIsotopics()
        {
            db.SetConnection();
            string sSQL = "Select * FROM composite_isotopics_rec";
            return db.DT(sSQL);
        }

        public bool Delete(string Id)
        {
            db.SetConnection();
            string s = "DELETE FROM composite_isotopics_rec where isotopics_id = " + SQLSpecific.QVal(Id);
            return db.Execute(s);
        }

		public long PrimaryKey(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return -1;
            db.SetConnection();
            string s = "SELECT id FROM composite_isotopics_rec where ci_isotopics_id = " + SQLSpecific.QVal(Id);
            string r = db.Scalar(s);
            long l = -1;
            if (!long.TryParse(r, out l))
                l = -1;
            return l;
        }

        public long Update(string Id, ElementList sParams)
        {
            db.SetConnection();

            DataTable dt = GetRows(Id); // must be unknown or at least one with same name because unique did not fire
            if (dt != null)
            {
                string wh = " where " + "ci_isotopics_id = " + SQLSpecific.QVal(Id);
                string sSQL = "UPDATE composite_isotopics_rec SET ";
                sSQL += sParams.ColumnEqValueList + wh;
                if (db.Execute(sSQL))
                    return PrimaryKey(Id);
                else
                    return -1;
			}
            else  // totally new 
            {
                ArrayList sqlList = new ArrayList();
                string sSQL = "Insert into composite_isotopics_rec ";
                sSQL += sParams.ColumnsValues;
                sqlList.Add(sSQL);
                sqlList.Add(SQLSpecific.getLastID("composite_isotopics_rec"));
                return db.ExecuteTransactionID(sqlList);
            }
        }

        public DataTable GetRows(string Name)
        {
            string sSQL = "SELECT * "
                  + " FROM composite_isotopics_rec"
                  + " Where ci_isotopics_id = " + SQLSpecific.QVal(Name);
            DataTable dt = db.DT(sSQL);
            if (dt.Rows.Count > 0) return dt;
            else return null;
        }


        public DataRow UniqueRow(string IsoName)
        {
            DataTable dt = BasicSelect(IsoName);
            if (dt.Rows.Count == 1) return dt.Rows[0];
            else return null;
        }

        public DataRow HasRow(string IsoName)
        {
            DataTable dt = BasicSelect(IsoName);
            if (dt.Rows.Count > 0) return dt.Rows[0];
            else return null;
        }

        public bool Unique(string IsoName)
        {
            return (UniqueRow(IsoName) != null);
        }

        public bool Has(string IsoName, Int32 DetectorType, string DetectorElectronics)
        {
            return (HasRow(IsoName) != null);
        }

        private DataTable BasicSelect(string IsoName)
        {
            db.SetConnection();
            string sSQL = "SELECT ci_isotopics_id "
                  + " FROM composite_isotopics_rec"
                  + " Where ci_isotopics_id = " + SQLSpecific.QVal(IsoName);
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        /// composite isotopic element /////////////////////////////////////////
        ///
        /// yes
        public long AddCIRetId(long cid, ElementList sParams)
        {
            //Returns ID
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = CIInsertStatement(cid, sParams);
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("composite_isotopic_rec"));
            return db.Execute(sqlList);
        }

        public bool AddCI(long cid, ElementList sParams)
        {
            db.SetConnection();
            string sSQL = CIInsertStatement(cid, sParams);
            return db.Execute(sSQL);
        }

        protected string CIInsertStatement(long cid, ElementList sParams)
        {
            sParams.Add(new Element("cid", cid));
            return "Insert into composite_isotopic_rec " + sParams.ColumnsValues;
        }

        public bool AddCIs(long cid, List<ElementList> sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "";
            foreach (ElementList els in sParams)
            {
                sSQL = CIInsertStatement(cid, els);
                sqlList.Add(sSQL);
            }
            return db.ExecuteTransaction(sqlList);
        }

        public DataTable GetCIs(long cid)
        {
            db.SetConnection();
            string sSQL = "SELECT * FROM composite_isotopic_rec Where cid=" + cid;
            return db.DT(sSQL);
        }

        public long Lookup(string name)
        {
            if (string.IsNullOrEmpty(name))
                return -1;
            db.SetConnection();
            string s = "SELECT * FROM composite_isotopic_recs WHERE ci_isotopics_id=" + SQLSpecific.QVal(name);
            string r = db.Scalar(s);
            long lr = -1;
            if (!long.TryParse(r, out lr))
                lr = -1;
            return lr;
        }

    }

    public class cm_pu_ratio_rec
    {
        public cm_pu_ratio_rec()
        {
            db = new DB(false);
        }

        DB db;

        public bool Has() // has at least one
        {
            DataTable dt = Get();
            return dt.Rows.Count > 0;
        }

        public long Create(ElementList sParams)
        {
            db.SetConnection();
            ArrayList sqlList = new ArrayList();
            string sSQL = "Insert into cm_pu_ratio_rec ";
            sSQL += sParams.ColumnsValues;
            sqlList.Add(sSQL);
            sqlList.Add(SQLSpecific.getLastID("cm_pu_ratio_rec"));
            return db.ExecuteTransactionID(sqlList);
        }

        public bool Update(ElementList sParams)
        {
            db.SetConnection();
            string sSQL = "UPDATE cm_pu_ratio_rec SET ";
            sSQL += sParams.ColumnEqValueList;
            return db.Execute(sSQL);
        }

        public DataTable Get() // all of them
        {
            db.SetConnection();
            string sSQL = "Select * FROM cm_pu_ratio_rec";
            DataTable dt = db.DT(sSQL);
            return dt;
        }

        public bool Delete(string Id)
        {
            db.SetConnection();
            string s = "DELETE FROM cm_pu_ratio_rec";
            return db.Execute(s);
        }
    }

}
