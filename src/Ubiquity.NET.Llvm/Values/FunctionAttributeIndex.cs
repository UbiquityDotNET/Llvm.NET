// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Function index for attributes</summary>
    /// <remarks>
    /// Attributes on functions apply to the function itself, the return type
    /// or one of the function's parameters. This enumeration is used to
    /// identify where the attribute applies.
    /// </remarks>
    public enum FunctionAttributeIndex
    {
        /// <summary>The attribute applies to the function itself</summary>
        Function = LLVMAttributeIndex.LLVMAttributeFunctionIndex,

        /// <summary>The attribute applies to the return type of the function</summary>
        ReturnType = LLVMAttributeIndex.LLVMAttributeReturnIndex,

        /// <summary>The attribute applies to the first parameter of the function</summary>
        /// <remarks>
        /// Additional parameters are identified by simply adding an integer value to
        /// this value. (i.e. Parameter1 == FunctionAttributeIndex.Parameter0 + 1 )
        /// </remarks>
        Parameter0 = ReturnType +1
    }
}
