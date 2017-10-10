// <copyright file="CleanupReturn.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class CleanupReturn
        : Terminator
    {
        internal CleanupReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
