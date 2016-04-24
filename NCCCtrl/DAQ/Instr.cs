/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS5NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
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
using System.Net.Sockets;
using Analysis;
using AnalysisDefs;
using DetectorDefs;
using NCCFile;
using NCCReporter;
namespace Instr
{

    using NC = NCC.CentralizedState;

    public enum DAQInstrState { Offline, Online, ReceivingData, Complete, HVCalib };  // unconnected, connected, spewing tuples at us, finished with something, doing an HV Calib step

    public static class Instruments
    {
        static public ActiveInstrumentList Active = new ActiveInstrumentList(); // "active only" list with methods to pick out active entries only 
        static public List<Instrument> All = Active;   // the complete list active/inactive

        public static bool NativeFA(this InstrType it)
        {
            return (it.isDG_AMSR_Match());
        }

    }

    public class ActiveInstrumentList : List<Instrument>
    {

        public new System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].selected)
                    yield return this[i];
            }
        }

        public Instrument LastActive(System.Type t)
        {
            return this.FindLast(i => { return i.GetType().Equals(t) && i.selected; });
        }
        public Instrument FirstActive(System.Type t)
        {
            return this.Find(i => { return i.GetType().Equals(t) && i.selected; });
        }
        public Instrument FirstActive()
        {
            return this.Find(i => { return i.selected; });
        }
        public bool HasLM()
        {
            return this.Exists(i => { return i is LMInstrument; });
        }
        public bool HasSocketBasedLM()
        {
            return this.Exists(i => { return (i is LMInstrument) && ((LMInstrument)i).SocketBased(); });
        }
		public bool HasMCA()
        {
            return Exists(i => { return (i is LMInstrument) &&  i.id.SRType == InstrType.MCA527; });
        }
		public bool HasLMMM()
        {
            return Exists(i => { return (i is LMInstrument) &&  i.id.SRType == InstrType.LMMM; });
        }
        public bool HasUSBBasedLM()
        {
            return this.Exists(i => { return i is LMInstrument && i.id.SRType.IsUSBBasedLM(); });
        }
        public Instrument FirstLM()
        {
            return this.Find(i => { return (i is LMInstrument); }); 
        }
        public bool HasConnectedLM()
        {
            return this.Exists(i => { return (i is LMInstrument && ((LMInstrument)i).SocketBased() && (i as LMInstrument).DAQState == DAQInstrState.Online); });
        }
        public Instrument AConnectedLM()
        {
            return this.Find(i => { return (i is LMInstrument && ((LMInstrument)i).SocketBased() && (i as LMInstrument).DAQState == DAQInstrState.Online); });
        }
        public bool HasSR()
        {
            return this.Exists(i => { return i is SRInstrument; });
        }
        public int IndexOfActiveInst(Instrument ind)
        {
            if (ind == null) return (-1);
            else return this.IndexOf(ind);
        }

        // get the nth active LM
        public LMInstrument FindByIndexer(int nth)
        {
            int idx = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] is LMInstrument && this[i].selected)
                {
                    if (idx == nth)
                        return (LMInstrument)this[i];
                    idx++;
                }
            }
            return null;
        }

        // get the LM ranking position (0, 1, 2, etc)
        public int RankPositionInList(LMInstrument inst)
        {
            int idx = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] is LMInstrument)
                {
                    if (this[i].Equals(inst))
                        return idx;
                    else
                        idx++;
                }
            }
            return -1;
        }

        public LMInstrument MatchByPort(int Port)
        {
            return (LMInstrument)this.Find(i =>
                                            {
                                                if (i is LMInstrument)
                                                {
                                                    LMInstrument lm = (LMInstrument)i;
                                                    return lm.port == Port;
                                                }
                                                else return false;
                                            }
                                                );
        }

        public SRInstrument MatchByDSID(DataSourceIdentifier id)
        {
            return (SRInstrument)this.Find(i =>
                                            {
                                                if (i is SRInstrument)
                                                {
                                                    SRInstrument sr = (SRInstrument)i;
                                                    return id.Equals(sr);
                                                }
                                                else return false;
                                            }
                                                );
        }

        /// <summary>
        /// enumerate the SRInstruments
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetSREnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] is SRInstrument && this[i].selected)
                    yield return this[i];
            }
        }
        /// <summary>
        /// enumerate the LMInstruments
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetLMEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i] is LMInstrument && this[i].selected)
                    yield return this[i];
            }
        }

        public System.Collections.IEnumerator GetConnectedEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].selected && this[i].DAQState == DAQInstrState.Online)
                    yield return this[i];
            }
        }

        public int ConnectedCount()
        {
            int j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].selected && this[i].DAQState == DAQInstrState.Online)
                    j++;
            }
            return j;
        }

        public int ConnectedLMCount()
        {
            int j = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].selected && this[i] is LMInstrument && this[i].DAQState == DAQInstrState.Online)
                    j++;
            }
            return j;
        }
    }

    // dev note: associate hard-wired channels at the instrument or DSID level. 
    // dev note: A channel should be mappable to an arbitrary detector by the user, despite channels being immutable for specific instruments.
    // dev note: The map as used by the Detector class maps a channel # to the source of a channel <instrument, dsid, timestamp, original channel #>. 
    public abstract class Instrument : IEquatable<Instrument>, IComparable<Instrument>
    {
        public DataSourceIdentifier id;

        private DAQInstrState daqstate;

        public DAQInstrState DAQState
        {
            get { return daqstate; }
            set { daqstate = value; }
        }

        public RDTBase RDT
        {
            get { return rdt; }
            set { rdt = value; }
        }

        public bool selected;

        RDTBase rdt;

        public Instrument()
        {
            id = new DataSourceIdentifier();
            daqstate = DAQInstrState.Online;
            selected = true;
            id.dt = System.DateTime.Now;
        }


        public void PendingComplete()
        {
            rdt.AssayPendingComplete();
        }
        public bool PendingWait()
        {
            rdt.AssayPendingWait(); return true;
        }
        public void PendingReset()
        {
            rdt.AssayPendingReset();
        }
        public bool IsPending()
        {
            if (rdt != null)
                return !rdt.AssayPendingIsSet();
            else
                return false;
        }

        public abstract void Init(LMLoggers.LognLM datalog, LMLoggers.LognLM alog);

        /// <summary>
        /// Connects to the instrument.
        /// </summary>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void Connect()
        {
        }

        /// <summary>
        /// Applies instrument settings.
        /// </summary>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void ApplySettings()
        {
        }

        /// <summary>
        /// Starts an assay operation.
        /// </summary>
        /// <param name="measurement">The measurement.</param>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void StartAssay(Measurement measurement)
        {
        }

        /// <summary>
        /// Stops the currently executing assay operation.
        /// </summary>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void StopAssay()
        {
        }

        /// <summary>
        /// Starts a high voltage calibration operation.
        /// </summary>
        /// <param name="voltage">The voltage to set in volts.</param>
        /// <param name="duration">The length of the measurement to take.</param>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void StartHVCalibration(int voltage, TimeSpan duration)
        {
        }

        /// <summary>
        /// Stops the currently executing high voltage calibration operation.
        /// </summary>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void StopHVCalibration()
        {
        }

        /// <summary>
        /// Closes the connection to the instrument.
        /// </summary>
        /// <exception cref="IOException">An error occurred communicating with the device.</exception>
        public virtual void Disconnect()
        {
        }

        public bool Equals(Instrument other)
        {
            if (other as Object != null &&
                this.selected == other.selected && this.daqstate == other.daqstate &&
                this.id == other.id)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is Instrument))
                throw new InvalidCastException("The 'obj' argument is not an Instrument object.");
            else
                return Equals(obj as Instrument);
        }

        public override int GetHashCode()
        {
            return this.selected.GetHashCode() ^ this.daqstate.GetHashCode() ^ this.id.GetHashCode();
        }

        public static bool operator ==(Instrument i1, Instrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return true;
                else
                    return i2.Equals(null);
            }
            else
                return i1.Equals(i2);
        }

        public static bool operator !=(Instrument i1, Instrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return false;
                else
                    return !i2.Equals(null);
            }
            else
                return (!i1.Equals(i2));
        }

        public static int Compare(Instrument x, Instrument y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = 0;
            if (x.selected)
                res = x.selected.CompareTo(y.selected);
            if (res == 0)
                res = x.daqstate.CompareTo(y.daqstate);
            if (res == 0 && (x.id != null))
                res = x.id.CompareTo(y.id);

            return res;
        }

        public int CompareTo(Instrument other)
        {
            return Compare(this, other);
        }

    }

    public class LMInstrument : Instrument, IEquatable<LMInstrument>
    {
        // TODO: refactor, because this class was intended for ethernet based LMMM-style boxes
        public LMInstrStatus lmstatus;

        public int port;
        public SocketAsyncEventArgs instrSocketEvent;

        public INeutronDataFile file;  // output only

        public override void Init(LMLoggers.LognLM datalog, LMLoggers.LognLM alog)
        {
            if (RDT == null)
            {
                RDT = new LMRawDataTransform();
                RDT.Init(datalog == null ? NC.App.Loggers.Logger(LMLoggers.AppSection.Data) : datalog, 
                    alog == null ? NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis) :  alog );

            }
        }

        public new LMRawDataTransform RDT
        {
            get { return (LMRawDataTransform)base.RDT; }
            set { base.RDT = value; }
        }

        public LMInstrument(Detector det)
            : base()
        {
            id = new DataSourceIdentifier(det.Id);
            lmstatus = new LMInstrStatus();
            file = new NCDFile();
            file.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Collect);
            Init(null,null);

        }

        /// <summary>
        /// LMMM-specific constructor for live instrument uptake while doing discovery on a local network
        /// </summary>
        /// <param name="e">socket event struct</param>
        /// <param name="EndpointPort">the source port</param>
        public LMInstrument(SocketAsyncEventArgs e, int EndpointPort)
            : base()
        {
            lmstatus = new LMInstrStatus();
            instrSocketEvent = e;
            port = EndpointPort;
            id.DetectorId = "new"; // always this special string, see IsNew
            id.SetSRType("LMMM"); // this constructor should only be called for LMMM ethernet enabled devices
            file = new NCDFile();
            file.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Collect);
            Init(null,null);

        }
        public bool IsNew()
        {
            return id.DetectorId.Equals("new"); // always this special string, see Constructor
        }


        internal INeutronDataFile PrepOutputFile(string prefix, int idx, LMLoggers.LognLM collog)
        {
            if (!NC.App.AppContext.LiveFileWrite)
                return file;
            file.GenName(prefix, idx); // dev note: good to provide external config approach to specify output file pattern 
            file.CreateForWriting();
            return file;
        }

        public ulong NumProcessedRawDataBuffers
        {
            get { return RDT.NumProcessedRawDataBuffers; }
            set { RDT.NumProcessedRawDataBuffers = value; }
        }

        public bool Equals(LMInstrument other)
        {
            if (other as Object != null && this.port == other.port &&  // could use NCDFile and socket data too
                Equals(other as Instrument)) // base equality 
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is LMInstrument))
                return false;
            else
                return Equals(obj as LMInstrument);
        }

        public override int GetHashCode()
        {
            return this.port.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(LMInstrument i1, LMInstrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return true;
                else
                    return i2.Equals(null);
            }
            else
                return i1.Equals(i2);
        }

        public static bool operator !=(LMInstrument i1, LMInstrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return false;
                else
                    return !i2.Equals(null);
            }
            else
                return (!i1.Equals(i2));
        }
    }

    public static class LMInstrumentExtensionMethods
    {
        public static bool SocketBased(this LMInstrument lm)
        {
            return lm.id.SRType.IsSocketBasedLM();
        }
    }
    public class SRInstrStatus
    {
        public bool one, two;
    }

    public class SRInstrument : Instrument, IEquatable<SRInstrument>
    {

        public SRInstrStatus srstatus;

        public SRInstrument(Detector det)
            : base()
        {
            id = new DataSourceIdentifier(det.Id);
            srstatus = new SRInstrStatus();
            DAQState = DAQInstrState.Offline;  // different scheme, we need to connect after the Instrument is known, rather than connect because the instrument is known
        }

        public override void Init(LMLoggers.LognLM datalog, LMLoggers.LognLM alog)
        {
            if (RDT == null)
            {
                RDT = new SRCycleDataTransform();  // only using the synch point for now
                RDT.Init(datalog == null ? datalog : NC.App.Loggers.Logger(LMLoggers.AppSection.Data), 
                    alog == null ? alog : NC.App.Loggers.Logger(LMLoggers.AppSection.Analysis));
            }
        }

        public DAQ.SRControl SRCtrl
        {
            get { return (RDT as SRCycleDataTransform).SRCtrl; }
            set { (RDT as SRCycleDataTransform).SRCtrl = value; }
        }

        public bool Equals(SRInstrument other)
        {
            // todo add RDT equality based on type and Cycle state
            if (other as Object != null && Equals(other as Instrument)) // base equality 
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is SRInstrument))
                return false; //throw new InvalidCastException("The 'obj' argument is not a SRInstrument object.");
            else
                return Equals(obj as LMInstrument);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode() ^ base.GetHashCode();
        }

        public static bool operator ==(SRInstrument i1, SRInstrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return true;
                else
                    return i2.Equals(null);
            }
            else
                return i1.Equals(i2);
        }

        public static bool operator !=(SRInstrument i1, SRInstrument i2)
        {
            if (i1 as Object == null)
            {
                if (i2 as Object == null)
                    return false;
                else
                    return !i2.Equals(null);
            }
            else
                return (!i1.Equals(i2));
        }

        public static int Compare(SRInstrument x, SRInstrument y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = (x as Instrument).CompareTo(y as Instrument);
            if (res == 0)
                res = x.selected.CompareTo(y.selected);
            if (res == 0)
                res = x.DAQState.CompareTo(y.DAQState);
            if (res == 0 && (x.id != null))
                res = x.id.CompareTo(y.id);

            return res;
        }

        public int CompareTo(SRInstrument other)
        {
            return Compare(this, other);
        }
    }

    /// <summary>
    /// Fields reported by LMMM for cStatus request.
    /// See parser at LMMMComm.LMMMLingo.SplitCStatusResponse
    /// </summary>
    public class LMInstrStatus
    {
        public bool debug;
        public int inputPath;
        public int leds;
        public int setpoint;
        public int hv;
        public int MaxLLD, LLD, u1; // likely incorrect, more fields probably from E14 fields e.g. LLD  vals
    }

    /// <summary>
    /// Channel rates upon request
    /// See parser at LMMMComm.LMMMLingo.SplitRatesStatusResponse
    /// </summary>
    public class RatesStatus
    {
        /// <summary>
        /// rates for each channel at time of request
        /// </summary>
        public long[] channels = new long[NC.ChannelCount];

        /// <summary>
        /// CSV string rep of rates, 1 . . . 32
        /// </summary>
        public override string ToString()
        {
            if (channels == null)
                return "";

            string le = "";
            foreach (long l in channels)
            {
                // assume no gaps in list, otherwise need a filler extender here for missing row entries
                le += (l.ToString() + ",");
            }
            return le;
        }
    }

    /// <summary>
    /// Fields reported by LMMM for power request.
    /// See parser at LMMMComm.LMMMLingo.SplitPowerReadResponse
    /// </summary>
    /// <remarks>Values are irrelevant for LMMM HW, relevant for E14 or for 2010 new electronics development(?)</remarks>
    public class PowerStatus
    {
        /// <summary>
        /// 1 if AC present
        /// </summary>
        public long ACPresent;
        /// <summary>
        /// 1 if battery present
        /// </summary>
        public long batteryPresent;
        /// <summary>
        /// Purported percentage remaining battery charge
        /// </summary>
        public long batterylevelPct;
    }
}
