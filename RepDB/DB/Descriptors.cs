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
        // all descriptors have an name and a description field, only the table name varies
        public class Descriptors
        {
            IDB db;
            string table;

            public Descriptors(string table, IDB db = null)
			{
                if (db == null)
                    this.db = new DB();
                else
                    this.db = db;
                this.table = table;
			}

			public DataTable  getDescs()
			{
                string sSQL = "SELECT [id] "
                    + " ,[name] "
                    + " ,[description] "
                    + " FROM " + table;

				return  db.DT(sSQL);
			}
            public bool Delete(string Id)
            {
                db.SetConnection();
                string s = "DELETE FROM " + table + " where " + SQLSpecific.QValCompare("name", Id, true);
                return db.Execute(s);
            }

            public long PrimaryKey(string Id)
            {
                if (String.IsNullOrEmpty(Id))
                    return -1;
                db.SetConnection();
                string s = "SELECT id FROM " + table + " where " + SQLSpecific.QValCompare("name", Id, true);
                string r = db.Scalar(s);
                long l = -1;
                if (!Int64.TryParse(r, out l))
                    l = -1;
                return l;
            }

            public bool Update(string Name, string Description)
            {
                db.SetConnection();
                string sSQL = "";
                bool updated = false;
                if (Unique(Name, Description))
                    return false;

                DataTable dt = GetRows(Name); // must be unknown or at least one with same name because unique did not fire
                if (dt != null)
                {
                    DataRow[] dr = dt.Select("description = " + SQLSpecific.QVal(Description));
                    if (dr.Length < 1) // desc changed for an existing type
                    {
                        sSQL = "UPDATE " + table +" SET "
                            + "[description] = " + SQLSpecific.QVal(Description)
                            + " WHERE " + SQLSpecific.QValCompare("name", Name, true);
                        updated = db.Execute(sSQL);
                    }
                }
                else  // totally new type name
                {
                    sSQL = "Insert into "  + table + " ([name], [description]) "
                        + " Values (" + SQLSpecific.QVal(Name) + "," + SQLSpecific.QVal(Description) + ")";
                    updated = db.Execute(sSQL);
                }
                return updated;
            }
            public DataRow UniqueRow(string Name, string Desc)
            {
                DataTable dt = BasicSelect(Name, Desc);
                if (dt.Rows.Count == 1) return dt.Rows[0];
                else return null;
            }

            public DataRow HasRow(string Name, string Desc)
            {
                DataTable dt = BasicSelect(Name, Desc);
                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }

            public DataTable GetRows(string Name)
            {
                string sSQL = "SELECT * "
                      + " FROM "  + table
                      + " Where " + SQLSpecific.QValCompare("name", Name, true);
                DataTable dt = db.DT(sSQL);
                if (dt.Rows.Count > 0) return dt;
                else return null;
            }

            public bool Unique(string Name, string Desc)
            {
                return (UniqueRow(Name, Desc) != null);
            }

            public bool Has(string Name, string Desc)
            {
                return (HasRow(Name, Desc) != null);
            }

            private DataTable BasicSelect(string Name, string Desc)
            {
                db.SetConnection();
                string sSQL = "SELECT name "
                      + " FROM "  + table
                      + " Where " + SQLSpecific.QValCompare("name", Name, true) + " AND ";
                sSQL += SQLSpecific.QValCompare("description", Desc, true);
                DataTable dt = db.DT(sSQL);
                return dt;
            }
 
		}
	}


