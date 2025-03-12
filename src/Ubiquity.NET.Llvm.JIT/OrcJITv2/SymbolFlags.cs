// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Generic and target specific flags for a symbol</summary>
    /// <param name="Generic">Generic options for this symbol</param>
    /// <param name="Target">Target specific flags for this symbol</param>
    public readonly record struct SymbolFlags(SymbolGenericOption Generic, byte Target)
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolFlags"/> class.</summary>
        /// <param name="genericOptions">Generic options for this symbol</param>
        public SymbolFlags(SymbolGenericOption genericOptions)
            : this( genericOptions, 0 )
        {
        }

        internal LLVMJITSymbolFlags ToABI() => new( (LLVMJITSymbolGenericFlags)Generic, Target );
    }
}
