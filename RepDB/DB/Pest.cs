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
using System.Data;
using System.Text.RegularExpressions;
using NCCConfig;
using NCCReporter;

namespace DB
{

    public enum Pieces { IOCodes, InvChangeCodes, 
        Measurements, HVResults,
        Detectors, TestParams, NormParams, BackgroundParams, AASSetupParams, Facilities, MBAs, Materials, Items, Isotopics, DetectorTypes, CollarItems,
        Strata, StrataWithAssoc, AcquireParams, UnattendedParams, CmPuRatioParams, CollarParams, CollarDetectorParams, CollarK5Params,
        AnalysisMethodSpecifiers,
        CountingAnalyzers, AppContext, LMParams, LMMultParams, HVParams, Results, CompositeIsotopics, HoldupConfigs, PoisonRods
    }

    public class Persistence
    {
<<<<<<< HEAD
=======
        //public LMLoggers.LognLM logger;
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
        public DBConfig cfg;

        public Persistence(LMLoggers.LognLM logger, DBConfig cfg)
        {
            this.cfg = cfg;
<<<<<<< HEAD
=======
            //this.logger = logger;
>>>>>>> c355399f558aa7a1290b63f16147ca7a85a453b0
            DBMain.pest = this;
        }

        public DataTableReader GetADataTableReader(Pieces p, String did = null)
        {
            return new DataTableReader(GetACollection(p, did));
        }

        // can use IList interface on this
        public DataTable GetACollection(Pieces p, String did = null)
        {
            DataTable dt = new DataTable();

            try
            {
                switch (p)
                {
                    default:
                        break;

                    case Pieces.HVParams:
                        HVParams hv = new HVParams();
                        dt = hv.Get(did);
                        break;
                    case Pieces.HVResults:
                        HVPlateauResults hvr = new HVPlateauResults();
                        dt = hvr.AllHVPlateauResults(did);
                        break;
                    case Pieces.Measurements:
                        Measurements ms = new Measurements();
                        dt = ms.AllMeasurements(did);
                        break;
                    case Pieces.CountingAnalyzers:
                        CountingAnalysisParameters cap = new CountingAnalysisParameters();
                        dt = cap.AnalyzerParamsForDetector(did);
                        break;

                    case Pieces.AnalysisMethodSpecifiers:
                        using(AnalysisMethodSpecifiers am = new AnalysisMethodSpecifiers())
                        {
                            dt = am.MethodsForDetector(did);
                        }
                        
                        break;

                    case Pieces.Detectors:
                        Detectors clsD = new Detectors();
                        dt = clsD.getDetectors(true);
                        break;
                    case Pieces.LMParams:
                        LMNetCommParams blue = new LMNetCommParams();
                        dt = blue.Get(did);
                        break;
                    case Pieces.LMMultParams:
                        LMMultiplicityParams purple = new LMMultiplicityParams();
                        dt = purple.Get(did);
                        break;
                    case Pieces.DetectorTypes:
                        Descriptors clsDT = new Descriptors("detector_types");
                        dt = clsDT.getDescs();
                        break;
                    case Pieces.Materials:
                        Descriptors clsMtl = new Descriptors("material_types");
                        dt = clsMtl.getDescs();
                        break;
                    case Pieces.TestParams:
                        TestParams tp = new TestParams();
                        dt = tp.Get();
                        break;
                    case Pieces.NormParams:
                        NormParams np = new NormParams();
                        dt = np.Get(did);
                        break;
                    case Pieces.AASSetupParams:
                        AASSetupParams aass = new AASSetupParams();
                        dt = aass.Get(did);
                        break;
                    case Pieces.BackgroundParams:
                        BackgroundParams clsB = new BackgroundParams();
                        TruncatedBackgroundParams clsTB = new TruncatedBackgroundParams();
                        dt = clsB.Get(did);
                        DataTable dt2 = clsTB.Get(did);
                        dt.Merge(dt2);
                        break;  // NEXT: caution, should use a select/join in the DB layer, instead of this datatable merge
                    case Pieces.Facilities:
                        Descriptors clsF = new Descriptors("facility_names");
                        dt = clsF.getDescs();
                        break;
                    case Pieces.MBAs:
                        Descriptors MBA = new Descriptors(p.ToString());
                        dt = MBA.getDescs();
                        break;
                    case Pieces.Items:
                        Items clsI = new Items();
                        dt = clsI.getItems();
                        break;
                    case Pieces.CollarItems:
                        CollarItems clsCI = new CollarItems();
                        dt = clsCI.getItems();
                        break;
                    case Pieces.Isotopics:
                        Isotopics clsIs = new Isotopics();
                        dt = clsIs.getIsotopics();
                        break;
                    case Pieces.Strata:
                        Strata s = new Strata();
                        dt = s.Get();
                        break;
                    case Pieces.StrataWithAssoc:
                        Strata ss = new Strata();
                        dt = ss.GetAssociations(did);
                        break;
                    case Pieces.AcquireParams:
                        AcquireParams aq = new AcquireParams();
                        dt = aq.Get(did);
                        break;
                    case Pieces.IOCodes:
                        Descriptors ioc = new Descriptors("io_code");
                        dt = ioc.getDescs();
                        break;
                    case Pieces.InvChangeCodes:
                        Descriptors icc = new Descriptors("inventory_change_code");
                        dt = icc.getDescs();    
                        break;
                    case Pieces.UnattendedParams:
                        UnattendParams u = new UnattendParams();
                        dt = u.Get(did);
                        break;
                    case Pieces.CmPuRatioParams:
                        cm_pu_ratio_rec cpu = new cm_pu_ratio_rec();
                        dt = cpu.Get();
                        break;
                    case Pieces.CollarParams:
                        collar_combined_rec combine = new collar_combined_rec();
                        dt = combine.GetCollar();
                        break;
                    case Pieces.CollarDetectorParams:
                        combine = new collar_combined_rec();
                        dt = combine.GetCollarDet();
                        break;
                    case Pieces.CollarK5Params:
                        combine = new collar_combined_rec();
                        dt = combine.GetK5();
                        break;
                    case Pieces.Results:
                        Results rr = new Results();
                        dt = rr.AllResults(did);
                        break;
                    case Pieces.CompositeIsotopics:
                        CompositeIsotopics clsCIs = new CompositeIsotopics();
                        dt = clsCIs.getCompositeIsotopics();
                        break;
                    case Pieces.HoldupConfigs:
                        holdup_config_rec hc = new holdup_config_rec();
                        dt = hc.Get();
                        break;
                    case Pieces.PoisonRods:
                        poison_rod_type_rec prt = new poison_rod_type_rec();
                        dt = prt.Get();
                        break;
                }
            }
            catch (Exception caught)
            {
                DBMain.AltLog(LogLevels.Warning, 70191, "Get Collection  '" + caught.Message + "' ");
            }
            return dt;
        }

		public bool IsItThere
		{
			get
			{
				DB db = new DB(true);
				DataTable dt = db.DBProbe("select name from facility_names");
				if (dt == null)
					IsItThereStr = db.DBErrorStr;
				else
					DBDescStr = db.DBDescStr;
				return dt != null;
			}
		}
		public string IsItThereStr;
		public string DBDescStr;

	}
}
