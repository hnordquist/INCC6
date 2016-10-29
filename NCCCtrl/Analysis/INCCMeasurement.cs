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
using INCCCore;

namespace AnalysisDefs
{
	using Tuple = AnalysisDefs.VTuple;
	using NC = NCC.CentralizedState;
	using Integ = NCC.IntegrationHelpers;
	public static class MeasurementExtensions
    {

        #region INCC calculation control methods

        private const int NUM_ACC_SNGL_WARNING = 5;
        private const int NUM_OUTLIER_WARNING = 5;
        private const int NUM_CHECKSUM_WARNING = 5;
        private const int NUM_HIGH_VOLTAGE_WARNING = 5;

        /// <summary>
        /// after each cycle, run this cycle limit test and exit code
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="mkey"></param>
        public static void CycleStatusTerminationCheck(this Measurement meas, Cycle cc)
        {
            bool stopAndComputeResults = meas.AcquireState.lm.SaveOnTerminate;
            IEnumerator iter = cc.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;

                if (meas.MeasCycleStatus.num_checksum_failures > meas.Tests.maxNumFailures)
                {
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 7071, "Maximum checksum failure count met, cycle {0} {1}", cc.seq, mkey);
                    if (stopAndComputeResults)
                    {
                        // URGENT: set state to end of measurement so that state moves out of cycles and skips forward to CalculateMeasurementResults
                    }
                    else
                    {
                        // exit processing completely, do not compute or save results
                        NC.App.Opstate.Abort();
                    }
                }
                if (meas.MeasCycleStatus.num_acc_sngl_failures > meas.Tests.maxNumFailures)
                {
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 7072, "Maximum A/S failure count met, cycle {0} {1}", cc.seq, mkey);
                    // ditto above comment block                    
                }
                if (meas.MeasCycleStatus.num_high_voltage_failures > meas.Tests.maxNumFailures)
                {
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 7073, "Maximum checksum failure count met, cycle {0} {1}", cc.seq, mkey);
                    // ditto above comment block     
                }
                if ((meas.MeasCycleStatus.num_acc_sngl_failures >= NUM_ACC_SNGL_WARNING) &&
                     (!meas.MeasCycleStatus.acc_sngl_warning_sent))
                {
                    meas.AddWarningMessage("At least " + meas.MeasCycleStatus.num_acc_sngl_failures + " A/S test failures", 7074, mkey);
                    meas.MeasCycleStatus.acc_sngl_warning_sent = true;
                }
                if ((meas.MeasCycleStatus.num_checksum_failures >= NUM_CHECKSUM_WARNING) &&
                    (!meas.MeasCycleStatus.checksum_warning_sent))
                {
                    meas.AddWarningMessage("At least " + meas.MeasCycleStatus.num_checksum_failures + " checksum failures", 7075, mkey);
                    meas.MeasCycleStatus.checksum_warning_sent = true;
                }
                if ((meas.MeasCycleStatus.num_high_voltage_failures >= NUM_HIGH_VOLTAGE_WARNING) &&
                    (!meas.MeasCycleStatus.high_voltage_warning_sent))
                {
                    meas.AddWarningMessage("At least " + meas.MeasCycleStatus.num_high_voltage_failures + " high voltage failures", 7076, mkey);
                    meas.MeasCycleStatus.high_voltage_warning_sent = true;
                }
                if (meas.AcquireState.acquire_type == AcquireConvergence.CycleCount)
                {
                    meas.MeasCycleStatus.acquire_num_runs = meas.MeasCycleStatus.initial_num_runs +
                        meas.MeasCycleStatus.num_outlier_failures + meas.MeasCycleStatus.num_acc_sngl_failures +
                        meas.MeasCycleStatus.num_checksum_failures;
                }
                else if (meas.Cycles.GetValidCycleCountForThisKey(mkey) < 100) // an arbitrary limit, make it a config item
                {
                    // URGENT: set state to end of measurement so that state moves out of cycles and skips forward to CalculateMeasurementResults
                    // todo: see take_data.cpp line 733 for mass check processing steps
                    if (meas.Cycles.GetValidCycleCountForThisKey(mkey) > 1)
                    {
                        INCCResult results;
                        MeasOptionSelector ar = new MeasOptionSelector(meas.MeasOption, mkey);
                        bool found = meas.INCCAnalysisResults.TryGetValue(ar, out results);
                        double error = 0;
                        if (meas.AcquireState.acquire_type == AcquireConvergence.DoublesPrecision)
                        {
                            if (results.rates.DTCRates.Doubles.v != 0.0)
                            {
                                error = results.rates.DTCRates.Doubles.err / results.rates.DTCRates.Doubles.v * 100.0;
                            }
                        }
                        else if (meas.AcquireState.acquire_type == AcquireConvergence.TriplesPrecision)
                        {
                            if (results.rates.DTCRates.Triples.v != 0.0)
                            {
                                error = results.rates.DTCRates.Triples.err / results.rates.DTCRates.Triples.v * 100.0;
                            }
                        }
                        else if (meas.AcquireState.acquire_type == AcquireConvergence.Pu240EffPrecision)
                        {
                            RatesAdjustments dtchoice = RatesAdjustments.Raw; // NEXT: design out this choice everywhere, it shoud be a flag on the current computational state somewhere 
                            INCCMethodResults.results_multiplicity_rec res = INCCAnalysis.CalculateMultiplicity(mkey, results.covariance_matrix, results.rates.GetDTCRates(dtchoice), meas, dtchoice);
                            if (res != null)
                            {
                                // the results obtained within the method are already stored on the result and they do not need to be copied here (VERIFY THIS)
                                if (res.pu240e_mass.v != 0.0)
                                {
                                    error = res.pu240e_mass.err /
                                        res.pu240e_mass.v * 100.0;
                                }
                                else
                                    error = 0.0;
                            }
                        }

                        meas.MeasCycleStatus.acquire_num_runs = (uint)((double)cc.seq * (error / meas.AcquireState.meas_precision) * (error / meas.AcquireState.meas_precision)) + 1;
                        meas.MeasCycleStatus.acquire_num_runs += meas.MeasCycleStatus.num_outlier_failures +
                            meas.MeasCycleStatus.num_acc_sngl_failures + meas.MeasCycleStatus.num_checksum_failures;
                    }
                    else
                        meas.MeasCycleStatus.acquire_num_runs = meas.AcquireState.max_num_runs;
                    if (meas.MeasCycleStatus.acquire_num_runs > meas.AcquireState.max_num_runs)
                        meas.MeasCycleStatus.acquire_num_runs = meas.AcquireState.max_num_runs;
                    if (meas.MeasCycleStatus.acquire_num_runs < meas.AcquireState.min_num_runs)
                        meas.MeasCycleStatus.acquire_num_runs = meas.AcquireState.min_num_runs;
                }
            }
        }

		public static void SetQCStatus(this Measurement meas, Cycle cycle)
		{
            IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
                cycle.SetQCStatus(mkey, QCTestStatus.Pass, cycle.HighVoltage);  // prep for analyis one by one
			}

		}

         //<summary>
         //This is a tentative summary over all cycles of the first stage counting results
         //Any simple sums/averages are finished here after the per-cycle Accumulate methods are used to accumulate the sums
         //But for the INCC cross-cycle methods, see CalcAveragesAndSums
         //</summary>
        public static void GenerateCycleCountingSummaries(this Measurement meas, bool ignoreSuspectResults = true)
        {
            if (NC.App.Opstate.IsAbortRequested)
                return;

            uint validMultCyclesCount = meas.CycleSummary(ignoreSuspectResults);  // APluralityOfMultiplicityAnalyzers: this may need to summarize across analyzers, as well as the single value on the measurement
			if (meas.Cycles.Count < 1)  // nothing can be done
				return;
			if (validMultCyclesCount == 0 && !meas.HasReportableData) // nothing can be done
				return;

			foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in meas.CountingAnalysisResults)  // set to cycle max bin counts, per multi analyzer, by key
            {
                if (pair.Key is Multiplicity && !pair.Key.suspect)
                {
                    ulong maxbins = 0; ulong minbins = 0;
                    MultiplicityCountingRes mcr = (MultiplicityCountingRes)pair.Value;
                    object obj; MultiplicityCountingRes ccm;
                    foreach (Cycle cc in meas.Cycles)
                    {
                        bool there = cc.CountingAnalysisResults.TryGetValue(pair.Key, out obj);
                        if (!there)
                            continue;
                        ccm = (MultiplicityCountingRes)obj;
                        maxbins = Math.Max(maxbins, ccm.MaxBins);
                        minbins = Math.Max(minbins, ccm.MinBins);
                    }
                    mcr.MaxBins = Math.Max(maxbins, mcr.MaxBins);
                    mcr.MinBins = Math.Max(minbins, mcr.MinBins);
                    Array.Resize(ref mcr.RAMult, (int)mcr.MaxBins);
                    Array.Resize(ref mcr.NormedAMult, (int)mcr.MaxBins);
                    Array.Resize(ref mcr.UnAMult, (int)mcr.MaxBins);
                    mcr.AB.Resize((int)mcr.MaxBins);

					double denom = 0.0;
                    if (mcr.S1Sum == 0 && mcr.S2Sum == 0)
					    foreach (Cycle cc in meas.Cycles)
                        {
						    if (!cc.QCStatusValid((Multiplicity)pair.Key))
							    continue;
                            bool there = cc.CountingAnalysisResults.TryGetValue(pair.Key, out obj);
                            if (!there)
                                continue;
                            ccm = (MultiplicityCountingRes)obj;

						    denom++;
                            mcr.S1Sum += ccm.Scaler1.v;
                            mcr.S2Sum += ccm.Scaler2.v;
                        }
					if (denom > 0) // error-corrected scaler rates wrongly carried in these two vars from CalcAvgAndSums after this step, should fix this someday
					{
						mcr.Scaler1.v = mcr.S1Sum / denom;
						mcr.Scaler2.v = mcr.S2Sum / denom;
					}
                }
                else if (pair.Key is BaseRate && !pair.Key.suspect) // Rates counts merge
                {
                    uint maxbins = 0;
                    RatesResultEnhanced rrm = (RatesResultEnhanced)pair.Value;
                    foreach (Cycle cc in meas.Cycles)
                    {
                        RatesResultEnhanced ccrrm = (RatesResultEnhanced)cc.CountingAnalysisResults[pair.Key];
                        maxbins = Math.Max(maxbins, ccrrm.completedIntervals);
                    }
                    rrm.Reset(Math.Max(rrm.completedIntervals, maxbins));
                }
            }

            try
            {
               //  dev note: need a trim function that marches through R+A and ANorm removing dual 0s from the end of the arrays, apply per cycle and at this summary point, prior to reporting and storage
                foreach (Cycle cc in meas.Cycles)
                {
                    foreach (KeyValuePair<SpecificCountingAnalyzerParams, object> pair in meas.CountingAnalysisResults)
                    {
                        if (!pair.Key.suspect) // accumulate the relevant counts
                        {
                            ICountingResult cr = (ICountingResult)pair.Value;
							object obj;
                            bool there = cc.CountingAnalysisResults.TryGetValue(pair.Key, out obj);
                            if (!there)
                                continue;
                            cr.Accumulate((ICountingResult)obj);
                        }
                    }
                }

                if (meas.Cycles.Count > 0)  // adjustments
                {
                    // average the summed coincidence matrix rates
                    IEnumerator iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.Coincidence));
                    while (iter.MoveNext())
                    {
                        if (meas.CountTimeInSeconds == 0)
                            continue;
                        CoincidenceMatrixResult cor = (CoincidenceMatrixResult)iter.Current;
                        int len = cor.RACoincidenceRate.Length;  // assuming always symmetric numXnum

                        for (int i = 0; i < cor.RACoincidenceRate.Length; i++)
                        {
                            for (int j = 0; j < cor.RACoincidenceRate.Length; j++)
                            {
                                cor.RACoincidenceRate[i][j] /= validMultCyclesCount;
                                cor.ACoincidenceRate[i][j] /= validMultCyclesCount;
                            }
                        }
                    }

                   //  make sure total time is there for the rates
                    uint validGeneralCyclesCount = meas.NumberOfRawCyclesWithCounts();
                    iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.BaseRate));
                    while (iter.MoveNext())
                    {
                        if (meas.CountTimeInSeconds == 0)
                            continue;
                        RatesResultEnhanced bre = (RatesResultEnhanced)iter.Current;
                        bre.totaltime = new TimeSpan(bre.totaltime.Ticks / validGeneralCyclesCount);  // avg ticks per cycle 
                    }

                    // do the final Feynman computation
                    IEnumerator fritter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.Feynman));
                    while (fritter.MoveNext())
                    {
                        FeynmanResultExt fr = (FeynmanResultExt)fritter.Current;
                        FeynmanResult nfr = LMRawAnalysis.FeynmanGateAnalysis.GetResultFromFeynmanGateDictionary(fr.numGatesHavingNumNeutrons, false);
                        fr.C = nfr.C;
                        fr.cbar = nfr.cbar;
                        fr.c2bar = nfr.c2bar;
                        fr.c3bar = nfr.c3bar;
                    }                                  
                }

                // fill in the traditional results rec values
                if (meas.INCCAnalysisResults.TradResultsRec != null)
                {
                    uint gc = meas.Cycles.GetValidCycleCount();
                    uint tnc = meas.NumberOfRawCyclesWithCounts();
                    meas.INCCAnalysisResults.TradResultsRec.total_number_runs = (int)tnc;
                    if (gc > 0)
                        meas.INCCAnalysisResults.TradResultsRec.total_good_count_time = (tnc / gc);
                }
            }
            catch (Exception ex)
            {
                meas.Logger.TraceEvent(NCCReporter.LogLevels.Error, 4040, "GenerateCycleSums error in cycle loop: " + ex.Message);
            }
        }



        /// <summary>
        /// Work the second and third phases of INCC mass calculations.
        /// This is called after all cycles are collected and then conditioned and filtered.
        /// </summary>
        public static void CalculateMeasurementResults(this Measurement meas)
        {
            if (meas.Cycles.GetValidCycleCount() > 0) // INCC5 Pu mass calcs
            {
                meas.OutlierProcessing(); // summary pass at the end of all the cycles
                meas.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
                meas.CalcAvgAndSums();
                meas.CalculateResults();
            } else // everything else
                meas.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
        }


        /// <summary>
        /// Do the third phase  . . .
        /// From calc_res.cpp,
        /// </summary>
        public static void CalculateResults(this Measurement meas)
        {
            IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                if (NC.App.Opstate.IsAbortRequested)
                    return;

                INCCResult results;
                MeasOptionSelector ar = new MeasOptionSelector(meas.MeasOption, mkey);
                bool found = meas.INCCAnalysisResults.TryGetValue(ar, out results);
				if (!found)
				{
					meas.AddErrorMessage("No results available", 10151, mkey);
					return;
				}
                /* if using measure to precision, and max # runs reached, then add warning message indicating actual precision reached. */
                if (meas.AcquireState.acquire_type == AcquireConvergence.DoublesPrecision)
                {
                    if (results.rates.DTCRates.DoublesRate != 0.0)
                    {
                        double error = results.rates.DTCRates.Doubles.err / results.rates.DTCRates.DoublesRate * 100.0;
                        if (error > meas.AcquireState.meas_precision)
                        {
                            meas.AddWarningMessage("Measurement doubles error = " + error.ToString("F2"), 10126, mkey);
                        }
                    }
                }
                else if (meas.AcquireState.acquire_type == AcquireConvergence.TriplesPrecision)
                {
                    if (results.rates.DTCRates.TriplesRate != 0.0)
                    {
                        double error = results.rates.DTCRates.Triples.err / results.rates.DTCRates.TriplesRate * 100.0;
                        if (error > meas.AcquireState.meas_precision)
                        {
                            meas.AddWarningMessage("Measurement triples error = " + error.ToString("F2"), 10127, mkey);
                        }
                    }
                }

                if (meas.MeasOption == AssaySelector.MeasurementOption.rates)   // Doug  Requirement #2
                {
                    // for a rates only measurement, all done!
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10180, "Rates Only measurement complete");
                    continue;
                }

                try
                {
                    switch (meas.MeasOption)
                    {
                        case AssaySelector.MeasurementOption.background: // Doug  Requirement #3
                            // a bkg is an average over a bunch of cycles with only the deadtime correction applied, 
                            // but note the variant for Truncated Mult background, where an additional calculation is made

                            if (results.rates.DTCRates.DoublesRate > meas.Tests.bkgDoublesRateLimit)
                                meas.AddWarningMessage("Background doubles rate " + results.rates.DTCRates.DoublesRate + " greater than " + meas.Tests.bkgDoublesRateLimit, 10141, mkey);
                            if (results.rates.DTCRates.TriplesRate > meas.Tests.bkgTriplesRateLimit)
                                meas.AddWarningMessage("Background triples rate " + results.rates.DTCRates.TriplesRate + " greater than " + meas.Tests.bkgTriplesRateLimit, 10142, mkey);

                            // dev note: this is a background measurement per se, so we copy the results to the Background class and store the entire thing that way
                            // NEXT: Need to account for Active bkg beginning here.                            
                            
                            if (Integ.GetCurrentAcquireParams().well_config == WellConfiguration.Active)// Is an active acquisition.
                            {
                                meas.Background.INCCActive.CopyFrom(results.rates.DeadtimeCorrectedRates);
                            }
                            else
                                meas.Background.CopyFrom(results.rates);
                            //  maybe if (INCCAnalysisState.Methods.Has(AnalysisMethod.TruncatedMultiplicity))
                            if (meas.Background.TMBkgParams.ComputeTMBkg)
                                // Trunc Mult Bkg step, calc_tm_rates, sets TM bkg rates on Measurement.Background
                                INCCAnalysis.calc_tm_rates(mkey, results, meas, meas.Background.TMBkgParams, meas.Detector.Id.SRType);

                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10181, "Background measurement complete");
                            break;
                        case AssaySelector.MeasurementOption.initial:

                            INCCResults.results_init_src_rec results_init_src = (INCCResults.results_init_src_rec)results;

                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10182, "Calculating Initial source measurement results");
                            bool funhappy = INCCAnalysis.initial_source_meas(meas, mkey, RatesAdjustments.DeadtimeCorrected);
                            if (!funhappy || !results_init_src.pass)
                            {
                                meas.AddWarningMessage("Initial source measurement failed", 10123, mkey);
                            }
                            // on fail, only the relevant results_init_src_rec is saved
                            // on pass, the normalization parameters are modified with the results_init_src_rec results, and so both are udpated.
                            break;
                        case AssaySelector.MeasurementOption.normalization:

                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10183, "Calculating Normalization measurement results");
                            bool happyfun = INCCAnalysis.bias_test(meas, mkey, RatesAdjustments.DeadtimeCorrected);
                            if (!happyfun)
                            {
                                meas.AddWarningMessage("Normalization test -- data quality is inadequate", 10124, mkey);
                            }
                            break;
                        case AssaySelector.MeasurementOption.precision:

                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10184, "Calculating Precision measurement results");
                            bool charmyfun = INCCAnalysis.precision_test(meas, mkey, RatesAdjustments.DeadtimeCorrected);
                            if (!charmyfun)
                            {
                                meas.AddWarningMessage("Precision test failed", 10125, mkey);
                            }
                            break;
                        case AssaySelector.MeasurementOption.calibration: // from calc_res.cpp
                            if (meas.INCCAnalysisState.Methods.CalibrationAnalysisSelected())
                            {
                                meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10185, "Calculating Calibration measurement results");
                                // dev note: since the analysis routines have similar signatures, design a class OK?
                                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.CalibrationCurve))
                                {
                                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10191, "Calculating " + AnalysisMethod.CalibrationCurve.FullName() + " measurement results");
                                    // get the current results_cal_curve_rec
                                    INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)
                                        meas.INCCAnalysisResults.LookupMethodResults(mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.CalibrationCurve, true);

                                    INCCAnalysisParams.cal_curve_rec cal_curve = (INCCAnalysisParams.cal_curve_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.CalibrationCurve);
                                    ccres.methodParams = new INCCAnalysisParams.cal_curve_rec(cal_curve);

                                    double pu_mass = ccres.pu_mass.v;
                                    meas.Isotopics.UpdateDeclaredPuMass(meas.MeasDate, ref pu_mass, INCCAnalysis.INCCParity);
                                    ccres.pu_mass.v = pu_mass;
                                    ccres.pu_mass.err = 0.0;
                                    ccres.pu240e_mass.err = 0.0;

                                    double pu240e_mass = ccres.pu240e_mass.v;
                                    meas.Isotopics.calc_pu240e(pu_mass, out pu240e_mass, meas);
                                    ccres.pu240e_mass.v = pu240e_mass;

                                }
                                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.KnownA))
                                {
                                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10192, "Calculating " + AnalysisMethod.KnownA.FullName() + " calibration results");
                                    INCCMethodResults.results_known_alpha_rec kares = INCCAnalysis.CalculateKnownAlpha(mkey, results.rates, meas, RatesAdjustments.DeadtimeCorrected);
                                    if (kares == null)
                                    {
                                        meas.AddErrorMessage("Known alpha analysis error", 10153, mkey);
                                    }
                                    else
                                    {
                                        kares.pu_mass.v = meas.AcquireState.mass;
                                        double pu_mass = kares.pu_mass.v;
                                        meas.Isotopics.UpdateDeclaredPuMass(meas.MeasDate, ref pu_mass, INCCAnalysis.INCCParity);
                                        kares.pu_mass.v = pu_mass;
                                        kares.pu_mass.err = 0.0;
                                        kares.pu240e_mass.err = 0.0;

                                        double pu240e_mass = kares.pu240e_mass.v;
                                        meas.Isotopics.calc_pu240e(pu_mass, out pu240e_mass, meas);
                                        kares.pu240e_mass.v = pu240e_mass;
                                    }
                                }
                                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.Active))
                                {
                                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10192, "Arranging " + AnalysisMethod.Active.FullName() + " calibration results");

                                    INCCSelector sel = new INCCSelector(meas.INCCAnalysisState.Methods.selector);
                                    INCCAnalysisParams.active_rec act;
                                    INCCAnalysisParams.INCCMethodDescriptor surr = meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.Active);
                                    if (surr == null)
                                    {
                                        act = new INCCAnalysisParams.active_rec();
                                        meas.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.Active, act);
                                    }
                                    else
                                    {
                                        act = (INCCAnalysisParams.active_rec)surr;
                                    }
                                    INCCMethodResults.results_active_rec res;
                                    res = (INCCMethodResults.results_active_rec)meas.INCCAnalysisResults.LookupMethodResults(mkey, sel, AnalysisMethod.Active, true);

                                    // all this does is copy the declared mass over to the results, WTF
                                    res.u235_mass.v = meas.AcquireState.mass;
                                    
                                }
                                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.AddASource))
                                {
                                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10152, "Calculating " + AnalysisMethod.AddASource.FullName() + " calibration results");
                                    INCCSelector sel = new INCCSelector(meas.INCCAnalysisState.Methods.selector);
                                    INCCAnalysisParams.add_a_source_rec aas;
                                    INCCMethodResults.results_add_a_source_rec acres;
                                    INCCAnalysisParams.INCCMethodDescriptor surr = meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.AddASource);
                                    if (surr == null)
                                    {
                                        aas = new INCCAnalysisParams.add_a_source_rec();
                                        meas.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.AddASource, aas);
                                    }
                                    else
                                    {
                                        aas = (INCCAnalysisParams.add_a_source_rec)surr;
                                    }
                                    acres = (INCCMethodResults.results_add_a_source_rec)meas.INCCAnalysisResults.LookupMethodResults(mkey, sel, AnalysisMethod.AddASource, true);

                                    acres.pu_mass.v = meas.AcquireState.mass;
                                    // update_declared_pu_mass 
                                    double pu_mass = acres.pu_mass.v;
                                    meas.Isotopics.UpdateDeclaredPuMass(meas.MeasDate, ref pu_mass, INCCAnalysis.INCCParity);
                                    acres.pu_mass.v = pu_mass;
                                    acres.pu_mass.err = 0.0;
                                    acres.pu240e_mass.err = 0.0;

                                    double pu240e_mass = acres.pu240e_mass.v;
                                    meas.Isotopics.calc_pu240e(pu_mass, out pu240e_mass, meas);
                                    acres.pu240e_mass.v = pu240e_mass;

                                }
                            }
                            else
                            {
                                meas.AddErrorMessage("No Calibration analysis methods selected", 10153, mkey);
                            }
                            break;
                        case AssaySelector.MeasurementOption.verification:
                            // see INCC calc_asy.cpp
                            // dev note: check for item in the item table, make sure to place this item id on the MeasurementId.item property
                            if (!string.IsNullOrEmpty(meas.AcquireState.item_id))
                            {
                                meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10194, "Using item id '{0}'", meas.AcquireState.item_id);
                            }
                            else
                                meas.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 10194, "No item id");

                            if (meas.INCCAnalysisState.Methods.VerificationAnalysisSelected())
                            {
                                meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10186, "Calculating {0} measurement results", meas.MeasOption.PrintName());

                                meas.CalculateVerificationResults(mkey, results);
                            }
                            else
                            {
                                meas.AddErrorMessage("No analysis methods selected", 10156, mkey);
                            }
                            break;
                        case AssaySelector.MeasurementOption.holdup:   // NEXT: Hold-up held up, implement it #35 
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Error, 10187, "Holdup analysis unsupported");
                            break;
                    }
                }
                catch (Exception e)
                {
                    meas.Logger.TraceException(e);
                }
            }

        }


        static void CalculateVerificationResults(this Measurement meas, Multiplicity mkey, MultiplicityCountingRes results)
        {
            Tuple normal_mass = new Tuple(-1, 0), backup_mass = new Tuple(-1, 0);
            try
            {
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.CalibrationCurve))
                {
                    // get the current results_cal_curve_rec and cal_curve params

                    //dev note: the rates as computed by the first and second phases are not yet on ccres, because they exist soley on the counting results MultiplicityCountingRes instance

                    INCCMethodResults.results_cal_curve_rec ccres = (INCCMethodResults.results_cal_curve_rec)
                        meas.INCCAnalysisResults.LookupMethodResults(mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.CalibrationCurve, true);
                    INCCAnalysisParams.cal_curve_rec cal_curve = (INCCAnalysisParams.cal_curve_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.CalibrationCurve);
                    INCCAnalysisParams.CalCurveResult status = INCCAnalysisParams.CalCurveResult.Unknown;
                    if (cal_curve == null)
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 10199, "No " + AnalysisMethod.KnownA.FullName() + " method parameters found");
                        return;
                    }
                    if (cal_curve.CalCurveType != INCCAnalysisParams.CalCurveType.HM)
                    {
                        Tuple pu240e = new Tuple();
                        Tuple doubles;
                        if (cal_curve.cev.useSingles)  // the 2009 MTS hack
                            doubles = new Tuple(results.rates.GetDTCRates(RatesAdjustments.DeadtimeCorrected).Singles);
                        else
                            doubles = new Tuple(results.rates.GetDTCRates(RatesAdjustments.DeadtimeCorrected).Doubles);
                        status = INCCAnalysis.CalculateCalibrationCurveOnly(cal_curve.cev, out pu240e, results.rates, doubles, RatesAdjustments.DeadtimeCorrected); // rates (triples) not used
                        ccres.pu240e_mass = pu240e;
                    }
                    else
                    {
                        // get the item id from the acquire record or the ItemId on the measurement itself
                        // if there is no item id use the empty default item id
                        // dev note: at some point the acquire record item id becomes a full ItemId record on the measurmeent
                        //if not NC.App.DB.ItemIdSet.Contains AcquireState.item_id then
                        //    get the default empty one
                        //end 

                        // from HEAVY_M.cpp
                        INCCAnalysis.calc_heavy_metal(
                                cal_curve.heavy_metal_corr_factor,
                                cal_curve.heavy_metal_reference,
                                results.rates.DTCRates.Singles,
                                results.rates.DTCRates.Doubles,
                                ref ccres.heavy_metal_content,
                                ref ccres.heavy_metal_correction,
                                ref ccres.heavy_metal_corr_singles,
                                ref ccres.heavy_metal_corr_doubles, meas);

                        status = INCCAnalysis.CalculateCalibrationCurveOnly(cal_curve.cev,
                                        out ccres.pu240e_mass,
                                        results.rates, ccres.heavy_metal_corr_doubles,
                                        RatesAdjustments.DeadtimeCorrected);

                        ccres.pu240e_mass.v *= meas.MeasurementId.Item.length;
                        ccres.pu240e_mass.err *= meas.MeasurementId.Item.length;
                    }

                    if (status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        string msg = String.Format("Passive calibration curve failed mass limits of {0} and {1}", cal_curve.cev.lower_mass_limit, cal_curve.cev.upper_mass_limit);
                        meas.AddErrorMessage(msg, 10196, mkey);
                    }
                    else if (status != INCCAnalysisParams.CalCurveResult.Success)
                    {
                        meas.AddErrorMessage("Passive calibration curve analysis error", 10197, mkey);
                    }
                    if (status == INCCAnalysisParams.CalCurveResult.Success || status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        ccres.dcl_pu_mass = meas.AcquireState.mass; // another requirement for the acquire state
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 10133, "calc_mass/calc_u235_mass are called next");
                        if (cal_curve.CalCurveType != INCCAnalysisParams.CalCurveType.U)
                        {
                            INCCAnalysis.calc_mass(ccres.pu240e_mass,
                                ref ccres.pu_mass, ref ccres.dcl_pu_mass, ref ccres.dcl_pu240e_mass, ref ccres.dcl_minus_asy_pu_mass, ref ccres.dcl_minus_asy_pu_mass_pct, ref ccres.pass,
                                meas);
                        }
                        else
                        {
                            INCCAnalysis.calc_u235_mass(cal_curve.percent_u235, ccres.pu240e_mass,
                               ref ccres.pu_mass, ref ccres.dcl_pu_mass, ref ccres.dcl_minus_asy_pu_mass, ref ccres.dcl_minus_asy_pu_mass_pct, ref ccres.pass,
                                meas);
                        }

                        if (!ccres.pass)
                        {
                            meas.AddWarningMessage("Passive calibration curve: failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Passive calibration curve: passed stratum rejection limits", 10200, mkey);
                        }

                        if (ccres.pu240e_mass.v > ccres.methodParams.cev.upper_mass_limit)
                        {
                            meas.AddWarningMessage("Passive calibration curve: upper Pu240e mass limit exceeded.", 10210, mkey);
                        }
                        if (ccres.pu240e_mass.v < ccres.methodParams.cev.lower_mass_limit)
                        {
                            meas.AddWarningMessage("Passive calibration curve: lower Pu240e mass limit exceeded.", 10211, mkey);
                        }
                    }
                    // normal and backup retention
                    if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.CalibrationCurve)
                    {
                        normal_mass.CopyFrom(ccres.pu240e_mass);
                    }
                    if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.CalibrationCurve)
                    {
                        backup_mass.CopyFrom(ccres.pu240e_mass);
                    }

                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.KnownA))
                {
                    INCCMethodResults.results_known_alpha_rec kares = (INCCMethodResults.results_known_alpha_rec)
                    meas.INCCAnalysisResults.LookupMethodResults(mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.KnownA, true);
                    INCCAnalysisParams.known_alpha_rec ka_params = (INCCAnalysisParams.known_alpha_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.KnownA);
                    if (ka_params == null)
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 10199, "No Known alpha method parameters found");
                        return;
                    }
                    bool success = false;
                    kares.dcl_pu_mass = meas.AcquireState.mass;  // dev note: another use of acq, a requirement, here
                    // copy the input calibration params to the copy on the results rec, to be saved with the KA results
                    kares.methodParams = new INCCAnalysisParams.known_alpha_rec(ka_params);

                    if (ka_params.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.Conventional)
                    {
                        INCCMethodResults.results_known_alpha_rec karesdup = INCCAnalysis.CalculateKnownAlpha(mkey, results.rates, meas, RatesAdjustments.DeadtimeCorrected); // rates (triples) not used
                        if (karesdup != null) // we have the new mass results, and they are preserved in the results map
                        {
                            success = true;
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Known alpha results for pu240E {0} +- {1}", karesdup.pu240e_mass.v, karesdup.pu240e_mass.err);
                        }
                    }
                    else if (ka_params.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.HeavyMetalCorrection)
                    {
                        INCCAnalysis.calc_heavy_metal(
                                ka_params.heavy_metal_corr_factor,
                                ka_params.heavy_metal_reference,
                                results.rates.DTCRates.Singles,
                                results.rates.DTCRates.Doubles,
                                ref kares.heavy_metal_content,
                                ref kares.heavy_metal_correction,
                                ref kares.corr_singles,
                                ref kares.corr_doubles, meas);

                        Rates HMSDRates = new Rates();
                        HMSDRates.DeadtimeCorrectedRates.Singles.CopyFrom(kares.corr_singles);
                        HMSDRates.DeadtimeCorrectedRates.Doubles.CopyFrom(kares.corr_doubles);

                        INCCMethodResults.results_known_alpha_rec karesdup = INCCAnalysis.CalculateKnownAlpha(mkey, HMSDRates, meas, RatesAdjustments.DeadtimeCorrected);
                        if (karesdup != null) // we have the new mass results, and they are preserved in the results map
                        {
                            success = true;
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Known alpha HM results for pu240E {0} +- {1}", karesdup.pu240e_mass.v, karesdup.pu240e_mass.err);
                        }
                        kares.pu240e_mass.v *= meas.MeasurementId.Item.length;
                        kares.pu240e_mass.err *= meas.MeasurementId.Item.length;
                    }
                    else if (ka_params.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToDryAlpha)
                    {
                        success =
                         INCCAnalysis.calc_known_alpha_moisture_corr(
                                    results.rates.DTCRates.Singles,
                                    results.rates.DTCRates.Doubles,
                                    results.Scaler1, results.Scaler2,
                                    ref kares.corr_singles, /* ring ratio */
                                    ref kares.corr_factor,
                                    ref kares.dry_alpha_or_mult_dbls, /* dry alpha */
                                    ref kares.mult_corr_doubles,
                                    ref kares.mult,
                                    ref kares.alphaK,
                                    ref kares.pu240e_mass, ka_params, meas, mkey);
                        if (success)
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Known alpha MoistureCorrAppliedToDryAlpha results for pu240E {0} +- {1}", kares.pu240e_mass.v, kares.pu240e_mass.err);

                    }
                    else if (ka_params.known_alpha_type == INCCAnalysisParams.KnownAlphaVariant.MoistureCorrAppliedToMultCorrDoubles)
                    {
                        success =
                            INCCAnalysis.calc_known_alpha_moisture_corr_mult_doubles(
                                    results.rates.DTCRates.Singles,
                                    results.rates.DTCRates.Doubles,
                                    results.Scaler1, results.Scaler2,
                                    ref kares.corr_singles, /* ring ratio */
                                    ref kares.corr_factor,
                                    ref kares.dry_alpha_or_mult_dbls, /* moist mult_corr_doubles */
                                    ref kares.mult_corr_doubles,
                                    ref kares.mult,
                                    ref kares.alphaK,
                                    ref kares.pu240e_mass, ka_params, meas, mkey);
                        if (success)
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Known alpha MoistureCorrAppliedToMultCorrDoubles results for pu240E {0} +- {1}", kares.pu240e_mass.v, kares.pu240e_mass.err);

                    }

                    if (success)
                    {
                        INCCAnalysis.calc_mass(kares.pu240e_mass,
                            ref kares.pu_mass, ref kares.dcl_pu_mass, ref kares.dcl_pu240e_mass, ref kares.dcl_minus_asy_pu_mass, ref kares.dcl_minus_asy_pu_mass_pct, ref kares.pass,
                            meas);
                    }
                    else
                    {
                        meas.AddErrorMessage("Known alpha analysis error", 10199, mkey);
                    }
                    if (!kares.pass)
                    {
                        meas.AddWarningMessage("Known alpha: failed stratum rejection limits", 10198, mkey);
                    }
                    else
                    {
                        meas.AddWarningMessage("Known alpha: passed stratum rejection limits", 10200, mkey);
                    }
                    if (kares.pu240e_mass.v > kares.methodParams.cev.upper_mass_limit)
                    {
                        meas.AddWarningMessage("Known alpha: upper Pu240e mass limit exceeded.", 10210, mkey);
                    }
                    if (kares.pu240e_mass.v < kares.methodParams.cev.lower_mass_limit)
                    {
                        meas.AddWarningMessage("Known alpha: lower Pu240e mass limit exceeded.", 10211, mkey);
                    }
                    // normal and backup retention
                    if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.KnownA)
                    {
                        normal_mass.CopyFrom(kares.pu240e_mass);
                    }
                    if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.KnownA)
                    {
                        backup_mass.CopyFrom(kares.pu240e_mass);
                    }
                }

                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.Multiplicity))
                {
                    double error = 0.0;
                    INCCAnalysisParams.multiplicity_rec mul_param = (INCCAnalysisParams.multiplicity_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.Multiplicity);
                    if (mul_param == null)
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 10198, "No Multiplicity method parameters found");
                        return;
                    }
                    INCCMethodResults.results_multiplicity_rec mmres = (INCCMethodResults.results_multiplicity_rec)meas.INCCAnalysisResults.LookupMethodResults(
                                                              mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.Multiplicity, true);
                    // weird rates (triples) used, but they are wrong (see note line 568 avg_sums.cs)
                    INCCMethodResults.results_multiplicity_rec mmresdup = INCCAnalysis.CalculateMultiplicity(mkey, results.covariance_matrix, results.rates.GetDTCRates(RatesAdjustments.DeadtimeCorrected), meas, RatesAdjustments.DeadtimeCorrected);
                    if (mmresdup != null) // we have the new mass results, and they are preserved in the results map
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Multiplicity results for pu240E {0} +- {1}", mmres.pu240e_mass.v, mmres.pu240e_mass.err);
                        if (meas.AcquireState.acquire_type == AcquireConvergence.Pu240EffPrecision)
                        {
                            if (mmres.pu240e_mass.v != 0.0)
                            {
                                error = mmres.pu240e_mass.err /
                                    mmres.pu240e_mass.v * 100.0;
                                if (error > meas.AcquireState.meas_precision)
                                {
                                    meas.AddWarningMessage(String.Format("Multiplicity: Pu240e error = {0}%", error), 10198, mkey);
                                }
                            }
                        }
                        mmres.dcl_pu_mass = meas.AcquireState.mass;  // another use of acq, a requirement, here

                        INCCAnalysis.calc_mass(mmres.pu240e_mass,
                            ref mmres.pu_mass, ref mmres.dcl_pu_mass, ref mmres.dcl_pu240e_mass, ref mmres.dcl_minus_asy_pu_mass, ref mmres.dcl_minus_asy_pu_mass_pct, ref mmres.pass,
                            meas);

                        if (!mmres.pass)
                        {
                            meas.AddWarningMessage("Multiplicity: failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Multiplicity: passed stratum rejection limits", 10200, mkey);
                        }

                        if (mul_param.solve_efficiency == INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED) // todo: implement Weighted
                        {
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 36010, "CONVENTIONAL_MULT_WEIGHTED Multiplicity measurement results");
                        }
                        else if (mul_param.solve_efficiency == INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL) // todo: implement DE
                        {
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 36010, "MULT_DUAL_ENERGY_MODEL Multiplicity measurement results");
                        }
                        // normal and backup retention
                        if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.Multiplicity)
                        {
                            normal_mass.CopyFrom(mmres.pu240e_mass);
                        }
                        if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.Multiplicity)
                        {
                            backup_mass.CopyFrom(mmres.pu240e_mass);
                        }
                    }
                    else
                    {
                        meas.AddErrorMessage("Multiplicity analysis error", 10198, mkey);
                    }

                    // copy the input calib to the results rec
                    mmres.methodParams = new INCCAnalysisParams.multiplicity_rec(mul_param);

                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.KnownM))
                {
                    INCCMethodResults.results_known_m_rec kmres = INCCAnalysis.CalculateKnownM(mkey, results, meas, RatesAdjustments.DeadtimeCorrected);

                    // calc mass
                    if (kmres != null) // you have calculated well my child 
                    {
                        INCCAnalysis.calc_mass(kmres.pu240e_mass,
                                            ref kmres.pu_mass, ref kmres.dcl_pu_mass, ref kmres.dcl_pu240e_mass,
                                            ref kmres.dcl_minus_asy_pu_mass, ref kmres.dcl_minus_asy_pu_mass_pct, ref kmres.pass, meas);

                        if (!kmres.pass)
                        {
                            meas.AddWarningMessage("Known M: failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Known M: passed stratum rejection limits", 10200, mkey);
                        }

                        if (kmres.pu240e_mass.v > kmres.methodParams.upper_mass_limit)
                        {
                            meas.AddWarningMessage("Known M: upper Pu240e mass limit exceeded.", 10210, mkey);
                        }
                        if (kmres.pu240e_mass.v < kmres.methodParams.lower_mass_limit)
                        {
                            meas.AddWarningMessage("Known M: lower Pu240e mass limit exceeded.", 10211, mkey);
                        }
                        // normal and backup retention
                        if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.KnownM)
                        {
                            normal_mass.CopyFrom(kmres.pu240e_mass);
                        }
                        if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.KnownM)
                        {
                            backup_mass.CopyFrom(kmres.pu240e_mass);
                        }
                    }
                    else
                    {
                        meas.AddErrorMessage("Known M: analysis error", 10198, mkey);
                    }

                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.Active))
                {
                    INCCAnalysisParams.active_rec act_param = (INCCAnalysisParams.active_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.Active);
                    if (act_param == null)
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 10198, "No Active method parameters found");
                        return;
                    }
                    INCCMethodResults.results_active_rec actres = (INCCMethodResults.results_active_rec)meas.INCCAnalysisResults.LookupMethodResults(
                                                              mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.Active, true);
                    /* calculate active doubles rate corrected for source yield factor */
                    //line 331 if calc_asy.cpp
                    Measurement.SourceYieldFactoredRates syfr = new Measurement.SourceYieldFactoredRates(results, meas);
                    // line 1267 of calc_asy.cpp
                    //Martyn says we need stuff here to deal with Cf active measurements HN 7.23.2015
                    actres.k0.v = syfr.source_yield_factor;
                    actres.k = new Tuple(syfr.total_corr_fact);
                    actres.k1 = new Tuple(meas.Norm.currNormalizationConstant);
                    // This stays the same for Cf. HN 7.23.2015
                    INCCAnalysisParams.CalCurveResult status = INCCAnalysis.CalculateCalibrationCurveOnly(act_param.cev,
                                    out actres.u235_mass, results.rates, syfr.corrected_doubles,
                                    RatesAdjustments.DeadtimeCorrected);

                    if (status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        string msg = String.Format("Active calibration curve failed mass limits of {0} and {1}", act_param.cev.lower_mass_limit, act_param.cev.upper_mass_limit);
                        meas.AddErrorMessage(msg, 10196, mkey);
                    }
                    else if (status != INCCAnalysisParams.CalCurveResult.Success)
                    {
                        meas.AddErrorMessage("Active calibration curve analysis error", 10197, mkey);
                    }
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 240, "Active results for U235 {0} +- {1}", actres.u235_mass.v, actres.u235_mass.err);
                    if (status == INCCAnalysisParams.CalCurveResult.Success || status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        actres.dcl_u235_mass = meas.AcquireState.mass;
                        INCCAnalysis.calc_decl_minus_assay_u235(actres.u235_mass, actres.dcl_u235_mass, ref actres.dcl_minus_asy_u235_mass, ref actres.dcl_minus_asy_u235_mass_pct, ref actres.pass, meas);
                        if (!actres.pass)
                        {
                            meas.AddWarningMessage("Active calibration curve: failed stratum rejection limits", 10198, mkey);
                        }
                        else if (!meas.Stratum.Unset)
                        {
                            meas.AddWarningMessage("Active calibration curve: passed stratum rejection limits", 10200, mkey);
                        }

                        if (actres.u235_mass.v > actres.methodParams.cev.upper_mass_limit)
                        {
                            meas.AddWarningMessage("Active calibration curve: upper U235 mass limit exceeded.", 10210, mkey);
                        }
                        if (actres.u235_mass.v < actres.methodParams.cev.lower_mass_limit)
                        {
                            meas.AddWarningMessage("Active calibration curve: lower U235 mass limit exceeded.", 10211, mkey);
                        }
                    }
                    // normal and backup retention not performed for active

                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.ActiveMultiplicity))
                {
                    // sets results class' mult v,err values at end of calculation                    
                    INCCMethodResults.results_active_mult_rec res = INCCAnalysis.CalculateActiveMultiplicity(mkey, results, meas, RatesAdjustments.DeadtimeCorrected);
                    if (res == null)
                    {
                        meas.AddErrorMessage("Active multiplicity analysis error", 10152, mkey);
                    }
                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.TruncatedMultiplicity))
                {
                    INCCMethodResults.results_truncated_mult_rec res = INCCAnalysis.CalculateTruncatedMult(mkey, results, meas, RatesAdjustments.DeadtimeCorrected);
                    // normal and backup retention
                    if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.TruncatedMultiplicity)
                    {
                        if (res.methodParams.known_eff)
                            normal_mass.CopyFrom(res.k.pu240e_mass);
                        else
                            normal_mass.CopyFrom(res.s.pu240e_mass);
                    }
                    if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.TruncatedMultiplicity)
                    {
                        if (res.methodParams.known_eff)
                            backup_mass.CopyFrom(res.k.pu240e_mass);
                        else
                            backup_mass.CopyFrom(res.s.pu240e_mass);
                    }
                }

                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.Collar))
                {
                    meas.AddWarningMessage("Collar mass results", 10153, mkey); // NEXT: Collar is incomplete, new design from IAEA is pending, this is a big task 
                    INCCAnalysis.CalculateCollar(mkey, results, meas, RatesAdjustments.DeadtimeCorrected);
                }

                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.ActivePassive))
                {
                    INCCAnalysisParams.active_passive_rec act_param = (INCCAnalysisParams.active_passive_rec)meas.INCCAnalysisState.Methods.GetMethodParameters(AnalysisMethod.ActivePassive);
                    if (act_param == null)
                    {
                        meas.Logger.TraceEvent(NCCReporter.LogLevels.Warning, 10198, "No active/passive method parameters found");
                        return;
                    }
                    INCCMethodResults.results_active_passive_rec actres = (INCCMethodResults.results_active_passive_rec)meas.INCCAnalysisResults.LookupMethodResults(
                                                              mkey, meas.INCCAnalysisState.Methods.selector, AnalysisMethod.ActivePassive, true);

                    INCCAnalysis.CalculateActivePassive(mkey, results, meas, RatesAdjustments.DeadtimeCorrected);

                    //line 1134 if calc_asy.cpp
                    // calculate delta doubles and error from passive and active doubles
                    Measurement.SourceYieldFactoredRates syfr = new Measurement.SourceYieldFactoredRates(results, meas);
                    actres.k0.v = syfr.source_yield_factor;
                    actres.k = new Tuple(syfr.total_corr_fact);
                    actres.k1 = new Tuple(meas.Norm.currNormalizationConstant);

                    actres.delta_doubles.v = syfr.corrected_doubles.v - results.DeadtimeCorrectedDoublesRate.v;
                    actres.delta_doubles.err = Math.Sqrt((syfr.corrected_doubles.v * results.DeadtimeCorrectedDoublesRate.err) + (syfr.corrected_doubles.v * results.DeadtimeCorrectedDoublesRate.err));

                    INCCAnalysisParams.CalCurveResult status = INCCAnalysis.CalculateCalibrationCurveOnly(act_param.cev,
                                                 out actres.u235_mass, results.rates, actres.delta_doubles,
                                                 RatesAdjustments.DeadtimeCorrected);

                    if (status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        string msg = String.Format("Active/passive calibration curve failed mass limits of {0} and {1}", act_param.cev.lower_mass_limit, act_param.cev.upper_mass_limit);
                        meas.AddErrorMessage(msg, 10196, mkey);
                    }
                    else if (status != INCCAnalysisParams.CalCurveResult.Success)
                    {
                        meas.AddErrorMessage("Active/passive calibration curve analysis error", 10197, mkey);
                    }
                    if (status == INCCAnalysisParams.CalCurveResult.Success || status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        actres.dcl_u235_mass = meas.AcquireState.mass;
                        INCCAnalysis.calc_decl_minus_assay_u235(actres.u235_mass, actres.dcl_u235_mass, ref actres.dcl_minus_asy_u235_mass, ref actres.dcl_minus_asy_u235_mass_pct, ref actres.pass, meas);
                        if (!actres.pass)
                        {
                            meas.AddWarningMessage("Active/passive: failed stratum rejection limits", 10198, mkey);
                        }
                        else if (!meas.Stratum.Unset)
                        {
                            meas.AddWarningMessage("Active/passive: passed stratum rejection limits", 10200, mkey);
                        }

                        if (actres.u235_mass.v > actres.methodParams.cev.upper_mass_limit)
                        {
                            meas.AddWarningMessage("Active/passive: upper U235 mass limit exceeded.", 10210, mkey);
                        }
                        if (actres.u235_mass.v < actres.methodParams.cev.lower_mass_limit)
                        {
                            meas.AddWarningMessage("Active/passive: lower U235 mass limit exceeded.", 10211, mkey);
                        }
                    }
                }


                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.AddASource))
                {
                    INCCAnalysisParams.CalCurveResult status;
                    INCCMethodResults.results_add_a_source_rec res = null;
                    // gotta do a sanity check for the AAS cycles, they may not be there due to unfinished processing in the overall code
                    if (meas.CFCycles != null)
                    {
                        res = INCCAnalysis.CalculateAddASource(mkey, results, meas, RatesAdjustments.DeadtimeCorrected, out status);
                    }
                    else
                    {
                        meas.AddErrorMessage("Add-a-source calibration curve cycles not present error", 10197, mkey);
                        status = INCCAnalysisParams.CalCurveResult.EpicFailLOL;
                    }
                    if (status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        string msg = String.Format("Add-a-source calibration curve failed mass limits of {0} and {1}", res.methodParams.cev.lower_mass_limit, res.methodParams.cev.upper_mass_limit);
                        meas.AddErrorMessage(msg, 10196, mkey);
                    }
                    else if (status != INCCAnalysisParams.CalCurveResult.Success)
                    {
                        meas.AddErrorMessage("Add-a-source calibration curve analysis error", 10197, mkey);
                    }
                    if (status == INCCAnalysisParams.CalCurveResult.Success || status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        res.dcl_pu_mass = meas.AcquireState.mass;
                        INCCAnalysis.calc_mass(res.pu240e_mass,
                                ref res.pu_mass, ref res.dcl_pu_mass, ref res.dcl_pu240e_mass,
                                ref res.dcl_minus_asy_pu_mass, ref res.dcl_minus_asy_pu_mass_pct, ref res.pass, meas);
                        if (!res.pass)
                        {
                            meas.AddWarningMessage("Add-a-source calibration curve: failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Add-a-source calibration curve: passed stratum rejection limits", 10200, mkey);
                        }

                        if (res.pu240e_mass.v > res.methodParams.cev.upper_mass_limit)
                        {
                            meas.AddWarningMessage("Add-a-source calibration curve: upper Pu240e mass limit exceeded.", 10210, mkey);
                        }
                        if (res.pu240e_mass.v < res.methodParams.cev.lower_mass_limit)
                        {
                            meas.AddWarningMessage("Add-a-source calibration curve: lower Pu240e mass limit exceeded.", 10211, mkey);
                        }
                        // normal and backup retention
                        if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.AddASource)
                        {
                            normal_mass.CopyFrom(res.pu240e_mass);
                        }
                        if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.AddASource)
                        {
                            backup_mass.CopyFrom(res.pu240e_mass);
                        }
                    }
                }
                if (meas.INCCAnalysisState.Methods.Has(AnalysisMethod.CuriumRatio))
                {
                    INCCAnalysisParams.CalCurveResult status;
                    INCCMethodResults.results_curium_ratio_rec res = INCCAnalysis.CalculateCuriumRatio(mkey, results, meas, RatesAdjustments.DeadtimeCorrected, out status);
                    if (status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        string msg = String.Format("Curium ratio calibration curve failed mass limits of {0} and {1}", res.methodParams.cev.lower_mass_limit, res.methodParams.cev.upper_mass_limit);
                        meas.AddErrorMessage(msg, 10196, mkey);
                    }
                    else if (status != INCCAnalysisParams.CalCurveResult.Success)
                    {
                        meas.AddErrorMessage("Curium ratio calibration curve analysis error", 10197, mkey);
                    }

                    if (status == INCCAnalysisParams.CalCurveResult.Success || status == INCCAnalysisParams.CalCurveResult.FailedOnMassLimit)
                    {
                        INCCAnalysisParams.cm_pu_ratio_rec cm_pu_ratio = NC.App.DB.Cm_Pu_RatioParameters.Get(); // load from DB, just like test params, 
                        // dev note: better not to ref DB here, because this is a one-off state retrieval and no other DB access occurs during mass calc processing, but that is how it works this morning

                        //calc  curium mass
                        INCCAnalysis.calc_curium_mass(res, cm_pu_ratio, meas);
                        res.u.dcl_mass = cm_pu_ratio.cm_dcl_u_mass;
                        res.u235.dcl_mass = cm_pu_ratio.cm_dcl_u235_mass;
                        if (!res.pu.pass)
                        {
                            meas.AddWarningMessage("Curium ratio: Pu failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Curium ratio: Pu passed stratum rejection limits", 10200, mkey);
                        }
                        if (!res.u.pass)
                        {
                            meas.AddWarningMessage("Curium ratio: U failed stratum rejection limits", 10198, mkey);
                        }
                        else
                        {
                            meas.AddWarningMessage("Curium ratio: U passed stratum rejection limits", 10200, mkey);
                        }
                        // normal and backup retention
                        if (meas.INCCAnalysisState.Methods.Normal == AnalysisMethod.CuriumRatio)
                        {
                            normal_mass.CopyFrom(res.pu.pu240e_mass);
                        }
                        if (meas.INCCAnalysisState.Methods.Backup == AnalysisMethod.CuriumRatio)
                        {
                            backup_mass.CopyFrom(res.pu.pu240e_mass);
                        }
                    }

                }

                // annotate the final results method marker on the INCC results instance.
                INCCMethodResults imr = null;
                if (normal_mass.v != -1.0)
                {
                    bool got = meas.INCCAnalysisResults.TryGetINCCResults(mkey, out imr);
                    if (got)
                        imr.primaryMethod = meas.INCCAnalysisState.Methods.Normal;
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10100, "Verification primary method {0} with mass {1} is from the normal method", imr.primaryMethod.ToString(), normal_mass.v);
                }
                else if (backup_mass.v != -1.0)
                {
                    bool got = meas.INCCAnalysisResults.TryGetINCCResults(mkey, out imr);
                    if (got)
                        imr.primaryMethod = meas.INCCAnalysisState.Methods.Backup;
                    meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10101, "Verification primary method {0} with mass {1} is from the backup method", imr.primaryMethod.ToString(), backup_mass.v);
                }
                if ((normal_mass.v != -1.0) && (backup_mass.v != -1.0))
                {
                    if (meas.INCCAnalysisState.Methods.Backup.IsNone())
                    {
                        double delta = Math.Abs(normal_mass.v - backup_mass.v);
                        double delta_error = Math.Sqrt(normal_mass.err * normal_mass.err +
                             backup_mass.err * backup_mass.err);

                        bool got = meas.INCCAnalysisResults.TryGetINCCResults(mkey, out imr);

                        if (delta <= (delta_error * meas.Tests.normalBackupAssayTestLimit))
                        {
                            if (got) imr.primaryMethod = meas.INCCAnalysisState.Methods.Normal;
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10102, "Verification primary method {0} with masses {1} and {2} is from the normal method", imr.primaryMethod.ToString(), normal_mass.v, backup_mass.v);
                        }
                        else
                        {
                            if (got) imr.primaryMethod = meas.INCCAnalysisState.Methods.Backup;
                            meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 10103, "Verification primary method {0} with masses {1} and {2} is from the backup method", imr.primaryMethod.ToString(), normal_mass.v, backup_mass.v);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                meas.Logger.TraceException(e);
            }

        }

        


        /// <summary>
        /// Do full outlier processing for all the Multiplicity results across all the cycles of the current Measurement
        /// </summary>
        public static void OutlierProcessing(this Measurement meas)
        {
            AssaySelector.MeasurementOption at = meas.MeasurementId.MeasOption;
            if (NC.App.Opstate.IsAbortRequested)
                return;
            if (!CycleProcessing.ConductStep(CycleProcessing.CycleStep.Outlier, at, meas.AcquireState.qc_tests))
                return;

            IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;

                meas.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 7002, "Final outlier conditioning {0}", mkey);

                INCCAnalysis.OutlierProcessing(mkey, meas);
            }
        }


        /// <summary>
        /// Calculate averages and sums for all cycles in a measurement that pass all tests
        /// Do processing for all the Multiplicity results across all the cycles of the current Measurement
        /// </summary>
        public static void CalcAvgAndSums(this Measurement meas)
        {
            RatesAdjustments dtchoice = RatesAdjustments.DeadtimeCorrected;

            if (NC.App.Opstate.IsAbortRequested)
                return;

            IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                meas.Logger.TraceEvent(NCCReporter.LogLevels.Info, 7003, "Calculating averages and sums for valid cycles {0} {1}", dtchoice, mkey);
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)meas.CountingAnalysisResults[mkey];
                mcr.ComputeHitSums();
				// APluralityOfMultiplicityAnalyzers: When a list mode sourced measurement is in use, and 
				// cycle data comes from a prior analysis via a DB cycle source or Verification reanalysis, 
				// doubles are computed using the LMSupport.SDTMultiplicityCalculator.
				// Note that DAQ LM sourced INCC5 analyses will have the unnormalized Acc distro. 
				// The method INCCAnalysis.CalcAveragesAndSums uses the unnormalized
				// Acc distro (mcr.UnAMult), but in the two cases above there is only the normalized Acc and RealsAcc distros are present.
				// So the *unnormalized distro* must be computed and placed on each cycle, then and only then are doubles calculated correctly.
                INCCAnalysis.CalcAveragesAndSums(mkey, mcr, meas, dtchoice, meas.Detector.Id.SRType);
                mcr.Scaler1Rate.v = mcr.Scaler1.v;
                mcr.Scaler1Rate.err = mcr.Scaler1.err;
                mcr.Scaler2Rate.v = mcr.Scaler2.v;
                mcr.Scaler2Rate.err = mcr.Scaler2.err;
                MeasOptionSelector mos = new MeasOptionSelector(meas.MeasOption, mkey);
                INCCResult results = meas.INCCAnalysisState.Lookup(mos);
                if (results != null)
                {
                    results.Scaler1Rate.v = mcr.Scaler1.v;
                    results.Scaler1Rate.err = mcr.Scaler1.err;
                    results.Scaler2Rate.v = mcr.Scaler2.v;
                    results.Scaler2Rate.err = mcr.Scaler2.err;
                }
            }
        }

        #endregion INCC calculation control methods

		/// <summary>
		/// Save all updated and new results in the database
		/// </summary>
		public static void SaveMeasurementResults(this Measurement meas)
		{
			if (meas.Detector.ListMode)
			{
				long mid = meas.MeasurementId.UniqueId;
				foreach(SpecificCountingAnalyzerParams s in meas.CountingAnalysisResults.Keys)
				{
					// this is a virtual LMSR so save each mkey
					Type t = s.GetType();
					DB.ElementList els = s.ToDBElementList();
					if (t.Equals(typeof(Multiplicity)))
                    {
                        Multiplicity thisone = ((Multiplicity)s);
						els.Add(new DB.Element("predelay", thisone.SR.predelay));
                    }
                    else if (t.Equals(typeof(Coincidence)))
                    {
                        Coincidence thisone = ((Coincidence)s);
						els.Add(new DB.Element("predelay", thisone.SR.predelay));
                    }
					DB.LMParamsRelatedBackToMeasurement counter = new DB.LMParamsRelatedBackToMeasurement(s.Table);
					s.Rank = counter.Create(mid, els);
					meas.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 34103, string.Format("Preserving {0}_m as {1}", s.Table, s.Rank));
				}
			}

            SaveMeasurementCycles(meas);

			IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator();
			while (iter.MoveNext())
			{
				Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                if (NC.App.Opstate.IsAbortRequested)
					return;

				INCCResult results;
				MeasOptionSelector moskey = new MeasOptionSelector(meas.MeasOption, mkey);
				bool found = meas.INCCAnalysisResults.TryGetValue(moskey, out results);  // APluralityOfMultiplicityAnalyzers: when does this actually get saved? It needs to be saved to DB after all calculations are complete

				try
				{
					switch (meas.MeasOption)
					{
                        case AssaySelector.MeasurementOption.background: // calculated in CalculateResults, update bkg and save the result to the DB
							// save the background result to the bkg_parms_rec table under the current SR key
							BackgroundParameters c = NC.App.DB.BackgroundParameters.Get(meas.Detector);
							if (c != null)
								c.Copy(meas.Background);  // copy changes back to original on user affirmation
							else
							{
								c = meas.Background;
								NC.App.DB.BackgroundParameters.GetMap().Add(meas.Detector, c);
							}
							NC.App.DB.BackgroundParameters.Set(meas.Detector, c);
							break;
                        case AssaySelector.MeasurementOption.initial: // calculated in CalculateResults, update norm and save the norm and init_src result to the DB
                            INCCResults.results_init_src_rec results_init_src = (INCCResults.results_init_src_rec)results;
                            // on fail, only the relevant results_init_src_rec is saved
                            // on pass, the normalization parameters are modified with the results_init_src_rec results, and so both are updated.
                            if (results_init_src.pass)
                            {
                                NormParameters np = NC.App.DB.NormParameters.Get(meas.Detector);
                                if (np != null)
                                    np.Copy(meas.Norm);  // copy any changes made to Norm as defined for the current detector
                                else
                                {
                                    np = meas.Norm;
                                    NC.App.DB.NormParameters.GetMap().Add(meas.Detector, np);
                                }
                                NC.App.DB.NormParameters.Set(meas.Detector, np);
                            }
                            SaveSpecificResultsForThisMeasurement(meas, results);
							break;
						case AssaySelector.MeasurementOption.normalization:  // calculated in CalculateResults, now save the result to the DB
                        case AssaySelector.MeasurementOption.precision:      // ditto
                            SaveSpecificResultsForThisMeasurement(meas, results);
							break;
						case AssaySelector.MeasurementOption.calibration: // ditto with the method analyses results, but they're a bit different
                        case AssaySelector.MeasurementOption.verification:
                            SaveMethodResultsForThisMeasurement(meas, moskey);
                            break;
                        case AssaySelector.MeasurementOption.holdup: // NEXT: Hold-up held up, implement it #35  
							break;
					}
				}
				catch (Exception e)
				{
					meas.Logger.TraceException(e);
				}

                SaveSummaryResultsForThisMeasurement(meas, moskey);

            }

            meas.PersistFileNames();

        }

        /// <summary>
        /// Preserve a measurement cycle list in a database
        /// Limited to INCC5 SR and INCC6 VSR values
        /// TODO: Need db table save for for LM-specific results Feynman, Rossi, Event, Coincidence) (Mult is working now)
        /// TODO: status shoud be set on db acquire lists, because it can read in e.g. 1500 but only 1450 are good, or cancel can cause only 20 to have been processed so the code should only use the processed cycles.
        /// </summary>
        /// <param name="m">The measurement containing the cycles to preserve</param>
        public static void SaveMeasurementCycles(this Measurement m)
        {
            long mid = m.MeasurementId.UniqueId;
            // Could we actually not do this when reanalyzing? hn 9.21.2015; No: the results are recalculated, so they must be stored and copied again 
			if (m.Detector.ListMode)
			{
				IEnumerator iter = m.CountingAnalysisResults.GetMultiplicityEnumerator();
				while (iter.MoveNext())
				{
					Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
					NC.App.DB.AddCycles(m.Cycles, mkey, mid, mkey.Rank);
				}
			}
			else
				NC.App.DB.AddCycles(m.Cycles, m.Detector.MultiplicityParams, mid);
            m.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 34105, string.Format("{0} cycles stored", m.Cycles.Count));
        }

        /// <summary>
        /// Save in the database the method results and copies of the current method parameters.
        /// The table entries are related back to the measurement.
        /// The method results are preserved in the results_\<method table name\> and \<method table name\>_m DB tables.
        /// </summary>
        /// <param name="m">The measurement to preserve</param>
        /// <param name="moskey">The option selector+multiplicity key for the method results map</param> 
        static void SaveMethodResultsForThisMeasurement(this Measurement m, MeasOptionSelector moskey)
        {
           // DB.Measurements ms = new DB.Measurements();
            long mid = m.MeasurementId.UniqueId;

            INCCMethodResults imrs;
            bool gotten = m.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
            if (gotten && imrs.Count > 0) // expected to be true for verification and calibration
            {
                //The distinct detector id and material type index the methods and the method results
                Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrs[m.INCCAnalysisState.Methods.selector];
                // now get an enumerator over the map of method results
                Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
                while (ai.MoveNext())
                {
                    INCCMethodResult imr = ai.Current.Value;
                    DB.ElementList els = imr.ToDBElementList(); // generates the Table property content too
                    DB.ParamsRelatedBackToMeasurement ar = new DB.ParamsRelatedBackToMeasurement(imr.Table);
                    long resid = ar.Create(mid, els);  // save the method results in the relevant results table
					do
					{
						long mresid = ar.CreateMethod(resid, mid, imr.methodParams.ToDBElementList()); // save the initial method params (the copy rides on the results)
						m.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 34104, string.Format("Method results {0} preserved ({1}, {2})", imr.Table, resid, mresid));
					} while (imr.methodParams.Pump > 0);
                }
            }                        
        }

        /// <summary>
        /// Save specific analysis results for Background, Initial Source, Normalization and Precision measurements.
        /// </summary>
        /// <param name="m">The measurement to preserve</param>
        /// <param name="res">The subclassed specific results instance</param> 
        static void SaveSpecificResultsForThisMeasurement(this Measurement m, INCCResult res)
        {
            long mid = m.MeasurementId.UniqueId;
            DB.ElementList els = res.ToDBElementList(); // generates the Table property content too
            DB.ParamsRelatedBackToMeasurement ar = new DB.ParamsRelatedBackToMeasurement(res.Table);
            long resid = ar.Create(mid, els);                          
            m.Logger.TraceEvent(NCCReporter.LogLevels.Verbose, 34103, String.Format("Results {0} preserved ({1})",resid, res.Table));
        }

        /// <summary>
        /// Save in the database the summary <see cref="INCCResults.results_rec"/> results for every type of INCC measurement.
        /// </summary>
        /// <param name="m">The measurement to preserve</param>
        /// <param name="moskey">The option selector+multiplicity key for the method results map</param> 
        static void SaveSummaryResultsForThisMeasurement(this Measurement m, MeasOptionSelector moskey)
        {
            long mid = m.MeasurementId.UniqueId;
            DB.Results dbres = new DB.Results();
            // save results with mid as foreign key
            bool b = dbres.Update(mid, m.INCCAnalysisResults.TradResultsRec.ToDBElementList()); // APluralityOfMultiplicityAnalyzers: results rec needs to be fully populated before here, or it needs to be saved again at the end of the processing
            m.Logger.TraceEvent(NCCReporter.LogLevels.Info, 34045, (b ? "Preserved " : "Failed to save ") + "summary results");
        }

		/// <summary>
        /// Check runtime status and if there is good measurement data, create a new measurement in the database
		/// </summary>
        /// <param name="m">The measurement to preserve</param>
        /// <param name="buffCount">count of data blocks processed so far, must be > 0 to force persistence</param>
        /// <param name="id">database id of persisted measurement, must be 0 to force transaction</param>
		public static void StartSavingMeasurement(this Measurement m, ulong buffCount, ref long id)
		{
			if (NC.App.Opstate.Continue && // stop and save OR no intervention
			buffCount > 0 && // there is data and state to save with this measuremnt
			id == 0 &&   // and the new measurement has not been preserved yet
			NC.App.Opstate.SOH == NCC.OperatingState.Living) // the operation was running so create the results data structures and create the basic measurement record in the DB
			{
				m.PrepareINCCResults();
				id = m.Persist();
			}
		}


        // for report recalculation
        public static void ReportRecalc(this Measurement m)
        {
            NC.App.Opstate.Measurement = m;
            CycleList cl = NC.App.DB.GetCycles(m.Detector, m.MeasurementId, m.AcquireState.data_src); // APluralityOfMultiplicityAnalyzers: // URGENT: get all the cycles associated with each analyzer, restoring into the correct key->result pair
            m.Cycles = new CycleList();
            m.CurrentRepetition = 0;
            foreach (Cycle cycle in cl)
            {
                m.CurrentRepetition++;
                m.Add(cycle);
                CycleProcessing.ApplyTheCycleConditioningSteps(cycle, m);
                m.CycleStatusTerminationCheck(cycle);
            }
           m.CalculateMeasurementResults();
        }
	}

}
