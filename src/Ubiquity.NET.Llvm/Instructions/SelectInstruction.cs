// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Select instruction</summary>
    public sealed class SelectInstruction
        : Instruction
    {
        internal SelectInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
