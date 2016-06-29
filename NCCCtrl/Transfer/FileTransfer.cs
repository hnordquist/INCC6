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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NCCReporter;

namespace NCCTransfer
{

    public class DescriptorPair
    {
        public string id, desc;

        public DescriptorPair()
        {
            id = string.Empty;
            desc = string.Empty;
        }
        public bool HasContent()
        {
            bool has = !string.IsNullOrWhiteSpace(id);
            return has;
        }
    }

    public class DetectorMaterialMethod : IEquatable<DetectorMaterialMethod>
    {
        public DetectorMaterialMethod()
        {
            item_type = string.Empty;
            detector_id = string.Empty;
        }
        public DetectorMaterialMethod(DetectorMaterialMethod src)
        {
            item_type = string.Copy(src.item_type);
            detector_id = string.Copy(src.detector_id);
            analysis_method = src.analysis_method;
            extra = src.extra;
        }
        public DetectorMaterialMethod(string it, string did, byte am)
        {
            item_type = it;
            detector_id = did;
            analysis_method = am;
        }
        public string item_type;
        public string detector_id;
        public byte analysis_method;
        public short extra; // -1, 0, 1 collar flag

        public bool Equals(DetectorMaterialMethod other)
        {
            if (detector_id.Equals(other.detector_id) & item_type.Equals(other.item_type)
                & analysis_method.Equals(other.analysis_method) & extra.Equals(other.extra))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class SelectorEquality : EqualityComparer<DetectorMaterialMethod>
        {
            public override int GetHashCode(DetectorMaterialMethod sel)
            {
                int hCode = sel.detector_id.ToLower().GetHashCode() ^ sel.item_type.ToLower().GetHashCode() ^ sel.analysis_method.GetHashCode() ^ sel.extra.GetHashCode();
                return hCode.GetHashCode();
            }
            public override bool Equals(DetectorMaterialMethod b1, DetectorMaterialMethod b2)
            {
                return Default.Equals(b1, b2);
            }
        }
    }

    //todo: identify shared data structures in the subclasses, move them to this parent class. 
    // and then design for additional file types and additional processing approaches for those file types, need a generalized file class to undergird all file-based processing.
    public class INCCTransferBase
    {
        protected LMLoggers.LognLM mlogger;
        public string Path;
		public bool Select;  // for UI-based filtering

        public INCCTransferBase(LMLoggers.LognLM logger, string mpath)
        {
            mlogger = logger;
            Path = string.IsNullOrEmpty(mpath) ? string.Empty : string.Copy(mpath);
			Select = true;
        }
        unsafe public virtual bool Restore(string source_path_filename)
        {
            return false;
        }

		unsafe public virtual bool Save(string path_filename)
        {
            return false;
        }

        // new material type
        public List<string> item_type_names_table = new List<string>();

		static internal string CleansePotentialFilename(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            char[] ca = s.ToCharArray();
            int i;
            char dchar;

			StringBuilder sb = new StringBuilder();
            char[] inv = System.IO.Path.GetInvalidFileNameChars();
            for (i = 0; i < ca.Length; i++)
            {
                dchar = ca[i];
                foreach (char c in inv)
                    if (dchar == c)
                    {
                        dchar = '_';
                        break;
                    }
                sb.Append(dchar);
            }
            return sb.ToString();
        }


    }
    
    //dev note: generally speaking the INCC DB item ids are not processed yet, only item types or materials types

    public class INCCInitialDataDetectorFile : INCCTransferBase
    {
        public INCCInitialDataDetectorFile(LMLoggers.LognLM logger, string mpath)
            : base(logger, mpath)
        {
        }

        public List<detector_rec> Detector = new List<detector_rec>();
        public List<sr_parms_rec> SRParms = new List<sr_parms_rec>();
        public List<bkg_parms_rec> BKGParms = new List<bkg_parms_rec>();
        public List<norm_parms_rec> NormParms = new List<norm_parms_rec>();
        public List<add_a_source_setup_rec> AASParms = new List<add_a_source_setup_rec>();
        public List<tm_bkg_parms_rec> TMBKGParms = new List<tm_bkg_parms_rec>();
        // dev note:  mid.SRType rides on the sr_parms struct, dont have it here unless it can be looked up in existing state
        public int tm_count = 0;

        unsafe new public bool Restore(string source_path_filename)   // migrated from restore.cpp
        {
            detector_rec detector = new detector_rec();
            sr_parms_rec sr_parms = new sr_parms_rec();
            bkg_parms_rec bkg_parms = new bkg_parms_rec();
            tm_bkg_parms_rec tm_bkg_parms = new tm_bkg_parms_rec();
            norm_parms_rec norm_parms = new norm_parms_rec();
            add_a_source_setup_rec add_a_source_setup = new add_a_source_setup_rec();

            long size_version_5_detector_parms = 6 + //  sizeof (INCCFileInfo.DETECTOR_SAVE_RESTORE) - 1 +
               Marshal.SizeOf(detector) + Marshal.SizeOf(sr_parms) +
                Marshal.SizeOf(bkg_parms) + Marshal.SizeOf(norm_parms) +
                Marshal.SizeOf(add_a_source_setup) + Marshal.SizeOf(tm_bkg_parms);

			
            // long size_version_6_detector_parms = size_version_5_detector_parms + List mode stuff added;

            bool result = false;
            FileStream stream;
            BinaryReader reader;
            FileInfo fi;

            mlogger.TraceEvent(LogLevels.Info, 33190, "Parsing the detector initial data  file {0}", source_path_filename);

            try
            {
                fi = new System.IO.FileInfo(source_path_filename);
                stream = fi.OpenRead();
                reader = new BinaryReader(stream);
            }
            catch (Exception e)
            {
                mlogger.TraceException(e);
                mlogger.TraceEvent(LogLevels.Warning, 33184, "Cannot open file {0}", source_path_filename);
                return result;
            }
            byte[] los_bytos = new byte[stream.Length];

            int thisread = reader.Read(los_bytos, 0, INCCFileInfo.DETECTOR_SAVE_RESTORE.Length);  // cannot throw due to length check under normal circumstances, so this is ok 
            string str2 = System.Text.ASCIIEncoding.ASCII.GetString(los_bytos, 0, thisread);

            if (!str2.Equals(INCCFileInfo.DETECTOR_SAVE_RESTORE)) // pre-check should prevent this condition 
            {
                reader.Close();
                return result;
            }

            try
            {
                int sz = 0;
                ///* read detector id file type */
                if (stream.Length == size_version_5_detector_parms)
                {
                    sz = Marshal.SizeOf(detector);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            detector = *(detector_rec*)pData;
                        }
                    else
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 33193, "Detector rec not read", source_path_filename);
                        return result;
                    }
                }
                else
                {
                    old_detector_rec old_detector = new old_detector_rec();
 
                    sz = Marshal.SizeOf(old_detector);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            old_detector = *(old_detector_rec*)pData;
                        }
                    else
                    {
                        mlogger.TraceEvent(LogLevels.Warning, 33193, "Old detector rec not read", source_path_filename);
                        return result;
                    }
                    TransferUtils.Copy(old_detector.detector_id, 0, detector.detector_id, 0, 12); // see struct dec
                    TransferUtils.Copy(old_detector.detector_type, 0, detector.detector_type, 0, 5); // see struct dec
                    TransferUtils.Copy(old_detector.electronics_id, 0, detector.electronics_id, 0, 9); // see struct dec
                }
                Detector.Add(detector);
                mlogger.TraceEvent(LogLevels.Info, 33195, "Transferring the detector '{0}' data",
                                 TransferUtils.str(detector.detector_id, INCC.MAX_DETECTOR_ID_LENGTH));

                sz = Marshal.SizeOf(sr_parms);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        sr_parms = *(sr_parms_rec*)pData;
                    }
                else
                {
                    throw new TransferUtils.TransferParsingException("sr_parms_rec read failed");
                }
                SRParms.Add(sr_parms);

                sz = Marshal.SizeOf(bkg_parms);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        bkg_parms = *(bkg_parms_rec*)pData;
                    }
                else
                {
                    throw new TransferUtils.TransferParsingException("bkg_parms read failed");
                }
                BKGParms.Add(bkg_parms);

                sz = Marshal.SizeOf(norm_parms);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        norm_parms = *(norm_parms_rec*)pData;
                    }
                else
                {
                    throw new TransferUtils.TransferParsingException("norm_parms read failed");
                }
                NormParms.Add(norm_parms);

                sz = Marshal.SizeOf(add_a_source_setup);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        add_a_source_setup = *(add_a_source_setup_rec*)pData;
                    }
                else
                {
                    add_a_source_setup.cm_rotation = 0;
                    throw new TransferUtils.TransferParsingException("add_a_source_setup_rec read failed");
                }
                AASParms.Add(add_a_source_setup);

                sz = Marshal.SizeOf(tm_bkg_parms);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        tm_bkg_parms = *(tm_bkg_parms_rec*)pData;
                        tm_count = 1;
                    }
                else
                {
                    tm_count = 0;
                }
                TMBKGParms.Add(tm_bkg_parms);

                // see BuildDetector for processing steps for data collected on this instance

                result = true;
            }
            catch (TransferUtils.TransferParsingException tpe)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33086, "Detector data file processing incomplete", tpe.Message);
                result = false;
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }

            try
            {
                reader.Close();
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }
            return result;
        }

		unsafe new public bool Save(string path)
        {
			int len = Detector.Count;
			for (int i = 0; i < len; i++)
			{
				detector_rec det = Detector[i];
				string detname =TransferUtils.str(det.detector_id, INCC.MAX_DETECTOR_ID_LENGTH);
				string s = CleansePotentialFilename(detname);
				Path = System.IO.Path.Combine(path, s + ".dat");
				FileStream stream = File.Create(Path);
				BinaryWriter bw = new BinaryWriter(stream);
				bw.Write(Encoding.ASCII.GetBytes(INCCFileInfo.DETECTOR_SAVE_RESTORE));
				WriteDetector(i, bw);
				WriteSRParms(i, bw);
				WriteBKGParms(i, bw);
				WriteNormParms(i, bw);
				WriteAASParms(i, bw);
				WriteTMBKGParms(i, bw);
				bw.Close();
				bw.Dispose();
				mlogger.TraceInformation("{0} Transfer detector {1}, saved as {2}", i+1, detname, Path);
			}

            return false;
        }

		unsafe void WriteDetector(int i, BinaryWriter bw)
		{
			int sz = sizeof(detector_rec);
			detector_rec p = Detector[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}
		unsafe void WriteSRParms(int i, BinaryWriter bw)
		{
			int sz = sizeof(sr_parms_rec);
			sr_parms_rec p = SRParms[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}
		unsafe void WriteBKGParms(int i, BinaryWriter bw)
		{
			int sz = sizeof(bkg_parms_rec);
			bkg_parms_rec p = BKGParms[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}

		unsafe void WriteNormParms(int i, BinaryWriter bw)
		{
			int sz = sizeof(norm_parms_rec);
			norm_parms_rec p = NormParms[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}

		unsafe void WriteAASParms(int i, BinaryWriter bw)
		{
			int sz = sizeof(add_a_source_setup_rec);
			add_a_source_setup_rec p = AASParms[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}

		unsafe void WriteTMBKGParms(int i, BinaryWriter bw)
		{
			int sz = sizeof(tm_bkg_parms_rec);
			tm_bkg_parms_rec p = TMBKGParms[i];
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}
    }

    public class INCCInitialDataCalibrationFile : INCCTransferBase
    {
        public class XFerDetectorMaterialMethodMap : Dictionary<DetectorMaterialMethod, object>
        {
            /// <summary>
            /// enumerate all the method calibration structs for this detector/material pair
            /// </summary>
            public IEnumerator GetMethodEnumerator(string det, string mtl)
            {
                foreach (KeyValuePair<DetectorMaterialMethod, object> pair in this)
                {
                    if (pair.Key.detector_id.Equals(det) && pair.Key.item_type.Equals(mtl))
                        yield return pair.Value;
                }
            }

            // returns the individual detector and material type pairings, use these as input to the GetMethodEnumerator 
            public IEnumerator GetDetectorMaterialEnumerator(string det = null)
            {
                foreach (KeyValuePair<DetectorMaterialMethod, object> pair in this)
                {
                    if (pair.Value.GetType().Equals(typeof(analysis_method_rec)))
					{
						if (det == null || ((det != null) && pair.Key.detector_id.Equals(det)))
                        yield return pair;
					}
                }
            }

			public bool GetPair(DetectorMaterialMethod dmm, out KeyValuePair<DetectorMaterialMethod, object> val)
			{
                foreach (KeyValuePair<DetectorMaterialMethod, object> pair in this)
				{
					if (dmm.Equals(pair.Key))
					{
						val = pair;
						return true;
					}
				}
				val = new KeyValuePair<DetectorMaterialMethod, object>();
				return false;
			}

			public List<string> GetDetectors
			{
				get
				{
					List<string> l = new List<string>();
					foreach (KeyValuePair<DetectorMaterialMethod, object> pair in this)
					{
						if (l.Contains(pair.Key.detector_id))
								continue;
						l.Add(pair.Key.detector_id);                
					}
					return l;
                }
            }
        }

        public INCCInitialDataCalibrationFile(LMLoggers.LognLM logger, string mpath)
            : base(logger, mpath)
        {

        }

        public List<analysis_method_multiplicity> mmkeyPtrList = new List<analysis_method_multiplicity>();

        public XFerDetectorMaterialMethodMap DetectorMaterialMethodParameters = new XFerDetectorMaterialMethodMap();
        
        unsafe new public bool Restore(string source_path_filename)   // migrated from restore.cpp
        {
            bool result = false;
            FileStream stream;
            BinaryReader reader;
            FileInfo fi;

            mlogger.TraceEvent(LogLevels.Info, 33290, "Parsing the calibration initial data file {0}", source_path_filename);
            try
            {
				fi = new System.IO.FileInfo(source_path_filename);
                stream = fi.OpenRead();
                reader = new BinaryReader(stream);
            }
            catch (Exception e)
            {
                mlogger.TraceException(e);
                mlogger.TraceEvent(LogLevels.Warning, 33284, "Cannot open file {0}", source_path_filename);
                return result;
            }
            byte[] los_bytos = new byte[stream.Length];

            int thisread = reader.Read(los_bytos, 0, INCCFileInfo.CALIBRATION_SAVE_RESTORE.Length);  // cannot throw due to length check under normal circumstances, so this is ok 
            string str2 = Encoding.ASCII.GetString(los_bytos, 0, thisread);

            if (!str2.Equals(INCCFileInfo.CALIBRATION_SAVE_RESTORE)) // pre-check should prevent this condition 
            {
                reader.Close();
                return result;
            }
            try
            {
                byte current_analysis_method = 0;
                DetectorMaterialMethod current = new DetectorMaterialMethod();
                int sz = 0;
                for (; ; )
                {
                    if (reader.PeekChar() != -1)
                        current_analysis_method = TransferUtils.ReadByte(reader, "analysis method");
                    else
                        break; // done with file I/O
                    switch (current_analysis_method)
                    {
                        case INCC.METHOD_NONE:  // important; this is the selected analyses for this detector/material type pair
                            analysis_method_rec analysis_method_record = new analysis_method_rec();
                            sz = Marshal.SizeOf(analysis_method_record);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { analysis_method_record = *(analysis_method_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("analysis_method_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(analysis_method_record.item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(analysis_method_record.analysis_method_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, analysis_method_record);
                            break;
                        case INCC.METHOD_CALCURVE:
                            cal_curve_rec cal_curve = new cal_curve_rec();
                            sz = Marshal.SizeOf(cal_curve);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { cal_curve = *(cal_curve_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("cal_curve_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(cal_curve.cal_curve_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(cal_curve.cal_curve_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, cal_curve); 
                            break;
                        case INCC.METHOD_AKNOWN:
                            known_alpha_rec known_alpha = new known_alpha_rec();
                            sz = Marshal.SizeOf(known_alpha);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { known_alpha = *(known_alpha_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("known_alpha read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(known_alpha.known_alpha_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(known_alpha.known_alpha_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, known_alpha);
                            break;
                        case INCC.METHOD_MKNOWN:
                            known_m_rec known_m = new known_m_rec();
                            sz = Marshal.SizeOf(known_m);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { known_m = *(known_m_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("known_m_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(known_m.known_m_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(known_m.known_m_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, known_m);       
                            break;
                        case INCC.METHOD_MULT:
                            multiplicity_rec multiplicity = new multiplicity_rec();
                            sz = Marshal.SizeOf(multiplicity);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { multiplicity = *(multiplicity_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("multiplicity_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(multiplicity.multiplicity_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(multiplicity.multiplicity_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, multiplicity);
                            if (multiplicity.mul_solve_efficiency == INCC.CONVENTIONAL_MULT_WEIGHTED)
                            {
                                analysis_method_multiplicity pmmkey = new analysis_method_multiplicity();
                                //            strcpy (pmmkey->multiplicity_item_type, current_item_type);
                                //            strcpy (pmmkey->multiplicity_detector_id, current_detector_id);
                                mmkeyPtrList.Add(pmmkey); //dev note: why isn't this finished? Do we need this part of the old INCC scheme?
                            }
                            break;
                        case INCC.DUAL_ENERGY_MULT_SAVE_RESTORE:
                           de_mult_rec de_mult = new de_mult_rec();
                           sz = Marshal.SizeOf(de_mult);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { de_mult = *(de_mult_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("de_mult_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(de_mult.de_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(de_mult.de_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, de_mult);  
                            break;
                        case INCC.METHOD_TRUNCATED_MULT:
                            truncated_mult_rec truncated_mult = new truncated_mult_rec();
                            sz = Marshal.SizeOf(truncated_mult);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { truncated_mult = *(truncated_mult_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("truncated_mult_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(truncated_mult.truncated_mult_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(truncated_mult.truncated_mult_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, truncated_mult);
                            break;
                        case INCC.METHOD_ACTIVE:
                            active_rec active = new active_rec();
                            sz = Marshal.SizeOf(active);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { active = *(active_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("active_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(active.active_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(active.active_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, active);
                            break;
                        case INCC.METHOD_ACTPAS:
                            active_passive_rec active_passive = new active_passive_rec();
                            sz = Marshal.SizeOf(active_passive);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { active_passive = *(active_passive_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("active_passive_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(active_passive.active_passive_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(active_passive.active_passive_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, active_passive);
                            break;
                        case INCC.COLLAR_DETECTOR_SAVE_RESTORE:
                            collar_detector_rec collar_detector = new collar_detector_rec();
                            sz = Marshal.SizeOf(collar_detector);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { collar_detector = *(collar_detector_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("collar_detector_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(collar_detector.collar_detector_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(collar_detector.collar_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            //((CollarDetectorMaterialMethod)current).reference_date = INCC.DateFrom(TransferUtils.str(collar_detector.col_reference_date, INCC.DATE_TIME_LENGTH));
                            //((CollarDetectorMaterialMethod)current).relative_doubles_rate = collar_detector.col_relative_doubles_rate;
							current.extra = (short)(collar_detector.collar_detector_mode == 0 ? 0 : 1);
							DetectorMaterialMethodParameters.Add(current, collar_detector);
                            mlogger.TraceEvent(LogLevels.Info, 103030, "Step 1 COLLAR_DETECTOR_SAVE_RESTORE {0} {1} {2}",current.detector_id,current.item_type,collar_detector.collar_detector_mode);

                            break;
                        case INCC.COLLAR_SAVE_RESTORE:
                            collar_rec collar = new collar_rec();
                            sz = Marshal.SizeOf(collar);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { collar = *(collar_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("collar_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(collar.collar_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(analysis_method_record.analysis_method_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
							current.extra = (short)(collar.collar_mode == 0 ? 0 : 1);
                            DetectorMaterialMethodParameters.Add(current, collar);
                            mlogger.TraceEvent(LogLevels.Info, 103031, "Step 2 COLLAR_SAVE_RESTORE [{0}] {1} {2}", current.detector_id, current.item_type, collar.collar_mode);
                            // dev note: if no preceding COLLAR_DETECTOR_SAVE_RESTORE, do not skip this entry!
                            break;
                        case INCC.COLLAR_K5_SAVE_RESTORE: // this is third in the series x 2
                            collar_k5_rec collar_k5 = new collar_k5_rec();
                            sz = Marshal.SizeOf(collar_k5);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { collar_k5 = *(collar_k5_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("collar_k5_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(collar_k5.collar_k5_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(analysis_method_record.analysis_method_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);                            
							current.extra = (short)(collar_k5.collar_k5_mode == 0 ? 0 : 1);
                            DetectorMaterialMethodParameters.Add(current, collar_k5);
                            mlogger.TraceEvent(LogLevels.Info, 103031, "Step 3 COLLAR_K5_SAVE_RESTORE [{0}] {1} {2}", current.detector_id, current.item_type, collar_k5.collar_k5_mode);
                            // dev note: if no preceding COLLAR_SAVE_RESTORE, skip this entry!
                            break;
                        case INCC.METHOD_ADDASRC:
                            add_a_source_rec add_a_source = new add_a_source_rec();
                            sz = Marshal.SizeOf(add_a_source);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { add_a_source = *(add_a_source_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("add_a_source_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(add_a_source.add_a_source_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(add_a_source.add_a_source_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, add_a_source);  
                            break;
                        case INCC.METHOD_ACTIVE_MULT:
                            active_mult_rec active_mult = new active_mult_rec();
                            sz = Marshal.SizeOf(active_mult);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { active_mult = *(active_mult_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("active_mult_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(active_mult.active_mult_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(active_mult.active_mult_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, active_mult);  
                            break;
                        case INCC.METHOD_CURIUM_RATIO:
                            curium_ratio_rec curium_ratio = new curium_ratio_rec();
                            sz = Marshal.SizeOf(curium_ratio);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                                fixed (byte* pData = los_bytos) { curium_ratio = *(curium_ratio_rec*)pData; }
                            else
                                throw new TransferUtils.TransferParsingException("curium_ratio_rec read failed");
                            current = new DetectorMaterialMethod(
                                TransferUtils.str(curium_ratio.curium_ratio_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
                                TransferUtils.str(curium_ratio.curium_ratio_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
                            DetectorMaterialMethodParameters.Add(current, curium_ratio);  
                            break;

                        case INCC.WMV_CALIB_TOKEN:
                            break;

                        default:
                            mlogger.TraceEvent(LogLevels.Warning, 33086, "Unhandled method value '{0:x8}'", current_analysis_method);
                            break;
                    }
                       

                    // add the current item info to the list
                    item_type_names_table.Add(current.item_type);
                        /* end processing for one set of calibration parameters */

                } /* end forever */

                if (mmkeyPtrList.Count > 0 && current_analysis_method == INCC.WMV_CALIB_TOKEN)
                {
                    //    fseek(fp, -1, SEEK_CUR);  //move back over 1 unsigned char WMV_CALIB_TOKEN marker for subsequent padder read
                    //    POSITION pos;
                    //    struct analysis_method_multiplicity *pmmkey;
                    //    for( pos = mmkeyPtrList.GetHeadPosition(); pos != NULL; )
                    //    {
                    //        pmmkey = (struct analysis_method_multiplicity *)mmkeyPtrList.GetNext( pos );
                    //        if (pmmkey)
                    //            DoWMVParametersTransfer(false, *pmmkey, fp);
                    //    }
                    //    for( pos = mmkeyPtrList.GetHeadPosition(); pos != NULL; )
                    //    {
                    //        pmmkey = (struct analysis_method_multiplicity *)mmkeyPtrList.GetNext( pos );
                    //        if (pmmkey)
                    //            delete pmmkey;
                    //    }

                }


                    result = true;
                //    mlogger.BatchLogL("Successfully applied calibration parameters from %.256s", source_path_filename);
                

            }
            catch (TransferUtils.TransferParsingException tpe)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33086, "Detector data file processing incomplete", tpe.Message);
                result = false;
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }

            try
            {
                reader.Close();
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }
            return result;

        }

		unsafe new public bool Save(string path)
		{
			List<string> l = DetectorMaterialMethodParameters.GetDetectors;
			mlogger.TraceInformation("{0} calibration sets to save off", l.Count);
			int i = 0;
			foreach (string det in l)
			{
				string s = CleansePotentialFilename(det);
				Path = System.IO.Path.Combine(path, s + ".cal");
				FileStream stream = File.Create(Path);
				BinaryWriter bw = new BinaryWriter(stream);
				bw.Write(Encoding.ASCII.GetBytes(INCCFileInfo.CALIBRATION_SAVE_RESTORE));
				IEnumerator iter = DetectorMaterialMethodParameters.GetDetectorMaterialEnumerator(det);
				while (iter.MoveNext())
				{
					analysis_method_rec rec = (analysis_method_rec)((KeyValuePair<DetectorMaterialMethod, object>)iter.Current).Value;
					WriteAM(rec, bw);
					//WriteSRParms(i, bw);
					//WriteBKGParms(i, bw);
					//WriteNormParms(i, bw);
					//WriteAASParms(i, bw);
					//WriteTMBKGParms(i, bw);
				}
				bw.Close();
				bw.Dispose();
				mlogger.TraceInformation("{0} Transfer calibration {1}, saved as {2}", i, det, Path);
				i++;
			}
			return false;
		}

		unsafe void WriteAM(analysis_method_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(analysis_method_rec);
			analysis_method_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_NONE);
			bw.Write(zb, 0, sz);
		}

		unsafe void WriteMul(multiplicity_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(multiplicity_rec);
			multiplicity_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_MULT);
			bw.Write(zb, 0, sz);
        }

    }

	public class TransferSummary
	{
			public DateTime dt;
			public string item, stratum, path, det;
			public bool overwrite; 
			public INCCTransferFile itf;
			public TransferSummary()
			{
			}
	}

    public class INCCTransferFile : INCCTransferBase
    {
        public INCCTransferFile(LMLoggers.LognLM logger, string mpath)
            : base(logger, mpath)
        {

        }


        unsafe new public bool Restore(string source_path_filename)   // migrated from restore.cpp
        {
            bool result = false;
            FileStream stream;
            BinaryReader reader;
            FileInfo fi;
            UInt16 n, number_runs;
            results_multiplicity_rec results_multiplicity;
            INCC.SaveResultsMask results_status;
            item_id_entry_rec item_id_entry;

            mlogger.TraceEvent(LogLevels.Info, 33090, "Parsing the measurement transfer file {0}", source_path_filename);

            results_rec results = new results_rec();
            meas_id id = new meas_id();
            try
            {
				fi = new System.IO.FileInfo(source_path_filename);
                stream = fi.OpenRead();
                reader = new BinaryReader(stream);
            }
            catch (Exception e)
            {
                mlogger.TraceException(e);
                mlogger.TraceEvent(LogLevels.Warning, 33084, "Cannot open file {0}", source_path_filename);
                return result;
            }
            int sz = Marshal.SizeOf(results);
            byte[] los_bytos = TransferUtils.TryReadBytes(reader, sz);
            if (los_bytos != null)
                fixed (byte* pData = los_bytos)
                {
                    results = *(results_rec*)pData;
                }
            else
            {
                mlogger.TraceEvent(LogLevels.Warning, 33093, "Results not read", source_path_filename);
                return result;
            }

            double db_version = 5.0;
            if (results.db_version != db_version)
            {
                old_results_rec old_results = new old_results_rec();
                sz = Marshal.SizeOf(old_results);
                stream.Seek(0, SeekOrigin.Begin);
                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                if (los_bytos != null)
                    fixed (byte* pData = los_bytos)
                    {
                        old_results = *(old_results_rec*)pData;
                    }
                else
                {
                }
                // todo: consider implementing this if old data still is of interest: convert_results (old_results, &results);
                mlogger.TraceEvent(LogLevels.Warning, 33094, "Cannot use file {0}, not a version 5 result", source_path_filename);
                return result;
            }

            try
            {

                TransferUtils.Copy(results.meas_date, 0, id.meas_date, 0, INCC.DATE_TIME_LENGTH);
                TransferUtils.Copy(results.meas_time, 0, id.meas_time, 0, INCC.DATE_TIME_LENGTH);
                TransferUtils.Copy(results.filename, 0, id.filename, 0, INCC.FILE_NAME_LENGTH);
                TransferUtils.Copy(results.results_detector_id, 0, id.results_detector_id, 0, INCC.MAX_DETECTOR_ID_LENGTH);

                mlogger.TraceEvent(LogLevels.Info, 33095, "Transferring the {0} measurement: {1}, detector {2}",
                                            TransferUtils.str(id.filename, INCC.FILE_NAME_LENGTH),
                                            INCC.DateTimeFrom(TransferUtils.str(id.meas_date, INCC.DATE_TIME_LENGTH), 
                                            TransferUtils.str(id.meas_time, INCC.DATE_TIME_LENGTH)).ToString("yy.MM.dd HH:mm:ss.ff K"), // my IAEA format
                                             TransferUtils.str(id.results_detector_id, INCC.MAX_DETECTOR_ID_LENGTH));

                ///* get word defining which results are valid for this data set */
                results_status = (INCC.SaveResultsMask)TransferUtils.ReadInt16(reader, "saved results");

                ///* add facility if necessary */
                string s = TransferUtils.str(results.results_facility, INCC.FACILITY_LENGTH);
                DescriptorPair facdesc = new DescriptorPair(); facdesc.id = s; facdesc.desc = TransferUtils.str(results.results_facility_description, INCC.DESCRIPTION_LENGTH);
                if (facdesc.HasContent())
                    facility_table.Add(facdesc);

                ///* add MBA if necessary */
                DescriptorPair mbadesc = new DescriptorPair(); mbadesc.id = TransferUtils.str(results.results_mba, INCC.MBA_LENGTH); mbadesc.desc = TransferUtils.str(results.results_mba_description, INCC.DESCRIPTION_LENGTH);
                if (mbadesc.HasContent())
                    mba_table.Add(mbadesc);

                ///* add isotopics data set if necessary */
                if ((results.meas_option == INCC.OPTION_ASSAY) ||
                    (results.meas_option == INCC.OPTION_CALIBRATION) ||
                    (results.meas_option == INCC.OPTION_HOLDUP))
                {
                    isotopics_rec isotopics = new isotopics_rec();

                    TransferUtils.Copy(results.item_am_date, 0, isotopics.am_date, 0, INCC.DATE_TIME_LENGTH);
                    TransferUtils.Copy(results.item_pu_date, 0, isotopics.pu_date, 0, INCC.DATE_TIME_LENGTH);
                    TransferUtils.Copy(results.item_isotopics_source_code, 0, isotopics.isotopics_source_code, 0, INCC.ISO_SOURCE_CODE_LENGTH);
                    TransferUtils.Copy(results.item_isotopics_id, 0, isotopics.isotopics_id, 0, INCC.MAX_ISOTOPICS_ID_LENGTH);
                    isotopics.am241 = results.item_am241;
                    isotopics.pu238 = results.item_pu238;
                    isotopics.pu239 = results.item_pu239;
                    isotopics.pu240 = results.item_pu240;
                    isotopics.pu241 = results.item_pu241;
                    isotopics.pu242 = results.item_pu242;

                    isotopics.am241_err = results.item_am241_err;
                    isotopics.pu238_err = results.item_pu238_err;
                    isotopics.pu239_err = results.item_pu239_err;
                    isotopics.pu240_err = results.item_pu240_err;
                    isotopics.pu241_err = results.item_pu241_err;
                    isotopics.pu242_err = results.item_pu242_err;
                    isotopics_table.Add(isotopics);
                }

                ///* add this result's item to the item id table */
                if (results.item_id[0] != '\0')
                {
                    TransferUtils.Copy(results.item_id, 0, item_id_entry.item_id_entry, 0, INCC.MAX_ITEM_ID_LENGTH);
                    TransferUtils.Copy(results.results_item_type, 0, item_id_entry.item_type_ascii, 0, INCC.MAX_ITEM_TYPE_LENGTH);
                    TransferUtils.Copy(results.item_isotopics_id, 0, item_id_entry.isotopics_id_ascii, 0, INCC.MAX_ISOTOPICS_ID_LENGTH);
                    TransferUtils.Copy(results.stratum_id, 0, item_id_entry.stratum_id_ascii, 0, INCC.MAX_STRATUM_ID_LENGTH);
                    TransferUtils.Copy(results.inventory_change_code, 0, item_id_entry.inventory_change_code_entry, 0, INCC.INVENTORY_CHG_LENGTH);
                    TransferUtils.Copy(results.io_code, 0, item_id_entry.io_code_entry, 0, INCC.INVENTORY_CHG_LENGTH);
                    item_id_entry.declared_mass_entry[0] = results.declared_mass;
                    item_id_entry.declared_u_mass_entry[0] = results.declared_u_mass;
                    item_id_entry.length_entry[0] = results.length;
                    // if this is a duplicate , worry about overwrite versus augment/add later

                    item_id_table.Add(item_id_entry);
                }

                // create a detector with all of it's dependents, can compare with existing stuff later, just build it in memory here for now

                restore_add_detector(results);

                results_rec_list.Add(results);

                #region method results
                // here we preserve specific typed method results records, these are generally small and are indexed by the detector/material type tuple
                if ((results_status & INCC.SaveResultsMask.SAVE_INIT_SRC_RESULTS) != 0)
                {
                    results_init_src_rec results_init_src = new results_init_src_rec();
                    sz = Marshal.SizeOf(results_init_src);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_init_src = *(results_init_src_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_init_src_rec read failed");
                    }
                    method_results_list.Add(results_init_src);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_BIAS_RESULTS) != 0)
                {
                    results_bias_rec results_bias = new results_bias_rec();
                    sz = Marshal.SizeOf(results_bias);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz); if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_bias = *(results_bias_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_bias_rec read failed");
                    }
                    method_results_list.Add(results_bias);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_PRECISION_RESULTS) != 0)
                {
                    results_precision_rec results_precision = new results_precision_rec();
                    sz = Marshal.SizeOf(results_precision);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_precision = *(results_precision_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_precision_rec read failed");
                    }
                    method_results_list.Add(results_precision);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_CAL_CURVE_RESULTS) != 0)
                {
                    results_cal_curve_rec results_cal_curve = new results_cal_curve_rec();
                    sz = Marshal.SizeOf(results_cal_curve);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_cal_curve = *(results_cal_curve_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_cal_curve_rec read failed");
                    }
                    method_results_list.Add(results_cal_curve);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_KNOWN_ALPHA_RESULTS) != 0)
                {
                    results_known_alpha_rec results_known_alpha = new results_known_alpha_rec();
                    sz = Marshal.SizeOf(results_known_alpha);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_known_alpha = *(results_known_alpha_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_known_alpha_rec read failed");
                    }
                    method_results_list.Add(results_known_alpha);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_KNOWN_M_RESULTS) != 0)
                {
                    results_known_m_rec results_known_m = new results_known_m_rec();
                    sz = Marshal.SizeOf(results_known_m);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_known_m = *(results_known_m_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_known_m_rec read failed");
                    }
                    method_results_list.Add(results_known_m);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_MULTIPLICITY_RESULTS) != 0)
                {
                    results_multiplicity = new results_multiplicity_rec();
                    sz = Marshal.SizeOf(results_multiplicity);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_multiplicity = *(results_multiplicity_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_multiplicity_rec read failed");
                    }
                    method_results_list.Add(results_multiplicity);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_DUAL_ENERGY_MULT_RESULTS) != 0)
                {
                    results_de_mult_rec results_de_mult = new results_de_mult_rec();
                    sz = Marshal.SizeOf(results_de_mult);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_de_mult = *(results_de_mult_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_de_mult_rec read failed");
                    }
                    method_results_list.Add(results_de_mult);
 
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_ACTIVE_PASSIVE_RESULTS) != 0)
                {
                    results_active_passive_rec results_active_passive = new results_active_passive_rec();
                    sz = Marshal.SizeOf(results_active_passive);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_active_passive = *(results_active_passive_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_active_passive_rec read failed");
                    }
                    method_results_list.Add(results_active_passive);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_COLLAR_RESULTS) != 0)
                {
                    results_collar_rec results_collar = new results_collar_rec();
                    sz = Marshal.SizeOf(results_collar);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_collar = *(results_collar_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_collar_rec read failed");
                    }
                    method_results_list.Add(results_collar);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_ACTIVE_RESULTS) != 0)
                {
                    results_active_rec results_active = new results_active_rec();
                    sz = Marshal.SizeOf(results_active);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_active = *(results_active_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_active_rec read failed");
                    }
                    method_results_list.Add(results_active);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_ACTIVE_MULTIPLICITY_RESULTS) != 0)
                {
                    results_active_mult_rec results_active_mult = new results_active_mult_rec();
                    sz = Marshal.SizeOf(results_active_mult);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_active_mult = *(results_active_mult_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_active_mult_rec read failed");
                    }
                    method_results_list.Add(results_active_mult);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_CURIUM_RATIO_RESULTS) != 0)
                {
                    results_curium_ratio_rec results_curium_ratio = new results_curium_ratio_rec();
                    sz = Marshal.SizeOf(results_curium_ratio);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_curium_ratio = *(results_curium_ratio_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_curium_ratio_rec read failed");
                    }
                    method_results_list.Add(results_curium_ratio);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_TRUNCATED_MULT_RESULTS) != 0)
                {
                    results_truncated_mult_rec results_truncated_mult = new results_truncated_mult_rec();
                    sz = Marshal.SizeOf(results_truncated_mult);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_truncated_mult = *(results_truncated_mult_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_truncated_mult_rec read failed");
                    }
                    method_results_list.Add(results_truncated_mult);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_TRUNCATED_MULT_BKG_RESULTS) != 0)
                {
                    results_tm_bkg_rec results_tm_bkg = new results_tm_bkg_rec();
                    sz = Marshal.SizeOf(results_tm_bkg);
                    los_bytos = TransferUtils.TryReadBytes(reader, sz);
                    if (los_bytos != null)
                        fixed (byte* pData = los_bytos)
                        {
                            results_tm_bkg = *(results_tm_bkg_rec*)pData;
                        }
                    else
                    {
                        throw new TransferUtils.TransferParsingException("results_tm_bkg_rec read failed");
                    }
                    method_results_list.Add(results_tm_bkg);
                }
                if ((results_status & INCC.SaveResultsMask.SAVE_ADD_A_SOURCE_RESULTS) != 0)
                {
                    results_add_a_source_rec results_add_a_source = new results_add_a_source_rec();
                    sz = Marshal.SizeOf(results_add_a_source);
                    los_bytos = TransferUtils.ReadBytes(reader, sz, "results_add_a_source_rec");
                    fixed (byte* pData = los_bytos)
                    {
                        results_add_a_source = *(results_add_a_source_rec*)pData;
                    }
                    method_results_list.Add(results_add_a_source);
                }
                #endregion method results

                // create cycle list here!
                // NEXT: this does not read multiple run sets for each add-a-src position, this only gets the first cycle set
                if (results.meas_option != INCC.OPTION_HOLDUP)
                {
                    number_runs = TransferUtils.ReadUInt16(reader, "number of runs");
                    mlogger.TraceEvent(LogLevels.Info, 33097, "Converting {0} INCC runs into cycles", number_runs);
                    run_rec run = new run_rec();
                    for (n = 0; n < number_runs; n++)
                    {
                        sz = Marshal.SizeOf(run);
                        los_bytos = TransferUtils.TryReadBytes(reader, sz);
                        if (los_bytos != null && los_bytos.Length >= sz)  // gonna fail here if size is not exact
                            fixed (byte* pData = los_bytos)
                            {
                                run = *(run_rec*)pData;
                            }
                        else
                        {
                            throw new TransferUtils.TransferParsingException("run_rec " + (n + 1).ToString() + " read failed");
                        }
                        run_rec_list.Add(run);
                    }
                }

                    int WMVcheck = 0;
                    if (results.meas_option == INCC.OPTION_ASSAY && reader.PeekChar() != -1)
                    {

                        CFrun_rec_list = new List<run_rec>[INCC.MAX_ADDASRC_POSITIONS+1];
                        for (int jj = 0; jj <= INCC.MAX_ADDASRC_POSITIONS; jj++)       
                            CFrun_rec_list[jj] = new List<run_rec>();

                        for (int j = 0; j < INCC.MAX_ADDASRC_POSITIONS; j++)
                        {
                            number_runs = TransferUtils.ReadUInt16(reader, "number of AAS CF" + (j + 1).ToString() + " runs");
                            mlogger.TraceEvent(LogLevels.Info, 33097, "Converting {0} AAS CF{1} INCC runs into cycles", number_runs, (j + 1));
                            run_rec run = new run_rec();
                            for (n = 0; n < number_runs; n++)
                            {
                                sz = Marshal.SizeOf(run);
                                los_bytos = TransferUtils.TryReadBytes(reader, sz);
                                if (los_bytos != null)
                                    fixed (byte* pData = los_bytos)
                                    {
                                        run = *(run_rec*)pData;
                                    }
                                else
                                {
                                    throw new TransferUtils.TransferParsingException("run_rec " + (n + 1).ToString() + " read failed");
                                }
                                CFrun_rec_list[j + 1].Add(run);
                                WMVcheck += number_runs;
                            }
                        }
                    }
                #region dev note: maybe can skip this WMV and HOLDUP for now
                //    if (results.meas_option == OPTION_HOLDUP)
                //    {
                //        for (j=0; j<HOLDUP_ROWS; j++)
                //        {
                //            for (k=0; k<HOLDUP_COLS; k++)
                //            {
                //                count = fread ((byte *) &number_runs,
                //                    sizeof (UInt16), 1, fp);
                //                if (count != 1)
                //                    break;
                //                if (number_runs == 0)
                //                    continue;
                //                d_keyfind (MEAS_ID, (byte *) &id, CURR_DB);
                //                d_setor (gl_holdup_run_sets[j][k], CURR_DB);
                //                for (n=0; n<number_runs; n++)
                //                {
                //                    struct run_rec run;
                //                    count = fread ((byte *) &run,
                //                        sizeof (struct run_rec), 1, fp);
                //                    if (count == 1)
                //                    {
                //                        d_fillnew (gl_holdup_run_recs[j][k],
                //                            (byte *) &run, CURR_DB);
                //                        d_connect (gl_holdup_run_sets[j][k], CURR_DB);
                //                    }
                //                    else
                //                        break;
                //                }
                //                if (count != 1)
                //                    break;
                //            }
                //            if (count != 1)
                //                break;
                //        }
                //    }
                //    // at end of file, read and imbue any potential 5.05+ WMV results
                //    if ((results_status & INCC.saveres.SAVE_MULTIPLICITY_RESULTS) != 0)
                //    {
                //        if (results_multiplicity.mul_solve_efficiency_res == CONVENTIONAL_MULT_WEIGHTED)
                //        {
                //            struct analysis_method_multiplicity mmkey;
                //            strcpy(mmkey.multiplicity_item_type, results_multiplicity.mul_multiplicity_item_type);
                //            strcpy(mmkey.multiplicity_detector_id, results_multiplicity.mul_multiplicity_detector_id);
                //            DoWMVResultsTransfer(false, id, mmkey, fp, WMVcheck);
                //        }
                //    }
                //    fclose (fp);
                //    result = true;

                #endregion dev note: maybe can skip this WMV and HOLDUP for now


                // now is the time to implement the detector tree creation with restore_replace_detector_structs semantics
                // but the work occurs in BuildMeasurement, not here, because the process is to accumulate work here, then execute there. 
                //    if ((overwrite_type == IDC_ALWAYS_OVERWRITE) ||
                //        (new_detector == TRUE))
                //    {
                //        restore_replace_detector_structs(results, id);
                //    }
                //    else if (overwrite_type == IDC_PROMPT_OVERWRITE)
                //    {
                //        mlogger.TraceInformation(
                //            "Detector %s already exists.\r\nDo you want to overwrite all of its parameters?",
                //            results.results_detector_id);
                //        GUI_ACTION response = GUI_YES; //GetMsgBox()->AskOnce (msg, GUI_ICON_QUESTION, GUI_NOYES);
                //        if (response == GUI_YES)
                //        {
                //            restore_replace_detector_structs(results, id);
                //        }
                //    }

                result = true;
                mlogger.TraceEvent(LogLevels.Verbose, 33098, "Intermediate results prepared for further processing");

            }
            catch (TransferUtils.TransferParsingException tpe)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33086, "Transfer file processing incomplete {0}", tpe.Message);
                result = false;
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }
            try
            {
                reader.Close();               
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }
            return result;
        }

        public List<results_rec> results_rec_list = new List<results_rec>();
        public List<iresultsbase> method_results_list = new List<iresultsbase>();

        // possible new fac and mba names 
        public List<DescriptorPair> facility_table = new List<DescriptorPair>();
        public List<DescriptorPair> mba_table = new List<DescriptorPair>();

        // possible new iso name and values 
        public List<isotopics_rec> isotopics_table = new List<isotopics_rec>();

        public List<item_id_entry_rec> item_id_table = new List<item_id_entry_rec>();
   
        // new stratum name
        public List<DescriptorPair> stratum_id_names_rec_table = new List<DescriptorPair>();

        // new detector
        public List<detector_rec> detector_rec_list = new List<detector_rec>();

        // los cycles de xfer
        public List<run_rec> run_rec_list = new List<run_rec>();

        // for add-a-src
        public List<run_rec>[] CFrun_rec_list;

        unsafe void restore_add_detector(results_rec results)
        {

           // stratum_id_names_rec stratum_id_names;
            detector_rec detector;

            // add results.results_item_type to item_type_names.item_type_names table
            item_type_names_table.Add(TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH));

            // check for empty string on these descriptors
            DescriptorPair desc = new DescriptorPair(); 
            desc.id = TransferUtils.str(results.stratum_id, INCC.MAX_STRATUM_ID_LENGTH); desc.desc = TransferUtils.str(results.stratum_id_description, INCC.DESCRIPTION_LENGTH);
            if (desc.HasContent())
                stratum_id_names_rec_table.Add(desc);
  
            // copy id, type and elec onto new detector struct
            TransferUtils.Copy(results.results_detector_id, 0, detector.detector_id, 0, INCC.MAX_DETECTOR_ID_LENGTH);
            TransferUtils.Copy(results.results_detector_type, 0, detector.detector_type, 0, INCC.DETECTOR_TYPE_LENGTH);
            TransferUtils.Copy(results.results_electronics_id, 0, detector.electronics_id, 0, INCC.ELECTRONICS_ID_LENGTH);

            // create new detector, and create new empty objects of the following types
            // init_detector_data (&detector);

            detector_rec_list.Add(detector); // not used, reconstructed in BuildMeasurement

        }

        

/* called to replace an existing detector's dependent structures */

//void restore_replace_detector_structs(struct results_rec& results, struct meas_id& id)

//{

//    struct stratum_id_names_rec stratum_id_names;
//    struct stratum_id_rec stratum_id_record;
//    struct stratum_id_key stratum_id_key;
//    struct bkg_parms_rec bkg_parms;
//    struct tm_bkg_parms_rec tm_bkg_parms;
//    struct sr_parms_rec sr_parms;
//    struct norm_parms_rec norm_parms;
//    struct cm_pu_ratio_rec cm_pu_ratio;
//    short index;
//    unsigned short collar_index, rod_index;
//    unsigned short i;
//    unsigned char found;
//    int status;

//    if (results.stratum_id[0] != '\0')
//    {
//        d_findfm (STRATUM_ID_NAMES_SET, CURR_DB);
//        d_recread ((char *) &stratum_id_names, CURR_DB);
//        index = -1;
//        for (i=0; i<MAX_STRATUM_IDS; i++)
//        {
//            if (!strcmpi (results.stratum_id,
//                stratum_id_names.stratum_id_names[i]))
//            {
//                index = MAX_STRATUM_IDS;
//                break;
//            }
//            if ((stratum_id_names.stratum_id_names[i][0] == '\0') &&
//                (index == -1))
//            {
//                index = i;
//            }
//        }
//        if (index == -1)
//        {
//            GetMsgBox()->Simple ("Stratum id table is full.\nYou must delete a stratum id before you can add a new one.");
//        }
//        else if (index < MAX_STRATUM_IDS)
//        {
//            strcpy (stratum_id_names.stratum_id_names[index],
//                results.stratum_id);
//            strcpy (stratum_id_names.stratum_id_names_description[index],
//                results.stratum_id_description);
//            d_findfm (STRATUM_ID_NAMES_SET, CURR_DB);
//            d_recwrite ((char *) &stratum_id_names, CURR_DB);
//            d_findfm (ACQUIRE_PARMS_SET, CURR_DB);
//            d_crwrite (ACQ_STRATUM_ID, results.stratum_id, CURR_DB);
//            build_stratum_id_table();
//        }

//        strcpy (stratum_id_key.stratum, results.stratum_id);
//        strcpy (stratum_id_key.stratum_id_detector_id,
//            results.results_detector_id);
//        strcpy (stratum_id_record.stratum, results.stratum_id);
//        strcpy (stratum_id_record.stratum_id_detector_id,
//            results.results_detector_id);
//        stratum_id_record.stratum_bias_uncertainty = results.bias_uncertainty;	
//        stratum_id_record.stratum_random_uncertainty =
//            results.random_uncertainty;	
//        stratum_id_record.stratum_systematic_uncertainty =
//            results.systematic_uncertainty;	
//        status = d_keyfind (STRATUM_ID_KEY, (char *) &stratum_id_key, CURR_DB);
//        if (status == S_NOTFOUND)
//        {
//            d_fillnew (STRATUM_ID_REC, (char *) &stratum_id_record, CURR_DB);
//        }
//        else
//        {
//            d_recwrite ((char *) &stratum_id_record, CURR_DB);
//        }
//    }

//    d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//    d_setor (DETECTOR_BKG_PARMS_SET, CURR_DB);
//    d_findfm (DETECTOR_BKG_PARMS_SET, CURR_DB);
//    d_recread ((char *) &bkg_parms, CURR_DB);
//    if (results.meas_option != OPTION_BKG)
//    {
//        bkg_parms.curr_passive_bkg_singles_rate =
//            results.passive_bkg_singles_rate;
//        bkg_parms.curr_passive_bkg_doubles_rate =
//            results.passive_bkg_doubles_rate;
//        bkg_parms.curr_passive_bkg_triples_rate =
//            results.passive_bkg_triples_rate;
//        bkg_parms.curr_active_bkg_singles_rate =
//            results.active_bkg_singles_rate;
//        bkg_parms.curr_passive_bkg_singles_err =
//            results.passive_bkg_singles_rate_err;
//        bkg_parms.curr_passive_bkg_doubles_err =
//            results.passive_bkg_doubles_rate_err;
//        bkg_parms.curr_passive_bkg_triples_err =
//            results.passive_bkg_triples_rate_err;
//        bkg_parms.curr_active_bkg_singles_err =
//            results.active_bkg_singles_rate_err;
//        bkg_parms.curr_passive_bkg_scaler1_rate =
//            results.passive_bkg_scaler1_rate;
//        bkg_parms.curr_passive_bkg_scaler2_rate =
//            results.passive_bkg_scaler2_rate;
//        bkg_parms.curr_active_bkg_scaler1_rate =
//            results.active_bkg_scaler1_rate;
//        bkg_parms.curr_active_bkg_scaler2_rate =
//            results.active_bkg_scaler2_rate;
//    }
//    else if (results.well_config == CONFIG_PASSIVE)
//    {
//        bkg_parms.curr_passive_bkg_singles_rate = results.singles;
//        bkg_parms.curr_passive_bkg_doubles_rate = results.doubles;
//        bkg_parms.curr_passive_bkg_triples_rate = results.triples;
//        bkg_parms.curr_passive_bkg_singles_err = results.singles_err;
//        bkg_parms.curr_passive_bkg_doubles_err = results.doubles_err;
//        bkg_parms.curr_passive_bkg_triples_err = results.triples_err;
//        bkg_parms.curr_passive_bkg_scaler1_rate = results.scaler1;
//        bkg_parms.curr_passive_bkg_scaler2_rate = results.scaler2;
//    }
//    else if (results.well_config == CONFIG_ACTIVE)
//    {
//        bkg_parms.curr_active_bkg_singles_rate = results.singles;
//        bkg_parms.curr_active_bkg_singles_err = results.singles_err;
//        bkg_parms.curr_active_bkg_scaler1_rate = results.scaler1;
//        bkg_parms.curr_active_bkg_scaler2_rate = results.scaler2;
//    }
//    d_recwrite ((char *) &bkg_parms, CURR_DB);

//    d_keyfind (MEAS_ID, (char *) &id, CURR_DB);
//    d_setor (RESULTS_TM_BKG_SET, CURR_DB);
//    status = d_findfm (RESULTS_TM_BKG_SET, CURR_DB);
//    if (status == S_OKAY)
//    {
//        d_recread ((char *) &results_tm_bkg, CURR_DB);
//        d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//        d_setor (DETECTOR_TM_BKG_PARMS_SET, CURR_DB);
//        d_findfm (DETECTOR_TM_BKG_PARMS_SET, CURR_DB);
//        d_recread ((char *) &tm_bkg_parms, CURR_DB);
//        tm_bkg_parms.tm_singles_bkg = results_tm_bkg.results_tm_singles_bkg;
//        tm_bkg_parms.tm_singles_bkg_err =
//            results_tm_bkg.results_tm_singles_bkg_err;
//        tm_bkg_parms.tm_zeros_bkg = results_tm_bkg.results_tm_zeros_bkg;
//        tm_bkg_parms.tm_zeros_bkg_err = results_tm_bkg.results_tm_zeros_bkg_err;
//        tm_bkg_parms.tm_ones_bkg = results_tm_bkg.results_tm_ones_bkg;
//        tm_bkg_parms.tm_ones_bkg_err = results_tm_bkg.results_tm_ones_bkg_err;
//        tm_bkg_parms.tm_twos_bkg = results_tm_bkg.results_tm_twos_bkg;
//        tm_bkg_parms.tm_twos_bkg_err = results_tm_bkg.results_tm_twos_bkg_err;
//        tm_bkg_parms.tm_bkg = TRUE;
//        d_recwrite ((char *) &tm_bkg_parms, CURR_DB);
//    }

//    d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//    d_setor (DETECTOR_SR_PARMS_SET, CURR_DB);
//    d_findfm (DETECTOR_SR_PARMS_SET, CURR_DB);
//    d_recread ((char *) &sr_parms, CURR_DB);
//    sr_parms.predelay = results.results_predelay;
//    sr_parms.gate_length = results.results_gate_length;
//    sr_parms.gate_length2 = results.results_gate_length2;
//    sr_parms.high_voltage = results.results_high_voltage;
//    sr_parms.die_away_time = results.results_die_away_time;
//    sr_parms.efficiency = results.results_efficiency;
//    sr_parms.multiplicity_deadtime = results.results_multiplicity_deadtime;
//    sr_parms.coeff_a_deadtime = results.results_coeff_a_deadtime;
//    sr_parms.coeff_b_deadtime = results.results_coeff_b_deadtime;
//    sr_parms.coeff_c_deadtime = results.results_coeff_c_deadtime;
//    sr_parms.doubles_gate_fraction = results.results_doubles_gate_fraction;
//    sr_parms.triples_gate_fraction = results.results_triples_gate_fraction;
//    d_recwrite ((char *) &sr_parms, CURR_DB);
//    calc_alpha_beta (sr_parms.sr_detector_id);

//    if ((results_status & SAVE_INIT_SRC_RESULTS) != 0)
//    {
//        d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//        d_setor (DETECTOR_NORM_PARMS_SET, CURR_DB);
//        d_findfm (DETECTOR_NORM_PARMS_SET, CURR_DB);
//        d_recread ((char *) &norm_parms, CURR_DB);
//        strcpy (norm_parms.source_id, results_init_src.init_src_id);
//        norm_parms.bias_mode = results_init_src.init_src_mode;
//        strcpy (norm_parms.ref_date, results.meas_date);
//        norm_parms.amli_ref_singles_rate = results.singles;
//        if (norm_parms.bias_mode == IDC_BIAS_CF252_SINGLES)
//        {
//            norm_parms.cf252_ref_doubles_rate = results.singles;
//            norm_parms.cf252_ref_doubles_rate_err = results.singles_err;
//        }
//        else
//        {
//            norm_parms.cf252_ref_doubles_rate = results.doubles;
//            norm_parms.cf252_ref_doubles_rate_err = results.doubles_err;
//        }
//        d_recwrite ((char *) &norm_parms, CURR_DB);
//    }

//    if ((results_status & SAVE_BIAS_RESULTS) != 0)
//    {
//        d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//        d_setor (DETECTOR_NORM_PARMS_SET, CURR_DB);
//        d_findfm (DETECTOR_NORM_PARMS_SET, CURR_DB);
//        d_recread ((char *) &norm_parms, CURR_DB);
//        strcpy (norm_parms.source_id, results_bias.bias_source_id);
//        norm_parms.bias_mode = results_bias.results_bias_mode;
//        d_recwrite ((char *) &norm_parms, CURR_DB);
//    }

//    strcpy (analysis_method_key.item_type, results.results_item_type);
//    strcpy (analysis_method_key.analysis_method_detector_id,
//        results.results_detector_id);
//    status = d_keyfind (ANALYSIS_METHOD, (char *) &analysis_method_key, CURR_DB);
//    if (status == S_OKAY)
//    {
//        d_recread ((char *) &analysis_method_record, CURR_DB);
//    }
//    else
//    {
//        strcpy (analysis_method_record.item_type, results.results_item_type);
//        strcpy (analysis_method_record.analysis_method_detector_id,
//            results.results_detector_id);
//        analysis_method_record.cal_curve = 0;
//        analysis_method_record.known_alpha = 0;
//        analysis_method_record.known_m = 0;
//        analysis_method_record.multiplicity = 0;
//        analysis_method_record.add_a_source = 0;
//        analysis_method_record.active = 0;
//        analysis_method_record.active_mult = 0;
//        analysis_method_record.active_passive = 0;
//        analysis_method_record.collar = 0;
//        analysis_method_record.curium_ratio = 0;
//        analysis_method_record.truncated_mult = 0;
//        analysis_method_record.normal_method = results.primary_analysis_method;
//        analysis_method_record.backup_method = METHOD_NONE;
//        analysis_method_record.analysis_method_spare1 = 0;
//        analysis_method_record.analysis_method_spare2 = 0;
//        analysis_method_record.analysis_method_spare3 = 0;
//        analysis_method_record.analysis_method_spare4 = 0;
//        d_fillnew (ANALYSIS_METHOD_REC,
//            (char *) &analysis_method_record, CURR_DB);
//        d_findfm (ITEM_TYPE_NAMES_SET, CURR_DB);
//        d_recread ((char *) &item_type_names, CURR_DB);
//        index = -1;
//        for (i=0; i<MAX_ITEM_TYPES; i++)
//        {
//            if (!strcmpi (results.results_item_type,
//                item_type_names.item_type_names[i]))
//            {
//                index = i;
//                break;
//            }
//            if ((item_type_names.item_type_names[i][0] == '\0') &&
//                (index == -1))
//            {
//                index = i;
//            }
//        }
//        if (index == -1)
//        {
//            GetMsgBox()->Simple ("Material type table is full.\nYou must delete a material type before you can add a new one.");
//        }
//        else
//        {
//            strcpy (item_type_names.item_type_names[index],
//                results.results_item_type);
//            d_recwrite ((char *) &item_type_names, CURR_DB);
//            build_item_type_table();
//        }
//    }
//    d_findfm (ACQUIRE_PARMS_SET, CURR_DB);
//    d_crwrite (ACQ_ITEM_TYPE, results.results_item_type, CURR_DB);

//    if ((results_status & SAVE_CAL_CURVE_RESULTS) != 0)
//    {
//        analysis_method_record.cal_curve = 1;
//        strcpy (method_cal_curve_key.cal_curve_item_type,
//            results.results_item_type);
//        strcpy (method_cal_curve_key.cal_curve_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_CAL_CURVE,
//            (char *) &method_cal_curve_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &cal_curve, CURR_DB);
//        }
//        else
//        {
//            cal_curve.cc_lower_mass_limit = -1e8;
//            cal_curve.cc_upper_mass_limit = 1e8;
//            for (i=0; i<MAX_NUM_CALIB_PTS; i++)
//            {
//                cal_curve.cc_dcl_mass[i] = 0.;
//                cal_curve.cc_doubles[i] = 0.;
//            }
//        }
//        strcpy (cal_curve.cal_curve_item_type, results.results_item_type);
//        strcpy (cal_curve.cal_curve_detector_id, results.results_detector_id);
//        cal_curve.cal_curve_equation = results_cal_curve.cc_cal_curve_equation;
//        cal_curve.cc_a = results_cal_curve.cc_a_res;
//        cal_curve.cc_b = results_cal_curve.cc_b_res;
//        cal_curve.cc_c = results_cal_curve.cc_c_res;
//        cal_curve.cc_d = results_cal_curve.cc_d_res;
//        cal_curve.cc_var_a = results_cal_curve.cc_var_a_res;
//        cal_curve.cc_var_b = results_cal_curve.cc_var_b_res;
//        cal_curve.cc_var_c = results_cal_curve.cc_var_c_res;
//        cal_curve.cc_var_d = results_cal_curve.cc_var_d_res;
//        cal_curve.cc_covar_ab = results_cal_curve.cc_covar_ab_res;
//        cal_curve.cc_covar_ac = results_cal_curve.cc_covar_ac_res;
//        cal_curve.cc_covar_ad = results_cal_curve.cc_covar_ad_res;
//        cal_curve.cc_covar_bc = results_cal_curve.cc_covar_bc_res;
//        cal_curve.cc_covar_bd = results_cal_curve.cc_covar_bd_res;
//        cal_curve.cc_covar_cd = results_cal_curve.cc_covar_cd_res;
//        cal_curve.cc_sigma_x = results_cal_curve.cc_sigma_x_res;
//        cal_curve.cc_cal_curve_type =
//            results_cal_curve.cc_cal_curve_type_res;
//        cal_curve.cc_heavy_metal_corr_factor =
//            results_cal_curve.cc_heavy_metal_corr_factor_res;
//        cal_curve.cc_heavy_metal_reference =
//            results_cal_curve.cc_heavy_metal_reference_res;
//        cal_curve.cc_percent_u235 = results_cal_curve.cc_percent_u235_res;
//        for (i=0; i<NUMBER_CC_SPARES; i++)
//            cal_curve.cal_curve_spares[i] = results_cal_curve.cc_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_CAL_CURVE,
//            (char *) &method_cal_curve_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &cal_curve, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (CAL_CURVE_REC, (char *) &cal_curve, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_KNOWN_ALPHA_RESULTS) != 0)
//    {
//        analysis_method_record.known_alpha = 1;
//        strcpy (method_known_alpha_key.known_alpha_item_type,
//            results.results_item_type);
//        strcpy (method_known_alpha_key.known_alpha_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_KNOWN_ALPHA,
//            (char *) &method_known_alpha_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &known_alpha, CURR_DB);
//        }
//        else
//        {
//            known_alpha.ka_lower_mass_limit = -1e8;
//            known_alpha.ka_upper_mass_limit = 1e8;
//            for (i=0; i<MAX_NUM_CALIB_PTS; i++)
//            {
//                known_alpha.ka_dcl_mass[i] = 0.;
//                known_alpha.ka_doubles[i] = 0.;
//            }
//        }
//        strcpy (known_alpha.known_alpha_item_type, results.results_item_type);
//        strcpy (known_alpha.known_alpha_detector_id, results.results_detector_id);
//        known_alpha.ka_alpha_wt = results_known_alpha.ka_alpha_wt_res;
//        known_alpha.ka_rho_zero = results_known_alpha.ka_rho_zero_res;
//        known_alpha.ka_k = results_known_alpha.ka_k_res;
//        known_alpha.ka_a = results_known_alpha.ka_a_res;
//        known_alpha.ka_b = results_known_alpha.ka_b_res;
//        known_alpha.ka_var_a = results_known_alpha.ka_var_a_res;
//        known_alpha.ka_var_b = results_known_alpha.ka_var_b_res;
//        known_alpha.ka_covar_ab = results_known_alpha.ka_covar_ab_res;
//        known_alpha.ka_sigma_x = results_known_alpha.ka_sigma_x_res;
//        /*for (i=0; i<NUMBER_KA_SPARES; i++)
//        {
//        known_alpha.known_alpha_spares[i] =
//        results_known_alpha.ka_spares_res[i];
//        }*/
//        status = d_keyfind (ANALYSIS_METHOD_KNOWN_ALPHA,
//            (char *) &method_known_alpha_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &known_alpha, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (KNOWN_ALPHA_REC, (char *) &known_alpha, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_KNOWN_M_RESULTS) != 0)
//    {
//        analysis_method_record.known_m = 1;
//        strcpy (method_known_m_key.known_m_item_type,
//            results.results_item_type);
//        strcpy (method_known_m_key.known_m_detector_id,
//            results.results_detector_id);
//        strcpy (known_m.known_m_item_type, results.results_item_type);
//        strcpy (known_m.known_m_detector_id, results.results_detector_id);
//        known_m.km_sf_rate = results_known_m.km_sf_rate_res;
//        known_m.km_vs1 = results_known_m.km_vs1_res;
//        known_m.km_vs2 = results_known_m.km_vs2_res;
//        known_m.km_vi1 = results_known_m.km_vi1_res;
//        known_m.km_vi2 = results_known_m.km_vi2_res;
//        known_m.km_b = results_known_m.km_b_res;
//        known_m.km_c = results_known_m.km_c_res;
//        known_m.km_sigma_x = results_known_m.km_sigma_x_res;
//        for (i=0; i<NUMBER_KM_SPARES; i++)
//            known_m.known_m_spares[i] = results_known_m.km_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_KNOWN_M,
//            (char *) &method_known_m_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &known_m, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (KNOWN_M_REC, (char *) &known_m, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_MULTIPLICITY_RESULTS) != 0)
//    {
//        analysis_method_record.multiplicity = 1;
//        strcpy (method_multiplicity_key.multiplicity_item_type,
//            results.results_item_type);
//        strcpy (method_multiplicity_key.multiplicity_detector_id,
//            results.results_detector_id);
//        strcpy (multiplicity.multiplicity_item_type, results.results_item_type);
//        strcpy (multiplicity.multiplicity_detector_id,
//            results.results_detector_id);
//        multiplicity.mul_solve_efficiency =
//            results_multiplicity.mul_solve_efficiency_res;
//        multiplicity.mul_sf_rate = results_multiplicity.mul_sf_rate_res;
//        multiplicity.mul_vs1 = results_multiplicity.mul_vs1_res;
//        multiplicity.mul_vs2 = results_multiplicity.mul_vs2_res;
//        multiplicity.mul_vs3 = results_multiplicity.mul_vs3_res;
//        multiplicity.mul_vi1 = results_multiplicity.mul_vi1_res;
//        multiplicity.mul_vi2 = results_multiplicity.mul_vi2_res;
//        multiplicity.mul_vi3 = results_multiplicity.mul_vi3_res;
//        multiplicity.mul_a = results_multiplicity.mul_a_res;
//        multiplicity.mul_b = results_multiplicity.mul_b_res;
//        multiplicity.mul_c = results_multiplicity.mul_c_res;
//        multiplicity.mul_sigma_x = results_multiplicity.mul_sigma_x_res;
//        multiplicity.mul_alpha_weight =
//            results_multiplicity.mul_alpha_weight_res;
//        for (i=0; i<NUMBER_MUL_SPARES; i++)
//            multiplicity.multiplicity_spares[i] =
//            results_multiplicity.mul_spares_res[i];
//        multiplicity.multeffcorfactor =
//            results_multiplicity.resultsmulteffcorfactor;

//        status = d_keyfind (ANALYSIS_METHOD_MULTIPLICITY,
//            (char *) &method_multiplicity_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &multiplicity, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (MULTIPLICITY_REC, (char *) &multiplicity, CURR_DB);
//        }
//        // WMV todo: not clear if any action is required here yet
//        // DoWMVParametersTransfer(bool bSave,
//        //	const struct acquire_parms_rec& acquire_parms,
//        //	const struct analysis_method_multiplicity& mmkey);

//    }
//    if ((results_status & SAVE_DUAL_ENERGY_MULT_RESULTS) != 0)
//    {
//        strcpy (analysis_method_de_mult_key.de_item_type,
//            results.results_item_type);
//        strcpy (analysis_method_de_mult_key.de_detector_id,
//            results.results_detector_id);
//        strcpy (de_mult.de_item_type, results.results_item_type);
//        strcpy (de_mult.de_detector_id, results.results_detector_id);
//        de_mult.de_inner_ring_efficiency =
//            results_de_mult.de_inner_ring_efficiency_res;
//        de_mult.de_outer_ring_efficiency =
//            results_de_mult.de_outer_ring_efficiency_res;
//        for (i=0; i<MAX_DUAL_ENERGY_ROWS; i++)
//        {
//            de_mult.de_neutron_energy[i] =
//                results_de_mult.de_neutron_energy_res[i];
//            de_mult.de_detector_efficiency[i] =
//                results_de_mult.de_detector_efficiency_res[i];
//            de_mult.de_inner_outer_ring_ratio[i] =
//                results_de_mult.de_inner_outer_ring_ratio_res[i];
//            de_mult.de_relative_fission[i] =
//                results_de_mult.de_relative_fission_res[i];
//        }
//        status = d_keyfind (ANALYSIS_METHOD_DE_MULT,
//            (char *) &analysis_method_de_mult_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &de_mult, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (DE_MULT_REC, (char *) &de_mult, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_ACTIVE_PASSIVE_RESULTS) != 0)
//    {
//        analysis_method_record.active_passive = 1;
//        strcpy (method_active_passive_key.active_passive_item_type,
//            results.results_item_type);
//        strcpy (method_active_passive_key.active_passive_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_ACTIVE_PASSIVE,
//            (char *) &method_active_passive_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &active_passive, CURR_DB);
//        }
//        else
//        {
//            active_passive.ap_lower_mass_limit = -1e8;
//            active_passive.ap_upper_mass_limit = 1e8;
//        }
//        strcpy (active_passive.active_passive_item_type,
//            results.results_item_type);
//        strcpy (active_passive.active_passive_detector_id,
//            results.results_detector_id);
//        active_passive.active_passive_equation =
//            results_active_passive.ap_active_passive_equation;
//        active_passive.ap_a = results_active_passive.ap_a_res;
//        active_passive.ap_b = results_active_passive.ap_b_res;
//        active_passive.ap_c = results_active_passive.ap_c_res;
//        active_passive.ap_d = results_active_passive.ap_d_res;
//        active_passive.ap_var_a = results_active_passive.ap_var_a_res;
//        active_passive.ap_var_b = results_active_passive.ap_var_b_res;
//        active_passive.ap_var_c = results_active_passive.ap_var_c_res;
//        active_passive.ap_var_d = results_active_passive.ap_var_d_res;
//        active_passive.ap_covar_ab = results_active_passive.ap_covar_ab_res;
//        active_passive.ap_covar_ac = results_active_passive.ap_covar_ac_res;
//        active_passive.ap_covar_ad = results_active_passive.ap_covar_ad_res;
//        active_passive.ap_covar_bc = results_active_passive.ap_covar_bc_res;
//        active_passive.ap_covar_bd = results_active_passive.ap_covar_bd_res;
//        active_passive.ap_covar_cd = results_active_passive.ap_covar_cd_res;
//        active_passive.ap_sigma_x = results_active_passive.ap_sigma_x_res;
//        for (i=0; i<NUMBER_AP_SPARES; i++)
//            active_passive.active_passive_spares[i] =
//            results_active_passive.ap_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_ACTIVE_PASSIVE,
//            (char *) &method_active_passive_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &active_passive, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (ACTIVE_PASSIVE_REC, (char *) &active_passive, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_COLLAR_RESULTS) != 0)
//    {
//        /* add this result's item to the collar item id table */
//        d_findfm (COLLAR_DATA_ENTRY_SET, CURR_DB);
//        d_recread ((char *) &collar_data_entry, CURR_DB);
//        found = FALSE;
//        for (i=0; i<MAX_COLLAR_DATA_SETS; i++)
//        {
//            if (!strcmpi (results.item_id,
//                collar_data_entry.col_item_id_entry[i]))
//            {
//                found = TRUE;
//                break;
//            }
//        }
//        if (i >= MAX_COLLAR_DATA_SETS)
//        {
//            for (i=0; i<MAX_COLLAR_DATA_SETS; i++)
//            {
//                if (collar_data_entry.col_item_id_entry[i][0] == '\0')
//                    break;
//            }
//        }
//        if (i < MAX_COLLAR_DATA_SETS)
//        {
//            collar_index = i;
//            if (found == FALSE)
//            {
//                strcpy (collar_data_entry.col_item_id_entry[collar_index],
//                    results.item_id);
//                collar_data_entry.col_total_pu_entry[collar_index] = 0.;
//                collar_data_entry.col_total_pu_err_entry[collar_index] = 0.;
//                collar_data_entry.col_depleted_u_entry[collar_index] = 0.;
//                collar_data_entry.col_depleted_u_err_entry[collar_index] = 0.;
//                collar_data_entry.col_natural_u_entry[collar_index] = 0.;
//                collar_data_entry.col_natural_u_err_entry[collar_index] = 0.;
//                collar_data_entry.col_enriched_u_entry[collar_index] = 0.;
//                collar_data_entry.col_enriched_u_err_entry[collar_index] = 0.;
//            }
//            collar_data_entry.col_length_entry[collar_index] =
//                results_collar.col_dcl_length;
//            collar_data_entry.col_length_err_entry[collar_index] =
//                results_collar.col_dcl_length_err;
//            collar_data_entry.col_total_u235_entry[collar_index] =
//                results_collar.col_dcl_total_u235;
//            collar_data_entry.col_total_u235_err_entry[collar_index] =
//                results_collar.col_dcl_total_u235_err;
//            collar_data_entry.col_total_u238_entry[collar_index] =
//                results_collar.col_dcl_total_u238;
//            collar_data_entry.col_total_u238_err_entry[collar_index] =
//                results_collar.col_dcl_total_u238_err;
//            collar_data_entry.col_total_rods_entry[collar_index] =
//                results_collar.col_dcl_total_rods;
//            collar_data_entry.col_total_poison_rods_entry[collar_index] =
//                results_collar.col_dcl_total_poison_rods;
//            collar_data_entry.col_poison_percent_entry[collar_index] =
//                results_collar.col_dcl_poison_percent;
//            collar_data_entry.col_poison_percent_err_entry[collar_index] =
//                results_collar.col_dcl_poison_percent_err;
//            strcpy (collar_data_entry.col_rod_type_entry[collar_index],
//                results_collar.col_poison_rod_type_res);
//            d_recwrite ((char *) &collar_data_entry, CURR_DB);
//        }
//        else
//            collar_index = MAX_COLLAR_DATA_SETS;

//        analysis_method_record.collar = 1;
//        strcpy (method_collar_key.collar_item_type,
//            results.results_item_type);
//        method_collar_key.collar_mode =	results_collar.col_collar_mode;
//        status = d_keyfind (ANALYSIS_METHOD_COLLAR,
//            (char *) &method_collar_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &collar, CURR_DB);
//        }
//        else
//        {
//            strcpy (collar.collar_item_type, results.results_item_type);
//            collar.collar_mode = results_collar.col_collar_mode;
//            collar.col_lower_mass_limit = -1e8;
//            collar.col_upper_mass_limit = 1e8;
//            for (rod_index=0; rod_index<MAX_POISON_ROD_TYPES; rod_index++)
//            {
//                collar.col_poison_rod_type[rod_index][0] = '\0';
//                collar.col_poison_absorption_fact[rod_index] = GadPAF;
//                collar.col_poison_rod_a[rod_index] = 0.;
//                collar.col_poison_rod_a_err[rod_index] = 0.;
//                collar.col_poison_rod_b[rod_index] = 0.;
//                collar.col_poison_rod_b_err[rod_index] = 0.;
//                collar.col_poison_rod_c[rod_index] = 0.;
//                collar.col_poison_rod_c_err[rod_index] = 0.;
//            }
//        }
//        collar.collar_equation = results_collar.col_collar_equation;
//        collar.col_a = results_collar.col_a_res;
//        collar.col_b = results_collar.col_b_res;
//        collar.col_c = results_collar.col_c_res;
//        collar.col_d = results_collar.col_d_res;
//        collar.col_var_a = results_collar.col_var_a_res;
//        collar.col_var_b = results_collar.col_var_b_res;
//        collar.col_var_c = results_collar.col_var_c_res;
//        collar.col_var_d = results_collar.col_var_d_res;
//        collar.col_covar_ab = results_collar.col_covar_ab_res;
//        collar.col_covar_ac = results_collar.col_covar_ac_res;
//        collar.col_covar_ad = results_collar.col_covar_ad_res;
//        collar.col_covar_bc = results_collar.col_covar_bc_res;
//        collar.col_covar_bd = results_collar.col_covar_bd_res;
//        collar.col_covar_cd = results_collar.col_covar_cd_res;
//        collar.col_sigma_x = results_collar.col_sigma_x_res;
//        collar.col_number_calib_rods = results_collar.col_number_calib_rods_res;
//        if (collar_index < MAX_COLLAR_DATA_SETS)
//        {
//            index = -1;
//            for (rod_index=0; rod_index<MAX_POISON_ROD_TYPES; rod_index++)
//            {
//                if (!strcmpi (collar.col_poison_rod_type[rod_index],
//                    collar_data_entry.col_rod_type_entry[collar_index]))
//                {
//                    index = rod_index;
//                    break;
//                }
//                if ((collar.col_poison_rod_type[rod_index][0] == '\0') &&
//                    (index == -1))
//                {
//                    index = rod_index;
//                }
//            }
//            if (index != -1)
//            {
//                strcpy (collar.col_poison_rod_type[index],
//                    results_collar.col_poison_rod_type_res);
//                collar.col_poison_absorption_fact[index] =
//                    results_collar.col_poison_absorption_fact_res; // jfl ab override
//                collar.col_poison_rod_a[index] =
//                    results_collar.col_poison_rod_a_res;
//                collar.col_poison_rod_a_err[index] =
//                    results_collar.col_poison_rod_a_err_res;
//                collar.col_poison_rod_b[index] =
//                    results_collar.col_poison_rod_b_res;
//                collar.col_poison_rod_b_err[index] =
//                    results_collar.col_poison_rod_b_err_res;
//                collar.col_poison_rod_c[index] =
//                    results_collar.col_poison_rod_c_res;
//                collar.col_poison_rod_c_err[index] =
//                    results_collar.col_poison_rod_c_err_res;
//                collar.col_u_mass_corr_fact_a =
//                    results_collar.col_u_mass_corr_fact_a_res;
//                collar.col_u_mass_corr_fact_a_err =
//                    results_collar.col_u_mass_corr_fact_a_err_res;
//                collar.col_u_mass_corr_fact_b =
//                    results_collar.col_u_mass_corr_fact_b_res;
//                collar.col_u_mass_corr_fact_b_err =
//                    results_collar.col_u_mass_corr_fact_b_err_res;
//                collar.col_sample_corr_fact =
//                    results_collar.col_sample_corr_fact_res;
//                collar.col_sample_corr_fact_err =
//                    results_collar.col_sample_corr_fact_err_res;
//            }
//        }
//        for (i=0; i<NUMBER_COL_SPARES; i++)
//            collar.collar_spares[i] = results_collar.col_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_COLLAR,
//            (char *) &method_collar_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &collar, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (COLLAR_REC, (char *) &collar, CURR_DB);
//        }
//        strcpy (method_collar_detector_key.collar_detector_item_type,
//            results.results_item_type);
//        method_collar_detector_key.collar_detector_mode =
//            results_collar.col_collar_mode;
//        strcpy (method_collar_detector_key.collar_detector_id,
//            results.results_detector_id);
//        strcpy (collar_detector.collar_detector_item_type,
//            results.results_item_type);
//        collar_detector.collar_detector_mode = results_collar.col_collar_mode;
//        strcpy (collar_detector.collar_detector_id,
//            results.results_detector_id);
//        strcpy (collar_detector.col_reference_date,
//            results_collar.col_reference_date_res);
//        collar_detector.col_relative_doubles_rate =
//            results_collar.col_relative_doubles_rate_res;
//        status = d_keyfind (ANALYSIS_METHOD_COLLAR_DETECTOR,
//            (char *) &method_collar_detector_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &collar_detector, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (COLLAR_DETECTOR_REC, (char *) &collar_detector, CURR_DB);
//        }
//        for (i=0; i<MAX_COLLAR_K5_PARAMETERS; i++)
//        {
//            strcpy (collar_k5.collar_k5_label[i],
//                results_collar.collar_k5_label_res[i]);
//            collar_k5.collar_k5_checkbox[i] =
//                results_collar.collar_k5_checkbox_res[i];
//            collar_k5.collar_k5[i] = results_collar.collar_k5_res[i];
//            collar_k5.collar_k5_err[i] = results_collar.collar_k5_err_res[i];
//        }
//        strcpy (collar_k5.collar_k5_item_type, results.results_item_type);
//        collar_k5.collar_k5_mode = results_collar.col_collar_mode;
//        strcpy (method_collar_k5_key.collar_k5_item_type,
//            results.results_item_type);
//        method_collar_k5_key.collar_k5_mode = results_collar.col_collar_mode;
//        status = d_keyfind (ANALYSIS_METHOD_COLLAR_K5,
//            (char *) &method_collar_k5_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &collar_k5, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (COLLAR_K5_REC, (char *) &collar_k5, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_ACTIVE_RESULTS) != 0)
//    {
//        analysis_method_record.active = 1;
//        strcpy (method_active_key.active_item_type,
//            results.results_item_type);
//        strcpy (method_active_key.active_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_ACTIVE,
//            (char *) &method_active_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &active, CURR_DB);
//        }
//        else
//        {
//            active.act_lower_mass_limit = -1e8;
//            active.act_upper_mass_limit = 1e8;
//            for (i=0; i<MAX_NUM_CALIB_PTS; i++)
//            {
//                active.act_dcl_mass[i] = 0.;
//                active.act_doubles[i] = 0.;
//            }
//        }
//        strcpy (active.active_item_type, results.results_item_type);
//        strcpy (active.active_detector_id, results.results_detector_id);
//        active.active_equation = results_active.act_active_equation;
//        active.act_a = results_active.act_a_res;
//        active.act_b = results_active.act_b_res;
//        active.act_c = results_active.act_c_res;
//        active.act_d = results_active.act_d_res;
//        active.act_var_a = results_active.act_var_a_res;
//        active.act_var_b = results_active.act_var_b_res;
//        active.act_var_c = results_active.act_var_c_res;
//        active.act_var_d = results_active.act_var_d_res;
//        active.act_covar_ab = results_active.act_covar_ab_res;
//        active.act_covar_ac = results_active.act_covar_ac_res;
//        active.act_covar_ad = results_active.act_covar_ad_res;
//        active.act_covar_bc = results_active.act_covar_bc_res;
//        active.act_covar_bd = results_active.act_covar_bd_res;
//        active.act_covar_cd = results_active.act_covar_cd_res;
//        active.act_sigma_x = results_active.act_sigma_x_res;
//        for (i=0; i<NUMBER_ACT_SPARES; i++)
//            active.active_spares[i] = results_active.act_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_ACTIVE,
//            (char *) &method_active_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &active, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (ACTIVE_REC, (char *) &active, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_ACTIVE_MULTIPLICITY_RESULTS) != 0)
//    {
//        analysis_method_record.active_mult = 1;
//        strcpy (active_mult.active_mult_item_type, results.results_item_type);
//        strcpy (active_mult.active_mult_detector_id,
//            results.results_detector_id);
//        active_mult.am_vt1 = results_active_mult.am_vt1_res;
//        active_mult.am_vt2 = results_active_mult.am_vt2_res;
//        active_mult.am_vt3 = results_active_mult.am_vt3_res;
//        active_mult.am_vf1 = results_active_mult.am_vf1_res;
//        active_mult.am_vf2 = results_active_mult.am_vf2_res;
//        active_mult.am_vf3 = results_active_mult.am_vf3_res;
//        for (i=0; i<NUMBER_AM_SPARES; i++)
//            results_active_mult.am_spares_res[i] =
//            results_active_mult.am_spares_res[i];
//        strcpy (method_active_mult_key.active_mult_item_type,
//            results.results_item_type);
//        strcpy (method_active_mult_key.active_mult_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_ACTIVE_MULT,
//            (char *) &method_active_mult_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &active_mult, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (ACTIVE_MULT_REC, (char *) &active_mult, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_CURIUM_RATIO_RESULTS) != 0)
//    {
//        analysis_method_record.curium_ratio = 1;
//        strcpy (method_curium_ratio_key.curium_ratio_item_type,
//            results.results_item_type);
//        strcpy (method_curium_ratio_key.curium_ratio_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_CURIUM_RATIO,
//            (char *) &method_curium_ratio_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &curium_ratio, CURR_DB);
//        }
//        else
//        {
//            curium_ratio.cr_lower_mass_limit = -1e8;
//            curium_ratio.cr_upper_mass_limit = 1e8;
//        }
//        cm_pu_ratio.cm_pu_ratio = results_curium_ratio.cr_cm_pu_ratio;
//        cm_pu_ratio.cm_pu_ratio_err = results_curium_ratio.cr_cm_pu_ratio_err;
//        cm_pu_ratio.cm_pu_half_life = results_curium_ratio.cr_pu_half_life;
//        strcpy (cm_pu_ratio.cm_pu_ratio_date,
//            results_curium_ratio.cr_cm_pu_ratio_date);
//        cm_pu_ratio.cm_u_ratio = results_curium_ratio.cr_cm_u_ratio;
//        cm_pu_ratio.cm_u_ratio_err = results_curium_ratio.cr_cm_u_ratio_err;
//        strcpy (cm_pu_ratio.cm_u_ratio_date,
//            results_curium_ratio.cr_cm_u_ratio_date);
//        strcpy (cm_pu_ratio.cm_id_label, results_curium_ratio.cr_cm_id_label);
//        strcpy (cm_pu_ratio.cm_id, results_curium_ratio.cr_cm_id);
//        strcpy (cm_pu_ratio.cm_input_batch_id,
//            results_curium_ratio.cr_cm_input_batch_id);
//        cm_pu_ratio.cm_dcl_u_mass = results_curium_ratio.cr_dcl_u_mass_res;
//        cm_pu_ratio.cm_dcl_u235_mass =
//            results_curium_ratio.cr_dcl_u235_mass_res;
//        d_findfm (CM_PU_RATIO_SET, CURR_DB);
//        d_recwrite ((char *) &cm_pu_ratio, CURR_DB);
//        strcpy (curium_ratio.curium_ratio_item_type, results.results_item_type);
//        strcpy (curium_ratio.curium_ratio_detector_id,
//            results.results_detector_id);
//        curium_ratio.curium_ratio_equation =
//            results_curium_ratio.cr_curium_ratio_equation;
//        curium_ratio.cr_a = results_curium_ratio.cr_a_res;
//        curium_ratio.cr_b = results_curium_ratio.cr_b_res;
//        curium_ratio.cr_c = results_curium_ratio.cr_c_res;
//        curium_ratio.cr_d = results_curium_ratio.cr_d_res;
//        curium_ratio.cr_var_a = results_curium_ratio.cr_var_a_res;
//        curium_ratio.cr_var_b = results_curium_ratio.cr_var_b_res;
//        curium_ratio.cr_var_c = results_curium_ratio.cr_var_c_res;
//        curium_ratio.cr_var_d = results_curium_ratio.cr_var_d_res;
//        curium_ratio.cr_covar_ab = results_curium_ratio.cr_covar_ab_res;
//        curium_ratio.cr_covar_ac = results_curium_ratio.cr_covar_ac_res;
//        curium_ratio.cr_covar_ad = results_curium_ratio.cr_covar_ad_res;
//        curium_ratio.cr_covar_bc = results_curium_ratio.cr_covar_bc_res;
//        curium_ratio.cr_covar_bd = results_curium_ratio.cr_covar_bd_res;
//        curium_ratio.cr_covar_cd = results_curium_ratio.cr_covar_cd_res;
//        curium_ratio.cr_sigma_x = results_curium_ratio.cr_sigma_x_res;
//        curium_ratio.curium_ratio_type =
//            results_curium_ratio.curium_ratio_type_res;
//        for (i=0; i<NUMBER_CR_SPARES; i++)
//            curium_ratio.curium_ratio_spares[i] =
//            results_curium_ratio.cr_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_CURIUM_RATIO,
//            (char *) &method_curium_ratio_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &curium_ratio, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (CURIUM_RATIO_REC, (char *) &curium_ratio, CURR_DB);
//        }
//    }
//    if ((results_status & SAVE_TRUNCATED_MULT_RESULTS) != 0)
//    {
//        analysis_method_record.truncated_mult = 1;
//        strcpy (method_truncated_mult_key.truncated_mult_item_type,
//            results.results_item_type);
//        strcpy (method_truncated_mult_key.truncated_mult_detector_id,
//            results.results_detector_id);
//        strcpy (truncated_mult.truncated_mult_item_type,
//            results.results_item_type);
//        strcpy (truncated_mult.truncated_mult_detector_id,
//            results.results_detector_id);
//        truncated_mult.tm_a = results_truncated_mult.tm_a_res;
//        truncated_mult.tm_b = results_truncated_mult.tm_b_res;
//        truncated_mult.tm_known_eff = results_truncated_mult.tm_known_eff_res;
//        truncated_mult.tm_solve_eff = results_truncated_mult.tm_solve_eff_res;
//        for (i=0; i<NUMBER_TM_SPARES; i++)
//            truncated_mult.truncated_mult_spares[i] =
//            results_truncated_mult.tm_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_TRUNCATED_MULT,
//            (char *) &method_truncated_mult_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &truncated_mult, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (TRUNCATED_MULT_REC, (char *) &truncated_mult, CURR_DB);
//        }
//        tm_bkg_parms.tm_singles_bkg = results_truncated_mult.tm_bkg_singles;
//        tm_bkg_parms.tm_singles_bkg_err =
//            results_truncated_mult.tm_bkg_singles_err;
//        tm_bkg_parms.tm_zeros_bkg = results_truncated_mult.tm_bkg_zeros;
//        tm_bkg_parms.tm_zeros_bkg_err = results_truncated_mult.tm_bkg_zeros_err;
//        tm_bkg_parms.tm_ones_bkg = results_truncated_mult.tm_bkg_ones;
//        tm_bkg_parms.tm_ones_bkg_err = results_truncated_mult.tm_bkg_ones_err;
//        tm_bkg_parms.tm_twos_bkg = results_truncated_mult.tm_bkg_twos;
//        tm_bkg_parms.tm_twos_bkg_err = results_truncated_mult.tm_bkg_twos_err;
//        tm_bkg_parms.tm_bkg = TRUE;
//        d_keyfind (DETECTOR_ID, results.results_detector_id, CURR_DB);
//        d_setor (DETECTOR_TM_BKG_PARMS_SET, CURR_DB);
//        d_findfm (DETECTOR_TM_BKG_PARMS_SET, CURR_DB);
//        d_recwrite ((char *) &tm_bkg_parms, CURR_DB);
//    }
//    if ((results_status & SAVE_ADD_A_SOURCE_RESULTS) != 0)
//    {
//        analysis_method_record.add_a_source = 1;
//        strcpy (method_add_a_source_key.add_a_source_item_type,
//            results.results_item_type);
//        strcpy (method_add_a_source_key.add_a_source_detector_id,
//            results.results_detector_id);
//        status = d_keyfind (ANALYSIS_METHOD_ADD_A_SOURCE,
//            (char *) &method_add_a_source_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recread ((char *) &add_a_source, CURR_DB);
//        }
//        else
//        {
//            add_a_source.ad_lower_mass_limit = -1e8;
//            add_a_source.ad_upper_mass_limit = 1e8;
//            for (i=0; i<MAX_NUM_CALIB_PTS; i++)
//            {
//                add_a_source.ad_dcl_mass[i] = 0.;
//                add_a_source.ad_doubles[i] = 0.;
//            }
//        }
//        strcpy (add_a_source.add_a_source_item_type, results.results_item_type);
//        strcpy (add_a_source.add_a_source_detector_id,
//            results.results_detector_id);
//        add_a_source.add_a_source_equation =
//            results_add_a_source.ad_add_a_source_equation;
//        add_a_source.ad_a = results_add_a_source.ad_a_res;
//        add_a_source.ad_b = results_add_a_source.ad_b_res;
//        add_a_source.ad_c = results_add_a_source.ad_c_res;
//        add_a_source.ad_d = results_add_a_source.ad_d_res;
//        add_a_source.ad_var_a = results_add_a_source.ad_var_a_res;
//        add_a_source.ad_var_b = results_add_a_source.ad_var_b_res;
//        add_a_source.ad_var_c = results_add_a_source.ad_var_c_res;
//        add_a_source.ad_var_d = results_add_a_source.ad_var_d_res;
//        add_a_source.ad_covar_ab = results_add_a_source.ad_covar_ab_res;
//        add_a_source.ad_covar_ac = results_add_a_source.ad_covar_ac_res;
//        add_a_source.ad_covar_ad = results_add_a_source.ad_covar_ad_res;
//        add_a_source.ad_covar_bc = results_add_a_source.ad_covar_bc_res;
//        add_a_source.ad_covar_bd = results_add_a_source.ad_covar_bd_res;
//        add_a_source.ad_covar_cd = results_add_a_source.ad_covar_cd_res;
//        add_a_source.ad_sigma_x = results_add_a_source.ad_sigma_x_res;
//        add_a_source.ad_position_dzero[0] =
//            results_add_a_source.ad_position_dzero_res[0];
//        add_a_source.ad_position_dzero[1] =
//            results_add_a_source.ad_position_dzero_res[1];
//        add_a_source.ad_position_dzero[2] =
//            results_add_a_source.ad_position_dzero_res[2];
//        add_a_source.ad_position_dzero[3] =
//            results_add_a_source.ad_position_dzero_res[3];
//        add_a_source.ad_position_dzero[4] =
//            results_add_a_source.ad_position_dzero_res[4];
//        add_a_source.ad_dzero_avg; results_add_a_source.ad_dzero_avg_res;
//        strcpy (add_a_source.ad_dzero_ref_date,
//            results_add_a_source.ad_dzero_ref_date_res);
//        add_a_source.ad_num_runs = results_add_a_source.ad_num_runs_res;
//        add_a_source.ad_cf_a = results_add_a_source.ad_cf_a_res;
//        add_a_source.ad_cf_b = results_add_a_source.ad_cf_b_res;
//        add_a_source.ad_cf_c = results_add_a_source.ad_cf_c_res;
//        add_a_source.ad_cf_d = results_add_a_source.ad_cf_d_res;
//        add_a_source.ad_use_truncated_mult =
//            results_add_a_source.ad_use_truncated_mult_res;
//        add_a_source.ad_tm_weighting_factor =
//            results_add_a_source.ad_tm_weighting_factor_res;
//        add_a_source.ad_tm_dbls_rate_upper_limit =
//            results_add_a_source.ad_tm_dbls_rate_upper_limit_res;
//        for (i=0; i<NUMBER_AD_SPARES; i++)
//            add_a_source.add_a_source_spares[i] =
//            results_add_a_source.ad_spares_res[i];
//        for (i=0; i<NUMBER_AD_CF_SPARES; i++)
//            add_a_source.add_a_source_cf_spares[i] =
//            results_add_a_source.ad_cf_spares_res[i];
//        status = d_keyfind (ANALYSIS_METHOD_ADD_A_SOURCE,
//            (char *) &method_add_a_source_key, CURR_DB);
//        if (status == S_OKAY)
//        {
//            d_recwrite ((char *) &add_a_source, CURR_DB);
//        }
//        else
//        {
//            d_fillnew (ADD_A_SOURCE_REC, (char *) &add_a_source, CURR_DB);
//        }
//    }

//    d_keyfind (ANALYSIS_METHOD, (char *) &analysis_method_key, CURR_DB);
//    d_recwrite ((char *) &analysis_method_record, CURR_DB);

//    return;

//}


}


    // NCC file partial conversion occur here, final translation to a cycle occur in caller logic
    public class INCCReviewFile : INCCTransferBase
    {

        public string detector, item;
        public List<run_rec_ext> runs;
        public List<run_rec_ext_plus> times;
        public DateTime dt;
        public byte meas_option;
        public UInt16 num_runs;
        public bool skip;

        public struct run_rec_ext_plus
        {
            public DateTime dt;
            public ushort n_mult;
        }

        public INCCReviewFile(LMLoggers.LognLM logger, string mpath)
            : base(logger, mpath)
        {
            runs = new List<run_rec_ext>();
            times = new List<run_rec_ext_plus>();
        }

        unsafe new public bool Restore(string source_path_filename)   // migrated from rd_srrev.cpp
        {
            bool result = false;
            FileStream stream;
            BinaryReader reader;
            FileInfo fi;

            mlogger.TraceEvent(LogLevels.Info, 37100, "Parsing a Radiation Review Measurement Data file {0}", source_path_filename);

            try
            {
                fi = new System.IO.FileInfo(source_path_filename);
                stream = fi.OpenRead();
                reader = new BinaryReader(stream);
            }
            catch (Exception e)
            {
                mlogger.TraceException(e);
                mlogger.TraceEvent(LogLevels.Warning, 37101, "Cannot open file {0}", source_path_filename);
                return result;
            }

            byte[] los_bytos;
            byte[] mark = reader.ReadBytes(4);

            try
            {
                if (TransferUtils.ByteEquals(mark, INCC.INTEGRATED_REVIEW, 4))
                {
                    INCC.integrated_review_data_rec ird = new INCC.integrated_review_data_rec();
                    int integrated_review_data_rec_size = Marshal.SizeOf(ird);  // should be 60
                    meas_option = reader.ReadByte(); // meas option byte v,b,n V,B,N                    
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.MAX_DETECTOR_ID_LENGTH - 1);
                    detector = System.Text.ASCIIEncoding.ASCII.GetString(los_bytos, 0, INCC.MAX_DETECTOR_ID_LENGTH - 1);
                    detector = detector.Trim();
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.MAX_ITEM_ID_LENGTH - 1);
                    item = System.Text.ASCIIEncoding.ASCII.GetString(los_bytos, 0, INCC.MAX_ITEM_ID_LENGTH - 1);
                    item = item.Trim();

                    byte[] meas_date = new byte[9];
                    byte[] meas_time = new byte[9]; 
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.DATE_TIME_LENGTH - 1);
                    Array.Copy(los_bytos, meas_date, INCC.DATE_TIME_LENGTH - 1);
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.DATE_TIME_LENGTH - 1);
                    Array.Copy(los_bytos, meas_time, INCC.DATE_TIME_LENGTH - 1);
                    dt = INCC.DateTimeFrom(
                                    System.Text.ASCIIEncoding.ASCII.GetString(meas_date, 0, INCC.DATE_TIME_LENGTH - 1),
                                    System.Text.ASCIIEncoding.ASCII.GetString(meas_time, 0, INCC.DATE_TIME_LENGTH - 1));

                    num_runs = reader.ReadUInt16();  // runs
                    for (int i = 0; i < num_runs; i++)
                    {
                        INCC.integrated_review_data_rec irdr = new INCC.integrated_review_data_rec();
                        los_bytos = TransferUtils.TryReadBytes(reader, integrated_review_data_rec_size);
                        if (los_bytos != null)
                        {
                            fixed (byte* pData = los_bytos)
                            {
                                irdr = *(INCC.integrated_review_data_rec*)pData;
                            }
                        }
                        else
                        {
                            break;
                        }
                        run_rec_ext rre = new run_rec_ext();
                        run_rec_ext_plus rrep = new run_rec_ext_plus();
                        TransferUtils.Copy(irdr.date, 0, rre.run_date, 0, INCC.DATE_TIME_LENGTH - 1);
                        rre.run_date[8] = 0;
                        TransferUtils.Copy(irdr.time, 0, rre.run_time, 0, INCC.DATE_TIME_LENGTH - 1);
                        rre.run_time[8] = 0;
                        rrep.dt = INCC.DateTimeFrom(TransferUtils.str(rre.run_date, INCC.DATE_TIME_LENGTH), TransferUtils.str(rre.run_time, INCC.DATE_TIME_LENGTH));
                        rre.run_count_time = (double)irdr.meas_time;
                        rre.run_singles = irdr.totals;
                        rre.run_reals_plus_acc = irdr.r_plus_a;
                        rre.run_acc = irdr.a;
                        rre.run_scaler1 = irdr.scaler1;
                        rre.run_scaler2 = irdr.scaler2;
                        rrep.n_mult = irdr.n_mult;
                        UInt32 mult_data;
                        for (int j = 0; j < irdr.n_mult && j < (INCC.SR_MAX_MULT * 2); j++)
                        {
                            mult_data = reader.ReadUInt32();
                            rre.run_mult_reals_plus_acc[j] = (double)mult_data;
                        }
                        for (int j = 0; j < irdr.n_mult && j < (INCC.SR_MAX_MULT * 2); j++)
                        {
                            mult_data = reader.ReadUInt32();
                            rre.run_mult_acc[j] = (double)mult_data;
                        }
                        runs.Add(rre);
                        times.Add(rrep);
                    }
                }
                else if (TransferUtils.ByteEquals(mark, INCC.OLD_REVIEW, 3))
                {
                    INCC.raw_data_rec rd = new INCC.raw_data_rec();
                    int raw_data_rec_size = Marshal.SizeOf(rd); 
                    mlogger.TraceEvent(LogLevels.Error, 33086, "Old Review data file processing unimplemented");

                    ///* read results date and time */
                    //fscanf(fp, "%8s %8s", results.meas_date, results.meas_time);
                    ///* number of good runs */
                    //fread(&results.number_good_runs, sizeof(short), 1, fp);
                }
                else
                {
                    reader.Close();
                }
                result = true;
            }
            catch (TransferUtils.TransferParsingException tpe)
            {
                mlogger.TraceEvent(LogLevels.Warning, 33086, "Review data file processing incomplete", tpe.Message);
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }

            try
            {
                reader.Close();
            }
            catch (Exception e)
            {
                if (mlogger != null) mlogger.TraceException(e);
            }
            return result;
        }
    }



}


