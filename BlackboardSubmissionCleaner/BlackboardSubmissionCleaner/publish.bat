dotnet publish -r linux-x64 -c release /p:PublishSingleFile=true
move bin\release\netcoreapp3.1\linux-x64\publish\BlackboardSubmissionCleaner ..\Publishes\V1.0\linux-x64

dotnet publish -r win-x64 -c release /p:PublishSingleFile=true
move bin\release\netcoreapp3.1\win-x64\publish\BlackboardSubmissionCleaner ..\Publishes\V1.0\win-x64

dotnet publish -r osx-x64 -c release /p:PublishSingleFile=true
move bin\release\netcoreapp3.1\osx-x64\publish\BlackboardSubmissionCleaner ..\Publishes\V1.0\osx-x64