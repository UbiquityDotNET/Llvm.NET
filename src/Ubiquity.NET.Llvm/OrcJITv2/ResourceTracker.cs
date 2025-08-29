// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Resource tracker</summary>
    public sealed class ResourceTracker
        : IDisposable
    {
        /// <summary>Gets a value indicating whether this instance is disposed or not</summary>
        public bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if this instance is already disposed</summary>
        public void ThrowIfDisposed( ) => ObjectDisposedException.ThrowIf( IsDisposed, this );

        /// <summary>Moves all resources associated with this tracker to <paramref name="other"/></summary>
        /// <param name="other">Destination tracker to receive all the resources</param>
        public void MoveTo( ResourceTracker other )
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull( other );

            LLVMOrcResourceTrackerTransferTo( Handle, other.Handle );
        }

        /// <summary>Removes all resources managed by this tracker</summary>
        public void RemoveAll( )
        {
            using var errorRef = LLVMOrcResourceTrackerRemove(Handle);
            errorRef.ThrowIfFailed();
        }

        /// <inheritdoc/>
        public void Dispose( ) => Handle.Dispose();

        internal ResourceTracker( LLVMOrcResourceTrackerRef h )
        {
            Handle = h.Move();
        }

        internal LLVMOrcResourceTrackerRef Handle { get; }
    }
}
