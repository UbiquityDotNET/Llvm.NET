// <copyright file="CleanupPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class CleanupPad
        : FuncletPad
    {
        internal CleanupPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
