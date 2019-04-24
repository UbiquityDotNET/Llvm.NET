// <copyright file="FPTrunc.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to truncate a floating point value to another floating point type</summary>
    /// <seealso href="xref:llvm_langref#fptruncto-to-instruction">LLVM fptruncto .. to Instruction</seealso>
    public class FPTrunc
        : Cast
    {
        internal FPTrunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
