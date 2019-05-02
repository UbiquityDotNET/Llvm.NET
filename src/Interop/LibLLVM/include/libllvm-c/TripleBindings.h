#ifndef LLVM_TRIPLE_BINDINGS_H
#define LLVM_TRIPLE_BINDINGS_H
#include <llvm-c\Types.h>

#ifdef __cplusplus
extern "C" {
#endif

    enum LLVMTripleArchType
    {
        LlvmTripleArchType_UnknownArch,

        LlvmTripleArchType_arm,            // ARM (little endian): arm, armv.*, xscale
        LlvmTripleArchType_armeb,          // ARM (big endian): armeb
        LlvmTripleArchType_aarch64,        // AArch64 (little endian): aarch64
        LlvmTripleArchType_aarch64_be,     // AArch64 (big endian): aarch64_be
        LlvmTripleArchType_arc,            // ARC: Synopsys ARC
        LlvmTripleArchType_avr,            // AVR: Atmel AVR microcontroller
        LlvmTripleArchType_bpfel,          // eBPF or extended BPF or 64-bit BPF (little endian)
        LlvmTripleArchType_bpfeb,          // eBPF or extended BPF or 64-bit BPF (big endian)
        LlvmTripleArchType_hexagon,        // Hexagon: hexagon
        LlvmTripleArchType_mips,           // MIPS: mips, mipsallegrex, mipsr6
        LlvmTripleArchType_mipsel,         // MIPSEL: mipsel, mipsallegrexe, mipsr6el
        LlvmTripleArchType_mips64,         // MIPS64: mips64, mips64r6, mipsn32, mipsn32r6
        LlvmTripleArchType_mips64el,       // MIPS64EL: mips64el, mips64r6el, mipsn32el, mipsn32r6el
        LlvmTripleArchType_msp430,         // MSP430: msp430
        LlvmTripleArchType_ppc,            // PPC: powerpc
        LlvmTripleArchType_ppc64,          // PPC64: powerpc64, ppu
        LlvmTripleArchType_ppc64le,        // PPC64LE: powerpc64le
        LlvmTripleArchType_r600,           // R600: AMD GPUs HD2XXX - HD6XXX
        LlvmTripleArchType_amdgcn,         // AMDGCN: AMD GCN GPUs
        LlvmTripleArchType_riscv32,        // RISC-V (32-bit): riscv32
        LlvmTripleArchType_riscv64,        // RISC-V (64-bit): riscv64
        LlvmTripleArchType_sparc,          // Sparc: sparc
        LlvmTripleArchType_sparcv9,        // Sparcv9: Sparcv9
        LlvmTripleArchType_sparcel,        // Sparc: (endianness = little). NB: 'Sparcle' is a CPU variant
        LlvmTripleArchType_systemz,        // SystemZ: s390x
        LlvmTripleArchType_tce,            // TCE (http://tce.cs.tut.fi/): tce
        LlvmTripleArchType_tcele,          // TCE little endian (http://tce.cs.tut.fi/): tcele
        LlvmTripleArchType_thumb,          // Thumb (little endian): thumb, thumbv.*
        LlvmTripleArchType_thumbeb,        // Thumb (big endian): thumbeb
        LlvmTripleArchType_x86,            // X86: i[3-9]86
        LlvmTripleArchType_x86_64,         // X86-64: amd64, x86_64
        LlvmTripleArchType_xcore,          // XCore: xcore
        LlvmTripleArchType_nvptx,          // NVPTX: 32-bit
        LlvmTripleArchType_nvptx64,        // NVPTX: 64-bit
        LlvmTripleArchType_le32,           // le32: generic little-endian 32-bit CPU (PNaCl)
        LlvmTripleArchType_le64,           // le64: generic little-endian 64-bit CPU (PNaCl)
        LlvmTripleArchType_amdil,          // AMDIL
        LlvmTripleArchType_amdil64,        // AMDIL with 64-bit pointers
        LlvmTripleArchType_hsail,          // AMD HSAIL
        LlvmTripleArchType_hsail64,        // AMD HSAIL with 64-bit pointers
        LlvmTripleArchType_spir,           // SPIR: standard portable IR for OpenCL 32-bit version
        LlvmTripleArchType_spir64,         // SPIR: standard portable IR for OpenCL 64-bit version
        LlvmTripleArchType_kalimba,        // Kalimba: generic kalimba
        LlvmTripleArchType_shave,          // SHAVE: Movidius vector VLIW processors
        LlvmTripleArchType_lanai,          // Lanai: Lanai 32-bit
        LlvmTripleArchType_wasm32,         // WebAssembly with 32-bit pointers
        LlvmTripleArchType_wasm64,         // WebAssembly with 64-bit pointers
        LlvmTripleArchType_renderscript32, // 32-bit RenderScript
        LlvmTripleArchType_renderscript64, // 64-bit RenderScript
        LlvmTripleArchType_LastArchType = LlvmTripleArchType_renderscript64
    };

    enum LLVMTripleSubArchType
    {
        LlvmTripleSubArchType_NoSubArch,

        LlvmTripleSubArchType_ARMSubArch_v8_5a,
        LlvmTripleSubArchType_ARMSubArch_v8_4a,
        LlvmTripleSubArchType_ARMSubArch_v8_3a,
        LlvmTripleSubArchType_ARMSubArch_v8_2a,
        LlvmTripleSubArchType_ARMSubArch_v8_1a,
        LlvmTripleSubArchType_ARMSubArch_v8,
        LlvmTripleSubArchType_ARMSubArch_v8r,
        LlvmTripleSubArchType_ARMSubArch_v8m_baseline,
        LlvmTripleSubArchType_ARMSubArch_v8m_mainline,
        LlvmTripleSubArchType_ARMSubArch_v7,
        LlvmTripleSubArchType_ARMSubArch_v7em,
        LlvmTripleSubArchType_ARMSubArch_v7m,
        LlvmTripleSubArchType_ARMSubArch_v7s,
        LlvmTripleSubArchType_ARMSubArch_v7k,
        LlvmTripleSubArchType_ARMSubArch_v7ve,
        LlvmTripleSubArchType_ARMSubArch_v6,
        LlvmTripleSubArchType_ARMSubArch_v6m,
        LlvmTripleSubArchType_ARMSubArch_v6k,
        LlvmTripleSubArchType_ARMSubArch_v6t2,
        LlvmTripleSubArchType_ARMSubArch_v5,
        LlvmTripleSubArchType_ARMSubArch_v5te,
        LlvmTripleSubArchType_ARMSubArch_v4t,

        LlvmTripleSubArchType_KalimbaSubArch_v3,
        LlvmTripleSubArchType_KalimbaSubArch_v4,
        LlvmTripleSubArchType_KalimbaSubArch_v5,

        LlvmTripleSubArchType_MipsSubArch_r6
    };

    enum LLVMTripleVendorType
    {
        LlvmTripleVendorType_UnknownVendor,

        LlvmTripleVendorType_Apple,
        LlvmTripleVendorType_PC,
        LlvmTripleVendorType_SCEI,
        LlvmTripleVendorType_BGP,
        LlvmTripleVendorType_BGQ,
        LlvmTripleVendorType_Freescale,
        LlvmTripleVendorType_IBM,
        LlvmTripleVendorType_ImaginationTechnologies,
        LlvmTripleVendorType_MipsTechnologies,
        LlvmTripleVendorType_NVIDIA,
        LlvmTripleVendorType_CSR,
        LlvmTripleVendorType_Myriad,
        LlvmTripleVendorType_AMD,
        LlvmTripleVendorType_Mesa,
        LlvmTripleVendorType_SUSE,
        LlvmTripleVendorType_OpenEmbedded,
        LlvmTripleVendorType_LastVendorType = LlvmTripleVendorType_OpenEmbedded
    };

    enum LLVMTripleOSType
    {
        LlvmTripleOSType_UnknownOS,

        LlvmTripleOSType_Ananas,
        LlvmTripleOSType_CloudABI,
        LlvmTripleOSType_Darwin,
        LlvmTripleOSType_DragonFly,
        LlvmTripleOSType_FreeBSD,
        LlvmTripleOSType_Fuchsia,
        LlvmTripleOSType_IOS,
        LlvmTripleOSType_KFreeBSD,
        LlvmTripleOSType_Linux,
        LlvmTripleOSType_Lv2,        // PS3
        LlvmTripleOSType_MacOSX,
        LlvmTripleOSType_NetBSD,
        LlvmTripleOSType_OpenBSD,
        LlvmTripleOSType_Solaris,
        LlvmTripleOSType_Win32,
        LlvmTripleOSType_Haiku,
        LlvmTripleOSType_Minix,
        LlvmTripleOSType_RTEMS,
        LlvmTripleOSType_NaCl,       // Native Client
        LlvmTripleOSType_CNK,        // BG/P Compute-Node Kernel
        LlvmTripleOSType_AIX,
        LlvmTripleOSType_CUDA,       // NVIDIA CUDA
        LlvmTripleOSType_NVCL,       // NVIDIA OpenCL
        LlvmTripleOSType_AMDHSA,     // AMD HSA Runtime
        LlvmTripleOSType_PS4,
        LlvmTripleOSType_ELFIAMCU,
        LlvmTripleOSType_TvOS,       // Apple tvOS
        LlvmTripleOSType_WatchOS,    // Apple watchOS
        LlvmTripleOSType_Mesa3D,
        LlvmTripleOSType_Contiki,
        LlvmTripleOSType_AMDPAL,     // AMD PAL Runtime
        LlvmTripleOSType_HermitCore, // HermitCore Unikernel/Multikernel
        LlvmTripleOSType_Hurd,       // GNU/Hurd
        LlvmTripleOSType_WASI,       // Experimental WebAssembly OS
        LlvmTripleOSType_LastOSType = LlvmTripleOSType_WASI
    };

    enum LLVMTripleEnvironmentType
    {
        LlvmTripleEnvironmentType_UnknownEnvironment,

        LlvmTripleEnvironmentType_GNU,
        LlvmTripleEnvironmentType_GNUABIN32,
        LlvmTripleEnvironmentType_GNUABI64,
        LlvmTripleEnvironmentType_GNUEABI,
        LlvmTripleEnvironmentType_GNUEABIHF,
        LlvmTripleEnvironmentType_GNUX32,
        LlvmTripleEnvironmentType_CODE16,
        LlvmTripleEnvironmentType_EABI,
        LlvmTripleEnvironmentType_EABIHF,
        LlvmTripleEnvironmentType_Android,
        LlvmTripleEnvironmentType_Musl,
        LlvmTripleEnvironmentType_MuslEABI,
        LlvmTripleEnvironmentType_MuslEABIHF,

        LlvmTripleEnvironmentType_MSVC,
        LlvmTripleEnvironmentType_Itanium,
        LlvmTripleEnvironmentType_Cygnus,
        LlvmTripleEnvironmentType_CoreCLR,
        LlvmTripleEnvironmentType_Simulator,  // Simulator variants of other systems, e.g., Apple's iOS
        LlvmTripleEnvironmentType_LastEnvironmentType = LlvmTripleEnvironmentType_Simulator
    };

    enum LLVMTripleObjectFormatType
    {
        LlvmTripleObjectFormatType_UnknownObjectFormat,
        LlvmTripleObjectFormatType_COFF,
        LlvmTripleObjectFormatType_ELF,
        LlvmTripleObjectFormatType_MachO,
        LlvmTripleObjectFormatType_Wasm,
    };

    typedef struct LLVMOpaqueTriple* LLVMTripleRef;

    LLVMTripleRef LLVMParseTriple( char const* triple );
    void LLVMDisposeTriple( LLVMTripleRef triple );
    LLVMTripleRef LLVMGetHostTriple( );

    LLVMBool LLVMTripleOpEqual( LLVMTripleRef lhs, LLVMTripleRef rhs );
    LLVMTripleArchType LLVMTripleGetArchType( LLVMTripleRef triple );
    LLVMTripleSubArchType LLVMTripleGetSubArchType( LLVMTripleRef triple );
    LLVMTripleVendorType LLVMTripleGetVendorType( LLVMTripleRef triple );
    LLVMTripleOSType LLVMTripleGetOsType( LLVMTripleRef triple );
    LLVMBool LLVMTripleHasEnvironment( LLVMTripleRef triple );
    LLVMTripleEnvironmentType LLVMTripleGetEnvironmentType( LLVMTripleRef triple );
    void LLVMTripleGetEnvironmentVersion( LLVMTripleRef triple, unsigned* major, unsigned* minor, unsigned* micro );
    LLVMTripleObjectFormatType LLVMTripleGetObjectFormatType( LLVMTripleRef triple );

    // Use LLVMDisposeMessage on return for all of these
    char const* LLVMTripleAsString( LLVMTripleRef triple, bool normalize );
    char const* LLVMTripleGetArchTypeName( LLVMTripleArchType type );
    char const* LLVMTripleGetArchTypeName( LLVMTripleArchType type );
    char const* LLVMTripleGetSubArchTypeName( LLVMTripleSubArchType type );
    char const* LLVMTripleGetVendorTypeName( LLVMTripleVendorType vendor );
    char const* LLVMTripleGetOsTypeName( LLVMTripleOSType osType );
    char const* LLVMTripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType );
    char const* LLVMTripleGetObjectFormatTypeName( LLVMTripleObjectFormatType objectFormatType );
    char const* LLVMNormalizeTriple( char const* triple );
#ifdef __cplusplus
}
#endif

#endif
