// <copyright file="ConstantTokenNone.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Constant token that is empty</summary>
    public class ConstantTokenNone
        : ConstantData
    {
        internal ConstantTokenNone( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
