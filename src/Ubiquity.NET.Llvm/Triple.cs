// -----------------------------------------------------------------------
// <copyright file="Triple.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.ArgValidators;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Triple to describe a target</summary>
    /// <remarks>
    /// <para>The term 'Triple' is a bit of a misnomer. At some point in the past it
    /// actually consisted of only three parts, but that has changed over the years
    /// without the name itself changing. The triple is normally represented as a
    /// string of 4 components delimited by '-'. Some of the components have
    /// sub components as part of the content. The canonical form of a triple is:
    /// <c>{Architecture}{SubArchitecture}-{Vendor}-{OS}-{Environment}{ObjectFormat}</c></para>
    /// <para>
    /// A few shorthand variations are allowed and converted to their full normalized form.
    /// In particular "cygwin" is a shorthand for the OS-Environment tuple "windows-cygnus"
    /// and "mingw" is a shorthand form of "windows-gnu".
    /// </para>
    /// <para>In addition to shorthand allowances, the OS component may optionally include
    /// a trailing version of the form Maj.Min.Micro. If any of the version number parts are
    /// not present, then they default to 0.</para>
    /// <para>
    /// For the environment "androideabi" is allowed and normalized to android (including
    /// an optional version number).
    /// </para>
    /// </remarks>
    public sealed class Triple
        : IEquatable<Triple>
    {
        /// <summary>Enumeration for the Architecture portion of a target triple</summary>
        [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Harder to read without them" )]
        public enum ArchType
        {
            /// <summary>Invalid or unknown architecture</summary>
            UnknownArch = LibLLVMTripleArchType.LibLLVMTripleArchType_UnknownArch,

            /// <summary>ARM (little endian): arm, armv.*, xscale</summary>
            Arm = LibLLVMTripleArchType.LibLLVMTripleArchType_arm,

            /// <summary>ARM (big endian): armeb</summary>
            Armeb = LibLLVMTripleArchType.LibLLVMTripleArchType_armeb,

            /// <summary>AArch64 (little endian): aarch64</summary>
            Aarch64 = LibLLVMTripleArchType.LibLLVMTripleArchType_aarch64,

            /// <summary>AArch64 (big endian): aarch64_be</summary>
            Aarch64BE = LibLLVMTripleArchType.LibLLVMTripleArchType_aarch64_be,

            /// <summary>AArch64 32 bit (Little endian) ILP32: aarch64_32</summary>
            Aarch64_32 = LibLLVMTripleArchType.LibLLVMTripleArchType_aarch64_32,

            /// <summary>Synopsis ARC</summary>
            Arc = LibLLVMTripleArchType.LibLLVMTripleArchType_arc,

            /// <summary>AVR: Atmel AVR micro-controller</summary>
            Avr = LibLLVMTripleArchType.LibLLVMTripleArchType_avr,

            /// <summary>eBPF or extended BPF or 64-bit BPF (little endian)</summary>
            BPFel = LibLLVMTripleArchType.LibLLVMTripleArchType_bpfel,

            /// <summary>eBPF or extended BPF or 64-bit BPF (big endian)</summary>
            BPFeb = LibLLVMTripleArchType.LibLLVMTripleArchType_bpfeb,

            /// <summary>Hexagon processor</summary>
            Hexagon = LibLLVMTripleArchType.LibLLVMTripleArchType_hexagon,

            /// <summary>MIPS: mips, mipsallegrex</summary>
            MIPS = LibLLVMTripleArchType.LibLLVMTripleArchType_mips,

            /// <summary>MIPSEL: mipsel, mipsallegrexel</summary>
            MIPSel = LibLLVMTripleArchType.LibLLVMTripleArchType_mipsel,

            /// <summary>MIPS 64 bit</summary>
            MIPS64 = LibLLVMTripleArchType.LibLLVMTripleArchType_mips64,

            /// <summary>MIPS 64-bit little endian</summary>
            MIPS64el = LibLLVMTripleArchType.LibLLVMTripleArchType_mips64el,

            /// <summary>MSP430</summary>
            MSP430 = LibLLVMTripleArchType.LibLLVMTripleArchType_msp430,

            /// <summary>PowerPC</summary>
            PPC = LibLLVMTripleArchType.LibLLVMTripleArchType_ppc,

            /// <summary>PowerPC 64-bit</summary>
            PPC64 = LibLLVMTripleArchType.LibLLVMTripleArchType_ppc64,

            /// <summary>PowerPC 64-bit little endian</summary>
            PPC64le = LibLLVMTripleArchType.LibLLVMTripleArchType_ppc64le,

            /// <summary>R600 AMD GPUS HD2XXX-HD6XXX</summary>
            R600 = LibLLVMTripleArchType.LibLLVMTripleArchType_r600,

            /// <summary>AMD GCN GPUs</summary>
            AMDGCN = LibLLVMTripleArchType.LibLLVMTripleArchType_amdgcn,

            /// <summary>RISC-V (32-bit)</summary>
            RiscV32 = LibLLVMTripleArchType.LibLLVMTripleArchType_riscv32,

            /// <summary>RISC-V (64-bit)</summary>
            RiscV64 = LibLLVMTripleArchType.LibLLVMTripleArchType_riscv64,

            /// <summary>Sparc</summary>
            Sparc = LibLLVMTripleArchType.LibLLVMTripleArchType_sparc,

            /// <summary>SPARC V9</summary>
            Sparcv9 = LibLLVMTripleArchType.LibLLVMTripleArchType_sparcv9,

            /// <summary>SPARC Little-Endian</summary>
            Sparcel = LibLLVMTripleArchType.LibLLVMTripleArchType_sparcel,

            /// <summary>SystemZ - s390x</summary>
            SystemZ = LibLLVMTripleArchType.LibLLVMTripleArchType_systemz,

            /// <summary>TCE</summary>
            /// <seealso href="http://tce.cs.tut.fi"/>
            TCE = LibLLVMTripleArchType.LibLLVMTripleArchType_tce,

            /// <summary>TCE Little-Endian</summary>
            /// <seealso href="http://tce.cs.tut.fi"/>
            TCEle = LibLLVMTripleArchType.LibLLVMTripleArchType_tcele,

            /// <summary>Thumb (little-endian)</summary>
            Thumb = LibLLVMTripleArchType.LibLLVMTripleArchType_thumb,

            /// <summary>Thumb (big-endian)</summary>
            Thumbeb = LibLLVMTripleArchType.LibLLVMTripleArchType_thumbeb,

            /// <summary>x86 i[3-9]86</summary>
            X86 = LibLLVMTripleArchType.LibLLVMTripleArchType_x86,

            /// <summary>X86 64-bit (amd64)</summary>
            Amd64 = LibLLVMTripleArchType.LibLLVMTripleArchType_x86_64,

            /// <summary>XCore</summary>
            Xcore = LibLLVMTripleArchType.LibLLVMTripleArchType_xcore,

            /// <summary>NVidia PTX 32-bit</summary>
            Nvptx = LibLLVMTripleArchType.LibLLVMTripleArchType_nvptx,

            /// <summary>NVidia PTX 64-bit</summary>
            Nvptx64 = LibLLVMTripleArchType.LibLLVMTripleArchType_nvptx64,

            /// <summary>Generic little-endian 32-bit CPU (PNaCl)</summary>
            Le32 = LibLLVMTripleArchType.LibLLVMTripleArchType_le32,

            /// <summary>Generic little-endian 64-bit CPU (PNaCl)</summary>
            Le64 = LibLLVMTripleArchType.LibLLVMTripleArchType_le64,

            /// <summary>AMD IL</summary>
            Amdil = LibLLVMTripleArchType.LibLLVMTripleArchType_amdil,

            /// <summary>AMD IL 64-bit pointers</summary>
            Amdil64 = LibLLVMTripleArchType.LibLLVMTripleArchType_amdil64,

            /// <summary>AMD HSAIL</summary>
            Hsail = LibLLVMTripleArchType.LibLLVMTripleArchType_hsail,

            /// <summary>AMD HSAIL with 64-bit pointers</summary>
            Hsail64 = LibLLVMTripleArchType.LibLLVMTripleArchType_hsail64,

            /// <summary>Standard Portable IR for OpenCL 32-bit version</summary>
            Spir = LibLLVMTripleArchType.LibLLVMTripleArchType_spir,

            /// <summary>Standard Portable IR for OpenCL 64-bit version</summary>
            Spir64 = LibLLVMTripleArchType.LibLLVMTripleArchType_spir64,

            /// <summary>Generic Kalimba</summary>
            Kalimba = LibLLVMTripleArchType.LibLLVMTripleArchType_kalimba,

            /// <summary>Movidius vector VLIW processors</summary>
            Shave = LibLLVMTripleArchType.LibLLVMTripleArchType_shave,

            /// <summary>Lanai 32-bit</summary>
            Lanai = LibLLVMTripleArchType.LibLLVMTripleArchType_lanai,

            /// <summary>WebAssembly with 32-bit pointers</summary>
            Wasm32 = LibLLVMTripleArchType.LibLLVMTripleArchType_wasm32,

            /// <summary>WebAssembly with 64-bit pointers</summary>
            Wasm64 = LibLLVMTripleArchType.LibLLVMTripleArchType_wasm64,

            /// <summary>Renderscript 32-bit</summary>
            Renderscript32 = LibLLVMTripleArchType.LibLLVMTripleArchType_renderscript32,

            /// <summary>Renderscript 64-bit</summary>
            Renderscript64 = LibLLVMTripleArchType.LibLLVMTripleArchType_renderscript64,

            /// <summary>NEC SX Aurora Vector Engine</summary>
            Ve = LibLLVMTripleArchType.LibLLVMTripleArchType_ve,
        }

        /// <summary>Processor sub architecture type</summary>
        [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Harder to understand without them" )]
        public enum SubArchType
        {
            /// <summary>No sub architecture</summary>
            NoSubArch = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_NoSubArch,

            /// <summary>ARM v8.5a</summary>
            ARMSubArch_v8_5a = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_5a,

            /// <summary>ARM v8.4a</summary>
            ARMSubArch_v8_4a = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_4a,

            /// <summary>ARM v8.3a</summary>
            ARMSubArch_v8_3a = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_3a,

            /// <summary>ARM v8.2a</summary>
            ARMSubArch_v8_2a = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_2a,

            /// <summary>ARM v8.1a</summary>
            ARMSubArch_v8_1a = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_1a,

            /// <summary>ARM v8</summary>
            ARMSubArch_v8 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8,

            /// <summary>ARM v8r</summary>
            ARMSubArch_v8r = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8r,

            /// <summary>ARM v8m baseline</summary>
            ARMSubArch_v8m_baseline = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8m_baseline,

            /// <summary>ARM v8m mainline</summary>
            ARMSubArch_v8m_mainline = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8m_mainline,

            /// <summary>ARM v8 1m mainline</summary>
            ARMSubArch_v8_1m_mainline = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v8_1m_mainline,

            /// <summary>ARM v7</summary>
            ARMSubArch_v7 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7,

            /// <summary>ARM v7em</summary>
            ARMSubArch_v7em = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7em,

            /// <summary>ARM v7m</summary>
            ARMSubArch_v7m = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7m,

            /// <summary>ARM v7s</summary>
            ARMSubArch_v7s = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7s,

            /// <summary>ARM v7k</summary>
            ARMSubArch_v7k = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7k,

            /// <summary>ARM v7ve</summary>
            ARMSubArch_v7ve = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v7ve,

            /// <summary>ARM v6</summary>
            ARMSubArch_v6 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v6,

            /// <summary>ARM v6m</summary>
            ARMSubArch_v6m = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v6m,

            /// <summary>ARM v6k</summary>
            ARMSubArch_v6k = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v6k,

            /// <summary>ARM v6t2</summary>
            ARMSubArch_v6t2 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v6t2,

            /// <summary>ARM v5</summary>
            ARMSubArch_v5 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v5,

            /// <summary>ARM v5te</summary>
            ARMSubArch_v5te = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v5te,

            /// <summary>ARM v4t</summary>
            ARMSubArch_v4t = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_ARMSubArch_v4t,

            /// <summary>Kalimba v3</summary>
            KalimbaSubArch_v3 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_KalimbaSubArch_v3,

            /// <summary>Kalimba v4</summary>
            KalimbaSubArch_v4 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_KalimbaSubArch_v4,

            /// <summary>Kalimba v5</summary>
            KalimbaSubArch_v5 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_KalimbaSubArch_v5,

            /// <summary>MIPS R6</summary>
            MipsSubArch_r6 = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_MipsSubArch_r6,

            /// <summary>PowerPC SPE</summary>
            PowerPC_SE = LibLLVMTripleSubArchType.LibLLVMTripleSubArchType_PPCSubArch_spe,
        }

        /// <summary>Vendor type for the triple</summary>
        public enum VendorType
        {
            /// <summary>Unknown vendor</summary>
            UnknownVendor = LibLLVMTripleVendorType.LibLLVMTripleVendorType_UnknownVendor,

            /// <summary>Apple</summary>
            Apple = LibLLVMTripleVendorType.LibLLVMTripleVendorType_Apple,

            /// <summary>Generic PC</summary>
            PC = LibLLVMTripleVendorType.LibLLVMTripleVendorType_PC,

            /// <summary>SCEI</summary>
            SCEI = LibLLVMTripleVendorType.LibLLVMTripleVendorType_SCEI,

            /// <summary>BGP</summary>
            BGP = LibLLVMTripleVendorType.LibLLVMTripleVendorType_BGP,

            /// <summary>BGQ</summary>
            BGQ = LibLLVMTripleVendorType.LibLLVMTripleVendorType_BGQ,

            /// <summary>Freescale</summary>
            Freescale = LibLLVMTripleVendorType.LibLLVMTripleVendorType_Freescale,

            /// <summary>IBM</summary>
            IBM = LibLLVMTripleVendorType.LibLLVMTripleVendorType_IBM,

            /// <summary>Imagination Technologies</summary>
            ImaginationTechnologies = LibLLVMTripleVendorType.LibLLVMTripleVendorType_ImaginationTechnologies,

            /// <summary>MIPS Technologies</summary>
            MipsTechnologies = LibLLVMTripleVendorType.LibLLVMTripleVendorType_MipsTechnologies,

            /// <summary>NVidia</summary>
            NVIDIA = LibLLVMTripleVendorType.LibLLVMTripleVendorType_NVIDIA,

            /// <summary>CSR</summary>
            CSR = LibLLVMTripleVendorType.LibLLVMTripleVendorType_CSR,

            /// <summary>Myriad</summary>
            Myriad = LibLLVMTripleVendorType.LibLLVMTripleVendorType_Myriad,

            /// <summary>AMD</summary>
            AMD = LibLLVMTripleVendorType.LibLLVMTripleVendorType_AMD,

            /// <summary>Mesa</summary>
            Mesa = LibLLVMTripleVendorType.LibLLVMTripleVendorType_Mesa,

            /// <summary>SUSE</summary>
            SUSE = LibLLVMTripleVendorType.LibLLVMTripleVendorType_SUSE,

            /// <summary>OpenEmbedded</summary>
            OpenEmebedded = LibLLVMTripleVendorType.LibLLVMTripleVendorType_OpenEmbedded,
        }

        /// <summary>OS type for the triple</summary>
        public enum OSType
        {
            /// <summary>Unknown OS</summary>
            UnknownOS = LibLLVMTripleOSType.LibLLVMTripleOSType_UnknownOS,

            /// <summary>Ananas</summary>
            Ananas = LibLLVMTripleOSType.LibLLVMTripleOSType_Ananas,

            /// <summary>CloudABI</summary>
            CloudABI = LibLLVMTripleOSType.LibLLVMTripleOSType_CloudABI,

            /// <summary>Darwin</summary>
            Darwin = LibLLVMTripleOSType.LibLLVMTripleOSType_Darwin,

            /// <summary>DragonFly</summary>
            DragonFly = LibLLVMTripleOSType.LibLLVMTripleOSType_DragonFly,

            /// <summary>FreeBSD</summary>
            FreeBSD = LibLLVMTripleOSType.LibLLVMTripleOSType_FreeBSD,

            /// <summary>Fuchsia</summary>
            Fuchsia = LibLLVMTripleOSType.LibLLVMTripleOSType_Fuchsia,

            /// <summary>iOS</summary>
            IOS = LibLLVMTripleOSType.LibLLVMTripleOSType_IOS,

            /// <summary>KFreeBSD</summary>
            KFreeBSD = LibLLVMTripleOSType.LibLLVMTripleOSType_KFreeBSD,

            /// <summary>Linux</summary>
            Linux = LibLLVMTripleOSType.LibLLVMTripleOSType_Linux,

            /// <summary>Lv2</summary>
            Lv2 = LibLLVMTripleOSType.LibLLVMTripleOSType_Lv2,

            /// <summary>Mac OSX</summary>
            MacOSX = LibLLVMTripleOSType.LibLLVMTripleOSType_MacOSX,

            /// <summary>NetBSD</summary>
            NetBSD = LibLLVMTripleOSType.LibLLVMTripleOSType_NetBSD,

            /// <summary>OpenBSD</summary>
            OpenBSD = LibLLVMTripleOSType.LibLLVMTripleOSType_OpenBSD,

            /// <summary>Solaris</summary>
            Solaris = LibLLVMTripleOSType.LibLLVMTripleOSType_Solaris,

            /// <summary>Windows WIN32</summary>
            Win32 = LibLLVMTripleOSType.LibLLVMTripleOSType_Win32,

            /// <summary>Haiku</summary>
            Haiku = LibLLVMTripleOSType.LibLLVMTripleOSType_Haiku,

            /// <summary>Minix</summary>
            Minix = LibLLVMTripleOSType.LibLLVMTripleOSType_Minix,

            /// <summary>RTEMS</summary>
            RTEMS = LibLLVMTripleOSType.LibLLVMTripleOSType_RTEMS,

            /// <summary>NaCl</summary>
            NaCl = LibLLVMTripleOSType.LibLLVMTripleOSType_NaCl,

            /// <summary>CNK</summary>
            CNK = LibLLVMTripleOSType.LibLLVMTripleOSType_CNK,

            /// <summary>AIX</summary>
            AIX = LibLLVMTripleOSType.LibLLVMTripleOSType_AIX,

            /// <summary>CUDA</summary>
            CUDA = LibLLVMTripleOSType.LibLLVMTripleOSType_CUDA,

            /// <summary>NVCL</summary>
            NVCL = LibLLVMTripleOSType.LibLLVMTripleOSType_NVCL,

            /// <summary>AMD HSA</summary>
            AMDHSA = LibLLVMTripleOSType.LibLLVMTripleOSType_AMDHSA,

            /// <summary>PS4</summary>
            PS4 = LibLLVMTripleOSType.LibLLVMTripleOSType_PS4,

            /// <summary>ELFIAMCU</summary>
            ELFIAMCU = LibLLVMTripleOSType.LibLLVMTripleOSType_ELFIAMCU,

            /// <summary>TvOS</summary>
            TvOS = LibLLVMTripleOSType.LibLLVMTripleOSType_TvOS,

            /// <summary>WatchOS</summary>
            WatchOS = LibLLVMTripleOSType.LibLLVMTripleOSType_WatchOS,

            /// <summary>Mesa3D</summary>
            Mesa3D = LibLLVMTripleOSType.LibLLVMTripleOSType_Mesa3D,

            /// <summary>Contiki</summary>
            Contiki = LibLLVMTripleOSType.LibLLVMTripleOSType_Contiki,

            /// <summary>AMD PAL Runtime</summary>
            AmdPAL = LibLLVMTripleOSType.LibLLVMTripleOSType_AMDPAL,

            /// <summary>HermitCore Unikernel/Multikernel</summary>
            HermitCore = LibLLVMTripleOSType.LibLLVMTripleOSType_HermitCore,

            /// <summary>GNU/Hurd</summary>
            Hurd = LibLLVMTripleOSType.LibLLVMTripleOSType_Hurd,

            /// <summary>WebAssembly OS</summary>
            WASI = LibLLVMTripleOSType.LibLLVMTripleOSType_WASI,

            /// <summary>Emscripten</summary>
            Emscripten = LibLLVMTripleOSType.LibLLVMTripleOSType_Emscripten,
        }

        /// <summary>Triple Environment type</summary>
        public enum EnvironmentType
        {
            /// <summary>Unknown environment</summary>
            UnknownEnvironment = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_UnknownEnvironment,

            /// <summary>GNU</summary>
            GNU = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNU,

            /// <summary>GNUABIN32</summary>
            GNUABIN32 = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNUABIN32,

            /// <summary>GNU ABI 64-bit</summary>
            GNUABI64 = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNUABI64,

            /// <summary>GNU EABI</summary>
            GNUEABI = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNUEABI,

            /// <summary>GNU EABI-HF</summary>
            GNUEABIHF = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNUEABIHF,

            /// <summary>GNU X32</summary>
            GNUX32 = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_GNUX32,

            /// <summary>CODE16</summary>
            CODE16 = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_CODE16,

            /// <summary>EABI</summary>
            EABI = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_EABI,

            /// <summary>EABI-HF</summary>
            EABIHF = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_EABIHF,

            /// <summary>Android</summary>
            Android = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_Android,

            /// <summary>MUSL</summary>
            Musl = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_Musl,

            /// <summary>MUSL EABI</summary>
            MuslEABI = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_MuslEABI,

            /// <summary>MUSL EABI-HF</summary>
            MuslEABIHF = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_MuslEABIHF,

            /// <summary>Microsoft Visual C</summary>
            MSVC = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_MSVC,

            /// <summary>Itanium</summary>
            Itanium = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_Itanium,

            /// <summary>Cygnus</summary>
            Cygnus = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_Cygnus,

            /// <summary>CoreCLR</summary>
            CoreCLR = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_CoreCLR,

            /// <summary>Simulator</summary>
            Simultator = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_Simulator,

            /// <summary>Mac Catalyst variant of Apple's iOS deployment target</summary>
            MacABI = LibLLVMTripleEnvironmentType.LibLLVMTripleEnvironmentType_MacABI,
        }

        /// <summary>Object format type for a Triple</summary>
        public enum ObjectFormatType
        {
            /// <summary>Unknown format</summary>
            UnknownObjectFormat = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_UnknownObjectFormat,

            /// <summary>COFF format</summary>
            COFF = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_COFF,

            /// <summary>ELF format</summary>
            ELF = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_ELF,

            /// <summary>MachO format</summary>
            MachO = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_MachO,

            /// <summary>Wasm format</summary>
            Wasm = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_Wasm,

            /// <summary>SCOFF format</summary>
            XCOFF = LibLLVMTripleObjectFormatType.LibLLVMTripleObjectFormatType_XCOFF,
        }

        /// <summary>Initializes a new instance of the <see cref="Triple"/> class from a triple string</summary>
        /// <param name="tripleTxt">Triple string to parse</param>
        /// <remarks>
        /// The <paramref name="tripleTxt"/> string is normalized before parsing to allow for
        /// common non-canonical forms of triples.
        /// </remarks>
        public Triple( string tripleTxt )
            : this( LibLLVMParseTriple( tripleTxt ) )
        {
        }

        /// <summary>Retrieves the final string form of the triple</summary>
        /// <returns>Normalized Triple string</returns>
        public override string ToString( ) => LibLLVMTripleAsString( TripleHandle, true );

        /// <summary>Gets the Architecture of the triple</summary>
        public ArchType ArchitectureType => ( ArchType )LibLLVMTripleGetArchType( TripleHandle );

        /// <summary>Gets the Sub Architecture type</summary>
        public SubArchType SubArchitecture => ( SubArchType )LibLLVMTripleGetSubArchType( TripleHandle );

        /// <summary>Gets the Vendor component of the triple</summary>
        public VendorType Vendor => ( VendorType )LibLLVMTripleGetVendorType( TripleHandle );

        /// <summary>Gets the OS Type for the triple</summary>
        public OSType OS => ( OSType )LibLLVMTripleGetOsType( TripleHandle );

        /// <summary>Gets the environment type for the triple</summary>
        public EnvironmentType Environment => ( EnvironmentType )LibLLVMTripleGetEnvironmentType( TripleHandle );

        /// <summary>Gets the object format type for the triple</summary>
        public ObjectFormatType ObjectFormat => ( ObjectFormatType )LibLLVMTripleGetObjectFormatType( TripleHandle );

        /// <summary>Gets the version number of the environment</summary>
        public Version EnvironmentVersion
        {
            get
            {
                LibLLVMTripleGetEnvironmentVersion( TripleHandle, out uint major, out uint minor, out uint micro );
                checked
                {
                    return new Version( ( int )major, ( int )minor, ( int )micro );
                }
            }
        }

        /// <summary>Retrieves the canonical name for an architecture type</summary>
        /// <param name="archType">Architecture type</param>
        /// <returns>String name for the architecture</returns>
        /// <overloads>
        /// Many parts of a triple can take a variety of literal string
        /// forms to allow for common real world triples when parsing.
        /// The GetCanonicalName methods provide the canonical form of
        /// such triple components used in a normalized triple.
        /// </overloads>
        public static string GetCanonicalName( ArchType archType )
            => LibLLVMTripleGetArchTypeName( ( LibLLVMTripleArchType )archType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for an architecture sub type</summary>
        /// <param name="subArchType">Architecture sub type</param>
        /// <returns>String name for the architecture sub type</returns>
        public static string GetCanonicalName( SubArchType subArchType )
            => LibLLVMTripleGetSubArchTypeName( ( LibLLVMTripleSubArchType )subArchType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the vendor component of a triple</summary>
        /// <param name="vendorType">Vendor type</param>
        /// <returns>String name for the vendor</returns>
        public static string GetCanonicalName( VendorType vendorType )
            => LibLLVMTripleGetVendorTypeName( ( LibLLVMTripleVendorType )vendorType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the OS component of a triple</summary>
        /// <param name="osType">OS type</param>
        /// <returns>String name for the OS</returns>
        public static string GetCanonicalName( OSType osType )
            => LibLLVMTripleGetOsTypeName( ( LibLLVMTripleOSType )osType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the environment component of a triple</summary>
        /// <param name="envType">Environment type</param>
        /// <returns>String name for the environment component</returns>
        public static string GetCanonicalName( EnvironmentType envType )
            => LibLLVMTripleGetEnvironmentTypeName( ( LibLLVMTripleEnvironmentType )envType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the object component of a triple</summary>
        /// <param name="objFormatType">Object type</param>
        /// <returns>String name for the object component</returns>
        public static string GetCanonicalName( ObjectFormatType objFormatType )
            => LibLLVMTripleGetObjectFormatTypeName( ( LibLLVMTripleObjectFormatType )objFormatType ) ?? string.Empty;

        /// <summary>Equality test for a triple</summary>
        /// <param name="other">triple to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public bool Equals( Triple? other )
        {
            return other != null && ( ReferenceEquals( this, other ) || LibLLVMTripleOpEqual( TripleHandle, other.TripleHandle ) );
        }

        /// <summary>Equality test for a triple</summary>
        /// <param name="obj">object to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public override bool Equals( object? obj )
        {
            return Equals( obj as Triple );
        }

        /// <summary>Gets a hash code for this <see cref="Triple"/></summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode( )
        {
            return ToString( ).GetHashCode( StringComparison.Ordinal );
        }

        /// <summary>Normalizes a triple string</summary>
        /// <param name="unNormalizedTriple">triple to normalize</param>
        /// <returns>Normalized string</returns>
        public static string Normalize( string unNormalizedTriple )
        {
            unNormalizedTriple.ValidateNotNullOrWhiteSpace( nameof( unNormalizedTriple ) );

            return LLVMNormalizeTargetTriple( unNormalizedTriple );
        }

        /// <summary>Gets the default <see cref="ObjectFormatType"/> for a given <see cref="ArchType"/> and <see cref="OSType"/></summary>
        /// <param name="arch">Architecture type</param>
        /// <param name="os">Operating system type</param>
        /// <returns>Default object format</returns>
        [SuppressMessage( "Maintainability", "CA1502:Avoid excessive complexity", Justification = "Type factory from native typekind" )]
        public static ObjectFormatType GetDefaultObjectFormat( ArchType arch, OSType os )
        {
            arch.ValidateDefined( nameof( arch ) );
            os.ValidateDefined( nameof( os ) );

            switch( arch )
            {
            case ArchType.UnknownArch:
            case ArchType.Aarch64:
            case ArchType.Arm:
            case ArchType.Thumb:
            case ArchType.X86:
            case ArchType.Amd64:
                if( IsOsDarwin( os ) )
                {
                    return ObjectFormatType.MachO;
                }

                if( os == OSType.Win32 )
                {
                    return ObjectFormatType.COFF;
                }

                return ObjectFormatType.ELF;

            case ArchType.Aarch64BE:
            case ArchType.AMDGCN:
            case ArchType.Amdil:
            case ArchType.Amdil64:
            case ArchType.Armeb:
            case ArchType.Avr:
            case ArchType.BPFeb:
            case ArchType.BPFel:
            case ArchType.Hexagon:
            case ArchType.Lanai:
            case ArchType.Hsail:
            case ArchType.Hsail64:
            case ArchType.Kalimba:
            case ArchType.Le32:
            case ArchType.Le64:
            case ArchType.MIPS:
            case ArchType.MIPS64:
            case ArchType.MIPS64el:
            case ArchType.MIPSel:
            case ArchType.MSP430:
            case ArchType.Nvptx:
            case ArchType.Nvptx64:
            case ArchType.PPC64le:
            case ArchType.R600:
            case ArchType.Renderscript32:
            case ArchType.Renderscript64:
            case ArchType.Shave:
            case ArchType.Sparc:
            case ArchType.Sparcel:
            case ArchType.Sparcv9:
            case ArchType.Spir:
            case ArchType.Spir64:
            case ArchType.SystemZ:
            case ArchType.TCE:
            case ArchType.Thumbeb:
            case ArchType.Wasm32:
            case ArchType.Wasm64:
            case ArchType.Xcore:
                return ObjectFormatType.ELF;

            case ArchType.PPC:
            case ArchType.PPC64:
                if( IsOsDarwin( os ) )
                {
                    return ObjectFormatType.MachO;
                }

                return ObjectFormatType.ELF;

            default:
                throw new ArgumentException( Resources.Unsupported_Architecture, nameof( arch ) );
            }
        }

        /// <summary>Provides the canonical Architecture form for a given architecture sub architecture pair</summary>
        /// <param name="archType">Architecture type</param>
        /// <param name="subArch">Sub Architecture type</param>
        /// <returns>Canonical <see cref="ArchType"/></returns>
        /// <remarks>
        /// Some architectures, particularly ARM variants, have multiple sub-architecture types that
        /// have a canonical form (i.e. Arch=<see cref="ArchType.Arm"/>; SubArch=<see cref="SubArchType.ARMSubArch_v7m"/>;
        /// has the Canonical Arch of <see cref="ArchType.Thumb"/>). This method retrieves the canonical Arch
        /// for a given architecture,SubArchitecture pair.
        /// </remarks>
        public static ArchType GetCanonicalArchForSubArch( ArchType archType, SubArchType subArch )
        {
            archType.ValidateDefined( nameof( archType ) );
            subArch.ValidateDefined( nameof( subArch ) );
            return archType switch
            {
                ArchType.Kalimba => subArch switch
                {
                    SubArchType.NoSubArch or SubArchType.KalimbaSubArch_v3 or SubArchType.KalimbaSubArch_v4 or SubArchType.KalimbaSubArch_v5 => ArchType.Kalimba,
                    _ => ArchType.UnknownArch,
                },
                ArchType.Arm or ArchType.Armeb => subArch switch
                {
                    SubArchType.ARMSubArch_v6m => archType == ArchType.Armeb ? ArchType.Thumbeb : ArchType.Thumb,
                    SubArchType.KalimbaSubArch_v3 or SubArchType.KalimbaSubArch_v4 or SubArchType.KalimbaSubArch_v5 => ArchType.UnknownArch,
                    _ => archType,
                },
                _ => archType,
            };
        }

        /// <summary>Gets a triple for the host LLVM is built for</summary>
        public static Triple HostTriple => new( LLVMGetDefaultTargetTriple( ) );

        /// <summary>Implicitly converts a triple to a string</summary>
        /// <param name="triple"><see cref="Triple"/> to convert</param>
        public static implicit operator string( Triple triple ) => triple.ValidateNotNull( nameof( triple ) ).ToString( );

        private Triple( LibLLVMTripleRef handle )
        {
            TripleHandle = handle;
        }

        private static bool IsOsDarwin( OSType osType )
        {
            osType.ValidateDefined( nameof( osType ) );
            return osType switch
            {
                OSType.Darwin or OSType.MacOSX or OSType.IOS or OSType.TvOS or OSType.WatchOS => true,
                _ => false,
            };
        }

        private readonly LibLLVMTripleRef TripleHandle;
    }
}
