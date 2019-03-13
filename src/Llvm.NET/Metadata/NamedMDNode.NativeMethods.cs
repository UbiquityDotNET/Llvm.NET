// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    public partial class NamedMDNode
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string /*char const**/ LLVMNamedMDNodeGetName( LLVMNamedMDNodeRef namedMDNode );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern UInt32 LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern /*MDNode*/ LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index, LLVMMetadataRef /*MDNode*/ node );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMNamedMDNodeEraseFromParent( LLVMNamedMDNodeRef namedMDNode );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMModuleRef LLVMNamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode );
        }
    }
}
