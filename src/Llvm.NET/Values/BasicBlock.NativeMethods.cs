// <copyright file="BasicBlock.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>Provides access to an LLVM Basic block</summary>
    public partial class BasicBlock
    {
        internal static new class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMBasicBlockAsValue", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMBasicBlockAsValue( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMValueIsBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMValueIsBasicBlock( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMValueAsBasicBlock", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMBasicBlockRef LLVMValueAsBasicBlock( LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockName", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetBasicBlockName( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockParent", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetBasicBlockParent( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBasicBlockTerminator", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetBasicBlockTerminator( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMRemoveBasicBlockFromParent", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMRemoveBasicBlockFromParent( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockBefore", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMMoveBasicBlockBefore( LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos );

            [DllImport( LibraryPath, EntryPoint = "LLVMMoveBasicBlockAfter", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMMoveBasicBlockAfter( LLVMBasicBlockRef BB, LLVMBasicBlockRef MovePos );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstInstruction", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetFirstInstruction( LLVMBasicBlockRef BB );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastInstruction", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetLastInstruction( LLVMBasicBlockRef BB );
        }
    }
}
