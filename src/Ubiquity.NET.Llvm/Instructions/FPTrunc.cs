// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to truncate a floating point value to another floating point type</summary>
    /// <seealso href="xref:llvm_langref#fptruncto-to-instruction">LLVM fptruncto .. to Instruction</seealso>
    public sealed class FPTrunc
        : Cast
    {
        internal FPTrunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
