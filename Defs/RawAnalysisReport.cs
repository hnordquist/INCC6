/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using DetectorDefs;
using NCCReporter;
namespace AnalysisDefs
{

    using N = NCC.CentralizedState;

    /*
 * select report sections, 
 * select report section detail,
 * gen report sections, 
 * StartReportGeneration
 * ConstructReportSections
 * FinishReportGeneration
 * that's it for now
 * */

    public class SimpleRawReport : SimpleReport
    {

        // these enums support only the simplest measurements and reports: rates and background

        public enum DetailsOrdering { CountersOverCycles, CyclesOverCounters }  // counters are rategates, feynman, rossi, eventspacing

        protected enum ReportSections { SofwareContext, DescriptiveSummary, MeasurementDetails, DetectorCalibration, RawAndMultSums, ComputedMultiplicityIntermediates, CycleSource, ChannelCounts, ChannelRates, MultiplicityDistributions, RawCycles, RateCycles, DTCRateCycles, RepResults, RepDytlewskiResults }

        enum DescriptiveSummary { Facility, MBA, MeasDate, ResultsFileName, InspNum, InspName, Comment }
        enum MeasurementDetails { MeasType, DetectorConfig, DataSource, QCTests, ErrorCalc, AccidentalsMethod, CycleCount, TotalCountTime }
        enum BaseDetectorCalibration
        {
            DetectorType, DetectorId, ElectronicsId
        }
        enum DetectorCalibration
        {
            DetectorType = BaseDetectorCalibration.DetectorType, 
            DetectorId = BaseDetectorCalibration.DetectorId, 
            ElectronicsId = BaseDetectorCalibration.ElectronicsId,

            Predelay, GateLength, HighVoltage, DieAwayTime, Efficiency,
            DTCoeffT, DTCoeffA, DTCoeffB, DTCoeffC, DoublesGateFraction, TriplesGateFraction,

            FA, LongDelay,
            Status
        }
        enum RawSums { Singles, RA, A }//, Scaler1, Scaler2 }
        enum CycleSource { Source, Identifier, DateTime }
        enum DistributionsAndAlphaBeta { RA, A, Alpha, Beta }
        enum eMultiplicityDistributions { RA, A }
        enum ComputedMultiplicityIntermediates { RAMoments, AMoments }

        enum INCCCycles { Singles, RA, A, Scaler1, Scaler2, QCTests }
        enum RawCycles { Singles, RA, A, QCTests }
        enum RateCycles { Singles, Doubles, Triples, Mass, QCTests }
        enum DTCRateCycles { Singles = RateCycles.Singles, Doubles = RateCycles.Doubles, Triples = RateCycles.Triples, Mass = RateCycles.Mass, QCTests = RateCycles.QCTests }
        enum RepResults { Singles, SinglesSigma, Doubles, DoublesSigma, Triples, TripleSigmas } //,, Quads, QuadsSigma }; //, Scaler1, Scaler1Sigma, Scaler2, Scaler2Sigma }
        enum ChannelCounts
        {
            C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, C26, C27, C28, C29, C30, C31, C32
        };


        public SimpleRawReport(NCCReporter.LMLoggers.LognLM ctrllog)
            : base(ctrllog)
        {
            selectedReportSections = Array.CreateInstance(typeof(bool), System.Enum.GetValues(typeof(ReportSections)).Length);
            foreach (ValueType v in System.Enum.GetValues(typeof(ReportSections)))
            {
                selectedReportSections.SetValue(true, (int)v);
            }
            FASelector = FAType.FAOn;
        }

        protected Array selectedReportSections;
        protected FAType FASelector;


        string DetectorCalibrationHeader(ValueType e)
        {
            string s = e.ToString();
            switch ((DetectorCalibration)e)
            {
                case DetectorCalibration.LongDelay:
                case DetectorCalibration.Predelay:
                case DetectorCalibration.GateLength:
                case DetectorCalibration.DieAwayTime:
                case DetectorCalibration.DTCoeffA:
                    s += " uSec";
                    break;
                case DetectorCalibration.DTCoeffT:
                case DetectorCalibration.DTCoeffC:
                    s += " nSec";
                    break;
                case DetectorCalibration.DTCoeffB:
                    s += " pSec";
                    break;
            }
            return s;
        }

        /*
         * call ConstructReportSections with a cycle to 
         *   gen empty row
         *   gen header row
         *   gen data row(s)
         */
        protected Section ConstructReportSection(ReportSections section)
        {
            Section sec = null;
            if (!(bool)selectedReportSections.GetValue((int)section))
                return sec;
            try
            {
                Row[] temp;
                switch (section)
                {
                    case ReportSections.SofwareContext:
                        temp = GenSoftwareConfigRows();
                        sec = new Section(null, 1, 1, temp.Length);
                        sec[1].Add(0, "Software application configuration details");
                        sec.InsertRange(sec.dataidx, temp);
                        break;
                    case ReportSections.DescriptiveSummary:
                        sec = new Section(typeof(DescriptiveSummary), 0, 1, 1);
                        sec.Add(GenDescriptiveSummaryRow());
                        break;
                    case ReportSections.MeasurementDetails:
                        sec = new Section(typeof(MeasurementDetails), 0, 1, 1);
                        sec.Add(GenMeasurementDetailsRow());
                        break;
                    case ReportSections.DetectorCalibration:

                        sec = new Section(typeof(DetectorCalibration), 0, 1, 1);
                        Row hcrow = sec[sec.Count - 1];
                        hcrow.Clear();
                        hcrow.TS = DetectorCalibrationHeader;
                        if (meas.CountingAnalysisResults.HasMultiplicity)
                        {
                            hcrow.GenFromEnum(typeof(DetectorCalibration), null, 0);
                            // there is a disconnect between the detector list and the Instrument list,
                            // there is disconnect between the detector and the subordinate SR analyzer param sets;
                            IEnumerator iter = meas.CountingAnalysisResults.GetMultiplicityEnumerator(true);
                            while (iter.MoveNext())
                            {
                                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                                sec.Add(GenDetectorCalibrationRow(meas.Detectors[0], mkey));
                            }
                        }
                        else
                        {
                            hcrow.GenFromEnum(typeof(BaseDetectorCalibration), null, 0);
                            sec.Add(GenDetectorCalibrationRow(meas.Detectors[0]));
                        }

                        break;
                    case ReportSections.ComputedMultiplicityIntermediates:
                        // RA, A, alpha, beta
                        // 0...3 rows
                        //sec = DoAcrossForManyMults("Averaged moments (NNV)", typeof(ComputedMultiplicityIntermediates), false);
                        //temp = GenMultiplicityIntermediatesRow(m.CountingAnalysisResults);
                        //sec.AddRange(temp);
                        break;
                    case ReportSections.ChannelCounts:
                        temp = GenChannelCountsRows();
                        sec = new Section(typeof(ChannelCounts), 1, 1, temp.Length);
                        sec[1].Add(0, "Hits per channel sums");
                        sec.InsertRange(sec.dataidx, temp);
                        break;
                    case ReportSections.ChannelRates:
                        temp = GenChannelRatesRows();
                        sec = new Section(typeof(ChannelCounts), 1, 1, temp.Length);
                        sec[1].Add(0, "Channel hits per second (averaged over the cycles)");
                        sec.InsertRange(sec.dataidx, temp);
                        break;
                    case ReportSections.RawAndMultSums:
                        sec = DoAcrossForManyMults("Summed cycle data", typeof(RawSums), false);
                        if (sec != null)
                        { 
                            sec.Add(GenRawSumsRow(meas.CountingAnalysisResults));  // done: uses # of analyzers and goes across
                            temp = GenMultDistRows(meas.CountingAnalysisResults, 0); //done: sum for all analyzers, see method comments
                            Section sec2 = new Section(typeof(DistributionsAndAlphaBeta), 0, 0, temp.Length, "Bin", meas.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity)));
                            sec.AddRange(sec2);
                            sec.AddRange(temp);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected Section ConstructReportSection(ReportSections section, Multiplicity mu, MultiplicityCountingRes mcr)
        {
            Section sec = null;
            if (!(bool)selectedReportSections.GetValue((int)section))
                return sec;
            try
            {
                switch (section)
                {
                    case ReportSections.RepResults:
                        sec = new Section(typeof(RepResults), 1, 1, 1);
                        sec[1].Add(0, "Corrected rates for SR " + mu.ShortName());
                        sec.Add(GenResultsRow(mcr));
                        break;
                    case ReportSections.RepDytlewskiResults:
                        sec = new Section(typeof(RepResults), 1, 1, 1);
                        sec[1].Add(0, "Dytlewski rates for SR " + mu.ShortName());
                        sec.Add(GenDytlewskiResultsRow(mcr));
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected Section ConstructReportSection(ReportSections section, CycleList cycles)
        {
            Section sec = null;
            if (!(bool)selectedReportSections.GetValue((int)section))
                return sec;
            try
            {
                switch (section)
                {
                    // these have multiple rows based on the report content
                    case ReportSections.CycleSource:
                        sec = new Section(typeof(CycleSource), 1, 1, 1, "Cycle");
                        sec[1].Add(0, "Cycle source");
                        break;
                    case ReportSections.MultiplicityDistributions:
                        sec = DoAcrossForManyMults("Cycle multiplicity distributions", typeof(eMultiplicityDistributions), true);
                        break;
                    case ReportSections.ChannelCounts:
                        break;
                    case ReportSections.ChannelRates:
                        break;
                    case ReportSections.ComputedMultiplicityIntermediates:
                        sec = DoAcrossForManyMults("Cycle moments (NNV)", typeof(ComputedMultiplicityIntermediates), true);
                        break;
                    case ReportSections.RawCycles:
                        if (cycles.Count > 0 && cycles[0].DataSourceId.source.INCCTransferData()) // todo: when SR DAQ works, need to choose INCC here too, because SR returns Scaler data
                            sec = DoAcrossForManyMults("Cycle raw data", typeof(INCCCycles), true);
                        else
                            sec = DoAcrossForManyMults("Cycle raw data", typeof(RawCycles), true);
                        break;
                    case ReportSections.DTCRateCycles:
                        sec = DoAcrossForManyMults("Cycle DTC rate data", typeof(DTCRateCycles), true);
                        break;
                    case ReportSections.RateCycles:
                        sec = DoAcrossForManyMults("Cycle rate data", typeof(RateCycles), true);
                        break;
                    case ReportSections.RepResults:
                        break;
                    case ReportSections.RepDytlewskiResults:

                        break;
                }
                if (sec == null) // no mult-based data
                    return sec;
                Row[] temp = null;
                foreach (Cycle cyc in cycles)
                {
                    switch (section)
                    {
                        // these have multiple rows based on the report content
                        case ReportSections.MultiplicityDistributions:
                            int repeat = meas.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity));
                            temp = GenMultDistRows(cyc.CountingAnalysisResults, cyc.seq); // done: uses # of analyzers and goes across
                            Section sec2 = new Section(typeof(eMultiplicityDistributions), 0, 0, temp.Length, "Cycle " + cyc.seq, repeat);
                            sec.AddRange(sec2);
                            sec.AddRange(temp);
                            break;
                        case ReportSections.ChannelCounts:
                            //    sec.Add(GenChannelCountsRows(cyc)); 
                            break;
                        case ReportSections.ChannelRates:
                            //    sec.Add(GenChannelCountsRows(cyc)); 
                            break;
                        case ReportSections.ComputedMultiplicityIntermediates:
                            temp = GenMultiplicityIntermediatesRow(cyc.CountingAnalysisResults, cyc); // alpha and beta are only in the summary because they do not change (gatelength-based)                        
                            sec.AddRange(temp);
                            break;
                        case ReportSections.RawCycles:
                            if (cyc.DataSourceId.source.SRDAQ(cyc.DataSourceId.SRType)) 
                                sec.Add(GenINCCCycleRow(cyc));
                            else
                                sec.Add(GenRawCycleRow(cyc)); // done: uses # of analyzers and goes across
                            break;
                        case ReportSections.DTCRateCycles:
                            sec.Add(GenRateCycleRow(cyc, dtc:true));  // # of analyzers and goes across
                            break;
                        case ReportSections.RateCycles:
                            sec.Add(GenRateCycleRow(cyc));
                            break;
                        case ReportSections.RepResults:
                            //    sec.Add(GenResultsCycleRow(cyc));
                            break;
                        case ReportSections.RepDytlewskiResults:
                            //    sec.Add(GenDytlewskiResultsCycleRow(cyc));
                            break;
                        case ReportSections.CycleSource:
                            sec.Add(GenCycleSourceRow(cyc));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected Section DoAcrossForManyMults(String title, Type column, bool cyclecolumn)
        {
            Section sec = null;
            int cyccol = (cyclecolumn ? 1 : 0);
            int rep = meas.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity));
            if (rep < 1)
            {
                return sec;
            }
            sec = new Section(column, 1, 1, 1, cyclecolumn ? "Cycle" : null, rep);
            sec[1].Add(0, title);
            int ecount = System.Enum.GetValues(column).Length;
            int i = 0;
            rep = 0;

            IEnumerator iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.Multiplicity));
            while (iter.MoveNext())
            {
                rep = 1 + cyccol + (ecount * i);
                Multiplicity mu = (Multiplicity)iter.Current;
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)meas.CountingAnalysisResults[mu];
                sec[1].Add(rep, "SR " + mu.ShortName());
                for (int j = 1; j < ecount; j++)
                    sec[1].Add(1 + cyccol + j + (ecount * i), "");
                i++;
            }
            return sec;
        }

        Row GenDescriptiveSummaryRow()
        {
            Row row = new Row();
            row.Add((int)DescriptiveSummary.Facility, meas.AcquireState.facility.ToString());
            row.Add((int)DescriptiveSummary.MBA, meas.AcquireState.mba.ToString());
            row.Add((int)DescriptiveSummary.MeasDate, meas.MeasDate.ToString());
            row.Add((int)DescriptiveSummary.ResultsFileName, meas.ResultsFileName);
            row.Add((int)DescriptiveSummary.InspNum, "");
            row.Add((int)DescriptiveSummary.InspName, "");
            row.Add((int)DescriptiveSummary.Comment, meas.AcquireState.comment);
            return row;
        }
        Row GenMeasurementDetailsRow()
        {
            string s = meas.AcquireState.data_src.HappyFunName() + " " + meas.AcquireState.data_src.ToString();
            if (meas.AcquireState.data_src.CompareTo(ConstructedSource.Live) != 0)
                // The other flag you were using here seemed to always be true.  If not live, it is from a file, right? hn 5.13.2015
            {
                if (N.App.AppContext.INCCXfer)
                    s = "INCC transfer " + s;
                else if (N.App.AppContext.PulseFileAssay)
                    s = "Sorted pulse " + s;
                else if (N.App.AppContext.PTRFileAssay)
                    s = "PTR-32 dual " + s;
                else
                    s = "NCD " + s;
            }
            Row row = new Row();
            row.Add((int)MeasurementDetails.MeasType, meas.MeasOption.PrintName());
            row.Add((int)MeasurementDetails.DetectorConfig, ""); // what was this supposed to be?
            row.Add((int)MeasurementDetails.DataSource, s);
            row.Add((int)MeasurementDetails.QCTests, meas.AcquireState.qc_tests ? "On" : "Off");
            row.Add((int)MeasurementDetails.ErrorCalc, meas.AcquireState.error_calc_method.ToString());
            // ErrorCalculationTechniqueExtensions.Override(meas.AcquireState.error_calc_method, meas.MeasOption, meas.Detectors.GetIt(mkey.sr)).ToString());
            row.Add((int)MeasurementDetails.AccidentalsMethod, meas.Tests.accidentalsMethod.ToString() + "d");
            row.Add((int)MeasurementDetails.CycleCount, meas.Cycles.Count.ToString());
            row.Add((int)MeasurementDetails.TotalCountTime, meas.CountTimeInSeconds.ToString());  // an averaged time 
            return row;
        }

        Row GenDetectorCalibrationRow(Detector det, Multiplicity mkey = null)
        {
            Row row = new Row();
            ShiftRegisterParameters sr = (mkey == null ? det.SRParams : mkey.SR);
            row.Add((int)DetectorCalibration.DetectorType, det.Id.SRType.ToString());
            row.Add((int)DetectorCalibration.DetectorId, det.Id.DetectorId);
            row.Add((int)DetectorCalibration.ElectronicsId, det.Id.ElectronicsId);
            if (mkey != null)
            {
                row.Add((int)DetectorCalibration.Predelay, (sr.predelay * 1e-1).ToString());
                row.Add((int)DetectorCalibration.GateLength, (sr.gateLength * 1e-1).ToString());
                row.Add((int)DetectorCalibration.HighVoltage, sr.highVoltage.ToString());
                row.Add((int)DetectorCalibration.DieAwayTime, (sr.dieAwayTime * 1e-1).ToString());
                row.Add((int)DetectorCalibration.Efficiency, sr.efficiency.ToString());
                row.Add((int)DetectorCalibration.DTCoeffT, (sr.deadTimeCoefficientTinNanoSecs).ToString());
                row.Add((int)DetectorCalibration.DTCoeffA, (sr.deadTimeCoefficientAinMicroSecs).ToString());
                row.Add((int)DetectorCalibration.DTCoeffB, (sr.deadTimeCoefficientBinPicoSecs).ToString());
                row.Add((int)DetectorCalibration.DTCoeffC, (sr.deadTimeCoefficientCinNanoSecs).ToString());
                row.Add((int)DetectorCalibration.DoublesGateFraction, sr.doublesGateFraction.ToString());
                row.Add((int)DetectorCalibration.TriplesGateFraction, sr.triplesGateFraction.ToString());


                if (det.Id.SRType.IsListMode()) // Only add long delay for LM instruments hn 9.21.2015
                {
                    row.Add((int)DetectorCalibration.FA, mkey.FA.ToString());
                    row.Add((int)DetectorCalibration.LongDelay, (mkey.FA == FAType.FAOn ? mkey.BackgroundGateTimeStepInTics * 1e-1 : mkey.AccidentalsGateDelayInTics * 1e-1).ToString());
                }
                else
                {
                    row.Add((int)DetectorCalibration.FA, "N/A");
                    row.Add((int)DetectorCalibration.LongDelay, "N/A");
                }
                row.Add((int)DetectorCalibration.Status, (mkey.suspect ? "Unusable because " + mkey.reason : "OK"));
            }
            return row;
        }

        Row[] GenMultiplicityIntermediatesRow(CountingResultsMap map, Cycle c = null)
        {
            Row[] rows = new Row[4] { new Row(), new Row(), new Row(), new Row() };  // four moments, 1 row for each
            //row.Add((int)ComputedMultiplicityIntermediates.Alpha, "");
            //row.Add((int)ComputedMultiplicityIntermediates.Beta, "");
            IEnumerator iter = map.GetATypedResultEnumerator(typeof(AnalysisDefs.Multiplicity));
            int ecount = System.Enum.GetValues(typeof(ComputedMultiplicityIntermediates)).Length;
            int i = 0, repeat = 0, r = 0;
            int cyclerow = (c != null ? 1 : 0);
            if (c != null)
            {
                for (r = 0; r < 4; r++)
                {
                    rows[r].Add(0, String.Format("{0}+{1}", (c.seq).ToString(), r.ToString()));
                }
            }
            while (iter.MoveNext())
            {
                repeat = (ecount * i) + cyclerow;
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)iter.Current;
                for (r = 0; r < 4; r++)
                {
                    rows[r].Add((int)ComputedMultiplicityIntermediates.RAMoments + repeat, mcr.RAFactorialMoments[r].ToString());
                    rows[r].Add((int)ComputedMultiplicityIntermediates.AMoments + repeat, mcr.AFactorialMoments[r].ToString());
                }
                i++;
            }
            return rows;
        }

        Row GenRawSumsRow(CountingResultsMap map)  // done: uses # of analyzers and goes across
        {
            Row row = new Row();
            IEnumerator iter = map.GetMultiplicityEnumerator();
            int ecount = System.Enum.GetValues(typeof(RawSums)).Length;
            int entries = 0, repeat = 0;
            while (iter.MoveNext())
            {
                repeat = (ecount * entries);
                //Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;  // clean up this syntax, it's silly
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
                row.Add((int)RawSums.Singles + repeat, meas.SinglesSum.ToString());
                row.Add((int)RawSums.RA + repeat, mcr.RASum.ToString());
                row.Add((int)RawSums.A + repeat, mcr.ASum.ToString());
                //Detector d = meas.Detectors.GetIt(mkey.sr)
                //if (d.Id.SRType <= LMDAQ.InstrType.AMSR)
                //{
                //    //row.Add((int)RawSums.Scaler1, m.Scaler1Sum.ToString());
                //    //row.Add((int)RawSums.Scaler2, m.Scaler2Sum.ToString());
                //}
                entries++;
            }
            return row;
        }

        Row GenResultsRow(MultiplicityCountingRes mcr)
        {
            Row row = new Row();
            row.Add((int)RepResults.Singles, mcr.DeadtimeCorrectedSinglesRate.v.ToString()); // todo: move to results list entry processing scope
            row.Add((int)RepResults.SinglesSigma, mcr.DeadtimeCorrectedSinglesRate.err.ToString());
            row.Add((int)RepResults.Doubles, mcr.DeadtimeCorrectedDoublesRate.v.ToString());
            row.Add((int)RepResults.DoublesSigma, mcr.DeadtimeCorrectedDoublesRate.err.ToString());
            if (!meas.AcquireState.data_src.ToString().Equals ("Shift Register"))
            {
                row.Add((int)RepResults.Triples, mcr.DeadtimeCorrectedTriplesRate.v.ToString());
                row.Add((int)RepResults.TripleSigmas, mcr.DeadtimeCorrectedTriplesRate.err.ToString());
            }
            //row.Add((int)RepResults.Quads, "");
            //row.Add((int)RepResults.QuadsSigma, "");
            return row;
        }
        Row GenDytlewskiResultsRow(MultiplicityCountingRes mcr)
        {
            Row row = new Row();
            row.Add((int)RepResults.Singles, mcr.DytlewskiCorrectedSinglesRate.v.ToString());
            row.Add((int)RepResults.SinglesSigma, mcr.DytlewskiCorrectedSinglesRate.err.ToString());
            row.Add((int)RepResults.Doubles, mcr.DytlewskiCorrectedDoublesRate.v.ToString());
            row.Add((int)RepResults.DoublesSigma, mcr.DytlewskiCorrectedDoublesRate.err.ToString());
            row.Add((int)RepResults.Triples, mcr.DytlewskiCorrectedTriplesRate.v.ToString());
            row.Add((int)RepResults.TripleSigmas, mcr.DytlewskiCorrectedTriplesRate.err.ToString());
            //row.Add((int)RepResults.Quads, "");
            //row.Add((int)RepResults.QuadsSigma, "");
            return row;
        }

        Row[] GenMultDistRows(CountingResultsMap map, int cycle)  // done: uses # of analyzers and goes across
        {
            ulong minbins = 0, maxbins = 0;
            IEnumerator iter = map.GetATypedResultEnumerator(typeof(AnalysisDefs.Multiplicity));
            while (iter.MoveNext())
            {
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)iter.Current;
                //GetMaxNonZeroLength(mcr.RAMult, mcr.NormedAMult, ref newlen);
                minbins = Math.Max(mcr.MinBins, minbins);
                maxbins = Math.Max(mcr.MaxBins, maxbins);
                //newlen = maxbins;
            }
            int ecount = cycle < 1 ? System.Enum.GetValues(typeof(DistributionsAndAlphaBeta)).Length : System.Enum.GetValues(typeof(eMultiplicityDistributions)).Length;
            Row[] rows = new Row[maxbins];
            int repeat = 0;
            iter = map.GetMultiplicityEnumerator();
            for (ulong i = 0; i < minbins; i++)
            {
                rows[i] = new Row();
                rows[i].Add(0, i.ToString());
            }
            for (ulong i = minbins; i < maxbins; i++)
            {
                rows[i] = new Row();
                rows[i].Add(0, i.ToString());
            }
            int step = 0;
            while (iter.MoveNext())
            {
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
                repeat = 1 + (ecount * step);

                for (ulong i = 0; i < minbins; i++)
                {
                    double RA, A, alpha, beta;
                    if ((ulong)mcr.RAMult.Length <= i)
                        RA = 0;
                    else
                        RA = mcr.RAMult[i];
                    if ((ulong)mcr.NormedAMult.Length <= i)
                        A = 0;
                    else
                        A = mcr.NormedAMult[i];
                    if ((ulong)mcr.alpha.Length <= i)
                        alpha = 0;
                    else
                        alpha = mcr.alpha[i];
                    if ((ulong)mcr.beta.Length <= i)
                        beta = 0;
                    else
                        beta = mcr.beta[i];
                    rows[i].Add((int)DistributionsAndAlphaBeta.RA + repeat, RA.ToString());
                    rows[i].Add((int)DistributionsAndAlphaBeta.A + repeat, A.ToString());
                    if (cycle < 1)
                    {
                        rows[i].Add((int)DistributionsAndAlphaBeta.Alpha + repeat, alpha.ToString());
                        rows[i].Add((int)DistributionsAndAlphaBeta.Beta + repeat, beta.ToString());
                    }
                }
                for (ulong i = minbins; i < maxbins; i++)
                {
                    double RA, A, alpha, beta;
                    if ((ulong)mcr.RAMult.Length <= i)
                        RA = 0;
                    else
                        RA = mcr.RAMult[i];
                    if ((ulong)mcr.NormedAMult.Length <= i)
                        A = 0;
                    else
                        A = mcr.NormedAMult[i];
                    if ((ulong)mcr.alpha.Length <= i)
                        alpha = 0;
                    else
                        alpha = mcr.alpha[i];
                    if ((ulong)mcr.beta.Length <= i)
                        beta = 0;
                    else
                        beta = mcr.beta[i];
                    rows[i].Add((int)DistributionsAndAlphaBeta.RA + repeat, RA.ToString());
                    rows[i].Add((int)DistributionsAndAlphaBeta.A + repeat, A.ToString());
                    if (cycle < 1)
                    {
                        rows[i].Add((int)DistributionsAndAlphaBeta.Alpha + repeat, alpha.ToString());
                        rows[i].Add((int)DistributionsAndAlphaBeta.Beta + repeat, beta.ToString());
                    }
                }
                step++;
            }
            return rows;
        }

        Row[] GenChannelCountsRows(Cycle c) // todo: implement
        {
            Row[] rows = new Row[1];
            return rows;
        }

        void GetMaxNonZeroLength(ulong[] RA, ulong[] A, ref ulong newlen)
        {
            try
            {
                int i = Math.Min(RA.Length, A.Length) - 1;
                for (; i >= 0; i--)
                {
                    if (RA[i] != 0.0 || A[i] != 0.0)
                        break;
                }
                newlen = Math.Max(newlen, (ulong)i + 1);
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
        }

        // Uses cycle counts on m.cycles.HitsPerChannel
        Row[] GenChannelCountsRows()
        {
            Row[] rows = new Row[1];
            rows[0] = new Row(); // reimplemented using  meas.Cycles.HitsPerChannel
            //RatesResultEnhanced rrm = meas.CountingAnalysisResults.GetFirstRatesResultMod(); // todo: only the first is used here, still need multiple rate analzyer output
            //// could be null due to lack of check for suspect status here, so account for it
            //if (rrm == null)
            //{
            //    BaseRate br = meas.CountingAnalysisResults.GetFirstRatesResultModKey(true);
            //    if (br != null)
            //    {
            //        rows[0].Add(0, "Unusable");
            //        rows[0].Add(1, br.reason);
            //    }
            //}
            //else
            //{
            //    for (int j = 0; j < N.ChannelCount; j++)
            //    {
            //        rows[0].Add(j, rrm.neutronsPerChannel[j].ToString());
            //    }
            //}
            // if no base rate analyzer available, use byte-counter based 1 sec rates
            if (rows[0].Count == 0)
            {
                for (int j = 0; j < N.ChannelCount; j++)
                {
                    rows[0].Add(j, meas.Cycles.HitsPerChannel[j].ToString());
                }
            }
            return rows;
        }

        // Uses cycle counts on m.cycles.HitsPerChannel
        Row[] GenChannelRatesRows()
        {
            Row[] rows = new Row[1];
            rows[0] = new Row(); // reimplemented using  meas.Cycles.HitsPerChannel
            //RatesResultEnhanced rrm = meas.CountingAnalysisResults.GetFirstRatesResultMod(); // todo: only the first is used here, still need multiple rate analyzer output
            //// could be null due to lack of check for suspect status here, so account for it            
            //if (rrm == null)
            //{
            //    BaseRate br = meas.CountingAnalysisResults.GetFirstRatesResultModKey(true);
            //    if (br != null)
            //    {
            //        rows[0].Add(0, "Unusable");
            //        rows[0].Add(1, br.reason);
            //    }
            //}
            //else
            //{
            //    for (int j = 0; j < N.ChannelCount; j++)
            //    {
            //        double v = 0;
            //        //ulong c = (ulong)meas.Cycles.Count;
            //        if (meas.CountTimeInSeconds != 0)
            //            v = (rrm.neutronsPerChannel[j] / meas.CountTimeInSeconds);
            //        rows[0].Add(j, v.ToString());
            //    }
            //}
            //// if no base rate analyzer available, use byte-counter based 1 sec rates
            if (rows[0].Count == 0)
            {
                for (int j = 0; j < N.ChannelCount; j++)
                {
                    double v = 0;
                    //ulong c = (ulong)meas.Cycles.Count;
                    if (meas.CountTimeInSeconds != 0)
                        v = (meas.Cycles.HitsPerChannel[j] / meas.CountTimeInSeconds);
                    rows[0].Add(j, v.ToString());
                }
            }
            return rows;
        }

        Row GenRawCycleRow(Cycle c) // uses # of analyzers and goes across
        {
            Row row = new Row();
            IEnumerator iter = c.CountingAnalysisResults.GetMultiplicityEnumerator();
            int ecount = System.Enum.GetValues(typeof(RawCycles)).Length;
            int i = 0, repeat = 0;
            row.Add(0, c.seq.ToString()); // todo: what if we are skipping cycles? 
            while (iter.MoveNext())
            {
                repeat = 1 + (ecount * i);
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;  // clean up this syntax, it's silly
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;

                row.Add((int)RawCycles.Singles + repeat, mcr.Totals.ToString());
                //row.Add((int)RawCycles.Singles + repeat, "0");
                row.Add((int)RawCycles.RA + repeat, mcr.RASum.ToString());
                row.Add((int)RawCycles.A + repeat, mcr.ASum.ToString());
                row.Add((int)RawCycles.QCTests + repeat, c.QCStatus(mkey).INCCString());
                i++;
            }
            return row;
        }

        Row GenINCCCycleRow(Cycle c) // uses # of analyzers and goes across
        {
            Row row = new Row();
            IEnumerator iter = c.CountingAnalysisResults.GetMultiplicityEnumerator();
            int ecount = System.Enum.GetValues(typeof(INCCCycles)).Length;
            int i = 0, repeat = 0;
            row.Add(0, c.seq.ToString()); // todo: what if we are skipping cycles? 
            while (iter.MoveNext())
            {
                repeat = 1 + (ecount * i);
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;  // clean up this syntax, it's silly
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;

                row.Add((int)INCCCycles.Singles + repeat, mcr.Totals.ToString());
                row.Add((int)INCCCycles.RA + repeat, mcr.RASum.ToString());
                row.Add((int)INCCCycles.A + repeat, mcr.ASum.ToString());
                row.Add((int)INCCCycles.Scaler1 + repeat, mcr.Scaler1.v.ToString());
                row.Add((int)INCCCycles.Scaler2 + repeat, mcr.Scaler2.v.ToString());
                row.Add((int)INCCCycles.QCTests + repeat, c.QCStatus(mkey).INCCString());
                i++;
            }
            return row;
        }

        Row GenRateCycleRow(Cycle c, bool dtc = false)  // uses # of analyzers and goes across
        {
            IEnumerator iter = c.CountingAnalysisResults.GetMultiplicityEnumerator();
            Row row = new Row();
            int ecount = System.Enum.GetValues(typeof(RateCycles)).Length;
            int i = 0, repeat = 0;
            row.Add(0, c.seq.ToString());
            while (iter.MoveNext())
            {
                repeat = 1 + (ecount * i);
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;  // clean up this syntax, it's silly
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
                if (dtc) 
                {
                    row.Add((int)RateCycles.Singles + repeat, mcr.DeadtimeCorrectedSinglesRate.v.ToString());
                    row.Add((int)RateCycles.Doubles + repeat, mcr.DeadtimeCorrectedDoublesRate.v.ToString());
                    row.Add((int)RateCycles.Triples + repeat, mcr.DeadtimeCorrectedTriplesRate.v.ToString());
                }
                else
                {
                    row.Add((int)RateCycles.Singles + repeat, mcr.RawSinglesRate.v.ToString());
                    row.Add((int)RateCycles.Doubles + repeat, mcr.RawDoublesRate.v.ToString());
                    if (!meas.AcquireState.data_src.HappyFunName().Equals("Shift register")) // No such thing as raw triples for a shift register.  Don't print them. hn 5.13.2015
                        row.Add((int)RateCycles.Triples + repeat, mcr.RawTriplesRate.v.ToString());
                    else
                        row.Add((int)RateCycles.Triples + repeat, "");
                }
                if (AssaySelector.ForMass(meas.MeasOption))
                    //row.Add((int)RateCycles.Mass + repeat, mcr.Mass.ToString());
                    row.Add((int)RateCycles.Mass + repeat, mcr.mass.ToString());
                else
                    row.Add((int)RateCycles.Mass + repeat, String.Empty);
                row.Add((int)RateCycles.QCTests + repeat, c.QCStatus(mkey).INCCString());
                i++;
            }
            return row;
        }

        Row GenCycleSourceRow(Cycle c)
        {
            Row row = new Row();
            row.Add(0, c.seq.ToString());
            row.Add((int)CycleSource.Source + 1, c.DataSourceId.source.ToString());
            row.Add((int)CycleSource.Identifier + 1, c.DataSourceId.IdentName());
            row.Add((int)CycleSource.DateTime + 1, c.DataSourceId.dt.ToString("MMM dd yyy HH:mm:ss.ff K")); // my favorite format
            return row;
        }
    }

    public class RawAnalysisReport : SimpleRawReport
    {

        enum Feynman
        {
            GateWidth, NumGates, CBar, C2Bar, C3bar, C // first row, second row is counts for up to NumGates
        }

        enum Rossi
        {
            GateWidth, Numgates // the data row is 1000 integers exactly
        }

        enum TimeInterval
        {
            GateWidth, Bins // it is an histogram of an arbitrary length
        }

        enum RateIntervalCalc
        {
            OverallTime
        }
        
        enum RateInterval  // as calculated in the analyzers
        {
            GateWidth, CompletedGates, OverallTime
        }

        enum CoincidenceMatrix
        {
            GateWidth, PreDelay, LongDelay
        }

        protected enum RawReportSections { HitsPerChn, RatePerChn, Feynman, Rossi, TimeInterval, CoincidenceMatrix };
        protected new Array selectedReportSections; // DB$CONFIG
        public List<string> replines;
        public RawAnalysisReport(NCCReporter.LMLoggers.LognLM ctrllog)
            : base(ctrllog)
        {

            selectedReportSections = Array.CreateInstance(typeof(bool), System.Enum.GetValues(typeof(RawReportSections)).Length);
            foreach (ValueType v in System.Enum.GetValues(typeof(RawReportSections)))// dev note: these would be user selections eventually, for now we enable every possible report section
            {
                selectedReportSections.SetValue(true, (int)v);
            }
        }


        string CoincidenceHeader(ValueType e)
        {
            string s = e.ToString();
            switch ((CoincidenceMatrix)e)
            {
                case CoincidenceMatrix.LongDelay:
                case CoincidenceMatrix.PreDelay:
                case CoincidenceMatrix.GateWidth:
                    s += " uSec";
                    break;
            }
            return s;
        }
        string RateHeader(ValueType e)
        {
            string s = e.ToString();
            switch ((RateInterval)e)
            {
                case RateInterval.GateWidth:
                    s += " 1e-7 s";
                    break;
                case RateInterval.OverallTime:
                    s += " sec";
                    break;
            }
            return s;
        }
        string RateHeaderCalc(ValueType e)
        {
            string s = e.ToString();
            switch ((RateIntervalCalc)e)
            {
                case RateIntervalCalc.OverallTime:
                    s += " sec";
                    break;
            }
            return s;
        }
        protected Section ConstructReportSection(RawReportSections section)
        {
            Section sec = null;
            if (!(bool)selectedReportSections.GetValue((int)section))  // user selection
                return sec;
            try
            {
                int i = 1;
                Row[] temp;
                IEnumerator iter;
                switch (section)
                {
                    case RawReportSections.CoincidenceMatrix:
                        iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.Coincidence));
                        while (iter.MoveNext())
                        {
                            CoincidenceMatrixResult cor = (CoincidenceMatrixResult)iter.Current;
                            if (sec == null) sec = new Section(typeof(CoincidenceMatrix), 1);
                            temp = GenCoincidenceRows(cor);
                            Row r = new Row(); r.Add(0, "Coincidence RA, A and R Matrix results (" + i + ")");
                            sec.AddLabelAndColumn(r);
                            Row hcrow = sec[sec.Count - 1];
                            hcrow.Clear();
                            hcrow.TS = CoincidenceHeader;
                            hcrow.GenFromEnum(typeof(CoincidenceMatrix), null, 0);
                            sec.AddRange(temp);
                            i++;
                        }
                        break;
                    case RawReportSections.Feynman:
                        iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.Feynman));
                        while (iter.MoveNext())
                        {
                            FeynmanResultExt fr = (FeynmanResultExt)iter.Current;
                            if (sec == null) sec = new Section(typeof(Feynman), 1);
                            temp = GenFeynRows(fr);
                            Row r = new Row(); r.Add(0, "Feynman results (" + i + ")");
                            sec.AddLabelAndColumn(r);
                            sec.AddRange(temp);
                            i++;
                        }
                        break;
                    case RawReportSections.Rossi:
                        iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.Rossi));
                        while (iter.MoveNext())
                        {
                            RossiAlphaResultExt rar = (RossiAlphaResultExt)iter.Current;
                            if (sec == null) sec = new Section(typeof(Rossi), 1);
                            temp = GenRossiRows(rar);
                            Row r = new Row(); r.Add(0, "Rossi-" + '\u03B1' + " results (" + i + ")");
                            sec.AddLabelAndColumn(r);
                            sec.AddRange(temp);
                            i++;
                        }
                        break;
                    case RawReportSections.TimeInterval:
                        iter = meas.CountingAnalysisResults.GetATypedResultEnumerator(typeof(AnalysisDefs.TimeInterval));
                        while (iter.MoveNext())
                        {
                            TimeIntervalResult tir = (TimeIntervalResult)iter.Current;
                            if (sec == null) sec = new Section(typeof(TimeInterval), 1);
                            temp = GenTimeIntervalRows(tir);
                            Row r = new Row(); r.Add(0, "Time Interval results (" + i + ")");
                            sec.AddLabelAndColumn(r);
                            sec.AddRange(temp);
                            i++;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }

            return sec;
        }

        // dev note: could make this a single function on four subclassed variants of a parent class
        protected Section ConstructReportSection(RawReportSections section, CycleList cycles)
        {
            Section sec = null;

            if (!(bool)selectedReportSections.GetValue((int)section))  // user selection
                return sec;
            try
            {
                int i = 1;
                Row[] temp;
                IEnumerator iter;
                switch (section)
                {
                    case RawReportSections.CoincidenceMatrix:
                        iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.Coincidence), true);
                        while (iter.MoveNext())
                        {
                            SpecificCountingAnalyzerParams sap = (SpecificCountingAnalyzerParams)iter.Current;
                            if (sec == null) sec = new Section(typeof(CoincidenceMatrix), 1);
                            Row r = new Row(); r.Add(0, "Coincidence RA, A and R Matrix results (" + i + ")");
                            if (sap.suspect || !meas.CountingAnalysisResults.ContainsKey(sap))
                            {
                                r.Add(1, "Unusable results");
                                r.Add(2, sap.reason);
                                continue;
                            }
                            sec.AddLabelAndColumn(r, "Cycle");
                            Row hcrow = sec[sec.Count - 1];
                            hcrow.Clear();
                            hcrow.TS = CoincidenceHeader;
                            hcrow.GenFromEnum(typeof(CoincidenceMatrix), "Cycle", 0);
                            foreach (Cycle cyc in cycles)
                            {
                                CoincidenceMatrixResult cor = (CoincidenceMatrixResult)cyc.CountingAnalysisResults[sap];
                                temp = GenCoincidenceRows(cor, cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                    case RawReportSections.HitsPerChn: // using  cycle.HitsPerChannel
                        //iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.BaseRate), true);
                        //while (iter.MoveNext())
                        //{
                        //    SpecificCountingAnalyzerParams sap = (SpecificCountingAnalyzerParams)iter.Current;
                        //    if (sec == null) sec = new Section(typeof(RateInterval), 1);
                        //    Row r = new Row(); r.Add(0, "Hits per channel (" + i + ")");
                        //    if (sap.suspect || !meas.CountingAnalysisResults.ContainsKey(sap))
                        //    {
                        //        r.Add(1, "Unusable results");
                        //        r.Add(2, sap.reason);
                        //        continue;
                        //    }
                        //    sec.AddLabelAndColumn(r, "Cycle");
                        //    Row hcrow = sec[sec.Count - 1];
                        //    hcrow.Clear();
                        //    hcrow.TS = RateHeader;
                        //    hcrow.GenFromEnum(typeof(RateInterval), "Cycle", 0);
                        //    foreach (Cycle cyc in cycles)
                        //    {
                        //        RatesResultEnhanced ccrrm = (RatesResultEnhanced)cyc.CountingAnalysisResults[sap];
                        //        temp = GenChnCountsRows(ccrrm, cyc);
                        //        sec.AddRange(temp);
                        //    }
                        //    i++;
                        //}

                        {
                            if (sec == null) sec = new Section(typeof(RateIntervalCalc), 1);
                            Row r = new Row(); r.Add(0, "Hits per channel");
                            sec.AddLabelAndColumn(r, "Cycle");
                            Row hcrow = sec[sec.Count - 1];
                            hcrow.Clear();
                            hcrow.TS = RateHeader;
                            hcrow.GenFromEnum(typeof(RateInterval), "Cycle", 0);
                            foreach (Cycle cyc in cycles)
                            {
                                temp = GenChnCountsRows(cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                    case RawReportSections.RatePerChn: // Using cycle.HitsPerChannel
                        {
                            if (sec == null) sec = new Section(typeof(RateIntervalCalc), 1);
                            Row r = new Row(); r.Add(0, "Channel hits per second");
                            sec.AddLabelAndColumn(r, "Cycle");
                            Row hcrow = sec[sec.Count - 1];
                            hcrow.Clear();
                            hcrow.TS = RateHeader;
                            hcrow.GenFromEnum(typeof(RateInterval), "Cycle", 0);
                            foreach (Cycle cyc in cycles)
                            {
                                temp = GenRatesRows(cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                    case RawReportSections.Feynman:
                        iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.Feynman), true);
                        while (iter.MoveNext())
                        {
                            SpecificCountingAnalyzerParams sap = (SpecificCountingAnalyzerParams)iter.Current;
                            if (sec == null) sec = new Section(typeof(Feynman), 1);
                            Row r = new Row(); r.Add(0, "Feynman results (" + i + ")");
                            sec.AddLabelAndColumn(r, "Cycle");
                            foreach (Cycle cyc in cycles)
                            {
                                Object obj;
                                bool there = cyc.CountingAnalysisResults.TryGetValue(sap, out obj);
                                if (!there)
                                    continue;
                                temp = GenFeynRows((FeynmanResultExt)obj, cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                    case RawReportSections.Rossi:
                        iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.Rossi), true);
                        while (iter.MoveNext())
                        {
                            if (sec == null) sec = new Section(typeof(Rossi), 1);
                            SpecificCountingAnalyzerParams sap = (SpecificCountingAnalyzerParams)iter.Current;
                            Row r = new Row(); r.Add(0, "Rossi-" + '\u03B1' + " results (" + i + ")");
                            sec.AddLabelAndColumn(r, "Cycle");
                            foreach (Cycle cyc in cycles)
                            {
                                Object obj;
                                bool there = cyc.CountingAnalysisResults.TryGetValue(sap, out obj);
                                if (!there)
                                    continue;
                                temp = GenRossiRows((RossiAlphaResultExt)obj, cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                    case RawReportSections.TimeInterval:
                        iter = meas.CountingAnalysisResults.GetATypedParameterEnumerator(typeof(AnalysisDefs.TimeInterval), true);
                        while (iter.MoveNext())
                        {
                            if (sec == null) sec = new Section(typeof(TimeInterval), 1);
                            SpecificCountingAnalyzerParams sap = (SpecificCountingAnalyzerParams)iter.Current;
                            Row r = new Row(); r.Add(0, "Time Interval results (" + i + ")");
                            sec.AddLabelAndColumn(r, "Cycle");
                            foreach (Cycle cyc in cycles)
                            {
                                Object obj;
                                bool there = cyc.CountingAnalysisResults.TryGetValue(sap, out obj);
                                if (!there)
                                    continue;
                                temp = GenTimeIntervalRows((TimeIntervalResult)obj, cyc);
                                sec.AddRange(temp);
                            }
                            i++;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }

            return sec;
        }

        // todo: these are too much cut-and-paste, the class design is shouting out loud here through the results class hierarchy, so do one
        Row[] GenRatesRows(RatesResultEnhanced rrm, Cycle c = null)
        {
            Row[] rows = new Row[2];
            rows[0] = GenRatesParamsRow(rrm, c); // interval and completed intervals
            Row[] rows2 = GenRatesDataRows(rrm, c); // the channel rate for the cycle
            rows[1] = rows2[0];
            return rows;
        }
        Row[] GenRatesRows(Cycle c = null)
        {
            Row[] rows = new Row[2];
            rows[0] = GenRatesParamsRow(c); // interval and completed intervals
            Row[] rows2 = GenRatesDataRows(c); // the channel rate for the cycle
            rows[1] = rows2[0];
            return rows;
        }
        Row[] GenChnCountsRows(RatesResultEnhanced rrm, Cycle c = null)
        {
            Row[] rows = new Row[2];
            rows[0] = GenRatesParamsRow(rrm, c); // interval and completed intervals
            Row[] rows2 = GenChnCountsRow(rrm, c); // the channel hits for the cycle
            rows[1] = rows2[0];
            return rows;
        }
        Row[] GenChnCountsRows(Cycle c)
        {
            Row[] rows = new Row[2];
            rows[0] = GenRatesParamsRow(c); // interval and completed intervals
            Row[] rows2 = GenChnCountsRow(c); // the channel hits for the cycle
            rows[1] = rows2[0];
            return rows;
        }
        Row[] GenCoincidenceRows(CoincidenceMatrixResult cor, Cycle c = null)
        {
            Row[] rows = new Row[1 + cor.RACoincidenceRate.Length];
            rows[0] = GenCoincidenceParamsRow(cor, c); // ??
            Row[] rows2 = GenCoincidenceDataRows(cor, c); // RA numxnum and A numxnum and R numxnum
            Array.Copy(rows2, 0, rows, 1, rows2.Length);
            // removed the Cn header rows, rows[2] = rows2[1];
            return rows;
        }
        Row[] GenFeynRows(FeynmanResultExt fr, Cycle c = null)
        {
            Row[] rows = new Row[2];
            rows[0] = GenFeynParamsRow(fr, c);
            rows[1] = GenFeynmanDataRow(fr, c);
            return rows;
        }

        Row[] GenRossiRows(RossiAlphaResultExt rar, Cycle c = null)
        {
            Row[] rows = new Row[2];
            rows[0] = GenRossiParamsRow(rar, c);
            rows[1] = GenRossiDataRow(rar, c);
            return rows;
        }
        Row[] GenTimeIntervalRows(TimeIntervalResult esr, Cycle c = null)
        {
            Row[] rows = new Row[3];
            rows[0] = GenTimeIntervalParamsRow(esr, c);
            Row[] rows2 = GenTimeIntervalDataRows(esr, c);
            rows[1] = rows2[0];
            rows[2] = rows2[1];
            return rows;
        }
        Row GenFeynParamsRow(FeynmanResultExt fr, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;

            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            row.Add((int)Feynman.GateWidth + shift, fr.gateWidth.ToString());
            row.Add((int)Feynman.NumGates + shift, fr.numGatesHavingNumNeutrons.Count.ToString());
            row.Add((int)Feynman.CBar + shift, fr.cbar.ToString());
            row.Add((int)Feynman.C2Bar + shift, fr.c2bar.ToString());
            row.Add((int)Feynman.C3bar + shift, fr.c3bar.ToString());
            row.Add((int)Feynman.C + shift, fr.C.ToString());
            return row;
        }
        Row GenFeynmanDataRow(FeynmanResultExt fr, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }

            foreach (KeyValuePair<UInt32, UInt32> pair in fr.numGatesHavingNumNeutrons)
            {
                row.Add((int)pair.Key + shift, pair.Value.ToString());
            };

            return row;
        }


        Row GenRossiParamsRow(RossiAlphaResultExt rar, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }

            row.Add((int)Rossi.GateWidth + shift, rar.gateWidth.ToString());
            row.Add((int)Rossi.Numgates + shift, rar.gateData.Length.ToString());
            return row;
        }

        // print only up to the last non-zero entry, later use the Feynman X,Y dual row technique for this compression attempt
        Row GenRossiDataRow(RossiAlphaResultExt rar, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            int maxindex = rar.gateData.Length - 1;
            int i = 0;
            for (i = rar.gateData.Length - 1; i >= 0; i--)
            {
                if (rar.gateData[i] > 0)
                {
                    maxindex = i;
                    break;
                }
            }
            //happy dad!
            if (i == 0) // rolled all the way to the start ofthe array and found all 0s, empty bins!
            {
                maxindex = 0; // not 1000 and not -1
            }

            for (i = 0; i <= maxindex; i++)
            {
                row.Add(i + shift, rar.gateData[i].ToString());
            }
            return row;
        }
        Row GenTimeIntervalParamsRow(TimeIntervalResult esr, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            row.Add((int)TimeInterval.GateWidth + shift, esr.gateWidthInTics.ToString());
            row.Add((int)TimeInterval.Bins + shift, esr.maxIndexOfNonzeroHistogramEntry.ToString());
            return row;
        }
        Row[] GenTimeIntervalDataRows(TimeIntervalResult tir, Cycle c = null)
        {
            Row[] rows = { new Row(), new Row() };
            int shift = 0;
            if (c != null)  // add the cycle label column
            {
                rows[0].Add(0, c.seq.ToString());
                rows[1].Add(0, c.seq.ToString());
                shift = 1;
            }
            for (int i = 0; i <= tir.maxIndexOfNonzeroHistogramEntry; i++)
            {
                if (tir.timeIntervalHistogram[i] == 0)
                    continue;
                rows[0].Add(i + shift, i.ToString());
                rows[1].Add(i + shift, tir.timeIntervalHistogram[i].ToString());
            }
            return rows;
        }
        Row GenRatesParamsRow(RatesResultEnhanced rrm, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            row.Add((int)RateInterval.GateWidth + shift, rrm.gateWidthInTics.ToString());
            row.Add((int)RateInterval.CompletedGates + shift, rrm.completedIntervals.ToString());
            row.Add((int)RateInterval.OverallTime + shift, rrm.totaltime.TotalSeconds.ToString());
            return row;
        }
        Row GenRatesParamsRow(Cycle c)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            row.Add((int)RateIntervalCalc.OverallTime + shift, c.TS.TotalSeconds.ToString());
            return row;
        }
        Row[] GenRatesDataRows(RatesResultEnhanced rrm, Cycle c = null)
        {
            Row[] rows = { new Row() };
            int shift = 0;
            if (c != null)  // add the cycle label column
            {
                rows[0].Add(0, c.seq.ToString());
                shift = 1;
            }
            // the channel hits/gatewidth for the cycle
            for (int i = 0; i < N.ChannelCount; i++)
            {
                double v = 0;
                if (c.TS.TotalSeconds != 0)
                    v = rrm.neutronsPerChannel[i] / c.TS.TotalSeconds;
                rows[0].Add(i + shift, v.ToString());
            }
            return rows;
        }
        Row[] GenRatesDataRows(Cycle c)
        {
            Row[] rows = { new Row() };
            int shift = 0;
            if (c != null)  // add the cycle label column
            {
                rows[0].Add(0, c.seq.ToString());
                shift = 1;
            }
            // the channel hits/gatewidth for the cycle
            for (int i = 0; i < N.ChannelCount; i++)
            {
                double v = 0;
                if (c.TS.TotalSeconds != 0)
                    v = c.HitsPerChannel[i] / c.TS.TotalSeconds;
                rows[0].Add(i + shift, v.ToString());
            }
            return rows;
        }
        Row[] GenChnCountsRow(RatesResultEnhanced rrm, Cycle c = null)
        {
            Row[] rows = { new Row() };
            int shift = 0;
            if (c != null)  // add the cycle label column
            {
                rows[0].Add(0, c.seq.ToString());
                shift = 1;
            }
            // the channel hits for the cycle
            for (int i = 0; i < N.ChannelCount; i++)
            {
                rows[0].Add(i + shift, rrm.neutronsPerChannel[i].ToString());
            }
            return rows;
        }
        Row[] GenChnCountsRow(Cycle c)
        {
            Row[] rows = { new Row() };
            int shift = 0;
            if (c != null)  // add the cycle label column
            {
                rows[0].Add(0, c.seq.ToString());
                shift = 1;
            }
            // the channel hits for the cycle
            for (int i = 0; i < N.ChannelCount; i++)
            {
                rows[0].Add(i + shift, c.HitsPerChannel[i].ToString());
            }
            return rows;
        }
        Row GenCoincidenceParamsRow(CoincidenceMatrixResult cor, Cycle c = null)
        {
            Row row = new Row();
            int shift = 0;
            if (c != null)
            {
                row.Add(0, c.seq.ToString());
                shift = 1;
            }
            row.Add((int)CoincidenceMatrix.GateWidth + shift, (cor.coincidenceGateWidth * 1e-1).ToString());
            row.Add((int)CoincidenceMatrix.PreDelay + shift, (cor.coincidenceDeadDelay * 1e-1).ToString());
            row.Add((int)CoincidenceMatrix.LongDelay + shift, (cor.accidentalsDelay * 1e-1).ToString());
            return row;
        }

        Row[] GenCoincidenceDataRows(CoincidenceMatrixResult cor, Cycle c = null)
        {
            int len = cor.RACoincidenceRate.Length;
            Row[] rows = new Row[len];
            for (int i = 0; i < len; i++)
                rows[i] = new Row();
            int shift = 0;
            for (int chn = 0; chn < len; chn++)
            {
                if (c != null)  // add the cycle label column
                {
                    rows[chn].Add(0, c.seq.ToString());
                    shift = 1;
                }
                int i = 0;
                // 
                for (i = 0; i < len; i++)
                {
                    rows[chn].Add(i + shift, cor.RACoincidenceRate[chn][i].ToString());
                }
                rows[chn].Add(i + shift, " "); i++;
                for (int j = 0; j < len; j++)
                {
                    rows[chn].Add(i + shift + j, cor.ACoincidenceRate[chn][j].ToString());
                }
                i += 32;
                rows[chn].Add(i + shift, " "); i++;
                for (int j = 0; j < len; j++)
                {
                    rows[chn].Add(i + shift + j, (cor.RACoincidenceRate[chn][j] - cor.ACoincidenceRate[chn][j]).ToString());
                }
            }
            return rows;
        }

        Row[] GenHauckRows(HauckResult rar, Cycle c = null)
        {
            Row[] rows = new Row[2];
            //rows[0] = GenHauckParamsRow(rar, c);  // todo: along with columns and related details
            //rows[1] = GenHauckDataRow(rar, c);
            return rows;
        }

        public override void StartReportGeneration(Measurement m, string pretext, char separator = ',', string suffixoverride = null)  // space char as separator forces text report generation
        {
            base.StartReportGeneration(m, pretext, separator);

            m.ResultsFileName = t.FullFilePath;  // save the full file path on this single member var for later
        }


        public void GenerateReport(Measurement m)
        {
            if (N.App.Opstate.IsAbortRequested) // handle stop, cancel, abort here, for stop does it roll back all report gen from here?
                return;
            StartReportGeneration(m, m.MeasOption.PrintName() + " ");

            try
            {
                // build each report section, order of construction matters here
                // these use the measurement for their data source
                sections.Add(ConstructReportSection(ReportSections.DescriptiveSummary));
                sections.Add(ConstructReportSection(ReportSections.MeasurementDetails));
                sections.Add(ConstructReportSection(ReportSections.DetectorCalibration));

                IEnumerator iter = m.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();
                while (iter.MoveNext())
                {
                    MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
                    INCCResult ir = m.INCCAnalysisResults[moskey];
                    Multiplicity mu = moskey.MultiplicityParams;
                    sections.Add(ConstructReportSection(ReportSections.RepResults, mu, ir));
                    // add back later sections.Add(ConstructReportSection(ReportSections.RepDytlewskiResults, mu, ir));
                }
                sections.Add(ConstructReportSection(ReportSections.ChannelCounts)); // from RateInterval or m.cycles.HitsPerChannel
                sections.Add(ConstructReportSection(ReportSections.ChannelRates)); // from RateInterval and/or  m.cycles.HitsPerChannel
                sections.Add(ConstructReportSection(ReportSections.ComputedMultiplicityIntermediates));

                sections.Add(ConstructReportSection(RawReportSections.CoincidenceMatrix));
                sections.Add(ConstructReportSection(RawReportSections.Feynman));
                sections.Add(ConstructReportSection(RawReportSections.Rossi));
                sections.Add(ConstructReportSection(RawReportSections.TimeInterval));

                sections.Add(ConstructReportSection(ReportSections.RawAndMultSums));

                sections.Add(ConstructReportSection(ReportSections.CycleSource, m.Cycles));
                sections.Add(ConstructReportSection(ReportSections.RawCycles, m.Cycles));
                sections.Add(ConstructReportSection(ReportSections.RateCycles, m.Cycles));
                sections.Add(ConstructReportSection(ReportSections.DTCRateCycles, m.Cycles));
                sections.Add(ConstructReportSection(ReportSections.MultiplicityDistributions, m.Cycles));
                sections.Add(ConstructReportSection(ReportSections.ComputedMultiplicityIntermediates, m.Cycles));

                // dev note: selectable detailed listing 1 or 2
                // dev note: add other ordering choice for output 1) CyclesOverCounters => for all cycles foreach analysis type in rates/gates rates/gates/perChannel, Feyn, Rossis, EventSpacing

                //this is choice 2) DetailsOrdering.CountersOverCycles  
                // 2) foreach analysis type in rates/gates rates/gates/perChannel, Feyn, Rossis, EventSpacing -> for each cycle
                sections.Add(ConstructReportSection(RawReportSections.RatePerChn, m.Cycles));
                sections.Add(ConstructReportSection(RawReportSections.HitsPerChn, m.Cycles)); // from RateInterval
                sections.Add(ConstructReportSection(RawReportSections.CoincidenceMatrix, m.Cycles));
                sections.Add(ConstructReportSection(RawReportSections.Feynman, m.Cycles));
                sections.Add(ConstructReportSection(RawReportSections.Rossi, m.Cycles));
                sections.Add(ConstructReportSection(RawReportSections.TimeInterval, m.Cycles));

                // environmental sampling
                sections.Add(ConstructReportSection(ReportSections.SofwareContext));
                // --> !! trim
                sections.RemoveAll(s => (s == null));

                // copy all section rows to the report row list (t.rows)
                int rowcount = 0;
                foreach (Section sec in sections)
                {
                    rowcount += sec.Count;
                }
                Array.Resize(ref t.rows, rowcount);

                int idx = 0;
                foreach (Section sec in sections)
                {
                    Array.Copy(sec.ToArray(), 0, t.rows, idx, sec.Count); idx += sec.Count;
                }

            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }

            FinishReportGeneration();
            replines = t.lines;
        }

    }





}




