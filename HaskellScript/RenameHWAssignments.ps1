write-output "Renaming Files"
Get-ChildItem .\*\* | Where {$_.extension -like ".hs"} | Rename-Item -NewName {$_.name -replace '(?s)^.*_'}
write-output "Completed Renaming Files"
