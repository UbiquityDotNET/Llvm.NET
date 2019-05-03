// <copyright file="Triple.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.Interop;
using Llvm.NET.Properties;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
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
        [SuppressMessage( "Microsoft.Design", "CA1027:MarkEnumsWithFlags", Justification = "Not actually flags" )]
        [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Harder to understand without them" )]
        public enum ArchType
        {
            /// <summary>Invalid or unknown architecture</summary>
            UnknownArch = LLVMTripleArchType.LlvmTripleArchType_UnknownArch,

            /// <summary>ARM (little endian): arm, armv.*, xscale</summary>
            Arm = LLVMTripleArchType.LlvmTripleArchType_arm,

            /// <summary>ARM (big endian): armeb</summary>
            Armeb = LLVMTripleArchType.LlvmTripleArchType_armeb,

            /// <summary>AArch64 (little endian): aarch64</summary>
            Aarch64 = LLVMTripleArchType.LlvmTripleArchType_aarch64,

            /// <summary>AArch64 (big endian): aarch64_be</summary>
            Aarch64_be = LLVMTripleArchType.LlvmTripleArchType_aarch64_be,

            /// <summary>Synopsis ARC</summary>
            Arc = LLVMTripleArchType.LlvmTripleArchType_arc,

            /// <summary>AVR: Atmel AVR micro-controller</summary>
            Avr = LLVMTripleArchType.LlvmTripleArchType_avr,

            /// <summary>eBPF or extended BPF or 64-bit BPF (little endian)</summary>
            BPFel = LLVMTripleArchType.LlvmTripleArchType_bpfel,

            /// <summary>eBPF or extended BPF or 64-bit BPF (big endian)</summary>
            BPFeb = LLVMTripleArchType.LlvmTripleArchType_bpfeb,

            /// <summary>Hexagon processor</summary>
            Hexagon = LLVMTripleArchType.LlvmTripleArchType_hexagon,

            /// <summary>MIPS: mips, mipsallegrex</summary>
            MIPS = LLVMTripleArchType.LlvmTripleArchType_mips,

            /// <summary>MIPSEL: mipsel, mipsallegrexel</summary>
            MIPSel = LLVMTripleArchType.LlvmTripleArchType_mipsel,

            /// <summary>MIPS 64 bit</summary>
            MIPS64 = LLVMTripleArchType.LlvmTripleArchType_mips64,

            /// <summary>MIPS 64-bit little endian</summary>
            MIPS64el = LLVMTripleArchType.LlvmTripleArchType_mips64el,

            /// <summary>MSP430</summary>
            MSP430 = LLVMTripleArchType.LlvmTripleArchType_msp430,

            /// <summary>PowerPC</summary>
            PPC = LLVMTripleArchType.LlvmTripleArchType_ppc,

            /// <summary>PowerPC 64-bit</summary>
            PPC64 = LLVMTripleArchType.LlvmTripleArchType_ppc64,

            /// <summary>PowerPC 64-bit little endian</summary>
            PPC64le = LLVMTripleArchType.LlvmTripleArchType_ppc64le,

            /// <summary>R600 AMD GPUS HD2XXX-HD6XXX</summary>
            R600 = LLVMTripleArchType.LlvmTripleArchType_r600,

            /// <summary>AMD GCN GPUs</summary>
            AMDGCN = LLVMTripleArchType.LlvmTripleArchType_amdgcn,

            /// <summary>RISC-V (32-bit)</summary>
            RiscV32 = LLVMTripleArchType.LlvmTripleArchType_riscv32,

            /// <summary>RISC-V (64-bit)</summary>
            RiscV64 = LLVMTripleArchType.LlvmTripleArchType_riscv64,

            /// <summary>Sparc</summary>
            Sparc = LLVMTripleArchType.LlvmTripleArchType_sparc,

            /// <summary>SPARC V9</summary>
            Sparcv9 = LLVMTripleArchType.LlvmTripleArchType_sparcv9,

            /// <summary>SPARC Little-Endian</summary>
            Sparcel = LLVMTripleArchType.LlvmTripleArchType_sparcel,

            /// <summary>SystemZ - s390x</summary>
            SystemZ = LLVMTripleArchType.LlvmTripleArchType_systemz,

            /// <summary>TCE</summary>
            /// <seealso href="http://tce.cs.tut.fi"/>
            TCE = LLVMTripleArchType.LlvmTripleArchType_tce,

            /// <summary>TCE Little-Endian</summary>
            /// <seealso href="http://tce.cs.tut.fi"/>
            TCEle = LLVMTripleArchType.LlvmTripleArchType_tcele,

            /// <summary>Thumb (little-endian)</summary>
            Thumb = LLVMTripleArchType.LlvmTripleArchType_thumb,

            /// <summary>Thumb (big-endian)</summary>
            Thumbeb = LLVMTripleArchType.LlvmTripleArchType_thumbeb,

            /// <summary>x86 i[3-9]86</summary>
            X86 = LLVMTripleArchType.LlvmTripleArchType_x86,

            /// <summary>X86 64-bit (amd64)</summary>
            X86_64 = LLVMTripleArchType.LlvmTripleArchType_x86_64,

            /// <summary>XCore</summary>
            Xcore = LLVMTripleArchType.LlvmTripleArchType_xcore,

            /// <summary>NVidia PTX 32-bit</summary>
            Nvptx = LLVMTripleArchType.LlvmTripleArchType_nvptx,

            /// <summary>NVidia PTX 64-bit</summary>
            Nvptx64 = LLVMTripleArchType.LlvmTripleArchType_nvptx64,

            /// <summary>Generic little-endian 32-bit CPU (PNaCl)</summary>
            Le32 = LLVMTripleArchType.LlvmTripleArchType_le32,

            /// <summary>Generic little-endian 64-bit CPU (PNaCl)</summary>
            Le64 = LLVMTripleArchType.LlvmTripleArchType_le64,

            /// <summary>AMD IL</summary>
            Amdil = LLVMTripleArchType.LlvmTripleArchType_amdil,

            /// <summary>AMD IL 64-bit pointers</summary>
            Amdil64 = LLVMTripleArchType.LlvmTripleArchType_amdil64,

            /// <summary>AMD HSAIL</summary>
            Hsail = LLVMTripleArchType.LlvmTripleArchType_hsail,

            /// <summary>AMD HSAIL with 64-bit pointers</summary>
            Hsail64 = LLVMTripleArchType.LlvmTripleArchType_hsail64,

            /// <summary>Standard Portable IR for OpenCL 32-bit version</summary>
            Spir = LLVMTripleArchType.LlvmTripleArchType_spir,

            /// <summary>Standard Portable IR for OpenCL 64-bit version</summary>
            Spir64 = LLVMTripleArchType.LlvmTripleArchType_spir64,

            /// <summary>Generic Kalimba</summary>
            Kalimba = LLVMTripleArchType.LlvmTripleArchType_kalimba,

            /// <summary>Movidius vector VLIW processors</summary>
            Shave = LLVMTripleArchType.LlvmTripleArchType_shave,

            /// <summary>Lanai 32-bit</summary>
            Lanai = LLVMTripleArchType.LlvmTripleArchType_lanai,

            /// <summary>WebAssembly with 32-bit pointers</summary>
            Wasm32 = LLVMTripleArchType.LlvmTripleArchType_wasm32,

            /// <summary>WebAssembly with 64-bit pointers</summary>
            Wasm64 = LLVMTripleArchType.LlvmTripleArchType_wasm64,

            /// <summary>Renderscript 32-bit</summary>
            Renderscript32 = LLVMTripleArchType.LlvmTripleArchType_renderscript32,

            /// <summary>Renderscript 64-bit</summary>
            Renderscript64 = LLVMTripleArchType.LlvmTripleArchType_renderscript64,

            /// <summary>Maximum architecture type</summary>
            LastArchType = Renderscript64
        }

        /// <summary>Processor sub architecture type</summary>
        [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Harder to understand without them")]
        public enum SubArchType
        {
            /// <summary>No sub architecture</summary>
            NoSubArch = LLVMTripleSubArchType.LlvmTripleSubArchType_NoSubArch,

            /// <summary>ARM v8.3a</summary>
            ARMSubArch_v8_3a = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8_3a,

            /// <summary>ARM v8.2a</summary>
            ARMSubArch_v8_2a = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8_2a,

            /// <summary>ARM v8.1a</summary>
            ARMSubArch_v8_1a = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8_1a,

            /// <summary>ARM v8</summary>
            ARMSubArch_v8 = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8,

            /// <summary>ARM v8r</summary>
            ARMSubArch_v8r = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8r,

            /// <summary>ARM v8m baseline</summary>
            ARMSubArch_v8m_baseline = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8m_baseline,

            /// <summary>ARM v8m mainline</summary>
            ARMSubArch_v8m_mainline = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v8m_mainline,

            /// <summary>ARM v7</summary>
            ARMSubArch_v7 = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7,

            /// <summary>ARM v7em</summary>
            ARMSubArch_v7em = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7em,

            /// <summary>ARM v7m</summary>
            ARMSubArch_v7m = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7m,

            /// <summary>ARM v7s</summary>
            ARMSubArch_v7s = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7s,

            /// <summary>ARM v7k</summary>
            ARMSubArch_v7k = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7k,

            /// <summary>ARM v7ve</summary>
            ARMSubArch_v7ve = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v7ve,

            /// <summary>ARM v6</summary>
            ARMSubArch_v6 = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6,

            /// <summary>ARM v6m</summary>
            ARMSubArch_v6m = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6m,

            /// <summary>ARM v6k</summary>
            ARMSubArch_v6k = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6k,

            /// <summary>ARM v6t2</summary>
            ARMSubArch_v6t2 = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v6t2,

            /// <summary>ARM v5</summary>
            ARMSubArch_v5 = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v5,

            /// <summary>ARM v5te</summary>
            ARMSubArch_v5te = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v5te,

            /// <summary>ARM v4t</summary>
            ARMSubArch_v4t = LLVMTripleSubArchType.LlvmTripleSubArchType_ARMSubArch_v4t,

            /// <summary>Kalimba v3</summary>
            KalimbaSubArch_v3 = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v3,

            /// <summary>Kalimba v4</summary>
            KalimbaSubArch_v4 = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v4,

            /// <summary>Kalimba v5</summary>
            KalimbaSubArch_v5 = LLVMTripleSubArchType.LlvmTripleSubArchType_KalimbaSubArch_v5,

            /// <summary>MIPS R6</summary>
            MipsSibArch_r6 = LLVMTripleSubArchType.LlvmTripleSubArchType_MipsSubArch_r6
        }

        /// <summary>Vendor type for the triple</summary>
        public enum VendorType
        {
            /// <summary>Unknown vendor</summary>
            UnknownVendor = LLVMTripleVendorType.LlvmTripleVendorType_UnknownVendor,

            /// <summary>Apple</summary>
            Apple = LLVMTripleVendorType.LlvmTripleVendorType_Apple,

            /// <summary>Generic PC</summary>
            PC = LLVMTripleVendorType.LlvmTripleVendorType_PC,

            /// <summary>SCEI</summary>
            SCEI = LLVMTripleVendorType.LlvmTripleVendorType_SCEI,

            /// <summary>BGP</summary>
            BGP = LLVMTripleVendorType.LlvmTripleVendorType_BGP,

            /// <summary>BGQ</summary>
            BGQ = LLVMTripleVendorType.LlvmTripleVendorType_BGQ,

            /// <summary>Freescale</summary>
            Freescale = LLVMTripleVendorType.LlvmTripleVendorType_Freescale,

            /// <summary>IBM</summary>
            IBM = LLVMTripleVendorType.LlvmTripleVendorType_IBM,

            /// <summary>Imagination Technologies</summary>
            ImaginationTechnologies = LLVMTripleVendorType.LlvmTripleVendorType_ImaginationTechnologies,

            /// <summary>MIPS Technologies</summary>
            MipsTechnologies = LLVMTripleVendorType.LlvmTripleVendorType_MipsTechnologies,

            /// <summary>NVidia</summary>
            NVIDIA = LLVMTripleVendorType.LlvmTripleVendorType_NVIDIA,

            /// <summary>CSR</summary>
            CSR = LLVMTripleVendorType.LlvmTripleVendorType_CSR,

            /// <summary>Myriad</summary>
            Myriad = LLVMTripleVendorType.LlvmTripleVendorType_Myriad,

            /// <summary>AMD</summary>
            AMD = LLVMTripleVendorType.LlvmTripleVendorType_AMD,

            /// <summary>Mesa</summary>
            Mesa = LLVMTripleVendorType.LlvmTripleVendorType_Mesa,

            /// <summary>SUSE</summary>
            SUSE = LLVMTripleVendorType.LlvmTripleVendorType_SUSE,

            /// <summary>OpenEmbedded</summary>
            OpenEmebedded = LLVMTripleVendorType.LlvmTripleVendorType_OpenEmbedded,
        }

        /// <summary>OS type for the triple</summary>
        public enum OSType
        {
            /// <summary>Unknown OS</summary>
            UnknownOS = LLVMTripleOSType.LlvmTripleOSType_UnknownOS,

            /// <summary>Ananas</summary>
            Ananas = LLVMTripleOSType.LlvmTripleOSType_Ananas,

            /// <summary>CloudABI</summary>
            CloudABI = LLVMTripleOSType.LlvmTripleOSType_CloudABI,

            /// <summary>Darwin</summary>
            Darwin = LLVMTripleOSType.LlvmTripleOSType_Darwin,

            /// <summary>DragonFly</summary>
            DragonFly = LLVMTripleOSType.LlvmTripleOSType_DragonFly,

            /// <summary>FreeBSD</summary>
            FreeBSD = LLVMTripleOSType.LlvmTripleOSType_FreeBSD,

            /// <summary>Fuchsia</summary>
            Fuchsia = LLVMTripleOSType.LlvmTripleOSType_Fuchsia,

            /// <summary>iOS</summary>
            IOS = LLVMTripleOSType.LlvmTripleOSType_IOS,

            /// <summary>KFreeBSD</summary>
            KFreeBSD = LLVMTripleOSType.LlvmTripleOSType_KFreeBSD,

            /// <summary>Linux</summary>
            Linux = LLVMTripleOSType.LlvmTripleOSType_Linux,

            /// <summary>Lv2</summary>
            Lv2 = LLVMTripleOSType.LlvmTripleOSType_Lv2,

            /// <summary>Mac OSX</summary>
            MacOSX = LLVMTripleOSType.LlvmTripleOSType_MacOSX,

            /// <summary>NetBSD</summary>
            NetBSD = LLVMTripleOSType.LlvmTripleOSType_NetBSD,

            /// <summary>OpenBSD</summary>
            OpenBSD = LLVMTripleOSType.LlvmTripleOSType_OpenBSD,

            /// <summary>Solaris</summary>
            Solaris = LLVMTripleOSType.LlvmTripleOSType_Solaris,

            /// <summary>Windows WIN32</summary>
            Win32 = LLVMTripleOSType.LlvmTripleOSType_Win32,

            /// <summary>Haiku</summary>
            Haiku = LLVMTripleOSType.LlvmTripleOSType_Haiku,

            /// <summary>Minix</summary>
            Minix = LLVMTripleOSType.LlvmTripleOSType_Minix,

            /// <summary>RTEMS</summary>
            RTEMS = LLVMTripleOSType.LlvmTripleOSType_RTEMS,

            /// <summary>NaCl</summary>
            NaCl = LLVMTripleOSType.LlvmTripleOSType_NaCl,

            /// <summary>CNK</summary>
            CNK = LLVMTripleOSType.LlvmTripleOSType_CNK,

            /// <summary>AIX</summary>
            AIX = LLVMTripleOSType.LlvmTripleOSType_AIX,

            /// <summary>CUDA</summary>
            CUDA = LLVMTripleOSType.LlvmTripleOSType_CUDA,

            /// <summary>NVCL</summary>
            NVCL = LLVMTripleOSType.LlvmTripleOSType_NVCL,

            /// <summary>AMD HSA</summary>
            AMDHSA = LLVMTripleOSType.LlvmTripleOSType_AMDHSA,

            /// <summary>PS4</summary>
            PS4 = LLVMTripleOSType.LlvmTripleOSType_PS4,

            /// <summary>ELFIAMCU</summary>
            ELFIAMCU = LLVMTripleOSType.LlvmTripleOSType_ELFIAMCU,

            /// <summary>TvOS</summary>
            TvOS = LLVMTripleOSType.LlvmTripleOSType_TvOS,

            /// <summary>WatchOS</summary>
            WatchOS = LLVMTripleOSType.LlvmTripleOSType_WatchOS,

            /// <summary>Mesa3D</summary>
            Mesa3D = LLVMTripleOSType.LlvmTripleOSType_Mesa3D,

            /// <summary>Contiki</summary>
            Contiki = LLVMTripleOSType.LlvmTripleOSType_Contiki,

            /// <summary>AMD PAL Runtime</summary>
            AmdPAL = LLVMTripleOSType.LlvmTripleOSType_AMDPAL,

            /// <summary>HermitCore Unikernel/Multikernel</summary>
            HermitCore = LLVMTripleOSType.LlvmTripleOSType_HermitCore,

            /// <summary>GNU/Hurd</summary>
            Hurd = LLVMTripleOSType.LlvmTripleOSType_Hurd,

            /// <summary>WebAssembly OS</summary>
            WASI = LLVMTripleOSType.LlvmTripleOSType_WASI,
        }

        /// <summary>Triple Environment type</summary>
        public enum EnvironmentType
        {
            /// <summary>Unknown environment</summary>
            UnknownEnvironment = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_UnknownEnvironment,

            /// <summary>GNU</summary>
            GNU = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNU,

            /// <summary>GNUABIN32</summary>
            GNUABIN32 = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUABIN32,

            /// <summary>GNU ABI 64-bit</summary>
            GNUABI64 = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUABI64,

            /// <summary>GNU EABI</summary>
            GNUEABI = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUEABI,

            /// <summary>GNU EABI-HF</summary>
            GNUEABIHF = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUEABIHF,

            /// <summary>GNU X32</summary>
            GNUX32 = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_GNUX32,

            /// <summary>CODE16</summary>
            CODE16 = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_CODE16,

            /// <summary>EABI</summary>
            EABI = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_EABI,

            /// <summary>EABI-HF</summary>
            EABIHF = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_EABIHF,

            /// <summary>Android</summary>
            Android = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Android,

            /// <summary>MUSL</summary>
            Musl = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Musl,

            /// <summary>MUSL EABI</summary>
            MuslEABI = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MuslEABI,

            /// <summary>MUSL EABI-HF</summary>
            MuslEABIHF = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MuslEABIHF,

            /// <summary>Microsoft Visual C</summary>
            MSVC = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_MSVC,

            /// <summary>Itanium</summary>
            Itanium = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Itanium,

            /// <summary>Cygnus</summary>
            Cygnus = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Cygnus,

            /// <summary>CoreCLR</summary>
            CoreCLR = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_CoreCLR,

            /// <summary>Simulator</summary>
            Simultator = LLVMTripleEnvironmentType.LlvmTripleEnvironmentType_Simulator
        }

        /// <summary>Object format type for a Triple</summary>
        public enum ObjectFormatType
        {
            /// <summary>Unknown format</summary>
            UnknownObjectFormat = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_UnknownObjectFormat,

            /// <summary>COFF format</summary>
            COFF = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_COFF,

            /// <summary>ELF format</summary>
            ELF = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_ELF,

            /// <summary>MachO format</summary>
            MachO = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_MachO,

            /// <summary>Wasm format</summary>
            Wasm = LLVMTripleObjectFormatType.LlvmTripleObjectFormatType_Wasm
        }

        /// <summary>Initializes a new instance of the <see cref="Triple"/> class from a triple string</summary>
        /// <param name="tripleTxt">Triple string to parse</param>
        /// <remarks>
        /// The <paramref name="tripleTxt"/> string is normalized before parsing to allow for
        /// common non-canonical forms of triples.
        /// </remarks>
        public Triple( string tripleTxt )
        {
            TripleHandle = LLVMParseTriple( tripleTxt );
        }

        /// <summary>Retrieves the final string form of the triple</summary>
        /// <returns>Normalized Triple string</returns>
        public override string ToString( ) => LLVMTripleAsString( TripleHandle, true );

        /// <summary>Gets the Architecture of the triple</summary>
        public ArchType ArchitectureType => ( ArchType )LLVMTripleGetArchType( TripleHandle );

        /// <summary>Gets the Sub Architecture type</summary>
        public SubArchType SubArchitecture => ( SubArchType )LLVMTripleGetSubArchType( TripleHandle );

        /// <summary>Gets the Vendor component of the triple</summary>
        public VendorType Vendor => ( VendorType )LLVMTripleGetVendorType( TripleHandle );

        /// <summary>Gets the OS Type for the triple</summary>
        public OSType OS => ( OSType )LLVMTripleGetOsType( TripleHandle );

        /// <summary>Gets the environment type for the triple</summary>
        public EnvironmentType Environment => ( EnvironmentType )LLVMTripleGetEnvironmentType( TripleHandle );

        /// <summary>Gets the object format type for the triple</summary>
        public ObjectFormatType ObjectFormat => ( ObjectFormatType )LLVMTripleGetObjectFormatType( TripleHandle );

        /// <summary>Gets the version number of the environment</summary>
        public Version EnvironmentVersion
        {
            get
            {
                LLVMTripleGetEnvironmentVersion( TripleHandle, out uint major, out uint minor, out uint micro );
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
            => LLVMTripleGetArchTypeName( ( LLVMTripleArchType )archType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for an architecture sub type</summary>
        /// <param name="subArchType">Architecture sub type</param>
        /// <returns>String name for the architecture sub type</returns>
        public static string GetCanonicalName( SubArchType subArchType )
            => LLVMTripleGetSubArchTypeName( ( LLVMTripleSubArchType )subArchType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the vendor component of a triple</summary>
        /// <param name="vendorType">Vendor type</param>
        /// <returns>String name for the vendor</returns>
        public static string GetCanonicalName( VendorType vendorType )
            => LLVMTripleGetVendorTypeName( ( LLVMTripleVendorType )vendorType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the OS component of a triple</summary>
        /// <param name="osType">OS type</param>
        /// <returns>String name for the OS</returns>
        public static string GetCanonicalName( OSType osType )
            => LLVMTripleGetOsTypeName( ( LLVMTripleOSType )osType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the environment component of a triple</summary>
        /// <param name="envType">Environment type</param>
        /// <returns>String name for the environment component</returns>
        public static string GetCanonicalName( EnvironmentType envType )
            => LLVMTripleGetEnvironmentTypeName( ( LLVMTripleEnvironmentType )envType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the object component of a triple</summary>
        /// <param name="objFormatType">Object type</param>
        /// <returns>String name for the object component</returns>
        public static string GetCanonicalName( ObjectFormatType objFormatType )
            => LLVMTripleGetObjectFormatTypeName( ( LLVMTripleObjectFormatType )objFormatType ) ?? string.Empty;

        /// <summary>Equality test for a triple</summary>
        /// <param name="other">triple to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public bool Equals( Triple other )
        {
            return (other != null && ReferenceEquals( this, other )) || LLVMTripleOpEqual( TripleHandle, other.TripleHandle );
        }

        /// <summary>Equality test for a triple</summary>
        /// <param name="obj">object to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public override bool Equals( object obj )
        {
            return Equals( obj as Triple );
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return ToString( ).GetHashCode( );
        }

        /// <summary>Normalizes a triple string</summary>
        /// <param name="unNormalizedTriple">triple to normalize</param>
        /// <returns>Normalized string</returns>
        public static string Normalize( string unNormalizedTriple )
        {
            unNormalizedTriple.ValidateNotNullOrWhiteSpace( nameof( unNormalizedTriple ) );

            return LLVMNormalizeTriple( unNormalizedTriple );
        }

        /// <summary>Gets the default <see cref="ObjectFormatType"/> for a given <see cref="ArchType"/> and <see cref="OSType"/></summary>
        /// <param name="arch">Architecture type</param>
        /// <param name="os">Operating system type</param>
        /// <returns>Default object format</returns>
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
            case ArchType.X86_64:
                if( IsOsDarwin( os ) )
                {
                    return ObjectFormatType.MachO;
                }

                if( os == OSType.Win32 )
                {
                    return ObjectFormatType.COFF;
                }

                return ObjectFormatType.ELF;

            case ArchType.Aarch64_be:
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
            switch( archType )
            {
            case ArchType.Kalimba:
                switch( subArch )
                {
                case SubArchType.NoSubArch:
                case SubArchType.KalimbaSubArch_v3:
                case SubArchType.KalimbaSubArch_v4:
                case SubArchType.KalimbaSubArch_v5:
                    return ArchType.Kalimba;

                default:
                    return ArchType.UnknownArch;
                }

            case ArchType.Arm:
            case ArchType.Armeb:
                switch( subArch )
                {
                case SubArchType.ARMSubArch_v6m:
                    return archType == ArchType.Armeb ? ArchType.Thumbeb : ArchType.Thumb;
                case SubArchType.KalimbaSubArch_v3:
                case SubArchType.KalimbaSubArch_v4:
                case SubArchType.KalimbaSubArch_v5:
                    return ArchType.UnknownArch;

                default:
                    return archType;
                }

            default:
                return archType;
            }
        }

        /// <summary>Gets a triple for the host LLVM is built for</summary>
        public static Triple HostTriple => new Triple( LLVMGetHostTriple( ) );

        /// <summary>Implicitly converts a triple to a string</summary>
        /// <param name="triple"><see cref="Triple"/> to convert</param>
        public static implicit operator string(Triple triple) => triple.ToString();

        private Triple( LLVMTripleRef handle )
        {
            TripleHandle = handle;
        }

        private static bool IsOsDarwin( OSType osType )
        {
            osType.ValidateDefined( nameof( osType ) );
            switch( osType )
            {
            case OSType.Darwin:
            case OSType.MacOSX:
            case OSType.IOS:
            case OSType.TvOS:
            case OSType.WatchOS:
                return true;

            default:
                return false;
            }
        }

        private readonly LLVMTripleRef TripleHandle;
    }
}
