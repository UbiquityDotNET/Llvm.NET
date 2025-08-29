// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
