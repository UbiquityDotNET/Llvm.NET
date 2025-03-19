// -----------------------------------------------------------------------
// <copyright file="ResourceTracker.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Resource tracker</summary>
    public sealed class ResourceTracker
        : IDisposable
    {
        /// <summary>Gets a value indicating whether this instance is disposed or not</summary>
        public bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if this instance is already disposed</summary>
        public void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(IsDisposed, this);

        /// <summary>Moves all resources associated with this tracker to <paramref name="other"/></summary>
        /// <param name="other">Destination tracker to receive all the resources</param>
        public void MoveTo(ResourceTracker other)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(other);

            LLVMOrcResourceTrackerTransferTo(Handle, other.Handle);
        }

        /// <summary>Removes all resources managed by this tracker</summary>
        public void RemoveAll()
        {
            using var errorRef = LLVMOrcResourceTrackerRemove(Handle);
            errorRef.ThrowIfFailed();
        }

        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        internal ResourceTracker(LLVMOrcResourceTrackerRef h)
        {
            Handle = h.Move();
        }

        internal LLVMOrcResourceTrackerRef Handle { get; }
    }
}
