using OpusSharp.Core.SafeHandlers;
using System;

namespace OpusSharp.Core
{
    /// <summary>
    /// Audio decoder with opus.
    /// </summary>
    public class OpusDecoder : Disposable
    {
        private readonly OpusDecoderSafeHandle Decoder;

        #region Variables
        /// <summary>
        /// Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.
        /// </summary>
        public int SampleRate 
        { 
            get 
            {
                if (Decoder.IsClosed) return 0;
                DecoderCtl(Enums.GenericCtl.OPUS_GET_SAMPLE_RATE_REQUEST, out int value);
                return value;
            }
        }

        /// <summary>
        /// Number of channels (1 or 2) in input signal.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Configures decoder gain adjustment.
        /// </summary>
        public int Gain
        {
            get
            {
                if (Decoder.IsClosed) return 0;
                DecoderCtl(Enums.DecoderCtl.OPUS_GET_GAIN_REQUEST, out int value);
                return value;
            }
            set
            {
                if (Decoder.IsClosed) return;
                DecoderCtl(Enums.DecoderCtl.OPUS_SET_GAIN_REQUEST, value);
            }
        }

        /// <summary>
        /// Gets the duration (in samples) of the last packet successfully decoded or concealed.
        /// </summary>
        public int LastPacketDuration
        {
            get
            {
                if (Decoder.IsClosed) return 0;
                DecoderCtl(Enums.DecoderCtl.OPUS_GET_LAST_PACKET_DURATION_REQUEST, out int value);
                return value;
            }
        }

        /// <summary>
        /// Gets the pitch of the last decoded frame, if available.
        /// </summary>
        public int Pitch
        {
            get
            {
                if (Decoder.IsClosed) return 0;
                DecoderCtl(Enums.DecoderCtl.OPUS_GET_PITCH_REQUEST, out int value);
                return value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and initializes an opus decoder.
        /// </summary>
        /// <param name="SampleRate">Sample rate to decode at (Hz). This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="Channels">Number of channels (1 or 2) to decode.</param>
        /// <exception cref="OpusException"></exception>
        public OpusDecoder(int SampleRate, int Channels)
        {
            Decoder = NativeOpus.opus_decoder_create(SampleRate, Channels, out var Error);
            CheckError((int)Error);

            this.Channels = Channels;
        }

        /// <summary>
        /// Decodes an Opus packet.
        /// </summary>
        /// <param name="input">Input payload. Use a NULL pointer to indicate packet loss.</param>
        /// <param name="inputLength">Number of bytes in payload.</param>
        /// <param name="output">Output signal (interleaved if 2 channels). length is frame_size*channels*sizeof(short).</param>
        /// <param name="frame_size">Number of samples per channel of available space in pcm. If this is less than the maximum packet duration (120ms; 5760 for 48kHz), this function will not be capable of decoding some packets. In the case of PLC (data==NULL) or FEC (decode_fec=1), then frame_size needs to be exactly the duration of audio that is missing, otherwise the decoder will not be in the optimal state to decode the next incoming packet. For the PLC and FEC cases, frame_size must be a multiple of 2.5 ms.</param>
        /// <param name="decodeFEC">Flag (false or true) to request that any in-band forward error correction data be decoded. If no such data is available, the frame is decoded as if it were lost.</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the decoded packet on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Decode(byte[] input, int inputLength, byte[] output, int frame_size, bool decodeFEC = false, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = output)
                result = NativeOpus.opus_decode(Decoder, inPtr + inputOffset, inputLength, outPtr + outputOffset, frame_size / 2, decodeFEC ? 1 : 0);
            CheckError(result);
            return result * sizeof(short) * Channels;
        }

        /// <summary>
        /// Decodes an Opus packet.
        /// </summary>
        /// <param name="input">Input payload. Use a NULL pointer to indicate packet loss.</param>
        /// <param name="inputLength">Number of bytes in payload.</param>
        /// <param name="output">Output signal (interleaved if 2 channels). length is frame_size*channels.</param>
        /// <param name="frame_size">Number of samples per channel of available space in pcm. If this is less than the maximum packet duration (120ms; 5760 for 48kHz), this function will not be capable of decoding some packets. In the case of PLC (data==NULL) or FEC (decode_fec=1), then frame_size needs to be exactly the duration of audio that is missing, otherwise the decoder will not be in the optimal state to decode the next incoming packet. For the PLC and FEC cases, frame_size must be a multiple of 2.5 ms.</param>
        /// <param name="decodeFEC">Flag (false or true) to request that any in-band forward error correction data be decoded. If no such data is available, the frame is decoded as if it were lost.</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the decoded packet on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Decode(byte[] input, int inputLength, short[] output, int frame_size, bool decodeFEC = false, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            byte[] byteOutput = new byte[output.Length * 2]; //Short to byte is 2 bytes.
            Buffer.BlockCopy(byteOutput, 0, byteOutput, 0, output.Length);

            int result = 0;
            fixed (byte* inPtr = input)
            fixed (byte* outPtr = byteOutput)
                result = NativeOpus.opus_decode(Decoder, inPtr + inputOffset, inputLength, outPtr + outputOffset, frame_size, decodeFEC ? 1 : 0);
            CheckError(result);
            Buffer.BlockCopy(byteOutput, 0, output, 0, output.Length);
            return result * Channels;
        }

        /// <summary>
        /// Decodes an Opus frame.
        /// </summary>
        /// <param name="input">Input in float format (interleaved if 2 channels), with a normal range of +/-1.0. Samples with a range beyond +/-1.0 are supported but will be clipped by decoders using the integer API and should only be used if it is known that the far end supports extended dynamic range. length is frame_size*channels*sizeof(float)</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="output">Output payload</param>
        /// <param name="inputOffset">Offset to start reading in the input.</param>
        /// <param name="outputOffset">Offset to start writing in the output.</param>
        /// <returns>The length of the decoded packet on success or a negative error code (see <see cref="Enums.OpusError"/>) on failure. Note: OpusSharp throws an error if there is a negative error code.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int DecodeFloat(byte[] input, int inputLength, float[] output, int frame_size, bool decodeFEC = false, int inputOffset = 0, int outputOffset = 0)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* inPtr = input)
            fixed (float* outPtr = output)
                result = NativeOpus.opus_decode_float(Decoder, inPtr + inputOffset, inputLength, outPtr + outputOffset, frame_size, decodeFEC ? 1 : 0);
            CheckError(result);
            return result * Channels;
        }

        /// <summary>
        /// Requests a CTL on the decoder.
        /// </summary>
        /// <param name="ctl">The decoder CTL to request.</param>
        /// <param name="value">The value to input.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void DecoderCtl(Enums.DecoderCtl ctl, int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_decoder_ctl(Decoder, (int)ctl, value));
        }

        /// <summary>
        /// Requests a CTL on the decoder.
        /// </summary>
        /// <param name="ctl">The decoder CTL to request.</param>
        /// <param name="value">The value that is outputted from the CTL.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void DecoderCtl(Enums.DecoderCtl ctl, out int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_decoder_ctl(Decoder, (int)ctl, out int val));
            value = val;
        }

        /// <summary>
        /// Requests a CTL on the decoder.
        /// </summary>
        /// <param name="ctl">The decoder CTL to request.</param>
        /// <param name="value">The value to input.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void DecoderCtl(Enums.GenericCtl ctl, int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_decoder_ctl(Decoder, (int)ctl, value));
        }

        /// <summary>
        /// Requests a CTL on the decoder.
        /// </summary>
        /// <param name="ctl">The decoder CTL to request.</param>
        /// <param name="value">The value that is outputted from the CTL.</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public void DecoderCtl(Enums.GenericCtl ctl, out int value)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_decoder_ctl(Decoder, (int)ctl, out int val));
            value = val;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Decoder.IsClosed)
                    Decoder.Dispose();
            }
        }

        /// <summary>
        /// Checks if the object is disposed and throws.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (Decoder.IsClosed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        public static unsafe Enums.PreDefCtl GetBandwidth(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_bandwidth(dataPtr);

            CheckError(result);
            return (Enums.PreDefCtl)result;
        }


        public static unsafe int GetSamplesPerFrame(byte[] data, int Fs)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_samples_per_frame(dataPtr, Fs);

            return result;
        }

        public static unsafe int GetNumberOfChannels(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_nb_channels(dataPtr);

            CheckError(result);
            return result;
        }

        public static unsafe int GetNumberOfFrames(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_nb_frames(dataPtr, data.Length);

            CheckError(result);
            return result;
        }

        public static unsafe int GetNumberOfSamples(byte[] data, int Fs)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_nb_samples(dataPtr, data.Length, Fs);

            CheckError(result);
            return result;
        }

        public static unsafe bool HasLbrr(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_has_lbrr(dataPtr, data.Length);

            CheckError(result);
            return result == 1;
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
