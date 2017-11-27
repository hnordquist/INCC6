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
using System.IO;
using System.Runtime.InteropServices;

namespace NCCTransfer
{

    /// <summary>
    /// Byte-oriented utilities for working with INCC5 binary data files
    /// </summary>
    public static class TransferUtils
    {

        public class TransferParsingException : Exception
        {
            public TransferParsingException()
            {
            }

            public TransferParsingException(string message)
                : base(message)
            {
            }
        }

        static public bool ByteBool(byte b)
        {
            return (b == 0 ? false : true);
        }

        //
        // Summary:
        //     Reads a 2-byte unsigned integer from the current stream and advances the current
        //     position of the stream by two bytes.
        //
        // Returns:
        //     A 2-byte unsigned integer read from the current stream.
        //
        // Exceptions:
        //   TransferUtils.TransferParsingException
        //     On any exception raised internally
        static public UInt16 ReadUInt16(BinaryReader reader, string msg)
        {
            UInt16 r = 0;
            try
            {
                r = reader.ReadUInt16();
            }
            catch (Exception e)
            {
                throw new TransferParsingException("reading ushort for " + msg + ": " + e.Message);
            }
            return r;
        }

        //
        // Summary:
        //     Reads a 2-byte signed integer from the current stream and advances the current
        //     position of the stream by two bytes.
        //
        // Returns:
        //     A 2-byte signed integer read from the current stream.
        //
        // Exceptions:
        //   TransferUtils.TransferParsingException
        //     On any exception raised internally
        static public Int16 ReadInt16(BinaryReader reader, string msg)
        {
            Int16 r = 0;
            try
            {
                r = reader.ReadInt16();
            }
            catch (Exception e)
            {
                throw new TransferParsingException("reading short for " + msg + ": " + e.Message);
            }
            return r;
        }

        //
        // Summary:
        //     Reads count bytes from the current stream and advances the current
        //     position of the stream by count bytes.
        //
        // Parameters:
        //   count:
        //     The number of bytes to read.
        //   msg:
        //     A string to add to the exception message if needed
        //
        // Returns:
        //     A byte array containing data read from the underlying stream. This might
        //     be less than the number of bytes requested if the end of the stream is reached
        //
        // Exceptions:
        //   TransferUtils.TransferParsingException
        //     On any exception raised internally
        static public byte[] ReadBytes(BinaryReader reader, int count, string msg)
        {
            byte[] r = null;
            try
            {
                r = reader.ReadBytes(count);
            }
            catch (Exception e)
            {
                throw new TransferParsingException(msg + " read failed: " + e.Message);
            }

            return r;
        }

        static public byte[] TryReadBytes(BinaryReader reader, int count)
        {
            byte[] r = null;
            try
            {
                r = reader.ReadBytes(count);
            }
            catch (Exception e)
            {
                throw new TransferParsingException(" read " + count.ToString() + " bytes failed: " + e.Message);
            }

            return r;
        }
        static public byte ReadByte(BinaryReader reader, string msg)
        {
            byte r = 0;
            try
            {
                r = reader.ReadByte();
            }
            catch (Exception e)
            {
                throw new TransferParsingException(msg + " read failed: " + e.Message);
            }

            return r;
        }


        static unsafe public double[] Copy(double* ptr, int len)
        {

            double[] doubles = new double[len];

            Marshal.Copy(new IntPtr(ptr), doubles, 0, len);
            return doubles;
        }

		static unsafe public void Copy(ref double[] doubles, double* ptr, int len)
        {
            Marshal.Copy(new IntPtr(ptr), doubles, 0, len);
        }

        static unsafe public bool[] Copy(int* ptr, int len)
        {

            bool[] bools = new bool[len];
            for (int i = 0; i < len; i++)
                bools[i] = (ptr[i] == 0 ? false : true);
            return bools;
        }
        static unsafe public bool[] Copy(byte* ptr, int len)
        {

            bool[] bools = new bool[len];
            for (int i = 0; i < len; i++)
                bools[i] = (ptr[i] == 0 ? false : true);
            return bools;
        }
        static unsafe public ulong[] multarrayxfer(double* ptr, int maxsize)
        {
            int len = maxsize;
            int i = maxsize;
            for (i = maxsize - 1; i >= 0; i--)
            {
                if (ptr[i] != 0.0)
                    break;
            }
            // i is new len

            ulong[] ulongs = new ulong[i + 1];
            for (int j = 0; j <= i; j++)
                ulongs[j] = (ulong)ptr[j];

            return ulongs;
        }

        static unsafe public string str(byte* ptr, int len)
        {

            byte[] bytes = new byte[len];

            Marshal.Copy(new IntPtr(ptr), bytes, 0, len);
            int idx = Array.FindIndex(bytes, (b) => (b.CompareTo(0) == 0));  // finds the first 0x00 in the converted string
            if (idx > -1)
                len = idx;

            string s = System.Text.Encoding.ASCII.GetString(bytes, 0, len);

            return s;
        }

        static unsafe public void PassPack(byte* b)
        {
            b[0] = 0x50; // " P A S S \0"; LOL
            b[1] = 0x41;
            b[2] = 0x53;
            b[3] = 0x53;
            b[4] = 0x00;
        }
        static unsafe public void FailPack(byte* b)
        {
            b[0] = 0x46; // " F A I L \0"; EPIC LOL
            b[1] = 0x41;
            b[2] = 0x49;
            b[3] = 0x4C;
            b[4] = 0x00;
        }
        static unsafe public bool PassCheck(byte* b)
        {
            bool res = (b[0] == 0x70) || (b[0] == 0x50) ? true : false; // a 'p' or a 'P' for Pass? 0x50/0x70
            return res;
        }

        static  public bool ByteEquals(byte[] a, byte[] b, int len)
        {
            int i;
            for (i = 0; i < len; i++)
                if (a[i] != b[i])
                    break;

            return i == len;
        }

		static unsafe public byte[] GetBytes(byte* ptr, int len)
		{
            if (ptr == null)
            {
                throw new ArgumentException();
            }
			byte[] res = new byte[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = *ptr;
                ptr += 1;
            }
			return res;
		}
        static unsafe public void Copy(byte[] src, byte* dst)
        {
            if (src == null || dst == null)
            {
                throw new ArgumentException();
            }

            // The following fixed statement pins the location of the src and dst objects
            // in memory so that they will not be moved by garbage collection.
            //fixed (byte* pSrc = src, pDst = dst)
            {
                byte* pd = dst;

                // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
                for (int i = 0; i < src.Length; i++)
                {
                    *((byte*)pd) = src[i];
                    pd += 1;
                }

            }
        }
		static unsafe public void CopyBoolsToBytes(bool[] src, byte* dst)
        {
			byte[] x = new byte[src.Length];
			for (int i = 0; i < src.Length; i++)
				x[i] = (byte)(src[i] ? 1: 0);
			Copy(x, dst);
		}
		static unsafe public void CopyBoolsToInts(bool[] src, int* dst)
        {
			int[] x = new int[src.Length];
			for (int i = 0; i < src.Length; i++)
				x[i] = (src[i] ? 1: 0);
			CopyInts(x, dst);
		}
		static unsafe public void CopyInts(int[] src, int* dst)
        {
            if (src == null || dst == null)
                throw new ArgumentException();

                byte* pd = (byte*)dst;

                // Loop over the count in blocks of 4 bytes, copying a int (4 bytes) at a time:
                for (int i = 0; i < src.Length; i++)
                {
					byte[] bx = BitConverter.GetBytes(src[i]);
					for (int j = 0; j < bx.Length; j++)
					{
						*(pd) = bx[j];
						pd += 1;
					}
                }
        }

		static unsafe public void CopyULongsToDbls(ulong[] src, double* dst)
        {
            if (src == null || dst == null)
            {
                throw new ArgumentException();
            }

            {
                byte* pd = (byte*)dst;

                // Loop over the count in blocks of 8 bytes (ulong), copying ulong to a double (8 bytes) 1 byte at a time:
                for (int i = 0; i < src.Length; i++)
                {
					byte[] bx = BitConverter.GetBytes(src[i]);
					for (int j = 0; j < bx.Length; j++)
					{
						*((byte*)pd) = bx[j];
						pd += 1;
					}
                }

            }
        }

		static unsafe public void CopyDbls(double[] src, double* dst)
        {
            if (src == null || dst == null)
            {
                throw new ArgumentException();
            }

            // The following fixed statement pins the location of the src and dst objects
            // in memory so that they will not be moved by garbage collection.
            //fixed (byte* pSrc = src, pDst = dst)
            {
                byte* pd = (byte*)dst;

                // Loop over the count in blocks of 8 bytes, copying a double (8 bytes) at a time:
                for (int i = 0; i < src.Length; i++)
                {
					byte[] bx = BitConverter.GetBytes(src[i]);
					for (int j = 0; j < bx.Length; j++)
					{
						*((byte*)pd) = bx[j];
						pd += 1;
					}
                }

            }
        }

        static unsafe public void Copy(byte* src, int srcIndex, byte* dst, int dstIndex, int count)
        {
            if (src == null || srcIndex < 0 ||
                dst == null || dstIndex < 0 || count < 0)
            {
                throw new ArgumentException();
            }

            // The following fixed statement pins the location of the src and dst objects
            // in memory so that they will not be moved by garbage collection.
            //fixed (byte* pSrc = src, pDst = dst)
            {
                byte* ps = src; 
                byte* pd = dst;

                // Loop over the count in bytes:
                for (int i = 0; i < count; i++)
                {
                    *((byte*)pd) = *((byte*)ps);
                    pd += 1;
                    ps += 1;
                }

            }
        }

        static unsafe public void Copy(byte[] src, int srcIndex, byte* dst, int dstIndex, int count)
        {
            if (src == null || srcIndex < 0 ||
                dst == null || dstIndex < 0 || count < 0)
            {
                throw new ArgumentException();
            }

            {
                byte* pd = dst;

                // Loop over the count in bytes:
                for (int i = 0; i < count; i++)
                {
                    *((byte*)pd) = src[i];
                    pd += 1;
                }

            }
        }
    }

    /// <summary>
    /// INCC5 binary compatible structures and utilities for use with the
    /// INCC5 transfer and initial data file import feature of INCC6. 
    /// These definitions are direct copies of the 2002 INCC5 database schema used by INCC 5.0.2 up through 5.1.2.5.
    /// The 2002 INCC5 database schema has not changed as of late 2015.
    /// </summary>
    public static class INCC
    {

        public static readonly DateTime ZeroIAEATime = new DateTime(1952, 1, 1);


        public static class Gen32
        {

            /*-----------------------------------------------------------------------------
            *  Define symbolically the various date-time formats.
            *---------------------------------------------------------------------------*/

            /* ------------------------------------------------
             *  9/5/89 - jap
             *  the following date-time formats have been replaced
             *  with the addition of the new date-time functions
             *  written by SK.  -1 through 5 are the same -- 6
             *  through 9 have been added.	See below.
             *
             *  #define DTF_NONE		   -1
             *  #define DTF_C		    0
             *  #define DTF_DEC		    1
             *  #define DTF_WFD		    2
             *  #define DTF_US		    3
             *  #define DTF_IAEA		    4
             *  #define DTF_MCA35		    5
             *
             * ------------------------------------------------ */

            /* ------------------------------------------------------------------
             *  Structures needed for handling time and date
             * ----------------------------------------------------------------*/

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            unsafe struct GEN_TIME_STRUCT
            {
                int hr;
                int min;
                int sec;
                int msec;
            };

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            unsafe struct GEN_DATE_STRUCT
            {
                int yr;
                int mon;
                int day;
            };

            /*-----------------------------------------------------------------------------
             *  Define symbolically the various date-time formats.
             *  Eventually gen will be cleaned up and these will combine with the DTF
             *  already in gen.
             *---------------------------------------------------------------------------*/

            static public int GEN_DTF_NONE = -1;
            //static public int GEN_DTF_C = 0;
            static public int GEN_DTF_DEC = 1;
            static public int GEN_DTF_WFD = 2;
            static public int GEN_DTF_US = 3;
            static public int GEN_DTF_IAEA = 4;
            static public int GEN_DTF_MCA35 = 5;
            //static public int GEN_DTF_EURO = 6;
            //static public int GEN_DTF_HM = 7;
            //static public int GEN_DTF_HMS = 8;
            //static public int GEN_DTF_HMSM = 9;

            /* number of days since gen library time zero (Jan. 1, 1952) to
               MS-DOS time zero (Jan 1, 1970) */

            //static double GEN_DOS_TIME_ZERO_DAYS = 6575.0;

            /* seconds per day */

            // static double SECONDS_PER_DAY = 86400.0;

            /* type of sort done by gen_get_filenames() */

            //     static int GEN_SORT_ALPHA = 0;
            //   static int GEN_SORT_ASC_DATE = 1;
            //     static int GEN_SORT_DES_DATE = 2;

            /* allocate max # strings possible */
            //    static int GEN_MAX_STR_ALLOC = -1;

            /* max length of a GEN error msg */
            //    static int GEN_MAX_MSG_LEN = 80;


            /*  Library status codes */

            static int GEN_SUCCESS = 0;
            //static int GEN_INVALID_DATE = 1;
            //static int GEN_INVALID_DATE_FORMAT = 2;
            //static int GEN_INVALID_TIME = 3;
            //static int GEN_INVALID_TIME_FORMAT = 4;
            //static int GEN_HEAP_ERROR = 5;
            static int GEN_INVALID_YEAR = 6;
            //static int GEN_FILE_OPEN_FAILURE = 7;
            //static int GEN_NO_FILE_NAME_MATCH = 8;
            //static int GEN_INVALID_DRIVE = 9;
            //static int GEN_FILE_WRITE_FAILURE = 10;

            /*==============================================================================
             *
             *  Name / File:	gen_date_time_str_to_seconds()	    CONV_D_T.C
             *
             *  Purpose:	 converts date and time to seconds from Jan 1 1952
             *
             *  Return value:    GEN_SUCCESS - success calculation
             *		     GEN_INVALID_YEAR - unsuccessful calculation
             *
             *  Author:           Edward A. Kern
             *
             *  Revision History:
             *  date / author          revision
             *  -----------------      --------
             *  11-Jun-1988  EAK	   Created
             *  04-Apr-1990  WCH	   Modified - changed name,
             *			   	      changed base date to Jan 1, 1952,
             *				      changed long to unsigned long.
             *  03-Oct-1990  WCH	   Modified - changed return values
             *  03-Jun-1993  WCH	   Modified - if date = 00.00.00 return 0 time
             *
            ==============================================================================*/

            /* <t>CONV_D_T.C   CONV_D_T.C	CONV_D_T.C   */
            /* <s>gen_date_time_str_to_seconds()   gen_date_time_str_to_seconds()	*/

            static uint REF_TIME = 0;		/* seconds at Jan 1, 1952 */
            static uint SECS_PER_YEAR = 31536000;	/* seconds in 365 day year */
            static uint SECS_PER_DAY = 86400;		/* seconds in one day */

            static int[] month_day = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


            /*
            ==============================================================================
             *
             *  Name / File:  gen_unpack_date()  /  UNP_DATE.C
             *
             *  Purpose:  unpacks the date string "mm/dd/yy" into the corresponding
             *            int integer day, month, and four digit year.  for example
             *            09/24/86 would return day=24, month=9, and year = 1986.
             *
             *  Return value:
             *
             *  Error returns:
             *
             *  Special notes:
             *
             *  Implicit Inputs:
             *
             *  Implicit Outputs:
             *
             *  Calls:
             *
             *           Revision History:
             *  date / author                revision
             *  -------------                --------
             *  02/14/88  Ed Kern            Created
             *  07/01/91  WCH		 Modified - use GEN_DTF_US and GEN_DTF_IAEA
             *  07/07/93  WCH		 Made to work correctly when year reaches 2000
             *  10/23/95  WCH		 Allow .-/ as delimiters for any date format
             *
            ==============================================================================
             */
            /* <t> gen_unpack_date()  gen_unpack_date()  gen_unpack_date()  */
            static void gen_unpack_date(string datestr,                   /* input: date string */
                                ref int p_day,
                                ref int p_month,
                        ref int p_year,
                        int iaea_date)
            {

                int i = 0;

                string[] dateparts = datestr.Split(new char[] { '.', '-', '/' }, 3);

                if (iaea_date != GEN_DTF_US)
                {
                    /* parse out the year and convert to four_digit year */
                    if (dateparts.Length > 0 && Int32.TryParse(dateparts[0], out i))
                    {
                        if (i < 52)
                            p_year = i + 2000;
                        else
                            p_year = i + 1900;
                    }

                    /* parse out the month */
                    if (dateparts.Length > 1 && Int32.TryParse(dateparts[1], out i))
                    {
                        p_month = i;
                    }

                    /* parse out the day */
                    if (dateparts.Length > 2 && Int32.TryParse(dateparts[2], out i))
                    {
                        p_day = i;
                    }
                }
                else
                {
                    /* parse out the month */
                    if (dateparts.Length > 0 && Int32.TryParse(dateparts[0], out i))
                    {
                        p_month = i;
                    }
                    /* parse out the day of the month */
                    if (dateparts.Length > 1 && Int32.TryParse(dateparts[1], out i))
                    {
                        p_day = i;
                    }
                    /* parse out the year and  convert to four-digit year */

                    if (dateparts.Length > 2 && Int32.TryParse(dateparts[2], out i))
                    {
                        if (i < 52)
                            p_year = i + 2000;
                        else
                            p_year = i + 1900;
                    }
                }
            }

            /*
            ==============================================================================
             *
             *  Name / File:  gen_unpack_time()  /  UNP_TIME.C
             *
             *  Purpose:  unpacks the time string "hh:mm:ss" into the corresponding
             *            int integer hours, minutes, and seconds.
             *
             *  Return value:
             *
             *  Error returns:
             *
             *  Special notes:
             *
             *  Implicit Inputs:
             *
             *  Implicit Outputs:
             *
             *  Calls:
             *
             *           Revision History:
             *  date / author                revision
             *  -------------                --------
             *  02/14/88  Ed Kern            Created
             *
            ==============================================================================
             */
            /* <t> gen_unpack_time()  gen_unpack_time()  gen_unpack_time()  */


            static void gen_unpack_time(string timestr,                   /* input: date string */
                                ref int p_hour,
                                ref int p_minute,
                                ref int p_second)
            {

                int i = 0;

                string[] timeparts = timestr.Split(new char[] { ':', '/' }, 3);

                /* parse out the hour */
                if (timeparts.Length > 0 && Int32.TryParse(timeparts[0], out i))
                {
                    p_hour = i;
                }

                /* parse out the minute */
                if (timeparts.Length > 1 && Int32.TryParse(timeparts[1], out i))
                {
                    p_minute = i;
                }

                /* parse out the second */
                if (timeparts.Length > 2 && Int32.TryParse(timeparts[2], out i))
                {
                    p_second = i;
                }
            }


            internal static int gen_date_time_str_to_seconds(
                ref uint ptr_total_seconds,   /* output: seconds from Jan 1, 1952 */
                string date, 		        /* input:  date string (IAEA or US) */
                string time,       			/* input: time string */
                int iaea_format) 			/* input:  IAEA or USA date format */
            {
                int year = 0;
                int month = 0;
                int day = 0;
                int hour = 0;
                int minute = 0;
                int second = 0;
                int status = GEN_INVALID_YEAR;
                int year_remainder;
                int days_in_year;
                int days_in_month = 0;
                int i;
                uint total_seconds;


                string[] dateparts = date.Split(new char[] { '.', '-', '/' }, 3);
                if (dateparts.Length == 3 && dateparts[0].Equals("00") && dateparts[1].Equals("00") && dateparts[2].Equals("00"))
                {
                    ptr_total_seconds = 0;
                    status = GEN_SUCCESS;
                    return status;
                }

                gen_unpack_date(date, ref day, ref month, ref year, iaea_format);
                gen_unpack_time(time, ref hour, ref minute, ref second);
                total_seconds = REF_TIME;
                if (year < 1952)
                    year += 100;
                year_remainder = year - 1952;
                if (year_remainder >= 0)
                {
                    status = GEN_SUCCESS;
                    while (year_remainder != 0)
                    {
                        total_seconds += SECS_PER_YEAR;
                        if (((year_remainder - 1) % 4) == 0)
                            total_seconds += SECS_PER_DAY;	    /* account for leap year */
                        year_remainder--;
                    }
                    days_in_year = 0;
                    for (i = 0; i < month - 1; i++)
                    {
                        days_in_month = month_day[i];
                        if (i == 1 && (year % 4) == 0)
                            days_in_month = 29;		     /* February of leap year */
                        days_in_year += days_in_month;
                    }
                    days_in_year += day - 1;
                    total_seconds += (uint)days_in_year * SECS_PER_DAY;
                    total_seconds += (uint)(hour * 3600 + minute * 60 + second);
                    ptr_total_seconds = total_seconds;
                }
                return status;
            }


            /*=============================================================================
            *  Name/File:       gen_date_time			datetime.c
            *
            *  Purpose:         Extract the current date and time and return them
            *                   in the standard VMS format.
            *
            *  Return value:    GEN_SUCCESS
            *
            *  Implicit Input:  type definitions in time.h
            *  Implicit Output: none
            *  Errors:          none
            *
            *  Calls:           Standard C library functions.
            *
            *  Special note:    
            *
            *  Author:          Thomas A. Kelley,  C-3
            *
            *  Revision History:
            *  date / author	revision
            *  ------------------	--------
            *  May, 1988 / TAK	Created.
            *  Sept 1989	JAP	changed DTF_'s to GEN_DTF_'s
            *===========================================================================*/

            // dev note: GEN_DTF_IAEA is the only param used now
            public static int gen_date_time(
           int format,       /* input, code for the type of date-time format	     */
           ref string ptr_string) /* output, pointer to string containing date & time    */
            {

                string date;
                //int i;
                //string month;
                DateTime now = DateTime.Now;
                // char temp;

                /*-----------------------------------------------------------------------------
                 *  If a null format is specifically requested, return a null string.
                 *---------------------------------------------------------------------------*/

                if (format == GEN_DTF_NONE)
                {
                    date = string.Empty;
                }

                else
                {

                    /*-----------------------------------------------------------------------------
                     *  Use the C library calls to get the current date and time.
                     *  An example of the format is  "Wed Jun 19 12:05:34 1999"
                     *---------------------------------------------------------------------------*/

                    //  strcpy (date, ctime (&now));
                    //  date[24] = EOS_CHAR;
                    //
                    date = now.ToString();
                    if (format == GEN_DTF_DEC)
                    {

                        /*-----------------------------------------------------------------------------
                         *  Reorder the ouput character string into the DEC format.
                         *  An example of the format is  "19-Jun-1999 12:05:34"
                         *---------------------------------------------------------------------------*/

                        date = now.ToString("dd-MMM-yyy HH:mm:ss");
                    }

                    else if (format == GEN_DTF_WFD)
                    {

                        /*-----------------------------------------------------------------------------
                         *  Reorder the output character string into the Windows for Data format.
                         *  An example of the format is  "99/06/19 12:05:34"
                         *---------------------------------------------------------------------------*/

                        date = now.ToString("yy/MM/dd HH:mm:ss");
                    }

                    else if (format == GEN_DTF_IAEA)
                    {

                        /*-----------------------------------------------------------------------------
                         *  Reorder the output character string into the I. A. E. A. format.
                         *  An example of the format is  "99.06.19 12:05:34"
                         *---------------------------------------------------------------------------*/
                        date = now.ToString("yy.MM.dd HH:mm:ss");

                    }

                    else if (format == GEN_DTF_US)
                    {

                        /*-----------------------------------------------------------------------------
                         *  Reorder the output character string into the US format.
                         *  An example of the format is  "06/19/99 12:05:34"
                         *---------------------------------------------------------------------------*/

                        date = now.ToString("MM/dd/yy HH:mm:ss");

                    }

                    else if (format == GEN_DTF_MCA35)
                    {

                        /*-----------------------------------------------------------------------------
                         *  Reorder the output character string for setting the date and time in
                         *  a Canberra MCA-35.
                         *  An example of the format is  "06; 19; 1999 12; 05; 34"
                         *---------------------------------------------------------------------------*/
                        date = now.ToString("MM; dd; yyyy HH; mm; ss");
                    }

                }

                ptr_string = date;

                return (GEN_SUCCESS);
            }
        }


        static public DateTime DateTimeFrom(string date, string time)
        {
            uint seconds_since_1952 = 0;
            int x = INCC.Gen32.gen_date_time_str_to_seconds(ref seconds_since_1952, date, time, INCC.Gen32.GEN_DTF_IAEA);
            // x should be GEN_SUCCESS (0)

            DateTime dt = ZeroIAEATime.AddSeconds(seconds_since_1952);
            return dt;

        }
        static public DateTime DateFrom(string date)
        {
            uint seconds_since_1952 = 0;
            int x = INCC.Gen32.gen_date_time_str_to_seconds(ref seconds_since_1952, date, "", INCC.Gen32.GEN_DTF_IAEA);
            // x should be GEN_SUCCESS (0)

            DateTime dt = ZeroIAEATime.AddSeconds(seconds_since_1952);
            return dt;

        }

        public static readonly byte[] INTEGRATED_REVIEW = new byte[] { 0x49, 0x52, 0x45, 0x56 }; // "IREV"
        public static readonly byte[] OLD_REVIEW = new byte[] { 0x52, 0x41, 0x57 }; // "RAW" 

        /* this relates only to the Rad Review .NCC binary file definition,  a series of these records */
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct raw_data_rec
	    {
            public fixed byte  date[8];
            public fixed byte  time[8];
            public UInt16 meas_time;
            public double totals;
            public double r_plus_a;
            public double a;
            public UInt16 meas_time2;
            public double totals2;
	    };
        /* structure of a run record from integrated review */
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct integrated_review_data_rec
        {
            public fixed byte  date[8];
            public fixed byte  time[8];
            public UInt16 meas_time;
            public double totals;
            public double r_plus_a;
            public double a;
            public double scaler1;
            public double scaler2;
            public UInt16 n_mult;
        };

        [Flags]
        public enum SaveResultsMask
        {
            SAVE_INIT_SRC_RESULTS = 0x1,
            SAVE_BIAS_RESULTS = 0x2,
            SAVE_PRECISION_RESULTS = 0x4,
            SAVE_CAL_CURVE_RESULTS = 0x8,
            SAVE_KNOWN_ALPHA_RESULTS = 0x10,
            SAVE_KNOWN_M_RESULTS = 0x20,
            SAVE_MULTIPLICITY_RESULTS = 0x40,
            SAVE_ACTIVE_PASSIVE_RESULTS = 0x80,
            SAVE_ACTIVE_RESULTS = 0x100,
            SAVE_ADD_A_SOURCE_RESULTS = 0x200,
            SAVE_ACTIVE_MULTIPLICITY_RESULTS = 0x400,
            SAVE_COLLAR_RESULTS = 0x800,
            SAVE_CURIUM_RATIO_RESULTS = 0x1000,
            SAVE_TRUNCATED_MULT_RESULTS = 0x2000,
            SAVE_TRUNCATED_MULT_BKG_RESULTS = 0x4000,
            SAVE_DUAL_ENERGY_MULT_RESULTS = 0x8000
        }

        public const int CONVENTIONAL_MULT = 0;		// 1st mult radio button
        public const int MULT_SOLVE_EFFICIENCY = 1;		// 2nd mult radio button
        public const int MULT_DUAL_ENERGY_MODE = 2;	// 3rd mult radio button
        public const int MULT_KNOWN_ALPHA = 3;		// 4th mult radio button
        public const int CONVENTIONAL_MULT_WEIGHTED = 4;	// 5th mult radio button

        /* Measurement Analysis Methods */
        public const int METHOD_CALCURVE = 0;	// Passive Calibration Curve
        public const int METHOD_AKNOWN = 1;	// Known Alpha
        public const int METHOD_MKNOWN = 2;	// Known M
        public const int METHOD_MULT = 3;// Multiplicity
        public const int METHOD_ADDASRC = 4;	// Add-a-source
        public const int METHOD_ACTIVE = 5;	// Active Calibration Curve
        public const int METHOD_ACTIVE_MULT = 6;	// Active Multiplicity
        public const int METHOD_ACTPAS = 7;	// Active/Passive
        public const int METHOD_COLLAR = 8;	// Collar
        public const int METHOD_NONE = 9;	// No analysis method selected
        public const int METHOD_CURIUM_RATIO = 10;	// Curium Ratio
        public const int METHOD_TRUNCATED_MULT = 11;	// Truncated multiplicity


        /* part of multiplicity, but separate identifier needed for save/restore */
        public const int DUAL_ENERGY_MULT_SAVE_RESTORE = 99;	// Dual energy multiplicity

        /* Save/restore collar calibration that is independent of detector */
        public const int COLLAR_SAVE_RESTORE = 100;
        /* Save/restore collar calibration that is dependent on detector */
        public const int COLLAR_DETECTOR_SAVE_RESTORE = 101;
        /* Save/restore collar K5 calibration that is independent of detector */
        public const int COLLAR_K5_SAVE_RESTORE = 102;
        // token used by the calibration parameter save and restore mess
        public const int WMV_CALIB_TOKEN = 0xFF;

        /* Measurement Options */
        public const int OPTION_RATES_ONLY = 0;// Rates Only
        public const int OPTION_BKG = 1;	// Background
        public const int OPTION_INIT_SRC = 2;	// Initial Source
        public const int OPTION_BIAS = 3;// MC Bias
        public const int OPTION_PREC = 4;	// MC Precision
        public const int OPTION_ASSAY = 5;	// Assay
        public const int OPTION_CALIBRATION = 6;// Calibration
        public const int OPTION_HOLDUP = 7;	// Holdup
        public const int OPTION_HIGH_VOLTAGE = 10;// High voltage plateau

        public const int PASS_FAIL_LENGTH = 5; /*length of Pass/Fail string */
        public const int MAX_RUN_TESTS_LENGTH = 21; /*max length of a run tests string */
        public const int NUM_ERROR_MSG_CODES = 10; /*max number of error msg codes */
        public const int NUM_WARNING_MSG_CODES = 10; /*max number of warning msg codes */
        public const int MAX_DETECTORS = 100; /*max number of detectors */
        public const int MAX_ITEM_TYPES = 100; /*max number of item types */
        public const int MAX_FACILITIES = 50; /*max number of facilities */
        public const int MAX_MBAS = 50; /*max number of mbas */
        public const int MAX_STRATUM_IDS = 100; /*max number of stratum ids */
        public const int MAX_CAMPAIGN_IDS = 100; /*max number of campaigns */
        public const int MAX_GLOVEBOX_IDS = 100; /*max # glovebox ids */
        public const int MAX_ITEM_IDS = 392; /*max # item ids */
        public const int MAX_ISOTOPICS_IDS = 1000; /*max number of isotopic ids */
        public const int MAX_ISO_COMPS = 20; /*max number isotopic compositions */
        public const int MAX_COLLAR_DATA_SETS = 100; /*max number collar data sets */
        public const int MAX_COLLAR_K5_PARAMETERS = 20; /*max number collar K = 5 parameters */
        public const int MAX_K5_LABEL_LENGTH = 31; /*max length K = 5 label */
        public const int MAX_INVENTORY_CHG_CODES = 31; /*max number inventory change codes */
        public const int MAX_IO_CODES = 28; /*max number I/O codes */
        public const int MAX_POISON_ROD_TYPES = 10; /*max # different types poison rods */
        public const int DATE_TIME_LENGTH = 9; /*length of a date or time string */
        public const int FILE_NAME_LENGTH = 13; /*length file name including suffix */
        public const int CHAR_FIELD_LENGTH = 41; /*length of database strings */
        public const int LONG_FILE_NAME_LENGTH = 256; /*length long file name including suffix */
        public const int MAX_PATH_LENGTH = LONG_FILE_NAME_LENGTH; /*length of file name path including drive and suffix */
        public const int MAX_COMMENT_LENGTH = 51; /*length of measurement comments */
        public const int ERR_MSG_LENGTH = 81; /*length of database error msg */
        public const int MULTI_ARRAY_SIZE = 128; /*size of multiplicity arrays */ // INCC5 handles 128 only, this INCC5 legacy code must remain at 128
        public const int MAX_NUM_CALIB_PTS = 20; /*max # calibration pts pairs in db */
        public const int MAX_ASSAY_SUMMARY_VARS = 130; /*max # assay summary variables */
        public const int MAX_HOLDUP_SUMMARY_VARS = 50; /*max # holdup summary variables */
        public const int SOURCE_ID_LENGTH = 13; /*max length of ref Cf = 252/AmLi source id */
        public const int INVENTORY_CHG_LENGTH = 3; /*length of inventory change code */
        public const int IO_CODE_LENGTH = 2; /*length of I/O code */
        public const int MBA_LENGTH = 5; /*length of an MBA code */
        public const int ISO_SOURCE_CODE_LENGTH = 3; /*length of an isotopics source code */
        public const int FACILITY_LENGTH = 13; /*length of a facility name */
        public const int MAX_ITEM_TYPE_LENGTH = 6; /*length of an item type name string */
        public const int MAX_ITEM_ID_LENGTH = 13;
        public const int MAX_CAMPAIGN_ID_LENGTH = 13;
        public const int MAX_GLOVEBOX_ID_LENGTH = 21;
        public const int MAX_STRATUM_ID_LENGTH = 13;
        public const int MAX_ISOTOPICS_ID_LENGTH = 13;
        public const int MAX_DETECTOR_ID_LENGTH = 12;
        public const int DETECTOR_TYPE_LENGTH = 12; /*length of a detector type */
        public const int ELECTRONICS_ID_LENGTH = 9; /*length of an electronics id */
        public const int DESCRIPTION_LENGTH = 21; /*length of a stratum id, facility and MBA description */
        public const int MAX_ROD_TYPE_LENGTH = 2;
        public const int MAX_ADDASRC_POSITIONS = 5; // max # add-a-source positions
        public const int MAX_DUAL_ENERGY_ROWS = 25; // max # dual energy rows for mult
        public const int NUMBER_SR_SPARES = 10; // # double spares in detector db
        public const int NUMBER_RESULTS_SPARES = 93; // # double spares in results db
        public const int NUMBER_RUN_SPARES = 10; // # double spares in run db
        public const int NUMBER_BIAS_SPARES = 10; // # double spares in bias results db
        public const int NUMBER_PREC_SPARES = 10; // # double spares in precision results db
        public const int NUMBER_CC_SPARES = 6; // # double spares in calibration curve db
        public const int NUMBER_KA_SPARES = 0; // # double spares in known alpha db
        public const int NUMBER_KM_SPARES = 10; // # double spares in known M db
        public const int NUMBER_MUL_SPARES = 9; // # double spares in multiplicity db
        public const int NUMBER_AP_SPARES = 10; // # double spares in active/passive db
        public const int NUMBER_ACT_SPARES = 10; // # double spares in active db
        public const int NUMBER_COL_SPARES = 10; // # double spares in collar db
        public const int NUMBER_AD_SPARES = 10; // # double spares in add-a-source db
        public const int NUMBER_AD_CF_SPARES = 7; // # double spares in add-a-source cf db
        public const int NUMBER_AM_SPARES = 10; // # double spares in active multiplication db
        public const int NUMBER_CR_SPARES = 10; // # double spares in curium ratio db
        public const int NUMBER_TM_SPARES = 10; // # double spares in truncated multiplicity db
        public const int NUMBER_CC_RESULTS_SPARES = 2; // # double spares in calibration curve results db
        public const int NUMBER_KA_RESULTS_SPARES = 0; // # double spares in known alpha results db
        public const int NUMBER_KM_RESULTS_SPARES = 10; // # double spares in known M results db
        public const int NUMBER_MUL_RESULTS_SPARES = 10; // # double spares in multiplicity results db
        public const int NUMBER_AP_RESULTS_SPARES = 10; // # double spares in active/passive results db
        public const int NUMBER_ACT_RESULTS_SPARES = 10; // # double spares in active results db
        public const int NUMBER_COL_RESULTS_SPARES = 10; // # double spares in collar results db
        public const int NUMBER_AD_RESULTS_SPARES = 4; // # double spares in add-a-source results db
        public const int NUMBER_AM_RESULTS_SPARES = 10; // # double spares in active multiplication results db
        public const int NUMBER_CR_RESULTS_SPARES = 10; // # double spares in curium ratio results db
        public const int NUMBER_TM_RESULTS_SPARES = 10; // # double spares in truncated multiplicity results db

        public const int IDC_USE_SINGLES = 1867; // "Use singles to calculate mass",
        public const int IDC_USE_DOUBLES = 1868; // "Use doubles rate to calculate mass",
        public const int IDC_USE_ADD_A_SOURCE_DOUBLES = 1924; //"Use add-a-source corrected doubles to calculate mass",

        public const int IDC_MEASURE_ACCIDENTALS = 1921; // for test parms
        public const int IDC_CALCULATE_ACCIDENTALS = 1923;

        public const int IDC_THEORETICAL_STD_DEV = 1566;
        public const int IDC_SAMPLE_STD_DEV = 1567;

        public const int IDC_BIAS_SINGLES = 1686; // "Use AmLi source for normalization test",
        public const int IDC_BIAS_DOUBLES = 1687; //"Use Cf252 source doubles rate for normalization test",
        public const int IDC_BIAS_COLLAR = 1688; // Collar normalization test
        public const int IDC_BIAS_CF252_SINGLES = 1690; //"Use Cf252 source singles rate for normalization test",

        // added this yet another multi array size
        /* SR Limits */
        public const int SR_MAX_MULT = 256;	/* Maximum number of multiplicity channels */
        public const int SR_EX_MAX_MULT = SR_MAX_MULT * 2; /*size of multiplicity arrays for the JSR-15 and future LANL products */

        /* ASS stuff */
        public const int IDC_NONE = 1471;
        public const int IDC_COMPUMOTOR_4000 = 1469;
        public const int IDC_COMPUMOTOR_3000 = 1361;
        public const int IDC_PLC_JCC21 = 1473;
        public const int IDC_PLC_WM3100 = 1474;
        public const int IDC_CANBERRA_COUNTER = 1482;
        public const int IDC_MANUAL = 1480;

      }

    # region the INCC structs
    // dev note: all 2-dim arrays from C++ declared here as [dim1 * dim2 ] to facilitate C++/C# unsafe declarations. 
    // In the transfer code, only the first position in any of the 2d arrays is used,  [,] indexing isn't needed yet

    /* database ncc_db record/key structure declarations */

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct archive_parms_rec
    {
        public UInt16 days_before_auto_archive;
        public UInt16 days_before_auto_delete;
        public UInt16 days_before_db_auto_delete;
        public fixed byte data_dir[INCC.MAX_PATH_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct test_parms_rec
    {
        public double acc_sngl_test_rate_limit;
        public double acc_sngl_test_precision_limit;
        public double acc_sngl_test_outlier_limit;
        public double outlier_test_limit;
        public double bkg_doubles_rate_limit;
        public double bkg_triples_rate_limit;
        public double chisq_limit;
        public UInt16 max_num_failures;
        public double high_voltage_test_limit;
        public double normal_backup_assay_test_limit;
        public UInt16 max_runs_for_outlier_test;
        public byte checksum_test;
        public UInt16 accidentals_method;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct acquire_parms_rec
    {
        public fixed byte acq_facility[INCC.FACILITY_LENGTH];
        public fixed byte acq_facility_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte acq_mba[INCC.MBA_LENGTH];
        public fixed byte acq_mba_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte acq_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte acq_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte acq_glovebox_id[INCC.MAX_GLOVEBOX_ID_LENGTH];
        public fixed byte acq_isotopics_id[INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte acq_comp_isotopics_id[INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte acq_campaign_id[INCC.MAX_CAMPAIGN_ID_LENGTH];
        public fixed byte acq_item_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte acq_stratum_id[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte acq_stratum_id_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte acq_user_id[INCC.CHAR_FIELD_LENGTH];
        public fixed byte acq_comment[INCC.MAX_COMMENT_LENGTH];
        public byte acq_ending_comment;
        public byte acq_data_src;
        public byte acq_qc_tests;
        public byte acq_print;
        public byte acq_review_detector_parms;
        public byte acq_review_calib_parms;
        public byte acq_review_isotopics;
        public byte acq_review_summed_raw_data;
        public byte acq_review_run_raw_data;
        public byte acq_review_run_rate_data;
        public byte acq_review_summed_mult_dist;
        public byte acq_review_run_mult_dist;
        public double acq_run_count_time;
        public UInt16 acq_acquire_type;
        public UInt16 acq_num_runs;
        public UInt16 acq_active_num_runs;
        public UInt16 acq_min_num_runs;
        public UInt16 acq_max_num_runs;
        public double acq_meas_precision;
        public byte acq_well_config;
        public double acq_mass;
        public UInt16 acq_error_calc_method;
        public fixed byte acq_inventory_change_code[INCC.INVENTORY_CHG_LENGTH];
        public fixed byte acq_io_code[INCC.IO_CODE_LENGTH];
        public byte acq_collar_mode;
        public double acq_drum_empty_weight;
        public fixed byte acq_meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte acq_meas_time[INCC.DATE_TIME_LENGTH];
        public fixed byte acq_meas_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct item_id_entry_rec
    {
        public fixed byte item_id_entry[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte item_type_ascii[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte isotopics_id_ascii[INCC.MAX_ITEM_IDS * INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte stratum_id_ascii[INCC.MAX_ITEM_IDS * INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte inventory_change_code_entry[INCC.MAX_ITEM_IDS * INCC.INVENTORY_CHG_LENGTH];
        public fixed byte io_code_entry[INCC.MAX_ITEM_IDS * INCC.IO_CODE_LENGTH];
        public fixed double declared_mass_entry[INCC.MAX_ITEM_IDS];
        public fixed double declared_u_mass_entry[INCC.MAX_ITEM_IDS];
        public fixed double length_entry[INCC.MAX_ITEM_IDS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct mba_item_id_entry_rec
    {
        public fixed byte mba_ascii[INCC.MAX_ITEM_IDS * INCC.MBA_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct collar_data_entry_rec
    {
        public fixed byte col_item_id_entry[INCC.MAX_COLLAR_DATA_SETS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed double col_length_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_length_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_pu_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_pu_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_depleted_u_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_depleted_u_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_natural_u_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_natural_u_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_enriched_u_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_enriched_u_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_u235_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_u235_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_u238_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_u238_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_rods_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_total_poison_rods_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_poison_percent_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed double col_poison_percent_err_entry[INCC.MAX_COLLAR_DATA_SETS];
        public fixed byte col_rod_type_entry[INCC.MAX_COLLAR_DATA_SETS * INCC.MAX_ROD_TYPE_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct poison_rod_type_rec
    {
        public fixed byte poison_rod_type[INCC.MAX_POISON_ROD_TYPES * INCC.MAX_ROD_TYPE_LENGTH];
        public fixed double poison_absorption_fact[INCC.MAX_POISON_ROD_TYPES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct holdup_config_rec
    {
        public fixed byte glovebox_id[INCC.MAX_GLOVEBOX_ID_LENGTH];
        public UInt16 num_rows;
        public UInt16 num_columns;
        public double distance;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cm_pu_ratio_rec
    {
        public double cm_pu_ratio;
        public double cm_pu_ratio_err;
        public double cm_pu_half_life;
        public fixed byte cm_pu_ratio_date[INCC.DATE_TIME_LENGTH];
        public double cm_u_ratio;
        public double cm_u_ratio_err;
        public fixed byte cm_u_ratio_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cm_id_label[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cm_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cm_input_batch_id[INCC.MAX_ITEM_ID_LENGTH];
        public double cm_dcl_u_mass;
        public double cm_dcl_u235_mass;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct item_type_names_rec
    {
        public fixed byte item_type_names[INCC.MAX_ITEM_TYPES * INCC.MAX_ITEM_TYPE_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct facility_names_rec
    {
        public fixed byte facility_names[INCC.MAX_FACILITIES * INCC.FACILITY_LENGTH];
        public fixed byte facility_description[INCC.MAX_FACILITIES * INCC.DESCRIPTION_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct mba_names_rec
    {
        public fixed byte mba_names[INCC.MAX_MBAS * INCC.MBA_LENGTH];
        public fixed byte mba_description[INCC.MAX_MBAS * INCC.DESCRIPTION_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct stratum_id_names_rec
    {
        public fixed byte stratum_id_names[INCC.MAX_STRATUM_IDS * INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte stratum_id_names_description[INCC.MAX_STRATUM_IDS * INCC.DESCRIPTION_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct inventory_change_code_rec
    {
        public fixed byte inventory_chg_codes[INCC.MAX_INVENTORY_CHG_CODES * INCC.INVENTORY_CHG_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct io_code_rec
    {
        public fixed byte io_codes[INCC.MAX_IO_CODES * INCC.IO_CODE_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct isotopics_rec
    {
        public double pu238;
        public double pu239;
        public double pu240;
        public double pu241;
        public double pu242;
        public double am241;
        public double pu238_err;
        public double pu239_err;
        public double pu240_err;
        public double pu241_err;
        public double pu242_err;
        public double am241_err;
        public fixed byte pu_date[INCC.DATE_TIME_LENGTH];
        public fixed byte am_date[INCC.DATE_TIME_LENGTH];
        public fixed byte isotopics_id[INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte isotopics_source_code[INCC.ISO_SOURCE_CODE_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct composite_isotopics_rec
    {
        public fixed byte ci_ref_date[INCC.DATE_TIME_LENGTH];
        public fixed double ci_pu_mass[INCC.MAX_ISO_COMPS];
        public fixed double ci_pu238[INCC.MAX_ISO_COMPS];
        public fixed double ci_pu239[INCC.MAX_ISO_COMPS];
        public fixed double ci_pu240[INCC.MAX_ISO_COMPS];
        public fixed double ci_pu241[INCC.MAX_ISO_COMPS];
        public fixed double ci_pu242[INCC.MAX_ISO_COMPS];
        public fixed double ci_am241[INCC.MAX_ISO_COMPS];
        public fixed byte ci_pu_date[INCC.MAX_ISO_COMPS * INCC.DATE_TIME_LENGTH];
        public fixed byte ci_am_date[INCC.MAX_ISO_COMPS * INCC.DATE_TIME_LENGTH];
        public fixed byte ci_isotopics_id[INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte ci_isotopics_source_code[INCC.ISO_SOURCE_CODE_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct assay_summary_rec
    {
        public byte as_print;
        public fixed byte as_path[INCC.MAX_PATH_LENGTH];
        public fixed int as_select[INCC.MAX_ASSAY_SUMMARY_VARS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct holdup_summary_rec
    {
        public byte hu_print;
        public fixed byte hu_path[INCC.MAX_PATH_LENGTH];
        public fixed int hu_select[INCC.MAX_HOLDUP_SUMMARY_VARS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct detector_rec
    {
        public fixed byte detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte detector_type[INCC.DETECTOR_TYPE_LENGTH];
        public fixed byte electronics_id[INCC.ELECTRONICS_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct alpha_beta_rec
    {
        public fixed double factorial[INCC.MULTI_ARRAY_SIZE];
        public fixed double alpha_array[INCC.MULTI_ARRAY_SIZE];
        public fixed double beta_array[INCC.MULTI_ARRAY_SIZE];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct bkg_parms_rec
    {
        public double curr_passive_bkg_singles_rate;
        public double curr_passive_bkg_singles_err;
        public double curr_passive_bkg_doubles_rate;
        public double curr_passive_bkg_doubles_err;
        public double curr_passive_bkg_triples_rate;
        public double curr_passive_bkg_triples_err;
        public double curr_active_bkg_singles_rate;
        public double curr_active_bkg_singles_err;
        public double curr_passive_bkg_scaler1_rate;
        public double curr_passive_bkg_scaler2_rate;
        public double curr_active_bkg_scaler1_rate;
        public double curr_active_bkg_scaler2_rate;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct tm_bkg_parms_rec
    {
        public double tm_singles_bkg;
        public double tm_singles_bkg_err;
        public double tm_zeros_bkg;
        public double tm_zeros_bkg_err;
        public double tm_ones_bkg;
        public double tm_ones_bkg_err;
        public double tm_twos_bkg;
        public double tm_twos_bkg_err;
        public byte tm_bkg;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct norm_parms_rec
    {
        public fixed byte source_id[INCC.SOURCE_ID_LENGTH];
        public double curr_normalization_constant;
        public double curr_normalization_constant_err;
        public UInt16 bias_mode;
        public double meas_rate;
        public double meas_rate_err;
        public double amli_ref_singles_rate;
        public double cf252_ref_doubles_rate;
        public double cf252_ref_doubles_rate_err;
        public fixed byte ref_date[INCC.DATE_TIME_LENGTH];
        public double init_src_precision_limit;
        public double bias_precision_limit;
        public double acceptance_limit_std_dev;
        public double acceptance_limit_percent;
        public double yield_relative_to_mrc_95;
        public byte bias_test_use_addasrc;
        public double bias_test_addasrc_position;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct sr_parms_rec
    {
        public fixed byte sr_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public short sr_type;
        public short sr_port_number;
        public double predelay;
        public double gate_length;
        public double gate_length2;
        public double high_voltage;
        public double die_away_time;
        public double efficiency;
        public double multiplicity_deadtime;
        public double coeff_a_deadtime;
        public double coeff_b_deadtime;
        public double coeff_c_deadtime;
        public double doubles_gate_fraction;
        public double triples_gate_fraction;
        public fixed double sr_spares[INCC.NUMBER_SR_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct unattended_parms_rec
    {
        public UInt32 error_seconds;
        public byte auto_import;
        public double add_a_source_threshold;
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct analysis_method_rec
    {
        public fixed byte item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte analysis_method_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public byte cal_curve;
        public byte known_alpha;
        public byte known_m;
        public byte multiplicity;
        public byte add_a_source;
        public byte active;
        public byte active_mult;
        public byte active_passive;
        public byte collar;
        public byte normal_method;
        public byte backup_method;
        public byte curium_ratio;
        public byte truncated_mult;
        public byte analysis_method_spare1;
        public byte analysis_method_spare2;
        public byte analysis_method_spare3;
        public byte analysis_method_spare4;
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct cal_curve_rec
    {
        public byte cal_curve_equation;
        public double cc_a;
        public double cc_b;
        public double cc_c;
        public double cc_d;
        public double cc_var_a;
        public double cc_var_b;
        public double cc_var_c;
        public double cc_var_d;
        public double cc_covar_ab;
        public double cc_covar_ac;
        public double cc_covar_ad;
        public double cc_covar_bc;
        public double cc_covar_bd;
        public double cc_covar_cd;
        public double cc_sigma_x;
        public double cc_cal_curve_type;
        public double cc_heavy_metal_corr_factor;
        public double cc_heavy_metal_reference;
        public double cc_percent_u235;
        public fixed double cal_curve_spares[INCC.NUMBER_CC_SPARES];
        public fixed byte cal_curve_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte cal_curve_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double cc_lower_mass_limit;
        public double cc_upper_mass_limit;
        public fixed double cc_dcl_mass[INCC.MAX_NUM_CALIB_PTS];
        public fixed double cc_doubles[INCC.MAX_NUM_CALIB_PTS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct active_rec
    {
        public byte active_equation;
        public double act_a;
        public double act_b;
        public double act_c;
        public double act_d;
        public double act_var_a;
        public double act_var_b;
        public double act_var_c;
        public double act_var_d;
        public double act_covar_ab;
        public double act_covar_ac;
        public double act_covar_ad;
        public double act_covar_bc;
        public double act_covar_bd;
        public double act_covar_cd;
        public double act_sigma_x;
        fixed double active_spares[INCC.NUMBER_ACT_SPARES];
        public fixed byte active_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double act_lower_mass_limit;
        public double act_upper_mass_limit;
        public fixed double act_dcl_mass[INCC.MAX_NUM_CALIB_PTS];
        public fixed double act_doubles[INCC.MAX_NUM_CALIB_PTS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct collar_rec
    {
        public byte collar_equation;
        public double col_a;
        public double col_b;
        public double col_c;
        public double col_d;
        public double col_var_a;
        public double col_var_b;
        public double col_var_c;
        public double col_var_d;
        public double col_covar_ab;
        public double col_covar_ac;
        public double col_covar_ad;
        public double col_covar_bc;
        public double col_covar_bd;
        public double col_covar_cd;
        public double col_sigma_x;
        public double col_number_calib_rods;
        public fixed byte col_poison_rod_type[INCC.MAX_POISON_ROD_TYPES * INCC.MAX_ROD_TYPE_LENGTH];
        public fixed double col_poison_absorption_fact[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_a[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_a_err[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_b[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_b_err[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_c[INCC.MAX_POISON_ROD_TYPES];
        public fixed double col_poison_rod_c_err[INCC.MAX_POISON_ROD_TYPES];
        public double col_u_mass_corr_fact_a;
        public double col_u_mass_corr_fact_a_err;
        public double col_u_mass_corr_fact_b;
        public double col_u_mass_corr_fact_b_err;
        public double col_sample_corr_fact;
        public double col_sample_corr_fact_err;
        fixed double collar_spares[INCC.NUMBER_COL_SPARES];
        public fixed byte collar_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        //Change to int to hold Enum CollarType
        public int collar_mode;
        public double col_lower_mass_limit;
        public double col_upper_mass_limit;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]

    unsafe public struct collar_detector_rec
    {
        public fixed byte collar_detector_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        //Changed to reflect new CollarType enum
        public int collar_detector_mode;
        public fixed byte collar_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte col_reference_date[INCC.DATE_TIME_LENGTH];
        public double col_relative_doubles_rate;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct collar_k5_rec
    {
        public fixed byte collar_k5_label[INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH];
        public fixed int collar_k5_checkbox[INCC.MAX_COLLAR_K5_PARAMETERS];
        public fixed double collar_k5[INCC.MAX_COLLAR_K5_PARAMETERS];
        public fixed double collar_k5_err[INCC.MAX_COLLAR_K5_PARAMETERS];
        public fixed byte collar_k5_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public int collar_k5_mode;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct known_alpha_rec
    {
        public double ka_alpha_wt;
        public double ka_rho_zero;
        public double ka_k;
        public double ka_a;
        public double ka_b;
        public double ka_var_a;
        public double ka_var_b;
        public double ka_covar_ab;
        public double ka_sigma_x;
        public double ka_known_alpha_type;
        public double ka_heavy_metal_corr_factor;
        public double ka_heavy_metal_reference;
        public double ka_ring_ratio_equation;
        public double ka_ring_ratio_a;
        public double ka_ring_ratio_b;
        public double ka_ring_ratio_c;
        public double ka_ring_ratio_d;
        public double ka_lower_corr_factor_limit;
        public double ka_upper_corr_factor_limit;
        public fixed byte known_alpha_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte known_alpha_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double ka_lower_mass_limit;
        public double ka_upper_mass_limit;
        public fixed double ka_dcl_mass[INCC.MAX_NUM_CALIB_PTS];
        public fixed double ka_doubles[INCC.MAX_NUM_CALIB_PTS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct known_m_rec
    {
        public double km_sf_rate;
        public double km_vs1;
        public double km_vs2;
        public double km_vi1;
        public double km_vi2;
        public double km_b;
        public double km_c;
        public double km_sigma_x;
        public fixed double known_m_spares[INCC.NUMBER_KM_SPARES];
        public fixed byte known_m_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte known_m_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double km_lower_mass_limit;
        public double km_upper_mass_limit;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct multiplicity_rec
    {
        public byte mul_solve_efficiency;
        public double mul_sf_rate;
        public double mul_vs1;
        public double mul_vs2;
        public double mul_vs3;
        public double mul_vi1;
        public double mul_vi2;
        public double mul_vi3;
        public double mul_a;
        public double mul_b;
        public double mul_c;
        public double mul_sigma_x;
        public double mul_alpha_weight;
        public fixed double multiplicity_spares[INCC.NUMBER_MUL_SPARES];
        public fixed byte multiplicity_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte multiplicity_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct active_passive_rec
    {
        public byte active_passive_equation;
        public double ap_a;
        public double ap_b;
        public double ap_c;
        public double ap_d;
        public double ap_var_a;
        public double ap_var_b;
        public double ap_var_c;
        public double ap_var_d;
        public double ap_covar_ab;
        public double ap_covar_ac;
        public double ap_covar_ad;
        public double ap_covar_bc;
        public double ap_covar_bd;
        public double ap_covar_cd;
        public double ap_sigma_x;
        fixed double active_passive_spares[INCC.NUMBER_AP_SPARES];
        public fixed byte active_passive_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_passive_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double ap_lower_mass_limit;
        public double ap_upper_mass_limit;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct add_a_source_rec
    {
        public byte add_a_source_equation;
        public double ad_a;
        public double ad_b;
        public double ad_c;
        public double ad_d;
        public double ad_var_a;
        public double ad_var_b;
        public double ad_var_c;
        public double ad_var_d;
        public double ad_covar_ab;
        public double ad_covar_ac;
        public double ad_covar_ad;
        public double ad_covar_bc;
        public double ad_covar_bd;
        public double ad_covar_cd;
        public double ad_sigma_x;
        public fixed double add_a_source_spares[INCC.NUMBER_AD_SPARES];
        public fixed double ad_position_dzero[INCC.MAX_ADDASRC_POSITIONS];
        public double ad_dzero_avg;
        public fixed byte ad_dzero_ref_date[INCC.DATE_TIME_LENGTH];
        public UInt16 ad_num_runs;
        public double ad_cf_a;
        public double ad_cf_b;
        public double ad_cf_c;
        public double ad_cf_d;
        public double ad_use_truncated_mult;
        public double ad_tm_weighting_factor;
        public double ad_tm_dbls_rate_upper_limit;
        fixed double add_a_source_cf_spares[INCC.NUMBER_AD_CF_SPARES];
        public fixed byte add_a_source_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte add_a_source_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double ad_lower_mass_limit;
        public double ad_upper_mass_limit;
        public fixed double ad_dcl_mass[INCC.MAX_NUM_CALIB_PTS];
        public fixed double ad_doubles[INCC.MAX_NUM_CALIB_PTS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct active_mult_rec
    {
        public double am_vt1;
        public double am_vt2;
        public double am_vt3;
        public double am_vf1;
        public double am_vf2;
        public double am_vf3;
        fixed double active_mult_spares[INCC.NUMBER_AM_SPARES];
        public fixed byte active_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct curium_ratio_rec
    {
        public byte curium_ratio_equation;
        public double cr_a;
        public double cr_b;
        public double cr_c;
        public double cr_d;
        public double cr_var_a;
        public double cr_var_b;
        public double cr_var_c;
        public double cr_var_d;
        public double cr_covar_ab;
        public double cr_covar_ac;
        public double cr_covar_ad;
        public double cr_covar_bc;
        public double cr_covar_bd;
        public double cr_covar_cd;
        public double cr_sigma_x;
        public UInt16 curium_ratio_type;
        fixed double curium_ratio_spares[INCC.NUMBER_CR_SPARES];
        public fixed byte curium_ratio_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte curium_ratio_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public double cr_lower_mass_limit;
        public double cr_upper_mass_limit;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct truncated_mult_rec
    {
        public double tm_a;
        public double tm_b;
        public byte tm_known_eff;
        public byte tm_solve_eff;
        public fixed double truncated_mult_spares[INCC.NUMBER_TM_SPARES];
        public fixed byte truncated_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte truncated_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct results_rec
    {
        public fixed byte meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte meas_time[INCC.DATE_TIME_LENGTH];
        public fixed byte filename[INCC.FILE_NAME_LENGTH];
        public fixed byte original_meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte results_facility[INCC.FACILITY_LENGTH];
        public fixed byte results_facility_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte results_mba[INCC.MBA_LENGTH];
        public fixed byte results_mba_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte item_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte stratum_id[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte stratum_id_description[INCC.DESCRIPTION_LENGTH];
        public fixed byte results_campaign_id[INCC.MAX_CAMPAIGN_ID_LENGTH];
        public fixed byte results_inspection_number[INCC.MAX_CAMPAIGN_ID_LENGTH];
        public fixed byte results_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public int results_collar_mode;
        public fixed byte results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte results_detector_type[INCC.DETECTOR_TYPE_LENGTH];
        public fixed byte results_electronics_id[INCC.ELECTRONICS_ID_LENGTH];
        public fixed byte results_glovebox_id[INCC.MAX_GLOVEBOX_ID_LENGTH];
        public UInt16 results_num_rows;
        public UInt16 results_num_columns;
        public double results_distance;
        public double bias_uncertainty;
        public double random_uncertainty;
        public double systematic_uncertainty;
        public double relative_std_dev;
        public byte completed;
        public byte meas_option;
        public fixed byte inventory_change_code[INCC.INVENTORY_CHG_LENGTH];
        public fixed byte io_code[INCC.IO_CODE_LENGTH];
        public byte well_config;
        public byte data_source;
        public byte results_qc_tests;
        public UInt16 error_calc_method;
        public byte results_print;
        public fixed byte user_id[INCC.CHAR_FIELD_LENGTH];
        public fixed byte comment[INCC.MAX_COMMENT_LENGTH];
        public fixed byte ending_comment[INCC.MAX_COMMENT_LENGTH];
        public double item_pu238;
        public double item_pu239;
        public double item_pu240;
        public double item_pu241;
        public double item_pu242;
        public double item_am241;
        public double item_pu238_err;
        public double item_pu239_err;
        public double item_pu240_err;
        public double item_pu241_err;
        public double item_pu242_err;
        public double item_am241_err;
        public fixed byte item_pu_date[INCC.DATE_TIME_LENGTH];
        public fixed byte item_am_date[INCC.DATE_TIME_LENGTH];
        public fixed byte item_isotopics_id[INCC.MAX_ISOTOPICS_ID_LENGTH];
        public fixed byte item_isotopics_source_code[INCC.ISO_SOURCE_CODE_LENGTH];
        public double normalization_constant;
        public double normalization_constant_err;
        public double results_predelay;
        public double results_gate_length;
        public double results_gate_length2;
        public double results_high_voltage;
        public double results_die_away_time;
        public double results_efficiency;
        public double results_multiplicity_deadtime;
        public double results_coeff_a_deadtime;
        public double results_coeff_b_deadtime;
        public double results_coeff_c_deadtime;
        public double results_doubles_gate_fraction;
        public double results_triples_gate_fraction;
        public double r_acc_sngl_test_rate_limit;
        public double r_acc_sngl_test_precision_limit;
        public double r_acc_sngl_test_outlier_limit;
        public double r_outlier_test_limit;
        public double r_bkg_doubles_rate_limit;
        public double r_bkg_triples_rate_limit;
        public double r_chisq_limit;
        public UInt16 r_max_num_failures;
        public double r_high_voltage_test_limit;
        public double passive_bkg_singles_rate;
        public double passive_bkg_singles_rate_err;
        public double passive_bkg_doubles_rate;
        public double passive_bkg_doubles_rate_err;
        public double passive_bkg_triples_rate;
        public double passive_bkg_triples_rate_err;
        public double active_bkg_singles_rate;
        public double active_bkg_singles_rate_err;
        public double passive_bkg_scaler1_rate;
        public double passive_bkg_scaler2_rate;
        public double active_bkg_scaler1_rate;
        public double active_bkg_scaler2_rate;
        public fixed byte error_msg_codes[INCC.NUM_ERROR_MSG_CODES * INCC.ERR_MSG_LENGTH];
        public fixed byte warning_msg_codes[INCC.NUM_WARNING_MSG_CODES * INCC.ERR_MSG_LENGTH];
        public UInt16 total_number_runs;
        public UInt16 number_good_runs;
        public double total_good_count_time;
        public double singles_sum;
        public double scaler1_sum;
        public double scaler2_sum;
        public double reals_plus_acc_sum;
        public double acc_sum;
        public fixed double mult_reals_plus_acc_sum[INCC.MULTI_ARRAY_SIZE];
        public fixed double mult_acc_sum[INCC.MULTI_ARRAY_SIZE];
        public double singles;
        public double singles_err;
        public double doubles;
        public double doubles_err;
        public double triples;
        public double triples_err;
        public double scaler1;
        public double scaler1_err;
        public double scaler2;
        public double scaler2_err;
        public double uncorrected_doubles;
        public double uncorrected_doubles_err;
        public double singles_multi;
        public double doubles_multi;
        public double triples_multi;
        public double declared_mass;
        public byte primary_analysis_method;
        public double net_drum_weight;
        public fixed byte passive_meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte passive_meas_time[INCC.DATE_TIME_LENGTH];
        public fixed byte passive_filename[INCC.FILE_NAME_LENGTH];
        public fixed byte passive_results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte active_meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte active_meas_time[INCC.DATE_TIME_LENGTH];
        public fixed byte active_filename[INCC.FILE_NAME_LENGTH];
        public fixed byte active_results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed double covariance_matrix[9];
        public double r_normal_backup_assay_test_lim;
        public double r_max_runs_for_outlier_test;
        public double r_checksum_test;
        public double results_accidentals_method;
        public double declared_u_mass;
        public double length;
        public double db_version;
        public fixed double results_spares[INCC.NUMBER_RESULTS_SPARES];
    };

    public interface iresultsbase  // lowercase because it's inferior LOL
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_init_src_rec : iresultsbase
    {
        public fixed byte init_src_id[INCC.SOURCE_ID_LENGTH];
        public fixed byte init_src_pass_fail[INCC.PASS_FAIL_LENGTH];
        public UInt16 init_src_mode;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_bias_rec : iresultsbase
    {
        public fixed byte bias_source_id[INCC.SOURCE_ID_LENGTH];
        public fixed byte bias_pass_fail[INCC.PASS_FAIL_LENGTH];
        public UInt16 results_bias_mode;
        public double bias_sngls_rate_expect;
        public double bias_sngls_rate_expect_err;
        public double bias_sngls_rate_expect_meas;
        public double bias_sngls_rate_expect_meas_err;
        public double bias_dbls_rate_expect;
        public double bias_dbls_rate_expect_err;
        public double bias_dbls_rate_expect_meas;
        public double bias_dbls_rate_expect_meas_err;
        public double new_norm_constant;
        public double new_norm_constant_err;
        public double meas_precision;
        public double required_precision;
        public double required_meas_seconds;
        public fixed double bias_spares[INCC.NUMBER_BIAS_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_precision_rec : iresultsbase
    {
        public fixed byte prec_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double prec_sample_var;
        public double prec_theoretical_var;
        public double prec_chi_sq;
        public double chi_sq_lower_limit;
        public double chi_sq_upper_limit;
        public fixed double prec_spares[INCC.NUMBER_PREC_SPARES];
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_cal_curve_rec : iresultsbase
    {
        public double cc_pu240e_mass;
        public double cc_pu240e_mass_err;
        public double cc_pu_mass;
        public double cc_pu_mass_err;
        public double cc_dcl_pu240e_mass;
        public double cc_dcl_pu_mass;
        public double cc_dcl_minus_asy_pu_mass;
        public double cc_dcl_minus_asy_pu_mass_err;
        public double cc_dcl_minus_asy_pu_mass_pct;
        public fixed byte cc_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double cc_dcl_u_mass;
        public double cc_length;
        public double cc_heavy_metal_content;
        public double cc_heavy_metal_correction;
        public double cc_heavy_metal_corr_singles;
        public double cc_heavy_metal_corr_singles_err;
        public double cc_heavy_metal_corr_doubles;
        public double cc_heavy_metal_corr_doubles_err;
        public fixed double cc_spares[INCC.NUMBER_CC_RESULTS_SPARES];
        public byte cc_cal_curve_equation;
        public double cc_a_res;
        public double cc_b_res;
        public double cc_c_res;
        public double cc_d_res;
        public double cc_var_a_res;
        public double cc_var_b_res;
        public double cc_var_c_res;
        public double cc_var_d_res;
        public double cc_covar_ab_res;
        public double cc_covar_ac_res;
        public double cc_covar_ad_res;
        public double cc_covar_bc_res;
        public double cc_covar_bd_res;
        public double cc_covar_cd_res;
        public double cc_sigma_x_res;
        public double cc_cal_curve_type_res;
        public double cc_heavy_metal_corr_factor_res;
        public double cc_heavy_metal_reference_res;
        public double cc_percent_u235_res;
        public fixed double cc_spares_res[INCC.NUMBER_CC_SPARES];
        public fixed byte cc_cal_curve_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte cc_cal_curve_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_known_alpha_rec : iresultsbase
    {
        public double ka_mult;
        public double ka_alpha;
        public double ka_mult_corr_doubles;
        public double ka_mult_corr_doubles_err;
        public double ka_pu240e_mass;
        public double ka_pu240e_mass_err;
        public double ka_pu_mass;
        public double ka_pu_mass_err;
        public double ka_dcl_pu240e_mass;
        public double ka_dcl_pu_mass;
        public double ka_dcl_minus_asy_pu_mass;
        public double ka_dcl_minus_asy_pu_mass_err;
        public double ka_dcl_minus_asy_pu_mass_pct;
        public fixed byte ka_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double ka_dcl_u_mass;
        public double ka_length;
        public double ka_heavy_metal_content;
        public double ka_heavy_metal_correction;
        public double ka_corr_singles;
        public double ka_corr_singles_err;
        public double ka_corr_doubles;
        public double ka_corr_doubles_err;
        public double ka_corr_factor;
        public double ka_dry_alpha_or_mult_dbls;
        public double ka_alpha_wt_res;
        public double ka_rho_zero_res;
        public double ka_k_res;
        public double ka_a_res;
        public double ka_b_res;
        public double ka_var_a_res;
        public double ka_var_b_res;
        public double ka_covar_ab_res;
        public double ka_sigma_x_res;
        public double ka_known_alpha_type_res;
        public double ka_heavy_metal_corr_factor_res;
        public double ka_heavy_metal_reference_res;
        public double ka_ring_ratio_equation_res;
        public double ka_ring_ratio_a_res;
        public double ka_ring_ratio_b_res;
        public double ka_ring_ratio_c_res;
        public double ka_ring_ratio_d_res;
        public double ka_lower_corr_factor_limit_res;
        public double ka_upper_corr_factor_limit_res;
        public fixed byte ka_known_alpha_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte ka_known_alpha_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_known_m_rec : iresultsbase
    {
        public double km_mult;
        public double km_alpha;
        public double km_pu239e_mass;
        public double km_pu240e_mass;
        public double km_pu240e_mass_err;
        public double km_pu_mass;
        public double km_pu_mass_err;
        public double km_dcl_pu240e_mass;
        public double km_dcl_pu_mass;
        public double km_dcl_minus_asy_pu_mass;
        public double km_dcl_minus_asy_pu_mass_err;
        public double km_dcl_minus_asy_pu_mass_pct;
        public fixed byte km_pass_fail[INCC.PASS_FAIL_LENGTH];
        public fixed double km_spares[INCC.NUMBER_KM_RESULTS_SPARES];
        public double km_sf_rate_res;
        public double km_vs1_res;
        public double km_vs2_res;
        public double km_vi1_res;
        public double km_vi2_res;
        public double km_b_res;
        public double km_c_res;
        public double km_sigma_x_res;
        public fixed double km_spares_res[INCC.NUMBER_KM_SPARES];
        public fixed byte km_known_m_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte km_known_m_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_multiplicity_rec : iresultsbase
    {
        public double mul_mult;
        public double mul_mult_err;
        public double mul_alpha;
        public double mul_alpha_err;
        public double mul_corr_factor;
        public double mul_corr_factor_err;
        public double mul_efficiency;
        public double mul_efficiency_err;
        public double mul_pu240e_mass;
        public double mul_pu240e_mass_err;
        public double mul_pu_mass;
        public double mul_pu_mass_err;
        public double mul_dcl_pu240e_mass;
        public double mul_dcl_pu_mass;
        public double mul_dcl_minus_asy_pu_mass;
        public double mul_dcl_minus_asy_pu_mass_err;
        public double mul_dcl_minus_asy_pu_mass_pct;
        public fixed byte mul_pass_fail[INCC.PASS_FAIL_LENGTH];
        public fixed double mul_spares[INCC.NUMBER_MUL_RESULTS_SPARES];
        public byte mul_solve_efficiency_res;
        public double mul_sf_rate_res;
        public double mul_vs1_res;
        public double mul_vs2_res;
        public double mul_vs3_res;
        public double mul_vi1_res;
        public double mul_vi2_res;
        public double mul_vi3_res;
        public double mul_a_res;
        public double mul_b_res;
        public double mul_c_res;
        public double mul_sigma_x_res;
        public double mul_alpha_weight_res;
        public fixed double mul_spares_res[INCC.NUMBER_MUL_SPARES];
        public fixed byte mul_multiplicity_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte mul_multiplicity_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_active_passive_rec : iresultsbase
    {
        public double ap_delta_doubles;
        public double ap_delta_doubles_err;
        public double ap_u235_mass;
        public double ap_u235_mass_err;
        public double ap_k0;
        public double ap_k1;
        public double ap_k1_err;
        public double ap_k;
        public double ap_k_err;
        public double ap_dcl_u235_mass;
        public double ap_dcl_minus_asy_u235_mass;
        public double ap_dcl_minus_asy_u235_mass_err;
        public double ap_dcl_minus_asy_u235_mass_pct;
        public fixed byte ap_pass_fail[INCC.PASS_FAIL_LENGTH];
        public fixed double ap_spares[INCC.NUMBER_AP_RESULTS_SPARES];
        public byte ap_active_passive_equation;
        public double ap_a_res;
        public double ap_b_res;
        public double ap_c_res;
        public double ap_d_res;
        public double ap_var_a_res;
        public double ap_var_b_res;
        public double ap_var_c_res;
        public double ap_var_d_res;
        public double ap_covar_ab_res;
        public double ap_covar_ac_res;
        public double ap_covar_ad_res;
        public double ap_covar_bc_res;
        public double ap_covar_bd_res;
        public double ap_covar_cd_res;
        public double ap_sigma_x_res;
        public fixed double ap_spares_res[INCC.NUMBER_AP_SPARES];
        public fixed byte ap_active_passive_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte ap_active_passive_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_active_rec : iresultsbase
    {
        public double act_u235_mass;
        public double act_u235_mass_err;
        public double act_k0;
        public double act_k1;
        public double act_k1_err;
        public double act_k;
        public double act_k_err;
        public double act_dcl_u235_mass;
        public double act_dcl_minus_asy_u235_mass;
        public double act_dcl_minus_asy_u235_mass_err;
        public double act_dcl_minus_asy_u235_mass_pct;
        public fixed byte act_pass_fail[INCC.PASS_FAIL_LENGTH];
        fixed double act_spares[INCC.NUMBER_ACT_RESULTS_SPARES];
        public byte act_active_equation;
        public double act_a_res;
        public double act_b_res;
        public double act_c_res;
        public double act_d_res;
        public double act_var_a_res;
        public double act_var_b_res;
        public double act_var_c_res;
        public double act_var_d_res;
        public double act_covar_ab_res;
        public double act_covar_ac_res;
        public double act_covar_ad_res;
        public double act_covar_bc_res;
        public double act_covar_bd_res;
        public double act_covar_cd_res;
        public double act_sigma_x_res;
        fixed double act_spares_res[INCC.NUMBER_ACT_SPARES];
        public fixed byte act_active_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte act_active_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct results_collar_rec : iresultsbase
    {
        public double col_u235_mass;
        public double col_u235_mass_err;
        public double col_percent_u235;
        public double col_total_u_mass;
        public double col_k0;
        public double col_k0_err;
        public double col_k1;
        public double col_k1_err;
        public double col_k2;
        public double col_k2_err;
        public double col_k3;
        public double col_k3_err;
        public double col_k4;
        public double col_k4_err;
        public double col_k5;
        public double col_k5_err;
        public double col_total_corr_fact;
        public double col_total_corr_fact_err;
        public fixed byte col_source_id[INCC.SOURCE_ID_LENGTH];
        public double col_corr_doubles;
        public double col_corr_doubles_err;
        public double col_dcl_length;
        public double col_dcl_length_err;
        public double col_dcl_total_u235;
        public double col_dcl_total_u235_err;
        public double col_dcl_total_u238;
        public double col_dcl_total_u238_err;
        public double col_dcl_total_rods;
        public double col_dcl_total_poison_rods;
        public double col_dcl_poison_percent;
        public double col_dcl_poison_percent_err;
        public double col_dcl_minus_asy_u235_mass;
        public double col_dcl_minus_asy_u235_mass_err;
        public double col_dcl_minus_asy_u235_mass_pct;
        public fixed byte col_pass_fail[INCC.PASS_FAIL_LENGTH];
        fixed double col_spares[INCC.NUMBER_COL_RESULTS_SPARES];
        public fixed byte col_collar_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public int col_collar_mode;
        public fixed byte col_collar_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public byte col_collar_equation;
        public double col_a_res;
        public double col_b_res;
        public double col_c_res;
        public double col_d_res;
        public double col_var_a_res;
        public double col_var_b_res;
        public double col_var_c_res;
        public double col_var_d_res;
        public double col_covar_ab_res;
        public double col_covar_ac_res;
        public double col_covar_ad_res;
        public double col_covar_bc_res;
        public double col_covar_bd_res;
        public double col_covar_cd_res;
        public double col_sigma_x_res;
        public double col_number_calib_rods_res;
        public fixed byte col_poison_rod_type_res[INCC.MAX_ROD_TYPE_LENGTH];
        public double col_poison_absorption_fact_res;
        public double col_poison_rod_a_res;
        public double col_poison_rod_a_err_res;
        public double col_poison_rod_b_res;
        public double col_poison_rod_b_err_res;
        public double col_poison_rod_c_res;
        public double col_poison_rod_c_err_res;
        public double col_u_mass_corr_fact_a_res;
        public double col_u_mass_corr_fact_a_err_res;
        public double col_u_mass_corr_fact_b_res;
        public double col_u_mass_corr_fact_b_err_res;
        public double col_sample_corr_fact_res;
        public double col_sample_corr_fact_err_res;
        public fixed byte col_reference_date_res[INCC.DATE_TIME_LENGTH];
        public double col_relative_doubles_rate_res;
        fixed double col_spares_res[INCC.NUMBER_COL_SPARES];
        public fixed byte collar_k5_label_res[INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH];
        public fixed byte collar_k5_checkbox_res[INCC.MAX_COLLAR_K5_PARAMETERS];
        public fixed double collar_k5_res[INCC.MAX_COLLAR_K5_PARAMETERS];
        public fixed double collar_k5_err_res[INCC.MAX_COLLAR_K5_PARAMETERS];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_add_a_source_rec : iresultsbase
    {
        public double ad_dzero_cf252_doubles;
        public fixed double ad_sample_cf252_doubles[INCC.MAX_ADDASRC_POSITIONS];
        public fixed double ad_sample_cf252_doubles_err[INCC.MAX_ADDASRC_POSITIONS];
        public fixed double ad_sample_cf252_ratio[INCC.MAX_ADDASRC_POSITIONS];
        public double ad_sample_avg_cf252_doubles;
        public double ad_sample_avg_cf252_doubles_err;
        public double ad_corr_doubles;
        public double ad_corr_doubles_err;
        public double ad_delta;
        public double ad_delta_err;
        public double ad_corr_factor;
        public double ad_corr_factor_err;
        public double ad_pu240e_mass;
        public double ad_pu240e_mass_err;
        public double ad_pu_mass;
        public double ad_pu_mass_err;
        public double ad_dcl_pu240e_mass;
        public double ad_dcl_pu_mass;
        public double ad_dcl_minus_asy_pu_mass;
        public double ad_dcl_minus_asy_pu_mass_err;
        public double ad_dcl_minus_asy_pu_mass_pct;
        public fixed byte ad_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double ad_tm_doubles_bkg;
        public double ad_tm_doubles_bkg_err;
        public double ad_tm_uncorr_doubles;
        public double ad_tm_uncorr_doubles_err;
        public double ad_tm_corr_doubles;
        public double ad_tm_corr_doubles_err;
        fixed double ad_spares[INCC.NUMBER_AD_RESULTS_SPARES];
        public byte ad_add_a_source_equation;
        public double ad_a_res;
        public double ad_b_res;
        public double ad_c_res;
        public double ad_d_res;
        public double ad_var_a_res;
        public double ad_var_b_res;
        public double ad_var_c_res;
        public double ad_var_d_res;
        public double ad_covar_ab_res;
        public double ad_covar_ac_res;
        public double ad_covar_ad_res;
        public double ad_covar_bc_res;
        public double ad_covar_bd_res;
        public double ad_covar_cd_res;
        public double ad_sigma_x_res;
        fixed double ad_spares_res[INCC.NUMBER_AD_SPARES];
        public fixed double ad_position_dzero_res[INCC.MAX_ADDASRC_POSITIONS];
        public double ad_dzero_avg_res;
        public fixed byte ad_dzero_ref_date_res[INCC.DATE_TIME_LENGTH];
        public UInt16 ad_num_runs_res;
        public double ad_cf_a_res;
        public double ad_cf_b_res;
        public double ad_cf_c_res;
        public double ad_cf_d_res;
        public double ad_use_truncated_mult_res;
        public double ad_tm_weighting_factor_res;
        public double ad_tm_dbls_rate_upper_limit_res;
        fixed double ad_cf_spares_res[INCC.NUMBER_AD_CF_SPARES];
        public fixed byte ad_add_a_source_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte ad_add_a_source_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct results_active_mult_rec : iresultsbase
    {
        public double am_mult;
        public double am_mult_err;
        fixed double am_spares[INCC.NUMBER_AM_RESULTS_SPARES];
        public double am_vt1_res;
        public double am_vt2_res;
        public double am_vt3_res;
        public double am_vf1_res;
        public double am_vf2_res;
        public double am_vf3_res;
        fixed double am_spares_res[INCC.NUMBER_AM_SPARES];
        public fixed byte am_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte am_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_curium_ratio_rec : iresultsbase
    {
        public double cr_pu240e_mass;
        public double cr_pu240e_mass_err;
        public double cr_cm_mass;
        public double cr_cm_mass_err;
        public double cr_pu_mass;
        public double cr_pu_mass_err;
        public double cr_u_mass;
        public double cr_u_mass_err;
        public double cr_u235_mass;
        public double cr_u235_mass_err;
        public double cr_dcl_pu_mass;
        public double cr_dcl_minus_asy_pu_mass;
        public double cr_dcl_minus_asy_pu_mass_err;
        public double cr_dcl_minus_asy_pu_mass_pct;
        public double cr_dcl_minus_asy_u_mass;
        public double cr_dcl_minus_asy_u_mass_err;
        public double cr_dcl_minus_asy_u_mass_pct;
        public double cr_dcl_minus_asy_u235_mass;
        public double cr_dcl_minus_asy_u235_mass_err;
        public double cr_dcl_minus_asy_u235_mass_pct;
        public fixed byte cr_pu_pass_fail[INCC.PASS_FAIL_LENGTH];
        public fixed byte cr_u_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double cr_cm_pu_ratio;
        public double cr_cm_pu_ratio_err;
        public double cr_pu_half_life;
        public fixed byte cr_cm_pu_ratio_date[INCC.DATE_TIME_LENGTH];
        public double cr_cm_u_ratio;
        public double cr_cm_u_ratio_err;
        public fixed byte cr_cm_u_ratio_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cr_cm_id_label[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cr_cm_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cr_cm_input_batch_id[INCC.MAX_ITEM_ID_LENGTH];
        public double cr_dcl_u_mass_res;
        public double cr_dcl_u235_mass_res;
        public double cr_cm_pu_ratio_decay_corr;
        public double cr_cm_pu_ratio_decay_corr_err;
        public double cr_cm_u_ratio_decay_corr;
        public double cr_cm_u_ratio_decay_corr_err;
        fixed double cr_spares[INCC.NUMBER_CR_RESULTS_SPARES];
        public byte cr_curium_ratio_equation;
        public double cr_a_res;
        public double cr_b_res;
        public double cr_c_res;
        public double cr_d_res;
        public double cr_var_a_res;
        public double cr_var_b_res;
        public double cr_var_c_res;
        public double cr_var_d_res;
        public double cr_covar_ab_res;
        public double cr_covar_ac_res;
        public double cr_covar_ad_res;
        public double cr_covar_bc_res;
        public double cr_covar_bd_res;
        public double cr_covar_cd_res;
        public double cr_sigma_x_res;
        public UInt16 curium_ratio_type_res;
        fixed double cr_spares_res[INCC.NUMBER_CR_SPARES];
        public fixed byte cr_curium_ratio_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte cr_curium_ratio_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_truncated_mult_rec : iresultsbase
    {
        public double tm_bkg_singles;
        public double tm_bkg_singles_err;
        public double tm_bkg_zeros;
        public double tm_bkg_zeros_err;
        public double tm_bkg_ones;
        public double tm_bkg_ones_err;
        public double tm_bkg_twos;
        public double tm_bkg_twos_err;
        public double tm_net_singles;
        public double tm_net_singles_err;
        public double tm_net_zeros;
        public double tm_net_zeros_err;
        public double tm_net_ones;
        public double tm_net_ones_err;
        public double tm_net_twos;
        public double tm_net_twos_err;
        public double tm_k_alpha;
        public double tm_k_alpha_err;
        public double tm_k_pu240e_mass;
        public double tm_k_pu240e_mass_err;
        public double tm_k_pu_mass;
        public double tm_k_pu_mass_err;
        public double tm_k_dcl_pu240e_mass;
        public double tm_k_dcl_pu_mass;
        public double tm_k_dcl_minus_asy_pu_mass;
        public double tm_k_dcl_minus_asy_pu_mass_err;
        public double tm_k_dcl_minus_asy_pu_mass_pct;
        public fixed byte tm_k_pass_fail[INCC.PASS_FAIL_LENGTH];
        public double tm_s_eff;
        public double tm_s_eff_err;
        public double tm_s_alpha;
        public double tm_s_alpha_err;
        public double tm_s_pu240e_mass;
        public double tm_s_pu240e_mass_err;
        public double tm_s_pu_mass;
        public double tm_s_pu_mass_err;
        public double tm_s_dcl_pu240e_mass;
        public double tm_s_dcl_pu_mass;
        public double tm_s_dcl_minus_asy_pu_mass;
        public double tm_s_dcl_minus_asy_pu_mass_err;
        public double tm_s_dcl_minus_asy_pu_mass_pct;
        public fixed byte tm_s_pass_fail[INCC.PASS_FAIL_LENGTH];
        public fixed double tm_spares[INCC.NUMBER_TM_RESULTS_SPARES];
        public double tm_a_res;
        public double tm_b_res;
        public byte tm_known_eff_res;
        public byte tm_solve_eff_res;
        public fixed double tm_spares_res[INCC.NUMBER_TM_SPARES];
        public fixed byte tm_truncated_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte tm_truncated_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_tm_bkg_rec : iresultsbase
    {
        public double results_tm_singles_bkg;
        public double results_tm_singles_bkg_err;
        public double results_tm_zeros_bkg;
        public double results_tm_zeros_bkg_err;
        public double results_tm_ones_bkg;
        public double results_tm_ones_bkg_err;
        public double results_tm_twos_bkg;
        public double results_tm_twos_bkg_err;
    };

    // the original INCC record
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct run_rec
    {
        public UInt16 run_number;
        public fixed byte run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        public double run_count_time;
        public double run_singles;
        public double run_scaler1;
        public double run_scaler2;
        public double run_reals_plus_acc;
        public double run_acc;
        public fixed double run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        public fixed double run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        public double run_singles_rate;
        public double run_doubles_rate;
        public double run_triples_rate;
        public double run_scaler1_rate;
        public double run_scaler2_rate;
        public double run_multiplicity_mult;
        public double run_multiplicity_alpha;
        public double run_multiplicity_efficiency;
        public double run_mass;
        public double run_high_voltage;
        fixed double run_spare[INCC.NUMBER_RUN_SPARES];
    };

    // the enhanced C# run record, with 512 capacity
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct run_rec_ext
    {
        public UInt16 run_number;
        public fixed byte run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        public double run_count_time;
        public double run_singles;
        public double run_scaler1;
        public double run_scaler2;
        public double run_reals_plus_acc;
        public double run_acc;
        public fixed double run_mult_reals_plus_acc[INCC.SR_MAX_MULT*2];
        public fixed double run_mult_acc[INCC.SR_MAX_MULT*2];
        public double run_singles_rate;
        public double run_doubles_rate;
        public double run_triples_rate;
        public double run_scaler1_rate;
        public double run_scaler2_rate;
        public double run_multiplicity_mult;
        public double run_multiplicity_alpha;
        public double run_multiplicity_efficiency;
        public double run_mass;
        public double run_high_voltage;
    };


    /* dont need these yet
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cf1_run_rec
    {
        UInt16 cf1_run_number;
        public fixed byte cf1_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cf1_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte cf1_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double cf1_run_count_time;
        double cf1_run_singles;
        double cf1_run_scaler1;
        double cf1_run_scaler2;
        double cf1_run_reals_plus_acc;
        double cf1_run_acc;
        fixed double cf1_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double cf1_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double cf1_run_singles_rate;
        double cf1_run_doubles_rate;
        double cf1_run_triples_rate;
        double cf1_run_scaler1_rate;
        double cf1_run_scaler2_rate;
        double cf1_run_multiplicity_mult;
        double cf1_run_multiplicity_alpha;
        double cf1_multiplicity_efficiency;
        double cf1_run_mass;
        double cf1_high_voltage;
        fixed double cf1_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cf2_run_rec
    {
        UInt16 cf2_run_number;
        public fixed byte cf2_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cf2_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte cf2_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double cf2_run_count_time;
        double cf2_run_singles;
        double cf2_run_scaler1;
        double cf2_run_scaler2;
        double cf2_run_reals_plus_acc;
        double cf2_run_acc;
        fixed double cf2_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double cf2_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double cf2_run_singles_rate;
        double cf2_run_doubles_rate;
        double cf2_run_triples_rate;
        double cf2_run_scaler1_rate;
        double cf2_run_scaler2_rate;
        double cf2_run_multiplicity_mult;
        double cf2_run_multiplicity_alpha;
        double cf2_multiplicity_efficiency;
        double cf2_run_mass;
        double cf2_high_voltage;
        fixed double cf2_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cf3_run_rec
    {
        UInt16 cf3_run_number;
        public fixed byte cf3_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cf3_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte cf3_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double cf3_run_count_time;
        double cf3_run_singles;
        double cf3_run_scaler1;
        double cf3_run_scaler2;
        double cf3_run_reals_plus_acc;
        double cf3_run_acc;
        fixed double cf3_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double cf3_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double cf3_run_singles_rate;
        double cf3_run_doubles_rate;
        double cf3_run_triples_rate;
        double cf3_run_scaler1_rate;
        double cf3_run_scaler2_rate;
        double cf3_run_multiplicity_mult;
        double cf3_run_multiplicity_alpha;
        double cf3_multiplicity_efficiency;
        double cf3_run_mass;
        double cf3_high_voltage;
        fixed double cf3_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cf4_run_rec
    {
        UInt16 cf4_run_number;
        public fixed byte cf4_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cf4_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte cf4_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double cf4_run_count_time;
        double cf4_run_singles;
        double cf4_run_scaler1;
        double cf4_run_scaler2;
        double cf4_run_reals_plus_acc;
        double cf4_run_acc;
        fixed double cf4_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double cf4_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double cf4_run_singles_rate;
        double cf4_run_doubles_rate;
        double cf4_run_triples_rate;
        double cf4_run_scaler1_rate;
        double cf4_run_scaler2_rate;
        double cf4_run_multiplicity_mult;
        double cf4_run_multiplicity_alpha;
        double cf4_multiplicity_efficiency;
        double cf4_run_mass;
        double cf4_high_voltage;
        fixed double cf4_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cf5_run_rec
    {
        UInt16 cf5_run_number;
        public fixed byte cf5_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte cf5_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte cf5_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double cf5_run_count_time;
        double cf5_run_singles;
        double cf5_run_scaler1;
        double cf5_run_scaler2;
        double cf5_run_reals_plus_acc;
        double cf5_run_acc;
        fixed double cf5_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double cf5_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double cf5_run_singles_rate;
        double cf5_run_doubles_rate;
        double cf5_run_triples_rate;
        double cf5_run_scaler1_rate;
        double cf5_run_scaler2_rate;
        double cf5_run_multiplicity_mult;
        double cf5_run_multiplicity_alpha;
        double cf5_multiplicity_efficiency;
        double cf5_run_mass;
        double cf5_high_voltage;
        fixed double cf5_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a1_run_rec
    {
        UInt16 a1_run_number;
        public fixed byte a1_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a1_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a1_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a1_run_count_time;
        double a1_run_singles;
        double a1_run_scaler1;
        double a1_run_scaler2;
        double a1_run_reals_plus_acc;
        double a1_run_acc;
        fixed double a1_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a1_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a1_run_singles_rate;
        double a1_run_doubles_rate;
        double a1_run_triples_rate;
        double a1_run_scaler1_rate;
        double a1_run_scaler2_rate;
        double a1_run_multiplicity_mult;
        double a1_run_multiplicity_alpha;
        double a1_multiplicity_efficiency;
        double a1_run_mass;
        double a1_high_voltage;
        fixed double a1_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a2_run_rec
    {
        UInt16 a2_run_number;
        public fixed byte a2_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a2_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a2_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a2_run_count_time;
        double a2_run_singles;
        double a2_run_scaler1;
        double a2_run_scaler2;
        double a2_run_reals_plus_acc;
        double a2_run_acc;
        fixed double a2_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a2_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a2_run_singles_rate;
        double a2_run_doubles_rate;
        double a2_run_triples_rate;
        double a2_run_scaler1_rate;
        double a2_run_scaler2_rate;
        double a2_run_multiplicity_mult;
        double a2_run_multiplicity_alpha;
        double a2_multiplicity_efficiency;
        double a2_run_mass;
        double a2_high_voltage;
        fixed double a2_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a3_run_rec
    {
        UInt16 a3_run_number;
        public fixed byte a3_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a3_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a3_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a3_run_count_time;
        double a3_run_singles;
        double a3_run_scaler1;
        double a3_run_scaler2;
        double a3_run_reals_plus_acc;
        double a3_run_acc;
        fixed double a3_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a3_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a3_run_singles_rate;
        double a3_run_doubles_rate;
        double a3_run_triples_rate;
        double a3_run_scaler1_rate;
        double a3_run_scaler2_rate;
        double a3_run_multiplicity_mult;
        double a3_run_multiplicity_alpha;
        double a3_multiplicity_efficiency;
        double a3_run_mass;
        double a3_high_voltage;
        fixed double a3_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a4_run_rec
    {
        UInt16 a4_run_number;
        public fixed byte a4_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a4_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a4_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a4_run_count_time;
        double a4_run_singles;
        double a4_run_scaler1;
        double a4_run_scaler2;
        double a4_run_reals_plus_acc;
        double a4_run_acc;
        fixed double a4_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a4_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a4_run_singles_rate;
        double a4_run_doubles_rate;
        double a4_run_triples_rate;
        double a4_run_scaler1_rate;
        double a4_run_scaler2_rate;
        double a4_run_multiplicity_mult;
        double a4_run_multiplicity_alpha;
        double a4_multiplicity_efficiency;
        double a4_run_mass;
        double a4_high_voltage;
        fixed double a4_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a5_run_rec
    {
        UInt16 a5_run_number;
        public fixed byte a5_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a5_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a5_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a5_run_count_time;
        double a5_run_singles;
        double a5_run_scaler1;
        double a5_run_scaler2;
        double a5_run_reals_plus_acc;
        double a5_run_acc;
        fixed double a5_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a5_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a5_run_singles_rate;
        double a5_run_doubles_rate;
        double a5_run_triples_rate;
        double a5_run_scaler1_rate;
        double a5_run_scaler2_rate;
        double a5_run_multiplicity_mult;
        double a5_run_multiplicity_alpha;
        double a5_multiplicity_efficiency;
        double a5_run_mass;
        double a5_high_voltage;
        fixed double a5_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct a6_run_rec
    {
        UInt16 a6_run_number;
        public fixed byte a6_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte a6_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte a6_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double a6_run_count_time;
        double a6_run_singles;
        double a6_run_scaler1;
        double a6_run_scaler2;
        double a6_run_reals_plus_acc;
        double a6_run_acc;
        fixed double a6_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double a6_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double a6_run_singles_rate;
        double a6_run_doubles_rate;
        double a6_run_triples_rate;
        double a6_run_scaler1_rate;
        double a6_run_scaler2_rate;
        double a6_run_multiplicity_mult;
        double a6_run_multiplicity_alpha;
        double a6_multiplicity_efficiency;
        double a6_run_mass;
        double a6_high_voltage;
        fixed double a6_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b1_run_rec
    {
        UInt16 b1_run_number;
        public fixed byte b1_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b1_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b1_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b1_run_count_time;
        double b1_run_singles;
        double b1_run_scaler1;
        double b1_run_scaler2;
        double b1_run_reals_plus_acc;
        double b1_run_acc;
        fixed double b1_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b1_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b1_run_singles_rate;
        double b1_run_doubles_rate;
        double b1_run_triples_rate;
        double b1_run_scaler1_rate;
        double b1_run_scaler2_rate;
        double b1_run_multiplicity_mult;
        double b1_run_multiplicity_alpha;
        double b1_multiplicity_efficiency;
        double b1_run_mass;
        double b1_high_voltage;
        fixed double b1_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b2_run_rec
    {
        UInt16 b2_run_number;
        public fixed byte b2_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b2_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b2_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b2_run_count_time;
        double b2_run_singles;
        double b2_run_scaler1;
        double b2_run_scaler2;
        double b2_run_reals_plus_acc;
        double b2_run_acc;
        fixed double b2_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b2_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b2_run_singles_rate;
        double b2_run_doubles_rate;
        double b2_run_triples_rate;
        double b2_run_scaler1_rate;
        double b2_run_scaler2_rate;
        double b2_run_multiplicity_mult;
        double b2_run_multiplicity_alpha;
        double b2_multiplicity_efficiency;
        double b2_run_mass;
        double b2_high_voltage;
        fixed double b2_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b3_run_rec
    {
        UInt16 b3_run_number;
        public fixed byte b3_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b3_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b3_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b3_run_count_time;
        double b3_run_singles;
        double b3_run_scaler1;
        double b3_run_scaler2;
        double b3_run_reals_plus_acc;
        double b3_run_acc;
        fixed double b3_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b3_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b3_run_singles_rate;
        double b3_run_doubles_rate;
        double b3_run_triples_rate;
        double b3_run_scaler1_rate;
        double b3_run_scaler2_rate;
        double b3_run_multiplicity_mult;
        double b3_run_multiplicity_alpha;
        double b3_multiplicity_efficiency;
        double b3_run_mass;
        double b3_high_voltage;
        fixed double b3_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b4_run_rec
    {
        UInt16 b4_run_number;
        public fixed byte b4_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b4_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b4_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b4_run_count_time;
        double b4_run_singles;
        double b4_run_scaler1;
        double b4_run_scaler2;
        double b4_run_reals_plus_acc;
        double b4_run_acc;
        fixed double b4_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b4_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b4_run_singles_rate;
        double b4_run_doubles_rate;
        double b4_run_triples_rate;
        double b4_run_scaler1_rate;
        double b4_run_scaler2_rate;
        double b4_run_multiplicity_mult;
        double b4_run_multiplicity_alpha;
        double b4_multiplicity_efficiency;
        double b4_run_mass;
        double b4_high_voltage;
        fixed double b4_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b5_run_rec
    {
        UInt16 b5_run_number;
        public fixed byte b5_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b5_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b5_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b5_run_count_time;
        double b5_run_singles;
        double b5_run_scaler1;
        double b5_run_scaler2;
        double b5_run_reals_plus_acc;
        double b5_run_acc;
        fixed double b5_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b5_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b5_run_singles_rate;
        double b5_run_doubles_rate;
        double b5_run_triples_rate;
        double b5_run_scaler1_rate;
        double b5_run_scaler2_rate;
        double b5_run_multiplicity_mult;
        double b5_run_multiplicity_alpha;
        double b5_multiplicity_efficiency;
        double b5_run_mass;
        double b5_high_voltage;
        fixed double b5_spare[INCC.NUMBER_RUN_SPARES];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct b6_run_rec
    {
        UInt16 b6_run_number;
        public fixed byte b6_run_date[INCC.DATE_TIME_LENGTH];
        public fixed byte b6_run_time[INCC.DATE_TIME_LENGTH];
        public fixed byte b6_run_tests[INCC.MAX_RUN_TESTS_LENGTH];
        double b6_run_count_time;
        double b6_run_singles;
        double b6_run_scaler1;
        double b6_run_scaler2;
        double b6_run_reals_plus_acc;
        double b6_run_acc;
        fixed double b6_run_mult_reals_plus_acc[INCC.MULTI_ARRAY_SIZE];
        fixed double b6_run_mult_acc[INCC.MULTI_ARRAY_SIZE];
        double b6_run_singles_rate;
        double b6_run_doubles_rate;
        double b6_run_triples_rate;
        double b6_run_scaler1_rate;
        double b6_run_scaler2_rate;
        double b6_run_multiplicity_mult;
        double b6_run_multiplicity_alpha;
        double b6_multiplicity_efficiency;
        double b6_run_mass;
        double b6_high_voltage;
        fixed double b6_spare[INCC.NUMBER_RUN_SPARES];
    };
    */
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct add_a_source_setup_rec
    {
        public UInt16 ad_type;
        public Int16 ad_port_number;
        public double ad_forward_over_travel;
        public double ad_reverse_over_travel;
        public UInt16 ad_number_positions;
        public fixed double ad_dist_to_move[INCC.MAX_ADDASRC_POSITIONS];
        public double cm_steps_per_inch;
        public UInt32 cm_forward_mask;
        public UInt32 cm_reverse_mask;
        public short cm_axis_number;
        public UInt32 cm_over_travel_state;
        public double cm_step_ratio;
        public double cm_slow_inches;
        public double plc_steps_per_inch;
        public double scale_conversion_factor;
        public byte cm_rotation;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct stratum_id_rec
    {
        public fixed byte stratum[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte stratum_id_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        double stratum_bias_uncertainty;
        double stratum_random_uncertainty;
        double stratum_systematic_uncertainty;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct de_mult_rec
    {
        public fixed double de_neutron_energy[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_detector_efficiency[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_inner_outer_ring_ratio[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_relative_fission[INCC.MAX_DUAL_ENERGY_ROWS];
        public double de_inner_ring_efficiency;
        public double de_outer_ring_efficiency;
        public fixed byte de_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte de_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct results_de_mult_rec : iresultsbase
    {
        public double de_meas_ring_ratio;
        public double de_interpolated_neutron_energy;
        public double de_energy_corr_factor;
        public fixed double de_neutron_energy_res[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_detector_efficiency_res[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_inner_outer_ring_ratio_res[INCC.MAX_DUAL_ENERGY_ROWS];
        public fixed double de_relative_fission_res[INCC.MAX_DUAL_ENERGY_ROWS];
        public double de_inner_ring_efficiency_res;
        public double de_outer_ring_efficiency_res;
        public fixed byte de_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte de_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method
    {
        public fixed byte item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte analysis_method_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_cal_curve
    {
        public fixed byte cal_curve_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte cal_curve_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_active
    {
        public fixed byte active_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_collar
    {
        public fixed byte collar_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        byte collar_mode;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_collar_detector
    {
        public fixed byte collar_detector_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        byte collar_detector_mode;
        public fixed byte collar_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_collar_k5
    {
        public fixed byte collar_k5_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        byte collar_k5_mode;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_known_alpha
    {
        public fixed byte known_alpha_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte known_alpha_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_known_m
    {
        public fixed byte known_m_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte known_m_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe public struct analysis_method_multiplicity
    {
        public fixed byte multiplicity_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte multiplicity_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_active_passive
    {
        public fixed byte active_passive_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_passive_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_add_a_source
    {
        public fixed byte add_a_source_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte add_a_source_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_active_mult
    {
        public fixed byte active_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte active_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_curium_ratio
    {
        public fixed byte curium_ratio_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte curium_ratio_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_truncated_mult
    {
        public fixed byte truncated_mult_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte truncated_mult_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct meas_id
    {
        public fixed byte meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte meas_time[INCC.DATE_TIME_LENGTH];
        public fixed byte filename[INCC.FILE_NAME_LENGTH];
        public fixed byte results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct report_id
    {
        public fixed byte results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte stratum_id[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte item_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte meas_time[INCC.DATE_TIME_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct stratum_report_id
    {
        public fixed byte stratum_id[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte item_id[INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte meas_date[INCC.DATE_TIME_LENGTH];
        public fixed byte meas_time[INCC.DATE_TIME_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct detector_item_id
    {
        public fixed byte results_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
        public fixed byte item_id[INCC.MAX_ITEM_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct stratum_id_key
    {
        public fixed byte stratum[INCC.MAX_STRATUM_ID_LENGTH];
        public fixed byte stratum_id_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct analysis_method_de_mult
    {
        public fixed byte de_item_type[INCC.MAX_ITEM_TYPE_LENGTH];
        public fixed byte de_detector_id[INCC.MAX_DETECTOR_ID_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct cm_pu_ratio_parms
    {
        public fixed byte item_id[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cm_id_label[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cm_id[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed byte cm_input_batch_id[INCC.MAX_ITEM_IDS * INCC.MAX_ITEM_ID_LENGTH];
        public fixed double cm_dcl_pu_mass[INCC.MAX_ITEM_IDS];
        public fixed double cm_dcl_u_mass[INCC.MAX_ITEM_IDS];
        public fixed double cm_dcl_u235_mass[INCC.MAX_ITEM_IDS];
        public fixed double cm_pu_ratio[INCC.MAX_ITEM_IDS];
        public fixed double cm_pu_ratio_err[INCC.MAX_ITEM_IDS];
        public fixed byte cm_pu_ratio_date[INCC.MAX_ITEM_IDS * INCC.DATE_TIME_LENGTH];
        public fixed double cm_u_ratio[INCC.MAX_ITEM_IDS];
        public fixed double cm_u_ratio_err[INCC.MAX_ITEM_IDS];
        public fixed byte cm_u_ratio_date[INCC.MAX_ITEM_IDS * INCC.DATE_TIME_LENGTH];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct old_results_rec
    {
        public fixed byte meas_date[9];
        public fixed byte meas_time[9];
        public fixed byte filename[13];
        public fixed byte original_meas_date[9];
        public fixed byte results_facility[5];
        public fixed byte results_facility_description[21];
        public fixed byte results_mba[5];
        public fixed byte results_mba_description[21];
        public fixed byte item_id[13];
        public fixed byte stratum_id[6];
        public fixed byte stratum_id_description[21];
        public fixed byte results_campaign_id[4];
        public fixed byte results_inspection_number[4];
        public fixed byte results_item_type[6];
        public byte results_collar_mode;
        public fixed byte results_detector_id[12];
        public fixed byte results_detector_type[5];
        public fixed byte results_electronics_id[9];
        public fixed byte results_glovebox_id[21];
        public UInt16 results_num_rows;
        public UInt16 results_num_columns;
        public double results_distance;
        public double bias_uncertainty;
        public double random_uncertainty;
        public double systematic_uncertainty;
        public double relative_std_dev;
        public byte completed;
        public byte meas_option;
        public fixed byte inventory_change_code[3];
        public fixed byte io_code[2];
        public byte well_config;
        public byte data_source;
        public byte results_qc_tests;
        public UInt16 error_calc_method;
        public byte results_print;
        public fixed byte user_id[41];
        public fixed byte comment[51];
        public fixed byte ending_comment[51];
        public double item_pu238;
        public double item_pu239;
        public double item_pu240;
        public double item_pu241;
        public double item_pu242;
        public double item_am241;
        public double item_pu238_err;
        public double item_pu239_err;
        public double item_pu240_err;
        public double item_pu241_err;
        public double item_pu242_err;
        public double item_am241_err;
        public fixed byte item_pu_date[9];
        public fixed byte item_am_date[9];
        public fixed byte item_isotopics_id[13];
        public fixed byte item_isotopics_source_code[3];
        public double normalization_constant;
        public double normalization_constant_err;
        public double results_predelay;
        public double results_gate_length;
        public double results_gate_length2;
        public double results_high_voltage;
        public double results_die_away_time;
        public double results_efficiency;
        public double results_multiplicity_deadtime;
        public double results_coeff_a_deadtime;
        public double results_coeff_b_deadtime;
        public double results_coeff_c_deadtime;
        public double results_doubles_gate_fraction;
        public double results_triples_gate_fraction;
        public double r_acc_sngl_test_rate_limit;
        public double r_acc_sngl_test_precision_limit;
        public double r_acc_sngl_test_outlier_limit;
        public double r_outlier_test_limit;
        public double r_bkg_doubles_rate_limit;
        public double r_bkg_triples_rate_limit;
        public double r_chisq_limit;
        public UInt16 r_max_num_failures;
        public double r_high_voltage_test_limit;
        public double passive_bkg_singles_rate;
        public double passive_bkg_singles_rate_err;
        public double passive_bkg_doubles_rate;
        public double passive_bkg_doubles_rate_err;
        public double passive_bkg_triples_rate;
        public double passive_bkg_triples_rate_err;
        public double active_bkg_singles_rate;
        public double active_bkg_singles_rate_err;
        public double passive_bkg_scaler1_rate;
        public double passive_bkg_scaler2_rate;
        public double active_bkg_scaler1_rate;
        public double active_bkg_scaler2_rate;
        fixed char error_msg_codes[10 * 81];
        fixed char warning_msg_codes[10 * 81];
        public UInt16 total_number_runs;
        public UInt16 number_good_runs;
        public double total_good_count_time;
        public double singles_sum;
        public double scaler1_sum;
        public double scaler2_sum;
        public double reals_plus_acc_sum;
        public double acc_sum;
        public fixed double mult_reals_plus_acc_sum[128];
        public fixed double mult_acc_sum[128];
        public double singles;
        public double singles_err;
        public double doubles;
        public double doubles_err;
        public double triples;
        public double triples_err;
        public double scaler1;
        public double scaler1_err;
        public double scaler2;
        public double scaler2_err;
        public double uncorrected_doubles;
        public double uncorrected_doubles_err;
        public double singles_multi;
        public double doubles_multi;
        public double triples_multi;
        public double declared_mass;
        public byte primary_analysis_method;
        public double net_drum_weight;
        public fixed byte passive_meas_date[9];
        public fixed byte passive_meas_time[9];
        public fixed byte passive_filename[13];
        public fixed byte passive_results_detector_id[12];
        public fixed byte active_meas_date[9];
        public fixed byte active_meas_time[9];
        public fixed byte active_filename[13];
        public fixed byte active_results_detector_id[12];
        public fixed double covariance_matrix[9];
        public double r_normal_backup_assay_test_lim;
        public double r_max_runs_for_outlier_test;
        public double r_checksum_test;
        public double results_accidentals_method;
        public fixed double results_spares[96];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct old_detector_rec
    {
        public fixed byte detector_id[12];
        public fixed byte detector_type[5];
        public fixed byte electronics_id[9];
    };

    # endregion the INCC structs
}