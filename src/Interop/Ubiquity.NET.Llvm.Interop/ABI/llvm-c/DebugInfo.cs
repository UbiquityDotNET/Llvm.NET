// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Usually ordering applies, however in this case the ordering is by method name
// and sometimes contains a wrapper method on the low level to make use easier.
#pragma warning disable SA1202 // Elements should be ordered by access

// IN semantics arrays and the length of that array are placed on a single line for clarity
#pragma warning disable SA1117 // Parameters should be on same line or separate lines

namespace Ubiquity.NET.Llvm.Interop.ABI.llvm_c
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

    public static partial class DebugInfo
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDebugMetadataVersion( );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMGetModuleDebugMetadataVersion( LLVMModuleRefAlias Module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMStripModuleDebugInfo( LLVMModuleRefAlias Module );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilderDisallowUnresolved( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIBuilderRef LLVMCreateDIBuilder( LLVMModuleRefAlias M );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalize( LLVMDIBuilderRefAlias Builder );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDIBuilderFinalizeSubprogram( LLVMDIBuilderRefAlias Builder, LLVMMetadataRef Subprogram );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateCompileUnit(
            LLVMDIBuilderRefAlias Builder,
            LLVMDWARFSourceLanguage Lang,
            LLVMMetadataRef FileRef,
            LazyEncodedString? Producer,
            [MarshalAs( UnmanagedType.Bool )] bool isOptimized,
            LazyEncodedString? Flags,
            uint RuntimeVer,
            LazyEncodedString? SplitName,
            LLVMDWARFEmissionKind Kind,
            uint DWOId,
            [MarshalAs( UnmanagedType.Bool )] bool SplitDebugInlining,
            [MarshalAs( UnmanagedType.Bool )] bool DebugInfoForProfiling,
            LazyEncodedString? SysRoot,
            LazyEncodedString? SDK
            )
        {
            return LLVMDIBuilderCreateCompileUnit(
                Builder,
                Lang,
                FileRef,
                Producer, Producer?.NativeStrLen ?? 0,
                isOptimized,
                Flags, Flags?.NativeStrLen ?? 0,
                RuntimeVer,
                SplitName, SplitName?.NativeStrLen ?? 0,
                Kind,
                DWOId,
                SplitDebugInlining,
                DebugInfoForProfiling,
                SysRoot, SysRoot?.NativeStrLen ?? 0,
                SDK, SDK?.NativeStrLen ?? 0
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateCompileUnit(
            LLVMDIBuilderRefAlias Builder,
            LLVMDWARFSourceLanguage Lang,
            LLVMMetadataRef FileRef,
            LazyEncodedString? Producer, nuint ProducerLen,
            [MarshalAs( UnmanagedType.Bool )] bool isOptimized,
            LazyEncodedString? Flags, nuint FlagsLen,
            uint RuntimeVer,
            LazyEncodedString? SplitName, nuint SplitNameLen,
            LLVMDWARFEmissionKind Kind,
            uint DWOId,
            [MarshalAs( UnmanagedType.Bool )] bool SplitDebugInlining,
            [MarshalAs( UnmanagedType.Bool )] bool DebugInfoForProfiling,
            LazyEncodedString? SysRoot, nuint SysRootLen,
            LazyEncodedString? SDK, nuint SDKLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateFile( LLVMDIBuilderRefAlias Builder, LazyEncodedString Filename, LazyEncodedString Directory )
        {
            return LLVMDIBuilderCreateFile( Builder, Filename, Filename.NativeStrLen, Directory, Directory.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFile(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Filename, nuint FilenameLen,
            LazyEncodedString Directory, nuint DirectoryLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateModule(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentScope,
            LazyEncodedString Name,
            LazyEncodedString ConfigMacros,
            LazyEncodedString IncludePath,
            LazyEncodedString APINotesFile
            )
        {
            return LLVMDIBuilderCreateModule(
                Builder,
                ParentScope,
                Name, Name.NativeStrLen,
                ConfigMacros, Name.NativeStrLen,
                IncludePath, IncludePath.NativeStrLen,
                APINotesFile, APINotesFile.NativeStrLen
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateModule(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentScope,
            LazyEncodedString Name, nuint NameLen,
            LazyEncodedString ConfigMacros, nuint ConfigMacrosLen,
            LazyEncodedString IncludePath, nuint IncludePathLen,
            LazyEncodedString APINotesFile, nuint APINotesFileLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateNameSpace(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentScope,
            LazyEncodedString Name,
            [MarshalAs( UnmanagedType.Bool )] bool ExportSymbols
            )
        {
            return LLVMDIBuilderCreateNameSpace( Builder, ParentScope, Name, Name.NativeStrLen, ExportSymbols );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNameSpace(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentScope,
            LazyEncodedString Name, nuint NameLen,
            [MarshalAs( UnmanagedType.Bool )] bool ExportSymbols
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateFunction(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LazyEncodedString LinkageName,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool IsLocalToUnit,
            [MarshalAs( UnmanagedType.Bool )] bool IsDefinition,
            uint ScopeLine,
            LLVMDIFlags Flags,
            [MarshalAs( UnmanagedType.Bool )] bool IsOptimized
            )
        {
            return LLVMDIBuilderCreateFunction(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                LinkageName, LinkageName.NativeStrLen,
                File,
                LineNo,
                Ty,
                IsLocalToUnit,
                IsDefinition,
                ScopeLine,
                Flags,
                IsOptimized
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateFunction(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LazyEncodedString LinkageName, nuint LinkageNameLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool IsLocalToUnit,
            [MarshalAs( UnmanagedType.Bool )] bool IsDefinition,
            uint ScopeLine,
            LLVMDIFlags Flags,
            [MarshalAs( UnmanagedType.Bool )] bool IsOptimized
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Line,
            uint Column
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Discriminator
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromNamespace(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef NS,
            LLVMMetadataRef File,
            uint Line
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromAlias(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef ImportedEntity,
            LLVMMetadataRef File,
            uint Line,
            LLVMMetadataRef[] Elements
            )
        {
            return LLVMDIBuilderCreateImportedModuleFromAlias(Builder, Scope, ImportedEntity, File, Line, Elements, checked((uint)Elements.Length));
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromAlias(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef ImportedEntity,
            LLVMMetadataRef File,
            uint Line,
            [In] LLVMMetadataRef[] Elements,
            uint NumElements // NumElements MUST be <= Elements.Length
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromModule(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef M,
            LLVMMetadataRef File,
            uint Line,
            LLVMMetadataRef[] Elements
            )
        {
            return LLVMDIBuilderCreateImportedModuleFromModule(Builder, Scope, M, File, Line, Elements, checked((uint)Elements.Length));
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedModuleFromModule(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef M,
            LLVMMetadataRef File,
            uint Line,
            [In] LLVMMetadataRef[] Elements,
            uint NumElements // NumElements MUST be <= Elements.Length
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateImportedDeclaration(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef Decl,
            LLVMMetadataRef File,
            uint Line,
            LazyEncodedString Name,
            LLVMMetadataRef[] Elements
            )
        {
            return LLVMDIBuilderCreateImportedDeclaration(
                Builder,
                Scope,
                Decl,
                File,
                Line,
                Name, Name.NativeStrLen,
                Elements, checked((uint)Elements.LongLength)
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateImportedDeclaration(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LLVMMetadataRef Decl,
            LLVMMetadataRef File,
            uint Line,
            LazyEncodedString Name, nuint NameLen,
            [In] LLVMMetadataRef[] Elements,
            uint NumElements // NumElements MUST be <= Elements.Length
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateDebugLocation(
            LLVMContextRefAlias Ctx,
            uint Line,
            uint Column,
            LLVMMetadataRef Scope,
            LLVMMetadataRef InlinedAt
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetLine( LLVMMetadataRef Location );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDILocationGetColumn( LLVMMetadataRef Location );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetScope( LLVMMetadataRef Location );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDILocationGetInlinedAt( LLVMMetadataRef Location );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIScopeGetFile( LLVMMetadataRef Scope );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMDIFileGetDirectory( LLVMMetadataRef File )
        {
            unsafe
            {
                byte* p = LLVMDIFileGetDirectory(File, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMDIFileGetDirectory( LLVMMetadataRef File, out uint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMDIFileGetFilename( LLVMMetadataRef File )
        {
            unsafe
            {
                byte* p = LLVMDIFileGetFilename(File, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMDIFileGetFilename( LLVMMetadataRef File, out uint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMDIFileGetSource( LLVMMetadataRef File )
        {
            unsafe
            {
                byte* p = LLVMDIFileGetSource(File, out uint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMDIFileGetSource( LLVMMetadataRef File, out uint Len );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray( LLVMDIBuilderRefAlias Builder, LLVMMetadataRef[] Data )
        {
            return LLVMDIBuilderGetOrCreateTypeArray( Builder, Data, checked((nuint)Data.LongLength) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray(
            LLVMDIBuilderRefAlias Builder,
            [In] LLVMMetadataRef[] Data,
            nuint NumElements // NumElements MUST be <= Data.Length
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateSubroutineType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef File,
            LLVMMetadataRef[] ParameterTypes,
            LLVMDIFlags Flags
            )
        {
            return LLVMDIBuilderCreateSubroutineType(Builder, File, ParameterTypes, checked((uint)ParameterTypes.Length), Flags);
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateSubroutineType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef File,
            [In] LLVMMetadataRef[] ParameterTypes,
            uint NumParameterTypes, // NumParameterTypes MUST be <= ParameterTypes.Length
            LLVMDIFlags Flags
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateMacro(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentMacroFile,
            uint Line,
            LLVMDWARFMacinfoRecordType RecordType,
            LazyEncodedString Name,
            LazyEncodedString Value
            )
        {
            return LLVMDIBuilderCreateMacro(
                Builder,
                ParentMacroFile,
                Line,
                RecordType,
                Name, Name.NativeStrLen,
                Value, Value.NativeStrLen
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMacro(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentMacroFile,
            uint Line,
            LLVMDWARFMacinfoRecordType RecordType,
            LazyEncodedString Name, nuint NameLen,
            LazyEncodedString Value, nuint ValueLen
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempMacroFile(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef ParentMacroFile,
            uint Line,
            LLVMMetadataRef File
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateEnumerator(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name,
            Int64 Value,
            bool IsUnsigned
            )
        {
            return LLVMDIBuilderCreateEnumerator(
                Builder,
                Name, Name.NativeStrLen,
                Value,
                IsUnsigned
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerator(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name, nuint NameLen,
            Int64 Value,
            [MarshalAs( UnmanagedType.Bool )] bool IsUnsigned
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateEnumerationType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMMetadataRef[] Elements,
            LLVMMetadataRef ClassTy
            )
        {
            return LLVMDIBuilderCreateEnumerationType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                SizeInBits,
                AlignInBits,
                Elements, checked((uint)Elements.LongLength),
                ClassTy
                );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateEnumerationType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            [In] LLVMMetadataRef[] Elements,
            uint NumElements,
            LLVMMetadataRef ClassTy
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateUnionType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef[] Elements,
            uint RunTimeLang,
            LazyEncodedString UniqueId
            )
        {
            return LLVMDIBuilderCreateUnionType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                SizeInBits,
                AlignInBits,
                Flags,
                Elements, checked((uint)Elements.LongLength),
                RunTimeLang,
                UniqueId, UniqueId.NativeStrLen
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnionType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            [In] LLVMMetadataRef[] Elements, uint NumElements,
            uint RunTimeLang,
            LazyEncodedString UniqueId, nuint UniqueIdLen
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArrayType(
            LLVMDIBuilderRefAlias Builder,
            UInt64 Size,
            UInt32 AlignInBits,
            LLVMMetadataRef Ty,
            [In] LLVMMetadataRef[] Subscripts,
            uint NumSubscripts
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateVectorType(
            LLVMDIBuilderRefAlias Builder,
            UInt64 Size,
            UInt32 AlignInBits,
            LLVMMetadataRef Ty,
            [In] LLVMMetadataRef[] Subscripts,
            uint NumSubscripts
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateUnspecifiedType(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name
            )
        {
            return LLVMDIBuilderCreateUnspecifiedType( Builder, Name, Name.NativeStrLen );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateUnspecifiedType(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name, nuint NameLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateBasicType(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name,
            UInt64 SizeInBits,
            UInt32 Encoding,
            LLVMDIFlags Flags
            )
        {
            return LLVMDIBuilderCreateBasicType(
                Builder,
                Name, Name.NativeStrLen,
                SizeInBits,
                Encoding,
                Flags
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBasicType(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name, nuint NameLen,
            UInt64 SizeInBits,
            UInt32 Encoding,
            LLVMDIFlags Flags
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreatePointerType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef PointeeTy,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            uint AddressSpace,
            LazyEncodedString? Name
            )
        {
            return LLVMDIBuilderCreatePointerType(
                Builder,
                PointeeTy,
                SizeInBits,
                AlignInBits,
                AddressSpace,
                Name, Name?.NativeStrLen ?? 0
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreatePointerType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef PointeeTy,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            uint AddressSpace,
            LazyEncodedString? Name, nuint NameLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateStructType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef DerivedFrom,
            LLVMMetadataRef[] Elements,
            uint RunTimeLang,
            LLVMMetadataRef VTableHolder,
            LazyEncodedString UniqueId
            )
        {
            return LLVMDIBuilderCreateStructType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                SizeInBits,
                AlignInBits,
                Flags,
                DerivedFrom,
                Elements, checked((uint)Elements.LongLength),
                RunTimeLang,
                VTableHolder,
                UniqueId, UniqueId.NativeStrLen
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStructType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef DerivedFrom,
            [In] LLVMMetadataRef[] Elements, uint NumElements,
            uint RunTimeLang,
            LLVMMetadataRef VTableHolder,
            LazyEncodedString UniqueId, nuint UniqueIdLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Ty
            )
        {
            return LLVMDIBuilderCreateMemberType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                SizeInBits,
                AlignInBits,
                OffsetInBits,
                Flags,
                Ty
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Ty
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateStaticMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            LLVMMetadataRef Type,
            LLVMDIFlags Flags,
            LLVMValueRef ConstantVal,
            UInt32 AlignInBits
            )
        {
            return LLVMDIBuilderCreateStaticMemberType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                Type,
                Flags,
                ConstantVal,
                AlignInBits
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateStaticMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            LLVMMetadataRef Type,
            LLVMDIFlags Flags,
            LLVMValueRef ConstantVal,
            UInt32 AlignInBits
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateMemberPointerType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef PointeeType,
            LLVMMetadataRef ClassType,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateObjCIVar(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Ty,
            LLVMMetadataRef PropertyNode
            )
        {
            return LLVMDIBuilderCreateObjCIVar(
                Builder,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                SizeInBits,
                AlignInBits,
                OffsetInBits,
                Flags,
                Ty,
                PropertyNode
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCIVar(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Ty,
            LLVMMetadataRef PropertyNode
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateObjCProperty(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            LazyEncodedString GetterName,
            LazyEncodedString SetterName,
            uint PropertyAttributes,
            LLVMMetadataRef Ty
            )
        {
            return LLVMDIBuilderCreateObjCProperty(
                Builder,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                GetterName, GetterName.NativeStrLen,
                SetterName, SetterName.NativeStrLen,
                PropertyAttributes,
                Ty
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjCProperty(
            LLVMDIBuilderRefAlias Builder,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            LazyEncodedString GetterName, nuint GetterNameLen,
            LazyEncodedString SetterName, nuint SetterNameLen,
            uint PropertyAttributes,
            LLVMMetadataRef Ty
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateObjectPointerType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Type,
            [MarshalAs( UnmanagedType.Bool )] bool Implicit
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateQualifiedType( LLVMDIBuilderRefAlias Builder, uint Tag, LLVMMetadataRef Type );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReferenceType( LLVMDIBuilderRefAlias Builder, uint Tag, LLVMMetadataRef Type );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateNullPtrType( LLVMDIBuilderRefAlias Builder );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateTypedef(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Type,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Scope,
            UInt32 AlignInBits
            )
        {
            return LLVMDIBuilderCreateTypedef(
                Builder,
                Type,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                Scope,
                AlignInBits
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTypedef(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Type,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Scope,
            UInt32 AlignInBits
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateInheritance(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Ty,
            LLVMMetadataRef BaseTy,
            UInt64 BaseOffset,
            UInt32 VBPtrOffset,
            LLVMDIFlags Flags
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateForwardDecl(
            LLVMDIBuilderRefAlias Builder,
            uint Tag,
            LazyEncodedString Name,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Line,
            uint RuntimeLang,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LazyEncodedString UniqueIdentifier
            )
        {
            return LLVMDIBuilderCreateForwardDecl(
                Builder,
                Tag,
                Name, Name.NativeStrLen,
                Scope,
                File,
                Line,
                RuntimeLang,
                SizeInBits,
                AlignInBits,
                UniqueIdentifier, UniqueIdentifier.NativeStrLen
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateForwardDecl(
            LLVMDIBuilderRefAlias Builder,
            uint Tag,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Line,
            uint RuntimeLang,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LazyEncodedString UniqueIdentifier, nuint UniqueIdentifierLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType(
            LLVMDIBuilderRefAlias Builder,
            uint Tag,
            LazyEncodedString Name,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Line,
            uint RuntimeLang,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            LazyEncodedString UniqueIdentifier
            )
        {
            return LLVMDIBuilderCreateReplaceableCompositeType(
                Builder,
                Tag,
                Name, Name.NativeStrLen,
                Scope,
                File,
                Line,
                RuntimeLang,
                SizeInBits,
                AlignInBits,
                Flags,
                UniqueIdentifier, UniqueIdentifier.NativeStrLen
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType(
            LLVMDIBuilderRefAlias Builder,
            uint Tag,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef Scope,
            LLVMMetadataRef File,
            uint Line,
            uint RuntimeLang,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            LLVMDIFlags Flags,
            LazyEncodedString UniqueIdentifier, nuint UniqueIdentifierLen
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateBitFieldMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt64 OffsetInBits,
            UInt64 StorageOffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Type
            )
        {
            return LLVMDIBuilderCreateBitFieldMemberType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                SizeInBits,
                OffsetInBits,
                StorageOffsetInBits,
                Flags,
                Type
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateBitFieldMemberType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt64 OffsetInBits,
            UInt64 StorageOffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef Type
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateClassType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef DerivedFrom,
            LLVMMetadataRef[] Elements,
            LLVMMetadataRef VTableHolder,
            LLVMMetadataRef TemplateParamsNode,
            LazyEncodedString UniqueIdentifier
            )
        {
            return LLVMDIBuilderCreateClassType(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNumber,
                SizeInBits,
                AlignInBits,
                OffsetInBits,
                Flags,
                DerivedFrom,
                Elements, checked((uint)Elements.LongLength),
                VTableHolder,
                TemplateParamsNode,
                UniqueIdentifier, UniqueIdentifier.NativeStrLen
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateClassType(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNumber,
            UInt64 SizeInBits,
            UInt32 AlignInBits,
            UInt64 OffsetInBits,
            LLVMDIFlags Flags,
            LLVMMetadataRef DerivedFrom,
            [In] LLVMMetadataRef[] Elements, uint NumElements,
            LLVMMetadataRef VTableHolder,
            LLVMMetadataRef TemplateParamsNode,
            LazyEncodedString UniqueIdentifier, nuint UniqueIdentifierLen
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateArtificialType( LLVMDIBuilderRefAlias Builder, LLVMMetadataRef Type );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LazyEncodedString? LLVMDITypeGetName( LLVMMetadataRef DType )
        {
            unsafe
            {
                byte* p = LLVMDITypeGetName(DType, out nuint len);
                return LazyEncodedString.FromUnmanaged( p, len );
            }
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial byte* LLVMDITypeGetName( LLVMMetadataRef DType, out nuint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetSizeInBits( LLVMMetadataRef DType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt64 LLVMDITypeGetOffsetInBits( LLVMMetadataRef DType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt32 LLVMDITypeGetAlignInBits( LLVMMetadataRef DType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDITypeGetLine( LLVMMetadataRef DType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDIFlags LLVMDITypeGetFlags( LLVMMetadataRef DType );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange( LLVMDIBuilderRefAlias Builder, Int64 LowerBound, Int64 Count );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderGetOrCreateArray( LLVMDIBuilderRefAlias Builder, LLVMMetadataRef[] Data )
        {
            return LLVMDIBuilderGetOrCreateArray( Builder, Data, checked((nuint)Data.LongLength) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderGetOrCreateArray(
            LLVMDIBuilderRefAlias Builder,
            [In] LLVMMetadataRef[] Data, nuint NumElements
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRefAlias Builder, UInt64[] Addr )
        {
            return LLVMDIBuilderCreateExpression( Builder, Addr, checked((nuint)Addr.LongLength) );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRefAlias Builder, [In] UInt64[] Addr, nuint Length );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateConstantValueExpression( LLVMDIBuilderRefAlias Builder, UInt64 Value );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LazyEncodedString Linkage,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            bool LocalToUnit,
            LLVMMetadataRef Expr,
            LLVMMetadataRef Decl,
            UInt32 AlignInBits
            )
        {
            return LLVMDIBuilderCreateGlobalVariableExpression(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                Linkage, Linkage.NativeStrLen,
                File,
                LineNo,
                Ty,
                LocalToUnit,
                Expr,
                Decl,
                AlignInBits
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LazyEncodedString Linkage, nuint LinkLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit,
            LLVMMetadataRef Expr,
            LLVMMetadataRef Decl,
            UInt32 AlignInBits
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial UInt16 LLVMGetDINodeTag( LLVMMetadataRef MD );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetVariable( LLVMMetadataRef GVE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIGlobalVariableExpressionGetExpression( LLVMMetadataRef GVE );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetFile( LLVMMetadataRef Var );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMDIVariableGetScope( LLVMMetadataRef Var );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDIVariableGetLine( LLVMMetadataRef Var );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMTemporaryMDNode( LLVMContextRefAlias Ctx, [In] LLVMMetadataRef[] Data, nuint NumElements );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMDisposeTemporaryMDNode( LLVMMetadataRef TempNode );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMMetadataReplaceAllUsesWith( LLVMMetadataRef TempTargetMetadata, LLVMMetadataRef Replacement );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateTempGlobalVariableFwdDecl(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LazyEncodedString Linkage,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            bool LocalToUnit,
            LLVMMetadataRef Decl,
            UInt32 AlignInBits
            )
        {
            return LLVMDIBuilderCreateTempGlobalVariableFwdDecl(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                Linkage, Linkage.NativeStrLen,
                File,
                LineNo,
                Ty,
                LocalToUnit,
                Decl,
                AlignInBits
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateTempGlobalVariableFwdDecl(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LazyEncodedString Linkage, nuint LnkLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool LocalToUnit,
            LLVMMetadataRef Decl,
            UInt32 AlignInBits
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordBefore(
            LLVMDIBuilderRefAlias Builder,
            LLVMValueRef Storage,
            LLVMMetadataRef VarInfo,
            LLVMMetadataRef Expr,
            LLVMMetadataRef DebugLoc,
            LLVMValueRef Instr
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDeclareRecordAtEnd(
            LLVMDIBuilderRefAlias Builder,
            LLVMValueRef Storage,
            LLVMMetadataRef VarInfo,
            LLVMMetadataRef Expr,
            LLVMMetadataRef DebugLoc,
            LLVMBasicBlockRef Block
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordBefore(
            LLVMDIBuilderRefAlias Builder,
            LLVMValueRef Val,
            LLVMMetadataRef VarInfo,
            LLVMMetadataRef Expr,
            LLVMMetadataRef DebugLoc,
            LLVMValueRef Instr
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertDbgValueRecordAtEnd(
            LLVMDIBuilderRefAlias Builder,
            LLVMValueRef Val,
            LLVMMetadataRef VarInfo,
            LLVMMetadataRef Expr,
            LLVMMetadataRef DebugLoc,
            LLVMBasicBlockRef Block
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateAutoVariable(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            bool AlwaysPreserve,
            LLVMDIFlags Flags,
            UInt32 AlignInBits
            )
        {
            return LLVMDIBuilderCreateAutoVariable(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                Ty,
                AlwaysPreserve,
                Flags,
                AlignInBits
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateAutoVariable(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve,
            LLVMDIFlags Flags,
            UInt32 AlignInBits
            );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateParameterVariable(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name,
            uint ArgNo,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            bool AlwaysPreserve,
            LLVMDIFlags Flags
            )
        {
            return LLVMDIBuilderCreateParameterVariable(
                Builder,
                Scope,
                Name, Name.NativeStrLen,
                ArgNo,
                File,
                LineNo,
                Ty,
                AlwaysPreserve,
                Flags
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateParameterVariable(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Scope,
            LazyEncodedString Name, nuint NameLen,
            uint ArgNo,
            LLVMMetadataRef File,
            uint LineNo,
            LLVMMetadataRef Ty,
            [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve,
            LLVMDIFlags Flags
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMGetSubprogram( LLVMValueRef Func );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMSetSubprogram( LLVMValueRef Func, LLVMMetadataRef SP );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial uint LLVMDISubprogramGetLine( LLVMMetadataRef Subprogram );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataRef LLVMInstructionGetDebugLoc( LLVMValueRef Inst );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial void LLVMInstructionSetDebugLoc( LLVMValueRef Inst, LLVMMetadataRef Loc );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static LLVMMetadataRef LLVMDIBuilderCreateLabel(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Context,
            LazyEncodedString Name,
            LLVMMetadataRef File,
            uint LineNo,
            bool AlwaysPreserve
            )
        {
            return LLVMDIBuilderCreateLabel(
                Builder,
                Context,
                Name, Name.NativeStrLen,
                File,
                LineNo,
                AlwaysPreserve
            );
        }

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        private static unsafe partial LLVMMetadataRef LLVMDIBuilderCreateLabel(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef Context,
            LazyEncodedString Name, nuint NameLen,
            LLVMMetadataRef File,
            uint LineNo,
            [MarshalAs( UnmanagedType.Bool )] bool AlwaysPreserve
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelBefore(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef LabelInfo,
            LLVMMetadataRef Location,
            LLVMValueRef InsertBefore
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMDbgRecordRef LLVMDIBuilderInsertLabelAtEnd(
            LLVMDIBuilderRefAlias Builder,
            LLVMMetadataRef LabelInfo,
            LLVMMetadataRef Location,
            LLVMBasicBlockRef InsertAtEnd
            );

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMMetadataKind LLVMGetMetadataKind( LLVMMetadataRef Metadata );
    }
}
