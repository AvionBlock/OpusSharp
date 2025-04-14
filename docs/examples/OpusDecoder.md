# OpusDecoder Examples

## Calling a CTL get

```csharp
using OpusSharp.Core;

OpusDecoder decoder = new OpusDecoder(48000, 2);

int sampleRate = 0;
decoder.Ctl<int>(GenericCTL.OPUS_GET_SAMPLE_RATE, ref sampleRate); //OpusSharp already checks if an error occurred with the CTL request and will throw an OpusException if there is an error, otherwise OpusErrorCodes.OPUS_OK.
Console.WriteLine(sampleRate);
```

## Calling a CTL set

```csharp
using OpusSharp.Core;

OpusDecoder decoder = new OpusDecoder(48000, 2);

//Most CTL set functions do not require a reference pointer, so we pass in the variable directly.
decoder.Ctl<int>(DecoderCTL.OPUS_SET_GAIN, 0); //OpusSharp already checks if an error occurred with the CTL request and will throw an OpusException if there is an error, otherwise OpusErrorCodes.OPUS_OK.
```

## Calling a CTL via OpusSharp.Core.Extensions

```csharp
using OpusSharp.Core;
using OpusSharp.Core.Extensions;

OpusDecoder decoder = new OpusDecoder(48000, 2);

decoder.SetGain(5); //Takes in a ushort.
Console.WriteLine(decoder.GetGain());
```
