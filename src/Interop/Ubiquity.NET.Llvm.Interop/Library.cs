// <copyright file="StaticState.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;

using Ubiquity.NET.Llvm.Interop.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed class Library
        : ILibLlvm
    {
        /// <inheritdoc/>
        public void RegisterTarget(CodeGenTarget target, TargetRegistration registrations = TargetRegistration.All)
        {
            switch(target)
            {
            case CodeGenTarget.Native:
                RegisterNative( registrations );
                break;
            case CodeGenTarget.AArch64:
                RegisterAArch64( registrations );
                break;
            case CodeGenTarget.AMDGPU:
                RegisterAMDGPU( registrations );
                break;
            case CodeGenTarget.ARM:
                RegisterARM( registrations );
                break;
            case CodeGenTarget.BPF:
                RegisterBPF( registrations );
                break;
            case CodeGenTarget.Hexagon:
                RegisterHexagon( registrations );
                break;
            case CodeGenTarget.Lanai:
                RegisterLanai( registrations );
                break;
            case CodeGenTarget.MIPS:
                RegisterMips( registrations );
                break;
            case CodeGenTarget.MSP430:
                RegisterMSP430( registrations );
                break;
            case CodeGenTarget.NvidiaPTX:
                RegisterNVPTX( registrations );
                break;
            case CodeGenTarget.PowerPC:
                RegisterPowerPC( registrations );
                break;
            case CodeGenTarget.Sparc:
                RegisterSparc( registrations );
                break;
            case CodeGenTarget.SystemZ:
                RegisterSystemZ( registrations );
                break;
            case CodeGenTarget.WebAssembly:
                RegisterWebAssembly( registrations );
                break;
            case CodeGenTarget.X86:
                RegisterX86( registrations );
                break;
            case CodeGenTarget.XCore:
                RegisterXCore( registrations );
                break;
            case CodeGenTarget.RISCV:
                RegisterRISCV( registrations );
                break;
            case CodeGenTarget.All:
                RegisterAll( registrations );
                break;
            }
        }

        // TODO: Does LLVM 20 fix the problem of re-init from same process? [Some static init wasn't re-run and stale data left in place]

        /// <summary>Initializes the native LLVM library support</summary>
        /// <returns>
        /// <see cref="ILibLlvm"/> implementation for the library
        /// </returns>
        /// <remarks>
        /// This can only be called once per application to initialize the
        /// LLVM library. <see cref="System.IDisposable.Dispose()"/> will release
        /// any resources allocated by the library. The V10 LLVM library does
        /// *NOT* support re-initialization within the same process. Thus, this
        /// is best used at the top level of the application and released at or
        /// near process exit.
        /// </remarks>
        public static ILibLlvm InitializeLLVM()
        {
            var previousState = (InitializationState)Interlocked.CompareExchange( ref CurrentInitializationState
                                                                                , (int)InitializationState.Initializing
                                                                                , (int)InitializationState.Uninitialized
                                                                                );
            if(previousState != InitializationState.Uninitialized)
            {
                throw new InvalidOperationException( Resources.Llvm_already_initialized );
            }

            // force loading the appropriate architecture specific
            // DLL before any use of the wrapped interop APIs to
            // allow building this library as ANYCPU
            string? thisModulePath = Path.GetDirectoryName( AppContext.BaseDirectory );
            if(string.IsNullOrWhiteSpace( thisModulePath ))
            {
                throw new InvalidOperationException( Resources.Cannot_determine_assembly_location );
            }

            string packageRoot = Path.GetFullPath( Path.Combine( thisModulePath, "..", ".." ) );
            var paths = new List<string>( );

            // TODO: support other non-windows runtimes via .NET CORE
            string osArch = Environment.Is64BitProcess ? "Win-x64" : "win-x86";
            string runTimePath = Path.Combine( "runtimes", osArch, "native" );

            // .NET core apps will actually run with references directly from the NuGet install
            // but full framework apps (including unit tests will have CopyLocal applied)
            paths.Add( Path.Combine( packageRoot, runTimePath ) );
            paths.Add( Path.Combine( thisModulePath, runTimePath ) );
            paths.Add( thisModulePath );
            var hLibLLVM = LoadLibrary( "Ubiquity.NET.LibLlvm.dll", paths );

            // dispose the library in the unlikely event of an exception here
            try
            {
                // Verify the version of LLVM in LibLLVM
                LibLLVMGetVersionInfo( out LibLLVMVersionInfo versionInfo );
                if(versionInfo.Major != VersionMajor
                 || versionInfo.Minor != VersionMinor
                 || versionInfo.Patch < VersionPatch
                  )
                {
                    string msgFmt = Resources.Mismatched_LibLLVM_version_Expected_0_1_2_Actual_3_4_5;
                    string msg = string.Format( CultureInfo.CurrentCulture
                                              , msgFmt
                                              , VersionMajor
                                              , VersionMinor
                                              , VersionPatch
                                              , versionInfo.Major
                                              , versionInfo.Minor
                                              , versionInfo.Patch
                                              );

                    throw new InvalidOperationException( msg );
                }

                // initialize the static fields
                unsafe
                {
                    LLVMInstallFatalErrorHandler( &FatalErrorHandler );
                }
            }
            catch
            {
                hLibLLVM.Dispose();
                throw;
            }

            Interlocked.Exchange( ref CurrentInitializationState, (int)InitializationState.Initialized );
            return new Library( hLibLLVM );
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            InternalShutdownLLVM( ModuleHandle );
        }

        // TODO: Figure out how to read targets.def to get the full set of target architectures
        // and generate all of the registration (including skipping of any init calls that don't exist).

        // basic pattern to follow for any new targets in the future
        /*
        /// <summary>Registers components for the XXX target</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        public static void RegisterXXX( TargetRegistration registrations = TargetRegistration.All )
        {
            if( registrations.HasFlag( TargetRegistration.Target ) )
            {
                LLVMInitializeXXXTarget( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            {
                LLVMInitializeXXXTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            {
                LLVMInitializeXXXTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmPrinter ) )
            {
                LLVMInitializeXXXAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            {
                LLVMInitializeXXXDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeXXXAsmParser( );
            }
        }
        */

        /// <summary>Registers components for all available targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterAll(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeAllTargets();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeAllTargetInfos();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeAllTargetMCs();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeAllAsmPrinters();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeAllDisassemblers();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeAllAsmParsers();
            }
        }

        /// <summary>Registers components for the target representing the system the calling process is running on</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterNative(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeNativeTarget();
            }

            /* Not supported on this platform
            //if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            //    LLVMInitializeNativeTargetInfo( );

            //if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            //    LLVMInitializeNativeTargetMC( );
            */

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeNativeAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeNativeDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeNativeAsmParser();
            }
        }

        /// <summary>Registers components for ARM AArch64 target(s)</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterAArch64(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeAArch64Target();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeAArch64TargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeAArch64TargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeAArch64AsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeAArch64Disassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeAArch64AsmParser();
            }
        }

        /// <summary>Registers components for AMDGPU targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterAMDGPU(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeAMDGPUTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeAMDGPUTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeAMDGPUTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeAMDGPUAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeAMDGPUDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeAMDGPUAsmParser();
            }
        }

        /// <summary>Registers components for ARM 32bit and 16bit thumb targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterARM(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeARMTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeARMTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeARMTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeARMAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeARMDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeARMAsmParser();
            }
        }

        /// <summary>Registers components for the Berkeley Packet Filter (BPF) target</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterBPF(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeBPFTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeBPFTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeBPFTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeBPFAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeBPFDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeBPFAsmParser();
            }
        }

        /// <summary>Registers components for the Hexagon CPU</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterHexagon(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeHexagonTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeHexagonTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeHexagonTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeHexagonAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeHexagonDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeHexagonAsmParser();
            }
        }

        /// <summary>Registers components for the Lanai target</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterLanai(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeLanaiTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeLanaiTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeLanaiTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeLanaiAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeLanaiDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeLanaiAsmParser();
            }
        }

        /// <summary>Registers components for MIPS targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterMips(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeMipsTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeMipsTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeMipsTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeMipsAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeMipsDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeMipsAsmParser();
            }
        }

        /// <summary>Registers components for MSP430 targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterMSP430(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeMSP430Target();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeMSP430TargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeMSP430TargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeMSP430AsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeMSP430Disassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeMSP430AsmParser();
            }
        }

        /// <summary>Registers components for the NVPTX targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterNVPTX(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeNVPTXTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeNVPTXTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeNVPTXTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeNVPTXAsmPrinter();
            }

            /*
            if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            {
                LLVMInitializeNVPTXDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeNVPTXAsmParser( );
            }
            */
        }

        /// <summary>Registers components for the PowerPC targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterPowerPC(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializePowerPCTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializePowerPCTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializePowerPCTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializePowerPCAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializePowerPCDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializePowerPCAsmParser();
            }
        }

        /// <summary>Registers components for SPARC targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterSparc(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeSparcTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeSparcTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeSparcTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeSparcAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeSparcDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeSparcAsmParser();
            }
        }

        /// <summary>Registers components for SystemZ targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterSystemZ(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeSystemZTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeSystemZTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeSystemZTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeSystemZAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeSystemZDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeSystemZAsmParser();
            }
        }

        /// <summary>Registers components for the WebAssembly target</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterWebAssembly(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeWebAssemblyTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeWebAssemblyTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeWebAssemblyTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeWebAssemblyAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeWebAssemblyDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeWebAssemblyAsmParser();
            }
        }

        /// <summary>Registers components for X86 targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterX86(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeX86Target();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeX86TargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeX86TargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeX86AsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeX86Disassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeX86AsmParser();
            }
        }

        /// <summary>Registers components for XCore targets</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterXCore(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeXCoreTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeXCoreTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeXCoreTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeXCoreAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeXCoreDisassembler();
            }

            /*
            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeXCoreAsmParser( );
            }
            */
        }

        /// <summary>Registers components for the RISCV target</summary>
        /// <param name="registrations">Flags indicating which components to register/enable</param>
        internal static void RegisterRISCV(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeRISCVTarget();
            }

            if(registrations.HasFlag( TargetRegistration.TargetInfo ))
            {
                LLVMInitializeRISCVTargetInfo();
            }

            if(registrations.HasFlag( TargetRegistration.TargetMachine ))
            {
                LLVMInitializeRISCVTargetMC();
            }

            if(registrations.HasFlag( TargetRegistration.AsmPrinter ))
            {
                LLVMInitializeRISCVAsmPrinter();
            }

            if(registrations.HasFlag( TargetRegistration.Disassembler ))
            {
                LLVMInitializeRISCVDisassembler();
            }

            if(registrations.HasFlag( TargetRegistration.AsmParser ))
            {
                LLVMInitializeRISCVAsmParser();
            }
        }

        private Library(IDisposable moduleHandle)
        {
            ModuleHandle = moduleHandle;
        }

        private readonly IDisposable ModuleHandle;

        private enum InitializationState
        {
            Uninitialized,
            Initializing,
            Initialized,
            ShuttingDown,
            ShutDown, // NOTE: This is a terminal state, it doesn't return to uninitialized
        }

        // version info for verification of matched LibLLVM
        private const int VersionMajor = 20;
        private const int VersionMinor = 1;
        private const int VersionPatch = 0;

        private static int CurrentInitializationState;

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void FatalErrorHandler(byte* reason)
        {
            try
            {
                // NOTE: LLVM will call exit() upon return from this function and there's no way to stop it
                Trace.TraceError( "LLVM Fatal Error: '{0}'; Application will exit.", AnsiStringMarshaller.ConvertToManaged( reason ) );
            }
            catch
            {
            }
        }

        private static void InternalShutdownLLVM(IDisposable hLibLLVM)
        {
            var previousState = (InitializationState)Interlocked.CompareExchange( ref CurrentInitializationState
                                                                                , (int)InitializationState.ShuttingDown
                                                                                , (int)InitializationState.Initialized
                                                                                );
            if(previousState != InitializationState.Initialized)
            {
                throw new InvalidOperationException( Resources.Llvm_not_initialized );
            }

            LLVMShutdown();
            hLibLLVM?.Dispose();

            Interlocked.Exchange( ref CurrentInitializationState, (int)InitializationState.ShutDown );
        }
    }
}
