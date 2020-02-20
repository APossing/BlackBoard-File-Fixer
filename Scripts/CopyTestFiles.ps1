param($FolderName = "Tests")
$testFiles = Get-ChildItem -File $FolderName
$folders = Get-ChildItem -Directory | Where {$_.name -ne $FolderName}

$curPath = $PSScriptRoot
write-output "Copying Test Files"
ForEach ($item in $folders)
{
	#copy all files inside the folder provided
	ForEach ($fileName in $testFiles)
	{
		Copy-Item -Path ($curPath + "\" + $FolderName + "\" + $fileName) -dest ($item.FullName + "\" + $fileName)
	}
}
write-output "Completed Copying Test Files"