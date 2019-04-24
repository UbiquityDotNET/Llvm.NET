// <copyright file="UnaryInstruction.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for unary operator instructions</summary>
    public class UnaryInstruction
        : Instruction
    {
        internal UnaryInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
