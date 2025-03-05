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
    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI" )]
    [SuppressMessage( "Design", "CA1069:Enums values should not be duplicated", Justification = "Values are defined by ABI; [Mutually exclusive but have different meanings in context]" )]
    [SuppressMessage( "Naming", "CA1700:Do not name enum values 'Reserved'", Justification = "Matches ABI" )]
    public enum LLVMDIFlags
        : Int32
    {
        LLVMDIFlagZero = 0,
        LLVMDIFlagPrivate = 1,
        LLVMDIFlagProtected = 2,
        LLVMDIFlagPublic = 3,
        LLVMDIFlagFwdDecl = 4,
        LLVMDIFlagAppleBlock = 8,
        LLVMDIFlagReservedBit4 = 16,
        LLVMDIFlagVirtual = 32,
        LLVMDIFlagArtificial = 64,
        LLVMDIFlagExplicit = 128,
        LLVMDIFlagPrototyped = 256,
        LLVMDIFlagObjcClassComplete = 512,
        LLVMDIFlagObjectPointer = 1024,
        LLVMDIFlagVector = 2048,
        LLVMDIFlagStaticMember = 4096,
        LLVMDIFlagLValueReference = 8192,
        LLVMDIFlagRValueReference = 16384,
        LLVMDIFlagReserved = 32768,
        LLVMDIFlagSingleInheritance = 65536,
        LLVMDIFlagMultipleInheritance = 131072,
        LLVMDIFlagVirtualInheritance = 196608,
        LLVMDIFlagIntroducedVirtual = 262144,
        LLVMDIFlagBitField = 524288,
        LLVMDIFlagNoReturn = 1048576,
        LLVMDIFlagTypePassByValue = 4194304,
        LLVMDIFlagTypePassByReference = 8388608,
        LLVMDIFlagEnumClass = 16777216,
        LLVMDIFlagFixedEnum = 16777216,
        LLVMDIFlagThunk = 33554432,
        LLVMDIFlagNonTrivial = 67108864,
        LLVMDIFlagBigEndian = 134217728,
        LLVMDIFlagLittleEndian = 268435456,
        LLVMDIFlagIndirectVirtualBase = 36,
        LLVMDIFlagAccessibility = 3,
        LLVMDIFlagPtrToMemberRep = 196608,
    }

    public enum LLVMDWARFSourceLanguage
        : Int32
    {
        LLVMDWARFSourceLanguageC89 = 0,
        LLVMDWARFSourceLanguageC = 1,
        LLVMDWARFSourceLanguageAda83 = 2,
        LLVMDWARFSourceLanguageC_plus_plus = 3,
        LLVMDWARFSourceLanguageCobol74 = 4,
        LLVMDWARFSourceLanguageCobol85 = 5,
        LLVMDWARFSourceLanguageFortran77 = 6,
        LLVMDWARFSourceLanguageFortran90 = 7,
        LLVMDWARFSourceLanguagePascal83 = 8,
        LLVMDWARFSourceLanguageModula2 = 9,
        LLVMDWARFSourceLanguageJava = 10,
        LLVMDWARFSourceLanguageC99 = 11,
        LLVMDWARFSourceLanguageAda95 = 12,
        LLVMDWARFSourceLanguageFortran95 = 13,
        LLVMDWARFSourceLanguagePLI = 14,
        LLVMDWARFSourceLanguageObjC = 15,
        LLVMDWARFSourceLanguageObjC_plus_plus = 16,
        LLVMDWARFSourceLanguageUPC = 17,
        LLVMDWARFSourceLanguageD = 18,
        LLVMDWARFSourceLanguagePython = 19,
        LLVMDWARFSourceLanguageOpenCL = 20,
        LLVMDWARFSourceLanguageGo = 21,
        LLVMDWARFSourceLanguageModula3 = 22,
        LLVMDWARFSourceLanguageHaskell = 23,
        LLVMDWARFSourceLanguageC_plus_plus_03 = 24,
        LLVMDWARFSourceLanguageC_plus_plus_11 = 25,
        LLVMDWARFSourceLanguageOCaml = 26,
        LLVMDWARFSourceLanguageRust = 27,
        LLVMDWARFSourceLanguageC11 = 28,
        LLVMDWARFSourceLanguageSwift = 29,
        LLVMDWARFSourceLanguageJulia = 30,
        LLVMDWARFSourceLanguageDylan = 31,
        LLVMDWARFSourceLanguageC_plus_plus_14 = 32,
        LLVMDWARFSourceLanguageFortran03 = 33,
        LLVMDWARFSourceLanguageFortran08 = 34,
        LLVMDWARFSourceLanguageRenderScript = 35,
        LLVMDWARFSourceLanguageBLISS = 36,
        LLVMDWARFSourceLanguageKotlin = 37,
        LLVMDWARFSourceLanguageZig = 38,
        LLVMDWARFSourceLanguageCrystal = 39,
        LLVMDWARFSourceLanguageC_plus_plus_17 = 40,
        LLVMDWARFSourceLanguageC_plus_plus_20 = 41,
        LLVMDWARFSourceLanguageC17 = 42,
        LLVMDWARFSourceLanguageFortran18 = 43,
        LLVMDWARFSourceLanguageAda2005 = 44,
        LLVMDWARFSourceLanguageAda2012 = 45,
        LLVMDWARFSourceLanguageHIP = 46,
        LLVMDWARFSourceLanguageAssembly = 47,
        LLVMDWARFSourceLanguageC_sharp = 48,
        LLVMDWARFSourceLanguageMojo = 49,
        LLVMDWARFSourceLanguageGLSL = 50,
        LLVMDWARFSourceLanguageGLSL_ES = 51,
        LLVMDWARFSourceLanguageHLSL = 52,
        LLVMDWARFSourceLanguageOpenCL_CPP = 53,
        LLVMDWARFSourceLanguageCPP_for_OpenCL = 54,
        LLVMDWARFSourceLanguageSYCL = 55,
        LLVMDWARFSourceLanguageRuby = 56,
        LLVMDWARFSourceLanguageMove = 57,
        LLVMDWARFSourceLanguageHylo = 58,
        LLVMDWARFSourceLanguageMetal = 59,
        LLVMDWARFSourceLanguageMips_Assembler = 60,
        LLVMDWARFSourceLanguageGOOGLE_RenderScript = 61,
        LLVMDWARFSourceLanguageBORLAND_Delphi = 62,
    }

    public enum LLVMDWARFEmissionKind
        : Int32
    {
        LLVMDWARFEmissionNone = 0,
        LLVMDWARFEmissionFull = 1,
        LLVMDWARFEmissionLineTablesOnly = 2,
    }

    public enum LLVMMetadataKind
        : Int32
    {
        LLVMMDStringMetadataKind = 0,
        LLVMConstantAsMetadataMetadataKind = 1,
        LLVMLocalAsMetadataMetadataKind = 2,
        LLVMDistinctMDOperandPlaceholderMetadataKind = 3,
        LLVMMDTupleMetadataKind = 4,
        LLVMDILocationMetadataKind = 5,
        LLVMDIExpressionMetadataKind = 6,
        LLVMDIGlobalVariableExpressionMetadataKind = 7,
        LLVMGenericDINodeMetadataKind = 8,
        LLVMDISubrangeMetadataKind = 9,
        LLVMDIEnumeratorMetadataKind = 10,
        LLVMDIBasicTypeMetadataKind = 11,
        LLVMDIDerivedTypeMetadataKind = 12,
        LLVMDICompositeTypeMetadataKind = 13,
        LLVMDISubroutineTypeMetadataKind = 14,
        LLVMDIFileMetadataKind = 15,
        LLVMDICompileUnitMetadataKind = 16,
        LLVMDISubprogramMetadataKind = 17,
        LLVMDILexicalBlockMetadataKind = 18,
        LLVMDILexicalBlockFileMetadataKind = 19,
        LLVMDINamespaceMetadataKind = 20,
        LLVMDIModuleMetadataKind = 21,
        LLVMDITemplateTypeParameterMetadataKind = 22,
        LLVMDITemplateValueParameterMetadataKind = 23,
        LLVMDIGlobalVariableMetadataKind = 24,
        LLVMDILocalVariableMetadataKind = 25,
        LLVMDILabelMetadataKind = 26,
        LLVMDIObjCPropertyMetadataKind = 27,
        LLVMDIImportedEntityMetadataKind = 28,
        LLVMDIMacroMetadataKind = 29,
        LLVMDIMacroFileMetadataKind = 30,
        LLVMDICommonBlockMetadataKind = 31,
        LLVMDIStringTypeMetadataKind = 32,
        LLVMDIGenericSubrangeMetadataKind = 33,
        LLVMDIArgListMetadataKind = 34,
        LLVMDIAssignIDMetadataKind = 35,
    }

    [SuppressMessage( "Design", "CA1008:Enums should have zero value", Justification = "Matches ABI" )]
    public enum LLVMDWARFMacinfoRecordType
        : Int32
    {
        LLVMDWARFMacinfoRecordTypeDefine = 1,
        LLVMDWARFMacinfoRecordTypeMacro = 2,
        LLVMDWARFMacinfoRecordTypeStartFile = 3,
        LLVMDWARFMacinfoRecordTypeEndFile = 4,
        LLVMDWARFMacinfoRecordTypeVendorExt = 255,
    }

    public static partial class NativeMethods
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDebugMetadataVersion();

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetModuleDebugMetadataVersion(LLVMModuleRef Module);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMStripModuleDebugInfo(LLVMModuleRef Module);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilderDisallowUnresolved(LLVMModuleRef M);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilder(LLVMModuleRef M);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalize(LLVMDIBuilderRef Builder);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalizeSubprogram(LLVMDIBuilderRef Builder, LLVMMetadataRef Subprogram);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateCompileUnit(
            LLVMDIBuilderRef Builder,
            LLVMDWARFSourceLanguage Lang,
            LLVMMetadataRef FileRef,
            string? Producer,
            size_t ProducerLen,
            [MarshalAs( UnmanagedType.Bool )] bool isOptimized,
            string? Flags,
            size_t FlagsLen,
            uint RuntimeVer,
            string? SplitName,
            size_t SplitNameLen,
            LLVMDWARFEmissionKind Kind,
            uint DWOId,
            [MarshalAs( UnmanagedType.Bool )] bool SplitDebugInlining,
            [MarshalAs( UnmanagedType.Bool )] bool DebugInfoForProfiling,
            string? SysRoot,
            size_t SysRootLen,
            string? SDK,
            size_t SDKLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFile(LLVMDIBuilderRef Builder, string? Filename, size_t FilenameLen, string? Directory, size_t DirectoryLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateModule(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentScope, string Name, size_t NameLen, string ConfigMacros, size_t ConfigMacrosLen, string IncludePath, size_t IncludePathLen, string APINotesFile, size_t APINotesFileLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNameSpace(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentScope, string Name, size_t NameLen, [MarshalAs( UnmanagedType.Bool )] bool ExportSymbols);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFunction(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, string LinkageName, size_t LinkageNameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool IsLocalToUnit, [MarshalAs( UnmanagedType.Bool )] bool IsDefinition, uint ScopeLine, LLVMDIFlags Flags, [MarshalAs( UnmanagedType.Bool )] bool IsOptimized);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint Column);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Discriminator);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromNamespace(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef NS, LLVMMetadataRef File, uint Line);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromAlias(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef ImportedEntity, LLVMMetadataRef File, uint Line, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromModule(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef M, LLVMMetadataRef File, uint Line, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedDeclaration(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, LLVMMetadataRef Decl, LLVMMetadataRef File, uint Line, string Name, size_t NameLen, out LLVMMetadataRef Elements, uint NumElements);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateDebugLocation(LLVMContextRef Ctx, uint Line, uint Column, LLVMMetadataRef Scope, LLVMMetadataRef InlinedAt);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetLine(LLVMMetadataRef Location);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetColumn(LLVMMetadataRef Location);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetScope(LLVMMetadataRef Location);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetInlinedAt(LLVMMetadataRef Location);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIScopeGetFile(LLVMMetadataRef Scope);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMDIFileGetDirectory(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMDIFileGetFilename(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMDIFileGetSource(LLVMMetadataRef File, out uint Len);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray(LLVMDIBuilderRef Builder, [In] LLVMMetadataRef[] Data, size_t NumElements);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateSubroutineType(LLVMDIBuilderRef Builder, LLVMMetadataRef File, [In] LLVMMetadataRef[] ParameterTypes, uint NumParameterTypes, LLVMDIFlags Flags);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMacro(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentMacroFile, uint Line, LLVMDWARFMacinfoRecordType RecordType, string Name, size_t NameLen, string Value, size_t ValueLen);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempMacroFile(LLVMDIBuilderRef Builder, LLVMMetadataRef ParentMacroFile, uint Line, LLVMMetadataRef File);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerator(LLVMDIBuilderRef Builder, string Name, size_t NameLen, Int64 Value, [MarshalAs( UnmanagedType.Bool )] bool IsUnsigned);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerationType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, [In] LLVMMetadataRef[] Elements, uint NumElements, LLVMMetadataRef ClassTy);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnionType(
            LLVMDIBuilderRef Builder,
            LLVMMetadataRef Scope,
            string Name,
            size_t NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            [In] LLVMMetadataRef[] Elements,
            uint NumElements,
            uint RunTimeLang,
            string UniqueId,
            size_t UniqueIdLen
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArrayType(LLVMDIBuilderRef Builder, UInt64 Size, UInt32 AlignInBits, LLVMMetadataRef Ty, [In] LLVMMetadataRef[] Subscripts, uint NumSubscripts);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateVectorType(LLVMDIBuilderRef Builder, UInt64 Size, UInt32 AlignInBits, LLVMMetadataRef Ty, [In] LLVMMetadataRef[] Subscripts, uint NumSubscripts);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnspecifiedType(LLVMDIBuilderRef Builder, string Name, size_t NameLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBasicType(LLVMDIBuilderRef Builder, string Name, size_t NameLen, UInt64 SizeInBits, UInt32 Encoding, LLVMDIFlags Flags);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreatePointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef PointeeTy, UInt64 SizeInBits, UInt32 AlignInBits, uint AddressSpace, string Name, size_t NameLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStructType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIFlags Flags, LLVMMetadataRef DerivedFrom, [In] LLVMMetadataRef[] Elements, uint NumElements, uint RunTimeLang, LLVMMetadataRef VTableHolder, string UniqueId, size_t UniqueIdLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIFlags Flags, LLVMMetadataRef Ty);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStaticMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, LLVMMetadataRef Type, LLVMDIFlags Flags, LLVMValueRef ConstantVal, UInt32 AlignInBits);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberPointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef PointeeType, LLVMMetadataRef ClassType, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIFlags Flags);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCIVar(LLVMDIBuilderRef Builder, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIFlags Flags, LLVMMetadataRef Ty, LLVMMetadataRef PropertyNode);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCProperty(LLVMDIBuilderRef Builder, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, string GetterName, size_t GetterNameLen, string SetterName, size_t SetterNameLen, uint PropertyAttributes, LLVMMetadataRef Ty);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjectPointerType(LLVMDIBuilderRef Builder, LLVMMetadataRef Type, [MarshalAs( UnmanagedType.Bool )] bool Implicit);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateQualifiedType(LLVMDIBuilderRef Builder, uint Tag, LLVMMetadataRef Type);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReferenceType(LLVMDIBuilderRef Builder, uint Tag, LLVMMetadataRef Type);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNullPtrType(LLVMDIBuilderRef Builder);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTypedef(LLVMDIBuilderRef Builder, LLVMMetadataRef Type, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Scope, UInt32 AlignInBits);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateInheritance(LLVMDIBuilderRef Builder, LLVMMetadataRef Ty, LLVMMetadataRef BaseTy, UInt64 BaseOffset, UInt32 VBPtrOffset, LLVMDIFlags Flags);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateForwardDecl(LLVMDIBuilderRef Builder, uint Tag, string Name, size_t NameLen, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint RuntimeLang, UInt64 SizeInBits, UInt32 AlignInBits, string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType(LLVMDIBuilderRef Builder, uint Tag, string Name, size_t NameLen, LLVMMetadataRef Scope, LLVMMetadataRef File, uint Line, uint RuntimeLang, UInt64 SizeInBits, UInt32 AlignInBits, LLVMDIFlags Flags, string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBitFieldMemberType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt64 OffsetInBits, UInt64 StorageOffsetInBits, LLVMDIFlags Flags, LLVMMetadataRef Type);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateClassType(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNumber, UInt64 SizeInBits, UInt32 AlignInBits, UInt64 OffsetInBits, LLVMDIFlags Flags, LLVMMetadataRef DerivedFrom, out LLVMMetadataRef Elements, uint NumElements, LLVMMetadataRef VTableHolder, LLVMMetadataRef TemplateParamsNode, string UniqueIdentifier, size_t UniqueIdentifierLen);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArtificialType(LLVMDIBuilderRef Builder, LLVMMetadataRef Type);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalUsing(typeof(ConstStringMarshaller))]
        public static unsafe partial string? LLVMDITypeGetName(LLVMMetadataRef DType, out size_t Length);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetSizeInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetOffsetInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LLVMDITypeGetAlignInBits(LLVMMetadataRef DType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDITypeGetLine(LLVMMetadataRef DType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIFlags LLVMDITypeGetFlags(LLVMMetadataRef DType);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange(LLVMDIBuilderRef Builder, Int64 LowerBound, Int64 Count);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateArray(LLVMDIBuilderRef Builder, [In] LLVMMetadataRef[] Data, size_t NumElements);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateExpression(LLVMDIBuilderRef Builder, [In] UInt64[] Addr, size_t Length);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateConstantValueExpression(LLVMDIBuilderRef Builder, UInt64 Value);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, string Linkage, size_t LinkLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit, LLVMMetadataRef Expr, LLVMMetadataRef Decl, UInt32 AlignInBits);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt16 LLVMGetDINodeTag(LLVMMetadataRef MD);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetVariable(LLVMMetadataRef GVE);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetExpression(LLVMMetadataRef GVE);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetFile(LLVMMetadataRef Var);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetScope(LLVMMetadataRef Var);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDIVariableGetLine(LLVMMetadataRef Var);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMTemporaryMDNode(LLVMContextRef Ctx, out LLVMMetadataRef Data, size_t NumElements);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTemporaryMDNode(LLVMMetadataRef TempNode);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMetadataReplaceAllUsesWith(LLVMMetadataRef TempTargetMetadata, LLVMMetadataRef Replacement);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempGlobalVariableFwdDecl(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, string Linkage, size_t LnkLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit, LLVMMetadataRef Decl, UInt32 AlignInBits);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordBefore(LLVMDIBuilderRef Builder, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMValueRef Instr);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordAtEnd(LLVMDIBuilderRef Builder, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMBasicBlockRef Block);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordBefore(LLVMDIBuilderRef Builder, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMValueRef Instr);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordAtEnd(LLVMDIBuilderRef Builder, LLVMValueRef Val, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef DebugLoc, LLVMBasicBlockRef Block);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateAutoVariable(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve, LLVMDIFlags Flags, UInt32 AlignInBits);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateParameterVariable(LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, string Name, size_t NameLen, uint ArgNo, LLVMMetadataRef File, uint LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve, LLVMDIFlags Flags);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetSubprogram(LLVMValueRef Func);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSubprogram(LLVMValueRef Func, LLVMMetadataRef SP);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDISubprogramGetLine(LLVMMetadataRef Subprogram);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMInstructionGetDebugLoc(LLVMValueRef Inst);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionSetDebugLoc(LLVMValueRef Inst, LLVMMetadataRef Loc);

        [LibraryImport( LibraryName, StringMarshallingCustomType = typeof( ExecutionEncodingStringMarshaller ) )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLabel(LLVMDIBuilderRef Builder, LLVMMetadataRef Context, string Name, size_t NameLen, LLVMMetadataRef File, uint LineNo, [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelBefore(LLVMDIBuilderRef Builder, LLVMMetadataRef LabelInfo, LLVMMetadataRef Location, LLVMValueRef InsertBefore);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelAtEnd(LLVMDIBuilderRef Builder, LLVMMetadataRef LabelInfo, LLVMMetadataRef Location, LLVMBasicBlockRef InsertAtEnd);

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataKind LLVMGetMetadataKind(LLVMMetadataRef Metadata);
    }
}
