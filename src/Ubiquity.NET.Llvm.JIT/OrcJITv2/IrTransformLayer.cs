// -----------------------------------------------------------------------
// <copyright file="IrTransformLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
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

            LLVMOrcIRTransformLayerEmit(Handle, r.Handle.MoveToNative(), tsm.Handle.MoveToNative());
        }

        internal IrTransformLayer(LLVMOrcIRTransformLayerRef h)
        {
            Handle = h;
        }

        internal LLVMOrcIRTransformLayerRef Handle { get; init; }
    }
}
