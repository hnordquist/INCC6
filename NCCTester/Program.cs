using System;
using System.Collections;
using System.Collections.Generic;
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
        
        private static LMLoggers.LognLM TestLogger;
        public static String testFile;

        public static String version;
        public static Detector det;
        public static DetectorDefs.DataSourceIdentifier dsid;
        public static Multiplicity mult;
        public static Measurement testMeasurement;
        public static MeasId measID;
        public static HVCalibrationParameters hv;
        public static AnalysisMethod am;

        //Results loaded from file.
        public static INCCAnalysisState state;
        public static INCCResult TestVersionResult = new INCCResult();
        public static INCCMethodResult TestVersionMethodResult = new INCCMethodResult();
        public static INCCAnalysisParams.active_mult_rec active_mult_params = new INCCAnalysisParams.active_mult_rec();
        public static NCCTransfer.results_active_mult_rec active_mult_results = new results_active_mult_rec();

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
        public static ulong Ulon = 0;

        //Regex....
        //This regex gets a <label>: <value>
        public static Regex labelValue = new Regex("^\\s*([\\w\\s()]+):\\s+([\\w\\s:.+]+)");
        //This one gets a <label>: <tuplepart1> +- <tuplepart2>
        public static Regex tuple = new Regex("^\\s+([\\w\\s/]+):\\s+([-?\\w\\s.]+)[+-]+\\s+([-?\\w\\s.]+)");
        //Triad of numbers
        public static Regex ThreeNums = new Regex("^\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)");
        //cycle table
        public static Regex cycleData = new Regex("^\\s*(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\d+)\\s+(\\w)+");
        public static Regex ratesData = new Regex("^\\s*(\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(-?\\d+.\\d+)\\s+(\\w+)");
        //Distribution
        public static Regex cycleDistHeader = new Regex("^Cycle (\\d+)*");

        public static void Setup()
        {
            Console.WriteLine("Creating Measurement....");
            det = new Detector();
            mult = new Multiplicity(FAType.FAOn);
            dsid = new DetectorDefs.DataSourceIdentifier();
            acq = new AcquireParameters();
            measID = new MeasId(AssaySelector.MeasurementOption.verification);
            INCCResults.results_rec rec = new INCCResults.results_rec();
            rec.det = det;
            hv = new HVCalibrationParameters();
            testMeasurement = new Measurement(AssaySelector.MeasurementOption.verification) ;
            testMeasurement.ResultsFiles = new ResultFiles();
            state = new INCCAnalysisState();
            testMeasurement.INCCAnalysisState = state;
            testFile = "C:\\CODE\\INCC7\\TestData\\ActiveMultiplicity\\713P0048.VER";

        }
        public static void LoadMeasurementFromFile(string fileName)
        {
            try
            {
                using (sr = new StreamReader(fileName))
                {

                    line = sr.ReadLine();//Version comes first.
                    string[] vers = line.Split();
                    version = vers[1];
                    line = sr.ReadLine();//Consume blank line
                    line = sr.ReadLine();//First line of header
                    lineCount += 3;
                    ReadHeader();

                    while (sr.Peek() != -1)
                    {
                        switch (line)
                        {
                            case "":
                                line= sr.ReadLine();
                                lineCount++;
                                continue;
                            case "Active summed raw data":
                                ReadActiveSummedData();
                                break;
                            case "Active summed multiplicity distributions":
                                ReadActiveSummedMult();
                                break;
                            case "Active results":
                                ReadActiveResults();
                                break;
                            case "Active multiplicity calibration parameters":
                                am = AnalysisMethod.ActiveMultiplicity;
                                INCCAnalysisParams.INCCMethodDescriptor imd = new INCCAnalysisParams.INCCMethodDescriptor();
                                state.Methods = new AnalysisMethods();
                                state.Methods.AddMethod(am, imd);
                                state.Methods.Normal = am;
                                ReadActiveMultiplicityCalibration();
                                break;
                            case "Active cycle data":
                                ReadActiveCycleData();
                                break;
                            case "Active multiplicity distributions":
                                ReadActiveMultDistributions();
                                break;
                            default:
                                break;
                        }
                        line = sr.ReadLine();
                        lineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public static void ReadHeader()
        {
            //Read all the header stuff. Always delineated by these two regex
            Console.WriteLine(String.Format("Reading INCC results file {0}",testFile));
            String type = String.Empty;
            String ID = String.Empty;
            String elec_id = String.Empty;
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
                        testMeasurement.MeasurementId = new MeasId(m.Groups[2].Value);
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
                    case "comment":
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
                    case "Active singles background"://Are the background rates raw or DTC? I think DTC
                        t = tuple.Match(line);
                        active = true;
                        Double.TryParse(t.Groups[2].Value, out val);
                        Double.TryParse(t.Groups[3].Value, out err);
                        ValErr = new VTuple(val, err);
                        testMeasurement.Background.DeadtimeCorrectedSinglesRate.CopyFrom(ValErr);
                        break;
                    case "Active doubles background":
                        t = tuple.Match(line);
                        Double.TryParse(t.Groups[2].Value, out val);
                        Double.TryParse(t.Groups[3].Value, out err);
                        ValErr = new VTuple(val, err);
                        testMeasurement.Background.DeadtimeCorrectedDoublesRate.CopyFrom(ValErr);
                        break;
                    case "Active triples background":
                        t = tuple.Match(line);
                        Double.TryParse(t.Groups[2].Value, out val);
                        Double.TryParse(t.Groups[3].Value, out err);
                        ValErr = new VTuple(val, err);
                        testMeasurement.Background.DeadtimeCorrectedTriplesRate.CopyFrom(ValErr);
                        break;
                    case "Active scaler1 background":
                        double.TryParse(m.Groups[2].Value, out val);
                        ValErr = new VTuple(val, 0);
                        testMeasurement.Background.Scaler1 = ValErr;
                        break;
                    case "Active scaler2 background":
                        double.TryParse(m.Groups[2].Value, out val);
                        ValErr = new VTuple(val, 0);
                        testMeasurement.Background.Scaler2 = ValErr;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", testFile, lineCount);
                        break;
                }

                line = sr.ReadLine();
                lineCount++;
            }
        }
        public static void ReadActiveSummedData()
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
                        System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", testFile, lineCount + 1);
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
            }
        }
        public static void ReadActiveSummedMult()
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
            InputResult.UnAMult = new ulong[Distribution.Count];//Again? Normalized?
            for (idx = 0; idx < Distribution.Count; idx++)
            {
                InputResult.RAMult[idx] = Distribution[idx].Item1;
                InputResult.UnAMult[idx] = Distribution[idx].Item2;
            }
        }
        public static void ReadActiveResults()
        {
            line = sr.ReadLine();//Consume blank;
            line = sr.ReadLine();
            lineCount += 2;
            while (line.Contains(":"))
            {
                Match m = tuple.Match(line);
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
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", testFile, lineCount + 1);
                        break;

                }
                line = sr.ReadLine();
                lineCount++;
            }

        }
        public static void ReadActiveMultiplicityResults()
        {

            line = sr.ReadLine();//Empty line
            line = sr.ReadLine();
            Match m = tuple.Match(line);

            //Save results. We have to enter all the method params first.
            double multiplication = 0;
            double mult_err = 0;
            double.TryParse(m.Groups[2].Value, out multiplication);
            double.TryParse(m.Groups[3].Value, out mult_err);
            active_mult_results.am_mult = multiplication;
            active_mult_results.am_mult_err = mult_err;
            line = sr.ReadLine();//empty
            line = sr.ReadLine();
            lineCount += 2;
        }
        public static void ReadActiveMultiplicityCalibration()
        {
            //Grab params
            line = sr.ReadLine(); //empty
            line = sr.ReadLine();
            lineCount += 2;
            double Firstfacttherm = 0.0;
            double Secondfacttherm = 0.0;
            double Thirdfacttherm = 0.0;
            double Firstfactfast = 0.0;
            double Secondfactfast = 0.0;
            double Thirdfactfast = 0.0;

            while (line.Contains(":"))
            {
                Match m = labelValue.Match(line);
                switch (m.Groups[1].Value)
                {
                    case "1st factorial moment thermal neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Firstfacttherm);
                        break;
                    case "2nd factorial moment thermal neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Secondfacttherm);
                        break;
                    case "3rd factorial moment thermal neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Thirdfacttherm);
                        break;
                    case "1st factorial moment fast neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Firstfactfast);
                        break;
                    case "2nd factorial moment fast neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Secondfactfast);
                        break;
                    case "3rd factorial moment fast neutron induced fission U235":
                        double.TryParse(m.Groups[2].Value, out Thirdfactfast);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", testFile, lineCount + 1);
                        break;
                }
                line = sr.ReadLine();
                lineCount++;
                
            }

        }
        public static void ReadActiveCycleData()
        {
            line = sr.ReadLine();//blank
            line = sr.ReadLine();//Count time
            Match m = labelValue.Match(line);//Count time
            long countTime = 0;
            Int64.TryParse(m.Groups[2].Value, out countTime);
            line = sr.ReadLine();//Blank
            line = sr.ReadLine();//header
            line = sr.ReadLine();//data

            lineCount += 3;
            //First raw
            {
                while (line != "")
                {
                    int idx = 0;
                    Match c;
                    
                    while (line != "")
                    {
                        //Cycle totals
                        c = cycleData.Match(line);
                        Int32.TryParse(c.Groups[1].Value, out idx);
                        mcrs.Add(new MultiplicityCountingRes());
                        cycles.Add(new Cycle(null));
                        if (c.Success)
                        {
                            cycles[idx - 1].TS = new TimeSpan(0, 0, (int)countTime);
                            cycles[idx - 1].seq = idx;
                            Double.TryParse(c.Groups[2].Value, out val);
                            mcrs[idx-1].Totals = val;
                            cycles[idx - 1].Totals = (ulong)val;
                            mcrs[idx-1].TS = cycles[idx - 1].TS;
                            mcrs[idx - 1].DeadtimeCorrectedSinglesRate.v = mcrs[idx - 1].RawSinglesRate.v;
                            Double.TryParse(c.Groups[3].Value, out val);
                            mcrs[idx-1].RASum = val;
                            double.TryParse(c.Groups[4].Value, out val);
                            mcrs[idx-1].ASum = val;
                            double.TryParse(c.Groups[5].Value, out val);
                            mcrs[idx-1].Scaler1.v = val;
                            double.TryParse(c.Groups[6].Value, out val);
                            mcrs[idx-1].Scaler2.v = val;

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
                            mcrs[idx-1].mass = val;
                            cycles[idx-1].SetQCStatus(mult,QCTestStatusExtensions.FromString(c.Groups[6].Value));
                        }
                        line = sr.ReadLine();
                        lineCount++;
                    }

                    testMeasurement.CountingAnalysisResults.Add(mult, mcrs[0]);
                    for (int i = 0; i < cycles.Count; i++ )
                    {
                        cycles[i].CountingAnalysisResults.Add (mult,mcrs[i]);
                        testMeasurement.Cycles.Add(cycles[i]);
                    }
                }
            }

        }
        public static void ReadActiveMultDistributions()
        {
            List<String> numbers = new List<String>();
            List<ulong[]> RAdists = new List<ulong[]>();
            List<ulong[]> Adists = new List<ulong[]>();
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
                for (int j = 0; j < numbers.Count; j++)
                {
                    Match m = ThreeNums.Match(numbers[j]);
                    UInt64.TryParse(m.Groups[2].Value, out Ulon);
                    RA[j] = Ulon;
                    UInt64.TryParse(m.Groups[3].Value, out Ulon);
                    A[j] = Ulon;
                }
                mcr1.RAMult = new ulong[numbers.Count];
                mcr1.UnAMult = new ulong[numbers.Count];
                RA.CopyTo(mcr1.RAMult,0);
                A.CopyTo (mcr1.UnAMult,0);
            }
        }
        public static void DoCycleProcessing()
        {

            foreach (Cycle c in cycles)
            {
                AnalysisDefs.CycleProcessing.ApplyTheCycleConditioningSteps(c, testMeasurement);
            }
        }
        static void Main(string[] args)
        {
            Setup();
            //TestLogger = new LMLoggers.LognLM("Test");
            Console.WriteLine("loading test file " + testFile);
            LoadMeasurementFromFile(testFile);
            testMeasurement.Detector = det;
            testMeasurement.Detector.MultiplicityParams = mult;
            testMeasurement.AnalysisParams = new CountingAnalysisParameters();
            testMeasurement.AnalysisParams.Add(mult); //Cycles and mult dist are now populated from file.
            //NEXT: Check this. The results file gives us DTC rates. Must be sure to start from raw if recalculating HN 4/16/2018

            //Copy to new measurement for comparison
            Console.WriteLine("now calculating new measurement for test comparison");
            //Measurement newResultMeasurement = BuildMeasurement(testMeasurement);
            // Geez, will I ever get all the inner stuff dependent on INCC app cleared out? HN 4/16/2018
            /*if (newResultMeasurement.Cycles.GetValidCycleCount() > 0) // INCC5 Pu mass calcs
            {
                INCCAnalysis.OutlierProcessing (mult,newResultMeasurement); // summary pass at the end of all the cycles
                newResultMeasurement.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
                newResultMeasurement.CalculateResults();
            }
            else // everything else
                newResultMeasurement.GenerateCycleCountingSummaries(ignoreSuspectResults: false);
            Console.WriteLine("comparing results for " + testFile);
            newResultMeasurement.CalculateMeasurementResults();*/
            
            //ol TestSuccess = CompareResults(testMeasurement, newResultMeasurement);
        }

        static bool CompareResults(Measurement original, Measurement recalculated)
        {
            CompareParameters(original, recalculated);
            //CompareRates();
            //CompareMultiplicityDistrubutions();
            //CompareMeasurementResults();
            return true;
        }
        static bool CompareParameters (Measurement original, Measurement recalculated)
        {
            bool same = true;
            Console.WriteLine("comparing detector info");
            same = original.Detector.CompareTo(recalculated.Detector)==0;
            Console.WriteLine(same ? "detectors same" : "detector mismatch");
            Console.WriteLine("comparing methods");
            same = original.INCCAnalysisState.Methods.Equals (recalculated.INCCAnalysisState.Methods);
            Console.WriteLine(same ? "methods same" : "methods mismatch");
            Console.WriteLine("comparing each method parameter set");
            IEnumerator iter =  original.INCCAnalysisState.Methods.GetMethodEnumerator();
            while (iter.MoveNext())
            {
                Console.WriteLine("comparing method parameter details for " );
            }
            return same;
        }
        //Build a measurement fresh from existing. No DB or "internal lameness" required.
        //Copy all but results.
        static Measurement BuildMeasurement (Measurement meas)
        {
            // gather it all together
            MeasurementTuple mt = new MeasurementTuple(new DetectorList(det),
                                    tp,
                                    np,
                                    bp,
                                    iso,
                                    acq,
                                    hv);
            det.Id.source = det.Id.source;  // set the detector overall data source value here

            // create the context holder for the measurement. Everything is rooted here ...
            Measurement copied = new Measurement(mt, meas.MeasOption, TestLogger);
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
        }
    }
}
    

