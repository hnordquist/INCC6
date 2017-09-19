/*
 * 186846
 * D Division
 * Defense Science Applications (DSA-3)
 * 
 * This software application and associated material was prepared by the 
 * Los Alamos National Security, LLC (LANS), under Contract DE-AC52-06NA25396
 * with the U.S. Department of Energy (DOE).
 * All rights in the software application and associated material are reserved by DOE
 * on behalf of the Government and LANS pursuant to the contract.
 * You are authorized to use the material for Government purposes but it is not to
 * be released or distributed to the public.
 * NEITHER THE UNITED STATES NOR THE UNITED STATES DEPARTMENT OF ENERGY, NOR
 * LOS ALAMOS NATIONAL SECURITY, LLC, NOR ANY OF THEIR EMPLOYEES, MAKES ANY WARRANTY,
 * EXPRESS OR IMPLIED, OR ASSUMES ANY LEGAL LIABILITY OR RESPONSIBILITY FOR THE
 * ACCURACY, COMPLETENESS, OR USEFULNESS OF ANY INFORMATION, APPARATUS, PRODUCT, OR
 * PROCESS DISCLOSED, OR REPRESENTS THAT ITS USE WOULD NOT INFRINGE PRIVATELY OWNED RIGHTS.
*/
using System;
using System.Collections.Generic;
using AnalysisDefs;
using DetectorDefs;
using NCCReporter;

namespace INCCCore
{
    public static partial class INCCAnalysis
    {
        /// <summary>
        /// From avg_sums.cpp of INCC
        /// </summary>
        /// <param name="mkey"></param>
        /// <param name="mcr"></param>
        /// <param name="meas"></param>
        /// <param name="choice"></param>
        static public void CalcAveragesAndSums(Multiplicity mkey, MultiplicityCountingRes mcr, Measurement meas, RatesAdjustments choice, InstrType SRType)
        {

            MeasOptionSelector mos = new MeasOptionSelector(meas.MeasOption, mkey);

            INCCResult results = meas.INCCAnalysisState.Lookup(mos);
            if (results == null)
            {
                meas.Logger.TraceEvent(LogLevels.Warning, 373737, "Skipping INCC-specific analysis, results null.");
                return;
            }

            //Used in summation/average
            double number_good_runs = 0.0;
            double singles_err = 0; double doubles_err = 0; double triples_err = 0;
            double scaler1_err = 0; double scaler2_err = 0;
            Rates ratessquared = new Rates();
            TimeSpan TS = new TimeSpan(0);
            MultiplicityCountingRes summmcr = new MultiplicityCountingRes();
            summmcr.Resize(mcr);
            choice = RatesAdjustments.DeadtimeCorrected;
            ratessquared.SetDT(choice);
            RatesAdjustments prev = mcr.rates.SetDT(choice);
            summmcr.rates.SetDT(choice);
            results.rates.SetDT(choice);
            results.SemiCorrectedRate.SetDT(choice);

            //Measured Rates Stuff
            double singles_meas = 0;
            double doubles_meas = 0;
            double singles_meas_err = 0;
            double doubles_meas_err = 0;
            double singles_wt = 0;
            double doubles_wt = 0;

            //Covariance stuff
            int MULTI_ARRAY_SIZE = (int)mcr.MaxBins;

            //DTC rates
            double singles_dtc = 0;
            double doubles_dtc = 0;
            double triples_dtc = 0;
            double singles_err_dtc = 0;
            double doubles_err_dtc = 0;

            meas.Background.rates.SetDT(choice);

            //set something in DTCRates as a default
            results.rates.DTCRates.SinglesRate = summmcr.rates.DTCRates.SinglesRate;
            results.rates.DTCRates.DoublesRate = summmcr.rates.DTCRates.DoublesRate;
            results.rates.DTCRates.TriplesRate = summmcr.rates.DTCRates.TriplesRate;

            try
            {
                IEnumerator<Cycle> iter = meas.Cycles.GetValidCycleListForThisKey(mkey);
                // Sum cycles -- send only what you need for unit testing
                SumCycles(meas.Cycles, mkey, ref summmcr, ref ratessquared, choice, ref number_good_runs, ref TS);

                //Consolidate averaging here for unit testing 
                CalculateAverageRates(ref summmcr, ref ratessquared, ref scaler1_err, ref scaler2_err, number_good_runs, TS);

                /* 
                 * 1 => "uncorrected_doubles" in the INCC code is not the raw number, nor normalized nor bkg corrected, but includes the DT correction(s)
                 */

                if ((meas.AcquireState.error_calc_method == ErrorCalculationTechnique.Sample) ||
                        SRType.isDG_AMSR_Match())                     
                    //(SRType == LMDAQ.InstrType.AMSR) || (SRType == LMDAQ.InstrType.DGSR))
                {

                    CalculateRatesErrorSample(summmcr, ratessquared, ref singles_err, ref doubles_err, ref triples_err, number_good_runs);

                    results.SetSemiCorrectedRateErr(choice, doubles_err);
                    results.rates.DTCRates.Singles.err = singles_err;
                    results.rates.DTCRates.Doubles.err = doubles_err;
                    results.rates.DTCRates.Triples.err = triples_err;
                    results.rates.DTCRates.Scaler1s.err = scaler1_err;
                    results.rates.DTCRates.Scaler2s.err = scaler2_err;

                    if ((meas.MeasOption != AssaySelector.MeasurementOption.background) &&
                          (meas.MeasOption != AssaySelector.MeasurementOption.precision))
                    {
                        results.rates.DTCRates.Singles.err = Math.Sqrt(singles_err * singles_err +
                            meas.Background.rates.DTCRates.Singles.err *
                            meas.Background.rates.DTCRates.Singles.err);
                        results.rates.DTCRates.Doubles.err = Math.Sqrt(doubles_err * doubles_err +
                            meas.Background.rates.DTCRates.Doubles.err *
                            meas.Background.rates.DTCRates.Doubles.err);
                        results.rates.DTCRates.Triples.err = Math.Sqrt(triples_err * triples_err +
                            meas.Background.rates.DTCRates.Triples.err *
                            meas.Background.rates.DTCRates.Triples.err);
                    }
                }

                //Default value
                results.rates.DTCRates.Zero();

                CalculateMeasuredRates(summmcr, ref singles_meas, ref doubles_meas, ref singles_wt, ref doubles_wt, 
                    ref singles_meas_err, ref doubles_meas_err, ref singles_err, ref doubles_err, ref singles_err_dtc,
                    ref doubles_err_dtc, meas.MeasOption, meas.Background.DeadtimeCorrectedSinglesRate.err, meas.Background.DytlewskiCorrectedDoublesRate.err,
                    meas.Norm.currNormalizationConstant, mkey.sr.predelay, mkey.sr.dieAwayTimeMS, mkey.sr.doublesGateFraction, 
                    mkey.sr.deadTimeCoefficientAinMicroSecs, mkey.sr.deadTimeCoefficientBinPicoSecs,TS);

                results.SetSemiCorrectedRateErr(choice, doubles_err);

                CalculateCovariance(summmcr, mcr, ref results.singles_multi, ref results.doubles_multi, ref results.triples_multi, triples_err, singles_meas,
                    mkey.sr.deadTimeCoefficientTinNanoSecs /*MULTIPLICITY_DEADTIME_CONV*/, mkey.sr.doublesGateFraction,meas.Norm.currNormalizationConstant.v,
                    ref results.covariance_matrix, meas.Background.rates.DTCRates, meas.MeasOption, MULTI_ARRAY_SIZE, TS);
                //Not sure of this.
                results.rates.DTCRates.TriplesRate = triples_dtc;

                if (meas.AcquireState.error_calc_method == ErrorCalculationTechnique.Theoretical ||
                    (meas.MeasOption == AssaySelector.MeasurementOption.normalization) || (meas.MeasOption == AssaySelector.MeasurementOption.initial))
                {
                    if (!SRType.isDG_AMSR_Match())// (SRType != LMDAQ.InstrType.AMSR) && (SRType != LMDAQ.InstrType.DGSR))
                    {
                        results.rates.DTCRates.Singles.err = singles_err;
                        results.rates.DTCRates.Doubles.err = doubles_err;
                        results.rates.DTCRates.Triples.err = triples_err;
                    }
                    VTuple scaler1 = new VTuple();
                    scaler1.v = results.rates.DTCRates.Scaler1Rate;
                    scaler1.err = 0;
                    VTuple scaler2 = new VTuple();
                    scaler2.v = results.rates.DTCRates.Scaler2Rate;
                    scaler2.err = 0;
                    CalculateErrorRatesTheoretical(number_good_runs, ref scaler1, ref scaler2);
                    results.rates.DTCRates.Scaler1s.err = scaler1.err;
                    results.rates.DTCRates.Scaler2s.err = scaler2.err;
                }

                //Deadtime correct new summed cycle
                if (number_good_runs > 0.0)
                {
                    /* perform corrections on sum of runs as if one run */

                    summmcr.TransferIntermediates(mcr); // make sure pre-computed alpha beta values are transferred to the results 
                    summmcr.TS = TimeSpan.FromTicks(TS.Ticks);  // copy the total time for the operation onto the summed mcr

                    if (CycleProcessing.ConductStep(CycleProcessing.CycleStep.Deadtime, meas.MeasOption))
                    {
                        CycleProcessing.DeadtimeCorrection(meas, mkey, summmcr, did: meas.Detectors[0].Id, isCycle: false);
                    }

                    results.SetSemiCorrectedRate(choice, summmcr.rates.DTCRates.Doubles.v);
                    // here the summary is bkg corrected and normed, just like a cycle
                    if (CycleProcessing.ConductStep(CycleProcessing.CycleStep.Background, meas.MeasOption))
                        CycleProcessing.BackgroundCorrection(mkey, summmcr, meas.Background);

                    if (CycleProcessing.ConductStep(CycleProcessing.CycleStep.Normalization, meas.MeasOption))
                        CycleProcessing.Normalization(mkey, summmcr, meas.Norm);
                }

                /* dev note: use these values on the measurement cycle list to get various summary numbers when needed
                * results.total_number_runs = meas.Cycles.GetTotalCycleCountForThisKey(mkey);
                * results.number_good_runs = meas.Cycles.GetValidCycleCountForThisKey(mkey);
                * results.total_good_count_time = TS.TotalSeconds;
                * */
                results.rates.RawRates.SinglesRate = summmcr.rates.RawRates.SinglesRate;
                results.rates.RawRates.DoublesRate = summmcr.rates.RawRates.DoublesRate;
                results.rates.RawRates.TriplesRate = summmcr.rates.DeadtimeCorrectedRates.TriplesRate;
                results.Totals = summmcr.Totals;
                results.S1Sum = summmcr.S1Sum;
                results.S2Sum = summmcr.S2Sum;
                results.RASum = summmcr.RASum;
                results.ASum = summmcr.ASum;
                results.Resize(summmcr);
                results.TransferIntermediates(summmcr);

                for (ushort k = 0; k < summmcr.RAMult.Length; k++)
                {
                    results.RAMult[k] = summmcr.RAMult[k];
                }
                for (ushort k = 0; k < summmcr.NormedAMult.Length; k++)
                {
                    results.NormedAMult[k] = summmcr.NormedAMult[k];
                }
                for (ushort k = 0; k < summmcr.UnAMult.Length; k++)
                {
                    results.UnAMult[k] = summmcr.UnAMult[k];
                }

                // INCC does not use these corrected rates here, it uses the precursor locally computed rates

                results.rates.DTCRates.SinglesRate = summmcr.rates.DTCRates.SinglesRate;
                results.rates.DTCRates.DoublesRate = summmcr.rates.DTCRates.DoublesRate;
                results.rates.DTCRates.TriplesRate = summmcr.rates.DTCRates.TriplesRate;

            }
            catch (Exception e)
            {
                meas.Logger.TraceException(e, true);
            }
            finally  // dev note: need to set the choice back so state from caller is unchanged
            {
                mcr.rates.SetDT(prev);
                results.SemiCorrectedRate.SetDT(prev);
                meas.Background.rates.SetDT(prev);
            }
        }

        public static void SumCycles(CycleList list, Multiplicity mkey, 
            ref MultiplicityCountingRes summmcr, ref Rates ratessquared, 
            RatesAdjustments choice, ref double number_good_runs, ref TimeSpan TS)
        {
            IEnumerator<Cycle> iter = list.GetEnumerator();
            while (iter.MoveNext())
            {
                Cycle run = iter.Current;
                MultiplicityCountingRes mr = (MultiplicityCountingRes)run.CountingAnalysisResults[mkey];

                number_good_runs++;
                TS += run.TS;
                summmcr.Totals += run.Totals;
                summmcr.RASum += mr.RASum;
                summmcr.ASum += mr.ASum;
                summmcr.UnASum += mr.UnASum;
                summmcr.Scaler1.v += mr.Scaler1.v; summmcr.S1Sum = summmcr.Scaler1.v;
                summmcr.Scaler2.v += mr.Scaler2.v; summmcr.S2Sum = summmcr.Scaler2.v;
                summmcr.BinSumLoop(mr);
                summmcr.rates.DTCRates.Add(mr.rates[choice]);
                ratessquared.DTCRates.AddSquare(mr.rates[choice]);
                summmcr.rates.RawRates.Add(mr.rates.RawRates);

            }
        }
        public static void CalculateAverageRates (ref MultiplicityCountingRes summmcr, ref Rates ratessquared, 
            ref double scaler1_err, ref double scaler2_err, double number_good_runs, TimeSpan TS)
        {
            if (number_good_runs > 1.0 && TS.TotalSeconds != 0.0)
            {
                summmcr.rates.RawRates.Div(number_good_runs);
                summmcr.rates.DTCRates.Div(number_good_runs);
                ratessquared.DTCRates.Div(number_good_runs);
                summmcr.Scaler1.v /= number_good_runs;
                summmcr.Scaler2.v /= number_good_runs;
                // todo: add quads? HN 4/25/17
                scaler1_err = Math.Sqrt(Math.Abs(
                    ratessquared.DTCRates.Scaler1s.v - (summmcr.Scaler1.v * summmcr.Scaler1.v)) / (number_good_runs - 1.0));
                scaler2_err = Math.Sqrt(Math.Abs(
                    ratessquared.DTCRates.Scaler2s.v - (summmcr.Scaler2.v * summmcr.Scaler2.v)) / (number_good_runs - 1.0));
            }
            else
                return;
        }

        public static void CalculateRatesErrorSample(MultiplicityCountingRes summmcr, Rates ratessquared,
                    ref double singles_err, ref double doubles_err, ref double triples_err, double number_good_runs)
        {
            if (number_good_runs > 1.0)
            {
                singles_err = Math.Sqrt(Math.Abs(ratessquared.DTCRates.SinglesRate - (summmcr.rates.DTCRates.SinglesRate * summmcr.rates.DTCRates.SinglesRate)) /
                (number_good_runs - 1.0));
                doubles_err = Math.Sqrt(Math.Abs(ratessquared.DTCRates.DoublesRate - (summmcr.rates.DTCRates.DoublesRate * summmcr.rates.DTCRates.DoublesRate)) /
                    (number_good_runs - 1.0));
                triples_err = Math.Sqrt(Math.Abs(ratessquared.DTCRates.TriplesRate - (summmcr.rates.DTCRates.TriplesRate * summmcr.rates.DTCRates.TriplesRate)) /
                    (number_good_runs - 1.0));
            }
        }

        public static void CalculateCovariance(MultiplicityCountingRes summmcr, MultiplicityCountingRes mcr,
            ref double singles_dtc, ref double doubles_dtc, ref double triples_dtc, double triples_err, double singles_meas,
            double deadtime, double doubles_gate_fraction, double norm_constant, ref double[]covariance_matrix, 
            StdRates BackgroundRates, AssaySelector.MeasurementOption mo, int size, TimeSpan TS)
        {
            double sum_p = 0;
            double sum_q = 0;
            double sum_alpha_q = 0;
            double sum_alpha_p_minus_q = 0;
            double sum_beta_p_minus_q = 0;
            double p_i = 0;
            double p_j = 0;
            double q_i = 0;
            double q_j = 0;
            double[] x = new double[3 * size];
            double[] xt = new double[size * 3];
            double[] y = new double[3 * size];
            double[] yt = new double[size * 3];
            double[] temp_x = new double[3 * size];
            double[] temp_y = new double[3 * size];
            double[] cov_p_ptr = new double[size];
            double[] cov_q_ptr = new double[size];
            double[] covariance_x = new double[3 * 3];
            double[] covariance_y = new double[3 * 3];
            double temp = 0;
            double[] norm = new double[3 * 3];

            if ((cov_p_ptr == null) || (cov_q_ptr == null))
            {
                triples_err = 0.0;
            }
            else
            {
                sum_p = 0.0;
                sum_q = 0.0;
                sum_alpha_q = 0.0;
                sum_alpha_p_minus_q = 0.0;
                sum_beta_p_minus_q = 0.0;
                for (ushort k = 0; k < size; k++)
                {
                    sum_p += summmcr.RAMult[k];
                    sum_q += summmcr.NormedAMult[k];
                    sum_alpha_q += mcr.alpha[k] *
                        summmcr.NormedAMult[k];
                    sum_alpha_p_minus_q += mcr.alpha[k] *
                                            ((double)summmcr.RAMult[k] - (double)summmcr.NormedAMult[k]);
                    sum_beta_p_minus_q += mcr.beta[k] *
                        ((double)summmcr.RAMult[k] - (double)summmcr.NormedAMult[k]);
                }
                for (int i = 0; i < size; i++)
                {
                    if (sum_p != 0.0)
                        p_i = summmcr.RAMult[i] / sum_p;
                    else
                        p_i = 0.0;
                    if (sum_q != 0.0)
                        q_i = summmcr.NormedAMult[i] / sum_q;
                    else
                        q_i = 0.0;
                    for (int j = 0; j < size; j++)
                    {
                        if (sum_p != 0.0)
                            p_j = summmcr.RAMult[j] / sum_p;
                        else
                            p_j = 0.0;
                        if (sum_q != 0.0)
                            q_j = summmcr.NormedAMult[j] / sum_q;
                        else
                            q_j = 0.0;
                        if (i == j)
                        {
                            cov_p_ptr[i * size + j] =
                                sum_p * p_i * (1.0 - p_i);
                            cov_q_ptr[i * size + j] =
                                sum_q * q_i * (1.0 - q_i);
                        }
                        else
                        {
                            cov_p_ptr[i * size + j] = -sum_p * p_i * p_j;
                            cov_q_ptr[i * size + j] = -sum_q * q_i * q_j;
                        }
                    }
                }
                temp = Math.Exp(deadtime * 1e-9 * singles_meas) / TS.TotalSeconds;
                singles_dtc = temp * sum_q;
                doubles_dtc = temp * sum_alpha_p_minus_q;
                if (singles_dtc != 0.0)
                    triples_dtc = temp * (sum_beta_p_minus_q - sum_alpha_q *
                    (doubles_dtc / singles_dtc));
                else
                    triples_dtc = 0.0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (i == 0)
                        {
                            x[i * size + j] = temp;
                            y[i * size + j] = 0.0;
                        }
                        else if (i == 1)
                        {
                            x[i * size + j] = temp *
                                mcr.alpha[j];
                            y[i * size + j] = -temp *
                                mcr.alpha[j];
                        }
                        else if (i == 2)
                        {
                            if (singles_dtc != 0.0)
                            {
                                x[i * size + j] = temp *
                                    (mcr.beta[j] -
                                    (sum_alpha_q / singles_dtc) * temp *
                                    (mcr.alpha[j] -
                                    doubles_dtc / singles_dtc));
                                y[i * size + j] = temp *
                                    (-mcr.beta[j] +
                                    (mcr.alpha[j] / singles_dtc) *
                                    (temp * sum_alpha_q - doubles_dtc));
                            }
                            else
                            {
                                x[i * size + j] = 0.0;
                                y[i * size + j] = 0.0;
                            }
                        }
                        xt[j * 3 + i] = x[i * size + j];
                        yt[j * 3 + i] = y[i * size + j];
                    }
                }
                INCCAnalysis.matrix_multiply(3, size, x, size,
                 size, cov_p_ptr, 3, size, temp_x);
                INCCAnalysis.matrix_multiply(3, size, temp_x, size,
                    3, xt, 3, 3, covariance_x);
                INCCAnalysis.matrix_multiply(3, size, y, size,
                    size, cov_q_ptr, 3, size, temp_y);
                INCCAnalysis.matrix_multiply(3, size, temp_y, size,
                    3, yt, 3, 3, covariance_y);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        covariance_matrix[i * 3 + j] = covariance_x[i * 3 + j] +
                            covariance_y[i * 3 + j];
                    }
                }
                covariance_matrix[0] = (singles_dtc + 2.0 * doubles_dtc /
                  doubles_gate_fraction) / TS.TotalSeconds;
                if ((mo == AssaySelector.MeasurementOption.background) ||
                   (mo == AssaySelector.MeasurementOption.precision) ||
                   (mo == AssaySelector.MeasurementOption.normalization) ||
                   (mo == AssaySelector.MeasurementOption.initial))
                {
                    triples_err = Math.Sqrt(covariance_matrix[8]);
                }
                /* calculate matrix to correct for normalization, background */

                temp = CycleProcessing.deadtime_C_correction(deadtime, singles_meas, traditional: false);
                temp = (1.0 + deadtime
                    * 1e-32 /*NEW_COEFF_C_DEADTIME_CONV*/  /* coeff C conv from db deadtime corr */
                    * Math.Pow(singles_meas, /* COEFF_C_DEADTIME_POWER*/ 5.0));

                norm[0] = norm_constant;
                norm[1] = Math.Pow(norm_constant, 1.5);
                norm[2] = norm_constant *
                    norm_constant * temp;
                norm[3] = norm[1];
                norm[4] = norm_constant *
                    norm_constant;
                norm[5] = Math.Pow(norm_constant, 2.5) * temp;
                norm[6] = norm[2];
                norm[7] = norm[5];
                norm[8] = norm_constant *
                    norm_constant *
                    norm_constant * temp * temp;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        covariance_matrix[i * 3 + j] *= norm[i * 3 + j];
                    }
                }
                singles_dtc = Math.Sqrt(norm_constant) *
                    (singles_dtc - BackgroundRates.Singles.v);
                doubles_dtc = norm_constant *
                    (doubles_dtc - BackgroundRates.Doubles.v);
                //This had singles where doubles should be
                if ((mo == AssaySelector.MeasurementOption.background) ||
                   (mo == AssaySelector.MeasurementOption.precision) ||
                   (mo == AssaySelector.MeasurementOption.normalization) ||
                   (mo == AssaySelector.MeasurementOption.initial))
                {
                    triples_dtc = temp * triples_dtc;
                }
                else
                {
                    triples_dtc = Math.Pow(norm_constant, 1.5) *
                        (temp * triples_dtc - BackgroundRates.Triples.v);
                }
                summmcr.rates.DTCRates.TriplesRate = triples_dtc;

                if ((mo != AssaySelector.MeasurementOption.background) &&
                   (mo != AssaySelector.MeasurementOption.precision) &&
                   (mo != AssaySelector.MeasurementOption.normalization) &&
                   (mo != AssaySelector.MeasurementOption.initial))
                {
                    triples_err = Math.Sqrt(covariance_matrix[8]);
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        covariance_matrix[i * 3 + j] = covariance_matrix[i * 3 + j]; // this is saved on the results for later use in the multiplicity calculation
                    }
                }
            }
        }
        public static void CalculateMeasuredRates(MultiplicityCountingRes summmcr, ref double singles_meas, ref double doubles_meas, 
            ref double singles_wt, ref double doubles_wt, ref double singles_meas_err, ref double doubles_meas_err,
            ref double singles_err, ref double doubles_err,ref double singles_err_dtc, ref double doubles_err_dtc,
            AssaySelector.MeasurementOption mo, double backgroundDTCsingleserr, double backgroundDTCdoubleserr, VTuple normconst,
            double predelay, double dieaway, double gatelength, double coeffa, double coeffb,
            TimeSpan TS)
        {
            if (TS.TotalSeconds != 0.0)
            {
                double f, g, delta, delta2;

                singles_meas = summmcr.Totals / TS.TotalSeconds;
                doubles_meas = (summmcr.RASum - summmcr.ASum) / TS.TotalSeconds;

                // math: units mismatch -- WTH?
                f = Math.Exp((-(double)predelay) / dieaway) *
                (1.0 - Math.Exp((-(double)gatelength) / dieaway));
                g = 1.0 - (1.0 - Math.Exp((-(double)gatelength) / dieaway)) /
                    (gatelength / dieaway);
                delta = coeffa * 1e-6 /*COEFF_A_DEADTIME_CONV*/ +
                    coeffb * 1e-12 /*COEFF_B_DEADTIME_CONV*/ * singles_meas;
                delta2 = coeffa * 1e-6 /*COEFF_A_DEADTIME_CONV*/  +
                        2.0 * coeffb * 1e-12 /*COEFF_B_DEADTIME_CONV*/ * singles_meas;
                if ((singles_meas != 0.0) && (f != 0.0) && (doubles_meas >= 0.0))
                {
                    singles_wt = Math.Sqrt(1.0 + (2.0 * doubles_meas) / (f * singles_meas));
                    doubles_wt = Math.Sqrt(1.0 + (8.0 * g * doubles_meas) / (f * singles_meas));
                }
                else
                {
                    singles_wt = 1.0;
                    doubles_wt = 1.0;
                }
                singles_meas_err = singles_wt * Math.Sqrt(summmcr.Totals) / TS.TotalSeconds;
                doubles_meas_err = doubles_wt *
                    Math.Sqrt(summmcr.RASum + summmcr.ASum) / TS.TotalSeconds;

                if (singles_meas != 0.0)
                {
                    singles_err = singles_meas * Math.Exp((delta / 4.0) * singles_meas) *
                        ((delta2 / 4.0) + (1.0 / singles_meas)) * singles_meas_err;
                }
                else
                    singles_err = 0.0;
                singles_err_dtc = singles_err;

                if ((mo == AssaySelector.MeasurementOption.normalization) ||
                    (mo == AssaySelector.MeasurementOption.initial))
                {
                    singles_err = Math.Sqrt(singles_err * singles_err +
                        backgroundDTCsingleserr *
                        backgroundDTCsingleserr);
                }

                if ((mo != AssaySelector.MeasurementOption.background) &&
                    (mo != AssaySelector.MeasurementOption.precision) &&
                    (mo != AssaySelector.MeasurementOption.normalization) &&
                    (mo != AssaySelector.MeasurementOption.initial))
                {
                    singles_err = normconst.v *
                        (singles_err_dtc * singles_err_dtc +
                         backgroundDTCsingleserr *
                         backgroundDTCsingleserr);
                    singles_err += (summmcr.rates.DTCRates.SinglesRate / (2.0 * normconst.v) *
                    normconst.err) *
                    (summmcr.rates.DTCRates.SinglesRate / (2.0 * normconst.v) *
                    normconst.err);
                    singles_err = Math.Sqrt(singles_err);
                }

                if (doubles_meas != 0.0)
                {
                    doubles_err = Math.Abs(doubles_meas * Math.Exp(delta * singles_meas)) *
                      Math.Sqrt((doubles_meas_err * doubles_meas_err) /
                        (doubles_meas * doubles_meas) +
                        delta2 * delta2 * singles_meas_err * singles_meas_err);
                }
                else
                    doubles_err = 0.0;
                doubles_err_dtc = doubles_err;

                if ((mo == AssaySelector.MeasurementOption.normalization) ||
                    (mo == AssaySelector.MeasurementOption.initial))
                {
                    doubles_err = Math.Sqrt(doubles_err * doubles_err +
                        backgroundDTCdoubleserr *
                        backgroundDTCdoubleserr);
                }

                if ((mo != AssaySelector.MeasurementOption.background) &&
                    (mo != AssaySelector.MeasurementOption.precision) &&
                    (mo != AssaySelector.MeasurementOption.normalization) &&
                    (mo != AssaySelector.MeasurementOption.initial))
                {
                    doubles_err = normconst.v *
                       normconst.v *
                        (doubles_err_dtc * doubles_err_dtc +
                       backgroundDTCdoubleserr *
                        backgroundDTCdoubleserr);
                    doubles_err += ((summmcr.rates.DTCRates.DoublesRate / normconst.v) *
                    normconst.err) *
                    ((summmcr.rates.DTCRates.DoublesRate / normconst.v) *
                    normconst.err);
                    doubles_err = Math.Sqrt(doubles_err);
                }
            }
        }
        public static void CalculateErrorRatesTheoretical(double number_good_runs, ref VTuple scaler1, ref VTuple scaler2)
        {
                // Add quads? What is this ratio???
                // results.resultsquadstriplesratioerror =
                //    Math.Sqrt (quads_triples_ratio / number_good_runs);
                if (number_good_runs > 0.0)
                {
                    if (scaler1.v > 0.0)
                        scaler1.err = Math.Sqrt(scaler1.v / number_good_runs);
                    else
                        scaler1.err = 0.0;
                    if (scaler2.v > 0.0)
                        scaler2.err = Math.Sqrt(scaler2.v / number_good_runs);
                    else
                        scaler2.err = 0.0;
                }
         }
    }
}
