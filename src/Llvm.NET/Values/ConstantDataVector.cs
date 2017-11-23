// <copyright file="ConstantDataVector.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
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
