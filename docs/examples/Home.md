# Examples

Below are some general examples for OpusSharp's api.

## Encoder Example
```cs
using OpusSharp.Core;

byte[] someAudioData = ...;
var frameSizeMs = 20;
var sampleRate = 48000;
var channels = 1;
var samplesPerFrame = sampleRate / (1000 / frameSizeMs) * channels; //960 samples per frame.
var encoder = new OpusEncoder(sampleRate, channels, OpusPredefinedValues.OPUS_APPLICATION_VOIP);

byte[] encodedAudio = new byte[1000] //1000 bytes for an encoded buffer should be enough according to the opus documentation.
var encodedBytes = encoder.Encode(someAudioData, samplesPerFrame, encodedAudio, encodedAudio.Length);

//We can use encodedBytes to trim any excess data from the encodedAudio buffer before sending over the network or writing to a file.
```

## Decoder Example
```cs
using OpusSharp.Core;

byte[] someEncodedAudio = ...;
var frameSizeMs = 20;
var sampleRate = 48000;
var channels = 1;
var samplesPerFrame = sampleRate / (1000 / frameSizeMs) * channels; //960 samples per frame.
var decoder = new OpusDecoder(sampleRate, channels);

var decoded = new byte[1920]; //We get 1920 bytes from doing this calculation because 16/8 (16 bit audio, 1 byte is 8 bits) equals 2 multiplied by samplesPerFrame gets us bytes per frame. 16/(sizeof(byte) * 8) * samplesPerFrame
var decodedSamples = decoder.Decode(someEncodedAudio, someEncodedAudio.Length, decoded, samplesPerFrame, false);
```

## NAudio Example
```csharp
using NAudio.Wave;
using OpusSharp.Core;

//Format to record and playback.
var format = new WaveFormat(48000, 2);
//Buffer for player to read and recorder to input.
var buffer = new BufferedWaveProvider(format) { ReadFully = true };

//Setup the encoder.
var encoder = new OpusEncoder(format.SampleRate, format.Channels, OpusPredefinedValues.OPUS_APPLICATION_VOIP);
//Setup the decoder.
var decoder = new OpusDecoder(format.SampleRate, format.Channels);

//Setup the recorder and player.
var recorder = new WaveInEvent() { BufferMilliseconds = 20, WaveFormat = format };
var player = new WaveOutEvent();
recorder.DataAvailable += Recorder_DataAvailable;

//Start recording and playback.
recorder.StartRecording();
player.Init(buffer);
player.Play();

void Recorder_DataAvailable(object? sender, WaveInEventArgs e)
{
    var encoded = new byte[1000]; //Allocate buffer to encode into.
    var encodedBytes = encoder.Encode(e.Buffer, 960, encoded, encoded.Length);
    Console.WriteLine(encodedBytes);
    
    
    var decoded = new byte[3840]; //Allocate buffer to decode into.
    var decodedSamples = decoder.Decode(encoded, encodedBytes, decoded, 960, false);
    Console.WriteLine(decodedSamples);
    
    //Add decoded audio into the player's buffer.
    buffer.AddSamples(decoded, 0, decoded.Length);
}

Console.ReadLine();
```
