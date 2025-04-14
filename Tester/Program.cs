using OpusSharp.Core;
using OpusSharp.Core.Extensions;

namespace Tester
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var encoder = new OpusEncoder(48000, 1, OpusPredefinedValues.OPUS_APPLICATION_VOIP);
            Console.WriteLine(encoder.GetPacketLostPercent());
        }
    }
}

