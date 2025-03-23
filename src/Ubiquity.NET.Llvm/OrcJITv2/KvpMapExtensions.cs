// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    internal static class KvpMapExtensions
    {
        internal static LLVMOrcCSymbolAliasMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }

        internal static LLVMOrcCSymbolMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }

        internal static LLVMOrcCSymbolFlagsMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, SymbolFlags> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }
    }
}
