// <copyright file="PointerToInt.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class PointerToInt
        : Cast
    {
        internal PointerToInt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
