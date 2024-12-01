dotnet build ./OpusSharp.Core/OpusSharp.Core.csproj -c Release

nuget pack ./OpusSharp.Core/OpusSharp.Core.nuspec -OutputDirectory local-nuget
nuget pack ./OpusSharp.Natives/OpusSharp.Natives.nuspec -OutputDirectory local-nuget
nuget pack ./OpusSharp/OpusSharp.nuspec -OutputDirectory local-nuget
