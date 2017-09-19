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
#define MULTI_ARRAY_SIZE 128		/* size of multiplicity arrays need 512 of these now hn 5.5.2015*/
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

database ncc_db
{
    data file "parms.dbf"		contains system,
						archive_parms_rec,
						bkg_parms_rec,
						tm_bkg_parms_rec,
						norm_parms_rec,
						sr_parms_rec,
						test_parms_rec,
						acquire_parms_rec,
						item_type_names_rec,
						facility_names_rec,
						mba_names_rec,
						stratum_id_names_rec,
						inventory_change_code_rec,
						io_code_rec,
						add_a_source_setup_rec,
						assay_summary_rec,
						holdup_summary_rec;
    data file "detector.dbf"		contains detector_rec;
    data file[32000] "item_ids.dbf"	contains item_id_entry_rec;
    data file "col_data.dbf"		contains collar_data_entry_rec,
						poison_rod_type_rec,
						collar_k5_rec;
    data file "holdup.dbf"		contains holdup_config_rec;
    data file "stratum.dbf"		contains stratum_id_rec;
    data file "method.dbf"		contains analysis_method_rec;
    data file "cal_curv.dbf"		contains cal_curve_rec;
    data file "known_a.dbf"		contains known_alpha_rec;
    data file "known_m.dbf"		contains known_m_rec;
    data file "multi.dbf"		contains multiplicity_rec;
    data file "de_multi.dbf"		contains de_mult_rec;
    data file "act_pass.dbf"		contains active_passive_rec;
    data file "active.dbf"		contains active_rec;
    data file "collar.dbf"		contains collar_rec,
						collar_detector_rec;
    data file "addasrc.dbf"		contains add_a_source_rec;
    data file "act_mult.dbf"		contains active_mult_rec;
    data file "cm_ratio.dbf"		contains curium_ratio_rec;
    data file "truncate.dbf"		contains truncated_mult_rec;
    data file "isotopic.dbf"		contains isotopics_rec;
    data file "comp_iso.dbf"		contains composite_isotopics_rec;
    data file[6400] "results.dbf"	contains results_rec;
    data file "res_bias.dbf"		contains results_bias_rec,
						results_init_src_rec;
    data file "res_prec.dbf"		contains results_precision_rec;
    data file "res_cc.dbf"		contains results_cal_curve_rec;
    data file "res_ka.dbf"		contains results_known_alpha_rec;
    data file "res_km.dbf"		contains results_known_m_rec;
    data file "res_mul.dbf"		contains results_multiplicity_rec;
    data file "resdemul.dbf"		contains results_de_mult_rec;
    data file "res_ap.dbf"		contains results_active_passive_rec;
    data file "res_act.dbf"		contains results_active_rec;
    data file "res_col.dbf"		contains results_collar_rec;
    data file "res_ad.dbf"		contains results_add_a_source_rec;
    data file "res_am.dbf"		contains results_active_mult_rec;
    data file[3000] "run.dbf"		contains run_rec,
						cf1_run_rec,
						cf2_run_rec,
						cf3_run_rec,
						cf4_run_rec,
						cf5_run_rec,
						a1_run_rec,
						a2_run_rec,
						a3_run_rec,
						a4_run_rec,
						a5_run_rec,
						a6_run_rec,
						b1_run_rec,
						b2_run_rec,
						b3_run_rec,
						b4_run_rec,
						b5_run_rec,
						b6_run_rec;
    data file[3200] "alfabeta.dbf"	contains alpha_beta_rec;
    data file "parms310.dbf"		contains cm_pu_ratio_rec,
						mba_item_id_entry_rec,
						unattended_parms_rec;
    data file "res_cr.dbf"		contains results_curium_ratio_rec;
    data file "res_tm.dbf"		contains results_truncated_mult_rec,
						results_tm_bkg_rec;

    key file "detector.key"		contains detector_id;
    key file "holdup.key"		contains glovebox_id;
    key file "isotopic.key"		contains isotopics_id;
    key file "comp_iso.key"		contains ci_isotopics_id;
    key file "stratum.key"		contains stratum_id_key;
    key file "method.key"		contains analysis_method;
    key file "cal_curv.key"		contains analysis_method_cal_curve;
    key file "known_a.key"		contains analysis_method_known_alpha;
    key file "known_m.key"		contains analysis_method_known_m;
    key file "multi.key"		contains analysis_method_multiplicity;
    key file "de_multi.key"		contains analysis_method_de_mult;
    key file "act_pass.key"		contains analysis_method_active_passive;
    key file "active.key"		contains analysis_method_active;
    key file "collar.key"		contains analysis_method_collar,
						analysis_method_collar_detector,
						analysis_method_collar_k5;
    key file "addasrc.key"		contains analysis_method_add_a_source;
    key file "act_mult.key"		contains analysis_method_active_mult;
    key file "results.key"		contains meas_id, report_id,
						stratum_report_id,
						detector_item_id;
    key file "cm_ratio.key"		contains analysis_method_curium_ratio;
    key file "truncate.key"		contains analysis_method_truncated_mult;

    record archive_parms_rec
    {
	unsigned short days_before_auto_archive;
	unsigned short days_before_auto_delete;
	unsigned short days_before_db_auto_delete;
	char data_dir[MAX_PATH_LENGTH];
    }

    set archive_parms_set
    {
	order last;
	owner system;
	member archive_parms_rec;
    }

    record test_parms_rec
    {
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
    }

    set test_parms_set
    {
	order last;
	owner system;
	member test_parms_rec;
    }

    record acquire_parms_rec
    {
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
    }

    set acquire_parms_set
    {
	order last;
	owner system;
	member acquire_parms_rec;
    }

    record item_id_entry_rec
    {
	char item_id_entry[MAX_ITEM_IDS][MAX_ITEM_ID_LENGTH];
	char item_type_ascii[MAX_ITEM_IDS][MAX_ITEM_TYPE_LENGTH];
	char isotopics_id_ascii[MAX_ITEM_IDS][MAX_ISOTOPICS_ID_LENGTH];
	char stratum_id_ascii[MAX_ITEM_IDS][MAX_STRATUM_ID_LENGTH];
	char inventory_change_code_entry[MAX_ITEM_IDS][INVENTORY_CHG_LENGTH];
	char io_code_entry[MAX_ITEM_IDS][IO_CODE_LENGTH];
	double declared_mass_entry[MAX_ITEM_IDS];
	double declared_u_mass_entry[MAX_ITEM_IDS];
	double length_entry[MAX_ITEM_IDS];
    }

    set item_id_entry_set
    {
	order last;
	owner system;
	member item_id_entry_rec;
    }

    record mba_item_id_entry_rec
    {
	char mba_ascii[MAX_ITEM_IDS][MBA_LENGTH];
    }

    set mba_item_id_entry_set
    {
	order last;
	owner system;
	member mba_item_id_entry_rec;
    }

    record collar_data_entry_rec
    {
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
    }

    set collar_data_entry_set
    {
	order last;
	owner system;
	member collar_data_entry_rec;
    }

    record poison_rod_type_rec
    {
	char poison_rod_type[MAX_POISON_ROD_TYPES][MAX_ROD_TYPE_LENGTH];
	double poison_absorption_fact[MAX_POISON_ROD_TYPES];
    }

    set poison_rod_type_set
    {
	order last;
	owner system;
	member poison_rod_type_rec;
    }

    record holdup_config_rec
    {
	unique key char glovebox_id[MAX_GLOVEBOX_ID_LENGTH];
	unsigned short num_rows;
	unsigned short num_columns;
	double distance;
    }

    set holdup_config_set
    {
	order last;
	owner system;
	member holdup_config_rec;
    }

    record cm_pu_ratio_rec
    {
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
    }

    set cm_pu_ratio_set
    {
	order last;
	owner system;
	member cm_pu_ratio_rec;
    }

    record item_type_names_rec
    {
	char item_type_names[MAX_ITEM_TYPES][MAX_ITEM_TYPE_LENGTH];
    }

    set item_type_names_set
    {
	order last;
	owner system;
	member item_type_names_rec;
    }

    record facility_names_rec
    {
	char facility_names[MAX_FACILITIES][FACILITY_LENGTH];
	char facility_description[MAX_FACILITIES][DESCRIPTION_LENGTH];
    }

    set facility_names_set
    {
	order last;
	owner system;
	member facility_names_rec;
    }

    record mba_names_rec
    {
	char mba_names[MAX_MBAS][MBA_LENGTH];
	char mba_description[MAX_MBAS][DESCRIPTION_LENGTH];
    }

    set mba_names_set
    {
	order last;
	owner system;
	member mba_names_rec;
    }

    record stratum_id_names_rec
    {
	char stratum_id_names[MAX_STRATUM_IDS][MAX_STRATUM_ID_LENGTH];
	char stratum_id_names_description[MAX_STRATUM_IDS][DESCRIPTION_LENGTH];
    }

    set stratum_id_names_set
    {
	order last;
	owner system;
	member stratum_id_names_rec;
    }

    record inventory_change_code_rec
    {
	char inventory_chg_codes[MAX_INVENTORY_CHG_CODES][INVENTORY_CHG_LENGTH];
    }

    set inventory_change_code_set
    {
	order last;
	owner system;
	member inventory_change_code_rec;
    }

    record io_code_rec
    {
	char io_codes[MAX_IO_CODES][IO_CODE_LENGTH];
    }

    set io_code_set
    {
	order last;
	owner system;
	member io_code_rec;
    }

    record isotopics_rec
    {
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
	unique key char isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
	char isotopics_source_code[ISO_SOURCE_CODE_LENGTH];
    }

    set isotopics_set
    {
	order ascending;
	owner system;
	member isotopics_rec by isotopics_id;
    }

    record composite_isotopics_rec
    {
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
	unique key char ci_isotopics_id[MAX_ISOTOPICS_ID_LENGTH];
	char ci_isotopics_source_code[ISO_SOURCE_CODE_LENGTH];
    }

    set composite_isotopics_set
    {
	order ascending;
	owner system;
	member composite_isotopics_rec by ci_isotopics_id;
    }

    record assay_summary_rec
    {
	unsigned char as_print;
	char as_path[MAX_PATH_LENGTH];
	int as_select[MAX_ASSAY_SUMMARY_VARS];
    }

    set assay_summary_set
    {
	order last;
	owner system;
	member assay_summary_rec;
    }

    record holdup_summary_rec
    {
	unsigned char hu_print;
	char hu_path[MAX_PATH_LENGTH];
	int hu_select[MAX_HOLDUP_SUMMARY_VARS];
    }

    set holdup_summary_set
    {
	order last;
	owner system;
	member holdup_summary_rec;
    }

    record detector_rec
    {
	unique key char detector_id[MAX_DETECTOR_ID_LENGTH];
	char detector_type[DETECTOR_TYPE_LENGTH];
	char electronics_id[ELECTRONICS_ID_LENGTH];
    }

    set detector_set
    {
	order last;
	owner system;
	member detector_rec;
    }

    record alpha_beta_rec
    {
	double factorial[MULTI_ARRAY_SIZE];
	double alpha_array[MULTI_ARRAY_SIZE];
	double beta_array[MULTI_ARRAY_SIZE];
    }

    set detector_alpha_beta_set
    {
	order last;
	owner detector_rec;
	member alpha_beta_rec;
    }

    record bkg_parms_rec
    {
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
    }

    set detector_bkg_parms_set
    {
	order last;
	owner detector_rec;
	member bkg_parms_rec;
    }

    record tm_bkg_parms_rec
    {
	double tm_singles_bkg;
	double tm_singles_bkg_err;
	double tm_zeros_bkg;
	double tm_zeros_bkg_err;
	double tm_ones_bkg;
	double tm_ones_bkg_err;
	double tm_twos_bkg;
	double tm_twos_bkg_err;
	unsigned char tm_bkg;
    }

    set detector_tm_bkg_parms_set
    {
	order last;
	owner detector_rec;
	member tm_bkg_parms_rec;
    }

    record norm_parms_rec
    {
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
    }

    set detector_norm_parms_set
    {
	order last;
	owner detector_rec;
	member norm_parms_rec;
    }

    record sr_parms_rec
    {
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
    }

    set detector_sr_parms_set
    {
	order last;
	owner detector_rec;
	member sr_parms_rec;
    }

    record unattended_parms_rec
    {
	unsigned long error_seconds;
	unsigned char auto_import;
	double add_a_source_threshold;
    }

    set detector_unattended_parms_set
    {
	order last;
	owner detector_rec;
	member unattended_parms_rec;
    }

    record analysis_method_rec
    {
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
	compound key analysis_method
	{
	    item_type ascending;
	    analysis_method_detector_id ascending;
	}
    }

    record cal_curve_rec	/* must be identical to active_rec */
    {
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
	compound key analysis_method_cal_curve
	{
	    cal_curve_item_type ascending;
	    cal_curve_detector_id ascending;
	}
    }

    record active_rec		/* must be identical to cal_curve_rec */
    {
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
	compound key analysis_method_active
	{
	    active_item_type ascending;
	    active_detector_id ascending;
	}
    }

    /* first part must be identical to cal_curve_rec and active_rec */
    record collar_rec
    {
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
	compound key analysis_method_collar
	{
	    collar_item_type ascending;
	    collar_mode ascending;
	}
    }

    record collar_detector_rec
    {
	char collar_detector_item_type[MAX_ITEM_TYPE_LENGTH];
	unsigned char collar_detector_mode;
	char collar_detector_id[MAX_DETECTOR_ID_LENGTH];
	char col_reference_date[DATE_TIME_LENGTH];
	double col_relative_doubles_rate;
	compound key analysis_method_collar_detector
	{
	    collar_detector_item_type ascending;
	    collar_detector_mode ascending;
	    collar_detector_id ascending;
	}
    }

    record collar_k5_rec
    {
	char collar_k5_label[MAX_COLLAR_K5_PARAMETERS][MAX_K5_LABEL_LENGTH];
	int collar_k5_checkbox[MAX_COLLAR_K5_PARAMETERS];
	double collar_k5[MAX_COLLAR_K5_PARAMETERS];
	double collar_k5_err[MAX_COLLAR_K5_PARAMETERS];
	char collar_k5_item_type[MAX_ITEM_TYPE_LENGTH];
	unsigned char collar_k5_mode;
	compound key analysis_method_collar_k5
	{
	    collar_k5_item_type ascending;
	    collar_k5_mode ascending;
	}
    }

    record known_alpha_rec
    {
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
	compound key analysis_method_known_alpha
	{
	    known_alpha_item_type ascending;
	    known_alpha_detector_id ascending;
	}
    }

    record known_m_rec
    {
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
	compound key analysis_method_known_m
	{
	    known_m_item_type ascending;
	    known_m_detector_id ascending;
	}
    }

    record multiplicity_rec
    {
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
	compound key analysis_method_multiplicity
	{
	    multiplicity_item_type ascending;
	    multiplicity_detector_id ascending;
	}
    }

    /* first part must be identical to cal_curve_rec and active_rec */
    record active_passive_rec
    {
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
	compound key analysis_method_active_passive
	{
	    active_passive_item_type ascending;
	    active_passive_detector_id ascending;
	}
    }

    /* first part must be identical to cal_curve_rec and active_rec */
    record add_a_source_rec
    {
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
	compound key analysis_method_add_a_source
	{
	    add_a_source_item_type ascending;
	    add_a_source_detector_id ascending;
	}
    }

    record active_mult_rec
    {
	double am_vt1;
	double am_vt2;
	double am_vt3;
	double am_vf1;
	double am_vf2;
	double am_vf3;
	double active_mult_spares[NUMBER_AM_SPARES];
	char active_mult_item_type[MAX_ITEM_TYPE_LENGTH];
	char active_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
	compound key analysis_method_active_mult
	{
	    active_mult_item_type ascending;
	    active_mult_detector_id ascending;
	}
    }

    record curium_ratio_rec  /* first part must be identical to cal_curve_rec */
    {
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
	compound key analysis_method_curium_ratio
	{
	    curium_ratio_item_type ascending;
	    curium_ratio_detector_id ascending;
	}
    }

    record truncated_mult_rec
    {
	double tm_a;
	double tm_b;
	unsigned char tm_known_eff;
	unsigned char tm_solve_eff;
	double truncated_mult_spares[NUMBER_TM_SPARES];
	char truncated_mult_item_type[MAX_ITEM_TYPE_LENGTH];
	char truncated_mult_detector_id[MAX_DETECTOR_ID_LENGTH];
	compound key analysis_method_truncated_mult
	{
	    truncated_mult_item_type ascending;
	    truncated_mult_detector_id ascending;
	}
    }

    record results_rec
    {
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
	compound key meas_id
	{
	    meas_date ascending;
	    meas_time ascending;
	    filename ascending;
	    results_detector_id ascending;
	}
	compound key report_id
	{
	    results_detector_id ascending;
	    stratum_id ascending;
	    item_id ascending;
	    meas_date descending;
	    meas_time descending;
	}
	compound key stratum_report_id
	{
	    stratum_id ascending;
	    results_detector_id ascending;
	    item_id ascending;
	    meas_date descending;
	    meas_time descending;
	}
	compound key detector_item_id
	{
	    results_detector_id ascending;
	    item_id ascending;
	}
    }

    set detector_results_set
    {
	order next;
	owner detector_rec;
	member results_rec;
    }

    record results_init_src_rec
    {
	char init_src_id[SOURCE_ID_LENGTH];
	char init_src_pass_fail[PASS_FAIL_LENGTH];
	unsigned short init_src_mode;
    }

    set results_init_src_set
    {
	order last;
	owner results_rec;
	member results_init_src_rec;
    }

    record results_bias_rec
    {
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
    }

    set results_bias_set
    {
	order last;
	owner results_rec;
	member results_bias_rec;
    }

    record results_precision_rec
    {
	char prec_pass_fail[PASS_FAIL_LENGTH];
	double prec_sample_var;
	double prec_theoretical_var;
	double prec_chi_sq;
	double chi_sq_lower_limit;
	double chi_sq_upper_limit;
	double prec_spares[NUMBER_PREC_SPARES];
    }

    set results_precision_set
    {
	order last;
	owner results_rec;
	member results_precision_rec;
    }

    record results_cal_curve_rec
    {
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
    }

    set results_cal_curve_set
    {
	order last;
	owner results_rec;
	member results_cal_curve_rec;
    }

    record results_known_alpha_rec
    {
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
	double ka_corr_singles;			/* may be ring ratio */
	double ka_corr_singles_err;		/* may be ring ratio error */
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
    }

    set results_known_alpha_set
    {
	order last;
	owner results_rec;
	member results_known_alpha_rec;
    }

    record results_known_m_rec
    {
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
    }

    set results_known_m_set
    {
	order last;
	owner results_rec;
	member results_known_m_rec;
    }

    record results_multiplicity_rec
    {
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
    }

    set results_multiplicity_set
    {
	order last;
	owner results_rec;
	member results_multiplicity_rec;
    }

    record results_active_passive_rec
    {
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
    }

    set results_active_passive_set
    {
	order last;
	owner results_rec;
	member results_active_passive_rec;
    }

    record results_active_rec
    {
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
    }

    set results_active_set
    {
	order last;
	owner results_rec;
	member results_active_rec;
    }

    record results_collar_rec
    {
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
    }

    set results_collar_set
    {
	order last;
	owner results_rec;
	member results_collar_rec;
    }

    record results_add_a_source_rec
    {
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
    }

    set results_add_a_source_set
    {
	order last;
	owner results_rec;
	member results_add_a_source_rec;
    }

    record results_active_mult_rec
    {
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
    }

    set results_active_mult_set
    {
	order last;
	owner results_rec;
	member results_active_mult_rec;
    }

    record results_curium_ratio_rec
    {
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
	double cr_cm_pu_ratio;	/* must be in same order as cm_pu_ratio_rec */
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
    }

    set results_curium_ratio_set
    {
	order last;
	owner results_rec;
	member results_curium_ratio_rec;
    }

    record results_truncated_mult_rec
    {
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
    }

    set results_truncated_mult_set
    {
	order last;
	owner results_rec;
	member results_truncated_mult_rec;
    }

    record results_tm_bkg_rec
    {
	double results_tm_singles_bkg;
	double results_tm_singles_bkg_err;
	double results_tm_zeros_bkg;
	double results_tm_zeros_bkg_err;
	double results_tm_ones_bkg;
	double results_tm_ones_bkg_err;
	double results_tm_twos_bkg;
	double results_tm_twos_bkg_err;
    }

    set results_tm_bkg_set
    {
	order last;
	owner results_rec;
	member results_tm_bkg_rec;
    }

    record run_rec		/* must be identical to cf run records */
    {
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
    }

    set results_run_set
    {
	order last;
	owner results_rec;
	member run_rec;
    }

    record cf1_run_rec		/* must be identical to run records */
    {
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
    }

    set results_cf1_run_set
    {
	order last;
	owner results_rec;
	member cf1_run_rec;
    }

    record cf2_run_rec		/* must be identical to run records */
    {
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
    }

    set results_cf2_run_set
    {
	order last;
	owner results_rec;
	member cf2_run_rec;
    }

    record cf3_run_rec		/* must be identical to run records */
    {
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
    }

    set results_cf3_run_set
    {
	order last;
	owner results_rec;
	member cf3_run_rec;
    }

    record cf4_run_rec		/* must be identical to run records */
    {
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
    }

    set results_cf4_run_set
    {
	order last;
	owner results_rec;
	member cf4_run_rec;
    }

    record cf5_run_rec		/* must be identical to run records */
    {
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
    }

    set results_cf5_run_set
    {
	order last;
	owner results_rec;
	member cf5_run_rec;
    }

    record a1_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a1_run_set
    {
	order last;
	owner results_rec;
	member a1_run_rec;
    }

    record a2_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a2_run_set
    {
	order last;
	owner results_rec;
	member a2_run_rec;
    }

    record a3_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a3_run_set
    {
	order last;
	owner results_rec;
	member a3_run_rec;
    }

    record a4_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a4_run_set
    {
	order last;
	owner results_rec;
	member a4_run_rec;
    }

    record a5_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a5_run_set
    {
	order last;
	owner results_rec;
	member a5_run_rec;
    }

    record a6_run_rec		/* must be identical to run records */
    {
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
    }

    set results_a6_run_set
    {
	order last;
	owner results_rec;
	member a6_run_rec;
    }

    record b1_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b1_run_set
    {
	order last;
	owner results_rec;
	member b1_run_rec;
    }

    record b2_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b2_run_set
    {
	order last;
	owner results_rec;
	member b2_run_rec;
    }

    record b3_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b3_run_set
    {
	order last;
	owner results_rec;
	member b3_run_rec;
    }

    record b4_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b4_run_set
    {
	order last;
	owner results_rec;
	member b4_run_rec;
    }

    record b5_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b5_run_set
    {
	order last;
	owner results_rec;
	member b5_run_rec;
    }

    record b6_run_rec		/* must be identical to run records */
    {
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
    }

    set results_b6_run_set
    {
	order last;
	owner results_rec;
	member b6_run_rec;
    }

    record add_a_source_setup_rec
    {
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
    }

    set detector_add_a_source_setup_set
    {
	order last;
	owner detector_rec;
	member add_a_source_setup_rec;
    }

/* stratum_id_rec, de_mult_rec, results_de_mult_rec and results_de_mult_set
   were added for INCC version 4.00 */

    record stratum_id_rec
    {
	char stratum[MAX_STRATUM_ID_LENGTH];
	char stratum_id_detector_id[MAX_DETECTOR_ID_LENGTH];
	double stratum_bias_uncertainty;
	double stratum_random_uncertainty;
	double stratum_systematic_uncertainty;
	compound key stratum_id_key
	{
	    stratum ascending;
	    stratum_id_detector_id ascending;
	}
    }

    record de_mult_rec
    {
	double de_neutron_energy[MAX_DUAL_ENERGY_ROWS];
	double de_detector_efficiency[MAX_DUAL_ENERGY_ROWS];
	double de_inner_outer_ring_ratio[MAX_DUAL_ENERGY_ROWS];
	double de_relative_fission[MAX_DUAL_ENERGY_ROWS];
	double de_inner_ring_efficiency;
	double de_outer_ring_efficiency;
	char de_item_type[MAX_ITEM_TYPE_LENGTH];
	char de_detector_id[MAX_DETECTOR_ID_LENGTH];
	compound key analysis_method_de_mult
	{
	    de_item_type ascending;
	    de_detector_id ascending;
	}
    }

    record results_de_mult_rec
    {
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
    }

    set results_de_mult_set
    {
	order last;
	owner results_rec;
	member results_de_mult_rec;
    }
}
