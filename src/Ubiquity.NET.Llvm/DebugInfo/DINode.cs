// -----------------------------------------------------------------------
// <copyright file="DINode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Root of the object hierarchy for Debug information metadata nodes</summary>
    public class DINode
        : MDNode
    {
        /// <summary>Gets the Dwarf tag for the node</summary>
        public Tag Tag => MetadataHandle == default ? Tag.None : ( Tag )LibLLVMDIDescriptorGetTag( MetadataHandle );

        internal DINode( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
