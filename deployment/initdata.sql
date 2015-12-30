INSERT INTO "test_parms_rec" VALUES(999.0,0.1,4.0,3.0,1.0,1.0,99.0,10,1.0,3.0,100,1,'Measure');
GO
INSERT INTO "facility_names" (name, [description]) VALUES('Empty','Empty');
GO
INSERT INTO "material_types" (name, [description]) VALUES('Pu','Pu');
GO
INSERT INTO "material_types" VALUES(2,'PUOX','PUOX');
GO
INSERT INTO "mbas" (name, [description]) VALUES('XXXX','XXXX');
GO
INSERT INTO "mbas" (name, [description]) VALUES('matBalArea','descriptionOfMBA');
GO
INSERT INTO "stratum_ids"  (name, [description], historical_bias,historical_rand_uncert,historical_systematic_uncert) VALUES('XXXX','XXXX',0.0,0.0,0.0);
GO
INSERT INTO "detectors" (detector_name,detector_type_id,detector_type_freeform,electronics_id,detector_alias) VALUES('XXXX/XXX/YY',10,'AMSR','XXXX/XXX','Default');
GO
INSERT INTO "detectors" (detector_name,detector_type_id,detector_type_freeform,electronics_id,detector_alias) VALUES('LM09',14,'LMMM','68Y-156220','hon-yock');
GO
INSERT INTO "sr_parms_rec" VALUES(1,'XXXX/XXX/YY',10,1,45.0,640.0,0.0,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001,NULL);
GO
INSERT INTO "alpha_beta_rec" VALUES(1,'0','0');
GO
INSERT INTO "bkg_parms_rec" 
(detector_id,passive_bkg_singles_rate,[passive_bkg_singles_rate_err],[passive_bkg_doubles_rate],[passive_bkg_doubles_rate_err],[passive_bkg_triples_rate],[passive_bkg_triples_rate_err],
[active_bkg_singles_rate],[active_bkg_singles_rate_err],[passive_bkg_scaler1_rate],[passive_bkg_scaler2_rate],[active_bkg_scaler1_rate],[active_bkg_scaler2_rate]
) VALUES(1, 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO "norm_parms_rec" 
(detector_id, [source_id],[normalization_constant],[normalization_constant_err],[bias_mode],[meas_rate],[meas_rate_err],[amli_ref_singles_rate],
 [cf252_ref_doubles_rate],[cf252_ref_doubles_rate_err],[ref_date],[init_src_precision_limit],[bias_precision_limit],[acceptance_limit_std_dev],
 [acceptance_limit_percent],[yield_relative_to_mrc_95],[bias_test_use_addasrc],[bias_test_addasrc_position])
VALUES(1,'',1.0,0.0,0,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO "add_a_source_setup_rec" 
	([detector_id],[type],[port_number],[forward_over_travel],[reverse_over_travel],[number_positions],[dist_to_move],
	[cm_steps_per_inch],[cm_forward_mask],[cm_reverse_mask],[cm_axis_number],[cm_over_travel_state],
	[cm_step_ratio],[cm_slow_inches],[plc_steps_per_inch],[scale_conversion_factor],[cm_rotation])
	VALUES(1,'PSC_WDAS',1,0,0,1,'0,0,0,0,0', 625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO "stratum_id_detector" VALUES(1,1);
GO
INSERT INTO "unattended_parms_rec" ([detector_id],[error_seconds],[auto_import],[add_a_source_threshold]) VALUES(1,600,'',0.0);
GO
INSERT INTO "HVCalibrationParams" ([detector_id],[minv],[maxv],[duration],[stepv],[delay]) VALUES(1,0,2000,10,5,2000);
GO
INSERT INTO "analysis_method_rec"  (item_type_id,[analysis_method_detector_id],[cal_curve],[known_alpha],
[known_m],[multiplicity],[add_a_source],[active],[active_mult],
[active_passive],[collar],[normal_method],[backup_method],[curium_ratio],[truncated_mult]) VALUES(1,1,1,0,0,0,0,0,0,0,0,0,0,0,0);
GO
INSERT INTO "sr_parms_rec" 	(detector_id,[sr_detector_id],[sr_type],[sr_port_number],[predelay],[gate_length],[gate_length2],
[high_voltage],[die_away_time],[efficiency],	[multiplicity_deadtime],[coeff_a_deadtime],[coeff_b_deadtime],
[coeff_c_deadtime],[doubles_gate_fraction],[triples_gate_fraction],[sr_baud]) 
VALUES(2,'LM09',14,1,45.0,640.0,0.0,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001, NULL);
GO
INSERT INTO "alpha_beta_rec" VALUES(2,'0','0');
GO
INSERT INTO "bkg_parms_rec" 
(detector_id,passive_bkg_singles_rate,[passive_bkg_singles_rate_err],[passive_bkg_doubles_rate],[passive_bkg_doubles_rate_err],[passive_bkg_triples_rate],[passive_bkg_triples_rate_err],
[active_bkg_singles_rate],[active_bkg_singles_rate_err],[active_bkg_doubles_rate],[active_bkg_doubles_rate_err],[active_bkg_triples_rate],[active_bkg_triples_rate_err],[passive_bkg_scaler1_rate],[passive_bkg_scaler2_rate],[active_bkg_scaler1_rate],[active_bkg_scaler2_rate]
) VALUES(2, 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO "norm_parms_rec" 
(detector_id, [source_id],[normalization_constant],[normalization_constant_err],[bias_mode],[meas_rate],[meas_rate_err],[amli_ref_singles_rate],
 [cf252_ref_doubles_rate],[cf252_ref_doubles_rate_err],[ref_date],[init_src_precision_limit],[bias_precision_limit],[acceptance_limit_std_dev],
 [acceptance_limit_percent],[yield_relative_to_mrc_95],[bias_test_use_addasrc],[bias_test_addasrc_position])
VALUES(2,'',1.0,0.0,0,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO "add_a_source_setup_rec" 
	([detector_id],[type],[port_number],[forward_over_travel],[reverse_over_travel],[number_positions],[dist_to_move],
	[cm_steps_per_inch],[cm_forward_mask],[cm_reverse_mask],[cm_axis_number],[cm_over_travel_state],
	[cm_step_ratio],[cm_slow_inches],[plc_steps_per_inch],[scale_conversion_factor],[cm_rotation])
VALUES(2,'None',1,0,0,1,'0,0,0,0,0', 625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO "stratum_id_detector" VALUES(2,1);
GO
INSERT INTO "unattended_parms_rec" ([detector_id],[error_seconds],[auto_import],[add_a_source_threshold]) VALUES(2,600,'',0.0);
GO
INSERT INTO "HVCalibrationParams" ([detector_id],[minv],[maxv],[duration],[stepv],[delay]) VALUES(2,0,2000,10,5,2000);
GO
INSERT INTO "analysis_method_rec" 
(item_type_id,[analysis_method_detector_id],[cal_curve],[known_alpha],[known_m],[multiplicity],[add_a_source],[active],
[active_mult], [active_passive],[collar],[normal_method],[backup_method],[curium_ratio],[truncated_mult]) 
VALUES(1,2,1,0,0,0,0,0,0,0,0,0,0,0,0);
GO
INSERT INTO "isotopics" 
([pu238],[pu239],[pu240],[pu241],[pu242],[am241],[pu238_err],[pu239_err],[pu240_err],[pu241_err],[pu242_err],[am241_err],
[pu_date],[am_date],[isotopics_id],[isotopics_source_code])
VALUES(0.0,0.0,100.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,'2010-01-01','2010-01-01','Default','OD');
GO
INSERT INTO "item_isotopics" (isotopics_source_code,isotopics_name) VALUES('OD','Default');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('JSR11','JSR11');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('SR4','SR4');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('JSR12','JSR12');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('HNC','HNC');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('JSR11A','JSR11A');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('MSR4','MSR4 or 2150');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('MSR4A','MSR4A');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('PSR','PSR4 or ISR');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('DGSR','DGSR');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('AMSR','AMSR');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('JSR15','JSR15');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('UNAP','UNAP');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('NPOD','NPOD');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('LMMM','LMMM');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('PTR32','PTR-32');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('MCA527','GBS Elektronik GmbH');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('N1','N1');
GO
INSERT INTO "detector_types" (name, [description]) VALUES('MCNPX','MCNPX');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('RD','Received during interim (domestic)');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('RF','Received during interim (foreign)');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('RN','Received during interim');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('SD','Shipped/measured during interim inspection (domestic)');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('SF','Shipped/measured during interim inspection (foreign)');
GO
INSERT INTO "inventory_change_code" (name, [description])  VALUES('SN','Shipped/measured during interim inspection');
GO
INSERT INTO "io_code" (name, [description])  VALUES('A','Received/measured during PIV and before PIV date');
GO
INSERT INTO "io_code" (name, [description])  VALUES('B','Shipped/measured during PIV and before PIV date');
GO
INSERT INTO "io_code" (name, [description])  VALUES('D','Shipped/measured during PIV and after PIV date');
GO
INSERT INTO "io_code" (name, [description])  VALUES('C','Received/measured during PIV and after PIV date');
GO
INSERT INTO "io_code" (name, [description])  VALUES('F','Flow measurement');
GO
INSERT INTO "io_code" (name, [description])  VALUES('I','Received during interim');
GO
INSERT INTO "io_code" (name, [description])  VALUES('0','Shipped/measured during interim');
GO
INSERT INTO "io_code" (name, [description])  VALUES('T','Test measurement');
GO
INSERT INTO "io_code" (name, [description])  VALUES('X','IIV');
GO
INSERT INTO "io_code" (name, [description])  VALUES('Z','Other');
GO
INSERT INTO "LMNetComm" VALUES(2,1,5002,5201,'127.0.0.1',500,8,8192,50,1,1,1);
Go
INSERT INTO "LMHWParams" VALUES(2,1,0,0,1705,600,30);
Go
INSERT INTO "LMINCCAppContext" VALUES('c:\temp',1,1,0,4,30,50,0,3,1,2,'MLMEmulator.exe',0,'./',0,0,0,1,NULL,0,0,0,0,0,0,0,0,0,0,128,1000,0,0,1,0,0);
GO
INSERT INTO "detectors" VALUES(3,'AS710',15,'PTR-32HV','N026','AS710-PTR32');
GO
INSERT INTO "add_a_source_setup_rec" VALUES(3,'None',1,0.0,0.0,1,'0,0,0,0,0',625.0,2,1,1,0,1.0,2.0,51.0,1.0,0);
GO
INSERT INTO "alpha_beta_rec" VALUES(3,'','');
GO
INSERT INTO "bkg_parms_rec" VALUES(3,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0);
GO
INSERT INTO "norm_parms_rec" VALUES(3,'',1.0,0.0,1,0.0,0.0,0.0,0.0,0.0,'2010-01-01T00:00:00',0.3,0.3,3.0,4.0,1.0,'0',0.0);
GO
INSERT INTO "sr_parms_rec" VALUES(3,'AS710',15,NULL,45.0,640.0,NULL,1680.0,500.0,0.0001,0.0,0.0,0.0,0.0,0.0001,0.0001,NULL);
GO
INSERT INTO "unattended_parms_rec" VALUES(3,600,1,0.0);
GO
INSERT INTO "HVCalibrationParams"  VALUES(3,5,2000,10,100,2000);
GO
INSERT INTO "stratum_id_detector" VALUES(3,1);
GO
INSERT INTO "LMNetComm" VALUES(3,1,5011,5000,'169.254.255.255',500,8,8192,50,0,1,0);
GO
INSERT INTO "LMHWParams" VALUES(3,1,1,0,1650,2,30);
GO
INSERT INTO "known_alpha_rec" VALUES(1,2,1,0.0,0.0,2.166,0.0,1.0,0.0,0.0,0.0,0.0,0,0.0,0.0,0,0.0,0.0,0.0,0.0,0.0,100000000.0,NULL,NULL,'0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0','0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0');
GO
INSERT INTO "multiplicity_rec" VALUES(1,2,1,0,473.5,2.154,3.789,5.211,3.163,8.24,17.321,1.0,0.0,0.0,0.0,1.0,NULL,NULL,1.0);
GO
INSERT INTO "analysis_method_rec" VALUES(4,2,1,1,1,0,1,0,0,0,0,0,4,2,0,0,NULL,NULL,NULL,NULL);
GO
INSERT INTO "cal_curve_rec" VALUES(1,2,1,0.0,1.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0,0,0.0,0.0,0.0,-100000000.0,100000000.0,'0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0','0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0');
GO
INSERT INTO "acquire_parms_rec" VALUES('Empty','Empty','XXXX','XXXX','AS710','Pu','','Default','Default','','','','','','','0',0,1,0,1,1,1,0,0,1,0,0,100.0,0,3,3,10,1000,1.0,0,0.0,2,'','',0,0.0,'2014-11-25T13:18:10','AS710');
GO
INSERT INTO "LMAcquireParams" VALUES(3,0,12,6,0,0,0,0,0,0,0,NULL,0,'c:\temp',0,'',0,0,'2014-11-25T15:23:08','Pu',1);
GO
