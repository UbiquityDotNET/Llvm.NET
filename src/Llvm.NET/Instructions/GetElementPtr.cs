// <copyright file="GetElementPtr.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class GetElementPtr
        : Instruction
    {
        internal GetElementPtr( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
