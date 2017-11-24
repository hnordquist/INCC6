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
using System.Collections.Generic;
using LMRawAnalysis;
namespace AnalysisDefs
{
    sealed public class CoincidenceResult
    {
        public UInt64 coincidenceGateWidth;
        public UInt64 coincidenceDeadDelay;

        public bool isSlowBackground;
        public UInt64 accidentalsDelay;  //nonzero iff isSlowBackground is true

        public double[][] RACoincidenceRate;
        public double[][] ACoincidenceRate;

        public CoincidenceResult(int numChannels)
        {
            int i;

            this.RACoincidenceRate = new double[numChannels][];
            this.ACoincidenceRate = new double[numChannels][];
            for (i = 0; i < numChannels; i++)
            {
                this.RACoincidenceRate[i] = new double[numChannels];
                this.ACoincidenceRate[i] = new double[numChannels];
            }
        }
    }

    sealed public class RossiAlphaResult
    {
        public UInt64 gateWidth;
        public UInt32[] gateData;

        public RossiAlphaResult(UInt64 gatewidth, UInt32[] gatedata)
        {
            int i;

            this.gateWidth = gatewidth;

            this.gateData = new UInt32[RawAnalysisProperties.numRAGatesPerWindow];
            for (i = 0; i < RawAnalysisProperties.numRAGatesPerWindow; i++)
            {
                this.gateData[i] = gatedata[i];
            }
        }
    }
    sealed public class RateResult
    {
        public UInt64 gateWidthInTics;
        public UInt32 numCompletedGates;
        public UInt32[] neutronsPerGate;
        public int numChannels;
        public UInt32[][] neutronsPerGatePerChannel;

        public RateResult(int numberOfChannels)
        {
            this.numChannels = numberOfChannels;
        }
    }
    sealed public class MultiplicityResult
    {
        public UInt64 multiplicityGateWidth;
        public UInt64 multiplicityDeadDelay;
        public double deadTimeCoefficientTinNanoSecs;
        public double deadTimeCoefficientAinMicroSecs;
        public double deadTimeCoefficientBinPicoSecs;
        public double deadTimeCoefficientCinNanoSecs;

        public bool isSlowBackground;
        public UInt64 accidentalsDelay;  //nonzero iff isSlowBackground is true

        public double singlesRatePerSecond;
        public double doublesRatePerSecond;
        public double triplesRatePerSecond;

        public double deadTimeCorrectedSinglesRate;
        public double deadTimeCorrectedDoublesRate;
        public double deadTimeCorrectedTriplesRate;

        public double dytlewskiDeadTimeCorrectedSinglesRate;
        public double dytlewskiDeadTimeCorrectedDoublesRate;
        public double dytlewskiDeadTimeCorrectedTriplesRate;

        //note: if isSlowBackground is true, then normalizedAccidentalDistribution is a copy of accidentalDistribution,
        //else is the accidentalDistribution scaled by the ratio of (num R+A gates) to (num fastBackground gates)
        public SortedDictionary<UInt64, UInt64> realPlusAccidentalDistribution = new SortedDictionary<UInt64, UInt64>();
        public SortedDictionary<UInt64, UInt64> accidentalDistribution = new SortedDictionary<UInt64, UInt64>();
        public SortedDictionary<UInt64, UInt64> normalizedAccidentalDistribution = new SortedDictionary<UInt64, UInt64>();

        public UInt64 maxABin;   //largest key in the Accidental dictionary - note is same for either accidentalDist or normalizedAccidentalDist
        public UInt64 maxRABin;  //largest key in the realPlusAccidental dictionary

        public Double[] alpha;
        public Double[] beta;

        //so-called Real+Accidentals (RA) factorial moments
        public double RAfactorialMoment0, RAfactorialMoment1, RAfactorialMoment2, RAfactorialMoment3;
        //so-called Accidentals (A) factorial moments
        public double AfactorialMoment0, AfactorialMoment1, AfactorialMoment2, AfactorialMoment3;
        public double AfactorialAlphaMoment1, RAfactorialAlphaMoment1;
        public double AfactorialBetaMoment2, RAfactorialBetaMoment2;

        public List<string> warnings = new List<string>();

    }
    sealed public class FeynmanResult
    {
        public UInt64 gateWidth;
        public Dictionary<UInt32, UInt32> numGatesHavingNumNeutrons = new Dictionary<UInt32, UInt32>();
        public UInt32 maxDictionaryKey;
        public double cbar;
        public double c2bar;
        public double c3bar;
        public double C;  //the denominator used during calculations of cbar, c2bar, and c3bar;
    }
    sealed public class EventSpacingResult
    {
        public UInt64 gateWidthInTics;
        public UInt32[] eventSpacingHistogram;
        public int maxIndexOfNonzeroHistogramEntry;  //the biggest index for which eventSpacingHistogram is nonzero

        public EventSpacingResult()
        {
            this.eventSpacingHistogram = new UInt32[RawAnalysisProperties.maxEventSpacing + 1];
        }
    }
 
}
