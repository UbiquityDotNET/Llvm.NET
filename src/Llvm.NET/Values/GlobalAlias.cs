﻿// -----------------------------------------------------------------------
// <copyright file="GlobalAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Global Alias for a function or global value</summary>
    public class GlobalAlias
        : GlobalIndirectSymbol
    {
        /// <summary>Gets or sets the aliasee that this Alias refers to</summary>
        public Constant Aliasee
        {
            get => IndirectSymbol;
            set => IndirectSymbol = value;
        }

        internal GlobalAlias( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
