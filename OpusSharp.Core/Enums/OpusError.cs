namespace OpusSharp.Core.Enums
{
    /// <summary>
    /// Specifies the type of opus error.
    /// </summary>
    public enum OpusError : int
    {
        /// <summary>
        /// No error. This does not throw the <seealso cref="OpusException"/>
        /// </summary>
        OK = 0,
        /// <summary>
        /// One or more invalid/out of range arguments.
        /// </summary>
        BAD_ARG = -1,
        /// <summary>
        /// Not enough bytes allocated in the buffer.
        /// </summary>
        BUFFER_TOO_SMALL = -2,
        /// <summary>
        /// An internal error was detected.
        /// </summary>
        INTERNAL_ERROR = -3,
        /// <summary>
        /// The compressed data passed is corrupted.
        /// </summary>
        INVALID_PACKET = -4,
        /// <summary>
        /// Invalid/unsupported request number.
        /// </summary>
        UNIMPLEMENTED = -5,
        /// <summary>
        /// An encoder or decoder structure is invalid or already freed.
        /// </summary>
        INVALID_STATE = -6,
        /// <summary>
        /// Memory allocation has failed.
        /// </summary>
        ALLOC_FAIL = -7
    }
}
