// <copyright file="InsertValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class InsertValue
        : Instruction
    {
        internal InsertValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
