// -----------------------------------------------------------------------
// <copyright file="DebugInfo.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    [Flags]
    [SuppressMessage( "StyleCop.CSharp.LayoutRules", "SA1515:Single-line comment should be preceded by blank line", Justification = "Comments document reserved value 'holes'" )]
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "The name 'Zero' has meaning in LLVM terms; 'None' does not." )]
    [SuppressMessage( "Design", "CA1069:Enums values should not be duplicated", Justification = "Values are defined by ABI; [Mutually exclusive but have different meanings in context]" )]
    public enum LLVMDIOption
        : Int32
    {
        Zero = 0,
        Private = 1,
        Protected = 2,
        Public = 3,
        FwdDecl = 4,
        AppleBlock = 8,
        // ReservedBit4 = 16,
        Virtual = 32,
        Artificial = 64,
        Explicit = 128,
        Prototyped = 256,
        ObjcClassComplete = 512,
        ObjectPointer = 1024,
        Vector = 2048,
        StaticMember = 4096,
        LValueReference = 8192,
        RValueReference = 16384,
        // Reserved = 32768,
        SingleInheritance = 65536,
        MultipleInheritance = 131072,
        VirtualInheritance = 196608,
        IntroducedVirtual = 262144,
        BitField = 524288,
        NoReturn = 1048576,
        TypePassByValue = 4194304,
        TypePassByReference = 8388608,
        EnumClass = 16777216,
        FixedEnum = 16777216,
        Thunk = 33554432,
        NonTrivial = 67108864,
        BigEndian = 134217728,
        LittleEndian = 268435456,
        IndirectVirtualBase = 36,
        Accessibility = 3,
        PtrToMemberRep = 196608,
    }

    public enum LLVMDWARFSourceLanguage
        : Int32
    {
        C89 = 0,
        C = 1,
        Ada83 = 2,
        C_plus_plus = 3,
        Cobol74 = 4,
        Cobol85 = 5,
        Fortran77 = 6,
        Fortran90 = 7,
        Pascal83 = 8,
        Modula2 = 9,
        Java = 10,
        C99 = 11,
        Ada95 = 12,
        Fortran95 = 13,
        PLI = 14,
        ObjC = 15,
        ObjC_plus_plus = 16,
        UPC = 17,
        D = 18,
        Python = 19,
        OpenCL = 20,
        Go = 21,
        Modula3 = 22,
        Haskell = 23,
        C_plus_plus_03 = 24,
        C_plus_plus_11 = 25,
        OCaml = 26,
        Rust = 27,
        C11 = 28,
        Swift = 29,
        Julia = 30,
        Dylan = 31,
        C_plus_plus_14 = 32,
        Fortran03 = 33,
        Fortran08 = 34,
        RenderScript = 35,
        BLISS = 36,
        Kotlin = 37,
        Zig = 38,
        Crystal = 39,
        C_plus_plus_17 = 40,
        C_plus_plus_20 = 41,
        C17 = 42,
        Fortran18 = 43,
        Ada2005 = 44,
        Ada2012 = 45,
        HIP = 46,
        Assembly = 47,
        C_sharp = 48,
        Mojo = 49,
        GLSL = 50,
        GLSL_ES = 51,
        HLSL = 52,
        OpenCL_CPP = 53,
        CPP_for_OpenCL = 54,
        SYCL = 55,
        Ruby = 56,
        Move = 57,
        Hylo = 58,
        Metal = 59,
        Mips_Assembler = 60,
        GOOGLE_RenderScript = 61,
        BORLAND_Delphi = 62,
    }

    public enum LLVMDWARFEmissionKind
        : Int32
    {
        None = 0,
        Full = 1,
        LineTablesOnly = 2,
    }

    public enum LLVMMetadataKind
        : Int32
    {
        MDString = 0,
        ConstantAsMetadata = 1,
        LocalAsMetadata = 2,
        DistinctMDOperandPlaceholder = 3,
        MDTuple = 4,
        DILocation = 5,
        DIExpression = 6,
        DIGlobalVariableExpression = 7,
        GenericDINode = 8,
        DISubrange = 9,
        DIEnumerator = 10,
        DIBasicType = 11,
        DIDerivedType = 12,
        DICompositeType = 13,
        DISubroutineType = 14,
        DIFile = 15,
        DICompileUnit = 16,
        DISubprogram = 17,
        DILexicalBlock = 18,
        DILexicalBlockFile = 19,
        DINamespace = 20,
        DIModule = 21,
        DITemplateTypeParameter = 22,
        DITemplateValueParameter = 23,
        DIGlobalVariable = 24,
        DILocalVariable = 25,
        DILabel = 26,
        DIObjCProperty = 27,
        DIImportedEntity = 28,
        DIMacro = 29,
        DIMacroFile = 30,
        DICommonBlock = 31,
        DIStringType = 32,
        DIGenericSubrange = 33,
        DIArgList = 34,
        DIAssignID = 35,
    }

    public enum LLVMDWARFMacinfoRecordType
        : Int32
    {
        Invalid = 0,
        Define = 1,
        Macro = 2,
        StartFile = 3,
        EndFile = 4,
        VendorExt = 255,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDebugMetadataVersion();

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetModuleDebugMetadataVersion(LLVMModuleRef Module);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMStripModuleDebugInfo(LLVMModuleRef Module);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilderDisallowUnresolved(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilder(LLVMModuleRef M);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalize(LLVMDIBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalizeSubprogram(LLVMDIBuilderRef Builder, LLVMMetadataRef Subprogram);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateCompileUnit(LLVMDIBuilderRef Builder, LLVMDWARFSourceLanguage Lang, LLVMMetadataRef FileRef, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Producer, size_t ProducerLen, [MarshalAs( UnmanagedType.Bool )] bool isOptimized, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Flags, size_t FlagsLen, uint RuntimeVer, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string SplitName, size_t SplitNameLen, LLVMDWARFEmissionKind Kind, uint DWOId, [MarshalAs( UnmanagedType.Bool )] bool SplitDebugInlining, [MarshalAs( UnmanagedType.Bool )] bool DebugInfoForProfiling, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string SysRoot, size_t SysRootLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string SDK, size_t SDKLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFile(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Filename, size_t FilenameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Directory, size_t DirectoryLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateModule(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentScope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string ConfigMacros, size_t ConfigMacrosLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string IncludePath, size_t IncludePathLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string APINotesFile, size_t APINotesFileLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNameSpace(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentScope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalAs( UnmanagedType.Bool )] bool ExportSymbols);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFunction(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string LinkageName, size_t LinkageNameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool IsLocalToUnit, [MarshalAs( UnmanagedType.Bool )] bool IsDefinition, uint ScopeLine, LLVMDIOption Flags, [MarshalAs( UnmanagedType.Bool )] bool IsOptimized);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint Column);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Discriminator);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromNamespace(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef NS, LLVMMetadataRef File, uint Line);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromAlias(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef ImportedEntity, LLVMMetadataRef File, uint Line, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromModule(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef M, LLVMMetadataRef File, uint Line, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedDeclaration(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef Decl, LLVMMetadataRef File, uint Line, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateDebugLocation(LLVMContextRef Ctx, uint Line, uint Column, LLVMMetadataRef Scope, LLVMMetadataRef InlinedAt);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetLine(LLVMMetadataRef Location);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetColumn(LLVMMetadataRef Location);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetScope(LLVMMetadataRef Location);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetInlinedAt(LLVMMetadataRef Location);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIScopeGetFile(LLVMMetadataRef Scope);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMDIFileGetDirectory(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMDIFileGetFilename(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMDIFileGetSource(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray(LLVMDIBuilderRef Builder, [In] LLVMMetadataRef[] Data, size_t NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateSubroutineType(LLVMDIBuilderRef Builder, LLVMMetadataRef File, [In] LLVMMetadataRef[] ParameterTypes, uint NumParameterTypes, LLVMDIOption Flags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMacro(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentMacroFile, uint Line, LLVMDWARFMacinfoRecordType RecordType, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Value, size_t ValueLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempMacroFile(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentMacroFile, uint Line, LLVMMetadataRef File);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerator(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, Int64 Value, [MarshalAs( UnmanagedType.Bool )] bool IsUnsigned);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerationType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, [In] LLVMMetadataRef[] Elements, uint NumElements, LLVMMetadataRef ClassTy);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnionType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIOption Flags, [In] LLVMMetadataRef[] Elements, uint NumElements, uint RunTimeLang, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string UniqueId, size_t UniqueIdLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArrayType(LLVMDIBuilderRef Builder, UInt64 Size, UInt32 AlignInBits, LLVMMetadataRef Ty, [In] LLVMMetadataRef[] Subscripts, uint NumSubscripts);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateVectorType(LLVMDIBuilderRef Builder, UInt64 Size, UInt32 AlignInBits, LLVMMetadataRef Ty, [In] LLVMMetadataRef[] Subscripts, uint NumSubscripts);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnspecifiedType(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBasicType(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, UInt64 SizeInBits, UInt32 Encoding, LLVMDIOption Flags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreatePointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef PointeeTy, UInt64 SizeInBits, UInt32 AlignInBits, uint AddressSpace, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStructType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIOption Flags, LLVMMetadataRef DerivedFrom, [In] LLVMMetadataRef[] Elements, uint NumElements, uint RunTimeLang, LLVMMetadataRef VTableHolder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string UniqueId, size_t UniqueIdLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIOption Flags, LLVMMetadataRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStaticMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, LLVMMetadataRef Type, LLVMDIOption Flags, LLVMValueRef ConstantVal, UInt32 AlignInBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberPointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef PointeeType, LLVMMetadataRef ClassType, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIOption Flags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCIVar(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIOption Flags, LLVMMetadataRef Ty, LLVMMetadataRef PropertyNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCProperty(LLVMDIBuilderRef Builder, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string GetterName, size_t GetterNameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string SetterName, size_t SetterNameLen, uint PropertyAttributes, LLVMMetadataRef Ty);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjectPointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef Type, [MarshalAs( UnmanagedType.Bool )] bool Implicit);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateQualifiedType(LLVMDIBuilderRef Builder, uint Tag, LLVMMetadataRef Type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReferenceType(LLVMDIBuilderRef Builder, uint Tag, LLVMMetadataRef Type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNullPtrType(LLVMDIBuilderRef Builder);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTypedef(LLVMDIBuilderRef Builder, LLVMMetadataRef Type, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Scope, UInt32 AlignInBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateInheritance(LLVMDIBuilderRef Builder, LLVMMetadataRef Ty, LLVMMetadataRef BaseTy, UInt64 BaseOffset, UInt32 VBPtrOffset, LLVMDIOption Flags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateForwardDecl(LLVMDIBuilderRef Builder, uint Tag, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint RuntimeLang, UInt64 SizeInBits, UInt32 AlignInBits, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType(LLVMDIBuilderRef Builder, uint Tag, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint RuntimeLang, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIOption Flags, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBitFieldMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt64 OffsetInBits, UInt64 StorageOffsetInBits, LLVMDIOption Flags, LLVMMetadataRef Type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateClassType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIOption Flags, LLVMMetadataRef DerivedFrom, out LLVMMetadataRef Elements, uint NumElements, LLVMMetadataRef VTableHolder, LLVMMetadataRef TemplateParamsNode, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArtificialType(LLVMDIBuilderRef Builder, LLVMMetadataRef Type);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing( typeof( AnsiStringMarshaller ) )]
        public static unsafe partial string LLVMDITypeGetName(LLVMMetadataRef DType, out size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetSizeInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetOffsetInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LLVMDITypeGetAlignInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDITypeGetLine(LLVMMetadataRef DType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIOption LLVMDITypeGetFlags(LLVMMetadataRef DType);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange(LLVMDIBuilderRef Builder, Int64 LowerBound, Int64 Count);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateArray(LLVMDIBuilderRef Builder, [In] LLVMMetadataRef[] Data, size_t NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateExpression(LLVMDIBuilderRef Builder, [In] UInt64[] Addr, size_t Length);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateConstantValueExpression(LLVMDIBuilderRef Builder, UInt64 Value);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Linkage, size_t LinkLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit, LLVMMetadataRef Expr, LLVMMetadataRef Decl, UInt32 AlignInBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt16 LLVMGetDINodeTag(LLVMMetadataRef MD);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetVariable(LLVMMetadataRef GVE);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetExpression(LLVMMetadataRef GVE);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetFile(LLVMMetadataRef Var);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetScope(LLVMMetadataRef Var);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDIVariableGetLine(LLVMMetadataRef Var);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMTemporaryMDNode(LLVMContextRef Ctx, out LLVMMetadataRef Data, size_t NumElements);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTemporaryMDNode(LLVMMetadataRef TempNode);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMetadataReplaceAllUsesWith(LLVMMetadataRef TempTargetMetadata, LLVMMetadataRef Replacement);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempGlobalVariableFwdDecl(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Linkage, size_t LnkLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit, LLVMMetadataRef Decl, UInt32 AlignInBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordBefore(LLVMDIBuilderRef Builder, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordAtEnd(LLVMDIBuilderRef Builder, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMBasicBlockRef Block);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordBefore(LLVMDIBuilderRef Builder, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMValueRef Instr);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordAtEnd(LLVMDIBuilderRef Builder, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMBasicBlockRef Block);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateAutoVariable(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve, LLVMDIOption Flags, UInt32 AlignInBits);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateParameterVariable(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, uint ArgNo, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve, LLVMDIOption Flags);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetSubprogram(LLVMValueRef Func);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSubprogram(LLVMValueRef Func, LLVMMetadataRef SP);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDISubprogramGetLine(LLVMMetadataRef Subprogram);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMInstructionGetDebugLoc(LLVMValueRef Inst);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionSetDebugLoc(LLVMValueRef Inst, LLVMMetadataRef Loc);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLabel(LLVMDIBuilderRef Builder, LLVMMetadataRef Context, [MarshalUsing( typeof( AnsiStringMarshaller ) )] string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelBefore(LLVMDIBuilderRef Builder, LLVMMetadataRef LabelInfo, LLVMMetadataRef Location, LLVMValueRef InsertBefore);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelAtEnd(LLVMDIBuilderRef Builder, LLVMMetadataRef LabelInfo, LLVMMetadataRef Location, LLVMBasicBlockRef InsertAtEnd);

        [LibraryImport( LibraryPath )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataKind LLVMGetMetadataKind(LLVMMetadataRef Metadata);
    }
}
