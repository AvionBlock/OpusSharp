using System;
using System.Runtime.InteropServices;

namespace OpusSharp.Core.SafeHandlers
{
    public class OpusDREDSafeHandle : SafeHandle
    {
        public OpusDREDSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_dred_free(handle);
            return true;
        }
    }
}
