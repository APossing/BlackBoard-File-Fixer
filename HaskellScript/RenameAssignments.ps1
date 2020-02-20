param($Extension)
write-output "Renaming Files"
Get-ChildItem .\*\* | Where {$_.extension -like $Extension} | Rename-Item -NewName {$_.name -replace '(?s)^.*[0-9]_'}
write-output "Completed Renaming Files"
