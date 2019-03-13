// <copyright file="Context.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;
using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET
{
    /// <summary>Encapsulates an LLVM context</summary>
    public sealed partial class Context
    {
        internal static class NativeMethods
        {
            internal enum LLVMDiagnosticSeverity
            {
                LLVMDSError = 0,
                LLVMDSWarning = 1,
                LLVMDSRemark = 2,
                LLVMDSNote = 3
            }

            [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
            internal delegate void LLVMDiagnosticHandler( LLVMDiagnosticInfoRef param0, IntPtr param1 );

            [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
            internal delegate void LLVMYieldCallback( LLVMContextRef param0, IntPtr param1 );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoDescription", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMGetDiagInfoDescription( LLVMDiagnosticInfoRef DI );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetDiagInfoSeverity", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMDiagnosticSeverity LLVMGetDiagInfoSeverity( LLVMDiagnosticInfoRef DI );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMFunctionType( LLVMTypeRef ReturnType, out LLVMTypeRef ParamTypes, uint ParamCount, [MarshalAs( UnmanagedType.Bool )]bool IsVarArg );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMContextCreateBasicBlock( LLVMContextRef context
                                                                               , [MarshalAs( UnmanagedType.LPStr )] string name
                                                                               , LLVMValueRef /*Function*/ function
                                                                               , LLVMBasicBlockRef insertBefore
                                                                               );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern uint LLVMGetMDKindIDInContext( LLVMContextRef C, [MarshalAs( UnmanagedType.LPStr )] string Name, uint SLen );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMContextGetIsODRUniquingDebugTypes( LLVMContextRef context );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMContextSetIsODRUniquingDebugTypes( LLVMContextRef context, [MarshalAs( UnmanagedType.Bool )] bool state );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]

            internal static extern LLVMMetadataRef LLVMMDString2( LLVMContextRef C, [MarshalAs( UnmanagedType.LPStr )] string Str, UInt32 SLen );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMMetadataRef LLVMMDNode2( LLVMContextRef C, out LLVMMetadataRef MDs, UInt32 Count );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt1TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt1TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt8TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt8TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt16TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt16TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt32TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt32TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt64TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt64TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMInt128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMInt128TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMIntTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMIntTypeInContext( LLVMContextRef C, uint NumBits );

            [DllImport( LibraryPath, EntryPoint = "LLVMHalfTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMHalfTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMFloatTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMFloatTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMDoubleTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMDoubleTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMTokenTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMTokenTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMMetadataTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMMetadataTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMX86FP80TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMX86FP80TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMFP128TypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMPPCFP128TypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMPPCFP128TypeInContext( LLVMContextRef C );

            // ReSharper disable CommentTypo
            /*[DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            //internal static extern LLVMMetadataRef LLVMTemporaryMDNode( LLVMContextRef C, out LLVMMetadataRef MDs, UInt32 Count );

            //[DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            //[return: MarshalAs( UnmanagedType.Bool )]
            //internal static extern bool LLVMRunPassPipeline( LLVMContextRef context, LLVMModuleRef M, LLVMTargetMachineRef TM, [MarshalAs( UnmanagedType.LPStr )] string passPipeline, LLVMOptVerifierKind VK, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveAssemblyUseListOrder, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveBitcodeUseListOrder );
            */

            [DllImport( LibraryPath, EntryPoint = "LLVMStructTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMStructTypeInContext( LLVMContextRef C, out LLVMTypeRef ElementTypes, uint ElementCount, [MarshalAs( UnmanagedType.Bool )]bool Packed );

            [DllImport( LibraryPath, EntryPoint = "LLVMStructCreateNamed", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMTypeRef LLVMStructCreateNamed( LLVMContextRef C, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMVoidTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMVoidTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMLabelTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMLabelTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMX86MMXTypeInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTypeRef LLVMX8MMXTypeInContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMContextCreate", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMContextRef LLVMContextCreate( );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMContextAlias LLVMGetGlobalContext( );

            [DllImport( LibraryPath, EntryPoint = "LLVMContextSetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMContextSetDiagnosticHandler( LLVMContextRef C, IntPtr Handler, IntPtr DiagnosticContext );

            [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticHandler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMDiagnosticHandler LLVMContextGetDiagnosticHandler( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMContextGetDiagnosticContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMContextGetDiagnosticContext( LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMContextSetYieldCallback", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMContextSetYieldCallback( LLVMContextRef C, LLVMYieldCallback Callback, IntPtr OpaqueHandle );

            [DllImport( LibraryPath, EntryPoint = "LLVMParseIRInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMParseIRInContext( LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM, out IntPtr OutMessage );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstStringInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMConstStringInContext( LLVMContextRef C, [MarshalAs( UnmanagedType.LPStr )] string Str, uint Length, [MarshalAs( UnmanagedType.Bool )]bool DontNullTerminate );

            [DllImport( LibraryPath, EntryPoint = "LLVMConstStructInContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMConstStructInContext( LLVMContextRef C, out LLVMValueRef ConstantVals, uint Count, [MarshalAs( UnmanagedType.Bool )]bool Packed );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateEnumAttribute", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMAttributeRef LLVMCreateEnumAttribute( LLVMContextRef C, uint KindID, ulong Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateStringAttribute", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMAttributeRef LLVMCreateStringAttribute( LLVMContextRef C, [MarshalAs( UnmanagedType.LPStr )] string K, uint KLength, [MarshalAs( UnmanagedType.LPStr )] string V, uint VLength );

            [DllImport( LibraryPath, EntryPoint = "LLVMAppendBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMAppendBasicBlockInContext( LLVMContextRef C, LLVMValueRef Fn, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMInsertBasicBlockInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMBasicBlockRef LLVMInsertBasicBlockInContext( LLVMContextRef C, LLVMBasicBlockRef BB, [MarshalAs( UnmanagedType.LPStr )] string Name );
        }
    }
}
