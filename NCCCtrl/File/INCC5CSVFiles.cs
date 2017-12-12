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
using AnalysisDefs;
using Microsoft.VisualBasic.FileIO;
using NCCReporter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
namespace NCCFile
{
    using NC = NCC.CentralizedState;

    public class CSVFile : NeutronDataFile
    {

        private List<string[]> mLines;

        public CSVFile()
        {
            Init();
        }

        public void Init()
        {
            stream = null;
            mLines = new List<string[]>();
        }

        public int LineCount { get { return mLines.Count; } }

        public List<string[]> Lines { get { return mLines; } }

        public void ProcessFile()
        {
            TextFieldParser tfp = new TextFieldParser(Filename);
            tfp.HasFieldsEnclosedInQuotes = true;
            tfp.Delimiters = new string[] { ",", "\t" };
            string[] line;

            while (!tfp.EndOfData)
            {
                line = tfp.ReadFields();
                mLines.Add(line);
            }
            tfp.Close();
        }

        // assumes strings that need to be quoted are quoted
        public static string EncodeAsCSVRow(string[] sa)
        {
            if (sa.Length < 1)
                return string.Empty;
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (string str in sa)
            {
                s.Append(str);
                s.Append(@",");
            }
            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }
    }


    /// <summary>
    /// Manager for INCC5 isotopic and composite isotopic input files (.iso etc)
    /// See Isotopics Data File Format p. 100, INCC Software Users Manual, March 29, 2009
    /// See Composite Isotopics Data File Format p. 101, INCC Software Users Manual, March 29, 2009
    /// </summary>
    public class IsoFiles
    {

        public IsoFiles()
        {
            Init();
        }

        public void Init()
        {
            //mPathToIsoFile = new Dictionary<string, CSVFile>();
            //mPathToCompFile = new Dictionary<string, CSVFile>();
			Results = new INCC5FileImportUtils();
        }  

        /// <summary>
        /// Enum of positional column ids for the INCC5 isotopics file format
        /// </summary>
        public enum IsoCol
        {
			IsoId, IsoSourceCode, Pu238, Pu239, Pu240, Pu241, Pu242, PuDate, Am241, AmDate, Pu238err, Pu239err, Pu240err, Pu241err, Pu242err, Am241err
        }

        /// <summary>
        /// Enum of positional column ids for the INCC5 composite isotopics file format
		/// CompIsoCol summary line is the entire enum, and without the first two entries is the subentry spec
        /// </summary>
        public enum CompIsCol
        {
			IsoId, IsoSourceCode, PuMass, Pu238, Pu239, Pu240, Pu241, Pu242, PuDate, Am241, AmDate, Pu238err, Pu239err, Pu240err, Pu241err, Pu242err, Am241err
        }

        /// <summary>
        /// Scan a set of iso and comp iso files.
        /// Creates a list of Isotopics from the iso files.
        /// Creates a list of CompositeIsotopics from the comp files.
        /// </summary>
        /// <param name="files">List of nop and cop files to process</param>
        public void Process(List<string> files)
        {
			if (files == null) 
				return;
            foreach (string l in files)
            try
				{
					CSVFile csv = new CSVFile();
					string name = l.Substring(l.LastIndexOf("\\") + 1); // Remove path information from string
	                csv.Log = NC.App.DataLogger;
	                csv.Filename = l;
		            csv.ExtractDateFromFilename();
					if (name.IndexOf('.') >= 0)
						csv.ThisSuffix = name.Substring(name.IndexOf('.'));
					csv.ProcessFile();  // split lines with scanner
                    bool isofile = false;
                    CompositeIsotopics ciso = null;
                    Isotopics iso = null;
                    iso = GenIso(csv.Lines[0]);
                    if (isofile = (iso != null))
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "got an iso file, process all the lines " + System.IO.Path.GetFileName(l));
                        NC.App.ControlLogger.TraceEvent(LogLevels.Info, 34100, "Processed " + iso.id + " from " + System.IO.Path.GetFileName(csv.Filename));
                        Results.IsoIsotopics.Add(iso);
                    }
                    else
                    {
                        ciso = (CompositeIsotopics)CompositeIsotopics(csv.Lines[0], headtest: true);
                        if (ciso != null)  // got a header of a comp iso file, process the rest of the lines
                        {
                            NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "got a header of a comp iso file, process the rest of the lines " + System.IO.Path.GetFileName(l));
                            Results.CompIsoIsotopics.Add(ciso);
                        }
                    }
                    for (int i = 1; i < csv.Lines.Count; i++)
					{
                        string[] entry = csv.Lines[i];
                        if (isofile)
                        {
                            iso = GenIso(entry);
                            if (iso != null)
                            {
                                NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "got an iso file, process all the lines " + System.IO.Path.GetFileName(l));
                                NC.App.ControlLogger.TraceEvent(LogLevels.Info, 34100, "Processed " + iso.id + " from " + System.IO.Path.GetFileName(csv.Filename));
                                Results.IsoIsotopics.Add(iso);
                            }
                            else NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "Skipped non-iso token entry");
                        }
                        else
                        {
                            CompositeIsotopic ci = (CompositeIsotopic)CompositeIsotopics(entry, headtest: false);
                            if (ci != null)  // got a header of a comp iso file, process the rest of the lines
                            {
                                ciso.isotopicComponents.Add(ci);
                            }

                        }
					}
				} 
				catch (MalformedLineException )  // not a CSV file
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "Skipped " + System.IO.Path.GetFileName(l));
				}           

        }

		Isotopics GenIso(string[] sa)
		{
			Array ev = Enum.GetValues(typeof(IsoCol));
			if (sa.Length != ev.Length)
				return null;
			Isotopics i = new Isotopics();
			string s = string.Empty;
			double v = 0, err = 0;
			foreach (IsoCol op in ev)
			{
				try
				{
					s = sa[(int)op];
					switch (op)
					{
					case IsoCol.AmDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.am_date);
						break;
					case IsoCol.PuDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.pu_date);
						break;
					case IsoCol.IsoSourceCode:
						Enum.TryParse(s, out i.source_code);
						break;
					case IsoCol.IsoId:
						i.id = string.Copy(s);
						break;
					case IsoCol.Pu238:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu238, v);
						break;
					case IsoCol.Pu239:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu239, v);
						break;
					case IsoCol.Pu240:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu240, v);
						break;
					case IsoCol.Pu241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu241, v);
						break;
					case IsoCol.Pu242:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu242, v);
						break;
					case IsoCol.Am241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.am241, v);
						break;
					case IsoCol.Pu238err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu238, err);
						break;
					case IsoCol.Pu239err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu239, err);
						break;
					case IsoCol.Pu240err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu240, err);
						break;
					case IsoCol.Pu241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu241, err);
						break;
					case IsoCol.Pu242err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu242, err);
						break;
					case IsoCol.Am241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.am241, err);
						break;
					}
				} 
				catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as isotopics element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}
			return i;
		}

		object CompositeIsotopics(string[] sa, bool headtest)
		{
            var res = new object();
            CompositeIsotopics cis = null;
            CompositeIsotopic ci = null;
            Array ev = Enum.GetValues(typeof(CompIsCol));
			if (headtest)  // full iso line
			{
				if (sa.Length != ev.Length)
					return null;
                cis = new CompositeIsotopics();
                res = cis;
            }
			else  // comp iso subentry line
			{
				if (sa.Length != ev.Length - 2)
					return null;
                ci = new CompositeIsotopic();
                res = ci;
            }
            float pumass = 0;
            Isotopics iso = new Isotopics();
			string s = string.Empty;
			double v = 0, err = 0;
			foreach (CompIsCol op in ev)
			{
				try
				{
                    if (headtest)
                        s = sa[(int)op];
                    else
                    {
                        if (op > CompIsCol.IsoSourceCode)
                            s = sa[(int)op - 2];
                        else
                            s = string.Empty;
                    }      

                    switch (op)
					{
					case CompIsCol.IsoSourceCode:
						if (headtest) {  Enum.TryParse(s, out cis.source_code); iso.source_code = cis.source_code; }
                        break;
					case CompIsCol.IsoId:
                        if (headtest) { cis.id = string.Copy(s); iso.id = cis.id; } 
                        break;
					case CompIsCol.PuMass:
						//if (!headtest)  // comp iso lines start with mass
						//{ 
							float sv = 0f;
							float.TryParse(s, out sv);
							pumass = sv;
						//}
						break;
					case CompIsCol.AmDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref iso.am_date);
						break;
					case CompIsCol.PuDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref iso.pu_date);
						break;
					case CompIsCol.Pu238:
						double.TryParse(s, out v);
						iso.SetVal(Isotope.pu238, v);
						break;
					case CompIsCol.Pu239:
						double.TryParse(s, out v);
                        iso.SetVal(Isotope.pu239, v);
						break;
					case CompIsCol.Pu240:
						double.TryParse(s, out v);
                        iso.SetVal(Isotope.pu240, v);
						break;
					case CompIsCol.Pu241:
						double.TryParse(s, out v);
                        iso.SetVal(Isotope.pu241, v);
						break;
					case CompIsCol.Pu242:
						double.TryParse(s, out v);
                        iso.SetVal(Isotope.pu242, v);
						break;
					case CompIsCol.Am241:
						double.TryParse(s, out v);
                        iso.SetVal(Isotope.am241, v);
						break;
					case CompIsCol.Pu238err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.pu238, err);
						break;
					case CompIsCol.Pu239err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.pu239, err);
						break;
					case CompIsCol.Pu240err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.pu240, err);
						break;
					case CompIsCol.Pu241err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.pu241, err);
						break;
					case CompIsCol.Pu242err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.pu242, err);
						break;
					case CompIsCol.Am241err:
						double.TryParse(s, out err);
                        iso.SetError(Isotope.am241, err);
						break;
					}
				} 
				catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as composite isotopics summary element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}
            // copy values onto the relevant object
            if (cis != null)
                cis.Copy(iso, pumass);
            if (ci != null)
                ci.Copy(iso, pumass);
            return res;
		}
		// The results of processing a folder with iso files
		public INCC5FileImportUtils Results;

        // private vars used for processing
       // private Dictionary<string, CSVFile> mPathToIsoFile;
       // private Dictionary<string, CSVFile> mPathToCompFile;
    }

	   
	/// <summary>
    /// Manager for Deming coefficient output file .dmr
    /// See Results (.dmr) Files p. 14, Deming Curve Fitting User’s Manual, February 26, 2002, LA-UR 02-1143
	/// 
	/// The dmr importer populates a CurveEquationVals (from a KA, CC, AAS, ACC instance) with coefficients and equation choice, then saves new state to DB
	///  
	///	Results (.dmr) Files
	///  The format of this file follows a general schema, with the details depending on the number of coefficients in
	///  the fitting equation. The results file is a COMMA delimited text file that can be imported into commercial
	///  spreadsheet programs.
	///  • The first row contains the coefficients. -- a b c d
	///  • The second row contains the absolute sigmas for the coefficients. -- sigmas for a b c d
	///  • The third row contains the variance for the first coefficient.    -- var a
	///  • Rows following the third (assuming there are two or more coefficients) contain
	///  •   #4  -- covar ba, var b
	///  •   #5  -- covar ca, covar cb, var c
	///  •   #6  -- covar bc, covar bd, covar cd, var d
	///     the covariances for the coefficient with the previous coefficients, and the variance for the coefficient in the last column
	///     Row 4 is present only if there are 2 or more coefficients; it contains: covariance b with a, variance b.
	///     Row 5 is present only if there are 3 or more coefficients; it contains: covariance c with a, covariance c with b, variance c.
	///     Row 6 is present if 4 coefficients; this is not documented but the scan pattern is inferred.
    /// </summary>
    public class CoefficientFile
    {

        public CoefficientFile()
        {
            Init();
        }

        public void Init()
        {
			Coefficients = new INCCAnalysisParams.CurveEquationVals();
        }


		/// <summary>
		/// Scan a dmr file.
		/// </summary>
		/// <param name="files">dmr file to process</param>
		public void Process(string file)
		{
			if (string.IsNullOrEmpty(file))
				return;
			try
			{
				CSVFile csv = new CSVFile();
				string name = file.Substring(file.LastIndexOf("\\") + 1); // Remove path information from string
				csv.Log = NC.App.DataLogger;
				csv.Filename = file;
				csv.ExtractDateFromFilename();
				if (name.IndexOf('.') >= 0)
					csv.ThisSuffix = name.Substring(name.IndexOf('.'));
				csv.ProcessFile();  // split lines with scanner
				if (csv.Lines.Count < 3)
				{
					NC.App.DataLogger.TraceEvent(LogLevels.Warning, 34100, "Skipped incomplete " + System.IO.Path.GetFileName(file));
					return;
				}
				// line 1
				int coeffnum = csv.Lines[0].Length; // 1 -> 3 lines, 2 -> 4 lines, 3 -> 5 lines, 4 -> 6 lines
				int lines = coeffnum + 2;
				if (csv.Lines.Count != lines)
				{
					NC.App.DataLogger.TraceEvent(LogLevels.Warning, 34100, "Expecting {0} lines, found {1}, skipping {2}", lines, csv.Lines.Count, System.IO.Path.GetFileName(file));
					return;
				}
				Coefficients.a = GetDouble(csv.Lines[0][0]);
				if (coeffnum > 1)
					Coefficients.b = GetDouble(csv.Lines[0][1]);
				if (coeffnum > 2)
					Coefficients.c = GetDouble(csv.Lines[0][2]);
				if (coeffnum > 3)
					Coefficients.d = GetDouble(csv.Lines[0][3]);

				// line 2 (skipped?)  // URGENT: check if this is used in INCC5
				//Coefficients.var_a = GetDouble(csv.Lines[1][0]);
				//if (coeffnum > 1)
				//	Coefficients.var_b = GetDouble(csv.Lines[2][1]);
				//if (coeffnum > 2)
				//	Coefficients.var_c = GetDouble(csv.Lines[3][2]);
				//if (coeffnum > 3)
				//	Coefficients.var_d = GetDouble(csv.Lines[4][3]);		

				// line 3
				Coefficients.var_a = GetDouble(csv.Lines[2][0]);

				// line 4
				if (csv.Lines.Count > 3)
				{
					if (csv.Lines[4].Length < 2)
						throw new Exception("Not enough entries on the b coefficient line " + csv.Lines[3].Length.ToString());
					Coefficients.setcovar(Coeff.a, Coeff.b, GetDouble(csv.Lines[3][0]));
					Coefficients.var_b = GetDouble(csv.Lines[3][1]);
				}
				if (csv.Lines.Count > 4)
				{
					if (csv.Lines[4].Length < 3)
						throw new Exception("Not enough entries on the c coefficient line " + csv.Lines[4].Length.ToString());
					Coefficients.setcovar(Coeff.a, Coeff.c, GetDouble(csv.Lines[4][0]));
					Coefficients.setcovar(Coeff.a, Coeff.c, GetDouble(csv.Lines[4][1]));
					Coefficients.var_c = GetDouble(csv.Lines[4][2]);
				}
				if (csv.Lines.Count > 5)
				{
					if (csv.Lines[5].Length < 4)
						throw new Exception("Not enough entries on the d coefficient line " + csv.Lines[5].Length.ToString());
					Coefficients.setcovar(Coeff.b, Coeff.c, GetDouble(csv.Lines[5][0]));
					Coefficients.setcovar(Coeff.b, Coeff.d, GetDouble(csv.Lines[5][1]));
					Coefficients.setcovar(Coeff.c, Coeff.d, GetDouble(csv.Lines[5][2]));
					Coefficients.var_d = GetDouble(csv.Lines[5][3]);
				}
			} catch (MalformedLineException)  // not a CSV file
			{
				NC.App.DataLogger.TraceEvent(LogLevels.Verbose, 34100, "Skipped " + Path.GetFileName(file));
			} catch (Exception e)  // not good
			{
				NC.App.DataLogger.TraceEvent(LogLevels.Verbose, 34100, e.Message + " - Wrongness experienced " + Path.GetFileName(file));
			}

		}

		double GetDouble(string s)
		{
			double res = 0;
			try
			{
				res = double.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands);
			}
			catch (Exception)
			{
			}
			return res;
		}



		// The results of processing a dmr file

		public INCCAnalysisParams.CurveEquationVals Coefficients;


    }



    /// <summary>
    /// Manager for INCC5 NCC_Dat
    /// See Item Relevant Data File Format p. 85, INCC Software Users Manual, March 29, 2009
    /// </summary>
    public class ItemFile
    {

        public ItemFile()
        {
            Init();
        }

        public void Init()
        {
			CollarItems = new List<CollarItemId>();
			Results = new INCC5FileImportUtils();
        }
  
        /// <summary>
        /// Enum of positional column ids 
        /// </summary>
        enum ItemCol
        {
            Facility, MBA, ItemId, StratumId, InvChangeCode, IOCode, MatType, IsoSourceCode, ItemType, IsoId,  // AnalysisDefs.Isotopics.SourceCode
			DeclRodLen,    DeclRodLenErr,
			DeclTotalPu,   DeclTotalPuErr,
			DeclDepletedU, DeclDepletedUErr,
			DeclNaturalU,  DeclNaturalUErr,
			DeclEnrichedU, DeclEnrichedUErr,
			DeclTotalU235, DeclTotalU235Err,
			DeclTotalU238, DeclTotalU238Err,
			Pu238, Pu239, Pu240, Pu241, Pu242, PuDate, Am241, AmDate, Pu238err, Pu239err, Pu240err, Pu241err, Pu242err, Am241err,
			IsoType,
			DeclTotalRods, DeclTotalPoisonRods, DeclTotalPoisonPercent, DeclTotalPoisonPercentErr, DeclRodType
        }


		enum ItemTypeIndicator
        {
			Blank, P, C, L
		}

		/// <summary>
		/// Scan a dat file.
		/// New facilities, MBAs, strata, inventory change codes, I/O codes, material types, isotopics,
		///     collar data and item ids will automatically be created if necessary. Already existing items will automatically be overwritten.        
		/// </summary>
		/// <param name="files">dat file to process</param>
		public void Process(string file)
		{
			if (string.IsNullOrEmpty(file))
				return;
			try
			{
				CSVFile csv = new CSVFile();
				string name = file.Substring(file.LastIndexOf("\\") + 1); // Remove path information from string
				csv.Log = NC.App.DataLogger;
				csv.Filename = file;
				csv.ExtractDateFromFilename();
				if (name.IndexOf('.') >= 0)
					csv.ThisSuffix = name.Substring(name.IndexOf('.'));
				csv.ProcessFile();  // split lines with scanner
				int el = System.Enum.GetValues(typeof(ItemCol)).Length;
				foreach (string[] entry in csv.Lines)
				{
					List<string> ls = new List<string>(entry);
					if (entry.Length < el)
					{
						ls.AddRange(new string[el - ls.Count]);
					}

					PopulateSingletons(ls);
					string code = Results.ItemTypes[Results.ItemTypes.Count - 1];
					string isoid = ls[(int)ItemCol.IsoId];
					if (UseDefaultIso(isoid))
						isoid = Isotopics.DefaultId;
					if (CompositeCode(code))
					{
						// comp isotopics id and item id iso id fields are the same here			
						Results.CompIsoIsotopics.Add(GenCompIso(ls));
					} else
					{
						// isotopics id and item id iso id fields are the same here, Default			
						Results.IsoIsotopics.Add(GenIso(ls));
					}
					if (CollarCode(code))
					{
						CollarItemId id = GenColl(ls);
						CollarItems.Add(id);
						id.item_id = Results.ItemNames[Results.ItemNames.Count - 1];  // the item id for this entry
					}
					ItemId iid = GenItemId(ls);
					Results.ItemIds.Add(iid);
					// mass was scooped up in one of the three scanners						
				}
			} catch (Microsoft.VisualBasic.FileIO.MalformedLineException)  // not a CSV file
			{
				NC.App.ControlLogger.TraceEvent(LogLevels.Verbose, 34100, "Skipped " + System.IO.Path.GetFileName(file));
			}
			Results.ApplyContent();
			Results.DoFacs();
			Results.CompIsotopicGen();
			Results.DoSrcCodes();
		}

/*		void ApplyContent()
*		{
Composite isotopics
SourceCodes
 }
*/
		
		void PopulateSingletons(List<string> sa)
		{
			string s = string.Empty;
			foreach (ItemCol op in System.Enum.GetValues(typeof(ItemCol)))
			{
				try
				{
					s = sa[(int)op];
					switch (op)
					{
					case ItemCol.Facility:
						Results.Facilities.Add(s);
						break;
					case ItemCol.MBA:
						Results.MBAs.Add(s);
						break;
					case ItemCol.ItemId:
						Results.ItemNames.Add(s); // used later
                        break;
					case ItemCol.StratumId:
						Results.Strata.Add(s);
						break;
					case ItemCol.InvChangeCode:
						Results.InventoryChangeCodes.Add(s);
						break;
					case ItemCol.IOCode:
						Results.IOCodes.Add(s);
						break;
					case ItemCol.MatType:
						Results.MaterialTypes.Add(s);
						break;
					case ItemCol.IsoSourceCode:
						Results.SourceCodes.Add(s); // vet later ... 
                        break;
					case ItemCol.ItemType:
						Results.ItemTypes.Add(s); // vet later for blank, P, C, L (u/l)
						break;
					}
				} catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as isotopics element " + op.ToString() + " " + ex.Message);
					return;
				}
			}
		}

		bool CompositeCode(string s)
		{
			return s.Equals("P", StringComparison.InvariantCultureIgnoreCase); // P (u/l)
		}
		bool UseDefaultIso(string s)
		{
			return (string.IsNullOrWhiteSpace(s) || s.Equals("L", StringComparison.InvariantCultureIgnoreCase));  // blank, L, (u/l)
		}
		bool CollarCode(string s)
		{
			return (s.Equals("L", StringComparison.InvariantCultureIgnoreCase));  // L, (u/l)
		}
		Isotopics GenIso(List<string> sa)
		{
			Isotopics i = new Isotopics();
			string s = string.Empty;
			double v = 0, err = 0;
			foreach (ItemCol op in System.Enum.GetValues(typeof(ItemCol)))
			{
				try
				{
					s = sa[(int)op]; 
					switch (op)
					{
					case ItemCol.AmDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.am_date);
						break;
					case ItemCol.PuDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.pu_date);
						break;
					case ItemCol.IsoSourceCode:
						Enum.TryParse(Results.SourceCodes[Results.SourceCodes.Count - 1], out i.source_code);
						break;
					case ItemCol.IsoId:
						i.id = string.Copy(s);
						break;
					case ItemCol.Pu238:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu238, v);
						break;
					case ItemCol.Pu239:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu239, v);
						break;
					case ItemCol.Pu240:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu240, v);
						break;
					case ItemCol.Pu241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu241, v);
						break;
					case ItemCol.Pu242:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu242, v);
						break;
					case ItemCol.Am241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.am241, v);
						break;
					case ItemCol.Pu238err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu238, err);
						break;
					case ItemCol.Pu239err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu239, err);
						break;
					case ItemCol.Pu240err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu240, err);
						break;
					case ItemCol.Pu241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu241, err);
						break;
					case ItemCol.Pu242err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu242, err);
						break;
					case ItemCol.Am241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.am241, err);
						break;
					}
				} catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as isotopics element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}
			return i;
		}

		ItemId GenItemId(List<string> sa)
		{
			ItemId i = new ItemId();
			string s = string.Empty;
			foreach (ItemCol op in System.Enum.GetValues(typeof(ItemCol)))
			{
				try
				{
					s = sa[(int)op];  // might blow here when file was badly created
					switch (op)
					{
					case ItemCol.ItemId:
						i.item = s;
						break;
					case ItemCol.IOCode:
						i.IOCode = s;
						break;
					case ItemCol.InvChangeCode:
						i.inventoryChangeCode = s;
						break;
					case ItemCol.MBA:
						i.mba = s;
						break;
					case ItemCol.MatType:
						i.material = s;
						break;
					case ItemCol.StratumId:
						i.stratum = s;
						break;
					case ItemCol.IsoId:
						i.isotopics = string.Copy(s);
						break;
					case ItemCol.DeclRodLen:
                        double.TryParse(s, out i.length);
						break;
					case ItemCol.DeclTotalPu:
                        double.TryParse(s, out i.declaredMass);
						break;
					case ItemCol.DeclEnrichedU:
                        double.TryParse(s, out i.declaredUMass);
						break;
					case ItemCol.AmDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.am_date);
						break;
					case ItemCol.PuDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.pu_date);
						break;
					}
				} catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as item element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}
			return i;
		}

		CompositeIsotopics GenCompIso(List<string> sa)
		{
			CompositeIsotopics i = new CompositeIsotopics();
			string s = string.Empty;
			double v = 0, err = 0;
			foreach (ItemCol op in System.Enum.GetValues(typeof(ItemCol)))
			{
				try
				{
					s = sa[(int)op]; 
					switch (op)
					{
					case ItemCol.AmDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.am_date);
						break;
					case ItemCol.PuDate:
						INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.pu_date);
						break;
					case ItemCol.IsoSourceCode:
						Enum.TryParse(s, out i.source_code);
						break;
					case ItemCol.DeclTotalPu:
						float sv = 0;
						float.TryParse(s, out sv);
						i.pu_mass = sv;
						break;
					case ItemCol.Pu238:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu238, v);
						break;
					case ItemCol.Pu239:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu239, v);
						break;
					case ItemCol.Pu240:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu240, v);
						break;
					case ItemCol.Pu241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu241, v);
						break;
					case ItemCol.Pu242:
						double.TryParse(s, out v);
						i.SetVal(Isotope.pu242, v);
						break;
					case ItemCol.Am241:
						double.TryParse(s, out v);
						i.SetVal(Isotope.am241, v);
						break;
					case ItemCol.Pu238err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu238, err);
						break;
					case ItemCol.Pu239err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu239, err);
						break;
					case ItemCol.Pu240err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu240, err);
						break;
					case ItemCol.Pu241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu241, err);
						break;
					case ItemCol.Pu242err:
						double.TryParse(s, out err);
						i.SetError(Isotope.pu242, err);
						break;
					case ItemCol.Am241err:
						double.TryParse(s, out err);
						i.SetError(Isotope.am241, err);
						break;
					}
				} catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as composite isotopics summary element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}
			return i;
		}

		CollarItemId GenColl(List<string> sa)
		{
			CollarItemId cid = new CollarItemId();

			string s = string.Empty;
			double v = 0;
			foreach (ItemCol op in System.Enum.GetValues(typeof(ItemCol)))
			{
				try
				{
					s = sa[(int)op]; 
					switch (op)
					{
					case ItemCol.DeclRodLen:
						double.TryParse(s, out v);
						cid.length.v = v;
						break;
					case ItemCol.DeclRodLenErr:
						double.TryParse(s, out v);
						cid.length.err = v;
						break;
					case ItemCol.DeclTotalPu:
						double.TryParse(s, out v);
						cid.total_pu.v = v;
						break;
					case ItemCol.DeclTotalPuErr:
						double.TryParse(s, out v);
						cid.total_pu.err = v;
						break;
					case ItemCol.DeclDepletedU:
						double.TryParse(s, out v);
						cid.depleted_u.v = v;
						break;
					case ItemCol.DeclDepletedUErr:
						double.TryParse(s, out v);
						cid.depleted_u.err = v;
						break;
					case ItemCol.DeclNaturalU:
						double.TryParse(s, out v);
						cid.natural_u.v = v;
						break;
					case ItemCol.DeclNaturalUErr:
						double.TryParse(s, out v);
						cid.natural_u.err = v;
						break;
					case ItemCol.DeclEnrichedU:
						double.TryParse(s, out v);
						cid.enriched_u.v = v;
						break;
					case ItemCol.DeclEnrichedUErr:
						double.TryParse(s, out v);
						cid.enriched_u.err = v;
						break;
					case ItemCol.DeclTotalU235:
						double.TryParse(s, out v);
						cid.total_u235.v = v;
						break;
					case ItemCol.DeclTotalU235Err:
						double.TryParse(s, out v);
						cid.total_u235.err = v;
						break;
					case ItemCol.DeclTotalU238:
						double.TryParse(s, out v);
						cid.total_u238.v = v;
						break;
					case ItemCol.DeclTotalU238Err:
						double.TryParse(s, out v);
						cid.total_u238.err = v;
						break;
					case ItemCol.DeclTotalRods:
						double.TryParse(s, out v);
						cid.total_rods = v;
						break;
					case ItemCol.DeclTotalPoisonRods:
						double.TryParse(s, out v);
						cid.total_poison_rods = v;
						break;
					case ItemCol.DeclTotalPoisonPercent:
						double.TryParse(s, out v);
						cid.poison_percent.v = v;
						break;
					case ItemCol.DeclTotalPoisonPercentErr:
						double.TryParse(s, out v);
						cid.poison_percent.err = v;
						break;
					case ItemCol.DeclRodType:
						cid.rod_type = s;
						break;
					}

				} catch (Exception ex)
				{
					NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as collar element " + op.ToString() + " " + ex.Message);
					return null;
				}
			}

			return cid;
		}

		// The results of processing a dat file

		public List<CollarItemId> CollarItems;

		public INCC5FileImportUtils Results;

    }


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
            COPRatioRecs = new List<INCCAnalysisParams.cm_pu_ratio_rec>();
			Results = new INCC5FileImportUtils();
        }

        /// <summary>
        /// Create a list of Isotopics instances from each CSV parsed line set
        /// </summary>
        /// <param name="csv">The field-scanned csv file instance with an array of tokenized lines</param>
        void GenerateIsotopics(CSVFile csv)
        {
            List<Isotopics> l = Results.IsoIsotopics;  // local alias
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
								INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.am_date);
                                break;
                            case NOPCol.PuDate:
								INCC5FileImportUtils.GenFromYYYYMMDD(s, ref i.pu_date );
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
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as isotopics element " + op.ToString() + " " + ex.Message);
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
            List<ItemId> l = Results.ItemIds;  // local alias
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
                                INCC5FileImportUtils.GenFromYYYYMMDD(s, ref iid.am_date);
                                break;
                            case NOPCol.PuDate:
								INCC5FileImportUtils.GenFromYYYYMMDD(s, ref iid.pu_date);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as item element " + op.ToString() + " " + ex.Message);
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
								INCC5FileImportUtils.GenFromYYYYMMDD(s, ref cpr.cm_pu_ratio_date);
                                break;
                            case COPCol.CmUDate:
								INCC5FileImportUtils.GenFromYYYYMMDD(s, ref cpr.cm_u_ratio_date);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        NC.App.ControlLogger.TraceEvent(LogLevels.Warning, 34100, s + " fails as CmPu ratio element " + op.ToString() + " " + ex.Message);
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
                NC.App.ControlLogger.TraceEvent(LogLevels.Info, 34100, "Processed " + System.IO.Path.GetFileName(csv.Filename));
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
			Results.ApplyContent();
        }

        // The results of processing a folder with nop and cop files
		public INCC5FileImportUtils Results;
        public List<INCCAnalysisParams.cm_pu_ratio_rec> COPRatioRecs;

        // private vars used for processing
        private Dictionary<string, CSVFile> mPathToNOPFile;
        private Dictionary<string, CSVFile> mPathToCOPFile;


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
 
 
    }

	public class INCC5FileImportUtils
	{
		public INCC5FileImportUtils()
		{
			Init();
		}

		public List<Isotopics> IsoIsotopics;
		public List<CompositeIsotopics> CompIsoIsotopics;
		public List<ItemId> ItemIds;
		public List<string> Facilities, MBAs, ItemNames, Strata, InventoryChangeCodes, IOCodes, MaterialTypes, SourceCodes, ItemTypes;
        //  vars used for processing

		void Init()
		{
			IsoIsotopics = new List<AnalysisDefs.Isotopics>();
            CompIsoIsotopics = new List<AnalysisDefs.CompositeIsotopics>();
			ItemIds = new List<ItemId>();
			Facilities = new List<string>();
			MBAs = new List<string>();
			Strata = new List<string>();
			InventoryChangeCodes = new List<string>();
			IOCodes = new List<string>();
			MaterialTypes = new List<string>();
			SourceCodes = new List<string>();
			ItemTypes = new List<string>();
			ItemNames = new List<string>();
		}

		static Regex yyyymmdd = new Regex("^\\d{4}\\d{2}\\d{2}$");
 
        /// <summary>
        /// Scan YYYYMMDD string and generate DateTime object from it.
        /// Assumes string is at least 6 chars long and string was vetted by caller
        /// </summary>
        /// <param name="s">The yyyymmdd</param>
        /// <returns>The equivalent DateTime instance</returns>
        static public bool GenFromYYYYMMDD(string s, ref DateTime dt)
        {
			Match pud = yyyymmdd.Match(s);
			if (!pud.Success) return false;
            int y, m, d;
            int.TryParse(s.Substring(0, 4), out y);
            int.TryParse(s.Substring(4, 2), out m);
            int.TryParse(s.Substring(6, 2), out d);
            dt = new DateTime(y, m, d); 
			return true;           
        }

        
        /// <summary>
        /// Update/create the items (MBAs, codes, item types) and isotopics found in DAT/NOP/COP/CSV files
        /// </summary>
        public void ApplyContent()
        {
            DoMBAs();
            StratumIds();
            InvChangeCodes();
            DoIOCodes();
            MtlTypes();
            DoItemIds();
            IsotopicGen();
        }
        protected void DoMBAs()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.MBAs.GetList();
            foreach (ItemId f in ItemIds)
            {
                if (string.IsNullOrEmpty(f.mba) || NC.App.DB.MBAs.Has(f.mba))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.mba, f.mba);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.MBAs.Set(d2);
            }
        }

		public void DoFacs()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.Facilities.GetList();
            foreach (string s in Facilities)
            {
                if (string.IsNullOrEmpty(s) || NC.App.DB.Facilities.Has(s))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(s, s);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.Facilities.Set(d2);
            }
        }

		public void DoSrcCodes()
        {

        }

        protected void DoItemIds()
        {
            List<ItemId> l = NC.App.DB.ItemIds.GetList();
            foreach (ItemId f in ItemIds)
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
            foreach (ItemId f in ItemIds)
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
            foreach (ItemId f in ItemIds)
            {
                if (string.IsNullOrEmpty(f.inventoryChangeCode) || NC.App.DB.InvChangeCodes.Has(f.inventoryChangeCode))
                    continue;
                INCCDB.Descriptor d2 = new INCCDB.Descriptor(f.inventoryChangeCode, f.inventoryChangeCode);
                d2.modified = true;
                l.Add(d2);
                NC.App.DB.InvChangeCodes.Set(d2);
            }
        }
        protected void DoIOCodes()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.IOCodes.GetList();
            foreach (ItemId f in ItemIds)
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
            foreach (ItemId f in ItemIds)
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

            foreach (Isotopics isotopics in IsoIsotopics)
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

		public void CompIsotopicGen()
        {
            // for each isotopics found, either overwrite existing isotopics, or add them to the list
            // write all the changes to the DB

            List<AnalysisDefs.CompositeIsotopics> l = NC.App.DB.CompositeIsotopics.GetList();

            foreach (CompositeIsotopics isotopics in CompIsoIsotopics)
            {
                // if sum of Pu isotopes <= 0 then use default isotopics vals
                if ((isotopics.pu238 + isotopics.pu239 + isotopics.pu240 + isotopics.pu241 + isotopics.pu242) <= 0.0)
                    isotopics.InitVals();

                CompositeIsotopics ex = l.Find(iso => string.Compare(iso.id, isotopics.id, true) == 0);
                if (ex == null)
                {
                    l.Add(isotopics); isotopics.modified = true;
                    NC.App.DB.CompositeIsotopics.Set(isotopics);
                }
                else
                {
                    ex.Copy(isotopics); ex.modified = true;
                    NC.App.DB.CompositeIsotopics.Set(ex);
                }
            }
        }

	}

    // NEXT: these features shoud be folded into the IsoFile class
    public class INCC5FileExportUtils
	{
		public INCC5FileExportUtils()
		{
			Init();
		}

		public List<Isotopics> IsoIsotopics;  // for export
		public CompositeIsotopics CompIsoIsotopics;
        public Logging.Log mlogger;  
        public CSVFile Output;

        void Init()
		{
			IsoIsotopics = new List<Isotopics>();
            CompIsoIsotopics = null;
            mlogger = NC.App.ControlLogger;
            Output = new CSVFile();
            Output.Log = mlogger;
        }

        string Quote(string s)
        {
            return "\"" + s + "\"";
        }

		string[] Convert(Isotopics i)
		{
			Array ev = Enum.GetValues(typeof(IsoFiles.IsoCol));
			string[] sa = new string[ev.Length];
			sa[(int)IsoFiles.IsoCol.AmDate] = Quote(i.am_date.ToString("yyyyMMdd"));
			sa[(int)IsoFiles.IsoCol.PuDate] = Quote(i.pu_date.ToString("yyyyMMdd"));
			sa[(int)IsoFiles.IsoCol.IsoSourceCode] = Quote(i.source_code.ToString());
			sa[(int)IsoFiles.IsoCol.IsoId] = Quote(i.id.ToString());
			sa[(int)IsoFiles.IsoCol.Pu238] = i.pu238.ToString();
			sa[(int)IsoFiles.IsoCol.Pu238err] = i.pu238_err.ToString();
			sa[(int)IsoFiles.IsoCol.Pu239] = i.pu239.ToString();
			sa[(int)IsoFiles.IsoCol.Pu239err] = i.pu239_err.ToString();
			sa[(int)IsoFiles.IsoCol.Pu240] = i.pu240.ToString();
			sa[(int)IsoFiles.IsoCol.Pu240err] = i.pu240_err.ToString();
			sa[(int)IsoFiles.IsoCol.Pu241] = i.pu241.ToString();
			sa[(int)IsoFiles.IsoCol.Pu241err] = i.pu241_err.ToString();
			sa[(int)IsoFiles.IsoCol.Pu242] = i.pu242.ToString();
			sa[(int)IsoFiles.IsoCol.Pu242err] = i.pu242_err.ToString();
			sa[(int)IsoFiles.IsoCol.Am241] = i.am241.ToString();
			sa[(int)IsoFiles.IsoCol.Am241err] = i.am241_err.ToString();
			return sa;
		}

		List<string[]> Convert(CompositeIsotopics i)
		{
            List<string[]> b = new List<string[]>();
            // do the header line 
            Array ev = Enum.GetValues(typeof(IsoFiles.CompIsCol));
			string[] sa = new string[ev.Length];
			sa[(int)IsoFiles.CompIsCol.AmDate] = Quote(i.am_date.ToString("yyyyMMdd"));
			sa[(int)IsoFiles.CompIsCol.PuDate] = Quote(i.pu_date.ToString("yyyyMMdd"));
            sa[(int)IsoFiles.CompIsCol.PuMass] = i.pu_mass.ToString();
            sa[(int)IsoFiles.CompIsCol.IsoSourceCode] = Quote(i.source_code.ToString());
            sa[(int)IsoFiles.CompIsCol.IsoId] = Quote(i.id.ToString());
            sa[(int)IsoFiles.CompIsCol.Pu238] = i.pu238.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu238err] = i.pu238_err.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu239] = i.pu239.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu239err] = i.pu239_err.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu240] = i.pu240.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu240err] = i.pu240_err.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu241] = i.pu241.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu241err] = i.pu241_err.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu242] = i.pu242.ToString();
			sa[(int)IsoFiles.CompIsCol.Pu242err] = i.pu242_err.ToString();
			sa[(int)IsoFiles.CompIsCol.Am241] = i.am241.ToString();
			sa[(int)IsoFiles.CompIsCol.Am241err] = i.am241_err.ToString();
            b.Add(sa);
            // now do each compite as a line
            foreach(CompositeIsotopic ci in i.isotopicComponents)
                b.Add(Convert(ci));
			return b;
		}

        string[] Convert(CompositeIsotopic i)
        {
            List<string[]> b = new List<string[]>();
            // do the header line 
            Array ev = Enum.GetValues(typeof(IsoFiles.CompIsCol));
            string[] sa = new string[ev.Length - 2]; // not the identifier header entry so just do a - 2
            sa[(int)IsoFiles.CompIsCol.AmDate - 2] = Quote(i.am_date.ToString("yyyyMMdd"));
            sa[(int)IsoFiles.CompIsCol.PuDate - 2] = Quote(i.pu_date.ToString("yyyyMMdd"));
            sa[(int)IsoFiles.CompIsCol.PuMass - 2] = i.pu_mass.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu238 - 2] = i.pu238.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu238err - 2] = i.pu238_err.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu239 - 2] = i.pu239.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu239err - 2] = i.pu239_err.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu240 - 2] = i.pu240.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu240err - 2] = i.pu240_err.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu241 - 2] = i.pu241.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu241err - 2] = i.pu241_err.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu242 - 2] = i.pu242.ToString();
            sa[(int)IsoFiles.CompIsCol.Pu242err - 2] = i.pu242_err.ToString();
            sa[(int)IsoFiles.CompIsCol.Am241 - 2] = i.am241.ToString();
            sa[(int)IsoFiles.CompIsCol.Am241err - 2] = i.am241_err.ToString();
            return sa;
        }

        public void ProcessIsotopicsToFile()
		{
			ProcessIsotopics();
            if (Output.Lines.Count > 0)
            {
                if (!Output.CreateForWriting())
                    return;
                StreamWriter writer = new StreamWriter(Output.stream);
                if (writer == null)
                    return; 
                WriteIsotopics(writer);
                writer.Close();
                Output.CloseStream();
            }
        }
        public void WriteIsotopics(StreamWriter writer)
        {
            foreach (string[] sa in Output.Lines)
            {
                string s = CSVFile.EncodeAsCSVRow(sa);
                writer.WriteLine(s);
                NC.App.ControlLogger.TraceInformation(sa[(int)IsoFiles.IsoCol.IsoId] + " isotopics written");
            }
        }  

        public void ProcessIsotopics()
		{
            foreach (Isotopics iso in IsoIsotopics)
			{
				string[] a = Convert(iso);
                // write array as line in a CSV file
                Output.Lines.Add(a);
            }
		}

		public void ProcessCompositeIsotopics()
		{
            if (CompIsoIsotopics == null) return;
			List<string[]> a = Convert(CompIsoIsotopics);
            foreach (string[] s in a)
                Output.Lines.Add(s);
        }

        public void ProcessCompositeIsotopicsToFile()
        {
            ProcessCompositeIsotopics();
            if (Output.Lines.Count > 0)
            {
                if (!Output.CreateForWriting())
                    return;
                StreamWriter writer = new StreamWriter(Output.stream);
                if (writer == null)
                    return;
                WriteCompositeIsotopics(writer);
                writer.Close();
                Output.CloseStream();
            }
        }
        public void WriteCompositeIsotopics(StreamWriter writer)
        {
            if (Output.Lines.Count < 1)
                return;
            foreach (string[] sa in Output.Lines)
            {
                string s = CSVFile.EncodeAsCSVRow(sa);
                writer.WriteLine(s);
            }
            NC.App.ControlLogger.TraceInformation(Output.Lines[0][(int)IsoFiles.CompIsCol.IsoId] + " composite isotopics written");
        }
    }

    /// <summary>
    /// Manager for INCC5 Stratum Authority files
    /// See Stratum Authority File Format p. 83, INCC Software Users Manual, March 29, 2009
    /// </summary>
    public class SAFile
    {

        public SAFile()
        {
            Init();
        }

        public void Init()
        {
            Strata = new List<INCCDB.StratumDescriptor>();
            Facilities_ = new List<INCCDB.Descriptor>();
        }


        /// <summary>
        /// Create a list of stratum and facilities from a single line set 
        /// </summary>
        /// <param name="csv">The field-scanned csv file instance with an array of tokenized lines</param>
        void GenerateStrata(CSVFile csv)
        {
            string s = string.Empty;
            Array ae = System.Enum.GetValues(typeof(SACol));
            foreach (string[] sa in csv.Lines)
            {
                if (sa.Length != ae.Length)
                    continue;
                INCCDB.Descriptor desc = null;
                Stratum strat = new Stratum();
                string facname = string.Empty;
                double d = 0;
                foreach (SACol op in ae)
                {
                    try
                    {
                        s = sa[(int)op];
                        switch (op)
                        {
                            case SACol.Facility:
                               Facilities_.Add(new INCCDB.Descriptor(s,s));
                                break;
                            case SACol.StratumId:
                                facname = string.Copy(s);
                                break;
                            case SACol.StratumDesc:
                                desc = new INCCDB.Descriptor(facname, s);
                                break;
                            case SACol.Bias:
                                double.TryParse(s, out d);
                                strat.bias_uncertainty = d;
                                break;
                            case SACol.Random:
                                double.TryParse(s, out d);
                                strat.random_uncertainty = d;
                                break;
                            case SACol.Systematic:
                                double.TryParse(s, out d);
                                strat.systematic_uncertainty = d;
                                break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                Strata.Add(new INCCDB.StratumDescriptor(desc, strat));
            }
        }

        /// <summary>
        /// Enum of positional column ids for the INCC5 Stratum file format
        /// </summary>
        enum SACol
        {
            Facility, StratumId, StratumDesc, Bias, Random, Systematic
        }


        /// <summary>
        /// Scan a stratum authority file.
        /// Creates a list of Strata and a list of Facilities from the file.
        /// </summary>
        /// <param name="file">file to process</param>
        public void Process(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            CSVFile csv = new CSVFile();
            string name = path.Substring(path.LastIndexOf("\\") + 1); // Remove path information from string
            csv.Log = NC.App.DataLogger;
            csv.Filename = path;
            csv.ExtractDateFromFilename();
            if (name.IndexOf('.') >= 0)
                csv.ThisSuffix = name.Substring(name.IndexOf('.'));
            csv.ProcessFile();  // split lines with scanner
            NC.App.ControlLogger.TraceEvent(LogLevels.Info, 34100, "Processed " + Path.GetFileName(csv.Filename));

            GenerateStrata(csv);
            DoFacs();
            DoStratumIds();
        }

    	public void DoFacs()
        {
            List<INCCDB.Descriptor> l = NC.App.DB.Facilities.GetList();
            foreach (INCCDB.Descriptor desc in Facilities_)
            {
                if (string.IsNullOrEmpty(desc.Name) || NC.App.DB.Facilities.Has(desc))
                    continue;
                desc.modified = true;
                l.Add(desc);
                NC.App.DB.Facilities.Set(desc);
            }
        }

        protected void DoStratumIds()
        {
            List<INCCDB.StratumDescriptor> l = NC.App.DB.StrataList();
            foreach (INCCDB.StratumDescriptor sde in Strata)
            {
                if (string.IsNullOrEmpty(sde.Desc.Name))
                    continue;
                int ex = l.FindIndex(sd => string.Compare(sd.Desc.Name, sde.Desc.Name, true) == 0);
                if (ex >= 0)   // replace
                {
                    l[ex] = sde;
                }                    
                else
                    l.Add(sde);
                sde.Desc.modified = true;
                NC.App.DB.UpdateStratum(sde.Desc, sde.Stratum);  // creates or updates it
            }
            NC.App.DB.Stratums.Reset();
        }

        public List<INCCDB.StratumDescriptor> Strata;
        public List<INCCDB.Descriptor> Facilities_;

    }
}