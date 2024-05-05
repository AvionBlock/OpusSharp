/////////////////////////////////////////////////////////////////////////////////
// paint.net                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, and contributors.                  //
// All Rights Reserved.                                                        //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace OpusSharp.Core
{
    /// <summary>
    /// Provides a standard implementation of IDisposable and IIsDisposed.
    /// </summary>
    [Serializable]
    public abstract class Disposable : IDisposable
    {
        private int isDisposed; // 0 for false, 1 for true

        /// <summary>
        /// Returns wether the object is disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get => Volatile.Read(ref this.isDisposed) != 0;
        }

        /// <summary>
        /// Makes an object disposable.
        /// </summary>
        protected Disposable()
        {
        }

        /// <summary>
        /// When the object is destructed and not disposed, it does not dispose.
        /// </summary>
        ~Disposable()
        {
            int oldIsDisposed = Interlocked.Exchange(ref this.isDisposed, 1);
            if (oldIsDisposed == 0)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            int oldIsDisposed = Interlocked.Exchange(ref this.isDisposed, 1);
            if (oldIsDisposed == 0)
            {
                try
                {
                    Dispose(true);
                }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}

// https://gist.github.com/rickbrew/fc3e660c0930747f031e64ab7696c60d