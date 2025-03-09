// -----------------------------------------------------------------------
// <copyright file="GlobalAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>LLVM Global Alias for a function or global value</summary>
    public class GlobalAlias
        : GlobalIndirectSymbol
    {
        /// <summary>Gets or sets the aliasee that this Alias refers to</summary>
        [DisallowNull]
        public Constant Aliasee
        {
            get => IndirectSymbol!;
            set => IndirectSymbol = value.ThrowIfNull();
        }

        internal GlobalAlias( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
