namespace OpusSharp.Core.Enums
{
    /// <summary>
    /// Pre-defined values for CTL interface.
    /// </summary>
    public enum PreDefCtl : int
    {
        /// <summary>
        /// Auto/default setting.
        /// </summary>
        OPUS_AUTO = -1000,

        /// <summary>
        /// Maximum bitrate.
        /// </summary>
        OPUS_BITRATE_MAX = -1,

        /// <summary>
        /// Best for most VoIP/videoconference applications where listening quality and intelligibility matter most.
        /// </summary>
        OPUS_APPLICATION_VOIP = 2048,

        /// <summary>
        /// Best for broadcast/high-fidelity application where the decoded audio should be as close as possible to the input.
        /// </summary>
        OPUS_APPLICATION_AUDIO = 2049,

        /// <summary>
        /// Only use when lowest-achievable latency is what matters most. Voice-optimized modes cannot be used.
        /// </summary>
        OPUS_APPLICATION_RESTRICTED_LOWDELAY = 2051,

        /// <summary>
        /// Signal being encoded is voice.
        /// </summary>
        OPUS_SIGNAL_VOICE = 3001,

        /// <summary>
        /// Signal being encoded is music.
        /// </summary>
        OPUS_SIGNAL_MUSIC = 3002,

        /// <summary>
        /// 4 kHz bandpass.
        /// </summary>
        OPUS_BANDWIDTH_NARROWBAND = 1101,

        /// <summary>
        /// 6 kHz bandpass.
        /// </summary>
        OPUS_BANDWIDTH_MEDIUMBAND = 1102,

        /// <summary>
        /// 8 kHz bandpass.
        /// </summary>
        OPUS_BANDWIDTH_WIDEBAND = 1103,

        /// <summary>
        /// 12 kHz bandpass.
        /// </summary>
        OPUS_BANDWIDTH_SUPERWIDEBAND = 1104,

        /// <summary>
        /// 20 kHz bandpass.
        /// </summary>
        OPUS_BANDWIDTH_FULLBAND = 1105,

        /// <summary>
        /// Select frame size from the argument (default).
        /// </summary>
        OPUS_FRAMESIZE_ARG = 5000,

        /// <summary>
        /// Use 2.5 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_2_5_MS = 5001,

        /// <summary>
        /// Use 5 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_5_MS = 5002,

        /// <summary>
        /// Use 10 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_10_MS = 5003,

        /// <summary>
        /// Use 20 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_20_MS = 5004,

        /// <summary>
        /// Use 40 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_40_MS = 5005,

        /// <summary>
        /// Use 60 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_60_MS = 5006,

        /// <summary>
        /// Use 80 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_80_MS = 5007,

        /// <summary>
        /// Use 100 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_100_MS = 5008,

        /// <summary>
        /// Use 120 ms frames.
        /// </summary>
        OPUS_FRAMESIZE_120_MS = 5009
    }
}
