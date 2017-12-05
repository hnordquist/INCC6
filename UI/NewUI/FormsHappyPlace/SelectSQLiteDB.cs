using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NewUI.FormsHappyPlace
{

    using NC = NCC.CentralizedState;

    public partial class SelectSQLiteDB : Form
    {
        public SelectSQLiteDB()
        {
            InitializeComponent();
            textBox1.Text = GetDBFileFromConxString(NC.App.Pest.cfg.MyDBConnectionString);
            _newpath = textBox1.Text;
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = GetUsersFile(ref _newpath, "Select a SQLite database file", _newpath, "SQLite database", "sqlite", "dat");
            if (dr == DialogResult.OK)
                textBox1.Text = _newpath;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // The provider name is invariant, we do not switch to SQL Server here (yet), so ...
            // NEXT: implement SQL Server connection selection dialog (as in IRAP)
            // TODO: implement SQLite to SQL Server and back again (as in IRAP)
            string res = NCCConfig.DBConfig.ReplaceDataSourceInSQLiteConnectionString(_newpath, NC.App.Config.DB.MyDBConnectionString);
            if (!string.IsNullOrEmpty(res) && !res.Equals(NC.App.Config.DB.MyDBConnectionString))
            {
                NC.App.Config.DB.MyDBConnectionString = res;
                if (!NC.App.LoadPersistenceConfig(NC.App.Config.DB))
                    NC.App.CollectLogger.TraceEvent(NCCReporter.LogLevels.Warning, 764, "Something failed switching to the selected database");
                else
                    NC.App.CollectLogger.TraceInformation("Database switched to " + _newpath);

                // dev note: connection string changes, and all App.Config attibutes changes, are preserved upon exit
            }
            Close();
        }

        private string _newpath;

        public static DialogResult GetUsersFile(ref string newpath, string title, string dir, string name, string ext, string ext2 = "")
        {
            OpenFileDialog RestoreFileDialog = new OpenFileDialog();
            List<string> paths = new List<string>();
            RestoreFileDialog.CheckFileExists = false;
            RestoreFileDialog.DefaultExt = ext;
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
                    newpath = s; //take the last
                }
            }
            return r;
        }

        private string GetDBFileFromConxString(string ConxString)
        {
            Match m = Regex.Match(ConxString, "^Data\\sSource(\\s)*=(\\s)*(?<DataSourceName>([^;]*))", RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            char[] trimchars = new char[1] { '\"' };
            if (m.Success)
                return (m.Groups[3].Value.TrimStart(trimchars).TrimEnd(trimchars));
            else
                return String.Empty;
        }

    }
}
