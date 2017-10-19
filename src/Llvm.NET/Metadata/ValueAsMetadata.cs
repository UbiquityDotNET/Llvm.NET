// <copyright file="ValueAsMetadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    public class ValueAsMetadata
        : LlvmMetadata
    {
        internal ValueAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }

        /*
        public Context Context { get; }
        public Value Value { get; }
        */
    }
}
