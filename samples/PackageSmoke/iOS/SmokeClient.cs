using OpusSharp.Core;
using OpusSharp.Core.Extensions;

namespace OpusSharp.Smoke.iOS;

public static class SmokeClient
{
    public static string Validate()
    {
        using var encoder = new OpusEncoder(48000, 1, OpusPredefinedValues.OPUS_APPLICATION_AUDIO);
        using var decoder = new OpusDecoder(48000, 1);

        var pcm = new short[960];
        var encoded = new byte[4000];
        var decoded = new short[960];

        encoder.SetBitRate(32000);
        var encodedBytes = encoder.Encode(pcm, 960, encoded, encoded.Length);
        var decodedSamples = decoder.Decode(encoded, encodedBytes, decoded, 960, false);

        return $"{OpusInfo.Version()}|{encodedBytes}|{decodedSamples}|{encoder.GetBitRate()}";
    }
}
