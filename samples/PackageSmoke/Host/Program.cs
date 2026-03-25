using OpusSharp.Core;
var version = OpusInfo.Version();
var errorText = OpusInfo.StringError((int)OpusErrorCodes.OPUS_INVALID_PACKET);

if (string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(errorText))
{
    return 1;
}

Console.WriteLine(version);
Console.WriteLine(errorText);

return 0;
