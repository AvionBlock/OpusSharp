namespace OpusSharp.Core.Extensions
{
    /// <summary>
    /// Contains the OpusDecoder helper extensions.
    /// </summary>
    public static class OpusDecoderExtensions
    {
        /// <summary>
        /// Sets gain for an opus decoder.
        /// </summary>
        /// <param name="decoder">The decoder state.</param>
        /// <param name="gain">The gain to set.</param>
        public static void SetGain(this OpusDecoder decoder, short gain)
        {
            decoder.Ctl(DecoderCTL.OPUS_SET_GAIN, gain);
        }

        /// <summary>
        /// Gets the gain for an opus decoder.
        /// </summary>
        /// <param name="decoder">The decoder state.</param>
        /// <returns>The gain for the opus decoder.</returns>
        public static int GetGain(this OpusDecoder decoder)
        {
            var gain = 0;
            decoder.Ctl(DecoderCTL.OPUS_GET_GAIN, ref gain);
            return gain;
        }

        /// <summary>
        /// Gets the duration (in samples) of the last packet successfully decoded or concealed.
        /// </summary>
        /// <param name="decoder">The decoder state.</param>
        /// <returns>The last packet duration (in samples).</returns>
        public static int GetLastPacketDuration(this OpusDecoder decoder)
        {
            var lastPacketDuration = 0;
            decoder.Ctl(DecoderCTL.OPUS_GET_LAST_PACKET_DURATION, ref lastPacketDuration);
            return lastPacketDuration;
        }

        /// <summary>
        /// Gets the pitch of the last decoded frame, if available.
        /// </summary>
        /// <param name="decoder">The decoder state.</param>
        /// <returns>The pitch of the last decoded frame if available.</returns>
        public static int GetPitch(this OpusDecoder decoder)
        {
            var pitch = 0;
            decoder.Ctl(DecoderCTL.OPUS_GET_PITCH, ref pitch);
            return pitch;
        }

        /// <summary>
        /// Gets the final state of the codec's entropy coder.
        /// </summary>
        /// <param name="decoder">The decoder state.</param>
        /// <returns>The final state of the codec's entropy coder.</returns>
        public static uint GetFinalRange(this OpusDecoder decoder)
        {
            var finalRange = 0u;
            decoder.Ctl(GenericCTL.OPUS_GET_FINAL_RANGE, ref finalRange);
            return finalRange;
        }
    }
}