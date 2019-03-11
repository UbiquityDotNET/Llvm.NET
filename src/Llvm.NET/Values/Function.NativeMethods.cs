// <copyright file="Function.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Function definition</summary>
    public partial class Function
    {
        internal static new class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern UInt32 LLVMGetArgumentIndex( LLVMValueRef Val );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );

            [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMStatus LLVMVerifyFunctionEx( LLVMValueRef Fn, LLVMVerifierFailureAction Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string OutMessages );

            [DllImport( LibraryPath, EntryPoint = "LLVMDeleteFunction", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMDeleteFunction( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMHasPersonalityFn", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMHasPersonalityFn( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPersonalityFn", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPersonalityFn( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetPersonalityFn", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMSetPersonalityFn( LLVMValueRef Fn, LLVMValueRef PersonalityFn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetIntrinsicID", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern uint LLVMGetIntrinsicID( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFunctionCallConv", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern uint LLVMGetFunctionCallConv( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetFunctionCallConv", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMSetFunctionCallConv( LLVMValueRef Fn, uint CC );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountParams", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern uint LLVMCountParams( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParams", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMGetParams( LLVMValueRef Fn, out LLVMValueRef Params );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetParam( LLVMValueRef Fn, uint Index );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetParamParent", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetParamParent( LLVMValueRef Inst );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetFirstParam( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetLastParam( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetNextParam( LLVMValueRef Arg );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousParam", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPreviousParam( LLVMValueRef Arg );

            [DllImport( LibraryPath, EntryPoint = "LLVMCountBasicBlocks", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern uint LLVMCountBasicBlocks( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlocks", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMGetBasicBlocks( LLVMValueRef Fn, out LLVMBasicBlockRef BasicBlocks );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetFirstBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetLastBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetNextBasicBlock( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetPreviousBasicBlock( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetEntryBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMGetEntryBasicBlock( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMAppendBasicBlock( LLVMValueRef Fn, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMInsertBasicBlock( LLVMBasicBlockRef InsertBeforeBB, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMDeleteBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMDeleteBasicBlock( LLVMBasicBlockRef BB );
        }
    }
}
