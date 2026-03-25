# Quick Start

The easiest way to get started with OpusSharp is to install the [OpusSharp](https://www.nuget.org/packages/OpusSharp) metapackage into a .NET application. It includes both `OpusSharp.Core` and `OpusSharp.Natives`.

## Step 1: Install OpusSharp

You can install OpusSharp via the nuget package manager through your IDE, e.g. VS22, Rider, etc...

Or you can install it via the dotnet CLI.
```csharp
dotnet add package OpusSharp --version x.y.z
```

## Step 2: Advanced Installation Options

If you want to manage native binaries yourself, install only [OpusSharp.Core](https://www.nuget.org/packages/OpusSharp.Core).

```csharp
dotnet add package OpusSharp.Core --version x.y.z
```

If you want only the precompiled binaries package, install [OpusSharp.Natives](https://www.nuget.org/packages/OpusSharp.Natives).

```csharp
dotnet add package OpusSharp.Natives --version x.y.z
```

> [!NOTE]
> iOS binaries are shipped through `OpusSharp.Natives` as `libopus.xcframework` and linked automatically in .NET iOS projects. At runtime OpusSharp automatically switches to `StaticNativeOpus` on iOS, so manually passing `use_static: true` is optional. WASM still requires manual static linking.

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
