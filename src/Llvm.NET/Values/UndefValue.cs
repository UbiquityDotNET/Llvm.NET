// <copyright file="UndefValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Represents an undefined value in LLVM IR</summary>
    public class UndefValue
        : ConstantData
    {
        internal UndefValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
