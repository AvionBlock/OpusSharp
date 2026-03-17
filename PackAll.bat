@echo off
if exist local-nuget rmdir /s /q local-nuget
mkdir local-nuget

dotnet pack .\OpusSharp.Core\OpusSharp.Core.csproj -c Release -o .\local-nuget
dotnet pack .\OpusSharp.Natives\OpusSharp.Natives.csproj -c Release -o .\local-nuget
dotnet pack .\OpusSharp\OpusSharp.csproj -c Release -o .\local-nuget
