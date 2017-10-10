// <copyright file="UserOp1.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class UserOp1 : Instruction
    {
        internal UserOp1( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
