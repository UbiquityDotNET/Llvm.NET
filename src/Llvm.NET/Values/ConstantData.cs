// <copyright file="ConstantData.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>Base class for constants with no operands</summary>
    public class ConstantData
        : Constant
    {
        internal ConstantData( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
