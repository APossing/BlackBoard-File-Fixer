**How to run C# implementation**
<br/>
1. Download appropriate self contained executable for your OS (currenlty have 64bit of osx, linux, windows)
1. Run with flags anywhere. All flags are optional, all flags are case insensitive
- `-Directory <string>` or `-D <string>` path to Unzipped blackboard submission, default is current directory if not given.
- `-Tests <string>` path to folder with tests (all subfolders and items in this folder will be copied to every folder under directory tag), default is "" and will not copy anything.
- `-NoTrim` Disables trimming of files after moving to named directories
- `-Verbose` or `-V` Shows almost everything that is happening during execution.  
- `-Extract` Extracts all zips contained within student folders.
- for example `BlackboardSubmissionCleaner -D C:\Users\Adam\Desktop\unzipBlackboard -v -Tests C:\Users\Adam\Desktop\Tests -Extract`  
<br/>

**.net Core 3.1 self contained executables download links**  

[Linux-x64](https://github.com/APossing/BlackBoard-File-Fixer/raw/master/BlackboardSubmissionCleaner/Publishes/V1.0/linux-x64/BlackboardSubmissionCleaner)  
[OSX-x64](https://github.com/APossing/BlackBoard-File-Fixer/raw/master/BlackboardSubmissionCleaner/Publishes/V1.0/osx-x64/BlackboardSubmissionCleaner)  
[Windows-x64](https://github.com/APossing/BlackBoard-File-Fixer/raw/master/BlackboardSubmissionCleaner/Publishes/V1.0/win-x64/BlackboardSubmissionCleaner.exe)  
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>  

**THIS AREA FOR THE SCRIPT FILES**  

<br/>
<br/>
<br/>  

**If running from Mac OS / unix system**  

1. Install PowerShell (https://github.com/PowerShell/PowerShell/releases/) and download and install your system's package and then open it and follow the main instructions.

**How to run**
1. unzip blackboard submission download. **BE SURE TO MAKE THE FILENAME SHORT AS THE MAX CHARACTERS IS 255 AND YOU WILL REACH IT IF YOU'RE NOT CAREFUL.**
1. Copy and paste script files into the unzipped folder.
1. Open PowerShell as admisistrator
enter the below command. The bypass execution policy will only last for this session inside PowerShell.
1. PowerShell defaults to restricted mode making it so you cannot run any scripts, so you will have to set your PowerShell to allow for unsigned scripts to run as this script is not signed. The bypass execution policy will only last for this session of PowerShell.
`Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass` (type A for Yes to All)
1. Copy path to script
1. Type `cd (paste path)`
1. Type `.\(script name)` to run a script  
<br/>
<br/>
<br/>

**Generally you will only run FolderizeSubmissions and RenameAssignments (in that order)**  
**For more automated running of students files, the CopyTestFiles and RunTests would be used (in that order as well)**

**FolderizeSubmissions**
- This creates a folder that is named after the students inside it (first.last) and will group and move all files that are not .ps1 files.

**RenameAssignments <-Extension: string value for extension>**
- The extension may be removed from the file if you wish to just rename everything in the folder. This will cut out all the blackboard extra stuff and leave the file as they have named it. It is regex and can be messed up with weird underscoring or other problems. **This regex is based off of blackboard leaving a -##_ at the end of the blackboard submission part**.

**CopyTestFiles <optional: FolderName (Default "Tests")>**
- This copies a folder into every one of the students folders with the purpose of putting test code inside.

**RunTests**
- An example of a file you can run inside each of the folders and should be included in the Tests folder

**RunHaskell1**
- An example of a way you can run all the scripts in just one call.
