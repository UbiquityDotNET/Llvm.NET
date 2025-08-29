// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Base class for cast instructions</summary>
    public class Cast
        : UnaryInstruction
    {
        /// <summary>Gets the source type of the cast</summary>
        public ITypeRef FromType => Operands.GetOperand<Value>( 0 )!.NativeType;

        /// <summary>Gets the destination type of the cast</summary>
        public ITypeRef ToType => NativeType;

        internal Cast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
