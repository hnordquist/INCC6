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
namespace AnalysisDefs
{
    using NC = NCC.CentralizedState;
    using Tuple = AnalysisDefs.VTuple;

    public static class KValsForAlpha
    {
        static KValsForAlpha()
        {
            K = new double[10] { 0.0, 134.0, .381, 1.41, .013, .02, 26.9, 10.2, 2.54, 1.69 };
        }
        public static readonly double[] K;
    }

    public enum KValsSource { AGTNMC, // Advanced Guide to Neutron Multiplicity Counting 
                            ESARDA_64, ESARDA_128, N_HENCC }
    public class KValsForPu240e
    {

        public double this[ushort i]
        {
            get
            {
                if (i < 10) return KValsForPu240e.losValuesDelK[source][i];
                else if (i == 10)
                    return K10;
                else if (i == 11)
                    return K11;
                else return 0.0;
            }
        }


        // nomenclature mapping
        // the two that vary per the source selector 
        double K10
        {
            get { return KValsForPu240e.losValuesDelK[source][1]; }
        }
        double K11
        {
            get { return KValsForPu240e.losValuesDelK[source][2]; }
        }

        public KValsForPu240e()
        {
            if (losValuesDelK == null)
            {
                losValuesDelK = new Dictionary<KValsSource, double[]>();

                // numbers from APPLICATION GUIDE TO NEUTRON MULTIPLICITY COUNTING, and the PANDA Manual Ch. 16, Ensslin
                // from calcmass.cpp, calc240e.cpp, knowm_m.cpp, CALC239E.CPP
                losValuesDelK.Add(KValsSource.AGTNMC, new double[10] { 0.0, 2.52, 1.68, 0.786, 0.515, 1.414, 0.422, 0.545, 0, 0 });

                // K1, K2, (K10, K11) from Ottmar et al. ESARDA conference Seville, Spain 1999
                losValuesDelK.Add(KValsSource.ESARDA_64, new double[10] { 0.0, 2.707, 1.658, 0.786, 0.515, 1.414, 0.422, 0.545, 0, 0 });
                losValuesDelK.Add(KValsSource.ESARDA_128, new double[10] { 0.0, 2.714, 1.667, 0.786, 0.515, 1.414, 0.422, 0.545, 0, 0 });
                losValuesDelK.Add(KValsSource.N_HENCC, new double[10] { 0.0, 2.718, 1.664, 0.786, 0.515, 1.414, 0.422, 0.545, 0, 0 });
            }
            source = KValsSource.AGTNMC;
        }

        static Dictionary<KValsSource, double[]> losValuesDelK;
        public KValsSource source;
    }


    public enum Isotope { none, pu238, pu239, pu240, pu241, pu242, am241, cf252, cm, us35, uu1 }

    public class Isotopics : ParameterBase, IComparable<Isotopics>
    {
        public enum SourceCode { OD, IA, IM, OA, OC, OE, OS, CO }

        static readonly public double LN2 = .69314718056;

        static public readonly double DAYS_PER_YEAR = 365.24219;	 		/* avg # of days per year */
        static public readonly double PU238HL = (87.74 * DAYS_PER_YEAR);	/* half life of Pu238 in days */
        static public readonly double PU239HL = (24119.0 * DAYS_PER_YEAR);	/* half life of Pu239 in days */
        static public readonly double PU240HL = (6564.0 * DAYS_PER_YEAR);	/* half life of Pu240 in days */
        static public readonly double PU241HL = (14.348 * DAYS_PER_YEAR);	/* half life of Pu241 in days */
        static public readonly double PU242HL = (376300.0 * DAYS_PER_YEAR); /* half life of Pu242 in days */
        static public readonly double AM241HL = (433.6 * DAYS_PER_YEAR);	/* half life of Am241 in days */
        static public readonly double CF252HL = (2.645 * DAYS_PER_YEAR);	/* half life of Cf252 in days */
        static public readonly double CMHL = (18.1 * DAYS_PER_YEAR);		/* half life of Cm in days */
        static public readonly double U235HL = (1.0 * DAYS_PER_YEAR);	    /* half life of U in days */

        public static readonly DateTime ZeroIAEATime;
        public static readonly double[] Halflives;

        public static readonly string DefaultId = "Default";

        static Isotopics()
        {
            ZeroIAEATime = new DateTime(1952, 1, 1);
            Halflives = new double[] { 0, PU238HL, PU239HL, PU240HL, PU241HL, PU242HL, AM241HL, CF252HL, CMHL, U235HL, 1.0 };
        }

        internal Tuple[] isotopes;
        public DateTime pu_date;
        public DateTime am_date;
        public string id;
        public SourceCode source_code;

        // INCC5-style getters
        public double pu238 { get { return this[Isotope.pu238].v; } }
        public double pu239 { get { return this[Isotope.pu239].v; } }
        public double pu240 { get { return this[Isotope.pu240].v; } }
        public double pu241 { get { return this[Isotope.pu241].v; } }
        public double pu242 { get { return this[Isotope.pu242].v; } }
        public double am241 { get { return this[Isotope.am241].v; } }
        public double pu238_err { get { return this[Isotope.pu238].err; } }
        public double pu239_err { get { return this[Isotope.pu239].err; } }
        public double pu240_err { get { return this[Isotope.pu240].err; } }
        public double pu241_err { get { return this[Isotope.pu241].err; } }
        public double pu242_err { get { return this[Isotope.pu242].err; } }
        public double am241_err { get { return this[Isotope.am241].err; } }

        public void SetPercentages(double Pu238, double Pu239, double Pu240, double Pu241, double Pu242, double Am241)
        {
            isotopes[1].v = Pu238;
            isotopes[2].v = Pu239;
            isotopes[3].v = Pu240;
            isotopes[4].v = Pu241;
            isotopes[5].v = Pu242;
            isotopes[6].v = Am241;
        }

        public void SetVal(Isotope iso, double val)
        {
            isotopes[(int)iso].v = val;
        }
        public void SetError(Isotope iso, double val)
        {
            isotopes[(int)iso].sigma = val;
        }
        public void SetValueError(Isotope iso, double val, double err)
        {
            isotopes[(int)iso].v = val;
            isotopes[(int)iso].sigma = err;
        }
        //public double this[IsotopesOfInterest i]
        //{
        //    get { return isotopes[(int)i].v; }
        //}
        public Tuple this[Isotope i]
        {
            get { return isotopes[(int)i]; }
        }

        static public Tuple[] MakeArray()
        {
            int count = System.Enum.GetValues(typeof(Isotope)).Length;
            Tuple[] ret = new Tuple[count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = new Tuple();
            return ret;
        }

        static public Tuple[] CopyArray(Tuple[] src)
        {
            int count = System.Enum.GetValues(typeof(Isotope)).Length;
            Tuple[] ret = new Tuple[count];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = new Tuple(src[i]);
            return ret;
        }

        public Isotopics()
        {
            InitVals();
            id = "Default";
            source_code = SourceCode.OD;
        }

        public Isotopics(Isotopics iso)
        {
            if (iso == null)
            {
                InitVals();
                id = DefaultId;
                source_code = SourceCode.OD;
            }
            else
            {
                pu_date = new DateTime(iso.pu_date.Ticks);
                am_date = new DateTime(iso.am_date.Ticks);
                isotopes = CopyArray(iso.isotopes);
                id = string.Copy(iso.id);
                source_code = iso.source_code;
            }
        }

        public void Copy(Isotopics src)
        {
            pu_date = new DateTime(src.pu_date.Ticks);
            am_date = new DateTime(src.am_date.Ticks);
            isotopes = CopyArray(src.isotopes);
            id = string.Copy(src.id);
            source_code = src.source_code;
        }

        public void CopyTo(Isotopics dest)
        {
            dest.pu_date = new DateTime(pu_date.Ticks);
            dest.am_date = new DateTime(am_date.Ticks);
            dest.isotopes = CopyArray(isotopes);
            dest.id = string.Copy(id);
            dest.source_code = source_code;
            dest.modified = true;
        }

        public void InitVals()
        {
            pu_date = new DateTime(2010, 1, 1);
            am_date = new DateTime(2010, 1, 1);
            isotopes = MakeArray();
            isotopes[(int)Isotope.pu240].v = 100.0;
        }

        public static int Compare(Isotopics x, Isotopics y)
        {
            int res = string.Compare(x.id, y.id, StringComparison.OrdinalIgnoreCase);

            if (res == 0)
                res = (DateTime.Compare(x.pu_date, y.pu_date));

            if (res == 0)
                res = (DateTime.Compare(x.am_date, y.am_date));

            if (res == 0)
            {
                // how to meaningfully diff the % here? Try this
                int re2 = 0;
                for (int i = 0; (re2 == 0) && i < x.isotopes.Length; i++)
                {
                    re2 = x.isotopes[i].v.CompareTo(y.isotopes[i].v);
                }
                for (int i = 0; (re2 == 0) && i < x.isotopes.Length; i++)
                {
                    re2 = x.isotopes[i].err.CompareTo(y.isotopes[i].err);
                }
                res = re2;
            }

            return res;
        }

        public int CompareTo(Isotopics other)
        {
            return Compare(this, other);
        }

        /// <summary>
        ///  update isotopic composition data in db to calling date's values
        /// </summary>
        /// <param name="PuMass">mass of item</param>
        /// <param name="ref_date">date to update to </param>
        /// <param name="curiso">current iso</param>
        /// <param name="newiso">updated iso</param>
        /// <param name="logger">should be thru the global state</param>
        /// <param name="INCCParity">if true, ignore current day fraction in calculations</param>
        /// <returns>updated isotopics</returns>
        public static Isotopics update_isotopics(double PuMass, /* mass of item */
                                DateTimeOffset ref_date, /* date to update to */
                                Isotopics curiso, NCCReporter.LMLoggers.LognLM logger, bool INCCParity)
        {

            double ref_days;
            double pu_days;
            double am_days;
            Tuple[] iso_mass = MakeArray();
            Tuple[] decay_fract_pu_to_am = MakeArray();
            Tuple[] decay_fract_am_to_now = MakeArray();

            Tuple[] decay_fract_pu_to_now = MakeArray();
            Tuple[] cur_mass = MakeArray();

            double temp = 0.0;
            double temp_sum = 0.0;
            double cur_mass_sum = 0.0;
            double x, y;
            double pu_mass = PuMass;

            if (pu_mass <= 0.0)
            {
                pu_mass = 1.0;
            }

            double isosum = 0.0;
            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                isosum += curiso[iso].v;
            }

            if (isosum <= 0.0)
            {
                logger?.TraceEvent(NCCReporter.LogLevels.Warning, 36783, "Unable to update isotopics, sum of Pu isotopes must be greater than zero");
                return null;
            }
            Isotopics newiso = new Isotopics();
            //dev note: roundabout way to do this from INCC code


            // The INCC6 parity flag is used to toggle this behavior:
            // INCC literally truncates the fractional date here, so it is always at time of the full day
            // 		gen_date_time_str_to_seconds (&seconds, results.meas_date, "00:00:00", GEN_DTF_IAEA);
            //      ref_days = (double) seconds / SECONDS_PER_DAY;
            //

            if (INCCParity)
            {
                pu_days = (curiso.pu_date.Date.Subtract(ZeroIAEATime)).TotalDays;
                am_days = (curiso.am_date.Date.Subtract(ZeroIAEATime)).TotalDays;
                ref_days = (long)(ref_date.Date.Subtract(ZeroIAEATime)).TotalDays;
            }
            else
            {
                pu_days = (curiso.pu_date.Subtract(ZeroIAEATime)).TotalDays;
                am_days = (curiso.am_date.Subtract(ZeroIAEATime)).TotalDays;
                ref_days = (long)(ref_date.Subtract(ZeroIAEATime)).TotalDays;
            }

            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                iso_mass[(int)iso].v = (curiso[iso].v * pu_mass / 100.0);
                decay_fract_pu_to_am[(int)iso].v = Math.Exp((-LN2 / Halflives[(int)iso]) * (am_days - pu_days));
                decay_fract_pu_to_now[(int)iso].v = Math.Exp((-LN2 / Halflives[(int)iso]) * (ref_days - pu_days));
            }
            decay_fract_am_to_now[(int)Isotope.pu241].v = Math.Exp((-LN2 / Halflives[(int)Isotope.pu241]) * (ref_days - am_days));
            decay_fract_am_to_now[(int)Isotope.am241].v = Math.Exp((-LN2 / Halflives[(int)Isotope.am241]) * (ref_days - am_days));

            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                cur_mass[(int)iso].v = iso_mass[(int)iso].v * decay_fract_pu_to_now[(int)iso].v;
            }
            cur_mass[(int)Isotope.am241].v = decay_fract_am_to_now[(int)Isotope.am241].v * curiso[Isotope.am241].v / 100.0;

            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                temp_sum += decay_fract_pu_to_am[(int)iso].v * iso_mass[(int)iso].v;
            }

            temp = decay_fract_pu_to_am[(int)Isotope.pu241].v * iso_mass[(int)Isotope.pu241].v *
                (decay_fract_am_to_now[(int)Isotope.am241].v - decay_fract_am_to_now[(int)Isotope.pu241].v)
                * (LN2 / Halflives[(int)Isotope.pu241]) / ((LN2 / Halflives[(int)Isotope.pu241]) - (LN2 / Halflives[(int)Isotope.am241]));
            cur_mass[(int)Isotope.am241].v = (cur_mass[(int)Isotope.am241].v * temp_sum) + temp;

            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                cur_mass_sum += cur_mass[(int)iso].v;
            }

            if (cur_mass_sum <= 0.0)
            {
                logger?.TraceEvent(NCCReporter.LogLevels.Warning, 36784, "Unable to update isotopics, mass sum must be greater than zero");
                return (null);
            }
            else
            {
                logger?.TraceEvent(NCCReporter.LogLevels.Verbose, 36722, "'update_isotopics' mass sum " + cur_mass_sum);
            }

            newiso.pu_date = new DateTime(ref_date.Ticks);
            newiso.am_date = new DateTime(ref_date.Ticks);
            for (Isotope iso = Isotope.pu238; iso <= Isotope.am241; iso++)
            {
                newiso[iso].v = 100.0 * cur_mass[(int)iso].v / cur_mass_sum;
            }
            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                if (curiso[iso].v != 0)
                    newiso[iso].sigma = curiso[iso].sigma * newiso[iso].v / curiso[iso].v;
                else
                    newiso[iso].sigma = curiso[iso].sigma;

            }
            if (curiso[Isotope.am241].v != 0.0)
            {
                x = (decay_fract_am_to_now[(int)Isotope.am241].v / 100.0) * temp_sum *
                   curiso[Isotope.am241].sigma;
                temp = decay_fract_pu_to_am[(int)Isotope.pu241].v *
                    (decay_fract_am_to_now[(int)Isotope.am241].v - decay_fract_am_to_now[(int)Isotope.pu241].v)
                    * (LN2 / PU241HL) / ((LN2 / PU241HL) - (LN2 / AM241HL));
                y = temp * (pu_mass / 100.0) * curiso[Isotope.pu241].sigma;
                newiso[Isotope.am241].sigma = Math.Sqrt(x * x + y * y) / cur_mass_sum * 100.0;
            }
            else
                newiso[Isotope.am241].sigma = curiso[Isotope.am241].sigma;

            return (newiso);
        }

        // from calc240e.cpp
        public void calc_pu240e(double pu_mass, out double pu240e_ptr, Measurement meas)
        {

            double iso_mass = 1.0;
            pu240e_ptr = 0.0;
            Isotopics iso = update_isotopics(iso_mass, meas.MeasDate, this, meas.Logger, NC.App.AppContext.INCCParity);
            if (iso == null)
                return;

            KValsForPu240e K = new KValsForPu240e(); K.source = meas.AcquireState.KValSource;
            pu240e_ptr = pu_mass * ((K[10] * iso[Isotope.pu238].v / 100.0) +
                (iso[Isotope.pu240].v / 100.0) + (K[11] * iso[Isotope.pu242].v / 100.0));

        }

        // from calc_res.cpp
        public void UpdateDeclaredPuMass(DateTimeOffset meas_date, ref double decl_pu_mass, bool INCCParity)
        {
            double ref_days;
            double pu_days;
            double pu238_iso_mass;
            double pu239_iso_mass;
            double pu240_iso_mass;
            double pu241_iso_mass;
            double pu242_iso_mass;
            double pu238_decay_fract_pu_to_now;
            double pu239_decay_fract_pu_to_now;
            double pu240_decay_fract_pu_to_now;
            double pu241_decay_fract_pu_to_now;
            double pu242_decay_fract_pu_to_now;
            double cur_mass_pu238;
            double cur_mass_pu239;
            double cur_mass_pu240;
            double cur_mass_pu241;
            double cur_mass_pu242;

            if (decl_pu_mass > 0)
            {
                /* update declared Pu mass based upon original isotopic composition */

                // INCC literally truncates the fractional date here, so it is always at time of the full day
                // 		gen_date_time_str_to_seconds (&seconds, results.meas_date, "00:00:00", GEN_DTF_IAEA);
                //      ref_days = (double) seconds / SECONDS_PER_DAY;
                //
                if (INCCParity)
                {
                    pu_days = (pu_date.Date.Subtract(ZeroIAEATime)).TotalDays;
                    ref_days = (meas_date.Date.Subtract(ZeroIAEATime)).TotalDays;
                }
                else
                {
                    pu_days = (pu_date.Subtract(ZeroIAEATime)).TotalDays;
                    ref_days = (meas_date.Subtract(ZeroIAEATime)).TotalDays;
                }
                pu238_iso_mass = this[Isotope.pu238].v * (decl_pu_mass) / 100.0;
                pu239_iso_mass = this[Isotope.pu239].v * (decl_pu_mass) / 100.0;
                pu240_iso_mass = this[Isotope.pu240].v * (decl_pu_mass) / 100.0;
                pu241_iso_mass = this[Isotope.pu241].v * (decl_pu_mass) / 100.0;
                pu242_iso_mass = this[Isotope.pu242].v * (decl_pu_mass) / 100.0;
                pu238_decay_fract_pu_to_now = Math.Exp((-LN2 / PU238HL) * (ref_days - pu_days));
                pu239_decay_fract_pu_to_now = Math.Exp((-LN2 / PU239HL) * (ref_days - pu_days));
                pu240_decay_fract_pu_to_now = Math.Exp((-LN2 / PU240HL) * (ref_days - pu_days));
                pu241_decay_fract_pu_to_now = Math.Exp((-LN2 / PU241HL) * (ref_days - pu_days));
                pu242_decay_fract_pu_to_now = Math.Exp((-LN2 / PU242HL) * (ref_days - pu_days));
                cur_mass_pu238 = pu238_iso_mass * pu238_decay_fract_pu_to_now;
                cur_mass_pu239 = pu239_iso_mass * pu239_decay_fract_pu_to_now;
                cur_mass_pu240 = pu240_iso_mass * pu240_decay_fract_pu_to_now;
                cur_mass_pu241 = pu241_iso_mass * pu241_decay_fract_pu_to_now;
                cur_mass_pu242 = pu242_iso_mass * pu242_decay_fract_pu_to_now;
                decl_pu_mass = cur_mass_pu238 + cur_mass_pu239 + cur_mass_pu240 +
                    cur_mass_pu241 + cur_mass_pu242;
            }

        }


        public override void GenParamList()
        {
            base.GenParamList();
            Table = "isotopics";

            ps.AddRange(TuplePair("pu238", this[Isotope.pu238]));
            ps.AddRange(TuplePair("pu239", this[Isotope.pu239]));
            ps.AddRange(TuplePair("pu240", this[Isotope.pu240]));
            ps.AddRange(TuplePair("pu241", this[Isotope.pu241]));
            ps.AddRange(TuplePair("pu242", this[Isotope.pu242]));
            ps.AddRange(TuplePair("am241", this[Isotope.am241]));
            ps.Add(new DBParamEntry("pu_date", pu_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("am_date", am_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("isotopics_id", id));
            ps.Add(new DBParamEntry("isotopics_source_code", source_code.ToString()));

        }
    }

    public class CompositeIsotopics : ParameterBase, IComparable<CompositeIsotopics>
    {
		// INCC5-style getters
        public double pu238 { get { return this[Isotope.pu238].v; } }
        public double pu239 { get { return this[Isotope.pu239].v; } }
        public double pu240 { get { return this[Isotope.pu240].v; } }
        public double pu241 { get { return this[Isotope.pu241].v; } }
        public double pu242 { get { return this[Isotope.pu242].v; } }
        public double am241 { get { return this[Isotope.am241].v; } }
		public double pu238_err { get { return this[Isotope.pu238].err; } }
        public double pu239_err { get { return this[Isotope.pu239].err; } }
        public double pu240_err { get { return this[Isotope.pu240].err; } }
        public double pu241_err { get { return this[Isotope.pu241].err; } }
        public double pu242_err { get { return this[Isotope.pu242].err; } }
        public double am241_err { get { return this[Isotope.am241].err; } }
		public double Summed   { get { return pu238 + pu239 + pu240 + pu241 + pu242; } }

		public void InitVals()
        {
            pu_date = new DateTime(2010, 1, 1);
            am_date = new DateTime(2010, 1, 1);
            ref_date = new DateTime(2010, 1, 1);
            isotopes = Isotopics.MakeArray();
            isotopes[(int)Isotope.pu240].v = 100.0;
            isotopicComponents = new List<CompositeIsotopic>();
            id = "Default";
            source_code = Isotopics.SourceCode.OD;
            pu_mass = 1;
        }

        internal Tuple[] isotopes;
        public DateTime pu_date;
        public DateTime am_date;
        public DateTime ref_date;
        public string id;
        public Isotopics.SourceCode source_code;
        public float pu_mass;
        public List<CompositeIsotopic> isotopicComponents;

        public void SetPercentages(double Pu238, double Pu239, double Pu240, double Pu241, double Pu242, double Am241)
        {
            isotopes[1].v = Pu238;
            isotopes[2].v = Pu239;
            isotopes[3].v = Pu240;
            isotopes[4].v = Pu241;
            isotopes[5].v = Pu242;
            isotopes[6].v = Am241;
        }

        public void SetVal(Isotope iso, double val)
        {
            isotopes[(int)iso].v = val;
        }

        public void SetValue(Isotope iso, double val)
        {
            isotopes[(int)iso].v = val;
        }

        public Tuple this[Isotope i]
        {
            get { return isotopes[(int)i]; }
        }

		public void SetError(Isotope iso, double val)
        {
            isotopes[(int)iso].sigma = val;
        }

        public CompositeIsotopics()
        {
            InitVals();
        }

        public CompositeIsotopics(CompositeIsotopics iso)
        {
            if (iso == null)
            {
                InitVals();
            }
            else
            {
                pu_date = new DateTime(iso.pu_date.Ticks);
                am_date = new DateTime(iso.am_date.Ticks);
                ref_date = new DateTime(iso.ref_date.Ticks);
                isotopes = Isotopics.CopyArray(iso.isotopes);
                id = string.Copy(iso.id);
                source_code = iso.source_code;
                pu_mass = iso.pu_mass;
                isotopicComponents = new List<CompositeIsotopic>(iso.isotopicComponents);
            }
        }

        public void Copy(CompositeIsotopics src)
        {
            pu_date = new DateTime(src.pu_date.Ticks);
            am_date = new DateTime(src.am_date.Ticks);
            ref_date = new DateTime(src.ref_date.Ticks);
            isotopes = Isotopics.CopyArray(src.isotopes);
            id = string.Copy(src.id);
            source_code = src.source_code;
            pu_mass = src.pu_mass;
            isotopicComponents = new List<CompositeIsotopic>(src.isotopicComponents);
        }

        public void CopyTo(CompositeIsotopics dest)
        {
            dest.pu_date = new DateTime(pu_date.Ticks);
            dest.am_date = new DateTime(am_date.Ticks);
            dest.ref_date = new DateTime(ref_date.Ticks);
            dest.isotopes = Isotopics.CopyArray(isotopes);
            dest.id = string.Copy(id);
            dest.source_code = source_code;
            dest.modified = true;
            dest.pu_mass = pu_mass;
            dest.isotopicComponents = new List<CompositeIsotopic>(isotopicComponents);
        }     

        public void Copy(Isotopics src, float mass)
        {
            pu_date = new DateTime(src.pu_date.Ticks);
            am_date = new DateTime(src.am_date.Ticks);
            isotopes = Isotopics.CopyArray(src.isotopes);
            id = string.Copy(src.id);
            source_code = src.source_code;
            pu_mass = mass;
        }

        public static int Compare(CompositeIsotopics x, CompositeIsotopics y)
        {
            int res = x.id.CompareTo(y.id);

            if (res == 0)
                res = (DateTime.Compare(x.pu_date, y.pu_date));

            if (res == 0)
                res = (DateTime.Compare(x.am_date, y.am_date));

            if (res == 0)
                res = (DateTime.Compare(x.ref_date, y.ref_date));

			if (res == 0)
            {
                // how to meaningfully diff the % here? Try this
                int re2 = 0;
                for (int i = 0; (re2 == 0) && i < x.isotopes.Length; i++)
                {
                    re2 = x.isotopes[i].v.CompareTo(y.isotopes[i].v);
                }
                res = re2;
            }

            if (res == 0)
                res = x.pu_mass.CompareTo(y.pu_mass);

            return res;
        }

        public int CompareTo(CompositeIsotopics other)
        {
            return Compare(this, other);
        }

        public override void GenParamList()
        {
            base.GenParamList();
            Table = "composite_isotopics_rec";

            ps.Add(new DBParamEntry("ci_pu238", pu238));
            ps.Add(new DBParamEntry("ci_pu239", pu239));
            ps.Add(new DBParamEntry("ci_pu240", pu240));
            ps.Add(new DBParamEntry("ci_pu241", pu241));
            ps.Add(new DBParamEntry("ci_pu242", pu242));
            ps.Add(new DBParamEntry("ci_am241", am241));
            ps.Add(new DBParamEntry("ci_pu_date", pu_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("ci_am_date", am_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("ci_ref_date", ref_date));
            ps.Add(new DBParamEntry("ci_isotopics_id", id));
            ps.Add(new DBParamEntry("ci_isotopics_source_code", source_code.ToString()));
            ps.Add(new DBParamEntry("ci_pu_mass", pu_mass));
        }

		public uint CombinedCalculation(out Isotopics newIsotopics, bool INCCParity, NCCReporter.LMLoggers.LognLM logger)
		{
			MassSum = 0;
			uint res = 0;
			newIsotopics = null;
			foreach(CompositeIsotopic ci in isotopicComponents)
			{
				if (ci.pu_mass == 0.0f)
					continue;
				res = ci.CalculateDecayAndMass(ref_date, INCCParity, logger);
				if (res != 0)
					break;
			}
			if (res != 0) return res;
			MassSum = CalculateMassSums();
			if (MassSum <= 0)
			{ 
				logger.TraceEvent(NCCReporter.LogLevels.Error, 36784, "Sum of masses = 0.");
				res = 36784;
			}
			else
				newIsotopics = CreateIsotopicsFromSums(ref_date, INCCParity, MassSum);
			return res;
		}

		internal Tuple[] currentMassSum = Isotopics.MakeArray();
		public double MassSum;
		public double CalculateMassSums()
		{
			double mass_sum = 0;
			for (Isotope iso = Isotope.pu238; iso <= Isotope.am241; iso++)
			{
				currentMassSum[(int)iso].v = 0;
			}
			foreach(CompositeIsotopic ci in isotopicComponents)
			{
				if (ci.pu_mass == 0.0f)
					continue;
				for (Isotope iso = Isotope.pu238; iso <= Isotope.am241; iso++)
				{
					currentMassSum[(int)iso].v += ci.CurrentMass[(int)iso].v;
				}
			}
            for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
            {
                mass_sum += currentMassSum[(int)iso].v;
            }
            return mass_sum;
		}

		public Isotopics CreateIsotopicsFromSums(DateTime ref_date, bool INCCParity, double mass_sum)
		{
			Isotopics isor = new Isotopics();
			isor.pu_date = new DateTime(ref_date.Ticks);
			isor.am_date = new DateTime(ref_date.Ticks);
			isor.source_code = source_code;
			isor.id = string.Copy(id);
			for (Isotope iso = Isotope.pu238; iso <= Isotope.am241; iso++)
			{
				isor.SetVal(iso, 100.0 * currentMassSum[(int)iso].v / mass_sum);
			}
			return isor;
		}

    }

    public class CompositeIsotopic : ParameterBase, IComparable<CompositeIsotopic>
    {  
        // INCC5-style getters
        public double pu238 { get { return this[Isotope.pu238].v; } }
        public double pu239 { get { return this[Isotope.pu239].v; } }
        public double pu240 { get { return this[Isotope.pu240].v; } }
        public double pu241 { get { return this[Isotope.pu241].v; } }
        public double pu242 { get { return this[Isotope.pu242].v; } }
        public double am241 { get { return this[Isotope.am241].v; } }
        public double pu238_err { get { return this[Isotope.pu238].err; } }
        public double pu239_err { get { return this[Isotope.pu239].err; } }
        public double pu240_err { get { return this[Isotope.pu240].err; } }
        public double pu241_err { get { return this[Isotope.pu241].err; } }
        public double pu242_err { get { return this[Isotope.pu242].err; } }
        public double am241_err { get { return this[Isotope.am241].err; } }

		public double Summed   { get { return pu238 + pu239 + pu240 + pu241 + pu242; } }

		internal  Tuple[] CurrentMass = Isotopics.MakeArray();

		public uint CalculateDecayAndMass(DateTime ref_date, bool INCCParity, NCCReporter.LMLoggers.LognLM logger)
		{
			double ref_days;
			double pu_days;
			double am_days;
			Tuple[] iso_mass = Isotopics.MakeArray();
			Tuple[] decay_fract_pu_to_am = Isotopics.MakeArray();
			Tuple[] decay_fract_am_to_now = Isotopics.MakeArray();
			Tuple[] decay_fract_pu_to_now = Isotopics.MakeArray();

			double temp = 0.0;
			double temp_sum = 0.0;
			double pumass = pu_mass;

			double isosum = 0.0;
			for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
			{
				isosum += isotopes[(int)iso].v;
			}

			if (isosum <= 0.0)
			{
				logger.TraceEvent(NCCReporter.LogLevels.Warning, 36783, "Unable to update isotopics, sum of Pu isotopes must be greater than zero");
				return 36783;
			}
			Tuple[] newiso = Isotopics.MakeArray();
			if (INCCParity)
			{
				pu_days = (pu_date.Date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
				am_days = (am_date.Date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
				ref_days = (long)(ref_date.Date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
			} else
			{
				pu_days = (pu_date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
				am_days = (am_date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
				ref_days = (long)(ref_date.Subtract(Isotopics.ZeroIAEATime)).TotalDays;
			}

			for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
			{
				iso_mass[(int)iso].v = (isotopes[(int)iso].v * pumass / 100.0);
				decay_fract_pu_to_am[(int)iso].v = Math.Exp((-Isotopics.LN2 / Isotopics.Halflives[(int)iso]) * (am_days - pu_days));
				decay_fract_pu_to_now[(int)iso].v = Math.Exp((-Isotopics.LN2 / Isotopics.Halflives[(int)iso]) * (ref_days - pu_days));
			}
			decay_fract_am_to_now[(int)Isotope.pu241].v = Math.Exp((-Isotopics.LN2 / Isotopics.Halflives[(int)Isotope.pu241]) * (ref_days - am_days));
			decay_fract_am_to_now[(int)Isotope.am241].v = Math.Exp((-Isotopics.LN2 / Isotopics.Halflives[(int)Isotope.am241]) * (ref_days - am_days));

			for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
			{
				CurrentMass[(int)iso].v = iso_mass[(int)iso].v * decay_fract_pu_to_now[(int)iso].v;
			}
			CurrentMass[(int)Isotope.am241].v = decay_fract_am_to_now[(int)Isotope.am241].v * isotopes[(int)Isotope.am241].v / 100.0;

			for (Isotope iso = Isotope.pu238; iso <= Isotope.pu242; iso++)
			{
				temp_sum += (decay_fract_pu_to_am[(int)iso].v * iso_mass[(int)iso].v);
			}

			temp = decay_fract_pu_to_am[(int)Isotope.pu241].v * iso_mass[(int)Isotope.pu241].v *
								  (decay_fract_am_to_now[(int)Isotope.am241].v - decay_fract_am_to_now[(int)Isotope.pu241].v)
								  * (Isotopics.LN2 / Isotopics.Halflives[(int)Isotope.pu241]) / ((Isotopics.LN2 / Isotopics.Halflives[(int)Isotope.pu241]) - (Isotopics.LN2 / Isotopics.Halflives[(int)Isotope.am241]));
			CurrentMass[(int)Isotope.am241].v = (CurrentMass[(int)Isotope.am241].v * temp_sum) + temp;
			return 0;
		}

		public void InitVals()
        {
            pu_date = new DateTime(2010, 1, 1);
            am_date = new DateTime(2010, 1, 1);
            isotopes = Isotopics.MakeArray();
            isotopes[(int)Isotope.pu240].v = 100.0;
            pu_mass = 1;
        }

        Tuple[] isotopes;
        public DateTime pu_date;
        public DateTime am_date;
        public float pu_mass;

        public void SetPercentages(double Pu238, double Pu239, double Pu240, double Pu241, double Pu242, double Am241)
        {
            isotopes[1].v = Pu238;
            isotopes[2].v = Pu239;
            isotopes[3].v = Pu240;
            isotopes[4].v = Pu241;
            isotopes[5].v = Pu242;
            isotopes[6].v = Am241;
        }

        public void SetVal(Isotope iso, double val)
        {
            isotopes[(int)iso].v = val;
        }

        public void SetValue(Isotope iso, double val)
        {
            isotopes[(int)iso].v = val;
        }

        public Tuple this[Isotope i]
        {
            get { return isotopes[(int)i]; }
        }

        public void SetError(Isotope iso, double val)
        {
            isotopes[(int)iso].sigma = val;
        }

        public CompositeIsotopic()
        {
            InitVals();
        }

        public CompositeIsotopic(CompositeIsotopic iso)
        {
            if (iso == null)
            {
                InitVals();
            }
            else
            {
                pu_date = new DateTime(iso.pu_date.Ticks);
                am_date = new DateTime(iso.am_date.Ticks);
                isotopes = Isotopics.CopyArray(iso.isotopes);
                pu_mass = iso.pu_mass;
            }
        }

        public void Copy(CompositeIsotopic src)
        {
            pu_date = new DateTime(src.pu_date.Ticks);
            am_date = new DateTime(src.am_date.Ticks);
            isotopes = Isotopics.CopyArray(src.isotopes);
            pu_mass = src.pu_mass;
        }

        public void CopyTo(CompositeIsotopic dest)
        {
            dest.pu_date = new DateTime(pu_date.Ticks);
            dest.am_date = new DateTime(am_date.Ticks);
            dest.isotopes = Isotopics.CopyArray(isotopes);
            dest.modified = true;
            dest.pu_mass = pu_mass;
        }

        public void Copy(Isotopics src, float mass)
        {
            pu_date = new DateTime(src.pu_date.Ticks);
            am_date = new DateTime(src.am_date.Ticks);
            isotopes = Isotopics.CopyArray(src.isotopes);
            pu_mass = mass;
        }

        public static int Compare(CompositeIsotopic x, CompositeIsotopic y)
        {
            int res = (DateTime.Compare(x.pu_date, y.pu_date));

            if (res == 0)
                res = (DateTime.Compare(x.am_date, y.am_date));

            if (res == 0)
            {
                // how to meaningfully diff the % here? Try this
                int re2 = 0;
                for (int i = 0; (re2 == 0) && i < x.isotopes.Length; i++)
                {
                    re2 = x.isotopes[i].v.CompareTo(y.isotopes[i].v);
                }
                res = re2;
            }

            if (res == 0)
                res = x.pu_mass.CompareTo(y.pu_mass);

            return res;
        }

        public int CompareTo(CompositeIsotopic other)
        {
            return Compare(this, other);
        }        
        public override void GenParamList()
        {
            base.GenParamList();
            Table = "composite_isotopic_rec";

            ps.Add(new DBParamEntry("pu238", pu238));
            ps.Add(new DBParamEntry("pu239", pu239));
            ps.Add(new DBParamEntry("pu240", pu240));
            ps.Add(new DBParamEntry("pu241", pu241));
            ps.Add(new DBParamEntry("pu242", pu242));
            ps.Add(new DBParamEntry("am241", am241));
            ps.Add(new DBParamEntry("pu_date", pu_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("am_date", am_date.ToString("yyyy-MM-dd")));
            ps.Add(new DBParamEntry("pu_mass", pu_mass));

        }
    }

    public class Stratum : ParameterBase, IComparable<Stratum>
    {
        public double bias_uncertainty
        {
            get;
            set;
        }
        public double random_uncertainty
        {
            get;
            set;
        }
        public double systematic_uncertainty
        {
            get;
            set;
        }
        public double relative_std_dev // computed during mass analysis
        {
            get;
            set;
        }

        // all zeros is the same as not being set
        public bool Unset
        {
            get { return (0.0 == bias_uncertainty) && (0.0 == random_uncertainty) == (0.0 == systematic_uncertainty); }
        }
        public Stratum()
        {
        }
        public Stratum(Stratum src)
        {
			CopyFrom(src);
        }

		public void CopyFrom(Stratum src)
		{
            if (src == null)
                return;
            bias_uncertainty = src.bias_uncertainty;
            random_uncertainty = src.random_uncertainty;
            systematic_uncertainty = src.systematic_uncertainty;
            relative_std_dev = src.relative_std_dev; // computed during mass analysis
		}

        public static int Compare(Stratum x, Stratum y)
        {
            int res = x.bias_uncertainty.CompareTo(y.bias_uncertainty);

            if (res == 0)
                res = x.random_uncertainty.CompareTo(y.random_uncertainty);

            if (res == 0)
                res = x.systematic_uncertainty.CompareTo(y.systematic_uncertainty);

            if (res == 0)
                res = x.relative_std_dev.CompareTo(y.relative_std_dev);

            return res;
        }

        public int CompareTo(Stratum other)
        {
            return Compare(this, other);
        }

        public Double[] ValueArray
        {
            get
            {
                Double[] x = new Double[4] { bias_uncertainty, random_uncertainty, systematic_uncertainty, relative_std_dev };
                return x;
            }
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "stratum_ids";
            this.ps.Add(new DBParamEntry("name", "-"));
            this.ps.Add(new DBParamEntry("description", "-"));
            this.ps.Add(new DBParamEntry("historical_bias", bias_uncertainty));
            this.ps.Add(new DBParamEntry("historical_rand_uncert", random_uncertainty));
            this.ps.Add(new DBParamEntry("historical_systematic_uncert", systematic_uncertainty));
        }

        public List<DBParamEntry> SubsetForResults()
        {
            List<DBParamEntry> s = new List<DBParamEntry>();
            s.Add(new DBParamEntry("bias_uncertainty", bias_uncertainty));
            s.Add(new DBParamEntry("random_uncertainty", random_uncertainty));
            s.Add(new DBParamEntry("systematic_uncertainty", systematic_uncertainty));
            s.Add(new DBParamEntry("relative_std_dev", relative_std_dev));

            return s;
        }
    }


}
