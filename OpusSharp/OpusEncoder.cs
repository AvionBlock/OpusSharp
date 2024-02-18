using OpusSharp.Enums;
using System;

namespace OpusSharp
{
    /// <summary>
    /// Audio encoder with opus.
    /// </summary>
    public class OpusEncoder : IDisposable
    {
        protected IntPtr Encoder { get; }

        private int bitrate;
        private int complexity;
        private int packetLossPerc;
        private OpusSignal signal;
        private bool isDisposed;

        public Application OpusApplication { get; }
        public int SampleRate { get; }
        public int Channels { get; }
        public int Bitrate 
        { 
            get => bitrate; 
            set
            {
                NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_BITRATE, value);
                bitrate = value;
            }
        }
        public int Complexity
        {
            get => complexity;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_COMPLEXITY, value));
                complexity = value;
            }
        }
        public int PacketLossPerc
        {
            get => packetLossPerc;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_PACKET_LOSS_PERC, value));
                PacketLossPerc = value;
            }
        }
        public OpusSignal Signal
        {
            get => signal;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_SIGNAL, (int)value));
                signal = value;
            }
        }


        public OpusEncoder(int SampleRate, int Channels, Application Application)
        {
            this.SampleRate = SampleRate;
            this.Channels = Channels;
            OpusApplication = Application;

            Encoder = NativeOpus.opus_encoder_create(SampleRate, Channels, (int)Application, out var Error);
            CheckError((int)Error);
            Bitrate = 32000;
            Complexity = 1;
            Signal = OpusSignal.Auto;
            PacketLossPerc = 0;
        }

        public unsafe int Encode(byte[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            int result = (int)OpusError.OK;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                NativeOpus.opus_encode(Encoder, (IntPtr)inPtr + inputOffset, frame_size, (IntPtr)outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }

        public unsafe int EncodeFloat(float[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            int result = (int)OpusError.OK;
            fixed (float* inPtr = input)
            fixed (byte* outPtr = output)
                NativeOpus.opus_encode_float(Encoder, (IntPtr)inPtr + inputOffset, frame_size, (IntPtr)outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }

        public int GetSize(int channels)
        {
            return NativeOpus.opus_encoder_get_size(channels);
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
                if (Encoder != IntPtr.Zero)
                    NativeOpus.opus_encoder_destroy(Encoder);

                if (!isDisposed)
                    isDisposed = true;
            }
        }

        ~OpusEncoder()
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
