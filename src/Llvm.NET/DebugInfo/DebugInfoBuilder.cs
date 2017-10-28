// <copyright file="DebugInfoBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Llvm.NET.Instructions;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugInfoBuilder is a factory class for creating DebugInformation for an LLVM
    /// <see cref="BitcodeModule"/></summary>
    /// <remarks>
    /// Many Debug information metadata nodes are created with unresolved references to additional
    /// metadata. To ensure such metadata is resolved applications should call the <see cref="Finish"/>
    /// method to resolve and finalize the metadata. After this point only fully resolved nodes may
    /// be added to ensure that the data remains valid.
    /// </remarks>
    /// <seealso href="xref:llvm_sourceleveldebugging">LLVM Source Level Debugging</seealso>
    public sealed class DebugInfoBuilder
        : IDisposable
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DINamespace CreateNamespace( DIScope scope, string name, bool exportSymbols )
        {
            if( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "name cannot be null or empty", nameof( name ) );
            }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlock CreateLexicalBlock( DIScope scope, DIFile file, uint line, uint column )
        {
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlockFile CreateLexicalBlockFile( DIScope scope, DIFile file, uint discriminator )
        {
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( file == null )
            {
                throw new ArgumentNullException( nameof( file ) );
            }

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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( string.IsNullOrWhiteSpace( name ) )
            {
                name = string.Empty;
            }

            if( string.IsNullOrWhiteSpace( mangledName ) )
            {
                mangledName = string.Empty;
            }

            if( signatureType == null )
            {
                throw new ArgumentNullException( nameof( signatureType ) );
            }

            if( function == null )
            {
                throw new ArgumentNullException( nameof( function ) );
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( subroutineType == null )
            {
                throw new ArgumentNullException( nameof( subroutineType ) );
            }

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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( type == null )
            {
                throw new ArgumentNullException( nameof( type ) );
            }

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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( type == null )
            {
                throw new ArgumentNullException( nameof( type ) );
            }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateQualifiedType( DIType baseType, QualifiedTypeTag tag )
        {
            if( baseType == null )
            {
                throw new ArgumentNullException( nameof( baseType ) );
            }

            var handle = LLVMDIBuilderCreateQualifiedType( BuilderHandle, ( uint )tag, baseType.MetadataHandle );
            return MDNode.FromHandle<DIDerivedType>( handle );
        }

        public DITypeArray CreateTypeArray( params DIType[ ] types ) => CreateTypeArray( ( IEnumerable<DIType> )types );

        public DITypeArray CreateTypeArray( IEnumerable<DIType> types )
        {
            var handles = types.Select( t => t.MetadataHandle ).ToArray( );
            long count = handles.LongLength;
            if( count == 0 )
            {
                handles = new[ ] { default( LLVMMetadataRef ) };
            }

            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, out handles[ 0 ], ( UInt64 )count );
            return new DITypeArray( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DITypeArray types )
        {
            if( types == null )
            {
                throw new ArgumentNullException( nameof( types ) );
            }

            var handle = LLVMDIBuilderCreateSubroutineType( BuilderHandle
                                                          , types.MetadataHandle
                                                          , ( uint )debugFlags
                                                          );
            return MDNode.FromHandle<DISubroutineType>( handle );
        }

        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags )
        {
            var typeArray = GetOrCreateTypeArray( null );
            return CreateSubroutineType( debugFlags, typeArray );
        }

        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType returnType, IEnumerable<DIType> types )
        {
            var typeArray = GetOrCreateTypeArray( types.Prepend( returnType ) );
            return CreateSubroutineType( debugFlags, typeArray );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateStructType( DIScope scope
                                               , string name
                                               , DIFile file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , uint flags
                                               , DIType derivedFrom
                                               , DINodeArray elements
                                               )
        {
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( elements == null )
            {
                throw new ArgumentNullException( nameof( elements ) );
            }

            var handle = LLVMDIBuilderCreateStructType( BuilderHandle
                                                      , scope.MetadataHandle
                                                      , name
                                                      , file?.MetadataHandle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , flags
                                                      , derivedFrom?.MetadataHandle ?? default
                                                      , elements.Tuple.MetadataHandle
                                                      );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

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
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, ( uint )debugFlags, derivedFrom, GetOrCreateArray( elements ) );
        }

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
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, ( uint )debugFlags, derivedFrom, GetOrCreateArray( elements ) );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateUnionType( DIScope scope
                                              , string name
                                              , DIFile file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , uint flags
                                              , DINodeArray elements
                                              )
        {
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( elements == null )
            {
                throw new ArgumentNullException( nameof( elements ) );
            }

            var handle = LLVMDIBuilderCreateUnionType( BuilderHandle
                                                     , scope.MetadataHandle
                                                     , name
                                                     , file?.MetadataHandle ?? default
                                                     , line
                                                     , bitSize
                                                     , bitAlign
                                                     , flags
                                                     , elements.Tuple.MetadataHandle
                                                     );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

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
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, ( uint )debugFlags, GetOrCreateArray( elements ) );
        }

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
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, ( uint )debugFlags, GetOrCreateArray( elements ) );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( type == null )
            {
                throw new ArgumentNullException( nameof( type ) );
            }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts )
        {
            if( elementType == null )
            {
                throw new ArgumentNullException( nameof( elementType ) );
            }

            if( subscripts == null )
            {
                throw new ArgumentNullException( nameof( subscripts ) );
            }

            var handle = LLVMDIBuilderCreateArrayType( BuilderHandle, bitSize, bitAlign, elementType.MetadataHandle, subscripts.Tuple.MetadataHandle );
            return MDNode.FromHandle<DICompositeType>( handle );
        }

        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[ ] subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, GetOrCreateArray( subscripts ) );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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

        public DISubRange CreateSubRange( long lo, long count )
        {
            var handle = LLVMDIBuilderGetOrCreateSubrange( BuilderHandle, lo, count );
            return MDNode.FromHandle<DISubRange>( handle );
        }

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

        public DITypeArray GetOrCreateTypeArray( params DIType[ ] types ) => GetOrCreateTypeArray( ( IEnumerable<DIType> )types );

        public DITypeArray GetOrCreateTypeArray( IEnumerable<DIType> types )
        {
            var buf = types.Select( t => t?.MetadataHandle ?? default ).ToArray( );
            var handle = LLVMDIBuilderGetOrCreateTypeArray( BuilderHandle, out buf[ 0 ], ( UInt64 )buf.LongLength );
            return new DITypeArray( handle );
        }

        public DIEnumerator CreateEnumeratorValue( string name, long value )
        {
            var handle = LLVMDIBuilderCreateEnumeratorValue( BuilderHandle, name, value );
            return MDNode.FromHandle<DIEnumerator>( handle );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( underlyingType == null )
            {
                throw new ArgumentNullException( nameof( underlyingType ) );
            }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
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
            if( scope == null )
            {
                throw new ArgumentNullException( nameof( scope ) );
            }

            if( type == null )
            {
                throw new ArgumentNullException( nameof( type ) );
            }

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

        public void Finish( )
        {
            if( !IsFinished )
            {
                var unresolvedTemps = from node in OwningModule.Context.Metadata.OfType<MDNode>( )
                                      where node.IsTemporary && !node.IsResolved
                                      select node;

                if( unresolvedTemps.Any( ) )
                {
                    var bldr = new StringBuilder( "Temporaries must be resolved before finalizing debug information:\n" );
                    foreach( var node in unresolvedTemps )
                    {
                        bldr.AppendFormat( "\t{0}\n", node.ToString( ) );
                    }

                    throw new InvalidOperationException( bldr.ToString( ) );
                }

                LLVMDIBuilderFinalize( BuilderHandle );
                IsFinished = true;
            }
        }

        public Instruction InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, Instruction insertBefore )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), location, insertBefore );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public Instruction InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, Instruction insertBefore )
        {
            if( storage == null )
            {
                throw new ArgumentNullException( nameof( storage ) );
            }

            if( varInfo == null )
            {
                throw new ArgumentNullException( nameof( varInfo ) );
            }

            if( expression == null )
            {
                throw new ArgumentNullException( nameof( expression ) );
            }

            if( location == null )
            {
                throw new ArgumentNullException( nameof( location ) );
            }

            if( insertBefore == null )
            {
                throw new ArgumentNullException( nameof( insertBefore ) );
            }

            var handle = LLVMDIBuilderInsertDeclareBefore( BuilderHandle
                                                         , storage.ValueHandle
                                                         , varInfo.MetadataHandle
                                                         , expression.MetadataHandle
                                                         , location.MetadataHandle
                                                         , insertBefore.ValueHandle
                                                         );
            return Value.FromHandle<Instruction>( handle );
        }

        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd )
        {
            return InsertDeclare( storage, varInfo, CreateExpression( ), location, insertAtEnd );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public CallInstruction InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression espression, DILocation location, BasicBlock insertAtEnd )
        {
            if( storage == null )
            {
                throw new ArgumentNullException( nameof( storage ) );
            }

            if( varInfo == null )
            {
                throw new ArgumentNullException( nameof( varInfo ) );
            }

            if( espression == null )
            {
                throw new ArgumentNullException( nameof( espression ) );
            }

            if( location == null )
            {
                throw new ArgumentNullException( nameof( location ) );
            }

            if( insertAtEnd == null )
            {
                throw new ArgumentNullException( nameof( insertAtEnd ) );
            }

            if( location.Scope.SubProgram != varInfo.Scope.SubProgram )
            {
                throw new ArgumentException( "Mismatched scopes for location and variable" );
            }

            var handle = LLVMDIBuilderInsertDeclareAtEnd( BuilderHandle
                                                        , storage.ValueHandle
                                                        , varInfo.MetadataHandle
                                                        , espression.MetadataHandle
                                                        , location.MetadataHandle
                                                        , insertAtEnd.BlockHandle
                                                        );
            return Value.FromHandle<CallInstruction>( handle );
        }

        public CallInstruction InsertValue( Value value
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , Instruction insertBefore
                                          )
        {
            return InsertValue( value, offset, varInfo, null, location, insertBefore );
        }

        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public CallInstruction InsertValue( Value value
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , Instruction insertBefore
                                          )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( varInfo == null )
            {
                throw new ArgumentNullException( nameof( varInfo ) );
            }

            if( expression == null )
            {
                throw new ArgumentNullException( nameof( expression ) );
            }

            if( location == null )
            {
                throw new ArgumentNullException( nameof( location ) );
            }

            if( insertBefore == null )
            {
                throw new ArgumentNullException( nameof( insertBefore ) );
            }

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

        public CallInstruction InsertValue( Value value
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            return InsertValue( value, offset, varInfo, null, location, insertAtEnd );
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public CallInstruction InsertValue(Value value
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            return InsertValue(value, 0, varInfo, expression, location, insertAtEnd);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public CallInstruction InsertValue( Value value
                                          , UInt64 offset
                                          , DILocalVariable varInfo
                                          , DIExpression expression
                                          , DILocation location
                                          , BasicBlock insertAtEnd
                                          )
        {
            if( value == null )
            {
                throw new ArgumentNullException( nameof( value ) );
            }

            if( varInfo == null )
            {
                throw new ArgumentNullException( nameof( varInfo ) );
            }

            if( expression == null )
            {
                throw new ArgumentNullException( nameof( expression ) );
            }

            if( location == null )
            {
                throw new ArgumentNullException( nameof( location ) );
            }

            if( insertAtEnd == null )
            {
                throw new ArgumentNullException( nameof( insertAtEnd ) );
            }

            if( location.Scope != varInfo.Scope )
            {
                throw new ArgumentException( "mismatched scopes" );
            }

            if( !location.Describes(insertAtEnd.ContainingFunction ) )
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

        public DIExpression CreateExpression( params ExpressionOp[ ] operations ) => CreateExpression( ( IEnumerable<ExpressionOp> )operations );

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

        public void Dispose( )
        {
            if( BuilderHandle == default )
            {
                LLVMDIBuilderDestroy( BuilderHandle );
                BuilderHandle = default;
            }
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
            if( owningModule == null )
            {
                throw new ArgumentNullException( nameof( owningModule ) );
            }

            BuilderHandle = LLVMNewDIBuilder( owningModule.ModuleHandle, allowUnresolved );
            OwningModule = owningModule;
        }

        private bool IsFinished;
    }
}
