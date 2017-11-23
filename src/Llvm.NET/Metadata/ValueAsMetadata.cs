// <copyright file="ValueAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
/* using Llvm.NET.Values; */

namespace Llvm.NET
{
    /// <summary>Used to wrap an <see cref="Llvm.NET.Values.Value"/> in the Metadata hierarchy</summary>
    public class ValueAsMetadata
        : LlvmMetadata
    {
        internal ValueAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        /* public Value Value => LLVMValueAsMetadataGetValue( MetadataHandle ); */

        /*
        public Context Context { get; }
        public Value Value { get; }
        */
    }
}
