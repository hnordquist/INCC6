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
using System.Collections.Generic;
using System.Reflection;
using AnalysisDefs;

namespace DetectorDefs
{

    public enum InstrType
    {
        Unknown,
        /*INCC*/
        JSR11, SR4, JSR12, HNC, JSR11A, MSR4, MSR4A, PSR, DGSR, AMSR,
        JSR15,// aka HHMR, order taken from sr.h
        UNAP, // aka JSR16, the 40k box
        /* LM */
        NPOD, LMMM, LMI = LMMM, PTR32, MCA527,
        /* Simulated */
        MCNPX, N1
    };  // note: only LMI used for older N-1 data, now the files say "LMMM" and we also find an occasional NPOD

    public static class InstrTypeExtensions
    {
        public static bool HasFA(this InstrType itype)
        {
            return itype >= InstrType.AMSR;
        }

        public static FAType DefaultFAFor(this InstrType itype)
        {
            return itype >= InstrType.AMSR ? FAType.FAOn : FAType.FAOff;
        }

        /// <summary>
        /// orignally matched only on DGSR and AMSR, added HHMR/JSR15 and UNAP
        /// </summary>
        /// <param name="itype"></param>
        /// <returns></returns>
        public static bool isDG_AMSR_Match(this InstrType itype)
        {
            return itype >= InstrType.DGSR && itype <= InstrType.UNAP;
        }

        /// <summary>
        /// AMSR selected, or faux-AMSR via UNAP/JSR15 choices
        /// </summary>
        /// <param name="itype"></param>
        /// <returns></returns>
        public static bool isVirtualAMSR(this InstrType itype)
        {
            return itype >= InstrType.AMSR && itype <= InstrType.UNAP;
        }

        /// <summary>
        /// These modern SRs have 512 multiplicity
        /// </summary>
        /// <param name="itype"></param>
        /// <returns></returns>
        public static bool BigLoveMultiplicity(this InstrType itype)
        {
            return (itype == InstrType.JSR15 || itype == InstrType.UNAP);
        }

        public static bool IsListMode(this InstrType itype)
        {
            return itype >= InstrType.NPOD && itype <= InstrType.MCA527;
        }

        public static bool IsSocketBasedLM(this InstrType itype)
        {
            return itype == InstrType.NPOD || itype == InstrType.LMMM || itype == InstrType.MCA527;
        }
        public static bool IsUSBBasedLM(this InstrType itype)
        {
            return itype == InstrType.PTR32;
        }

        public static bool IsCOMPortBasedSR(this InstrType itype)
        {
            return itype <= InstrType.JSR15;
        }

        public static bool IsSRWithVariableBaudRate(this InstrType itype)
        {
            return itype < InstrType.NPOD && itype >= InstrType.JSR15;
        }

		public static bool CanDoTriples(this InstrType itype)
		{
            return itype >= InstrType.MSR4A;
		}

        /// INCC5 merges every known SR type into a short list of 6, to match sr lib
        /// This function adds the JSR-15 to the list
        /// MSR4 or 2150, JSR-11, JSR-12, PSR or ISR, DGSR, AMSR, JSR-15
        /// MSR4          JSR11   JSR12   PSR         DGSR  AMSR   JSR15
        public static string INCC5ComboBoxString(this InstrType itype)
        {
            string s = string.Empty;
            switch (itype)
            {
                case InstrType.MSR4A:
                    s = "MSR4 or 2150";
                    break;
                case InstrType.JSR11:
                    s = "JSR-11";
                    break;
                case InstrType.JSR12:
                    s = "JSR-12";
                    break;
                case InstrType.JSR15:
                    s = "JSR-15";
                    break;
                case InstrType.PSR:
                    s = "PSR or ISR";
                    break;
                default:
                    s = itype.ToString();
                    break;
            }
            return s;
        }

        public static InstrType INCC5ComboBoxStringToType(string s)
        {
            InstrType i = InstrType.JSR15;
            if (!Enum.TryParse(s, out i))
            {
                if (s.CompareTo(INCC5ComboBoxString(InstrType.JSR12)) == 0)
                    i = InstrType.JSR12;
                else if (s.CompareTo(INCC5ComboBoxString(InstrType.JSR11)) == 0)
                    i = InstrType.JSR11;
                else if (s.CompareTo(INCC5ComboBoxString(InstrType.PSR)) == 0)
                    i = InstrType.PSR;
                if (s.CompareTo(INCC5ComboBoxString(InstrType.JSR15)) == 0)
                    i = InstrType.JSR15;
                else if (s.CompareTo(INCC5ComboBoxString(InstrType.MSR4A)) == 0)
                    i = InstrType.MSR4A;
            }

            return i;
        }

    }
 

    public enum ConstructedSource
    {
        Unknown = -1, Live = 0, DB, CycleFile, Manual, ReviewFile, // traditional INCC 
        NCDFile, PTRFile, MCA527File, SortedPulseTextFile,// List Mode file inputs 
        INCCTransferCopy, INCCTransfer, Reanalysis /* Reanalysis flag */ /*, SRDayFile  experimental value */
    };  // sources of data: file, DAQ, DB



    public static class ConstructedSourceExtensions
    {
        public static bool Live(this ConstructedSource src)
        {
            return (src == ConstructedSource.Live);
        }
        public static bool AcquireChoices(this ConstructedSource src)
        {
            return src >= ConstructedSource.Live && src <= ConstructedSource.ReviewFile;
        }
        public static bool INCC5FileData(this ConstructedSource src)
        {
            return src == ConstructedSource.DB || src == ConstructedSource.CycleFile || src == ConstructedSource.ReviewFile || src == ConstructedSource.Manual;
        }
        public static bool INCCTransferData(this ConstructedSource src)
        {
            return src == ConstructedSource.INCCTransferCopy || src == ConstructedSource.INCCTransfer;
        }
        public static bool SRDAQ(this ConstructedSource src, InstrType device)
        {
            return ((src == ConstructedSource.Live) && (device <= InstrType.UNAP)); // it is a Live SR DAQ
        }
		public static bool MightHaveScalerData(this ConstructedSource src, InstrType device)		
        {
            return (device <= InstrType.UNAP) && 
					(src.INCC5FileData() || src.INCCTransferData() || src == ConstructedSource.Reanalysis);
        }
        /// <summary>
        ///  if this combination of data source and specific device point to a virtual SR return true o.w. false
        /// </summary>
        /// <param name="src"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool UsingVirtualSRCounting(this ConstructedSource src, InstrType device)
        {
            bool needsAdditionalSpecification =
               ((src == ConstructedSource.Live &&
                (device >= InstrType.NPOD && device <= InstrType.MCA527)) // it is a Live LM DAQ, or
             ||
               (src >= ConstructedSource.NCDFile && src <= ConstructedSource.SortedPulseTextFile));  // data from other source and the processing went through the raw counting code 
            return needsAdditionalSpecification;
        }

        public static bool LMFiles(this ConstructedSource src, InstrType device)
        {
            bool ack = (src >= ConstructedSource.NCDFile && src <= ConstructedSource.SortedPulseTextFile) && device.IsListMode();  // data from other source and the processing went through the raw counting code 
            return ack;
        }

		public static double TimeBase(this ConstructedSource src, InstrType device)
		{
			double te = 1e-7;  // JSR files, LMMM and MCA-527 use 100 ns units, but MCA-527 can change 
			switch (device)
			{
			case InstrType.PTR32:
			case InstrType.MCNPX:
			case InstrType.N1:
				te = 1e-8;
				break;
			}

			switch (src) // src takes precedence over the inst type
			{
			case ConstructedSource.PTRFile:
			case ConstructedSource.SortedPulseTextFile:
				te = 1e-8;
				break;
			case ConstructedSource.NCDFile:
			case ConstructedSource.MCA527File: // LMMM and MCA-527 use 100 ns units, but MCA-527 can change (a header field value) 
				te = 1e-7;
				break;
			}
			return te;
		}

		static Dictionary<ConstructedSource, string> PrettyName;

        static ConstructedSourceExtensions()
        {
            if (PrettyName == null)
            {
                PrettyName = new Dictionary<ConstructedSource, string>();
                PrettyName.Add(ConstructedSource.CycleFile, "Disk file");
                PrettyName.Add(ConstructedSource.PTRFile, "PTR-32 data file pair");
                PrettyName.Add(ConstructedSource.SortedPulseTextFile, "Pulse file");
                PrettyName.Add(ConstructedSource.MCA527File, "MCA-527 data file");
                PrettyName.Add(ConstructedSource.DB, "Database");
                PrettyName.Add(ConstructedSource.Manual, "Manual entry");
                PrettyName.Add(ConstructedSource.ReviewFile, "Review disk file");
                PrettyName.Add(ConstructedSource.NCDFile, "LMMM data file");
                PrettyName.Add(ConstructedSource.Live, "Shift register");
                PrettyName.Add(ConstructedSource.INCCTransfer, "Transfer");
                PrettyName.Add(ConstructedSource.INCCTransferCopy, "Transfer Copy");
                PrettyName.Add(ConstructedSource.Unknown, "Unknown");
                PrettyName.Add(ConstructedSource.Reanalysis, "Reanalysis");
            }
		}

        public static string HappyFunName(this ConstructedSource src)
        {
            return PrettyName[src];
        }

		public static string ListModeLiveName =  "List mode device";

		public static string NameForViewing(this ConstructedSource src, InstrType device)
        {
			if (device.IsListMode() && src.Live())
			{
				return ListModeLiveName;
			}
			else
				return PrettyName[src];
        }

        public static ConstructedSource SrcToEnum(this string src)
        {
            foreach (KeyValuePair<ConstructedSource, string> pair in PrettyName)
            {
                if (pair.Value.Equals(src))
                    return pair.Key;
            }
			if (string.Compare(ListModeLiveName, src, true) == 0)
			{
				return ConstructedSource.Live;
			}
            return ConstructedSource.Unknown;
        }

    }


    /// <summary>
    /// Shift Register serial port details, List Mode ethernet details ride on a subclass for LM devices.
    /// This class has a port number as string (serial or ethernet),
    /// a default delay/wait time for use where needed by any protocol,
    /// and an outcast entry only for serial port use, the baud rate.
    /// Traditionally the Shift Register baud rate is hardcoded at 9600 (300 for JSR11) in the glorious API to the sr32 library, 
    /// but for modern devices the code needs an override to maxout the baud rate.
    /// 
    /// </summary>
    public class ConnectionInfo : IEquatable<ConnectionInfo>
    {
        public ConnectionInfo()
        {
            Port = string.Empty;
            Wait = 500;
            Baud = 9600;
        }
        public ConnectionInfo(ConnectionInfo src)
        {
            if (src != null)
            {
                Port = string.Copy(src.Port);
                Wait = src.Wait;
                Baud = src.Baud;
            }
            else
            {
                Port = string.Empty;
                Wait = 500;
                Baud = 9600;
            }
        }
        public string Port { get; set; }

        /// <summary>
        /// millisecond protocol delay value, 500 default
        /// </summary>
        public int Wait { set; get; }

        /// <summary>
        /// Baud rate for current Shift Register instrument, 9600 for the old and true, whatever for the new and lame
        /// </summary>
        public int Baud { set; get; }

        public bool Equals(ConnectionInfo other)
        {
            return (other != null) && Port.Equals(other.Port) && Baud == other.Baud && Wait == other.Wait;
        }

        public override string ToString()
        {
            return Port.ToString();
        }

        public static int Compare(ConnectionInfo x, ConnectionInfo y)
        {
            int res = 0;
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (res == 0)
                res = x.Port.CompareTo(y.Port);
            if (res == 0)
                res = x.Baud.CompareTo(y.Baud);
            if (res == 0)
                res = x.Wait.CompareTo(y.Wait);

            return res;
        }

        public int CompareTo(ConnectionInfo other)
        {
            return Compare(this, other);
        }
    }

    public class LMConnectionInfo : ConnectionInfo, IEquatable<LMConnectionInfo>
    {
        public LMMMNetComm NetComm { get; set; }

        public LMMMConfig DeviceConfig { get; set; }

        public LMConnectionInfo()
            : base()
        {
            NetComm = new LMMMNetComm();
            DeviceConfig = new LMMMConfig();
        }

        public LMConnectionInfo(LMConnectionInfo src)
            : base(src)
        {
            if (src != null)
            {
                NetComm = new LMMMNetComm(src.NetComm);
                DeviceConfig = new LMMMConfig(src.DeviceConfig);
            }
            else
            {
                NetComm = new LMMMNetComm();
                DeviceConfig = new LMMMConfig();
            }
            Wait = NetComm.Wait;
        }

        public bool Equals(LMConnectionInfo other)
        {
            return base.Equals(other);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int CompareTo(LMConnectionInfo other)
        {
            return base.CompareTo(other);
        }
    }

    /// <summary>
    /// Information about a generic instrument or file data source
    /// The "name" of specific detectors, for instance
    /// </summary>
    public class DataSourceIdentifier : ParameterBase, IEquatable<DataSourceIdentifier>, IComparable<DataSourceIdentifier>
    {
        public string DetectorId
        {
            get { return iname; }
            set { iname = value; }
        }

        public string DetectorName
        {
            get { return iname; }
            set { iname = value; }
        }

        public string ElectronicsId
        {
            get { return elecid; }
            set { elecid = value; }
        }

        public InstrType SRType
        {
            get { return srtype; }
            set { srtype = value; }
        }
        public void SetSRType(string v)
        {
            bool b = Enum.TryParse(v, out srtype);
            if (!b) srtype = InstrType.AMSR;
        }

        public int version; // not used

        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }
        public string ConnInfo
        {
            get { return conninfo.ToString(); }
            set { conninfo.Port = value; }
        }
        public ConnectionInfo FullConnInfo
        {
            get { return conninfo; }
            set { conninfo = value; }
        }
        // Originally a text field from INCC record with ambiguous content
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        ///  Raw serial port number; To use with SRLib, the number must be decremented by one
        /// </summary>
        public int SerialPort
        {
            get { int p = 0; int.TryParse(conninfo.Port, out p); return p; }
            set { conninfo.Port = value.ToString(); }
        }
        public int BaudRate
        {
            get { return conninfo.Baud; }
            set { if (value <= 0) value = 9600; conninfo.Baud = value; }
        }
        private string elecid;
        private string iname;
        private string type;
        private string desc;
        private InstrType srtype;
        private string filename;
        private ConnectionInfo conninfo; // ip and port, or serial port, e.g.
        public DateTimeOffset dt;
        public ConstructedSource source = ConstructedSource.Live;

		/// <summary>
		/// Returns a combined identification string meant to include data source, detector name and type
		/// </summary>
		/// <returns></returns>
		public string Identifier() 
		{
			string l = string.IsNullOrEmpty(filename) ? "*" : filename;
			switch (source)
			{
			case ConstructedSource.Live:
				l = iname + "-" + srtype.ToString() + (string.IsNullOrEmpty(ConnInfo) ? "" : "[" + ConnInfo + "]");
				break;
			case ConstructedSource.NCDFile:
			case ConstructedSource.SortedPulseTextFile:
			case ConstructedSource.PTRFile:
			case ConstructedSource.MCA527File:
				break;
			case ConstructedSource.INCCTransfer:
				l += " (INCC transfer file, recalculated)";
				break;
			case ConstructedSource.INCCTransferCopy:
				l += " (INCC transfer file)";
				break;
			case ConstructedSource.CycleFile:
				l += " (INCC5 test data file)";
				break;
			case ConstructedSource.ReviewFile:
				l += " (INCC Rad Review measurement data file)";
				break;
			case ConstructedSource.DB:
				l = "DB Acquire";
				break;
			case ConstructedSource.Manual:
				l = "Manual";
				break;
			case ConstructedSource.Reanalysis:
				l += (" " + source.HappyFunName());
				break;
			}
			return l;
		}

		public string SourceIdentName()  // devnote: used to gen output files, so make sure no offending chars are included
		{
			string l = "Unknown";
				switch (source)
				{
				case ConstructedSource.Live:
					l = iname + "-" + srtype.ToString() + (string.IsNullOrEmpty(ConnInfo) ? "" : "[" + ConnInfo + "]");
					break;
				case ConstructedSource.NCDFile:
				case ConstructedSource.SortedPulseTextFile:
				case ConstructedSource.PTRFile:
				case ConstructedSource.MCA527File:
					l = filename;
					break;
				case ConstructedSource.INCCTransfer:
					l = filename + " (INCC transfer file, recalculated)";
					break;
				case ConstructedSource.INCCTransferCopy:
					l = filename + " (INCC transfer file)";
					break;
				case ConstructedSource.CycleFile:
					l = filename + " (INCC5 test data file)";
					break;
				case ConstructedSource.DB:
					l = "DB Acquire";
					break;
				case ConstructedSource.ReviewFile:
					l = filename + " (INCC Rad Review measurement data file)";
					break;
				case ConstructedSource.Manual:
					l = "Manual";
					break;
				case ConstructedSource.Reanalysis:
					l += (" " + source.HappyFunName());
					break;

				}
			return l;
		}
		public DataSourceIdentifier()
        {
            iname = string.Empty;
            elecid = string.Empty;
            filename = string.Empty;
            type = string.Empty;
            desc = string.Empty;
            conninfo = new ConnectionInfo();
            dt = DateTimeOffset.Now;
        }
        public DataSourceIdentifier(DataSourceIdentifier dsid)
        {
            iname = string.Copy(dsid.iname);
            srtype = dsid.srtype;
            version = dsid.version;
            elecid = string.Copy(dsid.elecid);
            filename = string.Copy(dsid.filename);
            dt = new DateTimeOffset(dsid.dt.Ticks, dsid.dt.Offset);
            source = dsid.source;
            desc = string.Copy(dsid.desc);
            type = string.Copy(dsid.type);

            Type t = dsid.conninfo.GetType();
            ConstructorInfo ci = t.GetConstructor(new Type[] { t });
            conninfo = (ConnectionInfo)ci.Invoke(new object[] { dsid.conninfo });

        }

        public void SetIdDetails(string name, string elec, string typedesc, InstrType rtype)
        {
            DetectorId = string.Copy(name); 
            ElectronicsId = string.Copy(elec); 
            Type = string.Copy(typedesc);
            SRType = rtype;
        }

        // strong equality, maybe too strong
        public bool Equals(DataSourceIdentifier other)
        {
            if (other as object != null &&
                iname.Equals(other.iname)
                & srtype.Equals(other.srtype)
                & elecid.Equals(other.elecid)
                & filename.Equals(other.filename)
                & type.Equals(other.type)
                & dt.Equals(other.dt) // dev note: time may not be the same due to in-memory creation compared with fossilized definitions from DB
                & conninfo.Equals(other.conninfo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(DataSourceIdentifier d1, DataSourceIdentifier d2)
        {
            if (d1 as object == null)
            {
                if (d2 as object == null)
                    return true;
                else
                    return d2.Equals(null);
            }
            else
                return d1.Equals(d2);
        }

        public static bool operator !=(DataSourceIdentifier d1, DataSourceIdentifier d2)
        {
            return (!d1.Equals(d2));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is DataSourceIdentifier))
                return false; // throw new InvalidCastException("The 'obj' argument is not a DataSourceIdentifier object.");
            else
                return Equals(obj as DataSourceIdentifier);
        }

        public override int GetHashCode()
        {
            int hCode = iname.GetHashCode() ^ srtype.GetHashCode() ^ elecid.GetHashCode() ^
                filename.GetHashCode() ^ type.GetHashCode() ^ dt.GetHashCode() ^ conninfo.GetHashCode();
            return hCode.GetHashCode();
        }

        public static int Compare(DataSourceIdentifier x, DataSourceIdentifier y)
        {

            int res = x.iname.CompareTo(y.iname);
            if (res == 0)
                res = x.srtype.CompareTo(y.srtype);
            if (res == 0)
                res = x.version.CompareTo(y.version);
            if (res == 0)
                res = x.elecid.CompareTo(y.elecid);
            if (res == 0)
                res = x.filename.CompareTo(y.filename);
            if (res == 0)
                res = x.dt.Ticks.CompareTo(y.dt.Ticks);
            if (res == 0)
                res = x.desc.CompareTo(y.desc);
            //if (res == 0)
            //    res = x.source.CompareTo(y.source);
            if (res == 0)
                res = x.type.CompareTo(y.type);
            if (res == 0)
                res = x.conninfo.CompareTo(y.conninfo);
            return res;
        }

        public int CompareTo(DataSourceIdentifier other)
        {
            return Compare(this, other);
        }

        // entries for the traditional INCC5 table definition only, others ride on SR_Parms_rec, LMHWparams and LMNetConn
        public override void GenParamList()
        {
            base.GenParamList();
            Table = "detectors";
            ps.Add(new DBParamEntry("detector_name", DetectorName));
            ps.Add(new DBParamEntry("electronics_id", ElectronicsId));
            ps.Add(new DBParamEntry("detector_type_freeform", Type));
            ps.Add(new DBParamEntry("detector_type_id", (int)SRType));
            ps.Add(new DBParamEntry("detector_alias", Identifier()));
        }

        public DBParamEntry NewForINCC6Params
        {
            get
            {
                DBParamEntry d = new DBParamEntry("sr_baud", BaudRate);
                return d;
            }
        }
    }


}
