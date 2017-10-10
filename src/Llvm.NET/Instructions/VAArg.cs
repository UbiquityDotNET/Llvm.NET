// <copyright file="VaArg.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class VaArg : UnaryInstruction
    {
        internal VaArg( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
