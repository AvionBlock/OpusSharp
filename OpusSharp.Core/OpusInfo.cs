using OpusSharp.Core.Enums;
using System;
using System.Runtime.InteropServices;

namespace OpusSharp.Core
{
    public static class OpusInfo
    {
        /// <summary>
        /// Gets the libopus version string.
        /// </summary>
        /// <returns>The version.</returns>
        public static unsafe string NativeVersion()
        {
            return Marshal.PtrToStringAnsi((IntPtr)NativeOpus.opus_get_version_string());
        }

        /// <summary>
        /// Converts an opus error code into a human readable string.
        /// </summary>
        /// <param name="error">The error number.</param>
        /// <returns>The error information.</returns>
        public static unsafe string OpusStringError(OpusError error)
        {
            return Marshal.PtrToStringAnsi((IntPtr)NativeOpus.opus_strerror((int)error));
        }
    }
}
