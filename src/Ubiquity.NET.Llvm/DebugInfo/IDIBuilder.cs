// -----------------------------------------------------------------------
// <copyright file="IDIBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Describes the kind of macro declaration</summary>
    public enum MacroKind
    {
        /// <summary>Default None value. This is an invalid value that is not supported by LLVM</summary>
        None = 0,

        /// <summary>Macro definition</summary>
        Define = LLVMDWARFMacinfoRecordType.LLVMDWARFMacinfoRecordTypeDefine,

        /// <summary>Undefine a macro</summary>
        Undefine = LLVMDWARFMacinfoRecordType.LLVMDWARFMacinfoRecordTypeMacro,

        /// <summary>Start of file macro</summary>
        StartFile = LLVMDWARFMacinfoRecordType.LLVMDWARFMacinfoRecordTypeStartFile,

        /// <summary>End of file macro</summary>
        EndFile = LLVMDWARFMacinfoRecordType.LLVMDWARFMacinfoRecordTypeEndFile,

        /// <summary>Vendor specific extension type</summary>
        VendorExt = LLVMDWARFMacinfoRecordType.LLVMDWARFMacinfoRecordTypeVendorExt
    }

    /// <summary>DIBuilder is a factory class for creating DebugInformation for an LLVM <see cref="Module"/></summary>
    /// <remarks>
    /// <para>Many Debug information metadata nodes are created with unresolved references to additional
    /// metadata. To ensure such metadata is resolved applications should call the <see cref="Finish()"/>
    /// method to resolve and finalize the metadata. After this point only fully resolved nodes may
    /// be added to ensure that the data remains valid.</para>
    /// <para>This type is a 'byref like type' to prevent storing it as a member anywhere. It is NOT
    /// a member of <see cref="Module"/> but has one associated with it. Generally, at most one <see cref="DICompileUnit"/>
    /// is associated with a <see cref="DIBuilder"/>. If creating a function
    /// (via <see cref="CreateFunction(DIScope?, LazyEncodedString, LazyEncodedString, DIFile?, uint, DISubroutineType?, bool, bool, uint, DebugInfoFlags, bool, Function)"/>),
    /// then the creation of a <see cref="DICompileUnit"/> is required.</para>
    /// </remarks>
    /// <seealso href="xref:llvm_sourceleveldebugging">LLVM Source Level Debugging</seealso>
    public interface IDIBuilder
            : IEquatable<IDIBuilder>
    {
        /// <summary>Gets the module associated with this builder</summary>
        /// <remarks>
        /// <note type="warning">
        /// This is a projection only value, while the underlying type does hold on
        /// to a module it does NOT provide any means for a consumer (even a C++ one)
        /// from accessing that value. (It's a private member). Thus, this value is
        /// ONLY associated with a single projected instance. This is not usually a problem
        /// as there is no underlying LLVM type that produces a DIBuilder or alias to it so
        /// the only instances are those created by the calling application.
        /// </note>
        /// </remarks>
        IModule OwningModule { get; }

        /// <summary>Gets the compile unit created by this builder [If any]</summary>
        /// <remarks>
        /// <note type="warning">
        /// This is a projection only value, while the underlying type does hold on
        /// to a <see cref="DICompileUnit"/> it does NOT provide any means for a consumer
        /// (even a C++ one) from accessing that value. (It's a private member). Thus, this
        /// value is ONLY associated with a single projected instance. This is not usually
        /// a problem as there is no underlying LLVM type that produces a DIBuilder or alias
        /// to it so the only instances are those created by the calling application in the
        /// managed projection.
        /// </note>
        /// </remarks>
        DICompileUnit? CompileUnit { get; }

        /// <summary>Creates a new <see cref="DICompileUnit"/></summary>
        /// <param name="language"><see cref="SourceLanguage"/> for the compilation unit</param>
        /// <param name="sourceFilePath">Full path to the source file of this compilation unit</param>
        /// <param name="producer">Name of the application processing the compilation unit</param>
        /// <param name="optimized">Flag to indicate if the code in this compilation unit is optimized</param>
        /// <param name="compilationFlags">Additional tool specific flags</param>
        /// <param name="runtimeVersion">Runtime version</param>
        /// <returns><see cref="DICompileUnit"/></returns>
        DICompileUnit CreateCompileUnit( SourceLanguage language
                                       , string sourceFilePath
                                       , LazyEncodedString? producer
                                       , bool optimized = false
                                       , LazyEncodedString? compilationFlags = null
                                       , uint runtimeVersion = 0
                                       );

        /// <summary>Creates a new <see cref="DICompileUnit"/></summary>
        /// <param name="language"><see cref="SourceLanguage"/> for the compilation unit</param>
        /// <param name="fileName">Name of the source file of this compilation unit (without any path)</param>
        /// <param name="fileDirectory">Path of the directory containing the file</param>
        /// <param name="producer">Name of the application processing the compilation unit</param>
        /// <param name="optimized">Flag to indicate if the code in this compilation unit is optimized</param>
        /// <param name="compilationFlags">Additional tool specific flags</param>
        /// <param name="runtimeVersion">Runtime version</param>
        /// <param name="sysRoot">System root for the debug info to use [Default:<see cref="string.Empty"/>]</param>
        /// <param name="sdk">SDK name for the debug record [Default:<see cref="string.Empty"/>]</param>
        /// <returns><see cref="DICompileUnit"/></returns>
        DICompileUnit CreateCompileUnit( SourceLanguage language
                                       , LazyEncodedString fileName
                                       , LazyEncodedString fileDirectory
                                       , LazyEncodedString? producer
                                       , bool optimized
                                       , LazyEncodedString? compilationFlags
                                       , uint runtimeVersion
                                       , LazyEncodedString? sysRoot = null
                                       , LazyEncodedString? sdk = null
                                       );

        /// <summary>Creates a debugging information temporary entry for a macro file</summary>
        /// <param name="parent">Macro file parent, if any</param>
        /// <param name="line">Source line where the macro file is included</param>
        /// <param name="file">File information for the file containing the macro</param>
        /// <returns>Newly created <see cref="DIMacroFile"/></returns>
        /// <remarks>
        /// The list of macro node direct children is calculated by the use of the <see cref="CreateMacro"/>
        /// functions parentFile parameter.
        /// </remarks>
        DIMacroFile CreateTempMacroFile( DIMacroFile? parent, uint line, DIFile? file );

        /// <summary>Create a macro</summary>
        /// <param name="parentFile">Parent file containing the macro</param>
        /// <param name="line">Source line number where the macro is defined</param>
        /// <param name="kind">Id of macro</param>
        /// <param name="name">Name of the macro</param>
        /// <param name="value">Value of the macro (use String.Empty for <see cref="MacroKind.Undefine"/>)</param>
        /// <returns>Newly created macro node</returns>
        public DIMacro CreateMacro( DIMacroFile? parentFile, uint line, MacroKind kind, LazyEncodedString name, LazyEncodedString value );

        /// <summary>Creates a <see cref="DINamespace"/></summary>
        /// <param name="scope">Containing scope for the namespace or null if the namespace is a global one</param>
        /// <param name="name">Name of the namespace</param>
        /// <param name="exportSymbols">export symbols</param>
        /// <returns>Debug namespace</returns>
        DINamespace CreateNamespace( DIScope? scope, LazyEncodedString name, bool exportSymbols );

        /// <summary>Creates a <see cref="DIFile"/></summary>
        /// <param name="path">Path of the file (may be <see langword="null"/> or empty)</param>
        /// <returns>
        /// <see cref="DIFile"/> or <see langword="null"/> if <paramref name="path"/>
        /// is <see langword="null"/> empty, or all whitespace
        /// </returns>
        DIFile CreateFile( string? path );

        /// <summary>Creates a <see cref="DIFile"/></summary>
        /// <param name="fileName">Name of the file (may be <see langword="null"/> or empty)</param>
        /// <param name="directory">Path of the directory containing the file (may be <see langword="null"/> or empty)</param>
        /// <returns>
        /// <see cref="DIFile"/> created
        /// </returns>
        DIFile CreateFile( LazyEncodedString? fileName, LazyEncodedString? directory );

        /* TODO: Extend CreateFile with checksum info and source text params (both optional) */

        /// <summary>Creates a new <see cref="DILexicalBlock"/></summary>
        /// <param name="scope"><see cref="DIScope"/> for the block</param>
        /// <param name="file"><see cref="DIFile"/> containing the block</param>
        /// <param name="line">Starting line number for the block</param>
        /// <param name="column">Starting column for the block</param>
        /// <returns>
        /// <see cref="DILexicalBlock"/> created from the parameters
        /// </returns>
        DILexicalBlock CreateLexicalBlock( DIScope? scope, DIFile? file, uint line, uint column );

        /// <summary>Creates a <see cref="DILexicalBlockFile"/></summary>
        /// <param name="scope"><see cref="DIScope"/> for the block</param>
        /// <param name="file"><see cref="DIFile"/></param>
        /// <param name="discriminator">Discriminator to disambiguate lexical blocks with the same file info</param>
        /// <returns>
        /// <see cref="DILexicalBlockFile"/> constructed from the parameters
        /// </returns>
        DILexicalBlockFile CreateLexicalBlockFile( DIScope? scope, DIFile? file, uint discriminator );

        /// <summary>Factory method to create a <see cref="DISubProgram"/> with debug information</summary>
        /// <param name="scope"><see cref="DIScope"/> for the function</param>
        /// <param name="name">Name of the function as it appears in the source language</param>
        /// <param name="mangledName">Linkage (mangled) name of the function</param>
        /// <param name="file"><see cref="DIFile"/> containing the function</param>
        /// <param name="line">starting line of the function definition</param>
        /// <param name="signatureType"><see cref="DISubroutineType"/> for the function's signature type</param>
        /// <param name="isLocalToUnit">Flag to indicate if this function is local to the compilation unit or available externally</param>
        /// <param name="isDefinition">Flag to indicate if this is a definition or a declaration only</param>
        /// <param name="scopeLine">starting line of the first scope of the function's body</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this function</param>
        /// <param name="isOptimized">Flag to indicate if this function is optimized</param>
        /// <param name="function">Underlying LLVM <see cref="Function"/> to attach debug info to</param>
        /// <returns><see cref="DISubProgram"/> created based on the input parameters</returns>
        DISubProgram CreateFunction( DIScope? scope
                                   , LazyEncodedString name
                                   , LazyEncodedString mangledName
                                   , DIFile? file
                                   , uint line
                                   , DISubroutineType? signatureType
                                   , bool isLocalToUnit
                                   , bool isDefinition
                                   , uint scopeLine
                                   , DebugInfoFlags debugFlags
                                   , bool isOptimized
                                   , Function function
                                   );

        /// <summary>Creates a new forward declaration to a function</summary>
        /// <param name="scope"><see cref="DIScope"/> for the declaration</param>
        /// <param name="name">Name of the function as it appears in source</param>
        /// <param name="mangledName">mangled name of the function (for linker)</param>
        /// <param name="file">Source file location for the function</param>
        /// <param name="line">starting line of the declaration</param>
        /// <param name="subroutineType">Signature for the function</param>
        /// <param name="isLocalToUnit">Flag to indicate if this declaration is local to the compilation unit</param>
        /// <param name="isDefinition">Flag to indicate if this is a definition</param>
        /// <param name="scopeLine">Line of the first scope block</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the function</param>
        /// <param name="isOptimized">Flag to indicate if the function is optimized</param>
        /// <returns>Subprogram as a forward declaration</returns>
        DISubProgram ForwardDeclareFunction( DIScope? scope
                                           , LazyEncodedString name
                                           , LazyEncodedString mangledName
                                           , DIFile? file
                                           , uint line
                                           , DISubroutineType subroutineType
                                           , bool isLocalToUnit
                                           , bool isDefinition
                                           , uint scopeLine
                                           , DebugInfoFlags debugFlags
                                           , bool isOptimized
                                           );

        /// <summary>Creates a <see cref="DILocalVariable"/> for a given scope</summary>
        /// <param name="scope">Scope the variable belongs to</param>
        /// <param name="name">Name of the variable</param>
        /// <param name="file">File where the variable is declared</param>
        /// <param name="line">Line where the variable is declared</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="alwaysPreserve">Flag to indicate if this variable's debug information should always be preserved</param>
        /// <param name="debugFlags">Flags for the variable</param>
        /// <param name="alignInBits">Variable alignment (in Bits)</param>
        /// <returns><see cref="DILocalVariable"/></returns>
        DILocalVariable CreateLocalVariable( DIScope? scope
                                           , LazyEncodedString name
                                           , DIFile? file
                                           , uint line
                                           , DIType? type
                                           , bool alwaysPreserve = false
                                           , DebugInfoFlags debugFlags = DebugInfoFlags.None
                                           , uint alignInBits = 0
                                           );

        /// <summary>Creates an argument for a function as a <see cref="DILocalVariable"/></summary>
        /// <param name="scope">Scope for the argument</param>
        /// <param name="name">Name of the argument</param>
        /// <param name="file"><see cref="DIFile"/> containing the function this argument is declared in</param>
        /// <param name="line">Line number fort his argument</param>
        /// <param name="type">Debug type for this argument</param>
        /// <param name="alwaysPreserve">Flag to indicate if this argument is always preserved for debug view even if optimization would remove it</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this argument</param>
        /// <param name="argNo">One based argument index on the method (e.g the first argument is 1 not 0 )</param>
        /// <returns><see cref="DILocalVariable"/> representing the function argument</returns>
        DILocalVariable CreateArgument( DIScope? scope
                                      , LazyEncodedString name
                                      , DIFile? file
                                      , uint line
                                      , DIType? type
                                      , bool alwaysPreserve
                                      , DebugInfoFlags debugFlags
                                      , ushort argNo
                                      );

        /// <summary>Construct debug information for a basic type (a.k.a. primitive type)</summary>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size for the type</param>
        /// <param name="encoding"><see cref="DiTypeKind"/> encoding for the type</param>
        /// <param name="diFlags"><see cref="DebugInfoFlags"/> for the type</param>
        /// <returns>Basic type debugging information</returns>
        DIBasicType CreateBasicType( LazyEncodedString name
                                   , UInt64 bitSize
                                   , DiTypeKind encoding
                                   , DebugInfoFlags diFlags = DebugInfoFlags.None
                                   );

        /// <summary>Creates a pointer type with debug information</summary>
        /// <param name="pointeeType">base type of the pointer (<see langword="null"/> => void)</param>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size of the type</param>
        /// <param name="bitAlign">But alignment of the type</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns>Pointer type</returns>
        DIDerivedType CreatePointerType( DIType? pointeeType
                                       , LazyEncodedString? name
                                       , UInt64 bitSize
                                       , UInt32 bitAlign = 0
                                       , uint addressSpace = 0
                                       );

        /// <summary>Creates a qualified type</summary>
        /// <param name="baseType">Base type to add the qualifier to</param>
        /// <param name="tag">Qualifier to apply</param>
        /// <returns>Qualified type</returns>
        /// <exception cref="System.ArgumentException"><paramref name="tag"/> is <see cref="QualifiedTypeTag.None"/></exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="baseType"/> is <see langword="null"/></exception>
        DIDerivedType CreateQualifiedType( DIType? baseType, QualifiedTypeTag tag );

        /// <summary>Create a debug metadata array of debug types</summary>
        /// <param name="types">Types to include in the array</param>
        /// <returns>Array containing the types</returns>
        DITypeArray CreateTypeArray( params DIType?[] types ) => CreateTypeArray( (IEnumerable<DIType?>)types );

        /// <summary>Create a debug metadata array of debug types</summary>
        /// <param name="types">Types to include in the array</param>
        /// <returns>Array containing the types</returns>
        DITypeArray CreateTypeArray( IEnumerable<DIType?> types );

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, params DIType?[] types );

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, IEnumerable<DIType?> types );

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags );

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="returnType">Return type of the signature</param>
        /// <param name="types">Parameters for the function</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType? returnType, IEnumerable<DIType?> types );

        /// <summary>Creates debug description of a structure type</summary>
        /// <param name="scope">Scope containing the structure</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">File containing the type</param>
        /// <param name="line">Line of the start of the type</param>
        /// <param name="bitSize">Size of the type in bits</param>
        /// <param name="bitAlign">Bit alignment of the type</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the structure</param>
        /// <param name="derivedFrom"><see cref="DIType"/> this type is derived from, if any</param>
        /// <param name="elements">Node array describing the elements of the structure</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DICompositeType CreateStructType( DIScope? scope
                                        , LazyEncodedString name
                                        , DIFile? file
                                        , uint line
                                        , UInt64 bitSize
                                        , UInt32 bitAlign
                                        , DebugInfoFlags debugFlags
                                        , DIType? derivedFrom
                                        , params DINode[] elements
                                        );

        /// <summary>Creates debug description of a structure type</summary>
        /// <param name="scope">Scope containing the structure</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">File containing the type</param>
        /// <param name="line">Line of the start of the type</param>
        /// <param name="bitSize">Size of the type in bits</param>
        /// <param name="bitAlign">Bit alignment of the type</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the structure</param>
        /// <param name="derivedFrom"><see cref="DIType"/> this type is derived from, if any</param>
        /// <param name="elements">Node array describing the elements of the structure</param>
        /// <param name="runTimeLang">runtime language for the type</param>
        /// <param name="vTableHolder">VTable holder for the type</param>
        /// <param name="uniqueId">Unique ID for the type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DICompositeType CreateStructType( DIScope? scope
                                        , LazyEncodedString name
                                        , DIFile? file
                                        , uint line
                                        , UInt64 bitSize
                                        , UInt32 bitAlign
                                        , DebugInfoFlags debugFlags
                                        , DIType? derivedFrom
                                        , IEnumerable<DINode> elements
                                        , uint runTimeLang = 0
                                        , DIType? vTableHolder = null
                                        , LazyEncodedString? uniqueId = null
                                        );

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DICompositeType CreateUnionType( DIScope? scope
                                       , LazyEncodedString name
                                       , DIFile? file
                                       , uint line
                                       , UInt64 bitSize
                                       , UInt32 bitAlign
                                       , DebugInfoFlags debugFlags
                                       , DINodeArray elements
                                       );

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DICompositeType CreateUnionType( DIScope? scope
                                       , LazyEncodedString name
                                       , DIFile? file
                                       , uint line
                                       , UInt64 bitSize
                                       , UInt32 bitAlign
                                       , DebugInfoFlags debugFlags
                                       , params DINode[] elements
                                       );

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <param name="runTimeLang">Objective-C runtime version [Default=0]</param>
        /// <param name="uniqueId">A unique identifier for the type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DICompositeType CreateUnionType( DIScope? scope
                                       , LazyEncodedString name
                                       , DIFile? file
                                       , uint line
                                       , UInt64 bitSize
                                       , UInt32 bitAlign
                                       , DebugInfoFlags debugFlags
                                       , IEnumerable<DINode> elements
                                       , uint runTimeLang = 0
                                       , LazyEncodedString? uniqueId = null
                                       );

        /// <summary>Creates a <see cref="DIDerivedType"/> for a member of a type</summary>
        /// <param name="scope">Scope containing the member type</param>
        /// <param name="name">Name of the member type</param>
        /// <param name="file">File containing the member type</param>
        /// <param name="line">Line of the start of the member type</param>
        /// <param name="bitSize">Size of the member type in bits</param>
        /// <param name="bitAlign">Bit alignment of the member</param>
        /// <param name="bitOffset">Bit offset of the member</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the type</param>
        /// <param name="type">LLVM native type for the member type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        DIDerivedType CreateMemberType( DIScope? scope
                                      , LazyEncodedString name
                                      , DIFile? file
                                      , uint line
                                      , UInt64 bitSize
                                      , UInt32 bitAlign
                                      , UInt64 bitOffset
                                      , DebugInfoFlags debugFlags
                                      , DIType? type
                                      );

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts );

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[] subscripts );

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts );

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts );

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[] subscripts );

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts );

        /// <summary>Creates debug information for a type definition (e.g. type alias)</summary>
        /// <param name="type">Debug information for the aliased type</param>
        /// <param name="name">Name of the alias</param>
        /// <param name="file">File for the declaration of the typedef</param>
        /// <param name="line">line for the typedef</param>
        /// <param name="context">ContextAlias for creating the typedef</param>
        /// <param name="alignInBits">Bit alignment for the type</param>
        /// <returns><see cref="DIDerivedType"/>for the alias</returns>
        DIDerivedType CreateTypedef( DIType? type, LazyEncodedString name, DIFile? file, uint line, DINode? context, UInt32 alignInBits );

        /// <summary>Creates a new <see cref="DISubRange"/></summary>
        /// <param name="lowerBound">Lower bounds of the <see cref="DISubRange"/></param>
        /// <param name="count">Count of elements in the sub range</param>
        /// <returns><see cref="DISubRange"/></returns>
        DISubRange CreateSubRange( long lowerBound, long count );

        /// <summary>Gets or creates a node array with the specified elements</summary>
        /// <param name="elements">Elements of the array</param>
        /// <returns><see cref="DINodeArray"/></returns>
        /// <remarks>
        /// <note type="Note">
        /// As of LLVM 8.0 there's not much reason to manually construct a <see cref="DINodeArray"/>
        /// since use as an "in" parameter were superseded by overloads taking an actual array.
        /// </note>
        /// </remarks>
        DINodeArray GetOrCreateArray( IEnumerable<DINode> elements );

        /// <summary>Gets or creates a Type array with the specified types</summary>
        /// <param name="types">Types</param>
        /// <returns><see cref="DITypeArray"/></returns>
        DITypeArray GetOrCreateTypeArray( params IEnumerable<DIType> types );

        /// <summary>Creates a value for an enumeration</summary>
        /// <param name="name">Name of the value</param>
        /// <param name="value">Value of the enumerated value</param>
        /// <param name="isUnsigned">Indicates if the value is unsigned [Default: false]</param>
        /// <returns><see cref="DIEnumerator"/> for the name, value pair</returns>
        DIEnumerator CreateEnumeratorValue( LazyEncodedString name, long value, bool isUnsigned = false );

        /// <summary>Creates an enumeration type</summary>
        /// <param name="scope">Containing scope for the type</param>
        /// <param name="name">source language name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="file">Source file containing the type</param>
        /// <param name="lineNumber">Source file line number for the type</param>
        /// <param name="sizeInBits">Size, in bits, for the type</param>
        /// <param name="alignInBits">Alignment, in bits for the type</param>
        /// <param name="elements"><see cref="DIEnumerator"/> elements for the type</param>
        /// <param name="underlyingType">Underlying type for the enumerated type</param>
        /// <returns><see cref="DICompositeType"/> for the enumerated type</returns>
        DICompositeType CreateEnumerationType( DIScope? scope
                                             , LazyEncodedString name
                                             , DIFile? file
                                             , uint lineNumber
                                             , UInt64 sizeInBits
                                             , UInt32 alignInBits
                                             , IEnumerable<DIEnumerator> elements
                                             , DIType? underlyingType
                                             );

        /// <summary>Creates a new <see cref="DIGlobalVariableExpression"/></summary>
        /// <param name="scope">Scope for the expression</param>
        /// <param name="name">Source language name of the expression</param>
        /// <param name="linkageName">Linkage name of the expression</param>
        /// <param name="file">Source file for the expression</param>
        /// <param name="lineNo">Source Line number for the expression</param>
        /// <param name="type"><see cref="DIType"/> of the expression</param>
        /// <param name="isLocalToUnit">Flag to indicate if this is local to the compilation unit (e.g. static in C)</param>
        /// <param name="value"><see cref="Value"/> for the variable</param>
        /// <param name="declaration"><see cref="DINode"/> for the declaration of the variable</param>
        /// <param name="bitAlign">Bit alignment for the expression</param>
        /// <returns><see cref="DIGlobalVariableExpression"/> from the parameters</returns>
        DIGlobalVariableExpression CreateGlobalVariableExpression( DINode? scope
                                                                 , LazyEncodedString name
                                                                 , LazyEncodedString linkageName
                                                                 , DIFile? file
                                                                 , uint lineNo
                                                                 , DIType? type
                                                                 , bool isLocalToUnit
                                                                 , DIExpression? value
                                                                 , DINode? declaration = null
                                                                 , UInt32 bitAlign = 0
                                                                 );

        /// <summary>Finalizes debug information for a single <see cref="DISubProgram"/></summary>
        /// <param name="subProgram"><see cref="DISubProgram"/> to finalize debug information for</param>
        void Finish( DISubProgram subProgram );

        /// <summary>Finalizes debug information for all items built by this builder</summary>
        /// <remarks>
        /// <note type="note">
        ///  The term "finalize" here is in the context of LLVM rather than the .NET concept of Finalization.
        ///  In particular this will trigger resolving temporaries and will complete the list of locals for
        ///  any functions. So, the only nodes allowed after this is called are those that are fully resolved.
        /// </note>
        /// </remarks>
        void Finish( );

        /// <summary>Inserts an declare debug record for the given instruction</summary>
        /// <param name="storage">Value the declaration is bound to</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> for <paramref name="storage"/></param>
        /// <param name="location"><see cref="DILocation"/>for the variable</param>
        /// <param name="insertBefore"><see cref="Instructions.Instruction"/> to insert the declaration before</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the call to llvm.dbg.declare</returns>
        /// <remarks>
        /// This adds a call to the <see href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">llvm.dbg.declare</see> intrinsic.
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">LLVM: llvm.dbg.declare</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, Instruction insertBefore );

        /// <summary>Inserts an llvm.dbg.declare instruction before the given instruction</summary>
        /// <param name="storage">Value the declaration is bound to</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> for <paramref name="storage"/></param>
        /// <param name="expression"><see cref="DIExpression"/> for a debugger to use when extracting the value</param>
        /// <param name="location"><see cref="DILocation"/>for the variable</param>
        /// <param name="insertBefore"><see cref="Instructions.Instruction"/> to insert the declaration before</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the call to llvm.dbg.declare</returns>
        /// <remarks>
        /// This adds a call to the <see href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">llvm.dbg.declare</see> intrinsic.
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">LLVM: llvm.dbg.declare</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertDeclare( Value storage
                                 , DILocalVariable varInfo
                                 , DIExpression expression
                                 , DILocation location
                                 , Instruction insertBefore
                                 );

        /// <summary>Inserts a DebugRecord before the given instruction</summary>
        /// <param name="storage">Value the declaration is bound to</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> for <paramref name="storage"/></param>
        /// <param name="location"><see cref="DILocation"/>for the variable</param>
        /// <param name="insertAtEnd"><see cref="BasicBlock"/> to insert the declaration at the end of</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the call to llvm.dbg.declare</returns>
        /// <remarks>
        /// This adds a call to the <see href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">llvm.dbg.declare</see> intrinsic.
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">LLVM: llvm.dbg.declare</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd );

        /// <summary>Inserts a debug record before the given instruction</summary>
        /// <param name="storage">Value the declaration is bound to</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> for <paramref name="storage"/></param>
        /// <param name="expression"><see cref="DIExpression"/> for a debugger to use when extracting the value</param>
        /// <param name="location"><see cref="DILocation"/>for the variable</param>
        /// <param name="insertAtEnd"><see cref="BasicBlock"/> to insert the declaration at the end of</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the call to llvm.dbg.declare</returns>
        /// <remarks>
        /// This adds a call to the <see href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">llvm.dbg.declare</see> intrinsic.
        /// <note type="note">
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-declare">LLVM: llvm.dbg.declare</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, BasicBlock insertAtEnd );

        /// <summary>Inserts a debug record before the specified instruction</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertBefore">Location to insert the intrinsic</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the intrinsic</returns>
        /// <remarks>
        /// This intrinsic provides information when a user source variable is set to a new value.
        /// <note type="note">
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-value">LLVM: llvm.dbg.value</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertValue( Value value
                               , DILocalVariable varInfo
                               , DILocation location
                               , Instruction insertBefore
                               );

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic before the specified instruction</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="expression"><see cref="DIExpression"/> for the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertBefore">Location to insert the intrinsic</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the intrinsic</returns>
        /// <remarks>
        /// This intrinsic provides information when a user source variable is set to a new value.
        /// <note type="note">
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-value">LLVM: llvm.dbg.value</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        DebugRecord InsertValue( Value value
                               , DILocalVariable varInfo
                               , DIExpression? expression
                               , DILocation location
                               , Instruction insertBefore
                               );

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic at the end of a basic block</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertAtEnd">Block to append the intrinsic to the end of</param>
        /// <returns>The debug record</returns>
        /// <remarks>
        /// This intrinsic provides information when a user source variable is set to a new value.
        /// <note type="note">
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-value">LLVM: llvm.dbg.value</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        DebugRecord InsertValue( Value value
                               , DILocalVariable varInfo
                               , DILocation location
                               , BasicBlock insertAtEnd
                               );

        /// <summary>Inserts a DebugRecord at the end of a basic block</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="expression"><see cref="DIExpression"/> for the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertAtEnd">Block to append the intrinsic to the end of</param>
        /// <returns>The Debug record</returns>
        /// <remarks>
        /// This intrinsic provides information when a user source variable is set to a new value.
        /// <note type="note">
        /// The call has no impact on the actual machine code generated, as it is removed or ignored for actual target instruction
        /// selection. Instead, this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-value">LLVM: llvm.dbg.value</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        DebugRecord InsertValue( Value value
                               , DILocalVariable varInfo
                               , DIExpression? expression
                               , DILocation location
                               , BasicBlock insertAtEnd
                               );

        /// <summary>Creates a <see cref="DIExpression"/> from the provided <see cref="ExpressionOp"/>s</summary>
        /// <param name="operations">Operation sequence for the expression</param>
        /// <returns><see cref="DIExpression"/></returns>
        DIExpression CreateExpression( params IEnumerable<ExpressionOp> operations );

        /// <summary>Creates a <see cref="DIExpression"/> for a constant value</summary>
        /// <param name="value">Value of the expression</param>
        /// <returns><see cref="DIExpression"/></returns>
        DIExpression CreateConstantValueExpression( UInt64 value );

        /// <summary>Creates a replaceable composite type</summary>
        /// <param name="tag">Debug information <see cref="Tag"/> for the composite type (only values for a composite type are allowed)</param>
        /// <param name="name">Name of the type (use <see cref="string.Empty"/> for anonymous types)</param>
        /// <param name="scope">Scope of the type</param>
        /// <param name="file">Source file for the type</param>
        /// <param name="line">Source line for the type</param>
        /// <param name="lang">Source language the type is defined in</param>
        /// <param name="sizeInBits">size of the type in bits</param>
        /// <param name="alignBits">alignment of the type in bits</param>
        /// <param name="flags"><see cref="DebugInfoFlags"/> for the type</param>
        /// <param name="uniqueId">Unique identifier for the type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        DICompositeType CreateReplaceableCompositeType( Tag tag
                                                      , LazyEncodedString name
                                                      , DIScope? scope
                                                      , DIFile? file
                                                      , uint line
                                                      , uint lang = 0
                                                      , UInt64 sizeInBits = 0
                                                      , UInt32 alignBits = 0
                                                      , DebugInfoFlags flags = DebugInfoFlags.None
                                                      , LazyEncodedString? uniqueId = null
                                                      );
    }

    internal static class IDIBuilderExtensions
    {
        internal static LLVMDIBuilderRefAlias GetUnownedHandle( this IDIBuilder self )
        {
            if(self is IHandleWrapper<LLVMDIBuilderRefAlias> wrapper)
            {
                return wrapper.Handle;
            }
            else if(self is IGlobalHandleOwner<LLVMDIBuilderRef> owner)
            {
                // implicitly cast to the alias handle
                return owner.OwnedHandle;
            }
            else
            {
                throw new ArgumentException( "Internal Error - Unknown context type!", nameof( self ) );
            }
        }
    }
}
