// <copyright file="Trunc.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Trunc
        : Cast
    {
        internal Trunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
