###Adam Possing#####################################################################################
############################# ENSURE THE FOLDER NAME IS SHORT! #####################################
######### IF IT FAILS TO MOVE CERTAIN ITEMS ITS PROBABLY BECAUSE FODLER NAME IS TOO LONG ###########
####################################################################################################
####################################################################################################

# just place this file in the folder with the submissions and then run it

# If the script gives you a setting execution error (security error)
# run and hit A when prompted
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
# This will only change the policy for the given script and will return to default on next launch
# http://tritoneco.com/2014/02/21/fix-for-powershell-script-not-digitally-signed/



write-output "Moving Files"
Get-ChildItem -File | Where {$_.extension -notlike ".ps1"} |  # all files that arent the script
  Group-Object { $_.Name -creplace "^[^_]*_" -creplace '_.*' } |  # Group by part in between 2 underscores
  ForEach-Object {
    # Create directory if it doesnt exist
	if (!(test-path $_.Name))
	{
		$dir = New-Item -Type Directory -Name $_.Name
	}
	else
	{
		$dir = $PSScriptRoot + "/" + $_.Name
	}
    # Move files there
	$_.Group | Move-Item -Destination $dir
  }
 
write-output "Moving Files completed"

#write-output "Renaming Files"
#Get-ChildItem .\*\* | Where {$_.extension -like ".hs"} | Rename-Item -NewName {$_.name -replace '(?s)^.*_'}