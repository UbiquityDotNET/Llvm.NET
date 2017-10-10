// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#pragma warning disable SA1300 // "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Generated Interop"

/* Enums types and P/Invoke calls here are extensions to standard LLVM-C APIs
// many are cmoon bindings borrowed from the go bindings (or further extended from them)
// others are uinique to Llvm.NET to enable max use of the LLVM libraries in .NET based
// code.
*/

namespace Llvm.NET.Native
{
    [SuppressMessage( "Style", "IDE1006:Naming Styles", Justification = "Generated code relies on this to match C++" )]
    [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed." )]
    internal partial struct size_t
    {
        public static explicit operator size_t(int size) => new size_t( ( IntPtr )size );

        public static implicit operator int(size_t size) => size.Pointer.ToInt32( );

        public static implicit operator long( size_t size ) => size.Pointer.ToInt64( );

        internal size_t( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal IntPtr Pointer { get; }
    }

    internal partial struct LLVMMDOperandRef
    {
        internal LLVMMDOperandRef(IntPtr pointer)
        {
            Pointer = pointer;
        }

        internal readonly IntPtr Pointer;
    }

    internal partial struct LLVMNamedMDNodeRef
    {
        internal LLVMNamedMDNodeRef(IntPtr pointer)
        {
            Pointer = pointer;
        }

        internal readonly IntPtr Pointer;
    }

    internal partial struct LLVMComdatRef
    {
        internal LLVMComdatRef( IntPtr pointer )
        {
            Pointer = pointer;
        }

        internal readonly IntPtr Pointer;
    }

#pragma warning disable CA1008 // Enums should have zero value.
    internal enum LLVMModFlagBehavior
    {
        @Error = 1,
        @Warning = 2,
        @Require = 3,
        @Override = 4,
        @Append = 5,
        @AppendUnique = 6,
        @ModFlagBehaviorFirstVal = Error,
        @ModFlagBehaviorLastVal = AppendUnique
    }

    internal enum LLVMDwarfTag : ushort
    {
        ArrayType = 0x01,
        ClassType = 0x02,
        EntryPoint = 0x03,
        EnumerationType = 0x04,
        FormalParameter = 0x05,
        ImportedDeclaration = 0x08,
        Label = 0x0a,
        LexicalBlock = 0x0b,
        Member = 0x0d,
        PointerType = 0x0f,
        ReferenceType = 0x10,
        CompileUnit = 0x11,
        StringType = 0x12,
        StructureType = 0x13,
        SubroutineType = 0x15,
        TypeDef = 0x16,
        UnionType = 0x17,
        UnspecifiedParameters = 0x18,
        Variant = 0x19,
        CommonBlock = 0x1a,
        CommonInclusion = 0x1b,
        Inheritance = 0x1c,
        InlinedSubroutine = 0x1d,
        Module = 0x1e,
        PtrToMemberType = 0x1f,
        SetType = 0x20,
        SubrangeType = 0x21,
        WithStatement = 0x22,
        AccessDeclaration = 0x23,
        BaseType = 0x24,
        CatchBlock = 0x25,
        ConstType = 0x26,
        Constant = 0x27,
        Enumerator = 0x28,
        FileType = 0x29,
        Friend = 0x2a,
        NameList = 0x2b,
        NameListItem = 0x2c,
        PackedType = 0x2d,
        SubProgram = 0x2e,
        TemplateTypeParameter = 0x2f,
        TemplateValueParameter = 0x30,
        ThrownType = 0x31,
        TryBlock = 0x32,
        VariantPart = 0x33,
        Variable = 0x34,
        VolatileType = 0x35,
        DwarfProcedure = 0x36,
        RestrictType = 0x37,
        InterfaceType = 0x38,
        Namespace = 0x39,
        ImportedModule = 0x3a,
        UnspecifiedType = 0x3b,
        PartialUnit = 0x3c,
        ImportedUnit = 0x3d,
        Condition = 0x3f,
        SharedType = 0x40,
        TypeUnit = 0x41,
        RValueReferenceType = 0x42,
        TemplateAlias = 0x43,

        // New in DWARF 5:
        CoArrayType = 0x44,
        GenericSubrange = 0x45,
        DynamicType = 0x46,

        MipsLoop = 0x4081,
        FormatLabel = 0x4101,
        FunctionTemplate = 0x4102,
        ClassTemplate = 0x4103,
        GnuTemplateTemplateParam = 0x4106,
        GnuTemplateParameterPack = 0x4107,
        GnuFormalParameterPack = 0x4108,
        LoUser = 0x4080,
        AppleProperty = 0x4200,
        HiUser = 0xffff
    }
#pragma warning restore CA1008 // Enums should have zero value.

    internal enum LLVMMetadataKind
    {
        MDTuple,
        DILocation,
        GenericDINode,
        DISubrange,
        DIEnumerator,
        DIBasicType,
        DIDerivedType,
        DICompositeType,
        DISubroutineType,
        DIFile,
        DICompileUnit,
        DISubprogram,
        DILexicalBlock,
        DILexicalBlockFile,
        DINamespace,
        DIModule,
        DITemplateTypeParameter,
        DITemplateValueParameter,
        DIGlobalVariable,
        DILocalVariable,
        DIExpression,
        DIObjCProperty,
        DIImportedEntity,
        ConstantAsMetadata,
        LocalAsMetadata,
        MDString
    }

    internal enum LLVMOptVerifierKind
    {
        None,
        VerifyInAndOut,
        VerifyEachPass
    }

    internal enum LLVMTripleArchType
    {
        UnknownArch,
        arm,            // ARM (little endian): arm, armv.*, xscale
        armeb,          // ARM (big endian): armeb
        aarch64,        // AArch64 (little endian): aarch64
        aarch64_be,     // AArch64 (big endian): aarch64_be
        avr,            // AVR: Atmel AVR microcontroller
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
        LastVendorType = Mesa
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
        Bitrig,
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
        LastOSType = Contiki
    }

    internal enum LLVMTripleEnvironmentType
    {
        UnknownEnvironment,
        GNU,
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
        LastEnvironmentType = OpenCL
    }

    internal enum LLVMTripleObjectFormatType
    {
        UnknownObjectFormat,
        COFF,
        ELF,
        MachO,
        Wasm
    }

    internal enum LLVMComdatSelectionKind
    {
        ANY,
        EXACTMATCH,
        LARGEST,
        NODUPLICATES,
        SAMESIZE
    }

    [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Mapping to interop C based API" )]
    internal static partial class NativeMethods
    {
        [DllImport( LibraryPath, EntryPoint = "LLVMGetVersionInfo", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void GetVersionInfo( out LLVMVersionInfo pVersionInfo );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetValueID", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern int GetValueID( LLVMValueRef @val );

        [DllImport( LibraryPath, EntryPoint = "LLVMBuildIntCast2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef BuildIntCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.Bool )]bool isSigned, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDebugLoc", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void SetDebugLoc( LLVMValueRef inst, UInt32 line, UInt32 column, LLVMMetadataRef scope );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetDILocation", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void SetDILocation( LLVMValueRef inst, LLVMMetadataRef location );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDILocationScope", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocalScope*/ GetDILocationScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDILocationLine", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 GetDILocationLine( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDILocationColumn", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 GetDILocationColumn( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDILocationInlinedAt", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocation*/ GetDILocationInlinedAt( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, EntryPoint = "LLVMDILocationGetInlinedAtScope", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocalScope*/ DILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, EntryPoint = "LLVMVerifyFunctionEx", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMStatus VerifyFunctionEx( LLVMValueRef @Fn, LLVMVerifierFailureAction @Action, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")] out string @OutMessages );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAddressSanitizerFunctionPass", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddAddressSanitizerFunctionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddAddressSanitizerModulePass", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddAddressSanitizerModulePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddThreadSanitizerPass", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddThreadSanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddMemorySanitizerPass", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddMemorySanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddDataFlowSanitizerPass", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddDataFlowSanitizerPass( LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.LPStr )] string @ABIListFile );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddModuleFlag", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddModuleFlag( LLVMModuleRef @M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string @name, UInt32 @value );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddModuleFlagMetadata", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddModuleFlag( LLVMModuleRef @M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMMetadataRef @value );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleGetModuleFlagsMetadata", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMNamedMDNodeRef ModuleGetModuleFlagsMetadata( LLVMModuleRef module );

        [DllImport( LibraryPath, EntryPoint = "LLVMNamedMDNodeGetNumOperands", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 NamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, EntryPoint = "LLVMNamedMDNodeGetOperand", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern /*MDNode*/ LLVMMetadataRef NamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index );

        [DllImport( LibraryPath, EntryPoint = "LLVMNamedMDNodeGetParentModule", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMModuleRef NamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOrInsertFunction", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef GetOrInsertFunction( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMTypeRef functionType );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsConstantZeroValue", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsConstantZeroValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMRemoveGlobalFromParent", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void RemoveGlobalFromParent( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMConstantAsMetadata", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef ConstantAsMetadata( LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDString2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef MDString2( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, UInt32 @SLen );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNode2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef MDNode2( LLVMContextRef @C, out LLVMMetadataRef @MDs, UInt32 @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMTemporaryMDNode", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef TemporaryMDNode( LLVMContextRef @C, out LLVMMetadataRef @MDs, UInt32 @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMAddNamedMetadataOperand2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void AddNamedMetadataOperand2( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMMetadataRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetMetadata2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void SetMetadata2( LLVMValueRef @Inst, UInt32 @KindID, LLVMMetadataRef @MD );

        [DllImport( LibraryPath, EntryPoint = "LLVMMetadataReplaceAllUsesWith", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void MetadataReplaceAllUsesWith( LLVMMetadataRef @MD, LLVMMetadataRef @New );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetCurrentDebugLocation2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void SetCurrentDebugLocation2( LLVMBuilderRef @Bref, UInt32 @Line, UInt32 @Col, LLVMMetadataRef @Scope, LLVMMetadataRef @InlinedAt );

        [DllImport( LibraryPath, EntryPoint = "LLVMNewDIBuilder", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMDIBuilderRef NewDIBuilder( LLVMModuleRef @m, [MarshalAs(UnmanagedType.Bool)]bool allowUnresolved );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderDestroy", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void DIBuilderDestroy( LLVMDIBuilderRef @d );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderFinalize", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void DIBuilderFinalize( LLVMDIBuilderRef @d );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateCompileUnit", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateCompileUnit( LLVMDIBuilderRef @D, UInt32 @Language, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir, [MarshalAs( UnmanagedType.LPStr )] string @Producer, int @Optimized, [MarshalAs( UnmanagedType.LPStr )] string @Flags, UInt32 @RuntimeVersion );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateFile", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateFile( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateLexicalBlock", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateLexicalBlock( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Line, UInt32 @Column );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateLexicalBlockFile", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateLexicalBlockFile( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Discriminator );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateFunction", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateFunction( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateTempFunctionFwdDecl", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateAutoVariable", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateAutoVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateParameterVariable", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateParameterVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt32 @ArgNo, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateBasicType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateBasicType( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt64 @SizeInBits, UInt32 @Encoding );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreatePointerType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreatePointerType( LLVMDIBuilderRef @D, LLVMMetadataRef @PointeeType, UInt64 @SizeInBits, UInt32 @AlignInBits, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateQualifiedType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateQualifiedType( LLVMDIBuilderRef Dref, UInt32 Tag, LLVMMetadataRef BaseType );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateSubroutineType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateSubroutineType( LLVMDIBuilderRef @D, LLVMMetadataRef @ParameterTypes, UInt32 @Flags );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateStructType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateStructType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @DerivedFrom, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateUnionType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateUnionType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateMemberType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateMemberType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt64 @OffsetInBits, UInt32 @Flags, LLVMMetadataRef @Ty );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateArrayType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateArrayType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateVectorType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateVectorType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateTypedef", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateTypedef( LLVMDIBuilderRef @D, LLVMMetadataRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Context );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderGetOrCreateSubrange", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderGetOrCreateSubrange( LLVMDIBuilderRef @D, Int64 @Lo, Int64 @Count );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderGetOrCreateArray", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderGetOrCreateArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderGetOrCreateTypeArray", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderGetOrCreateTypeArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateExpression", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateExpression( LLVMDIBuilderRef @Dref, out Int64 @Addr, UInt64 @Length );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderInsertDeclareAtEnd", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef DIBuilderInsertDeclareAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Storage, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderInsertValueAtEnd", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef DIBuilderInsertValueAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Val, UInt64 @Offset, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateEnumerationType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateEnumerationType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @LineNumber, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @Elements, LLVMMetadataRef @UnderlyingType, [MarshalAs( UnmanagedType.LPStr )]string @UniqueId );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateEnumeratorValue", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateEnumeratorValue( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )]string @Name, Int64 @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIDescriptorGetTag", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMDwarfTag DIDescriptorGetTag( LLVMMetadataRef descriptor );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateGlobalVariableExpression", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateGlobalVariableExpression( LLVMDIBuilderRef Dref, LLVMMetadataRef Context, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )]bool isLocalToUnit, LLVMMetadataRef expression, LLVMMetadataRef Decl, UInt32 AlignInBits );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderInsertDeclareBefore", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef DIBuilderInsertDeclareBefore( LLVMDIBuilderRef Dref, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderInsertValueBefore", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef DIBuilderInsertValueBefore( LLVMDIBuilderRef Dref, /*llvm::Value **/LLVMValueRef Val, UInt64 Offset, /*DILocalVariable **/ LLVMMetadataRef VarInfo, /*DIExpression **/ LLVMMetadataRef Expr, /*const DILocation **/ LLVMMetadataRef DL, /*Instruction **/ LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, EntryPoint = "LLVMMetadataAsString", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string MetadataAsString( LLVMMetadataRef descriptor );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNodeReplaceAllUsesWith", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void MDNodeReplaceAllUsesWith( LLVMMetadataRef oldDescriptor, LLVMMetadataRef newDescriptor );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateReplaceableCompositeType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateReplaceableCompositeType( LLVMDIBuilderRef Dref, UInt32 Tag, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Line, UInt32 RuntimeLang, UInt64 SizeInBits, UInt64 AlignInBits, UInt32 Flags );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIBuilderCreateNamespace", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIBuilderCreateNamespace( LLVMDIBuilderRef Dref, LLVMMetadataRef scope, [MarshalAs( UnmanagedType.LPStr )] string name, [MarshalAs( UnmanagedType.Bool )]bool exportSymbols );

        [DllImport( LibraryPath, EntryPoint = "LLVMDILocation", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DILocation( LLVMContextRef context, UInt32 Line, UInt32 Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleSourceFileName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetModuleSourceFileName( LLVMModuleRef module );

        [DllImport( LibraryPath, EntryPoint = "LLVMSetModuleSourceFileName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void SetModuleSourceFileName( LLVMModuleRef module, [MarshalAs(UnmanagedType.LPStr)] string name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetModuleName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ))]
        internal static extern string GetModuleName( LLVMModuleRef module );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsTemporary", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsTemporary( LLVMMetadataRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsResolved", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsResolved( LLVMMetadataRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsDistinct", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsDistinct( LLVMMetadataRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMIsUniqued", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool IsUniqued( LLVMMetadataRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMDStringText", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetMDStringText( LLVMMetadataRef M, out UInt32 len );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetGlobalAlias", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef GetGlobalAlias( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetAliasee", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef GetAliasee( LLVMValueRef Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNodeResolveCycles", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void MDNodeResolveCycles( LLVMMetadataRef M );

        [DllImport( LibraryPath, EntryPoint = "LLVMSubProgramDescribes", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool SubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef function );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetLine", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 DITypeGetLine( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetSizeInBits", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 DITypeGetSizeInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetAlignInBits", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 DITypeGetAlignInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetOffsetInBits", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 DITypeGetOffsetInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetFlags", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 DITypeGetFlags( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetScope", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DITypeGetScope( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDITypeGetName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string DITypeGetName( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIScopeGetFile", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DIScopeGetFile( LLVMMetadataRef scope );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetArgumentIndex", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 GetArgumentIndex( LLVMValueRef Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDIFileName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetDIFileName( LLVMMetadataRef /*DIFile*/ file );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetDIFileDirectory", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string GetDIFileDirectory( LLVMMetadataRef /*DIFile*/ file );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNodeContext", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMContextRef GetNodeContext( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetMetadataID", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataKind GetMetadataID( LLVMMetadataRef /*Metadata*/ md );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNodeGetNumOperands", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 MDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, EntryPoint = "LLVMMDNodeGetOperand", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMDOperandRef MDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, UInt32 index );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetOperandNode", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef GetOperandNode( LLVMMDOperandRef operand );

        [DllImport( LibraryPath, EntryPoint = "LLVMDILocalScopeGetSubProgram", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef DILocalScopeGetSubProgram( LLVMMetadataRef /*DILocalScope*/ localScope );

        [DllImport( LibraryPath, EntryPoint = "LLVMFunctionGetSubprogram", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef FunctionGetSubprogram( LLVMValueRef function );

        [DllImport( LibraryPath, EntryPoint = "LLVMFunctionSetSubprogram", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void FunctionSetSubprogram( LLVMValueRef function, LLVMMetadataRef subprogram );

        [DllImport( LibraryPath, EntryPoint = "LLVMCreatePassRegistry", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMPassRegistryRef CreatePassRegistry( );

        [DllImport( LibraryPath, EntryPoint = "LLVMPassRegistryDispose", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void PassRegistryDispose( IntPtr hPassRegistry );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunPassPipeline", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool RunPassPipeline( LLVMContextRef context, LLVMModuleRef M, LLVMTargetMachineRef TM, [MarshalAs( UnmanagedType.LPStr )] string passPipeline, LLVMOptVerifierKind VK, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveAssemblyUseListOrder, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveBitcodeUseListOrder );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializeCodeGenForOpt", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void InitializeCodeGenForOpt( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, EntryPoint = "LLVMInitializePassesForLegacyOpt", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void InitializePassesForLegacyOpt( );

        [DllImport( LibraryPath, EntryPoint = "LLVMRunLegacyOptimizer", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void RunLegacyOptimizer( LLVMModuleRef Mref, LLVMTargetMachineRef TMref );

        [DllImport( LibraryPath, EntryPoint = "LLVMParseTriple", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleRef ParseTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMDisposeTriple", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void DisposeTriple( IntPtr triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleOpEqual", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TripleOpEqual( LLVMTripleRef lhs, LLVMTripleRef rhs );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetArchType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleArchType TripleGetArchType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetSubArchType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleSubArchType TripleGetSubArchType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetVendorType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleVendorType TripleGetVendorType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetOsType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleOSType TripleGetOsType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleHasEnvironment", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool TripleHasEnvironment( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetEnvironmentType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleEnvironmentType TripleGetEnvironmentType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetEnvironmentVersion", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void TripleGetEnvironmentVersion( LLVMTripleRef triple, out UInt32 major, out UInt32 minor, out UInt32 micro );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetObjectFormatType", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleObjectFormatType TripleGetObjectFormatType( LLVMTripleRef triple );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleAsString", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleAsString( LLVMTripleRef triple, [MarshalAs( UnmanagedType.U1 )]bool normalize );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetArchTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetArchTypeName( LLVMTripleArchType type );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetSubArchTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetSubArchTypeName( LLVMTripleSubArchType type );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetVendorTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetVendorTypeName( LLVMTripleVendorType vendor );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetOsTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetOsTypeName( LLVMTripleOSType osType );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetEnvironmentTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType );

        [DllImport( LibraryPath, EntryPoint = "LLVMTripleGetObjectFormatTypeName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string TripleGetObjectFormatTypeName( LLVMTripleObjectFormatType environmentType );

        [DllImport( LibraryPath, EntryPoint = "LLVMNormalizeTriple", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string NormalizeTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal delegate bool ComdatIteratorCallback( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleEnumerateComdats", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void ModuleEnumerateComdats( LLVMModuleRef module, ComdatIteratorCallback callback );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleInsertOrUpdateComdat", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatRef ModuleInsertOrUpdateComdat( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMComdatSelectionKind kind );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleComdatRemove", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void ModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMModuleComdatClear", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void ModuleComdatClear( LLVMModuleRef module );

        [DllImport( LibraryPath, EntryPoint = "LLVMGlobalObjectGetComdat", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatRef GlobalObjectGetComdat( LLVMValueRef Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGlobalObjectSetComdat", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void GlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMComdatGetKind", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatSelectionKind ComdatGetKind( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMComdatSetKind", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void ComdatSetKind( LLVMComdatRef comdatRef, LLVMComdatSelectionKind kind );

        [DllImport( LibraryPath, EntryPoint = "LLVMComdatGetName", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string ComdatGetName( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, EntryPoint = "LLVMAttributeToString", CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string AttributeToString( LLVMAttributeRef attribute );

        [DllImport( LibraryPath, EntryPoint = "LLVMDIGlobalVarExpGetVariable", CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMetadataRef DIGlobalVarExpGetVariable( LLVMMetadataRef metadataHandle );

        [DllImport( LibraryPath, EntryPoint = "LLVMGlobalVariableAddDebugExpression", CallingConvention = CallingConvention.Cdecl )]
        internal static extern void GlobalVariableAddDebugExpression( LLVMValueRef variable, LLVMMetadataRef metadataHandle );
    }
}
