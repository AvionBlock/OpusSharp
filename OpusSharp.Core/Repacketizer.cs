using OpusSharp.Core.SafeHandlers;
using System;

namespace OpusSharp.Core
{
    /// <summary>
    /// The repacketizer can be used to merge multiple Opus packets into a single packet or alternatively to split Opus packets that have previously been merged.
    /// </summary>
    public class Repacketizer : Disposable
    {
        private readonly OpusRepacketizerSafeHandle Repacker;

        #region Methods
        /// <summary>
        /// Creates and initializes an opus repacketizer.
        /// </summary>
        public Repacketizer()
        {
            Repacker = NativeOpus.opus_repacketizer_create();
        }

        /// <summary>
        /// (Re)initializes a previously allocated repacketizer state.
        /// </summary>
        public void ReInit()
        {
            NativeOpus.opus_repacketizer_init(Repacker);
        }

        /// <summary>
        /// Add a packet to the current repacketizer state.
        /// </summary>
        /// <param name="data">The packet data.</param>
        /// <returns>The number of bytes in the packet data.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Concat(byte[] data)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_repacketizer_cat(Repacker, dataPtr, data.Length);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Return the total number of frames contained in packet data submitted to the repacketizer state so far via <seealso cref="Concat(byte[])"/> since the last call to <seealso cref="ReInit()"/> or on construction.
        /// </summary>
        /// <returns>The total number of frames contained in the packet data submitted to the repacketizer state.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public int GetNumberOfFrames()
        {
            ThrowIfDisposed();
            return NativeOpus.opus_repacketizer_get_nb_frames(Repacker);
        }

        /// <summary>
        /// Construct a new packet from data previously submitted to the repacketizer state via <seealso cref="Concat(byte[])"/>
        /// </summary>
        /// <param name="data">The buffer in which to store the output packet.</param>
        /// <returns>The total size of the output packet on success, or an error code on failure.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int Out(byte[] data)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_repacketizer_out(Repacker, dataPtr, data.Length);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Construct a new packet from data previously submitted to the repacketizer state via <seealso cref="Concat(byte[])"/>
        /// </summary>
        /// <param name="begin">The index of the first frame in the current repacketizer state to include in the output.</param>
        /// <param name="end">One past the index of the last frame in the current repacketizer state to include in the output.</param>
        /// <param name="data">The buffer in which to store the output packet.</param>
        /// <returns>The total size of the output packet on success, or an <see cref="Enums.OpusError"/> on failure.</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="OpusException"></exception>
        public unsafe int OutRange(int begin, int end, byte[] data)
        {
            ThrowIfDisposed();

            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_repacketizer_out_range(Repacker, begin, end, dataPtr, data.Length);

            CheckError(result);
            return result;
        }

        /// <summary>
        /// Checks if the object is disposed and throws.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (Repacker.IsClosed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Repacker.IsClosed)
                    Repacker.Close();
            }
        }

        /// <summary>
        /// Pads a given Opus packet to a larger size (possibly changing the TOC sequence).
        /// </summary>
        /// <param name="data">The buffer containing the packet to pad.</param>
        /// <param name="len">The size of the packet. This must be at least 1.</param>
        /// <param name="new_len">The desired size of the packet after padding. This must be at least as large as len.</param>
        /// <exception cref="OpusException"></exception>
        public static unsafe void Pad(byte[] data, int len, int new_len)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_pad(dataPtr, len, new_len);

            CheckError(result);
        }

        /// <summary>
        /// Remove all padding from a given Opus packet and rewrite the TOC sequence to minimize space usage.
        /// </summary>
        /// <param name="data">The buffer containing the packet to strip.</param>
        /// <param name="len">The size of the packet. This must be at least 1.</param>
        /// <exception cref="OpusException"></exception>
        public static unsafe void Unpad(byte[] data, int len)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_packet_unpad(dataPtr, len);

            CheckError(result);
        }

        /// <summary>
        /// Pads a given Opus packet to a larger size (possibly changing the TOC sequence).
        /// </summary>
        /// <param name="data">The buffer containing the packet to pad.</param>
        /// <param name="len">The size of the packet. This must be at least 1.</param>
        /// <param name="new_len">The desired size of the packet after padding. This must be at least as large as len.</param>
        /// <param name="nb_streams">The number of streams (not channels) in the packet. This must be at least as large as len.</param>
        /// <exception cref="OpusException"></exception>
        public static unsafe void MultiStreamPad(byte[] data, int len, int new_len, int nb_streams)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_multistream_packet_pad(dataPtr, len, new_len, nb_streams);

            CheckError(result);
        }

        /// <summary>
        /// Remove all padding from a given Opus packet and rewrite the TOC sequence to minimize space usage.
        /// </summary>
        /// <param name="data">The buffer containing the packet to strip.</param>
        /// <param name="len">The size of the packet. This must be at least 1.</param>
        /// <param name="nb_streams">The number of streams (not channels) in the packet. This must be at least 1.</param>
        /// <exception cref="OpusException"></exception>
        public static unsafe void MultiStreamUnpad(byte[] data, int len, int nb_streams)
        {
            int result = 0;
            fixed (byte* dataPtr = data)
                result = NativeOpus.opus_multistream_packet_unpad(dataPtr, len, nb_streams);

            CheckError(result);
        }
        #endregion

        /// <summary>
        /// Check's for an opus error and throws if there is one.
        /// </summary>
        /// <param name="result"></param>
        /// <exception cref="OpusException"></exception>
        protected static void CheckError(int result)
        {
            if (result < 0)
                throw new OpusException(((Enums.OpusError)result).ToString());
        }
    }
}
