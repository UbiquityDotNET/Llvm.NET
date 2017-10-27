// <copyright file="MDString.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET
{
    public class MDString
        : LlvmMetadata
    {
        public override string ToString( )
        {
            return NativeMethods.LLVMGetMDStringText( MetadataHandle, out uint len );
        }

        internal MDString( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
