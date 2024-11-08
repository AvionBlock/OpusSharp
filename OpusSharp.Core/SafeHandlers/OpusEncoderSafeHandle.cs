using System.Runtime.InteropServices;
using System;

namespace OpusSharp.Core.SafeHandlers
{
    public class OpusEncoderSafeHandle : SafeHandle
    {
        public OpusEncoderSafeHandle(): base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_encoder_destroy(handle);
            return true;
        }
    }
}
