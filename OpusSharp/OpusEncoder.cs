using OpusSharp.Enums;
using System;

namespace OpusSharp
{
    /// <summary>
    /// Audio encoder with opus.
    /// </summary>
    public class OpusEncoder : IDisposable
    {
        protected IntPtr Encoder {  get; }

        public int SampleRate { get; }
        public int Channels { get; }


        public OpusEncoder(int SampleRate, int Channels, Application Application)
        {
            this.SampleRate = SampleRate;
            this.Channels = Channels;

            Encoder = NativeOpus.opus_encoder_create(SampleRate, Channels, (int)Application, out var Error);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
