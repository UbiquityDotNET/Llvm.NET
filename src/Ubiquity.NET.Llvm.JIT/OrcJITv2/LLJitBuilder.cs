// -----------------------------------------------------------------------
// <copyright file="LLJitBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.InteropProperties;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>ORC JIT v2 Builder</summary>
    public sealed class LLJitBuilder
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LLJitBuilder"/> class.</summary>
        public LLJitBuilder()
            : this(LLVMOrcCreateLLJITBuilder())
        {
        }

        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        /// <summary>Sets the target machine builder for this JIT builder</summary>
        /// <param name="targetMachineBuilder">Target machine builder for this JIT builder</param>
        public void SetTargetMachineBuilder(TargetMachineBuilder targetMachineBuilder)
        {
            ArgumentNullException.ThrowIfNull(targetMachineBuilder);
            LLVMOrcLLJITBuilderSetJITTargetMachineBuilder(Handle, targetMachineBuilder.Handle);
        }

        /// <summary>Sets the object linking layer factory for this builder</summary>
        /// <param name="creator">Factory to use for this builder</param>
        /// <remarks>
        /// If no factory is provided a default instance is used. This allows callers
        /// to customize the behavior if the default is insufficient.
        /// </remarks>
        public void SetObjectLinkingLayerCreator(IObjectLinkingLayerFactory creator)
        {
            ArgumentNullException.ThrowIfNull(creator);
            ObjectLinkingLayerContextHandle.ThrowIfHasValue();
            ObjectLinkingLayerContextHandle.Value = GCHandle.Alloc(creator);
            unsafe
            {
                LLVMOrcLLJITBuilderSetObjectLinkingLayerCreator(
                    Handle,
                    &NativeObjectLinkingLayerCreatorCallback,
                    (void*)GCHandle.ToIntPtr(ObjectLinkingLayerContextHandle.Value));
            }
        }

        /// <summary>Creates a JIT engine</summary>
        /// <param name="jit">The jit or <see langword="null"/></param>
        /// <returns>Status results of the operation</returns>
        /// <remarks>
        /// This function takes ownership of this builder (even on failures). Thus, it is
        /// not valid to use this instance after this method is called.
        /// </remarks>
        public ErrorInfo CreateJit(out LLJIT? jit)
        {
            ObjectDisposedException.ThrowIf(Handle.IsInvalid || Handle.IsClosed, this);
            jit = null;

            // This instance no longer owns the handle
            Handle.SetHandleAsInvalid();
#pragma warning disable CA2000 // Dispose objects before losing scope
            ErrorInfo retVal = new(LLVMOrcCreateLLJIT(out LLVMOrcLLJITRef jitHandle, Handle));
#pragma warning restore CA2000 // Dispose objects before losing scope
            if(retVal.Success)
            {
                jit = new LLJIT(jitHandle.Move()); // Wraps the jit Handle to transfer ownership (Dispose not needed)
            }
            else
            {
                // Assert the expectation that jitHandle is ALWAYS invalid on errors (So Dispose isn't needed)
                Debug.Assert(jitHandle.IsInvalid, "Expected an invalid handle on errors possible leak will result");
            }

            return retVal;
        }

        private LLJitBuilder(LLVMOrcLLJITBuilderRef h)
        {
            Handle = h;
        }

        private WriteOnce<GCHandle> ObjectLinkingLayerContextHandle = new();
        private readonly LLVMOrcLLJITBuilderRef Handle;

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe /*LLVMOrcObjectLayerRef*/ nint NativeObjectLinkingLayerCreatorCallback(void* context, nint sessionRef, byte* triple)
        {
            try
            {
                if(GCHandle.FromIntPtr((nint)context).Target is IObjectLinkingLayerFactory self)
                {
                    string managedTriple = ExecutionEncodingStringMarshaller.ConvertToManaged(triple) ?? string.Empty;

                    // caller takes ownership of the resulting handle; Don't Dispose it
                    ObjectLayer layer = self.Create(new ExecutionSession(sessionRef), managedTriple);
                    return layer.Handle.MoveToNative();
                }

                // TODO: How/Can this report an error? Internally the result is (via a C++ lambda) an "llvm::expected"
                // but it is unclear if this method can return an error or what happens on a null return...
                return 0; // This will probably crash in LLVM anyway - best effort.
            }
            catch
            {
                return 0; // This will probably crash in LLVM anyway - best effort.
            }
        }
    }
}
