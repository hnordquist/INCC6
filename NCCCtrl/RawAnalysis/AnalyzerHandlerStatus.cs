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

namespace LMRawAnalysis
{
    public enum StatusCode
    {
        Indeterminate,
        FinishedProcessingNeutrons,
        NowProcessingNeutrons,
        WaitingForNeutrons
    }
    
    
    sealed public class AnalyzerHandlerStatus
    {
        //basic info about the AnalyzerHandler status, one of the StatusCodes, above
        public StatusCode presentStatus;

        //number of different types of analyzers
        public int numFeynmanAnalyzers;
        public int numMultiplicityFastBackgroundAnalyzers;
        public int numMultiplicitySlowBackgroundAnalyzers;
        public int numRateAnalyzers;
        public int numRossiAlphaAnalyzers;
        public int numEventSpacingAnalyzers;
        public int numCoincidenceSlowBackgroundAnalyzers;

        //technical performance info about the AnalyzerHandler NeutronEvent queue
        public UInt64 numNeutronEventsReceived;
        public UInt64 numNeutronEventsProcessed, numNeutronEventsReceivedWhetherProcessedOrNot;
        public UInt64 capacityOfQueue;
        public UInt32 numCircuits;

        public Double PercentComplete
        {
            get { return (numNeutronEventsReceived > 0) ? (double)numNeutronEventsProcessed / (double)numNeutronEventsReceived : 0.0; }
    
        }


        public new String ToString() // new or override?
        {
            String result;
            switch (presentStatus)
            {
                case StatusCode.Indeterminate:
                    result = "We're hangin' out";
                    break;
                case StatusCode.FinishedProcessingNeutrons:
                    result = "Finished processing " + numNeutronEventsProcessed + " neutron events";
                    break;
                case StatusCode.NowProcessingNeutrons:
                    double pct = PercentComplete;
                    if (pct == 0.0)
                    {
                        result = String.Format("Preparing to process {0:N0} neutron events", numNeutronEventsReceived);
                    }
                    else
                    {
                        result = string.Format("{0:##.%}, processed {1:N0} of {2:N0} neutron events", pct, numNeutronEventsProcessed, numNeutronEventsReceived);
                    }
                    break;
                case StatusCode.WaitingForNeutrons:
                    result = "Awaiting more neutrons for processing";
                    break;
                default:
                    result = "Status code " + presentStatus.ToString();
                    break;
            }

            return (result);
        }
    }
}
