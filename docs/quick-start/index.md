# Quick Start

The easiest way to get started with OpusSharp is to install the [OpusSharp.Core](https://www.nuget.org/packages/OpusSharp.Core) package into a .NET application.

## Step 1: Install OpusSharp.Core

You can install OpusSharp.Core via the nuget package manager through your IDE, e.g. VS22, Rider, etc...

Or you can install it via the dotnet CLI.
```csharp
dotnet add package OpusSharp.Core --version x.y.z
```

## Step 2: Include Opus DLL.

By default, OpusSharp.Core DOES NOT contain the opus precompiled DLL's or binaries. This is so you can choose to provide your own DLL's or binary files instead of using OpusSharp's compiled binaries.

However if you want to use the precompiled binaries that OpusSharp provides, you can install the OpusSharp.Natives package onto your platform specific projects via the nuget package manager through your IDE, e.g. VS22, Rider, etc...

Or through the dotnet CLI.

```csharp
dotnet add package OpusSharp.Natives --version x.y.z
```

## Step 3: Create Encoder and Decoder

Creating the OpusEncoder and OpusDecoder is as easy as

### OpusEncoder

```csharp
using OpusSharp.Core;

var encoder = new OpusEncoder(48000, 2, OpusPredefinedValues.OPUS_APPLICATION_VOIP);

//20ms at 48khz is 960 samples
byte[] audio = ...;
byte[] encoded = new byte[1000];
int encodedBytes = encoder.Encode(audio, 960, encoded, encoded.Length);
Console.WriteLine(encodedBytes);
```

### OpusDecoder

```csharp
using OpusSharp.Core;

var decoder = new OpusDecoder(48000, 2);

//20ms at 48khz is 960 samples
byte[] encoded = ...;
byte[] decoded = new byte[3840]; //Works out to around 3840 bytes for 20ms audio
byte[] decodedSamples = decoder.Decode(encoded, encoded.Length, decoded, 960, false); //encoded.Length should not be used unless the size of the encoded audio is the exact same size.
Console.WriteLine(decodedSamples);
```

# Next Steps

📖 [Read the API for more information about the library](xref:OpusSharp.Core)

💬 [Use the discussions for help.](https://github.com/AvionBlock/OpusSharp/discussions)

📗 [Check out the examples.](../examples/Home.md)