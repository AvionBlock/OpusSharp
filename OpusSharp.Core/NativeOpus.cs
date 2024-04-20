using OpusSharp.Core.Enums;
using OpusSharp.Core.SafeHandlers;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace OpusSharp.Core
{
    internal static unsafe class NativeOpus
    {
#if __ANDROID__ || LINUX
        private const string DllName = "libopus.so";
#elif __IOS__ || __MACCATALYST__
        private const string DllName = "__Internal__";
#else
        private const string DllName = "opus";
#endif
        #region Encoder
        [DllImport(DllName, EntryPoint = "opus_encoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_get_size(int channels);

        [DllImport(DllName, EntryPoint = "opus_encoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusEncoderSafeHandle opus_encoder_create(int Fs, int channels, int application, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_encoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_init(OpusEncoderSafeHandle st, int Fs, int channels, int application);

        [DllImport(DllName, EntryPoint = "opus_encode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encode(OpusEncoderSafeHandle st, byte* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_encode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encode_float(OpusEncoderSafeHandle st, float* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_encoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_encoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_encoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_ctl(OpusEncoderSafeHandle st, int request, out int value);

        [DllImport(DllName, EntryPoint = "opus_encoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_ctl(OpusEncoderSafeHandle st, int request, int value);
        #endregion

        #region Decoder
        [DllImport(DllName, EntryPoint = "opus_decoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_get_size(int channels);

        [DllImport(DllName, EntryPoint = "opus_decoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusDecoderSafeHandle opus_decoder_create(int Fs, int channels, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_decoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_init(OpusDecoderSafeHandle st, int Fs, int channels);

        [DllImport(DllName, EntryPoint = "opus_decode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decode(OpusDecoderSafeHandle st, byte* data, int len, byte* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_decode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decode_float(OpusDecoderSafeHandle st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_decoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_ctl(OpusDecoderSafeHandle st, int request, int value);

        [DllImport(DllName, EntryPoint = "opus_decoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_ctl(OpusDecoderSafeHandle st, int request, out int value);

        [DllImport(DllName, EntryPoint = "opus_decoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_decoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_packet_parse", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_parse(byte* data, int len, out byte out_toc, out byte[] frames, out short[] size, out int payload_offset); //NEEDS FIXING

        [DllImport(DllName, EntryPoint = "opus_packet_get_bandwidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_bandwidth(byte* data);

        [DllImport(DllName, EntryPoint = "opus_packet_get_samples_per_frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_samples_per_frame(byte* data, int Fs);

        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_channels", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_nb_channels(byte* data);

        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_frames", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_nb_frames(byte[] packet, int len);

        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_samples", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_nb_samples(byte[] packet, int len, int Fs);

        [DllImport(DllName, EntryPoint = "opus_packet_has_lbrr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_has_lbrr(byte[] packet, int len);

        [DllImport(DllName, EntryPoint = "opus_decoder_get_nb_samples", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_get_nb_samples(OpusDecoderSafeHandle dec, byte[] packet, int len);

        [DllImport(DllName, EntryPoint = "opus_pcm_soft_clip", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_pcm_soft_clip(float* pcm, int frame_size, int channels, float* softclip_mem);
        #endregion

        #region Repacketizer
        [DllImport(DllName, EntryPoint = "opus_repacketizer_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_get_size();

        [DllImport(DllName, EntryPoint = "opus_repacketizer_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_init(OpusRepacketizerSafeHandle rp);

        [DllImport(DllName, EntryPoint = "opus_repacketizer_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusRepacketizerSafeHandle opus_repacketizer_create();

        [DllImport(DllName, EntryPoint = "opus_repacketizer_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_repacketizer_destroy(IntPtr rp);

        [DllImport(DllName, EntryPoint = "opus_repacketizer_cat", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_cat(OpusRepacketizerSafeHandle rp, byte* data, int len);

        [DllImport(DllName, EntryPoint = "opus_repacketizer_out_range", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_out_range(OpusRepacketizerSafeHandle rp, int begin, int end, byte* data, int maxlen);

        [DllImport(DllName, EntryPoint = "opus_repacketizer_get_nb_frames", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_get_nb_frames(OpusRepacketizerSafeHandle rp);

        [DllImport(DllName, EntryPoint = "opus_repacketizer_out", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_repacketizer_out(OpusRepacketizerSafeHandle rp, byte* data, int maxlen);

        [DllImport(DllName, EntryPoint = "opus_packet_pad", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_pad(byte* data, int len, int new_len);

        [DllImport(DllName, EntryPoint = "opus_packet_unpad", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_unpad(byte* data, int len);

        [DllImport(DllName, EntryPoint = "opus_multistream_packet_pad", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_packet_pad(byte* data, int len, int new_len, int nb_streams);

        [DllImport(DllName, EntryPoint = "opus_multistream_packet_unpad", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_packet_unpad(byte* data, int len, int nb_streams);
        #endregion

        #region MSEncoder
        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encoder_get_size(int streams, int coupled_streams);

        [DllImport(DllName, EntryPoint = "opus_multistream_surround_encoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_surround_encoder_get_size(int channels, int mapping_family);

        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusMSEncoderSafeHandle opus_multistream_encoder_create(int Fs, int channels, int streams, int coupled_streams, byte* mapping, int application, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encoder_init(OpusMSEncoderSafeHandle st, int Fs, int channels, int streams, int coupled_streams, byte* mapping, int application);

        [DllImport(DllName, EntryPoint = "opus_multistream_encode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encode(OpusMSEncoderSafeHandle st, byte* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_multistream_encode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encode_float(OpusMSEncoderSafeHandle st, float* pcm, int frame_size, byte* data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_multistream_encoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encoder_ctl(OpusMSEncoderSafeHandle st, int request, int value);

        [DllImport(DllName, EntryPoint = "opus_multistream_encoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_encoder_ctl(OpusMSEncoderSafeHandle st, int request, out int value);
        #endregion

        #region MSDecoder
        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decoder_get_size(int streams, int coupled_streams);

        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern OpusMSDecoderSafeHandle opus_multistream_decoder_create(int Fs, int channels, int streams, int coupled_streams, byte* mapping, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decoder_init(OpusMSDecoderSafeHandle st, int Fs, int channels, int streams, int coupled_streams, byte* mapping);

        [DllImport(DllName, EntryPoint = "opus_multistream_decode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decode(OpusMSDecoderSafeHandle st, byte* data, int len, byte* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_multistream_decode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decode_float(OpusMSDecoderSafeHandle st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_multistream_decoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decoder_ctl(OpusMSDecoderSafeHandle st, int request, int value);

        [DllImport(DllName, EntryPoint = "opus_multistream_decoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_multistream_decoder_ctl(OpusMSDecoderSafeHandle st, int request, out int value);
        #endregion

        #region LibInfo
        [DllImport(DllName, EntryPoint = "opus_get_version_string", CallingConvention = CallingConvention.Cdecl)]
        public static extern string opus_get_version_string();

        [DllImport(DllName, EntryPoint = "opus_strerror", CallingConvention = CallingConvention.Cdecl)]
        public static extern string opus_strerror(int error);
        #endregion
    }
}
