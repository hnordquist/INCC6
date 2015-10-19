/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

namespace LMRawAnalysis
{
    sealed public class RateAccumulator
    {
        public UInt64 zeroOffsetInTics;  //the time of the beginning of the zeroth gate
        public UInt32[] neutronsPerGate;
        public UInt32[][] neutronsPerGatePerChannel;
        public UInt32 numCompletedGates;
      
        public RateAccumulator next;

        public RateAccumulator(UInt64 GateZeroStartTimeInTics)
        {
            int i, j;
            this.zeroOffsetInTics = GateZeroStartTimeInTics;
            this.neutronsPerGate = new UInt32[RawAnalysisProperties.rateGatesPerAccumulator];
            this.neutronsPerGatePerChannel = new UInt32[RawAnalysisProperties.rateGatesPerAccumulator][];
            for (i = 0; i < RawAnalysisProperties.rateGatesPerAccumulator; i++)
            {
                this.neutronsPerGate[i] = 0;
                this.neutronsPerGatePerChannel[i] = new UInt32[RawAnalysisProperties.ChannelCount];
                for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
                {
                    this.neutronsPerGatePerChannel[i][j] = 0;
                }
            }
            this.numCompletedGates = 0;
            this.next = null;
        }
    }

    sealed public class RateGateAnalysis
    {
        public UInt64 gateWidth;
        public RateAccumulator gateStack;
        public UInt32 presentDataZeroGateIndexOffset;
        private RateAccumulator presentData;

        private UInt32[] chnmask;

        public RateGateAnalysis(UInt64 gateWidthInTics)
        {
            this.gateWidth = gateWidthInTics;
            this.gateStack = new RateAccumulator(0);
            this.presentData = this.gateStack;
            this.presentDataZeroGateIndexOffset = 0;

            this.chnmask = new UInt32[RawAnalysisProperties.ChannelCount];
            for (int i = 0; i < RawAnalysisProperties.ChannelCount; i++)
            {
                chnmask[i] = (uint)1 << i;
            }
        }

        public void HandleANeutronEvent(UInt64 eventTime, UInt32 eventNeutrons, UInt32 numNeutrons)
        {
            UInt32 whichGate;
            UInt32 relativeGateIndex;
            int j;

            whichGate = (UInt32)(eventTime / this.gateWidth);
            relativeGateIndex = whichGate - this.presentDataZeroGateIndexOffset;
            while (relativeGateIndex >= RawAnalysisProperties.rateGatesPerAccumulator)
            {
                UInt64 nextStartTime;

                //tell present data all its gates are filled
                this.presentData.numCompletedGates = RawAnalysisProperties.rateGatesPerAccumulator;
                //calculate the start time of the next block of gates
                nextStartTime = this.presentData.zeroOffsetInTics + (RawAnalysisProperties.rateGatesPerAccumulator * this.gateWidth);
                //create the next block of gates
                this.presentData.next = new RateAccumulator(nextStartTime);
                //point to the next block of gates
                this.presentData = this.presentData.next;
                if (this.presentData.next != null)
                {
                    this.presentData.next = null;
                }
                //subtract block size from the relativeGateIndex
                relativeGateIndex -= RawAnalysisProperties.rateGatesPerAccumulator;
                //increment presentDataZeroGateIndexOffset by the block size
                this.presentDataZeroGateIndexOffset += RawAnalysisProperties.rateGatesPerAccumulator;
            }
            //store the neutron data in the appropriate gate of the present block
            this.presentData.neutronsPerGate[relativeGateIndex] += numNeutrons;
            this.presentData.numCompletedGates = relativeGateIndex;
            //having stored the total, now store totals by individual channels for all RawAnalysisProperties.ChannelCount possible channels
            for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
            {
                if ((eventNeutrons & this.chnmask[j]) != 0)
                {
                    this.presentData.neutronsPerGatePerChannel[relativeGateIndex][j]++;
                }
            }
        }

        public AnalysisDefs.RateResult GetResult(LMLoggers.LognLM log)
        {
            AnalysisDefs.RateResult result;
            RateAccumulator aBlock;
            UInt32 i, j, blockOffset, index;

            result = new AnalysisDefs.RateResult(RawAnalysisProperties.ChannelCount);

            try
            {
                result.gateWidthInTics = this.gateWidth;
                result.numCompletedGates = this.presentDataZeroGateIndexOffset + this.presentData.numCompletedGates;
                result.neutronsPerGate = new UInt32[result.numCompletedGates];
                result.neutronsPerGatePerChannel = new UInt32[result.numCompletedGates][];

                aBlock = this.gateStack;
                blockOffset = 0;
                //copy data from all but the last block
                while (aBlock.next != null)
                {
                    for (i = 0; i < RawAnalysisProperties.rateGatesPerAccumulator; i++)
                    {
                        index = i + blockOffset;
                        result.neutronsPerGate[index] = aBlock.neutronsPerGate[i];
                        result.neutronsPerGatePerChannel[index] = new UInt32[RawAnalysisProperties.ChannelCount];
                        for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
                        {
                            result.neutronsPerGatePerChannel[index][j] = aBlock.neutronsPerGatePerChannel[i][j];
                        }
                    }
                    blockOffset += RawAnalysisProperties.rateGatesPerAccumulator;
                    aBlock = aBlock.next;
                }
                //copy data from the last block
                for (i = 0; i < aBlock.numCompletedGates; i++)
                {
                    index = i + blockOffset;
                    result.neutronsPerGate[index] = aBlock.neutronsPerGate[i];
                    result.neutronsPerGatePerChannel[index] = new UInt32[RawAnalysisProperties.ChannelCount];
                    for (j = 0; j < RawAnalysisProperties.ChannelCount; j++)
                    {
                        result.neutronsPerGatePerChannel[index][j] = aBlock.neutronsPerGatePerChannel[i][j];
                    }
                }
            }
            catch (System.OutOfMemoryException oom)
            {
                GC.Collect();
                if (log != null) log.TraceException(oom);
            }
            return (result);
        }

        /// <summary>
        /// ResetCompletely clears results and sets metadata for new data,
        /// such as running a new experiment or reading a new NCD file
        /// </summary>
        public void ResetCompletely(bool closeCounters)
        {
            this.gateStack = new RateAccumulator(0);
            this.presentData = this.gateStack;
            this.presentDataZeroGateIndexOffset = 0;
        }
    }
}
