param($FolderName = "Tests")
.\FolderizeSubmissions.ps1
.\RenameHWAssignments.ps1
.\CopyTestFiles.ps1 -FolderName $FolderName
.\RunTests.ps1 -FolderName $FolderName
