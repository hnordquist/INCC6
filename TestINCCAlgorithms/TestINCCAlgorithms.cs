using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisDefs;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;

namespace TestINCCAlgorithms
{
    [TestClass]
    public class TestINCCAlgorithms
    {
        public String version;
        public Detector det;
        public DetectorDefs.DataSourceIdentifier dsid;
        public Multiplicity mult;
        public Measurement passiveMeasurement;
        public Measurement activeMeasurement;
        public String testFile;
        INCCResults results = new INCCResults();
        INCCResult InputResult = new INCCResult();

        void Setup()
        {
            det = new Detector();
            mult = new Multiplicity(FAType.FAOn);
            passiveMeasurement = new Measurement(AssaySelector.MeasurementOption.verification);
            dsid = new DetectorDefs.DataSourceIdentifier();
            passiveMeasurement.ResultsFiles = new ResultFiles();
            testFile = "C:\\CODE\\INCC6 Test Suite\\Test INCCAlgorithms\\data\\713P0048.VER";
            
        }
        void LoadMeasurementFromFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    //First thing in file is the version. For this iteration, all are 5.1.2
                    string line = sr.ReadLine();
                    int lineCount = 1;
                    string[] elements = line.Split(new char[] { });
                    version = elements[1];
                    line = sr.ReadLine(); // Empty line
                    String ID = String.Empty;
                    String type = String.Empty;
                    String elec_id = String.Empty;
                    Stratum strat = new Stratum();
                    double val = 0;
                    double err = 0;
                    string[] formats = new string []{ "yy.MM.dd HH:mm:ss" };
                    //This regex gets a <label>: <value>
                    Regex split = new Regex("^\\s+([\\w\\s]+):\\s+([\\w\\s:.]+)");
                    //This one gets a <label>: <tuplepart1> += <tuplepart2>
                    Regex tuple = new Regex("^\\s+([\\w\\s]+):\\s+([\\w\\s.]+)[+-]+\\s+([\\w\\s.]+)");
                    Match m;
                    Match t;
                    String label;
                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();
                        lineCount++;

                        while (!line.Contains("summed raw data"))
                        {
                            //Read all the header stuff. Always delineated by these two regex
                            m = split.Match(line);
                            t = tuple.Match(line);
                            label =m.Groups[1].Value;
                            switch (label)
                            {
                                case "Facility":
                                    passiveMeasurement.AcquireState.facility = new INCCDB.Descriptor(m.Groups[2].Value, "Facility for test.");
                                    break;
                                case "Material balance area":
                                    passiveMeasurement.AcquireState.mba = new INCCDB.Descriptor(m.Groups[2].Value, "Test MBA");
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
                                    passiveMeasurement.AcquireState.inventory_change_code = m.Groups[2].Value;
                                    break;
                                case "I / O code":
                                    break;
                                case "Measurement date":
                                    DateTime dt;
                                    DateTime.TryParseExact(m.Groups[2].Value, formats, null,DateTimeStyles.AllowInnerWhite, out dt);//17.01.03  15:00:48
                                    passiveMeasurement.MeasDate = dt;
                                    break;
                                case "Results file name":
                                    passiveMeasurement.ResultsFiles.Add (new ResultFile (m.Groups[2].Value));
                                    break;
                                case "Inspection number": 
                                    passiveMeasurement.AcquireState.campaign_id = m.Groups[2].Value;
                                    break;
                                case "Item id":
                                    passiveMeasurement.MeasurementId = new MeasId(m.Groups[2].Value);
                                    break;
                                case "Stratum id":
                                    passiveMeasurement.AcquireState.stratum_id = new INCCDB.Descriptor (m.Groups[2].Value,"Test strata");
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
                                    passiveMeasurement.Stratum = strat;
                                    break;
                                case "Material type":
                                    passiveMeasurement.AcquireState.item_type = m.Groups[2].Value;
                                    break;
                                case "Original declared mass":
                                    Double.TryParse(m.Groups[2].Value, out val);
                                    passiveMeasurement.AcquireState.mass = val;
                                    break;
                                case "Measurement option"://Don't care. We know this from file extension at start.
                                    break;
                                case "Data source":
                                    passiveMeasurement.AcquireState.data_src = DetectorDefs.ConstructedSource.CycleFile;
                                    break;
                                case "QC tests":
                                    passiveMeasurement.AcquireState.qc_tests = m.Groups[2].Value == "ON" ? true : false;
                                    break;
                                case "Error calculation":
                                    passiveMeasurement.AcquireState.error_calc_method = m.Groups[2].Value == "Sample method" ? ErrorCalculationTechnique.Sample : ErrorCalculationTechnique.Theoretical;
                                    break;
                                case "Accidentals method":
                                    passiveMeasurement.Tests.accidentalsMethod = m.Groups[2].Value == "Measured" ? AccidentalsMethod.Measure : AccidentalsMethod.Calculate;
                                    break;
                                case "Inspector name":
                                    passiveMeasurement.AcquireState.user_id = m.Groups[2].Value;
                                    break;
                                case "comment":
                                    passiveMeasurement.AcquireState.comment = m.Groups[2].Value;
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
                                    mult.sr.dieAwayTime = val*10;
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
                                    Double.TryParse(t.Groups[2].Value, out val);
                                    Double.TryParse(t.Groups[3].Value, out err);
                                    VTuple ValErr = new VTuple(val,err);
                                    passiveMeasurement.Norm.currNormalizationConstant.CopyFrom(ValErr);
                                    break;
                                case "Active singles background"://Are the background rates raw or DTC? I think DTC
                                    Double.TryParse(t.Groups[2].Value, out val);
                                    Double.TryParse(t.Groups[3].Value, out err);
                                    ValErr = new VTuple(val, err);
                                    passiveMeasurement.Background.DeadtimeCorrectedSinglesRate.CopyFrom(ValErr);
                                    break;
                                case "Active doubles background":
                                    Double.TryParse(t.Groups[2].Value, out val);
                                    Double.TryParse(t.Groups[3].Value, out err);
                                    ValErr = new VTuple(val, err);
                                    passiveMeasurement.Background.DeadtimeCorrectedDoublesRate.CopyFrom(ValErr);
                                    break;
                                case "Active triples background": 
                                    Double.TryParse(t.Groups[2].Value, out val);
                                    Double.TryParse(t.Groups[3].Value, out err);
                                    ValErr = new VTuple(val, err);
                                    passiveMeasurement.Background.DeadtimeCorrectedTriplesRate.CopyFrom(ValErr);
                                    break;
                                case "Active scaler1 background":
                                    double.TryParse(m.Groups[2].Value, out val);
                                    ValErr = new VTuple(val, 0);
                                    passiveMeasurement.Background.Scaler1 = ValErr;
                                    break;
                                case "Active scaler2 background":
                                    double.TryParse(m.Groups[2].Value, out val);
                                    ValErr = new VTuple(val, 0);
                                    passiveMeasurement.Background.Scaler2 = ValErr;
                                    break;
                                case "":
                                    //empty line, just skip
                                    break;
                                default:
                                    System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", fileName, lineCount);
                                    break;
                            }
                            line = sr.ReadLine();
                            lineCount++;
                        }
                        //Now the average rates
                        line = sr.ReadLine();//Empty line
                        line = sr.ReadLine();
                        lineCount += 2;
                        if (line.Contains("summed raw data"))
                        {
                            line = sr.ReadLine(); //Empty line
                            //Grab Results
                            line = sr.ReadLine();
                            lineCount++;
                            while (line.Contains(":"))
                            {
                                m = split.Match(line);
                                label = m.Groups[1].Value;
                                switch (label)
                                {
                                    case "Number of good cycles"://Don't know where in results this is.
                                        
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
                                        break;
                                    case "Shift register 1st scaler sum":
                                        Double.TryParse(m.Groups[2].Value, out val);
                                        InputResult.S1Sum = val;
                                        break;
                                    case "Shift register 2nd scaler sum":
                                        Double.TryParse(m.Groups[2].Value, out val);
                                        InputResult.S2Sum = val;
                                        break;
                                    default:
                                        System.Diagnostics.Debug.WriteLine("Unknown label found in input file {0} at line {1}.", fileName, lineCount);
                                        break;
                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}.", ex.Message);

            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            Setup();
            LoadMeasurementFromFile("C:\\CODE\\INCC6 Test Suite\\Test INCCAlgorithms\\data\\713P0048.VER");
        }
    }
}
