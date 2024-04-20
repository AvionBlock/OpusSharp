using OpusSharp.Core.SafeHandlers;
using System;

namespace OpusSharp.Core
{
    /// <summary>
    /// Audio multistream encoder with opus.
    /// </summary>
    public class OpusMSEncoder : Disposable
    {
        private readonly OpusMSEncoderSafeHandle Encoder;

        #region Variables
        /// <summary>
        /// Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.
        /// </summary>
        public int SampleRate
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                return EncoderCtl(Enums.GenericCtl.OPUS_GET_SAMPLE_RATE_REQUEST);
            }
        }

        /// <summary>
        /// Number of channels in the input signal. This must be at most 255. It may be greater than the number of coded channels (streams + coupled_streams).
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Configures the bitrate in the multistream encoder.
        /// </summary>
        public int Bitrate
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_BITRATE_REQUEST, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_BITRATE_REQUEST, value);
            }
        }

        /// <summary>
        /// The coding mode that the multistream encoder is set to.
        /// </summary>
        public Enums.PreDefCtl OpusApplication
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_APPLICATION_REQUEST, out int value);
                return (Enums.PreDefCtl)value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_APPLICATION_REQUEST, (int)value);
            }
        }

        /// <summary>
        /// Configures the multistream encoder's computational complexity. The supported range is 0-10 inclusive with 10 representing the highest complexity.
        /// </summary>
        public int Complexity
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_COMPLEXITY_REQUEST, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_COMPLEXITY_REQUEST, value);
            }
        }

        /// <summary>
        /// Configures the multistream encoder's expected packet loss percentage. Loss percentage in the range 0-100, inclusive (default: 0).
        /// </summary>
        public int PacketLossPerc
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_PACKET_LOSS_PERC_REQUEST, out int value);
                return value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_PACKET_LOSS_PERC_REQUEST, value);
            }
        }

        /// <summary>
        /// Configures the type of signal being encoded.
        /// </summary>
        public Enums.PreDefCtl Signal
        {
            get
            {
                if (Encoder.IsClosed) return 0;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_SIGNAL_REQUEST, out int value);
                return (Enums.PreDefCtl)value;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_SIGNAL_REQUEST, (int)value);
            }
        }

        /// <summary>
        /// Enables or disables variable bitrate (VBR) in the multistream encoder.
        /// </summary>
        public bool VBR
        {
            get
            {
                if (Encoder.IsClosed) return false;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_VBR_REQUEST, out int value);
                return value == 1;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_VBR_REQUEST, value == true ? 1 : 0);
            }
        }

        /// <summary>
        /// Enables or disables constraint variable bitrate (CVBR) in the multistream encoder.
        /// </summary>
        public bool VBRConstraint
        {
            get
            {
                if (Encoder.IsClosed) return false;
                EncoderCtl(Enums.EncoderCtl.OPUS_GET_VBR_CONSTRAINT_REQUEST, out int value);
                return value == 1;
            }
            set
            {
                if (Encoder.IsClosed) return;
                EncoderCtl(Enums.EncoderCtl.OPUS_SET_VBR_CONSTRAINT_REQUEST, value == true ? 1 : 0);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and initializes an opus multistream encoder.
        /// </summary>
        /// <param name="SampleRate">Sampling rate of the input signal (in Hz). This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="Channels">Number of channels in the input signal. This must be at most 255. It may be greater than the number of coded channels (streams + coupled_streams).</param>
        /// <param name="Streams">The total number of streams to encode from the input. This must be no more than the number of channels.</param>
        /// <param name="CoupledStreams">Number of coupled (2 channel) streams to encode. This must be no larger than the total number of streams. Additionally, The total number of encoded channels (streams + coupled_streams) must be no more than the number of input channels.</param>
        /// <param name="mapping">Mapping from encoded channels to input channels, as described in Opus Multistream API. As an extra constraint, the multistream encoder does not allow encoding coupled streams for which one channel is unused since this is never a good idea.</param>
        /// <param name="Application">The target encoder application.</param>
        /// <exception cref="OpusException"></exception>
        public unsafe OpusMSEncoder(int SampleRate, int Channels, int Streams, int CoupledStreams, byte[] mapping, Enums.PreDefCtl Application)
        {
            Enums.OpusError error;
            fixed (byte* mapPtr = mapping)
                Encoder = NativeOpus.opus_multistream_encoder_create(SampleRate, Channels, Streams, CoupledStreams, mapPtr, (int)Application, out error);
            CheckError((int)error);

            this.Channels = Channels;
        }

        /// <summary>
        /// Encodes a multistream Opus frame.
        /// </summary>
        /// <param name="input">The input signal as interleaved samples. This must contain frame_size*channels</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload. This must contain storage for at least max_data_bytes. This variable is also used to determine the upper limit on the instant bitrate.</param>
        /// <param name="outputOffset">Offset to start reading in the input.</param>
        /// <param name="inputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Encode(byte[] input, int frame_size, byte[] output, int outputOffset = 0, int inputOffset = 0)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_multistream_encode(Encoder, inPtr + inputOffset, frame_size / 2, outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Encodes a multistream Opus frame.
        /// </summary>
        /// <param name="input">The input signal as interleaved samples. This must contain frame_size*channels</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload. This must contain storage for at least max_data_bytes. This variable is also used to determine the upper limit on the instant bitrate.</param>
        /// <param name="outputOffset">Offset to start reading in the input.</param>
        /// <param name="inputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Encode(short[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            byte[] byteInput = new byte[input.Length * 2]; //Short to byte is 2 bytes.
#pragma warning disable CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
            Buffer.BlockCopy(input, 0, byteInput, 0, input.Length);
#pragma warning restore CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument

            int result = 0;
            fixed (byte* inPtr = byteInput)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_multistream_encode(Encoder, inPtr + inputOffset, frame_size, outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            Buffer.BlockCopy(byteInput, 0, output, 0, output.Length);
            return result;
        }

        /// <summary>
        /// Encodes a multistream Opus frame from floating point input.
        /// </summary>
        /// <param name="input">The input signal as interleaved samples with a normal range of +/-1.0. Samples with a range beyond +/-1.0 are supported but will be clipped by decoders using the integer API and should only be used if it is known that the far end supports extended dynamic range. This must contain frame_size*channels samples.</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload. This must contain storage for at least max_data_bytes. This variable is also used to determine the upper limit on the instant bitrate.</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int EncodeFloat(float[] input, int frame_size, byte[] output, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (float* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_multistream_encode_float(Encoder, inPtr + inputOffset, frame_size, outPtr + outputOffset, output.Length - outputOffset);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <param name="value">The value to input.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void EncoderCtl(Enums.EncoderCtl ctl, int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_multistream_encoder_ctl(Encoder, (int)ctl, value));
        }

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <param name="value">The value that is outputted from the CTL.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void EncoderCtl(Enums.EncoderCtl ctl, out int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_multistream_encoder_ctl(Encoder, (int)ctl, out int val));
            value = val;
        }

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <param name="value">The value to input.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void EncoderCtl(Enums.GenericCtl ctl, int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_multistream_encoder_ctl(Encoder, (int)ctl, value));
        }

        /// <summary>
        /// Requests a CTL on the encoder.
        /// </summary>
        /// <param name="ctl">The encoder CTL to request.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public int EncoderCtl(Enums.GenericCtl ctl)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_multistream_encoder_ctl(Encoder, (int)ctl, out int val));
            return val;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Encoder.IsClosed)
                    Encoder.Close();
            }
        }

        /// <summary>
        /// Checks if the object is disposed and throws.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (Encoder.IsClosed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        #endregion

        /// <summary>
        /// Check's for an opus error and throws if there is one.
        /// </summary>
        /// <param name="result"></param>
        /// <exception cref="OpusException"></exception>
        protected static void CheckError(int result)
        {
            if (result < 0)
                throw new OpusException(((Enums.OpusError)result).ToString());
        }
    }
}