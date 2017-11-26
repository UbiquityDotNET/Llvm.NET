// <copyright file="FPExt.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Extends a floating point value to a larger floting point value</summary>
    /// <seealso href="xref:llvm_langref#fpext-to-instruction">LLVM 'fpext .. to' instruction</seealso>
    public class FPExt
        : Cast
    {
        internal FPExt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
