// -----------------------------------------------------------------------
// <copyright file="ConstantTokenNone.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Values
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
