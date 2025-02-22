// -----------------------------------------------------------------------
// <copyright file="TripleBindings.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    public enum LibLLVMTripleArchType
        : Int32
    {
        LibLLVMTripleArchType_UnknownArch = 0,
        LibLLVMTripleArchType_arm = 1,
        LibLLVMTripleArchType_armeb = 2,
        LibLLVMTripleArchType_aarch64 = 3,
        LibLLVMTripleArchType_aarch64_be = 4,
        LibLLVMTripleArchType_aarch64_32 = 5,
        LibLLVMTripleArchType_arc = 6,
        LibLLVMTripleArchType_avr = 7,
        LibLLVMTripleArchType_bpfel = 8,
        LibLLVMTripleArchType_bpfeb = 9,
        LibLLVMTripleArchType_hexagon = 10,
        LibLLVMTripleArchType_mips = 11,
        LibLLVMTripleArchType_mipsel = 12,
        LibLLVMTripleArchType_mips64 = 13,
        LibLLVMTripleArchType_mips64el = 14,
        LibLLVMTripleArchType_msp430 = 15,
        LibLLVMTripleArchType_ppc = 16,
        LibLLVMTripleArchType_ppc64 = 17,
        LibLLVMTripleArchType_ppc64le = 18,
        LibLLVMTripleArchType_r600 = 19,
        LibLLVMTripleArchType_amdgcn = 20,
        LibLLVMTripleArchType_riscv32 = 21,
        LibLLVMTripleArchType_riscv64 = 22,
        LibLLVMTripleArchType_sparc = 23,
        LibLLVMTripleArchType_sparcv9 = 24,
        LibLLVMTripleArchType_sparcel = 25,
        LibLLVMTripleArchType_systemz = 26,
        LibLLVMTripleArchType_tce = 27,
        LibLLVMTripleArchType_tcele = 28,
        LibLLVMTripleArchType_thumb = 29,
        LibLLVMTripleArchType_thumbeb = 30,
        LibLLVMTripleArchType_x86 = 31,
        LibLLVMTripleArchType_x86_64 = 32,
        LibLLVMTripleArchType_xcore = 33,
        LibLLVMTripleArchType_nvptx = 34,
        LibLLVMTripleArchType_nvptx64 = 35,
        LibLLVMTripleArchType_le32 = 36,
        LibLLVMTripleArchType_le64 = 37,
        LibLLVMTripleArchType_amdil = 38,
        LibLLVMTripleArchType_amdil64 = 39,
        LibLLVMTripleArchType_hsail = 40,
        LibLLVMTripleArchType_hsail64 = 41,
        LibLLVMTripleArchType_spir = 42,
        LibLLVMTripleArchType_spir64 = 43,
        LibLLVMTripleArchType_kalimba = 44,
        LibLLVMTripleArchType_shave = 45,
        LibLLVMTripleArchType_lanai = 46,
        LibLLVMTripleArchType_wasm32 = 47,
        LibLLVMTripleArchType_wasm64 = 48,
        LibLLVMTripleArchType_renderscript32 = 49,
        LibLLVMTripleArchType_renderscript64 = 50,
        LibLLVMTripleArchType_ve = 51,
        LibLLVMTripleArchType_LastArchType = 51,
    }

    public enum LibLLVMTripleSubArchType
        : Int32
    {
        LibLLVMTripleSubArchType_NoSubArch = 0,
        LibLLVMTripleSubArchType_ARMSubArch_v8_5a = 1,
        LibLLVMTripleSubArchType_ARMSubArch_v8_4a = 2,
        LibLLVMTripleSubArchType_ARMSubArch_v8_3a = 3,
        LibLLVMTripleSubArchType_ARMSubArch_v8_2a = 4,
        LibLLVMTripleSubArchType_ARMSubArch_v8_1a = 5,
        LibLLVMTripleSubArchType_ARMSubArch_v8 = 6,
        LibLLVMTripleSubArchType_ARMSubArch_v8r = 7,
        LibLLVMTripleSubArchType_ARMSubArch_v8m_baseline = 8,
        LibLLVMTripleSubArchType_ARMSubArch_v8m_mainline = 9,
        LibLLVMTripleSubArchType_ARMSubArch_v8_1m_mainline = 10,
        LibLLVMTripleSubArchType_ARMSubArch_v7 = 11,
        LibLLVMTripleSubArchType_ARMSubArch_v7em = 12,
        LibLLVMTripleSubArchType_ARMSubArch_v7m = 13,
        LibLLVMTripleSubArchType_ARMSubArch_v7s = 14,
        LibLLVMTripleSubArchType_ARMSubArch_v7k = 15,
        LibLLVMTripleSubArchType_ARMSubArch_v7ve = 16,
        LibLLVMTripleSubArchType_ARMSubArch_v6 = 17,
        LibLLVMTripleSubArchType_ARMSubArch_v6m = 18,
        LibLLVMTripleSubArchType_ARMSubArch_v6k = 19,
        LibLLVMTripleSubArchType_ARMSubArch_v6t2 = 20,
        LibLLVMTripleSubArchType_ARMSubArch_v5 = 21,
        LibLLVMTripleSubArchType_ARMSubArch_v5te = 22,
        LibLLVMTripleSubArchType_ARMSubArch_v4t = 23,
        LibLLVMTripleSubArchType_KalimbaSubArch_v3 = 24,
        LibLLVMTripleSubArchType_KalimbaSubArch_v4 = 25,
        LibLLVMTripleSubArchType_KalimbaSubArch_v5 = 26,
        LibLLVMTripleSubArchType_MipsSubArch_r6 = 27,
        LibLLVMTripleSubArchType_PPCSubArch_spe = 28,
    }

    public enum LibLLVMTripleVendorType
        : Int32
    {
        LibLLVMTripleVendorType_UnknownVendor = 0,
        LibLLVMTripleVendorType_Apple = 1,
        LibLLVMTripleVendorType_PC = 2,
        LibLLVMTripleVendorType_SCEI = 3,
        LibLLVMTripleVendorType_BGP = 4,
        LibLLVMTripleVendorType_BGQ = 5,
        LibLLVMTripleVendorType_Freescale = 6,
        LibLLVMTripleVendorType_IBM = 7,
        LibLLVMTripleVendorType_ImaginationTechnologies = 8,
        LibLLVMTripleVendorType_MipsTechnologies = 9,
        LibLLVMTripleVendorType_NVIDIA = 10,
        LibLLVMTripleVendorType_CSR = 11,
        LibLLVMTripleVendorType_Myriad = 12,
        LibLLVMTripleVendorType_AMD = 13,
        LibLLVMTripleVendorType_Mesa = 14,
        LibLLVMTripleVendorType_SUSE = 15,
        LibLLVMTripleVendorType_OpenEmbedded = 16,
        LibLLVMTripleVendorType_LastVendorType = 16,
    }

    public enum LibLLVMTripleOSType
        : Int32
    {
        LibLLVMTripleOSType_UnknownOS = 0,
        LibLLVMTripleOSType_Ananas = 1,
        LibLLVMTripleOSType_CloudABI = 2,
        LibLLVMTripleOSType_Darwin = 3,
        LibLLVMTripleOSType_DragonFly = 4,
        LibLLVMTripleOSType_FreeBSD = 5,
        LibLLVMTripleOSType_Fuchsia = 6,
        LibLLVMTripleOSType_IOS = 7,
        LibLLVMTripleOSType_KFreeBSD = 8,
        LibLLVMTripleOSType_Linux = 9,
        LibLLVMTripleOSType_Lv2 = 10,
        LibLLVMTripleOSType_MacOSX = 11,
        LibLLVMTripleOSType_NetBSD = 12,
        LibLLVMTripleOSType_OpenBSD = 13,
        LibLLVMTripleOSType_Solaris = 14,
        LibLLVMTripleOSType_Win32 = 15,
        LibLLVMTripleOSType_Haiku = 16,
        LibLLVMTripleOSType_Minix = 17,
        LibLLVMTripleOSType_RTEMS = 18,
        LibLLVMTripleOSType_NaCl = 19,
        LibLLVMTripleOSType_CNK = 20,
        LibLLVMTripleOSType_AIX = 21,
        LibLLVMTripleOSType_CUDA = 22,
        LibLLVMTripleOSType_NVCL = 23,
        LibLLVMTripleOSType_AMDHSA = 24,
        LibLLVMTripleOSType_PS4 = 25,
        LibLLVMTripleOSType_ELFIAMCU = 26,
        LibLLVMTripleOSType_TvOS = 27,
        LibLLVMTripleOSType_WatchOS = 28,
        LibLLVMTripleOSType_Mesa3D = 29,
        LibLLVMTripleOSType_Contiki = 30,
        LibLLVMTripleOSType_AMDPAL = 31,
        LibLLVMTripleOSType_HermitCore = 32,
        LibLLVMTripleOSType_Hurd = 33,
        LibLLVMTripleOSType_WASI = 34,
        LibLLVMTripleOSType_Emscripten = 35,
        LibLLVMTripleOSType_LastOSType = 35,
    }

    public enum LibLLVMTripleEnvironmentType
        : Int32
    {
        LibLLVMTripleEnvironmentType_UnknownEnvironment = 0,
        LibLLVMTripleEnvironmentType_GNU = 1,
        LibLLVMTripleEnvironmentType_GNUABIN32 = 2,
        LibLLVMTripleEnvironmentType_GNUABI64 = 3,
        LibLLVMTripleEnvironmentType_GNUEABI = 4,
        LibLLVMTripleEnvironmentType_GNUEABIHF = 5,
        LibLLVMTripleEnvironmentType_GNUX32 = 6,
        LibLLVMTripleEnvironmentType_CODE16 = 7,
        LibLLVMTripleEnvironmentType_EABI = 8,
        LibLLVMTripleEnvironmentType_EABIHF = 9,
        LibLLVMTripleEnvironmentType_Android = 10,
        LibLLVMTripleEnvironmentType_Musl = 11,
        LibLLVMTripleEnvironmentType_MuslEABI = 12,
        LibLLVMTripleEnvironmentType_MuslEABIHF = 13,
        LibLLVMTripleEnvironmentType_MSVC = 14,
        LibLLVMTripleEnvironmentType_Itanium = 15,
        LibLLVMTripleEnvironmentType_Cygnus = 16,
        LibLLVMTripleEnvironmentType_CoreCLR = 17,
        LibLLVMTripleEnvironmentType_Simulator = 18,
        LibLLVMTripleEnvironmentType_MacABI = 19,
        LibLLVMTripleEnvironmentType_LastEnvironmentType = 19,
    }

    public enum LibLLVMTripleObjectFormatType
        : Int32
    {
        LibLLVMTripleObjectFormatType_UnknownObjectFormat = 0,
        LibLLVMTripleObjectFormatType_COFF = 1,
        LibLLVMTripleObjectFormatType_ELF = 2,
        LibLLVMTripleObjectFormatType_MachO = 3,
        LibLLVMTripleObjectFormatType_Wasm = 4,
        LibLLVMTripleObjectFormatType_XCOFF = 5,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( Names.LibraryPath, StringMarshallingCustomType = typeof( AnsiStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleRef LibLLVMParseTriple(string triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMTripleOpEqual(LibLLVMTripleRef lhs, LibLLVMTripleRef rhs);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleArchType LibLLVMTripleGetArchType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleSubArchType LibLLVMTripleGetSubArchType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleVendorType LibLLVMTripleGetVendorType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleOSType LibLLVMTripleGetOsType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMTripleHasEnvironment(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleEnvironmentType LibLLVMTripleGetEnvironmentType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMTripleGetEnvironmentVersion(LibLLVMTripleRef triple, out uint major, out uint minor, out uint build);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleObjectFormatType LibLLVMTripleGetObjectFormatType(LibLLVMTripleRef triple);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleAsString(LibLLVMTripleRef triple, [MarshalAs( UnmanagedType.Bool )] bool normalize);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetArchTypeName(LibLLVMTripleArchType type);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetSubArchTypeName(LibLLVMTripleSubArchType type);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetVendorTypeName(LibLLVMTripleVendorType vendor);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetOsTypeName(LibLLVMTripleOSType osType);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetEnvironmentTypeName(LibLLVMTripleEnvironmentType environmentType);

        [LibraryImport( Names.LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial DisposeMessageString LibLLVMTripleGetObjectFormatTypeName(LibLLVMTripleObjectFormatType objectFormatType);
    }
}
