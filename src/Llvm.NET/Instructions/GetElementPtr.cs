// <copyright file="GetElementPtr.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to compute the address of a sub element of an aggregate data type</summary>
    /// <seealso href="xref:llvm_langref#getelementptr-instruction">LLVM getelementptr Instruction</seealso>
    public class GetElementPtr
        : Instruction
    {
        internal GetElementPtr( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
