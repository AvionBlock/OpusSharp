﻿using OpusSharp.Enums;
using System;
using System.Runtime.InteropServices;

namespace OpusSharp
{
    internal static class NativeOpus
    {
#if ANDROID
        private const string DllName = "libopus-share.so";
#elif IOS
        private const string DllName = "__Internal";
#else
        private const string DllName = "opus";
#endif
        #region Encoder
        [DllImport(DllName, EntryPoint = "opus_encoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_get_size(int channels);

        [DllImport(DllName, EntryPoint = "opus_encoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr opus_encoder_create(int Fs, int channels, int application, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_encoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_init(IntPtr st, int Fs, int channels, int application);

        [DllImport(DllName, EntryPoint = "opus_encode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encode(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_encode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encode_float(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        [DllImport(DllName, EntryPoint = "opus_encoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_encoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_encoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encoder_ctl(IntPtr st, int request, int value);
        #endregion

        #region Decoder
        [DllImport(DllName, EntryPoint = "opus_decoder_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_get_size(int channels);

        [DllImport(DllName, EntryPoint = "opus_decoder_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr opus_decoder_create(int Fs, int channels, out OpusError error);

        [DllImport(DllName, EntryPoint = "opus_decoder_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_init(IntPtr st, int Fs, int channels);

        [DllImport(DllName, EntryPoint = "opus_decode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decode(IntPtr st, IntPtr data, int len, IntPtr pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_decode_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decode_float(IntPtr st, IntPtr data, int len, IntPtr pcm, int frame_size, int decode_fec);

        [DllImport(DllName, EntryPoint = "opus_decoder_ctl", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_decoder_ctl(IntPtr st, int request, int value);

        [DllImport(DllName, EntryPoint = "opus_decoder_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_decoder_destroy(IntPtr st);

        [DllImport(DllName, EntryPoint = "opus_packet_parse", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_parse(IntPtr data, int len, IntPtr out_toc, IntPtr frames, short size, IntPtr payload_offset);

        [DllImport(DllName, EntryPoint = "opus_packet_get_bandwidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_bandwidth(IntPtr data);

        [DllImport(DllName, EntryPoint = "opus_packet_get_samples_per_frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_get_samples_per_frame(IntPtr data, int Fs);

        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_channels", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_nb_channels(IntPtr data);

        /* No Idea
        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_frames", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_nb_frames(IntPtr data);
        */

        /* No Idea
        [DllImport(DllName, EntryPoint = "opus_packet_get_nb_samples", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_nb_samples(IntPtr data, int len);
        */

        /* No Idea
        [DllImport(DllName, EntryPoint = "opus_decoder_get_nb_samples", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_packet_nb_samples(IntPtr dec, IntPtr packet, int len);
        */

        [DllImport(DllName, EntryPoint = "opus_pcm_soft_clip", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_pcm_soft_clip(IntPtr pcm, int frame_size, int channels, IntPtr softclip_mem);
        #endregion
    }
}