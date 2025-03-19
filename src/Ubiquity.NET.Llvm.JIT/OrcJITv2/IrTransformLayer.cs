// -----------------------------------------------------------------------
// <copyright file="IrTransformLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// CONSIDER: Disable or replace this analyzer - it has unusable defaults for ordering
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Delegate for an LLVM ORC JIT v2 <see cref="IrTransformLayer"/> transformation function</summary>
    /// <param name="module">Module transformation is on</param>
    /// <param name="responsibility">Responsibility for this transformation</param>
    /// <param name="replacementModule">New module if this transform replaces it [Set to <see langword="null"/> if </param>
    public delegate void TransformAction(ThreadSafeModule module, MaterializationResponsibility responsibility, out ThreadSafeModule? replacementModule);

    /// <summary>LLVM ORC JIT v2 IR Transform Layer</summary>
    public readonly ref struct IrTransformLayer
    {
        /// <summary>Emit a module</summary>
        /// <param name="r">Responsibility provided in materialization call back</param>
        /// <param name="tsm">Thread safe module to emit</param>
        public void Emit(MaterializationResponsibility r, ThreadSafeModule tsm)
        {
            ArgumentNullException.ThrowIfNull(tsm);

            r.ThrowIfIDisposed();
            tsm.ThrowIfIDisposed();

            LLVMOrcIRTransformLayerEmit(Handle, r.Handle, tsm.Handle);

            // transfer of ownership complete, mark them as such now.
            tsm.Handle.SetHandleAsInvalid();
            r.Handle.SetHandleAsInvalid();
        }

        /// <summary>Sets the transform function for the transform layer</summary>
        /// <param name="transformAction">Action to perform that transforms modules materialized in a JIT</param>
        public void SetTransform(TransformAction transformAction)
        {
            // Create a holder for the action; the Dispose will take care of things
            // in the event of an exception and will become a NOP if transfer of
            // ownership completes.
            using var holder = new TransformCallback(transformAction);
            unsafe
            {
                LLVMOrcIRTransformLayerSetTransform(Handle, TransformCallback.Callback, (void*)holder.AddRefAndGetNativeContext());
            }
        }

        internal IrTransformLayer(LLVMOrcIRTransformLayerRef h)
        {
            Handle = h;
        }

        internal LLVMOrcIRTransformLayerRef Handle { get; init; }

        // internal keep alive holder for a native call back as a delegate
        private sealed class TransformCallback
            : IDisposable
        {
            public TransformCallback(TransformAction transformAction)
            {
                AllocatedSelf = new(this);
                TransformAction = transformAction;
            }

            public void Dispose()
            {
                AllocatedSelf.Dispose();
            }

            internal TransformAction TransformAction { get; }

            internal unsafe nint AddRefAndGetNativeContext()
            {
                return AllocatedSelf.AddRefAndGetNativeContext();
            }

            private readonly SafeGCHandle AllocatedSelf;

            internal static unsafe delegate* unmanaged[Cdecl]<void*, nint*, nint, nint> Callback => &Transform;

            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
            [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
            [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "All instances are created as an alias or 'moved' to native Dispose() not needed" )]
            [SuppressMessage( "IDisposableAnalyzers.Correctness", "IDISP001:Dispose created", Justification = "Known Alias, no need for dispose" )]
            private static unsafe /*LLVMErrorRef*/ nint Transform(
                void* context,
                /*LLVMOrcThreadSafeModuleRef* */nint* modInOut,
                /*LLVMOrcMaterializationResponsibilityRef*/ nint resp
                )
            {
                // Sanity check the input for safety.
                if (resp == nint.Zero || *modInOut == nint.Zero)
                {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                    // Not ignored, it's "moved" to native code
                    return ErrorInfo.Create("Internal Error: got a callback with invalid handle value!")
                                    .MoveToNative();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }

                if(context is null)
                {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                    // Not ignored, it's "moved" to native code
                    return ErrorInfo.Create("Internal Error: Invalid context provided for native callback")
                                    .MoveToNative();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }

                try
                {
                    if(GCHandle.FromIntPtr( (nint)context ).Target is TransformCallback self)
                    {
                        // module and underlying LLVMModuleRef created here are aliases, no need to dispose them
                        ThreadSafeModule tsm = new(*modInOut, alias: true);
                        var responsibility = new MaterializationResponsibility(resp, alias: true);

                        self.TransformAction( tsm, responsibility, out ThreadSafeModule? replacedMod );
                        if(replacedMod is not null)
                        {
                            *modInOut = replacedMod.Handle.MoveToNative();
                        }
                    }

                    // default LLVMErrorRef is 0 which indicates success.
                    return default;
                }
                catch(Exception ex)
                {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                    // resulting instance is "moved" to native return; Dispose is wasted overhead for a NOP
                    return ErrorInfo.Create(ex)
                                    .MoveToNative();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }
            }
        }
    }
}
