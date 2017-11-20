// <copyright file="MDTuple.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Tuple of Metadata nodes</summary>
    /// <remarks>
    /// This acts as a container of nodes in the metadata heirarchy
    /// </remarks>
    public class MDTuple : MDNode
    {
        internal MDTuple( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
