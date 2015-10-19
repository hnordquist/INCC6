/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

namespace NCCReporter
{
    //using Excel = Microsoft.Office.Interop.Excel;

    // open bare worksheet, add rows one-by-one as they are added by the client to the log, the output file or the console
    public class ExcelPush
    {
        protected NCCReporter.LMLoggers.LognLM ctrllog;
       //protected Excel.Application target;

        // dev note: pre-define template somewhere, and use it, having the very nice graph already prepared for line by line updating here
        public ExcelPush(string existingWB, NCCReporter.LMLoggers.LognLM ctrllog)
        {
            this.ctrllog = ctrllog;
        }

        public ExcelPush(NCCReporter.LMLoggers.LognLM ctrllog)
        {
            this.ctrllog = ctrllog;
        }


        public static bool ExcelPresent(NCCReporter.LMLoggers.LognLM optlog = null)
        {
            try
            {
                //Excel.Application nitz;
                //nitz = new Excel.Application();
                return true;
            }
            catch (Exception e)
            {
                if (optlog != null)
                    optlog.TraceException(e);
            }
            return false;
        }

        public void ShowWB()
        {
            try
            {
                //if (target == null)
                //{
                //    target = new Excel.Application();
                //    target.Workbooks.Add(); // add a new empty WB
                //    target.Visible = true;  // pop it up to the front, really annoying, in the traditional Windows way!
                //}
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
        }

        
        public void AddHeaderRow(System.Type myenumtype)
        {
            //if (target == null)
            //    return;
            //Columns c = GenColumnRow(myenumtype); // add the column row, mind your indices!
            //for (int j = 1; j <= c.h.Length; j++)
            //{
            //    target.Cells[1, j] = c.h[j - 1];
            //}
        }

        // get column row content with enum and add this row to the WB
        static public Columns GenColumnRow(System.Type et)
        {
            //int len = System.Enum.GetValues(et).Length;
            return new HeadFoot(et).cols;
        }

    }
}
