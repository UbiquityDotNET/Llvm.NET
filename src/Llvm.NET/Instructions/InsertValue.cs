// <copyright file="InsertValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to insert a value into a member field in an aggregate value</summary>
    /// <seealso href="xref:llvm_langref#insertvalue-instruction">LLVM insertvalue Instruction</seealso>
    public class InsertValue
        : Instruction
    {
        internal InsertValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
