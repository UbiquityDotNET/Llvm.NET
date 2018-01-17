// <copyright file="VaArg.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to load an argument of a specified type from a variadic argument list</summary>
    public class VaArg
        : UnaryInstruction
    {
        internal VaArg( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
