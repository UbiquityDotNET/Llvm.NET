// <copyright file="ExtractElement.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class ExtractElement
        : Instruction
    {
        internal ExtractElement( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
