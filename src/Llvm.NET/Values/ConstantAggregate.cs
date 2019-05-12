// -----------------------------------------------------------------------
// <copyright file="ConstantAggregate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
