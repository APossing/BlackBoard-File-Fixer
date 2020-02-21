using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlackboardSubmissionCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
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
                    }
                }
            }

            bool folderizeResultSuccess = Folderize(executionFilePath, verbose);
            if (trim && folderizeResultSuccess)
            {
                TrimAssignments(executionFilePath, verbose, leftOutExtensions);
            }
            if (testFilePath != "" && folderizeResultSuccess)
            {
                CopyTestFiles(testFilePath, executionFilePath, verbose);
            }
        }

        private static bool Folderize(string directory, bool verbose)
        {
            DirectoryInfo d = new DirectoryInfo(directory);
            var files = d.GetFiles();
            Console.WriteLine("Listing first 5 files from directory...");
            for (int i = 0; i < Math.Min(5,files.Length-1); i++)
                Console.WriteLine(files[i].FullName);
            Console.WriteLine("Are you sure you want to continue folderize? [y]es [n]o");
            var key = Console.ReadKey();
            if (char.ToLower(key.KeyChar) != 'y')
            {
                Console.WriteLine("no files modified. Exiting...");
                return false;
            }


            Console.WriteLine("\nMoving files");
            var fileGroups = d.GetFiles().Where(x => !x.Name.EndsWith(".exe") && !x.Name.EndsWith(".ps1")).GroupBy(u => Regex.Replace(Regex.Replace(u.ToString(), "^[^_]*_", ""), "_.*", "")).ToList();
            foreach (var fileGroup in fileGroups)
            {
                string namedDirectory = directory + @"\" + fileGroup.Key;
                if (!System.IO.Directory.Exists(namedDirectory))
                {
                    if (verbose)
                        Console.WriteLine($"Creating directory {namedDirectory}");

                    System.IO.Directory.CreateDirectory(namedDirectory);
                }
                else
                {
                    if (verbose)
                        Console.WriteLine($"{namedDirectory} already exists...");
                }

                foreach (var file in fileGroup.ToList())
                {
                    string prevName = file.FullName;
                    Directory.Move(file.FullName, namedDirectory + "\\" + file.Name);
                    if (verbose)
                        Console.WriteLine($"{file.Name} has been moved from {prevName} into {file.FullName}");
                }
            }
            Console.WriteLine("Completed moving files");
            return true;
        }

        private static bool TrimAssignments(string directory, bool verbose, List<string> leftOutExtensions)
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(directory);
            var tempDirectories = mainDirectory.GetDirectories();

            Console.WriteLine("Listing first 5 directories from directory...");
            for (int i = 0; i < Math.Min(5, tempDirectories.Length - 1); i++)
                Console.WriteLine(tempDirectories[i].FullName);
            Console.WriteLine("Are you sure you want to continue trimming assignments? [y]es [n]o");
            var key = Console.ReadKey();
            if (char.ToLower(key.KeyChar) != 'y')
            {
                Console.WriteLine("no files modified. Exiting...");
                return false;
            }

            Console.WriteLine("\nTrimming files");
            foreach (var d in mainDirectory.GetDirectories())
            {
                var files = d.GetFiles().Where(file => !leftOutExtensions.Any(extension => file.Extension.EndsWith(extension)));
                foreach (var file in files)
                {
                    string prevName = file.FullName;
                    string newName = file.DirectoryName + @"\" + Regex.Replace(file.Name, "(?s)^.*[-][0-9][0-9]_", "");
                    if (!String.Equals(prevName, newName, StringComparison.OrdinalIgnoreCase))
                    {
                        Directory.Move(file.FullName, newName);
                        if (verbose)
                            Console.WriteLine($"{file.Name} has been moved from {prevName} into {newName}");
                    }
                    else
                    {
                        if (verbose)
                            Console.WriteLine($"{file.Name} has NOT moved As the prev and new names would be the same... from {prevName} into {newName}");
                    }
                }
            }
            Console.WriteLine("Completed renaming files");
            return true;
        }

        private static bool CopyTestFiles(string fromDirectory, string toDirectory, bool verbose)
        {
            DirectoryInfo fromDirectoryInfo = new DirectoryInfo(fromDirectory);
            DirectoryInfo toDirectoryInfo = new DirectoryInfo(toDirectory);
            var tempFromFiles = fromDirectoryInfo.GetFiles();
            var tempFromDirectories = fromDirectoryInfo.GetDirectories();
            var tempToDirectories = toDirectoryInfo.GetDirectories();


            Console.WriteLine("Listing first 5 files from test directory...");
            for (int i = 0; i < Math.Min(5, tempFromFiles.Length - 1); i++)
                Console.WriteLine(tempFromFiles[i].FullName);

            Console.WriteLine("Listing first 5 directories from test directory...");
            for (int i = 0; i < Math.Min(5, tempFromDirectories.Length - 1); i++)
                Console.WriteLine(tempFromDirectories[i].FullName);

            Console.WriteLine("Listing first 5 directories from main directory...");
            for (int i = 0; i < Math.Min(5, tempToDirectories.Length - 1); i++)
                Console.WriteLine(tempToDirectories[i].FullName);


            Console.WriteLine("Are you sure you want to continue copying test files? [y]es [n]o");
            var key = Console.ReadKey();
            if (char.ToLower(key.KeyChar) != 'y')
            {
                Console.WriteLine("no files modified. Exiting...");
                return false;
            }

            Console.WriteLine("starting test file copying");

            var directories = toDirectoryInfo.GetDirectories();
            foreach(var directory in directories)
            {
                if (verbose)
                    Console.WriteLine($"starting copy for {directory.Name}");
                DirectoryCopy(fromDirectory, directory.FullName, true, verbose);
            }

            Console.WriteLine("Completed copying test files");
            return true;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool verbose)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                if (verbose)
                    Console.WriteLine($"Copying file from {file.FullName} to {temppath}");
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs, verbose);
                }
            }
        }

    }
}
