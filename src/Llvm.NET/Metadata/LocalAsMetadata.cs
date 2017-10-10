// <copyright file="LocalAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    public class LocalAsMetadata
        : ValueAsMetadata
    {
        internal LocalAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
