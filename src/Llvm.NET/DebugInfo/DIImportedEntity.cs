// <copyright file="DIImportedEntity.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information forn an imported entity</summary>
    /// <seealso href="xref:llvm_langref#diimportedentity">LLVM DIImportedEntity</seealso>
    public class DIImportedEntity : DINode
    {
        internal DIImportedEntity( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
