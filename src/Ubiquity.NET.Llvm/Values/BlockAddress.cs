// -----------------------------------------------------------------------
// <copyright file="BlockAddress.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Constant address of a block</summary>
    public class BlockAddress
        : Constant
    {
        /// <summary>Gets the <see cref="Function"/> that owns the block</summary>
        public IrFunction Function => Operands.GetOperand<IrFunction>( 0 )!;

        /// <summary>Gets the <see cref="BasicBlock"/> the address refers to</summary>
        public BasicBlock BasicBlock => Operands.GetOperand<BasicBlock>( 1 )!;

        internal BlockAddress( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
