// <copyright file="DIMacroNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for macro related nodes in the debug hierarchy</summary>
    public class DIMacroNode
        : MDNode
    {
        internal DIMacroNode( LLVMMetadataRef handle )
           : base( handle )
        {
        }
    }
}
