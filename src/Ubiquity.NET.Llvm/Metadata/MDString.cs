// -----------------------------------------------------------------------
// <copyright file="MDString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Stores a string in LlvmMetadata</summary>
    public class MDString
        : LlvmMetadata
    {
        /// <summary>Gets the string from the metadata node</summary>
        /// <returns>String this node wraps</returns>
        public override string ToString( )
        {
            return LibLLVMGetMDStringText( MetadataHandle, out uint _ ) ?? string.Empty;
        }

        internal MDString( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
