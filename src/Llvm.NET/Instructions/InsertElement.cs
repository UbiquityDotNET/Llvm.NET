// <copyright file="InsertElement.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class InsertElement
        : Instruction
    {
        internal InsertElement( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
