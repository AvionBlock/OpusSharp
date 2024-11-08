using System;
using System.Runtime.InteropServices;

namespace OpusSharp.Core.SafeHandlers
{
    public class OpusDREDDecoderSafeHandle : SafeHandle
    {
        public OpusDREDDecoderSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_dred_decoder_destroy(handle);
            return true;
        }
    }
}
