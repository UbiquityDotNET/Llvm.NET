// -----------------------------------------------------------------------
// <copyright file="Enumerations.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// shut up tooling for no docs on things that are 1:1 renames/alaising of LLVM values and not entirely clear
#pragma warning disable CS1591 // Disable warning: "Missing XML comment for publicly visible type or member"

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Generic linkage flags for a symbol definition</summary>
    [Flags]
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Currently unclear but maps to LLVM 1:1" )]
    public enum GenericSymbolOption
    {
        None = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsNone,
        Exported = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsExported,
        Weak = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsWeak,
        Callable = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsCallable,
        MaterializationSideEffectsOnly = LLVMJITSymbolGenericFlags.LLVMJITSymbolGenericFlagsMaterializationSideEffectsOnly,
    }
}
