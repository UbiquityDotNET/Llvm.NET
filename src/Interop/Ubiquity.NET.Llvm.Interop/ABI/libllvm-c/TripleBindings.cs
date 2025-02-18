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
        Unknown = 0,
        Arm = 1,
        Armeb = 2,
        Aarch64 = 3,
        Aarch64_be = 4,
        Aarch64_32 = 5,
        Arc = 6,
        Avr = 7,
        Bpfel = 8,
        Bpfeb = 9,
        Hexagon = 10,
        Mips = 11,
        Mipsel = 12,
        Mips64 = 13,
        Mips64el = 14,
        Msp430 = 15,
        Ppc = 16,
        Ppc64 = 17,
        Ppc64le = 18,
        R600 = 19,
        Amdgcn = 20,
        Riscv32 = 21,
        Riscv64 = 22,
        Sparc = 23,
        Sparcv9 = 24,
        Sparcel = 25,
        Systemz = 26,
        Tce = 27,
        Tcele = 28,
        Thumb = 29,
        Thumbeb = 30,
        X86 = 31,
        X86_64 = 32,
        Xcore = 33,
        Nvptx = 34,
        Nvptx64 = 35,
        Le32 = 36,
        Le64 = 37,
        Amdil = 38,
        Amdil64 = 39,
        Hsail = 40,
        Hsail64 = 41,
        Spir = 42,
        Spir64 = 43,
        Kalimba = 44,
        Shave = 45,
        Lanai = 46,
        Wasm32 = 47,
        Wasm64 = 48,
        Renderscript32 = 49,
        Renderscript64 = 50,
        Ve = 51,
    }

    public enum LibLLVMTripleSubArchType
        : Int32
    {
        None = 0,
        ARMSubArch_v8_5a = 1,
        ARMSubArch_v8_4a = 2,
        ARMSubArch_v8_3a = 3,
        ARMSubArch_v8_2a = 4,
        ARMSubArch_v8_1a = 5,
        ARMSubArch_v8 = 6,
        ARMSubArch_v8r = 7,
        ARMSubArch_v8m_baseline = 8,
        ARMSubArch_v8m_mainline = 9,
        ARMSubArch_v8_1m_mainline = 10,
        ARMSubArch_v7 = 11,
        ARMSubArch_v7em = 12,
        ARMSubArch_v7m = 13,
        ARMSubArch_v7s = 14,
        ARMSubArch_v7k = 15,
        ARMSubArch_v7ve = 16,
        ARMSubArch_v6 = 17,
        ARMSubArch_v6m = 18,
        ARMSubArch_v6k = 19,
        ARMSubArch_v6t2 = 20,
        ARMSubArch_v5 = 21,
        ARMSubArch_v5te = 22,
        ARMSubArch_v4t = 23,
        KalimbaSubArch_v3 = 24,
        KalimbaSubArch_v4 = 25,
        KalimbaSubArch_v5 = 26,
        MipsSubArch_r6 = 27,
        PPCSubArch_spe = 28,
    }

    public enum LibLLVMTripleVendorType
        : Int32
    {
        Unknown = 0,
        Apple = 1,
        PC = 2,
        SCEI = 3,
        BGP = 4,
        BGQ = 5,
        Freescale = 6,
        IBM = 7,
        ImaginationTechnologies = 8,
        MipsTechnologies = 9,
        NVIDIA = 10,
        CSR = 11,
        Myriad = 12,
        AMD = 13,
        Mesa = 14,
        SUSE = 15,
        OpenEmbedded = 16,
    }

    public enum LibLLVMTripleOSType
        : Int32
    {
        UnknownOS = 0,
        Ananas = 1,
        CloudABI = 2,
        Darwin = 3,
        DragonFly = 4,
        FreeBSD = 5,
        Fuchsia = 6,
        IOS = 7,
        KFreeBSD = 8,
        Linux = 9,
        Lv2 = 10,
        MacOSX = 11,
        NetBSD = 12,
        OpenBSD = 13,
        Solaris = 14,
        Win32 = 15,
        Haiku = 16,
        Minix = 17,
        RTEMS = 18,
        NaCl = 19,
        CNK = 20,
        AIX = 21,
        CUDA = 22,
        NVCL = 23,
        AMDHSA = 24,
        PS4 = 25,
        ELFIAMCU = 26,
        TvOS = 27,
        WatchOS = 28,
        Mesa3D = 29,
        Contiki = 30,
        AMDPAL = 31,
        HermitCore = 32,
        Hurd = 33,
        WASI = 34,
        Emscripten = 35,
    }

    public enum LibLLVMTripleEnvironmentType
        : Int32
    {
        UnknownEnvironment = 0,
        GNU = 1,
        GNUABIN32 = 2,
        GNUABI64 = 3,
        GNUEABI = 4,
        GNUEABIHF = 5,
        GNUX32 = 6,
        CODE16 = 7,
        EABI = 8,
        EABIHF = 9,
        Android = 10,
        Musl = 11,
        MuslEABI = 12,
        MuslEABIHF = 13,
        MSVC = 14,
        Itanium = 15,
        Cygnus = 16,
        CoreCLR = 17,
        Simulator = 18,
        MacABI = 19,
    }

    public enum LibLLVMTripleObjectFormatType
        : Int32
    {
        UnknownObjectFormat = 0,
        COFF = 1,
        ELF = 2,
        MachO = 3,
        Wasm = 4,
        XCOFF = 5,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LibLLVMTripleRef LibLLVMParseTriple([MarshalUsing( typeof( AnsiStringMarshaller ) )] string triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMTripleOpEqual(LibLLVMTripleRef lhs, LibLLVMTripleRef rhs);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleArchType LibLLVMTripleGetArchType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleSubArchType LibLLVMTripleGetSubArchType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleVendorType LibLLVMTripleGetVendorType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleOSType LibLLVMTripleGetOsType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LibLLVMTripleHasEnvironment(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleEnvironmentType LibLLVMTripleGetEnvironmentType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LibLLVMTripleGetEnvironmentVersion(LibLLVMTripleRef triple, out uint major, out uint minor, out uint build);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleObjectFormatType LibLLVMTripleGetObjectFormatType(LibLLVMTripleRef triple);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleAsString(LibLLVMTripleRef triple, [MarshalAs( UnmanagedType.Bool )] bool normalize);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetArchTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleArchType type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetSubArchTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleSubArchType type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetVendorTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleVendorType vendor);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetOsTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleOSType osType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetEnvironmentTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleEnvironmentType environmentType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( DisposeMessageMarshaller ) )]
        public static unsafe partial string LibLLVMTripleGetObjectFormatTypeName(global::Ubiquity.NET.Llvm.Interop.LibLLVMTripleObjectFormatType objectFormatType);
    }
}
