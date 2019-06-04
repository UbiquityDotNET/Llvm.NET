﻿// -----------------------------------------------------------------------
// <copyright file="DebugInfoBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Llvm.NET.Instructions;
using Llvm.NET.Interop;
using Llvm.NET.Properties;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Defines the amount of debug information to emit</summary>
    public enum DwarfEmissionKind
    {
        /// <summary>No debug information</summary>
        None = 0,

        /// <summary>Full Debug information</summary>
        Full,

        /// <summary>Emit line tables only</summary>
        LineTablesOnly
    }

    /// <summary>DebugInfoBuilder is a factory class for creating DebugInformation for an LLVM <see cref="BitcodeModule"/></summary>
    /// <remarks>
    /// Many Debug information metadata nodes are created with unresolved references to additional
    /// metadata. To ensure such metadata is resolved applications should call the <see cref="Finish()"/>
    /// method to resolve and finalize the metadata. After this point only fully resolved nodes may
    /// be added to ensure that the data remains valid.
    /// </remarks>
    /// <seealso href="xref:llvm_sourceleveldebugging">LLVM Source Level Debugging</seealso>
    public sealed class DebugInfoBuilder
    {
        /// <summary>Gets the module that owns this builder</summary>
        public BitcodeModule OwningModule { get; }

        /// <summary>Creates a new <see cref="DICompileUnit"/></summary>
        /// <param name="language"><see cref="SourceLanguage"/> for the compilation unit</param>
        /// <param name="sourceFilePath">Full path to the source file of this compilation unit</param>
        /// <param name="producer">Name of the application processing the compilation unit</param>
        /// <param name="optimized">Flag to indicate if the code in this compilation unit is optimized</param>
        /// <param name="compilationFlags">Additional tool specific flags</param>
        /// <param name="runtimeVersion">Runtime version</param>
        /// <returns><see cref="DICompileUnit"/></returns>
        public DICompileUnit CreateCompileUnit( SourceLanguage language
                                              , string sourceFilePath
                                              , string producer
                                              , bool optimized
                                              , string compilationFlags
                                              , uint runtimeVersion
                                              )
        {
            return CreateCompileUnit( language
                                    , Path.GetFileName( sourceFilePath )
                                    , Path.GetDirectoryName( sourceFilePath ) ?? Environment.CurrentDirectory
                                    , producer
                                    , optimized
                                    , compilationFlags
                                    , runtimeVersion
                                    );
        }

        /// <summary>Creates a new <see cref="DICompileUnit"/></summary>
        /// <param name="language"><see cref="SourceLanguage"/> for the compilation unit</param>
        /// <param name="fileName">Name of the source file of this compilation unit (without any path)</param>
        /// <param name="fileDirectory">Path of the directory containing the file</param>
        /// <param name="producer">Name of the application processing the compilation unit</param>
        /// <param name="optimized">Flag to indicate if the code in this compilation unit is optimized</param>
        /// <param name="compilationFlags">Additional tool specific flags</param>
        /// <param name="runtimeVersion">Runtime version</param>
        /// <returns><see cref="DICompileUnit"/></returns>
        [SuppressMessage( "Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DICompileUnit", Justification = "It is spelled correctly 8^)" )]
        public DICompileUnit CreateCompileUnit( SourceLanguage language
                                              , string fileName
                                              , string fileDirectory
                                              , string producer
                                              , bool optimized
                                              , string compilationFlags
                                              , uint runtimeVersion
                                              )
        {
            if( OwningModule.DICompileUnit != null )
            {
                throw new InvalidOperationException( Resources.LLVM_only_allows_one_DICompileUnit_per_module );
            }

            var file = CreateFile( fileName, fileDirectory );
            var handle = LibLLVMDIBuilderCreateCompileUnit( BuilderHandle
                                                          , ( LibLLVMDwarfSourceLanguage )language
                                                          , file.MetadataHandle
                                                          , producer
                                                          , producer.Length
                                                          , optimized
                                                          , compilationFlags
                                                          , compilationFlags.Length
                                                          , runtimeVersion
                                                          , string.Empty
                                                          , size_t.Zero
                                                          , LLVMDWARFEmissionKind.LLVMDWARFEmissionFull
                                                          , 0
                                                          , false
                                                          , false
                                                          );
            var retVal = MDNode.FromHandle<DICompileUnit>( handle );
            OwningModule.DICompileUnit = retVal;
            return retVal;
        }

        /// <summary>Creates a <see cref="DINamespace"/></summary>
        /// <param name="scope">Containing scope for the namespace or null if the namespace is a global one</param>
        /// <param name="name">Name of the namespace</param>
        /// <param name="exportSymbols">export symbols</param>
        /// <returns>Debug namespace</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DINamespace CreateNamespace( DIScope scope, string name, bool exportSymbols )
        {
            name.ValidateNotNullOrWhiteSpace( nameof( name ) );

            var handle = LLVMDIBuilderCreateNameSpace( BuilderHandle
                                                     , scope?.MetadataHandle ?? default
                                                     , name
                                                     , name.Length
                                                     , exportSymbols
                                                     );

            return MDNode.FromHandle<DINamespace>( handle );
        }

        /// <summary>Creates a <see cref="DIFile"/></summary>
        /// <param name="path">Path of the file (may be <see langword="null"/> or empty)</param>
        /// <returns>
        /// <see cref="DIFile"/> or <see langword="null"/> if <paramref name="path"/>
        /// is <see langword="null"/> empty, or all whitespace
        /// </returns>
        public DIFile CreateFile( string path )
        {
            return string.IsNullOrWhiteSpace( path ) ? null : CreateFile( Path.GetFileName( path ), Path.GetDirectoryName( path ) );
        }

        /// <summary>Creates a <see cref="DIFile"/></summary>
        /// <param name="fileName">Name of the file (may be <see langword="null"/> or empty)</param>
        /// <param name="directory">Path of the directory containing the file (may be <see langword="null"/> or empty)</param>
        /// <returns>
        /// <see cref="DIFile"/> or <see langword="null"/> if <paramref name="fileName"/>
        /// is <see langword="null"/> empty, or all whitespace
        /// </returns>
        public DIFile CreateFile( string fileName, string directory )
        {
            if( string.IsNullOrWhiteSpace( fileName ) )
            {
                return null;
            }

            var handle = LLVMDIBuilderCreateFile( BuilderHandle
                                                , fileName
                                                , fileName.Length
                                                , directory ?? string.Empty
                                                , ( long )( directory?.Length ?? 0 )
                                                );
            return MDNode.FromHandle<DIFile>( handle );
        }

        /// <summary>Creates a new <see cref="DILexicalBlock"/></summary>
        /// <param name="scope"><see cref="DIScope"/> for the block</param>
        /// <param name="file"><see cref="DIFile"/> containing the block</param>
        /// <param name="line">Starting line number for the block</param>
        /// <param name="column">Starting column for the block</param>
        /// <returns>
        /// <see cref="DILexicalBlock"/> created from the parameters
        /// </returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlock CreateLexicalBlock( DIScope scope, DIFile file, uint line, uint column )
        {
            scope.ValidateNotNull( nameof( scope ) );

            var handle = LLVMDIBuilderCreateLexicalBlock( BuilderHandle
                                                        , scope.MetadataHandle
                                                        , file?.MetadataHandle ?? default
                                                        , line
                                                        , column
                                                        );

            return MDNode.FromHandle<DILexicalBlock>( handle );
        }

        /// <summary>Creates a <see cref="DILexicalBlockFile"/></summary>
        /// <param name="scope"><see cref="DIScope"/> for the block</param>
        /// <param name="file"><see cref="DIFile"/></param>
        /// <param name="discriminator">Discriminator to disambiguate lexical blocks with the same file info</param>
        /// <returns>
        /// <see cref="DILexicalBlockFile"/> constructed from the parameters
        /// </returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlockFile CreateLexicalBlockFile( DIScope scope, DIFile file, uint discriminator )
        {
            scope.ValidateNotNull( nameof( scope ) );
            file.ValidateNotNull( nameof( file ) );

            var handle = LLVMDIBuilderCreateLexicalBlockFile( BuilderHandle, scope.MetadataHandle, file.MetadataHandle, discriminator );
            return MDNode.FromHandle<DILexicalBlockFile>( handle );
        }

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
        /// <param name="function">Underlying LLVM <see cref="IrFunction"/> to attach debug info to</param>
        /// <returns><see cref="DISubProgram"/> created based on the input parameters</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DISubProgram CreateFunction( DIScope scope
                                          , [CanBeNull] string name
                                          , [CanBeNull] string mangledName
                                          , [CanBeNull] DIFile file
                                          , uint line
                                          , DISubroutineType signatureType
                                          , bool isLocalToUnit
                                          , bool isDefinition
                                          , uint scopeLine
                                          , DebugInfoFlags debugFlags
                                          , bool isOptimized
                                          , IrFunction function
                                          )
        {
            scope.ValidateNotNull( nameof( scope ) );
            signatureType.ValidateNotNull( nameof( signatureType ) );
            function.ValidateNotNull( nameof( function ) );

            if( string.IsNullOrWhiteSpace( name ) )
            {
                name = string.Empty;
            }

            if( string.IsNullOrWhiteSpace( mangledName ) )
            {
                mangledName = string.Empty;
            }

            var handle = LLVMDIBuilderCreateFunction( BuilderHandle
                                                    , scope.MetadataHandle
                                                    , name
                                                    , name.Length
                                                    , mangledName
                                                    , mangledName.Length
                                                    , file?.MetadataHandle ?? default
                                                    , line
                                                    , signatureType.MetadataHandle
                                                    , isLocalToUnit
                                                    , isDefinition
                                                    , scopeLine
                                                    , ( LLVMDIFlags )debugFlags
                                                    , isOptimized
                                                    );
            return MDNode.FromHandle<DISubProgram>( handle );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DISubProgram ForwardDeclareFunction( DIScope scope
                                                  , string name
                                                  , string mangledName
                                                  , DIFile file
                                                  , uint line
                                                  , DISubroutineType subroutineType
                                                  , bool isLocalToUnit
                                                  , bool isDefinition
                                                  , uint scopeLine
                                                  , DebugInfoFlags debugFlags
                                                  , bool isOptimized
                                                  )
        {
            scope.ValidateNotNull( nameof( scope ) );
            subroutineType.ValidateNotNull( nameof( subroutineType ) );

            if( string.IsNullOrWhiteSpace( name ) )
            {
                name = string.Empty;
            }

            if( string.IsNullOrWhiteSpace( mangledName ) )
            {
                mangledName = string.Empty;
            }

            var handle = LibLLVMDIBuilderCreateTempFunctionFwdDecl( BuilderHandle
                                                                  , scope.MetadataHandle
                                                                  , name
                                                                  , name.Length
                                                                  , mangledName
                                                                  , mangledName.Length
                                                                  , file?.MetadataHandle ?? default
                                                                  , line
                                                                  , subroutineType.MetadataHandle
                                                                  , isLocalToUnit
                                                                  , isDefinition
                                                                  , scopeLine
                                                                  , ( LLVMDIFlags )debugFlags
                                                                  , isOptimized
                                                                  );
            return MDNode.FromHandle<DISubProgram>( handle );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILocalVariable CreateLocalVariable( DIScope scope
                                                  , string name
                                                  , DIFile file
                                                  , uint line
                                                  , DIType type
                                                  , bool alwaysPreserve
                                                  , DebugInfoFlags debugFlags
                                                  , uint alignInBits = 0
                                                  )
        {
            scope.ValidateNotNull( nameof( scope ) );
            type.ValidateNotNull( nameof( type ) );

            var handle = LLVMDIBuilderCreateAutoVariable( BuilderHandle
                                                        , scope.MetadataHandle
                                                        , name
                                                        , name.Length
                                                        , file?.MetadataHandle ?? default
                                                        , line
                                                        , type.MetadataHandle
                                                        , alwaysPreserve
                                                        , ( LLVMDIFlags )debugFlags
                                                        , alignInBits
                                                        );
            return MDNode.FromHandle<DILocalVariable>( handle );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILocalVariable CreateArgument( DIScope scope
                                             , string name
                                             , DIFile file
                                             , uint line
                                             , DIType type
                                             , bool alwaysPreserve
                                             , DebugInfoFlags debugFlags
                                             , ushort argNo
                                             )
        {
            scope.ValidateNotNull( nameof( scope ) );
            type.ValidateNotNull( nameof( type ) );

            var handle = LLVMDIBuilderCreateParameterVariable( BuilderHandle
                                                             , scope.MetadataHandle
                                                             , name
                                                             , name.Length
                                                             , argNo
                                                             , file?.MetadataHandle ?? default
                                                             , line
                                                             , type.MetadataHandle
                                                             , alwaysPreserve
                                                             , ( LLVMDIFlags )debugFlags
                                                             );
            return MDNode.FromHandle<DILocalVariable>( handle );
        }

        /// <summary>Construct debug information for a basic type (a.k.a. primitive type)</summary>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size for the type</param>
        /// <param name="encoding"><see cref="DiTypeKind"/> encoding for the type</param>
        /// <param name="diFlags"><see cref="DebugInfoFlags"/> for the type</param>
        /// <returns>Basic type debugging information</returns>
        public DIBasicType CreateBasicType( string name, UInt64 bitSize, DiTypeKind encoding, DebugInfoFlags diFlags = DebugInfoFlags.None )
        {
            var handle = LLVMDIBuilderCreateBasicType( BuilderHandle, name, name.Length, bitSize, ( uint )encoding, ( LLVMDIFlags )diFlags );
            return MDNode.FromHandle<DIBasicType>( handle );
        }

        /// <summary>Creates a pointer type with debug information</summary>
        /// <param name="pointeeType">base type of the pointer</param>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size of the type</param>
        /// <param name="bitAlign">But alignment of the type</param>
        /// <param name="addressSpace">Address space for the pointer</param>
        /// <returns>Pointer type</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreatePointerType( DIType pointeeType, string name, UInt64 bitSize, UInt32 bitAlign = 0, uint addressSpace = 0)
        {
            var handle = LLVMDIBuilderCreatePointerType( BuilderHandle
                                                       , pointeeType?.MetadataHandle ?? default // null == void
                                                       , bitSize
                                                       , bitAlign
                                                       , addressSpace
                                                       , name ?? string.Empty
                                                       , name?.Length ?? 0
                                                       );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Creates a qualified type</summary>
        /// <param name="baseType">Base type to add the qualifier to</param>
        /// <param name="tag">Qualifier to apply</param>
        /// <returns>Qualified type</returns>
        /// <exception cref="System.ArgumentException"><paramref name="tag"/> is <see cref="QualifiedTypeTag.None"/></exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="baseType"/> is <see langword="null"/></exception>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateQualifiedType( DIType baseType, QualifiedTypeTag tag )
        {
            baseType.ValidateNotNull( nameof( baseType ) );

            var handle = LLVMDIBuilderCreateQualifiedType( BuilderHandle, ( uint )tag, baseType.MetadataHandle );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Create a debug metadata array of debug types</summary>
        /// <param name="types">Types to include in the array</param>
        /// <returns>Array containing the types</returns>
        public DITypeArray CreateTypeArray( params DIType[ ] types ) => CreateTypeArray( ( IEnumerable<DIType> )types );

        /// <summary>Create a debug metadata array of debug types</summary>
        /// <param name="types">Types to include in the array</param>
        /// <returns>Array containing the types</returns>
        public DITypeArray CreateTypeArray( IEnumerable<DIType> types )
        {
            var handles = types.Select( t => t.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, handles, handles.LongLength );
            return new DITypeArray( MDNode.FromHandle<MDTuple>( handle ) );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        [Obsolete("Use overload supporting Array or IEnumerable of DIType")]
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DITypeArray types )
        {
            return CreateSubroutineType( debugFlags, types.AsEnumerable( ) );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, params DIType[ ] types )
        {
            return CreateSubroutineType( debugFlags, ( IEnumerable<DIType> )types );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, IEnumerable<DIType> types )
        {
            types.ValidateNotNull( nameof( types ) );
            var handles = types.Select( t => t.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateSubroutineType( BuilderHandle
                                                          , LLVMMetadataRef.Zero
                                                          , handles
                                                          , checked(( uint )handles.Length)
                                                          , ( LLVMDIFlags )debugFlags
                                                          );

            return MDNode.FromHandle<DISubroutineType>( handle );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags )
        {
            return CreateSubroutineType( debugFlags, Array.Empty<DIType>() );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="returnType">Return type of the signature</param>
        /// <param name="types">Parameters for the function</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType returnType, IEnumerable<DIType> types )
        {
            return CreateSubroutineType( debugFlags, returnType != null ? types.Prepend( returnType ) : types );
        }

        /// <summary>Creates debug description of a structure type</summary>
        /// <param name="scope">Scope containing the structure</param>
        /// <param name="name">Name of the type</param>
        /// <param name="file">File containing the type</param>
        /// <param name="line">Line of the start of the type</param>
        /// <param name="bitSize">Size of the type in bits</param>
        /// <param name="bitAlign">Bit alignment of the type</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the structure</param>
        /// <param name="derivedFrom"><see cref="DIType"/> this type is derived from, if any</param>
        /// <param name="elements">Node array describing the elements of the structure</param>
        /// <returns><see cref="DICompositeType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        [Obsolete("Use overload accepting array or IEnumerable of DINode")]
        public DICompositeType CreateStructType( DIScope scope
                                               , string name
                                               , DIFile file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , DebugInfoFlags debugFlags
                                               , [CanBeNull] DIType derivedFrom
                                               , DINodeArray elements
                                               )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, (IEnumerable<DINode>)elements );
        }

        /// <summary>Creates debug description of a structure type</summary>
        /// <param name="scope">Scope containing the structure</param>
        /// <param name="name">Name of the type</param>
        /// <param name="file">File containing the type</param>
        /// <param name="line">Line of the start of the type</param>
        /// <param name="bitSize">Size of the type in bits</param>
        /// <param name="bitAlign">Bit alignment of the type</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the structure</param>
        /// <param name="derivedFrom"><see cref="DIType"/> this type is derived from, if any</param>
        /// <param name="elements">Node array describing the elements of the structure</param>
        /// <returns><see cref="DICompositeType"/></returns>
        public DICompositeType CreateStructType( DIScope scope
                                               , string name
                                               , DIFile file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , DebugInfoFlags debugFlags
                                               , DIType derivedFrom
                                               , params DINode[ ] elements
                                               )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, (IEnumerable<DINode>)( elements ) );
        }

        /// <summary>Creates debug description of a structure type</summary>
        /// <param name="scope">Scope containing the structure</param>
        /// <param name="name">Name of the type</param>
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
        public DICompositeType CreateStructType( DIScope scope
                                               , string name
                                               , DIFile file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , DebugInfoFlags debugFlags
                                               , DIType derivedFrom
                                               , IEnumerable<DINode> elements
                                               , uint runTimeLang = 0
                                               , [CanBeNull] LlvmMetadata vTableHolder = null
                                               , string uniqueId = ""
                                               )
        {
            scope.ValidateNotNull( nameof( scope ) );
            elements.ValidateNotNull( nameof( elements ) );
            var elementHandles = elements.Select( e => e.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateStructType( BuilderHandle
                                                      , scope.MetadataHandle
                                                      , name
                                                      , name.Length
                                                      , file?.MetadataHandle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , ( LLVMDIFlags )debugFlags
                                                      , derivedFrom?.MetadataHandle ?? default
                                                      , elementHandles
                                                      , (uint)elementHandles.Length
                                                      , runTimeLang
                                                      , vTableHolder?.MetadataHandle ?? default
                                                      , uniqueId ?? string.Empty
                                                      , uniqueId?.Length ?? 0
                                                      );

            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <returns><see cref="DICompositeType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateUnionType( DIScope scope
                                              , string name
                                              , DIFile file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , DebugInfoFlags debugFlags
                                              , DINodeArray elements
                                              )
        {
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, ( IEnumerable<DINode> )elements );
        }

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <returns><see cref="DICompositeType"/></returns>
        public DICompositeType CreateUnionType( DIScope scope
                                              , string name
                                              , DIFile file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , DebugInfoFlags debugFlags
                                              , params DINode[ ] elements
                                              )
        {
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, (IEnumerable<DINode>)elements);
        }

        /// <summary>Creates debug description of a union type</summary>
        /// <param name="scope">Scope containing the union</param>
        /// <param name="name">Name of the type</param>
        /// <param name="file">File containing the union</param>
        /// <param name="line">Line of the start of the union</param>
        /// <param name="bitSize">Size of the union in bits</param>
        /// <param name="bitAlign">Bit alignment of the union</param>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for the union</param>
        /// <param name="elements">Node array describing the elements of the union</param>
        /// <param name="runTimeLang">Objective-C runtime version [Default=0]</param>
        /// <param name="uniqueId">A unique identifier for the type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        public DICompositeType CreateUnionType( DIScope scope
                                               , string name
                                               , DIFile file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , DebugInfoFlags debugFlags
                                               , IEnumerable<DINode> elements
                                               , uint runTimeLang = 0
                                               , string uniqueId = ""
                                               )
        {
            scope.ValidateNotNull( nameof( scope ) );
            elements.ValidateNotNull( nameof( elements ) );
            var elementHandles = elements.Select( e => e.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateUnionType( BuilderHandle
                                                     , scope.MetadataHandle
                                                     , name
                                                     , name.Length
                                                     , file?.MetadataHandle ?? default
                                                     , line
                                                     , bitSize
                                                     , bitAlign
                                                     , ( LLVMDIFlags )debugFlags
                                                     , elementHandles
                                                     , (uint)elementHandles.Length
                                                     , runTimeLang
                                                     , uniqueId ?? string.Empty
                                                     , uniqueId?.Length ?? 0
                                                     );

            return MDNode.FromHandle<DICompositeType>( handle );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateMemberType( DIScope scope
                                             , string name
                                             , DIFile file
                                             , uint line
                                             , UInt64 bitSize
                                             , UInt32 bitAlign
                                             , UInt64 bitOffset
                                             , DebugInfoFlags debugFlags
                                             , DIType type
                                             )
        {
            scope.ValidateNotNull( nameof( scope ) );
            type.ValidateNotNull( nameof( type ) );

            var handle = LLVMDIBuilderCreateMemberType( BuilderHandle
                                                      , scope.MetadataHandle
                                                      , name
                                                      , name.Length
                                                      , file?.MetadataHandle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , bitOffset
                                                      , ( LLVMDIFlags )debugFlags
                                                      , type.MetadataHandle
                                                      );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, ( IEnumerable<DINode> )subscripts );
        }

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[ ] subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            elementType.ValidateNotNull( nameof( elementType ) );
            subscripts.ValidateNotNull( nameof( subscripts ) );

            var subScriptHandles = subscripts.Select( s => s.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateArrayType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subScriptHandles, (uint)subScriptHandles.Length );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return CreateVectorType( bitSize, bitAlign, elementType, ( IEnumerable<DINode> )subscripts );
        }

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[ ] subscripts )
        {
            return CreateVectorType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <summary>Creates debug information for a vector type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the Vector</param>
        /// <param name="subscripts">Dimensions for the Vector</param>
        /// <returns><see cref="DICompositeType"/> for the Vector</returns>
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            elementType.ValidateNotNull( nameof( elementType ) );
            subscripts.ValidateNotNull( nameof( subscripts ) );

            var subScriptHandles = subscripts.Select( s => s.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateVectorType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subScriptHandles, (uint)subScriptHandles.Length );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug information for a type definition (e.g. type alias)</summary>
        /// <param name="type">Debug information for the aliased type</param>
        /// <param name="name">Name of the alias</param>
        /// <param name="file">File for the declaration of the typedef</param>
        /// <param name="line">line for the typedef</param>
        /// <param name="context">Context for creating the typedef</param>
        /// <returns><see cref="DIDerivedType"/>for the alias</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateTypedef( DIType type, string name, DIFile file, uint line, DINode context )
        {
            var handle = LLVMDIBuilderCreateTypedef( BuilderHandle
                                                   , type?.MetadataHandle ?? default
                                                   , name
                                                   , name.Length
                                                   , file?.MetadataHandle ?? default
                                                   , line
                                                   , context?.MetadataHandle ?? default
                                                   );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Creates a new <see cref="DISubRange"/></summary>
        /// <param name="lo">Lower bounds of the <see cref="DISubRange"/></param>
        /// <param name="count">Count of elements in the sub range</param>
        /// <returns><see cref="DISubRange"/></returns>
        public DISubRange CreateSubRange( long lo, long count )
        {
            var handle = LLVMDIBuilderGetOrCreateSubrange( BuilderHandle, lo, count );
            return MDNode.FromHandle<DISubRange>( handle );
        }

        /// <summary>Gets or creates a node array with the specified elements</summary>
        /// <param name="elements">Elements of the array</param>
        /// <returns><see cref="DINodeArray"/></returns>
        /// <remarks>
        /// <note type="Note">
        /// As of LLVM 8.0 there's not much reason to manually construct a <see cref="DINodeArray"/>
        /// since use as an "in" parameter were superseded by overloads taking an actual array.
        /// </note>
        /// </remarks>
        public DINodeArray GetOrCreateArray( IEnumerable<DINode> elements )
        {
            var buf = elements.Select( d => d?.MetadataHandle ?? default ).ToArray( );
            long actualLen = buf.LongLength;

            var handle = LLVMDIBuilderGetOrCreateArray( BuilderHandle, buf, buf.LongLength );
            return new DINodeArray( LlvmMetadata.FromHandle<MDTuple>( OwningModule.Context, handle ) );
        }

        /// <summary>Gets or creates a Type array with the specified types</summary>
        /// <param name="types">Types</param>
        /// <returns><see cref="DITypeArray"/></returns>
        public DITypeArray GetOrCreateTypeArray( params DIType[ ] types ) => GetOrCreateTypeArray( ( IEnumerable<DIType> )types );

        /// <summary>Gets or creates a Type array with the specified types</summary>
        /// <param name="types">Types</param>
        /// <returns><see cref="DITypeArray"/></returns>
        public DITypeArray GetOrCreateTypeArray( IEnumerable<DIType> types )
        {
            var buf = types.Select( t => t?.MetadataHandle ?? default ).ToArray( );
            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, buf, buf.LongLength );
            return new DITypeArray( MDNode.FromHandle<MDTuple>( handle ) );
        }

        /// <summary>Creates a value for an enumeration</summary>
        /// <param name="name">Name of the value</param>
        /// <param name="value">Value of the enumerated value</param>
        /// <returns><see cref="DIEnumerator"/> for the name, value pair</returns>
        public DIEnumerator CreateEnumeratorValue( string name, long value )
        {
            var handle = LibLLVMDIBuilderCreateEnumeratorValue( BuilderHandle, name, value );
            return MDNode.FromHandle<DIEnumerator>( handle );
        }

        /// <summary>Creates an enumeration type</summary>
        /// <param name="scope">Containing scope for the type</param>
        /// <param name="name">source language name of the type</param>
        /// <param name="file">Source file containing the type</param>
        /// <param name="lineNumber">Source file line number for the type</param>
        /// <param name="sizeInBits">Size, in bits, for the type</param>
        /// <param name="alignInBits">Alignment, in bits for the type</param>
        /// <param name="elements"><see cref="DIEnumerator"/> elements for the type</param>
        /// <param name="underlyingType">Underlying type for the enumerated type</param>
        /// <returns><see cref="DICompositeType"/> for the enumerated type</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateEnumerationType( DIScope scope
                                                    , string name
                                                    , DIFile file
                                                    , uint lineNumber
                                                    , UInt64 sizeInBits
                                                    , UInt32 alignInBits
                                                    , IEnumerable<DIEnumerator> elements
                                                    , DIType underlyingType
                                                    )
        {
            scope.ValidateNotNull( nameof( scope ) );
            underlyingType.ValidateNotNull( nameof( underlyingType ) );

            var elementHandles = elements.Select( e => e.MetadataHandle ).ToArray( );
            var handle = LLVMDIBuilderCreateEnumerationType( BuilderHandle
                                                           , scope.MetadataHandle
                                                           , name
                                                           , name.Length
                                                           , file?.MetadataHandle ?? default
                                                           , lineNumber
                                                           , sizeInBits
                                                           , alignInBits
                                                           , elementHandles
                                                           , checked(( uint )elementHandles.Length)
                                                           , underlyingType.MetadataHandle
                                                           );

            return MDNode.FromHandle<DICompositeType>( handle );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIGlobalVariableExpression CreateGlobalVariableExpression( DINode scope
                                                                        , string name
                                                                        , string linkageName
                                                                        , DIFile file
                                                                        , uint lineNo
                                                                        , DIType type
                                                                        , bool isLocalToUnit
                                                                        , DIExpression value
                                                                        , DINode declaration = null
                                                                        , UInt32 bitAlign = 0
                                                                        )
        {
            scope.ValidateNotNull( nameof( scope ) );
            type.ValidateNotNull( nameof( type ) );

            var handle = LLVMDIBuilderCreateGlobalVariableExpression( BuilderHandle
                                                                    , scope.MetadataHandle
                                                                    , name
                                                                    , name.Length
                                                                    , linkageName
                                                                    , linkageName.Length
                                                                    , file?.MetadataHandle ?? default
                                                                    , lineNo
                                                                    , type.MetadataHandle
                                                                    , isLocalToUnit
                                                                    , value?.MetadataHandle ?? default
                                                                    , declaration?.MetadataHandle ?? default
                                                                    , bitAlign
                                                                    );
            return MDNode.FromHandle<DIGlobalVariableExpression>( handle );
        }

        /// <summary>Finalizes debug information for a single <see cref="DISubProgram"/></summary>
        /// <param name="subProgram"><see cref="DISubProgram"/> to finalize debug information for</param>
        public void Finish( DISubProgram subProgram )
        {
            LibLLVMDIBuilderFinalizeSubProgram( BuilderHandle, subProgram.MetadataHandle );
        }

        /// <summary>Finalizes debug information for all items built by this builder</summary>
        /// <remarks>
        /// <note type="note">
        ///  The term "finalize" here is in the context of LLVM rather than the .NET concept of Finalization.
        ///  In particular this will trigger resolving temporaries and will complete the list of locals for
        ///  any functions. So, the only nodes allowed after this is called are those that are fully resolved.
        /// </note>
        /// </remarks>
        public void Finish( )
        {
            if( IsFinished )
            {
                return;
            }

            var bldr = new StringBuilder( );
            var unresolvedTemps = from node in OwningModule.Context.Metadata.OfType<MDNode>( )
                                  where node.IsTemporary && !node.IsResolved
                                  select node;

            foreach( MDNode node in unresolvedTemps )
            {
                if( bldr.Length == 0 )
                {
                    bldr.AppendLine( Resources.Temporaries_must_be_resolved_before_finalizing_debug_information );
                }

                bldr.AppendFormat( CultureInfo.CurrentCulture, Resources.Unresolved_Debug_temporary_0, node );
                bldr.AppendLine( );
            }

            if( bldr.Length > 0 )
            {
                throw new InvalidOperationException( bldr.ToString( ) );
            }

            LLVMDIBuilderFinalize( BuilderHandle );
            IsFinished = true;
        }

        /// <summary>Inserts an llvm.dbg.declare instruction before the given instruction</summary>
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
        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, Instruction insertBefore )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), location, insertBefore );
        }

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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, Instruction insertBefore )
        {
            storage.ValidateNotNull( nameof( storage ) );
            varInfo.ValidateNotNull( nameof( varInfo ) );
            expression.ValidateNotNull( nameof( expression ) );
            location.ValidateNotNull( nameof( location ) );
            insertBefore.ValidateNotNull( nameof( insertBefore ) );

            var handle = LLVMDIBuilderInsertDeclareBefore( BuilderHandle
                                                         , storage.ValueHandle
                                                         , varInfo.MetadataHandle
                                                         , expression.MetadataHandle
                                                         , location.MetadataHandle
                                                         , insertBefore.ValueHandle
                                                         );

            return Value.FromHandle<CallInstruction>( handle );
        }

        /// <summary>Inserts an llvm.dbg.declare instruction before the given instruction</summary>
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
        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), location, insertAtEnd );
        }

        /// <summary>Inserts an llvm.dbg.declare instruction before the given instruction</summary>
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
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, BasicBlock insertAtEnd )
        {
            storage.ValidateNotNull( nameof( storage ) );
            varInfo.ValidateNotNull( nameof( varInfo ) );
            expression.ValidateNotNull( nameof( expression ) );
            location.ValidateNotNull( nameof( location ) );
            insertAtEnd.ValidateNotNull( nameof( insertAtEnd ) );

            if( location.Scope.SubProgram != varInfo.Scope.SubProgram )
            {
                throw new ArgumentException( Resources.Mismatched_scopes_for_location_and_variable );
            }

            var handle = LLVMDIBuilderInsertDeclareAtEnd( BuilderHandle
                                                        , storage.ValueHandle
                                                        , varInfo.MetadataHandle
                                                        , expression.MetadataHandle
                                                        , location.MetadataHandle
                                                        , insertAtEnd.BlockHandle
                                                        );
            return Value.FromHandle<CallInstruction>( handle );
        }

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic before the specified instruction</summary>
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
        public CallInstruction InsertValue( Value value
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , Instruction insertBefore
                                          )
        {
            return InsertValue( value, varInfo, null, location, insertBefore );
        }

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
        public CallInstruction InsertValue( Value value
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , Instruction insertBefore
                                          )
        {
            value.ValidateNotNull( nameof( value ) );
            varInfo.ValidateNotNull( nameof( varInfo ) );
            expression.ValidateNotNull( nameof( expression ) );
            location.ValidateNotNull( nameof( location ) );
            insertBefore.ValidateNotNull( nameof( insertBefore ) );

            var handle = LLVMDIBuilderInsertDbgValueBefore( BuilderHandle
                                                          , value.ValueHandle
                                                          , varInfo.MetadataHandle
                                                          , expression?.MetadataHandle ?? CreateExpression( ).MetadataHandle
                                                          , location.MetadataHandle
                                                          , insertBefore.ValueHandle
                                                          );
            var retVal = Value.FromHandle<CallInstruction>( handle );
            retVal.IsTailCall = true;
            return retVal;
        }

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic at the end of a basic block</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertAtEnd">Block to append the intrinsic to the end of</param>
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
        public CallInstruction InsertValue( Value value
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            return InsertValue( value, varInfo, null, location, insertAtEnd );
        }

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic at the end of a basic block</summary>
        /// <param name="value">New value</param>
        /// <param name="varInfo"><see cref="DILocalVariable"/> describing the variable</param>
        /// <param name="expression"><see cref="DIExpression"/> for the variable</param>
        /// <param name="location"><see cref="DILocation"/>for the assignment</param>
        /// <param name="insertAtEnd">Block to append the intrinsic to the end of</param>
        /// <returns><see cref="Instructions.CallInstruction"/> for the intrinsic</returns>
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
        public CallInstruction InsertValue( Value value
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            value.ValidateNotNull( nameof( value ) );
            varInfo.ValidateNotNull( nameof( varInfo ) );
            expression.ValidateNotNull( nameof( expression ) );
            location.ValidateNotNull( nameof( location ) );
            insertAtEnd.ValidateNotNull( nameof( insertAtEnd ) );

            if( location.Scope != varInfo.Scope )
            {
                throw new ArgumentException( Resources.Mismatched_scopes );
            }

            if( !LocationDescribes( location, insertAtEnd.ContainingFunction ) )
            {
                throw new ArgumentException( Resources.Location_does_not_describe_the_specified_block_s_containing_function );
            }

            var handle = LLVMDIBuilderInsertDbgValueAtEnd( BuilderHandle
                                                         , value.ValueHandle
                                                         , varInfo.MetadataHandle
                                                         , expression?.MetadataHandle ?? CreateExpression( ).MetadataHandle
                                                         , location.MetadataHandle
                                                         , insertAtEnd.BlockHandle
                                                         );

            var retVal = Value.FromHandle<CallInstruction>( handle );
            retVal.IsTailCall = true;
            return retVal;
        }

        /// <summary>Creates a <see cref="DIExpression"/> from the provided <see cref="ExpressionOp"/>s</summary>
        /// <param name="operations">Operation sequence for the expression</param>
        /// <returns><see cref="DIExpression"/></returns>
        public DIExpression CreateExpression( params ExpressionOp[ ] operations )
            => CreateExpression( ( IEnumerable<ExpressionOp> )operations );

        /// <summary>Creates a <see cref="DIExpression"/> from the provided <see cref="ExpressionOp"/>s</summary>
        /// <param name="operations">Operation sequence for the expression</param>
        /// <returns><see cref="DIExpression"/></returns>
        public DIExpression CreateExpression( IEnumerable<ExpressionOp> operations )
        {
            long[ ] args = operations.Cast<long>( ).ToArray( );
            var handle = LLVMDIBuilderCreateExpression( BuilderHandle, args, args.LongLength );
            return new DIExpression( handle );
        }

        /// <summary>Creates a replaceable composite type</summary>
        /// <param name="tag">Debug information <see cref="Tag"/> for the composite type (only values for a composite type are allowed)</param>
        /// <param name="name">Name of the type</param>
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
        public DICompositeType CreateReplaceableCompositeType( Tag tag
                                                             , string name
                                                             , DINode scope
                                                             , DIFile file
                                                             , uint line
                                                             , uint lang = 0
                                                             , UInt64 sizeInBits = 0
                                                             , UInt32 alignBits = 0
                                                             , DebugInfoFlags flags = DebugInfoFlags.None
                                                             , string uniqueId = ""
                                                             )
        {
            // TODO: validate that tag is really valid or document the result if it isn't (as long as LLVM won't crash at least)
            var handle = LLVMDIBuilderCreateReplaceableCompositeType( BuilderHandle
                                                                    , ( uint )tag
                                                                    , name
                                                                    , name.Length
                                                                    , scope?.MetadataHandle ?? default
                                                                    , file?.MetadataHandle ?? default
                                                                    , line
                                                                    , lang
                                                                    , sizeInBits
                                                                    , alignBits
                                                                    , ( LLVMDIFlags )flags
                                                                    , uniqueId
                                                                    , uniqueId.Length
                                                                    );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        internal DebugInfoBuilder( BitcodeModule owningModule )
            : this( owningModule, true )
        {
        }

        internal LLVMDIBuilderRef BuilderHandle { get; }

        // keeping this private for now as there doesn't seem to be a good reason to support
        // allowUnresolved == false
        private DebugInfoBuilder( BitcodeModule owningModule, bool allowUnresolved )
        {
            owningModule.ValidateNotNull( nameof( owningModule ) );
            BuilderHandle = allowUnresolved
                ? LLVMCreateDIBuilder( owningModule.ModuleHandle )
                : LLVMCreateDIBuilderDisallowUnresolved( owningModule.ModuleHandle );

            OwningModule = owningModule;
        }

        private bool IsFinished;

        private static bool LocationDescribes( DILocation location, IrFunction function )
        {
            return location.Scope.SubProgram.Describes( function )
                || location.InlinedAtScope.SubProgram.Describes( function );
        }
    }
}
