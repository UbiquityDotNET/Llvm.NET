// <copyright file="Target.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>LLVM Target Instruction Set Architecture</summary>
    public partial class Target
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetRef LLVMGetFirstTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetRef LLVMGetNextTarget( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMTargetRef LLVMGetTargetFromName( [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetFromTriple", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMStatus LLVMGetTargetFromTriple( [MarshalAs( UnmanagedType.LPStr )] string Triple, out LLVMTargetRef T, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetName", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetTargetName( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetDescription", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetTargetDescription( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasJIT", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTargetHasJIT( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasTargetMachine", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTargetHasTargetMachine( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMTargetHasAsmBackend", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTargetHasAsmBackend( LLVMTargetRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetMachine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMTargetMachineRef LLVMCreateTargetMachine( LLVMTargetRef T, [MarshalAs( UnmanagedType.LPStr )] string Triple, [MarshalAs( UnmanagedType.LPStr )] string CPU, [MarshalAs( UnmanagedType.LPStr )] string Features, LLVMCodeGenOptLevel Level, LLVMRelocMode Reloc, LLVMCodeModel CodeModel );
        }
    }
}
