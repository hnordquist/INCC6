using System;
using System.Collections.Generic;
using System.IO;

namespace BuildStamp
{
    class Program
    {
        static void Main(string[] args)
        {
            Stamp stamp = new Stamp();
            stamp.SetLastDT("Mar 23, 2017");  // from IAEA readme
            stamp.MinorVersion = 18;  // change this after a major release improvement 
            stamp.DevTreeRoot = args[0];
            stamp.Process();
        }
    }

    public class Stamp
    {

        public DateTime LastRelDT;  // last release date, obtained from somewhere
        public void SetLastDT(string dt)
        {
            LastRelDT = DateTime.Parse(dt);
        }

        public string DevTreeRoot { set; protected get; }
        public int MinorVersion { set; protected get; }

        const string exclu = "AssemblyInfo.cs";
        string[] projects = { "RepDB", "Defs", "NCCCtrl", "Cmd", "UI" };
        string[] filetypes = { "*.cs", "*.txt", "*.config", "*.sql" };
        public void Process()
        {
            List<string> list = new List<string>();
            foreach (string p in projects)
            {
                foreach (string f in filetypes)
                {
                    IEnumerable<string> sublist = Directory.EnumerateFiles(Path.Combine(DevTreeRoot, p), f, SearchOption.AllDirectories);
                    list.AddRange(sublist);
                }
            }
            list.RemoveAll(Ignore);
            List<Tuple<DateTime, string>> dtlist = new List<Tuple<DateTime, string>>();
            foreach (string s in list)
                dtlist.Add(Tuple.Create(Directory.GetLastWriteTimeUtc(s), s));
            dtlist.Sort(Compare);
            Console.WriteLine($"Last modified timestamp is {dtlist[dtlist.Count - 1].Item1} for file \r\n{dtlist[dtlist.Count - 1].Item2}");

            // calculate number of days since last release, and also get #of seconds in current day div 2
            int seconds = (int)(Math.Round(dtlist[dtlist.Count - 1].Item1.TimeOfDay.TotalSeconds)) / 2;
            TimeSpan ts = (dtlist[dtlist.Count - 1].Item1 - LastRelDT);
            int days = (dtlist[dtlist.Count - 1].Item1 - LastRelDT).Days;

            string stamp = $"[assembly: AssemblyVersion(\"6.{MinorVersion}.{days}.{seconds}\")]";
            bool updated = false;

            IEnumerable<string> assemblylist = Directory.EnumerateFiles(DevTreeRoot, "AssemblyInfo.cs", SearchOption.AllDirectories);
            foreach (string fp in assemblylist)
            {
                bool done = false;
                string[] lines = File.ReadAllLines(fp);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("[assembly: AssemblyVersion"))
                    {
                        if (!string.Equals(stamp, lines[i]))
                        {
                            lines[i] = $"[assembly: AssemblyVersion(\"6.{ MinorVersion}.{days}.{seconds}\")]";
                            done = true;
                        }
                        break;
                    }
                }
                if (done)
                {
                    updated = true;
                    File.WriteAllLines(fp, lines);
                    Console.WriteLine($"{fp} updated to 6.{MinorVersion}.{days}.{seconds}");
                }
            }
            if (!updated)
                Console.WriteLine($"AssemblyVersion 6.{MinorVersion}.{days}.{seconds} unchanged");
        }

        static bool Ignore(string f)
        {
            return (f.EndsWith(exclu, StringComparison.InvariantCultureIgnoreCase)) || f.Contains("bin\\") || f.Contains("obj\\");
        }

        public static int Compare(Tuple<DateTime, string> x, Tuple<DateTime, string> y)
        {
            return DateTime.Compare(x.Item1, y.Item1);
        }


    }

}
