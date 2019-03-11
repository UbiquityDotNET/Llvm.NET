// <copyright file="Store.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

using static Llvm.NET.Instructions.Instruction.NativeMethods;

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
