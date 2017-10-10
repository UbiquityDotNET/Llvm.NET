// <copyright file="ExtractValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class ExtractValue
        : UnaryInstruction
    {
        internal ExtractValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
