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
using System.Data.Common;
using NCCReporter;

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
                s = System.Text.RegularExpressions.Regex.Replace(s, @"[\'""]", @"$0$0");
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
            return SQLSpecific.Value(s, !String.IsNullOrEmpty(t));
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


        public static string getDateDiff(string date1, string date2, string unit)
        {
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite: // JFL TODO this is imprecise
                    if (unit.Equals("d")) unit = "";
                    if (unit.Equals("m")) unit = "/12";
                    if (unit.Equals("y")) unit = "/365";

                    return " (julianday(" + getDate(date1) + ") - julianday(" + getDate(date2) + ")) " + unit;
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    return "datediff('" + unit + "', " + getDate(date1) + ", " + getDate(date2) + ")";
                case DBMain.DbsWeLove.Oracle:
                    if (unit.Equals("d")) unit = "";
                    if (unit.Equals("m")) unit = "/12";
                    if (unit.Equals("y")) unit = "/365";
                    return "TRUNC(" + getDate(date1) + ") - TRUNC(" + getDate(date2) + ")" + unit;
                case DBMain.DbsWeLove.SQLServerClient:
                case DBMain.DbsWeLove.SQLCE4:
                    if (unit.Equals("d")) unit = "Day";
                    if (unit.Equals("m")) unit = "Month";
                    if (unit.Equals("y")) unit = "Year";
                    return "datediff(" + unit + "," + getDate(date1) + "," + getDate(date2) + ")";
                default:
                    return "";
            }
        }
        public static string getDateCompare(string field1, string field2)
        {
            string sProvider = DBMain.ProviderInvariantName;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    return "date(" + field1 + ") = date(" + field2 + ")";
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    return "DateValue(" + field1 + ") =DateValue(" + field2 + ")";
                case DBMain.DbsWeLove.Oracle:
                    return "Cast(" + field1 + "as Date) = cast(" + field2 + ")";
                case DBMain.DbsWeLove.SQLServerClient:
                case DBMain.DbsWeLove.SQLCE4:
                    return "Cast(" + field1 + "as Date) = cast(" + field2 + ")";
                default:
                    return "";
            }
        }
        public static string getConvertInt(string field)
        {
            string sProvider = DBMain.ProviderInvariantName;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    return "Cast(" + field + "as int)";
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    return "CInt(" + field + ")";
                case DBMain.DbsWeLove.Oracle:
                    return "Cast(" + field + "as int)";
                case DBMain.DbsWeLove.SQLServerClient:
                case DBMain.DbsWeLove.SQLCE4:
                    return "Cast(" + field + "as int)";
                default:
                    return "";
            }
        }
        public static string getNull(string field, string sNullValue)
        {
            string sProvider = DBMain.ProviderInvariantName;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    return "isnull(" + field + "," + sNullValue + ")";
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    return "iif(ISNULL(" + field + "), " + sNullValue + "," + field + ")";
                case DBMain.DbsWeLove.Oracle:
                    return "isnull(" + field + "," + sNullValue + ")";
                case DBMain.DbsWeLove.SQLServerClient:
                case DBMain.DbsWeLove.SQLCE4:
                    return "isnull(" + field + "," + sNullValue + ")";
                default:
                    return "";
            }

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
                    return "SELECT @@IDENTITY AS [Sheila]";
                default:
                    return "";
            }
        }

        public static string getCurrentDateTime()
        {
            string sProvider = DBMain.ProviderInvariantName;
            switch (DBMain.Provider)
            {
                case DBMain.DbsWeLove.SQLite:
                    return "datetime('now')";
                case DBMain.DbsWeLove.MDACJet:
                case DBMain.DbsWeLove.OleDB64: // todo: validate this is the same as Jet 4.0
                    return "Now()";
                case DBMain.DbsWeLove.Oracle:
                    return "SYSDATE()";
                case DBMain.DbsWeLove.SQLServerClient:
                case DBMain.DbsWeLove.SQLCE4:
                    return "GetDate()";
                default:
                    return System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString();
            }
        }

        public static string getDate(string sDate)
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
                case DBMain.DbsWeLove.SQLCE4:
                    s = "Cast('" + Utils.ConverttoDateTimeString(sDate) + "' as datetime)";
                    break;
                default:
                    s= "'" + sDate + "'";
                    break;
            }
            return s;
        }


        // todo: unused, but could be 
        public static DbConnection createDB()
        {
            string sProvider = DBMain.ProviderInvariantName;
            string sConnection = DBMain.ConnStr;

            if (sProvider.Equals("")) return null;
            DbProviderFactory fact = DbProviderFactories.GetFactory(sProvider);

            using (DbConnection cnn = fact.CreateConnection())
            {
                cnn.ConnectionString = sConnection;
                buildDB(cnn);
                return cnn;
            }
        }


        // devnote: is this needed. Better that all DB init, create, reset ops be performed outside the product
        public static bool buildDB(DbConnection sql_con)
        {
            sql_con.Open();
            DbCommand sql_cmd = sql_con.CreateCommand();

            //Build tables....
            using (DbTransaction mytransaction = sql_con.BeginTransaction())
            {
                try
                {
                    sql_cmd.ExecuteNonQuery();

                    mytransaction.Commit();
                    return true;
                }
                catch (Exception caught)
                {
                    mytransaction.Rollback();
                    Console.WriteLine(sql_cmd.CommandText);
                    try
                    {
                        DBMain.AltLog(LogLevels.Error, 70110, caught.Message + sql_cmd.CommandText);

                        sql_con.Close();
                        return false;
                    }
                    catch { return false; }
                }
            }
        }
    }

}
