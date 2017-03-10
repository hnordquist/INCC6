/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

		public override string ToString()
		{
			return detector_id + ", " + item_type + ", " + analysis_method.ToString() + ", "+ extra.ToString();
		}

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

    // todo: identify shared data structures in the subclasses, move them to this parent class. 
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

            mlogger.TraceEvent(LogLevels.Verbose, 33190, "Scanning the detector initial data file {0}", source_path_filename);

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
            string str2 = Encoding.ASCII.GetString(los_bytos, 0, thisread);

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
                mlogger.TraceEvent(LogLevels.Info, 33195, "Transferring detector '{0}' data, from {1}",
                                 TransferUtils.str(detector.detector_id, INCC.MAX_DETECTOR_ID_LENGTH), System.IO.Path.GetFileName(Path));

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
				Path = System.IO.Path.Combine(path, s + ".det");
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

            // returns the method map struct for each individual detector and the material types, if any
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

			public List<KeyValuePair<DetectorMaterialMethod, object>> GetDetectorsWithEntries
			{
				get
				{
					List<KeyValuePair<DetectorMaterialMethod, object>> l = new List<KeyValuePair<DetectorMaterialMethod, object>>();
					foreach (KeyValuePair<DetectorMaterialMethod, object> pair in this)
					{
						if (pair.Key.analysis_method == INCC.COLLAR_DETECTOR_SAVE_RESTORE)
							l.Add(pair);                
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

        public string Name { get; set; }

		unsafe new public bool Restore(string source_path_filename)   // migrated from restore.cpp
		{
			bool result = false;
			FileStream stream;
			BinaryReader reader;
			FileInfo fi;

			mlogger.TraceEvent(LogLevels.Info, 33290, "Scanning the calibration initial data file {0}", System.IO.Path.GetFileName(source_path_filename));
			try
			{
				fi = new System.IO.FileInfo(source_path_filename);
				stream = fi.OpenRead();
				reader = new BinaryReader(stream);
			} catch (Exception e)
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
				for (;;)
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
							fixed (byte* pData = los_bytos)
							{ analysis_method_record = *(analysis_method_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ cal_curve = *(cal_curve_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ known_alpha = *(known_alpha_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ known_m = *(known_m_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ multiplicity = *(multiplicity_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ de_mult = *(de_mult_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ truncated_mult = *(truncated_mult_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ active = *(active_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ active_passive = *(active_passive_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ collar_detector = *(collar_detector_rec*)pData; } else
							throw new TransferUtils.TransferParsingException("collar_detector_rec read failed");
						current = new DetectorMaterialMethod(
							TransferUtils.str(collar_detector.collar_detector_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
							TransferUtils.str(collar_detector.collar_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
						current.extra = (short)(collar_detector.collar_detector_mode == 0 ? 0 : 1);
						DetectorMaterialMethodParameters.Add(current, collar_detector);
						mlogger.TraceEvent(LogLevels.Verbose, 103030, "Step 1 COLLAR_DETECTOR_SAVE_RESTORE {0} {1} {2}", current.detector_id, current.item_type, collar_detector.collar_detector_mode);
						break;
					case INCC.COLLAR_SAVE_RESTORE:
						collar_rec collar = new collar_rec();
						sz = Marshal.SizeOf(collar);
						los_bytos = TransferUtils.TryReadBytes(reader, sz);
						if (los_bytos != null)
							fixed (byte* pData = los_bytos)
							{ collar = *(collar_rec*)pData; } else
							throw new TransferUtils.TransferParsingException("collar_rec read failed");
						current = new DetectorMaterialMethod(
							TransferUtils.str(collar.collar_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
							TransferUtils.str(analysis_method_record.analysis_method_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
						current.extra = (short)(collar.collar_mode == 0 ? 0 : 1);
						DetectorMaterialMethodParameters.Add(current, collar);
						mlogger.TraceEvent(LogLevels.Verbose, 103031, "Step 2 COLLAR_SAVE_RESTORE [{0}] {1} {2}", current.detector_id, current.item_type, collar.collar_mode);
						break;
					case INCC.COLLAR_K5_SAVE_RESTORE: // this is third in the series
						collar_k5_rec collar_k5 = new collar_k5_rec();
						sz = Marshal.SizeOf(collar_k5);
						los_bytos = TransferUtils.TryReadBytes(reader, sz);
						if (los_bytos != null)
							fixed (byte* pData = los_bytos)
							{ collar_k5 = *(collar_k5_rec*)pData; } else
							throw new TransferUtils.TransferParsingException("collar_k5_rec read failed");
						current = new DetectorMaterialMethod(
							TransferUtils.str(collar_k5.collar_k5_item_type, INCC.MAX_ITEM_TYPE_LENGTH),
							TransferUtils.str(analysis_method_record.analysis_method_detector_id, INCC.MAX_DETECTOR_ID_LENGTH), current_analysis_method);
						current.extra = (short)(collar_k5.collar_k5_mode == 0 ? 0 : 1);
						DetectorMaterialMethodParameters.Add(current, collar_k5);
						mlogger.TraceEvent(LogLevels.Verbose, 103031, "Step 3 COLLAR_K5_SAVE_RESTORE [{0}] {1} {2}", current.detector_id, current.item_type, collar_k5.collar_k5_mode);
						break;
					case INCC.METHOD_ADDASRC:
						add_a_source_rec add_a_source = new add_a_source_rec();
						sz = Marshal.SizeOf(add_a_source);
						los_bytos = TransferUtils.TryReadBytes(reader, sz);
						if (los_bytos != null)
							fixed (byte* pData = los_bytos)
							{ add_a_source = *(add_a_source_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ active_mult = *(active_mult_rec*)pData; } else
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
							fixed (byte* pData = los_bytos)
							{ curium_ratio = *(curium_ratio_rec*)pData; } else
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


			} catch (TransferUtils.TransferParsingException tpe)
			{
				mlogger.TraceEvent(LogLevels.Warning, 33086, "Detector data file processing incomplete", tpe.Message);
				result = false;
			} catch (Exception e)
			{
				if (mlogger != null)
					mlogger.TraceException(e);
			}

			try
			{
				reader.Close();
			} catch (Exception e)
			{
				if (mlogger != null)
					mlogger.TraceException(e);
			}
			return result;

		}

		unsafe new public bool Save(string path)
		{
			List<string> l = DetectorMaterialMethodParameters.GetDetectors;
			mlogger.TraceEvent(LogLevels.Verbose, 33154, "{0} calibration set{1} to save off", l.Count, (l.Count != 1 ? "s" : string.Empty));
			int i = 0; int count = 0;
            foreach (string det in l)
			{
				string s = CleansePotentialFilename(det);
				Path = System.IO.Path.Combine(path, s + ".cal");
				FileStream stream = File.Create(Path);
				BinaryWriter bw = new BinaryWriter(stream);
				bw.Write(Encoding.ASCII.GetBytes(INCCFileInfo.CALIBRATION_SAVE_RESTORE));
                IEnumerator miter = DetectorMaterialMethodParameters.GetDetectorMaterialEnumerator(det);
                while (miter.MoveNext())  // for a named detector, for each Material do this:
                {
                    analysis_method_rec rec = (analysis_method_rec)((KeyValuePair<DetectorMaterialMethod, object>)miter.Current).Value;
                    WriteAM(rec, bw); count++;
                    IEnumerator iiter = DetectorMaterialMethodParameters.GetMethodEnumerator(((KeyValuePair<DetectorMaterialMethod, object>)miter.Current).Key.detector_id, (((KeyValuePair<DetectorMaterialMethod, object>)miter.Current).Key.item_type));
                    if (iiter == null)
                        continue;
                    while (iiter.MoveNext())             // todo: doesn't handle weighted multiplicity
                    {
                        int cam = INCCKnew.OldTypeToOldMethodId(iiter.Current);
                        switch (cam)
                        {                
                            case INCC.METHOD_CALCURVE:
                                WriteCalCurve((cal_curve_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_ACTIVE:
                                WriteActiveCalCurve((active_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_AKNOWN:
                                WriteKA((known_alpha_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_MKNOWN:
                                WriteKM((known_m_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_MULT:
                                WriteMul((multiplicity_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.DUAL_ENERGY_MULT_SAVE_RESTORE:
                                WriteDE((de_mult_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_ACTPAS:
                                WriteActivePassive((active_passive_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.COLLAR_DETECTOR_SAVE_RESTORE:
                                WriteCollarDet((collar_detector_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.COLLAR_SAVE_RESTORE:
                                WriteCollar((collar_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.COLLAR_K5_SAVE_RESTORE:
                                WriteCollarK5((collar_k5_rec)iiter.Current, bw); count++;
                                break;
                            // collar detector save restore mode 0
                            //    collar detector
                            // collar detector save restore mode 1
                            //    collar detector
                            // COLLAR_SAVE_RESTORE mode 0
                            //    collar
                            // COLLAR_SAVE_RESTORE mode 1
                            //    collar
                            // COLLAR_K5_SAVE_RESTORE mode 0
                            //    k5 
                            // COLLAR_K5_SAVE_RESTORE mode 1
                            //    k5
                            case INCC.METHOD_ADDASRC:
                                WriteAAS((add_a_source_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_ACTIVE_MULT:
                                WriteActiveMul((active_mult_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_CURIUM_RATIO:
                                WriteCR((curium_ratio_rec)iiter.Current, bw); count++;
                                break;
                            case INCC.METHOD_TRUNCATED_MULT:
                                WriteTRMul((truncated_mult_rec)iiter.Current, bw); count++;
                                break;
                        }
                    }
                }
                bw.Close();
				bw.Dispose();
				mlogger.TraceInformation("{0} calibration parameters, saved to {1}", det, Path);
				i++;
			}
			return count > 0;
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

		unsafe void WriteCalCurve(cal_curve_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(cal_curve_rec);
			cal_curve_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_CALCURVE);
			bw.Write(zb, 0, sz);
        }

		unsafe void WriteKA(known_alpha_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(known_alpha_rec);
			known_alpha_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_AKNOWN);
			bw.Write(zb, 0, sz);
        }
		unsafe void WriteKM(known_m_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(known_m_rec);
			known_m_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_MKNOWN);
			bw.Write(zb, 0, sz);
        }
		unsafe void WriteActiveCalCurve(active_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(active_rec);
			active_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_ACTIVE);
			bw.Write(zb, 0, sz);
        }
        unsafe void WriteDE(de_mult_rec rec, BinaryWriter bw)
        {
            int sz = sizeof(de_mult_rec);
            de_mult_rec p = rec;
            byte* bytes = (byte*)&p;
            byte[] zb = TransferUtils.GetBytes(bytes, sz);
            bw.Write((byte)INCC.DUAL_ENERGY_MULT_SAVE_RESTORE);
            bw.Write(zb, 0, sz);
        }
        unsafe void WriteAAS(add_a_source_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(add_a_source_rec);
			add_a_source_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_ADDASRC);
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

		unsafe void WriteActiveMul(active_mult_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(active_mult_rec);
			active_mult_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_ACTIVE_MULT);
			bw.Write(zb, 0, sz);
        }

		unsafe void WriteActivePassive(active_passive_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(active_passive_rec);
			active_passive_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_ACTPAS);
			bw.Write(zb, 0, sz);
        }

		unsafe void WriteCollar(collar_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(collar_rec);
            collar_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.COLLAR_SAVE_RESTORE);
			bw.Write(zb, 0, sz);
        }
        unsafe void WriteCollarK5(collar_k5_rec rec, BinaryWriter bw)
        {
            int sz = sizeof(collar_k5_rec);
            collar_k5_rec p = rec;
            byte* bytes = (byte*)&p;
            byte[] zb = TransferUtils.GetBytes(bytes, sz);
            bw.Write((byte)INCC.COLLAR_K5_SAVE_RESTORE);
            bw.Write(zb, 0, sz);
        }
        unsafe void WriteCollarDet(collar_detector_rec rec, BinaryWriter bw)
        {
            int sz = sizeof(collar_detector_rec);
            collar_detector_rec p = rec;
            byte* bytes = (byte*)&p;
            byte[] zb = TransferUtils.GetBytes(bytes, sz);
            bw.Write((byte)INCC.COLLAR_DETECTOR_SAVE_RESTORE);
            bw.Write(zb, 0, sz);
        }
        unsafe void WriteCR(curium_ratio_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(curium_ratio_rec);
			curium_ratio_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_CURIUM_RATIO);
			bw.Write(zb, 0, sz);
        }
    	unsafe void WriteTRMul(truncated_mult_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(truncated_mult_rec);
			truncated_mult_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write((byte)INCC.METHOD_TRUNCATED_MULT);
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

		public string Name { set; get; }

		unsafe new public bool Save(string path)  
		{
			Path = System.IO.Path.Combine(path, Name);
			mlogger.TraceEvent(LogLevels.Verbose, 33154, "Saving measurement to " + Path);

			bool result = false;
			FileStream stream = null;
			BinaryWriter bw = null;

			try
			{
				stream = File.Create(Path);
				bw = new BinaryWriter(stream);
			} catch (Exception e)
			{
				mlogger.TraceException(e);
				mlogger.TraceEvent(LogLevels.Warning, 33084, "Cannot create file {0}", Path);
				return result;
			}

			try
			{
				WriteResultsRec(results_rec_list[0], bw);
				WriteResultsStatus(results_status_list[0], bw);
                foreach (iresultsbase irb in method_results_list)
                {
                    WriteMethodResults(irb, bw); 
                }
				bw.Write((ushort)run_rec_list.Count);
				foreach(run_rec r in run_rec_list)
				{
					WriteRunRec(r, bw);
				}
				if (CFrun_rec_list != null)
				{
					foreach(List<run_rec> cfrrl in CFrun_rec_list)
					{
						bw.Write((ushort)cfrrl.Count);
						foreach(run_rec r in cfrrl)
						{
							WriteRunRec(r, bw);
						}
					}
				}
				// devnote: not doing the WMV stuff at the end of the file
                result = true;
				mlogger.TraceInformation("Saved transfer file " + Path);
			} catch (Exception e)
			{
				if (mlogger != null)
					mlogger.TraceException(e);
				result = false;
			}
			try
			{
				bw.Close();
			} catch (Exception e)
			{
				if (mlogger != null)
					mlogger.TraceException(e);
				result = false;
			}
			return result;
		}

		unsafe void WriteResultsRec(results_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(results_rec);
			results_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
        }
		unsafe void WriteResultsStatus(INCC.SaveResultsMask rec, BinaryWriter bw)
		{
			bw.Write((short)rec);
        }

		unsafe void WriteRunRec(run_rec rec, BinaryWriter bw)
		{
			int sz = sizeof(run_rec);
			run_rec p = rec;
			byte* bytes = (byte*)&p;
			byte[] zb = TransferUtils.GetBytes(bytes, sz);	
			bw.Write(zb, 0, sz);
		}
        unsafe void WriteMethodResults(iresultsbase r, BinaryWriter bw)
        {
            int sz = 0;
            byte* bytes = null;
            byte[] zb = null;

            if (r is results_init_src_rec)
            {
                sz = sizeof(results_init_src_rec);
                results_init_src_rec rec = (results_init_src_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_bias_rec)
            {
                sz = sizeof(results_bias_rec);
                results_bias_rec rec = (results_bias_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_precision_rec)
            {
                sz = sizeof(results_precision_rec);
                results_precision_rec rec = (results_precision_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_cal_curve_rec)
            {
                sz = sizeof(results_cal_curve_rec);
                results_cal_curve_rec rec = (results_cal_curve_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_known_alpha_rec)
            {
                sz = sizeof(results_known_alpha_rec);
                results_known_alpha_rec rec = (results_known_alpha_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_multiplicity_rec)
            {
                sz = sizeof(results_multiplicity_rec);
                results_multiplicity_rec rec = (results_multiplicity_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_truncated_mult_rec)
            {
                sz = sizeof(results_truncated_mult_rec);
                results_truncated_mult_rec rec = (results_truncated_mult_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_known_m_rec)
            {
                sz = sizeof(results_known_m_rec);
                results_known_m_rec rec = (results_known_m_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_add_a_source_rec)
            {
                sz = sizeof(results_add_a_source_rec);
                results_add_a_source_rec rec = (results_add_a_source_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_curium_ratio_rec)
            {
                sz = sizeof(results_curium_ratio_rec);
                results_curium_ratio_rec rec = (results_curium_ratio_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_active_passive_rec)
            {
                sz = sizeof(results_active_passive_rec);
                results_active_passive_rec rec = (results_active_passive_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_active_mult_rec)
            {
                sz = sizeof(results_active_mult_rec);
                results_active_mult_rec rec = (results_active_mult_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_collar_rec)
            {
                sz = sizeof(results_collar_rec);
                results_collar_rec rec = (results_collar_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_de_mult_rec)
            {
                sz = sizeof(results_de_mult_rec);
                results_de_mult_rec rec = (results_de_mult_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            else if (r is results_tm_bkg_rec)
            {
                sz = sizeof(results_tm_bkg_rec);
                results_tm_bkg_rec rec = (results_tm_bkg_rec)r;
                bytes = (byte*)&rec;
                zb = TransferUtils.GetBytes(bytes, sz);
            }
            if (sz > 0 && zb != null)
                bw.Write(zb, 0, sz);
        }

        unsafe new public bool Restore(string source_path_filename)   // migrated from restore.cpp
        {
            bool result = false;
            FileStream stream;
            BinaryReader reader;
            FileInfo fi;
			ushort n, number_runs;
            results_multiplicity_rec results_multiplicity;
            INCC.SaveResultsMask results_status;
            item_id_entry_rec item_id_entry;

            mlogger.TraceEvent(LogLevels.Verbose, 33090, "Scanning the measurement transfer file {0}", source_path_filename);

            results_rec results = new results_rec();
            meas_id id = new meas_id();
            try
            {
				fi = new FileInfo(source_path_filename);
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
                // devnote: consider implementing this if old data still is of interest: convert_results (old_results, &results);
                mlogger.TraceEvent(LogLevels.Warning, 33094, "Cannot use file {0}, not a version 5 result", source_path_filename);
                return result;
            }

            try
            {

                TransferUtils.Copy(results.meas_date, 0, id.meas_date, 0, INCC.DATE_TIME_LENGTH);
                TransferUtils.Copy(results.meas_time, 0, id.meas_time, 0, INCC.DATE_TIME_LENGTH);
                TransferUtils.Copy(results.filename, 0, id.filename, 0, INCC.FILE_NAME_LENGTH);
                TransferUtils.Copy(results.results_detector_id, 0, id.results_detector_id, 0, INCC.MAX_DETECTOR_ID_LENGTH);

                string fname = TransferUtils.str(id.filename, INCC.FILE_NAME_LENGTH);
                mlogger.TraceEvent(LogLevels.Info, 33095, "Transferring {0} measurement for {1} {2}, from {3}",
                                            System.IO.Path.GetExtension(fname),
                                            TransferUtils.str(id.results_detector_id, INCC.MAX_DETECTOR_ID_LENGTH),
                                            INCC.DateTimeFrom(TransferUtils.str(id.meas_date, INCC.DATE_TIME_LENGTH),
                                            TransferUtils.str(id.meas_time, INCC.DATE_TIME_LENGTH)).ToString("yy.MM.dd HH:mm:ss K"), // IAEA format
                                             fname);

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
                results_status_list.Add(results_status);

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
                    mlogger.TraceEvent(LogLevels.Verbose, 33097, "Converting {0} INCC runs into cycles", number_runs);
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

					InitCFRunLists();
                    for (int j = 0; j < INCC.MAX_ADDASRC_POSITIONS; j++)
                    {
                        number_runs = TransferUtils.ReadUInt16(reader, "number of AAS CF" + (j + 1).ToString() + " runs");
                        mlogger.TraceEvent(LogLevels.Verbose, 33097, "Converting {0} AAS CF{1} INCC runs into cycles", number_runs, (j + 1));
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
        public List<INCC.SaveResultsMask> results_status_list = new List<INCC.SaveResultsMask>();
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

		public void InitCFRunLists()
		{
            CFrun_rec_list = new List<run_rec>[INCC.MAX_ADDASRC_POSITIONS+1];
            for (int jj = 0; jj <= INCC.MAX_ADDASRC_POSITIONS; jj++)       
                CFrun_rec_list[jj] = new List<run_rec>();
		}

		unsafe void restore_add_detector(results_rec results)
		{

			// stratum_id_names_rec stratum_id_names;
			detector_rec detector;

			// add results.results_item_type to item_type_names.item_type_names table
			item_type_names_table.Add(TransferUtils.str(results.results_item_type, INCC.MAX_ITEM_TYPE_LENGTH));

			// check for empty string on these descriptors
			DescriptorPair desc = new DescriptorPair();
			desc.id = TransferUtils.str(results.stratum_id, INCC.MAX_STRATUM_ID_LENGTH);
			desc.desc = TransferUtils.str(results.stratum_id_description, INCC.DESCRIPTION_LENGTH);
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
	}


    // NCC file partial conversion occur here, final translation to a cycle occur in caller logic
    public class INCCReviewFile : INCCTransferBase
    {

        public string detector, item;
        public List<run_rec_ext> runs;
        public List<run_rec_ext_plus> times;
        public DateTime dt;
        public byte meas_option;
        public ushort num_runs;
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

            mlogger.TraceEvent(LogLevels.Verbose, 37100, "Scanning the Radiation Review Measurement Data file {0}", source_path_filename);

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
                    detector = Encoding.ASCII.GetString(los_bytos, 0, INCC.MAX_DETECTOR_ID_LENGTH - 1);
                    detector = detector.Trim();
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.MAX_ITEM_ID_LENGTH - 1);
                    item = Encoding.ASCII.GetString(los_bytos, 0, INCC.MAX_ITEM_ID_LENGTH - 1);
                    item = item.Trim();

                    byte[] meas_date = new byte[9];
                    byte[] meas_time = new byte[9]; 
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.DATE_TIME_LENGTH - 1);
                    Array.Copy(los_bytos, meas_date, INCC.DATE_TIME_LENGTH - 1);
                    los_bytos = TransferUtils.TryReadBytes(reader, INCC.DATE_TIME_LENGTH - 1);
                    Array.Copy(los_bytos, meas_time, INCC.DATE_TIME_LENGTH - 1);
                    dt = INCC.DateTimeFrom(
									Encoding.ASCII.GetString(meas_date, 0, INCC.DATE_TIME_LENGTH - 1),
									Encoding.ASCII.GetString(meas_time, 0, INCC.DATE_TIME_LENGTH - 1));

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
                        rre.run_count_time = irdr.meas_time;
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
                            rre.run_mult_reals_plus_acc[j] = mult_data;
                        }
                        for (int j = 0; j < irdr.n_mult && j < (INCC.SR_MAX_MULT * 2); j++)
                        {
                            mult_data = reader.ReadUInt32();
                            rre.run_mult_acc[j] = mult_data;
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


