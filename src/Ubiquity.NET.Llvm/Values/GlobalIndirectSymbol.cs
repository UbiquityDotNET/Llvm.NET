// -----------------------------------------------------------------------
// <copyright file="GlobalIndirectSymbol.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Values
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
