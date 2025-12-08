// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to compute the address of a sub element of an aggregate data type</summary>
    /// <seealso href="xref:llvm_langref#getelementptr-instruction">LLVM getelementptr Instruction</seealso>
    public sealed class GetElementPtr
        : Instruction
    {
        internal GetElementPtr( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        /// <summary>Gets the base of the GEP instruction</summary>
        public Value? Base => Operands.GetOperand<Value>(0);

        /// <summary>Gets the index values for the GEP instruction</summary>
        public ImmutableArray<Value?> IndexValues => [ .. Operands.Skip(1) ];
    }
}
