// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Evaluated JIT symbol</summary>
    /// <param name="Address">JIT engine address of the symbol</param>
    /// <param name="Flags">flags for this symbol</param>
    public readonly record struct EvaluatedSymbol(UInt64 Address, SymbolFlags Flags)
    {
        internal LLVMJITEvaluatedSymbol ToABI() => new( Address, Flags.ToABI() );
    }
}
