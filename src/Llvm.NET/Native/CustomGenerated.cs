// <copyright file="CustomGenerated.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#pragma warning disable SA1300 // "StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Generated Interop"

/* Enums types and P/Invoke calls here are extensions to standard LLVM-C APIs
// many are common bindings borrowed from the go bindings (or further extended from them)
// others are unique to Llvm.NET to enable max use of the LLVM libraries in .NET based
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
        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMGetVersionInfo( out LLVMVersionInfo pVersionInfo );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern int LLVMGetValueID( LLVMValueRef @val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMBuildIntCast( LLVMBuilderRef @param0, LLVMValueRef @Val, LLVMTypeRef @DestTy, [MarshalAs( UnmanagedType.Bool )]bool isSigned, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMSetDebugLoc( LLVMValueRef inst, UInt32 line, UInt32 column, LLVMMetadataRef scope );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMSetDILocation( LLVMValueRef inst, LLVMMetadataRef location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocalScope*/ LLVMGetDILocationScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMGetDILocationLine( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMGetDILocationColumn( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocation*/ LLVMGetDILocationInlinedAt( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef /*DILocalScope*/ LLVMDILocationGetInlinedAtScope( LLVMMetadataRef /*DILocation*/ location );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMStatus LLVMVerifyFunctionEx( LLVMValueRef @Fn, LLVMVerifierFailureAction @Action, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")] out string @OutMessages );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddAddressSanitizerFunctionPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddAddressSanitizerModulePass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddThreadSanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddMemorySanitizerPass( LLVMPassManagerRef @PM );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddDataFlowSanitizerPass( LLVMPassManagerRef @PM, [MarshalAs( UnmanagedType.LPStr )] string @ABIListFile );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddModuleFlag( LLVMModuleRef @M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string @name, UInt32 @value );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddModuleFlag( LLVMModuleRef @M, LLVMModFlagBehavior behavior, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMMetadataRef @value );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMNamedMDNodeRef LLVMModuleGetModuleFlagsMetadata( LLVMModuleRef module );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern /*MDNode*/ LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, UInt32 index );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMModuleRef LLVMNamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMGetOrInsertFunction( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMTypeRef functionType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LLVMIsConstantZeroValue( LLVMValueRef @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMRemoveGlobalFromParent( LLVMValueRef @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMConstantAsMetadata( LLVMValueRef @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMMDString2( LLVMContextRef @C, [MarshalAs( UnmanagedType.LPStr )] string @Str, UInt32 @SLen );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMMDNode2( LLVMContextRef @C, out LLVMMetadataRef @MDs, UInt32 @Count );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMTemporaryMDNode( LLVMContextRef @C, out LLVMMetadataRef @MDs, UInt32 @Count );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMAddNamedMetadataOperand2( LLVMModuleRef @M, [MarshalAs( UnmanagedType.LPStr )] string @name, LLVMMetadataRef @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMSetMetadata2( LLVMValueRef @Inst, UInt32 @KindID, LLVMMetadataRef @MD );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMMetadataReplaceAllUsesWith( LLVMMetadataRef @MD, LLVMMetadataRef @New );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMSetCurrentDebugLocation2( LLVMBuilderRef @Bref, UInt32 @Line, UInt32 @Col, LLVMMetadataRef @Scope, LLVMMetadataRef @InlinedAt );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMDIBuilderRef LLVMNewDIBuilder( LLVMModuleRef @m, [MarshalAs(UnmanagedType.Bool)]bool allowUnresolved );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMDIBuilderDestroy( LLVMDIBuilderRef @d );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMDIBuilderFinalize( LLVMDIBuilderRef @d );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateCompileUnit( LLVMDIBuilderRef @D, UInt32 @Language, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir, [MarshalAs( UnmanagedType.LPStr )] string @Producer, int @Optimized, [MarshalAs( UnmanagedType.LPStr )] string @Flags, UInt32 @RuntimeVersion );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateFile( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Line, UInt32 @Column );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Discriminator );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateFunction( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateAutoVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateParameterVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt32 @ArgNo, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateBasicType( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt64 @SizeInBits, UInt32 @Encoding );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreatePointerType( LLVMDIBuilderRef @D, LLVMMetadataRef @PointeeType, UInt64 @SizeInBits, UInt32 @AlignInBits, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateQualifiedType( LLVMDIBuilderRef Dref, UInt32 Tag, LLVMMetadataRef BaseType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateSubroutineType( LLVMDIBuilderRef @D, LLVMMetadataRef @ParameterTypes, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateStructType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @DerivedFrom, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateUnionType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateMemberType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt64 @OffsetInBits, UInt32 @Flags, LLVMMetadataRef @Ty );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateArrayType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateVectorType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateTypedef( LLVMDIBuilderRef @D, LLVMMetadataRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Context );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange( LLVMDIBuilderRef @D, Int64 @Lo, Int64 @Count );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRef @Dref, out Int64 @Addr, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMDIBuilderInsertDeclareAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Storage, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMDIBuilderInsertValueAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Val, UInt64 @Offset, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateEnumerationType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @LineNumber, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @Elements, LLVMMetadataRef @UnderlyingType, [MarshalAs( UnmanagedType.LPStr )]string @UniqueId );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )]string @Name, Int64 @Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression( LLVMDIBuilderRef Dref, LLVMMetadataRef Context, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )]bool isLocalToUnit, LLVMMetadataRef expression, LLVMMetadataRef Decl, UInt32 AlignInBits );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMDIBuilderInsertDeclareBefore( LLVMDIBuilderRef Dref, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMDIBuilderInsertValueBefore( LLVMDIBuilderRef Dref, /*llvm::Value **/LLVMValueRef Val, UInt64 Offset, /*DILocalVariable **/ LLVMMetadataRef VarInfo, /*DIExpression **/ LLVMMetadataRef Expr, /*const DILocation **/ LLVMMetadataRef DL, /*Instruction **/ LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMMetadataAsString( LLVMMetadataRef descriptor );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMMDNodeReplaceAllUsesWith( LLVMMetadataRef oldDescriptor, LLVMMetadataRef newDescriptor );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType( LLVMDIBuilderRef Dref, UInt32 Tag, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Line, UInt32 RuntimeLang, UInt64 SizeInBits, UInt64 AlignInBits, UInt32 Flags );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIBuilderCreateNamespace( LLVMDIBuilderRef Dref, LLVMMetadataRef scope, [MarshalAs( UnmanagedType.LPStr )] string name, [MarshalAs( UnmanagedType.Bool )]bool exportSymbols );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDILocation( LLVMContextRef context, UInt32 Line, UInt32 Column, LLVMMetadataRef scope, LLVMMetadataRef InlinedAt );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetModuleSourceFileName( LLVMModuleRef module );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMSetModuleSourceFileName( LLVMModuleRef module, [MarshalAs(UnmanagedType.LPStr)] string name );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ))]
        internal static extern string LLVMGetModuleName( LLVMModuleRef module );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsTemporary( LLVMMetadataRef M );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsResolved( LLVMMetadataRef M );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsDistinct( LLVMMetadataRef M );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMIsUniqued( LLVMMetadataRef M );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetMDStringText( LLVMMetadataRef M, out UInt32 len );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMGetGlobalAlias( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMValueRef LLVMGetAliasee( LLVMValueRef Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMMDNodeResolveCycles( LLVMMetadataRef M );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMSubProgramDescribes( LLVMMetadataRef subProgram, LLVMValueRef function );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMDITypeGetLine( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 LLVMDITypeGetSizeInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 LLVMDITypeGetAlignInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt64 LLVMDITypeGetOffsetInBits( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMDITypeGetFlags( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDITypeGetScope( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMDITypeGetName( LLVMMetadataRef typeRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDIScopeGetFile( LLVMMetadataRef scope );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMGetArgumentIndex( LLVMValueRef Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetDIFileName( LLVMMetadataRef /*DIFile*/ file );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ) )]
        internal static extern string LLVMGetDIFileDirectory( LLVMMetadataRef /*DIFile*/ file );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMContextRef LLVMGetNodeContext( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataKind LLVMGetMetadataID( LLVMMetadataRef /*Metadata*/ md );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern UInt32 LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, UInt32 index );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMGetOperandNode( LLVMMDOperandRef operand );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMDILocalScopeGetSubProgram( LLVMMetadataRef /*DILocalScope*/ localScope );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMMetadataRef LLVMFunctionGetSubprogram( LLVMValueRef function );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMFunctionSetSubprogram( LLVMValueRef function, LLVMMetadataRef subprogram );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMPassRegistryRef LLVMCreatePassRegistry( );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMPassRegistryDispose( IntPtr hPassRegistry );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMRunPassPipeline( LLVMContextRef context, LLVMModuleRef M, LLVMTargetMachineRef TM, [MarshalAs( UnmanagedType.LPStr )] string passPipeline, LLVMOptVerifierKind VK, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveAssemblyUseListOrder, [MarshalAs( UnmanagedType.Bool )] bool ShouldPreserveBitcodeUseListOrder );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMInitializeCodeGenForOpt( LLVMPassRegistryRef R );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMInitializePassesForLegacyOpt( );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMRunLegacyOptimizer( LLVMModuleRef Mref, LLVMTargetMachineRef TMref );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleRef LLVMParseTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMDisposeTriple( IntPtr triple );

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

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal static extern bool LLVMTripleHasEnvironment( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleEnvironmentType LLVMTripleGetEnvironmentType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMTripleGetEnvironmentVersion( LLVMTripleRef triple, out UInt32 major, out UInt32 minor, out UInt32 micro );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMTripleObjectFormatType LLVMTripleGetObjectFormatType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleAsString( LLVMTripleRef triple, [MarshalAs( UnmanagedType.U1 )]bool normalize );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetArchTypeName( LLVMTripleArchType type );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetSubArchTypeName( LLVMTripleSubArchType type );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetVendorTypeName( LLVMTripleVendorType vendor );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetOsTypeName( LLVMTripleOSType osType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMTripleGetObjectFormatTypeName( LLVMTripleObjectFormatType environmentType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMNormalizeTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        internal delegate bool ComdatIteratorCallback( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMModuleEnumerateComdats( LLVMModuleRef module, ComdatIteratorCallback callback );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, [MarshalAs( UnmanagedType.LPStr )] string name, LLVMComdatSelectionKind kind );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMModuleComdatClear( LLVMModuleRef module );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatRef LLVMGlobalObjectGetComdat( LLVMValueRef Val );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern LLVMComdatSelectionKind LLVMComdatGetKind( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        internal static extern void LLVMComdatSetKind( LLVMComdatRef comdatRef, LLVMComdatSelectionKind kind );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMComdatGetName( LLVMComdatRef comdatRef );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
        internal static extern string LLVMAttributeToString( LLVMAttributeRef attribute );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern LLVMMetadataRef LLVMDIGlobalVarExpGetVariable( LLVMMetadataRef metadataHandle );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMGlobalVariableAddDebugExpression( LLVMValueRef variable, LLVMMetadataRef metadataHandle );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        internal static extern void LLVMExecutionEngineClearGlobalMappingsFromModule( LLVMExecutionEngineRef ee, LLVMModuleRef m );
    }
}
