// -----------------------------------------------------------------------
// <copyright file="Triple.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Llvm.Interop;

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
    /// <c>{Architecture}{SubArchitecture}-{Vendor}-{OS}-{EnvironmentKind}{ObjectFormatKind}</c></para>
    /// <para>
    /// A few shorthand variations are allowed and converted to their full normalized form.
    /// In particular "cygwin" is a shorthand for the OS-EnvironmentKind tuple "windows-cygnus"
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
        public enum ArchKind
        {
            /// <summary>Invalid or unknown architecture</summary>
            UnknownArch = LibLLVMTripleArchType.UnknownArch,

            /// <summary>ARM (little endian): arm, armv.*, xscale</summary>
            Arm = LibLLVMTripleArchType.arm,

            /// <summary>ARM (big endian): armeb</summary>
            Armeb = LibLLVMTripleArchType.armeb,

            /// <summary>AArch64 (little endian): aarch64</summary>
            Aarch64 = LibLLVMTripleArchType.aarch64,

            /// <summary>AArch64 (big endian): aarch64_be</summary>
            Aarch64BE = LibLLVMTripleArchType.aarch64_be,

            /// <summary>AArch64 32 bit (Little endian) ILP32: aarch64_32</summary>
            Aarch64_32 = LibLLVMTripleArchType.aarch64_32,

            /// <summary>Synopsis ARC</summary>
            Arc = LibLLVMTripleArchType.arc,

            /// <summary>AVR: Atmel AVR micro-controller</summary>
            Avr = LibLLVMTripleArchType.avr,

            /// <summary>eBPF or extended BPF or 64-bit BPF (little endian)</summary>
            BPFel = LibLLVMTripleArchType.bpfel,

            /// <summary>eBPF or extended BPF or 64-bit BPF (big endian)</summary>
            BPFeb = LibLLVMTripleArchType.bpfeb,

            /// <summary>csky</summary>
            Csky = LibLLVMTripleArchType.csky,

            /// <summary>32-bit DirectX bytecode</summary>
            DXil,

            /// <summary>Hexagon processor</summary>
            Hexagon = LibLLVMTripleArchType.hexagon,

            /// <summary>LoongArch (32-bit)</summary>
            LoongArch32 = LibLLVMTripleArchType.loongarch32,

            /// <summary>LoongArch (64-bit)</summary>
            LoongArch64 = LibLLVMTripleArchType.loongarch64,

            /// <summary>Motorola 680x0 family</summary>
            M68k = LibLLVMTripleArchType.m68k,

            /// <summary>MIPS: mips, mipsallegrex</summary>
            MIPS = LibLLVMTripleArchType.mips,

            /// <summary>MIPSEL: mipsel, mipsallegrexel</summary>
            MIPSel = LibLLVMTripleArchType.mipsel,

            /// <summary>MIPS 64 bit</summary>
            MIPS64 = LibLLVMTripleArchType.mips64,

            /// <summary>MIPS 64-bit little endian</summary>
            MIPS64el = LibLLVMTripleArchType.mips64el,

            /// <summary>MSP430</summary>
            MSP430 = LibLLVMTripleArchType.msp430,

            /// <summary>PowerPC</summary>
            PPC = LibLLVMTripleArchType.ppc,

            /// <summary>powerpc (little endian)</summary>
            PPCle = LibLLVMTripleArchType.ppcle,

            /// <summary>PowerPC 64-bit</summary>
            PPC64 = LibLLVMTripleArchType.ppc64,

            /// <summary>PowerPC 64-bit little endian</summary>
            PPC64le = LibLLVMTripleArchType.ppc64le,

            /// <summary>R600 AMD GPUS HD2XXX-HD6XXX</summary>
            R600 = LibLLVMTripleArchType.r600,

            /// <summary>AMD GCN GPUs</summary>
            AMDGCN = LibLLVMTripleArchType.amdgcn,

            /// <summary>RISC-V (32-bit)</summary>
            RiscV32 = LibLLVMTripleArchType.riscv32,

            /// <summary>RISC-V (64-bit)</summary>
            RiscV64 = LibLLVMTripleArchType.riscv64,

            /// <summary>Sparc</summary>
            Sparc = LibLLVMTripleArchType.sparc,

            /// <summary>SPARC V9</summary>
            Sparcv9 = LibLLVMTripleArchType.sparcv9,

            /// <summary>SPARC Little-Endian</summary>
            Sparcel = LibLLVMTripleArchType.sparcel,

            /// <summary>SystemZ - s390x</summary>
            SystemZ = LibLLVMTripleArchType.systemz,

            /// <summary>TCE</summary>
            TCE = LibLLVMTripleArchType.tce,

            /// <summary>TCE Little-Endian</summary>
            TCEle = LibLLVMTripleArchType.tcele,

            /// <summary>Thumb (little-endian)</summary>
            Thumb = LibLLVMTripleArchType.thumb,

            /// <summary>Thumb (big-endian)</summary>
            Thumbeb = LibLLVMTripleArchType.thumbeb,

            /// <summary>x86 i[3-9]86</summary>
            X86 = LibLLVMTripleArchType.x86,

            /// <summary>X86 64-bit (amd64)</summary>
            Amd64 = LibLLVMTripleArchType.x86_64,

            /// <summary>XCore</summary>
            Xcore = LibLLVMTripleArchType.xcore,

            /// <summary>Tensilica: Xtensa</summary>
            Xtensa = LibLLVMTripleArchType.xtensa,

            /// <summary>NVidia PTX 32-bit</summary>
            Nvptx = LibLLVMTripleArchType.nvptx,

            /// <summary>NVidia PTX 64-bit</summary>
            Nvptx64 = LibLLVMTripleArchType.nvptx64,

            /// <summary>AMD IL</summary>
            Amdil = LibLLVMTripleArchType.amdil,

            /// <summary>AMD IL 64-bit pointers</summary>
            Amdil64 = LibLLVMTripleArchType.amdil64,

            /// <summary>AMD HSAIL</summary>
            Hsail = LibLLVMTripleArchType.hsail,

            /// <summary>AMD HSAIL with 64-bit pointers</summary>
            Hsail64 = LibLLVMTripleArchType.hsail64,

            /// <summary>Standard Portable IR for OpenCL 32-bit version</summary>
            Spir = LibLLVMTripleArchType.spir,

            /// <summary>Standard Portable IR for OpenCL 64-bit version</summary>
            Spir64 = LibLLVMTripleArchType.spir64,

            /// <summary>SPIR-V with logical memory layout.</summary>
            SpirV = LibLLVMTripleArchType.spirv,

            /// <summary>SPIR-V with 32-bit pointers</summary>
            SpirV32 = LibLLVMTripleArchType.spirv32,

            /// <summary>SPIR-V with 64-bit pointers</summary>
            SpirV64 = LibLLVMTripleArchType.spirv64,

            /// <summary>Generic Kalimba</summary>
            Kalimba = LibLLVMTripleArchType.kalimba,

            /// <summary>Movidius vector VLIW processors</summary>
            Shave = LibLLVMTripleArchType.shave,

            /// <summary>Lanai 32-bit</summary>
            Lanai = LibLLVMTripleArchType.lanai,

            /// <summary>WebAssembly with 32-bit pointers</summary>
            Wasm32 = LibLLVMTripleArchType.wasm32,

            /// <summary>WebAssembly with 64-bit pointers</summary>
            Wasm64 = LibLLVMTripleArchType.wasm64,

            /// <summary>Renderscript 32-bit</summary>
            RenderScript32 = LibLLVMTripleArchType.renderscript32,

            /// <summary>Renderscript 64-bit</summary>
            RenderScript64 = LibLLVMTripleArchType.renderscript64,

            /// <summary>NEC SX Aurora Vector Engine</summary>
            Ve = LibLLVMTripleArchType.ve,
        }

        /// <summary>Processor sub architecture type</summary>
        [SuppressMessage( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Harder to understand without them" )]
        public enum SubArchKind
        {
            /// <summary>No sub architecture</summary>
            NoSubArch = LibLLVMTripleSubArchType.NoSubArch,

            /// <summary>ARM v9.6a</summary>
            ARMSubArch_v9_6a = LibLLVMTripleSubArchType.ARMSubArch_v9_6a,

            /// <summary>ARM v9.5a</summary>
            ARMSubArch_v9_5a = LibLLVMTripleSubArchType.ARMSubArch_v9_5a,

            /// <summary>ARM v9.4a</summary>
            ARMSubArch_v9_4a = LibLLVMTripleSubArchType.ARMSubArch_v9_4a,

            /// <summary>ARM v9.3a</summary>
            ARMSubArch_v9_3a = LibLLVMTripleSubArchType.ARMSubArch_v9_3a,

            /// <summary>ARM v9.2a</summary>
            ARMSubArch_v9_2a = LibLLVMTripleSubArchType.ARMSubArch_v9_2a,

            /// <summary>ARM v9.1a</summary>
            ARMSubArch_v9_1a = LibLLVMTripleSubArchType.ARMSubArch_v9_1a,

            /// <summary>ARM v9</summary>
            ARMSubArch_v9 = LibLLVMTripleSubArchType.ARMSubArch_v9,

            /// <summary>ARM v8.9a</summary>
            ARMSubArch_v8_9a = LibLLVMTripleSubArchType.ARMSubArch_v8_9a,

            /// <summary>ARM v8.8a</summary>
            ARMSubArch_v8_8a = LibLLVMTripleSubArchType.ARMSubArch_v8_8a,

            /// <summary>ARM v8.7a</summary>
            ARMSubArch_v8_7a = LibLLVMTripleSubArchType.ARMSubArch_v8_7a,

            /// <summary>ARM v8.6a</summary>
            ARMSubArch_v8_6a = LibLLVMTripleSubArchType.ARMSubArch_v8_6a,

            /// <summary>ARM v8.5a</summary>
            ARMSubArch_v8_5a = LibLLVMTripleSubArchType.ARMSubArch_v8_5a,

            /// <summary>ARM v8.4a</summary>
            ARMSubArch_v8_4a = LibLLVMTripleSubArchType.ARMSubArch_v8_4a,

            /// <summary>ARM v8.3a</summary>
            ARMSubArch_v8_3a = LibLLVMTripleSubArchType.ARMSubArch_v8_3a,

            /// <summary>ARM v8.2a</summary>
            ARMSubArch_v8_2a = LibLLVMTripleSubArchType.ARMSubArch_v8_2a,

            /// <summary>ARM v8.1a</summary>
            ARMSubArch_v8_1a = LibLLVMTripleSubArchType.ARMSubArch_v8_1a,

            /// <summary>ARM v8</summary>
            ARMSubArch_v8 = LibLLVMTripleSubArchType.ARMSubArch_v8,

            /// <summary>ARM v8r</summary>
            ARMSubArch_v8r = LibLLVMTripleSubArchType.ARMSubArch_v8r,

            /// <summary>ARM v8m baseline</summary>
            ARMSubArch_v8m_Baseline = LibLLVMTripleSubArchType.ARMSubArch_v8m_baseline,

            /// <summary>ARM v8m mainline</summary>
            ARMSubArch_v8m_Mainline = LibLLVMTripleSubArchType.ARMSubArch_v8m_mainline,

            /// <summary>ARM v8 1m mainline</summary>
            ARMSubArch_v8_1m_Mainline = LibLLVMTripleSubArchType.ARMSubArch_v8_1m_mainline,

            /// <summary>ARM v7</summary>
            ARMSubArch_v7 = LibLLVMTripleSubArchType.ARMSubArch_v7,

            /// <summary>ARM v7em</summary>
            ARMSubArch_v7em = LibLLVMTripleSubArchType.ARMSubArch_v7em,

            /// <summary>ARM v7m</summary>
            ARMSubArch_v7m = LibLLVMTripleSubArchType.ARMSubArch_v7m,

            /// <summary>ARM v7s</summary>
            ARMSubArch_v7s = LibLLVMTripleSubArchType.ARMSubArch_v7s,

            /// <summary>ARM v7k</summary>
            ARMSubArch_v7k = LibLLVMTripleSubArchType.ARMSubArch_v7k,

            /// <summary>ARM v7ve</summary>
            ARMSubArch_v7ve = LibLLVMTripleSubArchType.ARMSubArch_v7ve,

            /// <summary>ARM v6</summary>
            ARMSubArch_v6 = LibLLVMTripleSubArchType.ARMSubArch_v6,

            /// <summary>ARM v6m</summary>
            ARMSubArch_v6m = LibLLVMTripleSubArchType.ARMSubArch_v6m,

            /// <summary>ARM v6k</summary>
            ARMSubArch_v6k = LibLLVMTripleSubArchType.ARMSubArch_v6k,

            /// <summary>ARM v6t2</summary>
            ARMSubArch_v6t2 = LibLLVMTripleSubArchType.ARMSubArch_v6t2,

            /// <summary>ARM v5</summary>
            ARMSubArch_v5 = LibLLVMTripleSubArchType.ARMSubArch_v5,

            /// <summary>ARM v5te</summary>
            ARMSubArch_v5te = LibLLVMTripleSubArchType.ARMSubArch_v5te,

            /// <summary>ARM v4t</summary>
            ARMSubArch_v4t = LibLLVMTripleSubArchType.ARMSubArch_v4t,

            /// <summary>AArch64SubArch_Arm64e</summary>
            AArch64SubArch_Arm64e = LibLLVMTripleSubArchType.AArch64SubArch_arm64e,

            /// <summary>AArch64SubArch_Arm64e</summary>
            AArch64SubArch_Arm64ec = LibLLVMTripleSubArchType.AArch64SubArch_arm64ec,

            /// <summary>Kalimba v3</summary>
            KalimbaSubArch_v3 = LibLLVMTripleSubArchType.KalimbaSubArch_v3,

            /// <summary>Kalimba v4</summary>
            KalimbaSubArch_v4 = LibLLVMTripleSubArchType.KalimbaSubArch_v4,

            /// <summary>Kalimba v5</summary>
            KalimbaSubArch_v5 = LibLLVMTripleSubArchType.KalimbaSubArch_v5,

            /// <summary>MIPS R6</summary>
            MipsSubArch_r6 = LibLLVMTripleSubArchType.MipsSubArch_r6,

            /// <summary>PowerPC SPE</summary>
            PowerPC_SPE = LibLLVMTripleSubArchType.PPCSubArch_spe,

            // SPIR-V sub-arch corresponds to its version.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1602 // Enumeration items should be documented
            SPIRVSubArch_v10 = LibLLVMTripleSubArchType.SPIRVSubArch_v10,
            SPIRVSubArch_v11 = LibLLVMTripleSubArchType.SPIRVSubArch_v11,
            SPIRVSubArch_v12 = LibLLVMTripleSubArchType.SPIRVSubArch_v12,
            SPIRVSubArch_v13 = LibLLVMTripleSubArchType.SPIRVSubArch_v13,
            SPIRVSubArch_v14 = LibLLVMTripleSubArchType.SPIRVSubArch_v14,
            SPIRVSubArch_v15 = LibLLVMTripleSubArchType.SPIRVSubArch_v15,
            SPIRVSubArch_v16 = LibLLVMTripleSubArchType.SPIRVSubArch_v16,

            // DXIL sub-arch corresponds to its version.
            DXILSubArch_v1_0 = LibLLVMTripleSubArchType.DXILSubArch_v1_0,
            DXILSubArch_v1_1 = LibLLVMTripleSubArchType.DXILSubArch_v1_1,
            DXILSubArch_v1_2 = LibLLVMTripleSubArchType.DXILSubArch_v1_2,
            DXILSubArch_v1_3 = LibLLVMTripleSubArchType.DXILSubArch_v1_3,
            DXILSubArch_v1_4 = LibLLVMTripleSubArchType.DXILSubArch_v1_4,
            DXILSubArch_v1_5 = LibLLVMTripleSubArchType.DXILSubArch_v1_5,
            DXILSubArch_v1_6 = LibLLVMTripleSubArchType.DXILSubArch_v1_6,
            DXILSubArch_v1_7 = LibLLVMTripleSubArchType.DXILSubArch_v1_7,
            DXILSubArch_v1_8 = LibLLVMTripleSubArchType.DXILSubArch_v1_8,
            LatestDXILSubArch = DXILSubArch_v1_8,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>Vendor type for the triple</summary>
        public enum VendorKind
        {
            /// <summary>Unknown vendor</summary>
            Unknown = LibLLVMTripleVendorType.UnknownVendor,

            /// <summary>Apple</summary>
            Apple = LibLLVMTripleVendorType.Apple,

            /// <summary>Generic PC</summary>
            PC = LibLLVMTripleVendorType.PC,

            /// <summary>SCEI</summary>
            SCEI = LibLLVMTripleVendorType.SCEI,

            /// <summary>Freescale</summary>
            Freescale = LibLLVMTripleVendorType.Freescale,

            /// <summary>IBM</summary>
            IBM = LibLLVMTripleVendorType.IBM,

            /// <summary>Imagination Technologies</summary>
            ImaginationTechnologies = LibLLVMTripleVendorType.ImaginationTechnologies,

            /// <summary>MIPS Technologies</summary>
            MipsTechnologies = LibLLVMTripleVendorType.MipsTechnologies,

            /// <summary>NVidia</summary>
            NVIDIA = LibLLVMTripleVendorType.NVIDIA,

            /// <summary>CSR</summary>
            CSR = LibLLVMTripleVendorType.CSR,

            /// <summary>AMD</summary>
            AMD = LibLLVMTripleVendorType.AMD,

            /// <summary>Mesa</summary>
            Mesa = LibLLVMTripleVendorType.Mesa,

            /// <summary>SUSE</summary>
            SUSE = LibLLVMTripleVendorType.SUSE,

            /// <summary>OpenEmbedded</summary>
            OpenEmbedded = LibLLVMTripleVendorType.OpenEmbedded,

            /// <summary>Intel</summary>
            Intel = LibLLVMTripleVendorType.Intel,
        }

        /// <summary>OS type for the triple</summary>
        public enum OSKind
        {
            /// <summary>Unknown OS</summary>
            UnknownOS = LibLLVMTripleOSType.UnknownOS,

            /// <summary>Darwin</summary>
            Darwin = LibLLVMTripleOSType.Darwin,

            /// <summary>DragonFly</summary>
            DragonFly = LibLLVMTripleOSType.DragonFly,

            /// <summary>FreeBSD</summary>
            FreeBSD = LibLLVMTripleOSType.FreeBSD,

            /// <summary>Fuchsia</summary>
            Fuchsia = LibLLVMTripleOSType.Fuchsia,

            /// <summary>iOS</summary>
            IOS = LibLLVMTripleOSType.IOS,

            /// <summary>KFreeBSD</summary>
            KFreeBSD = LibLLVMTripleOSType.KFreeBSD,

            /// <summary>Linux</summary>
            Linux = LibLLVMTripleOSType.Linux,

            /// <summary>Lv2 (PS3)</summary>
            Lv2 = LibLLVMTripleOSType.Lv2,

            /// <summary>Mac OSX</summary>
            MacOSX = LibLLVMTripleOSType.MacOSX,

            /// <summary>NetBSD</summary>
            NetBSD = LibLLVMTripleOSType.NetBSD,

            /// <summary>OpenBSD</summary>
            OpenBSD = LibLLVMTripleOSType.OpenBSD,

            /// <summary>Solaris</summary>
            Solaris = LibLLVMTripleOSType.Solaris,

            /// <summary>Unified Extensible Firmware Interface (UEFI)</summary>
            UEFI = LibLLVMTripleOSType.UEFI,

            /// <summary>Windows (WIN32)</summary>
            Win32 = LibLLVMTripleOSType.Win32,

            /// <summary>Windows WIN32</summary>
            ZOS = LibLLVMTripleOSType.ZOS,

            /// <summary>Haiku</summary>
            Haiku = LibLLVMTripleOSType.Haiku,

            /// <summary>RTEMS</summary>
            RTEMS = LibLLVMTripleOSType.RTEMS,

            /// <summary>NaCl</summary>
            NaCl = LibLLVMTripleOSType.NaCl,

            /// <summary>AIX</summary>
            AIX = LibLLVMTripleOSType.AIX,

            /// <summary>CUDA</summary>
            CUDA = LibLLVMTripleOSType.CUDA,

            /// <summary>NVCL</summary>
            NVCL = LibLLVMTripleOSType.NVCL,

            /// <summary>AMD HSA Runtime</summary>
            AMDHSA = LibLLVMTripleOSType.AMDHSA,

            /// <summary>PS4</summary>
            PS4 = LibLLVMTripleOSType.PS4,

            /// <summary>PS5</summary>
            PS5 = LibLLVMTripleOSType.PS5,

            /// <summary>ELFIAMCU</summary>
            ELFIAMCU = LibLLVMTripleOSType.ELFIAMCU,

            /// <summary>TvOS</summary>
            TvOS = LibLLVMTripleOSType.TvOS,

            /// <summary>WatchOS</summary>
            WatchOS = LibLLVMTripleOSType.WatchOS,

            /// <summary>Apple bridgeOS</summary>
            BridgeOS = LibLLVMTripleOSType.BridgeOS,

            /// <summary>Apple DriverKit</summary>
            DriverKit = LibLLVMTripleOSType.DriverKit,

            /// <summary>Apple XROS</summary>
            XROS = LibLLVMTripleOSType.XROS,

            /// <summary>Mesa3D</summary>
            Mesa3D = LibLLVMTripleOSType.Mesa3D,

            /// <summary>AMD PAL Runtime</summary>
            AmdPAL = LibLLVMTripleOSType.AMDPAL,

            /// <summary>HermitCore Unikernel/Multikernel</summary>
            HermitCore = LibLLVMTripleOSType.HermitCore,

            /// <summary>GNU/Hurd</summary>
            Hurd = LibLLVMTripleOSType.Hurd,

            /// <summary>WebAssembly OS</summary>
            WASI = LibLLVMTripleOSType.WASI,

            /// <summary>Emscripten</summary>
            Emscripten = LibLLVMTripleOSType.Emscripten,

            /// <summary>DirectX ShaderModel</summary>
            ShaderModel = LibLLVMTripleOSType.ShaderModel,

            /// <summary>Lite OS</summary>
            LiteOS = LibLLVMTripleOSType.LiteOS,

            /// <summary>Serenity</summary>
            Serenity = LibLLVMTripleOSType.Serenity,

            /// <summary>Vulkan SPIR-V</summary>
            Vulkan = LibLLVMTripleOSType.Vulkan,
        }

        /// <summary>Triple EnvironmentKind type</summary>
        [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "NOT flags!" )]
        public enum EnvironmentKind
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1602 // Enumeration items should be documented
            /// <summary>Unknown environment</summary>
            Unknown = LibLLVMTripleEnvironmentType.UnknownEnvironment,
            GNU = LibLLVMTripleEnvironmentType.GNU,
            GNUT64 = LibLLVMTripleEnvironmentType.GNUT64,
            GNUABIN32 = LibLLVMTripleEnvironmentType.GNUABIN32,
            GNUABI64 = LibLLVMTripleEnvironmentType.GNUABI64,
            GNUEABI = LibLLVMTripleEnvironmentType.GNUEABI,
            GNUEABIHF = LibLLVMTripleEnvironmentType.GNUEABIHF,
            GNUEABIHFT64 = LibLLVMTripleEnvironmentType.GNUEABIHFT64,
            GNUF32 = LibLLVMTripleEnvironmentType.GNUF32,
            GNUF64 = LibLLVMTripleEnvironmentType.GNUF32,
            GNUSF = LibLLVMTripleEnvironmentType.GNUSF,
            GNUX32 = LibLLVMTripleEnvironmentType.GNUX32,
            GNUILP32 = LibLLVMTripleEnvironmentType.GNUILP32,
            CODE16 = LibLLVMTripleEnvironmentType.CODE16,
            EABI = LibLLVMTripleEnvironmentType.EABI,
            EABIHF = LibLLVMTripleEnvironmentType.EABIHF,
            Android = LibLLVMTripleEnvironmentType.Android,
            Musl = LibLLVMTripleEnvironmentType.Musl,
            MuslABIN32 = LibLLVMTripleEnvironmentType.MuslABIN32,
            MuslABI64 = LibLLVMTripleEnvironmentType.MuslABI64,
            MuslEABI = LibLLVMTripleEnvironmentType.MuslEABI,
            MuslEABIHF = LibLLVMTripleEnvironmentType.MuslEABIHF,
            MuslF32 = LibLLVMTripleEnvironmentType.MuslF32,
            MuslSF = LibLLVMTripleEnvironmentType.MuslF32,
            MuslX32 = LibLLVMTripleEnvironmentType.MuslX32,
            LLVM = LibLLVMTripleEnvironmentType.LLVM,
            MSVC = LibLLVMTripleEnvironmentType.MSVC,
            Itanium = LibLLVMTripleEnvironmentType.Itanium,
            Cygnus = LibLLVMTripleEnvironmentType.Cygnus,
            CoreCLR = LibLLVMTripleEnvironmentType.CoreCLR,
            Simulator = LibLLVMTripleEnvironmentType.Simulator,
            MacABI = LibLLVMTripleEnvironmentType.MacABI,
            Pixel = LibLLVMTripleEnvironmentType.Pixel,
            Vertex = LibLLVMTripleEnvironmentType.Vertex,
            Geometry = LibLLVMTripleEnvironmentType.Geometry,
            Hull = LibLLVMTripleEnvironmentType.Hull,
            Domain = LibLLVMTripleEnvironmentType.Domain,
            Compute = LibLLVMTripleEnvironmentType.Compute,
            Library = LibLLVMTripleEnvironmentType.Library,
            RayGeneration = LibLLVMTripleEnvironmentType.RayGeneration,
            Intersection = LibLLVMTripleEnvironmentType.Intersection,
            AnyHit = LibLLVMTripleEnvironmentType.AnyHit,
            ClosestHit = LibLLVMTripleEnvironmentType.ClosestHit,
            Miss = LibLLVMTripleEnvironmentType.Miss,
            Callable = LibLLVMTripleEnvironmentType.Callable,
            Mesh = LibLLVMTripleEnvironmentType.Mesh,
            Amplification = LibLLVMTripleEnvironmentType.Amplification,
            OpenCL = LibLLVMTripleEnvironmentType.OpenCL,
            OpenHOS = LibLLVMTripleEnvironmentType.OpenHOS,
            PAuthTest = LibLLVMTripleEnvironmentType.PAuthTest,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>Object format type for a Triple</summary>
        public enum ObjectFormatKind
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1602 // Enumeration items should be documented
            Unknown = LibLLVMTripleObjectFormatType.UnknownObjectFormat,
            COFF = LibLLVMTripleObjectFormatType.COFF,
            DXContainer = LibLLVMTripleObjectFormatType.DXContainer,
            ELF = LibLLVMTripleObjectFormatType.ELF,
            MachO = LibLLVMTripleObjectFormatType.MachO,
            SpirV = LibLLVMTripleObjectFormatType.SPIRV,
            Wasm = LibLLVMTripleObjectFormatType.Wasm,
            XCOFF = LibLLVMTripleObjectFormatType.XCOFF,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        public override string? ToString( ) => LibLLVMTripleAsString( TripleHandle, true ).ToString();

        /// <summary>Gets the Architecture of the triple</summary>
        public ArchKind ArchitectureType => ( ArchKind )LibLLVMTripleGetArchType( TripleHandle );

        /// <summary>Gets the Sub Architecture type</summary>
        public SubArchKind SubArchitecture => ( SubArchKind )LibLLVMTripleGetSubArchType( TripleHandle );

        /// <summary>Gets the Vendor component of the triple</summary>
        public VendorKind Vendor => ( VendorKind )LibLLVMTripleGetVendorType( TripleHandle );

        /// <summary>Gets the OS Type for the triple</summary>
        public OSKind OS => ( OSKind )LibLLVMTripleGetOsType( TripleHandle );

        /// <summary>Gets the environment type for the triple</summary>
        public EnvironmentKind Environment => ( EnvironmentKind )LibLLVMTripleGetEnvironmentType( TripleHandle );

        /// <summary>Gets the object format type for the triple</summary>
        public ObjectFormatKind ObjectFormat => ( ObjectFormatKind )LibLLVMTripleGetObjectFormatType( TripleHandle );

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
        public static string GetCanonicalName( ArchKind archType )
            => LibLLVMTripleGetArchTypeName( ( LibLLVMTripleArchType )archType ).ToString() ?? string.Empty;

        /// <summary>Retrieves the canonical name for the vendor component of a triple</summary>
        /// <param name="vendorType">Vendor type</param>
        /// <returns>String name for the vendor</returns>
        public static string GetCanonicalName( VendorKind vendorType )
            => LibLLVMTripleGetVendorTypeName( ( LibLLVMTripleVendorType )vendorType ).ToString() ?? string.Empty;

        /// <summary>Retrieves the canonical name for the OS component of a triple</summary>
        /// <param name="osType">OS type</param>
        /// <returns>String name for the OS</returns>
        public static string GetCanonicalName( OSKind osType )
            => LibLLVMTripleGetOsTypeName( ( LibLLVMTripleOSType )osType ).ToString() ?? string.Empty;

        /// <summary>Retrieves the canonical name for the environment component of a triple</summary>
        /// <param name="envType">Environment type</param>
        /// <returns>String name for the environment component</returns>
        public static string GetCanonicalName( EnvironmentKind envType )
            => LibLLVMTripleGetEnvironmentTypeName( ( LibLLVMTripleEnvironmentType )envType ).ToString() ?? string.Empty;

        /// <inheritdoc/>
        public bool Equals( Triple? other )
        {
            return other != null && ( ReferenceEquals( this, other ) || LibLLVMTripleOpEqual( TripleHandle, other.TripleHandle ) );
        }

        /// <inheritdoc/>
        public override bool Equals( object? obj )
        {
            return Equals( obj as Triple );
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return ToString( )?.GetHashCode( StringComparison.Ordinal ) ?? 0;
        }

        /// <summary>Normalizes a triple string</summary>
        /// <param name="unNormalizedTriple">triple to normalize</param>
        /// <returns>Normalized string</returns>
        public static string Normalize( string unNormalizedTriple )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( unNormalizedTriple );

            return LLVMNormalizeTargetTriple( unNormalizedTriple ).ToString() ?? string.Empty;
        }

        /// <summary>Gets a triple for the host LLVM is built for</summary>
        public static Triple Host => new(LibLLVMGetHostTriple());

        /// <summary>Implicitly converts a triple to a string</summary>
        /// <param name="triple"><see cref="Triple"/> to convert</param>
        /// <returns>Triple as a string or <see cref="string.Empty"/> if <paramref name="triple"/> is <see langword="null"/></returns>
        public static implicit operator string( Triple? triple ) => triple?.ToString( ) ?? string.Empty;

        private Triple( LibLLVMTripleRef handle )
        {
            TripleHandle = handle;
        }

        private readonly LibLLVMTripleRef TripleHandle;
    }
}
