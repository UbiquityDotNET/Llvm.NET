// <copyright file="DebugInfoBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using Llvm.NET.Instructions;
using Llvm.NET.Native;
using Llvm.NET.Values;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugInfoBuilder is a factory class for creating DebugInformation for an LLVM
    /// <see cref="BitcodeModule"/></summary>
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
                throw new InvalidOperationException( "LLVM only allows one DICompileUnit per module" );
            }

            var handle = LLVMDIBuilderCreateCompileUnit( BuilderHandle
                                                       , ( uint )language
                                                       , fileName
                                                       , fileDirectory
                                                       , producer
                                                       , optimized ? 1 : 0
                                                       , compilationFlags
                                                       , runtimeVersion
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

            var handle = LLVMDIBuilderCreateNamespace( BuilderHandle
                                                     , scope?.MetadataHandle ?? default
                                                     , name
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
            if( string.IsNullOrWhiteSpace( path ) )
            {
                return null;
            }

            return CreateFile( Path.GetFileName( path ), Path.GetDirectoryName( path ) );
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

            var handle = LLVMDIBuilderCreateFile( BuilderHandle, fileName, directory ?? string.Empty );
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
        /// <param name="function">Underlying LLVM <see cref="Function"/> to attach debug info to</param>
        /// <param name="typeParameter">Template parameter [default = null]</param>
        /// <param name="declaration">Template declarations [default = null]</param>
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
                                          , Function function
                                          , [CanBeNull] MDNode typeParameter = null
                                          , [CanBeNull] MDNode declaration = null
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
                                                    , mangledName
                                                    , file?.MetadataHandle ?? default
                                                    , line
                                                    , signatureType.MetadataHandle
                                                    , isLocalToUnit ? 1 : 0
                                                    , isDefinition ? 1 : 0
                                                    , scopeLine
                                                    , ( uint )debugFlags
                                                    , isOptimized ? 1 : 0
                                                    , typeParameter?.MetadataHandle ?? default
                                                    , declaration?.MetadataHandle ?? default
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

            var handle = LLVMDIBuilderCreateTempFunctionFwdDecl( BuilderHandle
                                                               , scope.MetadataHandle
                                                               , name
                                                               , mangledName
                                                               , file?.MetadataHandle ?? default
                                                               , line
                                                               , subroutineType.MetadataHandle
                                                               , isLocalToUnit ? 1 : 0
                                                               , isDefinition ? 1 : 0
                                                               , scopeLine
                                                               , ( uint )debugFlags
                                                               , isOptimized ? 1 : 0
                                                               , default
                                                               , default
                                                               );
            return MDNode.FromHandle<DISubProgram>( handle );
        }

        /// <summary>Creates a <see cref="DILocalVariable"/> for a given scope</summary>
        /// <param name="scope">Scope the variable belongs to</param>
        /// <param name="name">Name of the variable</param>
        /// <param name="file">File where the variable is declared</param>
        /// <param name="line">Line where the variable is declared</param>
        /// <param name="type">Type of the variable</param>
        /// <param name="alwaysPreserve">Flag to indicate if this variable's debug informarion should always be preserved</param>
        /// <param name="debugFlags">Flags for the variable</param>
        /// <returns><see cref="DILocalVariable"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILocalVariable CreateLocalVariable( DIScope scope
                                                  , string name
                                                  , DIFile file
                                                  , uint line
                                                  , DIType type
                                                  , bool alwaysPreserve
                                                  , DebugInfoFlags debugFlags
                                                  )
        {
            scope.ValidateNotNull( nameof( scope ) );
            type.ValidateNotNull( nameof( type ) );

            var handle = LLVMDIBuilderCreateAutoVariable( BuilderHandle
                                                        , scope.MetadataHandle
                                                        , name
                                                        , file?.MetadataHandle ?? default
                                                        , line
                                                        , type.MetadataHandle
                                                        , alwaysPreserve ? 1 : 0
                                                        , ( uint )debugFlags
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
                                                             , argNo
                                                             , file?.MetadataHandle ?? default
                                                             , line
                                                             , type.MetadataHandle
                                                             , alwaysPreserve ? 1 : 0
                                                             , ( uint )debugFlags
                                                             );
            return MDNode.FromHandle<DILocalVariable>( handle );
        }

        /// <summary>Construct debug information for a basic type (a.k.a. primitive type)</summary>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size for the type</param>
        /// <param name="encoding"><see cref="DiTypeKind"/> encoding for the type</param>
        /// <returns>Basic type debugging information</returns>
        public DIBasicType CreateBasicType( string name, UInt64 bitSize, DiTypeKind encoding )
        {
            var handle = LLVMDIBuilderCreateBasicType( BuilderHandle, name, bitSize, ( uint )encoding );
            return MDNode.FromHandle<DIBasicType>( handle );
        }

        /// <summary>Creates a pointer type with debug information</summary>
        /// <param name="pointeeType">base type of the pointer</param>
        /// <param name="name">Name of the type</param>
        /// <param name="bitSize">Bit size of the type</param>
        /// <param name="bitAlign">But alignment of the type</param>
        /// <returns>Pointer type</returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreatePointerType( DIType pointeeType, string name, UInt64 bitSize, UInt32 bitAlign = 0 )
        {
            var handle = LLVMDIBuilderCreatePointerType( BuilderHandle
                                                       , pointeeType?.MetadataHandle ?? default // null == void
                                                       , bitSize
                                                       , bitAlign
                                                       , name ?? string.Empty
                                                       );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Creates a qualified type</summary>
        /// <param name="baseType">Base type to add the qualifier to</param>
        /// <param name="tag">Qualifier to apply</param>
        /// <returns>Qualified type</returns>
        /// <exception cref="ArgumentException"><paramref name="tag"/> is <see cref="QualifiedTypeTag.None"/></exception>
        /// <exception cref="ArgumentNullException"><paramref name="baseType"/> is <see langword="null"/></exception>
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
            long count = handles.LongLength;
            if( count == 0 )
            {
                handles = new[ ] { default( LLVMMetadataRef ) };
            }

            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, out handles[ 0 ], ( UInt64 )count );
            return new DITypeArray( MDNode.FromHandle<MDTuple>( handle) );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="types">Parameter types</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DITypeArray types )
        {
            types.ValidateNotNull( nameof( types ) );

            var handle = LLVMDIBuilderCreateSubroutineType( BuilderHandle
                                                          , types.Tuple.MetadataHandle
                                                          , ( uint )debugFlags
                                                          );

            return MDNode.FromHandle<DISubroutineType>( handle );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags )
        {
            var typeArray = GetOrCreateTypeArray( null );
            return CreateSubroutineType( debugFlags, typeArray );
        }

        /// <summary>Creates a <see cref="DISubroutineType"/> to provide debug information for a function/procedure signature</summary>
        /// <param name="debugFlags"><see cref="DebugInfoFlags"/> for this signature</param>
        /// <param name="returnType">Return type of the signature</param>
        /// <param name="types">Parameters for the function</param>
        /// <returns><see cref="DISubroutineType"/></returns>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType returnType, IEnumerable<DIType> types )
        {
            var typeArray = GetOrCreateTypeArray( types.Prepend( returnType ) );
            return CreateSubroutineType( debugFlags, typeArray );
        }

        /// <summary>Creates debug desription of a structure type</summary>
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
            scope.ValidateNotNull( nameof( scope ) );
            elements.ValidateNotNull( nameof( elements ) );

            var handle = LLVMDIBuilderCreateStructType( BuilderHandle
                                                      , scope.MetadataHandle
                                                      , name
                                                      , file?.MetadataHandle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , (uint)debugFlags
                                                      , derivedFrom?.MetadataHandle ?? default
                                                      , elements.Tuple.MetadataHandle
                                                      );

            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug desription of a structure type</summary>
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
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, GetOrCreateArray( elements ) );
        }

        /// <summary>Creates debug desription of a structure type</summary>
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
                                               , IEnumerable<DINode> elements
                                               )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, GetOrCreateArray( elements ) );
        }

        /// <summary>Creates debug desription of a union type</summary>
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
            scope.ValidateNotNull( nameof( scope ) );
            elements.ValidateNotNull( nameof( elements ) );

            var handle = LLVMDIBuilderCreateUnionType( BuilderHandle
                                                     , scope.MetadataHandle
                                                     , name
                                                     , file?.MetadataHandle ?? default
                                                     , line
                                                     , bitSize
                                                     , bitAlign
                                                     , ( uint )debugFlags
                                                     , elements.Tuple.MetadataHandle
                                                     );

            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug desription of a union type</summary>
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
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, GetOrCreateArray( elements ) );
        }

        /// <summary>Creates debug desription of a union type</summary>
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
                                               , IEnumerable<DINode> elements
                                               )
        {
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, GetOrCreateArray( elements ) );
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
                                                      , file?.MetadataHandle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , bitOffset
                                                      , ( uint )debugFlags
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
            elementType.ValidateNotNull( nameof( elementType ) );
            subscripts.ValidateNotNull( nameof( subscripts ) );

            var handle = LLVMDIBuilderCreateArrayType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subscripts.Tuple.MetadataHandle );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        /// <summary>Creates debug information for an array type</summary>
        /// <param name="bitSize">Size, in bits for the type</param>
        /// <param name="bitAlign">Alignment in bits for the type</param>
        /// <param name="elementType">Type of elements in the array</param>
        /// <param name="subscripts">Dimensions for the array</param>
        /// <returns><see cref="DICompositeType"/> for the array</returns>
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[ ] subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, GetOrCreateArray( subscripts ) );
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
                                                   , file?.MetadataHandle ?? default
                                                   , line
                                                   , context?.MetadataHandle ?? default
                                                   );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        /// <summary>Creates a new subrange</summary>
        /// <param name="lo">Lower bounds of the subrange</param>
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
        public DINodeArray GetOrCreateArray( IEnumerable<DINode> elements )
        {
            var buf = elements.Select( d => d?.MetadataHandle ?? default ).ToArray( );
            long actualLen = buf.LongLength;

            // for the out parameter trick to work - need to have a valid array with at least one element
            if( buf.LongLength == 0 )
            {
                buf = new LLVMMetadataRef[ 1 ];
            }

            var handle = LLVMDIBuilderGetOrCreateArray( BuilderHandle, out buf[ 0 ], ( UInt64 )actualLen );
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
            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, out buf[ 0 ], ( UInt64 )buf.LongLength );
            return new DITypeArray( MDNode.FromHandle<MDTuple>( handle ) );
        }

        /// <summary>Creates a value for an enumeration</summary>
        /// <param name="name">Name of the value</param>
        /// <param name="value">Value of the enumerated value</param>
        /// <returns><see cref="DIEnumerator"/> for the name, value pair</returns>
        public DIEnumerator CreateEnumeratorValue( string name, long value )
        {
            var handle = LLVMDIBuilderCreateEnumeratorValue( BuilderHandle, name, value );
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
        /// <param name="uniqueId">unique ID for the type *default is an empty string</param>
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
                                                    , string uniqueId = ""
                                                    )
        {
            scope.ValidateNotNull( nameof( scope ) );
            underlyingType.ValidateNotNull( nameof( underlyingType ) );

            var elementHandles = elements.Select( e => e.MetadataHandle ).ToArray( );
            var elementArray = LLVMDIBuilderGetOrCreateArray( BuilderHandle, out elementHandles[ 0 ], ( UInt64 )elementHandles.LongLength );
            var handle = LLVMDIBuilderCreateEnumerationType( BuilderHandle
                                                           , scope.MetadataHandle
                                                           , name
                                                           , file?.MetadataHandle ?? default
                                                           , lineNumber
                                                           , sizeInBits
                                                           , alignInBits
                                                           , elementArray
                                                           , underlyingType.MetadataHandle
                                                           , uniqueId
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
        /// <returns><see cref="DIGlobalVariableExpression"/> from the prameters</returns>
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
                                                                    , linkageName
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
            LLVMDIBuilderFinalizeSubProgram( BuilderHandle, subProgram.MetadataHandle );
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
            if(IsFinished)
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
                    bldr.AppendLine( "Temporaries must be resolved before finalizing debug information:" );
                }

                bldr.AppendFormat( "\t{0}", node.ToString( ) );
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
        /// <param name="insertBefore"><see cref="Instructions.Instruction"/> to insert the declartion before</param>
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
        /// <param name="insertBefore"><see cref="Instructions.Instruction"/> to insert the declartion before</param>
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
        /// <param name="insertAtEnd"><see cref="BasicBlock"/> to insert the declartion at the end of</param>
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
        /// <param name="insertAtEnd"><see cref="BasicBlock"/> to insert the declartion at the end of</param>
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
                throw new ArgumentException( "Mismatched scopes for location and variable" );
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
        /// <param name="offset">Offset in the user source variable where <paramref name="value"/> is written</param>
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
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , Instruction insertBefore
                                          )
        {
            return InsertValue( value, offset, varInfo, null, location, insertBefore );
        }

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic before the specified instruction</summary>
        /// <param name="value">New value</param>
        /// <param name="offset">Offset in the user source variable where <paramref name="value"/> is written</param>
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
                                          , UInt64 offset
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

            var handle = LLVMDIBuilderInsertValueBefore( BuilderHandle
                                                       , value.ValueHandle
                                                       , offset
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
        /// <param name="offset">Offset in the user source variable where <paramref name="value"/> is written</param>
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
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            return InsertValue( value, offset, varInfo, null, location, insertAtEnd );
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
        /// selection. Instead this provides a means to bind the LLVM Debug information metadata to a particular LLVM <see cref="Value"/>
        /// that allows the transformation and optimization passes to track the debug information. Thus, even with optimized code
        /// the actual debug information is retained.
        /// </note>
        /// </remarks>
        /// <seealso href="xref:llvm_sourcelevel_debugging#lvm-dbg-value">LLVM: llvm.dbg.value</seealso>
        /// <seealso href="xref:llvm_sourcelevel_debugging#source-level-debugging-with-llvm">LLVM: Source Level Debugging with LLVM</seealso>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public CallInstruction InsertValue( Value value
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            return InsertValue(value, 0, varInfo, expression, location, insertAtEnd);
        }

        /// <summary>Inserts a call to the llvm.dbg.value intrinsic at the end of a basic block</summary>
        /// <param name="value">New value</param>
        /// <param name="offset">Offset in the user source variable where <paramref name="value"/> is written</param>
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
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public CallInstruction InsertValue( Value value
                                          , UInt64 offset
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
                throw new ArgumentException( "mismatched scopes" );
            }

            if( !LocationDescribes( location, insertAtEnd.ContainingFunction ) )
            {
                throw new ArgumentException( "location does not describe the specified block's containing function" );
            }

            var handle = LLVMDIBuilderInsertValueAtEnd( BuilderHandle
                                                      , value.ValueHandle
                                                      , offset
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
            var args = operations.Cast<long>( ).ToArray( );
            long actualCount = args.LongLength;
            if( args.Length == 0 )
            {
                args = new long[ 1 ];
            }

            var handle = LLVMDIBuilderCreateExpression( BuilderHandle, out args[ 0 ], ( UInt64 )actualCount );
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
        /// <param name="alignBits">alignement of the type in bits</param>
        /// <param name="flags"><see cref="DebugInfoFlags"/> for the type</param>
        /// <returns><see cref="DICompositeType"/></returns>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateReplaceableCompositeType( Tag tag
                                                             , string name
                                                             , DINode scope
                                                             , DIFile file
                                                             , uint line
                                                             , uint lang = 0
                                                             , UInt64 sizeInBits = 0
                                                             , UInt64 alignBits = 0
                                                             , DebugInfoFlags flags = DebugInfoFlags.None
                                                             )
        {
            // TODO: validate that tag is really valid or document the result if it isn't (as long as llvm won't crash at least)
            var handle = LLVMDIBuilderCreateReplaceableCompositeType( BuilderHandle
                                                                    , ( uint )tag
                                                                    , name
                                                                    , scope?.MetadataHandle ?? default
                                                                    , file?.MetadataHandle ?? default
                                                                    , line
                                                                    , lang
                                                                    , sizeInBits
                                                                    , alignBits
                                                                    , ( uint )flags
                                                                    );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        internal DebugInfoBuilder( BitcodeModule owningModule )
            : this( owningModule, true )
        {
        }

        internal LLVMDIBuilderRef BuilderHandle { get; private set; }

        // keeping this private for now as there doesn't seem to be a good reason to support
        // allowUnresolved == false
        private DebugInfoBuilder( BitcodeModule owningModule, bool allowUnresolved )
        {
            owningModule.ValidateNotNull( nameof( owningModule ) );

            BuilderHandle = LLVMNewDIBuilder( owningModule.ModuleHandle, allowUnresolved );
            OwningModule = owningModule;
        }

        private bool IsFinished;

        private static bool LocationDescribes( DILocation location, Function function )
        {
            return location.Scope.SubProgram.Describes( function )
                || location.InlinedAtScope.SubProgram.Describes( function );
        }

        #pragma warning disable SA1124 // Do not use regions
        #region LibLLVM P/Invoke APIs
        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMDIBuilderRef LLVMNewDIBuilder( LLVMModuleRef @m, [MarshalAs( UnmanagedType.Bool )]bool allowUnresolved );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMDIBuilderFinalize( LLVMDIBuilderRef @d );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMDIBuilderFinalizeSubProgram( LLVMDIBuilderRef dref, LLVMMetadataRef /*DISubProgram*/ subProgram );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateCompileUnit( LLVMDIBuilderRef @D, UInt32 @Language, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir, [MarshalAs( UnmanagedType.LPStr )] string @Producer, int @Optimized, [MarshalAs( UnmanagedType.LPStr )] string @Flags, UInt32 @RuntimeVersion );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateFile( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @File, [MarshalAs( UnmanagedType.LPStr )] string @Dir );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlock( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Line, UInt32 @Column );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateLexicalBlockFile( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, LLVMMetadataRef @File, UInt32 @Discriminator );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateFunction( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateTempFunctionFwdDecl( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, [MarshalAs( UnmanagedType.LPStr )] string @LinkageName, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @CompositeType, int @IsLocalToUnit, int @IsDefinition, UInt32 @ScopeLine, UInt32 @Flags, int @IsOptimized, LLVMMetadataRef TParam, LLVMMetadataRef Decl );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateAutoVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateParameterVariable( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt32 @ArgNo, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Ty, int @AlwaysPreserve, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateBasicType( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )] string @Name, UInt64 @SizeInBits, UInt32 @Encoding );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreatePointerType( LLVMDIBuilderRef @D, LLVMMetadataRef @PointeeType, UInt64 @SizeInBits, UInt32 @AlignInBits, [MarshalAs( UnmanagedType.LPStr )] string @Name );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateQualifiedType( LLVMDIBuilderRef Dref, UInt32 Tag, LLVMMetadataRef BaseType );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateSubroutineType( LLVMDIBuilderRef @D, LLVMMetadataRef @ParameterTypes, UInt32 @Flags );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateStructType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @DerivedFrom, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateUnionType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt32 @Flags, LLVMMetadataRef @ElementTypes );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateMemberType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, UInt64 @SizeInBits, UInt32 @AlignInBits, UInt64 @OffsetInBits, UInt32 @Flags, LLVMMetadataRef @Ty );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateArrayType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateVectorType( LLVMDIBuilderRef @D, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @ElementType, LLVMMetadataRef @Subscripts );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateTypedef( LLVMDIBuilderRef @D, LLVMMetadataRef @Ty, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @Line, LLVMMetadataRef @Context );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateSubrange( LLVMDIBuilderRef @D, Int64 @Lo, Int64 @Count );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderGetOrCreateTypeArray( LLVMDIBuilderRef @D, out LLVMMetadataRef @Data, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateExpression( LLVMDIBuilderRef @Dref, out Int64 @Addr, UInt64 @Length );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMValueRef LLVMDIBuilderInsertDeclareAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Storage, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMValueRef LLVMDIBuilderInsertValueAtEnd( LLVMDIBuilderRef @D, LLVMValueRef @Val, UInt64 @Offset, LLVMMetadataRef @VarInfo, LLVMMetadataRef @Expr, LLVMMetadataRef Location, LLVMBasicBlockRef @Block );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateEnumerationType( LLVMDIBuilderRef @D, LLVMMetadataRef @Scope, [MarshalAs( UnmanagedType.LPStr )] string @Name, LLVMMetadataRef @File, UInt32 @LineNumber, UInt64 @SizeInBits, UInt32 @AlignInBits, LLVMMetadataRef @Elements, LLVMMetadataRef @UnderlyingType, [MarshalAs( UnmanagedType.LPStr )]string @UniqueId );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef @D, [MarshalAs( UnmanagedType.LPStr )]string @Name, Int64 @Val );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMDwarfTag LLVMDIDescriptorGetTag( LLVMMetadataRef descriptor );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateGlobalVariableExpression( LLVMDIBuilderRef Dref, LLVMMetadataRef Context, [MarshalAs( UnmanagedType.LPStr )] string Name, [MarshalAs( UnmanagedType.LPStr )] string LinkageName, LLVMMetadataRef File, UInt32 LineNo, LLVMMetadataRef Ty, [MarshalAs( UnmanagedType.Bool )]bool isLocalToUnit, LLVMMetadataRef expression, LLVMMetadataRef Decl, UInt32 AlignInBits );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMValueRef LLVMDIBuilderInsertDeclareBefore( LLVMDIBuilderRef Dref, LLVMValueRef Storage, LLVMMetadataRef VarInfo, LLVMMetadataRef Expr, LLVMMetadataRef Location, LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMValueRef LLVMDIBuilderInsertValueBefore( LLVMDIBuilderRef Dref, /*llvm::Value **/LLVMValueRef Val, UInt64 Offset, /*DILocalVariable **/ LLVMMetadataRef VarInfo, /*DIExpression **/ LLVMMetadataRef Expr, /*const DILocation **/ LLVMMetadataRef DL, /*Instruction **/ LLVMValueRef InsertBefore );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateReplaceableCompositeType( LLVMDIBuilderRef Dref, UInt32 Tag, [MarshalAs( UnmanagedType.LPStr )] string Name, LLVMMetadataRef Scope, LLVMMetadataRef File, UInt32 Line, UInt32 RuntimeLang, UInt64 SizeInBits, UInt64 AlignInBits, UInt32 Flags );

        [DllImport( LibraryPath, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMMetadataRef LLVMDIBuilderCreateNamespace( LLVMDIBuilderRef Dref, LLVMMetadataRef scope, [MarshalAs( UnmanagedType.LPStr )] string name, [MarshalAs( UnmanagedType.Bool )]bool exportSymbols );
        #endregion
    }
}
