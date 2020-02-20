param($FolderName = "Tests")
$folders = Get-ChildItem -Directory | Where {$_.name -ne $FolderName}
$curPath = $PSScriptRoot
write-output "Running tests"
$jobs
$scriptBlock = { 
	param($item, $location)
	write-output $location
	Set-Location -Path $location
	write-output $item
	Push-Location $item
	#Run all scripts
	ForEach ($script in Get-ChildItem .\* | Where {$_.extension -like ".ps1"})
	{
		PowerShell ("./" + $script.Name)
	}
	write-output "Completed $item"
	Pop-Location
}

$location = Get-Location
$count = 0
ForEach ($item in $folders)
{
	write-output "starting $item"
	$job = Start-Job -name $item $scriptBlock -Arg $item, $location.Path
	$job | Wait-Job -Timeout 45
	$count++
	
	write-output ($count / $folders.count * 100)
}
write-output "Completed Running Tests"
