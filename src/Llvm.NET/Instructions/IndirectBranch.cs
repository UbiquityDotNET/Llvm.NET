// <copyright file="IndirectBranch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class IndirectBranch
        : Terminator
    {
        internal IndirectBranch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
