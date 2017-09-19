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
using Device;
using NCC;
using System.Collections.Generic;

namespace Analysis
{
    /// <summary>
    /// Converts raw data from a PTR-32.
    /// </summary>
    public class Ptr32ProcessingState : LMProcessingState
    {

        /// <summary>
        /// Initializes an new instance of the <see cref="Ptr32ProcessingState"/> class.
        /// </summary>
        public Ptr32ProcessingState()
        {
            InitParseBuffers(1, 4, false);
            m_parser = new Ptr32Parser(Analyze);
            m_writingFile = CentralizedState.App.AppContext.LiveFileWrite;
        }

        /// <summary>
        /// Perform initialization at the start of a new cycle.
        /// </summary>
        /// <param name="cycle">The cycle.</param>
        public override void StartCycle(Cycle cycle, object param = null)
        {
            base.StartCycle(cycle, param);

            if (cycle != null) {
                m_parser.Reset((ulong) (Ptr32.Frequency * CentralizedState.App.Opstate.Measurement.AcquireState.lm.Interval));
				cycle.ExpectedTS = new TimeSpan((long)(CentralizedState.App.Opstate.Measurement.AcquireState.lm.Interval * TimeSpan.TicksPerSecond));
                m_writingFile = CentralizedState.App.AppContext.LiveFileWrite;
            }
            if (param != null)
            {
                file = (NCCFile.PTRFilePair)param;
            }
        }

        /// <summary>
        /// Converts data in the raw data buffer.
        /// </summary>
        /// <param name="count">The number of bytes in the raw data buffer to convert.</param>
        /// <returns><c>null</c></returns>
        public override StreamStatusBlock ConvertDataBuffer(int count)
        {
            m_parser.Parse(rawDataBuff, 0, count);
            return null;
        }

        /// <summary>
        /// Performs clean up at the end of a cycle.
        /// </summary>
        public void EndOfCycleProcessing()
        {
            m_parser.Flush();
            if (file != null)
                file.CloseWriter();
        }

        /// <summary>
        /// Analyzes a sequence of events. Optionally writes data file pair
        /// </summary>
        /// <param name="times">The event times in clock ticks.</param>
        /// <param name="channelMasks">The event channel masks.</param>
        /// <param name="count">The number of events.</param>
        private void Analyze(List<ulong> times, List<uint> channelMasks, int count)
        {
            for (int i = 0; i < count; i++) {
                for (int channel = 0; channel < Ptr32.ChannelCount; channel++) {
                    if ((channelMasks[i] & (1u << channel)) != 0) {
                        cycle.HitsPerChannel[channel]++;
                        cycle.Totals++;
                    }
                }
            }

            cycle.TotalEvents += (ulong) count;
			if (cycle.TS.Ticks == 0L)
	            cycle.TS = TimeSpan.FromTicks((long) times[count - 1] / (Ptr32.Frequency / TimeSpan.TicksPerSecond)); // This is the actual last time, used only if no requested time is specified on the cycle

            if (cycle.Totals > 0 && cycle.TS.TotalSeconds > 0.0) {
                cycle.SinglesRate = cycle.Totals / cycle.TS.TotalSeconds;
            }

            Sup.HandleAnArrayOfNeutronEvents(times, channelMasks, count);

            if (m_writingFile)
            {
                file.Channels.Write(channelMasks, 0, count);
                file.Events.Write(times, 0, count);
            }
        }

		public void ResetRawDataBuffer()
		{
			if (rawDataBuff != null && rawDataBuff.Length != eventBufferLength)
				rawDataBuff = null;			
			if (rawDataBuff == null)
				rawDataBuff = new byte[eventBufferLength];
		}

        private Ptr32Parser m_parser;
        private NCCFile.PTRFilePair file;
        private bool m_writingFile;

    }
}
