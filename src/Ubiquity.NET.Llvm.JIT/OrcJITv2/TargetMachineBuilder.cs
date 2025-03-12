// -----------------------------------------------------------------------
// <copyright file="TargetMachineBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if FUTURE_DEVELOPMENT_AREA

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Target machine builder for ORC JIT v2</summary>
    public sealed class TargetMachineBuilder
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        internal TargetMachineBuilder(LLVMOrcJITTargetMachineBuilderRef h)
        {
            Handle = h;
        }

        internal LLVMOrcJITTargetMachineBuilderRef Handle { get; init; }
    }
}
#endif
