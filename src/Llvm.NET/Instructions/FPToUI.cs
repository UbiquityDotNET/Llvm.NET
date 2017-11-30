// <copyright file="FPToUI.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to convert a floating point value to an unsigned integer type</summary>
    /// <seealso href="xref:llvm_langref#fptoui-to-instruction">LLVM fptoui .. to Instruction</seealso>
    public class FPToUI : Cast
    {
        internal FPToUI( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
