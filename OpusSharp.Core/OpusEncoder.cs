using OpusSharp.Enums;
using OpusSharp.SafeHandlers;
using System;

namespace OpusSharp
{
    /// <summary>
    /// Audio encoder with opus.
    /// </summary>
    public class OpusEncoder : IDisposable
    {
        private readonly OpusEncoderSafeHandle Encoder;

        #region Variables
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
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.GET_BITRATE, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_BITRATE, value);
            }
        }
        /// <summary>
        /// The coding mode that the encoder is set to.
        /// </summary>
        public Enums.Application OpusApplication
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.GET_APPLICATION, out int value);
                return (Enums.Application)value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_APPLICATION, (int)value);
            }
        }
        /// <summary>
        /// Configures the encoder's computational complexity. The supported range is 0-10 inclusive with 10 representing the highest complexity.
        /// </summary>
        public int Complexity
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.GET_COMPLEXITY, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_COMPLEXITY, value);
            }
        }
        /// <summary>
        /// Configures the encoder's expected packet loss percentage. Loss percentage in the range 0-100, inclusive (default: 0).
        /// </summary>
        public int PacketLossPerc
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.GET_PACKET_LOSS_PERC, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_PACKET_LOSS_PERC, value);
            }
        }
        /// <summary>
        /// Configures the type of signal being encoded.
        /// </summary>
        public OpusSignal Signal
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.GET_SIGNAL, out int value);
                return (OpusSignal)value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_SIGNAL, (int)value);
            }
        }
        /// <summary>
        /// Enables or disables variable bitrate (VBR) in the encoder.
        /// </summary>
        public bool VBR
        {
            get
            {
                if (Encoder.IsClosed) return false;
                EncoderCtl(Enums.EncoderCtl.GET_VBR, out int value);
                return value == 1;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.SET_VBR, value == true ? 1 : 0);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and initializes an opus encoder.
        /// </summary>
        /// <param name="SampleRate">Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="Channels">Number of channels (1 or 2) in input signal.</param>
        /// <param name="Application">The coding mode that the encoder should set to.</param>
        public OpusEncoder(int SampleRate, int Channels, Enums.Application Application)
        {
            Encoder = NativeOpus.opus_encoder_create(SampleRate, Channels, (int)Application, out var Error);
            CheckError((int)Error);

            this.SampleRate = SampleRate;
            this.Channels = Channels;
        }

        /// <summary>
        /// Encodes an Opus frame.
        /// </summary>
        /// <param name="input">Input signal (interleaved if 2 channels). length is frame_size*channels</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see Error codes) on failure.</returns>
        public unsafe int Encode(byte[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            int result = (int)OpusError.OK;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_encode(Encoder, inPtr + inputOffset, frame_size / 2, outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
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
        public unsafe int Encode(short[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            byte[] byteInput = new byte[input.Length * 2]; //Short to byte is 2 bytes.
            Buffer.BlockCopy(input, 0, byteInput, 0, input.Length);

            int result = (int)OpusError.OK;
            fixed (byte* inPtr = byteInput)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_encode(Encoder, inPtr + inputOffset, frame_size, outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            Buffer.BlockCopy(byteInput, 0, output, 0, output.Length);
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
            ThrowIfDisposed();

            int result = (int)OpusError.OK;
            fixed (float* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_encode_float(Encoder, inPtr + inputOffset, frame_size, outPtr + outputOffset, output.Length - outputOffset);

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

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <param name="value">The value to input.</param>
        public void EncoderCtl(Enums.EncoderCtl ctl, int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)ctl, value));
        }

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <param name="value">The value that is outputted from the CTL.</param>
        public void EncoderCtl(Enums.EncoderCtl ctl, out int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_encoder_ctl(Encoder, (int)ctl, out int val));
            value = val;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!Encoder.IsClosed)
                Encoder.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if(Encoder.IsClosed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        #endregion

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
