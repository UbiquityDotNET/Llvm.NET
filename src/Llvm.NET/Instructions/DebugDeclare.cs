// <copyright file="DebugDeclare.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class DebugDeclare
        : DebugInfoIntrinsic
    {
        internal DebugDeclare( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
