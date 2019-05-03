// <copyright file="DisposableObject.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Llvm.NET.Interop
{
    /// <summary>Abstract base class for implementing the Disposable pattern</summary>
    public abstract class DisposableObject
        : IDisposable
    {
        /// <summary>Finalizes an instance of the <see cref="DisposableObject"/> class. This releases any unmanaged resources it owns</summary>
        ~DisposableObject( )
        {
            Dispose( false );
        }

        /// <inheritdoc/>
        [SuppressMessage( "Design", "CA1063:Implement IDisposable Correctly", Justification = "This guarantees dispose is idempotent" )]
        public void Dispose( )
        {
            bool needsDispose = Interlocked.Exchange( ref IsDisposed_, 1 ) == 0;
            if( needsDispose )
            {
                Dispose( true );
                GC.SuppressFinalize( this );
            }
        }

        /// <summary>Gets a value indicating whether the object is disposed or not</summary>
        public bool IsDisposed => IsDisposed_ != 0;

        /// <summary>Abstract method that is implemented by derived types to perform the dispose operation</summary>
        /// <param name="disposing">Indicates if this is a dispose or finalize operation</param>
        /// <remarks>
        /// This is guaranteed to only be called if <see cref="IsDisposed"/> returns <see langword="false"/>
        /// so the implementation should only be concerned with the actual release of resources. If <paramref name="disposing"/>
        /// is <see langword="true"/> then the implementation should release managed and unmanaged resources, otherwise it should
        /// only release the unmanaged resources
        /// </remarks>
        protected abstract void Dispose( bool disposing );

        private int IsDisposed_;
    }
}
