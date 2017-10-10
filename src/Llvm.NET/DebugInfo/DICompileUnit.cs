// <copyright file="DICompileUnit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug Information Compile Unit, which acts as the containing parent for debug information in a module</summary>
    /// <seealso href="xref:llvm_langref#dicompileunit">LLVM DICompileUnit</seealso>
    public class DICompileUnit : DIScope
    {
        /// <summary>Creates a new compilation unit from a native <see cref="LLVMMetadataRef"/></summary>
        /// <param name="handle">native reference to wrap</param>
        internal DICompileUnit( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
