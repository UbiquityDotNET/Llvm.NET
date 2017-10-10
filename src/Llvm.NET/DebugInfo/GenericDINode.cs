// <copyright file="GenericDINode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class GenericDINode : DINode
    {
        internal GenericDINode( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
