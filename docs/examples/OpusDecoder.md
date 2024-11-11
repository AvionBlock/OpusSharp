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

int gain = 0;
decoder.Ctl<int>(DecoderCTL.OPUS_SET_GAIN, ref gain); //OpusSharp already checks if an error occurred with the CTL request and will throw an OpusException if there is an error, otherwise OpusErrorCodes.OPUS_OK.
```