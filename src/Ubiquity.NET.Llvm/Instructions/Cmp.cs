// -----------------------------------------------------------------------
// <copyright file="Cmp.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Base class for compare instructions</summary>
    public class Cmp
        : Instruction
    {
        /// <summary>Gets the predicate for the comparison</summary>
        public Predicate Predicate => Opcode switch
        {
            OpCode.ICmp => ( Predicate )LLVMGetICmpPredicate( Handle ),
            OpCode.FCmp => ( Predicate )LLVMGetFCmpPredicate( Handle ),
            _ => Predicate.BadFcmpPredicate,
        };

        /* TODO: Predicate {set;} // new LibLLVM API, based on current Get*/

        internal Cmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
