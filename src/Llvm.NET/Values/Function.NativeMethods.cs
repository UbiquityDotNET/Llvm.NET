// <copyright file="Function.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

using CC = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Function definition</summary>
    public partial class Function
    {
        internal static new class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMGetArgumentIndex( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl )]
            internal static extern void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );

            [DllImport( LibraryPath, CallingConvention = CC.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMStatus LLVMVerifyFunctionEx( LLVMValueRef Fn, LLVMVerifierFailureAction Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string OutMessages );

            [DllImport( LibraryPath, EntryPoint = "LLVMDeleteFunction", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMDeleteFunction( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMHasPersonalityFn", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMHasPersonalityFn( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPersonalityFn", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPersonalityFn( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetPersonalityFn", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetPersonalityFn( LLVMValueRef Fn, LLVMValueRef PersonalityFn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIntrinsicID", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMGetIntrinsicID( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionCallConv", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMGetFunctionCallConv( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetFunctionCallConv", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMSetFunctionCallConv( LLVMValueRef Fn, uint CC );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountParams", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMCountParams( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParams", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMGetParams( LLVMValueRef Fn, out LLVMValueRef Params );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParam", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetParam( LLVMValueRef Fn, uint Index );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParamParent", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetParamParent( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstParam", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetFirstParam( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastParam", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetLastParam( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextParam", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetNextParam( LLVMValueRef Arg );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousParam", CallingConvention = CC.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPreviousParam( LLVMValueRef Arg );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountBasicBlocks", CallingConvention = CC.Cdecl )]
            internal static extern uint LLVMCountBasicBlocks( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlocks", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMGetBasicBlocks( LLVMValueRef Fn, out LLVMBasicBlockRef BasicBlocks );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetFirstBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetLastBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetNextBasicBlock( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetPreviousBasicBlock( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetEntryBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetEntryBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlock", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMAppendBasicBlock( LLVMValueRef Fn, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlock", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMInsertBasicBlock( LLVMBasicBlockRef InsertBeforeBB, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMDeleteBasicBlock", CallingConvention = CC.Cdecl )]
            internal static extern void LLVMDeleteBasicBlock( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetGC", CallingConvention = CC.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetGC( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetGC", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetGC( LLVMValueRef Fn, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddAttributeAtIndex", CallingConvention = CC.Cdecl)]
            internal static extern void LLVMAddAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, LLVMAttributeRef A );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributeCountAtIndex", CallingConvention = CC.Cdecl)]
            internal static extern uint LLVMGetAttributeCountAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetAttributesAtIndex", CallingConvention = CC.Cdecl)]
            internal static extern void LLVMGetAttributesAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, out LLVMAttributeRef Attrs );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetEnumAttributeAtIndex", CallingConvention = CC.Cdecl)]
            internal static extern LLVMAttributeRef LLVMGetEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetStringAttributeAtIndex", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMAttributeRef LLVMGetStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );

            [DllImport( LibraryPath, EntryPoint = "LLVMRemoveEnumAttributeAtIndex", CallingConvention = CC.Cdecl)]
            internal static extern void LLVMRemoveEnumAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, uint KindID );

            [DllImport( LibraryPath, EntryPoint = "LLVMRemoveStringAttributeAtIndex", CallingConvention = CC.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMRemoveStringAttributeAtIndex( LLVMValueRef F, LLVMAttributeIndex Idx, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLen );
        }
    }
}
