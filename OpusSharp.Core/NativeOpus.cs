using OpusSharp.Core.SafeHandlers;
using System;
using System.Runtime.InteropServices;
namespace OpusSharp.Core
{
    /// <summary>
    /// Native opus handler that directly calls the exported opus functions.
    /// </summary>
    public static class NativeOpus
    {
#if ANDROID
        private const string DllName = "libopus.so";
#elif LINUX
        private const string DllName = "libopus.so.0.10.1";
#elif WINDOWS
        private const string DllName = "opus.dll";
#elif MACOS || IOS || MACCATALYST
        private const string DllName = "__Internal__";
#else
        private const string DllName = "opus";
#endif

        //Encoder
        /// <summary>
        /// Gets the size of an <see cref="OpusEncoderSafeHandle"/> structure.
        /// </summary>
        /// <param name="channels">Number of channels. This must be 1 or 2.</param>
        /// <returns>The size in bytes.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_get_size(int channels);

        /// <summary>
        /// Allocates and initializes an encoder state.
        /// </summary>
        /// <param name="Fs">Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="channels">Number of channels (1 or 2) in input signal.</param>
        /// <param name="application">Coding mode (one of <see cref="OpusPredefinedValues.OPUS_APPLICATION_VOIP"/>, <see cref="OpusPredefinedValues.OPUS_APPLICATION_AUDIO"/> or <see cref="OpusPredefinedValues.OPUS_APPLICATION_RESTRICTED_LOWDELAY"/>)</param>
        /// <param name="error"><see cref="OpusErrorCodes.OPUS_OK"/> Success or <see cref="OpusErrorCodes"/>.</param>
        /// <returns><see cref="OpusEncoderSafeHandle"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusEncoderSafeHandle opus_encoder_create(int Fs, int channels, int application, int* error);

        /// <summary>
        /// Initializes a previously allocated encoder state. The memory pointed to by st must be at least the size returned by opus_encoder_get_size().
        /// </summary>
        /// <param name="st">Encoder state</param>
        /// <param name="Fs">Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="channels">Number of channels (1 or 2) in input signal.</param>
        /// <param name="application">>Coding mode (one of <see cref="OpusPredefinedValues.OPUS_APPLICATION_VOIP"/>, <see cref="OpusPredefinedValues.OPUS_APPLICATION_AUDIO"/> or <see cref="OpusPredefinedValues.OPUS_APPLICATION_RESTRICTED_LOWDELAY"/>)</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_init(OpusEncoderSafeHandle st, int Fs, int channels, int application);

        /// <summary>
        /// Encodes an Opus frame.
        /// </summary>
        /// <param name="st">Encoder state</param>
        /// <param name="pcm">Input signal (interleaved if 2 channels). length is frame_size*channels*sizeof(short)</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="data">Output payload. This must contain storage for at least max_data_bytes.</param>
        /// <param name="max_data_bytes">Size of the allocated memory for the output payload. This may be used to impose an upper limit on the instant bitrate, but should not be used as the only bitrate control. Use <see cref="EncoderCTL.OPUS_SET_BITRATE"/> to control the bitrate.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see <see cref="OpusErrorCodes"/>) on failure.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encode(OpusEncoderSafeHandle st, short* pcm, int frame_size, byte* data, int max_data_bytes);

        /// <summary>
        /// Encodes an Opus frame from floating point input.
        /// </summary>
        /// <param name="st">Encoder state</param>
        /// <param name="pcm">Input in float format (interleaved if 2 channels), with a normal range of +/-1.0. Samples with a range beyond +/-1.0 are supported but will be clipped by decoders using the integer API and should only be used if it is known that the far end supports extended dynamic range. length is frame_size*channels*sizeof(float)</param>
        /// <param name="frame_size">Number of samples per channel in the input signal. This must be an Opus frame size for the encoder's sampling rate. For example, at 48 kHz the permitted values are 120, 240, 480, 960, 1920, and 2880. Passing in a duration of less than 10 ms (480 samples at 48 kHz) will prevent the encoder from using the LPC or hybrid modes.</param>
        /// <param name="data">Output payload. This must contain storage for at least max_data_bytes.</param>
        /// <param name="max_data_bytes">Size of the allocated memory for the output payload. This may be used to impose an upper limit on the instant bitrate, but should not be used as the only bitrate control. Use <see cref="EncoderCTL.OPUS_SET_BITRATE"/> to control the bitrate.</param>
        /// <returns>The length of the encoded packet (in bytes) on success or a negative error code (see <see cref="OpusErrorCodes"/>) on failure.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encode_float(OpusEncoderSafeHandle st, float* pcm, int frame_size, byte* data, int max_data_bytes);

        /// <summary>
        /// Frees an OpusEncoder allocated by <see cref="opus_encoder_create(int, int, int, int*)"/>.
        /// </summary>
        /// <param name="st">State to be freed.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_encoder_destroy(IntPtr st);

        /// <summary>
        /// Perform a CTL function on an Opus encoder.
        /// </summary>
        /// <param name="st">Encoder state.</param>
        /// <param name="request">This and all remaining parameters should be replaced by one of the convenience macros in <see cref="GenericCTL"/> or <see cref="EncoderCTL"/>.</param>
        /// <param name="data">The data to input or output. MUST BE FIXED!</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encoder_ctl(OpusEncoderSafeHandle st, int request, void* data);

        /// <summary>
        /// Perform a CTL function on an Opus encoder.
        /// </summary>
        /// <param name="st">Encoder state.</param>
        /// <param name="request">This and all remaining parameters should be replaced by one of the convenience macros in <see cref="GenericCTL"/> or <see cref="EncoderCTL"/>.</param>
        /// <param name="data">The data to input or output. MUST BE FIXED!</param>
        /// <param name="data2">The second data to input or output. MUST BE FIXED!</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encoder_ctl(OpusEncoderSafeHandle st, int request, void* data, void* data2);

        //Decoder
        /// <summary>
        /// Gets the size of an <see cref="OpusDecoderSafeHandle"/> structure.
        /// </summary>
        /// <param name="channels">Number of channels. This must be 1 or 2.</param>
        /// <returns>The size in bytes.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_get_size(int channels);

        /// <summary>
        /// Allocates and initializes a decoder state.
        /// </summary>
        /// <param name="Fs">Sample rate to decode at (Hz). This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="channels">Number of channels (1 or 2) to decode.</param>
        /// <param name="error"><see cref="OpusErrorCodes.OPUS_OK"/> Success or <see cref="OpusErrorCodes"/>.</param>
        /// <returns><see cref="OpusDecoderSafeHandle"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDecoderSafeHandle opus_decoder_create(int Fs, int channels, int* error);

        /// <summary>
        /// Initializes a previously allocated decoder state.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="Fs">Sampling rate to decode to (Hz). This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="channels">Number of channels (1 or 2) to decode.</param>
        /// <returns><see cref="OpusErrorCodes.OPUS_OK"/> Success or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_init(OpusDecoderSafeHandle st, int Fs, int channels);

        /// <summary>
        /// Decode an Opus packet.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="data">Input payload. Use a NULL pointer to indicate packet loss.</param>
        /// <param name="len">Number of bytes in payload.</param>
        /// <param name="pcm">Output signal (interleaved if 2 channels). length is frame_size*channels*sizeof(short).</param>
        /// <param name="frame_size">Number of samples per channel of available space in pcm. If this is less than the maximum packet duration (120ms; 5760 for 48kHz), this function will not be capable of decoding some packets. In the case of PLC (data==NULL) or FEC (decode_fec=1), then frame_size needs to be exactly the duration of audio that is missing, otherwise the decoder will not be in the optimal state to decode the next incoming packet. For the PLC and FEC cases, frame_size must be a multiple of 2.5 ms.</param>
        /// <param name="decode_fec">Flag (0 or 1) to request that any in-band forward error correction data be decoded. If no such data is available, the frame is decoded as if it were lost.</param>
        /// <returns>Number of decoded samples or <see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decode(OpusDecoderSafeHandle st, byte* data, int len, short* pcm, int frame_size, int decode_fec);

        /// <summary>
        /// Decode an Opus packet with floating point output.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="data">Input payload. Use a NULL pointer to indicate packet loss.</param>
        /// <param name="len">Number of bytes in payload.</param>
        /// <param name="pcm">Output signal (interleaved if 2 channels). length is frame_size*channels*sizeof(float).</param>
        /// <param name="frame_size">Number of samples per channel of available space in pcm. If this is less than the maximum packet duration (120ms; 5760 for 48kHz), this function will not be capable of decoding some packets. In the case of PLC (data==NULL) or FEC (decode_fec=1), then frame_size needs to be exactly the duration of audio that is missing, otherwise the decoder will not be in the optimal state to decode the next incoming packet. For the PLC and FEC cases, frame_size must be a multiple of 2.5 ms.</param>
        /// <param name="decode_fec">Flag (0 or 1) to request that any in-band forward error correction data be decoded. If no such data is available the frame is decoded as if it were lost.</param>
        /// <returns>Number of decoded samples or <see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decode_float(OpusDecoderSafeHandle st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

        /// <summary>
        /// Perform a CTL function on an Opus decoder.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="request">This and all remaining parameters should be replaced by one of the convenience macros in <see cref="GenericCTL"/> or <see cref="DecoderCTL"/>.</param>
        /// <param name="data">The data to input or output. MUST BE FIXED!</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_ctl(OpusDecoderSafeHandle st, int request, void* data);

        /// <summary>
        /// Frees an OpusDecoder allocated by <see cref="opus_decoder_create(int, int, int*)"/>.
        /// </summary>
        /// <param name="st">State to be freed.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_decoder_destroy(IntPtr st);

        //Dred Decoder
        /// <summary>
        /// Gets the size of an <see cref="OpusDREDDecoderSafeHandle"/> structure.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_decoder_get_size();

        /// <summary>
        /// Allocates and initializes an <see cref="OpusDREDDecoderSafeHandle"/> state.
        /// </summary>
        /// <param name="error"><see cref="OpusErrorCodes.OPUS_OK"/> Success or <see cref="OpusErrorCodes"/>.</param>
        /// <returns><see cref="OpusDREDDecoderSafeHandle"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDREDDecoderSafeHandle opus_dred_decoder_create(int* error);

        /// <summary>
        /// Initializes an <see cref="OpusDREDDecoderSafeHandle"/> state.
        /// </summary>
        /// <param name="dec">State to be initialized.</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_decoder_init(OpusDREDDecoderSafeHandle dec);

        /// <summary>
        /// Frees an OpusDREDDecoder allocated by <see cref="opus_dred_decoder_create(int*)"/>.
        /// </summary>
        /// <param name="dec">State to be freed.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_dred_decoder_destroy(IntPtr dec);

        /// <summary>
        /// Perform a CTL function on an Opus DRED decoder.
        /// </summary>
        /// <param name="dred_dec">DRED Decoder state.</param>
        /// <param name="request">This and all remaining parameters should be replaced by one of the convenience macros in <see cref="GenericCTL"/> or <see cref="DecoderCTL"/>.</param>
        /// <param name="data">The data to input or output. MUST BE FIXED!</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_dred_decoder_ctl(OpusDREDDecoderSafeHandle dred_dec, int request, void* data);

        //Dred Packet?
        /// <summary>
        /// Gets the size of an OpusDRED structure.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_get_size();

        /// <summary>
        /// Allocates and initializes a <see cref="OpusDREDSafeHandle"/> state.
        /// </summary>
        /// <param name="error"><see cref="OpusErrorCodes.OPUS_OK"/> Success or <see cref="OpusErrorCodes"/>.</param>
        /// <returns><see cref="OpusDREDSafeHandle"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDREDSafeHandle opus_dred_alloc(int* error);

        /// <summary>
        /// Frees an OpusDRED allocated by <see cref="opus_dred_alloc(int*)"/>.
        /// </summary>
        /// <param name="dec">State to be freed.</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_dred_free(IntPtr dec); //I'M JUST FOLLOWING THE DOCS!

        /// <summary>
        /// Decode an Opus DRED packet.
        /// </summary>
        /// <param name="dred_dec">DRED Decoder state.</param>
        /// <param name="dred">DRED state.</param>
        /// <param name="data">Input payload.</param>
        /// <param name="len">Number of bytes in payload.</param>
        /// <param name="max_dred_samples">Maximum number of DRED samples that may be needed (if available in the packet).</param>
        /// <param name="sampling_rate">Sampling rate used for max_dred_samples argument. Needs not match the actual sampling rate of the decoder.</param>
        /// <param name="dred_end">Number of non-encoded (silence) samples between the DRED timestamp and the last DRED sample.</param>
        /// <param name="defer_processing">Flag (0 or 1). If set to one, the CPU-intensive part of the DRED decoding is deferred until <see cref="opus_dred_process(OpusDREDDecoderSafeHandle, OpusDREDSafeHandle, OpusDREDSafeHandle)"/> is called.</param>
        /// <returns>Offset (positive) of the first decoded DRED samples, zero if no DRED is present, or <see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_dred_parse(OpusDREDDecoderSafeHandle dred_dec, OpusDREDSafeHandle dred, byte* data, int len, int max_dred_samples, int sampling_rate, int* dred_end, int defer_processing);

        /// <summary>
        /// Finish decoding an Opus DRED packet.
        /// </summary>
        /// <param name="dred_dec">DRED Decoder state.</param>
        /// <param name="src">Source DRED state to start the processing from.</param>
        /// <param name="dst">Destination DRED state to store the updated state after processing.</param>
        /// <returns><see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_process(OpusDREDDecoderSafeHandle dred_dec, OpusDREDSafeHandle src, OpusDREDSafeHandle dst);

        /// <summary>
        /// Decode audio from an Opus DRED packet with floating point output.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="dred">DRED state.</param>
        /// <param name="dred_offset">position of the redundancy to decode (in samples before the beginning of the real audio data in the packet).</param>
        /// <param name="pcm">Output signal (interleaved if 2 channels). length is frame_size*channels*sizeof(short)</param>
        /// <param name="frame_size">Number of samples per channel to decode in pcm. frame_size must be a multiple of 2.5 ms.</param>
        /// <returns>Number of decoded samples or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_dred_decode(OpusDecoderSafeHandle st, OpusDREDSafeHandle dred, int dred_offset, int* pcm, int frame_size);

        /// <summary>
        /// Decode audio from an Opus DRED packet with floating point output.
        /// </summary>
        /// <param name="st">Decoder state.</param>
        /// <param name="dred">DRED state.</param>
        /// <param name="dred_offset">position of the redundancy to decode (in samples before the beginning of the real audio data in the packet).</param>
        /// <param name="pcm">Output signal (interleaved if 2 channels). length is frame_size*channels*sizeof(float).</param>
        /// <param name="frame_size">Number of samples per channel to decode in pcm. frame_size must be a multiple of 2.5 ms.</param>
        /// <returns>Number of decoded samples or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_dred_decode_float(OpusDecoderSafeHandle st, OpusDREDSafeHandle dred, int dred_offset, float* pcm, int frame_size);

        //Opus Packet Parsers
        /// <summary>
        /// Parse an opus packet into one or more frames.
        /// </summary>
        /// <param name="data">Opus packet to be parsed.</param>
        /// <param name="len">size of data.</param>
        /// <param name="out_toc">TOC pointer.</param>
        /// <param name="frames">encapsulated frames.</param>
        /// <param name="size">sizes of the encapsulated frames.</param>
        /// <param name="payload_offset">returns the position of the payload within the packet (in bytes).</param>
        /// <returns>number of frames.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_parse(byte* data, int len, byte* out_toc, byte*[] frames, short* size, int* payload_offset);

        /// <summary>
        /// Gets the bandwidth of an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet.</param>
        /// <returns><see cref="OpusPredefinedValues"/> or <see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_bandwidth(byte* data);

        /// <summary>
        /// Gets the number of samples per frame from an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet. This must contain at least one byte of data.</param>
        /// <param name="Fs">Sampling rate in Hz. This must be a multiple of 400, or inaccurate results will be returned.</param>
        /// <returns>Number of samples per frame.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_samples_per_frame(byte* data, int Fs);

        /// <summary>
        /// Gets the number of channels from an Opus packet.
        /// </summary>
        /// <param name="data">Opus packet.</param>
        /// <returns>Number of channels or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_channels(byte* data);

        /// <summary>
        /// Gets the number of frames in an Opus packet.
        /// </summary>
        /// <param name="packet">Opus packet.</param>
        /// <param name="len">Length of packet.</param>
        /// <returns>Number of frames or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_frames(byte* packet, int len);

        /// <summary>
        /// Gets the number of samples of an Opus packet.
        /// </summary>
        /// <param name="packet">Opus packet.</param>
        /// <param name="len">Length of packet.</param>
        /// <param name="Fs">Sampling rate in Hz. This must be a multiple of 400, or inaccurate results will be returned.</param>
        /// <returns>Number of samples or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_samples(byte* packet, int len, int Fs);

        /// <summary>
        /// Checks whether an Opus packet has LBRR.
        /// </summary>
        /// <param name="packet">Opus packet.</param>
        /// <param name="len">Length of packet.</param>
        /// <returns>1 is LBRR is present, 0 otherwise or <see cref="OpusErrorCodes"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_has_lbrr(byte* packet, int len);

        /// <summary>
        /// Gets the number of samples of an Opus packet.
        /// </summary>
        /// <param name="dec">Decoder state.</param>
        /// <param name="packet">Opus packet.</param>
        /// <param name="len">Length of packet.</param>
        /// <returns>Number of samples or <see cref="OpusErrorCodes"/>.</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_get_nb_samples(OpusDecoderSafeHandle dec, byte* packet, int len);

        /// <summary>
        /// Applies soft-clipping to bring a float signal within the [-1,1] range.
        /// </summary>
        /// <param name="pcm">Input PCM and modified PCM.</param>
        /// <param name="frame_size">Number of samples per channel to process.</param>
        /// <param name="channels">Number of channels.</param>
        /// <param name="softclip_mem">State memory for the soft clipping process (one float per channel, initialized to zero).</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void opus_pcm_soft_clip(float* pcm, int frame_size, int channels, float* softclip_mem);

        //Repacketizer
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_get_size();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusRepacketizerSafeHandle opus_repacketizer_init(OpusRepacketizerSafeHandle rp);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusRepacketizerSafeHandle opus_repacketizer_create();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_repacketizer_destroy(IntPtr rp);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_repacketizer_cat(OpusRepacketizerSafeHandle rp, byte* data, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_repacketizer_out_range(OpusRepacketizerSafeHandle rp, int begin, int end, byte* data, int maxlen);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_get_nb_frames(OpusRepacketizerSafeHandle rp);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_repacketizer_out(OpusRepacketizerSafeHandle rp, byte* data, int maxlen);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_pad(byte* data, int len, int new_len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_unpad(byte* data, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_packet_pad(byte* data, int len, int new_len, int nb_streams);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_packet_unpad(byte* data, int len, int nb_streams);

        //Multistream Encoder
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encoder_get_size(int streams, int coupled_streams);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_surround_encoder_get_size(int channels, int mapping_family);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusMSEncoderSafeHandle opus_multistream_encoder_create(int Fs, int channels, int streams, int coupled_streams, byte* mapping, int application, int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusMSEncoderSafeHandle opus_multistream_surround_encoder_create(int Fs, int channels, int mapping_family, int* streams, int* coupled_streams, byte* mapping, int application, int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_encoder_init(OpusMSEncoderSafeHandle st, int Fs, int channels, int streams, int coupled_streams, byte* mapping, int application);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_surround_encoder_init(OpusMSEncoderSafeHandle st, int Fs, int channels, int mapping_family, int* streams, int* coupled_streams, byte* mapping, int application);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_encode(OpusMSEncoderSafeHandle st, byte* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_encode_float(OpusMSEncoderSafeHandle st, float* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_multistream_encoder_destroy(IntPtr st);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_encoder_ctl(OpusMSEncoderSafeHandle st, int request, void* data);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_encoder_ctl(OpusMSEncoderSafeHandle st, int request, void* data, void* data2);

        //Multistream Decoder
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decoder_get_size(int streams, int coupled_streams);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusMSDecoderSafeHandle opus_multistream_decoder_create(int Fs, int channels, int streams, int coupled_streams, byte* mapping, int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_decoder_init(OpusMSDecoderSafeHandle st, int Fs, int channels, int streams, int coupled_streams, byte* mapping);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_decode(OpusMSDecoderSafeHandle st, byte* data, int len, short* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_decode_float(OpusMSDecoderSafeHandle st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_decoder_ctl(OpusMSDecoderSafeHandle st, int request, void* data);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_multistream_decoder_ctl(OpusMSDecoderSafeHandle st, int request, void* data, void* data2);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_multistream_decoder_destroy(IntPtr st);

        //Library Information
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern byte* opus_strerror(int error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern byte* opus_get_version_string();
    }
}
