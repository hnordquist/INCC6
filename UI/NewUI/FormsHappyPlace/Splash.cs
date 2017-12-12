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
using System.IO;
using System.Windows.Forms;
using NCCConfig;
using NCCReporter;

namespace NewUI
{
    public partial class Splash : Form
    {
        public Splash()
        {
            //Updated with BSD License...... HN 10.15.2015
            InitializeComponent();
            textBox1.Text = AssemblyConstants.ShortAssemblyProduct + " " + AppContextConfig.GetVersionString() +", " + textBox1.Text;
            Text = AssemblyConstants.ShortAssemblyProduct + " " + AppContextConfig.GetVersionString() + ", " + Text;

            richTextBox1.Text = @"This work was supported by the United States Member State Support Program to IAEA Safeguards; the U.S. Department of Energy, ";
            richTextBox1.Text += @"Office of Nonproliferation and National Security, International Safeguards Division; and the U.S. Department of Energy, Office of Safeguards and Security." + Environment.NewLine + Environment.NewLine;
            richTextBox1.Text += @"LA-CC-14-009. This software was exported from the United States in accordance with the Export Administration Regulations. Diversion contrary to U.S. law prohibited.";

            richTextBox2.Text = "Copyright © 2016, Los Alamos National Security, LLC. This software application and associated material (\"The Software\") was prepared by the Los Alamos National Security, LLC (LANS),";
            richTextBox2.Text += @" under Contract DE-AC52-06NA25396 with the U.S. Department of Energy (DOE) for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, LLC (LANS) for the";
            richTextBox2.Text += @"the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY,";
            richTextBox2.Text += @" EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, such modified software should be clearly marked, so as not to confuse";
            richTextBox2.Text += @"it with the version available from LANL. All rights in the software application and associated material are reserved by DOE on behalf of the Government and LANS pursuant to the contract.";
            
            richTextBox3.Text = @"Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:" + Environment.NewLine + Environment.NewLine;
            richTextBox3.Text += @"1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer."  + Environment.NewLine;
            richTextBox3.Text += @"2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution." + Environment.NewLine;
            richTextBox3.Text += @"3. Neither the name of the Los Alamos National Security, LLC., Los Alamos National Laboratory, LANL, the U.S. Government, nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission." + Environment.NewLine + Environment.NewLine;
            richTextBox3.Text += "THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS \"AS IS\" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.";
            richTextBox3.Text += @"IN NO EVENT SHALL THE LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;";
            richTextBox3.Text += @"LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE";
            richTextBox3.Text += @"OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void button2_Click(object sender, EventArgs e)
		{
			new Ack().ShowDialog();
		}

		static string notepadPath = string.Empty;
		static bool bNotepadHappensToBeThere = false;
		static void PrepNotepad()
		{
			if (string.IsNullOrEmpty(notepadPath))
			{
				notepadPath = Path.Combine(Environment.SystemDirectory, "notepad.exe");
				bNotepadHappensToBeThere = File.Exists(notepadPath);
			}
		}
		private void readme_Click(object sender, EventArgs e)
		{
			PrepNotepad();
			if (bNotepadHappensToBeThere)
			{
				string path = Path.GetFullPath(".\\readme.txt");
				if (File.Exists(path))
					System.Diagnostics.Process.Start(notepadPath, path);
				else 
					NCC.CentralizedState.App.ControlLogger.TraceEvent(LogLevels.Error, 22222, "The file '" + path + "' cannot be accessed.");
			}
		}
	}
}
