Get-ChildItem -File |  # Get files
  Group-Object { $_.Name -replace '_.*' } |  # Group by part before first underscore
  ForEach-Object {
    # Create directory
    $dir = New-Item -Type Directory -Name $_.Name
    # Move files there
    $_.Group | Move-Item -Destination $dir
  }