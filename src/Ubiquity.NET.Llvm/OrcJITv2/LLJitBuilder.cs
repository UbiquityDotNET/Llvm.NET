// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.LLJIT;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Delegate for an object layer factory</summary>
    /// <param name="session">Session to create the layer for</param>
    /// <param name="triple">Triple to create the layer for</param>
    /// <returns>new <see cref="ObjectLayer"/> instance</returns>
    /// <remarks>
    /// Ownership of the returned layer is transferred to the caller.
    /// </remarks>
    public delegate ObjectLayer ObjectLayerFactory( in ExecutionSession session, in Triple triple );

    /// <summary>ORC JIT v2 Builder</summary>
    public sealed class LLJitBuilder
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LLJitBuilder"/> class</summary>
        /// <param name="builder">Target machine builder to use for this instance</param>
        public LLJitBuilder( TargetMachineBuilder builder )
            : this( LLVMOrcCreateLLJITBuilder() )
        {
            SetTargetMachineBuilder( builder );
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            // ensure move semantics work by making this Idempotent.
            // If handle is null this instance does not own it (or the context)
            if(!Handle.IsNull)
            {
                Handle.Dispose();
                unsafe
                {
                    if(ObjectLinkingLayerContext is not null)
                    {
                        // If the callback context handle exists and is allocated then free it now.
                        // The handle for this builder itself is already closed so LLVM should not
                        // have any callbacks for this to handle... (docs are silent on the point)
                        NativeContext.Release(ObjectLinkingLayerContext);
                        ObjectLinkingLayerContext = null;
                    }
                }

                InvalidateAfterMove();
            }
        }

        /// <summary>Sets the target machine builder for this JIT builder</summary>
        /// <param name="targetMachineBuilder">Target machine builder for this JIT builder</param>
        public void SetTargetMachineBuilder( TargetMachineBuilder targetMachineBuilder )
        {
            ArgumentNullException.ThrowIfNull( targetMachineBuilder );
            LLVMOrcLLJITBuilderSetJITTargetMachineBuilder( Handle, targetMachineBuilder.Handle );

            // Ownership transferred to native code.
            targetMachineBuilder.InvalidateAfterMove();
        }

        /// <summary>Sets the object linking layer factory for this builder</summary>
        /// <param name="creator">Factory to use for this builder</param>
        /// <remarks>
        /// If no factory is provided a default instance is used. This allows callers
        /// to customize the behavior if the default is insufficient.
        /// </remarks>
        public void SetObjectLinkingLayerCreator( ObjectLayerFactory creator )
        {
            ArgumentNullException.ThrowIfNull( creator );
            unsafe
            {
                ObjectLinkingLayerContext = creator.AsNativeContext();

                LLVMOrcLLJITBuilderSetObjectLinkingLayerCreator(
                    Handle,
                    &NativeObjectLinkingLayerCreatorCallback,
                    ObjectLinkingLayerContext
                    );
            }
        }

        /// <summary>Creates a JIT engine</summary>
        /// <returns>Status results of the operation</returns>
        /// <remarks>
        /// This function takes ownership of this builder (even on failures/exceptions).
        /// Thus, after this method is called, it is not valid to use this instance except to
        /// call <see cref="Dispose"/>, which is a NOP.
        /// </remarks>
        public LLJit CreateJit( )
        {
            ObjectDisposedException.ThrowIf( Handle.IsNull, this );
            using LLVMErrorRef err = LLVMOrcCreateLLJIT(Handle, out LLVMOrcLLJITRef jitHandle);

            // calls Dispose in case something goes wrong before it is moved to return
            // NOTE: This is also a NOP if the call failed and jitHandle is NULL.
            try
            {
                // failed or not, transfer of builder happened...
                InvalidateAfterMove();
                err.ThrowIfFailed();
                return new( jitHandle ); // internally "moves" the handle so no dispose needed
            }
            catch
            {
                if(!jitHandle.IsNull)
                {
                    jitHandle.Dispose();
                }

                throw;
            }
        }

        /// <summary>Creates a new <see cref="LLJitBuilder"/> pre-configured with a <see cref="TargetMachine"/> for the current host system</summary>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns>Builder using this host as the template</returns>
        public static LLJitBuilder CreateBuilderForHost(
            CodeGenOpt optLevel = CodeGenOpt.Default,
            RelocationMode relocationMode = RelocationMode.Default,
            CodeModel codeModel = CodeModel.Default
        )
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // ownership is transferred. "Move" semantics
            return new( TargetMachineBuilder.FromHost( optLevel, relocationMode, codeModel ) );
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        internal LLVMOrcLLJITBuilderRef Handle { get; private set; }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning", Justification = "Move already occurred")]
        private void InvalidateAfterMove( )
        {
            Handle = default;
        }

        private LLJitBuilder( LLVMOrcLLJITBuilderRef h )
        {
            Handle = h;
        }

        private unsafe void* ObjectLinkingLayerContext = null;

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe /*LLVMOrcObjectLayerRef*/ nint NativeObjectLinkingLayerCreatorCallback(
            void* context,
            nint sessionRef,
            byte* triple
        )
        {
            try
            {
                if(NativeContext.TryFrom<ObjectLayerFactory>(context, out var self ))
                {
                    using var managedTriple = new Triple(LazyEncodedString.FromUnmanaged(triple));

                    // caller takes ownership of the resulting handle; Don't Dispose it
                    // It is returned via the raw native handle.
                    var factory = self( new ExecutionSession( sessionRef ), managedTriple );
                    return factory.Handle;
                }

                // TODO: How/Can this report an error? Internally the result is (via a C++ lambda) assigned to an "llvm::expected"
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
