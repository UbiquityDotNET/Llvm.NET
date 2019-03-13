// <copyright file="Module.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

using CallingConvention = System.Runtime.InteropServices.CallingConvention;

namespace Llvm.NET
{
    /// <summary>LLVM Bit-code module</summary>
    public sealed partial class BitcodeModule
    {
        internal static class NativeMethods
        {
#pragma warning disable CA1008 // Enums should have zero value.
            internal enum LLVMModFlagBehavior
            {
                Error = 1,
                Warning = 2,
                Require = 3,
                Override = 4,
                Append = 5,
                AppendUnique = 6,
                ModFlagBehaviorFirstVal = Error,
                ModFlagBehaviorLastVal = AppendUnique
            }
#pragma warning restore CA1008 // Enums should have zero value.

            [DllImport( LibraryPath, EntryPoint = "LLVMLinkModules2", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMLinkModules2( LLVMModuleRef Dest, LLVMModuleRef Src );

            [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMModuleRef LLVMModuleCreateWithName( [MarshalAs( UnmanagedType.LPStr )] string ModuleID );

            [DllImport( LibraryPath, EntryPoint = "LLVMModuleCreateWithNameInContext", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMModuleRef LLVMModuleCreateWithNameInContext( [MarshalAs( UnmanagedType.LPStr )] string ModuleID, LLVMContextRef C );

            [DllImport( LibraryPath, EntryPoint = "LLVMCloneModule", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMModuleRef LLVMCloneModule( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModule", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeModule( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleIdentifier", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetModuleIdentifier( LLVMModuleRef M, out size_t Len );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleIdentifier", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetModuleIdentifier( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Ident, size_t Len );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetDataLayoutStr", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetDataLayoutStr( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetDataLayout", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetDataLayoutStr( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string DataLayoutStr );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTarget", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetTarget( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetTarget", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetTarget( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Triple );

            [DllImport( LibraryPath, EntryPoint = "LLVMDumpModule", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDumpModule( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMStatus LLVMPrintModuleToFile( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Filename, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string ErrorMessage );

            [DllImport( LibraryPath, EntryPoint = "LLVMPrintModuleToString", CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMPrintModuleToString( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleInlineAsm", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMSetModuleInlineAsm( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Asm );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleContext", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMContextAlias LLVMGetModuleContext( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetTypeByName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMTypeRef LLVMGetTypeByName( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataNumOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern uint LLVMGetNamedMetadataNumOperands( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedMetadataOperands", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMGetNamedMetadataOperands( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name, out LLVMValueRef Dest );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddNamedMetadataOperand", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMAddNamedMetadataOperand( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMValueRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMAddFunction( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMTypeRef FunctionTy );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedFunction", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMGetNamedFunction( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstFunction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetFirstFunction( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastFunction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetLastFunction( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextFunction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetNextFunction( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousFunction", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPreviousFunction( LLVMValueRef Fn );

            [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFile", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMStatus LLVMWriteBitcodeToFile( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Path );

            [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFD", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMWriteBitcodeToFD( LLVMModuleRef M, int FD, int ShouldClose, int Unbuffered );

            [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFileHandle", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMWriteBitcodeToFileHandle( LLVMModuleRef M, int Handle );

            [DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToMemoryBuffer", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMMemoryBufferRef LLVMWriteBitcodeToMemoryBuffer( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMCreateModuleProviderForExistingModule", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMModuleProviderRef LLVMCreateModuleProviderForExistingModule( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMDisposeModuleProvider", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMDisposeModuleProvider( LLVMModuleProviderRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcode2", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMParseBitcode2( LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModule2", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMGetBitcodeModule2( LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM );

            [DllImport( LibraryPath, EntryPoint = "LLVMParseBitcodeInContext2", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMParseBitcodeInContext2( LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutModule );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetBitcodeModuleInContext2", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMGetBitcodeModuleInContext2( LLVMContextRef ContextRef, LLVMMemoryBufferRef MemBuf, out LLVMModuleRef OutM );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMVerifyModule( LLVMModuleRef M, LLVMVerifierFailureAction Action, [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )] out string OutMessage );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMSharedModuleRef LLVMOrcMakeSharedModule( LLVMModuleRef Mod );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMModuleGetFirstGlobalAlias( LLVMModuleRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMModuleGetNextGlobalAlias( LLVMValueRef /*GlobalAlias*/ valueRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMNamedMDNodeRef LLVMModuleGetFirstNamedMD( LLVMModuleRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMNamedMDNodeRef LLVMModuleGetNextNamedMD( LLVMNamedMDNodeRef nodeRef );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef /*Function*/ LLVMIntrinsicGetDeclaration( LLVMModuleRef m, UInt32 id, out LLVMTypeRef paramTypes, uint paramCount );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMIsIntrinsicOverloaded( UInt32 id );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetModuleSourceFileName( LLVMModuleRef module );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMSetModuleSourceFileName( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
            internal static extern string LLVMGetModuleName( LLVMModuleRef module );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddModuleFlag( LLVMModuleRef M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string name, UInt32 value );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddModuleFlagMetadata", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddModuleFlag( LLVMModuleRef M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMMetadataRef value );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMNamedMDNodeRef LLVMModuleGetModuleFlagsMetadata( LLVMModuleRef module );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMGetOrInsertFunction( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMTypeRef functionType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMValueRef LLVMGetGlobalAlias( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMAddGlobal( LLVMModuleRef M, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddGlobalInAddressSpace", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMAddGlobalInAddressSpace( LLVMModuleRef M, LLVMTypeRef Ty, [MarshalAs( UnmanagedType.LPStr )] string Name, uint AddressSpace );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNamedGlobal", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMGetNamedGlobal( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetFirstGlobal", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetFirstGlobal( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetLastGlobal", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetLastGlobal( LLVMModuleRef M );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetNextGlobal", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetNextGlobal( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetPreviousGlobal", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern LLVMValueRef LLVMGetPreviousGlobal( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMDeleteGlobal", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl )]
            internal static extern void LLVMDeleteGlobal( LLVMValueRef GlobalVar );

            [DllImport( LibraryPath, EntryPoint = "LLVMAddAlias", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMValueRef LLVMAddAlias( LLVMModuleRef M, LLVMTypeRef Ty, LLVMValueRef Aliasee, [MarshalAs( UnmanagedType.LPStr )] string Name );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMAddNamedMetadataOperand2( LLVMModuleRef M, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMMetadataRef Val );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleDataLayout", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetDataAlias LLVMGetModuleDataLayout( LLVMModuleRef M );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTargetDataAlias LLVMSetModuleDataLayout( LLVMModuleRef M, LLVMTargetDataRef DL );
        }
    }
}
