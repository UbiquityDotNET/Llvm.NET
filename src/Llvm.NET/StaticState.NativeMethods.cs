// <copyright file="StaticState.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Llvm.NET.Properties;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public static partial class StaticState
    {
        internal static class NativeMethods
        {
            /// <summary>Dynamically loads a DLL from a directory dependent on the current architecture</summary>
            /// <param name="moduleName">name of the DLL</param>
            /// <param name="alternatePaths">alternate path locations to use to search for the DLL</param>
            /// <returns>Handle for the DLL</returns>
            internal static IntPtr LoadWin32Library( string moduleName, IEnumerable<string> alternatePaths )
            {
                if( string.IsNullOrWhiteSpace( moduleName ) )
                {
                    throw new ArgumentNullException( nameof( moduleName ) );
                }

                var searchCookies = ( from path in alternatePaths
                                      where Directory.Exists( path )
                                      select (Cookie: AddDllDirectory( path ), Path: path)
                                    ).ToList( );

                try
                {
                    IntPtr moduleHandle = LoadLibraryExW( moduleName, IntPtr.Zero, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS );
                    if( moduleHandle != IntPtr.Zero )
                    {
                        return moduleHandle;
                    }

                    int lastError = Marshal.GetLastWin32Error( );
                    string errMessage = string.Format( Resources.LoadWin32Library_Error_0_occured_loading_1_search_paths_2, lastError, moduleName, string.Join( "\n", searchCookies.Select( p => p.Path ) ) );
                    throw new Win32Exception( lastError, errMessage );
                }
                finally
                {
                    foreach( var c in searchCookies )
                    {
                        RemoveDllDirectory( c.Cookie );
                    }
                }
            }

            [DllImport( "kernel32", SetLastError = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool FreeLibrary( IntPtr hModule );

            internal const UInt32 LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
            /* private const UInt32 LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400; */

            [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern IntPtr LoadLibraryExW( [MarshalAs( UnmanagedType.LPTStr )]string lpFileName, IntPtr hFile, UInt32 dwFlags );

            [DllImport( "kernel32", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern IntPtr AddDllDirectory( [MarshalAs( UnmanagedType.LPWStr )]string lp );

            [DllImport( "kernel32", SetLastError = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool RemoveDllDirectory( IntPtr dwCookie );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInstallFatalErrorHandler( LLVMFatalErrorHandler Handler );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMResetFatalErrorHandler( );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMEnablePrettyStackTrace( );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMGetVersionInfo( out LLVMVersionInfo pVersionInfo );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMShutdown( );

            [DllImport( LibraryPath, EntryPoint = "LLVMParseCommandLineOptions", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern void LLVMParseCommandLineOptions( int argc, string[ ] argv, [MarshalAs( UnmanagedType.LPStr )] string Overview );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAMDGPUTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeHexagonTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeNVPTXTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMSP430TargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeXCoreTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64TargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86TargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetInfo", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeBPFTargetInfo( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAMDGPUTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeHexagonTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeNVPTXTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430Target", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMSP430Target( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeXCoreTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Target", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64Target( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Target", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86Target( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeBPFTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAMDGPUTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeHexagonTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeNVPTXTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430TargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMSP430TargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeXCoreTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64TargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64TargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86TargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86TargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFTargetMC", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeBPFTargetMC( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAMDGPUAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeHexagonAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNVPTXAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeNVPTXAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMSP430AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMSP430AsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeXCoreAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64AsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86AsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeBPFAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeBPFAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAMDGPUAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAMDGPUAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64AsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64AsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86AsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86AsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSystemZDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSystemZDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeHexagonDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeHexagonDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeXCoreDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeXCoreDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeMipsDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeMipsDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAArch64Disassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAArch64Disassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeARMDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeARMDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializePowerPCDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializePowerPCDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeSparcDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeSparcDisassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeX86Disassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeX86Disassembler( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetInfos", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllTargetInfos( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargets", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllTargets( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllTargetMCs", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllTargetMCs( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmPrinters", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllAsmPrinters( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllAsmParsers", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllAsmParsers( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeAllDisassemblers", CallingConvention = CallingConvention.Cdecl )]
            internal static extern void LLVMInitializeAllDisassemblers( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeTarget", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMInitializeNativeTarget( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmParser", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMInitializeNativeAsmParser( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeAsmPrinter", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMInitializeNativeAsmPrinter( );

            [DllImport( LibraryPath, EntryPoint = "LLVMInitializeNativeDisassembler", CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMInitializeNativeDisassembler( );
        }
    }
}
