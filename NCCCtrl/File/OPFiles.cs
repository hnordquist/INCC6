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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AnalysisDefs;
using NCCReporter;
namespace NCCFile
{

    using NC = NCC.CentralizedState;

    /// <summary>
    /// Manager for INCC5 import input files (NCC, NOP, COP)
    /// See Operator Declaration File Format p. 87, 
    /// See Operator Declaration File Format for Curium Ratio Measurements p. 88, 
    /// See Radiation Review Measurement Data File Format p. 93, INCC Software Users Manual, March 29, 2009
    /// </summary>
    public class OPFiles
    {

        public OPFiles()
        {
            Init();
        }

        public void Init()
        {
            mPathToNOPFile = new Dictionary<string, CSVFile>();
            mPathToCOPFile = new Dictionary<string, CSVFile>();
            NOPIsotopics = new List<AnalysisDefs.Isotopics>();
            NOPItemIds = new List<ItemId>();
            COPRatioRecs = new List<INCCAnalysisParams.cm_pu_ratio_rec>();
            yyyymmdd = new Regex("^\\d{4}\\d{2}\\d{2}$");
            mlogger = NCC.CentralizedState.App.Loggers.Logger(LMLoggers.AppSection.Control);
        }

        /// <summary>
        /// Create a list of Isotopics instances from each CSV parsed line set
        /// </summary>
        /// <param name="csv">The field-scanned csv file instance with an array of tokenized lines</param>
        void GenerateIsotopics(CSVFile csv)
        {
            List<Isotopics> l = NOPIsotopics;  // local alias
            foreach (string[] sa in csv.Lines)
            {
                Isotopics i = new Isotopics();
                string s = string.Empty;
                double v = 0, err = 0;
                foreach (NOPCol op in System.Enum.GetValues(typeof(NOPCol)))
                {
                    try
                    {
                        s = string.Empty;
                        s = sa[(int)op];  // might blow here when file was badly created
                        switch (op)
                        {
                            case NOPCol.AmDate:
                                Match amd = yyyymmdd.Match(s);
                                if (amd.Success)
                                    i.am_date = GenFromYYYYMMDD(s);
                                break;
                            case NOPCol.PuDate:
                                Match pud = yyyymmdd.Match(s);
                                if (pud.Success)
                                    i.pu_date = GenFromYYYYMMDD(s);
                                break;
                            case NOPCol.IsoSourceCode:
                                System.Enum.TryParse<Isotopics.SourceCode>(s, out i.source_code);
                                break;
                            case NOPCol.IsoId:
                                i.id = string.Copy(s);
                                break;
                            case NOPCol.Pu238:
                                Double.TryParse(s, out v); i.SetVal(Isotope.pu238, v);
                                break;
                            case NOPCol.Pu239:
                                Double.TryParse(s, out v); i.SetVal(Isotope.pu239, v);
                                break;
                            case NOPCol.Pu240:
                                Double.TryParse(s, out v); i.SetVal(Isotope.pu240, v);
                                break;
                            case NOPCol.Pu241:
                                Double.TryParse(s, out v); i.SetVal(Isotope.pu241, v);
                                break;
                            case NOPCol.Pu242:
                                Double.TryParse(s, out v); i.SetVal(Isotope.pu242, v);
                                break;
                            case NOPCol.Am241:
                                Double.TryParse(s, out v); i.SetVal(Isotope.am241, v);
                                break;
                            case NOPCol.Pu238err:
                                Double.TryParse(s, out err); i.SetError(Isotope.pu238, err);
                                break;
                            case NOPCol.Pu239err:
                                Double.TryParse(s, out err); i.SetError(Isotope.pu239, err);
                                break;
                            case NOPCol.Pu240err:
                                Double.TryParse(s, out err); i.SetError(Isotope.pu240, err);
                                break;
                            case NOPCol.Pu241err:
                                Double.TryParse(s, out err); i.SetError(Isotope.pu241, err);
                                break;
                            case NOPCol.Pu242err:
                                Double.TryParse(s, out err); i.SetError(Isotope.pu242, err);
                                break;
                            case NOPCol.Am241err:
                                Double.TryParse(s, out err); i.SetError(Isotope.am241, err);
                                break;
                            case NOPCol.Norm:
                                // todo: the values in this field are related to COP file usage, bu I don't quite understand this yet, appears it should be looking for "Cf" in this field according to the documentation
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as isotopics element " + op.ToString() + " " + ex.Message);
                    }
                }

                l.Add(i);
            }
        }

        /// <summary>
        /// Create a list of Item Id instances from each CSV parsed line set
        /// </summary>
        /// <param name="csv">The field-scanned csv file instance with an array of tokenized lines</param>
        void GenerateItemIds(CSVFile csv)
        {
            List<ItemId> l = NOPItemIds;  // local alias
            string s = string.Empty;
            foreach (string[] sa in csv.Lines)
            {
                ItemId iid = new ItemId();
                foreach (NOPCol op in System.Enum.GetValues(typeof(NOPCol)))
                {
                    try
                    {
                        s = string.Empty;
                        s = sa[(int)op];  // might blow here when file was badly created
                        switch (op)
                        {
                            case NOPCol.MBA:
                                iid.mba = string.Copy(s);
                                break;
                            case NOPCol.MatType:
                                iid.material = string.Copy(s);
                                break;
                            case NOPCol.ItemId:
                                iid.item = string.Copy(s);
                                break;
                            case NOPCol.StratumId:
                                iid.stratum = string.Copy(s);
                                break;
                            case NOPCol.InvChangeCode:
                                iid.inventoryChangeCode = string.Copy(s);
                                break;
                            case NOPCol.IOCode:
                                iid.IOCode = string.Copy(s);
                                break;
                            case NOPCol.IsoId:
                                iid.isotopics = string.Copy(s);
                                break;
                            case NOPCol.DecPuMassGr:
                                Double.TryParse(s, out iid.declaredMass);
                                break;
                            case NOPCol.AmDate:
                                Match amd = yyyymmdd.Match(s);
                                if (amd.Success)
                                    iid.am_date = GenFromYYYYMMDD(s);
                                break;
                            case NOPCol.PuDate:
                                Match pud = yyyymmdd.Match(s);
                                if (pud.Success)
                                    iid.pu_date = GenFromYYYYMMDD(s);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as item element " + op.ToString() + " " + ex.Message);
                    }
                }
                l.Add(iid);
            }
        }

        /// <summary>
        /// Enum of positional column ids for the iNCC5 NOP file format
        /// </summary>
        enum NOPCol
        {
            Norm, MBA, ItemId, StratumId, InvChangeCode, IOCode, MatType, DecPuMassGr,
            IsoSourceCode, IsoId, Pu238, Pu239, Pu240, Pu241, Pu242, PuDate, Am241, AmDate, Pu238err, Pu239err, Pu240err, Pu241err, Pu242err, Am241err
        }

        /// <summary>
        /// Enum of positional column ids for the INCC5 COP file format
        /// </summary>
        enum COPCol
        {
            ItemId, LabelId, Id, InputBatchId, DecPuMassGr, DecUMassGr, DecU235MassGr,
            CmPuRatio, CmPuErr, CmPuDate, CmURatio, CmUErr, CmUDate
        }

        /// <summary>
        /// Create a list of CmPu ratio record instances from a CSV parsed line set
        /// </summary>
        /// <param name="csv">The field-scanned csv file instance with an array of tokenized lines</param>
        void GenerateCmPuRatioRecs(CSVFile csv)
        {
            List<INCCAnalysisParams.cm_pu_ratio_rec> l = COPRatioRecs;  // local alias

            INCCAnalysisParams.cm_pu_ratio_rec cpr = new INCCAnalysisParams.cm_pu_ratio_rec();
            string s = string.Empty;
            foreach (string[] sa in csv.Lines)
            {
                foreach (COPCol op in System.Enum.GetValues(typeof(COPCol)))
                {
                    try
                    {
                        s = string.Empty;
                        s = sa[(int)op];  // might blow here when file was badly created
                        switch (op)
                        {
                            case COPCol.ItemId:
                                //cpr.cm_id = string.Copy(s);
                                break;
                            case COPCol.DecPuMassGr:
                                //Double.TryParse(s, out cpr.cm_dcl_u235_mass);
                                break;
                            case COPCol.InputBatchId:
                                cpr.cm_input_batch_id = string.Copy(s);
                                break;
                            case COPCol.LabelId:
                                cpr.cm_id_label = string.Copy(s);
                                break;
                            case COPCol.Id:
                                cpr.cm_id = string.Copy(s);
                                break;
                            case COPCol.DecU235MassGr:
                                Double.TryParse(s, out cpr.cm_dcl_u235_mass);
                                break;
                            case COPCol.DecUMassGr:
                                Double.TryParse(s, out cpr.cm_dcl_u_mass);
                                break;
                            case COPCol.CmPuDate:
                                Match cmd = yyyymmdd.Match(s);
                                if (cmd.Success)
                                    cpr.cm_pu_ratio_date = GenFromYYYYMMDD(s);
                                break;
                            case COPCol.CmUDate:
                                Match ud = yyyymmdd.Match(s);
                                if (ud.Success)
                                    cpr.cm_u_ratio_date = GenFromYYYYMMDD(s);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as CmPu ratio element " + op.ToString() + " " + ex.Message);
                    }
                }
                l.Add(cpr);
            }
        }

        /// <summary>
        /// Scan a set of nop and cop files.
        /// Creates a list of Items and a list Isotopics from the nop files.
        /// Creates a list of CmPu Ratio records from the cop files.
        /// </summary>
        /// <param name="files">List of nop and cop files to process</param>
        public void Process(FileList<CSVFile> files)
        {
			if (files == null)
				return;
            foreach (CSVFile csv in files)
            {
                if (0 == string.Compare(csv.ThisSuffix, ".nop", true))
                    mPathToNOPFile.Add(csv.Filename, csv);
                else if (0 == string.Compare(csv.ThisSuffix, ".cop", true))
                    mPathToCOPFile.Add(csv.Filename, csv);
                else continue;
                csv.ProcessFile();  // split lines with scanner, construct istopics, item id and CmPu ratios
                mlogger.TraceEvent(LogLevels.Info, 34100, "Processed " + System.IO.Path.GetFileName(csv.Filename));
            }
            foreach (CSVFile csv in mPathToNOPFile.Values)
            {
                GenerateIsotopics(csv);
                // Isotopic data sets are created for each unique set of isotopics and 
                GenerateItemIds(csv);
                // An entry is made in the item data entry table for each unique item id.
            }
            foreach (CSVFile csv in mPathToCOPFile.Values)
            {
                GenerateCmPuRatioRecs(csv); // these are mapped by item id for use by NCC CmPuRatio operations
            }
			ApplyContent();
        }

        // The results of processing a folder with nop and cop files
        public List<Isotopics> NOPIsotopics;
        public List<ItemId> NOPItemIds;
        public List<INCCAnalysisParams.cm_pu_ratio_rec> COPRatioRecs;

        // private vars used for processing
        private Dictionary<string, CSVFile> mPathToNOPFile;
        private Dictionary<string, CSVFile> mPathToCOPFile;
        private Regex yyyymmdd;
        private LMLoggers.LognLM mlogger;


        /// <summary>
        /// Scan YYYYMMDD string and generate DateTime object from it.
        /// Assumes string is at least 6 chars long and string was vetted by caller
        /// </summary>
        /// <param name="s">The yyyymmdd</param>
        /// <returns>The equivalent DateTime instance</returns>
        static DateTime GenFromYYYYMMDD(string s)
        {
            int y, m, d;
            int.TryParse(s.Substring(0, 4), out y);
            int.TryParse(s.Substring(4, 2), out m);
            int.TryParse(s.Substring(6, 2), out d);
            DateTime dt = new DateTime(y, m, d);
            return dt;
        }

        /*
            2. The operator declaration file created by Operator Review for INCC is read. 
                Isotopic data sets are created for each unique set of isotopics and 
                an entry is made in the item data entry table for each unique item id. 
            3. The header for each Radiation Review file in the common database directory with an extension of .NCC is read. The files are sorted chronologically.
            4. If no detector id has been created for a measurement, a warning message is displayed, and that measurement is skipped. In other words, you must go into INCC and create the detector before you import any measurements from that detector. This is necessary because there are several detector parameters such as gate length, dead time, efficiency ,etc. and calibration parameters that are needed to calculate mass.
            5. Each measurement that passes step 4 is displayed in chronological order, one line per measurement, in a selection screen with the following information:
                        Detector id Item id Meas. Type Date Time Filename
            6. The user selects as many measurements as desired, and they are processed in chronological order. The default is to select all measurements. The last processed background and normalization measurements will be saved in the database and used with subsequent verification measurements.
            7. At this point there are two options. If the detector dependent “auto import” box was not checked, as each measurement is processed, the appropriate acquire dialog box is displayed, enabling the user to specify the desired details for that measurement. Steps 7, 8 and 9 only apply if “Auto import” was not checked.
            etc.
        */


        #region applycontent
        // The INCC5 semantics
        // The NOP files first
        // for each row in the NOP file
        // todo:  if ! Cf 
        //     pinpoint or create* mba
        //     pinpoint or create* item id by name, and copy the mba over pinpoint/new item id entry 
        //     pinpoint or create* stratum id by name, and copy the stratum id over to the pinpoint/new item id
        //     pinpoint or create* inv change code by name, and copy the inv change code over to the pinpoint/new item id
        //     pinpoint or create* IO code by name, and copy the IO code over to the pinpoint/new item id
        //     pinpoint or create* item/mtl type by name, and copy the item/mtl type over to the pinpoint/new item id
        //     copy the mass over to the item id entry (already done in processing)
        //     start a new isotopics, copy the iso source code over to the pinpoint/new item id
        //     pinpoint or create* isotopics id, assign it to the new isotopic entry from above
        //     finish setting up the isotopic instance
        //     if not a Cf NOP, test the iso against the 0% test, and set it to default values if <= 0%
        //     if the isotopic id exists, the current one then replaces/overwrites it
        // end loop
        //
        // *create, update each element in the appropriate in-memory list and DB table
        // stratums require an extra create and set because of the dual nature

        // todo: The curium ratio COP file content needs to be applied next
        // the table prepared in Process is to be made available for the subsequent SR Rev (NCC) file import only, by matching on the table's item id
        
        /// <summary>
        /// Update/create the items (MBAs, codes, item types) and isotopics found in the NOP files
        /// </summary>
        public void ApplyContent()
        {
            MBAs();
            StratumIds();
            InvChangeCodes();
            IOCodes();
            MtlTypes();
            ItemIds();
            IsotopicGen();
        }
        protected void MBAs()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.MBAs.GetList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.mba) || NC.App.DB.MBAs.Has(f.mba))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.mba, f.mba);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.MBAs.Set(d2);
            }
        }
        protected void ItemIds()
        {
            List<ItemId> l = NC.App.DB.ItemIds.GetList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.item))
                    continue;
                if (NC.App.DB.ItemIds.Has(f.item))
                {
                    // update the existing item id with the new info
                    ItemId Id = NC.App.DB.ItemIds.Get(f.item);
                    Id.Copy(f); Id.modified = true;
                    NC.App.DB.ItemIds.Set(Id);
                }
                else
                {
                    l.Add(f); f.modified = true;
                    NC.App.DB.ItemIds.Set(f);
                }
            }
			// regen ItemId list from new db content
			NC.App.DB.ItemIds.Refresh();
        }
        protected void StratumIds()
        {
            List<INCCDB.StratumDescriptor> l = NC.App.DB.StrataList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.stratum))
                    continue;
                INCCDB.StratumDescriptor ex = l.Find(sd => string.Compare(sd.Desc.Name, f.stratum, true) == 0);
                if (ex != null)
                    continue;
                INCCDB.Descriptor d = new INCCDB.Descriptor(f.stratum, f.stratum);
                INCCDB.StratumDescriptor d2 = new INCCDB.StratumDescriptor(d, new Stratum());
                l.Add(d2);
                d2.Desc.modified = true;
                NC.App.DB.UpdateStratum(d2.Desc, d2.Stratum);  // creates it
                                                               // NC.App.DB.AssociateStratum(det, d2.Desc, d2.Stratum); // associates it with the detector
            }
        }
        protected void InvChangeCodes()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.InvChangeCodes.GetList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.inventoryChangeCode) || NC.App.DB.InvChangeCodes.Has(f.inventoryChangeCode))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.inventoryChangeCode, f.inventoryChangeCode);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.InvChangeCodes.Set(d2);
            }
        }
        protected void IOCodes()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.IOCodes.GetList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.IOCode) || NC.App.DB.IOCodes.Has(f.IOCode))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.IOCode, f.IOCode);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.IOCodes.Set(d2);
            }
        }
        protected void MtlTypes()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.Materials.GetList();
            foreach (ItemId f in NOPItemIds)
            {
                if (string.IsNullOrEmpty(f.material) || NC.App.DB.Materials.Has(f.material))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.material, f.material);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.Materials.Set(d2);
            }
        }

        protected void IsotopicGen()
        {
            // for each isotopics found, either overwrite existing isotopics, or add them to the list
            // write all the changes to the DB

            List<AnalysisDefs.Isotopics> l = NC.App.DB.Isotopics.GetList();

            foreach (Isotopics isotopics in NOPIsotopics)
            {
                // if sum of Pu isotopes <= 0 then use default isotopics vals
                if ((isotopics.pu238 + isotopics.pu239 + isotopics.pu240 + isotopics.pu241 + isotopics.pu242) <= 0.0)
                    isotopics.InitVals();

                Isotopics ex = l.Find(iso => string.Compare(iso.id, isotopics.id, true) == 0);
                if (ex == null)
                {
                    l.Add(isotopics); isotopics.modified = true;
                    NC.App.DB.Isotopics.Set(isotopics);
                }
                else
                {
                    ex.Copy(isotopics); ex.modified = true;
                    NC.App.DB.Isotopics.Set(ex);
                }
            }


        }
        #endregion applycontent


    }

}
