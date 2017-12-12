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
using LMRawAnalysis;

namespace AnalysisDefs
{
    using NC = NCC.CentralizedState;

    public enum RatesAdjustments {/* None,*/ Raw, DeadtimeCorrected, DytlewskiDeadtimeCorrected };//, BackgroundCorrected, Normalized, Final };

    [Flags]
    public enum QCTestStatus
    {
        None = 0, Pass = 0x001, Fail = 0x002, OutlierTestFail = 0x004, AccSinglesTestFail = 0x008,
        Checksum1 = 0x010,   /* test1 singles vs. sum of multiplicity accidentals */
        Checksum2 = 0x020, 	/* test2 shift register reals + accidentals v. sum of i * multiplicity reals + accidentals */
        Checksum3 = 0x040, 	/* test3 shift register accidentals vs. sum of i * multiplicity accidentals */
        HighVoltageFail = 0x080,
        Empty = 0x100 // "no time on cycle" "empty cycle" status
    };

    public static class QCTestStatusExtensions
    {

        //#define PASS "Pass"			/* string indicating all tests passed */
        //#define FAILURE "Fail"			/* string indicating a test failed */
        //#define PASS_FAIL_SIZE sizeof("Pass")	/* length of Pass/Fail string */
        //#define ALL "All"			/* string indicating select all */
        //#define OUTLIER_FAIL "Fail outlier test"
        //#define ACCIDENTALS_SINGLES_FAIL "Fail A/S test"
        // "Fail checksum #1"
        // "Fail checksum #2"
        // "Fail checksum #3"
        public static string INCCString(this QCStatus st)
        {
            string s = String.Empty;
            switch (st.status)
            {
                case QCTestStatus.None:
                    s = "-";
                    break;
                case QCTestStatus.Pass:
                    s = "Pass";
                    break;
                case QCTestStatus.Fail:
                    s = "Fail";
                    break;
                case QCTestStatus.OutlierTestFail:
                    s = "Fail outlier test";
                    break;
                case QCTestStatus.AccSinglesTestFail:
                    s = "Fail A/S test";
                    break;
                case QCTestStatus.Checksum1:
                    s = "Fail checksum #1";
                    break;
                case QCTestStatus.Checksum2:
                    s = "Fail checksum #2";
                    break;
                case QCTestStatus.Checksum3:
                    s = "Fail checksum #3";
                    break;
                case QCTestStatus.HighVoltageFail:
                    if (st.voltage != QCStatus.DefVoltage)
                        s = string.Format("Fail HV {0:F2}", st.voltage);
                    else
                        s = "Fail HV";
                    break;
                case QCTestStatus.Empty:
                    s = "Empty cycle";
                    break;
            }
            return s;
        }

        public static QCTestStatus FromString(string s)
        {
            QCTestStatus x = QCTestStatus.None;
            if (string.Equals(s, "Pass", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Pass;
            else if (String.Equals(s, "Fail", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Fail;
            else if (string.Equals(s, "Fail outlier test", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.OutlierTestFail;
            else if (string.Equals(s, "Fail A/S test", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.AccSinglesTestFail;
            else if (string.Equals(s, "Fail checksum #1", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Checksum1;
            else if (string.Equals(s, "Fail checksum #2", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Checksum2;
            else if (string.Equals(s, "Fail checksum #3", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Checksum3;
            else if (string.Compare(s,0,"Fail HV",0,7) == 0)
                x = QCTestStatus.HighVoltageFail;
            else if (string.Equals(s, "Empty cycle", StringComparison.OrdinalIgnoreCase))
                x = QCTestStatus.Empty;
            return x;
        }

        public static bool Passes(this QCStatus st) { return st.status == QCTestStatus.Pass; }
        public static bool Unset(this QCStatus st) { return st.status == QCTestStatus.None; }
        public static bool Fails(this QCStatus st) { return st.status != QCTestStatus.Pass; }

    }

    public class VTuple
    {
        public double v, sigma;

        public double err
        {
            get { return sigma; }
            set { sigma = value; }
        }
        public VTuple()
        {
        }
        public VTuple(VTuple t)
        {
            if (t == null)
                return;
            v = t.v;
            sigma = t.sigma;
        }
        public VTuple(double v, double sig)
        {
            this.v = v;
            sigma = sig;
        }

        public void Zero()
        {
            v = 0; sigma = 0;
        }

        static public VTuple Create()
        {
            return new VTuple();
        }

        static public VTuple Create(double v, double sig)
        {
            return new VTuple(v, sig);
        }

        public void CopyFrom(VTuple t)
        {
            v = t.v; sigma = t.sigma;
        }

        public override string ToString()
        {
            return "(" + v + ", " + sigma + ")";
        }

        public static VTuple[] MakeArray(int length)
        {
            VTuple[] res = new VTuple[length];
            for (int i = 0; i < length; i++)
                res[i] = new VTuple();
            return res;
        }
        public static VTuple[] MakeArray(double[] vals, double[] errs)
        {
            VTuple[] res = new VTuple[vals.Length];
            for (int i = 0; i < vals.Length; i++)
                res[i] = new VTuple(vals[i], errs[i]);
            return res;
        }

        public static VTuple[] Copy(VTuple[] vals)
        {
            VTuple[] res = new VTuple[vals.Length];
            for (int i = 0; i < vals.Length; i++)
                res[i] = new VTuple(vals[i]);
            return res;
        }

        public int CompareTo(object other)
        {
            return Compare(this, (VTuple)other);
        }

        public int Compare(VTuple x, VTuple y)
        {
            int res = x.v.CompareTo(y.v);
            if (res == 0)
                res = x.err.CompareTo(y.err);
            return res;
        }

    }

    public class StdRates: IComparable<StdRates>
    {
        public VTuple Quads
        {
            get { return quadsRate; }
            set { quadsRate = value; }
        }

        public VTuple Triples
        {
            get { return triplesRate; }
            set { triplesRate = value; }
        }

        public VTuple Doubles
        {
            get { return doublesRate; }
            set { doublesRate = value; }
        }

        public VTuple Singles
        {
            get { return singlesRate; }
            set { singlesRate = value; }
        }
        public VTuple Scaler1s
        {
            get { return scaler1Rate; }
            set { scaler1Rate = value; }
        }
        public VTuple Scaler2s
        {
            get { return scaler2Rate; }
            set { scaler2Rate = value; }
        }
        public double QuadsRate
        {
            get { return quadsRate.v; }
            set { quadsRate.v = value; }
        }

        public double TriplesRate
        {
            get { return triplesRate.v; }
            set { triplesRate.v = value; }
        }

        public double DoublesRate
        {
            get { return doublesRate.v; }
            set { doublesRate.v = value; }
        }

        public double SinglesRate
        {
            get { return singlesRate.v; }
            set { singlesRate.v = value; }
        }
        public double Scaler1Rate
        {
            get { return scaler1Rate.v; }
            set { scaler1Rate.v = value; }
        }
        public double Scaler2Rate
        {
            get { return scaler2Rate.v; }
            set { scaler2Rate.v = value; }
        }
        VTuple singlesRate, doublesRate, triplesRate, quadsRate, scaler1Rate, scaler2Rate;

        public StdRates()
        {
            singlesRate = new VTuple();
            doublesRate = new VTuple();
            triplesRate = new VTuple();
            quadsRate = new VTuple();
            scaler1Rate = new VTuple();
            scaler2Rate = new VTuple();
        }

        public StdRates(StdRates stdr)
        {
            if (stdr == null)
            {
                singlesRate = new VTuple();
                doublesRate = new VTuple();
                triplesRate = new VTuple();
                quadsRate = new VTuple();
                scaler1Rate = new VTuple();
                scaler2Rate = new VTuple();
            }
            else
            {
                singlesRate = new VTuple(stdr.singlesRate);
                doublesRate = new VTuple(stdr.doublesRate);
                triplesRate = new VTuple(stdr.triplesRate);
                quadsRate = new VTuple(stdr.quadsRate);
                scaler1Rate = new VTuple(stdr.scaler1Rate);
                scaler2Rate = new VTuple(stdr.scaler2Rate);
            }
        }

        public void CopyFrom(StdRates stdr)
        {
            if (stdr == null)
            {
                singlesRate = new VTuple();
                doublesRate = new VTuple();
                triplesRate = new VTuple();
                quadsRate = new VTuple();
                scaler1Rate = new VTuple();
                scaler2Rate = new VTuple();
            }
            else
            {
                singlesRate = new VTuple(stdr.singlesRate);
                doublesRate = new VTuple(stdr.doublesRate);
                triplesRate = new VTuple(stdr.triplesRate);
                quadsRate = new VTuple(stdr.quadsRate);
                scaler1Rate = new VTuple(stdr.scaler1Rate);
                scaler2Rate = new VTuple(stdr.scaler2Rate);
            }
        }

        // square the std values and add to this v value (sum of squares)
        public void AddSquare(StdRates std)
        {
            this.SinglesRate += (std.SinglesRate * std.SinglesRate);
            this.DoublesRate += (std.DoublesRate * std.DoublesRate);
            this.TriplesRate += (std.TriplesRate * std.TriplesRate);
            this.QuadsRate += (std.QuadsRate * std.QuadsRate);
            this.Scaler1Rate += (std.Scaler1Rate * std.Scaler1Rate);
            this.Scaler2Rate += (std.Scaler2Rate * std.Scaler2Rate);
        }

        // add the rates to this
        public void Add(StdRates std)
        {
            this.SinglesRate += std.SinglesRate;
            this.DoublesRate += std.DoublesRate;
            this.TriplesRate += std.TriplesRate;
            this.QuadsRate += std.QuadsRate;
            this.Scaler1Rate += std.Scaler1Rate;
            this.Scaler2Rate += std.Scaler2Rate;
        }

        // div the rates by a const
        public void Div(double d)
        {
            if (d == 0.0)
                return;
            this.SinglesRate /= d;
            this.DoublesRate /= d;
            this.TriplesRate /= d;
            this.QuadsRate /= d;
            this.Scaler1Rate /= d;
            this.Scaler2Rate /= d;
        }

        public void Zero()
        {
            this.singlesRate.Zero();
            this.doublesRate.Zero();
            this.triplesRate.Zero();
            this.quadsRate.Zero();
            this.scaler1Rate.Zero();
            this.scaler2Rate.Zero();
        }

        public override string ToString()
        {
            string s = "S: " + singlesRate.ToString();
            s += ", ";
            s += ("D: " + doublesRate.ToString());
            s += ", ";
            s += ("T: " + triplesRate.ToString());
            return s;
        }

        public string ToStringINCC()
        {
            string s = "s: " + singlesRate.ToString();
            s += ", ";
            s += ("d: " + doublesRate.ToString());
            s += ", ";
            s += ("t: " + triplesRate.ToString());
            s += ", ";
            s += ("q: " + quadsRate.ToString());
            s += ", ";
            s += ("sc1: " + scaler1Rate.ToString());
            s += ", ";
            s += ("sc2: " + scaler2Rate.ToString());
            return s;
        }

        public static int Compare(StdRates x, StdRates y)
        {
            int res = x.Singles.CompareTo(y.Singles);
            if (res == 0)
                res = x.Doubles.CompareTo(y.Doubles);
            if (res == 0)
                res = x.Triples.CompareTo(y.Triples);
                        if (res == 0)
                res = x.Scaler1s.CompareTo(y.Scaler1s);
                        if (res == 0)
                            res = x.Scaler2s.CompareTo(y.Scaler2s);
            if (res == 0)
                res = x.Quads.CompareTo(y.Quads);
            return res;
        }

        public int CompareTo(StdRates other)
        {
            return Compare(this, other);
        }
    }

    public class Rates : Dictionary<RatesAdjustments, StdRates>, IComparable<Rates>  // rates before and after corrections
    {
        public Rates()
        {
            foreach (RatesAdjustments e in System.Enum.GetValues(typeof(RatesAdjustments)))
            {
                this[e] = new StdRates();
            }
        }
        public Rates(Rates r)
        {
            foreach (RatesAdjustments e in System.Enum.GetValues(typeof(RatesAdjustments)))
            {
                this[e] = new StdRates(r[e]);
            }
        }
        public void CopyFrom(Rates r)
        {
            foreach (RatesAdjustments e in System.Enum.GetValues(typeof(RatesAdjustments)))
            {
                this[e].CopyFrom(r[e]);
            }
            dtchoice = r.dtchoice;
        }
        public new StdRates this[RatesAdjustments t]
        {
            get { return base[t]; }
            set { base.Add(t, value); }
        }
        public StdRates DeadtimeCorrectedRates
        {
            get { return this[RatesAdjustments.DeadtimeCorrected]; }
        }
        public StdRates DytlewskiDeadtimeCorrectedRates
        {
            get { return this[RatesAdjustments.DytlewskiDeadtimeCorrected]; }
        }
        public StdRates RawRates
        {
            get { return this[RatesAdjustments.Raw]; }
        }
        private RatesAdjustments dtchoice;
        public RatesAdjustments SetDT(RatesAdjustments ra)
        {
            RatesAdjustments prev = dtchoice;
            dtchoice = ra;
            return prev;
        }
        public StdRates DTCRates
        {
            get { return this[dtchoice]; }
        }
        // readonly getter
        public StdRates GetDTCRates(RatesAdjustments ra)
        {
            return this[ra];
        }

        public static int Compare(Rates x, Rates y)
        {
            int res = 0;
            foreach (RatesAdjustments e in System.Enum.GetValues(typeof(RatesAdjustments)))
            {
                res = x[e].CompareTo(y[e]);
                if (res != 0)
                    break;
            }
            return res;
        }

        public int CompareTo(Rates other)
        {
            return Compare(this, other);
        }
    }

    /// <summary>
    /// The Accumulate method is implemented for summing multi-cycle typed list mode data
    /// </summary>
    public interface ICountingResult
    {
        void Accumulate(ICountingResult from);

		void GenParamList();

		DB.ElementList ToDBElementList(bool generate = true);
    }

    public class HauckResult : ParameterBase, ICountingResult
    {
        public int numInitialRossiAlphaGatesIgnored;
        public double[] rossiAlphaResiduals;

        public double correctedSinglesRate;  // events per second
        public double correctedDoublesRate;  // events per second
        public double measuredSinglesRate;  // events per second
        public double measuredDoublesRate;  // events per second

        public HauckResult()
        {
        }

        public void Accumulate(ICountingResult fromthis)
        {
            HauckResult from = (HauckResult)fromthis;
            throw new InvalidOperationException("Dev note: HauckResult accumulation is undefined");
        }

    }

    public class TimeIntervalResult : ParameterBase, ICountingResult
    {
        public ulong gateWidthInTics;
        public uint[] timeIntervalHistogram;
        public int maxIndexOfNonzeroHistogramEntry;  //the biggest index for which eventSpacingHistogram is nonzero

        public TimeIntervalResult()
        {
            timeIntervalHistogram = new uint[RawAnalysisProperties.maxEventSpacing + 1];
        }


        public void Accumulate(ICountingResult fromthis)
        {
            TimeIntervalResult from = (TimeIntervalResult)fromthis;
            gateWidthInTics = from.gateWidthInTics; // assuming these are identical across the cycles
            maxIndexOfNonzeroHistogramEntry = Math.Max(maxIndexOfNonzeroHistogramEntry, from.maxIndexOfNonzeroHistogramEntry);
            for (int i = 0; i < from.timeIntervalHistogram.Length; i++)
                timeIntervalHistogram[i] += from.timeIntervalHistogram[i];
        }

        public void TransferRawResult(EventSpacingResult esr)
        {
            if (esr == null)
                return;
            gateWidthInTics = esr.gateWidthInTics;
            timeIntervalHistogram = new uint[esr.maxIndexOfNonzeroHistogramEntry + 1];
            maxIndexOfNonzeroHistogramEntry = esr.maxIndexOfNonzeroHistogramEntry;
            for (int i = 0; i <= esr.maxIndexOfNonzeroHistogramEntry; i++)
            {
                timeIntervalHistogram[i] = esr.eventSpacingHistogram[i];
            }
        }
		public override void GenParamList()
        {
            base.GenParamList();
            Table = "CyclesTIR";
            ps.Add(new DBParamEntry("counter_type", GetType().Name));  // "TimeInterval" not good to hard code the typename here
            ps.Add(new DBParamEntry("gateWidth", gateWidthInTics));
            ps.Add(new DBParamEntry("gateData", timeIntervalHistogram));
            ps.Add(new DBParamEntry("length", timeIntervalHistogram.Length));
        }
    }

    public class RossiAlphaResultExt : ParameterBase, ICountingResult
    {
        public ulong gateWidth;
        public uint[] gateData;

        public RossiAlphaResultExt(ulong gatewidth, uint[] gatedata)
        {
            int i;

            gateWidth = gatewidth;

            gateData = new uint[RawAnalysisProperties.numRAGatesPerWindow];
            for (i = 0; i < RawAnalysisProperties.numRAGatesPerWindow; i++)
            {
                gateData[i] = gatedata[i];
            }
        }

        public RossiAlphaResultExt()
        {

        }


        public void Accumulate(ICountingResult fromthis)
        {
            RossiAlphaResultExt from = (RossiAlphaResultExt)fromthis;
            gateWidth = from.gateWidth; // assuming these are identical across the cycles
            for (int i = 0; i < gateData.Length; i++)
                gateData[i] += from.gateData[i];
        }

        public void TransferRawResult(RossiAlphaResult rar)
        {
            if (rar == null)
                return;
            gateWidth = rar.gateWidth;

            gateData = new uint[rar.gateData.Length];
            for (int i = 0; i < rar.gateData.Length; i++)
            {
                gateData[i] = rar.gateData[i];
            }
        }

		public override void GenParamList()
        {
            base.GenParamList();
            Table = "CyclesTIR";
            ps.Add(new DBParamEntry("counter_type", GetType().Name));  // "Rossi" not good to hard code the typename here
            ps.Add(new DBParamEntry("gateWidth", gateWidth));
            ps.Add(new DBParamEntry("gateData", gateData));
            ps.Add(new DBParamEntry("length", gateData.Length));
        }
    }

    public class FeynmanResultExt : ParameterBase, ICountingResult
    {
        public ulong gateWidth;
        public Dictionary<uint, uint> numGatesHavingNumNeutrons = new Dictionary<uint, uint>();
        public uint maxDictionaryKey;
        public double cbar;
        public double c2bar;
        public double c3bar;
        public double C;  //the denominator used during calculations of cbar, c2bar, and c3bar;

		public FeynmanResultExt()
		{

		}

        public void Accumulate(ICountingResult fromthis)
        {
            FeynmanResultExt from = (FeynmanResultExt)fromthis;
            gateWidth = from.gateWidth; // assuming these are identical across the cycles
            // use gates, not the test param cgates
            foreach (KeyValuePair<uint, uint> frpair in from.numGatesHavingNumNeutrons)
            {
                uint numNeuts = frpair.Key;
                uint cgates = 0;
                uint gates = frpair.Value; // the number of gates having that numNeutrons
                if (numGatesHavingNumNeutrons.TryGetValue(numNeuts, out cgates))
                    numGatesHavingNumNeutrons[numNeuts] += gates;
                else
                    numGatesHavingNumNeutrons.Add(numNeuts, gates);
            }
        }

        public void TransferRawResult(FeynmanResult fr)
        {
            if (fr == null)
                return;

            cbar = fr.cbar;
            c2bar = fr.c2bar;
            c3bar = fr.c3bar;
            C = fr.C;
            gateWidth = fr.gateWidth;
            numGatesHavingNumNeutrons = new Dictionary<uint, uint>(fr.numGatesHavingNumNeutrons);
        }

		public override void GenParamList()
        {
            base.GenParamList();
            Table = "CyclesFeyn";
            ps.Add(new DBParamEntry("gateWidth", gateWidth));
            ps.Add(new DBParamEntry("cbar", cbar));
            ps.Add(new DBParamEntry("c2bar", c2bar));
            ps.Add(new DBParamEntry("c3bar", c3bar));
            ps.Add(new DBParamEntry("C", C));
        }
    }

    public class CoincidenceMatrixResult : ParameterBase, ICountingResult
    {
        public ulong coincidenceGateWidth;
        public ulong coincidenceDeadDelay;    // SR predelay

        public bool isSlowBackground;    // !FA always true for now
        public ulong accidentalsDelay;  //nonzero iff isSlowBackground is true

        public double[][] RACoincidenceRate;
        public double[][] ACoincidenceRate;

        public CoincidenceMatrixResult(int numChannels)
        {
            int i;

            RACoincidenceRate = new double[numChannels][];
            ACoincidenceRate = new double[numChannels][];
            for (i = 0; i < numChannels; i++)
            {
                RACoincidenceRate[i] = new double[numChannels];
                ACoincidenceRate[i] = new double[numChannels];
            }
        }

        public void TransferRawResult(CoincidenceResult cor)
        {
            if (cor == null)
                return;

            coincidenceGateWidth = cor.coincidenceGateWidth;
            coincidenceDeadDelay = cor.coincidenceDeadDelay;
            isSlowBackground = cor.isSlowBackground;
            accidentalsDelay = cor.accidentalsDelay;
            int numChannels = cor.RACoincidenceRate.GetLength(0);
            RACoincidenceRate = new double[numChannels][];
            ACoincidenceRate = new double[numChannels][];

            for (int i = 0; i < numChannels; i++)
            {
                RACoincidenceRate[i] = new double[numChannels];
                ACoincidenceRate[i] = new double[numChannels];
                Array.Copy(cor.RACoincidenceRate[i], RACoincidenceRate[i], numChannels);
                Array.Copy(cor.ACoincidenceRate[i], ACoincidenceRate[i], numChannels);
            }

        }

        public void Accumulate(ICountingResult fromthis)
        {
            CoincidenceMatrixResult from = (CoincidenceMatrixResult)fromthis;
            coincidenceGateWidth = from.coincidenceGateWidth; // assuming these are identical across the cycles
            //fromaccidentalsDelay = from.accidentalsDelay; // assuming these are identical across the cycles
            //fromcoincidenceDeadDelay = from.coincidenceDeadDelay; // assuming these are identical across the cycles
            int len = RACoincidenceRate.Length;  // assuming always symmetric numXnum

            for (int i = 0; i < RACoincidenceRate.Length; i++)
            {
                for (int j = 0; j < RACoincidenceRate.Length; j++)
                {
                    RACoincidenceRate[i][j] += from.RACoincidenceRate[i][j];
                    ACoincidenceRate[i][j] += from.ACoincidenceRate[i][j];
                }
            }
        }

        public override void GenParamList()
        {
            base.GenParamList();
            Table = "CyclesCoin";
            ps.Add(new DBParamEntry("backgroundgatewidth", accidentalsDelay)); // !FA so not used
            ps.Add(new DBParamEntry("accidentalsgatewidth", accidentalsDelay));
            ps.Add(new DBParamEntry("FA", !isSlowBackground));
            ps.Add(new DBParamEntry("counter_type", GetType().Name));  // not good to hard code the typename here
            ps.Add(new DBParamEntry("gateWidth", coincidenceGateWidth));
            ps.Add(new DBParamEntry("predelay", coincidenceDeadDelay));
            ps.Add(new DBParamEntry("numchn", RACoincidenceRate.Length));
            // todo: sparse rep of each numchn array
        }
    }

    public class RatesResultEnhanced : ParameterBase, ICountingResult
    {
        //public uint aggregateRate;  //summed over all channels, in events per gate
        //public uint[] channelRate;  //rate for each channel


        public TimeSpan totaltime;
        public ulong gateWidthInTics;
        public uint completedIntervals;
        public ulong[] neutronsPerChannel;
        public Array neutronsPerIntervalPerChannel;
        public Array neutronsPerInterval;
        public RatesResultEnhanced(uint gates, ulong gateWidthInTics)
        {
            this.gateWidthInTics = gateWidthInTics;
            completedIntervals = gates;
            neutronsPerChannel = new ulong[NC.ChannelCount];
            neutronsPerIntervalPerChannel = Array.CreateInstance(typeof(ulong), completedIntervals, NC.ChannelCount);
            neutronsPerInterval = Array.CreateInstance(typeof(ulong), completedIntervals);
            totaltime = new TimeSpan();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccrr">Matching base rates element from a single cycle</param>
        public void Accumulate(ICountingResult fromthis)
        {
            RatesResultEnhanced from = (RatesResultEnhanced)fromthis;
            for (uint i = 0; i < NC.ChannelCount; i++)  /* channel sums across cycles  */
            {
                neutronsPerChannel[i] += from.neutronsPerChannel[i];
            }

            for (int i = 0; i < Math.Min(completedIntervals, from.completedIntervals); i++)
            {
                for (int j = 0; j < NC.ChannelCount; j++)  /* sum each element across the cycles */
                {
                    ulong sum = (ulong)neutronsPerIntervalPerChannel.GetValue(i, j) + (ulong)from.neutronsPerIntervalPerChannel.GetValue(i, j);
                    neutronsPerIntervalPerChannel.SetValue(sum, i, j);
                }
                ulong sum2 = (ulong)neutronsPerInterval.GetValue(i) + (ulong)from.neutronsPerInterval.GetValue(i);
                neutronsPerInterval.SetValue(sum2, i);
                totaltime += from.totaltime;
            }
        }

        public void TransferRawResult(RateResult rates)
        {
            for (int i = 0; i < rates.numCompletedGates; i++)
            {
                for (int j = 0; j < NC.ChannelCount; j++)
                {
                    neutronsPerChannel[j] += rates.neutronsPerGatePerChannel[i][j];
                    neutronsPerIntervalPerChannel.SetValue(rates.neutronsPerGatePerChannel[i][j], i, j);
                }
                neutronsPerInterval.SetValue(rates.neutronsPerGate[i], i);
            }
        }
        public void Reset(uint gates)
        {
            if (gates == completedIntervals)
                return;
            completedIntervals = gates;
            neutronsPerIntervalPerChannel = Array.CreateInstance(typeof(ulong), completedIntervals, NC.ChannelCount);
            neutronsPerInterval = Array.CreateInstance(typeof(ulong), completedIntervals);
            totaltime = new TimeSpan();
        }

        public string NsChnImage()
        {
            String s = "";
            int i = 0;
            foreach (ulong h in neutronsPerChannel)
            {
                i++;
                s += string.Format("{0}, ", h);
            }
            return s;
        }
        public string NsChnIntervalImage(uint bin)
        {
            String s = "";
            for (int i = 0; i < NC.ChannelCount; i++)
            {
                uint h = (uint)neutronsPerIntervalPerChannel.GetValue(bin, i);
                s += string.Format("{0}, ", h);
            }
            return s;
        }
        public string NsPerIntervalImage(uint bin)
        {
            String s = "";
            int i = 0;
            foreach (ulong h in neutronsPerChannel)
            {
                i++;
                s += string.Format("{0}, ", h);
            }
            return s;
        }
    }

    public class AlphaBeta : ParameterBase, IEquatable<AlphaBeta>, IComparable<AlphaBeta>
    {

        public double[] α;
        public double[] β;

        public AlphaBeta()
        {
            Init();
        }
        public AlphaBeta(int length)
        {
            Init(length);
        }
        public AlphaBeta(AlphaBeta src)
        {
            if (src == null)
            {
                Init();
                return;
            }
            α = new double[src.α.Length];
            β = new double[src.β.Length];
            for (int i = 0; i < src.α.Length; i++)
            {
                α[i] = src.α[i];
            }
            for (int i = 0; i < src.β.Length; i++)
            {
                β[i] = src.β[i];
            }
        }
        public AlphaBeta(double[] a, double[] b)
        {
            α = new double[a.Length];
            β = new double[b.Length];
            for (int i = 0; i < a.Length; i++)
            {
                α[i] = a[i];
            }
            for (int i = 0; i < b.Length; i++)
            {
                β[i] = b[i];
            }
        }
        public void Init(int len = 0)
        {
            α = new double[len];
            β = new double[len];
        }
        public void Resize(AlphaBeta src)
        {
            if (src == null)
                return;
            Array.Resize(ref α, src.α.Length);
            Array.Resize(ref β, src.β.Length);
        }
        public void Resize(int length)
        {
            Array.Resize(ref α, length);
            Array.Resize(ref β, length);
        }
        public void TransferIntermediates(AlphaBeta src)
        {
            if (src == null)
                return;

			// the point of all this resizing across each cycle and during the summary steps was to avoid max bin allocations and the extra tracking of the actual indices, it's become bug spawing feature instead, jimbo.
            //if (src.α.Length < α.Length)
            //    new object();

            int amax = Math.Max(α.Length, src.α.Length);
            int amin = Math.Min(α.Length, src.α.Length);
            int bmax = Math.Max(β.Length, src.β.Length);
            int bmin = Math.Min(β.Length, src.β.Length);
            Array.Resize(ref α, amax);
            Array.Resize(ref β, bmax);

            for (int i = 0; i < amin; i++)
            {
                α[i] = src.α[i];
            }
            for (int i = 0; i < bmin; i++)
            {
                β[i] = src.β[i];
            }
            for (int i = amin; i < amax && i < src.α.Length; i++)
            {
                α[i] = src.α[i];
            }
            for (int i = bmin; i < bmax && i < src.β.Length; i++)
            {
                β[i] = src.β[i];
            }
        }

        public void TransferRawResult(MultiplicityResult mr)
        {
            if (mr == null)
                return;
            α = new double[mr.alpha.Length];
            β = new double[mr.beta.Length];
            for (int i = 0; i < mr.alpha.Length; i++)
            {
                α[i] = mr.alpha[i];
            }
            for (int i = 0; i < mr.beta.Length; i++)
            {
                β[i] = mr.beta[i];
            }
        }

        public bool Unset 
        {
            get { return (α == null || α.Length == 0 || (α.Length == 1 && α[0] == 0)) && (β == null || β.Length == 0 || (β.Length == 1 && β[0] == 0)); }
         }

        public static int Compare(AlphaBeta x, AlphaBeta y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = x.α.Length.CompareTo(y.α.Length);
            if (res == 0)
                res = x.β.Length.CompareTo(y.β.Length);

            if (res == 0)
            {
                for (int i = 0; res == 0 && i < x.α.Length; i++)
                {
                    res = x.α[i].CompareTo(y.α[i]);
                }
                for (int i = 0; res == 0 && i < x.β.Length; i++)
                {
                    res = x.β[i].CompareTo(y.β[i]);
                };
            }
            return res;
        }

        public int CompareTo(AlphaBeta other)
        {
            return Compare(this, other);
        }

        public bool Equals(AlphaBeta other)
        {
            return Compare(this, other) == 0;
        }

        //public override bool Equals(Object obj)
        //{
        //    if (obj == null) return base.Equals(obj);

        //    if (!(obj is AlphaBeta))
        //        throw new InvalidCastException("The 'obj' argument is not an AlphaBeta object.");
        //    else
        //        return Equals(obj as AlphaBeta);
        //}
        public override void GenParamList()
        {
            base.GenParamList();
            Table = "alpha_beta_rec";
            ps.Add(new DBParamEntry("alpha_array", α));
            ps.Add(new DBParamEntry("beta_array", β));
        }

        public uint MaxBins
        {
           get { if (Unset) return 0;
                 else return (uint)(Math.Max(α.Length, β.Length) + 1); }
        }


    }

	/// <summary>
	/// Cache of computed AlphaBeta values
	/// The calculation can be expensive with high bin counts
	/// This cache reduces the calculations to once per unique AlphaBeta key
	/// </summary>
	static public class AlphaBetaCache
	{
		static private Dictionary<ABKey, AlphaBeta> ABCache;

		static public AlphaBeta GetAlphaBeta(ABKey abkey)
		{
			AlphaBeta AB = null;
			if (ABCache == null)
				ABCache = new Dictionary<ABKey, AlphaBeta>();

			if (ABCache.ContainsKey(abkey))
			{
				//ABKey.log.TraceEvent(NCCReporter.LogLevels.Info, 5439, "Got " + abkey.ToString());
				AB = ABCache[abkey];
			}

			return AB;
		}

		static public void AddAlphaBeta(ABKey abkey, AlphaBeta AB)
		{
			if (ABCache == null)
				ABCache = new Dictionary<ABKey, AlphaBeta>();
			ABCache[abkey] = AB; //  new AlphaBeta(AB);
		}
	}

	/// <summary>
	/// 3 element Key for the Alpha Beta cache,
	/// Alpha beta depends upon
	///    Multiplicity bin count
	///    Phi or really the precursor Deadtime Coefficient T
	///    The gatewidth, in tics -e7
	///    
	/// So the cache uses a 3 element key
	/// </summary>
	public class ABKey : IEquatable<ABKey>, IComparable<ABKey>
	{

		public ABKey()
		{
		}

		public ABKey(Multiplicity mkey, MultiplicityCountingRes mcr)
		{
			bins = (uint)mcr.MaxBins;
			deadTimeCoefficientTinNanoSecs = mkey.sr.deadTimeCoefficientTinNanoSecs;
			gateWidthTics = mkey.gateWidthTics;
		}

		public ABKey(Multiplicity mkey, uint maxbins)
		{
			bins = maxbins;
			deadTimeCoefficientTinNanoSecs = mkey.sr.deadTimeCoefficientTinNanoSecs;
			gateWidthTics = mkey.gateWidthTics;
		}
		public bool Equals(ABKey other)
		{
			return other != null &&
				other.gateWidthTics == gateWidthTics &&
				other.bins == bins &&
				other.deadTimeCoefficientTinNanoSecs == deadTimeCoefficientTinNanoSecs;
		}

		public int CompareTo(ABKey other)
		{
			if (other == null)
				return 1;
			else
			{
				int res = bins.CompareTo(other.bins);
				if (res == 0)
					res = deadTimeCoefficientTinNanoSecs.CompareTo(other.deadTimeCoefficientTinNanoSecs);
				if (res == 0)
					res = gateWidthTics.CompareTo(other.gateWidthTics);
				return res;
			}
		}

		public override string ToString()
		{
			return gateWidthTics.ToString() + "," + deadTimeCoefficientTinNanoSecs.ToString() + "," + bins.ToString();
		}

		public override int GetHashCode()
		{
			return gateWidthTics.GetHashCode() ^ deadTimeCoefficientTinNanoSecs.GetHashCode() ^ bins.GetHashCode();
		}


		public uint bins;
		public double deadTimeCoefficientTinNanoSecs;
		public ulong gateWidthTics;
	}

	//use:
	//input is an mcr and an mkey

	//            // check if the arrays have not been computed
	//            AlphaBeta AB = GetAlphaBeta(abkey);
	//            if (AB != null)
	//                return AB;


	//// if not already computed and stored in cache, create a key and compute alphabeta and store the computed alpha beta in the cache
	//           ABKey abkey = new ABKey(mkey, mcr);
	//           mcr.AB = cached_calc_alpha_beta(abkey);

	//       static AlphaBeta cached_calc_alpha_beta(ABKey abkey)
	//       {
	//           // check if the arrays have not been computed
	//           AlphaBeta AB = GetAlphaBeta(abkey);
	//           if (AB != null)
	//               return AB;

	//           AB = new AlphaBeta((int)abkey.bins);
	//           AddAlphaBeta(abkey, AB);



<<<<<<< HEAD
	public class MultiplicityCountingRes : ParameterBase, ICountingResult
    {

        public TimeSpan TS;
        public FAType FA;
        public Rates rates;
        public double mass; // computed in outlier passes when qc_test is true, and further computed in verification pass, error is not computed so no VTuple
        public double efficiency, multiplication, multiAlpha; // computed in the same manner as mass but only in multiplicity analysis

        public ulong accidentalsDelay;
        public ulong[] RAMult, UnAMult, NormedAMult;//Adding αβ moments for debug of DTC HN
        ulong maxBins, minBins;
        AlphaBeta αβ;

        public double singles_multi;  // these results are not actually used in INCC, WTF?
        public double doubles_multi;
        public double triples_multi;

        public AlphaBeta AB
        {
            get { return αβ; } // αβ
            set { αβ = value; }
        }
        public double[] alpha
        {
            get { return αβ.α; }
        }
        public double[] beta
        {
            get { return αβ.β; }
        }
        public double[] RAFactorialMoments;
        public double[] AFactorialMoments;
        public double AFactorialAlphaMoment1;
        public double RAFactorialAlphaMoment1;
        public double AFactorialBetaMoment2;
        public double RAFactorialBetaMoment2;

        double _RASum, _ASum, _UnASum, _S1Sum, _S2Sum;

        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public ulong MaxBins
        {
            get { return maxBins; }
            set { maxBins = value; }
        }
        public ulong MinBins
        {
            get { return minBins; }
            set { minBins = value; }
        }
        public double RASum
        {
            get { return _RASum; }
            set { _RASum = value; }
        }
        public double UnASum
        {
            get { return _UnASum; }
            set { _UnASum = value; }
        }
        public double ASum
        {
            get { return _ASum; }
            set { _ASum = value; }
        }
        public double S1Sum
        {
            get { return _S1Sum; }
            set { _S1Sum = value; }
        }
        public double S2Sum
        {
            get { return _S2Sum; }
            set { _S2Sum = value; }
        }
        // number of events, and hits per channel event
        VTuple singles; // aka totals and run_singles in INCC
        public double Totals
        {
            get { return (ulong)singles.v; }
            set { singles.v = value; }
        }

        public int idx; // dev note: need a better identifier then just this number, oughta use detector id

        // carried over from old INCC code, computed in CalcAveragesAndSums, used in CalculateMultiplicity
        public double[] covariance_matrix;

        VTuple scaler1, scaler2; // for backwards compatibility with old Shift Register measurement data
        public VTuple Scaler1
        {
            get { return scaler1; }
            set { scaler1 = value; }
        }
        public VTuple Scaler2
        {
            get { return scaler2; }
            set { scaler2 = value; }
        }
        public VTuple Scaler1Rate
        {
            get { return rates[RatesAdjustments.Raw].Scaler1s; }
        }
        public VTuple Scaler2Rate
        {
            get { return rates[RatesAdjustments.Raw].Scaler2s; }
        }
        public VTuple RawSinglesRate
        {
            get { return rates[RatesAdjustments.Raw].Singles; }
        }
        public VTuple RawDoublesRate
        {
            get { return rates[RatesAdjustments.Raw].Doubles; }
        }
        public VTuple RawTriplesRate
        {
            get { return rates[RatesAdjustments.Raw].Triples; }
        }
        public VTuple DeadtimeCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Singles; }
        }
        public VTuple DeadtimeCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Doubles; }
        }
        public VTuple DeadtimeCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Triples; }
        }
        public VTuple DytlewskiCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Singles; }
        }
        public VTuple DytlewskiCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Doubles; }
        }
        public VTuple DytlewskiCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Triples; }
        }
        public StdRates DeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected]; }
        }
        public StdRates DytlewskiDeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected]; }
        }

        private Rates uncorrectedfrominccres;
        public Rates SemiCorrectedRate
        {
            get { return uncorrectedfrominccres; }
            set { uncorrectedfrominccres = value; }
        }

        public VTuple GetSemiCorrectedRate(RatesAdjustments choice, double val)
        {
            return uncorrectedfrominccres[choice].Doubles;
        }
        public void SetSemiCorrectedRate(RatesAdjustments choice, double val)
        {
            uncorrectedfrominccres[choice].Doubles.v = val;
        }
        public void SetSemiCorrectedRateErr(RatesAdjustments choice, double val)
        {
            uncorrectedfrominccres[choice].Doubles.err = val;
        }

        public void TransferIntermediates(MultiplicityCountingRes src)
        {
            αβ.TransferIntermediates(src.αβ);

            Array.Resize(ref RAFactorialMoments, src.RAFactorialMoments.Length);
            Array.Resize(ref AFactorialMoments, src.AFactorialMoments.Length);
            Array.Copy(src.RAFactorialMoments, RAFactorialMoments, RAFactorialMoments.Length);
            Array.Copy(src.AFactorialMoments, AFactorialMoments, AFactorialMoments.Length);
            AFactorialAlphaMoment1 = src.AFactorialAlphaMoment1;
            RAFactorialAlphaMoment1 = src.RAFactorialAlphaMoment1;
            AFactorialBetaMoment2 = src.AFactorialBetaMoment2;
            RAFactorialBetaMoment2 = src.RAFactorialBetaMoment2;
        }

        public void TransferSums(MultiplicityCountingRes src)
        {
            _RASum = src._RASum;
            _ASum = src._ASum;
            _UnASum = src._UnASum;
            _S1Sum = src._S1Sum;
            _S2Sum = src._S2Sum;
        }
        public void Resize(MultiplicityCountingRes src)
        {
            αβ.Resize(src.αβ);
            Array.Resize(ref RAFactorialMoments, src.RAFactorialMoments.Length);
            Array.Resize(ref AFactorialMoments, src.AFactorialMoments.Length);
            Array.Resize(ref RAMult, src.RAMult.Length);
            Array.Resize(ref UnAMult, src.UnAMult.Length);
            Array.Resize(ref NormedAMult, src.NormedAMult.Length);
            Array.Resize(ref covariance_matrix, src.covariance_matrix.Length);
			MinBins = src.MinBins;
			MaxBins = src.MaxBins;
        }
        public MultiplicityCountingRes(FAType FA, int i)
        {
            Init();
            this.FA = FA;
            idx = i;
        }
        public MultiplicityCountingRes(MultiplicityCountingRes src)
        {
            Init();
			CopyFrom(src);
        }
        public MultiplicityCountingRes()
        {
            Init();
            FA = FAType.FAOn;
            idx = 0;
        }
        protected void Init()
        {
            RAMult = new ulong[0];
            UnAMult = new ulong[0];
            NormedAMult = new ulong[0];

            αβ = new AlphaBeta();

            rates = new Rates();
            singles = new VTuple();
            RAFactorialMoments = new double[4];
            AFactorialMoments = new double[4];
            AFactorialAlphaMoment1 = 0;
            RAFactorialAlphaMoment1 = 0;
            AFactorialBetaMoment2 = 0;
            RAFactorialBetaMoment2 =0;
            multiplication = 1.0;
            uncorrectedfrominccres = new Rates();
            covariance_matrix = new double[3 * 3];
            scaler1 = new VTuple();
            scaler2 = new VTuple();
            TS = new TimeSpan();
        }
		public void CopyFrom(MultiplicityCountingRes src)
        {
            FA = src.FA;
            idx = src.idx;
            Array.Resize(ref RAFactorialMoments, src.RAFactorialMoments.Length);
            Array.Resize(ref AFactorialMoments, src.AFactorialMoments.Length);
            AFactorialAlphaMoment1 = src.AFactorialAlphaMoment1;
            RAFactorialAlphaMoment1 = src.RAFactorialAlphaMoment1;
            AFactorialBetaMoment2 = src.AFactorialBetaMoment2;
            RAFactorialBetaMoment2 = src.RAFactorialBetaMoment2;
            Array.Resize(ref RAMult, src.RAMult.Length);
            Array.Resize(ref UnAMult, src.UnAMult.Length);
            Array.Resize(ref NormedAMult, src.NormedAMult.Length);
            Array.Resize(ref covariance_matrix, src.covariance_matrix.Length);
            Array.Copy(src.RAMult, RAMult, RAMult.Length);
            Array.Copy(src.UnAMult, UnAMult, UnAMult.Length);
            Array.Copy(src.NormedAMult, NormedAMult, NormedAMult.Length);
            Array.Copy(src.covariance_matrix, covariance_matrix, covariance_matrix.Length);
            TransferIntermediates(src);
            singles = new VTuple(src.singles);
            rates = new Rates(src.rates);
            uncorrectedfrominccres = new Rates(src.uncorrectedfrominccres);
            accidentalsDelay = src.accidentalsDelay;
            multiplication = src.multiplication;

            maxBins = src.maxBins;
            minBins = src.minBins;
            _RASum = src._RASum;
            _ASum = src._ASum;
            _UnASum = src._UnASum;
            _S1Sum = src._S1Sum;
            _S2Sum = src._S2Sum;
            scaler1 = new VTuple(src.scaler1);
            scaler2 = new VTuple(src.scaler2);

            multiAlpha = src.multiAlpha;
            mass = src.mass;
            efficiency = src.efficiency;

            TS = new TimeSpan(src.TS.Ticks);

            singles_multi = src.singles_multi;
            doubles_multi = src.doubles_multi;
            triples_multi = src.triples_multi;
        }


        public void TransferRawResult(MultiplicityResult mr)
        {
            if (mr == null)
                return;

            accidentalsDelay = mr.accidentalsDelay;

            RawSinglesRate.v = mr.singlesRatePerSecond;
            RawDoublesRate.v = mr.doublesRatePerSecond;
            RawTriplesRate.v = mr.triplesRatePerSecond;

            DeadtimeCorrectedSinglesRate.v = mr.deadTimeCorrectedSinglesRate;
            DeadtimeCorrectedDoublesRate.v = mr.deadTimeCorrectedDoublesRate;
            DeadtimeCorrectedTriplesRate.v = mr.deadTimeCorrectedTriplesRate;

            DytlewskiCorrectedSinglesRate.v = mr.dytlewskiDeadTimeCorrectedSinglesRate;
            DytlewskiCorrectedDoublesRate.v = mr.dytlewskiDeadTimeCorrectedDoublesRate;
            DytlewskiCorrectedTriplesRate.v = mr.dytlewskiDeadTimeCorrectedTriplesRate;

            // dev note: these arrays can have up to 2 extra bins with 0s in them because the max bin numbers are larger than necessary for some conditions
            RAMult = new ulong[mr.maxRABin + 1];  // dev note: hope it's not too large o.w. must code for compressed intervals (sans 0 entries) instead of assuming iso bin intervals (includes 0 entries)

            // todo: check this: these copies are wrong! because non-zero entries are skipped in the dictionary
            foreach (KeyValuePair<ulong, ulong> pair in mr.realPlusAccidentalDistribution)
            {
                RAMult[pair.Key] = pair.Value;
            }
            UnAMult = new ulong[mr.maxABin + 1];
            foreach (KeyValuePair<ulong, ulong> pair in mr.accidentalDistribution)
            {
                UnAMult[pair.Key] = pair.Value;
            }
            NormedAMult = new ulong[mr.maxABin + 1];
            foreach (KeyValuePair<ulong, ulong> pair in mr.normalizedAccidentalDistribution)
            {
                NormedAMult[pair.Key] = pair.Value;
            }

            maxBins = Math.Max(mr.maxRABin + 1, mr.maxABin + 1);
            minBins = Math.Min(mr.maxRABin + 1, mr.maxABin + 1);  

            ComputeHitSums();

            αβ = new AlphaBeta(mr.alpha, mr.beta);

            RAFactorialMoments[0] = mr.RAfactorialMoment0;
            RAFactorialMoments[1] = mr.RAfactorialMoment1;
            RAFactorialMoments[2] = mr.RAfactorialMoment2;
            RAFactorialMoments[3] = mr.RAfactorialMoment3;
            AFactorialMoments[0] = mr.AfactorialMoment0;
            AFactorialMoments[1] = mr.AfactorialMoment1;
            AFactorialMoments[2] = mr.AfactorialMoment2;
            AFactorialMoments[3] = mr.AfactorialMoment3;
            //For now, don't put intermediats in results. HN
            AFactorialAlphaMoment1 = mr.AfactorialAlphaMoment1;
            RAFactorialAlphaMoment1 = mr.RAfactorialAlphaMoment1;
            AFactorialBetaMoment2 = mr.AfactorialBetaMoment2;
            RAFactorialBetaMoment2 = mr.RAfactorialBetaMoment2;
        }

        public void ComputeHitSums()
        {
            double res = 0;
            //According to Martyn, these are "hits", not sums.  hn 5.13.2015
            // The term 'sums' for these values comes from the INCC5 source code.
            // INCC5 terminology is retained throughout the ported code as much as possible, to assist with references to the originating source code
            for (int i = 0; i < UnAMult.Length; i++)
            {
                res += (double)UnAMult[i] * i;
            }
            _UnASum = res;
            res = 0;
            for (int i = 0; i < NormedAMult.Length; i++)
            {
                res += (double)NormedAMult[i] * i;
            }
            _ASum = res;
            res = 0;
            for (int i = 0; i < RAMult.Length; i++)
            {
                res += (double) RAMult[i] * i;
            }
            _RASum = res;
        }

        public string Name
        {
            get { return ShortName(); }
        }


        public string ShortName()
        {
            string s = "";
            if (FA == FAType.FAOn)
            {
                s += "FA ";
            }
            else
            {
                s += "A ";
                s += Multiplicity.TimeUnitImage(accidentalsDelay);
            }
            return s;
        }

        public string[] StringifyCurrentMultiplicityDetails()
        {

            if (MaxBins > 1)
            {
                string[] x = new string[MaxBins + 11];
                int j = 0;
                x[j++] = Name;
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}{3,-20}", "Mult","R+A", "A(Normed)", "A(Raw)");
                for (uint i = 0; i < MinBins; i++)
                {
                    x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", i, RAMult[i], NormedAMult[i], UnAMult[i]);
                }
                for (ulong i = MinBins; i < MaxBins; i++)
                {
                    double RA, A, UA;
                    if ((ulong)RAMult.Length <= i)
                        RA = 0;
                    else
                        RA = RAMult[i];
                    if ((ulong)NormedAMult.Length <= i)
                        A = 0;
                    else
                        A = NormedAMult[i];
                    if ((ulong)UnAMult.Length <= i)
                        UA = 0;
                    else
                        UA = UnAMult[i];
                    x[j++] = string.Format("{0,10}\t{1,10}\t{2,10}\t{3,10}", i, RA, A, UA);
                }
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", "Sums", "R+A", "A(Normed)", "A(Raw)");
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", "", RASum, ASum, UnASum);
                x[j++] = string.Format("{0,-10}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "Moments", "0", "1", "2", "3");
                x[j++] = string.Format("{0,-20}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "RA", RAFactorialMoments[0], RAFactorialMoments[1], RAFactorialMoments[2], RAFactorialMoments[3]);
                x[j++] = string.Format("{0,-20}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "A", AFactorialMoments[0], AFactorialMoments[1], AFactorialMoments[2], AFactorialMoments[3]);
                //string ast = "Alpha"; for (int i = 0; i < alpha.Length; i++) ast += string.Format("\t{0,8}", alpha[i]);
                //x[j++] = ast;
                //string bst = "Beta"; for (int i = 0; i < beta.Length; i++) bst += string.Format("\t{0,8}", beta[i]);
                //x[j++] = bst;
                // In verbose mode, print out raw rates also, but do not print raw triples. hn 11.5.2014
                x[j++] = string.Format("Raw Rates: Singles: {0}, Doubles: {1}, Triples {2}", RawSinglesRate.v, RawDoublesRate.v, "------");
                x[j++] = string.Format("DTC Rates: Singles: {0}, Doubles: {1}, Triples {2}", DeadtimeCorrectedSinglesRate.v, DeadtimeCorrectedDoublesRate.v, DeadtimeCorrectedTriplesRate.v);
                x[j++] = string.Format("Scalers: {0}, {1}", Scaler1.v, Scaler2.v);
                return x;
            }
            else
            {
                string[] x = new string[2];
                int j = 0;
                x[j++] = Name;
=======
	public class MultiplicityCountingRes : ParameterBase, ICountingResult
    {

        public TimeSpan TS;
        public FAType FA;
        public Rates rates;
        public double mass; // computed in outlier passes when qc_test is true, and further computed in verification pass, error is not computed so no VTuple
        public double efficiency, multiplication, multiAlpha; // computed in the same manner as mass but only in multiplicity analysis

        public ulong accidentalsDelay;
        public ulong[] RAMult, UnAMult, NormedAMult;
        ulong maxBins, minBins;
        AlphaBeta αβ;

        public double singles_multi;  // these results are not actually used in INCC, WTF?
        public double doubles_multi;
        public double triples_multi;

        public AlphaBeta AB
        {
            get { return αβ; } // αβ
            set { αβ = value; }
        }
        public double[] alpha
        {
            get { return αβ.α; }
        }
        public double[] beta
        {
            get { return αβ.β; }
        }
        public double[] RAFactorialMoments;
        public double[] AFactorialMoments;
        public double AFactorialAlphaMoment1;
        public double RAFactorialAlphaMoment1;
        public double AFactorialBetaMoment2;
        public double RAFactorialBetaMoment2;

        double _RASum, _ASum, _UnASum, _S1Sum, _S2Sum;

        public double Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public ulong MaxBins
        {
            get { return maxBins; }
            set { maxBins = value; }
        }
        public ulong MinBins
        {
            get { return minBins; }
            set { minBins = value; }
        }
        public double RASum
        {
            get { return _RASum; }
            set { _RASum = value; }
        }
        public double UnASum
        {
            get { return _UnASum; }
            set { _UnASum = value; }
        }
        public double ASum
        {
            get { return _ASum; }
            set { _ASum = value; }
        }
        public double S1Sum
        {
            get { return _S1Sum; }
            set { _S1Sum = value; }
        }
        public double S2Sum
        {
            get { return _S2Sum; }
            set { _S2Sum = value; }
        }
        // number of events, and hits per channel event
        VTuple singles; // aka totals and run_singles in INCC
        public double Totals
        {
            get { return (ulong)singles.v; }
            set { singles.v = value; }
        }

        public int idx; // dev note: need a better identifier then just this number, oughta use detector id

        // carried over from old INCC code, computed in CalcAveragesAndSums, used in CalculateMultiplicity
        public double[] covariance_matrix;

        VTuple scaler1, scaler2; // for backwards compatibility with old Shift Register measurement data
        public VTuple Scaler1
        {
            get { return scaler1; }
            set { scaler1 = value; }
        }
        public VTuple Scaler2
        {
            get { return scaler2; }
            set { scaler2 = value; }
        }
        public VTuple Scaler1Rate
        {
            get { return rates[RatesAdjustments.Raw].Scaler1s; }
        }
        public VTuple Scaler2Rate
        {
            get { return rates[RatesAdjustments.Raw].Scaler2s; }
        }
        public VTuple RawSinglesRate
        {
            get { return rates[RatesAdjustments.Raw].Singles; }
        }
        public VTuple RawDoublesRate
        {
            get { return rates[RatesAdjustments.Raw].Doubles; }
        }
        public VTuple RawTriplesRate
        {
            get { return rates[RatesAdjustments.Raw].Triples; }
        }
        public VTuple DeadtimeCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Singles; }
        }
        public VTuple DeadtimeCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Doubles; }
        }
        public VTuple DeadtimeCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Triples; }
        }
        public VTuple DytlewskiCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Singles; }
        }
        public VTuple DytlewskiCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Doubles; }
        }
        public VTuple DytlewskiCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Triples; }
        }
        public StdRates DeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected]; }
        }
        public StdRates DytlewskiDeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected]; }
        }

        private Rates uncorrectedfrominccres;
        public Rates SemiCorrectedRate
        {
            get { return uncorrectedfrominccres; }
            set { uncorrectedfrominccres = value; }
        }

        public VTuple GetSemiCorrectedRate(RatesAdjustments choice, double val)
        {
            return uncorrectedfrominccres[choice].Doubles;
        }
        public void SetSemiCorrectedRate(RatesAdjustments choice, double val)
        {
            uncorrectedfrominccres[choice].Doubles.v = val;
        }
        public void SetSemiCorrectedRateErr(RatesAdjustments choice, double val)
        {
            uncorrectedfrominccres[choice].Doubles.err = val;
        }

        public void TransferIntermediates(MultiplicityCountingRes src)
        {
            αβ.TransferIntermediates(src.αβ);

            Array.Resize(ref RAFactorialMoments, src.RAFactorialMoments.Length);
            Array.Resize(ref AFactorialMoments, src.AFactorialMoments.Length);
            Array.Copy(src.RAFactorialMoments, RAFactorialMoments, RAFactorialMoments.Length);
            Array.Copy(src.AFactorialMoments, AFactorialMoments, AFactorialMoments.Length);
        }

        public void TransferSums(MultiplicityCountingRes src)
        {
            _RASum = src._RASum;
            _ASum = src._ASum;
            _UnASum = src._UnASum;
            _S1Sum = src._S1Sum;
            _S2Sum = src._S2Sum;
        }
        public void Resize(MultiplicityCountingRes src)
        {
            αβ.Resize(src.αβ);
            Array.Resize(ref RAFactorialMoments, src.RAFactorialMoments.Length);
            Array.Resize(ref AFactorialMoments, src.AFactorialMoments.Length);
            Array.Resize(ref RAMult, src.RAMult.Length);
            Array.Resize(ref UnAMult, src.UnAMult.Length);
            Array.Resize(ref NormedAMult, src.NormedAMult.Length);
            Array.Resize(ref covariance_matrix, src.covariance_matrix.Length);
			MinBins = src.MinBins;
			MaxBins = src.MaxBins;
        }
        public MultiplicityCountingRes(FAType FA, int i)
        {
            Init();
            this.FA = FA;
            idx = i;
        }
        public MultiplicityCountingRes(MultiplicityCountingRes src)
        {
            Init();
			CopyFrom(src);
        }
        public MultiplicityCountingRes()
        {
            Init();
            FA = FAType.FAOn;
            idx = 0;
        }
        protected void Init()
        {
            RAMult = new ulong[0];
            UnAMult = new ulong[0];
            NormedAMult = new ulong[0];

            αβ = new AlphaBeta();

            rates = new Rates();
            singles = new VTuple();
            RAFactorialMoments = new double[4];
            AFactorialMoments = new double[4];
            multiplication = 1.0;
            uncorrectedfrominccres = new Rates();
            covariance_matrix = new double[3 * 3];
            scaler1 = new VTuple();
            scaler2 = new VTuple();
            TS = new TimeSpan();
        }
		public void CopyFrom(MultiplicityCountingRes src)
        {
            FA = src.FA;
            idx = src.idx;
            Array.Resize(ref RAMult, src.RAMult.Length);
            Array.Resize(ref UnAMult, src.UnAMult.Length);
            Array.Resize(ref NormedAMult, src.NormedAMult.Length);
            Array.Resize(ref covariance_matrix, src.covariance_matrix.Length);
            Array.Copy(src.RAMult, RAMult, RAMult.Length);
            Array.Copy(src.UnAMult, UnAMult, UnAMult.Length);
            Array.Copy(src.NormedAMult, NormedAMult, NormedAMult.Length);
            Array.Copy(src.covariance_matrix, covariance_matrix, covariance_matrix.Length);
            TransferIntermediates(src);
            singles = new VTuple(src.singles);
            rates = new Rates(src.rates);
            uncorrectedfrominccres = new Rates(src.uncorrectedfrominccres);
            accidentalsDelay = src.accidentalsDelay;
            multiplication = src.multiplication;

            maxBins = src.maxBins;
            minBins = src.minBins;
            _RASum = src._RASum;
            _ASum = src._ASum;
            _UnASum = src._UnASum;
            _S1Sum = src._S1Sum;
            _S2Sum = src._S2Sum;
            scaler1 = new VTuple(src.scaler1);
            scaler2 = new VTuple(src.scaler2);

            multiAlpha = src.multiAlpha;
            mass = src.mass;
            efficiency = src.efficiency;

            TS = new TimeSpan(src.TS.Ticks);

            singles_multi = src.singles_multi;
            doubles_multi = src.doubles_multi;
            triples_multi = src.triples_multi;
        }


        public void TransferRawResult(MultiplicityResult mr)
        {
            if (mr == null)
                return;

            accidentalsDelay = mr.accidentalsDelay;

            RawSinglesRate.v = mr.singlesRatePerSecond;
            RawDoublesRate.v = mr.doublesRatePerSecond;
            RawTriplesRate.v = mr.triplesRatePerSecond;

            DeadtimeCorrectedSinglesRate.v = mr.deadTimeCorrectedSinglesRate;
            DeadtimeCorrectedDoublesRate.v = mr.deadTimeCorrectedDoublesRate;
            DeadtimeCorrectedTriplesRate.v = mr.deadTimeCorrectedTriplesRate;

            DytlewskiCorrectedSinglesRate.v = mr.dytlewskiDeadTimeCorrectedSinglesRate;
            DytlewskiCorrectedDoublesRate.v = mr.dytlewskiDeadTimeCorrectedDoublesRate;
            DytlewskiCorrectedTriplesRate.v = mr.dytlewskiDeadTimeCorrectedTriplesRate;

            // dev note: these arrays can have up to 2 extra bins with 0s in them because the max bin numbers are larger than necessary for some conditions
            RAMult = new ulong[mr.maxRABin + 1];  // dev note: hope it's not too large o.w. must code for compressed intervals (sans 0 entries) instead of assuming iso bin intervals (includes 0 entries)

            // todo: check this: these copies are wrong! because non-zero entries are skipped in the dictionary
            foreach (KeyValuePair<ulong, ulong> pair in mr.realPlusAccidentalDistribution)
            {
                RAMult[pair.Key] = pair.Value;
            }
            UnAMult = new ulong[mr.maxABin + 1];
            foreach (KeyValuePair<ulong, ulong> pair in mr.accidentalDistribution)
            {
                UnAMult[pair.Key] = pair.Value;
            }
            NormedAMult = new ulong[mr.maxABin + 1];
            foreach (KeyValuePair<ulong, ulong> pair in mr.normalizedAccidentalDistribution)
            {
                NormedAMult[pair.Key] = pair.Value;
            }

            maxBins = Math.Max(mr.maxRABin + 1, mr.maxABin + 1);
            minBins = Math.Min(mr.maxRABin + 1, mr.maxABin + 1);  

            ComputeHitSums();

            αβ = new AlphaBeta(mr.alpha, mr.beta);

            RAFactorialMoments[0] = mr.RAfactorialMoment0;
            RAFactorialMoments[1] = mr.RAfactorialMoment1;
            RAFactorialMoments[2] = mr.RAfactorialMoment2;
            RAFactorialMoments[3] = mr.RAfactorialMoment3;
            AFactorialMoments[0] = mr.AfactorialMoment0;
            AFactorialMoments[1] = mr.AfactorialMoment1;
            AFactorialMoments[2] = mr.AfactorialMoment2;
            AFactorialMoments[3] = mr.AfactorialMoment3;

        }

        public void ComputeHitSums()
        {
            double res = 0;
            //According to Martyn, these are "hits", not sums.  hn 5.13.2015
            // The term 'sums' for these values comes from the INCC5 source code.
            // INCC5 terminology is retained throughout the ported code as much as possible, to assist with references to the originating source code
            for (int i = 0; i < UnAMult.Length; i++)
            {
                res += (double)UnAMult[i] * i;
            }
            _UnASum = res;
            res = 0;
            for (int i = 0; i < NormedAMult.Length; i++)
            {
                res += (double)NormedAMult[i] * i;
            }
            _ASum = res;
            res = 0;
            for (int i = 0; i < RAMult.Length; i++)
            {
                res += (double) RAMult[i] * i;
            }
            _RASum = res;
        }

        public string Name
        {
            get { return ShortName(); }
        }


        public string ShortName()
        {
            string s = "";
            if (FA == FAType.FAOn)
            {
                s += "FA ";
            }
            else
            {
                s += "A ";
                s += Multiplicity.TimeUnitImage(accidentalsDelay);
            }
            return s;
        }

        public string[] StringifyCurrentMultiplicityDetails()
        {

            if (MaxBins > 1)
            {
                string[] x = new string[MaxBins + 11];
                int j = 0;
                x[j++] = Name;
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}{3,-20}", "Mult","R+A", "A(Normed)", "A(Raw)");
                for (uint i = 0; i < MinBins; i++)
                {
                    x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", i, RAMult[i], NormedAMult[i], UnAMult[i]);
                }
                for (ulong i = MinBins; i < MaxBins; i++)
                {
                    double RA, A, UA;
                    if ((ulong)RAMult.Length <= i)
                        RA = 0;
                    else
                        RA = RAMult[i];
                    if ((ulong)NormedAMult.Length <= i)
                        A = 0;
                    else
                        A = NormedAMult[i];
                    if ((ulong)UnAMult.Length <= i)
                        UA = 0;
                    else
                        UA = UnAMult[i];
                    x[j++] = string.Format("{0,10}\t{1,10}\t{2,10}\t{3,10}", i, RA, A, UA);
                }
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", "Sums", "R+A", "A(Normed)", "A(Raw)");
                x[j++] = string.Format("{0,-20}\t{1,-20}\t{2,-20}\t{3,-20}", "", RASum, ASum, UnASum);
                x[j++] = string.Format("{0,-10}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "Moments", "0", "1", "2", "3");
                x[j++] = string.Format("{0,-20}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "RA", RAFactorialMoments[0], RAFactorialMoments[1], RAFactorialMoments[2], RAFactorialMoments[3]);
                x[j++] = string.Format("{0,-20}\t{1,-10}\t{2,-10}\t{3,-10}\t{4,-10}", "A", AFactorialMoments[0], AFactorialMoments[1], AFactorialMoments[2], AFactorialMoments[3]);
                //string ast = "Alpha"; for (int i = 0; i < alpha.Length; i++) ast += string.Format("\t{0,8}", alpha[i]);
                //x[j++] = ast;
                //string bst = "Beta"; for (int i = 0; i < beta.Length; i++) bst += string.Format("\t{0,8}", beta[i]);
                //x[j++] = bst;
                // In verbose mode, print out raw rates also, but do not print raw triples. hn 11.5.2014
                x[j++] = string.Format("Raw Rates: Singles: {0}, Doubles: {1}, Triples {2}", RawSinglesRate.v, RawDoublesRate.v, "------");
                x[j++] = string.Format("DTC Rates: Singles: {0}, Doubles: {1}, Triples {2}", DeadtimeCorrectedSinglesRate.v, DeadtimeCorrectedDoublesRate.v, DeadtimeCorrectedTriplesRate.v);
                x[j++] = string.Format("Scalers: {0}, {1}", Scaler1.v, Scaler2.v);
                return x;
            }
            else
            {
                string[] x = new string[2];
                int j = 0;
                x[j++] = Name;
>>>>>>> 94570003551df64daeee65be0d76211f950d9ac5
                //x[j++] = "No RA or A data";
                x[j++] = string.Format("Raw Rates: Singles: {0}, Doubles: {1}", RawSinglesRate.v, RawDoublesRate.v);
                x[j++] = string.Format("DTC Rates: Singles: {0}, Doubles: {1}", DeadtimeCorrectedSinglesRate.v, DeadtimeCorrectedDoublesRate.v);
                x[j++] = string.Format("Scalers: {0}, {1}", Scaler1.v, Scaler2.v);

                return x;
            }
        }

        // Averages and sums happen in INCC CalcAvgAndSums, 
        // But keep this array resizing, it is a prep for CalcAvgAndSums
        public void Accumulate(ICountingResult fromthis)
        {
            MultiplicityCountingRes m = (MultiplicityCountingRes)fromthis;
            BinSumLoop(m);
            MaxBins = Math.Max(MaxBins, m.MaxBins);
            MinBins = Math.Max(MinBins, m.MinBins);
            αβ.TransferIntermediates(m.AB);
        }

        /// <summary>
        /// Sum RA and A on this instance from the param
        /// </summary>
        /// <param name="from"></param>
        public void BinSumLoop(MultiplicityCountingRes from)
        {

            for (uint i = 0; i < (uint)from.MinBins; i++)
            {
                RAMult[i] += from.RAMult[i];
                UnAMult[i] += from.UnAMult[i];
                NormedAMult[i] += from.NormedAMult[i];
            }
            for (uint i = (uint)from.MinBins; i < from.MaxBins; i++)
            {
                ulong RA, A, UA;
                if ((uint)from.RAMult.Length <= i)
                    RA = 0;
                else
                    RA = from.RAMult[i];
                if ((uint)from.UnAMult.Length <= i)
                    UA = 0;
                else
                    UA = from.UnAMult[i];
                if ((uint)from.NormedAMult.Length <= i)
                    A = 0;
                else
                    A = from.NormedAMult[i];
                RAMult[i] += RA;
                NormedAMult[i] += A;
                UnAMult[i] += UA;
            }
        }

        // subclasses will combine this with thier own specialized list generation 
        public override void GenParamList()
        {
            base.GenParamList();
            Table = "results_rec"; // or something else

            ps.Add(new DBParamEntry("singles_sum", Totals));
            ps.Add(new DBParamEntry("scaler1_sum", S1Sum));
            ps.Add(new DBParamEntry("scaler2_sum", S2Sum));
            ps.Add(new DBParamEntry("reals_plus_acc_sum", RASum));
            ps.Add(new DBParamEntry("acc_sum", ASum));
            ps.Add(new DBParamEntry("mult_reals_plus_acc_sum", RAMult));
            ps.Add(new DBParamEntry("mult_acc_sum", NormedAMult));
            ps.AddRange(TuplePair("singles", DeadtimeCorrectedRates.Singles));
            ps.AddRange(TuplePair("doubles", DeadtimeCorrectedRates.Doubles));
            ps.AddRange(TuplePair("triples", DeadtimeCorrectedRates.Triples));
            ps.AddRange(TuplePair("scaler1", Scaler1));
            ps.AddRange(TuplePair("scaler2", Scaler2));
            ps.AddRange(TuplePair("uncorrected_doubles", RawDoublesRate));
            ps.Add(new DBParamEntry("singles_multi", singles_multi)); 
            ps.Add(new DBParamEntry("doubles_multi", doubles_multi)); 
            ps.Add(new DBParamEntry("triples_multi", triples_multi)); 
            ps.Add(new DBParamEntry("declared_mass", Mass));
			{	// la super hack-a-whack
				DB.DB db = new DB.DB(true);
				if (db.TableHasColumn(Table,"mult_acc_un_sum"))
					ps.Add(new DBParamEntry("mult_acc_un_sum", UnAMult));
			}
        }

        public List<DBParamEntry> MoreForResults()
        {
            Table = "results_rec";

            List<DBParamEntry> s = new List<DBParamEntry>();
            s.Add(new DBParamEntry("covariance_matrix", covariance_matrix));
            return s;
        }

		public bool RawButNoDTCRates
		// For when DTC rates are not calculated, for whatever reason 
		{
			get
			{
				bool res = false;
				bool raw = (RawSinglesRate.v != 0 || RawDoublesRate.v != 0 || RawTriplesRate.v != 0);
				bool dtc = (DeadtimeCorrectedSinglesRate.v != 0 || DeadtimeCorrectedDoublesRate.v != 0 || DeadtimeCorrectedTriplesRate.v != 0);
				res = raw && !dtc;
				return res;
			}
		}

    }

    public class QCStatus
    {
        public QCStatus()
        {
            status = QCTestStatus.None;
            voltage = DefVoltage;
        }
        public QCStatus(QCTestStatus s)
        {
            status = s;
            voltage = DefVoltage;
        }
        public QCStatus(QCTestStatus s, double v)
        {
            status = s;
            voltage = v;
        }
        public QCStatus(QCStatus s)
        {
            status = s.status;
            voltage = s.voltage;
        }
        public QCTestStatus status;
        public double voltage;
        public const double DefVoltage = -999.8;
    }
    public class QCStatusMap : Dictionary<Multiplicity, QCStatus>
    {
        public QCStatusMap() : base (new MultiplicityEqualityComparer())
        {
        }

        // only the multiplicity results map entries are meaningful for cycle validation
        public System.Collections.Generic.IEnumerator<KeyValuePair<Multiplicity, QCStatus>> GetMultiplicityEnumerator()
        {
            foreach (KeyValuePair<Multiplicity, QCStatus> pair in this)
            {
                if (pair.Key is Multiplicity)
                    yield return pair;
            }
        }
        public System.Collections.Generic.IEnumerator<QCStatus> GetMultiplicityStatusEnumerator()
        {
            foreach (KeyValuePair<Multiplicity, QCStatus> pair in this)
            {
                if (pair.Key is Multiplicity)
                    yield return pair.Value;
            }
        }

        public bool Valid(Multiplicity mkey)
        {
            return (this[mkey].status == QCTestStatus.Pass);
        }

        // 
        /// <summary>
        /// True if all the multiplicity entries are valid. Use this condition to move forward for INCC5 calculations
        /// </summary>
        /// <returns>all the multiplicity entries on this cycle have a Pass status</returns>
        public bool StronglyValid()
        {
            bool good = false;
            foreach (KeyValuePair<Multiplicity, QCStatus> pair in this)
            {
                if (pair.Key is Multiplicity && (pair.Value.status == QCTestStatus.Pass))
                {
                    good = true;
                }
                else
                {
                    good = false;
                    break;
                }
            }
            return good;
        }

        /// <summary>
        /// True if all the multiplicity entries have an assigned status. Use this condition to move forward for INCC5 calculations.
        /// </summary>
        /// <returns>all the multiplicity entries on this cycle have some set status</returns>
		public bool WeaklyValid()
        {
            bool good = false;
            foreach (KeyValuePair<Multiplicity, QCStatus> pair in this)
            {
                if (pair.Key is Multiplicity && (pair.Value.status != QCTestStatus.None))
                {
                    good = true;
					break;
                }
                else
                {
                    good = false;
                    break;
                }
            }
            return good;
        }
    }

    public class CountingResultsMap : Dictionary<SpecificCountingAnalyzerParams, object>
    {
        public CountingResultsMap()
            : base(new SpecificCountingAnalyzerParamsEqualityComparer())
        {
        }

        public bool HasMultiplicity
        {
            get {
                System.Collections.IEnumerator ie = GetMultiplicityEnumerator();
                if (ie.MoveNext())
                    return true;
                else
                    return false;
            }
        }

        public MultiplicityCountingRes GetFirstMultiplicity
        {
            get
            {
                object pair;
                System.Collections.IEnumerator ie = GetMultiplicityEnumerator();
                if (ie.MoveNext())
                { 
                    pair = ie.Current;
                    return (MultiplicityCountingRes)((KeyValuePair < SpecificCountingAnalyzerParams, object> )pair).Value;
                }
                else
                    return null;
            }
        }

		public bool SingleMultiplicityAnalyzer
		{
			get {
				int mcount = 0;
				foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
					if (pair.Key is Multiplicity)
						mcount++;
				return (mcount == 1) && (Count == 1);
			}
		}

		public Multiplicity SingleMultiplicityAnalyzerKey
		{
			get {
				Multiplicity key = null;
				foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
					if (pair.Key is Multiplicity)
						key = (Multiplicity)pair.Key;
				return key;
			}
		}

        public SpecificCountingAnalyzerParams GetFirstMultiplicityOrFirstLMKey
        {
            get
            {
                if (HasMultiplicity)
                {
                    object pdair;
                    System.Collections.IEnumerator ide = GetMultiplicityEnumerator();
                    if (ide.MoveNext())
                    {
                        pdair = ide.Current;
                        return (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)pdair).Key;
                    }
                }
                else
                {
                    object pair;
                    System.Collections.IEnumerator ie = GetEnumerator();
                    if (ie.MoveNext())
                    {
                        pair = ie.Current;
                        return ((KeyValuePair < SpecificCountingAnalyzerParams, object> )pair).Key;
                    }
                }
                return null;
            }
        }

        // dev note: should be a way to rewrite this or redefine the containing class so the two kinds of iterators can be 2 generic templates, but I didn't do it.
        public System.Collections.IEnumerator GetATypedParameterEnumerator(Type t, bool includeSuspectEntries = false)
        {
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key.GetType().Equals(t) && (includeSuspectEntries || !pair.Key.suspect))
                    yield return pair.Key;
            }
        }

        public System.Collections.IEnumerator GetATypedResultEnumerator(Type t, bool includeSuspectEntries = false)
        {
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key.GetType().Equals(t) && (includeSuspectEntries || !pair.Key.suspect))
                    yield return pair.Value;
            }
        }

        public System.Collections.IEnumerator GetMultiplicityEnumerator(bool includeSuspectEntries = false)
        {
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key is Multiplicity && (includeSuspectEntries || !pair.Key.suspect))
                    yield return pair;
            }
        }
        public int GetResultsCount(Type t, bool includeSuspectEntries = false)
        {
            int count = 0;
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key.GetType().Equals(t) && (includeSuspectEntries || !pair.Key.suspect))
                    count++;
            }
            return count;
        }

        // the channel counts are the same for all rates analyzers so we only need the first one 
        public RatesResultEnhanced GetFirstRatesResultMod(bool includeSuspectEntries = false)
        {
            RatesResultEnhanced rrm = null;
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key is BaseRate && (includeSuspectEntries || !pair.Key.suspect))
                {
                    rrm = (RatesResultEnhanced)pair.Value;
                    break;
                }
            }
            return rrm;
        }

        public BaseRate GetFirstRatesResultModKey(bool includeSuspectEntries = false)
        {
            BaseRate br = null;
            foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in this)
            {
                if (pair.Key is BaseRate && (includeSuspectEntries || !pair.Key.suspect))
                {
                    br = (BaseRate)pair.Key;
                    break;
                }
            }
            return br;
        }
    }


    public class AnalysisMessages : Dictionary<Multiplicity, List<MeasurementMsg>>
    {
        public AnalysisMessages()
        {
        }

        public AnalysisMessages(AnalysisMessages am)
            : base(am)
        {
        }
    }

}
