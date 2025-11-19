// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

// CONSIDER: Disable or replace this analyzer - it has unusable defaults for ordering
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Delegate for an LLVM ORC JIT v2 <see cref="IrTransformLayer"/> transformation function</summary>
    /// <param name="module">Module transformation is on</param>
    /// <param name="responsibility">Responsibility for this transformation</param>
    /// <param name="replacementModule">New module if this transform replaces it [Set to <see langword="null"/> if </param>
    public delegate void TransformAction( ThreadSafeModule module, MaterializationResponsibility responsibility, out ThreadSafeModule? replacementModule );

    /// <summary>LLVM ORC JIT v2 IR Transform Layer</summary>
    public readonly ref struct IrTransformLayer
    {
        /// <summary>Emit a module</summary>
        /// <param name="r">Responsibility provided in materialization call back</param>
        /// <param name="tsm">Thread safe module to emit</param>
        public void Emit( MaterializationResponsibility r, ThreadSafeModule tsm )
        {
            ArgumentNullException.ThrowIfNull( tsm );

            r.ThrowIfIDisposed();
            tsm.ThrowIfIDisposed();

            LLVMOrcIRTransformLayerEmit( Handle, r.Handle, tsm.Handle );

            // transfer of ownership complete, mark them as such now.
            tsm.InvalidateAfterMove();
            r.InvalidateAfterMove();
        }

        /// <summary>Sets the transform function for the transform layer</summary>
        /// <param name="transformAction">Action to perform that transforms modules materialized in a JIT</param>
        public void SetTransform( TransformAction transformAction )
        {
            unsafe
            {
                // Create a holder for the action; the try/catch will take care of things
                // in the event of an exception and will be a NOP if transfer of ownership
                // completes.
                void* ctx = transformAction.AsNativeContext();
                try
                {
                    LLVMOrcIRTransformLayerSetTransform( Handle, &TransformCallback.Transform, ctx );
                }
                catch when (ctx is not null)
                {
                    NativeContext.Release(ref ctx);
                    throw;
                }
            }
        }

        internal IrTransformLayer( LLVMOrcIRTransformLayerRef h )
        {
            Handle = h;
        }

        internal LLVMOrcIRTransformLayerRef Handle { get; }

        // internal keep alive holder for a native call back as a delegate
        private static class TransformCallback
        {
            [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            internal static unsafe /*LLVMErrorRef*/ nint Transform(
                void* context,
                /*LLVMOrcThreadSafeModuleRef* */nint* modInOut,
                /*LLVMOrcMaterializationResponsibilityRef*/ nint resp
                )
            {
                // Sanity check the input for safety.
                if(resp == nint.Zero || *modInOut == nint.Zero)
                {
                    // created IDisposable is NOT ignored; it's "moved" to native code
                    return LLVMErrorRef.CreateForNativeOut( "Internal Error: got a callback with invalid handle value!"u8 );
                }

                try
                {
                    if(!NativeContext.TryFrom<TransformAction>(context, out var self ))
                    {
                        return LLVMErrorRef.CreateForNativeOut( "Internal Error: Invalid context provided for native callback"u8 );
                    }

#pragma warning disable IDISP001 // Dispose created
#pragma warning disable CA2000 // Dispose objects before losing scope
                    // module and underlying LLVMModuleRef created here are aliases, no need to dispose them
                    // disposal is wasted overhead
                    ThreadSafeModule tsm = new(*modInOut, alias: true);
                    var responsibility = new MaterializationResponsibility(resp, alias: true);

                    // if replaceMode is not null then it is moved to the native caller as an "out" param
                    // Dispose, even if NOP, is just wasted overhead.
                    self( tsm, responsibility, out ThreadSafeModule? replacedMod );
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore IDISP001 // Dispose created
                    if(replacedMod is not null)
                    {
                        *modInOut = replacedMod.Move();
                    }

                    // default LLVMErrorRef is 0 which indicates success.
                    return default;
                }
                catch(Exception ex)
                {
                    return LLVMErrorRef.CreateForNativeOut( ex.Message );
                }
            }
        }
    }
}
