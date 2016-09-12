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
// dev note: ReWire has an interesting modular architecture : devices, panels for UI interaction, mixers to match devices + channels (LM LOL) wth one another.
using System;
using System.Collections.Generic;
namespace AnalysisDefs 
{

    public class SpecificCountingAnalyzerParams : ParameterBase
    {
        public SpecificCountingAnalyzerParams()
            : base()
        {
            reason = String.Empty;
            Active = true;
        }

        public ulong gateWidthTics;
        public string reason;
        public bool suspect // true if something went wrong with operations on this key
        {
            get { return !String.IsNullOrEmpty(reason); }
        }

        /// <summary>
        /// Flags this analyzer as in use (active) or disabled (inactive)
        /// </summary>
        public bool Active
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true iff caller wants activeOnly and this analyzer is active 
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public bool ActiveConstraint(bool activeOnly)
        {
            return !activeOnly || Active;
        }

        /// <summary>
		/// A generalized marker field and for unique ID use in maps and database
        /// </summary>
        public long Rank
        {
            get;
            set;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "CountingParams";
            ps.Add(new DBParamEntry("gatewidth", gateWidthTics));
            ps.Add(new DBParamEntry("active", Active));
            ps.Add(new DBParamEntry("rank", Rank));
        }

		public const long Select = -99;  // values < 0 used for sorting and selection
    }

    public class SpecificCountingAnalyzerParamsEqualityComparer : IEqualityComparer<SpecificCountingAnalyzerParams>
    {

        public bool Equals(SpecificCountingAnalyzerParams scap1, SpecificCountingAnalyzerParams scap2)
        {
           
            if (scap1.gateWidthTics == scap2.gateWidthTics && scap1.reason.Equals(scap2.reason) && scap1.suspect == scap2.suspect)
            {
                System.Type t1 = scap1.GetType();
                System.Type t2 = scap2.GetType();
                if (t1.Equals(t2))
                {
                    if (t1.Equals(typeof(Multiplicity)))
                        return Multiplicity.Compare((Multiplicity)scap1, (Multiplicity)scap2) == 0;
                    else if (t1.Equals(typeof(Coincidence)))
                        return ((Coincidence)scap1).Equals((Coincidence)scap2);
                }

                return t1.Equals(t2);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(SpecificCountingAnalyzerParams scap)
        {
            int hCode = scap.gateWidthTics.GetHashCode() ^ scap.reason.GetHashCode() ^ scap.suspect.GetHashCode();
            return hCode.GetHashCode();
        }
    }

    public class BaseRate : SpecificCountingAnalyzerParams
    {
        public BaseRate() : base()
        {
            gateWidthTics = (ulong)1e7; // 1 second, expressed in 'tics', i.e. 'ticks'
        }
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "CountingParams";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name));
        }
    }

    public enum FAType { FAOn, FAOff }; // this is used as differentiator for both types of mult result, cut&paste city

    public class Multiplicity : SpecificCountingAnalyzerParams, IEquatable<Multiplicity>, IComparable<Multiplicity>
    {

        public FAType FA;

        // larger values produce more accurate results, but slow down the processing
        public ulong AccidentalsGateDelayInTics
        {
            get { return accidentalsGateDelayInTics; }
            set { accidentalsGateDelayInTics = value; }
        }

        public ulong BackgroundGateTimeStepInTics 
        {
            get { return backgroundGateTimeStepInTics; }
            set { backgroundGateTimeStepInTics = value; }
        }

        /// <summary>
        /// The original exposed member for prior clients, e.g. Core. Use SR now
        /// </summary>
        public ShiftRegisterParameters sr; 

        /// <summary>
        /// The SR params for this multiplicity definition. The set op overrides the gatelength assignment 
        /// </summary>
        public ShiftRegisterParameters SR
        {
            get { return sr; }
            set { sr = value; gateWidthTics = sr.gateLength; }
        }



        public void SetGateWidthTics(ulong value)
        {
            sr.gateLength = value; gateWidthTics = value;
        }
        private ulong backgroundGateTimeStepInTics;
        private ulong accidentalsGateDelayInTics;  // often a property of the HW

        public Multiplicity()
            : base()
        {
            Init();
        }

        public Multiplicity(FAType c) : base()
        {
            FA = c;
            Init();
        }
        public Multiplicity(Multiplicity src)
            : base()
        {
            Init(src);
        }

        private void Init(Multiplicity src = null)
        {
            if (src != null)
            {
                FA = src.FA;
                backgroundGateTimeStepInTics = src.backgroundGateTimeStepInTics;
                accidentalsGateDelayInTics = src.accidentalsGateDelayInTics;
                SR = new ShiftRegisterParameters(src.SR);
                reason = string.Copy(src.reason);
            }
            else
            {
                //10240 NOT a reasonable default value for FA analysis per Martyn. HN 6.24.2015
                if (FA == FAType.FAOn)
                {
                    backgroundGateTimeStepInTics = 2;
                    accidentalsGateDelayInTics = 2;
                }
                else
                {
                    backgroundGateTimeStepInTics = 10240;
                    accidentalsGateDelayInTics = 10240; // 10240 matches some traditional HW, larger values produce more accurate results, but slow down the processing
                }
                SR = new ShiftRegisterParameters();
            }
        }

        public void CopyValues(Multiplicity mul)
        {
            if (mul == null)
                return;
            FA = mul.FA;
            backgroundGateTimeStepInTics = mul.backgroundGateTimeStepInTics;
            accidentalsGateDelayInTics = mul.accidentalsGateDelayInTics;
            SR = new ShiftRegisterParameters(mul.SR);
            reason = string.Copy(mul.reason);
        }
        public bool Equals(Multiplicity other)
        {
            if (FA == other.FA & this.gateWidthTics == other.gateWidthTics & 
                suspect == other.suspect &&
                accidentalsGateDelayInTics == other.accidentalsGateDelayInTics && 
                backgroundGateTimeStepInTics == other.backgroundGateTimeStepInTics &&
                SR.Equals(other.SR))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		public bool EqualsButForLMValues(Multiplicity other)
		{
			if (other != null &&
				FA == other.FA && gateWidthTics == other.gateWidthTics && 
                AccidentalsGateDelayInTics == other.AccidentalsGateDelayInTics && 
                BackgroundGateTimeStepInTics == other.BackgroundGateTimeStepInTics &&
                EqualsButForLMValues(other.SR))
            {
                return true;
            }
            else
            {
                return false;
            }
		}
		public bool EqualsButForLMValues(ShiftRegisterParameters other)
		{
            if (other != null && 
				SR.dieAwayTime.Equals(other.dieAwayTime) &&
                SR.efficiency.Equals(other.efficiency) &&
                SR.doublesGateFraction.Equals(other.doublesGateFraction) &&
                SR.triplesGateFraction.Equals(other.triplesGateFraction) &&
                SR.deadTimeCoefficientTinNanoSecs.Equals(other.deadTimeCoefficientTinNanoSecs) &&
                SR.deadTimeCoefficientAinMicroSecs.Equals(other.deadTimeCoefficientAinMicroSecs) &&
                SR.deadTimeCoefficientBinPicoSecs.Equals(other.deadTimeCoefficientBinPicoSecs) &&
                SR.deadTimeCoefficientCinNanoSecs.Equals(other.deadTimeCoefficientCinNanoSecs) &&
                SR.highVoltage.Equals(other.highVoltage))
            {
                return true;
            }
            else
            {
                return false;
            }
		}

        public override int GetHashCode()
        {
            int hCode = FA.GetHashCode() ^ gateWidthTics.GetHashCode() ^ //suspect.GetHashCode() ^
                accidentalsGateDelayInTics.GetHashCode() ^ backgroundGateTimeStepInTics.GetHashCode();
            hCode ^= sr.GetHashCode();
            return hCode.GetHashCode();
        }
        public static string TimeUnitImage(ulong t, bool withunitsspace = false)
        {
            string s;
            if (t <= 10)  // tics
                s = t.ToString();
            else
                s = (t / 10).ToString();

            string unit;
            if (t <= 10)  // tics
                unit = "tics";
            else  // ms
                unit = "ms";
            if (withunitsspace)
                s += " ";
            s += unit;
            return s;
        }
        public string ShortName()
        {
            string s = "";
            if (FA == FAType.FAOn)
            {
                s += "FA " + TimeUnitImage(backgroundGateTimeStepInTics);
            }
            else
            {
                s += "A " + TimeUnitImage(accidentalsGateDelayInTics);
            }
            return s;
        }
        public override string ToString()
        {
            string s = "SR";
            if (FA == FAType.FAOn)
            {
                s += " fast acc. with internal gate ";
                s += TimeUnitImage(backgroundGateTimeStepInTics, withunitsspace: true);
            }
            else
            {
                s += " with acc. delay internal gate ";
                s += TimeUnitImage(accidentalsGateDelayInTics, withunitsspace: true);
            }
            return s;
        }

        public static int Compare(Multiplicity x, Multiplicity y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = 0;
            res = x.FA.CompareTo(y.FA);
            if (res == 0)
                res = x.gateWidthTics.CompareTo(y.gateWidthTics);
            if (res == 0)
                res = x.accidentalsGateDelayInTics.CompareTo(y.accidentalsGateDelayInTics);
            if (res == 0)
                res = x.backgroundGateTimeStepInTics.CompareTo(y.backgroundGateTimeStepInTics);
            if (res == 0)
                res = x.suspect.CompareTo(y.suspect);
            if (res == 0)
                res = x.SR.CompareTo(y.SR);

            return res;
        }

        public int CompareTo(Multiplicity other)
        {
            return Compare(this, other);
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "LMMultiplicity";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name)); 
            ps.Add(new DBParamEntry("backgroundgatewidth", BackgroundGateTimeStepInTics));
            ps.Add(new DBParamEntry("accidentalsgatewidth", AccidentalsGateDelayInTics));
            ps.Add(new DBParamEntry("FA", FA == FAType.FAOn));
        }
    }

    public class MultiplicityEqualityComparer : IEqualityComparer<Multiplicity>
    {
        SpecificCountingAnalyzerParamsEqualityComparer scap;
        public MultiplicityEqualityComparer()
        {
            scap = new SpecificCountingAnalyzerParamsEqualityComparer();
        }

        public bool Equals(Multiplicity m1, Multiplicity m2)
        {
            int res = Multiplicity.Compare(m1, m2);
            if (res != 0)
            {
                return scap.Equals((SpecificCountingAnalyzerParams)m1, (SpecificCountingAnalyzerParams)m2);
            }
            else
                return true;
        }
                    
        public int GetHashCode(Multiplicity m)
        {
            return m.GetHashCode();
        }
    }


    public class Coincidence : SpecificCountingAnalyzerParams, IEquatable<Coincidence>
    {
        public Coincidence()
            : base()
        {
            backgroundGateTimeStepInTics = 40960;
            accidentalsGateDelayInTics = 40960; // what are good defaults here? same as SR?
            gateWidthTics = 240;
            _sr = new ShiftRegisterParameters();
        }
        
        public UInt64 AccidentalsGateDelayInTics
        {
            get { return accidentalsGateDelayInTics; }
            set { accidentalsGateDelayInTics = value; }
        }

        public UInt64 BackgroundGateTimeStepInTics
        {
            get { return backgroundGateTimeStepInTics; }
            set { backgroundGateTimeStepInTics = value; }
        }

        public ShiftRegisterParameters _sr;

        public ShiftRegisterParameters SR
        {
            get { return _sr; }
            set { _sr = value; gateWidthTics = _sr.gateLength; }
        }
        public void SetGateWidthTics(ulong value)
        {
            _sr.gateLength = value; gateWidthTics = value;
        }

        private UInt64 backgroundGateTimeStepInTics;
        private UInt64 accidentalsGateDelayInTics;

        public bool Equals(Coincidence other)
        {
            if (other != null && this.gateWidthTics == other.gateWidthTics & this.suspect == other.suspect
                & this.SR.Equals(other.SR))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class CoincidenceEquality : EqualityComparer<Coincidence>
        {
            public override int GetHashCode(Coincidence m)
            {
                int hCode = m._sr.GetHashCode() ^ m.gateWidthTics.GetHashCode() ^ m.suspect.GetHashCode();
                return hCode.GetHashCode();
            }
            public override bool Equals(Coincidence b1, Coincidence b2)
            {
                return EqualityComparer<Coincidence>.Default.Equals(b1, b2);
            }
        }

        public override string ToString()
        {
            return String.Format("predelay {0}, gatewidth {1}, acc gate delay {2} coincidence matrix params", SR.predelay, SR.gateLength, AccidentalsGateDelayInTics); // expand later
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "LMMultiplicity";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name));
            ps.Add(new DBParamEntry("backgroundgatewidth", BackgroundGateTimeStepInTics));
            ps.Add(new DBParamEntry("accidentalsgatewidth", AccidentalsGateDelayInTics));
            ps.Add(new DBParamEntry("FA", false));
        }

    }

    public class Feynman : SpecificCountingAnalyzerParams
    {
        public Feynman()
            : base()
        {
            gateWidthTics = 250;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "CountingParams";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name));
        }
    }

    public class Rossi : SpecificCountingAnalyzerParams
    {
        public Rossi()
            : base()
        {
            gateWidthTics = 500;
        }
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "CountingParams";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name));
        }
    }

    public class TimeInterval : SpecificCountingAnalyzerParams
    {
        public TimeInterval()
            : base()
        {
            gateWidthTics = 250;
        }
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "CountingParams";
            ps.Add(new DBParamEntry("counter_type", this.GetType().Name));
        }
    }

    public class CountingAnalysisParameters : List<SpecificCountingAnalyzerParams>
    {
        public CountingAnalysisParameters(CountingAnalysisParameters src) : base(src) { }
        public CountingAnalysisParameters() : base() { }

        public static CountingAnalysisParameters Copy(CountingAnalysisParameters src)
        {
            if (src == null)
                return new CountingAnalysisParameters();
            else
                return new CountingAnalysisParameters(src);
        }

        public bool HasMultiplicity(FAType ft, bool activeOnly = true)
        {
            SpecificCountingAnalyzerParams sap = Find(p => { return p.ActiveConstraint(activeOnly) && 
                (p is Multiplicity) && ((p as Multiplicity).FA == ft); });
            return sap != null;
        }

        public bool HasMultiplicity(bool activeOnly = true)
        {
            SpecificCountingAnalyzerParams sap = Find(p =>
            {
                return p.ActiveConstraint(activeOnly) && (p is Multiplicity);
            });
            return sap != null;
        }

        public List<SpecificCountingAnalyzerParams> GetTheseParams(Type t, bool activeOnly = true)
        {
            return FindAll(p => { return p.ActiveConstraint(activeOnly) && (p.GetType() == t); });
        }

        // several get list<type>'s, because there may be several of each
        public List<SpecificCountingAnalyzerParams> GetMults(FAType ft, bool activeOnly = true)
        {
            return FindAll(p => { return (p.ActiveConstraint(activeOnly) && p is Multiplicity && ((p as Multiplicity).FA == ft)); });
        }
        public List<SpecificCountingAnalyzerParams> GetAllMults(bool activeOnly = true)
        {
            return FindAll(p => { return (p.ActiveConstraint(activeOnly) && p is Multiplicity); });
        }
        public List<SpecificCountingAnalyzerParams> GetRossis(bool activeOnly = true)
        {
            return GetTheseParams(typeof(Rossi), activeOnly);
        }
        public List<SpecificCountingAnalyzerParams> GetTimeIntervals(bool activeOnly = true)
        {
            return GetTheseParams(typeof(TimeInterval), activeOnly);
        }
        public List<SpecificCountingAnalyzerParams> GetBases(bool activeOnly = true)
        {
            return GetTheseParams(typeof(BaseRate), activeOnly);
        }
        public List<SpecificCountingAnalyzerParams> GetFeynmans(bool activeOnly = true)
        {
            return GetTheseParams(typeof(Feynman), activeOnly);
        }
        public List<SpecificCountingAnalyzerParams> GetCoincidences(bool activeOnly = true)
        {
            return GetTheseParams(typeof(Coincidence), activeOnly);
        }

        public TimeInterval GetFirstTimeInterval(bool activeOnly = true, bool addIfNotPresent = false)
        {
            List<SpecificCountingAnalyzerParams> l = GetTimeIntervals(activeOnly);
            if (l != null && l.Count > 0)
                return (TimeInterval)l[0];
            else
            {
              TimeInterval t = new TimeInterval();
              if (addIfNotPresent)
                  Add(t);
              return t;
            }
        }
        public Rossi GetFirstRossi(bool activeOnly = true, bool addIfNotPresent = false)
        {
            List<SpecificCountingAnalyzerParams> l = GetRossis(activeOnly);
            if (l != null && l.Count > 0)
                return (Rossi)l[0];
            else
            {
                Rossi t = new Rossi();
                if (addIfNotPresent)
                    Add(t);
                return t;
            }
        }
        public Coincidence GetFirstCoincidence(bool activeOnly = true, bool addIfNotPresent = false)
        {
            List<SpecificCountingAnalyzerParams> l = GetCoincidences(activeOnly);
            if (l != null && l.Count > 0)
                return (Coincidence)l[0];
            else
            {
                Coincidence t = new Coincidence();
                if (addIfNotPresent)
                    Add(t);
                return t;
            }
        }
        public Feynman GetFirstFeynman(bool activeOnly = true, bool addIfNotPresent = false)
        {
            List<SpecificCountingAnalyzerParams> l = GetFeynmans(activeOnly);
             if (l != null && l.Count > 0)
                 return (Feynman)l[0];
             else
             {
                 Feynman t = new Feynman();
                 if (addIfNotPresent)
                     Add(t);
                 return t;
             }
        }
        public Multiplicity GetFirstMult(FAType ft, out bool addedIfNotPresent)
        {
            addedIfNotPresent = false;
            List<SpecificCountingAnalyzerParams> l = GetMults(ft, activeOnly: false);
            if (l != null && l.Count > 0)
                return (Multiplicity)l[0];
            else
            {
                Multiplicity t = new Multiplicity(ft);
                addedIfNotPresent = true;
                Add(t);
                return t;
            }
        }
        public Multiplicity GetFirstMultiplicityAnalyzer()
        {
            List<SpecificCountingAnalyzerParams> l = 
                FindAll(p => { return (p.ActiveConstraint(true) && p is Multiplicity); });

            if (l != null && l.Count > 0)
                return (Multiplicity)l[0];
            else
            {
                Multiplicity t = new Multiplicity();
                return t;
            }
        }

        public BaseRate GetFirstRate(bool activeOnly = true, bool addIfNotPresent = false)
        {
            List<SpecificCountingAnalyzerParams> l = GetBases(activeOnly);
            if (l != null && l.Count > 0)
                return (BaseRate)l[0];
            else
            {
                BaseRate t = new BaseRate();
                if (addIfNotPresent)
                    Add(t);
                return t;
            }
        }


		public bool HasMatchingVSR(Multiplicity mul)
		{
			int matchidx = GetMatchingVSRIndex(mul);
			return matchidx >= 0;
		}

		public int GetMatchingVSRIndex(Multiplicity mul)
		{
			int matchidx = FindIndex(x => 
						((x is Multiplicity) && 
						(x as Multiplicity).FA == mul.FA) && // same FA
								                 x.Active && // active
						(x as Multiplicity).EqualsButForLMValues(mul)); // everything but gatewidth and predelay are the same
			return matchidx;
		}

		public bool PrepareMatchingVSR(Multiplicity mul)
		{
			Multiplicity match = null;
            bool anew = false;
			// Find a type match for existing SR settings
			// 1: fast == fast, conv == conv. 
			int matchidx = FindIndex(x => ((x is Multiplicity) && (x as Multiplicity).FA == mul.FA));
			// 2: if no fast/conv found, create by copying, add to list
			if (matchidx < 0)
			{
				match = new Multiplicity(mul.FA);
				match.CopyValues(mul);
				Add(match);
                anew = true;
            }
			else 
				match = (Multiplicity)this[matchidx];
			// 2: If not active, activate it.
			// Q: deactivate all others?
			match.Active = true;
			// 3: mark for display in GUI
			match.Rank = SpecificCountingAnalyzerParams.Select; // values < 0 flag uses for UI sorting and selection
            return anew;
		}
	}


    /// <summary>
    /// Kludge to exit flow of control in unusual processing locations
    /// </summary>
    public class CancellationRequestedException : Exception
    {
        public CancellationRequestedException()
        {
        }

        public CancellationRequestedException(string message)
            : base(message)
        {
        }
    }
    public class NeutronCountingException : Exception
    {
        public NeutronCountingException()
        {
        }

        public NeutronCountingException(string message)
            : base(message)
        {
        }
    }
    public class StopNeutronCountingException : NeutronCountingException
    {
        public StopNeutronCountingException()
        {
        }

        public StopNeutronCountingException(string message)
            : base(message)
        {
        }
    }
    public class FatalNeutronCountingException : NeutronCountingException
    {
        public FatalNeutronCountingException()
        {
        }

        public FatalNeutronCountingException(string message)
            : base(message)
        {
        }
    }

    public enum NormTest { AmLiSingles, Cf252Doubles, Collar, Cf252Singles }


}


// get the number of bits here, (see fun threads on this on the web), and add them to numTotalsEncountered
//{
//    uint v = swapped - ((swapped >> 1) & 0x55555555);                    // reuse input as temporary
//    v = (v & 0x33333333) + ((v >> 2) & 0x33333333);     // temp
//    uint c = ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
//    state.numTotalsEncountered += c;
//}
//{
//    uint v = swapped;
//    uint c = 0;
//    while (v != 0)
//    {
//        c++;
//        v &= (v - 1);
//    }
//    state.numTotalsEncountered += c;
//}
