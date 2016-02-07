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
using AnalysisDefs;
using DetectorDefs;
using DAQ;
using Instr;
namespace NCC
{

    using NC = NCC.CentralizedState;
    using RawStatus = LMRawAnalysis.AnalyzerHandlerStatus;

    
/*
 A. Status data for system and processing status during DAQ/File or "at Rest".
    
   1. Instrument State Snapshot (before and during, but not including raw analysis processing); all state but the actual raw analyzers
        a: (RDT) Data source name, time since start, if live then instrument connection status (name, time since connection, IP) 
        b: For HV Calib operation: The HV Calib Point status as it exists now, 
        c: For Assay operation: 
          1 buffer count, buffer rotation count, events count
          2 cycle # of #, cycle status, current cycle sums (event, hits, channel count snapshot)
      
   2. Measurement Snapshot    
      a: "At Rest" State of Health: 
        Need the current settings for assay, connection configuration; status, uptime,  (enable a heartbeat query to network if at rest but connected?)    
      b: For active HV or Assay operations: 
	      one per instrument RawDataTransform snapshots (see 1 above), or one RawDataTransform snapshot for file processing

   3. System snapshot    
      a: "At Rest" SOH:  sys uptime, sys environment: location and use of config file and DB, last measurement or task details (date/time, id, source)
      b: Current meta-data on measurements (none, in progress, cancelling, stopped, aborted, finished, number since start of session)
 
 B. Status data requirements for raw analysis results 
  
   1. Access to raw analyzer status "running, waiting, complete"
 
   2. Access to latest results for each active raw analyzer  
      a: Per cycle, use the results map Cycle.CountingAnalysisResults
      b: Over all cycles, Measurement.CountingAnalysisResults
  
 C. Status data requirements for INCC multi-cycle and mass analysis results (that is, everything computed in Measurement.CalculateMeasurementResults)
 
   1. If doing a verification measurement then need access during raw analysis (A2b and B) to intermittent mass calculations
      a: per cycle, mass values stored on Cycle.CountingAnalysisResults, computed in Cycle.CalculatePreliminaryMass
      b: at end of measurement, and before CalculateMeasurementResults
 
   2. For each INCC analysis underway, access to Measurement.INCCAnalysisResults (specified with the combo of measurement option, multiplicity shift register parameter, INCC method parameter
      a: Calculation progress and status (selector, method, and where at in steps: outlier, counting summary, calv avg, mass calc)
      b: Access to latest mass results (Measurement.INCCAnalysisResults.MethodsResults) for each combo of multiplicity shift register parameter, INCC method parameter and INCC material type)
 
 * */

    // dev note: might want lightweight versions or states with minimal information

    public class MeasurementStatus
    {
        // shared by all
        NCCAction action;
        OperatingState opstate;
        DateTime start;

        // Assay/HV/Discover measurement-specific state
        DAQInstrState State;
        public int CurrentRepetition { get; set; }
        public int RequestedRepetitions { get; set; }

        // current results for each instrument, but not for the overall measurement
        // public Dictionary<Instrument, CombinedProcessingStateSnapshot> snaps;
        public CombinedInstrumentProcessingStateSnapshot snaps; // one for now

        public AnalysisDefs.Measurement Measurement; // just a virtual pointer, not a frozen snapshot, be careful using this during proessing, the root of all that is good and evil


        // system snapshot only
        public MeasurementStatus()
        {
            // system operational state is already on NC.App.Opstate, this makes a copy
            action = NC.App.Opstate.Action;
            opstate = NC.App.Opstate.SOH;
            start = NC.App.Opstate.start;

            switch (NC.App.Opstate.Action)
            {
                case NCCAction.Nothing:
                case NCCAction.Prompt:
                case NCCAction.Maintenance:
                case NCCAction.Analysis:
                    // nothing more right now
                    break;
                case NCCAction.Discover:
                case NCCAction.HVCalibration:
                case NCCAction.Assay:
                    if (NC.App.Opstate is DAQControl.AssayState)
                    {
                        DAQControl.AssayState a = DAQControl.CurState;
                        State = a.State;
                    }
                    CurrentRepetition = NC.App.Opstate.Measurement.CurrentRepetition;
                    RequestedRepetitions = NC.App.Opstate.Measurement.RequestedRepetitions;
                    break;
            }
        }

        public void UpdateWithInstruments()
        {
            //snaps = new Dictionary<Instrument, CombinedInstrumentProcessingStateSnapshot>(Instruments.All.Count);
            //snaps = null;
            foreach (Instrument inst in Instruments.All)
            {
                // snaps.Add(inst, new CombinedProcessingStateSnapshot(inst));
                snaps = new CombinedInstrumentProcessingStateSnapshot(inst);
                break;  // one for now
            }
        }

        public void UpdateWithMeasurement()
        {
            Measurement = NC.App.Opstate.Measurement;
        }

        public bool StartedMeasurement
        {
            get
            {
                bool b = Measurement == null ? false : true;
                b = b && opstate != OperatingState.Void && action >= NCCAction.Assay && start.Ticks > 0;
                return b;
            }
        }


        // next: does not take the event handler itself into account, needs to be merged with the SOH logging summary status texts, the status logging and updating is incomplete
        override public string ToString()
        {
            string s;
            string dts = start.Ticks > 0 ? start.ToString("MMM dd yyy HH:mm:ss.ff K") : "";

            if (StartedMeasurement)
            {
                s = string.Format("At {0} of {1} cycles", CurrentRepetition, RequestedRepetitions);
                s += string.Format(", {0} started at {1} [{2}]", action, dts, opstate);
            }
            else
            {
                s = string.Format("{0} {1} [{2}]", action, dts, opstate);
            }
            return s;            
        }
    }

    // 1 Instrument State Snapshot (before and during, but not including raw analysis processing); all state but the actual raw analyzers
    public class CombinedInstrumentProcessingStateSnapshot
    {
        public CombinedInstrumentProcessingStateSnapshot(Instrument inst)
        {
            iss = new InstrumentStateSnapshot(inst);
            if (!iss.ins.IsSuspect)
            {
                if (inst is LMInstrument)
                {
                    LMInstrument lm = inst as LMInstrument;
                    cs = new CountersStatus(lm.RDT.ReadCountingProcessorStatus());
                }
                else if (inst is SRInstrument)
                {
                    cs = new CountersStatus(null);
                }
            }
        }

        public bool HasIdent
        {
            get { return iss != null && iss.ins != null && !(iss.ins.dsid == null); }
        }
        public bool HasProcessingStatus
        {
            get { return iss != null && iss.asy != null; }
        }

        public bool HasCounterStatus
        {
            get { return cs != null; }
        }

        public InstrumentStateSnapshot iss;
        public CountersStatus cs;

        public override string ToString()
        {
            String s = iss.ToString();
            if (iss.ins.IsSuspect)
                s = iss.ins.Reason + ", " + s;
            else
                s = cs + ", " + s;
            return s;
        }
    }

    public class InstrumentStateSnapshot
    {

        // basic single-line status summary
        public override string ToString()
        {
            String s = ins.ToString();

            switch (action)
            {
                case NCCAction.HVCalibration:
                    s += " HV "; // todo: finish this and clean up ActionInProgressStatusTimerCB impl
                    break;
                case NCCAction.Assay:
                    if (asy.cpss.dsid.SRType == InstrType.PTR32 || asy.cpss.dsid.SRType == InstrType.LMMM)
                    {
                        s += String.Format("; cycle {0}, rate {1:F4}, totals {2}, hits {3}, (IO {4}) ",  // some cycle info
                            asy.cpss.seq, asy.cpss.singles, asy.cpss.totals, asy.cpss.totalevents, asy.cpss.daqStatus);
                        s += String.Format("buff {0} {1}", asy.numProcessedRawDataBuffers, asy.assayPending ? "processing events" : "waiting for results");
                    }

                    break;
                case NCCAction.Discover:
                    s += " discover ";  // todo: finish this and clean up ActionInProgressStatusTimerCB impl
                    break;
            }
            return s;
        }

        public InstrumentStateSnapshot(Instrument inst)
        {
            action = NC.App.Opstate.Action;
            ins = new InstrumentStatus(inst);
            switch (action)
            {
                case NCCAction.HVCalibration:
                    hv = new HVStatusSnapshot(inst);
                    break;
                case NCCAction.Assay:
                    asy = new AssayProcessingStateSnapshot(inst);
                    break;
                case NCCAction.Discover:
                    //asy = new AssayProcessingStateSnapshot(inst);
                    break;
            }
        }

        //         1a: (RDT) Data source name, time since start, if live then instrument connection status (name, time since connection, IP) 
        public class InstrumentStatus : StatusCondition
        {
            public InstrumentStatus(Instrument inst)
            {
                if (inst == null)
                {
                    Reason = "No instrument";
                    return;
                }
                dsid = inst.id;
                state = inst.DAQState;
                active = inst.selected;
                if (inst.RDT == null)
                {
                    Reason = "No data processor";
                    return;
                }
                if (inst.RDT.Cycle == null)
                {
                    Reason = "No cycle";
                }

            }

            public DataSourceIdentifier dsid;
            public DAQInstrState state;
            public bool active;

            public override string ToString()
            {
                String s = String.Format("{0} {1}{2}", dsid == null ? "-" : dsid.Identifier(), state.ToString(), active ? "" : " (inactive)");
                if (IsSuspect)
                    s = String.Format("({0}) {1}", Reason, s);
                return s;
            }
        }

        // NEXT:  1b: For HV Calib operation: The HV Calib Point status as it exists now, 
        public class HVStatusSnapshot : HVControl.HVStatus
        {
            public HVStatusSnapshot(Instrument inst)
            {
            }
            // last status plus stepping placement 
        }

        // 1c1 For Assay operation:  buffer count, buffer rotation count, events count
        public class AssayProcessingStateSnapshot
        {
            public AssayProcessingStateSnapshot(Instrument inst)
            {
                cpss = new CycleProcessingStateSnapshot(inst);
                if (inst == null)
                    return;
                assayPending = inst.IsPending();
                if (inst.RDT == null)
                    return;
                if (inst is LMInstrument)
                {
                    LMInstrument lm = inst as LMInstrument;
                    Analysis.LMProcessingState lmrdtstate = (Analysis.LMProcessingState)(inst.RDT.State);
                    numProcessedRawDataBuffers = lm.NumProcessedRawDataBuffers;
                    maxValuesInBuffer = lmrdtstate.maxValuesInBuffer;
                    lastValue = lmrdtstate.lastValue;
                    wraparoundOffset = lmrdtstate.wraparoundOffset;
                    hitsPerChn = lmrdtstate.hitsPerChn;
                }
                else if (inst is SRInstrument)
                {
                }
                numTotalsEncountered = inst.RDT.State.NumTotalsEncountered; 
                numValuesParsed = inst.RDT.State.NumValuesParsed;
            }
            // the overall counts across the current cycle can span multiple buffer state transitions
            public CycleProcessingStateSnapshot cpss;


            // the buffering and byte-swapping state
            public bool assayPending;
            public ulong numProcessedRawDataBuffers;
            // accumulators across buffer transform calls
            public UInt32 maxValuesInBuffer;  //upper bound to number of structures in the array, based on buffer length
            public UInt32 numValuesParsed; //number of neutron-event/time structures parsed from the current buffer
            public UInt32 lastValue;
            public UInt64 wraparoundOffset;
            public UInt64 numTotalsEncountered;
            public Double[] hitsPerChn;

            public ushort ofseq;// = meas.AcquireState.num_runs;

        }

        // 1c2 For Assay operation: cycle # of #, cycle status, current cycle sums (event, hits, channel count snapshot)
        public class CycleProcessingStateSnapshot
        {
            public CycleProcessingStateSnapshot(Instrument inst)
            {
                if (inst == null || inst.RDT == null || inst.RDT.Cycle == null)
                    return;
                Cycle c = inst.RDT.Cycle;
                seq = c.seq;
                ts = new TimeSpan(c.TS.Ticks);
                singles = c.Totals;
                rawsinglesrate = c.SinglesRate;
                totalevents = c.TotalEvents;
                totals = c.Totals;
                message = c.Message;
                hitsPerChn = new double[c.HitsPerChannel.Length]; 
                Array.Copy(c.HitsPerChannel, hitsPerChn, hitsPerChn.Length);
                highVoltage = c.HighVoltage;
                dsid = new DataSourceIdentifier(c.DataSourceId);
                daqStatus = c.DaqStatus;
                //cycle = c;
            }

            public int seq;   // run number
            public TimeSpan ts;
            public double singles; // aka totals in INCC
            public double rawsinglesrate;
            public ulong totalevents, totals;  // events at time t and hits
            public string message; // NCD file or DAQ stream status block
            public Double[] hitsPerChn;
            public double highVoltage; // carried over from INCC runs
            public DataSourceIdentifier dsid;
            public AnalysisDefs.CycleDAQStatus daqStatus; // status of pulse stream collection
            //public Cycle cycle;
            public string HitsPerChnImage()
            {
                String s = "";
                if (hitsPerChn == null)
                    return s;
                int i = 0;
                foreach (Double h in hitsPerChn)
                {
                    i++;
                    s += String.Format("{0}, ", h);
                }
                return s;
            }
        }


        public NCCAction action; // HV or Assay or discover or nothing
        public InstrumentStatus ins;
        public HVStatusSnapshot hv;
        public AssayProcessingStateSnapshot asy;
    }

    // the counting state
    public class CountersStatus : StatusCondition
    {
        public CountersStatus(RawStatus rs)
        {
            if (rs == null)
            {
                Reason = "No counting processor status";
                status = String.Empty;
                return;
            }
            status = rs.ToString();
        }

        string status;

        public new string ToString()
        {
            return status;
        }
    }

    public class StatusCondition
    {
        public StatusCondition()
        {
            Reason = String.Empty;
        }
        public string Reason;
        public bool IsSuspect // true if appellant is null or something went wrong
        {
            get { return !String.IsNullOrEmpty(Reason); }
        }
    }

}
