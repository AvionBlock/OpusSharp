using System.Runtime.InteropServices;
using System;

namespace OpusSharp.Core.SafeHandlers
{
    public class OpusDecoderSafeHandle : SafeHandle
    {
        public OpusDecoderSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeHandler.opus_decoder_destroy(handle);
            return true;
        }
    }
}
