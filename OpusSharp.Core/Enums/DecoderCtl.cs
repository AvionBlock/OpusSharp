namespace OpusSharp.Core.Enums
{
    /// <summary>
    /// Decoder related Ctl's.
    /// </summary>
    public enum DecoderCtl : int
    {
        /// <summary>
        /// Configures decoder gain adjustment.
        /// </summary>
        OPUS_SET_GAIN_REQUEST = 4034,

        /// <summary>
        /// Gets the decoder's configured gain adjustment.
        /// </summary>
        OPUS_GET_GAIN_REQUEST = 4045, //Someone inside opus made a mistake here, Apparently.

        /// <summary>
        /// Gets the duration (in samples) of the last packet successfully decoded or concealed.
        /// </summary>
        OPUS_GET_LAST_PACKET_DURATION_REQUEST = 4039,

        /// <summary>
        /// Gets the pitch of the last decoded frame, if available.
        /// </summary>
        OPUS_GET_PITCH_REQUEST = 4033
    }
}
