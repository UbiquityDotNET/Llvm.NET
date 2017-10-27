// <copyright file="Cmp.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    public class Cmp
        : Instruction
    {
        public Predicate Predicate
        {
            get
            {
                switch( Opcode )
                {
                case OpCode.ICmp:
                    return ( Predicate )LLVMGetICmpPredicate( ValueHandle );

                case OpCode.FCmp:
                    return ( Predicate )LLVMGetFCmpPredicate( ValueHandle );

                default:
                    return Predicate.BadFcmpPredicate;
                }
            }
        }

        internal Cmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
