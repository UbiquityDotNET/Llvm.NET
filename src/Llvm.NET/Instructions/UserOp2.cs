// <copyright file="UserOp2.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class UserOp2 : Instruction
    {
        internal UserOp2( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
