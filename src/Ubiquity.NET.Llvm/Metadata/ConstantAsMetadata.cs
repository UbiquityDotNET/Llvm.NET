// -----------------------------------------------------------------------
// <copyright file="ConstantAsMetadata.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Constant <see cref="Value"/> as metadata</summary>
    public class ConstantAsMetadata
        : ValueAsMetadata
    {
        /// <summary>Gets the <see cref="Constant"/> this node wraps</summary>
        public Constant Constant => (Value as Constant)!;

        internal ConstantAsMetadata( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
