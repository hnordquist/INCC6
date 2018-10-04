using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AnalysisDefs;
using INCCCore;
using System.Globalization;

namespace NCCTester
{
    class NCCTester
    {

        public static String testFileDir = "";
        public static String testFile = "";
        public static String testOutputDir = "";
        public static Dictionary<String, AssaySelector.MeasurementOption> inputFiles = new Dictionary<String, AssaySelector.MeasurementOption>();
        public static String resultsFile = "";
        public static StreamWriter results;

        public static String version;
        public static Measurement testMeasurement;
        public static MeasId measID;
        public static AnalysisMethod am;
        public static AlphaBeta thisSetting;

        //Max length of mult dist over all cycles
        public static ulong maxmax;
        //Results loaded from file.
        public static Multiplicity mult;
        public static INCCResult TestVersionResult = new INCCResult();
        public static INCCMethodResult TestVersionMethodResult = new INCCMethodResult();
        public static INCCAnalysisParams.active_mult_rec active_mult_params = new INCCAnalysisParams.active_mult_rec();
        public static INCCMethodResults.results_active_mult_rec active_mult_results = new INCCMethodResults.results_active_mult_rec();
        public static INCCMethodResults.results_multiplicity_rec passive_mult_results = new INCCMethodResults.results_multiplicity_rec();
        public static INCCAnalysisParams.multiplicity_rec passive_mult_params = new INCCAnalysisParams.multiplicity_rec();
        public static INCCAnalysisParams.cal_curve_rec cal_curve_params = new INCCAnalysisParams.cal_curve_rec();
        public static INCCAnalysisParams.active_rec active_cal_curve_params = new INCCAnalysisParams.active_rec();
        public static INCCMethodResults.results_cal_curve_rec passive_cal_results = new INCCMethodResults.results_cal_curve_rec();
        public static INCCMethodResults.results_active_rec active_cal_results = new INCCMethodResults.results_active_rec();
        public static INCCAnalysisParams.curium_ratio_rec cm_ratio_cev_rec = new INCCAnalysisParams.curium_ratio_rec();
        public static INCCAnalysisParams.cm_pu_ratio_rec cm_ratio_params = new INCCAnalysisParams.cm_pu_ratio_rec();
        public static INCCMethodResults.results_curium_ratio_rec cm_ratio_results = new INCCMethodResults.results_curium_ratio_rec();
        public static INCCAnalysisParams.known_alpha_rec known_alpha_params = new INCCAnalysisParams.known_alpha_rec();
        public static INCCMethodResults.results_known_alpha_rec known_alpha_results = new INCCMethodResults.results_known_alpha_rec();

        //INCC6 Results
        public static INCCResult InputResult = new INCCResult();
        public static INCCMethodResult InputMethodResult = new INCCMethodResult();

        //All the stuff we need to calculate
        public static AcquireParameters acq;
        public static AnalysisDefs.TestParameters tp = new TestParameters();
        public static AnalysisDefs.NormParameters np = new NormParameters();
        public static AnalysisDefs.Isotopics iso = new AnalysisDefs.Isotopics();
        public static AnalysisDefs.Isotopics CalculatedIsotopics = new AnalysisDefs.Isotopics();
        public static AnalysisDefs.BackgroundParameters bp = new BackgroundParameters();
        public static AnalysisDefs.HVCalibrationParameters hvp = new HVCalibrationParameters();
        public static bool active = false;
        public static MeasOptionSelector mos;
        public static Stratum strat = new Stratum();

        //Temp stuff to do our work and read stuff in.
        public static StreamReader sr;
        public static String line = String.Empty;
        public static int lineCount = 0;
        public static String label;
        public static string[] formats = new string[] { "yy.MM.dd HH:mm:ss" };
        public static double val = 0.0;
        public static double err = 0.0;
        public static double val2 = 0.0;// Probably not needed
        public static double err2 = 0.0;
        public static ulong Ulon = 0;

        //Regex.... Expressions used to parse results files
        //This regex gets a <label>: <value>
        public static Regex labelValue = new Regex("^\\s*([\\w\\s(\\-*\\%-+*)]+):\\s+([\\w\\s:.+(\\=*\\^*\\-*#\\d)]+)");
        //public static Regex labelValue = new Regex("^\\s*([\\w\\s()]+):\\s+([\\w\\s:.+]+)");//Old won't match lines with = an ^ in <value>
        //This one gets a <label>: <tuplepart1> +- <tuplepart2>
        public static Regex tuple = new Regex("^\\s*([\\w\\s(\\-*\\%/*)]+):\\s+([-?\\w\\s.]+)[+-]+\\s+([-?\\w\\s.]+)");
        //public static Regex tuple = new Regex("^\\s+([\\w\\s/]+):\\s+([-?\\w\\s.]+)[+-]+\\s+([-?\\w\\s.]+)");//This regex can't match <label>'s with parenthesis, or, '-', or '%'
        //This one gets a <label>: <tuplepart1> +- <tuplepart2> <tuplepart3> +- <tuplepart4> 
        public static Regex tupleFiveGroup = new Regex("^\\s+([\\w\\s/]+):\\s+([-?\\w\\s.]+)[+-]+\\s+([-?\\w\\s.]+)\\s+([-?\\w\\s.]+)[+-]+\\s+([-?\\w\\s.]+)");
        //This one gets <label>: <tuplepart1> <tuplepart2>
        public static Regex twodates = new Regex("^\\s+([\\w\\s]+):\\s+(\\d{2}.\\d{2}.\\d{2})\\s+(\\d{2}.\\d{2}.\\d{2})");
        //Triad of numbers
        public static Regex ThreeNums = new Regex("^\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)");
        //cycle table
        public static Regex cycleData = new Regex("^\\s*(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\w+)");
        public static Regex ratesData = new Regex("^\\s*(\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(\\w+)");
        //Distribution
        public static Regex cycleDistHeader = new Regex("^Cycle (\\d+)*");

        //Keep track of test time.
        public static DateTime start;
        public static DateTime end;
        public static TimeSpan ts;
        public static bool verbose = false;
        public static int numMethods;

        public static AssaySelector.MeasurementOption currOption;

        static void Main(string[] args)
        {
            //Set the tolerance. Read from args.
            Double.TryParse (args[3], out val);
            CompareTools.tolerance = val;
            try
            {
                start = DateTime.Now;
                bool dirFail = false;
                if (args != null)
                {
                    testOutputDir = args[0];
                    testFileDir = args[1];
                    //Make sure they are directories
                    DirectoryInfo di = new DirectoryInfo(testOutputDir);
                    if (!di.Exists)
                    {
                        //output dir. Create if not there.
                        Directory.CreateDirectory(testOutputDir);
                        string s = Path.Combine(testOutputDir, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                        Directory.CreateDirectory(s);
                        testOutputDir = s;
                    }
                    resultsFile = Path.Combine(testOutputDir, DateTime.Now.ToString("yyyyMMddTHHmmss") + ".log");
                    di = new DirectoryInfo(testFileDir);
                    if (!di.Exists)//Bad, we have no files to test.
                    {
                        dirFail = true;
                        end = DateTime.Now;
                        ts = end - start;
                        return;
                    }
                    else
                    {
                        WalkDirectoryTree(di);
                    }
                }
                results = new StreamWriter(resultsFile, true);
                Setup();
                if (dirFail)
                {
                    PrintLine("Failure finding input directory");
                    //results.Close();
                    return;
                }
                PrintLine("***********************************************************");

                //What else here? HN
                PrintLine(String.Format("Starting INCC Algorithms test at {0}", start.ToLongTimeString()));
                PrintLine(String.Format("OS platform for test {0}", Environment.OSVersion.VersionString));
                PrintLine(String.Format("NCCCore Version {0}", typeof(INCCCore.INCCAnalysis).Assembly.GetName().Version));
                PrintLine(String.Format("Defs Version {0}", typeof(AnalysisDefs.INCCAnalysisParams).Assembly.GetName().Version));
                PrintLine(String.Format("NCCCtrl Version {0}", typeof(AnalysisDefs.MeasurementExtensions).Assembly.GetName().Version));
                PrintLine(String.Format("RepDB Version {0}", typeof(DB.DB).Assembly.GetName().Version));
                PrintLine(String.Format("{0} input files found in input directory", inputFiles.Count));

                PrintLine(String.Format("This test was run using a tolerance for equality of {0}", CompareTools.tolerance));

                PrintLine("***********************************************************");
                PrintLine("");

                verbose = args[2] == "verbose";
                int testNum = 0;

                foreach (KeyValuePair<String, AssaySelector.MeasurementOption> testFile in inputFiles)
                {
                    DateTime TestStart = DateTime.Now;
                    PrintLine("***********************************************************");
                    PrintLine(String.Format("Loading test file {0}/{1} at {2}", testNum + 1, inputFiles.Count, TestStart.ToLongTimeString()));
                    PrintLine(String.Format("Test file name: {0}: ", testFile.Key));
                    currOption = testFile.Value;
                    LoadMeasurementFromFile(testFile.Key);
                    testMeasurement.INCCAnalysisState.Methods.selector = new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type);

                    PrintLine("Measurement created from file");
                    PrintLine("Copying parameters and raw data for recalculation");

                    //Copy to new measurement for calculation/comparison
                    Measurement newResultMeasurement = new Measurement(testFile.Value, null);
                    newResultMeasurement = BuildMeasurement(testMeasurement);
                    newResultMeasurement.Detector.Id.source = DetectorDefs.ConstructedSource.INCCTransfer;
                    //Problem if no multiplicity distributions. Fix here.
                    MultiplicityCountingRes Checkmcr = ((MultiplicityCountingRes)newResultMeasurement.CountingAnalysisResults[mult]);
                    bool noMult = false;
                    if (Checkmcr.MaxBins == 1)
                    {
                        ((MultiplicityCountingRes)newResultMeasurement.CountingAnalysisResults[mult]).MaxBins = 0;
                        noMult = true;
                    }
                    //This seems to be adding the cycle that failed the outlier test. Why?
                    //This is equivalent code to CalculateMeasurement
                    if (newResultMeasurement.Cycles.GetValidCycleCount() > 0) // INCC5 Pu mass calcs
                    {
                        newResultMeasurement.OutlierProcessing(); // summary pass at the end of all the cycles
                        newResultMeasurement.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
                        newResultMeasurement.CalcAvgAndSums();
                        newResultMeasurement.CalculateResults();
                    }
                    else // everything else
                        newResultMeasurement.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
                    PrintLine("Measurement recalculated");

                    bool compare = true;
                    // Commented out. Was for diagnostics when writing parse code
                    //CompareParameters(testMeasurement, newResultMeasurement);
                    //PrintLine(compare ? "Parameters equal" : "Parameters not equal -- FAIL");

                    bool testCompare = true;
                    testCompare = CompareRates(testMeasurement, newResultMeasurement);
                    
                    compare = testCompare && testCompare;

                    if (!testFile.Value.Equals (AssaySelector.MeasurementOption.background) && !testFile.Value.Equals(AssaySelector.MeasurementOption.rates))
                    // No method results for rates only or background measurements
                    {
                        testCompare = CompareResults(testMeasurement, newResultMeasurement);
                        compare = testCompare && testCompare;
                    }

                    DateTime TestEnd = DateTime.Now;
                    TimeSpan TestTime = TestEnd - TestStart;
                    PrintLine(String.Format("Test {0}/{1} ended at {2}, {3} total milliseconds", testNum + 1, inputFiles.Count, TestStart.ToLongTimeString(), TestTime.Milliseconds));
                    testNum++;
                    PrintLine("***********************************************************");
                    PrintLine("");
                    ClearMeasurement(testMeasurement);
                }

                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in Main: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
            }

        }

        static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files in the root folder
            try
            {
                string[] extensions = new string[] { ".ver", ".rts", ".cal", ".bkg"};
                foreach (FileInfo f in root.EnumerateFiles().ToArray())
                {
                    switch(f.Extension.ToLower())
                    {
                        case ".ver":
                            inputFiles.Add(f.FullName,AssaySelector.MeasurementOption.verification);
                            break;
                        case ".rts":
                            inputFiles.Add(f.FullName, AssaySelector.MeasurementOption.rates);
                            break;
                        case ".cal":
                            inputFiles.Add(f.FullName, AssaySelector.MeasurementOption.calibration);
                            break;
                        case ".bkg":
                            inputFiles.Add(f.FullName, AssaySelector.MeasurementOption.background);
                            break;
                        default:
                            continue;
                    }
                        
                }


                //None found in root, but need to recurse through subdirs
                subDirs =  root.EnumerateDirectories().ToArray();
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in WalkDirectoryTree: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in WalkDirectoryTree: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }

            if (subDirs.Count() > 0)
            {
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
        public static void Setup()
        {
            //Set up all the data structs needed for test.
            mult = new Multiplicity(FAType.FAOn);
            acq = new AcquireParameters();
            // Verification by default, but is set during read for background, et. al.
            measID = new MeasId(currOption);
            INCCResults.results_rec rec = new INCCResults.results_rec();
            hvp = new HVCalibrationParameters();
            testMeasurement = new Measurement(currOption);
            testMeasurement.ResultsFiles = new ResultFiles();
            testMeasurement.AnalysisParams = new CountingAnalysisParameters();
            testMeasurement.INCCAnalysisState = new INCCAnalysisState();
            testMeasurement.INCCAnalysisState.Methods = new AnalysisMethods();
            numMethods = 0;
        }

        public static void ClearMeasurement(Measurement m)
        {
            //We reuse the same measurement object. Clear it out between runs.
            m.Cycles.Clear();
            m.AnalysisParams.Clear();
            m.CountingAnalysisResults.RemoveAll();
            testMeasurement.AnalysisParams = new CountingAnalysisParameters();
            testMeasurement.INCCAnalysisState.Methods = new AnalysisMethods();
            testMeasurement.INCCAnalysisState.Results = new INCCResults();
            testMeasurement.INCCAnalysisResults.MethodsResults = new Dictionary<SpecificCountingAnalyzerParams, INCCMethodResults>();
            testMeasurement.INCCAnalysisResults.MethodsResults.Clear();
            testMeasurement.INCCAnalysisResults.Clear();
            m.INCCAnalysisState.ClearINCCAnalysisResults();
            m.INCCAnalysisResults.Clear();
            m.INCCAnalysisResults.Remove(mos);
            m.Detectors.Clear();
            numMethods = 0;
        }
        public static void LoadMeasurementFromFile(string fileName)
        {
            try
            {
                using (sr = new StreamReader(fileName))
                {
                    //Let's skip blank lines until we get to the header
                    line = sr.ReadLine();
                    while (line == "")
                    {
                        line = sr.ReadLine();
                        lineCount++;
                    }
                    string[] vers = line.Split();
                    version = vers[1];
                    line = sr.ReadLine();//Consume blank line
                    line = sr.ReadLine();//First line of header
                    lineCount += 3;
                    ReadHeader();
                    INCCSelector sel = new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type);
                    while (sr.Peek() != -1)
                    {
                        switch (line.Trim(' '))
                        {
                            case "":
                            case "Detector configuration": // This says Passive or Active in a rates file. Doesn't matter.
                                line = sr.ReadLine();
                                lineCount++;
                                continue;
                            case "Passive summed raw data":
                            case "Active summed raw data":
                                ReadSummedData();
                                break;
                            case "Active multiplicity distributions":
                            case "Passive multiplicity distributions":
                                ReadMultDistributions();
                                break;
                            case "Passive summed multiplicity distributions":
                            case "Active summed multiplicity distributions":
                                ReadSummedMult();
                                break;
                            case "Active cycle data":
                            case "Passive cycle data":
                            case "Passive cycle raw data":
                            case "Passive cycle DTC rate data":
                                ReadCycleData();
                                break;
                            case "Number Passive cycles":
                                //Of course, not all INCC files are standard.
                                ReadSummedData(line.TrimStart());
                                break;
                            case "Results":
                            case "Active results":
                            case "Passive results":
                                ReadResults();
                                break;
                            //These should be compared at some point.
                            case "Warning messages":
                                ReadWarnings();
                                break;
                            case "Passive error messages":
                            case "Active error messages":
                                ReadErrors();
                                break;
                            case "Passive messages":
                                ReadMessages();
                                break;
                            case "Active multiplicity calibration parameters":
                                am = AnalysisMethod.ActiveMultiplicity;
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.ActiveMultiplicity] = true;
                                SetMethodPreference(am);
                                ReadActiveMultiplicityCalibration();
                                numMethods++;
                                break;
                            case "Active multiplicity results":
                                ReadActiveMultiplicityResults();
                                break;
                            case "Passive multiplicity results using weighted coefficients":
                            case "Passive multiplicity results":
                                ReadPassiveMultiplicityResults();
                                break;
                            case "Passive calibration curve calibration parameters":
                                am = AnalysisMethod.CalibrationCurve;// is this correct?
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.CalibrationCurve] = true;
                                SetMethodPreference(am);
                                ReadPassiveCalibrationCurveCalibration();
                                numMethods++;
                                break;
                            case "Passive multiplicity calibration parameters":
                                am = AnalysisMethod.Multiplicity;// is this correct?
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.Multiplicity] = true;
                                SetMethodPreference(am);
                                ReadPassiveMultiplicityCalibration();
                                numMethods++;
                                break;
                            case "Collar calibration parameters":
                                am = AnalysisMethod.CollarAmLi; // Assume AmLi to begin
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.CollarAmLi] = true;
                                SetMethodPreference(am);
                                ReadCollarCalibration();
                                numMethods++;
                                break;
                            case "Passive calibration curve results":
                                ReadPassiveCalibrationCurveResults();
                                break;
                            case "Analysis based on doubles rate."://this is getting skipped because the while loop in Passive Calibration curve parameters reads line and then the while loop rejects and reads next line
                                break;
                            //These denote the primary result, but we don't really care here.
                            case "PRIMARY RESULT":
                                break;
                            case "END PRIMARY RESULT":
                                break;
                            case "Known alpha results":
                                am = AnalysisMethod.KnownA;
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.KnownA] = true;
                                SetMethodPreference(am);
                                ReadKnownAlphaResults();
                                numMethods++;
                                break;
                            case "Active calibration curve results":
                                am = AnalysisMethod.Active;
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.Active] = true;
                                SetMethodPreference(am);
                                ReadActiveCalibrationResults();
                                numMethods++;
                                break;
                            case "Active calibration curve calibration parameters":
                                ReadActiveCalibrationParams();
                                break;
                            case "Curium Ratio parameters":
                            case "Curium ratio calibration parameters":
                                am = AnalysisMethod.CuriumRatio;
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.CuriumRatio] = true;
                                SetMethodPreference(am);
                                ReadCmRatioCalibration();
                                numMethods++;
                                break;
                            case "Curium ratio results":
                                am = AnalysisMethod.CuriumRatio;
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.CuriumRatio] = true;
                                SetMethodPreference(am);
                                ReadCmRatioResults();
                                numMethods++;
                                break;
                            case "Known alpha calibration parameters":
                                ReadKnownAlphaCalibration();
                                break;
                            case "Add a source calibration parameters":
                                am = AnalysisMethod.AddASource;// is this correct?
                                testMeasurement.INCCAnalysisState.Methods.choices[(int)AnalysisMethod.AddASource] = true;
                                SetMethodPreference(am);
                                ReadAddASourceCalibration();
                                numMethods++;
                                break;
                            case "Add a source results":
                                ReadAddASourceResults();
                                break;
                            case "Collar results":
                                ReadCollarResults();
                                break;
                            default:
                                //We will see this in log if there is something wrong with parsing code.
                                PrintLine(String.Format("Unknown label found in input file {0}.", line));
                                break;
                        }
                        line = sr.ReadLine();
                        lineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in LoadMeasurementFromFile: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
            }
        }

        public static void SetMethodPreference(AnalysisMethod cc)
        {
            if (numMethods == 0)
                testMeasurement.INCCAnalysisState.Methods.Normal = cc;
            else if (numMethods == 1)
                testMeasurement.INCCAnalysisState.Methods.Backup = cc;
            else if (numMethods == 3)
                testMeasurement.INCCAnalysisState.Methods.Auxiliary = cc;
        }
        public static void ReadWarnings()
        {
            string s = sr.ReadLine();
            s = sr.ReadLine();
            while (s != "" && s != null)
            {
                testMeasurement.AddWarningMessage(s, 9999, mult);
                s = sr.ReadLine();
            }
        }

        public static void ReadErrors()
        {
            string s = sr.ReadLine();
            s = sr.ReadLine();
            while (s != "" && s != null)
            {
                testMeasurement.AddErrorMessage(s, 9999, mult);
                s = sr.ReadLine();
            }
        }
        public static void ReadMessages()
        {
            string s = sr.ReadLine();
            s = sr.ReadLine();
            List<MeasurementMsg> list = new List<MeasurementMsg>();
            while (s != "" && s != null)
            {
                list.Add(new MeasurementMsg(NCCReporter.LogLevels.Info, 9999, s));
                s = sr.ReadLine();
            }

            testMeasurement.AddMessage(list,NCCReporter.LogLevels.Info,9999,"",new DateTimeOffset(DateTime.Now));
        }
        public static void ReadHeader()//Also reads background and isotopics if any
        {
            //Read all the header stuff. Always delineated by these two regex

            String type = String.Empty;
            String elec_id = String.Empty;
            try
            {
                while ((line.Contains(":") || line == "" || line == "Passive messages") && !line.Contains("summed raw data"))
                {
                    Match m = labelValue.Match(line);
                    label = m.Groups[1].Value;
                    switch (label.TrimStart())
                    {
                        case "":
                            line = sr.ReadLine();
                            lineCount++;//Consume extra spaces in header.
                            continue;
                        case "Number Passive cycles":
                            //Of course, some of the test files are different. SIGHS.
                            ReadSummedData(line);
                            break;
                        case "Detector configuration":
                            //Not sure needed for background. TODO
                            break;
                        case "Facility":
                            testMeasurement.AcquireState.facility = new INCCDB.Descriptor(m.Groups[2].Value, "Facility for test.");
                            break;
                        case "Material balance area":
                            testMeasurement.AcquireState.mba = new INCCDB.Descriptor(m.Groups[2].Value, "Test MBA");
                            break;
                        case "Detector type":
                            testMeasurement.Detector = new Detector();
                            testMeasurement.Detector.Id.SetSRType(m.Groups[2].Value);
                            break;
                        case "Detector id":
                            testMeasurement.AcquireState.detector_id = m.Groups[2].Value;
                            break;
                        case "Electronics id":
                            elec_id = m.Groups[2].Value;
                            testMeasurement.Detector.Id.SetIdDetails(testMeasurement.AcquireState.detector_id, m.Groups[2].Value, "", testMeasurement.Detector.Id.SRType);
                            Detector d = testMeasurement.Detector;
                            testMeasurement.Detector = new Detector(d.Id, mult, new AlphaBeta());
                            break;
                        case "Virtual shift register":
                            break;
                        case "Inventory change code":
                            testMeasurement.AcquireState.inventory_change_code = m.Groups[2].Value;
                            break;
                        case "I / O code":
                            break;
                        case "Measurement date":
                            DateTime dt;
                            DateTime.TryParseExact(m.Groups[2].Value, formats, null, DateTimeStyles.AllowInnerWhite, out dt);//17.01.03  15:00:48
                            testMeasurement.MeasDate = dt;
                            break;
                        case "Results file name":
                            testMeasurement.ResultsFiles.Add(new ResultFile(m.Groups[2].Value));
                            break;
                        case "Inspection number":
                            testMeasurement.AcquireState.campaign_id = m.Groups[2].Value;
                            break;
                        case "Item id":
                            MeasId mid = new MeasId(m.Groups[2].Value);
                            mid.MeasOption = currOption;
                            testMeasurement.MeasurementId = mid;
                            testMeasurement.AcquireState.item_id = m.Groups[2].Value;
                            break;
                        case "Stratum id":
                            testMeasurement.AcquireState.stratum_id = new INCCDB.Descriptor(m.Groups[2].Value, "Test strata");
                            break;
                        case "Bias uncertainty":
                            Double.TryParse(m.Groups[2].Value, out val);
                            strat.bias_uncertainty = val;
                            break;
                        case "Random uncertainty":
                            Double.TryParse(m.Groups[2].Value, out val);
                            strat.random_uncertainty = val;
                            break;
                        case "Systematic uncertainty":
                            Double.TryParse(m.Groups[2].Value, out val);
                            strat.systematic_uncertainty = val;
                            break;
                        case "Relative std deviation":
                            Double.TryParse(m.Groups[2].Value, out val);
                            strat.relative_std_dev = val;
                            testMeasurement.Stratum = strat;
                            break;
                        case "Material type":
                            testMeasurement.AcquireState.item_type = m.Groups[2].Value;
                            break;
                        case "Original declared mass":
                            Double.TryParse(m.Groups[2].Value, out val);
                            testMeasurement.AcquireState.mass = val;
                            break;
                        case "Measurement option"://Don't care. We know this from file extension at start.
                            mos = new MeasOptionSelector(currOption, mult);
                            break;
                        case "Data source":
                            testMeasurement.AcquireState.data_src = DetectorDefs.ConstructedSource.CycleFile;
                            break;
                        case "QC tests":
                            testMeasurement.AcquireState.qc_tests = m.Groups[2].Value == "ON" ? true : false;
                            break;
                        case "Error calculation":
                            testMeasurement.AcquireState.error_calc_method = m.Groups[2].Value == "Sample method" ? ErrorCalculationTechnique.Sample : ErrorCalculationTechnique.Theoretical;
                            break;
                        case "Accidentals method":
                            testMeasurement.Tests.accidentalsMethod = m.Groups[2].Value == "Measured" ? AccidentalsMethod.Measure : AccidentalsMethod.Calculate;
                            break;
                        case "Inspector name":
                            testMeasurement.AcquireState.user_id = m.Groups[2].Value;
                            break;
                        case "Active comment":
                        case "Passive comment":
                        case "Comment":
                            testMeasurement.AcquireState.comment = m.Groups[2].Value;
                            break;
                        case "Predelay":
                            Double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.predelayMS = val;
                            mult.sr.predelay = (ulong)val * 10;
                            break;
                        case "Gate length":
                            Double.TryParse(m.Groups[2].Value, out val);
                            mult.gateWidthTics = (ulong)val * 10;
                            mult.sr.gateLengthMS = val;
                            break;
                        case "2nd gate length"://What is this?
                            Double.TryParse(m.Groups[2].Value, out val);
                            break;
                        case "High voltage":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.highVoltage = val;
                            break;
                        case "Die away time":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.dieAwayTime = val * 10;
                            mult.sr.dieAwayTimeMS = val;
                            break;
                        case "Efficiency":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.efficiency = val;
                            break;
                        case "Multiplicity deadtime"://What is this?
                            double.TryParse(m.Groups[2].Value, out val);
                            break;
                        case "Coefficient A deadtime":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.deadTimeCoefficientAinMicroSecs = val;
                            break;
                        case "Coefficient B deadtime":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.deadTimeCoefficientBinPicoSecs = val;
                            break;
                        case "Coefficient C deadtime":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.deadTimeCoefficientCinNanoSecs = val;
                            break;
                        case "Doubles gate fraction":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.doublesGateFraction = val;
                            break;
                        case "Triples gate fraction":
                            double.TryParse(m.Groups[2].Value, out val);
                            mult.sr.triplesGateFraction = val;
                            break;
                        case "Normalization constant":
                            Match t = tuple.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            VTuple ValErr = new VTuple(val, err);
                            testMeasurement.Norm.currNormalizationConstant.CopyFrom(ValErr);
                            break;
                        //Begin background read
                        case "Active singles background"://Are the background rates raw or DTC? I think DTC
                        case "Active singles bkgrnd":
                        case "Passive singles bkgrnd":
                        case "Passive singles background":
                            t = tuple.Match(line);
                            line.TrimStart(' ');
                            if (line.StartsWith("Active"))
                                active = true;
                            else
                                active = false;
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedSinglesRate.CopyFrom(ValErr);
                            break;
                        case "Active doubles background":
                        case "Active doubles bkgrnd":
                        case "Passive doubles bkgrnd":
                        case "Passive doubles background":
                            t = tuple.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedDoublesRate.CopyFrom(ValErr);
                            break;
                        case "Active triples background":
                        case "Active triples bkgrnd":
                        case "Passive triples bkgrnd":
                        case "Passive triples background":
                            t = tuple.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedTriplesRate.CopyFrom(ValErr);
                            break;
                        case "Active scaler1 background":
                        case "Active scaler1 bkgrnd":
                        case "Passive scaler1 bkgrnd":
                        case "Passive scaler1 background":
                            double.TryParse(m.Groups[2].Value, out val);
                            ValErr = new VTuple(val, 0);
                            testMeasurement.Background.Scaler1.CopyFrom (ValErr);
                            testMeasurement.Background.DeadtimeCorrectedRates.Scaler1Rate = val;
                            testMeasurement.Background.DytlewskiDeadtimeCorrectedRates.Scaler1Rate = val;
                            break;
                        case "Active scaler2 background":
                        case "Active scaler2 bkgrnd":
                        case "Passive scaler2 bkgrnd":
                        case "Passive scaler2 background":
                            double.TryParse(m.Groups[2].Value, out val);
                            ValErr = new VTuple(val, 0);
                            testMeasurement.Background.Scaler2.CopyFrom(ValErr);
                            testMeasurement.Background.DeadtimeCorrectedRates.Scaler2Rate = val;
                            testMeasurement.Background.DytlewskiDeadtimeCorrectedRates.Scaler2Rate = val;
                            break;
                        //Begin isotopics read
                        case "Isotopics id":
                            testMeasurement.Isotopics.id = m.Groups[2].Value;
                            break;
                        case "Isotopics source code":
                            System.Enum.TryParse<Isotopics.SourceCode>(m.Groups[2].Value, out testMeasurement.Isotopics.source_code);
                            break;
                        case "Pu238":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.pu238, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.pu238, val2, err2);
                            break;
                        case "Pu239":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.pu239, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.pu239, val2, err2);
                            break;
                        case "Pu240":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.pu240, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.pu240, val2, err2);
                            break;
                        case "Pu241":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.pu241, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.pu241, val2, err2);
                            break;
                        case "Pu242":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.pu242, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.pu242, val2, err2);
                            ValErr = new VTuple(val, err);
                            break;
                        case "Pu date":// couldn't get this working
                            t = twodates.Match(line);
                            DateTime dateValue;
                            //Entered iso date
                            DateTime.TryParseExact(t.Groups[2].Value,
                                "yy.MM.dd",
                                new CultureInfo("en-US"),
                                    DateTimeStyles.None,
                                    out dateValue);
                            testMeasurement.Isotopics.pu_date = dateValue;
                            //calc date
                            DateTime.TryParseExact(t.Groups[3].Value,
                            "yy.MM.dd",
                            new CultureInfo("en-US"),
                                DateTimeStyles.None,
                                out dateValue);
                            CalculatedIsotopics.pu_date = dateValue;
                            break;
                        case "Am241":
                            t = tupleFiveGroup.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            testMeasurement.Isotopics.SetValueError(Isotope.am241, val, err);
                            Double.TryParse(t.Groups[4].Value, out val2);
                            Double.TryParse(t.Groups[5].Value, out err2);
                            CalculatedIsotopics.SetValueError(Isotope.am241, val2, err2);
                            break;
                        case "Am date":
                            t = twodates.Match(line);
                            //Entered iso date
                            DateTime.TryParseExact(t.Groups[2].Value,
                                "yy.MM.dd",
                                new CultureInfo("en-US"),
                                    DateTimeStyles.None,
                                    out dateValue);
                            testMeasurement.Isotopics.am_date = dateValue;
                            //calc date
                            DateTime.TryParseExact(t.Groups[3].Value,
                            "yy.MM.dd",
                            new CultureInfo("en-US"),
                                DateTimeStyles.None,
                                out dateValue);
                            CalculatedIsotopics.am_date = dateValue;
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }

                    line = sr.ReadLine();
                    lineCount++;
                }
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadHeader: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }
        public static void ReadSummedData(string lbl = "")
        {
            try
            {
                if (lbl == "")
                {
                    line = sr.ReadLine(); //Empty line

                    line = sr.ReadLine();//Grab Results
                    lineCount += 2;
                }
                else
                    line = lbl;
                if (!testMeasurement.CountingAnalysisResults.ContainsKey(mult))
                    testMeasurement.CountingAnalysisResults.Add(mult, new MultiplicityCountingRes());

                MultiplicityCountingRes mcrTotal = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];
                while (line.Contains(":"))
                {
                    Match m = labelValue.Match(line);
                    label = m.Groups[1].Value;
                    switch (label)
                    {
                        case "Number of good cycles"://Don't know where in results this is.
                        case "Number Passive cycles":
                            testMeasurement.Cycles = new CycleList();
                            int num = 0;
                            Int32.TryParse(m.Groups[2].Value, out num);
                            testMeasurement.Cycles.Capacity = num;
                            break;
                        case "Total count time":
                            // Not sure about this. Seems that in a background measurement this is cycle count time.
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.TS = TimeSpan.FromSeconds(val);
                            mcrTotal.TS = TimeSpan.FromSeconds(val);
                            break;
                        case "Count time (sec)":
                            // Not sure about this. Seems that in a background measurement this is cycle count time. TODO
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.TS = TimeSpan.FromSeconds(val * testMeasurement.Cycles.Count);
                            mcrTotal.TS = TimeSpan.FromSeconds(val * testMeasurement.Cycles.Count);
                            break;
                        case "Shift register singles sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Totals = val;
                            mcrTotal.Totals = val;
                            break;
                        case "Shift register reals + accidentals sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.RASum = val;
                            mcrTotal.RASum = val;
                            break;
                        case "Shift register accidentals sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.ASum = val;
                            InputResult.UnASum = val;
                            mcrTotal.ASum = val;
                            mcrTotal.UnASum = val;
                            break;
                        case "Shift register 1st scaler sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.S1Sum = val;
                            mcrTotal.S1Sum = val;
                            break;
                        case "Shift register 2nd scaler sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.S2Sum = val;
                            mcrTotal.S2Sum = val;
                            break;
                        case "":
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }
                    line = sr.ReadLine();
                    lineCount++;
                }
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadActiveSummedData: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }

        public static void ReadSummedMult()
        {

            try
            {
                line = sr.ReadLine();// Empty line
                line = sr.ReadLine();//header
                line = sr.ReadLine();//First data in table
                lineCount += 3;
                int idx = 0;
                ulong ra = 0;
                ulong a = 0;
                List<Tuple<ulong, ulong>> Distribution = new List<Tuple<ulong, ulong>>();
                MultiplicityCountingRes mcrTotal = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];

                while (line != "") //Read the whole table. Is delineated w/blank line.
                {
                    Match m = ThreeNums.Match(line);
                    Int32.TryParse(m.Groups[1].Value, out idx);
                    UInt64.TryParse(m.Groups[2].Value, out ra);
                    UInt64.TryParse(m.Groups[3].Value, out a);
                    Distribution.Add(Tuple.Create(ra, a));
                    line = sr.ReadLine();
                    lineCount++;
                }
                //Now create the correct length of the distribution and store in results.
                InputResult.RAMult = new ulong[Distribution.Count];
                mcrTotal.RAMult = new ulong[Distribution.Count];
                InputResult.NormedAMult = new ulong[Distribution.Count];
                mcrTotal.NormedAMult = new ulong[Distribution.Count];
                InputResult.UnAMult = new ulong[Distribution.Count];//Again? Normalized?
                mcrTotal.UnAMult = new ulong[Distribution.Count];
                for (idx = 0; idx < Distribution.Count; idx++)
                {
                    InputResult.RAMult[idx] = Distribution[idx].Item1;
                    mcrTotal.RAMult[idx] = Distribution[idx].Item1;
                    InputResult.UnAMult[idx] = Distribution[idx].Item2;
                    mcrTotal.UnAMult[idx] = Distribution[idx].Item2;
                    InputResult.NormedAMult[idx] = Distribution[idx].Item2;
                    mcrTotal.NormedAMult[idx] = Distribution[idx].Item2;
                }

            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadActiveSummedMult: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }

        public static void ReadResults()
        {
            try
            {
                line = sr.ReadLine();//Consume blank;
                line = sr.ReadLine();
                lineCount += 2;

                MultiplicityCountingRes mcrTotal = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];

                while (line.Contains(":"))
                {
                    Match m = tuple.Match(line);
                    //Aux ratios are only one value
                    if (!m.Success)
                        m = labelValue.Match(line);
                    switch (m.Groups[1].Value)
                    {
                        case "Singles":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.DeadtimeCorrectedSinglesRate.v = val;
                            mcrTotal.DeadtimeCorrectedSinglesRate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedSinglesRate.err = val;
                            mcrTotal.DeadtimeCorrectedSinglesRate.err = val;
                            break;
                        case "Doubles":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.DeadtimeCorrectedDoublesRate.v = val;
                            mcrTotal.DeadtimeCorrectedDoublesRate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedDoublesRate.err = val;
                            mcrTotal.DeadtimeCorrectedDoublesRate.err = val;
                            break;
                        case "Triples":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.DeadtimeCorrectedTriplesRate.v = val;
                            mcrTotal.DeadtimeCorrectedTriplesRate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedTriplesRate.err = val;
                            mcrTotal.DeadtimeCorrectedTriplesRate.err = val;
                            break;
                        case "Quads":
                            //INCC6 has no quads.
                            break;
                        case "Quads/Triples":
                            //INCC6 has no quads.
                            break;
                        case "Scaler 1":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Scaler1Rate.v = val;
                            InputResult.Scaler1.v = val;
                            mcrTotal.Scaler1.v = val;
                            mcrTotal.Scaler1Rate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.Scaler1Rate.err = val;
                            InputResult.Scaler1.err = val;
                            mcrTotal.Scaler1.err = val;
                            mcrTotal.Scaler1Rate.err = val;
                            break;
                        case "Scaler 2":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Scaler2Rate.v = val;
                            InputResult.Scaler2.v = val;
                            mcrTotal.Scaler2.v = val;
                            mcrTotal.Scaler2Rate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.Scaler2Rate.err = val;
                            InputResult.Scaler2.err = val;
                            mcrTotal.Scaler2.err = val;
                            mcrTotal.Scaler2Rate.err = val;
                            break;
                        case "Aux1 Ratio":
                            Double.TryParse(m.Groups[2].Value, out val);
                            //TODO: do not know where Aux ratio goes. Is it calculated?
                            break;
                        case "Aux2 Ratio":
                            Double.TryParse(m.Groups[2].Value, out val);
                            //TODO: do not know where Aux ratio goes. Is it calculated?
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }
                    line = sr.ReadLine();
                    lineCount++;
                }
                //Copy totals from InputResult to mcr for measurement
                mcrTotal.ASum = InputResult.ASum;
                StdRates DTCRates = new StdRates();
                DTCRates.SinglesRate = InputResult.DeadtimeCorrectedSinglesRate.v;
                DTCRates.DoublesRate = InputResult.DeadtimeCorrectedDoublesRate.v;
                DTCRates.TriplesRate = InputResult.DeadtimeCorrectedTriplesRate.v;
                DTCRates.Scaler1Rate = InputResult.Scaler1Rate.v;
                DTCRates.Scaler2Rate = InputResult.Scaler2Rate.v;

            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadResults: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }

        }
        public static void ReadActiveMultiplicityResults()
        {

            try
            {
                line = sr.ReadLine();//Empty line
                line = sr.ReadLine();
                Match m = tuple.Match(line);

                //Save results. We have to enter all the method params first.
                double multiplication = 0;
                double mult_err = 0;
                double.TryParse(m.Groups[2].Value, out multiplication);
                double.TryParse(m.Groups[3].Value, out mult_err);
                active_mult_results.mult.v = multiplication;
                active_mult_results.mult.err = mult_err;

                testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type), AnalysisMethod.ActiveMultiplicity, active_mult_results);
                line = sr.ReadLine();//empty
                lineCount += 2;
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadActiveMultiplicityResults: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }
        public static void ReadPassiveMultiplicityResults()
        {
            try
            {
                line = sr.ReadLine();//Consume blank;
                line = sr.ReadLine();
                lineCount += 2;
                while (line.Contains(":"))
                {
                    Match m = tuple.Match(line);
                    if (m.Success == false)
                        m = labelValue.Match(line);
                    switch (m.Groups[1].Value.TrimStart(' '))
                    {
                        case "Multiplication":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.mult.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.mult.err = val;
                            break;
                        case "Alpha":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.alphaK.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.alphaK.err = val;
                            break;
                        case "Multiplication correction factor":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.corr_factor.v = val;
                            break;
                        case "Pu240e mass (g)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.pu240e_mass.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.pu240e_mass.err = val;
                            break;
                        case "Pu240e (%)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            //
                            break;
                        case "Pu mass (g)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.pu_mass.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.pu_mass.err = val;
                            break;
                        case "Declared Pu240e mass (g)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.dcl_pu240e_mass = val;
                            break;
                        case "Declared Pu mass (g)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.dcl_pu_mass = val;
                            break;
                        case "Declared - assay Pu mass (g)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.dcl_minus_asy_pu_mass.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.dcl_minus_asy_pu_mass.err = val;
                            break;
                        case "Declared - assay Pu mass (%)":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.dcl_minus_asy_pu_mass_pct = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            //passive_mult_results.d = val; ??
                            break;
                        case "Efficiency":
                            Double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_results.efficiencyComputed.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            passive_mult_results.efficiencyComputed.err = val;
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }
                    line = sr.ReadLine();
                    lineCount++;
                }
                testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type), AnalysisMethod.Multiplicity, passive_mult_results);

            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadResults: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }
        public static void ReadActiveMultiplicityCalibration()
        {
            double val = 0;
            try
            {

                //Grab params
                line = sr.ReadLine(); //empty
                line = sr.ReadLine();
                lineCount += 2;
                while (line.Contains(":"))
                {
                    Match m = labelValue.Match(line);
                    switch (m.Groups[1].Value)
                    {
                        case "1st factorial moment thermal neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vt1 = val;
                            break;
                        case "2nd factorial moment thermal neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vt2 = val;
                            break;
                        case "3rd factorial moment thermal neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vt3 = val;
                            break;
                        case "1st factorial moment fast neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vf1 = val;
                            break;
                        case "2nd factorial moment fast neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vf2 = val;
                            break;
                        case "3rd factorial moment fast neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            active_mult_params.vf3 = val;
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }

                    testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.ActiveMultiplicity, active_mult_params);
                    line = sr.ReadLine();
                    lineCount++;

                }
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadActiveMultiplicityCalibration: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }

        }
        public static void ReadCollarCalibration()
        {
            line = sr.ReadLine();
        }
        public static void ReadPassiveMultiplicityCalibration()
        {
            double val = 0;
            try
            {

                //Grab params
                line = sr.ReadLine(); //empty
                line = sr.ReadLine();
                lineCount += 2;
                while (line.Contains(":"))
                {
                    Match m = labelValue.Match(line);
                    switch (m.Groups[1].Value)
                    {
                        case "Spontaneous fission rate":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.sf_rate = val;
                            break;
                        case "1st factorial moment spontaneous fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vs1 = val;
                            break;
                        case "2nd factorial moment spontaneous fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vs2 = val;
                            break;
                        case "3rd factorial moment spontaneous fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vs3 = val;
                            break;
                        case "1st factorial moment induced fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vi1 = val;
                            break;
                        case "2nd factorial moment induced fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vi2 = val;
                            break;
                        case "3rd factorial moment induced fission":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vi3 = val;
                            break;
                        case "1st factorial moment fast neutron induced fission U235":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.vi3 = val;
                            break;
                        case "a":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.a = val;
                            break;
                        case "b":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.b = val;
                            break;
                        case "c":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.c = val;
                            break;
                        case "sigma x":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.sigma_x = val;
                            break;
                        case "alpha weight":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.alpha_weight = val;
                            break;
                        case "efficiency correction factor":
                            double.TryParse(m.Groups[2].Value, out val);
                            passive_mult_params.multEffCorFactor = val;
                            break;
                        case "Weighted Coefficients":
                            //double.TryParse(m.Groups[2].Value, out val);
                            //passive_mult_params.solve_efficiency = new INCCAnalysisParams.MultChoice();
                            //= val;??
                            //TODO: Store weighted coefficients
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            break;
                        default:
                            PrintLine(String.Format("Unknown label found in input file {0}.", line));
                            break;
                    }
                    line = sr.ReadLine();
                    lineCount++;

                }
                testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.Multiplicity, passive_mult_params);
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadActiveMultiplicityCalibration: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }
        public static void ReadCycleData()
        {
            // But of course, sometimes background has everything, other times not. SIGHS TODO
            line = sr.ReadLine();//blank
            //Another variation has no count time.
            line = sr.ReadLine();//Count time, maybe.....
            long countTime = 0;
            List<MultiplicityCountingRes> mcrcycs = new List<MultiplicityCountingRes>();
            List<Cycle> cycs = new List<Cycle>();
            Match m = labelValue.Match(line);//Count time
            if (m.Success)
            {
                Int64.TryParse(m.Groups[2].Value, out countTime);
            }
            else
            {
                //Calculate cycle time with total/number cycles
                if (currOption != AssaySelector.MeasurementOption.background)
                    countTime = (long)Math.Round(InputResult.TS.TotalSeconds / testMeasurement.Cycles.Capacity);//Why do I have some weird # of cycles?
                // Background read in header
            }
            MultiplicityCountingRes mcrTotal = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];

            try
            {
                if (currOption != AssaySelector.MeasurementOption.background)
                {
                    line = sr.ReadLine();//Blank
                    line = sr.ReadLine();//header
                    line = sr.ReadLine();//data
                    lineCount += 3;
                }
                int idx = 0;
                Match c;

                if (currOption != AssaySelector.MeasurementOption.background)
                {
                    while (line != "")
                    {
                        idx = 0;

                        while (line != "")
                        {
                            //Cycle totals
                            c = cycleData.Match(line);
                            cycs.Add(new Cycle());
                            mcrcycs.Add(new MultiplicityCountingRes());

                            if (c.Success)
                            {
                                cycs[idx].TS = new TimeSpan(0, 0, (int)countTime);
                                Int32.TryParse(c.Groups[1].Value, out cycs[idx].seq);
                                Double.TryParse(c.Groups[2].Value, out val);
                                mcrcycs[idx].Totals = val;
                                cycs[idx].Totals = (ulong)val;
                                mcrcycs[idx].TS = cycs[idx].TS;
                                Double.TryParse(c.Groups[3].Value, out val);
                                mcrcycs[idx].RASum = val;
                                double.TryParse(c.Groups[4].Value, out val);
                                mcrcycs[idx].ASum = val;
                                mcrcycs[idx].UnASum = val;
                                //Scaler rates are not stored by cycle......Only totals.
                                double.TryParse(c.Groups[5].Value, out val);
                                mcrcycs[idx].Scaler1.v = val;
                                double.TryParse(c.Groups[6].Value, out val);
                                mcrcycs[idx].Scaler2.v = val;
                                QCTestStatus qc = QCTestStatusExtensions.FromString(c.Groups[7].Value);
                                cycs[idx].SetQCStatus(mult, qc);
                                line = sr.ReadLine();
                                lineCount++;
                                idx++;
                            }
                        }
                    }
                    line = sr.ReadLine();
                    
                }

                line = sr.ReadLine();

                while (line != "")
                {
                    idx = 0;
                    //Cycle rates -- set DTC corrected to raw for now.
                    c = ratesData.Match(line);
                    if (currOption == AssaySelector.MeasurementOption.background)
                    {
                        //background. We haven't created them yet.
                        cycs.Add(new Cycle());
                        mcrcycs.Add(new MultiplicityCountingRes());
                    }
                    if (c.Success)
                    {
                        Double.TryParse(c.Groups[2].Value, out val);
                        mcrcycs[idx].DeadtimeCorrectedSinglesRate.v = val;
                        cycs[idx].SinglesRate = val;
                        Double.TryParse(c.Groups[3].Value, out val);
                        mcrcycs[idx].rates.DeadtimeCorrectedRates.DoublesRate = val;
                        Double.TryParse(c.Groups[4].Value, out val);
                        mcrcycs[idx].rates.DeadtimeCorrectedRates.TriplesRate = val;
                        Double.TryParse(c.Groups[5].Value, out val);
                        mcrcycs[idx].mass = val;
                        cycs[idx].SetQCStatus(mult, QCTestStatusExtensions.FromString(c.Groups[6].Value));
                    }
                    line = sr.ReadLine();
                    lineCount++;
                    idx++;
                }
                if (currOption != AssaySelector.MeasurementOption.background)
                    // No mult for background.
                {
                    foreach (Cycle cy in cycs)
                    {
                        cy.CountingAnalysisResults.Add(mult, mcrcycs[cy.seq - 1]);
                        testMeasurement.Add(cy);
                    }
                }
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in ReadCycleData: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }
        public static void ReadMultDistributions()
        {
            List<String> numbers = new List<String>();
            List<ulong[]> RAdists = new List<ulong[]>();
            List<ulong[]> Adists = new List<ulong[]>();
            MultiplicityCountingRes totalMcr = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];
            line = sr.ReadLine();
            ulong max = 0;
            maxmax = 0;
            for (int i = 0; i < testMeasurement.Cycles.Count; i++)
            {
                line = sr.ReadLine();

                Match d = cycleDistHeader.Match(line);
                int cycleNum = 0;
                Int32.TryParse(d.Groups[1].Value, out cycleNum);
                line = sr.ReadLine();
                while (line != "" && line != null)
                {
                    numbers.Add(line);
                    line = sr.ReadLine();
                }

                if ((ulong)numbers.Count > max)//per cycle
                    max = (ulong)numbers.Count;
                if ((ulong)numbers.Count > maxmax)//Over all cycles
                    //Again, if failed cycle is the max, don't add that as max.
                    if (testMeasurement.Cycles[cycleNum-1].QCStatusValid(mult))
                        maxmax = (ulong)numbers.Count;

                ulong[] RA = new ulong[numbers.Count];
                ulong[] A = new ulong[numbers.Count];
                ulong[] normedA = new ulong[numbers.Count];

                for (int j = 0; j < numbers.Count; j++)
                {
                    Match m = ThreeNums.Match(numbers[j]);
                    UInt64.TryParse(m.Groups[2].Value, out Ulon);
                    RA[j] = Ulon;
                    UInt64.TryParse(m.Groups[3].Value, out Ulon);
                    A[j] = Ulon;
                    normedA[j] = Ulon;
                }
                ((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).RAMult = new ulong[numbers.Count];
                ((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).UnAMult = new ulong[numbers.Count];
                ((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).NormedAMult = new ulong[numbers.Count];
                ((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).MaxBins = (ulong)numbers.Count;
                RA.CopyTo(((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).RAMult, 0);
                A.CopyTo(((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).UnAMult, 0);
                normedA.CopyTo(((MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult]).NormedAMult, 0);
                numbers.Clear();
                max = 0;
            }

            totalMcr.MaxBins = maxmax;
        }

        public static void ReadPassiveCalibrationCurveResults()
        {
            line = sr.ReadLine();//Empty line
            line = sr.ReadLine();
            lineCount += 2;

            //while loop below
            while (line.Contains(":"))
            {
                Match t = tuple.Match(line);// some lines will contain tuple data
                Match m = labelValue.Match(line);// all lines can can be represented as labels, we need to go through all lines
                switch (m.Groups[1].Value)
                {
                    case "Pu240e mass (g)":
                        //how do we know that this is from a curium source?
                        double Pu240e_mass = 0;
                        double Pu240e_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out Pu240e_mass);
                        double.TryParse(t.Groups[3].Value, out Pu240e_mass_err);
                        passive_cal_results.pu240e_mass.v = Pu240e_mass;
                        passive_cal_results.pu240e_mass.err = Pu240e_mass_err;
                        break;
                    case "Pu240e (%)":
                        double Pu240e_percent = 0;
                        double.TryParse(m.Groups[2].Value, out Pu240e_percent);// not sure what to do with this value or where to parse it.
                        //This value gets calculated for the report. No need to store it.
                        break;
                    case "Pu mass (g)":
                        //how do we know that this is from a curium source?
                        double Pu_mass = 0;
                        double Pu_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out Pu_mass);
                        double.TryParse(t.Groups[3].Value, out Pu_mass_err);
                        passive_cal_results.pu_mass.v = Pu_mass;
                        passive_cal_results.pu_mass.err = Pu_mass_err;
                        break;
                    case "Declared Pu240e mass (g)":
                        double declared_Pu240e_mass = 0;
                        double.TryParse(m.Groups[2].Value, out declared_Pu240e_mass);// not sure what to do with this value or where to parse it.
                        passive_cal_results.dcl_pu240e_mass = declared_Pu240e_mass;
                        break;
                    case "Declared Pu mass (g)":
                        double declared_Pu_mass = 0;
                        double.TryParse(m.Groups[2].Value, out declared_Pu_mass);// not sure what to do with this value or where to parse it.
                        passive_cal_results.dcl_pu_mass = declared_Pu_mass;
                        break;
                    case "Declared - assay Pu mass (g)":
                        double declared_assay_Pu_mass = 0;
                        double declared_assay_Pu_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_Pu_mass);
                        double.TryParse(t.Groups[3].Value, out declared_assay_Pu_mass_err);
                        passive_cal_results.dcl_minus_asy_pu_mass.v = declared_assay_Pu_mass;
                        passive_cal_results.dcl_minus_asy_pu_mass.err = declared_assay_Pu_mass_err;
                        break;
                    case "Declared - assay Pu mass (%)":
                        double declared_assay_Pu_mass_percent = 0;
                        double declared_assay_Pu_mass_percent_err = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_Pu_mass_percent);
                        double.TryParse(t.Groups[3].Value, out declared_assay_Pu_mass_percent_err);
                        //These are also calculated.....
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
            testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type), AnalysisMethod.CalibrationCurve, passive_cal_results);

        }

        public static void ReadCollarResults()
        {
            line = sr.ReadLine();
        }

        public static void ReadPassiveCalibrationCurveCalibration()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            //line = sr.ReadLine();
            lineCount += 2;

            while (line.Contains(":"))
            {
                double num;
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "Equation":
                        cal_curve_params.cev.cal_curve_equation = CurveEquationStringToEnum(m.Groups[2].Value);
                        break;
                    case "a":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.a = num;
                        break;
                    case "b":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.b = num;
                        break;
                    case "c":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.c = num;
                        break;
                    case "d":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.d = num;
                        break;
                    case "variance a":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.var_a = num;
                        break;
                    case "variance b":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.var_b = num;
                        break;
                    case "variance c":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.var_c = num;
                        break;
                    case "variance d":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.var_d = num;
                        break;
                    case "covariance ab":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.a, Coeff.b, num);
                        break;
                    case "covariance ac":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.a, Coeff.c, num);
                        break;
                    case "covariance ad":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.a, Coeff.d, num);
                        break;
                    case "covariance bc":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.b, Coeff.c, num);
                        break;
                    case "covariance bd":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.b, Coeff.d, num);
                        break;
                    case "covariance cd":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.setcovar(Coeff.c, Coeff.d, num);
                        break;
                    case "sigma x":
                        double.TryParse(m.Groups[2].Value, out num);
                        cal_curve_params.cev.sigma_x = num;
                        break;
                    default:
                        PrintLine(String.Format("Unknown label found in input file {0}", line));
                        break;
                }
                line = sr.ReadLine();
                lineCount++;

            }
            testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.CalibrationCurve, cal_curve_params);

        }
        public static INCCAnalysisParams.CurveEquation CurveEquationStringToEnum(string s)
        {
            /*CUBIC, // a + bm + cm^2 + dm^3
            POWER, // am^b
            HOWARDS, // am / (1 + bm)
            EXPONENTIAL // a (1 - exp(bm))*/
            INCCAnalysisParams.CurveEquation eq = INCCAnalysisParams.CurveEquation.CUBIC;
            switch (s)
            {
                case "a + bm + cm^2 + dm^3":
                    eq = INCCAnalysisParams.CurveEquation.CUBIC;
                    break;
                case "am^b":
                    eq = INCCAnalysisParams.CurveEquation.POWER;
                    break;
                case "am / (1 + bm)":
                    eq = INCCAnalysisParams.CurveEquation.HOWARDS;
                    break;
                case "a (1 - exp(bm))":
                    eq = INCCAnalysisParams.CurveEquation.EXPONENTIAL;
                    break;
                default:
                    eq = INCCAnalysisParams.CurveEquation.CUBIC;
                    break;
            }

            return eq;
        }
        public static void ReadKnownAlphaResults()
        {
            line = sr.ReadLine();//Empty line
            line = sr.ReadLine();
            lineCount += 2;

            //while loop below
            while (line.Contains(":"))
            {
                Match t = tuple.Match(line);
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "Alpha":
                        double KnownAlpha = 0;
                        double.TryParse(m.Groups[2].Value, out KnownAlpha);
                        known_alpha_results.alphaK = KnownAlpha;
                        break;
                    case "Multiplication":
                        double known_Multiplication = 0;
                        double.TryParse(m.Groups[2].Value, out known_Multiplication);
                        known_alpha_results.mult = known_Multiplication;
                        break;
                    case "Multiplication corrected doubles":
                        double multiplication_corrected_doubles = 0;
                        double.TryParse(m.Groups[2].Value, out multiplication_corrected_doubles);
                        known_alpha_results.mult_corr_doubles.v = multiplication_corrected_doubles;
                        break;
                    case "Pu240e mass (g)":
                        double Pu240e_mass = 0;
                        double Pu240e_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out Pu240e_mass);
                        double.TryParse(t.Groups[3].Value, out Pu240e_mass_err);
                        known_alpha_results.pu240e_mass.v = Pu240e_mass;
                        known_alpha_results.pu240e_mass.err = Pu240e_mass_err;
                        break;
                    case "Pu240e (%)":
                        double Pu240e_percent = 0;
                        double.TryParse(m.Groups[2].Value, out Pu240e_percent);
                        //This is calculated, not stored.
                        break;
                    case "Pu mass (g)":
                        double Pu_mass = 0;
                        double Pu_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out Pu_mass);
                        double.TryParse(t.Groups[3].Value, out Pu_mass_err);
                        known_alpha_results.pu_mass.v = Pu_mass;
                        known_alpha_results.pu_mass.err = Pu_mass_err;
                        break;
                    case "Declared Pu240e mass (g)":
                        double declared_Pu240e_mass = 0;
                        double.TryParse(m.Groups[2].Value, out declared_Pu240e_mass);
                        known_alpha_results.dcl_pu240e_mass = declared_Pu240e_mass;
                        break;
                    case "Declared Pu mass (g)":
                        double declared_Pu_mass = 0;
                        double.TryParse(m.Groups[2].Value, out declared_Pu_mass);// not sure what to do with this value or where to parse it.
                        known_alpha_results.dcl_pu_mass = declared_Pu_mass;
                        break;
                    case "Declared - assay Pu mass (g)":
                        double declared_assay_Pu_mass = 0;
                        double declared_assay_Pu_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_Pu_mass);
                        double.TryParse(t.Groups[3].Value, out declared_assay_Pu_mass_err);
                        known_alpha_results.dcl_minus_asy_pu_mass.v = declared_assay_Pu_mass;
                        known_alpha_results.dcl_minus_asy_pu_mass.err = declared_assay_Pu_mass_err;
                        break;
                    case "Declared - assay Pu mass (%)":
                        double declared_assay_Pu_mass_percent = 0;
                        double declared_assay_Pu_mass_percent_err = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_Pu_mass_percent);
                        double.TryParse(t.Groups[3].Value, out declared_assay_Pu_mass_percent_err);
                        //Calculated, no need to store.
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
            testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type), AnalysisMethod.KnownA, known_alpha_results);

        }
        public static void ReadKnownAlphaCalibration()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            lineCount += 2;
            double a = 0.0;
            double b = 0.0;
            double variance_a = 0.0;
            double variance_b = 0.0;
            double covariance_ab = 0.0;

            while (line.Contains(":"))
            {
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {

                    case "Alpha weight":
                        double.TryParse(m.Groups[2].Value, out known_alpha_params.alpha_wt);
                        break;
                    case "Rho zero":
                        double.TryParse(m.Groups[2].Value, out known_alpha_params.rho_zero);
                        break;
                    case "k":
                        double.TryParse(m.Groups[2].Value, out known_alpha_params.k);
                        break;
                    case "a":
                        double.TryParse(m.Groups[2].Value, out a);
                        known_alpha_params.cev.a = a;
                        break;
                    case "b":
                        double.TryParse(m.Groups[2].Value, out b);
                        known_alpha_params.cev.b = b;
                        break;
                    case "variance a":
                        double.TryParse(m.Groups[2].Value, out variance_a);
                        known_alpha_params.cev.var_a = variance_a;
                        break;
                    case "variance b":
                        double.TryParse(m.Groups[2].Value, out variance_b);
                        known_alpha_params.cev.var_b = variance_b;
                        break;
                    case "covariance ab":
                        double.TryParse(m.Groups[2].Value, out covariance_ab);
                        known_alpha_params.cev.setcovar(Coeff.a, Coeff.b, covariance_ab);
                        break;
                    case "sigma x":
                        double.TryParse(m.Groups[2].Value, out known_alpha_params.cev.sigma_x);
                        break;
                    default:
                        PrintLine(String.Format("Unknown label found in input file {0}.", line));
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
            testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.KnownA, known_alpha_params);
        }

        public static void ReadCmRatioCalibration()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            //line = sr.ReadLine();
            lineCount += 2;

            while (line.Contains(":"))
            {
                double num;
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "Curium Ratio Variant":
                        cm_ratio_cev_rec.SetVariantByString(m.Groups[2].Value);
                        break;
                    case "Equation":
                        cm_ratio_cev_rec.cev.cal_curve_equation = CurveEquationStringToEnum(m.Groups[2].Value);
                        break;
                    case "a":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.a = num;
                        break;
                    case "b":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.b = num;
                        break;
                    case "c":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.c = num;
                        break;
                    case "d":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.d = num;
                        break;
                    case "variance a":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.var_a = num;
                        break;
                    case "variance b":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.var_b = num;
                        break;
                    case "variance c":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.var_c = num;
                        break;
                    case "variance d":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.var_d = num;
                        break;
                    case "covariance ab":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.a, Coeff.b, num);
                        break;
                    case "covariance ac":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.a, Coeff.c, num);
                        break;
                    case "covariance ad":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.a, Coeff.d, num);
                        break;
                    case "covariance bc":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.b, Coeff.c, num);
                        break;
                    case "covariance bd":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.b, Coeff.d, num);
                        break;
                    case "covariance cd":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.setcovar(Coeff.c, Coeff.d, num);
                        break;
                    case "sigma x":
                        double.TryParse(m.Groups[2].Value, out num);
                        cm_ratio_cev_rec.cev.sigma_x = num;
                        break;
                    default:
                        PrintLine(String.Format("Unknown label found in input file {0}", line));
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
            //Add this after you have the cm_pu info.
        }

        public static void ReadCmRatioResults()
        {
            line = sr.ReadLine();//Empty line
            line = sr.ReadLine();
            lineCount += 2;

            //while loop below
            while (line.Contains(":"))
            {
                Match t = tuple.Match(line);
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "NN-22":
                        cm_ratio_params.cm_id_label = t.Groups[2].Value;
                        break;
                    case "Input batch id":
                        cm_ratio_params.cm_input_batch_id = t.Groups[2].Value;
                        break;
                    case "Cm/Pu ratio date":
                        DateTime.TryParseExact(t.Groups[2].Value, "MM.dd.YY", null, DateTimeStyles.None, out cm_ratio_params.cm_pu_ratio_date);
                        break;
                    case "Cm/Pu ratio":
                        double cm_pu = 0;
                        double cm_pu_err = 0;
                        double.TryParse(t.Groups[2].Value, out cm_pu);
                        double.TryParse(t.Groups[3].Value, out cm_pu_err);
                        cm_ratio_params.cm_pu_ratio.v = cm_pu;
                        cm_ratio_params.cm_pu_ratio.err = cm_pu_err;
                        break;
                    case "Cm/U ratio date":
                        DateTime.TryParseExact(t.Groups[2].Value, "MM.dd.YY", null, DateTimeStyles.None, out cm_ratio_params.cm_u_ratio_date);
                        break;
                    case "Cm/U ratio":
                        double cm_u = 0;
                        double cm_u_err = 0;
                        double.TryParse(t.Groups[2].Value, out cm_u);
                        double.TryParse(t.Groups[3].Value, out cm_u_err);
                        cm_ratio_params.cm_u_ratio.v = cm_u;
                        cm_ratio_params.cm_u_ratio.err = cm_u_err;
                        break;
                    case "Pu effective half-life":
                        //Isn't this just a constant?
                        break;
                    case "Decay corrected Cm/Pu ratio":
                        double cm_pu_corr = 0;
                        double cm_pu_corr_err = 0;
                        double.TryParse(t.Groups[2].Value, out cm_pu_corr);
                        double.TryParse(t.Groups[3].Value, out cm_pu_corr_err);
                        cm_ratio_results.cm_pu_ratio_decay_corr.v = cm_pu_corr;
                        cm_ratio_results.cm_pu_ratio_decay_corr.err = cm_pu_corr_err;
                        break;
                    case "Decay corrected Cm/U ratio":
                        double cm_u_corr = 0;
                        double cm_u_corr_err = 0;
                        double.TryParse(t.Groups[2].Value, out cm_u_corr);
                        double.TryParse(t.Groups[3].Value, out cm_u_corr_err);
                        cm_ratio_results.cm_u_ratio_decay_corr.v = cm_u_corr;
                        cm_ratio_results.cm_u_ratio_decay_corr.err = cm_u_corr_err;
                        break;
                    case "Cm mass (g)":
                        double cm_mass = 0;
                        double cm_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out cm_mass);
                        double.TryParse(t.Groups[3].Value, out cm_mass_err);
                        cm_ratio_results.cm_mass.v = cm_mass;
                        cm_ratio_results.cm_mass.err = cm_mass_err;
                        break;
                    case "Pu mass (g)":
                        double pu_mass = 0;
                        double pu_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out pu_mass);
                        double.TryParse(t.Groups[3].Value, out pu_mass_err);
                        cm_ratio_results.pu.pu_mass.v = pu_mass;
                        cm_ratio_results.pu.pu_mass.err = pu_mass_err;
                        break;
                    case "U mass (g)":
                        double u_mass = 0;
                        double u_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out u_mass);
                        double.TryParse(t.Groups[3].Value, out u_mass_err);
                        cm_ratio_results.u.mass.v = u_mass;
                        cm_ratio_results.u.mass.err = u_mass_err;
                        break;
                    case "U235 mass (g)":
                        double u235_mass = 0;
                        double u235_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out u235_mass);
                        double.TryParse(t.Groups[3].Value, out u235_mass_err);
                        cm_ratio_results.u235.mass.v = u235_mass;
                        cm_ratio_results.u235.mass.err = u235_mass_err;
                        break;
                    case "Declared U mass (g)":
                        double dec_u_mass = 0;
                        double.TryParse(t.Groups[2].Value, out dec_u_mass);
                        cm_ratio_params.cm_dcl_u_mass = dec_u_mass;
                        cm_ratio_results.u.dcl_mass = dec_u_mass;
                        break;
                    case "Declared - assay U mass(g)":
                        double dec_assay_u = 0;
                        double dec_assay_u_err = 0;
                        double.TryParse(t.Groups[2].Value, out dec_assay_u);
                        double.TryParse(t.Groups[3].Value, out dec_assay_u_err);
                        cm_ratio_results.u.dcl_minus_asy_mass.v = dec_assay_u;
                        cm_ratio_results.u.dcl_minus_asy_mass.err = dec_assay_u_err;
                        break;
                    case "Declared - assay U mass (%)":
                        double dec_assay_u_pct = 0;
                        double dec_assay_u_pct_err = 0;
                        double.TryParse(t.Groups[2].Value, out dec_assay_u_pct);
                        double.TryParse(t.Groups[3].Value, out dec_assay_u_pct_err);
                        cm_ratio_results.u.dcl_minus_asy_mass.v = dec_assay_u_pct;
                        cm_ratio_results.u.dcl_minus_asy_mass.err = dec_assay_u_pct_err;
                        break;
                    case "Declared U235 mass (g)":
                        double dec_u235_mass = 0;
                        double.TryParse(t.Groups[2].Value, out dec_u235_mass);
                        cm_ratio_params.cm_dcl_u235_mass = dec_u235_mass;
                        cm_ratio_results.u235.dcl_mass = dec_u235_mass;
                        break;
                    case "Declared - assay U235 mass(g)":
                        double dec_assay_u235 = 0;
                        double dec_assay_u235_err = 0;
                        double.TryParse(t.Groups[2].Value, out dec_assay_u235);
                        double.TryParse(t.Groups[3].Value, out dec_assay_u235_err);
                        cm_ratio_results.u235.dcl_minus_asy_mass.v = dec_assay_u235;
                        cm_ratio_results.u235.dcl_minus_asy_mass.err = dec_assay_u235_err;
                        break;
                    case "Declared - assay U235 mass (%)":
                        double dec_assay_u235_pct = 0;
                        double dec_assay_u235_pct_err = 0;
                        double.TryParse(t.Groups[2].Value, out dec_assay_u235_pct);
                        double.TryParse(t.Groups[3].Value, out dec_assay_u235_pct_err);
                        cm_ratio_results.u235.dcl_minus_asy_mass_pct= dec_assay_u235_pct;
                        cm_ratio_results.u235.dcl_minus_asy_mass.err = dec_assay_u235_pct_err;
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
            testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.CuriumRatio, cm_ratio_cev_rec);
            cm_ratio_results.methodParams = cm_ratio_cev_rec;
            cm_ratio_results.methodParams2 = cm_ratio_params;
            testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.detector_id), AnalysisMethod.CuriumRatio, cm_ratio_results);

        }
        public static void ReadAddASourceCalibration()
        {
            //TODO
        }

        public static void ReadAddASourceResults()
        {
            //TODO
        }

        public static void ReadActiveCalibrationParams()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            //line = sr.ReadLine();
            lineCount += 2;

            while (line.Contains(":"))
            {
                double num;
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "Equation":
                        active_cal_curve_params.cev.cal_curve_equation = CurveEquationStringToEnum(m.Groups[2].Value);
                        break;
                    case "a":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.a = num;
                        break;
                    case "b":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.b = num;
                        break;
                    case "c":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.c = num;
                        break;
                    case "d":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.d = num;
                        break;
                    case "variance a":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.var_a = num;
                        break;
                    case "variance b":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.var_b = num;
                        break;
                    case "variance c":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.var_c = num;
                        break;
                    case "variance d":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.var_d = num;
                        break;
                    case "covariance ab":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.a, Coeff.b, num);
                        break;
                    case "covariance ac":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.a, Coeff.c, num);
                        break;
                    case "covariance ad":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.a, Coeff.d, num);
                        break;
                    case "covariance bc":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.b, Coeff.c, num);
                        break;
                    case "covariance bd":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.b, Coeff.d, num);
                        break;
                    case "covariance cd":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.setcovar(Coeff.c, Coeff.d, num);
                        break;
                    case "sigma x":
                        double.TryParse(m.Groups[2].Value, out num);
                        active_cal_curve_params.cev.sigma_x = num;
                        break;
                    default:
                        PrintLine(String.Format("Unknown label found in input file {0}", line));
                        break;
                }
                line = sr.ReadLine();
                lineCount++;

            }
            testMeasurement.INCCAnalysisState.Methods.AddMethod(AnalysisMethod.Active, active_cal_curve_params);

        }

        public static void ReadActiveCalibrationResults()
        {
            line = sr.ReadLine();//Empty line
            line = sr.ReadLine();
            lineCount += 2;

            //while loop below
            while (line.Contains(":"))
            {
                Match t = tuple.Match(line);// some lines will contain tuple data
                Match m = labelValue.Match(line);// all lines can can be represented as labels, we need to go through all lines
                switch (m.Groups[1].Value)
                {
                    case "Pu240e mass (g)":
                        //how do we know that this is from a curium source?
                        double u235_mass = 0;
                        double u235_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out u235_mass);
                        double.TryParse(t.Groups[3].Value, out u235_mass_err);
                        active_cal_results.u235_mass.v = u235_mass;
                        active_cal_results.u235_mass.err = u235_mass_err;
                        break;
                    case "k0 (source yield factor)":
                        double k0 = 0;
                        //double k0_err = 0;
                        double.TryParse(m.Groups[2].Value, out k0);
                        //double.TryParse(m.Groups[3].Value, out k0_err);
                        active_cal_results.k0.v = k0;
                        //Error not printed on report
                        //active_cal_results.k0.err = k0_err;
                        break;
                    case "k1 (stability factor)":
                        double k1 = 0;
                        double k1_err = 0;
                        double.TryParse(m.Groups[2].Value, out k1);
                        double.TryParse(m.Groups[3].Value, out k1_err);
                        active_cal_results.k1.v = k1;
                        active_cal_results.k1.err = k1_err;
                        break;
                    case "K (total correction factor)":
                        double k = 0;
                        double k_err = 0;
                        double.TryParse(m.Groups[2].Value, out k);
                        double.TryParse(m.Groups[3].Value, out k_err);
                        active_cal_results.k1.v = k;
                        active_cal_results.k1.err = k_err;
                        break;
                    case "U235 mass (g)":
                        //how do we know that this is from a curium source?
                        double U235_mass = 0;
                        double U235_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out U235_mass);
                        double.TryParse(t.Groups[3].Value, out U235_mass_err);
                        active_cal_results.u235_mass.v = U235_mass;
                        active_cal_results.u235_mass.err = U235_mass_err;
                        break;
                    case "Declared U235 mass (g)":
                        double declared_U235_mass = 0;
                        double.TryParse(m.Groups[2].Value, out declared_U235_mass);// not sure what to do with this value or where to parse it.
                        active_cal_results.dcl_u235_mass = declared_U235_mass;
                        break;
                    case "Declared - assay U235 mass (g)":
                        double declared_assay_U235_mass = 0;
                        double declared_assay_U235_mass_err = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_U235_mass);
                        double.TryParse(t.Groups[3].Value, out declared_assay_U235_mass_err);
                        active_cal_results.dcl_minus_asy_u235_mass.v = declared_assay_U235_mass;
                        active_cal_results.dcl_minus_asy_u235_mass.err = declared_assay_U235_mass_err;
                        break;
                    case "Declared - assay U235 mass (%)":
                        double declared_assay_U235_mass_pct = 0;
                        double.TryParse(t.Groups[2].Value, out declared_assay_U235_mass_pct);
                        active_cal_results.dcl_minus_asy_u235_mass_pct = declared_assay_U235_mass_pct;
                        break;
                }
                line = sr.ReadLine();
                lineCount++;

            }
            testMeasurement.INCCAnalysisResults.AddMethodResults(mult, new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type), AnalysisMethod.Active, active_cal_results);

        }

        static bool CompareParameters(Measurement original, Measurement recalculated)
        {
            bool same = true;//overall success
            bool temp = true;//stepwise success
            temp = original.Detector.CompareTo(recalculated.Detector) == 0;
            same = temp;
            PrintLine(String.Format("Detector parameters compared: {0}", temp ? "PASS" : "FAIL"));
            temp = original.INCCAnalysisState.Methods.Equals(recalculated.INCCAnalysisState.Methods);
            same = temp && same;
            PrintLine(String.Format("Method parameters compared: {0}", temp ? "PASS" : "FAIL"));
            return same;
            //Just here for diagnostic. If parameters do not match, neither will results.
        }

        static bool CompareRates(Measurement original, Measurement recalculated)
        {
            //Compare cycles and averages.
            bool same = true;
            bool temp = true;
            List<string> ls;

            PrintLine("Compare each cycle");
            for (int i = 0; i < original.Cycles.Count; i++)
            {
                MultiplicityCountingRes mcr1 = (MultiplicityCountingRes)original.Cycles[i].CountingAnalysisResults[mult];
                MultiplicityCountingRes mcr2 = (MultiplicityCountingRes)recalculated.Cycles[i].CountingAnalysisResults[mult];
                temp = mcr1.Equals(mcr2);

                PrintLine(String.Format("Cycle{0}: {1}", i + 1, temp ? "PASS" : "FAIL"));
                ls = mcr1.testStatus;
                if (ls != null)
                {
                    foreach (string s in ls)
                        PrintLine(s);
                }
                same = temp && same;
            }
            PrintLine("Compare rates for measurement");
            //TODO Compare actual singles,doubles, triples
            IEnumerator iter = original.CountingAnalysisResults.GetMultiplicityEnumerator();
            IEnumerator iter2 = recalculated.CountingAnalysisResults.GetMultiplicityEnumerator();
            if (iter != null && iter2 != null)
                //Only compare multiplicity for mult measurements.
            {
                PrintLine("Compare multiplicity counting for entire measurement:");
                while (iter.MoveNext())
                {
                    iter2.MoveNext();
                    Multiplicity mkey1 = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                    MultiplicityCountingRes mcr1 = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
                    /*if (verbose)
                    {
                        PrintLine("original multiplicity:");
                        lines = mcr.StringifyCurrentMultiplicityDetails();
                        foreach (string s in lines)
                            PrintLine(s);
                        PrintLine("");
                    }*/
                    Multiplicity mkey2 = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter2.Current)).Key;
                    MultiplicityCountingRes mcr2 = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter2.Current)).Value;
                    /*if (verbose)
                    {
                        PrintLine("recalculated multiplicity:");
                        lines = mcr2.StringifyCurrentMultiplicityDetails();
                        foreach (string s in lines)
                            PrintLine(s);
                        PrintLine("");
                    }*/
                    temp = mcr1.Equals(mcr2);
                    ls = mcr1.testStatus;
                    if (ls != null)
                    {
                        foreach (string s in ls)
                            PrintLine(s);
                    }
                    same = temp && same;
                }
                same = temp && same;
                PrintLine(temp ? "CountingResults match" : "CountingResults different: FAIL");
                // We sometimes fail here, and it seems that we are finding more outliers than the original code. Need to math it. HN
            }
            return same;
        }

        static bool CompareResults(Measurement original, Measurement recalculated)
        {
            //Compare results and method results.
            bool same = true;
            bool temp = true;
            INCCMethodResults imrs1, imrs2;
            //Get results for each measurement
            original.INCCAnalysisResults.MethodsResults.TryGetValue(mult, out imrs1);
            recalculated.INCCAnalysisResults.MethodsResults.TryGetValue(mult, out imrs2);

            IEnumerator iter1 = imrs1.GetEnumerator();
            IEnumerator iter2 = imrs2.GetEnumerator();
            KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>> kvp1 = new KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>();
            KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>> kvp2 = new KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>();
            while (iter1.MoveNext() && iter2.MoveNext())
            {
                kvp1 = (KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>)iter1.Current;
                kvp2 = (KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>)iter2.Current;

                Dictionary<AnalysisMethod, INCCMethodResult> d1 = kvp1.Value;
                Dictionary<AnalysisMethod, INCCMethodResult> d2 = kvp2.Value;

                foreach (AnalysisMethod am in d1.Keys)
                {
                    INCCMethodResult mr1, mr2;
                    d1.TryGetValue(am, out mr1);
                    d1.TryGetValue(am, out mr2);
                    temp = mr1.Equals(mr2);
                    if (temp)
                        PrintLine(String.Format("Analysis results comparison for {0}: PASS", am.FullName()));
                    else
                    {
                        PrintLine("Failure of results comparison");
                        PrintLine("Original method results:");
                        List<string> lr1 = mr1.ToSimpleLines(original);
                        foreach (string r in lr1)
                        {
                            PrintLine(r);
                        }
                        PrintLine("Recalculated method results:");
                        List<string> lr2 = mr1.ToSimpleLines(recalculated);
                        foreach (string r in lr2)
                        {
                            PrintLine(r);
                        }
                        return false;
                    }
                    same = temp && same;
                }
            }


            PrintLine(temp ? "All analysis Method Results match" : "At least one analysis result different: FAIL");
            return same;
        }

        //Build a measurement fresh from existing. No DB or "internal lameness" required.
        //Copy all but results.
        static Measurement BuildMeasurement(Measurement meas)
        {
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(meas.Detector),
                                   tp,
                                   np,
                                   bp,
                                   iso,
                                   acq,
                                   hvp);
            // create the context holder for the measurement. Everything is rooted here ...
            Measurement copied = new Measurement(mt, meas.MeasOption, null);
            copied.Detector = new Detector(meas.Detector);
            copied.AcquireState = new AcquireParameters(meas.AcquireState);
            copied.AnalysisParams = new CountingAnalysisParameters(meas.AnalysisParams);
            copied.INCCAnalysisState = new INCCAnalysisState();
            copied.INCCAnalysisState.Methods = new AnalysisMethods();
            copied.INCCAnalysisState.Methods.CopySettings(meas.INCCAnalysisState.Methods);
            copied.InitializeResultsSummarizers();
            copied.InitializeContext(clearCounterResults: true);
            copied.PrepareINCCResults();
            copied.INCCAnalysisState.Results = new INCCResults();
            copied.Background.Scaler1 = testMeasurement.Background.Scaler1;
            copied.Background.Scaler2 = testMeasurement.Background.Scaler2;
            copied.Background.DeadtimeCorrectedRates.CopyFrom(testMeasurement.Background.DeadtimeCorrectedRates);
            copied.Background.DytlewskiDeadtimeCorrectedRates.CopyFrom(testMeasurement.Background.DytlewskiDeadtimeCorrectedRates);
            INCCResults.results_rec xres = new INCCResults.results_rec();
            copied.INCCAnalysisResults.TradResultsRec = xres;
            copied.INCCAnalysisResults.MethodsResults = new Dictionary<SpecificCountingAnalyzerParams, INCCMethodResults>();
            IEnumerator ienum = meas.INCCAnalysisState.Methods.GetMethodEnumerator();
            while (ienum.MoveNext())
            {
                System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor> md = (System.Tuple<AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor>)ienum.Current;
                AnalysisMethod am = md.Item1;
                INCCAnalysisParams.INCCMethodDescriptor parms = md.Item2;
                copied.INCCAnalysisState.Methods.AddMethod(am, meas.INCCAnalysisState.Methods.GetMethodParameters(am));
                switch (am)
                {
                    case AnalysisMethod.ActiveMultiplicity:
                        copied.INCCAnalysisState.Results.AddMethodResults(mult, new INCCSelector(meas.AcquireState.item_id, meas.AcquireState.item_type), am, new INCCMethodResults.results_active_mult_rec());
                        break;
                    case AnalysisMethod.Multiplicity:
                        copied.INCCAnalysisState.Results.AddMethodResults(mult, new INCCSelector(meas.AcquireState.item_id, meas.AcquireState.item_type), am, new INCCMethodResults.results_multiplicity_rec());
                        break;
                    case AnalysisMethod.CalibrationCurve:
                        copied.INCCAnalysisState.Results.AddMethodResults(mult, new INCCSelector(meas.AcquireState.item_id, meas.AcquireState.item_type), am, new INCCMethodResults.results_cal_curve_rec());
                        break;
                    case AnalysisMethod.KnownA:
                        copied.INCCAnalysisState.Results.AddMethodResults(mult, new INCCSelector(meas.AcquireState.item_id, meas.AcquireState.item_type), am, new INCCMethodResults.results_known_alpha_rec());
                        break;
                }
            }
            ABKey thisRun = new ABKey(mult, (uint)maxmax);

            foreach (Cycle c in meas.Cycles)
            {
                ((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB = AlphaBetaCache.GetAlphaBeta(thisRun);
                if (((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB == null)
                {
                    ((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB = new AlphaBeta((int)maxmax);
                    ((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB.Resize((int)maxmax);
                    AlphaBetaCache.AddAlphaBeta(thisRun, ((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB);
                }
                else
                    ((MultiplicityCountingRes)c.CountingAnalysisResults[mult]).AB = AlphaBetaCache.GetAlphaBeta(thisRun);

                copied.Cycles.Add(new Cycle(c));
            }
            
            copied.CountingAnalysisResults.Remove(mult);
            MultiplicityCountingRes mcr2 = new MultiplicityCountingRes((MultiplicityCountingRes)meas.CountingAnalysisResults[mult]);
            CycleProcessing.calc_alpha_beta(mult, mcr2);
            thisSetting = mcr2.AB;
            mcr2.AB = (thisSetting == null || (thisSetting.MaxBins == 1 && mcr2.RASum == 0)) ?null:thisSetting;
            mcr2.RAMult = new ulong[mcr2.MaxBins];
            mcr2.UnAMult = new ulong[mcr2.MaxBins];
            mcr2.NormedAMult = new ulong[mcr2.MaxBins];
            copied.CountingAnalysisResults.Add(mult, mcr2);
            
            copied.INCCAnalysisState.PrepareINCCResults(currOption, mult, mcr2);


            LMRawAnalysis.SDTMultiplicityCalculator sdtmc = new LMRawAnalysis.SDTMultiplicityCalculator(1e-7);
            MultiplicityResult mr;
            MultiplicityCountingRes mcr = ((MultiplicityCountingRes)meas.CountingAnalysisResults[mult]);
            mcr.AB = (thisSetting == null || (thisSetting.MaxBins == 1 && mcr2.RASum == 0)) ? null : thisSetting;

            if (mcr.FA == FAType.FAOff)
            {
                // Fix triples. Was sending GW in wrong units. HN 4/26/2018
                mr = sdtmc.GetSDTMultiplicityResult(mcr.RAMult,
                        mcr.UnAMult,
                        mcr.FA == FAType.FAOn,
                        mult.gateWidthTics * 10, mult.sr.predelay,
                        mcr.FA == FAType.FAOff ? mcr.accidentalsDelay : 0,
                        mult.sr.deadTimeCoefficientTinNanoSecs, mult.sr.deadTimeCoefficientAinMicroSecs,
                        mult.sr.deadTimeCoefficientBinPicoSecs, mult.sr.deadTimeCoefficientCinNanoSecs,
                        mcr.LMTS[1].TotalSeconds);

            }
            else
                mr = sdtmc.GetSDTMultiplicityResult(mcr.RAMult,
                        mcr.UnAMult,
                        mcr.FA == FAType.FAOn,
                        mult.gateWidthTics * 10, mult.sr.predelay,
                        mcr.FA == FAType.FAOff ? mcr.accidentalsDelay : 0,
                        mult.sr.deadTimeCoefficientTinNanoSecs, mult.sr.deadTimeCoefficientAinMicroSecs,
                        mult.sr.deadTimeCoefficientBinPicoSecs, mult.sr.deadTimeCoefficientCinNanoSecs,
                        mcr.LMTS[0].TotalSeconds);

            copied.Stratum = new Stratum();
            copied.Stratum.CopyFrom(meas.Stratum);

            return copied;
        }

        static void PrintLine(String s)
        {
            Console.WriteLine(s);
            results.WriteLine(s);
            results.Flush();
        }
        ~NCCTester()
        {
            PrintLine("Finalizing object");
            results.Flush();
            results.Close();
        }
    }
    
}
    

