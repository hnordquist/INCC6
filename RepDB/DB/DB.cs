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
// SQL Server CE 
// id integer PRIMARY KEY IDENTITY
// GO
// insert into Tool13 (Column) values('foo');

// SQLite3
// id integer PRIMARY KEY
// or sometimes this keyword is required, unsure why not always
// id integer PRIMARY KEY AUTOINCREMENT
// insert into Tool13 values('foo');

//
//     [temptid] int NOT NULL,
//     FOREIGN KEY(temptid) REFERENCES Tempted(id)
// or
//     [temptid] int REFERENCES Tempted(id),

using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using NCCReporter;
namespace DB
{

    /// <summary>
    /// Static class for single point access to the database 
    /// See DB.CreateConnection for typical use
    /// </summary>
    internal static class DBMain
    {
        // single point of access to get application global state 
        // e.g. app config values and logger handle
        // The hook to the outside context of the DB class 
        public static Persistence pest;
        private static LMLoggers.LognLM Logger
        {
            get { if (pest != null) return pest.logger; else return null; }
        }

        /// <summary>
        /// Logs a message to the database logger.
        /// If logger not defined, logs to the console.
        /// </summary>
        public static void AltLog(LogLevels eventType, int id, string message, bool forceconsole = false)
        {
            if (!forceconsole && Logger != null)
                Logger.TraceEvent(eventType, id, message);
            else
                Console.WriteLine(eventType.ToString() + " " + id + " " + LMLoggers.LognLM.FlattenChars(message));
        }

        /// <summary>
        /// Logs a message to the database logger.
        /// Composes the .ENT format string with the args to prepare the message string.
        /// If logger not defined, logs to the console.
        /// </summary>
        public static void AltLog(LogLevels eventType, int id, string format, params object[] args)
        {
            if (Logger != null)
                Logger.TraceEvent(eventType, id, format, args);
            else
                Console.WriteLine(eventType.ToString() + " " + id + " " + LMLoggers.LognLM.FlattenChars(String.Format(format, args)));
      }

        public enum DbsWeLove { 
            SQLite,
            SQLServerClient,
            SQLCE4, //SQL Server CE 4
            OleDB64, // for W 7 or x64, post Jet and MDAC
            MDACJet, // 32bit XP, uses Jet and MDAC, from ye olden days of yore
            SQLCE3_5, //SQL Server CE 3.5
            Oracle,  // dev note: Version specific?
            Nom
        };

        public static DbsWeLove Provider;

        static DbsWeLove ConvertProviderToEnum(string p)
        {
            DbsWeLove k = DbsWeLove.SQLite; ;
            switch (p.ToUpper())
            {
                case "SYSTEM.DATA.SQLITE":
                    k = DbsWeLove.SQLite;
                    break;
                case "SYSTEM.DATA.SQLCLIENT":
                    k = DbsWeLove.SQLServerClient;
                    break;
                case "SYSTEM.DATA.SQLSERVERCE.4.0":
                    k = DbsWeLove.SQLCE4;
                    break;
                case "MICROSOFT.JET.OLEDB.4.0":
                    k = DbsWeLove.MDACJet;
                    break;
                case "SYSTEM.DATA.OLEDB":   // dev note: unsure about this as a default
                case "MICROSOFT.ACE.OLEDB.12.0":
                    k = DbsWeLove.OleDB64;
                    break;
                case "MSDAORA":
                    k = DbsWeLove.Oracle;
                    break;
                case "SYSTEM.DATA.SQLSERVERCE.3.5":
                    k = DbsWeLove.SQLCE3_5;
                    break;
                default:
                    k = DbsWeLove.SQLite;
                    break;
            }
            return k;
        }
        // this is used once to create the DBProvider factory for subsequent use by all DB processing.
        public static string ProviderInvariantName
        {
            get {   if (pest != null)  return pest.cfg.MyProviderName; 
                    else return String.Empty; }
        }

        // a pass-thru method exposing the app-wide connection string from the app config state
        public static string ConnStr
        {
            // e.g. "Data Source=.\\Listmode.SQLite;Version=3;New=False;Compress=True;" 
            get { if (pest != null) return pest.cfg.MyDBConnectionString; else return String.Empty; }
        }

        // the factory created from the ProviderInvariantName
        // if this fails, all DB access fails, but the code will treat the state as if it has no memory, and use whatever exists or can be imported into the app from the outside.
        // in other words, the app can run despite no DB persistence 
        private static DbProviderFactory fact;
        private static DbCommandBuilder bld;
        public static DbProviderFactory DBProvider
        {
            get
            {
                if (fact == null)
                {
                    Provider = ConvertProviderToEnum(ProviderInvariantName);
                    // dev note: if this blows with System.Configuration.ConfigurationErrorsException, 
                    // check the .csproj hint file path for the referenced system.data provider, a relative path to the csproj loc may differ from the runtime loc 
                    // (e.g. bin/release), and the assembly will not be loaded. 
                    // dev note: another source of error can be the loc of the provider reference assembly, the loc must be available at runtime 
                    fact = DbProviderFactories.GetFactory(ProviderInvariantName);
					bld = fact.CreateCommandBuilder();
                }
                return fact;
            }
        }

		public static DbCommand DBCmd
        {
            get
            {
                if (fact == null)
                {
					DbProviderFactory f = DBProvider;
				}
				return fact.CreateCommand();
            }
        }

		public static DbCommandBuilder DBCmdBuilder
        {
            get
            {
                if (bld == null || fact == null)
                {
					DbProviderFactory f = DBProvider;
				}
                return bld;
            }
        }
        /// <summary>
        /// The current DBProvider factory creates the typed DBConnection subclass
        /// </summary>
        /// <returns>A new DBConnection implemented for the current DB provider</returns>
        public static DbConnection CreateConnection(bool useConnStr = false)
        {
            DbConnection conn =  DBProvider.CreateConnection();
            if (useConnStr) conn.ConnectionString = ConnStr; 
            return conn;
        }

        public static bool TryConnection(string ConxString, DbsWeLove type, string provider, string DBFileName)
        {
            DbProviderFactory fact = DbProviderFactories.GetFactory(provider);
            DbConnection conn = fact.CreateConnection();
            if (conn != null)
            {
                //Now, change persistence object for new DB.
                pest.cfg.MyDBConnectionString = ConxString;
                pest.cfg.MyProviderName = provider;
                conn.Close();
                return true;
            }
            return false;
        }
        public static bool SwitchDB(string dbfile)
        {
            DbsWeLove newDB = DbsWeLove.SQLite;
            string provider = String.Empty;

            switch (Path.GetExtension(dbfile))
            {
                case ".sqlite":
                    newDB = DbsWeLove.SQLite;
                    provider = "System.Data.SQLite";
                    break;
                //case ".sdf":
                //    newDB = DbsWeLove.SQLServerClient;
                //    provider = "System.Data.SqlClient";
                //    break;
                //case ".sqlce":
                //    newDB = DbsWeLove.SQLCE4;
                //    break;
                //case ".mdb":
                //    newDB = DbsWeLove.MDACJet;
               //     break;
                //todo: Figure out other db types.  Only do SQLite and SQL to begin with
                //case "SYSTEM.DATA.OLEDB":   // dev note: unsure about this as a default
                //case "MICROSOFT.ACE.OLEDB.12.0":
                //    k = DbsWeLove.OleDB64;
                //   break;
                //case "MSDAORA":
                //    k = DbsWeLove.Oracle;
                //    break;
                //case "SYSTEM.DATA.SQLSERVERCE.3.5":
                //    k = DbsWeLove.SQLCE3_5;
                //    break;
                default:
                    return false;
            }
            //Try to build new connection string and connect.
            DbConnectionStringBuilder csb = new DbConnectionStringBuilder();
            csb["Data Source"] = dbfile;
            csb["Version"]="3";
            csb["New"]="False";
            csb["Compress"]="True";
            csb["foreign keys"] = "true";
            return TryConnection(csb.ConnectionString, newDB, provider, dbfile);
        }
        public static string GetProviderStringFromEnum (DbsWeLove type)
        {
            string provider = String.Empty;
            switch (type)
            {
                case DbsWeLove.SQLite:
                    provider = "System.Data.SQLite";
                    break;
                case DbsWeLove.SQLServerClient:
                    provider = "System.Data.SqlClient";
                    break;
                case DbsWeLove.SQLCE4:
                    provider = "System.Data.SqlServerCe.4.0";
                    break;
                case DbsWeLove.MDACJet:
                    provider = "System.Data.OleDb";
                    break;
            }
            return provider;
        }
        /// <summary>
        /// The current DBProvider factory creates the typed DbDataAdapter subclass
        /// </summary>
        /// <returns>A new DbDataAdapter implemented for the current DB provider</returns>
        public static DbDataAdapter CreateDataAdapter()
        {
            return DBProvider.CreateDataAdapter();
        }

        /// <summary>
        /// Handle DB specific exceptions.
        /// Write errors to console only. 
        /// </summary>
        /// <param name="dbx">the exception</param>
        /// <param name="sql">the SQL statement text that threw the error</param>
        /// <returns>return true if exception is from the preferred provider and no database was found, so that caller may try to create a new database</returns>
        public static bool DBExceptionHandler(DbException dbx, string sql)
        {
            bool neednew = false;
            //if (dbx is System.Data.SqlServerCe.SqlCeException)  // SQL Server CE -- specific test against various SQL errors v. the no database found error
            //{
            //    System.Data.SqlServerCe.SqlCeException x = (System.Data.SqlServerCe.SqlCeException)dbx;
            //    //x.NativeError == 
            //    DBMain.AltLog(LogLevels.Warning, 70147, DBExceptionString(dbx, sql));
            //}
            //else
            if (dbx is System.Data.SQLite.SQLiteException)  // SQLite3 -- specific test against various SQL errors v. the no database found error
            {
                System.Data.SQLite.SQLiteException x = (System.Data.SQLite.SQLiteException)dbx;
                if (x.ResultCode == SQLiteErrorCode.Error) // SQL error or missing database
                {
                    neednew = !(dbx.Message.EndsWith("syntax error") || // not an SQL syntax error 
                                dbx.Message.Contains("no such table") ||//or malformed schema,
                                dbx.Message.Contains("has no column named")); // nor mismatched column, but likely a missing DB
                    if (!neednew)
                    {
                        DBMain.AltLog(LogLevels.Warning, 70136, DBExceptionString(dbx, sql), true);
                    }
                }
                else
                {
                    DBMain.AltLog(LogLevels.Warning, 70137, DBExceptionString(dbx, sql));
                }
            }
            else if (dbx is System.Data.OleDb.OleDbException)  // Access
            {
                DBMain.AltLog(LogLevels.Warning, 70140, DBExceptionString(dbx, sql));
                // todo: expand when the "no DB present" code is known
            }
            else if (dbx is System.Data.Common.DbException)  // anything else
            {
                DBMain.AltLog(LogLevels.Warning, 70139, DBExceptionString(dbx, sql));
                // todo: expand when the "no DB present" code is known
            }
            else
            {
                Console.WriteLine(DBExceptionString(dbx, sql));
            }
            return neednew;
        }

        public static string DBExceptionString(DbException dbx, string sql)
        {
            return LMLoggers.LognLM.FlattenChars(dbx.GetType().Name + "'" + dbx.Message + "' " + dbx.ErrorCode.ToString("x8") + "; " + sql);
        }
  


        //private const ushort creationTriesMax = 5;
        //private static bool onetimeretry = true;
        //private static ulong creationTries = 0;
        //private static object arbitrary = new object();
        //private static ManualResetEventSlim mres = new ManualResetEventSlim(true);
        /// <summary>
        /// Create a new empty database (SQLite3 only for now). Attempt to create five times before giving up.
        /// </summary>
        /// <param name="sqlretry">The failed query string , retry it once here after initial DB creation</param>
        public static void Creation(DbCommand sqlretry, bool scalar = false)
        {
            DBMain.AltLog(LogLevels.Warning, 70120, "New DB attempt placeholder, likely called incorrectly");
            return;
            /// JFL Oct. 28, 2013. The following code was meant to auto-create a database if one was not found, but I am abandoning this idea for now.

            //mres.Wait(10000);
            //lock (arbitrary)
            //{
            //    mres.Reset();
            //    creationTries++;
            //    if (creationTries > creationTriesMax)
            //        return;

            //    if (DBMain.ProviderInvariantName.Equals("")) return;

            //    // try to populate an empty database here
            //   DBMain.AltLog(LogLevels.Warning, 70119, "Creating new DB via " + ConnStr);

            //    Assembly exa = System.Reflection.Assembly.GetExecutingAssembly();
            //    ResourceManager rm = new ResourceManager("LMDB.Schema", exa);
            //    DbTransaction Trans = null;
            //    DbConnection sql_con = CreateConnection();
            //    sql_con.ConnectionString = ConnStr;
            //    try
            //    {
            //        string f2 = rm.GetString("DBCreationScript");   // what if it isnt there?

            //        sql_con.Open();
            //        Trans = sql_con.BeginTransaction();
            //        DbCommand sql_cmd = sql_con.CreateCommand();
            //        sql_cmd.CommandText = f2;
            //        if (sql_cmd.ExecuteNonQuery() < 0)
            //        {
            //            Trans.Rollback();
            //            sql_con.Close();
            //        }
            //        else
            //        {
            //            Trans.Commit();

            //            creationTries = creationTriesMax + 1;

            //            sql_con.Close();
            //           DBMain.AltLog(LogLevels.Warning, 70120, "New DB constructed");

            //            if (onetimeretry)
            //            {
            //               DBMain.AltLog(LogLevels.Warning, 70121, "Retrying " + sqlretry.CommandText);

            //                onetimeretry = false;
            //                if (scalar)
            //                    sqlretry.ExecuteScalar();
            //                else
            //                    sqlretry.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //    catch (Exception caught)
            //    {
            //       DBMain.AltLog(LogLevels.Warning, 70108, LMLoggers.LognLM.FlattenChars("Creation  '" + caught.Message + "' "));
            //        if (Trans != null) Trans.Rollback();
            //        sql_con.Close();
            //    }
            //}
        }


    }

    public interface IDB
    {
         bool Execute(string sSQL);
         DataTable DT(string sSQL);
         string Scalar(string sSQL);
         int ScalarIntx();
         int Execute(ArrayList sqlList);
         bool ExecuteTransaction(ArrayList sqlList);
         long ExecuteTransactionID(ArrayList sqlList);
         void SetConnection();
    }
    // the core class that connects to a DB
    public class DB : IDB
    {

        public DbConnection sql_con;
        public DbCommand sql_cmd;
        public DbDataAdapter sql_da;

        public DB(bool connect = true)
        {
            sql_con = null;
            if (connect)
                SetConnection(); // Now optional because if this throws, hard to recover in chain of const calls
        }

        public void SetConnection()
        {
            if (sql_con == null)
                try
                {
                    if (DBMain.ProviderInvariantName.Equals("")) return;
                    sql_con = DBMain.CreateConnection(useConnStr:true);
                    sql_da = DBMain.CreateDataAdapter();
                }
                catch (Exception caught)
                {
                   DBMain.AltLog(LogLevels.Error, 70100, "Unable to connect with database " + caught.Message);
                }
        }

		public void CreateCommand(string sSQL)
        {
			SetConnection();
            sql_cmd = sql_con.CreateCommand();
			sql_cmd.CommandText = sSQL;
        }

        // assumes DB is there, but might need to build it before we get here 
        public bool Execute(string sSQL)
        {

            bool needDb = false;
            try
            {
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
		sql_con.Open();
                try
                {
                    int i = sql_cmd.ExecuteNonQuery();
                }
                catch (System.Data.Common.DbException dbx)
                {
                    needDb = DBMain.DBExceptionHandler(dbx, sSQL);
                }
            }
            catch (Exception caught)
            {
                try
                {
                   DBMain.AltLog(LogLevels.Warning, 70101, "Execute '" + caught.Message + "' " + sSQL);
                    sql_con.Close();
                    return false;
                }
                catch { }
            }
            if (sql_con != null) sql_con.Close();
            if (needDb)
                DBMain.Creation(sql_cmd);  // create an new empty DB, pass the failed query string, retry it once after initial DB creation

            return true;
        }

        public DataTableReader DR(string sSQL)
        {
            DataTableReader DTR = new DataTableReader(DT(sSQL));
            return (DTR);
        }


        //Return a DataTable based on SQL query
        public DataTable DT(string sSQL)
        {
            //DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            try
            {
                sql_con.Open();

                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
                sql_da.SelectCommand = sql_cmd;
                //DS.Reset();
               int i = sql_da.Fill(DT);  // hangs here during transfer operations
                //DT = DS.Tables[0];
            }
            catch (Exception caught)
            {
                try
                {
                   DBMain.AltLog(LogLevels.Warning, 70103, "DT '" + caught.Message + "' " + sSQL);
                }
                catch { }
            }
            //Return DataTable
            if (sql_con != null) sql_con.Close();
            return (DT);
        }

        public string Scalar(string sSQL)
        {
            string sData = "";
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
                var o = sql_cmd.ExecuteScalar();
                if (o != null)
                    sData = o.ToString();
            }
            catch (System.Data.Common.DbException dbx)
            {
                DBMain.DBExceptionHandler(dbx, sSQL);
            }
            catch (Exception caught)
            {
                try
                {
                   DBMain.AltLog(LogLevels.Warning, 70104, "Scalar  '" + caught.Message + "' " + sSQL);
                    sql_con.Close();
                    return null;
                }
                catch { }
            }
            sql_con.Close();
            return sData;
        }

		public int ScalarIntx()
		{
			int iData = -1;
			try
			{
				sql_con.Open();
				var wht = sql_cmd.ExecuteScalar();
				if (wht == null)
					iData = 0;
				else
					iData = Convert.ToInt32(wht.ToString());
			} catch (Exception caught)
			{
				try
				{
					DBMain.AltLog(LogLevels.Warning, 70105, "ScalarIntx  '" + caught.Message + "' " + sql_cmd.CommandText);
					sql_con.Close();
					return -1;
				} catch { }
			}
			sql_con.Close();
			return iData;
        }

        public int Execute(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement

            string sRtn = "";
            Int32 result = -1; // what to return here?
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                bool needDb = false; bool scalar = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText = sqlList[i].ToString();
                        try
                        {
                            if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                            {
                                //Expect back a string
                                scalar = true;
                                sRtn = sql_cmd.ExecuteScalar().ToString();
                                if (sRtn.Equals(""))
                                {
                                    sql_con.Close();
                                    return -1;
                                }
                            }
                            else
                            {
                                scalar = false;
                                if (sql_cmd.CommandText.Contains("<Rtn>"))
                                {
                                    //substitute in sRtn Value
                                    sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                                }
                                //Expect back # rows modified
                                if (sql_cmd.ExecuteNonQuery() < 0)
                                {
                                    sql_con.Close();
                                    return -1;
                                }
                            }
                        }
                        catch (System.Data.Common.DbException dbx)
                        {
                            needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);
                        }
                        if (needDb)
                            DBMain.Creation(sql_cmd, scalar);  // create a new empty DB, pass the failed query string, retry it once after initial DB creation                       
                    }
                }
                sql_con.Close();

                Int32.TryParse(sRtn, out result);
                return result;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null) DBMain.AltLog(LogLevels.Warning, 70107, "Execute list  '" + caught.Message + "' " + sql_cmd);
                if (sql_con != null) sql_con.Close();
                return -1;
            }
        }
        public bool ExecuteTransaction(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement

            DbTransaction Trans = null;
            string sRtn = "";
            try
            {
                sql_con.Open();
                Trans = sql_con.BeginTransaction();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.Transaction = Trans;
                bool needDb = false; bool scalar = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    sql_cmd.CommandText = sqlList[i].ToString();
                    try
                    {
                        if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                        {
                            //Expect back a string
                            sRtn = sql_cmd.ExecuteScalar().ToString();
                            if (sRtn.Equals(""))
                            {
                                Trans.Rollback();
                                sql_con.Close();
                                return false;
                            }
                        }
                        else
                        {
                            if (sql_cmd.CommandText.Contains("<Rtn>"))
                            {
                                //substitute in sRtn Value
                                sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                            }

                            //Expect back # rows modified
                            if (sql_cmd.ExecuteNonQuery() < 0)
                            {
                                Trans.Rollback();
                                sql_con.Close();
                                return false;
                            }
                        }
                    }
                    catch (System.Data.Common.DbException dbx)
                    {
                        needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);
                    }
                    if (needDb)
                        DBMain.Creation(sql_cmd, scalar);  // create a new empty DB, pass the failed query string, retry it once after initial DB creation  
                }
                Trans.Commit();
                sql_con.Close();
                return true;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null)DBMain.AltLog(LogLevels.Warning, 70107, "ExecuteTransaction  '" + caught.Message + "' " + sql_cmd);
                if (Trans != null) Trans.Rollback();
                if (sql_con != null) sql_con.Close();
                return false;
            }
        }

        public long ExecuteTransactionID(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement
            DbTransaction Trans = null;
            string sRtn = "";
            Int32 result = -1; // what to return here?
            try
            {
                sql_con.Open();
                Trans = sql_con.BeginTransaction();

                sql_cmd = sql_con.CreateCommand();
                sql_cmd.Transaction = Trans;
                bool needDb = false; bool scalar = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText = sqlList[i].ToString();
                       //DBMain.AltLog(LogLevels.Info, 70115, "sql => " + sql_cmd.CommandText);
                        try
                        {
                            if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                            {
                                //Expect back a string
                                scalar = true;
                                sRtn = sql_cmd.ExecuteScalar().ToString();
                                if (sRtn.Equals(""))
                                {
                                    Trans.Rollback();
                                    sql_con.Close();
                                    return -1;
                                }

                            }
                            else
                            {
                                scalar = false;
                                if (sql_cmd.CommandText.Contains("<Rtn>"))
                                {
                                    //substitute in sRtn Value
                                    sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                                }
                                //Expect back # rows modified
                                if (sql_cmd.ExecuteNonQuery() < 0)
                                {
                                    Trans.Rollback();
                                    sql_con.Close();
                                    return -1;
                                }
                            }
                        }
                        catch (System.Data.Common.DbException dbx)
                        {
                            Trans.Rollback();
                            needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);
                            sql_con.Close();
                            return -1;

                        }
                        if (needDb)
                            DBMain.Creation(sql_cmd, scalar);  // create a new empty DB, pass the failed query string, retry it once after initial DB creation                       
                    }
                }
                Trans.Commit();
                sql_con.Close();

                Int32.TryParse(sRtn, out result);
                return result;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null) DBMain.AltLog(LogLevels.Warning, 70107, "ExecuteTransactionID  '" + caught.Message + "' " + sql_cmd);
                if (Trans != null) Trans.Rollback();
                if (sql_con != null) sql_con.Close();
                return -1;
            }
        }

		/// <summary>
		/// 6.0.1.0			db.TableHasColumn("LMINCCAppContext","dataFilePath");
		/// 6.0.1.1			db.TableHasColumn("composite_isotopics_rec","id");
		/// 6.0.1.2			new table composite_isotopic_rec
		/// </summary>
		/// <param name="table"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public bool TableHasColumn(string table, string col)
		{
			string sql = "select " + col + " from " + table;
			DataTable dt = DT(sql);
			DataRow r =  dt.Rows.Find(col);
			return dt.Rows.Count > 0;
		}
	}

    public class DB2 : IDB, IDisposable
    {

        public DbConnection sql_con;
        public DbCommand sql_cmd;
        public DbDataAdapter sql_da;

        public DB2(bool connect = true)
        {
            sql_con = null;
            if (connect)
                SetConnection(); // Now optional because if this throws, hard to recover in chain of const calls
        }

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{ 
					if (sql_con != null)
					{
						sql_con.Close();
						sql_con.Dispose();
						sql_con = null;
					}
					if (sql_cmd != null)
					{
						sql_cmd.Dispose();
						sql_cmd = null;
					}
					if (sql_da != null)
					{
						sql_da.Dispose();
						sql_da = null;
					}
				}
			} catch (Exception caught)
			{
				Console.WriteLine(caught.Message);
			}
		}
		public void Dispose()
        {
			Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetConnection()
        {
            if (sql_con == null)
                try
                {
                    if (DBMain.ProviderInvariantName.Equals("")) return;
                    sql_con = DBMain.CreateConnection(useConnStr: true);
                    sql_da = DBMain.CreateDataAdapter();
                    sql_con.Open();
                    sql_cmd = sql_con.CreateCommand();
                }
                catch (Exception caught)
                {
                    DBMain.AltLog(LogLevels.Error, 70100, "Unable to connect with database " + caught.Message);
                }
        }


        ///////////////////////
        public bool Execute(string sSQL)
        {

            bool needDb = false;
            try
            {
                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
                try
                {
                    sql_cmd.ExecuteNonQuery();
                }
                catch (System.Data.Common.DbException dbx)
                {
                    needDb = DBMain.DBExceptionHandler(dbx, sSQL);
                }
            }
            catch (Exception caught)
            {
                try
                {
                    DBMain.AltLog(LogLevels.Warning, 70101, "Execute '" + caught.Message + "' " + sSQL);

                    return false;
                }
                catch { }
            }
            return true;
        }

        //Return a DataTable based on SQL query
        public DataTable DT(string sSQL)
        {
            DataSet DS = new DataSet();
            DataTable DT = new DataTable();
            try
            {
                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();

                sql_cmd.CommandText = sSQL;
                sql_da.SelectCommand = sql_cmd;
                DS.Reset();
                sql_da.Fill(DS);  // hangs here during transfer operations
                DT = DS.Tables[0];
            }
            catch (Exception caught)
            {
                try
                {
                    DBMain.AltLog(LogLevels.Warning, 70103, "DT '" + caught.Message + "' " + sSQL);
                }
                catch { }
            }
            return (DT);
        }

        public string Scalar(string sSQL)
        {
            string sData = "";
            try
            {
                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
                var o = sql_cmd.ExecuteScalar();
                if (o != null)
                    sData = o.ToString();
            }
            catch (System.Data.Common.DbException dbx)
            {
                DBMain.DBExceptionHandler(dbx, sSQL);
            }
            catch (Exception caught)
            {
                try
                {
                    DBMain.AltLog(LogLevels.Warning, 70104, "Scalar  '" + caught.Message + "' " + sSQL);

                    return null;
                }
                catch { }
            }
            return sData;
        }

		public int ScalarIntx()
		{
			int iData = -1;
			try
			{
				sql_con.Open();
				var wht = sql_cmd.ExecuteScalar();
				if (wht == null)
					iData = 0;
				else
					iData = Convert.ToInt32(wht.ToString());
			} catch (Exception caught)
			{
				try
				{
					DBMain.AltLog(LogLevels.Warning, 70105, "ScalarIntx  '" + caught.Message + "' " + sql_cmd.CommandText);
					sql_con.Close();
					return -1;
				} catch { }
			}
			sql_con.Close();
			return iData;
        }

        public int Execute(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement

            string sRtn = "";
            Int32 result = -1; // what to return here?
            try
            {
                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();

                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText = sqlList[i].ToString();
                        try
                        {
                            if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                            {
                                //Expect back a string
                                sRtn = sql_cmd.ExecuteScalar().ToString();
                                if (sRtn.Equals(""))
                                {
                                    return -1;
                                }
                            }
                            else
                            {
                                if (sql_cmd.CommandText.Contains("<Rtn>"))
                                {
                                    //substitute in sRtn Value
                                    sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                                }
                                //Expect back # rows modified
                                if (sql_cmd.ExecuteNonQuery() < 0)
                                {
                                    return -1;
                                }
                            }
                        }
                        catch (System.Data.Common.DbException dbx)
                        {
                            needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);
                        }
                    }
                }

                Int32.TryParse(sRtn, out result);
                return result;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null) DBMain.AltLog(LogLevels.Warning, 70107, "Execute list  '" + caught.Message + "' " + sql_cmd);
                return -1;
            }
        }
        public bool ExecuteTransaction(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement

            DbTransaction Trans = null;
            string sRtn = "";
            try
            {
                Trans = sql_con.BeginTransaction();
                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();

                sql_cmd.Transaction = Trans;
                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    sql_cmd.CommandText = sqlList[i].ToString();
                    try
                    {
                        if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                        {
                            //Expect back a string
                            sRtn = sql_cmd.ExecuteScalar().ToString();
                            if (sRtn.Equals(""))
                            {
                                Trans.Rollback();

                                return false;
                            }
                        }
                        else
                        {
                            if (sql_cmd.CommandText.Contains("<Rtn>"))
                            {
                                //substitute in sRtn Value
                                sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                            }

                            //Expect back # rows modified
                            if (sql_cmd.ExecuteNonQuery() < 0)
                            {
                                Trans.Rollback();

                                return false;
                            }
                        }
                    }
                    catch (System.Data.Common.DbException dbx)
                    {
                        needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);
                    }
                }
                Trans.Commit();

                return true;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null) DBMain.AltLog(LogLevels.Warning, 70107, "ExecuteTransaction  '" + caught.Message + "' " + sql_cmd);
                if (Trans != null) Trans.Rollback();
                return false;
            }
        }

        public long ExecuteTransactionID(ArrayList sqlList)
        {
            //Execute Sequence of Sql Statements
            //Check Sql Type to determine if we have return value to replace in next statement
            DbTransaction Trans = null;
            string sRtn = "";
            Int32 result = -1; // what to return here?
            try
            {

                Trans = sql_con.BeginTransaction();

                if (sql_cmd == null) sql_cmd = sql_con.CreateCommand();

                sql_cmd.Transaction = Trans;
                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText = sqlList[i].ToString();
                        //DBMain.AltLog(LogLevels.Info, 70115, "sql => " + sql_cmd.CommandText);
                        try
                        {
                            if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                            {
                                //Expect back a string
                                sRtn = sql_cmd.ExecuteScalar().ToString();
                                if (sRtn.Equals(""))
                                {
                                    Trans.Rollback();
                                    return -1;
                                }

                            }
                            else
                            {
                                if (sql_cmd.CommandText.Contains("<Rtn>"))
                                {
                                    //substitute in sRtn Value
                                    sql_cmd.CommandText = sql_cmd.CommandText.Replace("<Rtn>", sRtn);
                                }
                                //Expect back # rows modified
                                if (sql_cmd.ExecuteNonQuery() < 0)
                                {
                                    Trans.Rollback();

                                    return -1;
                                }
                            }
                        }
                        catch (System.Data.Common.DbException dbx)
                        {
                            Trans.Rollback();
                            needDb = DBMain.DBExceptionHandler(dbx, sql_cmd.CommandText);

                            return -1;

                        }
                    }
                }
                Trans.Commit();
                Int32.TryParse(sRtn, out result);
                return result;
            }
            catch (Exception caught)
            {
                if (sql_cmd != null) DBMain.AltLog(LogLevels.Warning, 70107, "ExecuteTransactionID '" + caught.Message + "' " + sql_cmd);
                if (Trans != null) Trans.Rollback();
                return -1;
            }
        }

        //public class Enclosure : IDisposable
        //{
        //}

    }

}
