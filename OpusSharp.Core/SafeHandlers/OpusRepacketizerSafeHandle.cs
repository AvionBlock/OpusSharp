using System.Runtime.ConstrainedExecution;
using System;
using System.Runtime.InteropServices;

namespace OpusSharp.Core.SafeHandlers
{
    internal class OpusRepacketizerSafeHandle : SafeHandle
    {
        public OpusRepacketizerSafeHandle() : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            NativeOpus.opus_repacketizer_destroy(handle);
            return true;
        }
    }
}
