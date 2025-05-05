// -----------------------------------------------------------------------
// <copyright file="ConstantDataVector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Vector of Constant Data</summary>
    public class ConstantDataVector
        : ConstantDataSequential
    {
        internal ConstantDataVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
