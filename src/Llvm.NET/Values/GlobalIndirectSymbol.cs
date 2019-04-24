// <copyright file="GlobalIndirectSymbol.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Global Indirect Symbol</summary>
    public class GlobalIndirectSymbol
        : GlobalValue
    {
        /// <summary>Gets or sets the symbol this indirectly references</summary>
        public Constant IndirectSymbol
        {
            get => GetOperand<Constant>( 0 );
            set => Operands[ 0 ] = value;
        }

        internal GlobalIndirectSymbol( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
