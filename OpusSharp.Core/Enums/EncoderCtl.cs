namespace OpusSharp.Core.Enums
{
    /// <summary>
    /// Encoder related CTL's.
    /// </summary>
    public enum EncoderCtl : int
    {
        /// <summary>
        /// Configures the encoder's computational complexity.
        /// </summary>
        OPUS_SET_COMPLEXITY_REQUEST = 4010,

        /// <summary>
        /// Gets the encoder's complexity configuration.
        /// </summary>
        OPUS_GET_COMPLEXITY_REQUEST = 4011,

        /// <summary>
        /// Configures the bitrate in the encoder.
        /// </summary>
        OPUS_SET_BITRATE_REQUEST = 4002,

        /// <summary>
        /// Gets the encoder's bitrate configuration.
        /// </summary>
        OPUS_GET_BITRATE_REQUEST = 4003,

        /// <summary>
        /// Enables or disables variable bitrate (VBR) in the encoder.
        /// </summary>
        OPUS_SET_VBR_REQUEST = 4006,

        /// <summary>
        /// Determine if variable bitrate (VBR) is enabled in the encoder.
        /// </summary>
        OPUS_GET_VBR_REQUEST = 4007,

        /// <summary>
        /// Enables or disables variable bitrate (VBR) in the encoder.
        /// </summary>
        OPUS_SET_VBR_CONSTRAINT_REQUEST = 4020,

        /// <summary>
        /// Determine if constrained VBR is enabled in the encoder.
        /// </summary>
        OPUS_GET_VBR_CONSTRAINT_REQUEST = 4021,

        /// <summary>
        /// Configures mono/stereo forcing in the encoder.
        /// </summary>
        OPUS_SET_FORCE_CHANNELS_REQUEST = 4022,

        /// <summary>
        /// Gets the encoder's forced channel configuration.
        /// </summary>
        OPUS_GET_FORCE_CHANNELS_REQUEST = 4023,

        /// <summary>
        /// Configures the maximum bandpass that the encoder will select automatically.
        /// </summary>
        OPUS_SET_MAX_BANDWIDTH_REQUEST = 4004,

        /// <summary>
        /// Gets the encoder's configured maximum allowed bandpass.
        /// </summary>
        OPUS_GET_MAX_BANDWIDTH_REQUEST = 4005,

        /// <summary>
        /// Sets the encoder's bandpass to a specific value.
        /// </summary>
        OPUS_SET_BANDWIDTH_REQUEST = 4008,

        /// <summary>
        /// Configures the type of signal being encoded.
        /// </summary>
        OPUS_SET_SIGNAL_REQUEST = 4024,

        /// <summary>
        /// Gets the encoder's configured signal type.
        /// </summary>
        OPUS_GET_SIGNAL_REQUEST = 4025,

        /// <summary>
        /// Configures the encoder's intended application.
        /// </summary>
        OPUS_SET_APPLICATION_REQUEST = 4000,

        /// <summary>
        /// Gets the encoder's configured application.
        /// </summary>
        OPUS_GET_APPLICATION_REQUEST = 4001,

        /// <summary>
        /// Gets the total samples of delay added by the entire codec.
        /// </summary>
        OPUS_GET_LOOKAHEAD_REQUEST = 4027,

        /// <summary>
        /// Configures the encoder's use of inband forward error correction (FEC).
        /// </summary>
        OPUS_SET_INBAND_FEC_REQUEST = 4012,

        /// <summary>
        /// Gets encoder's configured use of inband forward error correction.
        /// </summary>
        OPUS_GET_INBAND_FEC_REQUEST = 4013,

        /// <summary>
        /// Configures the encoder's expected packet loss percentage.
        /// </summary>
        OPUS_SET_PACKET_LOSS_PERC_REQUEST = 4014,

        /// <summary>
        /// Gets the encoder's configured packet loss percentage.
        /// </summary>
        OPUS_GET_PACKET_LOSS_PERC_REQUEST = 4015,

        /// <summary>
        /// Configures the encoder's use of discontinuous transmission (DTX).
        /// </summary>
        OPUS_SET_DTX_REQUEST = 4016,

        /// <summary>
        /// Gets encoder's configured use of discontinuous transmission.
        /// </summary>
        OPUS_GET_DTX_REQUEST = 4017,

        /// <summary>
        /// Configures the depth of signal being encoded.
        /// </summary>
        OPUS_SET_LSB_DEPTH_REQUEST = 4036,

        /// <summary>
        /// Gets the encoder's configured signal depth.
        /// </summary>
        OPUS_GET_LSB_DEPTH_REQUEST = 4037,

        /// <summary>
        /// Configures the encoder's use of variable duration frames.
        /// </summary>
        OPUS_SET_EXPERT_FRAME_DURATION_REQUEST = 4040,

        /// <summary>
        /// Gets the encoder's configured use of variable duration frames.
        /// </summary>
        OPUS_GET_EXPERT_FRAME_DURATION_REQUEST = 4041,

        /// <summary>
        /// If set to 1, disables almost all use of prediction, making frames almost completely independent.
        /// </summary>
        OPUS_SET_PREDICTION_DISABLED_REQUEST = 4042,

        /// <summary>
        /// Gets the encoder's configured prediction status.
        /// </summary>
        OPUS_GET_PREDICTION_DISABLED_REQUEST = 4043,

        /// <summary>
        /// If non-zero, enables Deep Redundancy (DRED) and use the specified maximum number of 10-ms redundant frames.
        /// </summary>
        OPUS_SET_DRED_DURATION_REQUEST = 4050,

        /// <summary>
        /// Gets the encoder's configured Deep Redundancy (DRED) maximum number of frames.
        /// </summary>
        OPUS_GET_DRED_DURATION_REQUEST = 4051,

        /// <summary>
        /// Provide external DNN weights from binary object (only when explicitly built without the weights).
        /// </summary>
        OPUS_SET_DNN_BLOB_REQUEST = 4052
    }
}
