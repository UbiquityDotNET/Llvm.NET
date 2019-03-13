// <copyright file="DINode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Root of the object hierarchy for Debug information metadata nodes</summary>
    public partial class DINode
    {
        internal static new class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMDITypeGetLine( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt64 LLVMDITypeGetSizeInBits( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt64 LLVMDITypeGetAlignInBits( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt64 LLVMDITypeGetOffsetInBits( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMDITypeGetFlags( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDITypeGetScope( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMDITypeGetName( LLVMMetadataRef typeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDIScopeGetFile( LLVMMetadataRef scope );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetDIFileName( LLVMMetadataRef /*DIFile*/ file );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetDIFileDirectory( LLVMMetadataRef /*DIFile*/ file );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef LLVMDILocalScopeGetSubProgram( LLVMMetadataRef /*DILocalScope*/ localScope );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMPassRegistryRef LLVMCreatePassRegistry();

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMMetadataRef LLVMDIGlobalVarExpGetVariable( LLVMMetadataRef metadataHandle );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMDIBasicTypeGetEncoding( LLVMMetadataRef /*DIBasicType*/ basicType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef /*DILocalScope*/ LLVMGetDILocationScope( LLVMMetadataRef /*DILocation*/ location );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMGetDILocationLine( LLVMMetadataRef /*DILocation*/ location );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMGetDILocationColumn( LLVMMetadataRef /*DILocation*/ location );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef /*DILocation*/ LLVMGetDILocationInlinedAt( LLVMMetadataRef /*DILocation*/ location );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMMetadataRef LLVMDILocation( LLVMContextRef context, UInt32 Line, UInt32 Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt );
        }
    }
}
