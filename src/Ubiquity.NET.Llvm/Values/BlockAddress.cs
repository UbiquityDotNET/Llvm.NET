// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Constant address of a block</summary>
    public class BlockAddress
        : Constant
    {
        /// <summary>Gets the <see cref="Function"/> that owns the block</summary>
        public Function Function => Operands.GetOperand<Function>( 0 )!;

        /// <summary>Gets the <see cref="BasicBlock"/> the address refers to</summary>
        public BasicBlock BasicBlock => Operands.GetOperand<BasicBlock>( 1 )!;

        internal BlockAddress( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
