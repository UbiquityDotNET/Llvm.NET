// <copyright file="Terminator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Terminator
        : Instruction
    {
        internal Terminator( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
