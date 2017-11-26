// <copyright file="DIExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information expression</summary>
    /// <seealso href="xref:llvm_langref#diexpression">LLVM DIExpression</seealso>
    public class DIExpression
        : MDNode
    {
        /* TODO: non-operand properties
        // pretty much all of the LLVM DIExpression implementation.
        // most of the focus is on a sequence of expression operands
        */

        internal DIExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
