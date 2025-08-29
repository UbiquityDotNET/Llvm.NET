// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

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
