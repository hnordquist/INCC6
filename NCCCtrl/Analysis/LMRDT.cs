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
using System.Threading;
using AnalysisDefs;
using NCCConfig;
using NCCReporter;
namespace Analysis
{
    using NC = NCC.CentralizedState;
    using RawAnalyzerHandler = LMRawAnalysis.AnalyzerHandler;
    using RawStatus = LMRawAnalysis.AnalyzerHandlerStatus;

    public class LMProcessingState : ProcessingState
    {
        internal LMProcessingState()
        {
            hitsPerChn = new double[NC.ChannelCount];
            StartCycle(null);
            assayPending = new ManualResetEventSlim(false);
            chnmask = new uint[NC.ChannelCount];
            for (int i = 0; i < NC.ChannelCount; i++)
            {
                chnmask[i] = (uint)1 << i;
            }
        }

		internal void InitParseBuffers(uint parsebufflenMb, uint unitBytes, bool useRawDataBuff)
		{
            eventBufferLength = parsebufflenMb * 1024 * 1024;
			maxValuesInBuffer = eventBufferLength / unitBytes;
			if (useRawDataBuff)
			{
				if (rawDataBuff != null && rawDataBuff.Length != eventBufferLength)
				{
					rawDataBuff = null;
				}
			}
			if (timeArray != null && timeArray.Count != maxValuesInBuffer)
			{
				timeArray = null;
			}
			if (neutronEventArray != null && neutronEventArray.Count != maxValuesInBuffer)
			{
				neutronEventArray = null;
			}
            sup = null;
			GCCollect();  // GC now, then allocate new if needed
			if (useRawDataBuff && (rawDataBuff == null))
				rawDataBuff = new byte[eventBufferLength];
			if (timeArray == null)
				timeArray = new List<ulong>(new ulong[maxValuesInBuffer]);
			if (neutronEventArray == null)
				neutronEventArray = new List<uint>(new uint[maxValuesInBuffer]);
			sup = new Supporter();
		}

		internal static void GCCollect()
		{
			LMLoggers.LognLM log = NC.App.Loggers.Logger(LMLoggers.AppSection.Control);
            long mem = GC.GetTotalMemory(false);
            log.TraceEvent(LogLevels.Verbose, 4255, "Total GC Memory is {0:N0}Kb", mem / 1024L);
            log.TraceEvent(LogLevels.Verbose, 4248, "GC now");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            log.TraceEvent(LogLevels.Verbose, 4284, "GC complete");
            mem = GC.GetTotalMemory(true);
            log.TraceEvent(LogLevels.Verbose, 4255, "Total GC Memory now {0:N0}Kb", mem / 1024L);
		}

        internal uint[] chnmask;


        internal double[] hitsPerChn;  // so we can accumulate huge counts above UInt64.MaxValue  18,446,744,073,709,551,615 v. 1.7976931348623157E+308

        internal const int stdlen = 1024 * 1024 * 50;  // this is a tunable config value 'parseBuffSize'
        internal uint eventBufferLength = stdlen;

        // processing flags
        internal bool usingStreamRawAnalysis, useAsynch, includingGen2;

        // converted data buffers
        internal List<ulong> timeArray;
        internal List<uint> neutronEventArray;

        // local processing buffer
        internal byte[] rawDataBuff;

        // accumulators across buffer transform calls, xfer to results for cycle at end of streaming 
        internal uint maxValuesInBuffer;  //upper bound to number of structures in the array, based on buffer length

        internal uint lastValue;
        internal ulong wraparoundOffset;

        // Wrapper code over the virtual SR counting processor
        private Supporter sup;
        public Supporter Sup
        {
            get { return sup; }
            set { sup = value; }
        }

        public override void StartCycle(Cycle cycle, object param = null)
        {
            base.StartCycle(cycle, param);
            wraparoundOffset = 0;
            ResetProcessingCounters();
        }

        /// <summary>
        /// Transfer the results from the raw analyzers to the current cycle.
        /// A specific transfer is made for each type of result and for each of those results.
        /// The gathered results are mapped using the AnalysisParameters entries from the contextual Measurement to corresponding results in the cycle Results map
        /// </summary>
        /// <param name="m">Measurement originator of each analyzer param that maps to each result</param>
        public override bool TransferResults(CountingAnalysisParameters ap)
        {
            // accumulate totals on the cycle object from the time/hits processor
            AccumulateCycleSummary();  // has an effect only if called after StartNewBuffer and additional data has arrived during input processing.
            bool complete = true;
            int i = 0;
            foreach (BaseRate ba in ap.GetBases())
            {
                complete &= cycle.Transfer(ba, Sup.GetIthRateResult(i)); i++;
            }
            i = 0;
            foreach (Multiplicity mu in (ap.GetMults(FAType.FAOn)))
            {
                complete &= cycle.Transfer(mu, Sup.GetIthMultiplicityResult(FAType.FAOn, i), i); i++;
            }
            i = 0;
            foreach (Multiplicity mu in (ap.GetMults(FAType.FAOff)))
            {
                complete &= cycle.Transfer(mu, Sup.GetIthMultiplicityResult(FAType.FAOff, i), i); i++;
            }

            i = 0;
            foreach (Coincidence co in ap.GetCoincidences())
            {
                complete &= cycle.Transfer(co, Sup.GetIthCoincidenceMatrixResult(i)); i++;
            }
            i = 0;
            foreach (Rossi ro in (ap.GetRossis()))
            {
                complete &= cycle.Transfer(ro, Sup.GetIthRossiAlphaResult(i)); i++;
            }
            i = 0;
            foreach (Feynman fy in (ap.GetFeynmans()))
            {
                complete &= cycle.Transfer(fy, Sup.GetIthFeynmanResult(i)); i++;
            }
            i = 0;
            foreach (TimeInterval es in (ap.GetTimeIntervals()))
            {
                complete &= cycle.Transfer(es, Sup.GetIthTimeIntervalResult(i)); i++;
            }

            return complete;
        }

        // copy accumulated data to cycle object and reset for next buffers-worth
        public void StartNewBuffer()
        {
            // accumulate totals and current raw rate on the current cycle
            AccumulateCycleSummary();

            // reset the buffer counters
            ResetProcessingCounters();
        }

        // reset for next buffers-worth
        public override void ResetProcessingCounters()
        {
            base.ResetProcessingCounters();
            lastValue = 0;
            for (int i = 0; i < NC.ChannelCount; i++)
                hitsPerChn[i] = 0.0;
        }

		/// <summary>
		/// Accumulate any remaining data on the cycle object from the time/hits processor
		/// Final high-voltage value is copied from data source 
		/// time interval is either last neutron or requested time interval
		/// </summary>
		public override void AccumulateCycleSummary()
		{
			base.AccumulateCycleSummary();
			if (base.numValuesParsed > 0 && numValuesParsed <= timeArray.Count)  // devnote: semantic change to list usage, was // reset in StartNewBuffer, so if == 0 then previous counts have already been accumulated on this cycle, or  pathological case: 0 values parsed => a file with no data but with a summary block at the end.
			{
				if (cycle.TS.Ticks == 0L)
				{
					double timebase = sup.Handler.ticSizeInSeconds;
					long tiks = 0;
					// convert to a TimeSpan
					if (timebase == 1e-7)  // LMMM NCD 
						tiks = (long)timeArray[(int)numValuesParsed - 1];
					else if (timebase == 1e-8)  // dev note: hack test until I can abstract this based on input file type spec, so far we only have 1e-7 and 1e-8 units
						tiks = (long)(timeArray[(int)numValuesParsed - 1] / 10);
					cycle.TS = TimeSpan.FromTicks(tiks); // This is the actual last time, used only if no requested time is specified on the cycle 
				}

				for (int i = 0; i < NC.ChannelCount; i++)
					cycle.HitsPerChannel[i] += hitsPerChn[i];
			}
		}

		// non-null StatusBlock returned when end of data encountered during processing
		public override StreamStatusBlock ConvertDataBuffer(int bytecount)
        {
            uint[] uintHolder1 = new uint[1];
            uint[] uintHolder2 = new uint[1];
            int index = 0;  //index into the active buffer of bytes from the last read 
            StreamStatusBlock res = null;

            // logger.TraceEvent(LogLevels.Verbose, 221, "{0}: Processing {1} bytes", numbuff, bytecount);
            /*
             *  case 0: UInt32 /= 0 is time, then next uint is channel hits
             *  case 1: UInt32 == 0 means end of data file, next uint is length of subsequent terminating ASCII message
             *  case 2: UInt32 == 0 means veto, following uint is skipped
             *  case 3: if gen2 flag is set we do an expensive examination for "trigger" because the null byte tests for gen3 do not work 
             * */
            while (index < bytecount)
            {
                //if ((index % (16 * 8192)) == 0) // dev note: could, maybe even should, flush log here based on some configurable modulus
                //{
                //    NC.App.Loggers.Flush();
                //}

                // dev note: review this code to remove all un-needed steps, even copies
                Buffer.BlockCopy(rawDataBuff, index, uintHolder1, 0, 4);
                index += 4;
                Buffer.BlockCopy(rawDataBuff, index, uintHolder2, 0, 4); //get time of event from array
                index += 4;

                if (uintHolder1[0] == 0)
                {
                    if (uintHolder2[0] == 0)  // 8 null bytes means end of the cycle
                    {
                        res = ExtractStatusBlock(ref index, bytecount);
                        //logger.TraceEvent(LogLevels.Verbose, 8888, "{0}: END-OF-CYCLE BLOCK!", state.NumValuesParsed);
                    }
                    else  // it is a veto, and TDB set flag to find its pair
                    {
                        // dev note: do something with the veto time in uintHolder2[0] 
                        //logger.TraceEvent(LogLevels.Info, 8888, "{0}: Veto!", state.NumValuesParsed);
                    }
                }
                else if (includingGen2 && // unsure of best way to find these older files, so I look for the word "triggers", for a three step logical check
                    ((uintHolder1[0] == 0x67697274) && (uintHolder2[0] == 0x73726567))) // girt sreg 
                {
                    res = new StreamStatusBlock();
                    res.index = index - 8;
                    uint messagelength = (uint)(bytecount - res.index);
                    res.msglen = (int)((0x000000FF) & (messagelength >> 24)           //shift byte 1 to byte 4 and mask it
                                        | (0x0000FF00) & (messagelength >> 8)     //shift byte 2 to byte 3 and mask it
                                        | (0x00FF0000) & (messagelength << 8)     //shift byte 3 to byte 2 and mask it
                                        | (0xFF000000) & (messagelength << 24));  //shift byte 4 to byte 1 and mask it
                    logger.TraceEvent(LogLevels.Verbose, 219, "todo gen2: for gen 2 we improperly parsed the preceding 8 bytes 0x0002 0x000+ or 0x0002 0x000-, so need to back it out of the counts, and moreover there may be other end patterns I haven't seen yet for gen 2 files.");
                }
                else  //should have a valid pair of UInt32s.  Swap byte order for first value, and read and swap bytes for next value.
                {
                    uint aValue, swapped;

                    //swap endianness of neutron event, already parsed from array...
                    aValue = uintHolder1[0];
                    swapped = ((0x000000FF) & (aValue >> 24)   //shift byte 1 to byte 4 and mask it
                               | (0x0000FF00) & (aValue >> 8)    //shift byte 2 to byte 3 and mask it
                               | (0x00FF0000) & (aValue << 8)    //shift byte 3 to byte 2 and mask it
                               | (0xFF000000) & (aValue << 24)); //shift byte 4 to byte 1 and mask it

                    neutronEventArray[(int)NumValuesParsed] = swapped;

                    for (short i = 0; i < NC.ChannelCount; i++)  // count channel hits here
                    {
                        if ((swapped & chnmask[i]) != 0)
                        {
                            hitsPerChn[i]++;
                            NumTotalsEncountered++;
                        }
                    }
                    //swap endianness of time of event, note: times are cumulative ticks, not deltas
                    aValue = uintHolder2[0];
                    swapped = ((0x000000FF) & (aValue >> 24)   //shift byte 1 to byte 4 and mask it
                               | (0x0000FF00) & (aValue >> 8)    //shift byte 2 to byte 3 and mask it
                               | (0x00FF0000) & (aValue << 8)    //shift byte 3 to byte 2 and mask it
                               | (0xFF000000) & (aValue << 24)); //shift byte 4 to byte 1 and mask it
                    if (swapped < lastValue)  //then we have wrapped around, so increment the wraparoundoffset by 33 bits
                    {
                        wraparoundOffset += 0x100000000;  // max clock
                    }
                    lastValue = swapped;

                    //store the time in the array, adding the wraparoundOffset in case the UInt32 has overflowed (which it will every 429.4967... seconds)
                    timeArray[(int)NumValuesParsed] = wraparoundOffset + ((ulong)swapped);

                    if (!usingStreamRawAnalysis) // drop them in, one by one
                        Sup.HandleANeutronEvent(timeArray[(int)NumValuesParsed], neutronEventArray[(int)NumValuesParsed]);

                    NumValuesParsed++;
                }
            }
            return res;
        }


        private StreamStatusBlock ExtractStatusBlock(ref int index, int bytecount)
        {
            int statusindex = index + 4;
            uint[] messagelength = new uint[1];
            Buffer.BlockCopy(rawDataBuff, index, messagelength, 0, 4);  // get length bytes

            uint len = messagelength[0];
            len = ((0x000000FF) & (len >> 24)           //shift byte 1 to byte 4 and mask it
                        | (0x0000FF00) & (len >> 8)     //shift byte 2 to byte 3 and mask it
                        | (0x00FF0000) & (len << 8)     //shift byte 3 to byte 2 and mask it
                        | (0xFF000000) & (len << 24));  //shift byte 4 to byte 1 and mask it

            StreamStatusBlock res = new StreamStatusBlock();
            // this sets up the SB for decoding later
            res.msglen = (int)len;
            res.index = statusindex;

            index = bytecount;  // set index to length of buffer, so loop will exit.
            return res;
        }
    }

    public class LMRawDataTransform : RDTBase
    {

        public byte[] RawDataBuff
        {
            get { return State.rawDataBuff; }
        }

        public new LMProcessingState State
        {
            get { return state as LMProcessingState; }
        }
        public int Stdlen
        {
            get { return LMProcessingState.stdlen; }
        }

        public uint CurEventBuffLen
        {
            get { return State.eventBufferLength; }
        }

        /// <summary>
        /// The current Cycle instance undergoing processing
        /// </summary>
        public override Cycle Cycle
        {
            get { return state.cycle; }
            set { state.cycle = value; }
        }

        private uint statusCheckCount;
        public uint StatusCheckCount
        {
            get { return statusCheckCount; }
        }


        // build up the requested analyses for this run from the list of analyzer params          
        internal void PrepareAndStartCountingAnalyzers(CountingAnalysisParameters ap)
        {
            if (ap == null)
            {
                logger.TraceEvent(LogLevels.Error, 1519, "No counting analyzers specified");
                return;
            }
 
            bool good = false;

            foreach (SpecificCountingAnalyzerParams sap in ap)
            {
                if (!sap.Active) // skip disabled analyzers
                    continue;
                if (sap is BaseRate)
                {
                    BaseRate b = (BaseRate)sap;
                    good = State.Sup.AddRates(b.gateWidthTics);
                    if (!good)
                    {
                        // perhaps the gate is too large
                        // dev note : each analyzer should publish it's limits with constant attributes on the class
                        good = State.Sup.AddRates(b.gateWidthTics = (ulong)1e7);
                        logger.TraceEvent(LogLevels.Warning, 1512, "Rate analyzer gatewidth created with default {0} ticks", b.gateWidthTics);
                    }
                }
                else if (sap is Multiplicity)
                {
                    Multiplicity m = (Multiplicity)sap;
                    good = State.Sup.AddMultiplicity(m, m.FA);
                }
                else if (sap is Rossi)
                {
                    Rossi r = (Rossi)sap;
                    good = State.Sup.AddRossi(r.gateWidthTics);
                }
                else if (sap is Feynman)
                {
                    Feynman f = (Feynman)sap;
                    good = State.Sup.AddFeynman(f.gateWidthTics);
                }
                else if (sap is TimeInterval)
                {
                    TimeInterval ti = (TimeInterval)sap;
                    good = State.Sup.AddTimeInterval(ti.gateWidthTics);
                }
                else if (sap is Coincidence)
                {
                    Coincidence co = (Coincidence)sap;
                    good = State.Sup.AddCoincidenceMatrix(co);
                }
            }
        }


        internal void SetupCountingAnalyzerHandler(Config cfg,
                                            double tickSizeInSeconds,
                                            RawAnalyzerHandler.AnalysesCompleted f = null,
                                            RawAnalyzerHandler.NeutronOutOfSequenceErrorEvent f2 = null,
                                            RawAnalyzerHandler.BlockCountMismatchErrorEvent f3 = null)
        {
            State.Sup.SetLogger(analogger);

            if (f == null) // use the local handle
                State.Sup.Construct((string s) =>
                {
                    AssayPendingComplete();
                    logger.TraceEvent(LogLevels.Info, 139, "Neutron counting: '" + s + "'");
                },
                (string s) =>
                {
                    AssayPendingComplete();
                    logger.TraceEvent(LogLevels.Error, 138, "Neutron counting processing stopped with error: '" + s + "'");
                    EndAnalysisImmediately();
                    throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in buffer handler PassToAnalysis
                },
                (string s) =>
                {
                    AssayPendingComplete();
                    logger.TraceEvent(LogLevels.Error, 137, "Neutron counting processing [Block] stopped with error: '" + s + "'");
                    EndAnalysisImmediately();
                    throw new FatalNeutronCountingException(s);  // emergency exit, caught and noted in buffer handler PassToAnalysis
                },
                 theTicSizeInSeconds: tickSizeInSeconds,
                 csa: NC.App.Opstate.CancelStopAbort); // starts the threads, so watch out!
            else
                State.Sup.Construct(f, f2, f3, theTicSizeInSeconds: tickSizeInSeconds, csa: NC.App.Opstate.CancelStopAbort); // ditto

            logger.TraceEvent(LogLevels.Verbose, 146, "Neutron counting task running, {0} time base", tickSizeInSeconds);
        }

		internal void ResetTickSizeInSeconds(double tickSizeInSeconds)
		{
			 State.Sup.ResetTickSizeInSeconds(tickSizeInSeconds);
		}

        public override void EndAnalysisImmediately()
        {
            // dev note: this can only be called ONCE in response to AnalyzerHandler.NeutronOutOfSequenceErrorEvent or AnalyzerHandler.BlockCountMismatchErrorEvent
            State.Sup.EndCountingImmediately();
        }

        public RawStatus ReadCountingProcessorStatus()
        {
            if (State == null || State.Sup == null)
                return null;
            State.Sup.GetCountingProcessorStatus();
            return State.Sup.Status;
        }

        public LMRawDataTransform()
        {
            state = new LMProcessingState();
        }

        public override void Init(LMLoggers.LognLM datalogger, LMLoggers.LognLM alogger)
        {
            base.Init(datalogger, alogger);
        }

        // used by file ops only
		public void SetLMState(NCCConfig.LMMMNetComm config, uint unitbytes = 8, bool useRawBuff = false)
        {
            State.useAsynch = config.UseAsynchAnalysis;
            State.includingGen2 = NC.App.AppContext.ParseGen2;
            State.usingStreamRawAnalysis = config.UsingStreamRawAnalysis;
            statusCheckCount = NC.App.AppContext.StatusPacketCount;
            State.InitParseBuffers(config.ParseBufferSize, unitbytes, useRawBuff);
        }

		public void SetLMStateFlags(NCCConfig.LMMMNetComm config)
        {
            State.useAsynch = config.UseAsynchAnalysis;
            State.includingGen2 = NC.App.AppContext.ParseGen2;
            State.usingStreamRawAnalysis = config.UsingStreamRawAnalysis;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="meas"></param>
        /// <returns></returns>
        public override bool EndOfCycleProcessing(Measurement meas, bool last = false)
        {
            bool happyfun = true;

            if (last)
            {
                if (NC.App.Opstate.SOH == NCC.OperatingState.Living) // all other states mean do not wait here
                {
                    // drain analyzer queue
                    EndCountingWhenFinishedWithPresentEventQueue();
                    // wait for results to be available
                    AssayPendingWait();
                }
                else
                {
                    logger.TraceEvent(LogLevels.Warning, 952, "Processing is '{0}', not waiting for analyzer completion", NC.App.Opstate.SOH);
                    EndAnalysisImmediately(); // again ok?
                }
                try
                {
                    // reset the counters 
                    State.Sup.ResetCompletely(closeCounters:true);
                }
                catch (Exception ex)
                {
                    logger.TraceEvent(LogLevels.Error, 143, "EndOfCycleProcessing reset internal error: " + ex.Message);
                    happyfun = false;
                }
            }
            else
            {
                if (NC.App.Opstate.SOH == NCC.OperatingState.Living) // all other states mean do not wait here
                {
                    // drain analyzer queue
                    EndCountingWhenFinishedWithPresentEventQueue();
                    // wait for results to be available
                    AssayPendingWait();
                }
                else
                {
                    logger.TraceEvent(LogLevels.Warning, 952, "Processing is '{0}', not waiting for analyzer completion", NC.App.Opstate.SOH);
                    EndAnalysisImmediately(); // again ok?
                }

                // get results and put them on the cycle object
                State.Sup.GetCountingResults();

                try
                {
                    // transfer results to my internal representation 
                    happyfun = state.TransferResults(meas.AnalysisParams);
                }
                catch (Exception ex)
                {
                    logger.TraceEvent(LogLevels.Error, 142, "EndOfCycleProcessing result transfer internal error: " + ex.Message);
                    happyfun = false;
                }
                try
                {
                    // reset the counters 
                    State.Sup.ResetCompletely(closeCounters:false);
                }
                catch (Exception ex)
                {
                    logger.TraceEvent(LogLevels.Error, 143, "EndOfCycleProcessing reset internal error: " + ex.Message);
                    happyfun = false;
                }
                try
                {
                    state.cycle.SetDatastreamEndStatus();
                    CycleProcessing.ApplyTheCycleConditioningSteps(state.cycle, meas);
                    meas.CycleStatusTerminationCheck(state.cycle);

                }
                catch (Exception ex)
                {
                    logger.TraceEvent(LogLevels.Error, 144, "EndOfCycleProcessing summary internal error: " + ex.Message);
                }
            }

            return happyfun;
        }

        public void EndCountingWhenFinishedWithPresentEventQueue()
        {
            State.Sup.EndCountingWhenFinishedWithPresentEventQueue();
        }

        public override void AssayPendingComplete()
        {
            state.assayPending.Set();
        }
        public override bool AssayPendingWait()
        {
            state.assayPending.Wait();
            return true;
        }
        public override void AssayPendingReset()
        {
            state.assayPending.Reset();
        }
        public override bool AssayPendingIsSet()
        {
            return state.assayPending.IsSet;
        }
        public override ManualResetEventSlim GetManualResetEvent()
        {
            return state.assayPending;
        }

        public override void StartCycle(Cycle cycle, object param = null)
        {
            state.StartCycle(cycle, param);
        }

        /// <summary>
        ///  Resets stream IO buffer indexing and moves accumulated data to cycle 
        ///  Optionally prepares Timespan var cycle.TS using max encountered ticks or shakes
        /// </summary>
        public void StartNewBuffer()
        {
            // accumulate interim totals on the cycle object, we are buffering a long stream
            State.StartNewBuffer();
        }

        public void PlaceStatusTextOnCurrentCycle(StreamStatusBlock sb)
        {
            ParseStatusBlock(sb, Cycle);
        }

        /// <summary>
        /// Convert a buffer of unprocessed tuples to time and hit streams, then send the converted streams to the counting task
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="idx"></param>
        /// <param name="bytecount"></param>
        /// <returns>A StatusBlock is returned at the end of a cycle File or DAQ</returns>
        public StreamStatusBlock PassBufferToTheCounters(byte[] buffer, int idx, int bytecount)
        {
            // dev note: if reading from socket stream, consider if socket return is larger than max buffer in here, what to do?
            // dev note: this buff copy is pointless in the single-threaded model
            Buffer.BlockCopy(buffer, idx, State.rawDataBuff, 0, bytecount);  // copy from the asynch socket caller
            StreamStatusBlock endofdata = null;

            if (state.NumValuesParsed >= (State.maxValuesInBuffer - (bytecount / 8))) // if limit will reach with this new data buffer of size bytecount, then must startnewbuffer here, AND must pass the "nearly full" stream buffers to the counters
            {
                logger.TraceEvent(LogLevels.Verbose, 644, state.NumValuesParsed + " >= " + (State.maxValuesInBuffer - (bytecount / 8)) + " starting new internal buffer");
                if (State.usingStreamRawAnalysis)
                    State.Sup.HandleAnArrayOfNeutronEvents(State.timeArray, State.neutronEventArray, (int)state.NumValuesParsed);
                StartNewBuffer(); // resets stream IO buffer indexing and moves accumulated data to cycle 
                logger.Flush();
            }

            endofdata = State.ConvertDataBuffer(bytecount);
            if (NumProcessedRawDataBuffers > 0) logger.TraceEvent(LogLevels.Verbose, 222, "{0}: Completed with {1} events", NumProcessedRawDataBuffers, state.NumValuesParsed);

            if (State.usingStreamRawAnalysis && (endofdata != null))
                State.Sup.HandleAnArrayOfNeutronEvents(State.timeArray, State.neutronEventArray, (int)State.NumValuesParsed);

            // logger.TraceEvent(LogLevels.Verbose, 224, "{0}: Worked {1} bytes", NumProcessedRawDataBuffers, bytecount);
            return endofdata;
        }

        public ulong NumProcessedRawDataBuffers
        {
            get ;
            set ;
        }
        /// <summary>
        /// file-based buffer processing
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public StreamStatusBlock PassBufferToTheCounters(int count)
        {
            StreamStatusBlock endofdata = null;

            endofdata = state.ConvertDataBuffer(count);
            if (NumProcessedRawDataBuffers > 0) logger.TraceEvent(LogLevels.Verbose, 222, "{0}: Completed with {1} events", NumProcessedRawDataBuffers, state.NumValuesParsed);
			State.Sup.HandleAnArrayOfNeutronEvents(State.timeArray, State.neutronEventArray, (int)state.NumValuesParsed);
         
            return endofdata;
        }


        /// <summary>
        ///  Tailored only for LMMM/NPOD! 
        ///  this looks at the status block content and sets the cycle status; Rates, Assay Cancelled, Status, are the three I've seen in data so far
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="text"></param>
        /// <param name="stat"></param>
        public void ParseStatusBlock(StreamStatusBlock sb, Cycle cycle)
        {
            //'Assay Cancelled. ' == Cancelled, 
            // Completed ==  status block found at end of file or at cycle finish time
            // 'Rates = 0,0,0,0,1,6,0,54,0,0,0,0,1071,546,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0' == Rates (see e.g. GDND/2010_10_20_155930_0.ncd)
            CycleDAQStatus stat = CycleDAQStatus.UnspecifiedTruncation;
            string text = "";
            if (sb != null)
            {
                sb.Decode(State.rawDataBuff);
                if (!string.IsNullOrEmpty(sb.msg))  // dev note: this needs expansion to support other data stream end conditions, not just the orignal LMMM
                {
                    if (text.StartsWith("Assay Cancelled."))  // dev note: string constants that should live in the LMMMLingo class.
                        stat = CycleDAQStatus.Cancelled;
                    else if (text.StartsWith("Rates ="))
                        stat = CycleDAQStatus.Rates;
                    else
                        stat = CycleDAQStatus.Completed;
                }
                else
                {
                    stat = CycleDAQStatus.UnspecifiedTruncation;
                }
            }
            cycle.DaqStatus = stat;
            cycle.Message = text;
        }

    } // LMRawDataTransform


    // Dev note: input is typically in fractional shakes, but we want to capture those fractional times within the same shake, so two units are needed, shakes and a higher precision to count within regions
    public class PulseProcessingState : LMProcessingState
    {
        public double[] timeInBuffer;
        public byte[] chnbytes;
        bool skipchannels = true;

        internal PulseProcessingState(uint Max)
        {
            timeInBuffer = new double[Max];
            Reset();
        }

        public override StreamStatusBlock ConvertDataBuffer(int bytecount)
        {
            StreamStatusBlock ssb = null;
            // NEXT: detect end of stream and build ssb
            string msg = PrepRawStreams((ulong)bytecount, chnbytes);
            return ssb;
        }

        public void Reset()
        {
            FirstEventTimeInShakes = 0; TotalDups = 0; TotalEvents = 0; LastTimeInShakes = 0;
            chnbytes = new byte[4] { 0x0, 0x0, 0x0, 0x0 }; // 1 channel is active
        }

        public UInt64 FirstEventTimeInShakes;
        public UInt64 TotalDups;
        public UInt64 TotalEvents;
        public UInt64 LastTimeInShakes;

        public string PrepRawStreams(ulong num, byte[] chnbytes, bool combineDuplicateHits = false)
        {
            ulong ROllOverShakes = ulong.MaxValue;

            string issue = string.Empty;
            ulong dups = 0, events = 0;
            uint channels = NCCFile.ByteArray.ToUInt32(chnbytes);


            double lasttime = 0;
            ulong timeUI8B4 = 0;
            ulong timeUI8Shakes = 0, circuits = 0;
            double firstread = 0;

            try
            {
                foreach (double time in timeInBuffer)
                {                    
                    events++;

                    if (events > num)
                    {
                        events--;
                        break;
                    }

                    timeUI8Shakes = Convert.ToUInt64(time);

                    if (events == 1)
                    {
                        if (FirstEventTimeInShakes == 0)
                            FirstEventTimeInShakes = timeUI8Shakes;
                        firstread = time;
                    }

                    if (timeUI8Shakes >= ROllOverShakes)  // first 184467440737... secs are handled quickly, but here we must do a moddiv
                    {
                        throw new Exception("Sorry, times greater 184467440737 seconds (about 5.8K years) are unsupported, but soon will be!");
                    }
                    if (lasttime > time) // ooops! 
                    {
                        throw new Exception(string.Format("{0}, {1} ({2}, {3}) out-of-order, you forgot to sort", lasttime, time, timeUI8B4, timeUI8Shakes));
                    }
                    else if (timeUI8B4 == timeUI8Shakes) // a duplicate ! 
                    {
                        dups++;
                        logger.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate event {0} at time {1} (due to hit within the same shake)", events, timeUI8B4);
                        continue;
                    }

                    timeUI8B4 = timeUI8Shakes;

                    // fill in the arrays for the analyzers
                    if (skipchannels)
                    {
                        neutronEventArray[(int)NumValuesParsed] = channels;
                    }
                    else
                    {
                        channels = NCCFile.ByteArray.ToUInt32(chnbytes);
                    }

                    // todo: can use the skipchannels single channel flag to optimize this loop to a single increment in hitsPerChn
                    for (short i = 0; i < NC.ChannelCount; i++)  // count channel hits here
                    {
                        if ((channels & chnmask[i]) != 0)
                        {
                            hitsPerChn[i]++;
                            NumTotalsEncountered++;
                        }
                    }

                    timeArray[(int)NumValuesParsed] = timeUI8B4;

                    if (!usingStreamRawAnalysis) // drop them in, one by one
                        Sup.HandleANeutronEvent(timeArray[(int)NumValuesParsed], neutronEventArray[(int)NumValuesParsed]);

                    NumValuesParsed++;

                    lasttime = time;
                }

                logger.TraceEvent(LogLevels.Verbose, 3338, "Converted {0} events between {1} and {2} shakes ({3} rollovers) ({4} duplicates skipped)", events, firstread, lasttime, circuits, dups);
            }
            catch (Exception e)
            {
                logger.TraceEvent(LogLevels.Verbose, 3339, "Converted {0} events between {1} and {2} shakes ({3} rollovers) ({4} duplicates skipped)", events, firstread, lasttime, circuits, dups);
                logger.TraceEvent(LogLevels.Warning, 3363, "Error parsing pulses encountered '{0}'", e.Message);
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                issue = e.Message;
            }
            TotalEvents += events;
            TotalDups += dups;
            LastTimeInShakes = timeUI8Shakes;
            return issue;
        }



    }


    public class PTRFileProcessingState : LMProcessingState
    {
        public uint[] channels;
        public double[] times;
        public byte[] chnInBuffer;
        public uint[] timeInBuffer;
        public bool mergeDuplicatesTimeChannelHits = true; // dev note: create external flag to toggle the use of this
        //New variable to track the reported count time in PTR file. HN 10.15.2015
        public long PTRReportedCountTime = 0;
        internal PTRFileProcessingState(uint Max, LMProcessingState src)
        {
            NC.App.Loggers.Logger(LMLoggers.AppSection.Data).TraceEvent(LogLevels.Verbose, 3337, "Max is {0} in PTRFileProcessingState; allocating {1} bytes", Max, (Max * 4) + Max + (Max * 8) + (Max * 4));
            channels = new uint[Max];
            times = new double[Max];
            chnInBuffer = new byte[Max];
            timeInBuffer = new uint[Max];
            Reset();

            // shallow copy src state vars
            hitsPerChn = src.hitsPerChn;
            chnmask = src.chnmask;
            StartCycle(null);
            assayPending = src.assayPending;

            useAsynch = src.useAsynch;
            includingGen2 = src.includingGen2;
            usingStreamRawAnalysis = src.usingStreamRawAnalysis;

            eventBufferLength = src.eventBufferLength;
            rawDataBuff = src.rawDataBuff;
            maxValuesInBuffer = src.maxValuesInBuffer;
            timeArray = src.timeArray;
            neutronEventArray = src.neutronEventArray;

            Sup = src.Sup;
        } 

        public override StreamStatusBlock ConvertDataBuffer(int bytecount)
        {
            StreamStatusBlock ssb = null;
            // NEXT: detect end of stream and build ssb

            // devnote: this rework might mess end up the stream end check, and could blow here
            // The analyzer code uses the list lengths to know when to stop counting neutrons. The buffer may be n, but the last neutron is in n - k.
            if (channels.Length < neutronEventArray.Count)
                neutronEventArray.RemoveRange(channels.Length - 1, neutronEventArray.Count - channels.Length);
            else if (channels.Length > neutronEventArray.Count)
                neutronEventArray.AddRange(new uint[channels.Length - neutronEventArray.Count]);

            if (times.Length < timeArray.Count)
                timeArray.RemoveRange(times.Length - 1, timeArray.Count - times.Length);
            else if (times.Length > timeArray.Count)
                timeArray.AddRange(new ulong[times.Length - timeArray.Count]);

            //Array.Resize(ref neutronEventArray, channels.Length);
            //Array.Resize(ref timeArray, times.Length);
            string msg = PrepRawStreams(num: (ulong)bytecount, combineDuplicateHits: mergeDuplicatesTimeChannelHits);
            return ssb;
        }

        public void Reset()
        {
            FirstEventTimeInShakes = 0; TotalDups = 0; TotalEvents = 0; LastTimeInShakes = 0;
        }

        public ulong FirstEventTimeInShakes;
        public ulong TotalDups;
        public ulong TotalEvents;
        public ulong LastTimeInShakes;

        public string PrepRawStreams(ulong num, bool combineDuplicateHits = false)
        {
            const ulong RollOverShakes = ulong.MaxValue;

            string issue = string.Empty;
            ulong dups = 0, events = 0;

            double lasttime = 0;
            ulong timeUI8B4 = 0;
            ulong timeUI8Shakes = 0, circuits = 0;
            double firstread = 0;

            try
            {
                uint chann_els = 0;
                foreach (double time in times)
                {
                    events++;

                    if (events > num)
                    {
                        events--;
                        break;
                    }

                    timeUI8Shakes = Convert.ToUInt64(time);

                    if (events == 1)
                    {
                        if (FirstEventTimeInShakes == 0)
                            FirstEventTimeInShakes = timeUI8Shakes;
                        firstread = time;
                    }

                    if (timeUI8Shakes >= RollOverShakes)  // first 184467440737.09551615 ... secs are handled quickly, but here we must do a moddiv
                    {
                        throw new Exception("Sorry, relative event times greater 184467440737 seconds are unsupported");
                    }
                    if (lasttime > time) // ooops! 
                    {
                        throw new Exception(string.Format("{0}, {1} ({2}, {3}) out-of-order, you forgot to sort", lasttime, time, timeUI8B4, timeUI8Shakes));
                    }
                    else if (timeUI8B4 == timeUI8Shakes) // found a duplicate! 
                    {
                        dups++;
                        if (combineDuplicateHits)
                        {
                            if (0 != (chann_els & channels[events]))
                            {
                                logger.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate channel hit event {0} [{1:x8}] at time {2}", events, channels[events], timeUI8B4);
                            }
                            else
                            {
                                chann_els |= channels[events];
                                logger.TraceEvent(LogLevels.Verbose, 3331, "Combining hits [{0:x8}] from duplicate event {1} ({2})[{3:x8}] at time {4}", chann_els, NumValuesParsed, events, channels[events], timeUI8B4);
                            }
                        }
                        else
                            logger.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate event {0} [{1:x8}] at time {2} (due to rounding)", events, channels[events], timeUI8B4);
                        continue;
                    }

                    timeUI8B4 = timeUI8Shakes;

                    // fill in the arrays for the analyzers
                    chann_els |= channels[NumValuesParsed];
                    neutronEventArray[(int)NumValuesParsed] = chann_els;

                    // todo: can use the skipchannels single channel flag to optimize this loop to a single increment in hitsPerChn
                    for (short i = 0; i < NC.ChannelCount; i++)  // count channel hits here
                    {
                        if ((neutronEventArray[(int)NumValuesParsed] & chnmask[i]) != 0)
                        {
                            hitsPerChn[i]++;
                            NumTotalsEncountered++;
                        }
                    }

                    timeArray[(int)NumValuesParsed] = timeUI8B4;
                    chann_els = 0;

                    if (!usingStreamRawAnalysis) // drop them in, one by one
                        Sup.HandleANeutronEvent(timeArray[(int)NumValuesParsed], neutronEventArray[(int)NumValuesParsed]);

                    NumValuesParsed++;
                    lasttime = time;
                }

                logger.TraceEvent(LogLevels.Verbose, 3338, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) ({5} duplicates skipped)({6})", events, NumValuesParsed, firstread, lasttime, circuits, dups, num);
            }
            catch (Exception e)
            {
                logger.TraceEvent(LogLevels.Verbose, 3339, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} rollovers) ({5} duplicates skipped)({6})", events, NumValuesParsed, firstread, lasttime, circuits, dups, num);
                logger.TraceEvent(LogLevels.Warning, 3363, "Error parsing pulses encountered '{0}'", e.Message);
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                issue = e.Message;
            }
            TotalEvents += events;
            TotalDups += dups;
            LastTimeInShakes = timeUI8Shakes;
            return issue;
        }



    }

    
public class MCA527FileProcessingState : LMProcessingState
    {
        public ulong[] timeInBuffer;
        // Track the reported count time
        public int ReportedCountTime = 0;
        internal MCA527FileProcessingState(uint Max, LMProcessingState src)
        {
            timeInBuffer = new ulong[Max];
            Reset();

            // shallow copy src state vars
            hitsPerChn = src.hitsPerChn;
            chnmask = src.chnmask;
            StartCycle(null);
            assayPending = src.assayPending;

            useAsynch = src.useAsynch;
            includingGen2 = src.includingGen2;
            usingStreamRawAnalysis = src.usingStreamRawAnalysis;

            eventBufferLength = src.eventBufferLength;
            rawDataBuff = src.rawDataBuff;
            maxValuesInBuffer = src.maxValuesInBuffer;
            timeArray = src.timeArray;
            neutronEventArray = src.neutronEventArray;

            Sup = src.Sup;
        } 

        public override StreamStatusBlock ConvertDataBuffer(int eventcount)
        {
            StreamStatusBlock ssb = null;
            // NEXT: detect end of stream and build ssb

            // devnote: this rework might mess end up the stream end check, and could blow here
            // The analyzer code uses the list lengths to know when to stop counting neutrons. The buffer may be n, but the last neutron is in n - k.
            if (timeInBuffer.Length < neutronEventArray.Count)
                neutronEventArray.RemoveRange(timeInBuffer.Length - 1, neutronEventArray.Count - timeInBuffer.Length);
            else if (timeInBuffer.Length > neutronEventArray.Count)
                neutronEventArray.AddRange(new uint[timeInBuffer.Length - neutronEventArray.Count]);

            if (timeInBuffer.Length < timeArray.Count)
                timeArray.RemoveRange(timeInBuffer.Length - 1, timeArray.Count - timeInBuffer.Length);
            else if (timeInBuffer.Length > timeArray.Count)
                timeArray.AddRange(new ulong[timeInBuffer.Length - timeArray.Count]);

            string msg = PrepRawStreams(num: (ulong)eventcount, combineDuplicateHits: true);
            return ssb;
        }

        public void Reset()
        {
            FirstEventTimeInShakes = 0; TotalDups = 0; TotalEvents = 0; LastTimeInShakes = 0;
        }

        public ulong FirstEventTimeInShakes;
        public ulong TotalDups;
        public ulong TotalEvents;
        public ulong LastTimeInShakes;

        public string PrepRawStreams(ulong num, bool combineDuplicateHits = false)
        {
            string issue = string.Empty;
            ulong dups = 0, events = 0;
            ulong lasttime = 0;
            double firstread = 0;

            try
            {
                foreach (ulong time in timeInBuffer)
                {
                    events++;

                    if (events > num)
                    {
                        events--;
                        break;
                    }

                    if (events == 1)
                    {
                        if (FirstEventTimeInShakes == 0)
                            FirstEventTimeInShakes = time;
                        firstread = time;
                    }
					if (lasttime > time) // ooops! 
                    {
                        throw new Exception(string.Format("{0}, {1} out-of-order, you forgot to sort", lasttime, time));
                    }
                    else if (lasttime == time) // found a duplicate! 
                    {
                        dups++;
                        // logger.TraceEvent(LogLevels.Verbose, 3337, "Skipping duplicate event {0} [{1:x8}] at time {2} (due to rounding)", events, channels[events], timeUI8B4);
                        continue;
                    }

                    // fill in the arrays for the analyzers
                    neutronEventArray[(int)NumValuesParsed] = 1;  // always the same
                    timeArray[(int)NumValuesParsed] = time;

					hitsPerChn[0]++;
                    NumTotalsEncountered++;

                    if (!usingStreamRawAnalysis) // drop them in, one by one
                        Sup.HandleANeutronEvent(timeArray[(int)NumValuesParsed], neutronEventArray[(int)NumValuesParsed]);

                    NumValuesParsed++;
                    lasttime = time;
                }

                logger.TraceEvent(LogLevels.Verbose, 3338, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} duplicates skipped)({5})", events, NumValuesParsed, firstread, lasttime, dups, num);
            }
            catch (Exception e)
            {
                logger.TraceEvent(LogLevels.Verbose, 3339, "Converted {0} hits ({1} events) between {2} and {3} shakes ({4} duplicates skipped)({5})", events, NumValuesParsed, firstread, lasttime, dups, num);
                logger.TraceEvent(LogLevels.Warning, 3363, "Error parsing pulses encountered '{0}'", e.Message);
                NC.App.Opstate.SOH = NCC.OperatingState.Trouble;
                issue = e.Message;
            }
            TotalEvents += events;
            TotalDups += dups;
            LastTimeInShakes = lasttime;
            return issue;
        }
    }
 
}