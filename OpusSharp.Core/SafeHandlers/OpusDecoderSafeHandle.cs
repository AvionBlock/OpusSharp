using System.Runtime.InteropServices;
using System;
using System.Runtime.ConstrainedExecution;

namespace OpusSharp.Core.SafeHandlers
{
    internal class OpusDecoderSafeHandle : SafeHandle
    {
        public OpusDecoderSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_decoder_destroy(handle);
            return true;
        }
    }
}
