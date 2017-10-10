// <copyright file="ConstantTokenNone.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    public class ConstantTokenNone : Constant
    {
        internal ConstantTokenNone( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
