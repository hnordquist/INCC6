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
using DetectorDefs;
using NCCReporter;
namespace AnalysisDefs
{
    using Tuple = VTuple;
    using N = NCC.CentralizedState;


    // these results are 1 - 1 with each srkey analyzer
    public class MethodResultsReport : SimpleReport
    {

        public enum INCCReportSection
        {
            Header,
            Context,
            Isotopics,
            ShiftRegister,
            Adjustments,
            CycleSummary,
            Messages,
            Reference,
            SummedRawData,
            SummedRA,
            MassResults,
            MethodResultsAndParams,
            MultiplicityDistributions, RawCycles, DTCRateCycles
        }

        protected enum Results { Rates, Background, Initial, Precision, Verification, Calibration };

        enum Columns { Label, Value };
        enum ComputedValueColumns { Primary, Err };
        enum ComputedIsoColumns { Declared, DecErr, Updated, UpErr };

        protected Array selectedReportSections;

        public List<List<string>> INCCResultsReports;

        public MethodResultsReport(NCCReporter.LMLoggers.LognLM ctrllog)
            : base(ctrllog)
        {
            INCCResultsReports = new List<List<string>>();
			selectedReportSections = Array.CreateInstance(typeof(bool), System.Enum.GetValues(typeof(INCCReportSection)).Length);
            foreach (ValueType v in System.Enum.GetValues(typeof(INCCReportSection)))
            {
                selectedReportSections.SetValue(false, (int)v);
            }
        }

		public void ApplyReportSectionSelections(bool[] sections)
		{
            if (sections == null)
                return;
            for (int i = 0; i < sections.Length; i++)
			{
                selectedReportSections.SetValue(sections[i], i);
            }
		}


        protected Section ConstructReportSection(INCCReportSection section, MeasOptionSelector moskey, INCCResult ir, Detector det)
        {
            INCCStyleSection sec = null;
            try
            {
                switch (section)
                {

                    // An identical copy of full INCC report sections
                    case INCCReportSection.SummedRawData:
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.Summary);
                        sec.SetFPCurrentFormatPrecision(0);
                        sec.AddHeader(String.Format("{0} summed raw data",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"));  // section header
                        sec.AddNumericRow("Shift register singles sum:", meas.SinglesSum);
                        sec.AddNumericRow("Shift register reals + accidentals sum:", ir.RASum);
                        sec.AddNumericRow("Shift register accidentals sum:", ir.ASum);
                        if (!det.Id.source.UsingVirtualSRCounting(det.Id.SRType))
                        {
                            sec.AddNumericRow("Shift register 1st scaler sum:", ir.S1Sum);
                            sec.AddNumericRow("Shift register 2nd scaler sum:", ir.S2Sum);
                        }
                        break;

                    case INCCReportSection.SummedRA:
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MultiColumn);
                        sec.AddHeader(String.Format("{0} summed multiplicity distributions",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"));  // section header
                        int[] srawidths = new int[] { 5, 12, 12 };
                        int minbin, maxbin;
                        minbin = Math.Min(ir.RAMult.Length, ir.NormedAMult.Length);
                        maxbin = Math.Max(ir.RAMult.Length, ir.NormedAMult.Length);
                        sec.AddColumnRowHeader(new string[] { " ", "R+A sums", "A sums" }, srawidths);
                        for (int i = 0; i < minbin; i++)
                            sec.AddColumnRow(new ulong[] { (ulong)i, ir.RAMult[i], ir.NormedAMult[i] }, srawidths);
                        for (int i = minbin; i < maxbin; i++)  // check for uneven column
                        {
                            ulong[] potential = new ulong[3];
                            potential[0] = (ulong)i;
                            if (i < ir.RAMult.Length)
                                potential[1] = ir.RAMult[i];
                            if (i < ir.NormedAMult.Length)
                                potential[2] = ir.NormedAMult[i];
                            sec.AddColumnRow(potential, srawidths);
                        }
                        break;

                    case INCCReportSection.MassResults:
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MethodResults);
                        //Results are not always passive. Boo.
                        sec.AddHeader(String.Format("{0} results",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"));  // section header
                        sec.AddNumericRow("Singles:", ir.DeadtimeCorrectedSinglesRate);
                        sec.AddNumericRow("Doubles:", ir.DeadtimeCorrectedDoublesRate);
                        sec.AddNumericRow("Triples:", ir.DeadtimeCorrectedTriplesRate);
                        //changed to DTC rates.  Raw rates meaningless here hn 11.5.2014
                        //sec.AddNumericRow("Quads:", mcr.DeadtimeCorrectedQuadsRate); // todo: quads delayed until pents are ready per DN
                        if (!det.Id.source.UsingVirtualSRCounting(det.Id.SRType))
                        {
                            sec.AddNumericRow("Scaler 1:", ir.Scaler1);
                            sec.AddNumericRow("Scaler 2:", ir.Scaler2);
                        }

                        //if (det.Id.SRType >= LMDAQ.InstrType.NPOD)
                        //{
                        //    sec.Add(new Row()); // blank line
                        //    sec.AddNumericRow("Dyt. Singles:", ir.DytlewskiCorrectedSinglesRate);
                        //    sec.AddNumericRow("Dyt. Doubles:", ir.DytlewskiCorrectedDoublesRate);
                        //    sec.AddNumericRow("Dyt. Triples:", ir.DytlewskiCorrectedTriplesRate);
                        //}
                        break;
                    case INCCReportSection.MethodResultsAndParams:
                        // ir contains the measurement option-specific results: empty for rates and holdup, and also empty for calib and verif, the method-focused analyses, 
                        // but values are present for initial, normalization, precision, and should be present for background for the tm bkg results 
                        List<Row> rl = ir.ToLines(meas);
                        sec = new INCCStyleSection(null, 0, INCCStyleSection.ReportSection.MethodResults);
                        sec.AddRange(rl);

                        switch (meas.MeasOption)
                        {
                            case AssaySelector.MeasurementOption.background:
                                if (meas.Background.TMBkgParams.ComputeTMBkg)
                                    ctrllog.TraceEvent(LogLevels.Warning, 82010, "Background truncated multiplicity"); // todo: present the tm bkg results on m.Background
                                break;
                            case AssaySelector.MeasurementOption.initial:
                            case AssaySelector.MeasurementOption.normalization:
                            case AssaySelector.MeasurementOption.precision:
                                break;
                            case AssaySelector.MeasurementOption.verification:
                            case AssaySelector.MeasurementOption.calibration:
                                {
                                    INCCMethodResults imrs;
                                    bool beendonegot = meas.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
                                    if (beendonegot && imrs.Count > 0) // should be true for verification and calibration
                                    {
                                        // we've got a distinct detector id and material type on the methods, so that is the indexer here
                                        Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrs[meas.INCCAnalysisState.Methods.selector];

                                        // now get an enumerator over the map of method results
                                        Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
                                        while (ai.MoveNext())
                                        {
                                            INCCMethodResult imr = ai.Current.Value;
                                            // show the primaryMethod
                                            if (ai.Current.Key.Equals(imrs.primaryMethod))
                                            {
                                                sec.Add(new Row());
                                                Row rh = new Row();
                                                rh.Add(0, "            PRIMARY RESULT");
                                                sec.Add(rh);
                                            }
                                            rl = imr.ToLines(meas);
                                            sec.AddRange(rl);
                                            // devnote: optional use of END_PRIMARY_RESULT as in some INCC report formats, but not others
											if (ai.Current.Key.Equals(imrs.primaryMethod))
                                            {
                                                sec.Add(new Row());
                                                Row rh = new Row();
                                                rh.Add(0, "          END PRIMARY RESULT");
                                                sec.Add(rh);
                                            }
                                        }
                                    }
                                }
                                break;
                            case AssaySelector.MeasurementOption.rates:
                            case AssaySelector.MeasurementOption.holdup:
                            case AssaySelector.MeasurementOption.unspecified:
                            default: // nothing new to present with these
                                break;
                        }
                        break;
                    case INCCReportSection.RawCycles:
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MultiColumn);
                        sec.AddHeader(String.Format("{0} cycle raw data",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"));  // section header
                        int[] crdwidths = new int[] { 5, 10, 10, 10, 10, 10, 10 };
                        sec.AddColumnRowHeader(new string[] { "Cycle", "Singles", "R+A  ", "A    ", "Scaler1", "Scaler2", "QC Tests" }, crdwidths);
                        foreach (Cycle cyc in meas.Cycles)
                        {
                            // if no results on the cycle, these map indexers throw
                            if (cyc.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity)) > 0)                            // if no results on the cycle, these map indexers throw
                            {
                                MultiplicityCountingRes mcr = (MultiplicityCountingRes)cyc.CountingAnalysisResults[moskey.MultiplicityParams];
                                sec.AddCycleColumnRow(cyc.seq,
                                    new ulong[] { (ulong)mcr.Totals, (ulong)mcr.RASum, (ulong)mcr.ASum, (ulong)mcr.Scaler1.v, (ulong)mcr.Scaler2.v },
                                    meas.AcquireState.qc_tests?cyc.QCStatus(moskey.MultiplicityParams).INCCString():"Off", crdwidths);
                            }
                        }
                        break;
                    case INCCReportSection.DTCRateCycles:
                       
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MultiColumn);
                        sec.AddHeader(String.Format("{0} cycle DTC rate data",meas.AcquireState.well_config == WellConfiguration.Active?"Active":"Passive"));  // section header
                        int[] crawidths = new int[] { 5, 13, 13, 13, 13, 10 };
                        sec.AddColumnRowHeader(new string[] { "Cycle", "Singles", "Doubles", "Triples", "Mass", "QC Tests" }, crawidths);
                        
                        foreach (Cycle cyc in meas.Cycles)
                        {
                            if (cyc.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity)) > 0)                            // if no results on the cycle, these map indexers throw
                            {
                                MultiplicityCountingRes mcr = (MultiplicityCountingRes)cyc.CountingAnalysisResults[moskey.MultiplicityParams];
                                //These debug rows show raw rates for comparison hn 10.30
                                //sec.AddCycleColumnRow(cyc.seq,
                                    //Again, could be wrong.
                                //    new double[] { mcr.RawSinglesRate.v, mcr.RawDoublesRate.v, -1, -1 },
                                //     cyc.QCStatus(moskey.MultiplicityParams).INCCString(), crawidths); 
                                    //Again, could be wrong.
                               // TODO: Am actually printing out the DTC rates per cycle.  This seems to work in all cases EXCEPT "precision" hn 11.5
                                sec.AddCycleColumnRow(cyc.seq,
                                     // Using the corrected rates!
                                     new double[] { mcr.DeadtimeCorrectedSinglesRate.v, mcr.DeadtimeCorrectedDoublesRate.v, mcr.DeadtimeCorrectedTriplesRate.v, mcr.mass/*Mass*/ },
                                      meas.AcquireState.qc_tests?cyc.QCStatus(moskey.MultiplicityParams).INCCString():"Off", crawidths);
                            }
                        }
                        break;
                    case INCCReportSection.MultiplicityDistributions:
                        sec = new INCCStyleSection(null, 1, INCCStyleSection.ReportSection.MultiColumn);
                        sec.AddHeader(String.Format("{0} multiplicity distributions for each cycle",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"));  // section header
                        int[] csrawidths = new int[] { 6, 12, 12 };
                        foreach (Cycle cyc in meas.Cycles)
                        {
                            if (cyc.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity)) > 0)                            // if no results on the cycle, these map indexers throw
                            {
                                MultiplicityCountingRes mcr = (MultiplicityCountingRes)cyc.CountingAnalysisResults[moskey.MultiplicityParams];
                                minbin = Math.Min(mcr.RAMult.Length, mcr.NormedAMult.Length);
                                maxbin = Math.Max(mcr.RAMult.Length, mcr.NormedAMult.Length);
                                sec.AddColumnRowHeader(new string[] { "Cycle " + cyc.seq, "R+A ", "A   " }, csrawidths);
                                for (int i = 0; i < minbin; i++)
                                    sec.AddColumnRow(new ulong[] { (ulong)i, mcr.RAMult[i], mcr.NormedAMult[i] }, csrawidths);
                                for (int i = minbin; i < maxbin; i++)  // check for uneven column
                                {
                                    ulong[] potential = new ulong[3];
                                    potential[0] = (ulong)i;
                                    if (i < mcr.RAMult.Length)
                                        potential[1] = mcr.RAMult[i];
                                    if (i < mcr.NormedAMult.Length)
                                        potential[2] = mcr.NormedAMult[i];
                                    sec.AddColumnRow(potential, csrawidths);
                                }
                            }
                            sec.Add(new Row());// blank
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected Section ConstructReportSection(INCCReportSection section, Detector det, MeasOptionSelector moskey = null)
        {
            if (moskey == null)
                return ConstructReportSectionI(section, det);
            else
                return ConstructReportSectionI(section, det, moskey.MultiplicityParams);
        }

        protected Section ConstructReportSectionI(INCCReportSection section, Detector det, Multiplicity mkey = null)
        {
            INCCStyleSection sec = null;
            try
            {
                switch (section)
                {
                    case INCCReportSection.Header:
                        sec = new INCCStyleSection(null, 1);
                        sec.AddHeader(N.App.Name + " " + N.App.Config.VersionString);  // section header
                        break;
                    case INCCReportSection.Context:
                        sec = new INCCStyleSection(null, 0);
                        ConstructContextContent(sec, det);
                        break;
                    case INCCReportSection.Isotopics:
                        if (AssaySelector.ForMass(meas.MeasOption))
                        {
                            sec = new INCCStyleSection(null, 1);
                            sec.SetFPCurrentFormatPrecision(4);
                            Isotopics curiso = Isotopics.update_isotopics(1.0, meas.MeasDate, meas.Isotopics, meas.logger, N.App.AppContext.INCCParity);
                            if (curiso == null)
                            {
                                curiso = new Isotopics();
                                meas.Isotopics.CopyTo(curiso);
                                ctrllog.TraceEvent(LogLevels.Warning, 82034,  "Using incorrect updated defaults for " + meas.Isotopics.id);
                            }
                            sec.AddTwo("Isotopics id: ", meas.Isotopics.id);
                            sec.AddTwo("Isotopics source code: ", meas.Isotopics.source_code.ToString());
                            sec.AddDualNumericRow("Pu238:", meas.Isotopics[Isotope.pu238], curiso[Isotope.pu238]);
                            sec.AddDualNumericRow("Pu239:", meas.Isotopics[Isotope.pu239], curiso[Isotope.pu239]);
                            sec.AddDualNumericRow("Pu240:", meas.Isotopics[Isotope.pu240], curiso[Isotope.pu240]);
                            sec.AddDualNumericRow("Pu241:", meas.Isotopics[Isotope.pu241], curiso[Isotope.pu241]);
                            sec.AddDualNumericRow("Pu242:", meas.Isotopics[Isotope.pu242], curiso[Isotope.pu242]);
                            sec.AddDualDateOnlyRow("Pu date:", meas.Isotopics.pu_date, curiso.pu_date);
                            sec.AddDualNumericRow("Am241:", meas.Isotopics[Isotope.am241], curiso[Isotope.am241]);
                            sec.AddDualDateOnlyRow("Am date:", meas.Isotopics.am_date, curiso.am_date);
                            // dev note: here is where the alternative K vals are added in the Euratom version
                        }
                        break;
                    case INCCReportSection.ShiftRegister:
                        sec = new INCCStyleSection(null, 1);
                        ConstructSRSection(sec, mkey, det);
                        break;
                    case INCCReportSection.Adjustments:
                        sec = new INCCStyleSection(null, 1);
                        if (AssaySelector.ForMass(meas.MeasOption) || meas.MeasOption == AssaySelector.MeasurementOption.rates)
                        {
                            ushort push = sec.FPFormatPrecision;
                            sec.SetFPCurrentFormatPrecision(4);
                            sec.AddNumericRow("Normalization constant:", meas.Norm.currNormalizationConstant);
                            sec.SetFPCurrentFormatPrecision(push);
                        }
                        if (AssaySelector.UsesBackground(meas.MeasOption))
                        {
                            sec.AddNumericRow("Passive singles bkgrnd:", meas.Background.DeadtimeCorrectedSinglesRate);
                            sec.AddNumericRow("Passive doubles bkgrnd:", meas.Background.DeadtimeCorrectedDoublesRate);
                            sec.AddNumericRow("Passive triples bkgrnd:", meas.Background.DeadtimeCorrectedTriplesRate);

                            if (det.Id.SRType <= InstrType.AMSR)
                            {
                                sec.AddNumericRow("Passive scaler1 bkgrnd:", meas.Background.Scaler1.v);
                                sec.AddNumericRow("Passive scaler2 bkgrnd:", meas.Background.Scaler2.v);
                            }

                            sec.AddNumericRow("Active singles bkgrnd:", meas.Background.INCCActive.Singles);
                            sec.AddNumericRow("Active doubles bkgrnd:", meas.Background.INCCActive.Doubles);
                            sec.AddNumericRow("Active triples bkgrnd:", meas.Background.INCCActive.Triples);
                            if (det.Id.SRType <= InstrType.AMSR)
                            {
                                sec.AddNumericRow("Passive scaler1 bkgrnd:", meas.Background.INCCActive.Scaler1Rate);
                                sec.AddNumericRow("Passive scaler2 bkgrnd:", meas.Background.INCCActive.Scaler2Rate);
                            }
                        }
                        break;
                    case INCCReportSection.CycleSummary:
                        sec = new INCCStyleSection(null, 1);
                        sec.AddIntegerRow(String.Format("Number {0} cycles:",meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected()?"Active":"Passive"), (int)meas.Cycles.GetValidCycleCountForThisKey(mkey)); //det.MultiplicityParams)); // could also use CycleList length but CycleList can be longer when a reanalysis occurs and the analysis processing stops short of the end of the list due to modified termination conditions
                        sec.AddNumericRow("Count time (sec):", (meas.Cycles.Count > 0 ? meas.Cycles[0].TS.TotalSeconds : 0.0));
                        break;

                    case INCCReportSection.Messages:
                        List<MeasurementMsg> sl = null;
                        bool found = meas.Messages.TryGetValue(mkey, out sl);
                        if (found)
                        {
                            sec = new INCCStyleSection(null, 1);
                            sec.AddHeader(String.Format("{0} messages", meas.INCCAnalysisState.Methods.HasActiveSelected() || meas.INCCAnalysisState.Methods.HasActiveMultSelected() ? "Active" : "Passive"));  /// todo: is there an active messages section header analog?
                            foreach (MeasurementMsg m in sl)
                            {
                                Row r = new Row();
                                r.Add(0, m.text);  // expand to log style with toString?
                                sec.Add(r);
                            }
                        }
                        break;
                    case INCCReportSection.Reference:
                        if (!meas.ResultsFiles.CSVResultsFileName.IsNullOrEmpty)
                        {
                            sec = new INCCStyleSection(null, 1);
                            sec.AddHeader("Counting results, summaries and cycle counts file name");  // section header
                            Row resline = new Row(); resline.Add(0, "  " + meas.ResultsFiles.CSVResultsFileName.Path);
                            sec.Add(resline);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected void ConstructSRSection(INCCStyleSection sec, Multiplicity mu, Detector det)
        {
            // if this is based on a virtual SR then show it
            if (det.Id.source.UsingVirtualSRCounting(det.Id.SRType))
                sec.AddTwo(" Virtual shift register:", mu.ToString());
            sec.AddTwo("Predelay:", mu.SR.predelayMS);
            sec.AddTwo("Gate length:", mu.SR.gateLengthMS);
            if (det.Id.SRType == InstrType.DGSR)
                sec.AddTwo("Gate length2:", mu.SR.gateLengthMS); 
            sec.AddIntegerRow("High voltage:", (Int32)mu.SR.highVoltage);
            sec.SetFPCurrentFormatPrecision(4);
            sec.AddTwo("Die away time:", mu.SR.dieAwayTimeMS);
            sec.AddTwo("Efficiency:", mu.SR.efficiency);
            sec.AddTwo("Multiplicity deadtime:", mu.SR.deadTimeCoefficientMultiplicityinNanoSecs);
            sec.AddTwo("Coefficient A deadtime:", mu.SR.deadTimeCoefficientAinMicroSecs);
            sec.AddTwo("Coefficient B deadtime:", mu.SR.deadTimeCoefficientBinPicoSecs);
            sec.AddTwo("Coefficient C deadtime:", mu.SR.deadTimeCoefficientCinNanoSecs);
            sec.AddTwo("Doubles gate fraction:", mu.SR.doublesGateFraction);
            sec.AddTwo("Triples gate fraction:", mu.SR.triplesGateFraction);
        }

        protected void ConstructContextContent(INCCStyleSection sec, Detector det)
        {

            sec.AddTwo("Facility: ", meas.AcquireState.facility.Name);
            sec.AddTwo("Material balance area: ", meas.AcquireState.mba.Name);
            sec.AddTwo("Detector type: ", det.Id.Type); // todo: revisit this, there can be multiple detectors because there can be multiple physical and virtual SR Params used to create results
            sec.AddTwo("Detector id: ", det.Id.DetectorId);
            sec.AddTwo("Electronics id: ", det.Id.ElectronicsId);
            sec.AddTwo("Inventory change code: ", meas.AcquireState.inventory_change_code);
            sec.AddTwo("I/O code: ", meas.AcquireState.io_code);
            sec.AddTwo("Measurement date: ", meas.MeasDate.ToString("yy.MM.dd     HH:mm:ss"));
			string name = System.IO.Path.GetFileName( meas.ResultsFiles.PrimaryINCC5Filename.Path);
            sec.AddTwo("Results file name: ", name);
            sec.AddTwo("Inspection number: ", meas.AcquireState.campaign_id);

            if (AssaySelector.ForMass(meas.MeasOption) || meas.MeasOption == AssaySelector.MeasurementOption.rates)
                /* item id only if an assay, calibration, holdup or rates only */
                //todo: need to check why item_id not stored in Measurement object directly....hn 5.14.2015
                // it is located on the AcquireParameters 
                sec.AddTwo("Item id: ", meas.AcquireState.item_id); // or       sec.AddTwo("Item id:",m.AcquireState.item);
            if (AssaySelector.HasStratum(meas.MeasOption))
            {
                sec.AddTwo("Stratum id: ", meas.AcquireState.stratum_id.Name);
                if (meas.Stratum != null)
                {
                    sec.AddTwo("Bias uncertainty:", meas.Stratum.bias_uncertainty);
                    sec.AddTwo("Random uncertainty:", meas.Stratum.random_uncertainty);
                    sec.AddTwo("Systematic uncertainty:", meas.Stratum.systematic_uncertainty);
                    sec.AddTwo("Relative std deviation:", meas.Stratum.relative_std_dev);
                }
                else
                {
                    sec.AddTwo("Bias uncertainty:", "0.0000");
                    sec.AddTwo("Random uncertainty:", "0.0000");
                    sec.AddTwo("Systematic uncertainty:", "0.0000");
                    sec.AddTwo("Relative std deviation:", "0.0000");
                }
            }
            if (AssaySelector.ForMass(meas.MeasOption))
            {
                sec.AddTwo("Material type: ", meas.AcquireState.item_type);
                sec.AddTwo("Original declared mass:", meas.AcquireState.mass);
            }
            sec.AddTwo("Measurement option: ", meas.MeasOption.PrintName());
            if (AssaySelector.DougsBasics(meas.MeasOption))
            {           /* well configuration */
                sec.AddTwo("Detector configuration: ", meas.AcquireState.well_config.ToString());
            }
            sec.AddTwo("Data source: ", det.Id.source.HappyFunName());
            sec.AddTwo("QC tests: ", meas.AcquireState.qc_tests ? "On" : "Off");
            ErrorCalculationTechnique ect = meas.AcquireState.error_calc_method.Override(meas.MeasOption, det.Id.SRType);
            if (ect != ErrorCalculationTechnique.None)
                sec.AddTwo("Error calculation: ", ect.ToString() + " method");
            sec.AddTwo("Accidentals method: ", meas.Tests.accidentalsMethod != AccidentalsMethod.None ? (meas.Tests.accidentalsMethod.ToString() + "d") : "Not set");
            sec.AddTwo("Inspector name: ", meas.AcquireState.user_id);
            sec.AddTwo("Passive comment:", meas.AcquireState.comment);
        }

        public override string GenBaseFileName(string pretext)
		{
			string path;
			if (N.App.AppContext.Results8Char)
				path =  MethodResultsReport.EightCharConvert(meas.MeasDate);
			else
				path = base.GenBaseFileName(pretext);
			return path;
		}


        public override void StartReportGeneration(Measurement m, string pretext, char separator, string suffixoverride = null)  // space char as separator forces text report generation
        {
            base.StartReportGeneration(m, pretext, separator, suffixoverride);

            m.ResultsFiles.Add(new ResultFile(t.FullFilePath));  // save the full file path on this list for later
        }

        public void GenerateReport(Measurement m)
        {
            if (N.App.Opstate.IsAbortRequested) // handle stop, cancel, abort here, for stop does it roll back all report gen from here?
                return;
            IEnumerator iter = m.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();

            while (iter.MoveNext())
            {
                MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
                INCCResult ir = m.INCCAnalysisResults[moskey];
                // create one results for each SR key
                StartReportGeneration(m, m.MeasOption.PrintName() + " INCC " +
                    (m.INCCAnalysisResults.Count > 1 ? "[" + moskey.MultiplicityParams.ShortName() + "] " : ""), // add an identifying signature to each file name if more than one active virtual SR
                    ' ',(N.App.AppContext.AssayTypeSuffix ? m.MeasOption.INCC5Suffix() : null));  // make these text files
                //Detector det = meas.Detectors.GetIt(moskey.SRParams); 
                // now assuming only one detector on the list, so can use [0], the mos keys have specific values for virtual SR counting, overiding the detector 
                Detector det = meas.Detector;
                try
                {
                    sections.Add(ConstructReportSection(INCCReportSection.Header, det));
                    sections.Add(ConstructReportSection(INCCReportSection.Context, det));
                    sections.Add(ConstructReportSection(INCCReportSection.Isotopics, det));
                    sections.Add(ConstructReportSection(INCCReportSection.ShiftRegister, det, moskey));
                    sections.Add(ConstructReportSection(INCCReportSection.Adjustments, det));
                    sections.Add(ConstructReportSection(INCCReportSection.CycleSummary, det, moskey));
                    sections.Add(ConstructReportSection(INCCReportSection.Messages, det, moskey));

                    sections.Add(ConstructReportSection(INCCReportSection.SummedRawData, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.SummedRA, moskey, ir, det));

                    sections.Add(ConstructReportSection(INCCReportSection.MassResults, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.MethodResultsAndParams, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.RawCycles, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.DTCRateCycles, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.MultiplicityDistributions, moskey, ir, det));
                    sections.Add(ConstructReportSection(INCCReportSection.Reference, det));
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
            }
        }

        protected override void FinishReportGeneration()
        {
            base.FinishReportGeneration();
            // capture the Row array before exit
            INCCResultsReports.Add(t.lines);
        }

        public void GenerateInitialReportContent(Measurement m)
        {
            foreach (Multiplicity mult in (m.AnalysisParams.GetAllMults()))
            {
                // create one results for each SR key
                StartReportContent(m);
                Detector det = meas.Detector;
                try
                {
                    sections.Add(ConstructReportSection(INCCReportSection.Header, det));
                    sections.Add(ConstructReportSection(INCCReportSection.Context, det));
                    sections.Add(ConstructReportSection(INCCReportSection.Isotopics, det));
                    sections.Add(ConstructReportSectionI(INCCReportSection.ShiftRegister, det, mult));
                    sections.Add(ConstructReportSection(INCCReportSection.Adjustments, det));

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

                t.CreateReport(0);
                INCCResultsReports.Add(t.lines);
            }
        }

        void StartReportContent(Measurement m)
        {
            PrepForReportGeneration(m, ' ');
        }
    
        /* INCC5 file naming scheme
			YMDHMMSS
			Y = last digit of the year
			M = month (0-9, A-C)
			D = day (0-9, A-V)
			H = hour (A-X)
			MM = minutes (00-59)
			SS = seconds (00-59)
		*/
        static public string EightCharConvert(DateTimeOffset dto)
		{
			Char[] table = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
			string y = dto.ToString("yy");
			char Y = y[y.Length-1];
			string M = string.Format("{0:X1}", dto.Month);
			char D = table[dto.Day];
			char H = table[dto.Hour + 10];
			string s = Y + M + D + H + dto.Minute.ToString("00") + dto.Second.ToString("00");

			return s;
		}

		static public bool TryEightCharConvert(string s, int decade, out DateTimeOffset result)
		{
			bool ret = false;
			result = new DateTimeOffset();
			if (s.Length < 8)
				return ret;
			try
			{
				s = s.Substring(0, 8);
				int Y = s[0] - 48;
				int M = s[1] - 65 + 10;  // try for the letter first
				if (M < 0)   // oh it was a number, adjust accordingly
					M = s[1] - 48;
				int D = s[2] - 65 + 10;  // try for the letter first
				if (D < 0)   // oh it was a number, adjust accordingly
					D = s[2] - 48;
				int H = s[3] - 65;
				int MM = 0;
				int SS = 0;
				int.TryParse(s.Substring(4, 2), out MM);
				int.TryParse(s.Substring(6, 2), out SS);
				ret = true;
				result = new DateTimeOffset(decade + Y, M, D, H, MM, SS, new TimeSpan());
			} catch (Exception)
			{ }
			return ret;
		}

		static public int NowDecade { get {return  DateTimeOffset.Now.Year - (DateTimeOffset.Now.Year % 10); } }
    }

    public class INCCStyleSection : Section
    {

        public enum ReportSection
        {
            Standard, Summary, MethodResults, MultiColumn
        }

        public INCCStyleSection(System.Type column, short prelines, ReportSection sec = ReportSection.Standard)
            : base(column, prelines)
        {
            this.sectiontype = sec;
            scientific = "{0,15:e}";
            SetFPCurrentFormatPrecision(N.App.AppContext.FPPrecision);
        }
        ReportSection sectiontype;
        string scientific;
        string fixedpt;
        string format;
        public ushort FPFormatPrecision;
        public enum NStyle { Exponent, Fixed }

        public void SetFloatingPointFormat(NStyle ff)
        {
            if (ff == NStyle.Fixed)
                format = fixedpt;
            else
                format = scientific;
        }
        public void SetFPFormatPrecision(ushort val)
        {
            FPFormatPrecision = val;
            fixedpt = String.Format("{{0,10:F{0}}}", FPFormatPrecision);
        }
        public void SetFPCurrentFormatPrecision(ushort val)
        {
            FPFormatPrecision = val;
            fixedpt = String.Format("{{0,10:F{0}}}", FPFormatPrecision);
            SetFloatingPointFormat(NStyle.Fixed);
        }
        void AddPaddedLabel(Row r, string label)
        {
            string s = String.Empty;

            switch (sectiontype) // INCC5 spacing hard-coded and enforced here
            {
                case ReportSection.Standard:
                    s = String.Format("{0,29}", label);
                    break;
                case ReportSection.MethodResults:
                    s = String.Format("{0,41}", label);
                    break;
                case ReportSection.Summary:
                    s = String.Format("{0,39}", label);
                    break;
                case ReportSection.MultiColumn:
                    s = String.Format("{0,4} ", label);
                    break;
            }
            r.Add(0, s);
        }
        string FormatFloatingPt(double v)
        {
            return String.Format(format, v);
        }
        string FormatInteger(Int32 v)
        {
            return String.Format("{0,10}", v);
        }
        public void AddTwo(string label, string value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, value);
            Add(r);
        }
        public void AddTwo(string label, double value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatFloatingPt(value));
            Add(r);
        }
        public void AddTwoWithTag(string label, double value, string tag)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatFloatingPt(value));
            r.Add(2, tag);
            Add(r);
        }
        public void AddNumericRow(string label, Tuple value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatFloatingPt(value.v));
            r.Add(2, " +-");
            r.Add(3, FormatFloatingPt(value.err));
            Add(r);
        }
        public void AddNumericRowWithExtra(string label, Tuple value, double extra)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatFloatingPt(value.v));
            r.Add(2, " +-");
            r.Add(3, FormatFloatingPt(value.err));
            r.Add(4, FormatFloatingPt(extra));
            Add(r);
        }
        public void AddIntegerRow(string label, Int32 value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatInteger(value));
            Add(r);
        }
        public void AddNumericRow(string label, double value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, FormatFloatingPt(value));
            Add(r);
        }
        public void AddDualNumericRow(string label, Tuple value, Tuple value2)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);

            r.Add(1, FormatFloatingPt(value.v));
            r.Add(2, " +-");
            r.Add(3, FormatFloatingPt(value.err));
            r.Add(4, "     ");
            r.Add(5, FormatFloatingPt(value2.v));
            r.Add(6, " +-");
            r.Add(7, FormatFloatingPt(value2.err));
            Add(r);
        }
        public void AddDateTimeRow(string label, DateTime value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, value.ToString(" yy.MM.dd"));
            r.Add(2, "     ");
            r.Add(3, value.ToString("HH:mm:ss"));
            Add(r);
        }
        public void AddDateTimeRow(string label, DateTimeOffset value)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);
            r.Add(1, value.ToString(" yy.MM.dd"));
            r.Add(2, "     ");
            r.Add(3, value.ToString("HH:mm:ss"));
            Add(r);
        }
        public void AddDualDateOnlyRow(string label, DateTime value, DateTime value2)
        {
            Row r = new Row();
            AddPaddedLabel(r, label);

            r.Add(1, value.ToString(" yy.MM.dd"));
            r.Add(2, "     ");
            r.Add(3, "     ");
            r.Add(4, "     ");
            r.Add(5, value2.ToString(" yy.MM.dd"));
            Add(r);
        }
        public void AddColumnRowHeader(string[] values, int[] widths)
        {
            Row r = new Row();
            for (int i = 0; i < values.Length; i++)
            {
                string f = String.Format("{{0,{0}}}", widths[i]);
                r.Add(i, String.Format(f, values[i]));
            }
            Add(r);
        }
        public void AddCycleColumnRow(int num, double[] values, string qc, int[] widths)
        {
            Row r = new Row();
            string f = String.Format("{{0,{0}}}", widths[0]);
            r.Add(0, String.Format(f, num));
            for (int i = 0; i < values.Length; i++)
            {
                f = String.Format("{{0,{0}}}", widths[i + 1]);
                r.Add(i + 1, String.Format(f, FormatFloatingPt(values[i])));
            }
            f = String.Format("{{0,{0}}}", widths[widths.Length - 1]);
            r.Add(widths.Length, String.Format(f, qc));
            Add(r);
        }
        public void AddCycleColumnRow(int num, ulong[] values, string qc, int[] widths)
        {
            Row r = new Row();
            string f = String.Format("{{0,{0}}}", widths[0]);
            r.Add(0, String.Format(f, num));
            for (int i = 0; i < values.Length; i++)
            {
                f = String.Format("{{0,{0}}}", widths[i + 1]);
                r.Add(i + 1, String.Format(f, values[i]));
            }
            f = String.Format("{{0,{0}}}", widths[widths.Length - 1]);
            r.Add(widths.Length, String.Format(f, qc));
            Add(r);
        }
        public void AddColumnRow(ulong[] values, int[] widths)
        {
            Row r = new Row();
            for (int i = 0; i < values.Length; i++)
            {
                string f = String.Format("{{0,{0}}}", widths[i]);
                r.Add(i, String.Format(f, values[i]));
            }
            Add(r);
        }
        public void AddHeader(string headtext)
        {
            Row rh = new Row(); rh.Add(0, headtext);  // section header text
            Add(rh);
            Add(new Row()); // blank line
        }
    }

    public class INCCTestDataStyle : INCCStyleSection
    {

        public INCCTestDataStyle(System.Type column, short prelines, ReportSection sec = ReportSection.Standard)
            : base(column, prelines, sec)
        {
        }

        public void AddOne(int value)
        {
            Row r = new Row();
            r.Add(0, value.ToString());
            Add(r);
        }
    }

    public class TestDataFile : SimpleReport
    {

        protected enum INCCTestDataSection
        {
            CycleSummary, CyclesWithMultiplicityDistributions
        }


        public List<List<string>> INCCTestDataFiles;

        public TestDataFile(NCCReporter.LMLoggers.LognLM ctrllog)
            : base(ctrllog)
        {
            INCCTestDataFiles = new List<List<string>>();
        }


        protected Section ConstructReportSection(INCCTestDataSection section, MeasOptionSelector moskey,  Detector det)
        {
            INCCTestDataStyle sec = null;
            try
            {
                switch (section)
                {

                    case INCCTestDataSection.CyclesWithMultiplicityDistributions:
                        sec = new INCCTestDataStyle(null, 0, INCCStyleSection.ReportSection.MultiColumn);
                        //Singles 1st Scaler 2nd Scaler Reals + Accidentals Accidentals
                        int[] crdwidths = new int[] {10, 10, 10, 10, 10 };
                        int[] srawidths = new int[] { 12, 12 };
                        foreach (Cycle cyc in meas.Cycles)
                        {
                            // if no results on the cycle, these map indexers throw
                            if (cyc.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity)) > 0)                            // if no results on the cycle, these map indexers throw
                            {
                                MultiplicityCountingRes mcr = (MultiplicityCountingRes)cyc.CountingAnalysisResults[moskey.MultiplicityParams];
                                sec.AddColumnRow(
                                    new ulong[] { (ulong)mcr.Totals, (ulong)mcr.Scaler1.v, (ulong)mcr.Scaler2.v, (ulong)mcr.RASum, (ulong)mcr.ASum },
                                     crdwidths);
                                int minbin, maxbin;
                                minbin = Math.Min(mcr.RAMult.Length, mcr.NormedAMult.Length);
                                maxbin = Math.Max(mcr.RAMult.Length, mcr.NormedAMult.Length);
                                sec.AddOne(maxbin);
                                for (int i = 0; i < minbin; i++)
                                    sec.AddColumnRow(new ulong[] { mcr.RAMult[i], mcr.NormedAMult[i] }, srawidths);
                                for (int i = minbin; i < maxbin; i++)  // check for uneven column
                                {
                                    ulong[] potential = new ulong[2];
                                    if (i < mcr.RAMult.Length)
                                        potential[0] = mcr.RAMult[i];
                                    if (i < mcr.NormedAMult.Length)
                                        potential[1] = mcr.NormedAMult[i];
                                    sec.AddColumnRow(potential, srawidths);
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        protected Section ConstructReportSection(INCCTestDataSection section, Detector det, MeasOptionSelector moskey = null)
        {
            INCCTestDataStyle sec = null;
            try
            {
                switch (section)
                {
                    case INCCTestDataSection.CycleSummary:
                        sec = new INCCTestDataStyle(null, 0);
                        sec.AddOne( (Int32)meas.Cycles.GetValidCycleCountForThisKey(moskey.MultiplicityParams)); // could also use CycleList length but CycleList can be longer when a reanalysis occurs and the analysis processing stops short of the end of the list due to modified termination conditions
                        sec.AddOne( (Int32)(meas.Cycles.Count > 0 ? meas.Cycles[0].TS.TotalSeconds : 0.0));
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            return sec;
        }

        public override void StartReportGeneration(Measurement m, string pretext, char separator, string suffixoverride = null)  // space char as separator forces text report generation
        {
            base.StartReportGeneration(m, pretext, separator, suffixoverride);
        }

        public void GenerateReport(Measurement m)
        {
            if (N.App.Opstate.IsAbortRequested) // handle stop, cancel, abort here, for stop does it roll back all report gen from here?
                return;
            IEnumerator iter = m.INCCAnalysisResults.GetMeasSelectorResultsEnumerator();
            while (iter.MoveNext())
            {
                MeasOptionSelector moskey = (MeasOptionSelector)iter.Current;
                // create one results for each SR key
                StartReportGeneration(m, m.MeasOption.PrintName() + " INCC " +
                    (m.INCCAnalysisResults.Count > 1 ? "[" + moskey.MultiplicityParams.ShortName() + "] " : ""), // add an identifying signature to each file name if more than one active virtual SR
                    ' ', "dat");  // make these text files with .dat suffix

                //Detector det = meas.Detectors.GetIt(moskey.SRParams);
                // now assuming only one detector on the list, so can use [0]
                Detector det = meas.Detector;
                try
                {
                    sections.Add(ConstructReportSection(INCCTestDataSection.CycleSummary, det, moskey));

                    sections.Add(ConstructReportSection(INCCTestDataSection.CyclesWithMultiplicityDistributions, moskey, det));
 
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
            }
        }

        protected override void FinishReportGeneration()
        {
            base.FinishReportGeneration();
            // capture the Row array before exit
            INCCTestDataFiles.Add(t.lines);
        }
    }
}




