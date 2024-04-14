using System;

namespace OpusSharp.Core
{
    public class OpusException : Exception
    {
        public OpusException() { }

        public OpusException(string message) : base(message) { }

        public OpusException(string message, Exception innerException) : base(message, innerException) { }
    }
}
