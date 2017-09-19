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
using System.Collections.ObjectModel;
using NCCReporter;

namespace AnalysisDefs
{

    using Tuple = VTuple;
    public enum CollarType { AmLiThermal = 0, AmLiFast = 1, CfThermal = 2, CfFast = 3 };
    
    // map from a selector pair key to a specific INCC analysis parameter instance
    public class INCCMethods : Dictionary<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor>
    {

    }

    // dev note: Collar is really two (Thermal mode and Fast (Cd)", the 'mode'), so this 1-1 mapping must be expanded somehow, ooops!
    public enum AnalysisMethod
    {
        None,
        CalibrationCurve, KnownA, KnownM, Multiplicity, AddASource, Active, ActiveMultiplicity, ActivePassive, Collar, INCCNone, CuriumRatio, TruncatedMultiplicity, 
        /*EachEntryAfterHereSupportsINCCHacks,*/
        DUAL_ENERGY_MULT_SAVE_RESTORE, COLLAR_SAVE_RESTORE, COLLAR_DETECTOR_SAVE_RESTORE, COLLAR_K5_SAVE_RESTORE, WMV_CALIB_TOKEN
    }
    public static class AnalysisMethodExtensions
    {
        public static bool IsNone(this AnalysisMethod am)
        {
            return (am == AnalysisMethod.None || am == AnalysisMethod.INCCNone);
        }

        public static string FullName(this AnalysisMethod am)
        {
            string s = "";
            switch (am)
            {
                case AnalysisMethod.KnownA:
                    s = "Known alpha";
                    break;
                case AnalysisMethod.CalibrationCurve:
                    s = "Passive calibration curve";
                    break;
                case AnalysisMethod.KnownM:
                    s = "Known M";
                    break;
                case AnalysisMethod.Multiplicity:
                    s = "Multiplicity";
                    break;
                case AnalysisMethod.TruncatedMultiplicity:
                    s = "Truncated multiplicity";
                    break;
                case AnalysisMethod.AddASource:
                    s = "Add-A-Source";
                    break;
                case AnalysisMethod.CuriumRatio:
                    s = "Curium ratio";
                    break;
                case AnalysisMethod.Active:
                    s = "Active calibration curve";
                    break;
                case AnalysisMethod.ActivePassive:
                    s = "Active/Passive";
                    break;
                case AnalysisMethod.Collar:
                    s = "Collar";
                    break;
                case AnalysisMethod.ActiveMultiplicity:
                   s = "Active multiplicity";
                    break;
                case AnalysisMethod.INCCNone:
                    s = "INCC5 Analysis method selector";
                    break;
                case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
                    s = "Dual energy multiplicity";
                    break;
                default:
                    s = am.ToString();
                    break;
            }
            return s;
        }
    }

    public class AnalysisMethods : ParameterBase, IEquatable<AnalysisMethods>
    {
        public AnalysisMethods()
        {
            init();
        }

        public AnalysisMethods(INCCSelector sel)
        {
            init();
            this.selector = new INCCSelector(sel);
        }

        public AnalysisMethods(LMLoggers.LognLM logger)
        {
            mlogger = logger;
            init();
        }

        private void init()
        {
            choices = new bool[Enum.GetValues(typeof(AnalysisMethod)).Length];
            choices[(int)AnalysisMethod.None] = true;  // None
            choices[(int)AnalysisMethod.INCCNone] = true;  // INCCNone
            methods = new INCCMethods();
        }

        private LMLoggers.LognLM mlogger;

        public INCCSelector selector;
        public AnalysisMethod Normal;
        public AnalysisMethod Backup;
        public AnalysisMethod Auxiliary;
        public bool[] choices;

        public void CopySettings(AnalysisMethods src)
        {
            Array.Copy(src.choices, choices, choices.Length);
            Normal = src.Normal;
            Backup = src.Backup;
            Auxiliary = src.Auxiliary;
            if (src.selector != null)
                selector = new INCCSelector(src.selector);
            modified = true;
        }

        public bool Has(AnalysisMethod m)
        {
            return (choices[(int)m]);
        }
        public bool AnySelected()
        {
            foreach (AnalysisMethod am in Enum.GetValues(typeof(AnalysisMethod)))
            {
                if (am.IsNone() || am > AnalysisMethod.TruncatedMultiplicity)
                    continue;
                if (choices[(int)am])
                    return true;
            }
            return false;
        }
        public AnalysisMethod GetFirstSelected ()
        {
            int i = (int) AnalysisMethod.None;
            while ((choices[i]) == false && i <= (int) AnalysisMethod.TruncatedMultiplicity)
            {
                i++;
            }
            return i == (int) AnalysisMethod.TruncatedMultiplicity?AnalysisMethod.None:(AnalysisMethod) i;
        }
        public AnalysisMethod GetSecondSelected()
        {
            int j = (int) (GetFirstSelected());
            int i = j + 1;
            
            if (!AnySelected())
                return AnalysisMethod.None;
            else
            {                
                while ((choices[i]) == false && i <= (int)AnalysisMethod.TruncatedMultiplicity)
                {
                    i++;
                }
                if (i > (int)AnalysisMethod.TruncatedMultiplicity)
                    return AnalysisMethod.None;
                else
                    return (AnalysisMethod)i;
            }
        }
        public AnalysisMethod GetThirdSelected()
        {
            int j = (int)(GetSecondSelected());
            int i = j + 1;
            if (!AnySelected())
                return
                    AnalysisMethod.None;
            else
            {
                while ((choices[i]) == false && i <= (int)AnalysisMethod.TruncatedMultiplicity)
                {
                    i++;
                }
                if (i > (int)AnalysisMethod.TruncatedMultiplicity)
                    return AnalysisMethod.None;
                else
                    return (AnalysisMethod)i;
            }
        }
        public bool VerificationAnalysisSelected() // todo: check this definition range
        {
            foreach (AnalysisMethod am in Enum.GetValues(typeof(AnalysisMethod)))
                if (am > AnalysisMethod.None && am <= AnalysisMethod.TruncatedMultiplicity && (am != AnalysisMethod.INCCNone))
                    if (choices[(int)am])
                        return true;
            return false;
        }
        public bool MassOutlierMethodSelected()
        {
            foreach (AnalysisMethod am in Enum.GetValues(typeof(AnalysisMethod)))
                if (massoutlier[(int)am] && choices[(int)am])
                    return true;
            return false;
        }
        public bool CalibrationAnalysisSelected()
        {
            foreach (AnalysisMethod am in Enum.GetValues(typeof(AnalysisMethod)))
                if (calib[(int)am] && choices[(int)am])
                    return true;
            return false;
        }
        public bool HasActiveSelected()
        {
            return (choices[(int)AnalysisMethod.Active] || choices[(int)AnalysisMethod.ActivePassive] || choices[(int)AnalysisMethod.ActiveMultiplicity]);
        }
        public bool HasActiveMultSelected()
        {
            return (choices[(int)AnalysisMethod.ActiveMultiplicity]);
        }

        private INCCMethods methods;

        public INCCAnalysisParams.INCCMethodDescriptor GetMethodParameters(AnalysisMethod am)
        {
            INCCAnalysisParams.INCCMethodDescriptor surr;
            //if (am.Equals(AnalysisMethod.Collar))
            //    ;
            bool got = methods.TryGetValue(am, out surr);
            return surr;
        }

        public bool HasMethod(AnalysisMethod am)
        {
            return methods.ContainsKey(am);
        }

        public void AddMethod(AnalysisMethod am, INCCAnalysisParams.INCCMethodDescriptor surr)
        {
            if (!methods.ContainsKey(am))
            {
                methods.Add(am, surr);
            }
            else if (NCC.CentralizedState.App.AppContext.OverwriteImportedDefs)
            {
                methods.Remove(am);
                methods.Add(am, surr);
            }
            else
                if (mlogger != null) mlogger.TraceEvent(NCCReporter.LogLevels.Warning, 34901, "{0} method not updated or replaced", am.FullName());
                
        }

        public System.Collections.IEnumerator GetMethodEnumerator()
        {
            foreach (AnalysisMethod am in Enum.GetValues(typeof(AnalysisMethod)))
                if (am > AnalysisMethod.None && am <= AnalysisMethod.TruncatedMultiplicity && (am != AnalysisMethod.INCCNone))
                    if (choices[(int)am])
                        yield return new Tuple<AnalysisMethod,INCCAnalysisParams.INCCMethodDescriptor>(am, (methods.ContainsKey(am) ?  methods[am] : null));
        }
        public bool Equals(AnalysisMethods other)
        {
            for (int i = 0; i< choices.Length; i++)
                if (choices[i] != other.choices[i])
                    return false;
            return (Normal == other.Normal &&
                    Backup == other.Backup &&
                    Auxiliary == other.Auxiliary &&
                    ((selector != null && other.selector != null) ? selector.Equals(other.selector) : true /* return true if one is null*/));  // selector equality definition was not correct
        }

        public override void GenParamList()
        {
            base.GenParamList();
            Table = "analysis_method_rec";
            ps.Add(new DBParamEntry("known_alpha", choices[(int)AnalysisMethod.KnownA]));
            ps.Add(new DBParamEntry("known_m", choices[(int)AnalysisMethod.KnownM]));
            ps.Add(new DBParamEntry("multiplicity", choices[(int)AnalysisMethod.Multiplicity]));
            ps.Add(new DBParamEntry("cal_curve", choices[(int)AnalysisMethod.CalibrationCurve]));
            ps.Add(new DBParamEntry("add_a_source", choices[(int)AnalysisMethod.AddASource]));
            ps.Add(new DBParamEntry("active", choices[(int)AnalysisMethod.Active]));
            ps.Add(new DBParamEntry("active_passive", choices[(int)AnalysisMethod.ActivePassive]));
            ps.Add(new DBParamEntry("active_mult", choices[(int)AnalysisMethod.ActiveMultiplicity]));
            ps.Add(new DBParamEntry("collar", choices[(int)AnalysisMethod.Collar]));
            ps.Add(new DBParamEntry("truncated_mult", choices[(int)AnalysisMethod.TruncatedMultiplicity]));
            ps.Add(new DBParamEntry("curium_ratio", choices[(int)AnalysisMethod.CuriumRatio]));
            ps.Add(new DBParamEntry("normal_method", (int)Normal));
            ps.Add(new DBParamEntry("backup_method", (int)Backup));
            ps.Add(new DBParamEntry("aux_method", (int)Auxiliary));
        }

        static bool[] massoutlier;
        static bool[] calib;
        static AnalysisMethods()
        {
            calib = new bool[Enum.GetValues(typeof(AnalysisMethod)).Length];

            calib[(int)AnalysisMethod.CalibrationCurve] = true;
            calib[(int)AnalysisMethod.KnownA] = true;
            calib[(int)AnalysisMethod.Active] = true;
            calib[(int)AnalysisMethod.AddASource] = true;

            massoutlier = new bool[Enum.GetValues(typeof(AnalysisMethod)).Length];
            massoutlier[(int)AnalysisMethod.CalibrationCurve] = true;
            massoutlier[(int)AnalysisMethod.KnownA] = true;
            massoutlier[(int)AnalysisMethod.KnownM] = true;
            massoutlier[(int)AnalysisMethod.Multiplicity] = true;
            massoutlier[(int)AnalysisMethod.TruncatedMultiplicity] = true;
            massoutlier[(int)AnalysisMethod.CuriumRatio] = true;
            // NEXT: add collar and others to these lists?
        }
    }


    public enum Coeff { a, b, c, d }  // old school limits
    public class INCCAnalysisParams 
    {

        // this will not be needed; it was a flag to tell the INCC DB lookup to grab the top bytes of one of six record definitions with identical first fields 
        public enum CalCurveAnalysisFlag { PASSIVE_ANALYSIS, ACTIVE_ANALYSIS, ADDASRC_ANALYSIS, ACTIVE_PASSIVE_ANALYSIS, COLLAR_ANALYSIS, CURIUM_RATIO_ANALYSIS }
        public enum CalCurveResult { Unknown, Success, Fail, FailedOnMassLimit, EpicFailLOL };
        public enum CurveEquation
        {
            CUBIC, // a + bm + cm^2 + dm^3
            POWER, // am^b
            HOWARDS, // am / (1 + bm)
            EXPONENTIAL // a (1 - exp(bm))
        }


        public enum CalCurveType { STD, HM, U } // varies from analysis to analysis

        public const int MAX_NUM_CALIB_PTS = 20;

        public class CurveEquationVals : INCCMethodDescriptor
        {
            public CurveEquation cal_curve_equation;
            protected double[] coeff;
            protected double[] var;
            public double[,] _covar;
            public double sigma_x;
            public double lower_mass_limit;
            public double upper_mass_limit;

            public double a
            {
                get { return coeff[(int)Coeff.a]; }
                set { coeff[(int)Coeff.a] = value;  }
            }
            public double b
            {
                get { return coeff[(int)Coeff.b]; }
                set { coeff[(int)Coeff.b] = value; }
            }
            public double c
            {
                get { return coeff[(int)Coeff.c]; }
                set { coeff[(int)Coeff.c] = value; }
            }
            public double d
            {
                get { return coeff[(int)Coeff.d]; }
                set { coeff[(int)Coeff.d] = value; }
            }
            public double var_a
            {
                get { return var[(int)Coeff.a]; }
                set { var[(int)Coeff.a] = value; }
            }
            public double var_b
            {
                get { return var[(int)Coeff.b]; }
                set { var[(int)Coeff.b] = value; }
            }
            public double var_c
            {
                get { return var[(int)Coeff.c]; }
                set { var[(int)Coeff.c] = value; }
			}

            public double var_d
            {
                get { return var[(int)Coeff.d]; }
                set { var[(int)Coeff.d] = value; }
            }
            public double covar(Coeff x, Coeff y)
            {
                return _covar[(int)x, (int)y];
            }
            public void setcovar(Coeff x, Coeff y, double v)
            {
                _covar[(int)x, (int)y] = v;
            }
            public bool useSingles; // the 2009 singles v. doubles rates flag for cal curve

            public CurveEquationVals()
            {
                int coeffnum = Enum.GetValues(typeof(Coeff)).Length;
                coeff = new double[coeffnum];
                coeff[1] = 1;
                var = new double[coeffnum];
                _covar = (double[,])Array.CreateInstance(typeof(double), coeffnum, coeffnum);
                Status = CalCurveResult.Unknown;
                lower_mass_limit = -1e8;
                upper_mass_limit = 1e8;
            }

            public CurveEquationVals(CurveEquationVals src)
            {
                int coeffnum = Enum.GetValues(typeof(Coeff)).Length;
                coeff = new double[coeffnum];
                var = new double[coeffnum];
                _covar = (double[,])Array.CreateInstance(typeof(double), coeffnum, coeffnum);
                Status = CalCurveResult.Unknown;
                lower_mass_limit = -1e8;
                upper_mass_limit = 1e8;
                if (src == null) return;

                Array.Copy(src.coeff, coeff, coeff.Length);
                Array.Copy(src.var, var, var.Length);
                Array.Copy(src._covar, _covar, _covar.Length);
                lower_mass_limit = src.lower_mass_limit;
                upper_mass_limit = src.upper_mass_limit;
                cal_curve_equation = src.cal_curve_equation;
                sigma_x = src.sigma_x;
                useSingles = src.useSingles;
                Status = src.Status;
            }

            public void CopyTo(CurveEquationVals cev)
            {
                if (cev == null) 
                    return;          
                Array.Copy(coeff, cev.coeff, coeff.Length);
                Array.Copy(var, cev.var, var.Length);
                Array.Copy(_covar, cev._covar, _covar.Length);
                cev.cal_curve_equation = cal_curve_equation;
                cev.sigma_x = sigma_x;
                cev.useSingles = useSingles;
                cev.Status = Status;
                cev.lower_mass_limit = lower_mass_limit;
                cev.upper_mass_limit = upper_mass_limit;
                modified = true;
            }

            // new, carry analysis state with the values
            public CalCurveResult Status;

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 0, INCCStyleSection.ReportSection.MethodResults);
                //sec.AddHeader("-- curve calibration parameters");  // section header
                // Passive calibration curve calibration parameters
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddTwo("Equation:", cal_curve_equation.ToDisplayString(useSingles)); // if useSingles then display with S instead of D in the equation stringo
                sec.AddTwo("a:", a);
                sec.AddTwo("b:", b);
                sec.AddTwo("c:", c);
                sec.AddTwo("d:", d);
                sec.AddTwo("variance a:", var_a);
                sec.AddTwo("variance b:", var_b);
                sec.AddTwo("variance c:", var_c);
                sec.AddTwo("variance d:", var_d);
                sec.AddTwo("covariance ab:", covar(Coeff.a, Coeff.b));
                sec.AddTwo("covariance ac:", covar(Coeff.a, Coeff.c));
                sec.AddTwo("covariance ad:", covar(Coeff.a, Coeff.d));
                sec.AddTwo("covariance bc:", covar(Coeff.b, Coeff.c));
                sec.AddTwo("covariance bd:", covar(Coeff.b, Coeff.d));
                sec.AddTwo("covariance cd:", covar(Coeff.c, Coeff.d));
                sec.AddTwo("sigma x:", sigma_x);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "cev";
                ps.Add(new DBParamEntry("cal_curve_equation", (int)cal_curve_equation));
                ps.Add(new DBParamEntry("a", a));
                ps.Add(new DBParamEntry("b", b));
                ps.Add(new DBParamEntry("var_a", var_a));
                ps.Add(new DBParamEntry("var_b", var_b));
                ps.Add(new DBParamEntry("var_c", var_c));
                ps.Add(new DBParamEntry("var_d", var_d));
                ps.Add(new DBParamEntry("c", c));
                ps.Add(new DBParamEntry("d", d));
                ps.Add(new DBParamEntry("covar_ab", covar(Coeff.a, Coeff.b)));
                ps.Add(new DBParamEntry("covar_ac", covar(Coeff.a, Coeff.c)));
                ps.Add(new DBParamEntry("covar_ad", covar(Coeff.a, Coeff.d)));
                ps.Add(new DBParamEntry("covar_bc", covar(Coeff.b, Coeff.c)));
                ps.Add(new DBParamEntry("covar_bd", covar(Coeff.b, Coeff.d)));
                ps.Add(new DBParamEntry("covar_cd", covar(Coeff.c, Coeff.d)));
                ps.Add(new DBParamEntry("sigma_x", sigma_x));
                ps.Add(new DBParamEntry("upper_mass_limit", upper_mass_limit));
                ps.Add(new DBParamEntry("lower_mass_limit", lower_mass_limit));
            }

        }



        // could use this to push the targeted calculations onto to the parameter class itself, OO
        public class INCCMethodDescriptor : ParameterBase, IINCCStringRep
        {
            // holder of the shared values and methods, whatever they may be
            // the old INCC report output style label -> value(s), convert to lines by ToString() on each row entry
            public virtual List<Row> ToLines(Measurement m) { return new List<Row>(); }

            // suitable for spreadsheet use, convert to lines by ToString() on each row entry
            public virtual List<Row> ToColumns(Measurement m) { return new List<Row>(); }

            // implement in subclass
            public virtual void CopyTo(INCCMethodDescriptor imd) { }

        }

        public enum KnownAlphaVariant { Conventional, HeavyMetalCorrection, MoistureCorrAppliedToDryAlpha, MoistureCorrAppliedToMultCorrDoubles }
        public class known_alpha_rec : INCCMethodDescriptor
        {
            public double alpha_wt;
            public double rho_zero=1;//change default to get results....hn 6.25.2015
            public double k;
            public CurveEquationVals cev; // only uses a and b for this one
            public KnownAlphaVariant known_alpha_type;
            public double heavy_metal_corr_factor;
            public double heavy_metal_reference;
            public CurveEquationVals ring_ratio;  // uses only eq, a,b,c,d
            public double lower_corr_factor_limit;
            public double upper_corr_factor_limit;
            public double[] dcl_mass;
            public double[] doubles;

            public known_alpha_rec()
            {
                cev = new CurveEquationVals();
                ring_ratio = new CurveEquationVals();
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                alpha_wt = 1.0;
                k = 2.166;
                lower_corr_factor_limit = 0.0;
                upper_corr_factor_limit = 1e8;
                cev.b = 1.0; //change default to get results....hn 6.25.2015
            }

            public known_alpha_rec(known_alpha_rec src)
            {
                cev = new CurveEquationVals(src.cev);
                ring_ratio = new CurveEquationVals(src.ring_ratio);
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                Array.Copy(src.dcl_mass, dcl_mass, dcl_mass.Length);
                Array.Copy(src.doubles, doubles, dcl_mass.Length);
                alpha_wt = src.alpha_wt;
                rho_zero = src.rho_zero;
                k = src.k;
                known_alpha_type = src.known_alpha_type;
                heavy_metal_corr_factor = src.heavy_metal_corr_factor;
                heavy_metal_reference = src.heavy_metal_reference;
                lower_corr_factor_limit = src.lower_corr_factor_limit;
                upper_corr_factor_limit = src.upper_corr_factor_limit;
            }

            public override void CopyTo(INCCMethodDescriptor imd) 
            {
                known_alpha_rec kar = (known_alpha_rec)imd;
                cev.CopyTo(kar.cev);
                ring_ratio.CopyTo(kar.ring_ratio);
                dcl_mass.CopyTo(kar.dcl_mass, 0);
                doubles.CopyTo(kar.dcl_mass, 0);
                kar.alpha_wt = alpha_wt;
                kar.rho_zero = rho_zero;
                kar.k = k;
                kar.known_alpha_type = known_alpha_type;
                kar.heavy_metal_corr_factor = heavy_metal_corr_factor;
                kar.heavy_metal_reference = heavy_metal_reference;
                kar.lower_corr_factor_limit = lower_corr_factor_limit;
                kar.upper_corr_factor_limit = upper_corr_factor_limit;
                kar.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Known alpha calibration parameters");
                sec.AddTwo("Alpha weight:", alpha_wt);
                sec.AddTwo("Rho zero:", rho_zero);
                sec.AddTwo("k:", k);
                sec.AddTwo("a:", cev.a);
                sec.AddTwo("b:", cev.b);
                sec.AddTwo("variance a:", cev.var_a);
                sec.AddTwo("variance b:", cev.var_b);
                sec.AddTwo("covariance ab:", cev.covar(Coeff.a, Coeff.b));
                sec.AddTwo("sigma x:", cev.sigma_x);

                if (known_alpha_type == KnownAlphaVariant.HeavyMetalCorrection)
                {
                    sec.AddTwo("Heavy metal correction factor:", heavy_metal_corr_factor);
                    sec.AddTwo("Heavy metal reference:", heavy_metal_reference);
                }
                if ((known_alpha_type == KnownAlphaVariant.MoistureCorrAppliedToDryAlpha) ||
                    (known_alpha_type == KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles))
                {
                    sec.AddTwo("Moisture correction equation:", ring_ratio.cal_curve_equation.ToString());
                    sec.AddTwo("Moisture correction a:", ring_ratio.a);
                    sec.AddTwo("Moisture correction b:", ring_ratio.b);
                    sec.AddTwo("Moisture correction c:", ring_ratio.c);
                    sec.AddTwo("Moisture correction d:", ring_ratio.d);
                }
                return sec;
            }

            public override void GenParamList() 
            {
                base.GenParamList();
                Table = "known_alpha_rec";
                ps.Add(new DBParamEntry("alpha_wt", alpha_wt));
                ps.Add(new DBParamEntry("rho_zero", rho_zero));
                ps.Add(new DBParamEntry("k", k));
                ps.Add(new DBParamEntry("known_alpha_type", (int)known_alpha_type));

                ps.Add(new DBParamEntry("a", cev.a));
                ps.Add(new DBParamEntry("b", cev.b)); ;
                ps.Add(new DBParamEntry("var_a", cev.var_a));
                ps.Add(new DBParamEntry("var_b", cev.var_b));
                ps.Add(new DBParamEntry("covar_ab", cev.covar(Coeff.a, Coeff.b)));
                ps.Add(new DBParamEntry("sigma_x", cev.sigma_x));
                ps.Add(new DBParamEntry("lower_corr_factor_limit", lower_corr_factor_limit));
                ps.Add(new DBParamEntry("upper_corr_factor_limit", upper_corr_factor_limit));
                //Missing lower/upper mass limits
                ps.Add(new DBParamEntry("lower_mass_limit",cev.lower_mass_limit));
                ps.Add(new DBParamEntry("upper_mass_limit", cev.upper_mass_limit));
                ps.Add(new DBParamEntry("heavy_metal_reference", heavy_metal_reference));
                ps.Add(new DBParamEntry("heavy_metal_corr_factor", heavy_metal_corr_factor));

                ps.Add(new DBParamEntry("ring_ratio_equation", (int)ring_ratio.cal_curve_equation));
                ps.Add(new DBParamEntry("ring_ratio_a", ring_ratio.a));
                ps.Add(new DBParamEntry("ring_ratio_b", ring_ratio.b));
                ps.Add(new DBParamEntry("ring_ratio_c", ring_ratio.c));
                ps.Add(new DBParamEntry("ring_ratio_d", ring_ratio.d));

                ps.Add(new DBParamEntry("doubles", doubles));
                ps.Add(new DBParamEntry("dcl_mass", dcl_mass));

            }
        }

        public static readonly ReadOnlyCollection<string> CollarTypeStrings = new ReadOnlyCollection<string>
            (new List<String> {
         "AmLi Thermal (no Cd)", "AmLi Fast (Cd)", "Cf Thermal (no Cd)", "Cf Fast (noCd)" });
        public enum MultChoice
        {
            CONVENTIONAL_MULT, // 1st mult radio button
            MULT_SOLVE_EFFICIENCY, // 2nd mult radio button
            MULT_DUAL_ENERGY_MODEL, // 3rd mult radio button
            MULT_KNOWN_ALPHA,		// 4th mult radio button
            CONVENTIONAL_MULT_WEIGHTED // 5th mult radio button
        }
        public class multiplicity_rec : INCCMethodDescriptor
        {
            public MultChoice solve_efficiency;
            public double sf_rate;
            public double vs1;
            public double vs2;
            public double vs3;
            public double vi1;
            public double vi2;
            public double vi3;
            public double a;
            public double b;
            public double c;
            public double sigma_x;
            public double alpha_weight;
            public double multEffCorFactor;

            public multiplicity_rec()
            {
                solve_efficiency = MultChoice.CONVENTIONAL_MULT;
                sf_rate = 473.5;
                vs1 = 2.154;
                vs2 = 3.789;
                vs3 = 5.211;
                vi1 = 3.163;
                vi2 = 8.24;
                vi3 = 17.321;
                a = 1.0;
                b = c = 0;
                sigma_x = 0.0;
                alpha_weight = 1.0;
                multEffCorFactor = 1.0;
            }

            public multiplicity_rec(multiplicity_rec src)
            {
                sf_rate = src.sf_rate;
                vs1 = src.vs1;
                vs2 = src.vs2;
                vs3 = src.vs3;
                vi1 = src.vi1;
                vi2 = src.vi2;
                vi3 = src.vi3;
                a = src.a;
                b = src.b;
                c = src.c;
                sigma_x = src.sigma_x;
                alpha_weight = src.alpha_weight;
                solve_efficiency = src.solve_efficiency;
                multEffCorFactor = src.multEffCorFactor;
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                multiplicity_rec tgt = (multiplicity_rec)imd;
                tgt.sf_rate = sf_rate;
                tgt.vs1 = vs1;
                tgt.vs2 = vs2;
                tgt.vs3 = vs3;
                tgt.vi1 = vi1;
                tgt.vi2 = vi2;
                tgt.vi3 = vi3;

                tgt.a = a;
                tgt.b = b;
                tgt.c = c;
                tgt.sigma_x = sigma_x;
                tgt.alpha_weight = alpha_weight;
                tgt.multEffCorFactor = multEffCorFactor;
                tgt.solve_efficiency = solve_efficiency;
                tgt.modified = true;
            }

            // dev note: needs 1 value (efficiency) from the sr
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Passive multiplicity calibration parameters");
                sec.AddTwo("Spontaneous fission rate:", sf_rate);
                sec.AddTwo("1st factorial moment spontaneous fission:", vs1);
                sec.AddTwo("2nd factorial moment spontaneous fission:", vs2);
                sec.AddTwo("3rd factorial moment spontaneous fission:", vs3);
                sec.AddTwo("1st factorial moment induced fission:", vi1);
                sec.AddTwo("2nd factorial moment induced fission:", vi2);
                sec.AddTwo("3rd factorial moment induced fission:", vi3);
                sec.AddTwo("a:", a);
                sec.AddTwo("b:", b);
                sec.AddTwo("c:", c);
                sec.AddTwo("sigma x:", sigma_x);
                sec.AddTwo("alpha weight:", alpha_weight);
                sec.AddTwo("efficiency correction factor:", ReportedECF(multEffCorFactor)); //  should be: FactoredEfficiency(mkey.sr.efficiency, multi.multEffCorFactor);

                // todo: weighted and dual-energy
                return sec;
            }


            public double ReportedECF(double effcf)
            {
                if (effcf == 0.0)
                    return 1.0;
                else
                    return effcf;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "multiplicity_rec";

                ps.Add(new DBParamEntry("solve_efficiency", (int)solve_efficiency));
                ps.Add(new DBParamEntry("sf_rate", sf_rate));
                ps.Add(new DBParamEntry("a", a));
                ps.Add(new DBParamEntry("b", b));
                ps.Add(new DBParamEntry("c", c));
                ps.Add(new DBParamEntry("vs1", vs1));
                ps.Add(new DBParamEntry("vs2", vs2));
                ps.Add(new DBParamEntry("vs3", vs3));
                ps.Add(new DBParamEntry("vi1", vi1));
                ps.Add(new DBParamEntry("vi2", vi2));
                ps.Add(new DBParamEntry("vi3", vi3));
                ps.Add(new DBParamEntry("sigma_x", sigma_x));
                ps.Add(new DBParamEntry("alpha_weight", alpha_weight));
                ps.Add(new DBParamEntry("eff_cor", ReportedECF(multEffCorFactor)));
            }
        }

        public class truncated_mult_rec : INCCMethodDescriptor
        {
            public double a;
            public double b;
            public bool known_eff; // solve for known eff
            public bool solve_eff; // Solve for efficiency 
            public truncated_mult_rec()
            {

            }
            public truncated_mult_rec(truncated_mult_rec src)
            {
                a = src.a;
                b = src.b;
                known_eff = src.known_eff;
                solve_eff = src.solve_eff;
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                truncated_mult_rec tgt = (truncated_mult_rec)imd;
                tgt.a = a;
                tgt.b = b;                
                tgt.known_eff = known_eff;
                tgt.solve_eff = solve_eff;
                tgt.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Truncated multiplicity calibration parameters");
                sec.AddTwo("a:", a);
                sec.AddTwo("b:", b);
                sec.AddTwo("Solve for known efficiency:", known_eff.ToString());
                sec.AddTwo("Solve for efficiency:", solve_eff.ToString());
                return sec;
            }

             public override void GenParamList()
            {
                base.GenParamList();
                Table = "truncated_mult_rec";
                ps.Add(new DBParamEntry("a", a));
                ps.Add(new DBParamEntry("b", b)); ;
                ps.Add(new DBParamEntry("known_eff", known_eff)); ;
                ps.Add(new DBParamEntry("solve_eff", solve_eff)); ;
            }
        };

        public class known_m_rec : INCCMethodDescriptor
        {
            public double sf_rate;
            public double vs1;
            public double vs2;
            public double vi1;
            public double vi2;
            public double b;
            public double c;
            public double sigma_x;
            public double lower_mass_limit;
            public double upper_mass_limit;
            public known_m_rec()
            {
                sf_rate = 473.5;
                vs1 = 2.154;
                vs2 = 3.789;

                vi1 = 3.163;
                vi2 = 8.24;
                b = c = 0;
                sigma_x = 0.0;

                upper_mass_limit = 1e8;
                lower_mass_limit = -1e8;        
            }
            public known_m_rec(known_m_rec  src)
            {
                if (src == null)
                    src = new known_m_rec();  // return; // or could return here

                sf_rate = src.sf_rate;
                vs1 = src.vs1;
                vs2 = src.vs2;
                vi1 = src.vi1;
                vi2 = src.vi2;

                b = src.b;
                c = src.c;
                sigma_x = src.sigma_x;
                upper_mass_limit = src.upper_mass_limit;
                lower_mass_limit = src.lower_mass_limit;
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                known_m_rec tgt = (known_m_rec)imd;
                tgt.sf_rate = sf_rate;
                tgt.vs1 = vs1;
                tgt.vs2 = vs2;
                tgt.vi1 = vi1;
                tgt.vi2 = vi2;

                tgt.b = b;
                tgt.c = c;
                tgt.sigma_x = sigma_x;
                tgt.upper_mass_limit = upper_mass_limit;
                tgt.lower_mass_limit = lower_mass_limit;
                tgt.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Known M calibration parameters");
                sec.AddTwo("Spontaneous fission rate:", sf_rate);
                sec.AddTwo("1st factorial moment spontaneous fission:", vs1);
                sec.AddTwo("2nd factorial moment spontaneous fission:", vs2);
                sec.AddTwo("1st factorial moment induced fission:", vi1);
                sec.AddTwo("2nd factorial moment induced fission:", vi2);
                sec.AddTwo("b:", b);
                sec.AddTwo("c:", c);
                sec.AddTwo("sigma x:", sigma_x);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "known_m_rec";
                ps.Add(new DBParamEntry("sf_rate", sf_rate));
                ps.Add(new DBParamEntry("b", b));
                ps.Add(new DBParamEntry("c", c));
                ps.Add(new DBParamEntry("vs1", vs1));
                ps.Add(new DBParamEntry("vs2", vs2));

                ps.Add(new DBParamEntry("vi1", vi1));
                ps.Add(new DBParamEntry("vi2", vi2));
                ps.Add(new DBParamEntry("sigma_x", sigma_x));
                ps.Add(new DBParamEntry("upper_mass_limit", upper_mass_limit));
                ps.Add(new DBParamEntry("lower_mass_limit", lower_mass_limit));
            }
        }

        public class cal_curve_rec : INCCMethodDescriptor
        {
            public double heavy_metal_corr_factor;
            public double heavy_metal_reference;
            public double percent_u235;
            public double[] dcl_mass;
            public double[] doubles;
            public CurveEquationVals cev;
            public CalCurveType CalCurveType;

            public cal_curve_rec()
            {
                CalCurveType = CalCurveType.STD;
                cev = new CurveEquationVals();
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
            }

            public cal_curve_rec(cal_curve_rec src)
            {
                if (src == null) return;
                heavy_metal_corr_factor = src.heavy_metal_corr_factor;
                heavy_metal_reference = src.heavy_metal_reference;
                percent_u235 = src.percent_u235;
                CalCurveType = src.CalCurveType;
                cev = new CurveEquationVals(src.cev);
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                Array.Copy(src.dcl_mass, dcl_mass, dcl_mass.Length);
                Array.Copy(src.doubles, doubles, doubles.Length);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                cal_curve_rec tgt = (cal_curve_rec)imd;
                cev.CopyTo(tgt.cev);
                dcl_mass.CopyTo(tgt.dcl_mass, 0);
                doubles.CopyTo(tgt.doubles, 0);
                tgt.heavy_metal_corr_factor = heavy_metal_corr_factor;
                tgt.heavy_metal_reference = heavy_metal_reference;
                tgt.percent_u235 = percent_u235;
                tgt.CalCurveType = CalCurveType;
                imd.modified = true;
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Passive calibration curve calibration parameters");
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);
                if (CalCurveType == CalCurveType.HM)
                {
                    sec.AddTwo("Heavy metal correction factor:", heavy_metal_corr_factor);
                    sec.AddTwo("Heavy metal reference:", heavy_metal_reference);
                }
                if (CalCurveType == CalCurveType.U)
                {
                    sec.AddTwo("% U235:", percent_u235);
                }
                sec.AddTwo("", string.Format("Analysis based on {0} rates.", cev.useSingles ? "singles" : "doubles"));

                return sec;
            }


            public override void GenParamList()
            {
                base.GenParamList();
                Table = "cal_curve_rec";
                cev.GenParamList();
                ps.AddRange(cev.ps);

                ps.Add(new DBParamEntry("percent_u235", percent_u235));
                ps.Add(new DBParamEntry("heavy_metal_reference", heavy_metal_reference));
                ps.Add(new DBParamEntry("heavy_metal_corr_factor", heavy_metal_corr_factor));
                ps.Add(new DBParamEntry("cal_curve_type",(int)CalCurveType));
                ps.Add(new DBParamEntry("doubles", doubles));
                ps.Add(new DBParamEntry("dcl_mass", dcl_mass));

            }

        }

        public class cm_pu_ratio_rec : INCCMethodDescriptor
        {
            public Tuple cm_pu_ratio;
            public double pu_half_life;
            public DateTime cm_pu_ratio_date;
            public Tuple cm_u_ratio;
            public DateTime cm_u_ratio_date;
            public string cm_id_label;
            public string cm_id;
            public string cm_input_batch_id;
            public double cm_dcl_u_mass;
            public double cm_dcl_u235_mass;

            public cm_pu_ratio_rec()
            {
                cm_pu_ratio = new Tuple(1,0);
                cm_pu_ratio_date = new DateTime(2014, 01, 01);
                cm_u_ratio = new Tuple(1,0);
                cm_u_ratio_date = new DateTime(2014, 01, 01);
                cm_id_label = string.Empty; cm_id = string.Empty; cm_input_batch_id = string.Empty;
                pu_half_life = (Isotopics.PU240HL / Isotopics.DAYS_PER_YEAR);
            }

            public cm_pu_ratio_rec(cm_pu_ratio_rec src)
            {
                cm_pu_ratio = new Tuple(src.cm_pu_ratio);
                pu_half_life = src.pu_half_life;
                cm_pu_ratio_date = new DateTime(src.cm_pu_ratio_date.Ticks);
                cm_u_ratio = new Tuple(src.cm_u_ratio);
                cm_u_ratio_date = new DateTime(src.cm_u_ratio_date.Ticks);
                cm_id_label = string.Copy(src.cm_id_label);
                cm_id = string.Copy(src.cm_id);
                cm_input_batch_id = string.Copy(src.cm_input_batch_id);
                cm_id_label = string.Copy(src.cm_id_label);
                cm_dcl_u_mass = src.cm_dcl_u_mass;
                cm_dcl_u235_mass = src.cm_dcl_u235_mass;
            }
            public override void CopyTo(INCCMethodDescriptor imd)
            {
                cm_pu_ratio_rec tgt = (cm_pu_ratio_rec)imd;
                tgt.cm_pu_ratio = new Tuple(cm_pu_ratio);
                tgt.pu_half_life = pu_half_life;
                tgt.cm_pu_ratio_date = new DateTime(cm_pu_ratio_date.Ticks);
                tgt.cm_u_ratio = new Tuple(cm_u_ratio);
                tgt.cm_u_ratio_date = new DateTime(cm_u_ratio_date.Ticks);
                tgt.cm_id_label = string.Copy(cm_id_label);
                tgt.cm_id = string.Copy(cm_id);
                tgt.cm_input_batch_id = string.Copy(cm_input_batch_id);
                tgt.cm_id_label = string.Copy(cm_id_label);
                tgt.cm_dcl_u_mass = cm_dcl_u_mass;
                tgt.cm_dcl_u235_mass = cm_dcl_u235_mass;

                imd.modified = true;
            }


            int Compare(cm_pu_ratio_rec x, cm_pu_ratio_rec y)
            {
                int res = string.Compare(x.cm_input_batch_id, y.cm_input_batch_id, true);
                if (res == 0)
                    res = string.Compare(x.cm_id, y.cm_id, true);
                if (res == 0)
                    res = string.Compare(x.cm_id_label, y.cm_id_label, true);
                if (res == 0)
                    res = (x.cm_pu_ratio_date.CompareTo(y.cm_pu_ratio_date));
                if (res == 0)
                    res = (x.cm_u_ratio_date.CompareTo(y.cm_u_ratio_date));

                if (res == 0)
                    res = (x.pu_half_life.CompareTo(y.pu_half_life));
                if (res == 0)
                    res = (x.cm_dcl_u235_mass.CompareTo(y.cm_dcl_u235_mass));
                if (res == 0)
                    res = (x.cm_dcl_u_mass.CompareTo(y.cm_dcl_u_mass));
                if (res == 0)
                    res = (x.cm_u_ratio.CompareTo(y.cm_u_ratio));
                if (res == 0)
                    res = (x.cm_pu_ratio.CompareTo(y.cm_pu_ratio));
                return res;
            }

            public int CompareTo(object other)
            {
                return Compare(this, (cm_pu_ratio_rec)other);
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "cm_pu_ratio_rec";
                ps.AddRange(TuplePair(cm_pu_ratio, "cm_pu_ratio"));
                ps.Add(new DBParamEntry("pu_half_life", pu_half_life));
                ps.Add(new DBParamEntry("cm_pu_ratio_date", cm_pu_ratio_date));
                ps.AddRange(TuplePair(cm_u_ratio, "cm_u_ratio"));
                ps.Add(new DBParamEntry("cm_u_ratio_date", cm_u_ratio_date));
                ps.Add(new DBParamEntry("cm_id_label", cm_id_label));
                ps.Add(new DBParamEntry("cm_id", cm_id));
                ps.Add(new DBParamEntry("cm_input_batch_id", cm_input_batch_id));
                ps.Add(new DBParamEntry("dcl_u_mass", cm_dcl_u_mass));
                ps.Add(new DBParamEntry("dcl_u235_mass", cm_dcl_u235_mass));
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.Standard);
                sec.AddTwo(cm_id_label, cm_id);
                sec.AddTwo("Input batch id:", cm_input_batch_id);
                sec.AddDateTimeRow("Cm/Pu ratio date:", cm_pu_ratio_date);
                sec.AddNumericRow("Cm/Pu ratio:", cm_pu_ratio);
                sec.AddDateTimeRow("Cm/U ratio date:", cm_u_ratio_date);
                sec.AddNumericRow("Cm/U ratio:", cm_u_ratio);
                return sec;
            }

        }
        public enum CuriumRatioVariant { UseSingles, UseDoubles, UseAddASourceDoubles }
        public class curium_ratio_rec : INCCMethodDescriptor
        {
            public CurveEquationVals cev;
            public CuriumRatioVariant curium_ratio_type; 

            public curium_ratio_rec()
            {
                cev = new CurveEquationVals();
            }

            public curium_ratio_rec(curium_ratio_rec src)
            {
                cev = new CurveEquationVals(src.cev);
                curium_ratio_type = src.curium_ratio_type;
            }
            public override void CopyTo(INCCMethodDescriptor imd)
            {
                curium_ratio_rec tgt = (curium_ratio_rec)imd;
                cev.CopyTo(tgt.cev);
                tgt.curium_ratio_type = curium_ratio_type;
                imd.modified = true;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "curium_ratio_rec";
                cev.GenParamList();
                ps.AddRange(cev.ps);
                ps.Add(new DBParamEntry("curium_ratio_type", (int)curium_ratio_type));

            }
            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Curium Ratio parameters");
                string s = string.Empty;
                switch (curium_ratio_type)
                {
                    case (CuriumRatioVariant.UseSingles):
                        s = "Using Singles";
                        break;
                    case (CuriumRatioVariant.UseDoubles):
                        s = "Using Doubles";
                        break;
                    case (CuriumRatioVariant.UseAddASourceDoubles):
                        s = "Using Add-a-Source Doubles";
                        break;
            	}

                sec.AddTwo("Curium Ratio Variant:", s); 
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);

                return sec;
        	}
        }
        public const int MAX_DUAL_ENERGY_ROWS = 25;
        public class de_mult_rec : INCCMethodDescriptor
        {
            public double[] neutron_energy;
            public double[] detector_efficiency;
            public double[] inner_outer_ring_ratio;
            public double[] relative_fission;
            public double inner_ring_efficiency;
            public double outer_ring_efficiency;

            public de_mult_rec()
            {
                neutron_energy = new double[MAX_DUAL_ENERGY_ROWS];
                detector_efficiency = new double[MAX_DUAL_ENERGY_ROWS];
                inner_outer_ring_ratio = new double[MAX_DUAL_ENERGY_ROWS];
                relative_fission = new double[MAX_DUAL_ENERGY_ROWS];
            }

            public de_mult_rec(de_mult_rec src)
            {
                Array.Copy(src.neutron_energy, neutron_energy, src.neutron_energy.Length);
                Array.Copy(src.detector_efficiency, detector_efficiency, src.detector_efficiency.Length);
                Array.Copy(src.inner_outer_ring_ratio, inner_outer_ring_ratio, src.inner_outer_ring_ratio.Length);
                Array.Copy(src.relative_fission, relative_fission, src.relative_fission.Length);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                de_mult_rec tgt = (de_mult_rec)imd;
                Array.Copy(neutron_energy, tgt.neutron_energy, neutron_energy.Length);
                Array.Copy(detector_efficiency, tgt.detector_efficiency, detector_efficiency.Length);
                Array.Copy(inner_outer_ring_ratio, tgt.inner_outer_ring_ratio, inner_outer_ring_ratio.Length);
                Array.Copy(relative_fission, tgt.relative_fission, relative_fission.Length);
                imd.modified = true;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "de_mult_rec";
                ps.Add(new DBParamEntry("neutron_energy", neutron_energy));
                ps.Add(new DBParamEntry("detector_efficiency", detector_efficiency));
                ps.Add(new DBParamEntry("inner_outer_ring_ratio", inner_outer_ring_ratio));
                ps.Add(new DBParamEntry("relative_fission", relative_fission));
                ps.Add(new DBParamEntry("inner_ring_efficiency", inner_ring_efficiency));
                ps.Add(new DBParamEntry("outer_ring_efficiency", outer_ring_efficiency));
            }

            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Dual energy calibration parameters"); // todo: complete this
                return sec;
            }
            
        };

        public const int MAX_ADDASRC_POSITIONS = 5;
        public class add_a_source_rec : INCCMethodDescriptor
        {
            public double dzero_avg;
            public double[] position_dzero;
            public ushort num_runs;
            public CurveEquationVals cf; // a-d only
            public double tm_weighting_factor;
            public double tm_dbls_rate_upper_limit;
            public bool use_truncated_mult;
            public DateTime dzero_ref_date;
            public CurveEquationVals cev; // all vals used for this one
            public double[] dcl_mass;
            public double[] doubles;

            
            public add_a_source_rec()
            {
                cev = new CurveEquationVals();
                cf = new CurveEquationVals();
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                position_dzero = new double[MAX_ADDASRC_POSITIONS];
            }

            public add_a_source_rec(add_a_source_rec src)
            {
                cev = new CurveEquationVals(src.cev);
                cf = new CurveEquationVals(src.cf);
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                position_dzero = new double[MAX_ADDASRC_POSITIONS];
                Array.Copy(src.dcl_mass, dcl_mass, dcl_mass.Length);
                Array.Copy(src.doubles, doubles, dcl_mass.Length);
                Array.Copy(src.position_dzero, position_dzero, position_dzero.Length); 
                num_runs = src.num_runs;
                dzero_avg = src.dzero_avg;
                tm_weighting_factor = src.tm_weighting_factor;
                use_truncated_mult = src.use_truncated_mult;
                tm_dbls_rate_upper_limit = src.tm_dbls_rate_upper_limit;
                dzero_ref_date = new DateTime(src.dzero_ref_date.Ticks);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                add_a_source_rec tgt = (add_a_source_rec)imd;
                cev.CopyTo(tgt.cev);
                cf.CopyTo(tgt.cf);
                dcl_mass.CopyTo(tgt.dcl_mass, 0);
                doubles.CopyTo(tgt.doubles, 0);
                position_dzero.CopyTo(tgt.position_dzero, 0);
                tgt.num_runs = num_runs;
                tgt.dzero_avg = dzero_avg;
                tgt.tm_weighting_factor = tm_weighting_factor;
                tgt.use_truncated_mult = use_truncated_mult;
                tgt.dzero_ref_date = new DateTime(dzero_ref_date.Ticks);
                imd.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Add-a-source calibration parameters");
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);
                sec.AddTwo("Number of add-a-source cycles:", this.num_runs);
                sec.AddTwo("cf a:", cf.a);
                sec.AddTwo("cf b:", cf.b);
                sec.AddTwo("cf c:", cf.c);
                sec.AddTwo("cf d:", cf.d);
                sec.AddDateTimeRow("R0 reference date: ", dzero_ref_date);
                sec.AddTwo("R0 average:", dzero_avg);
                sec.AddTwo("R0 position 1:", position_dzero[0]);
                sec.AddTwo("R0 position 2:", position_dzero[1]);
                sec.AddTwo("R0 position 3:", position_dzero[2]);
                sec.AddTwo("R0 position 4:", position_dzero[3]);
                sec.AddTwo("R0 position 5:", position_dzero[4]);

                if (use_truncated_mult)
                {
                    sec.AddTwo("Truncated mult weighting factor:", tm_weighting_factor);
                    sec.AddTwo("Truncated mult dbls rt upper lim:", tm_dbls_rate_upper_limit);
                }
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "add_a_source_rec";

                cev.GenParamList();
                ps.AddRange(cev.ps);

                ps.Add(new DBParamEntry("dzero_avg", dzero_avg));
                ps.Add(new DBParamEntry("dzero_ref_date", dzero_ref_date));
                ps.Add(new DBParamEntry("use_truncated_mult", use_truncated_mult));
                ps.Add(new DBParamEntry("tm_weighting_factor", tm_weighting_factor));
                ps.Add(new DBParamEntry("tm_dbls_rate_upper_limit", tm_dbls_rate_upper_limit));

                ps.Add(new DBParamEntry("cf_a", cf.a));
                ps.Add(new DBParamEntry("cf_b", cf.b));
                ps.Add(new DBParamEntry("cf_c", cf.c));
                ps.Add(new DBParamEntry("cf_d", cf.d));

                ps.Add(new DBParamEntry("doubles", doubles));
                ps.Add(new DBParamEntry("dcl_mass", dcl_mass));
                ps.Add(new DBParamEntry("position_dzero", position_dzero));

            }
        }


        public class active_passive_rec : INCCMethodDescriptor
        {
            public CurveEquationVals cev;
            public active_passive_rec()
            {
                cev = new CurveEquationVals();
            }

            public active_passive_rec(active_passive_rec src)
            {
                if (src == null)
                    cev = new CurveEquationVals();
                else
                    cev = new CurveEquationVals(src.cev);
            }

            public override void CopyTo(INCCMethodDescriptor imd) 
            {
                active_passive_rec tgt = (active_passive_rec)imd;
                cev.CopyTo(tgt.cev);
                imd.modified = true;
            }

            // todo: check for completeness with INCC5 output
            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Active/Passive calibration curve calibration parameters");
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "active_passive_rec";
                cev.GenParamList();
                ps.AddRange(cev.ps);
            }


        }

        public class active_rec : INCCMethodDescriptor
        {
            public double[] dcl_mass;
            public double[] doubles;
            public CurveEquationVals cev;
            public active_rec()
            {
                cev = new CurveEquationVals();
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
            }

            public active_rec(active_rec src)
            {
                dcl_mass = new double[MAX_NUM_CALIB_PTS];
                doubles = new double[MAX_NUM_CALIB_PTS];
                if (src == null)
                {
                    cev = new CurveEquationVals();
                    return;
                }
                else
                    cev = new CurveEquationVals(src.cev);
                Array.Copy(src.dcl_mass, dcl_mass, dcl_mass.Length);
                Array.Copy(src.doubles, doubles, doubles.Length);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                active_rec tgt = (active_rec)imd;
                cev.CopyTo(tgt.cev);
                dcl_mass.CopyTo(tgt.dcl_mass, 0);
                doubles.CopyTo(tgt.doubles, 0);
                imd.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Active calibration curve calibration parameters");
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "active_rec";
                cev.GenParamList();
                ps.AddRange(cev.ps);
                ps.Add(new DBParamEntry("doubles", doubles));
                ps.Add(new DBParamEntry("dcl_mass", dcl_mass));

            }

        }

        public class active_mult_rec : INCCMethodDescriptor
        {
            public double vt1;
            public double vt2;
            public double vt3;
            public double vf1;
            public double vf2;
            public double vf3;
            public active_mult_rec()
            {
                vt1 = 2.414; 
                vt2 = 4.638; 
                vt3 = 6.818; 
                vf1 = 2.637; 
                vf2 = 5.623; 
                vf3 = 9.476; 
            }

            public active_mult_rec(active_mult_rec src)
            {
                vt1 = src.vt1; vt2 = src.vt2; vt3 = src.vt3;
                vf1 = src.vf1; vf2 = src.vf2; vf3 = src.vf3;
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                active_mult_rec tgt = (active_mult_rec)imd;
                tgt.vt1 = vt1; tgt.vt2 = vt2; tgt.vt3 = vt3;
                tgt.vf1 = vf1; tgt.vf2 = vf2; tgt.vf3 = vf3;
                imd.modified = true;
            }

             // todo: check for INCC5 output completeness
            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddTwo("1st Factorial Moment Thermal Induced Fission",vt1); 
                sec.AddTwo("2nd Factorial Moment Thermal Induced Fission",vt2);
                sec.AddTwo("3rd Factorial Moment Thermal Induced Fission",vt3);
                sec.AddTwo("1st Factorial Moment Fast Induced Fission",vf1);
                sec.AddTwo("2nd Factorial Moment Fast Induced Fission",vf2);
                sec.AddTwo("3rd Factorial Moment Fast Induced Fission",vf3);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "active_mult_rec";
                ps.Add(new DBParamEntry("vt1", vt1));
                ps.Add(new DBParamEntry("vt2", vt2));
                ps.Add(new DBParamEntry("vt3", vt3));
                ps.Add(new DBParamEntry("vf1", vf1));
                ps.Add(new DBParamEntry("vf2", vf2));
                ps.Add(new DBParamEntry("vf3", vf3));
            }
        }

        public class collar_detector_rec : INCCMethodDescriptor
        {
            //Enum CollarType { AmLiThermal = 0, AmLiFast = 1, CfThermal = 2, CfFast = 3 }
            public int collar_mode;
            public DateTime reference_date;
            public double relative_doubles_rate;

            public collar_detector_rec()
            {
                collar_mode = (int)CollarType.AmLiThermal;
                relative_doubles_rate = 1;
                reference_date = new DateTime (1989,10,17);
            }

            public collar_detector_rec(collar_detector_rec src)
            {
                collar_mode = src.collar_mode;
                relative_doubles_rate = src.relative_doubles_rate;
                reference_date = new DateTime(src.reference_date.Ticks);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                collar_detector_rec tgt = (collar_detector_rec)imd;
                tgt.collar_mode = collar_mode;
                tgt.relative_doubles_rate = relative_doubles_rate;
                tgt.reference_date = new DateTime(reference_date.Ticks);
                imd.modified = true;
            }

            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Fixed); // uses E
                sec.AddTwo("Collar Mode", CollarTypeStrings[collar_mode]);
                sec.AddTwo("Reference Doubles Rate", relative_doubles_rate);
                sec.AddTwo("Reference Date", reference_date.ToShortDateString());
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "collar_detector_rec";
                ps.Add(new DBParamEntry("collar_detector_mode", collar_mode));
                ps.Add(new DBParamEntry("reference_date", reference_date));
                ps.Add(new DBParamEntry("relative_doubles_rate", relative_doubles_rate));
            }

        }

        public const int MAX_POISON_ROD_TYPES = 10; /*max # different types poison rods */
        public class collar_rec : INCCMethodDescriptor
        {
            //enum CollarType { AmLiThermal = 0, AmLiFast = 1, CfThermal = 2, CfFast = 3 }
            public int collar_mode;
            public double[] poison_absorption_fact;
            public Tuple[] poison_rod_a;
            public Tuple[] poison_rod_b;
            public Tuple[] poison_rod_c;
            public string[] poison_rod_type;
            public Tuple u_mass_corr_fact_a;
            public Tuple u_mass_corr_fact_b;
            public Tuple sample_corr_fact;
            public int number_calib_rods;

            public CurveEquationVals cev;
            public collar_rec()
            {
                cev = new CurveEquationVals();
                //The collar equation needs a nonzero default for a.
                cev.a = 1;
                cev.modified = true;
                poison_absorption_fact = new double[MAX_POISON_ROD_TYPES];
                poison_absorption_fact[0] = 0.647;
                poison_rod_a = new Tuple[MAX_POISON_ROD_TYPES];
                poison_rod_b = new Tuple[MAX_POISON_ROD_TYPES];
                poison_rod_c = new Tuple[MAX_POISON_ROD_TYPES];
                poison_rod_type = new string[MAX_POISON_ROD_TYPES];
                poison_rod_type[0] = "G";
                for (int i = 0; i < MAX_POISON_ROD_TYPES; i++)
                {
                    poison_rod_a[i]= new Tuple (0.0,0.0);
                    poison_rod_b[i] = new Tuple(0.0,0.0);
                    poison_rod_c[i] = new Tuple(0.0, 0.0);
                }
                number_calib_rods = 2;
                u_mass_corr_fact_a = new Tuple(.000724, .000012);
                u_mass_corr_fact_b = new Tuple(453, 4.53);
                sample_corr_fact = new Tuple();
            }

            public collar_rec(collar_rec src)
            {
                cev = new CurveEquationVals(src.cev);
                number_calib_rods = src.number_calib_rods;
                collar_mode = src.collar_mode;
                poison_absorption_fact = new double[MAX_POISON_ROD_TYPES];
                poison_rod_a = Tuple.Copy(src.poison_rod_a);
                poison_rod_b = Tuple.Copy(src.poison_rod_b);
                poison_rod_c = Tuple.Copy(src.poison_rod_c);
                poison_rod_type = new string[MAX_POISON_ROD_TYPES];
				for (int i = 0; i < MAX_POISON_ROD_TYPES; i++)
                {
					if (!string.IsNullOrEmpty(src.poison_rod_type[i]))
						poison_rod_type[i] = string.Copy(src.poison_rod_type[i]);
				}
                u_mass_corr_fact_a = new Tuple(src.u_mass_corr_fact_a);
                u_mass_corr_fact_b = new Tuple(src.u_mass_corr_fact_b);
                sample_corr_fact = new Tuple(src.sample_corr_fact);
                Array.Copy(src.poison_absorption_fact, poison_absorption_fact, poison_absorption_fact.Length);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                collar_rec tgt = (collar_rec)imd;
                cev.CopyTo(tgt.cev);
                tgt.number_calib_rods = number_calib_rods;
                tgt.collar_mode = collar_mode;
                Array.Copy(poison_absorption_fact, tgt.poison_absorption_fact, poison_absorption_fact.Length);
                tgt.poison_rod_a = Tuple.Copy(poison_rod_a);
                tgt.poison_rod_b = Tuple.Copy(poison_rod_b);
                tgt.poison_rod_c = Tuple.Copy(poison_rod_c);
				for (int i = 0; i < MAX_POISON_ROD_TYPES; i++)
                {
					if (!string.IsNullOrEmpty(poison_rod_type[i]))
						tgt.poison_rod_type[i] = string.Copy(poison_rod_type[i]);
				}
                tgt.u_mass_corr_fact_a = new Tuple(u_mass_corr_fact_a);
                tgt.u_mass_corr_fact_b = new Tuple(u_mass_corr_fact_b);
                tgt.sample_corr_fact = new Tuple(sample_corr_fact);
                imd.modified = true;
            }

            // todo: implement INCC5 output
            public override List<Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Collar calibration curve calibration parameters");
                List<Row> cevlines = cev.ToLines(m);
                sec.AddRange(cevlines);
                return sec;
            }

            public override void GenParamList()
            {
                base.GenParamList();
                Table = "collar_rec";
                cev.GenParamList();
                ps.AddRange(cev.ps);
                ps.Add(new DBParamEntry("number_calib_rods", number_calib_rods));
                ps.Add(new DBParamEntry("collar_mode", collar_mode));
                ps.Add(new DBParamEntry("poison_absorption_fact", poison_absorption_fact));
                ps.AddRange(TupleArrayPair("poison_rod_a", poison_rod_a));
                ps.AddRange(TupleArrayPair("poison_rod_b", poison_rod_b));
                ps.AddRange(TupleArrayPair("poison_rod_c", poison_rod_c));
                ps.Add(new DBParamEntry("poison_rod_type", poison_rod_type));

                ps.AddRange(TuplePair("u_mass_corr_fact_a", u_mass_corr_fact_a));
                ps.AddRange(TuplePair("u_mass_corr_fact_b", u_mass_corr_fact_b));
                ps.AddRange(TuplePair("sample_corr_fact", sample_corr_fact));
            }
        }

        public const int MAX_COLLAR_K5_PARAMETERS = 20; /*max number collar K = 5 parameters */
        public class collar_k5_rec : INCCMethodDescriptor
        {
            public int k5_mode;
            public bool[] k5_checkbox;
            public Tuple[] k5;
            public string k5_item_type;
            public string[] k5_label;

            public collar_k5_rec()
            {
                k5_checkbox = new bool[MAX_COLLAR_K5_PARAMETERS];
                k5 = new Tuple[MAX_COLLAR_K5_PARAMETERS];
                k5_label = new string[MAX_COLLAR_K5_PARAMETERS];
                for (int j = 0; j < MAX_COLLAR_K5_PARAMETERS; j++)
                {
                    k5_checkbox[j]= false;
                    k5[j] = new Tuple(0, 0);
                    k5_label[j] = string.Empty;
                }
				k5_item_type = string.Empty;
            }

            public collar_k5_rec(collar_k5_rec src)
            {
                k5_mode = src.k5_mode;
                k5 = new Tuple[MAX_COLLAR_K5_PARAMETERS];
                k5_checkbox = new bool[MAX_COLLAR_K5_PARAMETERS];
                k5_label = new string[MAX_COLLAR_K5_PARAMETERS];
                k5 = Tuple.Copy(src.k5);
                Array.Copy(src.k5_checkbox, k5_checkbox, k5_checkbox.Length);
                Array.Copy(src.k5_label, k5_label, k5_label.Length); // todo: not a deep copy but does it matter?
                k5_item_type = string.Copy(src.k5_item_type);
            }

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                collar_k5_rec tgt = (collar_k5_rec)imd;
                tgt.k5_mode = k5_mode;
                tgt.k5_item_type = k5_item_type;
                Array.Copy (k5,tgt.k5, k5.Length);
                Array.Copy(k5_checkbox, tgt.k5_checkbox, k5_checkbox.Length);
                Array.Copy(k5_label, tgt.k5_label, k5_label.Length); // todo: not a deep copy but does it matter?
                imd.modified = true;
            }

            // todo: full collar implementation required
            public override List<NCCReporter.Row> ToLines(Measurement m)
            {
                INCCStyleSection sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                sec.SetFloatingPointFormat(INCCStyleSection.NStyle.Exponent); // uses E
                sec.AddHeader("Collar k5 calibration curve calibration parameters");
                return sec;
            }


            public override void GenParamList()
            {
                base.GenParamList();
                Table = "collar_k5_rec";

                ps.Add(new DBParamEntry("k5_mode", k5_mode));
                ps.Add(new DBParamEntry("k5_checkbox", k5_checkbox));
                ps.AddRange(TupleArrayPair("k5", k5));
                // devnote: "k5_item_type" now handled by relation back to relevant item type entry in DB ps.Add(new DBParamEntry("k5_item_type", k5_item_type));
                ps.Add(new DBParamEntry("k5_label", k5_label));
            }
        }

        public class collar_combined_rec : INCCMethodDescriptor
        {
            public collar_k5_rec k5;
            public collar_rec collar;
            public collar_detector_rec collar_det;
            private int mode;
            
            public collar_combined_rec(int collar_mode = -1)
            {
                mode = collar_mode;
				Init();
            }
            public collar_combined_rec(collar_combined_rec src, int collar_mode = -1)
            {
                mode = collar_mode;
                Init();
				src.collar.CopyTo(collar);
                src.collar_det.CopyTo(collar_det);
                src.k5.CopyTo(k5);
            }

            public string GetCollarModeString ()
            {
                return CollarTypeStrings[collar.collar_mode];
            }
            //This method is never used..... hmmmm.
            public collar_combined_rec(collar_detector_rec det, collar_rec col, collar_k5_rec k5e)
            {
                collar = col; // shallow copies, ok?
                collar_det = det;
                k5 = k5e;
                if (mode != -1)
                {
                    //Set in Analysis Parms selection....
                    collar.collar_mode = mode;
                    collar.modified = true;
                    collar_det.collar_mode = mode;
                    collar_det.modified = true;
                    k5.k5_mode = mode;
                    k5.modified = true;
                }
				Pump = 0; // prep for use
            }

			public void Init()
			{
                collar_det = new collar_detector_rec();
                collar = new collar_rec();
                k5 = new collar_k5_rec();
                if (mode != -1)
                {
                    //Set in Analysis Parms selection....
                    collar.collar_mode = mode;
                    collar.modified = true;
                    collar_det.collar_mode = mode;
                    collar_det.modified = true;
                    k5.k5_mode = mode;
                    k5.modified = true;
                }
                Pump = 0; // prep for use
			}

            public override void CopyTo(INCCMethodDescriptor imd)
            {
                collar_combined_rec tgt = (collar_combined_rec)imd;
				if (tgt == null)
					return;
                tgt.collar.CopyTo (collar);
                tgt.collar_det.CopyTo(collar_det);
                tgt.k5.CopyTo(k5);
                tgt.modified = true;
            }
            public override void GenParamList()  // this does not fill in the three data structures, see the pump processing below 
            {
                base.GenParamList();
                Table = "collar_combined_rec";  // not a database table name, only a contextual placeholder
            }

			public override DB.ElementList ToDBElementList(bool generate = true)
            {
				DB.ElementList el = null;
                if (Pump == 0)
				{
					Pump = 1;
					el = collar_det.ToDBElementList(generate);
					el.OptTable = collar_det.Table;
				} else
                if (Pump == 1)
				{
					Pump = 2;
					el = collar.ToDBElementList(generate);
					el.OptTable = collar.Table;
 				} else
				if (Pump == 2)
				{
					Pump = 0;
					el = k5.ToDBElementList(generate);
					el.OptTable = k5.Table;
				}
				else  // can never get here lol
				{
					Pump = 0;
					el = ToDBElementList(generate);
				}
				return el;
            }
        }
    }


    public static class CurveEquationDisplay
    {
        public static string ToDisplayString(this INCCAnalysisParams.CurveEquation ceq, bool useSingles = false)
        {
            string s = (useSingles ? "S " : "D ") ;
            switch (ceq)
            {
                case INCCAnalysisParams.CurveEquation.CUBIC:
                    s += "= a + bm + cm^2 + dm^3";
                    break;
                case INCCAnalysisParams.CurveEquation.POWER:
                    s += "= am^b";
                    break;
                case INCCAnalysisParams.CurveEquation.HOWARDS:
                    s += "= am / (1 + bm)";
                    break;
                case INCCAnalysisParams.CurveEquation.EXPONENTIAL:
                    s += "= a (1 - exp(bm))";
                    break;
            }
            return s;
        }
    }
}
