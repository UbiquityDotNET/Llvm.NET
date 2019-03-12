// <copyright file="DataLayout.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET
{
    /// <summary>Provides access to LLVM target data layout information</summary>
    public sealed partial class DataLayout
    {
        internal static class NativeMethods
        {
            /* TODO: Support constructing DataLayout from a string*/
            [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMTargetDataRef LLVMCreateTargetData( [MarshalAs( UnmanagedType.LPStr )] string StringRep );

            [DllImport( LibraryPath, EntryPoint = "LLVMCopyStringRepOfTargetData", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMCopyStringRepOfTargetData( LLVMTargetDataRef TD );

            [DllImport( LibraryPath, EntryPoint = "LLVMByteOrder", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMByteOrdering LLVMByteOrder( LLVMTargetDataRef TD );

            [DllImport( LibraryPath, EntryPoint = "LLVMPointerSize", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMPointerSize( LLVMTargetDataRef TD );

            [DllImport( LibraryPath, EntryPoint = "LLVMPointerSizeForAS", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMPointerSizeForAS( LLVMTargetDataRef TD, uint AS );

            [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMIntPtrTypeInContext( LLVMContextRef C, LLVMTargetDataRef TD );

            [DllImport( LibraryPath, EntryPoint = "LLVMIntPtrTypeForASInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMIntPtrTypeForASInContext( LLVMContextRef C, LLVMTargetDataRef TD, uint AS );

            [DllImport( LibraryPath, EntryPoint = "LLVMSizeOfTypeInBits", CallingConvention = CallingConvention.Cdecl )]
            internal static extern ulong LLVMSizeOfTypeInBits( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMStoreSizeOfType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern ulong LLVMStoreSizeOfType( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMABISizeOfType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern ulong LLVMABISizeOfType( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMABIAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMABIAlignmentOfType( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMCallFrameAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMCallFrameAlignmentOfType( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfType", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMPreferredAlignmentOfType( LLVMTargetDataRef TD, LLVMTypeRef Ty );

            [DllImport( LibraryPath, EntryPoint = "LLVMPreferredAlignmentOfGlobal", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMPreferredAlignmentOfGlobal( LLVMTargetDataRef TD, LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMElementAtOffset", CallingConvention = CallingConvention.Cdecl )]
            internal static extern uint LLVMElementAtOffset( LLVMTargetDataRef TD, LLVMTypeRef StructTy, ulong Offset );

            [DllImport( LibraryPath, EntryPoint = "LLVMOffsetOfElement", CallingConvention = CallingConvention.Cdecl )]
            internal static extern ulong LLVMOffsetOfElement( LLVMTargetDataRef TD, LLVMTypeRef StructTy, uint Element );
        }
    }
}
