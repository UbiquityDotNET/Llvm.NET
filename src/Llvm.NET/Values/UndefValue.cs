// <copyright file="UndefValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    public class UndefValue : Constant
    {
        internal UndefValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
