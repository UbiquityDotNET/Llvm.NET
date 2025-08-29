// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Evaluated JIT symbol</summary>
    /// <param name="Address">JIT engine address of the symbol</param>
    /// <param name="Flags">flags for this symbol</param>
    public readonly record struct EvaluatedSymbol( UInt64 Address, SymbolFlags Flags )
    {
        internal LLVMJITEvaluatedSymbol ToABI( ) => new( Address, Flags.ToABI() );
    }
}
