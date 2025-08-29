// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to shuffle the elements of a vector</summary>
    public sealed class ShuffleVector
        : Instruction
    {
        internal ShuffleVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
