using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SubmissionCleanerEngine
{
    public static class SubmissionCleaner
    {
        public static bool Folderize(string directory, bool verbose, char slash)
        { 
            DirectoryInfo d = new DirectoryInfo(directory);
            var files = d.GetFiles();
            Console.WriteLine("Listing first 5 files from directory...");
            for (int i = 0; i < Math.Min(5, files.Length - 1); i++)
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
                string namedDirectory = directory + slash + fileGroup.Key;

                if (!System.IO.Directory.Exists(namedDirectory))
                {
                    if (verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Creating directory {namedDirectory}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    System.IO.Directory.CreateDirectory(namedDirectory);
                }
                else
                {
                    if (verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{namedDirectory} already exists...");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                foreach (var file in fileGroup.ToList())
                {
                    string prevName = file.FullName;
                    string newName = namedDirectory + slash + file.Name;
                    Directory.Move(file.FullName, newName);
                    if (verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{file.Name} has been moved from {prevName} into {file.FullName}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            Console.WriteLine("Completed moving files");
            return true;
        }

        public static bool TrimAssignments(string directory, bool verbose, List<string> leftOutExtensions, char slash)
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
                    string newName = file.DirectoryName + slash + Regex.Replace(file.Name, "(?s)^.*[-][0-9][0-9]_", "");
                    if (!String.Equals(prevName, newName, StringComparison.OrdinalIgnoreCase))
                    {
                        Directory.Move(file.FullName, newName);
                        if (verbose)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{file.Name} has been moved from {prevName} into {newName}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        if (verbose)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{file.Name} has NOT moved As the prev and new names would be the same... from {prevName} into {newName}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
            }
            Console.WriteLine("Completed renaming files");
            return true;
        }

        public static bool CopyTestFiles(string fromDirectory, string toDirectory, bool verbose)
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
            foreach (var directory in directories)
            {
                if (verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"starting copy for {directory.Name}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
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
                {
                    Console.WriteLine($"Copying file from {file.FullName} to {temppath}");
                }
                if (File.Exists(temppath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{temppath} already exists, cannot copy");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Copied {file.Name} to {temppath}");
                    Console.ForegroundColor = ConsoleColor.White;
                    file.CopyTo(temppath, false);
                }
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

        private static bool ExtractZips(string directory, bool verbose)
        {


            return true;
        }

    }
}
