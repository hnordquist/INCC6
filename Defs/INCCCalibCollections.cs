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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NCCReporter;
namespace AnalysisDefs
{

    using NC = NCC.CentralizedState;


    public partial class INCCDB
    {

        INCCAnalysisMethodMap dmam;
        /// <summary>
        /// The map of INCCSelector(detectors, materials) ->  AnalysisMethods
        /// </summary>
        public INCCAnalysisMethodMap DetectorMaterialAnalysisMethods
        {
            get
            {
                if (dmam == null)
                {
                    DataTable dt = NC.App.Pest.GetACollection(DB.Pieces.AnalysisMethodSpecifiers);
                    dmam = new INCCAnalysisMethodMap();

                    // column 0 is mat name, 1 is det name, 2 is mat id, 3 is det id, 4 is first choice boolean
                    foreach (DataRow dr in dt.Rows)
                    {
                        INCCSelector sel = new INCCSelector((string)dr["detector_name"], (string)dr["name"]); //use the names, not ids, gotta look em up
                        AnalysisMethods ams = new AnalysisMethods(sel);
                        ams.choices[(int)AnalysisMethod.KnownA] = DB.Utils.DBBool(dr["known_alpha"]);
                        ams.choices[(int)AnalysisMethod.KnownM] = DB.Utils.DBBool(dr["known_m"]);
                        ams.choices[(int)AnalysisMethod.Multiplicity] = DB.Utils.DBBool(dr["multiplicity"]);
                        ams.choices[(int)AnalysisMethod.CalibrationCurve] = DB.Utils.DBBool(dr["cal_curve"]);
                        ams.choices[(int)AnalysisMethod.AddASource] = DB.Utils.DBBool(dr["add_a_source"]);
                        ams.choices[(int)AnalysisMethod.Active] = DB.Utils.DBBool(dr["active"]);
                        ams.choices[(int)AnalysisMethod.ActivePassive] = DB.Utils.DBBool(dr["active_passive"]);
                        ams.choices[(int)AnalysisMethod.ActiveMultiplicity] = DB.Utils.DBBool(dr["active_mult"]);
                        ams.choices[(int)AnalysisMethod.Collar] = DB.Utils.DBBool(dr["collar"]);
                        ams.choices[(int)AnalysisMethod.CuriumRatio] = DB.Utils.DBBool(dr["curium_ratio"]);
                        ams.choices[(int)AnalysisMethod.TruncatedMultiplicity] = DB.Utils.DBBool(dr["truncated_mult"]);
                        ams.Normal = (AnalysisMethod)DB.Utils.DBInt32(dr["normal_method"]);
                        ams.Backup = (AnalysisMethod)DB.Utils.DBInt32(dr["backup_method"]);
                        dmam.Add(sel, ams);
                        //if (ams.AnySelected()) ams.choices[(int)AnalysisMethod.None] = false;
                    }

                    using (DB.AnalysisMethodSpecifiers db = new DB.AnalysisMethodSpecifiers())
                    {
                        foreach (INCCSelector sel in dmam.Keys)
                        {
                            IngestAnalysisMethodSpecificsFromDB(sel, dmam[sel], db);
                        }
                    }

                }
                return dmam;
            }
        }

        public void UpdateAnalysisMethods()
        {
            DB.AnalysisMethodSpecifiers db = new DB.AnalysisMethodSpecifiers();
            foreach (KeyValuePair<INCCSelector, AnalysisMethods> kv in DetectorMaterialAnalysisMethods)
            {
                if (kv.Value.modified)
                {
                    UpdateAnalysisMethods(kv.Key, kv.Value, db);
                    UpdateAnalysisMethodSpecifics(kv.Key.detectorid, kv.Key.material, db);
                }
            }
        }

        public void UpdateAnalysisMethod(INCCSelector sel, AnalysisMethods ams)
        {
            DB.AnalysisMethodSpecifiers db = new DB.AnalysisMethodSpecifiers();

            if (ams.modified)
            {
                UpdateAnalysisMethods(sel, ams, db);
            }
            UpdateAnalysisMethodSpecifics(sel.detectorid, sel.material,db);
        }


        public void UpdateAnalysisMethods(Detector det, string mat)
        {
            DB.AnalysisMethodSpecifiers db = new DB.AnalysisMethodSpecifiers();
            AnalysisDefs.AnalysisMethods sam = null;  
            var res =   // this finds the am for the given detector and acquire type
                    from am in this.DetectorMaterialAnalysisMethods
                    where (am.Key.detectorid.Equals(det.Id.DetectorId,StringComparison.OrdinalIgnoreCase) 
                    && am.Key.material == mat)
                    select am;
            foreach (KeyValuePair<INCCSelector, AnalysisMethods> kv in res)
            {
                sam = kv.Value;
                if (sam.modified)
                    UpdateAnalysisMethods(kv.Key, sam, db);
            }
        }

        public void UpdateAnalysisMethods(INCCSelector sel, AnalysisMethods am, DB.AnalysisMethodSpecifiers db = null)
        {
            if (db == null)
                db = new DB.AnalysisMethodSpecifiers();
            DB.ElementList saParams;
            saParams = am.ToDBElementList();       
            if (!db.Update(sel.detectorid, sel.material, saParams)) // am not there, so add it
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34027, "Failed to update analysis method spec for " + sel.ToString());
            }
            else
            {
                NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34026, "Updated/created analysis method spec for " + sel.ToString());
            }
            am.modified = false;

        }

        /// the details
        public void UpdateAnalysisMethodSpecifics(string detname, string mat, DB.AnalysisMethodSpecifiers db = null)
        {
            if (db == null)
                db = new DB.AnalysisMethodSpecifiers();
            var res =   // this finds the am for the given detector and acquire type
                    from am in this.DetectorMaterialAnalysisMethods
                    where (am.Key.detectorid.Equals(detname, StringComparison.OrdinalIgnoreCase) && 
                           am.Key.material.Equals(mat, StringComparison.OrdinalIgnoreCase))
                    select am;
            if (res.Count() > 0)  // now execute the select expression and test the result for existence
            {
                KeyValuePair<INCCSelector, AnalysisMethods> kv = res.First();
                AnalysisMethods sam = kv.Value;  // the descriptor instance
                
                IEnumerator iter = kv.Value.GetMethodEnumerator();
                while (iter.MoveNext())
                {
                    System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor> md = (System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor>)iter.Current;
                    if (md.Item2 == null) // case from INCC5 transfer missing params, reflects file write bugs in INCC5 code
                    {
                        NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34029, "Missing {0}'s INCC {1} {2} method parameters, adding default values", detname, kv.Key.material, md.Item1.FullName());
                        //OK, there is probably smarter way of doing ths, but for now, does find the nulls, then add default params where necessary. hn 9.23.2015
                        if (md.Item2 == null)
                        {
                            INCCAnalysisParams.INCCMethodDescriptor rec = new INCCAnalysisParams.INCCMethodDescriptor();
                            switch (md.Item1)
                            {
                                case AnalysisMethod.Active:
                                    rec = new INCCAnalysisParams.active_rec();
                                    break;
                                case AnalysisMethod.ActiveMultiplicity:
                                    rec = new INCCAnalysisParams.active_mult_rec();
                                    break;
                                case AnalysisMethod.ActivePassive:
                                    rec = new INCCAnalysisParams.active_passive_rec();
                                    break;
                                case AnalysisMethod.AddASource:
                                    rec = new INCCAnalysisParams.add_a_source_rec();
                                    break;
                                case AnalysisMethod.CalibrationCurve:
                                    rec = new INCCAnalysisParams.cal_curve_rec();
                                    break;
                                case AnalysisMethod.Collar:
                                    rec = new INCCAnalysisParams.collar_combined_rec();
                                    break;
                                case AnalysisMethod.CuriumRatio:
                                    rec = new INCCAnalysisParams.curium_ratio_rec();
                                    break;
                                case AnalysisMethod.KnownA:
                                    rec = new INCCAnalysisParams.known_alpha_rec();
                                    break;
                                case AnalysisMethod.KnownM:
                                    rec = new INCCAnalysisParams.known_m_rec();
                                    break;
                                case AnalysisMethod.Multiplicity:
                                    rec = new INCCAnalysisParams.multiplicity_rec();
                                    break;
                                case AnalysisMethod.TruncatedMultiplicity:
                                    rec = new INCCAnalysisParams.truncated_mult_rec();
                                    break;
                                case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
                                    rec = new INCCAnalysisParams.de_mult_rec();
                                    break;
                                default:
                                    break;
                            }
                            sam.AddMethod(md.Item1, rec);
                        }
                        continue;
                    }

                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34030, "Updating {0},{1} {2}", detname, mat, md.Item2.GetType().Name);
                    DB.ElementList parms = null;
					bool bonk = false;
                    switch (md.Item1)
                    {
                        case AnalysisMethod.KnownA:
                        case AnalysisMethod.CalibrationCurve:
                        case AnalysisMethod.KnownM:
                        case AnalysisMethod.Multiplicity:
                        case AnalysisMethod.TruncatedMultiplicity:
                        case AnalysisMethod.AddASource:
                        case AnalysisMethod.CuriumRatio:
                        case AnalysisMethod.Active:
                        case AnalysisMethod.ActivePassive:
                        case AnalysisMethod.ActiveMultiplicity:
						case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
                            parms = (md.Item2).ToDBElementList();
							break;
                        case AnalysisMethod.Collar:  // bad mojo with the design break here
							parms = (md.Item2).ToDBElementList();
							db.UpdateCalib(detname, mat, parms.OptTable, parms);
							parms = (md.Item2).ToDBElementList();
							db.UpdateCalib(detname, mat, parms.OptTable, parms);
							parms = (md.Item2).ToDBElementList();
							db.UpdateCalib(detname, mat, parms.OptTable, parms);
							parms = null; bonk = false;  // skip the final processing step below
							break;
                        default:
							bonk = true;
                            break;
                    }
                    if (parms != null)
                        db.UpdateCalib(detname, mat, md.Item2.GetType().Name, parms);  // det, mat, amid, params
                    else if (bonk)
                    {
                        //Didn't exist, so create and store. hn 9.22.2015
                        sam.AddMethod(md.Item1, md.Item2);
                    }
                }
            }
        }


        /// <summary>
        /// Get specific parameter sets for the given detector, material type pair.
        /// Returns default values if database entry not found
        /// </summary>
        /// <param name="detname"></param>
        /// <param name="mat"></param>
        /// <param name="db"></param>
        public void IngestAnalysisMethodSpecificsFromDB(INCCSelector sel, AnalysisMethods ams, DB.AnalysisMethodSpecifiers db)
        {

            foreach (AnalysisMethod am in System.Enum.GetValues(typeof(AnalysisMethod)))
            {
                if (!ams.choices[(int)am])
                    continue;
                if (!(am > AnalysisMethod.None && am <= AnalysisMethod.TruncatedMultiplicity && (am != AnalysisMethod.INCCNone)))
                {
                    if (!am.IsNone())
                        NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34061, "Skipping DB ingest of {0} {1} calib params", sel, am);
                    continue;
                }
                string current = String.Format("{0} {1} parameters", sel, am.FullName());
                int logid = 34170 + (int)am;
                LogLevels lvl = LogLevels.Verbose;
                DataRow dr;
                switch (am)
                {
                    case AnalysisMethod.KnownA:
                        INCCAnalysisParams.known_alpha_rec ks = new INCCAnalysisParams.known_alpha_rec();
                        dr = db.Get(sel.detectorid, sel.material, "known_alpha_rec");
                        if (dr != null)
                        {
                            ks.rho_zero = DB.Utils.DBDouble(dr["rho_zero"]);
                            ks.alpha_wt = DB.Utils.DBDouble(dr["alpha_wt"]);
                            ks.k = DB.Utils.DBDouble(dr["k"]);
                            ks.cev.a = DB.Utils.DBDouble(dr["a"]);
                            ks.cev.b = DB.Utils.DBDouble(dr["b"]);
                            ks.cev.var_a = DB.Utils.DBDouble(dr["var_a"]);
                            ks.cev.var_b = DB.Utils.DBDouble(dr["var_b"]);
                            ks.cev.setcovar(Coeff.a, Coeff.b, DB.Utils.DBDouble(dr["covar_ab"]));
                            ks.cev.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
                            ks.known_alpha_type = (INCCAnalysisParams.KnownAlphaVariant)(DB.Utils.DBInt32(dr["known_alpha_type"]));
                            ks.ring_ratio.cal_curve_equation = (INCCAnalysisParams.CurveEquation)(DB.Utils.DBInt32(dr["ring_ratio_equation"]));
                            ks.ring_ratio.a = DB.Utils.DBDouble(dr["ring_ratio_a"]);
                            ks.ring_ratio.b = DB.Utils.DBDouble(dr["ring_ratio_b"]);
                            ks.ring_ratio.c = DB.Utils.DBDouble(dr["ring_ratio_c"]);
                            ks.ring_ratio.d = DB.Utils.DBDouble(dr["ring_ratio_d"]);
                            ks.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                            ks.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
                            ks.heavy_metal_reference = DB.Utils.DBDouble(dr["heavy_metal_reference"]);
                            ks.heavy_metal_corr_factor = DB.Utils.DBDouble(dr["heavy_metal_corr_factor"]);
                            ks.cev.upper_mass_limit = DB.Utils.DBDouble(dr["upper_mass_limit"]);
                            ks.cev.lower_mass_limit = DB.Utils.DBDouble(dr["lower_mass_limit"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, ks);
                        break;
                    case AnalysisMethod.CalibrationCurve:
                        INCCAnalysisParams.cal_curve_rec cs = new INCCAnalysisParams.cal_curve_rec();
                        dr = db.Get(sel.detectorid, sel.material, "cal_curve_rec");
                        if (dr != null)
                        {
                            CalCurveDBSnock(cs.cev, dr);
                            cs.CalCurveType = (INCCAnalysisParams.CalCurveType)DB.Utils.DBInt32(dr["cal_curve_type"]);
                            cs.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                            cs.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
                            cs.percent_u235 = DB.Utils.DBDouble(dr["percent_u235"]);
                            cs.heavy_metal_reference = DB.Utils.DBDouble(dr["heavy_metal_reference"]);
                            cs.heavy_metal_corr_factor = DB.Utils.DBDouble(dr["heavy_metal_corr_factor"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, cs);
                        break;
                    case AnalysisMethod.KnownM:
                        INCCAnalysisParams.known_m_rec ms = new INCCAnalysisParams.known_m_rec();
                        dr = db.Get(sel.detectorid, sel.material, "known_m_rec");
                        if (dr != null)
                        {
                            ms.sf_rate = DB.Utils.DBDouble(dr["sf_rate"]);
                            ms.vs1 = DB.Utils.DBDouble(dr["vs1"]);
                            ms.vs2 = DB.Utils.DBDouble(dr["vs2"]);
                            ms.vi1 = DB.Utils.DBDouble(dr["vi1"]);
                            ms.vi2 = DB.Utils.DBDouble(dr["vi2"]);
                            ms.b = DB.Utils.DBDouble(dr["b"]);
                            ms.c = DB.Utils.DBDouble(dr["c"]);
                            ms.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
                            ms.lower_mass_limit = DB.Utils.DBDouble(dr["lower_mass_limit"]);
                            ms.upper_mass_limit = DB.Utils.DBDouble(dr["upper_mass_limit"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, ms);
                        break;
                    case AnalysisMethod.Multiplicity:
                        INCCAnalysisParams.multiplicity_rec mu = new INCCAnalysisParams.multiplicity_rec();
                        dr = db.Get(sel.detectorid, sel.material, "multiplicity_rec");
                        if (dr != null)
                        {
                            mu.solve_efficiency = (INCCAnalysisParams.MultChoice)DB.Utils.DBInt32(dr["solve_efficiency"]);
                            mu.sf_rate = DB.Utils.DBDouble(dr["sf_rate"]);
                            mu.vs1 = DB.Utils.DBDouble(dr["vs1"]);
                            mu.vs2 = DB.Utils.DBDouble(dr["vs2"]);
                            mu.vs3 = DB.Utils.DBDouble(dr["vs3"]);
                            mu.vi1 = DB.Utils.DBDouble(dr["vi1"]);
                            mu.vi2 = DB.Utils.DBDouble(dr["vi2"]);
                            mu.vi3 = DB.Utils.DBDouble(dr["vi3"]);
                            mu.a = DB.Utils.DBDouble(dr["a"]);
                            mu.b = DB.Utils.DBDouble(dr["b"]);
                            mu.c = DB.Utils.DBDouble(dr["c"]);
                            mu.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
                            mu.alpha_weight = DB.Utils.DBDouble(dr["alpha_weight"]);
                            mu.multEffCorFactor = DB.Utils.DBDouble(dr["eff_cor"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, mu);
                        break;
                    case AnalysisMethod.TruncatedMultiplicity:
                        INCCAnalysisParams.truncated_mult_rec tm = new INCCAnalysisParams.truncated_mult_rec();
                        dr = db.Get(sel.detectorid, sel.material, "truncated_mult_rec");
                        if (dr != null)
                        {
                            tm.known_eff = DB.Utils.DBBool(dr["known_eff"]);
                            tm.solve_eff = DB.Utils.DBBool(dr["vs1"]);
                            tm.a = DB.Utils.DBDouble(dr["a"]);
                            tm.b = DB.Utils.DBDouble(dr["b"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, tm);
                        break;
                    case AnalysisMethod.CuriumRatio:
                        INCCAnalysisParams.curium_ratio_rec cm = new INCCAnalysisParams.curium_ratio_rec();
                        dr = db.Get(sel.detectorid, sel.material, "curium_ratio_rec");
                        if (dr != null)
                        {
                            cm.curium_ratio_type = (INCCAnalysisParams.CuriumRatioVariant)DB.Utils.DBInt32(dr["curium_ratio_type"]);
                            CalCurveDBSnock(cm.cev, dr);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, cm);
                        break;
                    case AnalysisMethod.Active:
                        INCCAnalysisParams.active_rec ar = new INCCAnalysisParams.active_rec();
                        dr = db.Get(sel.detectorid, sel.material, "active_rec");
                        if (dr != null)
                        {
                            CalCurveDBSnock(ar.cev, dr);
                            ar.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                            ar.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
                        }
                        else
                            lvl = LogLevels.Warning;
                        ams.AddMethod(am, ar);
                        break;
                    case AnalysisMethod.AddASource:
                        INCCAnalysisParams.add_a_source_rec aas = new INCCAnalysisParams.add_a_source_rec();
                        dr = db.Get(sel.detectorid, sel.material, "add_a_source_rec");
                        if (dr != null)
                        {
                            CalCurveDBSnock(aas.cev, dr);
                            aas.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                            aas.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);

                            aas.cf.a = DB.Utils.DBDouble(dr["cf_a"]);
                            aas.cf.b = DB.Utils.DBDouble(dr["cf_b"]);
                            aas.cf.c = DB.Utils.DBDouble(dr["cf_c"]);
                            aas.cf.d = DB.Utils.DBDouble(dr["cf_d"]);

                            aas.dzero_avg = DB.Utils.DBDouble(dr["dzero_avg"]);
                            aas.num_runs = DB.Utils.DBUInt16(dr["num_runs"]);
                            aas.position_dzero = DB.Utils.ReifyDoubles((string)dr["position_dzero"]);
                            aas.dzero_ref_date = DB.Utils.DBDateTime(dr["dzero_ref_date"]);
                            aas.use_truncated_mult = DB.Utils.DBBool(dr["use_truncated_mult"]);
                            aas.tm_dbls_rate_upper_limit = DB.Utils.DBDouble(dr["tm_dbls_rate_upper_limit"]);
                            aas.tm_weighting_factor = DB.Utils.DBDouble(dr["tm_weighting_factor"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, aas);
                        break;
                    case AnalysisMethod.ActiveMultiplicity:
                        INCCAnalysisParams.active_mult_rec amr = new INCCAnalysisParams.active_mult_rec();
                        dr = db.Get(sel.detectorid, sel.material, "active_mult_rec");
                        if (dr != null)
                        {
                            amr.vf1 = DB.Utils.DBDouble(dr["vf1"]);
                            amr.vf2 = DB.Utils.DBDouble(dr["vf2"]);
                            amr.vf3 = DB.Utils.DBDouble(dr["vf3"]);
                            amr.vt1 = DB.Utils.DBDouble(dr["vt1"]);
                            amr.vt2 = DB.Utils.DBDouble(dr["vt2"]);
                            amr.vt3 = DB.Utils.DBDouble(dr["vt3"]);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, amr);
                        break;
                    case AnalysisMethod.ActivePassive:
                        INCCAnalysisParams.active_passive_rec acp = new INCCAnalysisParams.active_passive_rec();
                        dr = db.Get(sel.detectorid, sel.material, "active_passive_rec");
                        if (dr != null)
                        {
                            CalCurveDBSnock(acp.cev, dr);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, acp);
                        break;
                    case AnalysisMethod.Collar:
                        INCCAnalysisParams.collar_combined_rec cr = new INCCAnalysisParams.collar_combined_rec();
                        dr = db.Get(sel.detectorid, sel.material, "collar_detector_rec");
                        if (dr != null)
                        {
                            cr.collar_det.collar_mode = DB.Utils.DBBool(dr["collar_detector_mode"]);
                            cr.collar_det.reference_date = DB.Utils.DBDateTime(dr["reference_date"]);
                            cr.collar_det.relative_doubles_rate = DB.Utils.DBDouble(dr["relative_doubles_rate"]);
                        }
                        else
                            lvl = LogLevels.Info;
						dr = db.Get(sel.detectorid, sel.material, "collar_rec");
                        if (dr != null)
                        {
                            CalCurveDBSnock(cr.collar.cev, dr);
                            cr.collar.collar_mode = DB.Utils.DBBool(dr["collar_mode"]);
                            cr.collar.number_calib_rods = DB.Utils.DBInt32(dr["number_calib_rods"]);
                            cr.collar.sample_corr_fact.v = DB.Utils.DBDouble(dr["sample_corr_fact"]);
                            cr.collar.sample_corr_fact.err = DB.Utils.DBDouble(dr["sample_corr_fact_err"]);
                            cr.collar.u_mass_corr_fact_a.v = DB.Utils.DBDouble(dr["u_mass_corr_fact_a"]);
                            cr.collar.u_mass_corr_fact_a.err = DB.Utils.DBDouble(dr["u_mass_corr_fact_a_err"]);
                            cr.collar.u_mass_corr_fact_b.v = DB.Utils.DBDouble(dr["u_mass_corr_fact_b"]);
                            cr.collar.u_mass_corr_fact_b.err = DB.Utils.DBDouble(dr["u_mass_corr_fact_b_err"]);
                            cr.collar.poison_absorption_fact = DB.Utils.ReifyDoubles(dr["poison_absorption_fact"].ToString());
                            cr.collar.poison_rod_type = DB.Utils.ReifyStrings(dr["poison_rod_type"].ToString());
                            TupleArraySlurp(ref cr.collar.poison_rod_a, "poison_rod_a", dr);
                            TupleArraySlurp(ref cr.collar.poison_rod_b, "poison_rod_b", dr);
                            TupleArraySlurp(ref cr.collar.poison_rod_c, "poison_rod_c", dr);
                        }
                        else
                            lvl = LogLevels.Info;
						dr = db.Get(sel.detectorid, sel.material, "collar_k5_rec");
                        if (dr != null)
                        {
                            cr.k5.k5_mode = DB.Utils.DBBool(dr["k5_mode"]);
                            cr.k5.k5_checkbox = DB.Utils.ReifyBools(dr["k5_checkbox"].ToString());
							cr.k5.k5_item_type = string.Copy(sel.material);
							cr.k5.k5_label = DB.Utils.ReifyStrings(dr["k5_label"].ToString());
                            TupleArraySlurp(ref cr.k5.k5, "k5", dr);
                        }
                        else
                            lvl = LogLevels.Info;
                        ams.AddMethod(am, cr);
                        break;
                    default:
                        lvl = LogLevels.Error; logid = 34181; current = "Choosing to not construct" + current;
                        break;
                }
                switch (lvl)
                {
                    case LogLevels.Info:
                        current = "Using default for " + current;
                        lvl = LogLevels.Verbose;
                        break;
                    case LogLevels.Verbose:
                        current = "Retrieved for " + current;
                        break;
                    default:
                        break;
                }
                NC.App.Pest.logger.TraceEvent(lvl, logid, current);
            } // for

        }

		public static void IngestAnalysisMethodResultsFromDB(Measurement m,  DB.DB db = null)
		{
			AnalysisMethods ams = m.INCCAnalysisState.Methods;
			IEnumerator iter = m.CountingAnalysisResults.GetMultiplicityEnumerator();
			while (iter.MoveNext())
			{
				Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
				foreach (AnalysisMethod am in System.Enum.GetValues(typeof(AnalysisMethod)))
				{
					if (!ams.choices[(int)am])
						continue;
					if (!(am > AnalysisMethod.None && am <= AnalysisMethod.TruncatedMultiplicity && (am != AnalysisMethod.INCCNone)))
					{
						if (!am.IsNone())
							NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34061, "Skipping DB ingest of {0} calib results", am);
						continue;
					}
					long mid = m.MeasurementId.UniqueId;
					DB.ParamsRelatedBackToMeasurement ar = new DB.ParamsRelatedBackToMeasurement(db);
					DataTable dt;
					switch (am)
					{
					case AnalysisMethod.CalibrationCurve:
						{ 
							INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)
								m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
							ar.table = "results_cal_curve_rec";
							dt = ar.GetCombinedResults(mid);
							foreach (DataRow dr in dt.Rows)
							{
								long rid = DB.Utils.DBInt64(dr["id"]);
								ccres.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
								ccres.pu_mass = VTupleHelper.Make(dr, "pu_mass");
								ccres.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
								ccres.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
								ccres.pass = DB.Utils.DBBool(dr["pass"]);
								ccres.dcl_u_mass = DB.Utils.DBDouble(dr["dcl_u_mass"]);
								ccres.length = DB.Utils.DBDouble(dr["length"]);
								ccres.heavy_metal_content = DB.Utils.DBDouble(dr["heavy_metal_content"]);
								ccres.heavy_metal_correction = DB.Utils.DBDouble(dr["heavy_metal_correction"]);
								ccres.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                                ccres.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
								ccres.heavy_metal_corr_singles = VTupleHelper.Make(dr, "heavy_metal_corr_singles");
								ccres.heavy_metal_corr_doubles = VTupleHelper.Make(dr, "heavy_metal_corr_doubles");
								INCCAnalysisParams.cal_curve_rec cc = ccres.methodParams;
								CalCurveDBSnock(cc.cev, dr);	
								cc.CalCurveType = (INCCAnalysisParams.CalCurveType)DB.Utils.DBInt32(dr["cal_curve_type"]);
								cc.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
								cc.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
								cc.percent_u235 = DB.Utils.DBDouble(dr["percent_u235"]);
								cc.heavy_metal_reference = DB.Utils.DBDouble(dr["heavy_metal_reference"]);
								cc.heavy_metal_corr_factor = DB.Utils.DBDouble(dr["heavy_metal_corr_factor"]);						
							}
						}
						break;
					case AnalysisMethod.KnownA:
						{ 
							INCCMethodResults.results_known_alpha_rec res = (INCCMethodResults.results_known_alpha_rec)
								m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
							ar.table = "results_known_alpha_rec";
							dt = ar.GetCombinedResults(mid);
							foreach (DataRow dr in dt.Rows)
							{
								res.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
								res.pu_mass = VTupleHelper.Make(dr, "pu_mass");
								res.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
								res.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
								res.pass = DB.Utils.DBBool(dr["pass"]);
								res.mult = DB.Utils.DBDouble(dr["mult"]);
								res.alphaK = DB.Utils.DBDouble(dr["alpha"]);
								res.mult_corr_doubles = VTupleHelper.Make(dr, "mult_corr_doubles");

								res.dcl_u_mass = DB.Utils.DBDouble(dr["dcl_u_mass"]);
								res.length = DB.Utils.DBDouble(dr["length"]);
								res.heavy_metal_content = DB.Utils.DBDouble(dr["heavy_metal_content"]);
								res.heavy_metal_correction = DB.Utils.DBDouble(dr["heavy_metal_correction"]);

								res.corr_factor = DB.Utils.DBDouble(dr["corr_factor"]);
								res.dry_alpha_or_mult_dbls = DB.Utils.DBDouble(dr["dry_alpha_or_mult_dbls"]);
								res.corr_singles = VTupleHelper.Make(dr, "corr_singles");
								res.corr_doubles = VTupleHelper.Make(dr, "corr_doubles");

								INCCAnalysisParams.known_alpha_rec ka = res.methodParams;
								ka.rho_zero = DB.Utils.DBDouble(dr["rho_zero"]);
								ka.alpha_wt = DB.Utils.DBDouble(dr["alpha_wt"]);
								ka.k = DB.Utils.DBDouble(dr["k"]);
								ka.cev.a = DB.Utils.DBDouble(dr["a"]);
								ka.cev.b = DB.Utils.DBDouble(dr["b"]);
								ka.cev.var_a = DB.Utils.DBDouble(dr["var_a"]);
								ka.cev.var_b = DB.Utils.DBDouble(dr["var_b"]);
								ka.cev.setcovar(Coeff.a, Coeff.b, DB.Utils.DBDouble(dr["covar_ab"]));
								ka.cev.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
								ka.known_alpha_type = (INCCAnalysisParams.KnownAlphaVariant)(DB.Utils.DBInt32(dr["known_alpha_type"]));
								ka.ring_ratio.cal_curve_equation = (INCCAnalysisParams.CurveEquation)(DB.Utils.DBInt32(dr["ring_ratio_equation"]));
								ka.ring_ratio.a = DB.Utils.DBDouble(dr["ring_ratio_a"]);
								ka.ring_ratio.b = DB.Utils.DBDouble(dr["ring_ratio_b"]);
								ka.ring_ratio.c = DB.Utils.DBDouble(dr["ring_ratio_c"]);
								ka.ring_ratio.d = DB.Utils.DBDouble(dr["ring_ratio_d"]);
								ka.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
								ka.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
								ka.heavy_metal_reference = DB.Utils.DBDouble(dr["heavy_metal_reference"]);
								ka.heavy_metal_corr_factor = DB.Utils.DBDouble(dr["heavy_metal_corr_factor"]);
								ka.cev.upper_mass_limit = DB.Utils.DBDouble(dr["upper_mass_limit"]);
								ka.cev.lower_mass_limit = DB.Utils.DBDouble(dr["lower_mass_limit"]);		
							}
						}
						break;

					case AnalysisMethod.KnownM:
                        {
                            INCCMethodResults.results_known_m_rec res = (INCCMethodResults.results_known_m_rec)
                                m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                            ar.table = "results_known_m_rec";
                            dt = ar.GetCombinedResults(mid);
                            foreach (DataRow dr in dt.Rows)
                            {
                                res.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
                                res.pu_mass = VTupleHelper.Make(dr, "pu_mass");
                                res.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
                                res.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
                                res.pass = DB.Utils.DBBool(dr["pass"]);
                                res.mult = DB.Utils.DBDouble(dr["mult"]);
                                res.alpha = DB.Utils.DBDouble(dr["alpha"]);
                                res.pu239e_mass = DB.Utils.DBDouble(dr["pu239e_mass"]);
                                res.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                                res.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
                                INCCAnalysisParams.known_m_rec km = res.methodParams;
                                km.b = DB.Utils.DBDouble(dr["b"]);
                                km.c = DB.Utils.DBDouble(dr["c"]);
                                km.vs1 = DB.Utils.DBDouble(dr["vs1"]);
                                km.vs2 = DB.Utils.DBDouble(dr["vs2"]);
                                km.vi1 = DB.Utils.DBDouble(dr["vi1"]);
                                km.vi2 = DB.Utils.DBDouble(dr["vi2"]);
                                km.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
                                km.sf_rate = DB.Utils.DBDouble(dr["sf_rate"]);
                                km.lower_mass_limit = DB.Utils.DBDouble(dr["lower_mass_limit"]);
                                km.upper_mass_limit = DB.Utils.DBDouble(dr["upper_mass_limit"]);
                            }
                        }
                        break;
                    case AnalysisMethod.Multiplicity:
                        {
                            INCCMethodResults.results_multiplicity_rec res = (INCCMethodResults.results_multiplicity_rec)
                                m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                            ar.table = "results_multiplicity_rec";
                            dt = ar.GetCombinedResults(mid);
                            foreach (DataRow dr in dt.Rows)
                            {
                                res.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
                                res.pu_mass = VTupleHelper.Make(dr, "pu_mass");
                                res.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
                                res.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
                                res.pass = DB.Utils.DBBool(dr["pass"]);
                                res.mult = VTupleHelper.Make(dr, "mult");
                                res.alphaK = VTupleHelper.Make(dr, "alpha");
                                res.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                                res.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
                                res.corr_factor = VTupleHelper.Make(dr, "corr_factor");
                                res.efficiencyComputed = VTupleHelper.Make(dr, "efficiency");
                                INCCAnalysisParams.multiplicity_rec mu = res.methodParams;
                                mu.solve_efficiency = (INCCAnalysisParams.MultChoice)DB.Utils.DBInt32(dr["solve_efficiency"]);
                                mu.sf_rate = DB.Utils.DBDouble(dr["sf_rate"]);
                                mu.vs1 = DB.Utils.DBDouble(dr["vs1"]);
                                mu.vs2 = DB.Utils.DBDouble(dr["vs2"]);
                                mu.vs3 = DB.Utils.DBDouble(dr["vs3"]);
                                mu.vi1 = DB.Utils.DBDouble(dr["vi1"]);
                                mu.vi2 = DB.Utils.DBDouble(dr["vi2"]);
                                mu.vi3 = DB.Utils.DBDouble(dr["vi3"]);
                                mu.a = DB.Utils.DBDouble(dr["a"]);
                                mu.b = DB.Utils.DBDouble(dr["b"]);
                                mu.c = DB.Utils.DBDouble(dr["c"]);
                                mu.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
                                mu.alpha_weight = DB.Utils.DBDouble(dr["alpha_weight"]);
                                mu.multEffCorFactor = DB.Utils.DBDouble(dr["eff_cor"]);
                            }
                        }
                        break;
                    case AnalysisMethod.AddASource:
                        {
                            INCCMethodResults.results_add_a_source_rec res = (INCCMethodResults.results_add_a_source_rec)
                                m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                            ar.table = "results_add_a_source_rec";
                            dt = ar.GetCombinedResults(mid);
                            foreach (DataRow dr in dt.Rows)
                            {
                                res.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
                                res.pu_mass = VTupleHelper.Make(dr, "pu_mass");
                                res.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
                                res.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
                                res.pass = DB.Utils.DBBool(dr["pass"]);
                                res.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                                res.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
                                res.corr_doubles = VTupleHelper.Make(dr, "corr_doubles");
                                res.corr_factor = VTupleHelper.Make(dr, "corr_factor");
                                res.delta = VTupleHelper.Make(dr, "delta");
                                res.tm_doubles_bkg = VTupleHelper.Make(dr, "tm_doubles_bkg");
                                res.tm_uncorr_doubles = VTupleHelper.Make(dr, "tm_uncorr_doubles");
                                res.tm_corr_doubles = VTupleHelper.Make(dr, "tm_corr_doubles");
                                res.sample_avg_cf252_doubles = VTupleHelper.Make(dr, "sample_avg_cf252_doubles");
                                res.dzero_cf252_doubles = DB.Utils.DBDouble(dr["dzero_cf252_doubles"]);
                                res.sample_cf252_doubles = TupleArraySlurp(ref res.sample_cf252_doubles, "sample_cf252_doubles", dr);
                                res.sample_cf252_ratio = DB.Utils.ReifyDoubles((string)dr["sample_cf252_ratio"]);

                                INCCAnalysisParams.add_a_source_rec aas = res.methodParams;
                                CalCurveDBSnock(aas.cev, dr);
                                aas.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                                aas.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
                                aas.cf.a = DB.Utils.DBDouble(dr["cf_a"]);
                                aas.cf.b = DB.Utils.DBDouble(dr["cf_b"]);
                                aas.cf.c = DB.Utils.DBDouble(dr["cf_c"]);
                                aas.cf.d = DB.Utils.DBDouble(dr["cf_d"]);
                                aas.dzero_avg = DB.Utils.DBDouble(dr["dzero_avg"]);
                                aas.num_runs = DB.Utils.DBUInt16(dr["num_runs"]);
                                aas.position_dzero = DB.Utils.ReifyDoubles((string)dr["position_dzero"]);
                                aas.dzero_ref_date = DB.Utils.DBDateTime(dr["dzero_ref_date"]);
                                aas.use_truncated_mult = DB.Utils.DBBool(dr["use_truncated_mult"]);
                                aas.tm_dbls_rate_upper_limit = DB.Utils.DBDouble(dr["tm_dbls_rate_upper_limit"]);
                                aas.tm_weighting_factor = DB.Utils.DBDouble(dr["tm_weighting_factor"]);
                            }
                        }
                        break;  
                    case AnalysisMethod.Active:
                    {
                        INCCMethodResults.results_active_rec res = (INCCMethodResults.results_active_rec)
                            m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                        ar.table = "results_active_rec";
                        dt = ar.GetCombinedResults(mid);
                        foreach (DataRow dr in dt.Rows)
                        {
                            res.k0.v = DB.Utils.DBDouble(dr["k0"]);
                            res.k1 = VTupleHelper.Make(dr, "k1");
                            res.k = VTupleHelper.Make(dr, "k");
                            res.u235_mass = VTupleHelper.Make(dr, "u235_mass");
                            res.dcl_u235_mass = DB.Utils.DBDouble(dr["dcl_u235_mass"]);
                            res.u235_mass = VTupleHelper.Make(dr, "u235_mass");
                            res.dcl_minus_asy_u235_mass = VTupleHelper.Make(dr, "dcl_minus_asy_u235_mass");
                            res.dcl_minus_asy_u235_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_u235_mass_pct"]);
                            res.pass = DB.Utils.DBBool(dr["pass"]);

                            INCCAnalysisParams.active_rec a = res.methodParams;
                            CalCurveDBSnock(a.cev, dr);
                            a.dcl_mass = DB.Utils.ReifyDoubles((string)dr["dcl_mass"]);
                            a.doubles = DB.Utils.ReifyDoubles((string)dr["doubles"]);
                        }
                    }
                    break;  
                    case AnalysisMethod.CuriumRatio:
                    {
                        INCCMethodResults.results_curium_ratio_rec res = (INCCMethodResults.results_curium_ratio_rec)
                            m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                        ar.table = "results_curium_ratio_rec";
                        dt = ar.GetCombinedResults(mid);
                        foreach (DataRow dr in dt.Rows)
                        {
                            res.cm_pu_ratio_decay_corr = VTupleHelper.Make(dr, "cm_pu_ratio_decay_corr");
                            res.cm_u_ratio_decay_corr = VTupleHelper.Make(dr, "cm_u_ratio_decay_corr");
                            res.cm_mass = VTupleHelper.Make(dr,"cm_mass");

                            INCCAnalysisParams.cm_pu_ratio_rec v = res.methodParams2;
                            v.cm_pu_ratio = VTupleHelper.Make(dr, "cm_pu_ratio");
                            v.cm_u_ratio = VTupleHelper.Make(dr, "cm_u_ratio");
                            v.pu_half_life = DB.Utils.DBDouble(dr["pu_half_life"]);
                            v.cm_pu_ratio_date = DB.Utils.DBDateTime(dr["cm_pu_ratio_date"]);
                            v.cm_id_label = dr["cm_id_label"].ToString();
                            v.cm_id = dr["cm_id"].ToString();
                            v.cm_input_batch_id = dr["cm_input_batch_id"].ToString();
                            v.cm_dcl_u_mass = DB.Utils.DBDouble(dr["dcl_u_mass"]);
                            v.cm_dcl_u235_mass = DB.Utils.DBDouble(dr["dcl_u235_mass"]);

                            INCCAnalysisParams.curium_ratio_rec cr = res.methodParams;
                            cr.curium_ratio_type = (INCCAnalysisParams.CuriumRatioVariant)DB.Utils.DBInt32(dr["curium_ratio_type"]);
                            CalCurveDBSnock(cr.cev, dr);

                            res.pu.pu240e_mass = VTupleHelper.Make(dr, "pu240e_mass");
                            res.pu.mass = VTupleHelper.Make(dr, "pu_mass");
                            res.pu.dcl_pu240e_mass = DB.Utils.DBDouble(dr["dcl_pu240e_mass"]);
                            res.pu.dcl_pu_mass = DB.Utils.DBDouble(dr["dcl_pu_mass"]);
                            res.pu.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                            res.pu.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
                            res.pu.dcl_minus_asy_mass = VTupleHelper.Make(dr, "dcl_minus_asy_pu_mass");
                            res.pu.dcl_minus_asy_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_pu_mass_pct"]);
                            res.pu.pass = DB.Utils.DBBool(dr["pu_pass"]);

                            res.u.mass = VTupleHelper.Make(dr, "u_mass");
                            res.u.dcl_minus_asy_mass = VTupleHelper.Make(dr, "dcl_minus_asy_u_mass");
                            res.u.dcl_minus_asy_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_u_mass_pct"]);
                            res.u.dcl_mass = v.cm_dcl_u_mass;
                            res.u.pass = DB.Utils.DBBool(dr["u_pass"]);

                            res.u235.mass = VTupleHelper.Make(dr, "u235_mass");
                            res.u235.dcl_minus_asy_mass = VTupleHelper.Make(dr, "dcl_minus_asy_u235_mass");
                            res.u235.dcl_minus_asy_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_u235_mass_pct"]);
                            res.u235.dcl_mass = v.cm_dcl_u235_mass;
                        }
                    }
                    break;

					case AnalysisMethod.Collar:
                    {
                        INCCMethodResults.results_collar_rec res = (INCCMethodResults.results_collar_rec)
                        m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
                        ar.table = "results_collar_rec";
                        dt = ar.GetCombinedResults(mid);  // results_collar_rec + collar_rec_m
						long rid = 0;
						if (dt.Rows.Count > 0)
								rid = DB.Utils.DBInt64(dt.Rows[0]["rid"]);  // the results key (rid) relates the 3 collar params in the results to the typed collar results
						else
								continue;
						DataTable dt_collar_detector_rec_m = ar.GetMethodResultsMethod(mid, rid, "collar_detector_rec");
						DataTable dt_collar_k5_rec_m = ar.GetMethodResultsMethod(mid, rid, "collar_k5_rec");
                        for (int di = 0; di < dt.Rows.Count; di++)
                        {
							DataRow dr = dt.Rows[di];
                            res.k0 = VTupleHelper.Make(dr, "k0");
                            res.k1 = VTupleHelper.Make(dr, "k1");
                            res.k2 = VTupleHelper.Make(dr, "k2");
                            res.k3 = VTupleHelper.Make(dr, "k3");
                            res.k4 = VTupleHelper.Make(dr, "k4");
                            res.k5 = VTupleHelper.Make(dr, "k5");
                            res.u235_mass = VTupleHelper.Make(dr, "u235_mass");
                            res.total_corr_fact = VTupleHelper.Make(dr, "total_corr_fact");
                            res.dcl_length = VTupleHelper.Make(dr, "dcl_length");
                            res.dcl_total_u235 = VTupleHelper.Make(dr, "dcl_total_u235");
                            res.dcl_total_u238 = VTupleHelper.Make(dr, "dcl_total_u238");
                            res.dcl_poison_percent = VTupleHelper.Make(dr, "dcl_poison_percent");
                            res.dcl_minus_asy_u235_mass = VTupleHelper.Make(dr, "dcl_minus_asy_u235_mass");
                            res.corr_doubles = VTupleHelper.Make(dr, "corr_doubles");
                            res.percent_u235 = DB.Utils.DBDouble(dr["percent_u235"]);
                            res.total_u_mass = DB.Utils.DBDouble(dr["total_u_mass"]);
                            res.dcl_total_rods = DB.Utils.DBDouble(dr["dcl_total_rods"]);
                            res.dcl_total_poison_rods = DB.Utils.DBDouble(dr["dcl_total_poison_rods"]);
                            res.dcl_minus_asy_u235_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_u235_mass_pct"]);
                            res.pass = DB.Utils.DBBool(dr["pass"]);
                            res.source_id = dr["source_id"].ToString();

                            INCCAnalysisParams.collar_combined_rec cr = res.methodParams;
                            CalCurveDBSnock(cr.collar.cev, dr);
                            cr.collar.collar_mode = DB.Utils.DBBool(dr["collar_mode"]);
                            cr.collar.number_calib_rods = DB.Utils.DBInt32(dr["number_calib_rods"]);
                            cr.collar.sample_corr_fact.v = DB.Utils.DBDouble(dr["sample_corr_fact"]);
                            cr.collar.sample_corr_fact.err = DB.Utils.DBDouble(dr["sample_corr_fact_err"]);
                            cr.collar.u_mass_corr_fact_a.v = DB.Utils.DBDouble(dr["u_mass_corr_fact_a"]);
                            cr.collar.u_mass_corr_fact_a.err = DB.Utils.DBDouble(dr["u_mass_corr_fact_a_err"]);
                            cr.collar.u_mass_corr_fact_b.v = DB.Utils.DBDouble(dr["u_mass_corr_fact_b"]);
                            cr.collar.u_mass_corr_fact_b.err = DB.Utils.DBDouble(dr["u_mass_corr_fact_b_err"]);
                            cr.collar.poison_absorption_fact = DB.Utils.ReifyDoubles(dr["poison_absorption_fact"].ToString());
                            cr.collar.poison_rod_type = DB.Utils.ReifyStrings(dr["poison_rod_type"].ToString());
                            TupleArraySlurp(ref cr.collar.poison_rod_a, "poison_rod_a", dr);
                            TupleArraySlurp(ref cr.collar.poison_rod_b, "poison_rod_b", dr);
                            TupleArraySlurp(ref cr.collar.poison_rod_c, "poison_rod_c", dr);

							if (di < dt_collar_detector_rec_m.Rows.Count)
								dr = dt_collar_detector_rec_m.Rows[di];
                            cr.collar_det.collar_mode = DB.Utils.DBBool(dr["collar_detector_mode"]);
                            cr.collar_det.reference_date = DB.Utils.DBDateTime(dr["reference_date"]);
                            cr.collar_det.relative_doubles_rate = DB.Utils.DBDouble(dr["relative_doubles_rate"]);

							if (di < dt_collar_k5_rec_m.Rows.Count)
								dr = dt_collar_k5_rec_m.Rows[di];
							cr.k5.k5_mode = DB.Utils.DBBool(dr["k5_mode"]);
                            bool[] b = DB.Utils.ReifyBools(dr["k5_checkbox"].ToString());
                            for (int i = 0; i < b.Length && i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i++)
                                cr.k5.k5_checkbox[i] = b[i];
                            cr.k5.k5_item_type = string.Copy(m.INCCAnalysisState.Methods.selector.material);
                            string[] s = DB.Utils.ReifyStrings(dr["k5_label"].ToString());
                            for (int i = 0; i < s.Length && i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i++)
                                cr.k5.k5_label[i] = s[i];
                            TupleArraySlurp(ref cr.k5.k5, "k5", dr);
                       } 
                    }
                    break;
					case AnalysisMethod.ActiveMultiplicity:
					{
						INCCMethodResults.results_active_mult_rec res = (INCCMethodResults.results_active_mult_rec)
							m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
						ar.table = "results_active_mult_rec";
						dt = ar.GetCombinedResults(mid);
						foreach (DataRow dr in dt.Rows)
						{
							res.mult = VTupleHelper.Make(dr, "mult");
							res.methodParams.vf1 = DB.Utils.DBDouble(dr["vf1"]);
							res.methodParams.vf2 = DB.Utils.DBDouble(dr["vf2"]);
							res.methodParams.vf3 = DB.Utils.DBDouble(dr["vf3"]);
							res.methodParams.vt1 = DB.Utils.DBDouble(dr["vt1"]);
							res.methodParams.vt2 = DB.Utils.DBDouble(dr["vt2"]);
							res.methodParams.vt3 = DB.Utils.DBDouble(dr["vt3"]);
						}
					}
					break;
					case AnalysisMethod.ActivePassive:
					{
						INCCMethodResults.results_active_passive_rec res = (INCCMethodResults.results_active_passive_rec)
							m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
						ar.table = "results_active_passive_rec";
						dt = ar.GetCombinedResults(mid);
						foreach (DataRow dr in dt.Rows)
						{
							res.k0.v = DB.Utils.DBDouble(dr["k0"]);
							res.k = VTupleHelper.Make(dr, "k");
							res.k1 = VTupleHelper.Make(dr, "k1");
							res.delta_doubles = VTupleHelper.Make(dr, "delta_doubles");
							res.u235_mass = VTupleHelper.Make(dr, "u235_mass");
							res.dcl_u235_mass = DB.Utils.DBDouble(dr["dcl_u235_mass"]);
							res.dcl_minus_asy_u235_mass_pct = DB.Utils.DBDouble(dr["dcl_minus_asy_u235_mass_pct"]);
							res.dcl_minus_asy_u235_mass = VTupleHelper.Make(dr, "dcl_minus_asy_u235_mass");
                            res.pass = DB.Utils.DBBool(dr["pass"]);

                            INCCAnalysisParams.active_passive_rec a = res.methodParams;
                            CalCurveDBSnock(a.cev, dr);
						}
					}
					break;
					case AnalysisMethod.TruncatedMultiplicity:
					{
						INCCMethodResults.results_truncated_mult_rec res = (INCCMethodResults.results_truncated_mult_rec)
							m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
						ar.table = "results_truncated_mult_rec";
						dt = ar.GetCombinedResults(mid);
						foreach (DataRow dr in dt.Rows)
						{
							res.bkg.Singles = VTupleHelper.Make(dr, "bkg_singles");
							res.bkg.Zeros = VTupleHelper.Make(dr, "bkg_zeros");
							res.bkg.Ones = VTupleHelper.Make(dr, "bkg_ones");
							res.bkg.Twos = VTupleHelper.Make(dr, "bkg_twos");
							res.net.Singles = VTupleHelper.Make(dr, "net_singles");
							res.net.Zeros = VTupleHelper.Make(dr, "net_zeros");
							res.net.Ones = VTupleHelper.Make(dr, "net_ones");
							res.net.Twos = VTupleHelper.Make(dr, "net_twos");

                            res.k.alpha = VTupleHelper.Make(dr, "k_alpha");
                            res.k.pu240e_mass = VTupleHelper.Make(dr, "k_pu240e_mass");
                            res.k.mass = VTupleHelper.Make(dr, "k_pu_mass");
                            res.k.dcl_pu240e_mass = DB.Utils.DBDouble(dr["k_dcl_pu240e_mass"]);
                            res.k.dcl_pu_mass = DB.Utils.DBDouble(dr["k_dcl_pu_mass"]);
                            res.k.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "k_dcl_minus_asy_pu_mass");
                            res.k.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["k_dcl_minus_asy_pu_mass_pct"]);
                            res.k.pass = DB.Utils.DBBool(dr["k_pass"]);

							res.s.eff = VTupleHelper.Make(dr, "s_eff");
							res.s.alpha = VTupleHelper.Make(dr, "s_alpha");
                            res.s.pu240e_mass = VTupleHelper.Make(dr, "s_pu240e_mass");
                            res.s.mass = VTupleHelper.Make(dr, "s_pu_mass");
                            res.s.dcl_pu240e_mass = DB.Utils.DBDouble(dr["s_dcl_pu240e_mass"]);
                            res.s.dcl_pu_mass = DB.Utils.DBDouble(dr["s_dcl_pu_mass"]);
                            res.s.dcl_minus_asy_pu_mass = VTupleHelper.Make(dr, "s_dcl_minus_asy_pu_mass");
                            res.s.dcl_minus_asy_pu_mass_pct = DB.Utils.DBDouble(dr["s_dcl_minus_asy_pu_mass_pct"]);
                            res.s.pass = DB.Utils.DBBool(dr["s_pass"]);

							INCCAnalysisParams.truncated_mult_rec a = res.methodParams;
                            a.known_eff = DB.Utils.DBBool(dr["known_eff"]);
                            a.solve_eff = DB.Utils.DBBool(dr["vs1"]);
                            a.a = DB.Utils.DBDouble(dr["a"]);
                            a.b = DB.Utils.DBDouble(dr["b"]);						}
					}
					break;
					case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
					{
						INCCMethodResults.results_de_mult_rec res = (INCCMethodResults.results_de_mult_rec)
							m.INCCAnalysisResults.LookupMethodResults(mkey, m.INCCAnalysisState.Methods.selector, am, false);
						ar.table = "results_de_mult_rec";
						dt = ar.GetCombinedResults(mid);
						foreach (DataRow dr in dt.Rows)
						{
							res.meas_ring_ratio = DB.Utils.DBDouble(dr["meas_ring_ratio"]);
							res.interpolated_neutron_energy = DB.Utils.DBDouble(dr["interpolated_neutron_energy"]);
							res.energy_corr_factor = DB.Utils.DBDouble(dr["energy_corr_factor"]);
							INCCAnalysisParams.de_mult_rec a = res.methodParams;
                            a.inner_ring_efficiency = DB.Utils.DBDouble(dr["inner_ring_efficiency"]);
                            a.outer_ring_efficiency = DB.Utils.DBDouble(dr["outer_ring_efficiency"]);
							a.neutron_energy = DB.Utils.ReifyDoubles(dr["neutron_energy"].ToString());
							a.detector_efficiency = DB.Utils.ReifyDoubles(dr["detector_efficiency"].ToString());
							a.inner_outer_ring_ratio = DB.Utils.ReifyDoubles(dr["inner_outer_ring_ratio"].ToString());
							a.relative_fission = DB.Utils.ReifyDoubles(dr["relative_fission"].ToString());
						}
					}
						break;
					default:
						NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34061, "Unimplemented DB restore of {0} calib results", am.FullName());
						break;
					}
				} // for
			}

		}


		static VTuple[] TupleArraySlurp(ref VTuple[] dest, string field, DataRow dr)
        {
            double[] v = DB.Utils.ReifyDoubles(dr[field].ToString());
            double[] err = DB.Utils.ReifyDoubles(dr[field + "_err"].ToString());
            for (int i = 0; i < v.Length && i < dest.Length; i++)
                dest[i] = new VTuple(v[i], err[i]);
            return dest;
        }

        static void CalCurveDBSnock(INCCAnalysisParams.CurveEquationVals cev, DataRow dr)
        {            
            if (dr == null) return;
            cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)(DB.Utils.DBInt32(dr["cal_curve_equation"]));
            cev.a = DB.Utils.DBDouble(dr["a"]);
            cev.b = DB.Utils.DBDouble(dr["b"]);
            cev.c = DB.Utils.DBDouble(dr["c"]);
            cev.d = DB.Utils.DBDouble(dr["d"]);
            cev.var_a = DB.Utils.DBDouble(dr["var_a"]);
            cev.var_b = DB.Utils.DBDouble(dr["var_b"]);
            cev.var_c = DB.Utils.DBDouble(dr["var_c"]);
            cev.var_d = DB.Utils.DBDouble(dr["var_d"]);
            cev.setcovar(Coeff.a, Coeff.b, DB.Utils.DBDouble(dr["covar_ab"]));
            cev.setcovar(Coeff.a, Coeff.c, DB.Utils.DBDouble(dr["covar_ac"]));
            cev.setcovar(Coeff.a, Coeff.d, DB.Utils.DBDouble(dr["covar_ad"]));
            cev.setcovar(Coeff.b, Coeff.c, DB.Utils.DBDouble(dr["covar_bc"]));
            cev.setcovar(Coeff.b, Coeff.d, DB.Utils.DBDouble(dr["covar_bd"]));
            cev.setcovar(Coeff.c, Coeff.d, DB.Utils.DBDouble(dr["covar_cd"]));
            cev.sigma_x = DB.Utils.DBDouble(dr["sigma_x"]);
            cev.upper_mass_limit = DB.Utils.DBDouble(dr["upper_mass_limit"]);
            cev.lower_mass_limit = DB.Utils.DBDouble(dr["lower_mass_limit"]);
        }


#region results
        // stub, TBD
        public void UpdateAnalysisMethodResults(string detname, string mat, DB.AnalysisMethodSpecifiers db = null)
        {
            if (db == null)
                db = new DB.AnalysisMethodSpecifiers();
            var res =   // this finds the am for the given detector and acquire type
                    from am in this.DetectorMaterialAnalysisMethods
                    where (am.Key.detectorid.Equals(detname, StringComparison.OrdinalIgnoreCase) && 
                           am.Key.material.Equals(mat, StringComparison.OrdinalIgnoreCase))
                    select am;
            if (res.Count() > 0)  // now execute the select expression and test the result for existence
            {
                KeyValuePair<INCCSelector, AnalysisMethods> kv = res.First();
                AnalysisMethods sam = kv.Value;  // the descriptor instance

                IEnumerator iter = kv.Value.GetMethodEnumerator();
                while (iter.MoveNext())
                {
                    System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor> md = (System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor>)iter.Current;
                    if (md.Item2 == null) // case from INCC5 transfer missing params, reflects file write bugs in INCC5 code
                    {
                        NC.App.Pest.logger.TraceEvent(LogLevels.Warning, 34029, "Missing {0}'s INCC {1} method parameters, skipping to next entry", detname, md.Item1.ToString());
                        continue;
                    }

                    NC.App.Pest.logger.TraceEvent(LogLevels.Verbose, 34030, "Updating <{0},{1}>: {2}", detname, mat, md.Item2.GetType().Name);

                   DB.ElementList parms = null;
                    switch (md.Item1)
                    {
                        case AnalysisMethod.KnownA:
                        case AnalysisMethod.CalibrationCurve:
                        case AnalysisMethod.KnownM:
                        case AnalysisMethod.Multiplicity:
                        case AnalysisMethod.TruncatedMultiplicity:
                        case AnalysisMethod.AddASource:
                        case AnalysisMethod.CuriumRatio:
                        case AnalysisMethod.Active:
                        case AnalysisMethod.ActivePassive:
                        case AnalysisMethod.Collar: // bad mojo with the design break here
                        case AnalysisMethod.ActiveMultiplicity:
                        case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
                            parms = ((ParameterBase)md.Item2).ToDBElementList();
                            break;
                        default:
                            break;
                    }
                    if (parms != null)
                        db.UpdateCalib(detname, mat, md.Item2.GetType().Name, parms);  // det, mat, amid, params
                }
            }
        }

#endregion results

    }
}
