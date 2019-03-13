// <copyright file="TargetMachine.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using static Llvm.NET.Target.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Target specific code generation information</summary>
    public sealed partial class TargetMachine
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTargetMachine", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeTargetMachine( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetRef LLVMGetTargetMachineTarget( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineTriple", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMGetTargetMachineTriple( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineCPU", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMGetTargetMachineCPU( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTargetMachineFeatureString", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMGetTargetMachineFeatureString( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateTargetDataLayout", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetDataRef LLVMCreateTargetDataLayout( LLVMTargetMachineRef T );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetTargetMachineAsmVerbosity", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMSetTargetMachineAsmVerbosity( LLVMTargetMachineRef T, [MarshalAs( UnmanagedType.Bool )]bool VerboseAsm );

            [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMStatus LLVMTargetMachineEmitToFile( LLVMTargetMachineRef T, LLVMModuleRef M, string Filename, LLVMCodeGenFileType codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage );

            [DllImport( LibraryPath, EntryPoint = "LLVMTargetMachineEmitToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMTargetMachineEmitToMemoryBuffer( LLVMTargetMachineRef T, LLVMModuleRef M, LLVMCodeGenFileType codegen, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage, out LLVMMemoryBufferRef OutMemBuf );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddAnalysisPasses", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMAddAnalysisPasses( LLVMTargetMachineRef T, LLVMPassManagerRef PM );
        }
    }
}
