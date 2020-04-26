// -----------------------------------------------------------------------
// <copyright file="Branch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Branch instruction</summary>
    public class Branch
        : Terminator
    {
        /// <summary>Gets a value indicating whether this branch is conditional</summary>
        public bool IsConditional => LLVMIsConditional( ValueHandle );

        /// <summary>Gets the condition for the branch, if any</summary>
        public Value? Condition
            => !IsConditional ? null : FromHandle<Value>( LLVMGetCondition( ValueHandle ).ThrowIfInvalid( ) );

        internal Branch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
