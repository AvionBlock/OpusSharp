namespace OpusSharp.Enums
{
    /// <summary>
    /// Specifies the intended applications.
    /// </summary>
    public enum Application
    {
        /// <summary>
        /// Best for most VoIP/videoconference applications where listening quality and intelligibility matter most.
        /// </summary>
        VOIP = 2048,
        /// <summary>
        /// Best for broadcast/high-fidelity application where the decoded audio should be as close as possible to the input.
        /// </summary>
        AUDIO = 2049,
        /// <summary>
        /// Only use when lowest-achievable latency is what matters most. Voice-optimized modes cannot be used.
        /// </summary>
        RESTRICTED_LOW_DELAY = 2051
    }
}
