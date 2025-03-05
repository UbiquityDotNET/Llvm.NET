// -----------------------------------------------------------------------
// <copyright file="ExecutionSession.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>ORC JIT v2 symbol string pool</summary>
    public readonly ref struct SymbolStringPool
    {
        internal SymbolStringPool(LLVMOrcSymbolStringPoolRef h)
        {
            Handle = h;
        }

        internal LLVMOrcSymbolStringPoolRef Handle { get; init; }
    }
}
