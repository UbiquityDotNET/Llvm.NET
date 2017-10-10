// <copyright file="Cast.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Cast
        : UnaryInstruction
    {
        internal Cast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
