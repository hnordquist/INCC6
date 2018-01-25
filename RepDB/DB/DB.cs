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
    public static class DBMain
    {
        // single point of access to get application global state 
        // e.g. app config values and logger handle
        // The hook to the outside context of the DB class 
        public static Persistence pest;
<<<<<<< HEAD

        public static bool ConsoleQuiet;
=======
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0

        /// <summary>
        /// Logs a message to the now defunct database logger.
        /// Now just logs to the console.
        /// </summary>
        public static void AltLog(LogLevels eventType, int id, string message, bool forceconsole = false)
        {
<<<<<<< HEAD
            if (ConsoleQuiet)
                return;
            Console.WriteLine(eventType.ToString() + " " + id + " " + Log.FlattenChars(message));
=======
                Console.WriteLine(eventType.ToString() + " " + id + " " + LMLoggers.LognLM.FlattenChars(message));
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
        }

        /// <summary>
        /// Logs a message to the database logger.
        /// Composes the .NET format string with the args to prepare the message string.
        /// If logger not defined, logs to the console.
        /// </summary>
        public static void AltLog(LogLevels eventType, int id, string format, params object[] args)
        {
<<<<<<< HEAD
            if (ConsoleQuiet)
                return;
            Console.WriteLine(eventType.ToString() + " " + id + " " + Log.FlattenChars(string.Format(format, args)));
=======
                Console.WriteLine(eventType.ToString() + " " + id + " " + LMLoggers.LognLM.FlattenChars(string.Format(format, args)));
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
      }

        public enum DbsWeLove { 
            SQLite,
            SQLServerClient,
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
                    else return string.Empty; }
        }

        // a pass-thru method exposing the app-wide connection string from the app config state
        public static string ConnStr
        {
            // e.g. "Data Source=.\\Listmode.SQLite;Version=3;New=False;Compress=True;Foreign_Keys=On;" 
            get { if (pest != null) return pest.cfg.MyDBConnectionString; else return string.Empty; }
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
            if (dbx is SQLiteException)  // SQLite3 -- specific test against various SQL errors v. the no database found error
            {
				SQLiteException x = (SQLiteException)dbx;
                if (x.ResultCode == SQLiteErrorCode.Error) // SQL error or missing database
                {
                    neednew = !(dbx.Message.EndsWith("syntax error") || // not an SQL syntax error 
                                dbx.Message.Contains("no such table") ||//or malformed schema,
                                dbx.Message.Contains("has no column named")); // nor mismatched column, but likely a missing DB
                    if (!neednew)
                    {
						AltLog(LogLevels.Warning, 70136, DBExceptionString(dbx, sql), true);
                    }
                }
                else
                {
					AltLog(LogLevels.Warning, 70137, DBExceptionString(dbx, sql));
                }
            }
            else if (dbx is System.Data.OleDb.OleDbException)  // Access
            {
				AltLog(LogLevels.Warning, 70140, DBExceptionString(dbx, sql));
                // todo: expand when the "no DB present" code is known
            }
            else if (dbx is DbException)  // anything else
            {
				AltLog(LogLevels.Warning, 70139, DBExceptionString(dbx, sql));
                // todo: expand when the "no DB present" code is known
            }
            else
            {
                if (!ConsoleQuiet)
                    Console.WriteLine(DBExceptionString(dbx, sql));
            }
            return neednew;
        }

        public static string DBExceptionString(DbException dbx, string sql)
        {
            return Log.FlattenChars(dbx.GetType().Name + "'" + dbx.Message + "' " + dbx.ErrorCode.ToString("x8") + "; " + sql);
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
			sql_cmd.CommandText =  sSQL;
        }

        // assumes DB is there, but might need to build it before we get here 
        public bool Execute(string sSQL)
        {

            bool needDb = false;
            try
            {
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText =  sSQL;
                SQLSpecific.OpenConn(sql_con);
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
            DataTable DT = new DataTable();
            try
            {
                SQLSpecific.OpenConn(sql_con);

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

		public DataTable DBProbe(string sSQL)
        {
            DataTable DT = new DataTable();
            try
            {
                SQLSpecific.OpenConn(sql_con);
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = sSQL;
                sql_da.SelectCommand = sql_cmd;
                int i = sql_da.Fill(DT);
				string s = "Database:" + sql_con.Database + ", DataSource:" + sql_con.DataSource.ToString() + ", ServerVersion:" + sql_con.ServerVersion;
				DBDescStr = sql_da.GetType().FullName + "; " + s + " (" + sql_con.ConnectionString + ")";
            }
            catch (Exception caught)
            {
				DT = null;
				DBErrorStr = caught.Message;
                DBMain.AltLog(LogLevels.Warning, 70193, "DBProbe '" + caught.Message + "'");
			}
            if (sql_con != null) sql_con.Close();
            return (DT);
        }
		public string DBErrorStr;
		public string DBDescStr;

        public string Scalar(string sSQL)
        {
            string sData = "";
            try
            {
                SQLSpecific.OpenConn(sql_con);
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText =  sSQL;
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
                SQLSpecific.OpenConn(sql_con);
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
                SQLSpecific.OpenConn(sql_con);
                sql_cmd = sql_con.CreateCommand();
                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText =  sqlList[i].ToString();
                        try
                        {
                            if (sqlList[i].ToString().ToUpper().StartsWith("SELECT"))
                            {
                                //Expect back a string
                                sRtn = sql_cmd.ExecuteScalar().ToString();
                                if (sRtn.Equals(""))
                                {
                                    sql_con.Close();
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
                                    sql_con.Close();
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
                SQLSpecific.OpenConn(sql_con);
                Trans = sql_con.BeginTransaction();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.Transaction = Trans;
                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    sql_cmd.CommandText =  sqlList[i].ToString();
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
                SQLSpecific.OpenConn(sql_con);
                Trans = sql_con.BeginTransaction();

                sql_cmd = sql_con.CreateCommand();
                sql_cmd.Transaction = Trans;
                bool needDb = false;
                for (int i = 0; i < sqlList.Count; i++)
                {
                    if (!sqlList[i].ToString().Equals(""))
                    {
                        sql_cmd.CommandText =  sqlList[i].ToString();
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
                                    sql_con.Close();
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
        /// 6.0.1.0		db.TableHasColumn("LMINCCAppContext","dataFilePath");
        /// 6.0.1.1		db.TableHasColumn("composite_isotopics_rec","id");
        /// 6.0.1.2		new table composite_isotopic_rec, mod cm_pu_ratio_rec
        /// 6.0.1.3		normalizing SQLite 3 and SQL Server 2012 schemas, mod results_curium_ratio_rec and cm_pu_ratio_rec
        /// 6.0.1.12	new tables collar_detector_rec_m and collar_k5_rec_m (for results), type name change: ntext to nvarchar
        /// 6.0.1.12	added aux_method to analysis_method_rec table (Jul 14, 2016)
        /// 6.0.16      added LMMultiplicity_m, CountingParams_m tables, keys for same orig, lmid FK field to Cycles for keeping VSR results, Unnormalized accidentals distros for VSR results 
        /// 6.17        added lmfilename to cycles; new tables CyclesMult, CyclesFeyn, CyclesTIR, CyclesCoin for LM analysis results
        /// </summary>
        /// <param name="table"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool TableHasColumn(string table, string col)
		{
			string sql = "select " + col + " from " + table;
			DataTable dt = DBProbe(sql);
			return dt != null && dt.Columns.Contains(col);
			//return dt.Rows.Count > 0;
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
                DBMain.AltLog(LogLevels.Error, 70130, caught.Message);
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
					SQLSpecific.OpenConn(sql_con);
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
                sql_cmd.CommandText =  sSQL;
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
                    DBMain.AltLog(LogLevels.Warning, 78103, "DT '" + caught.Message + "' " + sSQL);
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
                SQLSpecific.OpenConn(sql_con);
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
                    sql_cmd.CommandText =  sqlList[i].ToString();
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
                        sql_cmd.CommandText =  sqlList[i].ToString();
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

    }

}
