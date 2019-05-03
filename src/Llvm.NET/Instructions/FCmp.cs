// <copyright file="FCmp.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to perform comparison of floating point values</summary>
    /// <seealso href="xref:llvm_langref#fcmp-instruction">LLVM fcmp Instruction</seealso>
    public class FCmp
        : Cmp
    {
        internal FCmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
