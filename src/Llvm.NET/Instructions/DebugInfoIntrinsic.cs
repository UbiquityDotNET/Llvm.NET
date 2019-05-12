// -----------------------------------------------------------------------
// <copyright file="DebugInfoIntrinsic.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for debug information intrinsic functions in LLVM IR</summary>
    public class DebugInfoIntrinsic
        : Intrinsic
    {
        internal DebugInfoIntrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
