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
using System.Text;
using AnalysisDefs;
using DB;
using DetectorDefs;
using NCCReporter;
namespace NCCTransfer
{
	using Tuple = VTuple;
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

		public static unsafe INCCInitialDataDetectorFile FromDetectors(List<Detector> dets)
		{
			INCCInitialDataDetectorFile iddf = new INCCInitialDataDetectorFile(NC.App.ControlLogger, null);
			foreach(Detector det in dets)
			{
				detector_rec dr = new detector_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = det.Id.DetectorId.ToCharArray(0, Math.Min(det.Id.DetectorId.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,dr.detector_id);
				b = new byte[INCC.DETECTOR_TYPE_LENGTH];
				a = det.Id.Type.ToCharArray(0, Math.Min(det.Id.Type.Length, INCC.DETECTOR_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,dr.detector_type);
				b = new byte[INCC.ELECTRONICS_ID_LENGTH];
				a = det.Id.ElectronicsId.ToCharArray(0, Math.Min(det.Id.ElectronicsId.Length, INCC.ELECTRONICS_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,dr.electronics_id);
				iddf.Detector.Add(dr);

				sr_parms_rec sr = new sr_parms_rec();
				b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				a = det.Id.DetectorId.ToCharArray(0, Math.Min(det.Id.DetectorId.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,sr.sr_detector_id);
				sr.sr_type = (short)det.Id.SRType;
				sr.sr_port_number = (short)det.Id.SerialPort;
				sr.predelay = det.SRParams.predelayMS;
				sr.die_away_time = det.SRParams.dieAwayTimeMS;
				sr.gate_length = det.SRParams.gateLengthMS;
				sr.coeff_a_deadtime = det.SRParams.deadTimeCoefficientAinMicroSecs;
				sr.coeff_b_deadtime = det.SRParams.deadTimeCoefficientBinPicoSecs;
				sr.coeff_c_deadtime = det.SRParams.deadTimeCoefficientCinNanoSecs;
				sr.multiplicity_deadtime = det.SRParams.deadTimeCoefficientMultiplicityinNanoSecs;
				sr.doubles_gate_fraction = det.SRParams.doublesGateFraction;
				sr.triples_gate_fraction = det.SRParams.triplesGateFraction;
				sr.efficiency = det.SRParams.efficiency;
				sr.high_voltage = det.SRParams.highVoltage;
				iddf.SRParms.Add(sr);

				BackgroundParameters bkgp = NC.App.DB.BackgroundParameters.Get(det);
				bkg_parms_rec bkg = new bkg_parms_rec();
				bkg.curr_active_bkg_scaler1_rate = bkgp.INCCActive.Scaler1Rate;
				bkg.curr_active_bkg_scaler2_rate = bkgp.INCCActive.Scaler2Rate;
				bkg.curr_active_bkg_singles_err = bkgp.INCCActive.Singles.err;
				bkg.curr_active_bkg_singles_rate = bkgp.INCCActive.Singles.v;
				bkg.curr_passive_bkg_doubles_err = bkgp.DeadtimeCorrectedRates.Doubles.err;
				bkg.curr_passive_bkg_doubles_rate = bkgp.DeadtimeCorrectedRates.Doubles.v;
				bkg.curr_passive_bkg_scaler1_rate = bkgp.Scaler1.v;
				bkg.curr_passive_bkg_scaler2_rate = bkgp.Scaler2.v;
				bkg.curr_passive_bkg_singles_err = bkgp.DeadtimeCorrectedRates.Singles.err;
				bkg.curr_passive_bkg_singles_rate = bkgp.DeadtimeCorrectedRates.Singles.v;
				bkg.curr_passive_bkg_triples_err = bkgp.DeadtimeCorrectedRates.Triples.err;
				bkg.curr_passive_bkg_triples_rate = bkgp.DeadtimeCorrectedRates.Triples.v;
				iddf.BKGParms.Add(bkg);
				tm_bkg_parms_rec tm = new tm_bkg_parms_rec();
				tm.tm_singles_bkg = bkgp.TMBkgParams.Singles.v;
				tm.tm_ones_bkg = bkgp.TMBkgParams.Ones.v;
				tm.tm_twos_bkg = bkgp.TMBkgParams.Twos.v;
				tm.tm_zeros_bkg = bkgp.TMBkgParams.Zeros.v;
				tm.tm_singles_bkg_err = bkgp.TMBkgParams.Singles.err;
				tm.tm_ones_bkg_err = bkgp.TMBkgParams.Ones.err;
				tm.tm_twos_bkg_err = bkgp.TMBkgParams.Twos.err;
				tm.tm_zeros_bkg_err = bkgp.TMBkgParams.Zeros.err;
				tm.tm_bkg = (bkgp.TMBkgParams.ComputeTMBkg ? (byte)0x1 : (byte)0x0);
				iddf.TMBKGParms.Add(tm);

				NormParameters normp = NC.App.DB.NormParameters.Get(det);
				norm_parms_rec norm = new norm_parms_rec();
				norm.acceptance_limit_percent = normp.acceptanceLimitPercent;
				norm.acceptance_limit_std_dev = normp.acceptanceLimitStdDev;
				norm.amli_ref_singles_rate = normp.amliRefSinglesRate;
				norm.bias_mode = NewToOldBiasTestId(normp.biasMode);
				norm.bias_precision_limit = normp.biasPrecisionLimit;
				norm.bias_test_addasrc_position = normp.biasTestAddasrcPosition;
				norm.bias_test_use_addasrc = (byte)(normp.biasTestUseAddasrc ? 1 : 0);
				norm.cf252_ref_doubles_rate = normp.cf252RefDoublesRate.v;
				norm.cf252_ref_doubles_rate_err = normp.cf252RefDoublesRate.v;
				norm.curr_normalization_constant = normp.currNormalizationConstant.v;
				norm.curr_normalization_constant_err = normp.currNormalizationConstant.err;
				norm.init_src_precision_limit = normp.initSrcPrecisionLimit;
				norm.meas_rate = normp.measRate.v;
				norm.meas_rate_err = normp.measRate.err;
				norm.yield_relative_to_mrc_95 = normp.yieldRelativeToMrc95;
				b = new byte[INCC.DATE_TIME_LENGTH];
				a = normp.refDate.ToString("yy.MM.dd").ToCharArray();
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,norm.ref_date);
				b = new byte[INCC.SOURCE_ID_LENGTH];
				a = normp.sourceId.ToCharArray(0, Math.Min(normp.sourceId.Length, INCC.SOURCE_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b,norm.source_id);
				iddf.NormParms.Add(norm);

				AddASourceSetup aasp = NC.App.DB.AASSParameters.Get(det);
				add_a_source_setup_rec aas = new add_a_source_setup_rec();
				TransferUtils.CopyDbls(aasp.dist_to_move, aas.ad_dist_to_move);
				aas.ad_forward_over_travel = aasp.forward_over_travel;
				aas.ad_number_positions = aasp.number_positions;
				aas.ad_port_number = aasp.port_number;
				aas.ad_reverse_over_travel = aasp.reverse_over_travel;
				aas.ad_type = NewToOldAASId(aasp.type);
				aas.cm_axis_number = aasp.cm_axis_number;
				aas.cm_forward_mask = aasp.cm_forward_mask;
				aas.cm_over_travel_state = aasp.cm_over_travel_state;
				aas.cm_reverse_mask = aasp.cm_reverse_mask;
				aas.cm_rotation = (byte)(aasp.cm_rotation ? 1 : 0);
				aas.cm_slow_inches = aasp.cm_slow_inches;
				aas.cm_steps_per_inch = aasp.cm_steps_per_inch;
				aas.cm_step_ratio = aasp.cm_step_ratio;
				aas.plc_steps_per_inch = aasp.plc_steps_per_inch;
				aas.scale_conversion_factor = aasp.scale_conversion_factor;
				iddf.AASParms.Add(aas);
			}
			return iddf;
		}

        public static unsafe List<INCCInitialDataCalibrationFile> CalibFromDetectors(List<Detector> dets)
		{
            List<INCCInitialDataCalibrationFile> list = new List<INCCInitialDataCalibrationFile>();

			foreach(Detector det in dets)
			{
                INCCInitialDataCalibrationFile idcf = new INCCInitialDataCalibrationFile(NC.App.ControlLogger, null);
                idcf.Name = det.Id.DetectorId;
                foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
                {
                    INCCSelector se = new INCCSelector(det.Id.DetectorId, desc.Name);
                    if (!NC.App.DB.DetectorMaterialAnalysisMethods.ContainsKey(se))
                        continue;

					// do the method choice rec first
					DetectorMaterialMethod dmm = new DetectorMaterialMethod(se.material, se.detectorid, INCC.METHOD_NONE);
                    idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveAMR(se));

 					// then do each method rec
                    AnalysisMethods ams = NC.App.DB.DetectorMaterialAnalysisMethods[se];
                    IEnumerator iter = ams.GetMethodEnumerator();
                    while (iter.MoveNext())
                    {
                        Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor> md = (Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor>)iter.Current;
                        dmm = new DetectorMaterialMethod(se.material, se.detectorid, (byte)NewTypeToOldMethodId(md.Item1));
						switch(md.Item1)
						{
							case AnalysisMethod.KnownA:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveKA(se, md.Item2));
								break;
							case AnalysisMethod.CalibrationCurve:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveCC(se, md.Item2));
								break;
							case AnalysisMethod.KnownM:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveKM(se, md.Item2));
								break;
							case AnalysisMethod.Multiplicity:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveMult(se, md.Item2));
								break;
							case AnalysisMethod.TruncatedMultiplicity:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveTM(se, md.Item2));
								break;
							case AnalysisMethod.AddASource:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveAS(se, md.Item2));
								break;
							case AnalysisMethod.CuriumRatio:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveCR(se, md.Item2));
								break;
							case AnalysisMethod.Active:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveCA(se, md.Item2));
								break;
							case AnalysisMethod.ActivePassive:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveAP(se, md.Item2));
								break;
							case AnalysisMethod.ActiveMultiplicity:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveAM(se, md.Item2));
								break;
							case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
								  idcf.DetectorMaterialMethodParameters.Add(dmm, Calib5.MoveDE(se, md.Item2));
								break;
							case AnalysisMethod.Collar:
                                try
                                { 
									  idcf.DetectorMaterialMethodParameters.
											Add(new DetectorMaterialMethod(se.material, se.detectorid, INCC.COLLAR_DETECTOR_SAVE_RESTORE), Calib5.MoveCD(se, md.Item2));
									  idcf.DetectorMaterialMethodParameters.
											Add(new DetectorMaterialMethod(se.material, se.detectorid, INCC.COLLAR_SAVE_RESTORE), Calib5.MoveCO(se, md.Item2));
									  idcf.DetectorMaterialMethodParameters.
											Add(new DetectorMaterialMethod(se.material, se.detectorid, INCC.COLLAR_K5_SAVE_RESTORE), Calib5.MoveCK(se, md.Item2));
								}
								catch (Exception e)
								{
									NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34102, "Collar xfer processing error {0} {1}", md.Item1.FullName(), e.Message);
								}
								break;
							case AnalysisMethod.COLLAR_SAVE_RESTORE:
							case AnalysisMethod.COLLAR_DETECTOR_SAVE_RESTORE:
							case AnalysisMethod.COLLAR_K5_SAVE_RESTORE:
									NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "Got '{0}' ", md.Item1.FullName());
								break;
							case AnalysisMethod.INCCNone:
								break;
							default:
								break;
						}                       
                    }					
				}
                list.Add(idcf);
			}
			return list;
		}
		internal static class Calib5
		{

			internal unsafe static analysis_method_rec MoveAMR(INCCSelector se)
			{
				analysis_method_rec amr = new analysis_method_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, amr.analysis_method_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, amr.item_type);
				AnalysisMethods ams = NC.App.DB.DetectorMaterialAnalysisMethods[se];
				amr.backup_method = (byte)NewTypeToOldMethodId(ams.Backup);
				amr.normal_method = (byte)NewTypeToOldMethodId(ams.Normal);
				amr.cal_curve = (byte)(ams.choices[(int)AnalysisMethod.CalibrationCurve] ? 1 : 0);
				amr.active = (byte)(ams.choices[(int)AnalysisMethod.Active] ? 1 : 0);
				amr.active_mult = (byte)(ams.choices[(int)AnalysisMethod.ActiveMultiplicity] ? 1 : 0);
				amr.active_passive = (byte)(ams.choices[(int)AnalysisMethod.ActivePassive] ? 1 : 0);
				amr.add_a_source = (byte)(ams.choices[(int)AnalysisMethod.AddASource] ? 1 : 0);
				amr.collar = (byte)(ams.choices[(int)AnalysisMethod.Collar] ? 1 : 0);
				amr.curium_ratio = (byte)(ams.choices[(int)AnalysisMethod.CuriumRatio] ? 1 : 0);
				amr.known_alpha = (byte)(ams.choices[(int)AnalysisMethod.KnownA] ? 1 : 0);
				amr.known_m = (byte)(ams.choices[(int)AnalysisMethod.KnownM] ? 1 : 0);
				amr.multiplicity = (byte)(ams.choices[(int)AnalysisMethod.Multiplicity] ? 1 : 0);
				amr.truncated_mult = (byte)(ams.choices[(int)AnalysisMethod.TruncatedMultiplicity] ? 1 : 0);
				return amr;
			}

			internal unsafe static multiplicity_rec MoveMult(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.multiplicity_rec imr = (INCCAnalysisParams.multiplicity_rec)md;
				multiplicity_rec mr = new multiplicity_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, mr.multiplicity_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, mr.multiplicity_item_type);
				mr.mul_a = imr.a;
				mr.mul_b = imr.b;
				mr.mul_c = imr.c;
				mr.mul_vi1 = imr.vi1;
				mr.mul_vi2 = imr.vi2;
				mr.mul_vi3 = imr.vi3;
				mr.mul_vs1 = imr.vs1;
				mr.mul_vs2 = imr.vs2;
				mr.mul_vs3 = imr.vs3;
				mr.mul_alpha_weight = imr.alpha_weight;
				mr.mul_sf_rate = imr.sf_rate;
				mr.mul_sigma_x = imr.sigma_x;
				mr.mul_solve_efficiency = (byte)imr.solve_efficiency;
				return mr;
			}

			internal unsafe static known_alpha_rec MoveKA(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.known_alpha_rec imr = (INCCAnalysisParams.known_alpha_rec)md;
				known_alpha_rec m = new known_alpha_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.known_alpha_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.known_alpha_item_type);
				m.ka_a = imr.cev.a;
				m.ka_b = imr.cev.b;
				m.ka_var_a = imr.cev.var_a;
				m.ka_var_b = imr.cev.var_b;
				m.ka_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.ka_sigma_x = imr.cev.sigma_x;
				m.ka_ring_ratio_a = imr.ring_ratio.a;
				m.ka_ring_ratio_b = imr.ring_ratio.b;
				m.ka_ring_ratio_c = imr.ring_ratio.c;
				m.ka_ring_ratio_d = imr.ring_ratio.d;
				m.ka_alpha_wt = imr.alpha_wt;
				m.ka_rho_zero = imr.rho_zero;
				m.ka_ring_ratio_equation = (double)imr.ring_ratio.cal_curve_equation;
				m.ka_upper_mass_limit = imr.cev.upper_mass_limit;
				m.ka_lower_mass_limit = imr.cev.lower_mass_limit;
				m.ka_heavy_metal_corr_factor = imr.heavy_metal_corr_factor;
				m.ka_heavy_metal_reference = imr.heavy_metal_reference;
				m.ka_k = imr.k;
				m.ka_known_alpha_type = (double)imr.known_alpha_type;
				TransferUtils.CopyDbls(imr.dcl_mass, m.ka_dcl_mass);
				TransferUtils.CopyDbls(imr.doubles, m.ka_doubles);

				return m;
			}

			internal unsafe static known_m_rec MoveKM(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.known_m_rec imr = (INCCAnalysisParams.known_m_rec)md;
				known_m_rec m = new known_m_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.known_m_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.known_m_item_type);
				m.km_b = imr.b;
				m.km_c = imr.c;
				m.km_sf_rate = imr.sf_rate;
				m.km_sigma_x = imr.sigma_x;
				m.km_upper_mass_limit = imr.upper_mass_limit;
				m.km_lower_mass_limit = imr.lower_mass_limit;
				m.km_vi1 = imr.vi1;
				m.km_vi2 = imr.vi2;
				m.km_vs1 = imr.vs1;
				m.km_vs2 = imr.vs2;
				return m;
			}

			internal unsafe static cal_curve_rec MoveCC(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.cal_curve_rec imr = (INCCAnalysisParams.cal_curve_rec)md;
				cal_curve_rec m = new cal_curve_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.cal_curve_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.cal_curve_item_type);
				m.cc_a = imr.cev.a;
				m.cc_b = imr.cev.b;
				m.cc_c = imr.cev.c;
				m.cc_d = imr.cev.d;
				m.cc_cal_curve_type = (double)imr.CalCurveType;
				m.cc_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.cc_covar_ac = imr.cev.covar(Coeff.a, Coeff.c);
				m.cc_covar_ad = imr.cev.covar(Coeff.a, Coeff.d);
				m.cc_covar_bc = imr.cev.covar(Coeff.b, Coeff.c);
				m.cc_covar_bd = imr.cev.covar(Coeff.b, Coeff.d);
				m.cc_covar_cd = imr.cev.covar(Coeff.c, Coeff.d);
				m.cc_var_a = imr.cev.var_a;
				m.cc_var_b = imr.cev.var_b;
				m.cc_var_c = imr.cev.var_c;
				m.cc_var_d = imr.cev.var_d;
				m.cc_sigma_x = imr.cev.sigma_x;
				m.cc_upper_mass_limit = imr.cev.upper_mass_limit;
				m.cc_lower_mass_limit = imr.cev.lower_mass_limit;
				m.cc_heavy_metal_corr_factor = imr.heavy_metal_corr_factor;
				m.cc_heavy_metal_reference = imr.heavy_metal_reference;
				m.cc_cal_curve_type = (double)imr.CalCurveType;
				m.cc_percent_u235 = imr.percent_u235;
				TransferUtils.CopyDbls(imr.dcl_mass, m.cc_dcl_mass);
				TransferUtils.CopyDbls(imr.doubles, m.cc_doubles);
				return m;
			}

			internal unsafe static truncated_mult_rec MoveTM(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.truncated_mult_rec imr = (INCCAnalysisParams.truncated_mult_rec)md;
				truncated_mult_rec m = new truncated_mult_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.truncated_mult_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.truncated_mult_item_type);
				m.tm_a = imr.a;
				m.tm_b = imr.b;
				m.tm_known_eff = (byte)(imr.known_eff ? 1 : 0);
				m.tm_solve_eff = (byte)(imr.solve_eff ? 1 : 0);
				return m;
			}

			internal unsafe static curium_ratio_rec MoveCR(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.curium_ratio_rec imr = (INCCAnalysisParams.curium_ratio_rec)md;
				curium_ratio_rec m = new curium_ratio_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.curium_ratio_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.curium_ratio_item_type);
				m.cr_a = imr.cev.a;
				m.cr_b = imr.cev.b;
				m.cr_c = imr.cev.c;
				m.cr_d = imr.cev.d;
				m.curium_ratio_equation = (byte)imr.cev.cal_curve_equation;
				m.curium_ratio_type = NewToOldCRVariants(imr.curium_ratio_type);
				m.cr_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.cr_covar_ac = imr.cev.covar(Coeff.a, Coeff.c);
				m.cr_covar_ad = imr.cev.covar(Coeff.a, Coeff.d);
				m.cr_covar_bc = imr.cev.covar(Coeff.b, Coeff.c);
				m.cr_covar_bd = imr.cev.covar(Coeff.b, Coeff.d);
				m.cr_covar_cd = imr.cev.covar(Coeff.c, Coeff.d);
				m.cr_var_a = imr.cev.var_a;
				m.cr_var_b = imr.cev.var_b;
				m.cr_var_c = imr.cev.var_c;
				m.cr_var_d = imr.cev.var_d;
				m.cr_sigma_x = imr.cev.sigma_x;
				m.cr_upper_mass_limit = imr.cev.upper_mass_limit;
				m.cr_lower_mass_limit = imr.cev.lower_mass_limit;
				return m;
			}

			internal unsafe static active_mult_rec MoveAM(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.active_mult_rec imr = (INCCAnalysisParams.active_mult_rec)md;
				active_mult_rec m = new active_mult_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_mult_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_mult_item_type);
				m.am_vf1 = imr.vf1;
				m.am_vf2 = imr.vf2;
				m.am_vf3 = imr.vf3;
				m.am_vt1 = imr.vt1;
				m.am_vt2 = imr.vt2;
				m.am_vt3 = imr.vt3;
				return m;
			}

			internal unsafe static active_rec MoveCA(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.active_rec imr = (INCCAnalysisParams.active_rec)md;
				active_rec m = new active_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_item_type);
				m.act_a = imr.cev.a;
				m.act_b = imr.cev.b;
				m.act_c = imr.cev.c;
				m.act_d = imr.cev.d;
				m.active_equation = (byte)imr.cev.cal_curve_equation;
				m.act_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.act_covar_ac = imr.cev.covar(Coeff.a, Coeff.c);
				m.act_covar_ad = imr.cev.covar(Coeff.a, Coeff.d);
				m.act_covar_bc = imr.cev.covar(Coeff.b, Coeff.c);
				m.act_covar_bd = imr.cev.covar(Coeff.b, Coeff.d);
				m.act_covar_cd = imr.cev.covar(Coeff.c, Coeff.d);
				m.act_var_a = imr.cev.var_a;
				m.act_var_b = imr.cev.var_b;
				m.act_var_c = imr.cev.var_c;
				m.act_var_d = imr.cev.var_d;
				m.act_sigma_x = imr.cev.sigma_x;
				m.act_upper_mass_limit = imr.cev.upper_mass_limit;
				m.act_lower_mass_limit = imr.cev.lower_mass_limit;
				TransferUtils.CopyDbls(imr.dcl_mass, m.act_dcl_mass);
				TransferUtils.CopyDbls(imr.doubles, m.act_doubles);
				return m;
			}

			internal unsafe static active_passive_rec MoveAP(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.active_passive_rec imr = (INCCAnalysisParams.active_passive_rec)md;
				active_passive_rec m = new active_passive_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_passive_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.active_passive_item_type);
				m.ap_a = imr.cev.a;
				m.ap_b = imr.cev.b;
				m.ap_c = imr.cev.c;
				m.ap_d = imr.cev.d;
				m.active_passive_equation = (byte)imr.cev.cal_curve_equation;
				m.ap_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.ap_covar_ac = imr.cev.covar(Coeff.a, Coeff.c);
				m.ap_covar_ad = imr.cev.covar(Coeff.a, Coeff.d);
				m.ap_covar_bc = imr.cev.covar(Coeff.b, Coeff.c);
				m.ap_covar_bd = imr.cev.covar(Coeff.b, Coeff.d);
				m.ap_covar_cd = imr.cev.covar(Coeff.c, Coeff.d);
				m.ap_var_a = imr.cev.var_a;
				m.ap_var_b = imr.cev.var_b;
				m.ap_var_c = imr.cev.var_c;
				m.ap_var_d = imr.cev.var_d;
				m.ap_sigma_x = imr.cev.sigma_x;
				m.ap_upper_mass_limit = imr.cev.upper_mass_limit;
				m.ap_lower_mass_limit = imr.cev.lower_mass_limit;
				return m;
			}

			internal unsafe static add_a_source_rec MoveAS(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.add_a_source_rec imr = (INCCAnalysisParams.add_a_source_rec)md;
				add_a_source_rec m = new add_a_source_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.add_a_source_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.add_a_source_item_type);
				m.ad_a = imr.cev.a;
				m.ad_b = imr.cev.b;
				m.ad_c = imr.cev.c;
				m.ad_d = imr.cev.d;
				m.add_a_source_equation = (byte)imr.cev.cal_curve_equation;
				m.ad_covar_ab = imr.cev.covar(Coeff.a, Coeff.b);
				m.ad_covar_ac = imr.cev.covar(Coeff.a, Coeff.c);
				m.ad_covar_ad = imr.cev.covar(Coeff.a, Coeff.d);
				m.ad_covar_bc = imr.cev.covar(Coeff.b, Coeff.c);
				m.ad_covar_bd = imr.cev.covar(Coeff.b, Coeff.d);
				m.ad_covar_cd = imr.cev.covar(Coeff.c, Coeff.d);
				m.ad_var_a = imr.cev.var_a;
				m.ad_var_b = imr.cev.var_b;
				m.ad_var_c = imr.cev.var_c;
				m.ad_var_d = imr.cev.var_d;
				m.ad_sigma_x = imr.cev.sigma_x;
				m.ad_upper_mass_limit = imr.cev.upper_mass_limit;
				m.ad_lower_mass_limit = imr.cev.lower_mass_limit;
				TransferUtils.CopyDbls(imr.dcl_mass, m.ad_dcl_mass);
				TransferUtils.CopyDbls(imr.doubles, m.ad_doubles);
				TransferUtils.CopyDbls(imr.position_dzero, m.ad_position_dzero);
				m.ad_cf_a = imr.cf.a;
				m.ad_cf_b = imr.cf.b;
				m.ad_cf_c = imr.cf.c;
				m.ad_cf_d = imr.cf.d;
				m.ad_dzero_avg = imr.dzero_avg;
				m.ad_num_runs = imr.num_runs;
				m.ad_tm_dbls_rate_upper_limit = imr.tm_dbls_rate_upper_limit;
				m.ad_tm_weighting_factor = imr.tm_weighting_factor;
				m.ad_use_truncated_mult = imr.use_truncated_mult ? 1 : 0;
				b = new byte[INCC.DATE_TIME_LENGTH];
				a = imr.dzero_ref_date.ToString("yy.MM.dd").ToCharArray();
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.ad_dzero_ref_date);
				return m;
			}

			internal unsafe static collar_detector_rec MoveCD(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.collar_combined_rec imr = (INCCAnalysisParams.collar_combined_rec)md;
				collar_detector_rec m = new collar_detector_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.collar_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.collar_detector_item_type);
				m.collar_detector_mode = (byte)(imr.collar_det.collar_mode ? 1 : 0);

				b = new byte[INCC.DATE_TIME_LENGTH];
				a = imr.collar_det.reference_date.ToString("yy.MM.dd").ToCharArray();
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.col_reference_date);
				m.col_relative_doubles_rate = imr.collar_det.relative_doubles_rate;
				return m;
			}
			internal unsafe static collar_rec MoveCO(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.collar_combined_rec imr = (INCCAnalysisParams.collar_combined_rec)md;
				collar_rec m = new collar_rec();
				byte[] b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				char[] a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.collar_item_type);
				m.col_a = imr.collar.cev.a;
				m.col_b = imr.collar.cev.b;
				m.col_c = imr.collar.cev.c;
				m.col_d = imr.collar.cev.d;
				m.collar_equation = (byte)imr.collar.cev.cal_curve_equation;
				m.col_covar_ab = imr.collar.cev.covar(Coeff.a, Coeff.b);
				m.col_covar_ac = imr.collar.cev.covar(Coeff.a, Coeff.c);
				m.col_covar_ad = imr.collar.cev.covar(Coeff.a, Coeff.d);
				m.col_covar_bc = imr.collar.cev.covar(Coeff.b, Coeff.c);
				m.col_covar_bd = imr.collar.cev.covar(Coeff.b, Coeff.d);
				m.col_covar_cd = imr.collar.cev.covar(Coeff.c, Coeff.d);
				m.col_var_a = imr.collar.cev.var_a;
				m.col_var_b = imr.collar.cev.var_b;
				m.col_var_c = imr.collar.cev.var_c;
				m.col_var_d = imr.collar.cev.var_d;
				m.col_sigma_x = imr.collar.cev.sigma_x;
				m.col_upper_mass_limit = imr.collar.cev.upper_mass_limit;
				m.col_lower_mass_limit = imr.collar.cev.lower_mass_limit;

				m.col_number_calib_rods = imr.collar.number_calib_rods;
				m.col_sample_corr_fact = imr.collar.sample_corr_fact.v;
				m.col_sample_corr_fact_err = imr.collar.sample_corr_fact.err;
				m.col_u_mass_corr_fact_a = imr.collar.u_mass_corr_fact_a.v;
				m.col_u_mass_corr_fact_a_err = imr.collar.u_mass_corr_fact_a.err;
				m.col_u_mass_corr_fact_b = imr.collar.u_mass_corr_fact_b.v;
				m.col_u_mass_corr_fact_b_err = imr.collar.u_mass_corr_fact_b.err;
				m.collar_mode = (byte)(imr.collar.collar_mode ? 1 : 0);

				byte[] bb = new byte[INCC.MAX_POISON_ROD_TYPES * INCC.MAX_ROD_TYPE_LENGTH];
				int indx = 0;
				for (int i = 0; i < INCC.MAX_POISON_ROD_TYPES; i++)
				{
					if (string.IsNullOrEmpty(imr.collar.poison_rod_type[i]))
					{
						char[] aa = imr.collar.poison_rod_type[i].ToCharArray(0, Math.Min(imr.collar.poison_rod_type[i].Length, INCC.MAX_ROD_TYPE_LENGTH));
						Encoding.ASCII.GetBytes(aa, 0, aa.Length, bb, indx);
					}
					indx += 2;
				}
				TransferUtils.Copy(bb, 0, m.col_poison_rod_type, 0, INCC.MAX_POISON_ROD_TYPES * INCC.MAX_ROD_TYPE_LENGTH);

				TransferUtils.CopyDbls(imr.collar.poison_absorption_fact, m.col_poison_absorption_fact);
				CopyTuples(imr.collar.poison_rod_a, m.col_poison_rod_a, m.col_poison_rod_a_err, INCC.MAX_POISON_ROD_TYPES);
				CopyTuples(imr.collar.poison_rod_b, m.col_poison_rod_b, m.col_poison_rod_b_err, INCC.MAX_POISON_ROD_TYPES);
				CopyTuples(imr.collar.poison_rod_c, m.col_poison_rod_c, m.col_poison_rod_c_err, INCC.MAX_POISON_ROD_TYPES);
				return m;
			}
			internal unsafe static collar_k5_rec MoveCK(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.collar_combined_rec imr = (INCCAnalysisParams.collar_combined_rec)md;
				collar_k5_rec m = new collar_k5_rec();
				byte[] b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				char[] a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.collar_k5_item_type);
				CopyTuples(imr.k5.k5, m.collar_k5, m.collar_k5_err, INCC.MAX_COLLAR_K5_PARAMETERS);
				TransferUtils.CopyBoolsToInts(imr.k5.k5_checkbox, m.collar_k5_checkbox);
				m.collar_k5_mode = (byte)(imr.k5.k5_mode ? 1 : 0);

				byte[] bb = new byte[INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH];
				int indx = 0;
				for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
				{
					char[] aa = imr.k5.k5_label[i].ToCharArray(0, Math.Min(imr.k5.k5_label[i].Length, INCC.MAX_K5_LABEL_LENGTH));
					Encoding.ASCII.GetBytes(aa, 0, aa.Length, bb, indx);
					indx += INCC.MAX_K5_LABEL_LENGTH;
				}
				TransferUtils.Copy(bb, 0, m.collar_k5_label, 0, INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH);
				return m;
			}

			internal unsafe static de_mult_rec MoveDE(INCCSelector se, INCCAnalysisParams.INCCMethodDescriptor md)
			{
				INCCAnalysisParams.de_mult_rec imr = (INCCAnalysisParams.de_mult_rec)md;
				de_mult_rec m = new de_mult_rec();
				byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
				char[] a = se.detectorid.ToCharArray(0, Math.Min(se.detectorid.Length, INCC.MAX_DETECTOR_ID_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.de_detector_id);
				b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
				a = se.material.ToCharArray(0, Math.Min(se.material.Length, INCC.MAX_ITEM_TYPE_LENGTH));
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, m.de_item_type);
				m.de_inner_ring_efficiency = imr.inner_ring_efficiency;
				m.de_outer_ring_efficiency = imr.outer_ring_efficiency;

				TransferUtils.CopyDbls(imr.neutron_energy, m.de_neutron_energy);
				TransferUtils.CopyDbls(imr.detector_efficiency, m.de_detector_efficiency);
				TransferUtils.CopyDbls(imr.inner_outer_ring_ratio, m.de_inner_outer_ring_ratio);
				TransferUtils.CopyDbls(imr.relative_fission, m.de_relative_fission);
				return m;
			}
		}

		public static unsafe List<INCCTransferFile> XFerFromMeasurements(List<Measurement> meas)
		{
			List<INCCTransferFile> list = new List<INCCTransferFile>();
			foreach (Measurement m in meas)
			{
				IEnumerator iter = m.CountingAnalysisResults.GetMultiplicityEnumerator();
				while (iter.MoveNext())                     // for each mkey, a seperate xfer file should be emitted
				{
					INCCTransferFile itf = new INCCTransferFile(NC.App.ControlLogger, null);
					itf.Name = MethodResultsReport.EightCharConvert(m.MeasDate) + "." + m.MeasOption.INCC5Suffix();
					list.Add(itf);
					itf.results_rec_list.Add(Result5.MoveResultsRec(m));
					itf.results_status_list.Add(Result5.EncodeResultsStatus(m));
					Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
					if (m.MeasOption >= AssaySelector.MeasurementOption.background && m.MeasOption <= AssaySelector.MeasurementOption.holdup)
					{
						MeasOptionSelector mos = new MeasOptionSelector(m.MeasOption, mkey);
						if (m.MeasOption == AssaySelector.MeasurementOption.initial)
							itf.method_results_list.Add(Result5.MoveInitSrc(m.INCCAnalysisResults, mos));
						else if (m.MeasOption == AssaySelector.MeasurementOption.normalization)
							itf.method_results_list.Add(Result5.MoveBias(m.INCCAnalysisResults, mos));
						else if (m.MeasOption == AssaySelector.MeasurementOption.precision)
							itf.method_results_list.Add(Result5.MovePrecision(m.INCCAnalysisResults, mos));
						else if (m.MeasOption == AssaySelector.MeasurementOption.background)
							itf.method_results_list.Add(Result5.MoveTruncBkg(m.INCCAnalysisState, mos)); // NEXT: tm bkg, special case, unfinished
						else if (m.MeasOption == AssaySelector.MeasurementOption.verification || m.MeasOption == AssaySelector.MeasurementOption.calibration)
						{
							itf.method_results_list.Add(Result5.MoveCalCurve(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveKnownA(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveKnownM(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveMult(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveDEMult(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveActPass(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveCollar(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveActive(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveActiveMult(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveCurium(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveTruncMult(m.INCCAnalysisState, mos));
							itf.method_results_list.Add(Result5.MoveTruncBkg(m.INCCAnalysisState, mos)); // todo: here too, or just for bkg measurement?
							itf.method_results_list.Add(Result5.MoveAAS(m.INCCAnalysisState, mos));
						}
					}
					foreach (Cycle c in m.Cycles)
					{
						itf.run_rec_list.Add(Result5.MoveCycleToRunRec(c, mkey));
					}
					if (m.MeasOption == AssaySelector.MeasurementOption.verification && m.INCCAnalysisState.Methods.HasMethod(AnalysisMethod.AddASource))
					{
						itf.InitCFRunLists();
						int aasp = 0;
						foreach (CycleList cl in m.CFCycles)
						{
							foreach(Cycle cfc in cl)
								itf.CFrun_rec_list[aasp].Add(Result5.MoveCycleToRunRec(cfc, mkey));
							aasp++;
						}
					}
				}
			}
			return list;
		}

		static byte[] StringSquish(string s, int len)
        {
            byte[] b = new byte[len];
            if (s.Length > len)
                s = s.Substring(s.Length - len);
            char[] a = s.ToCharArray();
            Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
            return b;
        }

		internal static class Result5
		{

			internal unsafe static results_rec MoveResultsRec(Measurement m)
			{
				results_rec rec = new results_rec();
                byte[] b = StringSquish(m.MeasDate.ToString("yy.MM.dd"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, rec.meas_date);
				TransferUtils.Copy(b, rec.original_meas_date);  // this value is now properly tracked in INCC6
                b = StringSquish(m.MeasDate.ToString("HH:mm:ss"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, rec.meas_time);
				
				b = StringSquish(System.IO.Path.GetFileName(m.MeasurementId.FileName), INCC.FILE_NAME_LENGTH);
				TransferUtils.Copy(b, rec.filename);

				b = StringSquish(m.AcquireState.facility.Name, INCC.FACILITY_LENGTH);
				TransferUtils.Copy(b, rec.results_facility);
				b = StringSquish(m.AcquireState.mba.Name, INCC.MBA_LENGTH);
				TransferUtils.Copy(b, rec.results_mba);
				b = StringSquish(m.AcquireState.stratum_id.Name, INCC.MAX_STRATUM_ID_LENGTH);
				TransferUtils.Copy(b, rec.stratum_id);
				b = StringSquish(m.AcquireState.facility.Desc, INCC.DESCRIPTION_LENGTH);
				TransferUtils.Copy(b, rec.results_facility_description);
                b = StringSquish(m.AcquireState.mba.Desc, INCC.DESCRIPTION_LENGTH);
				TransferUtils.Copy(b, rec.results_mba_description);
                b = StringSquish(m.AcquireState.stratum_id.Desc, INCC.DESCRIPTION_LENGTH);
				TransferUtils.Copy(b, rec.stratum_id_description);

                b = StringSquish(m.AcquireState.item_id, INCC.MAX_ITEM_ID_LENGTH);
				TransferUtils.Copy(b, rec.item_id);

				b = StringSquish(m.AcquireState.campaign_id, INCC.MAX_CAMPAIGN_ID_LENGTH);
				TransferUtils.Copy(b, rec.results_campaign_id);
				TransferUtils.Copy(b, rec.results_inspection_number);

				b = StringSquish(m.AcquireState.item_type, INCC.MAX_ITEM_TYPE_LENGTH);
				TransferUtils.Copy(b, rec.results_item_type);

				rec.results_collar_mode = (byte)(m.AcquireState.collar_mode ? 1 : 0);

                b = StringSquish(m.Detector.Id.DetectorId, INCC.MAX_DETECTOR_ID_LENGTH);
				TransferUtils.Copy(b, rec.results_detector_id);
                b = StringSquish(m.Detector.Id.Type, INCC.DETECTOR_TYPE_LENGTH);
				TransferUtils.Copy(b, rec.results_detector_type);
                b = StringSquish(m.Detector.Id.ElectronicsId, INCC.ELECTRONICS_ID_LENGTH);
				TransferUtils.Copy(b, rec.results_electronics_id);

				b = StringSquish(m.AcquireState.glovebox_id, INCC.MAX_GLOVEBOX_ID_LENGTH);
				TransferUtils.Copy(b, rec.results_glovebox_id);
				rec.results_num_rows = m.INCCAnalysisResults.TradResultsRec.hc.num_rows;
				rec.results_num_columns = m.INCCAnalysisResults.TradResultsRec.hc.num_columns;
				rec.results_distance = m.INCCAnalysisResults.TradResultsRec.hc.distance;
				
				// devnote: full hc results record on a measurement is lost in the persist -> restore loop because upon 'read from DB' or restore we normally do not build out the TradResultsRec.
				rec.bias_uncertainty = m.Stratum.bias_uncertainty;
				rec.random_uncertainty = m.Stratum.random_uncertainty;
				rec.systematic_uncertainty = m.Stratum.systematic_uncertainty;
				rec.relative_std_dev = m.Stratum.relative_std_dev;

				b = StringSquish(m.AcquireState.inventory_change_code, INCC.INVENTORY_CHG_LENGTH);
                TransferUtils.Copy(b, rec.inventory_change_code);
                b = StringSquish(m.AcquireState.io_code, INCC.IO_CODE_LENGTH);
                TransferUtils.Copy(b, rec.io_code);

				rec.meas_option = (byte)m.MeasOption;

				rec.well_config = (byte)m.AcquireState.well_config; 
                rec.data_source = (byte)m.AcquireState.data_src; 
                rec.results_qc_tests = (byte)(m.AcquireState.qc_tests ? 1 : 0);
				rec.results_print = (byte)(m.AcquireState.print ? 1 : 0);
				rec.error_calc_method = (ushort) (m.AcquireState.error_calc_method == ErrorCalculationTechnique.Sample ? INCC.IDC_SAMPLE_STD_DEV : INCC.IDC_THEORETICAL_STD_DEV);

				b = StringSquish(m.AcquireState.user_id, INCC.CHAR_FIELD_LENGTH);
                TransferUtils.Copy(b, rec.user_id);
				b = StringSquish(m.AcquireState.comment, INCC.MAX_COMMENT_LENGTH);
                TransferUtils.Copy(b, rec.comment);
				b = StringSquish(m.AcquireState.ending_comment_str, INCC.MAX_COMMENT_LENGTH);
				TransferUtils.Copy(b, rec.ending_comment);

				rec.item_pu238 = m.Isotopics.pu238;
				rec.item_pu238_err = m.Isotopics.pu238_err;
				rec.item_pu239 = m.Isotopics.pu239;
				rec.item_pu239_err = m.Isotopics.pu239_err;
				rec.item_pu240 = m.Isotopics.pu240;
				rec.item_pu240_err = m.Isotopics.pu240_err;
				rec.item_pu241 = m.Isotopics.pu241;
				rec.item_pu241_err = m.Isotopics.pu241_err;
				rec.item_pu242 = m.Isotopics.pu242;
				rec.item_pu242_err = m.Isotopics.pu242_err;
				rec.item_am241 = m.Isotopics.am241;
				rec.item_am241 = m.Isotopics.am241_err;
				b = StringSquish(m.Isotopics.id, INCC.MAX_ISOTOPICS_ID_LENGTH);
				TransferUtils.Copy(b, rec.item_isotopics_id);
				b = StringSquish(m.Isotopics.source_code.ToString(), INCC.ISO_SOURCE_CODE_LENGTH);
				TransferUtils.Copy(b, rec.item_isotopics_source_code);

				rec.normalization_constant = m.Norm.currNormalizationConstant.v;
				rec.normalization_constant_err = m.Norm.currNormalizationConstant.err;
								
				rec.results_predelay = m.Detector.SRParams.predelayMS;
				rec.results_gate_length = m.Detector.SRParams.gateLengthMS;
				rec.results_gate_length2 = m.Detector.SRParams.gateLengthMS; // not used
				rec.results_high_voltage = m.Detector.SRParams.highVoltage; 
				rec.results_die_away_time = m.Detector.SRParams.dieAwayTimeMS; 
				rec.results_efficiency = m.Detector.SRParams.efficiency; 
				rec.results_multiplicity_deadtime = m.Detector.SRParams.deadTimeCoefficientMultiplicityinNanoSecs; 
				rec.results_coeff_a_deadtime = m.Detector.SRParams.deadTimeCoefficientAinMicroSecs;
				rec.results_coeff_b_deadtime = m.Detector.SRParams.deadTimeCoefficientBinPicoSecs;
				rec.results_coeff_c_deadtime = m.Detector.SRParams.deadTimeCoefficientCinNanoSecs;
				rec.results_doubles_gate_fraction = m.Detector.SRParams.triplesGateFraction;
				rec.results_triples_gate_fraction = m.Detector.SRParams.doublesGateFraction;

                // get the first results from the results map 
				MultiplicityCountingRes mcr = null;
                if (m.CountingAnalysisResults.Count > 0 && m.CountingAnalysisResults.HasMultiplicity)
                    try
                    {
                        mcr = (MultiplicityCountingRes)m.CountingAnalysisResults[m.Detector.MultiplicityParams];
                    }
                    catch (Exception)
                    {
                        if (mcr == null)
                            mcr = m.CountingAnalysisResults.GetFirstMultiplicity;
                    }
                if (mcr == null)
                    mcr = new MultiplicityCountingRes();  // inadequate attempt tries to account for LM-only condition, where no mcr, or no matching mcr, exists

				rec.r_acc_sngl_test_rate_limit = m.Tests.accSnglTestRateLimit;
				rec.r_acc_sngl_test_precision_limit = m.Tests.accSnglTestPrecisionLimit;
				rec.r_acc_sngl_test_outlier_limit = m.Tests.accSnglTestOutlierLimit;
				rec.r_outlier_test_limit = m.Tests.outlierTestLimit;
				rec.r_bkg_doubles_rate_limit = m.Tests.bkgDoublesRateLimit;
				rec.r_bkg_triples_rate_limit = m.Tests.bkgTriplesRateLimit;
				rec.r_chisq_limit = m.Tests.chiSquaredLimit;
				rec.r_max_num_failures = m.Tests.maxNumFailures;
				rec.r_high_voltage_test_limit = m.Tests.highVoltageTestLimit;

				rec.r_normal_backup_assay_test_lim = m.Tests.normalBackupAssayTestLimit;
				rec.r_max_runs_for_outlier_test = m.Tests.maxCyclesForOutlierTest;
				rec.r_checksum_test = (byte)(m.Tests.checksum ? 1 : 0);
				rec.results_accidentals_method = (m.Tests.accidentalsMethod == AccidentalsMethod.Measure ? INCC.IDC_MEASURE_ACCIDENTALS : INCC.IDC_CALCULATE_ACCIDENTALS);

				rec.passive_bkg_singles_rate = m.Background.DeadtimeCorrectedRates.Singles.v;
				rec.passive_bkg_singles_rate_err = m.Background.DeadtimeCorrectedRates.Singles.err;
				rec.passive_bkg_doubles_rate = m.Background.DeadtimeCorrectedRates.Doubles.v;
				rec.passive_bkg_doubles_rate_err = m.Background.DeadtimeCorrectedRates.Doubles.err;
				rec.passive_bkg_triples_rate = m.Background.DeadtimeCorrectedRates.Triples.v;
				rec.passive_bkg_triples_rate_err = m.Background.DeadtimeCorrectedRates.Triples.err;
				rec.active_bkg_singles_rate = m.Background.INCCActive.Singles.v;
				rec.active_bkg_singles_rate_err = m.Background.INCCActive.Singles.err;
				rec.passive_bkg_scaler1_rate = m.Background.Scaler1.v;
				rec.passive_bkg_scaler2_rate = m.Background.Scaler2.v;
				rec.active_bkg_scaler1_rate = m.Background.INCCActive.Scaler1Rate;
				rec.active_bkg_scaler2_rate = m.Background.INCCActive.Scaler2Rate;

				List<MeasurementMsg> msgs = m.GetMessageList(m.Detector.MultiplicityParams);
				byte[] bb = new byte[INCC.NUM_ERROR_MSG_CODES * INCC.ERR_MSG_LENGTH];
				int indx = 0, recidx = 0;
				for (int i = 0; i < msgs.Count && recidx < INCC.NUM_ERROR_MSG_CODES; i++)
				{
					if (msgs.Count > i && string.IsNullOrEmpty(msgs[i].text) && msgs[i].IsError)
					{
						char[] aa = msgs[i].text.ToCharArray(0, Math.Min(msgs[i].text.Length, INCC.ERR_MSG_LENGTH));
						Encoding.ASCII.GetBytes(aa, 0, aa.Length, bb, indx);
						recidx++;
					}
					indx += INCC.ERR_MSG_LENGTH;
				}
				TransferUtils.Copy(bb, 0, rec.error_msg_codes, 0, INCC.NUM_ERROR_MSG_CODES * INCC.ERR_MSG_LENGTH);
				bb = new byte[INCC.NUM_ERROR_MSG_CODES * INCC.ERR_MSG_LENGTH];
				indx = 0; recidx = 0;
				for (int i = 0; i < msgs.Count && recidx < INCC.NUM_ERROR_MSG_CODES; i++)
				{
					if (msgs.Count > i && string.IsNullOrEmpty(msgs[i].text) && msgs[i].IsWarning)
					{
						char[] aa = msgs[i].text.ToCharArray(0, Math.Min(msgs[i].text.Length, INCC.ERR_MSG_LENGTH));
						Encoding.ASCII.GetBytes(aa, 0, aa.Length, bb, indx);
						recidx++;
					}
					indx += INCC.ERR_MSG_LENGTH;
				}
				TransferUtils.Copy(bb, 0, rec.warning_msg_codes, 0, INCC.NUM_ERROR_MSG_CODES * INCC.ERR_MSG_LENGTH);

				// these ride on, or can be computed from, m.Cycles, but they sum across multiple analyzers and so are not considered complete yet for LM measurements
				rec.total_number_runs = (ushort)m.Cycles.GetValidCycleCount(); // any and all cycles
				rec.number_good_runs = (ushort)m.Cycles.GetUseableCycleCount();  // those that are marked OK
				rec.total_good_count_time = (ushort)m.Cycles.GetUseableCycleCount() * m.AcquireState.run_count_time;  // check against time on first cycle, to assert

				rec.singles_sum = mcr.Totals;
				rec.scaler1_sum = mcr.S1Sum;
				rec.scaler2_sum = mcr.S2Sum;
				rec.reals_plus_acc_sum = mcr.RASum;
				rec.acc_sum = mcr.ASum;
				rec.singles = mcr.DeadtimeCorrectedSinglesRate.v;
				rec.singles_err = mcr.DeadtimeCorrectedSinglesRate.err;
				rec.doubles = mcr.DeadtimeCorrectedDoublesRate.v;
				rec.doubles_err = mcr.DeadtimeCorrectedDoublesRate.err;
				rec.triples = mcr.DeadtimeCorrectedTriplesRate.v;
				rec.triples_err = mcr.DeadtimeCorrectedTriplesRate.err;
				rec.scaler1 = mcr.Scaler1.v;
				rec.scaler1_err = mcr.Scaler1.err;
				rec.scaler2 = mcr.Scaler2.v;
				rec.scaler2_err = mcr.Scaler2.err;		
				rec.uncorrected_doubles = mcr.RawDoublesRate.v;
				rec.uncorrected_doubles_err = mcr.RawDoublesRate.err;
				rec.singles_multi = mcr.singles_multi;
				rec.doubles_multi = mcr.doubles_multi;
				rec.triples_multi = mcr.triples_multi;
				rec.declared_mass= mcr.mass;
				TransferUtils.CopyULongsToDbls(mcr.RAMult, rec.mult_reals_plus_acc_sum);
				TransferUtils.CopyULongsToDbls(mcr.NormedAMult, rec.mult_acc_sum);
				for (int ix = 0; ix < 9; ix++)
				    rec.covariance_matrix[ix] = mcr.covariance_matrix[ix];	

				INCCMethodResults imr;
				bool got = m.INCCAnalysisResults.TryGetINCCResults(m.Detector.MultiplicityParams, out imr);
				if (got)
					rec.primary_analysis_method = (byte)NewTypeToOldMethodId(imr.primaryMethod);

				// rec.net_drum_weight =  // NEXT: no entry in INCC6 results for this result value, add it
				// NEXT: duo of passive and active measurent results identifier not yet properly handled in INCC6
                b = StringSquish(m.MeasDate.ToString("yy.MM.dd"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, rec.passive_meas_date);
				TransferUtils.Copy(b, rec.active_meas_date);
                b = StringSquish(m.MeasDate.ToString("HH:mm:ss"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, rec.passive_meas_time);
				TransferUtils.Copy(b, rec.active_meas_time);
				
				b = StringSquish(System.IO.Path.GetFileName(m.MeasurementId.FileName), INCC.FILE_NAME_LENGTH);
				TransferUtils.Copy(b, rec.passive_filename);
				TransferUtils.Copy(b, rec.active_filename);
				b = StringSquish(System.IO.Path.GetFileName(m.MeasurementId.FileName), INCC.FILE_NAME_LENGTH);
				TransferUtils.Copy(b, rec.passive_results_detector_id);
				TransferUtils.Copy(b, rec.active_results_detector_id);
                b = StringSquish(m.Detector.Id.DetectorId, INCC.MAX_DETECTOR_ID_LENGTH);
				TransferUtils.Copy(b, rec.passive_results_detector_id);
				TransferUtils.Copy(b, rec.active_results_detector_id);
				ItemId itid = NC.App.DB.ItemIds.Get(m.AcquireState.item_id);		
				if (itid != null)
				{
					rec.declared_u_mass = itid.declaredUMass;
					rec.length = itid.length;
				}
				rec.db_version = 5.0;
                return rec;
			}

			internal static INCC.SaveResultsMask ResultsMaskMap(AnalysisMethod am)
			{
				INCC.SaveResultsMask r = 0;
				switch (am)
				{
				case AnalysisMethod.KnownA:
					r = INCC.SaveResultsMask.SAVE_KNOWN_ALPHA_RESULTS;
					break;
				case AnalysisMethod.CalibrationCurve:
					r = INCC.SaveResultsMask.SAVE_CAL_CURVE_RESULTS;
					break;
				case AnalysisMethod.KnownM:
					r = INCC.SaveResultsMask.SAVE_KNOWN_M_RESULTS;
					break;
				case AnalysisMethod.Multiplicity:
					r = INCC.SaveResultsMask.SAVE_MULTIPLICITY_RESULTS;
					break;
				case AnalysisMethod.TruncatedMultiplicity:
					r = INCC.SaveResultsMask.SAVE_TRUNCATED_MULT_RESULTS;
					break;
				case AnalysisMethod.AddASource:
					r = INCC.SaveResultsMask.SAVE_ADD_A_SOURCE_RESULTS;
					break;
				case AnalysisMethod.CuriumRatio:
					r = INCC.SaveResultsMask.SAVE_CURIUM_RATIO_RESULTS;
					break;
				case AnalysisMethod.Active:
					r = INCC.SaveResultsMask.SAVE_ACTIVE_RESULTS;
					break;
				case AnalysisMethod.ActivePassive:
					r = INCC.SaveResultsMask.SAVE_ACTIVE_PASSIVE_RESULTS;
					break;
				case AnalysisMethod.ActiveMultiplicity:
					r = INCC.SaveResultsMask.SAVE_ACTIVE_MULTIPLICITY_RESULTS;
					break;
				case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:
					r = INCC.SaveResultsMask.SAVE_DUAL_ENERGY_MULT_RESULTS;
					break;
				case AnalysisMethod.Collar:
					r = INCC.SaveResultsMask.SAVE_COLLAR_RESULTS;
					break;
				default:
					break;
				}

				// r = INCC.SaveResultsMask.SAVE_TRUNCATED_MULT_BKG_RESULTS; break;  
				// todo: ^^ tm bkg handling design incomplete
				return r;

			}

			internal static INCC.SaveResultsMask EncodeResultsStatus(Measurement m)
			{
				INCC.SaveResultsMask res = 0x0;
				switch (m.MeasOption)
				{
				case AssaySelector.MeasurementOption.verification:
				case AssaySelector.MeasurementOption.calibration:
					{
						IEnumerator iter = m.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();

						while (iter.MoveNext())
						{
							MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
							INCCResult ir = m.INCCAnalysisResults[moskey];
							INCCMethodResults imrs;
							bool found = m.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
							if (found && imrs.Count > 0) // should be true for verification and calibration
							{
								// we've got a distinct detector id and material type on the methods, so that is the indexer here
								Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrs[m.INCCAnalysisState.Methods.selector];

								// now get an enumerator over the map of method results
								Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
								while (ai.MoveNext())
								{
									res |= ResultsMaskMap(ai.Current.Key);
								}
							}
						}
					}
					break;
				case AssaySelector.MeasurementOption.initial:
					res |= INCC.SaveResultsMask.SAVE_INIT_SRC_RESULTS;
					break;
				case AssaySelector.MeasurementOption.normalization:
					res |= INCC.SaveResultsMask.SAVE_BIAS_RESULTS;
					break;
				case AssaySelector.MeasurementOption.precision:
					res |= INCC.SaveResultsMask.SAVE_PRECISION_RESULTS;
					break;
				default:
					res = 0;
					break;
				}
				return res;
			}

			internal static unsafe run_rec MoveCycleToRunRec(Cycle c, Multiplicity mkey)
			{
				run_rec res = new run_rec();
				res.run_number = (ushort)c.seq;
                byte[] b = StringSquish(c.DataSourceId.dt.ToString("yy.MM.dd"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, res.run_date);
                b = StringSquish(c.DataSourceId.dt.ToString("HH:mm:ss"), INCC.DATE_TIME_LENGTH);
				TransferUtils.Copy(b, res.run_time);
				QCStatus qc = c.QCStatus(mkey);
				b = StringSquish(QCTestStatusExtensions.INCCString(qc), INCC.MAX_RUN_TESTS_LENGTH); 
				TransferUtils.Copy(b, res.run_tests);
				res.run_count_time = c.TS.TotalSeconds;
				res.run_singles = c.Totals; // raw counts
				MultiplicityCountingRes mcr = c.MultiplicityResults(mkey);
				res.run_scaler1 = mcr.Scaler1.v;
				res.run_scaler2 = mcr.Scaler2.v;
				res.run_reals_plus_acc = mcr.RASum;
				res.run_acc = mcr.ASum;
				TransferUtils.CopyULongsToDbls(mcr.RAMult, res.run_mult_reals_plus_acc);
				TransferUtils.CopyULongsToDbls(mcr.NormedAMult, res.run_mult_acc);
				res.run_singles_rate = mcr.DeadtimeCorrectedSinglesRate.v; // correct counts or not?
				res.run_doubles_rate = mcr.DeadtimeCorrectedDoublesRate.v;
				res.run_triples_rate = mcr.DeadtimeCorrectedTriplesRate.v;
				res.run_scaler1_rate = mcr.Scaler1Rate.v;
				res.run_scaler2_rate = mcr.Scaler2Rate.v;
				res.run_multiplicity_mult = mcr.multiplication;
				res.run_multiplicity_alpha = mcr.multiAlpha;
				res.run_multiplicity_efficiency = mcr.efficiency;
				res.run_mass = mcr.Mass;
				res.run_high_voltage = c.HighVoltage;
				return res;
			}

			internal static unsafe iresultsbase MoveInitSrc(INCCResults ir, MeasOptionSelector mos)
			{
				results_init_src_rec res = new results_init_src_rec();
				INCCResult results;
                bool found = ir.TryGetValue(mos, out results);
				if (found)
				{
					INCCResults.results_init_src_rec t = (INCCResults.results_init_src_rec)results;
					res.init_src_mode = NewToOldBiasTestId(t.mode);
					StatePack(t.pass, res.init_src_pass_fail);
					StrToBytes(INCC.SOURCE_ID_LENGTH,t.init_src_id, res.init_src_id);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveBias(INCCResults ir, MeasOptionSelector mos)
			{
				results_bias_rec res = new results_bias_rec();
				INCCResult results;
                bool found = ir.TryGetValue(mos, out results);
				if (found)
				{
					INCCResults.results_bias_rec t = (INCCResults.results_bias_rec)results;
					res.results_bias_mode = NewToOldBiasTestId(t.mode);
					StatePack(t.pass, res.bias_pass_fail);
					StrToBytes(INCC.SOURCE_ID_LENGTH, t.sourceId, res.bias_source_id);
					res.bias_sngls_rate_expect = t.biasSnglsRateExpect.v; 
					res.bias_sngls_rate_expect_err = t.biasSnglsRateExpect.err; 
					res.bias_sngls_rate_expect_meas = t.biasSnglsRateExpectMeas.v; 
					res.bias_sngls_rate_expect_meas_err = t.biasSnglsRateExpectMeas.err; 
					res.bias_dbls_rate_expect = t.biasDblsRateExpect.v; 
					res.bias_dbls_rate_expect_err = t.biasDblsRateExpect.err; 
					res.bias_dbls_rate_expect_meas = t.biasDblsRateExpectMeas.v; 
					res.bias_dbls_rate_expect_meas_err = t.biasDblsRateExpectMeas.err; 
					res.new_norm_constant = t.newNormConstant.v;
					res.new_norm_constant_err = t.newNormConstant.err;
					res.required_precision = t.requiredPrecision;
					res.required_meas_seconds = t.requiredMeasSeconds;
				}
				return res;
			}
			internal static unsafe iresultsbase MovePrecision(INCCResults ir, MeasOptionSelector mos)
			{
				results_precision_rec res = new results_precision_rec();
				INCCResult results;
                bool found = ir.TryGetValue(mos, out results);
				if (found)
				{
					INCCResults.results_precision_rec t = (INCCResults.results_precision_rec)results;
					StatePack(t.pass, res.prec_pass_fail);
					res.chi_sq_lower_limit = t.chiSqLowerLimit;
					res.chi_sq_upper_limit = t.chiSqUpperLimit;
					res.prec_chi_sq = t.precChiSq;
					res.prec_sample_var = t.precSampleVar;
					res.prec_theoretical_var = t.precTheoreticalVar;
				}
				return res;
			}
			internal static unsafe iresultsbase MoveCalCurve(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_cal_curve_rec res = new results_cal_curve_rec(); 
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.CalibrationCurve, out result);
				if (found)
				{
					INCCMethodResults.results_cal_curve_rec m = (INCCMethodResults.results_cal_curve_rec)result;
					res.cc_pu240e_mass = m.pu240e_mass.v;
					res.cc_pu240e_mass_err = m.pu240e_mass.err;
					res.cc_pu_mass = m.pu_mass.v;
					res.cc_pu_mass_err = m.pu_mass.err;
					res.cc_dcl_pu240e_mass = m.dcl_pu240e_mass;
					res.cc_dcl_pu_mass = m.dcl_pu_mass;
					res.cc_dcl_minus_asy_pu_mass = m.dcl_minus_asy_pu_mass.v;
					res.cc_dcl_minus_asy_pu_mass_err = m.dcl_minus_asy_pu_mass.err;
					res.cc_dcl_minus_asy_pu_mass_pct = m.dcl_minus_asy_pu_mass_pct;
					StatePack(m.pass, res.cc_pass_fail);
					res.cc_dcl_u_mass = m.dcl_u_mass;
					res.cc_length = m.length;
					res.cc_heavy_metal_content = m.heavy_metal_content;
					res.cc_heavy_metal_correction = m.heavy_metal_correction;
					res.cc_heavy_metal_corr_singles = m.heavy_metal_corr_singles.v;
					res.cc_heavy_metal_corr_singles_err = m.heavy_metal_corr_singles.err;
					res.cc_heavy_metal_corr_doubles = m.heavy_metal_corr_doubles.v;
					res.cc_heavy_metal_corr_doubles_err = m.heavy_metal_corr_doubles.err;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.cc_cal_curve_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.cc_cal_curve_item_type);
					res.cc_a_res = m.methodParams.cev.a;
					res.cc_b_res = m.methodParams.cev.b;
					res.cc_c_res = m.methodParams.cev.c;
					res.cc_d_res = m.methodParams.cev.d;
					res.cc_cal_curve_type_res = (double)m.methodParams.CalCurveType;
					res.cc_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.cc_covar_ac_res = m.methodParams.cev.covar(Coeff.a, Coeff.c);
					res.cc_covar_ad_res = m.methodParams.cev.covar(Coeff.a, Coeff.d);
					res.cc_covar_bc_res = m.methodParams.cev.covar(Coeff.b, Coeff.c);
					res.cc_covar_bd_res = m.methodParams.cev.covar(Coeff.b, Coeff.d);
					res.cc_covar_cd_res = m.methodParams.cev.covar(Coeff.c, Coeff.d);
					res.cc_var_a_res = m.methodParams.cev.var_a;
					res.cc_var_b_res = m.methodParams.cev.var_b;
					res.cc_var_c_res = m.methodParams.cev.var_c;
					res.cc_var_d_res = m.methodParams.cev.var_d;
					res.cc_sigma_x_res = m.methodParams.cev.sigma_x;
					res.cc_heavy_metal_corr_factor_res = m.methodParams.heavy_metal_corr_factor;
					res.cc_heavy_metal_reference_res = m.methodParams.heavy_metal_reference;
					res.cc_percent_u235_res = m.methodParams.percent_u235;
				}
				return res;
			}
			internal unsafe static iresultsbase MoveKnownA(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_known_alpha_rec res = new results_known_alpha_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.KnownA, out result);
				if (found)
				{
					INCCMethodResults.results_known_alpha_rec m = (INCCMethodResults.results_known_alpha_rec)result;
					res.ka_mult = m.mult;
					res.ka_alpha = m.alphaK;
					res.ka_mult_corr_doubles = m.mult_corr_doubles.v;
					res.ka_mult_corr_doubles_err = m.mult_corr_doubles.err;
					res.ka_pu240e_mass = m.pu240e_mass.v;
					res.ka_pu240e_mass_err = m.pu240e_mass.err;
					res.ka_pu_mass = m.pu_mass.v;
					res.ka_pu_mass_err = m.pu_mass.err;
					res.ka_dcl_pu240e_mass = m.dcl_pu240e_mass;
					res.ka_dcl_minus_asy_pu_mass = m.dcl_minus_asy_pu_mass.v;
					res.ka_dcl_minus_asy_pu_mass_err = m.dcl_minus_asy_pu_mass.err;
					res.ka_dcl_minus_asy_pu_mass_pct = m.dcl_minus_asy_pu_mass_pct;
					StatePack(m.pass, res.ka_pass_fail);
					res.ka_dcl_u_mass = m.dcl_u_mass;
					res.ka_length = m.length;
					res.ka_heavy_metal_content = m.heavy_metal_content;
					res.ka_heavy_metal_correction = m.heavy_metal_correction;
					res.ka_corr_singles = m.corr_singles.v;
					res.ka_corr_singles_err = m.corr_singles.err;
					res.ka_corr_doubles = m.corr_doubles.v;
					res.ka_corr_doubles_err = m.corr_doubles.err;
					res.ka_corr_factor = m.corr_factor;
					res.ka_dry_alpha_or_mult_dbls = m.dry_alpha_or_mult_dbls;
					res.ka_alpha_wt_res = m.methodParams.alpha_wt;
					res.ka_rho_zero_res = m.methodParams.rho_zero;
					res.ka_k_res = m.methodParams.k;
					res.ka_a_res = m.methodParams.cev.a;
					res.ka_b_res = m.methodParams.cev.b;
					res.ka_var_a_res = m.methodParams.cev.var_a;
					res.ka_var_b_res = m.methodParams.cev.var_b;
					res.ka_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.ka_sigma_x_res = m.methodParams.cev.sigma_x;
					res.ka_known_alpha_type_res = (double)m.methodParams.known_alpha_type;
					res.ka_heavy_metal_corr_factor_res = m.methodParams.heavy_metal_corr_factor;
					res.ka_heavy_metal_reference_res = m.methodParams.heavy_metal_reference;
					res.ka_lower_corr_factor_limit_res = m.methodParams.lower_corr_factor_limit;
					res.ka_upper_corr_factor_limit_res = m.methodParams.upper_corr_factor_limit;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.ka_known_alpha_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.ka_known_alpha_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveKnownM(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_known_m_rec res = new results_known_m_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.KnownM, out result);
				if (found)
				{
					INCCMethodResults.results_known_m_rec m = (INCCMethodResults.results_known_m_rec)result;
					res.km_mult = m.mult;
					res.km_alpha = m.alpha;
					res.km_pu239e_mass = m.pu239e_mass;
					res.km_pu240e_mass = m.pu240e_mass.v;
					res.km_pu240e_mass_err = m.pu240e_mass.err;
					res.km_pu_mass = m.pu_mass.v;
					res.km_pu_mass_err = m.pu_mass.err;
					res.km_dcl_pu240e_mass = m.dcl_pu240e_mass;
					res.km_dcl_minus_asy_pu_mass = m.dcl_minus_asy_pu_mass.v;
					res.km_dcl_minus_asy_pu_mass_err = m.dcl_minus_asy_pu_mass.err;
					res.km_dcl_minus_asy_pu_mass_pct = m.dcl_minus_asy_pu_mass_pct;
					StatePack(m.pass, res.km_pass_fail);
					res.km_sf_rate_res = m.methodParams.sf_rate;
					res.km_vs1_res = m.methodParams.vs1;
					res.km_vs2_res = m.methodParams.vs2;
					res.km_vi1_res = m.methodParams.vi1;
					res.km_vi2_res = m.methodParams.vi2;
					res.km_b_res = m.methodParams.b;
					res.km_c_res = m.methodParams.c;
					res.km_sigma_x_res = m.methodParams.sigma_x;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.km_known_m_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.km_known_m_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveMult(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_multiplicity_rec res = new results_multiplicity_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.Multiplicity, out result);
				if (found)
				{
					INCCMethodResults.results_multiplicity_rec m = (INCCMethodResults.results_multiplicity_rec)result;
					res.mul_mult = m.mult.v;
					res.mul_mult_err = m.mult.err;
					res.mul_alpha = m.alphaK.v;
					res.mul_alpha_err = m.alphaK.err;
					res.mul_corr_factor = m.alphaK.v;
					res.mul_corr_factor_err = m.alphaK.err;
					res.mul_efficiency = m.alphaK.v;
					res.mul_efficiency_err = m.alphaK.err;
					res.mul_pu240e_mass = m.pu240e_mass.v;
					res.mul_pu240e_mass_err = m.pu240e_mass.err;
					res.mul_pu_mass = m.pu_mass.v;
					res.mul_pu_mass_err = m.pu_mass.err;
					res.mul_dcl_pu240e_mass = m.dcl_pu240e_mass;
					res.mul_dcl_minus_asy_pu_mass = m.dcl_minus_asy_pu_mass.v;
					res.mul_dcl_minus_asy_pu_mass_err = m.dcl_minus_asy_pu_mass.err;
					res.mul_dcl_minus_asy_pu_mass_pct = m.dcl_minus_asy_pu_mass_pct;
					StatePack(m.pass, res.mul_pass_fail);
					res.mul_solve_efficiency_res = (byte)m.solve_efficiency_choice;
					res.mul_sf_rate_res = m.methodParams.sf_rate;
					res.mul_vs1_res = m.methodParams.vs1;
					res.mul_vs2_res = m.methodParams.vs2;
					res.mul_vs3_res = m.methodParams.vs3;
					res.mul_vi1_res = m.methodParams.vi1;
					res.mul_vi2_res = m.methodParams.vi2;
					res.mul_vi3_res = m.methodParams.vi3;
					res.mul_a_res = m.methodParams.a;
					res.mul_b_res = m.methodParams.b;
					res.mul_c_res = m.methodParams.c;
					res.mul_sigma_x_res = m.methodParams.sigma_x;
					res.mul_alpha_weight_res = m.methodParams.alpha_weight;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.mul_multiplicity_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.mul_multiplicity_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveDEMult(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_de_mult_rec res = new results_de_mult_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE, out result);
				if (found)
				{
					INCCMethodResults.results_de_mult_rec m = (INCCMethodResults.results_de_mult_rec)result;
					res.de_meas_ring_ratio = m.meas_ring_ratio;
					res.de_interpolated_neutron_energy = m.interpolated_neutron_energy;
					res.de_energy_corr_factor = m.energy_corr_factor;
					TransferUtils.CopyDbls(m.methodParams.neutron_energy, res.de_neutron_energy_res);
					TransferUtils.CopyDbls(m.methodParams.detector_efficiency, res.de_detector_efficiency_res);
					TransferUtils.CopyDbls(m.methodParams.inner_outer_ring_ratio, res.de_inner_outer_ring_ratio_res);
					TransferUtils.CopyDbls(m.methodParams.relative_fission, res.de_relative_fission_res);
					res.de_inner_ring_efficiency_res = m.methodParams.inner_ring_efficiency;
					res.de_outer_ring_efficiency_res = m.methodParams.outer_ring_efficiency;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.de_mult_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.de_mult_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveActPass(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_active_passive_rec res = new results_active_passive_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.ActivePassive, out result);
				if (found)
				{
					INCCMethodResults.results_active_passive_rec m = (INCCMethodResults.results_active_passive_rec)result;
					res.ap_delta_doubles = m.delta_doubles.v;
					res.ap_delta_doubles_err = m.delta_doubles.err;
					res.ap_u235_mass = m.u235_mass.v;
					res.ap_u235_mass_err = m.u235_mass.err;
					res.ap_k0 = m.k0.v;
					res.ap_k1 = m.k1.v;
					res.ap_k1_err = m.k1.err;
					res.ap_k = m.k.v;
					res.ap_k_err = m.k.err;
					res.ap_dcl_u235_mass = m.dcl_u235_mass;
					res.ap_dcl_minus_asy_u235_mass = m.dcl_minus_asy_u235_mass.v;
					res.ap_dcl_minus_asy_u235_mass_err = m.dcl_minus_asy_u235_mass.err;
					res.ap_dcl_minus_asy_u235_mass_pct = m.dcl_minus_asy_u235_mass_pct;
					StatePack(m.pass, res.ap_pass_fail);
					res.ap_active_passive_equation = (byte)m.methodParams.cev.cal_curve_equation;
					res.ap_a_res = m.methodParams.cev.a;
					res.ap_b_res = m.methodParams.cev.b;
					res.ap_c_res = m.methodParams.cev.c;
					res.ap_d_res = m.methodParams.cev.d;
					res.ap_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.ap_covar_ac_res = m.methodParams.cev.covar(Coeff.a, Coeff.c);
					res.ap_covar_ad_res = m.methodParams.cev.covar(Coeff.a, Coeff.d);
					res.ap_covar_bc_res = m.methodParams.cev.covar(Coeff.b, Coeff.c);
					res.ap_covar_bd_res = m.methodParams.cev.covar(Coeff.b, Coeff.d);
					res.ap_covar_cd_res = m.methodParams.cev.covar(Coeff.c, Coeff.d);
					res.ap_var_a_res = m.methodParams.cev.var_a;
					res.ap_var_b_res = m.methodParams.cev.var_b;
					res.ap_var_c_res = m.methodParams.cev.var_c;
					res.ap_var_d_res = m.methodParams.cev.var_d;
					res.ap_sigma_x_res = m.methodParams.cev.sigma_x;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.ap_active_passive_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.ap_active_passive_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveCollar(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_collar_rec res = new results_collar_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.Collar, out result);
				if (found)
				{
					INCCMethodResults.results_collar_rec m = (INCCMethodResults.results_collar_rec)result;
					res.col_u235_mass = m.u235_mass.v;
					res.col_u235_mass_err = m.u235_mass.err;
					res.col_percent_u235 = m.percent_u235;
					res.col_total_u_mass = m.total_u_mass;
					res.col_k0 = m.k0.v;
					res.col_k0_err = m.k0.err;
					res.col_k1 = m.k1.v;
					res.col_k1_err = m.k1.err;
					res.col_k2 = m.k2.v;
					res.col_k2_err = m.k2.err;
					res.col_k3 = m.k3.v;
					res.col_k3_err = m.k3.err;
					res.col_k4 = m.k4.v;
					res.col_k4_err = m.k4.err;
					res.col_k5 = m.k5.v;
					res.col_k5_err = m.k5.err;
					res.col_total_corr_fact = m.total_corr_fact.v;
					res.col_total_corr_fact_err = m.total_corr_fact.err;
					StrToBytes(INCC.SOURCE_ID_LENGTH, m.source_id, res.col_source_id);
					res.col_corr_doubles = m.corr_doubles.v;
					res.col_corr_doubles_err = m.corr_doubles.err;
					res.col_dcl_length = m.dcl_length.v;
					res.col_dcl_length_err = m.dcl_length.err;
					res.col_dcl_total_u235 = m.dcl_total_u235.v;
					res.col_dcl_total_u235_err = m.dcl_total_u235.err;
					res.col_dcl_total_u238 = m.dcl_total_u238.v;
					res.col_dcl_total_u238_err = m.dcl_total_u238.err;
					res.col_dcl_total_rods = m.dcl_total_rods;
					res.col_dcl_total_poison_rods = m.dcl_total_poison_rods;
					res.col_dcl_poison_percent = m.dcl_poison_percent.v;
					res.col_dcl_poison_percent_err = m.dcl_poison_percent.err;
					res.col_dcl_minus_asy_u235_mass = m.dcl_minus_asy_u235_mass.v;
					res.col_dcl_minus_asy_u235_mass_err = m.dcl_minus_asy_u235_mass.err;
					res.col_dcl_minus_asy_u235_mass_pct = m.dcl_minus_asy_u235_mass_pct;
					StatePack(m.pass, res.col_pass_fail);
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.col_collar_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.col_collar_item_type);
					res.col_collar_mode = (byte)(m.methodParams.collar.collar_mode ? 1 : 0);
					res.col_collar_equation = (byte)m.methodParamsC.cev.cal_curve_equation;
					res.col_a_res = m.methodParamsC.cev.a;
					res.col_b_res = m.methodParamsC.cev.b;
					res.col_c_res = m.methodParamsC.cev.c;
					res.col_d_res = m.methodParamsC.cev.d;
					res.col_covar_ab_res = m.methodParamsC.cev.covar(Coeff.a, Coeff.b);
					res.col_covar_ac_res = m.methodParamsC.cev.covar(Coeff.a, Coeff.c);
					res.col_covar_ad_res = m.methodParamsC.cev.covar(Coeff.a, Coeff.d);
					res.col_covar_bc_res = m.methodParamsC.cev.covar(Coeff.b, Coeff.c);
					res.col_covar_bd_res = m.methodParamsC.cev.covar(Coeff.b, Coeff.d);
					res.col_covar_cd_res = m.methodParamsC.cev.covar(Coeff.c, Coeff.d);
					res.col_var_a_res = m.methodParamsC.cev.var_a;
					res.col_var_b_res = m.methodParamsC.cev.var_b;
					res.col_var_c_res = m.methodParamsC.cev.var_c;
					res.col_var_d_res = m.methodParamsC.cev.var_d;
					res.col_sigma_x_res = m.methodParamsC.cev.sigma_x;
					res.col_number_calib_rods_res = m.methodParamsC.number_calib_rods;
					StrToBytes(INCC.MAX_ROD_TYPE_LENGTH, m.methodParamsC.poison_rod_type[0], res.col_poison_rod_type_res);
					res.col_poison_absorption_fact_res = m.methodParamsC.poison_absorption_fact[0];
					res.col_poison_rod_a_res = m.methodParamsC.poison_rod_a[0].v;
					res.col_poison_rod_a_err_res = m.methodParamsC.poison_rod_a[0].err;
					res.col_poison_rod_b_res = m.methodParamsC.poison_rod_b[0].v;
					res.col_poison_rod_b_err_res = m.methodParamsC.poison_rod_b[0].err;
					res.col_poison_rod_c_res = m.methodParamsC.poison_rod_c[0].v;
					res.col_poison_rod_c_err_res = m.methodParamsC.poison_rod_c[0].err;
					res.col_u_mass_corr_fact_a_res =  m.methodParamsC.u_mass_corr_fact_a.v;
					res.col_u_mass_corr_fact_a_err_res =  m.methodParamsC.u_mass_corr_fact_a.err;
					res.col_u_mass_corr_fact_b_res =  m.methodParamsC.u_mass_corr_fact_b.v;
					res.col_u_mass_corr_fact_b_err_res =  m.methodParamsC.u_mass_corr_fact_b.err;
					res.col_sample_corr_fact_res =  m.methodParamsC.sample_corr_fact.v;
					res.col_sample_corr_fact_err_res =  m.methodParamsC.sample_corr_fact.err;
					byte[] b = new byte[INCC.DATE_TIME_LENGTH];
					char[] a = m.methodParamsDetector.reference_date.ToString("yy.MM.dd").ToCharArray();
					Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
					TransferUtils.Copy(b, res.col_reference_date_res);
					res.col_relative_doubles_rate_res = m.methodParamsDetector.relative_doubles_rate;

					byte[] bb = new byte[INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH];
					int indx = 0;
					for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
					{
						char[] aa = m.methodParamsK5.k5_label[i].ToCharArray(0, Math.Min(m.methodParamsK5.k5_label[i].Length, INCC.MAX_K5_LABEL_LENGTH));
						Encoding.ASCII.GetBytes(aa, 0, aa.Length, bb, indx);
						indx += INCC.MAX_K5_LABEL_LENGTH;
					}
					TransferUtils.Copy(bb, 0, res.collar_k5_label_res, 0, INCC.MAX_COLLAR_K5_PARAMETERS * INCC.MAX_K5_LABEL_LENGTH);
					TransferUtils.CopyBoolsToBytes(m.methodParamsK5.k5_checkbox, res.collar_k5_checkbox_res);
					CopyTuples(m.methodParamsK5.k5, res.collar_k5_res, res.collar_k5_err_res, INCC.MAX_COLLAR_K5_PARAMETERS);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveActive(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_active_rec res = new results_active_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.Active, out result);
				if (found)
				{
					INCCMethodResults.results_active_rec m = (INCCMethodResults.results_active_rec)result;
					res.act_u235_mass = m.u235_mass.v;
					res.act_u235_mass_err = m.u235_mass.err;
					res.act_k0 = m.k0.v;
					res.act_k1 = m.k1.v;
					res.act_k1_err = m.k1.err;
					res.act_k = m.k.v;
					res.act_k_err = m.k.err;
					res.act_dcl_u235_mass = m.dcl_u235_mass;
					res.act_dcl_minus_asy_u235_mass = m.dcl_minus_asy_u235_mass.v;
					res.act_dcl_minus_asy_u235_mass_err = m.dcl_minus_asy_u235_mass.err;
					res.act_dcl_minus_asy_u235_mass_pct = m.dcl_minus_asy_u235_mass_pct;
					StatePack(m.pass, res.act_pass_fail);
					res.act_active_equation = (byte)m.methodParams.cev.cal_curve_equation;
					res.act_a_res = m.methodParams.cev.a;
					res.act_b_res = m.methodParams.cev.b;
					res.act_c_res = m.methodParams.cev.c;
					res.act_d_res = m.methodParams.cev.d;
					res.act_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.act_covar_ac_res = m.methodParams.cev.covar(Coeff.a, Coeff.c);
					res.act_covar_ad_res = m.methodParams.cev.covar(Coeff.a, Coeff.d);
					res.act_covar_bc_res = m.methodParams.cev.covar(Coeff.b, Coeff.c);
					res.act_covar_bd_res = m.methodParams.cev.covar(Coeff.b, Coeff.d);
					res.act_covar_cd_res = m.methodParams.cev.covar(Coeff.c, Coeff.d);
					res.act_var_a_res = m.methodParams.cev.var_a;
					res.act_var_b_res = m.methodParams.cev.var_b;
					res.act_var_c_res = m.methodParams.cev.var_c;
					res.act_var_d_res = m.methodParams.cev.var_d;
					res.act_sigma_x_res = m.methodParams.cev.sigma_x;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.act_active_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.act_active_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveActiveMult(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_active_mult_rec res = new results_active_mult_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.ActiveMultiplicity, out result);
				if (found)
				{
					INCCMethodResults.results_active_mult_rec m = (INCCMethodResults.results_active_mult_rec)result;
					res.am_mult = m.mult.v;
					res.am_mult_err = m.mult.err;
					res.am_vt1_res = m.methodParams.vt1;
					res.am_vt2_res = m.methodParams.vt2;
					res.am_vt3_res = m.methodParams.vt3;
					res.am_vf1_res = m.methodParams.vf1;
					res.am_vf2_res = m.methodParams.vf2;
					res.am_vf3_res = m.methodParams.vf3;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.am_mult_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.am_mult_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveCurium(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_curium_ratio_rec res = new results_curium_ratio_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.CuriumRatio, out result);
				if (found)
				{
					INCCMethodResults.results_curium_ratio_rec m = (INCCMethodResults.results_curium_ratio_rec)result;

					res.cr_pu240e_mass = m.pu.pu240e_mass.v;
					res.cr_pu240e_mass_err = m.pu.pu240e_mass.err;
					res.cr_cm_mass = m.cm_mass.v;
					res.cr_cm_mass_err = m.cm_mass.err;
					res.cr_pu_mass = m.pu.mass.v;
					res.cr_pu_mass_err = m.pu.mass.err;
					res.cr_u_mass = m.u.mass.v;
					res.cr_u_mass_err = m.u.mass.err;
					res.cr_u235_mass = m.u235.mass.v;
					res.cr_u235_mass_err = m.u235.mass.err;
					res.cr_dcl_pu_mass = m.pu.dcl_mass;
					res.cr_dcl_minus_asy_pu_mass = m.pu.dcl_minus_asy_mass.v;
					res.cr_dcl_minus_asy_pu_mass_err = m.pu.dcl_minus_asy_mass.err;
					res.cr_dcl_minus_asy_pu_mass_pct = m.pu.dcl_minus_asy_mass_pct;
					res.cr_dcl_minus_asy_u_mass = m.u235.dcl_minus_asy_mass.v;
					res.cr_dcl_minus_asy_u_mass_err = m.u235.dcl_minus_asy_mass.err;
					res.cr_dcl_minus_asy_u_mass_pct = m.u235.dcl_minus_asy_mass_pct;
					res.cr_dcl_minus_asy_u235_mass = m.u235.dcl_minus_asy_mass.v;
					res.cr_dcl_minus_asy_u235_mass_err = m.u235.dcl_minus_asy_mass.err;
					res.cr_dcl_minus_asy_u235_mass_pct = m.u235.dcl_minus_asy_mass_pct;
					StatePack(m.pu.pass, res.cr_pu_pass_fail);
					StatePack(m.u.pass, res.cr_u_pass_fail);

					res.cr_cm_pu_ratio = m.methodParams2.cm_pu_ratio.v;
					res.cr_cm_pu_ratio_err = m.methodParams2.cm_pu_ratio.err;
					res.cr_pu_half_life = m.methodParams2.pu_half_life;
					byte[] b = new byte[INCC.DATE_TIME_LENGTH];
					char[] a = m.methodParams2.cm_pu_ratio_date.ToString("yy.MM.dd").ToCharArray();
					Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
					TransferUtils.Copy(b, res.cr_cm_pu_ratio_date);
					res.cr_cm_u_ratio = m.methodParams2.cm_u_ratio.v;
					res.cr_cm_u_ratio = m.methodParams2.cm_u_ratio.err;
					a = m.methodParams2.cm_u_ratio_date.ToString("yy.MM.dd").ToCharArray();
					Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
					TransferUtils.Copy(b, res.cr_cm_u_ratio_date);
					StrToBytes(INCC.MAX_ITEM_ID_LENGTH, m.methodParams2.cm_id_label, res.cr_cm_id_label);
					StrToBytes(INCC.MAX_ITEM_ID_LENGTH, m.methodParams2.cm_id, res.cr_cm_id);
					StrToBytes(INCC.MAX_ITEM_ID_LENGTH, m.methodParams2.cm_input_batch_id, res.cr_cm_input_batch_id);
					res.cr_dcl_u_mass_res = m.methodParams2.cm_dcl_u_mass;
					res.cr_dcl_u235_mass_res = m.methodParams2.cm_dcl_u_mass;

					res.cr_cm_pu_ratio_decay_corr = m.cm_pu_ratio_decay_corr.v;
					res.cr_cm_pu_ratio_decay_corr_err = m.cm_pu_ratio_decay_corr.err;
					res.cr_cm_u_ratio_decay_corr = m.cm_u_ratio_decay_corr.v;
					res.cr_cm_u_ratio_decay_corr_err = m.cm_u_ratio_decay_corr.err;

					res.cr_curium_ratio_equation = (byte)m.methodParams.cev.cal_curve_equation;
					res.cr_a_res = m.methodParams.cev.a;
					res.cr_b_res = m.methodParams.cev.b;
					res.cr_c_res = m.methodParams.cev.c;
					res.cr_d_res = m.methodParams.cev.d;
					res.cr_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.cr_covar_ac_res = m.methodParams.cev.covar(Coeff.a, Coeff.c);
					res.cr_covar_ad_res = m.methodParams.cev.covar(Coeff.a, Coeff.d);
					res.cr_covar_bc_res = m.methodParams.cev.covar(Coeff.b, Coeff.c);
					res.cr_covar_bd_res = m.methodParams.cev.covar(Coeff.b, Coeff.d);
					res.cr_covar_cd_res = m.methodParams.cev.covar(Coeff.c, Coeff.d);
					res.cr_var_a_res = m.methodParams.cev.var_a;
					res.cr_var_b_res = m.methodParams.cev.var_b;
					res.cr_var_c_res = m.methodParams.cev.var_c;
					res.cr_var_d_res = m.methodParams.cev.var_d;
					res.cr_sigma_x_res = m.methodParams.cev.sigma_x;
					res.curium_ratio_type_res = NewToOldCRVariants(m.methodParams.curium_ratio_type);
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.cr_curium_ratio_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.cr_curium_ratio_item_type);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveTruncMult(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_truncated_mult_rec res = new results_truncated_mult_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.TruncatedMultiplicity, out result);
				if (found)
				{
					INCCMethodResults.results_truncated_mult_rec m = (INCCMethodResults.results_truncated_mult_rec)result;
					res.tm_bkg_singles = m.bkg.Singles.v;
					res.tm_bkg_singles_err = m.bkg.Singles.err;
					res.tm_bkg_zeros = m.bkg.Zeros.v;
					res.tm_bkg_zeros_err = m.bkg.Zeros.err;
					res.tm_bkg_ones = m.bkg.Ones.v;
					res.tm_bkg_ones_err = m.bkg.Ones.err;				
					res.tm_bkg_twos = m.bkg.Twos.v;
					res.tm_bkg_twos_err = m.bkg.Twos.err;
					res.tm_net_singles = m.net.Singles.v;
					res.tm_net_singles_err = m.net.Singles.err;
					res.tm_net_zeros = m.net.Zeros.v;
					res.tm_net_zeros_err = m.net.Zeros.err;
					res.tm_net_ones = m.net.Ones.v;
					res.tm_net_ones_err = m.net.Ones.err;				
					res.tm_net_twos = m.net.Twos.v;
					res.tm_net_twos_err = m.net.Twos.err;
					res.tm_k_alpha = m.k.alpha.v;
					res.tm_k_alpha_err = m.k.alpha.err;
					res.tm_k_pu240e_mass = m.k.pu240e_mass.v;
					res.tm_k_pu240e_mass_err = m.k.pu240e_mass.err;
					res.tm_k_pu_mass = m.k.pu_mass.v;
					res.tm_k_pu_mass_err = m.k.pu_mass.err;
					res.tm_k_dcl_pu240e_mass = m.k.dcl_pu240e_mass;
					res.tm_k_dcl_pu_mass = m.k.dcl_pu_mass;
					res.tm_k_dcl_minus_asy_pu_mass = m.k.dcl_minus_asy_pu_mass.v;
					res.tm_k_dcl_minus_asy_pu_mass_err = m.k.dcl_minus_asy_pu_mass.err;
					res.tm_k_dcl_minus_asy_pu_mass_pct = m.k.dcl_minus_asy_pu_mass_pct;
					StatePack(m.k.pass, res.tm_k_pass_fail);
					res.tm_s_eff = m.s.eff.v;
					res.tm_s_eff_err = m.s.eff.err;
					res.tm_s_alpha = m.s.alpha.v;
					res.tm_s_alpha_err = m.s.alpha.err;
					res.tm_s_pu240e_mass = m.s.pu240e_mass.v;
					res.tm_s_pu240e_mass_err = m.s.pu240e_mass.err;
					res.tm_s_pu_mass = m.s.pu_mass.v;
					res.tm_s_pu_mass_err = m.s.pu_mass.err;
					res.tm_s_dcl_pu240e_mass = m.s.dcl_pu240e_mass;
					res.tm_s_dcl_pu_mass = m.s.dcl_pu_mass;
					res.tm_s_dcl_minus_asy_pu_mass = m.s.dcl_minus_asy_pu_mass.v;
					res.tm_s_dcl_minus_asy_pu_mass_err = m.s.dcl_minus_asy_pu_mass.err;
					res.tm_s_dcl_minus_asy_pu_mass_pct = m.s.dcl_minus_asy_pu_mass_pct;
					StatePack(m.s.pass, res.tm_s_pass_fail);
					res.tm_a_res = m.methodParams.a;
					res.tm_b_res = m.methodParams.b;
					res.tm_known_eff_res = (byte)(m.methodParams.known_eff ? 1 : 0);
					res.tm_solve_eff_res = (byte)(m.methodParams.known_eff ? 1 : 0);
				}
				return res;
			}
			internal static unsafe iresultsbase MoveTruncBkg(INCCAnalysisState ias, MeasOptionSelector mos) // NEXT: confused with tm_bkg and bkg measurements
			{
				results_tm_bkg_rec res = new results_tm_bkg_rec();
				INCCMethodResult result;
                bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.None, out result);
				if (found)
				{
					INCCMethodResults.results_tm_bkg_rec m = (INCCMethodResults.results_tm_bkg_rec)result;  // todo: tm bkg handling design incomplete
					res.results_tm_singles_bkg = m.methodParams.Singles.v;
					res.results_tm_singles_bkg_err = m.methodParams.Singles.err;
					res.results_tm_zeros_bkg = m.methodParams.Zeros.v;
					res.results_tm_zeros_bkg_err = m.methodParams.Zeros.err;
					res.results_tm_ones_bkg = m.methodParams.Ones.v;
					res.results_tm_ones_bkg_err = m.methodParams.Ones.err;
					res.results_tm_twos_bkg = m.methodParams.Twos.v;
					res.results_tm_twos_bkg_err = m.methodParams.Twos.err;
				}
				return res;
			}
			internal static unsafe iresultsbase MoveAAS(INCCAnalysisState ias, MeasOptionSelector mos)
			{
				results_add_a_source_rec res = new results_add_a_source_rec();
				INCCMethodResult result;
				bool found = ias.Results.TryGetMethodResults(mos.MultiplicityParams, ias.Methods.selector, AnalysisMethod.AddASource, out result);
				if (found)
				{
					INCCMethodResults.results_add_a_source_rec m = (INCCMethodResults.results_add_a_source_rec)result;
					res.ad_dzero_cf252_doubles = m.dzero_cf252_doubles;
					CopyTuples(m.sample_cf252_doubles, res.ad_sample_cf252_doubles, res.ad_sample_cf252_doubles_err, INCC.MAX_ADDASRC_POSITIONS);
					TransferUtils.CopyDbls(m.sample_cf252_ratio, res.ad_sample_cf252_ratio);
					res.ad_sample_avg_cf252_doubles = m.sample_avg_cf252_doubles.v;
					res.ad_sample_avg_cf252_doubles_err = m.sample_avg_cf252_doubles.err;
					res.ad_corr_doubles = m.corr_doubles.v;
					res.ad_corr_doubles_err = m.corr_doubles.err;
					res.ad_delta = m.delta.v;
					res.ad_delta_err = m.delta.err;
					res.ad_corr_factor = m.corr_factor.v;
					res.ad_corr_factor_err = m.corr_factor.err;
					res.ad_pu240e_mass = m.pu240e_mass.v;
					res.ad_pu240e_mass_err = m.pu240e_mass.err;
					res.ad_pu_mass = m.pu_mass.v;
					res.ad_pu_mass_err = m.pu_mass.err;
					res.ad_dcl_pu240e_mass = m.dcl_pu240e_mass;
					res.ad_dcl_pu_mass = m.dcl_pu_mass;
					res.ad_dcl_minus_asy_pu_mass = m.dcl_minus_asy_pu_mass.v;
					res.ad_dcl_minus_asy_pu_mass_err = m.dcl_minus_asy_pu_mass.err;
					res.ad_dcl_minus_asy_pu_mass_pct = m.dcl_minus_asy_pu_mass_pct;
					StatePack(m.pass, res.ad_pass_fail);
					res.ad_tm_corr_doubles = m.tm_corr_doubles.v;
					res.ad_tm_corr_doubles_err = m.tm_corr_doubles.err;
					res.ad_tm_doubles_bkg = m.tm_doubles_bkg.v;
					res.ad_tm_doubles_bkg_err = m.tm_doubles_bkg.err;
					res.ad_tm_uncorr_doubles = m.tm_uncorr_doubles.v;
					res.ad_tm_uncorr_doubles_err = m.tm_uncorr_doubles.err;

					res.ad_add_a_source_equation = (byte)m.methodParams.cev.cal_curve_equation;
					res.ad_a_res = m.methodParams.cev.a;
					res.ad_b_res = m.methodParams.cev.b;
					res.ad_c_res = m.methodParams.cev.c;
					res.ad_d_res = m.methodParams.cev.d;
					res.ad_covar_ab_res = m.methodParams.cev.covar(Coeff.a, Coeff.b);
					res.ad_covar_ac_res = m.methodParams.cev.covar(Coeff.a, Coeff.c);
					res.ad_covar_ad_res = m.methodParams.cev.covar(Coeff.a, Coeff.d);
					res.ad_covar_bc_res = m.methodParams.cev.covar(Coeff.b, Coeff.c);
					res.ad_covar_bd_res = m.methodParams.cev.covar(Coeff.b, Coeff.d);
					res.ad_covar_cd_res = m.methodParams.cev.covar(Coeff.c, Coeff.d);
					res.ad_var_a_res = m.methodParams.cev.var_a;
					res.ad_var_b_res = m.methodParams.cev.var_b;
					res.ad_var_c_res = m.methodParams.cev.var_c;
					res.ad_var_d_res = m.methodParams.cev.var_d;
					res.ad_sigma_x_res = m.methodParams.cev.sigma_x;
					TransferUtils.CopyDbls(m.methodParams.position_dzero, res.ad_position_dzero_res);
					res.ad_dzero_avg_res = m.methodParams.dzero_avg;
					byte[] b = new byte[INCC.DATE_TIME_LENGTH];
					char[] a = m.methodParams.dzero_ref_date.ToString("yy.MM.dd").ToCharArray();
					Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
					TransferUtils.Copy(b, res.ad_dzero_ref_date_res);
					res.ad_num_runs_res = m.methodParams.num_runs;
					res.ad_cf_a_res = m.methodParams.cf.a;
					res.ad_cf_b_res = m.methodParams.cf.b;
					res.ad_cf_c_res = m.methodParams.cf.c;
					res.ad_cf_d_res = m.methodParams.cf.d;
					res.ad_use_truncated_mult_res = (m.methodParams.use_truncated_mult ? 1 : 0);
					res.ad_tm_weighting_factor_res = m.methodParams.tm_weighting_factor;
					res.ad_tm_dbls_rate_upper_limit_res = m.methodParams.tm_dbls_rate_upper_limit;
					StrToBytes(INCC.MAX_DETECTOR_ID_LENGTH, ias.Methods.selector.detectorid, res.ad_add_a_source_detector_id);
					StrToBytes(INCC.MAX_ITEM_TYPE_LENGTH, ias.Methods.selector.material, res.ad_add_a_source_item_type);
				}
				return res;
			}

			internal static unsafe void StatePack(bool state, byte* b)
			{
				if (state)
					TransferUtils.PassPack(b);
				else
					TransferUtils.PassPack(b);
			}

			internal static unsafe void StrToBytes(int maxlen, string src, byte* tgt)
			{
				char[] a = src.ToCharArray(0, Math.Min(src.Length, maxlen));
				byte[] b = new byte[maxlen];
				Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
				TransferUtils.Copy(b, tgt);
			}
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
			if (srtype.IsListMode())
				det.Id.FullConnInfo = new LMConnectionInfo();

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
				ABKey abkey = new ABKey(mkey, 512); // NEXT: maxbins is arbitrary
				LMRawAnalysis.SDTMultiplicityCalculator.SetAlphaBeta(abkey, det.AB);
                mcr.AB.TransferIntermediates(det.AB);
            }
            catch (Exception e)
            {
                mlogger.TraceEvent(LogLevels.Warning, 34064, "Detector transfer processing error {0} {1} ({2})", det.Id.DetectorName, e.Message, System.IO.Path.GetFileName(iddf.Path));
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

        public static int OldTypeToOldMethodId(object o)
        {
			Type t = o.GetType();
            if (t.Equals(typeof(analysis_method_rec)))
                return INCC.METHOD_NONE;	// No analysis method selected
            else if (t.Equals(typeof(cal_curve_rec)))
                return INCC.METHOD_CALCURVE;	// Passive Calibration Curve
            else if (t.Equals(typeof(known_alpha_rec)))
                return INCC.METHOD_AKNOWN;	// Known Alpha
            else if (t.Equals(typeof(known_m_rec)))
                return INCC.METHOD_MKNOWN;	// Known M
            else if (t.Equals(typeof(multiplicity_rec)))
                return INCC.METHOD_MULT;// Multiplicity
            else if (t.Equals(typeof(add_a_source_rec)))
                return INCC.METHOD_ADDASRC;	// Add-a-source
            else if (t.Equals(typeof(active_rec)))
                return INCC.METHOD_ACTIVE;	// Active Calibration Curve
            else if (t.Equals(typeof(active_mult_rec)))
                return INCC.METHOD_ACTIVE_MULT;	// Active Multiplicity
            else if (t.Equals(typeof(active_passive_rec)))
                return INCC.METHOD_ACTPAS;	// Active/Passive
            else if (t.Equals(typeof(curium_ratio_rec)))
                return INCC.METHOD_CURIUM_RATIO;	// Curium Ratio
            else if (t.Equals(typeof(truncated_mult_rec)))
                return INCC.METHOD_TRUNCATED_MULT;	// Truncated multiplicity 
            else if (t.Equals(typeof(collar_detector_rec)))
                return INCC.COLLAR_DETECTOR_SAVE_RESTORE;
            else if (t.Equals(typeof(collar_rec)))
                return INCC.COLLAR_SAVE_RESTORE;
            else if (t.Equals(typeof(collar_k5_rec)))
                return INCC.COLLAR_K5_SAVE_RESTORE;
            else if (t.Equals(typeof(de_mult_rec)))
                return INCC.DUAL_ENERGY_MULT_SAVE_RESTORE;

            return -1;
        }

        private static int NewTypeToOldMethodId(AnalysisMethod am)
        {
            int id = 0;
            if (am == AnalysisMethod.None)
                id = INCC.METHOD_NONE;
            else if (am <= AnalysisMethod.TruncatedMultiplicity)
                id = (int)(am) - 1;
            else
                switch (am)
                {
                    case AnalysisMethod.DUAL_ENERGY_MULT_SAVE_RESTORE:	// Dual energy multiplicity
                        id = INCC.DUAL_ENERGY_MULT_SAVE_RESTORE;
                        break;
                    case AnalysisMethod.Collar:
                    case AnalysisMethod.COLLAR_SAVE_RESTORE:
                        id = INCC.COLLAR_SAVE_RESTORE;
                        break;
                    case AnalysisMethod.COLLAR_DETECTOR_SAVE_RESTORE:
                        id = INCC.COLLAR_DETECTOR_SAVE_RESTORE;
                        break;
                    case AnalysisMethod.COLLAR_K5_SAVE_RESTORE:
                        id = INCC.COLLAR_K5_SAVE_RESTORE;
                        break;
                    case AnalysisMethod.WMV_CALIB_TOKEN:
                        id = INCC.WMV_CALIB_TOKEN;
                        break;
                }
            return id;
        }


        static unsafe public Tuple[] Copy(double* vptr, double* errptr, int len)
        {

            Tuple[] vals = new Tuple[len];
            for (int i = 0; i < len; i++)
                vals[i] = new Tuple(vptr[i], errptr[i]);
            return vals;
        }

		static unsafe public void CopyTuples(Tuple[] src, double* dstval, double* dsterr, int maxlen)
        {
            if (dstval == null || dsterr == null || src == null)
				throw new ArgumentException();            

			int len = src.Length;
			double [] v = new double[maxlen];
			double [] err = new double[maxlen];

			for (int i = 0; i < maxlen; i++)
			{
				v[i] = src[i].v;
				err[i] = src[i].err;
			}
			TransferUtils.CopyDbls(v, dstval);
			TransferUtils.CopyDbls(err, dsterr);

		}

        public unsafe void BuildCalibration(INCCInitialDataCalibrationFile idcf, int num)
        {

            mlogger.TraceEvent(LogLevels.Verbose, 34200, "Building calibration content from {0} {1}", num, idcf.Path);
            bool overwrite = NC.App.AppContext.OverwriteImportedDefs;
            IEnumerator iter = idcf.DetectorMaterialMethodParameters.GetDetectorMaterialEnumerator();
            while (iter.MoveNext())
            {
                DetectorMaterialMethod mkey = ((KeyValuePair<DetectorMaterialMethod, object>)iter.Current).Key;

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
                                am.choices[(int)AnalysisMethod.None] = am.choices[(int)AnalysisMethod.INCCNone] = false;
                            break;
                        case INCC.METHOD_CALCURVE:
                            cal_curve_rec cal_curve = (cal_curve_rec)miter2.Current;
                            INCCAnalysisParams.cal_curve_rec cc = new INCCAnalysisParams.cal_curve_rec();
                            cc.heavy_metal_corr_factor = cal_curve.cc_heavy_metal_corr_factor;
                            cc.heavy_metal_reference = cal_curve.cc_heavy_metal_reference;
                            cc.cev.lower_mass_limit = cal_curve.cc_lower_mass_limit;
                            cc.cev.upper_mass_limit = cal_curve.cc_upper_mass_limit;
                            cc.percent_u235 = cal_curve.cc_percent_u235;
                            TransferUtils.Copy(ref cc.dcl_mass, cal_curve.cc_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            TransferUtils.Copy(ref cc.doubles, cal_curve.cc_doubles, INCC.MAX_NUM_CALIB_PTS);
  
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
                            TransferUtils.Copy(ref ka.dcl_mass, known_alpha.ka_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            TransferUtils.Copy(ref ka.doubles, known_alpha.ka_doubles, INCC.MAX_NUM_CALIB_PTS);
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
                            TransferUtils.Copy(ref de.detector_efficiency, de_mult.de_detector_efficiency, INCC.MAX_DUAL_ENERGY_ROWS);
                            TransferUtils.Copy(ref de.inner_outer_ring_ratio, de_mult.de_inner_outer_ring_ratio, INCC.MAX_DUAL_ENERGY_ROWS);
                            TransferUtils.Copy(ref de.neutron_energy, de_mult.de_neutron_energy, INCC.MAX_DUAL_ENERGY_ROWS);
                            TransferUtils.Copy(ref de.relative_fission, de_mult.de_relative_fission, INCC.MAX_DUAL_ENERGY_ROWS);
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

                            TransferUtils.Copy(ref aas.dcl_mass, add_a_source.ad_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            TransferUtils.Copy(ref aas.doubles, add_a_source.ad_doubles, INCC.MAX_NUM_CALIB_PTS);

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
                            TransferUtils.Copy(ref ar.dcl_mass, active.act_dcl_mass, INCC.MAX_NUM_CALIB_PTS);
                            TransferUtils.Copy(ref ar.doubles, active.act_doubles, INCC.MAX_NUM_CALIB_PTS);

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
                        case INCC.COLLAR_SAVE_RESTORE:
 							mlogger.TraceEvent(LogLevels.Verbose, 34213, " Collar params entry for COLLAR_SAVE_RESTORE");							
							break;					
                        case INCC.COLLAR_DETECTOR_SAVE_RESTORE:
 							mlogger.TraceEvent(LogLevels.Verbose, 34212, " Main entry for COLLAR_DETECTOR_SAVE_RESTORE");
                            break;
                        case INCC.COLLAR_K5_SAVE_RESTORE:
 							mlogger.TraceEvent(LogLevels.Verbose, 34214, " K5 entry for COLLAR_K5_SAVE_RESTORE");
                            collar_k5_rec collar_k5 = (collar_k5_rec)miter2.Current;
							INCCAnalysisParams.collar_combined_rec combined = CollarEntryProcesser(idcf, sel, collar_k5.collar_k5_mode);
							if (combined != null)
								am.AddMethod(AnalysisMethod.Collar, combined);							
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

		unsafe collar_detector_rec MakeAFake(collar_detector_rec src, string det, string mat)
		{
			collar_detector_rec m = new collar_detector_rec();
			byte[] b = new byte[INCC.MAX_DETECTOR_ID_LENGTH];
			char[] a = det.ToCharArray(0, Math.Min(det.Length, INCC.MAX_DETECTOR_ID_LENGTH));
			Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
			TransferUtils.Copy(b, m.collar_detector_id);
			b = new byte[INCC.MAX_ITEM_TYPE_LENGTH];
			a = mat.ToCharArray(0, Math.Min(mat.Length, INCC.MAX_ITEM_TYPE_LENGTH));
			Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
			TransferUtils.Copy(b, m.collar_detector_item_type);
			//m.collar_detector_mode = (byte)(imr.collar_det.collar_mode ? 1 : 0);

			b = new byte[INCC.DATE_TIME_LENGTH];
			a = @"89.10.17".ToCharArray();
			Encoding.ASCII.GetBytes(a, 0, a.Length, b, 0);
			TransferUtils.Copy(b, m.col_reference_date);
			m.col_relative_doubles_rate = 1.0;
			return m;
		}

		INCCAnalysisParams.collar_combined_rec CollarEntryProcesser(INCCInitialDataCalibrationFile idcf, INCCSelector sel, byte mode)
		{
			DetectorMaterialMethod m1 = new DetectorMaterialMethod(sel.material, sel.detectorid, INCC.COLLAR_DETECTOR_SAVE_RESTORE); m1.extra = mode;
			DetectorMaterialMethod m2 = new DetectorMaterialMethod(sel.material, sel.detectorid, INCC.COLLAR_SAVE_RESTORE); m2.extra = mode;
			DetectorMaterialMethod m3 = new DetectorMaterialMethod(sel.material, sel.detectorid, INCC.COLLAR_K5_SAVE_RESTORE); m3.extra = mode;

			KeyValuePair<DetectorMaterialMethod, object> k1, k2, k3;
			bool ok = idcf.DetectorMaterialMethodParameters.GetPair(m1, out k1);
			if (!ok)
			{
				mlogger.TraceEvent(LogLevels.Verbose, 30811, "No collar detector values for " + m1.ToString());
				ok = idcf.DetectorMaterialMethodParameters.GetPair(m2, out k2);
				if (ok)
				{
					// k5 and collar but no det entry, find the closest match det entry and make a fake one and use it
					List<KeyValuePair<DetectorMaterialMethod, object>> l = idcf.DetectorMaterialMethodParameters.GetDetectorsWithEntries;
					if (l.Count > 0)
					{
						object o = l[0].Value;
						collar_detector_rec rec = MakeAFake((collar_detector_rec)o, sel.detectorid, sel.material);
                        DetectorMaterialMethod mf1 = new DetectorMaterialMethod(sel.material, sel.detectorid, INCC.COLLAR_DETECTOR_SAVE_RESTORE); mf1.extra = mode;
                        k1 = new KeyValuePair<DetectorMaterialMethod, object>(mf1, rec);
					}
					else
						return null;
				}
				else
				{
					mlogger.TraceEvent(LogLevels.Verbose, 30812, "No collar values for " + m2.ToString());
					return null;
				}
			}
			else
			{
				ok = idcf.DetectorMaterialMethodParameters.GetPair(m2, out k2);
				if (!ok)
				{
					mlogger.TraceEvent(LogLevels.Verbose, 30812, "No collar values for " + m2.ToString());
					return null;
				}
			}
			ok = idcf.DetectorMaterialMethodParameters.GetPair(m3, out k3);
			if (!ok)
			{
				mlogger.TraceEvent(LogLevels.Verbose, 30813, "No k5 values for " + m3.ToString());
				return null;
			}
			collar_rec collar;
			collar_detector_rec collar_detector;
			collar_k5_rec collar_k5;
			if (k1.Key.extra == -1)
				return null;
			collar_detector = (collar_detector_rec)k1.Value;
			if (k2.Key.extra == -1)
				return null;
			collar = (collar_rec)k2.Value;
			if (k3.Key.extra == -1)
				return null;
			collar_k5 = (collar_k5_rec)k3.Value;
			// got the three
			INCCAnalysisParams.collar_combined_rec combined = new INCCAnalysisParams.collar_combined_rec();
			ushort bonk = 0;
			CollarDet(combined, collar_detector, bonk);
			bonk = 1;
			CollarParm(combined, collar, bonk);
			bonk = 2;
			CollarK5(combined, collar_k5, bonk);
			bonk = 3;

			k1.Key.extra = -1;
			k2.Key.extra = -1;
			k3.Key.extra = -1;
			return combined;
		}

		unsafe void CollarK5(INCCAnalysisParams.collar_combined_rec combined, collar_k5_rec collar_k5, ushort bonk)
		{
			combined.k5 = new INCCAnalysisParams.collar_k5_rec();
			combined.k5.k5_mode = (collar_k5.collar_k5_mode  != 0 ? true: false);
			combined.k5.k5_item_type = TransferUtils.str(collar_k5.collar_k5_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
			combined.k5.k5 = Copy(collar_k5.collar_k5, collar_k5.collar_k5_err, INCC.MAX_COLLAR_K5_PARAMETERS);
			combined.k5.k5_checkbox = TransferUtils.Copy(collar_k5.collar_k5_checkbox,INCC.MAX_COLLAR_K5_PARAMETERS);
			for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
			{
				int index = i * INCC.MAX_K5_LABEL_LENGTH;
				combined.k5.k5_label[i] = TransferUtils.str(collar_k5.collar_k5_label + index, INCC.MAX_K5_LABEL_LENGTH);
			}
			mlogger.TraceEvent(LogLevels.Verbose, 34214, " -- Collar k5 has mode {0} and is ready to be bonked", combined.k5.k5_mode);
		}

		unsafe void CollarDet(INCCAnalysisParams.collar_combined_rec combined, collar_detector_rec collar_detector, ushort bonk)
		{
			combined.collar_det = new INCCAnalysisParams.collar_detector_rec();
			combined.collar_det.collar_mode = (collar_detector.collar_detector_mode == 0 ? false : true);
			combined.collar_det.reference_date = INCC.DateFrom(TransferUtils.str(collar_detector.col_reference_date, INCC.DATE_TIME_LENGTH));
			combined.collar_det.relative_doubles_rate = collar_detector.col_relative_doubles_rate;
			mlogger.TraceEvent(LogLevels.Verbose, 34212, " -- Collar det has mode {0} {1}",  combined.collar_det.collar_mode, bonk);
		}

		unsafe void CollarParm(INCCAnalysisParams.collar_combined_rec combined, collar_rec collar, ushort bonk)
		{
			combined.collar = new INCCAnalysisParams.collar_rec();
			combined.collar.cev.lower_mass_limit = collar.col_lower_mass_limit;
			combined.collar.cev.upper_mass_limit = collar.col_upper_mass_limit;
			combined.collar.cev.a = collar.col_a;
			combined.collar.cev.b = collar.col_b;
			combined.collar.cev.c = collar.col_c;
			combined.collar.cev.d = collar.col_d;
			combined.collar.cev.var_a = collar.col_var_a;
			combined.collar.cev.var_b = collar.col_var_b;
			combined.collar.cev.var_c = collar.col_var_c;
			combined.collar.cev.var_d = collar.col_var_d;
			combined.collar.cev.setcovar(Coeff.a, Coeff.b, collar.col_covar_ab);
			combined.collar.cev._covar[0, 2] = collar.col_covar_ac;
			combined.collar.cev._covar[0, 3] = collar.col_covar_ad;
			combined.collar.cev._covar[1, 2] = collar.col_covar_bc;
			combined.collar.cev._covar[1, 3] = collar.col_covar_bd;
			combined.collar.cev._covar[2, 3] = collar.col_covar_cd;
			combined.collar.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)collar.collar_equation;
			combined.collar.cev.sigma_x = collar.col_sigma_x;

			combined.collar.poison_absorption_fact = TransferUtils.Copy(collar.col_poison_absorption_fact, INCC.MAX_POISON_ROD_TYPES);
			combined.collar.poison_rod_a = Copy(collar.col_poison_rod_a, collar.col_poison_rod_a_err, INCC.MAX_POISON_ROD_TYPES);
			combined.collar.poison_rod_b = Copy(collar.col_poison_rod_b, collar.col_poison_rod_b_err, INCC.MAX_POISON_ROD_TYPES);
			combined.collar.poison_rod_c = Copy(collar.col_poison_rod_c, collar.col_poison_rod_c_err, INCC.MAX_POISON_ROD_TYPES);
			combined.collar.collar_mode = (collar.collar_mode == 0 ? false : true);
			combined.collar.u_mass_corr_fact_a.v = collar.col_u_mass_corr_fact_a;
			combined.collar.u_mass_corr_fact_a.err = collar.col_u_mass_corr_fact_a_err;
			combined.collar.u_mass_corr_fact_b.v = collar.col_u_mass_corr_fact_b;
			combined.collar.u_mass_corr_fact_b.err = collar.col_u_mass_corr_fact_b_err;
			combined.collar.sample_corr_fact.v = collar.col_sample_corr_fact;
			combined.collar.sample_corr_fact.err = collar.col_sample_corr_fact_err;
			combined.collar.number_calib_rods = (int)collar.col_number_calib_rods;
			for (int i = 0; i < INCC.MAX_POISON_ROD_TYPES; i++)
			{
				int index = i * INCC.MAX_ROD_TYPE_LENGTH;
				combined.collar.poison_rod_type[i] = TransferUtils.str(collar.col_poison_rod_type + index, INCC.MAX_ROD_TYPE_LENGTH);
			}
			mlogger.TraceEvent(LogLevels.Verbose, 34213, " -- Collar params has mode {0} {1}", combined.collar.collar_mode, bonk);
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

		private static ushort NewToOldBiasTestId(NormTest id)
        {
            ushort nt = INCC.IDC_BIAS_SINGLES;
            switch (id)
            {
				case NormTest.AmLiSingles:				
                    nt = INCC.IDC_BIAS_SINGLES;
                    break;
				case NormTest.Cf252Doubles:				
                    nt = INCC.IDC_BIAS_DOUBLES;
                    break;
				case NormTest.Collar:				
                    nt = INCC.IDC_BIAS_COLLAR;
                    break;
				case NormTest.Cf252Singles:				
                    nt = INCC.IDC_BIAS_CF252_SINGLES;
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
		private static ushort NewToOldCRVariants(INCCAnalysisParams.CuriumRatioVariant id)
        {
            ushort isdh = INCC.IDC_USE_DOUBLES;
            switch (id)
            {
                case INCCAnalysisParams.CuriumRatioVariant.UseDoubles:
                    isdh = INCC.IDC_USE_DOUBLES;
                    break;
			case INCCAnalysisParams.CuriumRatioVariant.UseSingles:  
                    isdh = INCC.IDC_USE_SINGLES;
                    break;
                case INCCAnalysisParams.CuriumRatioVariant.UseAddASourceDoubles:
                    isdh = INCC.IDC_USE_ADD_A_SOURCE_DOUBLES;
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

		private static ushort NewToOldAASId(AddASourceFlavors id)
        {
            ushort nt = 0;
            switch (id)
            {
                case AddASourceFlavors.None:
                    nt = INCC.IDC_NONE;
                    break;
                case AddASourceFlavors.CompuMotor_4000:
                    nt = INCC.IDC_COMPUMOTOR_4000;
                    break;
                case AddASourceFlavors.CompuMotor_3000:
                    nt = INCC.IDC_COMPUMOTOR_3000;
                    break;
                case AddASourceFlavors.PLC_JCC21:
                    nt = INCC.IDC_PLC_JCC21;
                    break;
                case AddASourceFlavors.PLC_WM3100:
                    nt = INCC.IDC_PLC_WM3100;
                    break;
                case AddASourceFlavors.Canberra_Counter:
                    nt = INCC.IDC_CANBERRA_COUNTER;
                    break;
                case AddASourceFlavors.Manual:
                    nt = INCC.IDC_MANUAL;
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

		public class TransferSummary
		{
			public DateTimeOffset dto;
			public string item, stratum, path, det, comment, material;
			public AssaySelector.MeasurementOption meastype;
			public bool select; 
			public int index;
			public TransferSummary()
			{
			}
		}

		public static unsafe TransferSummary ConstructSummary(INCCTransferFile itf, int index)
		{
			TransferSummary t = new TransferSummary();
			results_rec results = itf.results_rec_list[0];
			t.path = itf.Path;
			t.det = TransferUtils.str(results.results_detector_id, INCC.MAX_DETECTOR_ID_LENGTH);
            t.stratum = TransferUtils.str(results.stratum_id, INCC.MAX_STRATUM_ID_LENGTH);
            t.item = TransferUtils.str(results.item_id, INCC.MAX_ITEM_ID_LENGTH);
			t.dto = INCC.DateTimeFrom(TransferUtils.str(results.meas_date, INCC.DATE_TIME_LENGTH), TransferUtils.str(results.meas_time, INCC.DATE_TIME_LENGTH));
			t.comment = TransferUtils.str(results.comment, INCC.MAX_COMMENT_LENGTH);
            t.material = TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
			t.meastype = (AssaySelector.MeasurementOption)results.meas_option;
			t.select = false;
			t.index = index;
			return t;
		}

		public static List<TransferSummary> ConstructSummaryList(List<INCCTransferBase> it)
		{
			List<TransferSummary> list = new List<TransferSummary>();
			foreach (INCCTransferBase itf in it)
			{ 
				if (itf is INCCTransferFile)
				{
					int idx = it.IndexOf(itf);
					list.Add(ConstructSummary((INCCTransferFile)itf, idx));
				}
			}
			return list;
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
                bool okparse = Enum.TryParse(check, true, out checksc);
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
            acq.meas_detector_id = string.Copy(det.Id.DetectorId);  // probably incorrect usage, but differnce is ambiguous in INCC5
            acq.item_type = TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
            acq.qc_tests = TransferUtils.ByteBool(results.results_qc_tests);
            acq.user_id = TransferUtils.str(results.user_id, INCC.CHAR_FIELD_LENGTH);
            acq.num_runs = results.total_number_runs;
            if (results.number_good_runs > 0)
                acq.run_count_time = results.total_good_count_time / results.number_good_runs;
            else
                acq.run_count_time = results.total_good_count_time; // should be 0.0 by default for this special case
            acq.MeasDateTime = meas.MeasurementId.MeasDateTime;
            acq.error_calc_method = INCCErrorCalculationTechnique(results.error_calc_method);
            acq.campaign_id = TransferUtils.str(results.results_campaign_id, INCC.MAX_CAMPAIGN_ID_LENGTH);
            if (string.IsNullOrEmpty(acq.campaign_id))
                acq.campaign_id = TransferUtils.str(results.results_inspection_number, INCC.MAX_CAMPAIGN_ID_LENGTH);
			acq.comment = TransferUtils.str(results.comment, INCC.MAX_COMMENT_LENGTH);//"Original file name " + meas.MeasurementId.FileName;
			string ending_comment_str = TransferUtils.str(results.ending_comment, INCC.MAX_COMMENT_LENGTH);
			acq.ending_comment = !string.IsNullOrEmpty(acq.ending_comment_str);
			
			acq.data_src = (ConstructedSource)results.data_source;
			acq.well_config = (WellConfiguration)results.well_config;
			acq.print = TransferUtils.ByteBool(results.results_print);

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

            if (itf.item_id_table.Count > 0)  // devnote: there should be only one item entry
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
                mlogger.TraceEvent(LogLevels.Error, 34063, "No analysis methods for {0}, (calibration information is missing), creating placeholders", sel.ToString()); // devnote: can get missing paramters from the meas results for calib and verif below, so need to visit this condition after results processing below (newres.methodParams!) and reconstruct the calib parameters. 
                meas.INCCAnalysisState.Methods = new AnalysisMethods(mlogger);
                meas.INCCAnalysisState.Methods.selector = sel;
            }
            // prepare analyzer params from sr params above
            meas.AnalysisParams = new AnalysisDefs.CountingAnalysisParameters();
            meas.AnalysisParams.Add(det.MultiplicityParams);

            mlogger.TraceEvent(LogLevels.Verbose, 34030, "Transferring the {0} cycles", itf.run_rec_list.Count);
            meas.InitializeContext();
            meas.PrepareINCCResults(); // prepares INCCResults objects
            ulong MaxBins = 0;
            foreach (run_rec r in itf.run_rec_list)
            {
                ulong x= AddToCycleList(r, det);
                if (x > MaxBins)
                    MaxBins = x;
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
            mcr.Scaler1Rate.v = results.scaler1;
            mcr.Scaler2Rate.v = results.scaler2;
            mcr.Scaler1Rate.err = results.scaler1_err;
            mcr.Scaler2Rate.err = results.scaler2_err;
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
			mcr.RawDoublesRate.v = results.uncorrected_doubles;
			mcr.RawDoublesRate.err = results.uncorrected_doubles_err;
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
            result.rates.RawRates.Scaler1s.v = results.scaler1;
            result.rates.RawRates.Scaler2s.v = results.scaler2;
            result.rates.RawRates.Scaler1s.err = results.scaler1_err;
            result.rates.RawRates.Scaler2s.err = results.scaler2_err;
            result.S1Sum = results.scaler1_sum;
            result.S2Sum = results.scaler2_sum;
            result.ASum = results.acc_sum;
            result.RASum = results.reals_plus_acc_sum;
            result.RAMult = TransferUtils.multarrayxfer(results.mult_reals_plus_acc_sum, INCC.MULTI_ARRAY_SIZE);
            result.NormedAMult = TransferUtils.multarrayxfer(results.mult_acc_sum, INCC.MULTI_ARRAY_SIZE);
            result.MaxBins = (ulong)Math.Max(result.RAMult.Length, result.NormedAMult.Length);
            result.MinBins = (ulong)Math.Min(result.RAMult.Length, result.NormedAMult.Length);
			mcr.RawDoublesRate.v = results.uncorrected_doubles;
			mcr.RawDoublesRate.err = results.uncorrected_doubles_err;
			result.singles_multi = results.singles_multi;
            result.doubles_multi = results.doubles_multi;
            result.triples_multi = results.triples_multi;


			// hack expansion of Normed mult array to same length as Acc mult array on each cycle to accomodate TheoreticalOutlier calc array length bug
            ExpandMaxBins(MaxBins, meas.Cycles, det.MultiplicityParams);
            Bloat(MaxBins, mcr);

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
            bool got = meas.INCCAnalysisResults.TryGetINCCResults(det.MultiplicityParams, out imr); // only ever this single mkey for INCC5-style transfer import, (thankfully)
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
                        newres.methodParams.cev.a = oldres.ad_a_res; newres.methodParams.cev.b = oldres.ad_b_res;
                        newres.methodParams.cev.c = oldres.ad_c_res; newres.methodParams.cev.d = oldres.ad_d_res;
                        newres.methodParams.cev.var_a = oldres.ad_var_a_res; newres.methodParams.cev.var_b = oldres.ad_var_b_res;
                        newres.methodParams.cev.var_c = oldres.ad_var_c_res; newres.methodParams.cev.var_d = oldres.ad_var_d_res;
                        newres.methodParams.cev.setcovar(Coeff.a, Coeff.b, oldres.ad_covar_ab_res);
                        newres.methodParams.cev._covar[0, 2] = oldres.ad_covar_ac_res;
                        newres.methodParams.cev._covar[0, 3] = oldres.ad_covar_ad_res;
                        newres.methodParams.cev._covar[1, 2] = oldres.ad_covar_bc_res;
                        newres.methodParams.cev._covar[1, 3] = oldres.ad_covar_bd_res;
                        newres.methodParams.cev._covar[2, 3] = oldres.ad_covar_cd_res;
                        newres.methodParams.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.ad_add_a_source_equation;
                        newres.methodParams.cev.sigma_x = oldres.ad_sigma_x_res;

                        newres.methodParams.cf.a = oldres.ad_cf_a_res; newres.methodParams.cf.b = oldres.ad_cf_b_res;
                        newres.methodParams.cf.c = oldres.ad_cf_c_res; newres.methodParams.cf.d = oldres.ad_cf_d_res;
                        newres.methodParams.dzero_avg = oldres.ad_dzero_avg_res;
                        newres.methodParams.num_runs = oldres.ad_num_runs_res;
                        newres.methodParams.tm_dbls_rate_upper_limit = oldres.ad_tm_dbls_rate_upper_limit_res;
                        newres.methodParams.tm_weighting_factor = oldres.ad_tm_weighting_factor_res;
                        newres.methodParams.use_truncated_mult = (oldres.ad_use_truncated_mult_res == 0 ? false : true);
                        newres.methodParams.dzero_ref_date = INCC.DateFrom(TransferUtils.str(oldres.ad_dzero_ref_date_res, INCC.DATE_TIME_LENGTH));
                        newres.methodParams.position_dzero = TransferUtils.Copy(oldres.ad_position_dzero_res, INCC.MAX_ADDASRC_POSITIONS);
                        // devnote: the original methodParams dcl_mass, doubles, min, max not preserved in INCC5 aas result rec
                    }
                    else if (r is results_curium_ratio_rec)
                    {
                        // dev note: untested
                        mlogger.TraceEvent(LogLevels.Verbose, 34056, ("Transferring method results for " + r.GetType().ToString()));
                        results_curium_ratio_rec oldres = (results_curium_ratio_rec)r;
                        INCCMethodResults.results_curium_ratio_rec newres = (INCCMethodResults.results_curium_ratio_rec)
                        meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.CuriumRatio, true);
                        newres.pu.pu240e_mass = new Tuple(oldres.cr_pu240e_mass, oldres.cr_pu240e_mass_err);
                        newres.pu.mass = new Tuple(oldres.cr_pu_mass, oldres.cr_pu_mass_err);
                        newres.pu.dcl_mass = oldres.cr_dcl_pu_mass;
                        newres.pu.dcl_minus_asy_mass = new Tuple(oldres.cr_dcl_minus_asy_pu_mass, oldres.cr_dcl_minus_asy_pu_mass_err);
                        newres.pu.dcl_minus_asy_mass_pct = oldres.cr_dcl_minus_asy_pu_mass_pct;
                        newres.pu.dcl_minus_asy_pu_mass = new Tuple(oldres.cr_dcl_minus_asy_pu_mass, oldres.cr_dcl_minus_asy_pu_mass_err);
                        newres.pu.dcl_minus_asy_pu_mass_pct = oldres.cr_dcl_minus_asy_pu_mass_pct;
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
                    else if (r is results_active_passive_rec) // NEXT: confusion with combined, it's the same as Active internally? expand and study
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
                    else if (r is results_active_rec)
					{
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

                        newres.methodParamsC.cev.a = oldres.col_a_res; newres.methodParamsC.cev.b = oldres.col_b_res;
                        newres.methodParamsC.cev.c = oldres.col_c_res; newres.methodParamsC.cev.d = oldres.col_d_res;
                        newres.methodParamsC.cev.var_a = oldres.col_var_a_res; newres.methodParamsC.cev.var_b = oldres.col_var_b_res;
                        newres.methodParamsC.cev.var_c = oldres.col_var_c_res; newres.methodParamsC.cev.var_d = oldres.col_var_d_res;
                        newres.methodParamsC.cev.setcovar(Coeff.a,Coeff.b,oldres.col_covar_ab_res);
                        newres.methodParamsC.cev._covar[0, 2] = oldres.col_covar_ac_res;
                        newres.methodParamsC.cev._covar[0, 3] = oldres.col_covar_ad_res;
                        newres.methodParamsC.cev._covar[1, 2] = oldres.col_covar_bc_res;
                        newres.methodParamsC.cev._covar[1, 3] = oldres.col_covar_bd_res;
                        newres.methodParamsC.cev._covar[2, 3] = oldres.col_covar_cd_res;
                        newres.methodParamsC.cev.cal_curve_equation = (INCCAnalysisParams.CurveEquation)oldres.col_collar_equation;
                        newres.methodParamsC.cev.sigma_x = oldres.col_sigma_x_res;
                        newres.methodParamsC.poison_absorption_fact[0] = oldres.col_poison_absorption_fact_res;
                        newres.methodParamsC.poison_rod_a[0] = new Tuple(oldres.col_poison_rod_a_res, oldres.col_poison_rod_a_err_res);
                        newres.methodParamsC.poison_rod_b[0] = new Tuple(oldres.col_poison_rod_b_res, oldres.col_poison_rod_b_err_res);
                        newres.methodParamsC.poison_rod_c[0] = new Tuple(oldres.col_poison_rod_c_res, oldres.col_poison_rod_c_err_res);
                        newres.methodParamsC.collar_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamsC.number_calib_rods = (int)oldres.col_number_calib_rods_res;
                        newres.methodParamsC.poison_rod_type[0] = TransferUtils.str(oldres.col_poison_rod_type_res, INCC.MAX_ROD_TYPE_LENGTH);
                        newres.methodParamsC.u_mass_corr_fact_a.v = oldres.col_u_mass_corr_fact_a_res; newres.methodParamsC.u_mass_corr_fact_a.err = oldres.col_u_mass_corr_fact_a_err_res;
                        newres.methodParamsC.u_mass_corr_fact_b.v = oldres.col_u_mass_corr_fact_b_res; newres.methodParamsC.u_mass_corr_fact_b.err = oldres.col_u_mass_corr_fact_b_err_res;
                        newres.methodParamsC.sample_corr_fact.v = oldres.col_sample_corr_fact_res; newres.methodParamsC.sample_corr_fact.err = oldres.col_sample_corr_fact_err_res;

                        newres.methodParamsDetector.collar_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamsDetector.reference_date = INCC.DateFrom(TransferUtils.str(oldres.col_reference_date_res, INCC.DATE_TIME_LENGTH));
                        newres.methodParamsDetector.relative_doubles_rate = oldres.col_relative_doubles_rate_res;

                        newres.methodParamsK5.k5_mode = (oldres.col_collar_mode == 0 ? false : true);
                        newres.methodParamsK5.k5_item_type = TransferUtils.str(oldres.col_collar_item_type, INCC.MAX_ITEM_TYPE_LENGTH);
                        newres.methodParamsK5.k5 = Copy(oldres.collar_k5_res, oldres.collar_k5_err_res, INCC.MAX_COLLAR_K5_PARAMETERS);
                        newres.methodParamsK5.k5_checkbox = TransferUtils.Copy(oldres.collar_k5_checkbox_res, INCC.MAX_COLLAR_K5_PARAMETERS);
                        for (int i = 0; i < INCC.MAX_COLLAR_K5_PARAMETERS; i++)
                        {
                            int index = i * INCC.MAX_K5_LABEL_LENGTH;
                            newres.methodParamsK5.k5_label[i] = TransferUtils.str(oldres.collar_k5_label_res + index, INCC.MAX_K5_LABEL_LENGTH);
                        }

                        CollarItemId cid = new CollarItemId(TransferUtils.str(results.item_id, INCC.MAX_ITEM_ID_LENGTH));
                        cid.length = new Tuple(newres.dcl_length);
                        cid.total_u235 = new Tuple(newres.dcl_total_u235);
                        cid.total_u238 = new Tuple(newres.dcl_total_u238);
                        cid.total_rods = newres.dcl_total_rods;
                        cid.total_poison_rods = newres.dcl_total_poison_rods;
                        cid.poison_percent = new Tuple(newres.dcl_poison_percent);
                        cid.rod_type = newres.methodParamsC.poison_rod_type[0];
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
                    else if (r is results_tm_bkg_rec)
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34062, ("Transferring method results for " + r.GetType().ToString()));
                        results_tm_bkg_rec oldres = (results_tm_bkg_rec)r;  // todo: tm bkg handling design incomplete, these values are attached to bkg measurements when the truncated flag is enabled
						INCCMethodResults.results_tm_bkg_rec newres =
							(INCCMethodResults.results_tm_bkg_rec)meas.INCCAnalysisResults.LookupMethodResults(det.MultiplicityParams, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.None, true);
						newres.methodParams.Singles.v = oldres.results_tm_singles_bkg;
						newres.methodParams.Singles.err = oldres.results_tm_singles_bkg_err;
						newres.methodParams.Zeros.v = oldres.results_tm_zeros_bkg;
						newres.methodParams.Zeros.err = oldres.results_tm_zeros_bkg_err;
						newres.methodParams.Ones.v = oldres.results_tm_ones_bkg;
						newres.methodParams.Ones.err = oldres.results_tm_ones_bkg_err;
						newres.methodParams.Twos.v = oldres.results_tm_twos_bkg;
						newres.methodParams.Twos.err = oldres.results_tm_twos_bkg_err;
					}
				    else 
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34040, ("todo: Transferring method results for " + r.GetType().ToString())); // todo: complete the list
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
            xres.original_meas_date = INCC.DateFrom(TransferUtils.str(results.original_meas_date, INCC.DATE_TIME_LENGTH));
			// NEXT: copy move passive and active meas id's here

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
                            ParamsRelatedBackToMeasurement ar = new ParamsRelatedBackToMeasurement(ir.Table);
                            long resid = ar.Create(mid, els);
                            mlogger.TraceEvent(LogLevels.Verbose, 34103, string.Format("Preserving {0} as {1}", ir.Table, resid));
                        }
                        break;
                    case AssaySelector.MeasurementOption.verification:
                    case AssaySelector.MeasurementOption.calibration:
					{
						INCCMethodResults imrs;
						bool have = meas.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
						if (have) // should be true for verification and calibration
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
										do
										{
											long resmid = ar.CreateMethod(resid, mid, _imr.methodParams.ToDBElementList());
											mlogger.TraceEvent(LogLevels.Verbose, 34101, string.Format("Preserving {0} as {1},{2}", _imr.Table, resmid, resid));
										} while (_imr.methodParams.Pump > 0);
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

        void ExpandMaxBins(ulong _MaxBins, CycleList cl, Multiplicity key)
        {
            foreach(Cycle c in cl)
            {
                MultiplicityCountingRes cmcr = (MultiplicityCountingRes)c.CountingAnalysisResults[key];
                if (_MaxBins > (ulong)cmcr.RAMult.Length || _MaxBins > (ulong)cmcr.NormedAMult.Length)
                    Bloat(_MaxBins, cmcr);
            } 
        }

        void Bloat(ulong _MaxBins, MultiplicityCountingRes amcr)
        {
            ulong[] RA = new ulong[_MaxBins];
            ulong[] NA = new ulong[_MaxBins];
            ulong[] UNA = new ulong[_MaxBins];
            amcr.MaxBins = _MaxBins;
            Array.Copy(amcr.RAMult, RA, amcr.RAMult.Length); // adds trailing 0s by leaving them untouched
            Array.Copy(amcr.NormedAMult, NA, amcr.NormedAMult.Length); // adds trailing 0s
            Array.Copy(amcr.UnAMult, UNA, amcr.UnAMult.Length); // adds trailing 0s
            amcr.RAMult = RA;
            amcr.NormedAMult = NA;
            amcr.UnAMult = UNA;
        }

        unsafe ulong AddToCycleList(run_rec run, Detector det, int cfindex = -1)  // cf index only for AAS positional cycles
        {
            Cycle cycle = new Cycle(mlogger);
            ulong MaxBins = 0;
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
					LMRawAnalysis.SDTMultiplicityCalculator.SetAlphaBeta(abkey, det.AB);               
                }
                mcr.AB.TransferIntermediates(det.AB);  // copy alpha beta onto the cycle's results 
                MaxBins = mcr.MaxBins;
            }
            catch (Exception e)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33085, "Cycle processing error {0} {1}", run.run_number, e.Message);
            }
            return MaxBins;
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

            mcr.RAMult = TransferUtils.multarrayxfer(run.run_mult_reals_plus_acc, INCC.MULTI_ARRAY_SIZE);
            mcr.NormedAMult = TransferUtils.multarrayxfer(run.run_mult_acc, INCC.MULTI_ARRAY_SIZE);
            mcr.MaxBins = (ulong)Math.Max(mcr.RAMult.Length, mcr.NormedAMult.Length);
            mcr.MinBins = (ulong)Math.Min(mcr.RAMult.Length, mcr.NormedAMult.Length);
            mcr.UnAMult = new ulong[mcr.MaxBins]; // todo: compute this
            return mcr;
        }

        #endregion measurement data transfer

    }
}
