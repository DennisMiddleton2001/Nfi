namespace Nfi;

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

public class NtfsAnalysis
{

        public enum OUTPUT_OPTION
        {
            OptTxt = 1,
            OptHex = 2,
            OptBin = 3
        };


    public delegate void DeligateProgressUpdate(UInt32 Progress);

    public class NTFSVolume
    {
        public string m_VolumeName="";
        
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        extern static Int32 LoadLibrary(string szFilename);

        [DllImport("NtfsEngine.dll")]
        extern static Int32 NtfsOpenVolume(
            [MarshalAs(UnmanagedType.LPWStr)]string szPath,
            Int32 DebugFlags,
            Int32 RelativeSector,
            Int64 NumberSectors,
            Int64 MftStartLcn,
            Int64 Mft2StartLcn,
            Int32 SectorsPerCluster,
            Int32 RawInitFlags);


        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsCloseVolume();

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsReadFrs(UInt64 Frs, bool IncludeChildren, UInt32 OptionOut, [MarshalAs(UnmanagedType.LPWStr)]string szDocumentFilename);

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsReadStream(
            UInt64 Frs, 
            UInt32 Type,
            [MarshalAs(UnmanagedType.LPWStr)]string szAttrName, 
            UInt32 Option, 
            UInt64 Start, 
            Int32 Bytes,
            [MarshalAs(UnmanagedType.LPWStr)]string szOutputFilename);
          
        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsPrintFolder(
            UInt64 FileRecordNumber,
            [MarshalAs(UnmanagedType.LPWStr)]string OutputFilename);

        [DllImport("NtfsEngine.dll")]
       extern static UInt32 NtfsPrintLcnRange(
            UInt64 Lcn, 
            Int32 Length, 
            UInt32 Options, 
            [MarshalAs(UnmanagedType.LPWStr)]string OutputFilename);

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsPrintLbnRange(
            UInt64 Lcn,
            Int32 Length,
            UInt32 Options,
            [MarshalAs(UnmanagedType.LPWStr)]string OutputFilename);

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsGetNamedFile([MarshalAs(UnmanagedType.LPWStr)] out string szFoundFile, ref UInt64 pThisFrs, ref UInt32 pProgress, [MarshalAs(UnmanagedType.LPWStr)]string Extension, bool OnlyDeleted);

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsCrackPath(
            UInt64 Frs,
            [MarshalAs(UnmanagedType.LPWStr)]string OutputFilename);

        [DllImport("NtfsEngine.dll")]
        extern static UInt32 NtfsGetVolumeInformation([MarshalAs(UnmanagedType.LPWStr)]string OutputFilename);

        public long OpenVolume(string VolumeName)
        {
            Int32 i = LoadLibrary("NtfsEngine.dll");
            long Error=NtfsOpenVolume(VolumeName, 0, 0, 0, 0, 0, 0, 0);
            if (Error == 0)
            {
                this.m_VolumeName = VolumeName;
            }
            return Error;
        }

        public UInt32 GetNamedFile(out string szFoundFile, ref UInt64 ThisFrs, ref UInt32 Progress, string Extension, bool OnlyDeleted)
        {
            return NtfsGetNamedFile(out szFoundFile, ref ThisFrs, ref Progress, Extension, OnlyDeleted);
        }
        public long PrintFriendlyIndex(UInt64 FileRecordNumber, string OutputFilename)
        {
            return NtfsPrintFolder(FileRecordNumber, OutputFilename);
        }
        public long OpenVolume(string VolumeName, Int32 DebugFlags, Int32 RelativeSector, Int64 NumberSectors, Int64 MftStartLcn, Int64 Mft2StartLcn, Int32 SectorsPerCluster, Int32 RawInitFlags)
        {
            Int32 i = LoadLibrary("NtfsEngine.dll");
            long Error = NtfsOpenVolume(VolumeName, DebugFlags, RelativeSector, NumberSectors, Mft2StartLcn, 0, SectorsPerCluster, RawInitFlags);
            if (Error == 0)
            {
                this.m_VolumeName = VolumeName;
            }
            return Error;
        }
        
        public long CloseVolume()
        {
            return NtfsCloseVolume();
        }
    
        public long PrintFrs(UInt64 FileRecordNumber, bool IncludeChildren, UInt32 OptionOut, string OutputFilename)
        {
            return NtfsReadFrs(FileRecordNumber, IncludeChildren, OptionOut, OutputFilename);
        }

        public long PrintStream(UInt64 Frs, UInt32 TypeCode, string szAttrName, UInt32 OptionOut, UInt64 StartOffset, Int32 Bytes, string OutputFilename)
        {
            return NtfsReadStream(Frs, TypeCode, szAttrName, OptionOut, StartOffset, Bytes, OutputFilename);
        }

        public long PrintLcnRange(UInt64 Lcn, Int32 Length, UInt32 Options, string OutputFilename)
        {            
            return NtfsPrintLcnRange(Lcn, Length, Options, OutputFilename);
        }

        public long PrintLbnRange(UInt64 Lbn, Int32 Length, UInt32 Options, string OutputFilename)
        {            
            return NtfsPrintLbnRange(Lbn, Length, Options, OutputFilename);
        }
        public long CrackPath(UInt64 Frs, string OutputFilename)
        {
            return NtfsCrackPath(Frs, OutputFilename);
        }
        public long GetVolumeInformation(string OutputFilename)
        {
            return NtfsGetVolumeInformation(OutputFilename);
        }

    }


}


