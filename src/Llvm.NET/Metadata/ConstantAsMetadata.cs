// <copyright file="ConstantAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    public class ConstantAsMetadata
        : ValueAsMetadata
    {
        internal ConstantAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
