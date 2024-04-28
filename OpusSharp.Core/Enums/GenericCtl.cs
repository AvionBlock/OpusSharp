namespace OpusSharp.Core.Enums
{
    /// <summary>
    /// These macros are used with the Decoder and Encoder CTL calls to generate a particular request.
    /// </summary>
    public enum GenericCtl : int
    {
        /// <summary>
        /// Resets the codec state to be equivalent to a freshly initialized state.
        /// </summary>
        OPUS_RESET_STATE = 4028,

        /// <summary>
        /// Gets the final state of the codec's entropy coder.
        /// </summary>
        OPUS_GET_FINAL_RANGE_REQUEST = 4031,

        /// <summary>
        /// Gets the encoder's configured bandpass or the decoder's last bandpass.
        /// </summary>
        OPUS_GET_BANDWIDTH_REQUEST = 4009,

        /// <summary>
        /// Gets the sampling rate the encoder or decoder was initialized with.
        /// </summary>
        OPUS_GET_SAMPLE_RATE_REQUEST = 4029,

        /// <summary>
        /// If set to 1, disables the use of phase inversion for intensity stereo, improving the quality of mono downmixes, but slightly reducing normal stereo quality.
        /// </summary>
        OPUS_SET_PHASE_INVERSION_DISABLED_REQUEST = 4046,

        /// <summary>
        /// Gets the encoder's configured phase inversion status.
        /// </summary>
        OPUS_GET_PHASE_INVERSION_DISABLED_REQUEST = 4047,

        /// <summary>
        /// Gets the DTX state of the encoder.
        /// </summary>
        OPUS_GET_IN_DTX_REQUEST = 4049
    }
}
