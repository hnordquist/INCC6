using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

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
            stamp.DoFileTimeUpdate(args[1]);
            stamp.ProcessTheTimeFile();
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

        public void DoFileTimeUpdate(string ftimeexe)
        {
            Console.WriteLine($"ftimeexe {DevTreeRoot}");
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ftimeexe, DevTreeRoot);
            psi.CreateNoWindow = true;
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            p.WaitForExit();
        }


        public struct ChristophsData
        {
            public string name;
            public DateTime dt;
            long ftime;
            int size;
            string hash;
            public ChristophsData(string n, string t, string ft, string sz, string h)
            {
                name = n; hash = h;
                dt = DateTime.Parse(t);
                ftime = long.Parse(ft);
                size = int.Parse(sz);
            }

            public static int Compare(ChristophsData x, ChristophsData y)
            {
                return DateTime.Compare(x.dt, y.dt);
            }

            static string[] projects = { "RepDB", "Defs", "NCCCtrl", "Cmd", "UI" };
            static string[] filetypes = { ".cs", ".txt", ".config", ".sql" };
            const string exclu = "AssemblyInfo.cs";


            public static bool AssemblyVersionFiles(ChristophsData f)
            {
                return (f.name.EndsWith(exclu, StringComparison.InvariantCultureIgnoreCase));// || f.name.Contains("bin\\") || f.name.Contains("obj\\");
            }
            public static bool NotMyProjects(ChristophsData f)
            {
                bool res = true;
                foreach (string s in projects)
                {
                    if (f.name.StartsWith(s))
                    {
                        res = false;
                        break;
                    }
                }
                return res;
            }
            public static bool NotMyFiles(ChristophsData f)
            {
                bool res = true;
                foreach (string s in filetypes)
                {
                    if (f.name.EndsWith(s))
                    {
                        res = false;
                        break;
                    }
                }
                return res;
            }
        }
        List<ChristophsData> Files = new List<ChristophsData>();
        public void ProcessTheTimeFile()
        {
            XDocument doc = XDocument.Load(Path.Combine(DevTreeRoot, "FileTimes.ftm"));
            foreach (XElement sectionElement in doc.Root.Elements("file"))
                Files.Add(new ChristophsData(sectionElement.Attribute("name").Value, sectionElement.Attribute("time").Value, sectionElement.Attribute("filetime").Value, sectionElement.Attribute("size").Value, sectionElement.Attribute("hash").Value));
            Files.RemoveAll(ChristophsData.NotMyProjects);
            Files.RemoveAll(ChristophsData.NotMyFiles);
            Files.RemoveAll(ChristophsData.AssemblyVersionFiles);
            Files.Sort(ChristophsData.Compare);
        }

        public void Process()
        {

            Console.WriteLine($"Last modified timestamp is {Files[Files.Count - 1].dt} for file \r\n{Files[Files.Count - 1].name}");

            // calculate number of days since last release, and also get #of seconds in current day div 2
            int seconds = (int)(Math.Round(Files[Files.Count - 1].dt.TimeOfDay.TotalSeconds)) / 2;
            TimeSpan ts = (Files[Files.Count - 1].dt - LastRelDT);
            int days = (Files[Files.Count - 1].dt - LastRelDT).Days;

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

    }

}
