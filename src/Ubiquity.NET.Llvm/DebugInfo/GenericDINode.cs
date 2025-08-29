// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Generic tagged DWARF-like IrMetadata node</summary>
    public class GenericDINode
        : DINode
    {
        /// <summary>Gets the header for this node</summary>
        /// <remarks>
        /// The header is a, possibly empty, null separated string
        /// header that contains arbitrary fields.
        /// </remarks>
        public string Header => GetOperandString( 0 );

        internal GenericDINode( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
