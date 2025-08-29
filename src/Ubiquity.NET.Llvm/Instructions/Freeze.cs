// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Freeze a poison or undef value</summary>
    public sealed class Freeze
        : UnaryInstruction
    {
        /// <summary>Gets the value this instruction freezes</summary>
        public Value Value => Operands.GetOperand<Value>( 0 )!;

        internal Freeze( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
