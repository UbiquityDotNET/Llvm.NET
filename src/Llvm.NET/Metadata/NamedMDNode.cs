// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    /// <remarks>Despite its name a NamedMDNode is not itself an MDNode. It is owned directly by a
    /// a <see cref="BitcodeModule"/> and contains a list of <see cref="MDNode"/> operands.</remarks>
    public partial class NamedMDNode
    {
        /// <summary>Gets the name of the node</summary>
        public string Name => LLVMNamedMDNodeGetName( NativeHandle );

        /// <summary>Gets the operands for the node</summary>
        public IList<MDNode> Operands { get; }

        /// <summary>Gets the module that owns this node</summary>
        public BitcodeModule ParentModule => BitcodeModule.FromHandle( LLVMNamedMDNodeGetParentModule( NativeHandle ) );

        internal NamedMDNode( LLVMNamedMDNodeRef nativeNode )
        {
            NativeHandle = nativeNode;
            Operands = new OperandIterator( this );
        }

        private LLVMNamedMDNodeRef NativeHandle;

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        private static extern string /*char const**/ LLVMNamedMDNodeGetName( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern UInt32 LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern /*MDNode*/ LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index, LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMNamedMDNodeEraseFromParent( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern LLVMModuleRef LLVMNamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode );
    }
}
