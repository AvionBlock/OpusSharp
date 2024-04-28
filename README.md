# OpusSharp
OpusSharp aims to be a cross platform C# compatible version of the native opus codec/library. The code uses the native compiled DLL's with instructions on how to compile you're own. Currently Windows and Android binaries are only available.

# Examples
Encoder:
```cs
using OpusSharp.Core;

public class ExampleEncoder
{
  public const int SampleRate = 48000; //48khz
  public const int Channels = 2; //Stereo

  private OpusEncoder Encoder;

  public ExampleEncoder()
  {
    Encoder = new OpusEncoder(SampleRate, Channels, OpusSharp.Core.Enums.PreDefCtl.OPUS_APPLICATION_AUDIO);
    Encoder.Bitrate = 64000; //64k bitrate.
  }

  public int Encode(byte[] buffer, int count, byte[] output)
  {
    var encodeBuffer = new byte[1000]; //Allocate our encode buffer.
    var encodedBytes = Encoder.Encode(buffer, count, encodeBuffer); //Encode the audio. OpusSharp automatically converts byte count to short count for the native opus handling input.
    output = encodeBuffer.SkipLast(encodeBuffer.Length - encodedBytes).ToArray(); //Trim the buffer so it only contains the encoded bytes and not the empty ones.
    return encodedBytes; //Return the amount of bytes encoded.
  }
}
```

```cs
using OpusSharp.Core;

public class ExampleDecoder
{
  public const int SampleRate = 48000; //48khz
  public const int Channels = 2; //Stereo.
  public const int FrameSize = 20; //20 ms.

  private OpusDecoder Decoder;

  public ExampleDecoder()
  {
    Decoder = new OpusDecoder(SampleRate, Channels);
  }

  public int Decode(byte[] input, int count, byte[] decoded)
  {
    decoded = new byte[SampleRate / 1000 * FrameSize * Channels]; //Allocate the required buffer size to handle the decoded audio.
    var bytesRead = Decoder.Decode(input, count, decoded, decoded.Length); //Decode the audio. Again, OpusSharp automatically converts byte count to short count for the native opus handling output.
    return bytesRead;
  }
}
```

You are allowed to directly use the CTL.
```cs
var isUsingFec = Encoder.EncoderCtl(OpusSharp.Core.Enums.EncoderCtl.OPUS_GET_INBAND_FEC_REQUEST);
Console.WriteLine(isUsingFec);
```

# Packaging as nuget
To use this library, you will need to package it so it can dynamically be loaded onto your project without having to declare or make your own library specific to a platform as that is handled by the nuget file.

First, Open the `.sln` file in an IDE of you're choice, then right click on the solution and build.

To package, Make sure to have nuget already installed either by an EXE in the same directory as this repository or install via PATH, Then just run `PackAll.bat`.

# Compiling Native Builds
Please check [OpusLibs](./OpusLibs) for more information.

# Using the nuget package
Just install it onto your directory, If you cannot find your package in the nuget package manager of your project, you will need to add you're local nuget feed to you're IDE: https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources

# Working And Tested
- OpusEncoder - Every function is working
- OpusDecoder - Every function is working except for Parse() function as that throws a System.EngineExecutionException, I don't know why...

# Not Tested
- OpusMSEncoder
- OpusMSDecoder
- Repacketizer

# Planned
- OpusInfo
- DredDecoder
