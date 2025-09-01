// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm
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
        /// <remarks>
        /// This implementation guarantees that Dispose is an idempotent call and does NOT trigger any exceptions. That is,
        /// if already disposed, derived types will never see a call to <see cref="Dispose(bool)"/> as this will silently
        /// treat it as a NOP. Additionally, this follows the standard pattern such that finalization is suppressed when
        /// disposing.
        /// </remarks>
        [SuppressMessage( "Design", "CA1063:Implement IDisposable Correctly", Justification = "This guarantees dispose is idempotent, and therefore superior to the official pattern" )]
        public void Dispose( )
        {
#if NET9_0_OR_GREATER
            if(!Interlocked.Exchange( ref IsDisposed_, true ))
            {
                Dispose( true );
                GC.SuppressFinalize( this );
            }
#else
            if(Interlocked.CompareExchange( ref IsDisposed_, 1, 0 ) == 0)
            {
                Dispose( true );
                GC.SuppressFinalize( this );
            }
#endif
        }

// .NET 9 adds support for Interlocked.CompareExchange() for a bool
#if NET9_0_OR_GREATER
        /// <summary>Gets a value indicating whether the object is disposed or not</summary>
        public bool IsDisposed => IsDisposed_;
#else
        /// <summary>Gets a value indicating whether the object is disposed or not</summary>
        public bool IsDisposed => IsDisposed_ != 0;
#endif

        /// <summary>Abstract method that is implemented by derived types to perform the dispose operation</summary>
        /// <param name="disposing">Indicates if this is a dispose or finalize operation</param>
        /// <remarks>
        /// This is guaranteed to only be called if <see cref="IsDisposed"/> returns <see langword="false"/>
        /// so the implementation should only be concerned with the actual release of resources. If <paramref name="disposing"/>
        /// is <see langword="true"/> then the implementation should release managed and unmanaged resources, otherwise it should
        /// only release the unmanaged resources
        /// </remarks>
        protected virtual void Dispose( bool disposing )
        {
        }

// .NET 9 adds support for Interlocked.CompareExchange() for a bool
#if NET9_0_OR_GREATER
        // do not write directly to this field, it should only be done with interlocked calls in Dispose() to ensure correct behavior
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Indicates the field should never be directly written to" )]
        [SuppressMessage( "Style", "IDE0044:Add readonly modifier", Justification = "Modified by Interlocked expression" )]
        private bool IsDisposed_;
#else
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Indicates the field should never be directly written to" )]
        [SuppressMessage( "Style", "IDE0044:Add readonly modifier", Justification = "Modified by Interlocked expression" )]
        private int IsDisposed_ = 0;
#endif
    }
}
