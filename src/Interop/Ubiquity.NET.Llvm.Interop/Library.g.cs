// <copyright file="StaticState.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System.Reflection;

namespace Ubiquity.NET.Llvm.Interop
{
    // TODO: Figure out how to read targets.def, AsmPrinters.def, AsmParsers.def, Disassemblers.def
    // to get the full set of target architectures and which one supports which target registrations
    // and generate all of the registration (including skipping of any init calls that don't exist).

    /// <summary>Provides support for various LLVM static state initialization and manipulation</summary>
    public sealed partial class Library
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

            case CodeGenTarget.AVR:
                RegisterAVR( registrations );
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

            case CodeGenTarget.LoongArch:
                RegisterLoongArch( registrations );
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

            case CodeGenTarget.RISCV:
                RegisterRISCV( registrations );
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

            case CodeGenTarget.All:
                RegisterAll( registrations );
                break;
            }
        }

        // basic pattern to follow for any new targets in the future
        /*
        internal static void RegisterXXX( TargetRegistration registrations = TargetRegistration.All )
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

        internal static void RegisterNative(TargetRegistration registrations = TargetRegistration.All)
        {
            if(registrations.HasFlag( TargetRegistration.Target ))
            {
                LLVMInitializeNativeTarget();
            }

            /* Not supported for the this target
            if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            {
                LLVMInitializeNativeTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            {
                LLVMInitializeNativeTargetMC( );
            }
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

        internal static void RegisterAVR( TargetRegistration registrations = TargetRegistration.All )
        {
            if( registrations.HasFlag( TargetRegistration.Target ) )
            {
                LLVMInitializeAVRTarget( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            {
                LLVMInitializeAVRTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            {
                LLVMInitializeAVRTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmPrinter ) )
            {
                LLVMInitializeAVRAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            {
                LLVMInitializeAVRDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeAVRAsmParser( );
            }
        }

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

        internal static void RegisterLoongArch( TargetRegistration registrations = TargetRegistration.All )
        {
            if( registrations.HasFlag( TargetRegistration.Target ) )
            {
                LLVMInitializeLoongArchTarget( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetInfo ) )
            {
                LLVMInitializeLoongArchTargetInfo( );
            }

            if( registrations.HasFlag( TargetRegistration.TargetMachine ) )
            {
                LLVMInitializeLoongArchTargetMC( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmPrinter ) )
            {
                LLVMInitializeLoongArchAsmPrinter( );
            }

            if( registrations.HasFlag( TargetRegistration.Disassembler ) )
            {
                LLVMInitializeLoongArchDisassembler( );
            }

            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeLoongArchAsmParser( );
            }
        }

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

            /* Not supported for this target
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

            /* Not supported for this target
            if( registrations.HasFlag( TargetRegistration.AsmParser ) )
            {
                LLVMInitializeXCoreAsmParser( );
            }
            */
        }

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
    }
}
