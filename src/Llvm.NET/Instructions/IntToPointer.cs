// <copyright file="IntToPointer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class IntToPointer
        : Cast
    {
        internal IntToPointer( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
