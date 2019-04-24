// <copyright file="GlobalIFunc.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Global Indirect Function</summary>
    /// <remarks>
    /// represents a single indirect function in the IR. Indirect function uses
    /// ELF symbol type extension to mark that the address of a declaration should
    /// be resolved at runtime by calling a resolver function.
    /// </remarks>
    public class GlobalIFunc
        : GlobalIndirectSymbol
    {
        /// <summary>Gets or sets the ifunc resolver</summary>
        public Constant Resolver
        {
            get => IndirectSymbol;
            set => IndirectSymbol = value;
        }

        internal GlobalIFunc( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
