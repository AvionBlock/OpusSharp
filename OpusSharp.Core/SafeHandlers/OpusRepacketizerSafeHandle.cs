using System;
using System.Runtime.InteropServices;

namespace OpusSharp.Core.SafeHandlers
{
    public class OpusRepacketizerSafeHandle : SafeHandle
    {
        public OpusRepacketizerSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeHandler.opus_repacketizer_destroy(handle);
            return true;
        }
    }
}
