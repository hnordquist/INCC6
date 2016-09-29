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
using System.Data.Common;
namespace DB
{
    public static class SQLSpecific
    {

        public static string QVal(string s)
        {
            return "'" + s + "'";
        }
        /// <summary>
        /// Builds an optionally case-insensitive string comparison clause with quoted strings
        /// </summary>
        /// <param name="f">the column name</param>
        /// <param name="s">the target value</param>
        /// <param name="ignoreCase">converts both strings to upper case before comparing, likely to work with our SQL dialects</param>
        /// <returns>the clause</returns>
        public static string QValCompare(string f, string s, bool ignoreCase)
        {
            if (ignoreCase)
                return "upper(" + f + ") = upper(" + QVal(s) + ")";
            return f + " = " + QVal(s);
        }

        public static string Value(string s, bool quote)
        {
            if (quote)
            {
                s = System.Text.RegularExpressions.Regex.Replace(s, @"[\'""]", @"$0$0");  // happy fun!
                switch (DBMain.Provider)
                {
                    case DBMain.DbsWeLove.SQLite:
                        s = "'" + s + "'";
                        break;
                    case DBMain.DbsWeLove.SQLCE4:
                    case DBMain.DbsWeLove.SQLServerClient:
                        s = "N'" + s + "'";
                        break;
                    default:
                        s = "'" + s + "'";
                        break;
                }
            }
            return s;
        }

        // fix this later
        public static string Value(string s, string t)
        {
            return Value(s, !string.IsNullOrEmpty(t));
        }

        // shortcut canonical date format, works with SQLite, might work with everything else
        public static string ConvertDateTime(DateTimeOffset dt)
        {
            return dt.ToString("o"); //dev note: figure out if this date format is cross-db provider usable
        }

        public static string getDate(DateTimeOffset dt)
        {
            return getDate(ConvertDateTime(dt));
        }

        public static string ConvertDateTime(DateTime dt)
        {
            return dt.ToString("s"); // .NET sortable date pattern "YYYY-MM-DDTHH:MM:SS.SSS");  // consider round-trip "o" too
        }

        public static string getDate(DateTime dt)
        {
            return getDate(ConvertDateTime(dt));
        }


        public static string getLastID(string table)
        {
            string sProvider = DBMain.ProviderInvariantName;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    return "SELECT last_insert_rowid()";
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64:
                    return "select @@Identity from " + table;
                case DBMain.DbsWeLove.Oracle:
                    return "DBMS_SQL.LAST_ROW_ID";
                case DBMain.DbsWeLove.SQLServerClient:
                    return "SELECT IDENT_CURRENT ('" + table + "') AS Current_Identity";
                case DBMain.DbsWeLove.SQLCE4:
                    return "SELECT @@IDENTITY AS " + table; //?? placeholder
                default:
                    return "";
            }
        }

        public static string getDate(string sDate) // sDate is in ISO format e.g. "2010-06-23T14:18:04.0000000+02:00"
        {
            string s;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    s= "'" + sDate + "'";
                    break;
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    s= "'#" + sDate + "#'";
                    break;
                case DBMain.DbsWeLove.Oracle:
                    s= "TO_DATE('" + Utils.ConverttoDateTimeString(sDate) + "','YYYYMMDD HH:MM:SS')";
                    break;
                case DBMain.DbsWeLove.SQLServerClient:
					s= "'" + sDate + "'";
                    break;
                case DBMain.DbsWeLove.SQLCE4:
                    s = "Cast('" + sDate + "' as datetime)";
                    break;
                default:
                    s= "'" + sDate + "'";
                    break;
            }
            return s;
        }

		public static void OpenConn(DbConnection conn)
		{
  			conn.Open();
			switch (DBMain.Provider)
            {
				case DBMain.DbsWeLove.SQLite:				
					DbCommand cmd = conn.CreateCommand();
					cmd.CommandText= ("PRAGMA foreign_keys=on;");
					int i = cmd.ExecuteNonQuery();
					break;
				default:
					break;
			}
		}
	}

}
