// -----------------------------------------------------------------------
// <copyright file="Branch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Branch instruction</summary>
    public sealed class Branch
        : Terminator
    {
        /// <summary>Gets a value indicating whether this branch is conditional</summary>
        public bool IsConditional => LLVMIsConditional( Handle );

        /// <summary>Gets the condition for the branch, if any</summary>
        public Value? Condition
            => !IsConditional ? null : FromHandle<Value>( LLVMGetCondition( Handle ).ThrowIfInvalid( ) );

        internal Branch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
