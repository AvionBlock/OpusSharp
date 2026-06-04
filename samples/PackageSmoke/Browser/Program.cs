using OpusSharp.Core;

var pcm = new short[1024];
var encoded = new byte[4000];
var decoded = new short[1024];

using var encoder = new OpusEncoder(48000, 1, OpusPredefinedValues.OPUS_APPLICATION_AUDIO);
using var decoder = new OpusDecoder(48000, 1);

var encodedBytes = encoder.Encode(pcm, 960, encoded, encoded.Length);
var decodedSamples = decoder.Decode(encoded, encodedBytes, decoded, 960, false);

if (encodedBytes <= 0 || decodedSamples != 960)
{
    return 1;
}

Console.WriteLine($"{OpusInfo.Version()}|{encodedBytes}|{decodedSamples}");
return 0;
