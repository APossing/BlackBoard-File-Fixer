param($extension)
write-output "Renaming Files"
Get-ChildItem .\*\* | Where {$_.extension -like $Extension} | Rename-Item -NewName {$_.name -replace '(?s)^.*_'}
write-output "Completed Renaming Files"
