// <copyright file="LocalAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Contains a local Value as Metadata</summary>
    public class LocalAsMetadata
        : ValueAsMetadata
    {
        internal LocalAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        /*
        static public LocalAsMetadata GetIfExists(Value local);
        static public LocalAsMetadata Create(Value local);
        */
    }
}
