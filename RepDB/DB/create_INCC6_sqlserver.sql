USE [INCC6]
CREATE TABLE detectors(
	[detector_id] INTEGER IDENTITY PRIMARY KEY,
	[detector_name] nvarchar(256) NOT NULL,
	[detector_type_id] int NOT NULL,
	[detector_type_freeform] nvarchar(256) NULL,
	[electronics_id] nvarchar(256) NULL,
	[detector_alias] nvarchar(256) NULL
);
GO
CREATE TABLE measurements(
	[id] INTEGER IDENTITY Primary Key,
	[detector_id] nvarchar(256) NOT NULL,
	[DateTime] nvarchar(40) NOT NULL,
	[Notes] nvarchar(1024) NULL,
	[Type] nvarchar(50) NOT NULL,
	[FileName] nvarchar(1024) NOT NULL
);
GO
CREATE TABLE material_types(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL
);
GO
CREATE TABLE holdup_config_rec(
	[id] INTEGER IDENTITY Primary Key,
	[glovebox_id] nvarchar(256) NULL,
	[num_rows] int NULL,
	[num_columns] int NULL,
	[distance] float NULL
);
GO
CREATE TABLE active_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE active_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[vt1] float,
	[vt2] float,
	[vt3] float,
	[vf1] float,
	[vf2] float,
	[vf3] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE active_passive_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE add_a_source_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[cf_a] float,
	[cf_b] float,
	[cf_c] float,
	[cf_d] float,
	[dzero_avg] float,
	[position_dzero] nvarchar(256),
	[dzero_ref_date] nvarchar(40),   
	[num_runs] int,
	[use_truncated_mult] int,
	[tm_weighting_factor] float,
	[tm_dbls_rate_upper_limit] float, 
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE cal_curve_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_type] int,
	[cal_curve_equation] int,
	[heavy_metal_corr_factor] float,
	[heavy_metal_reference] float,
	[percent_u235] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE collar_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[Cf252] int,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[number_calib_rods] int,
	[collar_mode] int,
	[poison_rod_type] nvarchar(max),
	[poison_absorption_fact] nvarchar(max),
	[poison_rod_a] nvarchar(max),
	[poison_rod_a_err] nvarchar(max),
	[poison_rod_b] nvarchar(max),
	[poison_rod_b_err] nvarchar(max),
	[poison_rod_c] nvarchar(max),
	[poison_rod_c_err] nvarchar(max),
	[u_mass_corr_fact_a] float,
	[u_mass_corr_fact_a_err] float,
	[u_mass_corr_fact_b] float,
	[u_mass_corr_fact_b_err] float,
	[sample_corr_fact] float,
	[sample_corr_fact_err] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE collar_detector_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[reference_date] nvarchar(40),
	[relative_doubles_rate] float,
	[collar_detector_mode] int,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE collar_k5_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[k5_mode] int,
	[k5_label] nvarchar(max),
	[k5_checkbox] nvarchar(max),
	[k5] nvarchar(max),
	[k5_err] nvarchar(max),
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE curium_ratio_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[curium_ratio_type] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,	
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE  cm_pu_ratio_rec(
	[cm_pu_ratio] float,
	[cm_pu_ratio_err] float,
	[pu_half_life] float,
	[cm_pu_ratio_date] nvarchar(40),
	[cm_u_ratio] float,
	[cm_u_ratio_err] float,
	[cm_u_ratio_date] nvarchar(40),
	[cm_id_label] nvarchar(512),
	[cm_id] nvarchar(512),
	[cm_input_batch_id] nvarchar(512),
	[dcl_u_mass] float,
	[dcl_u235_mass] float
);    
GO
CREATE TABLE known_alpha_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[alpha_wt] float,
	[rho_zero] float,
	[k] float,
	[a] float,
	[b] float,
	[var_a] float,
	[var_b] float,
	[covar_ab] float,
	[sigma_x] float,
	[known_alpha_type] int,
	[heavy_metal_corr_factor] float,
	[heavy_metal_reference] float,
	[ring_ratio_equation] int,
	[ring_ratio_a] float,
	[ring_ratio_b] float,
	[ring_ratio_c] float,
	[ring_ratio_d] float,
	[lower_corr_factor_limit] float,
	[upper_corr_factor_limit] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE known_m_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[sf_rate] float,
	[vs1] float,
	[vs2] float,
	[vi1] float,
	[vi2] float,
	[b] float,
	[c] float,
	[sigma_x] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE multiplicity_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[solve_efficiency] int,
	[sf_rate] float,
	[vs1] float,
	[vs2] float,
	[vs3] float,
	[vi1] float,
	[vi2] float,
	[vi3] float,
	[a] float,
	[b] float,
	[c] float,
	[sigma_x] float,
	[alpha_weight] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[eff_cor] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE truncated_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[known_eff] int,
	[solve_eff] int,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE de_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[item_type_id] INTEGER NOT NULL,
	[detector_id] INTEGER NOT NULL,
	[neutron_energy] nvarchar(max) NULL,
	[detector_efficiency] nvarchar(max) NULL,
	[inner_outer_ring_ratio] nvarchar(max) NULL,
	[inner_ring_efficiency] float,
	[outer_ring_efficiency] float,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE analysis_method_rec(
   [id] INTEGER IDENTITY Primary Key,
   [item_type_id] INTEGER NOT NULL,
   [analysis_method_detector_id] INTEGER NOT NULL,
   [cal_curve] int,
   [known_alpha] int,
   [known_m] int,
   [multiplicity] int,
   [add_a_source] int,
   [active] int,
   [active_mult] int,
   [active_passive] int,
   [collar] int,
   [normal_method] int,
   [backup_method] int,
   [aux_method] int,
   [curium_ratio] int,
   [truncated_mult] int,
   [spare1] int,
   [spare2] int,
   [spare3] int,
   [spare4] int,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(analysis_method_detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE acquire_parms_rec(
	[id] INTEGER IDENTITY Primary Key,
	[facility] nvarchar(256) NULL,
	[facility_description] nvarchar(1024) NULL,
	[mba] nvarchar(256) NULL,
	[mba_description] nvarchar(1024) NULL,
	[detector_id] nvarchar(256) NULL,
	[item_type] nvarchar(256) NULL,
	[glovebox_id] nvarchar(256) NULL,
	[isotopics_id] nvarchar(256) NULL,
	[comp_isotopics_id] nvarchar(256) NULL,
	[campaign_id] nvarchar(256) NULL,
	[item_id] nvarchar(128) NULL,
	[stratum_id] nvarchar(256) NULL,
	[stratum_id_description] nvarchar(1024) NULL,
	[user_id] nvarchar(256) NULL,
	[comment] nvarchar(256) NULL,
	[ending_comment] nvarchar(256) NULL,
	[data_src] int NULL,
	[qc_tests] int  NULL,
	[acq_print] int  NULL,
	[review_detector_parms] int NULL,
	[review_calib_parms] int NULL,
	[review_isotopics] int  NULL,
	[review_summed_raw_data] int  NULL,
	[review_run_raw_data] int  NULL,
	[review_run_rate_data] int  NULL,
	[review_summed_mult_dist] int  NULL,
	[review_run_mult_dist] int  NULL,
	[run_count_time] float NULL,
	[acquire_type] int NULL,
	[num_runs] int NULL,
	[active_num_runs] int NULL,
	[min_num_runs] int NULL,
	[max_num_runs] int NULL,
	[meas_precision] float NULL,
	[well_config] int NULL,
	[mass] float NULL,
	[error_calc_method] int NULL,
	[inventory_change_code] nvarchar(10) NULL,
	[io_code] nvarchar(10) NULL,
	[collar_mode] int NULL,
	[drum_empty_weight] float NULL,
	[MeasDate] nvarchar(40) NOT NULL,
	[CheckDate] nvarchar(40) NOT NULL,
	[meas_detector_id] nvarchar(256) NOT NULL
);
GO
CREATE TABLE add_a_source_setup_rec(
	[detector_id] INTEGER NOT NULL,
	[type] nvarchar(40),
	[port_number] int NULL,
	[forward_over_travel] float NULL,
	[reverse_over_travel] float NULL,
	[number_positions] int NULL,
	[dist_to_move] nvarchar(max),
	[cm_steps_per_inch] float NULL,
	[cm_forward_mask] int NULL,
	[cm_reverse_mask] int NULL,
	[cm_axis_number] int NULL,
	[cm_over_travel_state] int NULL,
	[cm_step_ratio] float NULL,
	[cm_slow_inches] float NULL,
	[plc_steps_per_inch] float NULL,
	[scale_conversion_factor] float NULL,
	[cm_rotation] int NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE alpha_beta_rec(
	[detector_id] INTEGER NOT NULL, 
	--/*[factorial] nvarchar(1024) NULL, not needed post-2010*/
	[alpha_array] nvarchar(max) NULL,
	[beta_array] nvarchar(max) NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE bkg_parms_rec(
	[detector_id] INTEGER NOT NULL,
	[passive_bkg_singles_rate] float NULL,
	[passive_bkg_singles_rate_err] float NULL,
	[passive_bkg_doubles_rate] float NULL,
	[passive_bkg_doubles_rate_err] float NULL,
	[passive_bkg_triples_rate] float NULL,
	[passive_bkg_triples_rate_err] float NULL,
	[active_bkg_singles_rate] float NULL,
	[active_bkg_singles_rate_err] float NULL,
	[active_bkg_doubles_rate] float NULL,
	[active_bkg_doubles_rate_err] float NULL,
	[active_bkg_triples_rate] float NULL,
	[active_bkg_triples_rate_err] float NULL,
	[passive_bkg_scaler1_rate] float NULL,
	[passive_bkg_scaler2_rate] float NULL,
	[active_bkg_scaler1_rate] float NULL,
	[active_bkg_scaler2_rate] float NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE composite_isotopics_rec(
	[id] INTEGER IDENTITY Primary Key,
	[ci_isotopics_id] nvarchar(64) NOT NULL,
	[ci_ref_date] nvarchar(40) NULL,
	[ci_pu_mass] float NULL,
	[ci_pu238] float NULL,
	[ci_pu239] float NULL,
	[ci_pu240] float NULL,
	[ci_pu241] float NULL,
	[ci_pu242] float NULL,
	[ci_am241] float NULL,
	[ci_pu_date] nvarchar(20) NULL,
	[ci_am_date] nvarchar(20) NULL,
	[ci_isotopics_source_code] nvarchar(10) NULL
);
GO
CREATE TABLE composite_isotopic_rec(
	[id] INTEGER IDENTITY Primary Key,
	[cid] INTEGER REFERENCES composite_isotopics_rec(id) on DELETE CASCADE,
	[pu_mass] float NULL,
	[pu238] float NULL,
	[pu239] float NULL,
	[pu240] float NULL,
	[pu241] float NULL,
	[pu242] float NULL,
	[am241] float NULL,
	[pu_date] nvarchar(20) NULL,
	[am_date] nvarchar(20) NULL);
GO
CREATE TABLE cycles(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[lmid] INTEGER NULL,
	[cycle_time] float NOT NULL,
	[singles] float NULL,
	[scaler1] float NULL,
	[scaler2] float NULL,
	[reals_plus_acc] float NULL,
	[acc] float NULL,
	[mult_reals_plus_acc] nvarchar(1024) NULL,
	[mult_acc] nvarchar(1024) NULL,
	[singles_rate] float NULL,
	[doubles_rate] float NULL,
	[triples_rate] float NULL,
	[scaler1_rate] float NULL,
	[scaler2_rate] float NULL,
	[multiplicity_mult] float NULL,
	[multiplicity_alpha] float NULL,
	[multiplicity_efficiency] float NULL,
	[mass] float NULL,
	[high_voltage] float NULL,
	[status] int NULL,
	[chnhits] nvarchar(1024) NULL,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE cycleslm(
	[id] INTEGER IDENTITY Primary Key,
	[cid] INTEGER NOT NULL,
	[chnhits] nvarchar(1024) NULL,
	FOREIGN KEY(cid) REFERENCES cycles(id) on DELETE CASCADE
);
GO
CREATE TABLE detector_types(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(256) NOT NULL,
	[description] nvarchar(1024) NULL 
);
GO
CREATE TABLE error_codes(
	[id] INTEGER IDENTITY Primary Key,
	[severity] nvarchar(10) NULL,
	[encoding] nvarchar(15) NULL,
	[text] nvarchar(512) NULL
);
GO
CREATE TABLE facility_names(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(256) NOT NULL,
	[description] nvarchar(1024) NULL 
);
GO
CREATE TABLE inventory_change_code(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(10) NOT NULL,
	[description] nvarchar(256) NULL
);
GO
CREATE TABLE io_code(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(10) NOT NULL,
	[description] nvarchar(256) NULL
);
GO
CREATE TABLE isotopics (
   [id] INTEGER IDENTITY Primary Key,
   [pu238] float,
   [pu239] float,
   [pu240] float,
   [pu241] float,
   [pu242] float,
   [am241] float,
   [pu238_err] float,
   [pu239_err] float,
   [pu240_err] float,
   [pu241_err] float,
   [pu242_err] float,
   [am241_err] float,
   [pu_date] nvarchar(40) NOT NULL,
   [am_date] nvarchar(40) NOT NULL,
   [isotopics_id] nvarchar(64) NOT NULL,
   [isotopics_source_code] nvarchar(10) NOT NULL
);
GO
CREATE TABLE isotopics_source_code(
	[code] nvarchar(10) NOT NULL,
	[description] nvarchar(256) NULL
);
GO
CREATE TABLE items(
	[item_id] INTEGER IDENTITY Primary Key,
	[item_name] nvarchar(256) NULL,
	[material_type_id] nvarchar(50) NULL,
	[isotopics_id] nvarchar(256) NULL,
	[stratum_id] nvarchar(256) NULL,
	[inventory_change_code] nvarchar(10) NULL,
	[io_code] nvarchar(10) NULL,
	[declared_mass_entry] float NULL,
	[declared_u_mass_entry] float NULL,
	[length_entry] float NULL,
	[mba] nvarchar(256) NULL
);
GO
CREATE TABLE item_isotopics(
	[isotopics_id] INTEGER IDENTITY Primary Key,
	[isotopics_source_code] nvarchar(10) NULL,
	[isotopics_name] nvarchar(256) NOT NULL,
 CONSTRAINT [isotopics_name] UNIQUE 
(
	[isotopics_name]
)
);
GO
CREATE TABLE mbas(
	[id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL
);
GO
CREATE TABLE norm_parms_rec(
	[detector_id] INTEGER NOT NULL,
	[source_id] nvarchar(256) NULL,
	[normalization_constant] float NULL,
	[normalization_constant_err] float NULL,
	[bias_mode] int NULL,
	[meas_rate] float NULL,
	[meas_rate_err] float NULL,
	[amli_ref_singles_rate] float NULL,
	[cf252_ref_doubles_rate] float NULL,
	[cf252_ref_doubles_rate_err] float NULL,
	[ref_date] nvarchar(40) NULL,
	[init_src_precision_limit] float NULL,
	[bias_precision_limit] float NULL,
	[acceptance_limit_std_dev] float NULL,
	[acceptance_limit_percent] float NULL,
	[yield_relative_to_mrc_95] float NULL,
	[bias_test_use_addasrc] nvarchar(40) NULL,
	[bias_test_addasrc_position] float NULL,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE sr_parms_rec(
	[detector_id] INTEGER NOT NULL,
	[sr_detector_id] nvarchar(256) NULL,
	[sr_type] int NULL,
	[sr_port_number] int NULL,
	[predelay] float NULL,
	[gate_length] float NULL,
	[gate_length2] float NULL,
	[high_voltage] float NULL,
	[die_away_time] float NULL,
	[efficiency] float NULL,
	[multiplicity_deadtime] float NULL,
	[coeff_a_deadtime] float NULL,
	[coeff_b_deadtime] float NULL,
	[coeff_c_deadtime] float NULL,
	[doubles_gate_fraction] float NULL,
	[triples_gate_fraction] float NULL,
	[sr_baud] int NULL,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE sr_parms_ext(
	[detector_id] INTEGER NOT NULL, 
	[baud] int NULL,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE stratum_ids(
	[stratum_id] INTEGER IDENTITY Primary Key,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL,
	[historical_bias] float NULL,
	[historical_rand_uncert] float NULL,
	[historical_systematic_uncert] float NULL 
);
GO
CREATE TABLE stratum_id_detector(
	[detector_id] INTEGER NOT NULL, 
	[stratum_id] INTEGER NOT NULL,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE stratum_id_instances(
	[detector_id] INTEGER NOT NULL, 
	[stratum_id] INTEGER NOT NULL,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL,
	[historical_bias] float NULL,
	[historical_rand_uncert] float NULL,
	[historical_systematic_uncert] float NULL, 
	 CONSTRAINT [PKDS] PRIMARY KEY 
	(
		[detector_id],
		[stratum_id]
	),
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE notification_log(
	[id] INTEGER IDENTITY Primary Key,
	[msg] nvarchar(4000) NULL,
	[ts] nvarchar(40) NOT NULL 
);
GO
CREATE TABLE test_parms_rec(
	[acc_sngl_test_rate_limit] float NULL,
	[acc_sngl_test_precision_limit] float NULL,
	[acc_sngl_test_outlier_limit] float NULL,
	[outlier_test_limit] float NULL,
	[bkg_doubles_rate_limit] float NULL,
	[bkg_triples_rate_limit] float NULL,
	[chisq_limit] float NULL,
	[max_num_failures] int NULL,
	[high_voltage_test_limit] float NULL,
	[normal_backup_assay_test_limit] float NULL,
	[max_runs_for_outlier_test] int NULL,
	[checksum_test] int NULL,
	[accidentals_method] nvarchar(12) NULL
);
GO
CREATE TABLE tm_bkg_parms_rec(
	[detector_id] INTEGER NOT NULL, 
	[tm_singles_bkg] float NULL,
	[tm_singles_bkg_err] float NULL,
	[tm_zeros_bkg] float NULL,
	[tm_zeros_bkg_err] float NULL,
	[tm_ones_bkg] float NULL,
	[tm_ones_bkg_err] float NULL,
	[tm_twos_bkg] float NULL,
	[tm_twos_bkg_err] float NULL,
	[tm_bkg] int NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE 
);
GO
CREATE TABLE unattended_parms_rec(
	[detector_id] INTEGER NOT NULL, 
	[error_seconds] int NULL,
	[auto_import] int NULL,
	[add_a_source_threshold] float NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE LMNetComm(
	[detector_id] INTEGER NOT NULL, 	
	[broadcast] int NULL,
	[port] int NULL,
	[broadcastport] int NULL,
	[subnet] nvarchar(256) NULL,
	[wait] int NULL,
	
	[numConnections] int NULL,
	[receiveBufferSize] int NULL,
	[parseBufferSize] int NULL,
	[useAsyncAnalysis] int NULL,
	[streamRawAnalysis] int NULL,
	[useAsyncFileIO] int NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE LMHWParams(
	[detector_id] INTEGER NOT NULL, 
	[leds] int NULL,
	[input] int NULL,
	[debug] int NULL,
	[hv] int NULL,	
	[LLD] int NULL,
	[hvtimeout] int null,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE LMAcquireParams(
	[detector_id] INTEGER NOT NULL, 	
	[separation] int NULL,
	[interval] float NULL,
	[cycles] int NULL,
	[feedback] int NULL,
	
	[minHV] int NULL,
	[maxHV] int NULL,
	[step] int NULL,
	[hvduration] int NULL,
	[delay] int NULL,
	[hvx] int NULL,
	
	[detector] nvarchar(255) NULL,
	[saveOnTerminate] int NULL,
	[results] TEXT NULL,
	[includeConfig] int NULL,
	[message] nvarchar(1024) NULL,
	[assayType] int NULL,
	[lm] int NULL, 
	[MeasDate] nvarchar(40) NOT NULL,
	[item_type] nvarchar(256) not NULL default 'Pu',
	[FADefault] int not NULL default 1,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE,
	CONSTRAINT [dtd] PRIMARY KEY 
(
	[detector_id],
	[item_type]
)
);
GO
CREATE TABLE LMINCCAppContext(
	[root] nvarchar(max) NULL, 
	[dailyRootPath] int NULL,
	[logging] int NULL,
	[logDetails] int NULL,
	[level] int NULL,
	[rolloverIntervalMin] int NULL,
	[rolloverSizeMB] int NULL, /* (1024 * 1024), */
	[logResults] int NULL, /* 0 none, 1 log file only, 2 console/UI only, 3 everywhere */
	[fpPrec] int NULL,
	[openResults] int NULL,
	[verbose] int NULL,
	[emulatorapp] nvarchar(max) NULL,
	[serveremulation] int NULL,
	[fileinput] nvarchar(max) NULL,

	[recurse] int NULL,
	[parseGen2] int NULL,
	[replay] int NULL,
	[INCCParity] int NULL,
	[INCCXfer] int NULL,
	[sortPulseFile] int NULL,
	[pulseFileNCD] int NULL,
	[ptrFileNCD] int NULL,
	[nilaFileNCD] int NULL,
	[testDataFileAssay] int NULL,
	[reviewFileAssay] int NULL,
	[pulseFileAssay] int NULL,
	[ptrFileAssay] int NULL,
	[dbDataAssay] int NULL,
	[nilaFileAssay] int NULL,
	[opStatusPktInterval] int NULL, /* every 1Mb, or 128 8192Kb, socket packet receipts, get the status from the analyzes, */
	[opStatusTimeInterval] int NULL, /* status poll timer fires every 1000 milliseconds */
	[auxRatioReport] int NULL,
	[autoCreateMissing] int NULL,
	[overwriteDefs] int NULL,
	[gen5RevDataFile] int NULL,
	[liveFileWrite] int NULL,
	[resultsFilePath] nvarchar(max) NULL, 
	[logFilePath] nvarchar(max) NULL,
	[results8Char] int NULL,
	[assayTypeSuffix] int NULL
);
GO
/* for Feynman, Rossi, Time, etc. */
CREATE TABLE CountingParams(
	[detector_id] INTEGER NOT NULL,
	[gatewidth]float NULL,
	[counter_type] nvarchar(40) NULL,
	[active] int not NULL default 1,
	[rank] int not NULL default 0,
	FOREIGN KEY (detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
/* mult FA on, Mult Conv, Coincidence (Mult Conv) */
CREATE TABLE LMMultiplicity(
	[detector_id] INTEGER NOT NULL,
	[gatewidth] float NULL,
	[counter_type] nvarchar(40) NULL,
	[backgroundgatewidth] float NULL,
	[accidentalsgatewidth] float NULL,
	[FA] int not NULL default 1,
	[active] int not NULL default 1,
	[rank] int not NULL default 0,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE HVCalibrationParams(
	[detector_id] INTEGER NOT NULL,
	[minv] int NULL,
	[maxv] int NULL,
	[duration] int NULL,
	[stepv] int NULL,
	[delay] int not NULL default 2000,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE HVResult(
	[id] INTEGER IDENTITY Primary Key,
	[detector_id] nvarchar(256) NOT NULL,
	[HVPDateTime] nvarchar(40) NOT NULL,
	[minv] int NULL,
	[maxv] int NULL,
	[duration] int NULL,
	[stepv] int NULL,
	[delay] int NULL
);
GO
CREATE TABLE HVStatus(
	[hvp_id] int NOT NULL,
	[setpt] float NULL,
	[read] float NULL,
	[counts] nvarchar(max),
	[HVPDateTime] nvarchar(40) NOT NULL,
	FOREIGN KEY(hvp_id) REFERENCES HVResult(id)
);
GO
CREATE TABLE collar_data_entry(
	[item_id] INTEGER IDENTITY Primary Key,
	[item_name] nvarchar(256) NULL,
	[rod_type] nvarchar(50) NULL,
	[length_entry] float NULL,
	[length_entry_err] float NULL,
	[total_pu] float NULL,
	[total_pu_err] float NULL,
	[depleted_u] float NULL,
	[depleted_u_err] float NULL,
	[natural_u] float NULL,
	[natural_u_err] float NULL,
	[enriched_u] float NULL,
	[enriched_u_err] float NULL,
	[total_u235] float NULL,
	[total_u235_err] float NULL,
	[total_u238] float NULL,
	[total_u238_err] float NULL,
	[poison_percent] float NULL,
	[poison_percent_err] float NULL,
	[total_rods] float NULL,
	[total_poison_rods] float NULL
);
GO
CREATE TABLE poison_rod_type_rec(
	[id] INTEGER IDENTITY Primary Key,
	[poison_rod_type] nvarchar NULL,
	[poison_absorption_fact] float NULL
);
GO
CREATE INDEX  alpha_beta_recixdid on alpha_beta_rec(detector_id);
GO
CREATE INDEX LMHWParamsixdid on LMHWParams(detector_id);
GO
CREATE INDEX LMAcquireParamsixdid on LMAcquireParams(MeasDate);
GO
CREATE TABLE results_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[original_meas_date] nvarchar(40) NULL,

	[facility] nvarchar(256) NULL,  /* from the acquire params def */
	[facility_description] nvarchar(1024) NULL,
	[mba] nvarchar(256) NULL,
	[mba_description] nvarchar(1024) NULL,
	[item_type] nvarchar(256) NULL,
	[campaign_id] nvarchar(256) NULL,
	[item_id] nvarchar(128) NULL,
	[stratum_id] nvarchar(256) NULL,
	[stratum_id_description] nvarchar(1024) NULL,
	[collar_mode] int NULL,
	[inventory_change_code] nvarchar(10) NULL,
	[io_code] nvarchar(10) NULL,
	[well_config] int NULL,
	[data_src] int NULL,
	[qc_tests] int NULL,
	[error_calc_method] int NULL,
	[acq_print] int NULL,
	[user_id] nvarchar(256) NULL,
	[comment] nvarchar(256) NULL,
	[ending_comment] nvarchar(256) NULL,
	[num_runs] int NULL,
	
	[inspection_number] nvarchar(256) NULL,  /* todo */
	
	[detector_name] nvarchar(256)  NULL,  /* from the detector def */
	[detector_type_id] int NULL,
	[detector_type_freeform] nvarchar(256) NULL,
	[electronics_id] nvarchar(256) NULL,
	[detector_alias] nvarchar(256) NULL,

	[completed] int NULL, /* oddball */
	[meas_option] nvarchar(64) NULL, 	/* oddball */

	[num_rows] int NULL,	/* holdup_config_rec */
	[num_columns] int NULL,
	[distance] float NULL,
	[glovebox_id] nvarchar(256) NULL,	

	[bias_uncertainty] float NULL, /* from the stratum */
	[random_uncertainty] float NULL,
	[systematic_uncertainty] float NULL,
	[relative_std_dev] float NULL,

	[pu238] float NULL, /* indirectly from the item */
	[pu239] float NULL,
	[pu240] float NULL,
	[pu241] float NULL,
	[pu242] float NULL,
	[am241] float NULL,
	[pu238_err] float NULL,
	[pu239_err] float NULL,
	[pu240_err] float NULL,
	[pu241_err] float NULL,
	[pu242_err] float NULL,
	[am241_err] float NULL,
	[pu_date] nvarchar(20) NULL,
	[am_date] nvarchar(20) NULL,
	[isotopics_id] nvarchar(256) NULL,
	[isotopics_source_code] nvarchar(256) NULL,

	[length] float NULL, /* from the item */
	[declared_u_mass] float NULL,
	
	[normalization_constant] float NULL,  /* from normalization */
	[normalization_constant_err] float NULL,

	[predelay] float NULL,     /* sr_parms_rec */
	[gate_length] float NULL,
	[gate_length2] float NULL,
	[high_voltage] float NULL,
	[die_away_time] float NULL,
	[efficiency] float NULL,
	[multiplicity_deadtime] float NULL,
	[coeff_a_deadtime] float NULL,
	[coeff_b_deadtime] float NULL,
	[coeff_c_deadtime] float NULL,
	[doubles_gate_fraction] float NULL,
	[triples_gate_fraction] float NULL,

	[acc_sngl_test_rate_limit] float NULL,    /* from test params */
	[acc_sngl_test_precision_limit] float NULL,
	[acc_sngl_test_outlier_limit] float NULL,
	[outlier_test_limit] float NULL,
	[bkg_doubles_rate_limit] float NULL,
	[bkg_triples_rate_limit] float NULL,
	[chisq_limit] float NULL,
	[max_num_failures] int NULL,
	[high_voltage_test_limit] float NULL,
	[normal_backup_assay_test_limit] float NULL,  
	[max_runs_for_outlier_test] float NULL,
	[checksum_test] float NULL,
	[accidentals_method] nvarchar(12) NULL,

	[passive_bkg_singles_rate] float NULL,   /* bkg_parms_rec */
	[passive_bkg_singles_rate_err] float NULL,
	[passive_bkg_doubles_rate] float NULL,
	[passive_bkg_doubles_rate_err] float NULL,
	[passive_bkg_triples_rate] float NULL,
	[passive_bkg_triples_rate_err] float NULL,
	[active_bkg_singles_rate] float NULL,
	[active_bkg_singles_rate_err] float NULL,
	[active_bkg_doubles_rate] float NULL,
	[active_bkg_doubles_rate_err] float NULL,
	[active_bkg_triples_rate] float NULL,
	[active_bkg_triples_rate_err] float NULL,
	[passive_bkg_scaler1_rate] float NULL,
	[passive_bkg_scaler2_rate] float NULL,
	[active_bkg_scaler1_rate] float NULL,
	[active_bkg_scaler2_rate] float NULL,
	
	[singles_sum] float NULL,   /* multiplicity */
	[scaler1_sum] float NULL,
	[scaler2_sum] float NULL,
	[reals_plus_acc_sum] float NULL,
	[acc_sum] float NULL,
	[mult_reals_plus_acc_sum] nvarchar(1024) NULL,
	[mult_acc_sum] nvarchar(1024) NULL,
	[singles] float NULL,
	[singles_err] float NULL,
	[doubles] float NULL,
	[doubles_err] float NULL,
	[triples] float NULL,
	[triples_err] float NULL,
	[scaler1] float NULL,
	[scaler1_err] float NULL,
	[scaler2] float NULL,
	[scaler2_err] float NULL,
	[uncorrected_doubles] float NULL,
	[uncorrected_doubles_err] float NULL,
	[singles_multi] float NULL,
	[doubles_multi] float NULL,
	[triples_multi] float NULL,
	[declared_mass] float NULL,
	[covariance_matrix] nvarchar(256) NULL,
	
	[primary_analysis_method] nvarchar(40) NULL, /* oddballs */
	[total_number_runs] int NULL,
	[total_good_count_time] float NULL,	
	[net_drum_weight] float NULL,
	[db_version] float NULL,
		
	[passive_DateTime] nvarchar(40) NULL,  /* to do from here */
	[passive_filename] nvarchar(1024) NULL,
	[passive_detector_id] nvarchar(256) NULL,
	[active_DateTime] nvarchar(40) NULL,
	[active_filename] nvarchar(1024) NULL,
	[active_detector_id] nvarchar(256) NULL,
	/* todo: list mode params analogized to sr parms */
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE LMMultiplicity_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[gatewidth] float NULL,
	[counter_type] nvarchar(40) NULL,
	[backgroundgatewidth] float NULL,
	[accidentalsgatewidth] float NULL,
	[FA] int not NULL default 1,
	[active] int not NULL default 1,
	[rank] int not NULL default 0,
	[predelay] float NULL,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE);
GO
CREATE TABLE CountingParams_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[gatewidth]float NULL,
	[counter_type] nvarchar(40) NULL,
	[active] int not NULL default 1,
	[rank] int not NULL default 0,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE results_active_passive_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[delta_doubles] float,
	[delta_doubles_err] float,
	[u235_mass] float,
	[u235_mass_err] float,
	[k0] float,
	[k1] float,
	[k1_err] float,
	[k] float,
	[k_err] float,
	[dcl_u235_mass] float,
	[dcl_minus_asy_u235_mass] float,
	[dcl_minus_asy_u235_mass_err] float,
	[dcl_minus_asy_u235_mass_pct] float,
	[pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE active_passive_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_active_passive_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_add_a_source_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[dzero_cf252_doubles] float,
	[sample_cf252_doubles] nvarchar(512),
	[sample_cf252_doubles_err] nvarchar(512),
	[sample_cf252_ratio] nvarchar(512),
	[sample_avg_cf252_doubles] float,
	[sample_avg_cf252_doubles_err] float,
	[corr_doubles] float,
	[corr_doubles_err] float,
	[delta] float,
	[delta_err] float,
	[corr_factor] float,
	[corr_factor_err] float,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[dcl_pu240e_mass] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[pass] int,
	[tm_doubles_bkg] float,
	[tm_doubles_bkg_err] float,
	[tm_uncorr_doubles] float,
	[tm_uncorr_doubles_err] float,
	[tm_corr_doubles] float,
	[tm_corr_doubles_err] float,
	[add_a_source_equation] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE add_a_source_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[cf_a] float,
	[cf_b] float,
	[cf_c] float,
	[cf_d] float,
	[dzero_avg] float,
	[position_dzero] nvarchar(256),
	[dzero_ref_date] nvarchar(40),   
	[num_runs] int,
	[use_truncated_mult] int,
	[tm_weighting_factor] float,
	[tm_dbls_rate_upper_limit] float, 
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_add_a_source_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_cal_curve_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[dcl_pu240e_mass] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[pass] int,
	[dcl_u_mass] float,
	[length] float,
	[heavy_metal_content] float,
	[heavy_metal_correction] float,
	[heavy_metal_corr_singles] float,
	[heavy_metal_corr_singles_err] float,
	[heavy_metal_corr_doubles] float,
	[heavy_metal_corr_doubles_err] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE cal_curve_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_type] int,
	[cal_curve_equation] int,
	[heavy_metal_corr_factor] float,
	[heavy_metal_reference] float,
	[percent_u235] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_cal_curve_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_curium_ratio_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[cm_mass] float,
	[cm_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[u_mass] float,
	[u_mass_err] float,
	[u235_mass] float,
	[u235_mass_err] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[dcl_minus_asy_u_mass] float,
	[dcl_minus_asy_u_mass_err] float,
	[dcl_minus_asy_u_mass_pct] float,
	[dcl_minus_asy_u235_mass] float,
	[dcl_minus_asy_u235_mass_err] float,
	[dcl_minus_asy_u235_mass_pct] float,
	[pu_pass] int,
	[u_pass] int,
	[cm_pu_ratio_decay_corr] float,
	[cm_pu_ratio_decay_corr_err] float,
	[cm_u_ratio_decay_corr] float,
	[cm_u_ratio_decay_corr_err] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE cm_pu_ratio_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[cm_pu_ratio] float,
	[cm_pu_ratio_err] float,
	[pu_half_life] float,
	[cm_pu_ratio_date] nvarchar(40),
	[cm_u_ratio] float,
	[cm_u_ratio_err] float,
	[cm_u_ratio_date] nvarchar(40),
	[cm_id_label] nvarchar(512),
	[cm_id] nvarchar(512),
	[cm_input_batch_id] nvarchar(512),
	[dcl_u_mass] float,
	[dcl_u235_mass] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_curium_ratio_rec(id) on DELETE NO ACTION
);
GO  
CREATE TABLE curium_ratio_rec_m(
	[id] INTEGER Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[curium_ratio_type] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_curium_ratio_rec(id) on DELETE NO ACTION
);
GO    
CREATE TABLE results_known_alpha_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[mult] float,
	[alpha] float,
	[mult_corr_doubles] float,
	[mult_corr_doubles_err] float,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[dcl_pu240e_mass] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[pass] int,
	[dcl_u_mass] float,
	[length] float,
	[heavy_metal_content] float,
	[heavy_metal_correction] float,
	[corr_singles] float,
	[corr_singles_err] float,
	[corr_doubles] float,
	[corr_doubles_err] float,
	[corr_factor] float,
	[dry_alpha_or_mult_dbls] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE known_alpha_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[alpha_wt] float,
	[rho_zero] float,
	[k] float,
	[a] float,
	[b] float,
	[var_a] float,
	[var_b] float,
	[covar_ab] float,
	[sigma_x] float,
	[known_alpha_type] int,
	[heavy_metal_corr_factor] float,
	[heavy_metal_reference] float,
	[ring_ratio_equation] int,
	[ring_ratio_a] float,
	[ring_ratio_b] float,
	[ring_ratio_c] float,
	[ring_ratio_d] float,
	[lower_corr_factor_limit] float,
	[upper_corr_factor_limit] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_known_alpha_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_known_m_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[mult] float,
	[mult_err] float,
	[alpha] float,
	[alpha_err] float,
	[pu239e_mass] float,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[dcl_pu240e_mass] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE known_m_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[sf_rate] float,
	[vs1] float,
	[vs2] float,
	[vi1] float,
	[vi2] float,
	[b] float,
	[c] float,
	[sigma_x] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_known_m_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_multiplicity_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[mult] float,
	[mult_err] float,
	[alpha] float,
	[alpha_err] float,
	[corr_factor] float,
	[corr_factor_err] float,
	[efficiency] float,
	[efficiency_err] float,
	[pu240e_mass] float,
	[pu240e_mass_err] float,
	[pu_mass] float,
	[pu_mass_err] float,
	[dcl_pu240e_mass] float,
	[dcl_pu_mass] float,
	[dcl_minus_asy_pu_mass] float,
	[dcl_minus_asy_pu_mass_err] float,
	[dcl_minus_asy_pu_mass_pct] float,
	[pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE multiplicity_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[solve_efficiency] int,
	[sf_rate] float,
	[vs1] float,
	[vs2] float,
	[vs3] float,
	[vi1] float,
	[vi2] float,
	[vi3] float,
	[a] float,
	[b] float,
	[c] float,
	[sigma_x] float,
	[alpha_weight] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[eff_cor] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_multiplicity_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_truncated_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[bkg_singles] float,
	[bkg_singles_err] float,
	[bkg_zeroes] float,
	[bkg_zeroes_err] float,
	[bkg_ones] float,
	[bkg_ones_err] float,
	[bkg_twos] float,
	[bkg_twos_err] float,
	[net_singles] float,
	[net_singles_err] float,
	[net_zeroes] float,
	[net_zeroes_err] float,
	[net_ones] float,
	[net_ones_err] float,
	[net_twos] float,
	[net_twos_err] float,
	[k_alpha] float,
	[k_alpha_err] float,	
	[k_pu240e_mass] float,
	[k_pu240e_mass_err] float,
	[k_pu_mass] float,
	[k_pu_mass_err] float,
	[k_dcl_pu240e_mass] float,
	[k_dcl_pu_mass] float,
	[k_dcl_minus_asy_pu_mass] float,
	[k_dcl_minus_asy_pu_mass_err] float,
	[k_dcl_minus_asy_pu_mass_pct] float,
	[k_pass] int,
	[s_eff] float,
	[s_eff_err] float,	
	[s_alpha] float,
	[s_alpha_err] float,	
	[s_pu240e_mass] float,
	[s_pu240e_mass_err] float,
	[s_pu_mass] float,
	[s_pu_mass_err] float,
	[s_dcl_pu240e_mass] float,
	[s_dcl_pu_mass] float,
	[s_dcl_minus_asy_pu_mass] float,
	[s_dcl_minus_asy_pu_mass_err] float,
	[s_dcl_minus_asy_pu_mass_pct] float,
	[s_pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE truncated_mult_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[known_eff] int,
	[solve_eff] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_truncated_mult_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_active_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[k0] float,
	[k1] float,
	[k1_err] float,
	[k_err] float,
	[k] float,
	[u235_mass] float,
	[u235_mass_err] float,
	[dcl_u235_mass] float,
	[dcl_minus_asy_u235_mass] float,
	[dcl_minus_asy_u235_mass_err] float,
	[dcl_minus_asy_u235_mass_pct] float,
	[pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
 );
GO
CREATE TABLE active_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[dcl_mass] nvarchar(max),
	[doubles] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_active_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_active_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
    [mult] float,
    [mult_err] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE active_mult_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[vt1] float,
	[vt2] float,
	[vt3] float,
	[vf1] float,
	[vf2] float,
	[vf3] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_active_mult_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_collar_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[u235_mass] float,
	[u235_mass_err] float,
	[percent_u235] float,
	[total_u_mass] float,
	[k0] float,
	[k0_err] float,
	[k1] float,
	[k1_err] float,
	[k2] float,
	[k2_err] float,
	[k3] float,
	[k3_err] float,
	[k4] float,
	[k4_err] float,
	[k5] float,
	[k5_err] float,
	[total_corr_fact] float,
	[total_corr_fact_err] float,
	[source_id] nvarchar(128),
	[corr_doubles] float,
	[corr_doubles_err] float,
	[dcl_length] float,
	[dcl_length_err] float,
	[dcl_total_u235] float,
	[dcl_total_u235_err] float,
	[dcl_total_u238] float,
	[dcl_total_u238_err] float,
	[dcl_total_rods] float,
	[dcl_total_poison_rods] float,
	[dcl_poison_percent] float,
	[dcl_poison_percent_err] float,
	[dcl_minus_asy_u235_mass] float,
	[dcl_minus_asy_u235_mass_err] float,
	[dcl_minus_asy_u235_mass_pct] float,
	[pass] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE collar_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[a] float,
	[b] float,
	[c] float,
	[d] float,
	[var_a] float,
	[var_b] float,
	[var_c] float,
	[var_d] float,
	[covar_ab] float,
	[covar_ac] float,
	[covar_ad] float,
	[covar_bc] float,
	[covar_bd] float,
	[covar_cd] float,
	[sigma_x] float,
	[cal_curve_equation] int,
	[lower_mass_limit] float,
	[upper_mass_limit] float,
	[number_calib_rods] int,
	[collar_mode] int,
	[poison_rod_type] nvarchar(max),
	[poison_absorption_fact] nvarchar(max),
	[poison_rod_a] nvarchar(max),
	[poison_rod_a_err] nvarchar(max),
	[poison_rod_b] nvarchar(max),
	[poison_rod_b_err] nvarchar(max),
	[poison_rod_c] nvarchar(max),
	[poison_rod_c_err] nvarchar(max),
	[u_mass_corr_fact_a] float,
	[u_mass_corr_fact_a_err] float,
	[u_mass_corr_fact_b] float,
	[u_mass_corr_fact_b_err] float,
	[sample_corr_fact] float,
	[sample_corr_fact_err] float, 
	/* collar_detector_rec */
	[reference_date] nvarchar(40),
	[relative_doubles_rate] float,
	[collar_detector_mode] int,
	/* collar_k5_rec */
	[k5_mode] int,
	[k5_label] nvarchar(max),
	[k5_checkbox] nvarchar(max),
	[k5] nvarchar(max),
	[k5_err] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_collar_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE collar_detector_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[reference_date] nvarchar(40),
	[relative_doubles_rate] float,
	[collar_detector_mode] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_collar_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE collar_k5_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[k5_mode] int,
	[k5_label] nvarchar(max),
	[k5_checkbox] nvarchar(max),
	[k5] nvarchar(max),
	[k5_err] nvarchar(max),
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_collar_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_de_mult_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
    [de_energy_corr_factor] float,
	[interpolated_neutron_energy] float,
	[energy_corr_factor] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE de_mult_rec_m(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[rid] INTEGER NOT NULL,
	[neutron_energy] nvarchar(max) NULL,
	[detector_efficiency] nvarchar(max) NULL,
	[inner_outer_ring_ratio] nvarchar(max) NULL,
	[inner_ring_efficiency] float,
	[outer_ring_efficiency] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE,
	FOREIGN KEY(rid) REFERENCES results_de_mult_rec(id) on DELETE NO ACTION
);
GO
CREATE TABLE results_tm_bkg_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[tm_singles_bkg] float NULL,
	[tm_singles_bkg_err] float NULL,
	[tm_zeros_bkg] float NULL,
	[tm_zeros_bkg_err] float NULL,
	[tm_ones_bkg] float NULL,
	[tm_ones_bkg_err] float NULL,
	[tm_twos_bkg] float NULL,
	[tm_twos_bkg_err] float NULL,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
/* todo: URGENT tm_bkg_parms_rec */

CREATE TABLE results_init_src_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[init_src_id] nvarchar(512),
	[pass] int,
	[mode] int,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE results_bias_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[source_id] nvarchar(512),
	[pass] int,
	[mode] int,
	[sngls_rate_expect] float,
	[sngls_rate_expect_err] float,
	[sngls_rate_expect_meas] float,
	[sngls_rate_expect_meas_err] float,
	[dbls_rate_expect] float,
	[dbls_rate_expect_err] float,
	[dbls_rate_expect_meas] float,
	[dbls_rate_expect_meas_err] float,
	[new_norm_constant] float,
	[new_norm_constant_err] float,
	[meas_precision] float,
	[required_precision] float,
	[required_meas_seconds] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE results_precision_rec(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[pass] int,
	[sample_var] float,
	[theoretical_var] float,
	[chi_sq] float,
	[chi_sq_lower_limit] float,
	[chi_sq_upper_limit] float,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE analysis_messages(
	[mid] INTEGER NOT NULL,
	[msgid] int NOT NULL,
	[level] int NOT NULL,
	[text] nvarchar(1024),
	[ts] nvarchar(40) NOT NULL ,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
CREATE TABLE results_filenames(
	[id] INTEGER IDENTITY Primary Key,
	[mid] INTEGER NOT NULL,
	[FileName] nvarchar(1024) NULL,
	FOREIGN KEY(mid) REFERENCES measurements(id) on DELETE CASCADE
);
GO
INSERT INTO [test_parms_rec] VALUES(999.0,0.1,4.0,3.0,1.0,1.0,99.0,10,1.0,3.0,100,1,'Measure');
GO
INSERT INTO [facility_names] VALUES('XXXX','XXXX');
GO
INSERT INTO [material_types] VALUES('Pu','Pu');
GO
INSERT INTO [material_types] VALUES('PUOX','PUOX');
GO
INSERT INTO [mbas] VALUES('XXXX','XXXX');
GO
INSERT INTO [mbas] VALUES('matBalArea','descriptionOfMBA');
GO
INSERT INTO [stratum_ids]  (name, [description], historical_bias,historical_rand_uncert,historical_systematic_uncert) VALUES('XXXX','XXXX',0.0,0.0,0.0);
GO
INSERT INTO [detectors] (detector_name,detector_type_id,detector_type_freeform,electronics_id,detector_alias) VALUES('XXXX/XXX/YY',10,'AMSR','XXXX/XXX','Default');
GO
INSERT INTO [detectors] (detector_name,detector_type_id,detector_type_freeform,electronics_id,detector_alias) VALUES('LM09',14,'LMMM','68Y-156220','');
GO
INSERT INTO [sr_parms_rec] VALUES(1,'XXXX/XXX/YY',10,1,45.0,640.0,0.0,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001,NULL);
GO
INSERT INTO [alpha_beta_rec] VALUES(1, '0','0');
GO
INSERT INTO [bkg_parms_rec]
(detector_id,passive_bkg_singles_rate,[passive_bkg_singles_rate_err],[passive_bkg_doubles_rate],[passive_bkg_doubles_rate_err],[passive_bkg_triples_rate],[passive_bkg_triples_rate_err],
[active_bkg_singles_rate],[active_bkg_singles_rate_err],[passive_bkg_scaler1_rate],[passive_bkg_scaler2_rate],[active_bkg_scaler1_rate],[active_bkg_scaler2_rate]
) VALUES(1, 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO [norm_parms_rec]
(detector_id, [source_id],[normalization_constant],[normalization_constant_err],[bias_mode],[meas_rate],[meas_rate_err],[amli_ref_singles_rate],
 [cf252_ref_doubles_rate],[cf252_ref_doubles_rate_err],[ref_date],[init_src_precision_limit],[bias_precision_limit],[acceptance_limit_std_dev],
 [acceptance_limit_percent],[yield_relative_to_mrc_95],[bias_test_use_addasrc],[bias_test_addasrc_position])
VALUES(1,'',1.0,0.0,0,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO [add_a_source_setup_rec]
	([detector_id],[type],[port_number],[forward_over_travel],[reverse_over_travel],[number_positions],[dist_to_move],
	[cm_steps_per_inch],[cm_forward_mask],[cm_reverse_mask],[cm_axis_number],[cm_over_travel_state],
	[cm_step_ratio],[cm_slow_inches],[plc_steps_per_inch],[scale_conversion_factor],[cm_rotation])
	VALUES(1,'PSC_WDAS',1,0,0,1,'0,0,0,0,0', 625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO [stratum_id_detector] VALUES(1,1);
GO
INSERT INTO [unattended_parms_rec] ([detector_id],[error_seconds],[auto_import],[add_a_source_threshold]) VALUES(1,600,'',0.0);
GO
INSERT INTO [HVCalibrationParams] ([detector_id],[minv],[maxv],[duration],[stepv],[delay]) VALUES(1,0,2000,10,5,2000);
GO
INSERT INTO [analysis_method_rec] (item_type_id,[analysis_method_detector_id],[cal_curve],[known_alpha],
[known_m],[multiplicity],[add_a_source],[active],[active_mult],
[active_passive],[collar],[normal_method],[backup_method],[curium_ratio],[truncated_mult]) VALUES(1,1,1,0,0,0,0,0,0,0,0,0,0,0,0);
GO
INSERT INTO [sr_parms_rec]	(detector_id,[sr_detector_id],[sr_type],[sr_port_number],[predelay],[gate_length],[gate_length2],
[high_voltage],[die_away_time],[efficiency], [multiplicity_deadtime],[coeff_a_deadtime],[coeff_b_deadtime],
[coeff_c_deadtime],[doubles_gate_fraction],[triples_gate_fraction],[sr_baud]) 
VALUES(2,'LM09',14,1,45.0,640.0,0.0,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001, NULL);
GO
INSERT INTO [alpha_beta_rec] VALUES(2,'0','0');
GO
INSERT INTO [bkg_parms_rec]
(detector_id,passive_bkg_singles_rate,[passive_bkg_singles_rate_err],[passive_bkg_doubles_rate],[passive_bkg_doubles_rate_err],[passive_bkg_triples_rate],[passive_bkg_triples_rate_err],
[active_bkg_singles_rate],[active_bkg_singles_rate_err],[active_bkg_doubles_rate],[active_bkg_doubles_rate_err],[active_bkg_triples_rate],[active_bkg_triples_rate_err],[passive_bkg_scaler1_rate],[passive_bkg_scaler2_rate],[active_bkg_scaler1_rate],[active_bkg_scaler2_rate]
) VALUES(2, 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO [norm_parms_rec]
(detector_id, [source_id],[normalization_constant],[normalization_constant_err],[bias_mode],[meas_rate],[meas_rate_err],[amli_ref_singles_rate],
 [cf252_ref_doubles_rate],[cf252_ref_doubles_rate_err],[ref_date],[init_src_precision_limit],[bias_precision_limit],[acceptance_limit_std_dev],
 [acceptance_limit_percent],[yield_relative_to_mrc_95],[bias_test_use_addasrc],[bias_test_addasrc_position])
VALUES(2,'',1.0,0.0,0,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO [add_a_source_setup_rec]
	([detector_id],[type],[port_number],[forward_over_travel],[reverse_over_travel],[number_positions],[dist_to_move],
	[cm_steps_per_inch],[cm_forward_mask],[cm_reverse_mask],[cm_axis_number],[cm_over_travel_state],
	[cm_step_ratio],[cm_slow_inches],[plc_steps_per_inch],[scale_conversion_factor],[cm_rotation])
VALUES(2,'None',1,0,0,1,'0,0,0,0,0', 625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO [stratum_id_detector] VALUES(2,1);
GO
INSERT INTO [unattended_parms_rec] ([detector_id],[error_seconds],[auto_import],[add_a_source_threshold]) VALUES(2,600,'',0.0);
GO
INSERT INTO [HVCalibrationParams] ([detector_id],[minv],[maxv],[duration],[stepv],[delay]) VALUES(2,0,2000,10,5,2000);
GO
INSERT INTO [analysis_method_rec] 
(item_type_id,[analysis_method_detector_id],[cal_curve],[known_alpha],[known_m],[multiplicity],[add_a_source],[active],
[active_mult], [active_passive],[collar],[normal_method],[backup_method],[curium_ratio],[truncated_mult]) 
VALUES(1,2,1,0,0,0,0,0,0,0,0,0,0,0,0);
GO
INSERT INTO [isotopics] 
([pu238],[pu239],[pu240],[pu241],[pu242],[am241],[pu238_err],[pu239_err],[pu240_err],[pu241_err],[pu242_err],[am241_err],
[pu_date],[am_date],[isotopics_id],[isotopics_source_code])
VALUES(0.0,0.0,100.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,'2010-01-01','2010-01-01','Default','OD');
GO
INSERT INTO [composite_isotopics_rec] 
([ci_pu238],[ci_pu239],[ci_pu240],[ci_pu241],[ci_pu242],[ci_am241],
 [ci_pu_date],[ci_am_date],[ci_isotopics_id],[ci_isotopics_source_code],[ci_pu_mass],[ci_ref_date])
VALUES(0.0,0.0,100.0,0.0,0.0,0.0,'2010-01-01','2010-01-01','Default','OD',1.0,'2016-01-01');
GO
INSERT INTO [composite_isotopic_rec] 
([cid],[pu238],[pu239],[pu240],[pu241],[pu242],[am241],[pu_date],[am_date],[pu_mass])
VALUES(1,0.0,0.0,100.0,0.0,0.0,0.0,'2010-01-01','2010-01-01',0.0);
GO
INSERT INTO [item_isotopics] VALUES('OD','Default');
GO
INSERT INTO [detector_types] VALUES('JSR11','JSR11');
GO
INSERT INTO [detector_types] VALUES('SR4','SR4');
GO
INSERT INTO [detector_types] VALUES('JSR12','JSR12');
GO
INSERT INTO [detector_types] VALUES('HNC','HNC');
GO
INSERT INTO [detector_types] VALUES('JSR11A','JSR11A');
GO
INSERT INTO [detector_types] VALUES('MSR4','MSR4 or 2150');
GO
INSERT INTO [detector_types] VALUES('MSR4A','MSR4A');
GO
INSERT INTO [detector_types] VALUES('PSR','PSR4 or ISR');
GO
INSERT INTO [detector_types] VALUES('DGSR','DGSR');
GO
INSERT INTO [detector_types] VALUES('AMSR','Advanced Multiplicty Shift Register');
GO
INSERT INTO [detector_types] VALUES('JSR15','JSR15');
GO
INSERT INTO [detector_types] VALUES('UNAP','UNAP Unrequited');
GO
INSERT INTO [detector_types] VALUES('NPOD','NPOD Unrequited');
GO
INSERT INTO [detector_types] VALUES('LMMM','LMMM');
GO
INSERT INTO [detector_types] VALUES('PTR32','Magyar Tudományos Akadémia Energiatudományi Kutatóközpont');
GO
INSERT INTO [detector_types] VALUES('MCA527','GBS Elektronik GmbH');
GO
INSERT INTO [detector_types] VALUES('N1','N1');
GO
INSERT INTO [detector_types] VALUES('MCNPX','MCNPX');
GO
INSERT INTO [inventory_change_code] VALUES('RD','Received during interim (domestic)');
GO
INSERT INTO [inventory_change_code] VALUES('RF','Received during interim (foreign)');
GO
INSERT INTO [inventory_change_code] VALUES('RN','Received during interim');
GO
INSERT INTO [inventory_change_code] VALUES('SD','Shipped/measured during interim inspection (domestic)');
GO
INSERT INTO [inventory_change_code] VALUES('SF','Shipped/measured during interim inspection (foreign)');
GO
INSERT INTO [inventory_change_code] VALUES('SN','Shipped/measured during interim inspection');
GO
INSERT INTO [io_code] VALUES('A','Received/measured during PIV and before PIV date');
GO
INSERT INTO [io_code] VALUES('B','Shipped/measured during PIV and before PIV date');
GO
INSERT INTO [io_code] VALUES('D','Shipped/measured during PIV and after PIV date');
GO
INSERT INTO [io_code] VALUES('C','Received/measured during PIV and after PIV date');
GO
INSERT INTO [io_code] VALUES('F','Flow measurement');
GO
INSERT INTO [io_code] VALUES('I','Received during interim');
GO
INSERT INTO [io_code] VALUES('0','Shipped/measured during interim');
GO
INSERT INTO [io_code] VALUES('T','Test measurement');
GO
INSERT INTO [io_code] VALUES('X','IIV');
GO
INSERT INTO [io_code] VALUES('Z','Other');
GO
INSERT INTO [LMNetComm] VALUES(2,1,5002,5201,'127.0.0.1',500,8,8192,50,1,1,1);
Go
INSERT INTO [LMHWParams] VALUES(2,1,0,0,1705,600,30);
Go
INSERT INTO [LMINCCAppContext] VALUES('c:\temp',1,1,0,4,30,50,0,3,1,2,'./',0,'./',0,0,0,1,NULL,0,0,0,0,0,0,0,0,0,0,128,1000,0,0,1,0,0,'','',1,1);
GO
INSERT INTO [detectors] VALUES('AS710',15,'PTR-32HV','N026','AS710-PTR32');
GO
INSERT INTO [detectors] VALUES('MCA1153',16,'MCA527','01153','1 chn');
GO
INSERT INTO [detectors] VALUES ('JSR-15',11,'JSR15','','JSR-15-JSR15');
GO
INSERT INTO [add_a_source_setup_rec] VALUES(3,'None',1,0.0,0.0,1,'0,0,0,0,0',625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO [add_a_source_setup_rec] VALUES(4,'None',1,0.0,0.0,1,'0,0,0,0,0',625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO [add_a_source_setup_rec] VALUES(5,'None',1,0.0,0.0,1,'0,0,0,0,0',625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO [alpha_beta_rec] VALUES(3,'','');
GO
INSERT INTO [alpha_beta_rec] VALUES(4,'','');
GO
INSERT INTO [alpha_beta_rec] VALUES(5,'','');
GO
INSERT INTO [bkg_parms_rec] VALUES(3,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO [bkg_parms_rec] VALUES(4,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO [bkg_parms_rec] VALUES(5,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO [norm_parms_rec] VALUES(3,'',1.0,0.0,1,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO [norm_parms_rec] VALUES(4,'',1.0,0.0,1,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO [norm_parms_rec] VALUES(5,'',1.0,0.0,1,0.0,0.0,200.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO [sr_parms_rec] VALUES(3,'AS710',15,NULL,45.0,640.0,NULL,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001,NULL);
GO
INSERT INTO [sr_parms_rec] VALUES(4,'MCA1163',16,NULL,15,240.0,NULL,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001,NULL);
GO
INSERT INTO [sr_parms_rec] VALUES(5,'JSR-15',11,3,45,640,NULL,1680,500,0.0001,0,0,0,0,0.0001,0.0001,9600);
GO
INSERT INTO [unattended_parms_rec] VALUES(3,600,1,0.0);
GO
INSERT INTO [unattended_parms_rec] VALUES(4,600,1,0.0);
GO
INSERT INTO [unattended_parms_rec] VALUES(5,600,1,0.0);
GO
INSERT INTO [HVCalibrationParams] VALUES(3,5,2000,10,100,2000);
GO
INSERT INTO [HVCalibrationParams] VALUES(4,5,2000,10,100,2000);
GO
INSERT INTO [HVCalibrationParams] VALUES(5,5,2000,10,100,2000);
GO
INSERT INTO [stratum_id_detector] VALUES(3,1);
GO
INSERT INTO [stratum_id_detector] VALUES(4,1);
GO
INSERT INTO [stratum_id_detector] VALUES(5,1);
GO
INSERT INTO [LMNetComm] VALUES(3,1,5011,5000,'169.254.255.255',500,8,8192,50,0,1,0);
GO
INSERT INTO [LMHWParams] VALUES(3,1,1,0,1650,2,30);
GO
INSERT INTO [LMNetComm] VALUES(4,1,5011,5000,'169.254.255.255',500,8,8192,50,0,1,0);
GO
INSERT INTO [LMHWParams] VALUES(4,1,1,0,1650,2,30);
GO
INSERT INTO [known_alpha_rec] VALUES(2,1,0.0,0.0,2.166,0.0,1.0,0.0,0.0,0.0,0.0,0,0.0,0.0,0,0.0,0.0,0.0,0.0,0.0,100000000.0,NULL,NULL,'0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0','0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0');
GO
INSERT INTO [multiplicity_rec] VALUES(2,1,0,473.5,2.154,3.789,5.211,3.163,8.24,17.321,1.0,0.0,0.0,0.0,1.0,NULL,NULL,1.0);
GO
INSERT INTO [analysis_method_rec] VALUES(2,1,1,1,0,1,0,0,0,0,0,4,2,0,0,0,NULL,NULL,NULL,NULL);
GO
INSERT INTO [cal_curve_rec] VALUES(2,1,0.0,1.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0,0,0.0,0.0,0.0,-100000000.0,100000000.0,'0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0','0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0');
GO
INSERT INTO [acquire_parms_rec] VALUES('XXXX','XXXX','XXXX','XXXX','AS710','Pu','','Default','Default','','','','','','','0',0,1,0,1,1,1,0,0,1,0,0,100.0,0,3,3,10,1000,1.0,0,0.0,2,'','',0,0.0,'2014-11-25T13:18:10','2014-11-25T13:18:10', 'AS710');
GO
INSERT INTO [analysis_method_rec] VALUES (1,5,1,0,0,0,0,0,0,0,0,1,0,1,0,0,NULL,NULL,NULL,NULL);
GO
INSERT INTO [analysis_method_rec] VALUES (2,5,0,0,0,0,0,0,0,0,1,9,0,0,0,0,NULL,NULL,NULL,NULL);
GO
INSERT INTO [acquire_parms_rec] VALUES('XXXX','XXXX','XXXX','XXXX','JSR-15','Pu','','Default','Default','','','XXXX','XXXX','','','0',0,1,0,1,1,1,0,0,1,0,0,100.0,0,3,3,10,1000,1,0,40,2,'','',0,0,'2016-08-05T11:05:39.2729192-06:00','2016-08-05T16:28:15.4647845-06:00','JSR-15');
GO
INSERT INTO [LMAcquireParams] VALUES(3,0,12,6,0,0,0,0,0,0,0,NULL,0,'c:\temp',0,'',0,0,'2014-11-25T15:23:08','Pu',1);
GO
INSERT INTO [LMAcquireParams] VALUES(4,0,12,6,0,0,0,0,0,0,0,NULL,0,'c:\temp',0,'',0,0,'2016-01-13T13:52:17','Pu',1);
GO
INSERT INTO [poison_rod_type_rec] VALUES('G',0.647);
GO
