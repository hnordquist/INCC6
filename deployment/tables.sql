PRAGMA foreign_keys=on;
CREATE TABLE detectors(
	[detector_id] INTEGER PRIMARY KEY,
	[detector_name] nvarchar(256) NOT NULL,
	[detector_type_id] int NOT NULL,
	[detector_type_freeform] nvarchar(256) NULL,
	[electronics_id] nvarchar(256) NULL,
	[detector_alias] nvarchar(256) NULL
);
GO
CREATE TABLE measurements(
	[id] INTEGER Primary Key,
	[detector_id] nvarchar(256) NOT NULL,
	[DateTime] nvarchar(40) NOT NULL,
	[Notes] nvarchar(1024) NULL,
	[Type] nvarchar(50) NOT NULL,
	[FileName] nvarchar(1024) NOT NULL
);
GO
CREATE TABLE material_types(
	[id] INTEGER Primary Key,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL
);
GO
CREATE TABLE active_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[dcl_mass] ntext,
	[doubles] ntext
);
GO
CREATE TABLE active_mult_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[vt1] float,
	[vt2] float,
	[vt3] float,
	[vf1] float,
	[vf2] float,
	[vf3] float
);
GO
CREATE TABLE active_passive_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[upper_mass_limit] float
);
GO
CREATE TABLE add_a_source_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[dcl_mass] ntext,
	[doubles] ntext
);
GO
CREATE TABLE cal_curve_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[dcl_mass] ntext,
	[doubles] ntext
);
GO
CREATE TABLE collar_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[poison_rod_type] ntext,
	[poison_absorption_fact] ntext,
	[poison_rod_a] ntext,
	[poison_rod_a_err] ntext,
	[poison_rod_b] ntext,
	[poison_rod_b_err] ntext,
	[poison_rod_c] ntext,
	[poison_rod_c_err] ntext,
	[u_mass_corr_fact_a] float,
	[u_mass_corr_fact_a_err] float,
	[u_mass_corr_fact_b] float,
	[u_mass_corr_fact_b_err] float,
	[sample_corr_fact] float,
	[sample_corr_fact_err] float
);
GO
CREATE TABLE collar_detector_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[reference_date] nvarchar(40),
	[relative_doubles_rate] float,
	[collar_detector_mode] int
);
GO
CREATE TABLE collar_k5_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[k5_mode] int,
	[k5_label] ntext,
	[k5_checkbox] ntext,
	[k5] ntext,
	[k5_err] ntext
);
GO
CREATE TABLE curium_ratio_rec(
	[id] INTEGER Primary Key,
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
	[id] INTEGER Primary Key,
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
	[dcl_mass] ntext,
	[doubles] ntext,
	FOREIGN KEY(item_type_id) REFERENCES material_types(id),
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE known_m_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[sf_rate] float,
	[vs1] float,
	[vs2] float,
	[vi1] float,
	[vi2] float,
	[b] float,
	[c] float,
	[sigma_x] float,
	[lower_mass_limit] float,
	[upper_mass_limit] float
);
GO
CREATE TABLE multiplicity_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[eff_cor] float
);
GO
CREATE TABLE truncated_mult_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[a] float,
	[b] float,
	[known_eff] int,
	[solve_eff] int
);
GO
CREATE TABLE de_mult_rec(
	[id] INTEGER Primary Key,
	[item_type_id] INTEGER REFERENCES material_types(id),
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[neutron_energy] ntext NULL,
	[detector_efficiency] ntext NULL,
	[inner_outer_ring_ratio] ntext NULL,
	[inner_ring_efficiency] float,
	[outer_ring_efficiency] float
);
GO
CREATE TABLE analysis_method_rec(
   [id] INTEGER Primary Key,
   [item_type_id] INTEGER NOT NULL,
   [analysis_method_detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
   [curium_ratio] int,
   [truncated_mult] int,
   [spare1] int,
   [spare2] int,
   [spare3] int,
   [spare4] int
);
GO
CREATE TABLE acquire_parms_rec(
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
	[meas_detector_id] nvarchar(256) NOT NULL,
   CONSTRAINT [dtd] PRIMARY KEY 
(
	[meas_detector_id],
	[item_type]
)
);
GO
CREATE TABLE add_a_source_setup_rec(
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[type] nvarchar(40),
	[port_number] int NULL,
	[forward_over_travel] float NULL,
	[reverse_over_travel] float NULL,
	[number_positions] int NULL,
	[dist_to_move] ntext,
	[cm_steps_per_inch] float NULL,
	[cm_forward_mask] int NULL,
	[cm_reverse_mask] int NULL,
	[cm_axis_number] int NULL,
	[cm_over_travel_state] int NULL,
	[cm_step_ratio] float NULL,
	[cm_slow_inches] float NULL,
	[plc_steps_per_inch] float NULL,
	[scale_conversion_factor] float NULL,
	[cm_rotation] int NULL
);
GO
CREATE TABLE alpha_beta_rec(
	[detector_id] INTEGER NOT NULL, 
	--/*[factorial] nvarchar(1024) NULL, not needed post-2010*/
	[alpha_array] ntext NULL,
	[beta_array] ntext NULL,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE
);
GO
CREATE TABLE bkg_parms_rec(
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	[active_bkg_scaler2_rate] float NULL
);
GO
CREATE TABLE composite_isotopics_rec(
	[ci_isotopics_id] nvarchar(299) NOT NULL,
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
	[ci_isotopics_source_code] nvarchar(256) NULL
);
GO
CREATE TABLE cycles(
	[id] INTEGER Primary Key,
	[mid] INTEGER REFERENCES measurements(id) on DELETE CASCADE,
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
	[chnhits] nvarchar(1024) NULL
);
GO
CREATE TABLE cycleslm(
	[id] INTEGER Primary Key,
	[cid] INTEGER REFERENCES cycles(id) on DELETE CASCADE,
	[chnhits] nvarchar(1024) NULL
);
GO
CREATE TABLE detector_types(
	[id] INTEGER Primary Key,
	[name] nvarchar(256) NOT NULL,
	[description] nvarchar(1024) NULL 
);
GO
CREATE TABLE error_codes(
	[id] INTEGER Primary Key,
	[severity] nvarchar(10) NULL,
	[encoding] nvarchar(15) NULL,
	[text] nvarchar(512) NULL
);
GO
CREATE TABLE facility_names(
	[id] INTEGER Primary Key,
	[name] nvarchar(256) NOT NULL,
	[description] nvarchar(1024) NULL 
);
GO
CREATE TABLE inventory_change_code(
	[id] INTEGER Primary Key,
	[name] nvarchar(10) NOT NULL,
	[description] nvarchar(256) NULL
);
GO
CREATE TABLE io_code(
	[id] INTEGER Primary Key,
	[name] nvarchar(10) NOT NULL,
	[description] nvarchar(256) NULL
);
GO
CREATE TABLE isotopics (
   [id] INTEGER Primary Key,
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
   [pu_date] datetime NOT NULL,
   [am_date] datetime NOT NULL,
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
	[item_id] INTEGER Primary Key,
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
	[isotopics_id] INTEGER Primary Key,
	[isotopics_source_code] nvarchar(10) NULL,
	[isotopics_name] nvarchar(256) NOT NULL,
 CONSTRAINT [isotopics_name] UNIQUE 
(
	[isotopics_name]
)
);
GO
CREATE TABLE mbas(
	[id] INTEGER Primary Key,
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
	[stratum_id] INTEGER Primary Key,
	[name] nvarchar(256) NULL,
	[description] nvarchar(1024) NULL,
	[historical_bias] float NULL,
	[historical_rand_uncert] float NULL,
	[historical_systematic_uncert] float NULL 
);
GO
CREATE TABLE stratum_id_detector(
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[stratum_id] INTEGER NOT NULL 
);
GO
CREATE TABLE stratum_id_instances(
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
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
	)
);
GO
CREATE TABLE notification_log(
	[id] INTEGER Primary Key,
	[msg] nvarchar(4000) NULL,
	[ts] datetime NOT NULL 
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
	[item_type] nvarchar(256) NULL,
	[FADefault] not NULL default 1,
	FOREIGN KEY(detector_id) REFERENCES detectors(detector_id) on DELETE CASCADE,
	CONSTRAINT [dtd] PRIMARY KEY 
(
	[detector_id],
	[item_type]
)
);
GO
CREATE TABLE LMINCCAppContext(
	[root] ntext NULL, 
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
	[emulatorapp] ntext NULL,
	[serveremulation] int NULL,
	[fileinput] ntext NULL,

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
	[resultsFilePath] ntext NULL, 
	[logFilePath] ntext NULL
);
GO
/* for Feynman, Rossi, Time, etc. */
CREATE TABLE CountingParams(
	[detector_id] INTEGER REFERENCES detectors(detector_id) on DELETE CASCADE,
	[gatewidth]float NULL,
	[counter_type] nvarchar(40) NULL,
	[active] int not NULL default 1,
	[rank] int not NULL default 0
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
	[id] INTEGER Primary Key,
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
	[counts] ntext,
	[HVPDateTime] nvarchar(40) NOT NULL,
	FOREIGN KEY([hvp_id]) REFERENCES HVResult(id)
);
GO
CREATE TABLE collar_data_entry(
	[item_id] INTEGER Primary Key,
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
CREATE INDEX  alpha_beta_recixdid on alpha_beta_rec(detector_id);
GO
CREATE INDEX LMHWParamsixdid on LMHWParams(detector_id);
GO