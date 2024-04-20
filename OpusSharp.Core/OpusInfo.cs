using OpusSharp.Core.Enums;

namespace OpusSharp.Core
{
    public static class OpusInfo
    {
        /// <summary>
        /// Gets the libopus version string.
        /// </summary>
        /// <returns>The version.</returns>
        public static string NativeVersion()
        {
            return NativeOpus.opus_get_version_string();
        }

        /// <summary>
        /// Converts an opus error code into a human readable string.
        /// </summary>
        /// <param name="error">The error number.</param>
        /// <returns>The error information.</returns>
        public static string OpusStringError(OpusError error)
        {
            return NativeOpus.opus_strerror((int)error);
        }
    }
}
