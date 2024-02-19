using OpusSharp.Enums;
using System;

namespace OpusSharp
{
    public class OpusDecoder : IDisposable
    {
        protected IntPtr Decoder { get; }

        private bool isDisposed;
        public int SampleRate { get; }
        public int Channels { get; }

        public OpusDecoder(int SampleRate, int Channels)
        {
            this.SampleRate = SampleRate;
            this.Channels = Channels;

            Decoder = NativeOpus.opus_decoder_create(SampleRate, Channels, out var Error);
            CheckError((int)Error);
        }

        public unsafe int Decode(byte[] input, int inputLength, byte[] output, int frame_size, bool decodeFEC = false, int inputOffset = 0, int outputOffset = 0)
        {
            int result = 0;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_decode(Decoder, (IntPtr)inPtr + inputOffset, inputLength, (IntPtr)outPtr + outputOffset, frame_size, decodeFEC ? 1 : 0);
            CheckError(result);
            return result * sizeof(short) * Channels;
        }

        public unsafe int DecodeFloat(byte[] input, int inputLength, float[] output, int frame_size, bool decodeFEC = false, int inputOffset = 0, int outputOffset = 0)
        {
            int result = 0;
            fixed (byte* inPtr = input)
            fixed (float* outPtr = output)
                result = NativeOpus.opus_decode(Decoder, (IntPtr)inPtr + inputOffset, inputLength, (IntPtr)outPtr + outputOffset, frame_size, decodeFEC ? 1 : 0);
            CheckError(result);
            return result * sizeof(float) * Channels;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (Decoder != IntPtr.Zero)
                    NativeOpus.opus_decoder_destroy(Decoder);

                if (!isDisposed)
                    isDisposed = true;
            }
        }

        ~OpusDecoder()
        {
            Dispose(false);
        }

        protected static void CheckError(int result)
        {
            if (result < 0)
                throw new Exception($"Opus Error: {(OpusError)result}");
        }
    }
}
