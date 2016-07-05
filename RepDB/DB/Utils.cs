/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Collections.Generic;
using System.Text;
namespace DB
{
    public enum ParamType { Bytes, String, Boolean, Double, UInt8, UInt16, UInt32, UInt64, Int8, Int16, Int32, Int64, DT, TS, DTOffset };

    public class Element
    {
        public Element()
        {
        }
        public Element(string Name, string Value, bool Quote)
        {
            this.Name = Name;
            this.Value = Value;
            this.Quote = Quote;
            //this.PType = ParamType.String;
        }
        public Element(string Name, string Value, int PType)
        {
            this.Name = Name;
            this.Value = Value;
            this.PType = (ParamType)PType;
            ByteRep = true;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Quote { get; set; }

        // quote status based on actual internal type
        public bool NewQuote { get { return PType == ParamType.String || PType == ParamType.DTOffset || PType == ParamType.DT | PType == ParamType.TS; } }

        // todo: subclass this later, just get it to work now
        public ParamType PType { get; set; }
        public bool ByteRep { get; set; }

        public string QValue
        {
            get {
                if (Quote)
                {
                    return SQLSpecific.Value(Value,true);
                }
                else if (ByteRep)
                {
                    //byte[] b = Utils.AsBytes(Value);
                    //string s = Utils.StrFromBytes(b);
                    //return "'" + s + "'";
                    return "'" + Value + "'";
                }
                else
                    return Value;
            }
        }


        public Element(Element src)
        {
            Name = String.Copy(src.Name); Value = String.Copy(src.Value); Quote = src.Quote;
        }

        public Element(string NewName, Element src)
        {
            Name = String.Copy(NewName); Value = String.Copy(src.Value); Quote = src.Quote;
        }
        public Element(string Name, DateTime dt)
        {
            this.Name = String.Copy(Name); Value = SQLSpecific.getDate(dt); Quote = false;
        }
        public Element(string Name, DateTimeOffset dto)
        {
            this.Name = String.Copy(Name); Value = SQLSpecific.getDate(dto); Quote = false;
        }
        public Element(string Name, string Value)
        {
            this.Name = String.Copy(Name); this.Value = Value; Quote = true;
        }
        public Element(string Name, bool Value)
        {
            this.Name = String.Copy(Name); this.Value = (Value ? "1" : "0");
        }

        public Element(string Name, double Value)
        {
            this.Name = String.Copy(Name); this.Value = NaNFilter(Value);
        }
        public Element(string Name, Int32 Value)
        {
            this.Name = String.Copy(Name); this.Value = Value.ToString();
        }
        public Element(string Name, UInt64 Value)
        {
            this.Name = String.Copy(Name); this.Value = Value.ToString();
        }
        public Element(string Name, string[] Value)
        {
            this.Name = String.Copy(Name); this.Value = Utils.Stringify(Value); Quote = true;
        }
        //public Element(string Name, double[] Value)
        //{
        //    this.Name = String.Copy(Name); this.Bytes = Utils.ToBytes(Value); IsBytes = true;
        //}
        //public Element(string Name, long[] Value)
        //{
        //    this.Name = String.Copy(Name); this.Bytes = Utils.ToBytes(Value); IsBytes = true;
        //}
        //public Element(string Name, ulong[] Value)
        //{
        //    this.Name = String.Copy(Name); this.Bytes = Utils.ToBytes(Value); IsBytes = true;
        //}
        //public Element(string Name, int[] Value)
        //{
        //    this.Name = String.Copy(Name); this.Bytes = Utils.ToBytes(Value); 
        //}
        //public Element(string Name, bool[] Value)
        //{
        //    this.Name = String.Copy(Name); this.Bytes = Utils.ToBytes(Value); 
        //}

        // dev note: could use Double.Epsilon as a signal value for this condition, then NaN can be written to DB and restored on read
        public static string NaNFilter(double d)
        {
            if (Double.IsNaN(d))
            {
                return Double.Epsilon.ToString();
            }
            else
            {
                return d.ToString();
            }
        }


    }

    public class ElementList: List<Element>
    {
        public ElementList()
        {
        }
        public string OptTable { get; set;  }

		public string ColumnsValues
        {
            get
            {
                StringBuilder cv = new StringBuilder();
                cv.Append('(');
                foreach (Element e in this)
                {
                    cv.Append(e.Name); cv.Append(',');
                }
                cv.Remove(cv.Length - 1, 1);
                cv.Append(") Values(");
                foreach (Element e in this)
                {
                    cv.Append(e.QValue); cv.Append(',');
                }
                cv.Remove(cv.Length - 1, 1);
                cv.Append(')');
                return cv.ToString();
            }
        }

        public string ColumnEqValueList {
            get
            {
                StringBuilder cv = new StringBuilder();
                foreach (Element e in this)
                {
                    cv.Append(e.Name);
                    cv.Append('=');
                    cv.Append(e.QValue);
                    cv.Append(',');
                }
                cv.Remove(cv.Length - 1, 1);
                return cv.ToString();
            }
        }

        public string ColumnEqValueAndList
        {
            get
            {
                StringBuilder cv = new StringBuilder();
                foreach (Element e in this)
                {
                    cv.Append(e.Name);
                    cv.Append('=');
                    cv.Append(e.QValue);
                    cv.Append(", AND ");
                }
                cv.Remove(cv.Length - 6, 6);
                return cv.ToString();
            }
        }
    }

    public static class SqliteCB
    {

        //SqliteCommand = ElementList

    }
    

    public static class  Utils
    {
        public static UInt64 DBUInt64(object o)
        {
            UInt64 d = 0;
            UInt64.TryParse(o.ToString(), out d);
            return d;
        }
        public static UInt64 DBUInt64(string s)
        {
            UInt64 d = 0;
            UInt64.TryParse(s, out d);
            return d;
        }
        public static Int64 DBInt64(object o)
        {
            Int64 d = 0;
            Int64.TryParse(o.ToString(), out d);
            return d;
        }
        public static Int64 DBInt64(string s)
        {
            Int64 d = 0;
            Int64.TryParse(s, out d);
            return d;
        }
        public static Double DBDouble(object o)
        {
            return DBDouble(o.ToString());
        }
        public static Double DBDouble(string s)
        {
            Double d = 0;
            Double.TryParse(s, out d);
            if (d == Double.Epsilon)
                return Double.NaN;
            else
                return d;
        }
      static   public Int32 DBInt32(object o)
        {
            Int32 i = 0;
            Int32.TryParse(o.ToString(), out i);
            return i;
        }
        public static Int32 DBInt32(string s)
        {
            Int32 i = 0;
            Int32.TryParse(s, out i);
            return i;
        }
        public static UInt32 DBUInt32(object o)
        {
            return DBUInt32(o.ToString());
        }
        public static UInt32 DBUInt32(string s)
        {
            UInt32 i = 0;
            UInt32.TryParse(s, out i);
            return i;
        }
        public static Int16 DBInt16(object o)
        {
            return DBInt16(o.ToString());
        }
        public static Int16 DBInt16(string s)
        {
            Int16 i = 0;
            Int16.TryParse(s, out i);
            return i;
        }

        public static UInt16 DBUInt16(string s)
        {
            UInt16 i = 0;
            UInt16.TryParse(s, out i);
            return i;
        }
        public static UInt16 DBUInt16(object o)
        {
            return DBUInt16(o.ToString());
        }
        public static bool DBBool(object o)
        {
            return DBBool(o.ToString());
        }
        public static bool DBBool(string s)
        {
            bool b;
            if (!bool.TryParse(s, out b))
                if ("1".CompareTo(s) == 0) b = true;
            return b;
        }
        public static DateTime DBDateTime(object o)
        {
            DateTime d;
            DateTime.TryParse(o.ToString(), out d);
            return d;
        }
        public static DateTime DBDateTime(string s)
        {
            DateTime d;
            DateTime.TryParse(s, out d);
            return d;
        }
        public static DateTimeOffset DBDateTimeOffset(object o)
        {
            DateTimeOffset d;
            DateTimeOffset.TryParse(o.ToString(), out d);
            return d;
        }
        public static DateTimeOffset DBDateTimeOffset(string s)
        {
            DateTimeOffset d;
            DateTimeOffset.TryParse(s, out d);
            return d;
        }

        public static object ConverttoDateTime(string sValue)
        {
            //Return DateTime value or DBNull
            System.DateTime Value;
            try
            {
                Value = Convert.ToDateTime(sValue);
                if (Value.TimeOfDay.ToString().Equals("12:00:00 AM")) //fix SQL Issue
                    Value = Convert.ToDateTime(Value.Date.ToShortDateString() + " 00:00:01");
                return Value;
            }
            catch { return DBNull.Value; }
        }
        public static string ConverttoDateTimeString(string sValue)
        {
            //Return DateTime value or DBNull
            System.DateTime Value;
            try
            {
                Value = Convert.ToDateTime(sValue);
                if (Value.TimeOfDay.ToString().Equals("12:00:00 AM") || Value.TimeOfDay.ToString().Equals("00:00:00")) //fix SQL Issue
                {
                    return Value.Date.ToShortDateString() + " 00:00:01";
                }
                else //remove any AM or PM 
                {
                    return Value.Date.ToShortDateString() + Value.ToString(" HH:mm:ss");
                }
            }
            catch { return ""; }
        }

        // saved as 'Ticks' (a long) in DB 
        static public TimeSpan DBTimeSpan(object o)
        {
            long ticks = 0;
            long.TryParse(o.ToString(), out ticks);
            return new TimeSpan(ticks);
        }

        public static double[] ReifyDoubles(string doubles)
        {
            string[] s = doubles.Split(',');
            double[] res = new double[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                double.TryParse(s[i], out res[i]);
            }
            return res;
        }
        public static Int64[] ReifyInt64s(string Int64s)
        {
            string[] s = Int64s.Split(',');
            Int64[] res = new Int64[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                Int64.TryParse(s[i], out res[i]);
            }
            return res;
        }
        public static UInt64[] ReifyUInt64s(string UInt64s)
        {
            string[] s = UInt64s.Split(',');
            UInt64[] res = new UInt64[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                UInt64.TryParse(s[i], out res[i]);
            }
            return res;
        }
        public static Int32[] ReifyInt32s(string Int32s)
        {
            string[] s = Int32s.Split(',');
            Int32[] res = new Int32[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                Int32.TryParse(s[i], out res[i]);
            }
            return res;
        }

        public static bool[] ReifyBools(string bools)
        {
            string[] s = bools.Split(',');
            bool[] res = new bool[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bool.TryParse(s[i], out res[i]);
            }
            return res;
        }

        public static string[] ReifyStrings(string strings)
        {
            string[] res = strings.Split(',');
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = res[i].Trim('"');
            }
            return res;
        }

        public static string Stringify(bool[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder(a.Length * 2);
            foreach (bool b in a)
            {
                s.Append(b ? '1' : '0');
                s.Append(',');
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        public static string Stringify(double[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder(a.Length * (2 + 14));
            foreach (double d in a)
            {
                s.Append(d);
                s.Append(',');
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        public static string Stringify(ulong[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder(a.Length * (2 + 14));
            foreach (ulong ul in a)
            {
                s.Append(ul);
                s.Append(',');
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        public static string Stringify(long[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder(a.Length * (2 + 14));
            foreach (long l in a)
            {
                s.Append(l);
                s.Append(',');
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        public static string Stringify(string[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder();
            foreach (string ls in a)
            {
                s.Append('"');
                s.Append(@""",");  // TODO: works with SQLite ,but dunno with others yet
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        public static string Stringify(int[] a)
        {
            if (a.Length < 1)
                return String.Empty;
            StringBuilder s = new StringBuilder();
            foreach (int i in a)
            {
                s.Append(i);
                s.Append(',');
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
        

        // dev note: may not ever need these unfinished encodings for use in blobs.
        // The max string list of doubles (0,1, 2.3,  456.7890123456789)... is (1028*16) + 1028 (1028 16 char doubles, with comma seps)
        // The max string list of UInt64 (0,1, -2, 18446744073709551615)... is (1028*20) + 1028 (1028 20 char UInt64s, with comma seps)
        // The max string list of Int64  (0,1, -2, -9223372036854775808)... is (1028*20) + 1028 (1028 20 char Int64s, with comma seps)
        // 21588 unicode chars for SQL CE and SQLite implies 43176 bytes long, 
        // The SQLite text/blob/string max is 5x10^8 chars or 1,000,000,000 bytes by default, no issue there
        // The SQL CE 4 NTEXT limit is 2^30-2/2 chars or 2^30 -2 bytes, no issue there
        // todo: determine max value and type for SQL Server (NTEXT?)
        public static byte[] AsBytes(string s)
        {
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static byte[] ToBytes(bool[] bools)
        {
            Encoding unicode = Encoding.Unicode;
            string s = Stringify(bools);

            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static bool[] BoolsFromBytes(byte[] bools)
        {
            String decodedString = StrFromBytes(bools);
            return ReifyBools(decodedString);
        }

        public static byte[] ToBytes(double[] dbls)
        {
            Encoding unicode = Encoding.Unicode;
            string s = Stringify(dbls);

            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static double[] DoublesFromBytes(byte[] dbls)
        {
            String decodedString = StrFromBytes(dbls);
            return ReifyDoubles(decodedString);
        }

        public static byte[] ToBytes(int[] ints)
        {
            Encoding unicode = Encoding.Unicode;
            string s = Stringify(ints);

            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static int[] IntsFromBytes(byte[] ints)
        {
            String decodedString = StrFromBytes(ints);
            return ReifyInt32s(decodedString);
        }

        public static byte[] ToBytes(ulong[] ulongs)
        {
            Encoding unicode = Encoding.Unicode;
            string s = Stringify(ulongs);

            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static ulong[] ULongsFromBytes(byte[] ulongs)
        {
            String decodedString = StrFromBytes(ulongs);
            return ReifyUInt64s(decodedString);
        }

        public static byte[] ToBytes(long[] longs)
        {
            Encoding unicode = Encoding.Unicode;
            string s = Stringify(longs);

            byte[] unicodeBytes = unicode.GetBytes(s);
            return unicodeBytes;
        }

        public static long[] LongsFromBytes(byte[] longs)
        {
            String decodedString = StrFromBytes(longs);
            return ReifyInt64s(decodedString);
        }

        public static string StrFromBytes(byte[] dbls)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            String decodedString = utf8.GetString(dbls);
            return decodedString;
        }

    }
}
