// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
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
    internal sealed class DIBuilderAlias
        : IDIBuilder
        , IHandleWrapper<LLVMDIBuilderRefAlias>
        , IEquatable<DIBuilderAlias>
    {
        #region IEquatable<T>

        /// <inheritdoc/>
        public bool Equals( IDIBuilder? other ) => other is not null && NativeHandle.Equals( other.GetUnownedHandle() );

        /// <inheritdoc/>
        public bool Equals( DIBuilderAlias? other ) => other is not null && NativeHandle.Equals( other.NativeHandle );

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is ContextAlias alias
                                                  ? Equals( alias )
                                                  : Equals( obj as IContext );

        /// <inheritdoc/>
        public override int GetHashCode( ) => NativeHandle.GetHashCode();

        #endregion

        /// <inheritdoc/>
        public IModule OwningModule { get; }

        /// <inheritdoc/>
        public DICompileUnit? CompileUnit { get; private set; }

        /// <inheritdoc/>
        public DICompileUnit CreateCompileUnit( SourceLanguage language
                                              , string sourceFilePath
                                              , LazyEncodedString? producer
                                              , bool optimized = false
                                              , LazyEncodedString? compilationFlags = null
                                              , uint runtimeVersion = 0
                                              )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( sourceFilePath );
            string validatedSrcPath = Path.GetFileName( sourceFilePath )
                                   ?? throw new ArgumentException( "Valid File name not present", nameof( sourceFilePath ) );

            return CreateCompileUnit( language
                                    , validatedSrcPath
                                    , Path.GetDirectoryName( sourceFilePath ) ?? Environment.CurrentDirectory
                                    , producer
                                    , optimized
                                    , compilationFlags
                                    , runtimeVersion
                                    );
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DICompileUnit", Justification = "It is spelled correctly 8^)" )]
        public DICompileUnit CreateCompileUnit( SourceLanguage language
                                              , LazyEncodedString fileName
                                              , LazyEncodedString fileDirectory
                                              , LazyEncodedString? producer
                                              , bool optimized
                                              , LazyEncodedString? compilationFlags
                                              , uint runtimeVersion
                                              , LazyEncodedString? sysRoot = null
                                              , LazyEncodedString? sdk = null
                                              )
        {
            // LLVM will crash the process if the source language is not defined.
            if(!Enum.IsDefined( language ))
            {
                throw new ArgumentException( "Undefined language value", nameof( language ) );
            }

            if(CompileUnit is not null)
            {
                throw new InvalidOperationException( Resources.LLVM_only_allows_one_DICompileUnit_per_builder );
            }

            var file = CreateFile( fileName, fileDirectory );
            var handle = LLVMDIBuilderCreateCompileUnit( NativeHandle.ThrowIfInvalid()
                                                       , ( LLVMDWARFSourceLanguage )language
                                                       , file.Handle
                                                       , producer
                                                       , optimized
                                                       , compilationFlags
                                                       , runtimeVersion
                                                       , SplitName: null
                                                       , LLVMDWARFEmissionKind.LLVMDWARFEmissionFull
                                                       , DWOId: 0
                                                       , SplitDebugInlining: false
                                                       , DebugInfoForProfiling: false
                                                       , sysRoot
                                                       , sdk
                                                       );

            CompileUnit = (DICompileUnit)handle.ThrowIfInvalid().CreateMetadata()!;
            return CompileUnit;
        }

        /// <inheritdoc/>
        public DIMacroFile CreateTempMacroFile( DIMacroFile? parent, uint line, DIFile? file )
        {
            var handle = LLVMDIBuilderCreateTempMacroFile( NativeHandle.ThrowIfInvalid()
                                                         , parent?.Handle ?? LLVMMetadataRef.Zero
                                                         , line
                                                         , file?.Handle ?? LLVMMetadataRef.Zero
                                                         );

            return (DIMacroFile)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DIMacro CreateMacro( DIMacroFile? parentFile, uint line, MacroKind kind, LazyEncodedString name, LazyEncodedString value )
        {
            kind.ThrowIfNotDefined();
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            ArgumentNullException.ThrowIfNull( value );

            switch(kind)
            {
            case MacroKind.Define:
            case MacroKind.Undefine:
                break;

            default:
                throw new NotSupportedException( "LLVM currently only supports MacroKind.Define and MacroKind.Undefine" );
            }

            var handle = LLVMDIBuilderCreateMacro( NativeHandle.ThrowIfInvalid()
                                                 , parentFile?.Handle ?? LLVMMetadataRef.Zero
                                                 , line
                                                 , ( LLVMDWARFMacinfoRecordType )kind
                                                 , name
                                                 , value
                                                 );

            return (DIMacro)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DINamespace CreateNamespace( DIScope? scope, LazyEncodedString name, bool exportSymbols )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMDIBuilderCreateNameSpace( NativeHandle.ThrowIfInvalid()
                                                     , scope?.Handle ?? default
                                                     , name
                                                     , exportSymbols
                                                     );

            return (DINamespace)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DIFile CreateFile( string? path )
        {
            return CreateFile(
                Path.GetFileName( path ) ?? LazyEncodedString.Empty,
                Path.GetDirectoryName( path ) ?? LazyEncodedString.Empty
                );
        }

        /// <inheritdoc/>
        public DIFile CreateFile( LazyEncodedString? fileName, LazyEncodedString? directory )
        {
            var handle = LLVMDIBuilderCreateFile( NativeHandle.ThrowIfInvalid()
                                                , fileName ?? LazyEncodedString.Empty
                                                , directory ?? LazyEncodedString.Empty
                                                );

            return (DIFile)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /* TODO: Extend CreateFile with checksum info and source text params (both optional) */

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlock CreateLexicalBlock( DIScope? scope, DIFile? file, uint line, uint column )
        {
            var handle = LLVMDIBuilderCreateLexicalBlock( NativeHandle.ThrowIfInvalid()
                                                        , scope?.Handle ?? default
                                                        , file?.Handle ?? default
                                                        , line
                                                        , column
                                                        );

            return (DILexicalBlock)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILexicalBlockFile CreateLexicalBlockFile( DIScope? scope, DIFile? file, uint discriminator )
        {
            var handle = LLVMDIBuilderCreateLexicalBlockFile( NativeHandle.ThrowIfInvalid()
                                                            , scope?.Handle ?? default
                                                            , file?.Handle ?? default
                                                            , discriminator
                                                            );

            return (DILexicalBlockFile)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DISubProgram CreateFunction( DIScope? scope
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
                                          )
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( mangledName );
            ArgumentNullException.ThrowIfNull( function );

            // force whitespace strings to empty
            if(LazyEncodedString.IsNullOrWhiteSpace( name ))
            {
                name = LazyEncodedString.Empty;
            }

            if(LazyEncodedString.IsNullOrWhiteSpace( mangledName ))
            {
                mangledName = LazyEncodedString.Empty;
            }

            var handle = LLVMDIBuilderCreateFunction( NativeHandle.ThrowIfInvalid()
                                                    , scope?.Handle ?? default
                                                    , name
                                                    , mangledName
                                                    , file?.Handle ?? default
                                                    , line
                                                    , signatureType?.Handle ?? default
                                                    , isLocalToUnit
                                                    , isDefinition
                                                    , scopeLine
                                                    , ( LLVMDIFlags )debugFlags
                                                    , isOptimized
                                                    );

            var retVal =(DISubProgram)handle.ThrowIfInvalid( ).CreateMetadata( )!;

            // sanity checks
            SanityCheck( retVal, isDefinition );

            function.DISubProgram = retVal;
            return retVal;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DISubProgram ForwardDeclareFunction( DIScope? scope
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
                                                  )
        {
            ArgumentNullException.ThrowIfNull( subroutineType );

            if(LazyEncodedString.IsNullOrWhiteSpace( name ))
            {
                name = LazyEncodedString.Empty;
            }

            if(LazyEncodedString.IsNullOrWhiteSpace( mangledName ))
            {
                mangledName = LazyEncodedString.Empty;
            }

            var handle = LibLLVMDIBuilderCreateTempFunctionFwdDecl( NativeHandle.ThrowIfInvalid()
                                                                  , scope?.Handle ?? default
                                                                  , name
                                                                  , mangledName
                                                                  , file?.Handle ?? default
                                                                  , line
                                                                  , subroutineType.Handle
                                                                  , isLocalToUnit
                                                                  , isDefinition
                                                                  , scopeLine
                                                                  , ( LLVMDIFlags )debugFlags
                                                                  , isOptimized
                                                                  );

            return (DISubProgram)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILocalVariable CreateLocalVariable( DIScope? scope
                                                  , LazyEncodedString name
                                                  , DIFile? file
                                                  , uint line
                                                  , DIType? type
                                                  , bool alwaysPreserve = false
                                                  , DebugInfoFlags debugFlags = DebugInfoFlags.None
                                                  , uint alignInBits = 0
                                                  )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMDIBuilderCreateAutoVariable( NativeHandle.ThrowIfInvalid()
                                                        , scope?.Handle ?? default
                                                        , name
                                                        , file?.Handle ?? default
                                                        , line
                                                        , type?.Handle ?? default
                                                        , alwaysPreserve
                                                        , ( LLVMDIFlags )debugFlags
                                                        , alignInBits
                                                        );

            return (DILocalVariable)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DILocalVariable CreateArgument( DIScope? scope
                                             , LazyEncodedString name
                                             , DIFile? file
                                             , uint line
                                             , DIType? type
                                             , bool alwaysPreserve
                                             , DebugInfoFlags debugFlags
                                             , ushort argNo
                                             )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMDIBuilderCreateParameterVariable( NativeHandle.ThrowIfInvalid()
                                                             , scope?.Handle ?? default
                                                             , name
                                                             , argNo
                                                             , file?.Handle ?? default
                                                             , line
                                                             , type?.Handle ?? default
                                                             , alwaysPreserve
                                                             , ( LLVMDIFlags )debugFlags
                                                             );

            return (DILocalVariable)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DIBasicType CreateBasicType( LazyEncodedString name
                                          , UInt64 bitSize
                                          , DiTypeKind encoding
                                          , DebugInfoFlags diFlags = DebugInfoFlags.None
                                          )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMDIBuilderCreateBasicType(
                            NativeHandle.ThrowIfInvalid(),
                            name,
                            bitSize,
                            ( uint )encoding,
                            ( LLVMDIFlags )diFlags
                            );
            return (DIBasicType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreatePointerType( DIType? pointeeType
                                              , LazyEncodedString? name
                                              , UInt64 bitSize
                                              , UInt32 bitAlign = 0
                                              , uint addressSpace = 0
                                              )
        {
            var handle = LLVMDIBuilderCreatePointerType(
                            NativeHandle.ThrowIfInvalid(),
                            pointeeType?.Handle ?? default,
                            bitSize,
                            bitAlign,
                            addressSpace,
                            name
                            );
            return (DIDerivedType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateQualifiedType( DIType? baseType, QualifiedTypeTag tag )
        {
            var handle = LLVMDIBuilderCreateQualifiedType( NativeHandle.ThrowIfInvalid(), ( uint )tag, baseType?.Handle ?? default );
            return (DIDerivedType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DITypeArray CreateTypeArray( params DIType?[] types ) => CreateTypeArray( (IEnumerable<DIType?>)types );

        /// <inheritdoc/>
        public DITypeArray CreateTypeArray( IEnumerable<DIType?> types )
        {
            var handles = types.Select( t => t?.Handle ?? default ).ToArray( );
            var handle = LLVMDIBuilderGetOrCreateTypeArray( NativeHandle.ThrowIfInvalid(), handles );
            return new DITypeArray( (MDTuple)handle.ThrowIfInvalid().CreateMetadata()! );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, params DIType?[] types )
        {
            return CreateSubroutineType( debugFlags, (IEnumerable<DIType?>)types );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, IEnumerable<DIType?> types )
        {
            ArgumentNullException.ThrowIfNull( types );

            var handles = types.Select( t => t?.Handle ?? default ).ToArray( );
            var handle = LLVMDIBuilderCreateSubroutineType( NativeHandle.ThrowIfInvalid()
                                                          , LLVMMetadataRef.Zero
                                                          , handles
                                                          , ( LLVMDIFlags )debugFlags
                                                          );

            return (DISubroutineType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags )
        {
            return CreateSubroutineType( debugFlags, [] );
        }

        /// <inheritdoc/>
        public DISubroutineType CreateSubroutineType( DebugInfoFlags debugFlags, DIType? returnType, IEnumerable<DIType?> types )
        {
            return CreateSubroutineType( debugFlags, returnType != null ? types.Prepend( returnType ) : types );
        }

        /// <inheritdoc/>
        public DICompositeType CreateStructType( DIScope? scope
                                               , LazyEncodedString name
                                               , DIFile? file
                                               , uint line
                                               , UInt64 bitSize
                                               , UInt32 bitAlign
                                               , DebugInfoFlags debugFlags
                                               , DIType? derivedFrom
                                               , params DINode[] elements
                                               )
        {
            return CreateStructType( scope, name, file, line, bitSize, bitAlign, debugFlags, derivedFrom, (IEnumerable<DINode>)(elements) );
        }

        /// <inheritdoc/>
        public DICompositeType CreateStructType( DIScope? scope
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
                                               )
        {
            ArgumentNullException.ThrowIfNull( elements );
            ArgumentNullException.ThrowIfNull( name );

            var elementHandles = elements.Select( e => e.Handle ).ToArray( );
            var handle = LLVMDIBuilderCreateStructType(
                            NativeHandle.ThrowIfInvalid(),
                            scope?.Handle ?? default,
                            name,
                            file?.Handle ?? default,
                            line,
                            bitSize,
                            bitAlign,
                            ( LLVMDIFlags )debugFlags,
                            derivedFrom?.Handle ?? default,
                            elementHandles,
                            runTimeLang,
                            vTableHolder?.Handle ?? default,
                            uniqueId ?? LazyEncodedString.Empty
                            );

            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateUnionType( DIScope? scope
                                              , LazyEncodedString name
                                              , DIFile? file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , DebugInfoFlags debugFlags
                                              , DINodeArray elements
                                              )
        {
            return CreateUnionType(
                scope,
                name,
                file,
                line,
                bitSize,
                bitAlign,
                debugFlags,
                (IEnumerable<DINode>)elements
                );
        }

        /// <inheritdoc/>
        public DICompositeType CreateUnionType( DIScope? scope
                                              , LazyEncodedString name
                                              , DIFile? file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , DebugInfoFlags debugFlags
                                              , params DINode[] elements
                                              )
        {
            return CreateUnionType( scope, name, file, line, bitSize, bitAlign, debugFlags, (IEnumerable<DINode>)elements );
        }

        /// <inheritdoc/>
        public DICompositeType CreateUnionType( DIScope? scope
                                              , LazyEncodedString name
                                              , DIFile? file
                                              , uint line
                                              , UInt64 bitSize
                                              , UInt32 bitAlign
                                              , DebugInfoFlags debugFlags
                                              , IEnumerable<DINode> elements
                                              , uint runTimeLang = 0
                                              , LazyEncodedString? uniqueId = null
                                              )
        {
            ArgumentNullException.ThrowIfNull( name );
            ArgumentNullException.ThrowIfNull( elements );

            var elementHandles = elements.Select( e => e.Handle ).ToArray( );
            var handle = LLVMDIBuilderCreateUnionType( NativeHandle.ThrowIfInvalid()
                                                     , scope?.Handle ?? default
                                                     , name
                                                     , file?.Handle ?? default
                                                     , line
                                                     , bitSize
                                                     , bitAlign
                                                     , ( LLVMDIFlags )debugFlags
                                                     , elementHandles
                                                     , runTimeLang
                                                     , uniqueId ?? LazyEncodedString.Empty
                                                     );

            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateMemberType( DIScope? scope
                                             , LazyEncodedString name
                                             , DIFile? file
                                             , uint line
                                             , UInt64 bitSize
                                             , UInt32 bitAlign
                                             , UInt64 bitOffset
                                             , DebugInfoFlags debugFlags
                                             , DIType? type
                                             )
        {
            ArgumentNullException.ThrowIfNull( name );

            var handle = LLVMDIBuilderCreateMemberType( NativeHandle.ThrowIfInvalid()
                                                      , scope?.Handle ?? default
                                                      , name
                                                      , file?.Handle ?? default
                                                      , line
                                                      , bitSize
                                                      , bitAlign
                                                      , bitOffset
                                                      , ( LLVMDIFlags )debugFlags
                                                      , type?.Handle ?? default
                                                      );

            return (DIDerivedType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[] subscripts )
        {
            return CreateArrayType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateArrayType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            ArgumentNullException.ThrowIfNull( elementType );
            ArgumentNullException.ThrowIfNull( subscripts );

            var subScriptHandles = subscripts.Select( s => s.Handle ).ToArray( );
            var handle = LLVMDIBuilderCreateArrayType(
                            NativeHandle.ThrowIfInvalid(),
                            bitSize,
                            bitAlign,
                            elementType.Handle,
                            subScriptHandles,
                            (uint)subScriptHandles.Length
                            );
            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, DINodeArray subscripts )
        {
            return CreateVectorType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, params DINode[] subscripts )
        {
            return CreateVectorType( bitSize, bitAlign, elementType, (IEnumerable<DINode>)subscripts );
        }

        /// <inheritdoc/>
        public DICompositeType CreateVectorType( UInt64 bitSize, UInt32 bitAlign, DIType elementType, IEnumerable<DINode> subscripts )
        {
            ArgumentNullException.ThrowIfNull( elementType );
            ArgumentNullException.ThrowIfNull( subscripts );

            var subScriptHandles = subscripts.Select( s => s.Handle ).ToArray( );
            var handle = LLVMDIBuilderCreateVectorType(
                            NativeHandle.ThrowIfInvalid(),
                            bitSize,
                            bitAlign,
                            elementType.Handle,
                            subScriptHandles,
                            (uint)subScriptHandles.Length
                            );
            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIDerivedType CreateTypedef( DIType? type, LazyEncodedString name, DIFile? file, uint line, DINode? context, UInt32 alignInBits )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            var handle = LLVMDIBuilderCreateTypedef( NativeHandle.ThrowIfInvalid()
                                                   , type?.Handle ?? default
                                                   , name
                                                   , file?.Handle ?? default
                                                   , line
                                                   , context?.Handle ?? default
                                                   , alignInBits
                                                   );

            return (DIDerivedType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DISubRange CreateSubRange( long lowerBound, long count )
        {
            var handle = LLVMDIBuilderGetOrCreateSubrange( NativeHandle.ThrowIfInvalid(), lowerBound, count );
            return (DISubRange)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DINodeArray GetOrCreateArray( IEnumerable<DINode> elements )
        {
            var buf = elements.Select( d => d?.Handle ?? default ).ToArray( );
            long actualLen = buf.LongLength;

            var handle = LLVMDIBuilderGetOrCreateArray( NativeHandle.ThrowIfInvalid(), buf ).ThrowIfInvalid();

            // assume wrapped tuple is not null since underlying handle is already checked.
            var tuple = (MDTuple) handle.CreateMetadata()!;
            return new DINodeArray( tuple );
        }

        /// <inheritdoc/>
        public DITypeArray GetOrCreateTypeArray( params IEnumerable<DIType> types )
        {
            var buf = types.Select( t => t?.Handle ?? default ).ToArray( );
            var handle = LLVMDIBuilderGetOrCreateTypeArray( NativeHandle.ThrowIfInvalid(), buf );
            return new DITypeArray( (MDTuple)handle.ThrowIfInvalid().CreateMetadata()! );
        }

        /// <inheritdoc/>
        public DIEnumerator CreateEnumeratorValue( LazyEncodedString name, long value, bool isUnsigned = false )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );
            var handle = LLVMDIBuilderCreateEnumerator( NativeHandle.ThrowIfInvalid(), name, value, isUnsigned );
            return (DIEnumerator)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateEnumerationType( DIScope? scope
                                                    , LazyEncodedString name
                                                    , DIFile? file
                                                    , uint lineNumber
                                                    , UInt64 sizeInBits
                                                    , UInt32 alignInBits
                                                    , IEnumerable<DIEnumerator> elements
                                                    , DIType? underlyingType
                                                    )
        {
            ArgumentNullException.ThrowIfNull( name );

            var elementHandles = elements.Select( e => e.Handle ).ToArray( );
            var handle = LLVMDIBuilderCreateEnumerationType( NativeHandle.ThrowIfInvalid()
                                                           , scope?.Handle ?? default
                                                           , name
                                                           , file?.Handle ?? default
                                                           , lineNumber
                                                           , sizeInBits
                                                           , alignInBits
                                                           , elementHandles
                                                           , underlyingType?.Handle ?? default
                                                           );

            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DIGlobalVariableExpression CreateGlobalVariableExpression( DINode? scope
                                                                        , LazyEncodedString name
                                                                        , LazyEncodedString linkageName
                                                                        , DIFile? file
                                                                        , uint lineNo
                                                                        , DIType? type
                                                                        , bool isLocalToUnit
                                                                        , DIExpression? value
                                                                        , DINode? declaration = null
                                                                        , UInt32 bitAlign = 0
                                                                        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( name );

            if(string.IsNullOrWhiteSpace( linkageName ))
            {
                linkageName = name;
            }

            var handle = LLVMDIBuilderCreateGlobalVariableExpression( NativeHandle.ThrowIfInvalid()
                                                                    , scope?.Handle ?? default
                                                                    , name
                                                                    , linkageName
                                                                    , file?.Handle ?? default
                                                                    , lineNo
                                                                    , type?.Handle ?? default
                                                                    , isLocalToUnit
                                                                    , value?.Handle ?? default
                                                                    , declaration?.Handle ?? default
                                                                    , bitAlign
                                                                    );
            return (DIGlobalVariableExpression)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public void Finish( DISubProgram subProgram )
        {
            ArgumentNullException.ThrowIfNull( subProgram );
            LLVMDIBuilderFinalizeSubprogram( NativeHandle.ThrowIfInvalid(), subProgram.Handle );
        }

        /// <inheritdoc/>
        public void Finish( )
        {
#if HAVE_PER_CONTEXT_ENUMERABLE_METADA
            // TODO: Figure out API to enumerate the metadata owned by a context if it isn't "cached"
            // This was detecting any unresolved nodes and reporting details of them before calling
            // the native API that will simply crash if there are unresolved nodes...
            // That's a bad experience this was trying to avoid. But it is a perf overhead so perhaps
            // could trigger only on a debug build...
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
#endif
            LLVMDIBuilderFinalize( NativeHandle.ThrowIfInvalid() );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, Instruction insertBefore )
        {
            return InsertDeclare( storage, varInfo, CreateExpression(), location, insertBefore );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage
                                        , DILocalVariable varInfo
                                        , DIExpression expression
                                        , DILocation location
                                        , Instruction insertBefore
                                        )
        {
            ArgumentNullException.ThrowIfNull( storage );
            ArgumentNullException.ThrowIfNull( varInfo );
            ArgumentNullException.ThrowIfNull( expression );
            ArgumentNullException.ThrowIfNull( location );
            ArgumentNullException.ThrowIfNull( insertBefore );

            var handle = LLVMDIBuilderInsertDeclareRecordBefore( NativeHandle.ThrowIfInvalid()
                                                               , storage.Handle
                                                               , varInfo.Handle
                                                               , expression.Handle
                                                               , location.Handle
                                                               , insertBefore.Handle
                                                               );

            return new( handle.ThrowIfInvalid() );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DILocation location, BasicBlock insertAtEnd )
        {
            return InsertDeclare( storage, varInfo, CreateExpression(), location, insertAtEnd );
        }

        /// <inheritdoc/>
        public DebugRecord InsertDeclare( Value storage, DILocalVariable varInfo, DIExpression expression, DILocation location, BasicBlock insertAtEnd )
        {
            ArgumentNullException.ThrowIfNull( storage );
            ArgumentNullException.ThrowIfNull( varInfo );
            ArgumentNullException.ThrowIfNull( expression );
            ArgumentNullException.ThrowIfNull( location );
            ArgumentNullException.ThrowIfNull( insertAtEnd );

            // use default equality comparer as either one might be null
            if(!EqualityComparer<DISubProgram>.Default.Equals( varInfo.Scope.SubProgram, location.Scope?.SubProgram ))
            {
                throw new ArgumentException( Resources.Mismatched_scopes_for_location_and_variable );
            }

            var handle = LLVMDIBuilderInsertDeclareRecordAtEnd( NativeHandle.ThrowIfInvalid()
                                                              , storage.Handle
                                                              , varInfo.Handle
                                                              , expression.Handle
                                                              , location.Handle
                                                              , insertAtEnd.BlockHandle
                                                              );

            return new( handle.ThrowIfInvalid() );
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value
                                      , DILocalVariable varInfo
                                      , DILocation location
                                      , Instruction insertBefore
                                      )
        {
            return InsertValue( value, varInfo, null, location, insertBefore );
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public DebugRecord InsertValue( Value value
                                      , DILocalVariable varInfo
                                      , DIExpression? expression
                                      , DILocation location
                                      , Instruction insertBefore
                                      )
        {
            ArgumentNullException.ThrowIfNull( value );
            ArgumentNullException.ThrowIfNull( varInfo );
            ArgumentNullException.ThrowIfNull( location );
            ArgumentNullException.ThrowIfNull( insertBefore );

            var handle = LLVMDIBuilderInsertDbgValueRecordBefore( NativeHandle.ThrowIfInvalid()
                                                                , value.Handle
                                                                , varInfo.Handle
                                                                , expression?.Handle ?? CreateExpression( ).Handle
                                                                , location.Handle
                                                                , insertBefore.Handle
                                                                );

            return new( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public DebugRecord InsertValue( Value value
                                      , DILocalVariable varInfo
                                      , DILocation location
                                      , BasicBlock insertAtEnd
                                      )
        {
            return InsertValue( value, varInfo, null, location, insertAtEnd );
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Interop API requires specific derived type" )]
        public DebugRecord InsertValue( Value value
                                      , DILocalVariable varInfo
                                      , DIExpression? expression
                                      , DILocation location
                                      , BasicBlock insertAtEnd
                                      )
        {
            ArgumentNullException.ThrowIfNull( value );
            ArgumentNullException.ThrowIfNull( varInfo );
            ArgumentNullException.ThrowIfNull( location );
            ArgumentNullException.ThrowIfNull( insertAtEnd );

            if(!location.Scope.Equals( varInfo.Scope ))
            {
                throw new ArgumentException( Resources.Mismatched_scopes );
            }

            if((insertAtEnd.ContainingFunction is null) || !LocationDescribes( location, insertAtEnd.ContainingFunction ))
            {
                throw new ArgumentException( Resources.Location_does_not_describe_the_specified_block_s_containing_function );
            }

            var handle = LLVMDIBuilderInsertDeclareRecordAtEnd( NativeHandle.ThrowIfInvalid()
                                                              , value.Handle
                                                              , varInfo.Handle
                                                              , expression?.Handle ?? CreateExpression( ).Handle
                                                              , location.Handle
                                                              , insertAtEnd.BlockHandle
                                                              );

            return new( handle.ThrowIfInvalid() )!;
        }

        /// <inheritdoc/>
        public DIExpression CreateExpression( params IEnumerable<ExpressionOp> operations )
        {
            UInt64[ ] args = [ .. operations.Cast<UInt64>( ) ];
            var handle = LLVMDIBuilderCreateExpression( NativeHandle.ThrowIfInvalid(), args );
            return (DIExpression)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        public DIExpression CreateConstantValueExpression( UInt64 value )
        {
            LLVMMetadataRef handle = LLVMDIBuilderCreateConstantValueExpression( NativeHandle.ThrowIfInvalid(), value );
            return (DIExpression)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        /// <inheritdoc/>
        [SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Specific type required by interop call" )]
        public DICompositeType CreateReplaceableCompositeType( Tag tag
                                                             , LazyEncodedString name
                                                             , DIScope? scope
                                                             , DIFile? file
                                                             , uint line
                                                             , uint lang = 0
                                                             , UInt64 sizeInBits = 0
                                                             , UInt32 alignBits = 0
                                                             , DebugInfoFlags flags = DebugInfoFlags.None
                                                             , LazyEncodedString? uniqueId = null
                                                             )
        {
            tag.ThrowIfNotDefined();
            ArgumentNullException.ThrowIfNull( name );
            uniqueId ??= string.Empty;

            // TODO: validate that `tag` is really valid for a composite type or document the result if it isn't (as long as LLVM won't crash at least)
            var handle = LLVMDIBuilderCreateReplaceableCompositeType( NativeHandle.ThrowIfInvalid()
                                                                    , ( uint )tag
                                                                    , name
                                                                    , scope?.Handle ?? default
                                                                    , file?.Handle ?? default
                                                                    , line
                                                                    , lang
                                                                    , sizeInBits
                                                                    , alignBits
                                                                    , ( LLVMDIFlags )flags
                                                                    , uniqueId ?? LazyEncodedString.Empty
                                                                    );
            return (DICompositeType)handle.ThrowIfInvalid().CreateMetadata()!;
        }

        LLVMDIBuilderRefAlias IHandleWrapper<LLVMDIBuilderRefAlias>.Handle => NativeHandle;

        internal DIBuilderAlias( LLVMDIBuilderRefAlias nativeHandle, IModule owningModule )
        {
            if(nativeHandle.IsNull)
            {
                throw new ArgumentException( "Invalid handle value", nameof( nativeHandle ) );
            }

            NativeHandle = nativeHandle;
            OwningModule = owningModule;
        }

        private readonly LLVMDIBuilderRefAlias NativeHandle;

        private static void SanityCheck( DISubProgram retVal, bool isDefinition )
        {
            if(isDefinition)
            {
                if(!retVal.IsDistinct)
                {
                    throw new InternalCodeGeneratorException( "Expected a distinct DISubProgram for a definition)" );
                }

                if(retVal.IsUniqued)
                {
                    throw new InternalCodeGeneratorException( "Expected a non-uniqued DISubProgram for a definition)" );
                }

                if(retVal.CompileUnit is null)
                {
                    throw new InternalCodeGeneratorException( "LLVM Requires a CompileUnit for a DISubProgram for a definition" );
                }
            }
            else
            {
                if(retVal.IsDistinct)
                {
                    throw new InternalCodeGeneratorException( "Expected a non-distinct DISubProgram for a declaration)" );
                }

                if(!retVal.IsUniqued)
                {
                    throw new InternalCodeGeneratorException( "Expected a uniqued DISubProgram for a declaration)" );
                }
            }
        }

        private static bool LocationDescribes( DILocation location, Function function )
        {
            return (location.Scope.SubProgram?.Describes( function ) ?? false)
                   || (location.InlinedAtScope?.SubProgram?.Describes( function ) ?? false);
        }
    }
}
