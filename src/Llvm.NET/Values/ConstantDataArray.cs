// -----------------------------------------------------------------------
// <copyright file="ConstantDataArray.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

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
