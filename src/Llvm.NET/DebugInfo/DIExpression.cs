// <copyright file="DIExpression.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information expression</summary>
    /// <seealso href="xref:llvm_langref#diexpression">LLVM DIExpression</seealso>
    public class DIExpression : MDNode
    {
        internal DIExpression( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
