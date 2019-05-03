// <copyright file="Return.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Return instruction</summary>
    public class ReturnInstruction
        : Terminator
    {
        internal ReturnInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
