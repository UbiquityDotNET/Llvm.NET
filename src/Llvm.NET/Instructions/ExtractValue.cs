// <copyright file="ExtractValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to extract the value of a member field from an aggregate value</summary>
    /// <seealso href="xref:llvm_langref#extractvalue-instruction">LLVM extractvalue Instruction</seealso>
    public class ExtractValue
        : UnaryInstruction
    {
        internal ExtractValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
