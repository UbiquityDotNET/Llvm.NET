// <copyright file="Metadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

// SA1515  Single-line comment should be preceded by blank line
#pragma warning disable SA1515

// SA1025  Code should not contain multiple whitespace characters in a row.
#pragma warning disable SA1025

namespace Llvm.NET
{
    /// <summary>Root of the LLVM Metadata hierarchy</summary>
    public abstract partial class LlvmMetadata
    {
        internal enum LLVMMetadataKind
        {
            MDTuple,
            DILocation,
            GenericDINode,
            DISubrange,
            DIEnumerator,
            DIBasicType,
            DIDerivedType,
            DICompositeType,
            DISubroutineType,
            DIFile,
            DICompileUnit,
            DISubprogram,
            DILexicalBlock,
            DILexicalBlockFile,
            DINamespace,
            DIModule,
            DITemplateTypeParameter,
            DITemplateValueParameter,
            DIGlobalVariable,
            DILocalVariable,
            DIExpression,
            DIObjCProperty,
            DIImportedEntity,
            ConstantAsMetadata,
            LocalAsMetadata,
            MDString
        }

        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataKind LLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMMetadataReplaceAllUsesWith( LLVMMetadataRef MD, LLVMMetadataRef New );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMMetadataAsString( LLVMMetadataRef descriptor );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMMDNodeReplaceAllUsesWith( LLVMMetadataRef oldDescriptor, LLVMMetadataRef newDescriptor );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsTemporary( LLVMMetadataRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsResolved( LLVMMetadataRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsDistinct( LLVMMetadataRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsUniqued( LLVMMetadataRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetMDStringText( LLVMMetadataRef M, out UInt32 len );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMMDNodeResolveCycles( LLVMMetadataRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef function );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern UInt32 LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, UInt32 index );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMMDNodeReplaceOperand( LLVMMetadataRef /* MDNode */ node, UInt32 index, LLVMMetadataRef operand );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMGetOperandNode( LLVMMDOperandRef operand );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );
        }
    }
}
