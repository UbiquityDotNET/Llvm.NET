// -----------------------------------------------------------------------
// <copyright file="UndefValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

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
