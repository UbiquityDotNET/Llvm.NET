// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to convert a floating point value to an unsigned integer type</summary>
    /// <seealso href="xref:llvm_langref#fptoui-to-instruction">LLVM fptoui .. to Instruction</seealso>
    public sealed class FPToUI
        : Cast
    {
        internal FPToUI( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
