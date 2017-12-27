// <copyright file="StaticState.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Target tools to register/enable</summary>
    [Flags]
    public enum TargetRegistrations
    {
        /// <summary>Register nothing</summary>
        None = 0x00,

        /// <summary>Register the Target class</summary>
        Target = 0x01,

        /// <summary>Register the Target info for the target</summary>
        TargetInfo = 0x02,

        /// <summary>Register the target machine(s) for a target</summary>
        TargetMachine = 0x04,

        /// <summary>Registers the assembly source code generator for a target</summary>
        AsmPrinter = 0x08,

        /// <summary>Registers the Disassembler for a target</summary>
        Disassembler = 0x10,

        /// <summary>Registers the assembly source parser for a target</summary>
        AsmParser = 0x20,

        /// <summary>Registers all the code generation components</summary>
        CodeGen = Target | TargetInfo | TargetMachine,

        /// <summary>Registers all components</summary>
        All = CodeGen | AsmPrinter | Disassembler | AsmParser
    }

    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public static class StaticState
    {
        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns>
        /// <see cref="IDisposable"/> implementation for the library
        /// </returns>
        /// <remarks>
        /// This should be called once per application to initialize the
        /// LLVM library. <see cref="IDisposable.Dispose()"/> will release
        /// any resources allocated by the library.
        /// </remarks>
        public static IDisposable InitializeLLVM()
        {
            return LazyInitializer.EnsureInitialized( ref LlvmInitializationState
                                                    , ref LlvmStateInitialized
                                                    , ref InitializationSyncObj
                                                    , InternalInitializeLLVM
                                                    );
        }

        /// <summary>Parse a command line string for LLVM Options</summary>
        /// <param name="args">args to parse</param>
        /// <param name="overview">overview of the application for help/diagnostics</param>
        /// <remarks>
        /// Use fo this method is discouraged as calling applications should control
        /// options directly without reliance on particulars of the LLVM arument handling
        /// </remarks>
        public static void ParseCommandLineOptions( string[ ] args, string overview )
        {
            if( args == null )
            {
                throw new ArgumentNullException( nameof( args ) );
            }

            LLVMParseCommandLineOptions( args.Length, args, overview );
        }

        // basic pattern to follow for any new targets in the future
        /*
        public static void RegisterXXX( TargetRegistrations registrations = TargetRegistration.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
                LLVMNative.InitializeXXXTarget( );

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
                LLVMNative.InitializeXXXTargetInfo( );

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
                LLVMNative.InitializeXXXTargetMC( );

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
                LLVMNative.InitializeXXXAsmPrinter( );

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
                LLVMNative.InitializeXXXDisassembler( );

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
                LLVMNative.InitializeXXXAsmParser( );
        }
        */

        /// <summary>Registers components for all available targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterAll( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeAllTargets( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeAllTargetInfos( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeAllTargetMCs( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeAllAsmPrinters( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeAllDisassemblers( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeAllAsmParsers( );
            }
        }

        /// <summary>Registers components for the target representing the system the calling process is running on</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterNative( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeNativeTarget( );
            }

            /* Not supported on this platform
            //if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            //    LLVMNative.InitializeNativeTargetInfo( );

            //if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            //    LLVMNative.InitializeNativeTargetMC( );
            */

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeNativeAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeNativeDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeNativeAsmParser( );
            }
        }

        /// <summary>Registers components for ARM AArch64 target(s)</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterAArch64( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeAArch64Target( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeAArch64TargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeAArch64TargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeAArch64AsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeAArch64Disassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeAArch64AsmParser( );
            }
        }

        /// <summary>Registers components for ARM 32bit and 16bit thumb targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterARM( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeARMTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeARMTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeARMTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeARMAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeARMDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeARMAsmParser( );
            }
        }

        /// <summary>Registers components for the Hexagon CPU</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterHexagon( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeHexagonTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeHexagonTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeHexagonTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeHexagonAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeHexagonDisassembler( );
            }

            /*
            //if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            //    LLVMNative.InitializeHexagonAsmParser( );
            */
        }

        /// <summary>Registers components for MIPS targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterMips( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeMipsTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeMipsTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeMipsTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeMipsAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeMipsDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeMipsAsmParser( );
            }
        }

        /// <summary>Registers components for MSP430 targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterMSP430( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeMSP430Target( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeMSP430TargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeMSP430TargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeMSP430AsmPrinter( );
            }

            /*
            //if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            //    LLVMNative.InitializeMSP430Disassembler( );

            //if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            //    LLVMNative.InitializeMSP430AsmParser( );
            */
        }

        /// <summary>Registers components for the NVPTX targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterNVPTX( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeNVPTXTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeNVPTXTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeNVPTXTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeNVPTXAsmPrinter( );
            }

            /*
            //if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            //    LLVMNative.InitializeNVPTXDisassembler( );

            //if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            //    LLVMNative.InitializeNVPTXAsmParser( );
            */
        }

        /// <summary>Registers components for the PowerPC targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterPowerPC( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializePowerPCTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializePowerPCTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializePowerPCTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializePowerPCAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializePowerPCDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializePowerPCAsmParser( );
            }
        }

        /// <summary>Registers components for AMDGPU targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterAMDGPU( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeAMDGPUTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeAMDGPUTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeAMDGPUTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeAMDGPUAsmPrinter( );
            }

            /*
            //if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            //    LLVMNative.InitializeAMDGPUDisassembler( );
            */

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeAMDGPUAsmParser( );
            }
        }

        /// <summary>Registers components for SPARC targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterSparc( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeSparcTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeSparcTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeSparcTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeSparcAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeSparcDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeSparcAsmParser( );
            }
        }

        /// <summary>Registers components for SystemZ targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterSystemZ( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeSystemZTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeSystemZTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeSystemZTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeSystemZAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeSystemZDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeSystemZAsmParser( );
            }
        }

        /// <summary>Registers components for X86 targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterX86( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeX86Target( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeX86TargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeX86TargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeX86AsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeX86Disassembler( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmParser ) )
            {
                LLVMInitializeX86AsmParser( );
            }
        }

        /// <summary>Registers components for XCore targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterXCore( TargetRegistrations registrations = TargetRegistrations.All )
        {
            if( registrations.HasFlag( TargetRegistrations.Target ) )
            {
                LLVMInitializeXCoreTarget( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetInfo ) )
            {
                LLVMInitializeXCoreTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistrations.TargetMachine ) )
            {
                LLVMInitializeXCoreTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistrations.AsmPrinter ) )
            {
                LLVMInitializeXCoreAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistrations.Disassembler ) )
            {
                LLVMInitializeXCoreDisassembler( );
            }

            /*
            //if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            //    LLVMNative.InitializeXCoreAsmParser( );
            */
        }

        // version info for verification of matched LibLLVM
        private const int VersionMajor = 5;
        private const int VersionMinor = 0;
        private const int VersionPatch = 1;

        private static IDisposable LlvmInitializationState;
        private static object InitializationSyncObj;
        private static bool LlvmStateInitialized;

        private static void FatalErrorHandler( string reason )
        {
            // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
            Trace.TraceError( "LLVM Fatal Error: '{0}'; Application exiting.", reason );
        }

        private static IDisposable InternalInitializeLLVM( )
        {
            // force loading the appropriate architecture specific
            // DLL before any use of the wrapped inter-op APIs to
            // allow building this library as ANYCPU
            string thisModulePath = Path.GetDirectoryName( Assembly.GetExecutingAssembly( ).Location );
            string packageRoot = Path.GetFullPath( Path.Combine( thisModulePath, "..", ".." ) );
            var paths = new List<string>( );

            string osArch = Environment.Is64BitProcess ? "Win-x64" : "win-x86";
            string runTimePath = Path.Combine( "runtimes", osArch, "native" );

            // .NET core apps will actually run with references directly from the nuget install
            // but full framework apps (including unit tests will have CopyLocal applied)
            paths.Add( Path.Combine( packageRoot, runTimePath ) );
            paths.Add( Path.Combine( thisModulePath, runTimePath ) );
            IntPtr hLibLLVM = LoadWin32Library( "LibLlvm.dll", paths );

            // Verify the version of LLVM in LibLLVM
            LLVMGetVersionInfo( out LLVMVersionInfo versionInfo );
            if( versionInfo.Major != VersionMajor
             || versionInfo.Minor != VersionMinor
             || versionInfo.Patch < VersionPatch
              )
            {
                throw new InvalidOperationException( $"Mismatched LibLLVM version - Expected: {VersionMajor}.{VersionMinor}.{VersionPatch} Actual: {versionInfo.Major}.{versionInfo.Minor}.{versionInfo.Patch}" );
            }

            // initialize the static fields
            FatalErrorHandlerDelegate = new Lazy<LLVMFatalErrorHandler>( ( ) => FatalErrorHandler, LazyThreadSafetyMode.PublicationOnly );
            LLVMInstallFatalErrorHandler( FatalErrorHandlerDelegate.Value );
            return new DisposableAction( ( ) => InternalShutdownLLVM( hLibLLVM ) );
        }

        private static void InternalShutdownLLVM( IntPtr hLibLLVM )
        {
            lock( InitializationSyncObj )
            {
                LlvmInitializationState = null;
                LlvmStateInitialized = false;
                LLVMShutdown( );
                if( !hLibLLVM.IsNull( ) )
                {
                    FreeLibrary( hLibLLVM );
                }
            }
        }

        // lazy initialized singleton unmanaged delegate so it is never collected
        private static Lazy<LLVMFatalErrorHandler> FatalErrorHandlerDelegate;
    }
}
