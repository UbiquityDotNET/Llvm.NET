// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Linkage flags for a symbol in the JIT</summary>
    [Flags]
    [SuppressMessage( "Design", "CA1028:Enum Storage should be Int32", Justification = "Matches ABI" )]
    public enum SymbolGenericOption
        : byte
    {
        /// <summary>No specific linkage</summary>
        None = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsNone,

        /// <summary>Symbol is exported</summary>
        Exported = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsExported,

        /// <summary>Symbol is weak</summary>
        Weak = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsWeak,

        /// <summary>Symbol is callable</summary>
        Callable = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsCallable,

        /// <summary>Symbol has materialization side effects only (normally used for static initializers)</summary>
        MaterializationSideEffectsOnly = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsMaterializationSideEffectsOnly,
    }
}
