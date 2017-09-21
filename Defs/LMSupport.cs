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
using System.Collections.Generic;
using AnalysisDefs;
using BigNum;
using NCCReporter;

namespace LMRawAnalysis
{
    static public class RawAnalysisProperties
    {
        /// <summary>
        /// maxEventsPerBlock is a performance parameter for maximum speed particularly when reading from database.
        /// The value should be large enough that neutrons are passed from database to AnalyzerHandler in chunks,
        /// balancing the time required to read from disk with processing speed by threaded analyses.
        /// This parameter optimization will vary from computer to computer, so would make sense for this
        /// parameter to be a configuration-file parameter.
        /// </summary>
        static public int maxEventsPerBlock = 100000;

        /// <summary>
        /// ChannelCount is the number of neutron-detector bits. The value 32 matches both certain detector hardware
        /// and the number of bits in a UInt32 used to report neutrons, e.g. from NCD files.
        /// </summary>
        public const Int32 ChannelCount = 32; // forever, or until we integrate the n-channel code 

        /// <summary>
        /// circularListBlockIncrement is a moderate-size heuristic. Making the value too large makes the
        /// neutron-event queues too large, and making the event too small makes the analyzers increase the
        /// size of the queue more often. Other than that, this value shouldn't matter much.
        /// </summary>
        static public int circularListBlockIncrement = 43000;  //used for block size for various circular linked lists

        /// <summary>
        /// numRAGatesPerWindow is the number of Rossi-Alpha gates assessed after each neutron trigger.
        /// The value 1000 was hardcoded in legacy analyses.
        /// </summary>
        static public int numRAGatesPerWindow = 1000; //number of Rossi-Alpha gates in a Rossi-Alpha window

        /// <summary>
        /// maxNeutronsPerMultiplicityGate is the largest number of neutrons that can be counted in any single
        /// multiplicity time gate. The numbers of neutrons in gates are used in the factorial calculations,
        /// so there is a practical upper bound of 1029 for which the factorial calculations overflow the
        /// largest value that can be stored inside an IEEE double. Using 1000 instead of 1029 allows
        /// considerable headroom for a multiplicative coefficient. As in the legacy code, the last value
        /// of the vector of the neutron histrogram is used to count any gates that exceede this max value.
        /// The legacy value for this max at least in some codes was 128.
        /// </summary>
        static public int maxNeutronsPerMultiplicityGate = 1000;

        /// <summary>
        /// maxEventSpacing is a fairly-large heuristic for the largest duration measured in gates by an EventSpacingAnalyzer.
        /// </summary>
        static public int maxEventSpacing = 42000;  //upper bound to counting of gates by EventSpacingAnalysis

        /// <summary>
        /// rateGatesPerAccumulator is a moderate-size heuristic for the number of gates recorded by a single RateAccumulator.
        /// When the RateAccumulator overflows, the code allocates a new RateAccumulator in a linked list
        /// of these blocks.
        /// </summary>
        static public UInt32 rateGatesPerAccumulator = 4096;
    }

     public class SDTMultiplicityCalculator
    {
        protected double ticSizeInSeconds;

		public LMLoggers.LognLM Log {get; set; }

        public SDTMultiplicityCalculator(double theTicSizeInSeconds)
        {
			ticSizeInSeconds = theTicSizeInSeconds;
        }

        /// <summary>
        /// GetSDTMultiplicityResult() calculates Singles, Doubles, and Triples rates plus related metadata.
        /// 
        /// This utility method works for both Slow- and Fast-Background Multiplicity Analyzers.
        /// 
        /// Note that the input parameter "accidentalsHistogram" need not be normalized,
        /// and that a normalized-accidentals distribution will be included in output and will be valid
        /// for both slow and fast accidentals.
        /// 
        /// </summary>
        /// <param name="realsPlusAccidentalsHistogram"> The histogram of gate populations. </param>
        /// <param name="accidentalsHistogram"> The histogram of gate populations - NOT NORMALIZED. </param>
        /// <param name="wasFastAccidentals"> true or false. Affects how PTsingles are calculated, etc. </param>
        /// <param name="multiplicityGateWidth"> as a UInt64, in 100-nanosecond tics. </param>
        /// <param name="multiplicityDeadDelay"> as a UInt64, in 100-nanosecond tics. </param>
        /// <param name="accidentalsDelay"> as a UInt64, in 100-nanosecond tics. </param>
        /// <param name="deadTimeCoeffTinNanoSecs"> as a double, in nanoseconds. </param>
        /// <param name="deadTimeCoeffAinMicroSecs"> as a double, in microseconds. </param>
        /// <param name="deadTimeCoeffBinPicoSecs"> as a double, in picoseconds. </param>
        /// <param name="deadTimeCoeffCinNanoSecs"> as a double, in nanoseconds. </param>
        /// <param name="totalMeasurementTime"> as a double, in seconds. </param>
        /// <param name="normedAccidentalsHistogram">  UInt64[]. </param>
        /// <returns></returns>
        public MultiplicityResult GetSDTMultiplicityResult(UInt64[] realsPlusAccidentalsHistogram,
                                                           UInt64[] accidentalsHistogram,
                                                           Boolean wasFastAccidentals,
                                                           UInt64 multiplicityGateWidth,
                                                           UInt64 multiplicityDeadDelay,
                                                           UInt64 accidentalsDelay,
                                                           double deadTimeCoeffTinNanoSecs,
                                                           double deadTimeCoeffAinMicroSecs,
                                                           double deadTimeCoeffBinPicoSecs,
                                                           double deadTimeCoeffCinNanoSecs,
                                                           double totalMeasurementTime,
                                                           UInt64[] normedAccidentalsHistogram = null)
        {
            MultiplicityResult result;
            double phi;
            double gateInSeconds;
            UInt32 biggestKey, biggestAKey;
            int arrayLength;
            double[] alpha;
            double[] beta;
            //todo: check this out. Makes it slow and is dumb HN -- yes, I said it
            BigFloat[] α = new BigFloat[0], β = new BigFloat[0];

            result = new MultiplicityResult();
            if (wasFastAccidentals == true)
            {
                result.isSlowBackground = false;
            }
            else
            {
                result.isSlowBackground = true;
            }

            //store parameters
            result.multiplicityGateWidth = multiplicityGateWidth;
            result.multiplicityDeadDelay = multiplicityDeadDelay;
            result.accidentalsDelay = accidentalsDelay;
            result.deadTimeCoefficientTinNanoSecs = deadTimeCoeffTinNanoSecs;
            result.deadTimeCoefficientAinMicroSecs = deadTimeCoeffAinMicroSecs;
            result.deadTimeCoefficientBinPicoSecs = deadTimeCoeffBinPicoSecs;
            result.deadTimeCoefficientCinNanoSecs = deadTimeCoeffCinNanoSecs;

            //copy the real-plus-accidental multiplicity histogram
            biggestKey = 0;
            arrayLength = realsPlusAccidentalsHistogram.Length;
            for (int i = 0; i < arrayLength; i++)
            {
                if (realsPlusAccidentalsHistogram[i] > 0)
                {
                    result.realPlusAccidentalDistribution.Add((UInt64)i, realsPlusAccidentalsHistogram[i]);
                    biggestKey = (UInt32)i;
                }
            }
            result.maxRABin = biggestKey;

            //copy the accidental-only histogram
            biggestAKey = 0;
            arrayLength = accidentalsHistogram.Length;
            for (int i = 0; i < arrayLength; i++)
            {
                if (accidentalsHistogram[i] > 0)
                {
                    result.accidentalDistribution.Add((UInt32)i, accidentalsHistogram[i]);
                    biggestAKey = (UInt32)i;
                }
            }
            result.maxABin = biggestAKey;

            //************************************************************
            //Normalize the AccidentalDistribution, 
            //scaling the FastBackgroundAnalysis result in proportion 
            //to the number of Real+Accidental gate
            UInt64 numAccidentalGates = 0;
            UInt64 numRealPlusAccidentalGates = 0;
            foreach (KeyValuePair<UInt64, UInt64> pair in result.realPlusAccidentalDistribution)
            {
                numRealPlusAccidentalGates += pair.Value;
            }

            // if normed distro presented, compute the normalization param and recompute the unnormalized array from it (JFL but does not seem to work)
            if (normedAccidentalsHistogram != null)
            {
                for (int i = 0; i < normedAccidentalsHistogram.Length; i++)
                {
                    if (normedAccidentalsHistogram[i] > 0)
                    {
                        result.normalizedAccidentalDistribution.Add((UInt32)i, normedAccidentalsHistogram[i]);
                    }
                }

                UInt64 numNAccidentalGates = 0;

                foreach (ulong no in normedAccidentalsHistogram)
                {
                    numNAccidentalGates += no;
                }

                double denormalizingRatio;
                UInt64 denormalizedRate;
                denormalizingRatio = ((double)numNAccidentalGates) / ((double)numRealPlusAccidentalGates);

                result.accidentalDistribution.Clear();
                foreach (KeyValuePair<UInt64, UInt64> pair in result.normalizedAccidentalDistribution)
                {
                    denormalizedRate = (UInt64)(pair.Value * denormalizingRatio);
                    result.accidentalDistribution.Add(pair.Key, denormalizedRate);
                    biggestAKey = (UInt32)pair.Key;
                }
                result.maxABin = biggestAKey;

                foreach (KeyValuePair<UInt64, UInt64> pair in result.accidentalDistribution)
                {
                    numAccidentalGates += pair.Value;
                }

            }
            else
            {
                foreach (KeyValuePair<UInt64, UInt64> pair in result.accidentalDistribution)
                {
                    numAccidentalGates += pair.Value;
                }


                double normalizingRatio;
                UInt64 normalizedRate;
                normalizingRatio = ((double)numRealPlusAccidentalGates) / ((double)numAccidentalGates);

                result.normalizedAccidentalDistribution.Clear();
                foreach (KeyValuePair<UInt64, UInt64> pair in result.accidentalDistribution)
                {
                    normalizedRate = (UInt64)(pair.Value * normalizingRatio);
                    result.normalizedAccidentalDistribution.Add(pair.Key, normalizedRate);
                }
            }


            //*** END Normalizing the AccidentalDistribution *************


            //store the bigger key...
            if (biggestAKey > biggestKey)
            {
                biggestKey = biggestAKey;
            }
            if (biggestKey < 2)
            {
                biggestKey = 2;  //...minimum size for data-output arrays...
            }

            alpha = new double[biggestKey + 1];
            beta = new double[biggestKey + 1];

            gateInSeconds = ((double)multiplicityGateWidth) * ticSizeInSeconds;
            phi = (deadTimeCoeffTinNanoSecs / 1E9) / gateInSeconds;
            bool standard = true;  // dev note: this toggle signals use of BigNum when FP overflow is detected
            int axover = 0, bxover = 0;
            //calculate the alphas
            alpha[0] = 0.0;
            alpha[1] = 1.0;
            //todo: do math simplification here as done for SR. HN
            for (int n = 2; n <= biggestKey; n++)
            {
                if (phi > 1e-20)
                {
                    alpha[n] = 1.0;
                    if (standard)
                    {
                        for (int k = 0; k <= (n - 2); k++)
                        {
                            double alphaCoeff;
                            alphaCoeff = binomialCoefficient((n - 1), (k + 1))
                                         * Math.Pow((double)(k + 1), (double)k)
                                         * Math.Pow(phi, (double)k)
                                         / (Math.Pow((1.0 - ((k + 1) * phi)), (double)(k + 2)));
                            alpha[n] += alphaCoeff;
                            if (Double.IsInfinity(alpha[n]) || Double.IsNaN(alpha[n]))
                            {
                                result.warnings.Add("Overflow alpha at n = " + n + ", k = " + k);
                                alpha[n] = 0;
                                axover = n;
                                standard = false; k = n; n = n - 1; // redo the loop
                                α = new BigFloat[biggestKey + 1];
								Log.TraceEvent(LogLevels.Warning, 13, result.warnings[result.warnings.Count - 1]);
                            }
                        }
                    }
                    else
                    {
                        // INCCCycleConditioning.calc_alpha_beta 
                        PrecisionSpec ps128 = new PrecisionSpec(128, PrecisionSpec.BaseType.BIN);
                        BigFloat one = new BigFloat(1, ps128);
                        BigFloat combination;
                        BigFloat sum;
                        double raise1, power1, power2;
                        double log1, log2, log3;
                        BigFloat exp1, exp2, exp3;
                        /* calculate alpha array */
                        sum = new BigFloat(0, ps128);
                        for (int k = 0; k <= (n - 2); k++)
                        {
                            combination = new BigFloat(binomialCoefficient((n - 1), (k + 1)), ps128);
                            raise1 = (double)(k + 1);
                            power1 = (double)k;
                            power2 = (double)(k + 2);
                            log1 = Math.Log(raise1);
                            log2 = Math.Log(phi);
                            log3 = Math.Log(1.0 - raise1 * phi);
                            exp1 = BigFloat.Exp(new BigFloat(log1 * power1, ps128));
                            exp2 = BigFloat.Exp(new BigFloat(log2 * power1, ps128));
                            exp3 = BigFloat.Exp(new BigFloat(log3 * power2, ps128));
                            sum += combination * exp1 * exp2 / exp3;
                        }
                        α[n] = new BigFloat(one + sum, ps128);
                    }
                }
                else
                {
                    alpha[n] = 1.0;
                }
            }

            //calculate the betas
            standard = true;
            beta[0] = 0.0;
            beta[1] = 0.0;
            beta[2] = alpha[2] - 1.0;
            for (int n = 3; n <= biggestKey; n++)
            {
                if (phi > 1e-20)
                {
                    beta[n] = alpha[n] - 1.0;
                    if (standard)
                    {
                        for (int k = 0; k <= (n - 3); k++)
                        {
                            double betaCoeff;
                            betaCoeff = binomialCoefficient((n - 1), (k + 2))
                                        * (k + 1)
                                        * Math.Pow((double)(k + 2), (double)k)
                                        * Math.Pow(phi, (double)k)
                                        / (Math.Pow((1.0 - ((k + 2) * phi)), (double)(k + 3)));
                            beta[n] += betaCoeff;
                            if (Double.IsInfinity(beta[n]) || Double.IsNaN(beta[n]))
                            {
                                result.warnings.Add("Overflow beta at n = " + n + ", k = " + k);
                                beta[n] = 0;
                                bxover = n;
                                standard = false; k = n; n = n - 1; // redo the loop
                                β = new BigFloat[biggestKey + 1];
								Log.TraceEvent(LogLevels.Warning, 13, result.warnings[result.warnings.Count - 1]);
                            }
                        }
                    }
                    else
                    {
                        PrecisionSpec ps128 = new PrecisionSpec(128, PrecisionSpec.BaseType.BIN);
                        BigFloat one = new BigFloat(1, ps128);
                        BigFloat combination;
                        BigFloat sum;
                        double raise1, power1, power2;
                        double log1, log2, log3;
                        BigFloat exp1, exp2, exp3;
                        sum = new BigFloat(0, ps128);
                        for (int k = 0; k <= n - 3; k++)
                        {
                            combination = new BigFloat(binomialCoefficient((n - 1), (k + 2)), ps128);
                            raise1 = (double)(k + 2);
                            power1 = (double)k;
                            power2 = (double)(k + 3);
                            log1 = Math.Log(raise1);
                            log2 = Math.Log(phi);
                            log3 = Math.Log(1.0 - raise1 * phi);
                            exp1 = BigFloat.Exp(new BigFloat(log1 * power1, ps128));
                            exp2 = BigFloat.Exp(new BigFloat(log2 * power1, ps128));
                            exp3 = BigFloat.Exp(new BigFloat(log3 * power2, ps128));
                            sum += combination * (new BigFloat(k + 1, ps128)) * exp1 * exp2 / exp3;
                        }
                        β[n] = α[n] - one + sum;
                    }
                }
                else
                {
                    beta[n] = 0.0;
                }
            }

            //store the alpha and beta coefficients
            result.alpha = new double[biggestKey + 1];
            result.beta = new double[biggestKey + 1];
            for (int i = 0; i <= biggestKey; i++)
            {
                result.alpha[i] = alpha[i];
                result.beta[i] = beta[i];
            }

            double lastGoodD = 0;
            for (int i = axover; axover > 0 && i <= biggestKey; i++)
            {
                double d;
                bool good = Double.TryParse(α[i].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out d);  // what to do when it is really larger than a double?
                if (!good)
                {
                    result.alpha[i] = lastGoodD;
                    result.warnings.Add(String.Format("α[{0}] conversion failed on {1}", i, α[i].ToString()));
 					Log.TraceEvent(LogLevels.Warning, 13, result.warnings[result.warnings.Count - 1]);
               }
                else
                {
                    lastGoodD = d;
                    result.alpha[i] = d;
                }
            }

            for (int i = bxover; bxover > 0 && i <= biggestKey; i++)
            {
                double d;
                bool good = Double.TryParse(β[i].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out d); 
                if (!good)
                {
                    result.beta[i] = lastGoodD;
                    result.warnings.Add(String.Format("β[{0}] conversion failed on {1}", i, β[i].ToString()));  // URGENT: alpha/beta arrays are to be transformed from doubles to BigFloat types when this conversion happens to overflow                
					Log.TraceEvent(LogLevels.Warning, 13, result.warnings[result.warnings.Count - 1]);
                 }
                else
                {
                    lastGoodD = d;
                    result.beta[i] = d;
                }
            }

            //NOTE: in the following calculations,
            //variables named RAxxx refer to "Reals Plus Accidentals" phenomena, and
            //variables named Axxx  refer to "Accidentals" phenomena (such as, fast-background counting)

            //calculate the factorial moments
            double RAfactorialMoment0, RAfactorialMoment1, RAfactorialMoment2, RAfactorialMoment3;
            double AfactorialMoment0, AfactorialMoment1, AfactorialMoment2, AfactorialMoment3;
            double RAfactorialMomentAlpha1, AfactorialMomentAlpha1;
            double RAfactorialMomentBeta2, AfactorialMomentBeta2;
            RAfactorialMoment0 = 0.0;
            RAfactorialMoment1 = 0.0;
            RAfactorialMoment2 = 0.0;
            RAfactorialMoment3 = 0.0;
            AfactorialMoment0 = 0.0;
            AfactorialMoment1 = 0.0;
            AfactorialMoment2 = 0.0;
            AfactorialMoment3 = 0.0;
            RAfactorialMomentAlpha1 = 0.0;
            AfactorialMomentAlpha1 = 0.0;
            RAfactorialMomentBeta2 = 0.0;
            AfactorialMomentBeta2 = 0.0;

            for (int i = 0; i <= biggestKey; i++)
            {
                UInt64 gateCount;
                int j = i + 1;
                int k = i + 2;
                int L = i + 3;

                if (result.realPlusAccidentalDistribution.TryGetValue((UInt64)i, out gateCount))
                {
                    RAfactorialMoment0 += (double)gateCount;
                }
                if (result.accidentalDistribution.TryGetValue((UInt64)i, out gateCount))
                {
                    AfactorialMoment0 += gateCount;
                }

                if (j <= biggestKey)
                {
                    if (result.realPlusAccidentalDistribution.TryGetValue((UInt64)j, out gateCount))
                    {
                        RAfactorialMoment1 += (double)(((UInt64)j) * gateCount);
                        RAfactorialMomentAlpha1 += alpha[j] * ((double)gateCount);
                    }
                    if (result.accidentalDistribution.TryGetValue((UInt64)j, out gateCount))
                    {
                        AfactorialMoment1 += (double)(((UInt64)j) * gateCount);
                        AfactorialMomentAlpha1 += alpha[j] * ((double)gateCount);
                    }
                }

                if (k <= biggestKey)
                {
                    if (result.realPlusAccidentalDistribution.TryGetValue((UInt32)k, out gateCount))
                    {
                        RAfactorialMoment2 += (double)(((UInt64)(k * (k - 1) / 2)) * gateCount);
                        RAfactorialMomentBeta2 += beta[k] * ((double)gateCount);
                    }
                    if (result.accidentalDistribution.TryGetValue((UInt32)k, out gateCount))
                    {
                        AfactorialMoment2 += (double)(((UInt64)(k * (k - 1) / 2)) * gateCount);
                        AfactorialMomentBeta2 += beta[k] * ((double)gateCount);
                    }
                }

                if (L <= biggestKey)
                {
                    if (result.realPlusAccidentalDistribution.TryGetValue((UInt32)L, out gateCount))
                    {
                        RAfactorialMoment3 += ((double)(((UInt64)(L * (L - 1) * (L - 2))) * gateCount)) / 6.0;
                    }
                    if (result.accidentalDistribution.TryGetValue((UInt32)L, out gateCount))
                    {
                        AfactorialMoment3 += ((double)(((UInt64)(L * (L - 1) * (L - 2))) * gateCount)) / 6.0;
                    }
                }
            }

            //store the factorial moments
            result.RAfactorialMoment0 = RAfactorialMoment0;
            result.RAfactorialMoment1 = RAfactorialMoment1;
            result.RAfactorialMoment2 = RAfactorialMoment2;
            result.RAfactorialMoment3 = RAfactorialMoment3;
            result.AfactorialMoment0 = AfactorialMoment0;
            result.AfactorialMoment1 = AfactorialMoment1;
            result.AfactorialMoment2 = AfactorialMoment2;
            result.AfactorialMoment3 = AfactorialMoment3;

            //NOTE: in the following calculations,
            //variables named PTxxx refer to "Pulse Triggered" phenomena, and
            //variables named RTxxx refer to "Regularly Triggered" phenomena (such as, fast-background counting)

            //penultimately, use all this to calculate the SDT rates
            double PTsingles, RTsingles, normRAfactorialMoment1, normRAfactorialMoment2;
            double normRAfactorialMomentAlpha1, normRAfactorialMomentBeta2;
            double normAfactorialMoment0, normAfactorialMoment1, normAfactorialMoment2, normAfactorialMoment3;
            double normAfactorialMomentAlpha1, normAfactorialMomentBeta2;

            if (wasFastAccidentals)
            {
                PTsingles = RAfactorialMoment0;
                //double gateFactor = numAccidentalGates / Math.Floor(totalMeasurementTime / (multiplicityGateWidth * ticSizeInSeconds));
                RTsingles = AfactorialMoment1 / (AfactorialMoment0*TimeSpan.FromTicks((long)multiplicityGateWidth).TotalSeconds);
                normRAfactorialMoment1 = RAfactorialMoment1 / PTsingles;
                normRAfactorialMoment2 = RAfactorialMoment2 / PTsingles;
                //NOT USED:  double normRAfactorialMoment3 = RAfactorialMoment3 / PTsingles;
                normRAfactorialMomentAlpha1 = RAfactorialMomentAlpha1 / PTsingles;
                normRAfactorialMomentBeta2 = RAfactorialMomentBeta2 / PTsingles;
                normAfactorialMoment0 = AfactorialMoment0 / AfactorialMoment0;
                normAfactorialMoment1 = AfactorialMoment1 / AfactorialMoment0;
                normAfactorialMoment2 = AfactorialMoment2 / AfactorialMoment0;
                normAfactorialMoment3 = AfactorialMoment3 / AfactorialMoment0;
                normAfactorialMomentAlpha1 = AfactorialMomentAlpha1 / AfactorialMoment0;
                normAfactorialMomentBeta2 = AfactorialMomentBeta2 / AfactorialMoment0;
            }
            else
            {
                PTsingles = RAfactorialMoment0;  //XXX SHOULDN'T THIS BE RAfactorialMoment0 not AfactorialMoment0???, answer, no, the two values should be the same, RA and A of 0 are identical for "slow"
                RTsingles = AfactorialMoment0;
                normRAfactorialMoment1 = RAfactorialMoment1 / PTsingles;
                normRAfactorialMoment2 = RAfactorialMoment2 / PTsingles;
                //NOT USED:  double normRAfactorialMoment3 = RAfactorialMoment3 / PTsingles;
                normRAfactorialMomentAlpha1 = RAfactorialMomentAlpha1 / PTsingles;
                normRAfactorialMomentBeta2 = RAfactorialMomentBeta2 / PTsingles;
                normAfactorialMoment0 = AfactorialMoment0 / RTsingles;
                normAfactorialMoment1 = AfactorialMoment1 / RTsingles;
                normAfactorialMoment2 = AfactorialMoment2 / RTsingles;
                normAfactorialMoment3 = AfactorialMoment3 / RTsingles;
                normAfactorialMomentAlpha1 = AfactorialMomentAlpha1 / RTsingles;
                normAfactorialMomentBeta2 = AfactorialMomentBeta2 / RTsingles;
            }

            double RTdoubles = (0.5 / (multiplicityGateWidth * ticSizeInSeconds)) * ((2.0 * normAfactorialMoment2) - Math.Pow(normAfactorialMoment1, 2.0));
            double RTtriples = (0.16667 / (multiplicityGateWidth * ticSizeInSeconds))
                               * ((6.0 * normAfactorialMoment3) - (6.0 * normAfactorialMoment1 * normAfactorialMoment2) + (2.0 * Math.Pow(normAfactorialMoment1, 3)));

            double PTdoubles = PTsingles * (normRAfactorialMoment1 - normAfactorialMoment1);
            double PTtriples;
            double PTtriplesDTcoef;
            //Store alphabeta intermediates
            result.AfactorialAlphaMoment1 = normAfactorialMomentAlpha1;
            result.RAfactorialAlphaMoment1 = normRAfactorialMomentAlpha1;
            result.AfactorialBetaMoment2= normAfactorialMomentBeta2;
            result.RAfactorialBetaMoment2 = normRAfactorialMomentBeta2;
            if (AfactorialMoment0 != 0.0)
            {
                PTtriples = PTsingles * ((normRAfactorialMoment2 - normAfactorialMoment2)
                                       - ((normAfactorialMoment1 / normAfactorialMoment0)
                                          * (normRAfactorialMoment1 - normAfactorialMoment1)));
                PTtriplesDTcoef = PTsingles * ((normRAfactorialMomentBeta2 - normAfactorialMomentBeta2)
                                               - ((normAfactorialMomentAlpha1 / normAfactorialMoment0)
                                                  * (normRAfactorialMomentAlpha1 - normAfactorialMomentAlpha1)));
            }
            else
            {
                PTtriples = 0.0;
                PTtriplesDTcoef = 0.0;
            }

            if (totalMeasurementTime > 1E-12)
            {
                PTsingles /= totalMeasurementTime;
                PTdoubles /= totalMeasurementTime;
                PTtriples /= totalMeasurementTime;
                PTtriplesDTcoef /= totalMeasurementTime;
            }
            else
            {
                PTsingles = 0.0;
                PTdoubles = 0.0;
                PTtriples = 0.0;
                PTtriplesDTcoef = 0.0;
            }

            //store the SDT rates
            result.singlesRatePerSecond = PTsingles;
            result.doublesRatePerSecond = PTdoubles;
            result.triplesRatePerSecond = PTtriples;

            //now that rates are calculated, calculate the dead-time corrections

            // dead time correction coefficients for RT rates (INCC as well as H&C)
            // determined experimentally using D/T ratio - see analysisComparison/triplesDeadTime.xls
            // the best fit (poly3) used to reproduce the trend
            // note: valid only for sources in the range of ~100n/s - ~500,000 n/s
            /** NOT USED ***
            double DTcoeffT0_RT = 3.42854465;
            double DTcoeffT1_RT = 3.35351651E-6;
            double DTcoeffT2_RT = -5.83706327E-12;
            double DTcoeffT3_RT = 2.03604973E-17;
             ***************/

            // dead time correction coefficients for PT rates with background calculated using H&C consecutive gates
            // determined experimentally using D/T ratio - see analysisComparison/triplesDeadTime.xls
            // the best fit (poly3) used to reproduce the trend
            // note: valid only for sources in the range of ~100n/s - ~500,000 n/s
            /** NOT USED ***
            double DTcoeffT0_PT = 2.78760077;
            double DTcoeffT1_PT = 2.86078894E-6;
            double DTcoeffT2_PT = -8.21994836E-12;
            double DTcoeffT3_PT = 9.45195862E-17;
             ***************/

            /** NOT USED ***
            double DTcoeffA_RT = 0.2063;  // these values were determined using two source method
            double DTcoeffB_RT = 0.04256;
             ***************/

            double DTcoeffA = deadTimeCoeffAinMicroSecs;
            double DTcoeffB = deadTimeCoeffBinPicoSecs;
            double DTcoeffC = deadTimeCoeffCinNanoSecs;
            double DTcoeffT = deadTimeCoeffTinNanoSecs;

            double exponent = ((DTcoeffA / 1E6) + ((DTcoeffB / 1E12) * PTsingles)) * PTsingles;
            double PTsinglesDTcorr = PTsingles * Math.Exp(exponent / 4.0);
            double PTdoublesDTcorr = PTdoubles * Math.Exp(exponent);
            double PTtriplesDTcorr = PTtriplesDTcoef * Math.Exp((DTcoeffT / 1E9) * PTsingles);

            /** NOT USED ***
            double RTsinglesDTcorr = RTsingles * Math.Exp(( ((DTcoeffA/1E6) + ((DTcoeffB/1E12)*RTsingles)) *RTsingles)/4.0);
            double RTdoublesDTcorr = RTdoubles * Math.Exp( ((DTcoeffA_RT/1E6) + ((DTcoeffB_RT/1E12)*RTsingles)) *RTsingles);
            double RTtriplesDTcorr = RTtriples* (( (DTcoeffT3_RT*Math.Pow(RTsingles,3)) 
                                                   + (DTcoeffT2_RT*Math.Pow(RTsingles,2)) 
                                                   + (DTcoeffT1_RT*RTsingles)
                                                   + DTcoeffT0_RT)
                                                 /DTcoeffT0_RT); 
             ***************/

            //store the dead-time corrected values
            result.deadTimeCorrectedSinglesRate = PTsinglesDTcorr;
            result.deadTimeCorrectedDoublesRate = PTdoublesDTcorr;
            result.deadTimeCorrectedTriplesRate = PTtriplesDTcorr;

            //Calculate the Dytlewski Dead-Time-Corrected Rates, based upon
            //N. Dytlewski, Dead-time corrections for multiplicity counters,
            //Nuclear Instruments and Methods in Physics Research A305(1991)492-494
            double P03, P02, P13, P12, P23;
            P03 = Math.Pow((1.0 - (2.0 * phi)), 3.0);
            P02 = Math.Pow((1.0 - phi), 2.0);
            P13 = (2.0 * Math.Pow((1.0 - phi), 3.0)) - (2.0 * P03);
            P12 = 1.0 - P02;
            P23 = 1.0 + P03 - (2.0 * Math.Pow((1.0 - phi), 3.0));
            result.dytlewskiDeadTimeCorrectedTriplesRate = PTtriples / P03;
            //Martyn made me do it. hn 2.6.2015
            result.deadTimeCorrectedTriplesRate = result.dytlewskiDeadTimeCorrectedTriplesRate;
            result.dytlewskiDeadTimeCorrectedDoublesRate = (PTdoubles / P02) + ((PTtriples * P13) / (P02 * P03));
            result.dytlewskiDeadTimeCorrectedSinglesRate = PTsingles + ((P12 * PTdoubles) / P02) + (PTtriples * (((P12 * P13) / (P02 * P03)) - (P23 / P03)));

            return (result);
        }


        /// <summary>
        /// binomialCoefficient calculates "a choose b"
        /// or  a! / (b! * (a-b)!)
        /// 
        /// Academic testing shows this algorithm is valid for "a" as large as 1029,
        /// above which the calculation overflows the maximum size of the 64-bit double,
        /// which has a maximum size of 1.7 E+308.
        /// 
        /// For a == 1029, the biggest coefficient is ~1.4xE+308.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double binomialCoefficient(int a, int b)
        {
            double answer;
            int c;

            answer = 1.0;

            if (a > b)
            {
                int i;

                // make "c" the lesser of b and a-b, whichever is less than a/2
                if ((2 * b) > a)
                    c = a - b;
                else
                    c = b;

                for (i = 0; i < c; i++)
                {
                    double x;

                    x = ((double)(a - i)) / ((double)(i + 1));
                    answer *= x;
                }
            }

            return (answer);
        }

		public static void SetAlphaBeta(Multiplicity mkey, MultiplicityCountingRes mcr)
		{
			ABKey abkey = new ABKey(mkey, mcr);
			SetAlphaBeta(abkey, mcr.AB);
		}

		/// <summary>
		/// Calc alpha beta, save result in cache
		/// NEXT: extend with Numerics.BigInteger replacing BigFloat, as per above (for when phi is non-zero)
		/// </summary>
		/// <param name="key">three values needed for alpha beta (max bin count, gatewidth, phi aka multiplicity T) </param>
		/// <param name="AB">The alpha beta array as used on detectors and multiplicity counting results</param>
		public static void SetAlphaBeta(ABKey key, AlphaBeta AB)
		{
			AlphaBeta _AB = AlphaBetaCache.GetAlphaBeta(key);
            if (_AB != null)
			{
				AB.α = _AB.α;
				AB.β = _AB.β;
				return;
            }

			if (key.bins != AB.α.Length)
				AB.Resize((int)key.bins + 1);

			AB.α[0] = 0.0;
			AB.β[0] = 0.0;
			if (key.deadTimeCoefficientTinNanoSecs == 0.0)
			{
				double n = 0;
				for (uint i = 1; i < key.bins; i++)
				{
					n = i;
					AB.α[i] = n;
					AB.β[i] = (n * (n - 1.0)) / 2.0;
				}
			} else if (key.gateWidthTics != 0.0)
			{
				uint biggestKey = key.bins - 1;
				AB.Init((int)key.bins);
                if (biggestKey <= 1)
                    goto cache;

                double gateInSeconds = key.gateWidthTics * 1e-7;
				double phi = (key.deadTimeCoefficientTinNanoSecs / 1E9) / gateInSeconds;

				AB.α[0] = 0.0;
				AB.α[1] = 1.0;
				AB.β[0] = 0.0;
				AB.β[1] = 0.0;

				if (biggestKey > 1)
				{
					for (int n = 2; n <= biggestKey; n++)
					{
						if (phi > 1e-20)
						{
							AB.α[n] = 1.0;
							double alphaCoeff = 0;
							for (int k = 0; k <= (n - 2); k++)
							{
								alphaCoeff = binomialCoefficient(n - 1, k + 1)
											* Math.Pow((k + 1) * phi, k)
											/ Math.Pow(1.0 - ((k + 1) * phi), k + 2);
								AB.α[n] += alphaCoeff;
							}
						} else
						{
							AB.α[n] = 1.0;
						}
					}

					AB.β[0] = 0.0;
					AB.β[1] = 0.0;
					AB.β[2] = AB.α[2] - 1.0;
					for (int n = 3; n <= biggestKey; n++)
					{
						if (phi > 1e-20)
						{
							AB.β[n] = AB.α[n] - 1.0;
							for (int k = 0; k <= (n - 3); k++)
							{
								double betaCoeff;
								betaCoeff = binomialCoefficient(n - 1, k + 2)
											* (k + 1)
											* Math.Pow((k + 2) * phi, k)
											/ Math.Pow(1.0 - ((k + 2) * phi), k + 3);
								AB.β[n] += betaCoeff;
							}
						} else
						{
							AB.β[n] = 0.0;
						}
					}
				}
			}
cache:		AlphaBetaCache.AddAlphaBeta(key, AB);
		}

	}
}
