﻿// -----------------------------------------------------------------------
// <copyright file="Cmp.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Base class for compare instructions</summary>
    public class Cmp
        : Instruction
    {
        /// <summary>Gets the predicate for the comparison</summary>
        public Predicate Predicate => Opcode switch
        {
            OpCode.ICmp => ( Predicate )LLVMGetICmpPredicate( ValueHandle ),
            OpCode.FCmp => ( Predicate )LLVMGetFCmpPredicate( ValueHandle ),
            _ => Predicate.BadFcmpPredicate,
        };

        /* TODO: Predicate {set;} */

        internal Cmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
