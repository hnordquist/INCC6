/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Data.Common;
using System.Diagnostics;


namespace DB
{
    // devnote: need to add a graduated shut-down when 1) DB is not there, 2) error messages are duplicates and 3) overall performance is too slow.
    
    // an instance of this class is operable if a DB has been configured and is available via the global DBMain static class
    public class DBListener : TraceListener
    {
        DbConnection sql_con;
        DbCommand sql_cmd;
        //string sSQL;
        string current;
        int previous;
        ulong count;
        ulong connAttempts;
        bool header;


        public DBListener()
        {
            current = String.Empty;
            header = true;
            count = connAttempts = 0;
            //sSQL = "INSERT INTO notification_log (msg, ts) VALUES ('{0}',{1})"; // 2nd param is a date native function inlined
        }

        protected bool CombineHeaderAndBody(string message)
        {
            bool writeNow = false;
            if (header)
            {
                current = message;
            }
            else
            {
                current = current + message;
                writeNow = true;
            }
            header = !header; //toggle
            return writeNow; // if true then write on next step
        }

        //
        // The header is logged first then a second call has the message body
        // Helpfully both appear in one single entry in the DB, so we cache the header and append the body and then write it.
        //
        public override void Write(string message)
        {
            bool writeNow = CombineHeaderAndBody(message);
            if (!writeNow)
                return;

            if (current.GetHashCode() != previous)
            {
                previous = current.GetHashCode();
                count = 0;
            }
            else
                count++;
            string s = String.Empty;
            if (count > 10)
            {
                if (count == 10)
                    s = "Message repeated . . .";
                else if ((count % 1000) == 0)
                    s = "Message repeated 1000 times . . .";
                else
                    return;
            }			
        }
        public override void WriteLine(string message)
        {
            if (ExternalPrefix(message) && header) // expecting a header, but not getting one, so make one
            {
                Write("Ext ");
            }
            Write(message);
        }
        static string[] prefixes = { "DB", "Control", "Data","Analysis","App" ,"Net","LMComm"};
        bool ExternalPrefix(string message)
        {
            bool res = true;
            foreach (string s in prefixes)
            {
                if (message.StartsWith(s))
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

        public override void Close()
        {
            try
            {
                if (sql_con == null)
                {
                    sql_con.Close();
                }
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
            }
            base.Close();
        }

        bool Prep()
        {
            bool res = false;
            if (sql_con == null)
            {
                if (SetConnection())
                {
                    if (sql_con != null)
                    {
		                SQLSpecific.OpenConn(sql_con);
                        sql_cmd = sql_con.CreateCommand();
                        res = true;
                    }
                }
            }
            else
                res = sql_con.State == ConnectionState.Open;

            return res;
        }

        protected new void Dispose()
        {
            try
            {
                if (sql_con != null)
                {
                    sql_con.Close();
                }
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
            }
            base.Dispose();
        }

        bool SetConnection()
        {
            if (connAttempts > 25)
                if ((connAttempts % 250) != 0)
                    return false;

            // retry 25 times, then every 250 attempts
            try
            {
                if (DBMain.ProviderInvariantName.Equals("")) return false;
                sql_con = DBMain.CreateConnection(useConnStr: true);
                sql_con.ConnectionString = DBMain.ConnStr;
                return true;
            }
            catch (Exception caught)
            {
                connAttempts++;
                Console.WriteLine(caught.Message);
            }
            return false;
        }
    }
}
