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
                return DecoderCtl(Enums.DecoderCtl.OPUS_GET_GAIN_REQUEST);
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
                return DecoderCtl(Enums.DecoderCtl.OPUS_GET_LAST_PACKET_DURATION_REQUEST);
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
                return DecoderCtl(Enums.DecoderCtl.OPUS_GET_PITCH_REQUEST);
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
            return result * sizeof(short);
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
#pragma warning disable CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
            Buffer.BlockCopy(byteOutput, 0, output, 0, output.Length);
#pragma warning restore CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
            return result;
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
            return result;
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
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public int DecoderCtl(Enums.DecoderCtl ctl)
        {
            ThrowIfDisposed();

            CheckError(NativeOpus.opus_decoder_ctl(Decoder, (int)ctl, out int val));
            return val;
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

        /// <summary>
        /// Gets the number of samples of an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet</param>
        /// <returns>Number of samples.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public int GetNumberOfSamples(byte[] data)
        {
            ThrowIfDisposed();

            var result = NativeOpus.opus_decoder_get_nb_samples(Decoder, data, data.Length);

            CheckError(result);
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Decoder.IsClosed)
                    Decoder.Close();
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

        /// <summary>
        /// Gets the bandwidth of an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet</param>
        /// <returns>The bandwidth.</returns>
        /// <exception cref="OpusException"></exception>
        public static unsafe Enums.PreDefCtl GetBandwidth(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_bandwidth(dataPtr);

            CheckError(result);
            return (Enums.PreDefCtl)result;
        }

        /// <summary>
        /// Gets the number of samples per frame from an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet. This must contain at least one byte of data.</param>
        /// <param name="Fs">Sampling rate in Hz. This must be a multiple of 400, or inaccurate results will be returned.</param>
        /// <returns>Number of samples per frame.</returns>
        public static unsafe int GetSamplesPerFrame(byte[] data, int Fs)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_samples_per_frame(dataPtr, Fs);

            return result;
        }

        /// <summary>
        /// Gets the number of channels from an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet</param>
        /// <returns>Number of channels</returns>
        /// <exception cref="OpusException"></exception>
        public static unsafe int GetNumberOfChannels(byte[] data)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_get_nb_channels(dataPtr);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Gets the number of frames in an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet.</param>
        /// <returns>Number of frames.</returns>
        /// <exception cref="OpusException"></exception>
        public static int GetNumberOfFrames(byte[] data)
        {
            var result = NativeOpus.opus_packet_get_nb_frames(data, data.Length);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Gets the number of samples of an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet</param>
        /// <param name="Fs">Sampling rate in Hz. This must be a multiple of 400, or inaccurate results will be returned.</param>
        /// <returns>Number of samples.</returns>
        /// <exception cref="OpusException"></exception>
        public static int GetNumberOfSamples(byte[] data, int Fs)
        {
            var result = NativeOpus.opus_packet_get_nb_samples(data, data.Length, Fs);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Checks whether an Opus packet has LBRR.
        /// </summary>
        /// <param name="data">Opus packet</param>
        /// <returns>Wether the LBRR is present.</returns>
        /// <exception cref="OpusException"></exception>
        public static unsafe bool HasLbrr(byte[] data)
        {
            var result = NativeOpus.opus_packet_has_lbrr(data, data.Length);

            CheckError(result);
            return result == 1;
        }

        /// <summary>
        /// Parse an opus packet into one or more frames. THIS FUNCTION IS NOT WORKING, DO NOT USE THIS FUNCTION UNTIL IT IS FIXED/FIGURED OUT.
        /// </summary>
        /// <param name="data">Opus packet to be parsed</param>
        /// <param name="out_toc">TOC pointer</param>
        /// <param name="frames">encapsulated frames</param>
        /// <param name="size">sizes of the encapsulated frames</param>
        /// <param name="payloadOffset">returns the position of the payload within the packet (in bytes)</param>
        /// <returns>number of frames.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static unsafe int Parse(byte[] data, out byte out_toc, out byte[] frames, out short[] size, out int payloadOffset)
        {
            throw new NotImplementedException();

            /*
            frames = new byte[48];
            size = new short[48];

            var result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_parse(dataPtr, data.Length, out out_toc, out frames, out size, out payloadOffset);

            return result;
            */
        }

        /// <summary>
        /// Applies soft-clipping to bring a float signal within the [-1,1] range.
        /// </summary>
        /// <param name="data">Input PCM and modified PCM</param>
        /// <param name="channels">Number of channels</param>
        /// <param name="softclipMem">State memory for the soft clipping process (one float per channel, initialized to zero)</param>
        public static unsafe void PcmSoftClip(float[] data, int channels, out float[] softclipMem)
        {
            softclipMem = new float[channels];
            fixed (float* dataPtr = data)
            fixed (float* softclipMemPtr = softclipMem)
                NativeOpus.opus_pcm_soft_clip(dataPtr, data.Length, channels, softclipMemPtr);
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
