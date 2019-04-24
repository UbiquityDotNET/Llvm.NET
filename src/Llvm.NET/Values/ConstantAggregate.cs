// <copyright file="ConstantAggregate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Values
{
    /// <summary>Base class for aggregate constants (with operands).</summary>
    public class ConstantAggregate
        : Constant
    {
        internal ConstantAggregate( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
