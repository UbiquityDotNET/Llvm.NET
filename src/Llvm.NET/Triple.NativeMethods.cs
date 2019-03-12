// <copyright file="Triple.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Triple to describe a target</summary>
    public sealed partial class Triple
    {
        internal static class NativeMethods
        {
            internal enum LLVMTripleArchType
            {
                UnknownArch,
                arm,            // ARM (little endian): arm, armv.*, xscale
                armeb,          // ARM (big endian): armeb
                aarch64,        // AArch64 (little endian): aarch64
                aarch64_be,     // AArch64 (big endian): aarch64_be
                arc,            // ARC: Synopsys ARC
                avr,            // AVR: Atmel AVR Micro-controller
                bpfel,          // eBPF or extended BPF or 64-bit BPF (little endian)
                bpfeb,          // eBPF or extended BPF or 64-bit BPF (big endian)
                hexagon,        // Hexagon: hexagon
                mips,           // MIPS: mips, mipsallegrex
                mipsel,         // MIPSEL: mipsel, mipsallegrexel
                mips64,         // MIPS64: mips64
                mips64el,       // MIPS64EL: mips64el
                msp430,         // MSP430: msp430
                nios2,          // NIOSII: nios2
                ppc,            // PPC: powerpc
                ppc64,          // PPC64: powerpc64, ppu
                ppc64le,        // PPC64LE: powerpc64le
                r600,           // R600: AMD GPUs HD2XXX - HD6XXX
                amdgcn,         // AMDGCN: AMD GCN GPUs
                riscV32,        // RISC-V (32-bit): riscv32
                riscV64,        // RISC-V (64-bit): riscv64
                sparc,          // Sparc: sparc
                sparcv9,        // Sparcv9: Sparcv9
                sparcel,        // Sparc: (endianness = little). NB: 'Sparcle' is a CPU variant
                systemz,        // SystemZ: s390x
                tce,            // TCE (http://tce.cs.tut.fi/): tce
                tcele,          // TCE little endian (http://tce.cs.tut.fi/): tcele
                thumb,          // Thumb (little endian): thumb, thumbv.*
                thumbeb,        // Thumb (big endian): thumbeb
                x86,            // X86: i[3-9]86
                x86_64,         // X86-64: amd64, x86_64
                xcore,          // XCore: xcore
                nvptx,          // NVPTX: 32-bit
                nvptx64,        // NVPTX: 64-bit
                le32,           // le32: generic little-endian 32-bit CPU (PNaCl)
                le64,           // le64: generic little-endian 64-bit CPU (PNaCl)
                amdil,          // AMDIL
                amdil64,        // AMDIL with 64-bit pointers
                hsail,          // AMD HSAIL
                hsail64,        // AMD HSAIL with 64-bit pointers
                spir,           // SPIR: standard portable IR for OpenCL 32-bit version
                spir64,         // SPIR: standard portable IR for OpenCL 64-bit version
                kalimba,        // Kalimba: generic kalimba
                shave,          // SHAVE: Movidius vector VLIW processors
                lanai,          // Lanai: Lanai 32-bit
                wasm32,         // WebAssembly with 32-bit pointers
                wasm64,         // WebAssembly with 64-bit pointers
                renderscript32, // 32-bit RenderScript
                renderscript64, // 64-bit RenderScript
                LastArchType = renderscript64
            }

            internal enum LLVMTripleSubArchType
            {
                NoSubArch,
                ARMSubArch_v8_3a,
                ARMSubArch_v8_2a,
                ARMSubArch_v8_1a,
                ARMSubArch_v8,
                ARMSubArch_v8r,
                ARMSubArch_v8m_baseline,
                ARMSubArch_v8m_mainline,
                ARMSubArch_v7,
                ARMSubArch_v7em,
                ARMSubArch_v7m,
                ARMSubArch_v7s,
                ARMSubArch_v7k,
                ARMSubArch_v7ve,
                ARMSubArch_v6,
                ARMSubArch_v6m,
                ARMSubArch_v6k,
                ARMSubArch_v6t2,
                ARMSubArch_v5,
                ARMSubArch_v5te,
                ARMSubArch_v4t,
                KalimbaSubArch_v3,
                KalimbaSubArch_v4,
                KalimbaSubArch_v5
            }

            internal enum LLVMTripleVendorType
            {
                UnknownVendor,
                Apple,
                PC,
                SCEI,
                BGP,
                BGQ,
                Freescale,
                IBM,
                ImaginationTechnologies,
                MipsTechnologies,
                NVIDIA,
                CSR,
                Myriad,
                AMD,
                Mesa,
                SUSE,
                LastVendorType = SUSE
            }

            internal enum LLVMTripleOSType
            {
                UnknownOS,

                Ananas,
                CloudABI,
                Darwin,
                DragonFly,
                FreeBSD,
                Fuchsia,
                IOS,
                KFreeBSD,
                Linux,
                Lv2,        // PS3
                MacOSX,
                NetBSD,
                OpenBSD,
                Solaris,
                Win32,
                Haiku,
                Minix,
                RTEMS,
                NaCl,       // Native Client
                CNK,        // BG/P Compute-Node Kernel
                AIX,
                CUDA,       // NVIDIA CUDA
                NVCL,       // NVIDIA OpenCL
                AMDHSA,     // AMD HSA Runtime
                PS4,
                ELFIAMCU,
                TvOS,       // Apple tvOS
                WatchOS,    // Apple watchOS
                Mesa3D,
                Contiki,
                AMDPAL,
                LastOSType = AMDPAL
            }

            internal enum LLVMTripleEnvironmentType
            {
                UnknownEnvironment,
                GNU,
                GNUABIN32,
                GNUABI64,
                GNUEABI,
                GNUEABIHF,
                GNUX32,
                CODE16,
                EABI,
                EABIHF,
                Android,
                Musl,
                MuslEABI,
                MuslEABIHF,
                MSVC,
                Itanium,
                Cygnus,
                AMDOpenCL,
                CoreCLR,
                OpenCL,
                Simulator,
                LastEnvironmentType = Simulator
            }

            internal enum LLVMTripleObjectFormatType
            {
                UnknownObjectFormat,
                COFF,
                ELF,
                MachO,
                Wasm
            }

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleRef LLVMParseTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMTripleRef LLVMGetHostTriple( );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTripleOpEqual( LLVMTripleRef lhs, LLVMTripleRef rhs );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleArchType LLVMTripleGetArchType( LLVMTripleRef triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleSubArchType LLVMTripleGetSubArchType( LLVMTripleRef triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleVendorType LLVMTripleGetVendorType( LLVMTripleRef triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleOSType LLVMTripleGetOsType( LLVMTripleRef triple );

            /*[DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.Bool )]
            internal static extern bool LLVMTripleHasEnvironment( LLVMTripleRef triple );
            */

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleEnvironmentType LLVMTripleGetEnvironmentType( LLVMTripleRef triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern void LLVMTripleGetEnvironmentVersion( LLVMTripleRef triple, out UInt32 major, out UInt32 minor, out UInt32 micro );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            internal static extern LLVMTripleObjectFormatType LLVMTripleGetObjectFormatType( LLVMTripleRef triple );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleAsString( LLVMTripleRef triple, [MarshalAs( UnmanagedType.U1 )]bool normalize );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetArchTypeName( LLVMTripleArchType type );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetSubArchTypeName( LLVMTripleSubArchType type );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetVendorTypeName( LLVMTripleVendorType vendor );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetOsTypeName( LLVMTripleOSType osType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMTripleGetObjectFormatTypeName( LLVMTripleObjectFormatType environmentType );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
            [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
            internal static extern string LLVMNormalizeTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

            [DllImport( LibraryPath, EntryPoint = "LLVMGetDefaultTargetTriple", CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetDefaultTargetTriple( );
        }
    }
}
