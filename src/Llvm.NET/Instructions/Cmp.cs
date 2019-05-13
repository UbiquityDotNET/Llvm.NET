// -----------------------------------------------------------------------
// <copyright file="Cmp.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for compare instructions</summary>
    public class Cmp
        : Instruction
    {
        /// <summary>Gets the predicate for the comparison</summary>
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

        /* TODO: Predicate {set;} */

        internal Cmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
