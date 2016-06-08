/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
using AnalysisDefs;
using DB;
using DetectorDefs;
using NCCReporter;
namespace NCCTransfer
{
    using Tuple = AnalysisDefs.VTuple;
    using NC = NCC.CentralizedState;

    public class INCCKnew
    {
        LMLoggers.LognLM mlogger;

        public INCCKnew()
        {
        }
        public INCCKnew(LMLoggers.LognLM logger)
        {
            mlogger = logger;
        }

        public int DetectorIndex;

        public unsafe void BuildDetector(INCCInitialDataDetectorFile iddf, int num)
        {
            INCCDB DB = NC.App.DB;

            detector_rec d = iddf.Detector[0];
            sr_parms_rec sr = iddf.SRParms[0];
            bkg_parms_rec bkh = iddf.BKGParms[0];
            norm_parms_rec norm = iddf.NormParms[0];
            tm_bkg_parms_rec tmbkg = iddf.TMBKGParms[0];
            add_a_source_setup_rec aass = iddf.AASParms[0];

            InstrType srtype = (InstrType)sr.sr_type;

            bool overwrite = NC.App.AppContext.OverwriteImportedDefs;

            mlogger.TraceEvent(LogLevels.Verbose, 34100, "Building '{0}' detector '{1}' from {2} {3}",
                                srtype.ToString(),
                                TransferUtils.str(d.detector_id, INCC.MAX_DETECTOR_ID_LENGTH),
                                num, iddf.Path);

            // if the detector is not known internally, then 
            //        add the detector to the list of detectors in memory, and 
            //        associate a new set of SR, Bkg and Norm and AB params with the new detector 
            // todo: What are the HV Params from INCC5, and once that is known, can they be transferred to the INCC6 HV Param and results tables
            Detector det = new Detector();

            try
            {
                // this transfer should be a method in this class, it will be used elsewhere too
                det.Id.DetectorId = TransferUtils.str(d.detector_id, INCC.MAX_DETECTOR_ID_LENGTH);
                det.Id.SRType = srtype;
                det.Id.Type = TransferUtils.str(d.detector_type, INCC.DETECTOR_TYPE_LENGTH);
                det.Id.ElectronicsId = TransferUtils.str(d.electronics_id, INCC.ELECTRONICS_ID_LENGTH);
                det.Id.ConnInfo = sr.sr_port_number.ToString();
                det.Id.source = ConstructedSource.INCCTransferCopy;
                det.MultiplicityParams.FA = srtype.DefaultFAFor();
                det.MultiplicityParams.gateWidthTics = (ulong)(sr.gate_length * 10.0);  // shift down to tics from microseconds
                det.SRParams.deadTimeCoefficientAinMicroSecs = sr.coeff_a_deadtime;
                det.SRParams.deadTimeCoefficientBinPicoSecs = sr.coeff_b_deadtime;
                det.SRParams.deadTimeCoefficientCinNanoSecs = sr.coeff_c_deadtime;
                det.SRParams.deadTimeCoefficientMultiplicityinNanoSecs = sr.multiplicity_deadtime;
                det.SRParams.dieAwayTime = sr.die_away_time * 10.0;  // shift down to tics from microseconds
                det.SRParams.doublesGateFraction = sr.doubles_gate_fraction;
                det.SRParams.efficiency = sr.efficiency;
                det.SRParams.gateLength = (ulong)(sr.gate_length * 10.0);  // shift down to tics from microseconds
                //det.SRParams.gateLength2 = sr.gate_length2;
                det.SRParams.highVoltage = sr.high_voltage;
                det.SRParams.predelay = (ulong)(sr.predelay * 10.0);  // shift down to tics from microseconds
                det.SRParams.triplesGateFraction = sr.triples_gate_fraction;
                //    = sr.sr_type , sr.sr_port_number, sr.sr_detector_id  these are in the Id now, not the SRparams, but they travel together.

                if (NC.App.AppContext.OverwriteImportedDefs)
                    DB.Detectors.Replace(det);
                else
                    DB.Detectors.AddOnlyIfNotThere(det);
                DetectorIndex = DB.Detectors.Count - 1;

                BackgroundParameters bkg = new BackgroundParameters();
                bkg.DeadtimeCorrectedRates.Singles.v = bkh.curr_passive_bkg_singles_rate;
                bkg.DeadtimeCorrectedRates.Doubles.v = bkh.curr_passive_bkg_doubles_rate;
                bkg.DeadtimeCorrectedRates.Triples.v = bkh.curr_passive_bkg_triples_rate;
                bkg.DeadtimeCorrectedRates.Singles.err = bkh.curr_passive_bkg_singles_err;
                bkg.DeadtimeCorrectedRates.Doubles.err = bkh.curr_passive_bkg_doubles_err;
                bkg.DeadtimeCorrectedRates.Triples.err = bkh.curr_passive_bkg_triples_err;
                bkg.Scaler1.v = bkh.curr_passive_bkg_scaler1_rate;
                bkg.Scaler2.v = bkh.curr_passive_bkg_scaler2_rate;
                bkg.INCCActive.Singles.v = bkh.curr_active_bkg_singles_rate;
                bkg.INCCActive.Singles.err = bkh.curr_active_bkg_singles_err;
                bkg.INCCActive.Scaler1Rate = bkh.curr_active_bkg_scaler1_rate;
                bkg.INCCActive.Scaler2Rate = bkh.curr_active_bkg_scaler2_rate;
                bkg.TMBkgParams.Singles.v = tmbkg.tm_singles_bkg;
                bkg.TMBkgParams.Ones.v = tmbkg.tm_ones_bkg;
                bkg.TMBkgParams.Twos.v = tmbkg.tm_twos_bkg;
                bkg.TMBkgParams.Zeros.v = tmbkg.tm_zeros_bkg;
                bkg.TMBkgParams.Singles.err = tmbkg.tm_singles_bkg_err;
                bkg.TMBkgParams.Ones.err = tmbkg.tm_ones_bkg_err;
                bkg.TMBkgParams.Twos.err = tmbkg.tm_twos_bkg_err;
                bkg.TMBkgParams.Zeros.err = tmbkg.tm_zeros_bkg_err;
                bkg.TMBkgParams.ComputeTMBkg = (tmbkg.tm_bkg == 0 ? false : true);

                if (DB.BackgroundParameters.Get(det.Id.DetectorName) == null)
                {
                    DB.BackgroundParameters.GetMap().Add(det, bkg); // saved to DB at below
                }
                else if (overwrite)
                {
                    bkg.modified = true;
                    NC.App.DB.BackgroundParameters.GetMap().Remove(det);
                    NC.App.DB.BackgroundParameters.GetMap().Add(det, bkg);
                    NC.App.DB.BackgroundParameters.Set(det, bkg);
                }

                // save the params listed here using the detector as the key
                NormParameters normp = new NormParameters();
                normp.acceptanceLimitPercent = norm.acceptance_limit_percent;
                normp.acceptanceLimitStdDev = norm.acceptance_limit_std_dev;
                normp.amliRefSinglesRate = norm.amli_ref_singles_rate;
                normp.biasMode = OldToNewBiasTestId(norm.bias_mode);
                normp.biasPrecisionLimit = norm.bias_precision_limit;
                normp.biasTestAddasrcPosition = norm.bias_test_addasrc_position;
                normp.biasTestUseAddasrc = (norm.bias_test_use_addasrc == 0 ? false : true);
                normp.cf252RefDoublesRate.v = norm.cf252_ref_doubles_rate;
                normp.currNormalizationConstant.v = norm.curr_normalization_constant;
                normp.currNormalizationConstant.err = norm.curr_normalization_constant_err;
                normp.initSrcPrecisionLimit = norm.init_src_precision_limit;
                normp.measRate.v = norm.meas_rate;
                normp.measRate.err = norm.meas_rate_err;
                normp.refDate = INCC.DateFrom(TransferUtils.str(norm.ref_date, INCC.DATE_TIME_LENGTH));
                normp.sourceId = TransferUtils.str(norm.source_id, INCC.SOURCE_ID_LENGTH);
                normp.yieldRelativeToMrc95 = norm.yield_relative_to_mrc_95;

                if (DB.NormParameters.Get(det.Id.DetectorName) == null)
                {
                    DB.NormParameters.GetMap().Add(det, normp); // saved to DB at end
                }
                else if (overwrite)
                {
                    normp.modified = true;
                    DB.NormParameters.GetMap().Remove(det);
                    DB.NormParameters.GetMap().Add(det, normp); // the in-memory map
                    DB.NormParameters.Set(det, normp);  // the DB table
                }
                AddASourceSetup aassp = new AddASourceSetup();
                aassp.type = OldToNewAASId(aass.ad_type);
                aassp.port_number = aass.ad_port_number;
                aassp.forward_over_travel = aass.ad_forward_over_travel;
                aassp.reverse_over_travel = aass.ad_reverse_over_travel;
                aassp.number_positions = aass.ad_number_positions;
                aassp.dist_to_move = TransferUtils.Copy(aass.ad_dist_to_move, INCC.MAX_ADDASRC_POSITIONS);
                aassp.cm_steps_per_inch = aass.cm_steps_per_inch;
                aassp.cm_forward_mask = aass.cm_forward_mask;
                aassp.cm_reverse_mask = aass.cm_reverse_mask;
                aassp.cm_axis_number = aass.cm_axis_number;
                aassp.cm_over_travel_state = aass.cm_over_travel_state;
                aassp.cm_step_ratio = aass.cm_step_ratio;
                aassp.cm_slow_inches = aass.cm_slow_inches;
                aassp.plc_steps_per_inch = aass.plc_steps_per_inch;
                aassp.scale_conversion_factor = aass.scale_conversion_factor;
                aassp.cm_rotation = (aass.cm_rotation == 0 ? false : true);

                if (!DB.AASSParameters.GetMap().ContainsKey(det))
                {
                    DB.AASSParameters.GetMap().Add(det, aassp);
                }
                else if (overwrite)
                {
                    aassp.modified = true;
                    DB.AASSParameters.GetMap().Remove(det);
                    DB.AASSParameters.GetMap().Add(det, aassp); // todo: in-memory and db 
                }
                if (!DB.UnattendedParameters.GetMap().ContainsKey(det))
                {
                    DB.UnattendedParameters.GetMap().Add(det, new UnattendedParameters());
                }
                else if (overwrite)
                {
                    DB.UnattendedParameters.GetMap().Remove(det);
                    DB.UnattendedParameters.GetMap().Add(det, new UnattendedParameters());
                }


                // the alpha beta arrays must be sized here, for first use by the calc_alpha_beta code in the cycle conditioning code
                Multiplicity mkey = new Multiplicity(srtype.DefaultFAFor());
                mkey.SR = new ShiftRegisterParameters(det.SRParams);
                MultiplicityCountingRes mcr = new MultiplicityCountingRes(srtype.DefaultFAFor(), 0);
				ABKey abkey = new ABKey(mkey, mcr);
				AnalysisDefs.AlphaBeta AB = AlphaBetaCache.GetAlphaBeta(abkey);
				if (AB == null)
				{
					CycleProcessing.calc_alpha_beta(mkey, mcr);
					AlphaBetaCache.AddAlphaBeta(abkey, mcr.AB);
				}
                det.AB.TransferIntermediates(mcr.AB);
            }
            catch (Exception e)
            {
                mlogger.TraceEvent(LogLevels.Warning, 34064, "Detector transfer processing error {0} {1}", det.Id.DetectorName, e.Message);
            }
        }

        // 0 is calib curve up to truncated mult at 11, 9 is none
        private AnalysisMethod OldToNewMethodId(int id)
        {
            AnalysisMethod am = AnalysisMethod.None;
            if (id >= 0 && id <= INCC.METHOD_TRUNCATED_MULT)
            {
                am = (AnalysisMethod)(id + 1);
            }
            else if (id < 0 || id > INCC.METHOD_TRUNCATED_MULT)
            {
                switch (id)
                {
                    case INCC.DUAL_ENERGY_MULT_SAVE_RESTORE:	// Dual energy multiplicity
                        am = AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE;
                        break;
                    case INCC.COLLAR_SAVE_RESTORE:
                        am = AnalysisMethod.COLLAR_SAVE_RESTORE;
                        break;
                    case INCC.COLLAR_DETECTOR_SAVE_RESTORE:
                        am = AnalysisMethod.COLLAR_DETECTOR_SAVE_RESTORE;
                       break;
                    case INCC.COLLAR_K5_SAVE_RESTORE:
                       am = AnalysisMethod.COLLAR_K5_SAVE_RESTORE;
                       break;
                    case INCC.WMV_CALIB_TOKEN:
                       am = AnalysisMethod.WMV_CALIB_TOKEN;
                        break;
                }
            }
            return am;
        }

        private int OldTypeToOldMethodId(object o)
        {
            System.Type t = o.GetType();
            if (t.Equals(typeof(NCCTransfer.analysis_method_rec)))
                return INCC.METHOD_NONE;	// No analysis method selected
            else if (t.Equals(typeof(NCCTransfer.cal_curve_rec)))
                return INCC.METHOD_CALCURVE;	// Passive Calibration Curve
            else if (t.Equals(typeof(NCCTransfer.known_alpha_rec)))
                return INCC.METHOD_AKNOWN;	// Known Alpha
            else if (t.Equals(typeof(NCCTransfer.known_m_rec)))
                return INCC.METHOD_MKNOWN;	// Known M
            else if (t.Equals(typeof(NCCTransfer.multiplicity_rec)))
                return INCC.METHOD_MULT;// Multiplicity
            else if (t.Equals(typeof(NCCTransfer.add_a_source_rec)))
                return INCC.METHOD_ADDASRC;	// Add-a-source
            else if (t.Equals(typeof(NCCTransfer.active_rec)))
                return INCC.METHOD_ACTIVE;	// Active Calibration Curve
            else if (t.Equals(typeof(NCCTransfer.active_mult_rec)))
                return INCC.METHOD_ACTIVE_MULT;	// Active Multiplicity
            else if (t.Equals(typeof(NCCTransfer.active_passive_rec)))
                return INCC.METHOD_ACTPAS;	// Active/Passive
            else if (t.Equals(typeof(NCCTransfer.curium_ratio_rec)))
                return INCC.METHOD_CURIUM_RATIO;	// Curium Ratio
            else if (t.Equals(typeof(NCCTransfer.truncated_mult_rec)))
                return INCC.METHOD_TRUNCATED_MULT;	// Truncated multiplicity 
            else if (t.Equals(typeof(NCCTransfer.collar_detector_rec)))
                return INCC.COLLAR_DETECTOR_SAVE_RESTORE;
            else if (t.Equals(typeof(NCCTransfer.collar_rec)))
                return INCC.COLLAR_SAVE_RESTORE;
            else if (t.Equals(typeof(NCCTransfer.collar_k5_rec)))
                return INCC.COLLAR_K5_SAVE_RESTORE;
            else if (t.Equals(typeof(NCCTransfer.de_mult_rec)))
                return INCC.DUAL_ENERGY_MULT_SAVE_RESTORE;

            return -1;
        }

        static unsafe public Tuple[] Copy(double* vptr, double* errptr, int len)
        {

            Tuple[] vals = new Tuple[len];
            for (int i = 0; i < len; i++)
                vals[i] = new Tuple(vptr[i], errptr[i]);
            return vals;
        }

        public unsafe void BuildCalibration(INCCInitialDataCalibrationFile idcf, int num)
        {

            mlogger.TraceEvent(LogLevels.Verbose, 34200, "Building calibration content from {0} {1}", num, idcf.Path);
            bool overwrite = NC.App.AppContext.OverwriteImportedDefs;
            IEnumerator iter = idcf.DetectorMaterialMethodParameters.GetDetectorMaterialEnumerator();
            while (iter.MoveNext())
            {
                DetectorMaterialMethod mkey = (DetectorMaterialMethod)iter.Current;

                mlogger.TraceEvent(LogLevels.Verbose, 34210, "Constructing calibration for {0} {1}", mkey.detector_id, mkey.item_type);

                INCCDB.Descriptor desc = new INCCDB.Descriptor(mkey.item_type, "");
                if (!NC.App.DB.Materials.Has(desc) || overwrite)
                {
                    NC.App.DB.Materials.Update(desc);                    
                }

                Detector det;
                // look in real detector list for entry with this basic id, if not found, punt or later try creating a new empty detector, it will be filled in later as processing proceeds

                det = NC.App.DB.Detectors.GetItByDetectorId(mkey.detector_id);
                if (det == null)
                {
                    // old code punted if detector not found
                    mlogger.TraceEvent(LogLevels.Warning, 34207, "Skipping detector {0}, pre-existing def not found", mkey.detector_id); // dev note: need flags to control this ehavior in a few more places
                    continue;
                }
                IEnumerator miter2 = idcf.DetectorMaterialMethodParameters.GetMethodEnumerator(mkey.detector_id, mkey.item_type);
                if (miter2 == null)
                    continue;
                INCCSelector sel = new INCCSelector(mkey.detector_id, mkey.item_type);
                AnalysisMethods am;
                bool found = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out am);
                if (!found || am == null)
                {
                    am = new AnalysisMethods(mlogger);
                    am.modified = true;
                    NC.App.DB.DetectorMaterialAnalysisMethods.Add(sel, am);
                }
                while (miter2.MoveNext())
                {
                    // use ref here, not copy so updates update the ref? nope, it;s a map, need to remove and replace
                    int cam = OldTypeToOldMethodId(miter2.Current);
                    mlogger.TraceEvent(LogLevels.Verbose, 34211, "Converting {0} {1} method {2}", mkey.detector_id, mkey.item_type, OldToNewMethodId(cam).FullName());

                    // save the analysis method obtained here under the existing/new detector+material type pair                    
                    switch (cam)
                    {
                        case INCC.METHOD_NONE:
                            analysis_method_rec iamr = (analysis_method_rec)miter2.Current;
                            // create entries in the am map if needed, copy other settings onto am proper
                            am.Backup = OldToNewMethodId(iamr.backup_method);
                            am.Normal = OldToNewMethodId(iamr.normal_method);
                            am.choices[(int)AnalysisMethod.CalibrationCurve] = (iamr.cal_curve == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.Active] = (iamr.active == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.ActiveMultiplicity] = (iamr.active_mult == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.ActivePassive] = (iamr.active_passive == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.AddASource] = (iamr.add_a_source == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.Collar] = (iamr.collar == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.CuriumRatio] = (iamr.curium_ratio == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.KnownA] = (iamr.known_alpha == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.KnownM] = (iamr.known_m == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.Multiplicity] = (iamr.multiplicity == 0 ? false : true);
                            am.choices[(int)AnalysisMethod.TruncatedMultiplicity] = (iamr.truncated_mult == 0 ? false : true);
                            if (am.AnySelected())
                                am.choices[(int)AnalysisMethod.None] = am.choices[(int)AnalysisMethod.INCCNone] = false; // JFL open questionable change
                            break;
                        case INCC.METHOD_CALCURVE:
                            cal_curve_rec cal_curve = (cal_curve_rec)miter2.Current;
                            INCCAnalysisParams.cal_curve_rec cc = new INCCAnalysisParams.cal_curve_rec();
                            cc.heavy_metal_corr_factor = cal_curve.cc_heavy_metal_corr_factor;
                            cc.heavy_metal_reference = cal_curve.cc_heavy_metal_reference;
                            cc.cev.lower_mass_limit = cal_curve.cc_lower_mass_limit;
                            cc.cev.upper_mass_limit = cal_curve.cc_upper_mass_limit;
                            cc.percent_u235 = cal_curve.cc_percent_u235;
                            cc.dcl_mass = TransferUtils.Copy(cal_curve.cc_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            cc.doubles = TransferUtils.Copy(cal_curve.cc_doubles, INCC.MAX_NUM_CALIB_PTS);

                            cc.cev.a = cal_curve.cc_a; cc.cev.b = cal_curve.cc_b;
                            cc.cev.c = cal_curve.cc_c; cc.cev.d = cal_curve.cc_d;
                            cc.cev.var_a = cal_curve.cc_var_a; cc.cev.var_b = cal_curve.cc_var_b;
                            cc.cev.var_c = cal_curve.cc_var_c; cc.cev.var_d = cal_curve.cc_var_d;
                            cc.cev.setcovar(Coeff.a,Coeff.b,cal_curve.cc_covar_ab);
                            cc.cev._covar[0, 2] = cal_curve.cc_covar_ac;
                            cc.cev._covar[0, 3] = cal_curve.cc_covar_ad;
                            cc.cev._covar[1, 2] = cal_curve.cc_covar_bc;
                            cc.cev._covar[1, 3] = cal_curve.cc_covar_bd;
                            cc.cev._covar[2, 3] = cal_curve.cc_covar_cd;
                            cc.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)cal_curve.cal_curve_equation;
                            cc.CalCurveType = (INCCAnalysisParams.CalCurveType)cal_curve.cc_cal_curve_type;
                            cc.cev.sigma_x = cal_curve.cc_sigma_x;
                            am.AddMethod(AnalysisMethod.CalibrationCurve, cc);
                            break;
                        case INCC.METHOD_AKNOWN:
                            known_alpha_rec known_alpha = (known_alpha_rec)miter2.Current;
                            INCCAnalysisParams.known_alpha_rec ka = new INCCAnalysisParams.known_alpha_rec();

                            ka.alpha_wt = known_alpha.ka_alpha_wt;
                            ka.dcl_mass = TransferUtils.Copy(known_alpha.ka_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            ka.doubles = TransferUtils.Copy(known_alpha.ka_doubles, INCC.MAX_NUM_CALIB_PTS);
                            ka.heavy_metal_corr_factor = known_alpha.ka_heavy_metal_corr_factor;
                            ka.heavy_metal_reference = known_alpha.ka_heavy_metal_reference;
                            ka.k = known_alpha.ka_k;
                            ka.known_alpha_type = (INCCAnalysisParams.KnownAlphaVariant)known_alpha.ka_known_alpha_type;
                            ka.lower_corr_factor_limit = known_alpha.ka_lower_corr_factor_limit;
                            ka.upper_corr_factor_limit = known_alpha.ka_upper_corr_factor_limit;
                            ka.rho_zero = known_alpha.ka_rho_zero;

                            ka.ring_ratio.cal_curve_equation = (INCCAnalysisParams.CurveEquation)known_alpha.ka_ring_ratio_equation;
                            ka.ring_ratio.a = known_alpha.ka_ring_ratio_a;
                            ka.ring_ratio.b = known_alpha.ka_ring_ratio_b;
                            ka.ring_ratio.c = known_alpha.ka_ring_ratio_c;
                            ka.ring_ratio.d = known_alpha.ka_ring_ratio_d;

                            ka.cev.a = known_alpha.ka_a;
                            ka.cev.b = known_alpha.ka_b;
                            ka.cev.var_a = known_alpha.ka_var_a;
                            ka.cev.var_b = known_alpha.ka_var_b;
                            ka.cev.sigma_x = known_alpha.ka_sigma_x;
                            ka.cev._covar[(int)Coeff.a, (int)Coeff.b] = known_alpha.ka_covar_ab;
                            ka.cev.lower_mass_limit = known_alpha.ka_lower_mass_limit;
                            ka.cev.upper_mass_limit = known_alpha.ka_upper_mass_limit;
                            am.AddMethod(AnalysisMethod.KnownA, ka);
                            break;
                        case INCC.METHOD_MKNOWN:
                            known_m_rec known_m = (known_m_rec)miter2.Current;
                            INCCAnalysisParams.known_m_rec km = new INCCAnalysisParams.known_m_rec();
                            km.sf_rate = known_m.km_sf_rate;
                            km.vs1 = known_m.km_vs1;
                            km.vs2 = known_m.km_vs2;
                            km.vi1 = known_m.km_vi1;
                            km.vi2 = known_m.km_vi2;
                            km.b = known_m.km_b;
                            km.c = known_m.km_c;
                            km.lower_mass_limit = known_m.km_lower_mass_limit;
                            km.upper_mass_limit = known_m.km_upper_mass_limit;
                            am.AddMethod(AnalysisMethod.KnownM, km);
                            break;
                        case INCC.METHOD_MULT:
                            multiplicity_rec multiplicity = (multiplicity_rec)miter2.Current;
                            INCCAnalysisParams.multiplicity_rec m = new INCCAnalysisParams.multiplicity_rec();
                            m.solve_efficiency = (INCCAnalysisParams.MultChoice)multiplicity.mul_solve_efficiency;
                            m.sf_rate = multiplicity.mul_sf_rate;
                            m.vs1 = multiplicity.mul_vs1;
                            m.vs2 = multiplicity.mul_vs2;
                            m.vs3 = multiplicity.mul_vs3;
                            m.vi1 = multiplicity.mul_vi1;
                            m.vi2 = multiplicity.mul_vi2;
                            m.vi3 = multiplicity.mul_vi3;
                            m.a = multiplicity.mul_a;
                            m.b = multiplicity.mul_b;
                            m.c = multiplicity.mul_c;
                            m.sigma_x = multiplicity.mul_sigma_x;
                            m.alpha_weight = multiplicity.mul_alpha_weight;
                            am.AddMethod(AnalysisMethod.Multiplicity, m);
                            break;
                        case INCC.DUAL_ENERGY_MULT_SAVE_RESTORE:
                            de_mult_rec de_mult = (de_mult_rec)miter2.Current;
                            INCCAnalysisParams.de_mult_rec de = new INCCAnalysisParams.de_mult_rec();
                            de.detector_efficiency = TransferUtils.Copy(de_mult.de_detector_efficiency, INCC.MAX_DUAL_ENERGY_ROWS);
                            de.inner_outer_ring_ratio = TransferUtils.Copy(de_mult.de_inner_outer_ring_ratio, INCC.MAX_DUAL_ENERGY_ROWS);
                            de.neutron_energy = TransferUtils.Copy(de_mult.de_neutron_energy, INCC.MAX_DUAL_ENERGY_ROWS);
                            de.relative_fission = TransferUtils.Copy(de_mult.de_relative_fission, INCC.MAX_DUAL_ENERGY_ROWS);
                            de.inner_ring_efficiency = de_mult.de_inner_ring_efficiency;
                            de.outer_ring_efficiency = de_mult.de_outer_ring_efficiency;
                            am.AddMethod(AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE, de);
                            break;
                        case INCC.METHOD_TRUNCATED_MULT:
                            truncated_mult_rec truncated_mult = (truncated_mult_rec)miter2.Current;
                            INCCAnalysisParams.truncated_mult_rec tm = new INCCAnalysisParams.truncated_mult_rec();
                            tm.a = truncated_mult.tm_a;
                            tm.b = truncated_mult.tm_b;
                            tm.known_eff = (truncated_mult.tm_known_eff == 0 ? false : true);
                            tm.solve_eff = (truncated_mult.tm_solve_eff == 0 ? false : true);
                            am.AddMethod(AnalysisMethod.TruncatedMultiplicity, tm);
                            break;
                        case INCC.METHOD_CURIUM_RATIO:
                            curium_ratio_rec curium_ratio = (curium_ratio_rec)miter2.Current;
                            INCCAnalysisParams.curium_ratio_rec cr = new INCCAnalysisParams.curium_ratio_rec();
                            cr.curium_ratio_type = OldToNewCRVariants(curium_ratio.curium_ratio_type);

                            cr.cev.a = curium_ratio.cr_a; cr.cev.b = curium_ratio.cr_b;
                            cr.cev.c = curium_ratio.cr_c; cr.cev.d = curium_ratio.cr_d;
                            cr.cev.var_a = curium_ratio.cr_var_a; cr.cev.var_b = curium_ratio.cr_var_b;
                            cr.cev.var_c = curium_ratio.cr_var_c; cr.cev.var_d = curium_ratio.cr_var_d;
                            cr.cev.setcovar(Coeff.a,Coeff.b,curium_ratio.cr_covar_ab);
                            cr.cev._covar[0, 2] = curium_ratio.cr_covar_ac;
                            cr.cev._covar[0, 3] = curium_ratio.cr_covar_ad;
                            cr.cev._covar[1, 2] = curium_ratio.cr_covar_bc;
                            cr.cev._covar[1, 3] = curium_ratio.cr_covar_bd;
                            cr.cev._covar[2, 3] = curium_ratio.cr_covar_cd;
                            cr.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)curium_ratio.curium_ratio_equation;
                            cr.cev.sigma_x = curium_ratio.cr_sigma_x;
                            cr.cev.lower_mass_limit = curium_ratio.cr_lower_mass_limit;
                            cr.cev.upper_mass_limit = curium_ratio.cr_upper_mass_limit;
                            am.AddMethod(AnalysisMethod.CuriumRatio, cr);
                            break;
                        case INCC.METHOD_ADDASRC:
                            add_a_source_rec add_a_source = (add_a_source_rec)miter2.Current;
                            INCCAnalysisParams.add_a_source_rec aas = new INCCAnalysisParams.add_a_source_rec(); 
                            aas.cev.a = add_a_source.ad_a; aas.cev.b = add_a_source.ad_b;
                            aas.cev.c = add_a_source.ad_c; aas.cev.d = add_a_source.ad_d;
                            aas.cev.var_a = add_a_source.ad_var_a; aas.cev.var_b = add_a_source.ad_var_b;
                            aas.cev.var_c = add_a_source.ad_var_c; aas.cev.var_d = add_a_source.ad_var_d;
                            aas.cev.setcovar(Coeff.a,Coeff.b,add_a_source.ad_covar_ab);
                            aas.cev._covar[0, 2] = add_a_source.ad_covar_ac;
                            aas.cev._covar[0, 3] = add_a_source.ad_covar_ad;
                            aas.cev._covar[1, 2] = add_a_source.ad_covar_bc;
                            aas.cev._covar[1, 3] = add_a_source.ad_covar_bd;
                            aas.cev._covar[2, 3] = add_a_source.ad_covar_cd;
                            aas.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)add_a_source.add_a_source_equation;
                            aas.cev.sigma_x = add_a_source.ad_sigma_x;
                            aas.cev.lower_mass_limit = add_a_source.ad_lower_mass_limit;
                            aas.cev.upper_mass_limit = add_a_source.ad_upper_mass_limit;

                            aas.cf.a = add_a_source.ad_cf_a; aas.cf.b = add_a_source.ad_cf_b;
                            aas.cf.c = add_a_source.ad_cf_c; aas.cf.d = add_a_source.ad_cf_d;
                            aas.dzero_avg = add_a_source.ad_dzero_avg;
                            aas.num_runs = add_a_source.ad_num_runs;
                            aas.tm_dbls_rate_upper_limit = add_a_source.ad_tm_dbls_rate_upper_limit;
                            aas.tm_weighting_factor = add_a_source.ad_tm_weighting_factor;
                            aas.use_truncated_mult = (add_a_source.ad_use_truncated_mult == 0 ? false : true);
                            aas.dzero_ref_date = INCC.DateFrom(TransferUtils.str(add_a_source.ad_dzero_ref_date, INCC.DATE_TIME_LENGTH));
                            aas.position_dzero = TransferUtils.Copy(add_a_source.ad_position_dzero, INCC.MAX_ADDASRC_POSITIONS);
                            am.AddMethod(AnalysisMethod.AddASource, aas);
                            break;
                        case INCC.METHOD_ACTIVE:
                            active_rec active = (active_rec)miter2.Current;
                            INCCAnalysisParams.active_rec ar = new INCCAnalysisParams.active_rec();

                            ar.cev.lower_mass_limit = active.act_lower_mass_limit;
                            ar.cev.upper_mass_limit = active.act_upper_mass_limit;
                            ar.dcl_mass = TransferUtils.Copy(active.act_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            ar.doubles = TransferUtils.Copy(active.act_doubles, INCC.MAX_NUM_CALIB_PTS);

                            ar.cev.a = active.act_a; ar.cev.b = active.act_b;
                            ar.cev.c = active.act_c; ar.cev.d = active.act_d;
                            ar.cev.var_a = active.act_var_a; ar.cev.var_b = active.act_var_b;
                            ar.cev.var_c = active.act_var_c; ar.cev.var_d = active.act_var_d;
                            ar.cev.setcovar(Coeff.a,Coeff.b,active.act_covar_ab);
                            ar.cev._covar[0, 2] = active.act_covar_ac;
                            ar.cev._covar[0, 3] = active.act_covar_ad;
                            ar.cev._covar[1, 2] = active.act_covar_bc;
                            ar.cev._covar[1, 3] = active.act_covar_bd;
                            ar.cev._covar[2, 3] = active.act_covar_cd;
                            ar.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)active.active_equation;
                            ar.cev.sigma_x = active.act_sigma_x;
                            am.AddMethod(AnalysisMethod.Active, ar);
                            break;
                        case INCC.METHOD_ACTPAS:
                            active_passive_rec active_passive = (active_passive_rec)miter2.Current;
                            INCCAnalysisParams.active_passive_rec apr = new INCCAnalysisParams.active_passive_rec();
                            apr.cev.lower_mass_limit = active_passive.ap_lower_mass_limit;
                            apr.cev.upper_mass_limit = active_passive.ap_upper_mass_limit;

                            apr.cev.a = active_passive.ap_a; apr.cev.b = active_passive.ap_b;
                            apr.cev.c = active_passive.ap_c; apr.cev.d = active_passive.ap_d;
                            apr.cev.var_a = active_passive.ap_var_a; apr.cev.var_b = active_passive.ap_var_b;
                            apr.cev.var_c = active_passive.ap_var_c; apr.cev.var_d = active_passive.ap_var_d;
                            apr.cev.setcovar(Coeff.a,Coeff.b,active_passive.ap_covar_ab);
                            apr.cev._covar[0, 2] = active_passive.ap_covar_ac;
                            apr.cev._covar[0, 3] = active_passive.ap_covar_ad;
                            apr.cev._covar[1, 2] = active_passive.ap_covar_bc;
                            apr.cev._covar[1, 3] = active_passive.ap_covar_bd;
                            apr.cev._covar[2, 3] = active_passive.ap_covar_cd;
                            apr.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)active_passive.active_passive_equation;
                            apr.cev.sigma_x = active_passive.ap_sigma_x;
                            am.AddMethod(AnalysisMethod.ActivePassive, apr);
                            break;
                        case INCC.COLLAR_DETECTOR_SAVE_RESTORE:
                            collar_detector_rec collar_detector = (collar_detector_rec)miter2.Current;
                            INCCAnalysisParams.collar_detector_rec cdr = new INCCAnalysisParams.collar_detector_rec();
                            cdr.collar_mode = (collar_detector.collar_detector_mode == 0 ? false : true);
                            cdr.reference_date = INCC.DateFrom(TransferUtils.str(collar_detector.col_reference_date, INCC.DATE_TIME_LENGTH));
                            cdr.relative_doubles_rate = collar_detector.col_relative_doubles_rate;
                            mlogger.TraceEvent(LogLevels.Info, 34211, " -- Collar det has mode {0}", cdr.collar_mode);
                            //NEXT: detector factor the dual collar mode domain key issue, need to bridge into these tables some other way
                            break;
                        case INCC.COLLAR_SAVE_RESTORE:
                            collar_rec collar = (collar_rec)miter2.Current;
                            INCCAnalysisParams.collar_rec c = new INCCAnalysisParams.collar_rec();
                            c.cev.lower_mass_limit = collar.col_lower_mass_limit;
                            c.cev.upper_mass_limit = collar.col_upper_mass_limit;
                            c.cev.a = collar.col_a; c.cev.b = collar.col_b;
                            c.cev.c = collar.col_c; c.cev.d = collar.col_d;
                            c.cev.var_a = collar.col_var_a; c.cev.var_b = collar.col_var_b;
                            c.cev.var_c = collar.col_var_c; c.cev.var_d = collar.col_var_d;
                            c.cev.setcovar(Coeff.a,Coeff.b,collar.col_covar_ab);
                            c.cev._covar[0, 2] = collar.col_covar_ac;
                            c.cev._covar[0, 3] = collar.col_covar_ad;
                            c.cev._covar[1, 2] = collar.col_covar_bc;
                            c.cev._covar[1, 3] = collar.col_covar_bd;
                            c.cev._covar[2, 3] = collar.col_covar_cd;
                            c.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)collar.collar_equation;
                            c.cev.sigma_x = collar.col_sigma_x;

                            c.poison_absorption_fact = TransferUtils.Copy(collar.col_poison_absorption_fact, INCC.MAX_POISON_ROD_TYPES);
                            c.poison_rod_a = Copy(collar.col_poison_rod_a, collar.col_poison_rod_a_err, INCC.MAX_POISON_ROD_TYPES);
                            c.poison_rod_b = Copy(collar.col_poison_rod_b, collar.col_poison_rod_b_err, INCC.MAX_POISON_ROD_TYPES);
                            c.poison_rod_c = Copy(collar.col_poison_rod_c, collar.col_poison_rod_c_err, INCC.MAX_POISON_ROD_TYPES);
                            c.collar_mode = (collar.collar_mode == 0 ? false : true);
                            c.u_mass_corr_fact_a.v = collar.col_u_mass_corr_fact_a;c.u_mass_corr_fact_a.err = collar.col_u_mass_corr_fact_a_err;
                            c.u_mass_corr_fact_b.v = collar.col_u_mass_corr_fact_b; c.u_mass_corr_fact_b.err = collar.col_u_mass_corr_fact_b_err;
                            c.sample_corr_fact.v = collar.col_sample_corr_fact;c.sample_corr_fact.err = collar.col_sample_corr_fact_err;
                            c.number_calib_rods = (int)collar.col_number_calib_rods;
                            for (int i = 0; i < INCC.MAX_POISON_ROD_TYPES; i++)
                            {
                                int index = i * INCC.MAX_ROD_TYPE_LENGTH;
                                c.poison_rod_type[i] = TransferUtils.str(collar.col_poison_rod_type + index, INCC.MAX_ROD_TYPE_LENGTH);
                            }
                            mlogger.TraceEvent(LogLevels.Info, 34211, " -- Collar params has mode {0}", c.collar_mode);
                            //NEXT: collar factor the dual collar mode domain key issue, need to bridge into these tables some other way

                            am.AddMethod(AnalysisMethod.Collar, c);
                            break;
                        case INCC.COLLAR_K5_SAVE_RESTORE:
                            collar_k5_rec collar_k5 = (collar_k5_rec)miter2.Current;
                            INCCAnalysisParams.collar_k5_rec ck5 = new INCCAnalysisParams.collar_k5_rec();
                            ck5.k5_mode = (collar_k5.collar_k5_mode  != 0 ? true: false);
                            ck5.k5_item_type = TransferUtils.str(collar_k5.collar_k5_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
                            ck5.k5 = Copy(collar_k5.collar_k5, collar_k5.collar_k5_err, INCC.MAX_COLLAR_K5_PARAMETERS);
                            ck5.k5_checkbox = TransferUtils.Copy(collar_k5.collar_k5_checkbox,INCC.MAX_COLLAR_K5_PARAMETERS);
                            for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
                            {
                                int index = i * INCC.MAX_K5_LABEL_LENGTH;
                                ck5.k5_label[i] = TransferUtils.str(collar_k5.collar_k5_label + index, INCC.MAX_K5_LABEL_LENGTH);
                            }
                            mlogger.TraceEvent(LogLevels.Info, 34211, " -- Collar k5 has mode {0}", ck5.k5_mode);
                            //NEXT: k5 factor the dual collar mode domain key issue, need to bridge into these tables some other way
                            break;
                        case INCC.METHOD_ACTIVE_MULT:
                            active_mult_rec active_mult = (active_mult_rec)miter2.Current;
                            INCCAnalysisParams.active_mult_rec acm = new INCCAnalysisParams.active_mult_rec();
                            acm.vf1 = active_mult.am_vf1;
                            acm.vf2 = active_mult.am_vf2;
                            acm.vf3 = active_mult.am_vf3;
                            acm.vt1 = active_mult.am_vt1;
                            acm.vt2 = active_mult.am_vt2;
                            acm.vt3 = active_mult.am_vt2;
                            am.AddMethod(AnalysisMethod.ActiveMultiplicity, acm);
                            break;
                        case INCC.WMV_CALIB_TOKEN:
                            mlogger.TraceEvent(LogLevels.Warning, 34247, "Skipping calib token");  // todo: weighted multiplicity not fully implemented throughout
                            break;
                    }
                }
            }
        }


        #region measurement data transfer


        private ErrorCalculationTechnique INCCErrorCalculationTechnique(int idx)
        {
            ErrorCalculationTechnique a = ErrorCalculationTechnique.Sample;
            switch (idx)
            {
                case INCC.IDC_THEORETICAL_STD_DEV:
                    a = ErrorCalculationTechnique.Theoretical;
                    break;
                case INCC.IDC_SAMPLE_STD_DEV:
                    a = ErrorCalculationTechnique.Sample;
                    break;
            }
            return a;
        }

        private AccidentalsMethod INCCAccidentalsMethod(int idx)
        {
            AccidentalsMethod a = AccidentalsMethod.None;
            switch (idx)
            {
                case INCC.IDC_MEASURE_ACCIDENTALS:
                    a = AccidentalsMethod.Measure;
                    break;
                case INCC.IDC_CALCULATE_ACCIDENTALS:
                    a = AccidentalsMethod.Calculate;
                    break;
            }
            return a;
        }

        private NormTest OldToNewBiasTestId(int id)
        {
            NormTest nt = NormTest.AmLiSingles;
            if (id < INCC.IDC_BIAS_SINGLES || id > INCC.IDC_BIAS_CF252_SINGLES)
                return nt;
            else
                switch (id)
                {
                    case INCC.IDC_BIAS_SINGLES:
                        nt = AnalysisDefs.NormTest.AmLiSingles;
                        break;
                    case INCC.IDC_BIAS_DOUBLES:
                        nt = AnalysisDefs.NormTest.Cf252Doubles;
                        break;
                    case INCC.IDC_BIAS_COLLAR:
                        nt = AnalysisDefs.NormTest.Collar;
                        break;
                    case INCC.IDC_BIAS_CF252_SINGLES:
                        nt = AnalysisDefs.NormTest.Cf252Singles;
                        break;
                }
            return nt;
        }

        private INCCAnalysisParams.CuriumRatioVariant OldToNewCRVariants(int id)
        {
            INCCAnalysisParams.CuriumRatioVariant isdh = INCCAnalysisParams.CuriumRatioVariant.UseDoubles;
            switch (id)
            {
                case INCC.IDC_USE_DOUBLES:
                    isdh = INCCAnalysisParams.CuriumRatioVariant.UseDoubles;
                    break;
                case INCC.IDC_USE_SINGLES:
                    isdh = INCCAnalysisParams.CuriumRatioVariant.UseSingles;
                    break;
                case INCC.IDC_USE_ADD_A_SOURCE_DOUBLES:
                    isdh = INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles;
                    break;
            }
            return isdh;
        }


        private AddASourceFlavors OldToNewAASId(int id)
        {
            AddASourceFlavors nt = AddASourceFlavors.None;
            switch (id)
            {
                case INCC.IDC_NONE:
                    nt = AddASourceFlavors.None;
                    break;
                case INCC.IDC_COMPUMOTOR_4000:
                    nt = AddASourceFlavors.CompuMotor_4000;
                    break;
                case INCC.IDC_COMPUMOTOR_3000:
                    nt = AddASourceFlavors.CompuMotor_3000;
                    break;
                case INCC.IDC_PLC_JCC21:
                    nt = AddASourceFlavors.PLC_JCC21;
                    break;
                case INCC.IDC_PLC_WM3100:
                    nt = AddASourceFlavors.PLC_WM3100;
                    break;
                case INCC.IDC_CANBERRA_COUNTER:
                    nt = AddASourceFlavors.Canberra_Counter;
                    break;
                case INCC.IDC_MANUAL:
                    nt = AddASourceFlavors.Manual;
                    break;
            }
            return nt;
        }


        private Measurement meas;

        public Measurement Meas
        {
            get { return meas; }
            set { meas = value; }
        }

        public unsafe bool BuildMeasurement(INCCTransferFile itf, int num)
        {

            bool overwrite = NC.App.AppContext.OverwriteImportedDefs;
            results_rec results = itf.results_rec_list[0];
            meas_id id;

            TransferUtils.Copy(results.meas_date, 0, id.meas_date, 0, INCC.DATE_TIME_LENGTH);
            TransferUtils.Copy(results.meas_time, 0, id.meas_time, 0, INCC.DATE_TIME_LENGTH);
            TransferUtils.Copy(results.filename, 0, id.filename, 0, INCC.FILE_NAME_LENGTH);
            TransferUtils.Copy(results.results_detector_id, 0, id.results_detector_id, 0, INCC.MAX_DETECTOR_ID_LENGTH);
            DateTime dt = INCC.DateTimeFrom(TransferUtils.str(id.meas_date, INCC.DATE_TIME_LENGTH), TransferUtils.str(id.meas_time, INCC.DATE_TIME_LENGTH));

            // do not use content from last BuildDetector call, instead look up the detector on the pre-existing list
            string detname = TransferUtils.str(id.results_detector_id, INCC.MAX_DETECTOR_ID_LENGTH);
            Detector det = NC.App.DB.Detectors.GetItByDetectorId(detname);
            if (det == null)
            {
                mlogger.TraceEvent(LogLevels.Error, 34087, "Unknown detector '{0}', not importing this measurement {1}", detname, dt.ToString("s"));
                return false;
            }

            meas = new Measurement((AssaySelector.MeasurementOption)results.meas_option, mlogger);
   
           // TODO: update detector details from this meas result, since there could be a difference 
            meas.MeasurementId.MeasDateTime = dt;
            meas.MeasurementId.FileName = TransferUtils.str(id.filename, INCC.FILE_NAME_LENGTH);
  
            meas.Detectors.Add(det);  // in practice this is a list with one element, e.g. meas.Detector

            TestParameters t = new TestParameters();
            t.accSnglTestRateLimit = results.r_acc_sngl_test_rate_limit;
            t.accSnglTestPrecisionLimit = results.r_acc_sngl_test_precision_limit;
            t.accSnglTestOutlierLimit = results.r_acc_sngl_test_outlier_limit;
            t.outlierTestLimit = results.r_outlier_test_limit;
            t.bkgDoublesRateLimit = results.r_bkg_doubles_rate_limit;
            t.bkgTriplesRateLimit = results.r_bkg_triples_rate_limit;
            t.chiSquaredLimit = results.r_chisq_limit;
            t.maxNumFailures = results.r_max_num_failures;
            t.highVoltageTestLimit = results.r_high_voltage_test_limit;
            t.normalBackupAssayTestLimit = results.r_normal_backup_assay_test_lim;
            t.maxCyclesForOutlierTest = (uint)results.r_max_runs_for_outlier_test;
            t.checksum = (results.r_checksum_test == 0.0 ? false : true);
            t.accidentalsMethod = INCCAccidentalsMethod((int)results.results_accidentals_method);
            meas.Tests.Copy(t);

            NormParameters n = NC.App.DB.NormParameters.Get(detname);
            meas.Norm.Copy(n);
            BackgroundParameters b = NC.App.DB.BackgroundParameters.Get(detname);
            meas.Background.Copy(b);
            // DB write for these occurs at end of processing here

            // Isotopics handled in this block
            if (itf.isotopics_table.Count > 0)
            {
                string isoname = TransferUtils.str(results.item_isotopics_id, INCC.MAX_ISOTOPICS_ID_LENGTH);
                NCCTransfer.isotopics_rec isotopics = itf.isotopics_table[0];
                //bool first = true; // forgot why the code does this, to put the default in the map?
                // foreach (LMTransfer.isotopics_rec ir in itf.istopics_table)
                // {
                AnalysisDefs.Isotopics iso;
                //       if (first)
                iso = meas.Isotopics;
                //        else
                //            iso = new Isotopics();
                //         first = false;
                iso.am_date = INCC.DateFrom(TransferUtils.str(isotopics.am_date, INCC.DATE_TIME_LENGTH));
                iso.pu_date = INCC.DateFrom(TransferUtils.str(isotopics.pu_date, INCC.DATE_TIME_LENGTH));
                iso.id = TransferUtils.str(isotopics.isotopics_id, INCC.MAX_ISOTOPICS_ID_LENGTH);
                AnalysisDefs.Isotopics.SourceCode checksc = AnalysisDefs.Isotopics.SourceCode.OD;
                string check = TransferUtils.str(isotopics.isotopics_source_code, INCC.ISO_SOURCE_CODE_LENGTH);
                bool okparse = System.Enum.TryParse(check, true, out checksc);
                iso.source_code = checksc;
                iso.SetValueError(Isotope.am241, isotopics.am241, isotopics.am241_err);
                iso.SetValueError(Isotope.pu238, isotopics.pu238, isotopics.pu238_err);
                iso.SetValueError(Isotope.pu239, isotopics.pu239, isotopics.pu239_err);
                iso.SetValueError(Isotope.pu240, isotopics.pu240, isotopics.pu240_err);
                iso.SetValueError(Isotope.pu241, isotopics.pu241, isotopics.pu241_err);
                iso.SetValueError(Isotope.pu242, isotopics.pu242, isotopics.pu242_err);

                if (!NC.App.DB.Isotopics.Has(iso.id))
                {
                    NC.App.DB.Isotopics.GetList().Add(iso);
                    mlogger.TraceEvent(LogLevels.Info, 34021, "Identified new isotopics {0}", iso.id);
                }
                else
                {
                    if (overwrite)
                    {
                        NC.App.DB.Isotopics.Replace(iso);
                        mlogger.TraceEvent(LogLevels.Warning, 34022, "Replaced existing isotopics {0}", iso.id);
                    }
                    else
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34022, "Not replacing existing isotopics {0}", iso.id);
                    }
                }
                iso.modified = true;
            }

            AcquireParameters acq = meas.AcquireState;
            acq.detector_id = det.Id.DetectorId;
            acq.meas_detector_id = string.Copy(det.Id.DetectorId);
            acq.item_type = TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
            acq.qc_tests = TransferUtils.ByteBool(results.results_qc_tests);
            acq.user_id = TransferUtils.str(results.user_id, INCC.CHAR_FIELD_LENGTH);
            acq.num_runs = results.number_good_runs;
            if (results.total_number_runs > 0)
                acq.run_count_time = results.total_good_count_time / results.total_number_runs;
            else
                acq.run_count_time = results.total_good_count_time; // should be 0.0 by default for this special case
            acq.MeasDateTime = meas.MeasurementId.MeasDateTime;
            acq.meas_detector_id = det.Id.DetectorId;
            acq.error_calc_method = INCCErrorCalculationTechnique(results.error_calc_method);
            acq.campaign_id = TransferUtils.str(results.results_campaign_id, INCC.MAX_CAMPAIGN_ID_LENGTH);
            if (string.IsNullOrEmpty(acq.campaign_id))
                acq.campaign_id = TransferUtils.str(results.results_inspection_number, INCC.MAX_CAMPAIGN_ID_LENGTH);
			acq.comment = TransferUtils.str(results.comment, INCC.MAX_COMMENT_LENGTH);//"Original file name " + meas.MeasurementId.FileName;
            if (string.IsNullOrEmpty(acq.comment))
			{
				acq.comment = TransferUtils.str(results.ending_comment, INCC.MAX_COMMENT_LENGTH);
				acq.ending_comment = !string.IsNullOrEmpty(acq.comment); // NEXT: nowhere to save the ending comment AND the standard comment
			}

            mlogger.TraceEvent(LogLevels.Verbose, 34000, "Building {0} measurement {1} '{2},{3}' from {2}", meas.MeasOption.PrintName(), num, acq.detector_id, acq.item_type, itf.Path);

            if (itf.facility_table.Count > 0)
                meas.AcquireState.facility = new INCCDB.Descriptor(string.Copy(itf.facility_table[0].id), string.Copy(itf.facility_table[0].desc));
            if (itf.mba_table.Count > 0)
                meas.AcquireState.mba = new INCCDB.Descriptor(string.Copy(itf.mba_table[0].id), string.Copy(itf.mba_table[0].desc));
            if (itf.stratum_id_names_rec_table.Count > 0)
                meas.AcquireState.stratum_id = new INCCDB.Descriptor(string.Copy(itf.stratum_id_names_rec_table[0].id), string.Copy(itf.stratum_id_names_rec_table[0].desc));

            // stratum values
            meas.Stratum = new Stratum();
            meas.Stratum.bias_uncertainty = results.bias_uncertainty;
            meas.Stratum.random_uncertainty = results.random_uncertainty;
            meas.Stratum.relative_std_dev = results.relative_std_dev;
            meas.Stratum.systematic_uncertainty = results.systematic_uncertainty;

            INCCDB.Descriptor mtdesc = new INCCDB.Descriptor(string.Copy(acq.item_type), string.Empty);
            if (!NC.App.DB.Materials.Has(mtdesc) || overwrite)
            {
                NC.App.DB.Materials.Update(mtdesc);
            }

            // prepare norm parms, should be done with a prior pass with the detector file, then this code looks it up in the collection set at this point in the processing
            meas.Norm.currNormalizationConstant.v = results.normalization_constant;
            meas.Norm.currNormalizationConstant.err = results.normalization_constant_err;

            foreach (DescriptorPair dp in itf.facility_table)
            {
                INCCDB.Descriptor idesc = new INCCDB.Descriptor(string.Copy(dp.id), string.Copy(dp.desc));
                if (!NC.App.DB.Facilities.Has(idesc) || overwrite)
                {
                    idesc.modified = true;
                    NC.App.DB.Facilities.Update(idesc);
                }
            }
            foreach (DescriptorPair dp in itf.mba_table)
            {
                INCCDB.Descriptor idesc = new INCCDB.Descriptor(string.Copy(dp.id), string.Copy(dp.desc));
                if (!NC.App.DB.MBAs.Has(idesc) || overwrite)
                {
                    idesc.modified = true;
                    NC.App.DB.MBAs.Update(idesc);
                }
            }
            foreach (DescriptorPair dp in itf.stratum_id_names_rec_table)
            {
                INCCDB.Descriptor idesc = new INCCDB.Descriptor(string.Copy(dp.id), string.Copy(dp.desc));
                if (meas.Stratum != null) 
                {
                   idesc.modified = true;
                   NC.App.DB.UpdateStratum(idesc, meas.Stratum);  // creates it
                   NC.App.DB.AssociateStratum(det, idesc, meas.Stratum); // associates it with the detector
                }
            }

            if (itf.item_id_table.Count > 0)  // todo: use an assert here, there should be only one ever
            {
                ItemId item = new ItemId();
                item.declaredMass = results.declared_mass;
                item.declaredUMass = results.declared_u_mass;
                item.length = results.length;
                item.IOCode = TransferUtils.str(results.io_code, INCC.IO_CODE_LENGTH);
                item.inventoryChangeCode = TransferUtils.str(results.inventory_change_code, INCC.INVENTORY_CHG_LENGTH);
                item.isotopics = TransferUtils.str(results.item_isotopics_id, INCC.MAX_ISOTOPICS_ID_LENGTH);
                item.stratum = TransferUtils.str(results.stratum_id, INCC.MAX_STRATUM_ID_LENGTH);
                item.item = TransferUtils.str(results.item_id, INCC.MAX_ITEM_ID_LENGTH);
                item.material = TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
                //copy measurement dates to item
                // JFL this is a deep copy of the date
                item.pu_date = new DateTime(meas.Isotopics.pu_date.Ticks);
                item.am_date = new DateTime(meas.Isotopics.am_date.Ticks);
                item.modified = true;

                List<ItemId> list = NC.App.DB.ItemIds.GetList();
				bool flump = list.Exists(i => { return string.Compare(item.item, i.item, true) == 0; });
                if (flump && overwrite)
                {
                    list.Remove(item);
                    list.Add(item);
                }
                else
                    list.Add(item);

                // fill in the acquire record from the item id
                acq.ApplyItemId(item);

                meas.MeasurementId.Item = new ItemId(item);
            }

            meas.INCCAnalysisState = new INCCAnalysisState();

            INCCSelector sel = new INCCSelector(acq.detector_id, acq.item_type);
            AnalysisMethods am;
            bool found = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out am);
            if (found)
            {
                meas.INCCAnalysisState.Methods = am;
                meas.INCCAnalysisState.Methods.selector = sel;
            }
            else
            {
                mlogger.TraceEvent(LogLevels.Error, 34063, "No analysis methods for {0}, (calibration information is missing), creating placeholders", sel.ToString()); // JFL dev note: can get missing paramters from the meas results for calib and verif below, so need to visit this condition after results processing below (newres.methodParams!) and reconstruct the calib parameters. 
                meas.INCCAnalysisState.Methods = new AnalysisMethods(mlogger);
                meas.INCCAnalysisState.Methods.selector = sel;
            }
            // prepare analyzer params from sr params above
            meas.AnalysisParams = new AnalysisDefs.CountingAnalysisParameters();
            meas.AnalysisParams.Add(det.MultiplicityParams);

            mlogger.TraceEvent(LogLevels.Verbose, 34030, "Transferring the {0} cycles", itf.run_rec_list.Count);
            meas.InitializeContext();
            meas.PrepareINCCResults(); // prepares INCCResults objects
            foreach (run_rec r in itf.run_rec_list)
            {
                AddToCycleList(r, det);  
            }
            for (int cf = 1; (itf.CFrun_rec_list != null) && (cf < itf.CFrun_rec_list.Length); cf++)
            {
                foreach (run_rec r in itf.CFrun_rec_list[cf])
                {
                    AddToCycleList(r, det, cf);
                }
            }

            // summarize the result in the result, if you know what I mean
            MultiplicityCountingRes mcr = (MultiplicityCountingRes)meas.CountingAnalysisResults[det.MultiplicityParams];
            for (int i = 0; i < 9; i++)
                mcr.covariance_matrix[i] = results.covariance_matrix[i];
            mcr.DeadtimeCorrectedRates.Singles.err = results.singles_err;
            mcr.DeadtimeCorrectedRates.Doubles.err = results.doubles_err;
            mcr.DeadtimeCorrectedRates.Triples.err = results.triples_err;
            mcr.DeadtimeCorrectedRates.Singles.v = results.singles;
            mcr.DeadtimeCorrectedRates.Doubles.v = results.doubles;
            mcr.DeadtimeCorrectedRates.Triples.v = results.triples;
            mcr.Scaler1.v = results.scaler1;
            mcr.Scaler2.v = results.scaler2;
            mcr.Scaler1.err = results.scaler1_err;
            mcr.Scaler2.err = results.scaler2_err; 
            mcr.ASum = results.acc_sum;
            mcr.RASum = results.reals_plus_acc_sum;
            mcr.S1Sum = results.scaler1_sum;
            mcr.S2Sum = results.scaler2_sum;
            mcr.mass = results.declared_mass;
            mcr.FA = det.MultiplicityParams.FA;
            mcr.RAMult = TransferUtils.multarrayxfer(results.mult_reals_plus_acc_sum, INCC.MULTI_ARRAY_SIZE);
            mcr.NormedAMult = TransferUtils.multarrayxfer(results.mult_acc_sum, INCC.MULTI_ARRAY_SIZE);
            mcr.MaxBins = (ulong)Math.Max(mcr.RAMult.Length, mcr.NormedAMult.Length);
            mcr.MinBins = (ulong)Math.Min(mcr.RAMult.Length, mcr.NormedAMult.Length);
            // dunno yet mcr.SemiCorrectedRates.v =  results.singles_multi;
            mcr.singles_multi = results.singles_multi;
            mcr.doubles_multi = results.doubles_multi;
            mcr.triples_multi = results.triples_multi;

            INCCResult result;
            MeasOptionSelector mos = new MeasOptionSelector(meas.MeasOption, det.MultiplicityParams);
            result = meas.INCCAnalysisState.Lookup(mos);
            result.DeadtimeCorrectedRates.Singles.err = results.singles_err;
            result.DeadtimeCorrectedRates.Doubles.err = results.doubles_err;
            result.DeadtimeCorrectedRates.Triples.err = results.triples_err;
            result.DeadtimeCorrectedRates.Singles.v = results.singles;
            result.DeadtimeCorrectedRates.Doubles.v = results.doubles;
            result.DeadtimeCorrectedRates.Triples.v = results.triples;
            result.Scaler1.v = results.scaler1;
            result.Scaler2.v = results.scaler2;
            result.Scaler1.err = results.scaler1_err;
            result.Scaler2.err = results.scaler2_err;
            result.S1Sum = results.scaler1_sum;
            result.S2Sum = results.scaler2_sum;
            result.ASum = results.acc_sum;
            result.RASum = results.reals_plus_acc_sum;
            result.RAMult = TransferUtils.multarrayxfer(results.mult_reals_plus_acc_sum, INCC.MULTI_ARRAY_SIZE);
            result.NormedAMult = TransferUtils.multarrayxfer(results.mult_acc_sum, INCC.MULTI_ARRAY_SIZE);
            result.MaxBins = (ulong)Math.Max(result.RAMult.Length, result.NormedAMult.Length);
            result.MinBins = (ulong)Math.Min(result.RAMult.Length, result.NormedAMult.Length);
            result.singles_multi = results.singles_multi;
            result.doubles_multi = results.doubles_multi;
            result.triples_multi = results.triples_multi;

            List<MeasurementMsg> msgs = meas.GetMessageList(det.MultiplicityParams);

            // move the error messages
            for (int i = 0; i < INCC.NUM_ERROR_MSG_CODES; i++)
            {
                int index = i * INCC.ERR_MSG_LENGTH;
                string e = TransferUtils.str(results.error_msg_codes + index, INCC.ERR_MSG_LENGTH);
                if (e.Length > 0)
                    meas.AddMessage(msgs, LogLevels.Error, 911, e, meas.MeasurementId.MeasDateTime);
            }
            // move the warning messages
            for (int i = 0; i < INCC.NUM_WARNING_MSG_CODES; i++)
            {
                int index = i * INCC.ERR_MSG_LENGTH;
                string w = TransferUtils.str(results.warning_msg_codes + index, INCC.ERR_MSG_LENGTH);
                if (w.Length > 0)
                    meas.AddMessage(msgs, LogLevels.Warning, 411, w, meas.MeasurementId.MeasDateTime);
            }

            #region results transfer
            INCCMethodResults imr;
            bool got = meas.INCCAnalysisResults.TryGetINCCResults(det.MultiplicityParams, out imr);
            if (got)
                imr.primaryMethod = OldToNewMethodId(results.primary_analysis_method);
            // check these results against the meas.MeasOption expectation => seems to always be 1 - 1 with (opt, sr) -> results, and subresults only for verif and calib choice
            //         rates -> none, 
            //         bkg -> bkg, norm -> bias, init src -> init src, prec ->  prec, 
            //         calib -> calib, 1 or more INCC methods on the methods submap
            //         verif -> 1 or more INCC methods on the methods submap
            foreach (iresultsbase r in itf.method_results_list)
            {
                if (r is results_init_src_rec)
                {
                    mlogger.TraceEvent(LogLevels.Verbose, 34041, ("Transferring initial source results"));

                    results_init_src_rec oldres = (results_init_src_rec)r;

                    INCCResults.results_init_src_rec newres = (INCCResults.results_init_src_rec)
                    meas.INCCAnalysisState.Lookup(new MeasOptionSelector(meas.MeasOption, det.MultiplicityParams), typeof(INCCResults.results_init_src_rec));

                    newres.mode = OldToNewBiasTestId(oldres.init_src_mode);
                    newres.pass = TransferUtils.PassCheck(oldres.init_src_pass_fail);
                    newres.init_src_id = TransferUtils.str(oldres.init_src_id, INCC.SOURCE_ID_LENGTH);
                }
                else if (r is results_bias_rec)
                {
                    mlogger.TraceEvent(LogLevels.Verbose, 34042, ("Transferring normalization results"));

                    results_bias_rec oldres = (results_bias_rec)r;
                    INCCResults.results_bias_rec newres = (INCCResults.results_bias_rec)meas.INCCAnalysisState.Lookup(new MeasOptionSelector(meas.MeasOption, det.MultiplicityParams), typeof(INCCResults.results_bias_rec));

                    newres.pass = TransferUtils.PassCheck(oldres.bias_pass_fail);
                    newres.biasDblsRateExpect.v = oldres.bias_dbls_rate_expect;
                    newres.biasDblsRateExpectMeas.v = oldres.bias_dbls_rate_expect_meas;
                    newres.biasSnglsRateExpect.v = oldres.bias_sngls_rate_expect;
                    newres.biasSnglsRateExpectMeas.v = oldres.bias_sngls_rate_expect_meas;
                    newres.biasDblsRateExpect.err = oldres.bias_dbls_rate_expect_err;
                    newres.biasDblsRateExpectMeas.err = oldres.bias_dbls_rate_expect_meas_err;
                    newres.biasSnglsRateExpect.err = oldres.bias_sngls_rate_expect_err;
                    newres.biasSnglsRateExpectMeas.err = oldres.bias_sngls_rate_expect_meas_err;
                    newres.newNormConstant.v = oldres.new_norm_constant;
                    newres.newNormConstant.err = oldres.new_norm_constant_err;
                    newres.measPrecision = oldres.meas_precision;
                    newres.requiredMeasSeconds = oldres.required_meas_seconds;
                    newres.requiredPrecision = oldres.required_precision;
                    newres.mode = OldToNewBiasTestId(oldres.results_bias_mode);
                    newres.sourceId = TransferUtils.str(oldres.bias_source_id, INCC.SOURCE_ID_LENGTH);
                      // NEXT: for init src and bias, results norm values transferred to meas.norm

                }
                else if (r is results_precision_rec)
                {
                    mlogger.TraceEvent(LogLevels.Verbose, 34043, ("Transferring precision results"));

                    results_precision_rec oldres = (results_precision_rec)r;
                    INCCResults.results_precision_rec newres = 
                        (INCCResults.results_precision_rec)meas.INCCAnalysisState.Lookup(new MeasOptionSelector(meas.MeasOption, det.MultiplicityParams), typeof(INCCResults.results_precision_rec));

                    newres.chiSqLowerLimit = oldres.chi_sq_lower_limit;
                    newres.chiSqUpperLimit = oldres.chi_sq_upper_limit;
                    newres.precChiSq = oldres.prec_chi_sq;
                    newres.precSampleVar = oldres.prec_sample_var;
                    newres.precTheoreticalVar = oldres.prec_theoretical_var;
                    newres.pass = TransferUtils.PassCheck(oldres.prec_pass_fail);
                }
                else
                {
                    if (r is results_cal_curve_rec)
                    {
                        // need to look up in existing map and see if it is there and then create and load it if not
                        mlogger.TraceEvent(LogLevels.Verbose, 34050, ("Transferring method results for " + r.GetType().ToString()));
                        results_cal_curve_rec oldres = (results_cal_curve_rec)r;
                        INCCMethodResults.results_cal_curve_rec newres = 
                            (INCCMethodResults.results_cal_curve_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.CalibrationCurve, true);

                        newres.pu240e_mass = new Tuple(oldres.cc_pu240e_mass, oldres.cc_pu240e_mass_err);
                        newres.pu_mass = new Tuple(oldres.cc_pu_mass, oldres.cc_pu_mass_err);
                        newres.dcl_pu240e_mass = oldres.cc_dcl_pu240e_mass;
                        newres.dcl_pu_mass = oldres.cc_dcl_pu_mass;
                        newres.dcl_minus_asy_pu_mass = new Tuple(oldres.cc_dcl_minus_asy_pu_mass, oldres.cc_dcl_minus_asy_pu_mass_err);
                        newres.dcl_minus_asy_pu_mass_pct = oldres.cc_dcl_minus_asy_pu_mass_pct;
                        newres.pass = TransferUtils.PassCheck(oldres.cc_pass_fail);
                        newres.dcl_u_mass = oldres.cc_dcl_u_mass;
                        newres.length = oldres.cc_length;
                        newres.heavy_metal_content = oldres.cc_heavy_metal_content;
                        newres.heavy_metal_correction = oldres.cc_heavy_metal_correction;
                        newres.heavy_metal_corr_singles = new Tuple(oldres.cc_heavy_metal_corr_singles, oldres.cc_heavy_metal_corr_singles_err);
                        newres.heavy_metal_corr_doubles = new Tuple(oldres.cc_heavy_metal_corr_doubles, oldres.cc_heavy_metal_corr_doubles_err);
                        newres.methodParams.heavy_metal_corr_factor = oldres.cc_heavy_metal_corr_factor_res;
                        newres.methodParams.heavy_metal_reference = oldres.cc_heavy_metal_reference_res;
                        // newres.methodParams.cev.lower_mass_limit = oldres.cc_lower_mass_limit_res;
                        // newres.methodParams.cev.upper_mass_limit = oldres.cc_upper_mass_limit_res;
                        newres.methodParams.percent_u235 = oldres.cc_percent_u235_res;

                        newres.methodParams.cev.a = oldres.cc_a_res; newres.methodParams.cev.b = oldres.cc_b_res;
                        newres.methodParams.cev.c = oldres.cc_c_res; newres.methodParams.cev.d = oldres.cc_d_res;
                        newres.methodParams.cev.var_a = oldres.cc_var_a_res; newres.methodParams.cev.var_b = oldres.cc_var_b_res;
                        newres.methodParams.cev.var_c = oldres.cc_var_c_res; newres.methodParams.cev.var_d = oldres.cc_var_d_res;
                        newres.methodParams.cev.setcovar(Coeff.a,Coeff.b,oldres.cc_covar_ab_res);
                        newres.methodParams.cev._covar[0, 2] = oldres.cc_covar_ac_res;
                        newres.methodParams.cev._covar[0, 3] = oldres.cc_covar_ad_res;
                        newres.methodParams.cev._covar[1, 2] = oldres.cc_covar_bc_res;
                        newres.methodParams.cev._covar[1, 3] = oldres.cc_covar_bd_res;
                        newres.methodParams.cev._covar[2, 3] = oldres.cc_covar_cd_res;
                        newres.methodParams.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.cc_cal_curve_equation;
                        newres.methodParams.CalCurveType = (INCCAnalysisParams.CalCurveType)oldres.cc_cal_curve_type_res;
                        newres.methodParams.cev.sigma_x = oldres.cc_sigma_x_res;
                    }
                    else if (r is results_known_alpha_rec)
                    {
                        //  need to look up in existing map and see if it is there and then create and load it if not
                        mlogger.TraceEvent(LogLevels.Verbose, 34051, ("Transferring method results for " + r.GetType().ToString()));
                        results_known_alpha_rec oldres = (results_known_alpha_rec)r;
                        INCCMethodResults.results_known_alpha_rec newres = 
                            (INCCMethodResults.results_known_alpha_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownA, true);
                        newres.corr_doubles.v = oldres.ka_corr_doubles;
                        newres.corr_doubles.err = oldres.ka_corr_doubles_err;
                        newres.mult = oldres.ka_mult;
                        newres.alphaK = oldres.ka_alpha;
                        newres.mult_corr_doubles = new Tuple(oldres.ka_mult_corr_doubles, oldres.ka_mult_corr_doubles_err);
                        newres.pu240e_mass = new Tuple(oldres.ka_pu240e_mass, oldres.ka_pu240e_mass_err);
                        newres.pu_mass = new Tuple(oldres.ka_pu_mass, oldres.ka_pu_mass_err);
                        newres.dcl_pu240e_mass = oldres.ka_dcl_pu240e_mass;
                        newres.dcl_pu_mass = oldres.ka_dcl_pu_mass;
                        newres.dcl_minus_asy_pu_mass = new Tuple(oldres.ka_dcl_minus_asy_pu_mass, oldres.ka_dcl_minus_asy_pu_mass_err);
                        newres.dcl_minus_asy_pu_mass_pct = oldres.ka_dcl_minus_asy_pu_mass_pct;
                        newres.pass = TransferUtils.PassCheck(oldres.ka_pass_fail);
                        newres.dcl_u_mass = oldres.ka_dcl_u_mass;
                        newres.length = oldres.ka_length;
                        newres.heavy_metal_content = oldres.ka_heavy_metal_content;
                        newres.heavy_metal_correction = oldres.ka_heavy_metal_correction;
                        newres.corr_singles = new Tuple(oldres.ka_corr_singles, oldres.ka_corr_singles_err);
                        newres.corr_doubles = new Tuple(oldres.ka_corr_doubles, oldres.ka_corr_doubles_err);
                        newres.corr_factor = oldres.ka_corr_factor;
                        newres.dry_alpha_or_mult_dbls = oldres.ka_dry_alpha_or_mult_dbls;
                        newres.upper_corr_factor_limit = oldres.ka_upper_corr_factor_limit_res;
                        newres.lower_corr_factor_limit = oldres.ka_lower_corr_factor_limit_res;

                        newres.methodParams = new INCCAnalysisParams.known_alpha_rec();
                        newres.methodParams.alpha_wt = oldres.ka_alpha_wt_res;
                        newres.methodParams.heavy_metal_corr_factor = oldres.ka_heavy_metal_corr_factor_res;
                        newres.methodParams.heavy_metal_reference = oldres.ka_heavy_metal_reference_res;
                        newres.methodParams.k = oldres.ka_k_res;
                        newres.methodParams.known_alpha_type = (INCCAnalysisParams.KnownAlphaVariant)oldres.ka_known_alpha_type_res;
                        newres.methodParams.lower_corr_factor_limit = oldres.ka_lower_corr_factor_limit_res;
                        newres.methodParams.upper_corr_factor_limit = oldres.ka_upper_corr_factor_limit_res;
                        newres.methodParams.rho_zero = oldres.ka_rho_zero_res;

                        newres.methodParams.ring_ratio.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.ka_ring_ratio_equation_res;
                        newres.methodParams.ring_ratio.a = oldres.ka_ring_ratio_a_res;
                        newres.methodParams.ring_ratio.b = oldres.ka_ring_ratio_b_res;
                        newres.methodParams.ring_ratio.c = oldres.ka_ring_ratio_c_res;
                        newres.methodParams.ring_ratio.d = oldres.ka_ring_ratio_d_res;

                        newres.methodParams.cev.a = oldres.ka_a_res;
                        newres.methodParams.cev.b = oldres.ka_b_res;
                        newres.methodParams.cev.var_a = oldres.ka_var_a_res;
                        newres.methodParams.cev.var_b= oldres.ka_var_b_res;
                        newres.methodParams.cev.sigma_x = oldres.ka_sigma_x_res;
                        newres.methodParams.cev.setcovar(Coeff.a, Coeff.b, oldres.ka_covar_ab_res);
                        newres.methodParams.cev.lower_mass_limit = oldres.ka_lower_corr_factor_limit_res;
                        newres.methodParams.cev.upper_mass_limit = oldres.ka_upper_corr_factor_limit_res;
                    }
                    else if (r is results_multiplicity_rec)
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34052, ("Transferring method results for " + r.GetType().ToString()));
                        results_multiplicity_rec oldres = (results_multiplicity_rec)r;
                        INCCMethodResults.results_multiplicity_rec newres = 
                            (INCCMethodResults.results_multiplicity_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.Multiplicity, true);

                        newres.solve_efficiency_choice = (INCCAnalysisParams.MultChoice)oldres.mul_solve_efficiency_res;
                        newres.pass = TransferUtils.PassCheck(oldres.mul_pass_fail);
                        newres.efficiencyComputed.v = oldres.mul_efficiency; newres.efficiencyComputed.err = oldres.mul_efficiency_err;
                        newres.mult.v = oldres.mul_mult; newres.mult.err = oldres.mul_mult_err;
                        newres.alphaK.v = oldres.mul_alpha; newres.alphaK.err = oldres.mul_alpha_err;
                        newres.corr_factor.v = oldres.mul_corr_factor; newres.corr_factor.err = oldres.mul_corr_factor_err;
                        newres.pu240e_mass.v = oldres.mul_pu240e_mass; newres.pu240e_mass.err = oldres.mul_pu240e_mass_err;
                        newres.pu_mass.v = oldres.mul_pu_mass; newres.pu_mass.err = oldres.mul_pu_mass_err;
                        newres.dcl_minus_asy_pu_mass.v = oldres.mul_dcl_minus_asy_pu_mass; newres.dcl_minus_asy_pu_mass.err = oldres.mul_dcl_minus_asy_pu_mass_err;
                        newres.dcl_pu240e_mass = oldres.mul_dcl_pu240e_mass;
                        newres.dcl_pu_mass = oldres.mul_dcl_pu_mass;
                        newres.dcl_minus_asy_pu_mass_pct = oldres.mul_dcl_minus_asy_pu_mass_pct;

                        newres.methodParams = new INCCAnalysisParams.multiplicity_rec();
                        newres.methodParams.sf_rate = oldres.mul_sf_rate_res;
                        newres.methodParams.vs1 = oldres.mul_vs1_res;
                        newres.methodParams.vs2 = oldres.mul_vs2_res;
                        newres.methodParams.vs3 = oldres.mul_vs3_res;
                        newres.methodParams.vi1 = oldres.mul_vi1_res;
                        newres.methodParams.vi2 = oldres.mul_vi2_res;
                        newres.methodParams.vi3 = oldres.mul_vi3_res;
                        newres.methodParams.a = oldres.mul_a_res;
                        newres.methodParams.b = oldres.mul_b_res;
                        newres.methodParams.c = oldres.mul_b_res;
                        newres.methodParams.sigma_x = oldres.mul_sigma_x_res;
                        newres.methodParams.alpha_weight = oldres.mul_alpha_weight_res;
                        newres.methodParams.multEffCorFactor = oldres.mul_corr_factor;
                    }
                    else if (r is results_truncated_mult_rec)
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34053, ("Transferring method results for " + r.GetType().ToString()));
                        results_truncated_mult_rec oldres = (results_truncated_mult_rec)r;
                        INCCMethodResults.results_truncated_mult_rec newres = 
                            (INCCMethodResults.results_truncated_mult_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.TruncatedMultiplicity, true);
                        newres.k.pass = TransferUtils.PassCheck(oldres.tm_k_pass_fail);
                        newres.bkg.Singles.v = oldres.tm_bkg_singles;
                        newres.bkg.Singles.err = oldres.tm_bkg_singles_err;
                        newres.bkg.Zeros.v = oldres.tm_bkg_zeros;
                        newres.bkg.Zeros.err = oldres.tm_bkg_zeros_err;
                        newres.bkg.Ones.v = oldres.tm_bkg_ones;
                        newres.bkg.Ones.err = oldres.tm_bkg_ones_err;
                        newres.bkg.Twos.v = oldres.tm_bkg_twos;
                        newres.bkg.Twos.err = oldres.tm_bkg_twos_err;
                        newres.net.Singles.v = oldres.tm_net_singles;
                        newres.net.Singles.err = oldres.tm_net_singles_err;
                        newres.net.Zeros.v = oldres.tm_net_zeros;
                        newres.net.Zeros.err = oldres.tm_net_zeros_err;
                        newres.net.Ones.v = oldres.tm_net_ones;
                        newres.net.Ones.err = oldres.tm_net_ones_err;
                        newres.net.Twos.v = oldres.tm_net_twos;
                        newres.net.Twos.err = oldres.tm_net_twos_err;
                        newres.k.alpha.v = oldres.tm_k_alpha;
                        newres.k.alpha.err = oldres.tm_k_alpha_err;
                        newres.k.pu_mass.v = oldres.tm_k_pu_mass;
                        newres.k.pu_mass.err = oldres.tm_k_pu_mass_err;
                        newres.k.pu240e_mass.v = oldres.tm_k_pu240e_mass;
                        newres.k.pu240e_mass.err = oldres.tm_k_pu240e_mass_err;
                        newres.k.dcl_minus_asy_pu_mass.v = oldres.tm_k_dcl_minus_asy_pu_mass;
                        newres.k.dcl_minus_asy_pu_mass.err = oldres.tm_k_dcl_minus_asy_pu_mass_err;
                        newres.s.alpha.v = oldres.tm_s_alpha;
                        newres.s.alpha.err = oldres.tm_s_alpha_err;
                        newres.s.pu_mass.v = oldres.tm_s_pu_mass;
                        newres.s.pu_mass.err = oldres.tm_s_pu_mass_err;
                        newres.s.pu240e_mass.v = oldres.tm_s_pu240e_mass;
                        newres.s.pu240e_mass.err = oldres.tm_s_pu240e_mass_err;
                        newres.s.dcl_minus_asy_pu_mass.v = oldres.tm_s_dcl_minus_asy_pu_mass;
                        newres.s.dcl_minus_asy_pu_mass.err = oldres.tm_s_dcl_minus_asy_pu_mass_err;
                        newres.methodParams.a = oldres.tm_a_res;
                        newres.methodParams.b = oldres.tm_b_res;
                        newres.methodParams.known_eff = (oldres.tm_known_eff_res == 0 ? false : true);
                        newres.methodParams.solve_eff = (oldres.tm_solve_eff_res == 0 ? false : true);
                    }
                    else if (r is results_known_m_rec)
                    {
                        // dev note: untested
                        mlogger.TraceEvent(LogLevels.Verbose, 34054, ("Transferring method results for " + r.GetType().ToString()));
                        results_known_m_rec oldres = (results_known_m_rec)r;
                        INCCMethodResults.results_known_m_rec newres = 
                            (INCCMethodResults.results_known_m_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownM, true);
                        newres.pu_mass = new Tuple(oldres.km_pu_mass, oldres.km_pu_mass_err);
                        newres.pu240e_mass = new Tuple(oldres.km_pu240e_mass, oldres.km_pu240e_mass);
                        newres.alpha = oldres.km_alpha;
                        newres.mult = oldres.km_mult;
                        newres.dcl_minus_asy_pu_mass = new Tuple(oldres.km_dcl_minus_asy_pu_mass, oldres.km_dcl_minus_asy_pu_mass_err);
                        newres.pu239e_mass = oldres.km_pu240e_mass;
                        newres.dcl_pu240e_mass = oldres.km_dcl_pu240e_mass;
                        newres.dcl_pu_mass = oldres.km_dcl_pu_mass;
                        newres.dcl_minus_asy_pu_mass_pct = oldres.km_dcl_minus_asy_pu_mass_pct;
                        newres.pass = TransferUtils.PassCheck(oldres.km_pass_fail);
                        newres.methodParams.b = oldres.km_b_res;
                        newres.methodParams.c = oldres.km_c_res;
                        newres.methodParams.sf_rate = oldres.km_sf_rate_res;
                        newres.methodParams.sigma_x = oldres.km_sigma_x_res;
                        newres.methodParams.vs1 = oldres.km_vs1_res;
                        newres.methodParams.vi1 = oldres.km_vi1_res;
                        newres.methodParams.vs2 = oldres.km_vs2_res;
                        newres.methodParams.vi2 = oldres.km_vi2_res;
                    }
                    else if (r is results_add_a_source_rec)
                    {
                        // dev note: untested
                        mlogger.TraceEvent(LogLevels.Verbose, 34055, ("Transferring method results for " + r.GetType().ToString()));
                        results_add_a_source_rec oldres = (results_add_a_source_rec)r;
                        INCCMethodResults.results_add_a_source_rec newres = 
                            (INCCMethodResults.results_add_a_source_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownM, true);
                        newres.pu_mass = new Tuple(oldres.ad_pu_mass, oldres.ad_pu_mass_err);
                        newres.pu240e_mass = new Tuple(oldres.ad_pu240e_mass, oldres.ad_pu240e_mass);
                        newres.dcl_minus_asy_pu_mass = new Tuple(oldres.ad_dcl_minus_asy_pu_mass, oldres.ad_dcl_minus_asy_pu_mass_err);
                        newres.dcl_pu240e_mass = oldres.ad_dcl_pu240e_mass;
                        newres.dcl_pu_mass = oldres.ad_dcl_pu_mass;
                        newres.dcl_minus_asy_pu_mass_pct = oldres.ad_dcl_minus_asy_pu_mass_pct;
                        newres.pass = TransferUtils.PassCheck(oldres.ad_pass_fail);

                        newres.dzero_cf252_doubles = oldres.ad_dzero_cf252_doubles;
                        newres.sample_avg_cf252_doubles.v = oldres.ad_sample_avg_cf252_doubles;
                        newres.sample_avg_cf252_doubles.err = oldres.ad_sample_avg_cf252_doubles_err;
                        newres.corr_doubles.v = oldres.ad_corr_doubles;
                        newres.corr_doubles.err = oldres.ad_corr_doubles_err;
                        newres.delta.v = oldres.ad_delta;
                        newres.delta.err = oldres.ad_delta_err;
                        newres.corr_factor.v = oldres.ad_corr_factor;
                        newres.corr_factor.err = oldres.ad_corr_factor_err;
                        newres.sample_cf252_ratio = TransferUtils.Copy(oldres.ad_sample_cf252_ratio, INCC.MAX_ADDASRC_POSITIONS);
                        newres.sample_cf252_doubles = Tuple.MakeArray(
                                vals: TransferUtils.Copy(oldres.ad_sample_cf252_doubles, INCC.MAX_ADDASRC_POSITIONS),
                                errs: TransferUtils.Copy(oldres.ad_sample_cf252_doubles_err, INCC.MAX_ADDASRC_POSITIONS));
                        newres.tm_doubles_bkg.v = oldres.ad_tm_doubles_bkg;
                        newres.tm_uncorr_doubles.v = oldres.ad_tm_uncorr_doubles;
                        newres.tm_corr_doubles.v = oldres.ad_corr_doubles;
                        newres.tm_doubles_bkg.err = oldres.ad_tm_doubles_bkg_err;
                        newres.tm_uncorr_doubles.err = oldres.ad_tm_uncorr_doubles_err;
                        newres.tm_corr_doubles.err = oldres.ad_corr_doubles_err;
                        //newres.methodParams; // NEXT: unfinished
                    }
                    else if (r is results_curium_ratio_rec)
                    {
                        // dev note: untested
                        mlogger.TraceEvent(LogLevels.Verbose, 34056, ("Transferring method results for " + r.GetType().ToString()));
                        results_curium_ratio_rec oldres = (results_curium_ratio_rec)r;
                        INCCMethodResults.results_curium_ratio_rec newres = (INCCMethodResults.results_curium_ratio_rec)
                        meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.CuriumRatio, true);
                        newres.pu.mass = new Tuple(oldres.cr_pu_mass, oldres.cr_pu_mass_err);
                        newres.pu.dcl_mass = oldres.cr_dcl_pu_mass;
                        newres.pu.dcl_minus_asy_mass = new Tuple(oldres.cr_dcl_minus_asy_pu_mass, oldres.cr_dcl_minus_asy_pu_mass_err);
                        newres.pu.dcl_minus_asy_mass_pct = oldres.cr_dcl_minus_asy_pu_mass_pct;
                        newres.pu.pass = TransferUtils.PassCheck(oldres.cr_pu_pass_fail);
                      
                        newres.u.mass = new Tuple(oldres.cr_u_mass, oldres.cr_u_mass_err);
                        newres.u.dcl_mass = oldres.cr_dcl_u_mass_res;
                        newres.u.dcl_minus_asy_mass = new Tuple(oldres.cr_dcl_minus_asy_u_mass, oldres.cr_dcl_minus_asy_u_mass_err);
                        newres.u.dcl_minus_asy_mass_pct = oldres.cr_dcl_minus_asy_u_mass_pct;
                        newres.u.pass = TransferUtils.PassCheck(oldres.cr_u_pass_fail);
                       
                        newres.u235.mass = new Tuple(oldres.cr_u235_mass, oldres.cr_u235_mass_err);
                        newres.u235.dcl_mass = oldres.cr_dcl_u235_mass_res;
                        newres.u235.dcl_minus_asy_mass = new Tuple(oldres.cr_dcl_minus_asy_u235_mass, oldres.cr_dcl_minus_asy_u235_mass_err);
                        newres.u235.dcl_minus_asy_mass_pct = oldres.cr_dcl_minus_asy_u235_mass_pct;                        
                        
                        newres.cm_mass = new Tuple(oldres.cr_cm_mass, oldres.cr_cm_mass_err);
                        newres.methodParams2.cm_pu_ratio = new Tuple(oldres.cr_cm_pu_ratio, oldres.cr_cm_pu_ratio_err);
                        newres.cm_pu_ratio_decay_corr = new Tuple(oldres.cr_cm_pu_ratio_decay_corr, oldres.cr_cm_pu_ratio_decay_corr_err);
                        newres.methodParams2.cm_pu_ratio_date = INCC.DateFrom(TransferUtils.str(oldres.cr_cm_pu_ratio_date, INCC.DATE_TIME_LENGTH));
                        newres.cm_u_ratio_decay_corr = new Tuple(oldres.cr_cm_u_ratio_decay_corr, oldres.cr_cm_u_ratio_decay_corr_err);
                        newres.methodParams2.cm_u_ratio_date = INCC.DateFrom(TransferUtils.str(oldres.cr_cm_pu_ratio_date, INCC.DATE_TIME_LENGTH));
                        newres.methodParams2.pu_half_life = oldres.cr_pu_half_life;

                        newres.methodParams2.cm_id_label = TransferUtils.str(oldres.cr_cm_id_label, INCC.MAX_ITEM_ID_LENGTH);
                        newres.methodParams2.cm_id = TransferUtils.str(oldres.cr_cm_id, INCC.MAX_ITEM_ID_LENGTH);
                        newres.methodParams2.cm_input_batch_id = TransferUtils.str(oldres.cr_cm_input_batch_id, INCC.MAX_ITEM_ID_LENGTH);

                        newres.methodParams.cev.a = oldres.cr_a_res; newres.methodParams.cev.b = oldres.cr_b_res;
                        newres.methodParams.cev.c = oldres.cr_c_res; newres.methodParams.cev.d = oldres.cr_d_res;
                        newres.methodParams.cev.var_a = oldres.cr_var_a_res; newres.methodParams.cev.var_b = oldres.cr_var_b_res;
                        newres.methodParams.cev.var_c = oldres.cr_var_c_res; newres.methodParams.cev.var_d = oldres.cr_var_d_res;
                        newres.methodParams.cev.setcovar(Coeff.a,Coeff.b,oldres.cr_covar_ab_res);
                        newres.methodParams.cev._covar[0, 2] = oldres.cr_covar_ac_res;
                        newres.methodParams.cev._covar[0, 3] = oldres.cr_covar_ad_res;
                        newres.methodParams.cev._covar[1, 2] = oldres.cr_covar_bc_res;
                        newres.methodParams.cev._covar[1, 3] = oldres.cr_covar_bd_res;
                        newres.methodParams.cev._covar[2, 3] = oldres.cr_covar_cd_res;
                        newres.methodParams.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.cr_curium_ratio_equation;
                        newres.methodParams.cev.sigma_x = oldres.cr_sigma_x_res;
                        newres.methodParams.curium_ratio_type = OldToNewCRVariants(oldres.curium_ratio_type_res);
                    }
                    else if (r is results_active_passive_rec) // NEXT:confusion with combined, it's the same as Active internally? expand and study
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34057, ("Transferring method results for " + r.GetType().ToString()));
                        results_active_passive_rec oldres = (results_active_passive_rec)r;
                        INCCMethodResults.results_active_passive_rec newres =
                            (INCCMethodResults.results_active_passive_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.ActivePassive, true);

                        newres.pass = TransferUtils.PassCheck(oldres.ap_pass_fail);
                        newres.k.v = oldres.ap_k; newres.k.err = oldres.ap_k_err;
                        newres.k0.v = oldres.ap_k0;
                        newres.k1.v = oldres.ap_k1; newres.k1.err = oldres.ap_k1_err;
                        newres.u235_mass.v = oldres.ap_u235_mass; newres.u235_mass.err = oldres.ap_u235_mass_err;
                        newres.dcl_minus_asy_u235_mass.v = oldres.ap_dcl_minus_asy_u235_mass; newres.dcl_minus_asy_u235_mass.err = oldres.ap_dcl_minus_asy_u235_mass_err;

                        newres.dcl_u235_mass = oldres.ap_dcl_u235_mass;
                        newres.dcl_minus_asy_u235_mass_pct = oldres.ap_dcl_minus_asy_u235_mass_pct;

                        newres.methodParams = new INCCAnalysisParams.active_passive_rec();
                        newres.methodParams.cev.a = oldres.ap_a_res; newres.methodParams.cev.b = oldres.ap_b_res;
                        newres.methodParams.cev.c = oldres.ap_c_res; newres.methodParams.cev.d = oldres.ap_d_res;
                        newres.methodParams.cev.var_a = oldres.ap_var_a_res; newres.methodParams.cev.var_b = oldres.ap_var_b_res;
                        newres.methodParams.cev.var_c = oldres.ap_var_c_res; newres.methodParams.cev.var_d = oldres.ap_var_d_res;
                        newres.methodParams.cev.setcovar(Coeff.a, Coeff.b, oldres.ap_covar_ab_res);
                        newres.methodParams.cev._covar[0, 2] = oldres.ap_covar_ac_res;
                        newres.methodParams.cev._covar[0, 3] = oldres.ap_covar_ad_res;
                        newres.methodParams.cev._covar[1, 2] = oldres.ap_covar_bc_res;
                        newres.methodParams.cev._covar[1, 3] = oldres.ap_covar_bd_res;
                        newres.methodParams.cev._covar[2, 3] = oldres.ap_covar_cd_res;
                        newres.methodParams.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.ap_active_passive_equation;
                        newres.methodParams.cev.sigma_x = oldres.ap_sigma_x_res;
                        newres.delta_doubles.v = oldres.ap_delta_doubles;
                        newres.delta_doubles.err = oldres.ap_delta_doubles_err;
                    }
                    else if (r is results_active_rec) {
                        mlogger.TraceEvent(LogLevels.Verbose, 34058, ("Transferring method results for " + r.GetType().ToString()));
                        results_active_rec oldres = (results_active_rec)r;
                        INCCMethodResults.results_active_rec newres =
                            (INCCMethodResults.results_active_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.Active, true);
                        newres.pass = TransferUtils.PassCheck(oldres.act_pass_fail);
                        newres.k.v = oldres.act_k; newres.k.err = oldres.act_k_err;
                        newres.k0.v = oldres.act_k0;
                        newres.k1.v = oldres.act_k1; newres.k1.err = oldres.act_k1_err;
                        newres.u235_mass.v = oldres.act_u235_mass; newres.u235_mass.err = oldres.act_u235_mass_err;
                        newres.dcl_minus_asy_u235_mass.v = oldres.act_dcl_minus_asy_u235_mass; newres.dcl_minus_asy_u235_mass.err = oldres.act_dcl_minus_asy_u235_mass_err;

                        newres.dcl_u235_mass = oldres.act_dcl_u235_mass;
                        newres.dcl_minus_asy_u235_mass_pct = oldres.act_dcl_minus_asy_u235_mass_pct;

                        newres.methodParams = new INCCAnalysisParams.active_rec();
                        newres.methodParams.cev.a = oldres.act_a_res; newres.methodParams.cev.b = oldres.act_b_res;
                        newres.methodParams.cev.c = oldres.act_c_res; newres.methodParams.cev.d = oldres.act_d_res;
                        newres.methodParams.cev.var_a = oldres.act_var_a_res; newres.methodParams.cev.var_b = oldres.act_var_b_res;
                        newres.methodParams.cev.var_c = oldres.act_var_c_res; newres.methodParams.cev.var_d = oldres.act_var_d_res;
                        newres.methodParams.cev.setcovar(Coeff.a,Coeff.b,oldres.act_covar_ab_res);
                        newres.methodParams.cev._covar[0, 2] = oldres.act_covar_ac_res;
                        newres.methodParams.cev._covar[0, 3] = oldres.act_covar_ad_res;
                        newres.methodParams.cev._covar[1, 2] = oldres.act_covar_bc_res;
                        newres.methodParams.cev._covar[1, 3] = oldres.act_covar_bd_res;
                        newres.methodParams.cev._covar[2, 3] = oldres.act_covar_cd_res;
                        newres.methodParams.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.act_active_equation;
                        newres.methodParams.cev.sigma_x = oldres.act_sigma_x_res;
                    }
                    else if (r is results_active_mult_rec) 
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34059, ("Transferring method results for " + r.GetType().ToString()));
                        results_active_mult_rec oldres = (results_active_mult_rec)r;
                        INCCMethodResults.results_active_mult_rec newres =
                            (INCCMethodResults.results_active_mult_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.ActiveMultiplicity, true);
                        newres.methodParams = new INCCAnalysisParams.active_mult_rec();
                        newres.methodParams.vf1 = oldres.am_vf1_res;
                        newres.methodParams.vf2 = oldres.am_vf2_res;
                        newres.methodParams.vf3 = oldres.am_vf3_res;
                        newres.methodParams.vt1 = oldres.am_vt1_res;
                        newres.methodParams.vt2 = oldres.am_vt2_res;
                        newres.methodParams.vt3 = oldres.am_vt2_res;
                        newres.mult.v = oldres.am_mult;
                        newres.mult.err = oldres.am_mult_err;
                     }
                    else if (r is results_collar_rec) 
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34060, ("Transferring method results for " + r.GetType().ToString()));
                        results_collar_rec oldres = (results_collar_rec)r;
                        INCCMethodResults.results_collar_rec newres =
                            (INCCMethodResults.results_collar_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.Collar, true);

                        newres.u235_mass.v = oldres.col_u235_mass; newres.u235_mass.err = oldres.col_u235_mass_err;
                        newres.percent_u235 = oldres.col_percent_u235;
                        newres.total_u_mass = oldres.col_total_u_mass;
                        newres.k0.v = oldres.col_k0; newres.k0.err = oldres.col_k0_err;
                        newres.k1.v = oldres.col_k1; newres.k1.err = oldres.col_k1_err;
                        newres.k2.v = oldres.col_k2; newres.k2.err = oldres.col_k2_err;
                        newres.k3.v = oldres.col_k3; newres.k3.err = oldres.col_k3_err;
                        newres.k4.v = oldres.col_k4; newres.k4.err = oldres.col_k4_err;
                        newres.k5.v = oldres.col_k5; newres.k5.err = oldres.col_k5_err;
                        newres.source_id = TransferUtils.str(oldres.col_source_id, INCC.SOURCE_ID_LENGTH);
                        newres.total_corr_fact.v = oldres.col_total_corr_fact; newres.total_corr_fact.err = oldres.col_total_corr_fact_err;
                        newres.corr_doubles.v = oldres.col_corr_doubles; newres.corr_doubles.err = oldres.col_corr_doubles_err;
                        newres.dcl_length.v = oldres.col_dcl_length; newres.dcl_length.err = oldres.col_dcl_length_err;
                        newres.dcl_total_u235.v = oldres.col_dcl_total_u235; newres.dcl_total_u235.err = oldres.col_dcl_total_u235_err;
                        newres.dcl_total_u238.v = oldres.col_dcl_total_u238; newres.dcl_total_u238.err = oldres.col_dcl_total_u238_err;
                        newres.dcl_total_rods = oldres.col_dcl_total_rods;
                        newres.dcl_total_poison_rods = oldres.col_dcl_total_poison_rods;

                        newres.dcl_poison_percent.v = oldres.col_dcl_poison_percent;
                        newres.dcl_poison_percent.err = oldres.col_dcl_poison_percent_err;
                        newres.dcl_minus_asy_u235_mass_pct = oldres.col_dcl_minus_asy_u235_mass_pct;
                        newres.dcl_minus_asy_u235_mass.v = oldres.col_dcl_minus_asy_u235_mass;
                        newres.dcl_minus_asy_u235_mass.err = oldres.col_dcl_minus_asy_u235_mass_err;

                        //newres.c = new INCCAnalysisParams.collar_rec();
                        newres.methodParamsc.cev.a = oldres.col_a_res; newres.methodParamsc.cev.b = oldres.col_b_res;
                        newres.methodParamsc.cev.c = oldres.col_c_res; newres.methodParamsc.cev.d = oldres.col_d_res;
                        newres.methodParamsc.cev.var_a = oldres.col_var_a_res; newres.methodParamsc.cev.var_b = oldres.col_var_b_res;
                        newres.methodParamsc.cev.var_c = oldres.col_var_c_res; newres.methodParamsc.cev.var_d = oldres.col_var_d_res;
                        newres.methodParamsc.cev.setcovar(Coeff.a,Coeff.b,oldres.col_covar_ab_res);
                        newres.methodParamsc.cev._covar[0, 2] = oldres.col_covar_ac_res;
                        newres.methodParamsc.cev._covar[0, 3] = oldres.col_covar_ad_res;
                        newres.methodParamsc.cev._covar[1, 2] = oldres.col_covar_bc_res;
                        newres.methodParamsc.cev._covar[1, 3] = oldres.col_covar_bd_res;
                        newres.methodParamsc.cev._covar[2, 3] = oldres.col_covar_cd_res;
                        newres.methodParamsc.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.col_collar_equation;
                        //newres.methodParamsc.cev.CalCurveType = (INCCAnalysisParams.CalCurveType)oldres.;
                        newres.methodParamsc.cev.sigma_x = oldres.col_sigma_x_res;
                        newres.methodParamsc.poison_absorption_fact[0] = oldres.col_poison_absorption_fact_res;
                        newres.methodParamsc.poison_rod_a[0] = new Tuple(oldres.col_poison_rod_a_res, oldres.col_poison_rod_a_err_res);
                        newres.methodParamsc.poison_rod_b[0] = new Tuple(oldres.col_poison_rod_b_res, oldres.col_poison_rod_b_err_res);
                        newres.methodParamsc.poison_rod_c[0] = new Tuple(oldres.col_poison_rod_c_res, oldres.col_poison_rod_c_err_res);
                        newres.methodParamsc.collar_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamsc.number_calib_rods = (int)oldres.col_number_calib_rods_res;
                        newres.methodParamsc.poison_rod_type[0] = TransferUtils.str(oldres.col_poison_rod_type_res, INCC.MAX_ROD_TYPE_LENGTH);
                        newres.methodParamsc.u_mass_corr_fact_a.v = oldres.col_u_mass_corr_fact_a_res; newres.methodParamsc.u_mass_corr_fact_a.err = oldres.col_u_mass_corr_fact_a_err_res;
                        newres.methodParamsc.u_mass_corr_fact_b.v = oldres.col_u_mass_corr_fact_b_res; newres.methodParamsc.u_mass_corr_fact_b.err = oldres.col_u_mass_corr_fact_b_err_res;
                        newres.methodParamsc.sample_corr_fact.v = oldres.col_sample_corr_fact_res; newres.methodParamsc.sample_corr_fact.err = oldres.col_sample_corr_fact_err_res;

                        newres.methodParamscd.collar_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamscd.reference_date = INCC.DateFrom(TransferUtils.str(oldres.col_reference_date_res, INCC.DATE_TIME_LENGTH));
                        newres.methodParamscd.relative_doubles_rate = oldres.col_relative_doubles_rate_res;

                        newres.methodParamsck5.k5_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamsck5.k5_item_type = TransferUtils.str(oldres.col_collar_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
                        newres.methodParamsck5.k5 = Copy(oldres.collar_k5_res, oldres.collar_k5_err_res, INCC.MAX_COLLAR_K5_PARAMETERS);
                        newres.methodParamsck5.k5_checkbox = TransferUtils.Copy(oldres.collar_k5_checkbox_res, INCC.MAX_COLLAR_K5_PARAMETERS);
                        for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
                        {
                            int index = i * INCC.MAX_K5_LABEL_LENGTH;
                            newres.methodParamsck5.k5_label[i] = TransferUtils.str(oldres.collar_k5_label_res + index, INCC.MAX_K5_LABEL_LENGTH);
                        }

                        CollarItemId cid = new CollarItemId(TransferUtils.str(results.item_id, INCC.MAX_ITEM_ID_LENGTH));
                        cid.length = new Tuple(newres.dcl_length);
                        cid.total_u235 = new Tuple(newres.dcl_total_u235);
                        cid.total_u238 = new Tuple(newres.dcl_total_u238);
                        cid.total_rods = newres.dcl_total_rods;
                        cid.total_poison_rods = newres.dcl_total_poison_rods;
                        cid.poison_percent = new Tuple(newres.dcl_poison_percent);
                        cid.modified = true;

                        List<CollarItemId> list = NC.App.DB.CollarItemIds.GetList();
						bool glump = list.Exists(i => { return string.Compare(cid.item_id, i.item_id, true) == 0; });
                        if (glump && overwrite)
                        {
                            list.Remove(cid);
                            list.Add(cid);
                        }
                        else list.Add(cid);
                    }
                    else if (r is results_de_mult_rec) 
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 34061, ("Transferring method results for " + r.GetType().ToString()));
                        results_de_mult_rec oldres = (results_de_mult_rec)r;
                        INCCMethodResults.results_de_mult_rec newres =
                            (INCCMethodResults.results_de_mult_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE, true);
                        newres.meas_ring_ratio = oldres.de_meas_ring_ratio;
                        newres.energy_corr_factor = oldres.de_energy_corr_factor;
                        newres.interpolated_neutron_energy = oldres.de_interpolated_neutron_energy;
                        newres.methodParams.detector_efficiency = TransferUtils.Copy(oldres.de_detector_efficiency_res, INCC.MAX_DUAL_ENERGY_ROWS);
                        newres.methodParams.inner_outer_ring_ratio = TransferUtils.Copy(oldres.de_inner_outer_ring_ratio_res, INCC.MAX_DUAL_ENERGY_ROWS);
                        newres.methodParams.neutron_energy = TransferUtils.Copy(oldres.de_neutron_energy_res, INCC.MAX_DUAL_ENERGY_ROWS);
                        newres.methodParams.relative_fission = TransferUtils.Copy(oldres.de_relative_fission_res, INCC.MAX_DUAL_ENERGY_ROWS);
                        newres.methodParams.inner_ring_efficiency = oldres.de_inner_ring_efficiency_res;
                        newres.methodParams.outer_ring_efficiency = oldres.de_outer_ring_efficiency_res;
                    }
                    else 
                    {
                        if (r is results_tm_bkg_rec)
                        {
                            mlogger.TraceEvent(LogLevels.Warning, 34062, ("todo: Transferring method results for " + r.GetType().ToString()));  // todo: tm bkg 
                            results_tm_bkg_rec oldres = (results_tm_bkg_rec)r;
                            //INCCMethodResults.results_tm_bkg_rec newres =
                            //    (INCCMethodResults.results_tm_bkg_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod., true);
                        }
                        else mlogger.TraceEvent(LogLevels.Warning, 34040, ("todo: Transferring method results for " + r.GetType().ToString())); // todo: complete the list
                    }
                }

            }
            #endregion results transfer
            // dev note: here is where the detector tree creation and replacement with restore_replace_detector_structs occurs in the original code
            // todo: based on overwrite flag, replace det-dependent classes, the analysis_methods: (none-map) and each (det,mat) -> calib from results class instances

            #region Meas&Results

            AnalysisDefs.holdup_config_rec hc = new AnalysisDefs.holdup_config_rec();
            hc.glovebox_id = TransferUtils.str(results.results_glovebox_id, INCC.MAX_GLOVEBOX_ID_LENGTH);
            hc.num_columns = results.results_num_columns;
            hc.num_rows = results.results_num_rows;
            hc.distance = results.results_distance; 
            meas.AcquireState.glovebox_id = hc.glovebox_id;
            // this is a good place to create the general results_rec
            INCCResults.results_rec xres = new INCCResults.results_rec(meas);
            meas.INCCAnalysisResults.TradResultsRec = xres;
            // oddball entries in need of preservation             
            xres.total_good_count_time = results.total_good_count_time;
            xres.total_number_runs = results.total_number_runs;
            xres.primary = imr.primaryMethod;
            xres.net_drum_weight = results.net_drum_weight;
            xres.completed = (results.completed != 0 ? true : false);
            xres.db_version = results.db_version;
            xres.hc = hc;

            long mid = meas.Persist();

            // save the warning and error messages from the results here, these rode on the results rec in INCC5
            NC.App.DB.AddAnalysisMessages(msgs, mid);

            // Store off Params

            NC.App.DB.UpdateDetector(det);
            NC.App.DB.NormParameters.Set(det, meas.Norm);  // might have been saved in earlier step already 
            NC.App.DB.BackgroundParameters.Set(det, meas.Background); 
 
            INCCDB.AcquireSelector acqsel = new INCCDB.AcquireSelector(det, acq.item_type, acq.MeasDateTime);
            if (overwrite)
            {
                NC.App.DB.ReplaceAcquireParams(acqsel, acq);
            }
            else
                NC.App.DB.AddAcquireParams(acqsel, acq); // gotta add it to the global map, then push it to the DB

            if (meas.Tests != null) // added only if not found on the list
				NC.App.DB.TestParameters.Set(meas.Tests);
 
            NC.App.DB.Isotopics.SetList();  // new style de 'mouser'!
            //NC.App.DB.CompositeIsotopics.SetList();// next:NYI
            NC.App.DB.ItemIds.SetList(); // This writes any new items ids to the DB
            NC.App.DB.CollarItemIds.SetList(); // so does this

            //Loop over measurement cycles
            NC.App.DB.AddCycles(meas.Cycles, det.MultiplicityParams, mid);

            //Store off Params

            // todo: complete table design and creation/update here, still have issues with collar and curium ratio results
            // get results based on meas option, (Calib and Verif have method results, others do not), update into DB here
            IEnumerator iter = meas.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();
            while (iter.MoveNext())  // only one result indexer is present when doing an INCC5 transfer
            {
                MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
                INCCResult ir = meas.INCCAnalysisResults[moskey];
                // ir contains the measurement option-specific results: empty for rates and holdup, and also empty for calib and verif, the method-focused analyses, 
                // but values are present for initial, normalization, precision, and should be present for background for the tm bkg results 
                // next: specific GetParams for the init, norm and prec, and all the ver and cal mass methods 
                switch (meas.MeasOption)
                {
                    case AssaySelector.MeasurementOption.background:
                        if (meas.Background.TMBkgParams.ComputeTMBkg)
                            mlogger.TraceEvent(LogLevels.Warning, 82010, "Background truncated multiplicity"); // todo: present the tm bkg results on m.Background
                        break;
                    case AssaySelector.MeasurementOption.initial:
                    case AssaySelector.MeasurementOption.normalization:
                    case AssaySelector.MeasurementOption.precision:
                        {
                            ElementList els = ir.ToDBElementList();
                            DB.ParamsRelatedBackToMeasurement ar = new ParamsRelatedBackToMeasurement(ir.Table);
                            long resid = ar.Create(mid, els);
                            mlogger.TraceEvent(LogLevels.Verbose, 34103, string.Format("Preserving {0} as {1}", ir.Table, resid));
                        }
                        break;
                    case AssaySelector.MeasurementOption.verification:
                    case AssaySelector.MeasurementOption.calibration:
					{
						INCCMethodResults imrs;
						bool beendonegot = meas.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
						if (beendonegot) // should be true for verification and calibration
						{
							if (imrs.ContainsKey(meas.INCCAnalysisState.Methods.selector))
							{
								// we've got a distinct detector id and material type on the methods, so that is the indexer here
								Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrs[meas.INCCAnalysisState.Methods.selector];

								// now get an enumerator over the map of method results
								Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
								while (ai.MoveNext())
								{
									if (ai.Current.Key.IsNone())
										continue;
									INCCMethodResult _imr = ai.Current.Value;
									// insert this into the DB under the current meas key
									try
									{
										ElementList els = _imr.ToDBElementList(); // also sets table property, gotta do it first
										ParamsRelatedBackToMeasurement ar = new ParamsRelatedBackToMeasurement(_imr.Table);
										long resid = ar.Create(mid, els);
										ElementList mels = _imr.methodParams.ToDBElementList();
										long resmid = ar.CreateMethod(resid, mid, mels);
										mlogger.TraceEvent(LogLevels.Verbose, 34101, string.Format("Preserving {0} as {1},{2}", _imr.Table, resmid, resid));
									} 
									catch (Exception e)
									{
										mlogger.TraceEvent(LogLevels.Warning, 34102, "Results processing error {0} {1}", _imr.ToString(), e.Message);
									}
								}
							} else
								mlogger.TraceEvent(LogLevels.Error, 34102, "Results missing for {0}", meas.INCCAnalysisState.Methods.selector.ToString());

						}
					}
					break;
                    case AssaySelector.MeasurementOption.rates:
                    case AssaySelector.MeasurementOption.holdup:
                    case AssaySelector.MeasurementOption.unspecified:
                    default: // nothing new to present with these
                        break;
                }
            }
            #endregion Meas&Results

              // todo: handle well config setting with bkg here; 

            return true;
        }

        unsafe void AddToCycleList(run_rec run, Detector det, int cfindex = -1)  // cf index only for AAS positional cycles
        {
            Cycle cycle = new Cycle(mlogger);
            try
            {
                cycle.UpdateDataSourceId(ConstructedSource.INCCTransferCopy, // becomes transfer if reanalysis occurs
                                            det.Id.SRType,
                                            INCC.DateTimeFrom(TransferUtils.str(run.run_date, INCC.DATE_TIME_LENGTH), TransferUtils.str(run.run_time, INCC.DATE_TIME_LENGTH)), det.Id.FileName);
                meas.Add(cycle, cfindex);

                // mcr is created and stored in the cycle's CountingAnalysisResults
                MultiplicityCountingRes mcr = RunToCycle(run, cycle, det.MultiplicityParams);
                // AB is calculated at runtime when conditioning a cycle AFAICT, so its gotta be worked here at least once
                if (det.AB.Unset)
                {
					ABKey abkey = new ABKey(det.MultiplicityParams, mcr);
					AnalysisDefs.AlphaBeta AB = AlphaBetaCache.GetAlphaBeta(abkey);
					if (AB == null)
					{
						CycleProcessing.calc_alpha_beta(det.MultiplicityParams, mcr);
						AlphaBetaCache.AddAlphaBeta(abkey, mcr.AB);
					}                  
                }
                mcr.AB.TransferIntermediates(det.AB);  // copy alpha beta onto the cycle's results 
            }
            catch (Exception e)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33085, "Cycle processing error {0} {1}", run.run_number, e.Message);
            }
        }

        /// <summary>
        ///  Create the cycle counting results, add it to the measurement and copy the data from the run to the equivalent fields on the cycle
        /// </summary>
        /// <param name="run"></param>
        /// <param name="cycle"></param>
        unsafe static public MultiplicityCountingRes RunToCycle(run_rec run, Cycle cycle, Multiplicity key)
        {
            cycle.seq = run.run_number;
            cycle.TS = new TimeSpan(0, 0, (int)run.run_count_time);  // dev note: check if this is always only in seconds, or fractions of a second

            cycle.Totals = (ulong)run.run_singles;
            cycle.SinglesRate = run.run_singles / run.run_count_time; // use this value in the conditioning steps, it is not yet the DT corrected rate

            string s = TransferUtils.str(run.run_tests, INCC.MAX_RUN_TESTS_LENGTH);
            QCTestStatus qcts = QCTestStatusExtensions.FromString(s);

            cycle.SetQCStatus(key, qcts); // creates entry if not found

            MultiplicityCountingRes mcr = new MultiplicityCountingRes(key.FA, cycle.seq);
            cycle.CountingAnalysisResults.Add(key, mcr);
            mcr.Totals = cycle.Totals;
            mcr.TS = cycle.TS;

            mcr.DeadtimeCorrectedSinglesRate.v = run.run_singles_rate; // overridden later, not used
            mcr.DeadtimeCorrectedDoublesRate.v = run.run_doubles_rate;
            mcr.DeadtimeCorrectedTriplesRate.v = run.run_triples_rate;

            mcr.RASum = (ulong)run.run_reals_plus_acc;
            mcr.ASum = (ulong)run.run_acc;

            mcr.efficiency = run.run_multiplicity_efficiency;
            mcr.mass = run.run_mass;
            mcr.multiAlpha = run.run_multiplicity_alpha;
            mcr.multiplication = run.run_multiplicity_mult;
            cycle.HighVoltage = run.run_high_voltage;

            // assign the hits to a single channel (0)
            cycle.HitsPerChannel[0] = run.run_singles;
            mcr.RawSinglesRate.v = run.run_singles_rate;
            mcr.RawDoublesRate.v = run.run_doubles_rate;
            mcr.RawTriplesRate.v = run.run_triples_rate;

            mcr.Scaler1.v = run.run_scaler1;
            mcr.Scaler2.v = run.run_scaler2;
            mcr.Scaler1Rate.v = run.run_scaler1_rate;
            mcr.Scaler2Rate.v = run.run_scaler2_rate;

            long indexmax = 0;
            for (ulong i = 0; i < INCC.MULTI_ARRAY_SIZE; i++)
            {
                if (run.run_mult_acc[i] > 0 || run.run_mult_reals_plus_acc[i] > 0)
                {
                    indexmax = (long)i;
                }
            }

            mcr.MaxBins = (ulong)indexmax + 1;  // this is the index, not the count
            mcr.MinBins = (ulong)indexmax + 1;

            mcr.NormedAMult = new ulong[mcr.MaxBins];
            mcr.RAMult = new ulong[mcr.MaxBins];
            mcr.UnAMult = new ulong[mcr.MaxBins];

            for (ulong i = 0; i < (ulong)mcr.MaxBins; i++)
            {
                mcr.NormedAMult[i] = (ulong)run.run_mult_acc[i];
                mcr.RAMult[i] = (ulong)run.run_mult_reals_plus_acc[i];
            }

            return mcr;
        }

        #endregion measurement data transfer

    }
}
