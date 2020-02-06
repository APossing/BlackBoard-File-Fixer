Get-ChildItem -File |  # Get files
  Group-Object { $_.Name -creplace "^[^_]*_" -creplace '_.*' } |  # Group by part in between 2 underscores
  ForEach-Object {
    # Create directory
	if (!test-path $_.Name)
	{
		$dir = New-Item -Type Directory -Name $_.Name
	}
	else
	{
		$dir = $PSScriptRoot + "/" + $_.Name
	}
	
    #rename file to just be hw1 or hw1tests
    #Rename-Item $_.FullName -creplace '(?s)^.*_'
    # Move files there
    $_.Group | Move-Item -whatif -Destination $dir
  }