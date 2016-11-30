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
using NCCReporter;
using DetectorDefs;

namespace AnalysisDefs
{
    using NC = NCC.CentralizedState;
  
    //carries mapped results on each cycle and the overarching measurement object
    public class CountingResults : CountingResultsMap
    {
        public CountingResults()
        {
        }       
    }

    public enum CycleDAQStatus { None, Completed, Cancelled, UnspecifiedTruncation, Rates };  // could make these a bit mask to mark cycles as failing multiple tests

    public partial class Cycle: ParameterBase
    {
        //** INCC

        public int seq;   // run number
        private TimeSpan ts; // timestamp of last neutron v. requested time
        private VTuple singles; // aka totals in INCC
        private double rawsinglesrate;
        private ulong totalevents;  // events at time t
        double[] hitsPerChn; // NC.ChannelCount
        private string message; // NCD file or DAQ stream status block
        // dev note: get per-channel HV during LMMM DAQ too, not doing that yet!
        private double highVoltage; // dev note: carried over from INCC runs, a single channel of HV values, could expand to n channels to support the channel count of the collecting instrument or the virtual detector's channel count
        private QCStatusMap qcstatus; // dev note: need one for each multiplicity result, so this really belongs on a datum attached to each analyzer result in the counting results map
        private DataSourceIdentifier dsid;
        private CountingResults countresults;
        private CycleDAQStatus daqStatus;
        private LMLoggers.LognLM logger;

        public double HighVoltage
        {
            get { return highVoltage; }
            set { highVoltage = value; }
        }

        public CycleDAQStatus DaqStatus
        {
            get { return daqStatus; }
            set { daqStatus = value; }
        }

        public QCStatus QCStatus(Multiplicity key)
        {
            QCStatus val = new QCStatus();
            bool ok = qcstatus.TryGetValue(key, out val);
            return val;
        }
        public bool HasEntry(Multiplicity key)
        {
            QCStatus val = null;
            bool ok = qcstatus.TryGetValue(key, out val);
            return ok;
        }
        public void SetQCStatus(Multiplicity key, QCTestStatus newval, double voltage = -1)
        {
            QCStatus val = new QCStatus();
            bool ok = qcstatus.TryGetValue(key, out val);
            if (!ok)
                qcstatus.Add(key, new QCStatus(newval, voltage));
            else
                qcstatus[key] = new QCStatus(newval, voltage);
        }

        // 
        /// <summary>
        /// True if all the multiplicity entries are valid. Use this condition to move forward for INCC5 calculations
        /// </summary>
        /// <returns>All the multiplicity entries on this cycle have a Pass status</returns>
        public bool QCStatusStronglyValid
        {
            get { return qcstatus.StronglyValid(); }
        }

        public bool QCStatusValid(Multiplicity key)
        {
			if (qcstatus.ContainsKey(key))
            return QCStatus(key).status == QCTestStatus.Pass;
			else
				return false; 
        }

		public bool QCStatusWeaklyValidForTHisKey(Multiplicity key)
        {
			if (qcstatus.ContainsKey(key))
				return QCStatus(key).status != QCTestStatus.None;
			else
				return false; 
        }
        public bool QCStatusWeaklyValid
        {
            get { return qcstatus.WeaklyValid(); }
        }
        public bool QCStatusHasFlag(QCTestStatus filter, Multiplicity key)
        {
			if (qcstatus.ContainsKey(key))
            return QCStatus(key).status.HasFlag(filter);
			else
				return false;
        }
        public bool QCStatusHasChecksumError(Multiplicity key)
        {
			if (qcstatus.ContainsKey(key)) {
            QCTestStatus ts = QCStatus(key).status;
            return ((ts == QCTestStatus.Checksum1) || (ts == QCTestStatus.Checksum2) || 
                (ts == QCTestStatus.Checksum3)); // err yeah, guess I could use the flags and mask for true here
			} else
				return false;
        }
        public TimeSpan TS
        {
            get { return ts; }
            set { ts = value; }
        }
		public TimeSpan ExpectedTS
		{
			set { ts = value; }
		}
		// number of events, and hits per channel event
		public ulong Totals
        {
            get { return (ulong)singles.v; }
            set { singles.v = value; }
        }

        // number of events per time unit
        public ulong TotalEvents
        {
            get { return totalevents; }
            set { totalevents = value; }
        }

        // unadjusted raw singles rate, unioned channel hits "triggers"
        public double SinglesRate
        {
            get { return rawsinglesrate; }
            set { rawsinglesrate = value; }
        }
        // hits per channel, can be huge for a long cycle ;`)
        public double[] HitsPerChannel
        {
            get { return hitsPerChn; }
			set { hitsPerChn = value; }

        }
        // normally this is the status block from an NCD file or interval from a live DAQ
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        // those glorious results!
        public CountingResults CountingAnalysisResults
        {
            get { return countresults; }
        }

        public DataSourceIdentifier DataSourceId
        {
            get { return dsid; }
            set { dsid = value; }
        }

        public Cycle(LMLoggers.LognLM logger)
        {
            // Raw counts aka totals
            singles = new VTuple();
            dsid = new DataSourceIdentifier();
            qcstatus = new QCStatusMap();
            countresults = new CountingResults();
            daqStatus = CycleDAQStatus.None;
            this.logger = logger;
            hitsPerChn = new double[NC.ChannelCount];
        }

        public bool Transfer(BaseRate ba, RateResult rates)
        {
            if (rates == null)
                return true;
            bool res = true;
            try
            {
                RatesResultEnhanced rrm = new RatesResultEnhanced(rates.numCompletedGates, ba.gateWidthTics);
                rrm.totaltime = new TimeSpan(this.TS.Ticks);
                countresults.Add(ba, rrm);
                rrm.TransferRawResult(rates);
            }
            catch (OutOfMemoryException e)
            {
                ba.reason = "BaseRate transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87405, ba.reason);
            }
            return res;
        }

        public bool Transfer(Multiplicity mup, MultiplicityResult mr, int idx)
        {
            if (mr == null)
                return true;
            bool res = true;
            try
            {
                SetQCStatus(mup, QCTestStatus.Pass);  // marked Pass at the outset 
                MultiplicityCountingRes lmcs = new MultiplicityCountingRes(mup.FA, idx);
                countresults.Add(mup, lmcs);
                lmcs.Totals = Totals;
                lmcs.TransferRawResult(mr);
                lmcs.TS = new TimeSpan(TS.Ticks);
            }
            catch (OutOfMemoryException e)
            {
                mup.reason = "Multiplicity transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87406,  mup.reason);
            }
            return res;
        }

        public bool Transfer(Coincidence cop, CoincidenceResult cor)
        {
            if (cor == null)
                return true;
            bool res = true;
            try
            {
                CoincidenceMatrixResult cr = new CoincidenceMatrixResult(cor.RACoincidenceRate.Length); // check for equality to numchannels, should always be numXnum size
                countresults.Add(cop, cr);
                cr.TransferRawResult(cor);
            }
            catch (OutOfMemoryException e)
            {
                cop.reason = "Coincidence matrix transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87410, cop.reason);
            }
            return res;
        }

        public bool Transfer(Feynman ryp, FeynmanResult fr)
        {
            if (fr == null)
                return true;
            bool res = true;
            try
            {
                FeynmanResultExt lfr = new FeynmanResultExt();
                countresults.Add(ryp, lfr);
                lfr.TransferRawResult(fr);
            }
            catch (OutOfMemoryException e)
            {
                ryp.reason = "Feynman transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87407, ryp.reason);
            }
            return res;
        }

        public bool Transfer(Rossi rap, RossiAlphaResult rar)
        {
            if (rar == null)
                return true;
            bool res = true;
            try
            {
                RossiAlphaResultExt ra = new RossiAlphaResultExt(rar.gateWidth, rar.gateData);   // deep copy via constructor menas no extra transfer function is needed
                countresults.Add(rap, ra);
            }
            catch (OutOfMemoryException e)
            {
                rap.reason = "Rossi transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87408, rap.reason);
            }
            return res;
        }

        public bool Transfer(TimeInterval esp, EventSpacingResult esr)
        {
            if (esr == null)
                return true;
            bool res = true;
            try
            {
                TimeIntervalResult lesr = new TimeIntervalResult();  // the deep copy from ES to TI follows
                countresults.Add(esp, (object)lesr);
                lesr.TransferRawResult(esr);
            }
            catch (OutOfMemoryException e)
            {
                esp.reason = "TimeInterval transfer " + e.Message;
                res = false;
                logger.TraceEvent(LogLevels.Error, 87409, esp.reason);
            }
            return res;
        }

        public string NsChnImage()
        {
            String s = "";
            int i = 0;
            foreach (double h in HitsPerChannel)
            {
                i++;
                s += string.Format("{0}, ", h);
            }
            return s;
        }
        public string[] StringifyCycleMultiplicityDetails()
        {
            string[] x = null;
            try
            {
                int j = 0;
                RatesResultEnhanced rrm = countresults.GetFirstRatesResultMod();  // devnote: only the first is used here, still need multiple rate analyzer output
                x = new string[3];
                if (rrm != null)
                {
                    x[j++] = string.Format("Channel hits (BaseA): {0}", rrm.NsChnImage());
                    x[j++] = string.Format("Channel hits (CyclC): {0}", NsChnImage());
                    x[j++] = string.Format("Cycle status text: " + LMLoggers.LognLM.FlattenChars(Message));
                    System.Collections.IEnumerator iter = countresults.GetATypedResultEnumerator(typeof(AnalysisDefs.Multiplicity));
                    while (iter.MoveNext())
                    {
                        MultiplicityCountingRes mcr = (MultiplicityCountingRes)iter.Current;
                        try
                        {
                            string[] temp = mcr.StringifyCurrentMultiplicityDetails();
                            Array.Resize(ref x, temp.Length + x.Length);
                            Array.Copy(temp, 0, x, j, temp.Length);
                            j += temp.Length;
                        }
                        catch (Exception ex)
                        {
                            logger.TraceEvent(LogLevels.Error, 87118, "StringifyCycleMultiplicityDetails error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    x = new string[1] { "" };
                    //Martyn says this is useless. HN 10-6-2015
                    //x[j++] = "No multiplicity data for JSR-12 instruments.";
                }

            }
            catch (Exception ex2)
            {
                logger.TraceEvent(LogLevels.Error, 87119, "StringifyCycleMultiplicityDetails error: " + ex2.Message);
            }
            return x;
        }

        public void SetUpdatedDataSourceId(DataSourceIdentifier dataSourceIdentifier)
        {
            dsid = new DataSourceIdentifier(dataSourceIdentifier);
            dsid.dt = DateTimeOffset.Now;
        }

        /// <summary>
        /// Set up the meta-data needed for cycle processing.
        /// Important for cycles obtained from files and the database.
        /// </summary>
        /// <param name="data_src">Source of cycle data</param>
        /// <param name="SRType">Expected detector type</param>
        /// <param name="dto">The start DateTimeOffset timestamp of the cycle</param>
        /// <param name="path">Filename of source of cycle, if any</param>
        public void UpdateDataSourceId(ConstructedSource data_src, InstrType SRType, DateTimeOffset dto, string path)
        {
            dsid.source = data_src;
            dsid.SRType = SRType;
            if (!string.IsNullOrEmpty(path))
				dsid.FileName = path;                              
            dsid.dt = dto;
        }

        public MultiplicityCountingRes MultiplicityResults(Multiplicity pk)
        {
             if (CountingAnalysisResults.ContainsKey(pk))
                return (MultiplicityCountingRes)CountingAnalysisResults[pk]; 
            else
                return null;
        }

        /// <summary>
        /// Create parameter list for the results on a cycle
        /// </summary>
        /// <param name="mkey">The multiplicity parameters used to select the specific results. There can be more than one such results set per cycle.</param>
        public void GenParamList(Multiplicity mkey)
        {
            GenParamList(); // ^ does the basic INCC5 and new LM cycle stuff

            // now add the mkey stuff
            Table = "cycles";
            MultiplicityCountingRes pmcr = null;
            QCTestStatus status = QCTestStatus.None;
            if (CountingAnalysisResults.HasMultiplicity)
                try
                {
                    pmcr = (MultiplicityCountingRes)CountingAnalysisResults[mkey];
                    status = qcstatus[mkey].status;
                }
                catch (Exception) // mkey not found happens when a param is changed on a VSR that is not reflected back to the default [0] SR 
                {
                    logger.TraceEvent(LogLevels.Warning, 7832, "Cycle status not set in DB, mkey mismatch: " + mkey.ToString());
                }
            if (pmcr == null)
                pmcr = new MultiplicityCountingRes();  // null results 
            ps.Add(new DBParamEntry("scaler1", pmcr.Scaler1.v));
            ps.Add(new DBParamEntry("scaler2", pmcr.Scaler2.v));
            ps.Add(new DBParamEntry("reals_plus_acc", pmcr.RASum));
            ps.Add(new DBParamEntry("acc", pmcr.ASum));
            ps.Add(new DBParamEntry("mult_reals_plus_acc", pmcr.RAMult));
            ps.Add(new DBParamEntry("mult_acc", pmcr.NormedAMult));
            ps.Add(new DBParamEntry("scaler1_rate", pmcr.Scaler1Rate.v));
            ps.Add(new DBParamEntry("scaler2_rate", pmcr.Scaler2Rate.v));
            ps.Add(new DBParamEntry("doubles_rate", pmcr.RawDoublesRate.v));
            ps.Add(new DBParamEntry("triples_rate", pmcr.RawTriplesRate.v));
            ps.Add(new DBParamEntry("multiplicity_mult", pmcr.multiplication));
            ps.Add(new DBParamEntry("multiplicity_alpha", pmcr.multiAlpha));
            ps.Add(new DBParamEntry("multiplicity_efficiency", pmcr.efficiency));
            ps.Add(new DBParamEntry("mass", pmcr.mass));
            ps.Add(new DBParamEntry("status", (int)status));
			{	// la super hack-a-whack
				DB.DB db = new DB.DB(true);
				if (db.TableHasColumn(Table,"mult_acc_un"))
					ps.Add(new DBParamEntry("mult_acc_un", pmcr.UnAMult));
			}          
        }

        public void GenParamList(SpecificCountingAnalyzerParams skey)
        {
            GenParamList(); // ^ does the basic INCC5 and new LM cycle stuff

            // now add the skey stuff
            Table = "cycles";
        }


        /// <summary>
        /// Create parameter list and add the essential data values of a cycle
        /// results are handled elsewhere on a per mkey basis (see GenParamList(Multiplicity pk) infra)
        /// </summary>
        public override void GenParamList()
        {
            base.GenParamList();
            Table = "cycles";
            ps.Add(new DBParamEntry("singles", Totals));
            ps.Add(new DBParamEntry("singles_rate", SinglesRate));
            ps.Add(new DBParamEntry("high_voltage", HighVoltage));
            ps.Add(new DBParamEntry("cycle_time", TS));
            if (DataSourceId.SRType.IsListMode())
            {
                ps.Add(new DBParamEntry("chnhits", HitsPerChannel));
				ps.Add(new DBParamEntry("lmfilename", dsid.FileName));
            }
        }

        /// <summary>
        /// Sets the status of the cycle stream processing if processing ended before a fully formed status block was parsed.
        /// The status is None if no end block was encountered, and if it is none and the parsed message is empty, set status to UnspecifiedTruncation
        /// </summary>
        public void SetDatastreamEndStatus()
        {
            if (daqStatus == CycleDAQStatus.None && string.IsNullOrEmpty(Message))
                // cycle was completed abnormally due to interruption or premature end of stream
                daqStatus = CycleDAQStatus.UnspecifiedTruncation;
            //else if (String.IsNullOrEmpty(Message))
            //    logger.TraceEvent(NCCReporter.LogLevels.Verbose, 7832, "SetStatus on cycle {0}", daqStatus);
        }

    }


    public class INCCCycleConditioning
    {
    }
}
