// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic llvm.memcpy instruction</summary>
    public sealed class MemCpy
        : MemIntrinsic
    {
        internal MemCpy( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
