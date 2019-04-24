// <copyright file="UserOp1.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Custom operator that can be used in LLVM transform passes but should be removed before target instruction selection</summary>
    public class UserOp1
        : Instruction
    {
        internal UserOp1( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
