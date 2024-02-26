using System.Runtime.InteropServices;
using System;

namespace OpusSharp.SafeHandlers
{
    internal class OpusEncoderSafeHandle : SafeHandle
    {
        public OpusEncoderSafeHandle(): base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_decoder_destroy(handle);
            return true;
        }
    }
}
