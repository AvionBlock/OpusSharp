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

        /// <summary>
        /// The coding mode that the encoder is set to.
        /// </summary>
        public Enums.Application OpusApplication { get; }

        /// <summary>
        /// Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.
        /// </summary>
        public int SampleRate { get; }

        /// <summary>
        /// Number of channels (1 or 2) in input signal.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Configures the bitrate in the encoder.
        /// </summary>
        public int Bitrate 
        { 
            get => bitrate; 
            set
            {
                NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_BITRATE, value);
                bitrate = value;
            }
        }
        /// <summary>
        /// Configures the encoder's computational complexity. The supported range is 0-10 inclusive with 10 representing the highest complexity.
        /// </summary>
        public int Complexity
        {
            get => complexity;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_COMPLEXITY, value));
                complexity = value;
            }
        }
        /// <summary>
        /// Configures the encoder's expected packet loss percentage. Loss percentage in the range 0-100, inclusive (default: 0).
        /// </summary>
        public int PacketLossPerc
        {
            get => packetLossPerc;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_PACKET_LOSS_PERC, value));
                PacketLossPerc = value;
            }
        }
        /// <summary>
        /// Configures the type of signal being encoded.
        /// </summary>
        public OpusSignal Signal
        {
            get => signal;
            set
            {
                CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)EncoderCtl.SET_SIGNAL, (int)value));
                signal = value;
            }
        }

        /// <summary>
        /// Creates and initializes an opus encoder.
        /// </summary>
        /// <param name="SampleRate">Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="Channels">Number of channels (1 or 2) in input signal.</param>
        /// <param name="Application">The coding mode that the encoder should set to.</param>
        public OpusEncoder(int SampleRate, int Channels, Enums.Application Application)
        {
            this.SampleRate = SampleRate;
            this.Channels = Channels;
            OpusApplication = Application;

            Encoder = NativeOpus.opus_encoder_create(SampleRate, Channels, (int)Application, out var Error);
            CheckError((int)Error);
            Bitrate = 32000;
            Complexity = 0;
            Signal = OpusSignal.Auto;
            PacketLossPerc = 0;
        }

        /// <summary>
        /// Encodes an Opus frame.
        /// </summary>
        /// <param name="input">Input signal (interleaved if 2 channels). length is frame_size*channels*sizeof(short)</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see Error codes) on failure.</returns>
        public unsafe int Encode(byte[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            int result = (int)OpusError.OK;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                NativeOpus.opus_encode(Encoder, (IntPtr)inPtr + inputOffset, frame_size, (IntPtr)outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Encodes an Opus frame.
        /// </summary>
        /// <param name="input">Input in float format (interleaved if 2 channels), with a normal range of +/-1.0. Samples with a range beyond +/-1.0 are supported but will be clipped by decoders using the integer API and should only be used if it is known that the far end supports extended dynamic range. length is frame_size*channels*sizeof(float)</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see Error codes) on failure.</returns>
        public unsafe int EncodeFloat(float[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            int result = (int)OpusError.OK;
            fixed (float* inPtr = input)
            fixed (byte* outPtr = output)
                NativeOpus.opus_encode_float(Encoder, (IntPtr)inPtr + inputOffset, frame_size, (IntPtr)outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }
        /// <summary>
        /// Gets the size of an OpusEncoder structure.
        /// </summary>
        /// <param name="channels">Number of channels. This must be 1 or 2.</param>
        /// <returns>The size in bytes.</returns>
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
