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
using System.Collections.Generic;
using NCCReporter;
namespace AnalysisDefs
{
    using NC = NCC.CentralizedState;

    public class ReportMangler
    {
        public ReportMangler(NCCReporter.LMLoggers.LognLM ctrllog)
        {
            this.ctrllog = ctrllog;
            INCCResultsReports = new List<List<string>>();
            TestDataFiles = new List<List<string>>();
        }

        NCCReporter.LMLoggers.LognLM ctrllog;

        // list of reports preserved for use by UI
        public List<List<string>> INCCResultsReports;

        // summary report preserved for use by UI
        public List<string> ResultsReport;

        // list of test data files
        public List<List<string>> TestDataFiles;

        public void GenerateReports(Measurement m)
        {
            if (m.Detector.ListMode) // generate list mode report if it is list mode, hey!
			{
				RawAnalysisReport rep = new AnalysisDefs.RawAnalysisReport(ctrllog);
				rep.GenerateReport(m);
				ResultsReport = rep.replines;
			}
            MethodResultsReport mrep = new AnalysisDefs.MethodResultsReport(ctrllog);
			mrep.ApplyReportSectionSelections(m.AcquireState.review.Selections);
            mrep.GenerateReport(m);
            foreach (List<string> r in mrep.INCCResultsReports)
            {
                INCCResultsReports.Add(r);
            }
            if (NC.App.AppContext.CreateINCC5TestDataFile)
            { 
                TestDataFile mdat = new AnalysisDefs.TestDataFile(ctrllog);
                mdat.GenerateReport(m);
                foreach (List<string> r in mdat.INCCTestDataFiles)
                {
                    TestDataFiles.Add(r);
                }
            }

            if (NC.App.AppContext.OpenResults)
            {
                string notepadPath = System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe");
                if (System.IO.File.Exists(notepadPath))
                {
                    foreach (ResultFile fname in m.ResultsFiles)
                        System.Diagnostics.Process.Start(notepadPath, fname.Path);
                }
                // todo:optional enablement
                // Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(m.ResultsFileName);
                //excel.Visible = true;
            }
        }
    }


    public class SimpleReport
    {

        public SimpleReport(LMLoggers.LognLM ctrllog)
        {
            this.ctrllog = ctrllog;
        }

        protected LMLoggers.LognLM ctrllog;
        protected TabularReport t;
        protected Measurement meas;
        protected List<Section> sections;

        public void PrepForReportGeneration(Measurement m, char separator)
        {
            this.meas = m;
            t = new TabularReport(NC.App.Loggers);
            t.Separator = separator;
            sections = new List<Section>(200);
        }

        private static string CleansePotentialFilename(string s)
        {
            if (String.IsNullOrEmpty(s))  // null to empty string, callers may now relax
                return "";
            char[] ca = s.ToCharArray();
            int i;
            char dchar;

            System.Text.StringBuilder dumptext = new System.Text.StringBuilder();
            char[] inv = System.IO.Path.GetInvalidFileNameChars();
            for (i = 0; i < ca.Length; i++)
            {
                dchar = ca[i];
                foreach (char c in inv)
                    if (dchar == c)
                    {
                        dchar = '_';
                        break;
                    }
                dumptext.Append(dchar);
            }
            return dumptext.ToString();
        }

        // detector id + pretext + datetime pattern
        public virtual string GenBaseFileName(string pretext)
        {
            // create a unique file name for the output results 
            string name = pretext + meas.MeasDate.ToString("yyyyMMddHHmmss");
            if (!String.IsNullOrEmpty(meas.Detector.Id.DetectorId))
                name = meas.Detector.Id.DetectorId + " " + name;
            name = CleansePotentialFilename(name);
            return name;
        }
        public void CreateOutputFile(string filename, string suffixoverride)
        {
            try
            {
                t.CreateOutputFile(meas.AcquireState.lm.Results, filename, suffixoverride);
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
        }

        public virtual void StartReportGeneration(Measurement m, string pretext, char separator = ',', string suffixoverride = null)
        {
            PrepForReportGeneration(m, separator);

            CreateOutputFile(GenBaseFileName(pretext), suffixoverride);  // use the default name generator GenBaseFileName

        }
        protected virtual void FinishReportGeneration()
        {
            try
            {
                // t.rows has the list of rows
                t.CreateReport(NC.App.AppContext.LogResults);
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            finally
            {
                t.CloseOutputFile();
            }
        }

        /// <summary>
        /// Row representation of configuration file and assembly details
        /// </summary>
        /// <returns></returns>
        static public Row[] GenSoftwareConfigRows()
        {
            string[] foo = NCCConfig.Config.ShowCfgLines(NC.App.Config, true, true);
            Row[] rows = new Row[foo.Length];
            for (int i = 0; i < foo.Length; i++)
            {
                rows[i] = new Row();
                rows[i].Add(0, foo[i]);
            }
            return rows;
        }
    }

}




