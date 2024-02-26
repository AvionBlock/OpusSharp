@echo off
nuget pack OpusSharp/OpusSharp.nuspec -Version 1.0.0 -OutputDirectory local-nuget
nuget pack OpusSharp.Android/OpusSharp.Android.nuspec -Version 1.0.0 -OutputDirectory local-nuget
nuget pack OpusSharp.Windows/OpusSharp.Windows.nuspec -Version 1.0.0 -OutputDirectory local-nuget
nuget pack OpusSharp.Core/OpusSharp.Core.nuspec -Version 1.0.0 -OutputDirectory local-nuget