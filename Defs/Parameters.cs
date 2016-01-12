/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016, Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Reflection;
using DetectorDefs;
namespace AnalysisDefs
{
    using Tuple = VTuple;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The parameters required to acquire/take/reify and preserve a measurement
    /// dev note: may need to be modified to associate the params with each detector on the list, not the present design assuming one detector with shared TP, Norm and Bkg params
    /// </summary>
    public class MeasurementTuple : Tuple<DetectorList,
        TestParameters, // a DB global
        NormParameters, // with the detector
        BackgroundParameters,// with the detector
        Isotopics, // a DB global
        AcquireParameters, 
        HVCalibrationParameters> // remaining measurement state goes here 
    //Stratum> // optional stratum declaration, can be null
    {
        public DetectorList Detectors
        {
            get { return Item1; }
        }

        public MeasurementTuple(DetectorList d, TestParameters t, NormParameters n, BackgroundParameters bkg, Isotopics iso, AcquireParameters acq, HVCalibrationParameters hv)
            : base(d, t, n, bkg, iso, acq, hv) // might need a deep copy later
        {
        }
        public MeasurementTuple()  // never permit null members
            : base(new DetectorList(), new TestParameters(), new NormParameters(), new BackgroundParameters(), new Isotopics(), new AcquireParameters(), new HVCalibrationParameters())
        {
        }
    }



    public class ShiftRegisterParameters : ParameterBase, IEquatable<ShiftRegisterParameters>, IComparable<ShiftRegisterParameters>
    {

        public UInt64 gateLength; // tics (1e-7), unlike INCC (1e-6)
        public UInt64 predelay;  // tics (1e-7),  unlike INCC (1e-6)
        public double deadTimeCoefficientTinNanoSecs;
        public double deadTimeCoefficientMultiplicityinNanoSecs  /// alias for T used in INCC
        {
            get { return deadTimeCoefficientTinNanoSecs; }
            set { deadTimeCoefficientTinNanoSecs = value; }
        }
        public double deadTimeCoefficientAinMicroSecs;
        public double deadTimeCoefficientBinPicoSecs;
        public double deadTimeCoefficientCinNanoSecs;

        public double dieAwayTime; // tics (1e-7), unlike INCC (1e-6)

        public double highVoltage, efficiency;
        public double doublesGateFraction, triplesGateFraction;

        public ShiftRegisterParameters(ShiftRegisterParameters sr)
        {
            if (sr == null)
                return;
            this.deadTimeCoefficientTinNanoSecs = sr.deadTimeCoefficientTinNanoSecs;
            this.deadTimeCoefficientAinMicroSecs = sr.deadTimeCoefficientAinMicroSecs;
            this.deadTimeCoefficientBinPicoSecs = sr.deadTimeCoefficientBinPicoSecs;
            this.deadTimeCoefficientCinNanoSecs = sr.deadTimeCoefficientCinNanoSecs;
            this.gateLength = sr.gateLength;
            this.predelay = sr.predelay;
            this.highVoltage = sr.highVoltage;
            this.dieAwayTime = sr.dieAwayTime;
            this.efficiency = sr.efficiency;
            this.doublesGateFraction = sr.doublesGateFraction;
            this.triplesGateFraction = sr.triplesGateFraction;
            //this.type = sr.type;
            //this.id = String.Copy(sr.id);
        }

        public void CopyValues(ShiftRegisterParameters sr)
        {
            if (sr == null)
                return;
            this.deadTimeCoefficientTinNanoSecs = sr.deadTimeCoefficientTinNanoSecs;
            this.deadTimeCoefficientAinMicroSecs = sr.deadTimeCoefficientAinMicroSecs;
            this.deadTimeCoefficientBinPicoSecs = sr.deadTimeCoefficientBinPicoSecs;
            this.deadTimeCoefficientCinNanoSecs = sr.deadTimeCoefficientCinNanoSecs;
            this.gateLength = sr.gateLength;
            this.predelay = sr.predelay;
            this.highVoltage = sr.highVoltage;
            this.dieAwayTime = sr.dieAwayTime;
            this.efficiency = sr.efficiency;
            this.doublesGateFraction = sr.doublesGateFraction;
            this.triplesGateFraction = sr.triplesGateFraction;
        }

        public ShiftRegisterParameters()
        {
            predelay = 45;		/* predelay */
            gateLength = 640;		/* gate length */
            highVoltage = 1680.0;		/* high voltage */
            dieAwayTime = 500.0;		/* die away time */
            efficiency = 0.0001;		/* efficiency */
            doublesGateFraction = 0.0001;		/* doubles gate fraction */
            triplesGateFraction = 0.0001;/* triples gate fraction */
        }

        public ShiftRegisterParameters(ulong pd, ulong gl, double hv, double da, double eff, double dgf, double tgf, double dtA, double dtB, double dtC, double dtMult)
        {
            predelay = pd;		/* predelay */
            gateLength = gl;		/* gate length */
            highVoltage = hv;		/* high voltage */
            dieAwayTime = da;		/* die away time */
            efficiency = eff;		/* efficiency */
            doublesGateFraction = dgf;		/* doubles gate fraction */
            triplesGateFraction = tgf;/* triples gate fraction */
            deadTimeCoefficientAinMicroSecs = dtA;
            deadTimeCoefficientBinPicoSecs = dtB;
            deadTimeCoefficientCinNanoSecs = dtC;
            deadTimeCoefficientMultiplicityinNanoSecs = dtMult;
        }

        // for old INCC code where the units are in ms (1e-6), but our units are tics (1e-7)
        public double predelayMS
        {
            get { return predelay / 10.0; }            
            set { predelay = (ulong) Math.Round(value * 10.0); }  // todo: check behavior of this from UI entry
        }
        public double gateLengthMS
        {
            get { return gateLength / 10.0; }
            set { gateLength = (ulong)Math.Round(value * 10.0); }  // todo: check behavior of this from UI entry
        }
        public double dieAwayTimeMS
        {
            get { return dieAwayTime / 10.0; }
            set { dieAwayTime = (ulong)Math.Round(value * 10.0); }  // todo: check behavior of this from UI entry
        }

        // clearly this can be table-driven, but no time righ tnow to design the tables
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "sr_parms_rec";
            this.ps.Add(new DBParamEntry("multiplicity_deadtime", deadTimeCoefficientTinNanoSecs));
            this.ps.Add(new DBParamEntry("coeff_a_deadtime", deadTimeCoefficientAinMicroSecs));
            this.ps.Add(new DBParamEntry("coeff_b_deadtime", deadTimeCoefficientBinPicoSecs));
            this.ps.Add(new DBParamEntry("coeff_c_deadtime", deadTimeCoefficientCinNanoSecs));
            this.ps.Add(new DBParamEntry("gate_length", gateLength));
            this.ps.Add(new DBParamEntry("predelay", predelay));
            this.ps.Add(new DBParamEntry("high_voltage", highVoltage));
            this.ps.Add(new DBParamEntry("die_away_time", dieAwayTime));
            this.ps.Add(new DBParamEntry("efficiency", efficiency));
            this.ps.Add(new DBParamEntry("doubles_gate_fraction", doublesGateFraction));
            this.ps.Add(new DBParamEntry("triples_gate_fraction", triplesGateFraction));
        }

        public bool Equals(ShiftRegisterParameters other)
        {
            if (other as Object != null &&
                this.dieAwayTime.Equals(other.dieAwayTime)
                & this.efficiency.Equals(other.efficiency)
                & this.doublesGateFraction.Equals(other.doublesGateFraction)
                & this.triplesGateFraction.Equals(other.triplesGateFraction)
                & this.deadTimeCoefficientTinNanoSecs.Equals(other.deadTimeCoefficientTinNanoSecs)
                & this.deadTimeCoefficientAinMicroSecs.Equals(other.deadTimeCoefficientAinMicroSecs)
                & this.deadTimeCoefficientBinPicoSecs.Equals(other.deadTimeCoefficientBinPicoSecs)
                & this.deadTimeCoefficientCinNanoSecs.Equals(other.deadTimeCoefficientCinNanoSecs)
                & this.gateLength.Equals(other.gateLength)
                  & this.predelay.Equals(other.predelay)
                & this.highVoltage.Equals(other.highVoltage))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ShiftRegisterParameters sr1, ShiftRegisterParameters sr2)
        {
            return sr1.Equals(sr2);
        }

        public static bool operator !=(ShiftRegisterParameters sr1, ShiftRegisterParameters sr2)
        {
            if (sr1 as Object != null)
                return (!sr1.Equals(sr2));
            else
                return (sr1 as Object) != (sr2 as Object);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is ShiftRegisterParameters))
                return false;// throw new ("The 'obj' argument is not a ShiftRegisterParameters object.");
            else
                return Equals(obj as ShiftRegisterParameters);
        }
        public override int GetHashCode()
        {
            int hCode = deadTimeCoefficientAinMicroSecs.GetHashCode()
                ^ deadTimeCoefficientBinPicoSecs.GetHashCode()
                ^ deadTimeCoefficientCinNanoSecs.GetHashCode()
                ^ dieAwayTime.GetHashCode()
                ^ highVoltage.GetHashCode()
                ^ efficiency.GetHashCode()
                ^ doublesGateFraction.GetHashCode()
                ^ triplesGateFraction.GetHashCode()
                ^ gateLength.GetHashCode()
                ^ predelay.GetHashCode()
                ^ deadTimeCoefficientTinNanoSecs.GetHashCode();

            return hCode.GetHashCode();
        }

        public static int Compare(ShiftRegisterParameters x, ShiftRegisterParameters y)
        {
            if (x as Object == null && y as Object == null)
                return 0;
            if (x as Object == null)
                return -1;
            if (y as Object == null)
                return 1;
            int res = x.dieAwayTime.CompareTo(y.dieAwayTime);
            if (res == 0)
                res = x.efficiency.CompareTo(y.efficiency);
            if (res == 0)
                res = x.doublesGateFraction.CompareTo(y.doublesGateFraction);
            if (res == 0)
                res = x.triplesGateFraction.CompareTo(y.triplesGateFraction);
            if (res == 0)
                res = x.deadTimeCoefficientTinNanoSecs.CompareTo(y.deadTimeCoefficientTinNanoSecs);
            if (res == 0)
                res = x.deadTimeCoefficientAinMicroSecs.CompareTo(y.deadTimeCoefficientAinMicroSecs);
            if (res == 0)
                res = x.deadTimeCoefficientBinPicoSecs.CompareTo(y.deadTimeCoefficientBinPicoSecs);
            if (res == 0)
                res = x.deadTimeCoefficientCinNanoSecs.CompareTo(y.deadTimeCoefficientCinNanoSecs);
            if (res == 0)
                res = x.gateLength.CompareTo(y.gateLength);
            if (res == 0)
                res = x.predelay.CompareTo(y.predelay);
            if (res == 0)
                res = x.highVoltage.CompareTo(y.highVoltage);

            return res;
        }

        public int CompareTo(ShiftRegisterParameters other)
        {
            return Compare(this, other);
        }
    }

    public enum AccidentalsMethod { None, Measure, Calculate };

    // checks and test limits
    public class TestParameters : ParameterBase, IComparable<TestParameters>// tethered to detector or else it is a global
    {
        public double accSnglTestRateLimit;
        public double accSnglTestPrecisionLimit;
        public double accSnglTestOutlierLimit;
        public double outlierTestLimit;
        public double bkgDoublesRateLimit;
        public double bkgTriplesRateLimit;
        public double chiSquaredLimit;
        public ushort maxNumFailures;
        public double highVoltageTestLimit;
        public double normalBackupAssayTestLimit;
        public uint maxCyclesForOutlierTest;
        public bool checksum;
        public AccidentalsMethod accidentalsMethod;

        public TestParameters()
        {
            accSnglTestRateLimit = 1000.0;
            accSnglTestPrecisionLimit = 0.1;
            accSnglTestOutlierLimit = 4.0;
            outlierTestLimit = 3.0;
            bkgDoublesRateLimit = 1.0;
            bkgTriplesRateLimit = 1.0;
            chiSquaredLimit = 99.0;
            maxNumFailures = 10;
            highVoltageTestLimit = 1.0;
            normalBackupAssayTestLimit = 3.0;
            maxCyclesForOutlierTest = 100;
            checksum = true; // this is likely really a bool
            accidentalsMethod = AccidentalsMethod.Measure;
        }
        public TestParameters(TestParameters src)
        {
            if (src == null)
            {
                accSnglTestRateLimit = 1000.0;
                accSnglTestPrecisionLimit = 0.1;
                accSnglTestOutlierLimit = 4.0;
                outlierTestLimit = 3.0;
                bkgDoublesRateLimit = 1.0;
                bkgTriplesRateLimit = 1.0;
                chiSquaredLimit = 99.0;
                maxNumFailures = 10;
                highVoltageTestLimit = 1.0;
                normalBackupAssayTestLimit = 3.0;
                maxCyclesForOutlierTest = 100;
                checksum = true; 
                accidentalsMethod = AccidentalsMethod.Measure;
            }
            else
            {
                accSnglTestRateLimit = src.accSnglTestRateLimit;
                accSnglTestPrecisionLimit = src.accSnglTestPrecisionLimit;
                accSnglTestOutlierLimit = src.accSnglTestOutlierLimit;
                outlierTestLimit = src.outlierTestLimit;
                bkgDoublesRateLimit = src.bkgDoublesRateLimit;
                bkgTriplesRateLimit = src.bkgTriplesRateLimit;
                chiSquaredLimit = src.chiSquaredLimit;
                maxNumFailures = src.maxNumFailures;
                highVoltageTestLimit = src.highVoltageTestLimit;
                normalBackupAssayTestLimit = src.normalBackupAssayTestLimit;
                maxCyclesForOutlierTest = src.maxCyclesForOutlierTest;
                checksum = src.checksum;
                accidentalsMethod = src.accidentalsMethod;
            }
        }
        public void Copy(TestParameters src)
        {
            if (src == null)
                return;
            accSnglTestRateLimit = src.accSnglTestRateLimit;
            accSnglTestPrecisionLimit = src.accSnglTestPrecisionLimit;
            accSnglTestOutlierLimit = src.accSnglTestOutlierLimit;
            outlierTestLimit = src.outlierTestLimit;
            bkgDoublesRateLimit = src.bkgDoublesRateLimit;
            bkgTriplesRateLimit = src.bkgTriplesRateLimit;
            chiSquaredLimit = src.chiSquaredLimit;
            maxNumFailures = src.maxNumFailures;
            highVoltageTestLimit = src.highVoltageTestLimit;
            normalBackupAssayTestLimit = src.normalBackupAssayTestLimit;
            maxCyclesForOutlierTest = src.maxCyclesForOutlierTest;
            checksum = src.checksum;
            accidentalsMethod = src.accidentalsMethod;
            modified = src.modified;
        }
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "test_parms_rec";
            this.ps.Add(new DBParamEntry("acc_sngl_test_rate_limit", accSnglTestRateLimit));
            this.ps.Add(new DBParamEntry("acc_sngl_test_precision_limit", accSnglTestPrecisionLimit));
            this.ps.Add(new DBParamEntry("acc_sngl_test_outlier_limit", accSnglTestOutlierLimit));
            this.ps.Add(new DBParamEntry("outlier_test_limit", outlierTestLimit));
            this.ps.Add(new DBParamEntry("bkg_doubles_rate_limit", bkgDoublesRateLimit));
            this.ps.Add(new DBParamEntry("bkg_triples_rate_limit", bkgTriplesRateLimit));
            this.ps.Add(new DBParamEntry("chisq_limit", chiSquaredLimit));
            this.ps.Add(new DBParamEntry("max_num_failures", maxNumFailures));
            this.ps.Add(new DBParamEntry("high_voltage_test_limit", highVoltageTestLimit));
            this.ps.Add(new DBParamEntry("normal_backup_assay_test_limit", normalBackupAssayTestLimit));
            this.ps.Add(new DBParamEntry("max_runs_for_outlier_test", maxCyclesForOutlierTest));
            this.ps.Add(new DBParamEntry("checksum_test", checksum));
            this.ps.Add(new DBParamEntry("accidentals_method", accidentalsMethod.ToString()));
        }


        public static int Compare(TestParameters x, TestParameters y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            int res = x.accSnglTestRateLimit.CompareTo(y.accSnglTestRateLimit);
            if (res == 0)
                res = x.accSnglTestPrecisionLimit.CompareTo(y.accSnglTestPrecisionLimit);
            if (res == 0)
                res = x.accSnglTestOutlierLimit.CompareTo(y.accSnglTestOutlierLimit);
			if (res == 0)
				res = x.outlierTestLimit.CompareTo(y.outlierTestLimit);
            if (res == 0)
                res = x.bkgDoublesRateLimit.CompareTo(y.bkgDoublesRateLimit);
            if (res == 0)
                res = x.bkgTriplesRateLimit.CompareTo(y.bkgTriplesRateLimit);
            if (res == 0)
                res = x.chiSquaredLimit.CompareTo(y.chiSquaredLimit);

            if (res == 0)
                res = x.maxNumFailures.CompareTo(y.maxNumFailures);
            if (res == 0)
                res = x.highVoltageTestLimit.CompareTo(y.highVoltageTestLimit);
            if (res == 0)
                res = x.normalBackupAssayTestLimit.CompareTo(y.normalBackupAssayTestLimit);
            if (res == 0)
                res = x.maxCyclesForOutlierTest.CompareTo(y.maxCyclesForOutlierTest);

            if (res == 0)
                res = x.checksum.CompareTo(y.checksum);
            if (res == 0)
                res = x.accidentalsMethod.CompareTo(y.accidentalsMethod);
            return res;
        }

        public int CompareTo(TestParameters other)
        {
            return Compare(this, other);
        }
    }

    public class NormParameters : ParameterBase // tethered to detector
    {
        public NormParameters()
        {
            currNormalizationConstant = new Tuple(1.0, 0);
            cf252RefDoublesRate = new Tuple();
            measRate = new Tuple();
            refDate = new DateTime(2010, 01, 01);
            sourceId = String.Empty;
            biasMode = NormTest.Cf252Doubles;
            initSrcPrecisionLimit = 0.3;
            biasPrecisionLimit = 0.3;
            acceptanceLimitStdDev = 3.0;
            acceptanceLimitPercent = 4.0;
            yieldRelativeToMrc95 = 1.0;
            biasTestUseAddasrc = false;
        }

        public NormParameters(NormParameters src)
        {
            if (src == null)
            {
                currNormalizationConstant = new Tuple(1.0, 0);
                cf252RefDoublesRate = new Tuple();
                measRate = new Tuple();
                refDate = new DateTime(2010, 01, 01);
                sourceId = String.Empty;
                biasMode = NormTest.Cf252Doubles;
                initSrcPrecisionLimit = 0.3;
                biasPrecisionLimit = 0.3;
                acceptanceLimitStdDev = 3.0;
                acceptanceLimitPercent = 4.0;
                yieldRelativeToMrc95 = 1.0;
                biasTestUseAddasrc = false;
            }
            else
            {
                currNormalizationConstant = new Tuple(src.currNormalizationConstant);
                cf252RefDoublesRate = new Tuple(src.cf252RefDoublesRate);
                measRate = new Tuple(src.measRate);
                refDate = new DateTime(src.refDate.Ticks);
                sourceId = String.Copy(src.sourceId);
                biasMode = src.biasMode;
                initSrcPrecisionLimit = src.initSrcPrecisionLimit;
                biasPrecisionLimit = src.biasPrecisionLimit;
                acceptanceLimitStdDev = src.acceptanceLimitStdDev;
                acceptanceLimitPercent = src.acceptanceLimitPercent;
                yieldRelativeToMrc95 = src.yieldRelativeToMrc95;
                biasTestUseAddasrc = src.biasTestUseAddasrc;
                modified = src.modified;
                amliRefSinglesRate = src.amliRefSinglesRate;
                biasTestAddasrcPosition = src.biasTestAddasrcPosition;
            }
        }

        public string sourceId;
        public Tuple currNormalizationConstant;
        public NormTest biasMode;
        public Tuple measRate;
        public double amliRefSinglesRate;
        public Tuple cf252RefDoublesRate;
        public DateTime refDate;
        public double initSrcPrecisionLimit;
        public double biasPrecisionLimit;
        public double acceptanceLimitStdDev;
        public double acceptanceLimitPercent;
        public double yieldRelativeToMrc95;
        public bool biasTestUseAddasrc;
        public double biasTestAddasrcPosition;

        public void Copy(NormParameters src)
        {
            if (src == null)
                return;
            currNormalizationConstant = new Tuple(src.currNormalizationConstant);
            cf252RefDoublesRate = new Tuple(src.cf252RefDoublesRate);
            measRate = new Tuple(src.measRate);
            refDate = new DateTime(src.refDate.Ticks);
            sourceId = String.Copy(src.sourceId);
            biasMode = src.biasMode;
            initSrcPrecisionLimit = src.initSrcPrecisionLimit;
            biasPrecisionLimit = src.biasPrecisionLimit;
            acceptanceLimitStdDev = src.acceptanceLimitStdDev;
            acceptanceLimitPercent = src.acceptanceLimitPercent;
            yieldRelativeToMrc95 = src.yieldRelativeToMrc95;
            biasTestUseAddasrc = src.biasTestUseAddasrc;
            modified = src.modified;
            amliRefSinglesRate = src.amliRefSinglesRate;
            biasTestAddasrcPosition = src.biasTestAddasrcPosition;
        }

        public void CopyTo(NormParameters dest)
        {
            if (dest == null)
                return;
            dest.currNormalizationConstant = new Tuple(currNormalizationConstant);
            dest.cf252RefDoublesRate = new Tuple(cf252RefDoublesRate);
            dest.measRate = new Tuple(measRate);
            dest.refDate = new DateTime(refDate.Ticks);
            dest.sourceId = String.Copy(sourceId);
            dest.biasMode = biasMode;
            dest.initSrcPrecisionLimit = initSrcPrecisionLimit;
            dest.biasPrecisionLimit = biasPrecisionLimit;
            dest.acceptanceLimitStdDev = acceptanceLimitStdDev;
            dest.acceptanceLimitPercent = acceptanceLimitPercent;
            dest.yieldRelativeToMrc95 = yieldRelativeToMrc95;
            dest.biasTestUseAddasrc = biasTestUseAddasrc;
            dest.modified = modified;
            dest.amliRefSinglesRate = amliRefSinglesRate;
            dest.biasTestAddasrcPosition = biasTestAddasrcPosition;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "norm_parms_rec";
            this.ps.Add(new DBParamEntry("source_id", sourceId));
            ps.AddRange(DBParamList.TuplePair("normalization_constant", currNormalizationConstant));
            this.ps.Add(new DBParamEntry("bias_mode", (Int32)biasMode));
            ps.AddRange(DBParamList.TuplePair("meas_rate", measRate));
            this.ps.Add(new DBParamEntry("amli_ref_singles_rate", amliRefSinglesRate));
            ps.AddRange(DBParamList.TuplePair("cf252_ref_doubles_rate", cf252RefDoublesRate));
            this.ps.Add(new DBParamEntry("ref_date", refDate));
            this.ps.Add(new DBParamEntry("init_src_precision_limit", initSrcPrecisionLimit));
            this.ps.Add(new DBParamEntry("bias_precision_limit", biasPrecisionLimit));
            this.ps.Add(new DBParamEntry("acceptance_limit_std_dev", acceptanceLimitStdDev));
            this.ps.Add(new DBParamEntry("acceptance_limit_percent", acceptanceLimitPercent));
            this.ps.Add(new DBParamEntry("yield_relative_to_mrc_95", yieldRelativeToMrc95));
            this.ps.Add(new DBParamEntry("bias_test_use_addasrc", biasTestUseAddasrc));
            this.ps.Add(new DBParamEntry("bias_test_addasrc_position", biasTestAddasrcPosition));

            //    [source_id] [nvarchar(256) ] NULL,
            //    [normalization_constant] [float] NULL,
            //    [normalization_constant_err] [float] NULL,
            //    [bias_mode] [int] NULL,
            //    [meas_rate] [float] NULL,
            //    [meas_rate_err] [float] NULL,
            //    [amli_ref_singles_rate] [float] NULL,
            //    [cf252_ref_doubles_rate] [float] NULL,
            //    [cf252_ref_doubles_rate_err] [float] NULL,
            //    [ref_date] [nvarchar(20) ] NULL,
            //    [init_src_precision_limit] [float] NULL,
            //    [bias_precision_limit] [float] NULL,
            //    [acceptance_limit_std_dev] [float] NULL,
            //    [acceptance_limit_percent] [float] NULL,
            //    [yield_relative_to_mrc_95] [float] NULL,
            //    [bias_test_use_addasrc] [nvarchar(256) ] NULL,
            //    [bias_test_addasrc_position] [float] NULL
        }

    }

    /// <summary>
    /// Uniquely identifies a measurement with timestamp, assay type, and an optional item id
    /// This is meant to be interpreted in the context of a detector list, so it is unique for a detector list, not all detectors.
    /// </summary>
    public class MeasId : ParameterBase, IComparable<MeasId>, IEquatable<MeasId>
    {
        public DateTimeOffset MeasDateTime
        {
            get;
            set;
        }
        public AssaySelector.MeasurementOption MeasOption
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// Uniquely identifies measurment with shared datetime and option properties 
        /// In INCC5, the time 1s increment coupled with the file name seemed to cover this case
        /// </summary>
        public int UniqueId
        {
            get;
            set;
        }
        public MeasId(string item)
        {
            MeasDateTime = DateTimeOffset.Now;
            Item = new ItemId(item);
            FileName = string.Empty;
        }
        public MeasId(string item, DateTimeOffset dt)
        {
            MeasDateTime = new DateTimeOffset(dt.Ticks, dt.Offset);
            Item = new ItemId(item);
            FileName = string.Empty;
        }
        public MeasId(AssaySelector.MeasurementOption op)
        {
            MeasOption = op;
            MeasDateTime = DateTimeOffset.Now;
            Item = new ItemId();
            FileName = string.Empty;
        }
        public MeasId(MeasId src)
        {
            MeasOption = src.MeasOption;
            MeasDateTime = new DateTimeOffset(src.MeasDateTime.Ticks, src.MeasDateTime.Offset);
            Item = new ItemId(src.Item);
            FileName = string.Copy(src.FileName);
        }
        public MeasId(AssaySelector.MeasurementOption op, DateTimeOffset dt, ItemId it = null)
        {
            MeasOption = op;
            MeasDateTime = new DateTimeOffset(dt.Ticks, dt.Offset);
            Item = (it == null ? new ItemId() : new ItemId(it));
            FileName = string.Empty;
        }

        public MeasId(AssaySelector.MeasurementOption op, DateTimeOffset dt, string fn)
        {
            MeasOption = op;
            MeasDateTime = new DateTimeOffset(dt.Ticks, dt.Offset);
            FileName = String.Copy(fn);
            Item = new ItemId();
        }
        // expanded info
        public ItemId Item { get; set; }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "measurements";
            this.ps.Add(new DBParamEntry("detector_id", "TBD"));
            this.ps.Add(new DBParamEntry("DateTime", MeasDateTime));
            this.ps.Add(new DBParamEntry("Type", MeasOption.ToString()));
            this.ps.Add(new DBParamEntry("Notes", ""));
            this.ps.Add(new DBParamEntry("FileName", FileName));
            // note: unique id is read-only, generated in database insertion
        }

        public int CompareTo(MeasId other)
        {
            if (other == null)
                return -1;
            int res = 0;
            res = MeasOption.CompareTo(other.MeasOption);
            if (res == 0)
                res = MeasDateTime.CompareTo(other.MeasDateTime);
            if (res == 0)
                res = FileName.CompareTo(other.FileName);
            if (res == 0 && Item != null && other.Item != null)
                res = Item.CompareTo(other.Item);
            return res;
        }

        public bool Equals(MeasId other)
        {
            if (other == null)
                return false;
            bool res = false;
            res = MeasOption.Equals(other.MeasOption);
            if (res)
                res = MeasDateTime.Equals(other.MeasDateTime);
            if (res)
                res = FileName.Equals(other.FileName);
            //if (res && Item != null && other.Item != null)
            //    res = (Item.CompareTo(other.Item) == 0);
            return res; 
        }
    }



    /// <summary>
    ///  A combined meas state with selected methods and params and the computed results
    /// </summary>
    public class INCCAnalysisState
    {
        public AnalysisMethods Methods;  // by material and detector id selector, one or more methods, with params

        public INCCResults Results;  // by mkey, then material and detector id selector, results for the methods on am

        public INCCAnalysisState()
        {
            Results = new INCCResults();
        }

        public void ClearINCCAnalysisResults()
        {
            Results.Clear();
            Results.MethodsResults.Clear();
            //Results.TradResultsRec. maybe not needed
        }

        /// <summary>
        /// a one-time creation method that fills the results map for the key
        /// </summary>
        /// <param name="mkey"></param>
        /// <param name="mcr"></param>
        public bool PrepareINCCResults(AssaySelector.MeasurementOption option, Multiplicity mkey, MultiplicityCountingRes mcr)
        {
            INCCResult newmcr;
            MeasOptionSelector ar = new MeasOptionSelector(option, mkey);

            bool good = Results.TryGetValue(ar, out newmcr);
            if (!good)
            {
                ConstructorInfo ci = GetOptionType(option).GetConstructor(Type.EmptyTypes);
                newmcr = (INCCResult)ci.Invoke(null);
                // copy the FA and idx properties since the param-less constructor creates this instance
                // newmcr.FA = mcr.FA;
                //   newmcr.idx = mcr.idx;
                Results.Add(ar, newmcr);
            }
			return good;
        }

        // this is silly, this is why we do OO design and I havent done it here
        private System.Type GetOptionType(AssaySelector.MeasurementOption option)
        {
            System.Type res = null;
            switch (option)
            {
                case AssaySelector.MeasurementOption.rates:
                    res = typeof(INCCResult); // nothing more to be computed 
                    break;
                case AssaySelector.MeasurementOption.background:
                    res = typeof(INCCResult); // NEXT: the tm bkg values are carried on  Measurement.Background, but probably need a real bkg results struct to match the pattern with the other here
                    break;
                case AssaySelector.MeasurementOption.initial:
                    res = typeof(INCCResults.results_init_src_rec);
                    break;
                case AssaySelector.MeasurementOption.normalization:
                    res = typeof(INCCResults.results_bias_rec);
                    break;
                case AssaySelector.MeasurementOption.precision:
                    res = typeof(INCCResults.results_precision_rec);
                    break;
                case AssaySelector.MeasurementOption.verification:
                    res = typeof(INCCResult);                    // place holder: the code creates a separate map (methodresults) using classes from INCCMethodResults
                    break;
                case AssaySelector.MeasurementOption.calibration:
                    res = typeof(INCCResult);                    // place holder: the code creates a separate map (methodresults) using classes from INCCMethodResults
                    break;
                case AssaySelector.MeasurementOption.holdup:
                    res = typeof(INCCResult);  // NEXT: Hold-up held up, implement it #102 
                    break;
                case AssaySelector.MeasurementOption.unspecified:
                    res = typeof(INCCResult);  // hope to never get here

                    break;
                default:
                    break;
            }

            return res;
        }

        public INCCResult Lookup(MeasOptionSelector mos, System.Type t = null)
        {
            INCCResult m = null;
            bool got = Results.TryGetValue(mos, out m);
            if (!got && t != null)
            {
                ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                m = (INCCResult)ci.Invoke(null);
                Results.Add(new MeasOptionSelector(mos), m);
            }
            return m;
        }


        /// <summary>
        /// a one-time creation method that fills the method results maps for the mkey and selector (detector/SR params/material)
        /// </summary>
        /// <param name="mkey"></param>
        /// <param name="sel"></param>
        public bool PrepareINCCMethodResults(SpecificCountingAnalyzerParams mkey, INCCSelector sel, Measurement m)
        {
            INCCMethodResults imr;
            AnalysisMethods am = m.INCCAnalysisState.Methods;
            if (am == null)//this shouldn't EVER happen hn 5.7.2015
                am = new AnalysisMethods();
            bool existed = Results.MethodsResults.TryGetValue(mkey, out imr);
            // first look for the incc results map
            if (!existed || imr == null) // add new method map to results for this key
            {
                imr = new INCCMethodResults();
                Results.MethodsResults.Add(mkey, imr);
                
                // second, go through the list of selected methods and add a new typed result instance, 
                foreach (AnalysisMethod method in System.Enum.GetValues(typeof(AnalysisMethod))) // could use the GetOptionType scheme here
                {
                    if (Methods.Has(method))
                    {
                        switch (method)
                        {
                            case AnalysisMethod.None:
                            case AnalysisMethod.INCCNone:
                                break;
                            // Now takes the method parameters from AnalysisState and copies them to results rec. hn 5.7.2015
                            case AnalysisMethod.KnownA:
                                INCCMethodResults.results_known_alpha_rec karec = new INCCMethodResults.results_known_alpha_rec();
                                am.GetMethodParameters(AnalysisMethod.KnownA).CopyTo(karec.methodParams);
                                imr.AddMethodResults(sel, method, karec);
                                break;
                            case AnalysisMethod.Multiplicity:
                                INCCMethodResults.results_multiplicity_rec multrec = new INCCMethodResults.results_multiplicity_rec();
                                am.GetMethodParameters(AnalysisMethod.Multiplicity).CopyTo(multrec.methodParams);
                                imr.AddMethodResults(sel, method, multrec);
                                break;
                            case AnalysisMethod.CalibrationCurve:
                                INCCMethodResults.results_cal_curve_rec calrec = new INCCMethodResults.results_cal_curve_rec ();
                                am.GetMethodParameters(AnalysisMethod.CalibrationCurve).CopyTo(calrec.methodParams);
                                imr.AddMethodResults(sel, method, calrec);
                                break;
                            case AnalysisMethod.TruncatedMultiplicity:
                                INCCMethodResults.results_truncated_mult_rec tmrec = new INCCMethodResults.results_truncated_mult_rec ();
                                am.GetMethodParameters(AnalysisMethod.TruncatedMultiplicity).CopyTo(tmrec.methodParams);
                                imr.AddMethodResults(sel, method, tmrec);
                                break;
                            case AnalysisMethod.KnownM:
                                INCCMethodResults.results_known_m_rec kmrec = new INCCMethodResults.results_known_m_rec();
                                am.GetMethodParameters(AnalysisMethod.KnownM).CopyTo(kmrec.methodParams);
                                imr.AddMethodResults(sel, method, new INCCMethodResults.results_known_m_rec());
                                break;
                            case AnalysisMethod.AddASource:
                                INCCMethodResults.results_add_a_source_rec aasrec = new INCCMethodResults.results_add_a_source_rec ();
                                am.GetMethodParameters(AnalysisMethod.AddASource).CopyTo(aasrec.methodParams);
                                imr.AddMethodResults(sel, method, aasrec);
                                break;
                            case AnalysisMethod.Active:
                                INCCMethodResults.results_active_rec arec = new INCCMethodResults.results_active_rec ();
                                am.GetMethodParameters(AnalysisMethod.Active).CopyTo(arec.methodParams);
                                imr.AddMethodResults(sel, method, arec);
                                break;
                            case AnalysisMethod.ActiveMultiplicity:
                                INCCMethodResults.results_active_mult_rec amrec = new INCCMethodResults.results_active_mult_rec ();
                                am.GetMethodParameters(AnalysisMethod.ActiveMultiplicity).CopyTo(amrec.methodParams);
                                imr.AddMethodResults(sel, method, amrec);
                                break;
                            case AnalysisMethod.ActivePassive:
                                INCCMethodResults.results_active_passive_rec aprec = new INCCMethodResults.results_active_passive_rec ();
                                am.GetMethodParameters(AnalysisMethod.ActivePassive).CopyTo(aprec.methodParams);
                                imr.AddMethodResults(sel, method, aprec);
                                break;
                            case AnalysisMethod.Collar:
                                INCCMethodResults.results_collar_rec collrec = new INCCMethodResults.results_collar_rec ();
                                am.GetMethodParameters(AnalysisMethod.Collar).CopyTo(collrec.methodParams.collar);
                                // todo: more needed here due to the combined nature of the collar params
                                imr.AddMethodResults(sel, method, collrec);
                                break;
                            case AnalysisMethod.CuriumRatio:
                                INCCMethodResults.results_curium_ratio_rec cmrec = new INCCMethodResults.results_curium_ratio_rec ();
                                am.GetMethodParameters(AnalysisMethod.CuriumRatio).CopyTo(cmrec.methodParams);
                                imr.AddMethodResults(sel, method, cmrec);
                                break;
                            default:
                                imr.AddMethodResults(sel, method, new INCCMethodResult());
                                break;
                        }
                    }
                }
            }
			return existed;
            // done!
        }
    }

    public enum ErrorCalculationTechnique { None, Sample, Theoretical }   // used in outlier calc and uncertainty calc, carried on the acq

    public static class ErrorCalculationTechniqueExtensions
    {
        public static ErrorCalculationTechnique Override(this ErrorCalculationTechnique ect, AssaySelector.MeasurementOption mopt, DetectorDefs.InstrType itype)
        {
            ErrorCalculationTechnique overridden = ErrorCalculationTechnique.None;
            if (mopt != AssaySelector.MeasurementOption.normalization &&
                mopt != AssaySelector.MeasurementOption.holdup &&
                mopt != AssaySelector.MeasurementOption.initial)
            {
                if (ect == ErrorCalculationTechnique.Sample || itype.isDG_AMSR_Match())  //itype == LMDAQ.InstrType.AMSR || itype == LMDAQ.InstrType.DGSR)
                    overridden = ErrorCalculationTechnique.Sample;
                else
                    overridden = ErrorCalculationTechnique.Theoretical;
            }
            return overridden;
        }
    }

    public enum WellConfiguration { Passive, Active };

    public enum AcquireConvergence { CycleCount, DoublesPrecision, TriplesPrecision, Pu240EffPrecision }

    public class AcquireParameters : ParameterBase
    {
        public INCCDB.Descriptor facility;
        public INCCDB.Descriptor mba;

        public string detector_id;
        public string item_type;
        public string glovebox_id;
        public string isotopics_id;
        public string comp_isotopics_id;
        public string campaign_id;
        public string item_id;
        public INCCDB.Descriptor stratum_id;
        public string user_id;
        public string comment;
        public bool ending_comment;
        public DetectorDefs.ConstructedSource data_src;
        public bool qc_tests;
        public bool print;
        public bool review_detector_parms;
        public bool review_calib_parms;
        public bool review_isotopics;
        public bool review_summed_raw_data;
        public bool review_run_raw_data;
        public bool review_run_rate_data;
        public bool review_summed_mult_dist;
        public bool review_run_mult_dist;
        public double run_count_time;
        public AcquireConvergence acquire_type;
        public UInt16 num_runs;
        public UInt16 active_num_runs;
        public UInt16 min_num_runs;
        public UInt16 max_num_runs;
        public double meas_precision;
        public WellConfiguration well_config;
        public double mass;
        public ErrorCalculationTechnique error_calc_method;
        public string inventory_change_code;
        public string io_code;
        public bool collar_mode;
        public double drum_empty_weight;
        protected DateTimeOffset _MeasDateTime;
        public string meas_detector_id;

        public LMAcquireParams lm;

        public AcquireParameters()
        {
            facility = new INCCDB.Descriptor("Empty", "The default facility name");
            mba = new INCCDB.Descriptor("XXXX", "The default MBA name");
            detector_id = "XXXX/XXX/YY";
            item_type = "Pu";
            glovebox_id = String.Empty;
            isotopics_id = Isotopics.DefaultId;
            comp_isotopics_id = Isotopics.DefaultId;
            campaign_id = String.Empty;
            item_id = String.Empty;
            stratum_id = new INCCDB.Descriptor();
            user_id = String.Empty;
            comment = String.Empty;
            qc_tests = true;
            review_detector_parms = true;
            review_calib_parms = true;
            review_isotopics = true;
            review_run_rate_data = true;
            run_count_time = 100;
            acquire_type = AcquireConvergence.CycleCount;
            num_runs = 3;
            active_num_runs = 3;
            min_num_runs = 10;
            max_num_runs = 1000;
            meas_precision = 1.0;
            well_config = WellConfiguration.Passive;
            error_calc_method = ErrorCalculationTechnique.Theoretical;
            inventory_change_code = String.Empty;
            io_code = String.Empty;
            _MeasDateTime = new DateTimeOffset(2010, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
            meas_detector_id = "XXXX/XXX/YY";
            lm = new LMAcquireParams(_MeasDateTime);
            lm.Cycles = num_runs;
            lm.Interval = run_count_time;
        }

        public DateTimeOffset MeasDateTime
        {
            get
            {
                return _MeasDateTime;
            }
             set
            {
                _MeasDateTime = value;
                if (lm != null)
                    lm.TimeStamp = value;
            }
        }

        public AcquireParameters(AcquireParameters src)
        {

            facility = new INCCDB.Descriptor(src.facility);
            mba = new INCCDB.Descriptor(src.mba);
            detector_id = String.Copy(src.detector_id);
            item_type = String.Copy(src.item_type);
            glovebox_id = String.Copy(src.glovebox_id);
            isotopics_id = String.Copy(src.isotopics_id);
            comp_isotopics_id = String.Copy(src.comp_isotopics_id);
            campaign_id = String.Copy(src.campaign_id);
            item_id = String.Copy(src.item_id);
            stratum_id = new INCCDB.Descriptor(src.stratum_id);
            user_id = String.Copy(src.user_id);
            comment = String.Copy(src.comment);
            ending_comment = src.ending_comment;
            data_src = src.data_src;
            qc_tests = src.qc_tests;
            print = src.print;
            review_detector_parms = src.review_detector_parms;
            review_calib_parms = src.review_calib_parms;
            review_isotopics = src.review_isotopics;
            review_summed_raw_data = src.review_summed_raw_data;
            review_run_raw_data = src.review_run_raw_data;
            review_run_rate_data = src.review_run_rate_data;
            review_summed_mult_dist = src.review_summed_mult_dist;
            review_run_mult_dist = src.review_run_mult_dist;
            run_count_time = src.run_count_time;
            acquire_type = src.acquire_type;
            num_runs = src.num_runs;
            active_num_runs = src.active_num_runs;
            min_num_runs = src.min_num_runs;
            max_num_runs = src.max_num_runs;
            meas_precision = src.meas_precision;
            well_config = src.well_config;
            mass = src.mass;
            error_calc_method = src.error_calc_method;
            inventory_change_code = String.Copy(src.inventory_change_code);
            io_code = String.Copy(src.io_code);
            collar_mode = src.collar_mode;
            drum_empty_weight = src.drum_empty_weight;
            _MeasDateTime = new DateTimeOffset(src._MeasDateTime.Ticks, src._MeasDateTime.Offset);
            meas_detector_id = String.Copy(src.meas_detector_id);
            lm = new LMAcquireParams(src.lm);
        }

        public void ApplyItemId(ItemId id)
        {
            if (id == null)
                return;
            item_id = String.Copy(id.item);
            item_type = String.Copy(id.material);
            isotopics_id = String.Copy(id.isotopics);
            stratum_id = new INCCDB.Descriptor(id.stratum, String.Copy(id.stratum));
            inventory_change_code = String.Copy(id.inventoryChangeCode);
            io_code = String.Copy(id.IOCode);
            mass = id.declaredMass;
        }

        public ItemId ItemId
        {
            get
            {
                ItemId id = new ItemId(item_id);
                id.material = String.Copy(item_type);
                id.isotopics = String.Copy(isotopics_id);
                id.stratum = String.Copy(stratum_id.Name);
                id.inventoryChangeCode = String.Copy(inventory_change_code);
                id.IOCode = String.Copy(io_code);
                id.declaredMass = mass;
                id.declaredUMass = mass; // devnote: these two mass values cannot exist at the same time, so the input is always one value
                //id.length = 
                return id;
            }
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "acquire_parms_rec";
            this.ps.Add(new DBParamEntry("facility", facility.Name));
            this.ps.Add(new DBParamEntry("facility_description", facility.Desc));
            this.ps.Add(new DBParamEntry("mba", mba.Name));
            this.ps.Add(new DBParamEntry("mba_description", mba.Desc));
            this.ps.Add(new DBParamEntry("detector_id", detector_id));
            this.ps.Add(new DBParamEntry("item_type", item_type));
            this.ps.Add(new DBParamEntry("glovebox_id", glovebox_id));
            this.ps.Add(new DBParamEntry("isotopics_id", isotopics_id));
            this.ps.Add(new DBParamEntry("comp_isotopics_id", comp_isotopics_id));
            this.ps.Add(new DBParamEntry("campaign_id", campaign_id));
            this.ps.Add(new DBParamEntry("item_id", item_id));
            this.ps.Add(new DBParamEntry("stratum_id", stratum_id.Name));
            this.ps.Add(new DBParamEntry("stratum_id_description", stratum_id.Desc));
            this.ps.Add(new DBParamEntry("user_id", user_id));
            this.ps.Add(new DBParamEntry("comment", comment));
            this.ps.Add(new DBParamEntry("ending_comment", ending_comment));

            this.ps.Add(new DBParamEntry("data_src", (int)data_src));
            this.ps.Add(new DBParamEntry("qc_tests", qc_tests));
            this.ps.Add(new DBParamEntry("acq_print", print));
            this.ps.Add(new DBParamEntry("review_detector_parms", review_detector_parms));
            this.ps.Add(new DBParamEntry("review_calib_parms", review_calib_parms));
            this.ps.Add(new DBParamEntry("review_isotopics", review_isotopics));
            this.ps.Add(new DBParamEntry("review_summed_raw_data", review_summed_raw_data));
            this.ps.Add(new DBParamEntry("review_run_raw_data", review_run_raw_data));
            this.ps.Add(new DBParamEntry("review_run_rate_data", review_run_rate_data));
            this.ps.Add(new DBParamEntry("review_summed_mult_dist", review_summed_mult_dist));
            this.ps.Add(new DBParamEntry("review_run_mult_dist", review_run_mult_dist));

            this.ps.Add(new DBParamEntry("run_count_time", run_count_time));
            this.ps.Add(new DBParamEntry("acquire_type", (int)acquire_type));
            this.ps.Add(new DBParamEntry("num_runs", num_runs));
            this.ps.Add(new DBParamEntry("active_num_runs", active_num_runs));
            this.ps.Add(new DBParamEntry("min_num_runs", min_num_runs));
            this.ps.Add(new DBParamEntry("max_num_runs", max_num_runs));
            this.ps.Add(new DBParamEntry("meas_precision", meas_precision));
            this.ps.Add(new DBParamEntry("well_config", (int)well_config));
            this.ps.Add(new DBParamEntry("mass", mass));
            this.ps.Add(new DBParamEntry("error_calc_method", (int)error_calc_method));
            this.ps.Add(new DBParamEntry("inventory_change_code", inventory_change_code));
            this.ps.Add(new DBParamEntry("io_code", io_code));
            this.ps.Add(new DBParamEntry("collar_mode", collar_mode));
            this.ps.Add(new DBParamEntry("drum_empty_weight", drum_empty_weight));
            this.ps.Add(new DBParamEntry("MeasDate", _MeasDateTime));
            this.ps.Add(new DBParamEntry("meas_detector_id", meas_detector_id));
        }

        public List<DBParamEntry> SubsetForResults()
        {
            this.GenParamList();
            List<DBParamEntry> acr = new List<DBParamEntry>();
            acr.Add(Find("facility"));
            acr.Add(Find("facility_description"));
            acr.Add(Find("mba"));
            acr.Add(Find("mba_description"));
            acr.Add(Find("item_type"));
            acr.Add(Find("campaign_id"));
            acr.Add(Find("item_id"));
            acr.Add(Find("stratum_id"));
            acr.Add(Find("stratum_id_description"));
            acr.Add(Find("collar_mode"));
            acr.Add(Find("inventory_change_code"));
            acr.Add(Find("io_code"));
            acr.Add(Find("well_config"));
            acr.Add(Find("data_src"));
            acr.Add(Find("qc_tests"));
            acr.Add(Find("error_calc_method"));
            acr.Add(Find("acq_print"));
            acr.Add(Find("user_id"));
            acr.Add(Find("comment"));
            acr.Add(Find("ending_comment"));
            acr.Add(Find("num_runs"));
            return acr;
        }
    }

    public enum AddASourceFlavors { None, CompuMotor_3000, CompuMotor_4000, PLC_JCC21, PLC_WM3100, Canberra_Counter, PSC_WDAS = Canberra_Counter, Manual };

    public class AddASourceSetup : ParameterBase, IComparable<AddASourceSetup>, IEquatable<AddASourceSetup>
    {
        public AddASourceSetup(AddASourceSetup src)
        {
            type = src.type; port_number = src.port_number; forward_over_travel = src.forward_over_travel; reverse_over_travel = src.reverse_over_travel; number_positions = src.number_positions;
            dist_to_move = new double[5];
            Array.Copy(src.dist_to_move, dist_to_move, src.dist_to_move.Length);
            cm_steps_per_inch = src.cm_steps_per_inch;
            cm_forward_mask = src.cm_forward_mask;
            cm_reverse_mask = src.cm_reverse_mask;
            cm_axis_number = src.cm_axis_number;
            cm_over_travel_state = src.cm_over_travel_state;
            cm_step_ratio = src.cm_step_ratio;
            cm_slow_inches = src.cm_slow_inches;
            plc_steps_per_inch = src.plc_steps_per_inch;
            scale_conversion_factor = src.scale_conversion_factor;
            cm_rotation = src.cm_rotation;
        }
        public AddASourceSetup()
        {
            type = AddASourceFlavors.None;	/* add-a-source type */
            port_number = 1;		/* add-a-source port number */
            forward_over_travel = 0.0;		/* distance to forward over travel (inches) */
            reverse_over_travel = 0.0;		/* distance to reverse over travel (inches) */
            number_positions = 1;		/* number of measurement positions */
            dist_to_move = new double[] { 0, 0, 0, 0, 0 };
            //  	0.,		/* number of inches to move Cf252 to 1st position */
            //      0.,		/* number of inches to move Cf252 to 2nd position */
            //      0.,		/* number of inches to move Cf252 to 3rd position */
            //      0.,		/* number of inches to move Cf252 to 4th position */
            //      0.,		/* number of inches to move Cf252 to 5th position */
            cm_steps_per_inch = 625.0;		/* CM steps per inch */
            cm_forward_mask = 2;		/* CM forward over travel mask */
            cm_reverse_mask = 1;		/* CM reverse over travel mask */
            cm_axis_number = 1;		/* CM axis number */
            cm_over_travel_state = 0;		/* CM bit state when in an over travel switch */
            cm_step_ratio = 1.0;		/* CM steps commanded / encoder steps returned */
            cm_slow_inches = 2.0;		/* # of inches to move Cf252 while searching for home */
            plc_steps_per_inch = 51.0;		/* PLC steps per inch */
            scale_conversion_factor = 1.0;		/* scale conversion factor */
            cm_rotation = false;		/* rotate */
        }
        public AddASourceFlavors type { get; set; }
        public Int16 port_number { get; set; }
        public double forward_over_travel { get; set; }
        public double reverse_over_travel { get; set; }
        public UInt16 number_positions { get; set; }

        public double[] dist_to_move;
        public double cm_steps_per_inch { get; set; }
        public UInt32 cm_forward_mask { get; set; }
        public UInt32 cm_reverse_mask { get; set; }
        public short cm_axis_number { get; set; }
        public UInt32 cm_over_travel_state { get; set; }
        public double cm_step_ratio { get; set; }
        public double cm_slow_inches { get; set; }
        public double plc_steps_per_inch { get; set; }
        public double scale_conversion_factor { get; set; }
        public bool cm_rotation { get; set; }



        public static int Compare(AddASourceSetup x, AddASourceSetup y)
        {
            int res = x.type.CompareTo(y.type);
            if (res == 0)
                res = x.port_number.CompareTo(y.port_number);
            if (res == 0)
                res = x.reverse_over_travel.CompareTo(y.reverse_over_travel);
            if (res == 0)
                res = x.forward_over_travel.CompareTo(y.forward_over_travel);
            if (res == 0)
                res = x.number_positions.CompareTo(y.number_positions);
            if (res == 0)
                res = x.cm_steps_per_inch.CompareTo(y.cm_steps_per_inch);
            if (res == 0)
                res = x.cm_forward_mask.CompareTo(y.cm_forward_mask);
            if (res == 0)
                res = x.cm_reverse_mask.CompareTo(y.cm_reverse_mask);
            if (res == 0)
                res = x.cm_axis_number.CompareTo(y.cm_axis_number);
            if (res == 0)
                res = x.cm_over_travel_state.CompareTo(y.cm_over_travel_state);
            if (res == 0)
                res = x.cm_step_ratio.CompareTo(y.cm_step_ratio);
            if (res == 0)
                res = x.cm_slow_inches.CompareTo(y.cm_slow_inches);
            if (res == 0)
                res = x.plc_steps_per_inch.CompareTo(y.plc_steps_per_inch);
            if (res == 0)
                res = x.scale_conversion_factor.CompareTo(y.scale_conversion_factor);
            if (res == 0)
                res = x.cm_rotation.CompareTo(y.cm_rotation);
            return res;
        }

        public int CompareTo(AddASourceSetup other)
        {
            return Compare(this, other);
        }

        public bool Equals(AddASourceSetup other)
        {
            return CompareTo(other) == 0;
        }

        public void Copy(AddASourceSetup src)
        {
            if (src == null)
                return;
            type = src.type;
            port_number = src.port_number;
            reverse_over_travel = src.reverse_over_travel;
            forward_over_travel = src.forward_over_travel;
            number_positions = src.number_positions;
            cm_steps_per_inch = src.cm_steps_per_inch;
            cm_forward_mask = src.cm_forward_mask;
            cm_reverse_mask = src.cm_reverse_mask;
            cm_axis_number = src.cm_axis_number;
            cm_over_travel_state = src.cm_over_travel_state;
            cm_step_ratio = src.cm_step_ratio;
            cm_slow_inches = src.cm_slow_inches;
            plc_steps_per_inch = src.plc_steps_per_inch;
            scale_conversion_factor = src.scale_conversion_factor;
            cm_rotation = src.cm_rotation;
            dist_to_move = new double[] { 0, 0, 0, 0, 0 };
            Array.Copy(dist_to_move, src.dist_to_move, dist_to_move.Length);
            modified = src.modified;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "add_a_source_setup_rec";
            this.ps.Add(new DBParamEntry("port_number", port_number));
            this.ps.Add(new DBParamEntry("forward_over_travel", forward_over_travel));
            this.ps.Add(new DBParamEntry("type", type.ToString()));
            this.ps.Add(new DBParamEntry("reverse_over_travel", reverse_over_travel));
            this.ps.Add(new DBParamEntry("number_positions", number_positions));
            this.ps.Add(new DBParamEntry("dist_to_move", dist_to_move));
            this.ps.Add(new DBParamEntry("cm_steps_per_inch", cm_steps_per_inch));
            this.ps.Add(new DBParamEntry("cm_forward_mask", cm_forward_mask));
            this.ps.Add(new DBParamEntry("cm_reverse_mask", cm_reverse_mask));
            this.ps.Add(new DBParamEntry("cm_axis_number", cm_axis_number));
            this.ps.Add(new DBParamEntry("cm_over_travel_state", cm_over_travel_state));
            this.ps.Add(new DBParamEntry("cm_step_ratio", cm_step_ratio));
            this.ps.Add(new DBParamEntry("cm_slow_inches", cm_slow_inches));
            this.ps.Add(new DBParamEntry("plc_steps_per_inch", plc_steps_per_inch));
            this.ps.Add(new DBParamEntry("scale_conversion_factor", scale_conversion_factor));
            this.ps.Add(new DBParamEntry("cm_rotation", cm_rotation));
        }
    }

    public class BackgroundParameters : ParameterBase, IComparable<BackgroundParameters>
    {

        public BackgroundParameters()
        {
            rates = new Rates();
            TMBkgParams = new TruncatedMultiplicityBackgroundParameters();
            Scaler1 = new Tuple();
            Scaler2 = new Tuple();
            INCCActive = new StdRates();
        }


        public BackgroundParameters(BackgroundParameters src)
        {
            if (src == null)
            {
                rates = new Rates();
                TMBkgParams = new TruncatedMultiplicityBackgroundParameters();
                Scaler1 = new Tuple();
                Scaler2 = new Tuple();
                INCCActive = new StdRates();
            }
            else
            {
                rates = new Rates(src.rates);
                TMBkgParams = new TruncatedMultiplicityBackgroundParameters(); // todo: src.TMBkgParams);
                Scaler1 = new Tuple(src.Scaler1);
                Scaler2 = new Tuple(src.Scaler2);
                INCCActive = new StdRates(src.INCCActive);
                modified = src.modified;
            }
        }


        public bool TMMode { set; get; }  // devnote: kludgey toggle for the old DB schema

        public TruncatedMultiplicityBackgroundParameters TMBkgParams;
        public Rates rates;
        public Tuple Scaler1, Scaler2;
        public StdRates INCCActive; // old active rates from the INCCbkg table

        // for later merging with multiplicity Results class, since each detector with an SR parm set should have it;s own bkg parms 
        public Tuple RawSinglesRate
        {
            get { return rates[RatesAdjustments.Raw].Singles; }
        }
        public Tuple RawDoublesRate
        {
            get { return rates[RatesAdjustments.Raw].Doubles; }
        }
        public Tuple RawTriplesRate
        {
            get { return rates[RatesAdjustments.Raw].Triples; }
        }
        public Tuple DeadtimeCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Singles; }
        }
        public Tuple DeadtimeCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Doubles; }
        }
        public Tuple DeadtimeCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected].Triples; }
        }
        public Tuple DytlewskiCorrectedSinglesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Singles; }
        }
        public Tuple DytlewskiCorrectedDoublesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Doubles; }
        }
        public Tuple DytlewskiCorrectedTriplesRate
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected].Triples; }
        }
        public StdRates DeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DeadtimeCorrected]; }
        }
        public StdRates DytlewskiDeadtimeCorrectedRates
        {
            get { return rates[RatesAdjustments.DytlewskiDeadtimeCorrected]; }
        }

        public void Copy(BackgroundParameters src)
        {
            if (src == null)
                return;
            rates = new Rates(src.rates);
            INCCActive = new StdRates(src.INCCActive);
            TMBkgParams = new TruncatedMultiplicityBackgroundParameters(src.TMBkgParams);
            Scaler1 = new Tuple(src.Scaler1);
            Scaler2 = new Tuple(src.Scaler2);
            modified = src.modified;
        }
        public void CopyTo(BackgroundParameters dest)
        {
            if (dest == null)
                return;
            dest.rates = new Rates(rates);
            dest.INCCActive = new StdRates(INCCActive);
            dest.TMBkgParams = new TruncatedMultiplicityBackgroundParameters(this.TMBkgParams);
            dest.Scaler1 = new Tuple(Scaler1);
            dest.Scaler2 = new Tuple(Scaler2);
            dest.modified = modified;
        }

        public static int Compare(BackgroundParameters x, BackgroundParameters y)
        {
            int res = x.rates.CompareTo(y.rates);
            if (res == 0)
                res = x.Scaler1.CompareTo(y.Scaler1);
            if (res == 0)
                res = x.Scaler2.CompareTo(y.Scaler2);
            if (res == 0)
                res = x.TMBkgParams.CompareTo(y.TMBkgParams);
            return res;
        }

        public int CompareTo(BackgroundParameters other)
        {
            return Compare(this, other);
        }

        // clearly this can be table-driven, but there was no time to design the ORM tool and tables
        public override void GenParamList()
        {
            base.GenParamList();

            if (TMMode)
            {
                Table = "tm_bkg_parms_rec";
                ps.AddRange(DBParamList.TuplePair("tm_zeros_bkg", TMBkgParams.Zeros));
                ps.AddRange(DBParamList.TuplePair("tm_ones_bkg", TMBkgParams.Ones));
                ps.AddRange(DBParamList.TuplePair("tm_twos_bkg", TMBkgParams.Twos));
                ps.AddRange(DBParamList.TuplePair("tm_singles_bkg", TMBkgParams.Singles));
                ps.Add(new DBParamEntry("tm_bkg",  TMBkgParams.ComputeTMBkg));
            }
            else
            {
                Table = "bkg_parms_rec";
                ps.AddRange(DBParamList.TuplePair("passive_bkg_singles_rate", DeadtimeCorrectedSinglesRate));
                ps.AddRange(DBParamList.TuplePair("passive_bkg_doubles_rate", DeadtimeCorrectedDoublesRate));
                ps.AddRange(DBParamList.TuplePair("passive_bkg_triples_rate", DeadtimeCorrectedTriplesRate));
                ps.Add(new DBParamEntry("passive_bkg_scaler1_rate", Scaler1.v));
                ps.Add(new DBParamEntry("passive_bkg_scaler2_rate", Scaler2.v));
                ps.AddRange(DBParamList.TuplePair("active_bkg_singles_rate", INCCActive.Singles));
                ps.AddRange(DBParamList.TuplePair("active_bkg_doubles_rate", INCCActive.Doubles));
                ps.AddRange(DBParamList.TuplePair("active_bkg_triples_rate", INCCActive.Triples));
                ps.Add(new DBParamEntry("active_bkg_scaler1_rate", INCCActive.Scaler1Rate));
                ps.Add(new DBParamEntry("active_bkg_scaler2_rate", INCCActive.Scaler2Rate));
            }  
        }
    }

    public class TruncatedMultiplicityBins : ParameterBase, IComparable<TruncatedMultiplicityBins>
    {
        public TruncatedMultiplicityBins()
        {
            Singles = new Tuple();
            Zeros = new Tuple();
            Ones = new Tuple();
            Twos = new Tuple();
        }
        public TruncatedMultiplicityBins(TruncatedMultiplicityBins src)
        {
            Singles = new Tuple(src.Singles);
            Zeros = new Tuple(src.Zeros);
            Ones = new Tuple(src.Ones);
            Twos = new Tuple(src.Twos);
        }
        public void CopyTo(TruncatedMultiplicityBins dest)
        {
            dest.Singles = new Tuple(Singles);
            dest.Zeros = new Tuple(Zeros);
            dest.Ones = new Tuple(Ones);
            dest.Twos = new Tuple(Twos);
        }
        public Tuple Singles, Zeros, Ones, Twos;
        public void SZeros()
        {
            Singles.Zero();
            Zeros.Zero();
            Ones.Zero();
            Twos.Zero();
        }

        public string prefix;
        public override void GenParamList()
        {
            base.GenParamList();
            ps.AddRange(DBParamList.TuplePair(prefix + "_singles", Singles));
            ps.AddRange(DBParamList.TuplePair(prefix + "_zeros", Zeros));
            ps.AddRange(DBParamList.TuplePair(prefix + "_ones", Ones));
            ps.AddRange(DBParamList.TuplePair(prefix + "_twos", Twos));
        }

        public static int Compare(TruncatedMultiplicityBins x, TruncatedMultiplicityBins y) 
        {
            int res = x.Singles.CompareTo(y.Singles);
            if (res == 0)
                res = x.Zeros.CompareTo(y.Zeros);
            if (res == 0)
                res = x.Ones.CompareTo(y.Ones);
            if (res == 0)
                res = x.Twos.CompareTo(y.Twos);
            return res;
        }

        public int CompareTo(TruncatedMultiplicityBins other)
        {
            return Compare(this, other);
        }
    }

    public class TruncatedMultiplicityBackgroundParameters : TruncatedMultiplicityBins
    {
        public TruncatedMultiplicityBackgroundParameters()
        {
            ComputeTMBkg = false;
        }

        public TruncatedMultiplicityBackgroundParameters(TruncatedMultiplicityBackgroundParameters src) : base(src)
        {
            ComputeTMBkg = src.ComputeTMBkg;
        }
        public bool ComputeTMBkg;  // use me! flag
    }

    public class UnattendedParameters : ParameterBase, IComparable<UnattendedParameters>
    {

        public UInt32 ErrorSeconds { get; set; }
        public bool AutoImport { get; set; }
        public double AASThreshold { get; set; }

        public UnattendedParameters()
        {
            ErrorSeconds = 600;		/* max time for finding a data match (seconds) */
            AutoImport = true;			/* automatic import of measurements (no dialog box) */
            AASThreshold = 0.0;		/* add-a-source threshold */
        }

        public UnattendedParameters(UnattendedParameters src)
        {
            if (src == null)
            { 
                ErrorSeconds = 600;		
                AutoImport = true;
                AASThreshold = 0.0;
            } else {
                ErrorSeconds = src.ErrorSeconds;
                AutoImport = src.AutoImport;
                AASThreshold = src.AASThreshold;
            }
        }

        public void Copy(UnattendedParameters src)
        {
            if (src == null)
                return;
            ErrorSeconds = src.ErrorSeconds;
            AutoImport = src.AutoImport;
            AASThreshold = src.AASThreshold;
        }
        public static int Compare(UnattendedParameters x, UnattendedParameters y)
        {
            int res = x.ErrorSeconds.CompareTo(y.ErrorSeconds);
            if (res == 0)
                res = x.AutoImport.CompareTo(y.AutoImport);
            if (res == 0)
                res = x.AASThreshold.CompareTo(y.AASThreshold);
            return res;
        }

        public int CompareTo(UnattendedParameters other)
        {
            return Compare(this, other);
        }
        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "Unatt";

            this.ps.Add(new DBParamEntry("add_a_source_threshold", AASThreshold));
            this.ps.Add(new DBParamEntry("auto_import", AutoImport));
            this.ps.Add(new DBParamEntry("error_seconds", ErrorSeconds));

        }
    }

    public class ItemId : ParameterBase, IComparable<ItemId>
    {
        public string item, material, isotopics, stratum, inventoryChangeCode, IOCode;
        public double declaredMass, declaredUMass, length;

        // moved from results struct to item itself (joe, why did you do this?) because the results class has a direct copy of the same data, so I use a copy of the ItemId instead of expanding the ItemId fields onto the results
        public DateTime pu_date, am_date;
        public string mba; //moved from item_id to mba table

        public ItemId()
        {
            length = 1;
            item = String.Empty;
            material = String.Empty; isotopics = String.Empty; stratum = String.Empty; mba = string.Empty;
            inventoryChangeCode = String.Empty; IOCode = String.Empty;
            pu_date = new DateTime(Isotopics.ZeroIAEATime.Ticks);
            am_date = new DateTime(Isotopics.ZeroIAEATime.Ticks);
        }
        
        // Added this constructor to automagically put isotopic dates from measurement in Item structure.  hn 11.5.2014
        public ItemId(string itemName, Isotopics measIso)
        {
            length = 1;
            item = itemName;
            material = String.Empty; isotopics = String.Empty; stratum = String.Empty; mba = string.Empty;
            inventoryChangeCode = String.Empty; IOCode = String.Empty;
            //We should always put the measurements pu and am dates in the item structure.  hn 11.5.2014
            pu_date = measIso.pu_date;
            am_date = measIso.am_date;
        }
        public ItemId(ItemId src)
        {
            item = String.Copy(src.item); material = String.Copy(src.material); isotopics = String.Copy(src.isotopics); stratum = String.Copy(src.stratum); inventoryChangeCode = String.Copy(src.inventoryChangeCode); IOCode = String.Copy(src.IOCode); mba = String.Copy(src.mba);
            declaredMass = src.declaredMass; declaredUMass = src.declaredUMass; length = src.length;
            pu_date = new DateTime(src.pu_date.Ticks); am_date = new DateTime(src.am_date.Ticks);
        }
        public ItemId(string item)
        {
            length = 1;
            this.item = item;
            material = String.Empty; isotopics = String.Empty; stratum = String.Empty; mba = string.Empty;
            inventoryChangeCode = String.Empty; IOCode = String.Empty;
            pu_date = new DateTime(Isotopics.ZeroIAEATime.Ticks);
            am_date = new DateTime(Isotopics.ZeroIAEATime.Ticks);
        }

        public void Copy(ItemId src)
        {
            item = String.Copy(src.item); material = String.Copy(src.material); isotopics = String.Copy(src.isotopics); stratum = String.Copy(src.stratum); inventoryChangeCode = String.Copy(src.inventoryChangeCode); IOCode = String.Copy(src.IOCode); mba = String.Copy(src.mba);
            declaredMass = src.declaredMass; declaredUMass = src.declaredUMass; length = src.length;
            pu_date = new DateTime(src.pu_date.Ticks); am_date = new DateTime(src.am_date.Ticks);
        }

        public static int Compare(ItemId x, ItemId y)
        {
            int res = x.item.CompareTo(y.item);
            if (res == 0)
                res = x.length.CompareTo(y.length);
            if (res == 0)
                res = x.material.CompareTo(y.material);
            if (res == 0)
                res = x.declaredUMass.CompareTo(y.declaredUMass);
            if (res == 0)
                res = x.declaredMass.CompareTo(y.declaredMass);
            if (res == 0)
                res = x.isotopics.CompareTo(y.isotopics);

            if (res == 0)
                res = x.stratum.CompareTo(y.stratum);
            if (res == 0)
                res = x.pu_date.CompareTo(y.pu_date);
            if (res == 0)
                res = x.am_date.CompareTo(y.am_date);
            if (res == 0)
                res = x.inventoryChangeCode.CompareTo(y.inventoryChangeCode);

            if (res == 0)
                res = x.IOCode.CompareTo(y.IOCode);
            if (res == 0)
                res = x.mba.CompareTo(y.mba);
            return res;
        }

        public int CompareTo(ItemId other)
        {
            return Compare(this, other);
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "ItemId";
            if (String.IsNullOrEmpty(item)) item = "";
            this.ps.Add(new DBParamEntry("item_name", item));
            if (String.IsNullOrEmpty(mba)) mba = "";
            this.ps.Add(new DBParamEntry("mba", mba));
            if (String.IsNullOrEmpty(material)) material = "Pu";
            this.ps.Add(new DBParamEntry("material_type_id", material));
            if (String.IsNullOrEmpty(isotopics)) isotopics = "Default";
            this.ps.Add(new DBParamEntry("isotopics_id", isotopics));
            if (String.IsNullOrEmpty(stratum)) stratum = "";
            this.ps.Add(new DBParamEntry("stratum_id", stratum));
            if (String.IsNullOrEmpty(inventoryChangeCode)) inventoryChangeCode = "";
            this.ps.Add(new DBParamEntry("inventory_change_code", inventoryChangeCode));
            if (String.IsNullOrEmpty(IOCode)) IOCode = "";
            this.ps.Add(new DBParamEntry("io_code", IOCode));
            this.ps.Add(new DBParamEntry("declared_mass_entry", declaredMass));
            this.ps.Add(new DBParamEntry("declared_u_mass_entry", declaredUMass));
            this.ps.Add(new DBParamEntry("length_entry", length));
        }
    }

    // From the collar_data_entry_rec table in the old DB
    public class CollarItemId : ParameterBase, IComparable<CollarItemId>
    {
        public string item_id, rod_type;
        public double total_rods, total_poison_rods;
        public Tuple length, total_pu, depleted_u, natural_u, enriched_u, total_u235, total_u238, poison_percent;

        public CollarItemId()
        {
            length = new Tuple(1,0);
            item_id = String.Empty;
            rod_type = String.Empty;
            total_pu = new Tuple(); 
            depleted_u = new Tuple(); natural_u = new Tuple(); enriched_u = new Tuple(); total_u235 = new Tuple(); total_u238 = new Tuple(); poison_percent = new Tuple();
        }

        public CollarItemId(CollarItemId src)
        {
            item_id = String.Copy(src.item_id); rod_type = String.Copy(src.rod_type);
            total_rods = src.total_rods; total_poison_rods = src.total_poison_rods;
            length = new Tuple(src.length); total_pu = new Tuple(src.total_pu);
            depleted_u.CopyFrom(src.depleted_u);
            natural_u = new Tuple(src.natural_u); enriched_u = new Tuple(src.enriched_u); total_u235 = new Tuple(src.total_u235); total_u238 = new Tuple(src.total_u238); poison_percent = new Tuple(src.poison_percent); 
        }

        public CollarItemId(string item)
        {
            length = new Tuple(1, 0);
            this.item_id = String.Copy(item);
            rod_type = String.Empty;
            total_pu = new Tuple(); depleted_u = new Tuple(); natural_u = new Tuple(); enriched_u = new Tuple(); total_u235 = new Tuple(); total_u238 = new Tuple(); poison_percent = new Tuple();
        }

        public static int Compare(CollarItemId x, CollarItemId y)
        {
            int res = x.item_id.CompareTo(y.item_id);
            if (res == 0)
                res = x.length.CompareTo(y.length);
            if (res == 0)
                res = x.rod_type.CompareTo(y.rod_type);
            if (res == 0)
                res = x.total_rods.CompareTo(y.total_rods);
            if (res == 0)
                res = x.total_poison_rods.CompareTo(y.total_poison_rods);
            if (res == 0)
                res = x.total_pu.CompareTo(y.total_pu);

            if (res == 0)
                res = x.depleted_u.CompareTo(y.depleted_u);
            if (res == 0)
                res = x.natural_u.CompareTo(y.natural_u);
            if (res == 0)
                res = x.enriched_u.CompareTo(y.enriched_u);
            if (res == 0)
                res = x.total_u235.CompareTo(y.total_u235);

            if (res == 0)
                res = x.total_u238.CompareTo(y.total_u238);
            if (res == 0)
                res = x.poison_percent.CompareTo(y.poison_percent);
            return res;
        }

        public int CompareTo(CollarItemId other)
        {
            return Compare(this, other);
        }

        public override void GenParamList()
        {
            base.GenParamList();
            Table = "collar_data_entry";
            if (String.IsNullOrEmpty(item_id)) item_id = "";
                ps.Add(new DBParamEntry("item_name", item_id));
            if (String.IsNullOrEmpty(rod_type)) rod_type = "";
                ps.Add(new DBParamEntry("rod_type", rod_type));
            ps.AddRange(DBParamList.TuplePair("length_entry", length));
            ps.Add(new DBParamEntry("total_rods", total_rods));
            ps.Add(new DBParamEntry("total_poison_rods", total_poison_rods));
            ps.AddRange(DBParamList.TuplePair("total_pu", total_pu));
            ps.AddRange(DBParamList.TuplePair("depleted_u", depleted_u));
            ps.AddRange(DBParamList.TuplePair("natural_u", natural_u));
            ps.AddRange(DBParamList.TuplePair("enriched_u", enriched_u));
            ps.AddRange(DBParamList.TuplePair("total_u235", total_u235));
            ps.AddRange(DBParamList.TuplePair("total_u238", total_u238));
            ps.AddRange(DBParamList.TuplePair("poison_percent", poison_percent));
        }
    }

    public class holdup_config_rec : ParameterBase, IComparable<holdup_config_rec>, IEquatable<holdup_config_rec>
    {
        public holdup_config_rec(holdup_config_rec src)
        {
            num_rows = src.num_rows; num_columns = src.num_columns; distance = src.distance; glovebox_id = String.Copy(src.glovebox_id);
        }
        public holdup_config_rec()
        {
        }
        public UInt16 num_rows { get; set; }
        public UInt16 num_columns { get; set; }
        public double distance { get; set; }
        public string glovebox_id { get; set; }

        public static int Compare(holdup_config_rec x, holdup_config_rec y)
        {
            int res = x.glovebox_id.CompareTo(y.glovebox_id);
            if (res == 0)
                res = x.distance.CompareTo(y.distance);
            if (res == 0)
                res = x.num_columns.CompareTo(y.num_columns);
            if (res == 0)
                res = x.num_rows.CompareTo(y.num_rows);
            return res;
        }

        public int CompareTo(holdup_config_rec other)
        {
            return Compare(this, other);
        }

        public bool Equals(holdup_config_rec other)
        {
            return CompareTo(other) == 0;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "x";
            this.ps.Add(new DBParamEntry("num_rows", num_rows));
            this.ps.Add(new DBParamEntry("num_columns", num_columns));
            this.ps.Add(new DBParamEntry("glovebox_id", glovebox_id));
            this.ps.Add(new DBParamEntry("distance", distance));
        }

    }

    // cmd line values become the defaults for the constructors here, 
    // UI actual use pulls from same cmd line default settings first, then specifies changes to the defaults through a dialog, 
    // final state has params written with other acquire params on detector/mat type
    // NEXT: no UI available for these, where do they go?
    public class LMMMConfig : NCCConfig.LMMMConfig, IParameterBase, IComparable<LMMMConfig>, IEquatable<LMMMConfig>
    {
        public LMMMConfig()
            : base(NCCConfig.Config.ParameterBasis())
        {             
        }
        public LMMMConfig(LMMMConfig src)
            : base(src)
        {
        }

        public static int Compare(LMMMConfig x, LMMMConfig y)
        {
            int res = x.LEDs.CompareTo(y.LEDs);
            if (res == 0)
                res = x.Input.CompareTo(y.Input);
            if (res == 0)
                res = x.Debug.CompareTo(y.Debug);
            if (res == 0)
                res = x.HV.CompareTo(y.HV);
            if (res == 0)
                res = x.LLD.CompareTo(y.LLD);
            return res;
        }

        public int CompareTo(LMMMConfig other)
        {
            return Compare(this, other);
        }

        public bool Equals(LMMMConfig other)
        {
            return CompareTo(other) == 0;
        }

        private ParameterBase pb;
        public void GenParamList()
        {
            pb.GenParamList();
            pb.Table = "LMMMConfig";
            pb.ps.Add(new DBParamEntry("leds", LEDs));
            pb.ps.Add(new DBParamEntry("input", Input));
            pb.ps.Add(new DBParamEntry("debug", Debug));
            pb.ps.Add(new DBParamEntry("hv", HV));
            pb.ps.Add(new DBParamEntry("LLD", LLD)); // alias for VoltageTolerance on PTR32 and MCA527
            pb.ps.Add(new DBParamEntry("hvtimeout", HVTimeout));

        }

        public DB.ElementList ToDBElementList()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToDBElementList(false);
        }
        public string[] ToSimpleValueArray()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToSimpleValueArray();
        }
        public List<DBParamEntry> ParamList()
        {
            return pb.ps;
        }
        public bool modified
        {
            get
            {
                if (pb == null) pb = new ParameterBase();
                return pb.modified;
            }
            set
            {
                if (pb == null) pb = new ParameterBase();
                pb.modified = value;
            }
        }

    }

    // cmd line values become the defaults for the constructors here, 
    // UI actual use pulls from same cmd line default settings first, then specifies changes to the defaults through a dialog, 
    // final state has params written with other acquire params on detector/mat type
    public class LMMMNetComm : NCCConfig.LMMMNetComm, IParameterBase, IComparable<LMMMNetComm>, IEquatable<LMMMNetComm> //, IINCCStringRep
    {
        public LMMMNetComm()
            : base(NCCConfig.Config.ParameterBasis())
        {
			//resetVal(NCCConfig.NCCFlags.streamRawAnalysis, true, typeof(bool));
        }

        public LMMMNetComm(LMMMNetComm src) : base(src)
        {
        }

        public static int Compare(LMMMNetComm x, LMMMNetComm y)
        {
            int res = x.LMListeningPort.CompareTo(y.LMListeningPort);
            if (res == 0)
                res = x.Broadcast.CompareTo(y.Broadcast);
            if (res == 0)
                res = x.Port.CompareTo(y.Port);
            if (res == 0)
                res = x.Wait.CompareTo(y.Wait);
            if (res == 0)
                res = x.Subnet.CompareTo(y.Subnet);

            if (res == 0)
                res = x.ReceiveBufferSize.CompareTo(y.ReceiveBufferSize); 
            if (res == 0)
                res = x.ParseBufferSize.CompareTo(y.ParseBufferSize); 
            if (res == 0)
                res = x.UseAsynchAnalysis.CompareTo(y.UseAsynchAnalysis);
            if (res == 0)
                res = x.NumConnections.CompareTo(y.NumConnections);
            if (res == 0)
                res = x.UseAsynchFileIO.CompareTo(y.UseAsynchFileIO);
            if (res == 0)
                res = x.UsingStreamRawAnalysis.CompareTo(y.UsingStreamRawAnalysis);
            return res;
        }

        public int CompareTo(LMMMNetComm other)
        {
            return Compare(this, other);
        }

        public bool Equals(LMMMNetComm other)
        {
            return CompareTo(other) == 0;
        }


        private ParameterBase pb;
        public void GenParamList()
        {
            pb.GenParamList();
            pb.Table = "LMMMNetComm";
            pb.ps.Add(new DBParamEntry("broadcast", Broadcast));
            pb.ps.Add(new DBParamEntry("broadcastport", LMListeningPort));
            pb.ps.Add(new DBParamEntry("port", Port));
            pb.ps.Add(new DBParamEntry("subnet", Subnet));
            pb.ps.Add(new DBParamEntry("wait", Wait));
            pb.ps.Add(new DBParamEntry("numConnections", NumConnections));
            pb.ps.Add(new DBParamEntry("receiveBufferSize", ReceiveBufferSize));
            pb.ps.Add(new DBParamEntry("parseBufferSize", ParseBufferSize));
            pb.ps.Add(new DBParamEntry("useAsyncFileIO", UseAsynchFileIO));
            pb.ps.Add(new DBParamEntry("useAsyncAnalysis", UseAsynchAnalysis));
            pb.ps.Add(new DBParamEntry("streamRawAnalysis", UsingStreamRawAnalysis));
        }

        public DB.ElementList ToDBElementList()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToDBElementList(false);
        }
        public string[] ToSimpleValueArray()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToSimpleValueArray();
        }
        public List<DBParamEntry> ParamList()
        {
            return pb.ps;
        }
        public bool modified
        {
            get
            {
                if (pb == null) pb = new ParameterBase();
                return pb.modified;
            }
            set
            {
                if (pb == null) pb = new ParameterBase();
                pb.modified = value;
            }
        }

    }

    public class LMAcquireParams : NCCConfig.LMAcquireConfig, IParameterBase, IComparable<LMAcquireParams> //, IINCCStringRep
    {

        // cmd line values become the defaults for the constructors here, 
        // UI actual use pulls from same cmd line default settings first, then specifies changes to the defaults through a dialog, 
        // final state has params written with other acquire params on detector/mat type

        // Most params are in UI via LMConnection or HVPlateau, 
        public LMAcquireParams()
            : base(NCCConfig.Config.ParameterBasis())
        {             
        }
        public LMAcquireParams(DateTimeOffset dt)
            : base(NCCConfig.Config.ParameterBasis())
        {
            TimeStamp = new DateTimeOffset(dt.Ticks, dt.Offset);
        }

        public LMAcquireParams(LMAcquireParams src)
            : base(src)
        {
            Results = String.Copy(src.Results);
            TimeStamp = new DateTimeOffset(src.TimeStamp.Ticks, src.TimeStamp.Offset);
        }

        public void ApplyHVParams(HVCalibrationParameters hvp)
        {
            MinHV = hvp.MinHV;
            MaxHV = hvp.MaxHV;
            HVDuration = hvp.HVDuration;
            Delay = hvp.DelayMS;
            Step = hvp.Step;
        }

        string results;
        public string Results
        {
            get
            {
                if (String.IsNullOrEmpty(results))
                {
                    return NCC.CentralizedState.App.AppContext.RootPathOverride();
                }
                else if (NCC.CentralizedState.App.AppContext.DailyRootPath)
                {
                    string part = DateTime.Now.ToString("yyyy-MMdd");
                    if (!results.EndsWith(part))  // it's not the current day
                    {
                        Match m = Regex.Match(results, "\\d{4}-\\d{4}$");
                        if (m.Success)  // it is a pattern match meant to use the override daily path scheme, so use it
                        {
                            // strip and replace
                            return NCC.CentralizedState.App.AppContext.RootPathOverride();
                        }
                    }
                }                
                // else use the path exactly as user has specifeid, it is not overridden by the daily flag
                return results;
            }
            set
            {
                string warmed = TrimCmdLineFlagpath(value);
                results = warmed;
            }
        }

        public static int Compare(LMAcquireParams x, LMAcquireParams y)
        {
            int res = x.Results.CompareTo(y.Results);
            if (res == 0)
                res = x.Detector.CompareTo(y.Detector);
            if (res == 0)
                res = x.SaveOnTerminate.CompareTo(y.SaveOnTerminate);
            if (res == 0)
                res = x.IncludeConfig.CompareTo(y.IncludeConfig);
            if (res == 0)
                res = x.Message.CompareTo(y.Message);
            if (res == 0)
                res = x.LM.CompareTo(y.LM);
            if (res == 0)
                res = x.TimeStamp.CompareTo(y.TimeStamp);
            if (res == 0)
                res = x.AssayType.CompareTo(y.AssayType);
            if (res == 0)
                res = x.Feedback.CompareTo(y.Feedback);
            if (res == 0)
                res = x.Separation.CompareTo(y.Separation);
            if (res == 0)
                res = x.Cycles.CompareTo(y.Cycles);
            if (res == 0)
                res = x.Interval.CompareTo(y.Interval);
            if (res == 0)
                res = x.MinHV.CompareTo(y.MinHV);
            if (res == 0)
                res = x.MaxHV.CompareTo(y.MaxHV);
            if (res == 0)
                res = x.Step.CompareTo(y.Step);
            if (res == 0)
                res = x.HVDuration.CompareTo(y.HVDuration);
            if (res == 0)
                res = x.Delay.CompareTo(y.Delay);
            if (res == 0)
                res = x.HVX.CompareTo(y.HVX);

            return res;
        }

        public int CompareTo(LMAcquireParams other)
        {
            return Compare(this, other);
        }

        private ParameterBase pb;
        public void GenParamList()
        {
            pb.GenParamList();
            pb.Table = "LMAcquireParams";
            pb.ps.Add(new DBParamEntry("separation", Separation));
            pb.ps.Add(new DBParamEntry("interval", Interval));
            pb.ps.Add(new DBParamEntry("cycles", Cycles));
            pb.ps.Add(new DBParamEntry("minHV", MinHV));
            pb.ps.Add(new DBParamEntry("maxHV", MaxHV));
            pb.ps.Add(new DBParamEntry("step", Step));
            pb.ps.Add(new DBParamEntry("hvduration", HVDuration));
            pb.ps.Add(new DBParamEntry("delay", Delay));
            pb.ps.Add(new DBParamEntry("hvx", HVX));
            pb.ps.Add(new DBParamEntry("feedback", Feedback));
            pb.ps.Add(new DBParamEntry("saveOnTerminate", SaveOnTerminate));
            //pb.ps.Add(new DBParamEntry("detector", Detector)); // this is actually right on the acquire parms itself, so it is not saved here, only a cmd line flag can set it internally

            pb.ps.Add(new DBParamEntry("results", Results));
            pb.ps.Add(new DBParamEntry("includeConfig", IncludeConfig));
            pb.ps.Add(new DBParamEntry("message", Message));
            pb.ps.Add(new DBParamEntry("lm", LM));
            pb.ps.Add(new DBParamEntry("assayType", AssayType));
            pb.ps.Add(new DBParamEntry("MeasDate", TimeStamp));
            pb.ps.Add(new DBParamEntry("FADefault", (int)FADefault)); // new for 297
        }

        public DB.ElementList ToDBElementList()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToDBElementList(false);
        }
        public string[] ToSimpleValueArray()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToSimpleValueArray();
        }
        public List<DBParamEntry> ParamList()
        {
            return pb.ps;
        }
        public bool modified
        {
            get
            {
                if (pb == null) pb = new ParameterBase();
                return pb.modified;
            }
            set
            {
                if (pb == null) pb = new ParameterBase();
                pb.modified = value;
            }
        }

        public DateTimeOffset TimeStamp { get; set; }

		public FAType FADefault  { get; set; }
    }

    public class LMINCCAppContext : NCCConfig.AppContextConfig, IParameterBase
    {
        // read for DB first, then copy this object over to the global config space, then process cmd line
        public LMINCCAppContext() : base(NCCConfig.Config.ParameterBasis())
        {             
        }

        private ParameterBase pb;
        public void GenParamList()
        {
            if (pb == null) pb = new ParameterBase(); else pb.GenParamList();
            pb.Table = "LMINCCAppContext";
            pb.ps.Add(new DBParamEntry("root", RootPath));
            pb.ps.Add(new DBParamEntry("dailyRootPath", DailyRootPath)); 
            pb.ps.Add(new DBParamEntry("logging", Logging));
            //pb.ps.Add(new DBParamEntry("logAutoPath", DailyLogPath));
            pb.ps.Add(new DBParamEntry("logDetails", LoggingDetails));
            int i = (int)base.LevelAsUInt16;
            pb.ps.Add(new DBParamEntry("level", i));
            pb.ps.Add(new DBParamEntry("rolloverIntervalMin", RolloverIntervalMin));
            pb.ps.Add(new DBParamEntry("rolloverSizeMB", RolloverSizeMB));
            pb.ps.Add(new DBParamEntry("logResults", LogResults));
            pb.ps.Add(new DBParamEntry("fpPrec", FPPrecision));
            pb.ps.Add(new DBParamEntry("openResults", OpenResults));
            i = (int)Verbose();
            pb.ps.Add(new DBParamEntry("verbose", i));
            pb.ps.Add(new DBParamEntry("emulatorapp", EmuLoc));
            pb.ps.Add(new DBParamEntry("serveremulation", Emulate));
            pb.ps.Add(new DBParamEntry("fileinput", FileInput));
            pb.ps.Add(new DBParamEntry("recurse", Recurse));
            pb.ps.Add(new DBParamEntry("parseGen2", ParseGen2));
            pb.ps.Add(new DBParamEntry("replay", Replay));
            pb.ps.Add(new DBParamEntry("INCCParity", INCCParity));
            pb.ps.Add(new DBParamEntry("sortPulseFile", SortPulseFile));
            pb.ps.Add(new DBParamEntry("pulseFileAssay", PulseFileAssay));
            pb.ps.Add(new DBParamEntry("ptrFileAssay", PTRFileAssay));
            pb.ps.Add(new DBParamEntry("testDataFileAssay", TestDataFileAssay));
            pb.ps.Add(new DBParamEntry("dbDataAssay", DBDataAssay));
            pb.ps.Add(new DBParamEntry("reviewFileAssay", ReviewFileAssay));
            pb.ps.Add(new DBParamEntry("nilaFileAssay", MCA527FileAssay));
            pb.ps.Add(new DBParamEntry("opStatusPktInterval", StatusPacketCount));
            pb.ps.Add(new DBParamEntry("opStatusTimeInterval", StatusTimerMilliseconds));
            pb.ps.Add(new DBParamEntry("auxRatioReport", AuxRatioReport));
            pb.ps.Add(new DBParamEntry("autoCreateMissing", AutoCreateMissing));
            pb.ps.Add(new DBParamEntry("overwriteDefs", OverwriteImportedDefs));
            pb.ps.Add(new DBParamEntry("gen5RevDataFile", CreateINCC5TestDataFile));
            pb.ps.Add(new DBParamEntry("liveFileWrite", LiveFileWrite));
        }

        public DB.ElementList ToDBElementList()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToDBElementList(false);
        }
        public string[] ToSimpleValueArray()
        {
            if (pb == null) pb = new ParameterBase();
            GenParamList();
            return pb.ToSimpleValueArray();
        }
        public List<DBParamEntry> ParamList()
        {
            return pb.ps;
        }
        public bool modified
        {
            get { if (pb == null) pb = new ParameterBase(); 
                return pb.modified; }
            set { if (pb == null) pb = new ParameterBase(); 
                pb.modified = value; }
        }
    }
    
    // selection based on id and material, sap pulled from DB, used to pull methods and params from DB
    public class INCCSelector : IEquatable<INCCSelector>
    {
        public string detectorid;
        public string material;
        public string otheruniquelyidentifyinginformation;

        public INCCSelector()
        {
            otheruniquelyidentifyinginformation = String.Empty;
        }
        public INCCSelector(INCCSelector src)
        {
            detectorid = String.Copy(src.detectorid);
            material = String.Copy(src.material);
            otheruniquelyidentifyinginformation = String.Copy(src.otheruniquelyidentifyinginformation);
        }
        public INCCSelector(string id, string material)
        {
            this.detectorid = String.Copy(id);
            this.material = String.Copy(material);
            otheruniquelyidentifyinginformation = String.Empty;
        }
        public bool Equals(INCCSelector other)
        {
            if (this.detectorid.Equals(other.detectorid, StringComparison.OrdinalIgnoreCase) & this.material.Equals(other.material, StringComparison.OrdinalIgnoreCase)
                & this.otheruniquelyidentifyinginformation.Equals(other.otheruniquelyidentifyinginformation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return String.Format("<{0},{1}>{2}", detectorid, material, otheruniquelyidentifyinginformation);
        }

        public class SelectorEquality : EqualityComparer<INCCSelector>
        {
            public override int GetHashCode(INCCSelector sel)
            {
                int hCode = sel.detectorid.ToLower().GetHashCode() ^ sel.material.ToLower().GetHashCode() ^ sel.otheruniquelyidentifyinginformation.GetHashCode();
                return hCode.GetHashCode();
            }
            public override bool Equals(INCCSelector b1, INCCSelector b2)
            {
                return EqualityComparer<INCCSelector>.Default.Equals(b1, b2);
            }
        }
    }


    public class MeasurementMsg: ParameterBase
    {
        NCCReporter.LogLevels lvl;
        Int32 id;

        public DateTimeOffset dt { get; set; }

        public string text { get; set; }

        public MeasurementMsg()
        {
        }

        public MeasurementMsg(NCCReporter.LogLevels level, Int32 mid, string mtext)
        {
            this.text = String.Copy(mtext);
            lvl = level;
            id = mid;
            dt = DateTime.Now;
        }

        public MeasurementMsg(NCCReporter.LogLevels level, Int32 mid, string mtext, DateTimeOffset _dt)
        {
            this.text = String.Copy(mtext);
            lvl = level;
            id = mid;
            dt = new DateTimeOffset(_dt.Ticks, _dt.Offset);
        }


        public override string ToString()
        {
            // Analysis	Information	34014	The savory natto is served
            return String.Format("Analysis {0} {1} {2}", lvl.ToString(), id.ToString(), text);
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "analysis_messages";
            ps.Add(new DBParamEntry("msgid", id));
            ps.Add(new DBParamEntry("level", (Int32)lvl));
            ps.Add(new DBParamEntry("text", text));
            ps.Add(new DBParamEntry("ts", dt));
        } 
    }

    /// <summary>
    /// Results of a single HV set and read step
    /// </summary>
    public class HVStatus : ParameterBase, IComparable<HVStatus>, IEquatable<HVStatus>
    {
        public HVStatus()
        {
            Time = DateTime.Now;
            Counts = new ulong[NCC.CentralizedState.ChannelCount];
        }

        public ulong[] Counts;
        public int HVsetpt { get; set; }
        public int HVread { get; set; }
        public DateTimeOffset Time;

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "HVStatus";
            ps.Add(new DBParamEntry("setpt", HVsetpt));
            ps.Add(new DBParamEntry("read", HVread));
            ps.Add(new DBParamEntry("HVPDateTime", Time));
            ps.Add(new DBParamEntry("counts", Counts));
        }

        public static int Compare(HVStatus x, HVStatus y)
        {
            int res = x.Time.CompareTo(y.Time);
            if (res == 0)
                res = x.HVsetpt.CompareTo(y.HVsetpt);
            if (res == 0)
                res = x.HVread.CompareTo(y.HVread);
            if (res == 0)
                for (int i = 0; i < x.Counts.Length && i < y.Counts.Length; i++)
                {
                    if (x.Counts[i] == y.Counts[i])
                        continue;
                    res = (int)(x.Counts[i] - y.Counts[i]);
                    break;
                }
            return res;
        }

        public int CompareTo(HVStatus other)
        {
            return Compare(this, other);
        }

        public bool Equals(HVStatus other)
        {
            return CompareTo(other) == 0;
        }



    }

    /// <summary>
    /// HV parameters to use for a plateau
    /// </summary>
    public class HVCalibrationParameters : ParameterBase, IComparable<HVCalibrationParameters>, IEquatable<HVCalibrationParameters>
    {
        public HVCalibrationParameters(AnalysisDefs.LMAcquireParams acq)
        {
            MinHV = acq.MinHV;
            MaxHV = acq.MaxHV;
            Step = acq.Step;
            HVDuration = acq.HVDuration;
            DelayMS = acq.Delay; // todo: check this for unit conversion
        }
        public HVCalibrationParameters()
        {
            DelayMS = 2000;
        }
        public HVCalibrationParameters(HVCalibrationParameters src)
        {
            if (src == null)
                return;
            MinHV = src.MinHV;
            MaxHV = src.MaxHV;
            Step = src.Step;
            HVDuration = src.HVDuration;
            DelayMS = src.DelayMS;
        }
        public void Copy(HVCalibrationParameters src)
        {
            if (src == null)
                return;
            MinHV = src.MinHV;
            MaxHV = src.MaxHV;
            Step = src.Step;
            HVDuration = src.HVDuration;
            DelayMS = src.DelayMS;
            modified = src.modified;
        }

        public Int32 MinHV
        {
            get;
            set;
        }
        public Int32 MaxHV
        {
            get;
            set;
        }
        public Int32 Step
        {
            get;
            set;
        }
        public Int32 HVDuration
        {
            get;
            set;
        }
        public Int32 DelayMS
        {
            get;
            set;
        }

        public static int Compare(HVCalibrationParameters x, HVCalibrationParameters y)
        {
            int res = x.MinHV.CompareTo(y.MinHV);
            if (res == 0)
                res = x.MaxHV.CompareTo(y.MaxHV);
            if (res == 0)
                res = x.Step.CompareTo(y.Step);
            if (res == 0)
                res = x.HVDuration.CompareTo(y.HVDuration);
            if (res == 0)
                res = x.DelayMS.CompareTo(y.DelayMS);
            return res;
        }

        public int CompareTo(HVCalibrationParameters other)
        {
            return Compare(this, other);
        }

        public bool Equals(HVCalibrationParameters other)
        {
            return CompareTo(other) == 0;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "HVCalibrationParams";
            ps.Add(new DBParamEntry("minv", MinHV));
            ps.Add(new DBParamEntry("maxv", MaxHV));
            ps.Add(new DBParamEntry("stepv", Step));
            ps.Add(new DBParamEntry("duration", HVDuration));
            ps.Add(new DBParamEntry("delay", DelayMS));
        }
    }

	public class ResultFile : ParameterBase,  IComparable<ResultFile>, IEquatable<ResultFile>
    {
		public string Path  // csv file or INCC5-style text files; can have multiple output files if more than one SR params
		{
            get;
            set;
        }
        public ResultFile()
        {
			Path = string.Empty;
        }
        public ResultFile(string name)
        {
			Path = name;
        }
        public ResultFile(ResultFile src)
        {
            if (src == null)
				Path = string.Empty;
            else
				Path = string.Copy(src.Path);
        }

        public void Copy(ResultFile src)
        {
            if (src == null)
                return;
		    Path = string.Copy(src.Path);
        }
        public static int Compare(ResultFile x, ResultFile y)
        {
            int res = x.Path.CompareTo(y.Path);
            return res;
        }

        public int CompareTo(ResultFile other)
        {
            return Compare(this, other);
        }

		public bool Equals(ResultFile other)
        {
            return CompareTo(other) == 0;
        }

        public override void GenParamList()
        {
            base.GenParamList();
            this.Table = "results_filenames";
            this.ps.Add(new DBParamEntry("FileName", Path));
        }

    }

    public enum DBParamType { Bytes, String, Boolean, Double, UInt8, UInt16, UInt32, UInt64, Int8, Int16, Int32, Int64, DT, TS, DTOffset };
    public class DBParamList
    {
        public DBParamList()
        {
            ps = new List<DBParamEntry>();  // eliminate, occurs during the Get below
        }
        public List<DBParamEntry> ps;

        /// <summary>
        /// new empty list
        /// </summary>
        virtual public void GenParamList()
        {
            ps = new List<DBParamEntry>();
        }

        public DBParamEntry Find(string name)
        {
                return ps.Find(dbp => { return dbp.Name.Equals(name); });
        }

        /// <summary>
        /// For the DB API, just native types and an Array
        /// </summary>
        /// <returns></returns>
        public DB.ElementList ToDBElementList(bool generate = true)
        {
            if (generate)
                GenParamList(); // resolved in the child class
            DB.ElementList theParams = new DB.ElementList();

            int i = 0;
            foreach (DBParamEntry f in ps)
            {
                theParams.Add(f.AsElement);
                i++;
            }
            return theParams;
        }


        public string[] ToSimpleValueArray(bool generate = true)
        {
            if (generate) 
                GenParamList(); // resolved in the child class
            string[] vals = new string[ps.Count];

            int i = 0;
            foreach (DBParamEntry f in ps)
            {
                vals[i] = f.Value;
                i++;
            }
            return vals;
        }

        public static List<DBParamEntry> TuplePair(string id, Tuple tup)
        {
            List<DBParamEntry> l = new List<DBParamEntry>();
            l.Add(new DBParamEntry(id, tup.v));
            l.Add(new DBParamEntry(id + "_err", tup.err));
            return l;
        }
        public static List<DBParamEntry> TuplePair(Tuple tup, string id)
        {
            List<DBParamEntry> l = new List<DBParamEntry>();
            l.Add(new DBParamEntry(id, tup.v));
            l.Add(new DBParamEntry(id + "_err", tup.err));
            return l;
        }
        public static List<DBParamEntry> TupleArrayPair(string id, Tuple[] tups)
        {
            List<DBParamEntry> l = new List<DBParamEntry>();
            double[] v = new double[tups.Length];
            double[] err = new double[tups.Length];
            for (int i = 0; i < tups.Length; i++)
            {
                err[i] = (tups[i] != null ? tups[i].err : 0);
                v[i] =   (tups[i] != null ? tups[i].v : 0);
            }
            l.Add(new DBParamEntry(id, v));
            l.Add(new DBParamEntry(id + "_err", err));
            return l;
        }


        public string Table { get; set;  }
    }


    public class DBParamEntry : Tuple<string, string, DBParamType, bool>
    {
        public DBParamType ByteType { get; set; }

        public DBParamEntry()
            : base(String.Empty, String.Empty, DBParamType.String, false)
        {
        }

        public DBParamEntry(DBParamEntry src)
            : base(String.Copy(src.Item1), String.Copy(src.Item2), src.Item3, src.Item4)
        {
        }

        public DBParamEntry(string NewName, DBParamEntry src)
            : base(String.Copy(NewName), String.Copy(src.Item2), src.Item3, src.Item4)
        {
        }
        public DBParamEntry(string name, DateTime value) :
            base(String.Copy(name), value.ToString("s"), DBParamType.DT, true)
        {
        }
        public DBParamEntry(string name, DateTimeOffset value) :
            base(String.Copy(name), value.ToString("o"), DBParamType.DTOffset, true)
        {
        }
        public DBParamEntry(string name, TimeSpan value) :
            base(String.Copy(name), value.Ticks.ToString(), DBParamType.TS, true)
        {
        }
        public DBParamEntry(string name, string value) :
            base(String.Copy(name), String.Copy(NullStringFilter(value)), DBParamType.String, true)
        {
        }
        public DBParamEntry(string name, bool value) :
            base(String.Copy(name), (value ? "1" : "0"), DBParamType.Boolean, false)
        {
        }

        // dev note: could use Double.Epsilon as a signal value for this condition, then NaN can be written to DB and restored on read
        public static string NaNFilter(double d)
        {
            if (Double.IsNaN(d))
            {
                return Double.Epsilon.ToString();
            }
            else
            {
                return d.ToString();
            }
        }

        public static string NullStringFilter(string s)
        {
            if (s == null)
            {
                return String.Empty;
            }
            else
            {
                return s;
            }
        }

        public DBParamEntry(string name, double value) :
            base(String.Copy(name), NaNFilter(value), DBParamType.Double, false)
        {
        }
        public DBParamEntry(string name, Int32 value) :
            base(String.Copy(name), value.ToString(), DBParamType.Int32, false)
        {
        }

        public DBParamEntry(string name, UInt64 value) :
            base(String.Copy(name), value.ToString(), DBParamType.UInt64, false)
        {
        }

        public DBParamEntry(string name, string[] value) :
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.String, true)
        {
        }

        // string to byte conversions happen in the DB.Element class later on this side, start by encoding as string
        public DBParamEntry(string name, double[] value):
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.Double, true)
        {
            ByteType = DBParamType.String;
        }
        public DBParamEntry(string name, long[] value) :
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.Int64, true)
        {
            ByteType = DBParamType.String;
        }
        public DBParamEntry(string name, ulong[] value) :
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.UInt64, true)
        {
            ByteType = DBParamType.String;
        }
        public DBParamEntry(string name, int[] value) :
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.Int32, true)
        {
            ByteType = DBParamType.String;
        }
        public DBParamEntry(string name, bool[] value) :
            base(String.Copy(name), DB.Utils.Stringify(value), DBParamType.Boolean, true)
        {
            ByteType = DBParamType.String;
        }


        public string Name
        {
            get { return this.Item1; }
        }
        public string Value
        {
            get { return this.Item2; }
        }
        public DBParamType Type
        {
            get { return this.Item3; }
        }
        public bool Quote
        {
            get { return this.Item4; }
        }

        public DB.Element AsElement
        {
            get {
                if (ByteType == DBParamType.String) // a string rep of an array of objects
                    return new DB.Element(Item1, Item2, (int)Type);
                else
                    return new DB.Element(Item1, Item2, Item4); 
            }
        }
    }

    public interface IParameterBase
    {
        void GenParamList();
        DB.ElementList ToDBElementList();
        string[] ToSimpleValueArray();
        List<DBParamEntry> ParamList();
    }

    public class ParameterBase : DBParamList
    {
        public bool modified;

        public ParameterBase()
        {
            modified = false;
        }

        public override void GenParamList() { base.GenParamList(); }

    }


    public class CRISPParamList
    {
        public CRISPParamList()
        {
        }
        protected List<CRISPParam> pl;

        virtual public void GenParamList()
        {
            pl = new List<CRISPParam>();
        }

    }

    public class CRISPParam : Tuple<string, object>
    {
        public CRISPParam()
            : base(String.Empty, null)
        {
        }

        public CRISPParam(string name, string value) :
            base(String.Copy(name), String.Copy(value))
        {
        }
        public CRISPParam(string name, bool value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, double value)
            : base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, float value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, Byte value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, Int16 value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, UInt16 value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, Int32 value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, UInt32 value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, UInt64 value) :
            base(String.Copy(name), value)
        {
        }
        public CRISPParam(string name, Int64 value) :
            base(String.Copy(name), value)
        {
        }
        public string Name
        {
            get { return this.Item1; }
        }
        public string Value
        {
            get { return this.Item2.ToString(); }
        }
        public override string ToString()
        {
            Type t = Item2.GetType();
            if (t.Equals(typeof(String)))
                return Item1.ToString() + ":"+ "'" +  Item2.ToString() + "'";  // todo: need a full character escape processor here
            else
                return Item1.ToString() + ":"+ Item2.ToString();
        }
    }

}


