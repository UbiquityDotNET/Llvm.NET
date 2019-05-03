// <copyright file="BlockAddress.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Constant address of a block</summary>
    public class BlockAddress
        : Constant
    {
        /// <summary>Gets the <see cref="Function"/> that owns the block</summary>
        public Function Function => GetOperand<Function>( 0 );

        /// <summary>Gets the <see cref="BasicBlock"/> the address refers to</summary>
        public BasicBlock BasicBlock => GetOperand<BasicBlock>( 1 );

        internal BlockAddress( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
