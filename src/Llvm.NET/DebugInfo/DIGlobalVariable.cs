// <copyright file="DIGlobalVariable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information for a global variable</summary>
    /// <seealso href="xref:llvm_langref#diglobalvariable">LLVM DIGlobalVariable</seealso>
    public class DIGlobalVariable
        : DIVariable
    {
        internal DIGlobalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
