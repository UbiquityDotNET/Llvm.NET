// <copyright file="DINode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Root of the object hierarchy for Debug information metadata nodes</summary>
    public class DINode
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

        // ReSharper disable IdentifierTypo
        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );
    }
}
