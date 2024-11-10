using NAudio.Wave;
using OpusSharp.Core;

var format = new WaveFormat(48000, 2);
var buffer = new BufferedWaveProvider(format) { ReadFully = true };
var encoder = new OpusEncoder(format.SampleRate, format.Channels, OpusPredefinedValues.OPUS_APPLICATION_VOIP);
var decoder = new OpusDecoder(format.SampleRate, format.Channels);
var recorder = new WaveInEvent() { BufferMilliseconds = 20, WaveFormat = format };
var player = new WaveOutEvent();
recorder.DataAvailable += Recorder_DataAvailable;

recorder.StartRecording();
player.Init(buffer);
player.Play();

void Recorder_DataAvailable(object? sender, WaveInEventArgs e)
{
    var encoded = new byte[1000];
    var encodedBytes = encoder.Encode(e.Buffer, 960, encoded, encoded.Length);
    Console.WriteLine(encodedBytes);
    var decoded = new byte[3840];
    var decodedSamples = decoder.Decode(encoded, encodedBytes, decoded, 960, false);
    Console.WriteLine(decodedSamples);
    buffer.AddSamples(decoded, 0, decoded.Length);
}

Console.ReadLine();