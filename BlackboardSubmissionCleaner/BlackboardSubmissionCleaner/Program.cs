using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using SubmissionCleanerEngine;

namespace BlackboardSubmissionCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            bool isMacOs = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            string executionFilePath = AppDomain.CurrentDomain.BaseDirectory;
            string testFilePath = "";
            bool verbose = false;
            bool trim = true;
            List<string> leftOutExtensions = new List<string> { "txt" };
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (String.Equals(args[i], "-Directory", StringComparison.OrdinalIgnoreCase))
                    {
                        i++;
                        executionFilePath = args[i];
                    }
                    else if (String.Equals(args[i], "-D", StringComparison.OrdinalIgnoreCase))
                    {
                        i++;
                        executionFilePath = args[i];
                    }
                    else if (String.Equals(args[i], "-Tests", StringComparison.OrdinalIgnoreCase))
                    {
                        i++;
                        testFilePath = args[i];
                    }
                    else if (String.Equals(args[i], "-NoTrim", StringComparison.OrdinalIgnoreCase))
                    {
                        trim = false;
                    }
                    else if (String.Equals(args[i], "-Verbose", StringComparison.OrdinalIgnoreCase))
                    {
                        verbose = true;
                    }
                    else if (String.Equals(args[i], "-V", StringComparison.OrdinalIgnoreCase))
                    {
                        verbose = true;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid input \"{0}\"", args[i]);
                        return;
                    }
                }
            }


            char slash = isLinux || isMacOs ? '/' : '\\';
            bool folderizeResultSuccess = SubmissionCleaner.Folderize(executionFilePath, verbose, slash);
            if (trim && folderizeResultSuccess)
            {
                SubmissionCleaner.TrimAssignments(executionFilePath, verbose, leftOutExtensions, slash);
            }
            if (testFilePath != "" && folderizeResultSuccess)
            {
                SubmissionCleaner.CopyTestFiles(testFilePath, executionFilePath, verbose);
            }
        }
    }

        
}
