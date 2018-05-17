/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using NCCReporter;
using NewUI.Logging;
namespace NewUI
{

    using NC = NCC.CentralizedState;


    public static class UIIntegration
    {
        public static  Controller Controller;

        /// <summary>
        /// Obtain user-selected folder location for pleasant purposes
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dir">dir to start nav</param>
        /// <returns>Empty if nothing selected or dlg cancelled, selected dir otherwise</returns>
        /// 
        public static string GetUsersFolder(string title, string dir)
        {
            try
            {
                FolderSelect.FolderSelectDialog fsd = new FolderSelect.FolderSelectDialog();
                fsd.Title = title;
                fsd.InitialDirectory = dir;
                if (fsd.ShowDialog())
                    return fsd.FileName;
                else
                    return string.Empty;
            }
            catch (Exception) { }
            return string.Empty;
        }

        public static DialogResult GetUsersFile(string title, string dir, string name, string ext, string ext2 = "")
        {
            System.Windows.Forms.OpenFileDialog RestoreFileDialog = new System.Windows.Forms.OpenFileDialog();
            List<string> paths = new List<string>();
            RestoreFileDialog.CheckFileExists = false;
            RestoreFileDialog.DefaultExt = "ext";
            RestoreFileDialog.Filter = name + " files (." + ext + ")|*." + ext;
            if (!String.IsNullOrEmpty(ext2))
            {
                RestoreFileDialog.Filter += "| (." + ext2 + ")|*." + ext2;
            }
            RestoreFileDialog.Filter += "|All files (*.*)|*.*";
            RestoreFileDialog.InitialDirectory = NC.App.AppContext.FileInput;
            RestoreFileDialog.Title = title;
            RestoreFileDialog.Multiselect = false;
            RestoreFileDialog.RestoreDirectory = true;
            DialogResult r = DialogResult.No;
            r = RestoreFileDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                foreach (string s in RestoreFileDialog.FileNames)
                {
                    paths.Add(s);
                }
                NC.App.AppContext.FileInputList = paths;
                NC.App.AppContext.FileInput = null;  // no explicit folder System.IO.Path.GetDirectoryName(paths[0]);
            }
            return r;
        }

        public static DialogResult GetUsersFilesFolder(string title, string dir, string name, string ext, string ext2 = "")
        {
            FoldorFile.FileOrFolderChoice choice = ChooseFiles();
            if (choice == FoldorFile.FileOrFolderChoice.Cancel)
                return DialogResult.Cancel;

            if (choice == FoldorFile.FileOrFolderChoice.Folder)
            {
                string xs = UIIntegration.GetUsersFolder("Select folder", dir);
                if (!String.IsNullOrEmpty(xs))
                {
                    NC.App.AppContext.FileInput = xs;
                    NC.App.AppContext.FileInputList = null;  // no explicit file list
                    NC.App.AppContext.modified = true;
                    NC.App.LMBD.UpdateLMINCCAppContext();
                    return DialogResult.OK;                }
                else
                    return DialogResult.Cancel;
            }
            // choice == files
            System.Windows.Forms.OpenFileDialog RestoreFileDialog = new System.Windows.Forms.OpenFileDialog(); 
            List<string> paths = new List<string>();
            RestoreFileDialog.CheckFileExists = false;
            RestoreFileDialog.DefaultExt = "ext";
            RestoreFileDialog.Filter = name + " files (." + ext + ")|*." + ext ;
            if (!String.IsNullOrEmpty(ext2))
            {
                RestoreFileDialog.Filter += "| (." + ext2 + ")|*." + ext2;
            }
            RestoreFileDialog.Filter += "|All files (*.*)|*.*";
            RestoreFileDialog.InitialDirectory = NC.App.AppContext.FileInput;
            RestoreFileDialog.Title = title;
            RestoreFileDialog.Multiselect = true;
            RestoreFileDialog.RestoreDirectory = true;
            DialogResult r = DialogResult.No; 
            r = RestoreFileDialog.ShowDialog();
            if (r == DialogResult.OK)
            { 
                foreach (string s in RestoreFileDialog.FileNames)
                {
                    paths.Add(s);
                }
                FileSel fs = new FileSel(paths);
                r = fs.ShowDialog();
                if (r == DialogResult.OK)
                {
                    NC.App.AppContext.FileInputList = paths;
                    NC.App.AppContext.FileInput = null;  // no explicit folder System.IO.Path.GetDirectoryName(paths[0]);
                    NC.App.AppContext.modified = true;
                    NC.App.LMBD.UpdateLMINCCAppContext();
                }
            }
            return r;
        }

        static public FoldorFile.FileOrFolderChoice ChooseFiles()
        {
            FoldorFile x = new FoldorFile();
            x.TopMost = true;
            x.ShowDialog();
            return x.Choice;
        }

    }

    public partial class MainWindow : Window
    {

        public void OneSecondGuiUpdate(object source, System.Timers.ElapsedEventArgs e)
        {
            if (UIIntegration.Controller.updateGUIWithNewData || UIIntegration.Controller.updateGUIWithChannelRatesData)
            {
                UIIntegration.Controller.UpdateGUIWithNewData(this);
            }
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UIIntegration.Controller = new Controller();
			if (UIIntegration.Controller.Initialize(this))
			{
				EnableLog.IsChecked = NC.App.AppContext.Logging;
				logLevels.SelectedIndex = NC.App.AppContext.LevelAsUInt16;
				logResults.SelectedIndex = NC.App.AppContext.LogResults;
				((Window)sender).Title = NCC.IntegrationHelpers.GetAppTitle();
				// Find the WPF trace listener
				WPFTraceListener listener = null;

				foreach (LMLoggers.AppSection source in Enum.GetValues(typeof(LMLoggers.AppSection)))
				{
					foreach (TraceListener l in NC.App.Logger(source).TS.Listeners)
					{
						if (l is WPFTraceListener)
						{
							listener = (WPFTraceListener)l;
							break;
						}
					}
				}
				if (listener != null)
				{
					loggingListBox.DataContext = listener;
				}
				Splash pauseme = new Splash();
				pauseme.ShowDialog();
			}
			else
			{
				EnableLog.IsEnabled = false;
				System.Windows.Controls.DockPanel d = ((System.Windows.Controls.DockPanel)Content);
				System.Windows.Controls.Menu m = (System.Windows.Controls.Menu)d.Children[0];
				m.IsEnabled = false;
				System.Windows.MessageBox.Show(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss") + " App Error\r\n" + NC.App.Pest.IsItThereStr, "ERROR");
			}
		}

		delegate void SetBooleanCB(bool enable);

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NC.App.AppContext != null && NC.App.AppContext.modified)
                NC.App.LMBD.UpdateLMINCCAppContext(); // out to the DB with you!
            if (UIIntegration.Controller != null)
                UIIntegration.Controller.Close();
        }
    }

}