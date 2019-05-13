// -----------------------------------------------------------------------
// <copyright file="Store.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to store a value to memory</summary>
    public class Store
        : Instruction
    {
        /// <summary>Gets or sets a value indicating whether the store is volatile</summary>
        public bool IsVolatile
        {
            get => LLVMGetVolatile( ValueHandle );
            set => LLVMSetVolatile( ValueHandle, value );
        }

        internal Store( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
