// -----------------------------------------------------------------------
// <copyright file="FPExt.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Extends a floating point value to a larger floating point value</summary>
    /// <seealso href="xref:llvm_langref#fpext-to-instruction">LLVM fpext .. to instruction</seealso>
    public sealed class FPExt
        : Cast
    {
        internal FPExt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
