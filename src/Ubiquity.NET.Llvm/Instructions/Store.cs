// -----------------------------------------------------------------------
// <copyright file="Store.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to store a value to memory</summary>
    public sealed class Store
        : Instruction
    {
        /// <summary>Gets or sets a value indicating whether the store is volatile</summary>
        public bool IsVolatile
        {
            get => LLVMGetVolatile( Handle );
            set => LLVMSetVolatile( Handle, value );
        }

        internal Store( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
