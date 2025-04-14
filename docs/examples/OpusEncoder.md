# OpusEncoder Examples

## Calling a CTL get

```csharp
using OpusSharp.Core;

OpusEncoder encoder = new OpusEncoder(48000, 2, OpusPredefinedValues.OPUS_APPLICATION_VOIP);

int sampleRate = 0;
encoder.Ctl<int>(GenericCTL.OPUS_GET_SAMPLE_RATE, ref sampleRate); //OpusSharp already checks if an error occurred with the CTL request and will throw an OpusException if there is an error, otherwise OpusErrorCodes.OPUS_OK.
Console.WriteLine(sampleRate);
```

## Calling a CTL set

```csharp
using OpusSharp.Core;

OpusEncoder encoder = new OpusEncoder(48000, 2, OpusPredefinedValues.OPUS_APPLICATION_VOIP);

int vbr = 1; //1 == true, 0 == false
encoder.Ctl<int>(EncoderCTL.OPUS_SET_VBR, ref vbr); //OpusSharp already checks if an error occurred with the CTL request and will throw an OpusException if there is an error, otherwise OpusErrorCodes.OPUS_OK.
```

## Calling a CTL via OpusSharp.Core.Extensions

```csharp
using OpusSharp.Core;
using OpusSharp.Core.Extensions;

OpusEncoder encoder = new OpusEncoder(48000, 2, OpusPredefinedValues.OPUS_APPLICATION_AUDIO);

encoder.SetComplexity(2);
Console.WriteLine(encoder.GetComplexity());
```
