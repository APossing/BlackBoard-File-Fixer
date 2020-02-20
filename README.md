**FolderizeSubmissions**
- This creates a folder that is named after the students inside it (first.last) and will group and move all files that are not .ps1 files.

**RenameAssignments <-Extension: string value for extension>**
- The extension may be removed from the file if you wish to just rename everything in the folder. This will cut out all the blackboard extra stuff and leave the file as they have named it. It is regex and can be messed up with weird underscoring or other problems. **This regex is based off of blackboard leaving a number and then an _ at the end of it**.

**CopyTestFiles <optional: FolderName (Default "Tests")>**
- This copies a folder into every one of the students folders with the purpose of putting test code inside.

**RunTests**
- An example of a file you can run inside each of the folders and should be included in the Tests folder

**RunHaskell1**
- An example of a way you can run all the scripts in just one call.


**Be sure to allow powershell scripts!!!**
