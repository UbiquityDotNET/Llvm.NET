// <copyright file="DINode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

using static Llvm.NET.DebugInfo.DINode.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Root of the object hierarchy for Debug information metadata nodes</summary>
    public partial class DINode
        : MDNode
    {
        /// <summary>Gets the Dwarf tag for the node</summary>
        public Tag Tag
        {
            get
            {
                if( MetadataHandle == default )
                {
                    return (Tag)ushort.MaxValue;
                }

                return ( Tag )LLVMDIDescriptorGetTag( MetadataHandle );
            }
        }

        internal DINode( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
