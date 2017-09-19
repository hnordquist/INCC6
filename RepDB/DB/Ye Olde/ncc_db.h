/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
#ifndef NCC_DB_H
#define NCC_DB_H

/* Raima Database Manager 4.5 [Build 17] */

/* schema file for INCC version 5.00 database */

#define PASS_FAIL_LENGTH 5		/* length of Pass/Fail string */
#define MAX_RUN_TESTS_LENGTH 21		/* max length of a run tests string */
#define NUM_ERROR_MSG_CODES 10		/* max number of error msg codes */
#define NUM_WARNING_MSG_CODES 10	/* max number of warning msg codes */
#define MAX_DETECTORS 100		/* max number of detectors */
#define MAX_ITEM_TYPES 100		/* max number of item types */
#define MAX_FACILITIES 50		/* max number of facilities */
#define MAX_MBAS 50			/* max number of mbas */
#define MAX_STRATUM_IDS 100		/* max number of stratum ids */
#define MAX_CAMPAIGN_IDS 100		/* max number of campaigns */
#define MAX_GLOVEBOX_IDS 100		/* max # glovebox ids */
#define MAX_ITEM_IDS 392		/* max # item ids */
#define MAX_ISOTOPICS_IDS 1000		/* max number of isotopic ids */
#define MAX_ISO_COMPS 20		/* max number isotopic compositions */
#define MAX_COLLAR_DATA_SETS 100	/* max number collar data sets */
#define MAX_COLLAR_K5_PARAMETERS 20	/* max number collar K5 parameters */
#define MAX_K5_LABEL_LENGTH 31		/* max length K5 label */
#define MAX_INVENTORY_CHG_CODES 31	/* max number inventory change codes */
#define MAX_IO_CODES 28			/* max number I/O codes */
#define MAX_POISON_ROD_TYPES 10		/* max # different types poison rods */
#define DATE_TIME_LENGTH 9		/* length of a date or time string */
#define FILE_NAME_LENGTH 13		/* length file name including suffix */
#define CHAR_FIELD_LENGTH 41		/* length of database strings */
#define LONG_FILE_NAME_LENGTH 256	/* length long file name including suffix */
#define MAX_PATH_LENGTH LONG_FILE_NAME_LENGTH /* length of file name path including drive and suffix */
#define MAX_COMMENT_LENGTH 51		/* length of measurement comments */
#define ERR_MSG_LENGTH 81		/* length of database error msg */
#define MULTI_ARRAY_SIZE 128		/* size of multiplicity arrays */
#define MAX_NUM_CALIB_PTS 20		/* max # calibration pts pairs in db */
#define MAX_ASSAY_SUMMARY_VARS 130	/* max # assay summary variables */
#define MAX_HOLDUP_SUMMARY_VARS 50	/* max # holdup summary variables */
#define SOURCE_ID_LENGTH 13		/* max length of ref Cf252/AmLi source id */
#define INVENTORY_CHG_LENGTH 3		/* length of inventory change code */
#define IO_CODE_LENGTH 2		/* length of I/O code */
#define MBA_LENGTH 5			/* length of an MBA code */
#define ISO_SOURCE_CODE_LENGTH 3	/* length of an isotopics source code */
#define FACILITY_LENGTH 13		/* length of a facility name */
#define MAX_ITEM_TYPE_LENGTH 6		/* length of an item type name string */
#define MAX_ITEM_ID_LENGTH 13
#define MAX_CAMPAIGN_ID_LENGTH 13
#define MAX_GLOVEBOX_ID_LENGTH 21
#define MAX_STRATUM_ID_LENGTH 13
#define MAX_ISOTOPICS_ID_LENGTH 13
#define MAX_DETECTOR_ID_LENGTH 12
#define DETECTOR_TYPE_LENGTH 12		/* length of a detector type */
#define ELECTRONICS_ID_LENGTH 9		/* length of an electronics id */
#define DESCRIPTION_LENGTH 21		/* length of a stratum id, facility and MBA description */
#define MAX_ROD_TYPE_LENGTH 2
#define MAX_ADDASRC_POSITIONS 5		// max # add-a-source positions
#define MAX_DUAL_ENERGY_ROWS 25		// max # dual energy rows for mult
#define NUMBER_SR_SPARES 10		// # double spares in detector db
#define NUMBER_RESULTS_SPARES 93	// # double spares in results db
#define NUMBER_RUN_SPARES 10		// # double spares in run db
#define NUMBER_BIAS_SPARES 10		// # double spares in bias results db
#define NUMBER_PREC_SPARES 10		// # double spares in precision results db
#define NUMBER_CC_SPARES 6		// # double spares in calibration curve db
#define NUMBER_KA_SPARES 0		// # double spares in known alpha db
#define NUMBER_KM_SPARES 10		// # double spares in known M db
#define NUMBER_MUL_SPARES 9		// # double spares in multiplicity db
#define NUMBER_AP_SPARES 10		// # double spares in active/passive db
#define NUMBER_ACT_SPARES 10		// # double spares in active db
#define NUMBER_COL_SPARES 10		// # double spares in collar db
#define NUMBER_AD_SPARES 10		// # double spares in add-a-source db
#define NUMBER_AD_CF_SPARES 7		// # double spares in add-a-source cf db
#define NUMBER_AM_SPARES 10		// # double spares in active multiplication db
#define NUMBER_CR_SPARES 10		// # double spares in curium ratio db
#define NUMBER_TM_SPARES 10		// # double spares in truncated multiplicity db
#define NUMBER_CC_RESULTS_SPARES 2	// # double spares in calibration curve results db
#define NUMBER_KA_RESULTS_SPARES 0	// # double spares in known alpha results db
#define NUMBER_KM_RESULTS_SPARES 10	// # double spares in known M results db
#define NUMBER_MUL_RESULTS_SPARES 10	// # double spares in multiplicity results db
#define NUMBER_AP_RESULTS_SPARES 10	// # double spares in active/passive results db
#define NUMBER_ACT_RESULTS_SPARES 10	// # double spares in active results db
#define NUMBER_COL_RESULTS_SPARES 10	// # double spares in collar results db
#define NUMBER_AD_RESULTS_SPARES 4	// # double spares in add-a-source results db
#define NUMBER_AM_RESULTS_SPARES 10	// # double spares in active multiplication results db
#define NUMBER_CR_RESULTS_SPARES 10	// # double spares in curium ratio results db
#define NUMBER_TM_RESULTS_SPARES 10	// # double spares in truncated multiplicity results db


/* database ncc_db record/key structure declarations */

struct archive_parms_rec {
   unsigned short days_before_auto_archive;
   unsigned short days_before_auto_delete;
   unsigned short days_before_db_auto_delete;
   char data_dir[MAX_PATH_LENGTH];
};

struct test_parms_rec {
   double acc_sngl_test_rate_limit;
   double acc_sngl_test_precision_limit;
   double acc_sngl_test_outlier_limit;
   double outlier_test_limit;
   double bkg_doubles_rate_limit;
   double bkg_triples_rate_limit;
   double chisq_limit;
   unsigned short max_num_failures;
   double high_voltage_test_limit;
   double normal_backup_assay_test_limit;
   unsigned short max_runs_for_outlier_test;
   unsigned char checksum_test;
   unsigned short accidentals_method;
};

struct acquire_parms_rec {
   char acq_facility[FACILITY_LENGTH];
   char acq_facility_description[DESCRIPTION_LENGTH];
   char acq_mba[MBA_LENGTH];
   char acq_mba_description[DESCRIPTION_LENGTH];
   char acq_detector_id[MAX_DETECTOR_ID_LENGTH];
   char acq_item_type[MAX_ITEM_TYPE_LENGTH];
   char acq_glovebox_id[MAX_GLOVEBOX_ID_LENGTH];
   char acq_isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
   char acq_comp_isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
   char acq_campaign_id[MAX_CAMPAIGN_ID_LENGTH];
   char acq_item_id[MAX_ITEM_ID_LENGTH];
   char acq_stratum_id[MAX_STRATUM_ID_LENGTH];
   char acq_stratum_id_description[DESCRIPTION_LENGTH];
   char acq_user_id[CHAR_FIELD_LENGTH];
   char acq_comment[MAX_COMMENT_LENGTH];
   unsigned char acq_ending_comment;
   unsigned char acq_data_src;
   unsigned char acq_qc_tests;
   unsigned char acq_print;
   unsigned char acq_review_detector_parms;
   unsigned char acq_review_calib_parms;
   unsigned char acq_review_isotopics;
   unsigned char acq_review_summed_raw_data;
   unsigned char acq_review_run_raw_data;
   unsigned char acq_review_run_rate_data;
   unsigned char acq_review_summed_mult_dist;
   unsigned char acq_review_run_mult_dist;
   double acq_run_count_time;
   unsigned short acq_acquire_type;
   unsigned short acq_num_runs;
   unsigned short acq_active_num_runs;
   unsigned short acq_min_num_runs;
   unsigned short acq_max_num_runs;
   double acq_meas_precision;
   unsigned char acq_well_config;
   double acq_mass;
   unsigned short acq_error_calc_method;
   char acq_inventory_change_code[INVENTORY_CHG_LENGTH];
   char acq_io_code[IO_CODE_LENGTH];
   unsigned char acq_collar_mode;
   double acq_drum_empty_weight;
   char acq_meas_date[DATE_TIME_LENGTH];
   char acq_meas_time[DATE_TIME_LENGTH];
   char acq_meas_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct item_id_entry_rec {
   char item_id_entry[MAX_ITEM_IDS][MAX_ITEM_ID_LENGTH];
   char item_type_ascii[MAX_ITEM_IDS][MAX_ITEM_TYPE_LENGTH];
   char isotopics_id_ascii[MAX_ITEM_IDS][MAX_ISOTOPICS_ID_LENGTH];
   char stratum_id_ascii[MAX_ITEM_IDS][MAX_STRATUM_ID_LENGTH];
   char inventory_change_code_entry[MAX_ITEM_IDS][INVENTORY_CHG_LENGTH];
   char io_code_entry[MAX_ITEM_IDS][IO_CODE_LENGTH];
   double declared_mass_entry[MAX_ITEM_IDS];
   double declared_u_mass_entry[MAX_ITEM_IDS];
   double length_entry[MAX_ITEM_IDS];
};

struct mba_item_id_entry_rec {
   char mba_ascii[MAX_ITEM_IDS][MBA_LENGTH];
};

struct collar_data_entry_rec {
   char col_item_id_entry[MAX_COLLAR_DATA_SETS][MAX_ITEM_ID_LENGTH];
   double col_length_entry[MAX_COLLAR_DATA_SETS];
   double col_length_err_entry[MAX_COLLAR_DATA_SETS];
   double col_total_pu_entry[MAX_COLLAR_DATA_SETS];
   double col_total_pu_err_entry[MAX_COLLAR_DATA_SETS];
   double col_depleted_u_entry[MAX_COLLAR_DATA_SETS];
   double col_depleted_u_err_entry[MAX_COLLAR_DATA_SETS];
   double col_natural_u_entry[MAX_COLLAR_DATA_SETS];
   double col_natural_u_err_entry[MAX_COLLAR_DATA_SETS];
   double col_enriched_u_entry[MAX_COLLAR_DATA_SETS];
   double col_enriched_u_err_entry[MAX_COLLAR_DATA_SETS];
   double col_total_u235_entry[MAX_COLLAR_DATA_SETS];
   double col_total_u235_err_entry[MAX_COLLAR_DATA_SETS];
   double col_total_u238_entry[MAX_COLLAR_DATA_SETS];
   double col_total_u238_err_entry[MAX_COLLAR_DATA_SETS];
   double col_total_rods_entry[MAX_COLLAR_DATA_SETS];
   double col_total_poison_rods_entry[MAX_COLLAR_DATA_SETS];
   double col_poison_percent_entry[MAX_COLLAR_DATA_SETS];
   double col_poison_percent_err_entry[MAX_COLLAR_DATA_SETS];
   char col_rod_type_entry[MAX_COLLAR_DATA_SETS][MAX_ROD_TYPE_LENGTH];
};

struct poison_rod_type_rec {
   char poison_rod_type[MAX_POISON_ROD_TYPES][MAX_ROD_TYPE_LENGTH];
   double poison_absorption_fact[MAX_POISON_ROD_TYPES];
};

struct holdup_config_rec {
   char glovebox_id[MAX_GLOVEBOX_ID_LENGTH];
   unsigned short num_rows;
   unsigned short num_columns;
   double distance;
};

struct cm_pu_ratio_rec {
   double cm_pu_ratio;
   double cm_pu_ratio_err;
   double cm_pu_half_life;
   char cm_pu_ratio_date[DATE_TIME_LENGTH];
   double cm_u_ratio;
   double cm_u_ratio_err;
   char cm_u_ratio_date[DATE_TIME_LENGTH];
   char cm_id_label[MAX_ITEM_ID_LENGTH];
   char cm_id[MAX_ITEM_ID_LENGTH];
   char cm_input_batch_id[MAX_ITEM_ID_LENGTH];
   double cm_dcl_u_mass;
   double cm_dcl_u235_mass;
};

struct item_type_names_rec {
   char item_type_names[MAX_ITEM_TYPES][MAX_ITEM_TYPE_LENGTH];
};

struct facility_names_rec {
   char facility_names[MAX_FACILITIES][FACILITY_LENGTH];
   char facility_description[MAX_FACILITIES][DESCRIPTION_LENGTH];
};

struct mba_names_rec {
   char mba_names[MAX_MBAS][MBA_LENGTH];
   char mba_description[MAX_MBAS][DESCRIPTION_LENGTH];
};

struct stratum_id_names_rec {
   char stratum_id_names[MAX_STRATUM_IDS][MAX_STRATUM_ID_LENGTH];
   char stratum_id_names_description[MAX_STRATUM_IDS][DESCRIPTION_LENGTH];
};

struct inventory_change_code_rec {
   char inventory_chg_codes[MAX_INVENTORY_CHG_CODES][INVENTORY_CHG_LENGTH];
};

struct io_code_rec {
   char io_codes[MAX_IO_CODES][IO_CODE_LENGTH];
};

struct isotopics_rec {
   double pu238;
   double pu239;
   double pu240;
   double pu241;
   double pu242;
   double am241;
   double pu238_err;
   double pu239_err;
   double pu240_err;
   double pu241_err;
   double pu242_err;
   double am241_err;
   char pu_date[DATE_TIME_LENGTH];
   char am_date[DATE_TIME_LENGTH];
   char isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
   char isotopics_source_code[ISO_SOURCE_CODE_LENGTH];
};

struct composite_isotopics_rec {
   char ci_ref_date[DATE_TIME_LENGTH];
   double ci_pu_mass[MAX_ISO_COMPS];
   double ci_pu238[MAX_ISO_COMPS];
   double ci_pu239[MAX_ISO_COMPS];
   double ci_pu240[MAX_ISO_COMPS];
   double ci_pu241[MAX_ISO_COMPS];
   double ci_pu242[MAX_ISO_COMPS];
   double ci_am241[MAX_ISO_COMPS];
   char ci_pu_date[MAX_ISO_COMPS][DATE_TIME_LENGTH];
   char ci_am_date[MAX_ISO_COMPS][DATE_TIME_LENGTH];
   char ci_isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
   char ci_isotopics_source_code[ISO_SOURCE_CODE_LENGTH];
};

struct assay_summary_rec {
   unsigned char as_print;
   char as_path[MAX_PATH_LENGTH];
   int as_select[MAX_ASSAY_SUMMARY_VARS];
};

struct holdup_summary_rec {
   unsigned char hu_print;
   char hu_path[MAX_PATH_LENGTH];
   int hu_select[MAX_HOLDUP_SUMMARY_VARS];
};

struct detector_rec {
   char detector_id[MAX_DETECTOR_ID_LENGTH];
   char detector_type[DETECTOR_TYPE_LENGTH];
   char electronics_id[ELECTRONICS_ID_LENGTH];
};

struct alpha_beta_rec {
   double factorial[MULTI_ARRAY_SIZE];
   double alpha_array[MULTI_ARRAY_SIZE];
   double beta_array[MULTI_ARRAY_SIZE];
};

struct bkg_parms_rec {
   double curr_passive_bkg_singles_rate;
   double curr_passive_bkg_singles_err;
   double curr_passive_bkg_doubles_rate;
   double curr_passive_bkg_doubles_err;
   double curr_passive_bkg_triples_rate;
   double curr_passive_bkg_triples_err;
   double curr_active_bkg_singles_rate;
   double curr_active_bkg_singles_err;
   double curr_passive_bkg_scaler1_rate;
   double curr_passive_bkg_scaler2_rate;
   double curr_active_bkg_scaler1_rate;
   double curr_active_bkg_scaler2_rate;
};

struct tm_bkg_parms_rec {
   double tm_singles_bkg;
   double tm_singles_bkg_err;
   double tm_zeros_bkg;
   double tm_zeros_bkg_err;
   double tm_ones_bkg;
   double tm_ones_bkg_err;
   double tm_twos_bkg;
   double tm_twos_bkg_err;
   unsigned char tm_bkg;
};

struct norm_parms_rec {
   char source_id[SOURCE_ID_LENGTH];
   double curr_normalization_constant;
   double curr_normalization_constant_err;
   unsigned short bias_mode;
   double meas_rate;
   double meas_rate_err;
   double amli_ref_singles_rate;
   double cf252_ref_doubles_rate;
   double cf252_ref_doubles_rate_err;
   char ref_date[DATE_TIME_LENGTH];
   double init_src_precision_limit;
   double bias_precision_limit;
   double acceptance_limit_std_dev;
   double acceptance_limit_percent;
   double yield_relative_to_mrc_95;
   unsigned char bias_test_use_addasrc;
   double bias_test_addasrc_position;
};

struct sr_parms_rec {
   char sr_detector_id[MAX_DETECTOR_ID_LENGTH];
   short sr_type;
   short sr_port_number;
   double predelay;
   double gate_length;
   double gate_length2;
   double high_voltage;
   double die_away_time;
   double efficiency;
   double multiplicity_deadtime;
   double coeff_a_deadtime;
   double coeff_b_deadtime;
   double coeff_c_deadtime;
   double doubles_gate_fraction;
   double triples_gate_fraction;
   double sr_spares[NUMBER_SR_SPARES];
};

struct unattended_parms_rec {
   unsigned long error_seconds;
   unsigned char auto_import;
   double add_a_source_threshold;
};

struct analysis_method_rec {
   char item_type[MAX_ITEM_TYPE_LENGTH];
   char analysis_method_detector_id[MAX_DETECTOR_ID_LENGTH];
   unsigned char cal_curve;
   unsigned char known_alpha;
   unsigned char known_m;
   unsigned char multiplicity;
   unsigned char add_a_source;
   unsigned char active;
   unsigned char active_mult;
   unsigned char active_passive;
   unsigned char collar;
   unsigned char normal_method;
   unsigned char backup_method;
   unsigned char curium_ratio;
   unsigned char truncated_mult;
   unsigned char analysis_method_spare1;
   unsigned char analysis_method_spare2;
   unsigned char analysis_method_spare3;
   unsigned char analysis_method_spare4;
};

struct cal_curve_rec {
   unsigned char cal_curve_equation;
   double cc_a;
   double cc_b;
   double cc_c;
   double cc_d;
   double cc_var_a;
   double cc_var_b;
   double cc_var_c;
   double cc_var_d;
   double cc_covar_ab;
   double cc_covar_ac;
   double cc_covar_ad;
   double cc_covar_bc;
   double cc_covar_bd;
   double cc_covar_cd;
   double cc_sigma_x;
   double cc_cal_curve_type;
   double cc_heavy_metal_corr_factor;
   double cc_heavy_metal_reference;
   double cc_percent_u235;
   double cal_curve_spares[NUMBER_CC_SPARES];
   char cal_curve_item_type[MAX_ITEM_TYPE_LENGTH];
   char cal_curve_detector_id[MAX_DETECTOR_ID_LENGTH];
   double cc_lower_mass_limit;
   double cc_upper_mass_limit;
   double cc_dcl_mass[MAX_NUM_CALIB_PTS];
   double cc_doubles[MAX_NUM_CALIB_PTS];
};

struct active_rec {
   unsigned char active_equation;
   double act_a;
   double act_b;
   double act_c;
   double act_d;
   double act_var_a;
   double act_var_b;
   double act_var_c;
   double act_var_d;
   double act_covar_ab;
   double act_covar_ac;
   double act_covar_ad;
   double act_covar_bc;
   double act_covar_bd;
   double act_covar_cd;
   double act_sigma_x;
   double active_spares[NUMBER_ACT_SPARES];
   char active_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_detector_id[MAX_DETECTOR_ID_LENGTH];
   double act_lower_mass_limit;
   double act_upper_mass_limit;
   double act_dcl_mass[MAX_NUM_CALIB_PTS];
   double act_doubles[MAX_NUM_CALIB_PTS];
};

struct collar_rec {
   unsigned char collar_equation;
   double col_a;
   double col_b;
   double col_c;
   double col_d;
   double col_var_a;
   double col_var_b;
   double col_var_c;
   double col_var_d;
   double col_covar_ab;
   double col_covar_ac;
   double col_covar_ad;
   double col_covar_bc;
   double col_covar_bd;
   double col_covar_cd;
   double col_sigma_x;
   double col_number_calib_rods;
   char col_poison_rod_type[MAX_POISON_ROD_TYPES][MAX_ROD_TYPE_LENGTH];
   double col_poison_absorption_fact[MAX_POISON_ROD_TYPES];
   double col_poison_rod_a[MAX_POISON_ROD_TYPES];
   double col_poison_rod_a_err[MAX_POISON_ROD_TYPES];
   double col_poison_rod_b[MAX_POISON_ROD_TYPES];
   double col_poison_rod_b_err[MAX_POISON_ROD_TYPES];
   double col_poison_rod_c[MAX_POISON_ROD_TYPES];
   double col_poison_rod_c_err[MAX_POISON_ROD_TYPES];
   double col_u_mass_corr_fact_a;
   double col_u_mass_corr_fact_a_err;
   double col_u_mass_corr_fact_b;
   double col_u_mass_corr_fact_b_err;
   double col_sample_corr_fact;
   double col_sample_corr_fact_err;
   double collar_spares[NUMBER_COL_SPARES];
   char collar_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char collar_mode;
   double col_lower_mass_limit;
   double col_upper_mass_limit;
};

struct collar_detector_rec {
   char collar_detector_item_type[MAX_ITEM_TYPE_LENGTH];
   //Change to int, can now hold CollarType
   int collar_detector_mode;
   char collar_detector_id[MAX_DETECTOR_ID_LENGTH];
   char col_reference_date[DATE_TIME_LENGTH];
   double col_relative_doubles_rate;
};

struct collar_k5_rec {
   char collar_k5_label[MAX_COLLAR_K5_PARAMETERS][MAX_K5_LABEL_LENGTH];
   int collar_k5_checkbox[MAX_COLLAR_K5_PARAMETERS];
   double collar_k5[MAX_COLLAR_K5_PARAMETERS];
   double collar_k5_err[MAX_COLLAR_K5_PARAMETERS];
   char collar_k5_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char collar_k5_mode;
};

struct known_alpha_rec {
   double ka_alpha_wt;
   double ka_rho_zero;
   double ka_k;
   double ka_a;
   double ka_b;
   double ka_var_a;
   double ka_var_b;
   double ka_covar_ab;
   double ka_sigma_x;
   double ka_known_alpha_type;
   double ka_heavy_metal_corr_factor;
   double ka_heavy_metal_reference;
   double ka_ring_ratio_equation;
   double ka_ring_ratio_a;
   double ka_ring_ratio_b;
   double ka_ring_ratio_c;
   double ka_ring_ratio_d;
   double ka_lower_corr_factor_limit;
   double ka_upper_corr_factor_limit;
   char known_alpha_item_type[MAX_ITEM_TYPE_LENGTH];
   char known_alpha_detector_id[MAX_DETECTOR_ID_LENGTH];
   double ka_lower_mass_limit;
   double ka_upper_mass_limit;
   double ka_dcl_mass[MAX_NUM_CALIB_PTS];
   double ka_doubles[MAX_NUM_CALIB_PTS];
};

struct known_m_rec {
   double km_sf_rate;
   double km_vs1;
   double km_vs2;
   double km_vi1;
   double km_vi2;
   double km_b;
   double km_c;
   double km_sigma_x;
   double known_m_spares[NUMBER_KM_SPARES];
   char known_m_item_type[MAX_ITEM_TYPE_LENGTH];
   char known_m_detector_id[MAX_DETECTOR_ID_LENGTH];
   double km_lower_mass_limit;
   double km_upper_mass_limit;
};

struct multiplicity_rec {
   unsigned char mul_solve_efficiency;
   double mul_sf_rate;
   double mul_vs1;
   double mul_vs2;
   double mul_vs3;
   double mul_vi1;
   double mul_vi2;
   double mul_vi3;
   double mul_a;
   double mul_b;
   double mul_c;
   double mul_sigma_x;
   double mul_alpha_weight;
   double multiplicity_spares[NUMBER_MUL_SPARES];
   char multiplicity_item_type[MAX_ITEM_TYPE_LENGTH];
   char multiplicity_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct active_passive_rec {
   unsigned char active_passive_equation;
   double ap_a;
   double ap_b;
   double ap_c;
   double ap_d;
   double ap_var_a;
   double ap_var_b;
   double ap_var_c;
   double ap_var_d;
   double ap_covar_ab;
   double ap_covar_ac;
   double ap_covar_ad;
   double ap_covar_bc;
   double ap_covar_bd;
   double ap_covar_cd;
   double ap_sigma_x;
   double active_passive_spares[NUMBER_AP_SPARES];
   char active_passive_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_passive_detector_id[MAX_DETECTOR_ID_LENGTH];
   double ap_lower_mass_limit;
   double ap_upper_mass_limit;
};

struct add_a_source_rec {
   unsigned char add_a_source_equation;
   double ad_a;
   double ad_b;
   double ad_c;
   double ad_d;
   double ad_var_a;
   double ad_var_b;
   double ad_var_c;
   double ad_var_d;
   double ad_covar_ab;
   double ad_covar_ac;
   double ad_covar_ad;
   double ad_covar_bc;
   double ad_covar_bd;
   double ad_covar_cd;
   double ad_sigma_x;
   double add_a_source_spares[NUMBER_AD_SPARES];
   double ad_position_dzero[MAX_ADDASRC_POSITIONS];
   double ad_dzero_avg;
   char ad_dzero_ref_date[DATE_TIME_LENGTH];
   unsigned short ad_num_runs;
   double ad_cf_a;
   double ad_cf_b;
   double ad_cf_c;
   double ad_cf_d;
   double ad_use_truncated_mult;
   double ad_tm_weighting_factor;
   double ad_tm_dbls_rate_upper_limit;
   double add_a_source_cf_spares[NUMBER_AD_CF_SPARES];
   char add_a_source_item_type[MAX_ITEM_TYPE_LENGTH];
   char add_a_source_detector_id[MAX_DETECTOR_ID_LENGTH];
   double ad_lower_mass_limit;
   double ad_upper_mass_limit;
   double ad_dcl_mass[MAX_NUM_CALIB_PTS];
   double ad_doubles[MAX_NUM_CALIB_PTS];
};

struct active_mult_rec {
   double am_vt1;
   double am_vt2;
   double am_vt3;
   double am_vf1;
   double am_vf2;
   double am_vf3;
   double active_mult_spares[NUMBER_AM_SPARES];
   char active_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct curium_ratio_rec {
   unsigned char curium_ratio_equation;
   double cr_a;
   double cr_b;
   double cr_c;
   double cr_d;
   double cr_var_a;
   double cr_var_b;
   double cr_var_c;
   double cr_var_d;
   double cr_covar_ab;
   double cr_covar_ac;
   double cr_covar_ad;
   double cr_covar_bc;
   double cr_covar_bd;
   double cr_covar_cd;
   double cr_sigma_x;
   unsigned short curium_ratio_type;
   double curium_ratio_spares[NUMBER_CR_SPARES];
   char curium_ratio_item_type[MAX_ITEM_TYPE_LENGTH];
   char curium_ratio_detector_id[MAX_DETECTOR_ID_LENGTH];
   double cr_lower_mass_limit;
   double cr_upper_mass_limit;
};

struct truncated_mult_rec {
   double tm_a;
   double tm_b;
   unsigned char tm_known_eff;
   unsigned char tm_solve_eff;
   double truncated_mult_spares[NUMBER_TM_SPARES];
   char truncated_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char truncated_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_rec {
   char meas_date[DATE_TIME_LENGTH];
   char meas_time[DATE_TIME_LENGTH];
   char filename[FILE_NAME_LENGTH];
   char original_meas_date[DATE_TIME_LENGTH];
   char results_facility[FACILITY_LENGTH];
   char results_facility_description[DESCRIPTION_LENGTH];
   char results_mba[MBA_LENGTH];
   char results_mba_description[DESCRIPTION_LENGTH];
   char item_id[MAX_ITEM_ID_LENGTH];
   char stratum_id[MAX_STRATUM_ID_LENGTH];
   char stratum_id_description[DESCRIPTION_LENGTH];
   char results_campaign_id[MAX_CAMPAIGN_ID_LENGTH];
   char results_inspection_number[MAX_CAMPAIGN_ID_LENGTH];
   char results_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char results_collar_mode;
   char results_detector_id[MAX_DETECTOR_ID_LENGTH];
   char results_detector_type[DETECTOR_TYPE_LENGTH];
   char results_electronics_id[ELECTRONICS_ID_LENGTH];
   char results_glovebox_id[MAX_GLOVEBOX_ID_LENGTH];
   unsigned short results_num_rows;
   unsigned short results_num_columns;
   double results_distance;
   double bias_uncertainty;
   double random_uncertainty;
   double systematic_uncertainty;
   double relative_std_dev;
   unsigned char completed;
   unsigned char meas_option;
   char inventory_change_code[INVENTORY_CHG_LENGTH];
   char io_code[IO_CODE_LENGTH];
   unsigned char well_config;
   unsigned char data_source;
   unsigned char results_qc_tests;
   unsigned short error_calc_method;
   unsigned char results_print;
   char user_id[CHAR_FIELD_LENGTH];
   char comment[MAX_COMMENT_LENGTH];
   char ending_comment[MAX_COMMENT_LENGTH];
   double item_pu238;
   double item_pu239;
   double item_pu240;
   double item_pu241;
   double item_pu242;
   double item_am241;
   double item_pu238_err;
   double item_pu239_err;
   double item_pu240_err;
   double item_pu241_err;
   double item_pu242_err;
   double item_am241_err;
   char item_pu_date[DATE_TIME_LENGTH];
   char item_am_date[DATE_TIME_LENGTH];
   char item_isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
   char item_isotopics_source_code[ISO_SOURCE_CODE_LENGTH];
   double normalization_constant;
   double normalization_constant_err;
   double results_predelay;
   double results_gate_length;
   double results_gate_length2;
   double results_high_voltage;
   double results_die_away_time;
   double results_efficiency;
   double results_multiplicity_deadtime;
   double results_coeff_a_deadtime;
   double results_coeff_b_deadtime;
   double results_coeff_c_deadtime;
   double results_doubles_gate_fraction;
   double results_triples_gate_fraction;
   double r_acc_sngl_test_rate_limit;
   double r_acc_sngl_test_precision_limit;
   double r_acc_sngl_test_outlier_limit;
   double r_outlier_test_limit;
   double r_bkg_doubles_rate_limit;
   double r_bkg_triples_rate_limit;
   double r_chisq_limit;
   unsigned short r_max_num_failures;
   double r_high_voltage_test_limit;
   double passive_bkg_singles_rate;
   double passive_bkg_singles_rate_err;
   double passive_bkg_doubles_rate;
   double passive_bkg_doubles_rate_err;
   double passive_bkg_triples_rate;
   double passive_bkg_triples_rate_err;
   double active_bkg_singles_rate;
   double active_bkg_singles_rate_err;
   double passive_bkg_scaler1_rate;
   double passive_bkg_scaler2_rate;
   double active_bkg_scaler1_rate;
   double active_bkg_scaler2_rate;
   char error_msg_codes[NUM_ERROR_MSG_CODES][ERR_MSG_LENGTH];
   char warning_msg_codes[NUM_WARNING_MSG_CODES][ERR_MSG_LENGTH];
   unsigned short total_number_runs;
   unsigned short number_good_runs;
   double total_good_count_time;
   double singles_sum;
   double scaler1_sum;
   double scaler2_sum;
   double reals_plus_acc_sum;
   double acc_sum;
   double mult_reals_plus_acc_sum[MULTI_ARRAY_SIZE];
   double mult_acc_sum[MULTI_ARRAY_SIZE];
   double singles;
   double singles_err;
   double doubles;
   double doubles_err;
   double triples;
   double triples_err;
   double scaler1;
   double scaler1_err;
   double scaler2;
   double scaler2_err;
   double uncorrected_doubles;
   double uncorrected_doubles_err;
   double singles_multi;
   double doubles_multi;
   double triples_multi;
   double declared_mass;
   unsigned char primary_analysis_method;
   double net_drum_weight;
   char passive_meas_date[DATE_TIME_LENGTH];
   char passive_meas_time[DATE_TIME_LENGTH];
   char passive_filename[FILE_NAME_LENGTH];
   char passive_results_detector_id[MAX_DETECTOR_ID_LENGTH];
   char active_meas_date[DATE_TIME_LENGTH];
   char active_meas_time[DATE_TIME_LENGTH];
   char active_filename[FILE_NAME_LENGTH];
   char active_results_detector_id[MAX_DETECTOR_ID_LENGTH];
   double covariance_matrix[9];
   double r_normal_backup_assay_test_lim;
   double r_max_runs_for_outlier_test;
   double r_checksum_test;
   double results_accidentals_method;
   double declared_u_mass;
   double length;
   double db_version;
   double results_spares[NUMBER_RESULTS_SPARES];
};

struct results_init_src_rec {
   char init_src_id[SOURCE_ID_LENGTH];
   char init_src_pass_fail[PASS_FAIL_LENGTH];
   unsigned short init_src_mode;
};

struct results_bias_rec {
   char bias_source_id[SOURCE_ID_LENGTH];
   char bias_pass_fail[PASS_FAIL_LENGTH];
   unsigned short results_bias_mode;
   double bias_sngls_rate_expect;
   double bias_sngls_rate_expect_err;
   double bias_sngls_rate_expect_meas;
   double bias_sngls_rate_expect_meas_err;
   double bias_dbls_rate_expect;
   double bias_dbls_rate_expect_err;
   double bias_dbls_rate_expect_meas;
   double bias_dbls_rate_expect_meas_err;
   double new_norm_constant;
   double new_norm_constant_err;
   double meas_precision;
   double required_precision;
   double required_meas_seconds;
   double bias_spares[NUMBER_BIAS_SPARES];
};

struct results_precision_rec {
   char prec_pass_fail[PASS_FAIL_LENGTH];
   double prec_sample_var;
   double prec_theoretical_var;
   double prec_chi_sq;
   double chi_sq_lower_limit;
   double chi_sq_upper_limit;
   double prec_spares[NUMBER_PREC_SPARES];
};

struct results_cal_curve_rec {
   double cc_pu240e_mass;
   double cc_pu240e_mass_err;
   double cc_pu_mass;
   double cc_pu_mass_err;
   double cc_dcl_pu240e_mass;
   double cc_dcl_pu_mass;
   double cc_dcl_minus_asy_pu_mass;
   double cc_dcl_minus_asy_pu_mass_err;
   double cc_dcl_minus_asy_pu_mass_pct;
   char cc_pass_fail[PASS_FAIL_LENGTH];
   double cc_dcl_u_mass;
   double cc_length;
   double cc_heavy_metal_content;
   double cc_heavy_metal_correction;
   double cc_heavy_metal_corr_singles;
   double cc_heavy_metal_corr_singles_err;
   double cc_heavy_metal_corr_doubles;
   double cc_heavy_metal_corr_doubles_err;
   double cc_spares[NUMBER_CC_RESULTS_SPARES];
   unsigned char cc_cal_curve_equation;
   double cc_a_res;
   double cc_b_res;
   double cc_c_res;
   double cc_d_res;
   double cc_var_a_res;
   double cc_var_b_res;
   double cc_var_c_res;
   double cc_var_d_res;
   double cc_covar_ab_res;
   double cc_covar_ac_res;
   double cc_covar_ad_res;
   double cc_covar_bc_res;
   double cc_covar_bd_res;
   double cc_covar_cd_res;
   double cc_sigma_x_res;
   double cc_cal_curve_type_res;
   double cc_heavy_metal_corr_factor_res;
   double cc_heavy_metal_reference_res;
   double cc_percent_u235_res;
   double cc_spares_res[NUMBER_CC_SPARES];
   char cc_cal_curve_item_type[MAX_ITEM_TYPE_LENGTH];
   char cc_cal_curve_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_known_alpha_rec {
   double ka_mult;
   double ka_alpha;
   double ka_mult_corr_doubles;
   double ka_mult_corr_doubles_err;
   double ka_pu240e_mass;
   double ka_pu240e_mass_err;
   double ka_pu_mass;
   double ka_pu_mass_err;
   double ka_dcl_pu240e_mass;
   double ka_dcl_pu_mass;
   double ka_dcl_minus_asy_pu_mass;
   double ka_dcl_minus_asy_pu_mass_err;
   double ka_dcl_minus_asy_pu_mass_pct;
   char ka_pass_fail[PASS_FAIL_LENGTH];
   double ka_dcl_u_mass;
   double ka_length;
   double ka_heavy_metal_content;
   double ka_heavy_metal_correction;
   double ka_corr_singles;
   double ka_corr_singles_err;
   double ka_corr_doubles;
   double ka_corr_doubles_err;
   double ka_corr_factor;
   double ka_dry_alpha_or_mult_dbls;
   double ka_alpha_wt_res;
   double ka_rho_zero_res;
   double ka_k_res;
   double ka_a_res;
   double ka_b_res;
   double ka_var_a_res;
   double ka_var_b_res;
   double ka_covar_ab_res;
   double ka_sigma_x_res;
   double ka_known_alpha_type_res;
   double ka_heavy_metal_corr_factor_res;
   double ka_heavy_metal_reference_res;
   double ka_ring_ratio_equation_res;
   double ka_ring_ratio_a_res;
   double ka_ring_ratio_b_res;
   double ka_ring_ratio_c_res;
   double ka_ring_ratio_d_res;
   double ka_lower_corr_factor_limit_res;
   double ka_upper_corr_factor_limit_res;
   char ka_known_alpha_item_type[MAX_ITEM_TYPE_LENGTH];
   char ka_known_alpha_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_known_m_rec {
   double km_mult;
   double km_alpha;
   double km_pu239e_mass;
   double km_pu240e_mass;
   double km_pu240e_mass_err;
   double km_pu_mass;
   double km_pu_mass_err;
   double km_dcl_pu240e_mass;
   double km_dcl_pu_mass;
   double km_dcl_minus_asy_pu_mass;
   double km_dcl_minus_asy_pu_mass_err;
   double km_dcl_minus_asy_pu_mass_pct;
   char km_pass_fail[PASS_FAIL_LENGTH];
   double km_spares[NUMBER_KM_RESULTS_SPARES];
   double km_sf_rate_res;
   double km_vs1_res;
   double km_vs2_res;
   double km_vi1_res;
   double km_vi2_res;
   double km_b_res;
   double km_c_res;
   double km_sigma_x_res;
   double km_spares_res[NUMBER_KM_SPARES];
   char km_known_m_item_type[MAX_ITEM_TYPE_LENGTH];
   char km_known_m_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_multiplicity_rec {
   double mul_mult;
   double mul_mult_err;
   double mul_alpha;
   double mul_alpha_err;
   double mul_corr_factor;
   double mul_corr_factor_err;
   double mul_efficiency;
   double mul_efficiency_err;
   double mul_pu240e_mass;
   double mul_pu240e_mass_err;
   double mul_pu_mass;
   double mul_pu_mass_err;
   double mul_dcl_pu240e_mass;
   double mul_dcl_pu_mass;
   double mul_dcl_minus_asy_pu_mass;
   double mul_dcl_minus_asy_pu_mass_err;
   double mul_dcl_minus_asy_pu_mass_pct;
   char mul_pass_fail[PASS_FAIL_LENGTH];
   double mul_spares[NUMBER_MUL_RESULTS_SPARES];
   unsigned char mul_solve_efficiency_res;
   double mul_sf_rate_res;
   double mul_vs1_res;
   double mul_vs2_res;
   double mul_vs3_res;
   double mul_vi1_res;
   double mul_vi2_res;
   double mul_vi3_res;
   double mul_a_res;
   double mul_b_res;
   double mul_c_res;
   double mul_sigma_x_res;
   double mul_alpha_weight_res;
   double mul_spares_res[NUMBER_MUL_SPARES];
   char mul_multiplicity_item_type[MAX_ITEM_TYPE_LENGTH];
   char mul_multiplicity_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_active_passive_rec {
   double ap_delta_doubles;
   double ap_delta_doubles_err;
   double ap_u235_mass;
   double ap_u235_mass_err;
   double ap_k0;
   double ap_k1;
   double ap_k1_err;
   double ap_k;
   double ap_k_err;
   double ap_dcl_u235_mass;
   double ap_dcl_minus_asy_u235_mass;
   double ap_dcl_minus_asy_u235_mass_err;
   double ap_dcl_minus_asy_u235_mass_pct;
   char ap_pass_fail[PASS_FAIL_LENGTH];
   double ap_spares[NUMBER_AP_RESULTS_SPARES];
   unsigned char ap_active_passive_equation;
   double ap_a_res;
   double ap_b_res;
   double ap_c_res;
   double ap_d_res;
   double ap_var_a_res;
   double ap_var_b_res;
   double ap_var_c_res;
   double ap_var_d_res;
   double ap_covar_ab_res;
   double ap_covar_ac_res;
   double ap_covar_ad_res;
   double ap_covar_bc_res;
   double ap_covar_bd_res;
   double ap_covar_cd_res;
   double ap_sigma_x_res;
   double ap_spares_res[NUMBER_AP_SPARES];
   char ap_active_passive_item_type[MAX_ITEM_TYPE_LENGTH];
   char ap_active_passive_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_active_rec {
   double act_u235_mass;
   double act_u235_mass_err;
   double act_k0;
   double act_k1;
   double act_k1_err;
   double act_k;
   double act_k_err;
   double act_dcl_u235_mass;
   double act_dcl_minus_asy_u235_mass;
   double act_dcl_minus_asy_u235_mass_err;
   double act_dcl_minus_asy_u235_mass_pct;
   char act_pass_fail[PASS_FAIL_LENGTH];
   double act_spares[NUMBER_ACT_RESULTS_SPARES];
   unsigned char act_active_equation;
   double act_a_res;
   double act_b_res;
   double act_c_res;
   double act_d_res;
   double act_var_a_res;
   double act_var_b_res;
   double act_var_c_res;
   double act_var_d_res;
   double act_covar_ab_res;
   double act_covar_ac_res;
   double act_covar_ad_res;
   double act_covar_bc_res;
   double act_covar_bd_res;
   double act_covar_cd_res;
   double act_sigma_x_res;
   double act_spares_res[NUMBER_ACT_SPARES];
   char act_active_item_type[MAX_ITEM_TYPE_LENGTH];
   char act_active_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_collar_rec {
   double col_u235_mass;
   double col_u235_mass_err;
   double col_percent_u235;
   double col_total_u_mass;
   double col_k0;
   double col_k0_err;
   double col_k1;
   double col_k1_err;
   double col_k2;
   double col_k2_err;
   double col_k3;
   double col_k3_err;
   double col_k4;
   double col_k4_err;
   double col_k5;
   double col_k5_err;
   double col_total_corr_fact;
   double col_total_corr_fact_err;
   char col_source_id[SOURCE_ID_LENGTH];
   double col_corr_doubles;
   double col_corr_doubles_err;
   double col_dcl_length;
   double col_dcl_length_err;
   double col_dcl_total_u235;
   double col_dcl_total_u235_err;
   double col_dcl_total_u238;
   double col_dcl_total_u238_err;
   double col_dcl_total_rods;
   double col_dcl_total_poison_rods;
   double col_dcl_poison_percent;
   double col_dcl_poison_percent_err;
   double col_dcl_minus_asy_u235_mass;
   double col_dcl_minus_asy_u235_mass_err;
   double col_dcl_minus_asy_u235_mass_pct;
   char col_pass_fail[PASS_FAIL_LENGTH];
   double col_spares[NUMBER_COL_RESULTS_SPARES];
   char col_collar_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char col_collar_mode;
   char col_collar_detector_id[MAX_DETECTOR_ID_LENGTH];
   unsigned char col_collar_equation;
   double col_a_res;
   double col_b_res;
   double col_c_res;
   double col_d_res;
   double col_var_a_res;
   double col_var_b_res;
   double col_var_c_res;
   double col_var_d_res;
   double col_covar_ab_res;
   double col_covar_ac_res;
   double col_covar_ad_res;
   double col_covar_bc_res;
   double col_covar_bd_res;
   double col_covar_cd_res;
   double col_sigma_x_res;
   double col_number_calib_rods_res;
   char col_poison_rod_type_res[MAX_ROD_TYPE_LENGTH];
   double col_poison_absorption_fact_res;
   double col_poison_rod_a_res;
   double col_poison_rod_a_err_res;
   double col_poison_rod_b_res;
   double col_poison_rod_b_err_res;
   double col_poison_rod_c_res;
   double col_poison_rod_c_err_res;
   double col_u_mass_corr_fact_a_res;
   double col_u_mass_corr_fact_a_err_res;
   double col_u_mass_corr_fact_b_res;
   double col_u_mass_corr_fact_b_err_res;
   double col_sample_corr_fact_res;
   double col_sample_corr_fact_err_res;
   char col_reference_date_res[DATE_TIME_LENGTH];
   double col_relative_doubles_rate_res;
   double col_spares_res[NUMBER_COL_SPARES];
   char collar_k5_label_res[MAX_COLLAR_K5_PARAMETERS][MAX_K5_LABEL_LENGTH];
   unsigned char collar_k5_checkbox_res[MAX_COLLAR_K5_PARAMETERS];
   double collar_k5_res[MAX_COLLAR_K5_PARAMETERS];
   double collar_k5_err_res[MAX_COLLAR_K5_PARAMETERS];
};

struct results_add_a_source_rec {
   double ad_dzero_cf252_doubles;
   double ad_sample_cf252_doubles[MAX_ADDASRC_POSITIONS];
   double ad_sample_cf252_doubles_err[MAX_ADDASRC_POSITIONS];
   double ad_sample_cf252_ratio[MAX_ADDASRC_POSITIONS];
   double ad_sample_avg_cf252_doubles;
   double ad_sample_avg_cf252_doubles_err;
   double ad_corr_doubles;
   double ad_corr_doubles_err;
   double ad_delta;
   double ad_delta_err;
   double ad_corr_factor;
   double ad_corr_factor_err;
   double ad_pu240e_mass;
   double ad_pu240e_mass_err;
   double ad_pu_mass;
   double ad_pu_mass_err;
   double ad_dcl_pu240e_mass;
   double ad_dcl_pu_mass;
   double ad_dcl_minus_asy_pu_mass;
   double ad_dcl_minus_asy_pu_mass_err;
   double ad_dcl_minus_asy_pu_mass_pct;
   char ad_pass_fail[PASS_FAIL_LENGTH];
   double ad_tm_doubles_bkg;
   double ad_tm_doubles_bkg_err;
   double ad_tm_uncorr_doubles;
   double ad_tm_uncorr_doubles_err;
   double ad_tm_corr_doubles;
   double ad_tm_corr_doubles_err;
   double ad_spares[NUMBER_AD_RESULTS_SPARES];
   unsigned char ad_add_a_source_equation;
   double ad_a_res;
   double ad_b_res;
   double ad_c_res;
   double ad_d_res;
   double ad_var_a_res;
   double ad_var_b_res;
   double ad_var_c_res;
   double ad_var_d_res;
   double ad_covar_ab_res;
   double ad_covar_ac_res;
   double ad_covar_ad_res;
   double ad_covar_bc_res;
   double ad_covar_bd_res;
   double ad_covar_cd_res;
   double ad_sigma_x_res;
   double ad_spares_res[NUMBER_AD_SPARES];
   double ad_position_dzero_res[MAX_ADDASRC_POSITIONS];
   double ad_dzero_avg_res;
   char ad_dzero_ref_date_res[DATE_TIME_LENGTH];
   unsigned short ad_num_runs_res;
   double ad_cf_a_res;
   double ad_cf_b_res;
   double ad_cf_c_res;
   double ad_cf_d_res;
   double ad_use_truncated_mult_res;
   double ad_tm_weighting_factor_res;
   double ad_tm_dbls_rate_upper_limit_res;
   double ad_cf_spares_res[NUMBER_AD_CF_SPARES];
   char ad_add_a_source_item_type[MAX_ITEM_TYPE_LENGTH];
   char ad_add_a_source_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_active_mult_rec {
   double am_mult;
   double am_mult_err;
   double am_spares[NUMBER_AM_RESULTS_SPARES];
   double am_vt1_res;
   double am_vt2_res;
   double am_vt3_res;
   double am_vf1_res;
   double am_vf2_res;
   double am_vf3_res;
   double am_spares_res[NUMBER_AM_SPARES];
   char am_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char am_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_curium_ratio_rec {
   double cr_pu240e_mass;
   double cr_pu240e_mass_err;
   double cr_cm_mass;
   double cr_cm_mass_err;
   double cr_pu_mass;
   double cr_pu_mass_err;
   double cr_u_mass;
   double cr_u_mass_err;
   double cr_u235_mass;
   double cr_u235_mass_err;
   double cr_dcl_pu_mass;
   double cr_dcl_minus_asy_pu_mass;
   double cr_dcl_minus_asy_pu_mass_err;
   double cr_dcl_minus_asy_pu_mass_pct;
   double cr_dcl_minus_asy_u_mass;
   double cr_dcl_minus_asy_u_mass_err;
   double cr_dcl_minus_asy_u_mass_pct;
   double cr_dcl_minus_asy_u235_mass;
   double cr_dcl_minus_asy_u235_mass_err;
   double cr_dcl_minus_asy_u235_mass_pct;
   char cr_pu_pass_fail[PASS_FAIL_LENGTH];
   char cr_u_pass_fail[PASS_FAIL_LENGTH];
   double cr_cm_pu_ratio;
   double cr_cm_pu_ratio_err;
   double cr_pu_half_life;
   char cr_cm_pu_ratio_date[DATE_TIME_LENGTH];
   double cr_cm_u_ratio;
   double cr_cm_u_ratio_err;
   char cr_cm_u_ratio_date[DATE_TIME_LENGTH];
   char cr_cm_id_label[MAX_ITEM_ID_LENGTH];
   char cr_cm_id[MAX_ITEM_ID_LENGTH];
   char cr_cm_input_batch_id[MAX_ITEM_ID_LENGTH];
   double cr_dcl_u_mass_res;
   double cr_dcl_u235_mass_res;
   double cr_cm_pu_ratio_decay_corr;
   double cr_cm_pu_ratio_decay_corr_err;
   double cr_cm_u_ratio_decay_corr;
   double cr_cm_u_ratio_decay_corr_err;
   double cr_spares[NUMBER_CR_RESULTS_SPARES];
   unsigned char cr_curium_ratio_equation;
   double cr_a_res;
   double cr_b_res;
   double cr_c_res;
   double cr_d_res;
   double cr_var_a_res;
   double cr_var_b_res;
   double cr_var_c_res;
   double cr_var_d_res;
   double cr_covar_ab_res;
   double cr_covar_ac_res;
   double cr_covar_ad_res;
   double cr_covar_bc_res;
   double cr_covar_bd_res;
   double cr_covar_cd_res;
   double cr_sigma_x_res;
   unsigned short curium_ratio_type_res;
   double cr_spares_res[NUMBER_CR_SPARES];
   char cr_curium_ratio_item_type[MAX_ITEM_TYPE_LENGTH];
   char cr_curium_ratio_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_truncated_mult_rec {
   double tm_bkg_singles;
   double tm_bkg_singles_err;
   double tm_bkg_zeros;
   double tm_bkg_zeros_err;
   double tm_bkg_ones;
   double tm_bkg_ones_err;
   double tm_bkg_twos;
   double tm_bkg_twos_err;
   double tm_net_singles;
   double tm_net_singles_err;
   double tm_net_zeros;
   double tm_net_zeros_err;
   double tm_net_ones;
   double tm_net_ones_err;
   double tm_net_twos;
   double tm_net_twos_err;
   double tm_k_alpha;
   double tm_k_alpha_err;
   double tm_k_pu240e_mass;
   double tm_k_pu240e_mass_err;
   double tm_k_pu_mass;
   double tm_k_pu_mass_err;
   double tm_k_dcl_pu240e_mass;
   double tm_k_dcl_pu_mass;
   double tm_k_dcl_minus_asy_pu_mass;
   double tm_k_dcl_minus_asy_pu_mass_err;
   double tm_k_dcl_minus_asy_pu_mass_pct;
   char tm_k_pass_fail[PASS_FAIL_LENGTH];
   double tm_s_eff;
   double tm_s_eff_err;
   double tm_s_alpha;
   double tm_s_alpha_err;
   double tm_s_pu240e_mass;
   double tm_s_pu240e_mass_err;
   double tm_s_pu_mass;
   double tm_s_pu_mass_err;
   double tm_s_dcl_pu240e_mass;
   double tm_s_dcl_pu_mass;
   double tm_s_dcl_minus_asy_pu_mass;
   double tm_s_dcl_minus_asy_pu_mass_err;
   double tm_s_dcl_minus_asy_pu_mass_pct;
   char tm_s_pass_fail[PASS_FAIL_LENGTH];
   double tm_spares[NUMBER_TM_RESULTS_SPARES];
   double tm_a_res;
   double tm_b_res;
   unsigned char tm_known_eff_res;
   unsigned char tm_solve_eff_res;
   double tm_spares_res[NUMBER_TM_SPARES];
   char tm_truncated_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char tm_truncated_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_tm_bkg_rec {
   double results_tm_singles_bkg;
   double results_tm_singles_bkg_err;
   double results_tm_zeros_bkg;
   double results_tm_zeros_bkg_err;
   double results_tm_ones_bkg;
   double results_tm_ones_bkg_err;
   double results_tm_twos_bkg;
   double results_tm_twos_bkg_err;
};

struct run_rec {
   unsigned short run_number;
   char run_date[DATE_TIME_LENGTH];
   char run_time[DATE_TIME_LENGTH];
   char run_tests[MAX_RUN_TESTS_LENGTH];
   double run_count_time;
   double run_singles;
   double run_scaler1;
   double run_scaler2;
   double run_reals_plus_acc;
   double run_acc;
   double run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double run_mult_acc[MULTI_ARRAY_SIZE];
   double run_singles_rate;
   double run_doubles_rate;
   double run_triples_rate;
   double run_scaler1_rate;
   double run_scaler2_rate;
   double run_multiplicity_mult;
   double run_multiplicity_alpha;
   double run_multiplicity_efficiency;
   double run_mass;
   double run_high_voltage;
   double run_spare[NUMBER_RUN_SPARES];
};

struct cf1_run_rec {
   unsigned short cf1_run_number;
   char cf1_run_date[DATE_TIME_LENGTH];
   char cf1_run_time[DATE_TIME_LENGTH];
   char cf1_run_tests[MAX_RUN_TESTS_LENGTH];
   double cf1_run_count_time;
   double cf1_run_singles;
   double cf1_run_scaler1;
   double cf1_run_scaler2;
   double cf1_run_reals_plus_acc;
   double cf1_run_acc;
   double cf1_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double cf1_run_mult_acc[MULTI_ARRAY_SIZE];
   double cf1_run_singles_rate;
   double cf1_run_doubles_rate;
   double cf1_run_triples_rate;
   double cf1_run_scaler1_rate;
   double cf1_run_scaler2_rate;
   double cf1_run_multiplicity_mult;
   double cf1_run_multiplicity_alpha;
   double cf1_multiplicity_efficiency;
   double cf1_run_mass;
   double cf1_high_voltage;
   double cf1_spare[NUMBER_RUN_SPARES];
};

struct cf2_run_rec {
   unsigned short cf2_run_number;
   char cf2_run_date[DATE_TIME_LENGTH];
   char cf2_run_time[DATE_TIME_LENGTH];
   char cf2_run_tests[MAX_RUN_TESTS_LENGTH];
   double cf2_run_count_time;
   double cf2_run_singles;
   double cf2_run_scaler1;
   double cf2_run_scaler2;
   double cf2_run_reals_plus_acc;
   double cf2_run_acc;
   double cf2_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double cf2_run_mult_acc[MULTI_ARRAY_SIZE];
   double cf2_run_singles_rate;
   double cf2_run_doubles_rate;
   double cf2_run_triples_rate;
   double cf2_run_scaler1_rate;
   double cf2_run_scaler2_rate;
   double cf2_run_multiplicity_mult;
   double cf2_run_multiplicity_alpha;
   double cf2_multiplicity_efficiency;
   double cf2_run_mass;
   double cf2_high_voltage;
   double cf2_spare[NUMBER_RUN_SPARES];
};

struct cf3_run_rec {
   unsigned short cf3_run_number;
   char cf3_run_date[DATE_TIME_LENGTH];
   char cf3_run_time[DATE_TIME_LENGTH];
   char cf3_run_tests[MAX_RUN_TESTS_LENGTH];
   double cf3_run_count_time;
   double cf3_run_singles;
   double cf3_run_scaler1;
   double cf3_run_scaler2;
   double cf3_run_reals_plus_acc;
   double cf3_run_acc;
   double cf3_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double cf3_run_mult_acc[MULTI_ARRAY_SIZE];
   double cf3_run_singles_rate;
   double cf3_run_doubles_rate;
   double cf3_run_triples_rate;
   double cf3_run_scaler1_rate;
   double cf3_run_scaler2_rate;
   double cf3_run_multiplicity_mult;
   double cf3_run_multiplicity_alpha;
   double cf3_multiplicity_efficiency;
   double cf3_run_mass;
   double cf3_high_voltage;
   double cf3_spare[NUMBER_RUN_SPARES];
};

struct cf4_run_rec {
   unsigned short cf4_run_number;
   char cf4_run_date[DATE_TIME_LENGTH];
   char cf4_run_time[DATE_TIME_LENGTH];
   char cf4_run_tests[MAX_RUN_TESTS_LENGTH];
   double cf4_run_count_time;
   double cf4_run_singles;
   double cf4_run_scaler1;
   double cf4_run_scaler2;
   double cf4_run_reals_plus_acc;
   double cf4_run_acc;
   double cf4_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double cf4_run_mult_acc[MULTI_ARRAY_SIZE];
   double cf4_run_singles_rate;
   double cf4_run_doubles_rate;
   double cf4_run_triples_rate;
   double cf4_run_scaler1_rate;
   double cf4_run_scaler2_rate;
   double cf4_run_multiplicity_mult;
   double cf4_run_multiplicity_alpha;
   double cf4_multiplicity_efficiency;
   double cf4_run_mass;
   double cf4_high_voltage;
   double cf4_spare[NUMBER_RUN_SPARES];
};

struct cf5_run_rec {
   unsigned short cf5_run_number;
   char cf5_run_date[DATE_TIME_LENGTH];
   char cf5_run_time[DATE_TIME_LENGTH];
   char cf5_run_tests[MAX_RUN_TESTS_LENGTH];
   double cf5_run_count_time;
   double cf5_run_singles;
   double cf5_run_scaler1;
   double cf5_run_scaler2;
   double cf5_run_reals_plus_acc;
   double cf5_run_acc;
   double cf5_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double cf5_run_mult_acc[MULTI_ARRAY_SIZE];
   double cf5_run_singles_rate;
   double cf5_run_doubles_rate;
   double cf5_run_triples_rate;
   double cf5_run_scaler1_rate;
   double cf5_run_scaler2_rate;
   double cf5_run_multiplicity_mult;
   double cf5_run_multiplicity_alpha;
   double cf5_multiplicity_efficiency;
   double cf5_run_mass;
   double cf5_high_voltage;
   double cf5_spare[NUMBER_RUN_SPARES];
};

struct a1_run_rec {
   unsigned short a1_run_number;
   char a1_run_date[DATE_TIME_LENGTH];
   char a1_run_time[DATE_TIME_LENGTH];
   char a1_run_tests[MAX_RUN_TESTS_LENGTH];
   double a1_run_count_time;
   double a1_run_singles;
   double a1_run_scaler1;
   double a1_run_scaler2;
   double a1_run_reals_plus_acc;
   double a1_run_acc;
   double a1_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a1_run_mult_acc[MULTI_ARRAY_SIZE];
   double a1_run_singles_rate;
   double a1_run_doubles_rate;
   double a1_run_triples_rate;
   double a1_run_scaler1_rate;
   double a1_run_scaler2_rate;
   double a1_run_multiplicity_mult;
   double a1_run_multiplicity_alpha;
   double a1_multiplicity_efficiency;
   double a1_run_mass;
   double a1_high_voltage;
   double a1_spare[NUMBER_RUN_SPARES];
};

struct a2_run_rec {
   unsigned short a2_run_number;
   char a2_run_date[DATE_TIME_LENGTH];
   char a2_run_time[DATE_TIME_LENGTH];
   char a2_run_tests[MAX_RUN_TESTS_LENGTH];
   double a2_run_count_time;
   double a2_run_singles;
   double a2_run_scaler1;
   double a2_run_scaler2;
   double a2_run_reals_plus_acc;
   double a2_run_acc;
   double a2_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a2_run_mult_acc[MULTI_ARRAY_SIZE];
   double a2_run_singles_rate;
   double a2_run_doubles_rate;
   double a2_run_triples_rate;
   double a2_run_scaler1_rate;
   double a2_run_scaler2_rate;
   double a2_run_multiplicity_mult;
   double a2_run_multiplicity_alpha;
   double a2_multiplicity_efficiency;
   double a2_run_mass;
   double a2_high_voltage;
   double a2_spare[NUMBER_RUN_SPARES];
};

struct a3_run_rec {
   unsigned short a3_run_number;
   char a3_run_date[DATE_TIME_LENGTH];
   char a3_run_time[DATE_TIME_LENGTH];
   char a3_run_tests[MAX_RUN_TESTS_LENGTH];
   double a3_run_count_time;
   double a3_run_singles;
   double a3_run_scaler1;
   double a3_run_scaler2;
   double a3_run_reals_plus_acc;
   double a3_run_acc;
   double a3_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a3_run_mult_acc[MULTI_ARRAY_SIZE];
   double a3_run_singles_rate;
   double a3_run_doubles_rate;
   double a3_run_triples_rate;
   double a3_run_scaler1_rate;
   double a3_run_scaler2_rate;
   double a3_run_multiplicity_mult;
   double a3_run_multiplicity_alpha;
   double a3_multiplicity_efficiency;
   double a3_run_mass;
   double a3_high_voltage;
   double a3_spare[NUMBER_RUN_SPARES];
};

struct a4_run_rec {
   unsigned short a4_run_number;
   char a4_run_date[DATE_TIME_LENGTH];
   char a4_run_time[DATE_TIME_LENGTH];
   char a4_run_tests[MAX_RUN_TESTS_LENGTH];
   double a4_run_count_time;
   double a4_run_singles;
   double a4_run_scaler1;
   double a4_run_scaler2;
   double a4_run_reals_plus_acc;
   double a4_run_acc;
   double a4_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a4_run_mult_acc[MULTI_ARRAY_SIZE];
   double a4_run_singles_rate;
   double a4_run_doubles_rate;
   double a4_run_triples_rate;
   double a4_run_scaler1_rate;
   double a4_run_scaler2_rate;
   double a4_run_multiplicity_mult;
   double a4_run_multiplicity_alpha;
   double a4_multiplicity_efficiency;
   double a4_run_mass;
   double a4_high_voltage;
   double a4_spare[NUMBER_RUN_SPARES];
};

struct a5_run_rec {
   unsigned short a5_run_number;
   char a5_run_date[DATE_TIME_LENGTH];
   char a5_run_time[DATE_TIME_LENGTH];
   char a5_run_tests[MAX_RUN_TESTS_LENGTH];
   double a5_run_count_time;
   double a5_run_singles;
   double a5_run_scaler1;
   double a5_run_scaler2;
   double a5_run_reals_plus_acc;
   double a5_run_acc;
   double a5_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a5_run_mult_acc[MULTI_ARRAY_SIZE];
   double a5_run_singles_rate;
   double a5_run_doubles_rate;
   double a5_run_triples_rate;
   double a5_run_scaler1_rate;
   double a5_run_scaler2_rate;
   double a5_run_multiplicity_mult;
   double a5_run_multiplicity_alpha;
   double a5_multiplicity_efficiency;
   double a5_run_mass;
   double a5_high_voltage;
   double a5_spare[NUMBER_RUN_SPARES];
};

struct a6_run_rec {
   unsigned short a6_run_number;
   char a6_run_date[DATE_TIME_LENGTH];
   char a6_run_time[DATE_TIME_LENGTH];
   char a6_run_tests[MAX_RUN_TESTS_LENGTH];
   double a6_run_count_time;
   double a6_run_singles;
   double a6_run_scaler1;
   double a6_run_scaler2;
   double a6_run_reals_plus_acc;
   double a6_run_acc;
   double a6_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double a6_run_mult_acc[MULTI_ARRAY_SIZE];
   double a6_run_singles_rate;
   double a6_run_doubles_rate;
   double a6_run_triples_rate;
   double a6_run_scaler1_rate;
   double a6_run_scaler2_rate;
   double a6_run_multiplicity_mult;
   double a6_run_multiplicity_alpha;
   double a6_multiplicity_efficiency;
   double a6_run_mass;
   double a6_high_voltage;
   double a6_spare[NUMBER_RUN_SPARES];
};

struct b1_run_rec {
   unsigned short b1_run_number;
   char b1_run_date[DATE_TIME_LENGTH];
   char b1_run_time[DATE_TIME_LENGTH];
   char b1_run_tests[MAX_RUN_TESTS_LENGTH];
   double b1_run_count_time;
   double b1_run_singles;
   double b1_run_scaler1;
   double b1_run_scaler2;
   double b1_run_reals_plus_acc;
   double b1_run_acc;
   double b1_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b1_run_mult_acc[MULTI_ARRAY_SIZE];
   double b1_run_singles_rate;
   double b1_run_doubles_rate;
   double b1_run_triples_rate;
   double b1_run_scaler1_rate;
   double b1_run_scaler2_rate;
   double b1_run_multiplicity_mult;
   double b1_run_multiplicity_alpha;
   double b1_multiplicity_efficiency;
   double b1_run_mass;
   double b1_high_voltage;
   double b1_spare[NUMBER_RUN_SPARES];
};

struct b2_run_rec {
   unsigned short b2_run_number;
   char b2_run_date[DATE_TIME_LENGTH];
   char b2_run_time[DATE_TIME_LENGTH];
   char b2_run_tests[MAX_RUN_TESTS_LENGTH];
   double b2_run_count_time;
   double b2_run_singles;
   double b2_run_scaler1;
   double b2_run_scaler2;
   double b2_run_reals_plus_acc;
   double b2_run_acc;
   double b2_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b2_run_mult_acc[MULTI_ARRAY_SIZE];
   double b2_run_singles_rate;
   double b2_run_doubles_rate;
   double b2_run_triples_rate;
   double b2_run_scaler1_rate;
   double b2_run_scaler2_rate;
   double b2_run_multiplicity_mult;
   double b2_run_multiplicity_alpha;
   double b2_multiplicity_efficiency;
   double b2_run_mass;
   double b2_high_voltage;
   double b2_spare[NUMBER_RUN_SPARES];
};

struct b3_run_rec {
   unsigned short b3_run_number;
   char b3_run_date[DATE_TIME_LENGTH];
   char b3_run_time[DATE_TIME_LENGTH];
   char b3_run_tests[MAX_RUN_TESTS_LENGTH];
   double b3_run_count_time;
   double b3_run_singles;
   double b3_run_scaler1;
   double b3_run_scaler2;
   double b3_run_reals_plus_acc;
   double b3_run_acc;
   double b3_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b3_run_mult_acc[MULTI_ARRAY_SIZE];
   double b3_run_singles_rate;
   double b3_run_doubles_rate;
   double b3_run_triples_rate;
   double b3_run_scaler1_rate;
   double b3_run_scaler2_rate;
   double b3_run_multiplicity_mult;
   double b3_run_multiplicity_alpha;
   double b3_multiplicity_efficiency;
   double b3_run_mass;
   double b3_high_voltage;
   double b3_spare[NUMBER_RUN_SPARES];
};

struct b4_run_rec {
   unsigned short b4_run_number;
   char b4_run_date[DATE_TIME_LENGTH];
   char b4_run_time[DATE_TIME_LENGTH];
   char b4_run_tests[MAX_RUN_TESTS_LENGTH];
   double b4_run_count_time;
   double b4_run_singles;
   double b4_run_scaler1;
   double b4_run_scaler2;
   double b4_run_reals_plus_acc;
   double b4_run_acc;
   double b4_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b4_run_mult_acc[MULTI_ARRAY_SIZE];
   double b4_run_singles_rate;
   double b4_run_doubles_rate;
   double b4_run_triples_rate;
   double b4_run_scaler1_rate;
   double b4_run_scaler2_rate;
   double b4_run_multiplicity_mult;
   double b4_run_multiplicity_alpha;
   double b4_multiplicity_efficiency;
   double b4_run_mass;
   double b4_high_voltage;
   double b4_spare[NUMBER_RUN_SPARES];
};

struct b5_run_rec {
   unsigned short b5_run_number;
   char b5_run_date[DATE_TIME_LENGTH];
   char b5_run_time[DATE_TIME_LENGTH];
   char b5_run_tests[MAX_RUN_TESTS_LENGTH];
   double b5_run_count_time;
   double b5_run_singles;
   double b5_run_scaler1;
   double b5_run_scaler2;
   double b5_run_reals_plus_acc;
   double b5_run_acc;
   double b5_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b5_run_mult_acc[MULTI_ARRAY_SIZE];
   double b5_run_singles_rate;
   double b5_run_doubles_rate;
   double b5_run_triples_rate;
   double b5_run_scaler1_rate;
   double b5_run_scaler2_rate;
   double b5_run_multiplicity_mult;
   double b5_run_multiplicity_alpha;
   double b5_multiplicity_efficiency;
   double b5_run_mass;
   double b5_high_voltage;
   double b5_spare[NUMBER_RUN_SPARES];
};

struct b6_run_rec {
   unsigned short b6_run_number;
   char b6_run_date[DATE_TIME_LENGTH];
   char b6_run_time[DATE_TIME_LENGTH];
   char b6_run_tests[MAX_RUN_TESTS_LENGTH];
   double b6_run_count_time;
   double b6_run_singles;
   double b6_run_scaler1;
   double b6_run_scaler2;
   double b6_run_reals_plus_acc;
   double b6_run_acc;
   double b6_run_mult_reals_plus_acc[MULTI_ARRAY_SIZE];
   double b6_run_mult_acc[MULTI_ARRAY_SIZE];
   double b6_run_singles_rate;
   double b6_run_doubles_rate;
   double b6_run_triples_rate;
   double b6_run_scaler1_rate;
   double b6_run_scaler2_rate;
   double b6_run_multiplicity_mult;
   double b6_run_multiplicity_alpha;
   double b6_multiplicity_efficiency;
   double b6_run_mass;
   double b6_high_voltage;
   double b6_spare[NUMBER_RUN_SPARES];
};

struct add_a_source_setup_rec {
   unsigned short ad_type;
   short ad_port_number;
   double ad_forward_over_travel;
   double ad_reverse_over_travel;
   unsigned short ad_number_positions;
   double ad_dist_to_move[MAX_ADDASRC_POSITIONS];
   double cm_steps_per_inch;
   unsigned long cm_forward_mask;
   unsigned long cm_reverse_mask;
   short cm_axis_number;
   unsigned long cm_over_travel_state;
   double cm_step_ratio;
   double cm_slow_inches;
   double plc_steps_per_inch;
   double scale_conversion_factor;
   unsigned char cm_rotation;
};

struct stratum_id_rec {
   char stratum[MAX_STRATUM_ID_LENGTH];
   char stratum_id_detector_id[MAX_DETECTOR_ID_LENGTH];
   double stratum_bias_uncertainty;
   double stratum_random_uncertainty;
   double stratum_systematic_uncertainty;
};

struct de_mult_rec {
   double de_neutron_energy[MAX_DUAL_ENERGY_ROWS];
   double de_detector_efficiency[MAX_DUAL_ENERGY_ROWS];
   double de_inner_outer_ring_ratio[MAX_DUAL_ENERGY_ROWS];
   double de_relative_fission[MAX_DUAL_ENERGY_ROWS];
   double de_inner_ring_efficiency;
   double de_outer_ring_efficiency;
   char de_item_type[MAX_ITEM_TYPE_LENGTH];
   char de_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct results_de_mult_rec {
   double de_meas_ring_ratio;
   double de_interpolated_neutron_energy;
   double de_energy_corr_factor;
   double de_neutron_energy_res[MAX_DUAL_ENERGY_ROWS];
   double de_detector_efficiency_res[MAX_DUAL_ENERGY_ROWS];
   double de_inner_outer_ring_ratio_res[MAX_DUAL_ENERGY_ROWS];
   double de_relative_fission_res[MAX_DUAL_ENERGY_ROWS];
   double de_inner_ring_efficiency_res;
   double de_outer_ring_efficiency_res;
   char de_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char de_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method {
   char item_type[MAX_ITEM_TYPE_LENGTH];
   char analysis_method_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_cal_curve {
   char cal_curve_item_type[MAX_ITEM_TYPE_LENGTH];
   char cal_curve_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_active {
   char active_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_collar {
   char collar_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char collar_mode;
};

struct analysis_method_collar_detector {
   char collar_detector_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char collar_detector_mode;
   char collar_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_collar_k5 {
   char collar_k5_item_type[MAX_ITEM_TYPE_LENGTH];
   unsigned char collar_k5_mode;
};

struct analysis_method_known_alpha {
   char known_alpha_item_type[MAX_ITEM_TYPE_LENGTH];
   char known_alpha_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_known_m {
   char known_m_item_type[MAX_ITEM_TYPE_LENGTH];
   char known_m_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_multiplicity {
   char multiplicity_item_type[MAX_ITEM_TYPE_LENGTH];
   char multiplicity_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_active_passive {
   char active_passive_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_passive_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_add_a_source {
   char add_a_source_item_type[MAX_ITEM_TYPE_LENGTH];
   char add_a_source_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_active_mult {
   char active_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char active_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_curium_ratio {
   char curium_ratio_item_type[MAX_ITEM_TYPE_LENGTH];
   char curium_ratio_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_truncated_mult {
   char truncated_mult_item_type[MAX_ITEM_TYPE_LENGTH];
   char truncated_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct meas_id {
   char meas_date[DATE_TIME_LENGTH];
   char meas_time[DATE_TIME_LENGTH];
   char filename[FILE_NAME_LENGTH];
   char results_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct report_id {
   char results_detector_id[MAX_DETECTOR_ID_LENGTH];
   char stratum_id[MAX_STRATUM_ID_LENGTH];
   char item_id[MAX_ITEM_ID_LENGTH];
   char meas_date[DATE_TIME_LENGTH];
   char meas_time[DATE_TIME_LENGTH];
};

struct stratum_report_id {
   char stratum_id[MAX_STRATUM_ID_LENGTH];
   char results_detector_id[MAX_DETECTOR_ID_LENGTH];
   char item_id[MAX_ITEM_ID_LENGTH];
   char meas_date[DATE_TIME_LENGTH];
   char meas_time[DATE_TIME_LENGTH];
};

struct detector_item_id {
   char results_detector_id[MAX_DETECTOR_ID_LENGTH];
   char item_id[MAX_ITEM_ID_LENGTH];
};

struct stratum_id_key {
   char stratum[MAX_STRATUM_ID_LENGTH];
   char stratum_id_detector_id[MAX_DETECTOR_ID_LENGTH];
};

struct analysis_method_de_mult {
   char de_item_type[MAX_ITEM_TYPE_LENGTH];
   char de_detector_id[MAX_DETECTOR_ID_LENGTH];
};

/* record, field and set table entry definitions */

/* File Id Constants */

/* Record Name Constants */
#define ARCHIVE_PARMS_REC 10000
#define TEST_PARMS_REC 10002
#define ACQUIRE_PARMS_REC 10003
#define ITEM_ID_ENTRY_REC 10004
#define MBA_ITEM_ID_ENTRY_REC 10005
#define COLLAR_DATA_ENTRY_REC 10006
#define POISON_ROD_TYPE_REC 10007
#define HOLDUP_CONFIG_REC 10008
#define CM_PU_RATIO_REC 10009
#define ITEM_TYPE_NAMES_REC 10010
#define FACILITY_NAMES_REC 10011
#define MBA_NAMES_REC 10012
#define STRATUM_ID_NAMES_REC 10013
#define INVENTORY_CHANGE_CODE_REC 10014
#define IO_CODE_REC 10015
#define ISOTOPICS_REC 10016
#define COMPOSITE_ISOTOPICS_REC 10017
#define ASSAY_SUMMARY_REC 10018
#define HOLDUP_SUMMARY_REC 10019
#define DETECTOR_REC 10020
#define ALPHA_BETA_REC 10021
#define BKG_PARMS_REC 10022
#define TM_BKG_PARMS_REC 10023
#define NORM_PARMS_REC 10024
#define SR_PARMS_REC 10025
#define UNATTENDED_PARMS_REC 10026
#define ANALYSIS_METHOD_REC 10027
#define CAL_CURVE_REC 10028
#define ACTIVE_REC 10029
#define COLLAR_REC 10030
#define COLLAR_DETECTOR_REC 10031
#define COLLAR_K5_REC 10032
#define KNOWN_ALPHA_REC 10033
#define KNOWN_M_REC 10034
#define MULTIPLICITY_REC 10035
#define ACTIVE_PASSIVE_REC 10036
#define ADD_A_SOURCE_REC 10037
#define ACTIVE_MULT_REC 10038
#define CURIUM_RATIO_REC 10039
#define TRUNCATED_MULT_REC 10040
#define RESULTS_REC 10041
#define RESULTS_INIT_SRC_REC 10042
#define RESULTS_BIAS_REC 10043
#define RESULTS_PRECISION_REC 10044
#define RESULTS_CAL_CURVE_REC 10045
#define RESULTS_KNOWN_ALPHA_REC 10046
#define RESULTS_KNOWN_M_REC 10047
#define RESULTS_MULTIPLICITY_REC 10048
#define RESULTS_ACTIVE_PASSIVE_REC 10049
#define RESULTS_ACTIVE_REC 10050
#define RESULTS_COLLAR_REC 10051
#define RESULTS_ADD_A_SOURCE_REC 10052
#define RESULTS_ACTIVE_MULT_REC 10053
#define RESULTS_CURIUM_RATIO_REC 10054
#define RESULTS_TRUNCATED_MULT_REC 10055
#define RESULTS_TM_BKG_REC 10056
#define RUN_REC 10057
#define CF1_RUN_REC 10058
#define CF2_RUN_REC 10059
#define CF3_RUN_REC 10060
#define CF4_RUN_REC 10061
#define CF5_RUN_REC 10062
#define A1_RUN_REC 10063
#define A2_RUN_REC 10064
#define A3_RUN_REC 10065
#define A4_RUN_REC 10066
#define A5_RUN_REC 10067
#define A6_RUN_REC 10068
#define B1_RUN_REC 10069
#define B2_RUN_REC 10070
#define B3_RUN_REC 10071
#define B4_RUN_REC 10072
#define B5_RUN_REC 10073
#define B6_RUN_REC 10074
#define ADD_A_SOURCE_SETUP_REC 10075
#define STRATUM_ID_REC 10076
#define DE_MULT_REC 10077
#define RESULTS_DE_MULT_REC 10078

/* Field Name Constants */
#define DAYS_BEFORE_AUTO_ARCHIVE 0L
#define DAYS_BEFORE_AUTO_DELETE 1L
#define DAYS_BEFORE_DB_AUTO_DELETE 2L
#define DATA_DIR 3L
#define ACC_SNGL_TEST_RATE_LIMIT 2000L
#define ACC_SNGL_TEST_PRECISION_LIMIT 2001L
#define ACC_SNGL_TEST_OUTLIER_LIMIT 2002L
#define OUTLIER_TEST_LIMIT 2003L
#define BKG_DOUBLES_RATE_LIMIT 2004L
#define BKG_TRIPLES_RATE_LIMIT 2005L
#define CHISQ_LIMIT 2006L
#define MAX_NUM_FAILURES 2007L
#define HIGH_VOLTAGE_TEST_LIMIT 2008L
#define NORMAL_BACKUP_ASSAY_TEST_LIMIT 2009L
#define MAX_RUNS_FOR_OUTLIER_TEST 2010L
#define CHECKSUM_TEST 2011L
#define ACCIDENTALS_METHOD 2012L
#define ACQ_FACILITY 3000L
#define ACQ_FACILITY_DESCRIPTION 3001L
#define ACQ_MBA 3002L
#define ACQ_MBA_DESCRIPTION 3003L
#define ACQ_DETECTOR_ID 3004L
#define ACQ_ITEM_TYPE 3005L
#define ACQ_GLOVEBOX_ID 3006L
#define ACQ_ISOTOPICS_ID 3007L
#define ACQ_COMP_ISOTOPICS_ID 3008L
#define ACQ_CAMPAIGN_ID 3009L
#define ACQ_ITEM_ID 3010L
#define ACQ_STRATUM_ID 3011L
#define ACQ_STRATUM_ID_DESCRIPTION 3012L
#define ACQ_USER_ID 3013L
#define ACQ_COMMENT 3014L
#define ACQ_ENDING_COMMENT 3015L
#define ACQ_DATA_SRC 3016L
#define ACQ_QC_TESTS 3017L
#define ACQ_PRINT 3018L
#define ACQ_REVIEW_DETECTOR_PARMS 3019L
#define ACQ_REVIEW_CALIB_PARMS 3020L
#define ACQ_REVIEW_ISOTOPICS 3021L
#define ACQ_REVIEW_SUMMED_RAW_DATA 3022L
#define ACQ_REVIEW_RUN_RAW_DATA 3023L
#define ACQ_REVIEW_RUN_RATE_DATA 3024L
#define ACQ_REVIEW_SUMMED_MULT_DIST 3025L
#define ACQ_REVIEW_RUN_MULT_DIST 3026L
#define ACQ_RUN_COUNT_TIME 3027L
#define ACQ_ACQUIRE_TYPE 3028L
#define ACQ_NUM_RUNS 3029L
#define ACQ_ACTIVE_NUM_RUNS 3030L
#define ACQ_MIN_NUM_RUNS 3031L
#define ACQ_MAX_NUM_RUNS 3032L
#define ACQ_MEAS_PRECISION 3033L
#define ACQ_WELL_CONFIG 3034L
#define ACQ_MASS 3035L
#define ACQ_ERROR_CALC_METHOD 3036L
#define ACQ_INVENTORY_CHANGE_CODE 3037L
#define ACQ_IO_CODE 3038L
#define ACQ_COLLAR_MODE 3039L
#define ACQ_DRUM_EMPTY_WEIGHT 3040L
#define ACQ_MEAS_DATE 3041L
#define ACQ_MEAS_TIME 3042L
#define ACQ_MEAS_DETECTOR_ID 3043L
#define ITEM_ID_ENTRY 4000L
#define ITEM_TYPE_ASCII 4001L
#define ISOTOPICS_ID_ASCII 4002L
#define STRATUM_ID_ASCII 4003L
#define INVENTORY_CHANGE_CODE_ENTRY 4004L
#define IO_CODE_ENTRY 4005L
#define DECLARED_MASS_ENTRY 4006L
#define DECLARED_U_MASS_ENTRY 4007L
#define LENGTH_ENTRY 4008L
#define MBA_ASCII 5000L
#define COL_ITEM_ID_ENTRY 6000L
#define COL_LENGTH_ENTRY 6001L
#define COL_LENGTH_ERR_ENTRY 6002L
#define COL_TOTAL_PU_ENTRY 6003L
#define COL_TOTAL_PU_ERR_ENTRY 6004L
#define COL_DEPLETED_U_ENTRY 6005L
#define COL_DEPLETED_U_ERR_ENTRY 6006L
#define COL_NATURAL_U_ENTRY 6007L
#define COL_NATURAL_U_ERR_ENTRY 6008L
#define COL_ENRICHED_U_ENTRY 6009L
#define COL_ENRICHED_U_ERR_ENTRY 6010L
#define COL_TOTAL_U235_ENTRY 6011L
#define COL_TOTAL_U235_ERR_ENTRY 6012L
#define COL_TOTAL_U238_ENTRY 6013L
#define COL_TOTAL_U238_ERR_ENTRY 6014L
#define COL_TOTAL_RODS_ENTRY 6015L
#define COL_TOTAL_POISON_RODS_ENTRY 6016L
#define COL_POISON_PERCENT_ENTRY 6017L
#define COL_POISON_PERCENT_ERR_ENTRY 6018L
#define COL_ROD_TYPE_ENTRY 6019L
#define POISON_ROD_TYPE 7000L
#define POISON_ABSORPTION_FACT 7001L
#define GLOVEBOX_ID 8000L
#define NUM_ROWS 8001L
#define NUM_COLUMNS 8002L
#define DISTANCE 8003L
#define CM_PU_RATIO 9000L
#define CM_PU_RATIO_ERR 9001L
#define CM_PU_HALF_LIFE 9002L
#define CM_PU_RATIO_DATE 9003L
#define CM_U_RATIO 9004L
#define CM_U_RATIO_ERR 9005L
#define CM_U_RATIO_DATE 9006L
#define CM_ID_LABEL 9007L
#define CM_ID 9008L
#define CM_INPUT_BATCH_ID 9009L
#define CM_DCL_U_MASS 9010L
#define CM_DCL_U235_MASS 9011L
#define ITEM_TYPE_NAMES 10000L
#define FACILITY_NAMES 11000L
#define FACILITY_DESCRIPTION 11001L
#define MBA_NAMES 12000L
#define MBA_DESCRIPTION 12001L
#define STRATUM_ID_NAMES 13000L
#define STRATUM_ID_NAMES_DESCRIPTION 13001L
#define INVENTORY_CHG_CODES 14000L
#define IO_CODES 15000L
#define PU238 16000L
#define PU239 16001L
#define PU240 16002L
#define PU241 16003L
#define PU242 16004L
#define AM241 16005L
#define PU238_ERR 16006L
#define PU239_ERR 16007L
#define PU240_ERR 16008L
#define PU241_ERR 16009L
#define PU242_ERR 16010L
#define AM241_ERR 16011L
#define PU_DATE 16012L
#define AM_DATE 16013L
#define ISOTOPICS_ID 16014L
#define ISOTOPICS_SOURCE_CODE 16015L
#define CI_REF_DATE 17000L
#define CI_PU_MASS 17001L
#define CI_PU238 17002L
#define CI_PU239 17003L
#define CI_PU240 17004L
#define CI_PU241 17005L
#define CI_PU242 17006L
#define CI_AM241 17007L
#define CI_PU_DATE 17008L
#define CI_AM_DATE 17009L
#define CI_ISOTOPICS_ID 17010L
#define CI_ISOTOPICS_SOURCE_CODE 17011L
#define AS_PRINT 18000L
#define AS_PATH 18001L
#define AS_SELECT 18002L
#define HU_PRINT 19000L
#define HU_PATH 19001L
#define HU_SELECT 19002L
#define DETECTOR_ID 20000L
#define DETECTOR_TYPE 20001L
#define ELECTRONICS_ID 20002L
#define FACTORIAL 21000L
#define ALPHA_ARRAY 21001L
#define BETA_ARRAY 21002L
#define CURR_PASSIVE_BKG_SINGLES_RATE 22000L
#define CURR_PASSIVE_BKG_SINGLES_ERR 22001L
#define CURR_PASSIVE_BKG_DOUBLES_RATE 22002L
#define CURR_PASSIVE_BKG_DOUBLES_ERR 22003L
#define CURR_PASSIVE_BKG_TRIPLES_RATE 22004L
#define CURR_PASSIVE_BKG_TRIPLES_ERR 22005L
#define CURR_ACTIVE_BKG_SINGLES_RATE 22006L
#define CURR_ACTIVE_BKG_SINGLES_ERR 22007L
#define CURR_PASSIVE_BKG_SCALER1_RATE 22008L
#define CURR_PASSIVE_BKG_SCALER2_RATE 22009L
#define CURR_ACTIVE_BKG_SCALER1_RATE 22010L
#define CURR_ACTIVE_BKG_SCALER2_RATE 22011L
#define TM_SINGLES_BKG 23000L
#define TM_SINGLES_BKG_ERR 23001L
#define TM_ZEROS_BKG 23002L
#define TM_ZEROS_BKG_ERR 23003L
#define TM_ONES_BKG 23004L
#define TM_ONES_BKG_ERR 23005L
#define TM_TWOS_BKG 23006L
#define TM_TWOS_BKG_ERR 23007L
#define TM_BKG 23008L
#define SOURCE_ID 24000L
#define CURR_NORMALIZATION_CONSTANT 24001L
#define CURR_NORMALIZATION_CONSTANT_ERR 24002L
#define BIAS_MODE 24003L
#define MEAS_RATE 24004L
#define MEAS_RATE_ERR 24005L
#define AMLI_REF_SINGLES_RATE 24006L
#define CF252_REF_DOUBLES_RATE 24007L
#define CF252_REF_DOUBLES_RATE_ERR 24008L
#define REF_DATE 24009L
#define INIT_SRC_PRECISION_LIMIT 24010L
#define BIAS_PRECISION_LIMIT 24011L
#define ACCEPTANCE_LIMIT_STD_DEV 24012L
#define ACCEPTANCE_LIMIT_PERCENT 24013L
#define YIELD_RELATIVE_TO_MRC_95 24014L
#define BIAS_TEST_USE_ADDASRC 24015L
#define BIAS_TEST_ADDASRC_POSITION 24016L
#define SR_DETECTOR_ID 25000L
#define SR_TYPE 25001L
#define SR_PORT_NUMBER 25002L
#define PREDELAY 25003L
#define GATE_LENGTH 25004L
#define GATE_LENGTH2 25005L
#define HIGH_VOLTAGE 25006L
#define DIE_AWAY_TIME 25007L
#define EFFICIENCY 25008L
#define MULTIPLICITY_DEADTIME 25009L
#define COEFF_A_DEADTIME 25010L
#define COEFF_B_DEADTIME 25011L
#define COEFF_C_DEADTIME 25012L
#define DOUBLES_GATE_FRACTION 25013L
#define TRIPLES_GATE_FRACTION 25014L
#define SR_SPARES 25015L
#define ERROR_SECONDS 26000L
#define AUTO_IMPORT 26001L
#define ADD_A_SOURCE_THRESHOLD 26002L
#define ITEM_TYPE 27000L
#define ANALYSIS_METHOD_DETECTOR_ID 27001L
#define CAL_CURVE 27002L
#define KNOWN_ALPHA 27003L
#define KNOWN_M 27004L
#define MULTIPLICITY 27005L
#define ADD_A_SOURCE 27006L
#define ACTIVE 27007L
#define ACTIVE_MULT 27008L
#define ACTIVE_PASSIVE 27009L
#define COLLAR 27010L
#define NORMAL_METHOD 27011L
#define BACKUP_METHOD 27012L
#define CURIUM_RATIO 27013L
#define TRUNCATED_MULT 27014L
#define ANALYSIS_METHOD_SPARE1 27015L
#define ANALYSIS_METHOD_SPARE2 27016L
#define ANALYSIS_METHOD_SPARE3 27017L
#define ANALYSIS_METHOD_SPARE4 27018L
#define ANALYSIS_METHOD 27019L
#define CAL_CURVE_EQUATION 28000L
#define CC_A 28001L
#define CC_B 28002L
#define CC_C 28003L
#define CC_D 28004L
#define CC_VAR_A 28005L
#define CC_VAR_B 28006L
#define CC_VAR_C 28007L
#define CC_VAR_D 28008L
#define CC_COVAR_AB 28009L
#define CC_COVAR_AC 28010L
#define CC_COVAR_AD 28011L
#define CC_COVAR_BC 28012L
#define CC_COVAR_BD 28013L
#define CC_COVAR_CD 28014L
#define CC_SIGMA_X 28015L
#define CC_CAL_CURVE_TYPE 28016L
#define CC_HEAVY_METAL_CORR_FACTOR 28017L
#define CC_HEAVY_METAL_REFERENCE 28018L
#define CC_PERCENT_U235 28019L
#define CAL_CURVE_SPARES 28020L
#define CAL_CURVE_ITEM_TYPE 28021L
#define CAL_CURVE_DETECTOR_ID 28022L
#define CC_LOWER_MASS_LIMIT 28023L
#define CC_UPPER_MASS_LIMIT 28024L
#define CC_DCL_MASS 28025L
#define CC_DOUBLES 28026L
#define ANALYSIS_METHOD_CAL_CURVE 28027L
#define ACTIVE_EQUATION 29000L
#define ACT_A 29001L
#define ACT_B 29002L
#define ACT_C 29003L
#define ACT_D 29004L
#define ACT_VAR_A 29005L
#define ACT_VAR_B 29006L
#define ACT_VAR_C 29007L
#define ACT_VAR_D 29008L
#define ACT_COVAR_AB 29009L
#define ACT_COVAR_AC 29010L
#define ACT_COVAR_AD 29011L
#define ACT_COVAR_BC 29012L
#define ACT_COVAR_BD 29013L
#define ACT_COVAR_CD 29014L
#define ACT_SIGMA_X 29015L
#define ACTIVE_SPARES 29016L
#define ACTIVE_ITEM_TYPE 29017L
#define ACTIVE_DETECTOR_ID 29018L
#define ACT_LOWER_MASS_LIMIT 29019L
#define ACT_UPPER_MASS_LIMIT 29020L
#define ACT_DCL_MASS 29021L
#define ACT_DOUBLES 29022L
#define ANALYSIS_METHOD_ACTIVE 29023L
#define COLLAR_EQUATION 30000L
#define COL_A 30001L
#define COL_B 30002L
#define COL_C 30003L
#define COL_D 30004L
#define COL_VAR_A 30005L
#define COL_VAR_B 30006L
#define COL_VAR_C 30007L
#define COL_VAR_D 30008L
#define COL_COVAR_AB 30009L
#define COL_COVAR_AC 30010L
#define COL_COVAR_AD 30011L
#define COL_COVAR_BC 30012L
#define COL_COVAR_BD 30013L
#define COL_COVAR_CD 30014L
#define COL_SIGMA_X 30015L
#define COL_NUMBER_CALIB_RODS 30016L
#define COL_POISON_ROD_TYPE 30017L
#define COL_POISON_ABSORPTION_FACT 30018L
#define COL_POISON_ROD_A 30019L
#define COL_POISON_ROD_A_ERR 30020L
#define COL_POISON_ROD_B 30021L
#define COL_POISON_ROD_B_ERR 30022L
#define COL_POISON_ROD_C 30023L
#define COL_POISON_ROD_C_ERR 30024L
#define COL_U_MASS_CORR_FACT_A 30025L
#define COL_U_MASS_CORR_FACT_A_ERR 30026L
#define COL_U_MASS_CORR_FACT_B 30027L
#define COL_U_MASS_CORR_FACT_B_ERR 30028L
#define COL_SAMPLE_CORR_FACT 30029L
#define COL_SAMPLE_CORR_FACT_ERR 30030L
#define COLLAR_SPARES 30031L
#define COLLAR_ITEM_TYPE 30032L
#define COLLAR_MODE 30033L
#define COL_LOWER_MASS_LIMIT 30034L
#define COL_UPPER_MASS_LIMIT 30035L
#define ANALYSIS_METHOD_COLLAR 30036L
#define COLLAR_DETECTOR_ITEM_TYPE 31000L
#define COLLAR_DETECTOR_MODE 31001L
#define COLLAR_DETECTOR_ID 31002L
#define COL_REFERENCE_DATE 31003L
#define COL_RELATIVE_DOUBLES_RATE 31004L
#define ANALYSIS_METHOD_COLLAR_DETECTOR 31005L
#define COLLAR_K5_LABEL 32000L
#define COLLAR_K5_CHECKBOX 32001L
#define COLLAR_K5 32002L
#define COLLAR_K5_ERR 32003L
#define COLLAR_K5_ITEM_TYPE 32004L
#define COLLAR_K5_MODE 32005L
#define ANALYSIS_METHOD_COLLAR_K5 32006L
#define KA_ALPHA_WT 33000L
#define KA_RHO_ZERO 33001L
#define KA_K 33002L
#define KA_A 33003L
#define KA_B 33004L
#define KA_VAR_A 33005L
#define KA_VAR_B 33006L
#define KA_COVAR_AB 33007L
#define KA_SIGMA_X 33008L
#define KA_KNOWN_ALPHA_TYPE 33009L
#define KA_HEAVY_METAL_CORR_FACTOR 33010L
#define KA_HEAVY_METAL_REFERENCE 33011L
#define KA_RING_RATIO_EQUATION 33012L
#define KA_RING_RATIO_A 33013L
#define KA_RING_RATIO_B 33014L
#define KA_RING_RATIO_C 33015L
#define KA_RING_RATIO_D 33016L
#define KA_LOWER_CORR_FACTOR_LIMIT 33017L
#define KA_UPPER_CORR_FACTOR_LIMIT 33018L
#define KNOWN_ALPHA_ITEM_TYPE 33019L
#define KNOWN_ALPHA_DETECTOR_ID 33020L
#define KA_LOWER_MASS_LIMIT 33021L
#define KA_UPPER_MASS_LIMIT 33022L
#define KA_DCL_MASS 33023L
#define KA_DOUBLES 33024L
#define ANALYSIS_METHOD_KNOWN_ALPHA 33025L
#define KM_SF_RATE 34000L
#define KM_VS1 34001L
#define KM_VS2 34002L
#define KM_VI1 34003L
#define KM_VI2 34004L
#define KM_B 34005L
#define KM_C 34006L
#define KM_SIGMA_X 34007L
#define KNOWN_M_SPARES 34008L
#define KNOWN_M_ITEM_TYPE 34009L
#define KNOWN_M_DETECTOR_ID 34010L
#define KM_LOWER_MASS_LIMIT 34011L
#define KM_UPPER_MASS_LIMIT 34012L
#define ANALYSIS_METHOD_KNOWN_M 34013L
#define MUL_SOLVE_EFFICIENCY 35000L
#define MUL_SF_RATE 35001L
#define MUL_VS1 35002L
#define MUL_VS2 35003L
#define MUL_VS3 35004L
#define MUL_VI1 35005L
#define MUL_VI2 35006L
#define MUL_VI3 35007L
#define MUL_A 35008L
#define MUL_B 35009L
#define MUL_C 35010L
#define MUL_SIGMA_X 35011L
#define MUL_ALPHA_WEIGHT 35012L
#define MULTIPLICITY_SPARES 35013L
#define MULTIPLICITY_ITEM_TYPE 35014L
#define MULTIPLICITY_DETECTOR_ID 35015L
#define ANALYSIS_METHOD_MULTIPLICITY 35016L
#define ACTIVE_PASSIVE_EQUATION 36000L
#define AP_A 36001L
#define AP_B 36002L
#define AP_C 36003L
#define AP_D 36004L
#define AP_VAR_A 36005L
#define AP_VAR_B 36006L
#define AP_VAR_C 36007L
#define AP_VAR_D 36008L
#define AP_COVAR_AB 36009L
#define AP_COVAR_AC 36010L
#define AP_COVAR_AD 36011L
#define AP_COVAR_BC 36012L
#define AP_COVAR_BD 36013L
#define AP_COVAR_CD 36014L
#define AP_SIGMA_X 36015L
#define ACTIVE_PASSIVE_SPARES 36016L
#define ACTIVE_PASSIVE_ITEM_TYPE 36017L
#define ACTIVE_PASSIVE_DETECTOR_ID 36018L
#define AP_LOWER_MASS_LIMIT 36019L
#define AP_UPPER_MASS_LIMIT 36020L
#define ANALYSIS_METHOD_ACTIVE_PASSIVE 36021L
#define ADD_A_SOURCE_EQUATION 37000L
#define AD_A 37001L
#define AD_B 37002L
#define AD_C 37003L
#define AD_D 37004L
#define AD_VAR_A 37005L
#define AD_VAR_B 37006L
#define AD_VAR_C 37007L
#define AD_VAR_D 37008L
#define AD_COVAR_AB 37009L
#define AD_COVAR_AC 37010L
#define AD_COVAR_AD 37011L
#define AD_COVAR_BC 37012L
#define AD_COVAR_BD 37013L
#define AD_COVAR_CD 37014L
#define AD_SIGMA_X 37015L
#define ADD_A_SOURCE_SPARES 37016L
#define AD_POSITION_DZERO 37017L
#define AD_DZERO_AVG 37018L
#define AD_DZERO_REF_DATE 37019L
#define AD_NUM_RUNS 37020L
#define AD_CF_A 37021L
#define AD_CF_B 37022L
#define AD_CF_C 37023L
#define AD_CF_D 37024L
#define AD_USE_TRUNCATED_MULT 37025L
#define AD_TM_WEIGHTING_FACTOR 37026L
#define AD_TM_DBLS_RATE_UPPER_LIMIT 37027L
#define ADD_A_SOURCE_CF_SPARES 37028L
#define ADD_A_SOURCE_ITEM_TYPE 37029L
#define ADD_A_SOURCE_DETECTOR_ID 37030L
#define AD_LOWER_MASS_LIMIT 37031L
#define AD_UPPER_MASS_LIMIT 37032L
#define AD_DCL_MASS 37033L
#define AD_DOUBLES 37034L
#define ANALYSIS_METHOD_ADD_A_SOURCE 37035L
#define AM_VT1 38000L
#define AM_VT2 38001L
#define AM_VT3 38002L
#define AM_VF1 38003L
#define AM_VF2 38004L
#define AM_VF3 38005L
#define ACTIVE_MULT_SPARES 38006L
#define ACTIVE_MULT_ITEM_TYPE 38007L
#define ACTIVE_MULT_DETECTOR_ID 38008L
#define ANALYSIS_METHOD_ACTIVE_MULT 38009L
#define CURIUM_RATIO_EQUATION 39000L
#define CR_A 39001L
#define CR_B 39002L
#define CR_C 39003L
#define CR_D 39004L
#define CR_VAR_A 39005L
#define CR_VAR_B 39006L
#define CR_VAR_C 39007L
#define CR_VAR_D 39008L
#define CR_COVAR_AB 39009L
#define CR_COVAR_AC 39010L
#define CR_COVAR_AD 39011L
#define CR_COVAR_BC 39012L
#define CR_COVAR_BD 39013L
#define CR_COVAR_CD 39014L
#define CR_SIGMA_X 39015L
#define CURIUM_RATIO_TYPE 39016L
#define CURIUM_RATIO_SPARES 39017L
#define CURIUM_RATIO_ITEM_TYPE 39018L
#define CURIUM_RATIO_DETECTOR_ID 39019L
#define CR_LOWER_MASS_LIMIT 39020L
#define CR_UPPER_MASS_LIMIT 39021L
#define ANALYSIS_METHOD_CURIUM_RATIO 39022L
#define TM_A 40000L
#define TM_B 40001L
#define TM_KNOWN_EFF 40002L
#define TM_SOLVE_EFF 40003L
#define TRUNCATED_MULT_SPARES 40004L
#define TRUNCATED_MULT_ITEM_TYPE 40005L
#define TRUNCATED_MULT_DETECTOR_ID 40006L
#define ANALYSIS_METHOD_TRUNCATED_MULT 40007L
#define MEAS_DATE 41000L
#define MEAS_TIME 41001L
#define FILENAME 41002L
#define ORIGINAL_MEAS_DATE 41003L
#define RESULTS_FACILITY 41004L
#define RESULTS_FACILITY_DESCRIPTION 41005L
#define RESULTS_MBA 41006L
#define RESULTS_MBA_DESCRIPTION 41007L
#define ITEM_ID 41008L
#define STRATUM_ID 41009L
#define STRATUM_ID_DESCRIPTION 41010L
#define RESULTS_CAMPAIGN_ID 41011L
#define RESULTS_INSPECTION_NUMBER 41012L
#define RESULTS_ITEM_TYPE 41013L
#define RESULTS_COLLAR_MODE 41014L
#define RESULTS_DETECTOR_ID 41015L
#define RESULTS_DETECTOR_TYPE 41016L
#define RESULTS_ELECTRONICS_ID 41017L
#define RESULTS_GLOVEBOX_ID 41018L
#define RESULTS_NUM_ROWS 41019L
#define RESULTS_NUM_COLUMNS 41020L
#define RESULTS_DISTANCE 41021L
#define BIAS_UNCERTAINTY 41022L
#define RANDOM_UNCERTAINTY 41023L
#define SYSTEMATIC_UNCERTAINTY 41024L
#define RELATIVE_STD_DEV 41025L
#define COMPLETED 41026L
#define MEAS_OPTION 41027L
#define INVENTORY_CHANGE_CODE 41028L
#define IO_CODE 41029L
#define WELL_CONFIG 41030L
#define DATA_SOURCE 41031L
#define RESULTS_QC_TESTS 41032L
#define ERROR_CALC_METHOD 41033L
#define RESULTS_PRINT 41034L
#define USER_ID 41035L
#define COMMENT 41036L
#define ENDING_COMMENT 41037L
#define ITEM_PU238 41038L
#define ITEM_PU239 41039L
#define ITEM_PU240 41040L
#define ITEM_PU241 41041L
#define ITEM_PU242 41042L
#define ITEM_AM241 41043L
#define ITEM_PU238_ERR 41044L
#define ITEM_PU239_ERR 41045L
#define ITEM_PU240_ERR 41046L
#define ITEM_PU241_ERR 41047L
#define ITEM_PU242_ERR 41048L
#define ITEM_AM241_ERR 41049L
#define ITEM_PU_DATE 41050L
#define ITEM_AM_DATE 41051L
#define ITEM_ISOTOPICS_ID 41052L
#define ITEM_ISOTOPICS_SOURCE_CODE 41053L
#define NORMALIZATION_CONSTANT 41054L
#define NORMALIZATION_CONSTANT_ERR 41055L
#define RESULTS_PREDELAY 41056L
#define RESULTS_GATE_LENGTH 41057L
#define RESULTS_GATE_LENGTH2 41058L
#define RESULTS_HIGH_VOLTAGE 41059L
#define RESULTS_DIE_AWAY_TIME 41060L
#define RESULTS_EFFICIENCY 41061L
#define RESULTS_MULTIPLICITY_DEADTIME 41062L
#define RESULTS_COEFF_A_DEADTIME 41063L
#define RESULTS_COEFF_B_DEADTIME 41064L
#define RESULTS_COEFF_C_DEADTIME 41065L
#define RESULTS_DOUBLES_GATE_FRACTION 41066L
#define RESULTS_TRIPLES_GATE_FRACTION 41067L
#define R_ACC_SNGL_TEST_RATE_LIMIT 41068L
#define R_ACC_SNGL_TEST_PRECISION_LIMIT 41069L
#define R_ACC_SNGL_TEST_OUTLIER_LIMIT 41070L
#define R_OUTLIER_TEST_LIMIT 41071L
#define R_BKG_DOUBLES_RATE_LIMIT 41072L
#define R_BKG_TRIPLES_RATE_LIMIT 41073L
#define R_CHISQ_LIMIT 41074L
#define R_MAX_NUM_FAILURES 41075L
#define R_HIGH_VOLTAGE_TEST_LIMIT 41076L
#define PASSIVE_BKG_SINGLES_RATE 41077L
#define PASSIVE_BKG_SINGLES_RATE_ERR 41078L
#define PASSIVE_BKG_DOUBLES_RATE 41079L
#define PASSIVE_BKG_DOUBLES_RATE_ERR 41080L
#define PASSIVE_BKG_TRIPLES_RATE 41081L
#define PASSIVE_BKG_TRIPLES_RATE_ERR 41082L
#define ACTIVE_BKG_SINGLES_RATE 41083L
#define ACTIVE_BKG_SINGLES_RATE_ERR 41084L
#define PASSIVE_BKG_SCALER1_RATE 41085L
#define PASSIVE_BKG_SCALER2_RATE 41086L
#define ACTIVE_BKG_SCALER1_RATE 41087L
#define ACTIVE_BKG_SCALER2_RATE 41088L
#define ERROR_MSG_CODES 41089L
#define WARNING_MSG_CODES 41090L
#define TOTAL_NUMBER_RUNS 41091L
#define NUMBER_GOOD_RUNS 41092L
#define TOTAL_GOOD_COUNT_TIME 41093L
#define SINGLES_SUM 41094L
#define SCALER1_SUM 41095L
#define SCALER2_SUM 41096L
#define REALS_PLUS_ACC_SUM 41097L
#define ACC_SUM 41098L
#define MULT_REALS_PLUS_ACC_SUM 41099L
#define MULT_ACC_SUM 41100L
#define SINGLES 41101L
#define SINGLES_ERR 41102L
#define DOUBLES 41103L
#define DOUBLES_ERR 41104L
#define TRIPLES 41105L
#define TRIPLES_ERR 41106L
#define SCALER1 41107L
#define SCALER1_ERR 41108L
#define SCALER2 41109L
#define SCALER2_ERR 41110L
#define UNCORRECTED_DOUBLES 41111L
#define UNCORRECTED_DOUBLES_ERR 41112L
#define SINGLES_MULTI 41113L
#define DOUBLES_MULTI 41114L
#define TRIPLES_MULTI 41115L
#define DECLARED_MASS 41116L
#define PRIMARY_ANALYSIS_METHOD 41117L
#define NET_DRUM_WEIGHT 41118L
#define PASSIVE_MEAS_DATE 41119L
#define PASSIVE_MEAS_TIME 41120L
#define PASSIVE_FILENAME 41121L
#define PASSIVE_RESULTS_DETECTOR_ID 41122L
#define ACTIVE_MEAS_DATE 41123L
#define ACTIVE_MEAS_TIME 41124L
#define ACTIVE_FILENAME 41125L
#define ACTIVE_RESULTS_DETECTOR_ID 41126L
#define COVARIANCE_MATRIX 41127L
#define R_NORMAL_BACKUP_ASSAY_TEST_LIM 41128L
#define R_MAX_RUNS_FOR_OUTLIER_TEST 41129L
#define R_CHECKSUM_TEST 41130L
#define RESULTS_ACCIDENTALS_METHOD 41131L
#define DECLARED_U_MASS 41132L
#define LENGTH 41133L
#define DB_VERSION 41134L
#define RESULTS_SPARES 41135L
#define MEAS_ID 41136L
#define REPORT_ID 41137L
#define STRATUM_REPORT_ID 41138L
#define DETECTOR_ITEM_ID 41139L
#define INIT_SRC_ID 42000L
#define INIT_SRC_PASS_FAIL 42001L
#define INIT_SRC_MODE 42002L
#define BIAS_SOURCE_ID 43000L
#define BIAS_PASS_FAIL 43001L
#define RESULTS_BIAS_MODE 43002L
#define BIAS_SNGLS_RATE_EXPECT 43003L
#define BIAS_SNGLS_RATE_EXPECT_ERR 43004L
#define BIAS_SNGLS_RATE_EXPECT_MEAS 43005L
#define BIAS_SNGLS_RATE_EXPECT_MEAS_ERR 43006L
#define BIAS_DBLS_RATE_EXPECT 43007L
#define BIAS_DBLS_RATE_EXPECT_ERR 43008L
#define BIAS_DBLS_RATE_EXPECT_MEAS 43009L
#define BIAS_DBLS_RATE_EXPECT_MEAS_ERR 43010L
#define NEW_NORM_CONSTANT 43011L
#define NEW_NORM_CONSTANT_ERR 43012L
#define MEAS_PRECISION 43013L
#define REQUIRED_PRECISION 43014L
#define REQUIRED_MEAS_SECONDS 43015L
#define BIAS_SPARES 43016L
#define PREC_PASS_FAIL 44000L
#define PREC_SAMPLE_VAR 44001L
#define PREC_THEORETICAL_VAR 44002L
#define PREC_CHI_SQ 44003L
#define CHI_SQ_LOWER_LIMIT 44004L
#define CHI_SQ_UPPER_LIMIT 44005L
#define PREC_SPARES 44006L
#define CC_PU240E_MASS 45000L
#define CC_PU240E_MASS_ERR 45001L
#define CC_PU_MASS 45002L
#define CC_PU_MASS_ERR 45003L
#define CC_DCL_PU240E_MASS 45004L
#define CC_DCL_PU_MASS 45005L
#define CC_DCL_MINUS_ASY_PU_MASS 45006L
#define CC_DCL_MINUS_ASY_PU_MASS_ERR 45007L
#define CC_DCL_MINUS_ASY_PU_MASS_PCT 45008L
#define CC_PASS_FAIL 45009L
#define CC_DCL_U_MASS 45010L
#define CC_LENGTH 45011L
#define CC_HEAVY_METAL_CONTENT 45012L
#define CC_HEAVY_METAL_CORRECTION 45013L
#define CC_HEAVY_METAL_CORR_SINGLES 45014L
#define CC_HEAVY_METAL_CORR_SINGLES_ERR 45015L
#define CC_HEAVY_METAL_CORR_DOUBLES 45016L
#define CC_HEAVY_METAL_CORR_DOUBLES_ERR 45017L
#define CC_SPARES 45018L
#define CC_CAL_CURVE_EQUATION 45019L
#define CC_A_RES 45020L
#define CC_B_RES 45021L
#define CC_C_RES 45022L
#define CC_D_RES 45023L
#define CC_VAR_A_RES 45024L
#define CC_VAR_B_RES 45025L
#define CC_VAR_C_RES 45026L
#define CC_VAR_D_RES 45027L
#define CC_COVAR_AB_RES 45028L
#define CC_COVAR_AC_RES 45029L
#define CC_COVAR_AD_RES 45030L
#define CC_COVAR_BC_RES 45031L
#define CC_COVAR_BD_RES 45032L
#define CC_COVAR_CD_RES 45033L
#define CC_SIGMA_X_RES 45034L
#define CC_CAL_CURVE_TYPE_RES 45035L
#define CC_HEAVY_METAL_CORR_FACTOR_RES 45036L
#define CC_HEAVY_METAL_REFERENCE_RES 45037L
#define CC_PERCENT_U235_RES 45038L
#define CC_SPARES_RES 45039L
#define CC_CAL_CURVE_ITEM_TYPE 45040L
#define CC_CAL_CURVE_DETECTOR_ID 45041L
#define KA_MULT 46000L
#define KA_ALPHA 46001L
#define KA_MULT_CORR_DOUBLES 46002L
#define KA_MULT_CORR_DOUBLES_ERR 46003L
#define KA_PU240E_MASS 46004L
#define KA_PU240E_MASS_ERR 46005L
#define KA_PU_MASS 46006L
#define KA_PU_MASS_ERR 46007L
#define KA_DCL_PU240E_MASS 46008L
#define KA_DCL_PU_MASS 46009L
#define KA_DCL_MINUS_ASY_PU_MASS 46010L
#define KA_DCL_MINUS_ASY_PU_MASS_ERR 46011L
#define KA_DCL_MINUS_ASY_PU_MASS_PCT 46012L
#define KA_PASS_FAIL 46013L
#define KA_DCL_U_MASS 46014L
#define KA_LENGTH 46015L
#define KA_HEAVY_METAL_CONTENT 46016L
#define KA_HEAVY_METAL_CORRECTION 46017L
#define KA_CORR_SINGLES 46018L
#define KA_CORR_SINGLES_ERR 46019L
#define KA_CORR_DOUBLES 46020L
#define KA_CORR_DOUBLES_ERR 46021L
#define KA_CORR_FACTOR 46022L
#define KA_DRY_ALPHA_OR_MULT_DBLS 46023L
#define KA_ALPHA_WT_RES 46024L
#define KA_RHO_ZERO_RES 46025L
#define KA_K_RES 46026L
#define KA_A_RES 46027L
#define KA_B_RES 46028L
#define KA_VAR_A_RES 46029L
#define KA_VAR_B_RES 46030L
#define KA_COVAR_AB_RES 46031L
#define KA_SIGMA_X_RES 46032L
#define KA_KNOWN_ALPHA_TYPE_RES 46033L
#define KA_HEAVY_METAL_CORR_FACTOR_RES 46034L
#define KA_HEAVY_METAL_REFERENCE_RES 46035L
#define KA_RING_RATIO_EQUATION_RES 46036L
#define KA_RING_RATIO_A_RES 46037L
#define KA_RING_RATIO_B_RES 46038L
#define KA_RING_RATIO_C_RES 46039L
#define KA_RING_RATIO_D_RES 46040L
#define KA_LOWER_CORR_FACTOR_LIMIT_RES 46041L
#define KA_UPPER_CORR_FACTOR_LIMIT_RES 46042L
#define KA_KNOWN_ALPHA_ITEM_TYPE 46043L
#define KA_KNOWN_ALPHA_DETECTOR_ID 46044L
#define KM_MULT 47000L
#define KM_ALPHA 47001L
#define KM_PU239E_MASS 47002L
#define KM_PU240E_MASS 47003L
#define KM_PU240E_MASS_ERR 47004L
#define KM_PU_MASS 47005L
#define KM_PU_MASS_ERR 47006L
#define KM_DCL_PU240E_MASS 47007L
#define KM_DCL_PU_MASS 47008L
#define KM_DCL_MINUS_ASY_PU_MASS 47009L
#define KM_DCL_MINUS_ASY_PU_MASS_ERR 47010L
#define KM_DCL_MINUS_ASY_PU_MASS_PCT 47011L
#define KM_PASS_FAIL 47012L
#define KM_SPARES 47013L
#define KM_SF_RATE_RES 47014L
#define KM_VS1_RES 47015L
#define KM_VS2_RES 47016L
#define KM_VI1_RES 47017L
#define KM_VI2_RES 47018L
#define KM_B_RES 47019L
#define KM_C_RES 47020L
#define KM_SIGMA_X_RES 47021L
#define KM_SPARES_RES 47022L
#define KM_KNOWN_M_ITEM_TYPE 47023L
#define KM_KNOWN_M_DETECTOR_ID 47024L
#define MUL_MULT 48000L
#define MUL_MULT_ERR 48001L
#define MUL_ALPHA 48002L
#define MUL_ALPHA_ERR 48003L
#define MUL_CORR_FACTOR 48004L
#define MUL_CORR_FACTOR_ERR 48005L
#define MUL_EFFICIENCY 48006L
#define MUL_EFFICIENCY_ERR 48007L
#define MUL_PU240E_MASS 48008L
#define MUL_PU240E_MASS_ERR 48009L
#define MUL_PU_MASS 48010L
#define MUL_PU_MASS_ERR 48011L
#define MUL_DCL_PU240E_MASS 48012L
#define MUL_DCL_PU_MASS 48013L
#define MUL_DCL_MINUS_ASY_PU_MASS 48014L
#define MUL_DCL_MINUS_ASY_PU_MASS_ERR 48015L
#define MUL_DCL_MINUS_ASY_PU_MASS_PCT 48016L
#define MUL_PASS_FAIL 48017L
#define MUL_SPARES 48018L
#define MUL_SOLVE_EFFICIENCY_RES 48019L
#define MUL_SF_RATE_RES 48020L
#define MUL_VS1_RES 48021L
#define MUL_VS2_RES 48022L
#define MUL_VS3_RES 48023L
#define MUL_VI1_RES 48024L
#define MUL_VI2_RES 48025L
#define MUL_VI3_RES 48026L
#define MUL_A_RES 48027L
#define MUL_B_RES 48028L
#define MUL_C_RES 48029L
#define MUL_SIGMA_X_RES 48030L
#define MUL_ALPHA_WEIGHT_RES 48031L
#define MUL_SPARES_RES 48032L
#define MUL_MULTIPLICITY_ITEM_TYPE 48033L
#define MUL_MULTIPLICITY_DETECTOR_ID 48034L
#define AP_DELTA_DOUBLES 49000L
#define AP_DELTA_DOUBLES_ERR 49001L
#define AP_U235_MASS 49002L
#define AP_U235_MASS_ERR 49003L
#define AP_K0 49004L
#define AP_K1 49005L
#define AP_K1_ERR 49006L
#define AP_K 49007L
#define AP_K_ERR 49008L
#define AP_DCL_U235_MASS 49009L
#define AP_DCL_MINUS_ASY_U235_MASS 49010L
#define AP_DCL_MINUS_ASY_U235_MASS_ERR 49011L
#define AP_DCL_MINUS_ASY_U235_MASS_PCT 49012L
#define AP_PASS_FAIL 49013L
#define AP_SPARES 49014L
#define AP_ACTIVE_PASSIVE_EQUATION 49015L
#define AP_A_RES 49016L
#define AP_B_RES 49017L
#define AP_C_RES 49018L
#define AP_D_RES 49019L
#define AP_VAR_A_RES 49020L
#define AP_VAR_B_RES 49021L
#define AP_VAR_C_RES 49022L
#define AP_VAR_D_RES 49023L
#define AP_COVAR_AB_RES 49024L
#define AP_COVAR_AC_RES 49025L
#define AP_COVAR_AD_RES 49026L
#define AP_COVAR_BC_RES 49027L
#define AP_COVAR_BD_RES 49028L
#define AP_COVAR_CD_RES 49029L
#define AP_SIGMA_X_RES 49030L
#define AP_SPARES_RES 49031L
#define AP_ACTIVE_PASSIVE_ITEM_TYPE 49032L
#define AP_ACTIVE_PASSIVE_DETECTOR_ID 49033L
#define ACT_U235_MASS 50000L
#define ACT_U235_MASS_ERR 50001L
#define ACT_K0 50002L
#define ACT_K1 50003L
#define ACT_K1_ERR 50004L
#define ACT_K 50005L
#define ACT_K_ERR 50006L
#define ACT_DCL_U235_MASS 50007L
#define ACT_DCL_MINUS_ASY_U235_MASS 50008L
#define ACT_DCL_MINUS_ASY_U235_MASS_ERR 50009L
#define ACT_DCL_MINUS_ASY_U235_MASS_PCT 50010L
#define ACT_PASS_FAIL 50011L
#define ACT_SPARES 50012L
#define ACT_ACTIVE_EQUATION 50013L
#define ACT_A_RES 50014L
#define ACT_B_RES 50015L
#define ACT_C_RES 50016L
#define ACT_D_RES 50017L
#define ACT_VAR_A_RES 50018L
#define ACT_VAR_B_RES 50019L
#define ACT_VAR_C_RES 50020L
#define ACT_VAR_D_RES 50021L
#define ACT_COVAR_AB_RES 50022L
#define ACT_COVAR_AC_RES 50023L
#define ACT_COVAR_AD_RES 50024L
#define ACT_COVAR_BC_RES 50025L
#define ACT_COVAR_BD_RES 50026L
#define ACT_COVAR_CD_RES 50027L
#define ACT_SIGMA_X_RES 50028L
#define ACT_SPARES_RES 50029L
#define ACT_ACTIVE_ITEM_TYPE 50030L
#define ACT_ACTIVE_DETECTOR_ID 50031L
#define COL_U235_MASS 51000L
#define COL_U235_MASS_ERR 51001L
#define COL_PERCENT_U235 51002L
#define COL_TOTAL_U_MASS 51003L
#define COL_K0 51004L
#define COL_K0_ERR 51005L
#define COL_K1 51006L
#define COL_K1_ERR 51007L
#define COL_K2 51008L
#define COL_K2_ERR 51009L
#define COL_K3 51010L
#define COL_K3_ERR 51011L
#define COL_K4 51012L
#define COL_K4_ERR 51013L
#define COL_K5 51014L
#define COL_K5_ERR 51015L
#define COL_TOTAL_CORR_FACT 51016L
#define COL_TOTAL_CORR_FACT_ERR 51017L
#define COL_SOURCE_ID 51018L
#define COL_CORR_DOUBLES 51019L
#define COL_CORR_DOUBLES_ERR 51020L
#define COL_DCL_LENGTH 51021L
#define COL_DCL_LENGTH_ERR 51022L
#define COL_DCL_TOTAL_U235 51023L
#define COL_DCL_TOTAL_U235_ERR 51024L
#define COL_DCL_TOTAL_U238 51025L
#define COL_DCL_TOTAL_U238_ERR 51026L
#define COL_DCL_TOTAL_RODS 51027L
#define COL_DCL_TOTAL_POISON_RODS 51028L
#define COL_DCL_POISON_PERCENT 51029L
#define COL_DCL_POISON_PERCENT_ERR 51030L
#define COL_DCL_MINUS_ASY_U235_MASS 51031L
#define COL_DCL_MINUS_ASY_U235_MASS_ERR 51032L
#define COL_DCL_MINUS_ASY_U235_MASS_PCT 51033L
#define COL_PASS_FAIL 51034L
#define COL_SPARES 51035L
#define COL_COLLAR_ITEM_TYPE 51036L
#define COL_COLLAR_MODE 51037L
#define COL_COLLAR_DETECTOR_ID 51038L
#define COL_COLLAR_EQUATION 51039L
#define COL_A_RES 51040L
#define COL_B_RES 51041L
#define COL_C_RES 51042L
#define COL_D_RES 51043L
#define COL_VAR_A_RES 51044L
#define COL_VAR_B_RES 51045L
#define COL_VAR_C_RES 51046L
#define COL_VAR_D_RES 51047L
#define COL_COVAR_AB_RES 51048L
#define COL_COVAR_AC_RES 51049L
#define COL_COVAR_AD_RES 51050L
#define COL_COVAR_BC_RES 51051L
#define COL_COVAR_BD_RES 51052L
#define COL_COVAR_CD_RES 51053L
#define COL_SIGMA_X_RES 51054L
#define COL_NUMBER_CALIB_RODS_RES 51055L
#define COL_POISON_ROD_TYPE_RES 51056L
#define COL_POISON_ABSORPTION_FACT_RES 51057L
#define COL_POISON_ROD_A_RES 51058L
#define COL_POISON_ROD_A_ERR_RES 51059L
#define COL_POISON_ROD_B_RES 51060L
#define COL_POISON_ROD_B_ERR_RES 51061L
#define COL_POISON_ROD_C_RES 51062L
#define COL_POISON_ROD_C_ERR_RES 51063L
#define COL_U_MASS_CORR_FACT_A_RES 51064L
#define COL_U_MASS_CORR_FACT_A_ERR_RES 51065L
#define COL_U_MASS_CORR_FACT_B_RES 51066L
#define COL_U_MASS_CORR_FACT_B_ERR_RES 51067L
#define COL_SAMPLE_CORR_FACT_RES 51068L
#define COL_SAMPLE_CORR_FACT_ERR_RES 51069L
#define COL_REFERENCE_DATE_RES 51070L
#define COL_RELATIVE_DOUBLES_RATE_RES 51071L
#define COL_SPARES_RES 51072L
#define COLLAR_K5_LABEL_RES 51073L
#define COLLAR_K5_CHECKBOX_RES 51074L
#define COLLAR_K5_RES 51075L
#define COLLAR_K5_ERR_RES 51076L
#define AD_DZERO_CF252_DOUBLES 52000L
#define AD_SAMPLE_CF252_DOUBLES 52001L
#define AD_SAMPLE_CF252_DOUBLES_ERR 52002L
#define AD_SAMPLE_CF252_RATIO 52003L
#define AD_SAMPLE_AVG_CF252_DOUBLES 52004L
#define AD_SAMPLE_AVG_CF252_DOUBLES_ERR 52005L
#define AD_CORR_DOUBLES 52006L
#define AD_CORR_DOUBLES_ERR 52007L
#define AD_DELTA 52008L
#define AD_DELTA_ERR 52009L
#define AD_CORR_FACTOR 52010L
#define AD_CORR_FACTOR_ERR 52011L
#define AD_PU240E_MASS 52012L
#define AD_PU240E_MASS_ERR 52013L
#define AD_PU_MASS 52014L
#define AD_PU_MASS_ERR 52015L
#define AD_DCL_PU240E_MASS 52016L
#define AD_DCL_PU_MASS 52017L
#define AD_DCL_MINUS_ASY_PU_MASS 52018L
#define AD_DCL_MINUS_ASY_PU_MASS_ERR 52019L
#define AD_DCL_MINUS_ASY_PU_MASS_PCT 52020L
#define AD_PASS_FAIL 52021L
#define AD_TM_DOUBLES_BKG 52022L
#define AD_TM_DOUBLES_BKG_ERR 52023L
#define AD_TM_UNCORR_DOUBLES 52024L
#define AD_TM_UNCORR_DOUBLES_ERR 52025L
#define AD_TM_CORR_DOUBLES 52026L
#define AD_TM_CORR_DOUBLES_ERR 52027L
#define AD_SPARES 52028L
#define AD_ADD_A_SOURCE_EQUATION 52029L
#define AD_A_RES 52030L
#define AD_B_RES 52031L
#define AD_C_RES 52032L
#define AD_D_RES 52033L
#define AD_VAR_A_RES 52034L
#define AD_VAR_B_RES 52035L
#define AD_VAR_C_RES 52036L
#define AD_VAR_D_RES 52037L
#define AD_COVAR_AB_RES 52038L
#define AD_COVAR_AC_RES 52039L
#define AD_COVAR_AD_RES 52040L
#define AD_COVAR_BC_RES 52041L
#define AD_COVAR_BD_RES 52042L
#define AD_COVAR_CD_RES 52043L
#define AD_SIGMA_X_RES 52044L
#define AD_SPARES_RES 52045L
#define AD_POSITION_DZERO_RES 52046L
#define AD_DZERO_AVG_RES 52047L
#define AD_DZERO_REF_DATE_RES 52048L
#define AD_NUM_RUNS_RES 52049L
#define AD_CF_A_RES 52050L
#define AD_CF_B_RES 52051L
#define AD_CF_C_RES 52052L
#define AD_CF_D_RES 52053L
#define AD_USE_TRUNCATED_MULT_RES 52054L
#define AD_TM_WEIGHTING_FACTOR_RES 52055L
#define AD_TM_DBLS_RATE_UPPER_LIMIT_RES 52056L
#define AD_CF_SPARES_RES 52057L
#define AD_ADD_A_SOURCE_ITEM_TYPE 52058L
#define AD_ADD_A_SOURCE_DETECTOR_ID 52059L
#define AM_MULT 53000L
#define AM_MULT_ERR 53001L
#define AM_SPARES 53002L
#define AM_VT1_RES 53003L
#define AM_VT2_RES 53004L
#define AM_VT3_RES 53005L
#define AM_VF1_RES 53006L
#define AM_VF2_RES 53007L
#define AM_VF3_RES 53008L
#define AM_SPARES_RES 53009L
#define AM_MULT_ITEM_TYPE 53010L
#define AM_MULT_DETECTOR_ID 53011L
#define CR_PU240E_MASS 54000L
#define CR_PU240E_MASS_ERR 54001L
#define CR_CM_MASS 54002L
#define CR_CM_MASS_ERR 54003L
#define CR_PU_MASS 54004L
#define CR_PU_MASS_ERR 54005L
#define CR_U_MASS 54006L
#define CR_U_MASS_ERR 54007L
#define CR_U235_MASS 54008L
#define CR_U235_MASS_ERR 54009L
#define CR_DCL_PU_MASS 54010L
#define CR_DCL_MINUS_ASY_PU_MASS 54011L
#define CR_DCL_MINUS_ASY_PU_MASS_ERR 54012L
#define CR_DCL_MINUS_ASY_PU_MASS_PCT 54013L
#define CR_DCL_MINUS_ASY_U_MASS 54014L
#define CR_DCL_MINUS_ASY_U_MASS_ERR 54015L
#define CR_DCL_MINUS_ASY_U_MASS_PCT 54016L
#define CR_DCL_MINUS_ASY_U235_MASS 54017L
#define CR_DCL_MINUS_ASY_U235_MASS_ERR 54018L
#define CR_DCL_MINUS_ASY_U235_MASS_PCT 54019L
#define CR_PU_PASS_FAIL 54020L
#define CR_U_PASS_FAIL 54021L
#define CR_CM_PU_RATIO 54022L
#define CR_CM_PU_RATIO_ERR 54023L
#define CR_PU_HALF_LIFE 54024L
#define CR_CM_PU_RATIO_DATE 54025L
#define CR_CM_U_RATIO 54026L
#define CR_CM_U_RATIO_ERR 54027L
#define CR_CM_U_RATIO_DATE 54028L
#define CR_CM_ID_LABEL 54029L
#define CR_CM_ID 54030L
#define CR_CM_INPUT_BATCH_ID 54031L
#define CR_DCL_U_MASS_RES 54032L
#define CR_DCL_U235_MASS_RES 54033L
#define CR_CM_PU_RATIO_DECAY_CORR 54034L
#define CR_CM_PU_RATIO_DECAY_CORR_ERR 54035L
#define CR_CM_U_RATIO_DECAY_CORR 54036L
#define CR_CM_U_RATIO_DECAY_CORR_ERR 54037L
#define CR_SPARES 54038L
#define CR_CURIUM_RATIO_EQUATION 54039L
#define CR_A_RES 54040L
#define CR_B_RES 54041L
#define CR_C_RES 54042L
#define CR_D_RES 54043L
#define CR_VAR_A_RES 54044L
#define CR_VAR_B_RES 54045L
#define CR_VAR_C_RES 54046L
#define CR_VAR_D_RES 54047L
#define CR_COVAR_AB_RES 54048L
#define CR_COVAR_AC_RES 54049L
#define CR_COVAR_AD_RES 54050L
#define CR_COVAR_BC_RES 54051L
#define CR_COVAR_BD_RES 54052L
#define CR_COVAR_CD_RES 54053L
#define CR_SIGMA_X_RES 54054L
#define CURIUM_RATIO_TYPE_RES 54055L
#define CR_SPARES_RES 54056L
#define CR_CURIUM_RATIO_ITEM_TYPE 54057L
#define CR_CURIUM_RATIO_DETECTOR_ID 54058L
#define TM_BKG_SINGLES 55000L
#define TM_BKG_SINGLES_ERR 55001L
#define TM_BKG_ZEROS 55002L
#define TM_BKG_ZEROS_ERR 55003L
#define TM_BKG_ONES 55004L
#define TM_BKG_ONES_ERR 55005L
#define TM_BKG_TWOS 55006L
#define TM_BKG_TWOS_ERR 55007L
#define TM_NET_SINGLES 55008L
#define TM_NET_SINGLES_ERR 55009L
#define TM_NET_ZEROS 55010L
#define TM_NET_ZEROS_ERR 55011L
#define TM_NET_ONES 55012L
#define TM_NET_ONES_ERR 55013L
#define TM_NET_TWOS 55014L
#define TM_NET_TWOS_ERR 55015L
#define TM_K_ALPHA 55016L
#define TM_K_ALPHA_ERR 55017L
#define TM_K_PU240E_MASS 55018L
#define TM_K_PU240E_MASS_ERR 55019L
#define TM_K_PU_MASS 55020L
#define TM_K_PU_MASS_ERR 55021L
#define TM_K_DCL_PU240E_MASS 55022L
#define TM_K_DCL_PU_MASS 55023L
#define TM_K_DCL_MINUS_ASY_PU_MASS 55024L
#define TM_K_DCL_MINUS_ASY_PU_MASS_ERR 55025L
#define TM_K_DCL_MINUS_ASY_PU_MASS_PCT 55026L
#define TM_K_PASS_FAIL 55027L
#define TM_S_EFF 55028L
#define TM_S_EFF_ERR 55029L
#define TM_S_ALPHA 55030L
#define TM_S_ALPHA_ERR 55031L
#define TM_S_PU240E_MASS 55032L
#define TM_S_PU240E_MASS_ERR 55033L
#define TM_S_PU_MASS 55034L
#define TM_S_PU_MASS_ERR 55035L
#define TM_S_DCL_PU240E_MASS 55036L
#define TM_S_DCL_PU_MASS 55037L
#define TM_S_DCL_MINUS_ASY_PU_MASS 55038L
#define TM_S_DCL_MINUS_ASY_PU_MASS_ERR 55039L
#define TM_S_DCL_MINUS_ASY_PU_MASS_PCT 55040L
#define TM_S_PASS_FAIL 55041L
#define TM_SPARES 55042L
#define TM_A_RES 55043L
#define TM_B_RES 55044L
#define TM_KNOWN_EFF_RES 55045L
#define TM_SOLVE_EFF_RES 55046L
#define TM_SPARES_RES 55047L
#define TM_TRUNCATED_MULT_ITEM_TYPE 55048L
#define TM_TRUNCATED_MULT_DETECTOR_ID 55049L
#define RESULTS_TM_SINGLES_BKG 56000L
#define RESULTS_TM_SINGLES_BKG_ERR 56001L
#define RESULTS_TM_ZEROS_BKG 56002L
#define RESULTS_TM_ZEROS_BKG_ERR 56003L
#define RESULTS_TM_ONES_BKG 56004L
#define RESULTS_TM_ONES_BKG_ERR 56005L
#define RESULTS_TM_TWOS_BKG 56006L
#define RESULTS_TM_TWOS_BKG_ERR 56007L
#define RUN_NUMBER 57000L
#define RUN_DATE 57001L
#define RUN_TIME 57002L
#define RUN_TESTS 57003L
#define RUN_COUNT_TIME 57004L
#define RUN_SINGLES 57005L
#define RUN_SCALER1 57006L
#define RUN_SCALER2 57007L
#define RUN_REALS_PLUS_ACC 57008L
#define RUN_ACC 57009L
#define RUN_MULT_REALS_PLUS_ACC 57010L
#define RUN_MULT_ACC 57011L
#define RUN_SINGLES_RATE 57012L
#define RUN_DOUBLES_RATE 57013L
#define RUN_TRIPLES_RATE 57014L
#define RUN_SCALER1_RATE 57015L
#define RUN_SCALER2_RATE 57016L
#define RUN_MULTIPLICITY_MULT 57017L
#define RUN_MULTIPLICITY_ALPHA 57018L
#define RUN_MULTIPLICITY_EFFICIENCY 57019L
#define RUN_MASS 57020L
#define RUN_HIGH_VOLTAGE 57021L
#define RUN_SPARE 57022L
#define CF1_RUN_NUMBER 58000L
#define CF1_RUN_DATE 58001L
#define CF1_RUN_TIME 58002L
#define CF1_RUN_TESTS 58003L
#define CF1_RUN_COUNT_TIME 58004L
#define CF1_RUN_SINGLES 58005L
#define CF1_RUN_SCALER1 58006L
#define CF1_RUN_SCALER2 58007L
#define CF1_RUN_REALS_PLUS_ACC 58008L
#define CF1_RUN_ACC 58009L
#define CF1_RUN_MULT_REALS_PLUS_ACC 58010L
#define CF1_RUN_MULT_ACC 58011L
#define CF1_RUN_SINGLES_RATE 58012L
#define CF1_RUN_DOUBLES_RATE 58013L
#define CF1_RUN_TRIPLES_RATE 58014L
#define CF1_RUN_SCALER1_RATE 58015L
#define CF1_RUN_SCALER2_RATE 58016L
#define CF1_RUN_MULTIPLICITY_MULT 58017L
#define CF1_RUN_MULTIPLICITY_ALPHA 58018L
#define CF1_MULTIPLICITY_EFFICIENCY 58019L
#define CF1_RUN_MASS 58020L
#define CF1_HIGH_VOLTAGE 58021L
#define CF1_SPARE 58022L
#define CF2_RUN_NUMBER 59000L
#define CF2_RUN_DATE 59001L
#define CF2_RUN_TIME 59002L
#define CF2_RUN_TESTS 59003L
#define CF2_RUN_COUNT_TIME 59004L
#define CF2_RUN_SINGLES 59005L
#define CF2_RUN_SCALER1 59006L
#define CF2_RUN_SCALER2 59007L
#define CF2_RUN_REALS_PLUS_ACC 59008L
#define CF2_RUN_ACC 59009L
#define CF2_RUN_MULT_REALS_PLUS_ACC 59010L
#define CF2_RUN_MULT_ACC 59011L
#define CF2_RUN_SINGLES_RATE 59012L
#define CF2_RUN_DOUBLES_RATE 59013L
#define CF2_RUN_TRIPLES_RATE 59014L
#define CF2_RUN_SCALER1_RATE 59015L
#define CF2_RUN_SCALER2_RATE 59016L
#define CF2_RUN_MULTIPLICITY_MULT 59017L
#define CF2_RUN_MULTIPLICITY_ALPHA 59018L
#define CF2_MULTIPLICITY_EFFICIENCY 59019L
#define CF2_RUN_MASS 59020L
#define CF2_HIGH_VOLTAGE 59021L
#define CF2_SPARE 59022L
#define CF3_RUN_NUMBER 60000L
#define CF3_RUN_DATE 60001L
#define CF3_RUN_TIME 60002L
#define CF3_RUN_TESTS 60003L
#define CF3_RUN_COUNT_TIME 60004L
#define CF3_RUN_SINGLES 60005L
#define CF3_RUN_SCALER1 60006L
#define CF3_RUN_SCALER2 60007L
#define CF3_RUN_REALS_PLUS_ACC 60008L
#define CF3_RUN_ACC 60009L
#define CF3_RUN_MULT_REALS_PLUS_ACC 60010L
#define CF3_RUN_MULT_ACC 60011L
#define CF3_RUN_SINGLES_RATE 60012L
#define CF3_RUN_DOUBLES_RATE 60013L
#define CF3_RUN_TRIPLES_RATE 60014L
#define CF3_RUN_SCALER1_RATE 60015L
#define CF3_RUN_SCALER2_RATE 60016L
#define CF3_RUN_MULTIPLICITY_MULT 60017L
#define CF3_RUN_MULTIPLICITY_ALPHA 60018L
#define CF3_MULTIPLICITY_EFFICIENCY 60019L
#define CF3_RUN_MASS 60020L
#define CF3_HIGH_VOLTAGE 60021L
#define CF3_SPARE 60022L
#define CF4_RUN_NUMBER 61000L
#define CF4_RUN_DATE 61001L
#define CF4_RUN_TIME 61002L
#define CF4_RUN_TESTS 61003L
#define CF4_RUN_COUNT_TIME 61004L
#define CF4_RUN_SINGLES 61005L
#define CF4_RUN_SCALER1 61006L
#define CF4_RUN_SCALER2 61007L
#define CF4_RUN_REALS_PLUS_ACC 61008L
#define CF4_RUN_ACC 61009L
#define CF4_RUN_MULT_REALS_PLUS_ACC 61010L
#define CF4_RUN_MULT_ACC 61011L
#define CF4_RUN_SINGLES_RATE 61012L
#define CF4_RUN_DOUBLES_RATE 61013L
#define CF4_RUN_TRIPLES_RATE 61014L
#define CF4_RUN_SCALER1_RATE 61015L
#define CF4_RUN_SCALER2_RATE 61016L
#define CF4_RUN_MULTIPLICITY_MULT 61017L
#define CF4_RUN_MULTIPLICITY_ALPHA 61018L
#define CF4_MULTIPLICITY_EFFICIENCY 61019L
#define CF4_RUN_MASS 61020L
#define CF4_HIGH_VOLTAGE 61021L
#define CF4_SPARE 61022L
#define CF5_RUN_NUMBER 62000L
#define CF5_RUN_DATE 62001L
#define CF5_RUN_TIME 62002L
#define CF5_RUN_TESTS 62003L
#define CF5_RUN_COUNT_TIME 62004L
#define CF5_RUN_SINGLES 62005L
#define CF5_RUN_SCALER1 62006L
#define CF5_RUN_SCALER2 62007L
#define CF5_RUN_REALS_PLUS_ACC 62008L
#define CF5_RUN_ACC 62009L
#define CF5_RUN_MULT_REALS_PLUS_ACC 62010L
#define CF5_RUN_MULT_ACC 62011L
#define CF5_RUN_SINGLES_RATE 62012L
#define CF5_RUN_DOUBLES_RATE 62013L
#define CF5_RUN_TRIPLES_RATE 62014L
#define CF5_RUN_SCALER1_RATE 62015L
#define CF5_RUN_SCALER2_RATE 62016L
#define CF5_RUN_MULTIPLICITY_MULT 62017L
#define CF5_RUN_MULTIPLICITY_ALPHA 62018L
#define CF5_MULTIPLICITY_EFFICIENCY 62019L
#define CF5_RUN_MASS 62020L
#define CF5_HIGH_VOLTAGE 62021L
#define CF5_SPARE 62022L
#define A1_RUN_NUMBER 63000L
#define A1_RUN_DATE 63001L
#define A1_RUN_TIME 63002L
#define A1_RUN_TESTS 63003L
#define A1_RUN_COUNT_TIME 63004L
#define A1_RUN_SINGLES 63005L
#define A1_RUN_SCALER1 63006L
#define A1_RUN_SCALER2 63007L
#define A1_RUN_REALS_PLUS_ACC 63008L
#define A1_RUN_ACC 63009L
#define A1_RUN_MULT_REALS_PLUS_ACC 63010L
#define A1_RUN_MULT_ACC 63011L
#define A1_RUN_SINGLES_RATE 63012L
#define A1_RUN_DOUBLES_RATE 63013L
#define A1_RUN_TRIPLES_RATE 63014L
#define A1_RUN_SCALER1_RATE 63015L
#define A1_RUN_SCALER2_RATE 63016L
#define A1_RUN_MULTIPLICITY_MULT 63017L
#define A1_RUN_MULTIPLICITY_ALPHA 63018L
#define A1_MULTIPLICITY_EFFICIENCY 63019L
#define A1_RUN_MASS 63020L
#define A1_HIGH_VOLTAGE 63021L
#define A1_SPARE 63022L
#define A2_RUN_NUMBER 64000L
#define A2_RUN_DATE 64001L
#define A2_RUN_TIME 64002L
#define A2_RUN_TESTS 64003L
#define A2_RUN_COUNT_TIME 64004L
#define A2_RUN_SINGLES 64005L
#define A2_RUN_SCALER1 64006L
#define A2_RUN_SCALER2 64007L
#define A2_RUN_REALS_PLUS_ACC 64008L
#define A2_RUN_ACC 64009L
#define A2_RUN_MULT_REALS_PLUS_ACC 64010L
#define A2_RUN_MULT_ACC 64011L
#define A2_RUN_SINGLES_RATE 64012L
#define A2_RUN_DOUBLES_RATE 64013L
#define A2_RUN_TRIPLES_RATE 64014L
#define A2_RUN_SCALER1_RATE 64015L
#define A2_RUN_SCALER2_RATE 64016L
#define A2_RUN_MULTIPLICITY_MULT 64017L
#define A2_RUN_MULTIPLICITY_ALPHA 64018L
#define A2_MULTIPLICITY_EFFICIENCY 64019L
#define A2_RUN_MASS 64020L
#define A2_HIGH_VOLTAGE 64021L
#define A2_SPARE 64022L
#define A3_RUN_NUMBER 65000L
#define A3_RUN_DATE 65001L
#define A3_RUN_TIME 65002L
#define A3_RUN_TESTS 65003L
#define A3_RUN_COUNT_TIME 65004L
#define A3_RUN_SINGLES 65005L
#define A3_RUN_SCALER1 65006L
#define A3_RUN_SCALER2 65007L
#define A3_RUN_REALS_PLUS_ACC 65008L
#define A3_RUN_ACC 65009L
#define A3_RUN_MULT_REALS_PLUS_ACC 65010L
#define A3_RUN_MULT_ACC 65011L
#define A3_RUN_SINGLES_RATE 65012L
#define A3_RUN_DOUBLES_RATE 65013L
#define A3_RUN_TRIPLES_RATE 65014L
#define A3_RUN_SCALER1_RATE 65015L
#define A3_RUN_SCALER2_RATE 65016L
#define A3_RUN_MULTIPLICITY_MULT 65017L
#define A3_RUN_MULTIPLICITY_ALPHA 65018L
#define A3_MULTIPLICITY_EFFICIENCY 65019L
#define A3_RUN_MASS 65020L
#define A3_HIGH_VOLTAGE 65021L
#define A3_SPARE 65022L
#define A4_RUN_NUMBER 66000L
#define A4_RUN_DATE 66001L
#define A4_RUN_TIME 66002L
#define A4_RUN_TESTS 66003L
#define A4_RUN_COUNT_TIME 66004L
#define A4_RUN_SINGLES 66005L
#define A4_RUN_SCALER1 66006L
#define A4_RUN_SCALER2 66007L
#define A4_RUN_REALS_PLUS_ACC 66008L
#define A4_RUN_ACC 66009L
#define A4_RUN_MULT_REALS_PLUS_ACC 66010L
#define A4_RUN_MULT_ACC 66011L
#define A4_RUN_SINGLES_RATE 66012L
#define A4_RUN_DOUBLES_RATE 66013L
#define A4_RUN_TRIPLES_RATE 66014L
#define A4_RUN_SCALER1_RATE 66015L
#define A4_RUN_SCALER2_RATE 66016L
#define A4_RUN_MULTIPLICITY_MULT 66017L
#define A4_RUN_MULTIPLICITY_ALPHA 66018L
#define A4_MULTIPLICITY_EFFICIENCY 66019L
#define A4_RUN_MASS 66020L
#define A4_HIGH_VOLTAGE 66021L
#define A4_SPARE 66022L
#define A5_RUN_NUMBER 67000L
#define A5_RUN_DATE 67001L
#define A5_RUN_TIME 67002L
#define A5_RUN_TESTS 67003L
#define A5_RUN_COUNT_TIME 67004L
#define A5_RUN_SINGLES 67005L
#define A5_RUN_SCALER1 67006L
#define A5_RUN_SCALER2 67007L
#define A5_RUN_REALS_PLUS_ACC 67008L
#define A5_RUN_ACC 67009L
#define A5_RUN_MULT_REALS_PLUS_ACC 67010L
#define A5_RUN_MULT_ACC 67011L
#define A5_RUN_SINGLES_RATE 67012L
#define A5_RUN_DOUBLES_RATE 67013L
#define A5_RUN_TRIPLES_RATE 67014L
#define A5_RUN_SCALER1_RATE 67015L
#define A5_RUN_SCALER2_RATE 67016L
#define A5_RUN_MULTIPLICITY_MULT 67017L
#define A5_RUN_MULTIPLICITY_ALPHA 67018L
#define A5_MULTIPLICITY_EFFICIENCY 67019L
#define A5_RUN_MASS 67020L
#define A5_HIGH_VOLTAGE 67021L
#define A5_SPARE 67022L
#define A6_RUN_NUMBER 68000L
#define A6_RUN_DATE 68001L
#define A6_RUN_TIME 68002L
#define A6_RUN_TESTS 68003L
#define A6_RUN_COUNT_TIME 68004L
#define A6_RUN_SINGLES 68005L
#define A6_RUN_SCALER1 68006L
#define A6_RUN_SCALER2 68007L
#define A6_RUN_REALS_PLUS_ACC 68008L
#define A6_RUN_ACC 68009L
#define A6_RUN_MULT_REALS_PLUS_ACC 68010L
#define A6_RUN_MULT_ACC 68011L
#define A6_RUN_SINGLES_RATE 68012L
#define A6_RUN_DOUBLES_RATE 68013L
#define A6_RUN_TRIPLES_RATE 68014L
#define A6_RUN_SCALER1_RATE 68015L
#define A6_RUN_SCALER2_RATE 68016L
#define A6_RUN_MULTIPLICITY_MULT 68017L
#define A6_RUN_MULTIPLICITY_ALPHA 68018L
#define A6_MULTIPLICITY_EFFICIENCY 68019L
#define A6_RUN_MASS 68020L
#define A6_HIGH_VOLTAGE 68021L
#define A6_SPARE 68022L
#define B1_RUN_NUMBER 69000L
#define B1_RUN_DATE 69001L
#define B1_RUN_TIME 69002L
#define B1_RUN_TESTS 69003L
#define B1_RUN_COUNT_TIME 69004L
#define B1_RUN_SINGLES 69005L
#define B1_RUN_SCALER1 69006L
#define B1_RUN_SCALER2 69007L
#define B1_RUN_REALS_PLUS_ACC 69008L
#define B1_RUN_ACC 69009L
#define B1_RUN_MULT_REALS_PLUS_ACC 69010L
#define B1_RUN_MULT_ACC 69011L
#define B1_RUN_SINGLES_RATE 69012L
#define B1_RUN_DOUBLES_RATE 69013L
#define B1_RUN_TRIPLES_RATE 69014L
#define B1_RUN_SCALER1_RATE 69015L
#define B1_RUN_SCALER2_RATE 69016L
#define B1_RUN_MULTIPLICITY_MULT 69017L
#define B1_RUN_MULTIPLICITY_ALPHA 69018L
#define B1_MULTIPLICITY_EFFICIENCY 69019L
#define B1_RUN_MASS 69020L
#define B1_HIGH_VOLTAGE 69021L
#define B1_SPARE 69022L
#define B2_RUN_NUMBER 70000L
#define B2_RUN_DATE 70001L
#define B2_RUN_TIME 70002L
#define B2_RUN_TESTS 70003L
#define B2_RUN_COUNT_TIME 70004L
#define B2_RUN_SINGLES 70005L
#define B2_RUN_SCALER1 70006L
#define B2_RUN_SCALER2 70007L
#define B2_RUN_REALS_PLUS_ACC 70008L
#define B2_RUN_ACC 70009L
#define B2_RUN_MULT_REALS_PLUS_ACC 70010L
#define B2_RUN_MULT_ACC 70011L
#define B2_RUN_SINGLES_RATE 70012L
#define B2_RUN_DOUBLES_RATE 70013L
#define B2_RUN_TRIPLES_RATE 70014L
#define B2_RUN_SCALER1_RATE 70015L
#define B2_RUN_SCALER2_RATE 70016L
#define B2_RUN_MULTIPLICITY_MULT 70017L
#define B2_RUN_MULTIPLICITY_ALPHA 70018L
#define B2_MULTIPLICITY_EFFICIENCY 70019L
#define B2_RUN_MASS 70020L
#define B2_HIGH_VOLTAGE 70021L
#define B2_SPARE 70022L
#define B3_RUN_NUMBER 71000L
#define B3_RUN_DATE 71001L
#define B3_RUN_TIME 71002L
#define B3_RUN_TESTS 71003L
#define B3_RUN_COUNT_TIME 71004L
#define B3_RUN_SINGLES 71005L
#define B3_RUN_SCALER1 71006L
#define B3_RUN_SCALER2 71007L
#define B3_RUN_REALS_PLUS_ACC 71008L
#define B3_RUN_ACC 71009L
#define B3_RUN_MULT_REALS_PLUS_ACC 71010L
#define B3_RUN_MULT_ACC 71011L
#define B3_RUN_SINGLES_RATE 71012L
#define B3_RUN_DOUBLES_RATE 71013L
#define B3_RUN_TRIPLES_RATE 71014L
#define B3_RUN_SCALER1_RATE 71015L
#define B3_RUN_SCALER2_RATE 71016L
#define B3_RUN_MULTIPLICITY_MULT 71017L
#define B3_RUN_MULTIPLICITY_ALPHA 71018L
#define B3_MULTIPLICITY_EFFICIENCY 71019L
#define B3_RUN_MASS 71020L
#define B3_HIGH_VOLTAGE 71021L
#define B3_SPARE 71022L
#define B4_RUN_NUMBER 72000L
#define B4_RUN_DATE 72001L
#define B4_RUN_TIME 72002L
#define B4_RUN_TESTS 72003L
#define B4_RUN_COUNT_TIME 72004L
#define B4_RUN_SINGLES 72005L
#define B4_RUN_SCALER1 72006L
#define B4_RUN_SCALER2 72007L
#define B4_RUN_REALS_PLUS_ACC 72008L
#define B4_RUN_ACC 72009L
#define B4_RUN_MULT_REALS_PLUS_ACC 72010L
#define B4_RUN_MULT_ACC 72011L
#define B4_RUN_SINGLES_RATE 72012L
#define B4_RUN_DOUBLES_RATE 72013L
#define B4_RUN_TRIPLES_RATE 72014L
#define B4_RUN_SCALER1_RATE 72015L
#define B4_RUN_SCALER2_RATE 72016L
#define B4_RUN_MULTIPLICITY_MULT 72017L
#define B4_RUN_MULTIPLICITY_ALPHA 72018L
#define B4_MULTIPLICITY_EFFICIENCY 72019L
#define B4_RUN_MASS 72020L
#define B4_HIGH_VOLTAGE 72021L
#define B4_SPARE 72022L
#define B5_RUN_NUMBER 73000L
#define B5_RUN_DATE 73001L
#define B5_RUN_TIME 73002L
#define B5_RUN_TESTS 73003L
#define B5_RUN_COUNT_TIME 73004L
#define B5_RUN_SINGLES 73005L
#define B5_RUN_SCALER1 73006L
#define B5_RUN_SCALER2 73007L
#define B5_RUN_REALS_PLUS_ACC 73008L
#define B5_RUN_ACC 73009L
#define B5_RUN_MULT_REALS_PLUS_ACC 73010L
#define B5_RUN_MULT_ACC 73011L
#define B5_RUN_SINGLES_RATE 73012L
#define B5_RUN_DOUBLES_RATE 73013L
#define B5_RUN_TRIPLES_RATE 73014L
#define B5_RUN_SCALER1_RATE 73015L
#define B5_RUN_SCALER2_RATE 73016L
#define B5_RUN_MULTIPLICITY_MULT 73017L
#define B5_RUN_MULTIPLICITY_ALPHA 73018L
#define B5_MULTIPLICITY_EFFICIENCY 73019L
#define B5_RUN_MASS 73020L
#define B5_HIGH_VOLTAGE 73021L
#define B5_SPARE 73022L
#define B6_RUN_NUMBER 74000L
#define B6_RUN_DATE 74001L
#define B6_RUN_TIME 74002L
#define B6_RUN_TESTS 74003L
#define B6_RUN_COUNT_TIME 74004L
#define B6_RUN_SINGLES 74005L
#define B6_RUN_SCALER1 74006L
#define B6_RUN_SCALER2 74007L
#define B6_RUN_REALS_PLUS_ACC 74008L
#define B6_RUN_ACC 74009L
#define B6_RUN_MULT_REALS_PLUS_ACC 74010L
#define B6_RUN_MULT_ACC 74011L
#define B6_RUN_SINGLES_RATE 74012L
#define B6_RUN_DOUBLES_RATE 74013L
#define B6_RUN_TRIPLES_RATE 74014L
#define B6_RUN_SCALER1_RATE 74015L
#define B6_RUN_SCALER2_RATE 74016L
#define B6_RUN_MULTIPLICITY_MULT 74017L
#define B6_RUN_MULTIPLICITY_ALPHA 74018L
#define B6_MULTIPLICITY_EFFICIENCY 74019L
#define B6_RUN_MASS 74020L
#define B6_HIGH_VOLTAGE 74021L
#define B6_SPARE 74022L
#define AD_TYPE 75000L
#define AD_PORT_NUMBER 75001L
#define AD_FORWARD_OVER_TRAVEL 75002L
#define AD_REVERSE_OVER_TRAVEL 75003L
#define AD_NUMBER_POSITIONS 75004L
#define AD_DIST_TO_MOVE 75005L
#define CM_STEPS_PER_INCH 75006L
#define CM_FORWARD_MASK 75007L
#define CM_REVERSE_MASK 75008L
#define CM_AXIS_NUMBER 75009L
#define CM_OVER_TRAVEL_STATE 75010L
#define CM_STEP_RATIO 75011L
#define CM_SLOW_INCHES 75012L
#define PLC_STEPS_PER_INCH 75013L
#define SCALE_CONVERSION_FACTOR 75014L
#define CM_ROTATION 75015L
#define STRATUM 76000L
#define STRATUM_ID_DETECTOR_ID 76001L
#define STRATUM_BIAS_UNCERTAINTY 76002L
#define STRATUM_RANDOM_UNCERTAINTY 76003L
#define STRATUM_SYSTEMATIC_UNCERTAINTY 76004L
#define STRATUM_ID_KEY 76005L
#define DE_NEUTRON_ENERGY 77000L
#define DE_DETECTOR_EFFICIENCY 77001L
#define DE_INNER_OUTER_RING_RATIO 77002L
#define DE_RELATIVE_FISSION 77003L
#define DE_INNER_RING_EFFICIENCY 77004L
#define DE_OUTER_RING_EFFICIENCY 77005L
#define DE_ITEM_TYPE 77006L
#define DE_DETECTOR_ID 77007L
#define ANALYSIS_METHOD_DE_MULT 77008L
#define DE_MEAS_RING_RATIO 78000L
#define DE_INTERPOLATED_NEUTRON_ENERGY 78001L
#define DE_ENERGY_CORR_FACTOR 78002L
#define DE_NEUTRON_ENERGY_RES 78003L
#define DE_DETECTOR_EFFICIENCY_RES 78004L
#define DE_INNER_OUTER_RING_RATIO_RES 78005L
#define DE_RELATIVE_FISSION_RES 78006L
#define DE_INNER_RING_EFFICIENCY_RES 78007L
#define DE_OUTER_RING_EFFICIENCY_RES 78008L
#define DE_MULT_ITEM_TYPE 78009L
#define DE_MULT_DETECTOR_ID 78010L

/* Set Name Constants */
#define ARCHIVE_PARMS_SET 20000
#define TEST_PARMS_SET 20001
#define ACQUIRE_PARMS_SET 20002
#define ITEM_ID_ENTRY_SET 20003
#define MBA_ITEM_ID_ENTRY_SET 20004
#define COLLAR_DATA_ENTRY_SET 20005
#define POISON_ROD_TYPE_SET 20006
#define HOLDUP_CONFIG_SET 20007
#define CM_PU_RATIO_SET 20008
#define ITEM_TYPE_NAMES_SET 20009
#define FACILITY_NAMES_SET 20010
#define MBA_NAMES_SET 20011
#define STRATUM_ID_NAMES_SET 20012
#define INVENTORY_CHANGE_CODE_SET 20013
#define IO_CODE_SET 20014
#define ISOTOPICS_SET 20015
#define COMPOSITE_ISOTOPICS_SET 20016
#define ASSAY_SUMMARY_SET 20017
#define HOLDUP_SUMMARY_SET 20018
#define DETECTOR_SET 20019
#define DETECTOR_ALPHA_BETA_SET 20020
#define DETECTOR_BKG_PARMS_SET 20021
#define DETECTOR_TM_BKG_PARMS_SET 20022
#define DETECTOR_NORM_PARMS_SET 20023
#define DETECTOR_SR_PARMS_SET 20024
#define DETECTOR_UNATTENDED_PARMS_SET 20025
#define DETECTOR_RESULTS_SET 20026
#define RESULTS_INIT_SRC_SET 20027
#define RESULTS_BIAS_SET 20028
#define RESULTS_PRECISION_SET 20029
#define RESULTS_CAL_CURVE_SET 20030
#define RESULTS_KNOWN_ALPHA_SET 20031
#define RESULTS_KNOWN_M_SET 20032
#define RESULTS_MULTIPLICITY_SET 20033
#define RESULTS_ACTIVE_PASSIVE_SET 20034
#define RESULTS_ACTIVE_SET 20035
#define RESULTS_COLLAR_SET 20036
#define RESULTS_ADD_A_SOURCE_SET 20037
#define RESULTS_ACTIVE_MULT_SET 20038
#define RESULTS_CURIUM_RATIO_SET 20039
#define RESULTS_TRUNCATED_MULT_SET 20040
#define RESULTS_TM_BKG_SET 20041
#define RESULTS_RUN_SET 20042
#define RESULTS_CF1_RUN_SET 20043
#define RESULTS_CF2_RUN_SET 20044
#define RESULTS_CF3_RUN_SET 20045
#define RESULTS_CF4_RUN_SET 20046
#define RESULTS_CF5_RUN_SET 20047
#define RESULTS_A1_RUN_SET 20048
#define RESULTS_A2_RUN_SET 20049
#define RESULTS_A3_RUN_SET 20050
#define RESULTS_A4_RUN_SET 20051
#define RESULTS_A5_RUN_SET 20052
#define RESULTS_A6_RUN_SET 20053
#define RESULTS_B1_RUN_SET 20054
#define RESULTS_B2_RUN_SET 20055
#define RESULTS_B3_RUN_SET 20056
#define RESULTS_B4_RUN_SET 20057
#define RESULTS_B5_RUN_SET 20058
#define RESULTS_B6_RUN_SET 20059
#define DETECTOR_ADD_A_SOURCE_SETUP_SET 20060
#define RESULTS_DE_MULT_SET 20061

/* Field Sizes */
#define SIZEOF_DAYS_BEFORE_AUTO_ARCHIVE 2
#define SIZEOF_DAYS_BEFORE_AUTO_DELETE 2
#define SIZEOF_DAYS_BEFORE_DB_AUTO_DELETE 2
#define SIZEOF_DATA_DIR 256
#define SIZEOF_ACC_SNGL_TEST_RATE_LIMIT 8
#define SIZEOF_ACC_SNGL_TEST_PRECISION_LIMIT 8
#define SIZEOF_ACC_SNGL_TEST_OUTLIER_LIMIT 8
#define SIZEOF_OUTLIER_TEST_LIMIT 8
#define SIZEOF_BKG_DOUBLES_RATE_LIMIT 8
#define SIZEOF_BKG_TRIPLES_RATE_LIMIT 8
#define SIZEOF_CHISQ_LIMIT 8
#define SIZEOF_MAX_NUM_FAILURES 2
#define SIZEOF_HIGH_VOLTAGE_TEST_LIMIT 8
#define SIZEOF_NORMAL_BACKUP_ASSAY_TEST_LIMIT 8
#define SIZEOF_MAX_RUNS_FOR_OUTLIER_TEST 2
#define SIZEOF_CHECKSUM_TEST 1
#define SIZEOF_ACCIDENTALS_METHOD 2
#define SIZEOF_ACQ_FACILITY 13
#define SIZEOF_ACQ_FACILITY_DESCRIPTION 21
#define SIZEOF_ACQ_MBA 5
#define SIZEOF_ACQ_MBA_DESCRIPTION 21
#define SIZEOF_ACQ_DETECTOR_ID 12
#define SIZEOF_ACQ_ITEM_TYPE 6
#define SIZEOF_ACQ_GLOVEBOX_ID 21
#define SIZEOF_ACQ_ISOTOPICS_ID 13
#define SIZEOF_ACQ_COMP_ISOTOPICS_ID 13
#define SIZEOF_ACQ_CAMPAIGN_ID 13
#define SIZEOF_ACQ_ITEM_ID 13
#define SIZEOF_ACQ_STRATUM_ID 13
#define SIZEOF_ACQ_STRATUM_ID_DESCRIPTION 21
#define SIZEOF_ACQ_USER_ID 41
#define SIZEOF_ACQ_COMMENT 51
#define SIZEOF_ACQ_ENDING_COMMENT 1
#define SIZEOF_ACQ_DATA_SRC 1
#define SIZEOF_ACQ_QC_TESTS 1
#define SIZEOF_ACQ_PRINT 1
#define SIZEOF_ACQ_REVIEW_DETECTOR_PARMS 1
#define SIZEOF_ACQ_REVIEW_CALIB_PARMS 1
#define SIZEOF_ACQ_REVIEW_ISOTOPICS 1
#define SIZEOF_ACQ_REVIEW_SUMMED_RAW_DATA 1
#define SIZEOF_ACQ_REVIEW_RUN_RAW_DATA 1
#define SIZEOF_ACQ_REVIEW_RUN_RATE_DATA 1
#define SIZEOF_ACQ_REVIEW_SUMMED_MULT_DIST 1
#define SIZEOF_ACQ_REVIEW_RUN_MULT_DIST 1
#define SIZEOF_ACQ_RUN_COUNT_TIME 8
#define SIZEOF_ACQ_ACQUIRE_TYPE 2
#define SIZEOF_ACQ_NUM_RUNS 2
#define SIZEOF_ACQ_ACTIVE_NUM_RUNS 2
#define SIZEOF_ACQ_MIN_NUM_RUNS 2
#define SIZEOF_ACQ_MAX_NUM_RUNS 2
#define SIZEOF_ACQ_MEAS_PRECISION 8
#define SIZEOF_ACQ_WELL_CONFIG 1
#define SIZEOF_ACQ_MASS 8
#define SIZEOF_ACQ_ERROR_CALC_METHOD 2
#define SIZEOF_ACQ_INVENTORY_CHANGE_CODE 3
#define SIZEOF_ACQ_IO_CODE 2
#define SIZEOF_ACQ_COLLAR_MODE 1
#define SIZEOF_ACQ_DRUM_EMPTY_WEIGHT 8
#define SIZEOF_ACQ_MEAS_DATE 9
#define SIZEOF_ACQ_MEAS_TIME 9
#define SIZEOF_ACQ_MEAS_DETECTOR_ID 12
#define SIZEOF_ITEM_ID_ENTRY 5096
#define SIZEOF_ITEM_TYPE_ASCII 2352
#define SIZEOF_ISOTOPICS_ID_ASCII 5096
#define SIZEOF_STRATUM_ID_ASCII 5096
#define SIZEOF_INVENTORY_CHANGE_CODE_ENTRY 1176
#define SIZEOF_IO_CODE_ENTRY 784
#define SIZEOF_DECLARED_MASS_ENTRY 3136
#define SIZEOF_DECLARED_U_MASS_ENTRY 3136
#define SIZEOF_LENGTH_ENTRY 3136
#define SIZEOF_MBA_ASCII 1960
#define SIZEOF_COL_ITEM_ID_ENTRY 1300
#define SIZEOF_COL_LENGTH_ENTRY 800
#define SIZEOF_COL_LENGTH_ERR_ENTRY 800
#define SIZEOF_COL_TOTAL_PU_ENTRY 800
#define SIZEOF_COL_TOTAL_PU_ERR_ENTRY 800
#define SIZEOF_COL_DEPLETED_U_ENTRY 800
#define SIZEOF_COL_DEPLETED_U_ERR_ENTRY 800
#define SIZEOF_COL_NATURAL_U_ENTRY 800
#define SIZEOF_COL_NATURAL_U_ERR_ENTRY 800
#define SIZEOF_COL_ENRICHED_U_ENTRY 800
#define SIZEOF_COL_ENRICHED_U_ERR_ENTRY 800
#define SIZEOF_COL_TOTAL_U235_ENTRY 800
#define SIZEOF_COL_TOTAL_U235_ERR_ENTRY 800
#define SIZEOF_COL_TOTAL_U238_ENTRY 800
#define SIZEOF_COL_TOTAL_U238_ERR_ENTRY 800
#define SIZEOF_COL_TOTAL_RODS_ENTRY 800
#define SIZEOF_COL_TOTAL_POISON_RODS_ENTRY 800
#define SIZEOF_COL_POISON_PERCENT_ENTRY 800
#define SIZEOF_COL_POISON_PERCENT_ERR_ENTRY 800
#define SIZEOF_COL_ROD_TYPE_ENTRY 200
#define SIZEOF_POISON_ROD_TYPE 20
#define SIZEOF_POISON_ABSORPTION_FACT 80
#define SIZEOF_GLOVEBOX_ID 21
#define SIZEOF_NUM_ROWS 2
#define SIZEOF_NUM_COLUMNS 2
#define SIZEOF_DISTANCE 8
#define SIZEOF_CM_PU_RATIO 8
#define SIZEOF_CM_PU_RATIO_ERR 8
#define SIZEOF_CM_PU_HALF_LIFE 8
#define SIZEOF_CM_PU_RATIO_DATE 9
#define SIZEOF_CM_U_RATIO 8
#define SIZEOF_CM_U_RATIO_ERR 8
#define SIZEOF_CM_U_RATIO_DATE 9
#define SIZEOF_CM_ID_LABEL 13
#define SIZEOF_CM_ID 13
#define SIZEOF_CM_INPUT_BATCH_ID 13
#define SIZEOF_CM_DCL_U_MASS 8
#define SIZEOF_CM_DCL_U235_MASS 8
#define SIZEOF_ITEM_TYPE_NAMES 600
#define SIZEOF_FACILITY_NAMES 650
#define SIZEOF_FACILITY_DESCRIPTION 1050
#define SIZEOF_MBA_NAMES 250
#define SIZEOF_MBA_DESCRIPTION 1050
#define SIZEOF_STRATUM_ID_NAMES 1300
#define SIZEOF_STRATUM_ID_NAMES_DESCRIPTION 2100
#define SIZEOF_INVENTORY_CHG_CODES 93
#define SIZEOF_IO_CODES 56
#define SIZEOF_PU238 8
#define SIZEOF_PU239 8
#define SIZEOF_PU240 8
#define SIZEOF_PU241 8
#define SIZEOF_PU242 8
#define SIZEOF_AM241 8
#define SIZEOF_PU238_ERR 8
#define SIZEOF_PU239_ERR 8
#define SIZEOF_PU240_ERR 8
#define SIZEOF_PU241_ERR 8
#define SIZEOF_PU242_ERR 8
#define SIZEOF_AM241_ERR 8
#define SIZEOF_PU_DATE 9
#define SIZEOF_AM_DATE 9
#define SIZEOF_ISOTOPICS_ID 13
#define SIZEOF_ISOTOPICS_SOURCE_CODE 3
#define SIZEOF_CI_REF_DATE 9
#define SIZEOF_CI_PU_MASS 160
#define SIZEOF_CI_PU238 160
#define SIZEOF_CI_PU239 160
#define SIZEOF_CI_PU240 160
#define SIZEOF_CI_PU241 160
#define SIZEOF_CI_PU242 160
#define SIZEOF_CI_AM241 160
#define SIZEOF_CI_PU_DATE 180
#define SIZEOF_CI_AM_DATE 180
#define SIZEOF_CI_ISOTOPICS_ID 13
#define SIZEOF_CI_ISOTOPICS_SOURCE_CODE 3
#define SIZEOF_AS_PRINT 1
#define SIZEOF_AS_PATH 256
#define SIZEOF_AS_SELECT 520
#define SIZEOF_HU_PRINT 1
#define SIZEOF_HU_PATH 256
#define SIZEOF_HU_SELECT 200
#define SIZEOF_DETECTOR_ID 12
#define SIZEOF_DETECTOR_TYPE 12
#define SIZEOF_ELECTRONICS_ID 9
#define SIZEOF_FACTORIAL 1024
#define SIZEOF_ALPHA_ARRAY 1024
#define SIZEOF_BETA_ARRAY 1024
#define SIZEOF_CURR_PASSIVE_BKG_SINGLES_RATE 8
#define SIZEOF_CURR_PASSIVE_BKG_SINGLES_ERR 8
#define SIZEOF_CURR_PASSIVE_BKG_DOUBLES_RATE 8
#define SIZEOF_CURR_PASSIVE_BKG_DOUBLES_ERR 8
#define SIZEOF_CURR_PASSIVE_BKG_TRIPLES_RATE 8
#define SIZEOF_CURR_PASSIVE_BKG_TRIPLES_ERR 8
#define SIZEOF_CURR_ACTIVE_BKG_SINGLES_RATE 8
#define SIZEOF_CURR_ACTIVE_BKG_SINGLES_ERR 8
#define SIZEOF_CURR_PASSIVE_BKG_SCALER1_RATE 8
#define SIZEOF_CURR_PASSIVE_BKG_SCALER2_RATE 8
#define SIZEOF_CURR_ACTIVE_BKG_SCALER1_RATE 8
#define SIZEOF_CURR_ACTIVE_BKG_SCALER2_RATE 8
#define SIZEOF_TM_SINGLES_BKG 8
#define SIZEOF_TM_SINGLES_BKG_ERR 8
#define SIZEOF_TM_ZEROS_BKG 8
#define SIZEOF_TM_ZEROS_BKG_ERR 8
#define SIZEOF_TM_ONES_BKG 8
#define SIZEOF_TM_ONES_BKG_ERR 8
#define SIZEOF_TM_TWOS_BKG 8
#define SIZEOF_TM_TWOS_BKG_ERR 8
#define SIZEOF_TM_BKG 1
#define SIZEOF_SOURCE_ID 13
#define SIZEOF_CURR_NORMALIZATION_CONSTANT 8
#define SIZEOF_CURR_NORMALIZATION_CONSTANT_ERR 8
#define SIZEOF_BIAS_MODE 2
#define SIZEOF_MEAS_RATE 8
#define SIZEOF_MEAS_RATE_ERR 8
#define SIZEOF_AMLI_REF_SINGLES_RATE 8
#define SIZEOF_CF252_REF_DOUBLES_RATE 8
#define SIZEOF_CF252_REF_DOUBLES_RATE_ERR 8
#define SIZEOF_REF_DATE 9
#define SIZEOF_INIT_SRC_PRECISION_LIMIT 8
#define SIZEOF_BIAS_PRECISION_LIMIT 8
#define SIZEOF_ACCEPTANCE_LIMIT_STD_DEV 8
#define SIZEOF_ACCEPTANCE_LIMIT_PERCENT 8
#define SIZEOF_YIELD_RELATIVE_TO_MRC_95 8
#define SIZEOF_BIAS_TEST_USE_ADDASRC 1
#define SIZEOF_BIAS_TEST_ADDASRC_POSITION 8
#define SIZEOF_SR_DETECTOR_ID 12
#define SIZEOF_SR_TYPE 2
#define SIZEOF_SR_PORT_NUMBER 2
#define SIZEOF_PREDELAY 8
#define SIZEOF_GATE_LENGTH 8
#define SIZEOF_GATE_LENGTH2 8
#define SIZEOF_HIGH_VOLTAGE 8
#define SIZEOF_DIE_AWAY_TIME 8
#define SIZEOF_EFFICIENCY 8
#define SIZEOF_MULTIPLICITY_DEADTIME 8
#define SIZEOF_COEFF_A_DEADTIME 8
#define SIZEOF_COEFF_B_DEADTIME 8
#define SIZEOF_COEFF_C_DEADTIME 8
#define SIZEOF_DOUBLES_GATE_FRACTION 8
#define SIZEOF_TRIPLES_GATE_FRACTION 8
#define SIZEOF_SR_SPARES 80
#define SIZEOF_ERROR_SECONDS 4
#define SIZEOF_AUTO_IMPORT 1
#define SIZEOF_ADD_A_SOURCE_THRESHOLD 8
#define SIZEOF_ITEM_TYPE 6
#define SIZEOF_ANALYSIS_METHOD_DETECTOR_ID 12
#define SIZEOF_CAL_CURVE 1
#define SIZEOF_KNOWN_ALPHA 1
#define SIZEOF_KNOWN_M 1
#define SIZEOF_MULTIPLICITY 1
#define SIZEOF_ADD_A_SOURCE 1
#define SIZEOF_ACTIVE 1
#define SIZEOF_ACTIVE_MULT 1
#define SIZEOF_ACTIVE_PASSIVE 1
#define SIZEOF_COLLAR 1
#define SIZEOF_NORMAL_METHOD 1
#define SIZEOF_BACKUP_METHOD 1
#define SIZEOF_CURIUM_RATIO 1
#define SIZEOF_TRUNCATED_MULT 1
#define SIZEOF_ANALYSIS_METHOD_SPARE1 1
#define SIZEOF_ANALYSIS_METHOD_SPARE2 1
#define SIZEOF_ANALYSIS_METHOD_SPARE3 1
#define SIZEOF_ANALYSIS_METHOD_SPARE4 1
#define SIZEOF_CAL_CURVE_EQUATION 1
#define SIZEOF_CC_A 8
#define SIZEOF_CC_B 8
#define SIZEOF_CC_C 8
#define SIZEOF_CC_D 8
#define SIZEOF_CC_VAR_A 8
#define SIZEOF_CC_VAR_B 8
#define SIZEOF_CC_VAR_C 8
#define SIZEOF_CC_VAR_D 8
#define SIZEOF_CC_COVAR_AB 8
#define SIZEOF_CC_COVAR_AC 8
#define SIZEOF_CC_COVAR_AD 8
#define SIZEOF_CC_COVAR_BC 8
#define SIZEOF_CC_COVAR_BD 8
#define SIZEOF_CC_COVAR_CD 8
#define SIZEOF_CC_SIGMA_X 8
#define SIZEOF_CC_CAL_CURVE_TYPE 8
#define SIZEOF_CC_HEAVY_METAL_CORR_FACTOR 8
#define SIZEOF_CC_HEAVY_METAL_REFERENCE 8
#define SIZEOF_CC_PERCENT_U235 8
#define SIZEOF_CAL_CURVE_SPARES 48
#define SIZEOF_CAL_CURVE_ITEM_TYPE 6
#define SIZEOF_CAL_CURVE_DETECTOR_ID 12
#define SIZEOF_CC_LOWER_MASS_LIMIT 8
#define SIZEOF_CC_UPPER_MASS_LIMIT 8
#define SIZEOF_CC_DCL_MASS 160
#define SIZEOF_CC_DOUBLES 160
#define SIZEOF_ACTIVE_EQUATION 1
#define SIZEOF_ACT_A 8
#define SIZEOF_ACT_B 8
#define SIZEOF_ACT_C 8
#define SIZEOF_ACT_D 8
#define SIZEOF_ACT_VAR_A 8
#define SIZEOF_ACT_VAR_B 8
#define SIZEOF_ACT_VAR_C 8
#define SIZEOF_ACT_VAR_D 8
#define SIZEOF_ACT_COVAR_AB 8
#define SIZEOF_ACT_COVAR_AC 8
#define SIZEOF_ACT_COVAR_AD 8
#define SIZEOF_ACT_COVAR_BC 8
#define SIZEOF_ACT_COVAR_BD 8
#define SIZEOF_ACT_COVAR_CD 8
#define SIZEOF_ACT_SIGMA_X 8
#define SIZEOF_ACTIVE_SPARES 80
#define SIZEOF_ACTIVE_ITEM_TYPE 6
#define SIZEOF_ACTIVE_DETECTOR_ID 12
#define SIZEOF_ACT_LOWER_MASS_LIMIT 8
#define SIZEOF_ACT_UPPER_MASS_LIMIT 8
#define SIZEOF_ACT_DCL_MASS 160
#define SIZEOF_ACT_DOUBLES 160
#define SIZEOF_COLLAR_EQUATION 1
#define SIZEOF_COL_A 8
#define SIZEOF_COL_B 8
#define SIZEOF_COL_C 8
#define SIZEOF_COL_D 8
#define SIZEOF_COL_VAR_A 8
#define SIZEOF_COL_VAR_B 8
#define SIZEOF_COL_VAR_C 8
#define SIZEOF_COL_VAR_D 8
#define SIZEOF_COL_COVAR_AB 8
#define SIZEOF_COL_COVAR_AC 8
#define SIZEOF_COL_COVAR_AD 8
#define SIZEOF_COL_COVAR_BC 8
#define SIZEOF_COL_COVAR_BD 8
#define SIZEOF_COL_COVAR_CD 8
#define SIZEOF_COL_SIGMA_X 8
#define SIZEOF_COL_NUMBER_CALIB_RODS 8
#define SIZEOF_COL_POISON_ROD_TYPE 20
#define SIZEOF_COL_POISON_ABSORPTION_FACT 80
#define SIZEOF_COL_POISON_ROD_A 80
#define SIZEOF_COL_POISON_ROD_A_ERR 80
#define SIZEOF_COL_POISON_ROD_B 80
#define SIZEOF_COL_POISON_ROD_B_ERR 80
#define SIZEOF_COL_POISON_ROD_C 80
#define SIZEOF_COL_POISON_ROD_C_ERR 80
#define SIZEOF_COL_U_MASS_CORR_FACT_A 8
#define SIZEOF_COL_U_MASS_CORR_FACT_A_ERR 8
#define SIZEOF_COL_U_MASS_CORR_FACT_B 8
#define SIZEOF_COL_U_MASS_CORR_FACT_B_ERR 8
#define SIZEOF_COL_SAMPLE_CORR_FACT 8
#define SIZEOF_COL_SAMPLE_CORR_FACT_ERR 8
#define SIZEOF_COLLAR_SPARES 80
#define SIZEOF_COLLAR_ITEM_TYPE 6
#define SIZEOF_COLLAR_MODE 1
#define SIZEOF_COL_LOWER_MASS_LIMIT 8
#define SIZEOF_COL_UPPER_MASS_LIMIT 8
#define SIZEOF_COLLAR_DETECTOR_ITEM_TYPE 6
#define SIZEOF_COLLAR_DETECTOR_MODE 1
#define SIZEOF_COLLAR_DETECTOR_ID 12
#define SIZEOF_COL_REFERENCE_DATE 9
#define SIZEOF_COL_RELATIVE_DOUBLES_RATE 8
#define SIZEOF_COLLAR_K5_LABEL 620
#define SIZEOF_COLLAR_K5_CHECKBOX 80
#define SIZEOF_COLLAR_K5 160
#define SIZEOF_COLLAR_K5_ERR 160
#define SIZEOF_COLLAR_K5_ITEM_TYPE 6
#define SIZEOF_COLLAR_K5_MODE 1
#define SIZEOF_KA_ALPHA_WT 8
#define SIZEOF_KA_RHO_ZERO 8
#define SIZEOF_KA_K 8
#define SIZEOF_KA_A 8
#define SIZEOF_KA_B 8
#define SIZEOF_KA_VAR_A 8
#define SIZEOF_KA_VAR_B 8
#define SIZEOF_KA_COVAR_AB 8
#define SIZEOF_KA_SIGMA_X 8
#define SIZEOF_KA_KNOWN_ALPHA_TYPE 8
#define SIZEOF_KA_HEAVY_METAL_CORR_FACTOR 8
#define SIZEOF_KA_HEAVY_METAL_REFERENCE 8
#define SIZEOF_KA_RING_RATIO_EQUATION 8
#define SIZEOF_KA_RING_RATIO_A 8
#define SIZEOF_KA_RING_RATIO_B 8
#define SIZEOF_KA_RING_RATIO_C 8
#define SIZEOF_KA_RING_RATIO_D 8
#define SIZEOF_KA_LOWER_CORR_FACTOR_LIMIT 8
#define SIZEOF_KA_UPPER_CORR_FACTOR_LIMIT 8
#define SIZEOF_KNOWN_ALPHA_ITEM_TYPE 6
#define SIZEOF_KNOWN_ALPHA_DETECTOR_ID 12
#define SIZEOF_KA_LOWER_MASS_LIMIT 8
#define SIZEOF_KA_UPPER_MASS_LIMIT 8
#define SIZEOF_KA_DCL_MASS 160
#define SIZEOF_KA_DOUBLES 160
#define SIZEOF_KM_SF_RATE 8
#define SIZEOF_KM_VS1 8
#define SIZEOF_KM_VS2 8
#define SIZEOF_KM_VI1 8
#define SIZEOF_KM_VI2 8
#define SIZEOF_KM_B 8
#define SIZEOF_KM_C 8
#define SIZEOF_KM_SIGMA_X 8
#define SIZEOF_KNOWN_M_SPARES 80
#define SIZEOF_KNOWN_M_ITEM_TYPE 6
#define SIZEOF_KNOWN_M_DETECTOR_ID 12
#define SIZEOF_KM_LOWER_MASS_LIMIT 8
#define SIZEOF_KM_UPPER_MASS_LIMIT 8
#define SIZEOF_MUL_SOLVE_EFFICIENCY 1
#define SIZEOF_MUL_SF_RATE 8
#define SIZEOF_MUL_VS1 8
#define SIZEOF_MUL_VS2 8
#define SIZEOF_MUL_VS3 8
#define SIZEOF_MUL_VI1 8
#define SIZEOF_MUL_VI2 8
#define SIZEOF_MUL_VI3 8
#define SIZEOF_MUL_A 8
#define SIZEOF_MUL_B 8
#define SIZEOF_MUL_C 8
#define SIZEOF_MUL_SIGMA_X 8
#define SIZEOF_MUL_ALPHA_WEIGHT 8
#define SIZEOF_MULTIPLICITY_SPARES 72
#define SIZEOF_MULTIPLICITY_ITEM_TYPE 6
#define SIZEOF_MULTIPLICITY_DETECTOR_ID 12
#define SIZEOF_ACTIVE_PASSIVE_EQUATION 1
#define SIZEOF_AP_A 8
#define SIZEOF_AP_B 8
#define SIZEOF_AP_C 8
#define SIZEOF_AP_D 8
#define SIZEOF_AP_VAR_A 8
#define SIZEOF_AP_VAR_B 8
#define SIZEOF_AP_VAR_C 8
#define SIZEOF_AP_VAR_D 8
#define SIZEOF_AP_COVAR_AB 8
#define SIZEOF_AP_COVAR_AC 8
#define SIZEOF_AP_COVAR_AD 8
#define SIZEOF_AP_COVAR_BC 8
#define SIZEOF_AP_COVAR_BD 8
#define SIZEOF_AP_COVAR_CD 8
#define SIZEOF_AP_SIGMA_X 8
#define SIZEOF_ACTIVE_PASSIVE_SPARES 80
#define SIZEOF_ACTIVE_PASSIVE_ITEM_TYPE 6
#define SIZEOF_ACTIVE_PASSIVE_DETECTOR_ID 12
#define SIZEOF_AP_LOWER_MASS_LIMIT 8
#define SIZEOF_AP_UPPER_MASS_LIMIT 8
#define SIZEOF_ADD_A_SOURCE_EQUATION 1
#define SIZEOF_AD_A 8
#define SIZEOF_AD_B 8
#define SIZEOF_AD_C 8
#define SIZEOF_AD_D 8
#define SIZEOF_AD_VAR_A 8
#define SIZEOF_AD_VAR_B 8
#define SIZEOF_AD_VAR_C 8
#define SIZEOF_AD_VAR_D 8
#define SIZEOF_AD_COVAR_AB 8
#define SIZEOF_AD_COVAR_AC 8
#define SIZEOF_AD_COVAR_AD 8
#define SIZEOF_AD_COVAR_BC 8
#define SIZEOF_AD_COVAR_BD 8
#define SIZEOF_AD_COVAR_CD 8
#define SIZEOF_AD_SIGMA_X 8
#define SIZEOF_ADD_A_SOURCE_SPARES 80
#define SIZEOF_AD_POSITION_DZERO 40
#define SIZEOF_AD_DZERO_AVG 8
#define SIZEOF_AD_DZERO_REF_DATE 9
#define SIZEOF_AD_NUM_RUNS 2
#define SIZEOF_AD_CF_A 8
#define SIZEOF_AD_CF_B 8
#define SIZEOF_AD_CF_C 8
#define SIZEOF_AD_CF_D 8
#define SIZEOF_AD_USE_TRUNCATED_MULT 8
#define SIZEOF_AD_TM_WEIGHTING_FACTOR 8
#define SIZEOF_AD_TM_DBLS_RATE_UPPER_LIMIT 8
#define SIZEOF_ADD_A_SOURCE_CF_SPARES 56
#define SIZEOF_ADD_A_SOURCE_ITEM_TYPE 6
#define SIZEOF_ADD_A_SOURCE_DETECTOR_ID 12
#define SIZEOF_AD_LOWER_MASS_LIMIT 8
#define SIZEOF_AD_UPPER_MASS_LIMIT 8
#define SIZEOF_AD_DCL_MASS 160
#define SIZEOF_AD_DOUBLES 160
#define SIZEOF_AM_VT1 8
#define SIZEOF_AM_VT2 8
#define SIZEOF_AM_VT3 8
#define SIZEOF_AM_VF1 8
#define SIZEOF_AM_VF2 8
#define SIZEOF_AM_VF3 8
#define SIZEOF_ACTIVE_MULT_SPARES 80
#define SIZEOF_ACTIVE_MULT_ITEM_TYPE 6
#define SIZEOF_ACTIVE_MULT_DETECTOR_ID 12
#define SIZEOF_CURIUM_RATIO_EQUATION 1
#define SIZEOF_CR_A 8
#define SIZEOF_CR_B 8
#define SIZEOF_CR_C 8
#define SIZEOF_CR_D 8
#define SIZEOF_CR_VAR_A 8
#define SIZEOF_CR_VAR_B 8
#define SIZEOF_CR_VAR_C 8
#define SIZEOF_CR_VAR_D 8
#define SIZEOF_CR_COVAR_AB 8
#define SIZEOF_CR_COVAR_AC 8
#define SIZEOF_CR_COVAR_AD 8
#define SIZEOF_CR_COVAR_BC 8
#define SIZEOF_CR_COVAR_BD 8
#define SIZEOF_CR_COVAR_CD 8
#define SIZEOF_CR_SIGMA_X 8
#define SIZEOF_CURIUM_RATIO_TYPE 2
#define SIZEOF_CURIUM_RATIO_SPARES 80
#define SIZEOF_CURIUM_RATIO_ITEM_TYPE 6
#define SIZEOF_CURIUM_RATIO_DETECTOR_ID 12
#define SIZEOF_CR_LOWER_MASS_LIMIT 8
#define SIZEOF_CR_UPPER_MASS_LIMIT 8
#define SIZEOF_TM_A 8
#define SIZEOF_TM_B 8
#define SIZEOF_TM_KNOWN_EFF 1
#define SIZEOF_TM_SOLVE_EFF 1
#define SIZEOF_TRUNCATED_MULT_SPARES 80
#define SIZEOF_TRUNCATED_MULT_ITEM_TYPE 6
#define SIZEOF_TRUNCATED_MULT_DETECTOR_ID 12
#define SIZEOF_MEAS_DATE 9
#define SIZEOF_MEAS_TIME 9
#define SIZEOF_FILENAME 13
#define SIZEOF_ORIGINAL_MEAS_DATE 9
#define SIZEOF_RESULTS_FACILITY 13
#define SIZEOF_RESULTS_FACILITY_DESCRIPTION 21
#define SIZEOF_RESULTS_MBA 5
#define SIZEOF_RESULTS_MBA_DESCRIPTION 21
#define SIZEOF_ITEM_ID 13
#define SIZEOF_STRATUM_ID 13
#define SIZEOF_STRATUM_ID_DESCRIPTION 21
#define SIZEOF_RESULTS_CAMPAIGN_ID 13
#define SIZEOF_RESULTS_INSPECTION_NUMBER 13
#define SIZEOF_RESULTS_ITEM_TYPE 6
#define SIZEOF_RESULTS_COLLAR_MODE 1
#define SIZEOF_RESULTS_DETECTOR_ID 12
#define SIZEOF_RESULTS_DETECTOR_TYPE 12
#define SIZEOF_RESULTS_ELECTRONICS_ID 9
#define SIZEOF_RESULTS_GLOVEBOX_ID 21
#define SIZEOF_RESULTS_NUM_ROWS 2
#define SIZEOF_RESULTS_NUM_COLUMNS 2
#define SIZEOF_RESULTS_DISTANCE 8
#define SIZEOF_BIAS_UNCERTAINTY 8
#define SIZEOF_RANDOM_UNCERTAINTY 8
#define SIZEOF_SYSTEMATIC_UNCERTAINTY 8
#define SIZEOF_RELATIVE_STD_DEV 8
#define SIZEOF_COMPLETED 1
#define SIZEOF_MEAS_OPTION 1
#define SIZEOF_INVENTORY_CHANGE_CODE 3
#define SIZEOF_IO_CODE 2
#define SIZEOF_WELL_CONFIG 1
#define SIZEOF_DATA_SOURCE 1
#define SIZEOF_RESULTS_QC_TESTS 1
#define SIZEOF_ERROR_CALC_METHOD 2
#define SIZEOF_RESULTS_PRINT 1
#define SIZEOF_USER_ID 41
#define SIZEOF_COMMENT 51
#define SIZEOF_ENDING_COMMENT 51
#define SIZEOF_ITEM_PU238 8
#define SIZEOF_ITEM_PU239 8
#define SIZEOF_ITEM_PU240 8
#define SIZEOF_ITEM_PU241 8
#define SIZEOF_ITEM_PU242 8
#define SIZEOF_ITEM_AM241 8
#define SIZEOF_ITEM_PU238_ERR 8
#define SIZEOF_ITEM_PU239_ERR 8
#define SIZEOF_ITEM_PU240_ERR 8
#define SIZEOF_ITEM_PU241_ERR 8
#define SIZEOF_ITEM_PU242_ERR 8
#define SIZEOF_ITEM_AM241_ERR 8
#define SIZEOF_ITEM_PU_DATE 9
#define SIZEOF_ITEM_AM_DATE 9
#define SIZEOF_ITEM_ISOTOPICS_ID 13
#define SIZEOF_ITEM_ISOTOPICS_SOURCE_CODE 3
#define SIZEOF_NORMALIZATION_CONSTANT 8
#define SIZEOF_NORMALIZATION_CONSTANT_ERR 8
#define SIZEOF_RESULTS_PREDELAY 8
#define SIZEOF_RESULTS_GATE_LENGTH 8
#define SIZEOF_RESULTS_GATE_LENGTH2 8
#define SIZEOF_RESULTS_HIGH_VOLTAGE 8
#define SIZEOF_RESULTS_DIE_AWAY_TIME 8
#define SIZEOF_RESULTS_EFFICIENCY 8
#define SIZEOF_RESULTS_MULTIPLICITY_DEADTIME 8
#define SIZEOF_RESULTS_COEFF_A_DEADTIME 8
#define SIZEOF_RESULTS_COEFF_B_DEADTIME 8
#define SIZEOF_RESULTS_COEFF_C_DEADTIME 8
#define SIZEOF_RESULTS_DOUBLES_GATE_FRACTION 8
#define SIZEOF_RESULTS_TRIPLES_GATE_FRACTION 8
#define SIZEOF_R_ACC_SNGL_TEST_RATE_LIMIT 8
#define SIZEOF_R_ACC_SNGL_TEST_PRECISION_LIMIT 8
#define SIZEOF_R_ACC_SNGL_TEST_OUTLIER_LIMIT 8
#define SIZEOF_R_OUTLIER_TEST_LIMIT 8
#define SIZEOF_R_BKG_DOUBLES_RATE_LIMIT 8
#define SIZEOF_R_BKG_TRIPLES_RATE_LIMIT 8
#define SIZEOF_R_CHISQ_LIMIT 8
#define SIZEOF_R_MAX_NUM_FAILURES 2
#define SIZEOF_R_HIGH_VOLTAGE_TEST_LIMIT 8
#define SIZEOF_PASSIVE_BKG_SINGLES_RATE 8
#define SIZEOF_PASSIVE_BKG_SINGLES_RATE_ERR 8
#define SIZEOF_PASSIVE_BKG_DOUBLES_RATE 8
#define SIZEOF_PASSIVE_BKG_DOUBLES_RATE_ERR 8
#define SIZEOF_PASSIVE_BKG_TRIPLES_RATE 8
#define SIZEOF_PASSIVE_BKG_TRIPLES_RATE_ERR 8
#define SIZEOF_ACTIVE_BKG_SINGLES_RATE 8
#define SIZEOF_ACTIVE_BKG_SINGLES_RATE_ERR 8
#define SIZEOF_PASSIVE_BKG_SCALER1_RATE 8
#define SIZEOF_PASSIVE_BKG_SCALER2_RATE 8
#define SIZEOF_ACTIVE_BKG_SCALER1_RATE 8
#define SIZEOF_ACTIVE_BKG_SCALER2_RATE 8
#define SIZEOF_ERROR_MSG_CODES 810
#define SIZEOF_WARNING_MSG_CODES 810
#define SIZEOF_TOTAL_NUMBER_RUNS 2
#define SIZEOF_NUMBER_GOOD_RUNS 2
#define SIZEOF_TOTAL_GOOD_COUNT_TIME 8
#define SIZEOF_SINGLES_SUM 8
#define SIZEOF_SCALER1_SUM 8
#define SIZEOF_SCALER2_SUM 8
#define SIZEOF_REALS_PLUS_ACC_SUM 8
#define SIZEOF_ACC_SUM 8
#define SIZEOF_MULT_REALS_PLUS_ACC_SUM 1024
#define SIZEOF_MULT_ACC_SUM 1024
#define SIZEOF_SINGLES 8
#define SIZEOF_SINGLES_ERR 8
#define SIZEOF_DOUBLES 8
#define SIZEOF_DOUBLES_ERR 8
#define SIZEOF_TRIPLES 8
#define SIZEOF_TRIPLES_ERR 8
#define SIZEOF_SCALER1 8
#define SIZEOF_SCALER1_ERR 8
#define SIZEOF_SCALER2 8
#define SIZEOF_SCALER2_ERR 8
#define SIZEOF_UNCORRECTED_DOUBLES 8
#define SIZEOF_UNCORRECTED_DOUBLES_ERR 8
#define SIZEOF_SINGLES_MULTI 8
#define SIZEOF_DOUBLES_MULTI 8
#define SIZEOF_TRIPLES_MULTI 8
#define SIZEOF_DECLARED_MASS 8
#define SIZEOF_PRIMARY_ANALYSIS_METHOD 1
#define SIZEOF_NET_DRUM_WEIGHT 8
#define SIZEOF_PASSIVE_MEAS_DATE 9
#define SIZEOF_PASSIVE_MEAS_TIME 9
#define SIZEOF_PASSIVE_FILENAME 13
#define SIZEOF_PASSIVE_RESULTS_DETECTOR_ID 12
#define SIZEOF_ACTIVE_MEAS_DATE 9
#define SIZEOF_ACTIVE_MEAS_TIME 9
#define SIZEOF_ACTIVE_FILENAME 13
#define SIZEOF_ACTIVE_RESULTS_DETECTOR_ID 12
#define SIZEOF_COVARIANCE_MATRIX 72
#define SIZEOF_R_NORMAL_BACKUP_ASSAY_TEST_LIM 8
#define SIZEOF_R_MAX_RUNS_FOR_OUTLIER_TEST 8
#define SIZEOF_R_CHECKSUM_TEST 8
#define SIZEOF_RESULTS_ACCIDENTALS_METHOD 8
#define SIZEOF_DECLARED_U_MASS 8
#define SIZEOF_LENGTH 8
#define SIZEOF_DB_VERSION 8
#define SIZEOF_RESULTS_SPARES 744
#define SIZEOF_INIT_SRC_ID 13
#define SIZEOF_INIT_SRC_PASS_FAIL 5
#define SIZEOF_INIT_SRC_MODE 2
#define SIZEOF_BIAS_SOURCE_ID 13
#define SIZEOF_BIAS_PASS_FAIL 5
#define SIZEOF_RESULTS_BIAS_MODE 2
#define SIZEOF_BIAS_SNGLS_RATE_EXPECT 8
#define SIZEOF_BIAS_SNGLS_RATE_EXPECT_ERR 8
#define SIZEOF_BIAS_SNGLS_RATE_EXPECT_MEAS 8
#define SIZEOF_BIAS_SNGLS_RATE_EXPECT_MEAS_ERR 8
#define SIZEOF_BIAS_DBLS_RATE_EXPECT 8
#define SIZEOF_BIAS_DBLS_RATE_EXPECT_ERR 8
#define SIZEOF_BIAS_DBLS_RATE_EXPECT_MEAS 8
#define SIZEOF_BIAS_DBLS_RATE_EXPECT_MEAS_ERR 8
#define SIZEOF_NEW_NORM_CONSTANT 8
#define SIZEOF_NEW_NORM_CONSTANT_ERR 8
#define SIZEOF_MEAS_PRECISION 8
#define SIZEOF_REQUIRED_PRECISION 8
#define SIZEOF_REQUIRED_MEAS_SECONDS 8
#define SIZEOF_BIAS_SPARES 80
#define SIZEOF_PREC_PASS_FAIL 5
#define SIZEOF_PREC_SAMPLE_VAR 8
#define SIZEOF_PREC_THEORETICAL_VAR 8
#define SIZEOF_PREC_CHI_SQ 8
#define SIZEOF_CHI_SQ_LOWER_LIMIT 8
#define SIZEOF_CHI_SQ_UPPER_LIMIT 8
#define SIZEOF_PREC_SPARES 80
#define SIZEOF_CC_PU240E_MASS 8
#define SIZEOF_CC_PU240E_MASS_ERR 8
#define SIZEOF_CC_PU_MASS 8
#define SIZEOF_CC_PU_MASS_ERR 8
#define SIZEOF_CC_DCL_PU240E_MASS 8
#define SIZEOF_CC_DCL_PU_MASS 8
#define SIZEOF_CC_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_CC_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_CC_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_CC_PASS_FAIL 5
#define SIZEOF_CC_DCL_U_MASS 8
#define SIZEOF_CC_LENGTH 8
#define SIZEOF_CC_HEAVY_METAL_CONTENT 8
#define SIZEOF_CC_HEAVY_METAL_CORRECTION 8
#define SIZEOF_CC_HEAVY_METAL_CORR_SINGLES 8
#define SIZEOF_CC_HEAVY_METAL_CORR_SINGLES_ERR 8
#define SIZEOF_CC_HEAVY_METAL_CORR_DOUBLES 8
#define SIZEOF_CC_HEAVY_METAL_CORR_DOUBLES_ERR 8
#define SIZEOF_CC_SPARES 16
#define SIZEOF_CC_CAL_CURVE_EQUATION 1
#define SIZEOF_CC_A_RES 8
#define SIZEOF_CC_B_RES 8
#define SIZEOF_CC_C_RES 8
#define SIZEOF_CC_D_RES 8
#define SIZEOF_CC_VAR_A_RES 8
#define SIZEOF_CC_VAR_B_RES 8
#define SIZEOF_CC_VAR_C_RES 8
#define SIZEOF_CC_VAR_D_RES 8
#define SIZEOF_CC_COVAR_AB_RES 8
#define SIZEOF_CC_COVAR_AC_RES 8
#define SIZEOF_CC_COVAR_AD_RES 8
#define SIZEOF_CC_COVAR_BC_RES 8
#define SIZEOF_CC_COVAR_BD_RES 8
#define SIZEOF_CC_COVAR_CD_RES 8
#define SIZEOF_CC_SIGMA_X_RES 8
#define SIZEOF_CC_CAL_CURVE_TYPE_RES 8
#define SIZEOF_CC_HEAVY_METAL_CORR_FACTOR_RES 8
#define SIZEOF_CC_HEAVY_METAL_REFERENCE_RES 8
#define SIZEOF_CC_PERCENT_U235_RES 8
#define SIZEOF_CC_SPARES_RES 48
#define SIZEOF_CC_CAL_CURVE_ITEM_TYPE 6
#define SIZEOF_CC_CAL_CURVE_DETECTOR_ID 12
#define SIZEOF_KA_MULT 8
#define SIZEOF_KA_ALPHA 8
#define SIZEOF_KA_MULT_CORR_DOUBLES 8
#define SIZEOF_KA_MULT_CORR_DOUBLES_ERR 8
#define SIZEOF_KA_PU240E_MASS 8
#define SIZEOF_KA_PU240E_MASS_ERR 8
#define SIZEOF_KA_PU_MASS 8
#define SIZEOF_KA_PU_MASS_ERR 8
#define SIZEOF_KA_DCL_PU240E_MASS 8
#define SIZEOF_KA_DCL_PU_MASS 8
#define SIZEOF_KA_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_KA_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_KA_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_KA_PASS_FAIL 5
#define SIZEOF_KA_DCL_U_MASS 8
#define SIZEOF_KA_LENGTH 8
#define SIZEOF_KA_HEAVY_METAL_CONTENT 8
#define SIZEOF_KA_HEAVY_METAL_CORRECTION 8
#define SIZEOF_KA_CORR_SINGLES 8
#define SIZEOF_KA_CORR_SINGLES_ERR 8
#define SIZEOF_KA_CORR_DOUBLES 8
#define SIZEOF_KA_CORR_DOUBLES_ERR 8
#define SIZEOF_KA_CORR_FACTOR 8
#define SIZEOF_KA_DRY_ALPHA_OR_MULT_DBLS 8
#define SIZEOF_KA_ALPHA_WT_RES 8
#define SIZEOF_KA_RHO_ZERO_RES 8
#define SIZEOF_KA_K_RES 8
#define SIZEOF_KA_A_RES 8
#define SIZEOF_KA_B_RES 8
#define SIZEOF_KA_VAR_A_RES 8
#define SIZEOF_KA_VAR_B_RES 8
#define SIZEOF_KA_COVAR_AB_RES 8
#define SIZEOF_KA_SIGMA_X_RES 8
#define SIZEOF_KA_KNOWN_ALPHA_TYPE_RES 8
#define SIZEOF_KA_HEAVY_METAL_CORR_FACTOR_RES 8
#define SIZEOF_KA_HEAVY_METAL_REFERENCE_RES 8
#define SIZEOF_KA_RING_RATIO_EQUATION_RES 8
#define SIZEOF_KA_RING_RATIO_A_RES 8
#define SIZEOF_KA_RING_RATIO_B_RES 8
#define SIZEOF_KA_RING_RATIO_C_RES 8
#define SIZEOF_KA_RING_RATIO_D_RES 8
#define SIZEOF_KA_LOWER_CORR_FACTOR_LIMIT_RES 8
#define SIZEOF_KA_UPPER_CORR_FACTOR_LIMIT_RES 8
#define SIZEOF_KA_KNOWN_ALPHA_ITEM_TYPE 6
#define SIZEOF_KA_KNOWN_ALPHA_DETECTOR_ID 12
#define SIZEOF_KM_MULT 8
#define SIZEOF_KM_ALPHA 8
#define SIZEOF_KM_PU239E_MASS 8
#define SIZEOF_KM_PU240E_MASS 8
#define SIZEOF_KM_PU240E_MASS_ERR 8
#define SIZEOF_KM_PU_MASS 8
#define SIZEOF_KM_PU_MASS_ERR 8
#define SIZEOF_KM_DCL_PU240E_MASS 8
#define SIZEOF_KM_DCL_PU_MASS 8
#define SIZEOF_KM_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_KM_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_KM_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_KM_PASS_FAIL 5
#define SIZEOF_KM_SPARES 80
#define SIZEOF_KM_SF_RATE_RES 8
#define SIZEOF_KM_VS1_RES 8
#define SIZEOF_KM_VS2_RES 8
#define SIZEOF_KM_VI1_RES 8
#define SIZEOF_KM_VI2_RES 8
#define SIZEOF_KM_B_RES 8
#define SIZEOF_KM_C_RES 8
#define SIZEOF_KM_SIGMA_X_RES 8
#define SIZEOF_KM_SPARES_RES 80
#define SIZEOF_KM_KNOWN_M_ITEM_TYPE 6
#define SIZEOF_KM_KNOWN_M_DETECTOR_ID 12
#define SIZEOF_MUL_MULT 8
#define SIZEOF_MUL_MULT_ERR 8
#define SIZEOF_MUL_ALPHA 8
#define SIZEOF_MUL_ALPHA_ERR 8
#define SIZEOF_MUL_CORR_FACTOR 8
#define SIZEOF_MUL_CORR_FACTOR_ERR 8
#define SIZEOF_MUL_EFFICIENCY 8
#define SIZEOF_MUL_EFFICIENCY_ERR 8
#define SIZEOF_MUL_PU240E_MASS 8
#define SIZEOF_MUL_PU240E_MASS_ERR 8
#define SIZEOF_MUL_PU_MASS 8
#define SIZEOF_MUL_PU_MASS_ERR 8
#define SIZEOF_MUL_DCL_PU240E_MASS 8
#define SIZEOF_MUL_DCL_PU_MASS 8
#define SIZEOF_MUL_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_MUL_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_MUL_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_MUL_PASS_FAIL 5
#define SIZEOF_MUL_SPARES 80
#define SIZEOF_MUL_SOLVE_EFFICIENCY_RES 1
#define SIZEOF_MUL_SF_RATE_RES 8
#define SIZEOF_MUL_VS1_RES 8
#define SIZEOF_MUL_VS2_RES 8
#define SIZEOF_MUL_VS3_RES 8
#define SIZEOF_MUL_VI1_RES 8
#define SIZEOF_MUL_VI2_RES 8
#define SIZEOF_MUL_VI3_RES 8
#define SIZEOF_MUL_A_RES 8
#define SIZEOF_MUL_B_RES 8
#define SIZEOF_MUL_C_RES 8
#define SIZEOF_MUL_SIGMA_X_RES 8
#define SIZEOF_MUL_ALPHA_WEIGHT_RES 8
#define SIZEOF_MUL_SPARES_RES 72
#define SIZEOF_MUL_MULTIPLICITY_ITEM_TYPE 6
#define SIZEOF_MUL_MULTIPLICITY_DETECTOR_ID 12
#define SIZEOF_AP_DELTA_DOUBLES 8
#define SIZEOF_AP_DELTA_DOUBLES_ERR 8
#define SIZEOF_AP_U235_MASS 8
#define SIZEOF_AP_U235_MASS_ERR 8
#define SIZEOF_AP_K0 8
#define SIZEOF_AP_K1 8
#define SIZEOF_AP_K1_ERR 8
#define SIZEOF_AP_K 8
#define SIZEOF_AP_K_ERR 8
#define SIZEOF_AP_DCL_U235_MASS 8
#define SIZEOF_AP_DCL_MINUS_ASY_U235_MASS 8
#define SIZEOF_AP_DCL_MINUS_ASY_U235_MASS_ERR 8
#define SIZEOF_AP_DCL_MINUS_ASY_U235_MASS_PCT 8
#define SIZEOF_AP_PASS_FAIL 5
#define SIZEOF_AP_SPARES 80
#define SIZEOF_AP_ACTIVE_PASSIVE_EQUATION 1
#define SIZEOF_AP_A_RES 8
#define SIZEOF_AP_B_RES 8
#define SIZEOF_AP_C_RES 8
#define SIZEOF_AP_D_RES 8
#define SIZEOF_AP_VAR_A_RES 8
#define SIZEOF_AP_VAR_B_RES 8
#define SIZEOF_AP_VAR_C_RES 8
#define SIZEOF_AP_VAR_D_RES 8
#define SIZEOF_AP_COVAR_AB_RES 8
#define SIZEOF_AP_COVAR_AC_RES 8
#define SIZEOF_AP_COVAR_AD_RES 8
#define SIZEOF_AP_COVAR_BC_RES 8
#define SIZEOF_AP_COVAR_BD_RES 8
#define SIZEOF_AP_COVAR_CD_RES 8
#define SIZEOF_AP_SIGMA_X_RES 8
#define SIZEOF_AP_SPARES_RES 80
#define SIZEOF_AP_ACTIVE_PASSIVE_ITEM_TYPE 6
#define SIZEOF_AP_ACTIVE_PASSIVE_DETECTOR_ID 12
#define SIZEOF_ACT_U235_MASS 8
#define SIZEOF_ACT_U235_MASS_ERR 8
#define SIZEOF_ACT_K0 8
#define SIZEOF_ACT_K1 8
#define SIZEOF_ACT_K1_ERR 8
#define SIZEOF_ACT_K 8
#define SIZEOF_ACT_K_ERR 8
#define SIZEOF_ACT_DCL_U235_MASS 8
#define SIZEOF_ACT_DCL_MINUS_ASY_U235_MASS 8
#define SIZEOF_ACT_DCL_MINUS_ASY_U235_MASS_ERR 8
#define SIZEOF_ACT_DCL_MINUS_ASY_U235_MASS_PCT 8
#define SIZEOF_ACT_PASS_FAIL 5
#define SIZEOF_ACT_SPARES 80
#define SIZEOF_ACT_ACTIVE_EQUATION 1
#define SIZEOF_ACT_A_RES 8
#define SIZEOF_ACT_B_RES 8
#define SIZEOF_ACT_C_RES 8
#define SIZEOF_ACT_D_RES 8
#define SIZEOF_ACT_VAR_A_RES 8
#define SIZEOF_ACT_VAR_B_RES 8
#define SIZEOF_ACT_VAR_C_RES 8
#define SIZEOF_ACT_VAR_D_RES 8
#define SIZEOF_ACT_COVAR_AB_RES 8
#define SIZEOF_ACT_COVAR_AC_RES 8
#define SIZEOF_ACT_COVAR_AD_RES 8
#define SIZEOF_ACT_COVAR_BC_RES 8
#define SIZEOF_ACT_COVAR_BD_RES 8
#define SIZEOF_ACT_COVAR_CD_RES 8
#define SIZEOF_ACT_SIGMA_X_RES 8
#define SIZEOF_ACT_SPARES_RES 80
#define SIZEOF_ACT_ACTIVE_ITEM_TYPE 6
#define SIZEOF_ACT_ACTIVE_DETECTOR_ID 12
#define SIZEOF_COL_U235_MASS 8
#define SIZEOF_COL_U235_MASS_ERR 8
#define SIZEOF_COL_PERCENT_U235 8
#define SIZEOF_COL_TOTAL_U_MASS 8
#define SIZEOF_COL_K0 8
#define SIZEOF_COL_K0_ERR 8
#define SIZEOF_COL_K1 8
#define SIZEOF_COL_K1_ERR 8
#define SIZEOF_COL_K2 8
#define SIZEOF_COL_K2_ERR 8
#define SIZEOF_COL_K3 8
#define SIZEOF_COL_K3_ERR 8
#define SIZEOF_COL_K4 8
#define SIZEOF_COL_K4_ERR 8
#define SIZEOF_COL_K5 8
#define SIZEOF_COL_K5_ERR 8
#define SIZEOF_COL_TOTAL_CORR_FACT 8
#define SIZEOF_COL_TOTAL_CORR_FACT_ERR 8
#define SIZEOF_COL_SOURCE_ID 13
#define SIZEOF_COL_CORR_DOUBLES 8
#define SIZEOF_COL_CORR_DOUBLES_ERR 8
#define SIZEOF_COL_DCL_LENGTH 8
#define SIZEOF_COL_DCL_LENGTH_ERR 8
#define SIZEOF_COL_DCL_TOTAL_U235 8
#define SIZEOF_COL_DCL_TOTAL_U235_ERR 8
#define SIZEOF_COL_DCL_TOTAL_U238 8
#define SIZEOF_COL_DCL_TOTAL_U238_ERR 8
#define SIZEOF_COL_DCL_TOTAL_RODS 8
#define SIZEOF_COL_DCL_TOTAL_POISON_RODS 8
#define SIZEOF_COL_DCL_POISON_PERCENT 8
#define SIZEOF_COL_DCL_POISON_PERCENT_ERR 8
#define SIZEOF_COL_DCL_MINUS_ASY_U235_MASS 8
#define SIZEOF_COL_DCL_MINUS_ASY_U235_MASS_ERR 8
#define SIZEOF_COL_DCL_MINUS_ASY_U235_MASS_PCT 8
#define SIZEOF_COL_PASS_FAIL 5
#define SIZEOF_COL_SPARES 80
#define SIZEOF_COL_COLLAR_ITEM_TYPE 6
#define SIZEOF_COL_COLLAR_MODE 1
#define SIZEOF_COL_COLLAR_DETECTOR_ID 12
#define SIZEOF_COL_COLLAR_EQUATION 1
#define SIZEOF_COL_A_RES 8
#define SIZEOF_COL_B_RES 8
#define SIZEOF_COL_C_RES 8
#define SIZEOF_COL_D_RES 8
#define SIZEOF_COL_VAR_A_RES 8
#define SIZEOF_COL_VAR_B_RES 8
#define SIZEOF_COL_VAR_C_RES 8
#define SIZEOF_COL_VAR_D_RES 8
#define SIZEOF_COL_COVAR_AB_RES 8
#define SIZEOF_COL_COVAR_AC_RES 8
#define SIZEOF_COL_COVAR_AD_RES 8
#define SIZEOF_COL_COVAR_BC_RES 8
#define SIZEOF_COL_COVAR_BD_RES 8
#define SIZEOF_COL_COVAR_CD_RES 8
#define SIZEOF_COL_SIGMA_X_RES 8
#define SIZEOF_COL_NUMBER_CALIB_RODS_RES 8
#define SIZEOF_COL_POISON_ROD_TYPE_RES 2
#define SIZEOF_COL_POISON_ABSORPTION_FACT_RES 8
#define SIZEOF_COL_POISON_ROD_A_RES 8
#define SIZEOF_COL_POISON_ROD_A_ERR_RES 8
#define SIZEOF_COL_POISON_ROD_B_RES 8
#define SIZEOF_COL_POISON_ROD_B_ERR_RES 8
#define SIZEOF_COL_POISON_ROD_C_RES 8
#define SIZEOF_COL_POISON_ROD_C_ERR_RES 8
#define SIZEOF_COL_U_MASS_CORR_FACT_A_RES 8
#define SIZEOF_COL_U_MASS_CORR_FACT_A_ERR_RES 8
#define SIZEOF_COL_U_MASS_CORR_FACT_B_RES 8
#define SIZEOF_COL_U_MASS_CORR_FACT_B_ERR_RES 8
#define SIZEOF_COL_SAMPLE_CORR_FACT_RES 8
#define SIZEOF_COL_SAMPLE_CORR_FACT_ERR_RES 8
#define SIZEOF_COL_REFERENCE_DATE_RES 9
#define SIZEOF_COL_RELATIVE_DOUBLES_RATE_RES 8
#define SIZEOF_COL_SPARES_RES 80
#define SIZEOF_COLLAR_K5_LABEL_RES 620
#define SIZEOF_COLLAR_K5_CHECKBOX_RES 20
#define SIZEOF_COLLAR_K5_RES 160
#define SIZEOF_COLLAR_K5_ERR_RES 160
#define SIZEOF_AD_DZERO_CF252_DOUBLES 8
#define SIZEOF_AD_SAMPLE_CF252_DOUBLES 40
#define SIZEOF_AD_SAMPLE_CF252_DOUBLES_ERR 40
#define SIZEOF_AD_SAMPLE_CF252_RATIO 40
#define SIZEOF_AD_SAMPLE_AVG_CF252_DOUBLES 8
#define SIZEOF_AD_SAMPLE_AVG_CF252_DOUBLES_ERR 8
#define SIZEOF_AD_CORR_DOUBLES 8
#define SIZEOF_AD_CORR_DOUBLES_ERR 8
#define SIZEOF_AD_DELTA 8
#define SIZEOF_AD_DELTA_ERR 8
#define SIZEOF_AD_CORR_FACTOR 8
#define SIZEOF_AD_CORR_FACTOR_ERR 8
#define SIZEOF_AD_PU240E_MASS 8
#define SIZEOF_AD_PU240E_MASS_ERR 8
#define SIZEOF_AD_PU_MASS 8
#define SIZEOF_AD_PU_MASS_ERR 8
#define SIZEOF_AD_DCL_PU240E_MASS 8
#define SIZEOF_AD_DCL_PU_MASS 8
#define SIZEOF_AD_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_AD_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_AD_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_AD_PASS_FAIL 5
#define SIZEOF_AD_TM_DOUBLES_BKG 8
#define SIZEOF_AD_TM_DOUBLES_BKG_ERR 8
#define SIZEOF_AD_TM_UNCORR_DOUBLES 8
#define SIZEOF_AD_TM_UNCORR_DOUBLES_ERR 8
#define SIZEOF_AD_TM_CORR_DOUBLES 8
#define SIZEOF_AD_TM_CORR_DOUBLES_ERR 8
#define SIZEOF_AD_SPARES 32
#define SIZEOF_AD_ADD_A_SOURCE_EQUATION 1
#define SIZEOF_AD_A_RES 8
#define SIZEOF_AD_B_RES 8
#define SIZEOF_AD_C_RES 8
#define SIZEOF_AD_D_RES 8
#define SIZEOF_AD_VAR_A_RES 8
#define SIZEOF_AD_VAR_B_RES 8
#define SIZEOF_AD_VAR_C_RES 8
#define SIZEOF_AD_VAR_D_RES 8
#define SIZEOF_AD_COVAR_AB_RES 8
#define SIZEOF_AD_COVAR_AC_RES 8
#define SIZEOF_AD_COVAR_AD_RES 8
#define SIZEOF_AD_COVAR_BC_RES 8
#define SIZEOF_AD_COVAR_BD_RES 8
#define SIZEOF_AD_COVAR_CD_RES 8
#define SIZEOF_AD_SIGMA_X_RES 8
#define SIZEOF_AD_SPARES_RES 80
#define SIZEOF_AD_POSITION_DZERO_RES 40
#define SIZEOF_AD_DZERO_AVG_RES 8
#define SIZEOF_AD_DZERO_REF_DATE_RES 9
#define SIZEOF_AD_NUM_RUNS_RES 2
#define SIZEOF_AD_CF_A_RES 8
#define SIZEOF_AD_CF_B_RES 8
#define SIZEOF_AD_CF_C_RES 8
#define SIZEOF_AD_CF_D_RES 8
#define SIZEOF_AD_USE_TRUNCATED_MULT_RES 8
#define SIZEOF_AD_TM_WEIGHTING_FACTOR_RES 8
#define SIZEOF_AD_TM_DBLS_RATE_UPPER_LIMIT_RES 8
#define SIZEOF_AD_CF_SPARES_RES 56
#define SIZEOF_AD_ADD_A_SOURCE_ITEM_TYPE 6
#define SIZEOF_AD_ADD_A_SOURCE_DETECTOR_ID 12
#define SIZEOF_AM_MULT 8
#define SIZEOF_AM_MULT_ERR 8
#define SIZEOF_AM_SPARES 80
#define SIZEOF_AM_VT1_RES 8
#define SIZEOF_AM_VT2_RES 8
#define SIZEOF_AM_VT3_RES 8
#define SIZEOF_AM_VF1_RES 8
#define SIZEOF_AM_VF2_RES 8
#define SIZEOF_AM_VF3_RES 8
#define SIZEOF_AM_SPARES_RES 80
#define SIZEOF_AM_MULT_ITEM_TYPE 6
#define SIZEOF_AM_MULT_DETECTOR_ID 12
#define SIZEOF_CR_PU240E_MASS 8
#define SIZEOF_CR_PU240E_MASS_ERR 8
#define SIZEOF_CR_CM_MASS 8
#define SIZEOF_CR_CM_MASS_ERR 8
#define SIZEOF_CR_PU_MASS 8
#define SIZEOF_CR_PU_MASS_ERR 8
#define SIZEOF_CR_U_MASS 8
#define SIZEOF_CR_U_MASS_ERR 8
#define SIZEOF_CR_U235_MASS 8
#define SIZEOF_CR_U235_MASS_ERR 8
#define SIZEOF_CR_DCL_PU_MASS 8
#define SIZEOF_CR_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_CR_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_CR_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_CR_DCL_MINUS_ASY_U_MASS 8
#define SIZEOF_CR_DCL_MINUS_ASY_U_MASS_ERR 8
#define SIZEOF_CR_DCL_MINUS_ASY_U_MASS_PCT 8
#define SIZEOF_CR_DCL_MINUS_ASY_U235_MASS 8
#define SIZEOF_CR_DCL_MINUS_ASY_U235_MASS_ERR 8
#define SIZEOF_CR_DCL_MINUS_ASY_U235_MASS_PCT 8
#define SIZEOF_CR_PU_PASS_FAIL 5
#define SIZEOF_CR_U_PASS_FAIL 5
#define SIZEOF_CR_CM_PU_RATIO 8
#define SIZEOF_CR_CM_PU_RATIO_ERR 8
#define SIZEOF_CR_PU_HALF_LIFE 8
#define SIZEOF_CR_CM_PU_RATIO_DATE 9
#define SIZEOF_CR_CM_U_RATIO 8
#define SIZEOF_CR_CM_U_RATIO_ERR 8
#define SIZEOF_CR_CM_U_RATIO_DATE 9
#define SIZEOF_CR_CM_ID_LABEL 13
#define SIZEOF_CR_CM_ID 13
#define SIZEOF_CR_CM_INPUT_BATCH_ID 13
#define SIZEOF_CR_DCL_U_MASS_RES 8
#define SIZEOF_CR_DCL_U235_MASS_RES 8
#define SIZEOF_CR_CM_PU_RATIO_DECAY_CORR 8
#define SIZEOF_CR_CM_PU_RATIO_DECAY_CORR_ERR 8
#define SIZEOF_CR_CM_U_RATIO_DECAY_CORR 8
#define SIZEOF_CR_CM_U_RATIO_DECAY_CORR_ERR 8
#define SIZEOF_CR_SPARES 80
#define SIZEOF_CR_CURIUM_RATIO_EQUATION 1
#define SIZEOF_CR_A_RES 8
#define SIZEOF_CR_B_RES 8
#define SIZEOF_CR_C_RES 8
#define SIZEOF_CR_D_RES 8
#define SIZEOF_CR_VAR_A_RES 8
#define SIZEOF_CR_VAR_B_RES 8
#define SIZEOF_CR_VAR_C_RES 8
#define SIZEOF_CR_VAR_D_RES 8
#define SIZEOF_CR_COVAR_AB_RES 8
#define SIZEOF_CR_COVAR_AC_RES 8
#define SIZEOF_CR_COVAR_AD_RES 8
#define SIZEOF_CR_COVAR_BC_RES 8
#define SIZEOF_CR_COVAR_BD_RES 8
#define SIZEOF_CR_COVAR_CD_RES 8
#define SIZEOF_CR_SIGMA_X_RES 8
#define SIZEOF_CURIUM_RATIO_TYPE_RES 2
#define SIZEOF_CR_SPARES_RES 80
#define SIZEOF_CR_CURIUM_RATIO_ITEM_TYPE 6
#define SIZEOF_CR_CURIUM_RATIO_DETECTOR_ID 12
#define SIZEOF_TM_BKG_SINGLES 8
#define SIZEOF_TM_BKG_SINGLES_ERR 8
#define SIZEOF_TM_BKG_ZEROS 8
#define SIZEOF_TM_BKG_ZEROS_ERR 8
#define SIZEOF_TM_BKG_ONES 8
#define SIZEOF_TM_BKG_ONES_ERR 8
#define SIZEOF_TM_BKG_TWOS 8
#define SIZEOF_TM_BKG_TWOS_ERR 8
#define SIZEOF_TM_NET_SINGLES 8
#define SIZEOF_TM_NET_SINGLES_ERR 8
#define SIZEOF_TM_NET_ZEROS 8
#define SIZEOF_TM_NET_ZEROS_ERR 8
#define SIZEOF_TM_NET_ONES 8
#define SIZEOF_TM_NET_ONES_ERR 8
#define SIZEOF_TM_NET_TWOS 8
#define SIZEOF_TM_NET_TWOS_ERR 8
#define SIZEOF_TM_K_ALPHA 8
#define SIZEOF_TM_K_ALPHA_ERR 8
#define SIZEOF_TM_K_PU240E_MASS 8
#define SIZEOF_TM_K_PU240E_MASS_ERR 8
#define SIZEOF_TM_K_PU_MASS 8
#define SIZEOF_TM_K_PU_MASS_ERR 8
#define SIZEOF_TM_K_DCL_PU240E_MASS 8
#define SIZEOF_TM_K_DCL_PU_MASS 8
#define SIZEOF_TM_K_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_TM_K_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_TM_K_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_TM_K_PASS_FAIL 5
#define SIZEOF_TM_S_EFF 8
#define SIZEOF_TM_S_EFF_ERR 8
#define SIZEOF_TM_S_ALPHA 8
#define SIZEOF_TM_S_ALPHA_ERR 8
#define SIZEOF_TM_S_PU240E_MASS 8
#define SIZEOF_TM_S_PU240E_MASS_ERR 8
#define SIZEOF_TM_S_PU_MASS 8
#define SIZEOF_TM_S_PU_MASS_ERR 8
#define SIZEOF_TM_S_DCL_PU240E_MASS 8
#define SIZEOF_TM_S_DCL_PU_MASS 8
#define SIZEOF_TM_S_DCL_MINUS_ASY_PU_MASS 8
#define SIZEOF_TM_S_DCL_MINUS_ASY_PU_MASS_ERR 8
#define SIZEOF_TM_S_DCL_MINUS_ASY_PU_MASS_PCT 8
#define SIZEOF_TM_S_PASS_FAIL 5
#define SIZEOF_TM_SPARES 80
#define SIZEOF_TM_A_RES 8
#define SIZEOF_TM_B_RES 8
#define SIZEOF_TM_KNOWN_EFF_RES 1
#define SIZEOF_TM_SOLVE_EFF_RES 1
#define SIZEOF_TM_SPARES_RES 80
#define SIZEOF_TM_TRUNCATED_MULT_ITEM_TYPE 6
#define SIZEOF_TM_TRUNCATED_MULT_DETECTOR_ID 12
#define SIZEOF_RESULTS_TM_SINGLES_BKG 8
#define SIZEOF_RESULTS_TM_SINGLES_BKG_ERR 8
#define SIZEOF_RESULTS_TM_ZEROS_BKG 8
#define SIZEOF_RESULTS_TM_ZEROS_BKG_ERR 8
#define SIZEOF_RESULTS_TM_ONES_BKG 8
#define SIZEOF_RESULTS_TM_ONES_BKG_ERR 8
#define SIZEOF_RESULTS_TM_TWOS_BKG 8
#define SIZEOF_RESULTS_TM_TWOS_BKG_ERR 8
#define SIZEOF_RUN_NUMBER 2
#define SIZEOF_RUN_DATE 9
#define SIZEOF_RUN_TIME 9
#define SIZEOF_RUN_TESTS 21
#define SIZEOF_RUN_COUNT_TIME 8
#define SIZEOF_RUN_SINGLES 8
#define SIZEOF_RUN_SCALER1 8
#define SIZEOF_RUN_SCALER2 8
#define SIZEOF_RUN_REALS_PLUS_ACC 8
#define SIZEOF_RUN_ACC 8
#define SIZEOF_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_RUN_MULT_ACC 1024
#define SIZEOF_RUN_SINGLES_RATE 8
#define SIZEOF_RUN_DOUBLES_RATE 8
#define SIZEOF_RUN_TRIPLES_RATE 8
#define SIZEOF_RUN_SCALER1_RATE 8
#define SIZEOF_RUN_SCALER2_RATE 8
#define SIZEOF_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_RUN_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_RUN_MASS 8
#define SIZEOF_RUN_HIGH_VOLTAGE 8
#define SIZEOF_RUN_SPARE 80
#define SIZEOF_CF1_RUN_NUMBER 2
#define SIZEOF_CF1_RUN_DATE 9
#define SIZEOF_CF1_RUN_TIME 9
#define SIZEOF_CF1_RUN_TESTS 21
#define SIZEOF_CF1_RUN_COUNT_TIME 8
#define SIZEOF_CF1_RUN_SINGLES 8
#define SIZEOF_CF1_RUN_SCALER1 8
#define SIZEOF_CF1_RUN_SCALER2 8
#define SIZEOF_CF1_RUN_REALS_PLUS_ACC 8
#define SIZEOF_CF1_RUN_ACC 8
#define SIZEOF_CF1_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_CF1_RUN_MULT_ACC 1024
#define SIZEOF_CF1_RUN_SINGLES_RATE 8
#define SIZEOF_CF1_RUN_DOUBLES_RATE 8
#define SIZEOF_CF1_RUN_TRIPLES_RATE 8
#define SIZEOF_CF1_RUN_SCALER1_RATE 8
#define SIZEOF_CF1_RUN_SCALER2_RATE 8
#define SIZEOF_CF1_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_CF1_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_CF1_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_CF1_RUN_MASS 8
#define SIZEOF_CF1_HIGH_VOLTAGE 8
#define SIZEOF_CF1_SPARE 80
#define SIZEOF_CF2_RUN_NUMBER 2
#define SIZEOF_CF2_RUN_DATE 9
#define SIZEOF_CF2_RUN_TIME 9
#define SIZEOF_CF2_RUN_TESTS 21
#define SIZEOF_CF2_RUN_COUNT_TIME 8
#define SIZEOF_CF2_RUN_SINGLES 8
#define SIZEOF_CF2_RUN_SCALER1 8
#define SIZEOF_CF2_RUN_SCALER2 8
#define SIZEOF_CF2_RUN_REALS_PLUS_ACC 8
#define SIZEOF_CF2_RUN_ACC 8
#define SIZEOF_CF2_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_CF2_RUN_MULT_ACC 1024
#define SIZEOF_CF2_RUN_SINGLES_RATE 8
#define SIZEOF_CF2_RUN_DOUBLES_RATE 8
#define SIZEOF_CF2_RUN_TRIPLES_RATE 8
#define SIZEOF_CF2_RUN_SCALER1_RATE 8
#define SIZEOF_CF2_RUN_SCALER2_RATE 8
#define SIZEOF_CF2_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_CF2_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_CF2_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_CF2_RUN_MASS 8
#define SIZEOF_CF2_HIGH_VOLTAGE 8
#define SIZEOF_CF2_SPARE 80
#define SIZEOF_CF3_RUN_NUMBER 2
#define SIZEOF_CF3_RUN_DATE 9
#define SIZEOF_CF3_RUN_TIME 9
#define SIZEOF_CF3_RUN_TESTS 21
#define SIZEOF_CF3_RUN_COUNT_TIME 8
#define SIZEOF_CF3_RUN_SINGLES 8
#define SIZEOF_CF3_RUN_SCALER1 8
#define SIZEOF_CF3_RUN_SCALER2 8
#define SIZEOF_CF3_RUN_REALS_PLUS_ACC 8
#define SIZEOF_CF3_RUN_ACC 8
#define SIZEOF_CF3_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_CF3_RUN_MULT_ACC 1024
#define SIZEOF_CF3_RUN_SINGLES_RATE 8
#define SIZEOF_CF3_RUN_DOUBLES_RATE 8
#define SIZEOF_CF3_RUN_TRIPLES_RATE 8
#define SIZEOF_CF3_RUN_SCALER1_RATE 8
#define SIZEOF_CF3_RUN_SCALER2_RATE 8
#define SIZEOF_CF3_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_CF3_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_CF3_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_CF3_RUN_MASS 8
#define SIZEOF_CF3_HIGH_VOLTAGE 8
#define SIZEOF_CF3_SPARE 80
#define SIZEOF_CF4_RUN_NUMBER 2
#define SIZEOF_CF4_RUN_DATE 9
#define SIZEOF_CF4_RUN_TIME 9
#define SIZEOF_CF4_RUN_TESTS 21
#define SIZEOF_CF4_RUN_COUNT_TIME 8
#define SIZEOF_CF4_RUN_SINGLES 8
#define SIZEOF_CF4_RUN_SCALER1 8
#define SIZEOF_CF4_RUN_SCALER2 8
#define SIZEOF_CF4_RUN_REALS_PLUS_ACC 8
#define SIZEOF_CF4_RUN_ACC 8
#define SIZEOF_CF4_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_CF4_RUN_MULT_ACC 1024
#define SIZEOF_CF4_RUN_SINGLES_RATE 8
#define SIZEOF_CF4_RUN_DOUBLES_RATE 8
#define SIZEOF_CF4_RUN_TRIPLES_RATE 8
#define SIZEOF_CF4_RUN_SCALER1_RATE 8
#define SIZEOF_CF4_RUN_SCALER2_RATE 8
#define SIZEOF_CF4_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_CF4_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_CF4_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_CF4_RUN_MASS 8
#define SIZEOF_CF4_HIGH_VOLTAGE 8
#define SIZEOF_CF4_SPARE 80
#define SIZEOF_CF5_RUN_NUMBER 2
#define SIZEOF_CF5_RUN_DATE 9
#define SIZEOF_CF5_RUN_TIME 9
#define SIZEOF_CF5_RUN_TESTS 21
#define SIZEOF_CF5_RUN_COUNT_TIME 8
#define SIZEOF_CF5_RUN_SINGLES 8
#define SIZEOF_CF5_RUN_SCALER1 8
#define SIZEOF_CF5_RUN_SCALER2 8
#define SIZEOF_CF5_RUN_REALS_PLUS_ACC 8
#define SIZEOF_CF5_RUN_ACC 8
#define SIZEOF_CF5_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_CF5_RUN_MULT_ACC 1024
#define SIZEOF_CF5_RUN_SINGLES_RATE 8
#define SIZEOF_CF5_RUN_DOUBLES_RATE 8
#define SIZEOF_CF5_RUN_TRIPLES_RATE 8
#define SIZEOF_CF5_RUN_SCALER1_RATE 8
#define SIZEOF_CF5_RUN_SCALER2_RATE 8
#define SIZEOF_CF5_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_CF5_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_CF5_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_CF5_RUN_MASS 8
#define SIZEOF_CF5_HIGH_VOLTAGE 8
#define SIZEOF_CF5_SPARE 80
#define SIZEOF_A1_RUN_NUMBER 2
#define SIZEOF_A1_RUN_DATE 9
#define SIZEOF_A1_RUN_TIME 9
#define SIZEOF_A1_RUN_TESTS 21
#define SIZEOF_A1_RUN_COUNT_TIME 8
#define SIZEOF_A1_RUN_SINGLES 8
#define SIZEOF_A1_RUN_SCALER1 8
#define SIZEOF_A1_RUN_SCALER2 8
#define SIZEOF_A1_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A1_RUN_ACC 8
#define SIZEOF_A1_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A1_RUN_MULT_ACC 1024
#define SIZEOF_A1_RUN_SINGLES_RATE 8
#define SIZEOF_A1_RUN_DOUBLES_RATE 8
#define SIZEOF_A1_RUN_TRIPLES_RATE 8
#define SIZEOF_A1_RUN_SCALER1_RATE 8
#define SIZEOF_A1_RUN_SCALER2_RATE 8
#define SIZEOF_A1_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A1_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A1_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A1_RUN_MASS 8
#define SIZEOF_A1_HIGH_VOLTAGE 8
#define SIZEOF_A1_SPARE 80
#define SIZEOF_A2_RUN_NUMBER 2
#define SIZEOF_A2_RUN_DATE 9
#define SIZEOF_A2_RUN_TIME 9
#define SIZEOF_A2_RUN_TESTS 21
#define SIZEOF_A2_RUN_COUNT_TIME 8
#define SIZEOF_A2_RUN_SINGLES 8
#define SIZEOF_A2_RUN_SCALER1 8
#define SIZEOF_A2_RUN_SCALER2 8
#define SIZEOF_A2_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A2_RUN_ACC 8
#define SIZEOF_A2_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A2_RUN_MULT_ACC 1024
#define SIZEOF_A2_RUN_SINGLES_RATE 8
#define SIZEOF_A2_RUN_DOUBLES_RATE 8
#define SIZEOF_A2_RUN_TRIPLES_RATE 8
#define SIZEOF_A2_RUN_SCALER1_RATE 8
#define SIZEOF_A2_RUN_SCALER2_RATE 8
#define SIZEOF_A2_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A2_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A2_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A2_RUN_MASS 8
#define SIZEOF_A2_HIGH_VOLTAGE 8
#define SIZEOF_A2_SPARE 80
#define SIZEOF_A3_RUN_NUMBER 2
#define SIZEOF_A3_RUN_DATE 9
#define SIZEOF_A3_RUN_TIME 9
#define SIZEOF_A3_RUN_TESTS 21
#define SIZEOF_A3_RUN_COUNT_TIME 8
#define SIZEOF_A3_RUN_SINGLES 8
#define SIZEOF_A3_RUN_SCALER1 8
#define SIZEOF_A3_RUN_SCALER2 8
#define SIZEOF_A3_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A3_RUN_ACC 8
#define SIZEOF_A3_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A3_RUN_MULT_ACC 1024
#define SIZEOF_A3_RUN_SINGLES_RATE 8
#define SIZEOF_A3_RUN_DOUBLES_RATE 8
#define SIZEOF_A3_RUN_TRIPLES_RATE 8
#define SIZEOF_A3_RUN_SCALER1_RATE 8
#define SIZEOF_A3_RUN_SCALER2_RATE 8
#define SIZEOF_A3_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A3_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A3_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A3_RUN_MASS 8
#define SIZEOF_A3_HIGH_VOLTAGE 8
#define SIZEOF_A3_SPARE 80
#define SIZEOF_A4_RUN_NUMBER 2
#define SIZEOF_A4_RUN_DATE 9
#define SIZEOF_A4_RUN_TIME 9
#define SIZEOF_A4_RUN_TESTS 21
#define SIZEOF_A4_RUN_COUNT_TIME 8
#define SIZEOF_A4_RUN_SINGLES 8
#define SIZEOF_A4_RUN_SCALER1 8
#define SIZEOF_A4_RUN_SCALER2 8
#define SIZEOF_A4_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A4_RUN_ACC 8
#define SIZEOF_A4_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A4_RUN_MULT_ACC 1024
#define SIZEOF_A4_RUN_SINGLES_RATE 8
#define SIZEOF_A4_RUN_DOUBLES_RATE 8
#define SIZEOF_A4_RUN_TRIPLES_RATE 8
#define SIZEOF_A4_RUN_SCALER1_RATE 8
#define SIZEOF_A4_RUN_SCALER2_RATE 8
#define SIZEOF_A4_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A4_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A4_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A4_RUN_MASS 8
#define SIZEOF_A4_HIGH_VOLTAGE 8
#define SIZEOF_A4_SPARE 80
#define SIZEOF_A5_RUN_NUMBER 2
#define SIZEOF_A5_RUN_DATE 9
#define SIZEOF_A5_RUN_TIME 9
#define SIZEOF_A5_RUN_TESTS 21
#define SIZEOF_A5_RUN_COUNT_TIME 8
#define SIZEOF_A5_RUN_SINGLES 8
#define SIZEOF_A5_RUN_SCALER1 8
#define SIZEOF_A5_RUN_SCALER2 8
#define SIZEOF_A5_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A5_RUN_ACC 8
#define SIZEOF_A5_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A5_RUN_MULT_ACC 1024
#define SIZEOF_A5_RUN_SINGLES_RATE 8
#define SIZEOF_A5_RUN_DOUBLES_RATE 8
#define SIZEOF_A5_RUN_TRIPLES_RATE 8
#define SIZEOF_A5_RUN_SCALER1_RATE 8
#define SIZEOF_A5_RUN_SCALER2_RATE 8
#define SIZEOF_A5_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A5_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A5_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A5_RUN_MASS 8
#define SIZEOF_A5_HIGH_VOLTAGE 8
#define SIZEOF_A5_SPARE 80
#define SIZEOF_A6_RUN_NUMBER 2
#define SIZEOF_A6_RUN_DATE 9
#define SIZEOF_A6_RUN_TIME 9
#define SIZEOF_A6_RUN_TESTS 21
#define SIZEOF_A6_RUN_COUNT_TIME 8
#define SIZEOF_A6_RUN_SINGLES 8
#define SIZEOF_A6_RUN_SCALER1 8
#define SIZEOF_A6_RUN_SCALER2 8
#define SIZEOF_A6_RUN_REALS_PLUS_ACC 8
#define SIZEOF_A6_RUN_ACC 8
#define SIZEOF_A6_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_A6_RUN_MULT_ACC 1024
#define SIZEOF_A6_RUN_SINGLES_RATE 8
#define SIZEOF_A6_RUN_DOUBLES_RATE 8
#define SIZEOF_A6_RUN_TRIPLES_RATE 8
#define SIZEOF_A6_RUN_SCALER1_RATE 8
#define SIZEOF_A6_RUN_SCALER2_RATE 8
#define SIZEOF_A6_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_A6_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_A6_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_A6_RUN_MASS 8
#define SIZEOF_A6_HIGH_VOLTAGE 8
#define SIZEOF_A6_SPARE 80
#define SIZEOF_B1_RUN_NUMBER 2
#define SIZEOF_B1_RUN_DATE 9
#define SIZEOF_B1_RUN_TIME 9
#define SIZEOF_B1_RUN_TESTS 21
#define SIZEOF_B1_RUN_COUNT_TIME 8
#define SIZEOF_B1_RUN_SINGLES 8
#define SIZEOF_B1_RUN_SCALER1 8
#define SIZEOF_B1_RUN_SCALER2 8
#define SIZEOF_B1_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B1_RUN_ACC 8
#define SIZEOF_B1_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B1_RUN_MULT_ACC 1024
#define SIZEOF_B1_RUN_SINGLES_RATE 8
#define SIZEOF_B1_RUN_DOUBLES_RATE 8
#define SIZEOF_B1_RUN_TRIPLES_RATE 8
#define SIZEOF_B1_RUN_SCALER1_RATE 8
#define SIZEOF_B1_RUN_SCALER2_RATE 8
#define SIZEOF_B1_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B1_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B1_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B1_RUN_MASS 8
#define SIZEOF_B1_HIGH_VOLTAGE 8
#define SIZEOF_B1_SPARE 80
#define SIZEOF_B2_RUN_NUMBER 2
#define SIZEOF_B2_RUN_DATE 9
#define SIZEOF_B2_RUN_TIME 9
#define SIZEOF_B2_RUN_TESTS 21
#define SIZEOF_B2_RUN_COUNT_TIME 8
#define SIZEOF_B2_RUN_SINGLES 8
#define SIZEOF_B2_RUN_SCALER1 8
#define SIZEOF_B2_RUN_SCALER2 8
#define SIZEOF_B2_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B2_RUN_ACC 8
#define SIZEOF_B2_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B2_RUN_MULT_ACC 1024
#define SIZEOF_B2_RUN_SINGLES_RATE 8
#define SIZEOF_B2_RUN_DOUBLES_RATE 8
#define SIZEOF_B2_RUN_TRIPLES_RATE 8
#define SIZEOF_B2_RUN_SCALER1_RATE 8
#define SIZEOF_B2_RUN_SCALER2_RATE 8
#define SIZEOF_B2_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B2_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B2_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B2_RUN_MASS 8
#define SIZEOF_B2_HIGH_VOLTAGE 8
#define SIZEOF_B2_SPARE 80
#define SIZEOF_B3_RUN_NUMBER 2
#define SIZEOF_B3_RUN_DATE 9
#define SIZEOF_B3_RUN_TIME 9
#define SIZEOF_B3_RUN_TESTS 21
#define SIZEOF_B3_RUN_COUNT_TIME 8
#define SIZEOF_B3_RUN_SINGLES 8
#define SIZEOF_B3_RUN_SCALER1 8
#define SIZEOF_B3_RUN_SCALER2 8
#define SIZEOF_B3_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B3_RUN_ACC 8
#define SIZEOF_B3_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B3_RUN_MULT_ACC 1024
#define SIZEOF_B3_RUN_SINGLES_RATE 8
#define SIZEOF_B3_RUN_DOUBLES_RATE 8
#define SIZEOF_B3_RUN_TRIPLES_RATE 8
#define SIZEOF_B3_RUN_SCALER1_RATE 8
#define SIZEOF_B3_RUN_SCALER2_RATE 8
#define SIZEOF_B3_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B3_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B3_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B3_RUN_MASS 8
#define SIZEOF_B3_HIGH_VOLTAGE 8
#define SIZEOF_B3_SPARE 80
#define SIZEOF_B4_RUN_NUMBER 2
#define SIZEOF_B4_RUN_DATE 9
#define SIZEOF_B4_RUN_TIME 9
#define SIZEOF_B4_RUN_TESTS 21
#define SIZEOF_B4_RUN_COUNT_TIME 8
#define SIZEOF_B4_RUN_SINGLES 8
#define SIZEOF_B4_RUN_SCALER1 8
#define SIZEOF_B4_RUN_SCALER2 8
#define SIZEOF_B4_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B4_RUN_ACC 8
#define SIZEOF_B4_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B4_RUN_MULT_ACC 1024
#define SIZEOF_B4_RUN_SINGLES_RATE 8
#define SIZEOF_B4_RUN_DOUBLES_RATE 8
#define SIZEOF_B4_RUN_TRIPLES_RATE 8
#define SIZEOF_B4_RUN_SCALER1_RATE 8
#define SIZEOF_B4_RUN_SCALER2_RATE 8
#define SIZEOF_B4_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B4_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B4_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B4_RUN_MASS 8
#define SIZEOF_B4_HIGH_VOLTAGE 8
#define SIZEOF_B4_SPARE 80
#define SIZEOF_B5_RUN_NUMBER 2
#define SIZEOF_B5_RUN_DATE 9
#define SIZEOF_B5_RUN_TIME 9
#define SIZEOF_B5_RUN_TESTS 21
#define SIZEOF_B5_RUN_COUNT_TIME 8
#define SIZEOF_B5_RUN_SINGLES 8
#define SIZEOF_B5_RUN_SCALER1 8
#define SIZEOF_B5_RUN_SCALER2 8
#define SIZEOF_B5_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B5_RUN_ACC 8
#define SIZEOF_B5_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B5_RUN_MULT_ACC 1024
#define SIZEOF_B5_RUN_SINGLES_RATE 8
#define SIZEOF_B5_RUN_DOUBLES_RATE 8
#define SIZEOF_B5_RUN_TRIPLES_RATE 8
#define SIZEOF_B5_RUN_SCALER1_RATE 8
#define SIZEOF_B5_RUN_SCALER2_RATE 8
#define SIZEOF_B5_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B5_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B5_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B5_RUN_MASS 8
#define SIZEOF_B5_HIGH_VOLTAGE 8
#define SIZEOF_B5_SPARE 80
#define SIZEOF_B6_RUN_NUMBER 2
#define SIZEOF_B6_RUN_DATE 9
#define SIZEOF_B6_RUN_TIME 9
#define SIZEOF_B6_RUN_TESTS 21
#define SIZEOF_B6_RUN_COUNT_TIME 8
#define SIZEOF_B6_RUN_SINGLES 8
#define SIZEOF_B6_RUN_SCALER1 8
#define SIZEOF_B6_RUN_SCALER2 8
#define SIZEOF_B6_RUN_REALS_PLUS_ACC 8
#define SIZEOF_B6_RUN_ACC 8
#define SIZEOF_B6_RUN_MULT_REALS_PLUS_ACC 1024
#define SIZEOF_B6_RUN_MULT_ACC 1024
#define SIZEOF_B6_RUN_SINGLES_RATE 8
#define SIZEOF_B6_RUN_DOUBLES_RATE 8
#define SIZEOF_B6_RUN_TRIPLES_RATE 8
#define SIZEOF_B6_RUN_SCALER1_RATE 8
#define SIZEOF_B6_RUN_SCALER2_RATE 8
#define SIZEOF_B6_RUN_MULTIPLICITY_MULT 8
#define SIZEOF_B6_RUN_MULTIPLICITY_ALPHA 8
#define SIZEOF_B6_MULTIPLICITY_EFFICIENCY 8
#define SIZEOF_B6_RUN_MASS 8
#define SIZEOF_B6_HIGH_VOLTAGE 8
#define SIZEOF_B6_SPARE 80
#define SIZEOF_AD_TYPE 2
#define SIZEOF_AD_PORT_NUMBER 2
#define SIZEOF_AD_FORWARD_OVER_TRAVEL 8
#define SIZEOF_AD_REVERSE_OVER_TRAVEL 8
#define SIZEOF_AD_NUMBER_POSITIONS 2
#define SIZEOF_AD_DIST_TO_MOVE 40
#define SIZEOF_CM_STEPS_PER_INCH 8
#define SIZEOF_CM_FORWARD_MASK 4
#define SIZEOF_CM_REVERSE_MASK 4
#define SIZEOF_CM_AXIS_NUMBER 2
#define SIZEOF_CM_OVER_TRAVEL_STATE 4
#define SIZEOF_CM_STEP_RATIO 8
#define SIZEOF_CM_SLOW_INCHES 8
#define SIZEOF_PLC_STEPS_PER_INCH 8
#define SIZEOF_SCALE_CONVERSION_FACTOR 8
#define SIZEOF_CM_ROTATION 1
#define SIZEOF_STRATUM 13
#define SIZEOF_STRATUM_ID_DETECTOR_ID 12
#define SIZEOF_STRATUM_BIAS_UNCERTAINTY 8
#define SIZEOF_STRATUM_RANDOM_UNCERTAINTY 8
#define SIZEOF_STRATUM_SYSTEMATIC_UNCERTAINTY 8
#define SIZEOF_DE_NEUTRON_ENERGY 200
#define SIZEOF_DE_DETECTOR_EFFICIENCY 200
#define SIZEOF_DE_INNER_OUTER_RING_RATIO 200
#define SIZEOF_DE_RELATIVE_FISSION 200
#define SIZEOF_DE_INNER_RING_EFFICIENCY 8
#define SIZEOF_DE_OUTER_RING_EFFICIENCY 8
#define SIZEOF_DE_ITEM_TYPE 6
#define SIZEOF_DE_DETECTOR_ID 12
#define SIZEOF_DE_MEAS_RING_RATIO 8
#define SIZEOF_DE_INTERPOLATED_NEUTRON_ENERGY 8
#define SIZEOF_DE_ENERGY_CORR_FACTOR 8
#define SIZEOF_DE_NEUTRON_ENERGY_RES 200
#define SIZEOF_DE_DETECTOR_EFFICIENCY_RES 200
#define SIZEOF_DE_INNER_OUTER_RING_RATIO_RES 200
#define SIZEOF_DE_RELATIVE_FISSION_RES 200
#define SIZEOF_DE_INNER_RING_EFFICIENCY_RES 8
#define SIZEOF_DE_OUTER_RING_EFFICIENCY_RES 8
#define SIZEOF_DE_MULT_ITEM_TYPE 6
#define SIZEOF_DE_MULT_DETECTOR_ID 12

#endif    /* NCC_DB_H */
