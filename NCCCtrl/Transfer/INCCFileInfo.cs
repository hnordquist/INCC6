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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NCCReporter;


/// <summary>
/// Support for binary compatibilty with INCC5 transfer/init data files
/// </summary>
namespace NCCTransfer
{
    public enum eFileType { eUnknown, eInitialDataDetector, eInitialDataCalibration, eTransfer, eNCC, eOldNCC, eZip, eFolder, eFileList }


    public partial class INCCFileInfo
    {


        protected string mpath;
        protected eFileType mft;
        protected LMLoggers.LognLM mlogger;

        public static readonly string INTEGRATED_REVIEW =  "IREV";
        public static readonly string OLD_REVIEW = "RAW";

        static public string DETECTOR_SAVE_RESTORE = "DETECT";	/* id for detector save/restore files */
        static public string CALIBRATION_SAVE_RESTORE = "CAL  "; /* id for calibration save/restore files */
        public static string[] Extensions = new string[] {".BKG",		/* background measurement file extension */
                                                ".INS",		/* initial source file extension */
                                                 ".NOR",			/* bias measurement file extension */
                                                ".PRE",		/* precision measurement file extension */
                                                 ".VER"	,	/* verification measurement file extension */
                                                 ".RTS"	,	/* rates only measurement file extension */
                                                 ".CAL",		/* calibration measurement file extension */
                                                ".HUP",		/* holdup measurement file extension */
                                                ".CAL",	/* calibration parameter file extension */
                                                ".NCC",	/* import from radiation review file extension */
                                                 ".NOP",	/* import from operator review file extension */
                                                 ".COP", /* import curium ratio from operator review file extension */
                                                ".VOL"		/* high voltage plateau file extension */
                                                    };

        public enum INCCFileExt
        {
            BACKGROUND_EXT,
            INIT_SRC_EXT,
            BIAS_EXT,
            PRECISION_EXT,
            ASSAY_EXT,
            RATES_ONLY_EXT,
            CALIBRATION_EXT,
            HOLDUP_EXT,
            CALIB_PARAMETER_EXT,
            RADIATION_REV_IMPORT_EXT,
            OPERATOR_REV_IMPORT_EXT,
            OP_REV_IMPORT_CM_RATIO_EXT,
            HIGH_VOLTAGE_EXT
        }

        public INCCFileInfo(LMLoggers.LognLM logger)
        {
            mlogger = logger;
        }

        public void SetFilePath(string path)
        {
            mpath = String.Copy(path);
            mft = DetermineFileType(path);
        }

        public string GetFilePath() { return mpath; }
        public eFileType GetFileType() { return mft; }
        public bool IsINCCFile() { return !(mft == eFileType.eUnknown || mft == eFileType.eZip || mft == eFileType.eFolder); }

        public virtual INCCTransferBase Restore()
        { // current file

            INCCTransferBase result = null;
            switch (mft)
            {
                case eFileType.eInitialDataDetector:
                    {
                        INCCInitialDataDetectorFile idd = new INCCInitialDataDetectorFile(mlogger, mpath);
                        bool good = idd.Restore(mpath);
                        if (good)
                            result = idd; // this now has the structs ready
                    }
                    break;
                case eFileType.eInitialDataCalibration:
                    {
                        INCCInitialDataCalibrationFile idcal = new INCCInitialDataCalibrationFile(mlogger, mpath);
                        bool good = idcal.Restore(mpath);
                        if (good)
                            result = idcal; // this now has the structs ready
                    }
                    break;
                case eFileType.eTransfer:
                    {
                        INCCTransferFile idt = new INCCTransferFile(mlogger, mpath);
                        bool good = idt.Restore(mpath);
                        if (good)
                            result = idt; // this now has the structs ready
                    }
                    break;
                case eFileType.eNCC:
                    {
                        INCCReviewFile irf = new INCCReviewFile(mlogger, mpath);
                        bool good = irf.Restore(mpath);
                        if (good)
                            result = irf; // this now has the structs ready
                    }
                    break;
                case eFileType.eUnknown:
                    mlogger.TraceEvent(LogLevels.Info, 34007, "Skipping {0}", mpath);
                    break;
                default:  // for future expansion
                    // dev note: NOP and COP files are just CSV or Tab-delimited text files, so no binary matching is needed, so I am moving the handling of these to another class (OPfiles)
                    mlogger.TraceEvent(LogLevels.Info, 34008, "Not processing the unsupported file {0}", mpath);
                    break;
            };

            return result;
        }
        public virtual bool Save() { return false; } // NEXT: implement INCC5 File > Save As/Export > Transfer and Initial Data INCC5 MEDIUM #97 

        unsafe protected eFileType DetermineFileType(string source_path_filename)
        {
            eFileType result = eFileType.eUnknown;
            mlogger.TraceEvent(LogLevels.Verbose, 33001, "Determining file type of {0}", source_path_filename);
            if (!File.Exists(source_path_filename)) // file is not there or no permissions
            {
                mlogger.TraceEvent(LogLevels.Warning, 33002, "Cannot access file {0}", source_path_filename);
                return result;
            }

            FileStream stream;
            BinaryReader reader;
            byte[] buff;
            FileInfo fi;

            try
            {
                stream = File.OpenRead(source_path_filename);
                reader = new BinaryReader(stream);
                buff = new byte[stream.Length];
                fi = new System.IO.FileInfo(source_path_filename);
            }
            catch (Exception e)
            {
                mlogger.TraceException(e);
                return result;
            }

            if ((fi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed ||
                fi.Extension.ToLower().Equals(".zip") | fi.Extension.ToLower().Equals(".zipx"))
            {
                result = eFileType.eZip;
                mlogger.TraceEvent(LogLevels.Warning, 33039, "Compressed archive use is unavailable today {0}", source_path_filename);
                reader.Close(); 
                return result;
            }

            int thisread = 0;
            string str2, str2a,str2b;
            if (stream.Length < CALIBRATION_SAVE_RESTORE.Length)
            {
                mlogger.TraceEvent(LogLevels.Info, 33003, "Skipping this tiny file");
                reader.Close();
                return result;
            }
            else if (stream.Length >= DETECTOR_SAVE_RESTORE.Length)
            {
                thisread = reader.Read(buff, 0, DETECTOR_SAVE_RESTORE.Length);  // cannot throw due to length check under normal circumstances, so this is ok 
                str2 = System.Text.ASCIIEncoding.ASCII.GetString(buff, 0, thisread);
                stream.Seek(0, SeekOrigin.Begin);
                thisread = reader.Read(buff, 0, INTEGRATED_REVIEW.Length);  
                str2a = System.Text.ASCIIEncoding.ASCII.GetString(buff, 0, thisread);
                stream.Seek(0, SeekOrigin.Begin);
                thisread = reader.Read(buff, 0, OLD_REVIEW.Length); 
                str2b = System.Text.ASCIIEncoding.ASCII.GetString(buff, 0, thisread);
            }
            else
            {
                mlogger.TraceEvent(LogLevels.Info, 33004, "Skipping this small file");
                reader.Close();
                return result;
            }

            if (str2.Equals(DETECTOR_SAVE_RESTORE))
            {
                result = eFileType.eInitialDataDetector;
                mlogger.TraceEvent(LogLevels.Info, 33009, "The file is an initial data file with detector parameters");
            }
            else if (str2a.Equals(INTEGRATED_REVIEW))
            {
                result = eFileType.eNCC;
                mlogger.TraceEvent(LogLevels.Info, 33009, "The file is a Radiation Review data file");
            }
            else if (str2b.Equals(OLD_REVIEW))
            {
                result = eFileType.eOldNCC;
                mlogger.TraceEvent(LogLevels.Info, 33009, "The file is an olde-style Radiation Review data file");
            }
            else
            {
                mlogger.TraceEvent(LogLevels.Verbose, 33010, "The file is not an initial data file with detector parameters, or an NCC file");
                stream.Seek(0, SeekOrigin.Begin);
                bool found = false;
                foreach (INCCFileExt fe in System.Enum.GetValues(typeof(INCCFileExt)))
                {
                    if (fe < INCCFileExt.CALIB_PARAMETER_EXT
                        && Extensions[(int)fe] == (fi.Extension.ToUpper()))
                    {
                        found = true;
                    }
                }
                if (!found)
                    mlogger.TraceEvent(LogLevels.Verbose, 33011, "The file is not an initial data calibration or transfer file, suffix mismatch");
                else
                {
                    bool calSuffix = false;
                    calSuffix = Extensions[(int)INCCFileExt.CALIBRATION_EXT].Equals(fi.Extension.ToUpper());
                    if (calSuffix)
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 33012, "The file may be an initial data calibration or transfer file");
                        thisread = reader.Read(buff, 0, CALIBRATION_SAVE_RESTORE.Length);
                        if (thisread > 0)
                        {
                            str2 = System.Text.ASCIIEncoding.ASCII.GetString(buff, 0, thisread); // emtpy string result should be ok here
                            if (str2.Equals(CALIBRATION_SAVE_RESTORE))
                            {
                                result = eFileType.eInitialDataCalibration;
                                mlogger.TraceEvent(LogLevels.Info, 33013, "The file is an initial data calibration file");
                            }
                        }
                    }
                    else
                        mlogger.TraceEvent(LogLevels.Verbose, 33014, "The file may be a transfer file");


                    if (result == eFileType.eUnknown) // check for transfer file now
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        results_rec results = new results_rec();

                        double db_version = 5.0;
                        int sz = Marshal.SizeOf(results);
                        byte[] los_bytos = TransferUtils.TryReadBytes(reader, sz);
                        if (los_bytos != null)
                            fixed (byte* pData = los_bytos)
                            {
                                results = *(results_rec*)pData;
                            }
                        else
                        {
                            mlogger.TraceEvent(LogLevels.Warning, 33096, "Results not read", source_path_filename);
                            reader.Close(); 
                            return result;
                        }

                        if (results.db_version != db_version)
                        {
                            old_results_rec old_results = new old_results_rec();
                            sz = Marshal.SizeOf(old_results);
                            stream.Seek(0, SeekOrigin.Begin);
                            los_bytos = TransferUtils.TryReadBytes(reader, sz);
                            if (los_bytos != null)
                            {
                                mlogger.TraceEvent(LogLevels.Verbose, 33015, "The file may be an older transfer file, from an earlier INCC version");
                                mlogger.TraceEvent(LogLevels.Warning, 33016, "Cannot use file {0}, not a version 5 result", source_path_filename);
                            }
                            else
                            {
                                mlogger.TraceEvent(LogLevels.Info, 33017, ("The file is not an older transfer file"));
                            }
                            reader.Close(); 
                            return result;
                        }
                        else
                            mlogger.TraceEvent(LogLevels.Verbose, 33018, "The file may be a current INCC transfer file");

                        string[] nums;
                        stream.Seek(0, SeekOrigin.Begin);
                        thisread = reader.Read(buff, 0, 9);  // gotta be 9 here, I think the previous results read guarantees that
                        str2 = System.Text.ASCIIEncoding.ASCII.GetString(buff, 0, thisread);
                        nums = str2.Split(new char[] { '.' }, 3);
                        // if the first 9 bytes are a date "11.08.05\0"  followed by a null, then we likely have one
                        // todo: make a better test, use the DateTimeFrom, it checks more bytes, and do a file suffix check for the meas type too
                        if ((buff[8] == 0x0) && (nums.Length == 3))
                        {
                            result = eFileType.eTransfer;
                            mlogger.TraceEvent(LogLevels.Verbose, 33019, "The file is a likely a transfer file");
                        }
                        else
                            mlogger.TraceEvent(LogLevels.Verbose, 33020, "The file is not a transfer file");
                    }  // transfer file content check
                }  // transfer file or ini data cal content check
            } // transfer file or ini data cal suffix check

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

    public class INCCFileOrFolderInfo : INCCFileInfo
    {

        public INCCFileOrFolderInfo(LMLoggers.LognLM logger, string searchPattern = "")
            : base(logger)
        {
            this.searchPattern = searchPattern;
        }

        public void SetPath(string path)  // zip or folder
        {
            fpath = String.Copy(path);
            mft = DetermineFolderStateType(path);
            if (!(IsZip() || IsFolder()))
            {
                base.SetFilePath(path);
            }
        }

        public void SetFileList(List<string> paths) 
        {
            if (paths.Count() < 1)
                return;
            fpath = String.Copy(paths[0]);
            this.paths = paths;
            mft = eFileType.eFileList;
        }
        List<string> paths = null;
        string searchPattern;

        public string GetPath() { return fpath; }

        public bool IsZip() { return (mft == eFileType.eZip); }  // zip path?
        public bool IsFolder() { return (mft == eFileType.eFolder); }  // folder path?
        public bool IsPotentialINCCFile() { return !(IsZip() || IsFolder()); }
        public new virtual List<INCCTransferBase> Restore()
        { // folder and file(s)
            List<INCCTransferBase> results = new List<INCCTransferBase>();
            if (IsZip() || IsFolder())
            {
                // do the folder or zip extract, then individual restore
                mlogger.TraceEvent(LogLevels.Info, 33020, "Use the folder content");
                if (IsFolder())
                {
                    IEnumerable<string> effs = null;
                    effs = from f in
                               (String.IsNullOrEmpty(searchPattern) ? Directory.EnumerateFiles(fpath) : Directory.EnumerateFiles(fpath, searchPattern))
                           select f;

                    if (effs == null || (effs.Count() <= 0))
                    {
                        mlogger.TraceEvent(LogLevels.Info, 33021, "No files found in {0}, see ya . . .", fpath);
                    }

                    mlogger.TraceEvent(LogLevels.Info, 33022, "Examining {0} files in {1} for INCC file processing", effs.Count(), fpath);

                    // Show files and build list
                    foreach (var f in effs)
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 33023, "  {0}", f.Substring(f.LastIndexOf("\\") + 1));// Remove path information from string.

                        base.SetFilePath(f);
                        INCCTransferBase itf = base.Restore();
                        if (itf == null)
                        {
                            mlogger.TraceEvent(LogLevels.Verbose, 33024, "Unable to restore file {0}", f);
                        }
                        else
                        {
                            results.Add(itf);//mlogger.TraceInformation("  R file %.256s", fullfilepath);
                        }
                    }
                    return results;
                }
                else if (IsZip())
                {
                    mlogger.TraceEvent(LogLevels.Warning, 33025, "Unable to restore compressed file {0}", fpath);
                }
                return results;
            }
            else if (mft == eFileType.eFileList)
            {
                mlogger.TraceEvent(LogLevels.Info, 33022, "Examining {0} files for INCC file processing", paths.Count());

                // Show files and build list
                foreach (var f in paths)
                {
                    mlogger.TraceEvent(LogLevels.Verbose, 33023, "  {0}", f.Substring(f.LastIndexOf("\\") + 1));// Remove path information from string.

                    base.SetFilePath(f);
                    INCCTransferBase itf = base.Restore();
                    if (itf == null)
                    {
                        mlogger.TraceEvent(LogLevels.Verbose, 33024, "Unable to restore file {0}", f);
                    }
                    else
                    {
                        results.Add(itf);
                    }
                }
            }
            else  // 1 lonely file
            {
                INCCTransferBase itf = base.Restore();
                if (itf == null)
                {
                    mlogger.TraceEvent(LogLevels.Verbose, 33026, "Unable to restore file {0}", base.GetFilePath());
                }
                else
                    results.Add(itf);
            }
            return results;
        }

        public new virtual bool Save() // folder and file(s)
        {
            if (IsZip() || IsFolder())
            {
                // do the folder or zip extract, then individual restore
                return false;
            }
            else
                return base.Save();
        }

        public eFileType DetermineFolderStateType(string source_path_filename)
        {
            eFileType result = eFileType.eUnknown;

            mlogger.TraceEvent(LogLevels.Verbose, 33027, "Determining state of {0}", source_path_filename);

            bool isdir = Directory.Exists(source_path_filename);
            bool isfile = File.Exists(source_path_filename);
            if (!isdir && !isfile) // folder is not there or no permissions
            {
                mlogger.TraceEvent(LogLevels.Warning, 33028, "Cannot access {0}", source_path_filename);
                return result;
            }
            if (isdir)
            {
                mlogger.TraceEvent(LogLevels.Info, 33029, "Folder found");
                result = eFileType.eFolder;
            }
            else
            {
                System.IO.FileInfo fi = null;

                fi = new System.IO.FileInfo(source_path_filename);
                if ((fi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed || fi.Extension.ToLower().Equals(".zip"))
                {
                    result = eFileType.eZip;
                    mlogger.TraceEvent(LogLevels.Info, 33030, "Compressed archive found");
                    // dev note:  System.IO.Compression.DeflateStream does not provide for zipped folders, only zipped single files (the lame losers!), so need to find a full implementation of zip/unzip and add it here
                }
            }
            return result;

        }

        string fpath;

    }
}



