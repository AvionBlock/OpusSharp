<!---
Title: OpusInfo
Xref: opusinfo-examples
--->

Opus info examples.

## Get version
```csharp
using OpusSharp.Core;

Console.WriteLine(OpusInfo.Version());
```

## Get string error
```csharp
using OpusSharp.Core;

Console.WriteLine(OpusInfo.StringError(0)); //opus_ok;
```