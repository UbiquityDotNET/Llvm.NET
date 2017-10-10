// <copyright file="Unreachable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Unreachable
        : Terminator
    {
        internal Unreachable( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
