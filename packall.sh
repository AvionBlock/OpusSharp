#!/usr/bin/env sh
set -eu

rm -rf ./local-nuget
mkdir -p ./local-nuget

dotnet pack ./OpusSharp.Core/OpusSharp.Core.csproj -c Release -o ./local-nuget
dotnet pack ./OpusSharp.Natives/OpusSharp.Natives.csproj -c Release -o ./local-nuget
dotnet pack ./OpusSharp/OpusSharp.csproj -c Release -o ./local-nuget
