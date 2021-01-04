#ifndef LLVM_TRIPLE_BINDINGS_H
#define LLVM_TRIPLE_BINDINGS_H
#include <llvm-c/Types.h>

#ifdef __cplusplus
extern "C" {
#endif

    enum LibLLVMTripleArchType
    {
        LibLLVMTripleArchType_UnknownArch,

        LibLLVMTripleArchType_arm,            // ARM (little endian): arm, armv.*, xscale
        LibLLVMTripleArchType_armeb,          // ARM (big endian): armeb
        LibLLVMTripleArchType_aarch64,        // AArch64 (little endian): aarch64
        LibLLVMTripleArchType_aarch64_be,     // AArch64 (big endian): aarch64_be
        LibLLVMTripleArchType_aarch64_32,     // AArch64 (little endian) ILP32: aarch64_32
        LibLLVMTripleArchType_arc,            // ARC: Synopsys ARC
        LibLLVMTripleArchType_avr,            // AVR: Atmel AVR microcontroller
        LibLLVMTripleArchType_bpfel,          // eBPF or extended BPF or 64-bit BPF (little endian)
        LibLLVMTripleArchType_bpfeb,          // eBPF or extended BPF or 64-bit BPF (big endian)
        LibLLVMTripleArchType_hexagon,        // Hexagon: hexagon
        LibLLVMTripleArchType_mips,           // MIPS: mips, mipsallegrex, mipsr6
        LibLLVMTripleArchType_mipsel,         // MIPSEL: mipsel, mipsallegrexe, mipsr6el
        LibLLVMTripleArchType_mips64,         // MIPS64: mips64, mips64r6, mipsn32, mipsn32r6
        LibLLVMTripleArchType_mips64el,       // MIPS64EL: mips64el, mips64r6el, mipsn32el, mipsn32r6el
        LibLLVMTripleArchType_msp430,         // MSP430: msp430
        LibLLVMTripleArchType_ppc,            // PPC: powerpc
        LibLLVMTripleArchType_ppc64,          // PPC64: powerpc64, ppu
        LibLLVMTripleArchType_ppc64le,        // PPC64LE: powerpc64le
        LibLLVMTripleArchType_r600,           // R600: AMD GPUs HD2XXX - HD6XXX
        LibLLVMTripleArchType_amdgcn,         // AMDGCN: AMD GCN GPUs
        LibLLVMTripleArchType_riscv32,        // RISC-V (32-bit): riscv32
        LibLLVMTripleArchType_riscv64,        // RISC-V (64-bit): riscv64
        LibLLVMTripleArchType_sparc,          // Sparc: sparc
        LibLLVMTripleArchType_sparcv9,        // Sparcv9: Sparcv9
        LibLLVMTripleArchType_sparcel,        // Sparc: (endianness = little). NB: 'Sparcle' is a CPU variant
        LibLLVMTripleArchType_systemz,        // SystemZ: s390x
        LibLLVMTripleArchType_tce,            // TCE (http://tce.cs.tut.fi/): tce
        LibLLVMTripleArchType_tcele,          // TCE little endian (http://tce.cs.tut.fi/): tcele
        LibLLVMTripleArchType_thumb,          // Thumb (little endian): thumb, thumbv.*
        LibLLVMTripleArchType_thumbeb,        // Thumb (big endian): thumbeb
        LibLLVMTripleArchType_x86,            // X86: i[3-9]86
        LibLLVMTripleArchType_x86_64,         // X86-64: amd64, x86_64
        LibLLVMTripleArchType_xcore,          // XCore: xcore
        LibLLVMTripleArchType_nvptx,          // NVPTX: 32-bit
        LibLLVMTripleArchType_nvptx64,        // NVPTX: 64-bit
        LibLLVMTripleArchType_le32,           // le32: generic little-endian 32-bit CPU (PNaCl)
        LibLLVMTripleArchType_le64,           // le64: generic little-endian 64-bit CPU (PNaCl)
        LibLLVMTripleArchType_amdil,          // AMDIL
        LibLLVMTripleArchType_amdil64,        // AMDIL with 64-bit pointers
        LibLLVMTripleArchType_hsail,          // AMD HSAIL
        LibLLVMTripleArchType_hsail64,        // AMD HSAIL with 64-bit pointers
        LibLLVMTripleArchType_spir,           // SPIR: standard portable IR for OpenCL 32-bit version
        LibLLVMTripleArchType_spir64,         // SPIR: standard portable IR for OpenCL 64-bit version
        LibLLVMTripleArchType_kalimba,        // Kalimba: generic kalimba
        LibLLVMTripleArchType_shave,          // SHAVE: Movidius vector VLIW processors
        LibLLVMTripleArchType_lanai,          // Lanai: Lanai 32-bit
        LibLLVMTripleArchType_wasm32,         // WebAssembly with 32-bit pointers
        LibLLVMTripleArchType_wasm64,         // WebAssembly with 64-bit pointers
        LibLLVMTripleArchType_renderscript32, // 32-bit RenderScript
        LibLLVMTripleArchType_renderscript64, // 64-bit RenderScript
        LibLLVMTripleArchType_ve,             // NEC SX-Aurora Vector Engine
        LibLLVMTripleArchType_LastArchType = LibLLVMTripleArchType_ve
    };

    enum LibLLVMTripleSubArchType
    {
        LibLLVMTripleSubArchType_NoSubArch,

        LibLLVMTripleSubArchType_ARMSubArch_v8_5a,
        LibLLVMTripleSubArchType_ARMSubArch_v8_4a,
        LibLLVMTripleSubArchType_ARMSubArch_v8_3a,
        LibLLVMTripleSubArchType_ARMSubArch_v8_2a,
        LibLLVMTripleSubArchType_ARMSubArch_v8_1a,
        LibLLVMTripleSubArchType_ARMSubArch_v8,
        LibLLVMTripleSubArchType_ARMSubArch_v8r,
        LibLLVMTripleSubArchType_ARMSubArch_v8m_baseline,
        LibLLVMTripleSubArchType_ARMSubArch_v8m_mainline,
        LibLLVMTripleSubArchType_ARMSubArch_v8_1m_mainline,
        LibLLVMTripleSubArchType_ARMSubArch_v7,
        LibLLVMTripleSubArchType_ARMSubArch_v7em,
        LibLLVMTripleSubArchType_ARMSubArch_v7m,
        LibLLVMTripleSubArchType_ARMSubArch_v7s,
        LibLLVMTripleSubArchType_ARMSubArch_v7k,
        LibLLVMTripleSubArchType_ARMSubArch_v7ve,
        LibLLVMTripleSubArchType_ARMSubArch_v6,
        LibLLVMTripleSubArchType_ARMSubArch_v6m,
        LibLLVMTripleSubArchType_ARMSubArch_v6k,
        LibLLVMTripleSubArchType_ARMSubArch_v6t2,
        LibLLVMTripleSubArchType_ARMSubArch_v5,
        LibLLVMTripleSubArchType_ARMSubArch_v5te,
        LibLLVMTripleSubArchType_ARMSubArch_v4t,

        LibLLVMTripleSubArchType_KalimbaSubArch_v3,
        LibLLVMTripleSubArchType_KalimbaSubArch_v4,
        LibLLVMTripleSubArchType_KalimbaSubArch_v5,

        LibLLVMTripleSubArchType_MipsSubArch_r6,

        LibLLVMTripleSubArchType_PPCSubArch_spe
    };

    enum LibLLVMTripleVendorType
    {
        LibLLVMTripleVendorType_UnknownVendor,

        LibLLVMTripleVendorType_Apple,
        LibLLVMTripleVendorType_PC,
        LibLLVMTripleVendorType_SCEI,
        LibLLVMTripleVendorType_BGP,
        LibLLVMTripleVendorType_BGQ,
        LibLLVMTripleVendorType_Freescale,
        LibLLVMTripleVendorType_IBM,
        LibLLVMTripleVendorType_ImaginationTechnologies,
        LibLLVMTripleVendorType_MipsTechnologies,
        LibLLVMTripleVendorType_NVIDIA,
        LibLLVMTripleVendorType_CSR,
        LibLLVMTripleVendorType_Myriad,
        LibLLVMTripleVendorType_AMD,
        LibLLVMTripleVendorType_Mesa,
        LibLLVMTripleVendorType_SUSE,
        LibLLVMTripleVendorType_OpenEmbedded,
        LibLLVMTripleVendorType_LastVendorType = LibLLVMTripleVendorType_OpenEmbedded
    };

    enum LibLLVMTripleOSType
    {
        LibLLVMTripleOSType_UnknownOS,

        LibLLVMTripleOSType_Ananas,
        LibLLVMTripleOSType_CloudABI,
        LibLLVMTripleOSType_Darwin,
        LibLLVMTripleOSType_DragonFly,
        LibLLVMTripleOSType_FreeBSD,
        LibLLVMTripleOSType_Fuchsia,
        LibLLVMTripleOSType_IOS,
        LibLLVMTripleOSType_KFreeBSD,
        LibLLVMTripleOSType_Linux,
        LibLLVMTripleOSType_Lv2,        // PS3
        LibLLVMTripleOSType_MacOSX,
        LibLLVMTripleOSType_NetBSD,
        LibLLVMTripleOSType_OpenBSD,
        LibLLVMTripleOSType_Solaris,
        LibLLVMTripleOSType_Win32,
        LibLLVMTripleOSType_Haiku,
        LibLLVMTripleOSType_Minix,
        LibLLVMTripleOSType_RTEMS,
        LibLLVMTripleOSType_NaCl,       // Native Client
        LibLLVMTripleOSType_CNK,        // BG/P Compute-Node Kernel
        LibLLVMTripleOSType_AIX,
        LibLLVMTripleOSType_CUDA,       // NVIDIA CUDA
        LibLLVMTripleOSType_NVCL,       // NVIDIA OpenCL
        LibLLVMTripleOSType_AMDHSA,     // AMD HSA Runtime
        LibLLVMTripleOSType_PS4,
        LibLLVMTripleOSType_ELFIAMCU,
        LibLLVMTripleOSType_TvOS,       // Apple tvOS
        LibLLVMTripleOSType_WatchOS,    // Apple watchOS
        LibLLVMTripleOSType_Mesa3D,
        LibLLVMTripleOSType_Contiki,
        LibLLVMTripleOSType_AMDPAL,     // AMD PAL Runtime
        LibLLVMTripleOSType_HermitCore, // HermitCore Unikernel/Multikernel
        LibLLVMTripleOSType_Hurd,       // GNU/Hurd
        LibLLVMTripleOSType_WASI,       // Experimental WebAssembly OS
        LibLLVMTripleOSType_Emscripten,
        LibLLVMTripleOSType_LastOSType = LibLLVMTripleOSType_Emscripten
    };

    enum LibLLVMTripleEnvironmentType
    {
        LibLLVMTripleEnvironmentType_UnknownEnvironment,

        LibLLVMTripleEnvironmentType_GNU,
        LibLLVMTripleEnvironmentType_GNUABIN32,
        LibLLVMTripleEnvironmentType_GNUABI64,
        LibLLVMTripleEnvironmentType_GNUEABI,
        LibLLVMTripleEnvironmentType_GNUEABIHF,
        LibLLVMTripleEnvironmentType_GNUX32,
        LibLLVMTripleEnvironmentType_CODE16,
        LibLLVMTripleEnvironmentType_EABI,
        LibLLVMTripleEnvironmentType_EABIHF,
        LibLLVMTripleEnvironmentType_Android,
        LibLLVMTripleEnvironmentType_Musl,
        LibLLVMTripleEnvironmentType_MuslEABI,
        LibLLVMTripleEnvironmentType_MuslEABIHF,

        LibLLVMTripleEnvironmentType_MSVC,
        LibLLVMTripleEnvironmentType_Itanium,
        LibLLVMTripleEnvironmentType_Cygnus,
        LibLLVMTripleEnvironmentType_CoreCLR,
        LibLLVMTripleEnvironmentType_Simulator, // Simulator variants of other systems, e.g., Apple's iOS
        LibLLVMTripleEnvironmentType_MacABI, // Mac Catalyst variant of Apple's iOS deployment target.
        LibLLVMTripleEnvironmentType_LastEnvironmentType = LibLLVMTripleEnvironmentType_MacABI
    };

    enum LibLLVMTripleObjectFormatType
    {
        LibLLVMTripleObjectFormatType_UnknownObjectFormat,

        LibLLVMTripleObjectFormatType_COFF,
        LibLLVMTripleObjectFormatType_ELF,
        LibLLVMTripleObjectFormatType_MachO,
        LibLLVMTripleObjectFormatType_Wasm,
        LibLLVMTripleObjectFormatType_XCOFF,
    };

    typedef struct LibLLVMOpaqueTriple* LibLLVMTripleRef;

    LibLLVMTripleRef LibLLVMParseTriple( char const* triple );
    void LibLLVMDisposeTriple( LibLLVMTripleRef triple );

    LLVMBool LibLLVMTripleOpEqual( LibLLVMTripleRef lhs, LibLLVMTripleRef rhs );
    LibLLVMTripleArchType LibLLVMTripleGetArchType( LibLLVMTripleRef triple );
    LibLLVMTripleSubArchType LibLLVMTripleGetSubArchType( LibLLVMTripleRef triple );
    LibLLVMTripleVendorType LibLLVMTripleGetVendorType( LibLLVMTripleRef triple );
    LibLLVMTripleOSType LibLLVMTripleGetOsType( LibLLVMTripleRef triple );
    LLVMBool LibLLVMTripleHasEnvironment( LibLLVMTripleRef triple );
    LibLLVMTripleEnvironmentType LibLLVMTripleGetEnvironmentType( LibLLVMTripleRef triple );
    void LibLLVMTripleGetEnvironmentVersion( LibLLVMTripleRef triple, unsigned* major, unsigned* minor, unsigned* micro );
    LibLLVMTripleObjectFormatType LibLLVMTripleGetObjectFormatType( LibLLVMTripleRef triple );

    // Use LLVMDisposeMessage on return for all of these
    char const* LibLLVMTripleAsString( LibLLVMTripleRef triple, bool normalize );
    char const* LibLLVMTripleGetArchTypeName( LibLLVMTripleArchType type );
    char const* LibLLVMTripleGetArchTypeName( LibLLVMTripleArchType type );
    char const* LibLLVMTripleGetSubArchTypeName( LibLLVMTripleSubArchType type );
    char const* LibLLVMTripleGetVendorTypeName( LibLLVMTripleVendorType vendor );
    char const* LibLLVMTripleGetOsTypeName( LibLLVMTripleOSType osType );
    char const* LibLLVMTripleGetEnvironmentTypeName( LibLLVMTripleEnvironmentType environmentType );
    char const* LibLLVMTripleGetObjectFormatTypeName( LibLLVMTripleObjectFormatType objectFormatType );
#ifdef __cplusplus
}
#endif

#endif
