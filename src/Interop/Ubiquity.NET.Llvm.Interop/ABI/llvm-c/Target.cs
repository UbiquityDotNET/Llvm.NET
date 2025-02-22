// -----------------------------------------------------------------------
// <copyright file="Target.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LLVMByteOrdering
        : Int32
    {
        LLVMBigEndian = 0,
        LLVMLittleEndian = 1,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64TargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430TargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeNVPTXTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSPIRVTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVETargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86TargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeXCoreTargetInfo();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64Target();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430Target();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeNVPTXTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSPIRVTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVETarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86Target();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeXCoreTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64TargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430TargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeNVPTXTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSPIRVTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVETargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86TargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeXCoreTargetMC();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64AsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430AsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeNVPTXAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSPIRVAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVEAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86AsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeXCoreAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64AsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430AsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVEAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86AsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAArch64Disassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAMDGPUDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeARMDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAVRDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeBPFDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeHexagonDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLanaiDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeLoongArchDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMipsDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeMSP430Disassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializePowerPCDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeRISCVDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSparcDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeSystemZDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeVEDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeWebAssemblyDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeX86Disassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeXCoreDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllTargetInfos();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllTargets();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllTargetMCs();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllAsmPrinters();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllAsmParsers();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInitializeAllDisassemblers();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMInitializeNativeTarget();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMInitializeNativeAsmParser();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMInitializeNativeAsmPrinter();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMStatus LLVMInitializeNativeDisassembler();

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRefAlias LLVMGetModuleDataLayout(LLVMModuleRef M);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetModuleDataLayout(LLVMModuleRef M, LLVMTargetDataRef DL);

        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTargetDataRef LLVMCreateTargetData(string StringRep);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMAddTargetLibraryInfo(LLVMTargetLibraryInfoRef TLI, LLVMPassManagerRef PM);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LLVMCopyStringRepOfTargetData(LLVMTargetDataRef TD);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LLVMByteOrdering LLVMByteOrder(LLVMTargetDataRef TD);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMPointerSize(LLVMTargetDataRef TD);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMPointerSizeForAS(LLVMTargetDataRef TD, uint AS);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntPtrType(LLVMTargetDataRef TD);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntPtrTypeForAS(LLVMTargetDataRef TD, uint AS);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntPtrTypeInContext(LLVMContextRef C, LLVMTargetDataRef TD);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMTypeRef LLVMIntPtrTypeForASInContext(LLVMContextRef C, LLVMTargetDataRef TD, uint AS);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMSizeOfTypeInBits(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMStoreSizeOfType(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMABISizeOfType(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMABIAlignmentOfType(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMCallFrameAlignmentOfType(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMPreferredAlignmentOfType(LLVMTargetDataRef TD, LLVMTypeRef Ty);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMPreferredAlignmentOfGlobal(LLVMTargetDataRef TD, LLVMValueRef GlobalVar);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMElementAtOffset(LLVMTargetDataRef TD, LLVMTypeRef StructTy, ulong Offset);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial ulong LLVMOffsetOfElement(LLVMTargetDataRef TD, LLVMTypeRef StructTy, uint Element);
    }
}
