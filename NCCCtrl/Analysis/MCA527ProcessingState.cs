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

namespace Analysis
{
    /// <summary>
    /// Converts raw data from a PTR-32.
    /// </summary>
    public class MCA527ProcessingState : LMProcessingState, IMCADeviceCallbackObject
    {

        /// <summary>
        /// Initializes an new instance of the <see cref="MCA527ProcessingState"/> class.
        /// </summary>
        public MCA527ProcessingState()
        {
            InitParseBuffers(1,4, false);
            m_writingFile = CentralizedState.App.AppContext.LiveFileWrite;
        }
        /// <summary>
        /// Perform initialization at the start of a new cycle.
        /// </summary>
        /// <param name="cycle">The cycle.</param>
        /// <param name="param">optional output file (MCAFile) class instance.</param>
        public override void StartCycle(Cycle cycle, object param = null)
        {
           base.StartCycle(cycle, param);

            if (cycle != null) {
				cycle.ExpectedTS = new TimeSpan((long)(CentralizedState.App.Opstate.Measurement.AcquireState.lm.Interval * TimeSpan.TicksPerSecond));
                m_writingFile = CentralizedState.App.AppContext.LiveFileWrite;
            }
            if (param != null)
            {
                file = (NCCFile.MCAFile)param;
            }
        }

		public void BeginSweep(uint sweepNumber)
		{
		}

		public void FinishedSweep(uint sweepNumber, double sweepDurationSeconds)
		{
		}

		public void ReadTimestamps(uint sweepNumber, uint[] timestamps)
		{
		}

        /// <summary>
        /// Converts data in the raw data buffer.
        /// </summary>
        /// <param name="count">The number of bytes in the raw data buffer to convert.</param>
        /// <returns><c>null</c></returns>
        public override StreamStatusBlock ConvertDataBuffer(int count)
        {
			// assign all neutrons to channel 0
			for (int i = 0; i < neutronEventArray.Count; i++)
			{
				neutronEventArray[i] = 1;
			}
			return null;
        }

        /// <summary>
        /// Performs clean up at the end of a cycle.
        /// </summary>
        public void EndOfCycleProcessing()
        {
            if (file != null)
                file.CloseWriter();
		}


		        /// <summary>
        /// The number of channels.
        /// </summary>
        public const int ChannelCount = 1;

        /// <summary>
        /// The clock frequency in hertz (Hz).
        /// </summary>
        public const long Frequency = 100000000;

        /// <summary>
        /// The maximum voltage in volts (V).
        /// </summary>
        public const int MaxVoltage = 2000;

        public NCCFile.MCAFile file;
        public MCADevice device;
        private bool m_writingFile;

    }
}
