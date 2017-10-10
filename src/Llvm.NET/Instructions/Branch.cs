// <copyright file="Branch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Branch
        : Terminator
    {
        internal Branch( LLVMValueRef valueRef)
            : base( valueRef )
        {
        }
    }
}
