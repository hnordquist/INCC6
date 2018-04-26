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
using System.Globalization;
using System.Threading;
using AnalysisDefs;
using NCCReporter;
using System.Threading.Tasks;

namespace Analysis
{

    // returned filled-in at end of data during data processing loop method
    /// <summary>
    /// Originally created for LMMM list mode end-of-cycle text blocks
    /// </summary>
    public class StreamStatusBlock
    {
        //public StatusBlock(int msglen, int index)
        //{
        //    this.msglen = msglen; this.index = index;
        //}
        public StreamStatusBlock(string _msg = "")
        {
            this.msg = _msg;
            if (!String.IsNullOrEmpty(_msg))
                Encode(_msg);

        }
        public void Encode(string msg)
        {
            this.msg = msg;
            byterep = System.Text.Encoding.ASCII.GetBytes(msg);
            msglen = byterep.Length;
            index = 0;
        }

        public void Decode(byte[] srcbuff)
        {
            byterep = new byte[msglen];
			if (srcbuff == null || byterep == null)
				return;
            Buffer.BlockCopy(srcbuff, index, byterep, 0, msglen);
            msg = System.Text.Encoding.ASCII.GetString(byterep, 0, msglen);
        }

        public int msglen;  // length of message text in status block
        public int index;  // start block index in the buffer used to find the block
        public string msg;  // block content as a string
        public byte[] byterep;  // bytes of the block
    };

    public abstract class ProcessingState
    {
        public LMLoggers.LognLM logger;

        protected UInt32 numValuesParsed; //number of neutron-event/time structures parsed from the current buffer
        public UInt32 NumValuesParsed
        {
            get { return numValuesParsed; }
            set { numValuesParsed = value; }
        }
        public UInt64 NumTotalsEncountered;
        public ManualResetEventSlim assayPending;
        // active Cycle instance accumulation in-progress
        public Cycle cycle;
        public virtual void StartCycle(Cycle cycle, object param = null)
        {
            this.cycle = cycle;
        }
        // reset for next buffers-worth
        public virtual void ResetProcessingCounters()
        {
            numValuesParsed = 0;
            NumTotalsEncountered = 0;
        }

		/// <summary>
		/// Accumulate any remaining data on the cycle object from the time/hits processor,
		/// Final high-voltage value is copied from data source,
		/// Time interval is either last neutron or requested time interval.
		/// </summary>
		/// <param name="useLastNeutron">Use the last neutron time, o.w. time is set at cycle creation time</param>
        public virtual void AccumulateCycleSummary() 
        {
            if (numValuesParsed > 0)
            {
                // update the counters
                cycle.TotalEvents += numValuesParsed;
                cycle.Totals += NumTotalsEncountered;
            }

            if (cycle.Totals > 0 && cycle.TS.TotalSeconds > 0.0)
                cycle.SinglesRate = cycle.Totals / cycle.TS.TotalSeconds;
            
        }
        /// <summary>
        /// presumes multiple analyzer results per instrument, detached from specific detector and other settings (for LM)
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public virtual bool TransferResults(CountingAnalysisParameters ap)
        {
            return true;
        }


        // convert the current byte buffer of the named size to the internal time and channel event format
        public abstract StreamStatusBlock ConvertDataBuffer(int bytecount);

    }

    public abstract class RDTBase
    {

        /// <summary>
        /// Finalizes an instance of the <see cref="RDTBase"/> class.
        /// </summary>
        ~RDTBase()
        {
            state = null; // break circular reference
        }

        //protected Config cfg;
        public LMLoggers.LognLM logger, analogger;

        protected ProcessingState state;
        public ProcessingState State
        {
            get { return state; }
            set { state = value; }
        }

        internal delegate bool WaitingForCountingResults();

        /// <summary>
        /// Create reportable results strings, expel them via logger 
        /// Call this method after the final cycle summarizer operations
        /// </summary>
        internal void FlushCycleSummaryResults()
        {
            string[] rows = Cycle.StringifyCycleMultiplicityDetails();
            foreach (string s in rows)
            {
                //Per Martyn request, change to information level. hn 9.21.2015
                logger.TraceEvent(LogLevels.Info, 152, s);
            }
            Double CycleSinglesRate = 0;
            if (Cycle.Totals > 0 && Cycle.TS.TotalSeconds > 0.0)
                CycleSinglesRate = Cycle.Totals / Cycle.TS.TotalSeconds;

            if (Cycle.CountingAnalysisResults.HasMultiplicity)
            {
                double singles = Cycle.CountingAnalysisResults.GetFirstMultiplicity.RawSinglesRate.v;
                double doubles = Cycle.CountingAnalysisResults.GetFirstMultiplicity.RawDoublesRate.v;
                double triples = Cycle.CountingAnalysisResults.GetFirstMultiplicity.RawTriplesRate.v;
                logger.TraceEvent(LogLevels.Info, 153, String.Format("Cycle {0} complete, results {1}/sec, {2} hits, {3} triggers",
                                        Cycle.seq, CycleSinglesRate, Cycle.Totals, Cycle.TotalEvents));
                logger.TraceEvent(LogLevels.Info, 2018001, String.Format("Cycle {0,-3}     Singles: {1,14:F3}     Doubles: {2,14:F3}     Triples: {3,14:F3}",
                                        Cycle.seq, singles, doubles, triples));
            }
        }

        public virtual void Init(LMLoggers.LognLM datalogger, LMLoggers.LognLM alogger)
        {
            //cfg = config;
            this.logger = datalogger;
            this.analogger = alogger;
            state.logger = datalogger;
        }

        public virtual Cycle Cycle
        {
            get;
            set;
        }

        public virtual void StartCycle(Cycle cycle, object param = null)
        {
        }

        public abstract void AssayPendingComplete();
        public abstract bool AssayPendingWait();
        public abstract void AssayPendingReset();
        public abstract bool AssayPendingIsSet();
        public abstract ManualResetEventSlim GetManualResetEvent();
        public abstract bool EndOfCycleProcessing(Measurement meas, bool last = false);
        public abstract void EndAnalysisImmediately(); // like a cancel   

    }

}