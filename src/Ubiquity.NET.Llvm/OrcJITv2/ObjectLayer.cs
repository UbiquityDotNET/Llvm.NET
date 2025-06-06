// -----------------------------------------------------------------------
// <copyright file="ObjectLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>ORC JIT v2 Object linking layer</summary>
    public sealed class ObjectLayer
        : IDisposable
    {
        /// <summary>Adds an object file to the specified library</summary>
        /// <param name="jitDyLib">Library to add the object file to</param>
        /// <param name="objBuffer">Buffer for the object file</param>
        /// <remarks>
        /// The <paramref name="objBuffer"/> is transferred to the native implementation
        /// of the library and is no longer usable after this (Dispose is still a NOP,
        /// but any other operation results in an <see cref="ObjectDisposedException"/>)
        /// </remarks>
        public void Add(JITDyLib jitDyLib, MemoryBuffer objBuffer)
        {
            using LLVMErrorRef errorRef = LLVMOrcObjectLayerAddObjectFile(Handle, jitDyLib.Handle, objBuffer.Handle);
            // Even if an error occurred, ownership is transferred
            objBuffer.Handle.SetHandleAsInvalid();
            errorRef.ThrowIfFailed();
        }

        /// <summary>Adds an object file to the specified <see cref="ResourceTracker"/></summary>
        /// <param name="rt">Tracker to add the object file to</param>
        /// <param name="objBuffer">Buffer for the object file</param>
        /// <remarks>
        /// The <paramref name="objBuffer"/> is transferred to the native implementation
        /// of the library and is no longer usable after this (Dispose is still a NOP,
        /// but any other operation results in an <see cref="ObjectDisposedException"/>)
        /// </remarks>
        public void Add(ResourceTracker rt, MemoryBuffer objBuffer)
        {
            using LLVMErrorRef errorRef = LLVMOrcObjectLayerAddObjectFileWithRT(Handle, rt.Handle, objBuffer.Handle);
            // Even if an error occurred, ownership is transferred
            objBuffer.Handle.SetHandleAsInvalid();
            errorRef.ThrowIfFailed();
        }

        /// <summary>Emits an object buffer to the object layer</summary>
        /// <param name="resp">Materialization Responsibility</param>
        /// <param name="objBuffer">Object buffer to emit</param>
        /// <remarks>
        /// Ownership of both parameters is transferred to the native code.
        /// Calling Dispose() on either after this is called is a NOP, other
        /// operations result in an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Emit(MaterializationResponsibility resp, MemoryBuffer objBuffer)
        {
            LLVMOrcObjectLayerEmit(Handle, resp.Handle, objBuffer.Handle);
            resp.Handle.SetHandleAsInvalid();
            objBuffer.Handle.SetHandleAsInvalid();
        }

        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        internal ObjectLayer( LLVMOrcObjectLayerRef h)
        {
            Handle = h.Move();
        }

        internal LLVMOrcObjectLayerRef Handle { get; init; }
    }
}
