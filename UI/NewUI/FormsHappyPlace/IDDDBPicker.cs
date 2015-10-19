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
using System.IO;
using System.Windows.Forms;

namespace NewUI
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using NC = NCC.CentralizedState;

    public partial class IDDDBPicker : Form
    {
        string ConxString = String.Empty;
        string ProviderName = String.Empty;

        public IDDDBPicker()
        {
            InitializeComponent();
            //Get current DB info
            ConxString = NC.App.Pest.cfg.MyDBConnectionString;
            ProviderName = NC.App.Pest.cfg.MyProviderName;
            ToolTip dbToolTip = new ToolTip();
            dbToolTip.SetToolTip(SelectBtn, "The main DB file should be named \"INCC6\" and will be set as the new default DB.");
            TextBoxDatabaseFile.Text = GetDBFileFromConxString (ConxString);
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = Path.GetDirectoryName (TextBoxDatabaseFile.Text);
            DialogResult dr = fb.ShowDialog();

            if (dr == DialogResult.OK)
                if (fb.SelectedPath != Path.GetDirectoryName(TextBoxDatabaseFile.Text))
                //todo: check on error handling for bad DB. hn 3/5/15
                //They selected a different db
                {
                    List<string> files = new List<string>(Directory.EnumerateFiles(fb.SelectedPath));
                    //Search for main DB file\
                    bool success = false;
                    foreach (var f in files)
                    {
                        String s = (String)f;
                        if (s.ToLower().Contains("incc6"))
                        {
                            //OK, so was failing when pointing to a dir with another file containing 'incc', just or them.
                            success = success || NC.App.Pest.SwitchDB(f);
                            if (success)
                            {
                                NCCConfig.DBConfig dbnew = NC.App.Config.DB;
                                NC.App.LoadPersistenceConfig(dbnew);
                                NC.App.Config.RetainChanges();
                                TextBoxDatabaseFile.Text = GetDBFileFromConxString (dbnew.MyDBConnectionString);
                            }
                        }
                    }
                    if (!success)
                    {
                        MessageBox.Show("No INCC6 database was found at the selected location.  Keeping current DB connection", "WARNING");
                    }
        }

        }
        private string GetDBFileFromConxString (string ConxString)
        {
            Match m = Regex.Match(ConxString, "^Data\\sSource(\\s)*=(\\s)*(?<DataSourceName>([^;]*))", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            char[] trimchars = new char [1]{ '\"' };
            if (m.Success)
                return (m.Groups[3].Value.TrimStart(trimchars).TrimEnd(trimchars));
            else
                return String.Empty;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            NC.App.Config.RetainChanges(); 
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
