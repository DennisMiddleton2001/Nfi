using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Nfi
{
    internal class C_NtfsInfo
    {
        static void LaunchSecinspet(string arguments)
        {                        
            Process MyProcess = new Process();
            MyProcess.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\secinspect.exe";
            MyProcess.StartInfo.Arguments = arguments;
            MyProcess.Start();
            MyProcess.WaitForExit();
        }

        static void PrintUsage()
        {
            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\NfiReadme.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);

            }
        }

        static void Main(string[] args)
        {
            if (args.Length > 0 && string.Equals(args[0].ToUpper(), "-DISK"))
            {            
                if (args.Length > 1)
                {
                    LaunchSecinspet(args[1]);
                }
                else{
                    LaunchSecinspet("");
                }
                return;
            }

        NtfsAnalysis.NTFSVolume v = new();
        string outputName = ".\\tmp.txt";
        File.Delete(outputName);
        
        int j = 0;

        Console.WriteLine("APP:       NFI.EXE");
        Console.WriteLine("ArgCount:  " + args.Length);
        foreach (var i in args)
        {            
            Console.WriteLine("ARG[" + j + "]:  " + i);
            j++;
        }
        Console.WriteLine("=============================");
        Console.WriteLine();

        try {   
            if (args.Length > 0)
                {
                    
                    long Err = v.OpenVolume(args[1].ToString(),2,0,0,0,0,0,0);
                    
                    if (Err > 0)
                    {
                        throw new ArgumentException("Unable to open volume " + args[1] + ".  It is either not NTFS, or you do not have permission.");
                    }
                }
                
                if(string.Equals(args[0].ToUpper(),"-FRS"))
                {        
                    
                    v.PrintFrs(ulong.Parse(args[2],System.Globalization.NumberStyles.HexNumber), true, (uint)NtfsAnalysis.OUTPUT_OPTION.OptTxt, outputName);
                }

                else if(string.Equals(args[0].ToUpper(),"-FRSH"))
                {                    
                    v.PrintFrs(ulong.Parse(args[2],System.Globalization.NumberStyles.HexNumber), true, (uint)NtfsAnalysis.OUTPUT_OPTION.OptHex, outputName);
                }                
                else if (string.Equals(args[0].ToUpper(), "-DIR"))
                {
                    v.PrintFriendlyIndex(ulong.Parse(args[2], System.Globalization.NumberStyles.HexNumber), outputName);      
                }
                else if (string.Equals(args[0].ToUpper(), "-VOL"))
                {
                    v.GetVolumeInformation(outputName);
                }
                else if (string.Equals(args[0].ToUpper(), "-ATTR") ||
                        string.Equals(args[0].ToUpper(), "-ATTRH"))
                {
                    ulong frsNumber = ulong.Parse(args[2],System.Globalization.NumberStyles.HexNumber);
                    uint streamType = uint.Parse(args[3],System.Globalization.NumberStyles.HexNumber);
                    string streamName = args[4].ToUpper();
                    
                    if (streamType == 0x80 && frsNumber == 0x6)
                    {
                        if (frsNumber == 0x6)
                        {
                            streamName = "";
                        }
                        else
                        {
                            streamName = "$" + streamName;
                        }
                    }
                    else if (args.Length == 5)
                    {
                        if ( frsNumber !=0x6)
                        {
                            streamName = "" + streamName;
                        }
                     }

                     //Console.WriteLine("FRS:" + frsNumber + " TYP: " + streamType + " ATTR: " + streamName);
                    if (string.Equals(args[0].ToUpper(), "-ATTR"))
                    {
                        v.PrintStream(frsNumber, streamType, streamName, (uint) NtfsAnalysis.OUTPUT_OPTION.OptTxt, 0, int.MaxValue, outputName);
                    }
                    else
                    {
                        v.PrintStream(frsNumber, streamType, streamName, (uint) NtfsAnalysis.OUTPUT_OPTION.OptHex, 0, int.MaxValue, outputName);
                    }
                }
                else if (string.Equals(args[0].ToUpper(), "-LCN"))
                {
                   UInt64 block= UInt64.Parse(args[2], System.Globalization.NumberStyles.HexNumber);
                    Int32 length =  Int32.Parse(args[3], System.Globalization.NumberStyles.HexNumber);
                    v.PrintLcnRange(block, length, (uint) NtfsAnalysis.OUTPUT_OPTION.OptHex, outputName);
                }
                else if (string.Equals(args[0].ToUpper(), "-LBN"))
                {
                   UInt64 block= UInt64.Parse(args[2], System.Globalization.NumberStyles.HexNumber);
                    Int32 length =  Int32.Parse(args[3], System.Globalization.NumberStyles.HexNumber);
                    v.PrintLbnRange(block, length, (uint) NtfsAnalysis.OUTPUT_OPTION.OptHex, outputName);
                }
                else{
                    PrintUsage();
                    throw new ArgumentNullException("Invalid command line.");
                }

                /////////////////////////////////////////////////////////////////////////////
                // Output whatever we just read.
                /////////////////////////////////////////////////////////////////////////////
                string[] lines = File.ReadAllLines(outputName);
                foreach (var line in lines)
                    Console.WriteLine(line);                
        } 
        catch(Exception e) 
            {
                PrintUsage();
                Console.WriteLine(e.ToString());
                return;       
            }
        }
    }
}
    