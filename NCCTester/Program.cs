using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AnalysisDefs;
using INCCCore;
using NCCTransfer;
using NCCReporter;
using System.Globalization;

namespace NCCTester
{
    class Program
    {

        public static String testFileDir = "";
        public static String testFile = "";
        public static String testOutputDir = "";
        public static List<String> inputFiles = new List<string>();
        public static String resultsFile = "";
        public static StreamWriter results;

        public static String version;
        public static Detector det;
        public static DetectorDefs.DataSourceIdentifier dsid;
        public static Multiplicity mult;
        public static Measurement testMeasurement;
        public static MeasId measID;
        public static AnalysisMethod am;
        public static AnalysisMethod cc; // Probably not needed

        //Results loaded from file.
        public static INCCAnalysisState state;
        public static INCCResult TestVersionResult = new INCCResult();
        public static INCCMethodResult TestVersionMethodResult = new INCCMethodResult();
        public static INCCAnalysisParams.active_mult_rec active_mult_params = new INCCAnalysisParams.active_mult_rec();
        public static INCCMethodResults.results_active_mult_rec active_mult_results = new INCCMethodResults.results_active_mult_rec();
        public static INCCMethodResults.results_multiplicity_rec passive_mult_results = new INCCMethodResults.results_multiplicity_rec();
        public static INCCAnalysisParams.multiplicity_rec passive_mult_params = new INCCAnalysisParams.multiplicity_rec();
        public static INCCAnalysisParams.cal_curve_rec cal_curve_params = new INCCAnalysisParams.cal_curve_rec();
        public static INCCMethodResults.results_cal_curve_rec passive_cal_results = new INCCMethodResults.results_cal_curve_rec();
        public static INCCAnalysisParams.cm_pu_ratio_rec cm_ratio_params = new INCCAnalysisParams.cm_pu_ratio_rec();
        public static INCCMethodResults.results_curium_ratio_rec curium_ratio_results = new INCCMethodResults.results_curium_ratio_rec();
        public static INCCAnalysisParams.known_alpha_rec known_alpha_params = new INCCAnalysisParams.known_alpha_rec();
        public static INCCMethodResults.results_known_alpha_rec known_alpha_results = new INCCMethodResults.results_known_alpha_rec();

        //INCC6 Results
        public static INCCResult InputResult = new INCCResult();
        public static INCCMethodResult InputMethodResult = new INCCMethodResult();

        //All the stuff we need to calculate
        public static List<MultiplicityCountingRes> mcrs = new List<MultiplicityCountingRes>();
        public static List<Cycle> cycles = new List<Cycle>();
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

        //Regex....
        //This regex gets a <label>: <value>
        public static Regex labelValue = new Regex("^\\s*([\\w\\s(\\-*\\%*)]+):\\s+([\\w\\s:.+(\\=*\\^*\\-*)]+)");
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
        public static Regex cycleData = new Regex("^\\s*(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\w)+");
        public static Regex ratesData = new Regex("^\\s*(\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(\\w+)");
        //Distribution
        public static Regex cycleDistHeader = new Regex("^Cycle (\\d+)*");

        //Keep track of test time.
        public static DateTime start;
        public static DateTime end;
        public static TimeSpan ts;
        public static bool verbose = false;
        public static int numMethods;

        static void Main(string[] args)
        {
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
                PrintLine(String.Format("Starting INCC Algorithms test at {0}", start.ToLongTimeString()));
                PrintLine(String.Format("{0} *.ver files found in input directory", inputFiles.Count));
                PrintLine("***********************************************************");
                PrintLine("");
                verbose = args[2] == "verbose";
                int testNum = 0;
                foreach (string testFile in inputFiles)
                {
                    DateTime TestStart = DateTime.Now;
                    PrintLine("***********************************************************");
                    PrintLine(String.Format("Loading test file {0}/{1} at {2}", testNum + 1, inputFiles.Count, TestStart.ToLongTimeString()));
                    PrintLine(String.Format("Test file name: {0}: ", testFile));
                    LoadMeasurementFromFile(testFile);
                    testMeasurement.Detector = det;
                    testMeasurement.Detector.MultiplicityParams = mult;
                    testMeasurement.AnalysisParams = new CountingAnalysisParameters();
                    testMeasurement.AnalysisParams.Add(mult); //Cycles and mult dist are now populated from file.
                    testMeasurement.INCCAnalysisState.Methods.selector = new INCCSelector(testMeasurement.AcquireState.item_id, testMeasurement.AcquireState.item_type);

                    PrintLine("Measurement created from file");
                    PrintLine("Copying parameters and raw data for recalculation");

                    //Copy to new measurement for calculation/comparison
                    Measurement newResultMeasurement = new Measurement(AssaySelector.MeasurementOption.verification, null);
                    newResultMeasurement = BuildMeasurement(testMeasurement);
                    newResultMeasurement.Detector.Id.source = DetectorDefs.ConstructedSource.INCCTransfer;

                    newResultMeasurement.CalcAvgAndSums();
                    newResultMeasurement.CalculateMeasurementResults();
                    newResultMeasurement.CalculateResults();
                    PrintLine("Measurement recalculated");

                    bool compare = true;// CompareParameters(testMeasurement, newResultMeasurement);
                    bool testCompare = true;
                    //Check that Parameters are actually compared in the Equals function.
                    //This is only for diagnostics.
                    //PrintLine(compare ? "Parameters equal" : "Parameters not equal -- FAIL");
                    testCompare = CompareRates(testMeasurement, newResultMeasurement);
                    compare = testCompare && testCompare;

                    testCompare = CompareResults(testMeasurement, newResultMeasurement);
                    compare = testCompare && testCompare;

                    DateTime TestEnd = DateTime.Now;
                    TimeSpan TestTime = TestEnd - TestStart;
                    PrintLine(String.Format("Test {0}/{1} ended at {2}, {3} total milliseconds for this test", testNum + 1, inputFiles.Count, TestStart.ToLongTimeString(), TestTime.Milliseconds));
                    testNum++;
                    PrintLine("***********************************************************");
                    PrintLine("");
                    ClearMeasurement(testMeasurement);
                }
                
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));

                //results.Close();
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in Main: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }

        }

        static void PrintLine(String s)
        {
            Console.WriteLine(s);
            results.WriteLine(s);
            results.Flush();
        }

        static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.ver*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
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

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    //Console.WriteLine(fi.FullName);
                    inputFiles.Add(fi.FullName);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
        public static void Setup()
        {
            det = new Detector();
            mult = new Multiplicity(FAType.FAOn);
            dsid = new DetectorDefs.DataSourceIdentifier();
            acq = new AcquireParameters();
            measID = new MeasId(AssaySelector.MeasurementOption.verification);
            INCCResults.results_rec rec = new INCCResults.results_rec();
            rec.det = det;
            hvp = new HVCalibrationParameters();
            testMeasurement = new Measurement(AssaySelector.MeasurementOption.verification);
            testMeasurement.ResultsFiles = new ResultFiles();
            state = new INCCAnalysisState();
            testMeasurement.INCCAnalysisState = state;
            state.Methods = new AnalysisMethods();
            numMethods = 0;
        }

        public static void ClearMeasurement(Measurement m)
        {
            m.Cycles.Clear();
            m.CountingAnalysisResults.RemoveAll();
            state.Methods = new AnalysisMethods();
            m.INCCAnalysisResults.Clear();
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
                        switch (line.TrimStart(' '))
                        {
                            case "":
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
                                ReadCycleData();
                                break;
                            case "Results":
                            case "Active results":
                                ReadResults();
                                break;
                            //These should be compared at some point.
                            case "Warning messages":
                                ReadWarnings();
                                break;
                            case "Passive error messages":
                                ReadErrors();
                                break;
                            case "Active multiplicity calibration parameters":
                                am = AnalysisMethod.ActiveMultiplicity;
                                state.Methods.AddMethod(am, active_mult_params);
                                state.Methods.choices[(int)AnalysisMethod.ActiveMultiplicity] = true;
                                SetMethodPreference(cc);
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
                                cc = AnalysisMethod.CalibrationCurve;// is this correct?
                                state.Methods.AddMethod(cc,cal_curve_params);
                                state.Methods.choices[(int)AnalysisMethod.CalibrationCurve] = true;
                                SetMethodPreference(cc);
                                ReadPassiveCalibrationCurveCalibration();
                                numMethods++;
                                break;
                            case "Passive multiplicity calibration parameters":
                                cc = AnalysisMethod.Multiplicity;// is this correct?
                                state.Methods.AddMethod(cc, passive_mult_params);
                                state.Methods.choices[(int)AnalysisMethod.Multiplicity] = true;
                                SetMethodPreference(cc);
                                ReadPassiveMultiplicityCalibration();
                                numMethods++;
                                break;
                            case "Passive calibration curve results":
                                ReadPassiveCalibrationCurveResults();
                                break;
                            case "Analysis based on doubles rate."://this is getting skipped because the while loop in Passive Calibration curve parameters reads line and then the while loop rejects and reads next line
                                break;
                            case "PRIMARY RESULT":
                                break;
                            case "Known alpha results":
                                cc = AnalysisMethod.KnownA;// is this correct?
                                state.Methods.AddMethod(cc, known_alpha_params);
                                state.Methods.choices[(int)AnalysisMethod.KnownA] = true;
                                SetMethodPreference(cc);
                                ReadKnownAlphaResults();
                                numMethods++;
                                break;
                            case "END PRIMARY RESULT":
                                break;

                            default:
                                PrintLine(String.Format("Unknown label found in input file {0}.", line));
                                break;
                        }
                        line = sr.ReadLine();
                        lineCount++;
                    }
                }
                //TODO: When all done, store the primary/secondary/tertiary analysis method.
            }
            catch (Exception ex)
            {
                end = DateTime.Now;
                ts = end - start;
                PrintLine(String.Format("Exception in LoadMeasurementFromFile: {0}", ex.Message));
                PrintLine(String.Format("Ending test at {0}", end.ToLongTimeString()));
                PrintLine(String.Format("Total test time {0}", ts.ToString()));
                //results.Close();
            }
        }

        public static void SetMethodPreference(AnalysisMethod cc)
        {
            if (numMethods == 0)
                state.Methods.Normal = cc;
            else if (numMethods == 1)
                state.Methods.Backup = cc;
            else if (numMethods == 3)
                state.Methods.Auxiliary = cc;
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
        public static void ReadHeader()//Also reads background and isotopics if any
        {
            //Read all the header stuff. Always delineated by these two regex
           
            String type = String.Empty;
            String ID = String.Empty;
            String elec_id = String.Empty;
            try
            {
                while ((line.Contains(":") || line == "") && !line.Contains("summed raw data"))
                {
                    Match m = labelValue.Match(line);
                    label = m.Groups[1].Value;
                    switch (label)
                    {
                        case "":
                            line = sr.ReadLine();
                            lineCount++;//Consume extra spaces in header.
                            continue;
                        case "Facility":
                            testMeasurement.AcquireState.facility = new INCCDB.Descriptor(m.Groups[2].Value, "Facility for test.");
                            break;
                        case "Material balance area":
                            testMeasurement.AcquireState.mba = new INCCDB.Descriptor(m.Groups[2].Value, "Test MBA");
                            break;
                        case "Detector type":
                            type = m.Groups[2].Value;
                            break;
                        case "Detector id":
                            ID = m.Groups[2].Value;
                            break;
                        case "Electronics id":
                            elec_id = m.Groups[2].Value;
                            dsid.SetSRType(type);
                            dsid.SetIdDetails(ID, elec_id, "", dsid.SRType);
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
                            mid.MeasOption = AssaySelector.MeasurementOption.verification;
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
                            mos = new MeasOptionSelector(AssaySelector.MeasurementOption.verification, mult);
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
                        case "Passive singles background":
                            t = tuple.Match(line);
                            if (line.TrimStart(' ').StartsWith("Active") ? active = true : active = false)
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedSinglesRate.CopyFrom(ValErr);
                            break;
                        case "Active doubles background":
                        case "Passive doubles background":
                            t = tuple.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedDoublesRate.CopyFrom(ValErr);
                            break;
                        case "Active triples background":
                        case "Passive triples background":
                            t = tuple.Match(line);
                            Double.TryParse(t.Groups[2].Value, out val);
                            Double.TryParse(t.Groups[3].Value, out err);
                            ValErr = new VTuple(val, err);
                            testMeasurement.Background.DeadtimeCorrectedTriplesRate.CopyFrom(ValErr);
                            break;
                        case "Active scaler1 background":
                        case "Passive scaler1 background":
                            double.TryParse(m.Groups[2].Value, out val);
                            ValErr = new VTuple(val, 0);
                            testMeasurement.Background.Scaler1 = ValErr;
                            break;
                        case "Active scaler2 background":
                        case "Passive scaler2 background":
                            double.TryParse(m.Groups[2].Value, out val);
                            ValErr = new VTuple(val, 0);
                            testMeasurement.Background.Scaler2 = ValErr;
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
                            DateTime.TryParseExact (t.Groups[2].Value,
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
                            CalculatedIsotopics.SetValueError(Isotope.am241,val2, err2);
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
        public static void ReadSummedData()
        {
            try
            {
                line = sr.ReadLine(); //Empty line

                line = sr.ReadLine();//Grab Results
                lineCount += 2;
                while (line.Contains(":"))
                {
                    Match m = labelValue.Match(line);
                    label = m.Groups[1].Value;
                    switch (label)
                    {
                        case "Number of good cycles"://Don't know where in results this is.
                            testMeasurement.Cycles = new CycleList();
                            int num = 0;
                            Int32.TryParse(m.Groups[2].Value, out num);
                            testMeasurement.Cycles.Capacity = num;
                            break;
                        case "Total count time":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.TS = TimeSpan.FromSeconds(val);
                            break;
                        case "Shift register singles sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Totals = val;
                            break;
                        case "Shift register reals + accidentals sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.RASum = val;
                            break;
                        case "Shift register accidentals sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.ASum = val;
                            InputResult.UnASum = val;
                            break;
                        case "Shift register 1st scaler sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.S1Sum = val;
                            break;
                        case "Shift register 2nd scaler sum":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.S2Sum = val;
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
                InputResult.NormedAMult = new ulong[Distribution.Count];
                InputResult.UnAMult = new ulong[Distribution.Count];//Again? Normalized?
                for (idx = 0; idx < Distribution.Count; idx++)
                {
                    InputResult.RAMult[idx] = Distribution[idx].Item1;
                    InputResult.UnAMult[idx] = Distribution[idx].Item2;
                    InputResult.NormedAMult[idx] = Distribution[idx].Item2;
                }

                object o = new object();
                testMeasurement.CountingAnalysisResults.TryGetValue(mult, out o);
                MultiplicityCountingRes mcr;
                if (o == null)
                    mcr = new MultiplicityCountingRes();
                else
                    mcr = (MultiplicityCountingRes)o;
                

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
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedSinglesRate.err = val;
                            break;
                        case "Doubles":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.DeadtimeCorrectedDoublesRate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedDoublesRate.err = val;
                            break;
                        case "Triples":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.DeadtimeCorrectedTriplesRate.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.DeadtimeCorrectedTriplesRate.err = val;
                            break;
                        case "Quads":
                            //INCC6 has no quads.
                            break;
                        case "Quads/Triples":
                            //INCC6 has no quads.
                            break;
                        case "Scaler 1":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Scaler1.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.Scaler1.err = val;
                            break;
                        case "Scaler 2":
                            Double.TryParse(m.Groups[2].Value, out val);
                            InputResult.Scaler1.v = val;
                            Double.TryParse(m.Groups[3].Value, out val);
                            InputResult.Scaler1.err = val;
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
        public static void ReadPassiveMultiplicityCalibration ()
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
        public static void ReadCycleData()
        {
            line = sr.ReadLine();//blank
            line = sr.ReadLine();//Count time
            Match m = labelValue.Match(line);//Count time
            long countTime = 0;
            Int64.TryParse(m.Groups[2].Value, out countTime);
            try
            {
                line = sr.ReadLine();//Blank
                line = sr.ReadLine();//header
                line = sr.ReadLine();//data

                lineCount += 3;
                //First raw
                cycles.Clear();
                while (line != "")
                {
                    int idx = 0;
                    Match c;
                    while (line != "")
                    {
                        //Cycle totals
                        c = cycleData.Match(line);
                        Int32.TryParse(c.Groups[1].Value, out idx);
                        cycles.Add(new Cycle(null));
                        mcrs.Add(new MultiplicityCountingRes());

                        if (c.Success)
                        {
                            cycles[idx - 1].TS = new TimeSpan(0, 0, (int)countTime);
                            cycles[idx - 1].seq = idx;
                            Double.TryParse(c.Groups[2].Value, out val);
                            mcrs[idx - 1].Totals = val;
                            cycles[idx - 1].Totals = (ulong)val;
                            mcrs[idx - 1].TS = cycles[idx - 1].TS;
                            mcrs[idx - 1].DeadtimeCorrectedSinglesRate.v = mcrs[idx - 1].RawSinglesRate.v;
                            Double.TryParse(c.Groups[3].Value, out val);
                            mcrs[idx - 1].RASum = val;
                            double.TryParse(c.Groups[4].Value, out val);
                            mcrs[idx - 1].ASum = val;
                            double.TryParse(c.Groups[5].Value, out val);
                            mcrs[idx - 1].Scaler1.v = val;
                            double.TryParse(c.Groups[6].Value, out val);
                            mcrs[idx - 1].Scaler2.v = val;
                            line = sr.ReadLine();
                            lineCount++;
                        }
                    }
                    line = sr.ReadLine();
                    line = sr.ReadLine();
                    while (line != "")
                    {
                        //Cycle rates -- set DTC corrected to raw for now.
                        c = ratesData.Match(line);
                        Int32.TryParse(c.Groups[1].Value, out idx);
                        if (c.Success)
                        {
                            Double.TryParse(c.Groups[2].Value, out val);
                            mcrs[idx - 1].DeadtimeCorrectedSinglesRate.v = val;
                            cycles[idx - 1].SinglesRate = val;
                            Double.TryParse(c.Groups[3].Value, out val);
                            mcrs[idx - 1].rates.DeadtimeCorrectedRates.DoublesRate = val;
                            Double.TryParse(c.Groups[4].Value, out val);
                            mcrs[idx - 1].rates.DeadtimeCorrectedRates.TriplesRate = val;
                            Double.TryParse(c.Groups[5].Value, out val);
                            mcrs[idx - 1].mass = val;
                            cycles[idx - 1].SetQCStatus(mult, QCTestStatusExtensions.FromString(c.Groups[6].Value));
                        }
                        line = sr.ReadLine();
                        lineCount++;
                    }

                    for (int i = 0; i < cycles.Count; i++)
                    {
                        cycles[i].CountingAnalysisResults.Add(mult, mcrs[i]);
                        testMeasurement.Cycles.Add(cycles[i]);
                    }
                    testMeasurement.CountingAnalysisResults[mult] = new MultiplicityCountingRes() ;
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
            MultiplicityCountingRes summcr = new MultiplicityCountingRes();
            //created summed mcr arrays
            ulong[] sumRA = new ulong[1];
            ulong[] sumA = new ulong[1];
            ulong[] sumnormedA = new ulong[1];
            for (int i = 0; i < testMeasurement.Cycles.Count; i ++)
            {
                line = sr.ReadLine();
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
                MultiplicityCountingRes mcr1 = (MultiplicityCountingRes)testMeasurement.Cycles[i].CountingAnalysisResults[mult];
                ulong[] RA = new ulong[numbers.Count];
                ulong[] A = new ulong[numbers.Count];
                ulong[] normedA = new ulong[numbers.Count];
                if (i==0)
                {
                    //created summed mcr arrays
                    sumRA = new ulong[numbers.Count];
                    sumA = new ulong[numbers.Count];
                    sumnormedA = new ulong[numbers.Count];
                }
                for (int j = 0; j < numbers.Count; j++)
                {
                    Match m = ThreeNums.Match(numbers[j]);
                    UInt64.TryParse(m.Groups[2].Value, out Ulon);
                    RA[j] = Ulon;
                    sumRA[j] += Ulon;
                    UInt64.TryParse(m.Groups[3].Value, out Ulon);
                    A[j] = Ulon;
                    sumA[j] += Ulon;
                    normedA[j] = Ulon;
                    sumnormedA[j] += Ulon;
                }
                mcr1.RAMult = new ulong[numbers.Count];
                mcr1.UnAMult = new ulong[numbers.Count];
                mcr1.NormedAMult = new ulong[numbers.Count];
                mcr1.MaxBins = (ulong)numbers.Count;
                RA.CopyTo(mcr1.RAMult,0);
                A.CopyTo(mcr1.UnAMult, 0);
                normedA.CopyTo(mcr1.NormedAMult,0);
                numbers.Clear();
            }
            summcr = (MultiplicityCountingRes)testMeasurement.CountingAnalysisResults[mult];
            summcr.RAMult = new ulong[sumRA.Length];
            summcr.UnAMult = new ulong[sumA.Length];
            summcr.NormedAMult = new ulong[sumnormedA.Length];
            sumRA.CopyTo(summcr.RAMult,0);
            sumA.CopyTo(summcr.UnAMult, 0);
            sumnormedA.CopyTo(summcr.NormedAMult, 0);
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

        }
        public static INCCAnalysisParams.CurveEquation CurveEquationStringToEnum (string s)
        {
            /*CUBIC, // a + bm + cm^2 + dm^3
            POWER, // am^b
            HOWARDS, // am / (1 + bm)
            EXPONENTIAL // a (1 - exp(bm))*/
            INCCAnalysisParams.CurveEquation eq = INCCAnalysisParams.CurveEquation.CUBIC;
            switch (s)
            {
                case "a + bm + cm^2 + dm^3":
                    eq= INCCAnalysisParams.CurveEquation.CUBIC;
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

        }
        public static void ReadKnownAlphaCalibration()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            lineCount += 2;

            double alpha_weight = 0.0;
            double rho_zero = 0.0;
            double k = 0.0;
            double a = 0.0;
            double b = 0.0;
            double variance_a = 0.0;
            double variance_b = 0.0;
            double covariance_ab = 0.0;
            double sigma_x = 0.0;

            while (line.Contains(":"))
            {
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {

                    case "Alpha weight":
                        double.TryParse(m.Groups[2].Value, out alpha_weight);
                        known_alpha_params.alpha_wt = alpha_weight;
                        break;
                    case "Rho zero":
                        double.TryParse(m.Groups[2].Value, out rho_zero);
                        known_alpha_params.rho_zero = rho_zero;
                        break;
                    case "k":
                        double.TryParse(m.Groups[2].Value, out k);
                        known_alpha_params.k = k;
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
                        double.TryParse(m.Groups[2].Value, out sigma_x);
                        known_alpha_params.cev.sigma_x = sigma_x;
                        break;
                    default:
                        PrintLine(String.Format("Unknown label found in input file {0}.", line));
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }

        }

        static bool CompareParameters (Measurement original, Measurement recalculated)
        {
            bool same = true;//overall success
            bool temp = true;//stepwise success
            temp = original.Detector.CompareTo(recalculated.Detector) == 0;
            same = temp;
            PrintLine(String.Format("Detector parameters compared: {0}", temp ? "PASS" : "FAIL"));
            temp = original.INCCAnalysisState.Methods.Equals(recalculated.INCCAnalysisState.Methods);
            same = temp && same;
            PrintLine(String.Format("Method parameters compared: {0}", temp? "PASS" : "FAIL"));
            return same;          
        }

        static bool CompareRates (Measurement original, Measurement recalculated)
        {
            //Compare cycles and averages.
            bool same = true;
            bool temp = true;
            string[] lines;
            List<string> ls;

            PrintLine("Compare each cycle");
            for (int i = 0; i < original.Cycles.Count; i++)
            {
                MultiplicityCountingRes mcr1 = (MultiplicityCountingRes)original.Cycles[i].CountingAnalysisResults[mult];
                MultiplicityCountingRes mcr2 = (MultiplicityCountingRes)recalculated.Cycles[i].CountingAnalysisResults[mult];
                temp = mcr1.Equals(mcr2);

                PrintLine(String.Format("Cycle{0}: {1}", i+1, temp ? "PASS" : "FAIL"));
                ls = mcr1.TestMessages;
                if (ls != null)
                {
                    foreach (string s in ls)
                        PrintLine(s);
                }
                same = temp && same;
            }
            IEnumerator iter = original.CountingAnalysisResults.GetMultiplicityEnumerator();
            IEnumerator iter2 = recalculated.CountingAnalysisResults.GetMultiplicityEnumerator();

            PrintLine("Compare multiplicity counting for entire measurement:");
            while (iter.MoveNext())
            {
                iter2.MoveNext();
                Multiplicity mkey = (Multiplicity)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Key;
                MultiplicityCountingRes mcr = (MultiplicityCountingRes)((KeyValuePair<SpecificCountingAnalyzerParams, object>)(iter.Current)).Value;
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
                temp = mcr.Equals(mcr2);
                ls = mcr.TestMessages;
                if (ls != null)
                {
                    foreach (string s in ls)
                        PrintLine(s);
                }
                same = temp && same;
            }
            same = temp && same;
            PrintLine(temp ? "CountingResults match" : "CountingResults different: FAIL");
            return same;
        }

        static bool CompareResults(Measurement original, Measurement recalculated)
        {
            //Compare results and method results.
            bool same = true;
            bool temp = true;
            IEnumerator ienum = original.INCCAnalysisState.Methods.GetMethodEnumerator();
            while (ienum.MoveNext())
            {
                AnalysisMethod am = ((Tuple < AnalysisMethod, INCCAnalysisParams.INCCMethodDescriptor >) ienum.Current).Item1;
                INCCMethodResults imrs1;
                INCCMethodResults imrs2;
                original.INCCAnalysisResults.TryGetINCCResults(mult, out imrs1);
                recalculated.INCCAnalysisResults.TryGetINCCResults(mult, out imrs2);
                IEnumerator enum1 = imrs1.GetEnumerator();
                IEnumerator enum2 = imrs2.GetEnumerator();
                while (enum1.MoveNext())
                {
                    enum2.MoveNext();
                    KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>> kvp1 = (KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>)enum1.Current;
                    KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>> kvp2 = (KeyValuePair<INCCSelector, Dictionary<AnalysisMethod, INCCMethodResult>>)enum2.Current;
                    foreach (KeyValuePair<AnalysisMethod, INCCMethodResult> methodres in kvp1.Value)
                    {
                        PrintLine(String.Format("Comparing results for {0}", methodres.Key.FullName()));
                        INCCMethodResult mr2;
                        kvp2.Value.TryGetValue(methodres.Key, out mr2);
                        temp = methodres.Value.Equals(mr2);

                        if (temp)
                            PrintLine(String.Format("Analysis results comparison for {0}: PASS", am.ToString()));
                        else
                        {
                            PrintLine ("Failure of results comparison");
                        }
                        same = temp && same;
                    }

                }



            }
            PrintLine(temp ? "All analysis Method Results match" : "At least one analysis result different: FAIL");
            return same;
        }

        //Build a measurement fresh from existing. No DB or "internal lameness" required.
        //Copy all but results.
        static Measurement BuildMeasurement (Measurement meas)
        {
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(det),
                                   tp,
                                   np,
                                   bp,
                                   iso,
                                   acq,
                                   hvp);
            det.Id.source = det.Id.source;  // set the detector overall data source value here

            // create the context holder for the measurement. Everything is rooted here ...
            Measurement copied = new Measurement(mt, meas.MeasOption, null);
            copied.Detector = meas.Detector;
            copied.AnalysisParams = meas.AnalysisParams;
            copied.INCCAnalysisState = meas.INCCAnalysisState;
            copied.INCCAnalysisState.Methods = meas.INCCAnalysisState.Methods;
            copied.InitializeResultsSummarizers();
            copied.InitializeContext(clearCounterResults: true);
            copied.PrepareINCCResults();
            foreach (Cycle c in meas.Cycles)
                copied.Cycles.Add(c);
            copied.CountingAnalysisResults.Remove(mult);
            copied.CountingAnalysisResults.Add(mult, meas.CountingAnalysisResults[mult]);

            copied.Stratum = new Stratum();
            copied.Stratum.CopyFrom(meas.Stratum);

            INCCResults.results_rec xres = new INCCResults.results_rec();
            copied.INCCAnalysisResults.TradResultsRec = xres;

            return copied;
/*
            // gather it all together
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(det),
                                    tp,
                                    np,
                                    bp,
                                    iso,
                                    acq,
                                    hvp);
            det.Id.source = det.Id.source;  // set the detector overall data source value here

            // create the context holder for the measurement. Everything is rooted here ...
            Measurement copied = new Measurement(mt, meas.MeasOption,null);
            //meas.InitializeContext(clearCounterResults: true);

            copied.Detector = meas.Detector;
            copied.CountingAnalysisResults.Remove(mult);
            copied.CountingAnalysisResults.Add(mult, meas.CountingAnalysisResults[mult]);
            copied.INCCAnalysisState = meas.INCCAnalysisState;
            copied.AcquireState = meas.AcquireState;
            copied.AnalysisParams = meas.AnalysisParams;
            copied.INCCAnalysisState.Methods = meas.INCCAnalysisState.Methods;
            copied.INCCAnalysisState.PrepareINCCMethodResults(mult, new INCCSelector(meas.AcquireState.item_id, meas.AcquireState.item_type),copied);

            foreach (Cycle c in meas.Cycles)
            {
                copied.Cycles.Add(c);
            }
            copied.Stratum = new Stratum();
            copied.Stratum.CopyFrom(meas.Stratum);

            return copied;*/


        }


        ~Program()
        {
            PrintLine("Finalizing object");
            results.Flush();
            results.Close();
        }
    }
    
}
    

