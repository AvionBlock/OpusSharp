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
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_get_size(int channels);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusEncoderSafeHandle opus_encoder_create(int Fs, int application, int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_init(OpusEncoderSafeHandle st, int Fs, int channels, int application);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encode(OpusEncoderSafeHandle st, short* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encode_float(OpusEncoderSafeHandle st, float* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_encoder_destroy(IntPtr st);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_encoder_ctl(OpusEncoderSafeHandle st, int request, void* data);

        //Decoder
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_get_size(int channels);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDecoderSafeHandle opus_decoder_create(int Fs, int channels, int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_init(OpusDecoderSafeHandle st, int Fs, int channels);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decode(OpusDecoderSafeHandle st, byte* data, int len, short* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decode_float(OpusDecoderSafeHandle st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_ctl(OpusDecoderSafeHandle st, int request, void* data);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_decoder_destroy(IntPtr st);

        //Dred Decoder
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_decoder_get_size();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDREDDecoderSafeHandle opus_dred_decoder_create(int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_decoder_init(OpusDREDDecoderSafeHandle dec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_dred_decoder_destroy(IntPtr dec);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_dred_decoder_ctl(OpusDREDDecoderSafeHandle dred_dec, int request, void* data);

        //Dred Packet?
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_get_size();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern OpusDREDSafeHandle opus_dred_alloc(int* error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_dred_free(IntPtr dec); //I'M JUST FOLLOWING THE DOCS!

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_dred_parse(OpusDREDDecoderSafeHandle dred_dec, OpusDREDSafeHandle dred, byte* data, int len, int max_dred_samples, int sampling_rate, int* dred_end, int defer_processing);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_dred_process(OpusDREDDecoderSafeHandle dred_dec, OpusDREDSafeHandle src, OpusDREDSafeHandle dst);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_dred_decode(OpusDecoderSafeHandle st, OpusDREDSafeHandle dred, int dred_offset, int* pcm, int frame_size);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_decoder_dred_decode_float(OpusDecoderSafeHandle st, OpusDREDSafeHandle dred, int dred_offset, float* pcm, int frame_size);

        //Opus Packet Parsers
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_parse(byte* data, int len, byte* out_toc, byte* frames, short* size, int* payload_offset);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_bandwidth(byte* data);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_samples_per_frame(byte* data, int Fs);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_channels(byte* data);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_frames(byte* packet, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_samples(byte* packet, int len, int Fs);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_has_lbrr(byte* packet, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int opus_packet_get_nb_samples(OpusDecoderSafeHandle dec, byte* packet, int len);

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
        public static extern void opus_multistream_decoder_destroy(IntPtr st);

        //Library Information
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern byte* opus_strerror(int error);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern byte* opus_get_version_string();
    }
}
