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
using System.Windows.Forms;
using AnalysisDefs;
using NCC;

namespace NewUI
{
	using System.Collections;
	using System.Collections.Generic;
	using N = CentralizedState;

	public partial class IDDEndingComment : Form
    {

		AcquireParameters acq;

        public IDDEndingComment()
        {
            InitializeComponent();
            acq = N.App.Opstate.Measurement.AcquireState;
            Text += " for " + acq.detector_id;
			FieldFiller();
		}

        // Based on the measurement type and result, show a bunch of stuff
        void FieldFiller()
        {
            Measurement m = N.App.Opstate.Measurement;
            int rep = m.CountingAnalysisResults.GetResultsCount(typeof(Multiplicity));
            int i = 0;
            IEnumerator iter = m.CountingAnalysisResults.GetMultiplicityEnumerator();
            while (iter.MoveNext())
            {
                i++;
                string augmenter = (rep > 1 ? " " + i.ToString() : ""); // use mkey indicator here, not just this indexer
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                INCCResult r;
                MeasOptionSelector moskey = new MeasOptionSelector(m.MeasOption, mkey);
                bool found = m.INCCAnalysisResults.TryGetValue(moskey, out r);  // APluralityOfMultiplicityAnalyzers:
                switch (m.MeasOption)
                {
                    case AssaySelector.MeasurementOption.rates:
                    case AssaySelector.MeasurementOption.background:
                        //if (passive)
                        listView1.Items.Add(new ListViewItem(new string[] { "Passive rates" + augmenter }));  
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format(" Singles: {0,13:F3} +- {1,9:F3}", r.DeadtimeCorrectedSinglesRate.v, r.DeadtimeCorrectedSinglesRate.err) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format(" Doubles: {0,13:F3} +- {1,9:F3}", r.DeadtimeCorrectedDoublesRate.v, r.DeadtimeCorrectedDoublesRate.err) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format(" Triples: {0,13:F3} +- {1,9:F3}", r.DeadtimeCorrectedTriplesRate.v, r.DeadtimeCorrectedTriplesRate.err) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Scaler 1: {0,13:F3} +- {1,9:F3}", r.Scaler1Rate.v, r.Scaler1Rate.err) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Scaler 2: {0,13:F3} +- {1,9:F3}", r.Scaler2Rate.v, r.Scaler2Rate.err) }));
                        //if (active)
                        break;
                    case AssaySelector.MeasurementOption.initial:
                        INCCResults.results_init_src_rec isr = (INCCResults.results_init_src_rec)r;
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("                 Source id: {0}", isr.init_src_id) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Initial source measurement: {0}", (isr.pass ? "passed" : "failed")) }));
                        break;
                    case AssaySelector.MeasurementOption.normalization:
                        INCCResults.results_bias_rec br = (INCCResults.results_bias_rec)r;
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Normalization results for reference source: {0}", br.sourceId) }));
                        if (br.mode == NormTest.Cf252Doubles)
                        {
                            listView1.Items.Add(new ListViewItem(new string[] {
                            string.Format("Normalization doubles rate expected/measured: {0,13:F3} +- {1,9:F3}",
                                br.biasDblsRateExpectMeas.v, br.biasDblsRateExpectMeas.err) }));
                        }
                        else if (br.mode == NormTest.Cf252Singles)
                        {
                            listView1.Items.Add(new ListViewItem(new string[] {
                            string.Format("Normalization singles rate expected/measured: {0,13:F3} +- {1,9:F3}",
                                br.biasDblsRateExpectMeas.v, br.biasDblsRateExpectMeas.err) }));
                        }
                        else
                        {
                            listView1.Items.Add(new ListViewItem(new string[] {
                            string.Format("Normalization singles rate expected/measured: {0,13:F3} +- {1,9:F3}",
                                br.biasSnglsRateExpectMeas.v, br.biasSnglsRateExpectMeas.err) }));
                        }
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Normalization test {0}", (br.pass ? "passed" : "failed")) }));
                        break;
                    case AssaySelector.MeasurementOption.precision:
                        INCCResults.results_precision_rec pr = (INCCResults.results_precision_rec)r;
                        listView1.Items.Add(new ListViewItem(new string[] { "Precision results" + augmenter }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Chi-square lower limit: {0,13:F3}", pr.chiSqLowerLimit) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Chi-square upper limit: {0,13:F3}", pr.chiSqUpperLimit) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("       Sample variance: {0,13:F3}", pr.precSampleVar) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("  Theoretical variance: {0,13:F3}", pr.precTheoreticalVar) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("            Chi-square: {0,13:F3}", pr.precChiSq) }));
                        listView1.Items.Add(new ListViewItem(new string[] {
                        string.Format("Precision test {0}.", (pr.pass ? "passed" : "failed")) }));
                        break;
                    case AssaySelector.MeasurementOption.calibration:
                        INCCMethodResults imrs;
                        bool ok = m.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrs);
                        if (ok && imrs.Count > 0) // should be true for verification and calibration
                        {
                            // we've got a distinct detector id and material type on the methods, so that is the indexer here
                            Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrs[m.INCCAnalysisState.Methods.selector];

                            // now get an enumerator over the map of method results
                            Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
                            while (ai.MoveNext())
                            {
                                INCCMethodResult imr = ai.Current.Value;
                                if (imr is INCCMethodResults.results_cal_curve_rec)
                                {
                                    listView1.Items.Add(new ListViewItem(new string[] { "Passive calibration curve results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("       Pu mass: {0,13:F3}", ((INCCMethodResults.results_cal_curve_rec)imr).pu_mass.v) }));
                                }
                                else if (imr is INCCMethodResults.results_known_alpha_rec)
                                {
                                    listView1.Items.Add(new ListViewItem(new string[] { "Known alpha results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("Multiplication: {0,13:F3}", ((INCCMethodResults.results_known_alpha_rec)imr).mult) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("         Alpha: {0,13:F3}", ((INCCMethodResults.results_known_alpha_rec)imr).alphaK) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("       Pu mass: {0,13:F3}", ((INCCMethodResults.results_known_alpha_rec)imr).pu_mass.v) }));
                                }
                                else if (imr is INCCMethodResults.results_add_a_source_rec)
                                {
                                    listView1.Items.Add(new ListViewItem(new string[] { "Add-a-source results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("       Pu mass: {0,13:F3}", ((INCCMethodResults.results_add_a_source_rec)imr).pu_mass.v) }));
                                }
                                else if (imr is INCCMethodResults.results_active_rec)
                                {
                                    listView1.Items.Add(new ListViewItem(new string[] { "Active calibration curve results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("      U235 mass: {0,13:F3}", ((INCCMethodResults.results_active_rec)imr).u235_mass.v) }));
                                }
                            }
                        }
                        break;
                    case AssaySelector.MeasurementOption.verification:
                    case AssaySelector.MeasurementOption.holdup:
                        INCCMethodResults imrvs;
                        bool okv = m.INCCAnalysisResults.TryGetINCCResults(moskey.MultiplicityParams, out imrvs);
                        if (okv && imrvs.Count > 0) // should be true for verification and calibration
                        {
                            // we've got a distinct detector id and material type on the methods, so that is the indexer here
                            Dictionary<AnalysisMethod, INCCMethodResult> amimr = imrvs[m.INCCAnalysisState.Methods.selector];

                            // now get an enumerator over the map of method results
                            Dictionary<AnalysisMethod, INCCMethodResult>.Enumerator ai = amimr.GetEnumerator();
                            while (ai.MoveNext())
                            {
                                INCCMethodResult imr = ai.Current.Value;
                                if (imr is INCCMethodResults.results_cal_curve_rec)
                                {
                                    INCCMethodResults.results_cal_curve_rec d = (INCCMethodResults.results_cal_curve_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Passive calibration curve results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu_mass.v, d.pu_mass.err) }));
                                    if (d.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("        Declared Pu mass: {0,13:F3}", d.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_pu_mass.v, d.dcl_minus_asy_pu_mass.err, d.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_known_alpha_rec)
                                {
                                    INCCMethodResults.results_known_alpha_rec d = (INCCMethodResults.results_known_alpha_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Known alpha results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("          Multiplication: {0,13:F3}", d.mult) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                   Alpha: {0,13:F3}", d.alphaK) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu_mass.v, d.pu_mass.err) }));
                                    if (d.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("        Declared Pu mass: {0,13:F3}", d.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_pu_mass.v, d.dcl_minus_asy_pu_mass.err, d.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_known_m_rec)
                                {
                                    INCCMethodResults.results_known_m_rec d = (INCCMethodResults.results_known_m_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Known M results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("          Multiplication: {0,13:F3}", d.mult) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                   Alpha: {0,13:F3}", d.alpha) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu_mass.v, d.pu_mass.err) }));
                                    if (d.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("        Declared Pu mass: {0,13:F3}", d.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_pu_mass.v, d.dcl_minus_asy_pu_mass.err, d.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_multiplicity_rec)
                                {
                                    INCCMethodResults.results_multiplicity_rec d = (INCCMethodResults.results_multiplicity_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Passive multiplicity results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("          Multiplication: {0,13:F3} +- {1,9:F3}", d.mult.v, d.mult.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                   Alpha: {0,13:F3} +- {1,9:F3}", d.alphaK.v, d.alphaK.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("       Correction factor: {0,13:F3}", d.corr_factor.v, d.corr_factor.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu_mass.v, d.pu_mass.err) }));
                                    if (d.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("        Declared Pu mass: {0,13:F3}", d.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_pu_mass.v, d.dcl_minus_asy_pu_mass.err, d.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_add_a_source_rec)
                                {
                                    INCCMethodResults.results_add_a_source_rec d = (INCCMethodResults.results_add_a_source_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Add-a-source results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                   Delta: {0,13:F3} +- {1,9:F3}", d.delta.v, d.delta.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("       Correction factor: {0,13:F3}", d.corr_factor.v, d.corr_factor.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu_mass.v, d.pu_mass.err) }));
                                    if (d.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("        Declared Pu mass: {0,13:F3}", d.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_pu_mass.v, d.dcl_minus_asy_pu_mass.err, d.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_curium_ratio_rec)
                                {
                                    INCCMethodResults.results_curium_ratio_rec d = (INCCMethodResults.results_curium_ratio_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Curium ratio results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Cm mass: {0,13:F3}  +- {1,9:F3}", d.cm_mass.v, d.cm_mass.err) }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.pu.pu_mass.v, d.pu.pu_mass.err) }));
                                    if (d.pu.dcl_pu_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared Pu mass: {0,13:F3}", d.pu.dcl_pu_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.pu.dcl_minus_asy_pu_mass.v, d.pu.dcl_minus_asy_pu_mass.err, d.pu.dcl_minus_asy_pu_mass_pct) }));
                                    }
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                  U mass: {0,13:F3} +- {1,9:F3}", d.u.mass.v, d.u.mass.err) }));
                                    if (d.u.dcl_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("         Declared U mass: {0,13:F3}", d.u.dcl_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format(" Declared - assay U mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.u.dcl_minus_asy_mass.v, d.u.dcl_minus_asy_mass.err, d.u.dcl_minus_asy_mass_pct) }));
                                    }
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("      Declared U235 mass: {0,13:F3} +- {1,9:F3}", d.u235.mass.v, d.u235.mass.err) }));
                                    if (d.u235.dcl_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("      Declared U235 mass: {0,13:F3}", d.u235.dcl_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("  Decl - assay U235 mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.u235.dcl_minus_asy_mass.v, d.u235.dcl_minus_asy_mass.err, d.u235.dcl_minus_asy_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_truncated_mult_rec)
                                {
                                    INCCMethodResults.results_truncated_mult_rec d = (INCCMethodResults.results_truncated_mult_rec)imr;
                                    if (d.methodParams.known_eff)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] { "Known efficiency truncated multiplicity results" + augmenter }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.k.pu_mass.v, d.k.pu_mass.err) }));
                                        if (d.k.dcl_pu_mass > 0)
                                        {
                                            listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared Pu mass: {0,13:F3}", d.k.dcl_pu_mass) }));
                                            listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.k.dcl_minus_asy_pu_mass.v, d.k.dcl_minus_asy_pu_mass.err, d.k.dcl_minus_asy_pu_mass_pct) }));
                                        }
                                    }
                                    if (d.methodParams.solve_eff)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] { "Solve efficiency truncated multiplicity results" + augmenter }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                        string.Format("                 Pu mass: {0,13:F3} +- {1,9:F3}", d.s.pu_mass.v, d.s.pu_mass.err) }));
                                        if (d.s.dcl_pu_mass > 0)
                                        {
                                            listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared Pu mass: {0,13:F3}", d.s.dcl_pu_mass) }));
                                            listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay Pu mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.s.dcl_minus_asy_pu_mass.v, d.s.dcl_minus_asy_pu_mass.err, d.s.dcl_minus_asy_pu_mass_pct) }));
                                        }
                                    }
                                }
                                else if (imr is INCCMethodResults.results_active_rec)
                                {
                                    INCCMethodResults.results_active_rec d = (INCCMethodResults.results_active_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Active calibration curve results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("                 U235 mass: {0,13:F3} +- {1,9:F3}", d.u235_mass.v,d.u235_mass.err) }));
                                    if (d.dcl_u235_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared U235 mass: {0,13:F3}", d.dcl_u235_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay U235 mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_u235_mass.v, d.dcl_minus_asy_u235_mass.err, d.dcl_minus_asy_u235_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_active_passive_rec)
                                {
                                    INCCMethodResults.results_active_passive_rec d = (INCCMethodResults.results_active_passive_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Active/passive results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("                 U235 mass: {0,13:F3} +- {1,9:F3}", d.u235_mass.v,d.u235_mass.err) }));
                                    if (d.dcl_u235_mass > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared U235 mass: {0,13:F3}", d.dcl_u235_mass) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay U235 mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_u235_mass.v, d.dcl_minus_asy_u235_mass.err, d.dcl_minus_asy_u235_mass_pct) }));
                                    }
                                }
                                else if (imr is INCCMethodResults.results_collar_rec)
                                {
                                    INCCMethodResults.results_collar_rec d = (INCCMethodResults.results_collar_rec)imr;
                                    listView1.Items.Add(new ListViewItem(new string[] { "Collar results" + augmenter }));
                                    listView1.Items.Add(new ListViewItem(new string[] {
                                    string.Format("                 U235 mass: {0,13:F3} +- {1,9:F3}", d.u235_mass.v,d.u235_mass.err) }));
                                    if (d.dcl_total_u235.v > 0)
                                    {
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("        Declared U235 mass: {0,13:F3} +- {1,9:F3}", d.dcl_total_u235.v, d.dcl_total_u235.err) }));
                                        listView1.Items.Add(new ListViewItem(new string[] {
                                            string.Format("Declared - assay U235 mass: {0,13:F3} +- {1,9:F3} or {2,6:F2}%",
                                            d.dcl_minus_asy_u235_mass.v, d.dcl_minus_asy_u235_mass.err, d.dcl_minus_asy_u235_mass_pct) }));
                                    }
                                }
                            }
                        }
                        break;
                    case AssaySelector.MeasurementOption.unspecified:
                        listView1.Items.Add(new ListViewItem(new string[] { "List mode results" + augmenter }));
                        listView1.Items.Add(new ListViewItem(new string[] { "TODO " })); // todo: fill in with something useful for LM
                        break;
                }
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
			if (acq.modified)
			{
				Measurement m = N.App.Opstate.Measurement;
				DB.Results dbres = new DB.Results();
				dbres.UpdateEndingComment(m.MeasurementId.UniqueId, acq.ending_comment_str);
				// save the comment in the existing measurement result_rec, in the database, it is a surgical insertion at this point because the process is complete
			}
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

		private void EndingCommentTextBox_Leave(object sender, EventArgs e)
		{
            if ((((TextBox)sender).Text) != acq.ending_comment_str)
            {
                acq.modified = true;
                acq.ending_comment_str = ((TextBox)sender).Text;
            }
		}
	}
}
