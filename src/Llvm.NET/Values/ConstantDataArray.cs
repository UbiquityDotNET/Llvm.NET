// <copyright file="ConstantDataArray.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Array of constant data</summary>
    public class ConstantDataArray
        : ConstantDataSequential
    {
        internal ConstantDataArray( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
