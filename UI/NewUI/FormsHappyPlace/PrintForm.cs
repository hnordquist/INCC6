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
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace NewUI
{
    public partial class PrintForm : Form
    {
        private System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
        string FileName;
        string[] lines;
        public PrintForm(string FilePath, string Header, string btnText = "")
        {
            InitializeComponent();
            this.Text = Header;
            if (!String.IsNullOrEmpty(btnText))
                PrintParameters.Text = btnText;
            string text = File.ReadAllText(FilePath);
            lines = File.ReadAllLines(FilePath);
            PrintText.Text = text;
            FileName = FilePath;
            doc.PrintPage += new PrintPageEventHandler(this.document_PrintPage);
            this.TopMost = true;
        }

        private void PrintParameters_Click(object sender, EventArgs e)
        {
            PrintDialog = new PrintDialog();
            PrintDialog.Document = doc;
            PrintDialog.Document.DocumentName = FileName;
            PrintDialog.AllowSomePages = true;

            if (PrintDialog.ShowDialog() == DialogResult.OK)
                doc.Print();
        }

        private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = PrintText.Text;
            System.Drawing.Font PrintFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Regular);
            e.Graphics.DrawString(text, PrintFont, System.Drawing.Brushes.Black, 10, 10);
        }

        private void SaveToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "Text files (*.txt) | All files (*.*)";
            SaveFileDialog.DefaultExt = ".txt";
            SaveFileDialog.InitialDirectory = NCCConfig.Config.DefaultPath;
            
            if (SaveFileDialog.ShowDialog () == DialogResult.OK)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(SaveFileDialog.FileName);
                    sw.AutoFlush = true;
                    for (int j = 0; j < lines.Length; j++)
                        sw.WriteLine(lines[j]);
                    sw.Close();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "File error trying to save " + SaveFileDialog.FileName);
                }
            }
            this.Close();
        }
    }
}
